using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.BLL.Production;
using Architecture.BLL.pcc;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.Model.Pcc;
using Architecture.Model.Production;
using Architecture.Model.pcc;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Architecture.WinApp.Supplier_Purchase;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.pcc;

public class frmWorkOrderConcrete : Form
{
	private Attachment AT = new Attachment();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtCustomerDetail = new DataTable();

	private DataTable dtInput = new DataTable();

	private DataTable dtHistoryGrid = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtRefParty = new DataTable();

	private DataTable dtItem = new DataTable();

	private DataTable dtItemInput = new DataTable();

	private DataTable dtAttributes = new DataTable();

	private DataTable dtCustomerOrderDetailWithItem = new DataTable();

	private DataTable dtBOMDataforCombo = new DataTable();

	private string ScreenName = "frmWorkOrderConcrete";

	private bool UpdateMode;

	private int RecId;

	private int PreviousbomId;

	private int DocumentTypeId = 1870;

	private int updateDetailIndex = -1;

	private bool FormInitialized;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private bool ItemSearchWithNameOrCode = true;

	private int DefaultDaysToLessFromHistoryFromDate;

	private List<WorkOrderCustomerDetail> lstRemoveRecordcustomerDetail = new List<WorkOrderCustomerDetail>();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton btnPrint;

	private TextBox txtWorkOrderNo;

	private Label label12;

	private TabPage tabPage3;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewHistory;

	private ToolStripSeparator toolStripSeparator1;

	private GroupBox groupBox2;

	private Label label21;

	private Label label37;

	private DateTimePicker txtdocdate;

	private TextBox txtdocno;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnDelete;

	private ToolStripButton btnshortcutkeys;

	private Panel panel38;

	private Label label32;

	internal GridEX grdHistoryDetail;

	private Panel panel9;

	internal GridEX grdHistory;

	private Panel panel15;

	private Label label17;

	private Panel panel13;

	private GroupBox groupBox1;

	private Label label19;

	internal UltraCombo cmbDateTypeHistory;

	private DateTimePicker ToDateHistory;

	private Label label28;

	private TextBox ToDocNoHistory;

	private DateTimePicker FromDateHistory;

	private Label label35;

	private TextBox FromDocNoHistory;

	private Button btnshow;

	private CtrlGrdBar ctrlGrdBar1;

	private GroupBox groupBox3;

	internal UltraCombo CmbCustomerOrderNo;

	internal UltraCombo CmbCustomerName;

	private TextBox txtCustomerOrderQty;

	private Label label10;

	private Label label38;

	private Label label25;

	private Label label23;

	private DateTimePicker txtDeadLineDateMain;

	private DateTimePicker txtEndDate;

	private DateTimePicker txtStartdate;

	private CheckBox ChkPreview;

	private TextBox txtRemarksHeader;

	private Label label8;

	private TextBox txtBatchDesc;

	private Label label13;

	private Label label15;

	private Label label36;

	internal UltraCombo cmbItemUOM;

	internal UltraCombo CmbItemDetail;

	private Label label26;

	internal GridEX grdCustomerDetail;

	private Button BtnAddCustomer;

	private Button BtnCancelCustomer;

	private Button BtnUpdateCustmer;

	private Label label9;

	private ToolStripButton btnSaveAs;

	private ToolStripButton btnAttachment;

	private TextBox textBox1;

	private Label label1;

	private TextBox txtVarientEquivalent;

	private Label label43;

	internal UltraCombo CmbAttribute;

	private Label label42;

	private TextBox txtReferenceNoDetail;

	private Label label2;

	internal UltraCombo CmbProductionType;

	private Label label16;

	internal UltraCombo CmbProductionTypeHistory;

	private Label label3;

	private Label label5;

	private ToolStripButton BtnRefreshHistory;

	private RadioButton rdSearchByCode;

	private RadioButton rdSearchByName;

	internal UltraCombo CmbWorkOrderNoHistory;

	private Label label6;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	internal UltraCombo CmbBOM;

	private Label label7;

	private Panel panel5;

	private Panel panel2;

	private Panel panel4;

	internal GridEX grdInputMaterial;

	private Panel panel7;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label4;

	private Panel panel14;

	private CtrlGrdBar ctrlGrdBar3;

	private Label label48;

	private TextBox txtBottomForce;

	private TextBox txtTopForce;

	private Label label47;

	private Button btnDefinePlant;

	internal UltraCombo CmbPlantName;

	private Label label11;

	private Panel panel6;

	private RadioButton RadCustomers;

	private RadioButton RadRefParty;

	private Label label14;

	private Button BtnReferenceParty;

	internal UltraCombo CmbItemNameHistory;

	private Label label20;

	internal UltraCombo CmbPlantNameHistory;

	private Label label18;

	public frmWorkOrderConcrete(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InitializeComponentCustom();
		InitializeComponentMethod();
	}

