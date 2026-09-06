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
using Architecture.BLL.Mfg;
using Architecture.BLL.Production;
using Architecture.BLL.pcc;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Common;
using Architecture.Model.Inventory;
using Architecture.Model.Pcc;
using Architecture.Model.Production;
using Architecture.Reports;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Production;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.pcc;

public class frmBillOfMaterialConcrete : Form
{
	private Attachment AT = new Attachment();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtInput = new DataTable();

	private DataTable dtOutPutByProduct = new DataTable();

	private DataTable dtOverHead = new DataTable();

	private DataTable dtOperationProcess = new DataTable();

	private DataTable dtContractorWages = new DataTable();

	private DataTable lstitem = new DataTable();

	private DataTable dtitemHeader = new DataTable();

	private DataTable dtitemInput = new DataTable();

	private DataTable dtUom = new DataTable();

	private DataTable dtRateUom = new DataTable();

	private DataTable dtuomGetAll = new DataTable();

	private DataTable dtHistory = new DataTable();

	private DataTable dtAccounts = new DataTable();

	private DataTable dtOperationcombo = new DataTable();

	private DataTable dtInputMainItems = new DataTable();

	private DataTable dtUomInputMain = new DataTable();

	private bool UpdateMode;

	private int RecId;

	private int ProcessRecId;

	private bool ProcessUpdateMode;

	private List<BomInputDetail> lstRemoveRecordInput = new List<BomInputDetail>();

	private List<BomByProductDetail> lstRemoveRecordOutPut = new List<BomByProductDetail>();

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

	private CheckBox chkIsDefault;

	private CheckBox ChkIsActive;

	private TextBox txtQty;

	private Label label18;

	internal UltraCombo cmbUOM;

	private Label label44;

	internal UltraCombo cmbItem;

	private Label label7;

	private TextBox txtNetValue;

	private Label label10;

	private TextBox txtOverHead;

	private Label label9;

	private TextBox txtLessByProduct;

	private Label label8;

	private TextBox txtTotalInputs;

	private Label label6;

	internal UltraCombo comCompany;

	private Label label4;

	internal UltraCombo combranches;

	private Label label45;

	internal UltraCombo comproject;

	private Label label46;

	private TextBox txtBom;

	private Label label11;

	private Label label12;

	private TabPage tabPage3;

	private Label label13;

	private DateTimePicker txtEffectedDate;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewHistory;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton btnRegister;

	private Panel panel4;

	private Panel panel20;

	private Label label5;

	private Label label25;

	private Label label24;

	private Label label3;

	private GroupBox groupBox2;

	private Label label2;

	private Label label21;

	private Panel panel6;

	internal GridEX grdOperatinalDetail;

	internal UltraCombo cmbBOMType;

	private Label label37;

	private DateTimePicker txtdocdate;

	private TextBox txtdocno;

	private Panel panel10;

	private Panel panel16;

	private Label label27;

	private Label label26;

	internal GridEX grdInputMaterial;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnDelete;

	private ToolStripButton btnshortcutkeys;

	private Panel panel38;

	private Label label32;

	private Panel panel45;

	private Panel panel8;

	internal GridEX grdhistoryoperations;

	private Panel panel9;

	private Label label14;

	internal GridEX grdHistoryOverHead;

	private Panel panel42;

	private Label label34;

	private Panel panel19;

	internal GridEX grdHistoryInput;

	private Panel panel44;

	private Label label16;

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

	internal UltraCombo cmbbomtypeHistory;

	internal UltraCombo CmbBomNoHistory;

	private Label label20;

	internal UltraCombo cmbItemhistory;

	private Label label29;

	private Button btnshow;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStripButton BtnProductionWorkstation;

	private CheckBox ChkPreview;

	internal GridEX grdHistoryOutput;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator2;

	private TextBox txtRemarksHeader;

	internal GridEX grdOutPutByProduct;

	private Panel panel11;

	private Label label23;

	private Panel panel7;

	private Label label1;

	private Panel panel17;

	private TextBox txtValuePerQty;

	private Label label31;

	private Label label36;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label39;

	private Label label38;

	private TextBox txtWagesAmount;

	internal GridEX grdWagesHistory;

	private Panel panel18;

	private Label label40;

	private Panel panel12;

	private Label label15;

	private CtrlGrdBar ctrlGrdBar3;

	internal GridEX grdContractorWages;

	private Panel panel5;

	private Label labItemCaption;

	private Label label33;

	internal GridEX grdOverHead;

	private Panel panel14;

	private Label label22;

	private Button btnDefinePlant;

	internal UltraCombo CmbPlantName;

	private Label label41;

	internal UltraCombo CmbProductionType;

	private Label label42;

	private ToolStripButton btnattachment;

	private Label label43;

	private Label label48;

	private TextBox txtBottomForce;

	private TextBox txtTopForce;

	private Label label47;

