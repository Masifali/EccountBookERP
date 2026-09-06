using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.AssetSchema;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
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
namespace Architecture.WinApp.ReturnableInventoryManagement
{

public class DeliveryOrder_ReturnAble : Form
{
	private int RecId;

	private string ScreenName = "";

	private bool IsApproved;

	private int updateDetailIndex = -1;

	private bool FormInitialized;

	private bool ItemSearchWithNameOrCode = true;

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool SubsidiaryAccountAllowOnVouchers;

	private DataTable dtWh = new DataTable();

	private DataTable table = new DataTable();

	private DataTable dtitem = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dthistoryfill = new DataTable();

	private DataTable dtAssetItem = new DataTable();

	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

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

	private DateTimePicker DocDate;

	private TextBox txtdocno;

	private Label label5;

	private GroupBox groupBox3;

	private Button btnplus;

	private TextBox txtqty;

	private Label label38;

	private Label label36;

	private Label label37;

	private Panel DetailGridPanel;

	private Panel panel7;

	private Panel panel9;

	internal UltraCombo CmbItemName;

	internal UltraCombo CmbPackUom;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	private Button btnUpdateDetail;

	private Button btnCancelUpdateDetial;

	private Panel panel12;

	internal GridEX grdhistory;

	private ToolStripButton btnRefresh;

	private ToolStrip toolStrip2;

	private ToolStripButton btnLoadAll;

	internal UltraCombo CmbSupplierCustomer;

	private Label label2;

	internal UltraCombo CmbWareHouse;

	private Label labelWarFrom;

	private CheckBox ChkPrint;

	private TextBox txtSubNoId;

	private TextBox txtDocNoId;

	private TextBox txtRefDocTypeId;

	private Label lblBalance;

	private TextBox txtVehicleNo;

	private Label label12;

	private ToolStripButton btnSaveAs;

	internal UltraCombo cmbvehicletype;

	private Label label14;

	private ToolStripButton btnDelete;

	private GroupBox groupBox1;

	private ToolStripSeparator toolStripSeparator2;

	internal GridEX grd;

	private Panel panel5;

	private Label label15;

	private Label label16;

	internal GridEX GrdHistoryDetail;

	private Label label27;

	internal UltraCombo CmbApprovedBy;

	private Label label6;

	internal UltraCombo CmbRequestedBy;

	private Label label1;

	internal UltraCombo CmbDeliveryType;

	private Label label11;

	private Label label18;

	private TextBox txtremarks;

	private Button RequestedApprovedLookup;

	private Panel panel8;

	private Panel panel16;

	private GroupBox groupBox2;

	private Label label24;

	internal UltraCombo CmbCustomerHistory;

	private Button btnShowHistory;

	private Label label7;

	private Label label13;

	private Label label20;

	internal UltraCombo CmbDeliveryTypeHistory;

	private TextBox txtToDocNoHistory;

	private TextBox txtFromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Label label22;

	internal UltraCombo CmbApprovedByHistory;

	private Label label21;

	internal UltraCombo CmbRequestedByHistory;

	private Panel panel6;

	private Panel panel10;

	private Panel panel13;

	private Panel panel11;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnRefreshHistory;

	private TextBox txtItemDescriptions;

	private Label label52;

	private RadioButton rdbtnItemCode;

	private RadioButton rdbtnItemName;

	private Label label19;

	private Label label10;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	internal UltraCombo CmbReferenceParty;

	private Label label3;

	internal UltraCombo CmbItemCategory;

	private Label label8;

	private Button button1;

	private Label label28;

	private DateTimePicker datReturnableDate;

