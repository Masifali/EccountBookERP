// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
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
namespace Architecture.WinApp.Sale
{

public class DeliveryOrderTransfer : Form
{
	private Attachment AT = new Attachment();

	private Rightsobjects formright = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private bool IsApproved;

	private DataTable table = new DataTable();

	private DataTable dthistoryfill = new DataTable();

	private DataTable BranchList = new DataTable();

	private DataTable dtitem = new DataTable();

	private bool SubsidiaryAccountAllownOnVouchers;

	private bool WarningMessageOnDOForPackingWeight;

	private bool MultiBranchFeatur;

	public int Id;

	public int DeliveryOrderFormId;

	private int updateDetailIndex = -1;

	private int DefaultDaysToLessFromHistoryFromDate;

	private string ScreenName = "";

	private List<InvDeliveryOrderdetail> lstRemoveRecord = new List<InvDeliveryOrderdetail>();

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

	private Label label13;

	private Label label18;

	private DateTimePicker DocDate;

	private TextBox txtdocno;

	private Label label5;

	private GroupBox groupBox3;

	private Button btnplus;

	private TextBox txtremarksdetail;

	private Label label47;

	private TextBox txtweight;

	private TextBox txtQty;

	private Label label35;

	private Label label39;

	private Label label38;

	private Label label36;

	private Label label37;

	private Panel panel7;

	private Panel panel8;

	internal UltraCombo CmbItemName;

	internal UltraCombo CmbPackUom;

	internal UltraCombo CmbPackingType;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	private Button btnUpdateDetail;

	private Button btnCancelUpdateDetial;

	private Panel panel12;

	internal GridEX grdhistory;

	private ToolStripButton btnRefresh;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewHistory;

	internal UltraCombo cmbBranchTo;

	internal UltraCombo cmbBranchFrom;

	internal UltraCombo CmbWareHouseFrom;

	private Label label3;

	private Label label4;

	internal UltraCombo CmbDeliveryOrderType;

	internal UltraCombo CmbJobLotFrom;

	private Label label8;

	private CheckBox ChkPrint;

	private TextBox txtGrossWeight;

	private Label label6;

	private TextBox txtPackTotal;

	private Label label1;

	internal UltraCombo CmbCropYear;

	private Label label7;

	private TextBox txtSubNoId;

	private TextBox txtDocNoId;

	private TextBox txtRefDocTypeId;

	private Label lblBalance;

	private TextBox txtVehicleNo;

	private ToolStripButton btnSaveAs;

	internal UltraCombo cmbvehicletype;

	private Label label14;

	private ToolStripButton btnDelete;

	private GroupBox groupBox1;

	internal GridEX grd;

	private Panel panel5;

	private Label label15;

	private Label label16;

	internal GridEX GrdHistoryDetail;

	private TabControl DetailGridPanel;

	private TabPage tabPage3;

	private Panel panel9;

	private Panel panel6;

	private Label label27;

	private GroupBox groupBox2;

	private Button btnShowHistory;

	private Label label51;

	private Label label52;

	private Label label53;

	private Label label54;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Label label19;

	private ToolStripButton btnRefreshHistory;

	private Panel panel13;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private RadioButton rdSearchByCode;

	private RadioButton rdSearchByName;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripButton BtnShortCutkeys;

	private TextBox txtPackUnit;

	private Label label20;

	internal UltraCombo CmbWarehouseTo;

	private Label label2;

	internal UltraCombo CmbJobLotTo;

	private Label label9;

	private Label label10;

	private TextBox txtremarks;