	public frmBillOfMaterialConcrete(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmBillOfMaterial_Manufacturing_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 750);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			btnPrint.Enabled = formright.DoHavePrintRights;
			btnDelete.Enabled = formright.DoHaveCanDelete;
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
			dtAccounts.Columns.Add("Id");
			dtAccounts.Columns.Add("AccountTitle");
			dtitemHeader.Columns.Add("Id", typeof(int));
			dtitemHeader.Columns.Add("ItemName");
			dtInputMainItems.Columns.Add("Id", typeof(int));
			dtInputMainItems.Columns.Add("ItemName");
			dtUom.Columns.Add("Id", typeof(int));
			dtUom.Columns.Add("ItemUOM", typeof(double));
			dtUom.Columns.Add("ItemUOMCode", typeof(string));
			dtUomInputMain.Columns.Add("Id", typeof(int));
			dtUomInputMain.Columns.Add("ItemUOM", typeof(double));
			dtUomInputMain.Columns.Add("ItemUOMCode", typeof(string));
			dtRateUom.Columns.Add("RateId", typeof(int));
			dtRateUom.Columns.Add("RateUOM", typeof(double));
			dtRateUom.Columns.Add("RateUOMCode", typeof(string));
			ItemFill();
			uomgetAll();
			AccountTitleFill();
			Datetypefill();
			AllCombobind();
			DocumentNoFill();
			TypeBind();
			ProductionTypeFill();
			PlantFeader();
			dtInput.Columns.Add("Id", typeof(int));
			dtInput.Columns.Add("ItemId", typeof(int));
			dtInput.Columns.Add("ItemName", typeof(string));
			dtInput.Columns.Add("ItemUomId", typeof(int));
			dtInput.Columns.Add("Uom", typeof(decimal));
			dtInput.Columns.Add("UomCode", typeof(string));
			dtInput.Columns.Add("ItemQty", typeof(decimal));
			dtInput.Columns.Add("NetWeight", typeof(decimal));
			dtInput.Columns.Add("Rate", typeof(decimal));
			dtInput.Columns.Add("RateUomId", typeof(int));
			dtInput.Columns.Add("RateUom", typeof(decimal));
			dtInput.Columns.Add("RateUomCode", typeof(string));
			dtInput.Columns.Add("Amount", typeof(decimal));
			dtInput.Columns.Add("MinQty", typeof(decimal));
			dtInput.Columns.Add("MaxQty", typeof(decimal));
			dtInput.Columns.Add("MainItemId", typeof(int));
			dtInput.Columns.Add("MainItemName", typeof(string));
			dtInput.Columns.Add("MainItemUomId", typeof(int));
			dtInput.Columns.Add("MainUomCode", typeof(string));
			AddRowInInputGrid();
			grdInputMaterial.RetrieveStructure();
			grdInputSettings();
			dtOutPutByProduct.Columns.Add("Id", typeof(int));
			dtOutPutByProduct.Columns.Add("ItemId", typeof(int));
			dtOutPutByProduct.Columns.Add("ItemName", typeof(string));
			dtOutPutByProduct.Columns.Add("ItemUomId", typeof(int));
			dtOutPutByProduct.Columns.Add("Uom", typeof(decimal));
			dtOutPutByProduct.Columns.Add("UomCode", typeof(string));
			dtOutPutByProduct.Columns.Add("ItemQty", typeof(decimal));
			dtOutPutByProduct.Columns.Add("NetWeight", typeof(decimal));
			dtOutPutByProduct.Columns.Add("Rate", typeof(decimal));
			dtOutPutByProduct.Columns.Add("RateUomId", typeof(int));
			dtOutPutByProduct.Columns.Add("RateUom", typeof(decimal));
			dtOutPutByProduct.Columns.Add("RateUomCode", typeof(string));
			dtOutPutByProduct.Columns.Add("Amount", typeof(decimal));
			AddRowInOutPutByProductGrid();
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
			AddRowInOverHeadGrid();
			grdOverHead.RetrieveStructure();
			grdOverHeadGridSetting();
			dtContractorWages.Columns.Add("Id", typeof(int));
			dtContractorWages.Columns.Add("ContractorWagesRateScheduleId", typeof(int));
			dtContractorWages.Columns.Add("ServiceActivity");
			dtContractorWages.Columns.Add("Qty", typeof(decimal));
			dtContractorWages.Columns.Add("Rate", typeof(decimal));
			dtContractorWages.Columns.Add("Amount", typeof(decimal));
			dtContractorWages.Columns.Add("Remarks");
			AddRowInContractorWagesGrid();
			grdContractorWages.RetrieveStructure();
			grdContractorWagesGridSetting();
			txtBom.Focus();
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
			int code = BomHeader.GenerateCode(new BomHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 1851,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			txtdocno.Text = code.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemFill()
	{
		try
		{
			int Id = Conversion.ToInt(cmbItem.Value);
			dtitemInput.Clear();
			dtitemInput = CommonServices.ItemGetAllServiceBind();
			lstitem.Clear();
			lstitem = CommonServices.GetItemByItemTypeId("45,46,47");
			if (lstitem.Rows.Count > 0)
			{
				dtitemHeader.Clear();
				for (int i = 0; i < lstitem.Rows.Count; i++)
				{
					dtitemHeader.Rows.Add(lstitem.Rows[i]["Id"], lstitem.Rows[i]["ItemName"]);
				}
				if (dtitemHeader.Rows.Count > 0)
				{
					DDL.BindDDL(dtitemHeader, cmbItem, "Id", "ItemName", "Item Name", ZeroIndex: true);
					((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 350;
					if (Id > 0)
					{
						if (dtitemHeader.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
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
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountTitleFill()
	{
		try
		{
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
			MessageBox.Show(ex.Message);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbItem_Leave(object sender, EventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(cmbUOM.Value);
			((Control)(object)cmbUOM).Text = string.Empty;
			DataTable dtuomheader = new DataTable();
			dtuomheader.Columns.Add("Id", typeof(int));
			dtuomheader.Columns.Add("ItemUOM");
			for (int i = 0; i < dtuomGetAll.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtuomGetAll.Rows[i]["ItemId"]) == Conversion.ToInt(cmbItem.Value))
				{
					dtuomheader.Rows.Add(dtuomGetAll.Rows[i]["Id"], dtuomGetAll.Rows[i]["UOMDescription"]);
				}
			}
			if (dtuomheader.Rows.Count > 0)
			{
				DDL.BindDDL(dtuomheader, cmbUOM, "Id", "ItemUOM", "UOM", ZeroIndex: false);
				((UltraGridBase)cmbUOM).DisplayLayout.Bands[0].Columns[1].Width = 193;
				if (Id > 0)
				{
					if (dtuomheader.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						cmbUOM.Value = Id;
					}
					else
					{
						((Control)(object)cmbUOM).Text = string.Empty;
					}
				}
			}
			labItemCaption.Text = Conversion.ToString((object)((Control)(object)cmbItem).Text);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void TypeBind()
	{
		DataTable DtType = new DataTable();
		DtType.Columns.Add("Id");
		DtType.Columns.Add("Type");
		DtType.Rows.Add(1, "Semi-Finish Goods");
		DtType.Rows.Add(2, "Finish Goods");
		if (DtType.Rows.Count > 0)
		{
			DDL.BindDDLNew(DtType, cmbBOMType, "Id", "Type", "Finish Type", false);
			((UltraGridBase)cmbBOMType).Rows[0].Activate();
		}
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
			MessageBox.Show(ex.Message);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbProductionType_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbProductionType.Value) > 0)
			{
				int Id = Conversion.ToInt(cmbItem.Value);
				dtitemHeader.Clear();
				dtitemHeader = CommonServices.GetItemByItemTypeId(Conversion.ToString(CmbProductionType.Value));
				if (dtitemHeader.Rows.Count > 0)
				{
					DDL.BindDDL(dtitemHeader, cmbItem, "Id", "ItemName", "Item Name", ZeroIndex: true);
					((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 350;
					if (Id > 0)
					{
						if (dtitemHeader.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
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
			else
			{
				ItemFill();
			}
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

	private bool FormValidation()
	{
		if (txtdocno.Text.Trim() == string.Empty || txtdocno.Text == "0")
		{
			MessageBox.Show("doc No Field Required");
			txtdocno.Focus();
			return false;
		}
		if (txtBom.Text.Trim() == string.Empty || txtBom.Text == "0")
		{
			MessageBox.Show("BOM Field Required");
			txtBom.Focus();
			return false;
		}
		if (((Control)(object)CmbProductionType).Text.Trim() == string.Empty || Conversion.ToInt(CmbProductionType.Value) == 0)
		{
			MessageBox.Show("Bom Type Field Required");
			CmbProductionType.Focus();
			return false;
		}
		if (((Control)(object)cmbBOMType).Text.Trim() == string.Empty || Conversion.ToInt(cmbBOMType.Value) == 0)
		{
			MessageBox.Show("Finish Type Field Required");
			cmbBOMType.Focus();
			return false;
		}
		if (((Control)(object)cmbItem).Text.Trim() == string.Empty || Conversion.ToInt(cmbItem.Value) == 0)
		{
			MessageBox.Show("Item Feild Required");
			cmbItem.Focus();
			return false;
		}
		if (((Control)(object)cmbUOM).Text.Trim() == string.Empty || Conversion.ToInt(cmbUOM.Value) == 0)
		{
			MessageBox.Show("IteUom Field Required");
			cmbUOM.Focus();
			return false;
		}
		return true;
	}

	private void AddRowInInputGrid()
	{
		dtInput.Rows.Add(0, 0, "", 0, 0, "", 0, 0, 0, 0, 0, "", 0, 0, 0, 0, "", 0, "");
		grdInputMaterial.DataSource = dtInput;
	}

	private void grdInputComboFill()
	{
		try
		{
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
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInputSettings()
	{
		try
		{
			grdInputComboFill();
			grdInputMaterial.RootTable.Columns["Id"].Visible = false;
			grdInputMaterial.RootTable.Columns["ItemId"].Visible = false;
			grdInputMaterial.RootTable.Columns["RateUomId"].Visible = false;
			grdInputMaterial.RootTable.Columns["RateUom"].Visible = false;
			grdInputMaterial.RootTable.Columns["ItemUomId"].Visible = false;
			grdInputMaterial.RootTable.Columns["MainItemId"].Visible = false;
			grdInputMaterial.RootTable.Columns["MainItemUomId"].Visible = false;
			grdInputMaterial.RootTable.Columns["NetWeight"].Visible = false;
			grdInputMaterial.RootTable.Columns["RateUomCode"].Visible = false;
			grdInputMaterial.RootTable.Columns["Uom"].Visible = false;
			grdInputMaterial.RootTable.Columns["UomCode"].Caption = "Uom";
			grdInputMaterial.RootTable.Columns["MainUomCode"].Caption = "Main Uom";
			grdInputMaterial.RootTable.Columns["RateUomCode"].Caption = "Rate Uom";
			grdInputMaterial.RootTable.Columns["Id"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["NetWeight"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["RateUomId"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["RateUom"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["RateUomCode"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["ItemUomId"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["Uom"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["UomCode"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["MainItemUomId"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["MainUomCode"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["MainItemId"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["MainItemName"].EditType = (EditType)0;
			grdInputMaterial.RootTable.Columns["ItemQty"].EditType = (EditType)1;
			grdInputMaterial.RootTable.Columns["Rate"].EditType = (EditType)1;
			grdInputMaterial.RootTable.Columns["MinQty"].EditType = (EditType)1;
			grdInputMaterial.RootTable.Columns["MaxQty"].EditType = (EditType)1;
			grdInputMaterial.RootTable.Columns["ItemName"].Width = 100;
			grdInputMaterial.RootTable.Columns["UomCode"].Width = 60;
			grdInputMaterial.RootTable.Columns["ItemQty"].Width = 70;
			grdInputMaterial.RootTable.Columns["Rate"].Width = 55;
			grdInputMaterial.RootTable.Columns["Amount"].Width = 75;
			grdInputMaterial.RootTable.Columns["MinQty"].Width = 60;
			grdInputMaterial.RootTable.Columns["MaxQty"].Width = 60;
			grdInputMaterial.RootTable.Columns["MainItemName"].Width = 105;
			grdInputMaterial.RootTable.Columns["MainUomCode"].Width = 60;
			grdInputMaterial.RootTable.Columns["MinQty"].AggregateFunction = (AggregateFunction)2;
			grdInputMaterial.RootTable.Columns["MinQty"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["MinQty"].FormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["MinQty"].TotalFormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["MaxQty"].AggregateFunction = (AggregateFunction)2;
			grdInputMaterial.RootTable.Columns["MaxQty"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["MaxQty"].FormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["MaxQty"].TotalFormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdInputMaterial.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["ItemQty"].FormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["ItemQty"].TotalFormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["Uom"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["Uom"].FormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["Uom"].TotalFormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["RateUom"].TextAlignment = (TextAlignment)3;
			grdInputMaterial.RootTable.Columns["RateUom"].FormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["RateUom"].TotalFormatString = "##,#.####";
			grdInputMaterial.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)2;
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
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInputMaterial_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdInputMaterial.UpdateData();
			GridEXRow item = grdInputMaterial.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Column.Key == "ItemId")
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
				if (e.Column.Key == "ItemQty" || e.Column.Key == "Uom" || e.Column.Key == "ItemUomId")
				{
					if (item.Cells["ItemQty"].Value.ToString() != string.Empty && item.Cells["ItemUomId"].Value.ToString() != string.Empty)
					{
						decimal num = Conversion.ToDecimal((object)item.Cells["ItemQty"].Value.ToString());
						decimal quom = Conversion.ToDecimal((object)item.Cells["Uom"].Value.ToString());
						decimal Weight2 = num * quom;
						item.Cells["NetWeight"].Value = Weight2;
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
				if (e.Column.Key == "Rate" || e.Column.Key == "RateUomId" || e.Column.Key == "RateUom")
				{
					decimal Weight = default(decimal);
					decimal Iuom = default(decimal);
					if (!(item.Cells["ItemQty"].Value.ToString() != string.Empty) || !(item.Cells["ItemUomId"].Value.ToString() != string.Empty))
					{
						throw new Exception("ItemQty And ItemUom Should be greater than zero...");
					}
					decimal num3 = Conversion.ToDecimal((object)item.Cells["ItemQty"].Value.ToString());
					Iuom = Conversion.ToDecimal((object)item.Cells["Uom"].Value.ToString());
					Weight = num3 * Iuom;
					_ = Weight != 0m;
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
				if (e.Column.Key == "MinQty")
				{
					if (item.Cells["ItemQty"].Value.ToString() == string.Empty || Conversion.ToInt(item.Cells["ItemQty"].Value) == 0)
					{
						item.Cells["MinQty"].Value = 0;
						throw new Exception("ItemQty Required...Please Add that First");
					}
					if (Conversion.ToInt(item.Cells["MinQty"].Value) > Conversion.ToInt(item.Cells["ItemQty"].Value))
					{
						item.Cells["MinQty"].Value = 0;
						throw new Exception("MinQty Should be less than or equal to ItemQty...");
					}
				}
				if (e.Column.Key == "MaxQty")
				{
					if (item.Cells["ItemQty"].Value.ToString() == string.Empty || Conversion.ToInt(item.Cells["ItemQty"].Value) == 0)
					{
						item.Cells["MaxQty"].Value = 0;
						throw new Exception("ItemQty Required...Please Add that First");
					}
					if (Conversion.ToInt(item.Cells["MaxQty"].Value) < Conversion.ToInt(item.Cells["ItemQty"].Value))
					{
						item.Cells["MaxQty"].Value = 0;
						throw new Exception("MaxQty Should be greater than or equal to ItemQty...");
					}
				}
			}
			grdInputMaterial.UpdateData();
			Calculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InputGridUombind(int ItemId, GridEX grd, int Index)
	{
	}

	private void grdInputMaterial_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdInputMaterial.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					BomInputDetail d1 = new BomInputDetail();
					d1.Id = Conversion.ToInt(r.Cells["Id"].Value);
					d1.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					d1.ItemUomId = Conversion.ToInt(r.Cells["ItemUomId"].Value);
					d1.ItemQty = Conversion.ToDecimal((object)r.Cells["ItemQty"].Text);
					d1.WeightKg = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
					d1.Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
					d1.RateUomId = Conversion.ToInt(r.Cells["RateUomId"].Value);
					d1.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
					d1.ActionTypeId = 3;
					lstRemoveRecordInput.Add(d1);
					dtInput.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					grdInputMaterial.UpdateData();
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				if (dtInput.Rows.Count == 0)
				{
					AddRowInInputGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInInputGrid();
			}
			grdInputMaterial.UpdateData();
			Calculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetInputMainItems(int Index)
	{
		try
		{
			if (grdInputMaterial.RowCount <= 0)
			{
				return;
			}
			dtInputMainItems.Clear();
			GridEXRow[] rows = grdInputMaterial.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (r.RowIndex != Index)
				{
					dtInputMainItems.Rows.Add(Conversion.ToInt(r.Cells["ItemId"].Value), Conversion.ToString(r.Cells["ItemName"].Value));
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GetInputMainItemsUom(int ItemId)
	{
		try
		{
			if (grdInputMaterial.RowCount <= 0)
			{
				return;
			}
			dtUomInputMain.Clear();
			GridEXRow[] rows = grdInputMaterial.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (!dtUomInputMain.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Conversion.ToInt(r.Cells["Id"].Value)) && Conversion.ToInt(r.Cells["ItemId"].Value) == ItemId)
				{
					dtUomInputMain.Rows.Add(Conversion.ToInt(r.Cells["ItemUomId"].Value), Conversion.ToDecimal(r.Cells["Uom"].Value), Conversion.ToString(r.Cells["UomCode"].Value));
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AddRowInOutPutByProductGrid()
	{
		dtOutPutByProduct.Rows.Add(0, 0, "", 0, 0, "", 0, 0, 0, 0, 0, "", 0);
		grdOutPutByProduct.DataSource = dtOutPutByProduct;
	}

	private void grdOutPutByProductComboFill()
	{
		try
		{
			grdOutPutByProduct.RootTable.Columns.Add("Delete");
			grdOutPutByProduct.RootTable.Columns["Delete"].Key = "Delete";
			grdOutPutByProduct.RootTable.Columns["Delete"].Caption = "X";
			grdOutPutByProduct.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOutPutByProduct.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdOutPutByProduct.RootTable.Columns["Delete"].ButtonText = "X";
			grdOutPutByProduct.RootTable.Columns["Delete"].Width = 40;
			grdOutPutByProduct.RootTable.Columns["Delete"].Position = 0;
			grdOutPutByProduct.RootTable.Columns.Add("Add");
			grdOutPutByProduct.RootTable.Columns["Add"].Key = "Add";
			grdOutPutByProduct.RootTable.Columns["Add"].Caption = "+";
			grdOutPutByProduct.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOutPutByProduct.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdOutPutByProduct.RootTable.Columns["Add"].ButtonText = "+";
			grdOutPutByProduct.RootTable.Columns["Add"].Width = 40;
			grdOutPutByProduct.RootTable.Columns["Add"].Position = 1;
			grdOutPutByProduct.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOutPutGridSetting()
	{
		try
		{
			grdOutPutByProductComboFill();
			grdOutPutByProduct.RootTable.Columns["Id"].Visible = false;
			grdOutPutByProduct.RootTable.Columns["RateUomId"].Visible = false;
			grdOutPutByProduct.RootTable.Columns["ItemUomId"].Visible = false;
			grdOutPutByProduct.RootTable.Columns["RateUom"].Visible = false;
			grdOutPutByProduct.RootTable.Columns["RateUomCode"].Visible = false;
			grdOutPutByProduct.RootTable.Columns["NetWeight"].Visible = false;
			grdOutPutByProduct.RootTable.Columns["ItemId"].Visible = false;
			grdOutPutByProduct.RootTable.Columns["Uom"].Visible = false;
			grdOutPutByProduct.RootTable.Columns["UomCode"].Caption = "Uom";
			grdOutPutByProduct.RootTable.Columns["RateUomCode"].Caption = "Rate Uom";
			grdOutPutByProduct.RootTable.Columns["Id"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["NetWeight"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["RateUomId"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["RateUom"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["ItemUomId"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["RateUomCode"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["Uom"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["UomCode"].EditType = (EditType)0;
			grdOutPutByProduct.RootTable.Columns["ItemQty"].EditType = (EditType)1;
			grdOutPutByProduct.RootTable.Columns["Rate"].EditType = (EditType)1;
			grdOutPutByProduct.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdOutPutByProduct.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdOutPutByProduct.RootTable.Columns["ItemQty"].FormatString = "##,#.####";
			grdOutPutByProduct.RootTable.Columns["ItemQty"].TotalFormatString = "##,#.####";
			grdOutPutByProduct.RootTable.Columns["Uom"].TextAlignment = (TextAlignment)3;
			grdOutPutByProduct.RootTable.Columns["Uom"].FormatString = "##,#.####";
			grdOutPutByProduct.RootTable.Columns["Uom"].TotalFormatString = "##,#.####";
			grdOutPutByProduct.RootTable.Columns["RateUom"].TextAlignment = (TextAlignment)3;
			grdOutPutByProduct.RootTable.Columns["RateUom"].FormatString = "##,#.####";
			grdOutPutByProduct.RootTable.Columns["RateUom"].TotalFormatString = "##,#.####";
			grdOutPutByProduct.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)2;
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
			CommonServices.GridAutoAdjustmentNew(grdOutPutByProduct);
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOutPutByProduct_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdOutPutByProduct.UpdateData();
			GridEXRow item = grdOutPutByProduct.CurrentRow;
			if (e.Column.Key == "ItemId")
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
			if ((e.Column.Key == "ItemQty" || e.Column.Key == "Uom" || e.Column.Key == "ItemUomId") && item != null && (int)item.RowType == 0)
			{
				if (item.Cells["ItemQty"].Value.ToString() != string.Empty && item.Cells["ItemUomId"].Value.ToString() != string.Empty)
				{
					decimal num = Conversion.ToDecimal((object)item.Cells["ItemQty"].Value.ToString());
					decimal quom = Conversion.ToDecimal((object)item.Cells["Uom"].Value.ToString());
					decimal Weight2 = num * quom;
					item.Cells["NetWeight"].Value = Weight2;
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
				decimal Weight = default(decimal);
				decimal Iuom = default(decimal);
				if (!(item.Cells["ItemQty"].Value.ToString() != string.Empty) || !(item.Cells["ItemUomId"].Value.ToString() != string.Empty))
				{
					throw new Exception("ItemQty And ItemUom Should be greater than zero...");
				}
				decimal num3 = Conversion.ToDecimal((object)item.Cells["ItemQty"].Value.ToString());
				Iuom = Conversion.ToDecimal((object)item.Cells["Uom"].Value.ToString());
				Weight = num3 * Iuom;
				_ = Weight != 0m;
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
			grdOutPutByProduct.UpdateData();
			CommonServices.GridAutoAdjustmentNew(grdOutPutByProduct);
			Calculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOutPutByProduct_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow r = grdOutPutByProduct.CurrentRow;
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					BomByProductDetail d1 = new BomByProductDetail();
					d1.Id = Conversion.ToInt(r.Cells["Id"].Value);
					d1.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					d1.ItemUomId = Conversion.ToInt(r.Cells["ItemUomId"].Value);
					d1.ItemQty = Conversion.ToDecimal((object)r.Cells["ItemQty"].Text);
					d1.Weight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
					d1.ItemRate = Conversion.ToDecimal(r.Cells["Rate"].Value);
					d1.RateUomId = Conversion.ToInt(r.Cells["RateUomId"].Value);
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
			}
			if (e.Column.Key == "Add")
			{
				AddRowInOutPutByProductGrid();
			}
			grdOutPutByProduct.UpdateData();
			Calculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInOverHeadGrid()
	{
		dtOverHead.Rows.Add(0, 0, 0, 0, 0, 0, 0, "");
		grdOverHead.DataSource = dtOverHead;
	}

	private void grdOverHeadComboFill()
	{
		try
		{
			if (dtAccounts.Rows.Count > 0)
			{
				grdOverHead.RootTable.Columns["OverHeadAccountId"].ColumnType = (ColumnType)0;
				grdOverHead.RootTable.Columns["OverHeadAccountId"].LimitToList = true;
				grdOverHead.RootTable.Columns["OverHeadAccountId"].HasValueList = true;
				grdOverHead.RootTable.Columns["OverHeadAccountId"].EditType = (EditType)4;
				grdOverHead.RootTable.Columns["OverHeadAccountId"].Caption = "AccountTitle";
				grdOverHead.RootTable.Columns["OverHeadAccountId"].ValueList.PopulateValueList((IEnumerable)dtAccounts.DefaultView, "Id", "AccountTitle");
				grdOverHead.RootTable.Columns["OverHeadAccountId"].Width = 220;
			}
			grdOverHead.RootTable.Columns.Add("Delete");
			grdOverHead.RootTable.Columns["Delete"].Key = "Delete";
			grdOverHead.RootTable.Columns["Delete"].Caption = "X";
			grdOverHead.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOverHead.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdOverHead.RootTable.Columns["Delete"].ButtonText = "X";
			grdOverHead.RootTable.Columns["Delete"].Width = 40;
			grdOverHead.RootTable.Columns["Delete"].Position = 0;
			grdOverHead.RootTable.Columns.Add("Add");
			grdOverHead.RootTable.Columns["Add"].Key = "Add";
			grdOverHead.RootTable.Columns["Add"].Caption = "+";
			grdOverHead.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOverHead.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdOverHead.RootTable.Columns["Add"].ButtonText = "+";
			grdOverHead.RootTable.Columns["Add"].Width = 40;
			grdOverHead.RootTable.Columns["Add"].Position = 1;
			grdOverHead.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOverHeadGridSetting()
	{
		try
		{
			grdOverHeadComboFill();
			grdOverHead.RootTable.Columns["Id"].Visible = false;
			grdOverHead.RootTable.Columns["OverHeadTypeId"].Visible = false;
			grdOverHead.RootTable.Columns["PercentOfTotalInput"].Caption = "Percentage";
			grdOverHead.RootTable.Columns["Id"].EditType = (EditType)0;
			grdOverHead.RootTable.Columns["OverHeadTypeId"].EditType = (EditType)0;
			grdOverHead.RootTable.Columns["Qty"].EditType = (EditType)1;
			grdOverHead.RootTable.Columns["Rate"].EditType = (EditType)1;
			grdOverHead.RootTable.Columns["PercentOfTotalInput"].EditType = (EditType)1;
			grdOverHead.RootTable.Columns["Amount"].EditType = (EditType)1;
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
			CommonServices.GridAutoAdjustmentNew(grdOverHead);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
				GridEXRow[] rows = grdOverHead.GetRows();
				foreach (GridEXRow r in rows)
				{
					TotalPercentage += Conversion.ToDouble(r.Cells["PercentOfTotalInput"].Value);
				}
				if (TotalPercentage > 100.0)
				{
					double Percent = Conversion.ToDouble(grdOverHead.CurrentRow.Cells["PercentOfTotalInput"].Value);
					double i = TotalPercentage - 100.0;
					grdOverHead.CurrentRow.Cells["PercentOfTotalInput"].Value = Percent - i;
					MessageBox.Show("TotalPercentage Can not be Greater than 100...");
					return;
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
			CommonServices.GridAutoAdjustmentNew(grdOverHead);
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
		dtContractorWages.Rows.Add(0, 0, "", 0, 0, 0, "");
		grdContractorWages.DataSource = dtContractorWages;
	}

	private void grdContractorWagesComboFill()
	{
		try
		{
			grdContractorWages.RootTable.Columns.Add("Delete");
			grdContractorWages.RootTable.Columns["Delete"].Key = "Delete";
			grdContractorWages.RootTable.Columns["Delete"].Caption = "X";
			grdContractorWages.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdContractorWages.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdContractorWages.RootTable.Columns["Delete"].ButtonText = "X";
			grdContractorWages.RootTable.Columns["Delete"].Width = 40;
			grdContractorWages.RootTable.Columns["Delete"].Position = 0;
			grdContractorWages.RootTable.Columns.Add("Add");
			grdContractorWages.RootTable.Columns["Add"].Key = "Add";
			grdContractorWages.RootTable.Columns["Add"].Caption = "+";
			grdContractorWages.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdContractorWages.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdContractorWages.RootTable.Columns["Add"].ButtonText = "+";
			grdContractorWages.RootTable.Columns["Add"].Width = 40;
			grdContractorWages.RootTable.Columns["Add"].Position = 1;
			grdContractorWages.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdContractorWagesGridSetting()
	{
		try
		{
			grdContractorWagesComboFill();
			grdContractorWages.RootTable.Columns["Id"].Visible = false;
			grdContractorWages.RootTable.Columns["ContractorWagesRateScheduleId"].Visible = false;
			grdContractorWages.RootTable.Columns["Id"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["ContractorWagesRateScheduleId"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["Qty"].EditType = (EditType)1;
			grdContractorWages.RootTable.Columns["Rate"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["ServiceActivity"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["Amount"].EditType = (EditType)0;
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
			CommonServices.GridAutoAdjustmentNew(grdContractorWages);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			CommonServices.GridAutoAdjustmentNew(grdContractorWages);
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
			EnableDisableItemName();
			Calculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EnableDisableItemName()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdContractorWages.RowCount > 0)
			{
				GridEXRow r = grdContractorWages.GetRow(0);
				if (r != null && (int)r.RowType == 0 && Conversion.ToInt(r.Cells["ContractorWagesRateScheduleId"].Value) > 0)
				{
					((Control)(object)cmbItem).Enabled = false;
					((Control)(object)CmbProductionType).Enabled = false;
				}
				else
				{
					((Control)(object)cmbItem).Enabled = true;
					((Control)(object)CmbProductionType).Enabled = true;
				}
			}
			else
			{
				((Control)(object)cmbItem).Enabled = true;
				((Control)(object)CmbProductionType).Enabled = true;
			}
			labItemCaption.Text = Conversion.ToString((object)((Control)(object)cmbItem).Text) + "         you can Romove the following rows to Enable Production Type and Item ";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInOperationProcessGrid()
	{
		dtOperationProcess.Rows.Add(0, 0, "", "", 0, 0, 0, 0, "");
		grdOperatinalDetail.DataSource = dtOperationProcess;
	}

	private void grdOperationComboFill()
	{
		try
		{
			dtOperationcombo = WorkStationOperation.GetAllWorkStationOpertaion(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.BranchesId);
			grdOperatinalDetail.RootTable.Columns["WorkStationOperationId"].ColumnType = (ColumnType)0;
			grdOperatinalDetail.RootTable.Columns["WorkStationOperationId"].HasValueList = true;
			grdOperatinalDetail.RootTable.Columns["WorkStationOperationId"].LimitToList = true;
			grdOperatinalDetail.RootTable.Columns["WorkStationOperationId"].EditType = (EditType)4;
			grdOperatinalDetail.RootTable.Columns["WorkStationOperationId"].Caption = "WorkStationOperation";
			grdOperatinalDetail.RootTable.Columns["WorkStationOperationId"].ValueList.PopulateValueList((IEnumerable)dtOperationcombo.DefaultView, "Id", "Description");
			grdOperatinalDetail.RootTable.Columns["WorkStationOperationId"].Width = 220;
			grdOperatinalDetail.RootTable.Columns.Add("Add");
			grdOperatinalDetail.RootTable.Columns["Add"].Key = "Add";
			grdOperatinalDetail.RootTable.Columns["Add"].Caption = "+";
			grdOperatinalDetail.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOperatinalDetail.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdOperatinalDetail.RootTable.Columns["Add"].ButtonText = "+";
			grdOperatinalDetail.RootTable.Columns["Add"].Width = 40;
			grdOperatinalDetail.RootTable.Columns["Add"].Position = 0;
			grdOperatinalDetail.RootTable.Columns.Add("Delete");
			grdOperatinalDetail.RootTable.Columns["Delete"].Key = "Delete";
			grdOperatinalDetail.RootTable.Columns["Delete"].Caption = "X";
			grdOperatinalDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOperatinalDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdOperatinalDetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdOperatinalDetail.RootTable.Columns["Delete"].Width = 40;
			grdOperatinalDetail.RootTable.Columns["Delete"].Position = 0;
			grdOperatinalDetail.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdProcessSetting()
	{
		try
		{
			grdOperationComboFill();
			grdOperatinalDetail.RootTable.Columns["Id"].Visible = false;
			grdOperatinalDetail.RootTable.Columns["CapacityQty"].Visible = false;
			grdOperatinalDetail.RootTable.Columns["Id"].EditType = (EditType)0;
			grdOperatinalDetail.RootTable.Columns["CapacityQty"].EditType = (EditType)0;
			grdOperatinalDetail.RootTable.Columns["OperationName"].EditType = (EditType)0;
			grdOperatinalDetail.RootTable.Columns["WorkStationName"].EditType = (EditType)0;
			grdOperatinalDetail.RootTable.Columns["OperationCost"].EditType = (EditType)0;
			grdOperatinalDetail.RootTable.Columns["OperationHour"].EditType = (EditType)1;
			grdOperatinalDetail.RootTable.Columns["HourlyCost"].EditType = (EditType)1;
			grdOperatinalDetail.RootTable.Columns["OperationRemarks"].EditType = (EditType)1;
			grdOperatinalDetail.RootTable.Columns["CapacityQty"].AggregateFunction = (AggregateFunction)2;
			grdOperatinalDetail.RootTable.Columns["CapacityQty"].TextAlignment = (TextAlignment)3;
			grdOperatinalDetail.RootTable.Columns["CapacityQty"].FormatString = "##,#.####";
			grdOperatinalDetail.RootTable.Columns["CapacityQty"].TotalFormatString = "##,#.####";
			grdOperatinalDetail.RootTable.Columns["HourlyCost"].TextAlignment = (TextAlignment)3;
			grdOperatinalDetail.RootTable.Columns["HourlyCost"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdOperatinalDetail.RootTable.Columns["HourlyCost"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdOperatinalDetail.RootTable.Columns["OperationCost"].TextAlignment = (TextAlignment)3;
			grdOperatinalDetail.RootTable.Columns["OperationCost"].AggregateFunction = (AggregateFunction)2;
			grdOperatinalDetail.RootTable.Columns["OperationCost"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdOperatinalDetail.RootTable.Columns["OperationCost"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdOperatinalDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdProcess_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdOperatinalDetail.CurrentRow;
				dtOperationProcess.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtOperationProcess.Rows.Count == 0)
				{
					AddRowInOperationProcessGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInOperationProcessGrid();
			}
			grdOperatinalDetail.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdProcess_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdOperatinalDetail.UpdateData();
			GridEXRow item = grdOperatinalDetail.CurrentRow;
			if (e.Column.Key == "WorkStationOperationId")
			{
				int Id = Conversion.ToInt(item.Cells["WorkStationOperationId"].Value);
				if (Id > 0)
				{
					DataTable dt1 = WorkStationOperation.GetOpertaionByWorkStationOpertaionId(Id);
					DataTable workStationByWorkStationOpertaionId = WorkStationOperation.GetWorkStationByWorkStationOpertaionId(Id);
					if (dt1.Rows.Count > 0)
					{
						item.Cells["OperationName"].Value = Conversion.ToString(dt1.Rows[0]["Description"]);
					}
					if (workStationByWorkStationOpertaionId.Rows.Count > 0)
					{
						item.Cells["WorkStationName"].Value = Conversion.ToString(dt1.Rows[0]["Description"]);
					}
				}
			}
			if ((e.Column.Key == "OperationHour" || e.Column.Key == "HourlyCost") && item != null && (int)item.RowType == 0)
			{
				decimal Amount = default(decimal);
				decimal Iuom = default(decimal);
				if (!(item.Cells["OperationHour"].Value.ToString() != string.Empty) || !(item.Cells["HourlyCost"].Value.ToString() != string.Empty))
				{
					throw new Exception("OperationHour And Rate Should be greater than zero...");
				}
				decimal num = Conversion.ToDecimal((object)item.Cells["OperationHour"].Value.ToString());
				Iuom = Conversion.ToDecimal((object)item.Cells["HourlyCost"].Text.ToString());
				Amount = num * Iuom;
				item.Cells["OperationCost"].Value = Amount;
			}
			grdOperatinalDetail.UpdateData();
			CommonServices.GridAutoAdjustmentNew(grdOperatinalDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			txtBom.Text = string.Empty;
			txtEffectedDate.Value = DateTime.Now;
			((Control)(object)cmbBOMType).Text = string.Empty;
			((Control)(object)cmbItem).Text = string.Empty;
			((Control)(object)cmbUOM).Text = string.Empty;
			txtQty.Text = string.Empty;
			txtTopForce.Text = string.Empty;
			txtBottomForce.Text = string.Empty;
			lstRemoveRecordInput.Clear();
			lstRemoveRecordOutPut.Clear();
			dtInput.Rows.Clear();
			grdInputMaterial.ClearStructure();
			dtOverHead.Rows.Clear();
			grdOverHead.ClearStructure();
			dtContractorWages.Rows.Clear();
			grdContractorWages.ClearStructure();
			dtOutPutByProduct.Rows.Clear();
			grdOutPutByProduct.ClearStructure();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			UpdateMode = false;
			AddRowInInputGrid();
			grdInputMaterial.RetrieveStructure();
			grdInputSettings();
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
			txtLessByProduct.Text = string.Empty;
			txtNetValue.Text = string.Empty;
			txtValuePerQty.Text = string.Empty;
			EnableDisableItemName();
			txtdocdate.Focus();
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
			ItemFill();
			uomgetAll();
			AccountTitleFill();
			ProductionTypeFill();
			PlantFeader();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmBillOfMaterialConcrete_FormClosed(object sender, FormClosedEventArgs e)
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
			if (grdInputMaterial.RowCount > 0)
			{
				double sumoftotalInputs = (from GridEXRow row in grdInputMaterial.GetRows()
					where Conversion.ToInt(row.Cells["MainItemId"].Value) == 0
					select Conversion.ToDouble(row.Cells["Amount"].Value)).Sum();
				string sumoftotalOutputs = grdOutPutByProduct.GetTotal(grdOutPutByProduct.RootTable.Columns["Amount"], (AggregateFunction)2).ToString();
				string sumofOverhead = grdOverHead.GetTotal(grdOverHead.RootTable.Columns["Amount"], (AggregateFunction)2).ToString();
				string sumofContractorWages = grdContractorWages.GetTotal(grdContractorWages.RootTable.Columns["Amount"], (AggregateFunction)2).ToString();
				txtTotalInputs.Text = Math.Round(sumoftotalInputs, 2).ToString();
				txtOverHead.Text = Math.Round(Conversion.ToDouble((object)sumofOverhead), 2).ToString();
				txtWagesAmount.Text = Math.Round(Conversion.ToDouble((object)sumofContractorWages), 2).ToString();
				txtLessByProduct.Text = Math.Round(Conversion.ToDouble((object)sumoftotalOutputs), 2).ToString();
				double NetValue = sumoftotalInputs + Conversion.ToDouble((object)sumofOverhead) + Conversion.ToDouble((object)sumofContractorWages) - Conversion.ToDouble((object)sumoftotalOutputs);
				txtNetValue.Text = NetValue.ToString();
				txtValuePerQty.Text = Math.Round(NetValue / Conversion.ToDouble((object)txtQty.Text), 2).ToString();
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
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Expected O, but got Unknown
		//IL_0a25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2c: Expected O, but got Unknown
		//IL_0f84: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f8b: Expected O, but got Unknown
		//IL_1180: Unknown result type (might be due to invalid IL or missing references)
		//IL_1187: Expected O, but got Unknown
		//IL_131e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1325: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			if (grdInputMaterial.GetRows().Count() == 0)
			{
				MessageBox.Show("Grid Record Not Found...\nGrid Input Should have atleast one record");
				return;
			}
			BomHeader bOMHeader = new BomHeader();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				bOMHeader.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			bOMHeader.CompanyId = UserAccount.CompanyId;
			bOMHeader.BranchesId = UserAccount.BranchesId;
			bOMHeader.ProjectsId = UserAccount.BranchesId;
			bOMHeader.OrganizationId = UserAccount.OrganizationId;
			bOMHeader.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			bOMHeader.ProductionTypeId = Conversion.ToInt(CmbProductionType.Value);
			bOMHeader.PlantId = Conversion.ToInt(CmbPlantName.Value);
			bOMHeader.ItemId = Conversion.ToInt(cmbItem.Value);
			bOMHeader.ItemUomId = Conversion.ToInt(cmbUOM.Value);
			bOMHeader.BomName = txtBom.Text;
			bOMHeader.BomType = ((Control)(object)cmbBOMType).Text;
			bOMHeader.BomLevelType = ((Control)(object)cmbBOMType).Text;
			bOMHeader.EffectedDate = txtEffectedDate.Value;
			bOMHeader.BomBatchQty = Conversion.ToDecimal((object)txtQty.Text);
			bOMHeader.DocumentTypeId = 1851;
			bOMHeader.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
			bOMHeader.DocNo = Conversion.ToInt((object)txtdocno.Text);
			bOMHeader.RemarksHeader = Conversion.ToString((object)txtRemarksHeader.Text);
			bOMHeader.IsActive = ChkIsActive.Checked;
			bOMHeader.IsDefault = chkIsDefault.Checked;
			bOMHeader.EntryUserId = UserAccount.ID;
			bOMHeader.EntryDate = DateTime.Now;
			bOMHeader.ModifyDate = DateTime.Now;
			bOMHeader.ModifyUserId = UserAccount.ID;
			bOMHeader.ApprovedUserId = UserAccount.ID;
			bOMHeader.ApprovedDate = DateTime.Now;
			bOMHeader.IsApproved = false;
			bOMHeader.TopForce = Conversion.ToDecimal((object)txtTopForce.Text);
			bOMHeader.BottomForce = Conversion.ToDecimal((object)txtBottomForce.Text);
			bOMHeader.BomInputDetailList = new List<BomInputDetail>();
			bOMHeader.BomFohDetailList = new List<BomFohDetail>();
			bOMHeader.BomByProductDetailList = new List<BomByProductDetail>();
			bOMHeader.BomOperationDetailList = new List<BomOperationDetail>();
			bOMHeader.BomWagesDetailList = new List<BomWagesDetail>();
			bOMHeader.AttachmentsList = new List<DMSAttachments>();
			GridEXRow[] rows = grdInputMaterial.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				if (Conversion.ToInt((object)r3.Cells["ItemId"].Value.ToString()) != 0)
				{
					BomInputDetail d3 = new BomInputDetail();
					d3.Id = Conversion.ToInt(r3.Cells["Id"].Value);
					if (d3.Id > 0)
					{
						d3.ActionTypeId = 2;
					}
					else
					{
						d3.ActionTypeId = 1;
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
					if (r3.Cells["NetWeight"].Text == string.Empty)
					{
						MessageBox.Show("NetWeight Field Required in Grid Input Row No: " + (r3.RowIndex + 1));
						return;
					}
					d3.WeightKg = Conversion.ToDecimal(r3.Cells["NetWeight"].Value);
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
						DataRow[] dr3 = dt2.Select("Id='" + Conversion.ToInt(r3.Cells["RateUomId"].Value) + "'");
						if (dr3.Length == 0)
						{
							MessageBox.Show("RateUom in Input Grid Row No: " + (r3.RowIndex + 1) + " Is not Define Against Item " + Conversion.ToString((object)r3.Cells["ItemId"].Text) + ". Please Check...");
							return;
						}
						d3.RateUomId = Conversion.ToInt(dr3[0].ItemArray[0]);
					}
					if (r3.Cells["Amount"].Text == string.Empty)
					{
						MessageBox.Show("AmountField Required in Grid Input Row No: " + (r3.RowIndex + 1));
						return;
					}
					d3.Amount = Conversion.ToDecimal(r3.Cells["Amount"].Value);
					d3.MinQty = Conversion.ToDecimal(r3.Cells["MinQty"].Value);
					d3.MaxQty = Conversion.ToDecimal(r3.Cells["MaxQty"].Value);
					d3.MainItemId = Conversion.ToInt(r3.Cells["MainItemId"].Value);
					d3.MainItemUomId = Conversion.ToInt(r3.Cells["MainItemUomId"].Value);
					if (d3.MinQty > 0m && d3.MinQty > d3.ItemQty)
					{
						throw new Exception($"MinQty Should be less than or equal to ItemQty... in Input Grid Row No : {r3.RowIndex + 1}");
					}
					d3.MinQty = d3.ItemQty;
					if (d3.MaxQty > 0m && d3.MaxQty < d3.ItemQty)
					{
						throw new Exception($"MaxQty Should be greater than or equal to ItemQty... in Input Grid Row No : {r3.RowIndex + 1}");
					}
					d3.MaxQty = d3.ItemQty;
					if (d3.MainItemId > 0 && d3.MainItemId == d3.ItemId)
					{
						throw new Exception($"MainItem Can't be Same as Item in Input Grid Row No : {r3.RowIndex + 1}");
					}
					if (d3.MainItemId > 0 && d3.MainItemUomId == 0)
					{
						throw new Exception($"MainItemUom required when MainItem is Selected in Input Grid Row No : {r3.RowIndex + 1}");
					}
					bOMHeader.BomInputDetailList.Add(d3);
					continue;
				}
				MessageBox.Show("Item Field Required in Grid Input Row No: " + (r3.RowIndex + 1));
				return;
			}
			if (lstRemoveRecordInput.Count > 0)
			{
				for (int k = 0; k < lstRemoveRecordInput.Count; k++)
				{
					bOMHeader.BomInputDetailList.Add(lstRemoveRecordInput[k]);
				}
			}
			rows = grdOutPutByProduct.GetRows();
			foreach (GridEXRow r4 in rows)
			{
				if (Conversion.ToInt((object)r4.Cells["ItemId"].Value.ToString()) == 0)
				{
					continue;
				}
				BomByProductDetail d4 = new BomByProductDetail();
				d4.Id = Conversion.ToInt(r4.Cells["Id"].Value);
				if (d4.Id > 0)
				{
					d4.ActionTypeId = 2;
				}
				else
				{
					d4.ActionTypeId = 1;
				}
				if (r4.Cells["ItemId"].Text == string.Empty)
				{
					MessageBox.Show("Item Field Required in Grid Input Row No: " + (r4.RowIndex + 1));
					return;
				}
				d4.ItemId = Conversion.ToInt(r4.Cells["ItemId"].Value);
				if (r4.Cells["ItemUomId"].Text == string.Empty)
				{
					MessageBox.Show("UOM  Field Required in Grid Input Row No: " + (r4.RowIndex + 1));
					return;
				}
				DataTable dt4 = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r4.Cells["ItemId"].Value));
				if (dt4.Rows.Count > 0)
				{
					DataRow[] dr4 = dt4.Select("Id='" + Conversion.ToInt(r4.Cells["ItemUomId"].Value) + "'");
					if (dr4.Length == 0)
					{
						MessageBox.Show("UOM in Output Grid Row No: " + (r4.RowIndex + 1) + " Is not Define Against Item " + Conversion.ToString((object)r4.Cells["ItemId"].Text) + ". Please Check...");
						return;
					}
					d4.ItemUomId = Conversion.ToInt(dr4[0].ItemArray[0]);
				}
				if (r4.Cells["ItemQty"].Text == string.Empty)
				{
					MessageBox.Show("ItemQty Field Required in Grid Input Row No: " + (r4.RowIndex + 1));
					return;
				}
				d4.ItemQty = Conversion.ToDecimal((object)r4.Cells["ItemQty"].Text);
				if (r4.Cells["NetWeight"].Text == string.Empty)
				{
					MessageBox.Show("NetWeight Field Required in Grid Input Row No: " + (r4.RowIndex + 1));
					return;
				}
				d4.Weight = Conversion.ToDecimal(r4.Cells["NetWeight"].Value);
				if (r4.Cells["Rate"].Text == string.Empty)
				{
					MessageBox.Show("Rate Field Required in Grid Output Row No: " + (r4.RowIndex + 1));
					return;
				}
				d4.ItemRate = Conversion.ToDecimal(r4.Cells["Rate"].Value);
				if (r4.Cells["RateUomId"].Text == string.Empty)
				{
					MessageBox.Show("Rate UOM Field Required in Grid Output Row No: " + (r4.RowIndex + 1));
					return;
				}
				DataTable dt3 = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r4.Cells["ItemId"].Value));
				if (dt3.Rows.Count > 0)
				{
					DataRow[] dr2 = dt3.Select("Id='" + Conversion.ToInt(r4.Cells["RateUomId"].Value) + "'");
					if (dr2.Length == 0)
					{
						MessageBox.Show("RateUom in output Grid Row No: " + (r4.RowIndex + 1) + " Is not Define Against Item " + Conversion.ToString((object)r4.Cells["ItemId"].Text) + ". Please Check...");
						return;
					}
					d4.RateUomId = Conversion.ToInt(dr2[0].ItemArray[0]);
				}
				if (r4.Cells["Amount"].Text == string.Empty)
				{
					MessageBox.Show("AmountField Required in Grid Output Row No: " + (r4.RowIndex + 1));
					return;
				}
				d4.Amount = Conversion.ToDecimal(r4.Cells["Amount"].Value);
				bOMHeader.BomByProductDetailList.Add(d4);
			}
			if (lstRemoveRecordOutPut.Count > 0)
			{
				for (int j = 0; j < lstRemoveRecordOutPut.Count; j++)
				{
					bOMHeader.BomByProductDetailList.Add(lstRemoveRecordOutPut[j]);
				}
			}
			rows = grdOverHead.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if (Conversion.ToInt(r2.Cells["OverHeadAccountId"].Value) > 0)
				{
					BomFohDetail d2 = new BomFohDetail();
					if (r2.Cells["OverHeadAccountId"].Text == string.Empty)
					{
						MessageBox.Show("OverHeadAccount Field Required in Grid Over Head Row No: " + (r2.RowIndex + 1));
						return;
					}
					d2.Id = Conversion.ToInt(r2.Cells["Id"].Value);
					d2.OverHeadTypeId = Conversion.ToInt(r2.Cells["OverHeadAccountId"].Value);
					d2.GLAccountId = Conversion.ToInt(r2.Cells["OverHeadAccountId"].Value);
					if (r2.Cells["Amount"].Text == string.Empty)
					{
						MessageBox.Show("Amount Field Required in Grid Over Head Row No: " + (r2.RowIndex + 1));
						return;
					}
					d2.PercentOfTotalInput = Conversion.ToDecimal((object)r2.Cells["PercentOfTotalInput"].Text);
					d2.Qty = Conversion.ToDecimal((object)r2.Cells["Qty"].Text);
					d2.Rate = Conversion.ToDecimal((object)r2.Cells["Rate"].Text);
					d2.Amount = Conversion.ToDecimal((object)r2.Cells["Amount"].Text);
					d2.OhRemarks = Conversion.ToString((object)r2.Cells["OhRemarks"].Text);
					bOMHeader.BomFohDetailList.Add(d2);
				}
			}
			rows = grdContractorWages.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt(r.Cells["ContractorWagesRateScheduleId"].Value) > 0)
				{
					BomWagesDetail d = new BomWagesDetail();
					if (r.Cells["ContractorWagesRateScheduleId"].Text == string.Empty)
					{
						MessageBox.Show("Service Activity Field Required in Grid Contractor Wages Row No: " + (r.RowIndex + 1));
						return;
					}
					d.Id = Conversion.ToInt(r.Cells["Id"].Value);
					d.ContractorWagesRateScheduleId = Conversion.ToInt(r.Cells["ContractorWagesRateScheduleId"].Value);
					d.ItemId = Conversion.ToInt(cmbItem.Value);
					if (r.Cells["Amount"].Text == string.Empty)
					{
						MessageBox.Show("Amount Field Required in Grid Contractor Wages Row No: " + (r.RowIndex + 1));
						return;
					}
					d.ItemQty = Conversion.ToDecimal((object)r.Cells["Qty"].Text);
					d.Rate = Conversion.ToDecimal((object)r.Cells["Rate"].Text);
					d.Amount = Conversion.ToDecimal((object)r.Cells["Amount"].Text);
					d.Remarks = Conversion.ToString((object)r.Cells["Remarks"].Text);
					bOMHeader.BomWagesDetailList.Add(d);
				}
			}
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Path.GetFileName(Attachment.dtAttachments.Rows[i][1].ToString());
				attach.ScreenName = base.Name;
				bOMHeader.AttachmentsList.Add(attach);
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
			int success = BomHeader.Save(bOMHeader);
			bOMHeader.Id = success;
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + bOMHeader.DocNo);
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
				MessageBox.Show("Data Save Successfully....  " + bOMHeader.DocNo);
			}
			FromReset();
			if (ChkPreview.Checked && formright.DoHavePrintRights)
			{
				BOMSlip(bOMHeader.Id);
			}
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message);
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
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		try
		{
			dtInput.Clear();
			grdInputMaterial.ClearStructure();
			dtOutPutByProduct.Clear();
			grdOutPutByProduct.ClearStructure();
			dtOverHead.Clear();
			grdOverHead.ClearStructure();
			dtContractorWages.Clear();
			grdContractorWages.ClearStructure();
			dtOperationProcess.Clear();
			grdOperatinalDetail.ClearStructure();
			BomHeader bOMHeader = new BomHeader();
			RecId = Id;
			bOMHeader = BomHeader.ReadById(RecId);
			if (bOMHeader == null)
			{
				return;
			}
			tabControl1.SelectedIndex = 0;
			tabControl1.SelectedIndex = 0;
			txtdocdate.Value = bOMHeader.DocDate;
			txtdocno.Text = Conversion.ToString((object)bOMHeader.DocNo);
			txtRemarksHeader.Text = Conversion.ToString((object)bOMHeader.RemarksHeader);
			CmbPlantName.Value = bOMHeader.PlantId;
			CmbProductionType.Value = bOMHeader.ProductionTypeId;
			CmbProductionType_Leave(null, null);
			cmbItem.Value = bOMHeader.ItemId;
			cmbItem_Leave(null, null);
			cmbUOM.Value = bOMHeader.ItemUomId;
			txtBom.Text = bOMHeader.BomName;
			((Control)(object)cmbBOMType).Text = bOMHeader.BomType;
			txtEffectedDate.Value = bOMHeader.EffectedDate;
			txtQty.Text = bOMHeader.BomBatchQty.ToString();
			ChkIsActive.Checked = bOMHeader.IsActive;
			chkIsDefault.Checked = bOMHeader.IsDefault;
			txtTopForce.Text = bOMHeader.TopForce.ToString();
			txtBottomForce.Text = bOMHeader.BottomForce.ToString();
			if (bOMHeader.BomInputDetailList.Count > 0)
			{
				for (int l = 0; l < bOMHeader.BomInputDetailList.Count; l++)
				{
					dtInput.Rows.Add(bOMHeader.BomInputDetailList[l].Id, bOMHeader.BomInputDetailList[l].ItemId, bOMHeader.BomInputDetailList[l].ItemName, bOMHeader.BomInputDetailList[l].ItemUomId, bOMHeader.BomInputDetailList[l].PackUomEquivalent, bOMHeader.BomInputDetailList[l].PackUom, bOMHeader.BomInputDetailList[l].ItemQty, bOMHeader.BomInputDetailList[l].WeightKg, bOMHeader.BomInputDetailList[l].Rate, bOMHeader.BomInputDetailList[l].RateUomId, bOMHeader.BomInputDetailList[l].RateUomEquivalent, bOMHeader.BomInputDetailList[l].RateUom, bOMHeader.BomInputDetailList[l].Amount, bOMHeader.BomInputDetailList[l].MinQty, bOMHeader.BomInputDetailList[l].MaxQty, bOMHeader.BomInputDetailList[l].MainItemId, bOMHeader.BomInputDetailList[l].MainItemName, bOMHeader.BomInputDetailList[l].MainItemUomId, bOMHeader.BomInputDetailList[l].MainPackUom);
				}
			}
			else
			{
				AddRowInInputGrid();
			}
			if (bOMHeader.BomByProductDetailList.Count > 0)
			{
				for (int k = 0; k < bOMHeader.BomByProductDetailList.Count; k++)
				{
					dtOutPutByProduct.Rows.Add(bOMHeader.BomByProductDetailList[k].Id, bOMHeader.BomByProductDetailList[k].ItemId, bOMHeader.BomByProductDetailList[k].ItemName, bOMHeader.BomByProductDetailList[k].ItemUomId, bOMHeader.BomByProductDetailList[k].PackUomEquivalent, bOMHeader.BomByProductDetailList[k].PackUom, bOMHeader.BomByProductDetailList[k].ItemQty, bOMHeader.BomByProductDetailList[k].Weight, bOMHeader.BomByProductDetailList[k].ItemRate, bOMHeader.BomByProductDetailList[k].RateUomId, bOMHeader.BomByProductDetailList[k].RateUomEquivalent, bOMHeader.BomByProductDetailList[k].RateUom, bOMHeader.BomByProductDetailList[k].Amount);
				}
			}
			else
			{
				AddRowInOutPutByProductGrid();
			}
			if (bOMHeader.BomFohDetailList.Count > 0)
			{
				for (int j = 0; j < bOMHeader.BomFohDetailList.Count; j++)
				{
					dtOverHead.Rows.Add(bOMHeader.BomFohDetailList[j].Id, bOMHeader.BomFohDetailList[j].OverHeadTypeId, bOMHeader.BomFohDetailList[j].GLAccountId, bOMHeader.BomFohDetailList[j].PercentOfTotalInput, bOMHeader.BomFohDetailList[j].Qty, bOMHeader.BomFohDetailList[j].Rate, bOMHeader.BomFohDetailList[j].Amount, bOMHeader.BomFohDetailList[j].OhRemarks);
				}
			}
			else
			{
				AddRowInOverHeadGrid();
			}
			if (bOMHeader.BomWagesDetailList.Count > 0)
			{
				for (int i = 0; i < bOMHeader.BomWagesDetailList.Count; i++)
				{
					dtContractorWages.Rows.Add(bOMHeader.BomWagesDetailList[i].Id, bOMHeader.BomWagesDetailList[i].ContractorWagesRateScheduleId, bOMHeader.BomWagesDetailList[i].ServiceActivity, bOMHeader.BomWagesDetailList[i].ItemQty, bOMHeader.BomWagesDetailList[i].Rate, bOMHeader.BomWagesDetailList[i].Amount, bOMHeader.BomWagesDetailList[i].Remarks);
				}
			}
			else
			{
				AddRowInContractorWagesGrid();
			}
			_ = bOMHeader.BomOperationDetailList.Count;
			_ = 0;
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
			UpdateMode = true;
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
			List<DMSAttachments> byID = DMSAttachments.GetByID(RecId, base.Name);
			AT = new Attachment();
			foreach (DMSAttachments Attachment in byID)
			{
				TempAttachments TA = new TempAttachments();
				TA.Id = 0;
				TA.Path = Attachment.Attachment;
				AT.lst.Add(TA);
			}
			Calculation();
			EnableDisableItemName();
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
					BomHeader.DeleteByID(UserAccount.ID, RecId);
					MessageBox.Show("Delete Record Seccessfully");
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
			BOMSlip(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BOMSlip(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.Id = PrintId;
			DataTable dt = BomHeader.BillOfMaterial_SlipAndRegister(obj);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(BomHeader.BillOfMaterialAllDetails_SubReport(obj.Id), "BillOfMaterialAllDetails_SubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1851_USP_BillOfMaterial_Slip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllCombobind()
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		try
		{
			string Idtype = Conversion.ToString(cmbbomtypeHistory.Value);
			int Idname = Conversion.ToInt(CmbBomNoHistory.Value);
			int Iditem = Conversion.ToInt(cmbItemhistory.Value);
			DataTable dtcombo = BomHeader.GetDataForDropDownFromBOM(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtcombo.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtBOMType = new DataTable();
			dtBOMType.Columns.Add("Id");
			dtBOMType.Columns.Add("BOMType");
			DataTable dtBOMName = new DataTable();
			dtBOMName.Columns.Add("Id", typeof(int));
			dtBOMName.Columns.Add("BOMName");
			DataTable dtitem = new DataTable();
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName");
			for (int i = 0; i < dtcombo.Rows.Count; i++)
			{
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "BOMType")
				{
					dtBOMType.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "BOMName")
				{
					dtBOMName.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "ItemHeader")
				{
					dtitem.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
			}
			if (dtBOMType.Rows.Count > 0)
			{
				DDL.BindDDL(dtBOMType, cmbbomtypeHistory, "Id", "BOMType", "BOM Type", ZeroIndex: true);
				((UltraGridBase)cmbbomtypeHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (Idtype != null && Idtype != "" && Idtype != "0")
				{
					if (dtitem.AsEnumerable().Any((DataRow row) => row.Field<string>("Id") == Idtype))
					{
						((Control)(object)cmbbomtypeHistory).Text = Idtype;
					}
					else
					{
						((Control)(object)cmbbomtypeHistory).Text = string.Empty;
					}
				}
			}
			if (dtBOMName.Rows.Count > 0)
			{
				DDL.BindDDL(dtBOMName, CmbBomNoHistory, "Id", "BOMName", "BOM Name", ZeroIndex: true);
				((UltraGridBase)CmbBomNoHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (Idname > 0)
				{
					if (dtitem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Idname))
					{
						CmbBomNoHistory.Value = Idname;
					}
					else
					{
						((Control)(object)CmbBomNoHistory).Text = string.Empty;
					}
				}
			}
			if (dtitem.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtitem, cmbItemhistory, "Id", "ItemName", "Item Name", ZeroIndex: true);
			((UltraGridBase)cmbItemhistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (Iditem > 0)
			{
				if (dtitem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Iditem))
				{
					cmbItemhistory.Value = Iditem;
				}
				else
				{
					((Control)(object)cmbItemhistory).Text = string.Empty;
				}
			}
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
			DataTable dt = CommonServices.DateType();
			DDL.BindDDL(dt, cmbDateTypeHistory, "Id", "Parameters", "Parameters", ZeroIndex: true);
			((UltraGridBase)cmbDateTypeHistory).Rows[2].Activate();
			((UltraGridBase)cmbDateTypeHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
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
			header.DocumentTypeId = 1851;
			header.FromDocNo = Conversion.ToInt((object)txtFromNoHistory.Text.Trim());
			header.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text.Trim());
			header.FromDate = Conversion.ToDateTime((object)txtFromdateHistory.Value);
			header.ToDate = Conversion.ToDateTime((object)txtToDateHistory.Value);
			if (Conversion.ToInt(cmbbomtypeHistory.Value) > 0)
			{
				header.ProductionNo = Conversion.ToString((object)((Control)(object)cmbbomtypeHistory).Text);
			}
			header.Id = Conversion.ToInt(CmbBomNoHistory.Value);
			header.ItemId = Conversion.ToInt(cmbItemhistory.Value);
			header.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			dtHistory = BomHeader.FormHistory(header);
			if (dtHistory.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtcol = new DataTable();
			dtcol.Columns.Add("RecordNo", typeof(int));
			dtcol.Columns.Add("Id", typeof(int));
			dtcol.Columns.Add("DocumentTypeId", typeof(int));
			dtcol.Columns.Add("DocNo", typeof(int));
			dtcol.Columns.Add("DocDate", typeof(DateTime));
			dtcol.Columns.Add("BomName", typeof(string));
			dtcol.Columns.Add("BomType", typeof(string));
			dtcol.Columns.Add("PlantName", typeof(string));
			dtcol.Columns.Add("FinishType", typeof(string));
			dtcol.Columns.Add("ItemName", typeof(string));
			dtcol.Columns.Add("Uom", typeof(string));
			dtcol.Columns.Add("BomBatchQty", typeof(decimal));
			dtcol.Columns.Add("EffectedDate", typeof(DateTime));
			dtcol.Columns.Add("IsActive", typeof(bool));
			dtcol.Columns.Add("IsDefault", typeof(bool));
			dtcol.Columns.Add("RemarksHeader", typeof(string));
			dtcol.Columns.Add("EntryUser", typeof(string));
			dtcol.Columns.Add("EntryDate", typeof(DateTime));
			dtcol.Columns.Add("ModifyUser", typeof(string));
			dtcol.Columns.Add("ModifyDate", typeof(DateTime));
			dtcol.Columns.Add("IsApproved", typeof(bool));
			dtcol.Columns.Add("NoOfAttachments", typeof(int));
			dtcol.Rows.Clear();
			foreach (DataRow row in dtHistory.Rows)
			{
				dtcol.Rows.Add(row["RecordNo"], row["Id"], row["DocumentTypeId"], row["DocNo"], row["DocDate"], row["BomName"], row["ProductionType"], row["PlantName"], row["BomType"], row["ItemName"], row["Uom"], row["BomBatchQty"], row["EffectedDate"], row["IsActive"], row["IsDefault"], row["RemarksHeader"], row["EntryUserName"], row["EntryDate"], row["ModifyUserName"], row["ModifyDate"], row["IsApproved"], row["NoOfAttachments"]);
			}
			DatagridHistory.DataSource = dtcol;
			DatagridHistory.RetrieveStructure();
			HistoryGridSetting();
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
			DatagridHistory.RootTable.Columns["Id"].Visible = false;
			DatagridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DatagridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DatagridHistory.RootTable.Columns["BomBatchQty"].AggregateFunction = (AggregateFunction)2;
			DatagridHistory.RootTable.Columns["BomBatchQty"].TextAlignment = (TextAlignment)3;
			DatagridHistory.RootTable.Columns["BomBatchQty"].FormatString = "##,#.####";
			DatagridHistory.RootTable.Columns["BomBatchQty"].TotalFormatString = "##,#.####";
			CommonServices.GridAutoAdjustmentNew(DatagridHistory);
			DatagridHistory.RootTable.Columns["IsActive"].Width = 40;
			DatagridHistory.RootTable.Columns["IsActive"].ColumnType = (ColumnType)3;
			DatagridHistory.RootTable.Columns["IsActive"].CheckBoxTrueValue = "True";
			DatagridHistory.RootTable.Columns["IsActive"].CheckBoxFalseValue = "False";
			DatagridHistory.RootTable.Columns.Add("Print");
			DatagridHistory.RootTable.Columns["Print"].Key = "Print";
			DatagridHistory.RootTable.Columns["Print"].Caption = "Print";
			DatagridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DatagridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DatagridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DatagridHistory.RootTable.Columns["Print"].Width = 50;
			DatagridHistory.RootTable.Columns["Print"].Position = 0;
			DatagridHistory.RootTable.Columns.Add("Edit");
			DatagridHistory.RootTable.Columns["Edit"].Key = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DatagridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DatagridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].Width = 50;
			DatagridHistory.RootTable.Columns["Edit"].Position = 0;
			DatagridHistory.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			if (formright.DoHaveUpdateRights)
			{
				ReadById(Conversion.ToInt((object)DatagridHistory.CurrentRow.Cells["Id"].Value.ToString()));
				return;
			}
			throw new Exception("ypu don't have updae rights...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Print")
			{
				BOMSlip(Conversion.ToInt((object)DatagridHistory.CurrentRow.Cells["Id"].Value.ToString()));
			}
			if (e.Column.Key == "Edit")
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("ypu don't have updae rights...");
				}
				ReadById(Conversion.ToInt((object)DatagridHistory.CurrentRow.Cells["Id"].Value.ToString()));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DatagridHistory.CurrentRow.Cells["Id"].Value), 1851);
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
			BomHeader bOMHeader = new BomHeader();
			bOMHeader = BomHeader.ReadById(Id);
			if (bOMHeader == null)
			{
				return;
			}
			DataTable dtInputHistory = new DataTable();
			DataTable dtOutPutByProductHistory = new DataTable();
			DataTable dtOverHeadHistory = new DataTable();
			DataTable dtWagesHistory = new DataTable();
			new DataTable();
			dtInputHistory.Columns.Add("Id", typeof(int));
			dtInputHistory.Columns.Add("Item", typeof(string));
			dtInputHistory.Columns.Add("Uom", typeof(string));
			dtInputHistory.Columns.Add("ItemQty", typeof(decimal));
			dtInputHistory.Columns.Add("NetWeight", typeof(decimal));
			dtInputHistory.Columns.Add("Rate", typeof(decimal));
			dtInputHistory.Columns.Add("RateUom", typeof(string));
			dtInputHistory.Columns.Add("Amount", typeof(decimal));
			dtInputHistory.Columns.Add("MinQty", typeof(decimal));
			dtInputHistory.Columns.Add("MaxQty", typeof(decimal));
			dtInputHistory.Columns.Add("MainItemName", typeof(string));
			dtOutPutByProductHistory.Columns.Add("Id", typeof(int));
			dtOutPutByProductHistory.Columns.Add("Item", typeof(string));
			dtOutPutByProductHistory.Columns.Add("Uom", typeof(string));
			dtOutPutByProductHistory.Columns.Add("ItemQty", typeof(decimal));
			dtOutPutByProductHistory.Columns.Add("NetWeight", typeof(decimal));
			dtOutPutByProductHistory.Columns.Add("Rate", typeof(decimal));
			dtOutPutByProductHistory.Columns.Add("RateUom", typeof(string));
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
			dtWagesHistory.Columns.Add("ItemName");
			dtWagesHistory.Columns.Add("ServiceActivity");
			dtWagesHistory.Columns.Add("Qty", typeof(decimal));
			dtWagesHistory.Columns.Add("Rate", typeof(decimal));
			dtWagesHistory.Columns.Add("Amount", typeof(decimal));
			dtWagesHistory.Columns.Add("Remarks");
			if (bOMHeader.BomInputDetailList.Count > 0)
			{
				for (int l = 0; l < bOMHeader.BomInputDetailList.Count; l++)
				{
					dtInputHistory.Rows.Add(bOMHeader.BomInputDetailList[l].Id, bOMHeader.BomInputDetailList[l].ItemName, bOMHeader.BomInputDetailList[l].PackUom, bOMHeader.BomInputDetailList[l].ItemQty, bOMHeader.BomInputDetailList[l].WeightKg, bOMHeader.BomInputDetailList[l].Rate, bOMHeader.BomInputDetailList[l].RateUom, bOMHeader.BomInputDetailList[l].Amount, bOMHeader.BomInputDetailList[l].MinQty, bOMHeader.BomInputDetailList[l].MaxQty, bOMHeader.BomInputDetailList[l].MainItemName);
				}
			}
			else
			{
				grdHistoryInput.ClearStructure();
			}
			if (bOMHeader.BomByProductDetailList.Count > 0)
			{
				for (int k = 0; k < bOMHeader.BomByProductDetailList.Count; k++)
				{
					dtOutPutByProductHistory.Rows.Add(bOMHeader.BomByProductDetailList[k].Id, bOMHeader.BomByProductDetailList[k].ItemName, bOMHeader.BomByProductDetailList[k].PackUom, bOMHeader.BomByProductDetailList[k].ItemQty, bOMHeader.BomByProductDetailList[k].Weight, bOMHeader.BomByProductDetailList[k].ItemRate, bOMHeader.BomByProductDetailList[k].RateUom, bOMHeader.BomByProductDetailList[k].Amount);
				}
			}
			else
			{
				grdHistoryOutput.ClearStructure();
			}
			if (bOMHeader.BomFohDetailList.Count > 0)
			{
				for (int j = 0; j < bOMHeader.BomFohDetailList.Count; j++)
				{
					dtOverHeadHistory.Rows.Add(bOMHeader.BomFohDetailList[j].Id, bOMHeader.BomFohDetailList[j].OverHeadTypeId, bOMHeader.BomFohDetailList[j].OverHeadAccountTitle, bOMHeader.BomFohDetailList[j].PercentOfTotalInput, bOMHeader.BomFohDetailList[j].Qty, bOMHeader.BomFohDetailList[j].Rate, bOMHeader.BomFohDetailList[j].Amount, bOMHeader.BomFohDetailList[j].OhRemarks);
				}
			}
			else
			{
				grdHistoryOverHead.ClearStructure();
			}
			if (bOMHeader.BomWagesDetailList.Count > 0)
			{
				for (int i = 0; i < bOMHeader.BomWagesDetailList.Count; i++)
				{
					dtWagesHistory.Rows.Add(bOMHeader.BomWagesDetailList[i].Id, bOMHeader.BomWagesDetailList[i].ItemName, bOMHeader.BomWagesDetailList[i].ServiceActivity, bOMHeader.BomWagesDetailList[i].ItemQty, bOMHeader.BomWagesDetailList[i].Rate, bOMHeader.BomWagesDetailList[i].Amount, bOMHeader.BomWagesDetailList[i].Remarks);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryInputSetting()
	{
		try
		{
			grdHistoryInput.RootTable.Columns["Id"].Visible = false;
			grdHistoryInput.RootTable.Columns["NetWeight"].Visible = false;
			grdHistoryInput.RootTable.Columns["RateUom"].Visible = false;
			grdHistoryInput.RootTable.Columns["MinQty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryInput.RootTable.Columns["MinQty"].TextAlignment = (TextAlignment)3;
			grdHistoryInput.RootTable.Columns["MinQty"].FormatString = "##,#.####";
			grdHistoryInput.RootTable.Columns["MinQty"].TotalFormatString = "##,#.####";
			grdHistoryInput.RootTable.Columns["MaxQty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryInput.RootTable.Columns["MaxQty"].TextAlignment = (TextAlignment)3;
			grdHistoryInput.RootTable.Columns["MaxQty"].FormatString = "##,#.####";
			grdHistoryInput.RootTable.Columns["MaxQty"].TotalFormatString = "##,#.####";
			grdHistoryInput.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryInput.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdHistoryInput.RootTable.Columns["ItemQty"].FormatString = "##,#.####";
			grdHistoryInput.RootTable.Columns["ItemQty"].TotalFormatString = "##,#.####";
			grdHistoryInput.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)2;
			grdHistoryInput.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdHistoryInput.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryInput.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryInput.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryInput.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryInput.RootTable.Columns["NetWeight"].FormatString = "##,#.####";
			grdHistoryInput.RootTable.Columns["NetWeight"].TotalFormatString = "##,#.####";
			grdHistoryInput.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdHistoryInput.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdHistoryInput.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistoryInput.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdHistoryInput);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryOutputSetting()
	{
		try
		{
			grdHistoryOutput.RootTable.Columns["Id"].Visible = false;
			grdHistoryOutput.RootTable.Columns["NetWeight"].Visible = false;
			grdHistoryOutput.RootTable.Columns["RateUom"].Visible = false;
			grdHistoryOutput.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOutput.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdHistoryOutput.RootTable.Columns["ItemQty"].FormatString = "##,#.####";
			grdHistoryOutput.RootTable.Columns["ItemQty"].TotalFormatString = "##,#.####";
			grdHistoryOutput.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOutput.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdHistoryOutput.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryOutput.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryOutput.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryOutput.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOutput.RootTable.Columns["NetWeight"].FormatString = "##,#.####";
			grdHistoryOutput.RootTable.Columns["NetWeight"].TotalFormatString = "##,#.####";
			grdHistoryOutput.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdHistoryOutput.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOutput.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistoryOutput.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdHistoryOutput);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryOverHeadSetting()
	{
		try
		{
			grdHistoryOverHead.RootTable.Columns["Id"].Visible = false;
			grdHistoryOverHead.RootTable.Columns["OverHeadType"].Visible = false;
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
			CommonServices.GridAutoAdjustmentNew(grdHistoryOverHead);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdWagesHistorySetting()
	{
		try
		{
			grdWagesHistory.RootTable.Columns["Id"].Visible = false;
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
			CommonServices.GridAutoAdjustmentNew(grdWagesHistory);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryOperationSetting()
	{
		try
		{
			grdhistoryoperations.RootTable.Columns["Id"].Visible = false;
			grdhistoryoperations.RootTable.Columns["CapacityQty"].Visible = false;
			grdhistoryoperations.RootTable.Columns["CapacityQty"].AggregateFunction = (AggregateFunction)2;
			grdhistoryoperations.RootTable.Columns["CapacityQty"].TextAlignment = (TextAlignment)3;
			grdhistoryoperations.RootTable.Columns["CapacityQty"].FormatString = "##,#.####";
			grdhistoryoperations.RootTable.Columns["CapacityQty"].TotalFormatString = "##,#.####";
			grdhistoryoperations.RootTable.Columns["HourlyCost"].TextAlignment = (TextAlignment)3;
			grdhistoryoperations.RootTable.Columns["HourlyCost"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdhistoryoperations.RootTable.Columns["HourlyCost"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdhistoryoperations.RootTable.Columns["OperationCost"].TextAlignment = (TextAlignment)3;
			grdhistoryoperations.RootTable.Columns["OperationCost"].AggregateFunction = (AggregateFunction)2;
			grdhistoryoperations.RootTable.Columns["OperationCost"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdhistoryoperations.RootTable.Columns["OperationCost"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdhistoryoperations);
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
			((Control)(object)cmbItemhistory).Text = string.Empty;
			((Control)(object)cmbbomtypeHistory).Text = string.Empty;
			((Control)(object)CmbBomNoHistory).Text = string.Empty;
			grdHistoryOverHead.ClearStructure();
			grdHistoryOutput.ClearStructure();
			grdHistoryInput.ClearStructure();
			grdhistoryoperations.ClearStructure();
			cmbDateTypeHistory.Focus();
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
					if (((Control)(object)grdInputMaterial).Focused)
					{
						((Control)(object)grdOutPutByProduct).Focus();
					}
					else if (((Control)(object)grdOutPutByProduct).Focused)
					{
						((Control)(object)grdOverHead).Focus();
					}
					else if (((Control)(object)grdOverHead).Focused)
					{
						((Control)(object)grdContractorWages).Focus();
					}
					else if (((Control)(object)grdContractorWages).Focused)
					{
						((Control)(object)grdInputMaterial).Focus();
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
					((Control)(object)grdHistoryInput).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grdHistoryInput).Focused)
					{
						((Control)(object)grdHistoryOutput).Focus();
					}
					else if (((Control)(object)grdHistoryOutput).Focused)
					{
						((Control)(object)grdHistoryOverHead).Focus();
					}
					else if (((Control)(object)grdHistoryOverHead).Focused)
					{
						((Control)(object)grdWagesHistory).Focus();
					}
					else if (((Control)(object)grdWagesHistory).Focused)
					{
						((Control)(object)grdHistoryInput).Focus();
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

	private void grdInputMaterial_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Expected O, but got Unknown
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Expected O, but got Unknown
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
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
				if (grdInputMaterial.CurrentColumn.Key == "Add")
				{
					AddRowInInputGrid();
				}
				if (grdInputMaterial.CurrentColumn.Key == "Delete")
				{
					if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
					{
						if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
						{
							return;
						}
						BomInputDetail d2 = new BomInputDetail();
						d2.Id = Conversion.ToInt(r.Cells["Id"].Value);
						d2.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						d2.ItemUomId = Conversion.ToInt(r.Cells["ItemUomId"].Value);
						d2.ItemQty = Conversion.ToDecimal((object)r.Cells["ItemQty"].Text);
						d2.WeightKg = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
						d2.Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
						d2.RateUomId = Conversion.ToInt(r.Cells["RateUomId"].Value);
						d2.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
						d2.ActionTypeId = 3;
						lstRemoveRecordInput.Add(d2);
						dtInput.Rows[r.RowIndex].Delete();
						if (r != null && (int)r.RowType == 0)
						{
							r.Delete();
						}
						grdInputMaterial.UpdateData();
					}
					else if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					if (dtInput.Rows.Count == 0)
					{
						AddRowInInputGrid();
					}
				}
				grdInputMaterial.UpdateData();
				Calculation();
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInInputGrid();
				grdInputMaterial.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					BomInputDetail d1 = new BomInputDetail();
					d1.Id = Conversion.ToInt(r.Cells["Id"].Value);
					d1.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					d1.ItemUomId = Conversion.ToInt(r.Cells["ItemUomId"].Value);
					d1.ItemQty = Conversion.ToDecimal((object)r.Cells["ItemQty"].Text);
					d1.WeightKg = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
					d1.Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
					d1.RateUomId = Conversion.ToInt(r.Cells["RateUomId"].Value);
					d1.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
					d1.ActionTypeId = 3;
					lstRemoveRecordInput.Add(d1);
					dtInput.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					grdInputMaterial.UpdateData();
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				if (dtInput.Rows.Count == 0)
				{
					AddRowInInputGrid();
				}
				grdInputMaterial.UpdateData();
				Calculation();
			}
			if (e.KeyCode != Keys.F1)
			{
				return;
			}
			int Id = 0;
			decimal Name = default(decimal);
			string NameCode = "";
			if (grdInputMaterial.CurrentColumn.Key == "ItemName")
			{
				int PreviousItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				GrdPopUp obj5 = new GrdPopUp(dtitemInput, "Id", "ItemName");
				obj5.StartPosition = FormStartPosition.CenterScreen;
				obj5.ShowDialog();
				obj5.BringToFront();
				Id = Conversion.ToInt((object)obj5.ReturnId);
				NameCode = Conversion.ToString((object)obj5.ReturnName);
				r.Cells["ItemId"].Value = Id;
				r.Cells["ItemName"].Value = NameCode;
				if (Id != PreviousItemId)
				{
					r.Cells["ItemUomId"].Value = 0;
					r.Cells["Uom"].Value = 0;
					r.Cells["UomCode"].Value = "";
					r.Cells["RateUom"].Value = 0;
					r.Cells["RateUomId"].Value = 0;
					r.Cells["RateUomCode"].Value = "";
					r.Cells["NetWeight"].Value = 0;
					r.Cells["Amount"].Value = 0;
				}
			}
			if (grdInputMaterial.CurrentColumn.Key == "MainItemId" || grdInputMaterial.CurrentColumn.Key == "MainItemName")
			{
				GetInputMainItems(r.RowIndex);
				GrdPopUp obj4 = new GrdPopUp(dtInputMainItems, "Id", "ItemName");
				obj4.StartPosition = FormStartPosition.CenterScreen;
				obj4.ShowDialog();
				obj4.BringToFront();
				if (Conversion.ToInt(r.Cells["ItemId"].Value) == Id)
				{
					Id = 0;
					NameCode = "";
					MessageBox.Show("MainItem Can't be Same As Item");
				}
				else
				{
					Id = Conversion.ToInt((object)obj4.ReturnId);
					NameCode = Conversion.ToString((object)obj4.ReturnName);
				}
				r.Cells["MainItemId"].Value = Id;
				r.Cells["MainItemName"].Value = NameCode;
				if (Id > 0)
				{
					GetInputMainItemsUom(Id);
					DataRow foundRow3 = dtUomInputMain.AsEnumerable().FirstOrDefault();
					int UomId = ((foundRow3 != null) ? Conversion.ToInt(foundRow3["Id"]) : 0);
					string UomCode = ((foundRow3 != null) ? Conversion.ToString(foundRow3["ItemUOMCode"]) : "");
					r.Cells["MainItemUomId"].Value = UomId;
					r.Cells["MainUomCode"].Value = UomCode;
				}
			}
			if (grdInputMaterial.CurrentColumn.Key == "MainItemUomId" || grdInputMaterial.CurrentColumn.Key == "MainUomCode")
			{
				int MainItemId = Conversion.ToInt(r.Cells["MainItemId"].Value);
				if (MainItemId == 0)
				{
					throw new Exception("Select Main Item First...");
				}
				GetInputMainItemsUom(MainItemId);
				GrdPopUp obj3 = new GrdPopUp(dtUomInputMain, "Id", "ItemUOMCode");
				obj3.StartPosition = FormStartPosition.CenterScreen;
				obj3.ShowDialog();
				obj3.BringToFront();
				Id = Conversion.ToInt((object)obj3.ReturnId);
				NameCode = Conversion.ToString((object)obj3.ReturnName);
				r.Cells["MainItemUomId"].Value = Id;
				r.Cells["MainUomCode"].Value = NameCode;
			}
			if (grdInputMaterial.CurrentColumn.Key == "UomCode")
			{
				bindUom(Conversion.ToInt(r.Cells["ItemId"].Value));
				GrdPopUp obj2 = new GrdPopUp(dtUom, "Id", "ItemUOMCode");
				obj2.StartPosition = FormStartPosition.CenterScreen;
				obj2.ShowDialog();
				obj2.BringToFront();
				Id = Conversion.ToInt((object)obj2.ReturnId);
				NameCode = Conversion.ToString((object)obj2.ReturnName);
				DataRow foundRow2 = dtUom.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == Id);
				Name = ((foundRow2 != null) ? Conversion.ToDecimal(foundRow2["ItemUOM"]) : 0m);
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
				GrdPopUp obj = new GrdPopUp(dtRateUom, "RateId", "RateUOMCode");
				obj.StartPosition = FormStartPosition.CenterScreen;
				obj.ShowDialog();
				obj.BringToFront();
				Id = Conversion.ToInt((object)obj.ReturnId);
				NameCode = Conversion.ToString((object)obj.ReturnName);
				DataRow foundRow = dtRateUom.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("RateId") == Id);
				Name = ((foundRow != null) ? Conversion.ToDecimal(foundRow["RateUOM"]) : 0m);
				r.Cells["RateUom"].Value = Name;
				r.Cells["RateUomId"].Value = Id;
				r.Cells["RateUomCode"].Value = NameCode;
			}
			decimal Weight = default(decimal);
			decimal Iuom = default(decimal);
			if (r.Cells["ItemQty"].Value.ToString() != string.Empty && r.Cells["ItemUomId"].Value.ToString() != string.Empty)
			{
				decimal num = Conversion.ToDecimal((object)r.Cells["ItemQty"].Value.ToString());
				Iuom = Conversion.ToDecimal((object)r.Cells["Uom"].Value.ToString());
				Weight = num * Iuom;
				r.Cells["NetWeight"].Value = Weight;
			}
			else
			{
				Weight = default(decimal);
				r.Cells["NetWeight"].Value = Weight;
			}
			if (Weight != 0m && Conversion.ToDecimal(r.Cells["Rate"].Value) != 0m && Conversion.ToInt(r.Cells["RateUomId"].Value) != 0)
			{
				decimal num2 = Conversion.ToDecimal((object)r.Cells["ItemQty"].Value.ToString());
				Iuom = Conversion.ToDecimal((object)r.Cells["Uom"].Value.ToString());
				decimal rate = Conversion.ToDecimal((object)r.Cells["Rate"].Value.ToString());
				decimal Ruom = Conversion.ToDecimal((object)r.Cells["RateUom"].Value.ToString());
				decimal Amount = num2 * Iuom / Ruom * rate;
				r.Cells["Amount"].Value = Amount;
			}
			else
			{
				r.Cells["Amount"].Value = 0;
			}
			grdInputMaterial.UpdateData();
			Calculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOutPutByProduct_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Expected O, but got Unknown
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Expected O, but got Unknown
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
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
						BomByProductDetail d2 = new BomByProductDetail();
						d2.Id = Conversion.ToInt(r.Cells["Id"].Value);
						d2.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						d2.ItemUomId = Conversion.ToInt(r.Cells["ItemUomId"].Value);
						d2.ItemQty = Conversion.ToDecimal((object)r.Cells["ItemQty"].Text);
						d2.Weight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
						d2.ItemRate = Conversion.ToDecimal(r.Cells["Rate"].Value);
						d2.RateUomId = Conversion.ToInt(r.Cells["RateUomId"].Value);
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
					BomByProductDetail d1 = new BomByProductDetail();
					d1.Id = Conversion.ToInt(r.Cells["Id"].Value);
					d1.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					d1.ItemUomId = Conversion.ToInt(r.Cells["ItemUomId"].Value);
					d1.ItemQty = Conversion.ToDecimal((object)r.Cells["ItemQty"].Text);
					d1.Weight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
					d1.ItemRate = Conversion.ToDecimal(r.Cells["Rate"].Value);
					d1.RateUomId = Conversion.ToInt(r.Cells["RateUomId"].Value);
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
				int PreviousItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				GrdPopUp obj3 = new GrdPopUp(dtitemHeader, "Id", "ItemName");
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
					r.Cells["Amount"].Value = 0;
				}
			}
			if (grdOutPutByProduct.CurrentColumn.Key == "UomCode")
			{
				bindUom(Conversion.ToInt(r.Cells["ItemId"].Value));
				GrdPopUp obj2 = new GrdPopUp(dtUom, "Id", "ItemUOMCode");
				obj2.StartPosition = FormStartPosition.CenterScreen;
				obj2.ShowDialog();
				obj2.BringToFront();
				Id = Conversion.ToInt((object)obj2.ReturnId);
				NameCode = Conversion.ToString((object)obj2.ReturnName);
				DataRow foundRow2 = dtUom.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == Id);
				Name = ((foundRow2 != null) ? Conversion.ToDecimal(foundRow2["ItemUOM"]) : 0m);
				r.Cells["ItemUomId"].Value = Id;
				r.Cells["Uom"].Value = Name;
				r.Cells["UomCode"].Value = NameCode;
				r.Cells["RateUom"].Value = Name;
				r.Cells["RateUomId"].Value = Id;
				r.Cells["RateUomCode"].Value = NameCode;
			}
			if (grdOutPutByProduct.CurrentColumn.Key == "RateUomCode")
			{
				bindRateUom(Conversion.ToInt(r.Cells["ItemId"].Value));
				GrdPopUp obj = new GrdPopUp(dtRateUom, "RateId", "RateUOMCode");
				obj.StartPosition = FormStartPosition.CenterScreen;
				obj.ShowDialog();
				obj.BringToFront();
				Id = Conversion.ToInt((object)obj.ReturnId);
				NameCode = Conversion.ToString((object)obj.ReturnName);
				DataRow foundRow = dtRateUom.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("RateId") == Id);
				Name = ((foundRow != null) ? Conversion.ToDecimal(foundRow["RateUOM"]) : 0m);
				r.Cells["RateUom"].Value = Name;
				r.Cells["RateUomId"].Value = Id;
				r.Cells["RateUomCode"].Value = NameCode;
			}
			decimal Weight = default(decimal);
			decimal Iuom = default(decimal);
			if (r.Cells["ItemQty"].Value.ToString() != string.Empty && r.Cells["ItemUomId"].Value.ToString() != string.Empty)
			{
				decimal num = Conversion.ToDecimal((object)r.Cells["ItemQty"].Value.ToString());
				Iuom = Conversion.ToDecimal((object)r.Cells["Uom"].Value.ToString());
				Weight = num * Iuom;
				r.Cells["NetWeight"].Value = Weight;
			}
			else
			{
				Weight = default(decimal);
				r.Cells["NetWeight"].Value = Weight;
			}
			if (Weight != 0m && Conversion.ToDecimal(r.Cells["Rate"].Value) != 0m && Conversion.ToInt(r.Cells["RateUomId"].Value) != 0)
			{
				decimal num2 = Conversion.ToDecimal((object)r.Cells["ItemQty"].Value.ToString());
				Iuom = Conversion.ToDecimal((object)r.Cells["Uom"].Value.ToString());
				decimal rate = Conversion.ToDecimal((object)r.Cells["Rate"].Value.ToString());
				decimal Ruom = Conversion.ToDecimal((object)r.Cells["RateUom"].Value.ToString());
				decimal Amount = num2 * Iuom / Ruom * rate;
				r.Cells["Amount"].Value = Amount;
			}
			else
			{
				r.Cells["Amount"].Value = 0;
			}
			grdOutPutByProduct.UpdateData();
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
			if (e.KeyCode == Keys.F1)
			{
				int Id = 0;
				string Name = "";
				if (grdContractorWages.CurrentColumn != null && grdContractorWages.CurrentColumn.Key == "ServiceActivity")
				{
					DataTable dt = ContractorWagesRateSchedule.GetServiceActivityByItemId(1, Conversion.ToInt(cmbItem.Value), Conversion.ToDateTime((object)txtdocdate.Value), 0, 0);
					GrdPopUp obj = new GrdPopUp(dt, "ContractorWagesRateScheduleId", "WagesActivity");
					obj.StartPosition = FormStartPosition.CenterScreen;
					obj.ShowDialog();
					obj.BringToFront();
					Id = Conversion.ToInt((object)obj.ReturnId);
					Name = Conversion.ToString((object)obj.ReturnName);
					r.Cells["ContractorWagesRateScheduleId"].Value = Id;
					r.Cells["ServiceActivity"].Value = Name;
					if (Id > 0)
					{
						DataRow foundRow = dt.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("ContractorWagesRateScheduleId") == Id);
						decimal Qty = ((foundRow != null) ? Conversion.ToDecimal(foundRow["Qty"]) : 0m);
						decimal RatePerQty = ((foundRow != null) ? Conversion.ToDecimal(foundRow["Rate"]) : 0m) / Qty;
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
				grdContractorWages.UpdateData();
				Calculation();
			}
			EnableDisableItemName();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOperatinalDetail_KeyDown(object sender, KeyEventArgs e)
	{
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
						throw new Exception("You don't have update rights...");
					}
					ReadById(Conversion.ToInt((object)Id));
				}
				if (DatagridHistory.CurrentColumn.Key == "Print")
				{
					BOMSlip(Conversion.ToInt(DatagridHistory.CurrentRow.Cells["Id"].Value));
				}
				if (DatagridHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, 1851);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("You don't have update rights...");
				}
				ReadById(Conversion.ToInt((object)Id));
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
			MessageBox.Show(ex.ToString());
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
			MessageBox.Show(ex.ToString());
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
			MessageBox.Show(ex.ToString());
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
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_048c: Expected O, but got Unknown
		//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Expected O, but got Unknown
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Expected O, but got Unknown
		//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0505: Expected O, but got Unknown
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Expected O, but got Unknown
		//IL_053d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0547: Expected O, but got Unknown
		//IL_0548: Unknown result type (might be due to invalid IL or missing references)
		//IL_0552: Expected O, but got Unknown
		//IL_055e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0568: Expected O, but got Unknown
		//IL_0595: Unknown result type (might be due to invalid IL or missing references)
		//IL_059f: Expected O, but got Unknown
		//IL_0721: Unknown result type (might be due to invalid IL or missing references)
		//IL_072b: Expected O, but got Unknown
		//IL_072c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0736: Expected O, but got Unknown
		//IL_0742: Unknown result type (might be due to invalid IL or missing references)
		//IL_074c: Expected O, but got Unknown
		//IL_0758: Unknown result type (might be due to invalid IL or missing references)
		//IL_0762: Expected O, but got Unknown
		//IL_0876: Unknown result type (might be due to invalid IL or missing references)
		//IL_0880: Expected O, but got Unknown
		//IL_0897: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a1: Expected O, but got Unknown
		//IL_08c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cd: Expected O, but got Unknown
		//IL_08e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ee: Expected O, but got Unknown
		//IL_0905: Unknown result type (might be due to invalid IL or missing references)
		//IL_090f: Expected O, but got Unknown
		//IL_0947: Unknown result type (might be due to invalid IL or missing references)
		//IL_0951: Expected O, but got Unknown
		//IL_0968: Unknown result type (might be due to invalid IL or missing references)
		//IL_0972: Expected O, but got Unknown
		//IL_0989: Unknown result type (might be due to invalid IL or missing references)
		//IL_0993: Expected O, but got Unknown
		//IL_0994: Unknown result type (might be due to invalid IL or missing references)
		//IL_099e: Expected O, but got Unknown
		//IL_09b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bf: Expected O, but got Unknown
		//IL_11d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e1: Expected O, but got Unknown
		//IL_11ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_11f8: Expected O, but got Unknown
		//IL_14a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ad: Expected O, but got Unknown
		//IL_14ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_14c4: Expected O, but got Unknown
		//IL_16c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_16cc: Expected O, but got Unknown
		//IL_16d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_16e3: Expected O, but got Unknown
		//IL_1990: Unknown result type (might be due to invalid IL or missing references)
		//IL_199a: Expected O, but got Unknown
		//IL_19a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_19b1: Expected O, but got Unknown
		//IL_496f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4979: Expected O, but got Unknown
		//IL_4986: Unknown result type (might be due to invalid IL or missing references)
		//IL_4990: Expected O, but got Unknown
		//IL_6e7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e86: Expected O, but got Unknown
		//IL_6eaa: Unknown result type (might be due to invalid IL or missing references)
		//IL_6eb4: Expected O, but got Unknown
		GridEXLayout grdContractorWages_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOverHead_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOutPutByProduct_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInputMaterial_DesignTimeLayout = new GridEXLayout();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
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
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
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
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		GridEXLayout grdOperatinalDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		GridEXLayout grdWagesHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryOverHead_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryOutput_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryInput_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DatagridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdhistoryoperations_DesignTimeLayout = new GridEXLayout();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
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
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.pcc.frmBillOfMaterialConcrete));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grdContractorWages = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.labItemCaption = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.grdOverHead = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label22 = new System.Windows.Forms.Label();
		this.grdOutPutByProduct = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdInputMaterial = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.panel17 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.CmbPlantName = new UltraCombo();
		this.cmbBOMType = new UltraCombo();
		this.cmbItem = new UltraCombo();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.cmbUOM = new UltraCombo();
		this.btnDefinePlant = new System.Windows.Forms.Button();
		this.label43 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.CmbProductionType = new UltraCombo();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label6 = new System.Windows.Forms.Label();
		this.txtValuePerQty = new System.Windows.Forms.TextBox();
		this.label31 = new System.Windows.Forms.Label();
		this.txtNetValue = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.panel20 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.txtTotalInputs = new System.Windows.Forms.TextBox();
		this.label39 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.txtWagesAmount = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtOverHead = new System.Windows.Forms.TextBox();
		this.txtLessByProduct = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.txtdocdate = new System.Windows.Forms.DateTimePicker();
		this.label42 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.txtEffectedDate = new System.Windows.Forms.DateTimePicker();
		this.label13 = new System.Windows.Forms.Label();
		this.txtBom = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtRemarksHeader = new System.Windows.Forms.TextBox();
		this.label18 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label27 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.grdOperatinalDetail = new GridEX();
		this.comCompany = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.combranches = new UltraCombo();
		this.label45 = new System.Windows.Forms.Label();
		this.comproject = new UltraCombo();
		this.chkIsDefault = new System.Windows.Forms.CheckBox();
		this.label46 = new System.Windows.Forms.Label();
		this.ChkIsActive = new System.Windows.Forms.CheckBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ChkPreview = new System.Windows.Forms.CheckBox();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.BtnProductionWorkstation = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.label21 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel45 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdWagesHistory = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.label40 = new System.Windows.Forms.Label();
		this.grdHistoryOverHead = new GridEX();
		this.panel42 = new System.Windows.Forms.Panel();
		this.label34 = new System.Windows.Forms.Label();
		this.panel19 = new System.Windows.Forms.Panel();
		this.grdHistoryOutput = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.grdHistoryInput = new GridEX();
		this.panel44 = new System.Windows.Forms.Panel();
		this.label16 = new System.Windows.Forms.Label();
		this.DatagridHistory = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label17 = new System.Windows.Forms.Label();
		this.panel13 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label14 = new System.Windows.Forms.Label();
		this.grdhistoryoperations = new GridEX();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.btnshow = new System.Windows.Forms.Button();
		this.cmbItemhistory = new UltraCombo();
		this.label29 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.cmbbomtypeHistory = new UltraCombo();
		this.CmbBomNoHistory = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
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
		this.btnRegister = new System.Windows.Forms.ToolStripButton();
		this.label32 = new System.Windows.Forms.Label();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label47 = new System.Windows.Forms.Label();
		this.txtTopForce = new System.Windows.Forms.TextBox();
		this.label48 = new System.Windows.Forms.Label();
		this.txtBottomForce = new System.Windows.Forms.TextBox();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdContractorWages).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOverHead).BeginInit();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOutPutByProduct).BeginInit();
		this.panel11.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInputMaterial).BeginInit();
		this.panel7.SuspendLayout();
		this.panel17.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBOMType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionType).BeginInit();
		this.panel4.SuspendLayout();
		this.panel20.SuspendLayout();
		this.panel16.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOperatinalDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comCompany).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel45.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdWagesHistory).BeginInit();
		this.panel18.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryOverHead).BeginInit();
		this.panel42.SuspendLayout();
		this.panel19.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryOutput).BeginInit();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryInput).BeginInit();
		this.panel44.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).BeginInit();
		this.panel15.SuspendLayout();
		this.panel13.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistoryoperations).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemhistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbomtypeHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBomNoHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).BeginInit();
		this.panel38.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1234, 711);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1234, 711);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1226, 682);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Controls.Add(this.panel10);
		this.panel2.Controls.Add(this.panel16);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.grdOperatinalDetail);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.comCompany);
		this.panel2.Controls.Add(this.label4);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.combranches);
		this.panel2.Controls.Add(this.label45);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.comproject);
		this.panel2.Controls.Add(this.chkIsDefault);
		this.panel2.Controls.Add(this.label46);
		this.panel2.Controls.Add(this.ChkIsActive);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 58);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1220, 621);
		this.panel2.TabIndex = 0;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdContractorWages);
		this.panel6.Controls.Add(this.panel5);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdOverHead);
		this.panel6.Controls.Add(this.panel14);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdOutPutByProduct);
		this.panel6.Controls.Add(this.panel11);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(686, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(534, 621);
		this.panel6.TabIndex = 0;
		((ScrollableBase)this.grdContractorWages).BackColor = System.Drawing.Color.White;
		grdContractorWages_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdContractorWages.DesignTimeLayout = grdContractorWages_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdContractorWages.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Location = new System.Drawing.Point(0, 515);
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Name = "grdContractorWages";
		((ScrollableBase)this.grdContractorWages).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Size = new System.Drawing.Size(534, 106);
		((System.Windows.Forms.Control)(object)this.grdContractorWages).TabIndex = 2;
		this.grdContractorWages.TotalRow = (InheritableBoolean)1;
		this.grdContractorWages.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdContractorWages).VisualStyle = (VisualStyle)4;
		this.grdContractorWages.CellUpdated += new ColumnActionEventHandler(grdContractorWages_CellUpdated);
		this.grdContractorWages.ColumnButtonClick += new ColumnActionEventHandler(grdContractorWages_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdContractorWages).KeyDown += new System.Windows.Forms.KeyEventHandler(grdContractorWages_KeyDown);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.labItemCaption);
		this.panel5.Controls.Add(this.label33);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 488);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(534, 27);
		this.panel5.TabIndex = 13;
		this.labItemCaption.AutoSize = true;
		this.labItemCaption.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labItemCaption.ForeColor = System.Drawing.Color.White;
		this.labItemCaption.Location = new System.Drawing.Point(159, 5);
		this.labItemCaption.Name = "labItemCaption";
		this.labItemCaption.Size = new System.Drawing.Size(90, 16);
		this.labItemCaption.TabIndex = 126;
		this.labItemCaption.Text = "Item Caption";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.ForeColor = System.Drawing.Color.White;
		this.label33.Location = new System.Drawing.Point(12, 5);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(148, 16);
		this.label33.TabIndex = 126;
		this.label33.Text = "Contractor Wages Of ";
		((ScrollableBase)this.grdOverHead).BackColor = System.Drawing.Color.White;
		grdOverHead_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOverHead.DesignTimeLayout = grdOverHead_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Dock = System.Windows.Forms.DockStyle.Top;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOverHead.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Location = new System.Drawing.Point(0, 309);
		((System.Windows.Forms.Control)(object)this.grdOverHead).Name = "grdOverHead";
		((ScrollableBase)this.grdOverHead).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Size = new System.Drawing.Size(534, 179);
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
		this.panel14.Location = new System.Drawing.Point(0, 283);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(534, 26);
		this.panel14.TabIndex = 12;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.White;
		this.label22.Location = new System.Drawing.Point(8, 5);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(71, 16);
		this.label22.TabIndex = 126;
		this.label22.Text = "OverHead";
		((ScrollableBase)this.grdOutPutByProduct).BackColor = System.Drawing.Color.White;
		grdOutPutByProduct_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOutPutByProduct.DesignTimeLayout = grdOutPutByProduct_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOutPutByProduct).Dock = System.Windows.Forms.DockStyle.Top;
		((System.Windows.Forms.Control)(object)this.grdOutPutByProduct).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOutPutByProduct.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOutPutByProduct).Location = new System.Drawing.Point(0, 27);
		((System.Windows.Forms.Control)(object)this.grdOutPutByProduct).Name = "grdOutPutByProduct";
		((ScrollableBase)this.grdOutPutByProduct).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOutPutByProduct).Size = new System.Drawing.Size(534, 256);
		((System.Windows.Forms.Control)(object)this.grdOutPutByProduct).TabIndex = 0;
		this.grdOutPutByProduct.TotalRow = (InheritableBoolean)1;
		this.grdOutPutByProduct.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOutPutByProduct).VisualStyle = (VisualStyle)4;
		this.grdOutPutByProduct.CellUpdated += new ColumnActionEventHandler(grdOutPutByProduct_CellUpdated);
		this.grdOutPutByProduct.ColumnButtonClick += new ColumnActionEventHandler(grdOutPutByProduct_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdOutPutByProduct).KeyDown += new System.Windows.Forms.KeyEventHandler(grdOutPutByProduct_KeyDown);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.ctrlGrdBar3);
		this.panel11.Controls.Add(this.label23);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(534, 27);
		this.panel11.TabIndex = 9;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(12, 5);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(172, 16);
		this.label23.TabIndex = 126;
		this.label23.Text = "OutPut by Product/Scrap";
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdInputMaterial);
		this.panel10.Controls.Add(this.panel7);
		this.panel10.Controls.Add(this.panel17);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(686, 621);
		this.panel10.TabIndex = 3;
		((ScrollableBase)this.grdInputMaterial).BackColor = System.Drawing.Color.White;
		grdInputMaterial_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInputMaterial.DesignTimeLayout = grdInputMaterial_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInputMaterial.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).Location = new System.Drawing.Point(0, 236);
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).Name = "grdInputMaterial";
		((ScrollableBase)this.grdInputMaterial).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInputMaterial).Size = new System.Drawing.Size(686, 385);
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
		this.panel7.Location = new System.Drawing.Point(0, 214);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(686, 22);
		this.panel7.TabIndex = 3;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(3, 3);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(99, 16);
		this.label1.TabIndex = 126;
		this.label1.Text = "Input Material";
		this.panel17.Controls.Add(this.groupBox2);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(0, 0);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(686, 214);
		this.panel17.TabIndex = 2;
		this.groupBox2.Controls.Add(this.label48);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPlantName);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBOMType);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.groupBox2.Controls.Add(this.txtBottomForce);
		this.groupBox2.Controls.Add(this.txtTopForce);
		this.groupBox2.Controls.Add(this.txtQty);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbUOM);
		this.groupBox2.Controls.Add(this.btnDefinePlant);
		this.groupBox2.Controls.Add(this.label43);
		this.groupBox2.Controls.Add(this.label37);
		this.groupBox2.Controls.Add(this.label41);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbProductionType);
		this.groupBox2.Controls.Add(this.panel4);
		this.groupBox2.Controls.Add(this.txtdocdate);
		this.groupBox2.Controls.Add(this.label42);
		this.groupBox2.Controls.Add(this.txtdocno);
		this.groupBox2.Controls.Add(this.txtEffectedDate);
		this.groupBox2.Controls.Add(this.label13);
		this.groupBox2.Controls.Add(this.txtBom);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.label47);
		this.groupBox2.Controls.Add(this.txtRemarksHeader);
		this.groupBox2.Controls.Add(this.label18);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add(this.label44);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(5, 3);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(678, 207);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main Information";
		this.CmbPlantName.AllowNull = (DefaultableBoolean)1;
		this.CmbPlantName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPlantName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPlantName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance65;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance71).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPlantName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPlantName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPlantName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPlantName).Location = new System.Drawing.Point(252, 16);
		((System.Windows.Forms.Control)(object)this.CmbPlantName).Name = "CmbPlantName";
		((System.Windows.Forms.Control)(object)this.CmbPlantName).Size = new System.Drawing.Size(171, 26);
		((System.Windows.Forms.Control)(object)this.CmbPlantName).TabIndex = 6;
		this.cmbBOMType.AllowNull = (DefaultableBoolean)1;
		this.cmbBOMType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBOMType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBOMType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBOMType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance62;
		((SpecialBoxBase)((UltraGridBase)this.cmbBOMType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance30).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance96).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBOMType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBOMType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBOMType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbBOMType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBOMType).Location = new System.Drawing.Point(252, 44);
		((System.Windows.Forms.Control)(object)this.cmbBOMType).Name = "cmbBOMType";
		((System.Windows.Forms.Control)(object)this.cmbBOMType).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.cmbBOMType).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.cmbBOMType).Leave += new System.EventHandler(cmbItem_Leave);
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(252, 72);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 9;
		((System.Windows.Forms.Control)(object)this.cmbItem).Leave += new System.EventHandler(cmbItem_Leave);
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQty.Location = new System.Drawing.Point(381, 101);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(70, 23);
		this.txtQty.TabIndex = 11;
		this.txtQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQty_KeyPress);
		this.cmbUOM.AllowNull = (DefaultableBoolean)1;
		this.cmbUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance34;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance81).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance82).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance83).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Location = new System.Drawing.Point(252, 99);
		((System.Windows.Forms.Control)(object)this.cmbUOM).Name = "cmbUOM";
		((System.Windows.Forms.Control)(object)this.cmbUOM).Size = new System.Drawing.Size(80, 26);
		((System.Windows.Forms.Control)(object)this.cmbUOM).TabIndex = 10;
		this.btnDefinePlant.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnDefinePlant.Location = new System.Drawing.Point(423, 16);
		this.btnDefinePlant.Name = "btnDefinePlant";
		this.btnDefinePlant.Size = new System.Drawing.Size(31, 26);
		this.btnDefinePlant.TabIndex = 7;
		this.btnDefinePlant.TabStop = false;
		this.btnDefinePlant.UseVisualStyleBackColor = true;
		this.btnDefinePlant.Click += new System.EventHandler(btnDefinePlant_Click);
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(1, 50);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(57, 15);
		this.label43.TabIndex = 138;
		this.label43.Text = "Doc Date";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(1, 22);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(57, 15);
		this.label37.TabIndex = 138;
		this.label37.Text = "Doc Date";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(216, 22);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(34, 15);
		this.label41.TabIndex = 142;
		this.label41.Text = "Plant";
		this.CmbProductionType.AllowNull = (DefaultableBoolean)1;
		this.CmbProductionType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbProductionType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbProductionType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance68;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance123).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbProductionType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Location = new System.Drawing.Point(62, 126);
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Name = "CmbProductionType";
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Size = new System.Drawing.Size(150, 26);
		((System.Windows.Forms.Control)(object)this.CmbProductionType).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Leave += new System.EventHandler(CmbProductionType_Leave);
		this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.txtValuePerQty);
		this.panel4.Controls.Add(this.label31);
		this.panel4.Controls.Add(this.txtNetValue);
		this.panel4.Controls.Add(this.label10);
		this.panel4.Controls.Add(this.panel20);
		this.panel4.Controls.Add(this.txtTotalInputs);
		this.panel4.Controls.Add(this.label39);
		this.panel4.Controls.Add(this.label9);
		this.panel4.Controls.Add(this.label25);
		this.panel4.Controls.Add(this.label38);
		this.panel4.Controls.Add(this.label24);
		this.panel4.Controls.Add(this.txtWagesAmount);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.txtOverHead);
		this.panel4.Controls.Add(this.txtLessByProduct);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Location = new System.Drawing.Point(457, 10);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(221, 192);
		this.panel4.TabIndex = 14;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(5, 30);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(80, 17);
		this.label6.TabIndex = 91;
		this.label6.Text = "Total Inputs";
		this.txtValuePerQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtValuePerQty.Enabled = false;
		this.txtValuePerQty.Font = new System.Drawing.Font("Verdana", 10.5f, System.Drawing.FontStyle.Bold);
		this.txtValuePerQty.Location = new System.Drawing.Point(87, 163);
		this.txtValuePerQty.Name = "txtValuePerQty";
		this.txtValuePerQty.ReadOnly = true;
		this.txtValuePerQty.Size = new System.Drawing.Size(131, 25);
		this.txtValuePerQty.TabIndex = 6;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(5, 166);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(75, 17);
		this.label31.TabIndex = 97;
		this.label31.Text = "Value / Qty";
		this.txtNetValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetValue.Enabled = false;
		this.txtNetValue.Font = new System.Drawing.Font("Verdana", 10.5f, System.Drawing.FontStyle.Bold);
		this.txtNetValue.Location = new System.Drawing.Point(87, 136);
		this.txtNetValue.Name = "txtNetValue";
		this.txtNetValue.ReadOnly = true;
		this.txtNetValue.Size = new System.Drawing.Size(131, 25);
		this.txtNetValue.TabIndex = 5;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(5, 139);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(66, 17);
		this.label10.TabIndex = 97;
		this.label10.Text = "Net Value";
		this.panel20.BackColor = System.Drawing.Color.Teal;
		this.panel20.Controls.Add(this.label5);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(0, 0);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(219, 23);
		this.panel20.TabIndex = 0;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(5, 4);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(141, 16);
		this.label5.TabIndex = 33;
		this.label5.Text = "Calculation Summery";
		this.txtTotalInputs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalInputs.Enabled = false;
		this.txtTotalInputs.Font = new System.Drawing.Font("Verdana", 10.5f, System.Drawing.FontStyle.Bold);
		this.txtTotalInputs.Location = new System.Drawing.Point(87, 25);
		this.txtTotalInputs.Name = "txtTotalInputs";
		this.txtTotalInputs.ReadOnly = true;
		this.txtTotalInputs.Size = new System.Drawing.Size(131, 25);
		this.txtTotalInputs.TabIndex = 1;
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(5, 83);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(81, 17);
		this.label39.TabIndex = 95;
		this.label39.Text = "Total Wages";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(5, 56);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(63, 17);
		this.label9.TabIndex = 95;
		this.label9.Text = "Total O.H";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(26, 129);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(18, 16);
		this.label25.TabIndex = 93;
		this.label25.Text = "=";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(26, 71);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(18, 16);
		this.label38.TabIndex = 93;
		this.label38.Text = "+";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(29, 98);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(14, 16);
		this.label24.TabIndex = 93;
		this.label24.Text = "-";
		this.txtWagesAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWagesAmount.Enabled = false;
		this.txtWagesAmount.Font = new System.Drawing.Font("Verdana", 10.5f, System.Drawing.FontStyle.Bold);
		this.txtWagesAmount.Location = new System.Drawing.Point(87, 79);
		this.txtWagesAmount.Name = "txtWagesAmount";
		this.txtWagesAmount.ReadOnly = true;
		this.txtWagesAmount.Size = new System.Drawing.Size(131, 25);
		this.txtWagesAmount.TabIndex = 3;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(26, 45);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(18, 16);
		this.label3.TabIndex = 93;
		this.label3.Text = "+";
		this.txtOverHead.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOverHead.Enabled = false;
		this.txtOverHead.Font = new System.Drawing.Font("Verdana", 10.5f, System.Drawing.FontStyle.Bold);
		this.txtOverHead.Location = new System.Drawing.Point(87, 52);
		this.txtOverHead.Name = "txtOverHead";
		this.txtOverHead.ReadOnly = true;
		this.txtOverHead.Size = new System.Drawing.Size(131, 25);
		this.txtOverHead.TabIndex = 2;
		this.txtLessByProduct.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLessByProduct.Enabled = false;
		this.txtLessByProduct.Font = new System.Drawing.Font("Verdana", 10.5f, System.Drawing.FontStyle.Bold);
		this.txtLessByProduct.Location = new System.Drawing.Point(87, 106);
		this.txtLessByProduct.Name = "txtLessByProduct";
		this.txtLessByProduct.ReadOnly = true;
		this.txtLessByProduct.Size = new System.Drawing.Size(131, 25);
		this.txtLessByProduct.TabIndex = 4;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(5, 111);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(75, 17);
		this.label8.TabIndex = 93;
		this.label8.Text = "By Product";
		this.txtdocdate.CustomFormat = "dd-MMM-yy";
		this.txtdocdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdocdate.Location = new System.Drawing.Point(62, 46);
		this.txtdocdate.Name = "txtdocdate";
		this.txtdocdate.Size = new System.Drawing.Size(150, 23);
		this.txtdocdate.TabIndex = 1;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(1, 132);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(60, 15);
		this.label42.TabIndex = 143;
		this.label42.Text = "Bom Type";
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(62, 18);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(150, 23);
		this.txtdocno.TabIndex = 0;
		this.txtEffectedDate.CustomFormat = "dd-MMM-yy";
		this.txtEffectedDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEffectedDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtEffectedDate.Location = new System.Drawing.Point(62, 74);
		this.txtEffectedDate.Name = "txtEffectedDate";
		this.txtEffectedDate.Size = new System.Drawing.Size(150, 23);
		this.txtEffectedDate.TabIndex = 2;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 8f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(1, 69);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(55, 33);
		this.label13.TabIndex = 133;
		this.label13.Text = "Effected Date";
		this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtBom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBom.Location = new System.Drawing.Point(62, 101);
		this.txtBom.Name = "txtBom";
		this.txtBom.Size = new System.Drawing.Size(150, 23);
		this.txtBom.TabIndex = 3;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(1, 105);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(53, 15);
		this.label11.TabIndex = 130;
		this.label11.Text = "BOM No";
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 8f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(214, 43);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(37, 28);
		this.label12.TabIndex = 131;
		this.label12.Text = "Finish Type";
		this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(216, 78);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(32, 15);
		this.label7.TabIndex = 85;
		this.label7.Text = "Item";
		this.txtRemarksHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksHeader.Location = new System.Drawing.Point(62, 156);
		this.txtRemarksHeader.Multiline = true;
		this.txtRemarksHeader.Name = "txtRemarksHeader";
		this.txtRemarksHeader.Size = new System.Drawing.Size(390, 46);
		this.txtRemarksHeader.TabIndex = 5;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(336, 105);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(26, 15);
		this.label18.TabIndex = 89;
		this.label18.Text = "Qty";
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.White;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(1, 160);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(52, 15);
		this.label2.TabIndex = 87;
		this.label2.Text = "Remarks";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(216, 105);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(35, 15);
		this.label44.TabIndex = 87;
		this.label44.Text = "UOM";
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.label27);
		this.panel16.Controls.Add(this.label26);
		this.panel16.Enabled = false;
		this.panel16.Location = new System.Drawing.Point(1407, 18);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(89, 24);
		this.panel16.TabIndex = 5;
		this.panel16.Visible = false;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.ForeColor = System.Drawing.Color.White;
		this.label27.Location = new System.Drawing.Point(6, 5);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(119, 16);
		this.label27.TabIndex = 127;
		this.label27.Text = "Operations Detail";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ForeColor = System.Drawing.Color.White;
		this.label26.Location = new System.Drawing.Point(7, 2);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(0, 19);
		this.label26.TabIndex = 126;
		((ScrollableBase)this.grdOperatinalDetail).BackColor = System.Drawing.Color.White;
		grdOperatinalDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOperatinalDetail.DesignTimeLayout = grdOperatinalDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOperatinalDetail).Enabled = false;
		((System.Windows.Forms.Control)(object)this.grdOperatinalDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOperatinalDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOperatinalDetail).Location = new System.Drawing.Point(1407, 48);
		((System.Windows.Forms.Control)(object)this.grdOperatinalDetail).Name = "grdOperatinalDetail";
		((ScrollableBase)this.grdOperatinalDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOperatinalDetail).Size = new System.Drawing.Size(89, 58);
		((System.Windows.Forms.Control)(object)this.grdOperatinalDetail).TabIndex = 1;
		this.grdOperatinalDetail.TotalRow = (InheritableBoolean)1;
		this.grdOperatinalDetail.TotalRowPosition = (TotalRowPosition)1;
		((System.Windows.Forms.Control)(object)this.grdOperatinalDetail).Visible = false;
		((ScrollableBase)this.grdOperatinalDetail).VisualStyle = (VisualStyle)4;
		this.grdOperatinalDetail.CellUpdated += new ColumnActionEventHandler(grdProcess_CellUpdated);
		this.grdOperatinalDetail.ColumnButtonClick += new ColumnActionEventHandler(grdProcess_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdOperatinalDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdOperatinalDetail_KeyDown);
		this.comCompany.AllowNull = (DefaultableBoolean)1;
		this.comCompany.AutoCompleteMode = (AutoCompleteMode)4;
		this.comCompany.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comCompany.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comCompany).DisplayLayout.Appearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.comCompany).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comCompany).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comCompany).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comCompany).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance118;
		((SpecialBoxBase)((UltraGridBase)this.comCompany).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comCompany).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.comCompany).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comCompany).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance124).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance125).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).TextHAlignAsString = "Left";
		((UltraGridBase)this.comCompany).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance128;
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
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.combranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance103;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.combranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance55).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance56).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
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
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.comproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance111;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance139).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance139).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.comproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance46).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).TextHAlignAsString = "Left";
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance59;
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
		this.chkIsDefault.AutoSize = true;
		this.chkIsDefault.Checked = true;
		this.chkIsDefault.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkIsDefault.Enabled = false;
		this.chkIsDefault.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkIsDefault.Location = new System.Drawing.Point(1436, 112);
		this.chkIsDefault.Name = "chkIsDefault";
		this.chkIsDefault.Size = new System.Drawing.Size(74, 19);
		this.chkIsDefault.TabIndex = 9;
		this.chkIsDefault.Text = "IsDefault";
		this.chkIsDefault.UseVisualStyleBackColor = true;
		this.chkIsDefault.Visible = false;
		this.label46.AutoSize = true;
		this.label46.Enabled = false;
		this.label46.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(1289, 84);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(48, 17);
		this.label46.TabIndex = 128;
		this.label46.Text = "Project";
		this.label46.Visible = false;
		this.ChkIsActive.AutoSize = true;
		this.ChkIsActive.Checked = true;
		this.ChkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkIsActive.Enabled = false;
		this.ChkIsActive.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkIsActive.Location = new System.Drawing.Point(1346, 112);
		this.ChkIsActive.Name = "ChkIsActive";
		this.ChkIsActive.Size = new System.Drawing.Size(68, 19);
		this.ChkIsActive.TabIndex = 8;
		this.ChkIsActive.Text = "IsActive";
		this.ChkIsActive.UseVisualStyleBackColor = true;
		this.ChkIsActive.Visible = false;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ChkPreview);
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Controls.Add(this.label21);
		this.panel3.Controls.Add(this.label36);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1220, 55);
		this.panel3.TabIndex = 0;
		this.ChkPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPreview.AutoSize = true;
		this.ChkPreview.Checked = true;
		this.ChkPreview.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkPreview.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPreview.ForeColor = System.Drawing.Color.White;
		this.ChkPreview.Location = new System.Drawing.Point(1127, 31);
		this.ChkPreview.Name = "ChkPreview";
		this.ChkPreview.Size = new System.Drawing.Size(84, 20);
		this.ChkPreview.TabIndex = 13;
		this.ChkPreview.Text = "Preview";
		this.ChkPreview.UseVisualStyleBackColor = true;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[14]
		{
			this.btnnew, this.btnRefresh, this.toolStripSeparator3, this.btnsave, this.btnUpdate, this.btnDelete, this.toolStripSeparator, this.btnattachment, this.BtnProductionWorkstation, this.toolStripSeparator4,
			this.btnPrint, this.toolStripSeparator5, this.btnshortcutkeys, this.toolStripSeparator2
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1220, 27);
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
		this.btnattachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.BtnProductionWorkstation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.BtnProductionWorkstation.Enabled = false;
		this.BtnProductionWorkstation.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnProductionWorkstation.Image = (System.Drawing.Image)resources.GetObject("BtnProductionWorkstation.Image");
		this.BtnProductionWorkstation.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnProductionWorkstation.Name = "BtnProductionWorkstation";
		this.BtnProductionWorkstation.Size = new System.Drawing.Size(149, 24);
		this.BtnProductionWorkstation.Text = "Define Work Stations";
		this.BtnProductionWorkstation.Visible = false;
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(100, 24);
		this.btnPrint.Text = "1851-&Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.White;
		this.label21.Location = new System.Drawing.Point(7, 32);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(298, 19);
		this.label21.TabIndex = 126;
		this.label21.Text = "Bill Of Material Manufacturing         ";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.ForeColor = System.Drawing.Color.White;
		this.label36.Location = new System.Drawing.Point(681, 40);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(2332, 15);
		this.label36.TabIndex = 131;
		this.label36.Text = resources.GetString("label36.Text");
		this.tabPage3.Controls.Add(this.panel45);
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.DatagridHistory);
		this.tabPage3.Controls.Add(this.panel15);
		this.tabPage3.Controls.Add(this.panel13);
		this.tabPage3.Controls.Add(this.panel38);
		this.tabPage3.Location = new System.Drawing.Point(4, 4);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(1226, 682);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "History";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel45.Controls.Add(this.panel8);
		this.panel45.Controls.Add(this.panel19);
		this.panel45.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel45.Location = new System.Drawing.Point(0, 400);
		this.panel45.Name = "panel45";
		this.panel45.Size = new System.Drawing.Size(1226, 282);
		this.panel45.TabIndex = 55;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdWagesHistory);
		this.panel8.Controls.Add(this.panel18);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryOverHead);
		this.panel8.Controls.Add(this.panel42);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(645, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(581, 282);
		this.panel8.TabIndex = 50;
		this.grdWagesHistory.AllowEdit = (InheritableBoolean)2;
		grdWagesHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdWagesHistory.DesignTimeLayout = grdWagesHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdWagesHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdWagesHistory.DynamicFiltering = true;
		this.grdWagesHistory.FilterMode = (FilterMode)2;
		this.grdWagesHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdWagesHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdWagesHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdWagesHistory).Location = new System.Drawing.Point(0, 187);
		((System.Windows.Forms.Control)(object)this.grdWagesHistory).Name = "grdWagesHistory";
		((ScrollableBase)this.grdWagesHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdWagesHistory).Size = new System.Drawing.Size(581, 95);
		((System.Windows.Forms.Control)(object)this.grdWagesHistory).TabIndex = 53;
		this.grdWagesHistory.TotalRow = (InheritableBoolean)1;
		this.grdWagesHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdWagesHistory).VisualStyle = (VisualStyle)4;
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.label40);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(0, 168);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(581, 19);
		this.panel18.TabIndex = 54;
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.ForeColor = System.Drawing.Color.White;
		this.label40.Location = new System.Drawing.Point(3, 0);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(126, 16);
		this.label40.TabIndex = 127;
		this.label40.Text = "Contractor Wages";
		this.grdHistoryOverHead.AllowEdit = (InheritableBoolean)2;
		grdHistoryOverHead_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryOverHead.DesignTimeLayout = grdHistoryOverHead_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryOverHead).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdHistoryOverHead.DynamicFiltering = true;
		this.grdHistoryOverHead.FilterMode = (FilterMode)2;
		this.grdHistoryOverHead.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryOverHead).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryOverHead.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryOverHead).Location = new System.Drawing.Point(0, 19);
		((System.Windows.Forms.Control)(object)this.grdHistoryOverHead).Name = "grdHistoryOverHead";
		((ScrollableBase)this.grdHistoryOverHead).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryOverHead).Size = new System.Drawing.Size(581, 149);
		((System.Windows.Forms.Control)(object)this.grdHistoryOverHead).TabIndex = 0;
		this.grdHistoryOverHead.TotalRow = (InheritableBoolean)1;
		this.grdHistoryOverHead.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryOverHead).VisualStyle = (VisualStyle)4;
		this.panel42.BackColor = System.Drawing.Color.Teal;
		this.panel42.Controls.Add(this.label34);
		this.panel42.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel42.Location = new System.Drawing.Point(0, 0);
		this.panel42.Name = "panel42";
		this.panel42.Size = new System.Drawing.Size(581, 19);
		this.panel42.TabIndex = 49;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.ForeColor = System.Drawing.Color.White;
		this.label34.Location = new System.Drawing.Point(3, 0);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(78, 16);
		this.label34.TabIndex = 127;
		this.label34.Text = "OverHeads";
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryOutput);
		this.panel19.Controls.Add(this.panel12);
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryInput);
		this.panel19.Controls.Add(this.panel44);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel19.Location = new System.Drawing.Point(0, 0);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(645, 282);
		this.panel19.TabIndex = 4;
		this.grdHistoryOutput.AllowEdit = (InheritableBoolean)2;
		grdHistoryOutput_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryOutput.DesignTimeLayout = grdHistoryOutput_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryOutput.DynamicFiltering = true;
		this.grdHistoryOutput.FilterMode = (FilterMode)2;
		this.grdHistoryOutput.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryOutput.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Location = new System.Drawing.Point(0, 187);
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Name = "grdHistoryOutput";
		((ScrollableBase)this.grdHistoryOutput).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Size = new System.Drawing.Size(645, 95);
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).TabIndex = 52;
		this.grdHistoryOutput.TotalRow = (InheritableBoolean)1;
		this.grdHistoryOutput.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryOutput).VisualStyle = (VisualStyle)4;
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label15);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 168);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(645, 19);
		this.panel12.TabIndex = 52;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(3, 0);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(172, 16);
		this.label15.TabIndex = 127;
		this.label15.Text = "OutPut by Product/Scrap";
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
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).Size = new System.Drawing.Size(645, 149);
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).TabIndex = 0;
		this.grdHistoryInput.TotalRow = (InheritableBoolean)1;
		this.grdHistoryInput.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryInput).VisualStyle = (VisualStyle)4;
		this.panel44.BackColor = System.Drawing.Color.Teal;
		this.panel44.Controls.Add(this.label16);
		this.panel44.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel44.Location = new System.Drawing.Point(0, 0);
		this.panel44.Name = "panel44";
		this.panel44.Size = new System.Drawing.Size(645, 19);
		this.panel44.TabIndex = 49;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.White;
		this.label16.Location = new System.Drawing.Point(-1, 0);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(99, 16);
		this.label16.TabIndex = 127;
		this.label16.Text = "Input Material";
		this.DatagridHistory.AllowEdit = (InheritableBoolean)2;
		DatagridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DatagridHistory.DesignTimeLayout = DatagridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Dock = System.Windows.Forms.DockStyle.Top;
		this.DatagridHistory.DynamicFiltering = true;
		this.DatagridHistory.FilterMode = (FilterMode)2;
		this.DatagridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DatagridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Location = new System.Drawing.Point(0, 169);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Name = "DatagridHistory";
		((ScrollableBase)this.DatagridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Size = new System.Drawing.Size(1226, 231);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).TabIndex = 1;
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
		this.panel15.Location = new System.Drawing.Point(0, 146);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1226, 23);
		this.panel15.TabIndex = 53;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(8, 3);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(54, 16);
		this.label17.TabIndex = 127;
		this.label17.Text = "History";
		this.panel13.BackColor = System.Drawing.Color.White;
		this.panel13.Controls.Add(this.panel9);
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistoryoperations);
		this.panel13.Controls.Add(this.groupBox1);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 61);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1226, 85);
		this.panel13.TabIndex = 0;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label14);
		this.panel9.Enabled = false;
		this.panel9.Location = new System.Drawing.Point(1039, 6);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(132, 19);
		this.panel9.TabIndex = 51;
		this.panel9.Visible = false;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(3, 0);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(119, 16);
		this.label14.TabIndex = 127;
		this.label14.Text = "Operations Detail";
		this.grdhistoryoperations.AllowEdit = (InheritableBoolean)2;
		grdhistoryoperations_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistoryoperations.DesignTimeLayout = grdhistoryoperations_DesignTimeLayout;
		this.grdhistoryoperations.DynamicFiltering = true;
		((System.Windows.Forms.Control)(object)this.grdhistoryoperations).Enabled = false;
		this.grdhistoryoperations.FilterMode = (FilterMode)2;
		this.grdhistoryoperations.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistoryoperations).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistoryoperations.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdhistoryoperations).Location = new System.Drawing.Point(1044, 31);
		((System.Windows.Forms.Control)(object)this.grdhistoryoperations).Name = "grdhistoryoperations";
		((ScrollableBase)this.grdhistoryoperations).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistoryoperations).Size = new System.Drawing.Size(122, 45);
		((System.Windows.Forms.Control)(object)this.grdhistoryoperations).TabIndex = 1;
		this.grdhistoryoperations.TotalRow = (InheritableBoolean)1;
		this.grdhistoryoperations.TotalRowPosition = (TotalRowPosition)1;
		((System.Windows.Forms.Control)(object)this.grdhistoryoperations).Visible = false;
		((ScrollableBase)this.grdhistoryoperations).VisualStyle = (VisualStyle)4;
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemhistory);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbbomtypeHistory);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBomNoHistory);
		this.groupBox1.Controls.Add(this.label20);
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
		this.groupBox1.Size = new System.Drawing.Size(927, 76);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(857, 46);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(62, 25);
		this.btnshow.TabIndex = 8;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.cmbItemhistory.AllowNull = (DefaultableBoolean)1;
		this.cmbItemhistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemhistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemhistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemhistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance115;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemhistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance133).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance135).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance135).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance135).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance136).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemhistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemhistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemhistory).Location = new System.Drawing.Point(651, 45);
		((System.Windows.Forms.Control)(object)this.cmbItemhistory).Name = "cmbItemhistory";
		((System.Windows.Forms.Control)(object)this.cmbItemhistory).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemhistory).TabIndex = 7;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(612, 51);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(32, 15);
		this.label29.TabIndex = 180;
		this.label29.Text = "Item";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(612, 22);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(32, 15);
		this.label30.TabIndex = 178;
		this.label30.Text = "Type";
		this.cmbbomtypeHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbbomtypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbbomtypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbbomtypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance88;
		((SpecialBoxBase)((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance94).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance95).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance99).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbbomtypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbbomtypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbbomtypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbbomtypeHistory).Location = new System.Drawing.Point(651, 16);
		((System.Windows.Forms.Control)(object)this.cmbbomtypeHistory).Name = "cmbbomtypeHistory";
		((System.Windows.Forms.Control)(object)this.cmbbomtypeHistory).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.cmbbomtypeHistory).TabIndex = 6;
		this.CmbBomNoHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbBomNoHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBomNoHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBomNoHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBomNoHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBomNoHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBomNoHistory).Location = new System.Drawing.Point(411, 45);
		((System.Windows.Forms.Control)(object)this.CmbBomNoHistory).Name = "CmbBomNoHistory";
		((System.Windows.Forms.Control)(object)this.CmbBomNoHistory).Size = new System.Drawing.Size(195, 26);
		((System.Windows.Forms.Control)(object)this.CmbBomNoHistory).TabIndex = 5;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(322, 51);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(56, 15);
		this.label20.TabIndex = 176;
		this.label20.Text = "BOM No.";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(5, 22);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(60, 15);
		this.label19.TabIndex = 174;
		this.label19.Text = "Date Type";
		this.cmbDateTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbDateTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance144;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Location = new System.Drawing.Point(97, 16);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Name = "cmbDateTypeHistory";
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Size = new System.Drawing.Size(224, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).TabIndex = 0;
		this.cmbDateTypeHistory.ValueChanged += new System.EventHandler(cmbDateTypeHistory_ValueChanged);
		this.txtToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.CustomFormat = "dd-MMM-yy";
		this.txtToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateHistory.Location = new System.Drawing.Point(210, 47);
		this.txtToDateHistory.Name = "txtToDateHistory";
		this.txtToDateHistory.Size = new System.Drawing.Size(111, 23);
		this.txtToDateHistory.TabIndex = 2;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(5, 51);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(91, 15);
		this.label28.TabIndex = 171;
		this.label28.Text = "Date From && To";
		this.txtToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(509, 18);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.Size = new System.Drawing.Size(97, 23);
		this.txtToDocNoHistory.TabIndex = 4;
		this.txtFromdateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.CustomFormat = "dd-MMM-yy";
		this.txtFromdateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromdateHistory.Location = new System.Drawing.Point(97, 47);
		this.txtFromdateHistory.Name = "txtFromdateHistory";
		this.txtFromdateHistory.Size = new System.Drawing.Size(111, 23);
		this.txtFromdateHistory.TabIndex = 1;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(322, 22);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(88, 15);
		this.label35.TabIndex = 173;
		this.label35.Text = "Doc From && To";
		this.txtFromNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromNoHistory.Location = new System.Drawing.Point(411, 18);
		this.txtFromNoHistory.Name = "txtFromNoHistory";
		this.txtFromNoHistory.Size = new System.Drawing.Size(97, 23);
		this.txtFromNoHistory.TabIndex = 3;
		this.panel38.BackColor = System.Drawing.Color.Teal;
		this.panel38.Controls.Add(this.toolStrip2);
		this.panel38.Controls.Add(this.label32);
		this.panel38.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel38.Location = new System.Drawing.Point(0, 0);
		this.panel38.Name = "panel38";
		this.panel38.Size = new System.Drawing.Size(1226, 61);
		this.panel38.TabIndex = 48;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.toolStripSeparator1, this.btnRegister });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1226, 27);
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
		this.btnRegister.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRegister.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRegister.Name = "btnRegister";
		this.btnRegister.Size = new System.Drawing.Size(61, 24);
		this.btnRegister.Text = "&Print";
		this.btnRegister.Visible = false;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.White;
		this.label32.Location = new System.Drawing.Point(5, 31);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(326, 19);
		this.label32.TabIndex = 127;
		this.label32.Text = "Bill Of Material Manufacturing History  ";
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(477, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar3.MyGrid = this.grdOutPutByProduct;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(57, 25);
		this.ctrlGrdBar3.TabIndex = 127;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(637, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdInputMaterial;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(46, 21);
		this.ctrlGrdBar2.TabIndex = 127;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = null;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1179, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(9, 6, 9, 6);
		this.ctrlGrdBar1.MyGrid = null;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(42, 22);
		this.ctrlGrdBar1.TabIndex = 128;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(336, 124);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(47, 30);
		this.label47.TabIndex = 89;
		this.label47.Text = "Bottom\r\nForce";
		this.txtTopForce.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTopForce.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTopForce.Location = new System.Drawing.Point(252, 128);
		this.txtTopForce.Name = "txtTopForce";
		this.txtTopForce.Size = new System.Drawing.Size(80, 23);
		this.txtTopForce.TabIndex = 12;
		this.txtTopForce.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQty_KeyPress);
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(216, 124);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(36, 30);
		this.label48.TabIndex = 144;
		this.label48.Text = "Top\r\nForce";
		this.txtBottomForce.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBottomForce.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBottomForce.Location = new System.Drawing.Point(381, 128);
		this.txtBottomForce.Name = "txtBottomForce";
		this.txtBottomForce.Size = new System.Drawing.Size(70, 23);
		this.txtBottomForce.TabIndex = 13;
		this.txtBottomForce.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQty_KeyPress);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1234, 711);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmBillOfMaterialConcrete";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmBillOfMaterialConcrete_FormClosed);
		base.Load += new System.EventHandler(frmBillOfMaterial_Manufacturing_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmBillOfMaterial_Manufacturing_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdContractorWages).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOverHead).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOutPutByProduct).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInputMaterial).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel17.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBOMType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionType).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel20.ResumeLayout(false);
		this.panel20.PerformLayout();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOperatinalDetail).EndInit();
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
		((System.ComponentModel.ISupportInitialize)this.grdWagesHistory).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryOverHead).EndInit();
		this.panel42.ResumeLayout(false);
		this.panel42.PerformLayout();
		this.panel19.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryOutput).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryInput).EndInit();
		this.panel44.ResumeLayout(false);
		this.panel44.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel13.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistoryoperations).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemhistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbomtypeHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBomNoHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).EndInit();
		this.panel38.ResumeLayout(false);
		this.panel38.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
