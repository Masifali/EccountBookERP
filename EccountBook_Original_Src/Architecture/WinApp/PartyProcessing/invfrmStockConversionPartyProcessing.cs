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
using Architecture.BLL.Inventory;
using Architecture.BLL.PartyProcessing;
using Architecture.BLL.Production;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.PartyProcessing;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.PartyProcessing;

public class invfrmStockConversionPartyProcessing : Form
{
	private UserAccount UserAccount = new UserAccount();

	private Rightsobjects formright = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	private int RecId;

	private int updateDetailIndex = -1;

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool UpdateMode;

	private bool WagesStatus;

	private bool WagesActiveOrInActive;

	private DataTable dtInputDetail = new DataTable();

	private DataTable dtOuputDetail = new DataTable();

	private DataTable dtPmDetail = new DataTable();

	private DataTable dtitem = new DataTable();

	private DataTable dtwh = new DataTable();

	private DataTable dtUom = new DataTable();

	private DataTable dtExpense = new DataTable();

	private DataTable dtChargeTo = new DataTable();

	private DataTable dtMoistureSlab = new DataTable();

	private List<StockConversionPartyProcessingDetail> lstRemoveRecord = new List<StockConversionPartyProcessingDetail>();

	private LoadavailableTransactionsForIssuancePartyProcessing Loader;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private TabPage tabPage2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripButton Print;

	private Panel panel4;

	private Panel panel10;

	private Panel panel11;

	private ToolStrip toolStrip2;

	private ToolStripSeparator toolStripSeparator1;

	internal GridEX GrdHistoryMain;

	internal GridEX grdHistoryDetail;

	internal GridEX grdHistoryPM;

	private ToolStripButton btnRefresh;

	internal UltraCombo cmbEntryType;

	private CheckBox chkPrint;

	private Button btnCancelDetail;

	private Button btnUpdateDetail;

	private TextBox txtdeailRemarks;

	private Label label21;

	internal UltraCombo cmbUOM;

	private Label label32;

	private Label label23;

	private Label label28;

	private TextBox txtStock;

	internal UltraCombo cmbGodown;

	internal UltraCombo cmbLot;

	internal UltraCombo cmbItem;

	internal UltraCombo CmbCropyr;

	private Button Add;

	private DateTimePicker txtDocdate;

	private Label label3;

	private Label label1;

	private Label label6;

	private Label label44;

	private Label label7;

	private TextBox txtUnitWeight;

	private TextBox txtdocnumber;

	private TextBox txtProductionNo;

	private TextBox txtQty;

	private TextBox txtRemarks;

	private Label label18;

	private Label label5;

	private Label label12;

	internal UltraCombo cmbBagType;

	private Label label10;

	private Label label8;

	private GroupBox groupBox1;

	private CtrlGrdBar ctrlGrdBar2;

	private CtrlGrdBar ctrlGrdBar1;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private Panel panel22;

	internal GridEX grdInput;

	private TabPage tabPage4;

	private Panel panel23;

	internal GridEX grdByProduct;

	private TabPage tabPage5;

	private Panel panel6;

	private GroupBox groupBox3;

	private Label label11;

	internal UltraCombo CmbStockPartyHeader;

	private TextBox txtBalanceWeight;

	private Label label82;

	private Label label13;

	internal UltraCombo cmbReferenceParty;

	private Label label14;

	private Panel panel3;

	private Panel panel8;

	private Label label16;

	private GroupBox groupBox2;

	private Panel panel24;

	private TextBox txtFinishGoodsQty;

	private TextBox txtPackingMaterialQty;

	private Panel panel25;

	private Label label34;

	private Label label35;

	private Label label36;

	private Label label39;

	private Label label31;

	private TextBox txtPackingMaterialWeight;

	private TextBox txtFinishGoodsWeight;

	private Panel panel51;

	private TextBox txtInputQuantity;

	private Panel panel52;

	private Label label15;

	private Label label17;

	private Label label19;

	private Label label25;

	private Label label29;

	private TextBox txtOutputByProductQty;

	private TextBox txtInputWeight;

	private TextBox txtOutputByProductWeight;

	private Label label2;

	private Label label4;

	private Label label9;

	private Label label22;

	private Label label20;

	private Panel panel7;

	private Panel panel13;

	private Panel panel14;

	private Panel panel9;

	private Panel panel12;

	private Panel panel5;

	private Label label24;

	private Label label26;

	private Label label30;

	private Label label27;

	private ToolStripButton btnNewHistory;

	private ToolStripButton BtnRefreshHistory;

	private DateTimePicker ToDateHistory;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Label label33;

	private TextBox txtToDocNoHistory;

	private TextBox txtFromDocNoHistory;

	private Label label37;

	private Button btnshowHistory;

	private DateTimePicker FromDateHistory;

	private Label label38;

	private Label label40;

	private ToolStripButton btnDelete;

	private ToolStripButton btnattachment;

	private ToolStripButton btnshortcutkeys;

	internal GridEX grdPM;

	private CtrlGrdBar ctrlGrdBar3;

	private Label label41;

	internal UltraCombo CmbJobOrder;

	private ToolStripButton btnLoadInput;