	public DeliveryOrder_ReturnAble(UserAccount user)
	{
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Expected O, but got Unknown
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
			if (dtWh.Columns.Count == 0)
			{
				dtWh.Columns.Add("Id");
				dtWh.Columns.Add("WareHouseName");
			}
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("CompanyName", typeof(string));
			dtSupplier.Columns.Add("PartyCode", typeof(string));
			dtSupplier.Columns.Add("GlAccountId", typeof(int));
			dtSupplier.Columns.Add("CityId", typeof(int));
			dtSupplier.Columns.Add("CityName", typeof(string));
			dtSupplier.Columns.Add("MobileNo", typeof(string));
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName");
			dtitem.Columns.Add("ItemCode");
			table.Columns.Add("Id", typeof(int));
			table.Columns.Add("WareHouseId", typeof(int));
			table.Columns.Add("ItemCategoryId", typeof(int));
			table.Columns.Add("ItemCategory", typeof(string));
			table.Columns.Add("ItemId", typeof(int));
			table.Columns.Add("ItemCode", typeof(string));
			table.Columns.Add("ItemName", typeof(string));
			table.Columns.Add("PackUomId", typeof(int));
			table.Columns.Add("PackUom");
			table.Columns.Add("ItemDescription", typeof(string));
			table.Columns.Add("ItemQty", typeof(double));
			table.Columns.Add("IsAssetItem", typeof(bool));
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
			SubsidiaryAccountAllowOnVouchers = CommonServices.GetERPFeatureById(4);
			string documentNoData = DocumentNoDbCall();
			DataTable refPartyData = RefPartydtDbCall();
			DataTable historyComboData = HistoryComboDbCall();
			DataTable itemCategoryData = ItemCategoryDbCall();
			DataTable deliveryTypeForReturnAbleData = CommonServices.StaticColumnsService("DeliveryTypeForReturnAble");
			return new
			{
				RefPartyData = refPartyData,
				DocumentNoData = documentNoData,
				HistoryComboData = historyComboData,
				ItemCategoryData = itemCategoryData,
				DeliveryTypeForReturnAbleData = deliveryTypeForReturnAbleData
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
				btnsave.Enabled = formright.DoHaveSaveRight;
				btnSaveAs.Enabled = formright.DoHaveSaveRight;
				btnupdate.Enabled = formright.DoHaveUpdateRights;
				btnprint.Enabled = formright.DoHavePrintRights;
				FormHelper.SetCtrlGrdBarRight(ctrlGrdBar1, formright);
				FormHelper.SetCtrlGrdBarRight(ctrlGrdBar2, formright);
				ItemSearchWithNameOrCode = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ItemSearchByCode"));
				DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"));
				if (ItemSearchWithNameOrCode)
				{
					rdbtnItemCode.Checked = true;
				}
				else
				{
					rdbtnItemName.Checked = true;
				}
				if (RecId == 0)
				{
					txtdocno.Text = result.DocumentNoData;
				}
				DeliveryTypeFill(result.DeliveryTypeForReturnAbleData);
				vehicleTypefill();
				SupplierDtFillFromGlobal();
				BindSupplierName();
				ReferencePartiesBind(result.RefPartyData);
				ItemCategoryBind(result.ItemCategoryData);
				RequestedByAndApprovedByFill();
				WarehousesDtFillFromGlobal();
				BindWareHouse();
				ItemDtsFillFromGlobal();
				ItemNameBind();
				GetAssetItems();
				DataTable dataTable = table;
				if (dataTable != null && dataTable.Columns.Count > 0)
				{
					grd.DataSource = table;
					grd.RetrieveStructure();
					grdSettings();
				}
				HistoryComboBind(result.HistoryComboData);
				FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
				ToDateHistory.Value = DateTime.Now;
				FormInitialized = true;
				DocDate.Focus();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void PurchsaeOrder_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 600);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable HistoryComboDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		try
		{
			return InvDeliveryOrder.GetDataForDropDownFromDeliveryOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeIds = "1200"
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void HistoryComboBind(DataTable dtcombo)
	{
		try
		{
			if (dtcombo.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtcust = new DataTable();
			dtcust.Columns.Add("Id", typeof(int));
			dtcust.Columns.Add("CustomerName", typeof(string));
			DataTable dtDeliverytype = new DataTable();
			dtDeliverytype.Columns.Add("Id", typeof(int));
			dtDeliverytype.Columns.Add("DeliveryType", typeof(string));
			DataTable dtRequestedBy = new DataTable();
			dtRequestedBy.Columns.Add("Id", typeof(int));
			dtRequestedBy.Columns.Add("name", typeof(string));
			DataTable dtApprovedBy = new DataTable();
			dtApprovedBy.Columns.Add("Id", typeof(int));
			dtApprovedBy.Columns.Add("name", typeof(string));
			for (int i = 0; i < dtcombo.Rows.Count; i++)
			{
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "Customer")
				{
					dtcust.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "DeliveryTypeDetail")
				{
					dtDeliverytype.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "RequestedBy")
				{
					dtRequestedBy.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "ApprovedBy")
				{
					dtApprovedBy.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbCustomerHistory, dtcust, "Id", "CustomerName", "Party Name", AllColumns: true, Conversion.ToInt(CmbCustomerHistory.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbDeliveryTypeHistory, dtDeliverytype, "Id", "DeliveryType", "Delivery Type", AllColumns: true, Conversion.ToInt(CmbDeliveryType.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			((UltraGridBase)CmbDeliveryTypeHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			InfragisticsHelper.BindAndRetainSelection(CmbRequestedByHistory, dtRequestedBy, "Id", "name", "Requested By", AllColumns: true, Conversion.ToInt(CmbRequestedByHistory.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbApprovedByHistory, dtApprovedBy, "Id", "name", "Approved By", AllColumns: true, Conversion.ToInt(CmbApprovedByHistory.Value), insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private string DocumentNoDbCall()
	{
		try
		{
			return CommonServices.DeliveryOrderGenerateCode(1200).ToString();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DeliveryTypeFill(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbDeliveryType, dt, "Id", "type", "Delivery Type", AllColumns: false, Conversion.ToInt(CmbDeliveryType.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)CmbDeliveryType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierDtFillFromGlobal()
	{
		try
		{
			dtSupplier.Clear();
			List<getGlobalAllSupplierCustomer> customers = clsGlobalVariables.globalAllSupplierCustomer;
			if (customers == null || customers.Count == 0)
			{
				return;
			}
			foreach (getGlobalAllSupplierCustomer item in customers)
			{
				object[] rowData = new object[7] { item.Id, item.CompanyName, item.PartyCode, item.GlAccountId, item.CityId, item.CityName, item.MobilePersonal };
				dtSupplier.Rows.Add(rowData);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindSupplierName()
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbSupplierCustomer, dtSupplier, "Id", "CompanyName", "Party Name", AllColumns: true, Conversion.ToInt(CmbSupplierCustomer.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dtSupplier.Rows.Count > 0)
			{
				((UltraGridBase)CmbSupplierCustomer).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbSupplierCustomer).DisplayLayout.Bands[0].Columns[4].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable RefPartydtDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return ReferenceParties.GetAll(new ReferenceParties
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

	private void ReferencePartiesBind(DataTable dtRefParty)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbReferenceParty, dtRefParty, "Id", "ReferencePartyName", "Reference Party Name", AllColumns: false, Conversion.ToInt(CmbReferenceParty.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void vehicleTypefill()
	{
		try
		{
			DataTable dt = VehicleType.GetAll();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbvehicletype, "Id", "VehicleDescription", "Vehicle Type", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WarehousesDtFillFromGlobal()
	{
		try
		{
			dtWh.Clear();
			List<GlobalWarehousesWithBranch> lst = clsGlobalVariables.globalWarehousesWithBranches.Where((GlobalWarehousesWithBranch r) => r.IsActive).ToList();
			if (lst != null && lst.Count > 0)
			{
				for (int i = 0; i < lst.Count; i++)
				{
					dtWh.Rows.Add(lst[i].WarehouseId, lst[i].WarehouseName);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindWareHouse()
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbWareHouse, dtWh, "Id", "WareHouseName", "Warehouse", AllColumns: true, Conversion.ToInt(CmbWareHouse.Value), insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RequestedByAndApprovedByFill()
	{
		try
		{
			int ReqId = Conversion.ToInt(CmbRequestedBy.Value);
			int AppId = Conversion.ToInt(CmbApprovedBy.Value);
			DataTable dt = CommonServices.GetLookupsByTypeIdDt(13);
			DataTable dtType = new DataTable();
			dtType.Columns.Add("Id", typeof(int));
			dtType.Columns.Add("Type");
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtType.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["LookupName"]);
				}
			}
			DDL.BindDDL(dtType, CmbRequestedBy, "Id", "Type", "Requested By", ZeroIndex: true);
			DDL.BindDDL(dtType, CmbApprovedBy, "Id", "Type", "Approved By", ZeroIndex: true);
			if (ReqId > 0)
			{
				if (dtType.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ReqId))
				{
					CmbRequestedBy.Value = ReqId;
				}
				else
				{
					((Control)(object)CmbRequestedBy).Text = string.Empty;
				}
			}
			if (AppId > 0)
			{
				if (dtType.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == AppId))
				{
					CmbApprovedBy.Value = AppId;
				}
				else
				{
					((Control)(object)CmbApprovedBy).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable ItemCategoryDbCall()
	{
		try
		{
			return CommonServices.GetItemCategoryForComboServiceBind("5,8");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ItemCategoryBind(DataTable dt)
	{
		try
		{
			DataTable dtCategory = new DataTable();
			dtCategory.Columns.Add("Id", typeof(int));
			dtCategory.Columns.Add("CategoryDescription", typeof(string));
			dtCategory.Columns.Add("ParentCategoryId", typeof(int));
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				dtCategory.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["CategoryDescription"], dt.Rows[i]["InventoryParentCategoriesId"]);
			}
			InfragisticsHelper.BindAndRetainSelection(CmbItemCategory, dtCategory, "Id", "CategoryDescription", "Item Category", AllColumns: true, Conversion.ToInt(CmbItemCategory.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			((UltraGridBase)CmbItemCategory).DisplayLayout.Bands[0].Columns[2].Hidden = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDtsFillFromGlobal(int CategoryId = 0)
	{
		try
		{
			dtitem.Clear();
			List<getGlobalAllItems> items = clsGlobalVariables.getGlobalAllItems.Where((getGlobalAllItems r) => (r.InventoryParentCategoriesId == 7 || r.InventoryParentCategoriesId == 8) && CategoryId == r.ItemCategoryId).ToList();
			if (items == null || items.Count == 0)
			{
				return;
			}
			foreach (getGlobalAllItems row in items)
			{
				dtitem.Rows.Add(row.Id, row.ItemName, row.ItemCode);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error loading items: " + ex.Message);
		}
	}

	private void GetAssetItems()
	{
		try
		{
			dtAssetItem = AssetRegister.GetAllAssetFromAssetRegister(UserAccount.OrganizationId, UserAccount.CompanyId, 0);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ItemNameBind()
	{
		try
		{
			int Id = Conversion.ToInt(CmbItemName.Value);
			string ValueMember = (rdbtnItemName.Checked ? "ItemName" : "ItemCode");
			string DisplayMember = (rdbtnItemName.Checked ? "Item Name" : "Item Code");
			InfragisticsHelper.BindAndRetainSelection(CmbItemName, dtitem, "Id", ValueMember, DisplayMember, AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void bindRateUomAndItemPackUom(int ID)
	{
		try
		{
			string PackUom = ((Control)(object)CmbPackUom).Text.Trim();
			DataTable dtuomsch = CommonServices.dtUomFromGloablUomScheduleByItemId(ID);
			InfragisticsHelper.BindAndRetainSelection(CmbPackUom, dtuomsch, "Id", "UOMCode", "Uom", AllColumns: true, PackUom, insertDefaultRow: true, null, null, ActivateRow: true, 0, "UOMCode");
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
			if (((UltraDropDownBase)CmbItemCategory).SelectedRow == null)
			{
				return;
			}
			if (((((UltraDropDownBase)CmbItemCategory).SelectedRow != null) ? Conversion.ToInt(((UltraDropDownBase)CmbItemCategory).SelectedRow.Cells["ParentCategoryId"].Value) : 0) != 5)
			{
				if (Conversion.ToInt(CmbItemName.Value) > 0)
				{
					bindRateUomAndItemPackUom(Conversion.ToInt(CmbItemName.Value));
					return;
				}
				((Control)(object)CmbPackUom).Text = string.Empty;
				((UltraGridBase)CmbPackUom).DataSource = null;
				return;
			}
			long ItemId = Conversion.ToInt(CmbItemName.Value);
			string PackUom = ((Control)(object)CmbPackUom).Text.Trim();
			DataRow obj = dtAssetItem.AsEnumerable().FirstOrDefault((DataRow r) => r.Field<long>("assetRegisterId") == ItemId);
			if (obj != null)
			{
				DataTable dtUom = new DataTable();
				dtUom.Columns.Add("Id", typeof(int));
				dtUom.Columns.Add("UOMCode", typeof(string));
				dtUom.Columns.Add("Equivalent", typeof(double));
				dtUom.Clear();
				dtUom.Rows.Add(Conversion.ToInt(obj["UomId"]), Conversion.ToString(obj["UOMCode"]), Conversion.ToDouble(obj["Equivalent"]));
				InfragisticsHelper.BindAndRetainSelection(CmbPackUom, dtUom, "Id", "UOMCode", "Uom", AllColumns: true, PackUom, insertDefaultRow: true, null, null, ActivateRow: true, 0, "UOMCode");
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
		if (Conversion.ToInt(CmbDeliveryType.Value) <= 0)
		{
			MessageBox.Show("Please Select Delivery Type field First");
			CmbDeliveryType.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbSupplierCustomer.Value) <= 0)
		{
			MessageBox.Show("Please Select Customer Field ");
			CmbSupplierCustomer.Focus();
			return false;
		}
		if (((UltraGridBase)cmbvehicletype).ActiveRow == null)
		{
			MessageBox.Show("Vehicle Type Field is Required");
			cmbvehicletype.Focus();
			return false;
		}
		if (txtVehicleNo.Text == string.Empty)
		{
			MessageBox.Show("Vehicle No Field is Required");
			txtVehicleNo.Focus();
			return false;
		}
		if (txtVehicleNo.Text == string.Empty)
		{
			MessageBox.Show("Vehicle No Field is Required");
			txtVehicleNo.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbDeliveryType.Value) == 4 && DocDate.Value.Date > datReturnableDate.Value.Date)
		{
			MessageBox.Show("Returnable date can not less than doc date");
			datReturnableDate.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbRequestedBy.Value) <= 0)
		{
			MessageBox.Show("Requested By Field is Required");
			CmbRequestedBy.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbApprovedBy.Value) <= 0)
		{
			MessageBox.Show("Approved By Field is Required");
			CmbApprovedBy.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)CmbWareHouse).ActiveRow == null || Conversion.ToInt(CmbWareHouse.Value) == 0)
		{
			MessageBox.Show("WareHouseName field is required");
			CmbWareHouse.Focus();
			return false;
		}
		if (((UltraGridBase)CmbItemCategory).ActiveRow == null || Conversion.ToInt(CmbItemCategory.Value) == 0)
		{
			MessageBox.Show("Item Category field is required");
			CmbItemCategory.Focus();
			return false;
		}
		if (((UltraGridBase)CmbItemName).ActiveRow == null || Conversion.ToInt(CmbItemName.Value) == 0)
		{
			MessageBox.Show("ItemName field is required");
			CmbItemName.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackUom).ActiveRow == null || Conversion.ToInt(CmbPackUom.Value) == 0)
		{
			MessageBox.Show("PackUOM field is required");
			CmbPackUom.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtqty.Text) == 0.0)
		{
			MessageBox.Show("ItemQty field is required");
			txtqty.Focus();
			return false;
		}
		return true;
	}

	private void btnplus_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				table.Rows.Add(0, CmbWareHouse.Value, CmbItemCategory.Value, ((Control)(object)CmbItemCategory).Text, CmbItemName.Value, Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemCode"].Value), Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemName"].Value), CmbPackUom.Value, ((Control)(object)CmbPackUom).Text.Trim(), Conversion.ToString((object)txtItemDescriptions.Text), Conversion.ToDouble((object)txtqty.Text.Trim()), Conversion.ToInt(((UltraDropDownBase)CmbItemCategory).SelectedRow.Cells["ParentCategoryId"].Value) == 5);
				grd.DataSource = table;
				grd.RetrieveStructure();
				grdSettings();
				ResetDetail();
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
			grd.RootTable.Columns["WareHouseId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["WareHouseId"].HasValueList = true;
			grd.RootTable.Columns["WareHouseId"].LimitToList = true;
			grd.RootTable.Columns["WareHouseId"].Caption = "WareHouse Name";
			grd.RootTable.Columns["WareHouseId"].ValueList.PopulateValueList((IEnumerable)dtWh.DefaultView, "Id", "WareHouseName");
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
			List<string> hiddenColumns = new List<string> { "Id", "ItemCategoryId", "ItemId", "PackUomId", "IsAssetItem" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType> { 
			{
				"WareHouseId",
				(EditType)4
			} };
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 2, 3, hiddenColumns, editableColumns);
			grd.RootTable.Columns["WareHouseId"].Width = Constants.InventoryConstants.WareHouseName;
			grd.RootTable.Columns["ItemCategory"].Width = Constants.InventoryConstants.CategoryDescription;
			grd.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			grd.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grd.RootTable.Columns["ItemDescription"].Width = Constants.InventoryConstants.CategoryDescription + 50;
			grd.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["ItemQty"].Caption = "Qty";
			grd.RootTable.Columns["PackUom"].Caption = "Uom";
			grd.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			grd.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			GridComboBind();
			grd.RootTable.Columns.Add("Add");
			grd.RootTable.Columns["Add"].Key = "Add";
			grd.RootTable.Columns["Add"].Caption = "+";
			grd.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Add"].ButtonText = "+";
			grd.RootTable.Columns["Add"].Width = 20;
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "X";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
			grd.RootTable.Columns["Delete"].Width = 20;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Expected O, but got Unknown
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Add")
			{
				GridEXRow item = grd.CurrentRow;
				if (item != null)
				{
					DataRow dr = table.Rows[item.RowIndex];
					table.Rows.Add(dr.ItemArray);
					if (btnupdate.Visible && btnupdate.Enabled)
					{
						table.Rows[table.Rows.Count - 1]["Id"] = 0;
					}
					grd.DataSource = table;
				}
			}
			if (!(e.Column.Key == "Delete"))
			{
				return;
			}
			GridEXRow r = grd.CurrentRow;
			if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					InvDeliveryOrderdetail vd = new InvDeliveryOrderdetail();
					vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					vd.WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
					vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					vd.PackUomId = Conversion.ToInt(r.Cells["PackUomId"].Value);
					vd.DoQty = Conversion.ToDouble(r.Cells["ItemQty"].Value);
					vd.LoadingQty = Conversion.ToDouble(r.Cells["ItemQty"].Value);
					vd.DoWeight = Conversion.ToDouble(r.Cells["ItemQty"].Value);
					vd.LoadingWeight = Conversion.ToDouble(r.Cells["ItemQty"].Value);
					vd.IsAssetItem = Conversion.ToBool(r.Cells["IsAseetItem"].Value);
					vd.ItemDiscription = Conversion.ToString(r.Cells["ItemDescription"].Value);
					vd.ActionTypeId = 3;
					lstRemoveRecord.Add(vd);
					table.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					grd.UpdateData();
				}
			}
			else if (r != null && (int)r.RowType == 0)
			{
				r.Delete();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			CommonServices.GridAutoAdjustmentNew(grd);
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		ResetDetail();
		txtRefDocTypeId.Text = string.Empty;
		txtDocNoId.Text = string.Empty;
		txtSubNoId.Text = string.Empty;
		btnplus.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelUpdateDetial.Visible = false;
	}

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				table.Rows[updateDetailIndex]["WareHouseId"] = CmbWareHouse.Value;
				table.Rows[updateDetailIndex]["ItemCategoryId"] = CmbItemCategory.Value;
				table.Rows[updateDetailIndex]["ItemCategory"] = ((Control)(object)CmbItemCategory).Text;
				table.Rows[updateDetailIndex]["ItemId"] = CmbItemName.Value;
				table.Rows[updateDetailIndex]["ItemCode"] = Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemCode"].Value);
				table.Rows[updateDetailIndex]["ItemName"] = Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemName"].Value);
				table.Rows[updateDetailIndex]["ItemDescription"] = Conversion.ToString((object)txtItemDescriptions.Text);
				table.Rows[updateDetailIndex]["PackUomId"] = CmbPackUom.Value;
				table.Rows[updateDetailIndex]["PackUom"] = ((Control)(object)CmbPackUom).Text;
				table.Rows[updateDetailIndex]["ItemQty"] = txtqty.Text;
				table.Rows[updateDetailIndex]["IsAssetItem"] = Conversion.ToInt(((UltraDropDownBase)CmbItemCategory).SelectedRow.Cells["ParentCategoryId"].Value) == 5;
				grd.DataSource = table;
				btnplus.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				ResetDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				CmbWareHouse.Value = item.Cells["WareHouseId"].Value.ToString();
				CmbItemCategory.Value = item.Cells["ItemCategoryId"].Value.ToString();
				CmbItemCategory_Leave(null, null);
				CmbItemName.Value = Conversion.ToInt(item.Cells["ItemId"].Value);
				CmbItemName_Leave(null, null);
				txtItemDescriptions.Text = item.Cells["ItemDescription"].Value.ToString();
				CmbPackUom.Value = item.Cells["PackUomId"].Value.ToString();
				txtqty.Text = item.Cells["ItemQty"].Value.ToString();
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

	private void Insert()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Expected O, but got Unknown
		try
		{
			if (grd.GetRows().Count() == 0)
			{
				throw new Exception("Grid Record Not Found");
			}
			if (!FormValidation())
			{
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
				po.IsApproved = IsApproved;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			double DoQty = 0.0;
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				DoQty += Conversion.ToDouble(r.Cells["ItemQty"].Value);
			}
			po.DocumentTypeId = 1200;
			po.DeliveryOrderType = "Local";
			po.DocDate = DocDate.Value;
			po.DocNo = Conversion.ToInt((object)txtdocno.Text);
			po.VehicleType = Conversion.ToString((object)((Control)(object)cmbvehicletype).Text);
			po.VehicleNo = Conversion.ToString((object)txtVehicleNo.Text.Trim());
			po.RequestedByLookUpId = Conversion.ToInt(CmbRequestedBy.Value);
			po.ApprovedByLookUpId = Conversion.ToInt(CmbApprovedBy.Value);
			if (Conversion.ToInt(CmbDeliveryType.Value) == 4)
			{
				po.ReturnableDate = Conversion.ToDateTime((object)datReturnableDate.Value);
			}
			po.LoadingInstructions = Conversion.ToString((object)txtremarks.Text);
			po.DoTotalQty = Conversion.ToDecimal((object)DoQty);
			po.GrossWeight = DoQty;
			po.NetWeight = DoQty;
			po.BranchesId = UserAccount.BranchesId;
			po.OrganizationId = UserAccount.OrganizationId;
			po.CompanyId = UserAccount.CompanyId;
			po.EntryUser = UserAccount.ID;
			po.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			po.ScreenName = Conversion.ToString((object)ScreenName);
			po.InvDeliveryOrderDetailList = new List<InvDeliveryOrderdetail>();
			po.InvDeliveryOrderExpenseslist = new List<InvDeliveryOrderExpense>();
			if (RecId > 0 && lstRemoveRecord.Count > 0)
			{
				for (int i = 0; i < lstRemoveRecord.Count; i++)
				{
					po.InvDeliveryOrderDetailList.Add(lstRemoveRecord[i]);
				}
			}
			rows = grd.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				InvDeliveryOrderdetail vd = new InvDeliveryOrderdetail();
				vd.Id = ((RecId != 0) ? Conversion.ToInt(r2.Cells["Id"].Value) : 0);
				vd.ActionTypeId = ((vd.Id <= 0) ? 1 : 2);
				vd.DeliveryTypeId = Conversion.ToInt(CmbDeliveryType.Value);
				vd.SupplierCustomerId = Conversion.ToInt(CmbSupplierCustomer.Value);
				vd.RefPartyId = Conversion.ToInt(CmbReferenceParty.Value);
				if (vd.SupplierCustomerId <= 0)
				{
					throw new Exception("Customer Field Required in Row# " + (r2.RowIndex + 1));
				}
				vd.WarehouseId = Conversion.ToInt(r2.Cells["WareHouseId"].Value);
				if (vd.WarehouseId <= 0)
				{
					throw new Exception("WareHouseName Field Required in Row#" + (r2.RowIndex + 1));
				}
				vd.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
				if (Conversion.ToInt(r2.Cells["ItemId"].Value) <= 0)
				{
					throw new Exception("Item Name Field Required in Row#" + (r2.RowIndex + 1));
				}
				vd.ItemDiscription = Conversion.ToString(r2.Cells["ItemDescription"].Value);
				vd.PackUomId = Conversion.ToInt(r2.Cells["PackUomId"].Value);
				if (Conversion.ToInt(r2.Cells["PackUomId"].Value) <= 0)
				{
					throw new Exception("PackUOM Field Required in Row#" + (r2.RowIndex + 1));
				}
				vd.DoQty = Conversion.ToDouble(r2.Cells["ItemQty"].Value);
				if (!(Conversion.ToDouble(r2.Cells["ItemQty"].Value) > 0.0))
				{
					throw new Exception("ItemQty Field Required in Row#" + (r2.RowIndex + 1));
				}
				vd.IsAssetItem = Conversion.ToBool(r2.Cells["IsAssetItem"].Value);
				vd.LoadingQty = vd.DoQty;
				vd.DoWeight = vd.DoQty;
				vd.LoadingWeight = vd.DoQty;
				vd.LoadingRemarks = vd.ItemDiscription;
				po.InvDeliveryOrderDetailList.Add(vd);
			}
			po.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			po.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<InvDeliveryOrder>(po, ScreenName, AT, "AttachmentsList", "DeleteAttachmentsList");
			int success = InvDeliveryOrder.Save(po);
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + po.DocNo);
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, po.DocumentTypeId, ScreenName);
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + po.DocNo);
			}
			Reset();
			if (ChkPrint.Checked)
			{
				CommonServices.InvDeliveryOrderSlip_1200(success, 1200);
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
			RecId = 0;
			Insert();
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
			RecId = 0;
			if (grd.RowCount > 0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					obj.Cells["Id"].Value = 0;
					obj.EndEdit();
				}
			}
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
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		try
		{
			RecId = ID;
			InvDeliveryOrder po = new InvDeliveryOrder();
			po = InvDeliveryOrder.GetByID(ID);
			tabControl1.SelectedIndex = 0;
			txtdocno.Text = Conversion.ToString((object)po.DocNo);
			DocDate.Value = po.DocDate;
			if (po.InvDeliveryOrderDetailList != null && po.InvDeliveryOrderDetailList.Count > 0)
			{
				CmbDeliveryType.Value = po.InvDeliveryOrderDetailList[0].DeliveryTypeId;
				CmbDeliveryType_Leave(null, null);
				CmbSupplierCustomer.Value = po.InvDeliveryOrderDetailList[0].SupplierCustomerId;
				CmbSupplierCustomer_Leave(null, null);
				CmbReferenceParty.Value = po.InvDeliveryOrderDetailList[0].RefPartyId;
			}
			((Control)(object)cmbvehicletype).Text = Conversion.ToString((object)po.VehicleType);
			txtVehicleNo.Text = po.VehicleNo;
			CmbRequestedBy.Value = po.RequestedByLookUpId;
			CmbApprovedBy.Value = po.ApprovedByLookUpId;
			txtremarks.Text = Conversion.ToString((object)po.LoadingInstructions);
			IsApproved = po.IsApproved;
			table.Clear();
			for (int i = 0; i < po.InvDeliveryOrderDetailList.Count; i++)
			{
				table.Rows.Add(po.InvDeliveryOrderDetailList[i].Id, po.InvDeliveryOrderDetailList[i].WarehouseId, po.InvDeliveryOrderDetailList[i].ItemCategoryId, po.InvDeliveryOrderDetailList[i].ItemCategory, po.InvDeliveryOrderDetailList[i].ItemId, po.InvDeliveryOrderDetailList[i].ItemCode, po.InvDeliveryOrderDetailList[i].ItemName, po.InvDeliveryOrderDetailList[i].PackUomId, po.InvDeliveryOrderDetailList[i].PackUOM, po.InvDeliveryOrderDetailList[i].ItemDiscription, po.InvDeliveryOrderDetailList[i].DoQty, po.InvDeliveryOrderDetailList[i].IsAssetItem);
			}
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			btnsave.Visible = false;
			btnupdate.Visible = true;
			btnSaveAs.Visible = true;
			DocDate.Focus();
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "UomSchedule", "SupplierCustomerLists", "WarehousesWithBranches", "AllItems" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			DeliveryTypeFill(CommonServices.StaticColumnsService("DeliveryTypeForReturnAble"));
			vehicleTypefill();
			SupplierDtFillFromGlobal();
			BindSupplierName();
			ReferencePartiesBind(RefPartydtDbCall());
			WarehousesDtFillFromGlobal();
			BindWareHouse();
			ItemCategoryBind(ItemCategoryDbCall());
			ItemDtsFillFromGlobal();
			GetAssetItems();
			CmbItemCategory_Leave(null, null);
			ItemNameBind();
			RequestedByAndApprovedByFill();
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
			lstRemoveRecord.Clear();
			RecId = 0;
			((Control)(object)CmbDeliveryType).Enabled = true;
			((Control)(object)CmbSupplierCustomer).Enabled = true;
			table.Rows.Clear();
			grd.DataSource = table;
			((Control)(object)CmbDeliveryType).Text = string.Empty;
			((UltraGridBase)CmbDeliveryType).Rows[0].Activate();
			CmbDeliveryType_Leave(null, null);
			((Control)(object)CmbSupplierCustomer).Text = string.Empty;
			txtVehicleNo.Text = string.Empty;
			((Control)(object)cmbvehicletype).Text = string.Empty;
			CmbRequestedBy.Value = 0;
			CmbApprovedBy.Value = 0;
			txtremarks.Text = string.Empty;
			IsApproved = false;
			((Control)(object)CmbItemName).Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			txtqty.Text = string.Empty;
			txtRefDocTypeId.Text = string.Empty;
			txtDocNoId.Text = string.Empty;
			txtSubNoId.Text = string.Empty;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			btnSaveAs.Visible = false;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			((Control)(object)CmbReferenceParty).Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			((UltraGridBase)CmbPackUom).DataSource = null;
			txtdocno.Text = DocumentNoDbCall();
			table.Rows.Clear();
			grd.DataSource = table;
			DocDate.Focus();
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
			((Control)(object)CmbItemName).Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			txtqty.Text = string.Empty;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			CmbItemName.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeliveryOrder_Engr_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnplus_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "rdbtnItemName")
			{
				CmbItemName.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShowHistory_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "grdhistory")
			{
				FromDateHistory.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "rdbtnItemName")
			{
				CmbItemName.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PurchsaeOrder_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Return)
		{
			SendKeys.Send("{TAB}");
		}
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
		if (e.Control && e.KeyCode == Keys.N)
		{
			if (tabControl1.SelectedIndex == 0)
			{
				btnnew_Click(null, null);
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				btnNewHistory_Click(null, null);
			}
		}
		if (e.Control && e.KeyCode == Keys.R)
		{
			if (tabControl1.SelectedIndex == 0)
			{
				btnRefresh_Click(null, null);
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				btnRefreshHistory_Click(null, null);
			}
		}
		if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
		{
			DocDate.Focus();
		}
		if (e.Control && e.KeyCode == Keys.S)
		{
			if (tabControl1.SelectedIndex == 0)
			{
				if (btnsave.Visible && btnsave.Enabled)
				{
					btnsave_Click(null, null);
				}
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				btnShowHistory_Click(null, null);
			}
		}
		if (e.Control && e.KeyCode == Keys.U && btnupdate.Visible && btnupdate.Enabled)
		{
			btnupdate_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.F5)
		{
			DocDate.Focus();
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
			if (tabControl1.SelectedIndex == 0)
			{
				((Control)(object)grd).Focus();
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				((Control)(object)grdhistory).Focus();
			}
		}
		if (e.Control && e.KeyCode == Keys.Up)
		{
			if (tabControl1.SelectedIndex == 0)
			{
				CmbWareHouse.Focus();
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				FromDateHistory.Focus();
			}
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
		if (e.KeyCode == Keys.Delete && tabControl1.SelectedIndex == 1)
		{
			GridEXRow[] checkedRows = grdhistory.GetCheckedRows();
			for (int i = 0; i < checkedRows.Length; i++)
			{
				int.Parse(checkedRows[i].Cells["Id"].Value.ToString());
			}
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
			int Id = Conversion.ToInt(item.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Return)
			{
				Reset();
				ReadById(Id);
				btnSaveAs.Visible = false;
				btnsave.Visible = false;
				btnupdate.Visible = true;
			}
			else
			{
				if (!e.Control || e.KeyCode != Keys.Space)
				{
					return;
				}
				if (grdhistory.CurrentColumn.Key == "Edit")
				{
					Reset();
					ReadById(Id);
					btnSaveAs.Visible = false;
					btnsave.Visible = false;
					btnupdate.Visible = true;
				}
				else if (grdhistory.CurrentColumn.Key == "SaveAs")
				{
					if (!(Conversion.ToString(grdhistory.CurrentRow.Cells["DeliveryType"].Value) != "Sale Order"))
					{
						throw new Exception("Record With type 'Sale Order' Can not be Save As");
					}
					Reset();
					RecId = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString());
					ReadById(RecId);
					btnSaveAs.Visible = true;
					btnsave.Visible = false;
					btnupdate.Visible = false;
					((Control)(object)CmbDeliveryType).Enabled = true;
					((Control)(object)CmbSupplierCustomer).Enabled = true;
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow obj in rows)
					{
						obj.BeginEdit();
						obj.Cells["Id"].Value = 0;
						obj.EndEdit();
					}
				}
				else if (grdhistory.CurrentColumn.Key == "Print")
				{
					CommonServices.InvDeliveryOrderSlip_1200(Id, 1200);
				}
				else if (grdhistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, 1200);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Expected O, but got Unknown
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
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
			else
			{
				if (!e.Control || e.KeyCode != Keys.Space)
				{
					return;
				}
				if (grd.CurrentColumn.Key == "Delete")
				{
					if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
					{
						if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
						{
							return;
						}
						InvDeliveryOrderdetail vd = new InvDeliveryOrderdetail();
						vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
						vd.DeliveryTypeId = Conversion.ToInt(r.Cells["DeliveryTypeId"].Value);
						vd.SaleOrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
						vd.SaleOrderDetailId = Conversion.ToInt(r.Cells["SaleOrderDetailId"].Value);
						vd.SupplierCustomerId = Conversion.ToInt(r.Cells["SupCustId"].Value);
						vd.WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
						vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						vd.PackUomId = Conversion.ToInt(r.Cells["PackUomId"].Value);
						vd.LoadingRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
						vd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
						vd.DoQty = Conversion.ToDouble(r.Cells["ItemQty"].Value);
						vd.LoadingQty = Conversion.ToDouble(r.Cells["ItemQty"].Value);
						vd.DoWeight = Conversion.ToDouble(r.Cells["ItemQty"].Value);
						vd.LoadingWeight = Conversion.ToDouble(r.Cells["ItemQty"].Value);
						vd.AssetId = Conversion.ToInt(r.Cells["MachineAssetId"].Value);
						vd.ActionTypeId = 3;
						lstRemoveRecord.Add(vd);
						table.Rows[r.RowIndex].Delete();
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
					if (grd.RowCount == 0)
					{
						((Control)(object)CmbDeliveryType).Enabled = true;
						((Control)(object)CmbSupplierCustomer).Enabled = true;
					}
				}
				else if (grd.CurrentColumn.Key == "Add")
				{
					DataRow dr = table.Rows[r.RowIndex];
					table.Rows.Add(dr.ItemArray);
					if (btnupdate.Visible && btnupdate.Enabled)
					{
						table.Rows[table.Rows.Count - 1]["Id"] = 0;
					}
					grd.DataSource = table;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridhistoryfill()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		try
		{
			dthistoryfill.Rows.Clear();
			ReportsParameters reports = new ReportsParameters();
			reports.OrganizationId = UserAccount.OrganizationId;
			reports.CompanyId = UserAccount.CompanyId;
			reports.DocumentTypeId = 1200;
			reports.Activity = "Local";
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
			reports.FromDocNo = Conversion.ToInt((object)txtFromDocNoHistory.Text);
			reports.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text);
			reports.ActionId = Conversion.ToInt(CmbDeliveryTypeHistory.Value);
			reports.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			reports.RequestedById = Conversion.ToInt(CmbRequestedByHistory.Value);
			reports.ApprovedById = Conversion.ToInt(CmbApprovedByHistory.Value);
			dthistoryfill = InvDeliveryOrder.FormHistoryNew(reports);
			if (dthistoryfill.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DeliveryType");
				dt.Columns.Add("Customer");
				dt.Columns.Add("ReferenceParty");
				dt.Columns.Add("VehicleType");
				dt.Columns.Add("VehicleNo");
				dt.Columns.Add("RequestedBy");
				dt.Columns.Add("ApprovedBy");
				dt.Columns.Add("ApprovalStatus");
				dt.Columns.Add("EntryUser");
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser");
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ApprovedUser");
				dt.Columns.Add("ApprovedDate", typeof(DateTime));
				dt.Columns.Add("Remarks");
				dt.Columns.Add("NoOfAttachments", typeof(int));
				int i;
				for (i = 0; i < dthistoryfill.Rows.Count; i++)
				{
					if (!dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Conversion.ToInt(dthistoryfill.Rows[i]["Id"])))
					{
						dt.Rows.Add(dthistoryfill.Rows[i]["Id"], dthistoryfill.Rows[i]["DocDate"], dthistoryfill.Rows[i]["DocNo"], dthistoryfill.Rows[i]["DeliveryType"], dthistoryfill.Rows[i]["CustomerName"], dthistoryfill.Rows[i]["ReferencePartyName"], dthistoryfill.Rows[i]["VehicleType"], dthistoryfill.Rows[i]["VehicleNo"], dthistoryfill.Rows[i]["RequestedBy"], dthistoryfill.Rows[i]["ApprovedBy"], dthistoryfill.Rows[i]["ApprovalStatus"], dthistoryfill.Rows[i]["EntryUser"], dthistoryfill.Rows[i]["EntryDate"], dthistoryfill.Rows[i]["ModifyUser"], dthistoryfill.Rows[i]["ModifyDate"], dthistoryfill.Rows[i]["ApprovedUser"], dthistoryfill.Rows[i]["ApprovedDate"], dthistoryfill.Rows[i]["HeaderRemarks"], dthistoryfill.Rows[i]["NoOfAttachments"]);
					}
				}
				grdhistory.DataSource = dt;
				grdhistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				grdhistory.ClearStructure();
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
			grdhistory.AllowEdit = (InheritableBoolean)2;
			grdhistory.AllowDelete = (InheritableBoolean)2;
			grdhistory.RootTable.Columns["Id"].Visible = false;
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			GridEX_Helper.GridWrappingAndColumnSettings(grdhistory, 2, 3);
			grdhistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant + 20;
			grdhistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdhistory.RootTable.Columns["DeliveryType"].Width = Constants.InventoryConstants.DiscountType;
			grdhistory.RootTable.Columns["Customer"].Width = Constants.InventoryConstants.SupplierName + 30;
			grdhistory.RootTable.Columns["ReferenceParty"].Width = Constants.InventoryConstants.SupplierName + 30;
			grdhistory.RootTable.Columns["VehicleType"].Width = Constants.InventoryConstants.VehicleTypeConstant;
			grdhistory.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			grdhistory.RootTable.Columns["RequestedBy"].Width = Constants.InventoryConstants.EntryUserConstants + 50;
			grdhistory.RootTable.Columns["ApprovedBy"].Width = Constants.InventoryConstants.EntryUserConstants + 50;
			grdhistory.RootTable.Columns["ApprovalStatus"].Width = Constants.InventoryConstants.StatusConstant;
			grdhistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 30;
			grdhistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdhistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.EntryUserConstants + 30;
			grdhistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdhistory.RootTable.Columns["ApprovedUser"].Width = Constants.InventoryConstants.EntryUserConstants + 30;
			grdhistory.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdhistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants + 30;
			grdhistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			grdhistory.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yyyy";
			grdhistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns.Add("Edit");
			grdhistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdhistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdhistory.RootTable.Columns["Edit"].Width = 35;
			grdhistory.RootTable.Columns["Edit"].Position = 0;
			grdhistory.RootTable.Columns.Add("Print");
			grdhistory.RootTable.Columns["Print"].Caption = "Print";
			grdhistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Print"].ButtonText = "Print";
			grdhistory.RootTable.Columns["Print"].Width = 40;
			grdhistory.RootTable.Columns["Print"].Position = 1;
			grdhistory.RootTable.Columns.Add("SaveAs");
			grdhistory.RootTable.Columns["SaveAs"].Caption = "SaveAs";
			grdhistory.RootTable.Columns["SaveAs"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["SaveAs"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["SaveAs"].ButtonText = "SaveAs";
			grdhistory.RootTable.Columns["SaveAs"].Width = 55;
			grdhistory.RootTable.Columns["SaveAs"].Position = 2;
			grdhistory.FrozenColumns = 3;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnLoadAll_Click(object sender, EventArgs e)
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

	private void grdhistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "NoOfAttachments")
			{
				GridEXRow item = grdhistory.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 1200);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_DoubleClick(object sender, EventArgs e)
	{
		RecId = Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value);
		ReadById(RecId);
		btnSaveAs.Visible = false;
		btnsave.Visible = false;
		btnupdate.Visible = true;
		btnDelete.Visible = false;
	}

	private void grdhistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				Reset();
				RecId = Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value);
				ReadById(RecId);
				btnSaveAs.Visible = false;
				btnsave.Visible = false;
				btnupdate.Visible = true;
				btnDelete.Visible = false;
			}
			else if (e.Column.Key == "SaveAs")
			{
				if (!(Conversion.ToString(grdhistory.CurrentRow.Cells["DeliveryType"].Value) != "Sale Order"))
				{
					throw new Exception("Record With type 'Sale Order' Can not be Save As");
				}
				Reset();
				RecId = Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value);
				ReadById(RecId);
				btnSaveAs.Visible = true;
				btnsave.Visible = false;
				btnupdate.Visible = false;
				btnDelete.Visible = false;
				((Control)(object)CmbDeliveryType).Enabled = true;
				((Control)(object)CmbSupplierCustomer).Enabled = true;
				if (grd.RowCount > 0)
				{
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow obj in rows)
					{
						obj.BeginEdit();
						obj.Cells["Id"].Value = 0;
						obj.EndEdit();
					}
				}
			}
			else if (e.Column.Key == "Print")
			{
				CommonServices.InvDeliveryOrderSlip_1200(Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value), 1200);
			}
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
	}

	private void grdhistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdhistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("WareHouseName");
				dt.Columns.Add("ItemCategory", typeof(string));
				dt.Columns.Add("ItemCode", typeof(string));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("UOM");
				dt.Columns.Add("ItemDescription", typeof(string));
				dt.Columns.Add("Qty", typeof(double));
				DataTable resultRows = (from row in dthistoryfill.AsEnumerable()
					where row.Field<int>("Id") == Conversion.ToInt(item.Cells["Id"].Value)
					select row).CopyToDataTable();
				for (int i = 0; i < resultRows.Rows.Count; i++)
				{
					dt.Rows.Add(resultRows.Rows[i]["WareHouseName"], resultRows.Rows[i]["ItemCategory"], resultRows.Rows[i]["ItemCode"], resultRows.Rows[i]["ItemName"], resultRows.Rows[i]["PackUOM"], resultRows.Rows[i]["ItemDiscription"], resultRows.Rows[i]["LoadingQty"]);
				}
				GrdHistoryDetail.DataSource = dt;
				GrdHistoryDetail.RetrieveStructure();
				GridDetailSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetailSetting()
	{
		try
		{
			GrdHistoryDetail.RootTable.Columns["WareHouseName"].Caption = "WareHouse";
			GrdHistoryDetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			GrdHistoryDetail.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(GrdHistoryDetail);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			ToDateHistory.Value = DateTime.Now;
			txtFromDocNoHistory.Text = string.Empty;
			txtToDocNoHistory.Text = string.Empty;
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			((Control)(object)CmbRequestedByHistory).Text = string.Empty;
			((Control)(object)CmbApprovedByHistory).Text = string.Empty;
			((Control)(object)CmbDeliveryTypeHistory).Text = string.Empty;
			grdhistory.DataSource = null;
			GrdHistoryDetail.DataSource = null;
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
			HistoryComboBind(HistoryComboDbCall());
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
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
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
			MessageBox.Show(ex.Message);
		}
	}

	private void btnattachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void btnprint_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.InvDeliveryOrderSlip_1200(RecId, 1200);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combitem_ValueChanged(object sender, EventArgs e)
	{
	}

	private void btnDefineReferenceParties_Click(object sender, EventArgs e)
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

	private void RequestedApprovedLookup_Click(object sender, EventArgs e)
	{
		try
		{
			frmInvLookup frmInvLookup = new frmInvLookup(UserAccount);
			frmInvLookup.Show();
			frmInvLookup.cmbProfileName.Value = 13;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSupplierCustomer_TextChanged(object sender, EventArgs e)
	{
	}

	private void DocDate_ValueChanged(object sender, EventArgs e)
	{
	}

	private void CmbSupplierCustomer_Leave(object sender, EventArgs e)
	{
	}

	private void AvailableStockGetByItem()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		try
		{
			double CurrStock = InvDeliveryOrder.GetAvailableStockForDeliveryOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				WarehouseId = Conversion.ToInt(CmbWareHouse.Value),
				ItemId = Conversion.ToInt(CmbItemName.Value),
				DocDate = Conversion.ToDateTime((object)DocDate.Value),
				stockUOM = Conversion.ToInt(CmbPackUom.Value)
			});
			if (CurrStock > 0.0)
			{
				lblBalance.Text = CurrStock.ToString();
			}
			else
			{
				lblBalance.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbWareHouse_TextChanged(object sender, EventArgs e)
	{
	}

	private void txtVehicleNo_TextChanged(object sender, EventArgs e)
	{
		txtVehicleNo.SelectionStart = txtVehicleNo.Text.Length;
		txtVehicleNo.Text = txtVehicleNo.Text.ToUpper();
	}

	private void CmbDeliveryType_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CmbSupplierCustomer_Leave(null, null);
			if (grd.RowCount <= 0 || ((UltraGridBase)CmbDeliveryType).ActiveRow == null || Conversion.ToInt(CmbDeliveryType.Value) <= 0)
			{
				return;
			}
			GridEXRow[] rows;
			if (Conversion.ToInt(CmbDeliveryType.Value) == 1)
			{
				bool isAllOrdersSelected = true;
				rows = grd.GetRows();
				for (int i = 0; i < rows.Length; i++)
				{
					if (Conversion.ToInt(rows[i].Cells["OrderId"].Value) == 0)
					{
						isAllOrdersSelected = false;
						break;
					}
				}
				if (!isAllOrdersSelected)
				{
					CmbDeliveryType.Value = 2;
					CmbDeliveryType.Focus();
					throw new Exception("You cannot select delivery type 'Sale Order' because data without OrderId exists in the grid.");
				}
			}
			rows = grd.GetRows();
			foreach (GridEXRow obj in rows)
			{
				obj.BeginEdit();
				obj.Cells["DeliveryTypeId"].Value = Conversion.ToInt(CmbDeliveryType.Value);
				obj.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbDeliveryType_Leave(object sender, EventArgs e)
	{
	}

	private void rdbtnItemName_CheckedChanged(object sender, EventArgs e)
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

	private void CmbItemCategory_Leave(object sender, EventArgs e)
	{
		try
		{
			dtitem.Rows.Clear();
			int CategoryId = Conversion.ToInt(CmbItemCategory.Value);
			if (((UltraGridBase)CmbItemCategory).ActiveRow != null && CategoryId > 0 && Conversion.ToInt(((UltraDropDownBase)CmbItemCategory).SelectedRow.Cells["ParentCategoryId"].Value) == 5)
			{
				if (dtAssetItem.Rows.Count > 0)
				{
					for (int i = 0; i < dtAssetItem.Rows.Count; i++)
					{
						if (Conversion.ToInt(dtAssetItem.Rows[i]["ItemCategoryId"]) == CategoryId)
						{
							dtitem.Rows.Add(dtAssetItem.Rows[i]["assetRegisterId"], dtAssetItem.Rows[i]["assetName"], dtAssetItem.Rows[i]["assetCode"]);
						}
					}
					ItemNameBind();
				}
				else
				{
					((Control)(object)CmbItemName).Text = string.Empty;
					((UltraGridBase)CmbItemName).DataSource = null;
				}
			}
			else
			{
				ItemDtsFillFromGlobal(CategoryId);
				ItemNameBind();
			}
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

	private void btnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				MessageBox.Show("Record Id Not Found", "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				if (IsApproved)
				{
					MessageBox.Show("Record has been approved", "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				else if (btnDelete.Visible && btnDelete.Enabled)
				{
					CommonServices.RemoveByID(1200, RecId);
					MessageBox.Show("Record Deleted Successfully");
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Expected O, but got Unknown
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Expected O, but got Unknown
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Expected O, but got Unknown
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Expected O, but got Unknown
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Expected O, but got Unknown
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Expected O, but got Unknown
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Expected O, but got Unknown
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Expected O, but got Unknown
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Expected O, but got Unknown
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Expected O, but got Unknown
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Expected O, but got Unknown
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Expected O, but got Unknown
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
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Expected O, but got Unknown
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Expected O, but got Unknown
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Expected O, but got Unknown
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Expected O, but got Unknown
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Expected O, but got Unknown
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Expected O, but got Unknown
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Expected O, but got Unknown
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Expected O, but got Unknown
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Expected O, but got Unknown
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Expected O, but got Unknown
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Expected O, but got Unknown
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Expected O, but got Unknown
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
		//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Expected O, but got Unknown
		//IL_051e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0528: Expected O, but got Unknown
		//IL_056b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0575: Expected O, but got Unknown
		//IL_0581: Unknown result type (might be due to invalid IL or missing references)
		//IL_058b: Expected O, but got Unknown
		//IL_05b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c2: Expected O, but got Unknown
		//IL_05ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f9: Expected O, but got Unknown
		//IL_0605: Unknown result type (might be due to invalid IL or missing references)
		//IL_060f: Expected O, but got Unknown
		//IL_0631: Unknown result type (might be due to invalid IL or missing references)
		//IL_063b: Expected O, but got Unknown
		//IL_0652: Unknown result type (might be due to invalid IL or missing references)
		//IL_065c: Expected O, but got Unknown
		//IL_0668: Unknown result type (might be due to invalid IL or missing references)
		//IL_0672: Expected O, but got Unknown
		//IL_0689: Unknown result type (might be due to invalid IL or missing references)
		//IL_0693: Expected O, but got Unknown
		//IL_07c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d2: Expected O, but got Unknown
		//IL_07f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fe: Expected O, but got Unknown
		//IL_084c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0856: Expected O, but got Unknown
		//IL_0862: Unknown result type (might be due to invalid IL or missing references)
		//IL_086c: Expected O, but got Unknown
		//IL_0878: Unknown result type (might be due to invalid IL or missing references)
		//IL_0882: Expected O, but got Unknown
		//IL_08c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cf: Expected O, but got Unknown
		//IL_0ee8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef2: Expected O, but got Unknown
		//IL_0eff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f09: Expected O, but got Unknown
		//IL_6011: Unknown result type (might be due to invalid IL or missing references)
		//IL_601b: Expected O, but got Unknown
		//IL_603f: Unknown result type (might be due to invalid IL or missing references)
		//IL_6049: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
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
		Appearance appearance218 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance233 = new Appearance();
		Appearance appearance234 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance238 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
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
		Appearance appearance140 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
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
		Appearance appearance253 = new Appearance();
		Appearance appearance254 = new Appearance();
		Appearance appearance255 = new Appearance();
		Appearance appearance256 = new Appearance();
		Appearance appearance257 = new Appearance();
		Appearance appearance258 = new Appearance();
		Appearance appearance259 = new Appearance();
		Appearance appearance260 = new Appearance();
		Appearance appearance261 = new Appearance();
		Appearance appearance262 = new Appearance();
		Appearance appearance263 = new Appearance();
		Appearance appearance264 = new Appearance();
		Appearance appearance196 = new Appearance();
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
		Appearance appearance207 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.ReturnableInventoryManagement.DeliveryOrder_ReturnAble));
		GridEXLayout GrdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance265 = new Appearance();
		Appearance appearance266 = new Appearance();
		Appearance appearance267 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance241 = new Appearance();
		Appearance appearance242 = new Appearance();
		Appearance appearance243 = new Appearance();
		Appearance appearance244 = new Appearance();
		Appearance appearance245 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance247 = new Appearance();
		Appearance appearance248 = new Appearance();
		Appearance appearance249 = new Appearance();
		Appearance appearance250 = new Appearance();
		Appearance appearance251 = new Appearance();
		Appearance appearance252 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.DetailGridPanel = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.CmbItemCategory = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.rdbtnItemCode = new System.Windows.Forms.RadioButton();
		this.rdbtnItemName = new System.Windows.Forms.RadioButton();
		this.txtItemDescriptions = new System.Windows.Forms.TextBox();
		this.label52 = new System.Windows.Forms.Label();
		this.lblBalance = new System.Windows.Forms.Label();
		this.CmbPackUom = new UltraCombo();
		this.label36 = new System.Windows.Forms.Label();
		this.CmbWareHouse = new UltraCombo();
		this.labelWarFrom = new System.Windows.Forms.Label();
		this.txtqty = new System.Windows.Forms.TextBox();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.label38 = new System.Windows.Forms.Label();
		this.CmbItemName = new UltraCombo();
		this.btnplus = new System.Windows.Forms.Button();
		this.label37 = new System.Windows.Forms.Label();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.CmbReferenceParty = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.CmbDeliveryType = new UltraCombo();
		this.RequestedApprovedLookup = new System.Windows.Forms.Button();
		this.label18 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.CmbSupplierCustomer = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.CmbApprovedBy = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.CmbRequestedBy = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.cmbvehicletype = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.txtVehicleNo = new System.Windows.Forms.TextBox();
		this.txtSubNoId = new System.Windows.Forms.TextBox();
		this.txtDocNoId = new System.Windows.Forms.TextBox();
		this.txtRefDocTypeId = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.ChkPrint = new System.Windows.Forms.CheckBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnSaveAs = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.GrdHistoryDetail = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label27 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdhistory = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.label22 = new System.Windows.Forms.Label();
		this.CmbApprovedByHistory = new UltraCombo();
		this.label21 = new System.Windows.Forms.Label();
		this.CmbRequestedByHistory = new UltraCombo();
		this.label24 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.label7 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.CmbDeliveryTypeHistory = new UltraCombo();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label16 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.button1 = new System.Windows.Forms.Button();
		this.label28 = new System.Windows.Forms.Label();
		this.datReturnableDate = new System.Windows.Forms.DateTimePicker();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.DetailGridPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel5.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbReferenceParty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierCustomer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbApprovedBy).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRequestedBy).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbvehicletype).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel12.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryDetail).BeginInit();
		this.panel11.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel16.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbApprovedByHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRequestedByHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTypeHistory).BeginInit();
		this.panel8.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1070, 584);
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
		this.tabControl1.Size = new System.Drawing.Size(1070, 584);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.DetailGridPanel);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(1062, 555);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.DetailGridPanel.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.DetailGridPanel.Controls.Add(this.panel5);
		this.DetailGridPanel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.DetailGridPanel.Location = new System.Drawing.Point(2, 258);
		this.DetailGridPanel.Margin = new System.Windows.Forms.Padding(2);
		this.DetailGridPanel.Name = "DetailGridPanel";
		this.DetailGridPanel.Size = new System.Drawing.Size(1058, 295);
		this.DetailGridPanel.TabIndex = 4;
		this.grd.AutomaticSort = false;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 28);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1058, 267);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel5.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Controls.Add(this.label15);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1058, 28);
		this.panel5.TabIndex = 2;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(4, 4);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(53, 18);
		this.label15.TabIndex = 0;
		this.label15.Text = "Detail";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox3);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add(this.txtSubNoId);
		this.panel4.Controls.Add(this.txtDocNoId);
		this.panel4.Controls.Add(this.txtRefDocTypeId);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 55);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1058, 203);
		this.panel4.TabIndex = 2;
		this.groupBox3.BackColor = System.Drawing.Color.White;
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemCategory);
		this.groupBox3.Controls.Add(this.label8);
		this.groupBox3.Controls.Add(this.rdbtnItemCode);
		this.groupBox3.Controls.Add(this.rdbtnItemName);
		this.groupBox3.Controls.Add(this.txtItemDescriptions);
		this.groupBox3.Controls.Add(this.label52);
		this.groupBox3.Controls.Add(this.lblBalance);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUom);
		this.groupBox3.Controls.Add(this.label36);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWareHouse);
		this.groupBox3.Controls.Add(this.labelWarFrom);
		this.groupBox3.Controls.Add(this.txtqty);
		this.groupBox3.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox3.Controls.Add(this.label38);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.groupBox3.Controls.Add(this.btnplus);
		this.groupBox3.Controls.Add(this.label37);
		this.groupBox3.Controls.Add(this.btnUpdateDetail);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(3, 132);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1010, 68);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Detail";
		this.CmbItemCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance135;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance141).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance142).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance142).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance142).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Location = new System.Drawing.Point(176, 40);
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Name = "CmbItemCategory";
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Size = new System.Drawing.Size(159, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Leave += new System.EventHandler(CmbItemCategory_Leave);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(176, 19);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(82, 15);
		this.label8.TabIndex = 133;
		this.label8.Text = "Item Category";
		this.rdbtnItemCode.AutoSize = true;
		this.rdbtnItemCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemCode.Location = new System.Drawing.Point(434, 17);
		this.rdbtnItemCode.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemCode.Name = "rdbtnItemCode";
		this.rdbtnItemCode.Size = new System.Drawing.Size(52, 19);
		this.rdbtnItemCode.TabIndex = 131;
		this.rdbtnItemCode.Text = "Code";
		this.rdbtnItemCode.UseVisualStyleBackColor = true;
		this.rdbtnItemName.AutoSize = true;
		this.rdbtnItemName.Checked = true;
		this.rdbtnItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemName.Location = new System.Drawing.Point(373, 17);
		this.rdbtnItemName.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemName.Name = "rdbtnItemName";
		this.rdbtnItemName.Size = new System.Drawing.Size(57, 19);
		this.rdbtnItemName.TabIndex = 130;
		this.rdbtnItemName.TabStop = true;
		this.rdbtnItemName.Text = "Name";
		this.rdbtnItemName.UseVisualStyleBackColor = true;
		this.rdbtnItemName.CheckedChanged += new System.EventHandler(rdbtnItemName_CheckedChanged);
		this.txtItemDescriptions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemDescriptions.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemDescriptions.Location = new System.Drawing.Point(637, 41);
		this.txtItemDescriptions.Name = "txtItemDescriptions";
		this.txtItemDescriptions.Size = new System.Drawing.Size(219, 23);
		this.txtItemDescriptions.TabIndex = 4;
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(637, 20);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(96, 15);
		this.label52.TabIndex = 52;
		this.label52.Text = "Item Description";
		this.lblBalance.AutoSize = true;
		this.lblBalance.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBalance.Location = new System.Drawing.Point(510, 19);
		this.lblBalance.Name = "lblBalance";
		this.lblBalance.Size = new System.Drawing.Size(14, 15);
		this.lblBalance.TabIndex = 46;
		this.lblBalance.Text = "0";
		this.CmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance173).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance173).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance174;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance179).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance180).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance180).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance180).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance180;
		((AppearanceBase)appearance181).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Location = new System.Drawing.Point(547, 40);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Name = "CmbPackUom";
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Size = new System.Drawing.Size(89, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabIndex = 3;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(547, 22);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(35, 15);
		this.label36.TabIndex = 8;
		this.label36.Text = "UOM";
		this.CmbWareHouse.AllowNull = (DefaultableBoolean)1;
		this.CmbWareHouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWareHouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWareHouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance208).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance209).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance209).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance209;
		((AppearanceBase)appearance210).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance210;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance211).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance211).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance211).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance212).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance212;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance213).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance213;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance214;
		((AppearanceBase)appearance215).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance215).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance216).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance216).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance216).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance216).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance216;
		((AppearanceBase)appearance217).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance218).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance218;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance219).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWareHouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Location = new System.Drawing.Point(3, 40);
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Name = "CmbWareHouse";
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Size = new System.Drawing.Size(172, 26);
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).TextChanged += new System.EventHandler(CmbWareHouse_TextChanged);
		this.labelWarFrom.AutoSize = true;
		this.labelWarFrom.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelWarFrom.Location = new System.Drawing.Point(3, 19);
		this.labelWarFrom.Name = "labelWarFrom";
		this.labelWarFrom.Size = new System.Drawing.Size(69, 15);
		this.labelWarFrom.TabIndex = 33;
		this.labelWarFrom.Text = "WareHouse";
		this.txtqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtqty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtqty.Location = new System.Drawing.Point(857, 41);
		this.txtqty.Name = "txtqty";
		this.txtqty.ShortcutsEnabled = false;
		this.txtqty.Size = new System.Drawing.Size(82, 23);
		this.txtqty.TabIndex = 5;
		this.txtqty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtqty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(940, 13);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(70, 25);
		this.btnCancelUpdateDetial.TabIndex = 8;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.btnCancelUpdateDetial.Leave += new System.EventHandler(btnCancelUpdateDetial_Leave);
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(857, 21);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(26, 15);
		this.label38.TabIndex = 13;
		this.label38.Text = "Qty";
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance184).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance185).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance185).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance185;
		((AppearanceBase)appearance186).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance186;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance232).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance232).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance232).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance232;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance233).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance233).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance233;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance234).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance234;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance235;
		((AppearanceBase)appearance236).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance236).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance237).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance237).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance237).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance237).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance237;
		((AppearanceBase)appearance238).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance239).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance239;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance240;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(336, 40);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(210, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 2;
		this.CmbItemName.ValueChanged += new System.EventHandler(combitem_ValueChanged);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Leave += new System.EventHandler(CmbItemName_Leave);
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(940, 39);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(33, 25);
		this.btnplus.TabIndex = 6;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.btnplus.Leave += new System.EventHandler(btnplus_Leave);
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(338, 19);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(32, 15);
		this.label37.TabIndex = 9;
		this.label37.Text = "Item";
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(944, 39);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(66, 25);
		this.btnUpdateDetail.TabIndex = 7;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.button1);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbReferenceParty);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.datReturnableDate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryType);
		this.groupBox1.Controls.Add(this.RequestedApprovedLookup);
		this.groupBox1.Controls.Add(this.label18);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.txtremarks);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbApprovedBy);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRequestedBy);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.txtdocno);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbvehicletype);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.DocDate);
		this.groupBox1.Controls.Add(this.txtVehicleNo);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1009, 128);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.CmbReferenceParty.AllowNull = (DefaultableBoolean)1;
		this.CmbReferenceParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbReferenceParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbReferenceParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbReferenceParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance111;
		((SpecialBoxBase)((UltraGridBase)this.CmbReferenceParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance116).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbReferenceParty).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbReferenceParty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbReferenceParty).Location = new System.Drawing.Point(100, 96);
		((System.Windows.Forms.Control)(object)this.CmbReferenceParty).Name = "CmbReferenceParty";
		((System.Windows.Forms.Control)(object)this.CmbReferenceParty).Size = new System.Drawing.Size(226, 26);
		((System.Windows.Forms.Control)(object)this.CmbReferenceParty).TabIndex = 4;
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.White;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(6, 94);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(62, 30);
		this.label3.TabIndex = 66;
		this.label3.Text = "Reference \r\nParty";
		this.CmbDeliveryType.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbDeliveryType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance151).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance151).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance162;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance187).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance187).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.CmbDeliveryType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance188;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.CmbDeliveryType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance190;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance191).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.CmbDeliveryType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance192).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance192).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance192).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance192).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.CmbDeliveryType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance194;
		((UltraGridBase)this.CmbDeliveryType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.CmbDeliveryType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryType).Location = new System.Drawing.Point(100, 42);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryType).Name = "CmbDeliveryType";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryType).Size = new System.Drawing.Size(226, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryType).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryType).TextChanged += new System.EventHandler(CmbDeliveryType_TextChanged);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryType).Leave += new System.EventHandler(CmbDeliveryType_Leave);
		this.RequestedApprovedLookup.BackColor = System.Drawing.Color.White;
		this.RequestedApprovedLookup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.RequestedApprovedLookup.Font = new System.Drawing.Font("Verdana", 11.75f);
		this.RequestedApprovedLookup.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.RequestedApprovedLookup.Location = new System.Drawing.Point(963, 15);
		this.RequestedApprovedLookup.Margin = new System.Windows.Forms.Padding(2);
		this.RequestedApprovedLookup.Name = "RequestedApprovedLookup";
		this.RequestedApprovedLookup.Size = new System.Drawing.Size(23, 26);
		this.RequestedApprovedLookup.TabIndex = 10;
		this.RequestedApprovedLookup.TabStop = false;
		this.RequestedApprovedLookup.UseVisualStyleBackColor = false;
		this.RequestedApprovedLookup.Click += new System.EventHandler(RequestedApprovedLookup_Click);
		this.label18.AutoSize = true;
		this.label18.BackColor = System.Drawing.Color.White;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(330, 100);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(52, 15);
		this.label18.TabIndex = 60;
		this.label18.Text = "Remarks";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(6, 48);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(78, 15);
		this.label11.TabIndex = 48;
		this.label11.Text = "Delivery Type";
		this.CmbSupplierCustomer.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplierCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplierCustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplierCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance220).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance221).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance221).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance221).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance221;
		((AppearanceBase)appearance222).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance222;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance223).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance223).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance223).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance223;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance224).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance224;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance225).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance226;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance227).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance227;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance228).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance228).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance228).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance228).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance228;
		((AppearanceBase)appearance229).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance230).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance230;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplierCustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Location = new System.Drawing.Point(100, 69);
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Name = "CmbSupplierCustomer";
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Size = new System.Drawing.Size(226, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).TextChanged += new System.EventHandler(CmbSupplierCustomer_TextChanged);
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Leave += new System.EventHandler(CmbSupplierCustomer_Leave);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(6, 75);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(69, 15);
		this.label2.TabIndex = 31;
		this.label2.Text = "Party Name";
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(406, 96);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(555, 23);
		this.txtremarks.TabIndex = 8;
		this.CmbApprovedBy.AllowNull = (DefaultableBoolean)1;
		this.CmbApprovedBy.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbApprovedBy.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbApprovedBy.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance253).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance253).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbApprovedBy).DisplayLayout.Appearance = (AppearanceBase)(object)appearance253;
		((UltraGridBase)this.CmbApprovedBy).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbApprovedBy).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance254).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance254).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance254).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance254).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbApprovedBy).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance254;
		((AppearanceBase)appearance255).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbApprovedBy).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance255;
		((SpecialBoxBase)((UltraGridBase)this.CmbApprovedBy).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance256).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance256).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance256).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance256).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbApprovedBy).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance256;
		((UltraGridBase)this.CmbApprovedBy).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbApprovedBy).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance257).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance257).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbApprovedBy).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance257;
		((AppearanceBase)appearance258).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance258).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbApprovedBy).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance258;
		((UltraGridBase)this.CmbApprovedBy).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbApprovedBy).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance259).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbApprovedBy).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance259;
		((AppearanceBase)appearance260).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance260).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbApprovedBy).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance260;
		((UltraGridBase)this.CmbApprovedBy).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbApprovedBy).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance261).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance261).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance261).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance261).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance261).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbApprovedBy).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance261;
		((AppearanceBase)appearance262).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbApprovedBy).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance262;
		((UltraGridBase)this.CmbApprovedBy).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbApprovedBy).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance263).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance263).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbApprovedBy).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance263;
		((UltraGridBase)this.CmbApprovedBy).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance264).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbApprovedBy).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance264;
		((UltraGridBase)this.CmbApprovedBy).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbApprovedBy).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbApprovedBy).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbApprovedBy).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbApprovedBy.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbApprovedBy).Location = new System.Drawing.Point(708, 42);
		((System.Windows.Forms.Control)(object)this.CmbApprovedBy).Name = "CmbApprovedBy";
		((System.Windows.Forms.Control)(object)this.CmbApprovedBy).Size = new System.Drawing.Size(253, 26);
		((System.Windows.Forms.Control)(object)this.CmbApprovedBy).TabIndex = 10;
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.White;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(626, 48);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(75, 15);
		this.label6.TabIndex = 54;
		this.label6.Text = "Approved By";
		this.CmbRequestedBy.AllowNull = (DefaultableBoolean)1;
		this.CmbRequestedBy.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRequestedBy.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRequestedBy.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Appearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance197).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance197).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRequestedBy).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance197;
		((AppearanceBase)appearance198).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance198;
		((SpecialBoxBase)((UltraGridBase)this.CmbRequestedBy).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance199).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance199).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance199).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance200;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance202;
		((AppearanceBase)appearance203).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance203).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance204).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance204).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance204).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance204).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance204;
		((AppearanceBase)appearance205).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance205;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRequestedBy).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRequestedBy.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRequestedBy).Location = new System.Drawing.Point(708, 15);
		((System.Windows.Forms.Control)(object)this.CmbRequestedBy).Name = "CmbRequestedBy";
		((System.Windows.Forms.Control)(object)this.CmbRequestedBy).Size = new System.Drawing.Size(253, 26);
		((System.Windows.Forms.Control)(object)this.CmbRequestedBy).TabIndex = 9;
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.White;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(626, 21);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(78, 15);
		this.label1.TabIndex = 52;
		this.label1.Text = "Requested By";
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(100, 17);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(95, 23);
		this.txtdocno.TabIndex = 0;
		this.cmbvehicletype.AllowNull = (DefaultableBoolean)1;
		this.cmbvehicletype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbvehicletype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbvehicletype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbvehicletype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance123;
		((SpecialBoxBase)((UltraGridBase)this.cmbvehicletype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance128).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbvehicletype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).Location = new System.Drawing.Point(406, 15);
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).Name = "cmbvehicletype";
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).Size = new System.Drawing.Size(215, 26);
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).TabIndex = 5;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(6, 21);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(88, 15);
		this.label5.TabIndex = 3;
		this.label5.Text = "Doc No && Data";
		this.label14.AutoSize = true;
		this.label14.BackColor = System.Drawing.Color.White;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(330, 21);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(73, 15);
		this.label14.TabIndex = 50;
		this.label14.Text = "Vehicle Type";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(330, 48);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(64, 15);
		this.label12.TabIndex = 50;
		this.label12.Text = "Vehicle No";
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(196, 17);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(130, 23);
		this.DocDate.TabIndex = 1;
		this.DocDate.TabStop = false;
		this.DocDate.ValueChanged += new System.EventHandler(DocDate_ValueChanged);
		this.txtVehicleNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVehicleNo.Location = new System.Drawing.Point(406, 44);
		this.txtVehicleNo.Name = "txtVehicleNo";
		this.txtVehicleNo.Size = new System.Drawing.Size(214, 23);
		this.txtVehicleNo.TabIndex = 6;
		this.txtVehicleNo.TextChanged += new System.EventHandler(txtVehicleNo_TextChanged);
		this.txtSubNoId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSubNoId.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSubNoId.Location = new System.Drawing.Point(1196, 10);
		this.txtSubNoId.Name = "txtSubNoId";
		this.txtSubNoId.ReadOnly = true;
		this.txtSubNoId.Size = new System.Drawing.Size(39, 25);
		this.txtSubNoId.TabIndex = 48;
		this.txtSubNoId.TabStop = false;
		this.txtSubNoId.Visible = false;
		this.txtDocNoId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoId.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoId.Location = new System.Drawing.Point(1196, 41);
		this.txtDocNoId.Name = "txtDocNoId";
		this.txtDocNoId.ReadOnly = true;
		this.txtDocNoId.Size = new System.Drawing.Size(39, 25);
		this.txtDocNoId.TabIndex = 47;
		this.txtDocNoId.TabStop = false;
		this.txtDocNoId.Visible = false;
		this.txtRefDocTypeId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRefDocTypeId.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRefDocTypeId.Location = new System.Drawing.Point(1196, 72);
		this.txtRefDocTypeId.Name = "txtRefDocTypeId";
		this.txtRefDocTypeId.ReadOnly = true;
		this.txtRefDocTypeId.Size = new System.Drawing.Size(39, 25);
		this.txtRefDocTypeId.TabIndex = 46;
		this.txtRefDocTypeId.TabStop = false;
		this.txtRefDocTypeId.Visible = false;
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add(this.ChkPrint);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1058, 28);
		this.panel3.TabIndex = 1;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(2, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(359, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "Delivery Order / Delivery Instructions Note";
		this.ChkPrint.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrint.AutoSize = true;
		this.ChkPrint.Checked = true;
		this.ChkPrint.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrint.ForeColor = System.Drawing.Color.White;
		this.ChkPrint.Location = new System.Drawing.Point(909, 6);
		this.ChkPrint.Name = "ChkPrint";
		this.ChkPrint.Size = new System.Drawing.Size(148, 20);
		this.ChkPrint.TabIndex = 9;
		this.ChkPrint.TabStop = false;
		this.ChkPrint.Text = "Preview On Save";
		this.ChkPrint.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1058, 25);
		this.panel2.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.btnnew, this.btnRefresh, this.btnupdate, this.btnSaveAs, this.btnsave, this.btnattachment, this.btnDelete, this.toolStripSeparator2, this.btnprint });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1058, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = (System.Drawing.Image)resources.GetObject("btnRefresh.Image");
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnSaveAs.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSaveAs.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveAs.Name = "btnSaveAs";
		this.btnSaveAs.Size = new System.Drawing.Size(80, 24);
		this.btnSaveAs.Text = "SaveAs";
		this.btnSaveAs.Visible = false;
		this.btnSaveAs.Click += new System.EventHandler(btnSaveAs_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnattachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.icons8_delete_bin_16;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnprint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnprint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(100, 24);
		this.btnprint.Text = "1200-Print";
		this.btnprint.Click += new System.EventHandler(btnprint_Click);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(1192, 604);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel7.Controls.Add(this.panel9);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(2, 2);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1188, 600);
		this.panel7.TabIndex = 0;
		this.panel9.Controls.Add(this.panel12);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Margin = new System.Windows.Forms.Padding(2);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1188, 600);
		this.panel9.TabIndex = 1;
		this.panel12.Controls.Add(this.panel6);
		this.panel12.Controls.Add(this.panel10);
		this.panel12.Controls.Add(this.panel16);
		this.panel12.Controls.Add(this.panel8);
		this.panel12.Controls.Add(this.toolStrip2);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1188, 600);
		this.panel12.TabIndex = 46;
		this.panel6.Controls.Add(this.panel13);
		this.panel6.Controls.Add(this.panel11);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 417);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1188, 183);
		this.panel6.TabIndex = 54;
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistoryDetail);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(0, 23);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1188, 160);
		this.panel13.TabIndex = 1;
		this.GrdHistoryDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.GrdHistoryDetail).BackColor = System.Drawing.Color.White;
		GrdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistoryDetail.DesignTimeLayout = GrdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdHistoryDetail.DynamicFiltering = true;
		this.GrdHistoryDetail.FilterMode = (FilterMode)2;
		this.GrdHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistoryDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Name = "GrdHistoryDetail";
		((ScrollableBase)this.GrdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Size = new System.Drawing.Size(1188, 160);
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).TabIndex = 0;
		this.GrdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.GrdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label27);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1188, 23);
		this.panel11.TabIndex = 0;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label27.ForeColor = System.Drawing.Color.White;
		this.label27.Location = new System.Drawing.Point(2, 3);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(211, 17);
		this.label27.TabIndex = 2;
		this.label27.Text = "Detail Of Above Selected Row";
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 136);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1188, 281);
		this.panel10.TabIndex = 53;
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
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1188, 281);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 0;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.grdhistory.SelectionChanged += new System.EventHandler(grdhistory_SelectionChanged);
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdhistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdhistory_KeyDown);
		this.panel16.Controls.Add(this.groupBox2);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 55);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1188, 81);
		this.panel16.TabIndex = 52;
		this.groupBox2.Controls.Add(this.rdapproveddate);
		this.groupBox2.Controls.Add(this.rdmodifydate);
		this.groupBox2.Controls.Add(this.drdocdate);
		this.groupBox2.Controls.Add(this.rdentrydate);
		this.groupBox2.Controls.Add(this.label22);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbApprovedByHistory);
		this.groupBox2.Controls.Add(this.label21);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRequestedByHistory);
		this.groupBox2.Controls.Add(this.label24);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox2.Controls.Add(this.btnShowHistory);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.label19);
		this.groupBox2.Controls.Add(this.label13);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add(this.label20);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryTypeHistory);
		this.groupBox2.Controls.Add(this.txtToDocNoHistory);
		this.groupBox2.Controls.Add(this.txtFromDocNoHistory);
		this.groupBox2.Controls.Add(this.ToDateHistory);
		this.groupBox2.Controls.Add(this.FromDateHistory);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(6, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1172, 74);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(1069, 49);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 11;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(1069, 26);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 10;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(988, 26);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 8;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(988, 49);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 9;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(657, 48);
		this.label22.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(75, 15);
		this.label22.TabIndex = 151;
		this.label22.Text = "Approved By";
		this.CmbApprovedByHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbApprovedByHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbApprovedByHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbApprovedByHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbApprovedByHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbApprovedByHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbApprovedByHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbApprovedByHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbApprovedByHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance148;
		((SpecialBoxBase)((UltraGridBase)this.CmbApprovedByHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbApprovedByHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbApprovedByHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbApprovedByHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbApprovedByHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbApprovedByHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbApprovedByHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbApprovedByHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbApprovedByHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance154).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbApprovedByHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.CmbApprovedByHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbApprovedByHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance155).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance155).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance155).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbApprovedByHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbApprovedByHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbApprovedByHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbApprovedByHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbApprovedByHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbApprovedByHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbApprovedByHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.CmbApprovedByHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbApprovedByHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbApprovedByHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbApprovedByHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbApprovedByHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbApprovedByHistory).Location = new System.Drawing.Point(740, 42);
		((System.Windows.Forms.Control)(object)this.CmbApprovedByHistory).Name = "CmbApprovedByHistory";
		((System.Windows.Forms.Control)(object)this.CmbApprovedByHistory).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.CmbApprovedByHistory).TabIndex = 7;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(657, 19);
		this.label21.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(78, 15);
		this.label21.TabIndex = 149;
		this.label21.Text = "Requested By";
		this.CmbRequestedByHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbRequestedByHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRequestedByHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRequestedByHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance265).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance265).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance265;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance266).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance266).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance266).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance266).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance266;
		((AppearanceBase)appearance267).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance267;
		((SpecialBoxBase)((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance104).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRequestedByHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRequestedByHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRequestedByHistory).Location = new System.Drawing.Point(740, 13);
		((System.Windows.Forms.Control)(object)this.CmbRequestedByHistory).Name = "CmbRequestedByHistory";
		((System.Windows.Forms.Control)(object)this.CmbRequestedByHistory).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.CmbRequestedByHistory).TabIndex = 6;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(381, 48);
		this.label24.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(58, 15);
		this.label24.TabIndex = 147;
		this.label24.Text = "Customer";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance241).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance241).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance241;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance242).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance242).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance242).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance242;
		((AppearanceBase)appearance243).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance243;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance244).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance244).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance244).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance245).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance245;
		((AppearanceBase)appearance246).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance246).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance246;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance247).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance247;
		((AppearanceBase)appearance248).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance248).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance248;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance249).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance249).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance249).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance249).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance249).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance249;
		((AppearanceBase)appearance250).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance250;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance251).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance251;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance252).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance252;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(461, 42);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(193, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 5;
		this.btnShowHistory.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnShowHistory.FlatAppearance.BorderSize = 0;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnShowHistory.Location = new System.Drawing.Point(923, 42);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(58, 26);
		this.btnShowHistory.TabIndex = 12;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.btnShowHistory.Leave += new System.EventHandler(btnShowHistory_Leave);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(381, 19);
		this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(78, 15);
		this.label7.TabIndex = 142;
		this.label7.Text = "Delivery Type";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label19.Location = new System.Drawing.Point(214, 48);
		this.label19.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(64, 15);
		this.label19.TabIndex = 140;
		this.label19.Text = "To Doc No";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(214, 19);
		this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(79, 15);
		this.label13.TabIndex = 140;
		this.label13.Text = "From Doc No";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(4, 48);
		this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(48, 15);
		this.label10.TabIndex = 13;
		this.label10.Text = "To Date";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label20.Location = new System.Drawing.Point(4, 19);
		this.label20.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(63, 15);
		this.label20.TabIndex = 13;
		this.label20.Text = "From Date";
		this.CmbDeliveryTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbDeliveryTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance161;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance163).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance163).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.CmbDeliveryTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance164;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance165).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.CmbDeliveryTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance166;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance167).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.CmbDeliveryTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance168).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance168).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance168).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance168;
		((AppearanceBase)appearance169).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbDeliveryTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.CmbDeliveryTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.CmbDeliveryTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTypeHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTypeHistory).Location = new System.Drawing.Point(461, 13);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTypeHistory).Name = "CmbDeliveryTypeHistory";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTypeHistory).Size = new System.Drawing.Size(193, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTypeHistory).TabIndex = 4;
		this.txtToDocNoHistory.Location = new System.Drawing.Point(294, 44);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.ShortcutsEnabled = false;
		this.txtToDocNoHistory.Size = new System.Drawing.Size(83, 23);
		this.txtToDocNoHistory.TabIndex = 3;
		this.txtToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoHistory_KeyPress);
		this.txtFromDocNoHistory.Location = new System.Drawing.Point(294, 15);
		this.txtFromDocNoHistory.Name = "txtFromDocNoHistory";
		this.txtFromDocNoHistory.ShortcutsEnabled = false;
		this.txtFromDocNoHistory.Size = new System.Drawing.Size(83, 23);
		this.txtFromDocNoHistory.TabIndex = 2;
		this.txtFromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNoHistory_KeyPress);
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(67, 44);
		this.ToDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(142, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(67, 15);
		this.FromDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(142, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label16);
		this.panel8.Controls.Add(this.ctrlGrdBar2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 27);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1188, 28);
		this.panel8.TabIndex = 49;
		this.label16.AutoSize = true;
		this.label16.BackColor = System.Drawing.Color.Teal;
		this.label16.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.White;
		this.label16.Location = new System.Drawing.Point(4, 4);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(190, 19);
		this.label16.TabIndex = 22;
		this.label16.Text = "Delivery Order History";
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.btnRefreshHistory, this.btnLoadAll });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1188, 27);
		this.toolStrip2.TabIndex = 1;
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
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.btnLoadAll.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadAll.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(84, 24);
		this.btnLoadAll.Text = "&LoadAll";
		this.btnLoadAll.Visible = false;
		this.btnLoadAll.Click += new System.EventHandler(btnLoadAll_Click);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1017, 3);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1148, 3);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdhistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.button1.BackColor = System.Drawing.Color.White;
		this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button1.Font = new System.Drawing.Font("Verdana", 11.75f);
		this.button1.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.button1.Location = new System.Drawing.Point(75, 96);
		this.button1.Margin = new System.Windows.Forms.Padding(2);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(23, 26);
		this.button1.TabIndex = 5;
		this.button1.TabStop = false;
		this.button1.UseVisualStyleBackColor = false;
		this.button1.Click += new System.EventHandler(btnDefineReferenceParties_Click);
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(330, 66);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(67, 30);
		this.label28.TabIndex = 138;
		this.label28.Text = "Returnable \r\nDate";
		this.datReturnableDate.CustomFormat = "dd-MMM-yy";
		this.datReturnableDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datReturnableDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datReturnableDate.Location = new System.Drawing.Point(406, 70);
		this.datReturnableDate.Name = "datReturnableDate";
		this.datReturnableDate.Size = new System.Drawing.Size(214, 23);
		this.datReturnableDate.TabIndex = 7;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1070, 584);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "DeliveryOrder_ReturnAble";
		this.Text = "Delivery Order";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(DeliveryOrder_Engr_FormClosing);
		base.Load += new System.EventHandler(PurchsaeOrder_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(PurchsaeOrder_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.DetailGridPanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbReferenceParty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierCustomer).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbApprovedBy).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRequestedBy).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbvehicletype).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryDetail).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel16.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbApprovedByHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRequestedByHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTypeHistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
