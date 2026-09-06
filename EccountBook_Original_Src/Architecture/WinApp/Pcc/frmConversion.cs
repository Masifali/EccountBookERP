using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.BLL.Production;
using Architecture.BLL.pcc;
using Architecture.BLL.pcc.Production;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Production;
using Architecture.Model.pcc.Production;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Production;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.pcc;

public class frmConversion : Form
{
	private WaitForm WF = new WaitForm();

	private Attachment AT = new Attachment();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtInput = new DataTable();

	private DataTable dtOutPutByProduct = new DataTable();

	private DataTable dtOverHead = new DataTable();

	private DataTable dtContractorWages = new DataTable();

	private DataTable dtUom = new DataTable();

	private DataTable dtRateUom = new DataTable();

	private DataTable dtuomGetAll = new DataTable();

	private DataTable dtHistory = new DataTable();

	private DataTable dtAccounts = new DataTable();

	private DataTable lstitem = new DataTable();

	private DataTable dtAttributes = new DataTable();

	private DataTable dtWareForInput = new DataTable();

	private DataTable dtWareForOutPut = new DataTable();

	private DataTable dtContr = new DataTable();

	private DataTable DtStock = new DataTable();

	private DataTable dtOutputBom = new DataTable();

	private DataTable dtOutputItem = new DataTable();

	private DataTable listUOM = new DataTable();

	private DataTable dtitem = new DataTable();

	private DataTable dtItemAll = new DataTable();

	private int updateDetailIndex = -1;

	private bool FifoMethod;

	private bool UpdateMode;

	private int RecId;

	private bool CheckWarehouseExists = true;

	private List<ConversionInputDetail> lstRemoveRecordInput = new List<ConversionInputDetail>();

	private List<ConversionOutPutDetail> lstRemoveRecordOutPut = new List<ConversionOutPutDetail>();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton btnPrint;

	private TextBox txtQty;

	private Label label18;

	internal UltraCombo cmbUOM;

	private Label label44;

	internal UltraCombo cmbItemBOM;

	private Label label7;

	private TextBox txtOverHead;

	private Label label9;

	private TextBox txtTotalInputs;

	private Label label6;

	internal UltraCombo comCompany;

	private Label label4;

	internal UltraCombo combranches;

	private Label label45;

	internal UltraCombo comproject;

	private Label label46;

	private TabPage tabPage3;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewHistory;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton btnRefreshHistory;

	private Panel panel4;

	private Panel panel20;

	private Label label5;

	private Label label3;

	private GroupBox groupBox2;

	private Label label2;

	private Label label21;

	private Label label37;

	private DateTimePicker txtdocdate;

	private TextBox txtdocno;

	private Panel panel10;

	internal GridEX grdInputMaterial;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnDelete;

	private ToolStripButton btnshortcutkeys;

	private Panel panel38;

	private Label label32;

	private Panel panel45;

	private Panel panel8;

	private Panel panel19;

	internal GridEX grdHistoryInput;

	private Panel panel44;

	internal GridEX DatagridHistory;

	private Panel panel15;

	private Label label17;

	private Panel panel13;

	private GroupBox groupBox1;

	private Label label19;

	internal UltraCombo cmbDateTypeHistory;

	private DateTimePicker txtToDateHistory;

	private Label label28;

	private TextBox txtToDocNoHistory;

	private DateTimePicker txtFromdateHistory;

	private Label label35;

	private TextBox txtFromNoHistory;

	private Label label30;

	internal UltraCombo cmbConversionNoHistory;

	private Button btnshow;

	private CtrlGrdBar ctrlGrdBar1;

	private CheckBox ChkPreviewSlip;

	internal GridEX grdHistoryOutput;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator2;

	private TextBox txtRemarksHeader;

	private Panel panel11;

	private Label label23;

	private Panel panel7;

	private Label label1;

	private CtrlGrdBar ctrlGrdBar2;

	private TextBox txtWagesAmount;

	internal GridEX grdWagesHistory;

	private Panel panel12;

	private CtrlGrdBar ctrlGrdBar3;

	internal GridEX grdContractorWages;

	private Panel panel5;

	private Label label33;

	internal GridEX grdOverHead;

	private Panel panel14;

	private Label label22;

	private Panel panel17;

	private Panel panel6;

	private TabControl tabControl2;

	private TabPage TabOutPut;

	internal GridEX grdOutPutByProduct;

	private TabPage TabWages;

	private TabPage TabOverheads;

	private TextBox txtProductionNo;

	private Label label8;

	private ToolStripButton btnGenerateRatesOutput;

	private Label label13;

	private Label label24;

	private Button BtnUpdateWarehouseInput;

	private Button BtnUpdateWarehouseOutPut;

	private Label label14;

	private Panel panel18;

	private Label label40;

	private Label label15;

	internal GridEX grdHistoryOverHead;

	private Panel panel42;

	private Label label34;

	private ToolStripButton btnVoucherReport;

	private CheckBox chkPrintVoucher;

	internal UltraCombo CmbProductionType;

	private Label label16;

	internal UltraCombo CmbPlantName;

	private Label label26;

	private Button btnDefinePlant;

	private ToolStripButton btnattachment;

	private RadioButton RadNotReffered;

	private RadioButton RadReffered;

	private RadioButton RadAll;

	private GroupBox groupBox3;

	internal UltraCombo CmbWarehouseName;

	private Label label41;

	private TextBox txtVarientEquivalent;

	private Label label43;

	internal UltraCombo CmbAttribute;

	private Label label42;

	private Button BtnAddDetail;

	private Button BtnUpdateDetail;

	private Button BtnCancelDetail;

	internal UltraCombo CmbContractorForUpdateInWagesGrid;

	private Button BtnUpdateConctratorInWages;

	private Label label66;

	private Label label65;

	internal UltraCombo CmbActivityforContractorUpdateInWagesGrid;

	private Panel PanelMessage;

	private ToolStrip ToolStripMessage;

	private ToolStripButton BtnMessage;

	private Button btnClearInput;

	private RadioButton rdSearchByCode;

	private RadioButton rdSearchByName;

	private TextBox txtOutPutAutoRatePerQtyAmount;

	private TextBox txtTotalOutPutAutoRatedAmount;

	private Label label31;

	private Label label10;

	private Label label25;