	public invfrmStockConversionPartyProcessing(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void invfrmStockConversionProduction_Load(object sender, EventArgs e)
	{
		try
		{
			tabPage1.AutoScroll = true;
			tabPage1.AutoScrollMargin = new Size(20, 20);
			tabPage1.AutoScrollMinSize = new Size(1000, 500);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			btnDelete.Enabled = formright.DoHaveCanDelete;
			Print.Enabled = formright.DoHavePrintRights;
			WagesStatus = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("WagesCompulsoryOnStockConversionPartyProcessing"));
			WagesRefDocumentsStatus obj = clsGlobalVariables.WagesRefDocumentsStatusList.Find((WagesRefDocumentsStatus row) => row.RefDocumentTypeId == 176);
			WagesActiveOrInActive = obj != null && obj.IsActive;
			dtViewAttachments.Columns.Add("AttchmentPath");
			dtitem.Columns.Add("Id");
			dtitem.Columns.Add("ItemName");
			dtitem.Columns.Add("ItemCode");
			GenerateDocNumber();
			StockPartyFill();
			CmbEntryTypeFill();
			Warehouse();
			CropYear();
			combojoblotfill();
			bagType();
			ItemFill();
			dtInputDetail.Columns.Add("Id", typeof(int));
			dtInputDetail.Columns.Add("RefDocNoId", typeof(int));
			dtInputDetail.Columns.Add("RefDocSubIdNo", typeof(int));
			dtInputDetail.Columns.Add("RefDocumentTypeId", typeof(int));
			dtInputDetail.Columns.Add("JobOrderId", typeof(int));
			dtInputDetail.Columns.Add("JobOrder", typeof(string));
			dtInputDetail.Columns.Add("ReferencePartyId", typeof(int));
			dtInputDetail.Columns.Add("ReferenceParty");
			dtInputDetail.Columns.Add("EntryType");
			dtInputDetail.Columns.Add("WareHouseId", typeof(int));
			dtInputDetail.Columns.Add("WareHouse");
			dtInputDetail.Columns.Add("ItemId", typeof(int));
			dtInputDetail.Columns.Add("ItemCode");
			dtInputDetail.Columns.Add("Item");
			dtInputDetail.Columns.Add("CropYearId", typeof(int));
			dtInputDetail.Columns.Add("CropYear");
			dtInputDetail.Columns.Add("JobLotId", typeof(int));
			dtInputDetail.Columns.Add("JobLot");
			dtInputDetail.Columns.Add("PackingTypeId", typeof(int));
			dtInputDetail.Columns.Add("PackingType");
			dtInputDetail.Columns.Add("ItemUOMId", typeof(int));
			dtInputDetail.Columns.Add("PackUom");
			dtInputDetail.Columns.Add("PackUomEquivalent", typeof(double));
			dtInputDetail.Columns.Add("Quantity", typeof(double));
			dtInputDetail.Columns.Add("Weight", typeof(double));
			dtInputDetail.Columns.Add("Remarks");
			dtInputDetail.Columns.Add("BalQty", typeof(double));
			dtInputDetail.Columns.Add("BalWeight", typeof(double));
			grdInput.DataSource = dtInputDetail;
			grdInput.RetrieveStructure();
			grdInputettings();
			dtOuputDetail.Columns.Add("Id", typeof(int));
			dtOuputDetail.Columns.Add("JobOrderId", typeof(int));
			dtOuputDetail.Columns.Add("JobOrder", typeof(string));
			dtOuputDetail.Columns.Add("ReferencePartyId", typeof(int));
			dtOuputDetail.Columns.Add("ReferenceParty");
			dtOuputDetail.Columns.Add("EntryType");
			dtOuputDetail.Columns.Add("WareHouseId", typeof(int));
			dtOuputDetail.Columns.Add("WareHouse");
			dtOuputDetail.Columns.Add("ItemId", typeof(int));
			dtOuputDetail.Columns.Add("ItemCode");
			dtOuputDetail.Columns.Add("Item");
			dtOuputDetail.Columns.Add("CropYearId", typeof(int));
			dtOuputDetail.Columns.Add("CropYear");
			dtOuputDetail.Columns.Add("JobLotId", typeof(int));
			dtOuputDetail.Columns.Add("JobLot");
			dtOuputDetail.Columns.Add("PackingTypeId", typeof(int));
			dtOuputDetail.Columns.Add("PackingType");
			dtOuputDetail.Columns.Add("ItemUOMId", typeof(int));
			dtOuputDetail.Columns.Add("PackUom");
			dtOuputDetail.Columns.Add("PackUomEquivalent", typeof(double));
			dtOuputDetail.Columns.Add("Quantity", typeof(double));
			dtOuputDetail.Columns.Add("Weight", typeof(double));
			dtOuputDetail.Columns.Add("Remarks");
			grdByProduct.DataSource = dtOuputDetail;
			grdByProduct.RetrieveStructure();
			gridsettingsByProduct();
			dtPmDetail.Columns.Add("Id", typeof(int));
			dtPmDetail.Columns.Add("ItemId", typeof(int));
			dtPmDetail.Columns.Add("ItemQTY", typeof(double));
			dtPmDetail.Columns.Add("WarehouseId", typeof(int));
			AddRowInPackingMaterialGrid();
			grdPM.RetrieveStructure();
			grdPackingMaterialSettings();
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			txtProductionNo.Focus();
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			ToDateHistory.Value = DateTime.Now;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtdocnumber.Text.Trim() == string.Empty || txtdocnumber.Text == "0" || txtdocnumber.Text == null)
		{
			MessageBox.Show("document Number Field Required");
			txtdocnumber.Focus();
			return false;
		}
		if (txtProductionNo.Text.Trim() == string.Empty || txtProductionNo.Text == "0" || txtProductionNo.Text == null)
		{
			MessageBox.Show("Production NO. Field Required");
			txtProductionNo.Focus();
			return false;
		}
		if (((UltraGridBase)CmbStockPartyHeader).ActiveRow == null || Conversion.ToInt(CmbStockPartyHeader.Value) == 0)
		{
			MessageBox.Show("Stock Party Field Required");
			CmbStockPartyHeader.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationOfDetailPortion()
	{
		if (((UltraGridBase)CmbStockPartyHeader).ActiveRow == null || Conversion.ToInt(CmbStockPartyHeader.Value) == 0)
		{
			MessageBox.Show("Stock Party Field is Required");
			CmbStockPartyHeader.Focus();
			return false;
		}
		if (((UltraGridBase)cmbReferenceParty).ActiveRow == null || Conversion.ToInt(cmbReferenceParty.Value) == 0)
		{
			MessageBox.Show("Reference Party Field is Required");
			cmbReferenceParty.Focus();
			return false;
		}
		if (((UltraGridBase)cmbEntryType).ActiveRow == null || Conversion.ToInt(cmbEntryType.Value) == 0)
		{
			MessageBox.Show("Entry Type Field Required");
			cmbEntryType.Focus();
			return false;
		}
		if (((UltraGridBase)cmbGodown).ActiveRow == null || Conversion.ToInt(cmbGodown.Value) == 0)
		{
			MessageBox.Show("Ware house Field Required");
			cmbGodown.Focus();
			return false;
		}
		if (((UltraGridBase)cmbItem).ActiveRow == null || Conversion.ToInt(cmbItem.Value) == 0)
		{
			MessageBox.Show("Item Field Field Required");
			cmbItem.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCropyr).ActiveRow == null || Conversion.ToInt(CmbCropyr.Value) == 0)
		{
			MessageBox.Show("Crop Year Field Required");
			CmbCropyr.Focus();
			return false;
		}
		if (((UltraGridBase)cmbLot).ActiveRow == null || Conversion.ToInt(cmbLot.Value) == 0)
		{
			MessageBox.Show("JobLot Field Required");
			cmbLot.Focus();
			return false;
		}
		if (((UltraGridBase)cmbBagType).ActiveRow == null || Conversion.ToInt(cmbBagType.Value) == 0)
		{
			MessageBox.Show("Packing Type Field Required");
			cmbBagType.Focus();
			return false;
		}
		if (((UltraGridBase)cmbUOM).ActiveRow == null || Conversion.ToInt(cmbUOM.Value) == 0)
		{
			MessageBox.Show("UOM Field Required");
			cmbUOM.Focus();
			return false;
		}
		if (txtQty.Text.Trim() == string.Empty || txtQty.Text.Trim() == "0")
		{
			MessageBox.Show("Quantity Field Required");
			txtQty.Focus();
			return false;
		}
		if (txtUnitWeight.Text.Trim() == string.Empty || txtUnitWeight.Text.Trim() == "0")
		{
			MessageBox.Show("Weight Field Required");
			txtUnitWeight.Focus();
			return false;
		}
		return true;
	}

	private void GenerateDocNumber()
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
			int code = StockConversionPartyProcessing.GetGenerateCode(new StockConversionPartyProcessing
			{
				CompanyId = UserAccount.CompanyId,
				OrganizationId = UserAccount.OrganizationId,
				BranchedId = UserAccount.BranchesId,
				DocumentTypeId = 176,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (code > 0)
			{
				txtdocnumber.Text = code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StockPartyFill()
	{
		try
		{
			int id = Conversion.ToInt(CmbStockPartyHeader.Value);
			DataTable dt = CommonServices.GetSupplierustomerForPartyProcessing();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbStockPartyHeader, "Id", "CompanyName", "Stock Party", true);
				((UltraGridBase)CmbStockPartyHeader).DisplayLayout.Bands[0].Columns[1].Width = 200;
				if (id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
					{
						CmbStockPartyHeader.Value = id;
					}
					else
					{
						CmbStockPartyHeader.Value = string.Empty;
					}
				}
			}
			else
			{
				CmbStockPartyHeader.Value = string.Empty;
				((UltraGridBase)CmbStockPartyHeader).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BindReferenceParty()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		try
		{
			int id = Conversion.ToInt(cmbReferenceParty.Value);
			DataTable dt = StockConversionPartyProcessing.GellAllReferencePartiesStockPartyWise(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				StockPartyId = Conversion.ToInt(CmbStockPartyHeader.Value)
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbReferenceParty, "SupplierCustomerId", "ReferencePartyName", "Reference Party", true);
				if (id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("SupplierCustomerId") == id))
					{
						cmbReferenceParty.Value = id;
					}
					else
					{
						cmbReferenceParty.Value = string.Empty;
					}
				}
				else
				{
					((UltraGridBase)cmbReferenceParty).Rows[1].Activate();
				}
			}
			else
			{
				((Control)(object)cmbReferenceParty).Text = string.Empty;
				((UltraGridBase)cmbReferenceParty).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbEntryTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("EntryType");
			dt.Rows.Add(2, "Recovery By Product");
			dt.Rows.Add(3, "Recovery Head Rice");
			DDL.BindDDL(dt, cmbEntryType, "Id", "EntryType", "EntryType", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Warehouse()
	{
		try
		{
			int id = Conversion.ToInt(cmbGodown.Value);
			dtwh = CommonServices.WareHouseGetAllService();
			if (dtwh.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtwh, cmbGodown, "Id", "WareHouseName", "WareHouse Name", true);
				if (id > 0)
				{
					if (dtwh.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
					{
						cmbGodown.Value = id;
					}
					else
					{
						cmbGodown.Value = string.Empty;
					}
				}
			}
			else
			{
				cmbGodown.Value = string.Empty;
				((UltraGridBase)cmbGodown).DataSource = null;
			}
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
			int id = Conversion.ToInt(cmbItem.Value);
			DataTable dt = Item.ReadAllForItemsForPartyProcessing(UserAccount.OrganizationId, UserAccount.CompanyId, "");
			if (dt.Rows.Count > 0)
			{
				dtitem.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtitem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ItemName"], dt.Rows[i]["ItemCode"]);
				}
			}
			if (dtitem.Rows.Count > 0)
			{
				DDL.BindDDL(dtitem, cmbItem, "Id", "ItemName", "Item Name", ZeroIndex: true);
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 400;
				if (id > 0)
				{
					if (dtitem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
					{
						cmbItem.Value = id;
					}
					else
					{
						cmbItem.Value = string.Empty;
					}
				}
			}
			else
			{
				cmbItem.Value = string.Empty;
				((UltraGridBase)cmbItem).DataSource = null;
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
			int ID = Conversion.ToInt(CmbCropyr.Value);
			DataTable dt = CommonServices.CropYearGetAllService();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbCropyr, "Id", "CropYear", "Crop Year", true);
				((UltraGridBase)CmbCropyr).Rows[1].Activate();
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbCropyr.Value = ID;
					}
					else
					{
						((Control)(object)CmbCropyr).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbCropyr).Text = string.Empty;
				((UltraGridBase)CmbCropyr).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combojoblotfill()
	{
		try
		{
			int id = Conversion.ToInt(cmbLot.Value);
			DataTable dt = jobLot.AllJobLotForPartyProcessing(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbLot, "Id", "JobLotDescription", "JobLotDescription", true);
				if (id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
					{
						cmbLot.Value = id;
					}
					else
					{
						cmbLot.Value = string.Empty;
					}
				}
			}
			else
			{
				cmbLot.Value = string.Empty;
				((UltraGridBase)cmbLot).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bagType()
	{
		try
		{
			int ID = Conversion.ToInt(cmbBagType.Value);
			DataTable dt = InvPackingType.Getall();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbBagType, "Id", "PackTypeDesc", "PackingType", true);
				((UltraGridBase)cmbBagType).Rows[1].Activate();
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						cmbBagType.Value = ID;
					}
					else
					{
						((Control)(object)cmbBagType).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbBagType).Text = string.Empty;
				((UltraGridBase)cmbBagType).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindUom()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		try
		{
			string PackUom = ((Control)(object)cmbUOM).Text;
			UOMSchedule val = new UOMSchedule
			{
				ItemId = Conversion.ToInt(cmbItem.Value),
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtsearchby = new DataTable();
			dtsearchby = UOMSchedule.SearchByObject(val);
			if (dtsearchby.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("UOMCode", typeof(string));
				dt.Columns.Add("Equivalent", typeof(double));
				for (int i = 0; i < dtsearchby.Rows.Count; i++)
				{
					dt.Rows.Add(dtsearchby.Rows[i]["Id"], dtsearchby.Rows[i]["UOMCode"], dtsearchby.Rows[i]["Equivalent"]);
				}
				DDL.BindDDL(dt, cmbUOM, "Id", "UOMCode", "PackUOM", ZeroIndex: false);
				((UltraGridBase)cmbUOM).DisplayLayout.Bands[0].Columns[1].Width = 75;
				((UltraGridBase)cmbUOM).DisplayLayout.Bands[0].Columns[2].Width = 85;
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == PackUom))
				{
					((Control)(object)cmbUOM).Text = PackUom;
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
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbChargeTo()
	{
		try
		{
			dtChargeTo.Columns.Add("Id");
			dtChargeTo.Columns.Add("ChargeTo");
			dtChargeTo.Rows.Add(1, "Recovery By Product");
			dtChargeTo.Rows.Add(2, "Recovery Head Rice");
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
			bindUom();
			if (((Control)(object)cmbEntryType).Text == "Issue")
			{
				GetBalQtyAndWeightForPartyProcessing();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbEntryType_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)cmbEntryType).Text != string.Empty)
			{
				if (((Control)(object)CmbStockPartyHeader).Text == string.Empty || Conversion.ToInt(CmbStockPartyHeader.Value) == 0)
				{
					MessageBox.Show("Please Select Stock Party First!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					CmbStockPartyHeader.Focus();
				}
				else if (((Control)(object)cmbEntryType).Text == "Issue")
				{
					tabControl2.SelectedIndex = 0;
					cmbGodown.Focus();
				}
				else if (((Control)(object)cmbEntryType).Text != "Issue")
				{
					tabControl2.SelectedIndex = 1;
					txtStock.Text = 0.ToString();
					txtBalanceWeight.Text = 0.ToString();
					cmbGodown.Focus();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddInGrid_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationOfDetailPortion())
			{
				return;
			}
			if (((Control)(object)cmbEntryType).Text == "Issue")
			{
				GridEXRow[] rows = grdInput.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value) > 0 || Conversion.ToInt(r.Cells["RefDocNoId"].Value) > 0 || Conversion.ToInt(r.Cells["RefDocSubIdNo"].Value) > 0)
					{
						throw new Exception("Record cannot add Because Loader Entry Exist!");
					}
				}
				dtInputDetail.Rows.Add(Conversion.ToInt((object)0), 0, 0, 0, Conversion.ToInt(CmbJobOrder.Value), Conversion.ToString((object)((Control)(object)CmbJobOrder).Text.Trim()), Conversion.ToInt(cmbReferenceParty.Value), Conversion.ToString((object)((Control)(object)cmbReferenceParty).Text.Trim()), Conversion.ToString((object)((Control)(object)cmbEntryType).Text), Conversion.ToInt(cmbGodown.Value), Conversion.ToString((object)((Control)(object)cmbGodown).Text), Conversion.ToInt(cmbItem.Value), Conversion.ToString(((UltraDropDownBase)cmbItem).SelectedRow.Cells["ItemCode"].Value), Conversion.ToString(((UltraDropDownBase)cmbItem).SelectedRow.Cells["ItemName"].Value), Conversion.ToInt(CmbCropyr.Value), Conversion.ToString((object)((Control)(object)CmbCropyr).Text), Conversion.ToInt(cmbLot.Value), Conversion.ToString((object)((Control)(object)cmbLot).Text), Conversion.ToInt(cmbBagType.Value), Conversion.ToString((object)((Control)(object)cmbBagType).Text), Conversion.ToInt(cmbUOM.Value), Conversion.ToString(((UltraDropDownBase)cmbUOM).SelectedRow.Cells["UomCode"].Value), Conversion.ToDouble(((UltraDropDownBase)cmbUOM).SelectedRow.Cells["Equivalent"].Value), Conversion.ToDouble((object)txtQty.Text), Conversion.ToDouble((object)txtUnitWeight.Text), Conversion.ToString((object)txtdeailRemarks.Text), 0, 0);
				grdInput.DataSource = dtInputDetail;
				grdInput.RetrieveStructure();
				grdInputettings();
				resetDetail();
			}
			else
			{
				dtOuputDetail.Rows.Add(Conversion.ToInt((object)0), Conversion.ToInt(CmbJobOrder.Value), Conversion.ToString((object)((Control)(object)CmbJobOrder).Text.Trim()), Conversion.ToInt(cmbReferenceParty.Value), Conversion.ToString((object)((Control)(object)cmbReferenceParty).Text.Trim()), Conversion.ToString((object)((Control)(object)cmbEntryType).Text), Conversion.ToInt(cmbGodown.Value), Conversion.ToString((object)((Control)(object)cmbGodown).Text), Conversion.ToInt(cmbItem.Value), Conversion.ToString(((UltraDropDownBase)cmbItem).SelectedRow.Cells["ItemCode"].Value), Conversion.ToString(((UltraDropDownBase)cmbItem).SelectedRow.Cells["ItemName"].Value), Conversion.ToInt(CmbCropyr.Value), Conversion.ToString((object)((Control)(object)CmbCropyr).Text), Conversion.ToInt(cmbLot.Value), Conversion.ToString((object)((Control)(object)cmbLot).Text), Conversion.ToInt(cmbBagType.Value), Conversion.ToString((object)((Control)(object)cmbBagType).Text), Conversion.ToInt(cmbUOM.Value), Conversion.ToString(((UltraDropDownBase)cmbUOM).SelectedRow.Cells["UomCode"].Value), Conversion.ToDouble(((UltraDropDownBase)cmbUOM).SelectedRow.Cells["Equivalent"].Value), Conversion.ToDouble((object)txtQty.Text), Conversion.ToDouble((object)txtUnitWeight.Text), Conversion.ToString((object)txtdeailRemarks.Text));
				grdByProduct.DataSource = dtOuputDetail;
				grdByProduct.RetrieveStructure();
				gridsettingsByProduct();
				resetDetail();
			}
			GenerateSummaryForUser();
			if (grdInput.GetRows().Count() > 0 || grdByProduct.GetRows().Count() > 0)
			{
				((Control)(object)CmbStockPartyHeader).Enabled = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationOfDetailPortion())
			{
				if (((Control)(object)cmbEntryType).Text == "Issue")
				{
					dtInputDetail.Rows[updateDetailIndex]["JobOrderId"] = Conversion.ToInt(CmbJobOrder.Value);
					dtInputDetail.Rows[updateDetailIndex]["JobOrder"] = Conversion.ToString((object)((Control)(object)CmbJobOrder).Text);
					dtInputDetail.Rows[updateDetailIndex]["ReferencePartyId"] = Conversion.ToInt(cmbReferenceParty.Value);
					dtInputDetail.Rows[updateDetailIndex]["ReferenceParty"] = Conversion.ToString((object)((Control)(object)cmbReferenceParty).Text);
					dtInputDetail.Rows[updateDetailIndex]["EntryType"] = Conversion.ToString((object)((Control)(object)cmbEntryType).Text);
					dtInputDetail.Rows[updateDetailIndex]["WareHouseId"] = Conversion.ToInt(cmbGodown.Value);
					dtInputDetail.Rows[updateDetailIndex]["WareHouse"] = Conversion.ToString((object)((Control)(object)cmbGodown).Text);
					dtInputDetail.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(cmbItem.Value);
					dtInputDetail.Rows[updateDetailIndex]["ItemCode"] = Conversion.ToString(((UltraDropDownBase)cmbItem).SelectedRow.Cells["ItemCode"].Value);
					dtInputDetail.Rows[updateDetailIndex]["Item"] = Conversion.ToString(((UltraDropDownBase)cmbItem).SelectedRow.Cells["ItemName"].Value);
					dtInputDetail.Rows[updateDetailIndex]["CropYearId"] = Conversion.ToInt(CmbCropyr.Value);
					dtInputDetail.Rows[updateDetailIndex]["CropYear"] = Conversion.ToString((object)((Control)(object)CmbCropyr).Text);
					dtInputDetail.Rows[updateDetailIndex]["JobLotId"] = Conversion.ToInt(cmbLot.Value);
					dtInputDetail.Rows[updateDetailIndex]["JobLot"] = Conversion.ToString((object)((Control)(object)cmbLot).Text);
					dtInputDetail.Rows[updateDetailIndex]["PackingTypeId"] = Conversion.ToInt(cmbBagType.Value);
					dtInputDetail.Rows[updateDetailIndex]["PackingType"] = Conversion.ToString((object)((Control)(object)cmbBagType).Text);
					dtInputDetail.Rows[updateDetailIndex]["ItemUOMId"] = Conversion.ToInt(cmbUOM.Value);
					dtInputDetail.Rows[updateDetailIndex]["PackUom"] = Conversion.ToString(((UltraDropDownBase)cmbUOM).SelectedRow.Cells["UOMCode"].Value);
					dtInputDetail.Rows[updateDetailIndex]["PackUomEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)cmbUOM).SelectedRow.Cells["Equivalent"].Value);
					dtInputDetail.Rows[updateDetailIndex]["Quantity"] = Conversion.ToDouble((object)txtQty.Text);
					dtInputDetail.Rows[updateDetailIndex]["Weight"] = Conversion.ToDouble((object)txtUnitWeight.Text);
					dtInputDetail.Rows[updateDetailIndex]["Remarks"] = Conversion.ToString((object)txtdeailRemarks.Text);
					btnUpdateDetail.Visible = false;
					btnCancelDetail.Visible = false;
					Add.Visible = true;
				}
				else
				{
					dtOuputDetail.Rows[updateDetailIndex]["JobOrderId"] = Conversion.ToInt(CmbJobOrder.Value);
					dtOuputDetail.Rows[updateDetailIndex]["JobOrder"] = Conversion.ToString((object)((Control)(object)CmbJobOrder).Text);
					dtOuputDetail.Rows[updateDetailIndex]["ReferencePartyId"] = Conversion.ToInt(cmbReferenceParty.Value);
					dtOuputDetail.Rows[updateDetailIndex]["ReferenceParty"] = Conversion.ToString((object)((Control)(object)cmbReferenceParty).Text);
					dtOuputDetail.Rows[updateDetailIndex]["EntryType"] = Conversion.ToString((object)((Control)(object)cmbEntryType).Text);
					dtOuputDetail.Rows[updateDetailIndex]["WareHouseId"] = Conversion.ToInt(cmbGodown.Value);
					dtOuputDetail.Rows[updateDetailIndex]["WareHouse"] = Conversion.ToString((object)((Control)(object)cmbGodown).Text);
					dtOuputDetail.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(cmbItem.Value);
					dtOuputDetail.Rows[updateDetailIndex]["ItemCode"] = Conversion.ToString(((UltraDropDownBase)cmbItem).SelectedRow.Cells["ItemCode"].Value);
					dtOuputDetail.Rows[updateDetailIndex]["Item"] = Conversion.ToString(((UltraDropDownBase)cmbItem).SelectedRow.Cells["ItemName"].Value);
					dtOuputDetail.Rows[updateDetailIndex]["CropYearId"] = Conversion.ToInt(CmbCropyr.Value);
					dtOuputDetail.Rows[updateDetailIndex]["CropYear"] = Conversion.ToString((object)((Control)(object)CmbCropyr).Text);
					dtOuputDetail.Rows[updateDetailIndex]["JobLotId"] = Conversion.ToInt(cmbLot.Value);
					dtOuputDetail.Rows[updateDetailIndex]["JobLot"] = Conversion.ToString((object)((Control)(object)cmbLot).Text);
					dtOuputDetail.Rows[updateDetailIndex]["PackingTypeId"] = Conversion.ToInt(cmbBagType.Value);
					dtOuputDetail.Rows[updateDetailIndex]["PackingType"] = Conversion.ToString((object)((Control)(object)cmbBagType).Text);
					dtOuputDetail.Rows[updateDetailIndex]["ItemUOMId"] = Conversion.ToInt(cmbUOM.Value);
					dtOuputDetail.Rows[updateDetailIndex]["PackUom"] = Conversion.ToString(((UltraDropDownBase)cmbUOM).SelectedRow.Cells["UOMCode"].Value);
					dtOuputDetail.Rows[updateDetailIndex]["PackUomEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)cmbUOM).SelectedRow.Cells["Equivalent"].Value);
					dtOuputDetail.Rows[updateDetailIndex]["Quantity"] = Conversion.ToDouble((object)txtQty.Text);
					dtOuputDetail.Rows[updateDetailIndex]["Weight"] = Conversion.ToDouble((object)txtUnitWeight.Text);
					dtOuputDetail.Rows[updateDetailIndex]["Remarks"] = Conversion.ToString((object)txtdeailRemarks.Text);
					btnUpdateDetail.Visible = false;
					btnCancelDetail.Visible = false;
					Add.Visible = true;
				}
				resetDetail();
				GenerateSummaryForUser();
				CmbJobOrder.Focus();
			}
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
			Add.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			resetDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInputettings()
	{
		try
		{
			grdInput.RootTable.Columns["Id"].Visible = false;
			grdInput.RootTable.Columns["RefDocNoId"].Visible = false;
			grdInput.RootTable.Columns["RefDocSubIdNo"].Visible = false;
			grdInput.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grdInput.RootTable.Columns["JobOrderId"].Visible = false;
			grdInput.RootTable.Columns["ReferencePartyId"].Visible = false;
			grdInput.RootTable.Columns["WareHouseId"].Visible = false;
			grdInput.RootTable.Columns["ItemId"].Visible = false;
			grdInput.RootTable.Columns["ItemUOMId"].Visible = false;
			grdInput.RootTable.Columns["JobLotId"].Visible = false;
			grdInput.RootTable.Columns["PackingTypeId"].Visible = false;
			grdInput.RootTable.Columns["CropYearId"].Visible = false;
			grdInput.RootTable.Columns["PackUomEquivalent"].Visible = false;
			grdInput.RootTable.Columns["BalQty"].Visible = false;
			grdInput.RootTable.Columns["BalWeight"].Visible = false;
			grdInput.RootTable.Columns["Id"].EditType = (EditType)0;
			grdInput.RootTable.Columns["RefDocNoId"].EditType = (EditType)0;
			grdInput.RootTable.Columns["RefDocSubIdNo"].EditType = (EditType)0;
			grdInput.RootTable.Columns["RefDocumentTypeId"].EditType = (EditType)0;
			grdInput.RootTable.Columns["JobOrderId"].EditType = (EditType)0;
			grdInput.RootTable.Columns["JobOrder"].EditType = (EditType)0;
			grdInput.RootTable.Columns["ReferencePartyId"].EditType = (EditType)0;
			grdInput.RootTable.Columns["ReferenceParty"].EditType = (EditType)0;
			grdInput.RootTable.Columns["EntryType"].EditType = (EditType)0;
			grdInput.RootTable.Columns["WareHouseId"].EditType = (EditType)0;
			grdInput.RootTable.Columns["WareHouse"].EditType = (EditType)0;
			grdInput.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdInput.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			grdInput.RootTable.Columns["Item"].EditType = (EditType)0;
			grdInput.RootTable.Columns["ItemUOMId"].EditType = (EditType)0;
			grdInput.RootTable.Columns["PackUom"].EditType = (EditType)0;
			grdInput.RootTable.Columns["PackUomEquivalent"].EditType = (EditType)0;
			grdInput.RootTable.Columns["CropYearId"].EditType = (EditType)0;
			grdInput.RootTable.Columns["CropYear"].EditType = (EditType)0;
			grdInput.RootTable.Columns["JobLotId"].EditType = (EditType)0;
			grdInput.RootTable.Columns["JobLot"].EditType = (EditType)0;
			grdInput.RootTable.Columns["PackingTypeId"].EditType = (EditType)0;
			grdInput.RootTable.Columns["PackingType"].EditType = (EditType)0;
			grdInput.RootTable.Columns["BalQty"].EditType = (EditType)0;
			grdInput.RootTable.Columns["BalWeight"].EditType = (EditType)0;
			grdInput.RootTable.Columns["Quantity"].AggregateFunction = (AggregateFunction)2;
			grdInput.RootTable.Columns["Quantity"].TextAlignment = (TextAlignment)3;
			grdInput.RootTable.Columns["Quantity"].FormatString = "#,##0.##";
			grdInput.RootTable.Columns["Quantity"].TotalFormatString = "#,##0.##";
			grdInput.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdInput.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdInput.RootTable.Columns["Weight"].FormatString = "#,##0.##";
			grdInput.RootTable.Columns["Weight"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustmentNew(grdInput);
			grdInput.RootTable.Columns["Remarks"].Width = 200;
			grdInput.RootTable.Columns.Add("Delete");
			grdInput.RootTable.Columns["Delete"].Caption = "X";
			grdInput.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInput.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdInput.RootTable.Columns["Delete"].ButtonText = "X";
			grdInput.RootTable.Columns["Delete"].Width = 35;
			grdInput.RootTable.Columns["Delete"].Position = 0;
			grdInput.FrozenColumns = 1;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdInput_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdInput.CurrentRow;
			if (item != null && (int)item.RowType == 0 && Conversion.ToInt(item.Cells["RefDocNoId"].Value) == 0 && Conversion.ToInt(item.Cells["RefDocSubIdNo"].Value) == 0 && Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) == 0)
			{
				updateDetailIndex = item.RowIndex;
				if (Conversion.ToInt(item.Cells["JobOrderId"].Value) > 0)
				{
					CmbJobOrder.Value = Conversion.ToInt(item.Cells["JobOrderId"].Value);
				}
				cmbReferenceParty.Value = item.Cells["ReferencePartyId"].Value.ToString();
				((Control)(object)cmbEntryType).Text = item.Cells["EntryType"].Text.ToString();
				cmbGodown.Value = item.Cells["WareHouseId"].Value.ToString();
				cmbItem.Value = item.Cells["ItemId"].Value.ToString();
				bindUom();
				cmbLot.Value = item.Cells["JobLotId"].Value.ToString();
				CmbCropyr.Value = item.Cells["CropYearId"].Value.ToString();
				cmbBagType.Value = item.Cells["PackingTypeId"].Value.ToString();
				cmbUOM.Value = item.Cells["ItemUOMId"].Value.ToString();
				txtQty.Text = item.Cells["Quantity"].Text.ToString();
				txtUnitWeight.Text = item.Cells["Weight"].Text.ToString();
				txtRemarks.Text = item.Cells["Remarks"].Text.ToString();
				Add.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelDetail.Visible = true;
				GetBalQtyAndWeightForPartyProcessing();
				CmbJobOrder.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInput_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdInput.UpdateData();
			GridEXRow r = grdInput.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			int refDocNoId = Conversion.ToInt(r.Cells["RefDocNoId"].Value);
			int refDocSubIdNo = Conversion.ToInt(r.Cells["RefDocSubIdNo"].Value);
			int refDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
			double BalanceQty = Conversion.ToDouble(r.Cells["BalQty"].Value);
			double BalanceWeight = Conversion.ToDouble(r.Cells["BalWeight"].Value);
			if (btnsave.Visible && btnsave.Enabled && (refDocNoId > 0 || refDocSubIdNo > 0 || refDocumentTypeId > 0))
			{
				if (e.Column.Key == "Quantity")
				{
					double Qty = Conversion.ToDouble(r.Cells["Quantity"].Value);
					Conversion.ToDouble(r.Cells["PackUomEquivalent"].Value);
					double Weight2 = BalanceWeight / BalanceQty * Qty;
					r.Cells["Weight"].Value = Weight2;
					if (BalanceQty < Qty)
					{
						r.Cells["Quantity"].Value = 0;
						r.Cells["Weight"].Value = 0;
						MessageBox.Show("Qty cannot greater than BalanceQty " + BalanceQty + " Please check!");
					}
				}
				if (e.Column.Key == "Weight")
				{
					double Weight = Conversion.ToDouble(r.Cells["Weight"].Value);
					if (Conversion.ToDouble(r.Cells["BalWeight"].Value) < Weight)
					{
						r.Cells["Weight"].Value = 0;
						MessageBox.Show("Weight cannot greater than BalanceWeight " + BalanceWeight.ToString("#,##0.###") + " Please check!");
					}
				}
			}
			else if (e.Column.Key == "Quantity")
			{
				double Qty2 = Conversion.ToDouble(r.Cells["Quantity"].Value);
				double Uom = Conversion.ToDouble(r.Cells["PackUomEquivalent"].Value);
				r.Cells["Weight"].Value = Qty2 * Uom;
			}
			grdInput.UpdateData();
			GenerateSummaryForUser();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInput_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		try
		{
			GridEXRow item = grdInput.CurrentRow;
			if (item == null || (int)item.RowType != 0 || !(e.Column.Key == "Delete"))
			{
				return;
			}
			if (updateDetailIndex == -1)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					if (btnsave.Visible && btnsave.Enabled)
					{
						item.Delete();
					}
					else if (Conversion.ToInt(item.Cells["Id"].Value) > 0)
					{
						StockConversionPartyProcessingDetail d = new StockConversionPartyProcessingDetail();
						d.Id = Conversion.ToInt(item.Cells["Id"].Value);
						d.RefDocNoId = Conversion.ToInt(item.Cells["RefDocNoId"].Value);
						d.RefDocSubIdNo = Conversion.ToInt(item.Cells["RefDocSubIdNo"].Value);
						d.RefDocumentTypeId = Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value);
						d.StockPartyId = Conversion.ToInt(CmbStockPartyHeader.Value);
						d.JobOrderId = Conversion.ToInt((object)item.Cells["JobOrderId"].Text);
						d.ReferencePartyId = Conversion.ToInt(item.Cells["ReferencePartyId"].Value);
						d.EntryType = Conversion.ToString((object)item.Cells["EntryType"].Text);
						d.WarehouseId = Conversion.ToInt(item.Cells["WareHouseId"].Value);
						d.ItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
						d.CropYearId = Conversion.ToInt(item.Cells["CropYearId"].Value);
						d.JobLotId = Conversion.ToInt(item.Cells["JobLotId"].Value);
						d.PackingtypeId = Conversion.ToInt(item.Cells["PackingTypeId"].Value);
						d.ItemUomId = Conversion.ToInt(item.Cells["ItemUOMId"].Value);
						d.Qty = Conversion.ToDouble(item.Cells["Quantity"].Value);
						d.Weight = Conversion.ToDouble(item.Cells["Weight"].Value);
						d.ProjectId = 0;
						d.PackUnit = 0;
						d.Remarks = Conversion.ToString((object)item.Cells["Remarks"].Text);
						d.ActionTypeId = 3;
						lstRemoveRecord.Add(d);
						item.Delete();
						grdInput.UpdateData();
					}
					else
					{
						item.Delete();
					}
					if (grdInput.GetRows().Count() == 0 && grdByProduct.GetRows().Count() == 0)
					{
						((Control)(object)CmbStockPartyHeader).Enabled = true;
					}
					GenerateSummaryForUser();
				}
				return;
			}
			throw new Exception("Reset the Detail First...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridsettingsByProduct()
	{
		try
		{
			grdByProduct.RootTable.Columns["Id"].Visible = false;
			grdByProduct.RootTable.Columns["JobOrderId"].Visible = false;
			grdByProduct.RootTable.Columns["ReferencePartyId"].Visible = false;
			grdByProduct.RootTable.Columns["WareHouseId"].Visible = false;
			grdByProduct.RootTable.Columns["ItemId"].Visible = false;
			grdByProduct.RootTable.Columns["CropYearId"].Visible = false;
			grdByProduct.RootTable.Columns["JobLotId"].Visible = false;
			grdByProduct.RootTable.Columns["PackingTypeId"].Visible = false;
			grdByProduct.RootTable.Columns["ItemUOMId"].Visible = false;
			grdByProduct.RootTable.Columns["PackUomEquivalent"].Visible = false;
			grdByProduct.RootTable.Columns["Id"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["JobOrderId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["JobOrder"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["ReferencePartyId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["ReferenceParty"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["EntryType"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["WareHouseId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["WareHouse"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["Item"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["CropYearId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["CropYear"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["JobLotId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["JobLot"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["PackingTypeId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["PackingType"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["ItemUOMId"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["PackUom"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["PackUomEquivalent"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["Quantity"].EditType = (EditType)1;
			grdByProduct.RootTable.Columns["Weight"].EditType = (EditType)1;
			grdByProduct.RootTable.Columns["Remarks"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["Quantity"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["Weight"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["Remarks"].FilterEditType = (FilterEditType)1;
			grdByProduct.RootTable.Columns["Quantity"].AggregateFunction = (AggregateFunction)2;
			grdByProduct.RootTable.Columns["Quantity"].TextAlignment = (TextAlignment)3;
			grdByProduct.RootTable.Columns["Quantity"].FormatString = "#,##0.##";
			grdByProduct.RootTable.Columns["Quantity"].TotalFormatString = "#,##0.##";
			grdByProduct.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdByProduct.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdByProduct.RootTable.Columns["Weight"].FormatString = "#,##0.##";
			grdByProduct.RootTable.Columns["Weight"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustmentNew(grdByProduct);
			grdByProduct.RootTable.Columns["Remarks"].Width = 200;
			grdByProduct.RootTable.Columns.Add("Delete");
			grdByProduct.RootTable.Columns["Delete"].Caption = "X";
			grdByProduct.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdByProduct.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdByProduct.RootTable.Columns["Delete"].ButtonText = "X";
			grdByProduct.RootTable.Columns["Delete"].Width = 35;
			grdByProduct.RootTable.Columns["Delete"].Position = 0;
			grdByProduct.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdByProduct_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdByProduct.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				if (Conversion.ToInt(item.Cells["JobOrderId"].Value) > 0)
				{
					CmbJobOrder.Value = Conversion.ToInt(item.Cells["JobOrderId"].Value);
				}
				cmbReferenceParty.Value = item.Cells["ReferencePartyId"].Value.ToString();
				((Control)(object)cmbEntryType).Text = item.Cells["EntryType"].Text.ToString();
				cmbGodown.Value = item.Cells["WareHouseId"].Value.ToString();
				cmbItem.Value = item.Cells["ItemId"].Value.ToString();
				bindUom();
				CmbCropyr.Value = item.Cells["CropYearId"].Value.ToString();
				cmbLot.Value = item.Cells["JobLotId"].Value.ToString();
				cmbBagType.Value = item.Cells["PackingTypeId"].Value.ToString();
				cmbUOM.Value = item.Cells["ItemUOMId"].Value.ToString();
				txtQty.Text = item.Cells["Quantity"].Text.ToString();
				txtUnitWeight.Text = item.Cells["Weight"].Text.ToString();
				txtRemarks.Text = item.Cells["Remarks"].Text.ToString();
				Add.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelDetail.Visible = true;
				CmbJobOrder.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdByProduct_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		try
		{
			GridEXRow item = grdByProduct.CurrentRow;
			if (item == null || (int)item.RowType != 0 || !(e.Column.Key == "Delete"))
			{
				return;
			}
			if (updateDetailIndex == -1)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					if (btnsave.Visible && btnsave.Enabled)
					{
						item.Delete();
					}
					else if (Conversion.ToInt(item.Cells["Id"].Value) > 0)
					{
						StockConversionPartyProcessingDetail Conversiondetail = new StockConversionPartyProcessingDetail();
						Conversiondetail.Id = Conversion.ToInt(item.Cells["Id"].Value);
						Conversiondetail.StockPartyId = Conversion.ToInt(CmbStockPartyHeader.Value);
						Conversiondetail.JobOrderId = Conversion.ToInt((object)item.Cells["JobOrderId"].Text);
						Conversiondetail.ReferencePartyId = Conversion.ToInt((object)item.Cells["ReferencePartyId"].Text);
						Conversiondetail.EntryType = Conversion.ToString((object)item.Cells["EntryType"].Text);
						Conversiondetail.WarehouseId = Conversion.ToInt(item.Cells["WareHouseId"].Value);
						Conversiondetail.ItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
						Conversiondetail.CropYearId = Conversion.ToInt(item.Cells["CropYearId"].Value);
						Conversiondetail.JobLotId = Conversion.ToInt(item.Cells["JobLotId"].Value);
						Conversiondetail.PackingtypeId = Conversion.ToInt(item.Cells["PackingTypeId"].Value);
						Conversiondetail.ItemUomId = Conversion.ToInt(item.Cells["ItemUOMId"].Value);
						Conversiondetail.Qty = Conversion.ToDouble(item.Cells["Quantity"].Value);
						Conversiondetail.Weight = Conversion.ToDouble(item.Cells["Weight"].Value);
						Conversiondetail.ProjectId = 0;
						Conversiondetail.PackUnit = 0;
						Conversiondetail.Remarks = Conversion.ToString((object)item.Cells["Remarks"].Text);
						Conversiondetail.ActionTypeId = 3;
						lstRemoveRecord.Add(Conversiondetail);
						item.Delete();
						grdByProduct.UpdateData();
					}
					else
					{
						item.Delete();
					}
					if (grdInput.GetRows().Count() == 0 && grdByProduct.GetRows().Count() == 0)
					{
						((Control)(object)CmbStockPartyHeader).Enabled = true;
					}
					GenerateSummaryForUser();
				}
				return;
			}
			throw new Exception("Reset the Detail First...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdByProduct_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grdByProduct.UpdateData();
			GridEXRow item = grdByProduct.CurrentRow;
			if (e.Column.Key == "Quantity")
			{
				double Qty = Conversion.ToDouble((object)item.Cells["Quantity"].Value.ToString());
				double Uom = Conversion.ToDouble((object)item.Cells["PackUomEquivalent"].Value.ToString());
				item.Cells["Weight"].Value = Qty * Uom;
			}
			if (e.Column.Key == "Weight")
			{
				Conversion.ToDouble((object)item.Cells["Weight"].Value.ToString());
			}
			GenerateSummaryForUser();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInPackingMaterialGrid()
	{
		dtPmDetail.Rows.Add(0, 0, 0, 0);
		grdPM.DataSource = dtPmDetail;
	}

	private void grdPackingMaterialSettings()
	{
		try
		{
			grdPM.RootTable.Columns["Id"].Visible = false;
			DataTable dt = CommonServices.GetItemPartyProcessing("7");
			grdPM.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			grdPM.RootTable.Columns["ItemId"].HasValueList = true;
			grdPM.RootTable.Columns["ItemId"].LimitToList = true;
			grdPM.RootTable.Columns["ItemId"].EditType = (EditType)4;
			grdPM.RootTable.Columns["ItemId"].Caption = "Item Name";
			grdPM.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dt.DefaultView, "Id", "ItemName");
			grdPM.RootTable.Columns["ItemId"].Width = 200;
			DataTable dtwarePM = CommonServices.GetActiveWareHouseByWareHouseType(2);
			grdPM.RootTable.Columns["WarehouseId"].ColumnType = (ColumnType)0;
			grdPM.RootTable.Columns["WarehouseId"].HasValueList = true;
			grdPM.RootTable.Columns["WarehouseId"].LimitToList = true;
			grdPM.RootTable.Columns["WarehouseId"].EditType = (EditType)4;
			grdPM.RootTable.Columns["WarehouseId"].Caption = "WareHouse";
			grdPM.RootTable.Columns["WarehouseId"].ValueList.PopulateValueList((IEnumerable)dtwarePM.DefaultView, "Id", "WareHouseName");
			grdPM.RootTable.Columns["WarehouseId"].Width = 100;
			grdPM.RootTable.Columns["ItemQTY"].AggregateFunction = (AggregateFunction)2;
			grdPM.RootTable.Columns["ItemQTY"].TextAlignment = (TextAlignment)3;
			grdPM.RootTable.Columns["ItemQTY"].FormatString = "#,##.##";
			grdPM.RootTable.Columns["ItemQTY"].TotalFormatString = "#,##.##";
			CommonServices.GridAutoAdjustmentNew(grdPM);
			grdPM.RootTable.Columns.Add("Delete");
			grdPM.RootTable.Columns["Delete"].Key = "Delete";
			grdPM.RootTable.Columns["Delete"].Caption = "X";
			grdPM.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPM.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdPM.RootTable.Columns["Delete"].ButtonText = "X";
			grdPM.RootTable.Columns["Delete"].Width = 30;
			grdPM.RootTable.Columns["Delete"].Position = 0;
			grdPM.RootTable.Columns.Add("Add");
			grdPM.RootTable.Columns["Add"].Key = "Add";
			grdPM.RootTable.Columns["Add"].Caption = "+";
			grdPM.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPM.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdPM.RootTable.Columns["Add"].ButtonText = "+";
			grdPM.RootTable.Columns["Add"].Width = 30;
			grdPM.RootTable.Columns["Add"].Position = 1;
			grdPM.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackingMaterial_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPM.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				dtPmDetail.Rows[r.RowIndex].Delete();
				r.Delete();
				if (dtPmDetail.Rows.Count == 0)
				{
					AddRowInPackingMaterialGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInPackingMaterialGrid();
			}
			grdPM.UpdateData();
			GenerateSummaryForUser();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackingMaterial_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPM.CurrentRow;
			if (r != null && (int)r.RowType == 0 && e.Column.Key == "ItemQTY")
			{
				GenerateSummaryForUser();
			}
			grdPM.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Insert()
	{
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Expected O, but got Unknown
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Expected O, but got Unknown
		//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cf: Expected O, but got Unknown
		//IL_0864: Unknown result type (might be due to invalid IL or missing references)
		//IL_086b: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			bool falag = false;
			GridEXRow[] rows = grdInput.GetRows();
			for (int j = 0; j < rows.Length; j++)
			{
				if (rows[j].Cells["EntryType"].Text == "Issue")
				{
					falag = true;
				}
			}
			if (!falag)
			{
				MessageBox.Show("Input is Required in Detail Grid");
				return;
			}
			if (grdInput.GetRows().Count() == 0)
			{
				throw new Exception("InPut Grid Not Found");
			}
			if (grdByProduct.GetRows().Count() == 0)
			{
				throw new Exception("OutPut Grid Not Found");
			}
			bool flagForHeadRiceEntry = false;
			rows = grdByProduct.GetRows();
			for (int j = 0; j < rows.Length; j++)
			{
				if (rows[j].Cells["EntryType"].Text == "Recovery Head Rice")
				{
					flagForHeadRiceEntry = true;
				}
			}
			if (!flagForHeadRiceEntry)
			{
				MessageBox.Show("Recovery Head Rice is Required in Detail Grid");
				return;
			}
			StockConversionPartyProcessing h = new StockConversionPartyProcessing();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				h.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			h.OrganizationId = UserAccount.OrganizationId;
			h.CompanyId = UserAccount.CompanyId;
			h.BranchedId = UserAccount.BranchesId;
			h.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			h.DocumentTypeId = 176;
			h.DocNo = Conversion.ToInt((object)txtdocnumber.Text);
			h.DocDate = Conversion.ToDateTime((object)txtDocdate.Value);
			h.ProductionNo = Conversion.ToString((object)txtProductionNo.Text);
			h.DocManualRef = string.Empty;
			h.StockPartyId = Conversion.ToInt(CmbStockPartyHeader.Value);
			h.Remarks = Conversion.ToString((object)txtRemarks.Text);
			h.EntryUser = UserAccount.ID;
			h.ModifyUser = UserAccount.ID;
			h.PostUser = UserAccount.ID;
			h.EntryDate = DateTime.Now;
			h.ModifyDate = DateTime.Now;
			h.PostDate = DateTime.Now;
			h.StockConversionPartyProcessingDetail = new List<StockConversionPartyProcessingDetail>();
			h.StockConversionPartyProcessingPackingMaterial = new List<StockConversionPartyProcessingPackingMaterial>();
			rows = grdInput.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				if (Conversion.ToInt(r3.Cells["ItemId"].Value) != 0)
				{
					StockConversionPartyProcessingDetail Conversiondetail3 = new StockConversionPartyProcessingDetail();
					Conversiondetail3.Id = Conversion.ToInt(r3.Cells["Id"].Value);
					Conversiondetail3.RefDocNoId = Conversion.ToInt(r3.Cells["RefDocNoId"].Value);
					Conversiondetail3.RefDocSubIdNo = Conversion.ToInt(r3.Cells["RefDocSubIdNo"].Value);
					Conversiondetail3.RefDocumentTypeId = Conversion.ToInt(r3.Cells["RefDocumentTypeId"].Value);
					Conversiondetail3.ActionTypeId = ((Conversiondetail3.Id <= 0) ? 1 : 2);
					Conversiondetail3.StockPartyId = Conversion.ToInt(CmbStockPartyHeader.Value);
					Conversiondetail3.JobOrderId = Conversion.ToInt(r3.Cells["JobOrderId"].Value);
					Conversiondetail3.ReferencePartyId = Conversion.ToInt(r3.Cells["ReferencePartyId"].Value);
					Conversiondetail3.EntryType = Conversion.ToString((object)r3.Cells["EntryType"].Text);
					Conversiondetail3.WarehouseId = Conversion.ToInt(r3.Cells["WareHouseId"].Value);
					Conversiondetail3.ItemId = Conversion.ToInt(r3.Cells["ItemId"].Value);
					Conversiondetail3.ItemUomId = Conversion.ToInt(r3.Cells["ItemUOMId"].Value);
					Conversiondetail3.CropYearId = Conversion.ToInt(r3.Cells["CropYearId"].Value);
					Conversiondetail3.JobLotId = Conversion.ToInt(r3.Cells["JobLotId"].Value);
					Conversiondetail3.PackingtypeId = Conversion.ToInt(r3.Cells["PackingTypeId"].Value);
					Conversiondetail3.Qty = Conversion.ToDouble(r3.Cells["Quantity"].Value);
					if (Conversiondetail3.Qty == 0.0)
					{
						MessageBox.Show("Qty field Required");
						return;
					}
					Conversiondetail3.Weight = Conversion.ToDouble(r3.Cells["Weight"].Value);
					if (Conversiondetail3.Weight == 0.0)
					{
						MessageBox.Show("Weight field Required");
						return;
					}
					Conversiondetail3.ProjectId = 0;
					Conversiondetail3.PackUnit = 0;
					Conversiondetail3.Remarks = Conversion.ToString((object)r3.Cells["Remarks"].Text);
					h.StockConversionPartyProcessingDetail.Add(Conversiondetail3);
				}
			}
			rows = grdByProduct.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if (Conversion.ToInt((object)r2.Cells["ItemId"].Value.ToString()) != 0)
				{
					StockConversionPartyProcessingDetail Conversiondetail2 = new StockConversionPartyProcessingDetail();
					Conversiondetail2.Id = Conversion.ToInt(r2.Cells["Id"].Value);
					Conversiondetail2.ActionTypeId = ((Conversiondetail2.Id <= 0) ? 1 : 2);
					Conversiondetail2.StockPartyId = Conversion.ToInt(CmbStockPartyHeader.Value);
					Conversiondetail2.JobOrderId = Conversion.ToInt(r2.Cells["JobOrderId"].Value);
					Conversiondetail2.ReferencePartyId = Conversion.ToInt((object)r2.Cells["ReferencePartyId"].Text);
					Conversiondetail2.EntryType = Conversion.ToString((object)r2.Cells["EntryType"].Text);
					Conversiondetail2.WarehouseId = Conversion.ToInt(r2.Cells["WareHouseId"].Value);
					Conversiondetail2.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
					Conversiondetail2.CropYearId = Conversion.ToInt(r2.Cells["CropYearId"].Value);
					Conversiondetail2.JobLotId = Conversion.ToInt(r2.Cells["JobLotId"].Value);
					Conversiondetail2.PackingtypeId = Conversion.ToInt(r2.Cells["PackingTypeId"].Value);
					Conversiondetail2.ItemUomId = Conversion.ToInt(r2.Cells["ItemUOMId"].Value);
					Conversiondetail2.Qty = Conversion.ToDouble(r2.Cells["Quantity"].Value);
					Conversiondetail2.Weight = Conversion.ToDouble(r2.Cells["Weight"].Value);
					Conversiondetail2.ProjectId = 0;
					Conversiondetail2.PackUnit = 0;
					Conversiondetail2.Remarks = Conversion.ToString((object)r2.Cells["Remarks"].Text);
					h.StockConversionPartyProcessingDetail.Add(Conversiondetail2);
				}
			}
			if (lstRemoveRecord.Count > 0)
			{
				for (int i = 0; i < lstRemoveRecord.Count; i++)
				{
					h.StockConversionPartyProcessingDetail.Add(lstRemoveRecord[i]);
				}
			}
			rows = grdPM.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt((object)r.Cells["ItemId"].Value.ToString()) != 0)
				{
					StockConversionPartyProcessingPackingMaterial Conversiondetail = new StockConversionPartyProcessingPackingMaterial();
					Conversiondetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
					if (!(r.Cells["ItemId"].Text != string.Empty))
					{
						MessageBox.Show("PackingItem Name Field Require");
						return;
					}
					Conversiondetail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					if (!(r.Cells["ItemQTY"].Text != string.Empty))
					{
						MessageBox.Show("PackingItem Qty Field Requirein row " + r.RowIndex + 1);
						return;
					}
					Conversiondetail.ItemQty = Conversion.ToDouble(r.Cells["ItemQTY"].Value);
					if (!(r.Cells["WarehouseId"].Text != string.Empty))
					{
						MessageBox.Show("Warehouse Field Require in row " + r.RowIndex + 1);
						return;
					}
					Conversiondetail.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
					h.StockConversionPartyProcessingPackingMaterial.Add(Conversiondetail);
				}
			}
			h.AttachmentsList = new List<DMSAttachments>();
			h.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<StockConversionPartyProcessing>(h, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			int code = StockConversionPartyProcessing.Save(h);
			if (RecId == 0)
			{
				MessageBox.Show("Record Save Successfully" + h.DocNo);
			}
			else
			{
				MessageBox.Show("Record Update Successfully" + h.DocNo);
				UpdateMode = false;
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, 176, base.Name);
			}
			if (WagesStatus && WagesActiveOrInActive)
			{
				frmWagesBillPartyProcessing obj = new frmWagesBillPartyProcessing(UserAccount);
				obj.RefDocTypeId = 176;
				obj.RefDocId = code;
				obj.ShowDialog();
				obj.BringToFront();
			}
			RefreshForm();
			summeryreset();
			if (chkPrint.Checked)
			{
				CrystalReportPrint_Helper.StockConversionPartyProcessing_SummeryReport600(code);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("RecId not found");
			}
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
			RecId = Id;
			StockConversionPartyProcessing SC = StockConversionPartyProcessing.ReadById(RecId);
			if (SC != null && SC.StockConversionPartyProcessingDetail.Count > 0)
			{
				tabControl1.SelectedIndex = 0;
				btnUpdate.Visible = true;
				btnDelete.Visible = true;
				btnsave.Visible = false;
				txtdocnumber.Text = Conversion.ToString((object)SC.DocNo);
				txtDocdate.Value = Conversion.ToDateTime((object)SC.DocDate);
				txtProductionNo.Text = Conversion.ToString((object)SC.ProductionNo);
				((Control)(object)CmbStockPartyHeader).Enabled = false;
				CmbStockPartyHeader.Value = Conversion.ToInt((object)SC.StockPartyId);
				CmbStockPartyHeader_Leave(null, null);
				txtRemarks.Text = Conversion.ToString((object)SC.Remarks);
				dtInputDetail.Clear();
				dtOuputDetail.Clear();
				for (int j = 0; j < SC.StockConversionPartyProcessingDetail.Count; j++)
				{
					if (SC.StockConversionPartyProcessingDetail[j].EntryType == "Issue")
					{
						dtInputDetail.Rows.Add(SC.StockConversionPartyProcessingDetail[j].Id, SC.StockConversionPartyProcessingDetail[j].RefDocNoId, SC.StockConversionPartyProcessingDetail[j].RefDocSubIdNo, SC.StockConversionPartyProcessingDetail[j].RefDocumentTypeId, SC.StockConversionPartyProcessingDetail[j].JobOrderId, SC.StockConversionPartyProcessingDetail[j].JobOrder, SC.StockConversionPartyProcessingDetail[j].ReferencePartyId, SC.StockConversionPartyProcessingDetail[j].ReferenceParty, SC.StockConversionPartyProcessingDetail[j].EntryType, SC.StockConversionPartyProcessingDetail[j].WarehouseId, SC.StockConversionPartyProcessingDetail[j].WareHouseName, SC.StockConversionPartyProcessingDetail[j].ItemId, SC.StockConversionPartyProcessingDetail[j].ItemCode, SC.StockConversionPartyProcessingDetail[j].ItemName, SC.StockConversionPartyProcessingDetail[j].CropYearId, SC.StockConversionPartyProcessingDetail[j].CropBatch, SC.StockConversionPartyProcessingDetail[j].JobLotId, SC.StockConversionPartyProcessingDetail[j].JobLotDescription, SC.StockConversionPartyProcessingDetail[j].PackingtypeId, SC.StockConversionPartyProcessingDetail[j].PackTypeDesc, SC.StockConversionPartyProcessingDetail[j].ItemUomId, SC.StockConversionPartyProcessingDetail[j].ItemUom, SC.StockConversionPartyProcessingDetail[j].ItemUomEquivalent, SC.StockConversionPartyProcessingDetail[j].Qty, SC.StockConversionPartyProcessingDetail[j].Weight, SC.StockConversionPartyProcessingDetail[j].Remarks, SC.StockConversionPartyProcessingDetail[j].Qty, SC.StockConversionPartyProcessingDetail[j].Weight);
					}
					else if (SC.StockConversionPartyProcessingDetail[j].EntryType != "Issue")
					{
						dtOuputDetail.Rows.Add(SC.StockConversionPartyProcessingDetail[j].Id, SC.StockConversionPartyProcessingDetail[j].JobOrderId, SC.StockConversionPartyProcessingDetail[j].JobOrder, SC.StockConversionPartyProcessingDetail[j].ReferencePartyId, SC.StockConversionPartyProcessingDetail[j].ReferenceParty, SC.StockConversionPartyProcessingDetail[j].EntryType, SC.StockConversionPartyProcessingDetail[j].WarehouseId, SC.StockConversionPartyProcessingDetail[j].WareHouseName, SC.StockConversionPartyProcessingDetail[j].ItemId, SC.StockConversionPartyProcessingDetail[j].ItemCode, SC.StockConversionPartyProcessingDetail[j].ItemName, SC.StockConversionPartyProcessingDetail[j].CropYearId, SC.StockConversionPartyProcessingDetail[j].CropBatch, SC.StockConversionPartyProcessingDetail[j].JobLotId, SC.StockConversionPartyProcessingDetail[j].JobLotDescription, SC.StockConversionPartyProcessingDetail[j].PackingtypeId, SC.StockConversionPartyProcessingDetail[j].PackTypeDesc, SC.StockConversionPartyProcessingDetail[j].ItemUomId, SC.StockConversionPartyProcessingDetail[j].ItemUom, SC.StockConversionPartyProcessingDetail[j].ItemUomEquivalent, SC.StockConversionPartyProcessingDetail[j].Qty, SC.StockConversionPartyProcessingDetail[j].Weight, SC.StockConversionPartyProcessingDetail[j].Remarks);
					}
				}
				grdInput.DataSource = dtInputDetail;
				grdInput.RetrieveStructure();
				grdInputettings();
				grdByProduct.DataSource = dtOuputDetail;
				grdByProduct.RetrieveStructure();
				gridsettingsByProduct();
				dtPmDetail.Clear();
				if (SC.StockConversionPartyProcessingPackingMaterial.Count > 0)
				{
					for (int i = 0; i < SC.StockConversionPartyProcessingPackingMaterial.Count; i++)
					{
						dtPmDetail.Rows.Add(SC.StockConversionPartyProcessingPackingMaterial[i].Id, SC.StockConversionPartyProcessingPackingMaterial[i].ItemId, SC.StockConversionPartyProcessingPackingMaterial[i].ItemQty, SC.StockConversionPartyProcessingPackingMaterial[i].WarehouseId);
					}
				}
				else
				{
					AddRowInPackingMaterialGrid();
				}
				grdPM.DataSource = dtPmDetail;
				grdPM.RetrieveStructure();
				grdPackingMaterialSettings();
				FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
				txtDocdate.Focus();
				GenerateSummaryForUser();
				return;
			}
			grdHistoryDetail.ClearStructure();
			dtInputDetail.Clear();
			grdPM.ClearStructure();
			dtPmDetail.Clear();
			summeryreset();
			throw new Exception("Record not found...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
				StockConversionPartyProcessing.DeleteByID(UserAccount.ID, RecId);
				MessageBox.Show("Delete Record Successfully");
				RefreshForm();
				summeryreset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		try
		{
			resetDetail();
			RefreshForm();
			summeryreset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void RefreshForm()
	{
		try
		{
			RecId = 0;
			updateDetailIndex = -1;
			lstRemoveRecord.Clear();
			AT = new Attachment();
			Attachment.dtAttachments.Clear();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			UpdateMode = false;
			((Control)(object)CmbStockPartyHeader).Enabled = true;
			((Control)(object)CmbStockPartyHeader).Text = string.Empty;
			((Control)(object)CmbJobOrder).Text = string.Empty;
			((UltraGridBase)CmbJobOrder).DataSource = null;
			txtProductionNo.Text = string.Empty;
			txtRemarks.Text = string.Empty;
			GenerateDocNumber();
			txtQty.Text = string.Empty;
			txtUnitWeight.Text = string.Empty;
			dtInputDetail.Clear();
			grdInput.ClearStructure();
			grdByProduct.ClearStructure();
			dtOuputDetail.Rows.Clear();
			grdPM.ClearStructure();
			dtExpense.Clear();
			dtPmDetail.Clear();
			AddRowInPackingMaterialGrid();
			grdPM.RetrieveStructure();
			grdPackingMaterialSettings();
			txtDocdate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void resetDetail()
	{
		try
		{
			updateDetailIndex = -1;
			((Control)(object)CmbJobOrder).Text = string.Empty;
			cmbGodown.Value = string.Empty;
			((Control)(object)cmbEntryType).Text = string.Empty;
			CmbCropyr.Value = string.Empty;
			cmbItem.Value = string.Empty;
			cmbUOM.Value = string.Empty;
			cmbLot.Value = string.Empty;
			cmbBagType.Value = string.Empty;
			txtUnitWeight.Text = string.Empty;
			txtQty.Text = string.Empty;
			txtStock.Text = 0.ToString();
			txtBalanceWeight.Text = 0.ToString();
			txtdeailRemarks.Text = string.Empty;
			CmbJobOrder.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void summeryreset()
	{
		txtInputQuantity.Text = string.Empty;
		txtInputWeight.Text = string.Empty;
		txtOutputByProductQty.Text = string.Empty;
		txtOutputByProductWeight.Text = string.Empty;
		txtFinishGoodsQty.Text = string.Empty;
		txtFinishGoodsWeight.Text = string.Empty;
		txtPackingMaterialQty.Text = string.Empty;
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			StockPartyFill();
			Warehouse();
			ItemFill();
			CropYear();
			combojoblotfill();
			bagType();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void button1_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void invfrmStockConversionPartyProcessing_FormClosed(object sender, FormClosedEventArgs e)
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
				txtDocdate.Focus();
			}
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
			txtFromDocNoHistory.Text = string.Empty;
			txtToDocNoHistory.Text = string.Empty;
			FromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			ToDateHistory.Value = DateTime.Now;
			drdocdate.Checked = true;
			GrdHistoryMain.ClearStructure();
			grdHistoryPM.ClearStructure();
			grdHistoryDetail.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefreshHistory_Click(object sender, EventArgs e)
	{
	}

	private void btnshowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			GridHistoryMainFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridHistoryMainFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = 176;
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = clsGlobalVariables.UserAccount.ID;
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
			obj.FromDocNo = Conversion.ToInt((object)txtFromDocNoHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text);
			DataTable dth = StockConversionPartyProcessing.FormHistory(obj);
			if (dth.Rows.Count > 0)
			{
				DataTable dtTarget = new DataTable();
				dtTarget.Columns.Add("Id", typeof(int));
				dtTarget.Columns.Add("DocumentTypeId", typeof(int));
				dtTarget.Columns.Add("DocNo", typeof(int));
				dtTarget.Columns.Add("DocDate", typeof(DateTime));
				dtTarget.Columns.Add("ProductionNo", typeof(string));
				dtTarget.Columns.Add("ManualNo", typeof(string));
				dtTarget.Columns.Add("StockPartyId", typeof(int));
				dtTarget.Columns.Add("StockPartyName", typeof(string));
				dtTarget.Columns.Add("Remarks", typeof(string));
				dtTarget.Columns.Add("EntryDate", typeof(DateTime));
				dtTarget.Columns.Add("EntryUser", typeof(string));
				dtTarget.Columns.Add("ModifyDate", typeof(DateTime));
				dtTarget.Columns.Add("ModifyUser", typeof(string));
				dtTarget.Columns.Add("NoOfAttachments", typeof(int));
				foreach (DataRow row in dth.Rows)
				{
					dtTarget.Rows.Add(row["Id"], row["DocumentTypeId"], row["DocNo"], row["DocDate"], row["ProductionNo"], row["DocManualRef"], row["StockPartyId"], row["StockPartyName"], row["Remarks"], row["EntryDate"], row["EntryUser"], row["ModifyDate"], row["ModifyUser"], row["NoOfAttachments"]);
				}
				GrdHistoryMain.DataSource = dtTarget;
				GrdHistoryMain.RetrieveStructure();
				GridHistoryMainSetting();
			}
			else
			{
				GrdHistoryMain.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridHistoryMainSetting()
	{
		try
		{
			GrdHistoryMain.RootTable.Columns["Id"].Visible = false;
			GrdHistoryMain.RootTable.Columns["DocumentTypeId"].Visible = false;
			GrdHistoryMain.RootTable.Columns["StockPartyId"].Visible = false;
			GrdHistoryMain.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			GrdHistoryMain.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			GrdHistoryMain.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			CommonServices.GridAutoAdjustmentNew(GrdHistoryMain);
			GrdHistoryMain.RootTable.Columns["Remarks"].Width = 260;
			GrdHistoryMain.RootTable.Columns.Add("Edit");
			GrdHistoryMain.RootTable.Columns["Edit"].Key = "Edit";
			GrdHistoryMain.RootTable.Columns["Edit"].Caption = "Edit";
			GrdHistoryMain.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistoryMain.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			GrdHistoryMain.RootTable.Columns["Edit"].ButtonText = "Edit";
			GrdHistoryMain.RootTable.Columns["Edit"].Width = 50;
			GrdHistoryMain.RootTable.Columns["Edit"].Position = 1;
			GrdHistoryMain.RootTable.Columns.Add("Print");
			GrdHistoryMain.RootTable.Columns["Print"].Key = "Print";
			GrdHistoryMain.RootTable.Columns["Print"].Caption = "Print";
			GrdHistoryMain.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistoryMain.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			GrdHistoryMain.RootTable.Columns["Print"].ButtonText = "Print";
			GrdHistoryMain.RootTable.Columns["Print"].Width = 50;
			GrdHistoryMain.RootTable.Columns["Print"].Position = 0;
			GrdHistoryMain.FrozenColumns = 2;
			ctrlGrdBar3_Load(null, null);
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
			GridEXRow r = GrdHistoryMain.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByScreenName(Conversion.ToInt(r.Cells["Id"].Value), base.Name);
			}
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
			GridEXRow r = GrdHistoryMain.CurrentRow;
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

	private void DatagridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = GrdHistoryMain.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				if (e.Column.Key == "Print")
				{
					CrystalReportPrint_Helper.StockConversionPartyProcessing_SummeryReport600(Conversion.ToInt(r.Cells["Id"].Value));
				}
				if (e.Column.Key == "Edit")
				{
					ReadById(Conversion.ToInt(r.Cells["Id"].Value));
				}
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
			GridEXRow r = GrdHistoryMain.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				HistoryDetailsBind(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryDetailsBind(int CurrId)
	{
		try
		{
			if (CurrId <= 0)
			{
				return;
			}
			StockConversionPartyProcessing SC = StockConversionPartyProcessing.ReadById(CurrId);
			if (SC != null && SC.StockConversionPartyProcessingDetail.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("JobOrder");
				dt.Columns.Add("ReferencePartyId", typeof(int));
				dt.Columns.Add("ReferenceParty");
				dt.Columns.Add("EntryType");
				dt.Columns.Add("WareHouseId", typeof(int));
				dt.Columns.Add("WareHouse");
				dt.Columns.Add("ItemId", typeof(int));
				dt.Columns.Add("ItemCode");
				dt.Columns.Add("Item");
				dt.Columns.Add("CropYearId", typeof(int));
				dt.Columns.Add("CropYear");
				dt.Columns.Add("JobLotId", typeof(int));
				dt.Columns.Add("JobLot");
				dt.Columns.Add("PackingTypeId", typeof(int));
				dt.Columns.Add("PackingType");
				dt.Columns.Add("ItemUOMId", typeof(int));
				dt.Columns.Add("PackUom");
				dt.Columns.Add("PackUomEquivalent", typeof(double));
				dt.Columns.Add("Quantity", typeof(double));
				dt.Columns.Add("Weight", typeof(double));
				dt.Columns.Add("Remarks");
				for (int j = 0; j < SC.StockConversionPartyProcessingDetail.Count; j++)
				{
					dt.Rows.Add(SC.StockConversionPartyProcessingDetail[j].Id, SC.StockConversionPartyProcessingDetail[j].JobOrder, SC.StockConversionPartyProcessingDetail[j].ReferencePartyId, SC.StockConversionPartyProcessingDetail[j].ReferenceParty, SC.StockConversionPartyProcessingDetail[j].EntryType, SC.StockConversionPartyProcessingDetail[j].WarehouseId, SC.StockConversionPartyProcessingDetail[j].WareHouseName, SC.StockConversionPartyProcessingDetail[j].ItemId, SC.StockConversionPartyProcessingDetail[j].ItemCode, SC.StockConversionPartyProcessingDetail[j].ItemName, SC.StockConversionPartyProcessingDetail[j].CropYearId, SC.StockConversionPartyProcessingDetail[j].CropBatch, SC.StockConversionPartyProcessingDetail[j].JobLotId, SC.StockConversionPartyProcessingDetail[j].JobLotDescription, SC.StockConversionPartyProcessingDetail[j].PackingtypeId, SC.StockConversionPartyProcessingDetail[j].PackTypeDesc, SC.StockConversionPartyProcessingDetail[j].ItemUomId, SC.StockConversionPartyProcessingDetail[j].ItemUom, SC.StockConversionPartyProcessingDetail[j].ItemUomEquivalent, SC.StockConversionPartyProcessingDetail[j].Qty, SC.StockConversionPartyProcessingDetail[j].Weight, SC.StockConversionPartyProcessingDetail[j].Remarks);
				}
				grdHistoryDetail.DataSource = dt;
				grdHistoryDetail.RetrieveStructure();
				grdHistoryDetailSettings();
				DataTable dtpack = new DataTable();
				dtpack.Columns.Add("Item");
				dtpack.Columns.Add("ItemQTY", typeof(double));
				dtpack.Columns.Add("WareHouse");
				for (int i = 0; i < SC.StockConversionPartyProcessingPackingMaterial.Count; i++)
				{
					dtpack.Rows.Add(SC.StockConversionPartyProcessingPackingMaterial[i].ItemName, SC.StockConversionPartyProcessingPackingMaterial[i].ItemQty, SC.StockConversionPartyProcessingPackingMaterial[i].WareHouseName);
				}
				grdHistoryPM.DataSource = dtpack;
				grdHistoryPM.RetrieveStructure();
				grdHistoryPmSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryDetailSettings()
	{
		try
		{
			grdHistoryDetail.RootTable.Columns["Id"].Visible = false;
			grdHistoryDetail.RootTable.Columns["ReferencePartyId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["WareHouseId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["ItemId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["ItemUOMId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["JobLotId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["PackingTypeId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["CropYearId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["PackUomEquivalent"].Visible = false;
			grdHistoryDetail.RootTable.Columns["Quantity"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["Quantity"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["Quantity"].FormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["Quantity"].TotalFormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["Weight"].FormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["Weight"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustmentNew(grdHistoryDetail);
			grdHistoryDetail.RootTable.Columns["Remarks"].Width = 200;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdHistoryPmSetting()
	{
		try
		{
			grdHistoryPM.RootTable.Columns["Item"].Width = 280;
			grdHistoryPM.RootTable.Columns["ItemQTY"].AggregateFunction = (AggregateFunction)2;
			grdHistoryPM.RootTable.Columns["ItemQTY"].TextAlignment = (TextAlignment)3;
			grdHistoryPM.RootTable.Columns["ItemQTY"].FormatString = "#,##";
			grdHistoryPM.RootTable.Columns["ItemQTY"].TotalFormatString = "#,##";
			CommonServices.GridAutoAdjustmentNew(grdHistoryPM);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbUOM_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbUOM).ActiveRow != null && Conversion.ToInt(cmbUOM.Value) != 0)
			{
				double ItemUom = Conversion.ToDouble(((UltraDropDownBase)cmbUOM).SelectedRow.Cells["Equivalent"].Value);
				double qty = Conversion.ToDouble((object)txtQty.Text);
				if (ItemUom > 0.0 && qty > 0.0)
				{
					double NetWeight = ItemUom * qty;
					txtUnitWeight.Text = Conversion.ToDouble((object)NetWeight).ToString("#,##0.###");
				}
				else
				{
					txtUnitWeight.Text = 0.ToString();
				}
			}
			else
			{
				txtUnitWeight.Text = 0.ToString();
			}
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
			if (((UltraGridBase)cmbUOM).ActiveRow != null && Conversion.ToInt(cmbUOM.Value) != 0)
			{
				double ItemUom = Conversion.ToDouble(((UltraDropDownBase)cmbUOM).SelectedRow.Cells["Equivalent"].Value);
				double qty = Conversion.ToDouble((object)txtQty.Text);
				if (ItemUom > 0.0 && qty > 0.0)
				{
					double NetWeight = ItemUom * qty;
					txtUnitWeight.Text = Conversion.ToDouble((object)NetWeight).ToString("#,##0.###");
				}
				else
				{
					txtUnitWeight.Text = 0.ToString();
				}
			}
			else
			{
				txtUnitWeight.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetBalQtyAndWeightForPartyProcessing()
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		try
		{
			if (((UltraGridBase)CmbStockPartyHeader).ActiveRow != null && Conversion.ToInt(CmbStockPartyHeader.Value) != 0 && ((UltraGridBase)cmbGodown).ActiveRow != null && Conversion.ToInt(cmbGodown.Value) != 0 && ((UltraGridBase)cmbItem).ActiveRow != null && Conversion.ToInt(cmbItem.Value) != 0)
			{
				DataTable dt = GetAvgRatesAndStockInHand.GetBalQtyAndWeightForPartyProcessing(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					ItemId = Conversion.ToInt(cmbItem.Value),
					StockPartyId = Conversion.ToInt(CmbStockPartyHeader.Value),
					WarehouseId = Conversion.ToInt(cmbGodown.Value)
				});
				if (dt.Rows.Count > 0)
				{
					txtStock.Text = Conversion.ToInt(dt.Rows[0]["BalQty"]).ToString();
					txtBalanceWeight.Text = Conversion.ToInt(dt.Rows[0]["BalStock"]).ToString();
				}
				else
				{
					txtStock.Text = string.Empty;
					txtBalanceWeight.Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GetCurrentItemStockByWarehouseByJobLotAndCropYear()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		try
		{
			DataTable dtGetCurrentItemStock = InvStockConversion.ItemStockFilterFromInventoryTransaction(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				WarehouseId = Conversion.ToInt(cmbGodown.Value),
				ItemId = Conversion.ToInt(cmbItem.Value),
				JobLotId = Conversion.ToInt(cmbLot.Value),
				ToDate = DateTime.Now,
				CropYear = Conversion.ToString((object)((Control)(object)CmbCropyr).Text),
				Activity = "GetCurrentItemStockByWarehouseByJobLotAndCropYear"
			});
			if (dtGetCurrentItemStock.Rows.Count > 0)
			{
				double stockAvailable = Math.Round(Conversion.ToDouble(dtGetCurrentItemStock.Rows[0]["AvailableItemStock"]), 2);
				txtStock.Text = stockAvailable.ToString();
			}
			else
			{
				txtStock.Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateSummaryForUser()
	{
		try
		{
			summeryreset();
			string QtyInput = Conversion.ToString(grdInput.GetTotal(grdInput.RootTable.Columns["Quantity"], (AggregateFunction)2));
			txtInputQuantity.Text = QtyInput;
			string WeightInput = Conversion.ToString(grdInput.GetTotal(grdInput.RootTable.Columns["Weight"], (AggregateFunction)2));
			txtInputWeight.Text = WeightInput;
			GridEXRow[] rows = grdByProduct.GetRows();
			foreach (GridEXRow r in rows)
			{
				r.BeginEdit();
				if (grdByProduct.GetRows().Count() == 1)
				{
					if (r.Cells["EntryType"].Text == "Recovery By Product")
					{
						txtOutputByProductQty.Text = Conversion.ToString((object)r.Cells["Quantity"].Text);
						txtOutputByProductWeight.Text = Conversion.ToString((object)r.Cells["Weight"].Text);
					}
					else if (r.Cells["EntryType"].Text == "Recovery Head Rice")
					{
						txtFinishGoodsQty.Text = Conversion.ToString((object)r.Cells["Quantity"].Text);
						txtFinishGoodsWeight.Text = Conversion.ToString((object)r.Cells["Weight"].Text);
					}
				}
				else if (grdByProduct.GetRows().Count() > 1)
				{
					if (r.Cells["EntryType"].Text == "Recovery By Product")
					{
						double OutputByProductQty = Conversion.ToDouble((object)txtOutputByProductQty.Text) + Conversion.ToDouble(r.Cells["Quantity"].Value);
						txtOutputByProductQty.Text = OutputByProductQty.ToString();
						double OutputByProductWeight = Conversion.ToDouble((object)txtOutputByProductWeight.Text) + Conversion.ToDouble(r.Cells["Weight"].Value);
						txtOutputByProductWeight.Text = OutputByProductWeight.ToString();
					}
					else if (r.Cells["EntryType"].Text == "Recovery Head Rice")
					{
						double FinishGoodsQty = Conversion.ToDouble((object)txtFinishGoodsQty.Text) + Conversion.ToDouble(r.Cells["Quantity"].Value);
						txtFinishGoodsQty.Text = FinishGoodsQty.ToString();
						double FinishGoodsWeight = Conversion.ToDouble((object)txtFinishGoodsWeight.Text) + Conversion.ToDouble(r.Cells["Weight"].Value);
						txtFinishGoodsWeight.Text = FinishGoodsWeight.ToString();
					}
				}
			}
			double Qty = Conversion.ToDouble(grdPM.GetTotal(grdPM.RootTable.Columns["ItemQTY"], (AggregateFunction)2));
			txtPackingMaterialQty.Text = Qty.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbStockPartyHeader_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)CmbStockPartyHeader).Text != string.Empty)
			{
				BindReferenceParty();
				GetBalQtyAndWeightForPartyProcessing();
			}
			else
			{
				((Control)(object)cmbReferenceParty).Text = string.Empty;
				((UltraGridBase)cmbReferenceParty).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void cmbGodown_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)cmbEntryType).Text == "Issue")
			{
				GetBalQtyAndWeightForPartyProcessing();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Print_Click(object sender, EventArgs e)
	{
		try
		{
			CrystalReportPrint_Helper.StockConversionPartyProcessing_SummeryReport600(RecId);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdByProduct).Name);
			if (fs != null)
			{
				grdByProduct.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdInput).Name);
			if (fs != null)
			{
				grdInput.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GrdHistoryMain).Name);
			if (fs != null)
			{
				GrdHistoryMain.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtUnitWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void invfrmStockConversionProduction_KeyDown(object sender, KeyEventArgs e)
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
					txtDocdate.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					FromDateHistory.Focus();
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
				if (e.Control && e.KeyCode == Keys.S && btnsave.Visible && btnsave.Enabled)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.Shift && e.KeyCode == Keys.Delete && btnDelete.Visible && btnDelete.Enabled)
				{
					btnDelete_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
				{
					btnUpdate_Click(null, null);
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
					txtDocdate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					cmbReferenceParty.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					tabControl2.SelectedIndex = 0;
					((Control)(object)grdInput).Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					btnattachment_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.P)
				{
					Print_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1))
				{
					Print_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grdInput).Focused)
					{
						tabControl2.SelectedIndex = 1;
						((Control)(object)grdByProduct).Focus();
					}
					else if (((Control)(object)grdByProduct).Focused)
					{
						tabControl2.SelectedIndex = 2;
						((Control)(object)grdPM).Focus();
					}
					else
					{
						tabControl2.SelectedIndex = 0;
						((Control)(object)grdInput).Focus();
					}
				}
				return;
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshowHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				FromDateHistory.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)GrdHistoryMain).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				FromDateHistory.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)GrdHistoryMain).Focused)
				{
					((Control)(object)grdHistoryDetail).Focus();
				}
				else if (((Control)(object)grdHistoryDetail).Focused)
				{
					((Control)(object)grdHistoryPM).Focus();
				}
				else
				{
					((Control)(object)GrdHistoryMain).Focus();
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
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+Shift+Delete", "For Delete");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print Slip");
			dt.Rows.Add("Alt+1", "For Print Slip");
			dt.Rows.Add("Ctrl+F5", "For Focus on Plan Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowRight", "For Change Focus from one Grid To another Grid");
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

	private void GrdHistoryMain_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = GrdHistoryMain.CurrentRow;
			if (r == null || (int)r.RowType != 0 || GrdHistoryMain.CurrentColumn == null)
			{
				return;
			}
			int CurrId = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (GrdHistoryMain.CurrentColumn.Key == "Edit")
				{
					ReadById(CurrId);
				}
				if (GrdHistoryMain.CurrentColumn.Key == "Print")
				{
					CrystalReportPrint_Helper.StockConversionPartyProcessing_SummeryReport600(CurrId);
				}
				if (GrdHistoryMain.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByScreenName(CurrId, base.Name);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				ReadById(CurrId);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				CrystalReportPrint_Helper.StockConversionPartyProcessing_SummeryReport600(CurrId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdByProduct_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		try
		{
			GridEXRow item = grdByProduct.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdByProduct.CurrentColumn == null)
			{
				return;
			}
			Conversion.ToInt(item.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Space && grdByProduct.CurrentColumn.Key == "Delete")
			{
				if (updateDetailIndex != -1)
				{
					throw new Exception("Reset the Detail First...");
				}
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				if (btnsave.Visible && btnsave.Enabled)
				{
					item.Delete();
				}
				else if (Conversion.ToInt(item.Cells["Id"].Value) > 0)
				{
					StockConversionPartyProcessingDetail Conversiondetail2 = new StockConversionPartyProcessingDetail();
					Conversiondetail2.Id = Conversion.ToInt(item.Cells["Id"].Value);
					Conversiondetail2.StockPartyId = Conversion.ToInt(CmbStockPartyHeader.Value);
					Conversiondetail2.JobOrderId = Conversion.ToInt((object)item.Cells["JobOrderId"].Text);
					Conversiondetail2.ReferencePartyId = Conversion.ToInt((object)item.Cells["ReferencePartyId"].Text);
					Conversiondetail2.EntryType = Conversion.ToString((object)item.Cells["EntryType"].Text);
					Conversiondetail2.WarehouseId = Conversion.ToInt(item.Cells["WareHouseId"].Value);
					Conversiondetail2.ItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
					Conversiondetail2.CropYearId = Conversion.ToInt(item.Cells["CropYearId"].Value);
					Conversiondetail2.JobLotId = Conversion.ToInt(item.Cells["JobLotId"].Value);
					Conversiondetail2.PackingtypeId = Conversion.ToInt(item.Cells["PackingTypeId"].Value);
					Conversiondetail2.ItemUomId = Conversion.ToInt(item.Cells["ItemUOMId"].Value);
					Conversiondetail2.Qty = Conversion.ToDouble(item.Cells["Quantity"].Value);
					Conversiondetail2.Weight = Conversion.ToDouble(item.Cells["Weight"].Value);
					Conversiondetail2.ProjectId = 0;
					Conversiondetail2.PackUnit = 0;
					Conversiondetail2.Remarks = Conversion.ToString((object)item.Cells["Remarks"].Text);
					Conversiondetail2.ActionTypeId = 3;
					lstRemoveRecord.Add(Conversiondetail2);
					item.Delete();
					grdByProduct.UpdateData();
				}
				else
				{
					item.Delete();
				}
				if (grdInput.GetRows().Count() == 0 && grdByProduct.GetRows().Count() == 0)
				{
					((Control)(object)CmbStockPartyHeader).Enabled = true;
				}
				GenerateSummaryForUser();
			}
			if (!e.Control || e.KeyCode != Keys.Delete)
			{
				return;
			}
			if (updateDetailIndex == -1)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					if (btnsave.Visible && btnsave.Enabled)
					{
						item.Delete();
					}
					else if (Conversion.ToInt(item.Cells["Id"].Value) > 0)
					{
						StockConversionPartyProcessingDetail Conversiondetail = new StockConversionPartyProcessingDetail();
						Conversiondetail.Id = Conversion.ToInt(item.Cells["Id"].Value);
						Conversiondetail.StockPartyId = Conversion.ToInt(CmbStockPartyHeader.Value);
						Conversiondetail.JobOrderId = Conversion.ToInt((object)item.Cells["JobOrderId"].Text);
						Conversiondetail.ReferencePartyId = Conversion.ToInt((object)item.Cells["ReferencePartyId"].Text);
						Conversiondetail.EntryType = Conversion.ToString((object)item.Cells["EntryType"].Text);
						Conversiondetail.WarehouseId = Conversion.ToInt(item.Cells["WareHouseId"].Value);
						Conversiondetail.ItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
						Conversiondetail.CropYearId = Conversion.ToInt(item.Cells["CropYearId"].Value);
						Conversiondetail.JobLotId = Conversion.ToInt(item.Cells["JobLotId"].Value);
						Conversiondetail.PackingtypeId = Conversion.ToInt(item.Cells["PackingTypeId"].Value);
						Conversiondetail.ItemUomId = Conversion.ToInt(item.Cells["ItemUOMId"].Value);
						Conversiondetail.Qty = Conversion.ToDouble(item.Cells["Quantity"].Value);
						Conversiondetail.Weight = Conversion.ToDouble(item.Cells["Weight"].Value);
						Conversiondetail.ProjectId = 0;
						Conversiondetail.PackUnit = 0;
						Conversiondetail.Remarks = Conversion.ToString((object)item.Cells["Remarks"].Text);
						Conversiondetail.ActionTypeId = 3;
						lstRemoveRecord.Add(Conversiondetail);
						item.Delete();
						grdByProduct.UpdateData();
					}
					else
					{
						item.Delete();
					}
					if (grdInput.GetRows().Count() == 0 && grdByProduct.GetRows().Count() == 0)
					{
						((Control)(object)CmbStockPartyHeader).Enabled = true;
					}
					GenerateSummaryForUser();
				}
				return;
			}
			throw new Exception("Reset the Detail First...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPM_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPM.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdPM.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdPM.CurrentColumn != null && grdPM.CurrentColumn.Key == "Delete")
				{
					dtPmDetail.Rows[r.RowIndex].Delete();
					r.Delete();
					if (dtPmDetail.Rows.Count == 0)
					{
						AddRowInPackingMaterialGrid();
					}
				}
				if (grdPM.CurrentColumn != null && grdPM.CurrentColumn.Key == "Add")
				{
					AddRowInPackingMaterialGrid();
				}
				grdPM.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				dtPmDetail.Rows[r.RowIndex].Delete();
				r.Delete();
				if (dtPmDetail.Rows.Count == 0)
				{
					AddRowInPackingMaterialGrid();
				}
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInPackingMaterialGrid();
			}
			GenerateSummaryForUser();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInput_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0403: Unknown result type (might be due to invalid IL or missing references)
		//IL_0409: Expected O, but got Unknown
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		try
		{
			GridEXRow item = grdInput.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdInput.CurrentColumn == null)
			{
				return;
			}
			Conversion.ToInt(item.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Space && grdInput.CurrentColumn.Key == "Delete")
			{
				if (updateDetailIndex != -1)
				{
					throw new Exception("Reset the Detail First...");
				}
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				if (btnsave.Visible && btnsave.Enabled)
				{
					item.Delete();
				}
				else if (Conversion.ToInt(item.Cells["Id"].Value) > 0)
				{
					StockConversionPartyProcessingDetail d2 = new StockConversionPartyProcessingDetail();
					d2.Id = Conversion.ToInt(item.Cells["Id"].Value);
					d2.RefDocNoId = Conversion.ToInt(item.Cells["RefDocNoId"].Value);
					d2.RefDocSubIdNo = Conversion.ToInt(item.Cells["RefDocSubIdNo"].Value);
					d2.RefDocumentTypeId = Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value);
					d2.StockPartyId = Conversion.ToInt(CmbStockPartyHeader.Value);
					d2.JobOrderId = Conversion.ToInt((object)item.Cells["JobOrderId"].Text);
					d2.ReferencePartyId = Conversion.ToInt(item.Cells["ReferencePartyId"].Value);
					d2.EntryType = Conversion.ToString((object)item.Cells["EntryType"].Text);
					d2.WarehouseId = Conversion.ToInt(item.Cells["WareHouseId"].Value);
					d2.ItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
					d2.CropYearId = Conversion.ToInt(item.Cells["CropYearId"].Value);
					d2.JobLotId = Conversion.ToInt(item.Cells["JobLotId"].Value);
					d2.PackingtypeId = Conversion.ToInt(item.Cells["PackingTypeId"].Value);
					d2.ItemUomId = Conversion.ToInt(item.Cells["ItemUOMId"].Value);
					d2.Qty = Conversion.ToDouble(item.Cells["Quantity"].Value);
					d2.Weight = Conversion.ToDouble(item.Cells["Weight"].Value);
					d2.ProjectId = 0;
					d2.PackUnit = 0;
					d2.Remarks = Conversion.ToString((object)item.Cells["Remarks"].Text);
					d2.ActionTypeId = 3;
					lstRemoveRecord.Add(d2);
					item.Delete();
					grdInput.UpdateData();
				}
				else
				{
					item.Delete();
				}
				if (grdInput.GetRows().Count() == 0 && grdByProduct.GetRows().Count() == 0)
				{
					((Control)(object)CmbStockPartyHeader).Enabled = true;
				}
				GenerateSummaryForUser();
			}
			if (!e.Control || e.KeyCode != Keys.Delete)
			{
				return;
			}
			if (updateDetailIndex == -1)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					if (btnsave.Visible && btnsave.Enabled)
					{
						item.Delete();
					}
					else if (Conversion.ToInt(item.Cells["Id"].Value) > 0)
					{
						StockConversionPartyProcessingDetail d = new StockConversionPartyProcessingDetail();
						d.Id = Conversion.ToInt(item.Cells["Id"].Value);
						d.RefDocNoId = Conversion.ToInt(item.Cells["RefDocNoId"].Value);
						d.RefDocSubIdNo = Conversion.ToInt(item.Cells["RefDocSubIdNo"].Value);
						d.RefDocumentTypeId = Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value);
						d.StockPartyId = Conversion.ToInt(CmbStockPartyHeader.Value);
						d.JobOrderId = Conversion.ToInt((object)item.Cells["JobOrderId"].Text);
						d.ReferencePartyId = Conversion.ToInt(item.Cells["ReferencePartyId"].Value);
						d.EntryType = Conversion.ToString((object)item.Cells["EntryType"].Text);
						d.WarehouseId = Conversion.ToInt(item.Cells["WareHouseId"].Value);
						d.ItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
						d.CropYearId = Conversion.ToInt(item.Cells["CropYearId"].Value);
						d.JobLotId = Conversion.ToInt(item.Cells["JobLotId"].Value);
						d.PackingtypeId = Conversion.ToInt(item.Cells["PackingTypeId"].Value);
						d.ItemUomId = Conversion.ToInt(item.Cells["ItemUOMId"].Value);
						d.Qty = Conversion.ToDouble(item.Cells["Quantity"].Value);
						d.Weight = Conversion.ToDouble(item.Cells["Weight"].Value);
						d.ProjectId = 0;
						d.PackUnit = 0;
						d.Remarks = Conversion.ToString((object)item.Cells["Remarks"].Text);
						d.ActionTypeId = 3;
						lstRemoveRecord.Add(d);
						item.Delete();
						grdInput.UpdateData();
					}
					else
					{
						item.Delete();
					}
					if (grdInput.GetRows().Count() == 0 && grdByProduct.GetRows().Count() == 0)
					{
						((Control)(object)CmbStockPartyHeader).Enabled = true;
					}
					GenerateSummaryForUser();
				}
				return;
			}
			throw new Exception("Reset the Detail First...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbStockPartyHeader_Leave(object sender, EventArgs e)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(CmbStockPartyHeader.Value) > 0)
			{
				int Id = Conversion.ToInt(CmbJobOrder.Value);
				DataTable dtPJO = InvProductionJobOrderPartyProcessing.GetJobOrderNoForConversion(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					BranchesId = UserAccount.BranchesId,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id,
					DocumentTypeId = 120,
					StockPartyId = Conversion.ToInt(CmbStockPartyHeader.Value)
				});
				if (dtPJO.Rows.Count > 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("Id", typeof(int));
					dt.Columns.Add("PlanCode");
					for (int i = 0; i < dtPJO.Rows.Count; i++)
					{
						dt.Rows.Add(dtPJO.Rows[i]["Id"], dtPJO.Rows[i]["PlanCode"]);
					}
					if (dt.Rows.Count > 0)
					{
						DDL.BindDDL(dt, CmbJobOrder, "Id", "PlanCode", "JobOrderNo", ZeroIndex: true);
						if (Id > 0)
						{
							if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
							{
								CmbJobOrder.Value = Id;
							}
							else
							{
								((Control)(object)CmbJobOrder).Text = string.Empty;
							}
						}
					}
					else
					{
						((Control)(object)CmbJobOrder).Text = string.Empty;
						((UltraGridBase)CmbJobOrder).DataSource = null;
					}
				}
				else
				{
					((Control)(object)CmbJobOrder).Text = string.Empty;
					((UltraGridBase)CmbJobOrder).DataSource = null;
				}
			}
			else
			{
				((Control)(object)CmbJobOrder).Text = string.Empty;
				((UltraGridBase)CmbJobOrder).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadInput_Click(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbStockPartyHeader).ActiveRow == null || Conversion.ToInt(CmbStockPartyHeader.Value) == 0)
			{
				CmbStockPartyHeader.Focus();
				throw new Exception("Stock Party Account select First");
			}
			if (grdInput.GetRows().Count() == 0)
			{
				Loader = new LoadavailableTransactionsForIssuancePartyProcessing(UserAccount);
				Loader.StockPartyId = Conversion.ToInt(CmbStockPartyHeader.Value);
				Loader.ShowDialog();
				Loader.BringToFront();
				LoadInInputGridDetail();
				return;
			}
			GridEXRow r = grdInput.GetRow(0);
			if (Conversion.ToInt(r.Cells["RefDocNoId"].Value) > 0 || Conversion.ToInt(r.Cells["RefDocSubIdNo"].Value) > 0 || Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value) > 0)
			{
				Loader = new LoadavailableTransactionsForIssuancePartyProcessing(UserAccount);
				Loader.StockPartyId = Conversion.ToInt(CmbStockPartyHeader.Value);
				Loader.ShowDialog();
				Loader.BringToFront();
				LoadInInputGridDetail();
				return;
			}
			throw new Exception("You Can't Load Because Direct Entry Already Exist");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInInputGridDetail()
	{
		try
		{
			DataTable dt = Loader.dtIssuance;
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			if (grdInput.GetRows().Count() > 0 && Conversion.ToInt(CmbStockPartyHeader.Value) != Conversion.ToInt(dt.Rows[0]["StockPartyId"]))
			{
				throw new Exception("Already Loaded Row's Have Different Stock Party. So you Can't Load Rows Of Different Stock Party!");
			}
			bool flag = false;
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (grdInput.GetRows().Count() > 0)
				{
					GridEXRow[] rows = grdInput.GetRows();
					foreach (GridEXRow item in rows)
					{
						if (Conversion.ToInt(item.Cells["RefDocNoId"].Value) == Conversion.ToInt(dt.Rows[i]["RefDocIdNo"]) && Conversion.ToInt(item.Cells["RefDocSubIdNo"].Value) == Conversion.ToInt(dt.Rows[i]["RefDocSubIdNo"]) && Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) == Conversion.ToInt(dt.Rows[i]["RefDocumentTypeId"]))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					dtInputDetail.Rows.Add(0, dt.Rows[i]["RefDocIdNo"], dt.Rows[i]["RefDocSubIdNo"], dt.Rows[i]["RefDocumentTypeId"], dt.Rows[i]["JobOrderId"], dt.Rows[i]["JobOrderNo"], dt.Rows[i]["SupplierCustomerId"], dt.Rows[i]["ReferencePartyName"], "Issue", dt.Rows[i]["WarehouseId"], dt.Rows[i]["WareHouseCode"], dt.Rows[i]["ItemId"], dt.Rows[i]["ItemCode"], dt.Rows[i]["ItemName"], dt.Rows[i]["CropYearId"], dt.Rows[i]["CropYear"], dt.Rows[i]["JobLotId"], dt.Rows[i]["JobLotCode"], dt.Rows[i]["InvPackingTypeId"], dt.Rows[i]["PackingType"], dt.Rows[i]["ItemUomId"], dt.Rows[i]["PackUom"], dt.Rows[i]["Equivalent"], dt.Rows[i]["QtyBalance"], dt.Rows[i]["WeightBalance"], "", dt.Rows[i]["QtyBalance"], dt.Rows[i]["WeightBalance"]);
				}
				flag = false;
			}
			grdInput.DataSource = dtInputDetail;
			grdInput.RetrieveStructure();
			grdInputettings();
			((Control)(object)CmbStockPartyHeader).Enabled = false;
			GenerateSummaryForUser();
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
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Expected O, but got Unknown
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Expected O, but got Unknown
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Expected O, but got Unknown
		//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Expected O, but got Unknown
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Expected O, but got Unknown
		//IL_043b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Expected O, but got Unknown
		//IL_059b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Expected O, but got Unknown
		//IL_05f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fd: Expected O, but got Unknown
		//IL_0614: Unknown result type (might be due to invalid IL or missing references)
		//IL_061e: Expected O, but got Unknown
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		//IL_063f: Expected O, but got Unknown
		//IL_0682: Unknown result type (might be due to invalid IL or missing references)
		//IL_068c: Expected O, but got Unknown
		//IL_068d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0697: Expected O, but got Unknown
		//IL_06b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c3: Expected O, but got Unknown
		//IL_071c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0726: Expected O, but got Unknown
		//IL_0727: Unknown result type (might be due to invalid IL or missing references)
		//IL_0731: Expected O, but got Unknown
		//IL_0732: Unknown result type (might be due to invalid IL or missing references)
		//IL_073c: Expected O, but got Unknown
		//IL_07cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d6: Expected O, but got Unknown
		//IL_07ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f7: Expected O, but got Unknown
		//IL_0819: Unknown result type (might be due to invalid IL or missing references)
		//IL_0823: Expected O, but got Unknown
		//IL_113d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1147: Expected O, but got Unknown
		//IL_1154: Unknown result type (might be due to invalid IL or missing references)
		//IL_115e: Expected O, but got Unknown
		//IL_1523: Unknown result type (might be due to invalid IL or missing references)
		//IL_152d: Expected O, but got Unknown
		//IL_153a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1544: Expected O, but got Unknown
		//IL_175c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1766: Expected O, but got Unknown
		//IL_1773: Unknown result type (might be due to invalid IL or missing references)
		//IL_177d: Expected O, but got Unknown
		//IL_6db6: Unknown result type (might be due to invalid IL or missing references)
		//IL_6dc0: Expected O, but got Unknown
		//IL_6de4: Unknown result type (might be due to invalid IL or missing references)
		//IL_6dee: Expected O, but got Unknown
		GridEXLayout grdInput_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdByProduct_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPM_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance56 = new Appearance();
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
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance78 = new Appearance();
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
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
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
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.PartyProcessing.invfrmStockConversionPartyProcessing));
		GridEXLayout GrdHistoryMain_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryPM_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel22 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdInput = new GridEX();
		this.label14 = new System.Windows.Forms.Label();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.panel23 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdByProduct = new GridEX();
		this.label16 = new System.Windows.Forms.Label();
		this.tabPage5 = new System.Windows.Forms.TabPage();
		this.grdPM = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.txtdocnumber = new System.Windows.Forms.TextBox();
		this.txtDocdate = new System.Windows.Forms.DateTimePicker();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.panel24 = new System.Windows.Forms.Panel();
		this.txtFinishGoodsQty = new System.Windows.Forms.TextBox();
		this.txtPackingMaterialQty = new System.Windows.Forms.TextBox();
		this.panel25 = new System.Windows.Forms.Panel();
		this.label34 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.txtPackingMaterialWeight = new System.Windows.Forms.TextBox();
		this.txtFinishGoodsWeight = new System.Windows.Forms.TextBox();
		this.panel51 = new System.Windows.Forms.Panel();
		this.txtInputQuantity = new System.Windows.Forms.TextBox();
		this.panel52 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.txtOutputByProductQty = new System.Windows.Forms.TextBox();
		this.txtInputWeight = new System.Windows.Forms.TextBox();
		this.txtOutputByProductWeight = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.CmbStockPartyHeader = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtProductionNo = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label41 = new System.Windows.Forms.Label();
		this.CmbJobOrder = new UltraCombo();
		this.Add = new System.Windows.Forms.Button();
		this.label13 = new System.Windows.Forms.Label();
		this.cmbReferenceParty = new UltraCombo();
		this.txtBalanceWeight = new System.Windows.Forms.TextBox();
		this.label82 = new System.Windows.Forms.Label();
		this.cmbEntryType = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.txtStock = new System.Windows.Forms.TextBox();
		this.btnCancelDetail = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.cmbBagType = new UltraCombo();
		this.cmbUOM = new UltraCombo();
		this.label44 = new System.Windows.Forms.Label();
		this.txtdeailRemarks = new System.Windows.Forms.TextBox();
		this.label21 = new System.Windows.Forms.Label();
		this.CmbCropyr = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.label32 = new System.Windows.Forms.Label();
		this.txtUnitWeight = new System.Windows.Forms.TextBox();
		this.label23 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.cmbGodown = new UltraCombo();
		this.cmbLot = new UltraCombo();
		this.cmbItem = new UltraCombo();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label20 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.chkPrint = new System.Windows.Forms.CheckBox();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.GrdHistoryMain = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.grdHistoryPM = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label30 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grdHistoryDetail = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label27 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label24 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.label33 = new System.Windows.Forms.Label();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromDocNoHistory = new System.Windows.Forms.TextBox();
		this.label37 = new System.Windows.Forms.Label();
		this.btnshowHistory = new System.Windows.Forms.Button();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label38 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label26 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.btnLoadInput = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel22.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInput).BeginInit();
		this.tabPage4.SuspendLayout();
		this.panel23.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdByProduct).BeginInit();
		this.tabPage5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPM).BeginInit();
		this.panel6.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox3.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.panel24.SuspendLayout();
		this.panel25.SuspendLayout();
		this.panel51.SuspendLayout();
		this.panel52.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbStockPartyHeader).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrder).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbReferenceParty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEntryType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBagType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropyr).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGodown).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryMain).BeginInit();
		this.panel7.SuspendLayout();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryPM).BeginInit();
		this.panel14.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).BeginInit();
		this.panel12.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel11.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(996, 616);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(996, 616);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.BackColor = System.Drawing.Color.White;
		this.tabPage1.Controls.Add(this.tabControl2);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(988, 587);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabControl2.Appearance = System.Windows.Forms.TabAppearance.Buttons;
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Controls.Add(this.tabPage5);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.tabControl2.Location = new System.Drawing.Point(3, 277);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(982, 307);
		this.tabControl2.TabIndex = 0;
		this.tabPage3.BackColor = System.Drawing.Color.White;
		this.tabPage3.Controls.Add(this.panel22);
		this.tabPage3.Location = new System.Drawing.Point(4, 28);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(974, 275);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Input";
		this.panel22.Controls.Add(this.label2);
		this.panel22.Controls.Add(this.panel3);
		this.panel22.Controls.Add(this.ctrlGrdBar2);
		this.panel22.Controls.Add(this.label14);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel22.Location = new System.Drawing.Point(3, 3);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(968, 269);
		this.panel22.TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.Teal;
		this.label2.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(4, 5);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(81, 14);
		this.label2.TabIndex = 5;
		this.label2.Text = "Input Detail";
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdInput);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 23);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(968, 246);
		this.panel3.TabIndex = 22;
		((ScrollableBase)this.grdInput).BackColor = System.Drawing.Color.White;
		grdInput_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInput.DesignTimeLayout = grdInput_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInput).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdInput).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInput.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInput).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdInput).Name = "grdInput";
		((ScrollableBase)this.grdInput).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInput).Size = new System.Drawing.Size(968, 246);
		((System.Windows.Forms.Control)(object)this.grdInput).TabIndex = 0;
		this.grdInput.TotalRow = (InheritableBoolean)1;
		this.grdInput.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInput).VisualStyle = (VisualStyle)4;
		this.grdInput.CellUpdated += new ColumnActionEventHandler(grdInput_CellUpdated);
		this.grdInput.ColumnButtonClick += new ColumnActionEventHandler(grdInput_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdInput).DoubleClick += new System.EventHandler(grdInput_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdInput).KeyDown += new System.Windows.Forms.KeyEventHandler(grdInput_KeyDown);
		this.label14.BackColor = System.Drawing.Color.Teal;
		this.label14.Dock = System.Windows.Forms.DockStyle.Top;
		this.label14.Location = new System.Drawing.Point(0, 0);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(968, 23);
		this.label14.TabIndex = 1;
		this.tabPage4.Controls.Add(this.panel23);
		this.tabPage4.Location = new System.Drawing.Point(4, 28);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(974, 275);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "Output";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.panel23.Controls.Add(this.label4);
		this.panel23.Controls.Add(this.panel8);
		this.panel23.Controls.Add(this.ctrlGrdBar1);
		this.panel23.Controls.Add(this.label16);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel23.Location = new System.Drawing.Point(3, 3);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(968, 269);
		this.panel23.TabIndex = 0;
		this.label4.AutoSize = true;
		this.label4.BackColor = System.Drawing.Color.Teal;
		this.label4.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(4, 4);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(91, 14);
		this.label4.TabIndex = 22;
		this.label4.Text = "Output Detail";
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdByProduct);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 19);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(968, 250);
		this.panel8.TabIndex = 21;
		((ScrollableBase)this.grdByProduct).BackColor = System.Drawing.Color.White;
		grdByProduct_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdByProduct.DesignTimeLayout = grdByProduct_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdByProduct).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdByProduct.FilterMode = (FilterMode)2;
		this.grdByProduct.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdByProduct).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdByProduct.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdByProduct).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdByProduct).Name = "grdByProduct";
		((ScrollableBase)this.grdByProduct).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdByProduct).Size = new System.Drawing.Size(968, 250);
		((System.Windows.Forms.Control)(object)this.grdByProduct).TabIndex = 0;
		this.grdByProduct.TotalRow = (InheritableBoolean)1;
		this.grdByProduct.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdByProduct).VisualStyle = (VisualStyle)4;
		this.grdByProduct.CellUpdated += new ColumnActionEventHandler(grdByProduct_CellUpdated);
		this.grdByProduct.ColumnButtonClick += new ColumnActionEventHandler(grdByProduct_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdByProduct).DoubleClick += new System.EventHandler(grdByProduct_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdByProduct).KeyDown += new System.Windows.Forms.KeyEventHandler(grdByProduct_KeyDown);
		this.label16.BackColor = System.Drawing.Color.Teal;
		this.label16.Dock = System.Windows.Forms.DockStyle.Top;
		this.label16.Location = new System.Drawing.Point(0, 0);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(968, 19);
		this.label16.TabIndex = 2;
		this.tabPage5.Controls.Add((System.Windows.Forms.Control)(object)this.grdPM);
		this.tabPage5.Controls.Add(this.panel6);
		this.tabPage5.Location = new System.Drawing.Point(4, 28);
		this.tabPage5.Name = "tabPage5";
		this.tabPage5.Size = new System.Drawing.Size(974, 275);
		this.tabPage5.TabIndex = 2;
		this.tabPage5.Text = "Packing Material";
		this.tabPage5.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdPM).BackColor = System.Drawing.Color.White;
		grdPM_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPM.DesignTimeLayout = grdPM_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPM).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPM.FilterMode = (FilterMode)2;
		this.grdPM.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPM).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPM.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPM).Location = new System.Drawing.Point(0, 24);
		((System.Windows.Forms.Control)(object)this.grdPM).Name = "grdPM";
		((ScrollableBase)this.grdPM).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPM).Size = new System.Drawing.Size(974, 251);
		((System.Windows.Forms.Control)(object)this.grdPM).TabIndex = 14;
		this.grdPM.TotalRow = (InheritableBoolean)1;
		this.grdPM.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPM).VisualStyle = (VisualStyle)4;
		this.grdPM.CellUpdated += new ColumnActionEventHandler(grdPackingMaterial_CellUpdated);
		this.grdPM.ColumnButtonClick += new ColumnActionEventHandler(grdPackingMaterial_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPM).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPM_KeyDown);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label9);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(974, 24);
		this.panel6.TabIndex = 0;
		this.label9.AutoSize = true;
		this.label9.BackColor = System.Drawing.Color.Teal;
		this.label9.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(7, 5);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(65, 14);
		this.label9.TabIndex = 6;
		this.label9.Text = "PM Detail";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox3);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 55);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(982, 222);
		this.panel4.TabIndex = 0;
		this.groupBox3.Controls.Add(this.txtdocnumber);
		this.groupBox3.Controls.Add(this.txtDocdate);
		this.groupBox3.Controls.Add(this.txtRemarks);
		this.groupBox3.Controls.Add(this.groupBox2);
		this.groupBox3.Controls.Add(this.label11);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbStockPartyHeader);
		this.groupBox3.Controls.Add(this.label6);
		this.groupBox3.Controls.Add(this.label22);
		this.groupBox3.Controls.Add(this.label1);
		this.groupBox3.Controls.Add(this.txtProductionNo);
		this.groupBox3.Controls.Add(this.label3);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(2, 3);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(973, 103);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Main";
		this.txtdocnumber.BackColor = System.Drawing.Color.White;
		this.txtdocnumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocnumber.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtdocnumber.Location = new System.Drawing.Point(59, 19);
		this.txtdocnumber.Name = "txtdocnumber";
		this.txtdocnumber.ReadOnly = true;
		this.txtdocnumber.Size = new System.Drawing.Size(122, 23);
		this.txtdocnumber.TabIndex = 0;
		this.txtDocdate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDocdate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtDocdate.Location = new System.Drawing.Point(59, 47);
		this.txtDocdate.Name = "txtDocdate";
		this.txtDocdate.Size = new System.Drawing.Size(122, 23);
		this.txtDocdate.TabIndex = 1;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRemarks.Location = new System.Drawing.Point(59, 75);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(398, 23);
		this.txtRemarks.TabIndex = 2;
		this.groupBox2.Controls.Add(this.panel24);
		this.groupBox2.Controls.Add(this.panel51);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.groupBox2.Location = new System.Drawing.Point(460, 9);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(507, 90);
		this.groupBox2.TabIndex = 5;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Summery Calculations";
		this.panel24.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel24.Controls.Add(this.txtFinishGoodsQty);
		this.panel24.Controls.Add(this.txtPackingMaterialQty);
		this.panel24.Controls.Add(this.panel25);
		this.panel24.Controls.Add(this.label39);
		this.panel24.Controls.Add(this.label31);
		this.panel24.Controls.Add(this.txtPackingMaterialWeight);
		this.panel24.Controls.Add(this.txtFinishGoodsWeight);
		this.panel24.Location = new System.Drawing.Point(266, 15);
		this.panel24.Name = "panel24";
		this.panel24.Size = new System.Drawing.Size(236, 75);
		this.panel24.TabIndex = 12;
		this.txtFinishGoodsQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFinishGoodsQty.Enabled = false;
		this.txtFinishGoodsQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFinishGoodsQty.Location = new System.Drawing.Point(65, 25);
		this.txtFinishGoodsQty.Name = "txtFinishGoodsQty";
		this.txtFinishGoodsQty.Size = new System.Drawing.Size(70, 23);
		this.txtFinishGoodsQty.TabIndex = 8;
		this.txtFinishGoodsQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPackingMaterialQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackingMaterialQty.Enabled = false;
		this.txtPackingMaterialQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackingMaterialQty.Location = new System.Drawing.Point(65, 49);
		this.txtPackingMaterialQty.Name = "txtPackingMaterialQty";
		this.txtPackingMaterialQty.Size = new System.Drawing.Size(70, 23);
		this.txtPackingMaterialQty.TabIndex = 16;
		this.txtPackingMaterialQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.panel25.BackColor = System.Drawing.Color.Teal;
		this.panel25.Controls.Add(this.label34);
		this.panel25.Controls.Add(this.label35);
		this.panel25.Controls.Add(this.label36);
		this.panel25.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel25.Location = new System.Drawing.Point(0, 0);
		this.panel25.Name = "panel25";
		this.panel25.Size = new System.Drawing.Size(234, 24);
		this.panel25.TabIndex = 0;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.ForeColor = System.Drawing.Color.White;
		this.label34.Location = new System.Drawing.Point(2, 4);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(79, 17);
		this.label34.TabIndex = 5;
		this.label34.Text = "Description";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.ForeColor = System.Drawing.Color.White;
		this.label35.Location = new System.Drawing.Point(85, 4);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(30, 17);
		this.label35.TabIndex = 2;
		this.label35.Text = "Qty";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.ForeColor = System.Drawing.Color.White;
		this.label36.Location = new System.Drawing.Point(147, 4);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(79, 17);
		this.label36.TabIndex = 4;
		this.label36.Text = "Net Weight";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label39.Location = new System.Drawing.Point(1, 51);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(30, 19);
		this.label39.TabIndex = 1;
		this.label39.Text = "PM";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label31.Location = new System.Drawing.Point(1, 27);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(62, 19);
		this.label31.TabIndex = 1;
		this.label31.Text = "F. Goods";
		this.txtPackingMaterialWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackingMaterialWeight.Enabled = false;
		this.txtPackingMaterialWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackingMaterialWeight.Location = new System.Drawing.Point(136, 49);
		this.txtPackingMaterialWeight.Name = "txtPackingMaterialWeight";
		this.txtPackingMaterialWeight.Size = new System.Drawing.Size(96, 23);
		this.txtPackingMaterialWeight.TabIndex = 17;
		this.txtPackingMaterialWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPackingMaterialWeight.Visible = false;
		this.txtFinishGoodsWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFinishGoodsWeight.Enabled = false;
		this.txtFinishGoodsWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFinishGoodsWeight.Location = new System.Drawing.Point(136, 25);
		this.txtFinishGoodsWeight.Name = "txtFinishGoodsWeight";
		this.txtFinishGoodsWeight.Size = new System.Drawing.Size(96, 23);
		this.txtFinishGoodsWeight.TabIndex = 9;
		this.txtFinishGoodsWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.panel51.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel51.Controls.Add(this.txtInputQuantity);
		this.panel51.Controls.Add(this.panel52);
		this.panel51.Controls.Add(this.label25);
		this.panel51.Controls.Add(this.label29);
		this.panel51.Controls.Add(this.txtOutputByProductQty);
		this.panel51.Controls.Add(this.txtInputWeight);
		this.panel51.Controls.Add(this.txtOutputByProductWeight);
		this.panel51.Location = new System.Drawing.Point(1, 15);
		this.panel51.Name = "panel51";
		this.panel51.Size = new System.Drawing.Size(266, 75);
		this.panel51.TabIndex = 10;
		this.txtInputQuantity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInputQuantity.Enabled = false;
		this.txtInputQuantity.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInputQuantity.Location = new System.Drawing.Point(92, 25);
		this.txtInputQuantity.Name = "txtInputQuantity";
		this.txtInputQuantity.Size = new System.Drawing.Size(70, 23);
		this.txtInputQuantity.TabIndex = 0;
		this.txtInputQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.panel52.BackColor = System.Drawing.Color.Teal;
		this.panel52.Controls.Add(this.label15);
		this.panel52.Controls.Add(this.label17);
		this.panel52.Controls.Add(this.label19);
		this.panel52.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel52.Location = new System.Drawing.Point(0, 0);
		this.panel52.Name = "panel52";
		this.panel52.Size = new System.Drawing.Size(264, 24);
		this.panel52.TabIndex = 0;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(2, 4);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(79, 17);
		this.label15.TabIndex = 5;
		this.label15.Text = "Description";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(111, 4);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(30, 17);
		this.label17.TabIndex = 2;
		this.label17.Text = "Qty";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.White;
		this.label19.Location = new System.Drawing.Point(175, 4);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(79, 17);
		this.label19.TabIndex = 4;
		this.label19.Text = "Net Weight";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(2, 27);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(92, 19);
		this.label25.TabIndex = 1;
		this.label25.Text = "Raw Material";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label29.Location = new System.Drawing.Point(2, 51);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(77, 19);
		this.label29.TabIndex = 1;
		this.label29.Text = "By Product";
		this.txtOutputByProductQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOutputByProductQty.Enabled = false;
		this.txtOutputByProductQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOutputByProductQty.Location = new System.Drawing.Point(92, 49);
		this.txtOutputByProductQty.Name = "txtOutputByProductQty";
		this.txtOutputByProductQty.Size = new System.Drawing.Size(70, 23);
		this.txtOutputByProductQty.TabIndex = 4;
		this.txtOutputByProductQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtInputWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInputWeight.Enabled = false;
		this.txtInputWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInputWeight.Location = new System.Drawing.Point(164, 25);
		this.txtInputWeight.Name = "txtInputWeight";
		this.txtInputWeight.Size = new System.Drawing.Size(96, 23);
		this.txtInputWeight.TabIndex = 1;
		this.txtInputWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOutputByProductWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOutputByProductWeight.Enabled = false;
		this.txtOutputByProductWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOutputByProductWeight.Location = new System.Drawing.Point(164, 49);
		this.txtOutputByProductWeight.Name = "txtOutputByProductWeight";
		this.txtOutputByProductWeight.Size = new System.Drawing.Size(96, 23);
		this.txtOutputByProductWeight.TabIndex = 5;
		this.txtOutputByProductWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(184, 51);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(67, 15);
		this.label11.TabIndex = 177;
		this.label11.Text = "Stock Party";
		this.CmbStockPartyHeader.AllowNull = (DefaultableBoolean)1;
		this.CmbStockPartyHeader.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbStockPartyHeader.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbStockPartyHeader.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbStockPartyHeader).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbStockPartyHeader).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbStockPartyHeader).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbStockPartyHeader).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStockPartyHeader).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance44;
		((SpecialBoxBase)((UltraGridBase)this.CmbStockPartyHeader).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStockPartyHeader).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbStockPartyHeader).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbStockPartyHeader).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbStockPartyHeader).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbStockPartyHeader).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbStockPartyHeader).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbStockPartyHeader).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStockPartyHeader).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbStockPartyHeader).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbStockPartyHeader).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbStockPartyHeader).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStockPartyHeader).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbStockPartyHeader).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbStockPartyHeader).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbStockPartyHeader).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbStockPartyHeader).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbStockPartyHeader).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbStockPartyHeader).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbStockPartyHeader).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbStockPartyHeader).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbStockPartyHeader).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbStockPartyHeader).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbStockPartyHeader.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbStockPartyHeader).Location = new System.Drawing.Point(270, 45);
		((System.Windows.Forms.Control)(object)this.CmbStockPartyHeader).Name = "CmbStockPartyHeader";
		((System.Windows.Forms.Control)(object)this.CmbStockPartyHeader).Size = new System.Drawing.Size(189, 26);
		((System.Windows.Forms.Control)(object)this.CmbStockPartyHeader).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbStockPartyHeader).TextChanged += new System.EventHandler(CmbStockPartyHeader_TextChanged);
		((System.Windows.Forms.Control)(object)this.CmbStockPartyHeader).Leave += new System.EventHandler(CmbStockPartyHeader_Leave);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(3, 79);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(52, 15);
		this.label6.TabIndex = 149;
		this.label6.Text = "Remarks";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(3, 51);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(57, 15);
		this.label22.TabIndex = 140;
		this.label22.Text = "Doc Date";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(3, 23);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(48, 15);
		this.label1.TabIndex = 140;
		this.label1.Text = "Doc No";
		this.txtProductionNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtProductionNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtProductionNo.Location = new System.Drawing.Point(270, 19);
		this.txtProductionNo.Name = "txtProductionNo";
		this.txtProductionNo.Size = new System.Drawing.Size(189, 23);
		this.txtProductionNo.TabIndex = 3;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(184, 23);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(85, 15);
		this.label3.TabIndex = 147;
		this.label3.Text = "Production No";
		this.groupBox1.Controls.Add(this.label41);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobOrder);
		this.groupBox1.Controls.Add(this.Add);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbReferenceParty);
		this.groupBox1.Controls.Add(this.txtBalanceWeight);
		this.groupBox1.Controls.Add(this.label82);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbEntryType);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.txtStock);
		this.groupBox1.Controls.Add(this.btnCancelDetail);
		this.groupBox1.Controls.Add(this.btnUpdateDetail);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBagType);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbUOM);
		this.groupBox1.Controls.Add(this.label44);
		this.groupBox1.Controls.Add(this.txtdeailRemarks);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropyr);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label18);
		this.groupBox1.Controls.Add(this.txtQty);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add(this.txtUnitWeight);
		this.groupBox1.Controls.Add(this.label23);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbGodown);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLot);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(2, 106);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(973, 112);
		this.groupBox1.TabIndex = 1;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Detail";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(4, 17);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(59, 15);
		this.label41.TabIndex = 215;
		this.label41.Text = "Job Order";
		this.CmbJobOrder.AllowNull = (DefaultableBoolean)1;
		this.CmbJobOrder.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobOrder.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobOrder.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrder).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrder).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance34).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance35).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrder).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbJobOrder.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobOrder).Location = new System.Drawing.Point(4, 36);
		((System.Windows.Forms.Control)(object)this.CmbJobOrder).Name = "CmbJobOrder";
		((System.Windows.Forms.Control)(object)this.CmbJobOrder).Size = new System.Drawing.Size(142, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobOrder).TabIndex = 0;
		this.Add.BackColor = System.Drawing.Color.Teal;
		this.Add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.Add.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Add.ForeColor = System.Drawing.Color.White;
		this.Add.Location = new System.Drawing.Point(831, 82);
		this.Add.Margin = new System.Windows.Forms.Padding(2);
		this.Add.Name = "Add";
		this.Add.Size = new System.Drawing.Size(36, 26);
		this.Add.TabIndex = 14;
		this.Add.Text = "+";
		this.Add.UseVisualStyleBackColor = false;
		this.Add.Click += new System.EventHandler(AddInGrid_Click);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(147, 17);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(89, 15);
		this.label13.TabIndex = 213;
		this.label13.Text = "Reference Party";
		this.cmbReferenceParty.AllowNull = (DefaultableBoolean)1;
		this.cmbReferenceParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbReferenceParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbReferenceParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbReferenceParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.cmbReferenceParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbReferenceParty).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbReferenceParty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbReferenceParty).Location = new System.Drawing.Point(147, 36);
		((System.Windows.Forms.Control)(object)this.cmbReferenceParty).Name = "cmbReferenceParty";
		((System.Windows.Forms.Control)(object)this.cmbReferenceParty).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.cmbReferenceParty).TabIndex = 1;
		this.txtBalanceWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBalanceWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceWeight.Location = new System.Drawing.Point(409, 84);
		this.txtBalanceWeight.Name = "txtBalanceWeight";
		this.txtBalanceWeight.ReadOnly = true;
		this.txtBalanceWeight.Size = new System.Drawing.Size(93, 23);
		this.txtBalanceWeight.TabIndex = 10;
		this.txtBalanceWeight.TabStop = false;
		this.txtBalanceWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label82.AutoSize = true;
		this.label82.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label82.Location = new System.Drawing.Point(409, 65);
		this.label82.Name = "label82";
		this.label82.Size = new System.Drawing.Size(79, 15);
		this.label82.TabIndex = 201;
		this.label82.Text = "Stock Weight";
		this.cmbEntryType.AllowNull = (DefaultableBoolean)1;
		this.cmbEntryType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEntryType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEntryType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEntryType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.cmbEntryType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEntryType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbEntryType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbEntryType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEntryType).Location = new System.Drawing.Point(328, 36);
		((System.Windows.Forms.Control)(object)this.cmbEntryType).Name = "cmbEntryType";
		((System.Windows.Forms.Control)(object)this.cmbEntryType).Size = new System.Drawing.Size(122, 26);
		((System.Windows.Forms.Control)(object)this.cmbEntryType).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbEntryType).Leave += new System.EventHandler(cmbEntryType_Leave);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(867, 17);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(57, 15);
		this.label8.TabIndex = 153;
		this.label8.Text = "Crop Year";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(329, 65);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(59, 15);
		this.label28.TabIndex = 163;
		this.label28.Text = "Stock Qty";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(4, 63);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(46, 15);
		this.label10.TabIndex = 155;
		this.label10.Text = "Job Lot";
		this.txtStock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtStock.Enabled = false;
		this.txtStock.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtStock.Location = new System.Drawing.Point(329, 84);
		this.txtStock.Name = "txtStock";
		this.txtStock.Size = new System.Drawing.Size(79, 23);
		this.txtStock.TabIndex = 9;
		this.txtStock.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.btnCancelDetail.BackColor = System.Drawing.Color.Teal;
		this.btnCancelDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnCancelDetail.ForeColor = System.Drawing.Color.White;
		this.btnCancelDetail.Location = new System.Drawing.Point(900, 82);
		this.btnCancelDetail.Name = "btnCancelDetail";
		this.btnCancelDetail.Size = new System.Drawing.Size(63, 26);
		this.btnCancelDetail.TabIndex = 16;
		this.btnCancelDetail.Text = "Cancel";
		this.btnCancelDetail.UseVisualStyleBackColor = false;
		this.btnCancelDetail.Click += new System.EventHandler(btnCancelDetail_Click);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(831, 82);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(72, 26);
		this.btnUpdateDetail.TabIndex = 15;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.cmbBagType.AllowNull = (DefaultableBoolean)1;
		this.cmbBagType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBagType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBagType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbBagType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBagType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBagType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBagType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance68;
		((SpecialBoxBase)((UltraGridBase)this.cmbBagType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBagType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbBagType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBagType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbBagType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBagType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBagType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBagType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbBagType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBagType).Location = new System.Drawing.Point(147, 82);
		((System.Windows.Forms.Control)(object)this.cmbBagType).Name = "cmbBagType";
		((System.Windows.Forms.Control)(object)this.cmbBagType).Size = new System.Drawing.Size(101, 26);
		((System.Windows.Forms.Control)(object)this.cmbBagType).TabIndex = 7;
		this.cmbUOM.AllowNull = (DefaultableBoolean)1;
		this.cmbUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance89).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Location = new System.Drawing.Point(249, 82);
		((System.Windows.Forms.Control)(object)this.cmbUOM).Name = "cmbUOM";
		((System.Windows.Forms.Control)(object)this.cmbUOM).Size = new System.Drawing.Size(78, 26);
		((System.Windows.Forms.Control)(object)this.cmbUOM).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Leave += new System.EventHandler(cmbUOM_Leave);
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(250, 65);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(61, 15);
		this.label44.TabIndex = 142;
		this.label44.Text = "Pack Uom";
		this.txtdeailRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdeailRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdeailRemarks.Location = new System.Drawing.Point(680, 84);
		this.txtdeailRemarks.Name = "txtdeailRemarks";
		this.txtdeailRemarks.Size = new System.Drawing.Size(149, 23);
		this.txtdeailRemarks.TabIndex = 13;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(679, 66);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(52, 15);
		this.label21.TabIndex = 172;
		this.label21.Text = "Remarks";
		this.CmbCropyr.AllowNull = (DefaultableBoolean)1;
		this.CmbCropyr.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropyr.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropyr.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropyr).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropyr).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance100).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance101).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance101).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance101).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCropyr.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Location = new System.Drawing.Point(868, 36);
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Name = "CmbCropyr";
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Size = new System.Drawing.Size(95, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropyr).TabIndex = 5;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(147, 65);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(60, 15);
		this.label12.TabIndex = 157;
		this.label12.Text = "Pack Type";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(450, 17);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(69, 15);
		this.label5.TabIndex = 151;
		this.label5.Text = "WareHouse";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(503, 65);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(54, 15);
		this.label18.TabIndex = 159;
		this.label18.Text = "Item Qty";
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQty.Location = new System.Drawing.Point(504, 84);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(82, 23);
		this.txtQty.TabIndex = 11;
		this.txtQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQty.TextChanged += new System.EventHandler(txtQty_TextChanged);
		this.txtQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQty_KeyPress);
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(588, 65);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(68, 15);
		this.label32.TabIndex = 169;
		this.label32.Text = "Net Weight";
		this.txtUnitWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtUnitWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtUnitWeight.Location = new System.Drawing.Point(588, 84);
		this.txtUnitWeight.Name = "txtUnitWeight";
		this.txtUnitWeight.Size = new System.Drawing.Size(91, 23);
		this.txtUnitWeight.TabIndex = 12;
		this.txtUnitWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtUnitWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtUnitWeight_KeyPress);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(328, 17);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(62, 15);
		this.label23.TabIndex = 167;
		this.label23.Text = "Entry Type";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(603, 17);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(67, 15);
		this.label7.TabIndex = 141;
		this.label7.Text = "Item Name";
		this.cmbGodown.AllowNull = (DefaultableBoolean)1;
		this.cmbGodown.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbGodown.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbGodown.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbGodown).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbGodown).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbGodown).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGodown).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.cmbGodown).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGodown).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbGodown).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbGodown).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance114).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance115).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbGodown).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbGodown).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbGodown).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbGodown).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbGodown.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbGodown).Location = new System.Drawing.Point(451, 36);
		((System.Windows.Forms.Control)(object)this.cmbGodown).Name = "cmbGodown";
		((System.Windows.Forms.Control)(object)this.cmbGodown).Size = new System.Drawing.Size(151, 26);
		((System.Windows.Forms.Control)(object)this.cmbGodown).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.cmbGodown).TextChanged += new System.EventHandler(cmbGodown_TextChanged);
		this.cmbLot.AllowNull = (DefaultableBoolean)1;
		this.cmbLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance120;
		((SpecialBoxBase)((UltraGridBase)this.cmbLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLot).Location = new System.Drawing.Point(4, 82);
		((System.Windows.Forms.Control)(object)this.cmbLot).Name = "cmbLot";
		((System.Windows.Forms.Control)(object)this.cmbLot).Size = new System.Drawing.Size(142, 26);
		((System.Windows.Forms.Control)(object)this.cmbLot).TabIndex = 6;
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
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(603, 36);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(264, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.cmbItem).Leave += new System.EventHandler(cmbItem_Leave);
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label20);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Controls.Add(this.chkPrint);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(982, 52);
		this.panel2.TabIndex = 0;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.ForeColor = System.Drawing.Color.White;
		this.label20.Location = new System.Drawing.Point(4, 30);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(242, 19);
		this.label20.TabIndex = 2;
		this.label20.Text = "Stock Conversion Production";
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.btnnew, this.btnRefresh, this.btnsave, this.btnUpdate, this.btnDelete, this.Print, this.btnattachment, this.btnLoadInput, this.btnshortcutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(982, 27);
		this.toolStrip1.TabIndex = 1;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Image = (System.Drawing.Image)resources.GetObject("btnRefresh.Image");
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnUpdate.Image = (System.Drawing.Image)resources.GetObject("btnUpdate.Image");
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.Print.Image = (System.Drawing.Image)resources.GetObject("Print.Image");
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(92, 24);
		this.Print.Text = "600-&Print";
		this.Print.Click += new System.EventHandler(Print_Click);
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
		this.chkPrint.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkPrint.AutoSize = true;
		this.chkPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPrint.ForeColor = System.Drawing.Color.White;
		this.chkPrint.Location = new System.Drawing.Point(896, 30);
		this.chkPrint.Name = "chkPrint";
		this.chkPrint.Size = new System.Drawing.Size(79, 20);
		this.chkPrint.TabIndex = 5;
		this.chkPrint.Text = "Priview";
		this.chkPrint.UseVisualStyleBackColor = true;
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistoryMain);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Controls.Add(this.panel10);
		this.tabPage2.Controls.Add(this.panel11);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(988, 587);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.GrdHistoryMain.AllowEdit = (InheritableBoolean)2;
		GrdHistoryMain_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistoryMain.DesignTimeLayout = GrdHistoryMain_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistoryMain).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdHistoryMain.DynamicFiltering = true;
		this.GrdHistoryMain.FilterMode = (FilterMode)2;
		this.GrdHistoryMain.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdHistoryMain).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistoryMain.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GrdHistoryMain).Location = new System.Drawing.Point(3, 118);
		((System.Windows.Forms.Control)(object)this.GrdHistoryMain).Name = "GrdHistoryMain";
		((ScrollableBase)this.GrdHistoryMain).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistoryMain).Size = new System.Drawing.Size(982, 279);
		((System.Windows.Forms.Control)(object)this.GrdHistoryMain).TabIndex = 1;
		this.GrdHistoryMain.TotalRow = (InheritableBoolean)1;
		this.GrdHistoryMain.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistoryMain).VisualStyle = (VisualStyle)4;
		this.GrdHistoryMain.ColumnButtonClick += new ColumnActionEventHandler(DatagridHistory_ColumnButtonClick);
		this.GrdHistoryMain.SelectionChanged += new System.EventHandler(DatagridHistory_SelectionChanged);
		this.GrdHistoryMain.LinkClicked += new ColumnActionEventHandler(DatagridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdHistoryMain).DoubleClick += new System.EventHandler(DatagridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.GrdHistoryMain).KeyDown += new System.Windows.Forms.KeyEventHandler(GrdHistoryMain_KeyDown);
		this.panel7.Controls.Add(this.panel13);
		this.panel7.Controls.Add(this.panel9);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel7.Location = new System.Drawing.Point(3, 397);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(982, 187);
		this.panel7.TabIndex = 47;
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryPM);
		this.panel13.Controls.Add(this.panel14);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(592, 0);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(390, 187);
		this.panel13.TabIndex = 2;
		this.grdHistoryPM.AllowDelete = (InheritableBoolean)1;
		this.grdHistoryPM.AllowEdit = (InheritableBoolean)2;
		grdHistoryPM_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryPM.DesignTimeLayout = grdHistoryPM_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryPM).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryPM.DynamicFiltering = true;
		this.grdHistoryPM.FilterMode = (FilterMode)2;
		this.grdHistoryPM.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryPM).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryPM.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryPM).Location = new System.Drawing.Point(0, 21);
		((System.Windows.Forms.Control)(object)this.grdHistoryPM).Name = "grdHistoryPM";
		((ScrollableBase)this.grdHistoryPM).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryPM).Size = new System.Drawing.Size(390, 166);
		((System.Windows.Forms.Control)(object)this.grdHistoryPM).TabIndex = 0;
		this.grdHistoryPM.TotalRow = (InheritableBoolean)1;
		this.grdHistoryPM.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryPM).VisualStyle = (VisualStyle)4;
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.label30);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(390, 21);
		this.panel14.TabIndex = 2;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label30.ForeColor = System.Drawing.Color.White;
		this.label30.Location = new System.Drawing.Point(4, 2);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(234, 17);
		this.label30.TabIndex = 71;
		this.label30.Text = "PM Detail of Above Selected Row";
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryDetail);
		this.panel9.Controls.Add(this.panel12);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(592, 187);
		this.panel9.TabIndex = 1;
		this.grdHistoryDetail.AllowDelete = (InheritableBoolean)1;
		this.grdHistoryDetail.AllowEdit = (InheritableBoolean)2;
		grdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryDetail.DesignTimeLayout = grdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryDetail.DynamicFiltering = true;
		this.grdHistoryDetail.FilterMode = (FilterMode)2;
		this.grdHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Location = new System.Drawing.Point(0, 21);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Name = "grdHistoryDetail";
		((ScrollableBase)this.grdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Size = new System.Drawing.Size(592, 166);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).TabIndex = 0;
		this.grdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.grdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label27);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(592, 21);
		this.panel12.TabIndex = 1;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label27.ForeColor = System.Drawing.Color.White;
		this.label27.Location = new System.Drawing.Point(4, 2);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(313, 17);
		this.label27.TabIndex = 71;
		this.label27.Text = "Input and Out Detail of Above Selected Row";
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.ctrlGrdBar3);
		this.panel5.Controls.Add(this.label24);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 96);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(982, 22);
		this.panel5.TabIndex = 46;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label24.ForeColor = System.Drawing.Color.White;
		this.label24.Location = new System.Drawing.Point(3, 2);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(132, 18);
		this.label24.TabIndex = 71;
		this.label24.Text = "Filtered Records";
		this.panel10.Controls.Add(this.ToDateHistory);
		this.panel10.Controls.Add(this.rdmodifydate);
		this.panel10.Controls.Add(this.drdocdate);
		this.panel10.Controls.Add(this.rdentrydate);
		this.panel10.Controls.Add(this.label33);
		this.panel10.Controls.Add(this.txtToDocNoHistory);
		this.panel10.Controls.Add(this.txtFromDocNoHistory);
		this.panel10.Controls.Add(this.label37);
		this.panel10.Controls.Add(this.btnshowHistory);
		this.panel10.Controls.Add(this.FromDateHistory);
		this.panel10.Controls.Add(this.label38);
		this.panel10.Controls.Add(this.label40);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 54);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(982, 42);
		this.panel10.TabIndex = 0;
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(255, 10);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(135, 23);
		this.ToDateHistory.TabIndex = 1;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(843, 12);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 7;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(685, 12);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 5;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(766, 12);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 6;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(506, 14);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(39, 15);
		this.label33.TabIndex = 615;
		this.label33.Text = "To No";
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(545, 10);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.ShortcutsEnabled = false;
		this.txtToDocNoHistory.Size = new System.Drawing.Size(59, 23);
		this.txtToDocNoHistory.TabIndex = 3;
		this.txtToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoHistory_KeyPress);
		this.txtFromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoHistory.Location = new System.Drawing.Point(444, 10);
		this.txtFromDocNoHistory.Name = "txtFromDocNoHistory";
		this.txtFromDocNoHistory.ShortcutsEnabled = false;
		this.txtFromDocNoHistory.Size = new System.Drawing.Size(59, 23);
		this.txtFromDocNoHistory.TabIndex = 2;
		this.txtFromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNoHistory_KeyPress);
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(393, 14);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(54, 15);
		this.label37.TabIndex = 614;
		this.label37.Text = "From No";
		this.btnshowHistory.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshowHistory.FlatAppearance.BorderSize = 0;
		this.btnshowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshowHistory.ForeColor = System.Drawing.Color.White;
		this.btnshowHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshowHistory.Location = new System.Drawing.Point(609, 8);
		this.btnshowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnshowHistory.Name = "btnshowHistory";
		this.btnshowHistory.Size = new System.Drawing.Size(68, 26);
		this.btnshowHistory.TabIndex = 4;
		this.btnshowHistory.Text = "Show";
		this.btnshowHistory.UseVisualStyleBackColor = false;
		this.btnshowHistory.Click += new System.EventHandler(btnshowHistory_Click);
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(72, 10);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(135, 23);
		this.FromDateHistory.TabIndex = 0;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(210, 14);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(48, 15);
		this.label38.TabIndex = 612;
		this.label38.Text = "Date To";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(6, 14);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(63, 15);
		this.label40.TabIndex = 613;
		this.label40.Text = "Date From";
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label26);
		this.panel11.Controls.Add(this.toolStrip2);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(3, 3);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(982, 51);
		this.panel11.TabIndex = 0;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label26.ForeColor = System.Drawing.Color.White;
		this.label26.Location = new System.Drawing.Point(4, 30);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(197, 18);
		this.label26.TabIndex = 71;
		this.label26.Text = "Stock Conversion History";
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.toolStripSeparator1, this.btnNewHistory, this.BtnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(982, 27);
		this.toolStrip2.TabIndex = 2;
		this.toolStrip2.Text = "toolStrip2";
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.BtnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshHistory.Name = "BtnRefreshHistory";
		this.BtnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshHistory.Text = "Refresh";
		this.BtnRefreshHistory.Click += new System.EventHandler(BtnRefreshHistory_Click);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(928, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdInput;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 20);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(922, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdByProduct;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 19);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(919, -1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar3.MyGrid = this.GrdHistoryMain;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(60, 22);
		this.ctrlGrdBar3.TabIndex = 72;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.btnLoadInput.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnLoadInput.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadInput.Name = "btnLoadInput";
		this.btnLoadInput.Size = new System.Drawing.Size(102, 24);
		this.btnLoadInput.Text = "Load Input";
		this.btnLoadInput.Click += new System.EventHandler(btnLoadInput_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(996, 616);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "invfrmStockConversionPartyProcessing";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(invfrmStockConversionPartyProcessing_FormClosed);
		base.Load += new System.EventHandler(invfrmStockConversionProduction_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(invfrmStockConversionProduction_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		this.panel22.ResumeLayout(false);
		this.panel22.PerformLayout();
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInput).EndInit();
		this.tabPage4.ResumeLayout(false);
		this.panel23.ResumeLayout(false);
		this.panel23.PerformLayout();
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdByProduct).EndInit();
		this.tabPage5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPM).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.panel24.ResumeLayout(false);
		this.panel24.PerformLayout();
		this.panel25.ResumeLayout(false);
		this.panel25.PerformLayout();
		this.panel51.ResumeLayout(false);
		this.panel51.PerformLayout();
		this.panel52.ResumeLayout(false);
		this.panel52.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbStockPartyHeader).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrder).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbReferenceParty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEntryType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBagType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropyr).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGodown).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryMain).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryPM).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
