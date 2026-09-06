using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.BLL.Pos;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.POS;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
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
namespace Architecture.WinApp.PreBookingAndDelivery
{

public class PreBookingOrder : Form
{
	private int updateDetailIndex = -1;

	private int PriceScheduleId;

	public int RecId;

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool RateEditableOnPreBookingOrder;

	private bool HasBookingOffice_App;

	private bool FormInitialized;

	private Rightsobjects formRights = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private Attachment AT = new Attachment();

	private DataTable dtItemService = new DataTable();

	private DataTable dtDetail = new DataTable();

	private DataTable dtHistoryData = new DataTable();

	private DataTable dtSupplierCustomer = new DataTable();

	private UserAccount UserAccount = new UserAccount();

	private List<PreBookingOrderDetail> lstRemoveRecord = new List<PreBookingOrderDetail>();

	private IContainer components;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnsave;

	private ToolStripButton btnSaveAs;

	private ToolStripButton btnupdate;

	private ToolStripButton btnDelete;

	private ToolStripButton btnattachment;

	private ToolStripButton btnPrint;

	private Label label23;

	private CheckBox ChkBox;

	private GroupBox groupBox1;

	private DateTimePicker DocDate;

	private Label label17;

	private TextBox txtdocno;

	internal UltraCombo CmbCustomer;

	private Label label12;

	private Label label2;

	private TextBox txtRemarks;

	private GroupBox groupBox2;

	internal UltraCombo CmbItem;

	private Label label37;

	internal UltraCombo cmbPackUom;

	private Label label36;

	private TextBox txtWeight;

	private Label label3;

	private TextBox txtqty;

	private Label label38;

	internal UltraCombo CmbRateUom;

	private Label label5;

	private TextBox txtItemRate;

	private Label label4;

	private Label label7;

	private TextBox txtItemAmount;

	private TextBox txtRemarksDetail;

	private Label label6;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private Button btnAddDetail;

	internal GridEX grd;

	private CtrlGrdBar ctrlGrdBar1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel2;

	private Panel panel1;

	private Panel panel5;

	private Panel panel3;

	private Panel panel6;

	private Panel panel4;

	private ToolStrip toolStrip2;

	private ToolStripButton btnResetHistory;

	private ToolStripButton btnRefreshHistory;

	private GroupBox groupBox3;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Button btnShowHistory;

	private Label label50;

	private Label label51;

	private Label label52;

	private Label label53;

	private Label label54;

	internal UltraCombo CmbCustomerHistory;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Panel panel8;

	private Label label8;

	internal GridEX GridDetail;

	private Panel panel10;

	private Label label10;

	internal GridEX DataGridHistory;

	private CtrlGrdBar ctrlGrdBar2;

	private ToolStripButton btnshortcutkeys;

	internal UltraCombo CmbCropyr;

	private Label label11;

	internal UltraCombo CmbCostCenter;

	private Label lblCostCenter;

	private TextBox txtDeliveryDays;

	internal UltraCombo CmbDeliveryTerm;

	private Label label30;

	private Label label28;

	internal UltraCombo CmbPaymentTerm;

	private DateTimePicker deliveryStartDate;

	private Label label32;

	private DateTimePicker DueDate;

	private TextBox txtDueDays;

	private Label label24;

	private Label label26;

	private TextBox txtCommRate;

	internal UltraCombo cmbCommRateUom;

	private TextBox txtCommAmount;

	internal UltraCombo cmbCommType;

	private TextBox txtCommRemarks;

	internal UltraCombo cmbCommissionAgent;

	private Label label20;

	private Label label21;

	private Label label25;

	private Label label19;

	private Label label13;

	private Label label14;

	private Label label1;

	private DateTimePicker datExpiryDate;

	private GroupBox groupBox5;

	internal UltraCombo CmbOtherCommissionAgent;

	private Label label15;

	private Label label16;

	private TextBox txtOtherCommissionRate;

	private TextBox txtOtherCommissionRemarks;

	private Label label18;

	private Label label22;

	internal UltraCombo CmbOtherCommissionUom;

	internal UltraCombo CmbOtherCommissionType;

	private Label label27;

	private Label label29;

	private TextBox txtOtherCommissionAmount;

	private GroupBox groupBox4;

	public PreBookingOrder(UserAccount Puser)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = Puser;
	}

