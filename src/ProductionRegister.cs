// using Architecture.Reports;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Production;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;
using GroupTotals = Janus.Windows.GridEX.GroupTotals;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Production_Reports
{

public class ProductionRegister : Form
{
	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dt1 = new DataTable();

	private DataTable dtEntryType = new DataTable();

	private DataTable dtBranch = new DataTable();

	public DateTime FromDate;

	public DateTime ToDate;

	public int StockAccountId;

	public int ItemId;

	public string EntryType = "";

	public bool RequestFromAnotherForm;

	private IContainer components;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnPrintCurrent;

	private ToolStripButton btnShortCutKeys;

	private Panel panel1;

	private Label label6;

	private Label label4;

	private Label label3;

	private Label label2;

	private Label Warehouse;

	private Label label27;

	private Panel panel2;

	private Label label23;

	private GroupBox groupBox1;

	private Panel panel3;

	private Label label1;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grd;

	private Label label9;

	private GroupBox SummaryInfoBox;

	private Label label7;

	private Label TotalOutPut;

	private Label TotalInput;

	private Label label15;

	private Label label14;

	private Label TotalFinishGoods;

	private Label TotalByProduct;

	private Label TotalIssuance;

	private Label label10;

	private Label label8;

	private Label TotalDifference;

	private Label label12;

	private Label label11;

	private RadioButton rdEntryDate;

	private RadioButton rdDocDate;

	private Label label13;

	private Label label16;

	private RadioButton rdThisDay;

	private RadioButton rdThisYear;

	private RadioButton rdThisMonth;

	private RadioButton rdthisWeek;

	private RadioButton rdFinancialYear;

	public UltraCombo cmbplant;

	public UltraCombo cmbJoborder;

	public UltraCombo cmbparentcategory;

	public UltraCombo cmbitem;

	public UltraCombo cmbWarehouse;

	public DateTimePicker datFromDate;

	public DateTimePicker datToDate;

	public Button btnshow;

	public UltraCombo cmbActivity;

	public UltraCombo CmbEntryType;

	public UltraCombo cmbBranchName;

	public UltraCombo CmbWipAccount;

	internal UltraCombo CmbStockAccount;

	private Label label38;

	public ProductionRegister(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void ProductionRegister_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1305, 600);
			BranchesFill();
			Activityfill();
			JobOrderBind();
			AllCombobind();
			EntryTypeFill();
			datFromDate.Focus();
			PrintButtonManage();
			if (!RequestFromAnotherForm)
			{
				datFromDate.Value = DateTime.Now.AddDays(-15.0);
				return;
			}
			datFromDate.Value = FromDate;
			datToDate.Value = ToDate;
			if (StockAccountId > 0)
			{
				CmbStockAccount.Value = StockAccountId;
			}
			if (ItemId > 0)
			{
				cmbitem.Value = ItemId;
			}
			if (EntryType != string.Empty)
			{
				string[] array = (EntryType + ",").Split(',');
				foreach (string author in array)
				{
					for (int i = 0; i < ((DisposableObjectCollectionBase)((UltraGridBase)CmbEntryType).Rows).Count; i++)
					{
						if (Conversion.ToString(((UltraGridBase)CmbEntryType).Rows[i].Cells["Activity"].Value) == Conversion.ToString((object)author))
						{
							((UltraGridBase)CmbEntryType).Rows[i].Cells["Selected"].Value = true;
						}
					}
				}
			}
			Gridfill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BranchesFill()
	{
		try
		{
			dtBranch = InvFoodProduction.GetBranchsAllocatedToUserFromProduction(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, 0, "");
			if (dtBranch.Rows.Count > 0)
			{
				DDL.BindDDL(dtBranch, cmbBranchName, "BranchId", "BranchName", "Branch Name", ZeroIndex: false);
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

	private void JobOrderBind()
	{
		try
		{
			DataTable dt = InvFoodProduction.getJobOrderFromProduction(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (dt != null && dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbJoborder, "Id", "JobOrderNo", "JobOrderNo", ZeroIndex: true);
				((UltraGridBase)cmbJoborder).DisplayLayout.Bands[0].Columns[1].Width = 350;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllCombobind()
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		try
		{
			int Idp = Conversion.ToInt(cmbparentcategory.Value);
			int Idwa = Conversion.ToInt(cmbWarehouse.Value);
			int Idit = Conversion.ToInt(cmbitem.Value);
			Conversion.ToInt(cmbJoborder.Value);
			int Idp2 = Conversion.ToInt(cmbplant.Value);
			int Idwip = Conversion.ToInt(CmbWipAccount.Value);
			DataTable dtcombo = InvFoodProduction.GetDataForDropDownFromFoodProduction(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtcombo.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtparent = new DataTable();
			dtparent.Columns.Add("Id", typeof(int));
			dtparent.Columns.Add("ParentCategory");
			DataTable dtware = new DataTable();
			dtware.Columns.Add("Id", typeof(int));
			dtware.Columns.Add("Warehouse");
			DataTable dtitem = new DataTable();
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName");
			DataTable dtST = new DataTable();
			dtST.Columns.Add("Id", typeof(int));
			dtST.Columns.Add("Name");
			DataTable dtplant = new DataTable();
			dtplant.Columns.Add("Id", typeof(int));
			dtplant.Columns.Add("Plant");
			DataTable dtWip = new DataTable();
			dtWip.Columns.Add("Id", typeof(int));
			dtWip.Columns.Add("WipAccount");
			for (int i = 0; i < dtcombo.Rows.Count; i++)
			{
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "ParentCategory")
				{
					dtparent.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "Warehouse")
				{
					dtware.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "ItemName")
				{
					dtitem.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "Plant")
				{
					dtplant.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "WipAccount")
				{
					dtWip.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "StockAccount")
				{
					dtST.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
			}
			if (dtparent.Rows.Count > 0)
			{
				DDL.BindDDL(dtparent, cmbparentcategory, "Id", "ParentCategory", "Parent Category", ZeroIndex: true);
				((UltraGridBase)cmbparentcategory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)cmbparentcategory).Rows[0].Activate();
			}
			if (dtware.Rows.Count > 0)
			{
				DDL.BindDDL(dtware, cmbWarehouse, "Id", "Warehouse", "Warehouse", ZeroIndex: true);
				((UltraGridBase)cmbWarehouse).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)cmbWarehouse).Rows[0].Activate();
			}
			if (dtitem.Rows.Count > 0)
			{
				DDL.BindDDL(dtitem, cmbitem, "Id", "ItemName", "Item", ZeroIndex: true);
				((UltraGridBase)cmbitem).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)cmbitem).Rows[0].Activate();
			}
			if (dtplant.Rows.Count > 0)
			{
				DDL.BindDDL(dtplant, cmbplant, "Id", "Plant", "Plant", ZeroIndex: true);
				((UltraGridBase)cmbplant).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)cmbplant).Rows[0].Activate();
			}
			if (dtWip.Rows.Count > 0)
			{
				DDL.BindDDL(dtWip, CmbWipAccount, "Id", "WipAccount", "WipAccount", ZeroIndex: true);
				((UltraGridBase)CmbWipAccount).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)CmbWipAccount).Rows[0].Activate();
			}
			if (dtST.Rows.Count > 0)
			{
				DDL.BindDDL(dtST, CmbStockAccount, "Id", "Name", "Stock Account", ZeroIndex: true);
				((UltraGridBase)CmbStockAccount).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)CmbStockAccount).Rows[0].Activate();
			}
			if (Idp > 0)
			{
				if (dtparent.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Idp))
				{
					cmbparentcategory.Value = Idp;
				}
				else
				{
					((Control)(object)cmbparentcategory).Text = string.Empty;
				}
			}
			if (Idwa > 0)
			{
				if (dtware.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Idwa))
				{
					cmbWarehouse.Value = Idwa;
				}
				else
				{
					((Control)(object)cmbWarehouse).Text = string.Empty;
				}
			}
			if (Idit > 0)
			{
				if (dtitem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Idit))
				{
					cmbitem.Value = Idit;
				}
				else
				{
					((Control)(object)cmbitem).Text = string.Empty;
				}
			}
			if (Idp2 > 0)
			{
				if (dtplant.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Idp2))
				{
					cmbplant.Value = Idp2;
				}
				else
				{
					((Control)(object)cmbplant).Text = string.Empty;
				}
			}
			if (Idwip > 0)
			{
				if (dtWip.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Idwip))
				{
					CmbWipAccount.Value = Idwip;
				}
				else
				{
					((Control)(object)CmbWipAccount).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EntryTypeFill()
	{
		try
		{
			if (dtEntryType.Columns.Count == 0)
			{
				dtEntryType.Columns.Add("Id");
				dtEntryType.Columns.Add("Activity");
			}
			dtEntryType.Rows.Add("Issue", "Material Input");
			dtEntryType.Rows.Add("ByProduct", "OutPut By Product");
			dtEntryType.Rows.Add("FinishGoods", "OutPut Finish Goods");
			DDL.BindDDL(dtEntryType, CmbEntryType, "Id", "Activity", "Entry Type", ZeroIndex: true);
			((UltraGridBase)CmbEntryType).Rows[0].Activate();
			UltraGridColumn obj = ((UltraGridBase)CmbEntryType).DisplayLayout.Bands[0].Columns.Add();
			((KeyedSubObjectBase)obj).Key = "Selected";
			((HeaderBase)obj.Header).Caption = string.Empty;
			obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
			obj.DataType = typeof(bool);
			((HeaderBase)obj.Header).VisiblePosition = 0;
			CmbEntryType.CheckedListSettings.CheckStateMember = "Selected";
			((EditorCheckedListSettings)CmbEntryType.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
			((EditorCheckedListSettings)CmbEntryType.CheckedListSettings).ListSeparator = ",";
			((EditorCheckedListSettings)CmbEntryType.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
			((UltraGridBase)CmbEntryType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Activityfill()
	{
		try
		{
			DataTable dtActivity = new DataTable();
			dtActivity.Columns.Add("Id");
			dtActivity.Columns.Add("Activity");
			dtActivity.Rows.Add(1, "Production Register");
			dtActivity.Rows.Add(2, "OutPut By Packing Material");
			dtActivity.Rows.Add(3, "Production_Summary");
			if (dtActivity.Rows.Count > 0)
			{
				DDL.BindDDL(dtActivity, cmbActivity, "Id", "Activity", "Activity Type", ZeroIndex: true);
				((UltraGridBase)cmbActivity).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)cmbActivity).Rows[1].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Gridfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			string Ids = "";
			if (Conversion.ToString((object)((Control)(object)CmbEntryType).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)CmbEntryType).Text) + ",").Split(',');
				foreach (string author2 in array)
				{
					DataRow[] dr2 = dtEntryType.Select("Activity='" + Conversion.ToString((object)author2) + "'");
					if (dr2.Length != 0)
					{
						Ids = Ids + "," + Conversion.ToString(dr2[0].ItemArray[0]);
					}
				}
				obj.EntryType = Ids;
			}
			dt1.Rows.Clear();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.JobOrderId = Conversion.ToInt(cmbJoborder.Value);
			if (rdDocDate.Checked)
			{
				if (datFromDate.Checked)
				{
					obj.FromDate = Conversion.ToDateTime((object)datFromDate.Value);
				}
				obj.ToDate = Conversion.ToDateTime((object)datToDate.Value);
			}
			else if (rdEntryDate.Checked)
			{
				if (datFromDate.Checked)
				{
					obj.EntryDateFrom = Conversion.ToDateTime((object)datFromDate.Value);
				}
				obj.EntryDateTo = Conversion.ToDateTime((object)datToDate.Value);
			}
			obj.ParentCategoryId = Conversion.ToInt(cmbparentcategory.Value);
			obj.WarehouseId = Conversion.ToInt(cmbWarehouse.Value);
			obj.ItemId = Conversion.ToInt(cmbitem.Value);
			obj.PlantId = Conversion.ToInt(cmbplant.Value);
			obj.AccountId = Conversion.ToInt(CmbWipAccount.Value);
			obj.StockAccountId = Conversion.ToInt(CmbStockAccount.Value);
			obj.Activity = Conversion.ToString((object)((Control)(object)cmbActivity).Text);
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
				obj.BranchesIds = BranchIds;
				dt1 = InvFoodProduction.ProductionRegister(obj);
				if (dt1.Rows.Count > 0)
				{
					if (Conversion.ToString((object)((Control)(object)cmbActivity).Text) == "Production Register")
					{
						SummaryInfoBox.Visible = false;
						DataTable dt = new DataTable();
						dt.Columns.Add("InvFoodProductionId", typeof(int));
						dt.Columns.Add("BranchName", typeof(string));
						dt.Columns.Add("DocDate", typeof(DateTime));
						dt.Columns.Add("DocNo", typeof(string));
						dt.Columns.Add("JobOrder", typeof(string));
						dt.Columns.Add("ReUseJobOrder", typeof(string));
						dt.Columns.Add("PlantName", typeof(string));
						dt.Columns.Add("RefDocType", typeof(string));
						dt.Columns.Add("RefDocNo", typeof(int));
						dt.Columns.Add("RefDocDate", typeof(DateTime));
						dt.Columns.Add("GrnNo", typeof(int));
						dt.Columns.Add("GrnDate", typeof(DateTime));
						dt.Columns.Add("VehicleNo", typeof(string));
						dt.Columns.Add("BiltyNo", typeof(string));
						dt.Columns.Add("SupplierOtherRef", typeof(string));
						dt.Columns.Add("Warehouse", typeof(string));
						dt.Columns.Add("Item", typeof(string));
						dt.Columns.Add("CropYear", typeof(string));
						dt.Columns.Add("JobLot", typeof(string));
						dt.Columns.Add("PackingType", typeof(string));
						dt.Columns.Add("UOM", typeof(string));
						dt.Columns.Add("ItemQty", typeof(double));
						dt.Columns.Add("Weight", typeof(double));
						dt.Columns.Add("Rate", typeof(double));
						dt.Columns.Add("RatePerKg", typeof(double));
						dt.Columns.Add("Amount", typeof(double));
						dt.Columns.Add("EntryType", typeof(string));
						dt.Columns.Add("WipAccount", typeof(string));
						dt.Columns.Add("EntryDate", typeof(DateTime));
						dt.Columns.Add("EntryUser", typeof(string));
						dt.Columns.Add("ModifyDate", typeof(DateTime));
						dt.Columns.Add("ModifyUser", typeof(string));
						dt.Columns.Add("MainRemarks", typeof(string));
						for (int i = 0; i < dt1.Rows.Count; i++)
						{
							dt.Rows.Add(Conversion.ToInt(dt1.Rows[i]["InvFoodProductionId"]), Conversion.ToString(dt1.Rows[i]["BranchName"]), Conversion.ToDateTime(dt1.Rows[i]["DocDate"]), Conversion.ToString(dt1.Rows[i]["DocCode"]), Conversion.ToString(dt1.Rows[i]["JobOrder"]), Conversion.ToString(dt1.Rows[i]["ReUseJobOrder"]), Conversion.ToString(dt1.Rows[i]["PlantName"]), Conversion.ToString(dt1.Rows[i]["RefDocType"]), Conversion.ToInt(dt1.Rows[i]["RefDocNo"]), Conversion.ToDateTime(dt1.Rows[i]["RefDocDate"]).ToShortDateString(), Conversion.ToInt(dt1.Rows[i]["GrnNo"]), Conversion.ToDateTime(dt1.Rows[i]["GrnDate"]).ToShortDateString(), Conversion.ToString(dt1.Rows[i]["VehicleNo"]), Conversion.ToString(dt1.Rows[i]["BiltyNo"]), Conversion.ToString(dt1.Rows[i]["SupplierOtherRef"]), Conversion.ToString(dt1.Rows[i]["WareHouseName"]), Conversion.ToString(dt1.Rows[i]["ItemName"]), Conversion.ToString(dt1.Rows[i]["CropBatch"]), Conversion.ToString(dt1.Rows[i]["JobLotDescription"]), Conversion.ToString(dt1.Rows[i]["PackTypeDesc"]), Conversion.ToString(dt1.Rows[i]["PackUom"]), Conversion.ToDouble(dt1.Rows[i]["Qty"]), Conversion.ToDouble(dt1.Rows[i]["Weight"]), Conversion.ToDouble(dt1.Rows[i]["Rate"]), Conversion.ToDouble(dt1.Rows[i]["Amount"]) / Conversion.ToDouble(dt1.Rows[i]["Weight"]), Conversion.ToDouble(dt1.Rows[i]["Amount"]), Conversion.ToString(dt1.Rows[i]["EntryType"]), Conversion.ToString(dt1.Rows[i]["WipAccount"]), dt1.Rows[i]["EntryDate"], Conversion.ToString(dt1.Rows[i]["EntryUser"]), dt1.Rows[i]["ModifyDate"], Conversion.ToString(dt1.Rows[i]["ModifyUser"]), Conversion.ToString(dt1.Rows[i]["MainRemarks"]));
						}
						((Control)(object)grd).Name = "ProductionRegister";
						grd.DataSource = dt;
						grd.RetrieveStructure();
						grd.RootTable.Groups.Add("EntryType");
						grdsetting();
					}
					else if (Conversion.ToString((object)((Control)(object)cmbActivity).Text) == "OutPut By Packing Material")
					{
						SummaryInfoBox.Visible = false;
						DataTable dtOutPut = new DataTable();
						dtOutPut.Columns.Add("JobOrder", typeof(string));
						dtOutPut.Columns.Add("Item", typeof(string));
						dtOutPut.Columns.Add("UOM", typeof(string));
						dtOutPut.Columns.Add("PmItem", typeof(string));
						dtOutPut.Columns.Add("BrandQty", typeof(double));
						dtOutPut.Columns.Add("BrandWeight", typeof(double));
						dtOutPut.Columns.Add("PmQty", typeof(double));
						for (int j = 0; j < dt1.Rows.Count; j++)
						{
							dtOutPut.Rows.Add(Conversion.ToString(dt1.Rows[j]["JobOrder"]), Conversion.ToString(dt1.Rows[j]["ItemName"]), Conversion.ToString(dt1.Rows[j]["PackUom"]), Conversion.ToString(dt1.Rows[j]["PmItem"]), Conversion.ToDouble(dt1.Rows[j]["BrandQty"]), Conversion.ToDouble(dt1.Rows[j]["BrandWeight"]), Conversion.ToDouble(dt1.Rows[j]["PmQty"]));
						}
						((Control)(object)grd).Name = "OutPutByPackingMaterial";
						grd.DataSource = dtOutPut;
						grd.RetrieveStructure();
						grdoutsetting();
					}
					else if (Conversion.ToString((object)((Control)(object)cmbActivity).Text) == "Production_Summary")
					{
						SummaryInfoBox.Visible = true;
						DataTable dtSummery = new DataTable();
						dtSummery.Columns.Add("EntryType", typeof(string));
						dtSummery.Columns.Add("JobOrderId", typeof(int));
						dtSummery.Columns.Add("JobOrder", typeof(string));
						dtSummery.Columns.Add("PlantId", typeof(int));
						dtSummery.Columns.Add("Plant", typeof(string));
						dtSummery.Columns.Add("ItemId", typeof(string));
						dtSummery.Columns.Add("ItemName", typeof(string));
						dtSummery.Columns.Add("Qty", typeof(double));
						dtSummery.Columns.Add("Weight", typeof(double));
						for (int k = 0; k < dt1.Rows.Count; k++)
						{
							dtSummery.Rows.Add(Conversion.ToString(dt1.Rows[k]["EntryType"]), Conversion.ToInt(dt1.Rows[k]["InvJobOrderId"]), Conversion.ToString(dt1.Rows[k]["JobOrder"]), Conversion.ToInt(dt1.Rows[k]["PlantId"]), Conversion.ToString(dt1.Rows[k]["PlantName"]), Conversion.ToInt(dt1.Rows[k]["ItemId"]), Conversion.ToString(dt1.Rows[k]["ItemName"]), Conversion.ToDouble(dt1.Rows[k]["Qty"]), Conversion.ToDouble(dt1.Rows[k]["Weight"]));
						}
						double Issuance = Conversion.ToDouble(dtSummery.Compute("Sum(Weight)", "EntryType = 'Material Input'"));
						double ByProduct = Conversion.ToDouble(dtSummery.Compute("Sum(Weight)", "EntryType = 'OutPut By Product'"));
						double FinishGoods = Conversion.ToDouble(dtSummery.Compute("Sum(Weight)", "EntryType = 'OutPut Finish Goods'"));
						TotalIssuance.Text = Issuance.ToString("#,##0.###");
						TotalByProduct.Text = ByProduct.ToString("#,##0.###");
						TotalFinishGoods.Text = FinishGoods.ToString("#,##0.###");
						TotalInput.Text = Issuance.ToString("#,##0.###");
						TotalOutPut.Text = (ByProduct + FinishGoods).ToString("#,##0.###");
						TotalDifference.Text = (Issuance - (ByProduct + FinishGoods)).ToString("#,##0.###");
						((Control)(object)grd).Name = "Production_Summary";
						grd.DataSource = dtSummery;
						grd.RetrieveStructure();
						grdSummarySetting();
					}
					PrintButtonManage();
				}
				else
				{
					grd.DataSource = null;
				}
				return;
			}
			cmbBranchName.Focus();
			throw new Exception("Select Branch First");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdsetting()
	{
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.DefaultFilterRowComparison = (FilterConditionOperator)7;
			grd.RootTable.Columns["InvFoodProductionId"].Visible = false;
			grd.RootTable.Columns["ModifyDate"].Visible = false;
			grd.RootTable.Columns["ModifyUser"].Visible = false;
			grd.RootTable.Columns["MainRemarks"].Visible = false;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grd.RootTable.HeaderLines = 2;
			grd.RootTable.Columns["BranchName"].Width = 120;
			grd.RootTable.Columns["DocDate"].Width = 70;
			grd.RootTable.Columns["DocNo"].Width = 55;
			grd.RootTable.Columns["JobOrder"].Width = 100;
			grd.RootTable.Columns["ReUseJobOrder"].Width = 100;
			grd.RootTable.Columns["PlantName"].Width = 80;
			grd.RootTable.Columns["RefDocType"].Width = 65;
			grd.RootTable.Columns["RefDocNo"].Width = 80;
			grd.RootTable.Columns["RefDocDate"].Width = 90;
			grd.RootTable.Columns["WipAccount"].Width = 100;
			grd.RootTable.Columns["SupplierOtherRef"].Width = 100;
			grd.RootTable.Columns["SupplierOtherRef"].Caption = "Supplier Reference";
			grd.RootTable.Columns["Warehouse"].Width = 100;
			grd.RootTable.Columns["Item"].Width = 120;
			grd.RootTable.Columns["CropYear"].Width = 60;
			grd.RootTable.Columns["CropYear"].Caption = "Crop";
			grd.RootTable.Columns["JobLot"].Width = 70;
			grd.RootTable.Columns["PackingType"].Width = 70;
			grd.RootTable.Columns["UOM"].Width = 50;
			grd.RootTable.Columns["ItemQty"].Width = 85;
			grd.RootTable.Columns["Weight"].Width = 100;
			grd.RootTable.Columns["Weight"].Caption = "Net Weight";
			grd.RootTable.Columns["Rate"].Width = 70;
			grd.RootTable.Columns["RatePerKg"].Width = 80;
			grd.RootTable.Columns["Amount"].Width = 110;
			grd.RootTable.Columns["EntryType"].Width = 100;
			grd.RootTable.Columns["EntryDate"].Width = 135;
			grd.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grd.RootTable.Columns["EntryUser"].Width = 80;
			grd.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grd.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Weight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["Weight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)3;
			grd.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Rate"].FormatString = "#,##0.###";
			grd.RootTable.Columns["Rate"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["RatePerKg"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RatePerKg"].FormatString = "#,##0.####";
			grd.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Amount"].FormatString = "#,##0.###";
			grd.RootTable.Columns["Amount"].TotalFormatString = "#,##0.###";
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdoutsetting()
	{
		try
		{
			grd.DefaultFilterRowComparison = (FilterConditionOperator)7;
			grd.RootTable.Columns["JobOrder"].Width = 140;
			grd.RootTable.Columns["Item"].Width = 210;
			grd.RootTable.Columns["UOM"].Width = 50;
			grd.RootTable.Columns["PmItem"].Width = 200;
			grd.RootTable.Columns["BrandQty"].Width = 90;
			grd.RootTable.Columns["BrandWeight"].Width = 90;
			grd.RootTable.Columns["PmQty"].Width = 90;
			grd.RootTable.Columns["PmQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["PmQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["PmQty"].FormatString = "#,##0.##";
			grd.RootTable.Columns["PmQty"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["BrandWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BrandWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BrandWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["BrandWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["BrandQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BrandQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BrandQty"].FormatString = "#,##0.##";
			grd.RootTable.Columns["BrandQty"].TotalFormatString = "#,##0.##";
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSummarySetting()
	{
		try
		{
			grd.DefaultFilterRowComparison = (FilterConditionOperator)7;
			grd.RootTable.Groups.Add("EntryType");
			grd.RootTable.Columns["EntryType"].HideWhenGrouped = (InheritableBoolean)1;
			grd.RootTable.Columns["JobOrderId"].Visible = false;
			grd.RootTable.Columns["PlantId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["EntryType"].Width = 130;
			grd.RootTable.Columns["JobOrder"].Width = 150;
			grd.RootTable.Columns["Plant"].Width = 90;
			grd.RootTable.Columns["ItemName"].Width = 210;
			grd.RootTable.Columns["Qty"].Width = 100;
			grd.RootTable.Columns["Weight"].Width = 100;
			grd.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Qty"].FormatString = "#,##0.##";
			grd.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Weight"].FormatString = "#,##0.###";
			grd.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
			ctrlGrdBar1_Load(null, null);
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
			Gridfill();
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
			((Control)(object)cmbparentcategory).Text = string.Empty;
			((Control)(object)cmbWarehouse).Text = string.Empty;
			((Control)(object)cmbitem).Text = string.Empty;
			((Control)(object)cmbJoborder).Text = string.Empty;
			((Control)(object)cmbplant).Text = string.Empty;
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
			AllCombobind();
			JobOrderBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ProductionRegister_FormClosed(object sender, FormClosedEventArgs e)
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

	private void ProductionRegister_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshow_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Alt)
			{
				switch (e.KeyCode)
				{
				case Keys.D1:
					if (btnPrintCurrent.Text == "553-ProductionRegisterWithActivity")
					{
						if (dt1.Rows.Count == 0 || dt1 == null)
						{
							throw new Exception("Record Not Found For Display");
						}
						Reporting val2 = new Reporting();
						val2.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
						val2.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
						val2.ShowReportWithDataTable(dt1, "553-ProductionRegisterWithActivity.rpt");
					}
					break;
				case Keys.D2:
					if (btnPrintCurrent.Text == "554-ProductionRegisterWithActivity(OutPut By Packing Material)")
					{
						if (dt1.Rows.Count == 0 || dt1 == null)
						{
							throw new Exception("Record Not Found For Display");
						}
						Reporting val = new Reporting();
						val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
						val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
						val.ShowReportWithDataTable(dt1, "554-ProductionRegisterWithActivity(OutPut By Packing Material).rpt");
					}
					break;
				}
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grd).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				datFromDate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				datFromDate.Focus();
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
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
			dt.Rows.Add("Ctrl+S", "For Show");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Alt+2", "For print 554-ProductionRegisterWithActivity(OutPut By Packing Material)");
			dt.Rows.Add("Alt+1", "For Print 553-ProductionRegisterWithActivity");
			dt.Rows.Add("Ctrl+F5", "For Focus On DateType ");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Supplier Customer in Filter");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortCutKeys_Click(object sender, EventArgs e)
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

	private void PrintButtonManage()
	{
		try
		{
			if (((UltraGridBase)cmbActivity).ActiveRow != null)
			{
				if (Conversion.ToString((object)((Control)(object)cmbActivity).Text) == "Production Register")
				{
					btnPrintCurrent.Text = "553-ProductionRegisterWithActivity";
				}
				else if (Conversion.ToString((object)((Control)(object)cmbActivity).Text) == "OutPut By Packing Material")
				{
					btnPrintCurrent.Text = "554-ProductionRegisterWithActivity(OutPut By Packing Material)";
				}
				else if (Conversion.ToString((object)((Control)(object)cmbActivity).Text) == "Production_Summary")
				{
					btnPrintCurrent.Text = "562-ProductionRegisterWithActivity";
				}
			}
			else
			{
				btnPrintCurrent.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrintCurrent_Click(object sender, EventArgs e)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (btnPrintCurrent.Text == "553-ProductionRegisterWithActivity")
			{
				if (dt1.Rows.Count == 0 || dt1 == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt1, "553-ProductionRegisterWithActivity.rpt");
			}
			else if (btnPrintCurrent.Text == "554-ProductionRegisterWithActivity(OutPut By Packing Material)")
			{
				if (dt1.Rows.Count == 0 || dt1 == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val2 = new Reporting();
				val2.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val2.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val2.ShowReportWithDataTable(dt1, "554-ProductionRegisterWithActivity(OutPut By Packing Material).rpt");
			}
			else if (btnPrintCurrent.Text == "562-ProductionRegisterWithActivity")
			{
				if (dt1.Rows.Count == 0 || dt1 == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val3 = new Reporting();
				val3.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val3.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val3.ShowReportWithDataTable(dt1, "562-ProductionRegisterWithActivity.rpt");
			}
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
			if (base.ActiveControl != null && base.ActiveControl.Name == "ctrlGrdBar1")
			{
				datFromDate.Focus();
			}
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
			if (((Control)(object)cmbBranchName).Text != string.Empty && ((UltraGridBase)cmbBranchName).ActiveRow != null)
			{
				AllCombobind();
				return;
			}
			((Control)(object)cmbparentcategory).Text = string.Empty;
			((UltraGridBase)cmbparentcategory).ActiveRow = null;
			((Control)(object)cmbWarehouse).Text = string.Empty;
			((UltraGridBase)cmbWarehouse).ActiveRow = null;
			((Control)(object)cmbitem).Text = string.Empty;
			((UltraGridBase)cmbitem).ActiveRow = null;
			((Control)(object)cmbJoborder).Text = string.Empty;
			((UltraGridBase)cmbJoborder).ActiveRow = null;
			((Control)(object)cmbplant).Text = string.Empty;
			((UltraGridBase)cmbplant).ActiveRow = null;
			((Control)(object)CmbEntryType).Text = string.Empty;
			((UltraGridBase)CmbEntryType).ActiveRow = null;
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
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Expected O, but got Unknown
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Expected O, but got Unknown
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Expected O, but got Unknown
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Expected O, but got Unknown
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Expected O, but got Unknown
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Expected O, but got Unknown
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Expected O, but got Unknown
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Expected O, but got Unknown
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Expected O, but got Unknown
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Expected O, but got Unknown
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Expected O, but got Unknown
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Expected O, but got Unknown
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Expected O, but got Unknown
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Expected O, but got Unknown
		//IL_0470: Unknown result type (might be due to invalid IL or missing references)
		//IL_047a: Expected O, but got Unknown
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d2: Expected O, but got Unknown
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Expected O, but got Unknown
		//IL_04e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f3: Expected O, but got Unknown
		//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fe: Expected O, but got Unknown
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Expected O, but got Unknown
		//IL_052b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Expected O, but got Unknown
		//IL_05a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ae: Expected O, but got Unknown
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Expected O, but got Unknown
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
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance56 = new Appearance();
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
		Appearance appearance82 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Production_Reports.ProductionRegister));
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
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnPrintCurrent = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.panel1 = new System.Windows.Forms.Panel();
		this.SummaryInfoBox = new System.Windows.Forms.GroupBox();
		this.TotalDifference = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.TotalOutPut = new System.Windows.Forms.Label();
		this.TotalInput = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.TotalFinishGoods = new System.Windows.Forms.Label();
		this.TotalByProduct = new System.Windows.Forms.Label();
		this.TotalIssuance = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.cmbBranchName = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.rdEntryDate = new System.Windows.Forms.RadioButton();
		this.cmbWarehouse = new UltraCombo();
		this.Warehouse = new System.Windows.Forms.Label();
		this.cmbparentcategory = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.rdDocDate = new System.Windows.Forms.RadioButton();
		this.btnshow = new System.Windows.Forms.Button();
		this.label11 = new System.Windows.Forms.Label();
		this.datToDate = new System.Windows.Forms.DateTimePicker();
		this.label16 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.CmbEntryType = new UltraCombo();
		this.label27 = new System.Windows.Forms.Label();
		this.CmbWipAccount = new UltraCombo();
		this.cmbActivity = new UltraCombo();
		this.cmbplant = new UltraCombo();
		this.datFromDate = new System.Windows.Forms.DateTimePicker();
		this.label6 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.cmbitem = new UltraCombo();
		this.cmbJoborder = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.rdFinancialYear = new System.Windows.Forms.RadioButton();
		this.rdThisYear = new System.Windows.Forms.RadioButton();
		this.rdThisMonth = new System.Windows.Forms.RadioButton();
		this.rdthisWeek = new System.Windows.Forms.RadioButton();
		this.rdThisDay = new System.Windows.Forms.RadioButton();
		this.label23 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grd = new GridEX();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbStockAccount = new UltraCombo();
		this.label38 = new System.Windows.Forms.Label();
		this.toolStrip3.SuspendLayout();
		this.panel1.SuspendLayout();
		this.SummaryInfoBox.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWarehouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbparentcategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbEntryType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWipAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbActivity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbplant).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJoborder).BeginInit();
		this.panel2.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStockAccount).BeginInit();
		base.SuspendLayout();
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNew, this.btnRefresh, this.btnPrintCurrent, this.btnShortCutKeys });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1302, 27);
		this.toolStrip3.TabIndex = 1;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(68, 24);
		this.btnNew.Text = "&Reset";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnPrintCurrent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrintCurrent.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintCurrent.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintCurrent.Name = "btnPrintCurrent";
		this.btnPrintCurrent.Size = new System.Drawing.Size(61, 24);
		this.btnPrintCurrent.Text = "Print";
		this.btnPrintCurrent.ToolTipText = "Print";
		this.btnPrintCurrent.Click += new System.EventHandler(btnPrintCurrent_Click);
		this.btnShortCutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeys.Name = "btnShortCutKeys";
		this.btnShortCutKeys.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKeys.Text = "ShortCut Keys";
		this.btnShortCutKeys.Click += new System.EventHandler(btnShortCutKeys_Click);
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.SummaryInfoBox);
		this.panel1.Controls.Add(this.groupBox1);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel1.Location = new System.Drawing.Point(0, 27);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1302, 166);
		this.panel1.TabIndex = 2;
		this.SummaryInfoBox.Controls.Add(this.TotalDifference);
		this.SummaryInfoBox.Controls.Add(this.label12);
		this.SummaryInfoBox.Controls.Add(this.TotalOutPut);
		this.SummaryInfoBox.Controls.Add(this.TotalInput);
		this.SummaryInfoBox.Controls.Add(this.label15);
		this.SummaryInfoBox.Controls.Add(this.label14);
		this.SummaryInfoBox.Controls.Add(this.TotalFinishGoods);
		this.SummaryInfoBox.Controls.Add(this.TotalByProduct);
		this.SummaryInfoBox.Controls.Add(this.TotalIssuance);
		this.SummaryInfoBox.Controls.Add(this.label10);
		this.SummaryInfoBox.Controls.Add(this.label8);
		this.SummaryInfoBox.Controls.Add(this.label7);
		this.SummaryInfoBox.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.SummaryInfoBox.Location = new System.Drawing.Point(1009, 28);
		this.SummaryInfoBox.Name = "SummaryInfoBox";
		this.SummaryInfoBox.Size = new System.Drawing.Size(289, 133);
		this.SummaryInfoBox.TabIndex = 0;
		this.SummaryInfoBox.TabStop = false;
		this.SummaryInfoBox.Text = "Production Information";
		this.SummaryInfoBox.Visible = false;
		this.TotalDifference.AutoSize = true;
		this.TotalDifference.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.TotalDifference.ForeColor = System.Drawing.Color.Teal;
		this.TotalDifference.Location = new System.Drawing.Point(94, 106);
		this.TotalDifference.Name = "TotalDifference";
		this.TotalDifference.Size = new System.Drawing.Size(15, 16);
		this.TotalDifference.TabIndex = 11;
		this.TotalDifference.Text = "0";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(6, 107);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(62, 15);
		this.label12.TabIndex = 10;
		this.label12.Text = "Difference";
		this.TotalOutPut.AutoSize = true;
		this.TotalOutPut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.TotalOutPut.ForeColor = System.Drawing.Color.Teal;
		this.TotalOutPut.Location = new System.Drawing.Point(94, 84);
		this.TotalOutPut.Name = "TotalOutPut";
		this.TotalOutPut.Size = new System.Drawing.Size(15, 16);
		this.TotalOutPut.TabIndex = 9;
		this.TotalOutPut.Text = "0";
		this.TotalInput.AutoSize = true;
		this.TotalInput.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.TotalInput.ForeColor = System.Drawing.Color.Teal;
		this.TotalInput.Location = new System.Drawing.Point(94, 61);
		this.TotalInput.Name = "TotalInput";
		this.TotalInput.Size = new System.Drawing.Size(15, 16);
		this.TotalInput.TabIndex = 8;
		this.TotalInput.Text = "0";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(6, 85);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(74, 15);
		this.label15.TabIndex = 7;
		this.label15.Text = "Total Output";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(6, 62);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(65, 15);
		this.label14.TabIndex = 6;
		this.label14.Text = "Total Input";
		this.TotalFinishGoods.AutoSize = true;
		this.TotalFinishGoods.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.TotalFinishGoods.ForeColor = System.Drawing.Color.Teal;
		this.TotalFinishGoods.Location = new System.Drawing.Point(188, 38);
		this.TotalFinishGoods.Name = "TotalFinishGoods";
		this.TotalFinishGoods.Size = new System.Drawing.Size(89, 16);
		this.TotalFinishGoods.TabIndex = 5;
		this.TotalFinishGoods.Text = "Finish Goods";
		this.TotalByProduct.AutoSize = true;
		this.TotalByProduct.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.TotalByProduct.ForeColor = System.Drawing.Color.Teal;
		this.TotalByProduct.Location = new System.Drawing.Point(94, 38);
		this.TotalByProduct.Name = "TotalByProduct";
		this.TotalByProduct.Size = new System.Drawing.Size(79, 16);
		this.TotalByProduct.TabIndex = 4;
		this.TotalByProduct.Text = "By Product";
		this.TotalIssuance.AutoSize = true;
		this.TotalIssuance.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.TotalIssuance.ForeColor = System.Drawing.Color.Teal;
		this.TotalIssuance.Location = new System.Drawing.Point(6, 38);
		this.TotalIssuance.Name = "TotalIssuance";
		this.TotalIssuance.Size = new System.Drawing.Size(66, 16);
		this.TotalIssuance.TabIndex = 3;
		this.TotalIssuance.Text = "Issuance";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(188, 19);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(75, 15);
		this.label10.TabIndex = 2;
		this.label10.Text = "Finish Goods";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(94, 19);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(65, 15);
		this.label8.TabIndex = 1;
		this.label8.Text = "By Product";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(6, 19);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(53, 15);
		this.label7.TabIndex = 0;
		this.label7.Text = "Issuance";
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbStockAccount);
		this.groupBox1.Controls.Add(this.label38);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchName);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add(this.rdEntryDate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbWarehouse);
		this.groupBox1.Controls.Add(this.Warehouse);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbparentcategory);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.rdDocDate);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.datToDate);
		this.groupBox1.Controls.Add(this.label16);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbEntryType);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWipAccount);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbActivity);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbplant);
		this.groupBox1.Controls.Add(this.datFromDate);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbitem);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbJoborder);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, 29);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1003, 133);
		this.groupBox1.TabIndex = 134;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.cmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance34).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance35).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Location = new System.Drawing.Point(106, 15);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Name = "cmbBranchName";
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Size = new System.Drawing.Size(225, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Leave += new System.EventHandler(cmbBranchName_Leave);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(6, 21);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(79, 15);
		this.label13.TabIndex = 0;
		this.label13.Text = "Branch Name";
		this.rdEntryDate.AutoSize = true;
		this.rdEntryDate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdEntryDate.Location = new System.Drawing.Point(921, 44);
		this.rdEntryDate.Name = "rdEntryDate";
		this.rdEntryDate.Size = new System.Drawing.Size(80, 19);
		this.rdEntryDate.TabIndex = 15;
		this.rdEntryDate.Text = "Entry Date";
		this.rdEntryDate.UseVisualStyleBackColor = true;
		this.cmbWarehouse.AllowNull = (DefaultableBoolean)1;
		this.cmbWarehouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWarehouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWarehouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWarehouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.cmbWarehouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance100).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance101).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance101).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance101).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance77).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbWarehouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbWarehouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbWarehouse).Location = new System.Drawing.Point(427, 98);
		((System.Windows.Forms.Control)(object)this.cmbWarehouse).Name = "cmbWarehouse";
		((System.Windows.Forms.Control)(object)this.cmbWarehouse).Size = new System.Drawing.Size(225, 26);
		((System.Windows.Forms.Control)(object)this.cmbWarehouse).TabIndex = 9;
		this.Warehouse.AutoSize = true;
		this.Warehouse.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Warehouse.Location = new System.Drawing.Point(335, 104);
		this.Warehouse.Name = "Warehouse";
		this.Warehouse.Size = new System.Drawing.Size(67, 15);
		this.Warehouse.TabIndex = 120;
		this.Warehouse.Text = "Warehouse";
		this.cmbparentcategory.AllowNull = (DefaultableBoolean)1;
		this.cmbparentcategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbparentcategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbparentcategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbparentcategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance5;
		((SpecialBoxBase)((UltraGridBase)this.cmbparentcategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance10).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance11).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance39).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbparentcategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbparentcategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbparentcategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbparentcategory).Location = new System.Drawing.Point(427, 42);
		((System.Windows.Forms.Control)(object)this.cmbparentcategory).Name = "cmbparentcategory";
		((System.Windows.Forms.Control)(object)this.cmbparentcategory).Size = new System.Drawing.Size(225, 26);
		((System.Windows.Forms.Control)(object)this.cmbparentcategory).TabIndex = 7;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(335, 46);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(91, 15);
		this.label3.TabIndex = 124;
		this.label3.Text = "Parent Category";
		this.rdDocDate.AutoSize = true;
		this.rdDocDate.Checked = true;
		this.rdDocDate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdDocDate.Location = new System.Drawing.Point(921, 19);
		this.rdDocDate.Name = "rdDocDate";
		this.rdDocDate.Size = new System.Drawing.Size(75, 19);
		this.rdDocDate.TabIndex = 14;
		this.rdDocDate.TabStop = true;
		this.rdDocDate.Text = "Doc Date";
		this.rdDocDate.UseVisualStyleBackColor = true;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(857, 98);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(59, 26);
		this.btnshow.TabIndex = 13;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.btnshow.Leave += new System.EventHandler(btnshow_Leave);
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(335, 19);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(62, 15);
		this.label11.TabIndex = 215;
		this.label11.Text = "Entry Type";
		this.datToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.CustomFormat = "dd-MMM-yy";
		this.datToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datToDate.Location = new System.Drawing.Point(219, 46);
		this.datToDate.Name = "datToDate";
		this.datToDate.Size = new System.Drawing.Size(112, 23);
		this.datToDate.TabIndex = 3;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(656, 19);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(46, 15);
		this.label16.TabIndex = 213;
		this.label16.Text = "Wip Ac";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(656, 76);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(75, 15);
		this.label9.TabIndex = 213;
		this.label9.Text = "Activity Type";
		this.CmbEntryType.AllowNull = (DefaultableBoolean)1;
		this.CmbEntryType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbEntryType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbEntryType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbEntryType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.CmbEntryType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance72).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance73).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbEntryType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbEntryType).Location = new System.Drawing.Point(427, 14);
		((System.Windows.Forms.Control)(object)this.CmbEntryType).Name = "CmbEntryType";
		((System.Windows.Forms.Control)(object)this.CmbEntryType).Size = new System.Drawing.Size(225, 26);
		((System.Windows.Forms.Control)(object)this.CmbEntryType).TabIndex = 6;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(6, 47);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(91, 15);
		this.label27.TabIndex = 132;
		this.label27.Text = "Date From && To";
		this.CmbWipAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbWipAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWipAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWipAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWipAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbWipAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWipAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWipAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWipAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.CmbWipAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWipAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbWipAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWipAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWipAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWipAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbWipAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWipAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWipAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWipAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbWipAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWipAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance114).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWipAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance115).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWipAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbWipAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWipAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWipAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbWipAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWipAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbWipAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWipAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWipAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWipAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWipAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWipAccount).Location = new System.Drawing.Point(740, 13);
		((System.Windows.Forms.Control)(object)this.CmbWipAccount).Name = "CmbWipAccount";
		((System.Windows.Forms.Control)(object)this.CmbWipAccount).Size = new System.Drawing.Size(176, 26);
		((System.Windows.Forms.Control)(object)this.CmbWipAccount).TabIndex = 10;
		this.cmbActivity.AllowNull = (DefaultableBoolean)1;
		this.cmbActivity.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbActivity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbActivity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbActivity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbActivity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbActivity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbActivity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbActivity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbActivity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbActivity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbActivity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbActivity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbActivity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbActivity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbActivity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbActivity).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbActivity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbActivity).Location = new System.Drawing.Point(740, 70);
		((System.Windows.Forms.Control)(object)this.cmbActivity).Name = "cmbActivity";
		((System.Windows.Forms.Control)(object)this.cmbActivity).Size = new System.Drawing.Size(176, 26);
		((System.Windows.Forms.Control)(object)this.cmbActivity).TabIndex = 12;
		this.cmbplant.AllowNull = (DefaultableBoolean)1;
		this.cmbplant.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbplant.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbplant.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbplant).DisplayLayout.Appearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbplant).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbplant).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbplant).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbplant).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance103;
		((SpecialBoxBase)((UltraGridBase)this.cmbplant).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbplant).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbplant).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbplant).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbplant).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbplant).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbplant).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbplant).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbplant).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbplant).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbplant).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbplant).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbplant).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbplant).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbplant).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbplant).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbplant).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbplant).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbplant).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbplant).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbplant).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbplant).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbplant).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbplant.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbplant).Location = new System.Drawing.Point(106, 98);
		((System.Windows.Forms.Control)(object)this.cmbplant).Name = "cmbplant";
		((System.Windows.Forms.Control)(object)this.cmbplant).Size = new System.Drawing.Size(225, 26);
		((System.Windows.Forms.Control)(object)this.cmbplant).TabIndex = 5;
		this.datFromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.Checked = false;
		this.datFromDate.CustomFormat = "dd-MMM-yy";
		this.datFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFromDate.Location = new System.Drawing.Point(106, 46);
		this.datFromDate.Name = "datFromDate";
		this.datFromDate.ShowCheckBox = true;
		this.datFromDate.Size = new System.Drawing.Size(112, 23);
		this.datFromDate.TabIndex = 2;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(8, 101);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(34, 15);
		this.label6.TabIndex = 128;
		this.label6.Text = "Plant";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(335, 75);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(67, 15);
		this.label2.TabIndex = 122;
		this.label2.Text = "Item Name";
		this.cmbitem.AllowNull = (DefaultableBoolean)1;
		this.cmbitem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbitem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbitem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbitem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbitem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance62).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance63).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance63).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance63).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbitem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbitem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbitem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbitem).Location = new System.Drawing.Point(427, 70);
		((System.Windows.Forms.Control)(object)this.cmbitem).Name = "cmbitem";
		((System.Windows.Forms.Control)(object)this.cmbitem).Size = new System.Drawing.Size(225, 26);
		((System.Windows.Forms.Control)(object)this.cmbitem).TabIndex = 8;
		this.cmbJoborder.AllowNull = (DefaultableBoolean)1;
		this.cmbJoborder.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbJoborder.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbJoborder.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbJoborder).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbJoborder).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbJoborder).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbJoborder).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJoborder).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.cmbJoborder).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJoborder).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbJoborder).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbJoborder).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance99).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbJoborder).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbJoborder).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbJoborder).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbJoborder).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJoborder).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbJoborder).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbJoborder).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbJoborder).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJoborder).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance118).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbJoborder).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbJoborder).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbJoborder).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbJoborder).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbJoborder).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbJoborder).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.cmbJoborder).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbJoborder).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbJoborder).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbJoborder).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbJoborder.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbJoborder).Location = new System.Drawing.Point(106, 70);
		((System.Windows.Forms.Control)(object)this.cmbJoborder).Name = "cmbJoborder";
		((System.Windows.Forms.Control)(object)this.cmbJoborder).Size = new System.Drawing.Size(225, 26);
		((System.Windows.Forms.Control)(object)this.cmbJoborder).TabIndex = 4;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(8, 73);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(59, 15);
		this.label4.TabIndex = 126;
		this.label4.Text = "Job Order";
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.rdFinancialYear);
		this.panel2.Controls.Add(this.rdThisYear);
		this.panel2.Controls.Add(this.rdThisMonth);
		this.panel2.Controls.Add(this.rdthisWeek);
		this.panel2.Controls.Add(this.rdThisDay);
		this.panel2.Controls.Add(this.label23);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1302, 28);
		this.panel2.TabIndex = 133;
		this.rdFinancialYear.AutoSize = true;
		this.rdFinancialYear.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdFinancialYear.ForeColor = System.Drawing.Color.White;
		this.rdFinancialYear.Location = new System.Drawing.Point(566, 4);
		this.rdFinancialYear.Name = "rdFinancialYear";
		this.rdFinancialYear.Size = new System.Drawing.Size(111, 21);
		this.rdFinancialYear.TabIndex = 3;
		this.rdFinancialYear.TabStop = true;
		this.rdFinancialYear.Text = "Financial Year";
		this.rdFinancialYear.UseVisualStyleBackColor = true;
		this.rdFinancialYear.Visible = false;
		this.rdThisYear.AutoSize = true;
		this.rdThisYear.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdThisYear.ForeColor = System.Drawing.Color.White;
		this.rdThisYear.Location = new System.Drawing.Point(478, 4);
		this.rdThisYear.Name = "rdThisYear";
		this.rdThisYear.Size = new System.Drawing.Size(82, 21);
		this.rdThisYear.TabIndex = 3;
		this.rdThisYear.TabStop = true;
		this.rdThisYear.Text = "This Year";
		this.rdThisYear.UseVisualStyleBackColor = true;
		this.rdThisYear.Visible = false;
		this.rdThisMonth.AutoSize = true;
		this.rdThisMonth.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdThisMonth.ForeColor = System.Drawing.Color.White;
		this.rdThisMonth.Location = new System.Drawing.Point(369, 4);
		this.rdThisMonth.Name = "rdThisMonth";
		this.rdThisMonth.Size = new System.Drawing.Size(97, 21);
		this.rdThisMonth.TabIndex = 3;
		this.rdThisMonth.TabStop = true;
		this.rdThisMonth.Text = "This Month";
		this.rdThisMonth.UseVisualStyleBackColor = true;
		this.rdThisMonth.Visible = false;
		this.rdthisWeek.AutoSize = true;
		this.rdthisWeek.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdthisWeek.ForeColor = System.Drawing.Color.White;
		this.rdthisWeek.Location = new System.Drawing.Point(274, 3);
		this.rdthisWeek.Name = "rdthisWeek";
		this.rdthisWeek.Size = new System.Drawing.Size(90, 21);
		this.rdthisWeek.TabIndex = 3;
		this.rdthisWeek.TabStop = true;
		this.rdthisWeek.Text = "This Week";
		this.rdthisWeek.UseVisualStyleBackColor = true;
		this.rdthisWeek.Visible = false;
		this.rdThisDay.AutoSize = true;
		this.rdThisDay.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdThisDay.ForeColor = System.Drawing.Color.White;
		this.rdThisDay.Location = new System.Drawing.Point(202, 3);
		this.rdThisDay.Name = "rdThisDay";
		this.rdThisDay.Size = new System.Drawing.Size(63, 21);
		this.rdThisDay.TabIndex = 3;
		this.rdThisDay.TabStop = true;
		this.rdThisDay.Text = "Today";
		this.rdThisDay.UseVisualStyleBackColor = true;
		this.rdThisDay.Visible = false;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(6, 3);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(171, 19);
		this.label23.TabIndex = 2;
		this.label23.Text = "Production Register";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 193);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1302, 28);
		this.panel3.TabIndex = 134;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1260, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 25);
		this.ctrlGrdBar1.TabIndex = 21;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grd.AllowEdit = (InheritableBoolean)2;
		this.grd.AlternatingColors = true;
		this.grd.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grd_DesignTimeLayout.LayoutString = resources.GetString("grd_DesignTimeLayout.LayoutString");
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		this.grd.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 221);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1302, 368);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 135;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(6, 3);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(67, 19);
		this.label1.TabIndex = 2;
		this.label1.Text = "History";
		this.CmbStockAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbStockAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbStockAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbStockAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbStockAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance43;
		((SpecialBoxBase)((UltraGridBase)this.CmbStockAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance49).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance50).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbStockAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbStockAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbStockAccount).Location = new System.Drawing.Point(740, 42);
		((System.Windows.Forms.Control)(object)this.CmbStockAccount).Name = "CmbStockAccount";
		((System.Windows.Forms.Control)(object)this.CmbStockAccount).Size = new System.Drawing.Size(176, 26);
		((System.Windows.Forms.Control)(object)this.CmbStockAccount).TabIndex = 11;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(656, 48);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(85, 15);
		this.label38.TabIndex = 602;
		this.label38.Text = "Stock Account";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1302, 589);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.toolStrip3);
		base.KeyPreview = true;
		base.Name = "ProductionRegister";
		this.Text = "ProductionRegister";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(ProductionRegister_FormClosed);
		base.Load += new System.EventHandler(ProductionRegister_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ProductionRegister_KeyDown);
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.SummaryInfoBox.ResumeLayout(false);
		this.SummaryInfoBox.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWarehouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbparentcategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbEntryType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWipAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbActivity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbplant).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJoborder).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStockAccount).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

}