	private void frmBillOfMaterial_Manufacturing_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(1100, 650);
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
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("CompanyName", typeof(string));
			dtSupplier.Columns.Add("PartyCode", typeof(string));
			dtSupplier.Columns.Add("GlAccountId", typeof(int));
			dtSupplier.Columns.Add("CityName", typeof(string));
			dtSupplier.Columns.Add("MobileNo", typeof(string));
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("ItemName");
			dtItem.Columns.Add("ItemCode");
			dtItem.Columns.Add("ItemCategoryId", typeof(int));
			dtItem.Columns.Add("ItemCategory");
			dtItem.Columns.Add("ItemTypeId", typeof(int));
			dtItem.Columns.Add("ItemType");
			dtItem.Columns.Add("ParentTypeId", typeof(int));
			dtItem.Columns.Add("ParentType");
			dtItemInput = dtItem.Clone();
			dtItem.Columns.Add("OrderDetailId", typeof(int));
			dtItem.Columns.Add("ItemAttributeVarientId", typeof(int));
			dtItem.Columns.Add("ItemAttribute", typeof(string));
			dtItem.Columns.Add("VarientEquivalent", typeof(decimal));
			dtItem.Columns.Add("OrderItemQty", typeof(decimal));
			dtCustomerDetail.Columns.Add("Id", typeof(int));
			dtCustomerDetail.Columns.Add("CustomerTypeId", typeof(int));
			dtCustomerDetail.Columns.Add("CustomerType", typeof(string));
			dtCustomerDetail.Columns.Add("CustomerId", typeof(int));
			dtCustomerDetail.Columns.Add("CustomerName", typeof(string));
			dtCustomerDetail.Columns.Add("OrderId", typeof(int));
			dtCustomerDetail.Columns.Add("OrderNo", typeof(int));
			dtCustomerDetail.Columns.Add("OrderDetailId", typeof(int));
			dtCustomerDetail.Columns.Add("ItemId", typeof(int));
			dtCustomerDetail.Columns.Add("ItemCode");
			dtCustomerDetail.Columns.Add("ItemName");
			dtCustomerDetail.Columns.Add("ItemUomId", typeof(int));
			dtCustomerDetail.Columns.Add("UomCode", typeof(string));
			dtCustomerDetail.Columns.Add("UomEquivalent", typeof(decimal));
			dtCustomerDetail.Columns.Add("ItemAttributeVarientId", typeof(int));
			dtCustomerDetail.Columns.Add("AttributeVarient", typeof(string));
			dtCustomerDetail.Columns.Add("VarientEquivalent", typeof(decimal));
			dtCustomerDetail.Columns.Add("ItemQty", typeof(decimal));
			dtCustomerDetail.Columns.Add("VarientQty", typeof(decimal));
			dtCustomerDetail.Columns.Add("NetWeight", typeof(decimal));
			dtCustomerDetail.Columns.Add("ReferenceNo");
			dtCustomerDetail.Columns.Add("Remarks");
			dtInput.Columns.Add("Id", typeof(int));
			dtInput.Columns.Add("BomHeaderId", typeof(int));
			dtInput.Columns.Add("BomNo", typeof(string));
			dtInput.Columns.Add("BomInputId", typeof(int));
			dtInput.Columns.Add("ItemId", typeof(int));
			dtInput.Columns.Add("ItemName", typeof(string));
			dtInput.Columns.Add("ItemUomId", typeof(int));
			dtInput.Columns.Add("Uom", typeof(decimal));
			dtInput.Columns.Add("UomCode", typeof(string));
			dtInput.Columns.Add("ItemQty", typeof(decimal));
			dtInput.Columns.Add("NetWeight", typeof(decimal));
			dtInput.Columns.Add("Remarks");
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
			DataTable productionTypeData = ProductionTypeDbCall();
			string documentNoData = DocumentNoDbCall();
			DataTable historyComboData = HistoryComboDbCall();
			DataTable plantFeaderData = PlantFeaderDbCall();
			new DataTable();
			BOMNoDbCall();
			RefPartydtDbCall();
			return new
			{
				ProductionTypeData = productionTypeData,
				DocumentNoData = documentNoData,
				HistoryComboData = historyComboData,
				PlantFeaderData = plantFeaderData
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
				btnUpdate.Enabled = formright.DoHaveUpdateRights;
				btnDelete.Enabled = formright.DoHaveCanDelete;
				btnPrint.Enabled = formright.DoHavePrintRights;
				FormHelper.SetCtrlGrdBarRight(ctrlGrdBar1, formright);
				GetConfigurationsFromGlobal();
				if (ItemSearchWithNameOrCode)
				{
					rdSearchByCode.Checked = true;
				}
				else
				{
					rdSearchByName.Checked = true;
				}
				if (RecId == 0)
				{
					txtdocno.Text = result.DocumentNoData;
				}
				ProductionTypeBind(result.ProductionTypeData);
				PlantBind(result.PlantFeaderData);
				SupplierDtFillFromGlobal();
				BindSupplierName();
				BomNoFill();
				DataTable dataTable = dtCustomerDetail;
				if (dataTable != null && dataTable.Columns.Count > 0)
				{
					grdCustomerDetail.DataSource = dtCustomerDetail;
					grdCustomerDetail.RetrieveStructure();
					grdCustomerDetailSetting();
				}
				DataTable dataTable2 = dtInput;
				if (dataTable2 != null && dataTable2.Columns.Count > 0)
				{
					AddRowInInputGrid();
					grdInputMaterial.DataSource = dtInput;
					grdInputMaterial.RetrieveStructure();
					grdInputSettings();
				}
				HistoryComboBind(result.HistoryComboData);
				Datetypefill();
				FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
				ToDateHistory.Value = DateTime.Now;
				FormInitialized = true;
				txtdocdate.Focus();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void GetConfigurationsFromGlobal()
	{
		try
		{
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"));
			ItemSearchWithNameOrCode = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ItemSearchByCode"));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			return WorkOrderHeader.GenerateCode(new WorkOrderHeader
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

	private DataTable ProductionTypeDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		try
		{
			return InvLookUp.GetLookupsByTypeIdDt(new InvLookUp
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				InvLookupTypeId = 6
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ProductionTypeBind(DataTable dt)
	{
		try
		{
			DataTable dttype = new DataTable();
			dttype.Columns.Add("Id", typeof(int));
			dttype.Columns.Add("ProductionName", typeof(string));
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToInt(dt.Rows[i]["Id"]) == 45 || Conversion.ToInt(dt.Rows[i]["Id"]) == 46 || Conversion.ToInt(dt.Rows[i]["Id"]) == 47)
					{
						dttype.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["LookupName"]);
					}
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbProductionType, dttype, "Id", "ProductionName", "Production Type", AllColumns: true, Conversion.ToInt(CmbProductionType.Value), insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BOMNoDbCall()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			dtBOMDataforCombo.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = 1851;
			dtBOMDataforCombo = BomHeader.GetAllBomNoAndItems(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BomNoFill()
	{
		try
		{
			DataTable dtWO = new DataTable();
			dtWO.Columns.Add("Id", typeof(int));
			dtWO.Columns.Add("BomNo", typeof(string));
			dtWO.Columns.Add("TopForce", typeof(decimal));
			dtWO.Columns.Add("BottomForce", typeof(decimal));
			DataTable dataTable = dtBOMDataforCombo;
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				HashSet<int> Ids = new HashSet<int>();
				foreach (DataRow dr in dtBOMDataforCombo.Rows)
				{
					int Id = Conversion.ToInt(dr["Id"]);
					if (Ids.Add(Id))
					{
						dtWO.Rows.Add(Id, dr["BomName"], dr["TopForce"], dr["BottomForce"]);
					}
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbBOM, dtWO, "Id", "BomNo", "Bom No", AllColumns: true, Conversion.ToInt(CmbBOM.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private DataTable PlantFeaderDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return InvProductionPlant.GetAll(new InvProductionPlant
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

	private void PlantBind(DataTable dtProductionplant)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbPlantName, dtProductionplant, "Id", "Description", "Plant", AllColumns: false, Conversion.ToInt(CmbPlantName.Value), insertDefaultRow: false, null, null, ActivateRow: false);
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
			List<getGlobalAllSupplierCustomer> customers = clsGlobalVariables.globalAllSupplierCustomer;
			if (customers == null || customers.Count == 0)
			{
				return;
			}
			foreach (getGlobalAllSupplierCustomer item in customers)
			{
				object[] rowData = new object[6] { item.Id, item.CompanyName, item.PartyCode, item.GlAccountId, item.CityName, item.MobilePersonal };
				dtSupplier.Rows.Add(rowData);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RefPartydtDbCall()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			dtRefParty.Clear();
			ReferenceParties obj = new ReferenceParties();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.ReferencePartyTypeId = 1;
			dtRefParty = ReferenceParties.ReferencePArtyByReferencePartyTypeIdOrSupplierCustomerId(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void SupplierdtFillFromRefPartydt()
	{
		try
		{
			dtSupplier.Clear();
			DataTable dataTable = dtRefParty;
			if (dataTable != null && dataTable.Rows.Count == 0)
			{
				return;
			}
			foreach (DataRow item in dtRefParty.Rows)
			{
				dtSupplier.Rows.Add(item["Id"], item["ReferencePartyName"]);
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
			InfragisticsHelper.BindAndRetainSelection(CmbCustomerName, dtSupplier, "Id", "CompanyName", "SupplierName", AllColumns: true, Conversion.ToInt(CmbCustomerName.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dtSupplier.Rows.Count > 0)
			{
				((UltraGridBase)CmbCustomerName).DisplayLayout.Bands[0].Columns[3].Hidden = true;
			}
			if (!RadCustomers.Checked)
			{
				((UltraGridBase)CmbCustomerName).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbCustomerName).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				((UltraGridBase)CmbCustomerName).DisplayLayout.Bands[0].Columns[5].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCustomerName_Leave(object sender, EventArgs e)
	{
		try
		{
			if (RadCustomers.Enabled)
			{
				int CustomerId = Conversion.ToInt(CmbCustomerName.Value);
				BindCustomerOrders(CustomerId);
			}
			CmbCustomerOrderNo_ValueChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindCustomerOrders(int CustomerId)
	{
		try
		{
			dtCustomerOrderDetailWithItem.Clear();
			if (CustomerId > 0)
			{
				dtCustomerOrderDetailWithItem = SaleOrder.SaleOrder_GetOrderInfo(UserAccount.OrganizationId, UserAccount.CompanyId, 0, CustomerId);
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("OrderId", typeof(int));
			dt.Columns.Add("OrderNo", typeof(int));
			HashSet<int> existingCustomerIds = new HashSet<int>();
			foreach (DataRow r in dtCustomerOrderDetailWithItem.Rows)
			{
				int OrderId = Conversion.ToInt(r["OrderId"]);
				if (existingCustomerIds.Add(OrderId))
				{
					dt.Rows.Add(OrderId, r["OrderNo"]);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbCustomerOrderNo, dt, "OrderId", "OrderNo", "OrderNo", AllColumns: true, Conversion.ToInt(CmbCustomerOrderNo.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCustomerOrderNo_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			int OrderId = Conversion.ToInt(CmbCustomerOrderNo.Value);
			int ProductionTypeId = Conversion.ToInt(CmbProductionType.Value);
			if (OrderId > 0)
			{
				ItemDtsFillForOrder(OrderId, ProductionTypeId);
				ItemNameBind();
				if (((UltraGridBase)CmbItemDetail).DataSource != null)
				{
					((UltraGridBase)CmbItemDetail).DisplayLayout.Bands[0].Columns[11].Hidden = false;
					((UltraGridBase)CmbItemDetail).DisplayLayout.Bands[0].Columns[12].Hidden = false;
					((UltraGridBase)CmbItemDetail).DisplayLayout.Bands[0].Columns[13].Hidden = false;
				}
			}
			else
			{
				ItemDtsFillFromGlobal(ProductionTypeId);
				ItemNameBind();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDtsFillFromGlobal(int parentTypeId)
	{
		try
		{
			dtItem.Clear();
			dtItemInput.Clear();
			List<getGlobalAllItems> items = clsGlobalVariables.getGlobalAllItems;
			if (items == null || items.Count == 0)
			{
				return;
			}
			bool filterByParent = parentTypeId > 0;
			foreach (getGlobalAllItems row in items)
			{
				bool isExcluded = row.ItemTypeOfTypeId == 14 || row.ItemTypeOfTypeId == 17;
				if (!isExcluded)
				{
					dtItemInput.Rows.Add(row.Id, row.ItemName, row.ItemCode, row.ItemCategoryId, row.ItemCategory, row.ItemTypeId, row.ItemType, row.ItemTypeOfTypeId, row.ItemTypeOfType);
				}
				if ((filterByParent && (row.ItemTypeOfTypeId == parentTypeId || !isExcluded)) || (!filterByParent && !isExcluded))
				{
					dtItem.Rows.Add(row.Id, row.ItemName, row.ItemCode, row.ItemCategoryId, row.ItemCategory, row.ItemTypeId, row.ItemType, row.ItemTypeOfTypeId, row.ItemTypeOfType);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error loading items: " + ex.Message);
		}
	}

	private void ItemDtsFillForOrder(int OrderId, int ParentTypeId)
	{
		try
		{
			dtItem.Clear();
			EnumerableRowCollection<DataRow> allItems = from item in dtCustomerOrderDetailWithItem.AsEnumerable()
				where item.Field<int>("OrderId") == OrderId && item.Field<int>("ItemTypeOfTypeId") == ParentTypeId
				select item;
			if (allItems == null || !allItems.Any())
			{
				return;
			}
			HashSet<int> existingCustomerIds = new HashSet<int>();
			foreach (DataRow row in allItems)
			{
				int ItemId = Conversion.ToInt(row["ItemId"]);
				if (existingCustomerIds.Add(ItemId))
				{
					dtItem.Rows.Add(row["ItemId"], row["ItemName"], row["ItemCode"], row["ItemCategoryId"], row["ItemCategory"], row["ItemTypeId"], row["ItemType"], row["ItemTypeOfTypeId"], row["ItemTypeOfType"], row["OrderDetailId"], row["ItemAttributeVarientId"], row["ItemAttribute"], row["VarientEquivalent"], row["OrderItemQty"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error loading items: " + ex.Message);
		}
	}

	private void ItemNameBind()
	{
		try
		{
			int Id = Conversion.ToInt(CmbItemDetail.Value);
			string ValueMember = (rdSearchByName.Checked ? "ItemName" : "ItemCode");
			string DisplayMember = (rdSearchByName.Checked ? "Item Name" : "Item Code");
			InfragisticsHelper.BindAndRetainSelection(CmbItemDetail, dtItem, "Id", ValueMember, DisplayMember, AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: false);
			if (dtItem.Rows.Count > 0)
			{
				((UltraGridBase)CmbItemDetail).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbItemDetail).DisplayLayout.Bands[0].Columns[5].Hidden = true;
				((UltraGridBase)CmbItemDetail).DisplayLayout.Bands[0].Columns[7].Hidden = true;
				((UltraGridBase)CmbItemDetail).DisplayLayout.Bands[0].Columns[9].Hidden = true;
				((UltraGridBase)CmbItemDetail).DisplayLayout.Bands[0].Columns[10].Hidden = true;
				((UltraGridBase)CmbItemDetail).DisplayLayout.Bands[0].Columns[11].Hidden = true;
				((UltraGridBase)CmbItemDetail).DisplayLayout.Bands[0].Columns[12].Hidden = true;
				((UltraGridBase)CmbItemDetail).DisplayLayout.Bands[0].Columns[13].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbItemDetail_Leave(object sender, EventArgs e)
	{
		try
		{
			int ItemId = Conversion.ToInt(CmbItemDetail.Value);
			if (ItemId > 0)
			{
				if (Conversion.ToInt(CmbCustomerOrderNo.Value) == 0)
				{
					AttributeVarientFill(ItemId);
				}
				else
				{
					double Qty = Conversion.ToDouble(((UltraDropDownBase)CmbItemDetail).SelectedRow.Cells["OrderItemQty"].Value);
					int Id = Conversion.ToInt(((UltraDropDownBase)CmbItemDetail).SelectedRow.Cells["ItemAttributeVarientId"].Value);
					string Code = Conversion.ToString(((UltraDropDownBase)CmbItemDetail).SelectedRow.Cells["ItemAttribute"].Value);
					double Equivalent = Conversion.ToDouble(((UltraDropDownBase)CmbItemDetail).SelectedRow.Cells["VarientEquivalent"].Value);
					DataTable dtVarient = new DataTable();
					dtVarient.Columns.Add("ItemAttributeVarientId", typeof(int));
					dtVarient.Columns.Add("ItemAttribute", typeof(string));
					dtVarient.Columns.Add("VarientEquivalent", typeof(double));
					dtVarient.Clear();
					dtVarient.Rows.Add(Id, Code, Equivalent);
					InfragisticsHelper.BindAndRetainSelection(CmbAttribute, dtVarient, "ItemAttributeVarientId", "ItemAttribute", "Item Attribute", AllColumns: true, Conversion.ToInt(CmbAttribute.Value), insertDefaultRow: false, null, null, ActivateRow: false);
					CmbAttribute_Leave(null, null);
					txtCustomerOrderQty.Text = Qty.ToString("#,##.##");
				}
			}
			DataTable dtUom = CommonServices.dtUomFromGloablUomScheduleByItemId(ItemId);
			InfragisticsHelper.BindAndRetainSelection(cmbItemUOM, dtUom, "Id", "UOMCode", "Uom", AllColumns: true, Conversion.ToInt(cmbItemUOM.Value), insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AttributeVarientFill(int ItemId)
	{
		try
		{
			dtAttributes.Clear();
			dtAttributes = ItemAttributeVarient.GetAllForCombo(UserAccount.OrganizationId, UserAccount.CompanyId, ItemId);
			InfragisticsHelper.BindAndRetainSelection(CmbAttribute, dtAttributes, "ItemAttributeVarientId", "ItemAttribute", "Item Attribute", AllColumns: true, Conversion.ToInt(CmbAttribute.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			CmbAttribute_Leave(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbAttribute_Leave(object sender, EventArgs e)
	{
		try
		{
			double Equivalent = 0.0;
			if (Conversion.ToInt(CmbAttribute.Value) > 0)
			{
				Equivalent = Conversion.ToDouble(((UltraDropDownBase)CmbAttribute).SelectedRow.Cells["VarientEquivalent"].Value);
				txtVarientEquivalent.Text = Equivalent.ToString();
			}
			else
			{
				txtVarientEquivalent.Text = Equivalent.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbProductionType_Leave(object sender, EventArgs e)
	{
		try
		{
			CmbCustomerOrderNo_ValueChanged(null, null);
			EnableDisableProductionType();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EnableDisableProductionType()
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdCustomerDetail.GetDataRows().Count() > 0)
			{
				GridEXRow r = (from rr in grdCustomerDetail.GetDataRows()
					where updateDetailIndex == -1 || rr.RowIndex != updateDetailIndex
					select rr).FirstOrDefault();
				if (r != null && (int)r.RowType == 0 && Conversion.ToInt(r.Cells["ItemId"].Value) > 0 && Conversion.ToInt(r.Cells["ItemQty"].Value) > 0)
				{
					((Control)(object)CmbProductionType).Enabled = false;
				}
				else
				{
					((Control)(object)CmbProductionType).Enabled = true;
				}
			}
			else
			{
				((Control)(object)CmbProductionType).Enabled = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void RadCustomers_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)CmbCustomerOrderNo).Enabled = RadCustomers.Checked;
			((Control)(object)CmbCustomerOrderNo).Text = (RadCustomers.Checked ? ((Control)(object)CmbCustomerOrderNo).Text : "");
			if (RadCustomers.Checked)
			{
				SupplierDtFillFromGlobal();
			}
			else
			{
				SupplierdtFillFromRefPartydt();
			}
			BindSupplierName();
			CmbCustomerName_Leave(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidationCustomerDetail()
	{
		if (!FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
		{
			((Control)(object)CmbProductionType, "Production Type", FormHelper.ValidType.INT),
			((Control)(object)CmbCustomerName, "Customer", FormHelper.ValidType.String),
			((Control)(object)CmbItemDetail, "Item Name", FormHelper.ValidType.INT),
			((Control)(object)cmbItemUOM, "UOM", FormHelper.ValidType.INT),
			((Control)(object)CmbAttribute, "Item Attribute", FormHelper.ValidType.INT),
			(txtVarientEquivalent, "Variant Equivalent", FormHelper.ValidType.Double),
			(txtCustomerOrderQty, "Qty", FormHelper.ValidType.Double)
		}))
		{
			return false;
		}
		return true;
	}

	private void BtnAddCustomer_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationCustomerDetail())
			{
				int UomId = Conversion.ToInt(cmbItemUOM.Value);
				string UomCode = "";
				decimal UomEquivalent = default(decimal);
				if (((UltraGridBase)cmbItemUOM).ActiveRow != null && UomId > 0)
				{
					UomCode = Conversion.ToString(((UltraDropDownBase)cmbItemUOM).SelectedRow.Cells["UOMCode"].Value);
					UomEquivalent = Conversion.ToDecimal(((UltraDropDownBase)cmbItemUOM).SelectedRow.Cells["Equivalent"].Value);
				}
				int ItemId = Conversion.ToInt(CmbItemDetail.Value);
				string ItemName = "";
				string ItemCode = "";
				int OrderDetailId = 0;
				if (((UltraGridBase)CmbItemDetail).ActiveRow != null && ItemId > 0)
				{
					ItemName = Conversion.ToString(((UltraDropDownBase)CmbItemDetail).SelectedRow.Cells["ItemName"].Value);
					ItemCode = Conversion.ToString(((UltraDropDownBase)CmbItemDetail).SelectedRow.Cells["ItemCode"].Value);
					OrderDetailId = Conversion.ToInt(((UltraDropDownBase)CmbItemDetail).SelectedRow.Cells["OrderDetailId"].Value);
				}
				int CustomerTypeId = (RadCustomers.Checked ? 1 : 2);
				string CustomerType = (RadCustomers.Checked ? "Customer" : "Ref Party");
				dtCustomerDetail.Rows.Add(0, CustomerTypeId, CustomerType, Conversion.ToInt(CmbCustomerName.Value), Conversion.ToString((object)((Control)(object)CmbCustomerName).Text.Trim()), Conversion.ToInt(CmbCustomerOrderNo.Value), Conversion.ToInt((object)((Control)(object)CmbCustomerOrderNo).Text.Trim()), OrderDetailId, ItemId, ItemCode, ItemName, UomId, UomCode, UomEquivalent, Conversion.ToInt(CmbAttribute.Value), Conversion.ToString((object)((Control)(object)CmbAttribute).Text), Conversion.ToDecimal((object)txtVarientEquivalent.Text.Trim()), Conversion.ToDecimal((object)txtCustomerOrderQty.Text.Trim()), Conversion.ToDecimal((object)txtCustomerOrderQty.Text) * Conversion.ToDecimal((object)txtVarientEquivalent.Text), Conversion.ToDecimal((object)txtCustomerOrderQty.Text.Trim()), Conversion.ToString((object)txtReferenceNoDetail.Text), Conversion.ToString((object)textBox1.Text));
				grdCustomerDetail.DataSource = dtCustomerDetail;
				grdCustomerDetail.RetrieveStructure();
				grdCustomerDetailSetting();
				ResetCustomerDetail();
				EnableDisableProductionType();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUpdateCustmer_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationCustomerDetail())
			{
				int UomId = Conversion.ToInt(cmbItemUOM.Value);
				string UomCode = "";
				decimal UomEquivalent = default(decimal);
				if (((UltraGridBase)cmbItemUOM).ActiveRow != null && UomId > 0)
				{
					UomCode = Conversion.ToString(((UltraDropDownBase)cmbItemUOM).SelectedRow.Cells["UOMCode"].Value);
					UomEquivalent = Conversion.ToDecimal(((UltraDropDownBase)cmbItemUOM).SelectedRow.Cells["Equivalent"].Value);
				}
				int ItemId = Conversion.ToInt(CmbItemDetail.Value);
				string ItemName = "";
				string ItemCode = "";
				int OrderDetailId = 0;
				if (((UltraGridBase)CmbItemDetail).ActiveRow != null && ItemId > 0)
				{
					ItemName = Conversion.ToString(((UltraDropDownBase)CmbItemDetail).SelectedRow.Cells["ItemName"].Value);
					ItemCode = Conversion.ToString(((UltraDropDownBase)CmbItemDetail).SelectedRow.Cells["ItemCode"].Value);
					OrderDetailId = Conversion.ToInt(((UltraDropDownBase)CmbItemDetail).SelectedRow.Cells["OrderDetailId"].Value);
				}
				dtCustomerDetail.Rows[updateDetailIndex]["CustomerTypeId"] = (RadCustomers.Checked ? 1 : 2);
				dtCustomerDetail.Rows[updateDetailIndex]["CustomerType"] = (RadCustomers.Checked ? "Customer" : "Ref Party");
				dtCustomerDetail.Rows[updateDetailIndex]["CustomerId"] = Conversion.ToInt(CmbCustomerName.Value);
				dtCustomerDetail.Rows[updateDetailIndex]["CustomerName"] = Conversion.ToString((object)((Control)(object)CmbCustomerName).Text);
				dtCustomerDetail.Rows[updateDetailIndex]["OrderId"] = Conversion.ToInt(CmbCustomerOrderNo.Value);
				dtCustomerDetail.Rows[updateDetailIndex]["OrderNo"] = Conversion.ToInt((object)((Control)(object)CmbCustomerOrderNo).Text);
				dtCustomerDetail.Rows[updateDetailIndex]["OrderDetailId"] = OrderDetailId;
				dtCustomerDetail.Rows[updateDetailIndex]["ItemId"] = ItemId;
				dtCustomerDetail.Rows[updateDetailIndex]["ItemCode"] = ItemCode;
				dtCustomerDetail.Rows[updateDetailIndex]["ItemName"] = ItemName;
				dtCustomerDetail.Rows[updateDetailIndex]["ItemUomId"] = UomId;
				dtCustomerDetail.Rows[updateDetailIndex]["UomCode"] = UomCode;
				dtCustomerDetail.Rows[updateDetailIndex]["UomEquivalent"] = UomEquivalent;
				dtCustomerDetail.Rows[updateDetailIndex]["ItemAttributeVarientId"] = Conversion.ToInt(CmbAttribute.Value);
				dtCustomerDetail.Rows[updateDetailIndex]["AttributeVarient"] = Conversion.ToString((object)((Control)(object)CmbAttribute).Text);
				dtCustomerDetail.Rows[updateDetailIndex]["VarientEquivalent"] = Conversion.ToDecimal((object)txtVarientEquivalent.Text.Trim());
				dtCustomerDetail.Rows[updateDetailIndex]["ItemQty"] = Conversion.ToDecimal((object)txtCustomerOrderQty.Text.Trim());
				dtCustomerDetail.Rows[updateDetailIndex]["VarientQty"] = Conversion.ToDecimal((object)txtCustomerOrderQty.Text) * Conversion.ToDecimal((object)txtVarientEquivalent.Text);
				dtCustomerDetail.Rows[updateDetailIndex]["NetWeight"] = Conversion.ToDecimal((object)txtCustomerOrderQty.Text.Trim());
				dtCustomerDetail.Rows[updateDetailIndex]["ReferenceNo"] = Conversion.ToString((object)txtReferenceNoDetail.Text);
				dtCustomerDetail.Rows[updateDetailIndex]["Remarks"] = Conversion.ToString((object)textBox1.Text);
				grdCustomerDetail.DataSource = dtCustomerDetail;
				ResetCustomerDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnCancelCustomer_Click(object sender, EventArgs e)
	{
		try
		{
			ResetCustomerDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdCustomerDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow Customer = grdCustomerDetail.CurrentRow;
			if (Customer != null)
			{
				updateDetailIndex = Customer.RowIndex;
				RadCustomers.Checked = Conversion.ToInt(Customer.Cells["CustomerTypeId"].Value) == 1;
				((Control)(object)CmbCustomerName).Text = Conversion.ToString(Customer.Cells["CustomerName"].Value);
				CmbCustomerName_Leave(null, null);
				CmbCustomerOrderNo.Value = Conversion.ToInt(Customer.Cells["OrderId"].Value);
				CmbItemDetail.Value = Conversion.ToInt(Customer.Cells["ItemId"].Value);
				CmbItemDetail_Leave(null, null);
				cmbItemUOM.Value = Conversion.ToInt(Customer.Cells["ItemUomId"].Value);
				CmbAttribute.Value = Conversion.ToInt(Customer.Cells["ItemAttributeVarientId"].Value);
				CmbAttribute_Leave(null, null);
				txtCustomerOrderQty.Text = Conversion.ToDecimal(Customer.Cells["ItemQty"].Value).ToString("#,##.###");
				txtReferenceNoDetail.Text = Conversion.ToString(Customer.Cells["ReferenceNo"].Value);
				textBox1.Text = Conversion.ToString(Customer.Cells["Remarks"].Value);
				BtnAddCustomer.Visible = false;
				BtnUpdateCustmer.Visible = true;
				BtnCancelCustomer.Visible = true;
			}
			EnableDisableProductionType();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdCustomerDetailSetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string>
			{
				"Id", "CustomerTypeId", "CustomerId", "OrderId", "OrderDetailId", "ItemId", "ItemUomId", "UomEquivalent", "ItemAttributeVarientId", "VarientEquivalent",
				"NetWeight"
			};
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"ItemQty",
					(EditType)1
				},
				{
					"ReferenceNo",
					(EditType)1
				},
				{
					"Remarks",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdCustomerDetail, 2, 3, hiddenColumns, editableColumns);
			grdCustomerDetail.RootTable.Columns["CustomerType"].Width = 80;
			grdCustomerDetail.RootTable.Columns["CustomerName"].Width = Constants.InventoryConstants.SupplierName;
			grdCustomerDetail.RootTable.Columns["OrderNo"].Width = Constants.InventoryConstants.OrderNo;
			grdCustomerDetail.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			GridEX_Helper.AddButton(grdCustomerDetail, "Delete", "X", 20, 0);
			GridEX_Helper.AddButton(grdCustomerDetail, "Add", "+", 20, 1);
			GridEX_Helper.AddButton(grdCustomerDetail, "Edit", "Edit", 40, 2);
			grdCustomerDetail.FrozenColumns = 3;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdCustomerDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grdCustomerDetail.UpdateData();
			GridEXRow Customer = grdCustomerDetail.CurrentRow;
			if (Customer != null)
			{
				if (e.Column.Key == "ItemQty")
				{
					decimal VarientEquivalent = Conversion.ToDecimal(Customer.Cells["VarientEquivalent"].Value);
					decimal Qty = Conversion.ToDecimal(Customer.Cells["ItemQty"].Value);
					Customer.Cells["VarientQty"].Value = Qty * VarientEquivalent;
				}
				grdCustomerDetail.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdCustomerDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow r = grdCustomerDetail.CurrentRow;
			if (r != null)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteCustomerDetailRow(r);
				}
				if (e.Column.Key == "Edit")
				{
					grdCustomerDetail_DoubleClick(null, null);
				}
				if (e.Column.Key == "Add")
				{
					AddCustomerDetailRow(r);
				}
				grdCustomerDetail.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddCustomerDetailRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (r != null && (int)r.RowType == 0)
			{
				DataRow dr = dtCustomerDetail.Rows[r.RowIndex];
				dtCustomerDetail.Rows.Add(dr.ItemArray);
				if (btnUpdate.Visible && btnUpdate.Enabled)
				{
					dtCustomerDetail.Rows[dtCustomerDetail.Rows.Count - 1]["Id"] = 0;
				}
				grdCustomerDetail.DataSource = dtCustomerDetail;
			}
			EnableDisableProductionType();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteCustomerDetailRow(GridEXRow r)
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
					WorkOrderCustomerDetail d = new WorkOrderCustomerDetail();
					d.WorkOrderCustomerDetailId = Conversion.ToInt(r.Cells["Id"].Value);
					FillDetailListCommonForInsertAndDelete(d, r);
					d.ActionTypeId = 3;
					lstRemoveRecordcustomerDetail.Add(d);
					dtCustomerDetail.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				else
				{
					dtCustomerDetail.Rows[r.RowIndex].Delete();
					r.Delete();
				}
			}
			EnableDisableProductionType();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInInputGrid()
	{
		dtInput.Rows.Add(0, 0, "", 0, 0, "", 0, 0, "", 0, 0, "");
		grdInputMaterial.DataSource = dtInput;
	}

	private void CmbBOM_Leave(object sender, EventArgs e)
	{
		try
		{
			int bomId = Conversion.ToInt(CmbBOM.Value);
			int count = grdInputMaterial.GetDataRows().Count();
			if (bomId == 0 && PreviousbomId == 0)
			{
				txtTopForce.Text = "";
				txtBottomForce.Text = "";
				dtInput.Clear();
				grdInputMaterial.DataSource = null;
				AddRowInInputGrid();
			}
			else if (bomId > 0 && PreviousbomId == 0)
			{
				PreviousbomId = bomId;
				FillInputDetailForBom(bomId);
				CellsCollection row = ((UltraDropDownBase)CmbBOM).SelectedRow.Cells;
				txtTopForce.Text = Conversion.ToString(row["TopForce"].Value);
				txtBottomForce.Text = Conversion.ToString(row["BottomForce"].Value);
			}
			else if (bomId != PreviousbomId && count > 0)
			{
				if (!FormHelper.ConfirmAction("Are you Sure To change BomNo??? Previous Data Of InputPut grid Will be Reset"))
				{
					CmbBOM.Value = PreviousbomId;
					return;
				}
				dtInput.Clear();
				grdInputMaterial.DataSource = null;
				PreviousbomId = bomId;
				if (bomId > 0)
				{
					FillInputDetailForBom(bomId);
					CellsCollection row2 = ((UltraDropDownBase)CmbBOM).SelectedRow.Cells;
					txtTopForce.Text = Conversion.ToString(row2["TopForce"].Value);
					txtBottomForce.Text = Conversion.ToString(row2["BottomForce"].Value);
				}
				else
				{
					txtTopForce.Text = "";
					txtBottomForce.Text = "";
					AddRowInInputGrid();
				}
			}
			else if (count == 0)
			{
				CellsCollection row3 = ((UltraDropDownBase)CmbBOM).SelectedRow.Cells;
				txtTopForce.Text = Conversion.ToString(row3["TopForce"].Value);
				txtBottomForce.Text = Conversion.ToString(row3["BottomForce"].Value);
				FillInputDetailForBom(bomId);
				PreviousbomId = bomId;
			}
			EnableDisableProductionType();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillInputDetailForBom(int bomId)
	{
		try
		{
			BomHeader bomHeader = BomHeader.ReadById(bomId);
			if (((bomHeader != null) ? bomHeader.BomInputDetailList : null) == null || bomHeader.BomInputDetailList.Count == 0)
			{
				grdInputMaterial.ClearStructure();
				EnableDisableProductionType();
				return;
			}
			HashSet<int> existingInputIds = new HashSet<int>(from r in grdInputMaterial.GetDataRows()
				select Conversion.ToInt(r.Cells["BomInputId"].Value));
			foreach (BomInputDetail input in bomHeader.BomInputDetailList)
			{
				if (!existingInputIds.Contains(input.Id))
				{
					dtInput.Rows.Add(0, input.BomHeaderId, input.BomHeaderNo, input.Id, input.ItemId, input.ItemName, input.ItemUomId, input.PackUomEquivalent, input.PackUom, input.ItemQty, input.WeightKg, "");
				}
			}
			grdInputMaterial.DataSource = dtInput;
			grdInputMaterial.RetrieveStructure();
			grdInputSettings();
			EnableDisableProductionType();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdInputSettings()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "BomHeaderId", "BomInputId", "ItemId", "ItemUomId", "Uom", "NetWeight" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"ItemQty",
					(EditType)1
				},
				{
					"Remarks",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdInputMaterial, 2, 3, hiddenColumns, editableColumns);
			grdInputMaterial.RootTable.Columns["UomCode"].Caption = "Uom";
			grdInputMaterial.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdInputMaterial.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			GridEX_Helper.AddButton(grdInputMaterial, "Delete", "X", 20, 0);
			GridEX_Helper.AddButton(grdInputMaterial, "Add", "+", 20, 1);
			grdInputMaterial.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInputMaterial_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdInputMaterial.UpdateData();
			GridEXRow item = grdInputMaterial.CurrentRow;
			if (e.Column.Key == "ItemName")
			{
				item.Cells["ItemUomId"].Value = 0;
				item.Cells["Uom"].Value = 0;
				item.Cells["UomCode"].Value = "";
				item.Cells["NetWeight"].Value = 0;
			}
			if ((e.Column.Key == "ItemQty" || e.Column.Key == "Uom" || e.Column.Key == "ItemUomId") && item != null && (int)item.RowType == 0)
			{
				if (Conversion.ToDouble(item.Cells["ItemQty"].Value) != 0.0 && item.Cells["ItemUomId"].Value.ToString() != string.Empty)
				{
					decimal num = Conversion.ToDecimal((object)item.Cells["ItemQty"].Value.ToString());
					decimal quom = Conversion.ToDecimal((object)item.Cells["Uom"].Value.ToString());
					decimal Weight = num * quom;
					item.Cells["NetWeight"].Value = Weight;
				}
				else
				{
					item.Cells["NetWeight"].Value = 0;
				}
			}
			grdInputMaterial.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInputMaterial_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdInputMaterial.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteInputDetailRow(r);
				}
				if (e.Column.Key == "Add")
				{
					AddRowInInputGrid();
				}
				grdInputMaterial.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteInputDetailRow(GridEXRow r)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				dtInput.Rows[r.RowIndex].Delete();
				r.Delete();
			}
			else
			{
				r.Delete();
			}
			if (dtInput.Rows.Count == 0)
			{
				AddRowInInputGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInputMaterial_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grdInputMaterial.CurrentRow;
			GridEXColumn col = grdInputMaterial.CurrentColumn;
			if (row == null || (int)row.RowType != 0 || col == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (col.Key == "Add")
				{
					AddRowInInputGrid();
				}
				else if (col.Key == "Delete")
				{
					DeleteInputDetailRow(row);
				}
				grdInputMaterial.UpdateData();
			}
			else if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInInputGrid();
				grdInputMaterial.UpdateData();
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteInputDetailRow(row);
				grdInputMaterial.UpdateData();
			}
			else if (e.KeyCode == Keys.F1)
			{
				HandleF1Popup(row, col.Key);
				grdInputMaterial.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HandleF1Popup(GridEXRow row, string colKey)
	{
		if (colKey == "ItemName")
		{
			int prevItemId = Conversion.ToInt(row.Cells["ItemId"].Value);
			GrdPopUp grdPopUp = new GrdPopUp(dtItemInput, "Id", "ItemName");
			grdPopUp.StartPosition = FormStartPosition.CenterScreen;
			grdPopUp.ShowDialog();
			int id = Conversion.ToInt((object)grdPopUp.ReturnId);
			string nameCode = Conversion.ToString((object)grdPopUp.ReturnName);
			row.Cells["ItemId"].Value = id;
			row.Cells["ItemName"].Value = nameCode;
			if (id != prevItemId)
			{
				row.Cells["ItemUomId"].Value = 0;
				row.Cells["Uom"].Value = 0;
				row.Cells["UomCode"].Value = "";
				row.Cells["NetWeight"].Value = 0;
			}
		}
		else if (colKey == "UomCode")
		{
			GrdPopUp grdPopUp2 = new GrdPopUp(CommonServices.dtUomFromGloablUomScheduleByItemId(Conversion.ToInt(row.Cells["ItemId"].Value)), "Id", "UOMCode", "Equivalent");
			grdPopUp2.StartPosition = FormStartPosition.CenterScreen;
			grdPopUp2.ShowDialog();
			int id = Conversion.ToInt((object)grdPopUp2.ReturnId);
			string nameCode = Conversion.ToString((object)grdPopUp2.ReturnName);
			decimal equivalent = Conversion.ToDecimal((object)grdPopUp2.ReturnCode);
			row.Cells["ItemUomId"].Value = id;
			row.Cells["Uom"].Value = equivalent;
			row.Cells["UomCode"].Value = nameCode;
			decimal qty = Conversion.ToDecimal(row.Cells["ItemQty"].Value ?? ((object)0));
			row.Cells["NetWeight"].Value = qty * equivalent;
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		FromReset();
	}

	private void FromReset()
	{
		try
		{
			RecId = 0;
			AT = new Attachment();
			Attachment.dtAttachments.Clear();
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			lstRemoveRecordcustomerDetail.Clear();
			PreviousbomId = 0;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			btnSaveAs.Visible = false;
			btnDelete.Visible = false;
			UpdateMode = false;
			txtWorkOrderNo.Text = string.Empty;
			txtBatchDesc.Text = string.Empty;
			txtRemarksHeader.Text = string.Empty;
			((Control)(object)CmbBOM).Text = string.Empty;
			txtTopForce.Text = string.Empty;
			txtBottomForce.Text = string.Empty;
			ResetCustomerDetail();
			((Control)(object)CmbCustomerName).Text = string.Empty;
			((Control)(object)CmbItemDetail).Text = string.Empty;
			((Control)(object)cmbItemUOM).Text = string.Empty;
			((Control)(object)CmbAttribute).Text = string.Empty;
			((UltraGridBase)CmbAttribute).DataSource = null;
			txtVarientEquivalent.Text = string.Empty;
			txtReferenceNoDetail.Text = string.Empty;
			dtCustomerDetail.Rows.Clear();
			grdCustomerDetail.DataSource = null;
			dtInput.Rows.Clear();
			grdInputMaterial.DataSource = null;
			txtdocno.Text = DocumentNoDbCall();
			EnableDisableProductionType();
			txtdocdate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetCustomerDetail()
	{
		try
		{
			((Control)(object)CmbCustomerOrderNo).Text = string.Empty;
			txtCustomerOrderQty.Text = string.Empty;
			textBox1.Text = string.Empty;
			updateDetailIndex = -1;
			BtnAddCustomer.Visible = true;
			BtnCancelCustomer.Visible = false;
			BtnUpdateCustmer.Visible = false;
			CmbCustomerName.Focus();
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
			List<string> allServices = new List<string> { "UomSchedule", "SupplierCustomerLists", "AllItems" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			RefPartydtDbCall();
			if (RadCustomers.Checked)
			{
				SupplierDtFillFromGlobal();
			}
			else
			{
				SupplierdtFillFromRefPartydt();
			}
			BindSupplierName();
			BindCustomerOrders(Conversion.ToInt(CmbCustomerName.Value));
			int ProductionTypeId = Conversion.ToInt(CmbProductionType.Value);
			ItemDtsFillFromGlobal(ProductionTypeId);
			PlantBind(PlantFeaderDbCall());
			ItemNameBind();
			BOMNoDbCall();
			BomNoFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmWorkOrderConcrete_FormClosed(object sender, FormClosedEventArgs e)
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

	private void Insert()
	{
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Expected O, but got Unknown
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_039e: Expected O, but got Unknown
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Expected O, but got Unknown
		try
		{
			if (grdCustomerDetail.GetRows().Count() == 0)
			{
				throw new Exception("Grid Record Not Found");
			}
			if (!FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
			{
				(txtdocno, "Doc No", FormHelper.ValidType.INT),
				((Control)(object)CmbProductionType, "Production Type", FormHelper.ValidType.INT),
				((Control)(object)CmbPlantName, "Plant", FormHelper.ValidType.INT),
				(txtWorkOrderNo, "Work Order No", FormHelper.ValidType.String)
			}))
			{
				return;
			}
			if (txtEndDate.Value.Date < txtStartdate.Value.Date)
			{
				MessageBox.Show("End Date cannot be earlier than Start Date.");
				txtEndDate.Focus();
			}
			else if (txtDeadLineDateMain.Value < txtEndDate.Value)
			{
				MessageBox.Show("Deadline must be greater than or equal to End Date.");
				txtDeadLineDateMain.Focus();
			}
			else
			{
				if (!FormHelper.ConfirmAction((RecId == 0) ? "Are you sure to Save" : "Are you sure to Update"))
				{
					return;
				}
				WorkOrderHeader Or = new WorkOrderHeader();
				Or.WorkOrderHeaderId = RecId;
				Or.CompanyId = UserAccount.CompanyId;
				Or.BranchesId = UserAccount.BranchesId;
				Or.ProjectsId = UserAccount.BranchesId;
				Or.OrganizationId = UserAccount.OrganizationId;
				Or.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				Or.DocumentTypeId = DocumentTypeId;
				Or.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
				Or.DocNo = Conversion.ToInt((object)txtdocno.Text);
				Or.ProductionTypeId = Conversion.ToInt(CmbProductionType.Value);
				Or.PlantId = Conversion.ToInt(CmbPlantName.Value);
				Or.BomHeaderId = Conversion.ToInt(CmbBOM.Value);
				Or.TopForce = Conversion.ToDecimal((object)txtTopForce.Text);
				Or.BottomForce = Conversion.ToDecimal((object)txtBottomForce.Text);
				Or.WorkOrderNo = txtWorkOrderNo.Text;
				Or.BatchDesc = txtBatchDesc.Text;
				Or.RemarksHeader = Conversion.ToString((object)txtRemarksHeader.Text);
				Or.StartDate = Conversion.ToDateTime((object)txtStartdate.Value);
				Or.EndDate = Conversion.ToDateTime((object)txtEndDate.Value);
				Or.DeadLine = Conversion.ToDateTime((object)txtDeadLineDateMain.Value);
				Or.EntryUserId = UserAccount.ID;
				Or.EntryDate = DateTime.Now;
				Or.ModifyDate = DateTime.Now;
				Or.ModifyUserId = UserAccount.ID;
				Or.ApprovedUserId = UserAccount.ID;
				Or.ApprovedDate = DateTime.Now;
				Or.IsApproved = false;
				Or.WorkOrderCustomerDetailList = new List<WorkOrderCustomerDetail>();
				Or.WorkOrderInputDetailList = new List<WorkOrderInputDetail>();
				if (lstRemoveRecordcustomerDetail.Count > 0 && RecId > 0)
				{
					for (int i = 0; i < lstRemoveRecordcustomerDetail.Count; i++)
					{
						Or.WorkOrderCustomerDetailList.Add(lstRemoveRecordcustomerDetail[i]);
					}
				}
				GridEXRow[] rows = grdCustomerDetail.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					WorkOrderCustomerDetail vd2 = new WorkOrderCustomerDetail();
					vd2.WorkOrderCustomerDetailId = ((RecId != 0) ? Conversion.ToInt(r2.Cells["Id"].Value) : 0);
					vd2.ActionTypeId = ((vd2.WorkOrderCustomerDetailId <= 0) ? 1 : 2);
					FillDetailListCommonForInsertAndDelete(vd2, r2);
					FormHelper.ValidateField(vd2.CustomerName, "Customer", r2.RowIndex);
					FormHelper.ValidateField(vd2.ItemId, "Item", r2.RowIndex);
					FormHelper.ValidateField(vd2.ItemUomId, "ItemUom", r2.RowIndex);
					FormHelper.ValidateField(vd2.ItemAttributeVarientId, "AttributeVarient", r2.RowIndex);
					FormHelper.ValidateField(vd2.ItemQty, "Qty", r2.RowIndex);
					if (string.IsNullOrWhiteSpace(Or.RemarksHeader))
					{
						Or.RemarksHeader = vd2.Remarks;
					}
					Or.WorkOrderCustomerDetailList.Add(vd2);
				}
				rows = grdInputMaterial.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.ToInt(r.Cells["ItemId"].Value) > 0)
					{
						WorkOrderInputDetail vd = new WorkOrderInputDetail();
						vd.Id = ((RecId != 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
						FillInputDetailListCommonForInsertAndDelete(vd, r);
						FormHelper.ValidateField(vd.ItemId, "Item", r.RowIndex);
						FormHelper.ValidateField(vd.ItemUomId, "ItemUom", r.RowIndex);
						FormHelper.ValidateField(vd.ItemQty, "Qty", r.RowIndex);
						if (string.IsNullOrWhiteSpace(Or.RemarksHeader))
						{
							Or.RemarksHeader = vd.Remarks;
						}
						Or.WorkOrderInputDetailList.Add(vd);
					}
				}
				Or.AttachmentsList = new List<DMSAttachments>();
				Or.DeleteAttachmentsList = new List<DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<WorkOrderHeader>(Or, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
				Or.AttachmentsValues = AttachmentsValues;
				Or.CustomAttachmentsValues = CustomAttachmentsValues;
				int success = WorkOrderHeader.Save(Or);
				Or.WorkOrderHeaderId = success;
				if (RecId > 0)
				{
					MessageBox.Show("Data Update Successfully....  " + Or.DocNo);
					FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, DocumentTypeId, base.Name);
				}
				else
				{
					MessageBox.Show("Data Save Successfully....  " + Or.DocNo);
				}
				FromReset();
				if (ChkPreview.Checked && formright.DoHavePrintRights)
				{
					WorkOrderSlip(Or.WorkOrderHeaderId);
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
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int Id)
	{
		try
		{
			FromReset();
			RecId = Id;
			WorkOrderHeader Or = WorkOrderHeader.ReadById(RecId);
			if (Or == null)
			{
				MessageBox.Show("No Record Found");
				return;
			}
			tabControl1.SelectedIndex = 0;
			UpdateMode = true;
			btnsave.Visible = false;
			btnSaveAs.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
			txtdocdate.Value = Or.DocDate;
			txtdocno.Text = Conversion.ToString((object)Or.DocNo);
			CmbProductionType.Value = Or.ProductionTypeId;
			txtWorkOrderNo.Text = Or.WorkOrderNo;
			txtBatchDesc.Text = Conversion.ToString((object)Or.BatchDesc);
			txtStartdate.Value = Or.StartDate;
			txtEndDate.Value = Or.EndDate;
			txtDeadLineDateMain.Value = Or.DeadLine;
			txtRemarksHeader.Text = Conversion.ToString((object)Or.RemarksHeader);
			AttachmentsValues = Or.AttachmentsValues;
			CustomAttachmentsValues = Or.CustomAttachmentsValues;
			CmbBOM.Value = Or.BomHeaderId;
			PreviousbomId = Or.BomHeaderId;
			CmbPlantName.Value = Or.PlantId;
			txtTopForce.Text = Or.TopForce.ToString("#,##.##");
			txtBottomForce.Text = Or.BottomForce.ToString("#,##.##");
			dtCustomerDetail.Rows.Clear();
			FillDetailFromListCommonForReadById(Or, dtCustomerDetail);
			grdCustomerDetail.DataSource = dtCustomerDetail;
			grdCustomerDetail.RetrieveStructure();
			grdCustomerDetailSetting();
			dtInput.Rows.Clear();
			FillInputDetailFromListCommonForReadById(Or, dtInput);
			grdInputMaterial.DataSource = dtInput;
			grdInputMaterial.RetrieveStructure();
			grdInputSettings();
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			EnableDisableProductionType();
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
					WorkOrderHeader.DeleteByID(UserAccount.ID, RecId);
					MessageBox.Show("Delete Record Successfully");
					FromReset();
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

	private void btnSaveAs_Click(object sender, EventArgs e)
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

	private void FillInputDetailListCommonForInsertAndDelete(WorkOrderInputDetail vd, GridEXRow r)
	{
		vd.BomHeaderId = Conversion.ToInt(r.Cells["BomHeaderId"].Value);
		vd.BomNo = Conversion.ToString(r.Cells["BomNo"].Value);
		vd.BomInputId = Conversion.ToInt(r.Cells["BomInputId"].Value);
		vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
		vd.ItemName = Conversion.ToString(r.Cells["ItemName"].Value);
		vd.ItemUomId = Conversion.ToInt(r.Cells["ItemUomId"].Value);
		vd.ItemUomCode = Conversion.ToString(r.Cells["UomCode"].Value);
		vd.ItemUomEquivalent = Conversion.ToDouble(r.Cells["Uom"].Value);
		vd.ItemQty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
		vd.WeightKg = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
		vd.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
	}

	private void FillDetailListCommonForInsertAndDelete(WorkOrderCustomerDetail vd, GridEXRow r)
	{
		vd.CustomerTypeId = Conversion.ToInt(r.Cells["CustomerTypeId"].Value);
		vd.CustomerType = Conversion.ToString(r.Cells["CustomerType"].Value);
		vd.CustomerId = Conversion.ToInt(r.Cells["CustomerId"].Value);
		vd.CustomerName = Conversion.ToString(r.Cells["CustomerName"].Value);
		vd.SaleOrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
		vd.SaleOrderNo = Conversion.ToInt((object)r.Cells["OrderNo"].Text);
		vd.SaleOrderDetailId = Conversion.ToInt(r.Cells["OrderDetailId"].Value);
		vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
		vd.ItemCode = Conversion.ToString(r.Cells["ItemCode"].Value);
		vd.ItemName = Conversion.ToString(r.Cells["ItemName"].Value);
		vd.ItemUomId = Conversion.ToInt(r.Cells["ItemUomId"].Value);
		vd.ItemUomCode = Conversion.ToString(r.Cells["UomCode"].Value);
		vd.ItemUomEquivalent = Conversion.ToDouble(r.Cells["UomEquivalent"].Value);
		vd.ItemAttributeVarientId = Conversion.ToInt(r.Cells["ItemAttributeVarientId"].Value);
		vd.ItemAttribute = Conversion.ToString((object)r.Cells["AttributeVarient"].Text);
		vd.VarientEquivalent = Conversion.ToDecimal(r.Cells["VarientEquivalent"].Value);
		vd.ItemQty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
		vd.VarientQty = Conversion.ToDecimal(r.Cells["VarientQty"].Value);
		vd.NetWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
		vd.ReferenceNo = Conversion.ToString(r.Cells["ReferenceNo"].Value);
		vd.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
	}

	private void FillInputDetailFromListCommonForReadById(WorkOrderHeader invGrn, DataTable dt)
	{
		if (invGrn != null && invGrn.WorkOrderInputDetailList?.Count == 0)
		{
			return;
		}
		foreach (WorkOrderInputDetail detail in invGrn.WorkOrderInputDetailList)
		{
			List<object> rowValues = new List<object>
			{
				detail.Id, detail.BomHeaderId, detail.BomNo, detail.BomInputId, detail.ItemId, detail.ItemName, detail.ItemUomId, detail.ItemUomEquivalent, detail.ItemUomCode, detail.ItemQty,
				detail.WeightKg, detail.Remarks
			};
			dt.Rows.Add(rowValues.ToArray());
		}
	}

	private void FillDetailFromListCommonForReadById(WorkOrderHeader invGrn, DataTable dt)
	{
		if (invGrn != null && invGrn.WorkOrderCustomerDetailList?.Count == 0)
		{
			return;
		}
		foreach (WorkOrderCustomerDetail detail in invGrn.WorkOrderCustomerDetailList)
		{
			List<object> rowValues = new List<object>
			{
				detail.WorkOrderCustomerDetailId, detail.CustomerTypeId, detail.CustomerType, detail.CustomerId, detail.CustomerName, detail.SaleOrderId, detail.SaleOrderNo, detail.SaleOrderDetailId, detail.ItemId, detail.ItemCode,
				detail.ItemName, detail.ItemUomId, detail.ItemUomCode, detail.ItemUomEquivalent, detail.ItemAttributeVarientId, detail.ItemAttribute, detail.VarientEquivalent, detail.ItemQty, detail.VarientQty, detail.NetWeight,
				detail.ReferenceNo, detail.Remarks
			};
			dt.Rows.Add(rowValues.ToArray());
		}
	}

	private void txtQtyDetail_KeyPress(object sender, KeyPressEventArgs e)
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

	private void btnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			WorkOrderSlip(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WorkOrderSlip(int PrintId)
	{
		try
		{
			CommonServices.WorkOrderConcrete_1870(PrintId);
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
			cmbDateTypeHistory.Focus();
		}
		else
		{
			txtdocdate.Focus();
		}
	}

	private DataTable HistoryComboDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return WorkOrderHeader.GetDataForDropDownFromWorkOrderHeader(new ReportsParameters
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

	private void HistoryComboBind(DataTable dtcombo)
	{
		try
		{
			DataTable dtWO = new DataTable();
			dtWO.Columns.Add("Id", typeof(int));
			dtWO.Columns.Add("Name");
			DataTable dtPT = dtWO.Clone();
			DataTable dtPlant = dtWO.Clone();
			DataTable dtItem = dtWO.Clone();
			if (dtcombo.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtcombo.Rows.Count; i++)
			{
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "WorkOrderNo")
				{
					dtWO.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "ProductionType")
				{
					dtPT.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "Plant")
				{
					dtPlant.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "Item")
				{
					dtItem.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbPlantNameHistory, dtPlant, "Id", "Name", "Plant Name", AllColumns: true, Conversion.ToInt(CmbPlantNameHistory.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbProductionTypeHistory, dtPT, "Id", "Name", "Production Type", AllColumns: true, Conversion.ToInt(CmbProductionTypeHistory.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbItemNameHistory, dtItem, "Id", "Name", "Item Name", AllColumns: true, Conversion.ToInt(CmbItemNameHistory.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbWorkOrderNoHistory, dtWO, "Id", "Name", "Work Order No", AllColumns: true, Conversion.ToInt(CmbWorkOrderNoHistory.Value), insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Datetypefill()
	{
		try
		{
			InfragisticsHelper.BindComboDateType(cmbDateTypeHistory);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbDateTypeHistory_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbDateTypeHistory.Value) == 1)
			{
				FromDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 2)
			{
				FromDateHistory.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 3)
			{
				DateTime now = DateTime.Now;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				FromDateHistory.Value = first;
				ToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				FromDateHistory.Value = first2;
				ToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 5)
			{
				FromDateHistory.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnHistoryRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)cmbDateTypeHistory).Text = string.Empty;
			((Control)(object)CmbProductionTypeHistory).Text = string.Empty;
			((Control)(object)CmbWorkOrderNoHistory).Text = string.Empty;
			grdHistory.DataSource = null;
			grdHistoryDetail.DataSource = null;
			cmbDateTypeHistory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefreshHistory_Click(object sender, EventArgs e)
	{
		Datetypefill();
		HistoryComboBind(HistoryComboDbCall());
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
			ReportsParameters obj = new ReportsParameters();
			obj.CompanyId = UserAccount.CompanyId;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = DocumentTypeId;
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
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
			obj.PlantId = Conversion.ToInt(CmbPlantName.Value);
			obj.ProductionTypeId = Conversion.ToInt(CmbProductionTypeHistory.Value);
			obj.ItemId = Conversion.ToInt(CmbItemNameHistory.Value);
			obj.WorkOrderId = Conversion.ToInt(CmbWorkOrderNoHistory.Value);
			DataTable dtHistory = WorkOrderHeader.FormHistory(obj);
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dataTable = dtHistoryGrid;
				if (dataTable != null && dataTable.Columns.Count == 0)
				{
					dtHistoryGrid.Columns.Add("Id", typeof(int));
					dtHistoryGrid.Columns.Add("DocumentTypeId", typeof(int));
					dtHistoryGrid.Columns.Add("DocNo", typeof(int));
					dtHistoryGrid.Columns.Add("DocDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("PlantName", typeof(string));
					dtHistoryGrid.Columns.Add("ProductionTypeId", typeof(int));
					dtHistoryGrid.Columns.Add("ProductionType", typeof(string));
					dtHistoryGrid.Columns.Add("WorkOrderNo", typeof(string));
					dtHistoryGrid.Columns.Add("BatchDesc", typeof(string));
					dtHistoryGrid.Columns.Add("StartDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("EndDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("DeadLine", typeof(DateTime));
					dtHistoryGrid.Columns.Add("RemarksHeader", typeof(string));
					dtHistoryGrid.Columns.Add("EntryDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("EntryUserName", typeof(string));
					dtHistoryGrid.Columns.Add("ModifyDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("ModifyUserName", typeof(string));
					dtHistoryGrid.Columns.Add("IsApproved", typeof(bool));
					dtHistoryGrid.Columns.Add("ApprovedDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("ApprovalUserName", typeof(string));
				}
				dtHistoryGrid.Rows.Clear();
				foreach (DataRow row in dtHistory.Rows)
				{
					dtHistoryGrid.Rows.Add(row["WorkOrderHeaderId"], row["DocumentTypeId"], row["DocNo"], row["DocDate"], row["PlantName"], row["ProductionTypeId"], row["ProductionType"], row["WorkOrderNo"], row["BatchDesc"], row["StartDate"], row["EndDate"], row["DeadLine"], row["RemarksHeader"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"], row["IsApproved"], row["ApprovedDate"], row["ApprovalUserName"]);
				}
				grdHistory.DataSource = dtHistoryGrid;
				grdHistory.RetrieveStructure();
				HistoryGridSetting();
			}
			else
			{
				grdHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSetting()
	{
		try
		{
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdHistory.RootTable.Columns["ProductionTypeId"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grdHistory, 2, 3);
			grdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdHistory.RootTable.Columns["StartDate"].FormatString = "dd-MMM-yy";
			grdHistory.RootTable.Columns["EndDate"].FormatString = "dd-MMM-yy";
			grdHistory.RootTable.Columns["DeadLine"].FormatString = "dd-MMM-yy";
			grdHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["PlantName"].Width = Constants.InventoryConstants.PlantName;
			grdHistory.RootTable.Columns["ProductionType"].Width = Constants.InventoryConstants.ProductionNo;
			grdHistory.RootTable.Columns["WorkOrderNo"].Width = Constants.InventoryConstants.InvoiceNoConstant;
			grdHistory.RootTable.Columns["BatchDesc"].Width = Constants.InventoryConstants.OtherDescription;
			grdHistory.RootTable.Columns["StartDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["EndDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["DeadLine"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["RemarksHeader"].Width = Constants.InventoryConstants.RemarksConstants;
			grdHistory.RootTable.Columns["EntryUserName"].Width = Constants.InventoryConstants.EntryUserConstants;
			grdHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["ModifyUserName"].Width = Constants.InventoryConstants.ModifyUserConstants;
			grdHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["IsApproved"].Width = Constants.InventoryConstants.IsApproved;
			grdHistory.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["ApprovalUserName"].Width = Constants.InventoryConstants.EntryUserConstants;
			GridEX_Helper.AddButton(grdHistory, "Edit", "Edit", 50, 0);
			GridEX_Helper.AddButton(grdHistory, "SaveAs", "SaveAs", 60, 1);
			GridEX_Helper.AddButton(grdHistory, "Print", "Print", 50, 2);
			grdHistory.FrozenColumns = 3;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count != 0)
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("ypu don't have updae rights...");
				}
				ReadById(Conversion.ToInt((object)r.Cells["Id"].Value.ToString()));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count == 0)
			{
				return;
			}
			if (e.Column.Key == "Print")
			{
				WorkOrderSlip(Conversion.ToInt((object)r.Cells["Id"].Value.ToString()));
			}
			if (e.Column.Key == "Edit")
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("you don't have update rights...");
				}
				ReadById(Conversion.ToInt((object)r.Cells["Id"].Value.ToString()));
			}
			if (e.Column.Key == "SaveAs")
			{
				DataGridHistory_SaveAs(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count != 0 && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["Id"].Value), DocumentTypeId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SaveAs(int CurrId)
	{
		try
		{
			ReadById(CurrId);
			btnsave.Visible = false;
			btnDelete.Visible = false;
			btnUpdate.Visible = false;
			btnSaveAs.Visible = true;
			txtdocdate.Value = DateTime.Now;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				GetDetailGrdByHeadId(Conversion.ToInt(r.Cells["Id"].Value));
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
			WorkOrderHeader Obj = WorkOrderHeader.ReadById(Id);
			if (Obj != null && Obj.WorkOrderCustomerDetailList.Count == 0)
			{
				grdHistoryDetail.ClearStructure();
				return;
			}
			DataTable dtDetailHistory = dtCustomerDetail.Clone();
			FillDetailFromListCommonForReadById(Obj, dtDetailHistory);
			grdHistoryDetail.DataSource = dtDetailHistory;
			grdHistoryDetail.RetrieveStructure();
			grddetailhistorySettings();
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
			List<string> hiddenColumns = new List<string>
			{
				"Id", "CustomerTypeId", "CustomerId", "OrderId", "OrderDetailId", "ItemId", "ItemUomId", "UomEquivalent", "ItemAttributeVarientId", "VarientEquivalent",
				"NetWeight"
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdHistoryDetail, 2, 3, hiddenColumns);
			grdHistoryDetail.RootTable.Columns["CustomerName"].Width = Constants.InventoryConstants.SupplierName;
			grdHistoryDetail.RootTable.Columns["OrderNo"].Width = Constants.InventoryConstants.OrderNo;
			grdHistoryDetail.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdHistory, dtHistoryGrid);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmBillOfMaterial_Manufacturing_KeyDown(object sender, KeyEventArgs e)
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
					if (btnUpdate.Visible && btnUpdate.Enabled && UpdateMode)
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
					btnRefresh_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.F5)
				{
					txtdocdate.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.F10)
				{
					AT.Show();
				}
				else if (e.Control && e.KeyCode == Keys.F12)
				{
					if (btnSaveAs.Visible && btnSaveAs.Enabled)
					{
						btnSaveAs_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdCustomerDetail).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					txtdocdate.Focus();
				}
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdHistoryDetail).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.S)
				{
					btnshow_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					((Control)(object)grdHistory).Focus();
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
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("F1", "For Combo Lookup on Current Column of any Focused Grid");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+D", "For Adding an row in Focused Grid");
			dt.Rows.Add("Ctrl+Delete", "For Deleting an row of Focused Grid");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On on Doc Date");
			dt.Rows.Add("Ctrl+ArrowRight", "to change focus from one grid to another");
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

	private void grdCustomerDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdCustomerDetail.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count == 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grdCustomerDetail.CurrentColumn.Key == "Delete")
				{
					DeleteCustomerDetailRow(r);
				}
				if (grdCustomerDetail.CurrentColumn.Key == "Edit")
				{
					grdCustomerDetail_DoubleClick(null, null);
				}
				grdCustomerDetail.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteCustomerDetailRow(r);
				grdCustomerDetail.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdCustomerDetail_DoubleClick(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_KeyDown(object sender, KeyEventArgs e)
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
					WorkOrderSlip(CurrId);
				}
				if (grdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(CurrId, DocumentTypeId);
				}
				if (grdHistory.CurrentColumn.Key == "SaveAs")
				{
					DataGridHistory_SaveAs(CurrId);
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

	private void BtnReferenceParty_Click(object sender, EventArgs e)
	{
		try
		{
			if (RadCustomers.Checked)
			{
				FormHelper.CanOpenForm("supfrmDefineSupplier", delegate
				{
					supfrmDefineSupplier supfrmDefineSupplier = new supfrmDefineSupplier(UserAccount);
					supfrmDefineSupplier.Show();
					supfrmDefineSupplier.BringToFront();
				}, UserAccount);
			}
			else
			{
				new DefineReferenceParties(UserAccount).Show();
			}
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
			ItemNameBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error loading items: " + ex.Message);
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
		//IL_048f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0499: Expected O, but got Unknown
		//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d0: Expected O, but got Unknown
		//IL_0513: Unknown result type (might be due to invalid IL or missing references)
		//IL_051d: Expected O, but got Unknown
		//IL_0576: Unknown result type (might be due to invalid IL or missing references)
		//IL_0580: Expected O, but got Unknown
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0596: Expected O, but got Unknown
		//IL_069f: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a9: Expected O, but got Unknown
		//IL_06aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b4: Expected O, but got Unknown
		//IL_06c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ca: Expected O, but got Unknown
		//IL_06cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d5: Expected O, but got Unknown
		//IL_06f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0701: Expected O, but got Unknown
		//IL_07ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0809: Expected O, but got Unknown
		//IL_0820: Unknown result type (might be due to invalid IL or missing references)
		//IL_082a: Expected O, but got Unknown
		//IL_0899: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a3: Expected O, but got Unknown
		//IL_08af: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b9: Expected O, but got Unknown
		//IL_08c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cf: Expected O, but got Unknown
		//IL_08db: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e5: Expected O, but got Unknown
		//IL_08fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0906: Expected O, but got Unknown
		//IL_0e23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2d: Expected O, but got Unknown
		//IL_0e3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e44: Expected O, but got Unknown
		//IL_115b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1165: Expected O, but got Unknown
		//IL_1172: Unknown result type (might be due to invalid IL or missing references)
		//IL_117c: Expected O, but got Unknown
		//IL_5bcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_5bd5: Expected O, but got Unknown
		//IL_5bf9: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c03: Expected O, but got Unknown
		GridEXLayout grdCustomerDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInputMaterial_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
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
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
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
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.pcc.frmWorkOrderConcrete));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grdCustomerDetail = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdInputMaterial = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label4 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.btnDefinePlant = new System.Windows.Forms.Button();
		this.CmbPlantName = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.txtBottomForce = new System.Windows.Forms.TextBox();
		this.txtTopForce = new System.Windows.Forms.TextBox();
		this.label48 = new System.Windows.Forms.Label();
		this.txtRemarksHeader = new System.Windows.Forms.TextBox();
		this.txtdocdate = new System.Windows.Forms.DateTimePicker();
		this.label47 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.CmbBOM = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.CmbProductionType = new UltraCombo();
		this.label16 = new System.Windows.Forms.Label();
		this.txtWorkOrderNo = new System.Windows.Forms.TextBox();
		this.txtEndDate = new System.Windows.Forms.DateTimePicker();
		this.txtStartdate = new System.Windows.Forms.DateTimePicker();
		this.label1 = new System.Windows.Forms.Label();
		this.txtBatchDesc = new System.Windows.Forms.TextBox();
		this.label25 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.txtDeadLineDateMain = new System.Windows.Forms.DateTimePicker();
		this.label13 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.BtnReferenceParty = new System.Windows.Forms.Button();
		this.panel6 = new System.Windows.Forms.Panel();
		this.RadCustomers = new System.Windows.Forms.RadioButton();
		this.RadRefParty = new System.Windows.Forms.RadioButton();
		this.rdSearchByCode = new System.Windows.Forms.RadioButton();
		this.rdSearchByName = new System.Windows.Forms.RadioButton();
		this.txtVarientEquivalent = new System.Windows.Forms.TextBox();
		this.txtReferenceNoDetail = new System.Windows.Forms.TextBox();
		this.BtnAddCustomer = new System.Windows.Forms.Button();
		this.label43 = new System.Windows.Forms.Label();
		this.CmbCustomerOrderNo = new UltraCombo();
		this.CmbAttribute = new UltraCombo();
		this.label42 = new System.Windows.Forms.Label();
		this.CmbItemDetail = new UltraCombo();
		this.cmbItemUOM = new UltraCombo();
		this.BtnCancelCustomer = new System.Windows.Forms.Button();
		this.label36 = new System.Windows.Forms.Label();
		this.BtnUpdateCustmer = new System.Windows.Forms.Button();
		this.CmbCustomerName = new UltraCombo();
		this.label14 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtCustomerOrderQty = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.textBox1 = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnSaveAs = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.label21 = new System.Windows.Forms.Label();
		this.ChkPreview = new System.Windows.Forms.CheckBox();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.grdHistory = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.grdHistoryDetail = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label17 = new System.Windows.Forms.Label();
		this.panel13 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnshow = new System.Windows.Forms.Button();
		this.CmbWorkOrderNoHistory = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.CmbItemNameHistory = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.CmbPlantNameHistory = new UltraCombo();
		this.label18 = new System.Windows.Forms.Label();
		this.CmbProductionTypeHistory = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.cmbDateTypeHistory = new UltraCombo();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label28 = new System.Windows.Forms.Label();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label5 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.panel38 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.label32 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdCustomerDetail).BeginInit();
		this.panel14.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInputMaterial).BeginInit();
		this.panel7.SuspendLayout();
		this.panel2.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionType).BeginInit();
		this.groupBox3.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerOrderNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAttribute).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerName).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).BeginInit();
		this.panel15.SuspendLayout();
		this.panel13.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWorkOrderNoHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemNameHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantNameHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionTypeHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).BeginInit();
		this.panel38.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1184, 611);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1184, 611);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grdCustomerDetail);
		this.tabPage1.Controls.Add(this.panel14);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1176, 582);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.grdCustomerDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdCustomerDetail).BackColor = System.Drawing.Color.White;
		grdCustomerDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdCustomerDetail.DesignTimeLayout = grdCustomerDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdCustomerDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).Location = new System.Drawing.Point(3, 291);
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).Name = "grdCustomerDetail";
		((ScrollableBase)this.grdCustomerDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).Size = new System.Drawing.Size(1170, 288);
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).TabIndex = 0;
		this.grdCustomerDetail.TotalRow = (InheritableBoolean)1;
		this.grdCustomerDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdCustomerDetail).VisualStyle = (VisualStyle)4;
		this.grdCustomerDetail.CellUpdated += new ColumnActionEventHandler(grdCustomerDetail_CellUpdated);
		this.grdCustomerDetail.ColumnButtonClick += new ColumnActionEventHandler(grdCustomerDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).DoubleClick += new System.EventHandler(grdCustomerDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdCustomerDetail_KeyDown);
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.ctrlGrdBar3);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(3, 270);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1170, 21);
		this.panel14.TabIndex = 131;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1124, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar3.MyGrid = this.grdCustomerDetail;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(43, 20);
		this.ctrlGrdBar3.TabIndex = 127;
		this.panel5.Controls.Add(this.panel4);
		this.panel5.Controls.Add(this.panel2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 58);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1170, 212);
		this.panel5.TabIndex = 130;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdInputMaterial);
		this.panel4.Controls.Add(this.panel7);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(950, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(220, 212);
		this.panel4.TabIndex = 2;
		((ScrollableBase)this.grdInputMaterial).BackColor = System.Drawing.Color.White;
		grdInputMaterial_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInputMaterial.DesignTimeLayout = grdInputMaterial_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInputMaterial.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).Name = "grdInputMaterial";
		((ScrollableBase)this.grdInputMaterial).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).Size = new System.Drawing.Size(220, 190);
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).TabIndex = 4;
		this.grdInputMaterial.TotalRow = (InheritableBoolean)1;
		this.grdInputMaterial.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInputMaterial).VisualStyle = (VisualStyle)4;
		this.grdInputMaterial.CellUpdated += new ColumnActionEventHandler(grdInputMaterial_CellUpdated);
		this.grdInputMaterial.ColumnButtonClick += new ColumnActionEventHandler(grdInputMaterial_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).KeyDown += new System.Windows.Forms.KeyEventHandler(grdInputMaterial_KeyDown);
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.ctrlGrdBar2);
		this.panel7.Controls.Add(this.label4);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(220, 22);
		this.panel7.TabIndex = 5;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(173, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdInputMaterial;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(43, 20);
		this.ctrlGrdBar2.TabIndex = 127;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.Teal;
		this.label4.Location = new System.Drawing.Point(3, 3);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(130, 16);
		this.label4.TabIndex = 126;
		this.label4.Text = "Bom Input Material";
		this.panel2.Controls.Add(this.groupBox2);
		this.panel2.Controls.Add(this.groupBox3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(950, 212);
		this.panel2.TabIndex = 1;
		this.groupBox2.Controls.Add(this.btnDefinePlant);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPlantName);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add(this.txtBottomForce);
		this.groupBox2.Controls.Add(this.txtTopForce);
		this.groupBox2.Controls.Add(this.label48);
		this.groupBox2.Controls.Add(this.txtRemarksHeader);
		this.groupBox2.Controls.Add(this.txtdocdate);
		this.groupBox2.Controls.Add(this.label47);
		this.groupBox2.Controls.Add(this.txtdocno);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBOM);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbProductionType);
		this.groupBox2.Controls.Add(this.label16);
		this.groupBox2.Controls.Add(this.txtWorkOrderNo);
		this.groupBox2.Controls.Add(this.txtEndDate);
		this.groupBox2.Controls.Add(this.txtStartdate);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add(this.txtBatchDesc);
		this.groupBox2.Controls.Add(this.label25);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add(this.label37);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add(this.txtDeadLineDateMain);
		this.groupBox2.Controls.Add(this.label13);
		this.groupBox2.Controls.Add(this.label23);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.groupBox2.Location = new System.Drawing.Point(3, 3);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(945, 98);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main Information";
		this.btnDefinePlant.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnDefinePlant.Location = new System.Drawing.Point(215, 42);
		this.btnDefinePlant.Name = "btnDefinePlant";
		this.btnDefinePlant.Size = new System.Drawing.Size(28, 26);
		this.btnDefinePlant.TabIndex = 3;
		this.btnDefinePlant.TabStop = false;
		this.btnDefinePlant.UseVisualStyleBackColor = true;
		this.CmbPlantName.AllowNull = (DefaultableBoolean)1;
		this.CmbPlantName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPlantName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPlantName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance93;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance101).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPlantName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPlantName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPlantName).Location = new System.Drawing.Point(57, 42);
		((System.Windows.Forms.Control)(object)this.CmbPlantName).Name = "CmbPlantName";
		((System.Windows.Forms.Control)(object)this.CmbPlantName).Size = new System.Drawing.Size(157, 26);
		((System.Windows.Forms.Control)(object)this.CmbPlantName).TabIndex = 2;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(3, 48);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(34, 15);
		this.label11.TabIndex = 151;
		this.label11.Text = "Plant";
		this.txtBottomForce.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBottomForce.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBottomForce.Location = new System.Drawing.Point(506, 44);
		this.txtBottomForce.Name = "txtBottomForce";
		this.txtBottomForce.Size = new System.Drawing.Size(65, 23);
		this.txtBottomForce.TabIndex = 8;
		this.txtTopForce.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTopForce.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTopForce.Location = new System.Drawing.Point(506, 17);
		this.txtTopForce.Name = "txtTopForce";
		this.txtTopForce.Size = new System.Drawing.Size(65, 23);
		this.txtTopForce.TabIndex = 7;
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(451, 21);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(59, 15);
		this.label48.TabIndex = 148;
		this.label48.Text = "Top Force";
		this.txtRemarksHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksHeader.Location = new System.Drawing.Point(57, 70);
		this.txtRemarksHeader.Name = "txtRemarksHeader";
		this.txtRemarksHeader.Size = new System.Drawing.Size(706, 23);
		this.txtRemarksHeader.TabIndex = 4;
		this.txtdocdate.CustomFormat = "dd-MMM-yy";
		this.txtdocdate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtdocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdocdate.Location = new System.Drawing.Point(123, 17);
		this.txtdocdate.Name = "txtdocdate";
		this.txtdocdate.Size = new System.Drawing.Size(120, 22);
		this.txtdocdate.TabIndex = 1;
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(451, 40);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(47, 30);
		this.label47.TabIndex = 147;
		this.label47.Text = "Bottom\r\nForce";
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(57, 17);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(65, 23);
		this.txtdocno.TabIndex = 0;
		this.CmbBOM.AllowNull = (DefaultableBoolean)1;
		this.CmbBOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbBOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance73;
		((SpecialBoxBase)((UltraGridBase)this.CmbBOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbBOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbBOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance79).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbBOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbBOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbBOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbBOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBOM).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBOM).Location = new System.Drawing.Point(311, 42);
		((System.Windows.Forms.Control)(object)this.CmbBOM).Name = "CmbBOM";
		((System.Windows.Forms.Control)(object)this.CmbBOM).Size = new System.Drawing.Size(140, 26);
		((System.Windows.Forms.Control)(object)this.CmbBOM).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbBOM).Leave += new System.EventHandler(CmbBOM_Leave);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(245, 48);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(51, 15);
		this.label7.TabIndex = 140;
		this.label7.Text = "Bom No";
		this.CmbProductionType.AllowNull = (DefaultableBoolean)1;
		this.CmbProductionType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbProductionType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbProductionType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance19;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance32).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance43).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance54).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbProductionType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Location = new System.Drawing.Point(311, 15);
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Name = "CmbProductionType";
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Size = new System.Drawing.Size(140, 26);
		((System.Windows.Forms.Control)(object)this.CmbProductionType).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Leave += new System.EventHandler(CmbProductionType_Leave);
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(245, 13);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(66, 30);
		this.label16.TabIndex = 140;
		this.label16.Text = "Production\r\nType";
		this.txtWorkOrderNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWorkOrderNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWorkOrderNo.Location = new System.Drawing.Point(643, 17);
		this.txtWorkOrderNo.Name = "txtWorkOrderNo";
		this.txtWorkOrderNo.Size = new System.Drawing.Size(120, 23);
		this.txtWorkOrderNo.TabIndex = 9;
		this.txtEndDate.CustomFormat = "dd-MMM-yy";
		this.txtEndDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtEndDate.Location = new System.Drawing.Point(823, 44);
		this.txtEndDate.Name = "txtEndDate";
		this.txtEndDate.Size = new System.Drawing.Size(120, 22);
		this.txtEndDate.TabIndex = 12;
		this.txtStartdate.CustomFormat = "dd-MMM-yy";
		this.txtStartdate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtStartdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtStartdate.Location = new System.Drawing.Point(823, 17);
		this.txtStartdate.Name = "txtStartdate";
		this.txtStartdate.Size = new System.Drawing.Size(120, 22);
		this.txtStartdate.TabIndex = 11;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(2, 74);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(52, 15);
		this.label1.TabIndex = 131;
		this.label1.Text = "Remarks";
		this.txtBatchDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBatchDesc.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBatchDesc.Location = new System.Drawing.Point(643, 44);
		this.txtBatchDesc.Name = "txtBatchDesc";
		this.txtBatchDesc.Size = new System.Drawing.Size(120, 23);
		this.txtBatchDesc.TabIndex = 10;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(763, 74);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(60, 15);
		this.label25.TabIndex = 138;
		this.label25.Text = "Dead Line";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(763, 48);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(55, 15);
		this.label9.TabIndex = 138;
		this.label9.Text = "End Date";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(2, 21);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(48, 15);
		this.label37.TabIndex = 138;
		this.label37.Text = "Doc No";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(574, 21);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(70, 15);
		this.label12.TabIndex = 131;
		this.label12.Text = "W.Order No";
		this.txtDeadLineDateMain.CustomFormat = "dd-MMM-yy";
		this.txtDeadLineDateMain.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtDeadLineDateMain.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDeadLineDateMain.Location = new System.Drawing.Point(823, 70);
		this.txtDeadLineDateMain.Name = "txtDeadLineDateMain";
		this.txtDeadLineDateMain.Size = new System.Drawing.Size(120, 22);
		this.txtDeadLineDateMain.TabIndex = 13;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(574, 48);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(66, 15);
		this.label13.TabIndex = 87;
		this.label13.Text = "Batch Desc";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(763, 21);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(60, 15);
		this.label23.TabIndex = 138;
		this.label23.Text = "Start Date";
		this.groupBox3.BackColor = System.Drawing.Color.White;
		this.groupBox3.Controls.Add(this.BtnReferenceParty);
		this.groupBox3.Controls.Add(this.panel6);
		this.groupBox3.Controls.Add(this.rdSearchByCode);
		this.groupBox3.Controls.Add(this.rdSearchByName);
		this.groupBox3.Controls.Add(this.txtVarientEquivalent);
		this.groupBox3.Controls.Add(this.txtReferenceNoDetail);
		this.groupBox3.Controls.Add(this.BtnAddCustomer);
		this.groupBox3.Controls.Add(this.label43);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerOrderNo);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAttribute);
		this.groupBox3.Controls.Add(this.label42);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemDetail);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemUOM);
		this.groupBox3.Controls.Add(this.BtnCancelCustomer);
		this.groupBox3.Controls.Add(this.label36);
		this.groupBox3.Controls.Add(this.BtnUpdateCustmer);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerName);
		this.groupBox3.Controls.Add(this.label14);
		this.groupBox3.Controls.Add(this.label26);
		this.groupBox3.Controls.Add(this.label2);
		this.groupBox3.Controls.Add(this.txtCustomerOrderQty);
		this.groupBox3.Controls.Add(this.label10);
		this.groupBox3.Controls.Add(this.textBox1);
		this.groupBox3.Controls.Add(this.label8);
		this.groupBox3.Controls.Add(this.label38);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.groupBox3.Location = new System.Drawing.Point(3, 102);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(945, 104);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Detail Information";
		this.BtnReferenceParty.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.BtnReferenceParty.Location = new System.Drawing.Point(225, 31);
		this.BtnReferenceParty.Name = "BtnReferenceParty";
		this.BtnReferenceParty.Size = new System.Drawing.Size(26, 26);
		this.BtnReferenceParty.TabIndex = 1;
		this.BtnReferenceParty.TabStop = false;
		this.BtnReferenceParty.UseVisualStyleBackColor = true;
		this.BtnReferenceParty.Click += new System.EventHandler(BtnReferenceParty_Click);
		this.panel6.Controls.Add(this.RadCustomers);
		this.panel6.Controls.Add(this.RadRefParty);
		this.panel6.Location = new System.Drawing.Point(4, 12);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(156, 18);
		this.panel6.TabIndex = 495;
		this.RadCustomers.AutoSize = true;
		this.RadCustomers.Checked = true;
		this.RadCustomers.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadCustomers.Location = new System.Drawing.Point(2, 0);
		this.RadCustomers.Name = "RadCustomers";
		this.RadCustomers.Size = new System.Drawing.Size(76, 19);
		this.RadCustomers.TabIndex = 493;
		this.RadCustomers.TabStop = true;
		this.RadCustomers.Text = "Customer";
		this.RadCustomers.UseVisualStyleBackColor = true;
		this.RadCustomers.CheckedChanged += new System.EventHandler(RadCustomers_CheckedChanged);
		this.RadRefParty.AutoSize = true;
		this.RadRefParty.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadRefParty.Location = new System.Drawing.Point(82, 0);
		this.RadRefParty.Name = "RadRefParty";
		this.RadRefParty.Size = new System.Drawing.Size(72, 19);
		this.RadRefParty.TabIndex = 494;
		this.RadRefParty.Text = "Ref Party";
		this.RadRefParty.UseVisualStyleBackColor = true;
		this.rdSearchByCode.AutoSize = true;
		this.rdSearchByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByCode.Location = new System.Drawing.Point(438, 13);
		this.rdSearchByCode.Name = "rdSearchByCode";
		this.rdSearchByCode.Size = new System.Drawing.Size(52, 19);
		this.rdSearchByCode.TabIndex = 494;
		this.rdSearchByCode.Text = "Code";
		this.rdSearchByCode.UseVisualStyleBackColor = true;
		this.rdSearchByName.AutoSize = true;
		this.rdSearchByName.Checked = true;
		this.rdSearchByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByName.Location = new System.Drawing.Point(382, 13);
		this.rdSearchByName.Name = "rdSearchByName";
		this.rdSearchByName.Size = new System.Drawing.Size(57, 19);
		this.rdSearchByName.TabIndex = 493;
		this.rdSearchByName.TabStop = true;
		this.rdSearchByName.Text = "Name";
		this.rdSearchByName.UseVisualStyleBackColor = true;
		this.rdSearchByName.CheckedChanged += new System.EventHandler(rdSearchByName_CheckedChanged);
		this.txtVarientEquivalent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVarientEquivalent.Enabled = false;
		this.txtVarientEquivalent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVarientEquivalent.Location = new System.Drawing.Point(792, 33);
		this.txtVarientEquivalent.Name = "txtVarientEquivalent";
		this.txtVarientEquivalent.Size = new System.Drawing.Size(70, 23);
		this.txtVarientEquivalent.TabIndex = 6;
		this.txtVarientEquivalent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtReferenceNoDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtReferenceNoDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtReferenceNoDetail.Location = new System.Drawing.Point(3, 76);
		this.txtReferenceNoDetail.Name = "txtReferenceNoDetail";
		this.txtReferenceNoDetail.Size = new System.Drawing.Size(221, 23);
		this.txtReferenceNoDetail.TabIndex = 8;
		this.BtnAddCustomer.BackColor = System.Drawing.Color.Teal;
		this.BtnAddCustomer.FlatAppearance.BorderSize = 0;
		this.BtnAddCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAddCustomer.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnAddCustomer.ForeColor = System.Drawing.Color.White;
		this.BtnAddCustomer.Location = new System.Drawing.Point(797, 76);
		this.BtnAddCustomer.Name = "BtnAddCustomer";
		this.BtnAddCustomer.Size = new System.Drawing.Size(39, 23);
		this.BtnAddCustomer.TabIndex = 10;
		this.BtnAddCustomer.Text = "+";
		this.BtnAddCustomer.UseVisualStyleBackColor = false;
		this.BtnAddCustomer.Click += new System.EventHandler(BtnAddCustomer_Click);
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(791, 15);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(62, 15);
		this.label43.TabIndex = 93;
		this.label43.Text = "Equivalent";
		this.CmbCustomerOrderNo.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerOrderNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerOrderNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerOrderNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance29;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance36).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance37).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerOrderNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerOrderNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerOrderNo).Location = new System.Drawing.Point(252, 31);
		((System.Windows.Forms.Control)(object)this.CmbCustomerOrderNo).Name = "CmbCustomerOrderNo";
		((System.Windows.Forms.Control)(object)this.CmbCustomerOrderNo).Size = new System.Drawing.Size(100, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerOrderNo).TabIndex = 2;
		this.CmbCustomerOrderNo.ValueChanged += new System.EventHandler(CmbCustomerOrderNo_ValueChanged);
		((System.Windows.Forms.Control)(object)this.CmbCustomerOrderNo).Leave += new System.EventHandler(CmbCustomerOrderNo_ValueChanged);
		this.CmbAttribute.AllowNull = (DefaultableBoolean)1;
		this.CmbAttribute.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAttribute.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAttribute.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Appearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAttribute).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance59;
		((SpecialBoxBase)((UltraGridBase)this.CmbAttribute).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance64).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance65).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance67).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAttribute).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAttribute.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAttribute).Location = new System.Drawing.Point(666, 31);
		((System.Windows.Forms.Control)(object)this.CmbAttribute).Name = "CmbAttribute";
		((System.Windows.Forms.Control)(object)this.CmbAttribute).Size = new System.Drawing.Size(125, 26);
		((System.Windows.Forms.Control)(object)this.CmbAttribute).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbAttribute).Leave += new System.EventHandler(CmbAttribute_Leave);
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(665, 15);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(54, 15);
		this.label42.TabIndex = 92;
		this.label42.Text = "Attribute";
		this.CmbItemDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbItemDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance5;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance10).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance11).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemDetail).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemDetail).Location = new System.Drawing.Point(353, 32);
		((System.Windows.Forms.Control)(object)this.CmbItemDetail).Name = "CmbItemDetail";
		((System.Windows.Forms.Control)(object)this.CmbItemDetail).Size = new System.Drawing.Size(220, 25);
		((System.Windows.Forms.Control)(object)this.CmbItemDetail).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbItemDetail).Leave += new System.EventHandler(CmbItemDetail_Leave);
		this.cmbItemUOM.AllowNull = (DefaultableBoolean)1;
		this.cmbItemUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance145).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance145).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance147;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance148).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance148).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance152).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance153).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemUOM).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemUOM).Location = new System.Drawing.Point(575, 32);
		((System.Windows.Forms.Control)(object)this.cmbItemUOM).Name = "cmbItemUOM";
		((System.Windows.Forms.Control)(object)this.cmbItemUOM).Size = new System.Drawing.Size(90, 25);
		((System.Windows.Forms.Control)(object)this.cmbItemUOM).TabIndex = 4;
		this.BtnCancelCustomer.BackColor = System.Drawing.Color.Teal;
		this.BtnCancelCustomer.FlatAppearance.BorderSize = 0;
		this.BtnCancelCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancelCustomer.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnCancelCustomer.ForeColor = System.Drawing.Color.White;
		this.BtnCancelCustomer.Location = new System.Drawing.Point(863, 76);
		this.BtnCancelCustomer.Name = "BtnCancelCustomer";
		this.BtnCancelCustomer.Size = new System.Drawing.Size(64, 23);
		this.BtnCancelCustomer.TabIndex = 11;
		this.BtnCancelCustomer.Text = "Cancel";
		this.BtnCancelCustomer.UseVisualStyleBackColor = false;
		this.BtnCancelCustomer.Visible = false;
		this.BtnCancelCustomer.Click += new System.EventHandler(BtnCancelCustomer_Click);
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(352, 15);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(32, 15);
		this.label36.TabIndex = 139;
		this.label36.Text = "Item";
		this.BtnUpdateCustmer.BackColor = System.Drawing.Color.Teal;
		this.BtnUpdateCustmer.FlatAppearance.BorderSize = 0;
		this.BtnUpdateCustmer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateCustmer.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnUpdateCustmer.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateCustmer.Location = new System.Drawing.Point(797, 76);
		this.BtnUpdateCustmer.Name = "BtnUpdateCustmer";
		this.BtnUpdateCustmer.Size = new System.Drawing.Size(64, 23);
		this.BtnUpdateCustmer.TabIndex = 10;
		this.BtnUpdateCustmer.Text = "Update";
		this.BtnUpdateCustmer.UseVisualStyleBackColor = false;
		this.BtnUpdateCustmer.Visible = false;
		this.BtnUpdateCustmer.Click += new System.EventHandler(BtnUpdateCustmer_Click);
		this.CmbCustomerName.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance44;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance49).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance50).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Location = new System.Drawing.Point(4, 31);
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Name = "CmbCustomerName";
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Leave += new System.EventHandler(CmbCustomerName_Leave);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(164, 14);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(39, 15);
		this.label14.TabIndex = 140;
		this.label14.Text = "Name";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(573, 15);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(35, 15);
		this.label26.TabIndex = 140;
		this.label26.Text = "UOM";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(3, 60);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(78, 15);
		this.label2.TabIndex = 87;
		this.label2.Text = "Reference No";
		this.txtCustomerOrderQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCustomerOrderQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCustomerOrderQty.Location = new System.Drawing.Point(863, 33);
		this.txtCustomerOrderQty.Name = "txtCustomerOrderQty";
		this.txtCustomerOrderQty.Size = new System.Drawing.Size(80, 23);
		this.txtCustomerOrderQty.TabIndex = 7;
		this.txtCustomerOrderQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQtyDetail_KeyPress);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(251, 15);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(81, 15);
		this.label10.TabIndex = 19;
		this.label10.Text = "Sale Order No";
		this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.textBox1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox1.Location = new System.Drawing.Point(225, 76);
		this.textBox1.Name = "textBox1";
		this.textBox1.Size = new System.Drawing.Size(566, 23);
		this.textBox1.TabIndex = 9;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(225, 60);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(52, 15);
		this.label8.TabIndex = 131;
		this.label8.Text = "Remarks";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label38.Location = new System.Drawing.Point(863, 15);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(26, 15);
		this.label38.TabIndex = 13;
		this.label38.Text = "Qty";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Controls.Add(this.label21);
		this.panel3.Controls.Add(this.ChkPreview);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1170, 55);
		this.panel3.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[10] { this.btnnew, this.btnRefresh, this.btnsave, this.btnSaveAs, this.btnUpdate, this.btnDelete, this.toolStripSeparator, this.btnAttachment, this.btnPrint, this.btnshortcutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1170, 27);
		this.toolStrip1.TabIndex = 2;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnSaveAs.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSaveAs.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveAs.Name = "btnSaveAs";
		this.btnSaveAs.Size = new System.Drawing.Size(80, 24);
		this.btnSaveAs.Text = "SaveAs";
		this.btnSaveAs.Visible = false;
		this.btnSaveAs.Click += new System.EventHandler(btnSaveAs_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "&Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.ToolTipText = "Alt_A";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(100, 24);
		this.btnPrint.Text = "1870-&Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.White;
		this.label21.Location = new System.Drawing.Point(6, 31);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(103, 19);
		this.label21.TabIndex = 126;
		this.label21.Text = "Work Order";
		this.ChkPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPreview.AutoSize = true;
		this.ChkPreview.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPreview.ForeColor = System.Drawing.Color.White;
		this.ChkPreview.Location = new System.Drawing.Point(1089, 30);
		this.ChkPreview.Name = "ChkPreview";
		this.ChkPreview.Size = new System.Drawing.Size(78, 20);
		this.ChkPreview.TabIndex = 12;
		this.ChkPreview.Text = "Preview";
		this.ChkPreview.UseVisualStyleBackColor = true;
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.tabPage3.Controls.Add(this.panel9);
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryDetail);
		this.tabPage3.Controls.Add(this.panel15);
		this.tabPage3.Controls.Add(this.panel13);
		this.tabPage3.Controls.Add(this.panel38);
		this.tabPage3.Location = new System.Drawing.Point(4, 4);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(1176, 582);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "History";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(0, 146);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1176, 219);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 0;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(DatagridHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(DatagridHistory_SelectionChanged);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(DatagridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(DatagridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DatagridHistory_KeyDown);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label15);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel9.Location = new System.Drawing.Point(0, 365);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1176, 24);
		this.panel9.TabIndex = 51;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(4, 5);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(225, 16);
		this.label15.TabIndex = 129;
		this.label15.Text = "Detail of the Above Selected Row";
		this.grdHistoryDetail.AllowEdit = (InheritableBoolean)2;
		grdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryDetail.DesignTimeLayout = grdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdHistoryDetail.DynamicFiltering = true;
		this.grdHistoryDetail.FilterMode = (FilterMode)2;
		this.grdHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Location = new System.Drawing.Point(0, 389);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Name = "grdHistoryDetail";
		((ScrollableBase)this.grdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Size = new System.Drawing.Size(1176, 193);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).TabIndex = 1;
		this.grdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.grdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.ctrlGrdBar1);
		this.panel15.Controls.Add(this.label17);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 123);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1176, 23);
		this.panel15.TabIndex = 53;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1129, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(9, 6, 9, 6);
		this.ctrlGrdBar1.MyGrid = this.grdHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(42, 22);
		this.ctrlGrdBar1.TabIndex = 128;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(8, 3);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(112, 16);
		this.label17.TabIndex = 127;
		this.label17.Text = "Filtered Records";
		this.panel13.BackColor = System.Drawing.Color.White;
		this.panel13.Controls.Add(this.groupBox1);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 52);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1176, 71);
		this.panel13.TabIndex = 0;
		this.groupBox1.Controls.Add(this.rdapproveddate);
		this.groupBox1.Controls.Add(this.rdmodifydate);
		this.groupBox1.Controls.Add(this.drdocdate);
		this.groupBox1.Controls.Add(this.rdentrydate);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWorkOrderNoHistory);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemNameHistory);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPlantNameHistory);
		this.groupBox1.Controls.Add(this.label18);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbProductionTypeHistory);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory);
		this.groupBox1.Controls.Add(this.ToDateHistory);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.ToDocNoHistory);
		this.groupBox1.Controls.Add(this.FromDateHistory);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Controls.Add(this.FromDocNoHistory);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 1);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1164, 69);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(1105, 13);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(63, 47);
		this.rdapproveddate.TabIndex = 13;
		this.rdapproveddate.Text = "Approved\r\nDate";
		this.rdapproveddate.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdapproveddate.Visible = false;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(1027, 46);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 12;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(1027, 10);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 10;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(1027, 28);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 11;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(969, 40);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(55, 25);
		this.btnshow.TabIndex = 9;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.CmbWorkOrderNoHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbWorkOrderNoHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWorkOrderNoHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWorkOrderNoHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWorkOrderNoHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbWorkOrderNoHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWorkOrderNoHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWorkOrderNoHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWorkOrderNoHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance142;
		((SpecialBoxBase)((UltraGridBase)this.CmbWorkOrderNoHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWorkOrderNoHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbWorkOrderNoHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWorkOrderNoHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWorkOrderNoHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWorkOrderNoHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbWorkOrderNoHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWorkOrderNoHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWorkOrderNoHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance104).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWorkOrderNoHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbWorkOrderNoHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWorkOrderNoHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWorkOrderNoHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWorkOrderNoHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbWorkOrderNoHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWorkOrderNoHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWorkOrderNoHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbWorkOrderNoHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWorkOrderNoHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbWorkOrderNoHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWorkOrderNoHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWorkOrderNoHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWorkOrderNoHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWorkOrderNoHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWorkOrderNoHistory).Location = new System.Drawing.Point(829, 39);
		((System.Windows.Forms.Control)(object)this.CmbWorkOrderNoHistory).Name = "CmbWorkOrderNoHistory";
		((System.Windows.Forms.Control)(object)this.CmbWorkOrderNoHistory).Size = new System.Drawing.Size(138, 26);
		((System.Windows.Forms.Control)(object)this.CmbWorkOrderNoHistory).TabIndex = 8;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(741, 45);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(88, 15);
		this.label6.TabIndex = 180;
		this.label6.Text = "Work Order No";
		this.CmbItemNameHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemNameHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemNameHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemNameHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance86;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance143).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance146).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance146).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance146).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemNameHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).Location = new System.Drawing.Point(829, 12);
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).Name = "CmbItemNameHistory";
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).Size = new System.Drawing.Size(195, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).TabIndex = 7;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(741, 18);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(67, 15);
		this.label20.TabIndex = 180;
		this.label20.Text = "Item Name";
		this.CmbPlantNameHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbPlantNameHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPlantNameHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPlantNameHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPlantNameHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbPlantNameHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPlantNameHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantNameHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantNameHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance76;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantNameHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantNameHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbPlantNameHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPlantNameHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPlantNameHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPlantNameHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbPlantNameHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPlantNameHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantNameHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance89).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPlantNameHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbPlantNameHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPlantNameHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance90).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance90).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance90).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantNameHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance123).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPlantNameHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbPlantNameHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPlantNameHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPlantNameHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbPlantNameHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPlantNameHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbPlantNameHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPlantNameHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPlantNameHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPlantNameHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPlantNameHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPlantNameHistory).Location = new System.Drawing.Point(577, 12);
		((System.Windows.Forms.Control)(object)this.CmbPlantNameHistory).Name = "CmbPlantNameHistory";
		((System.Windows.Forms.Control)(object)this.CmbPlantNameHistory).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.CmbPlantNameHistory).TabIndex = 5;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(482, 18);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(69, 15);
		this.label18.TabIndex = 180;
		this.label18.Text = "Plant Name";
		this.CmbProductionTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbProductionTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbProductionTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbProductionTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance127).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance127).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance128;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance134).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance135).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance135).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance135).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance136).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbProductionTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbProductionTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbProductionTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbProductionTypeHistory).Location = new System.Drawing.Point(577, 39);
		((System.Windows.Forms.Control)(object)this.CmbProductionTypeHistory).Name = "CmbProductionTypeHistory";
		((System.Windows.Forms.Control)(object)this.CmbProductionTypeHistory).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.CmbProductionTypeHistory).TabIndex = 6;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(482, 45);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(94, 15);
		this.label3.TabIndex = 180;
		this.label3.Text = "Production Type";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(5, 18);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(60, 15);
		this.label19.TabIndex = 174;
		this.label19.Text = "Date Type";
		this.cmbDateTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbDateTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance111).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance111).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance112;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance113).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance113).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance117).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance118).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance118).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance118).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Location = new System.Drawing.Point(84, 12);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Name = "cmbDateTypeHistory";
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Size = new System.Drawing.Size(261, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).TabIndex = 0;
		this.cmbDateTypeHistory.ValueChanged += new System.EventHandler(cmbDateTypeHistory_ValueChanged);
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(215, 41);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.Size = new System.Drawing.Size(130, 22);
		this.ToDateHistory.TabIndex = 2;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(5, 45);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(79, 15);
		this.label28.TabIndex = 171;
		this.label28.Text = "Date From,To";
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(408, 41);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.Size = new System.Drawing.Size(70, 23);
		this.ToDocNoHistory.TabIndex = 4;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(84, 41);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.Size = new System.Drawing.Size(130, 22);
		this.FromDateHistory.TabIndex = 1;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(347, 45);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(45, 15);
		this.label5.TabIndex = 173;
		this.label5.Text = "Doc To";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(347, 18);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(60, 15);
		this.label35.TabIndex = 173;
		this.label35.Text = "Doc From";
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(408, 14);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.Size = new System.Drawing.Size(70, 23);
		this.FromDocNoHistory.TabIndex = 3;
		this.panel38.BackColor = System.Drawing.Color.Teal;
		this.panel38.Controls.Add(this.toolStrip2);
		this.panel38.Controls.Add(this.label32);
		this.panel38.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel38.Location = new System.Drawing.Point(0, 0);
		this.panel38.Name = "panel38";
		this.panel38.Size = new System.Drawing.Size(1176, 52);
		this.panel38.TabIndex = 48;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.toolStripSeparator1, this.BtnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1176, 27);
		this.toolStrip2.TabIndex = 3;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnHistoryRefresh_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.BtnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshHistory.Name = "BtnRefreshHistory";
		this.BtnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshHistory.Text = "&Refresh";
		this.BtnRefreshHistory.Click += new System.EventHandler(BtnRefreshHistory_Click);
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.White;
		this.label32.Location = new System.Drawing.Point(5, 31);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(176, 19);
		this.label32.TabIndex = 127;
		this.label32.Text = "Work Order History  ";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1184, 611);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmWorkOrderConcrete";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmWorkOrderConcrete_FormClosed);
		base.Load += new System.EventHandler(frmBillOfMaterial_Manufacturing_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmBillOfMaterial_Manufacturing_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdCustomerDetail).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInputMaterial).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionType).EndInit();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerOrderNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAttribute).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerName).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel13.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWorkOrderNoHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemNameHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantNameHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionTypeHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).EndInit();
		this.panel38.ResumeLayout(false);
		this.panel38.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