	private void grdSetting()
	{
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["ScheduleId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["CropYearId"].Visible = false;
			grd.RootTable.Columns["PackUomId"].Visible = false;
			grd.RootTable.Columns["RateUomId"].Visible = false;
			grd.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			grd.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Weight"].FormatString = "#,##0.###";
			grd.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grd);
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "X";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
			grd.RootTable.Columns["Delete"].Width = 20;
			grd.RootTable.Columns["Delete"].Position = 0;
			grd.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PreBookingOrder_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1250, 625);
			FormInitialized = true;
			if (UserAccount.SupplierCustomerId > 0)
			{
				((Control)(object)CmbCustomer).Enabled = false;
				((Control)(object)CmbCustomerHistory).Enabled = false;
				CmbCustomer.Value = UserAccount.SupplierCustomerId;
				CmbCustomerHistory.Value = UserAccount.SupplierCustomerId;
			}
			else
			{
				((Control)(object)CmbCustomer).Enabled = true;
				((Control)(object)CmbCustomerHistory).Enabled = true;
			}
			HasBookingOffice_App = UserAccount.AppId == 5;
			if (HasBookingOffice_App)
			{
				((Control)(object)CmbCostCenter).Enabled = false;
				lblCostCenter.Visible = true;
				((Control)(object)CmbCostCenter).Visible = true;
				CostCenterFill();
			}
			else
			{
				lblCostCenter.Visible = false;
				((Control)(object)CmbCostCenter).Visible = false;
			}
			formRights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formRights.DoHaveSaveRight;
			btnPrint.Enabled = formRights.DoHavePrintRights;
			btnupdate.Enabled = formRights.DoHaveUpdateRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formRights.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formRights.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formRights.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formRights.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formRights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formRights.DoHaveGridGroupExpandRights;
			RateEditableOnPreBookingOrder = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("RateEditableOnPrebookingOrder"));
			if (RateEditableOnPreBookingOrder)
			{
				txtItemRate.Enabled = true;
			}
			else
			{
				txtItemRate.Enabled = false;
			}
			dtDetail.Columns.Add("Id", typeof(int));
			dtDetail.Columns.Add("ScheduleId", typeof(int));
			dtDetail.Columns.Add("ItemId", typeof(int));
			dtDetail.Columns.Add("ItemName", typeof(string));
			dtDetail.Columns.Add("CropYearId", typeof(int));
			dtDetail.Columns.Add("CropYear", typeof(string));
			dtDetail.Columns.Add("PackUomId", typeof(int));
			dtDetail.Columns.Add("PackUom", typeof(string));
			dtDetail.Columns.Add("ItemQty", typeof(double));
			dtDetail.Columns.Add("Weight", typeof(double));
			dtDetail.Columns.Add("ItemRate", typeof(double));
			dtDetail.Columns.Add("RateUomId", typeof(int));
			dtDetail.Columns.Add("RateUom", typeof(string));
			dtDetail.Columns.Add("ItemAmount", typeof(double));
			dtDetail.Columns.Add("Remarks", typeof(string));
			grd.DataSource = dtDetail;
			grd.RetrieveStructure();
			grdSetting();
			GenerateDocNo();
			SuppCustomerFill(Conversion.ToInt(CmbCostCenter.Value));
			PaymentTerms();
			DeliveryTerms();
			CommissionTypeFill();
			CommissionUOMFill();
			ItemFill();
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			HistoryCombosFill();
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			ToDateHistory.Value = DateTime.Now;
			DocDate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateDocNo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		try
		{
			int Code = PreBookingOrder.GenerateCode(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 129
			});
			if (Code > 0)
			{
				txtdocno.Text = Code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CostCenterFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		try
		{
			DataTable dt = UsersWithCostCenter.UsersWithCostCenter_AllocatedData(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				UserId = UserAccount.ID
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbCostCenter, "CostCenterId", "CostCenterName", "Cost Center", false);
				((UltraGridBase)CmbCostCenter).Rows[0].Activate();
			}
			else
			{
				((Control)(object)CmbCostCenter).Text = string.Empty;
				((UltraGridBase)CmbCostCenter).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void SuppCustomerFill(int CostCenterId = 0)
	{
		try
		{
			dtSupplierCustomer.Rows.Clear();
			dtSupplierCustomer = CommonServices.getSupplierCustomersForDropdown(0, CostCenterId, "1,2,3,4,5,6,12,14,15,16,17");
			if (dtSupplierCustomer.Rows.Count > 0)
			{
				DDL.BindDDL(dtSupplierCustomer, CmbCustomer, "Id", "CompanyName", "Customer Name", ZeroIndex: false);
				DDL.BindDDL(dtSupplierCustomer, cmbCommissionAgent, "Id", "CompanyName", "Commission Agent", ZeroIndex: false);
				DDL.BindDDL(dtSupplierCustomer, CmbOtherCommissionAgent, "Id", "CompanyName", "Other Commission Agent", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PaymentTerms()
	{
		try
		{
			int ID = Conversion.ToInt(CmbPaymentTerm.Value);
			DataTable dt = CommonServices.GetDueTermServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbPaymentTerm, "Id", "TermsDescription", "Payment Term", true);
				((UltraGridBase)CmbPaymentTerm).Rows[2].Activate();
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbPaymentTerm.Value = ID;
					}
					else
					{
						((Control)(object)CmbPaymentTerm).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbPaymentTerm).Text = string.Empty;
				((UltraGridBase)CmbPaymentTerm).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DeliveryTerms()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("DeliveryTerm");
			dt.Rows.Add(1, "Load");
			dt.Rows.Add(2, "Ponch");
			DDL.BindDDL(dt, CmbDeliveryTerm, "Id", "DeliveryTerm", "Delivery Term", ZeroIndex: false);
			((UltraGridBase)CmbDeliveryTerm).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("CommissionType");
			dt.Rows.Add(1, "Flat");
			dt.Rows.Add(2, "Percent");
			dt.Rows.Add(3, "Comm Weight");
			DDL.BindDDL(dt, cmbCommType, "Id", "CommissionType", "CommissionType", ZeroIndex: false);
			DDL.BindDDL(dt, CmbOtherCommissionType, "Id", "CommissionType", "CommissionType", ZeroIndex: false);
			((UltraGridBase)cmbCommType).Rows[0].Activate();
			((UltraGridBase)CmbOtherCommissionType).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionUOMFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("UOM");
			dt.Rows.Add(1, "40");
			dt.Rows.Add(2, "50");
			dt.Rows.Add(3, "60");
			dt.Rows.Add(4, "100");
			DDL.BindDDL(dt, cmbCommRateUom, "Id", "UOM", "UOM", ZeroIndex: false);
			((UltraGridBase)cmbCommRateUom).Rows[0].Activate();
			DDL.BindDDL(dt, CmbOtherCommissionUom, "Id", "UOM", "UOM", ZeroIndex: false);
			((UltraGridBase)CmbOtherCommissionUom).Rows[0].Activate();
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
			DataTable dtitem = CommonServices.ItemBindFromPricingSchedule(6);
			if (dtitem.Rows.Count > 0)
			{
				DDL.BindDDL(dtitem, CmbItem, "ItemId", "ItemName", "Item Name", ZeroIndex: false);
				((UltraGridBase)CmbItem).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbItem).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbItem).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				((UltraGridBase)CmbItem).DisplayLayout.Bands[0].Columns[1].Width = 500;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CropYear()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		try
		{
			string ID = Conversion.ToString((object)((Control)(object)CmbCropyr).Text);
			DataTable dtCrop = ItemPricingSchedule.getCropYearFromPricingScheduleByItemId(new ItemPricingSchedule
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				PriceTypeId = 6,
				ItemId = Conversion.ToInt(CmbItem.Value),
				EffectedDate = Conversion.ToDateTime((object)DocDate.Value)
			});
			if (dtCrop != null && dtCrop.Rows.Count > 0)
			{
				DDL.BindDDL(dtCrop, CmbCropyr, "Id", "CropYear", "Crop Year", ZeroIndex: false);
				if (!string.IsNullOrEmpty(ID))
				{
					if (dtCrop.AsEnumerable().Any((DataRow row) => row.Field<string>("CropYear") == ID))
					{
						((Control)(object)CmbCropyr).Text = ID;
					}
					else
					{
						((UltraGridBase)CmbCropyr).Rows[0].Activate();
					}
				}
				else
				{
					((UltraGridBase)CmbCropyr).Rows[0].Activate();
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

	private void HistoryCombosFill()
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		try
		{
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id", typeof(int));
			dtCustomer.Columns.Add("Customer", typeof(string));
			DataTable dt = PreBookingOrder.GetDataForDropDownFromPreBookingOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				AppId = UserAccount.AppId,
				UserId = UserAccount.ID,
				DocumentTypeIds = "129",
				Activity = "Customer",
				CostCenterId = Conversion.ToInt(CmbCostCenter.Value)
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
			}
			DDL.BindDDL(dtCustomer, CmbCustomerHistory, "Id", "Customer", "Customer Name", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbPaymentTerm_Leave(object sender, EventArgs e)
	{
		try
		{
			txtDueDays.Enabled = true;
			if (Conversion.ToInt(CmbPaymentTerm.Value) == 1 || Conversion.ToInt(CmbPaymentTerm.Value) == 3)
			{
				txtDueDays.Text = "";
				txtDueDays.Enabled = false;
			}
			else
			{
				txtDueDays.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void bindRateUomAndItemPackUom(int ID)
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		try
		{
			string PackUom = ((Control)(object)cmbPackUom).Text;
			string RateUom = ((Control)(object)CmbRateUom).Text;
			((Control)(object)cmbPackUom).Text = string.Empty;
			((Control)(object)CmbRateUom).Text = string.Empty;
			DataTable dtuomsch = UOMSchedule.SearchByObject(new UOMSchedule
			{
				ItemId = ID,
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtuomsch.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("UOMCode");
				dt.Columns.Add("Equivalent");
				for (int i = 0; i < dtuomsch.Rows.Count; i++)
				{
					dt.Rows.Add(dtuomsch.Rows[i]["Id"], dtuomsch.Rows[i]["UOMCode"], dtuomsch.Rows[i]["Equivalent"]);
				}
				DDL.BindDDL(dt, cmbPackUom, "Id", "UOMCode", "Pack Uom", ZeroIndex: false);
				((UltraGridBase)cmbPackUom).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				DDL.BindDDL(dt, CmbRateUom, "Id", "UOMCode", "Rate Uom", ZeroIndex: false);
				((UltraGridBase)CmbRateUom).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == PackUom))
				{
					((Control)(object)cmbPackUom).Text = PackUom;
				}
				else
				{
					((Control)(object)cmbPackUom).Text = string.Empty;
				}
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == RateUom))
				{
					((Control)(object)CmbRateUom).Text = RateUom;
				}
				else
				{
					((Control)(object)CmbRateUom).Text = string.Empty;
				}
			}
			else
			{
				((Control)(object)cmbPackUom).Text = string.Empty;
				((Control)(object)CmbRateUom).Text = string.Empty;
				((UltraGridBase)cmbPackUom).DataSource = null;
				((UltraGridBase)CmbRateUom).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbItem_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbItem.Value) > 0)
			{
				bindRateUomAndItemPackUom(Conversion.ToInt(CmbItem.Value));
			}
			else
			{
				((Control)(object)cmbPackUom).Text = string.Empty;
				((Control)(object)CmbRateUom).Text = string.Empty;
				((UltraGridBase)cmbPackUom).DataSource = null;
				((UltraGridBase)CmbRateUom).DataSource = null;
			}
			CropYear();
			getRateRateUom();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void getRateRateUom()
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(CmbItem.Value) > 0 && Conversion.ToInt(CmbCropyr.Value) > 0)
			{
				DataTable dt = ItemPricingSchedule.GetItemRateAndUomIdByEffectedDate(new ItemPricingSchedule
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					PriceTypeId = 6,
					ItemId = Conversion.ToInt(CmbItem.Value),
					CropYearId = Conversion.ToInt(CmbCropyr.Value),
					EffectedDate = Conversion.ToDateTime((object)DocDate.Value)
				});
				if (dt.Rows.Count > 0)
				{
					PriceScheduleId = Conversion.ToInt(dt.Rows[0]["Id"]);
					CmbRateUom.Value = Conversion.ToInt(dt.Rows[0]["RateUomId"]);
					double ItemRate = Conversion.ToDouble(dt.Rows[0]["ItemPrice"]);
					txtItemRate.Text = ItemRate.ToString("#,##0.###");
				}
				else
				{
					PriceScheduleId = 0;
					((Control)(object)CmbRateUom).Text = string.Empty;
					txtItemRate.Text = "0";
				}
			}
			else
			{
				PriceScheduleId = 0;
				((Control)(object)CmbRateUom).Text = string.Empty;
				txtItemRate.Text = "0";
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtdocno.Text.Trim() == string.Empty || txtdocno.Text.Trim() == "0" || txtdocno.Text.Trim() == null)
		{
			MessageBox.Show("Doc No Field is Required");
			txtdocno.Focus();
			return false;
		}
		if (HasBookingOffice_App && (((UltraGridBase)CmbCostCenter).ActiveRow == null || Conversion.ToInt(CmbCostCenter.Value) == 0))
		{
			MessageBox.Show("Cost Center Field is Required");
			CmbCostCenter.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCustomer).ActiveRow == null)
		{
			MessageBox.Show("Customer Name Field is Required");
			CmbCustomer.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPaymentTerm).ActiveRow == null || Conversion.ToInt(CmbPaymentTerm.Value) == 0)
		{
			MessageBox.Show("Payment Term Field is Required");
			CmbPaymentTerm.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbPaymentTerm.Value) == 2 && Conversion.ToInt((object)txtDueDays.Text) == 0)
		{
			MessageBox.Show("Due Days Field is Required");
			txtDueDays.Focus();
			return false;
		}
		if (((UltraGridBase)CmbDeliveryTerm).ActiveRow == null || Conversion.ToInt(CmbDeliveryTerm.Value) == 0)
		{
			MessageBox.Show("Delivery Term Field is Required");
			CmbDeliveryTerm.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)CmbItem).ActiveRow == null)
		{
			MessageBox.Show("ItemName Field is Required");
			CmbItem.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCropyr).ActiveRow == null)
		{
			MessageBox.Show("Crop Year Field is Required");
			CmbCropyr.Focus();
			return false;
		}
		if (((UltraGridBase)cmbPackUom).ActiveRow == null)
		{
			MessageBox.Show("PackUom Field is Required");
			cmbPackUom.Focus();
			return false;
		}
		if (txtqty.Text.Trim() == string.Empty || txtqty.Text.Trim() == "0")
		{
			MessageBox.Show("Item Qty Field is Required");
			txtqty.Focus();
			return false;
		}
		if (txtWeight.Text.Trim() == string.Empty || txtWeight.Text.Trim() == "0")
		{
			MessageBox.Show("Weight Field is Required");
			txtWeight.Focus();
			return false;
		}
		if (txtItemRate.Text.Trim() == string.Empty || txtItemRate.Text.Trim() == "0")
		{
			MessageBox.Show("Item Rate Field is Required");
			txtItemRate.Focus();
			return false;
		}
		if (txtItemAmount.Text.Trim() == string.Empty || txtItemAmount.Text.Trim() == "0")
		{
			MessageBox.Show("Item Amount Field is Required");
			txtItemAmount.Focus();
			return false;
		}
		return true;
	}

	private void btnAddDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				dtDetail.Rows.Add(0, PriceScheduleId, Conversion.ToInt(CmbItem.Value), ((Control)(object)CmbItem).Text.Trim(), Conversion.ToInt(CmbCropyr.Value), ((Control)(object)CmbCropyr).Text.Trim(), Conversion.ToInt(cmbPackUom.Value), ((Control)(object)cmbPackUom).Text.Trim(), Conversion.ToDouble((object)txtqty.Text.Trim()), Conversion.ToDouble((object)txtWeight.Text.Trim()), Conversion.ToDouble((object)txtItemRate.Text.Trim()), Conversion.ToInt(CmbRateUom.Value), ((Control)(object)CmbRateUom).Text.Trim(), Conversion.ToDouble((object)txtItemAmount.Text), txtRemarksDetail.Text.Trim());
				grd.DataSource = dtDetail;
				grd.RetrieveStructure();
				grdSetting();
				CmbItem.Focus();
				ResetDetail();
				DocDate.Enabled = false;
				TotalCommissionAmount();
				TotalOtherCommissionAmount();
			}
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
			PriceScheduleId = 0;
			((Control)(object)CmbItem).Text = string.Empty;
			((Control)(object)CmbCropyr).Text = string.Empty;
			((UltraGridBase)CmbCropyr).DataSource = null;
			((Control)(object)cmbPackUom).Text = string.Empty;
			txtqty.Text = string.Empty;
			txtWeight.Text = string.Empty;
			txtItemRate.Text = string.Empty;
			((Control)(object)CmbRateUom).Text = string.Empty;
			txtItemAmount.Text = string.Empty;
			txtRemarksDetail.Text = string.Empty;
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
			if (item != null && (int)item.RowType == 0)
			{
				updateDetailIndex = item.RowIndex;
				PriceScheduleId = Conversion.ToInt(item.Cells["ScheduleId"].Value);
				CmbItem.Value = item.Cells["ItemId"].Value.ToString();
				CmbItem_Leave(null, null);
				CmbCropyr.Value = Conversion.ToInt(item.Cells["CropYearId"].Value);
				cmbPackUom.Value = item.Cells["PackUomId"].Value.ToString();
				txtqty.Text = item.Cells["ItemQty"].Value.ToString();
				txtWeight.Text = item.Cells["Weight"].Value.ToString();
				txtItemRate.Text = item.Cells["ItemRate"].Value.ToString();
				CmbRateUom.Value = item.Cells["RateUomId"].Value.ToString();
				txtItemAmount.Text = item.Cells["ItemAmount"].Value.ToString();
				txtRemarksDetail.Text = item.Cells["Remarks"].Value.ToString();
				btnAddDetail.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				CmbItem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!(e.Column.Key == "Delete"))
			{
				return;
			}
			GridEXRow item = grd.CurrentRow;
			if (updateDetailIndex == Conversion.ToInt((object)item.RowIndex))
			{
				throw new Exception("You can't Delete this Record Because This Record is In Update Mode");
			}
			if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			if ((btnsave.Visible && btnsave.Enabled) || (btnSaveAs.Visible && btnSaveAs.Enabled))
			{
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
			}
			else if (Conversion.ToInt(item.Cells["Id"].Value) > 0)
			{
				PreBookingOrderDetail Pd = new PreBookingOrderDetail();
				Pd.Id = Conversion.ToInt(item.Cells["Id"].Value);
				Pd.PriceScheduleId = Conversion.ToInt(item.Cells["ScheduleId"].Value);
				Pd.OrderItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
				Pd.Crop = Conversion.ToString(item.Cells["CropYear"].Value);
				Pd.CropYearId = Conversion.ToInt(item.Cells["CropYearId"].Value);
				Pd.OrderItemUOMId = Conversion.ToInt(item.Cells["PackUomId"].Value);
				Pd.OrderItemQty = Conversion.ToDouble(item.Cells["ItemQty"].Value);
				Pd.NetWeight = Conversion.ToDouble(item.Cells["Weight"].Value);
				Pd.OrderItemRate = Conversion.ToDouble(item.Cells["ItemRate"].Value);
				Pd.OrderItemRateUOMId = Conversion.ToInt(item.Cells["RateUomId"].Value);
				Pd.Amount = Conversion.ToDouble(item.Cells["ItemAmount"].Value);
				Pd.OrderRemarks = Conversion.ToString(item.Cells["Remarks"].Value);
				Pd.ActionTypeId = 3;
				lstRemoveRecord.Add(Pd);
				dtDetail.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				grd.UpdateData();
			}
			else if (item != null && (int)item.RowType == 0)
			{
				item.Delete();
			}
			TotalCommissionAmount();
			if (grd.GetRows().Length == 0)
			{
				DocDate.Enabled = true;
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
				dtDetail.Rows[updateDetailIndex]["ScheduleId"] = PriceScheduleId;
				dtDetail.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(CmbItem.Value);
				dtDetail.Rows[updateDetailIndex]["ItemName"] = ((Control)(object)CmbItem).Text;
				dtDetail.Rows[updateDetailIndex]["CropYearId"] = Conversion.ToInt(CmbCropyr.Value);
				dtDetail.Rows[updateDetailIndex]["CropYear"] = ((Control)(object)CmbCropyr).Text;
				dtDetail.Rows[updateDetailIndex]["PackUomId"] = Conversion.ToInt(cmbPackUom.Value);
				dtDetail.Rows[updateDetailIndex]["PackUom"] = ((Control)(object)cmbPackUom).Text;
				dtDetail.Rows[updateDetailIndex]["ItemQty"] = Conversion.ToDouble((object)txtqty.Text);
				dtDetail.Rows[updateDetailIndex]["Weight"] = Conversion.ToDouble((object)txtWeight.Text);
				dtDetail.Rows[updateDetailIndex]["ItemRate"] = Conversion.ToDouble((object)txtItemRate.Text);
				dtDetail.Rows[updateDetailIndex]["RateUomId"] = Conversion.ToInt(CmbRateUom.Value);
				dtDetail.Rows[updateDetailIndex]["RateUom"] = ((Control)(object)CmbRateUom).Text;
				dtDetail.Rows[updateDetailIndex]["ItemAmount"] = Conversion.ToDouble((object)txtItemAmount.Text);
				dtDetail.Rows[updateDetailIndex]["Remarks"] = txtRemarksDetail.Text;
				grd.DataSource = dtDetail;
				btnAddDetail.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				ResetDetail();
				CmbItem.Focus();
				TotalCommissionAmount();
				TotalOtherCommissionAmount();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Expected O, but got Unknown
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_051f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grd.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grd.CurrentColumn.Key == "Delete")
				{
					if (updateDetailIndex == Conversion.ToInt((object)item.RowIndex))
					{
						throw new Exception("You can't Delete this Record Because This Record is In Update Mode");
					}
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
					{
						return;
					}
					if ((btnsave.Visible && btnsave.Enabled) || (btnSaveAs.Visible && btnSaveAs.Enabled))
					{
						if (item != null && (int)item.RowType == 0)
						{
							item.Delete();
						}
					}
					else if (Conversion.ToInt(item.Cells["Id"].Value) > 0)
					{
						PreBookingOrderDetail Pd = new PreBookingOrderDetail();
						Pd.Id = Conversion.ToInt(item.Cells["Id"].Value);
						Pd.PriceScheduleId = Conversion.ToInt(item.Cells["ScheduleId"].Value);
						Pd.OrderItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
						Pd.OrderItemUOMId = Conversion.ToInt(item.Cells["PackUomId"].Value);
						Pd.OrderItemQty = Conversion.ToDouble(item.Cells["ItemQty"].Value);
						Pd.NetWeight = Conversion.ToDouble(item.Cells["Weight"].Value);
						Pd.OrderItemRate = Conversion.ToDouble(item.Cells["ItemRate"].Value);
						Pd.OrderItemRateUOMId = Conversion.ToInt(item.Cells["RateUomId"].Value);
						Pd.Amount = Conversion.ToDouble(item.Cells["ItemAmount"].Value);
						Pd.OrderRemarks = Conversion.ToString(item.Cells["Remarks"].Value);
						Pd.ActionTypeId = 3;
						lstRemoveRecord.Add(Pd);
						dtDetail.Rows[item.RowIndex].Delete();
						if (item != null && (int)item.RowType == 0)
						{
							item.Delete();
						}
						grd.UpdateData();
					}
					else if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
				}
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (updateDetailIndex == Conversion.ToInt((object)item.RowIndex))
				{
					throw new Exception("You can't Delete this Record Because This Record is In Update Mode");
				}
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				if ((btnsave.Visible && btnsave.Enabled) || (btnSaveAs.Visible && btnSaveAs.Enabled))
				{
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
				}
				else if (Conversion.ToInt(item.Cells["Id"].Value) > 0)
				{
					PreBookingOrderDetail Pd2 = new PreBookingOrderDetail();
					Pd2.Id = Conversion.ToInt(item.Cells["Id"].Value);
					Pd2.PriceScheduleId = Conversion.ToInt(item.Cells["ScheduleId"].Value);
					Pd2.OrderItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
					Pd2.OrderItemUOMId = Conversion.ToInt(item.Cells["PackUomId"].Value);
					Pd2.OrderItemQty = Conversion.ToDouble(item.Cells["ItemQty"].Value);
					Pd2.NetWeight = Conversion.ToDouble(item.Cells["Weight"].Value);
					Pd2.OrderItemRate = Conversion.ToDouble(item.Cells["ItemRate"].Value);
					Pd2.OrderItemRateUOMId = Conversion.ToInt(item.Cells["RateUomId"].Value);
					Pd2.Amount = Conversion.ToDouble(item.Cells["ItemAmount"].Value);
					Pd2.OrderRemarks = Conversion.ToString(item.Cells["Remarks"].Value);
					Pd2.ActionTypeId = 3;
					lstRemoveRecord.Add(Pd2);
					dtDetail.Rows[item.RowIndex].Delete();
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
					grd.UpdateData();
				}
				else if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
			}
			else if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
			}
			TotalCommissionAmount();
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
			lstRemoveRecord.Clear();
			DocDate.Enabled = true;
			RecId = 0;
			((Control)(object)CmbCustomer).Text = string.Empty;
			txtDueDays.Text = string.Empty;
			txtDeliveryDays.Text = string.Empty;
			deliveryStartDate.Value = DocDate.Value;
			((Control)(object)cmbCommissionAgent).Text = string.Empty;
			((Control)(object)cmbCommType).Text = string.Empty;
			((Control)(object)cmbCommRateUom).Text = string.Empty;
			txtCommRate.Text = string.Empty;
			txtCommAmount.Text = string.Empty;
			txtCommRemarks.Text = string.Empty;
			((Control)(object)CmbOtherCommissionAgent).Text = string.Empty;
			((Control)(object)CmbOtherCommissionType).Text = string.Empty;
			((Control)(object)CmbOtherCommissionUom).Text = string.Empty;
			txtOtherCommissionRate.Text = string.Empty;
			txtOtherCommissionAmount.Text = string.Empty;
			txtOtherCommissionRemarks.Text = string.Empty;
			txtRemarks.Text = string.Empty;
			((Control)(object)CmbItem).Text = string.Empty;
			((Control)(object)cmbPackUom).Text = string.Empty;
			txtqty.Text = string.Empty;
			txtWeight.Text = string.Empty;
			txtItemRate.Text = string.Empty;
			((Control)(object)CmbRateUom).Text = string.Empty;
			txtItemAmount.Text = string.Empty;
			txtRemarksDetail.Text = string.Empty;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			btnSaveAs.Visible = false;
			btnAddDetail.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			GenerateDocNo();
			dtDetail.Clear();
			grd.DataSource = dtDetail;
			CmbCustomer.Focus();
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
			SuppCustomerFill(Conversion.ToInt(CmbCostCenter.Value));
			PaymentTerms();
			ItemFill();
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

	private void btnupdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				MessageBox.Show("Can't Update Record Because Id not Found", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else
			{
				Insert();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Expected O, but got Unknown
		try
		{
			if (grd.GetRows().Length == 0)
			{
				throw new Exception("Grid Record Not Found");
			}
			if (!FormValidation())
			{
				return;
			}
			PreBookingOrder po = new PreBookingOrder();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				po.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			po.DocumentTypeId = 129;
			po.OrganizationId = UserAccount.OrganizationId;
			po.CompanyId = UserAccount.CompanyId;
			po.BranchesId = UserAccount.BranchesId;
			po.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			po.EntryUser = UserAccount.ID;
			po.ModifyUser = UserAccount.ID;
			po.DocNo = Conversion.ToInt((object)txtdocno.Text);
			po.DocDate = DocDate.Value;
			po.OrderSupCustId = Conversion.ToInt(CmbCustomer.Value);
			po.RemarksHeader = Conversion.ToString((object)txtRemarks.Text);
			po.PaymentTermsId = Conversion.ToInt(CmbPaymentTerm.Value);
			po.OrderDueDays = Conversion.ToInt((object)txtDueDays.Text);
			po.OrderDueDate = DueDate.Value;
			po.DeliveryTerm = ((Control)(object)CmbDeliveryTerm).Text;
			po.DeliveryStartDate = deliveryStartDate.Value;
			po.DeliveryDays = Conversion.ToInt((object)txtDeliveryDays.Text);
			po.OrderExpiryDate = datExpiryDate.Value;
			int AgentId = Conversion.ToInt(cmbCommissionAgent.Value);
			double CommAmount = Conversion.ToDouble((object)txtCommAmount.Text.Trim());
			double CommRate = Conversion.ToDouble((object)txtCommRate.Text);
			if ((CommAmount > 0.0 || CommRate > 0.0) && AgentId == 0)
			{
				throw new Exception("Please Select Commission Agent when Commission Amount or Rate is Present...");
			}
			if (Conversion.ToInt(cmbCommissionAgent.Value) > 0 && (CommAmount == 0.0 || CommRate == 0.0))
			{
				if (CommAmount == 0.0)
				{
					throw new Exception("Commission Amount Required when Commission Agent is Selected...");
				}
				if (CommRate == 0.0)
				{
					throw new Exception("Commission Rate Required when Commission Agent is Selected...");
				}
			}
			if (((UltraGridBase)cmbCommissionAgent).ActiveRow != null)
			{
				po.BrokerAgentSupCustId = Conversion.ToInt(cmbCommissionAgent.Value);
				po.CommissionType = Conversion.ToString((object)((Control)(object)cmbCommType).Text);
				po.CommRate = Conversion.ToDouble((object)txtCommRate.Text);
				if (po.CommissionType == "Comm Weight")
				{
					po.UomScheduleIdCmRate = Conversion.ToInt((object)((Control)(object)cmbCommRateUom).Text);
				}
				else
				{
					po.UomScheduleIdCmRate = 0;
				}
				po.CommAmount = Conversion.ToDouble((object)txtCommAmount.Text.Trim());
				po.CommissionRemarks = Conversion.ToString((object)txtCommRemarks.Text);
			}
			int OtherAgentId = Conversion.ToInt(CmbOtherCommissionAgent.Value);
			double OtherCommAmount = Conversion.ToDouble((object)txtOtherCommissionAmount.Text.Trim());
			double OtherCommRate = Conversion.ToDouble((object)txtOtherCommissionRate.Text);
			if ((OtherCommAmount > 0.0 || OtherCommRate > 0.0) && OtherAgentId == 0)
			{
				throw new Exception("Please Select OtherCommission Agent when OtherCommission Amount or Rate is Present...");
			}
			if (Conversion.ToInt(CmbOtherCommissionAgent.Value) > 0 && (OtherCommAmount == 0.0 || OtherCommRate == 0.0))
			{
				if (OtherCommAmount == 0.0)
				{
					throw new Exception("OtherCommission Amount Required when OtherCommission Agent is Selected...");
				}
				if (OtherCommRate == 0.0)
				{
					throw new Exception("OtherCommission Rate Required when OtherCommission Agent is Selected...");
				}
			}
			if (((UltraGridBase)CmbOtherCommissionAgent).ActiveRow != null)
			{
				po.OtherCommissionAgentId = Conversion.ToInt(CmbOtherCommissionAgent.Value);
				po.OtherCommissionType = Conversion.ToString((object)((Control)(object)CmbOtherCommissionType).Text);
				po.OtherCommissionRate = Conversion.ToDouble((object)txtOtherCommissionRate.Text);
				if (po.OtherCommissionType == "Comm Weight")
				{
					po.OtherCommissionUom = Conversion.ToInt((object)((Control)(object)CmbOtherCommissionUom).Text);
				}
				else
				{
					po.OtherCommissionUom = 0.0;
				}
				po.OtherCommissionAmount = Conversion.ToDouble((object)txtOtherCommissionAmount.Text.Trim());
				po.OtherCommissionRemarks = Conversion.ToString((object)txtOtherCommissionRemarks.Text);
			}
			po.PreBookingOrderDetailList = new List<PreBookingOrderDetail>();
			if (RecId > 0 && lstRemoveRecord.Count > 0)
			{
				for (int i = 0; i < lstRemoveRecord.Count; i++)
				{
					po.PreBookingOrderDetailList.Add(lstRemoveRecord[i]);
				}
			}
			if (grd.RowCount > 0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					PreBookingOrderDetail vd = new PreBookingOrderDetail();
					vd.Id = ((RecId > 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
					vd.ActionTypeId = ((vd.Id <= 0) ? 1 : 2);
					vd.PriceScheduleId = Conversion.ToInt(r.Cells["ScheduleId"].Value);
					if (vd.PriceScheduleId == 0)
					{
						throw new Exception($"PriceScheduleId Field Required in Detail Grid Row No: {r.RowIndex + 1}");
					}
					vd.ItemPrice = Conversion.ToDouble(r.Cells["ItemRate"].Value);
					if (Conversion.ToInt(r.Cells["ItemId"].Value) > 0)
					{
						vd.OrderItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						if (Conversion.ToInt(r.Cells["PackUomId"].Value) > 0)
						{
							vd.OrderItemUOMId = Conversion.ToInt(r.Cells["PackUomId"].Value);
							if (Conversion.ToDouble(r.Cells["ItemQty"].Value) > 0.0)
							{
								vd.OrderItemQty = Conversion.ToDouble(r.Cells["ItemQty"].Value);
								if (Conversion.ToDouble(r.Cells["Weight"].Value) > 0.0)
								{
									vd.NetWeight = Conversion.ToDouble(r.Cells["Weight"].Value);
									if (Conversion.ToDouble(r.Cells["ItemRate"].Value) > 0.0)
									{
										vd.OrderItemRate = Conversion.ToDouble(r.Cells["ItemRate"].Value);
										vd.OrderItemRateUOMId = Conversion.ToInt(r.Cells["RateUomId"].Value);
										if (vd.OrderItemRateUOMId == 0)
										{
											throw new Exception($"RateUom Field Required in Detail Grid Row No: {r.RowIndex + 1}");
										}
										if (Conversion.ToDouble(r.Cells["ItemAmount"].Value) > 0.0)
										{
											vd.Amount = Conversion.ToDouble(r.Cells["ItemAmount"].Value);
											vd.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
											vd.Crop = Conversion.ToString(r.Cells["CropYear"].Value);
											vd.TotalAmount = vd.Amount;
											vd.OrderRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
											vd.CostCenterId = Conversion.ToInt(CmbCostCenter.Value);
											po.PreBookingOrderDetailList.Add(vd);
											continue;
										}
										throw new Exception($"Amount Field Required in Detail Grid Row No: {r.RowIndex + 1}");
									}
									throw new Exception($"Rate Field Required in Detail Grid Row No: {r.RowIndex + 1}");
								}
								throw new Exception($"Weight Field Required in Detail Grid Row No: {r.RowIndex + 1}");
							}
							throw new Exception($"Item Qty Field Required in Detail Grid Row No: {r.RowIndex + 1}");
						}
						throw new Exception($"Item UOM Field Required in Detail Grid Row No: {r.RowIndex + 1}");
					}
					throw new Exception($"Item Field Required in Detail Grid Row No: {r.RowIndex + 1}");
				}
				po.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				po.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<PreBookingOrder>(po, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
				int success = (po.Id = PreBookingOrder.Save(po));
				if (RecId > 0)
				{
					MessageBox.Show("Data Update Successfully....  " + po.DocNo);
					if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
					{
						global::Architecture.BLL.DMSAttachments.RemoveByIdAndNames(RecId, base.Name, 129, 0);
					}
				}
				else
				{
					MessageBox.Show("Data Save Successfully....  " + po.DocNo);
				}
				Reset();
				if (ChkBox.Checked)
				{
					CommonServices.PreBookingSlip(success);
				}
			}
			else
			{
				MessageBox.Show("Grid Record Not Found");
			}
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
			if (tabControl1.SelectedIndex == 0)
			{
				CmbCustomer.Focus();
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				FromDateHistory.Focus();
			}
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

	private void btnShowHistory_Click(object sender, EventArgs e)
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
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.AppId = UserAccount.AppId;
			obj.DocumentTypeId = 129;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.CanViewAllRecord = formRights.DoHaveCanViewAllRecordRights;
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
			obj.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			obj.CostCenterId = Conversion.ToInt(CmbCostCenter.Value);
			DataTable dthistoryfill = new DataTable();
			dthistoryfill = PreBookingOrder.FormHistory(obj);
			if (dthistoryfill.Rows.Count > 0)
			{
				if (dtHistoryData.Columns.Count == 0)
				{
					dtHistoryData.Columns.Add("Id");
					dtHistoryData.Columns.Add("DocNo", typeof(int));
					dtHistoryData.Columns.Add("DocDate", typeof(DateTime));
					dtHistoryData.Columns.Add("CustomerName");
					dtHistoryData.Columns.Add("PaymentTerm");
					dtHistoryData.Columns.Add("DueDays");
					dtHistoryData.Columns.Add("DueDate", typeof(DateTime));
					dtHistoryData.Columns.Add("DeliveryTerm");
					dtHistoryData.Columns.Add("DeliveryStartDate", typeof(DateTime));
					dtHistoryData.Columns.Add("DeliveryDays");
					dtHistoryData.Columns.Add("ExpiryDate", typeof(DateTime));
					dtHistoryData.Columns.Add("CommissionAgent");
					dtHistoryData.Columns.Add("CommType");
					dtHistoryData.Columns.Add("CommRate", typeof(double));
					dtHistoryData.Columns.Add("CommAmount", typeof(double));
					dtHistoryData.Columns.Add("OtherCommissionAgent");
					dtHistoryData.Columns.Add("OtherCommType");
					dtHistoryData.Columns.Add("OtherCommRate", typeof(double));
					dtHistoryData.Columns.Add("OtherCommAmount", typeof(double));
					dtHistoryData.Columns.Add("EntryUser");
					dtHistoryData.Columns.Add("EntryDate", typeof(DateTime));
					dtHistoryData.Columns.Add("ModifyUser");
					dtHistoryData.Columns.Add("ModifyDate", typeof(DateTime));
					dtHistoryData.Columns.Add("NoOfAttachments");
					dtHistoryData.Columns.Add("Remarks");
				}
				dtHistoryData.Clear();
				for (int i = 0; i < dthistoryfill.Rows.Count; i++)
				{
					dtHistoryData.Rows.Add(dthistoryfill.Rows[i]["Id"], dthistoryfill.Rows[i]["DocNo"], Conversion.ToDateTime(dthistoryfill.Rows[i]["DocDate"]).ToShortDateString(), dthistoryfill.Rows[i]["CustomerName"], dthistoryfill.Rows[i]["TermsDescription"], dthistoryfill.Rows[i]["DueDays"], Conversion.ToDateTime(dthistoryfill.Rows[i]["DueDate"]).ToShortDateString(), dthistoryfill.Rows[i]["DeliveryTerm"], Conversion.ToDateTime(dthistoryfill.Rows[i]["DeliveryStartDate"]).ToShortDateString(), dthistoryfill.Rows[i]["DeliveryDays"], Conversion.ToDateTime(dthistoryfill.Rows[i]["OrderExpiryDate"]).ToShortDateString(), dthistoryfill.Rows[i]["CommissionAgent"], dthistoryfill.Rows[i]["CommissionType"], dthistoryfill.Rows[i]["CommRate"], dthistoryfill.Rows[i]["CommAmount"], dthistoryfill.Rows[i]["OtherCommissionAgent"], dthistoryfill.Rows[i]["OtherCommissionType"], dthistoryfill.Rows[i]["OtherCommissionRate"], dthistoryfill.Rows[i]["OtherCommissionAmount"], dthistoryfill.Rows[i]["UserName"], dthistoryfill.Rows[i]["EntryDate"], dthistoryfill.Rows[i]["ModifyUserName"], dthistoryfill.Rows[i]["ModifyDate"], dthistoryfill.Rows[i]["NoOfAttachments"], dthistoryfill.Rows[i]["RemarksHeader"]);
				}
				DataGridHistory.DataSource = dtHistoryData;
				DataGridHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				DataGridHistory.ClearStructure();
				GridDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSettings()
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)DataGridHistory.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			DataGridHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			DataGridHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			DataGridHistory.RootTable.Columns["CustomerName"].Width = Constants.InventoryConstants.SupplierName;
			DataGridHistory.RootTable.Columns["PaymentTerm"].Width = Constants.InventoryConstants.TermConstant;
			DataGridHistory.RootTable.Columns["DueDays"].Width = Constants.InventoryConstants.DueDays;
			DataGridHistory.RootTable.Columns["DueDate"].Width = Constants.InventoryConstants.DateConstant;
			DataGridHistory.RootTable.Columns["DeliveryTerm"].Width = Constants.InventoryConstants.TermConstant;
			DataGridHistory.RootTable.Columns["DeliveryDays"].Width = Constants.InventoryConstants.DueDays;
			DataGridHistory.RootTable.Columns["DeliveryStartDate"].Width = Constants.InventoryConstants.DateConstant;
			DataGridHistory.RootTable.Columns["ExpiryDate"].Width = Constants.InventoryConstants.DateConstant;
			DataGridHistory.RootTable.Columns["CommissionAgent"].Width = Constants.InventoryConstants.CommissionAgent;
			DataGridHistory.RootTable.Columns["CommType"].Width = Constants.InventoryConstants.DiscountType;
			DataGridHistory.RootTable.Columns["CommRate"].Width = Constants.InventoryConstants.ExRateConstant;
			DataGridHistory.RootTable.Columns["CommAmount"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["OtherCommissionAgent"].Width = Constants.InventoryConstants.CommissionAgent;
			DataGridHistory.RootTable.Columns["OtherCommType"].Width = Constants.InventoryConstants.DiscountType;
			DataGridHistory.RootTable.Columns["OtherCommRate"].Width = Constants.InventoryConstants.ExRateConstant;
			DataGridHistory.RootTable.Columns["OtherCommAmount"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			DataGridHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			DataGridHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			DataGridHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			DataGridHistory.RootTable.Columns["Remarks"].Width = 250;
			DataGridHistory.RootTable.Columns["OtherCommRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["OtherCommRate"].FormatString = "#,##0.####";
			DataGridHistory.RootTable.Columns["OtherCommAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["OtherCommAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["OtherCommAmount"].FormatString = "#,##0.####";
			DataGridHistory.RootTable.Columns["OtherCommAmount"].TotalFormatString = "#,##0.####";
			DataGridHistory.RootTable.Columns["CommRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["CommRate"].FormatString = "#,##0.####";
			DataGridHistory.RootTable.Columns["CommAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["CommAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["CommAmount"].FormatString = "#,##0.####";
			DataGridHistory.RootTable.Columns["CommAmount"].TotalFormatString = "#,##0.####";
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yy hh:mm tt";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yy hh:mm tt";
			DataGridHistory.RootTable.Columns.Add("Edit");
			DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Width = 40;
			DataGridHistory.RootTable.Columns.Add("Print");
			DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridHistory.RootTable.Columns["Print"].Width = 50;
			DataGridHistory.RootTable.Columns.Add("SaveAs");
			DataGridHistory.RootTable.Columns["SaveAs"].Caption = "SaveAs";
			DataGridHistory.RootTable.Columns["SaveAs"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["SaveAs"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["SaveAs"].ButtonText = "SaveAs";
			DataGridHistory.RootTable.Columns["SaveAs"].Width = 60;
			DataGridHistory.RootTable.Columns["Edit"].Position = 0;
			DataGridHistory.RootTable.Columns["SaveAs"].Position = 1;
			DataGridHistory.RootTable.Columns["Print"].Position = 2;
			DataGridHistory.FrozenColumns = 3;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int ID)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			lstRemoveRecord.Clear();
			PreBookingOrder po = new PreBookingOrder();
			RecId = Conversion.ToInt((object)ID);
			po = PreBookingOrder.GetByID(RecId);
			tabControl1.SelectedIndex = 0;
			DocDate.Value = po.DocDate;
			txtdocno.Text = Conversion.ToString((object)po.DocNo);
			int costCenterId = ((po.PreBookingOrderDetailList != null && po.PreBookingOrderDetailList.Count > 0) ? po.PreBookingOrderDetailList[0].CostCenterId : 0);
			if (costCenterId > 0)
			{
				CmbCostCenter.Value = costCenterId;
			}
			CmbCustomer.Value = po.OrderSupCustId;
			CmbPaymentTerm.Value = Conversion.ToInt((object)po.PaymentTermsId);
			txtDueDays.Text = Conversion.ToString((object)po.OrderDueDays);
			DueDate.Value = po.OrderDueDate;
			((Control)(object)CmbDeliveryTerm).Text = Conversion.ToString((object)po.DeliveryTerm);
			deliveryStartDate.Value = po.DeliveryStartDate;
			txtDeliveryDays.Text = Conversion.ToString((object)po.DeliveryDays);
			datExpiryDate.Value = po.OrderExpiryDate;
			if (po.BrokerAgentSupCustId > 0)
			{
				cmbCommissionAgent.Value = Conversion.ToInt((object)po.BrokerAgentSupCustId);
			}
			((Control)(object)cmbCommType).Text = Conversion.ToString((object)po.CommissionType);
			txtCommRate.Text = Conversion.ToString((object)po.CommRate);
			if (po.UomScheduleIdCmRate > 0)
			{
				((Control)(object)cmbCommRateUom).Text = Conversion.ToString((object)po.UomScheduleIdCmRate);
			}
			txtCommAmount.Text = Conversion.ToString((object)po.CommAmount);
			txtCommRemarks.Text = Conversion.ToString((object)po.CommissionRemarks);
			if (po.OtherCommissionAgentId > 0)
			{
				CmbOtherCommissionAgent.Value = Conversion.ToInt((object)po.OtherCommissionAgentId);
			}
			((Control)(object)CmbOtherCommissionType).Text = Conversion.ToString((object)po.OtherCommissionType);
			txtOtherCommissionRate.Text = Conversion.ToString((object)po.OtherCommissionRate);
			if (po.OtherCommissionUom > 0.0)
			{
				((Control)(object)CmbOtherCommissionUom).Text = Conversion.ToString((object)po.OtherCommissionUom);
			}
			txtOtherCommissionAmount.Text = Conversion.ToString((object)po.OtherCommissionAmount);
			txtOtherCommissionRemarks.Text = Conversion.ToString((object)po.OtherCommissionRemarks);
			txtRemarks.Text = po.RemarksHeader;
			dtDetail.Clear();
			DocDate.Enabled = false;
			for (int i = 0; i < po.PreBookingOrderDetailList.Count; i++)
			{
				dtDetail.Rows.Add(po.PreBookingOrderDetailList[i].Id, po.PreBookingOrderDetailList[i].PriceScheduleId, po.PreBookingOrderDetailList[i].OrderItemId, po.PreBookingOrderDetailList[i].ItemName, po.PreBookingOrderDetailList[i].CropYearId, po.PreBookingOrderDetailList[i].Crop, po.PreBookingOrderDetailList[i].OrderItemUOMId, po.PreBookingOrderDetailList[i].UOMCode, po.PreBookingOrderDetailList[i].OrderItemQty, po.PreBookingOrderDetailList[i].NetWeight, po.PreBookingOrderDetailList[i].OrderItemRate, po.PreBookingOrderDetailList[i].OrderItemRateUOMId, po.PreBookingOrderDetailList[i].RateUom, po.PreBookingOrderDetailList[i].Amount, po.PreBookingOrderDetailList[i].OrderRemarks);
			}
			grd.DataSource = dtDetail;
			grd.RetrieveStructure();
			grdSetting();
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			btnsave.Visible = false;
			btnSaveAs.Visible = false;
			btnupdate.Visible = true;
			TotalCommissionAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				Reset();
				RecId = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(RecId);
			}
			if (e.Column.Key == "Print")
			{
				CommonServices.PreBookingSlip(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
			}
			if (e.Column.Key == "SaveAs")
			{
				Reset();
				RecId = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value);
				ReadById(RecId);
				btnSaveAs.Visible = true;
				btnsave.Visible = false;
				btnupdate.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 129);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (DataGridHistory.CurrentRow != null && DataGridHistory.CurrentRow.Cells.Count > 0 && (int)DataGridHistory.CurrentRow.RowType == 0)
			{
				GridDetailBind(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailBind(int Id)
	{
		try
		{
			PreBookingOrder po = PreBookingOrder.GetByID(Id);
			if (po != null)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("ItemName");
				dt.Columns.Add("PackUom");
				dt.Columns.Add("ItemQty", typeof(double));
				dt.Columns.Add("Weight", typeof(double));
				dt.Columns.Add("ItemRate", typeof(double));
				dt.Columns.Add("RateUom");
				dt.Columns.Add("ItemAmount", typeof(double));
				dt.Columns.Add("Remarks");
				for (int i = 0; i < po.PreBookingOrderDetailList.Count; i++)
				{
					dt.Rows.Add(po.PreBookingOrderDetailList[i].ItemName, po.PreBookingOrderDetailList[i].UOMCode, po.PreBookingOrderDetailList[i].OrderItemQty, po.PreBookingOrderDetailList[i].NetWeight, po.PreBookingOrderDetailList[i].OrderItemRate, po.PreBookingOrderDetailList[i].RateUom, po.PreBookingOrderDetailList[i].Amount, po.PreBookingOrderDetailList[i].OrderRemarks);
				}
				GridDetail.DataSource = dt;
				GridDetail.RetrieveStructure();
				GridDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
				GridDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
				GridDetail.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
				GridDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
				GridDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
				GridDetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
				GridDetail.RootTable.Columns["Weight"].FormatString = "#,##0.###";
				GridDetail.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
				GridDetail.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
				GridDetail.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
				GridDetail.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
				GridDetail.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
				GridDetail.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
				GridDetail.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
				CommonServices.GridAutoAdjustmentNew(GridDetail);
			}
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
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			ToDateHistory.Value = DateTime.Now;
			FromDocNoHistory.Clear();
			ToDocNoHistory.Clear();
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			DataGridHistory.ClearStructure();
			GridDetail.ClearStructure();
			drdocdate.Checked = true;
			FromDateHistory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryCombosFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grd, dtDetail);
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
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, DataGridHistory, dtHistoryData);
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

	private void cmbPackUom_TextChanged(object sender, EventArgs e)
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
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateWeight()
	{
		try
		{
			if (((UltraGridBase)cmbPackUom).ActiveRow != null && Conversion.ToInt(cmbPackUom.Value) > 0 && Conversion.ToDouble((object)txtqty.Text) > 0.0)
			{
				double num = Conversion.ToDouble(((UltraDropDownBase)cmbPackUom).SelectedRow.Cells[2].Value);
				double Qty = Conversion.ToDouble((object)txtqty.Text);
				txtWeight.Text = (num * Qty).ToString("#,##0.###");
			}
			else
			{
				txtWeight.Text = "0";
			}
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
			CalculateAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbRateUom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtItemRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateAmount()
	{
		try
		{
			if (Conversion.ToInt(CmbRateUom.Value) > 0 && Conversion.ToDouble((object)txtItemRate.Text) > 0.0 && Conversion.ToDouble((object)txtWeight.Text) > 0.0)
			{
				double RateUom = Conversion.ToDouble(((UltraDropDownBase)CmbRateUom).SelectedRow.Cells[2].Value);
				double num = Conversion.ToDouble((object)txtWeight.Text);
				double Rate = Conversion.ToDouble((object)txtItemRate.Text);
				double Amount = num / RateUom * Rate;
				txtItemAmount.Text = Amount.ToString(clsGlobalVariables.stringFormatsingle);
			}
			else
			{
				txtItemAmount.Text = "0";
			}
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
			if (RecId > 0)
			{
				CommonServices.PreBookingSlip(RecId);
			}
			else
			{
				MessageBox.Show("No Record Found for Display");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = DataGridHistory.CurrentRow;
			if (row != null && (int)row.RowType == 0)
			{
				Reset();
				RecId = Conversion.ToInt(row.Cells["Id"].Value);
				ReadById(RecId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || DataGridHistory.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (DataGridHistory.CurrentColumn.Key == "Edit")
				{
					if (formRights.DoHaveUpdateRights)
					{
						RecId = Conversion.ToInt(item.Cells["Id"].Value);
						ReadById(RecId);
					}
				}
				else if (DataGridHistory.CurrentColumn.Key == "SaveAs")
				{
					Reset();
					RecId = Conversion.ToInt(item.Cells["Id"].Value);
					ReadById(RecId);
					btnSaveAs.Visible = true;
					btnsave.Visible = false;
					btnupdate.Visible = false;
				}
				else if (DataGridHistory.CurrentColumn.Key == "Print")
				{
					CommonServices.PreBookingSlip(Conversion.ToInt(item.Cells["Id"].Value));
				}
				else if (DataGridHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 129);
				}
			}
			else if (e.Control && e.KeyCode == Keys.Return && formRights.DoHaveUpdateRights)
			{
				RecId = Conversion.ToInt(item.Cells["Id"].Value);
				ReadById(RecId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PreBookingOrder_FormClosed(object sender, FormClosedEventArgs e)
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

	private void PreBookingOrder_KeyDown(object sender, KeyEventArgs e)
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
					DocDate.Focus();
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
				if (e.Control && e.KeyCode == Keys.S)
				{
					if (btnsave.Visible && btnsave.Enabled)
					{
						btnsave_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.U)
				{
					if (btnupdate.Visible && btnupdate.Enabled)
					{
						btnupdate_Click(null, null);
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
					DocDate.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grd).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					CmbItem.Focus();
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
				else if (e.Control && e.KeyCode == Keys.P)
				{
					btnPrint_Click(null, null);
				}
				return;
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnResetHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefreshHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnShowHistory_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)DataGridHistory).Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)GridDetail).Focused)
				{
					((Control)(object)DataGridHistory).Focus();
				}
				else
				{
					((Control)(object)GridDetail).Focus();
				}
			}
			else if (e.Control && e.KeyCode == Keys.Up)
			{
				FromDateHistory.Focus();
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
			dt.Rows.Add("Ctrl+S", "For Save when in Form Tab And For History When in History Tab");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+F12", "For Save As");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus on Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Item Name in Detail Box");
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

	private void CmbCropyr_Leave(object sender, EventArgs e)
	{
		try
		{
			getRateRateUom();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCostCenter_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt((object)CmbCostCenter) <= 0 && dtSupplierCustomer.Rows.Count > 0)
			{
				DDL.BindDDL(dtSupplierCustomer, CmbCustomer, "Id", "CompanyName", "Customer Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DueDaysCalculation()
	{
		try
		{
			if (txtDueDays.Text.Trim() != string.Empty && txtDueDays.Text != null)
			{
				DueDate.Value = DocDate.Value.AddDays(Conversion.ToDouble((object)txtDueDays.Text.Trim()));
			}
			else
			{
				DueDate.Value = DocDate.Value;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CalculateExpiryDate()
	{
		try
		{
			if (Conversion.ToInt((object)txtDeliveryDays.Text) == 0)
			{
				datExpiryDate.Value = deliveryStartDate.Value;
			}
			else
			{
				datExpiryDate.Value = deliveryStartDate.Value.AddDays(Conversion.ToDouble((object)txtDeliveryDays.Text.Trim()));
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void TotalCommissionAmount()
	{
		try
		{
			if (txtCommRate.Text != string.Empty)
			{
				if (((Control)(object)cmbCommType).Text == "Flat")
				{
					double CommRate = Conversion.ToDouble((object)txtCommRate.Text.Trim());
					txtCommAmount.Text = CommRate.ToString("#,##0.###");
				}
				else if (((Control)(object)cmbCommType).Text == "Percent")
				{
					double CommRate3 = Conversion.ToDouble((object)txtCommRate.Text.Trim());
					object total = grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2);
					if (total != null)
					{
						double CommAmt = Conversion.ToDouble((object)total.ToString()) * CommRate3 / 100.0;
						txtCommAmount.Text = Math.Round(CommAmt, 0, MidpointRounding.AwayFromZero).ToString("#,##0.###");
					}
					else
					{
						txtCommAmount.Text = 0.ToString("#,##0.###");
					}
				}
				else if (((Control)(object)cmbCommType).Text == "Comm Weight")
				{
					double CommRate2 = Conversion.ToDouble((object)txtCommRate.Text.Trim());
					double UOM = Conversion.ToDouble((object)((Control)(object)cmbCommRateUom).Text.Trim());
					double Weight = Conversion.ToDouble(grd.GetTotal(grd.RootTable.Columns["Weight"], (AggregateFunction)2));
					if (Weight > 0.0 && CommRate2 > 0.0 && UOM > 0.0)
					{
						double CommAmt2 = Weight / UOM * CommRate2;
						txtCommAmount.Text = Math.Round(CommAmt2, 0, MidpointRounding.AwayFromZero).ToString("#,##0.###");
					}
					else
					{
						txtCommAmount.Text = 0.ToString("#,##0.###");
					}
				}
			}
			else
			{
				txtCommAmount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void txtDueDays_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtDueDays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DueDaysCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDeliveryDays_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtCommRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void deliveryStartDate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (deliveryStartDate.Value.Date < DocDate.Value.Date)
			{
				deliveryStartDate.Value = DocDate.Value;
				MessageBox.Show("Delivery Start Date Can't Be Less Than Doc Date");
			}
			CalculateExpiryDate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDeliveryDays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateExpiryDate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbCommType_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			TotalCommissionAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtCommRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalCommissionAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbCommRateUom_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			TotalCommissionAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtOtherCommissionRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void CmbOtherCommissionUom_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			TotalOtherCommissionAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalOtherCommissionAmount()
	{
		try
		{
			if (txtOtherCommissionRate.Text != string.Empty)
			{
				if (((Control)(object)CmbOtherCommissionType).Text == "Flat")
				{
					double CommRate = Conversion.ToDouble((object)txtOtherCommissionRate.Text.Trim());
					txtOtherCommissionAmount.Text = CommRate.ToString("#,##0.###");
				}
				else if (((Control)(object)CmbOtherCommissionType).Text == "Percent")
				{
					double CommRate3 = Conversion.ToDouble((object)txtOtherCommissionRate.Text.Trim());
					object total = grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2);
					if (total != null)
					{
						double CommAmt = Conversion.ToDouble((object)total.ToString()) * CommRate3 / 100.0;
						txtOtherCommissionAmount.Text = Math.Round(CommAmt, 0, MidpointRounding.AwayFromZero).ToString("#,##0.###");
					}
					else
					{
						txtOtherCommissionAmount.Text = 0.ToString("#,##0.###");
					}
				}
				else if (((Control)(object)CmbOtherCommissionType).Text == "Comm Weight")
				{
					double CommRate2 = Conversion.ToDouble((object)txtOtherCommissionRate.Text.Trim());
					double UOM = Conversion.ToDouble((object)((Control)(object)CmbOtherCommissionUom).Text.Trim());
					double Weight = Conversion.ToDouble(grd.GetTotal(grd.RootTable.Columns["Weight"], (AggregateFunction)2));
					if (Weight > 0.0 && CommRate2 > 0.0 && UOM > 0.0)
					{
						double CommAmt2 = Weight / UOM * CommRate2;
						txtOtherCommissionAmount.Text = Math.Round(CommAmt2, 0, MidpointRounding.AwayFromZero).ToString("#,##0.###");
					}
					else
					{
						txtOtherCommissionAmount.Text = 0.ToString("#,##0.###");
					}
				}
			}
			else
			{
				txtOtherCommissionAmount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbOtherCommissionType_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			TotalOtherCommissionAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtOtherCommissionRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalOtherCommissionAmount();
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
		//IL_05ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b8: Expected O, but got Unknown
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d9: Expected O, but got Unknown
		//IL_0632: Unknown result type (might be due to invalid IL or missing references)
		//IL_063c: Expected O, but got Unknown
		//IL_0674: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Expected O, but got Unknown
		//IL_068a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0694: Expected O, but got Unknown
		//IL_06a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06aa: Expected O, but got Unknown
		//IL_06f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0702: Expected O, but got Unknown
		//IL_075b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0765: Expected O, but got Unknown
		//IL_07b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bd: Expected O, but got Unknown
		//IL_07c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d3: Expected O, but got Unknown
		//IL_07df: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e9: Expected O, but got Unknown
		//IL_0842: Unknown result type (might be due to invalid IL or missing references)
		//IL_084c: Expected O, but got Unknown
		//IL_088f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0899: Expected O, but got Unknown
		//IL_089a: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a4: Expected O, but got Unknown
		//IL_08dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e6: Expected O, but got Unknown
		//IL_08fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0907: Expected O, but got Unknown
		//IL_093f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0949: Expected O, but got Unknown
		//IL_09ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d8: Expected O, but got Unknown
		//IL_5869: Unknown result type (might be due to invalid IL or missing references)
		//IL_5873: Expected O, but got Unknown
		//IL_784c: Unknown result type (might be due to invalid IL or missing references)
		//IL_7856: Expected O, but got Unknown
		//IL_787a: Unknown result type (might be due to invalid IL or missing references)
		//IL_7884: Expected O, but got Unknown
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
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
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance15 = new Appearance();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GridDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnSaveAs = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.label23 = new System.Windows.Forms.Label();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label1 = new System.Windows.Forms.Label();
		this.datExpiryDate = new System.Windows.Forms.DateTimePicker();
		this.txtDeliveryDays = new System.Windows.Forms.TextBox();
		this.CmbDeliveryTerm = new UltraCombo();
		this.label30 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.CmbPaymentTerm = new UltraCombo();
		this.deliveryStartDate = new System.Windows.Forms.DateTimePicker();
		this.label32 = new System.Windows.Forms.Label();
		this.DueDate = new System.Windows.Forms.DateTimePicker();
		this.txtDueDays = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.CmbCustomer = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.label17 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.txtCommRate = new System.Windows.Forms.TextBox();
		this.cmbCommRateUom = new UltraCombo();
		this.txtCommAmount = new System.Windows.Forms.TextBox();
		this.cmbCommType = new UltraCombo();
		this.txtCommRemarks = new System.Windows.Forms.TextBox();
		this.cmbCommissionAgent = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.CmbCropyr = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.btnAddDetail = new System.Windows.Forms.Button();
		this.label7 = new System.Windows.Forms.Label();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.txtItemAmount = new System.Windows.Forms.TextBox();
		this.txtRemarksDetail = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.CmbRateUom = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.txtItemRate = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.txtWeight = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtqty = new System.Windows.Forms.TextBox();
		this.label38 = new System.Windows.Forms.Label();
		this.cmbPackUom = new UltraCombo();
		this.label36 = new System.Windows.Forms.Label();
		this.CmbItem = new UltraCombo();
		this.label37 = new System.Windows.Forms.Label();
		this.grd = new GridEX();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.CmbOtherCommissionAgent = new UltraCombo();
		this.label15 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.txtOtherCommissionRate = new System.Windows.Forms.TextBox();
		this.txtOtherCommissionRemarks = new System.Windows.Forms.TextBox();
		this.label18 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.CmbOtherCommissionUom = new UltraCombo();
		this.CmbOtherCommissionType = new UltraCombo();
		this.label27 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.txtOtherCommissionAmount = new System.Windows.Forms.TextBox();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.CmbCostCenter = new UltraCombo();
		this.lblCostCenter = new System.Windows.Forms.Label();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.DataGridHistory = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label8 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label10 = new System.Windows.Forms.Label();
		this.GridDetail = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.label50 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.toolStrip1.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCommRateUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCommType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCommissionAgent).BeginInit();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCropyr).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOtherCommissionAgent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOtherCommissionUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOtherCommissionType).BeginInit();
		this.groupBox4.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCostCenter).BeginInit();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel8.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetail).BeginInit();
		this.panel4.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.btnnew, this.btnRefresh, this.btnsave, this.btnSaveAs, this.btnupdate, this.btnDelete, this.btnattachment, this.btnPrint, this.btnshortcutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(3, 3);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1170, 27);
		this.toolStrip1.TabIndex = 1;
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
		this.btnSaveAs.Size = new System.Drawing.Size(85, 24);
		this.btnSaveAs.Text = "Save As";
		this.btnSaveAs.Visible = false;
		this.btnSaveAs.Click += new System.EventHandler(btnSaveAs_Click);
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnattachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(92, 24);
		this.btnPrint.Text = "274-Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.Teal;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(3, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(159, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "Pre Booking Order";
		this.ChkBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBox.AutoSize = true;
		this.ChkBox.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBox.ForeColor = System.Drawing.Color.White;
		this.ChkBox.Location = new System.Drawing.Point(1066, 6);
		this.ChkBox.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(102, 19);
		this.ChkBox.TabIndex = 20;
		this.ChkBox.Text = "Print Preview";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.datExpiryDate);
		this.groupBox1.Controls.Add(this.txtDeliveryDays);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentTerm);
		this.groupBox1.Controls.Add(this.deliveryStartDate);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add(this.DueDate);
		this.groupBox1.Controls.Add(this.txtDueDays);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.txtRemarks);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomer);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.DocDate);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.txtdocno);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(505, 149);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main Information";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(297, 100);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(67, 15);
		this.label1.TabIndex = 45;
		this.label1.Text = "Expiry Date";
		this.datExpiryDate.CustomFormat = "dd-MMM-yy";
		this.datExpiryDate.Enabled = false;
		this.datExpiryDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datExpiryDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datExpiryDate.Location = new System.Drawing.Point(389, 96);
		this.datExpiryDate.Name = "datExpiryDate";
		this.datExpiryDate.Size = new System.Drawing.Size(110, 22);
		this.datExpiryDate.TabIndex = 10;
		this.txtDeliveryDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDeliveryDays.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDeliveryDays.Location = new System.Drawing.Point(389, 71);
		this.txtDeliveryDays.Name = "txtDeliveryDays";
		this.txtDeliveryDays.Size = new System.Drawing.Size(110, 23);
		this.txtDeliveryDays.TabIndex = 9;
		this.txtDeliveryDays.TextChanged += new System.EventHandler(txtDeliveryDays_TextChanged);
		this.txtDeliveryDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDeliveryDays_KeyPress);
		this.CmbDeliveryTerm.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance73;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance78).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance79).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance81).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbDeliveryTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Location = new System.Drawing.Point(389, 15);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Name = "CmbDeliveryTerm";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Size = new System.Drawing.Size(110, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).TabIndex = 7;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label30.Location = new System.Drawing.Point(297, 21);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(80, 15);
		this.label30.TabIndex = 26;
		this.label30.Text = "Delivery Term";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label28.Location = new System.Drawing.Point(297, 48);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(88, 15);
		this.label28.TabIndex = 21;
		this.label28.Text = "Delivery S.Date";
		this.CmbPaymentTerm.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance139).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance139).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance140;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance109).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance147).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPaymentTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Location = new System.Drawing.Point(106, 69);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Name = "CmbPaymentTerm";
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Size = new System.Drawing.Size(189, 26);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).TabIndex = 3;
		this.deliveryStartDate.CustomFormat = "dd-MMM-yy";
		this.deliveryStartDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.deliveryStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.deliveryStartDate.Location = new System.Drawing.Point(389, 44);
		this.deliveryStartDate.Name = "deliveryStartDate";
		this.deliveryStartDate.Size = new System.Drawing.Size(110, 22);
		this.deliveryStartDate.TabIndex = 8;
		this.deliveryStartDate.ValueChanged += new System.EventHandler(deliveryStartDate_ValueChanged);
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label32.Location = new System.Drawing.Point(297, 75);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(79, 15);
		this.label32.TabIndex = 24;
		this.label32.Text = "Delivery Days";
		this.DueDate.CustomFormat = "dd-MMM-yy";
		this.DueDate.Enabled = false;
		this.DueDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DueDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DueDate.Location = new System.Drawing.Point(185, 96);
		this.DueDate.Name = "DueDate";
		this.DueDate.Size = new System.Drawing.Size(110, 22);
		this.DueDate.TabIndex = 5;
		this.txtDueDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDueDays.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDueDays.Location = new System.Drawing.Point(106, 96);
		this.txtDueDays.Name = "txtDueDays";
		this.txtDueDays.Size = new System.Drawing.Size(78, 23);
		this.txtDueDays.TabIndex = 4;
		this.txtDueDays.TextChanged += new System.EventHandler(txtDueDays_TextChanged);
		this.txtDueDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDueDays_KeyPress);
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(6, 100);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(98, 15);
		this.label24.TabIndex = 22;
		this.label24.Text = "Due Days && Date";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label26.Location = new System.Drawing.Point(6, 75);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(89, 15);
		this.label26.TabIndex = 28;
		this.label26.Text = "Payment Terms";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(6, 124);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(52, 15);
		this.label2.TabIndex = 20;
		this.label2.Text = "Remarks";
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(106, 120);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(393, 23);
		this.txtRemarks.TabIndex = 6;
		this.CmbCustomer.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance113;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance119).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Location = new System.Drawing.Point(106, 42);
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Name = "CmbCustomer";
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Size = new System.Drawing.Size(189, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomer).TabIndex = 2;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(6, 48);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(93, 15);
		this.label12.TabIndex = 5;
		this.label12.Text = "Customer Name";
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(185, 17);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(110, 22);
		this.DocDate.TabIndex = 1;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(6, 21);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(96, 15);
		this.label17.TabIndex = 6;
		this.label17.Text = "Order No && Date";
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Enabled = false;
		this.txtdocno.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(107, 17);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.Size = new System.Drawing.Size(77, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.TabStop = false;
		this.txtCommRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCommRate.Location = new System.Drawing.Point(249, 42);
		this.txtCommRate.Name = "txtCommRate";
		this.txtCommRate.Size = new System.Drawing.Size(80, 23);
		this.txtCommRate.TabIndex = 2;
		this.txtCommRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCommRate.TextChanged += new System.EventHandler(txtCommRate_TextChanged);
		this.txtCommRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCommRate_KeyPress);
		this.cmbCommRateUom.AllowNull = (DefaultableBoolean)1;
		this.cmbCommRateUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCommRateUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCommRateUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCommRateUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbCommRateUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCommRateUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCommRateUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCommRateUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance36;
		((SpecialBoxBase)((UltraGridBase)this.cmbCommRateUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCommRateUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbCommRateUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCommRateUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCommRateUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCommRateUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbCommRateUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCommRateUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCommRateUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance42).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCommRateUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbCommRateUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCommRateUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance43).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCommRateUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCommRateUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbCommRateUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCommRateUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCommRateUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbCommRateUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCommRateUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbCommRateUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCommRateUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCommRateUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCommRateUom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCommRateUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCommRateUom).Location = new System.Drawing.Point(103, 67);
		((System.Windows.Forms.Control)(object)this.cmbCommRateUom).Name = "cmbCommRateUom";
		((System.Windows.Forms.Control)(object)this.cmbCommRateUom).Size = new System.Drawing.Size(91, 26);
		((System.Windows.Forms.Control)(object)this.cmbCommRateUom).TabIndex = 3;
		this.cmbCommRateUom.ValueChanged += new System.EventHandler(cmbCommRateUom_ValueChanged);
		this.txtCommAmount.BackColor = System.Drawing.Color.White;
		this.txtCommAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommAmount.Enabled = false;
		this.txtCommAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCommAmount.Location = new System.Drawing.Point(249, 69);
		this.txtCommAmount.Name = "txtCommAmount";
		this.txtCommAmount.Size = new System.Drawing.Size(80, 23);
		this.txtCommAmount.TabIndex = 4;
		this.txtCommAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.cmbCommType.AllowNull = (DefaultableBoolean)1;
		this.cmbCommType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCommType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCommType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCommType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbCommType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCommType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCommType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCommType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance50;
		((SpecialBoxBase)((UltraGridBase)this.cmbCommType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance51).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance51).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCommType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbCommType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCommType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCommType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCommType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbCommType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCommType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCommType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance55).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCommType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbCommType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCommType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance84).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCommType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCommType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbCommType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCommType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCommType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbCommType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCommType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbCommType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCommType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCommType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCommType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCommType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCommType).Location = new System.Drawing.Point(103, 40);
		((System.Windows.Forms.Control)(object)this.cmbCommType).Name = "cmbCommType";
		((System.Windows.Forms.Control)(object)this.cmbCommType).Size = new System.Drawing.Size(91, 26);
		((System.Windows.Forms.Control)(object)this.cmbCommType).TabIndex = 1;
		this.cmbCommType.ValueChanged += new System.EventHandler(cmbCommType_ValueChanged);
		this.txtCommRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommRemarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCommRemarks.Location = new System.Drawing.Point(103, 94);
		this.txtCommRemarks.Multiline = true;
		this.txtCommRemarks.Name = "txtCommRemarks";
		this.txtCommRemarks.Size = new System.Drawing.Size(226, 49);
		this.txtCommRemarks.TabIndex = 5;
		this.cmbCommissionAgent.AllowNull = (DefaultableBoolean)1;
		this.cmbCommissionAgent.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCommissionAgent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCommissionAgent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance90;
		((SpecialBoxBase)((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance97).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance98).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance98).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance98).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCommissionAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCommissionAgent).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCommissionAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCommissionAgent).Location = new System.Drawing.Point(103, 13);
		((System.Windows.Forms.Control)(object)this.cmbCommissionAgent).Name = "cmbCommissionAgent";
		((System.Windows.Forms.Control)(object)this.cmbCommissionAgent).Size = new System.Drawing.Size(226, 26);
		((System.Windows.Forms.Control)(object)this.cmbCommissionAgent).TabIndex = 0;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label20.Location = new System.Drawing.Point(6, 46);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(71, 15);
		this.label20.TabIndex = 36;
		this.label20.Text = "Comm Type";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(6, 73);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(72, 15);
		this.label21.TabIndex = 38;
		this.label21.Text = "Comm Uom";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(6, 98);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(91, 15);
		this.label25.TabIndex = 37;
		this.label25.Text = "Comm Remarks";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label19.Location = new System.Drawing.Point(197, 73);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(51, 15);
		this.label19.TabIndex = 33;
		this.label19.Text = "Amount";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(197, 46);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(30, 15);
		this.label13.TabIndex = 34;
		this.label13.Text = "Rate";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(6, 19);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(98, 15);
		this.label14.TabIndex = 35;
		this.label14.Text = "Sales Man/Agent";
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropyr);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add(this.btnAddDetail);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox2.Controls.Add(this.btnUpdateDetail);
		this.groupBox2.Controls.Add(this.txtItemAmount);
		this.groupBox2.Controls.Add(this.txtRemarksDetail);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRateUom);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add(this.txtItemRate);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add(this.txtWeight);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Controls.Add(this.txtqty);
		this.groupBox2.Controls.Add(this.label38);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackUom);
		this.groupBox2.Controls.Add(this.label36);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItem);
		this.groupBox2.Controls.Add(this.label37);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.groupBox2.Location = new System.Drawing.Point(3, 3);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1164, 64);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Detail Information";
		this.CmbCropyr.AllowNull = (DefaultableBoolean)1;
		this.CmbCropyr.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropyr.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropyr.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Appearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropyr).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropyr).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance13).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCropyr.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Location = new System.Drawing.Point(227, 33);
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Name = "CmbCropyr";
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Size = new System.Drawing.Size(102, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropyr).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Leave += new System.EventHandler(CmbCropyr_Leave);
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(231, 14);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(57, 15);
		this.label11.TabIndex = 27;
		this.label11.Text = "Crop Year";
		this.btnAddDetail.BackColor = System.Drawing.Color.Teal;
		this.btnAddDetail.FlatAppearance.BorderSize = 0;
		this.btnAddDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddDetail.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddDetail.ForeColor = System.Drawing.Color.White;
		this.btnAddDetail.Location = new System.Drawing.Point(1095, 35);
		this.btnAddDetail.Name = "btnAddDetail";
		this.btnAddDetail.Size = new System.Drawing.Size(28, 23);
		this.btnAddDetail.TabIndex = 9;
		this.btnAddDetail.Text = "+";
		this.btnAddDetail.UseVisualStyleBackColor = false;
		this.btnAddDetail.Click += new System.EventHandler(btnAddDetail_Click);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(855, 14);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(52, 15);
		this.label7.TabIndex = 22;
		this.label7.Text = "Remarks";
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(1096, 11);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(62, 22);
		this.btnCancelUpdateDetial.TabIndex = 11;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(1095, 35);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(64, 23);
		this.btnUpdateDetail.TabIndex = 10;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.txtItemAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemAmount.Enabled = false;
		this.txtItemAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemAmount.Location = new System.Drawing.Point(754, 35);
		this.txtItemAmount.Name = "txtItemAmount";
		this.txtItemAmount.Size = new System.Drawing.Size(102, 23);
		this.txtItemAmount.TabIndex = 7;
		this.txtItemAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRemarksDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDetail.Location = new System.Drawing.Point(857, 35);
		this.txtRemarksDetail.Name = "txtRemarksDetail";
		this.txtRemarksDetail.Size = new System.Drawing.Size(236, 23);
		this.txtRemarksDetail.TabIndex = 8;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(754, 14);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(79, 15);
		this.label6.TabIndex = 23;
		this.label6.Text = "Item Amount";
		this.CmbRateUom.AllowNull = (DefaultableBoolean)1;
		this.CmbRateUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance59;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance129).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance130).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRateUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Location = new System.Drawing.Point(676, 33);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Name = "CmbRateUom";
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Size = new System.Drawing.Size(77, 26);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).TextChanged += new System.EventHandler(CmbRateUom_TextChanged);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(679, 14);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(59, 15);
		this.label5.TabIndex = 21;
		this.label5.Text = "Rate Uom";
		this.txtItemRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemRate.Enabled = false;
		this.txtItemRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemRate.Location = new System.Drawing.Point(587, 35);
		this.txtItemRate.Name = "txtItemRate";
		this.txtItemRate.Size = new System.Drawing.Size(88, 23);
		this.txtItemRate.TabIndex = 5;
		this.txtItemRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtItemRate.TextChanged += new System.EventHandler(txtItemRate_TextChanged);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(591, 14);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(58, 15);
		this.label4.TabIndex = 19;
		this.label4.Text = "Item Rate";
		this.txtWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWeight.Enabled = false;
		this.txtWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWeight.Location = new System.Drawing.Point(498, 35);
		this.txtWeight.Name = "txtWeight";
		this.txtWeight.Size = new System.Drawing.Size(88, 23);
		this.txtWeight.TabIndex = 4;
		this.txtWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtWeight.TextChanged += new System.EventHandler(txtWeight_TextChanged);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(502, 14);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(46, 15);
		this.label3.TabIndex = 17;
		this.label3.Text = "Weight";
		this.txtqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtqty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtqty.Location = new System.Drawing.Point(409, 35);
		this.txtqty.Name = "txtqty";
		this.txtqty.Size = new System.Drawing.Size(88, 23);
		this.txtqty.TabIndex = 3;
		this.txtqty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtqty.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.txtqty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(412, 14);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(54, 15);
		this.label38.TabIndex = 15;
		this.label38.Text = "Item Qty";
		this.cmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.cmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance70;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance104).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance134).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPackUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackUom).Location = new System.Drawing.Point(331, 33);
		((System.Windows.Forms.Control)(object)this.cmbPackUom).Name = "cmbPackUom";
		((System.Windows.Forms.Control)(object)this.cmbPackUom).Size = new System.Drawing.Size(77, 26);
		((System.Windows.Forms.Control)(object)this.cmbPackUom).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbPackUom).TextChanged += new System.EventHandler(cmbPackUom_TextChanged);
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(333, 14);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(61, 15);
		this.label36.TabIndex = 13;
		this.label36.Text = "Pack Uom";
		this.CmbItem.AllowNull = (DefaultableBoolean)1;
		this.CmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance58;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance64).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance65).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItem).Location = new System.Drawing.Point(3, 33);
		((System.Windows.Forms.Control)(object)this.CmbItem).Name = "CmbItem";
		((System.Windows.Forms.Control)(object)this.CmbItem).Size = new System.Drawing.Size(223, 26);
		((System.Windows.Forms.Control)(object)this.CmbItem).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbItem).Leave += new System.EventHandler(CmbItem_Leave);
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(3, 14);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(67, 15);
		this.label37.TabIndex = 11;
		this.label37.Text = "Item Name";
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1170, 276);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1184, 611);
		this.tabControl1.TabIndex = 6;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.toolStrip1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1176, 585);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 281);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1170, 301);
		this.panel2.TabIndex = 5;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1170, 25);
		this.panel6.TabIndex = 24;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1129, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 23;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel1.Controls.Add(this.groupBox2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 212);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1170, 69);
		this.panel1.TabIndex = 4;
		this.panel5.Controls.Add(this.groupBox5);
		this.panel5.Controls.Add(this.groupBox4);
		this.panel5.Controls.Add(this.groupBox1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 59);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1170, 153);
		this.panel5.TabIndex = 3;
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOtherCommissionAgent);
		this.groupBox5.Controls.Add(this.label15);
		this.groupBox5.Controls.Add(this.label16);
		this.groupBox5.Controls.Add(this.txtOtherCommissionRate);
		this.groupBox5.Controls.Add(this.txtOtherCommissionRemarks);
		this.groupBox5.Controls.Add(this.label18);
		this.groupBox5.Controls.Add(this.label22);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOtherCommissionUom);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOtherCommissionType);
		this.groupBox5.Controls.Add(this.label27);
		this.groupBox5.Controls.Add(this.label29);
		this.groupBox5.Controls.Add(this.txtOtherCommissionAmount);
		this.groupBox5.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox5.Location = new System.Drawing.Point(837, 0);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(332, 149);
		this.groupBox5.TabIndex = 2;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Other Commission";
		this.CmbOtherCommissionAgent.AllowNull = (DefaultableBoolean)1;
		this.CmbOtherCommissionAgent.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOtherCommissionAgent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOtherCommissionAgent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOtherCommissionAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbOtherCommissionAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOtherCommissionAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOtherCommissionAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOtherCommissionAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbOtherCommissionAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOtherCommissionAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbOtherCommissionAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOtherCommissionAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOtherCommissionAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOtherCommissionAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbOtherCommissionAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOtherCommissionAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOtherCommissionAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance29).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOtherCommissionAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbOtherCommissionAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOtherCommissionAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance30).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOtherCommissionAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOtherCommissionAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbOtherCommissionAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOtherCommissionAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOtherCommissionAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbOtherCommissionAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOtherCommissionAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbOtherCommissionAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOtherCommissionAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOtherCommissionAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOtherCommissionAgent).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbOtherCommissionAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOtherCommissionAgent).Location = new System.Drawing.Point(101, 13);
		((System.Windows.Forms.Control)(object)this.CmbOtherCommissionAgent).Name = "CmbOtherCommissionAgent";
		((System.Windows.Forms.Control)(object)this.CmbOtherCommissionAgent).Size = new System.Drawing.Size(226, 26);
		((System.Windows.Forms.Control)(object)this.CmbOtherCommissionAgent).TabIndex = 0;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label15.Location = new System.Drawing.Point(6, 46);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(71, 15);
		this.label15.TabIndex = 36;
		this.label15.Text = "Comm Type";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(6, 73);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(72, 15);
		this.label16.TabIndex = 38;
		this.label16.Text = "Comm Uom";
		this.txtOtherCommissionRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherCommissionRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtOtherCommissionRate.Location = new System.Drawing.Point(247, 42);
		this.txtOtherCommissionRate.Name = "txtOtherCommissionRate";
		this.txtOtherCommissionRate.Size = new System.Drawing.Size(80, 23);
		this.txtOtherCommissionRate.TabIndex = 2;
		this.txtOtherCommissionRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOtherCommissionRate.TextChanged += new System.EventHandler(txtOtherCommissionRate_TextChanged);
		this.txtOtherCommissionRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOtherCommissionRate_KeyPress);
		this.txtOtherCommissionRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherCommissionRemarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtOtherCommissionRemarks.Location = new System.Drawing.Point(101, 94);
		this.txtOtherCommissionRemarks.Multiline = true;
		this.txtOtherCommissionRemarks.Name = "txtOtherCommissionRemarks";
		this.txtOtherCommissionRemarks.Size = new System.Drawing.Size(226, 49);
		this.txtOtherCommissionRemarks.TabIndex = 5;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label18.Location = new System.Drawing.Point(6, 19);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(98, 15);
		this.label18.TabIndex = 35;
		this.label18.Text = "Sales Man/Agent";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(6, 98);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(91, 15);
		this.label22.TabIndex = 37;
		this.label22.Text = "Comm Remarks";
		this.CmbOtherCommissionUom.AllowNull = (DefaultableBoolean)1;
		this.CmbOtherCommissionUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOtherCommissionUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOtherCommissionUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOtherCommissionUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbOtherCommissionUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOtherCommissionUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOtherCommissionUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOtherCommissionUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance161;
		((SpecialBoxBase)((UltraGridBase)this.CmbOtherCommissionUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOtherCommissionUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbOtherCommissionUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOtherCommissionUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOtherCommissionUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOtherCommissionUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbOtherCommissionUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOtherCommissionUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOtherCommissionUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance166).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOtherCommissionUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbOtherCommissionUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOtherCommissionUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOtherCommissionUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance168).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOtherCommissionUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbOtherCommissionUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOtherCommissionUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOtherCommissionUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbOtherCommissionUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOtherCommissionUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.CmbOtherCommissionUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOtherCommissionUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOtherCommissionUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOtherCommissionUom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbOtherCommissionUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOtherCommissionUom).Location = new System.Drawing.Point(101, 67);
		((System.Windows.Forms.Control)(object)this.CmbOtherCommissionUom).Name = "CmbOtherCommissionUom";
		((System.Windows.Forms.Control)(object)this.CmbOtherCommissionUom).Size = new System.Drawing.Size(91, 26);
		((System.Windows.Forms.Control)(object)this.CmbOtherCommissionUom).TabIndex = 3;
		this.CmbOtherCommissionUom.ValueChanged += new System.EventHandler(CmbOtherCommissionUom_ValueChanged);
		((System.Windows.Forms.Control)(object)this.CmbOtherCommissionUom).Leave += new System.EventHandler(CmbOtherCommissionType_ValueChanged);
		this.CmbOtherCommissionType.AllowNull = (DefaultableBoolean)1;
		this.CmbOtherCommissionType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOtherCommissionType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOtherCommissionType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOtherCommissionType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbOtherCommissionType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOtherCommissionType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance173).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance173).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOtherCommissionType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOtherCommissionType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance174;
		((SpecialBoxBase)((UltraGridBase)this.CmbOtherCommissionType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOtherCommissionType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbOtherCommissionType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOtherCommissionType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOtherCommissionType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOtherCommissionType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.CmbOtherCommissionType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOtherCommissionType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOtherCommissionType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance179).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOtherCommissionType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.CmbOtherCommissionType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOtherCommissionType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOtherCommissionType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOtherCommissionType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbOtherCommissionType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOtherCommissionType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOtherCommissionType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbOtherCommissionType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOtherCommissionType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbOtherCommissionType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOtherCommissionType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOtherCommissionType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOtherCommissionType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbOtherCommissionType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOtherCommissionType).Location = new System.Drawing.Point(101, 40);
		((System.Windows.Forms.Control)(object)this.CmbOtherCommissionType).Name = "CmbOtherCommissionType";
		((System.Windows.Forms.Control)(object)this.CmbOtherCommissionType).Size = new System.Drawing.Size(91, 26);
		((System.Windows.Forms.Control)(object)this.CmbOtherCommissionType).TabIndex = 1;
		this.CmbOtherCommissionType.ValueChanged += new System.EventHandler(CmbOtherCommissionType_ValueChanged);
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label27.Location = new System.Drawing.Point(195, 46);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(30, 15);
		this.label27.TabIndex = 34;
		this.label27.Text = "Rate";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label29.Location = new System.Drawing.Point(195, 73);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(51, 15);
		this.label29.TabIndex = 33;
		this.label29.Text = "Amount";
		this.txtOtherCommissionAmount.BackColor = System.Drawing.Color.White;
		this.txtOtherCommissionAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherCommissionAmount.Enabled = false;
		this.txtOtherCommissionAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtOtherCommissionAmount.Location = new System.Drawing.Point(247, 69);
		this.txtOtherCommissionAmount.Name = "txtOtherCommissionAmount";
		this.txtOtherCommissionAmount.Size = new System.Drawing.Size(80, 23);
		this.txtOtherCommissionAmount.TabIndex = 4;
		this.txtOtherCommissionAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCommissionAgent);
		this.groupBox4.Controls.Add(this.label20);
		this.groupBox4.Controls.Add(this.label21);
		this.groupBox4.Controls.Add(this.txtCommRate);
		this.groupBox4.Controls.Add(this.txtCommRemarks);
		this.groupBox4.Controls.Add(this.label14);
		this.groupBox4.Controls.Add(this.label25);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCommRateUom);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCommType);
		this.groupBox4.Controls.Add(this.label13);
		this.groupBox4.Controls.Add(this.label19);
		this.groupBox4.Controls.Add(this.txtCommAmount);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(505, 0);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(332, 149);
		this.groupBox4.TabIndex = 1;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Regular Commission";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCostCenter);
		this.panel3.Controls.Add(this.ChkBox);
		this.panel3.Controls.Add(this.lblCostCenter);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 30);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1170, 29);
		this.panel3.TabIndex = 2;
		this.CmbCostCenter.AllowNull = (DefaultableBoolean)1;
		this.CmbCostCenter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCostCenter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCostCenter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance116;
		((SpecialBoxBase)((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance127).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance127).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance171).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance171).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance180).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance180).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance180).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance180;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCostCenter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCostCenter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Location = new System.Drawing.Point(270, 2);
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Name = "CmbCostCenter";
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Size = new System.Drawing.Size(167, 26);
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).TabIndex = 28;
		((System.Windows.Forms.Control)(object)this.CmbCostCenter).Leave += new System.EventHandler(CmbCostCenter_Leave);
		this.lblCostCenter.AutoSize = true;
		this.lblCostCenter.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCostCenter.ForeColor = System.Drawing.Color.White;
		this.lblCostCenter.Location = new System.Drawing.Point(193, 8);
		this.lblCostCenter.Name = "lblCostCenter";
		this.lblCostCenter.Size = new System.Drawing.Size(72, 15);
		this.lblCostCenter.TabIndex = 29;
		this.lblCostCenter.Text = "Cost Center";
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Controls.Add(this.panel10);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.GridDetail);
		this.tabPage2.Controls.Add(this.panel4);
		this.tabPage2.Controls.Add(this.toolStrip2);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1176, 585);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.DataGridHistory).BackColor = System.Drawing.Color.White;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(3, 126);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1170, 274);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 23;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.ctrlGrdBar2);
		this.panel8.Controls.Add(this.label8);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 99);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1170, 27);
		this.panel8.TabIndex = 0;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1128, 3);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 24;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label8.AutoSize = true;
		this.label8.BackColor = System.Drawing.Color.Teal;
		this.label8.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(3, 3);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(217, 19);
		this.label8.TabIndex = 2;
		this.label8.Text = "PreBooking Order History";
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.label10);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel10.Location = new System.Drawing.Point(3, 400);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1170, 21);
		this.panel10.TabIndex = 1;
		this.label10.AutoSize = true;
		this.label10.BackColor = System.Drawing.Color.Teal;
		this.label10.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label10.ForeColor = System.Drawing.Color.White;
		this.label10.Location = new System.Drawing.Point(3, 2);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(295, 17);
		this.label10.TabIndex = 2;
		this.label10.Text = "Detail Information of above Selected Row";
		this.GridDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.GridDetail).BackColor = System.Drawing.Color.White;
		GridDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridDetail.DesignTimeLayout = GridDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		((System.Windows.Forms.Control)(object)this.GridDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GridDetail).Location = new System.Drawing.Point(3, 421);
		((System.Windows.Forms.Control)(object)this.GridDetail).Name = "GridDetail";
		((ScrollableBase)this.GridDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridDetail).Size = new System.Drawing.Size(1170, 161);
		((System.Windows.Forms.Control)(object)this.GridDetail).TabIndex = 24;
		this.GridDetail.TotalRow = (InheritableBoolean)1;
		this.GridDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridDetail).VisualStyle = (VisualStyle)4;
		this.panel4.Controls.Add(this.groupBox3);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 30);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1170, 69);
		this.panel4.TabIndex = 3;
		this.groupBox3.Controls.Add(this.rdapproveddate);
		this.groupBox3.Controls.Add(this.rdmodifydate);
		this.groupBox3.Controls.Add(this.drdocdate);
		this.groupBox3.Controls.Add(this.rdentrydate);
		this.groupBox3.Controls.Add(this.btnShowHistory);
		this.groupBox3.Controls.Add(this.label50);
		this.groupBox3.Controls.Add(this.label51);
		this.groupBox3.Controls.Add(this.label52);
		this.groupBox3.Controls.Add(this.label53);
		this.groupBox3.Controls.Add(this.label54);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox3.Controls.Add(this.ToDocNoHistory);
		this.groupBox3.Controls.Add(this.FromDocNoHistory);
		this.groupBox3.Controls.Add(this.ToDateHistory);
		this.groupBox3.Controls.Add(this.FromDateHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(0, 2);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1165, 65);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(934, 42);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 9;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(934, 19);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(853, 19);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 6;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(853, 42);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 7;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(785, 35);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 5;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(496, 18);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(58, 15);
		this.label50.TabIndex = 87;
		this.label50.Text = "Customer";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(392, 18);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(64, 15);
		this.label51.TabIndex = 86;
		this.label51.Text = "To Doc No";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(289, 18);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(79, 15);
		this.label52.TabIndex = 85;
		this.label52.Text = "From Doc No";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(147, 18);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(48, 15);
		this.label53.TabIndex = 84;
		this.label53.Text = "To Date";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(7, 18);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(63, 15);
		this.label54.TabIndex = 83;
		this.label54.Text = "From Date";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance145).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance145).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance146).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance146;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance148).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance148).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance155).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance156).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance156).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance156).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance157).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(496, 35);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(288, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 4;
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(392, 37);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(102, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(289, 37);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(102, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(147, 37);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(141, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(7, 37);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(138, 23);
		this.FromDateHistory.TabIndex = 0;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnResetHistory, this.btnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(3, 3);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1170, 27);
		this.toolStrip2.TabIndex = 2;
		this.toolStrip2.Text = "toolStrip2";
		this.btnResetHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnResetHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetHistory.Name = "btnResetHistory";
		this.btnResetHistory.Size = new System.Drawing.Size(59, 24);
		this.btnResetHistory.Text = "&New";
		this.btnResetHistory.Click += new System.EventHandler(btnResetHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1184, 611);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "PreBookingOrder";
		this.Text = "PreBookingOrder";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(PreBookingOrder_FormClosed);
		base.Load += new System.EventHandler(PreBookingOrder_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(PreBookingOrder_KeyDown);
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomer).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCommRateUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCommType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCommissionAgent).EndInit();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCropyr).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabPage1.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOtherCommissionAgent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOtherCommissionUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOtherCommissionType).EndInit();
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCostCenter).EndInit();
		this.tabPage2.ResumeLayout(false);
		this.tabPage2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetail).EndInit();
		this.panel4.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