	public frmConversion(UserAccount user)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmBillOfMaterial_Manufacturing_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1250, 750);
			WF.Show();
			FifoMethod = CommonServices.GetERPFeatureById(5);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			btnPrint.Enabled = formright.DoHavePrintRights;
			btnDelete.Enabled = formright.DoHaveCanDelete;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar2.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = formright.DoHaveGridExportRights;
			dtOutputItem.Columns.Add("OutItemId", typeof(int));
			dtOutputItem.Columns.Add("OutItemName", typeof(string));
			dtAccounts.Columns.Add("Id");
			dtAccounts.Columns.Add("AccountTitle");
			dtUom.Columns.Add("Id", typeof(int));
			dtUom.Columns.Add("ItemUOM", typeof(double));
			dtUom.Columns.Add("ItemUOMCode", typeof(string));
			dtRateUom.Columns.Add("RateId", typeof(int));
			dtRateUom.Columns.Add("RateUOM", typeof(double));
			dtRateUom.Columns.Add("RateUOMCode", typeof(string));
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName");
			dtitem.Columns.Add("ItemCode");
			dtitem.Columns.Add("InventoryParentCategoriesId", typeof(int));
			dtFillsForGrids();
			uomgetAll();
			Datetypefill();
			AllCombobind();
			DocumentNoFill();
			ProductionTypeFill();
			PlantFeader();
			ItemDetailFill(Conversion.ToInt(CmbProductionType.Value));
			UOMFill();
			dtInput.Columns.Add("Id", typeof(int));
			dtInput.Columns.Add("WarehouseId", typeof(int));
			dtInput.Columns.Add("WarehouseName", typeof(string));
			dtInput.Columns.Add("ItemId", typeof(int));
			dtInput.Columns.Add("ItemName", typeof(string));
			dtInput.Columns.Add("ItemUomId", typeof(int));
			dtInput.Columns.Add("Uom", typeof(decimal));
			dtInput.Columns.Add("UomCode", typeof(string));
			dtInput.Columns.Add("BalanceStock", typeof(decimal));
			dtInput.Columns.Add("ItemQty", typeof(decimal));
			dtInput.Columns.Add("ItemAttributeVarientId", typeof(int));
			dtInput.Columns.Add("AttributeVarient", typeof(string));
			dtInput.Columns.Add("VarientEquivalent", typeof(decimal));
			dtInput.Columns.Add("VarientQty", typeof(decimal));
			dtInput.Columns.Add("NetWeight", typeof(decimal));
			dtInput.Columns.Add("Rate", typeof(decimal));
			dtInput.Columns.Add("RateUomId", typeof(int));
			dtInput.Columns.Add("RateUom", typeof(decimal));
			dtInput.Columns.Add("RateUomCode", typeof(string));
			dtInput.Columns.Add("Amount", typeof(decimal));
			grdInputMaterial.DataSource = dtInput;
			AddRowInInputGrid();
			grdInputMaterial.RetrieveStructure();
			grdInputSettings();
			dtOutPutByProduct.Columns.Add("Id", typeof(int));
			dtOutPutByProduct.Columns.Add("WarehouseId", typeof(int));
			dtOutPutByProduct.Columns.Add("WarehouseName", typeof(string));
			dtOutPutByProduct.Columns.Add("ItemId", typeof(int));
			dtOutPutByProduct.Columns.Add("ItemName", typeof(string));
			dtOutPutByProduct.Columns.Add("ItemAttributeVarientId", typeof(int));
			dtOutPutByProduct.Columns.Add("AttributeVarient", typeof(string));
			dtOutPutByProduct.Columns.Add("VarientEquivalent", typeof(decimal));
			dtOutPutByProduct.Columns.Add("ItemUomId", typeof(int));
			dtOutPutByProduct.Columns.Add("Uom", typeof(decimal));
			dtOutPutByProduct.Columns.Add("UomCode", typeof(string));
			dtOutPutByProduct.Columns.Add("ItemQty", typeof(decimal));
			dtOutPutByProduct.Columns.Add("VarientQty", typeof(decimal));
			dtOutPutByProduct.Columns.Add("NetWeight", typeof(decimal));
			dtOutPutByProduct.Columns.Add("Rate", typeof(decimal));
			dtOutPutByProduct.Columns.Add("RateUomId", typeof(int));
			dtOutPutByProduct.Columns.Add("RateUom", typeof(decimal));
			dtOutPutByProduct.Columns.Add("RateUomCode", typeof(string));
			dtOutPutByProduct.Columns.Add("Amount", typeof(decimal));
			dtOutPutByProduct.Rows.Add(0, 0, "", 0, "", 0, "", 0, 0, 0, "", 0, 0, 0, 0, 0, 0, "", 0);
			grdOutPutByProduct.DataSource = dtOutPutByProduct;
			grdOutPutByProduct.RetrieveStructure();
			grdOutPutGridSetting();
			dtOverHead.Columns.Add("Id", typeof(int));
			dtOverHead.Columns.Add("OverHeadTypeId", typeof(int));
			dtOverHead.Columns.Add("OverHeadAccountId", typeof(int));
			dtOverHead.Columns.Add("PercentOfTotalInput", typeof(decimal));
			dtOverHead.Columns.Add("Qty", typeof(decimal));
			dtOverHead.Columns.Add("Rate", typeof(decimal));
			dtOverHead.Columns.Add("Amount", typeof(decimal));
			dtOverHead.Columns.Add("OhRemarks", typeof(string));
			grdOverHead.DataSource = dtOverHead;
			AddRowInOverHeadGrid();
			grdOverHead.RetrieveStructure();
			grdOverHeadGridSetting();
			dtContractorWages.Columns.Add("Id", typeof(int));
			dtContractorWages.Columns.Add("OutPutItemId", typeof(int));
			dtContractorWages.Columns.Add("OutPutItemName");
			dtContractorWages.Columns.Add("ContractorId", typeof(int));
			dtContractorWages.Columns.Add("ContractorName");
			dtContractorWages.Columns.Add("ContractorWagesRateScheduleId", typeof(int));
			dtContractorWages.Columns.Add("ServiceActivity");
			dtContractorWages.Columns.Add("Qty", typeof(decimal));
			dtContractorWages.Columns.Add("Rate", typeof(decimal));
			dtContractorWages.Columns.Add("Amount", typeof(decimal));
			dtContractorWages.Columns.Add("Remarks");
			AddRowInContractorWagesGrid();
			grdContractorWages.RetrieveStructure();
			grdContractorWagesGridSetting();
			txtdocdate.Focus();
		}
		catch (Exception ex)
		{
			WF.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			WF.Close();
		}
	}

	private void AddRowInInputGrid()
	{
		try
		{
			dtInput.Rows.Add(0, 0, "", 0, "", 0, 0, 0, 0, 0, 0, "", 0, 0, 0, 0, 0, 0, 0, 0);
			grdInputMaterial.DataSource = dtInput;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AddRowInOverHeadGrid()
	{
		dtOverHead.Rows.Add(0, 0, 0, 0, 0, 0, 0, "");
		grdOverHead.DataSource = dtOverHead;
	}

	private void dtFillsForGrids()
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		try
		{
			dtWareForOutPut.Clear();
			dtWareForOutPut = CommonServices.getActiveWareHouse();
			lstitem.Clear();
			lstitem = CommonServices.GetItemByItemTypeId("45,46,47");
			dtAttributes.Clear();
			dtAttributes = ItemAttributeVarient.GetAllForCombo(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToInt(cmbItemBOM.Value));
			new SaleOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			dtContr.Clear();
			SupplierCustomer supplier = new SupplierCustomer();
			supplier.OrganizationId = UserAccount.OrganizationId;
			supplier.CompanyId = UserAccount.CompanyId;
			dtContr = SupplierCustomer.ReadByOrganizationCompanyIdForContractorWages(supplier);
			DataTable dt = CommonServices.CoaAllocationGetAllServiceBind();
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			dtAccounts.Rows.Clear();
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 3 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 6 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 8 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 11 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 12 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 13 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 14 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 20 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 21)
				{
					dtAccounts.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["AccountTitle"]);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
			int code = ConversionHeader.GenerateCode(new ConversionHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 1867,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			txtdocno.Text = code.ToString();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void WarehouseFill(DataTable dtWare)
	{
		try
		{
			int ItId = Conversion.ToInt(CmbWarehouseName.Value);
			if (dtWare.Rows.Count > 0)
			{
				DDL.BindDDL(dtWare, CmbWarehouseName, "Id", "WareHouseName", "WareHouse", ZeroIndex: true);
				((UltraGridBase)CmbWarehouseName).DisplayLayout.Bands[0].Columns[1].Width = 350;
				if (ItId > 0)
				{
					if (dtWare.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId))
					{
						CmbWarehouseName.Value = ItId;
					}
					else
					{
						((Control)(object)CmbWarehouseName).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbWarehouseName).DataSource = null;
				((UltraGridBase)CmbWarehouseName).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AttributeVarientFill(int ItemId)
	{
		try
		{
			dtAttributes.Clear();
			int BomId = Conversion.ToInt(CmbAttribute.Value);
			dtAttributes = ItemAttributeVarient.GetAllForCombo(UserAccount.OrganizationId, UserAccount.CompanyId, ItemId);
			if (dtAttributes.Rows.Count > 0)
			{
				DDL.BindDDL(dtAttributes, CmbAttribute, "ItemAttributeVarientId", "ItemAttribute", "ItemAttribute", ZeroIndex: true);
				if (BomId > 0)
				{
					if (dtAttributes.AsEnumerable().Any((DataRow row) => row.Field<int>("ItemAttributeVarientId") == BomId))
					{
						CmbAttribute.Value = BomId;
					}
					else
					{
						((Control)(object)CmbAttribute).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbAttribute).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void bindRateUomAndItemPackUom()
	{
		try
		{
			if (Conversion.ToInt(cmbItemBOM.Value) > 0 && listUOM.Rows.Count > 0)
			{
				string Packuom = ((Control)(object)cmbUOM).Text;
				((Control)(object)cmbUOM).Text = string.Empty;
				DataTable dtUOM = new DataTable();
				dtUOM.Columns.Add("Id");
				dtUOM.Columns.Add("UOM");
				dtUOM.Columns.Add("UOMCode");
				dtUOM.Rows.Clear();
				for (int i = 0; i < listUOM.Rows.Count; i++)
				{
					if (Conversion.ToInt(listUOM.Rows[i]["ItemId"]) == Conversion.ToInt(cmbItemBOM.Value))
					{
						dtUOM.Rows.Add(listUOM.Rows[i]["Id"], listUOM.Rows[i]["UOMCode"], listUOM.Rows[i]["Equivalent"]);
					}
				}
				if (dtUOM.Rows.Count > 0)
				{
					DDL.BindDDL(dtUOM, cmbUOM, "Id", "UOM", "UOM", ZeroIndex: true);
					((UltraGridBase)cmbUOM).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					((UltraGridBase)cmbUOM).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
					if (dtUOM.AsEnumerable().Any((DataRow row) => row.Field<string>("UOM") == Packuom))
					{
						((Control)(object)cmbUOM).Text = Packuom;
					}
					else
					{
						((Control)(object)cmbUOM).Text = string.Empty;
					}
				}
				else
				{
					((Control)(object)cmbUOM).Text = string.Empty;
					((UltraGridBase)cmbUOM).DataSource = null;
				}
			}
			else
			{
				((Control)(object)cmbUOM).Text = string.Empty;
				((UltraGridBase)cmbUOM).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbBOMNo_Leave(object sender, EventArgs e)
	{
	}

	private void uomgetAll()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			UOMSchedule obj = new UOMSchedule();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtuomGetAll = UOMSchedule.Getall(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void bindUom(int ItemId)
	{
		try
		{
			dtUom.Clear();
			for (int i = 0; i < dtuomGetAll.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtuomGetAll.Rows[i]["ItemId"]) == Conversion.ToInt((object)ItemId))
				{
					dtUom.Rows.Add(dtuomGetAll.Rows[i]["Id"], dtuomGetAll.Rows[i]["Equivalent"], dtuomGetAll.Rows[i]["UOMCode"]);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void bindRateUom(int ItemId)
	{
		try
		{
			dtRateUom.Clear();
			if (dtuomGetAll.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtuomGetAll.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtuomGetAll.Rows[i]["ItemId"]) == Conversion.ToInt((object)ItemId))
				{
					dtRateUom.Rows.Add(dtuomGetAll.Rows[i]["Id"], dtuomGetAll.Rows[i]["Equivalent"], dtuomGetAll.Rows[i]["UOMCode"]);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void WorkOrderNoFill()
	{
	}

	private void ProductionTypeFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		try
		{
			int BomId = Conversion.ToInt(CmbProductionType.Value);
			DataTable dt = InvLookUp.GetLookupsByTypeIdDt(new InvLookUp
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				InvLookupTypeId = 6
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dttype = new DataTable();
				dttype.Columns.Add("Id", typeof(int));
				dttype.Columns.Add("ProductionName", typeof(string));
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToInt(dt.Rows[i]["Id"]) == 45 || Conversion.ToInt(dt.Rows[i]["Id"]) == 46 || Conversion.ToInt(dt.Rows[i]["Id"]) == 47)
					{
						dttype.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["LookupName"]);
					}
				}
				if (dttype.Rows.Count > 0)
				{
					DDL.BindDDL(dttype, CmbProductionType, "Id", "ProductionName", "Production Type", ZeroIndex: true);
					((UltraGridBase)CmbProductionType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
					((UltraGridBase)CmbProductionType).Rows[1].Activate();
					if (BomId > 0)
					{
						if (dttype.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == BomId))
						{
							CmbProductionType.Value = BomId;
						}
						else
						{
							((Control)(object)CmbProductionType).Text = string.Empty;
						}
					}
				}
				else
				{
					((Control)(object)CmbProductionType).Text = string.Empty;
					((UltraGridBase)CmbProductionType).DataSource = null;
				}
			}
			else
			{
				((Control)(object)CmbProductionType).Text = string.Empty;
				((UltraGridBase)CmbProductionType).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PlantFeader()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		try
		{
			int BomId = Conversion.ToInt(CmbPlantName.Value);
			DataTable dtProductionplant = new DataTable();
			dtProductionplant = InvProductionPlant.GetAll(new InvProductionPlant
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtProductionplant.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtProductionplant, CmbPlantName, "Id", "Description", "Plant", true);
				if (BomId > 0)
				{
					if (dtProductionplant.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == BomId))
					{
						CmbPlantName.Value = BomId;
					}
					else
					{
						((Control)(object)CmbPlantName).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbPlantName).DataSource = null;
			}
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

	private void cmbItem_Leave(object sender, EventArgs e)
	{
		try
		{
			AttributeVarientFill(Conversion.ToInt(cmbItemBOM.Value));
			bindRateUomAndItemPackUom();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPlantName_Leave(object sender, EventArgs e)
	{
		try
		{
			dtWareForInput = CommonServices.WareHouseAllocationToPlantsByProductionPlantId(Conversion.ToInt(CmbPlantName.Value));
			if (dtWareForInput.Rows.Count > 0)
			{
				int warehouseId = 0;
				WarehouseFill(dtWareForInput);
				DataRow firstRow = (from row in dtWareForInput.AsEnumerable()
					where row.Field<int>("Id") > 0
					select row).FirstOrDefault();
				if (firstRow != null)
				{
					warehouseId = Conversion.ToInt(firstRow["Id"]);
				}
				if (Conversion.ToInt(CmbWarehouseName.Value) == 0)
				{
					CmbWarehouseName.Value = warehouseId;
				}
			}
			EnableDisablePlant();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UOMFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			UOMSchedule obj = new UOMSchedule();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			listUOM = UOMSchedule.Getall(obj);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDetailFill(int TypeId)
	{
		try
		{
			int ID = Conversion.ToInt(cmbItemBOM.Value);
			dtitem.Rows.Clear();
			dtItemAll = CommonServices.GetItemByItemTypeId(TypeId.ToString());
			if (dtItemAll.Rows.Count > 0)
			{
				for (int i = 0; i < dtItemAll.Rows.Count; i++)
				{
					dtitem.Rows.Add(dtItemAll.Rows[i]["Id"], dtItemAll.Rows[i]["ItemName"], dtItemAll.Rows[i]["ItemCodeNew"], dtItemAll.Rows[i]["InventoryParentCategoriesId"]);
				}
			}
			if (dtitem.Rows.Count > 0)
			{
				if (rdSearchByName.Checked)
				{
					DDL.BindDDL(dtitem, cmbItemBOM, "Id", "ItemName", "Item", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitem, cmbItemBOM, "Id", "ItemCode", "Item", ZeroIndex: true);
				}
				((UltraGridBase)cmbItemBOM).DisplayLayout.Bands[0].Columns[1].Width = 300;
				((UltraGridBase)cmbItemBOM).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				if (ID > 0)
				{
					if (dtitem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						cmbItemBOM.Value = ID;
					}
					else
					{
						((Control)(object)cmbItemBOM).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbItemBOM).Text = string.Empty;
				((UltraGridBase)cmbItemBOM).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnLoadInputFromBom_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbPlantName.Value) == 0)
			{
				CmbPlantName.Focus();
				throw new Exception("Please Select Plant Name First");
			}
			FillWagesGridFilterCombo();
		}
		catch (Exception ex)
		{
			WF.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			WF.Close();
		}
	}

	private bool FormValidation()
	{
		if (txtdocno.Text.Trim() == string.Empty || txtdocno.Text == "0")
		{
			MessageBox.Show("Doc No Field Required");
			txtdocno.Focus();
			return false;
		}
		if (txtProductionNo.Text.Trim() == string.Empty || txtProductionNo.Text.Trim() == null)
		{
			MessageBox.Show("Production No Field Required");
			txtProductionNo.Focus();
			return false;
		}
		if (((Control)(object)CmbProductionType).Text.Trim() == string.Empty || Conversion.ToInt(CmbProductionType.Value) == 0)
		{
			MessageBox.Show("Production Type Feild Required");
			CmbProductionType.Focus();
			return false;
		}
		if (((Control)(object)CmbPlantName).Text.Trim() == string.Empty || Conversion.ToInt(CmbPlantName.Value) == 0)
		{
			MessageBox.Show("Plant Name Feild Required");
			CmbPlantName.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationOutPutGrid()
	{
		if (((Control)(object)cmbItemBOM).Text.Trim() == string.Empty || Conversion.ToInt(cmbItemBOM.Value) == 0)
		{
			MessageBox.Show("ItemBOM Feild Required");
			cmbItemBOM.Focus();
			return false;
		}
		if (txtQty.Text.Trim() == string.Empty || txtQty.Text == "0" || Conversion.ToDouble((object)txtQty.Text) == 0.0)
		{
			MessageBox.Show("Qty Field Required");
			txtQty.Focus();
			return false;
		}
		if (((Control)(object)CmbAttribute).Text.Trim() == string.Empty || Conversion.ToInt(CmbAttribute.Value) == 0)
		{
			MessageBox.Show("Item Attribute Field Required");
			CmbAttribute.Focus();
			return false;
		}
		if (txtVarientEquivalent.Text.Trim() == string.Empty || txtVarientEquivalent.Text == "0" || Conversion.ToDouble((object)txtVarientEquivalent.Text) == 0.0)
		{
			MessageBox.Show("Varient Equivalent Field Required");
			txtVarientEquivalent.Focus();
			return false;
		}
		if (((Control)(object)CmbWarehouseName).Text.Trim() == string.Empty || Conversion.ToInt(CmbWarehouseName.Value) == 0)
		{
			MessageBox.Show("Warehouse Field Required");
			CmbWarehouseName.Focus();
			return false;
		}
		return true;
	}

	private void CmbProductionType_Leave(object sender, EventArgs e)
	{
		try
		{
			ItemDetailFill(Conversion.ToInt(CmbProductionType.Value));
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
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdOutPutByProduct.GetDataRows().Count() > 0)
			{
				GridEXRow r = grdOutPutByProduct.GetDataRows().FirstOrDefault();
				GridEXRow rr = grdInputMaterial.GetDataRows().FirstOrDefault();
				if (r != null && (int)r.RowType == 0 && Conversion.ToInt(r.Cells["ItemId"].Value) > 0)
				{
					((Control)(object)CmbProductionType).Enabled = false;
				}
				else if (rr != null && (int)rr.RowType == 0 && Conversion.ToInt(rr.Cells["ItemId"].Value) > 0)
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

	private void EnableDisablePlant()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdInputMaterial.GetDataRows().Count() > 0)
			{
				GridEXRow r = grdInputMaterial.GetDataRows().FirstOrDefault();
				if (r != null && (int)r.RowType == 0 && Conversion.ToInt(r.Cells["WareHouseId"].Value) > 0)
				{
					((Control)(object)CmbPlantName).Enabled = false;
				}
				else
				{
					((Control)(object)CmbPlantName).Enabled = true;
				}
			}
			else
			{
				((Control)(object)CmbPlantName).Enabled = true;
			}
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
			grdInputMaterial.RootTable.Columns["Id"].Visible = false;
			grdInputMaterial.RootTable.Columns["WarehouseId"].Visible = false;
			grdInputMaterial.RootTable.Columns["ItemId"].Visible = false;
			grdInputMaterial.RootTable.Columns["ItemUomId"].Visible = false;
			grdInputMaterial.RootTable.Columns["Uom"].Visible = false;
			grdInputMaterial.RootTable.Columns["RateUomId"].Visible = false;
			grdInputMaterial.RootTable.Columns["RateUom"].Visible = false;
			grdInputMaterial.RootTable.Columns["RateUomCode"].Visible = false;
			grdInputMaterial.RootTable.Columns["NetWeight"].Visible = false;
			grdInputMaterial.RootTable.Columns["ItemAttributeVarientId"].Visible = false;
			grdInputMaterial.RootTable.Columns["UomCode"].Caption = "Uom";
			grdInputMaterial.RootTable.Columns["RateUomCode"].Caption = "Rate Uom";
			grdInputMaterial.RootTable.Columns["Rate"].Caption = "Avg Rate";
			grdInputMaterial.RootTable.Columns["WarehouseName"].Caption = "Warehouse";
			grdInputMaterial.RootTable.Columns["AttributeVarient"].Caption = "Variant";
			grdInputMaterial.RootTable.Columns["VarientQty"].Caption = "Variant Qty";
			grdInputMaterial.RootTable.Columns["VarientEquivalent"].Caption = "Variant Equivalent";
			grdInputMaterial.RootTable.HeaderLines = 2;
			grdInputMaterial.RootTable.Columns["Id"].HeaderAlignment = (TextAlignment)2;
			grdInputMaterial.RootTable.Columns["WarehouseId"].HeaderAlignment = (TextAlignment)2;
			grdInputMaterial.RootTable.Columns["WarehouseName"].HeaderAlignment = (TextAlignment)2;
			grdInputMaterial.RootTable.Columns["AttributeVarient"].HeaderAlignment = (TextAlignment)2;
			grdInputMaterial.RootTable.Columns["VarientQty"].HeaderAlignment = (TextAlignment)2;
			grdInputMaterial.RootTable.Columns["VarientEquivalent"].HeaderAlignment = (TextAlignment)2;
			grdInputMaterial.RootTable.Columns["ItemId"].HeaderAlignment = (TextAlignment)2;
			grdInputMaterial.RootTable.Columns["ItemName"].HeaderAlignment = (TextAlignment)2;
			grdInputMaterial.RootTable.Columns["ItemUomId"].HeaderAlignment = (TextAlignment)2;
			grdInputMaterial.RootTable.Columns["Uom"].HeaderAlignment = (TextAlignment)2;
			grdInputMaterial.RootTable.Columns["UomCode"].HeaderAlignment = (TextAlignment)2;
			grdInputMaterial.RootTable.Columns["BalanceStock"].HeaderAlignment = (TextAlignment)2;
			grdInputMaterial.RootTable.Columns["RateUomId"].HeaderAlignment = (TextAlignment)2;
			grdInputMaterial.RootTable.Columns["Rate"].HeaderAlignment = (TextAlignment)2;
			grdInputMaterial.RootTable.Columns["RateUom"].HeaderAlignment = (TextAlignment)2;
			grdInputMaterial.RootTable.Columns["RateUomCode"].HeaderAlignment = (TextAlignment)2;
			grdInputMaterial.RootTable.Columns["NetWeight"].HeaderAlignment = (TextAlignment)2;
			grdInputMaterial.RootTable.Columns["Amount"].HeaderAlignment = (TextAlignment)2;
			grdInputMaterial.RootTable.Columns["WarehouseId"].Width = Constants.InventoryConstants.WareHouseName;
			grdInputMaterial.RootTable.Columns["WarehouseName"].Width = Constants.InventoryConstants.WareHouseName;
			grdInputMaterial.RootTable.Columns["AttributeVarient"].Width = Constants.InventoryConstants.Variant;
			grdInputMaterial.RootTable.Columns["VarientQty"].Width = Constants.InventoryConstants.Variant;
			grdInputMaterial.RootTable.Columns["VarientEquivalent"].Width = Constants.InventoryConstants.Variant;
			grdInputMaterial.RootTable.Columns["ItemId"].Width = Constants.InventoryConstants.ItemName;
			grdInputMaterial.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdInputMaterial.RootTable.Columns["Uom"].Width = Constants.InventoryConstants.UomConstant;
			grdInputMaterial.RootTable.Columns["UomCode"].Width = Constants.InventoryConstants.UomConstant;
			grdInputMaterial.RootTable.Columns["BalanceStock"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grdInputMaterial.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdInputMaterial.RootTable.Columns["RateUom"].Width = Constants.InventoryConstants.UomConstant;
			grdInputMaterial.RootTable.Columns["RateUomCode"].Width = Constants.InventoryConstants.UomConstant;
			grdInputMaterial.RootTable.Columns["NetWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdInputMaterial.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdInputMaterial.RootTable.Columns["Id"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["WarehouseId"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["WarehouseName"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["AttributeVarient"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["VarientQty"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["VarientEquivalent"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["ItemUomId"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["Uom"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["UomCode"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["BalanceStock"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["RateUomId"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["Rate"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["RateUom"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["RateUomCode"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["NetWeight"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdInputMaterial.GroupByBoxVisible = true;
			grdInputMaterial.RootTable.GroupTotals = (GroupTotals)2;
			grdInputMaterial.RootTable.Columns["ItemQty"].EditType = (EditType)1;
			grdInputMaterial.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdInputMaterial.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["ItemQty"].FormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["ItemQty"].TotalFormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["VarientEquivalent"].AggregateFunction = (AggregateFunction)2;
			grdInputMaterial.RootTable.Columns["VarientEquivalent"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["VarientEquivalent"].FormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["VarientEquivalent"].TotalFormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["VarientQty"].AggregateFunction = (AggregateFunction)2;
			grdInputMaterial.RootTable.Columns["VarientQty"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["VarientQty"].FormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["VarientQty"].TotalFormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["BalanceStock"].AggregateFunction = (AggregateFunction)2;
			grdInputMaterial.RootTable.Columns["BalanceStock"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["BalanceStock"].FormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["BalanceStock"].TotalFormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["Uom"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["Uom"].FormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["Uom"].TotalFormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["RateUom"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["RateUom"].FormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["RateUom"].TotalFormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)3;
			grdInputMaterial.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdInputMaterial.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdInputMaterial.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdInputMaterial.RootTable.Columns["NetWeight"].FormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["NetWeight"].TotalFormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdInputMaterial.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdInputMaterial.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdInputMaterial.RootTable.Columns.Add("Delete");
			grdInputMaterial.RootTable.Columns["Delete"].Key = "Delete";
			grdInputMaterial.RootTable.Columns["Delete"].Caption = "X";
			grdInputMaterial.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInputMaterial.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdInputMaterial.RootTable.Columns["Delete"].ButtonText = "X";
			grdInputMaterial.RootTable.Columns["Delete"].Width = 20;
			grdInputMaterial.RootTable.Columns["Delete"].Position = 0;
			grdInputMaterial.RootTable.Columns.Add("Add");
			grdInputMaterial.RootTable.Columns["Add"].Key = "Add";
			grdInputMaterial.RootTable.Columns["Add"].Caption = "+";
			grdInputMaterial.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInputMaterial.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdInputMaterial.RootTable.Columns["Add"].ButtonText = "+";
			grdInputMaterial.RootTable.Columns["Add"].Width = 20;
			grdInputMaterial.RootTable.Columns["Add"].Position = 1;
			grdInputMaterial.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdInputMaterial_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0506: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Expected O, but got Unknown
		try
		{
			grdInputMaterial.UpdateData();
			GridEXRow item = grdInputMaterial.CurrentRow;
			if (e.Column.Key == "ItemName")
			{
				item.Cells["ItemUomId"].Value = 0;
				item.Cells["Uom"].Value = 0;
				item.Cells["UomCode"].Value = "";
				item.Cells["RateUom"].Value = 0;
				item.Cells["RateUomId"].Value = 0;
				item.Cells["RateUomCode"].Value = "";
				item.Cells["NetWeight"].Value = 0;
				item.Cells["Amount"].Value = 0;
			}
			if ((e.Column.Key == "ItemQty" || e.Column.Key == "Uom" || e.Column.Key == "ItemUomId" || e.Column.Key == "AttributeVarient") && item != null && (int)item.RowType == 0)
			{
				if (Conversion.ToDouble(item.Cells["ItemQty"].Value) != 0.0 && item.Cells["ItemUomId"].Value.ToString() != string.Empty)
				{
					decimal num = Conversion.ToDecimal((object)item.Cells["ItemQty"].Value.ToString());
					decimal quom = Conversion.ToDecimal((object)item.Cells["Uom"].Value.ToString());
					decimal Weight = num * quom;
					item.Cells["NetWeight"].Value = Weight;
					if (!FifoMethod)
					{
						Conversion.ToDecimal(DtStock.Rows[0]["AvgRate"]);
					}
					else
					{
						ReportsParameters obj1 = new ReportsParameters();
						obj1.OrganizationId = UserAccount.OrganizationId;
						obj1.CompanyId = UserAccount.CompanyId;
						obj1.ItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
						obj1.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
						obj1.WarehouseId = Conversion.ToInt(item.Cells["WarehouseId"].Value);
						obj1.stockUOM = Conversion.ToInt(item.Cells["ItemAttributeVarientId"].Value);
						obj1.Id = RecId;
						if (obj1.Id > 0)
						{
							obj1.DocumentTypeId = 1867;
						}
						obj1.ItemQty = Conversion.ToDouble(item.Cells["ItemQty"].Value);
						obj1.NetWeight = obj1.ItemQty;
						double AvgRat = CommonServices.GetAvgRateFromFIFOMethodConcrete(obj1);
						Conversion.ToDecimal((object)AvgRat);
						item.Cells["Rate"].Value = AvgRat;
					}
				}
				else
				{
					item.Cells["NetWeight"].Value = 0;
				}
				if (Conversion.ToDecimal(item.Cells["Rate"].Value) != 0m && Conversion.ToInt(item.Cells["RateUomId"].Value) != 0)
				{
					decimal Iuom2 = default(decimal);
					decimal num2 = Conversion.ToDecimal((object)item.Cells["ItemQty"].Value.ToString());
					Iuom2 = Conversion.ToDecimal((object)item.Cells["Uom"].Value.ToString());
					decimal rate2 = Conversion.ToDecimal((object)item.Cells["Rate"].Value.ToString());
					decimal Ruom2 = Conversion.ToDecimal((object)item.Cells["RateUom"].Value.ToString());
					decimal Amount2 = num2 * Iuom2 / Ruom2 * rate2;
					item.Cells["Amount"].Value = Amount2;
				}
				else
				{
					item.Cells["Amount"].Value = 0;
				}
			}
			if ((e.Column.Key == "Rate" || e.Column.Key == "RateUomId" || e.Column.Key == "RateUom") && item != null && (int)item.RowType == 0)
			{
				decimal Iuom = default(decimal);
				if (!(item.Cells["ItemQty"].Value.ToString() != string.Empty) || !(item.Cells["ItemUomId"].Value.ToString() != string.Empty))
				{
					throw new Exception("ItemQty And ItemUom Should be greater than zero...");
				}
				decimal num3 = Conversion.ToDecimal((object)item.Cells["ItemQty"].Value.ToString());
				Iuom = Conversion.ToDecimal((object)item.Cells["Uom"].Value.ToString());
				_ = num3 * Iuom;
				if (Conversion.ToDecimal(item.Cells["Rate"].Value) != 0m && Conversion.ToInt(item.Cells["RateUomId"].Value) != 0)
				{
					decimal num4 = Conversion.ToDecimal((object)item.Cells["ItemQty"].Value.ToString());
					Iuom = Conversion.ToDecimal((object)item.Cells["Uom"].Value.ToString());
					decimal rate = Conversion.ToDecimal((object)item.Cells["Rate"].Value.ToString());
					decimal Ruom = Conversion.ToDecimal((object)item.Cells["RateUom"].Value.ToString());
					decimal Amount = num4 * Iuom / Ruom * rate;
					item.Cells["Amount"].Value = Amount;
				}
				else
				{
					item.Cells["Amount"].Value = 0;
				}
			}
			grdInputMaterial.UpdateData();
			Calculation();
			EnableDisablePlant();
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
				Calculation();
				EnableDisablePlant();
				EnableDisableProductionType();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteInputDetailRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
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
				ConversionInputDetail d1 = new ConversionInputDetail();
				d1.Id = Conversion.ToInt(r.Cells["Id"].Value);
				d1.WareHouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
				d1.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				d1.ItemUomId = Conversion.ToInt((object)r.Cells["ItemUomId"].Text);
				d1.ItemQty = Conversion.ToDecimal((object)r.Cells["ItemQty"].Text);
				d1.ItemAttributeVarientId = Conversion.ToInt((object)r.Cells["ItemAttributeVarientId"].Text);
				d1.ItemAttribute = Conversion.ToString((object)r.Cells["AttributeVarient"].Text);
				d1.VarientEquivalent = Conversion.ToDecimal((object)r.Cells["VarientEquivalent"].Text);
				d1.VarientQty = Conversion.ToDecimal((object)r.Cells["VarientQty"].Text);
				d1.NetWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
				d1.Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
				d1.RateUomId = Conversion.ToInt((object)r.Cells["RateUomId"].Text);
				d1.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
				d1.ActionTypeId = 3;
				lstRemoveRecordInput.Add(d1);
				dtInput.Rows[r.RowIndex].Delete();
				r.Delete();
				grdInputMaterial.UpdateData();
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

	private void grdOutPutByProduct_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdOutPutByProduct.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				updateDetailIndex = r.RowIndex;
				cmbItemBOM.Value = r.Cells["ItemId"].Value.ToString();
				cmbItem_Leave(null, null);
				cmbUOM.Value = Conversion.ToInt(r.Cells["ItemUomId"].Value);
				CmbAttribute.Value = r.Cells["ItemAttributeVarientId"].Value.ToString();
				CmbAttribute_Leave(null, null);
				CmbWarehouseName.Value = r.Cells["WareHouseId"].Value.ToString();
				BtnAddDetail.Visible = false;
				BtnUpdateDetail.Visible = true;
				BtnCancelDetail.Visible = true;
				cmbItemBOM.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnAddDetail_Click(object sender, EventArgs e)
	{
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!FormValidationOutPutGrid())
			{
				return;
			}
			GridEXRow[] dataRows = grdOutPutByProduct.GetDataRows();
			foreach (GridEXRow obj in dataRows)
			{
				int ItemId = Conversion.ToInt(obj.Cells["ItemId"].Value);
				int WareHouseId = Conversion.ToInt(obj.Cells["WareHouseId"].Value);
				if (Conversion.ToInt(cmbItemBOM.Value) == ItemId && Conversion.ToInt(CmbWarehouseName.Value) == WareHouseId)
				{
					MessageBox.Show("Duplicate Record not added to the grid. An item with the same BOM No and WareHouse already exists");
					return;
				}
			}
			if (grdOutPutByProduct.GetDataRows().Count() == 1)
			{
				GridEXRow r = grdOutPutByProduct.GetDataRows().FirstOrDefault();
				if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && Conversion.ToInt(r.Cells["ItemId"].Value) == 0)
				{
					dtOutPutByProduct.Clear();
					grdOutPutByProduct.ClearStructure();
				}
			}
			dtOutPutByProduct.Rows.Add(0, Conversion.ToInt(CmbWarehouseName.Value), Conversion.ToString((object)((Control)(object)CmbWarehouseName).Text), Conversion.ToInt(cmbItemBOM.Value), Conversion.ToString((object)((Control)(object)cmbItemBOM).Text), Conversion.ToInt(CmbAttribute.Value), Conversion.ToString((object)((Control)(object)CmbAttribute).Text), Conversion.ToDecimal((object)txtVarientEquivalent.Text), Conversion.ToInt(cmbUOM.Value), Conversion.ToDecimal(((UltraDropDownBase)cmbUOM).SelectedRow.Cells[2].Value), Conversion.ToString((object)((Control)(object)cmbUOM).Text), Conversion.ToDecimal((object)txtQty.Text), Conversion.ToDecimal((object)txtQty.Text) * Conversion.ToDecimal((object)txtVarientEquivalent.Text), Conversion.ToDecimal((object)txtQty.Text) * Conversion.ToDecimal(((UltraDropDownBase)cmbUOM).SelectedRow.Cells[2].Value), 0, Conversion.ToInt(cmbUOM.Value), Conversion.ToDecimal(((UltraDropDownBase)cmbUOM).SelectedRow.Cells[2].Value), Conversion.ToString((object)((Control)(object)cmbUOM).Text), 0);
			grdOutPutByProduct.DataSource = dtOutPutByProduct;
			grdOutPutByProduct.RetrieveStructure();
			grdOutPutGridSetting();
			ResetDetailOutPut();
			EnableDisableProductionType();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationOutPutGrid())
			{
				return;
			}
			GridEXRow[] dataRows = grdOutPutByProduct.GetDataRows();
			foreach (GridEXRow item in dataRows)
			{
				int ItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
				int WareHouseId = Conversion.ToInt(item.Cells["WareHouseId"].Value);
				if (Conversion.ToInt(cmbItemBOM.Value) == ItemId && Conversion.ToInt(CmbWarehouseName.Value) == WareHouseId && item.RowIndex != updateDetailIndex)
				{
					MessageBox.Show("Duplicate Record not added to the grid. An item with the same BOM No and WareHouse already exists");
					return;
				}
			}
			dtOutPutByProduct.Rows[updateDetailIndex]["WarehouseId"] = Conversion.ToInt(CmbWarehouseName.Value);
			dtOutPutByProduct.Rows[updateDetailIndex]["WarehouseName"] = Conversion.ToString((object)((Control)(object)CmbWarehouseName).Text);
			dtOutPutByProduct.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(cmbItemBOM.Value);
			dtOutPutByProduct.Rows[updateDetailIndex]["ItemName"] = Conversion.ToString((object)((Control)(object)cmbItemBOM).Text);
			dtOutPutByProduct.Rows[updateDetailIndex]["ItemAttributeVarientId"] = Conversion.ToInt(CmbAttribute.Value);
			dtOutPutByProduct.Rows[updateDetailIndex]["AttributeVarient"] = Conversion.ToString((object)((Control)(object)CmbAttribute).Text);
			dtOutPutByProduct.Rows[updateDetailIndex]["VarientEquivalent"] = Conversion.ToDecimal((object)txtVarientEquivalent.Text);
			dtOutPutByProduct.Rows[updateDetailIndex]["ItemUomId"] = Conversion.ToInt(cmbUOM.Value);
			dtOutPutByProduct.Rows[updateDetailIndex]["Uom"] = Conversion.ToDecimal(((UltraDropDownBase)cmbUOM).SelectedRow.Cells[2].Value);
			dtOutPutByProduct.Rows[updateDetailIndex]["UomCode"] = Conversion.ToString((object)((Control)(object)cmbUOM).Text);
			dtOutPutByProduct.Rows[updateDetailIndex]["ItemUomId"] = Conversion.ToInt(cmbUOM.Value);
			dtOutPutByProduct.Rows[updateDetailIndex]["Uom"] = Conversion.ToDecimal(((UltraDropDownBase)cmbUOM).SelectedRow.Cells[2].Value);
			dtOutPutByProduct.Rows[updateDetailIndex]["UomCode"] = Conversion.ToString((object)((Control)(object)cmbUOM).Text);
			dtOutPutByProduct.Rows[updateDetailIndex]["ItemQty"] = Conversion.ToDecimal((object)txtQty.Text);
			dtOutPutByProduct.Rows[updateDetailIndex]["VarientQty"] = Conversion.ToDecimal((object)txtQty.Text) * Conversion.ToDecimal((object)txtVarientEquivalent.Text);
			dtOutPutByProduct.Rows[updateDetailIndex]["NetWeight"] = Conversion.ToDecimal((object)txtQty.Text) * Conversion.ToDecimal(((UltraDropDownBase)cmbUOM).SelectedRow.Cells[2].Value);
			BtnAddDetail.Visible = true;
			BtnUpdateDetail.Visible = false;
			BtnCancelDetail.Visible = false;
			ResetDetailOutPut();
			EnableDisableProductionType();
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
			ResetDetailOutPut();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInOutPutByProductGrid()
	{
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdOutPutByProduct.GetDataRows().Count() == 0)
			{
				dtOutPutByProduct.Rows.Add(0, 0, "", 0, "", 0, "", 0, 0, 0, "", 0, 0, 0, 0, 0, 0, "", 0);
			}
			else
			{
				int WarehouseId = 0;
				string WarehouseName = "";
				int ItemId = 0;
				string ItemName = "";
				int ItemAttributeVarientId = 0;
				string AttributeVarient = "";
				decimal VarientEquivalent = default(decimal);
				int ItemUomId = 0;
				decimal Uom = default(decimal);
				string UomCode = "";
				decimal ItemQty = default(decimal);
				decimal VarientQty = default(decimal);
				decimal NetWeight = default(decimal);
				decimal Rate = default(decimal);
				GridEXRow r = grdOutPutByProduct.CurrentRow;
				if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
				{
					ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					if (ItemId == 0)
					{
						GridEXRow[] dataRows = grdOutPutByProduct.GetDataRows();
						foreach (GridEXRow row in dataRows)
						{
							ItemId = Conversion.ToInt(row.Cells["ItemId"].Value);
							if (ItemId != 0)
							{
								ItemName = Conversion.ToString(row.Cells["ItemName"].Value);
								WarehouseId = Conversion.ToInt(row.Cells["WarehouseId"].Value);
								WarehouseName = Conversion.ToString(row.Cells["WarehouseName"].Value);
								ItemAttributeVarientId = Conversion.ToInt(row.Cells["ItemAttributeVarientId"].Value);
								AttributeVarient = Conversion.ToString(row.Cells["AttributeVarient"].Value);
								VarientEquivalent = Conversion.ToDecimal(row.Cells["VarientEquivalent"].Value);
								ItemUomId = Conversion.ToInt(row.Cells["ItemUomId"].Value);
								Uom = Conversion.ToDecimal(row.Cells["Uom"].Value);
								UomCode = Conversion.ToString(row.Cells["UomCode"].Value);
								ItemQty = Conversion.ToDecimal(row.Cells["ItemQty"].Value);
								VarientQty = Conversion.ToDecimal(row.Cells["VarientQty"].Value);
								NetWeight = Conversion.ToDecimal(row.Cells["NetWeight"].Value);
								Rate = Conversion.ToDecimal(row.Cells["Rate"].Value);
								Conversion.ToInt(row.Cells["RateUomId"].Value);
								break;
							}
						}
					}
					else
					{
						ItemName = Conversion.ToString(r.Cells["ItemName"].Value);
						WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
						WarehouseName = Conversion.ToString(r.Cells["WarehouseName"].Value);
						ItemAttributeVarientId = Conversion.ToInt(r.Cells["ItemAttributeVarientId"].Value);
						AttributeVarient = Conversion.ToString(r.Cells["AttributeVarient"].Value);
						VarientEquivalent = Conversion.ToDecimal(r.Cells["VarientEquivalent"].Value);
						ItemUomId = Conversion.ToInt(r.Cells["ItemUomId"].Value);
						Uom = Conversion.ToDecimal(r.Cells["Uom"].Value);
						UomCode = Conversion.ToString(r.Cells["UomCode"].Value);
						ItemQty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
						VarientQty = Conversion.ToDecimal(r.Cells["VarientQty"].Value);
						NetWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
						Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
						Conversion.ToInt(r.Cells["RateUomId"].Value);
					}
				}
				dtOutPutByProduct.Rows.Add(0, WarehouseId, WarehouseName, ItemId, ItemName, ItemAttributeVarientId, AttributeVarient, VarientEquivalent, ItemUomId, Uom, UomCode, ItemQty, VarientQty, NetWeight, Rate, ItemUomId, Uom, UomCode, 0);
			}
			grdOutPutByProduct.DataSource = dtOutPutByProduct;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdOutPutGridSetting()
	{
		try
		{
			grdOutPutByProduct.RootTable.Columns["Id"].Visible = false;
			grdOutPutByProduct.RootTable.Columns["RateUomId"].Visible = false;
			grdOutPutByProduct.RootTable.Columns["ItemUomId"].Visible = false;
			grdOutPutByProduct.RootTable.Columns["UomCode"].Visible = false;
			grdOutPutByProduct.RootTable.Columns["RateUom"].Visible = false;
			grdOutPutByProduct.RootTable.Columns["RateUomCode"].Visible = false;
			grdOutPutByProduct.RootTable.Columns["NetWeight"].Visible = false;
			grdOutPutByProduct.RootTable.Columns["ItemId"].Visible = false;
			grdOutPutByProduct.RootTable.Columns["WareHouseId"].Visible = false;
			grdOutPutByProduct.RootTable.Columns["ItemAttributeVarientId"].Visible = false;
			grdOutPutByProduct.RootTable.Columns["Uom"].Visible = false;
			grdOutPutByProduct.RootTable.HeaderLines = 2;
			grdOutPutByProduct.RootTable.Columns["UomCode"].Caption = "Uom";
			grdOutPutByProduct.RootTable.Columns["RateUomCode"].Caption = "Rate Uom";
			grdOutPutByProduct.RootTable.Columns["VarientQty"].Caption = "T.Qty";
			grdOutPutByProduct.RootTable.Columns["AttributeVarient"].Caption = "Variant";
			grdOutPutByProduct.RootTable.Columns["VarientEquivalent"].Caption = "Variant Unit";
			grdOutPutByProduct.RootTable.Columns["ItemQty"].Caption = "Qty";
			grdOutPutByProduct.RootTable.Columns["WarehouseName"].Caption = "Warehouse";
			grdOutPutByProduct.RootTable.Columns["Id"].HeaderAlignment = (TextAlignment)2;
			grdOutPutByProduct.RootTable.Columns["VarientQty"].HeaderAlignment = (TextAlignment)2;
			grdOutPutByProduct.RootTable.Columns["WareHouseId"].HeaderAlignment = (TextAlignment)2;
			grdOutPutByProduct.RootTable.Columns["WareHouseName"].HeaderAlignment = (TextAlignment)2;
			grdOutPutByProduct.RootTable.Columns["ItemId"].HeaderAlignment = (TextAlignment)2;
			grdOutPutByProduct.RootTable.Columns["ItemName"].HeaderAlignment = (TextAlignment)2;
			grdOutPutByProduct.RootTable.Columns["ItemAttributeVarientId"].HeaderAlignment = (TextAlignment)2;
			grdOutPutByProduct.RootTable.Columns["AttributeVarient"].HeaderAlignment = (TextAlignment)2;
			grdOutPutByProduct.RootTable.Columns["VarientEquivalent"].HeaderAlignment = (TextAlignment)2;
			grdOutPutByProduct.RootTable.Columns["ItemUomId"].HeaderAlignment = (TextAlignment)2;
			grdOutPutByProduct.RootTable.Columns["Uom"].HeaderAlignment = (TextAlignment)2;
			grdOutPutByProduct.RootTable.Columns["UomCode"].HeaderAlignment = (TextAlignment)2;
			grdOutPutByProduct.RootTable.Columns["NetWeight"].HeaderAlignment = (TextAlignment)2;
			grdOutPutByProduct.RootTable.Columns["Rate"].HeaderAlignment = (TextAlignment)2;
			grdOutPutByProduct.RootTable.Columns["RateUomId"].HeaderAlignment = (TextAlignment)2;
			grdOutPutByProduct.RootTable.Columns["RateUom"].HeaderAlignment = (TextAlignment)2;
			grdOutPutByProduct.RootTable.Columns["RateUomCode"].HeaderAlignment = (TextAlignment)2;
			grdOutPutByProduct.RootTable.Columns["Amount"].HeaderAlignment = (TextAlignment)2;
			grdOutPutByProduct.RootTable.Columns["ItemQty"].HeaderAlignment = (TextAlignment)2;
			grdOutPutByProduct.RootTable.Columns["VarientQty"].Width = Constants.InventoryConstants.Variant;
			grdOutPutByProduct.RootTable.Columns["WareHouseId"].Width = Constants.InventoryConstants.WareHouseName;
			grdOutPutByProduct.RootTable.Columns["WareHouseName"].Width = Constants.InventoryConstants.WareHouseName;
			grdOutPutByProduct.RootTable.Columns["ItemId"].Width = Constants.InventoryConstants.ItemName;
			grdOutPutByProduct.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdOutPutByProduct.RootTable.Columns["AttributeVarient"].Width = Constants.InventoryConstants.Variant;
			grdOutPutByProduct.RootTable.Columns["VarientEquivalent"].Width = Constants.InventoryConstants.Variant;
			grdOutPutByProduct.RootTable.Columns["Uom"].Width = Constants.InventoryConstants.UomConstant;
			grdOutPutByProduct.RootTable.Columns["UomCode"].Width = Constants.InventoryConstants.UomConstant;
			grdOutPutByProduct.RootTable.Columns["NetWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdOutPutByProduct.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdOutPutByProduct.RootTable.Columns["RateUom"].Width = Constants.InventoryConstants.ExRateConstant;
			grdOutPutByProduct.RootTable.Columns["RateUomCode"].Width = Constants.InventoryConstants.UomConstant;
			grdOutPutByProduct.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdOutPutByProduct.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdOutPutByProduct.RootTable.Columns["Id"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["VarientQty"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["WareHouseId"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["WareHouseName"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["ItemAttributeVarientId"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["AttributeVarient"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["VarientEquivalent"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["ItemUomId"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["Uom"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["UomCode"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["NetWeight"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["Rate"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["RateUomId"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["RateUom"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["RateUomCode"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["ItemQty"].EditType = (EditType)1;
			grdOutPutByProduct.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdOutPutByProduct.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdOutPutByProduct.RootTable.Columns["ItemQty"].FormatString = "##,#.####";
			grdOutPutByProduct.RootTable.Columns["ItemQty"].TotalFormatString = "##,#.####";
			grdOutPutByProduct.RootTable.Columns["VarientEquivalent"].AggregateFunction = (AggregateFunction)2;
			grdOutPutByProduct.RootTable.Columns["VarientEquivalent"].TextAlignment = (TextAlignment)3;
			grdOutPutByProduct.RootTable.Columns["VarientEquivalent"].FormatString = "##,#.####";
			grdOutPutByProduct.RootTable.Columns["VarientEquivalent"].TotalFormatString = "##,#.####";
			grdOutPutByProduct.RootTable.Columns["VarientQty"].AggregateFunction = (AggregateFunction)2;
			grdOutPutByProduct.RootTable.Columns["VarientQty"].TextAlignment = (TextAlignment)3;
			grdOutPutByProduct.RootTable.Columns["VarientQty"].FormatString = "##,#.####";
			grdOutPutByProduct.RootTable.Columns["VarientQty"].TotalFormatString = "##,#.####";
			grdOutPutByProduct.RootTable.Columns["Uom"].TextAlignment = (TextAlignment)3;
			grdOutPutByProduct.RootTable.Columns["Uom"].FormatString = "##,#.####";
			grdOutPutByProduct.RootTable.Columns["Uom"].TotalFormatString = "##,#.####";
			grdOutPutByProduct.RootTable.Columns["RateUom"].TextAlignment = (TextAlignment)3;
			grdOutPutByProduct.RootTable.Columns["RateUom"].FormatString = "##,#.####";
			grdOutPutByProduct.RootTable.Columns["RateUom"].TotalFormatString = "##,#.####";
			grdOutPutByProduct.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)3;
			grdOutPutByProduct.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdOutPutByProduct.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdOutPutByProduct.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdOutPutByProduct.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdOutPutByProduct.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdOutPutByProduct.RootTable.Columns["NetWeight"].FormatString = "##,#.####";
			grdOutPutByProduct.RootTable.Columns["NetWeight"].TotalFormatString = "##,#.####";
			grdOutPutByProduct.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdOutPutByProduct.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdOutPutByProduct.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdOutPutByProduct.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdOutPutByProduct.RootTable.Columns.Add("Delete");
			grdOutPutByProduct.RootTable.Columns["Delete"].Key = "Delete";
			grdOutPutByProduct.RootTable.Columns["Delete"].Caption = "X";
			grdOutPutByProduct.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOutPutByProduct.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdOutPutByProduct.RootTable.Columns["Delete"].ButtonText = "X";
			grdOutPutByProduct.RootTable.Columns["Delete"].Width = 20;
			grdOutPutByProduct.RootTable.Columns["Delete"].Position = 0;
			grdOutPutByProduct.RootTable.Columns.Add("Add");
			grdOutPutByProduct.RootTable.Columns["Add"].Key = "Add";
			grdOutPutByProduct.RootTable.Columns["Add"].Caption = "+";
			grdOutPutByProduct.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOutPutByProduct.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdOutPutByProduct.RootTable.Columns["Add"].ButtonText = "+";
			grdOutPutByProduct.RootTable.Columns["Add"].Width = 20;
			grdOutPutByProduct.RootTable.Columns["Add"].Position = 1;
			grdOutPutByProduct.FrozenColumns = 2;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdOutPutByProduct_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdOutPutByProduct.UpdateData();
			GridEXRow item = grdOutPutByProduct.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (Conversion.ToInt(item.Cells["ItemId"].Value) == 0)
				{
					if (Conversion.ToInt(cmbItemBOM.Value) != 0)
					{
						item.Cells["ItemId"].Value = Conversion.ToInt(cmbItemBOM.Value);
						item.Cells["ItemName"].Value = Conversion.ToString((object)((Control)(object)cmbItemBOM).Text);
					}
					if (Conversion.ToInt(cmbUOM.Value) != 0)
					{
						item.Cells["ItemUomId"].Value = Conversion.ToInt(cmbUOM.Value);
						item.Cells["Uom"].Value = Conversion.ToInt(((UltraDropDownBase)cmbUOM).SelectedRow.Cells[2].Value);
						item.Cells["UomCode"].Value = Conversion.ToString(((UltraDropDownBase)cmbUOM).SelectedRow.Cells[1].Value);
						item.Cells["RateUomId"].Value = Conversion.ToInt(cmbUOM.Value);
						item.Cells["RateUom"].Value = Conversion.ToInt(((UltraDropDownBase)cmbUOM).SelectedRow.Cells[2].Value);
						item.Cells["RateUomCode"].Value = Conversion.ToString(((UltraDropDownBase)cmbUOM).SelectedRow.Cells[1].Value);
					}
				}
				if (Conversion.ToInt(item.Cells["ItemId"].Value) > 0 && Conversion.ToInt(item.Cells["ItemAttributeVarientId"].Value) == 0)
				{
					dtAttributes.Clear();
					int ItemAttributeVarientId = 0;
					dtAttributes = ItemAttributeVarient.GetAllForCombo(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToInt(item.Cells["ItemId"].Value));
					if (dtAttributes.Rows.Count > 0)
					{
						DataRow firstRow = dtAttributes.AsEnumerable().FirstOrDefault();
						if (firstRow != null)
						{
							ItemAttributeVarientId = Conversion.ToInt(firstRow["ItemAttributeVarientId"]);
						}
						DataRow foundRow = dtAttributes.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("ItemAttributeVarientId") == ItemAttributeVarientId);
						string AttributeVarient = ((foundRow != null) ? Conversion.ToString(foundRow["ItemAttribute"]) : "");
						decimal VarientEquivalent2 = ((foundRow != null) ? Conversion.ToDecimal(foundRow["VarientEquivalent"]) : 0m);
						item.Cells["ItemAttributeVarientId"].Value = ItemAttributeVarientId;
						item.Cells["AttributeVarient"].Value = AttributeVarient;
						item.Cells["VarientEquivalent"].Value = VarientEquivalent2;
					}
				}
				if (e.Column.Key == "ItemQty")
				{
					if (item.Cells["ItemQty"].Value.ToString() != string.Empty && item.Cells["ItemUomId"].Value.ToString() != string.Empty)
					{
						decimal num = Conversion.ToDecimal((object)item.Cells["ItemQty"].Value.ToString());
						decimal VarientEquivalent = Conversion.ToDecimal((object)item.Cells["VarientEquivalent"].Value.ToString());
						decimal VarientQty = num * VarientEquivalent;
						item.Cells["VarientQty"].Value = VarientQty;
						decimal CurrentRowAmount = Conversion.ToDecimal(item.Cells["Rate"].Value) * VarientQty;
						item.Cells["Amount"].Value = CurrentRowAmount;
					}
					else
					{
						item.Cells["VarientQty"].Value = 0;
						item.Cells["Amount"].Value = 0;
					}
				}
				grdOutPutByProduct.UpdateData();
			}
			EnableDisableProductionType();
			Calculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOutPutByProduct_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdOutPutByProduct.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteOutputDetailRow(r);
				}
				if (e.Column.Key == "Add")
				{
					AddRowInOutPutByProductGrid();
				}
				grdOutPutByProduct.UpdateData();
				Calculation();
				EnableDisableProductionType();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteOutputDetailRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (updateDetailIndex == -1)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					ConversionOutPutDetail d1 = new ConversionOutPutDetail();
					d1.Id = Conversion.ToInt(r.Cells["Id"].Value);
					d1.WareHouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
					d1.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					d1.ItemAttributeVarientId = Conversion.ToInt(r.Cells["ItemAttributeVarientId"].Value);
					d1.ItemUomId = 0;
					d1.ItemQty = Conversion.ToDecimal((object)r.Cells["ItemQty"].Text);
					d1.VarientQty = Conversion.ToDecimal((object)r.Cells["VarientQty"].Text);
					d1.VarientEquivalent = Conversion.ToDecimal((object)r.Cells["VarientEquivalent"].Text);
					d1.NetWeight = 0m;
					d1.Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
					d1.RateUomId = 0;
					d1.Amount = 0m;
					d1.ActionTypeId = 3;
					lstRemoveRecordOutPut.Add(d1);
					dtOutPutByProduct.Rows[r.RowIndex].Delete();
					r.Delete();
					grdOutPutByProduct.UpdateData();
				}
				else
				{
					r.Delete();
				}
				if (dtOutPutByProduct.Rows.Count == 0)
				{
					AddRowInOutPutByProductGrid();
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

	private void grdOutPutByProduct_CellValueChanged(object sender, ColumnActionEventArgs e)
	{
	}

	private void GetOutPutGridBomNos()
	{
		try
		{
			if (grdOutPutByProduct.GetDataRows().Count() > 0)
			{
				dtOutputBom.Clear();
				GridEXRow[] dataRows = grdOutPutByProduct.GetDataRows();
				foreach (GridEXRow r in dataRows)
				{
					if (!dtOutputBom.AsEnumerable().Any((DataRow row) => row.Field<int>("BomHeaderId") == Conversion.ToInt(r.Cells["BomHeaderId"].Value)))
					{
						dtOutputBom.Rows.Add(Conversion.ToInt(r.Cells["BomHeaderId"].Value), Conversion.ToString(r.Cells["BomNo"].Value));
					}
				}
				return;
			}
			throw new Exception("Out put Grid Record No Found...");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GetOutPutGridItemsForWages(int BomId)
	{
		try
		{
			if (grdOutPutByProduct.GetDataRows().Count() > 0)
			{
				dtOutputItem.Clear();
				GridEXRow[] dataRows = grdOutPutByProduct.GetDataRows();
				foreach (GridEXRow r in dataRows)
				{
					if (!dtOutputItem.AsEnumerable().Any((DataRow row) => row.Field<int>("OutItemId") == Conversion.ToInt(r.Cells["ItemId"].Value)))
					{
						dtOutputItem.Rows.Add(Conversion.ToInt(r.Cells["ItemId"].Value), Conversion.ToString(r.Cells["ItemName"].Value));
					}
				}
				return;
			}
			throw new Exception("Out put Grid Record No Found...");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdOverHeadGridSetting()
	{
		try
		{
			grdOverHead.RootTable.Columns["Id"].Visible = false;
			grdOverHead.RootTable.Columns["OverHeadTypeId"].Visible = false;
			grdOverHead.RootTable.Columns["PercentOfTotalInput"].Caption = "Percentage";
			grdOverHead.RootTable.Columns["Id"].EditType = (EditType)0;
			grdOverHead.RootTable.Columns["OverHeadTypeId"].EditType = (EditType)0;
			grdOverHead.RootTable.Columns["Qty"].EditType = (EditType)1;
			grdOverHead.RootTable.Columns["Rate"].EditType = (EditType)1;
			grdOverHead.RootTable.Columns["PercentOfTotalInput"].EditType = (EditType)1;
			grdOverHead.RootTable.Columns["Amount"].EditType = (EditType)1;
			grdOverHead.RootTable.HeaderLines = 2;
			grdOverHead.RootTable.Columns["OverHeadTypeId"].HeaderAlignment = (TextAlignment)2;
			grdOverHead.RootTable.Columns["Qty"].HeaderAlignment = (TextAlignment)2;
			grdOverHead.RootTable.Columns["Rate"].HeaderAlignment = (TextAlignment)2;
			grdOverHead.RootTable.Columns["PercentOfTotalInput"].HeaderAlignment = (TextAlignment)2;
			grdOverHead.RootTable.Columns["Amount"].HeaderAlignment = (TextAlignment)2;
			grdOverHead.RootTable.Columns["OverHeadTypeId"].Width = Constants.InventoryConstants.DocumentType;
			grdOverHead.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
			grdOverHead.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdOverHead.RootTable.Columns["PercentOfTotalInput"].Width = Constants.InventoryConstants.PercentOfTotal;
			grdOverHead.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdOverHead.RootTable.GroupTotals = (GroupTotals)2;
			grdOverHead.RootTable.Columns["PercentOfTotalInput"].AggregateFunction = (AggregateFunction)2;
			grdOverHead.RootTable.Columns["PercentOfTotalInput"].TextAlignment = (TextAlignment)3;
			grdOverHead.RootTable.Columns["PercentOfTotalInput"].FormatString = "##,#.####";
			grdOverHead.RootTable.Columns["PercentOfTotalInput"].TotalFormatString = "##,#.####";
			grdOverHead.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdOverHead.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdOverHead.RootTable.Columns["Qty"].FormatString = "##,#.####";
			grdOverHead.RootTable.Columns["Qty"].TotalFormatString = "##,#.####";
			grdOverHead.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdOverHead.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdOverHead.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdOverHead.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdOverHead.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdOverHead.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdOverHead.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			if (dtAccounts.Rows.Count > 0)
			{
				grdOverHead.RootTable.Columns["OverHeadAccountId"].ColumnType = (ColumnType)0;
				grdOverHead.RootTable.Columns["OverHeadAccountId"].LimitToList = true;
				grdOverHead.RootTable.Columns["OverHeadAccountId"].HasValueList = true;
				grdOverHead.RootTable.Columns["OverHeadAccountId"].EditType = (EditType)4;
				grdOverHead.RootTable.Columns["OverHeadAccountId"].Caption = "AccountTitle";
				grdOverHead.RootTable.Columns["OverHeadAccountId"].ValueList.PopulateValueList((IEnumerable)dtAccounts.DefaultView, "Id", "AccountTitle");
				grdOverHead.RootTable.Columns["OverHeadAccountId"].Width = Constants.InventoryConstants.AccountTitle;
			}
			grdOverHead.RootTable.Columns.Add("Delete");
			grdOverHead.RootTable.Columns["Delete"].Key = "Delete";
			grdOverHead.RootTable.Columns["Delete"].Caption = "X";
			grdOverHead.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOverHead.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdOverHead.RootTable.Columns["Delete"].ButtonText = "X";
			grdOverHead.RootTable.Columns["Delete"].Width = 20;
			grdOverHead.RootTable.Columns["Delete"].Position = 0;
			grdOverHead.RootTable.Columns.Add("Add");
			grdOverHead.RootTable.Columns["Add"].Key = "Add";
			grdOverHead.RootTable.Columns["Add"].Caption = "+";
			grdOverHead.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOverHead.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdOverHead.RootTable.Columns["Add"].ButtonText = "+";
			grdOverHead.RootTable.Columns["Add"].Width = 20;
			grdOverHead.RootTable.Columns["Add"].Position = 1;
			grdOverHead.FrozenColumns = 2;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdOverHead_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdOverHead.UpdateData();
			GridEXRow item = grdOverHead.CurrentRow;
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item != null && (int)item.RowType == 0)
			{
				decimal Amount = default(decimal);
				decimal Iuom = default(decimal);
				if (!(item.Cells["Qty"].Value.ToString() != string.Empty) || !(item.Cells["Rate"].Value.ToString() != string.Empty))
				{
					throw new Exception("Qty And Rate Should be greater than zero...");
				}
				decimal num = Conversion.ToDecimal((object)item.Cells["Qty"].Value.ToString());
				Iuom = Conversion.ToDecimal((object)item.Cells["Rate"].Text.ToString());
				Amount = num * Iuom;
				item.Cells["Amount"].Value = Amount;
			}
			if (e.Column.Key == "PercentOfTotalInput" && item != null && (int)item.RowType == 0)
			{
				double TotalPercentage = 0.0;
				GridEXRow[] dataRows = grdOverHead.GetDataRows();
				foreach (GridEXRow r in dataRows)
				{
					TotalPercentage += Conversion.ToDouble(r.Cells["PercentOfTotalInput"].Value);
				}
				if (TotalPercentage > 100.0)
				{
					double Percent = Conversion.ToDouble(grdOverHead.CurrentRow.Cells["PercentOfTotalInput"].Value);
					double i = TotalPercentage - 100.0;
					grdOverHead.CurrentRow.Cells["PercentOfTotalInput"].Value = Percent - i;
					throw new Exception("TotalPercentage Can not be Greater than 100...");
				}
				if (item.Cells["PercentOfTotalInput"].Value.ToString() != string.Empty)
				{
					double num2 = Conversion.ToDouble((object)grdInputMaterial.GetTotal(grdInputMaterial.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
					double Percentage = Conversion.ToDouble((object)item.Cells["PercentOfTotalInput"].Value.ToString());
					double TotlaPercentage = num2 / 100.0 * Percentage;
					if (TotlaPercentage > 0.0)
					{
						item.Cells["Amount"].Value = Math.Round(TotlaPercentage, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					}
					else
					{
						item.Cells["Amount"].Value = 0;
					}
					item.Cells["Qty"].Value = 0;
					item.Cells["Rate"].Value = 0;
				}
			}
			if (e.Column.Key == "Amount")
			{
				item.Cells["Qty"].Value = 0;
				item.Cells["Rate"].Value = 0;
			}
			grdOverHead.UpdateData();
			Calculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOverHead_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdOverHead.CurrentRow;
				dtOverHead.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtOverHead.Rows.Count == 0)
				{
					AddRowInOverHeadGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInOverHeadGrid();
			}
			grdOverHead.UpdateData();
			Calculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInContractorWagesGrid()
	{
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdContractorWages.GetDataRows().Count() == 0)
			{
				dtContractorWages.Rows.Add(0, 0, "", 0, "", 0, "", 0, 0, 0, "");
			}
			else
			{
				int OutPutItemId = 0;
				string OutPutItemName = "";
				int ContractorId = 0;
				string ContractorName = "";
				GridEXRow r = grdContractorWages.CurrentRow;
				if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
				{
					OutPutItemId = Conversion.ToInt(r.Cells["OutPutItemId"].Value);
					if (OutPutItemId == 0)
					{
						GridEXRow[] dataRows = grdContractorWages.GetDataRows();
						foreach (GridEXRow row in dataRows)
						{
							OutPutItemId = Conversion.ToInt(row.Cells["OutPutItemId"].Value);
							if (OutPutItemId != 0)
							{
								OutPutItemName = Conversion.ToString(row.Cells["OutPutItemName"].Value);
								ContractorId = Conversion.ToInt(row.Cells["ContractorId"].Value);
								ContractorName = Conversion.ToString(row.Cells["ContractorName"].Value);
								break;
							}
						}
					}
					else
					{
						OutPutItemName = Conversion.ToString(r.Cells["OutPutItemName"].Value);
						ContractorId = Conversion.ToInt(r.Cells["ContractorId"].Value);
						ContractorName = Conversion.ToString(r.Cells["ContractorName"].Value);
					}
				}
				dtContractorWages.Rows.Add(0, OutPutItemId, OutPutItemName, ContractorId, ContractorName, 0, "", 0, 0, 0, "");
			}
			grdContractorWages.DataSource = dtContractorWages;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdContractorWagesComboFill()
	{
	}

	private void grdContractorWagesGridSetting()
	{
		try
		{
			grdContractorWagesComboFill();
			grdContractorWages.RootTable.Columns["Id"].Visible = false;
			grdContractorWages.RootTable.Columns["ContractorId"].Visible = false;
			grdContractorWages.RootTable.Columns["ContractorWagesRateScheduleId"].Visible = false;
			grdContractorWages.RootTable.Columns["OutPutItemId"].Visible = false;
			grdContractorWages.RootTable.Columns["Id"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["OutPutItemId"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["OutPutItemName"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["ContractorId"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["ContractorName"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["ContractorWagesRateScheduleId"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["ServiceActivity"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["Qty"].EditType = (EditType)1;
			grdContractorWages.RootTable.Columns["Rate"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdContractorWages.RootTable.HeaderLines = 2;
			grdContractorWages.RootTable.Columns["OutPutItemName"].HeaderAlignment = (TextAlignment)2;
			grdContractorWages.RootTable.Columns["ContractorName"].HeaderAlignment = (TextAlignment)2;
			grdContractorWages.RootTable.Columns["ServiceActivity"].HeaderAlignment = (TextAlignment)2;
			grdContractorWages.RootTable.Columns["Qty"].HeaderAlignment = (TextAlignment)2;
			grdContractorWages.RootTable.Columns["Rate"].HeaderAlignment = (TextAlignment)2;
			grdContractorWages.RootTable.Columns["Amount"].HeaderAlignment = (TextAlignment)2;
			grdContractorWages.RootTable.Columns["OutPutItemName"].Width = Constants.InventoryConstants.ItemName;
			grdContractorWages.RootTable.Columns["ContractorName"].Width = Constants.InventoryConstants.SupplierName;
			grdContractorWages.RootTable.Columns["ServiceActivity"].Width = Constants.InventoryConstants.ServiceActivity;
			grdContractorWages.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
			grdContractorWages.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdContractorWages.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdContractorWages.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdContractorWages.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdContractorWages.RootTable.Columns["Qty"].FormatString = "##,#.####";
			grdContractorWages.RootTable.Columns["Qty"].TotalFormatString = "##,#.####";
			grdContractorWages.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdContractorWages.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdContractorWages.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdContractorWages.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdContractorWages.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdContractorWages.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdContractorWages.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdContractorWages.RootTable.Columns.Add("Delete");
			grdContractorWages.RootTable.Columns["Delete"].Key = "Delete";
			grdContractorWages.RootTable.Columns["Delete"].Caption = "X";
			grdContractorWages.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdContractorWages.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdContractorWages.RootTable.Columns["Delete"].ButtonText = "X";
			grdContractorWages.RootTable.Columns["Delete"].Width = 20;
			grdContractorWages.RootTable.Columns["Delete"].Position = 0;
			grdContractorWages.RootTable.Columns.Add("Add");
			grdContractorWages.RootTable.Columns["Add"].Key = "Add";
			grdContractorWages.RootTable.Columns["Add"].Caption = "+";
			grdContractorWages.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdContractorWages.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdContractorWages.RootTable.Columns["Add"].ButtonText = "+";
			grdContractorWages.RootTable.Columns["Add"].Width = 20;
			grdContractorWages.RootTable.Columns["Add"].Position = 1;
			grdContractorWages.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdContractorWages_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdContractorWages.UpdateData();
			GridEXRow item = grdContractorWages.CurrentRow;
			if (e.Column.Key == "Qty" && item != null && (int)item.RowType == 0)
			{
				decimal Amount = default(decimal);
				decimal Iuom = default(decimal);
				if (!(item.Cells["Qty"].Value.ToString() != string.Empty) || !(item.Cells["Rate"].Value.ToString() != string.Empty))
				{
					throw new Exception("Qty And Rate Should be greater than zero...");
				}
				decimal num = Conversion.ToDecimal((object)item.Cells["Qty"].Value.ToString());
				Iuom = Conversion.ToDecimal((object)item.Cells["Rate"].Text.ToString());
				Amount = num * Iuom;
				item.Cells["Amount"].Value = Amount;
			}
			grdContractorWages.UpdateData();
			Calculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdContractorWages_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdContractorWages.CurrentRow;
				dtContractorWages.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtContractorWages.Rows.Count == 0)
				{
					AddRowInContractorWagesGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInContractorWagesGrid();
			}
			grdContractorWages.UpdateData();
			Calculation();
			FillWagesGridFilterCombo();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillWagesGridFilterCombo()
	{
		try
		{
			int Id1 = Conversion.ToInt(CmbActivityforContractorUpdateInWagesGrid.Value);
			string Id2 = Conversion.ToString((object)((Control)(object)CmbContractorForUpdateInWagesGrid).Text);
			if (grdContractorWages.GetDataRows().Count() > 0 && dtContractorWages.Rows.Count > 0)
			{
				DataTable distinctRowsActivity = (from row in dtContractorWages.AsEnumerable()
					group row by row.Field<string>("ServiceActivity") into @group
					select @group.First()).CopyToDataTable();
				if (dtContr.Rows.Count > 0)
				{
					DDL.BindDDLNew(dtContr, CmbContractorForUpdateInWagesGrid, "Id", "CompanyName", "ContractorName", true);
					if (Id1 > 0)
					{
						if (dtContr.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id1))
						{
							CmbContractorForUpdateInWagesGrid.Value = Id1;
						}
						else
						{
							((Control)(object)CmbContractorForUpdateInWagesGrid).Text = string.Empty;
						}
					}
				}
				else
				{
					((Control)(object)CmbContractorForUpdateInWagesGrid).Text = string.Empty;
					((UltraGridBase)CmbContractorForUpdateInWagesGrid).DataSource = null;
				}
				if (distinctRowsActivity.Rows.Count > 0)
				{
					DDL.BindDDLNew(distinctRowsActivity, CmbActivityforContractorUpdateInWagesGrid, "ContractorWagesRateScheduleId", "ServiceActivity", "ServiceActivity", true);
					if (Id2 != null && Id2 != string.Empty)
					{
						if (distinctRowsActivity.AsEnumerable().Any((DataRow row) => row.Field<string>("ServiceActivity") == Id2))
						{
							((Control)(object)CmbActivityforContractorUpdateInWagesGrid).Text = Id2;
						}
						else
						{
							((Control)(object)CmbActivityforContractorUpdateInWagesGrid).Text = string.Empty;
						}
					}
				}
				else
				{
					((Control)(object)CmbActivityforContractorUpdateInWagesGrid).Text = string.Empty;
					((UltraGridBase)CmbActivityforContractorUpdateInWagesGrid).DataSource = null;
				}
			}
			else
			{
				((Control)(object)CmbContractorForUpdateInWagesGrid).Text = string.Empty;
				((UltraGridBase)CmbContractorForUpdateInWagesGrid).DataSource = null;
				((Control)(object)CmbActivityforContractorUpdateInWagesGrid).Text = string.Empty;
				((UltraGridBase)CmbActivityforContractorUpdateInWagesGrid).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ResetDetailOutPut()
	{
		try
		{
			updateDetailIndex = -1;
			((Control)(object)cmbItemBOM).Text = "";
			((UltraGridBase)cmbUOM).DataSource = null;
			((Control)(object)cmbUOM).Text = "";
			txtQty.Text = "";
			((UltraGridBase)CmbAttribute).DataSource = null;
			txtVarientEquivalent.Text = "";
			((Control)(object)CmbAttribute).Text = "";
			((Control)(object)CmbWarehouseName).Text = "";
			((Control)(object)CmbAttribute).Visible = true;
			txtVarientEquivalent.Visible = true;
			((Control)(object)CmbWarehouseName).Visible = true;
			BtnAddDetail.Visible = true;
			BtnUpdateDetail.Visible = false;
			BtnCancelDetail.Visible = false;
			CmbWarehouseName.Focus();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		FromReset();
	}

	public void FromReset()
	{
		try
		{
			RecId = 0;
			AT = new Attachment();
			Attachment.dtAttachments.Clear();
			DocumentNoFill();
			((Control)(object)cmbItemBOM).Text = string.Empty;
			((Control)(object)cmbUOM).Text = string.Empty;
			txtQty.Text = string.Empty;
			dtInput.Rows.Clear();
			grdInputMaterial.ClearStructure();
			AddRowInInputGrid();
			grdInputMaterial.RetrieveStructure();
			grdInputSettings();
			dtOverHead.Rows.Clear();
			grdOverHead.ClearStructure();
			dtContractorWages.Rows.Clear();
			grdContractorWages.ClearStructure();
			lstRemoveRecordInput.Clear();
			lstRemoveRecordOutPut.Clear();
			dtOutPutByProduct.Rows.Clear();
			grdOutPutByProduct.ClearStructure();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			UpdateMode = false;
			AddRowInOutPutByProductGrid();
			grdOutPutByProduct.RetrieveStructure();
			grdOutPutGridSetting();
			AddRowInOverHeadGrid();
			grdOverHead.RetrieveStructure();
			grdOverHeadGridSetting();
			AddRowInContractorWagesGrid();
			grdContractorWages.RetrieveStructure();
			grdContractorWagesGridSetting();
			txtTotalInputs.Text = string.Empty;
			txtOverHead.Text = string.Empty;
			txtWagesAmount.Text = string.Empty;
			txtTotalOutPutAutoRatedAmount.Text = string.Empty;
			txtOutPutAutoRatePerQtyAmount.Text = string.Empty;
			EnableDisableProductionType();
			txtdocdate.Focus();
			FillWagesGridFilterCombo();
			EnableDisablePlant();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			dtFillsForGrids();
			uomgetAll();
			PlantFeader();
			FillWagesGridFilterCombo();
			ItemDetailFill(Conversion.ToInt(CmbProductionType.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmConversion_FormClosed(object sender, FormClosedEventArgs e)
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

	public void Calculation()
	{
		try
		{
			string sumoftotalInputs = ((grdInputMaterial.GetDataRows().Count() != 0) ? grdInputMaterial.GetTotal(grdInputMaterial.RootTable.Columns["Amount"], (AggregateFunction)2).ToString() : "");
			string sumofOverhead = ((grdOverHead.GetDataRows().Count() != 0) ? grdOverHead.GetTotal(grdOverHead.RootTable.Columns["Amount"], (AggregateFunction)2).ToString() : "");
			string sumofContractorWages = ((grdContractorWages.GetDataRows().Count() != 0) ? grdContractorWages.GetTotal(grdContractorWages.RootTable.Columns["Amount"], (AggregateFunction)2).ToString() : "");
			txtTotalInputs.Text = Math.Round(Conversion.ToDouble((object)sumoftotalInputs), 2).ToString(clsGlobalVariables.stringFormatsingle);
			txtOverHead.Text = Math.Round(Conversion.ToDouble((object)sumofOverhead), 2).ToString(clsGlobalVariables.stringFormatsingle);
			txtWagesAmount.Text = Math.Round(Conversion.ToDouble((object)sumofContractorWages), 2).ToString(clsGlobalVariables.stringFormatsingle);
			double NetValue = Conversion.ToDouble((object)sumoftotalInputs) + Conversion.ToDouble((object)sumofOverhead) + Conversion.ToDouble((object)sumofContractorWages);
			txtTotalOutPutAutoRatedAmount.Text = NetValue.ToString(clsGlobalVariables.stringFormatsingle);
			decimal OutPutAutoRateItemQty = (from GridEXRow row in grdOutPutByProduct.GetDataRows()
				select (decimal)row.Cells["VarientQty"].Value).Sum();
			txtOutPutAutoRatePerQtyAmount.Text = Math.Round(Conversion.ToDouble((object)txtTotalOutPutAutoRatedAmount.Text) / Conversion.ToDouble((object)OutPutAutoRateItemQty), 2).ToString(clsGlobalVariables.stringFormatsingle);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void IsDuplicateRowExists()
	{
		try
		{
			Dictionary<string, GridEXRow> uniqueKeys = new Dictionary<string, GridEXRow>();
			GridEXRow[] dataRows = grdOutPutByProduct.GetDataRows();
			foreach (GridEXRow item in dataRows)
			{
				int itemId = Conversion.ToInt(item.Cells["ItemId"].Value);
				int itemAttributeVarientId = Conversion.ToInt(item.Cells["ItemAttributeVarientId"].Value);
				int WarehouseId = Conversion.ToInt(item.Cells["WarehouseId"].Value);
				string key = $"{itemId}-{itemAttributeVarientId}-{WarehouseId}";
				if (uniqueKeys.TryGetValue(key, out var existingRow))
				{
					string ItemName = Conversion.ToString((object)item.Cells["ItemName"].Text);
					string AttributeVarient = Conversion.ToString((object)item.Cells["AttributeVarient"].Text);
					string WarehouseName = Conversion.ToString((object)item.Cells["WarehouseName"].Text);
					throw new Exception("Duplicate Record not added to the grid.\n" + $"Item {ItemName} with  Variant {AttributeVarient} and Warehouse {WarehouseName} exists in row No {item.RowIndex + 1} and Row No {existingRow.RowIndex + 1}");
				}
				uniqueKeys[key] = item;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public void CheckBomHeaderIds()
	{
		try
		{
			HashSet<int> outputIds = (from row in dtOutPutByProduct.AsEnumerable()
				where row.Field<int>("ItemId") > 0
				select row.Field<int>("ItemId")).ToHashSet();
			HashSet<int> WagesIds = (from row in dtContractorWages.AsEnumerable()
				where row.Field<int>("OutPutItemId") > 0
				select row.Field<int>("OutPutItemId")).ToHashSet();
			List<int> missingInOutput = WagesIds.Except(outputIds).ToList();
			List<int> missingInwages = outputIds.Except(WagesIds).ToList();
			if (missingInOutput.Any())
			{
				List<string> missingItemNames = (from row in dtInput.AsEnumerable()
					where missingInOutput.Contains(row.Field<int>("ItemId"))
					select row.Field<string>("ItemName")).ToList();
				throw new Exception("ItemName " + string.Join(", ", missingItemNames) + " found in Wages Grid but not in Output Grid.");
			}
			if (missingInwages.Any())
			{
				List<string> missingItemNames2 = (from row in dtOutPutByProduct.AsEnumerable()
					where missingInwages.Contains(row.Field<int>("ItemId"))
					select row.Field<string>("ItemName")).ToList();
				throw new Exception("ItemName " + string.Join(", ", missingItemNames2) + " found in Output Grid but not in Wages Grid.");
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Expected O, but got Unknown
		//IL_0aab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab2: Expected O, but got Unknown
		//IL_0f2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f34: Expected O, but got Unknown
		//IL_1193: Unknown result type (might be due to invalid IL or missing references)
		//IL_119a: Expected O, but got Unknown
		//IL_1494: Unknown result type (might be due to invalid IL or missing references)
		//IL_149b: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			if (grdInputMaterial.GetDataRows().Count() == 0 || grdOutPutByProduct.GetDataRows().Count() == 0)
			{
				throw new Exception("Grid Record Not Found...\nGrid (Input and OutPut) Should have atleast one record");
			}
			ConversionHeader ConversionHeader = new ConversionHeader();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				ConversionHeader.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			IsDuplicateRowExists();
			GridEXRow rrr = grdContractorWages.GetDataRows().FirstOrDefault();
			if (rrr != null && (int)rrr.RowType == 0 && Conversion.ToInt(rrr.Cells["OutPutItemId"].Value) > 0)
			{
				CheckBomHeaderIds();
			}
			btnGenerateRatesOutput_Click(null, null);
			decimal sumoftotalOutPut = Conversion.ToDecimal((object)grdOutPutByProduct.GetTotal(grdOutPutByProduct.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
			decimal TotalOutPutAutoRatedAmount = Conversion.ToDecimal((object)txtTotalOutPutAutoRatedAmount.Text);
			if (Math.Round(TotalOutPutAutoRatedAmount, 2) != Math.Round(sumoftotalOutPut, 2))
			{
				throw new Exception($"Net Vale {TotalOutPutAutoRatedAmount} In Header and Total OutPut Amount {sumoftotalOutPut} are not equal");
			}
			ConversionHeader.CompanyId = UserAccount.CompanyId;
			ConversionHeader.BranchesId = UserAccount.BranchesId;
			ConversionHeader.ProjectsId = UserAccount.BranchesId;
			ConversionHeader.OrganizationId = UserAccount.OrganizationId;
			ConversionHeader.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			ConversionHeader.DocumentTypeId = 1867;
			ConversionHeader.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
			ConversionHeader.DocNo = Conversion.ToInt((object)txtdocno.Text);
			ConversionHeader.ConversionNo = Conversion.ToString((object)txtProductionNo.Text);
			ConversionHeader.PlantId = Conversion.ToInt(CmbPlantName.Value);
			ConversionHeader.ConversionTypeId = Conversion.ToInt(CmbProductionType.Value);
			ConversionHeader.RemarksHeader = Conversion.ToString((object)txtRemarksHeader.Text);
			ConversionHeader.EntryUserId = UserAccount.ID;
			ConversionHeader.EntryDate = DateTime.Now;
			ConversionHeader.ModifyDate = DateTime.Now;
			ConversionHeader.ModifyUserId = UserAccount.ID;
			ConversionHeader.ApprovedUserId = UserAccount.ID;
			ConversionHeader.ApprovedDate = DateTime.Now;
			ConversionHeader.IsApproved = false;
			ConversionHeader.ConversionInputDetailList = new List<ConversionInputDetail>();
			ConversionHeader.ConversionOverHeadsDetailList = new List<ConversionOverHeadsDetail>();
			ConversionHeader.ConversionOutPutDetailList = new List<ConversionOutPutDetail>();
			ConversionHeader.ConversionWagesDetailList = new List<ConversionWagesDetail>();
			ConversionHeader.AttachmentsList = new List<DMSAttachments>();
			GridEXRow[] dataRows = grdInputMaterial.GetDataRows();
			foreach (GridEXRow r3 in dataRows)
			{
				if (Conversion.ToInt((object)r3.Cells["ItemId"].Value.ToString()) == 0)
				{
					continue;
				}
				ConversionInputDetail d3 = new ConversionInputDetail();
				d3.Id = Conversion.ToInt(r3.Cells["Id"].Value);
				if (d3.Id > 0)
				{
					d3.ActionTypeId = 2;
				}
				else
				{
					d3.ActionTypeId = 1;
				}
				if (r3.Cells["WarehouseId"].Text == string.Empty || Conversion.ToInt(r3.Cells["WarehouseId"].Value) == 0)
				{
					MessageBox.Show("Warehouse Field Required in Grid Input Row No: " + (r3.RowIndex + 1));
					return;
				}
				d3.WareHouseId = Conversion.ToInt(r3.Cells["WarehouseId"].Value);
				if (CheckWarehouseExists && !WareHouseAllocationToPlants.ValidateWareHouseAgainstPlantFromWareHouseAllocationToPlants(UserAccount.OrganizationId, UserAccount.CompanyId, d3.WareHouseId, Conversion.ToInt(CmbPlantName.Value)))
				{
					switch (MessageBox.Show(string.Format("Warehouse = {0} is Not Allocated to Selected Plant {1} In Input Grid row no : {2}\n", Conversion.ToString(r3.Cells["WarehouseName"].Value), Conversion.ToString((object)((Control)(object)CmbPlantName).Text), r3.RowIndex + 1) + "To Allow this Warehouse Select 'Yes'\nTo Not Allow this Select 'No'\nTo Cancel this Validation for Every Entry onwards In Input Grid Select 'Cancel'?", "Confirm", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
					{
					case DialogResult.No:
						CheckWarehouseExists = true;
						return;
					case DialogResult.Cancel:
						CheckWarehouseExists = false;
						break;
					}
				}
				if (r3.Cells["ItemId"].Text == string.Empty)
				{
					MessageBox.Show("Item Field Required in Grid Input Row No: " + (r3.RowIndex + 1));
					return;
				}
				d3.ItemId = Conversion.ToInt(r3.Cells["ItemId"].Value);
				if (r3.Cells["ItemUomId"].Text == string.Empty)
				{
					MessageBox.Show("UOM  Field Required in Grid Input Row No: " + (r3.RowIndex + 1));
					return;
				}
				DataTable dt = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r3.Cells["ItemId"].Value));
				if (dt.Rows.Count > 0)
				{
					DataRow[] dr = dt.Select("Id='" + Conversion.ToInt(r3.Cells["ItemUomId"].Value) + "'");
					if (dr.Length == 0)
					{
						MessageBox.Show("UOM in Input Grid Row No: " + (r3.RowIndex + 1) + " Is not Define Against Item " + Conversion.ToString((object)r3.Cells["ItemId"].Text) + ". Please Check...");
						return;
					}
					d3.ItemUomId = Conversion.ToInt(dr[0].ItemArray[0]);
				}
				if (r3.Cells["ItemQty"].Text == string.Empty)
				{
					MessageBox.Show("ItemQty Field Required in Grid Input Row No: " + (r3.RowIndex + 1));
					return;
				}
				d3.ItemQty = Conversion.ToDecimal((object)r3.Cells["ItemQty"].Text);
				if (r3.Cells["ItemAttributeVarientId"].Text == string.Empty)
				{
					MessageBox.Show("Varient Field Required in Grid Input Row No: " + (r3.RowIndex + 1));
					return;
				}
				d3.ItemAttributeVarientId = Conversion.ToInt((object)r3.Cells["ItemAttributeVarientId"].Text);
				if (r3.Cells["VarientEquivalent"].Text == string.Empty)
				{
					MessageBox.Show("Varient Equivalent Field Required in Grid Input Row No: " + (r3.RowIndex + 1));
					return;
				}
				d3.VarientEquivalent = Conversion.ToDecimal((object)r3.Cells["VarientEquivalent"].Text);
				d3.VarientQty = Conversion.ToDecimal((object)r3.Cells["VarientQty"].Text);
				if (r3.Cells["NetWeight"].Text == string.Empty)
				{
					MessageBox.Show("NetWeight Field Required in Grid Input Row No: " + (r3.RowIndex + 1));
					return;
				}
				d3.NetWeight = Conversion.ToDecimal(r3.Cells["NetWeight"].Value);
				if (r3.Cells["Rate"].Text == string.Empty)
				{
					MessageBox.Show("Rate Field Required in Grid Input Row No: " + (r3.RowIndex + 1));
					return;
				}
				d3.Rate = Conversion.ToDecimal(r3.Cells["Rate"].Value);
				if (r3.Cells["RateUomId"].Text == string.Empty)
				{
					MessageBox.Show("Rate UOM Field Required in Grid Input Row No: " + (r3.RowIndex + 1));
					return;
				}
				DataTable dt2 = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r3.Cells["ItemId"].Value));
				if (dt2.Rows.Count > 0)
				{
					DataRow[] dr2 = dt2.Select("Id='" + Conversion.ToInt(r3.Cells["RateUomId"].Value) + "'");
					if (dr2.Length == 0)
					{
						MessageBox.Show("RateUom in Input Grid Row No: " + (r3.RowIndex + 1) + " Is not Define Against Item " + Conversion.ToString((object)r3.Cells["ItemId"].Text) + ". Please Check...");
						return;
					}
					d3.RateUomId = Conversion.ToInt(dr2[0].ItemArray[0]);
				}
				if (r3.Cells["Amount"].Text == string.Empty)
				{
					MessageBox.Show("AmountField Required in Grid Input Row No: " + (r3.RowIndex + 1));
					return;
				}
				d3.Amount = Conversion.ToDecimal(r3.Cells["Amount"].Value);
				ConversionHeader.ConversionInputDetailList.Add(d3);
			}
			if (lstRemoveRecordInput.Count > 0)
			{
				for (int k = 0; k < lstRemoveRecordInput.Count; k++)
				{
					ConversionHeader.ConversionInputDetailList.Add(lstRemoveRecordInput[k]);
				}
			}
			dataRows = grdOutPutByProduct.GetDataRows();
			foreach (GridEXRow r4 in dataRows)
			{
				if (Conversion.ToInt((object)r4.Cells["ItemId"].Value.ToString()) != 0)
				{
					ConversionOutPutDetail d4 = new ConversionOutPutDetail();
					d4.Id = Conversion.ToInt(r4.Cells["Id"].Value);
					if (d4.Id > 0)
					{
						d4.ActionTypeId = 2;
					}
					else
					{
						d4.ActionTypeId = 1;
					}
					d4.EntryTypeId = 1;
					if (r4.Cells["WarehouseId"].Text == string.Empty || Conversion.ToInt(r4.Cells["WarehouseId"].Value) == 0)
					{
						MessageBox.Show("Warehouse Field Required in Grid OutPut Row No: " + (r4.RowIndex + 1));
						return;
					}
					d4.WareHouseId = Conversion.ToInt(r4.Cells["WarehouseId"].Value);
					if (r4.Cells["ItemId"].Text == string.Empty)
					{
						MessageBox.Show("Item Field Required in Grid OutPut Row No: " + (r4.RowIndex + 1));
						return;
					}
					d4.ItemId = Conversion.ToInt(r4.Cells["ItemId"].Value);
					if (r4.Cells["ItemAttributeVarientId"].Text == string.Empty)
					{
						MessageBox.Show("ItemAttributeVarient Field Required in Grid OutPut Row No: " + (r4.RowIndex + 1));
						return;
					}
					d4.ItemAttributeVarientId = Conversion.ToInt(r4.Cells["ItemAttributeVarientId"].Value);
					d4.VarientEquivalent = Conversion.ToDecimal(r4.Cells["VarientEquivalent"].Value);
					if (r4.Cells["ItemQty"].Text == string.Empty)
					{
						MessageBox.Show("ItemQty Field Required in Grid OutPut Row No: " + (r4.RowIndex + 1));
						return;
					}
					d4.ItemQty = Conversion.ToDecimal((object)r4.Cells["ItemQty"].Text);
					if (r4.Cells["VarientQty"].Text == string.Empty)
					{
						MessageBox.Show("VarientQty Field Required in Grid OutPut Row No: " + (r4.RowIndex + 1));
						return;
					}
					d4.VarientQty = Conversion.ToDecimal((object)r4.Cells["VarientQty"].Text);
					if (r4.Cells["Rate"].Text == string.Empty || Conversion.ToDecimal(r4.Cells["Rate"].Value) == 0m)
					{
						MessageBox.Show("Rate Field Required in Grid Output Row No: " + (r4.RowIndex + 1));
						return;
					}
					d4.Rate = Conversion.ToDecimal(r4.Cells["Rate"].Value);
					if (r4.Cells["Amount"].Text == string.Empty)
					{
						MessageBox.Show("AmountField Required in Grid Output Row No: " + (r4.RowIndex + 1));
						return;
					}
					d4.Amount = Conversion.ToDecimal(r4.Cells["Amount"].Value);
					d4.ItemUomId = 0;
					d4.NetWeight = d4.ItemQty;
					d4.RateUomId = 0;
					ConversionHeader.ConversionOutPutDetailList.Add(d4);
					continue;
				}
				MessageBox.Show("Item Field Required in Grid output Row No: " + (r4.RowIndex + 1));
				return;
			}
			if (lstRemoveRecordOutPut.Count > 0)
			{
				for (int j = 0; j < lstRemoveRecordOutPut.Count; j++)
				{
					ConversionHeader.ConversionOutPutDetailList.Add(lstRemoveRecordOutPut[j]);
				}
			}
			dataRows = grdOverHead.GetDataRows();
			foreach (GridEXRow r2 in dataRows)
			{
				if (Conversion.ToInt(r2.Cells["OverHeadAccountId"].Value) > 0)
				{
					ConversionOverHeadsDetail d2 = new ConversionOverHeadsDetail();
					if (r2.Cells["OverHeadAccountId"].Text == string.Empty || Conversion.ToInt(r2.Cells["OverHeadAccountId"].Value) == 0)
					{
						MessageBox.Show("OverHeadAccount Field Required in Grid Over Head Row No: " + (r2.RowIndex + 1));
						return;
					}
					d2.Id = Conversion.ToInt(r2.Cells["Id"].Value);
					d2.OverHeadTypeId = Conversion.ToInt(r2.Cells["OverHeadAccountId"].Value);
					d2.OverHeadAccountId = Conversion.ToInt(r2.Cells["OverHeadAccountId"].Value);
					if (r2.Cells["Amount"].Text == string.Empty || Conversion.ToDecimal(r2.Cells["Amount"].Value) == 0m)
					{
						MessageBox.Show("Amount Field Required in Grid Over Head Row No: " + (r2.RowIndex + 1));
						return;
					}
					d2.PercentOfTotalInput = Conversion.ToDecimal((object)r2.Cells["PercentOfTotalInput"].Text);
					d2.Qty = Conversion.ToDecimal((object)r2.Cells["Qty"].Text);
					d2.Rate = Conversion.ToDecimal((object)r2.Cells["Rate"].Text);
					d2.Amount = Conversion.ToDecimal((object)r2.Cells["Amount"].Text);
					d2.OhRemarks = Conversion.ToString((object)r2.Cells["OhRemarks"].Text);
					ConversionHeader.ConversionOverHeadsDetailList.Add(d2);
				}
			}
			dataRows = grdContractorWages.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				if (Conversion.ToInt(r.Cells["ContractorWagesRateScheduleId"].Value) > 0 || Conversion.ToInt(r.Cells["OutPutItemId"].Value) > 0)
				{
					ConversionWagesDetail d = new ConversionWagesDetail();
					if (r.Cells["OutPutItemId"].Text == string.Empty || Conversion.ToInt(r.Cells["OutPutItemId"].Value) == 0)
					{
						MessageBox.Show("OutPutItem Field Required in Grid Contractor Wages Row No: " + (r.RowIndex + 1));
						return;
					}
					d.Id = Conversion.ToInt(r.Cells["Id"].Value);
					d.ItemId = Conversion.ToInt(r.Cells["OutPutItemId"].Value);
					if (r.Cells["ContractorWagesRateScheduleId"].Text == string.Empty || Conversion.ToInt(r.Cells["ContractorWagesRateScheduleId"].Value) == 0)
					{
						MessageBox.Show("Service Activity Field Required in Grid Contractor Wages Row No: " + (r.RowIndex + 1));
						return;
					}
					d.ContractorWagesRateScheduleId = Conversion.ToInt(r.Cells["ContractorWagesRateScheduleId"].Value);
					if (r.Cells["ContractorId"].Text == string.Empty || Conversion.ToInt(r.Cells["ContractorId"].Value) == 0)
					{
						MessageBox.Show("Contractor  Field Required in Grid Contractor Wages Row No: " + (r.RowIndex + 1));
						return;
					}
					d.ContractorId = Conversion.ToInt(r.Cells["ContractorId"].Value);
					if (r.Cells["Amount"].Text == string.Empty || Conversion.ToDecimal(r.Cells["Amount"].Value) == 0m)
					{
						MessageBox.Show("Amount Field Required in Grid Contractor Wages Row No: " + (r.RowIndex + 1));
						return;
					}
					d.Qty = Conversion.ToDecimal((object)r.Cells["Qty"].Text);
					d.Rate = Conversion.ToDecimal((object)r.Cells["Rate"].Text);
					d.Amount = Conversion.ToDecimal((object)r.Cells["Amount"].Text);
					d.Remarks = Conversion.ToString((object)r.Cells["Remarks"].Text);
					d.OutPutItemId = Conversion.ToInt(r.Cells["OutPutItemId"].Value);
					ConversionHeader.ConversionWagesDetailList.Add(d);
				}
			}
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Path.GetFileName(Attachment.dtAttachments.Rows[i][1].ToString());
				attach.ScreenName = base.Name;
				ConversionHeader.AttachmentsList.Add(attach);
				if (!Directory.Exists(clsGlobalVariables.AttachmentFolderPath))
				{
					throw new Exception("Attachment Folder Path Not Configure Properly");
				}
				string fileSavePath = Path.Combine(clsGlobalVariables.AttachmentFolderPath, attach.Attachment);
				if (!File.Exists(fileSavePath))
				{
					File.Copy(Attachment.dtAttachments.Rows[i][1].ToString(), fileSavePath, overwrite: true);
				}
			}
			int success = ConversionHeader.Save(ConversionHeader);
			ConversionHeader.Id = success;
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + ConversionHeader.DocNo);
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0)
				{
					for (int l = 0; l < AT.RemovedAttachmentListInUpdateCase.Count; l++)
					{
						using (FileStream fs = new FileStream(AT.RemovedAttachmentListInUpdateCase[l].Path, FileMode.Open, FileAccess.ReadWrite))
						{
							fs.Flush();
							fs.Close();
							fs.Dispose();
						}
						bool fileDeleted = false;
						int attempts = 0;
						while (!fileDeleted && attempts < 5)
						{
							try
							{
								File.Delete(AT.RemovedAttachmentListInUpdateCase[l].Path);
								fileDeleted = true;
							}
							catch (IOException)
							{
								attempts++;
								Thread.Sleep(500);
							}
						}
					}
				}
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + ConversionHeader.DocNo);
			}
			FromReset();
			if (ChkPreviewSlip.Checked && formright.DoHavePrintRights)
			{
				CommonServices.ConversionSlipConcrete(ConversionHeader.Id);
			}
			if (chkPrintVoucher.Checked && formright.DoHavePrintRights)
			{
				VoucherReprot(ConversionHeader.Id);
			}
		}
		catch (Exception ex2)
		{
			throw new Exception(ex2.Message);
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
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		try
		{
			FromReset();
			dtInput.Clear();
			grdInputMaterial.ClearStructure();
			dtOutPutByProduct.Clear();
			grdOutPutByProduct.ClearStructure();
			dtOverHead.Clear();
			grdOverHead.ClearStructure();
			dtContractorWages.Clear();
			grdContractorWages.ClearStructure();
			ConversionHeader ConversionHeader = new ConversionHeader();
			RecId = Id;
			ConversionHeader = ConversionHeader.ReadById(RecId);
			if (ConversionHeader == null)
			{
				return;
			}
			UpdateMode = true;
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
			tabControl1.SelectedIndex = 0;
			txtdocdate.Value = ConversionHeader.DocDate;
			txtdocno.Text = Conversion.ToString((object)ConversionHeader.DocNo);
			CmbProductionType.Value = ConversionHeader.ConversionTypeId;
			CmbProductionType_Leave(null, null);
			CmbPlantName.Value = ConversionHeader.PlantId;
			CmbPlantName_Leave(null, null);
			txtProductionNo.Text = ConversionHeader.ConversionNo;
			txtRemarksHeader.Text = Conversion.ToString((object)ConversionHeader.RemarksHeader);
			if (ConversionHeader.ConversionOutPutDetailList.Count > 0)
			{
				for (int l = 0; l < ConversionHeader.ConversionOutPutDetailList.Count; l++)
				{
					dtOutPutByProduct.Rows.Add(ConversionHeader.ConversionOutPutDetailList[l].Id, ConversionHeader.ConversionOutPutDetailList[l].WareHouseId, ConversionHeader.ConversionOutPutDetailList[l].WareHouseName, ConversionHeader.ConversionOutPutDetailList[l].ItemId, ConversionHeader.ConversionOutPutDetailList[l].ItemName, ConversionHeader.ConversionOutPutDetailList[l].ItemAttributeVarientId, ConversionHeader.ConversionOutPutDetailList[l].ItemAttribute, ConversionHeader.ConversionOutPutDetailList[l].VarientEquivalent, ConversionHeader.ConversionOutPutDetailList[l].ItemUomId, ConversionHeader.ConversionOutPutDetailList[l].ItemUomEquivalent, ConversionHeader.ConversionOutPutDetailList[l].ItemUomCode, ConversionHeader.ConversionOutPutDetailList[l].ItemQty, ConversionHeader.ConversionOutPutDetailList[l].VarientQty, ConversionHeader.ConversionOutPutDetailList[l].NetWeight, ConversionHeader.ConversionOutPutDetailList[l].Rate, ConversionHeader.ConversionOutPutDetailList[l].RateUomId, ConversionHeader.ConversionOutPutDetailList[l].RateUomEquivalent, ConversionHeader.ConversionOutPutDetailList[l].RateUomCode, ConversionHeader.ConversionOutPutDetailList[l].Amount);
				}
			}
			else
			{
				AddRowInOutPutByProductGrid();
			}
			if (ConversionHeader.ConversionInputDetailList.Count > 0)
			{
				for (int k = 0; k < ConversionHeader.ConversionInputDetailList.Count; k++)
				{
					dtInput.Rows.Add(ConversionHeader.ConversionInputDetailList[k].Id, ConversionHeader.ConversionInputDetailList[k].WareHouseId, ConversionHeader.ConversionInputDetailList[k].WareHouseName, ConversionHeader.ConversionInputDetailList[k].ItemId, ConversionHeader.ConversionInputDetailList[k].ItemName, ConversionHeader.ConversionInputDetailList[k].ItemUomId, ConversionHeader.ConversionInputDetailList[k].ItemUomEquivalent, ConversionHeader.ConversionInputDetailList[k].ItemUomCode, ConversionHeader.ConversionInputDetailList[k].ItemQty, ConversionHeader.ConversionInputDetailList[k].ItemQty, ConversionHeader.ConversionInputDetailList[k].ItemAttributeVarientId, ConversionHeader.ConversionInputDetailList[k].ItemAttribute, ConversionHeader.ConversionInputDetailList[k].VarientEquivalent, ConversionHeader.ConversionInputDetailList[k].VarientQty, ConversionHeader.ConversionInputDetailList[k].NetWeight, ConversionHeader.ConversionInputDetailList[k].Rate, ConversionHeader.ConversionInputDetailList[k].RateUomId, ConversionHeader.ConversionInputDetailList[k].RateUomEquivalent, ConversionHeader.ConversionInputDetailList[k].RateUomCode, ConversionHeader.ConversionInputDetailList[k].Amount);
				}
			}
			else
			{
				grdInputMaterial.ClearStructure();
				AddRowInInputGrid();
			}
			if (ConversionHeader.ConversionOverHeadsDetailList.Count > 0)
			{
				for (int j = 0; j < ConversionHeader.ConversionOverHeadsDetailList.Count; j++)
				{
					dtOverHead.Rows.Add(ConversionHeader.ConversionOverHeadsDetailList[j].Id, ConversionHeader.ConversionOverHeadsDetailList[j].OverHeadTypeId, ConversionHeader.ConversionOverHeadsDetailList[j].OverHeadAccountId, ConversionHeader.ConversionOverHeadsDetailList[j].PercentOfTotalInput, ConversionHeader.ConversionOverHeadsDetailList[j].Qty, ConversionHeader.ConversionOverHeadsDetailList[j].Rate, ConversionHeader.ConversionOverHeadsDetailList[j].Amount, ConversionHeader.ConversionOverHeadsDetailList[j].OhRemarks);
				}
			}
			else
			{
				AddRowInOverHeadGrid();
			}
			if (ConversionHeader.ConversionWagesDetailList.Count > 0)
			{
				for (int i = 0; i < ConversionHeader.ConversionWagesDetailList.Count; i++)
				{
					dtContractorWages.Rows.Add(ConversionHeader.ConversionWagesDetailList[i].Id, ConversionHeader.ConversionWagesDetailList[i].OutPutItemId, ConversionHeader.ConversionWagesDetailList[i].OutPutItemName, ConversionHeader.ConversionWagesDetailList[i].ContractorId, ConversionHeader.ConversionWagesDetailList[i].ContractorName, ConversionHeader.ConversionWagesDetailList[i].ContractorWagesRateScheduleId, ConversionHeader.ConversionWagesDetailList[i].ServiceActivity, ConversionHeader.ConversionWagesDetailList[i].Qty, ConversionHeader.ConversionWagesDetailList[i].Rate, ConversionHeader.ConversionWagesDetailList[i].Amount, ConversionHeader.ConversionWagesDetailList[i].Remarks);
				}
			}
			else
			{
				AddRowInContractorWagesGrid();
			}
			grdInputMaterial.DataSource = dtInput;
			grdInputMaterial.RetrieveStructure();
			grdInputSettings();
			grdOutPutByProduct.DataSource = dtOutPutByProduct;
			grdOutPutByProduct.RetrieveStructure();
			grdOutPutGridSetting();
			grdOverHead.DataSource = dtOverHead;
			grdOverHead.RetrieveStructure();
			grdOverHeadGridSetting();
			grdContractorWages.DataSource = dtContractorWages;
			grdContractorWages.RetrieveStructure();
			grdContractorWagesGridSetting();
			List<DMSAttachments> byID = DMSAttachments.GetByID(RecId, base.Name);
			AT = new Attachment();
			foreach (DMSAttachments Attachment in byID)
			{
				TempAttachments TA = new TempAttachments();
				TA.Id = 0;
				TA.Path = Attachment.Attachment;
				AT.lst.Add(TA);
			}
			EnableDisableProductionType();
			Calculation();
			FillWagesGridFilterCombo();
			EnableDisablePlant();
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
					ConversionHeader.DeleteByID(UserAccount.ID, RecId);
					MessageBox.Show("Delete Record Successfully");
					FromReset();
				}
				return;
			}
			throw new Exception("No record found to Delete");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtQtyBOM_KeyPress(object sender, KeyPressEventArgs e)
	{
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.ConversionSlipConcrete(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnVoucherReport_Click(object sender, EventArgs e)
	{
		try
		{
			VoucherReprot(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VoucherReprot(int Id)
	{
		try
		{
			CommonServices.VoucherReport_118(Conversion.ToInt((object)CommonServices.VoucherHeadIdGet(Id, 1867)), 1867);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AllCombobind()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		try
		{
			int Idtype = Conversion.ToInt(cmbConversionNoHistory.Value);
			DataTable dtcombo = ConversionHeader.GetDataForDropDownFromConversionHeader(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Activity = "ConversionNo"
			});
			if (dtcombo.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtProductionNo = new DataTable();
			dtProductionNo.Columns.Add("Id", typeof(int));
			dtProductionNo.Columns.Add("ConversionNo");
			for (int i = 0; i < dtcombo.Rows.Count; i++)
			{
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "ConversionNo")
				{
					dtProductionNo.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
			}
			if (dtProductionNo.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtProductionNo, cmbConversionNoHistory, "Id", "ConversionNo", "Conversion No", ZeroIndex: true);
			((UltraGridBase)cmbConversionNoHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (Idtype > 0)
			{
				if (dtProductionNo.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Idtype))
				{
					cmbConversionNoHistory.Value = Idtype;
				}
				else
				{
					((Control)(object)cmbConversionNoHistory).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void Datetypefill()
	{
		try
		{
			DataTable dt = CommonServices.DateType();
			DDL.BindDDL(dt, cmbDateTypeHistory, "Id", "Parameters", "Parameters", ZeroIndex: true);
			((UltraGridBase)cmbDateTypeHistory).Rows[2].Activate();
			((UltraGridBase)cmbDateTypeHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void cmbDateTypeHistory_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbDateTypeHistory.Value) == 1)
			{
				txtFromdateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 2)
			{
				txtFromdateHistory.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 3)
			{
				DateTime now = DateTime.Now;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				txtFromdateHistory.Value = first;
				txtToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				txtFromdateHistory.Value = first2;
				txtToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 5)
			{
				txtFromdateHistory.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
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
			Datetypefill();
			AllCombobind();
		}
	}

	private void btnHistoryRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)cmbDateTypeHistory).Text = string.Empty;
			((Control)(object)cmbConversionNoHistory).Text = string.Empty;
			grdHistoryOverHead.ClearStructure();
			grdHistoryOutput.ClearStructure();
			grdHistoryInput.ClearStructure();
			cmbDateTypeHistory.Focus();
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
			WF.Show();
			HistoryGridFill();
		}
		catch (Exception ex)
		{
			WF.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			WF.Close();
		}
	}

	private void HistoryGridFill()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			dtHistory.Clear();
			ReportsParameters header = new ReportsParameters();
			header.CompanyId = UserAccount.CompanyId;
			header.OrganizationId = UserAccount.OrganizationId;
			header.BranchesId = UserAccount.BranchesId;
			header.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			header.DocumentTypeId = 1867;
			header.FromDocNo = Conversion.ToInt((object)txtFromNoHistory.Text.Trim());
			header.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text.Trim());
			if (txtFromdateHistory.Checked)
			{
				header.FromDate = Conversion.ToDateTime((object)txtFromdateHistory.Value);
			}
			if (txtToDateHistory.Checked)
			{
				header.ToDate = Conversion.ToDateTime((object)txtToDateHistory.Value);
			}
			header.Id = Conversion.ToInt(cmbConversionNoHistory.Value);
			header.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (RadReffered.Checked)
			{
				header.ActionId = 1;
			}
			else if (RadNotReffered.Checked)
			{
				header.ActionId = 2;
			}
			else if (RadAll.Checked)
			{
				header.ActionId = 0;
			}
			dtHistory = ConversionHeader.FormHistory(header);
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("RecordNo", typeof(int));
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("DocumentTypeId", typeof(int));
				dtcol.Columns.Add("DocNo", typeof(string));
				dtcol.Columns.Add("DocDate", typeof(DateTime));
				dtcol.Columns.Add("ConversionNo", typeof(string));
				dtcol.Columns.Add("PlantName", typeof(string));
				dtcol.Columns.Add("Remarks", typeof(string));
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("EntryUser", typeof(string));
				dtcol.Columns.Add("ModifyDate", typeof(DateTime));
				dtcol.Columns.Add("ModifyUser", typeof(string));
				dtcol.Columns.Add("IsApproved", typeof(bool));
				dtcol.Columns.Add("ApprovedDate", typeof(DateTime));
				dtcol.Columns.Add("NoOfAttachments", typeof(int));
				dtcol.Rows.Clear();
				foreach (DataRow row in dtHistory.Rows)
				{
					dtcol.Rows.Add(row["RecordNo"], row["Id"], row["DocumentTypeId"], row["DocNo"], row["DocDate"], row["ConversionNo"], row["PlantName"], row["RemarksHeader"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"], row["IsApproved"], row["ApprovedDate"], row["NoOfAttachments"]);
				}
				DatagridHistory.DataSource = dtcol;
				DatagridHistory.RetrieveStructure();
				HistoryGridSetting();
			}
			else
			{
				DatagridHistory.DataSource = null;
				grdHistoryInput.DataSource = null;
				grdHistoryOutput.DataSource = null;
				grdHistoryOverHead.DataSource = null;
				grdWagesHistory.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void HistoryGridSetting()
	{
		try
		{
			DatagridHistory.RootTable.Columns["Id"].Visible = false;
			DatagridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DatagridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DatagridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DatagridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DatagridHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DatagridHistory.RootTable.HeaderLines = 2;
			DatagridHistory.RootTable.Columns["RecordNo"].Width = Constants.InventoryConstants.RecordNo;
			DatagridHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DoNoConstant;
			DatagridHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			DatagridHistory.RootTable.Columns["ConversionNo"].Width = Constants.InventoryConstants.ProductionNo;
			DatagridHistory.RootTable.Columns["PlantName"].Width = Constants.InventoryConstants.PlantName;
			DatagridHistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			DatagridHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstant;
			DatagridHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			DatagridHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstant;
			DatagridHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants;
			DatagridHistory.RootTable.Columns["IsApproved"].Width = Constants.InventoryConstants.IsApproved;
			DatagridHistory.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstant;
			DatagridHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			DatagridHistory.RootTable.Columns["RecordNo"].HeaderAlignment = (TextAlignment)2;
			DatagridHistory.RootTable.Columns["DocNo"].HeaderAlignment = (TextAlignment)2;
			DatagridHistory.RootTable.Columns["DocDate"].HeaderAlignment = (TextAlignment)2;
			DatagridHistory.RootTable.Columns["ConversionNo"].HeaderAlignment = (TextAlignment)2;
			DatagridHistory.RootTable.Columns["PlantName"].HeaderAlignment = (TextAlignment)2;
			DatagridHistory.RootTable.Columns["Remarks"].HeaderAlignment = (TextAlignment)2;
			DatagridHistory.RootTable.Columns["EntryDate"].HeaderAlignment = (TextAlignment)2;
			DatagridHistory.RootTable.Columns["EntryUser"].HeaderAlignment = (TextAlignment)2;
			DatagridHistory.RootTable.Columns["ModifyDate"].HeaderAlignment = (TextAlignment)2;
			DatagridHistory.RootTable.Columns["ModifyUser"].HeaderAlignment = (TextAlignment)2;
			DatagridHistory.RootTable.Columns["IsApproved"].HeaderAlignment = (TextAlignment)2;
			DatagridHistory.RootTable.Columns["ApprovedDate"].HeaderAlignment = (TextAlignment)2;
			DatagridHistory.RootTable.Columns["NoOfAttachments"].HeaderAlignment = (TextAlignment)2;
			DatagridHistory.RootTable.Columns.Add("Print");
			DatagridHistory.RootTable.Columns["Print"].Key = "Print";
			DatagridHistory.RootTable.Columns["Print"].Caption = "Print";
			DatagridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DatagridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DatagridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DatagridHistory.RootTable.Columns["Print"].Width = 50;
			DatagridHistory.RootTable.Columns["Print"].Position = 0;
			DatagridHistory.RootTable.Columns.Add("Voucher");
			DatagridHistory.RootTable.Columns["Voucher"].Key = "Voucher";
			DatagridHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
			DatagridHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DatagridHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			DatagridHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			DatagridHistory.RootTable.Columns["Voucher"].Width = 70;
			DatagridHistory.RootTable.Columns["Voucher"].Position = 1;
			DatagridHistory.RootTable.Columns.Add("Edit");
			DatagridHistory.RootTable.Columns["Edit"].Key = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DatagridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DatagridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].Width = 50;
			DatagridHistory.RootTable.Columns["Edit"].Position = 2;
			DatagridHistory.FrozenColumns = 3;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DatagridHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(DatagridHistory.CurrentRow.Cells["Id"].Value);
				if (!formright.DoHaveUpdateRights)
				{
					WF.Close();
					throw new Exception("you don't have update rights...");
				}
				WF.Show();
				ReadById(CurrId);
			}
		}
		catch (Exception ex)
		{
			WF.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			WF.Close();
		}
	}

	private void DatagridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			int CurrId = Conversion.ToInt(DatagridHistory.CurrentRow.Cells["Id"].Value);
			if (e.Column.Key == "Print")
			{
				CommonServices.ConversionSlipConcrete(CurrId);
			}
			if (e.Column.Key == "Voucher")
			{
				VoucherReprot(CurrId);
			}
			if (e.Column.Key == "Edit")
			{
				if (!formright.DoHaveUpdateRights)
				{
					WF.Close();
					throw new Exception("ypu don't have updae rights...");
				}
				WF.Show();
				ReadById(Conversion.ToInt((object)DatagridHistory.CurrentRow.Cells["Id"].Value.ToString()));
			}
		}
		catch (Exception ex)
		{
			WF.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			WF.Close();
		}
	}

	private void DatagridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DatagridHistory.CurrentRow.Cells["Id"].Value), 1867);
			}
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
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				DatagridHistoryDetailbind(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistoryDetailbind(int Id)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		try
		{
			grdHistoryInput.ClearStructure();
			grdHistoryOutput.ClearStructure();
			grdHistoryOverHead.ClearStructure();
			grdWagesHistory.ClearStructure();
			ConversionHeader ConversionHeader = new ConversionHeader();
			ConversionHeader = ConversionHeader.ReadById(Id);
			if (ConversionHeader == null)
			{
				return;
			}
			DataTable dtInputHistory = new DataTable();
			DataTable dtOutPutByProductHistory = new DataTable();
			DataTable dtOverHeadHistory = new DataTable();
			DataTable dtWagesHistory = new DataTable();
			new DataTable();
			dtInputHistory.Columns.Add("Id", typeof(int));
			dtInputHistory.Columns.Add("WarehouseName", typeof(string));
			dtInputHistory.Columns.Add("ItemId", typeof(int));
			dtInputHistory.Columns.Add("ItemName", typeof(string));
			dtInputHistory.Columns.Add("Uom", typeof(string));
			dtInputHistory.Columns.Add("AttributeVariant", typeof(string));
			dtInputHistory.Columns.Add("VariantUnit", typeof(decimal));
			dtInputHistory.Columns.Add("ItemQty", typeof(decimal));
			dtInputHistory.Columns.Add("VariantQty", typeof(decimal));
			dtInputHistory.Columns.Add("Rate", typeof(decimal));
			dtInputHistory.Columns.Add("Amount", typeof(decimal));
			dtOutPutByProductHistory.Columns.Add("Id", typeof(int));
			dtOutPutByProductHistory.Columns.Add("WarehouseName", typeof(string));
			dtOutPutByProductHistory.Columns.Add("ItemId", typeof(int));
			dtOutPutByProductHistory.Columns.Add("ItemName", typeof(string));
			dtOutPutByProductHistory.Columns.Add("AttributeVariant", typeof(string));
			dtOutPutByProductHistory.Columns.Add("VariantUnit", typeof(decimal));
			dtOutPutByProductHistory.Columns.Add("Uom", typeof(string));
			dtOutPutByProductHistory.Columns.Add("ItemQty", typeof(decimal));
			dtOutPutByProductHistory.Columns.Add("VariantQty", typeof(decimal));
			dtOutPutByProductHistory.Columns.Add("Rate", typeof(decimal));
			dtOutPutByProductHistory.Columns.Add("Amount", typeof(decimal));
			dtOverHeadHistory.Columns.Add("Id", typeof(int));
			dtOverHeadHistory.Columns.Add("OverHeadType", typeof(string));
			dtOverHeadHistory.Columns.Add("OverHeadAccount", typeof(string));
			dtOverHeadHistory.Columns.Add("Percentage", typeof(decimal));
			dtOverHeadHistory.Columns.Add("Qty", typeof(decimal));
			dtOverHeadHistory.Columns.Add("Rate", typeof(decimal));
			dtOverHeadHistory.Columns.Add("Amount", typeof(decimal));
			dtOverHeadHistory.Columns.Add("OhRemarks", typeof(string));
			dtWagesHistory.Columns.Add("Id", typeof(int));
			dtWagesHistory.Columns.Add("ContractorName");
			dtWagesHistory.Columns.Add("ItemName");
			dtWagesHistory.Columns.Add("ServiceActivity");
			dtWagesHistory.Columns.Add("Qty", typeof(decimal));
			dtWagesHistory.Columns.Add("Rate", typeof(decimal));
			dtWagesHistory.Columns.Add("Amount", typeof(decimal));
			dtWagesHistory.Columns.Add("Remarks");
			if (ConversionHeader.ConversionInputDetailList.Count > 0)
			{
				for (int l = 0; l < ConversionHeader.ConversionInputDetailList.Count; l++)
				{
					dtInputHistory.Rows.Add(ConversionHeader.ConversionInputDetailList[l].Id, ConversionHeader.ConversionInputDetailList[l].WareHouseName, ConversionHeader.ConversionInputDetailList[l].ItemId, ConversionHeader.ConversionInputDetailList[l].ItemName, ConversionHeader.ConversionInputDetailList[l].ItemUomCode, ConversionHeader.ConversionInputDetailList[l].ItemAttribute, ConversionHeader.ConversionInputDetailList[l].VarientEquivalent, ConversionHeader.ConversionInputDetailList[l].ItemQty, ConversionHeader.ConversionInputDetailList[l].VarientQty, ConversionHeader.ConversionInputDetailList[l].Rate, ConversionHeader.ConversionInputDetailList[l].Amount);
				}
			}
			else
			{
				grdHistoryInput.ClearStructure();
			}
			if (ConversionHeader.ConversionOutPutDetailList.Count > 0)
			{
				for (int k = 0; k < ConversionHeader.ConversionOutPutDetailList.Count; k++)
				{
					dtOutPutByProductHistory.Rows.Add(ConversionHeader.ConversionOutPutDetailList[k].Id, ConversionHeader.ConversionOutPutDetailList[k].WareHouseName, ConversionHeader.ConversionOutPutDetailList[k].ItemId, ConversionHeader.ConversionOutPutDetailList[k].ItemName, ConversionHeader.ConversionOutPutDetailList[k].ItemAttribute, ConversionHeader.ConversionOutPutDetailList[k].VarientEquivalent, ConversionHeader.ConversionOutPutDetailList[k].ItemUomCode, ConversionHeader.ConversionOutPutDetailList[k].ItemQty, ConversionHeader.ConversionOutPutDetailList[k].VarientQty, ConversionHeader.ConversionOutPutDetailList[k].Rate, ConversionHeader.ConversionOutPutDetailList[k].Amount);
				}
			}
			else
			{
				grdHistoryOutput.ClearStructure();
			}
			if (ConversionHeader.ConversionOverHeadsDetailList.Count > 0)
			{
				for (int j = 0; j < ConversionHeader.ConversionOverHeadsDetailList.Count; j++)
				{
					dtOverHeadHistory.Rows.Add(ConversionHeader.ConversionOverHeadsDetailList[j].Id, ConversionHeader.ConversionOverHeadsDetailList[j].OverHeadTypeId, ConversionHeader.ConversionOverHeadsDetailList[j].OverHeadAccountTitle, ConversionHeader.ConversionOverHeadsDetailList[j].PercentOfTotalInput, ConversionHeader.ConversionOverHeadsDetailList[j].Qty, ConversionHeader.ConversionOverHeadsDetailList[j].Rate, ConversionHeader.ConversionOverHeadsDetailList[j].Amount, ConversionHeader.ConversionOverHeadsDetailList[j].OhRemarks);
				}
			}
			else
			{
				grdHistoryOverHead.ClearStructure();
			}
			if (ConversionHeader.ConversionWagesDetailList.Count > 0)
			{
				for (int i = 0; i < ConversionHeader.ConversionWagesDetailList.Count; i++)
				{
					dtWagesHistory.Rows.Add(ConversionHeader.ConversionWagesDetailList[i].Id, ConversionHeader.ConversionWagesDetailList[i].ContractorName, ConversionHeader.ConversionWagesDetailList[i].ItemName, ConversionHeader.ConversionWagesDetailList[i].ServiceActivity, ConversionHeader.ConversionWagesDetailList[i].Qty, ConversionHeader.ConversionWagesDetailList[i].Rate, ConversionHeader.ConversionWagesDetailList[i].Amount, ConversionHeader.ConversionWagesDetailList[i].Remarks);
				}
			}
			else
			{
				grdWagesHistory.ClearStructure();
			}
			grdHistoryInput.DataSource = dtInputHistory;
			grdHistoryInput.RetrieveStructure();
			grdHistoryInputSetting();
			grdHistoryOutput.DataSource = dtOutPutByProductHistory;
			grdHistoryOutput.RetrieveStructure();
			grdHistoryOutputSetting();
			grdHistoryOverHead.DataSource = dtOverHeadHistory;
			grdHistoryOverHead.RetrieveStructure();
			grdHistoryOverHeadSetting();
			grdWagesHistory.DataSource = dtWagesHistory;
			grdWagesHistory.RetrieveStructure();
			grdWagesHistorySetting();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdHistoryInputSetting()
	{
		try
		{
			grdHistoryInput.RootTable.Columns["Id"].Visible = false;
			grdHistoryInput.RootTable.Columns["ItemId"].Visible = false;
			grdHistoryInput.RootTable.HeaderLines = 2;
			grdHistoryInput.RootTable.Columns["WarehouseName"].HeaderAlignment = (TextAlignment)2;
			grdHistoryInput.RootTable.Columns["ItemName"].HeaderAlignment = (TextAlignment)2;
			grdHistoryInput.RootTable.Columns["Uom"].HeaderAlignment = (TextAlignment)2;
			grdHistoryInput.RootTable.Columns["AttributeVariant"].HeaderAlignment = (TextAlignment)2;
			grdHistoryInput.RootTable.Columns["VariantUnit"].HeaderAlignment = (TextAlignment)2;
			grdHistoryInput.RootTable.Columns["ItemQty"].HeaderAlignment = (TextAlignment)2;
			grdHistoryInput.RootTable.Columns["VariantQty"].HeaderAlignment = (TextAlignment)2;
			grdHistoryInput.RootTable.Columns["Rate"].HeaderAlignment = (TextAlignment)2;
			grdHistoryInput.RootTable.Columns["Amount"].HeaderAlignment = (TextAlignment)2;
			grdHistoryInput.RootTable.Columns["WarehouseName"].Width = Constants.InventoryConstants.WareHouseName;
			grdHistoryInput.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdHistoryInput.RootTable.Columns["Uom"].Width = Constants.InventoryConstants.UomConstant;
			grdHistoryInput.RootTable.Columns["AttributeVariant"].Width = Constants.InventoryConstants.Variant;
			grdHistoryInput.RootTable.Columns["VariantUnit"].Width = Constants.InventoryConstants.Variant;
			grdHistoryInput.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdHistoryInput.RootTable.Columns["VariantQty"].Width = Constants.InventoryConstants.Variant;
			grdHistoryInput.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdHistoryInput.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdHistoryInput.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryInput.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdHistoryInput.RootTable.Columns["ItemQty"].FormatString = "##,#.####";
			grdHistoryInput.RootTable.Columns["ItemQty"].TotalFormatString = "##,#.####";
			grdHistoryInput.RootTable.Columns["VariantUnit"].AggregateFunction = (AggregateFunction)2;
			grdHistoryInput.RootTable.Columns["VariantUnit"].TextAlignment = (TextAlignment)3;
			grdHistoryInput.RootTable.Columns["VariantUnit"].FormatString = "#,#.####";
			grdHistoryInput.RootTable.Columns["VariantUnit"].TotalFormatString = "#,#.####";
			grdHistoryInput.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)3;
			grdHistoryInput.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdHistoryInput.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryInput.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryInput.RootTable.Columns["VariantQty"].TextAlignment = (TextAlignment)3;
			grdHistoryInput.RootTable.Columns["VariantQty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryInput.RootTable.Columns["VariantQty"].FormatString = "##,#.####";
			grdHistoryInput.RootTable.Columns["VariantQty"].TotalFormatString = "##,#.####";
			grdHistoryInput.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdHistoryInput.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdHistoryInput.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistoryInput.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdHistoryOutputSetting()
	{
		try
		{
			grdHistoryOutput.RootTable.Columns["Id"].Visible = false;
			grdHistoryOutput.RootTable.Columns["ItemId"].Visible = false;
			grdHistoryOutput.RootTable.Columns["Uom"].Visible = false;
			grdHistoryOutput.RootTable.HeaderLines = 2;
			grdHistoryOutput.RootTable.Columns["WarehouseName"].HeaderAlignment = (TextAlignment)2;
			grdHistoryOutput.RootTable.Columns["ItemName"].HeaderAlignment = (TextAlignment)2;
			grdHistoryOutput.RootTable.Columns["Uom"].HeaderAlignment = (TextAlignment)2;
			grdHistoryOutput.RootTable.Columns["AttributeVariant"].HeaderAlignment = (TextAlignment)2;
			grdHistoryOutput.RootTable.Columns["VariantUnit"].HeaderAlignment = (TextAlignment)2;
			grdHistoryOutput.RootTable.Columns["ItemQty"].HeaderAlignment = (TextAlignment)2;
			grdHistoryOutput.RootTable.Columns["VariantQty"].HeaderAlignment = (TextAlignment)2;
			grdHistoryOutput.RootTable.Columns["Rate"].HeaderAlignment = (TextAlignment)2;
			grdHistoryOutput.RootTable.Columns["Amount"].HeaderAlignment = (TextAlignment)2;
			grdHistoryOutput.RootTable.Columns["WarehouseName"].Width = Constants.InventoryConstants.WareHouseName;
			grdHistoryOutput.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdHistoryOutput.RootTable.Columns["Uom"].Width = Constants.InventoryConstants.UomConstant;
			grdHistoryOutput.RootTable.Columns["AttributeVariant"].Width = Constants.InventoryConstants.Variant;
			grdHistoryOutput.RootTable.Columns["VariantUnit"].Width = Constants.InventoryConstants.Variant;
			grdHistoryOutput.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdHistoryOutput.RootTable.Columns["VariantQty"].Width = Constants.InventoryConstants.Variant;
			grdHistoryOutput.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdHistoryOutput.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdHistoryOutput.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOutput.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdHistoryOutput.RootTable.Columns["ItemQty"].FormatString = "##,#.####";
			grdHistoryOutput.RootTable.Columns["ItemQty"].TotalFormatString = "##,#.####";
			grdHistoryOutput.RootTable.Columns["VariantUnit"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOutput.RootTable.Columns["VariantUnit"].TextAlignment = (TextAlignment)3;
			grdHistoryOutput.RootTable.Columns["VariantUnit"].FormatString = "#,#.####";
			grdHistoryOutput.RootTable.Columns["VariantUnit"].TotalFormatString = "#,#.####";
			grdHistoryOutput.RootTable.Columns["VariantQty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOutput.RootTable.Columns["VariantQty"].TextAlignment = (TextAlignment)3;
			grdHistoryOutput.RootTable.Columns["VariantQty"].FormatString = "##,#.####";
			grdHistoryOutput.RootTable.Columns["VariantQty"].TotalFormatString = "##,#.####";
			grdHistoryOutput.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOutput.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdHistoryOutput.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryOutput.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryOutput.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdHistoryOutput.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOutput.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistoryOutput.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdHistoryOverHeadSetting()
	{
		try
		{
			grdHistoryOverHead.RootTable.Columns["Id"].Visible = false;
			grdHistoryOverHead.RootTable.Columns["OverHeadType"].Visible = false;
			grdHistoryOverHead.RootTable.HeaderLines = 2;
			grdHistoryOverHead.RootTable.Columns["OverHeadAccount"].HeaderAlignment = (TextAlignment)2;
			grdHistoryOverHead.RootTable.Columns["Qty"].HeaderAlignment = (TextAlignment)2;
			grdHistoryOverHead.RootTable.Columns["Rate"].HeaderAlignment = (TextAlignment)2;
			grdHistoryOverHead.RootTable.Columns["Amount"].HeaderAlignment = (TextAlignment)2;
			grdHistoryOverHead.RootTable.Columns["OhRemarks"].HeaderAlignment = (TextAlignment)2;
			grdHistoryOverHead.RootTable.Columns["OverHeadAccount"].Width = Constants.InventoryConstants.AccountTitle;
			grdHistoryOverHead.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
			grdHistoryOverHead.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdHistoryOverHead.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdHistoryOverHead.RootTable.Columns["OhRemarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdHistoryOverHead.RootTable.Columns["Percentage"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOverHead.RootTable.Columns["Percentage"].TextAlignment = (TextAlignment)3;
			grdHistoryOverHead.RootTable.Columns["Percentage"].FormatString = "##,#.####";
			grdHistoryOverHead.RootTable.Columns["Percentage"].TotalFormatString = "##,#.####";
			grdHistoryOverHead.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOverHead.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdHistoryOverHead.RootTable.Columns["Qty"].FormatString = "##,#.####";
			grdHistoryOverHead.RootTable.Columns["Qty"].TotalFormatString = "##,#.####";
			grdHistoryOverHead.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdHistoryOverHead.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryOverHead.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryOverHead.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdHistoryOverHead.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOverHead.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistoryOverHead.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdWagesHistorySetting()
	{
		try
		{
			grdWagesHistory.RootTable.Columns["Id"].Visible = false;
			grdWagesHistory.RootTable.HeaderLines = 2;
			grdWagesHistory.RootTable.Columns["ItemName"].HeaderAlignment = (TextAlignment)2;
			grdWagesHistory.RootTable.Columns["ContractorName"].HeaderAlignment = (TextAlignment)2;
			grdWagesHistory.RootTable.Columns["ServiceActivity"].HeaderAlignment = (TextAlignment)2;
			grdWagesHistory.RootTable.Columns["Qty"].HeaderAlignment = (TextAlignment)2;
			grdWagesHistory.RootTable.Columns["Rate"].HeaderAlignment = (TextAlignment)2;
			grdWagesHistory.RootTable.Columns["Amount"].HeaderAlignment = (TextAlignment)2;
			grdWagesHistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdWagesHistory.RootTable.Columns["ContractorName"].Width = Constants.InventoryConstants.SupplierName;
			grdWagesHistory.RootTable.Columns["ServiceActivity"].Width = Constants.InventoryConstants.ServiceActivity;
			grdWagesHistory.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
			grdWagesHistory.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdWagesHistory.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdWagesHistory.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdWagesHistory.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdWagesHistory.RootTable.Columns["Qty"].FormatString = "##,#.####";
			grdWagesHistory.RootTable.Columns["Qty"].TotalFormatString = "##,#.####";
			grdWagesHistory.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdWagesHistory.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdWagesHistory.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdWagesHistory.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdWagesHistory.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdWagesHistory.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdWagesHistory.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
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
			if (dtitem.Rows.Count > 0)
			{
				int ItemId = 0;
				if (Conversion.ToInt(cmbItemBOM.Value) > 0)
				{
					ItemId = Conversion.ToInt(cmbItemBOM.Value);
				}
				if (rdSearchByName.Checked)
				{
					DDL.BindDDL(dtitem, cmbItemBOM, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitem, cmbItemBOM, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				cmbItemBOM.Value = ItemId;
				cmbItemBOM.Focus();
			}
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
			AllCombobind();
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
					((Control)(object)DatagridHistory).Focus();
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
				if (e.Control && e.KeyCode == Keys.S && btnsave.Visible && btnsave.Enabled)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled && UpdateMode)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.P && btnPrint.Visible && btnPrint.Enabled)
				{
					btnPrint_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.L)
				{
					BtnLoadInputFromBom_Click(null, null);
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
					btnattachment_Click(null, null);
				}
				if (e.Control)
				{
					_ = e.KeyCode;
					_ = 123;
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdInputMaterial).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					txtdocdate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grdOutPutByProduct).Focused)
					{
						tabControl2.SelectedIndex = 1;
						((Control)(object)grdContractorWages).Focus();
					}
					else if (((Control)(object)grdContractorWages).Focused)
					{
						tabControl2.SelectedIndex = 2;
						((Control)(object)grdOverHead).Focus();
					}
					else if (((Control)(object)grdOverHead).Focused)
					{
						((Control)(object)grdInputMaterial).Focus();
					}
					else if (((Control)(object)grdInputMaterial).Focused)
					{
						tabControl2.SelectedIndex = 0;
						((Control)(object)grdOutPutByProduct).Focus();
					}
				}
			}
			else
			{
				if (tabControl1.SelectedIndex != 1)
				{
					return;
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)DatagridHistory).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.S)
				{
					btnshow_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					((Control)(object)grdHistoryOutput).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grdHistoryOutput).Focused)
					{
						((Control)(object)grdHistoryInput).Focus();
					}
					else if (((Control)(object)grdHistoryInput).Focused)
					{
						((Control)(object)grdWagesHistory).Focus();
					}
					else if (((Control)(object)grdWagesHistory).Focused)
					{
						((Control)(object)grdHistoryOverHead).Focus();
					}
					else if (((Control)(object)grdHistoryOverHead).Focused)
					{
						((Control)(object)grdHistoryOutput).Focus();
					}
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
			dt.Rows.Add("Ctrl+L", "For Load Input");
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
			throw new Exception(ex.Message);
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

	private void grdInputMaterial_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Expected O, but got Unknown
		//IL_09c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cf: Expected O, but got Unknown
		//IL_1025: Unknown result type (might be due to invalid IL or missing references)
		//IL_102c: Expected O, but got Unknown
		//IL_1531: Unknown result type (might be due to invalid IL or missing references)
		//IL_1538: Expected O, but got Unknown
		try
		{
			GridEXRow r = grdInputMaterial.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdInputMaterial.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				_ = grdInputMaterial.CurrentColumn.Key == "Add";
				if (grdInputMaterial.CurrentColumn.Key == "Delete")
				{
					DeleteInputDetailRow(r);
				}
				grdInputMaterial.UpdateData();
				Calculation();
			}
			if (e.Control)
			{
				_ = e.KeyCode;
				_ = 68;
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteInputDetailRow(r);
				grdInputMaterial.UpdateData();
				Calculation();
			}
			if (e.KeyCode == Keys.F1)
			{
				int Id = 0;
				decimal Name = default(decimal);
				string NameCode = "";
				string ItemName = "";
				if (grdInputMaterial.CurrentColumn != null && grdInputMaterial.CurrentColumn.Key == "ItemName")
				{
					int PreviousItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					GrdPopUp obj5 = new GrdPopUp(lstitem, "Id", "ItemName");
					obj5.StartPosition = FormStartPosition.CenterScreen;
					obj5.ShowDialog();
					obj5.BringToFront();
					Id = Conversion.ToInt((object)obj5.ReturnId);
					ItemName = Conversion.ToString((object)obj5.ReturnName);
					r.Cells["ItemId"].Value = Id;
					r.Cells["ItemName"].Value = ItemName;
					if (Id != PreviousItemId)
					{
						r.Cells["ItemUomId"].Value = Id;
						r.Cells["Uom"].Value = Name;
						r.Cells["UomCode"].Value = NameCode;
						r.Cells["RateUom"].Value = Name;
						r.Cells["RateUomId"].Value = Id;
						r.Cells["RateUomCode"].Value = NameCode;
						r.Cells["NetWeight"].Value = 0;
						r.Cells["Amount"].Value = 0;
					}
					DtStock.Clear();
					int wareId4 = Conversion.ToInt(r.Cells["WarehouseId"].Value);
					int ItemId4 = Conversion.ToInt(r.Cells["ItemId"].Value);
					int OrganizationId4 = UserAccount.OrganizationId;
					int CompanyId4 = UserAccount.CompanyId;
					DateTime DocDate4 = Conversion.ToDateTime((object)txtdocdate.Value);
					DtStock = GetAvgRatesAndStockInHand.GetStockInHandAndAvgRateFromEvaluationConcrete(OrganizationId4, CompanyId4, ItemId4, DocDate4, wareId4, 0, 0);
					if (DtStock.Rows.Count > 0)
					{
						decimal AvgRate4 = default(decimal);
						decimal Stock4 = Conversion.ToDecimal(DtStock.Rows[0]["QtyInHand"]);
						if (!FifoMethod)
						{
							AvgRate4 = Conversion.ToDecimal(DtStock.Rows[0]["AvgRate"]);
						}
						else
						{
							ReportsParameters obj9 = new ReportsParameters();
							obj9.OrganizationId = UserAccount.OrganizationId;
							obj9.CompanyId = UserAccount.CompanyId;
							obj9.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
							obj9.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
							obj9.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
							obj9.stockUOM = Conversion.ToInt(r.Cells["ItemAttributeVarientId"].Value);
							obj9.Id = RecId;
							if (obj9.Id > 0)
							{
								obj9.DocumentTypeId = 1867;
							}
							obj9.ItemQty = Conversion.ToDouble(r.Cells["ItemQty"].Value);
							obj9.NetWeight = obj9.ItemQty;
							AvgRate4 = Conversion.ToDecimal((object)CommonServices.GetAvgRateFromFIFOMethodConcrete(obj9));
						}
						r.Cells["BalanceStock"].Value = Stock4;
						r.Cells["Rate"].Value = AvgRate4;
						decimal RateUom4 = Conversion.ToDecimal(r.Cells["Rateuom"].Value);
						if (RateUom4 > 0m && AvgRate4 > 0m)
						{
							decimal num = Conversion.ToDecimal((object)r.Cells["ItemQty"].Value.ToString());
							decimal _Iuom3 = Conversion.ToDecimal((object)r.Cells["Uom"].Value.ToString());
							decimal Amount5 = num * _Iuom3 / RateUom4 * AvgRate4;
							r.Cells["Amount"].Value = Amount5;
						}
						else
						{
							r.Cells["Amount"].Value = 0;
						}
					}
					else
					{
						r.Cells["BalanceStock"].Value = 0;
						r.Cells["Rate"].Value = 0;
						r.Cells["Amount"].Value = 0;
					}
				}
				if (grdInputMaterial.CurrentColumn.Key == "UomCode")
				{
					bindUom(Conversion.ToInt(r.Cells["ItemId"].Value));
					GrdPopUp obj4 = new GrdPopUp(dtUom, "Id", "ItemUOMCode");
					obj4.StartPosition = FormStartPosition.CenterScreen;
					obj4.ShowDialog();
					obj4.BringToFront();
					Id = Conversion.ToInt((object)obj4.ReturnId);
					NameCode = Conversion.ToString((object)obj4.ReturnName);
					DataRow foundRow3 = dtUom.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == Id);
					Name = ((foundRow3 != null) ? Conversion.ToDecimal(foundRow3["ItemUOM"]) : 0m);
					r.Cells["ItemUomId"].Value = Id;
					r.Cells["Uom"].Value = Name;
					r.Cells["UomCode"].Value = NameCode;
					r.Cells["RateUom"].Value = Name;
					r.Cells["RateUomId"].Value = Id;
					r.Cells["RateUomCode"].Value = NameCode;
				}
				if (grdInputMaterial.CurrentColumn.Key == "RateUomCode")
				{
					bindRateUom(Conversion.ToInt(r.Cells["ItemId"].Value));
					GrdPopUp obj3 = new GrdPopUp(dtRateUom, "RateId", "RateUOMCode");
					obj3.StartPosition = FormStartPosition.CenterScreen;
					obj3.ShowDialog();
					obj3.BringToFront();
					Id = Conversion.ToInt((object)obj3.ReturnId);
					NameCode = Conversion.ToString((object)obj3.ReturnName);
					DataRow foundRow2 = dtRateUom.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("RateId") == Id);
					Name = ((foundRow2 != null) ? Conversion.ToDecimal(foundRow2["RateUOM"]) : 0m);
					r.Cells["RateUom"].Value = Name;
					r.Cells["RateUomId"].Value = Id;
					r.Cells["RateUomCode"].Value = NameCode;
					DtStock.Clear();
					int wareId3 = Conversion.ToInt(r.Cells["WarehouseId"].Value);
					int ItemId3 = Conversion.ToInt(r.Cells["ItemId"].Value);
					int OrganizationId3 = UserAccount.OrganizationId;
					int CompanyId3 = UserAccount.CompanyId;
					DateTime DocDate3 = Conversion.ToDateTime((object)txtdocdate.Value);
					DtStock = GetAvgRatesAndStockInHand.GetStockInHandAndAvgRateFromEvaluationConcrete(OrganizationId3, CompanyId3, ItemId3, DocDate3, wareId3, 0, 0);
					if (DtStock.Rows.Count > 0)
					{
						decimal AvgRate3 = default(decimal);
						decimal Stock3 = Conversion.ToDecimal(DtStock.Rows[0]["QtyInHand"]);
						if (!FifoMethod)
						{
							AvgRate3 = Conversion.ToDecimal(DtStock.Rows[0]["AvgRate"]);
						}
						else
						{
							ReportsParameters obj8 = new ReportsParameters();
							obj8.OrganizationId = UserAccount.OrganizationId;
							obj8.CompanyId = UserAccount.CompanyId;
							obj8.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
							obj8.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
							obj8.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
							obj8.stockUOM = Conversion.ToInt(r.Cells["ItemAttributeVarientId"].Value);
							obj8.Id = RecId;
							if (obj8.Id > 0)
							{
								obj8.DocumentTypeId = 1867;
							}
							obj8.ItemQty = Conversion.ToDouble(r.Cells["ItemQty"].Value);
							obj8.NetWeight = obj8.ItemQty;
							AvgRate3 = Conversion.ToDecimal((object)CommonServices.GetAvgRateFromFIFOMethodConcrete(obj8));
						}
						r.Cells["BalanceStock"].Value = Stock3;
						r.Cells["Rate"].Value = AvgRate3;
						decimal RateUom3 = Conversion.ToDecimal(r.Cells["Rateuom"].Value);
						if (RateUom3 > 0m && AvgRate3 > 0m)
						{
							decimal num2 = Conversion.ToDecimal((object)r.Cells["ItemQty"].Value.ToString());
							decimal _Iuom2 = Conversion.ToDecimal((object)r.Cells["Uom"].Value.ToString());
							decimal Amount4 = num2 * _Iuom2 / RateUom3 * AvgRate3;
							r.Cells["Amount"].Value = Amount4;
						}
						else
						{
							r.Cells["Amount"].Value = 0;
						}
					}
					else
					{
						r.Cells["BalanceStock"].Value = 0;
						r.Cells["Rate"].Value = 0;
						r.Cells["Amount"].Value = 0;
					}
				}
				decimal Weight = default(decimal);
				decimal Iuom = default(decimal);
				if (r.Cells["ItemQty"].Value.ToString() != string.Empty && r.Cells["ItemUomId"].Value.ToString() != string.Empty)
				{
					decimal num3 = Conversion.ToDecimal((object)r.Cells["ItemQty"].Value.ToString());
					Iuom = Conversion.ToDecimal((object)r.Cells["Uom"].Value.ToString());
					Weight = num3 * Iuom;
					r.Cells["NetWeight"].Value = Weight;
				}
				else
				{
					Weight = default(decimal);
					r.Cells["NetWeight"].Value = Weight;
				}
				if (Weight != 0m && Conversion.ToDecimal(r.Cells["Rate"].Value) != 0m && Conversion.ToInt(r.Cells["RateUomId"].Value) != 0)
				{
					decimal num4 = Conversion.ToDecimal((object)r.Cells["ItemQty"].Value.ToString());
					Iuom = Conversion.ToDecimal((object)r.Cells["Uom"].Value.ToString());
					decimal rate = Conversion.ToDecimal((object)r.Cells["Rate"].Value.ToString());
					decimal Ruom = Conversion.ToDecimal((object)r.Cells["RateUom"].Value.ToString());
					decimal Amount3 = num4 * Iuom / Ruom * rate;
					r.Cells["Amount"].Value = Amount3;
				}
				else
				{
					r.Cells["Amount"].Value = 0;
				}
				if (grdInputMaterial.CurrentColumn.Key == "WarehouseName")
				{
					if (Conversion.ToInt(CmbPlantName.Value) == 0)
					{
						MessageBox.Show("Please Select Plant First.");
						return;
					}
					dtWareForInput = CommonServices.WareHouseAllocationToPlantsByProductionPlantId(Conversion.ToInt(CmbPlantName.Value));
					GrdPopUp obj2 = new GrdPopUp(dtWareForInput, "Id", "WarehouseName");
					obj2.StartPosition = FormStartPosition.CenterScreen;
					obj2.ShowDialog();
					obj2.BringToFront();
					Id = Conversion.ToInt((object)obj2.ReturnId);
					NameCode = Conversion.ToString((object)obj2.ReturnName);
					r.Cells["WarehouseId"].Value = Id;
					r.Cells["WarehouseName"].Value = NameCode;
					DtStock.Clear();
					int wareId2 = Conversion.ToInt(r.Cells["WarehouseId"].Value);
					int ItemId2 = Conversion.ToInt(r.Cells["ItemId"].Value);
					int OrganizationId2 = UserAccount.OrganizationId;
					int CompanyId2 = UserAccount.CompanyId;
					DateTime DocDate2 = Conversion.ToDateTime((object)txtdocdate.Value);
					DtStock = GetAvgRatesAndStockInHand.GetStockInHandAndAvgRateFromEvaluationConcrete(OrganizationId2, CompanyId2, ItemId2, DocDate2, wareId2, 0, 0);
					if (DtStock.Rows.Count > 0)
					{
						decimal AvgRate2 = default(decimal);
						decimal Stock2 = Conversion.ToDecimal(DtStock.Rows[0]["QtyInHand"]);
						if (!FifoMethod)
						{
							AvgRate2 = Conversion.ToDecimal(DtStock.Rows[0]["AvgRate"]);
						}
						else
						{
							ReportsParameters obj7 = new ReportsParameters();
							obj7.OrganizationId = UserAccount.OrganizationId;
							obj7.CompanyId = UserAccount.CompanyId;
							obj7.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
							obj7.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
							obj7.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
							obj7.stockUOM = Conversion.ToInt(r.Cells["ItemAttributeVarientId"].Value);
							obj7.Id = RecId;
							if (obj7.Id > 0)
							{
								obj7.DocumentTypeId = 1867;
							}
							obj7.ItemQty = Conversion.ToDouble(r.Cells["ItemQty"].Value);
							obj7.NetWeight = obj7.ItemQty;
							AvgRate2 = Conversion.ToDecimal((object)CommonServices.GetAvgRateFromFIFOMethodConcrete(obj7));
						}
						r.Cells["BalanceStock"].Value = Stock2;
						r.Cells["Rate"].Value = AvgRate2;
						decimal RateUom2 = Conversion.ToDecimal(r.Cells["Rateuom"].Value);
						if (RateUom2 > 0m && AvgRate2 > 0m)
						{
							decimal num5 = Conversion.ToDecimal((object)r.Cells["ItemQty"].Value.ToString());
							decimal _Iuom = Conversion.ToDecimal((object)r.Cells["Uom"].Value.ToString());
							decimal Amount2 = num5 * _Iuom / RateUom2 * AvgRate2;
							r.Cells["Amount"].Value = Amount2;
						}
						else
						{
							r.Cells["Amount"].Value = 0;
						}
					}
					else
					{
						r.Cells["BalanceStock"].Value = 0;
						r.Cells["Rate"].Value = 0;
						r.Cells["Amount"].Value = 0;
					}
				}
				if (grdInputMaterial.CurrentColumn.Key == "AttributeVarient")
				{
					dtAttributes.Clear();
					dtAttributes = ItemAttributeVarient.GetAllForCombo(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToInt(r.Cells["ItemId"].Value));
					GrdPopUp obj = new GrdPopUp(dtAttributes, "ItemAttributeVarientId", "ItemAttribute");
					obj.StartPosition = FormStartPosition.CenterScreen;
					obj.ShowDialog();
					obj.BringToFront();
					Id = Conversion.ToInt((object)obj.ReturnId);
					NameCode = Conversion.ToString((object)obj.ReturnName);
					DataRow foundRow = dtAttributes.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("ItemAttributeVarientId") == Id);
					Name = ((foundRow != null) ? Conversion.ToDecimal(foundRow["VarientEquivalent"]) : 0m);
					r.Cells["ItemAttributeVarientId"].Value = Id;
					r.Cells["AttributeVarient"].Value = NameCode;
					r.Cells["VarientEquivalent"].Value = Name;
					decimal num6 = Conversion.ToDecimal((object)r.Cells["ItemQty"].Value.ToString());
					decimal VarientEquivalent = Conversion.ToDecimal((object)r.Cells["VarientEquivalent"].Value.ToString());
					decimal VarientQty = num6 * VarientEquivalent;
					r.Cells["VarientQty"].Value = VarientQty;
					DtStock.Clear();
					int wareId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
					int ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					int OrganizationId = UserAccount.OrganizationId;
					int CompanyId = UserAccount.CompanyId;
					DateTime DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
					DtStock = GetAvgRatesAndStockInHand.GetStockInHandAndAvgRateFromEvaluationConcrete(OrganizationId, CompanyId, ItemId, DocDate, wareId, 0, 0);
					if (DtStock.Rows.Count > 0)
					{
						decimal AvgRate = default(decimal);
						decimal Stock = Conversion.ToDecimal(DtStock.Rows[0]["QtyInHand"]);
						if (!FifoMethod)
						{
							AvgRate = Conversion.ToDecimal(DtStock.Rows[0]["AvgRate"]);
						}
						else
						{
							ReportsParameters obj6 = new ReportsParameters();
							obj6.OrganizationId = UserAccount.OrganizationId;
							obj6.CompanyId = UserAccount.CompanyId;
							obj6.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
							obj6.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
							obj6.WarehouseId = wareId;
							obj6.stockUOM = Conversion.ToInt(r.Cells["ItemAttributeVarientId"].Value);
							obj6.Id = RecId;
							if (obj6.Id > 0)
							{
								obj6.DocumentTypeId = 1867;
							}
							obj6.ItemQty = Conversion.ToDouble(r.Cells["ItemQty"].Value);
							obj6.NetWeight = obj6.ItemQty;
							AvgRate = Conversion.ToDecimal((object)CommonServices.GetAvgRateFromFIFOMethodConcrete(obj6));
						}
						r.Cells["BalanceStock"].Value = Stock;
						r.Cells["Rate"].Value = AvgRate;
						decimal RateUom = Conversion.ToDecimal(r.Cells["Rateuom"].Value);
						if (RateUom > 0m && AvgRate > 0m)
						{
							decimal num7 = Conversion.ToDecimal((object)r.Cells["ItemQty"].Value.ToString());
							decimal _Iuom4 = Conversion.ToDecimal((object)r.Cells["Uom"].Value.ToString());
							decimal Amount = num7 * _Iuom4 / RateUom * AvgRate;
							r.Cells["Amount"].Value = Amount;
						}
						else
						{
							r.Cells["Amount"].Value = 0;
						}
					}
					else
					{
						r.Cells["BalanceStock"].Value = 0;
						r.Cells["Rate"].Value = 0;
						r.Cells["Amount"].Value = 0;
					}
				}
				grdInputMaterial.UpdateData();
				Calculation();
			}
			EnableDisablePlant();
			EnableDisableProductionType();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOutPutByProduct_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Expected O, but got Unknown
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdOutPutByProduct.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdOutPutByProduct.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grdOutPutByProduct.CurrentColumn.Key == "Add")
				{
					if (Conversion.ToInt(cmbItemBOM.Value) == 0)
					{
						cmbItemBOM.Focus();
						throw new Exception("Please Select Item First");
					}
					AddRowInOutPutByProductGrid();
				}
				if (grdOutPutByProduct.CurrentColumn.Key == "Delete")
				{
					if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
					{
						if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
						{
							return;
						}
						ConversionOutPutDetail d2 = new ConversionOutPutDetail();
						d2.Id = Conversion.ToInt(r.Cells["Id"].Value);
						d2.WareHouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
						d2.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						d2.ItemAttributeVarientId = Conversion.ToInt(r.Cells["ItemAttributeVarientId"].Value);
						d2.ItemUomId = 0;
						d2.ItemQty = Conversion.ToDecimal((object)r.Cells["ItemQty"].Text);
						d2.VarientQty = Conversion.ToDecimal((object)r.Cells["VarientQty"].Text);
						d2.VarientEquivalent = Conversion.ToDecimal((object)r.Cells["VarientEquivalent"].Text);
						d2.NetWeight = 0m;
						d2.Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
						d2.RateUomId = 0;
						d2.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
						d2.ActionTypeId = 3;
						lstRemoveRecordOutPut.Add(d2);
						dtOutPutByProduct.Rows[r.RowIndex].Delete();
						if (r != null && (int)r.RowType == 0)
						{
							r.Delete();
						}
						grdOutPutByProduct.UpdateData();
					}
					else if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					if (dtOutPutByProduct.Rows.Count == 0)
					{
						AddRowInOutPutByProductGrid();
					}
				}
				grdOutPutByProduct.UpdateData();
				Calculation();
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				if (Conversion.ToInt(cmbItemBOM.Value) == 0)
				{
					cmbItemBOM.Focus();
					throw new Exception("Please Select Item First");
				}
				AddRowInOutPutByProductGrid();
				grdOutPutByProduct.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					ConversionOutPutDetail d1 = new ConversionOutPutDetail();
					d1.Id = Conversion.ToInt(r.Cells["Id"].Value);
					d1.WareHouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
					d1.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					d1.ItemAttributeVarientId = Conversion.ToInt(r.Cells["ItemAttributeVarientId"].Value);
					d1.ItemUomId = 0;
					d1.ItemQty = Conversion.ToDecimal((object)r.Cells["ItemQty"].Text);
					d1.VarientQty = Conversion.ToDecimal((object)r.Cells["VarientQty"].Text);
					d1.VarientEquivalent = Conversion.ToDecimal((object)r.Cells["VarientEquivalent"].Text);
					d1.NetWeight = 0m;
					d1.Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
					d1.RateUomId = 0;
					d1.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
					d1.ActionTypeId = 3;
					lstRemoveRecordOutPut.Add(d1);
					dtOutPutByProduct.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					grdOutPutByProduct.UpdateData();
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				if (dtOutPutByProduct.Rows.Count == 0)
				{
					AddRowInOutPutByProductGrid();
				}
				grdOutPutByProduct.UpdateData();
				Calculation();
			}
			if (e.KeyCode != Keys.F1)
			{
				return;
			}
			int Id = 0;
			decimal Name = default(decimal);
			string NameCode = "";
			if (grdOutPutByProduct.CurrentColumn.Key == "ItemName")
			{
				lstitem.Clear();
				lstitem = CommonServices.GetItemByItemTypeId(Conversion.ToString(CmbProductionType.Value));
				int PreviousItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				GrdPopUp obj3 = new GrdPopUp(lstitem, "Id", "ItemName");
				obj3.StartPosition = FormStartPosition.CenterScreen;
				obj3.ShowDialog();
				obj3.BringToFront();
				Id = Conversion.ToInt((object)obj3.ReturnId);
				NameCode = Conversion.ToString((object)obj3.ReturnName);
				r.Cells["ItemId"].Value = Id;
				r.Cells["ItemName"].Value = NameCode;
				if (Id != PreviousItemId)
				{
					r.Cells["ItemUomId"].Value = Id;
					r.Cells["Uom"].Value = Name;
					r.Cells["UomCode"].Value = NameCode;
					r.Cells["RateUom"].Value = Name;
					r.Cells["RateUomId"].Value = Id;
					r.Cells["RateUomCode"].Value = NameCode;
					r.Cells["NetWeight"].Value = 0;
				}
				if (Conversion.ToInt((object)r.Cells["ItemAttributeVarientId"]) == 0)
				{
					dtAttributes.Clear();
					int ItemAttributeVarientId = 0;
					dtAttributes = ItemAttributeVarient.GetAllForCombo(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToInt(r.Cells["ItemId"].Value));
					if (dtAttributes.Rows.Count > 0)
					{
						DataRow firstRow = dtAttributes.AsEnumerable().FirstOrDefault();
						if (firstRow != null)
						{
							ItemAttributeVarientId = Conversion.ToInt(firstRow["ItemAttributeVarientId"]);
						}
						DataRow foundRow2 = dtAttributes.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("ItemAttributeVarientId") == ItemAttributeVarientId);
						string AttributeVarient = ((foundRow2 != null) ? Conversion.ToString(foundRow2["ItemAttribute"]) : "");
						decimal VarientEquivalent2 = ((foundRow2 != null) ? Conversion.ToDecimal(foundRow2["VarientEquivalent"]) : 0m);
						r.Cells["ItemAttributeVarientId"].Value = Id;
						r.Cells["AttributeVarient"].Value = AttributeVarient;
						r.Cells["VarientEquivalent"].Value = VarientEquivalent2;
						decimal VarientQty2 = Conversion.ToDecimal((object)r.Cells["ItemQty"].Value.ToString()) * VarientEquivalent2;
						r.Cells["VarientQty"].Value = VarientQty2;
						decimal CurrentRowAmount2 = Conversion.ToDecimal(r.Cells["Rate"].Value) * VarientQty2;
						r.Cells["Amount"].Value = CurrentRowAmount2;
					}
				}
			}
			if (grdOutPutByProduct.CurrentColumn.Key == "WarehouseName")
			{
				GrdPopUp obj2 = new GrdPopUp(dtWareForOutPut, "Id", "WarehouseName");
				obj2.StartPosition = FormStartPosition.CenterScreen;
				obj2.ShowDialog();
				obj2.BringToFront();
				Id = Conversion.ToInt((object)obj2.ReturnId);
				NameCode = Conversion.ToString((object)obj2.ReturnName);
				r.Cells["WarehouseId"].Value = Id;
				r.Cells["WarehouseName"].Value = NameCode;
			}
			if (grdOutPutByProduct.CurrentColumn.Key == "AttributeVarient")
			{
				dtAttributes.Clear();
				dtAttributes = ItemAttributeVarient.GetAllForCombo(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToInt(r.Cells["ItemId"].Value));
				GrdPopUp obj = new GrdPopUp(dtAttributes, "ItemAttributeVarientId", "ItemAttribute");
				obj.StartPosition = FormStartPosition.CenterScreen;
				obj.ShowDialog();
				obj.BringToFront();
				Id = Conversion.ToInt((object)obj.ReturnId);
				NameCode = Conversion.ToString((object)obj.ReturnName);
				DataRow foundRow = dtAttributes.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("ItemAttributeVarientId") == Id);
				Name = ((foundRow != null) ? Conversion.ToDecimal(foundRow["VarientEquivalent"]) : 0m);
				r.Cells["ItemAttributeVarientId"].Value = Id;
				r.Cells["AttributeVarient"].Value = NameCode;
				r.Cells["VarientEquivalent"].Value = Name;
				decimal num = Conversion.ToDecimal((object)r.Cells["ItemQty"].Value.ToString());
				decimal VarientEquivalent = Conversion.ToDecimal((object)r.Cells["VarientEquivalent"].Value.ToString());
				decimal VarientQty = num * VarientEquivalent;
				r.Cells["VarientQty"].Value = VarientQty;
				decimal CurrentRowAmount = Conversion.ToDecimal(r.Cells["Rate"].Value) * VarientQty;
				r.Cells["Amount"].Value = CurrentRowAmount;
			}
			grdOutPutByProduct.UpdateData();
			EnableDisableProductionType();
			Calculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOverHead_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdOverHead.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdOverHead.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grdOverHead.CurrentColumn.Key == "Add")
				{
					AddRowInOverHeadGrid();
				}
				if (grdOverHead.CurrentColumn.Key == "Delete")
				{
					dtOverHead.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					if (dtOverHead.Rows.Count == 0)
					{
						AddRowInOverHeadGrid();
					}
				}
				grdOverHead.UpdateData();
				Calculation();
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInOverHeadGrid();
				grdOverHead.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				dtOverHead.Rows[r.RowIndex].Delete();
				if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				if (dtOverHead.Rows.Count == 0)
				{
					AddRowInOverHeadGrid();
				}
				grdOverHead.UpdateData();
				Calculation();
			}
			if (e.KeyCode == Keys.F1 && grdOverHead.CurrentColumn != null && grdOverHead.CurrentColumn.Key == "OverHeadAccountId")
			{
				GrdPopUp obj = new GrdPopUp(dtAccounts, "Id", "AccountTitle");
				obj.StartPosition = FormStartPosition.CenterScreen;
				obj.ShowDialog();
				obj.BringToFront();
				r.Cells["OverHeadAccountId"].Value = obj.ReturnId;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdContractorWages_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdContractorWages.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdContractorWages.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grdContractorWages.CurrentColumn.Key == "Add")
				{
					AddRowInContractorWagesGrid();
				}
				if (grdContractorWages.CurrentColumn.Key == "Delete")
				{
					dtContractorWages.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					if (dtContractorWages.Rows.Count == 0)
					{
						AddRowInContractorWagesGrid();
					}
				}
				grdContractorWages.UpdateData();
				Calculation();
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInContractorWagesGrid();
				grdContractorWages.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				dtContractorWages.Rows[r.RowIndex].Delete();
				if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				if (dtContractorWages.Rows.Count == 0)
				{
					AddRowInContractorWagesGrid();
				}
				grdContractorWages.UpdateData();
				Calculation();
			}
			if (e.KeyCode != Keys.F1)
			{
				return;
			}
			int Id = 0;
			string Name = "";
			if (grdContractorWages.CurrentColumn != null && (grdContractorWages.CurrentColumn.Key == "ContractorId" || grdContractorWages.CurrentColumn.Key == "ContractorName"))
			{
				int OutPutItemId2 = Conversion.ToInt(r.Cells["OutPutItemId"].Value);
				if (OutPutItemId2 == 0)
				{
					throw new Exception("Please Select OutPut Item for Wages Entry...");
				}
				GrdPopUp obj3 = new GrdPopUp(dtContr, "Id", "CompanyName");
				obj3.StartPosition = FormStartPosition.CenterScreen;
				obj3.ShowDialog();
				obj3.BringToFront();
				Id = Conversion.ToInt((object)obj3.ReturnId);
				Name = Conversion.ToString((object)obj3.ReturnName);
				r.Cells["ContractorId"].Value = Id;
				r.Cells["ContractorName"].Value = Name;
				string ServiceActivityId = Conversion.ToString(r.Cells["ServiceActivity"].Value);
				DataTable dt2 = ContractorWagesRateSchedule.GetServiceActivityByItemId(1, OutPutItemId2, Conversion.ToDateTime((object)txtdocdate.Value), 0, Id);
				if (dt2.Rows.Count > 0)
				{
					DataRow foundRow2 = dt2.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<string>("WagesActivity") == ServiceActivityId);
					decimal Rate2 = ((foundRow2 != null) ? Conversion.ToDecimal(foundRow2["Rate"]) : 0m);
					int ContractorWagesRateScheduleId = ((foundRow2 != null) ? Conversion.ToInt(foundRow2["ContractorWagesRateScheduleId"]) : 0);
					r.Cells["ContractorWagesRateScheduleId"].Value = ContractorWagesRateScheduleId;
					decimal Qty2 = ((foundRow2 != null) ? Conversion.ToDecimal(foundRow2["Qty"]) : 0m);
					decimal RatePerQty2 = ((Qty2 > 0m) ? (Rate2 / Qty2) : 0m);
					r.Cells["Rate"].Value = RatePerQty2;
					decimal CurrRowQty2 = Conversion.ToInt(r.Cells["Qty"].Value);
					decimal Amount2 = RatePerQty2 * CurrRowQty2;
					r.Cells["Amount"].Value = Amount2;
				}
			}
			if (grdContractorWages.CurrentColumn != null && grdContractorWages.CurrentColumn.Key == "ServiceActivity")
			{
				int ContrId = Conversion.ToInt(r.Cells["ContractorId"].Value);
				int OutPutItemId = Conversion.ToInt(r.Cells["OutPutItemId"].Value);
				if (OutPutItemId == 0)
				{
					throw new Exception("Please Select OutPut Item for Wages Entry...");
				}
				DataTable dt = ContractorWagesRateSchedule.GetServiceActivityByItemId(1, OutPutItemId, Conversion.ToDateTime((object)txtdocdate.Value), 0, ContrId);
				GrdPopUp obj2 = new GrdPopUp(dt, "ContractorWagesRateScheduleId", "WagesActivity");
				obj2.StartPosition = FormStartPosition.CenterScreen;
				obj2.ShowDialog();
				obj2.BringToFront();
				Id = Conversion.ToInt((object)obj2.ReturnId);
				Name = Conversion.ToString((object)obj2.ReturnName);
				r.Cells["ContractorWagesRateScheduleId"].Value = Id;
				r.Cells["ServiceActivity"].Value = Name;
				if (Id > 0)
				{
					DataRow foundRow = dt.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("ContractorWagesRateScheduleId") == Id);
					decimal Qty = ((foundRow != null) ? Conversion.ToDecimal(foundRow["Qty"]) : 0m);
					decimal Rate = ((foundRow != null) ? Conversion.ToDecimal(foundRow["Rate"]) : 0m);
					decimal RatePerQty = ((Qty > 0m) ? (Rate / Qty) : 0m);
					r.Cells["Rate"].Value = RatePerQty;
					decimal CurrRowQty = Conversion.ToInt(r.Cells["Qty"].Value);
					decimal Amount = RatePerQty * CurrRowQty;
					r.Cells["Amount"].Value = Amount;
				}
				else
				{
					r.Cells["Rate"].Value = 0;
					r.Cells["Amount"].Value = 0;
				}
			}
			if (grdContractorWages.CurrentColumn != null && (grdContractorWages.CurrentColumn.Key == "OutPutItemId" || grdContractorWages.CurrentColumn.Key == "OutPutItemName"))
			{
				if (Conversion.ToInt((object)grdOutPutByProduct.GetDataRows().Count()) == 0)
				{
					throw new Exception("Out Put Grid Record not founds...");
				}
				GetOutPutGridItemsForWages(0);
				if (dtOutputItem.Rows.Count > 0)
				{
					GrdPopUp obj = new GrdPopUp(dtOutputItem, "OutItemId", "OutItemName");
					obj.StartPosition = FormStartPosition.CenterScreen;
					obj.ShowDialog();
					obj.BringToFront();
					r.Cells["OutPutItemId"].Value = obj.ReturnId;
					r.Cells["OutPutItemName"].Value = obj.ReturnName;
				}
			}
			grdContractorWages.UpdateData();
			Calculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || DatagridHistory.CurrentColumn == null)
			{
				return;
			}
			int Id = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (DatagridHistory.CurrentColumn.Key == "Edit")
				{
					if (!formright.DoHaveUpdateRights)
					{
						throw new Exception("you don't have update rights...");
					}
					if (Conversion.ToInt(DatagridHistory.CurrentRow.Cells["ReleaseNo"].Value) > 0)
					{
						MessageBox.Show("You can't update record because this document is referred in release", "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					WF.Show();
					ReadById(Conversion.ToInt((object)Id));
				}
				if (DatagridHistory.CurrentColumn.Key == "Print")
				{
					CommonServices.ConversionSlipConcrete(Conversion.ToInt(DatagridHistory.CurrentRow.Cells["Id"].Value));
				}
				if (DatagridHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, 1867);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("you don't have update rights...");
				}
				if (Conversion.ToInt(DatagridHistory.CurrentRow.Cells["ReleaseNo"].Value) > 0)
				{
					MessageBox.Show("You can't update record because this document is referred in release", "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				WF.Show();
				ReadById(Conversion.ToInt((object)Id));
			}
		}
		catch (Exception ex)
		{
			WF.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			WF.Close();
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DatagridHistory).Name);
			if (fs != null)
			{
				DatagridHistory.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdInputMaterial).Name);
			if (fs != null)
			{
				grdInputMaterial.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdOutPutByProduct).Name);
			if (fs != null)
			{
				grdOutPutByProduct.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnGenerateRatesOutput_Click(object sender, EventArgs e)
	{
		try
		{
			UpdateInputCurrentStockAndAvgRate();
			UpdateWagesRates();
			UpdateRatesOutput();
			WF.Close();
		}
		catch (Exception ex)
		{
			WF.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			WF.Close();
		}
	}

	private void UpdateInputQtyIncaseOfSlabProduction1(bool isSlab)
	{
		try
		{
			if (grdInputMaterial.GetDataRows().Count() == 0)
			{
				throw new Exception("Input Grid Record not Found");
			}
			foreach (var item in (from row in dtOutPutByProduct.AsEnumerable()
				group row by (!row.IsNull("BomHeaderId")) ? Conversion.ToInt((object)row.Field<int>("BomHeaderId")) : 0 into @group
				select new
				{
					BomHeaderId = @group.Key,
					TotalItemQty = @group.Sum((DataRow row) => (!row.IsNull("ItemQty")) ? Conversion.ToDouble((object)row.Field<decimal>("ItemQty")) : 0.0)
				}).ToList())
			{
				int outBomId = item.BomHeaderId;
				decimal totalOutQtyOfCurrBom = Conversion.ToDecimal((object)item.TotalItemQty);
				foreach (GridEXRow item2 in from r in grdInputMaterial.GetDataRows()
					where Conversion.ToInt(r.Cells["BomHeaderId"].Value) == outBomId
					select r)
				{
					item2.BeginEdit();
					decimal avgRate = Conversion.ToDecimal(item2.Cells["Rate"].Value);
					decimal rateUom = Conversion.ToDecimal(item2.Cells["Rateuom"].Value);
					decimal qty = Conversion.ToDecimal((object)item2.Cells["BomQty"].Value.ToString());
					decimal finalQty = (isSlab ? (qty * totalOutQtyOfCurrBom) : qty);
					item2.Cells["ItemQty"].Value = finalQty;
					decimal BomMinQty = Conversion.ToDecimal((object)item2.Cells["BomMinQty"].Value.ToString());
					decimal BomMaxQty = Conversion.ToDecimal((object)item2.Cells["BomMaxQty"].Value.ToString());
					item2.Cells["MinQty"].Value = BomMinQty * (isSlab ? totalOutQtyOfCurrBom : 1m);
					item2.Cells["MaxQty"].Value = BomMaxQty * (isSlab ? totalOutQtyOfCurrBom : 1m);
					decimal iuom = Conversion.ToDecimal((object)item2.Cells["Uom"].Value.ToString());
					item2.Cells["NetWeight"].Value = finalQty * iuom;
					item2.Cells["Amount"].Value = ((rateUom > 0m && avgRate > 0m) ? (finalQty * iuom / rateUom * avgRate) : 0m);
					item2.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void UpdateWagesQtyIncaseOfSlabProduction(bool IsSlab)
	{
		try
		{
			if (grdContractorWages.GetDataRows().Count() == 0)
			{
				throw new Exception("Wages Grid Record not Found");
			}
			foreach (var item in (from row in dtOutPutByProduct.AsEnumerable()
				group row by new
				{
					BomHeaderId = ((!row.IsNull("BomHeaderId")) ? Conversion.ToInt((object)row.Field<int>("BomHeaderId")) : 0)
				} into @group
				select new
				{
					BomHeaderId = @group.Key.BomHeaderId,
					TotalItemQty = @group.Sum((DataRow row) => (!row.IsNull("ItemQty")) ? Conversion.ToDouble((object)row.Field<decimal>("ItemQty")) : 0.0)
				}).ToList())
			{
				int OutBomId = item.BomHeaderId;
				decimal TotalOutQtyOfCurrBom = (IsSlab ? Conversion.ToDecimal((object)item.TotalItemQty) : 1m);
				foreach (GridEXRow item2 in from r in grdContractorWages.GetDataRows()
					where Conversion.ToInt(r.Cells["BomHeaderId"].Value) == OutBomId
					select r)
				{
					item2.BeginEdit();
					decimal AvgRate = Conversion.ToDecimal(item2.Cells["Rate"].Value);
					decimal FinalQty = Conversion.ToDecimal((object)item2.Cells["BomQty"].Value.ToString()) * TotalOutQtyOfCurrBom;
					item2.Cells["Qty"].Value = FinalQty;
					decimal Amount = ((FinalQty > 0m && AvgRate > 0m) ? (FinalQty * AvgRate) : 0m);
					item2.Cells["Amount"].Value = Amount;
					item2.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void UpdateOHQtyIncaseOfSlabProduction(bool IsSlab)
	{
		try
		{
			if (grdOverHead.GetDataRows().Count() == 0)
			{
				throw new Exception("Overhead Grid Record not Found");
			}
			foreach (var item in (from row in dtOutPutByProduct.AsEnumerable()
				group row by new
				{
					BomHeaderId = ((!row.IsNull("BomHeaderId")) ? Conversion.ToInt((object)row.Field<int>("BomHeaderId")) : 0)
				} into @group
				select new
				{
					BomHeaderId = @group.Key.BomHeaderId,
					TotalItemQty = @group.Sum((DataRow row) => (!row.IsNull("ItemQty")) ? Conversion.ToDouble((object)row.Field<decimal>("ItemQty")) : 0.0)
				}).ToList())
			{
				int OutBomId = item.BomHeaderId;
				decimal TotalOutQtyOfCurrBom = (IsSlab ? Conversion.ToDecimal((object)item.TotalItemQty) : 1m);
				foreach (GridEXRow item2 in from r in grdOverHead.GetDataRows()
					where Conversion.ToInt(r.Cells["BomHeaderId"].Value) == OutBomId
					select r)
				{
					item2.BeginEdit();
					decimal AvgRate = Conversion.ToDecimal(item2.Cells["Rate"].Value);
					decimal FinalQty = Conversion.ToDecimal((object)item2.Cells["BomQty"].Value.ToString()) * TotalOutQtyOfCurrBom;
					item2.Cells["Qty"].Value = FinalQty;
					decimal Amount = ((FinalQty > 0m && AvgRate > 0m) ? (FinalQty * AvgRate) : 0m);
					item2.Cells["Amount"].Value = Amount;
					item2.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void UpdateInputCurrentStockAndAvgRate()
	{
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		try
		{
			if (grdInputMaterial.GetDataRows().Count() > 0)
			{
				DtStock.Clear();
				GridEXRow[] dataRows = grdInputMaterial.GetDataRows();
				foreach (GridEXRow r in dataRows)
				{
					int wareId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
					int ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					int OrganizationId = UserAccount.OrganizationId;
					int CompanyId = UserAccount.CompanyId;
					DateTime DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
					DtStock = GetAvgRatesAndStockInHand.GetStockInHandAndAvgRateFromEvaluationConcrete(OrganizationId, CompanyId, ItemId, DocDate, wareId, 0, 0);
					if (DtStock.Rows.Count > 0)
					{
						decimal AvgRate = default(decimal);
						decimal Stock = Conversion.ToDecimal(DtStock.Rows[0]["QtyInHand"]);
						if (!FifoMethod)
						{
							AvgRate = Conversion.ToDecimal(DtStock.Rows[0]["AvgRate"]);
						}
						else
						{
							ReportsParameters obj = new ReportsParameters();
							obj.OrganizationId = UserAccount.OrganizationId;
							obj.CompanyId = UserAccount.CompanyId;
							obj.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
							obj.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
							obj.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
							obj.stockUOM = Conversion.ToInt(r.Cells["ItemAttributeVarientId"].Value);
							obj.Id = RecId;
							if (obj.Id > 0)
							{
								obj.DocumentTypeId = 1867;
							}
							obj.ItemQty = Conversion.ToDouble(r.Cells["ItemQty"].Value);
							obj.NetWeight = obj.ItemQty;
							AvgRate = Conversion.ToDecimal((object)CommonServices.GetAvgRateFromFIFOMethodConcrete(obj));
						}
						r.BeginEdit();
						r.Cells["BalanceStock"].Value = Stock;
						r.Cells["Rate"].Value = AvgRate;
						decimal RateUom = Conversion.ToDecimal(r.Cells["Rateuom"].Value);
						if (RateUom > 0m && AvgRate > 0m)
						{
							decimal num = Conversion.ToDecimal((object)r.Cells["ItemQty"].Value.ToString());
							decimal Iuom = Conversion.ToDecimal((object)r.Cells["Uom"].Value.ToString());
							decimal Amount = num * Iuom / RateUom * AvgRate;
							r.Cells["Amount"].Value = Amount;
						}
						else
						{
							r.Cells["Amount"].Value = 0;
						}
						r.EndEdit();
					}
					else
					{
						r.BeginEdit();
						r.Cells["BalanceStock"].Value = 0;
						r.Cells["Rate"].Value = 0;
						r.Cells["Amount"].Value = 0;
						r.EndEdit();
					}
				}
				return;
			}
			if (grdInputMaterial.GetDataRows().Count() == 1 && Conversion.ToInt(grdInputMaterial.GetRow(0).Cells["WarehouseId"].Value) == 0)
			{
				throw new Exception("Warehouse Field is Empty in Input Grid Row No: 01");
			}
			throw new Exception("Input Grid Record not Found");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void UpdateWagesRates()
	{
		try
		{
			if (grdContractorWages.GetDataRows().Count() <= 0)
			{
				return;
			}
			GridEXRow[] dataRows = grdContractorWages.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				int ContrId = Conversion.ToInt(r.Cells["ContractorId"].Value);
				int ServiceActivityId = Conversion.ToInt(r.Cells["ContractorWagesRateScheduleId"].Value);
				r.BeginEdit();
				if (ServiceActivityId > 0)
				{
					DataTable dt = ContractorWagesRateSchedule.GetServiceActivityByItemId(1, Conversion.ToInt(r.Cells["OutPutItemId"].Value), Conversion.ToDateTime((object)txtdocdate.Value), ServiceActivityId, ContrId);
					if (dt.Rows.Count > 0)
					{
						DataRow foundRow = dt.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("ContractorWagesRateScheduleId") == ServiceActivityId);
						decimal Qty = ((foundRow != null) ? Conversion.ToDecimal(foundRow["Qty"]) : 0m);
						decimal Rate = ((foundRow != null) ? Conversion.ToDecimal(foundRow["Rate"]) : 0m);
						decimal RatePerQty = ((Qty > 0m) ? (Rate / Qty) : 0m);
						r.Cells["Rate"].Value = RatePerQty;
						decimal CurrRowQty = Conversion.ToInt(r.Cells["Qty"].Value);
						decimal Amount = RatePerQty * CurrRowQty;
						r.Cells["Amount"].Value = Amount;
					}
					else
					{
						r.Cells["Rate"].Value = 0;
						r.Cells["Amount"].Value = 0;
					}
				}
				else
				{
					r.Cells["Rate"].Value = 0;
					r.Cells["Amount"].Value = 0;
				}
				r.EndEdit();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void UpdateRatesOutput()
	{
		try
		{
			if (grdOutPutByProduct.GetDataRows().Count() == 1 && Conversion.ToInt(grdOutPutByProduct.GetDataRows().FirstOrDefault().Cells["ItemId"].Value) == 0)
			{
				WF.Close();
				throw new Exception("Output Grid Record not Filled Completely");
			}
			Calculation();
			decimal num = (from GridEXRow row in grdInputMaterial.GetDataRows()
				select (decimal)row.Cells["Amount"].Value).Sum();
			decimal sumOfOverhead = (from GridEXRow row in grdOverHead.GetDataRows()
				select (decimal)row.Cells["Amount"].Value).Sum();
			decimal sumOfContractorWages = (from GridEXRow row in grdContractorWages.GetDataRows()
				select (decimal)row.Cells["Amount"].Value).Sum();
			decimal netValue = num + sumOfOverhead + sumOfContractorWages;
			decimal totalVarientQty = grdOutPutByProduct.GetDataRows().Sum((GridEXRow row) => Conversion.ToDecimal(row.Cells["VarientQty"].Value));
			GridEXRow[] dataRows = grdOutPutByProduct.GetDataRows();
			foreach (GridEXRow obj in dataRows)
			{
				obj.BeginEdit();
				decimal totalCurrentRowVarientQty = Conversion.ToDecimal(obj.Cells["VarientQty"].Value);
				decimal currentRowAmount = ((totalVarientQty > 0m) ? (netValue / totalVarientQty * totalCurrentRowVarientQty) : 0m);
				obj.Cells["Amount"].Value = currentRowAmount;
				decimal currentRowRate = ((totalCurrentRowVarientQty > 0m) ? (currentRowAmount / totalCurrentRowVarientQty) : 0m);
				obj.Cells["Rate"].Value = currentRowRate;
				obj.EndEdit();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BtnUpdateWarehouseInput_Click(object sender, EventArgs e)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdInputMaterial.GetDataRows().Count() > 0)
			{
				GridEXRow rowsToValidate = grdOutPutByProduct.GetDataRows().FirstOrDefault();
				if (rowsToValidate != null && (int)rowsToValidate.RowType == 0 && rowsToValidate.Cells.Count > 0)
				{
					if (Conversion.ToInt(rowsToValidate.Cells["WarehouseId"].Value) == 0)
					{
						throw new Exception("Warehouse Field is Empty in Input Grid Row No: 01");
					}
					int wareId = 0;
					string wareName = "";
					(from r in grdInputMaterial.GetDataRows()
						where r.RowIndex == 0
						select r).FirstOrDefault();
					wareId = Conversion.ToInt(rowsToValidate.Cells["WarehouseId"].Value);
					wareName = Conversion.ToString(rowsToValidate.Cells["WarehouseName"].Value);
					GridEXRow[] dataRows = grdInputMaterial.GetDataRows();
					foreach (GridEXRow obj in dataRows)
					{
						obj.BeginEdit();
						obj.Cells["WarehouseId"].Value = wareId;
						obj.Cells["WarehouseName"].Value = wareName;
						obj.EndEdit();
					}
				}
				return;
			}
			throw new Exception("Input Grid Record not Found");
		}
		catch (Exception ex)
		{
			WF.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			WF.Close();
		}
	}

	private void BtnUpdateWarehouseOutPut_Click(object sender, EventArgs e)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdOutPutByProduct.GetDataRows().Count() > 0)
			{
				GridEXRow rowsToValidate = grdOutPutByProduct.GetDataRows().FirstOrDefault();
				if (rowsToValidate != null && (int)rowsToValidate.RowType == 0 && rowsToValidate.Cells.Count > 0)
				{
					if (Conversion.ToInt(rowsToValidate.Cells["WarehouseId"].Value) == 0)
					{
						throw new Exception("Warehouse Field is Empty in OutPut Grid Row No: 01");
					}
					int wareId = 0;
					string wareName = "";
					wareId = Conversion.ToInt(rowsToValidate.Cells["WarehouseId"].Value);
					wareName = Conversion.ToString(rowsToValidate.Cells["WarehouseName"].Value);
					GridEXRow[] dataRows = grdOutPutByProduct.GetDataRows();
					foreach (GridEXRow obj in dataRows)
					{
						obj.BeginEdit();
						obj.Cells["WarehouseId"].Value = wareId;
						obj.Cells["WarehouseName"].Value = wareName;
						obj.EndEdit();
					}
				}
				return;
			}
			throw new Exception("OutPut Grid Record not Found");
		}
		catch (Exception ex)
		{
			WF.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			WF.Close();
		}
	}

	private void BtnUpdateConctratorInWages_Click(object sender, EventArgs e)
	{
		try
		{
			if (grdContractorWages.GetDataRows().Count() > 0)
			{
				int ContrId = 0;
				string ContrName = string.Empty;
				string ActivtyName = string.Empty;
				if (((UltraGridBase)CmbContractorForUpdateInWagesGrid).ActiveRow != null && Conversion.ToInt(CmbContractorForUpdateInWagesGrid.Value) != 0)
				{
					ContrId = Conversion.ToInt(CmbContractorForUpdateInWagesGrid.Value);
					ContrName = Conversion.ToString((object)((Control)(object)CmbContractorForUpdateInWagesGrid).Text);
				}
				if (((UltraGridBase)CmbActivityforContractorUpdateInWagesGrid).ActiveRow != null && Conversion.ToInt(CmbActivityforContractorUpdateInWagesGrid.Value) != 0)
				{
					ActivtyName = Conversion.ToString((object)((Control)(object)CmbActivityforContractorUpdateInWagesGrid).Text);
				}
				GridEXRow[] dataRows = grdContractorWages.GetDataRows();
				foreach (GridEXRow r in dataRows)
				{
					if (ContrId == 0 && string.IsNullOrEmpty(ActivtyName))
					{
						if (Conversion.ToInt(r.Cells["ContractorId"].Value) == 0)
						{
							throw new Exception("ContractorName Field is Empty in Wages Grid Row No: 01");
						}
						ContrId = Conversion.ToInt(r.Cells["ContractorId"].Value);
						ContrName = Conversion.ToString(r.Cells["ContractorName"].Value);
					}
					if (!string.IsNullOrEmpty(ActivtyName) && Conversion.ToString(r.Cells["ServiceActivity"].Value) != ActivtyName)
					{
						continue;
					}
					r.BeginEdit();
					r.Cells["ContractorId"].Value = ((ContrId == 0) ? Conversion.ToInt(r.Cells["ContractorId"].Value) : ContrId);
					r.Cells["ContractorName"].Value = ContrName ?? Conversion.ToString(r.Cells["ContractorName"].Value);
					int ServiceActivityId = Conversion.ToInt(r.Cells["ContractorWagesRateScheduleId"].Value);
					if (ServiceActivityId > 0)
					{
						DataTable dt = ContractorWagesRateSchedule.GetServiceActivityByItemId(1, Conversion.ToInt(r.Cells["OutPutItemId"].Value), Conversion.ToDateTime((object)txtdocdate.Value), ServiceActivityId, ContrId);
						if (dt.Rows.Count > 0)
						{
							DataRow foundRow = dt.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("ContractorWagesRateScheduleId") == ServiceActivityId);
							decimal Qty = ((foundRow != null) ? Conversion.ToDecimal(foundRow["Qty"]) : 0m);
							decimal Rate = ((foundRow != null) ? Conversion.ToDecimal(foundRow["Rate"]) : 0m);
							decimal RatePerQty = ((Qty > 0m) ? (Rate / Qty) : 0m);
							r.Cells["Rate"].Value = RatePerQty;
							decimal CurrRowQty = Conversion.ToDecimal(r.Cells["Qty"].Value);
							r.Cells["Amount"].Value = RatePerQty * CurrRowQty;
						}
						else
						{
							r.Cells["Rate"].Value = 0;
							r.Cells["Amount"].Value = 0;
						}
					}
					else
					{
						r.Cells["Rate"].Value = 0;
						r.Cells["Amount"].Value = 0;
					}
					r.EndEdit();
				}
				if (ContrId == 0)
				{
					throw new Exception("ContractorName Field is Empty");
				}
				return;
			}
			throw new Exception("Wages Grid Record not Found");
		}
		catch (Exception ex)
		{
			WF.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			WF.Close();
		}
	}

	private void btnClearInput_Click(object sender, EventArgs e)
	{
		try
		{
			if (MessageBox.Show("are you sure to reset Input Detail?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				grdInputMaterial.ClearStructure();
				dtInput.Clear();
				AddRowInInputGrid();
				grdInputMaterial.RetrieveStructure();
				grdInputSettings();
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

	private void btnDefinePlant_Click(object sender, EventArgs e)
	{
		try
		{
			new DefineProductionPlant(UserAccount).Show();
			PlantFeader();
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
		//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Expected O, but got Unknown
		//IL_050f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Expected O, but got Unknown
		//IL_0551: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Expected O, but got Unknown
		//IL_0588: Unknown result type (might be due to invalid IL or missing references)
		//IL_0592: Expected O, but got Unknown
		//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05be: Expected O, but got Unknown
		//IL_05d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05df: Expected O, but got Unknown
		//IL_062d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0637: Expected O, but got Unknown
		//IL_0659: Unknown result type (might be due to invalid IL or missing references)
		//IL_0663: Expected O, but got Unknown
		//IL_069b: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a5: Expected O, but got Unknown
		//IL_06b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bb: Expected O, but got Unknown
		//IL_0709: Unknown result type (might be due to invalid IL or missing references)
		//IL_0713: Expected O, but got Unknown
		//IL_0714: Unknown result type (might be due to invalid IL or missing references)
		//IL_071e: Expected O, but got Unknown
		//IL_0806: Unknown result type (might be due to invalid IL or missing references)
		//IL_0810: Expected O, but got Unknown
		//IL_081c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0826: Expected O, but got Unknown
		//IL_0832: Unknown result type (might be due to invalid IL or missing references)
		//IL_083c: Expected O, but got Unknown
		//IL_0945: Unknown result type (might be due to invalid IL or missing references)
		//IL_094f: Expected O, but got Unknown
		//IL_0966: Unknown result type (might be due to invalid IL or missing references)
		//IL_0970: Expected O, but got Unknown
		//IL_0992: Unknown result type (might be due to invalid IL or missing references)
		//IL_099c: Expected O, but got Unknown
		//IL_09b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bd: Expected O, but got Unknown
		//IL_09d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09de: Expected O, but got Unknown
		//IL_0a4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a57: Expected O, but got Unknown
		//IL_0a63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6d: Expected O, but got Unknown
		//IL_1246: Unknown result type (might be due to invalid IL or missing references)
		//IL_1250: Expected O, but got Unknown
		//IL_125d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1267: Expected O, but got Unknown
		//IL_16be: Unknown result type (might be due to invalid IL or missing references)
		//IL_16c8: Expected O, but got Unknown
		//IL_16d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_16df: Expected O, but got Unknown
		//IL_16ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_16f6: Expected O, but got Unknown
		//IL_1b59: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b63: Expected O, but got Unknown
		//IL_1b70: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b7a: Expected O, but got Unknown
		//IL_29df: Unknown result type (might be due to invalid IL or missing references)
		//IL_29e9: Expected O, but got Unknown
		//IL_29f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a00: Expected O, but got Unknown
		//IL_83db: Unknown result type (might be due to invalid IL or missing references)
		//IL_83e5: Expected O, but got Unknown
		//IL_8409: Unknown result type (might be due to invalid IL or missing references)
		//IL_8413: Expected O, but got Unknown
		GridEXLayout grdInputMaterial_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOutPutByProduct_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdContractorWages_DesignTimeLayout = new GridEXLayout();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
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
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		GridEXLayout grdOverHead_DesignTimeLayout = new GridEXLayout();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		GridEXLayout grdHistoryOverHead_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryInput_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdWagesHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryOutput_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DatagridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
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
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel17 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.BtnUpdateWarehouseInput = new System.Windows.Forms.Button();
		this.grdInputMaterial = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label1 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.TabOutPut = new System.Windows.Forms.TabPage();
		this.grdOutPutByProduct = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.BtnUpdateWarehouseOutPut = new System.Windows.Forms.Button();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label23 = new System.Windows.Forms.Label();
		this.TabWages = new System.Windows.Forms.TabPage();
		this.grdContractorWages = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.PanelMessage = new System.Windows.Forms.Panel();
		this.ToolStripMessage = new System.Windows.Forms.ToolStrip();
		this.BtnMessage = new System.Windows.Forms.ToolStripButton();
		this.CmbContractorForUpdateInWagesGrid = new UltraCombo();
		this.BtnUpdateConctratorInWages = new System.Windows.Forms.Button();
		this.label66 = new System.Windows.Forms.Label();
		this.label65 = new System.Windows.Forms.Label();
		this.CmbActivityforContractorUpdateInWagesGrid = new UltraCombo();
		this.label33 = new System.Windows.Forms.Label();
		this.TabOverheads = new System.Windows.Forms.TabPage();
		this.grdOverHead = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label22 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.rdSearchByCode = new System.Windows.Forms.RadioButton();
		this.rdSearchByName = new System.Windows.Forms.RadioButton();
		this.BtnAddDetail = new System.Windows.Forms.Button();
		this.cmbItemBOM = new UltraCombo();
		this.BtnCancelDetail = new System.Windows.Forms.Button();
		this.BtnUpdateDetail = new System.Windows.Forms.Button();
		this.label7 = new System.Windows.Forms.Label();
		this.cmbUOM = new UltraCombo();
		this.label44 = new System.Windows.Forms.Label();
		this.txtVarientEquivalent = new System.Windows.Forms.TextBox();
		this.label43 = new System.Windows.Forms.Label();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.label18 = new System.Windows.Forms.Label();
		this.CmbAttribute = new UltraCombo();
		this.label42 = new System.Windows.Forms.Label();
		this.CmbWarehouseName = new UltraCombo();
		this.label41 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.txtProductionNo = new System.Windows.Forms.TextBox();
		this.btnDefinePlant = new System.Windows.Forms.Button();
		this.label37 = new System.Windows.Forms.Label();
		this.txtdocdate = new System.Windows.Forms.DateTimePicker();
		this.CmbProductionType = new UltraCombo();
		this.CmbPlantName = new UltraCombo();
		this.label26 = new System.Windows.Forms.Label();
		this.txtRemarksHeader = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.txtOutPutAutoRatePerQtyAmount = new System.Windows.Forms.TextBox();
		this.txtTotalOutPutAutoRatedAmount = new System.Windows.Forms.TextBox();
		this.txtTotalInputs = new System.Windows.Forms.TextBox();
		this.txtWagesAmount = new System.Windows.Forms.TextBox();
		this.txtOverHead = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.panel20 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.comCompany = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.combranches = new UltraCombo();
		this.label45 = new System.Windows.Forms.Label();
		this.comproject = new UltraCombo();
		this.label46 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnVoucherReport = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnGenerateRatesOutput = new System.Windows.Forms.ToolStripButton();
		this.btnClearInput = new System.Windows.Forms.Button();
		this.label21 = new System.Windows.Forms.Label();
		this.chkPrintVoucher = new System.Windows.Forms.CheckBox();
		this.ChkPreviewSlip = new System.Windows.Forms.CheckBox();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel45 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdHistoryOverHead = new GridEX();
		this.panel42 = new System.Windows.Forms.Panel();
		this.label34 = new System.Windows.Forms.Label();
		this.grdHistoryInput = new GridEX();
		this.panel44 = new System.Windows.Forms.Panel();
		this.label14 = new System.Windows.Forms.Label();
		this.panel19 = new System.Windows.Forms.Panel();
		this.grdWagesHistory = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.label40 = new System.Windows.Forms.Label();
		this.grdHistoryOutput = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.DatagridHistory = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label17 = new System.Windows.Forms.Label();
		this.panel13 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.RadNotReffered = new System.Windows.Forms.RadioButton();
		this.RadReffered = new System.Windows.Forms.RadioButton();
		this.RadAll = new System.Windows.Forms.RadioButton();
		this.btnshow = new System.Windows.Forms.Button();
		this.label30 = new System.Windows.Forms.Label();
		this.cmbConversionNoHistory = new UltraCombo();
		this.label19 = new System.Windows.Forms.Label();
		this.cmbDateTypeHistory = new UltraCombo();
		this.txtToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label28 = new System.Windows.Forms.Label();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromdateHistory = new System.Windows.Forms.DateTimePicker();
		this.label35 = new System.Windows.Forms.Label();
		this.txtFromNoHistory = new System.Windows.Forms.TextBox();
		this.panel38 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.label32 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel17.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInputMaterial).BeginInit();
		this.panel7.SuspendLayout();
		this.panel10.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.TabOutPut.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOutPutByProduct).BeginInit();
		this.panel11.SuspendLayout();
		this.TabWages.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdContractorWages).BeginInit();
		this.panel5.SuspendLayout();
		this.PanelMessage.SuspendLayout();
		this.ToolStripMessage.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbContractorForUpdateInWagesGrid).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbActivityforContractorUpdateInWagesGrid).BeginInit();
		this.TabOverheads.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOverHead).BeginInit();
		this.panel14.SuspendLayout();
		this.panel2.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemBOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAttribute).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseName).BeginInit();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantName).BeginInit();
		this.panel4.SuspendLayout();
		this.panel20.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.comCompany).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel45.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryOverHead).BeginInit();
		this.panel42.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryInput).BeginInit();
		this.panel44.SuspendLayout();
		this.panel19.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdWagesHistory).BeginInit();
		this.panel18.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryOutput).BeginInit();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).BeginInit();
		this.panel15.SuspendLayout();
		this.panel13.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbConversionNoHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).BeginInit();
		this.panel38.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1197, 711);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1197, 711);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel17);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1189, 682);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel17.Controls.Add(this.panel6);
		this.panel17.Controls.Add(this.panel10);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel17.Location = new System.Drawing.Point(3, 226);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1183, 453);
		this.panel17.TabIndex = 4;
		this.panel6.Controls.Add(this.BtnUpdateWarehouseInput);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdInputMaterial);
		this.panel6.Controls.Add(this.panel7);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(765, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(418, 453);
		this.panel6.TabIndex = 4;
		this.BtnUpdateWarehouseInput.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnUpdateWarehouseInput.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnUpdateWarehouseInput.FlatAppearance.BorderSize = 0;
		this.BtnUpdateWarehouseInput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateWarehouseInput.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Bold);
		this.BtnUpdateWarehouseInput.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateWarehouseInput.Location = new System.Drawing.Point(189, 3);
		this.BtnUpdateWarehouseInput.Name = "BtnUpdateWarehouseInput";
		this.BtnUpdateWarehouseInput.Size = new System.Drawing.Size(180, 21);
		this.BtnUpdateWarehouseInput.TabIndex = 9;
		this.BtnUpdateWarehouseInput.Text = "Update Same WareHouse In All Rows";
		this.BtnUpdateWarehouseInput.UseVisualStyleBackColor = false;
		this.BtnUpdateWarehouseInput.Click += new System.EventHandler(BtnUpdateWarehouseInput_Click);
		((ScrollableBase)this.grdInputMaterial).BackColor = System.Drawing.Color.White;
		grdInputMaterial_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInputMaterial.DesignTimeLayout = grdInputMaterial_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInputMaterial.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).Name = "grdInputMaterial";
		((ScrollableBase)this.grdInputMaterial).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).Size = new System.Drawing.Size(418, 427);
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).TabIndex = 0;
		this.grdInputMaterial.TotalRow = (InheritableBoolean)1;
		this.grdInputMaterial.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInputMaterial).VisualStyle = (VisualStyle)4;
		this.grdInputMaterial.CellUpdated += new ColumnActionEventHandler(grdInputMaterial_CellUpdated);
		this.grdInputMaterial.ColumnButtonClick += new ColumnActionEventHandler(grdInputMaterial_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).KeyDown += new System.Windows.Forms.KeyEventHandler(grdInputMaterial_KeyDown);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.ctrlGrdBar2);
		this.panel7.Controls.Add(this.label1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(418, 26);
		this.panel7.TabIndex = 3;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(370, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdInputMaterial;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(46, 25);
		this.ctrlGrdBar2.TabIndex = 127;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 8.75f, System.Drawing.FontStyle.Bold);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(5, 6);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(212, 14);
		this.label1.TabIndex = 126;
		this.label1.Text = "Input Material (Consumed Items)";
		this.panel10.Controls.Add(this.tabControl2);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(765, 453);
		this.panel10.TabIndex = 3;
		this.tabControl2.Controls.Add(this.TabOutPut);
		this.tabControl2.Controls.Add(this.TabWages);
		this.tabControl2.Controls.Add(this.TabOverheads);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(765, 453);
		this.tabControl2.TabIndex = 0;
		this.TabOutPut.Controls.Add((System.Windows.Forms.Control)(object)this.grdOutPutByProduct);
		this.TabOutPut.Controls.Add(this.panel11);
		this.TabOutPut.Location = new System.Drawing.Point(4, 25);
		this.TabOutPut.Name = "TabOutPut";
		this.TabOutPut.Padding = new System.Windows.Forms.Padding(3);
		this.TabOutPut.Size = new System.Drawing.Size(757, 424);
		this.TabOutPut.TabIndex = 0;
		this.TabOutPut.Text = "OutPut (Produced Items)";
		this.TabOutPut.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdOutPutByProduct).BackColor = System.Drawing.Color.White;
		grdOutPutByProduct_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOutPutByProduct.DesignTimeLayout = grdOutPutByProduct_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOutPutByProduct).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdOutPutByProduct).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOutPutByProduct.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOutPutByProduct).Location = new System.Drawing.Point(3, 30);
		((System.Windows.Forms.Control)(object)this.grdOutPutByProduct).Name = "grdOutPutByProduct";
		((ScrollableBase)this.grdOutPutByProduct).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOutPutByProduct).Size = new System.Drawing.Size(751, 391);
		((System.Windows.Forms.Control)(object)this.grdOutPutByProduct).TabIndex = 0;
		this.grdOutPutByProduct.TotalRow = (InheritableBoolean)1;
		this.grdOutPutByProduct.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOutPutByProduct).VisualStyle = (VisualStyle)4;
		this.grdOutPutByProduct.CellValueChanged += new ColumnActionEventHandler(grdOutPutByProduct_CellValueChanged);
		this.grdOutPutByProduct.CellUpdated += new ColumnActionEventHandler(grdOutPutByProduct_CellUpdated);
		this.grdOutPutByProduct.ColumnButtonClick += new ColumnActionEventHandler(grdOutPutByProduct_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdOutPutByProduct).DoubleClick += new System.EventHandler(grdOutPutByProduct_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdOutPutByProduct).KeyDown += new System.Windows.Forms.KeyEventHandler(grdOutPutByProduct_KeyDown);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.BtnUpdateWarehouseOutPut);
		this.panel11.Controls.Add(this.ctrlGrdBar3);
		this.panel11.Controls.Add(this.label23);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(3, 3);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(751, 27);
		this.panel11.TabIndex = 9;
		this.BtnUpdateWarehouseOutPut.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnUpdateWarehouseOutPut.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnUpdateWarehouseOutPut.FlatAppearance.BorderSize = 0;
		this.BtnUpdateWarehouseOutPut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateWarehouseOutPut.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnUpdateWarehouseOutPut.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateWarehouseOutPut.Location = new System.Drawing.Point(515, 2);
		this.BtnUpdateWarehouseOutPut.Name = "BtnUpdateWarehouseOutPut";
		this.BtnUpdateWarehouseOutPut.Size = new System.Drawing.Size(185, 24);
		this.BtnUpdateWarehouseOutPut.TabIndex = 9;
		this.BtnUpdateWarehouseOutPut.Text = "Update Same WareHouse In All Rows";
		this.BtnUpdateWarehouseOutPut.UseVisualStyleBackColor = false;
		this.BtnUpdateWarehouseOutPut.Click += new System.EventHandler(BtnUpdateWarehouseOutPut_Click);
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(703, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar3.MyGrid = this.grdOutPutByProduct;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(45, 25);
		this.ctrlGrdBar3.TabIndex = 127;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(12, 5);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(170, 16);
		this.label23.TabIndex = 126;
		this.label23.Text = "OutPut (Produced Items)";
		this.TabWages.Controls.Add((System.Windows.Forms.Control)(object)this.grdContractorWages);
		this.TabWages.Controls.Add(this.panel5);
		this.TabWages.Location = new System.Drawing.Point(4, 25);
		this.TabWages.Name = "TabWages";
		this.TabWages.Padding = new System.Windows.Forms.Padding(3);
		this.TabWages.Size = new System.Drawing.Size(757, 424);
		this.TabWages.TabIndex = 1;
		this.TabWages.Text = "Contractor Wages";
		this.TabWages.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdContractorWages).BackColor = System.Drawing.Color.White;
		grdContractorWages_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdContractorWages.DesignTimeLayout = grdContractorWages_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdContractorWages.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Location = new System.Drawing.Point(3, 34);
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Name = "grdContractorWages";
		((ScrollableBase)this.grdContractorWages).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Size = new System.Drawing.Size(751, 387);
		((System.Windows.Forms.Control)(object)this.grdContractorWages).TabIndex = 2;
		this.grdContractorWages.TotalRow = (InheritableBoolean)1;
		this.grdContractorWages.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdContractorWages).VisualStyle = (VisualStyle)4;
		this.grdContractorWages.CellUpdated += new ColumnActionEventHandler(grdContractorWages_CellUpdated);
		this.grdContractorWages.ColumnButtonClick += new ColumnActionEventHandler(grdContractorWages_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdContractorWages).KeyDown += new System.Windows.Forms.KeyEventHandler(grdContractorWages_KeyDown);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.PanelMessage);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbContractorForUpdateInWagesGrid);
		this.panel5.Controls.Add(this.BtnUpdateConctratorInWages);
		this.panel5.Controls.Add(this.label66);
		this.panel5.Controls.Add(this.label65);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbActivityforContractorUpdateInWagesGrid);
		this.panel5.Controls.Add(this.label33);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(751, 31);
		this.panel5.TabIndex = 13;
		this.PanelMessage.BackColor = System.Drawing.Color.White;
		this.PanelMessage.Controls.Add(this.ToolStripMessage);
		this.PanelMessage.Location = new System.Drawing.Point(199, 5);
		this.PanelMessage.Name = "PanelMessage";
		this.PanelMessage.Size = new System.Drawing.Size(25, 23);
		this.PanelMessage.TabIndex = 127;
		this.ToolStripMessage.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.ToolStripMessage.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.BtnMessage });
		this.ToolStripMessage.Location = new System.Drawing.Point(0, 0);
		this.ToolStripMessage.Name = "ToolStripMessage";
		this.ToolStripMessage.Size = new System.Drawing.Size(25, 25);
		this.ToolStripMessage.TabIndex = 0;
		this.ToolStripMessage.Text = "toolStrip3";
		this.BtnMessage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.BtnMessage.Image = Architecture.WinApp.Properties.Resources.info_i;
		this.BtnMessage.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnMessage.Name = "BtnMessage";
		this.BtnMessage.Size = new System.Drawing.Size(23, 22);
		this.BtnMessage.ToolTipText = "Click to update the selected contractor for the selected activity or the same contractor (as in the 1st row) when both combos are empty, in all rows of grid";
		this.CmbContractorForUpdateInWagesGrid.AllowNull = (DefaultableBoolean)1;
		this.CmbContractorForUpdateInWagesGrid.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbContractorForUpdateInWagesGrid.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbContractorForUpdateInWagesGrid.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance80;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbContractorForUpdateInWagesGrid).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbContractorForUpdateInWagesGrid.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbContractorForUpdateInWagesGrid).Location = new System.Drawing.Point(501, 2);
		((System.Windows.Forms.Control)(object)this.CmbContractorForUpdateInWagesGrid).Name = "CmbContractorForUpdateInWagesGrid";
		((System.Windows.Forms.Control)(object)this.CmbContractorForUpdateInWagesGrid).Size = new System.Drawing.Size(173, 26);
		((System.Windows.Forms.Control)(object)this.CmbContractorForUpdateInWagesGrid).TabIndex = 157;
		this.BtnUpdateConctratorInWages.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnUpdateConctratorInWages.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnUpdateConctratorInWages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateConctratorInWages.Font = new System.Drawing.Font("Verdana", 9.5f, System.Drawing.FontStyle.Bold);
		this.BtnUpdateConctratorInWages.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateConctratorInWages.Location = new System.Drawing.Point(677, 2);
		this.BtnUpdateConctratorInWages.Name = "BtnUpdateConctratorInWages";
		this.BtnUpdateConctratorInWages.Size = new System.Drawing.Size(72, 26);
		this.BtnUpdateConctratorInWages.TabIndex = 158;
		this.BtnUpdateConctratorInWages.Text = "Update";
		this.BtnUpdateConctratorInWages.UseVisualStyleBackColor = false;
		this.BtnUpdateConctratorInWages.Click += new System.EventHandler(BtnUpdateConctratorInWages_Click);
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label66.Location = new System.Drawing.Point(422, 6);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(81, 19);
		this.label66.TabIndex = 160;
		this.label66.Text = "Contractor";
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label65.Location = new System.Drawing.Point(227, 6);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(60, 19);
		this.label65.TabIndex = 161;
		this.label65.Text = "Activity";
		this.CmbActivityforContractorUpdateInWagesGrid.AllowNull = (DefaultableBoolean)1;
		this.CmbActivityforContractorUpdateInWagesGrid.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbActivityforContractorUpdateInWagesGrid.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbActivityforContractorUpdateInWagesGrid.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Appearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance93;
		((SpecialBoxBase)((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance98).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance99).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance99).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance99).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance100).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbActivityforContractorUpdateInWagesGrid).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbActivityforContractorUpdateInWagesGrid.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbActivityforContractorUpdateInWagesGrid).Location = new System.Drawing.Point(288, 2);
		((System.Windows.Forms.Control)(object)this.CmbActivityforContractorUpdateInWagesGrid).Name = "CmbActivityforContractorUpdateInWagesGrid";
		((System.Windows.Forms.Control)(object)this.CmbActivityforContractorUpdateInWagesGrid).Size = new System.Drawing.Size(132, 26);
		((System.Windows.Forms.Control)(object)this.CmbActivityforContractorUpdateInWagesGrid).TabIndex = 156;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.ForeColor = System.Drawing.Color.White;
		this.label33.Location = new System.Drawing.Point(3, 7);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(130, 16);
		this.label33.TabIndex = 126;
		this.label33.Text = "Contractor Wages ";
		this.TabOverheads.Controls.Add((System.Windows.Forms.Control)(object)this.grdOverHead);
		this.TabOverheads.Controls.Add(this.panel14);
		this.TabOverheads.Location = new System.Drawing.Point(4, 25);
		this.TabOverheads.Name = "TabOverheads";
		this.TabOverheads.Size = new System.Drawing.Size(757, 424);
		this.TabOverheads.TabIndex = 2;
		this.TabOverheads.Text = "Over Heads";
		this.TabOverheads.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdOverHead).BackColor = System.Drawing.Color.White;
		grdOverHead_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOverHead.DesignTimeLayout = grdOverHead_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOverHead.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grdOverHead).Name = "grdOverHead";
		((ScrollableBase)this.grdOverHead).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Size = new System.Drawing.Size(757, 402);
		((System.Windows.Forms.Control)(object)this.grdOverHead).TabIndex = 1;
		this.grdOverHead.TotalRow = (InheritableBoolean)1;
		this.grdOverHead.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOverHead).VisualStyle = (VisualStyle)4;
		this.grdOverHead.CellUpdated += new ColumnActionEventHandler(grdOverHead_CellUpdated);
		this.grdOverHead.ColumnButtonClick += new ColumnActionEventHandler(grdOverHead_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdOverHead).KeyDown += new System.Windows.Forms.KeyEventHandler(grdOverHead_KeyDown);
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.label22);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(757, 22);
		this.panel14.TabIndex = 12;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.White;
		this.label22.Location = new System.Drawing.Point(3, 3);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(71, 16);
		this.label22.TabIndex = 126;
		this.label22.Text = "OverHead";
		this.panel2.Controls.Add(this.groupBox3);
		this.panel2.Controls.Add(this.groupBox2);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.comCompany);
		this.panel2.Controls.Add(this.label4);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.combranches);
		this.panel2.Controls.Add(this.label45);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.comproject);
		this.panel2.Controls.Add(this.label46);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 58);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1183, 168);
		this.panel2.TabIndex = 0;
		this.groupBox3.Controls.Add(this.rdSearchByCode);
		this.groupBox3.Controls.Add(this.rdSearchByName);
		this.groupBox3.Controls.Add(this.BtnAddDetail);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemBOM);
		this.groupBox3.Controls.Add(this.BtnCancelDetail);
		this.groupBox3.Controls.Add(this.BtnUpdateDetail);
		this.groupBox3.Controls.Add(this.label7);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbUOM);
		this.groupBox3.Controls.Add(this.label44);
		this.groupBox3.Controls.Add(this.txtVarientEquivalent);
		this.groupBox3.Controls.Add(this.label43);
		this.groupBox3.Controls.Add(this.txtQty);
		this.groupBox3.Controls.Add(this.label18);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAttribute);
		this.groupBox3.Controls.Add(this.label42);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouseName);
		this.groupBox3.Controls.Add(this.label41);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.groupBox3.Location = new System.Drawing.Point(4, 76);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(824, 68);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Detail";
		this.rdSearchByCode.AutoSize = true;
		this.rdSearchByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByCode.Location = new System.Drawing.Point(302, 15);
		this.rdSearchByCode.Name = "rdSearchByCode";
		this.rdSearchByCode.Size = new System.Drawing.Size(52, 19);
		this.rdSearchByCode.TabIndex = 91;
		this.rdSearchByCode.Text = "Code";
		this.rdSearchByCode.UseVisualStyleBackColor = true;
		this.rdSearchByName.AutoSize = true;
		this.rdSearchByName.Checked = true;
		this.rdSearchByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByName.Location = new System.Drawing.Point(247, 15);
		this.rdSearchByName.Name = "rdSearchByName";
		this.rdSearchByName.Size = new System.Drawing.Size(57, 19);
		this.rdSearchByName.TabIndex = 90;
		this.rdSearchByName.TabStop = true;
		this.rdSearchByName.Text = "Name";
		this.rdSearchByName.UseVisualStyleBackColor = true;
		this.rdSearchByName.CheckedChanged += new System.EventHandler(rdSearchByName_CheckedChanged);
		this.BtnAddDetail.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnAddDetail.FlatAppearance.BorderSize = 0;
		this.BtnAddDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAddDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnAddDetail.ForeColor = System.Drawing.Color.White;
		this.BtnAddDetail.Location = new System.Drawing.Point(754, 36);
		this.BtnAddDetail.Name = "BtnAddDetail";
		this.BtnAddDetail.Size = new System.Drawing.Size(35, 23);
		this.BtnAddDetail.TabIndex = 6;
		this.BtnAddDetail.Text = "+";
		this.BtnAddDetail.UseVisualStyleBackColor = false;
		this.BtnAddDetail.Click += new System.EventHandler(BtnAddDetail_Click);
		this.cmbItemBOM.AllowNull = (DefaultableBoolean)1;
		this.cmbItemBOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemBOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemBOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemBOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemBOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemBOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemBOM).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemBOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemBOM).Location = new System.Drawing.Point(174, 34);
		((System.Windows.Forms.Control)(object)this.cmbItemBOM).Name = "cmbItemBOM";
		((System.Windows.Forms.Control)(object)this.cmbItemBOM).Size = new System.Drawing.Size(206, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemBOM).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbItemBOM).Leave += new System.EventHandler(cmbItem_Leave);
		this.BtnCancelDetail.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnCancelDetail.FlatAppearance.BorderSize = 0;
		this.BtnCancelDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancelDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnCancelDetail.ForeColor = System.Drawing.Color.White;
		this.BtnCancelDetail.Location = new System.Drawing.Point(754, 12);
		this.BtnCancelDetail.Name = "BtnCancelDetail";
		this.BtnCancelDetail.Size = new System.Drawing.Size(69, 20);
		this.BtnCancelDetail.TabIndex = 8;
		this.BtnCancelDetail.Text = "Cancel";
		this.BtnCancelDetail.UseVisualStyleBackColor = false;
		this.BtnCancelDetail.Visible = false;
		this.BtnCancelDetail.Click += new System.EventHandler(BtnCancelDetail_Click);
		this.BtnUpdateDetail.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.BtnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateDetail.Location = new System.Drawing.Point(754, 36);
		this.BtnUpdateDetail.Name = "BtnUpdateDetail";
		this.BtnUpdateDetail.Size = new System.Drawing.Size(69, 23);
		this.BtnUpdateDetail.TabIndex = 7;
		this.BtnUpdateDetail.Text = "Update";
		this.BtnUpdateDetail.UseVisualStyleBackColor = false;
		this.BtnUpdateDetail.Visible = false;
		this.BtnUpdateDetail.Click += new System.EventHandler(BtnUpdateDetail_Click);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(174, 17);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(67, 15);
		this.label7.TabIndex = 85;
		this.label7.Text = "Item Name";
		this.cmbUOM.AllowNull = (DefaultableBoolean)1;
		this.cmbUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Location = new System.Drawing.Point(383, 34);
		((System.Windows.Forms.Control)(object)this.cmbUOM).Name = "cmbUOM";
		((System.Windows.Forms.Control)(object)this.cmbUOM).Size = new System.Drawing.Size(85, 26);
		((System.Windows.Forms.Control)(object)this.cmbUOM).TabIndex = 2;
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(383, 17);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(35, 15);
		this.label44.TabIndex = 87;
		this.label44.Text = "UOM";
		this.txtVarientEquivalent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVarientEquivalent.Enabled = false;
		this.txtVarientEquivalent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVarientEquivalent.Location = new System.Drawing.Point(671, 36);
		this.txtVarientEquivalent.Name = "txtVarientEquivalent";
		this.txtVarientEquivalent.Size = new System.Drawing.Size(80, 23);
		this.txtVarientEquivalent.TabIndex = 5;
		this.txtVarientEquivalent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtVarientEquivalent.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQty_KeyPress);
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(671, 17);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(62, 15);
		this.label43.TabIndex = 89;
		this.label43.Text = "Equivalent";
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQty.Location = new System.Drawing.Point(470, 36);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(81, 23);
		this.txtQty.TabIndex = 3;
		this.txtQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQty_KeyPress);
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(470, 17);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(26, 15);
		this.label18.TabIndex = 89;
		this.label18.Text = "Qty";
		this.CmbAttribute.AllowNull = (DefaultableBoolean)1;
		this.CmbAttribute.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAttribute.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAttribute.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Appearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAttribute).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance40;
		((SpecialBoxBase)((UltraGridBase)this.CmbAttribute).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance45).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAttribute).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAttribute).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAttribute.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAttribute).Location = new System.Drawing.Point(553, 34);
		((System.Windows.Forms.Control)(object)this.CmbAttribute).Name = "CmbAttribute";
		((System.Windows.Forms.Control)(object)this.CmbAttribute).Size = new System.Drawing.Size(116, 26);
		((System.Windows.Forms.Control)(object)this.CmbAttribute).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbAttribute).Leave += new System.EventHandler(CmbAttribute_Leave);
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(553, 17);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(54, 15);
		this.label42.TabIndex = 85;
		this.label42.Text = "Attribute";
		this.CmbWarehouseName.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehouseName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouseName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouseName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance106;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance111).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouseName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWarehouseName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseName).Location = new System.Drawing.Point(5, 34);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseName).Name = "CmbWarehouseName";
		((System.Windows.Forms.Control)(object)this.CmbWarehouseName).Size = new System.Drawing.Size(167, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseName).TabIndex = 0;
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(5, 17);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(102, 15);
		this.label41.TabIndex = 85;
		this.label41.Text = "Warehouse Name";
		this.groupBox2.Controls.Add(this.txtdocno);
		this.groupBox2.Controls.Add(this.txtProductionNo);
		this.groupBox2.Controls.Add(this.btnDefinePlant);
		this.groupBox2.Controls.Add(this.label37);
		this.groupBox2.Controls.Add(this.txtdocdate);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbProductionType);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPlantName);
		this.groupBox2.Controls.Add(this.label26);
		this.groupBox2.Controls.Add(this.txtRemarksHeader);
		this.groupBox2.Controls.Add(this.label16);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(4, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(824, 72);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main";
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(89, 16);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(62, 23);
		this.txtdocno.TabIndex = 0;
		this.txtProductionNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtProductionNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtProductionNo.Location = new System.Drawing.Point(89, 44);
		this.txtProductionNo.Name = "txtProductionNo";
		this.txtProductionNo.Size = new System.Drawing.Size(185, 23);
		this.txtProductionNo.TabIndex = 2;
		this.btnDefinePlant.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnDefinePlant.Location = new System.Drawing.Point(512, 42);
		this.btnDefinePlant.Name = "btnDefinePlant";
		this.btnDefinePlant.Size = new System.Drawing.Size(28, 26);
		this.btnDefinePlant.TabIndex = 5;
		this.btnDefinePlant.TabStop = false;
		this.btnDefinePlant.UseVisualStyleBackColor = true;
		this.btnDefinePlant.Click += new System.EventHandler(btnDefinePlant_Click);
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(1, 20);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(88, 15);
		this.label37.TabIndex = 138;
		this.label37.Text = "Doc No && Date";
		this.txtdocdate.CustomFormat = "dd-MMM-yy";
		this.txtdocdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdocdate.Location = new System.Drawing.Point(152, 16);
		this.txtdocdate.Name = "txtdocdate";
		this.txtdocdate.Size = new System.Drawing.Size(122, 23);
		this.txtdocdate.TabIndex = 1;
		this.CmbProductionType.AllowNull = (DefaultableBoolean)1;
		this.CmbProductionType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbProductionType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbProductionType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance147).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance153).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbProductionType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Location = new System.Drawing.Point(373, 14);
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Name = "CmbProductionType";
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Size = new System.Drawing.Size(167, 26);
		((System.Windows.Forms.Control)(object)this.CmbProductionType).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Leave += new System.EventHandler(CmbProductionType_Leave);
		this.CmbPlantName.AllowNull = (DefaultableBoolean)1;
		this.CmbPlantName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPlantName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPlantName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance51).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance51).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance52;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance57).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance58).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPlantName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPlantName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPlantName).Location = new System.Drawing.Point(373, 42);
		((System.Windows.Forms.Control)(object)this.CmbPlantName).Name = "CmbPlantName";
		((System.Windows.Forms.Control)(object)this.CmbPlantName).Size = new System.Drawing.Size(138, 26);
		((System.Windows.Forms.Control)(object)this.CmbPlantName).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbPlantName).Leave += new System.EventHandler(CmbPlantName_Leave);
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(278, 48);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(69, 15);
		this.label26.TabIndex = 85;
		this.label26.Text = "Plant Name";
		this.txtRemarksHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksHeader.Location = new System.Drawing.Point(596, 14);
		this.txtRemarksHeader.Multiline = true;
		this.txtRemarksHeader.Name = "txtRemarksHeader";
		this.txtRemarksHeader.Size = new System.Drawing.Size(224, 54);
		this.txtRemarksHeader.TabIndex = 6;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(278, 20);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(94, 15);
		this.label16.TabIndex = 89;
		this.label16.Text = "Conversion Type";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(1, 48);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(85, 15);
		this.label8.TabIndex = 89;
		this.label8.Text = "Conversion No";
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.White;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(544, 20);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(52, 15);
		this.label2.TabIndex = 87;
		this.label2.Text = "Remarks";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel4.Controls.Add(this.txtOutPutAutoRatePerQtyAmount);
		this.panel4.Controls.Add(this.txtTotalOutPutAutoRatedAmount);
		this.panel4.Controls.Add(this.txtTotalInputs);
		this.panel4.Controls.Add(this.txtWagesAmount);
		this.panel4.Controls.Add(this.txtOverHead);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.label31);
		this.panel4.Controls.Add(this.label10);
		this.panel4.Controls.Add(this.panel20);
		this.panel4.Controls.Add(this.label13);
		this.panel4.Controls.Add(this.label9);
		this.panel4.Controls.Add(this.label25);
		this.panel4.Controls.Add(this.label24);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Location = new System.Drawing.Point(830, 1);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(349, 165);
		this.panel4.TabIndex = 2;
		this.txtOutPutAutoRatePerQtyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOutPutAutoRatePerQtyAmount.Enabled = false;
		this.txtOutPutAutoRatePerQtyAmount.Font = new System.Drawing.Font("Verdana", 10.5f, System.Drawing.FontStyle.Bold);
		this.txtOutPutAutoRatePerQtyAmount.Location = new System.Drawing.Point(195, 135);
		this.txtOutPutAutoRatePerQtyAmount.Name = "txtOutPutAutoRatePerQtyAmount";
		this.txtOutPutAutoRatePerQtyAmount.ReadOnly = true;
		this.txtOutPutAutoRatePerQtyAmount.Size = new System.Drawing.Size(149, 25);
		this.txtOutPutAutoRatePerQtyAmount.TabIndex = 5;
		this.txtOutPutAutoRatePerQtyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTotalOutPutAutoRatedAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalOutPutAutoRatedAmount.Enabled = false;
		this.txtTotalOutPutAutoRatedAmount.Font = new System.Drawing.Font("Verdana", 10.5f, System.Drawing.FontStyle.Bold);
		this.txtTotalOutPutAutoRatedAmount.Location = new System.Drawing.Point(195, 108);
		this.txtTotalOutPutAutoRatedAmount.Name = "txtTotalOutPutAutoRatedAmount";
		this.txtTotalOutPutAutoRatedAmount.ReadOnly = true;
		this.txtTotalOutPutAutoRatedAmount.Size = new System.Drawing.Size(149, 25);
		this.txtTotalOutPutAutoRatedAmount.TabIndex = 4;
		this.txtTotalOutPutAutoRatedAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTotalInputs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalInputs.Enabled = false;
		this.txtTotalInputs.Font = new System.Drawing.Font("Verdana", 10.5f, System.Drawing.FontStyle.Bold);
		this.txtTotalInputs.Location = new System.Drawing.Point(195, 25);
		this.txtTotalInputs.Name = "txtTotalInputs";
		this.txtTotalInputs.ReadOnly = true;
		this.txtTotalInputs.Size = new System.Drawing.Size(149, 25);
		this.txtTotalInputs.TabIndex = 0;
		this.txtTotalInputs.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtWagesAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWagesAmount.Enabled = false;
		this.txtWagesAmount.Font = new System.Drawing.Font("Verdana", 10.5f, System.Drawing.FontStyle.Bold);
		this.txtWagesAmount.Location = new System.Drawing.Point(195, 79);
		this.txtWagesAmount.Name = "txtWagesAmount";
		this.txtWagesAmount.ReadOnly = true;
		this.txtWagesAmount.Size = new System.Drawing.Size(149, 25);
		this.txtWagesAmount.TabIndex = 2;
		this.txtWagesAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOverHead.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOverHead.Enabled = false;
		this.txtOverHead.Font = new System.Drawing.Font("Verdana", 10.5f, System.Drawing.FontStyle.Bold);
		this.txtOverHead.Location = new System.Drawing.Point(195, 52);
		this.txtOverHead.Name = "txtOverHead";
		this.txtOverHead.ReadOnly = true;
		this.txtOverHead.Size = new System.Drawing.Size(149, 25);
		this.txtOverHead.TabIndex = 1;
		this.txtOverHead.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(3, 30);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(195, 17);
		this.label6.TabIndex = 91;
		this.label6.Text = "Total Inputs (Consumed Items)";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(3, 138);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(177, 17);
		this.label31.TabIndex = 97;
		this.label31.Text = "(Produced Item) Value / Qty";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(3, 111);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(175, 17);
		this.label10.TabIndex = 97;
		this.label10.Text = "Total (Produced Item) Value";
		this.panel20.BackColor = System.Drawing.Color.Teal;
		this.panel20.Controls.Add(this.label5);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(0, 0);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(347, 23);
		this.panel20.TabIndex = 1;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(5, 4);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(141, 16);
		this.label5.TabIndex = 33;
		this.label5.Text = "Calculation Summery";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(3, 83);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(117, 17);
		this.label13.TabIndex = 95;
		this.label13.Text = "Total Wages Value";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(3, 56);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(148, 17);
		this.label9.TabIndex = 95;
		this.label9.Text = "Total Over Heads Value";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(115, 96);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(15, 16);
		this.label25.TabIndex = 93;
		this.label25.Text = "=";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(115, 71);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(15, 16);
		this.label24.TabIndex = 93;
		this.label24.Text = "+";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(115, 42);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(15, 16);
		this.label3.TabIndex = 93;
		this.label3.Text = "+";
		this.comCompany.AllowNull = (DefaultableBoolean)1;
		this.comCompany.AutoCompleteMode = (AutoCompleteMode)4;
		this.comCompany.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comCompany.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comCompany).DisplayLayout.Appearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.comCompany).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comCompany).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comCompany).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comCompany).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance116;
		((SpecialBoxBase)((UltraGridBase)this.comCompany).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comCompany).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.comCompany).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comCompany).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance121).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).TextHAlignAsString = "Left";
		((UltraGridBase)this.comCompany).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.comCompany).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comCompany).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comCompany).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comCompany).Enabled = false;
		((System.Windows.Forms.Control)(object)this.comCompany).Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comCompany.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comCompany).Location = new System.Drawing.Point(1343, 18);
		((System.Windows.Forms.Control)(object)this.comCompany).Name = "comCompany";
		((System.Windows.Forms.Control)(object)this.comCompany).Size = new System.Drawing.Size(58, 27);
		((System.Windows.Forms.Control)(object)this.comCompany).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.comCompany).Visible = false;
		this.label4.AutoSize = true;
		this.label4.Enabled = false;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(1273, 23);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(63, 17);
		this.label4.TabIndex = 126;
		this.label4.Text = "Company";
		this.label4.Visible = false;
		this.combranches.AllowNull = (DefaultableBoolean)1;
		this.combranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.combranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.combranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance129;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.combranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance134).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance135).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance135).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance135).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance137).TextHAlignAsString = "Left";
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combranches).Enabled = false;
		((System.Windows.Forms.Control)(object)this.combranches).Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combranches).Location = new System.Drawing.Point(1343, 48);
		((System.Windows.Forms.Control)(object)this.combranches).Name = "combranches";
		((System.Windows.Forms.Control)(object)this.combranches).Size = new System.Drawing.Size(58, 27);
		((System.Windows.Forms.Control)(object)this.combranches).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.combranches).Visible = false;
		this.label45.AutoSize = true;
		this.label45.Enabled = false;
		this.label45.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(1289, 53);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(47, 17);
		this.label45.TabIndex = 127;
		this.label45.Text = "Branch";
		this.label45.Visible = false;
		this.comproject.AllowNull = (DefaultableBoolean)1;
		this.comproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.comproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.comproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance142;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.comproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance145).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance148).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance149).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).TextHAlignAsString = "Left";
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comproject).Enabled = false;
		((System.Windows.Forms.Control)(object)this.comproject).Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comproject).Location = new System.Drawing.Point(1343, 79);
		((System.Windows.Forms.Control)(object)this.comproject).Name = "comproject";
		((System.Windows.Forms.Control)(object)this.comproject).Size = new System.Drawing.Size(58, 27);
		((System.Windows.Forms.Control)(object)this.comproject).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.comproject).Visible = false;
		this.label46.AutoSize = true;
		this.label46.Enabled = false;
		this.label46.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(1289, 84);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(48, 17);
		this.label46.TabIndex = 128;
		this.label46.Text = "Project";
		this.label46.Visible = false;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Controls.Add(this.btnClearInput);
		this.panel3.Controls.Add(this.label21);
		this.panel3.Controls.Add(this.chkPrintVoucher);
		this.panel3.Controls.Add(this.ChkPreviewSlip);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1183, 55);
		this.panel3.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[14]
		{
			this.btnnew, this.btnRefresh, this.toolStripSeparator3, this.btnsave, this.btnUpdate, this.btnDelete, this.toolStripSeparator, this.btnVoucherReport, this.btnPrint, this.toolStripSeparator5,
			this.btnattachment, this.btnshortcutkeys, this.toolStripSeparator2, this.btnGenerateRatesOutput
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1183, 27);
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
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
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
		this.btnVoucherReport.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnVoucherReport.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnVoucherReport.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnVoucherReport.Name = "btnVoucherReport";
		this.btnVoucherReport.Size = new System.Drawing.Size(115, 24);
		this.btnVoucherReport.Text = "118-Voucher";
		this.btnVoucherReport.Click += new System.EventHandler(btnVoucherReport_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(100, 24);
		this.btnPrint.Text = "1867-&Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.btnattachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnGenerateRatesOutput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnGenerateRatesOutput.Image = Architecture.WinApp.Properties.Resources.Account_Transaction;
		this.btnGenerateRatesOutput.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnGenerateRatesOutput.Name = "btnGenerateRatesOutput";
		this.btnGenerateRatesOutput.Size = new System.Drawing.Size(290, 24);
		this.btnGenerateRatesOutput.Text = "Update Input,Wages,OH && Output Rate";
		this.btnGenerateRatesOutput.Click += new System.EventHandler(btnGenerateRatesOutput_Click);
		this.btnClearInput.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnClearInput.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.btnClearInput.FlatAppearance.BorderSize = 0;
		this.btnClearInput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnClearInput.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnClearInput.ForeColor = System.Drawing.Color.White;
		this.btnClearInput.Location = new System.Drawing.Point(848, 29);
		this.btnClearInput.Name = "btnClearInput";
		this.btnClearInput.Size = new System.Drawing.Size(93, 23);
		this.btnClearInput.TabIndex = 3;
		this.btnClearInput.Text = "Clear Input";
		this.btnClearInput.UseVisualStyleBackColor = false;
		this.btnClearInput.Click += new System.EventHandler(btnClearInput_Click);
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.White;
		this.label21.Location = new System.Drawing.Point(7, 31);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(99, 19);
		this.label21.TabIndex = 126;
		this.label21.Text = "Conversion";
		this.chkPrintVoucher.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkPrintVoucher.AutoSize = true;
		this.chkPrintVoucher.Checked = true;
		this.chkPrintVoucher.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkPrintVoucher.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPrintVoucher.ForeColor = System.Drawing.Color.White;
		this.chkPrintVoucher.Location = new System.Drawing.Point(1052, 30);
		this.chkPrintVoucher.Name = "chkPrintVoucher";
		this.chkPrintVoucher.Size = new System.Drawing.Size(124, 20);
		this.chkPrintVoucher.TabIndex = 15;
		this.chkPrintVoucher.Text = "Voucher Print";
		this.chkPrintVoucher.UseVisualStyleBackColor = true;
		this.ChkPreviewSlip.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPreviewSlip.AutoSize = true;
		this.ChkPreviewSlip.Checked = true;
		this.ChkPreviewSlip.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkPreviewSlip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPreviewSlip.ForeColor = System.Drawing.Color.White;
		this.ChkPreviewSlip.Location = new System.Drawing.Point(960, 30);
		this.ChkPreviewSlip.Name = "ChkPreviewSlip";
		this.ChkPreviewSlip.Size = new System.Drawing.Size(90, 20);
		this.ChkPreviewSlip.TabIndex = 16;
		this.ChkPreviewSlip.Text = "Slip Print";
		this.ChkPreviewSlip.UseVisualStyleBackColor = true;
		this.tabPage3.Controls.Add(this.panel45);
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.DatagridHistory);
		this.tabPage3.Controls.Add(this.panel15);
		this.tabPage3.Controls.Add(this.panel13);
		this.tabPage3.Controls.Add(this.panel38);
		this.tabPage3.Location = new System.Drawing.Point(4, 4);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(1189, 682);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "History";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel45.Controls.Add(this.panel8);
		this.panel45.Controls.Add(this.panel19);
		this.panel45.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel45.Location = new System.Drawing.Point(0, 356);
		this.panel45.Name = "panel45";
		this.panel45.Size = new System.Drawing.Size(1189, 326);
		this.panel45.TabIndex = 55;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryOverHead);
		this.panel8.Controls.Add(this.panel42);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryInput);
		this.panel8.Controls.Add(this.panel44);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(645, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(544, 326);
		this.panel8.TabIndex = 50;
		this.grdHistoryOverHead.AllowEdit = (InheritableBoolean)2;
		grdHistoryOverHead_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryOverHead.DesignTimeLayout = grdHistoryOverHead_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryOverHead).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryOverHead.DynamicFiltering = true;
		this.grdHistoryOverHead.FilterMode = (FilterMode)2;
		this.grdHistoryOverHead.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryOverHead).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryOverHead.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryOverHead).Location = new System.Drawing.Point(0, 215);
		((System.Windows.Forms.Control)(object)this.grdHistoryOverHead).Name = "grdHistoryOverHead";
		((ScrollableBase)this.grdHistoryOverHead).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryOverHead).Size = new System.Drawing.Size(544, 111);
		((System.Windows.Forms.Control)(object)this.grdHistoryOverHead).TabIndex = 1;
		this.grdHistoryOverHead.TotalRow = (InheritableBoolean)1;
		this.grdHistoryOverHead.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryOverHead).VisualStyle = (VisualStyle)4;
		this.panel42.BackColor = System.Drawing.Color.Teal;
		this.panel42.Controls.Add(this.label34);
		this.panel42.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel42.Location = new System.Drawing.Point(0, 196);
		this.panel42.Name = "panel42";
		this.panel42.Size = new System.Drawing.Size(544, 19);
		this.panel42.TabIndex = 58;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.ForeColor = System.Drawing.Color.White;
		this.label34.Location = new System.Drawing.Point(3, 0);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(78, 16);
		this.label34.TabIndex = 127;
		this.label34.Text = "OverHeads";
		this.grdHistoryInput.AllowEdit = (InheritableBoolean)2;
		grdHistoryInput_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryInput.DesignTimeLayout = grdHistoryInput_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdHistoryInput.DynamicFiltering = true;
		this.grdHistoryInput.FilterMode = (FilterMode)2;
		this.grdHistoryInput.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryInput.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).Location = new System.Drawing.Point(0, 19);
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).Name = "grdHistoryInput";
		((ScrollableBase)this.grdHistoryInput).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).Size = new System.Drawing.Size(544, 177);
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).TabIndex = 0;
		this.grdHistoryInput.TotalRow = (InheritableBoolean)1;
		this.grdHistoryInput.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryInput).VisualStyle = (VisualStyle)4;
		this.panel44.BackColor = System.Drawing.Color.Teal;
		this.panel44.Controls.Add(this.label14);
		this.panel44.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel44.Location = new System.Drawing.Point(0, 0);
		this.panel44.Name = "panel44";
		this.panel44.Size = new System.Drawing.Size(544, 19);
		this.panel44.TabIndex = 49;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(2, 1);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(222, 16);
		this.label14.TabIndex = 128;
		this.label14.Text = "Input Material (Consumed Items)";
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.grdWagesHistory);
		this.panel19.Controls.Add(this.panel18);
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryOutput);
		this.panel19.Controls.Add(this.panel12);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel19.Location = new System.Drawing.Point(0, 0);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(645, 326);
		this.panel19.TabIndex = 4;
		this.grdWagesHistory.AllowEdit = (InheritableBoolean)2;
		grdWagesHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdWagesHistory.DesignTimeLayout = grdWagesHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdWagesHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdWagesHistory.DynamicFiltering = true;
		this.grdWagesHistory.FilterMode = (FilterMode)2;
		this.grdWagesHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdWagesHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdWagesHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdWagesHistory).Location = new System.Drawing.Point(0, 215);
		((System.Windows.Forms.Control)(object)this.grdWagesHistory).Name = "grdWagesHistory";
		((ScrollableBase)this.grdWagesHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdWagesHistory).Size = new System.Drawing.Size(645, 111);
		((System.Windows.Forms.Control)(object)this.grdWagesHistory).TabIndex = 1;
		this.grdWagesHistory.TotalRow = (InheritableBoolean)1;
		this.grdWagesHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdWagesHistory).VisualStyle = (VisualStyle)4;
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.label40);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(0, 196);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(645, 19);
		this.panel18.TabIndex = 55;
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.ForeColor = System.Drawing.Color.White;
		this.label40.Location = new System.Drawing.Point(3, 0);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(126, 16);
		this.label40.TabIndex = 127;
		this.label40.Text = "Contractor Wages";
		this.grdHistoryOutput.AllowEdit = (InheritableBoolean)2;
		grdHistoryOutput_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryOutput.DesignTimeLayout = grdHistoryOutput_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdHistoryOutput.DynamicFiltering = true;
		this.grdHistoryOutput.FilterMode = (FilterMode)2;
		this.grdHistoryOutput.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryOutput.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Location = new System.Drawing.Point(0, 19);
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Name = "grdHistoryOutput";
		((ScrollableBase)this.grdHistoryOutput).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Size = new System.Drawing.Size(645, 177);
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).TabIndex = 0;
		this.grdHistoryOutput.TotalRow = (InheritableBoolean)1;
		this.grdHistoryOutput.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryOutput).VisualStyle = (VisualStyle)4;
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label15);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(645, 19);
		this.panel12.TabIndex = 52;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(6, 1);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(170, 16);
		this.label15.TabIndex = 127;
		this.label15.Text = "OutPut (Produced Items)";
		this.DatagridHistory.AllowEdit = (InheritableBoolean)2;
		DatagridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DatagridHistory.DesignTimeLayout = DatagridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Dock = System.Windows.Forms.DockStyle.Top;
		this.DatagridHistory.DynamicFiltering = true;
		this.DatagridHistory.FilterMode = (FilterMode)2;
		this.DatagridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DatagridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Location = new System.Drawing.Point(0, 140);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Name = "DatagridHistory";
		((ScrollableBase)this.DatagridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Size = new System.Drawing.Size(1189, 216);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).TabIndex = 0;
		this.DatagridHistory.TotalRow = (InheritableBoolean)1;
		this.DatagridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DatagridHistory).VisualStyle = (VisualStyle)4;
		this.DatagridHistory.ColumnButtonClick += new ColumnActionEventHandler(DatagridHistory_ColumnButtonClick);
		this.DatagridHistory.SelectionChanged += new System.EventHandler(DatagridHistory_SelectionChanged);
		this.DatagridHistory.LinkClicked += new ColumnActionEventHandler(DatagridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).DoubleClick += new System.EventHandler(DatagridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DatagridHistory_KeyDown);
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.ctrlGrdBar1);
		this.panel15.Controls.Add(this.label17);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 117);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1189, 23);
		this.panel15.TabIndex = 53;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = null;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1142, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(9, 6, 9, 6);
		this.ctrlGrdBar1.MyGrid = null;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(42, 22);
		this.ctrlGrdBar1.TabIndex = 128;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(8, 3);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(54, 16);
		this.label17.TabIndex = 127;
		this.label17.Text = "History";
		this.panel13.BackColor = System.Drawing.Color.White;
		this.panel13.Controls.Add(this.groupBox1);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 51);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1189, 66);
		this.panel13.TabIndex = 0;
		this.groupBox1.Controls.Add(this.RadNotReffered);
		this.groupBox1.Controls.Add(this.RadReffered);
		this.groupBox1.Controls.Add(this.RadAll);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbConversionNoHistory);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory);
		this.groupBox1.Controls.Add(this.txtToDateHistory);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.txtToDocNoHistory);
		this.groupBox1.Controls.Add(this.txtFromdateHistory);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Controls.Add(this.txtFromNoHistory);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(5, 1);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1181, 64);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.RadNotReffered.AutoSize = true;
		this.RadNotReffered.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.RadNotReffered.Location = new System.Drawing.Point(1074, 35);
		this.RadNotReffered.Name = "RadNotReffered";
		this.RadNotReffered.Size = new System.Drawing.Size(108, 23);
		this.RadNotReffered.TabIndex = 9;
		this.RadNotReffered.Text = "Not Reffered";
		this.RadNotReffered.UseVisualStyleBackColor = true;
		this.RadNotReffered.Visible = false;
		this.RadReffered.AutoSize = true;
		this.RadReffered.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.RadReffered.Location = new System.Drawing.Point(995, 35);
		this.RadReffered.Name = "RadReffered";
		this.RadReffered.Size = new System.Drawing.Size(80, 23);
		this.RadReffered.TabIndex = 8;
		this.RadReffered.Text = "Reffered";
		this.RadReffered.UseVisualStyleBackColor = true;
		this.RadReffered.Visible = false;
		this.RadAll.AutoSize = true;
		this.RadAll.Checked = true;
		this.RadAll.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.RadAll.Location = new System.Drawing.Point(950, 35);
		this.RadAll.Name = "RadAll";
		this.RadAll.Size = new System.Drawing.Size(44, 23);
		this.RadAll.TabIndex = 7;
		this.RadAll.TabStop = true;
		this.RadAll.Text = "All";
		this.RadAll.UseVisualStyleBackColor = true;
		this.RadAll.Visible = false;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(885, 34);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(62, 25);
		this.btnshow.TabIndex = 6;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(679, 15);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(85, 15);
		this.label30.TabIndex = 178;
		this.label30.Text = "Conversion No";
		this.cmbConversionNoHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbConversionNoHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbConversionNoHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbConversionNoHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbConversionNoHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.cmbConversionNoHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbConversionNoHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance155).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance155).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbConversionNoHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbConversionNoHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance156;
		((SpecialBoxBase)((UltraGridBase)this.cmbConversionNoHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbConversionNoHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbConversionNoHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbConversionNoHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbConversionNoHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbConversionNoHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbConversionNoHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbConversionNoHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbConversionNoHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbConversionNoHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbConversionNoHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbConversionNoHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbConversionNoHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbConversionNoHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbConversionNoHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbConversionNoHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbConversionNoHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbConversionNoHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbConversionNoHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbConversionNoHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbConversionNoHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbConversionNoHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbConversionNoHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbConversionNoHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbConversionNoHistory).Location = new System.Drawing.Point(679, 33);
		((System.Windows.Forms.Control)(object)this.cmbConversionNoHistory).Name = "cmbConversionNoHistory";
		((System.Windows.Forms.Control)(object)this.cmbConversionNoHistory).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.cmbConversionNoHistory).TabIndex = 5;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(5, 15);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(60, 15);
		this.label19.TabIndex = 174;
		this.label19.Text = "Date Type";
		this.cmbDateTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbDateTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance16;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance21).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance22).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Location = new System.Drawing.Point(3, 33);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Name = "cmbDateTypeHistory";
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Size = new System.Drawing.Size(168, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).TabIndex = 0;
		this.cmbDateTypeHistory.ValueChanged += new System.EventHandler(cmbDateTypeHistory_ValueChanged);
		this.txtToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.CustomFormat = "dd-MMM-yy";
		this.txtToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateHistory.Location = new System.Drawing.Point(327, 35);
		this.txtToDateHistory.Name = "txtToDateHistory";
		this.txtToDateHistory.ShowCheckBox = true;
		this.txtToDateHistory.Size = new System.Drawing.Size(152, 23);
		this.txtToDateHistory.TabIndex = 2;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(167, 15);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(91, 15);
		this.label28.TabIndex = 171;
		this.label28.Text = "Date From && To";
		this.txtToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(579, 35);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.Size = new System.Drawing.Size(97, 23);
		this.txtToDocNoHistory.TabIndex = 4;
		this.txtFromdateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.CustomFormat = "dd-MMM-yy";
		this.txtFromdateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromdateHistory.Location = new System.Drawing.Point(173, 35);
		this.txtFromdateHistory.Name = "txtFromdateHistory";
		this.txtFromdateHistory.ShowCheckBox = true;
		this.txtFromdateHistory.Size = new System.Drawing.Size(152, 23);
		this.txtFromdateHistory.TabIndex = 1;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(475, 15);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(88, 15);
		this.label35.TabIndex = 173;
		this.label35.Text = "Doc From && To";
		this.txtFromNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromNoHistory.Location = new System.Drawing.Point(481, 35);
		this.txtFromNoHistory.Name = "txtFromNoHistory";
		this.txtFromNoHistory.Size = new System.Drawing.Size(97, 23);
		this.txtFromNoHistory.TabIndex = 3;
		this.panel38.BackColor = System.Drawing.Color.Teal;
		this.panel38.Controls.Add(this.toolStrip2);
		this.panel38.Controls.Add(this.label32);
		this.panel38.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel38.Location = new System.Drawing.Point(0, 0);
		this.panel38.Name = "panel38";
		this.panel38.Size = new System.Drawing.Size(1189, 51);
		this.panel38.TabIndex = 48;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.toolStripSeparator1, this.btnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1189, 27);
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
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "&Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.White;
		this.label32.Location = new System.Drawing.Point(3, 29);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(172, 19);
		this.label32.TabIndex = 127;
		this.label32.Text = "Conversion History  ";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1197, 711);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmConversion";
		this.Text = "Conversion";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmConversion_FormClosed);
		base.Load += new System.EventHandler(frmBillOfMaterial_Manufacturing_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmBillOfMaterial_Manufacturing_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel17.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInputMaterial).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel10.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.TabOutPut.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOutPutByProduct).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.TabWages.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdContractorWages).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.PanelMessage.ResumeLayout(false);
		this.PanelMessage.PerformLayout();
		this.ToolStripMessage.ResumeLayout(false);
		this.ToolStripMessage.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbContractorForUpdateInWagesGrid).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbActivityforContractorUpdateInWagesGrid).EndInit();
		this.TabOverheads.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOverHead).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemBOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAttribute).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseName).EndInit();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantName).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel20.ResumeLayout(false);
		this.panel20.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.comCompany).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage3.ResumeLayout(false);
		this.panel45.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryOverHead).EndInit();
		this.panel42.ResumeLayout(false);
		this.panel42.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryInput).EndInit();
		this.panel44.ResumeLayout(false);
		this.panel44.PerformLayout();
		this.panel19.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdWagesHistory).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryOutput).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel13.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbConversionNoHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).EndInit();
		this.panel38.ResumeLayout(false);
		this.panel38.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