	public DeliveryOrderTransfer(UserAccount user)
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void DeliverOrder_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 650);
			ScreenName = Conversion.ToString(base.Tag);
			formright = CommonServices.SetRightsValueInRightsObject(ScreenName);
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnSaveAs.Enabled = formright.DoHaveSaveRight;
			btnupdate.Enabled = formright.DoHaveUpdateRights;
			btnDelete.Enabled = formright.DoHaveCanDelete;
			ChkPrint.Checked = formright.DoHavePrintRights;
			btnprint.Checked = formright.DoHavePrintRights;
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
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			MultiBranchFeatur = CommonServices.GetERPFeatureById(11);
			WarningMessageOnDOForPackingWeight = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("WarningMessageOnDOForPackingWeight"));
			if (MultiBranchFeatur)
			{
				CombBranchFill();
				label13.Visible = true;
				label18.Visible = true;
				((Control)(object)cmbBranchFrom).Visible = true;
				((Control)(object)cmbBranchTo).Visible = true;
			}
			else
			{
				label13.Visible = false;
				label18.Visible = false;
				((Control)(object)cmbBranchFrom).Visible = false;
				((Control)(object)cmbBranchTo).Visible = false;
			}
			if (dtitem.Columns.Count == 0)
			{
				dtitem.Columns.Add("Id", typeof(int));
				dtitem.Columns.Add("ItemName");
				dtitem.Columns.Add("ItemCode");
			}
			table.Columns.Add("Id", typeof(int));
			table.Columns.Add("WareHouseFromId", typeof(int));
			table.Columns.Add("WareHouseFrom", typeof(string));
			table.Columns.Add("WareHouseToId", typeof(int));
			table.Columns.Add("WareHouseTo", typeof(string));
			table.Columns.Add("ItemId", typeof(int));
			table.Columns.Add("ItemCode");
			table.Columns.Add("Item");
			table.Columns.Add("CropYearId");
			table.Columns.Add("CropYear");
			table.Columns.Add("JobLotFromId", typeof(int));
			table.Columns.Add("JobLotFrom", typeof(string));
			table.Columns.Add("JobLotToId", typeof(int));
			table.Columns.Add("JobLotTo", typeof(string));
			table.Columns.Add("PackingTypeId", typeof(int));
			table.Columns.Add("PackingType", typeof(string));
			table.Columns.Add("ItemUOMId", typeof(int));
			table.Columns.Add("ItemUOM");
			table.Columns.Add("PUomEquivalent", typeof(double));
			table.Columns.Add("QTY", typeof(double));
			table.Columns.Add("NetWeight", typeof(double));
			table.Columns.Add("EbUnit", typeof(double));
			table.Columns.Add("EbTotal", typeof(double));
			table.Columns.Add("GrossWeight", typeof(double));
			table.Columns.Add("Remarks");
			table.Columns.Add("AvailableStock", typeof(double));
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			DocumentNoFill();
			CmbDeliveryOrderTransferTypeFill();
			vehicleTypefill();
			if (MultiBranchFeatur)
			{
				WareHouseFillFrom(Conversion.ToInt(cmbBranchFrom.Value));
				WareHouseFillTo(Conversion.ToInt(cmbBranchTo.Value));
				JobLotFillFrom(Conversion.ToInt(cmbBranchFrom.Value));
				JobLotFillTo(Conversion.ToInt(cmbBranchTo.Value));
			}
			else
			{
				WareHouseFill();
				JobLotFill();
			}
			ItemDetailFillWithoutOrderId();
			CropYearFill();
			CmbPackingTypeFill();
			btnupdate.Visible = false;
			btnDelete.Visible = false;
			btnSaveAs.Visible = false;
			btnsave.Visible = true;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			cmbBranchFrom.Value = UserAccount.BranchesId;
			DocDate.Focus();
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			ToDateHistory.Value = DateTime.Now;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocumentNoFill()
	{
		try
		{
			int code = CommonServices.DeliveryOrderGenerateCode(84);
			if (code > 0)
			{
				txtdocno.Text = code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbDeliveryOrderTransferTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("OrderType");
			dt.Rows.Add(2, "StockTransfer");
			DDL.BindDDLNew(dt, CmbDeliveryOrderType, "Id", "OrderType", "OrderType", false);
			((UltraGridBase)CmbDeliveryOrderType).Rows[0].Activate();
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
			int ID = Conversion.ToInt(cmbvehicletype.Value);
			DataTable dt = VehicleType.GetAll();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbvehicletype, "Id", "VehicleDescription", "Vehicle Description", false);
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						cmbvehicletype.Value = ID;
					}
					else
					{
						((Control)(object)cmbvehicletype).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbvehicletype).Text = string.Empty;
				((UltraGridBase)cmbvehicletype).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CombBranchFill()
	{
		try
		{
			if (!MultiBranchFeatur)
			{
				return;
			}
			int Id1 = Conversion.ToInt(cmbBranchFrom.Value);
			int Id2 = Conversion.ToInt(cmbBranchTo.Value);
			BranchList = BranchesAllocationToUser.GetBranchsAllocatedToUser(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID);
			if (BranchList.Rows.Count > 0)
			{
				DDL.BindDDL(BranchList, cmbBranchFrom, "BranchId", "BranchName", "Branch Name", ZeroIndex: true);
				DDL.BindDDL(BranchList, cmbBranchTo, "BranchId", "BranchName", "Branch Name", ZeroIndex: true);
				((UltraGridBase)cmbBranchFrom).Rows[1].Activate();
				((UltraGridBase)cmbBranchTo).Rows[1].Activate();
				if (Id1 > 0)
				{
					if (BranchList.AsEnumerable().Any((DataRow row) => row.Field<int>("BranchId") == Id1))
					{
						cmbBranchFrom.Value = Id1;
					}
					else
					{
						((Control)(object)cmbBranchFrom).Text = string.Empty;
					}
				}
				if (Id2 > 0)
				{
					if (BranchList.AsEnumerable().Any((DataRow row) => row.Field<int>("BranchId") == Id2))
					{
						cmbBranchTo.Value = Id2;
					}
					else
					{
						((Control)(object)cmbBranchTo).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbBranchFrom).Text = string.Empty;
				((UltraGridBase)cmbBranchFrom).DataSource = null;
				((Control)(object)cmbBranchTo).Text = string.Empty;
				((UltraGridBase)cmbBranchTo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDetailFillWithoutOrderId()
	{
		try
		{
			int ID = Conversion.ToInt(CmbItemName.Value);
			dtitem.Rows.Clear();
			DataTable dtItemAll = CommonServices.ReadAllItems();
			if (dtItemAll.Rows.Count > 0)
			{
				for (int i = 0; i < dtItemAll.Rows.Count; i++)
				{
					dtitem.Rows.Add(dtItemAll.Rows[i]["Id"], dtItemAll.Rows[i]["ItemName"], dtItemAll.Rows[i]["ItemCodeNew"]);
				}
			}
			if (dtitem.Rows.Count > 0)
			{
				if (rdSearchByName.Checked)
				{
					DDL.BindDDL(dtitem, CmbItemName, "Id", "ItemName", "Item", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitem, CmbItemName, "Id", "ItemCode", "Item", ZeroIndex: true);
				}
				((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[1].Width = 300;
				if (ID > 0)
				{
					if (dtitem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbItemName.Value = ID;
					}
					else
					{
						((Control)(object)CmbItemName).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbItemName).Text = string.Empty;
				((UltraGridBase)CmbItemName).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CropYearFill()
	{
		try
		{
			string ID = Conversion.ToString((object)((Control)(object)CmbCropYear).Text);
			DataTable dtCrop = CommonServices.CropYearGetAllService();
			if (dtCrop.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCrop, CmbCropYear, "Id", "CropYear", "Crop Year", true);
				((UltraGridBase)CmbCropYear).Rows[0].Activate();
				if (!string.IsNullOrEmpty(ID))
				{
					if (dtCrop.AsEnumerable().Any((DataRow row) => row.Field<string>("CropYear") == ID))
					{
						((Control)(object)CmbCropYear).Text = ID;
					}
					else
					{
						((Control)(object)CmbCropYear).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbCropYear).Text = string.Empty;
				((UltraGridBase)CmbCropYear).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPackingTypeFill()
	{
		try
		{
			int ID = Conversion.ToInt(CmbPackingType.Value);
			DataTable dtPackingType = InvPackingType.Getall();
			if (dtPackingType.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtPackingType, CmbPackingType, "Id", "PackTypeDesc", "PackTypeDesc", true);
				if (ID > 0)
				{
					if (dtPackingType.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbPackingType.Value = ID;
					}
					else
					{
						((Control)(object)CmbPackingType).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbPackingType).Text = string.Empty;
				((UltraGridBase)CmbPackingType).DataSource = null;
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
			string Packuom = ((Control)(object)CmbPackUom).Text;
			((Control)(object)CmbPackUom).Text = string.Empty;
			DataTable dtuomsch = CommonServices.GetUomScheduleByItemId(ID);
			if (dtuomsch.Rows.Count > 0)
			{
				DDL.BindDDL(dtuomsch, CmbPackUom, "Id", "UOMCode", "PackUom", ZeroIndex: true);
				if (dtuomsch.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Packuom))
				{
					((Control)(object)CmbPackUom).Text = Packuom;
				}
				else
				{
					((Control)(object)CmbPackUom).Text = string.Empty;
				}
			}
			else
			{
				((Control)(object)CmbPackUom).Text = string.Empty;
				((UltraGridBase)CmbPackUom).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WareHouseFill()
	{
		try
		{
			int ID = Conversion.ToInt(CmbWareHouseFrom.Value);
			int ID_To = Conversion.ToInt(CmbWarehouseTo.Value);
			DataTable dtWh = CommonServices.getActiveWareHouse();
			if (dtWh.Rows.Count > 0)
			{
				DDL.BindDDL(dtWh, CmbWareHouseFrom, "Id", "WareHouseName", "WareHouseName", ZeroIndex: true);
				DDL.BindDDL(dtWh, CmbWarehouseTo, "Id", "WareHouseName", "WareHouseName", ZeroIndex: true);
				((UltraGridBase)CmbWareHouseFrom).Rows[1].Activate();
				((UltraGridBase)CmbWarehouseTo).Rows[1].Activate();
				if (ID > 0)
				{
					if (dtWh.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbWareHouseFrom.Value = ID;
					}
					else
					{
						((Control)(object)CmbWareHouseFrom).Text = string.Empty;
					}
				}
				if (ID_To > 0)
				{
					if (dtWh.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID_To))
					{
						CmbWarehouseTo.Value = ID_To;
					}
					else
					{
						((Control)(object)CmbWarehouseTo).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbWareHouseFrom).Text = string.Empty;
				((UltraGridBase)CmbWareHouseFrom).DataSource = null;
				((Control)(object)CmbWarehouseTo).Text = string.Empty;
				((UltraGridBase)CmbWarehouseTo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JobLotFill()
	{
		try
		{
			int ID = Conversion.ToInt(CmbJobLotFrom.Value);
			int ID_To = Conversion.ToInt(CmbJobLotTo.Value);
			DataTable dtJobLoat = CommonServices.JobLotGetAllService();
			if (dtJobLoat.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtJobLoat, CmbJobLotFrom, "Id", "JobLotDescription", "JobLotDescription", true);
				DDL.BindDDLNew(dtJobLoat, CmbJobLotTo, "Id", "JobLotDescription", "JobLotDescription", true);
				((UltraGridBase)CmbJobLotFrom).Rows[1].Activate();
				((UltraGridBase)CmbJobLotTo).Rows[1].Activate();
				if (ID > 0)
				{
					if (dtJobLoat.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbJobLotFrom.Value = ID;
					}
					else
					{
						((Control)(object)CmbJobLotFrom).Text = string.Empty;
					}
				}
				if (ID_To > 0)
				{
					if (dtJobLoat.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID_To))
					{
						CmbJobLotTo.Value = ID_To;
					}
					else
					{
						((Control)(object)CmbJobLotTo).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbJobLotFrom).Text = string.Empty;
				((UltraGridBase)CmbJobLotFrom).DataSource = null;
				((Control)(object)CmbJobLotTo).Text = string.Empty;
				((UltraGridBase)CmbJobLotTo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JobLotFillFrom(int BranchId)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		try
		{
			int ID = Conversion.ToInt(CmbJobLotFrom.Value);
			if (BranchId > 0)
			{
				DataTable dtJobLoat = JobLotsAllocationToBranch.GetJobLotsAllocatedToBranchByBranchId(new ReportsParameters
				{
					CompanyId = UserAccount.CompanyId,
					OrganizationId = UserAccount.OrganizationId,
					BranchIds = Conversion.ToString((object)BranchId)
				});
				if (dtJobLoat.Rows.Count > 0)
				{
					DDL.BindDDL(dtJobLoat, CmbJobLotFrom, "Id", "JobLotDescription", "JobLotFrom", ZeroIndex: true);
					((UltraGridBase)CmbJobLotFrom).Rows[1].Activate();
					if (ID > 0)
					{
						if (dtJobLoat.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
						{
							CmbJobLotFrom.Value = ID;
						}
						else
						{
							((Control)(object)CmbJobLotFrom).Text = string.Empty;
						}
					}
				}
				else
				{
					((Control)(object)CmbJobLotFrom).Text = string.Empty;
					((UltraGridBase)CmbJobLotFrom).DataSource = null;
				}
			}
			else
			{
				((Control)(object)CmbJobLotFrom).Text = string.Empty;
				((UltraGridBase)CmbJobLotFrom).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JobLotFillTo(int BranchId)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		try
		{
			int ID_To = Conversion.ToInt(CmbJobLotTo.Value);
			if (BranchId > 0)
			{
				DataTable dt = JobLotsAllocationToBranch.GetJobLotsAllocatedToBranchByBranchId(new ReportsParameters
				{
					CompanyId = UserAccount.CompanyId,
					OrganizationId = UserAccount.OrganizationId,
					BranchIds = Conversion.ToString((object)BranchId)
				});
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, CmbJobLotTo, "Id", "JobLotDescription", "JobLotTo", ZeroIndex: true);
					((UltraGridBase)CmbJobLotTo).Rows[1].Activate();
					if (ID_To > 0)
					{
						if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID_To))
						{
							CmbJobLotTo.Value = ID_To;
						}
						else
						{
							((Control)(object)CmbJobLotTo).Text = string.Empty;
						}
					}
				}
				else
				{
					((Control)(object)CmbJobLotTo).Text = string.Empty;
					((UltraGridBase)CmbJobLotTo).DataSource = null;
				}
			}
			else
			{
				((Control)(object)CmbJobLotTo).Text = string.Empty;
				((UltraGridBase)CmbJobLotTo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WareHouseFillFrom(int BranchId)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		try
		{
			int ID = Conversion.ToInt(CmbWareHouseFrom.Value);
			if (BranchId > 0)
			{
				DataTable dtWh = WarehousesAllocationToBranch.GetWarehousesAllocatedToBranchByBranchId(new ReportsParameters
				{
					CompanyId = UserAccount.CompanyId,
					OrganizationId = UserAccount.OrganizationId,
					BranchIds = Conversion.ToString((object)BranchId)
				});
				if (dtWh.Rows.Count > 0)
				{
					DDL.BindDDL(dtWh, CmbWareHouseFrom, "Id", "WareHouseName", "WareHouseName", ZeroIndex: true);
					((UltraGridBase)CmbWareHouseFrom).Rows[1].Activate();
					if (ID > 0)
					{
						if (dtWh.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
						{
							CmbWareHouseFrom.Value = ID;
						}
						else
						{
							((Control)(object)CmbWareHouseFrom).Text = string.Empty;
						}
					}
				}
				else
				{
					((Control)(object)CmbWareHouseFrom).Text = string.Empty;
					((UltraGridBase)CmbWareHouseFrom).DataSource = null;
				}
			}
			else
			{
				((Control)(object)CmbWareHouseFrom).Text = string.Empty;
				((UltraGridBase)CmbWareHouseFrom).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WareHouseFillTo(int BranchId)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		try
		{
			int ID_To = Conversion.ToInt(CmbWarehouseTo.Value);
			if (BranchId > 0)
			{
				DataTable dt = WarehousesAllocationToBranch.GetWarehousesAllocatedToBranchByBranchId(new ReportsParameters
				{
					CompanyId = UserAccount.CompanyId,
					OrganizationId = UserAccount.OrganizationId,
					BranchIds = Conversion.ToString((object)BranchId)
				});
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, CmbWarehouseTo, "Id", "WareHouseName", "WareHouseName", ZeroIndex: true);
					((UltraGridBase)CmbWarehouseTo).Rows[1].Activate();
					if (ID_To > 0)
					{
						if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID_To))
						{
							CmbWarehouseTo.Value = ID_To;
						}
						else
						{
							((Control)(object)CmbWarehouseTo).Text = string.Empty;
						}
					}
				}
				else
				{
					((Control)(object)CmbWarehouseTo).Text = string.Empty;
					((UltraGridBase)CmbWarehouseTo).DataSource = null;
				}
			}
			else
			{
				((Control)(object)CmbWarehouseTo).Text = string.Empty;
				((UltraGridBase)CmbWarehouseTo).DataSource = null;
			}
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
			bindRateUomAndItemPackUom(Conversion.ToInt(CmbItemName.Value));
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
				if (Conversion.ToInt(CmbItemName.Value) > 0)
				{
					ItemId = Conversion.ToInt(CmbItemName.Value);
				}
				if (rdSearchByName.Checked)
				{
					DDL.BindDDL(dtitem, CmbItemName, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitem, CmbItemName, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				CmbItemName.Value = ItemId;
				CmbItemName.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbBranchFrom_Leave(object sender, EventArgs e)
	{
		try
		{
			WareHouseFillFrom(Conversion.ToInt(cmbBranchFrom.Value));
			JobLotFillFrom(Conversion.ToInt(cmbBranchFrom.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbBranchTo_Leave(object sender, EventArgs e)
	{
		try
		{
			WareHouseFillTo(Conversion.ToInt(cmbBranchFrom.Value));
			JobLotFillTo(Conversion.ToInt(cmbBranchFrom.Value));
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
		if (((UltraGridBase)CmbDeliveryOrderType).ActiveRow == null || Conversion.ToInt(CmbDeliveryOrderType.Value) == 0)
		{
			MessageBox.Show("Do Type field is required");
			CmbDeliveryOrderType.Focus();
			return false;
		}
		if (MultiBranchFeatur)
		{
			if (((UltraGridBase)cmbBranchFrom).ActiveRow == null || Conversion.ToInt(cmbBranchFrom.Value) == 0)
			{
				MessageBox.Show("Branch From field is required");
				cmbBranchFrom.Focus();
				return false;
			}
			if (((UltraGridBase)cmbBranchTo).ActiveRow == null || Conversion.ToInt(cmbBranchTo.Value) == 0)
			{
				MessageBox.Show("Branch To field is required");
				cmbBranchTo.Focus();
				return false;
			}
			return true;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)CmbWareHouseFrom).ActiveRow == null || Conversion.ToInt(CmbWareHouseFrom.Value) == 0)
		{
			MessageBox.Show("WareHouse From field is required");
			CmbWareHouseFrom.Focus();
			return false;
		}
		if (((UltraGridBase)CmbWarehouseTo).ActiveRow == null || Conversion.ToInt(CmbWarehouseTo.Value) == 0)
		{
			MessageBox.Show("WareHouse To field is required");
			CmbWarehouseTo.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbWareHouseFrom.Value) == Conversion.ToInt(CmbWarehouseTo.Value))
		{
			MessageBox.Show(" 'WareHouse From' Cannot Be Equal To ' Warehouse To' ...");
			return false;
		}
		if (((UltraGridBase)CmbItemName).ActiveRow == null || Conversion.ToInt(CmbItemName.Value) == 0)
		{
			MessageBox.Show("ItemName field is required");
			CmbItemName.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCropYear).ActiveRow == null || Conversion.ToInt(CmbCropYear.Value) == 0)
		{
			MessageBox.Show("CropYear field is required");
			CmbCropYear.Focus();
			return false;
		}
		if (((UltraGridBase)CmbJobLotFrom).ActiveRow == null || Conversion.ToInt(CmbJobLotFrom.Value) == 0)
		{
			MessageBox.Show("JobLot From field is required");
			CmbJobLotFrom.Focus();
			return false;
		}
		if (((UltraGridBase)CmbJobLotTo).ActiveRow == null || Conversion.ToInt(CmbJobLotTo.Value) == 0)
		{
			MessageBox.Show("JobLot To field is required");
			CmbJobLotTo.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackingType).ActiveRow == null || Conversion.ToInt(CmbPackingType.Value) == 0)
		{
			MessageBox.Show("Packing type field is required");
			CmbPackingType.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackUom).ActiveRow == null || Conversion.ToInt(CmbPackUom.Value) == 0)
		{
			MessageBox.Show("PackUOM field is required");
			CmbPackUom.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtQty.Text) == 0.0)
		{
			MessageBox.Show("ItemQty field is required");
			txtQty.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtweight.Text) == 0.0)
		{
			MessageBox.Show("Net Weight field is required");
			txtweight.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtGrossWeight.Text) == 0.0)
		{
			MessageBox.Show("GrossWeight field is required");
			txtGrossWeight.Focus();
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
				table.Rows.Add(0, Conversion.ToInt(CmbWareHouseFrom.Value), Conversion.ToString((object)((Control)(object)CmbWareHouseFrom).Text), Conversion.ToInt(CmbWarehouseTo.Value), Conversion.ToString((object)((Control)(object)CmbWarehouseTo).Text), Conversion.ToInt(CmbItemName.Value), Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemCode"].Value), Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemName"].Value), Conversion.ToInt(CmbCropYear.Value), Conversion.ToString((object)((Control)(object)CmbCropYear).Text), Conversion.ToInt(CmbJobLotFrom.Value), Conversion.ToString((object)((Control)(object)CmbJobLotFrom).Text), Conversion.ToInt(CmbJobLotTo.Value), Conversion.ToString((object)((Control)(object)CmbJobLotTo).Text), Conversion.ToInt(CmbPackingType.Value), Conversion.ToString((object)((Control)(object)CmbPackingType).Text), Conversion.ToInt(CmbPackUom.Value), Conversion.ToString((object)((Control)(object)CmbPackUom).Text), Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value), Conversion.ToDouble((object)txtQty.Text.Trim()), Conversion.ToDouble((object)txtweight.Text.Trim()), Conversion.ToDouble((object)txtPackUnit.Text.Trim()), Conversion.ToDouble((object)txtPackTotal.Text.Trim()), Conversion.ToDouble((object)txtGrossWeight.Text.Trim()), txtremarksdetail.Text.Trim(), 0);
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

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				updateDetailIndex = item.RowIndex;
				CmbWareHouseFrom.Value = item.Cells["WareHouseFromId"].Value.ToString();
				CmbWarehouseTo.Value = item.Cells["WareHouseToId"].Value.ToString();
				CmbItemName.Value = item.Cells["ItemId"].Value.ToString();
				bindRateUomAndItemPackUom(Conversion.ToInt(CmbItemName.Value));
				CmbCropYear.Value = item.Cells["CropYearId"].Value.ToString();
				CmbJobLotFrom.Value = item.Cells["JobLotFromId"].Value.ToString();
				CmbJobLotTo.Value = item.Cells["JobLotToId"].Value.ToString();
				CmbPackingType.Value = item.Cells["PackingTypeId"].Value.ToString();
				CmbPackUom.Value = item.Cells["ItemUOMId"].Value.ToString();
				((Control)(object)CmbPackUom).Text = item.Cells["ItemUOM"].Text.ToString();
				txtQty.Text = item.Cells["QTY"].Value.ToString();
				txtweight.Text = item.Cells["NetWeight"].Value.ToString();
				txtPackUnit.Text = item.Cells["EbUnit"].Value.ToString();
				txtPackTotal.Text = item.Cells["EbTotal"].Value.ToString();
				txtGrossWeight.Text = item.Cells["GrossWeight"].Value.ToString();
				txtremarksdetail.Text = item.Cells["Remarks"].Value.ToString();
				btnplus.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
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
				table.Rows[updateDetailIndex]["WareHouseFromId"] = Conversion.ToInt(CmbWareHouseFrom.Value);
				table.Rows[updateDetailIndex]["WareHouseFrom"] = Conversion.ToString((object)((Control)(object)CmbWareHouseFrom).Text);
				table.Rows[updateDetailIndex]["WareHouseToId"] = Conversion.ToInt(CmbWarehouseTo.Value);
				table.Rows[updateDetailIndex]["WareHouseTo"] = Conversion.ToString((object)((Control)(object)CmbWarehouseTo).Text);
				table.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(CmbItemName.Value);
				table.Rows[updateDetailIndex]["ItemCode"] = Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemCode"].Value);
				table.Rows[updateDetailIndex]["Item"] = Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemName"].Value);
				table.Rows[updateDetailIndex]["CropYearId"] = Conversion.ToString(CmbCropYear.Value);
				table.Rows[updateDetailIndex]["CropYear"] = Conversion.ToString((object)((Control)(object)CmbCropYear).Text);
				table.Rows[updateDetailIndex]["JobLotFromId"] = Conversion.ToInt(CmbJobLotFrom.Value);
				table.Rows[updateDetailIndex]["JobLotFrom"] = Conversion.ToString((object)((Control)(object)CmbJobLotFrom).Text);
				table.Rows[updateDetailIndex]["JobLotToId"] = Conversion.ToInt(CmbJobLotTo.Value);
				table.Rows[updateDetailIndex]["JobLotTo"] = Conversion.ToString((object)((Control)(object)CmbJobLotTo).Text);
				table.Rows[updateDetailIndex]["PackingTypeId"] = Conversion.ToInt(CmbPackingType.Value);
				table.Rows[updateDetailIndex]["PackingType"] = Conversion.ToString((object)((Control)(object)CmbPackingType).Text);
				table.Rows[updateDetailIndex]["ItemUOMId"] = Conversion.ToInt(CmbPackUom.Value);
				table.Rows[updateDetailIndex]["ItemUOM"] = Conversion.ToString((object)((Control)(object)CmbPackUom).Text);
				table.Rows[updateDetailIndex]["PUomEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells["Equivalent"].Value);
				table.Rows[updateDetailIndex]["QTY"] = Conversion.ToDouble((object)txtQty.Text);
				table.Rows[updateDetailIndex]["NetWeight"] = Conversion.ToDouble((object)txtweight.Text);
				table.Rows[updateDetailIndex]["EbUnit"] = Conversion.ToDouble((object)txtPackUnit.Text);
				table.Rows[updateDetailIndex]["EbTotal"] = Conversion.ToDouble((object)txtPackTotal.Text);
				table.Rows[updateDetailIndex]["GrossWeight"] = Conversion.ToDouble((object)txtGrossWeight.Text);
				table.Rows[updateDetailIndex]["Remarks"] = Conversion.ToString((object)txtremarksdetail.Text);
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

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		txtRefDocTypeId.Text = string.Empty;
		txtDocNoId.Text = string.Empty;
		txtSubNoId.Text = string.Empty;
		btnplus.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelUpdateDetial.Visible = false;
		updateDetailIndex = -1;
	}

	private void grdSettings()
	{
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["WareHouseFromId"].Visible = false;
			grd.RootTable.Columns["WareHouseToId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["CropYearId"].Visible = false;
			grd.RootTable.Columns["JobLotFromId"].Visible = false;
			grd.RootTable.Columns["JobLotToId"].Visible = false;
			grd.RootTable.Columns["PackingTypeId"].Visible = false;
			grd.RootTable.Columns["ItemUOMId"].Visible = false;
			grd.RootTable.Columns["PUomEquivalent"].Visible = false;
			grd.RootTable.Columns["AvailableStock"].Visible = false;
			grd.RootTable.Columns["ItemUOM"].Caption = "Pack Uom";
			grd.RootTable.Columns["Remarks"].Caption = "Comments";
			grd.RootTable.Columns["Id"].EditType = (EditType)0;
			grd.RootTable.Columns["AvailableStock"].EditType = (EditType)0;
			grd.RootTable.Columns["WareHouseFromId"].EditType = (EditType)0;
			grd.RootTable.Columns["WareHouseFrom"].EditType = (EditType)0;
			grd.RootTable.Columns["WareHouseToId"].EditType = (EditType)0;
			grd.RootTable.Columns["WareHouseTo"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			grd.RootTable.Columns["Item"].EditType = (EditType)0;
			grd.RootTable.Columns["CropYearId"].EditType = (EditType)0;
			grd.RootTable.Columns["CropYear"].EditType = (EditType)0;
			grd.RootTable.Columns["JobLotFromId"].EditType = (EditType)0;
			grd.RootTable.Columns["JobLotFrom"].EditType = (EditType)0;
			grd.RootTable.Columns["JobLotToId"].EditType = (EditType)0;
			grd.RootTable.Columns["JobLotTo"].EditType = (EditType)0;
			grd.RootTable.Columns["PackingTypeId"].EditType = (EditType)0;
			grd.RootTable.Columns["PackingType"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemUOMId"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemUOM"].EditType = (EditType)0;
			grd.RootTable.Columns["PUomEquivalent"].EditType = (EditType)0;
			grd.RootTable.Columns["QTY"].EditType = (EditType)0;
			grd.RootTable.Columns["NetWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["EbUnit"].EditType = (EditType)0;
			grd.RootTable.Columns["EbTotal"].EditType = (EditType)0;
			grd.RootTable.Columns["GrossWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["AvailableStock"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["AvailableStock"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AvailableStock"].FormatString = "#,##0.###";
			grd.RootTable.Columns["QTY"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["QTY"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["QTY"].FormatString = "#,##0.###";
			grd.RootTable.Columns["QTY"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NetWeight"].FormatString = "#,##0.###";
			grd.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["EbUnit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["EbUnit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["EbUnit"].FormatString = "#,##0.###";
			grd.RootTable.Columns["EbUnit"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["EbTotal"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["EbTotal"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["EbTotal"].FormatString = "#,##0.####";
			grd.RootTable.Columns["EbTotal"].TotalFormatString = "#,##0.####";
			grd.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["GrossWeight"].FormatString = "#,##0.###";
			grd.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(grd);
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "X";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
			grd.RootTable.Columns["Delete"].Width = 20;
			grd.RootTable.Columns["Delete"].Position = 1;
			grd.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
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
			if (r != null && (int)r.RowType == 0 && e.Column.Key == "Delete")
			{
				DeleteDetailRow(r);
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
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
					{
						InvDeliveryOrderdetail vd = new InvDeliveryOrderdetail();
						vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
						vd.WarehouseId = Conversion.ToInt(r.Cells["WareHouseFromId"].Value);
						vd.WareHouseToId = Conversion.ToInt(r.Cells["WareHouseToId"].Value);
						vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						vd.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
						vd.JobLotId = Conversion.ToInt(r.Cells["JobLotFromId"].Value);
						vd.ToJobLotId = Conversion.ToInt(r.Cells["JobLotToId"].Value);
						vd.InvPackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
						vd.PackUomId = Conversion.ToInt(r.Cells["ItemUOMId"].Value);
						vd.DoQty = Conversion.ToDouble(r.Cells["QTY"].Value);
						vd.LoadingQty = Conversion.ToDouble(r.Cells["QTY"].Value);
						vd.DoWeight = Conversion.ToDouble(r.Cells["NetWeight"].Value);
						vd.LoadingWeight = Conversion.ToDouble(r.Cells["NetWeight"].Value);
						vd.PackingWeight = Conversion.ToDouble(r.Cells["EbUnit"].Value);
						vd.OuterEbTotal = Conversion.ToDouble(r.Cells["EbTotal"].Value);
						vd.GrossWeight = Conversion.ToDouble(r.Cells["GrossWeight"].Value);
						vd.LoadingRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
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
			throw new Exception("Reset Detail First");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.UpdateData();
			GridEXRow item = grd.CurrentRow;
			if (item != null)
			{
				_ = item.RowType;
			}
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Expected O, but got Unknown
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
			if (MultiBranchFeatur && Conversion.ToInt(CmbDeliveryOrderType.Value) == 2 && Conversion.ToInt(cmbBranchFrom.Value) == Conversion.ToInt(cmbBranchTo.Value))
			{
				MessageBox.Show(" 'Branch From' Cannot Be Equal To ' Branch To' ...");
				return;
			}
			InvDeliveryOrder po = new InvDeliveryOrder();
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				po.Id = Id;
				po.ModifyUser = UserAccount.ID;
				po.IsApproved = IsApproved;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			po.OrganizationId = UserAccount.OrganizationId;
			po.CompanyId = UserAccount.CompanyId;
			po.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			po.BranchesId = UserAccount.BranchesId;
			po.DocumentTypeId = 84;
			po.ScreenName = ScreenName;
			po.EntryUser = UserAccount.ID;
			po.ModifyUser = UserAccount.ID;
			po.ApprovedUser = UserAccount.ID;
			po.EntryDate = DateTime.Now;
			po.ModifyDate = DateTime.Now;
			po.ApprovedDate = DateTime.Now;
			po.DocNo = Conversion.ToInt((object)txtdocno.Text);
			po.DocDate = DocDate.Value;
			po.FromBranchId = Conversion.ToInt(cmbBranchFrom.Value);
			po.ToBranchId = Conversion.ToInt(cmbBranchTo.Value);
			po.DeliveryOrderType = Conversion.ToString((object)((Control)(object)CmbDeliveryOrderType).Text);
			po.VehicleType = Conversion.ToString((object)((Control)(object)cmbvehicletype).Text);
			po.VehicleNo = Conversion.ToString((object)txtVehicleNo.Text.Trim());
			po.LoadingInstructions = Conversion.ToString((object)txtremarks.Text);
			po.InvDeliveryOrderDetailList = new List<InvDeliveryOrderdetail>();
			po.InvDeliveryOrderExpenseslist = new List<InvDeliveryOrderExpense>();
			double NetWeight = 0.0;
			double GrossWeight = 0.0;
			if (Id > 0 && lstRemoveRecord.Count > 0)
			{
				for (int i = 0; i < lstRemoveRecord.Count; i++)
				{
					po.InvDeliveryOrderDetailList.Add(lstRemoveRecord[i]);
				}
			}
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				InvDeliveryOrderdetail vd = new InvDeliveryOrderdetail();
				if (Id == 0)
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
				if (Conversion.ToInt(r.Cells["WareHouseFromId"].Value) == 0)
				{
					throw new Exception($"WareHouseFrom Field Required in Detail Row No : {r.RowIndex + 1}");
				}
				if (Conversion.ToInt(r.Cells["WareHouseToId"].Value) == 0)
				{
					throw new Exception($"WareHouseTo Field Required in Detail Row No : {r.RowIndex + 1}");
				}
				if (Conversion.ToInt(r.Cells["ItemId"].Value) == 0)
				{
					throw new Exception($"Item Name Field Required in Detail Row No : {r.RowIndex + 1}");
				}
				if (Conversion.ToInt(r.Cells["CropYearId"].Value) == 0)
				{
					throw new Exception($"CropYear Name Field Required in Detail Row No : {r.RowIndex + 1}");
				}
				if (Conversion.ToInt(r.Cells["JobLotFromId"].Value) == 0)
				{
					throw new Exception($"JobLotFrom Field Required in Detail Row No : {r.RowIndex + 1}");
				}
				if (Conversion.ToInt(r.Cells["JobLotToId"].Value) == 0)
				{
					throw new Exception($"JobLotTo Field Required in Detail Row No : {r.RowIndex + 1}");
				}
				if (Conversion.ToInt(r.Cells["PackingTypeId"].Value) == 0)
				{
					throw new Exception($"PackingType Field Required in Detail Row No : {r.RowIndex + 1}");
				}
				if (Conversion.ToInt(r.Cells["ItemUOMId"].Value) == 0)
				{
					throw new Exception($"PackUOM Field Required in Detail Row No : {r.RowIndex + 1}");
				}
				if (Conversion.ToDouble(r.Cells["QTY"].Value) == 0.0)
				{
					throw new Exception($"QTY Field Required in Detail Row No : {r.RowIndex + 1}");
				}
				if (Conversion.ToDouble(r.Cells["NetWeight"].Value) == 0.0)
				{
					throw new Exception($"NetWeight Field Required in Detail Row No : {r.RowIndex + 1}");
				}
				if (Conversion.ToDouble(r.Cells["GrossWeight"].Value) == 0.0)
				{
					throw new Exception($"GrossWeight Field Required in Detail Row No : {r.RowIndex + 1}");
				}
				vd.WarehouseId = Conversion.ToInt(r.Cells["WareHouseFromId"].Value);
				vd.WareHouseToId = Conversion.ToInt(r.Cells["WareHouseToId"].Value);
				vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				vd.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
				vd.JobLotId = Conversion.ToInt(r.Cells["JobLotFromId"].Value);
				vd.ToJobLotId = Conversion.ToInt(r.Cells["JobLotToId"].Value);
				vd.InvPackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
				vd.PackUomId = Conversion.ToInt(r.Cells["ItemUOMId"].Value);
				vd.DoQty = Conversion.ToDouble(r.Cells["QTY"].Value);
				vd.LoadingQty = Conversion.ToDouble(r.Cells["QTY"].Value);
				vd.DoWeight = Conversion.ToDouble(r.Cells["NetWeight"].Value);
				vd.LoadingWeight = Conversion.ToDouble(r.Cells["NetWeight"].Value);
				vd.PackingWeight = Conversion.ToDouble(r.Cells["EbUnit"].Value);
				vd.OuterEbTotal = Conversion.ToDouble(r.Cells["EbTotal"].Value);
				vd.TotalPackingWeight = Conversion.ToDouble(r.Cells["EbTotal"].Value);
				vd.GrossWeight = Conversion.ToDouble(r.Cells["GrossWeight"].Value);
				GrossWeight += vd.GrossWeight;
				NetWeight += vd.LoadingWeight;
				vd.LoadingRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
				po.InvDeliveryOrderDetailList.Add(vd);
			}
			po.GrossWeight = GrossWeight;
			po.NetWeight = NetWeight;
			string WarningMessage = "";
			WarningMessage = InvDeliveryOrder.DeliveryOrderStackWarningMessage(po);
			if (WarningMessage != "" && MessageBox.Show(WarningMessage, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
			{
				return;
			}
			po.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			po.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<InvDeliveryOrder>(po, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			int success = InvDeliveryOrder.Save(po);
			if (Id > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + po.DocNo);
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
				{
					global::Architecture.BLL.DMSAttachments.RemoveByIdAndNames(Id, base.Name, 84, 0);
				}
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + po.DocNo);
			}
			Reset();
			if (ChkPrint.Checked)
			{
				CommonServices.InvDeliveryOrderSlip(success, 84);
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
			Id = 0;
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
			Id = 0;
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
			if (Id == 0)
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

	private void btnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (Id == 0)
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
					CommonServices.RemoveByID(84, Id);
					MessageBox.Show("Record Deleted Successfully");
					Reset();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void ReadById(int ID)
	{
		try
		{
			Id = ID;
			InvDeliveryOrder po = InvDeliveryOrder.GetByID(ID);
			if (po != null && po.InvDeliveryOrderDetailList.Count > 0)
			{
				tabControl1.SelectedIndex = 0;
				((Control)(object)CmbDeliveryOrderType).Enabled = false;
				DocDate.Value = po.DocDate;
				txtdocno.Text = Conversion.ToString((object)po.DocNo);
				((Control)(object)cmbvehicletype).Text = Conversion.ToString((object)po.VehicleType);
				txtVehicleNo.Text = po.VehicleNo;
				cmbBranchFrom.Value = Conversion.ToInt((object)po.FromBranchId);
				cmbBranchTo.Value = Conversion.ToInt((object)po.ToBranchId);
				txtremarks.Text = Conversion.ToString((object)po.LoadingInstructions);
				IsApproved = po.IsApproved;
				table.Clear();
				for (int i = 0; i < po.InvDeliveryOrderDetailList.Count; i++)
				{
					table.Rows.Add(po.InvDeliveryOrderDetailList[i].Id, po.InvDeliveryOrderDetailList[i].WarehouseId, po.InvDeliveryOrderDetailList[i].WareHouseName, po.InvDeliveryOrderDetailList[i].WareHouseToId, po.InvDeliveryOrderDetailList[i].WareHouseToName, po.InvDeliveryOrderDetailList[i].ItemId, po.InvDeliveryOrderDetailList[i].ItemCode, po.InvDeliveryOrderDetailList[i].ItemName, po.InvDeliveryOrderDetailList[i].CropYearId, po.InvDeliveryOrderDetailList[i].CropYear, po.InvDeliveryOrderDetailList[i].JobLotId, po.InvDeliveryOrderDetailList[i].JobLotDescription, po.InvDeliveryOrderDetailList[i].ToJobLotId, po.InvDeliveryOrderDetailList[i].ToJobLot, po.InvDeliveryOrderDetailList[i].InvPackingTypeId, po.InvDeliveryOrderDetailList[i].PackTypeDesc, po.InvDeliveryOrderDetailList[i].PackUomId, po.InvDeliveryOrderDetailList[i].PackUOM, po.InvDeliveryOrderDetailList[i].PUomEquivalent, po.InvDeliveryOrderDetailList[i].DoQty, po.InvDeliveryOrderDetailList[i].DoWeight, po.InvDeliveryOrderDetailList[i].PackingWeight, po.InvDeliveryOrderDetailList[i].OuterEbTotal, po.InvDeliveryOrderDetailList[i].GrossWeight, po.InvDeliveryOrderDetailList[i].LoadingRemarks);
				}
				grd.DataSource = table;
				grd.RetrieveStructure();
				grdSettings();
				FormHelper.LoadAttachmentsForObject(AT, Id, base.Name);
				btnsave.Visible = false;
				btnSaveAs.Visible = false;
				btnupdate.Visible = true;
				btnDelete.Visible = true;
			}
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
			vehicleTypefill();
			CombBranchFill();
			ItemDetailFillWithoutOrderId();
			bindRateUomAndItemPackUom(Conversion.ToInt(CmbItemName.Value));
			CropYearFill();
			CmbPackingTypeFill();
			if (MultiBranchFeatur)
			{
				WareHouseFillFrom(Conversion.ToInt(cmbBranchFrom.Value));
				WareHouseFillTo(Conversion.ToInt(cmbBranchTo.Value));
				JobLotFillFrom(Conversion.ToInt(cmbBranchFrom.Value));
				JobLotFillTo(Conversion.ToInt(cmbBranchTo.Value));
			}
			else
			{
				WareHouseFill();
				JobLotFill();
			}
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
			Id = 0;
			((Control)(object)CmbDeliveryOrderType).Enabled = false;
			IsApproved = false;
			DocDate.Focus();
			((Control)(object)CmbItemName).Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			txtQty.Text = string.Empty;
			txtweight.Text = string.Empty;
			((Control)(object)CmbPackingType).Text = string.Empty;
			txtremarks.Text = string.Empty;
			txtRefDocTypeId.Text = string.Empty;
			txtDocNoId.Text = string.Empty;
			txtSubNoId.Text = string.Empty;
			txtVehicleNo.Text = string.Empty;
			((Control)(object)cmbvehicletype).Text = string.Empty;
			txtPackUnit.Text = "0";
			txtPackTotal.Text = "0";
			txtGrossWeight.Text = "0";
			btnsave.Visible = true;
			btnupdate.Visible = false;
			btnDelete.Visible = false;
			btnSaveAs.Visible = false;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			table.Rows.Clear();
			grd.DataSource = table;
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
			txtRefDocTypeId.Text = string.Empty;
			txtDocNoId.Text = string.Empty;
			txtSubNoId.Text = string.Empty;
			((Control)(object)CmbItemName).Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			txtQty.Text = string.Empty;
			txtweight.Text = string.Empty;
			txtPackUnit.Text = "0";
			txtPackTotal.Text = "0";
			txtGrossWeight.Text = "0";
			txtremarksdetail.Text = string.Empty;
			((Control)(object)CmbWareHouseFrom).Text = string.Empty;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			updateDetailIndex = -1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeliveryOrderTransfer_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnResetHistory_Click(object sender, EventArgs e)
	{
		try
		{
			ResetHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetHistory()
	{
		try
		{
			FromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			ToDateHistory.Value = DateTime.Now;
			FromDocNoHistory.Clear();
			ToDocNoHistory.Clear();
			grdhistory.DataSource = null;
			GrdHistoryDetail.DataSource = null;
			drdocdate.Checked = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
	}

	private void btnShowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			gridhistoryfill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			reports.DocumentTypeId = 84;
			reports.Activity = "StockTransfer";
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
			dthistoryfill = InvDeliveryOrder.FormHistoryNew(reports);
			if (dthistoryfill.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DoType");
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("VehicleType");
				dt.Columns.Add("VehicleNo");
				dt.Columns.Add("FromBranch");
				dt.Columns.Add("ToBranch");
				dt.Columns.Add("GpDate", typeof(DateTime));
				dt.Columns.Add("GpNo", typeof(int));
				dt.Columns.Add("FactoryWeight", typeof(double));
				dt.Columns.Add("EntryUser");
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser");
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ApprovalStatus");
				dt.Columns.Add("ApprovedUser");
				dt.Columns.Add("ApprovedDate", typeof(DateTime));
				dt.Columns.Add("OrderStatus");
				dt.Columns.Add("Remarks");
				dt.Columns.Add("NoOfAttachments");
				int i;
				for (i = 0; i < dthistoryfill.Rows.Count; i++)
				{
					if (!dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Conversion.ToInt(dthistoryfill.Rows[i]["Id"])))
					{
						dt.Rows.Add(dthistoryfill.Rows[i]["Id"], dthistoryfill.Rows[i]["DeliveryOrderType"], Conversion.ToDateTime(dthistoryfill.Rows[i]["DocDate"]).ToShortDateString(), dthistoryfill.Rows[i]["DocNo"], dthistoryfill.Rows[i]["VehicleType"], dthistoryfill.Rows[i]["VehicleNo"], dthistoryfill.Rows[i]["FromBranchName"], dthistoryfill.Rows[i]["ToBranchName"], Conversion.ToDateTime(dthistoryfill.Rows[i]["GpDate"]).ToShortDateString(), Conversion.ToInt(dthistoryfill.Rows[i]["GpNo"]), Conversion.ToDouble(dthistoryfill.Rows[i]["FactoryWeight"]), dthistoryfill.Rows[i]["EntryUser"], dthistoryfill.Rows[i]["EntryDate"], dthistoryfill.Rows[i]["ModifyUser"], dthistoryfill.Rows[i]["ModifyDate"], dthistoryfill.Rows[i]["ApprovalStatus"], dthistoryfill.Rows[i]["ApprovedUser"], dthistoryfill.Rows[i]["ApprovedDate"], dthistoryfill.Rows[i]["OrderStatus"], dthistoryfill.Rows[i]["HeaderRemarks"], dthistoryfill.Rows[i]["NoOfAttachments"]);
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
			grdhistory.RootTable.Columns["FromBranch"].Visible = MultiBranchFeatur;
			grdhistory.RootTable.Columns["ToBranch"].Visible = MultiBranchFeatur;
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdhistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["FactoryWeight"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["FactoryWeight"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["FactoryWeight"].FormatString = "#,###.##";
			grdhistory.RootTable.Columns["FactoryWeight"].TotalFormatString = "#,###.##";
			CommonServices.GridAutoAdjustmentNew(grdhistory);
			grdhistory.RootTable.Columns.Add("Edit");
			grdhistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdhistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdhistory.RootTable.Columns["Edit"].Width = 50;
			grdhistory.RootTable.Columns["Edit"].Position = 0;
			grdhistory.RootTable.Columns.Add("Print");
			grdhistory.RootTable.Columns["Print"].Caption = "Print";
			grdhistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Print"].ButtonText = "Print";
			grdhistory.RootTable.Columns["Print"].Width = 50;
			grdhistory.RootTable.Columns["Print"].Position = 1;
			grdhistory.RootTable.Columns.Add("SaveAs");
			grdhistory.RootTable.Columns["SaveAs"].Caption = "SaveAs";
			grdhistory.RootTable.Columns["SaveAs"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["SaveAs"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["SaveAs"].ButtonText = "SaveAs";
			grdhistory.RootTable.Columns["SaveAs"].Width = 70;
			grdhistory.RootTable.Columns["SaveAs"].Position = 2;
			grdhistory.FrozenColumns = 3;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
				dt.Columns.Add("WareHouseFrom");
				dt.Columns.Add("WareHouseTo");
				dt.Columns.Add("ItemCode");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("CropYear");
				dt.Columns.Add("JobLotFrom");
				dt.Columns.Add("JobLotTo");
				dt.Columns.Add("PackingType");
				dt.Columns.Add("PackUOM");
				dt.Columns.Add("QTY", typeof(double));
				dt.Columns.Add("NetWeight", typeof(double));
				dt.Columns.Add("EbUnit", typeof(double));
				dt.Columns.Add("EbTotal", typeof(double));
				dt.Columns.Add("GrossWeight", typeof(double));
				dt.Columns.Add("Remarks");
				DataTable resultRows = (from row in dthistoryfill.AsEnumerable()
					where row.Field<int>("Id") == Conversion.ToInt(item.Cells["Id"].Value)
					select row).CopyToDataTable();
				for (int i = 0; i < resultRows.Rows.Count; i++)
				{
					dt.Rows.Add(resultRows.Rows[i]["WareHouseName"], resultRows.Rows[i]["WareHouseToName"], resultRows.Rows[i]["ItemCode"], resultRows.Rows[i]["ItemName"], resultRows.Rows[i]["CropYear"], resultRows.Rows[i]["JobLotDescription"], resultRows.Rows[i]["ToJobLot"], resultRows.Rows[i]["PackTypeDesc"], resultRows.Rows[i]["PackUOM"], resultRows.Rows[i]["LoadingQty"], resultRows.Rows[i]["LoadingWeight"], resultRows.Rows[i]["PackingWeight"], resultRows.Rows[i]["OuterEbTotal"], resultRows.Rows[i]["GrossWeight"], resultRows.Rows[i]["LoadingRemarks"]);
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
			GrdHistoryDetail.RootTable.Columns["Remarks"].Caption = "Comments";
			GrdHistoryDetail.RootTable.Columns["QTY"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["QTY"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["QTY"].FormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["QTY"].TotalFormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["NetWeight"].FormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["EbUnit"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["EbUnit"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["EbUnit"].FormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["EbUnit"].TotalFormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["EbTotal"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["EbTotal"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["EbTotal"].FormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["EbTotal"].TotalFormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["GrossWeight"].FormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustmentNew(GrdHistoryDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdhistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int id = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(id, 84);
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
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				Id = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				ReadById(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Expected O, but got Unknown
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			int Currid = Conversion.ToInt(r.Cells["Id"].Value);
			if (e.Column.Key == "SaveAs")
			{
				Reset();
				Id = Currid;
				ReadById(Id);
				btnsave.Visible = false;
				btnupdate.Visible = false;
				btnDelete.Visible = false;
				btnSaveAs.Visible = true;
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					obj.Cells["Id"].Value = 0;
					obj.EndEdit();
				}
			}
			if (e.Column.Key == "Edit")
			{
				string status = Conversion.ToString(grdhistory.CurrentRow.Cells["OrderStatus"].Value);
				if (!string.IsNullOrEmpty(status) && status != "Open")
				{
					MessageBox.Show("Sorry You can't Update this record because Order_Status is:" + status, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				Reset();
				Id = Currid;
				ReadById(Id);
			}
			if (e.Column.Key == "Print")
			{
				CommonServices.InvDeliveryOrderSlip(Currid, 84);
			}
			if (e.Column.Key == "Print264")
			{
				DataTable dt = InvGrnandGdnReports.InvDeliveryOrderSlip(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					UserId = clsGlobalVariables.UserAccount.ID,
					Id = Currid,
					DocumentTypeId = 84
				});
				if (dt.Rows.Count == 0 || dt == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "264-DeliveryChallanByDeliveryOrderTransfer.rpt");
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
		else
		{
			DocDate.Focus();
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
			MessageBox.Show(ex.ToString());
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void CalculateWeight()
	{
		try
		{
			if (((UltraGridBase)CmbPackUom).ActiveRow != null && txtQty.Text != string.Empty && Conversion.ToInt(CmbPackUom.Value) != 0)
			{
				double ItemPack = Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value);
				double Qty = Conversion.ToDouble((object)txtQty.Text);
				txtweight.Text = (ItemPack * Qty).ToString();
				txtGrossWeight.Text = (ItemPack * Qty).ToString();
			}
			else
			{
				txtweight.Text = "0";
				txtGrossWeight.Text = "0";
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
			if (Conversion.ToDouble((object)txtweight.Text) > 0.0)
			{
				double Qty = Conversion.ToDouble((object)txtQty.Text.Trim());
				double num = Conversion.ToDouble((object)txtweight.Text.Trim());
				double packingUnit = Conversion.ToDouble((object)txtPackUnit.Text.Trim());
				double PackingWeight = Conversion.ToDouble((object)txtPackTotal.Text.Trim());
				if (base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "PackUnit")
				{
					PackingWeight = packingUnit * Qty;
					txtPackTotal.Text = PackingWeight.ToString("#,##0.###");
				}
				else if (base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "PackWeight")
				{
					packingUnit = ((Qty > 0.0) ? (PackingWeight / Qty) : 0.0);
					txtPackUnit.Text = packingUnit.ToString("#,##0.#####");
				}
				else
				{
					PackingWeight = packingUnit * Qty;
					txtPackTotal.Text = PackingWeight.ToString("#,##0.###");
				}
				double GrossWeight = num + PackingWeight;
				txtGrossWeight.Text = GrossWeight.ToString("#,##0.###");
			}
			else
			{
				txtGrossWeight.Text = "0";
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
			CalculateGrossWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPackSize_TextChanged(object sender, EventArgs e)
	{
		CalculateGrossWeight();
	}

	private void CmbPackUom_TextChanged(object sender, EventArgs e)
	{
		CalculateWeight();
		CalculateGrossWeight();
	}

	private void txtweight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateGrossWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPackUnit_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateGrossWeight();
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
			CommonServices.InvDeliveryOrderSlip(Id, 84);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combitem_ValueChanged(object sender, EventArgs e)
	{
	}

	private void DocDate_ValueChanged(object sender, EventArgs e)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Expected O, but got Unknown
		try
		{
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow item in rows)
			{
				item.BeginEdit();
				double AvailableStock = InventoryStockEvalautionDetail.GetCurrentStockByItemId(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					ItemId = Conversion.ToInt((object)item.Cells["ItemId"].Value.ToString()),
					DocDate = DocDate.Value,
					WarehouseId = Conversion.ToInt((object)item.Cells["WareHouseFromId"].Value.ToString()),
					JobLotId = Conversion.ToInt((object)item.Cells["JobLotFromId"].Value.ToString())
				});
				item.Cells["AvailableStock"].Value = AvailableStock;
				item.BeginEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtVehicleNo_TextChanged(object sender, EventArgs e)
	{
		txtVehicleNo.SelectionStart = txtVehicleNo.Text.Length;
		txtVehicleNo.Text = txtVehicleNo.Text.ToUpper();
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

	private void FromDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void ToDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtweight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtPackUnit_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtPackWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void PurchsaeOrder_KeyDown(object sender, KeyEventArgs e)
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
					DocDate.Focus();
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
				if (e.Control && e.Shift && e.KeyCode == Keys.Delete && btnDelete.Visible && btnDelete.Enabled)
				{
					btnDelete_Click(null, null);
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
					((Control)(object)grd).Focus();
				}
				if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
				{
					btnprint_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1) && formright.DoHavePrintRights)
				{
					btnprint_Click(null, null);
				}
			}
			else
			{
				if (e.Control && e.KeyCode == Keys.S)
				{
					btnShowHistory_Click(null, null);
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
			dt.Rows.Add("Ctrl+P", "For Print 262");
			dt.Rows.Add("Alt+1", "For Print 262");
			dt.Rows.Add("Ctrl+F1", "For Loader SO Main Detail");
			dt.Rows.Add("Ctrl+F2", "For Loader SO Main");
			dt.Rows.Add("Ctrl+F5", "For Reference Parties Look up");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+F12", "For SaveAs");
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
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grd.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grd.CurrentColumn.Key == "Edit")
				{
					grd_DoubleClick(null, null);
				}
				if (grd.CurrentColumn.Key == "Delete")
				{
					DeleteDetailRow(r);
				}
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteDetailRow(r);
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grdhistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Expected O, but got Unknown
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
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
				string status = Conversion.ToString(item.Cells["OrderStatus"].Value);
				if (status != "Open")
				{
					MessageBox.Show("Sorry You can't Update this record because Order_Status is:" + status, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				Reset();
				Id = CuurID;
				ReadById(Id);
			}
			else
			{
				if (!e.Control || e.KeyCode != Keys.Space)
				{
					return;
				}
				if (grdhistory.CurrentColumn.Key == "Edit")
				{
					string status2 = Conversion.ToString(item.Cells["OrderStatus"].Value);
					if (status2 != "Open")
					{
						MessageBox.Show("Sorry You can't Update this record because Order_Status is:" + status2, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						return;
					}
					Reset();
					Id = CuurID;
					ReadById(Id);
				}
				if (grdhistory.CurrentColumn.Key == "SaveAs")
				{
					Reset();
					Id = CuurID;
					ReadById(Id);
					btnsave.Visible = false;
					btnupdate.Visible = false;
					btnDelete.Visible = false;
					btnSaveAs.Visible = true;
				}
				if (grdhistory.CurrentColumn.Key == "Print")
				{
					CommonServices.InvDeliveryOrderSlip(Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value), 84);
				}
				if (grdhistory.CurrentColumn.Key == "Print264")
				{
					DataTable dt = InvGrnandGdnReports.InvDeliveryOrderSlip(new ReportsParameters
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						UserId = clsGlobalVariables.UserAccount.ID,
						Id = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString()),
						DocumentTypeId = 84
					});
					if (dt.Rows.Count == 0 || dt == null)
					{
						throw new Exception("Record Not Found For Display");
					}
					Reporting val = new Reporting();
					val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
					val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
					val.ShowReportWithDataTable(dt, "264-DeliveryChallanByDeliveryOrderTransfer.rpt");
				}
				if (grdhistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(CuurID, 84);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeliveryOrderTransfer_FormClosed(object sender, FormClosedEventArgs e)
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
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Expected O, but got Unknown
		//IL_040b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Expected O, but got Unknown
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Expected O, but got Unknown
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Expected O, but got Unknown
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c2: Expected O, but got Unknown
		//IL_0505: Unknown result type (might be due to invalid IL or missing references)
		//IL_050f: Expected O, but got Unknown
		//IL_0547: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Expected O, but got Unknown
		//IL_055d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0567: Expected O, but got Unknown
		//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bf: Expected O, but got Unknown
		//IL_05c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ca: Expected O, but got Unknown
		//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d5: Expected O, but got Unknown
		//IL_0639: Unknown result type (might be due to invalid IL or missing references)
		//IL_0643: Expected O, but got Unknown
		//IL_064f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0659: Expected O, but got Unknown
		//IL_069c: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a6: Expected O, but got Unknown
		//IL_06a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b1: Expected O, but got Unknown
		//IL_07af: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b9: Expected O, but got Unknown
		//IL_07f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fb: Expected O, but got Unknown
		//IL_0f2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f35: Expected O, but got Unknown
		//IL_0f42: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f4c: Expected O, but got Unknown
		//IL_6a8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a96: Expected O, but got Unknown
		//IL_6aba: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ac4: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance38 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
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
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
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
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Sale.DeliveryOrderTransfer));
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel13 = new System.Windows.Forms.Panel();
		this.DetailGridPanel = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label15 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.btnplus = new System.Windows.Forms.Button();
		this.CmbJobLotTo = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.CmbWarehouseTo = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.txtPackUnit = new System.Windows.Forms.TextBox();
		this.label20 = new System.Windows.Forms.Label();
		this.rdSearchByCode = new System.Windows.Forms.RadioButton();
		this.rdSearchByName = new System.Windows.Forms.RadioButton();
		this.lblBalance = new System.Windows.Forms.Label();
		this.CmbCropYear = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.txtPackTotal = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbJobLotFrom = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.CmbWareHouseFrom = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.txtweight = new System.Windows.Forms.TextBox();
		this.label39 = new System.Windows.Forms.Label();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.label38 = new System.Windows.Forms.Label();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.CmbPackingType = new UltraCombo();
		this.CmbPackUom = new UltraCombo();
		this.CmbItemName = new UltraCombo();
		this.txtremarksdetail = new System.Windows.Forms.TextBox();
		this.label47 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.CmbDeliveryOrderType = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.cmbvehicletype = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.txtVehicleNo = new System.Windows.Forms.TextBox();
		this.label18 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.cmbBranchFrom = new UltraCombo();
		this.cmbBranchTo = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
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
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnSaveAs = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grdhistory = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label19 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label27 = new System.Windows.Forms.Label();
		this.GrdHistoryDetail = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.label51 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label16 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel13.SuspendLayout();
		this.DetailGridPanel.SuspendLayout();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel5.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLotTo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseTo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLotFrom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouseFrom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryOrderType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbvehicletype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchFrom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchTo).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel12.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryDetail).BeginInit();
		this.panel7.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.panel8.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(886, 490);
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
		this.tabControl1.Size = new System.Drawing.Size(886, 490);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel13);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(878, 461);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel13.Controls.Add(this.DetailGridPanel);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(2, 274);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(874, 185);
		this.panel13.TabIndex = 9;
		this.DetailGridPanel.Controls.Add(this.tabPage3);
		this.DetailGridPanel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.DetailGridPanel.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DetailGridPanel.Location = new System.Drawing.Point(0, 0);
		this.DetailGridPanel.Name = "DetailGridPanel";
		this.DetailGridPanel.SelectedIndex = 0;
		this.DetailGridPanel.Size = new System.Drawing.Size(874, 185);
		this.DetailGridPanel.TabIndex = 6;
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabPage3.Controls.Add(this.panel5);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(866, 156);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Do Detail";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.grd.AutomaticSort = false;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 26);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(860, 127);
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
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(860, 23);
		this.panel5.TabIndex = 2;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(820, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(6, 3);
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
		this.panel4.Location = new System.Drawing.Point(2, 52);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(874, 222);
		this.panel4.TabIndex = 2;
		this.groupBox3.BackColor = System.Drawing.Color.White;
		this.groupBox3.Controls.Add(this.btnplus);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLotTo);
		this.groupBox3.Controls.Add(this.label9);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouseTo);
		this.groupBox3.Controls.Add(this.label2);
		this.groupBox3.Controls.Add(this.txtPackUnit);
		this.groupBox3.Controls.Add(this.label20);
		this.groupBox3.Controls.Add(this.rdSearchByCode);
		this.groupBox3.Controls.Add(this.rdSearchByName);
		this.groupBox3.Controls.Add(this.lblBalance);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYear);
		this.groupBox3.Controls.Add(this.label7);
		this.groupBox3.Controls.Add(this.txtGrossWeight);
		this.groupBox3.Controls.Add(this.label6);
		this.groupBox3.Controls.Add(this.txtPackTotal);
		this.groupBox3.Controls.Add(this.label1);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLotFrom);
		this.groupBox3.Controls.Add(this.label8);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom);
		this.groupBox3.Controls.Add(this.label3);
		this.groupBox3.Controls.Add(this.txtweight);
		this.groupBox3.Controls.Add(this.label39);
		this.groupBox3.Controls.Add(this.txtQty);
		this.groupBox3.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox3.Controls.Add(this.label38);
		this.groupBox3.Controls.Add(this.btnUpdateDetail);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackingType);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUom);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.groupBox3.Controls.Add(this.txtremarksdetail);
		this.groupBox3.Controls.Add(this.label47);
		this.groupBox3.Controls.Add(this.label35);
		this.groupBox3.Controls.Add(this.label36);
		this.groupBox3.Controls.Add(this.label37);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.groupBox3.Location = new System.Drawing.Point(4, 106);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(871, 112);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Detail";
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(732, 83);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(39, 25);
		this.btnplus.TabIndex = 14;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.CmbJobLotTo.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLotTo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLotTo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLotTo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLotTo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLotTo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLotTo).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLotTo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLotTo).Location = new System.Drawing.Point(729, 36);
		((System.Windows.Forms.Control)(object)this.CmbJobLotTo).Name = "CmbJobLotTo";
		((System.Windows.Forms.Control)(object)this.CmbJobLotTo).Size = new System.Drawing.Size(139, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLotTo).TabIndex = 5;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(729, 16);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(62, 15);
		this.label9.TabIndex = 60;
		this.label9.Text = "Job Lot To";
		this.CmbWarehouseTo.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehouseTo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouseTo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouseTo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance52;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance82).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance84).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseTo).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWarehouseTo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseTo).Location = new System.Drawing.Point(159, 36);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseTo).Name = "CmbWarehouseTo";
		((System.Windows.Forms.Control)(object)this.CmbWarehouseTo).Size = new System.Drawing.Size(152, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseTo).TabIndex = 1;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(159, 16);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(88, 15);
		this.label2.TabIndex = 58;
		this.label2.Text = "Ware House To";
		this.txtPackUnit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackUnit.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackUnit.Location = new System.Drawing.Point(334, 84);
		this.txtPackUnit.Name = "txtPackUnit";
		this.txtPackUnit.Size = new System.Drawing.Size(70, 25);
		this.txtPackUnit.TabIndex = 10;
		this.txtPackUnit.Tag = "PackUnit";
		this.txtPackUnit.TextChanged += new System.EventHandler(txtPackUnit_TextChanged);
		this.txtPackUnit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPackUnit_KeyPress);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(336, 63);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(45, 15);
		this.label20.TabIndex = 56;
		this.label20.Text = "EB Unit";
		this.rdSearchByCode.AutoSize = true;
		this.rdSearchByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByCode.Location = new System.Drawing.Point(401, 15);
		this.rdSearchByCode.Name = "rdSearchByCode";
		this.rdSearchByCode.Size = new System.Drawing.Size(52, 19);
		this.rdSearchByCode.TabIndex = 54;
		this.rdSearchByCode.TabStop = true;
		this.rdSearchByCode.Text = "Code";
		this.rdSearchByCode.UseVisualStyleBackColor = true;
		this.rdSearchByName.AutoSize = true;
		this.rdSearchByName.Checked = true;
		this.rdSearchByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByName.Location = new System.Drawing.Point(342, 15);
		this.rdSearchByName.Name = "rdSearchByName";
		this.rdSearchByName.Size = new System.Drawing.Size(57, 19);
		this.rdSearchByName.TabIndex = 53;
		this.rdSearchByName.TabStop = true;
		this.rdSearchByName.Text = "Name";
		this.rdSearchByName.UseVisualStyleBackColor = true;
		this.rdSearchByName.CheckedChanged += new System.EventHandler(rdSearchByName_CheckedChanged);
		this.lblBalance.AutoSize = true;
		this.lblBalance.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBalance.Location = new System.Drawing.Point(548, 16);
		this.lblBalance.Name = "lblBalance";
		this.lblBalance.Size = new System.Drawing.Size(0, 15);
		this.lblBalance.TabIndex = 46;
		this.CmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance131;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance132).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance132).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance137).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance138).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance139).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Location = new System.Drawing.Point(495, 36);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Name = "CmbCropYear";
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Size = new System.Drawing.Size(92, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).TabIndex = 3;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(495, 16);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(54, 15);
		this.label7.TabIndex = 41;
		this.label7.Text = "CropYear";
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrossWeight.Location = new System.Drawing.Point(490, 84);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.ReadOnly = true;
		this.txtGrossWeight.Size = new System.Drawing.Size(86, 25);
		this.txtGrossWeight.TabIndex = 12;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(491, 63);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(75, 15);
		this.label6.TabIndex = 39;
		this.label6.Text = "GrossWeight";
		this.txtPackTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackTotal.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackTotal.Location = new System.Drawing.Point(406, 84);
		this.txtPackTotal.Name = "txtPackTotal";
		this.txtPackTotal.Size = new System.Drawing.Size(82, 25);
		this.txtPackTotal.TabIndex = 11;
		this.txtPackTotal.Tag = "PackWeight";
		this.txtPackTotal.TextChanged += new System.EventHandler(txtPackSize_TextChanged);
		this.txtPackTotal.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPackWeight_KeyPress);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(406, 63);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(49, 15);
		this.label1.TabIndex = 37;
		this.label1.Text = "EB Total";
		this.CmbJobLotFrom.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLotFrom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLotFrom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLotFrom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLotFrom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbJobLotFrom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLotFrom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLotFrom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLotFrom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLotFrom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLotFrom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbJobLotFrom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLotFrom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLotFrom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLotFrom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbJobLotFrom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLotFrom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLotFrom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance72).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLotFrom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbJobLotFrom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLotFrom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance73).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLotFrom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLotFrom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbJobLotFrom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLotFrom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLotFrom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbJobLotFrom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLotFrom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbJobLotFrom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLotFrom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLotFrom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLotFrom).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLotFrom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLotFrom).Location = new System.Drawing.Point(588, 36);
		((System.Windows.Forms.Control)(object)this.CmbJobLotFrom).Name = "CmbJobLotFrom";
		((System.Windows.Forms.Control)(object)this.CmbJobLotFrom).Size = new System.Drawing.Size(139, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLotFrom).TabIndex = 4;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(588, 16);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(77, 15);
		this.label8.TabIndex = 35;
		this.label8.Text = "Job Lot From";
		this.CmbWareHouseFrom.AllowNull = (DefaultableBoolean)1;
		this.CmbWareHouseFrom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWareHouseFrom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWareHouseFrom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance55;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWareHouseFrom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom).Location = new System.Drawing.Point(4, 36);
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom).Name = "CmbWareHouseFrom";
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom).Size = new System.Drawing.Size(152, 26);
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom).TabIndex = 0;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(4, 16);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(103, 15);
		this.label3.TabIndex = 33;
		this.label3.Text = "Ware House From";
		this.txtweight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtweight.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtweight.Location = new System.Drawing.Point(251, 84);
		this.txtweight.Name = "txtweight";
		this.txtweight.Size = new System.Drawing.Size(82, 25);
		this.txtweight.TabIndex = 9;
		this.txtweight.TextChanged += new System.EventHandler(txtweight_TextChanged);
		this.txtweight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtweight_KeyPress);
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(251, 63);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(68, 15);
		this.label39.TabIndex = 15;
		this.label39.Text = "Net Weight";
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQty.Location = new System.Drawing.Point(184, 84);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(66, 25);
		this.txtQty.TabIndex = 8;
		this.txtQty.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.txtQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(802, 83);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(65, 25);
		this.btnCancelUpdateDetial.TabIndex = 16;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(184, 63);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(26, 15);
		this.label38.TabIndex = 13;
		this.label38.Text = "Qty";
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(732, 83);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(68, 25);
		this.btnUpdateDetail.TabIndex = 15;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.CmbPackingType.AllowNull = (DefaultableBoolean)1;
		this.CmbPackingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Location = new System.Drawing.Point(4, 83);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Name = "CmbPackingType";
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Size = new System.Drawing.Size(98, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).TabIndex = 6;
		this.CmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance118;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance123).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance125).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Location = new System.Drawing.Point(105, 83);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Name = "CmbPackUom";
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Size = new System.Drawing.Size(76, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TextChanged += new System.EventHandler(CmbPackUom_TextChanged);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Leave += new System.EventHandler(combitempck_Leave);
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance90;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance109).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance12).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(314, 36);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 2;
		this.CmbItemName.ValueChanged += new System.EventHandler(combitem_ValueChanged);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Leave += new System.EventHandler(CmbItemName_Leave);
		this.txtremarksdetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarksdetail.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarksdetail.Location = new System.Drawing.Point(578, 84);
		this.txtremarksdetail.Name = "txtremarksdetail";
		this.txtremarksdetail.Size = new System.Drawing.Size(150, 25);
		this.txtremarksdetail.TabIndex = 13;
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(578, 63);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(65, 15);
		this.label47.TabIndex = 25;
		this.label47.Text = "Comments";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(4, 63);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(74, 15);
		this.label35.TabIndex = 25;
		this.label35.Text = "PackingType";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(105, 63);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(63, 15);
		this.label36.TabIndex = 8;
		this.label36.Text = "Pack UOM";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(314, 16);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(32, 15);
		this.label37.TabIndex = 9;
		this.label37.Text = "Item";
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchFrom);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchTo);
		this.groupBox1.Controls.Add(this.txtremarks);
		this.groupBox1.Controls.Add(this.txtdocno);
		this.groupBox1.Controls.Add(this.DocDate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryOrderType);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbvehicletype);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.txtVehicleNo);
		this.groupBox1.Controls.Add(this.label18);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.groupBox1.Location = new System.Drawing.Point(4, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(871, 101);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(52, 68);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(406, 25);
		this.txtremarks.TabIndex = 9;
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(52, 14);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(59, 23);
		this.txtdocno.TabIndex = 0;
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(114, 14);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(109, 22);
		this.DocDate.TabIndex = 1;
		this.DocDate.TabStop = false;
		this.DocDate.ValueChanged += new System.EventHandler(DocDate_ValueChanged);
		this.CmbDeliveryOrderType.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryOrderType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryOrderType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryOrderType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance144;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryOrderType).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryOrderType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryOrderType).Location = new System.Drawing.Point(52, 40);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryOrderType).Name = "CmbDeliveryOrderType";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryOrderType).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryOrderType).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryOrderType).TabStop = false;
		this.label10.AutoSize = true;
		this.label10.BackColor = System.Drawing.Color.White;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(226, 46);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(64, 15);
		this.label10.TabIndex = 51;
		this.label10.Text = "Vehicle No";
		this.cmbvehicletype.AllowNull = (DefaultableBoolean)1;
		this.cmbvehicletype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbvehicletype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbvehicletype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbvehicletype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance83;
		((SpecialBoxBase)((UltraGridBase)this.cmbvehicletype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance126).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbvehicletype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).Location = new System.Drawing.Point(295, 12);
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).Name = "cmbvehicletype";
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).Size = new System.Drawing.Size(163, 26);
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).TabIndex = 3;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(2, 18);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(48, 15);
		this.label5.TabIndex = 3;
		this.label5.Text = "Doc No";
		this.label14.AutoSize = true;
		this.label14.BackColor = System.Drawing.Color.White;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(226, 18);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(73, 15);
		this.label14.TabIndex = 50;
		this.label14.Text = "Vehicle Type";
		this.label33.AutoSize = true;
		this.label33.BackColor = System.Drawing.Color.White;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(2, 73);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(52, 15);
		this.label33.TabIndex = 8;
		this.label33.Text = "Remarks";
		this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtVehicleNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVehicleNo.Location = new System.Drawing.Point(295, 42);
		this.txtVehicleNo.Name = "txtVehicleNo";
		this.txtVehicleNo.Size = new System.Drawing.Size(164, 22);
		this.txtVehicleNo.TabIndex = 4;
		this.txtVehicleNo.TextChanged += new System.EventHandler(txtVehicleNo_TextChanged);
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(464, 46);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(60, 15);
		this.label18.TabIndex = 0;
		this.label18.Text = "Branch To";
		this.label18.Visible = false;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(464, 18);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(75, 15);
		this.label13.TabIndex = 0;
		this.label13.Text = "Branch From";
		this.cmbBranchFrom.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchFrom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchFrom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchFrom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchFrom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance17;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchFrom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchFrom).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbBranchFrom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBranchFrom).Location = new System.Drawing.Point(536, 12);
		((System.Windows.Forms.Control)(object)this.cmbBranchFrom).Name = "cmbBranchFrom";
		((System.Windows.Forms.Control)(object)this.cmbBranchFrom).Size = new System.Drawing.Size(190, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchFrom).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.cmbBranchFrom).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbBranchFrom).Leave += new System.EventHandler(cmbBranchFrom_Leave);
		this.cmbBranchTo.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchTo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchTo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchTo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchTo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance93;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchTo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance101).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchTo).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbBranchTo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBranchTo).Location = new System.Drawing.Point(536, 40);
		((System.Windows.Forms.Control)(object)this.cmbBranchTo).Name = "cmbBranchTo";
		((System.Windows.Forms.Control)(object)this.cmbBranchTo).Size = new System.Drawing.Size(190, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchTo).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.cmbBranchTo).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbBranchTo).Visible = false;
		((System.Windows.Forms.Control)(object)this.cmbBranchTo).Leave += new System.EventHandler(cmbBranchTo_Leave);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(2, 46);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(53, 15);
		this.label4.TabIndex = 27;
		this.label4.Text = "DO Type";
		this.txtSubNoId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSubNoId.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSubNoId.Location = new System.Drawing.Point(879, 15);
		this.txtSubNoId.Name = "txtSubNoId";
		this.txtSubNoId.ReadOnly = true;
		this.txtSubNoId.Size = new System.Drawing.Size(39, 25);
		this.txtSubNoId.TabIndex = 48;
		this.txtSubNoId.TabStop = false;
		this.txtSubNoId.Visible = false;
		this.txtDocNoId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoId.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoId.Location = new System.Drawing.Point(879, 46);
		this.txtDocNoId.Name = "txtDocNoId";
		this.txtDocNoId.ReadOnly = true;
		this.txtDocNoId.Size = new System.Drawing.Size(39, 25);
		this.txtDocNoId.TabIndex = 47;
		this.txtDocNoId.TabStop = false;
		this.txtDocNoId.Visible = false;
		this.txtRefDocTypeId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRefDocTypeId.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRefDocTypeId.Location = new System.Drawing.Point(879, 78);
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
		this.panel3.Size = new System.Drawing.Size(874, 25);
		this.panel3.TabIndex = 1;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(8, 2);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(201, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "Delivery Order Transfer";
		this.ChkPrint.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrint.AutoSize = true;
		this.ChkPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrint.ForeColor = System.Drawing.Color.White;
		this.ChkPrint.Location = new System.Drawing.Point(769, 1);
		this.ChkPrint.Name = "ChkPrint";
		this.ChkPrint.Size = new System.Drawing.Size(102, 20);
		this.ChkPrint.TabIndex = 8;
		this.ChkPrint.Text = "Is Preview";
		this.ChkPrint.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(874, 25);
		this.panel2.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[12]
		{
			this.btnnew, this.btnRefresh, this.toolStripSeparator6, this.btnupdate, this.btnSaveAs, this.btnsave, this.btnDelete, this.toolStripSeparator5, this.btnattachment, this.toolStripSeparator4,
			this.btnprint, this.BtnShortCutkeys
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(874, 27);
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
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(6, 27);
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
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.icons8_delete_bin_16;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.btnattachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.btnprint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnprint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(92, 24);
		this.btnprint.Text = "262-Print";
		this.btnprint.Click += new System.EventHandler(btnprint_Click);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel12);
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistoryDetail);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(878, 461);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(2, 149);
		this.panel9.Margin = new System.Windows.Forms.Padding(2);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(874, 126);
		this.panel9.TabIndex = 51;
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
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(874, 126);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 0;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.grdhistory.SelectionChanged += new System.EventHandler(grdhistory_SelectionChanged);
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdhistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdhistory_KeyDown);
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label19);
		this.panel12.Controls.Add(this.ctrlGrdBar2);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(2, 127);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(874, 22);
		this.panel12.TabIndex = 46;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label19.ForeColor = System.Drawing.Color.White;
		this.label19.Location = new System.Drawing.Point(3, 1);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(40, 17);
		this.label19.TabIndex = 2;
		this.label19.Text = "Main";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(828, -1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdhistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(43, 21);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label27);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel6.Location = new System.Drawing.Point(2, 275);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(874, 22);
		this.panel6.TabIndex = 49;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label27.ForeColor = System.Drawing.Color.White;
		this.label27.Location = new System.Drawing.Point(2, 3);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(211, 17);
		this.label27.TabIndex = 2;
		this.label27.Text = "Detail Of Above Selected Row";
		this.GrdHistoryDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.GrdHistoryDetail).BackColor = System.Drawing.Color.White;
		GrdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistoryDetail.DesignTimeLayout = GrdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.GrdHistoryDetail.DynamicFiltering = true;
		this.GrdHistoryDetail.FilterMode = (FilterMode)2;
		this.GrdHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistoryDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Location = new System.Drawing.Point(2, 297);
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Name = "GrdHistoryDetail";
		((ScrollableBase)this.GrdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Size = new System.Drawing.Size(874, 162);
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).TabIndex = 0;
		this.GrdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.GrdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel7.Controls.Add(this.groupBox2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(2, 53);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(874, 74);
		this.panel7.TabIndex = 0;
		this.groupBox2.Controls.Add(this.rdapproveddate);
		this.groupBox2.Controls.Add(this.rdmodifydate);
		this.groupBox2.Controls.Add(this.drdocdate);
		this.groupBox2.Controls.Add(this.rdentrydate);
		this.groupBox2.Controls.Add(this.btnShowHistory);
		this.groupBox2.Controls.Add(this.label51);
		this.groupBox2.Controls.Add(this.label52);
		this.groupBox2.Controls.Add(this.label53);
		this.groupBox2.Controls.Add(this.label54);
		this.groupBox2.Controls.Add(this.ToDocNoHistory);
		this.groupBox2.Controls.Add(this.FromDocNoHistory);
		this.groupBox2.Controls.Add(this.ToDateHistory);
		this.groupBox2.Controls.Add(this.FromDateHistory);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(2, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(874, 70);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(737, 39);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 8;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(645, 39);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 7;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(486, 39);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 5;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(562, 39);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 6;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(423, 36);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(58, 26);
		this.btnShowHistory.TabIndex = 4;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(340, 19);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(64, 15);
		this.label51.TabIndex = 86;
		this.label51.Text = "To Doc No";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(257, 19);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(79, 15);
		this.label52.TabIndex = 85;
		this.label52.Text = "From Doc No";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(134, 19);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(48, 15);
		this.label53.TabIndex = 84;
		this.label53.Text = "To Date";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(3, 19);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(63, 15);
		this.label54.TabIndex = 83;
		this.label54.Text = "From Date";
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(340, 38);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(260, 38);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(134, 38);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(126, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(6, 38);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(126, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label16);
		this.panel8.Controls.Add(this.toolStrip2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(2, 2);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(874, 51);
		this.panel8.TabIndex = 0;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.White;
		this.label16.Location = new System.Drawing.Point(6, 28);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(190, 19);
		this.label16.TabIndex = 22;
		this.label16.Text = "Delivery Order History";
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNewHistory, this.btnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(874, 27);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 10f);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(61, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnResetHistory_Click);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(886, 490);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "DeliveryOrderTransfer";
		this.Text = "Delivery Order";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(DeliveryOrderTransfer_FormClosed);
		base.Load += new System.EventHandler(DeliverOrder_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(PurchsaeOrder_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		this.DetailGridPanel.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLotTo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseTo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLotFrom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouseFrom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryOrderType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbvehicletype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchFrom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchTo).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryDetail).EndInit();
		this.panel7.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
