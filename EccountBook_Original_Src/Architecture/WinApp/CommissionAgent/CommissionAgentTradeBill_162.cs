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
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.CommissionAgent;

public class CommissionAgentTradeBill_162 : Form
{
	private Attachment AT = new Attachment();

	private Rightsobjects formright = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	public DataTable dtViewAttachments = new DataTable();

	private DataTable dtDetail = new DataTable();

	private DataTable dtInvExp = new DataTable();

	private DataTable dtFreight = new DataTable();

	private DataTable dtInvExpForSale = new DataTable();

	private DataTable dtExpForPackingMaterial = new DataTable();

	private DataTable dtAccountlst = new DataTable();

	private DataTable Accountlst = new DataTable();

	private DataTable dtBindCombo = new DataTable();

	private DataTable dtItem = new DataTable();

	private int RecId;

	private int VoucherHeadId;

	private int updateRowIndex = -1;

	private int DefaultDaysToLessFromHistoryFromDate;

	private List<InvCommAgentTradeBillDetail> lstRemoveRecord = new List<InvCommAgentTradeBillDetail>();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel MainPanel;

	private Panel ToolstripPanel;

	private ToolStrip toolstrip;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnAttachment;

	private Label label18;

	private DateTimePicker txtDocDate;

	private Label label3;

	private Label label8;

	private Label label7;

	private Label label6;

	private Label label5;

	private Label label4;

	private Label label2;

	private TextBox TxtDocNo;

	internal UltraCombo CmbSupplier;

	internal UltraCombo CmbCustomer;

	private TextBox txtMainRemarks;

	internal UltraCombo CmbDeliveryTerms;

	private TextBox txtDueDays;

	internal UltraCombo CmbPaymentTerms;

	private Label label15;

	private Label label12;

	private DateTimePicker txtDueDate;

	internal UltraCombo CmbBrokerCust;

	private TextBox txtCommPercentSupp;

	internal UltraCombo CmbCommAgentSupp;

	private TextBox txtBrokeryPercentCust;

	internal UltraCombo CmbPackUom;

	private Label label20;

	internal UltraCombo CmbPackType;

	private Label label19;

	internal UltraCombo CmbCropYear;

	private Label label11;

	internal UltraCombo CmbItemName;

	private Label label10;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel1;

	private Label label25;

	private TextBox txtPurchaseAmount;

	private Label label24;

	private TextBox txtRateSale;

	private Label label23;

	private TextBox txtRatePurchase;

	private Label label22;

	private TextBox txtPurGrossWeight;

	private Label label21;

	private TextBox txtQuantity;

	private Panel panel3;

	private Panel panel2;

	private TabPage tabPage2;

	private TextBox txtDetailRemarks;

	private Label label28;

	private Label label26;

	private TextBox txtSaleAmount;

	private Button btnCancelDetail;

	private Button btnUpdateDetail;

	private Button btnAddDetail;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel4;

	private ToolStrip toolStrip2;

	private ToolStripButton btnGrnFormHistory;

	private CtrlGrdBar ctrlGrdBar2;

	internal GridEX GrdDetailHistory;

	internal GridEX grdDetail;

	private TextBox txtRefDocNo;

	private Label label32;

	private TextBox txtVehicleNo;

	private Label label27;

	private Label label31;

	private TextBox txtBillWeightPurchase;

	private TextBox txtPurchaseBillAmount;

	private Label label14;

	private TextBox txtBrokeryAmountCust;

	private Label label13;

	private TextBox txtCommAmountSupp;

	private TextBox txtSaleBillAmount;

	private Label label33;

	private Label label36;

	private TextBox txtEBTotalPurchase;

	private Label label37;

	private TextBox txtEBPerUnitPurchase;

	private Label label29;

	internal UltraCombo CmbRateUom;

	internal UltraCombo CmbTradingAccount;

	private Label label38;

	private ToolStripButton btnPrint;

	private CheckBox ChkBox;

	private ToolStripButton btnPrint103;

	private ToolStripButton btnPrint170;

	private ToolStripButton btnPrint171;

	internal GridEX grdPackingMaterialSale;

	private ToolStripButton btnprint172;

	private Label label39;

	private TextBox txtEBTotalSale;

	private Label label40;

	private TextBox txtEBPerUnitSale;

	private Label label41;

	private TextBox txtBillWeightSale;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private TabPage tabPage4;

	internal GridEX grdFreightPurchase;

	private Panel panel5;

	private Panel panel8;

	private Label label34;

	private TabPage tabPage5;

	private TabPage tabPage6;

	private GroupBox groupBox4;

	private GroupBox groupBox3;

	private Label label35;

	private CtrlGrdBar ctrlGrdBar3;

	private GroupBox groupBox1;

	private GroupBox groupBox5;

	private TextBox txtAddlessWtSales;

	private Label label9;

	private Label label43;

	private Panel panel13;

	private Panel panel14;

	private Label label44;

	private Panel panel20;

	private Label label45;

	private TextBox txtOrderNoTo;

	private TextBox txtOrderNoFrom;

	private Label label46;

	internal UltraCombo cmbperemeter;

	private Label label47;

	private Button btnshowHistory;

	private DateTimePicker ToDate;

	private DateTimePicker fromdate;

	private Label label48;

	internal UltraCombo cmbtradAcHistory;

	private Label label49;

	private ToolStripButton btnNewHistory;

	internal UltraCombo cmbcusthistory;

	private Label label51;

	internal UltraCombo cmbsupplierhistory;

	private Label label50;

	private Label label52;

	private TextBox txtProfitLoss;

	internal GridEX grdInvExpForPurchase;

	private Label label53;

	internal GridEX grdInvExpForSale;

	private Label label54;

	internal GridEX GrdHistory;

	private Panel panel11;

	private Label label42;

	private Panel panel9;

	internal UltraCombo CmbDeliveryTermSale;

	private Label label55;

	private ToolStripButton BtnSlipNew1706;

	private TabPage tabPage7;

	private GroupBox groupBox6;

	private Label label64;

	private Label label61;

	internal UltraCombo CmbCommDrAcCust;

	internal UltraCombo CmbCommAgentCust;

	private TextBox txtCommPercentCust;

	private Label label62;

	private Label label63;

	private TextBox txtCommAmountCust;

	private Label label65;

	internal UltraCombo CmbBrokeryDrAcCust;

	private GroupBox groupBox2;

	private TextBox txtBrokeryAmountSupp;

	private TextBox txtBrokeryPercentSupp;

	private Label label56;

	private Label label57;

	private Label label59;

	private Label label58;

	internal UltraCombo CmbBrokeryDrAcSupp;

	internal UltraCombo CmbBrokerSupp;

	private Label label60;

	internal UltraCombo CmbCommDrAcSupp;

	private Label label66;

	private RadioButton rdSearchByCode;

	private RadioButton rdSearchByName;

	private ToolStripButton BtnDelete;

	private GroupBox groupBox7;

	private GroupBox groupBox8;

	internal UltraCombo CmbSalePaymentTerm;

	private DateTimePicker datSaleDueDate;

	private Label label67;

	private TextBox txtSaleDueDays;

	private Label label16;

	private Label label1;

	private TextBox txtBranchSrNo;

	private TextBox txtSaleGrossWeight;

	private Label label17;

	public CommissionAgentTradeBill_162(UserAccount pUser)
	{
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUser;
	}

	private void frmCommissionAgentOrder_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1150, 760);
			formright = CommonServices.SetRightsValueInRightsObject(Conversion.ToString((object)base.Name));
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			BtnDelete.Enabled = formright.DoHaveCanDelete;
			btnPrint.Enabled = formright.DoHavePrintRights;
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
			ctrlGrdBar3.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar3.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar3.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar3.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar3.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar3.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			PackingMaterialAccountsBind();
			AccountsFillForFreight();
			dtViewAttachments.Columns.Add("AttchmentPath");
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("ItemName");
			dtItem.Columns.Add("ItemCode");
			dtDetail.Columns.Add("Id", typeof(int));
			dtDetail.Columns.Add("ItemId", typeof(int));
			dtDetail.Columns.Add("ItemCode");
			dtDetail.Columns.Add("ItemName");
			dtDetail.Columns.Add("CropYearId", typeof(int));
			dtDetail.Columns.Add("CropYear");
			dtDetail.Columns.Add("PackTypeId", typeof(int));
			dtDetail.Columns.Add("PackTypeDesc");
			dtDetail.Columns.Add("UomId", typeof(int));
			dtDetail.Columns.Add("UomCode");
			dtDetail.Columns.Add("ItemQty", typeof(double));
			dtDetail.Columns.Add("GrossWeight", typeof(double));
			dtDetail.Columns.Add("PurchaseEbCut", typeof(double));
			dtDetail.Columns.Add("PurchaseEbCutTotal", typeof(double));
			dtDetail.Columns.Add("PurchaseBillWeight", typeof(double));
			dtDetail.Columns.Add("SaleGrossWeight", typeof(double));
			dtDetail.Columns.Add("SaleEbCut", typeof(double));
			dtDetail.Columns.Add("SaleEbCutTotal", typeof(double));
			dtDetail.Columns.Add("AddLessSale", typeof(double));
			dtDetail.Columns.Add("SaleBillWeight", typeof(double));
			dtDetail.Columns.Add("RatePurchase", typeof(double));
			dtDetail.Columns.Add("RateUomId", typeof(int));
			dtDetail.Columns.Add("RateUom");
			dtDetail.Columns.Add("RateSale", typeof(double));
			dtDetail.Columns.Add("PurchaseAmount", typeof(double));
			dtDetail.Columns.Add("PurchaseFreight", typeof(double));
			dtDetail.Columns.Add("PurchaseFreightLess", typeof(double));
			dtDetail.Columns.Add("PurchaseExpenses", typeof(double));
			dtDetail.Columns.Add("PurchaseNetAmount", typeof(double));
			dtDetail.Columns.Add("SaleAmount", typeof(double));
			dtDetail.Columns.Add("SaleExpenses", typeof(double));
			dtDetail.Columns.Add("SaleNetAmount", typeof(double));
			dtDetail.Columns.Add("PLAmount", typeof(double));
			dtDetail.Columns.Add("VehicleNo");
			dtDetail.Columns.Add("RefDocNo");
			dtDetail.Columns.Add("RemarksDetail");
			grdDetail.DataSource = dtDetail;
			grdDetail.RetrieveStructure();
			grdSettings();
			dtInvExp.Columns.Add("ItemId");
			dtInvExp.Columns.Add("ItemName");
			dtInvExp.Columns.Add("Qty", typeof(double));
			dtInvExp.Columns.Add("Rate", typeof(double));
			dtInvExp.Columns.Add("Amount", typeof(double));
			dtInvExp.Columns.Add("Remarks");
			dtInvExpForSale.Columns.Add("ItemId");
			dtInvExpForSale.Columns.Add("ItemName");
			dtInvExpForSale.Columns.Add("Qty", typeof(double));
			dtInvExpForSale.Columns.Add("Rate", typeof(double));
			dtInvExpForSale.Columns.Add("Amount", typeof(double));
			dtInvExpForSale.Columns.Add("Remarks");
			dtExpForPackingMaterial.Columns.Add("AccountId");
			dtExpForPackingMaterial.Columns.Add("Qty", typeof(double));
			dtExpForPackingMaterial.Columns.Add("Rate", typeof(double));
			dtExpForPackingMaterial.Columns.Add("Amount", typeof(double));
			dtExpForPackingMaterial.Columns.Add("Debit", typeof(double));
			dtExpForPackingMaterial.Columns.Add("Remarks");
			grdPackingMaterialSale.DataSource = dtExpForPackingMaterial;
			AddRowInPackingMaterialGrid();
			grdPackingMaterialSale.RetrieveStructure();
			grdPackignMaterialtSettings();
			dtFreight.Columns.Add("AccountId");
			dtFreight.Columns.Add("Qty", typeof(double));
			dtFreight.Columns.Add("Rate", typeof(double));
			dtFreight.Columns.Add("Amount", typeof(double));
			dtFreight.Columns.Add("Debit", typeof(double));
			dtFreight.Columns.Add("Remarks");
			AddRowInFreightGrid();
			grdFreightPurchase.RetrieveStructure();
			grdFreightSettings();
			OtherItemsBind();
			OtherItemsBindForSale();
			GenerateCode();
			AccountsFill();
			TradingAccountsBind();
			PaymentTermsFill();
			ItemFill();
			CropYearFill();
			DeliveryTermsFill();
			CommissionandBrokeryDrAccountsBind();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			BtnDelete.Visible = false;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			txtDocDate.Focus();
			ParameterFill();
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			ComboFillHistory();
			fromdate.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			ToDate.Value = DateTime.Now;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ParameterFill()
	{
		try
		{
			DataTable dt = CommonServices.DateType();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbperemeter, "Id", "Parameters", "Parameters", ZeroIndex: false);
				((UltraGridBase)cmbperemeter).Rows[0].Activate();
			}
			else
			{
				((Control)(object)cmbperemeter).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountsFill()
	{
		try
		{
			int ID1 = Conversion.ToInt(CmbSupplier.Value);
			int ID2 = Conversion.ToInt(CmbCommAgentSupp.Value);
			int ID3 = Conversion.ToInt(CmbBrokerSupp.Value);
			int ID4 = Conversion.ToInt(CmbCustomer.Value);
			int ID5 = Conversion.ToInt(CmbCommAgentCust.Value);
			int ID6 = Conversion.ToInt(CmbBrokerCust.Value);
			DataTable dt = CommonServices.SupplierCustomerGetAllServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbSupplier, "Id", "CompanyName", "Supplier", true);
				DDL.BindDDLNew(dt, CmbCommAgentSupp, "Id", "CompanyName", "Supp Comm Agent", true);
				DDL.BindDDLNew(dt, CmbBrokerSupp, "Id", "CompanyName", "Supp Broker A/c", true);
				DDL.BindDDLNew(dt, CmbCustomer, "Id", "CompanyName", "Customer", true);
				DDL.BindDDLNew(dt, CmbCommAgentCust, "Id", "CompanyName", "Sale Comm Agent", true);
				DDL.BindDDLNew(dt, CmbBrokerCust, "Id", "CompanyName", "Sale Broker A/c", true);
				if (ID1 > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID1))
					{
						CmbSupplier.Value = ID1;
					}
					else
					{
						((Control)(object)CmbSupplier).Text = string.Empty;
					}
				}
				if (ID2 > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID2))
					{
						CmbCommAgentSupp.Value = ID2;
					}
					else
					{
						((Control)(object)CmbCommAgentSupp).Text = string.Empty;
					}
				}
				if (ID3 > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID3))
					{
						CmbBrokerSupp.Value = ID3;
					}
					else
					{
						((Control)(object)CmbBrokerSupp).Text = string.Empty;
					}
				}
				if (ID4 > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID4))
					{
						CmbCustomer.Value = ID4;
					}
					else
					{
						((Control)(object)CmbCustomer).Text = string.Empty;
					}
				}
				if (ID5 > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID5))
					{
						CmbCommAgentCust.Value = ID5;
					}
					else
					{
						((Control)(object)CmbCommAgentCust).Text = string.Empty;
					}
				}
				if (ID6 > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID6))
					{
						CmbBrokerCust.Value = ID6;
					}
					else
					{
						((Control)(object)CmbBrokerCust).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbSupplier).Text = string.Empty;
				((UltraGridBase)CmbSupplier).DataSource = null;
				((Control)(object)CmbCommAgentSupp).Text = string.Empty;
				((UltraGridBase)CmbCommAgentSupp).DataSource = null;
				((Control)(object)CmbBrokerSupp).Text = string.Empty;
				((UltraGridBase)CmbBrokerSupp).DataSource = null;
				((Control)(object)CmbCustomer).Text = string.Empty;
				((UltraGridBase)CmbCustomer).DataSource = null;
				((Control)(object)CmbCommAgentCust).Text = string.Empty;
				((UltraGridBase)CmbCommAgentCust).DataSource = null;
				((Control)(object)CmbBrokerCust).Text = string.Empty;
				((UltraGridBase)CmbBrokerCust).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PaymentTermsFill()
	{
		try
		{
			int ID = Conversion.ToInt(CmbPaymentTerms.Value);
			int SaleId = Conversion.ToInt(CmbSalePaymentTerm.Value);
			DataTable dt = CommonServices.GetDueTermServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbPaymentTerms, "Id", "TermsDescription", "Payment Terms", true);
				DDL.BindDDLNew(dt, CmbSalePaymentTerm, "Id", "TermsDescription", "Payment Term", true);
				((UltraGridBase)CmbPaymentTerms).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)CmbSalePaymentTerm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)CmbPaymentTerms).Rows[2].Activate();
				((UltraGridBase)CmbSalePaymentTerm).Rows[2].Activate();
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbPaymentTerms.Value = ID;
					}
					else
					{
						((Control)(object)CmbPaymentTerms).Text = string.Empty;
					}
				}
				if (SaleId > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == SaleId))
					{
						CmbSalePaymentTerm.Value = SaleId;
					}
					else
					{
						((Control)(object)CmbSalePaymentTerm).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbPaymentTerms).Text = string.Empty;
				((UltraGridBase)CmbPaymentTerms).DataSource = null;
				((Control)(object)CmbSalePaymentTerm).Text = string.Empty;
				((UltraGridBase)CmbSalePaymentTerm).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeliveryTermsFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			dt.Rows.Add(1, "Load");
			dt.Rows.Add(2, "Ponch");
			DDL.BindDDL(dt, CmbDeliveryTerms, "Id", "Name", "Delivery Term", ZeroIndex: false);
			((UltraGridBase)CmbDeliveryTerms).DisplayLayout.Bands[0].Columns[1].Width = 200;
			DDL.BindDDL(dt, CmbDeliveryTermSale, "Id", "Name", "Delivery Term", ZeroIndex: false);
			((UltraGridBase)CmbDeliveryTermSale).DisplayLayout.Bands[0].Columns[1].Width = 200;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateCode()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_005f: Expected O, but got Unknown
		try
		{
			InvCommAgentTradeBill val = new InvCommAgentTradeBill
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 162,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			};
			int Code = InvCommAgentTradeBill.GenerateCode(val);
			int BranchSrNo = InvCommAgentTradeBill.GenerateBranchSrCode(val);
			TxtDocNo.Text = Conversion.ToString((object)Code);
			txtBranchSrNo.Text = Conversion.ToString((object)BranchSrNo);
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
			int ID = Conversion.ToInt(CmbItemName.Value);
			dtItem.Rows.Clear();
			DataTable dt = CommonServices.ReadAllItems();
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtItem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ItemName"], dt.Rows[i]["ItemCodeNew"]);
				}
			}
			if (dtItem.Rows.Count > 0)
			{
				if (rdSearchByName.Checked)
				{
					DDL.BindDDL(dtItem, CmbItemName, "Id", "ItemName", "Item", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtItem, CmbItemName, "Id", "ItemCode", "Item", ZeroIndex: true);
				}
				((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[1].Width = 300;
				if (ID > 0)
				{
					if (dtItem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
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

	private void rdSearchByName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtItem.Rows.Count > 0)
			{
				int ItemId = 0;
				if (Conversion.ToInt(CmbItemName.Value) > 0)
				{
					ItemId = Conversion.ToInt(CmbItemName.Value);
				}
				if (rdSearchByName.Checked)
				{
					DDL.BindDDL(dtItem, CmbItemName, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtItem, CmbItemName, "Id", "ItemCode", "Item Name", ZeroIndex: true);
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

	private void CropYearFill()
	{
		try
		{
			string ID = Conversion.ToString((object)((Control)(object)CmbCropYear).Text);
			DataTable dtCrop = CommonServices.CropYearGetAllService();
			if (dtCrop.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCrop, CmbCropYear, "Id", "CropYear", "Crop Year", true);
				((UltraGridBase)CmbCropYear).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
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

	private void CmbItemName_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbItemName.Value) > 0)
			{
				PackingTypeFill();
				packUomFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackingTypeFill()
	{
		try
		{
			int Id = Conversion.ToInt(CmbPackType.Value);
			DataTable dt = CommonServices.InvPackingTypeGetAllService();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbPackType, "Id", "PackTypeDesc", "PackTypeDesc", true);
				if (Id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbPackType.Value = Id;
					}
					else
					{
						((Control)(object)CmbPackType).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbPackType).Text = string.Empty;
				((UltraGridBase)CmbPackType).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void packUomFill()
	{
		try
		{
			string Packuom = ((Control)(object)CmbPackUom).Text;
			string Rateuom = ((Control)(object)CmbRateUom).Text;
			((Control)(object)CmbPackUom).Text = string.Empty;
			((Control)(object)CmbRateUom).Text = string.Empty;
			DataTable dt = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(CmbItemName.Value));
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbPackUom, "Id", "UOMCode", "Pack Uom", ZeroIndex: true);
				DDL.BindDDL(dt, CmbRateUom, "Id", "UOMCode", "Rate Uom", ZeroIndex: true);
				((UltraGridBase)CmbPackUom).DisplayLayout.Bands[0].Columns[1].Width = 80;
				((UltraGridBase)CmbPackUom).DisplayLayout.Bands[0].Columns[2].Width = 85;
				((UltraGridBase)CmbRateUom).DisplayLayout.Bands[0].Columns[1].Width = 80;
				((UltraGridBase)CmbRateUom).DisplayLayout.Bands[0].Columns[2].Width = 85;
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Packuom))
				{
					((Control)(object)CmbPackUom).Text = Packuom;
				}
				else
				{
					((Control)(object)CmbPackUom).Text = string.Empty;
				}
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Rateuom))
				{
					((Control)(object)CmbRateUom).Text = Rateuom;
				}
				else
				{
					((Control)(object)CmbRateUom).Text = string.Empty;
				}
			}
			else
			{
				((Control)(object)CmbPackUom).Text = string.Empty;
				((Control)(object)CmbRateUom).Text = string.Empty;
				((UltraGridBase)CmbPackUom).DataSource = null;
				((UltraGridBase)CmbRateUom).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionandBrokeryDrAccountsBind()
	{
		try
		{
			DataTable dt = DatatableHelper.GetAccountsFromGlobalByTypeIds(null, new int[3] { 2, 4, 15 }, null, new int[4] { 2, 3, 4, 5 });
			InfragisticsHelper.BindAndRetainSelection(CmbCommDrAcSupp, dt, "Id", "AccountTitle", "Commission Dr Account", AllColumns: true, Conversion.ToInt(CmbCommDrAcSupp.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbBrokeryDrAcSupp, dt, "Id", "AccountTitle", "Brokery Dr Account", AllColumns: true, Conversion.ToInt(CmbBrokeryDrAcSupp.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbCommDrAcCust, dt, "Id", "AccountTitle", "Commission Dr Account", AllColumns: true, Conversion.ToInt(CmbCommDrAcCust.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbBrokeryDrAcCust, dt, "Id", "AccountTitle", "Brokery Dr Account", AllColumns: true, Conversion.ToInt(CmbBrokeryDrAcCust.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TradingAccountsBind()
	{
		try
		{
			int Id = Conversion.ToInt(CmbTradingAccount.Value);
			DataTable dt = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("10");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbTradingAccount, "Id", "AccountTitle", "Trading Account", true);
				if (Id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbTradingAccount.Value = Id;
					}
					else
					{
						((Control)(object)CmbTradingAccount).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbTradingAccount).Text = string.Empty;
				((UltraGridBase)CmbTradingAccount).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackingMaterialAccountsBind()
	{
		try
		{
			dtAccountlst = CommonServices.CoaAllocationGetAllServiceBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSupplier_Leave(object sender, EventArgs e)
	{
		try
		{
			CheckSupplierAndFreightGlAccount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCustomer_Leave(object sender, EventArgs e)
	{
		try
		{
			CheckCustomerAndPMSaleGlAccount();
			ExpenseProportionForSale();
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
			RecId = 0;
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			lstRemoveRecord.Clear();
			updateRowIndex = -1;
			dtViewAttachments.Clear();
			MainFormReset();
			DetailFormReset();
			GenerateCode();
			ExpensesGridReset();
			dtDetail.Rows.Clear();
			grdDetail.DataSource = dtDetail;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			btnAddDetail.Visible = true;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			BtnDelete.Visible = false;
			CmbSupplier.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MainFormReset()
	{
		try
		{
			((Control)(object)CmbCommAgentSupp).Text = string.Empty;
			txtCommPercentSupp.Clear();
			((Control)(object)CmbBrokerCust).Text = string.Empty;
			txtBrokeryPercentCust.Clear();
			((Control)(object)CmbCommAgentCust).Text = string.Empty;
			txtCommPercentCust.Clear();
			((Control)(object)CmbBrokerSupp).Text = string.Empty;
			txtBrokeryPercentSupp.Clear();
			((Control)(object)CmbBrokeryDrAcCust).Text = string.Empty;
			((Control)(object)CmbBrokeryDrAcSupp).Text = string.Empty;
			((Control)(object)CmbCommDrAcCust).Text = string.Empty;
			((Control)(object)CmbCommDrAcSupp).Text = string.Empty;
			txtPurchaseBillAmount.Clear();
			txtSaleBillAmount.Clear();
			txtProfitLoss.Clear();
			txtMainRemarks.Clear();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailFormReset()
	{
		try
		{
			txtQuantity.Clear();
			txtPurGrossWeight.Clear();
			txtSaleGrossWeight.Clear();
			txtEBPerUnitPurchase.Clear();
			txtEBTotalPurchase.Clear();
			txtBillWeightPurchase.Clear();
			txtEBPerUnitSale.Clear();
			txtEBTotalSale.Clear();
			txtAddlessWtSales.Clear();
			txtBillWeightSale.Clear();
			txtRatePurchase.Clear();
			txtRateSale.Clear();
			txtPurchaseAmount.Clear();
			txtSaleAmount.Clear();
			txtVehicleNo.Clear();
			txtRefDocNo.Clear();
			txtDetailRemarks.Clear();
			CmbItemName.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExpensesGridReset()
	{
		try
		{
			if (grdInvExpForPurchase.RowCount > 0)
			{
				GridEXRow[] rows = grdInvExpForPurchase.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					obj.Cells["Remarks"].Value = "";
					obj.Cells["Amount"].Value = "0";
					obj.Cells["Qty"].Value = "0";
					obj.Cells["Rate"].Value = "0";
					obj.EndEdit();
				}
				grdInvExpForPurchase.UpdateData();
			}
			if (grdInvExpForSale.RowCount > 0)
			{
				GridEXRow[] rows = grdInvExpForSale.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["Remarks"].Value = "";
					obj2.Cells["Amount"].Value = "0";
					obj2.Cells["Qty"].Value = "0";
					obj2.Cells["Rate"].Value = "0";
					obj2.EndEdit();
				}
				grdInvExpForSale.UpdateData();
			}
			if (grdFreightPurchase.RowCount > 0)
			{
				GridEXRow[] rows = grdFreightPurchase.GetRows();
				foreach (GridEXRow obj3 in rows)
				{
					obj3.BeginEdit();
					obj3.Cells["Remarks"].Value = "";
					obj3.Cells["Amount"].Value = "0";
					obj3.Cells["Qty"].Value = "0";
					obj3.Cells["Rate"].Value = "0";
					obj3.EndEdit();
				}
				grdFreightPurchase.UpdateData();
			}
			if (grdPackingMaterialSale.RowCount > 0)
			{
				GridEXRow[] rows = grdPackingMaterialSale.GetRows();
				foreach (GridEXRow obj4 in rows)
				{
					obj4.BeginEdit();
					obj4.Cells["Remarks"].Value = "";
					obj4.Cells["Amount"].Value = "0";
					obj4.Cells["Qty"].Value = "0";
					obj4.Cells["Rate"].Value = "0";
					obj4.EndEdit();
				}
				grdPackingMaterialSale.UpdateData();
			}
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
			RefreshFormFields();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RefreshFormFields()
	{
		try
		{
			AccountsFill();
			DeliveryTermsFill();
			PaymentTermsFill();
			ItemFill();
			CropYearFill();
			CommissionandBrokeryDrAccountsBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionAgentTradeBill_162_FormClosing(object sender, FormClosingEventArgs e)
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

	private void CheckSupplierAndFreightGlAccount()
	{
		try
		{
			if (grdFreightPurchase.GetRows().Count() <= 0 || Conversion.ToInt(CmbSupplier.Value) <= 0 || (!(((Control)(object)CmbDeliveryTerms).Text == "Ponch") && !(((Control)(object)CmbDeliveryTerms).Text == "Ponch & PartyWeight") && !(((Control)(object)CmbDeliveryTerms).Text == "Ponch & FactoryWeight")))
			{
				return;
			}
			int glId = CommonServices.GetGlAccountIdBySupplierCustomerId(Conversion.ToInt(CmbSupplier.Value));
			GridEXRow[] rows = grdFreightPurchase.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (Conversion.ToInt(item.Cells["AccountId"].Value) == glId)
				{
					((Control)(object)CmbSupplier).Text = string.Empty;
					MessageBox.Show("Freight Account And Supplier Gl Account Can't be Same. First Delete Freight Grid Row#" + (item.RowIndex + 1) + " Or Change the Account in Freight Grid", "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					break;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionAgentAccoutValidation()
	{
		try
		{
			if (((UltraGridBase)CmbSupplier).ActiveRow != null && ((UltraGridBase)CmbCustomer).ActiveRow != null && Conversion.ToInt(CmbSupplier.Value) > 0 && Conversion.ToInt(CmbCustomer.Value) > 0 && Conversion.ToInt(CmbSupplier.Value) == Conversion.ToInt(CmbCustomer.Value))
			{
				CmbCustomer.Value = 0;
				CmbCustomer.Focus();
				tabControl2.SelectedIndex = 4;
				throw new Exception("Supplier Ac and Customer  Ac cannot be same");
			}
			if (((UltraGridBase)CmbCommAgentSupp).ActiveRow != null && ((UltraGridBase)CmbCommDrAcSupp).ActiveRow != null && Conversion.ToInt(CmbCommAgentSupp.Value) > 0 && Conversion.ToInt(CmbCommDrAcSupp.Value) > 0)
			{
				if (((UltraGridBase)CmbCommDrAcSupp).ActiveRow == null || Conversion.ToInt(CmbCommDrAcSupp.Value) == 0)
				{
					throw new Exception("Purchase Commission Dr_Ac Required");
				}
				if (SupplierCustomer.GetSupplierCustomerIdbyGlAccountId(Conversion.ToInt(CmbCommDrAcSupp.Value)) == Conversion.ToInt(CmbCommAgentSupp.Value))
				{
					CmbCommAgentSupp.Value = 0;
					CmbCommAgentSupp.Focus();
					tabControl2.SelectedIndex = 4;
					throw new Exception("Purchase CommissionAgent and Commission Dr  Ac cannot be same");
				}
			}
			if (((UltraGridBase)CmbBrokerSupp).ActiveRow != null && ((UltraGridBase)CmbBrokeryDrAcSupp).ActiveRow != null && Conversion.ToInt(CmbBrokerSupp.Value) > 0 && Conversion.ToInt(CmbBrokeryDrAcSupp.Value) > 0)
			{
				if (((UltraGridBase)CmbBrokeryDrAcSupp).ActiveRow == null || Conversion.ToInt(CmbBrokeryDrAcSupp.Value) == 0)
				{
					throw new Exception("Purchase Broker Dr_Ac Required");
				}
				if (SupplierCustomer.GetSupplierCustomerIdbyGlAccountId(Conversion.ToInt(CmbBrokeryDrAcSupp.Value)) == Conversion.ToInt(CmbBrokerSupp.Value))
				{
					CmbBrokerSupp.Value = 0;
					CmbBrokerSupp.Focus();
					tabControl2.SelectedIndex = 4;
					throw new Exception("Purchase Broker Ac and Broker Dr Ac cannot be same");
				}
			}
			if (((UltraGridBase)CmbCommAgentCust).ActiveRow != null && ((UltraGridBase)CmbCommDrAcCust).ActiveRow != null && Conversion.ToInt(CmbCommAgentCust.Value) > 0 && Conversion.ToInt(CmbCommDrAcCust.Value) > 0)
			{
				if (((UltraGridBase)CmbCommDrAcCust).ActiveRow == null || Conversion.ToInt(CmbCommDrAcCust.Value) == 0)
				{
					throw new Exception("Customer Commission Dr_Ac Required");
				}
				if (SupplierCustomer.GetSupplierCustomerIdbyGlAccountId(Conversion.ToInt(CmbCommDrAcCust.Value)) == Conversion.ToInt(CmbCommAgentCust.Value))
				{
					CmbCommAgentCust.Value = 0;
					CmbCommAgentCust.Focus();
					tabControl2.SelectedIndex = 4;
					throw new Exception("Sale CommissionAgent and Commission Dr  Ac cannot be same");
				}
			}
			if (((UltraGridBase)CmbBrokerCust).ActiveRow != null && ((UltraGridBase)CmbBrokeryDrAcCust).ActiveRow != null && Conversion.ToInt(CmbBrokerCust.Value) > 0 && Conversion.ToInt(CmbBrokeryDrAcCust.Value) > 0)
			{
				if (((UltraGridBase)CmbBrokeryDrAcCust).ActiveRow == null || Conversion.ToInt(CmbBrokeryDrAcCust.Value) == 0)
				{
					throw new Exception("Sale Brokery Dr_Ac Required");
				}
				if (SupplierCustomer.GetSupplierCustomerIdbyGlAccountId(Conversion.ToInt(CmbBrokeryDrAcCust.Value)) == Conversion.ToInt(CmbBrokerCust.Value))
				{
					CmbBrokerCust.Value = 0;
					CmbBrokerCust.Focus();
					tabControl2.SelectedIndex = 4;
					throw new Exception("Sale Broker Ac and Broker Dr Ac cannot be same");
				}
			}
			SupplierBillAmount();
			CustomerBillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (TxtDocNo.Text.Trim() == "" || TxtDocNo.Text.Trim() == "0" || TxtDocNo.Text.Trim() == null)
		{
			MessageBox.Show("DocNo Field is Required");
			TxtDocNo.Focus();
			return false;
		}
		if (txtBranchSrNo.Text.Trim() == "" || txtBranchSrNo.Text.Trim() == "0" || txtBranchSrNo.Text.Trim() == null)
		{
			MessageBox.Show("Branch Doc No Field is Required");
			txtBranchSrNo.Focus();
			return false;
		}
		if (((UltraGridBase)CmbSupplier).ActiveRow == null || Conversion.ToInt(CmbSupplier.Value) == 0)
		{
			MessageBox.Show("Supplier Field is Required");
			CmbSupplier.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCustomer).ActiveRow == null || Conversion.ToInt(CmbCustomer.Value) == 0)
		{
			MessageBox.Show("Customer Field is Required");
			CmbCustomer.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbSupplier.Value) == Conversion.ToInt(CmbCustomer.Value))
		{
			CmbCustomer.Value = 0;
			CmbCustomer.Focus();
			MessageBox.Show("Supplier Ac and Customer  Ac cannot be same");
			return false;
		}
		if (((UltraGridBase)CmbTradingAccount).ActiveRow == null || Conversion.ToInt(CmbTradingAccount.Value) == 0)
		{
			MessageBox.Show("TradingAccount Field is Required");
			CmbTradingAccount.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPaymentTerms).ActiveRow == null || Conversion.ToInt(CmbPaymentTerms.Value) == 0)
		{
			MessageBox.Show("Payment Term Field is Required");
			CmbPaymentTerms.Focus();
			return false;
		}
		if (((Control)(object)CmbPaymentTerms).Text == "Credit" && Conversion.ToInt((object)txtDueDays.Text.Trim()) == 0)
		{
			MessageBox.Show("Due Days Field Required");
			txtDueDays.Focus();
			return false;
		}
		if (((UltraGridBase)CmbDeliveryTerms).ActiveRow == null || Conversion.ToInt(CmbDeliveryTerms.Value) == 0)
		{
			MessageBox.Show("Delivery Term Field is Required");
			CmbDeliveryTerms.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtPurchaseBillAmount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Purchase Bill Amount Should Be Greater Than 0");
			txtPurchaseBillAmount.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtSaleBillAmount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Sale Bill Amount Should Be Greater Than 0");
			txtPurchaseBillAmount.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)CmbItemName).ActiveRow == null || Conversion.ToInt(CmbItemName.Value) == 0)
		{
			MessageBox.Show("Item Field Required");
			CmbItemName.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCropYear).ActiveRow == null || Conversion.ToInt(CmbCropYear.Value) == 0)
		{
			MessageBox.Show("Crop Year Field Required");
			CmbCropYear.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackType).ActiveRow == null || Conversion.ToInt(CmbPackType.Value) == 0)
		{
			MessageBox.Show("Pack Type Field Required");
			CmbPackType.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackUom).ActiveRow == null || Conversion.ToInt(CmbPackUom.Value) == 0)
		{
			MessageBox.Show("Pack Uom Field Required");
			CmbPackUom.Focus();
			return false;
		}
		if (Conversion.ToInt((object)txtQuantity.Text.Trim()) == 0)
		{
			MessageBox.Show("Quantity Field Required");
			txtQuantity.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtPurGrossWeight.Text.Trim()) == 0.0)
		{
			MessageBox.Show(" Gross Weight Field is Required");
			txtPurGrossWeight.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtBillWeightPurchase.Text.Trim()) == 0.0)
		{
			MessageBox.Show(" Purchase Bill Weight Field is Required");
			txtPurGrossWeight.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtBillWeightSale.Text.Trim()) == 0.0)
		{
			MessageBox.Show(" Sale Bill Weight Field is Required");
			txtPurGrossWeight.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtRatePurchase.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Rate Purchase Field is Required");
			txtRatePurchase.Focus();
			return false;
		}
		if (((UltraGridBase)CmbRateUom).ActiveRow == null || Conversion.ToInt(CmbRateUom.Value) == 0)
		{
			MessageBox.Show("RateUom Field is Required");
			CmbRateUom.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtRateSale.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Rate Sale Field is Required");
			txtRateSale.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtPurchaseAmount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Purchase Amount Field is Required");
			txtPurchaseAmount.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtSaleAmount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Sale Amount Field is Required");
			txtSaleAmount.Focus();
			return false;
		}
		if (Conversion.ToString((object)txtRefDocNo.Text.Trim()) == string.Empty)
		{
			MessageBox.Show("Ref Doc No Field is Required");
			txtRefDocNo.Focus();
			return false;
		}
		if (Conversion.ToString((object)txtVehicleNo.Text.Trim()) == string.Empty)
		{
			MessageBox.Show("Vehicle No Field is Required");
			txtVehicleNo.Focus();
			return false;
		}
		return true;
	}

	private void CheckCustomerAndPMSaleGlAccount()
	{
		try
		{
			if (grdPackingMaterialSale.GetRows().Count() <= 0 || Conversion.ToInt(CmbCustomer.Value) <= 0 || (!(((Control)(object)CmbDeliveryTerms).Text == "Load") && !(((Control)(object)CmbDeliveryTerms).Text == "Load & PartyWeight") && !(((Control)(object)CmbDeliveryTerms).Text == "Load & FactoryWeight")))
			{
				return;
			}
			int glId = CommonServices.GetGlAccountIdBySupplierCustomerId(Conversion.ToInt(CmbCustomer.Value));
			GridEXRow[] rows = grdPackingMaterialSale.GetRows();
			for (int i = 0; i < rows.Length; i++)
			{
				if (Conversion.ToInt(rows[i].Cells["AccountId"].Value) == glId)
				{
					((Control)(object)CmbCustomer).Text = string.Empty;
					MessageBox.Show("PM Sale Account And Customer Gl Account Can't be Same", "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					break;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSupplier_TextChanged(object sender, EventArgs e)
	{
		CommissionAgentAccoutValidation();
	}

	private void CmbCustomer_TextChanged(object sender, EventArgs e)
	{
		CommissionAgentAccoutValidation();
	}

	private void CmbCommAgent_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CommissionAgentAccoutValidation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCommDrAcSupp_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CommissionAgentAccoutValidation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbBroker_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CommissionAgentAccoutValidation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbBrokeryDrAcSupp_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CommissionAgentAccoutValidation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCommAgentCust_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CommissionAgentAccoutValidation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCommDrAcCust_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CommissionAgentAccoutValidation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbBrokerCust_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CommissionAgentAccoutValidation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbBrokeryDrAcCust_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CommissionAgentAccoutValidation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				dtDetail.Rows.Add(0, CmbItemName.Value, ((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemCode"].Value, ((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemName"].Value, CmbCropYear.Value, ((Control)(object)CmbCropYear).Text.Trim(), CmbPackType.Value, ((Control)(object)CmbPackType).Text.Trim(), CmbPackUom.Value, ((Control)(object)CmbPackUom).Text.Trim(), Conversion.ToDouble((object)txtQuantity.Text.Trim()), Conversion.ToDouble((object)txtPurGrossWeight.Text.Trim()), Conversion.ToDouble((object)txtEBPerUnitPurchase.Text), Conversion.ToDouble((object)txtEBTotalPurchase.Text), Conversion.ToDouble((object)txtBillWeightPurchase.Text.Trim()), Conversion.ToDouble((object)txtSaleGrossWeight.Text), Conversion.ToDouble((object)txtEBPerUnitSale.Text), Conversion.ToDouble((object)txtEBTotalSale.Text), Conversion.ToDouble((object)txtAddlessWtSales.Text.Trim()), Conversion.ToDouble((object)txtBillWeightSale.Text.Trim()), Conversion.ToDouble((object)txtRatePurchase.Text.Trim()), CmbRateUom.Value, ((Control)(object)CmbRateUom).Text.Trim(), Conversion.ToDouble((object)txtRateSale.Text.Trim()), Conversion.ToDouble((object)txtPurchaseAmount.Text.Trim()), 0, 0, 0, 0, Conversion.ToDouble((object)txtSaleAmount.Text.Trim()), 0, 0, 0, txtVehicleNo.Text, txtRefDocNo.Text, txtDetailRemarks.Text.Trim());
				grdDetail.DataSource = dtDetail;
				grdDetail.RetrieveStructure();
				grdSettings();
				DetailFormReset();
				ExpenseProportionForPurchase();
				ExpenseProportionForSale();
				FreightProportion();
				PLAmountProportion();
				CalculateCommAndBrokeryAmount();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdDetail.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				updateRowIndex = item.RowIndex;
				CmbItemName.Value = item.Cells["ItemId"].Value.ToString();
				CmbItemName_Leave(null, null);
				CmbCropYear.Value = item.Cells["CropYearId"].Text.ToString();
				CmbPackType.Value = item.Cells["PackTypeId"].Value.ToString();
				CmbPackUom.Value = item.Cells["UomId"].Value.ToString();
				CmbRateUom.Value = item.Cells["RateUomId"].Value.ToString();
				txtQuantity.Text = item.Cells["ItemQty"].Value.ToString();
				txtPurGrossWeight.Text = item.Cells["GrossWeight"].Value.ToString();
				txtEBPerUnitPurchase.Text = item.Cells["PurchaseEbCut"].Value.ToString();
				txtEBTotalPurchase.Text = item.Cells["PurchaseEbCutTotal"].Value.ToString();
				txtBillWeightPurchase.Text = item.Cells["PurchaseBillWeight"].Value.ToString();
				txtSaleGrossWeight.Text = item.Cells["SaleGrossWeight"].Value.ToString();
				txtEBPerUnitSale.Text = item.Cells["SaleEbCut"].Value.ToString();
				txtEBTotalSale.Text = item.Cells["SaleEbCutTotal"].Value.ToString();
				txtAddlessWtSales.Text = item.Cells["AddLessSale"].Value.ToString();
				txtBillWeightSale.Text = item.Cells["SaleBillWeight"].Value.ToString();
				txtRatePurchase.Text = item.Cells["RatePurchase"].Value.ToString();
				txtRateSale.Text = item.Cells["RateSale"].Value.ToString();
				txtPurchaseAmount.Text = item.Cells["PurchaseAmount"].Value.ToString();
				txtSaleAmount.Text = item.Cells["SaleAmount"].Value.ToString();
				txtVehicleNo.Text = item.Cells["VehicleNo"].Text.ToString();
				txtRefDocNo.Text = item.Cells["RefDocNo"].Text.ToString();
				txtDetailRemarks.Text = item.Cells["RemarksDetail"].Text.ToString();
				btnAddDetail.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelDetail.Visible = true;
				CmbItemName.Focus();
				ExpenseProportionForPurchase();
				ExpenseProportionForSale();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		try
		{
			grdDetail.AllowEdit = (InheritableBoolean)2;
			List<string> hiddenColumns = new List<string> { "Id", "ItemId", "CropYearId", "PackTypeId", "UomId", "RateUomId", "PLAmount" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdDetail, 2, 3, hiddenColumns);
			grdDetail.RootTable.Columns["ItemName"].Width = 250;
			grdDetail.RootTable.Columns["PurchaseFreight"].Caption = "PurchaseFreightAdd";
			grdDetail.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			grdDetail.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdDetail.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			grdDetail.RootTable.Columns["PackTypeDesc"].Width = Constants.InventoryConstants.PackingType;
			grdDetail.RootTable.Columns["UomCode"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdDetail.RootTable.Columns["GrossWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["PurchaseEbCut"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["PurchaseEbCutTotal"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["PurchaseBillWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["SaleGrossWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["SaleEbCut"].Width = Constants.InventoryConstants.EbWt;
			grdDetail.RootTable.Columns["SaleEbCutTotal"].Width = Constants.InventoryConstants.WtCut;
			grdDetail.RootTable.Columns["AddLessSale"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["SaleBillWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["RatePurchase"].Width = Constants.InventoryConstants.ExRateConstant;
			grdDetail.RootTable.Columns["RateUom"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["RateSale"].Width = Constants.InventoryConstants.ExRateConstant;
			grdDetail.RootTable.Columns["PurchaseAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["PurchaseFreight"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["PurchaseFreightLess"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["PurchaseExpenses"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["PurchaseNetAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["SaleAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["SaleExpenses"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["SaleNetAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["PLAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			grdDetail.RootTable.Columns["RefDocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdDetail.RootTable.Columns["RemarksDetail"].Width = Constants.InventoryConstants.RemarksConstants;
			grdDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["GrossWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["SaleGrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["SaleGrossWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["SaleGrossWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["SaleGrossWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["PurchaseEbCut"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["PurchaseEbCut"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["PurchaseEbCut"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["PurchaseEbCut"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["PurchaseEbCutTotal"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["PurchaseEbCutTotal"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["PurchaseEbCutTotal"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["PurchaseEbCutTotal"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["SaleEbCut"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["SaleEbCut"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["SaleEbCut"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["SaleEbCut"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["AddLessSale"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["AddLessSale"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["AddLessSale"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["AddLessSale"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["SaleEbCutTotal"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["SaleEbCutTotal"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["SaleEbCutTotal"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["SaleEbCutTotal"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["PurchaseAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["PurchaseAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["PurchaseAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["PurchaseAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["PurchaseExpenses"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["PurchaseExpenses"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["PurchaseExpenses"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["PurchaseExpenses"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["PurchaseFreight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["PurchaseFreight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["PurchaseFreight"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["PurchaseFreight"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["PurchaseFreightLess"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["PurchaseFreightLess"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["PurchaseFreightLess"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["PurchaseFreightLess"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["PurchaseNetAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["PurchaseNetAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["PurchaseNetAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["PurchaseNetAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["PLAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["PLAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["PLAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["PLAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["SaleAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["SaleAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["SaleAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["SaleAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["SaleExpenses"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["SaleExpenses"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["SaleExpenses"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["SaleExpenses"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["SaleNetAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["SaleNetAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["SaleNetAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["SaleNetAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["PurchaseBillWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["PurchaseBillWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["PurchaseBillWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["PurchaseBillWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["SaleBillWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["SaleBillWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["SaleBillWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["SaleBillWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns.Add("Delete");
			grdDetail.RootTable.Columns["Delete"].Key = "Delete";
			grdDetail.RootTable.Columns["Delete"].Caption = "X";
			grdDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdDetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdDetail.RootTable.Columns["Delete"].Width = 20;
			ctrlGrdBar1_Load(null, null);
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
			btnAddDetail.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			DetailFormReset();
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
				dtDetail.Rows[updateRowIndex]["ItemId"] = CmbItemName.Value;
				dtDetail.Rows[updateRowIndex]["ItemCode"] = ((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemCode"].Value;
				dtDetail.Rows[updateRowIndex]["ItemName"] = ((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemName"].Value;
				dtDetail.Rows[updateRowIndex]["CropYearId"] = CmbCropYear.Value;
				dtDetail.Rows[updateRowIndex]["CropYear"] = ((Control)(object)CmbCropYear).Text;
				dtDetail.Rows[updateRowIndex]["PackTypeId"] = CmbPackType.Value;
				dtDetail.Rows[updateRowIndex]["PackTypeDesc"] = ((Control)(object)CmbPackType).Text;
				dtDetail.Rows[updateRowIndex]["UomId"] = CmbPackUom.Value;
				dtDetail.Rows[updateRowIndex]["UomCode"] = ((Control)(object)CmbPackUom).Text;
				dtDetail.Rows[updateRowIndex]["RateUomId"] = CmbRateUom.Value;
				dtDetail.Rows[updateRowIndex]["RateUom"] = ((Control)(object)CmbRateUom).Text;
				dtDetail.Rows[updateRowIndex]["ItemQty"] = Conversion.ToDouble((object)txtQuantity.Text);
				dtDetail.Rows[updateRowIndex]["GrossWeight"] = Conversion.ToDouble((object)txtPurGrossWeight.Text);
				dtDetail.Rows[updateRowIndex]["PurchaseEbCut"] = Conversion.ToDouble((object)txtEBPerUnitPurchase.Text);
				dtDetail.Rows[updateRowIndex]["PurchaseEbCutTotal"] = Conversion.ToDouble((object)txtEBTotalPurchase.Text);
				dtDetail.Rows[updateRowIndex]["PurchaseBillWeight"] = Conversion.ToDouble((object)txtBillWeightPurchase.Text);
				dtDetail.Rows[updateRowIndex]["SaleGrossWeight"] = Conversion.ToDouble((object)txtSaleGrossWeight.Text);
				dtDetail.Rows[updateRowIndex]["SaleEbCut"] = Conversion.ToDouble((object)txtEBPerUnitSale.Text);
				dtDetail.Rows[updateRowIndex]["SaleEbCutTotal"] = Conversion.ToDouble((object)txtEBTotalSale.Text);
				dtDetail.Rows[updateRowIndex]["AddLessSale"] = Conversion.ToDouble((object)txtAddlessWtSales.Text);
				dtDetail.Rows[updateRowIndex]["SaleBillWeight"] = Conversion.ToDouble((object)txtBillWeightSale.Text);
				dtDetail.Rows[updateRowIndex]["RatePurchase"] = Conversion.ToDouble((object)txtRatePurchase.Text.Trim());
				dtDetail.Rows[updateRowIndex]["RateSale"] = Conversion.ToDouble((object)txtRateSale.Text);
				dtDetail.Rows[updateRowIndex]["PurchaseAmount"] = Conversion.ToDouble((object)txtPurchaseAmount.Text.Trim());
				dtDetail.Rows[updateRowIndex]["SaleAmount"] = Conversion.ToDouble((object)txtSaleAmount.Text.Trim());
				dtDetail.Rows[updateRowIndex]["VehicleNo"] = txtVehicleNo.Text.Trim();
				dtDetail.Rows[updateRowIndex]["RefDocNo"] = txtRefDocNo.Text.Trim();
				dtDetail.Rows[updateRowIndex]["RemarksDetail"] = txtDetailRemarks.Text.Trim();
				btnAddDetail.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelDetail.Visible = false;
				DetailFormReset();
				FreightProportion();
				ExpenseProportionForPurchase();
				ExpenseProportionForSale();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grdDetail.CurrentColumn != null && e.Column.Key == "Delete" && MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				if (btnsave.Visible && btnsave.Enabled)
				{
					r.Delete();
				}
				else
				{
					InvCommAgentTradeBillDetail detail = new InvCommAgentTradeBillDetail();
					detail.Id = Conversion.ToInt(r.Cells["Id"].Value);
					detail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					detail.ItemName = Conversion.ToString((object)r.Cells["ItemName"].Text);
					detail.Crop = Conversion.ToInt(r.Cells["CropYearId"].Value);
					detail.CropYear = Conversion.ToString((object)r.Cells["CropYear"].Text);
					detail.PackingTypeId = Conversion.ToInt(r.Cells["PackTypeId"].Value);
					detail.PackTypeDesc = Conversion.ToString((object)r.Cells["PackTypeDesc"].Text);
					detail.PackUomId = Conversion.ToInt(r.Cells["UomId"].Value);
					detail.PackUom = Conversion.ToString((object)r.Cells["UomCode"].Text);
					detail.Qty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
					detail.GrossWeight = Conversion.ToDecimal(r.Cells["GrossWeight"].Value);
					detail.SaleGrossWeight = Conversion.ToDecimal(r.Cells["SaleGrossWeight"].Value);
					detail.EbCut = Conversion.ToDouble(r.Cells["PurchaseEbCut"].Value);
					detail.EbCutTotal = Conversion.ToDouble(r.Cells["PurchaseEbCutTotal"].Value);
					detail.BillWeight = Conversion.ToDecimal(r.Cells["PurchaseBillWeight"].Value);
					detail.SaleEbCut = Conversion.ToDouble(r.Cells["SaleEbCut"].Value);
					detail.SaleEbCutTotal = Conversion.ToDouble(r.Cells["SaleEbCutTotal"].Value);
					detail.SaleBillWeight = Conversion.ToDecimal(r.Cells["SaleBillWeight"].Value);
					detail.RatePurchase = Conversion.ToDecimal(r.Cells["RatePurchase"].Value);
					detail.RateSale = Conversion.ToDecimal(r.Cells["RateSale"].Value);
					detail.ItemAmount = Conversion.ToDouble(r.Cells["PurchaseAmount"].Value);
					detail.SaleAmount = Conversion.ToDouble(r.Cells["SaleAmount"].Value);
					detail.VehicleNo = Conversion.ToString(r.Cells["VehicleNo"].Value);
					detail.RefDocNo = Conversion.ToString(r.Cells["RefDocNo"].Value);
					detail.RemarksDetail = Conversion.ToString((object)r.Cells["RemarksDetail"].Text.Trim());
					detail.ActionTypeId = 3;
					lstRemoveRecord.Add(detail);
					dtDetail.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				grdDetail.UpdateData();
				PLAmountProportion();
				CalculateCommAndBrokeryAmount();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OtherItemsBindForSale()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtOtherItem = InventoryItemsOther.GetAll(new InventoryItemsOther
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			for (int i = 0; i < dtOtherItem.Rows.Count; i++)
			{
				dtInvExpForSale.Rows.Add(dtOtherItem.Rows[i]["Id"], dtOtherItem.Rows[i]["OtherItemName"], 0, 0, 0, "");
			}
			grdInvExpForSale.DataSource = dtInvExpForSale;
			grdInvExpForSale.RetrieveStructure();
			grdInvExpForSaleSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInSaleExpenseGrid()
	{
		try
		{
			dtInvExpForSale.Rows.Add(0, "", "", 0);
			grdInvExpForSale.DataSource = dtInvExpForSale;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExpForSale_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdInvExpForSale.CurrentRow;
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item != null && (int)item.RowType == 0 && Conversion.ToDouble(item.Cells["Qty"].Value) > 0.0 && Conversion.ToDouble(item.Cells["Rate"].Value) > 0.0)
			{
				double Qty = Conversion.ToDouble(item.Cells["Qty"].Value);
				double Rate = Conversion.ToDouble(item.Cells["Rate"].Value);
				item.Cells["Amount"].Value = Qty * Rate;
			}
			if (e.Column.Key == "Amount" && item != null && (int)item.RowType == 0)
			{
				item.Cells["Qty"].Value = 0;
				item.Cells["Rate"].Value = 0;
			}
			grdInvExpForSale.UpdateData();
			ExpenseProportionForSale();
			grdInvExpForSale.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExpForSaleSettings()
	{
		try
		{
			grdInvExpForSale.RootTable.Columns["ItemId"].Visible = false;
			grdInvExpForSale.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdInvExpForSale.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdInvExpForSale.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdInvExpForSale.RootTable.Columns["Qty"].FormatString = "#,##0.##";
			grdInvExpForSale.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			grdInvExpForSale.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdInvExpForSale.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdInvExpForSale.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdInvExpForSale.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdInvExpForSale.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdInvExpForSale.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdInvExpForSale);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountsFillForFreight()
	{
		try
		{
			Accountlst.Rows.Clear();
			Accountlst = CommonServices.CoaAllocationAccountTitleByAccountTypeIds(null, "2,11,12,,13,14,15,20,21,22");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInFreightGrid()
	{
		dtFreight.Rows.Add(0, 0, 0, 0, 0, "");
		grdFreightPurchase.DataSource = dtFreight;
	}

	private void grdFreightSettings()
	{
		try
		{
			grdFreightPurchase.RootTable.Columns["Amount"].Caption = "Credit";
			grdFreightPurchase.RootTable.Columns["Debit"].Visible = false;
			grdFreightPurchase.RootTable.Columns["AccountId"].ColumnType = (ColumnType)0;
			grdFreightPurchase.RootTable.Columns["AccountId"].HasValueList = true;
			grdFreightPurchase.RootTable.Columns["AccountId"].LimitToList = true;
			grdFreightPurchase.RootTable.Columns["AccountId"].EditType = (EditType)4;
			grdFreightPurchase.RootTable.Columns["AccountId"].Caption = "Account";
			grdFreightPurchase.RootTable.Columns["AccountId"].ValueList.PopulateValueList((IEnumerable)Accountlst.DefaultView, "Id", "AccountTitle");
			grdFreightPurchase.RootTable.Columns["AccountId"].Width = 250;
			grdFreightPurchase.RootTable.Columns.Add("Delete");
			grdFreightPurchase.RootTable.Columns["Delete"].Key = "Delete";
			grdFreightPurchase.RootTable.Columns["Delete"].Caption = "X";
			grdFreightPurchase.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdFreightPurchase.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdFreightPurchase.RootTable.Columns["Delete"].ButtonText = "X";
			grdFreightPurchase.RootTable.Columns["Delete"].Width = 30;
			grdFreightPurchase.RootTable.Columns["Delete"].Position = 0;
			grdFreightPurchase.RootTable.Columns.Add("Add");
			grdFreightPurchase.RootTable.Columns["Add"].Key = "Add";
			grdFreightPurchase.RootTable.Columns["Add"].Caption = "+";
			grdFreightPurchase.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdFreightPurchase.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdFreightPurchase.RootTable.Columns["Add"].ButtonText = "+";
			grdFreightPurchase.RootTable.Columns["Add"].Width = 30;
			grdFreightPurchase.RootTable.Columns["Add"].Position = 1;
			grdFreightPurchase.FrozenColumns = 2;
			grdFreightPurchase.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdFreightPurchase.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdFreightPurchase.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			grdFreightPurchase.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			grdFreightPurchase.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdFreightPurchase.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdFreightPurchase.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdFreightPurchase.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdFreightPurchase.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdFreightPurchase.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdFreightPurchase.RootTable.Columns["Debit"].AggregateFunction = (AggregateFunction)2;
			grdFreightPurchase.RootTable.Columns["Debit"].TextAlignment = (TextAlignment)3;
			grdFreightPurchase.RootTable.Columns["Debit"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdFreightPurchase.RootTable.Columns["Debit"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdFreightPurchase);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFreight_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdFreightPurchase.UpdateData();
			GridEXRow item = grdFreightPurchase.CurrentRow;
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item != null && (int)item.RowType == 0 && Conversion.ToDouble(item.Cells["Qty"].Value) > 0.0 && Conversion.ToDouble(item.Cells["Rate"].Value) > 0.0)
			{
				double Qty = Conversion.ToDouble(item.Cells["Qty"].Value);
				double Rate = Conversion.ToDouble(item.Cells["Rate"].Value);
				item.Cells["Amount"].Value = Qty * Rate;
			}
			if (e.Column.Key == "Amount" && item != null && (int)item.RowType == 0)
			{
				if (Conversion.ToDouble((object)item.Cells["Debit"].Value.ToString()) > 0.0)
				{
					item.Cells["Amount"].Value = 0;
					MessageBox.Show("Debit Side is aleady added");
				}
				else
				{
					item.Cells["Qty"].Value = 0;
					item.Cells["Rate"].Value = 0;
				}
			}
			if (e.Column.Key == "Debit")
			{
				if (item != null && (int)item.RowType == 0 && Conversion.ToDouble(item.Cells["Credit"].Value) > 0.0)
				{
					item.Cells["Debit"].Value = 0;
					MessageBox.Show("Credit Side is aleady added");
				}
			}
			else if (e.Column.Key == "AccountId" && item != null && (int)item.RowType == 0 && Conversion.ToInt(CmbSupplier.Value) > 0 && (((Control)(object)CmbDeliveryTerms).Text == "Ponch" || ((Control)(object)CmbDeliveryTerms).Text == "Ponch & PartyWeight" || ((Control)(object)CmbDeliveryTerms).Text == "Ponch & FactoryWeight"))
			{
				int glId = CommonServices.GetGlAccountIdBySupplierCustomerId(Conversion.ToInt(CmbSupplier.Value));
				if (Conversion.ToInt(item.Cells["AccountId"].Value) == glId)
				{
					item.Cells["AccountId"].Value = 0;
					MessageBox.Show("Freight Account And Supplier Gl Account Can't be Same", "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}
			CommonServices.GridAutoAdjustmentNew(grdFreightPurchase);
			grdFreightPurchase.UpdateData();
			FreightProportion();
			ExpenseProportionForSale();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFreight_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdFreightPurchase.CurrentRow;
			if (item == null || (int)item.RowType != 0 || item.Cells.Count <= 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				dtFreight.Rows[item.RowIndex].Delete();
				item.Delete();
				if (dtFreight.Rows.Count == 0)
				{
					AddRowInFreightGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				dtFreight.Rows.Add(item.Cells["AccountId"].Value, 0, 0, 0, 0, item.Cells["Remarks"].Value);
				grdFreightPurchase.DataSource = dtFreight;
			}
			grdFreightPurchase.UpdateData();
			FreightProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OtherItemsBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtOtherItem = InventoryItemsOther.GetAll(new InventoryItemsOther
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			for (int i = 0; i < dtOtherItem.Rows.Count; i++)
			{
				dtInvExp.Rows.Add(dtOtherItem.Rows[i]["Id"], dtOtherItem.Rows[i]["OtherItemName"], 0, 0, 0, "");
			}
			grdInvExpForPurchase.DataSource = dtInvExp;
			grdInvExpForPurchase.RetrieveStructure();
			grdInvExpSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInPurchaseExpenseGrid()
	{
		try
		{
			dtInvExp.Rows.Add(0, "", "", 0);
			grdInvExpForPurchase.DataSource = dtInvExp;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExpForPurchase_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdInvExpForPurchase.CurrentRow;
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item != null && (int)item.RowType == 0 && Conversion.ToDouble(item.Cells["Qty"].Value) > 0.0 && Conversion.ToDouble(item.Cells["Rate"].Value) > 0.0)
			{
				double Qty = Conversion.ToDouble(item.Cells["Qty"].Value);
				double Rate = Conversion.ToDouble(item.Cells["Rate"].Value);
				item.Cells["Amount"].Value = Qty * Rate;
			}
			if (e.Column.Key == "Amount" && item != null && (int)item.RowType == 0)
			{
				item.Cells["Qty"].Value = 0;
				item.Cells["Rate"].Value = 0;
			}
			grdInvExpForPurchase.UpdateData();
			ExpenseProportionForPurchase();
			grdInvExpForPurchase.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExpSettings()
	{
		try
		{
			grdInvExpForPurchase.RootTable.Columns["ItemId"].Visible = false;
			grdInvExpForPurchase.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdInvExpForPurchase.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdInvExpForPurchase.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdInvExpForPurchase.RootTable.Columns["Qty"].FormatString = "#,##0.##";
			grdInvExpForPurchase.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			grdInvExpForPurchase.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdInvExpForPurchase.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdInvExpForPurchase.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdInvExpForPurchase.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdInvExpForPurchase.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdInvExpForPurchase.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdpackingMaterial_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPackingMaterialSale.CurrentRow;
			if (item == null || (int)item.RowType != 0 || item.Cells.Count <= 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				dtExpForPackingMaterial.Rows[item.RowIndex].Delete();
				item.Delete();
				if (dtExpForPackingMaterial.Rows.Count == 0)
				{
					AddRowInPackingMaterialGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				dtExpForPackingMaterial.Rows.Add(item.Cells["AccountId"].Value, 0, 0, 0, 0, item.Cells["Remarks"].Value);
				grdPackingMaterialSale.DataSource = dtExpForPackingMaterial;
			}
			grdPackingMaterialSale.UpdateData();
			ExpenseProportionForSale();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdpackingMaterial_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdPackingMaterialSale.UpdateData();
			GridEXRow item = grdPackingMaterialSale.CurrentRow;
			if (e.Column.Key == "Qty" || e.Column.Key == "Rate")
			{
				if (item != null && (int)item.RowType == 0 && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
				{
					double Qty = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
					double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
					item.Cells["Amount"].Value = Qty * Rate;
				}
			}
			else if (e.Column.Key == "Amount")
			{
				if (item != null && (int)item.RowType == 0)
				{
					item.Cells["Qty"].Value = 0;
					item.Cells["Rate"].Value = 0;
				}
			}
			else if (e.Column.Key == "AccountId" && item != null && (int)item.RowType == 0 && Conversion.ToInt(CmbCustomer.Value) > 0 && (((Control)(object)CmbDeliveryTermSale).Text == "Load" || ((Control)(object)CmbDeliveryTermSale).Text == "Load & PartyWeight" || ((Control)(object)CmbDeliveryTermSale).Text == "Load & FactoryWeight"))
			{
				int glId = CommonServices.GetGlAccountIdBySupplierCustomerId(Conversion.ToInt(CmbCustomer.Value));
				if (Conversion.ToInt(item.Cells["AccountId"].Value) == glId)
				{
					item.Cells["AccountId"].Value = 0;
					MessageBox.Show("PM Sale Expense Account And Customer Gl Account Can't be Same", "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}
			CommonServices.GridAutoAdjustmentNew(grdPackingMaterialSale);
			grdPackingMaterialSale.UpdateData();
			ExpenseProportionForSale();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInPackingMaterialGrid()
	{
		dtExpForPackingMaterial.Rows.Add(0, 0, 0, 0, 0, "");
		grdPackingMaterialSale.DataSource = dtExpForPackingMaterial;
	}

	private void grdPackignMaterialtSettings()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		try
		{
			List<COAAllocation> Coalst = new List<COAAllocation>();
			if (dtAccountlst.Rows.Count > 0)
			{
				for (int i = 0; i < dtAccountlst.Rows.Count; i++)
				{
					COAAllocation cOAAllocation = new COAAllocation();
					cOAAllocation.Id = Conversion.ToInt(dtAccountlst.Rows[i]["Id"]);
					cOAAllocation.AccountTitle = dtAccountlst.Rows[i]["AccountTitle"].ToString();
					Coalst.Add(cOAAllocation);
				}
			}
			grdPackingMaterialSale.RootTable.Columns["Amount"].Caption = "Credit";
			grdPackingMaterialSale.RootTable.Columns["Debit"].Visible = false;
			grdPackingMaterialSale.RootTable.Columns["AccountId"].ColumnType = (ColumnType)0;
			grdPackingMaterialSale.RootTable.Columns["AccountId"].HasValueList = true;
			grdPackingMaterialSale.RootTable.Columns["AccountId"].LimitToList = true;
			grdPackingMaterialSale.RootTable.Columns["AccountId"].EditType = (EditType)4;
			grdPackingMaterialSale.RootTable.Columns["AccountId"].Caption = "Account Title";
			grdPackingMaterialSale.RootTable.Columns["AccountId"].ValueList.PopulateValueList((IEnumerable)Coalst, "Id", "AccountTitle");
			grdPackingMaterialSale.RootTable.Columns["AccountId"].Width = 220;
			grdPackingMaterialSale.RootTable.Columns.Add("Delete");
			grdPackingMaterialSale.RootTable.Columns["Delete"].Key = "Delete";
			grdPackingMaterialSale.RootTable.Columns["Delete"].Caption = "X";
			grdPackingMaterialSale.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPackingMaterialSale.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdPackingMaterialSale.RootTable.Columns["Delete"].ButtonText = "X";
			grdPackingMaterialSale.RootTable.Columns["Delete"].Width = 20;
			grdPackingMaterialSale.RootTable.Columns["Delete"].Position = 0;
			grdPackingMaterialSale.RootTable.Columns.Add("Add");
			grdPackingMaterialSale.RootTable.Columns["Add"].Key = "Add";
			grdPackingMaterialSale.RootTable.Columns["Add"].Caption = "+";
			grdPackingMaterialSale.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPackingMaterialSale.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdPackingMaterialSale.RootTable.Columns["Add"].ButtonText = "+";
			grdPackingMaterialSale.RootTable.Columns["Add"].Width = 20;
			grdPackingMaterialSale.RootTable.Columns["Add"].Position = 1;
			grdPackingMaterialSale.FrozenColumns = 2;
			grdPackingMaterialSale.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdPackingMaterialSale.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdPackingMaterialSale.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			grdPackingMaterialSale.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			grdPackingMaterialSale.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdPackingMaterialSale.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdPackingMaterialSale.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdPackingMaterialSale.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdPackingMaterialSale.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdPackingMaterialSale.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdPackingMaterialSale.RootTable.Columns["Debit"].AggregateFunction = (AggregateFunction)2;
			grdPackingMaterialSale.RootTable.Columns["Debit"].TextAlignment = (TextAlignment)3;
			grdPackingMaterialSale.RootTable.Columns["Debit"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdPackingMaterialSale.RootTable.Columns["Debit"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdPackingMaterialSale);
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
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Insert()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Expected O, but got Unknown
		//IL_0a77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7e: Expected O, but got Unknown
		//IL_0ce1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce8: Expected O, but got Unknown
		//IL_1003: Unknown result type (might be due to invalid IL or missing references)
		//IL_100a: Expected O, but got Unknown
		//IL_1454: Unknown result type (might be due to invalid IL or missing references)
		//IL_145b: Expected O, but got Unknown
		try
		{
			if (grdDetail.GetRows().Count() == 0)
			{
				MessageBox.Show("Grid Record Not Found");
			}
			else
			{
				if (!FormValidation())
				{
					return;
				}
				InvCommAgentTradeBill obj = new InvCommAgentTradeBill();
				if (RecId > 0)
				{
					if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
					{
						return;
					}
					obj.Id = RecId;
				}
				else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				if (Conversion.ToInt((object)txtCommAmountSupp.Text) > 0 && (((UltraGridBase)CmbCommAgentSupp).ActiveRow == null || Conversion.ToInt(CmbCommAgentSupp.Value) == 0))
				{
					CmbCommAgentSupp.Focus();
					throw new Exception("Commission Agent Field is Required");
				}
				if (Conversion.ToInt((object)txtBrokeryAmountCust.Text) > 0 && (((UltraGridBase)CmbBrokerCust).ActiveRow == null || Conversion.ToInt(CmbBrokerCust.Value) == 0))
				{
					CmbBrokerCust.Focus();
					throw new Exception("BrokerAc Field is Required");
				}
				CommissionAgentAccoutValidation();
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.BranchesId = UserAccount.BranchesId;
				obj.ProjectsId = UserAccount.BranchesId;
				obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				obj.DocumentTypeId = 162;
				obj.EnteryDate = DateTime.Now;
				obj.ModifyDate = DateTime.Now;
				obj.ApprovedDate = DateTime.Now;
				obj.EnteryUserId = UserAccount.ID;
				obj.ModifyUserId = UserAccount.ID;
				obj.ApprovalUserId = UserAccount.ID;
				obj.IsApproved = false;
				obj.DocDate = Conversion.ToDateTime((object)txtDocDate.Value);
				obj.DocNo = Convert.ToInt32(TxtDocNo.Text.Trim());
				obj.BranchSrNo = Convert.ToInt32(txtBranchSrNo.Text.Trim());
				obj.TradingGlAccountId = Conversion.ToInt(CmbTradingAccount.Value);
				obj.RemarksHeader = txtMainRemarks.Text.Trim();
				obj.SupplierId = Conversion.ToInt(CmbSupplier.Value);
				obj.DeliveryTerm = Conversion.ToString((object)((Control)(object)CmbDeliveryTerms).Text);
				obj.PaymentTermId = Conversion.ToInt(CmbPaymentTerms.Value);
				obj.DueDays = Conversion.ToInt((object)txtDueDays.Text);
				obj.DueDate = Conversion.ToDateTime((object)txtDueDate.Value);
				obj.PurchaseBillAmount = Conversion.ToDouble((object)txtPurchaseBillAmount.Text);
				obj.CustomerId = Conversion.ToInt(CmbCustomer.Value);
				obj.DeliveryTermSale = Conversion.ToString((object)((Control)(object)CmbDeliveryTermSale).Text);
				obj.SalePaymentTermId = Conversion.ToInt(CmbSalePaymentTerm.Value);
				obj.SaleDueDays = Conversion.ToInt((object)txtSaleDueDays.Text);
				obj.SaleDueDate = Conversion.ToDateTime((object)datSaleDueDate.Value);
				obj.SaleBillAmount = Conversion.ToDouble((object)txtSaleBillAmount.Text);
				obj.PLAmount = Conversion.ToDouble((object)txtProfitLoss.Text);
				obj.CommissionAgentId = Conversion.ToInt(CmbCommAgentSupp.Value);
				obj.CommissionDebitGLId = Conversion.ToInt(CmbCommDrAcSupp.Value);
				obj.CommRate = Conversion.ToDouble((object)txtCommPercentSupp.Text);
				obj.CommAmount = Conversion.ToDouble((object)txtCommAmountSupp.Text);
				obj.BrokerId = Conversion.ToInt(CmbBrokerSupp.Value);
				obj.BrokeryDebitGLId = Conversion.ToInt(CmbBrokeryDrAcSupp.Value);
				obj.BrokeryRate = Conversion.ToDouble((object)txtBrokeryPercentSupp.Text);
				obj.BrokeryAmount = Conversion.ToDouble((object)txtBrokeryAmountSupp.Text);
				obj.CustCommissionAgentId = Conversion.ToInt(CmbCommAgentCust.Value);
				obj.CustCommissionDebitGLId = Conversion.ToInt(CmbCommDrAcCust.Value);
				obj.CustCommRate = Conversion.ToDouble((object)txtCommPercentCust.Text);
				obj.CustCommAmount = Conversion.ToDouble((object)txtCommAmountCust.Text);
				obj.CustBrokerId = Conversion.ToInt(CmbBrokerCust.Value);
				obj.CustBrokeryDebitGLId = Conversion.ToInt(CmbBrokeryDrAcCust.Value);
				obj.CustBrokeryRate = Conversion.ToDouble((object)txtBrokeryPercentCust.Text);
				obj.CustBrokeryAmount = Conversion.ToDouble((object)txtBrokeryAmountCust.Text);
				obj.ScreenName = base.Name;
				obj.InvCommAgentTradeBillDetailslist = new List<InvCommAgentTradeBillDetail>();
				obj.InvCommAgentTradePurchaseExpList = new List<CommisionAgentBillPurchaseExpense>();
				obj.InvCommAgentTradeSaleExpList = new List<CommisionAgentBillSaleExpense>();
				obj.CommisionAgentBillSaleExpenseCreditToReleventAcsList = new List<CommisionAgentBillSaleExpenseCreditToReleventAc>();
				obj.InvCommAgentTradeFreightExpList = new List<CommisionAgentBillPurchaseFreightExpense>();
				if (lstRemoveRecord.Count > 0)
				{
					for (int i = 0; i < lstRemoveRecord.Count; i++)
					{
						obj.InvCommAgentTradeBillDetailslist.Add(lstRemoveRecord[i]);
					}
				}
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow r3 in rows)
				{
					InvCommAgentTradeBillDetail orderDetail = new InvCommAgentTradeBillDetail();
					orderDetail.Id = Conversion.ToInt(r3.Cells["Id"].Value);
					orderDetail.ItemId = Conversion.ToInt(r3.Cells["ItemId"].Value);
					orderDetail.ItemName = Conversion.ToString((object)r3.Cells["ItemName"].Text);
					orderDetail.Crop = Conversion.ToInt(r3.Cells["CropYearId"].Value);
					orderDetail.CropYear = Conversion.ToString((object)r3.Cells["CropYear"].Text);
					orderDetail.PackingTypeId = Conversion.ToInt(r3.Cells["PackTypeId"].Value);
					orderDetail.PackTypeDesc = Conversion.ToString((object)r3.Cells["PackTypeDesc"].Text);
					orderDetail.PackUomId = Conversion.ToInt(r3.Cells["UomId"].Value);
					orderDetail.RateUomId = Conversion.ToInt(r3.Cells["RateUomId"].Value);
					orderDetail.PackUom = Conversion.ToString((object)r3.Cells["UomCode"].Text);
					orderDetail.Qty = Conversion.ToDecimal(r3.Cells["ItemQty"].Value);
					orderDetail.GrossWeight = Conversion.ToDecimal(r3.Cells["GrossWeight"].Value);
					orderDetail.EbCut = Conversion.ToDouble(r3.Cells["PurchaseEbCut"].Value);
					orderDetail.EbCutTotal = Conversion.ToDouble(r3.Cells["PurchaseEbCutTotal"].Value);
					orderDetail.BillWeight = Conversion.ToDecimal(r3.Cells["PurchaseBillWeight"].Value);
					orderDetail.SaleGrossWeight = Conversion.ToDecimal(r3.Cells["SaleGrossWeight"].Value);
					orderDetail.SaleEbCut = Conversion.ToDouble(r3.Cells["SaleEbCut"].Value);
					orderDetail.SaleEbCutTotal = Conversion.ToDouble(r3.Cells["SaleEbCutTotal"].Value);
					orderDetail.AddLessSale = Conversion.ToDecimal(r3.Cells["AddLessSale"].Value);
					orderDetail.SaleBillWeight = Conversion.ToDecimal(r3.Cells["SaleBillWeight"].Value);
					orderDetail.RatePurchase = Conversion.ToDecimal(r3.Cells["RatePurchase"].Value);
					orderDetail.RateSale = Conversion.ToDecimal(r3.Cells["RateSale"].Value);
					orderDetail.ItemAmount = Conversion.ToDouble(r3.Cells["PurchaseAmount"].Value);
					orderDetail.PurchaseExpenses = Conversion.ToDouble(r3.Cells["PurchaseExpenses"].Value);
					orderDetail.PurchaseFreight = Conversion.ToDecimal(r3.Cells["PurchaseFreight"].Value);
					orderDetail.PurchaseFreightLess = Conversion.ToDouble(r3.Cells["PurchaseFreightLess"].Value);
					orderDetail.PurchaseNetAmount = Conversion.ToDouble(r3.Cells["PurchaseNetAmount"].Value);
					orderDetail.SaleAmount = Conversion.ToDouble(r3.Cells["SaleAmount"].Value);
					orderDetail.SaleExpenses = Conversion.ToDouble(r3.Cells["SaleExpenses"].Value);
					orderDetail.SaleNetAmount = Conversion.ToDouble(r3.Cells["SaleNetAmount"].Value);
					orderDetail.VehicleNo = Conversion.ToString(r3.Cells["VehicleNo"].Value);
					orderDetail.RefDocNo = Conversion.ToString(r3.Cells["RefDocNo"].Value);
					orderDetail.RemarksDetail = Conversion.ToString((object)r3.Cells["RemarksDetail"].Text.Trim());
					if (orderDetail.Id > 0)
					{
						orderDetail.ActionTypeId = 2;
					}
					else
					{
						orderDetail.ActionTypeId = 1;
					}
					obj.InvCommAgentTradeBillDetailslist.Add(orderDetail);
				}
				rows = grdInvExpForPurchase.GetRows();
				foreach (GridEXRow r5 in rows)
				{
					if (Conversion.ToInt(r5.Cells["ItemId"].Value) > 0 && Conversion.ToInt(r5.Cells["Amount"].Value) > 0)
					{
						CommisionAgentBillPurchaseExpense PE2 = new CommisionAgentBillPurchaseExpense();
						PE2.InvExpItemId = Conversion.ToInt(r5.Cells["ItemId"].Value);
						PE2.Qty = Conversion.ToDouble(r5.Cells["Qty"].Value);
						PE2.Rate = Conversion.ToDouble(r5.Cells["Rate"].Value);
						PE2.Amount = Conversion.ToDouble(r5.Cells["Amount"].Value);
						PE2.Remarks = Conversion.ToString((object)r5.Cells["Remarks"].Text.Trim());
						if ((PE2.Remarks == "0" || PE2.Remarks == string.Empty || PE2.Remarks == null) && Conversion.ToDouble(r5.Cells["Amount"].Value) > 0.0)
						{
							PE2.Remarks = "Item Name: " + Conversion.ToString((object)r5.Cells["ItemName"].Text.Trim()) + "  Amount: " + Conversion.ToString((object)r5.Cells["Amount"].Text.Trim());
						}
						obj.InvCommAgentTradePurchaseExpList.Add(PE2);
					}
					else if (Conversion.ToInt(r5.Cells["ItemId"].Value) > 0 && Conversion.ToInt(r5.Cells["Amount"].Value) < 0)
					{
						MessageBox.Show("Amount Can't be Less than 0 in Purchase Expense Grid in Row#" + (r5.RowIndex + 1), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						tabControl2.SelectedIndex = 0;
						((Control)(object)grdInvExpForPurchase).Focus();
						return;
					}
				}
				rows = grdInvExpForSale.GetRows();
				foreach (GridEXRow r4 in rows)
				{
					if (Conversion.ToInt(r4.Cells["ItemId"].Value) > 0 && Conversion.ToDouble(r4.Cells["Amount"].Value) > 0.0)
					{
						CommisionAgentBillSaleExpense SE2 = new CommisionAgentBillSaleExpense();
						SE2.InvExpItemId = Conversion.ToInt(r4.Cells["ItemId"].Value);
						SE2.Qty = Conversion.ToDouble(r4.Cells["Qty"].Value);
						SE2.Rate = Conversion.ToDouble(r4.Cells["Rate"].Value);
						SE2.Amount = Conversion.ToDouble(r4.Cells["Amount"].Value);
						SE2.Remarks = Conversion.ToString((object)r4.Cells["Remarks"].Text.Trim());
						if ((SE2.Remarks == "0" || SE2.Remarks == string.Empty || SE2.Remarks == null) && Conversion.ToDouble(r4.Cells["Amount"].Value) > 0.0)
						{
							SE2.Remarks = "Item Name: " + Conversion.ToString((object)r4.Cells["ItemName"].Text.Trim()) + "  Amount: " + Conversion.ToString((object)r4.Cells["Amount"].Text.Trim());
						}
						obj.InvCommAgentTradeSaleExpList.Add(SE2);
					}
					else if (Conversion.ToInt(r4.Cells["ItemId"].Value) > 0 && Conversion.ToInt(r4.Cells["Amount"].Value) < 0)
					{
						MessageBox.Show("Amount Can't be Less than 0 in Sale Expense Grid in Row#" + (r4.RowIndex + 1), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						tabControl2.SelectedIndex = 2;
						((Control)(object)grdInvExpForSale).Focus();
						return;
					}
				}
				rows = grdPackingMaterialSale.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					if (Conversion.ToInt(r2.Cells["AccountId"].Value) > 0 && Conversion.ToDouble(r2.Cells["Amount"].Value) > 0.0)
					{
						if (((Control)(object)CmbDeliveryTermSale).Text == "Load" || ((Control)(object)CmbDeliveryTermSale).Text == "Load & PartyWeight" || ((Control)(object)CmbDeliveryTermSale).Text == "Load & FactoryWeight")
						{
							int glId2 = CommonServices.GetGlAccountIdBySupplierCustomerId(obj.CustomerId);
							if (Conversion.ToInt(r2.Cells["AccountId"].Value) == glId2)
							{
								MessageBox.Show("PM Sale Expense Account And Customer Gl Account Can't be Same in Row#" + (r2.RowIndex + 1), "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
								tabControl2.SelectedIndex = 3;
								((Control)(object)grdPackingMaterialSale).Focus();
								return;
							}
						}
						CommisionAgentBillSaleExpenseCreditToReleventAc SE = new CommisionAgentBillSaleExpenseCreditToReleventAc();
						SE.AccountId = Conversion.ToInt(r2.Cells["AccountId"].Value);
						SE.Qty = Conversion.ToDouble(r2.Cells["Qty"].Value);
						SE.Rate = Conversion.ToDouble(r2.Cells["Rate"].Value);
						SE.Amount = Conversion.ToDouble(r2.Cells["Amount"].Value);
						SE.Remarks = Conversion.ToString((object)r2.Cells["Remarks"].Text.Trim());
						if (SE.Remarks == "0" || SE.Remarks == string.Empty || SE.Remarks == null)
						{
							SE.Remarks = Conversion.ToString((object)r2.Cells["AccountId"].Text.Trim());
							if (SE.Qty > 0.0)
							{
								SE.Remarks = SE.Remarks + "  Qty: " + Conversion.ToString((object)r2.Cells["Qty"].Text.Trim());
							}
							if (SE.Rate > 0.0)
							{
								SE.Remarks = SE.Remarks + "  Rate: " + Conversion.ToString((object)r2.Cells["Rate"].Text.Trim());
							}
							if (SE.Amount > 0.0)
							{
								SE.Remarks = SE.Remarks + "  Amount: " + Conversion.ToString((object)r2.Cells["Amount"].Text.Trim());
							}
						}
						obj.CommisionAgentBillSaleExpenseCreditToReleventAcsList.Add(SE);
					}
					else if (Conversion.ToInt(r2.Cells["AccountId"].Value) > 0 && Conversion.ToInt(r2.Cells["Amount"].Value) < 0)
					{
						MessageBox.Show("Amount Can't be Less than 0 in PM Sale Expense Grid in Row#" + (r2.RowIndex + 1), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						tabControl2.SelectedIndex = 3;
						((Control)(object)grdPackingMaterialSale).Focus();
						return;
					}
				}
				rows = grdFreightPurchase.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.ToInt(r.Cells["AccountId"].Value) > 0 && Conversion.ToDouble(r.Cells["Amount"].Value) > 0.0)
					{
						if (((Control)(object)CmbDeliveryTerms).Text == "Ponch" || ((Control)(object)CmbDeliveryTerms).Text == "Ponch & PartyWeight" || ((Control)(object)CmbDeliveryTerms).Text == "Ponch & FactoryWeight")
						{
							int glId = CommonServices.GetGlAccountIdBySupplierCustomerId(obj.SupplierId);
							if (Conversion.ToInt(r.Cells["AccountId"].Value) == glId)
							{
								MessageBox.Show($"Freight Account And Supplier Gl Account Can't be Same in Row# {r.RowIndex + 1} " + "\nBecause Purchase Delivery Term is " + Conversion.ToString((object)((Control)(object)CmbDeliveryTerms).Text), "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
								tabControl2.SelectedIndex = 1;
								((Control)(object)grdFreightPurchase).Focus();
								return;
							}
						}
						else if (Conversion.ToInt(r.Cells["AccountId"].Value) == Conversion.ToInt(CmbTradingAccount.Value))
						{
							MessageBox.Show($"Freight Account And TradingAccount Can't be Same in Row# {r.RowIndex + 1} " + "\nBecause Purchase Delivery Term is " + Conversion.ToString((object)((Control)(object)CmbDeliveryTerms).Text), "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							tabControl2.SelectedIndex = 1;
							((Control)(object)grdFreightPurchase).Focus();
							return;
						}
						CommisionAgentBillPurchaseFreightExpense PE = new CommisionAgentBillPurchaseFreightExpense();
						PE.AccountId = Conversion.ToInt(r.Cells["AccountId"].Value);
						PE.Qty = Conversion.ToDecimal(r.Cells["Qty"].Value);
						PE.Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
						PE.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
						PE.Remarks = Conversion.ToString((object)r.Cells["Remarks"].Text.Trim());
						if (PE.Remarks == "0" || PE.Remarks == string.Empty || PE.Remarks == null)
						{
							PE.Remarks = Conversion.ToString((object)r.Cells["AccountId"].Text.Trim());
							if (PE.Qty > 0m)
							{
								PE.Remarks = PE.Remarks + "  Qty: " + Conversion.ToString((object)r.Cells["Qty"].Text.Trim());
							}
							if (PE.Rate > 0m)
							{
								PE.Remarks = PE.Remarks + "  Rate: " + Conversion.ToString((object)r.Cells["Rate"].Text.Trim());
							}
							if (PE.Amount > 0m)
							{
								PE.Remarks = PE.Remarks + "  Amount: " + Conversion.ToString((object)r.Cells["Amount"].Text.Trim());
							}
						}
						obj.InvCommAgentTradeFreightExpList.Add(PE);
					}
					else if (Conversion.ToInt(r.Cells["AccountId"].Value) > 0 && Conversion.ToInt(r.Cells["Amount"].Value) < 0)
					{
						MessageBox.Show("Amount Can't be Less than 0 in Freight Expense Grid in Row#" + (r.RowIndex + 1), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						tabControl2.SelectedIndex = 1;
						((Control)(object)grdFreightPurchase).Focus();
						return;
					}
				}
				obj.AttachmentsList = new List<DMSAttachments>();
				obj.DeleteAttachmentsList = new List<DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<InvCommAgentTradeBill>(obj, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
				int success = InvCommAgentTradeBill.Save(obj);
				if (RecId > 0)
				{
					MessageBox.Show("Record Update Successfully [ " + obj.DocNo + " ] ");
					if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
					{
						DMSAttachments.RemoveByIdAndNames(RecId, base.Name, obj.DocumentTypeId, 0);
					}
				}
				else
				{
					MessageBox.Show("Record Save Successfully  [ " + obj.DocNo + " ] ");
				}
				Reset();
				if (ChkBox.Checked)
				{
					CommissionAgentSlip(success, "168-CommisionTradeBillAndSlip.rpt");
				}
				grdDetail.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void ReadById(int ID)
	{
		try
		{
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			BtnDelete.Visible = true;
			lstRemoveRecord.Clear();
			RecId = Conversion.ToInt((object)ID);
			VoucherHeadId = CommonServices.VoucherHeadIdGet(RecId, 162);
			InvCommAgentTradeBill obj = InvCommAgentTradeBill.GetByID(RecId);
			if (obj == null)
			{
				return;
			}
			tabControl1.SelectedIndex = 0;
			TxtDocNo.Text = obj.DocNo.ToString();
			txtBranchSrNo.Text = Conversion.ToString((object)obj.BranchSrNo);
			txtDocDate.Value = obj.DocDate;
			CmbTradingAccount.Value = obj.TradingGlAccountId;
			CmbSupplier.Value = obj.SupplierId;
			CmbPaymentTerms.Value = obj.PaymentTermId;
			txtDueDays.Text = obj.DueDays.ToString();
			txtDueDate.Text = obj.DueDate.ToString();
			((Control)(object)CmbDeliveryTerms).Text = obj.DeliveryTerm;
			txtPurchaseBillAmount.Text = obj.PurchaseBillAmount.ToString(clsGlobalVariables.stringFormatboth);
			CmbCustomer.Value = obj.CustomerId;
			((Control)(object)CmbDeliveryTermSale).Text = obj.DeliveryTermSale;
			CmbSalePaymentTerm.Value = obj.SalePaymentTermId;
			txtSaleDueDays.Text = obj.SaleDueDays.ToString();
			datSaleDueDate.Value = Conversion.ToDateTime((object)obj.SaleDueDate);
			txtSaleBillAmount.Text = obj.SaleBillAmount.ToString(clsGlobalVariables.stringFormatboth);
			txtProfitLoss.Text = obj.PLAmount.ToString();
			txtMainRemarks.Text = obj.RemarksHeader;
			CmbCommAgentSupp.Value = obj.CommissionAgentId;
			CmbCommDrAcSupp.Value = obj.CommissionDebitGLId;
			txtCommPercentSupp.Text = obj.CommRate.ToString();
			txtCommAmountSupp.Text = obj.CommAmount.ToString(clsGlobalVariables.stringFormatboth);
			CmbBrokerSupp.Value = obj.BrokerId;
			CmbBrokeryDrAcSupp.Value = obj.BrokeryDebitGLId;
			txtBrokeryPercentSupp.Text = obj.BrokeryRate.ToString();
			txtBrokeryAmountSupp.Text = obj.BrokeryAmount.ToString(clsGlobalVariables.stringFormatboth);
			CmbCommAgentCust.Value = obj.CustCommissionAgentId;
			CmbCommDrAcCust.Value = obj.CustCommissionDebitGLId;
			txtCommPercentCust.Text = obj.CustCommRate.ToString();
			txtCommAmountCust.Text = obj.CustCommAmount.ToString(clsGlobalVariables.stringFormatboth);
			CmbBrokerCust.Value = obj.CustBrokerId;
			CmbBrokeryDrAcCust.Value = obj.CustBrokeryDebitGLId;
			txtBrokeryPercentCust.Text = obj.CustBrokeryRate.ToString();
			txtBrokeryAmountCust.Text = obj.CustBrokeryAmount.ToString(clsGlobalVariables.stringFormatboth);
			dtDetail.Rows.Clear();
			for (int m = 0; m < obj.InvCommAgentTradeBillDetailslist.Count; m++)
			{
				dtDetail.Rows.Add(obj.InvCommAgentTradeBillDetailslist[m].Id, obj.InvCommAgentTradeBillDetailslist[m].ItemId, obj.InvCommAgentTradeBillDetailslist[m].ItemCode, obj.InvCommAgentTradeBillDetailslist[m].ItemName, obj.InvCommAgentTradeBillDetailslist[m].Crop, obj.InvCommAgentTradeBillDetailslist[m].CropYear, obj.InvCommAgentTradeBillDetailslist[m].PackingTypeId, obj.InvCommAgentTradeBillDetailslist[m].PackTypeDesc, obj.InvCommAgentTradeBillDetailslist[m].PackUomId, obj.InvCommAgentTradeBillDetailslist[m].PackUom, obj.InvCommAgentTradeBillDetailslist[m].Qty, obj.InvCommAgentTradeBillDetailslist[m].GrossWeight, obj.InvCommAgentTradeBillDetailslist[m].EbCut, obj.InvCommAgentTradeBillDetailslist[m].EbCutTotal, obj.InvCommAgentTradeBillDetailslist[m].BillWeight, obj.InvCommAgentTradeBillDetailslist[m].SaleGrossWeight, obj.InvCommAgentTradeBillDetailslist[m].SaleEbCut, obj.InvCommAgentTradeBillDetailslist[m].SaleEbCutTotal, obj.InvCommAgentTradeBillDetailslist[m].AddLessSale, obj.InvCommAgentTradeBillDetailslist[m].SaleBillWeight, obj.InvCommAgentTradeBillDetailslist[m].RatePurchase, obj.InvCommAgentTradeBillDetailslist[m].RateUomId, obj.InvCommAgentTradeBillDetailslist[m].RateUom, obj.InvCommAgentTradeBillDetailslist[m].RateSale, obj.InvCommAgentTradeBillDetailslist[m].ItemAmount, obj.InvCommAgentTradeBillDetailslist[m].PurchaseFreight, obj.InvCommAgentTradeBillDetailslist[m].PurchaseFreightLess, obj.InvCommAgentTradeBillDetailslist[m].PurchaseExpenses, obj.InvCommAgentTradeBillDetailslist[m].PurchaseNetAmount, obj.InvCommAgentTradeBillDetailslist[m].SaleAmount, obj.InvCommAgentTradeBillDetailslist[m].SaleExpenses, obj.InvCommAgentTradeBillDetailslist[m].SaleNetAmount, 0, obj.InvCommAgentTradeBillDetailslist[m].VehicleNo, obj.InvCommAgentTradeBillDetailslist[m].RefDocNo, obj.InvCommAgentTradeBillDetailslist[m].RemarksDetail);
			}
			if (obj.InvCommAgentTradePurchaseExpList.Count > 0)
			{
				for (int l = 0; l < obj.InvCommAgentTradePurchaseExpList.Count; l++)
				{
					GridEXRow[] rows = grdInvExpForPurchase.GetRows();
					foreach (GridEXRow item2 in rows)
					{
						if (Conversion.ToInt(item2.Cells["ItemId"].Value) == Conversion.ToInt((object)obj.InvCommAgentTradePurchaseExpList[l].InvExpItemId))
						{
							item2.BeginEdit();
							item2.Cells["Qty"].Value = Conversion.ToDouble((object)obj.InvCommAgentTradePurchaseExpList[l].Qty);
							item2.Cells["Rate"].Value = Conversion.ToDouble((object)obj.InvCommAgentTradePurchaseExpList[l].Rate);
							item2.Cells["Amount"].Value = Conversion.ToDouble((object)obj.InvCommAgentTradePurchaseExpList[l].Amount);
							item2.Cells["Remarks"].Value = Conversion.ToString((object)obj.InvCommAgentTradePurchaseExpList[l].Remarks);
							item2.EndEdit();
						}
					}
				}
				grdInvExpForPurchase.UpdateData();
			}
			if (obj.InvCommAgentTradeSaleExpList.Count > 0)
			{
				for (int k = 0; k < obj.InvCommAgentTradeSaleExpList.Count; k++)
				{
					GridEXRow[] rows = grdInvExpForSale.GetRows();
					foreach (GridEXRow item in rows)
					{
						if (Conversion.ToInt(item.Cells["ItemId"].Value) == Conversion.ToInt((object)obj.InvCommAgentTradeSaleExpList[k].InvExpItemId))
						{
							item.BeginEdit();
							item.Cells["Qty"].Value = Conversion.ToDouble((object)obj.InvCommAgentTradeSaleExpList[k].Qty);
							item.Cells["Rate"].Value = Conversion.ToDouble((object)obj.InvCommAgentTradeSaleExpList[k].Rate);
							item.Cells["Amount"].Value = Conversion.ToDouble((object)obj.InvCommAgentTradeSaleExpList[k].Amount);
							item.Cells["Remarks"].Value = Conversion.ToString((object)obj.InvCommAgentTradeSaleExpList[k].Remarks);
							item.EndEdit();
						}
					}
				}
				grdInvExpForSale.UpdateData();
			}
			dtExpForPackingMaterial.Rows.Clear();
			for (int j = 0; j < obj.CommisionAgentBillSaleExpenseCreditToReleventAcsList.Count; j++)
			{
				dtExpForPackingMaterial.Rows.Add(obj.CommisionAgentBillSaleExpenseCreditToReleventAcsList[j].AccountId, obj.CommisionAgentBillSaleExpenseCreditToReleventAcsList[j].Qty, obj.CommisionAgentBillSaleExpenseCreditToReleventAcsList[j].Rate, obj.CommisionAgentBillSaleExpenseCreditToReleventAcsList[j].Amount, 0, obj.CommisionAgentBillSaleExpenseCreditToReleventAcsList[j].Remarks);
			}
			if (dtExpForPackingMaterial.Rows.Count == 0)
			{
				AddRowInPackingMaterialGrid();
			}
			dtFreight.Rows.Clear();
			for (int i = 0; i < obj.InvCommAgentTradeFreightExpList.Count; i++)
			{
				dtFreight.Rows.Add(obj.InvCommAgentTradeFreightExpList[i].AccountId, obj.InvCommAgentTradeFreightExpList[i].Qty, obj.InvCommAgentTradeFreightExpList[i].Rate, obj.InvCommAgentTradeFreightExpList[i].Amount, 0, obj.InvCommAgentTradeFreightExpList[i].Remarks);
			}
			if (dtFreight.Rows.Count == 0)
			{
				AddRowInFreightGrid();
			}
			grdDetail.DataSource = dtDetail;
			grdDetail.RetrieveStructure();
			grdSettings();
			grdFreightPurchase.DataSource = dtFreight;
			grdFreightPurchase.RetrieveStructure();
			grdFreightSettings();
			grdPackingMaterialSale.DataSource = dtExpForPackingMaterial;
			grdPackingMaterialSale.RetrieveStructure();
			grdPackignMaterialtSettings();
			ExpenseProportionForPurchase();
			ExpenseProportionForSale();
			FreightProportion();
			PLAmountProportion();
			CalculateCommAndBrokeryAmount();
			tabControl1.SelectedIndex = 0;
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
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
				throw new Exception("RecId not Found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				MessageBox.Show("Record Id Not Found", "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				InvCommAgentTradeBill.DeleteByID(UserAccount.ID, RecId);
				MessageBox.Show("Delete Record Seccessfully");
				Reset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdDetail).Name);
			if (fs != null)
			{
				grdDetail.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GrdHistory).Name);
			if (fs != null)
			{
				GrdHistory.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GrdDetailHistory).Name);
			if (fs != null)
			{
				GrdDetailHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

	private void HistoryGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.DocumentTypeId = 162;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.FromDate = fromdate.Value;
			obj.ToDate = ToDate.Value;
			obj.FromDocNo = Conversion.ToInt((object)txtOrderNoFrom.Text);
			obj.ToDocNo = Conversion.ToInt((object)txtOrderNoTo.Text);
			obj.GlAccountId = Conversion.ToInt(cmbtradAcHistory.Value);
			obj.SupplierId = Conversion.ToInt(cmbsupplierhistory.Value);
			obj.CustomerId = Conversion.ToInt(cmbcusthistory.Value);
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
			DataTable dtHistory = InvCommAgentTradeBill.FormHistory(obj);
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("BranchSrNo", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("Supplier");
				dt.Columns.Add("Customer");
				dt.Columns.Add("RefDocNos");
				dt.Columns.Add("VehicleNos");
				dt.Columns.Add("PurDeliveryTerm");
				dt.Columns.Add("SaleDeliveryTerm");
				dt.Columns.Add("PaymentTerm");
				dt.Columns.Add("DueDays", typeof(int));
				dt.Columns.Add("DueDate", typeof(DateTime));
				dt.Columns.Add("CommAgent");
				dt.Columns.Add("CommRate", typeof(double));
				dt.Columns.Add("CommAmount", typeof(double));
				dt.Columns.Add("Broker");
				dt.Columns.Add("BrokeryRate", typeof(double));
				dt.Columns.Add("BrokeryAmount", typeof(double));
				dt.Columns.Add("PurchaseBillAmount", typeof(double));
				dt.Columns.Add("SaleBillAmount", typeof(double));
				dt.Columns.Add("PLAmount", typeof(double));
				dt.Columns.Add("Remarks");
				dt.Columns.Add("EntryUser");
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser");
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("NoOfAttachments");
				for (int i = 0; i < dtHistory.Rows.Count; i++)
				{
					dt.Rows.Add(dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["DocNo"], dtHistory.Rows[i]["BranchSrNo"], Conversion.ToDateTime(dtHistory.Rows[i]["DocDate"]).ToShortDateString(), dtHistory.Rows[i]["SupplierName"], dtHistory.Rows[i]["CustomerName"], dtHistory.Rows[i]["RefDocNos"], dtHistory.Rows[i]["VehicleNos"], dtHistory.Rows[i]["DeliveryTerm"], dtHistory.Rows[i]["DeliveryTermSale"], dtHistory.Rows[i]["TermsDescription"], dtHistory.Rows[i]["DueDays"], Conversion.ToDateTime(dtHistory.Rows[i]["DueDate"]).ToShortDateString(), dtHistory.Rows[i]["CommissionAgent"], dtHistory.Rows[i]["CommRate"], dtHistory.Rows[i]["CommAmount"], dtHistory.Rows[i]["BrokerName"], dtHistory.Rows[i]["BrokeryRate"], dtHistory.Rows[i]["BrokeryAmount"], dtHistory.Rows[i]["PurchaseBillAmount"], dtHistory.Rows[i]["SaleBillAmount"], dtHistory.Rows[i]["PLAmount"], dtHistory.Rows[i]["RemarksHeader"], dtHistory.Rows[i]["EntryUserName"], Conversion.ToDateTime(dtHistory.Rows[i]["EnteryDate"]).ToShortDateString(), dtHistory.Rows[i]["ModifyUserName"], Conversion.ToDateTime(dtHistory.Rows[i]["ModifyDate"]).ToShortDateString(), dtHistory.Rows[i]["NoOfAttachments"]);
				}
				GrdHistory.DataSource = dt;
				GrdHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				GrdHistory.ClearStructure();
				GrdDetailHistory.ClearStructure();
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
			GrdHistory.RootTable.Columns["Id"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(GrdHistory, 3, 3);
			GrdHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			GrdHistory.RootTable.Columns["BranchSrNo"].Width = Constants.InventoryConstants.DocNoConstant;
			GrdHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant + 15;
			GrdHistory.RootTable.Columns["Supplier"].Width = Constants.InventoryConstants.SupplierName + 20;
			GrdHistory.RootTable.Columns["Customer"].Width = Constants.InventoryConstants.SupplierName + 20;
			GrdHistory.RootTable.Columns["RefDocNos"].Width = Constants.InventoryConstants.DocNoConstant + 30;
			GrdHistory.RootTable.Columns["VehicleNos"].Width = Constants.InventoryConstants.VehicleNoConstant + 30;
			GrdHistory.RootTable.Columns["PurDeliveryTerm"].Width = Constants.InventoryConstants.TermConstant;
			GrdHistory.RootTable.Columns["SaleDeliveryTerm"].Width = Constants.InventoryConstants.TermConstant;
			GrdHistory.RootTable.Columns["PaymentTerm"].Width = Constants.InventoryConstants.TermConstant;
			GrdHistory.RootTable.Columns["DueDays"].Width = Constants.InventoryConstants.DueDays;
			GrdHistory.RootTable.Columns["DueDate"].Width = Constants.InventoryConstants.DateConstant + 15;
			GrdHistory.RootTable.Columns["CommAgent"].Width = Constants.InventoryConstants.SupplierName;
			GrdHistory.RootTable.Columns["CommRate"].Width = Constants.InventoryConstants.ExRateConstant;
			GrdHistory.RootTable.Columns["CommAmount"].Width = Constants.InventoryConstants.AmountConstant;
			GrdHistory.RootTable.Columns["Broker"].Width = Constants.InventoryConstants.SupplierName;
			GrdHistory.RootTable.Columns["BrokeryRate"].Width = Constants.InventoryConstants.ExRateConstant;
			GrdHistory.RootTable.Columns["BrokeryAmount"].Width = Constants.InventoryConstants.AmountConstant;
			GrdHistory.RootTable.Columns["PurchaseBillAmount"].Width = Constants.InventoryConstants.AmountConstant;
			GrdHistory.RootTable.Columns["SaleBillAmount"].Width = Constants.InventoryConstants.AmountConstant;
			GrdHistory.RootTable.Columns["PLAmount"].Width = Constants.InventoryConstants.AmountConstant;
			GrdHistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			GrdHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			GrdHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			GrdHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			GrdHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			GrdHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			GrdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			GrdHistory.RootTable.Columns["CommRate"].TextAlignment = (TextAlignment)3;
			GrdHistory.RootTable.Columns["CommRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			GrdHistory.RootTable.Columns["CommAmount"].AggregateFunction = (AggregateFunction)2;
			GrdHistory.RootTable.Columns["CommAmount"].TextAlignment = (TextAlignment)3;
			GrdHistory.RootTable.Columns["CommAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GrdHistory.RootTable.Columns["CommAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GrdHistory.RootTable.Columns["BrokeryRate"].TextAlignment = (TextAlignment)3;
			GrdHistory.RootTable.Columns["BrokeryRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			GrdHistory.RootTable.Columns["BrokeryAmount"].AggregateFunction = (AggregateFunction)2;
			GrdHistory.RootTable.Columns["BrokeryAmount"].TextAlignment = (TextAlignment)3;
			GrdHistory.RootTable.Columns["BrokeryAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GrdHistory.RootTable.Columns["BrokeryAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GrdHistory.RootTable.Columns["PurchaseBillAmount"].AggregateFunction = (AggregateFunction)2;
			GrdHistory.RootTable.Columns["PurchaseBillAmount"].TextAlignment = (TextAlignment)3;
			GrdHistory.RootTable.Columns["PurchaseBillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GrdHistory.RootTable.Columns["PurchaseBillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GrdHistory.RootTable.Columns["SaleBillAmount"].AggregateFunction = (AggregateFunction)2;
			GrdHistory.RootTable.Columns["SaleBillAmount"].TextAlignment = (TextAlignment)3;
			GrdHistory.RootTable.Columns["SaleBillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GrdHistory.RootTable.Columns["SaleBillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GrdHistory.RootTable.Columns["PLAmount"].AggregateFunction = (AggregateFunction)2;
			GrdHistory.RootTable.Columns["PLAmount"].TextAlignment = (TextAlignment)3;
			GrdHistory.RootTable.Columns["PLAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GrdHistory.RootTable.Columns["PLAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GrdHistory.RootTable.Columns.Add("Edit");
			GrdHistory.RootTable.Columns["Edit"].Caption = "Edit";
			GrdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			GrdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			GrdHistory.RootTable.Columns["Edit"].Width = 40;
			GrdHistory.RootTable.Columns["Edit"].Position = 0;
			GrdHistory.RootTable.Columns.Add("Print");
			GrdHistory.RootTable.Columns["Print"].Caption = "Print";
			GrdHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			GrdHistory.RootTable.Columns["Print"].ButtonText = "Print";
			GrdHistory.RootTable.Columns["Print"].Width = 40;
			GrdHistory.RootTable.Columns["Print"].Position = 1;
			GrdHistory.FrozenColumns = 2;
			ctrlGrdBar2_Load(null, null);
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
			if (item != null && (int)item.RowType == 0 && GrdHistory.CurrentColumn != null && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 162);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridhistorydetail()
	{
		try
		{
			GridEX_Helper.GridWrappingAndColumnSettings(GrdDetailHistory, 3, 3);
			GrdDetailHistory.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			GrdDetailHistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			GrdDetailHistory.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			GrdDetailHistory.RootTable.Columns["PackTypeDesc"].Width = Constants.InventoryConstants.PackingType;
			GrdDetailHistory.RootTable.Columns["UOMCode"].Width = Constants.InventoryConstants.UomConstant;
			GrdDetailHistory.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
			GrdDetailHistory.RootTable.Columns["GrossWeight"].Width = Constants.InventoryConstants.WeightConstant;
			GrdDetailHistory.RootTable.Columns["PurchaseEbCut"].Width = Constants.InventoryConstants.WeightConstant;
			GrdDetailHistory.RootTable.Columns["PurchaseEbCutTotal"].Width = Constants.InventoryConstants.WeightConstant;
			GrdDetailHistory.RootTable.Columns["PurchaseBillWeight"].Width = Constants.InventoryConstants.WeightConstant;
			GrdDetailHistory.RootTable.Columns["SaleGrossWeight"].Width = Constants.InventoryConstants.WeightConstant;
			GrdDetailHistory.RootTable.Columns["SaleEbCut"].Width = Constants.InventoryConstants.WeightConstant;
			GrdDetailHistory.RootTable.Columns["SaleEbCutTotal"].Width = Constants.InventoryConstants.WeightConstant;
			GrdDetailHistory.RootTable.Columns["AddLessSale"].Width = Constants.InventoryConstants.WeightConstant;
			GrdDetailHistory.RootTable.Columns["SaleBillWeight"].Width = Constants.InventoryConstants.WeightConstant;
			GrdDetailHistory.RootTable.Columns["RatePurchase"].Width = Constants.InventoryConstants.ExRateConstant;
			GrdDetailHistory.RootTable.Columns["RateSale"].Width = Constants.InventoryConstants.ExRateConstant;
			GrdDetailHistory.RootTable.Columns["PurchaseAmount"].Width = Constants.InventoryConstants.AmountConstant;
			GrdDetailHistory.RootTable.Columns["PurchaseFreight"].Width = Constants.InventoryConstants.AmountConstant;
			GrdDetailHistory.RootTable.Columns["PurchaseExpenses"].Width = Constants.InventoryConstants.AmountConstant;
			GrdDetailHistory.RootTable.Columns["PurchaseNetAmount"].Width = Constants.InventoryConstants.AmountConstant;
			GrdDetailHistory.RootTable.Columns["SaleAmount"].Width = Constants.InventoryConstants.AmountConstant;
			GrdDetailHistory.RootTable.Columns["SaleExpenses"].Width = Constants.InventoryConstants.AmountConstant;
			GrdDetailHistory.RootTable.Columns["SaleNetAmount"].Width = Constants.InventoryConstants.AmountConstant;
			GrdDetailHistory.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			GrdDetailHistory.RootTable.Columns["RefDocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			GrdDetailHistory.RootTable.Columns["RemarksDetail"].Width = Constants.InventoryConstants.RemarksConstants;
			GrdDetailHistory.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			GrdDetailHistory.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			GrdDetailHistory.RootTable.Columns["Qty"].FormatString = "#,##0.##";
			GrdDetailHistory.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			GrdDetailHistory.RootTable.Columns["PurchaseEbCut"].AggregateFunction = (AggregateFunction)2;
			GrdDetailHistory.RootTable.Columns["PurchaseEbCut"].TextAlignment = (TextAlignment)3;
			GrdDetailHistory.RootTable.Columns["PurchaseEbCut"].FormatString = "#,##0.##";
			GrdDetailHistory.RootTable.Columns["PurchaseEbCut"].TotalFormatString = "#,##0.##";
			GrdDetailHistory.RootTable.Columns["PurchaseEbCutTotal"].AggregateFunction = (AggregateFunction)2;
			GrdDetailHistory.RootTable.Columns["PurchaseEbCutTotal"].TextAlignment = (TextAlignment)3;
			GrdDetailHistory.RootTable.Columns["PurchaseEbCutTotal"].FormatString = "#,##0.##";
			GrdDetailHistory.RootTable.Columns["PurchaseEbCutTotal"].TotalFormatString = "#,##0.##";
			GrdDetailHistory.RootTable.Columns["SaleEbCutTotal"].AggregateFunction = (AggregateFunction)2;
			GrdDetailHistory.RootTable.Columns["SaleEbCutTotal"].TextAlignment = (TextAlignment)3;
			GrdDetailHistory.RootTable.Columns["SaleEbCutTotal"].FormatString = "#,##0.##";
			GrdDetailHistory.RootTable.Columns["SaleEbCutTotal"].TotalFormatString = "#,##0.##";
			GrdDetailHistory.RootTable.Columns["SaleEbCut"].AggregateFunction = (AggregateFunction)2;
			GrdDetailHistory.RootTable.Columns["SaleEbCut"].TextAlignment = (TextAlignment)3;
			GrdDetailHistory.RootTable.Columns["SaleEbCut"].FormatString = "#,##0.##";
			GrdDetailHistory.RootTable.Columns["SaleEbCut"].TotalFormatString = "#,##0.##";
			GrdDetailHistory.RootTable.Columns["AddLessSale"].AggregateFunction = (AggregateFunction)2;
			GrdDetailHistory.RootTable.Columns["AddLessSale"].TextAlignment = (TextAlignment)3;
			GrdDetailHistory.RootTable.Columns["AddLessSale"].FormatString = "#,##0.##";
			GrdDetailHistory.RootTable.Columns["AddLessSale"].TotalFormatString = "#,##0.##";
			GrdDetailHistory.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			GrdDetailHistory.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			GrdDetailHistory.RootTable.Columns["GrossWeight"].FormatString = "#,##0.##";
			GrdDetailHistory.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.##";
			GrdDetailHistory.RootTable.Columns["PurchaseBillWeight"].AggregateFunction = (AggregateFunction)2;
			GrdDetailHistory.RootTable.Columns["PurchaseBillWeight"].TextAlignment = (TextAlignment)3;
			GrdDetailHistory.RootTable.Columns["PurchaseBillWeight"].FormatString = "#,##0.##";
			GrdDetailHistory.RootTable.Columns["PurchaseBillWeight"].TotalFormatString = "#,##0.##";
			GrdDetailHistory.RootTable.Columns["SaleBillWeight"].AggregateFunction = (AggregateFunction)2;
			GrdDetailHistory.RootTable.Columns["SaleBillWeight"].TextAlignment = (TextAlignment)3;
			GrdDetailHistory.RootTable.Columns["SaleBillWeight"].FormatString = "#,##0.##";
			GrdDetailHistory.RootTable.Columns["SaleBillWeight"].TotalFormatString = "#,##0.##";
			GrdDetailHistory.RootTable.Columns["RatePurchase"].TextAlignment = (TextAlignment)3;
			GrdDetailHistory.RootTable.Columns["RatePurchase"].FormatString = clsGlobalVariables.DecimalRateFormate;
			GrdDetailHistory.RootTable.Columns["RateSale"].TextAlignment = (TextAlignment)3;
			GrdDetailHistory.RootTable.Columns["RateSale"].FormatString = clsGlobalVariables.DecimalRateFormate;
			GrdDetailHistory.RootTable.Columns["PurchaseAmount"].AggregateFunction = (AggregateFunction)2;
			GrdDetailHistory.RootTable.Columns["PurchaseAmount"].TextAlignment = (TextAlignment)3;
			GrdDetailHistory.RootTable.Columns["PurchaseAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GrdDetailHistory.RootTable.Columns["PurchaseAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GrdDetailHistory.RootTable.Columns["PurchaseFreight"].AggregateFunction = (AggregateFunction)2;
			GrdDetailHistory.RootTable.Columns["PurchaseFreight"].TextAlignment = (TextAlignment)3;
			GrdDetailHistory.RootTable.Columns["PurchaseFreight"].FormatString = clsGlobalVariables.stringFormatsingle;
			GrdDetailHistory.RootTable.Columns["PurchaseFreight"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GrdDetailHistory.RootTable.Columns["PurchaseExpenses"].AggregateFunction = (AggregateFunction)2;
			GrdDetailHistory.RootTable.Columns["PurchaseExpenses"].TextAlignment = (TextAlignment)3;
			GrdDetailHistory.RootTable.Columns["PurchaseExpenses"].FormatString = clsGlobalVariables.stringFormatsingle;
			GrdDetailHistory.RootTable.Columns["PurchaseExpenses"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GrdDetailHistory.RootTable.Columns["PurchaseNetAmount"].AggregateFunction = (AggregateFunction)2;
			GrdDetailHistory.RootTable.Columns["PurchaseNetAmount"].TextAlignment = (TextAlignment)3;
			GrdDetailHistory.RootTable.Columns["PurchaseNetAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GrdDetailHistory.RootTable.Columns["PurchaseNetAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GrdDetailHistory.RootTable.Columns["SaleAmount"].AggregateFunction = (AggregateFunction)2;
			GrdDetailHistory.RootTable.Columns["SaleAmount"].TextAlignment = (TextAlignment)3;
			GrdDetailHistory.RootTable.Columns["SaleAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GrdDetailHistory.RootTable.Columns["SaleAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GrdDetailHistory.RootTable.Columns["SaleExpenses"].AggregateFunction = (AggregateFunction)2;
			GrdDetailHistory.RootTable.Columns["SaleExpenses"].TextAlignment = (TextAlignment)3;
			GrdDetailHistory.RootTable.Columns["SaleExpenses"].FormatString = clsGlobalVariables.stringFormatsingle;
			GrdDetailHistory.RootTable.Columns["SaleExpenses"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GrdDetailHistory.RootTable.Columns["SaleNetAmount"].AggregateFunction = (AggregateFunction)2;
			GrdDetailHistory.RootTable.Columns["SaleNetAmount"].TextAlignment = (TextAlignment)3;
			GrdDetailHistory.RootTable.Columns["SaleNetAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GrdDetailHistory.RootTable.Columns["SaleNetAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = GrdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && GrdHistory.CurrentColumn != null)
			{
				if (e.Column.Key == "Edit")
				{
					Reset();
					RecId = Conversion.ToInt(GrdHistory.CurrentRow.Cells["Id"].Value);
					ReadById(RecId);
				}
				else if (e.Column.Key == "Print")
				{
					CommissionAgentSlip(Conversion.ToInt(GrdHistory.CurrentRow.Cells["Id"].Value), "168-CommisionTradeBillAndSlip.rpt");
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbperemeter_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbperemeter.Value) == 1)
			{
				fromdate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 2)
			{
				fromdate.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				fromdate.Value = first;
				ToDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				fromdate.Value = first2;
				ToDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 5)
			{
				fromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
				ToDate.Value = DateTime.Now;
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
			ParameterFill();
			txtOrderNoFrom.Text = string.Empty;
			txtOrderNoTo.Text = string.Empty;
			((Control)(object)cmbtradAcHistory).Text = string.Empty;
			((Control)(object)cmbsupplierhistory).Text = string.Empty;
			((Control)(object)cmbcusthistory).Text = string.Empty;
			cmbperemeter.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshowHistory_Click(object sender, EventArgs e)
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

	private void ComboFillHistory()
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		try
		{
			DataTable dtAccount = new DataTable();
			dtAccount.Columns.Add("Id");
			dtAccount.Columns.Add("name");
			DataTable dtsupplier = new DataTable();
			dtsupplier.Columns.Add("Id");
			dtsupplier.Columns.Add("name");
			DataTable dtcustomer = new DataTable();
			dtcustomer.Columns.Add("Id");
			dtcustomer.Columns.Add("name");
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtBindCombo = InvCommAgentTradeBill.GetDataForDropDownFromCommOrder(obj);
			if (dtBindCombo.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtBindCombo.Rows.Count; i++)
			{
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "TradingAccount")
				{
					dtAccount.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "Supplier")
				{
					dtsupplier.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "Customer")
				{
					dtcustomer.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
			}
			if (dtAccount.Rows.Count > 0)
			{
				DDL.BindDDL(dtAccount, cmbtradAcHistory, "Id", "name", "Trading Account", ZeroIndex: false);
				((UltraGridBase)cmbtradAcHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtsupplier.Rows.Count > 0)
			{
				DDL.BindDDL(dtsupplier, cmbsupplierhistory, "Id", "name", "Supplier", ZeroIndex: false);
				((UltraGridBase)cmbsupplierhistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtcustomer.Rows.Count > 0)
			{
				DDL.BindDDL(dtcustomer, cmbcusthistory, "Id", "name", "Customer", ZeroIndex: false);
				((UltraGridBase)cmbcusthistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshowHistory_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name != null && base.ActiveControl.Name == "GrdHistory")
			{
				cmbperemeter.Focus();
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
			Reset();
			RecId = Conversion.ToInt((object)GrdHistory.CurrentRow.Cells["Id"].Value.ToString());
			ReadById(RecId);
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
			GridEXRow item = GrdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				InvCommAgentTradeBill CommAgentBill = InvCommAgentTradeBill.GetByID(Conversion.ToInt(item.Cells["Id"].Value));
				DataTable dthistorydetail = new DataTable();
				dthistorydetail.Columns.Add("ItemCode");
				dthistorydetail.Columns.Add("ItemName");
				dthistorydetail.Columns.Add("CropYear");
				dthistorydetail.Columns.Add("PackTypeDesc");
				dthistorydetail.Columns.Add("UOMCode");
				dthistorydetail.Columns.Add("Qty", typeof(double));
				dthistorydetail.Columns.Add("GrossWeight", typeof(double));
				dthistorydetail.Columns.Add("PurchaseEbCut", typeof(double));
				dthistorydetail.Columns.Add("PurchaseEbCutTotal", typeof(double));
				dthistorydetail.Columns.Add("PurchaseBillWeight", typeof(double));
				dthistorydetail.Columns.Add("SaleGrossWeight", typeof(double));
				dthistorydetail.Columns.Add("SaleEbCut", typeof(double));
				dthistorydetail.Columns.Add("SaleEbCutTotal", typeof(double));
				dthistorydetail.Columns.Add("AddLessSale", typeof(double));
				dthistorydetail.Columns.Add("SaleBillWeight", typeof(double));
				dthistorydetail.Columns.Add("RatePurchase", typeof(double));
				dthistorydetail.Columns.Add("RateSale", typeof(double));
				dthistorydetail.Columns.Add("PurchaseAmount", typeof(double));
				dthistorydetail.Columns.Add("PurchaseFreight", typeof(double));
				dthistorydetail.Columns.Add("PurchaseExpenses", typeof(double));
				dthistorydetail.Columns.Add("PurchaseNetAmount", typeof(double));
				dthistorydetail.Columns.Add("SaleAmount", typeof(double));
				dthistorydetail.Columns.Add("SaleExpenses", typeof(double));
				dthistorydetail.Columns.Add("SaleNetAmount", typeof(double));
				dthistorydetail.Columns.Add("VehicleNo");
				dthistorydetail.Columns.Add("RefDocNo");
				dthistorydetail.Columns.Add("RemarksDetail");
				dthistorydetail.Clear();
				for (int i = 0; i < CommAgentBill.InvCommAgentTradeBillDetailslist.Count; i++)
				{
					dthistorydetail.Rows.Add(CommAgentBill.InvCommAgentTradeBillDetailslist[i].ItemCode, CommAgentBill.InvCommAgentTradeBillDetailslist[i].ItemName, CommAgentBill.InvCommAgentTradeBillDetailslist[i].CropYear, CommAgentBill.InvCommAgentTradeBillDetailslist[i].PackTypeDesc, CommAgentBill.InvCommAgentTradeBillDetailslist[i].PackUom, CommAgentBill.InvCommAgentTradeBillDetailslist[i].Qty, CommAgentBill.InvCommAgentTradeBillDetailslist[i].GrossWeight, CommAgentBill.InvCommAgentTradeBillDetailslist[i].EbCut, CommAgentBill.InvCommAgentTradeBillDetailslist[i].EbCutTotal, CommAgentBill.InvCommAgentTradeBillDetailslist[i].BillWeight, CommAgentBill.InvCommAgentTradeBillDetailslist[i].SaleGrossWeight, CommAgentBill.InvCommAgentTradeBillDetailslist[i].SaleEbCut, CommAgentBill.InvCommAgentTradeBillDetailslist[i].SaleEbCutTotal, CommAgentBill.InvCommAgentTradeBillDetailslist[i].AddLessSale, CommAgentBill.InvCommAgentTradeBillDetailslist[i].SaleBillWeight, CommAgentBill.InvCommAgentTradeBillDetailslist[i].RatePurchase, CommAgentBill.InvCommAgentTradeBillDetailslist[i].RateSale, CommAgentBill.InvCommAgentTradeBillDetailslist[i].ItemAmount, CommAgentBill.InvCommAgentTradeBillDetailslist[i].PurchaseFreight, CommAgentBill.InvCommAgentTradeBillDetailslist[i].PurchaseExpenses, CommAgentBill.InvCommAgentTradeBillDetailslist[i].PurchaseNetAmount, CommAgentBill.InvCommAgentTradeBillDetailslist[i].SaleAmount, CommAgentBill.InvCommAgentTradeBillDetailslist[i].SaleExpenses, CommAgentBill.InvCommAgentTradeBillDetailslist[i].SaleNetAmount, CommAgentBill.InvCommAgentTradeBillDetailslist[i].VehicleNo, CommAgentBill.InvCommAgentTradeBillDetailslist[i].RefDocNo, CommAgentBill.InvCommAgentTradeBillDetailslist[i].RemarksDetail);
				}
				GrdDetailHistory.DataSource = dthistorydetail;
				GrdDetailHistory.RetrieveStructure();
				gridhistorydetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnGrnFormHistory_Click(object sender, EventArgs e)
	{
		try
		{
			new CommissionAgentTrade_Register(UserAccount).Show();
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
			if (Conversion.ToDouble((object)txtQuantity.Text) > 0.0 && ((UltraGridBase)CmbPackUom).ActiveRow != null && Conversion.ToInt(CmbPackUom.Value) > 0)
			{
				double num = double.Parse(txtQuantity.Text);
				double PackUom = Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value);
				double Weight = num * PackUom;
				txtPurGrossWeight.Text = Weight.ToString("#,##0.##");
				txtSaleGrossWeight.Text = Weight.ToString("#,##0.##");
			}
			else
			{
				txtPurGrossWeight.Text = 0.ToString();
				txtSaleGrossWeight.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateBillWeightSale()
	{
		try
		{
			if (Conversion.ToDouble((object)txtQuantity.Text) > 0.0 || Conversion.ToDouble((object)txtEBTotalSale.Text) > 0.0)
			{
				double qty = Conversion.ToDouble((object)txtQuantity.Text);
				double Ebunit = Conversion.ToDouble((object)txtEBPerUnitSale.Text);
				double EbWeightSale = Conversion.ToDouble((object)txtEBTotalSale.Text);
				if (Conversion.ToString(base.ActiveControl.Tag) == "SaleEbTotal")
				{
					Ebunit = ((EbWeightSale > 0.0) ? (EbWeightSale / qty) : 0.0);
					txtEBPerUnitSale.Text = Ebunit.ToString("#,##0.###");
				}
				else if (Conversion.ToString(base.ActiveControl.Tag) == "SaleEBUnit")
				{
					txtEBTotalSale.Text = ((qty > 0.0) ? (qty * Ebunit) : 0.0).ToString("#,##0.###");
				}
			}
			double AddLessSale = Conversion.ToDouble((object)txtAddlessWtSales.Text);
			if (Conversion.ToDouble((object)txtEBTotalSale.Text) > 0.0)
			{
				txtBillWeightSale.Text = (Conversion.ToDouble((object)txtSaleGrossWeight.Text) - Conversion.ToDouble((object)txtEBTotalSale.Text) + AddLessSale).ToString("#,##0.##");
				return;
			}
			txtEBTotalSale.Text = 0.ToString();
			txtBillWeightSale.Text = (Conversion.ToDouble((object)txtSaleGrossWeight.Text) + AddLessSale).ToString("#,##0.##");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CalculateBillWeightPurchase()
	{
		try
		{
			if (Conversion.ToDouble((object)txtQuantity.Text) > 0.0 || Conversion.ToDouble((object)txtEBTotalPurchase.Text) > 0.0)
			{
				double qty = Conversion.ToDouble((object)txtQuantity.Text);
				double Ebunit = Conversion.ToDouble((object)txtEBPerUnitPurchase.Text);
				double EbWeightPurchase = Conversion.ToDouble((object)txtEBTotalPurchase.Text);
				if (Conversion.ToString(base.ActiveControl.Tag) == "PurchaseEbTotal")
				{
					Ebunit = ((EbWeightPurchase > 0.0) ? (EbWeightPurchase / qty) : 0.0);
					txtEBPerUnitPurchase.Text = Ebunit.ToString("#,##0.###");
				}
				else if (Conversion.ToString(base.ActiveControl.Tag) == "EbUnitPurchase")
				{
					txtEBTotalPurchase.Text = ((qty > 0.0) ? (qty * Ebunit) : 0.0).ToString("#,##0.###");
				}
			}
			if (Conversion.ToDouble((object)txtEBTotalPurchase.Text) > 0.0)
			{
				txtBillWeightPurchase.Text = (Conversion.ToDouble((object)txtPurGrossWeight.Text) - Conversion.ToDouble((object)txtEBTotalPurchase.Text)).ToString("#,##0.###");
				return;
			}
			txtEBTotalPurchase.Text = 0.ToString();
			txtBillWeightPurchase.Text = Conversion.ToDouble((object)txtPurGrossWeight.Text).ToString("#,##0.##");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CalculateAmount()
	{
		try
		{
			if (Conversion.ToDouble((object)txtRatePurchase.Text) > 0.0 && Conversion.ToDouble((object)txtBillWeightPurchase.Text) > 0.0 && ((UltraGridBase)CmbRateUom).ActiveRow != null && Conversion.ToInt(CmbRateUom.Value) > 0)
			{
				double purchaseRate = Conversion.ToDouble((object)txtRatePurchase.Text);
				double num = double.Parse(txtBillWeightPurchase.Text);
				double RateUom2 = Conversion.ToDouble(((UltraDropDownBase)CmbRateUom).SelectedRow.Cells[2].Value);
				double PurAmount = num / RateUom2 * purchaseRate;
				txtPurchaseAmount.Text = PurAmount.ToString(clsGlobalVariables.stringFormatsingle);
			}
			else
			{
				txtPurchaseAmount.Text = "0";
			}
			if (Conversion.ToDouble((object)txtRateSale.Text) > 0.0 && Conversion.ToDouble((object)txtBillWeightSale.Text) > 0.0 && ((UltraGridBase)CmbRateUom).ActiveRow != null && Conversion.ToInt(CmbRateUom.Value) > 0)
			{
				double SaleRate = Conversion.ToDouble((object)txtRateSale.Text);
				double num2 = double.Parse(txtBillWeightSale.Text);
				double RateUom = Conversion.ToDouble(((UltraDropDownBase)CmbRateUom).SelectedRow.Cells[2].Value);
				double SaleAmount = num2 / RateUom * SaleRate;
				txtSaleAmount.Text = SaleAmount.ToString(clsGlobalVariables.stringFormatsingle);
			}
			else
			{
				txtSaleAmount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPackUom_Leave(object sender, EventArgs e)
	{
		try
		{
			CalculateGrossWeight();
			CalculateBillWeightPurchase();
			CalculateBillWeightSale();
			CalculateAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQuantity_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateGrossWeight();
			CalculateBillWeightPurchase();
			CalculateBillWeightSale();
			CalculateAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtGrossWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateBillWeightPurchase();
			CalculateAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtEBPerUnit_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateBillWeightPurchase();
			CalculateAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtEBTotalPurchase_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateBillWeightPurchase();
			CalculateAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtSaleGrossWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateBillWeightSale();
			CalculateAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtEBPerUnitSale_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateBillWeightSale();
			CalculateAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtEBTotalSale_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateBillWeightSale();
			CalculateAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtAddlessWtSales_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateBillWeightSale();
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

	private void txtSupplierRate_TextChanged(object sender, EventArgs e)
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

	private void txtCustomerRate_TextChanged(object sender, EventArgs e)
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

	private void CalculateCommAndBrokeryAmount()
	{
		try
		{
			double SupplierCommAmount = 0.0;
			double CustomerBrokeryAmount = 0.0;
			double SupplierBrokeryAmount = 0.0;
			double CustomerCommAmount = 0.0;
			double purchaseAmount = ((grdDetail.GetRows().Count() > 0) ? Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["PurchaseAmount"], (AggregateFunction)2)) : 0.0);
			double SaleAmount = ((grdDetail.GetRows().Count() > 0) ? Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["SaleNetAmount"], (AggregateFunction)2)) : 0.0);
			double SupplierCommRate = Conversion.ToDouble((object)txtCommPercentSupp.Text);
			if (SupplierCommRate > 0.0 && purchaseAmount > 0.0)
			{
				SupplierCommAmount = SupplierCommRate * purchaseAmount / 100.0;
				txtCommAmountSupp.Text = SupplierCommAmount.ToString(clsGlobalVariables.stringFormatboth);
			}
			else
			{
				txtCommAmountSupp.Text = "0";
			}
			double SupplierBrokeryRate = Conversion.ToDouble((object)txtBrokeryPercentSupp.Text);
			if (SupplierBrokeryRate > 0.0 && purchaseAmount > 0.0)
			{
				SupplierBrokeryAmount = SupplierBrokeryRate * purchaseAmount / 100.0;
				txtBrokeryAmountSupp.Text = SupplierBrokeryAmount.ToString(clsGlobalVariables.stringFormatboth);
			}
			else
			{
				txtBrokeryAmountSupp.Text = "0";
			}
			double CustomerCommRate = Conversion.ToDouble((object)txtCommPercentCust.Text);
			if (CustomerCommRate > 0.0 && SaleAmount > 0.0)
			{
				CustomerCommAmount = CustomerCommRate * SaleAmount / 100.0;
				txtCommAmountCust.Text = CustomerCommAmount.ToString(clsGlobalVariables.stringFormatboth);
			}
			else
			{
				txtCommAmountCust.Text = "0";
			}
			double CustomerBrokeryRate = Conversion.ToDouble((object)txtBrokeryPercentCust.Text);
			if (CustomerBrokeryRate > 0.0 && SaleAmount > 0.0)
			{
				CustomerBrokeryAmount = CustomerBrokeryRate * SaleAmount / 100.0;
				txtBrokeryAmountCust.Text = CustomerBrokeryAmount.ToString(clsGlobalVariables.stringFormatboth);
			}
			else
			{
				txtBrokeryAmountCust.Text = "0";
			}
			SupplierBillAmount();
			CustomerBillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtCommPercent_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateCommAndBrokeryAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
			throw;
		}
	}

	private void txtBrokeryPercent_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateCommAndBrokeryAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CustomerBillAmount()
	{
		try
		{
			string DeliveryTerm = Conversion.ToString((object)((Control)(object)CmbDeliveryTermSale).Text);
			double SaleAmount = 0.0;
			double FreightAmountPurchase = 0.0;
			double ExpAmountSale = 0.0;
			double PMAmountSale = 0.0;
			SaleAmount = ((grdDetail.GetRows().Count() > 0) ? Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["SaleAmount"], (AggregateFunction)2)) : 0.0);
			if (((UltraGridBase)CmbCustomer).ActiveRow != null && Conversion.ToInt(CmbCustomer.Value) > 0 && grdFreightPurchase.RowCount > 0)
			{
				int glId2 = CommonServices.GetGlAccountIdBySupplierCustomerId(Conversion.ToInt(CmbCustomer.Value));
				FreightAmountPurchase = (from GridEXRow row in grdFreightPurchase.GetRows()
					where Conversion.ToInt(row.Cells["AccountId"].Value) == glId2
					select (double)row.Cells["Amount"].Value).Sum();
				if (FreightAmountPurchase != 0.0)
				{
					SaleAmount -= FreightAmountPurchase;
				}
			}
			ExpAmountSale = Conversion.ToDouble(grdInvExpForSale.GetTotal(grdInvExpForSale.RootTable.Columns["Amount"], (AggregateFunction)2));
			if (ExpAmountSale != 0.0)
			{
				SaleAmount += ExpAmountSale;
			}
			if (DeliveryTerm == "Load")
			{
				PMAmountSale = Conversion.ToDouble(grdPackingMaterialSale.GetTotal(grdPackingMaterialSale.RootTable.Columns["Amount"], (AggregateFunction)2));
				SaleAmount += PMAmountSale;
			}
			else if (DeliveryTerm == "Ponch")
			{
				int CustomerId = Conversion.ToInt(CmbCustomer.Value);
				int glId = CommonServices.GetGlAccountIdBySupplierCustomerId(CustomerId);
				PMAmountSale = (from GridEXRow row in grdPackingMaterialSale.GetRows()
					where Conversion.ToInt(row.Cells["AccountId"].Value) == glId
					select (double)row.Cells["Amount"].Value).Sum();
				if (PMAmountSale != 0.0)
				{
					SaleAmount -= PMAmountSale;
				}
			}
			if (((UltraGridBase)CmbCommAgentSupp).ActiveRow != null && Conversion.ToInt(CmbCommAgentSupp.Value) > 0)
			{
				if (Conversion.ToInt(CmbCustomer.Value) == Conversion.ToInt(CmbCommAgentSupp.Value))
				{
					SaleAmount -= Conversion.ToDouble((object)txtCommAmountSupp.Text);
				}
				if (Conversion.ToInt(CmbCommDrAcSupp.Value) > 0 && CommonServices.GetGlAccountIdBySupplierCustomerId(Conversion.ToInt(CmbCustomer.Value)) == Conversion.ToInt(CmbCommDrAcSupp.Value))
				{
					SaleAmount += Conversion.ToDouble((object)txtCommAmountSupp.Text);
				}
			}
			if (((UltraGridBase)CmbCommAgentCust).ActiveRow != null && Conversion.ToInt(CmbCommAgentCust.Value) > 0)
			{
				if (Conversion.ToInt(CmbCustomer.Value) == Conversion.ToInt(CmbCommAgentCust.Value))
				{
					SaleAmount -= Conversion.ToDouble((object)txtCommAmountCust.Text);
				}
				if (Conversion.ToInt(CmbCommDrAcCust.Value) > 0 && CommonServices.GetGlAccountIdBySupplierCustomerId(Conversion.ToInt(CmbCustomer.Value)) == Conversion.ToInt(CmbCommDrAcCust.Value))
				{
					SaleAmount += Conversion.ToDouble((object)txtCommAmountCust.Text);
				}
			}
			if (((UltraGridBase)CmbBrokerSupp).ActiveRow != null && Conversion.ToInt(CmbBrokerSupp.Value) > 0)
			{
				if (Conversion.ToInt(CmbCustomer.Value) == Conversion.ToInt(CmbBrokerSupp.Value))
				{
					SaleAmount -= Conversion.ToDouble((object)txtBrokeryAmountSupp.Text);
				}
				if (Conversion.ToInt(CmbBrokeryDrAcSupp.Value) > 0 && CommonServices.GetGlAccountIdBySupplierCustomerId(Conversion.ToInt(CmbCustomer.Value)) == Conversion.ToInt(CmbBrokeryDrAcSupp.Value))
				{
					SaleAmount += Conversion.ToDouble((object)txtBrokeryAmountSupp.Text);
				}
			}
			if (((UltraGridBase)CmbBrokerCust).ActiveRow != null && Conversion.ToInt(CmbBrokerCust.Value) > 0)
			{
				if (Conversion.ToInt(CmbCustomer.Value) == Conversion.ToInt(CmbBrokerCust.Value))
				{
					SaleAmount -= Conversion.ToDouble((object)txtBrokeryAmountCust.Text);
				}
				if (Conversion.ToInt(CmbBrokeryDrAcCust.Value) > 0 && CommonServices.GetGlAccountIdBySupplierCustomerId(Conversion.ToInt(CmbCustomer.Value)) == Conversion.ToInt(CmbBrokeryDrAcCust.Value))
				{
					SaleAmount += Conversion.ToDouble((object)txtBrokeryAmountCust.Text);
				}
			}
			txtSaleBillAmount.Text = SaleAmount.ToString(clsGlobalVariables.stringFormatboth);
			CalculateProfitLoss();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierBillAmount()
	{
		try
		{
			string DeliveryTerm = Conversion.ToString((object)((Control)(object)CmbDeliveryTerms).Text);
			double PurchaseAmount = 0.0;
			double ExpAmountPurchase = 0.0;
			double FreightAmountPurchase = 0.0;
			double PMAmountSale = 0.0;
			PurchaseAmount = ((grdDetail.GetRows().Count() > 0) ? Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["PurchaseAmount"], (AggregateFunction)2)) : 0.0);
			ExpAmountPurchase = Conversion.ToDouble(grdInvExpForPurchase.GetTotal(grdInvExpForPurchase.RootTable.Columns["Amount"], (AggregateFunction)2));
			PurchaseAmount += ExpAmountPurchase;
			if (((UltraGridBase)CmbSupplier).ActiveRow != null && Conversion.ToInt(CmbSupplier.Value) > 0 && grdFreightPurchase.RowCount > 0)
			{
				if (DeliveryTerm == "Load")
				{
					int glId2 = CommonServices.GetGlAccountIdBySupplierCustomerId(Conversion.ToInt(CmbSupplier.Value));
					FreightAmountPurchase = (from GridEXRow row in grdFreightPurchase.GetRows()
						where Conversion.ToInt(row.Cells["AccountId"].Value) == glId2
						select (double)row.Cells["Amount"].Value).Sum();
					PurchaseAmount += FreightAmountPurchase;
				}
				else if (DeliveryTerm == "Ponch")
				{
					FreightAmountPurchase = Conversion.ToDouble(grdFreightPurchase.GetTotal(grdFreightPurchase.RootTable.Columns["Amount"], (AggregateFunction)2));
					PurchaseAmount -= FreightAmountPurchase;
				}
			}
			if (((UltraGridBase)CmbSupplier).ActiveRow != null && Conversion.ToInt(CmbSupplier.Value) > 0 && grdPackingMaterialSale.RowCount > 0)
			{
				int glId = CommonServices.GetGlAccountIdBySupplierCustomerId(Conversion.ToInt(CmbSupplier.Value));
				PMAmountSale = (from GridEXRow row in grdPackingMaterialSale.GetRows()
					where Conversion.ToInt(row.Cells["AccountId"].Value) == glId
					select (double)row.Cells["Amount"].Value).Sum();
				PurchaseAmount += PMAmountSale;
			}
			if (((UltraGridBase)CmbCommAgentSupp).ActiveRow != null && Conversion.ToInt(CmbCommAgentSupp.Value) > 0)
			{
				if (Conversion.ToInt(CmbSupplier.Value) == Conversion.ToInt(CmbCommAgentSupp.Value))
				{
					PurchaseAmount += Conversion.ToDouble((object)txtCommAmountSupp.Text);
				}
				if (Conversion.ToInt(CmbCommDrAcSupp.Value) > 0 && CommonServices.GetGlAccountIdBySupplierCustomerId(Conversion.ToInt(CmbSupplier.Value)) == Conversion.ToInt(CmbCommDrAcSupp.Value))
				{
					PurchaseAmount -= Conversion.ToDouble((object)txtCommAmountSupp.Text);
				}
			}
			if (((UltraGridBase)CmbCommAgentCust).ActiveRow != null && Conversion.ToInt(CmbCommAgentCust.Value) > 0)
			{
				if (Conversion.ToInt(CmbSupplier.Value) == Conversion.ToInt(CmbCommAgentCust.Value))
				{
					PurchaseAmount += Conversion.ToDouble((object)txtCommAmountCust.Text);
				}
				if (Conversion.ToInt(CmbCommDrAcCust.Value) > 0 && CommonServices.GetGlAccountIdBySupplierCustomerId(Conversion.ToInt(CmbSupplier.Value)) == Conversion.ToInt(CmbCommDrAcCust.Value))
				{
					PurchaseAmount -= Conversion.ToDouble((object)txtCommAmountCust.Text);
				}
			}
			if (((UltraGridBase)CmbBrokerSupp).ActiveRow != null && Conversion.ToInt(CmbBrokerSupp.Value) > 0)
			{
				if (Conversion.ToInt(CmbSupplier.Value) == Conversion.ToInt(CmbBrokerSupp.Value))
				{
					PurchaseAmount += Conversion.ToDouble((object)txtBrokeryAmountSupp.Text);
				}
				if (Conversion.ToInt(CmbBrokeryDrAcSupp.Value) > 0 && CommonServices.GetGlAccountIdBySupplierCustomerId(Conversion.ToInt(CmbSupplier.Value)) == Conversion.ToInt(CmbBrokeryDrAcSupp.Value))
				{
					PurchaseAmount -= Conversion.ToDouble((object)txtBrokeryAmountSupp.Text);
				}
			}
			if (((UltraGridBase)CmbBrokerCust).ActiveRow != null && Conversion.ToInt(CmbBrokerCust.Value) > 0)
			{
				if (Conversion.ToInt(CmbSupplier.Value) == Conversion.ToInt(CmbBrokerCust.Value))
				{
					PurchaseAmount += Conversion.ToDouble((object)txtBrokeryAmountCust.Text);
				}
				if (Conversion.ToInt(CmbBrokeryDrAcCust.Value) > 0 && CommonServices.GetGlAccountIdBySupplierCustomerId(Conversion.ToInt(CmbSupplier.Value)) == Conversion.ToInt(CmbBrokeryDrAcCust.Value))
				{
					PurchaseAmount -= Conversion.ToDouble((object)txtBrokeryAmountCust.Text);
				}
			}
			txtPurchaseBillAmount.Text = PurchaseAmount.ToString(clsGlobalVariables.stringFormatboth);
			CalculateProfitLoss();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateProfitLoss()
	{
		try
		{
			string DeliveryTermPurchase = Conversion.ToString((object)((Control)(object)CmbDeliveryTerms).Text);
			string DeliveryTermSale = Conversion.ToString((object)((Control)(object)CmbDeliveryTermSale).Text);
			double TradingDebitAmount = 0.0;
			double TradingCreditAmount = 0.0;
			double FreightGrid = 0.0;
			double PmSaleGrid = 0.0;
			TradingDebitAmount += ((grdDetail.GetRows().Count() > 0) ? Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["PurchaseAmount"], (AggregateFunction)2)) : 0.0);
			TradingCreditAmount += ((grdDetail.GetRows().Count() > 0) ? Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["SaleAmount"], (AggregateFunction)2)) : 0.0);
			TradingDebitAmount += ((grdInvExpForPurchase.GetRows().Count() > 0) ? Conversion.ToDouble(grdInvExpForPurchase.GetTotal(grdInvExpForPurchase.RootTable.Columns["Amount"], (AggregateFunction)2)) : 0.0);
			if (DeliveryTermPurchase == "Load")
			{
				FreightGrid = ((grdFreightPurchase.GetRows().Count() > 0) ? Conversion.ToDouble(grdFreightPurchase.GetTotal(grdFreightPurchase.RootTable.Columns["Amount"], (AggregateFunction)2)) : 0.0);
				TradingDebitAmount += FreightGrid;
			}
			else if (DeliveryTermPurchase == "Ponch")
			{
				FreightGrid = (from GridEXRow row in grdFreightPurchase.GetRows()
					where Conversion.ToInt(row.Cells["AccountId"].Value) == Conversion.ToInt(CmbTradingAccount.Value)
					select (double)row.Cells["Amount"].Value).Sum();
				TradingCreditAmount += FreightGrid;
			}
			TradingCreditAmount += ((grdInvExpForSale.GetRows().Count() > 0) ? Conversion.ToDouble(grdInvExpForSale.GetTotal(grdInvExpForSale.RootTable.Columns["Amount"], (AggregateFunction)2)) : 0.0);
			if (DeliveryTermSale == "Ponch")
			{
				PmSaleGrid = ((grdPackingMaterialSale.GetRows().Count() > 0) ? Conversion.ToDouble(grdPackingMaterialSale.GetTotal(grdPackingMaterialSale.RootTable.Columns["Amount"], (AggregateFunction)2)) : 0.0);
				TradingDebitAmount += PmSaleGrid;
			}
			else if (DeliveryTermSale == "Load")
			{
				PmSaleGrid = (from GridEXRow row in grdPackingMaterialSale.GetRows()
					where Conversion.ToInt(row.Cells["AccountId"].Value) == Conversion.ToInt(CmbTradingAccount.Value)
					select (double)row.Cells["Amount"].Value).Sum();
				TradingCreditAmount += PmSaleGrid;
			}
			if (((UltraGridBase)CmbCommAgentSupp).ActiveRow != null && Conversion.ToInt(CmbCommAgentSupp.Value) > 0)
			{
				int glId4 = CommonServices.GetGlAccountIdBySupplierCustomerId(Conversion.ToInt(CmbCommAgentSupp.Value));
				if (Conversion.ToInt(CmbTradingAccount.Value) == glId4)
				{
					TradingCreditAmount += Conversion.ToDouble((object)txtCommAmountSupp.Text);
				}
				if (Conversion.ToInt(CmbCommDrAcSupp.Value) > 0 && Conversion.ToInt(CmbTradingAccount.Value) == Conversion.ToInt(CmbCommDrAcSupp.Value))
				{
					TradingDebitAmount += Conversion.ToDouble((object)txtCommAmountSupp.Text);
				}
			}
			if (((UltraGridBase)CmbCommAgentCust).ActiveRow != null && Conversion.ToInt(CmbCommAgentCust.Value) > 0)
			{
				int glId3 = CommonServices.GetGlAccountIdBySupplierCustomerId(Conversion.ToInt(CmbCommAgentCust.Value));
				if (Conversion.ToInt(CmbTradingAccount.Value) == glId3)
				{
					TradingCreditAmount += Conversion.ToDouble((object)txtCommAmountCust.Text);
				}
				if (Conversion.ToInt(CmbCommDrAcCust.Value) > 0 && Conversion.ToInt(CmbTradingAccount.Value) == Conversion.ToInt(CmbCommDrAcCust.Value))
				{
					TradingDebitAmount += Conversion.ToDouble((object)txtCommAmountCust.Text);
				}
			}
			if (((UltraGridBase)CmbBrokerSupp).ActiveRow != null && Conversion.ToInt(CmbBrokerSupp.Value) > 0)
			{
				int glId2 = CommonServices.GetGlAccountIdBySupplierCustomerId(Conversion.ToInt(CmbBrokerSupp.Value));
				if (Conversion.ToInt(CmbTradingAccount.Value) == glId2)
				{
					TradingCreditAmount += Conversion.ToDouble((object)txtBrokeryAmountSupp.Text);
				}
				if (Conversion.ToInt(CmbBrokeryDrAcSupp.Value) > 0 && Conversion.ToInt(CmbTradingAccount.Value) == Conversion.ToInt(CmbBrokeryDrAcSupp.Value))
				{
					TradingDebitAmount += Conversion.ToDouble((object)txtBrokeryAmountSupp.Text);
				}
			}
			if (((UltraGridBase)CmbBrokerCust).ActiveRow != null && Conversion.ToInt(CmbBrokerCust.Value) > 0)
			{
				int glId = CommonServices.GetGlAccountIdBySupplierCustomerId(Conversion.ToInt(CmbBrokerCust.Value));
				if (Conversion.ToInt(CmbTradingAccount.Value) == glId)
				{
					TradingCreditAmount += Conversion.ToDouble((object)txtBrokeryAmountCust.Text);
				}
				if (Conversion.ToInt(CmbBrokeryDrAcCust.Value) > 0 && Conversion.ToInt(CmbTradingAccount.Value) == Conversion.ToInt(CmbBrokeryDrAcCust.Value))
				{
					TradingDebitAmount += Conversion.ToDouble((object)txtBrokeryAmountCust.Text);
				}
			}
			txtProfitLoss.Text = (TradingDebitAmount - TradingCreditAmount).ToString(clsGlobalVariables.stringFormatboth);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbDeliveryTermSale_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CustomerBillAmount();
			SupplierBillAmount();
			ExpenseProportionForPurchase();
			FreightProportion();
			ExpenseProportionForSale();
			BillProportionForSale();
			PLAmountProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbDeliveryTerms_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CustomerBillAmount();
			SupplierBillAmount();
			ExpenseProportionForPurchase();
			FreightProportion();
			ExpenseProportionForSale();
			BillProportionForSale();
			PLAmountProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExpenseProportionForPurchase()
	{
		try
		{
			double PurchaseAmount = ((grdDetail.GetRows().Count() > 0) ? Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["PurchaseAmount"], (AggregateFunction)2)) : 0.0);
			double Expense = Conversion.ToDouble(grdInvExpForPurchase.GetTotal(grdInvExpForPurchase.RootTable.Columns["Amount"], (AggregateFunction)2));
			if (PurchaseAmount > 0.0)
			{
				double TotalDiffernce = Expense;
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double Amount = Conversion.ToDouble((object)obj.Cells["PurchaseAmount"].Value.ToString());
					obj.Cells["PurchaseExpenses"].Value = Math.Round(TotalDiffernce) / PurchaseAmount * Amount;
					obj.EndEdit();
				}
			}
			else
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["PurchaseExpenses"].Value = 0;
				}
			}
			BillProportionForPurchase();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExpenseProportionForSale()
	{
		try
		{
			if (grdDetail.GetRows().Count() <= 0)
			{
				return;
			}
			double SaleAmount = ((grdDetail.GetRows().Count() > 0) ? Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["SaleAmount"], (AggregateFunction)2)) : 0.0);
			if (SaleAmount > 0.0)
			{
				double Expense = Conversion.ToDouble(grdInvExpForSale.GetTotal(grdInvExpForSale.RootTable.Columns["Amount"], (AggregateFunction)2));
				double PackingExpense = ((grdPackingMaterialSale.GetRows().Count() > 0) ? Conversion.ToDouble(grdPackingMaterialSale.GetTotal(grdPackingMaterialSale.RootTable.Columns["Amount"], (AggregateFunction)2)) : 0.0);
				if (((UltraGridBase)CmbTradingAccount).ActiveRow != null && Conversion.ToInt(CmbTradingAccount.Value) > 0)
				{
					PackingExpense = (from GridEXRow row in grdPackingMaterialSale.GetRows()
						where Conversion.ToInt(row.Cells["AccountId"].Value) == Conversion.ToInt(CmbTradingAccount.Value)
						select (double)row.Cells["Amount"].Value).Sum();
				}
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow obj in rows)
				{
					double TotalExpAmount = Expense + PackingExpense;
					TotalExpAmount = Math.Abs(TotalExpAmount);
					obj.BeginEdit();
					double Amount = Conversion.ToDouble(obj.Cells["SaleAmount"].Value);
					obj.Cells["SaleExpenses"].Value = TotalExpAmount / SaleAmount * Amount;
					obj.EndEdit();
				}
			}
			else
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["SaleExpenses"].Value = 0;
				}
			}
			BillProportionForSale();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BillProportionForPurchase()
	{
		try
		{
			if (((Control)(object)CmbDeliveryTerms).Text == "Load")
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow r in rows)
				{
					r.BeginEdit();
					r.Cells["PurchaseNetAmount"].Value = Conversion.ToDouble(r.Cells["PurchaseAmount"].Value) + Conversion.ToDouble(r.Cells["PurchaseExpenses"].Value) + Conversion.ToDouble(r.Cells["PurchaseFreight"].Value);
					r.EndEdit();
				}
			}
			else if (((Control)(object)CmbDeliveryTerms).Text == "Ponch")
			{
				double Freight = 0.0;
				if (((UltraGridBase)CmbTradingAccount).ActiveRow != null && Conversion.ToInt(CmbTradingAccount.Value) > 0)
				{
					Freight = (from GridEXRow row in grdFreightPurchase.GetRows()
						where Conversion.ToInt(row.Cells["AccountId"].Value) == Conversion.ToInt(CmbTradingAccount.Value)
						select (double)row.Cells["Amount"].Value).Sum();
				}
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					r2.BeginEdit();
					r2.Cells["PurchaseNetAmount"].Value = Conversion.ToDouble(r2.Cells["PurchaseAmount"].Value) + Conversion.ToDouble(r2.Cells["PurchaseExpenses"].Value) - Freight;
					r2.EndEdit();
				}
			}
			PLAmountProportion();
			CalculateCommAndBrokeryAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BillProportionForSale()
	{
		try
		{
			if (grdDetail.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow r in rows)
				{
					r.BeginEdit();
					r.Cells["SaleNetAmount"].Value = Conversion.ToDouble((object)r.Cells["SaleAmount"].Value.ToString()) + Conversion.ToDouble((object)r.Cells["SaleExpenses"].Value.ToString());
					r.EndEdit();
				}
			}
			PLAmountProportion();
			CalculateCommAndBrokeryAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PLAmountProportion()
	{
		try
		{
			if (grdDetail.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow r in rows)
				{
					r.BeginEdit();
					r.Cells["PLAmount"].Value = Conversion.ToDouble((object)r.Cells["PurchaseNetAmount"].Value.ToString()) - Conversion.ToDouble((object)r.Cells["SaleNetAmount"].Value.ToString());
					r.EndEdit();
				}
			}
			else
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					obj.Cells["PLAmount"].Value = 0;
					obj.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FreightProportion()
	{
		try
		{
			double NetWeight = Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["PurchaseBillWeight"], (AggregateFunction)2));
			double Freight = Conversion.ToDouble(grdFreightPurchase.GetTotal(grdFreightPurchase.RootTable.Columns["Amount"], (AggregateFunction)2));
			if (Freight > 0.0)
			{
				if (((Control)(object)CmbDeliveryTerms).Text == "Load")
				{
					GridEXRow[] rows = grdDetail.GetRows();
					foreach (GridEXRow obj in rows)
					{
						obj.BeginEdit();
						double Weight = Conversion.ToDouble((object)obj.Cells["PurchaseBillWeight"].Value.ToString());
						obj.Cells["PurchaseFreight"].Value = Freight / NetWeight * Weight;
						obj.EndEdit();
					}
					grdDetail.RootTable.Columns["PurchaseFreight"].Visible = true;
					grdDetail.RootTable.Columns["PurchaseFreightLess"].Visible = false;
				}
				else if (((Control)(object)CmbDeliveryTerms).Text == "Ponch")
				{
					Freight = (from GridEXRow row in grdFreightPurchase.GetRows()
						where Conversion.ToInt(row.Cells["AccountId"].Value) == Conversion.ToInt(CmbTradingAccount.Value)
						select (double)row.Cells["Amount"].Value).Sum();
					GridEXRow[] rows = grdDetail.GetRows();
					foreach (GridEXRow obj2 in rows)
					{
						obj2.BeginEdit();
						double Weight2 = Conversion.ToDouble((object)obj2.Cells["PurchaseBillWeight"].Value.ToString());
						obj2.Cells["PurchaseFreightLess"].Value = Freight / NetWeight * Weight2;
						obj2.EndEdit();
					}
					grdDetail.RootTable.Columns["PurchaseFreight"].Visible = false;
					grdDetail.RootTable.Columns["PurchaseFreightLess"].Visible = true;
				}
			}
			else
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow obj3 in rows)
				{
					obj3.BeginEdit();
					obj3.Cells["PurchaseFreight"].Value = 0;
				}
			}
			BillProportionForPurchase();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPaymentTerms_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)CmbPaymentTerms).Text == "Cash")
			{
				txtDueDays.Enabled = false;
				txtDueDays.Text = "1";
			}
			else
			{
				txtDueDays.Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtDueDays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (txtDueDays.Text.Trim() != string.Empty && txtDueDays.Text != null)
			{
				txtDueDate.Value = txtDocDate.Value.AddDays(Conversion.ToDouble((object)txtDueDays.Text.Trim()));
			}
			else
			{
				txtDueDate.Value = txtDocDate.Value;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionAgentSlip(int PrintId, string ReportName)
	{
		try
		{
			CommonServices.CommisionTradeBillAndSlip168(PrintId, ReportName);
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
			CommissionAgentSlip(RecId, "168-CommisionTradeBillAndSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint103_Click(object sender, EventArgs e)
	{
		try
		{
			if (VoucherHeadId == 0)
			{
				throw new Exception("Record Not Found For Display");
			}
			CommonServices.VoucherReport_118(VoucherHeadId, 162);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint170_Click(object sender, EventArgs e)
	{
		try
		{
			CommissionAgentSlip(RecId, "170-CommisionTradeBillPurchaseSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint171_Click(object sender, EventArgs e)
	{
		try
		{
			CommissionAgentSlip(RecId, "171-CommisionTradeBillSaleSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnprint172_Click(object sender, EventArgs e)
	{
		try
		{
			CommissionAgentSlip(RecId, "172-CommisionTradeBillSaleSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnSlipNew1706_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.CommissionBillSuppCustndIncomeTransSlip1706(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtCommPercent_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtBrokeryPercent_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtSupplierRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtSaleGrossWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtCustomerRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtBillAmount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtRatePurchase_Leave(object sender, EventArgs e)
	{
	}

	private void txtOrderNoFrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtOrderNoTo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtAddlessWtSales_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunctionWithMinus(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtEBPerUnit_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtBrokeryPercentSupp_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtCommPercentCust_KeyPress(object sender, KeyPressEventArgs e)
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

	private void CommissionAgentTradeBill_162_KeyDown(object sender, KeyEventArgs e)
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
					CmbSupplier.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					cmbperemeter.Focus();
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
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.Shift && e.KeyCode == Keys.Delete && BtnDelete.Visible && BtnDelete.Enabled)
				{
					BtnDelete_Click(null, null);
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
					CmbSupplier.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdDetail).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grdDetail).Focused)
					{
						tabControl2.SelectedIndex = 0;
						((Control)(object)grdInvExpForPurchase).Focus();
					}
					else if (((Control)(object)grdInvExpForPurchase).Focused)
					{
						tabControl2.SelectedIndex = 1;
						((Control)(object)grdFreightPurchase).Focus();
					}
					else if (((Control)(object)grdFreightPurchase).Focused)
					{
						tabControl2.SelectedIndex = 2;
						((Control)(object)grdInvExpForSale).Focus();
					}
					else if (((Control)(object)grdInvExpForSale).Focused)
					{
						tabControl2.SelectedIndex = 3;
						((Control)(object)grdPackingMaterialSale).Focus();
					}
					else if (((Control)(object)grdPackingMaterialSale).Focused)
					{
						tabControl2.SelectedIndex = 4;
						CmbCommAgentSupp.Focus();
					}
					else
					{
						tabControl2.SelectedIndex = 0;
						((Control)(object)grdDetail).Focus();
					}
				}
				if (e.Control && e.KeyCode == Keys.Left)
				{
					if (tabControl2.SelectedIndex == 4)
					{
						tabControl2.SelectedIndex = 3;
						((Control)(object)grdPackingMaterialSale).Focus();
					}
					else if (((Control)(object)grdPackingMaterialSale).Focused)
					{
						tabControl2.SelectedIndex = 2;
						((Control)(object)grdInvExpForSale).Focus();
					}
					else if (((Control)(object)grdInvExpForSale).Focused)
					{
						tabControl2.SelectedIndex = 1;
						((Control)(object)grdFreightPurchase).Focus();
					}
					else if (((Control)(object)grdFreightPurchase).Focused)
					{
						tabControl2.SelectedIndex = 0;
						((Control)(object)grdInvExpForPurchase).Focus();
					}
					else if (((Control)(object)grdInvExpForPurchase).Focused)
					{
						tabControl2.SelectedIndex = 4;
						CmbCommAgentSupp.Focus();
					}
					else
					{
						tabControl2.SelectedIndex = 0;
						((Control)(object)grdDetail).Focus();
					}
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					CmbItemName.Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					AT.Show();
				}
				if (e.Control && e.KeyCode == Keys.P)
				{
					btnPrint_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1))
				{
					btnPrint_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.NumPad2 || e.KeyCode == Keys.D2))
				{
					btnPrint170_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.NumPad3 || e.KeyCode == Keys.D3))
				{
					btnPrint171_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.NumPad4 || e.KeyCode == Keys.D4))
				{
					btnprint172_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.NumPad5 || e.KeyCode == Keys.D5))
				{
					BtnSlipNew1706_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.NumPad6 || e.KeyCode == Keys.D6))
				{
					btnPrint103_Click(null, null);
				}
				return;
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				cmbperemeter.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)GrdDetailHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)GrdDetailHistory).Focused)
				{
					((Control)(object)GrdHistory).Focus();
				}
				else
				{
					((Control)(object)GrdDetailHistory).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				((Control)(object)GrdHistory).Focus();
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
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Alt+1", "For Print Slip 168");
			dt.Rows.Add("Alt+2", "For Print Slip 170");
			dt.Rows.Add("Alt+3", "For Print Slip 171");
			dt.Rows.Add("Alt+4", "For Print Slip 172");
			dt.Rows.Add("Alt+5", "For Print Slip 1706");
			dt.Rows.Add("Alt+6", "For Print Voucher 118");
			dt.Rows.Add("Ctrl+F5", "For Focus on Supplier Name");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+D", "For Adding an row in Focused Grid");
			dt.Rows.Add("Ctrl+Delete", "For Deleting an row of Focused Grid");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Customer Name in Detail Box");
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

	private void btnShortcutKeys_Click(object sender, EventArgs e)
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

	private void grdDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdDetail.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdDetail.CurrentColumn.Key == "Delete")
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					if (btnsave.Visible && btnsave.Enabled)
					{
						r.Delete();
					}
					else
					{
						InvCommAgentTradeBillDetail detail2 = new InvCommAgentTradeBillDetail();
						detail2.Id = Conversion.ToInt(r.Cells["Id"].Value);
						detail2.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						detail2.ItemName = Conversion.ToString((object)r.Cells["ItemName"].Text);
						detail2.Crop = Conversion.ToInt(r.Cells["CropYearId"].Value);
						detail2.CropYear = Conversion.ToString((object)r.Cells["CropYear"].Text);
						detail2.PackingTypeId = Conversion.ToInt(r.Cells["PackTypeId"].Value);
						detail2.PackTypeDesc = Conversion.ToString((object)r.Cells["PackTypeDesc"].Text);
						detail2.PackUomId = Conversion.ToInt(r.Cells["UomId"].Value);
						detail2.PackUom = Conversion.ToString((object)r.Cells["UomCode"].Text);
						detail2.Qty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
						detail2.GrossWeight = Conversion.ToDecimal(r.Cells["GrossWeight"].Value);
						detail2.EbCut = Conversion.ToDouble(r.Cells["PurchaseEbCut"].Value);
						detail2.EbCutTotal = Conversion.ToDouble(r.Cells["PurchaseEbCutTotal"].Value);
						detail2.BillWeight = Conversion.ToDecimal(r.Cells["PurchaseBillWeight"].Value);
						detail2.SaleGrossWeight = Conversion.ToDecimal(r.Cells["SaleGrossWeight"].Value);
						detail2.SaleEbCut = Conversion.ToDouble(r.Cells["SaleEbCut"].Value);
						detail2.SaleEbCutTotal = Conversion.ToDouble(r.Cells["SaleEbCutTotal"].Value);
						detail2.SaleBillWeight = Conversion.ToDecimal(r.Cells["SaleBillWeight"].Value);
						detail2.RatePurchase = Conversion.ToDecimal(r.Cells["RatePurchase"].Value);
						detail2.RateSale = Conversion.ToDecimal(r.Cells["RateSale"].Value);
						detail2.ItemAmount = Conversion.ToDouble(r.Cells["PurchaseAmount"].Value);
						detail2.SaleAmount = Conversion.ToDouble(r.Cells["SaleAmount"].Value);
						detail2.VehicleNo = Conversion.ToString(r.Cells["VehicleNo"].Value);
						detail2.RefDocNo = Conversion.ToString(r.Cells["RefDocNo"].Value);
						detail2.RemarksDetail = Conversion.ToString((object)r.Cells["RemarksDetail"].Text.Trim());
						detail2.ActionTypeId = 3;
						lstRemoveRecord.Add(detail2);
						dtDetail.Rows[r.RowIndex].Delete();
						r.Delete();
					}
					grdDetail.UpdateData();
					PLAmountProportion();
					CalculateCommAndBrokeryAmount();
				}
				if (grdDetail.CurrentColumn.Key == "Edit")
				{
					grdDetail_DoubleClick(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				if (btnsave.Visible && btnsave.Enabled)
				{
					r.Delete();
				}
				else
				{
					InvCommAgentTradeBillDetail detail = new InvCommAgentTradeBillDetail();
					detail.Id = Conversion.ToInt(r.Cells["Id"].Value);
					detail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					detail.ItemName = Conversion.ToString((object)r.Cells["ItemName"].Text);
					detail.Crop = Conversion.ToInt(r.Cells["CropYearId"].Value);
					detail.CropYear = Conversion.ToString((object)r.Cells["CropYear"].Text);
					detail.PackingTypeId = Conversion.ToInt(r.Cells["PackTypeId"].Value);
					detail.PackTypeDesc = Conversion.ToString((object)r.Cells["PackTypeDesc"].Text);
					detail.PackUomId = Conversion.ToInt(r.Cells["UomId"].Value);
					detail.PackUom = Conversion.ToString((object)r.Cells["UomCode"].Text);
					detail.Qty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
					detail.GrossWeight = Conversion.ToDecimal(r.Cells["GrossWeight"].Value);
					detail.EbCut = Conversion.ToDouble(r.Cells["PurchaseEbCut"].Value);
					detail.EbCutTotal = Conversion.ToDouble(r.Cells["PurchaseEbCutTotal"].Value);
					detail.BillWeight = Conversion.ToDecimal(r.Cells["PurchaseBillWeight"].Value);
					detail.SaleEbCut = Conversion.ToDouble(r.Cells["SaleEbCut"].Value);
					detail.SaleEbCutTotal = Conversion.ToDouble(r.Cells["SaleEbCutTotal"].Value);
					detail.SaleBillWeight = Conversion.ToDecimal(r.Cells["SaleBillWeight"].Value);
					detail.RatePurchase = Conversion.ToDecimal(r.Cells["RatePurchase"].Value);
					detail.RateSale = Conversion.ToDecimal(r.Cells["RateSale"].Value);
					detail.ItemAmount = Conversion.ToDouble(r.Cells["PurchaseAmount"].Value);
					detail.SaleAmount = Conversion.ToDouble(r.Cells["SaleAmount"].Value);
					detail.VehicleNo = Conversion.ToString(r.Cells["VehicleNo"].Value);
					detail.RefDocNo = Conversion.ToString(r.Cells["RefDocNo"].Value);
					detail.RemarksDetail = Conversion.ToString((object)r.Cells["RemarksDetail"].Text.Trim());
					detail.ActionTypeId = 3;
					lstRemoveRecord.Add(detail);
					dtDetail.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				grdDetail.UpdateData();
				PLAmountProportion();
				CalculateCommAndBrokeryAmount();
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdDetail_DoubleClick(null, null);
			}
			if (e.KeyCode == Keys.F1)
			{
				CommonServices.GridAutoAdjustmentNew(grdDetail);
			}
			((Control)(object)grdDetail).Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExpForSale_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdInvExpForSale.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdInvExpForSale.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdInvExpForSale.CurrentColumn.Key == "Delete")
				{
					dtInvExpForSale.Rows[item.RowIndex].Delete();
					if (item != null && (int)item.RowType == 0)
					{
						if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
						{
							return;
						}
						item.Delete();
					}
					if (dtInvExpForSale.Rows.Count == 0)
					{
						AddRowInSaleExpenseGrid();
					}
				}
				if (grdInvExpForSale.CurrentColumn.Key == "Add")
				{
					AddRowInSaleExpenseGrid();
				}
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				dtInvExpForSale.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					item.Delete();
				}
				if (dtInvExpForSale.Rows.Count == 0)
				{
					AddRowInSaleExpenseGrid();
				}
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInSaleExpenseGrid();
			}
			if (e.KeyCode == Keys.F1)
			{
				CommonServices.GridAutoAdjustmentNew(grdInvExpForSale);
			}
			((Control)(object)grdInvExpForSale).Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFreight_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdFreightPurchase.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdFreightPurchase.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdFreightPurchase.CurrentColumn.Key == "Delete")
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					dtFreight.Rows[item.RowIndex].Delete();
					item.Delete();
					if (dtFreight.Rows.Count == 0)
					{
						AddRowInFreightGrid();
					}
				}
				if (grdFreightPurchase.CurrentColumn.Key == "Add")
				{
					dtFreight.Rows.Add(item.Cells["AccountId"].Value, 0, 0, 0, 0, item.Cells["Remarks"].Value);
					grdFreightPurchase.DataSource = dtFreight;
				}
				grdFreightPurchase.UpdateData();
				FreightProportion();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				dtFreight.Rows[item.RowIndex].Delete();
				item.Delete();
				if (dtFreight.Rows.Count == 0)
				{
					AddRowInFreightGrid();
				}
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				dtFreight.Rows.Add(item.Cells["AccountId"].Value, 0, 0, 0, 0, item.Cells["Remarks"].Value);
				grdFreightPurchase.DataSource = dtFreight;
				grdFreightPurchase.UpdateData();
				FreightProportion();
			}
			if (e.KeyCode == Keys.F1)
			{
				CommonServices.GridAutoAdjustmentNew(grdFreightPurchase);
			}
			((Control)(object)grdFreightPurchase).Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdpackingMaterial_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPackingMaterialSale.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdPackingMaterialSale.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdPackingMaterialSale.CurrentColumn.Key == "Delete")
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					dtExpForPackingMaterial.Rows[item.RowIndex].Delete();
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
					if (dtExpForPackingMaterial.Rows.Count == 0)
					{
						AddRowInPackingMaterialGrid();
					}
				}
				if (grdPackingMaterialSale.CurrentColumn.Key == "Add")
				{
					dtExpForPackingMaterial.Rows.Add(item.Cells["AccountId"].Value, 0, 0, 0, 0, item.Cells["Remarks"].Value);
					grdPackingMaterialSale.DataSource = dtExpForPackingMaterial;
				}
				grdPackingMaterialSale.UpdateData();
				FreightProportion();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				dtExpForPackingMaterial.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtExpForPackingMaterial.Rows.Count == 0)
				{
					AddRowInPackingMaterialGrid();
				}
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				dtExpForPackingMaterial.Rows.Add(item.Cells["AccountId"].Value, 0, 0, 0, 0, item.Cells["Remarks"].Value);
				grdPackingMaterialSale.DataSource = dtExpForPackingMaterial;
				grdPackingMaterialSale.UpdateData();
				FreightProportion();
			}
			if (e.KeyCode == Keys.F1)
			{
				CommonServices.GridAutoAdjustmentNew(grdPackingMaterialSale);
			}
			((Control)(object)grdPackingMaterialSale).Focus();
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
			GridEXRow item = GrdHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || GrdHistory.CurrentColumn == null)
			{
				return;
			}
			int CurrId = Conversion.ToInt((object)GrdHistory.CurrentRow.Cells["Id"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (GrdHistory.CurrentColumn.Key == "Edit")
				{
					Reset();
					ReadById(CurrId);
					CmbSupplier.Focus();
				}
				if (GrdHistory.CurrentColumn.Key == "Print")
				{
					CommissionAgentSlip(CurrId, "168-CommisionTradeBillAndSlip.rpt");
				}
				if (GrdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(CurrId, 162);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				Reset();
				ReadById(CurrId);
				CmbSupplier.Focus();
			}
			if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
			{
				CommissionAgentSlip(CurrId, "168-CommisionTradeBillAndSlip.rpt");
			}
			((Control)(object)GrdHistory).Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSalePaymentTerm_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)CmbSalePaymentTerm).Text == "Cash")
			{
				txtSaleDueDays.Enabled = false;
				txtSaleDueDays.Text = "1";
			}
			else
			{
				txtSaleDueDays.Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtSaleDueDays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (txtSaleDueDays.Text.Trim() != string.Empty && txtSaleDueDays.Text != null)
			{
				datSaleDueDate.Value = txtDocDate.Value.AddDays(Conversion.ToDouble((object)txtSaleDueDays.Text));
			}
			else
			{
				datSaleDueDate.Value = txtDocDate.Value;
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
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Expected O, but got Unknown
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Expected O, but got Unknown
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0512: Expected O, but got Unknown
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Expected O, but got Unknown
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Expected O, but got Unknown
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_0527: Expected O, but got Unknown
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_052e: Expected O, but got Unknown
		//IL_052e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Expected O, but got Unknown
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Expected O, but got Unknown
		//IL_053c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Expected O, but got Unknown
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Expected O, but got Unknown
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Expected O, but got Unknown
		//IL_0551: Unknown result type (might be due to invalid IL or missing references)
		//IL_0558: Expected O, but got Unknown
		//IL_0558: Unknown result type (might be due to invalid IL or missing references)
		//IL_055f: Expected O, but got Unknown
		//IL_055f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Expected O, but got Unknown
		//IL_0566: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Expected O, but got Unknown
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Expected O, but got Unknown
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Expected O, but got Unknown
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Expected O, but got Unknown
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Expected O, but got Unknown
		//IL_0589: Unknown result type (might be due to invalid IL or missing references)
		//IL_0590: Expected O, but got Unknown
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Expected O, but got Unknown
		//IL_0597: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Expected O, but got Unknown
		//IL_059e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Expected O, but got Unknown
		//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ac: Expected O, but got Unknown
		//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b3: Expected O, but got Unknown
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Expected O, but got Unknown
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c1: Expected O, but got Unknown
		//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c8: Expected O, but got Unknown
		//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cf: Expected O, but got Unknown
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d6: Expected O, but got Unknown
		//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dd: Expected O, but got Unknown
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e4: Expected O, but got Unknown
		//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Expected O, but got Unknown
		//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Expected O, but got Unknown
		//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f9: Expected O, but got Unknown
		//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0600: Expected O, but got Unknown
		//IL_0600: Unknown result type (might be due to invalid IL or missing references)
		//IL_0607: Expected O, but got Unknown
		//IL_0607: Unknown result type (might be due to invalid IL or missing references)
		//IL_060e: Expected O, but got Unknown
		//IL_060e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0615: Expected O, but got Unknown
		//IL_0615: Unknown result type (might be due to invalid IL or missing references)
		//IL_061c: Expected O, but got Unknown
		//IL_061c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0623: Expected O, but got Unknown
		//IL_0623: Unknown result type (might be due to invalid IL or missing references)
		//IL_062a: Expected O, but got Unknown
		//IL_062a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0631: Expected O, but got Unknown
		//IL_0631: Unknown result type (might be due to invalid IL or missing references)
		//IL_0638: Expected O, but got Unknown
		//IL_0638: Unknown result type (might be due to invalid IL or missing references)
		//IL_063f: Expected O, but got Unknown
		//IL_063f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0646: Expected O, but got Unknown
		//IL_0646: Unknown result type (might be due to invalid IL or missing references)
		//IL_064d: Expected O, but got Unknown
		//IL_064d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0654: Expected O, but got Unknown
		//IL_0654: Unknown result type (might be due to invalid IL or missing references)
		//IL_065b: Expected O, but got Unknown
		//IL_065b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0662: Expected O, but got Unknown
		//IL_0662: Unknown result type (might be due to invalid IL or missing references)
		//IL_0669: Expected O, but got Unknown
		//IL_0669: Unknown result type (might be due to invalid IL or missing references)
		//IL_0670: Expected O, but got Unknown
		//IL_0670: Unknown result type (might be due to invalid IL or missing references)
		//IL_0677: Expected O, but got Unknown
		//IL_0677: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Expected O, but got Unknown
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0685: Expected O, but got Unknown
		//IL_0685: Unknown result type (might be due to invalid IL or missing references)
		//IL_068c: Expected O, but got Unknown
		//IL_068c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0693: Expected O, but got Unknown
		//IL_0693: Unknown result type (might be due to invalid IL or missing references)
		//IL_069a: Expected O, but got Unknown
		//IL_069a: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a1: Expected O, but got Unknown
		//IL_06a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a8: Expected O, but got Unknown
		//IL_06a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06af: Expected O, but got Unknown
		//IL_06c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c7: Expected O, but got Unknown
		//IL_06c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ce: Expected O, but got Unknown
		//IL_06ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d5: Expected O, but got Unknown
		//IL_06d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dc: Expected O, but got Unknown
		//IL_06dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e3: Expected O, but got Unknown
		//IL_06e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ea: Expected O, but got Unknown
		//IL_06ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f1: Expected O, but got Unknown
		//IL_06f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f8: Expected O, but got Unknown
		//IL_06f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ff: Expected O, but got Unknown
		//IL_06ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0708: Expected O, but got Unknown
		//IL_070a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0713: Expected O, but got Unknown
		//IL_0715: Unknown result type (might be due to invalid IL or missing references)
		//IL_071e: Expected O, but got Unknown
		//IL_0720: Unknown result type (might be due to invalid IL or missing references)
		//IL_0729: Expected O, but got Unknown
		//IL_072b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0734: Expected O, but got Unknown
		//IL_0736: Unknown result type (might be due to invalid IL or missing references)
		//IL_073f: Expected O, but got Unknown
		//IL_0741: Unknown result type (might be due to invalid IL or missing references)
		//IL_074a: Expected O, but got Unknown
		//IL_074c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0755: Expected O, but got Unknown
		//IL_0757: Unknown result type (might be due to invalid IL or missing references)
		//IL_0760: Expected O, but got Unknown
		//IL_0762: Unknown result type (might be due to invalid IL or missing references)
		//IL_076b: Expected O, but got Unknown
		//IL_076d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0776: Expected O, but got Unknown
		//IL_0778: Unknown result type (might be due to invalid IL or missing references)
		//IL_0781: Expected O, but got Unknown
		//IL_0783: Unknown result type (might be due to invalid IL or missing references)
		//IL_078c: Expected O, but got Unknown
		//IL_078e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0797: Expected O, but got Unknown
		//IL_0799: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a2: Expected O, but got Unknown
		//IL_07a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ad: Expected O, but got Unknown
		//IL_07af: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b8: Expected O, but got Unknown
		//IL_07ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c3: Expected O, but got Unknown
		//IL_07c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ce: Expected O, but got Unknown
		//IL_07d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d9: Expected O, but got Unknown
		//IL_07db: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e4: Expected O, but got Unknown
		//IL_07e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ef: Expected O, but got Unknown
		//IL_07f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fa: Expected O, but got Unknown
		//IL_07fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0805: Expected O, but got Unknown
		//IL_0807: Unknown result type (might be due to invalid IL or missing references)
		//IL_0810: Expected O, but got Unknown
		//IL_0812: Unknown result type (might be due to invalid IL or missing references)
		//IL_081b: Expected O, but got Unknown
		//IL_081d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0826: Expected O, but got Unknown
		//IL_0828: Unknown result type (might be due to invalid IL or missing references)
		//IL_0831: Expected O, but got Unknown
		//IL_0833: Unknown result type (might be due to invalid IL or missing references)
		//IL_083c: Expected O, but got Unknown
		//IL_083e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0847: Expected O, but got Unknown
		//IL_0849: Unknown result type (might be due to invalid IL or missing references)
		//IL_0852: Expected O, but got Unknown
		//IL_0854: Unknown result type (might be due to invalid IL or missing references)
		//IL_085d: Expected O, but got Unknown
		//IL_085f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0868: Expected O, but got Unknown
		//IL_086a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0873: Expected O, but got Unknown
		//IL_0875: Unknown result type (might be due to invalid IL or missing references)
		//IL_087e: Expected O, but got Unknown
		//IL_0880: Unknown result type (might be due to invalid IL or missing references)
		//IL_0889: Expected O, but got Unknown
		//IL_088b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0894: Expected O, but got Unknown
		//IL_0896: Unknown result type (might be due to invalid IL or missing references)
		//IL_089f: Expected O, but got Unknown
		//IL_08a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08aa: Expected O, but got Unknown
		//IL_08ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b5: Expected O, but got Unknown
		//IL_08b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c0: Expected O, but got Unknown
		//IL_0905: Unknown result type (might be due to invalid IL or missing references)
		//IL_090f: Expected O, but got Unknown
		//IL_0931: Unknown result type (might be due to invalid IL or missing references)
		//IL_093b: Expected O, but got Unknown
		//IL_095d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0967: Expected O, but got Unknown
		//IL_097e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0988: Expected O, but got Unknown
		//IL_09aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b4: Expected O, but got Unknown
		//IL_09d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e0: Expected O, but got Unknown
		//IL_09e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09eb: Expected O, but got Unknown
		//IL_0a39: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a43: Expected O, but got Unknown
		//IL_0a44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4e: Expected O, but got Unknown
		//IL_0abd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac7: Expected O, but got Unknown
		//IL_0ac8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad2: Expected O, but got Unknown
		//IL_0b20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2a: Expected O, but got Unknown
		//IL_0b2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b35: Expected O, but got Unknown
		//IL_0b8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b98: Expected O, but got Unknown
		//IL_0ba4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bae: Expected O, but got Unknown
		//IL_0c33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3d: Expected O, but got Unknown
		//IL_0cd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce2: Expected O, but got Unknown
		//IL_0ce3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ced: Expected O, but got Unknown
		//IL_0d88: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d92: Expected O, but got Unknown
		//IL_0db4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dbe: Expected O, but got Unknown
		//IL_0de0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dea: Expected O, but got Unknown
		//IL_0e17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e21: Expected O, but got Unknown
		//IL_0e2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e37: Expected O, but got Unknown
		//IL_0e38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e42: Expected O, but got Unknown
		//IL_0e9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea5: Expected O, but got Unknown
		//IL_0fa3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fad: Expected O, but got Unknown
		//IL_0fb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc3: Expected O, but got Unknown
		//IL_0ff0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ffa: Expected O, but got Unknown
		//IL_1006: Unknown result type (might be due to invalid IL or missing references)
		//IL_1010: Expected O, but got Unknown
		//IL_1048: Unknown result type (might be due to invalid IL or missing references)
		//IL_1052: Expected O, but got Unknown
		//IL_108a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1094: Expected O, but got Unknown
		//IL_1827: Unknown result type (might be due to invalid IL or missing references)
		//IL_1831: Expected O, but got Unknown
		//IL_1b91: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b9b: Expected O, but got Unknown
		//IL_1e37: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e41: Expected O, but got Unknown
		//IL_1e4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e58: Expected O, but got Unknown
		//IL_209f: Unknown result type (might be due to invalid IL or missing references)
		//IL_20a9: Expected O, but got Unknown
		//IL_2336: Unknown result type (might be due to invalid IL or missing references)
		//IL_2340: Expected O, but got Unknown
		//IL_234d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2357: Expected O, but got Unknown
		//IL_c1e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_c1eb: Expected O, but got Unknown
		//IL_c20f: Unknown result type (might be due to invalid IL or missing references)
		//IL_c219: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInvExpForPurchase_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdFreightPurchase_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInvExpForSale_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPackingMaterialSale_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance234 = new Appearance();
		Appearance appearance245 = new Appearance();
		Appearance appearance256 = new Appearance();
		Appearance appearance267 = new Appearance();
		Appearance appearance278 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
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
		Appearance appearance158 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance273 = new Appearance();
		Appearance appearance274 = new Appearance();
		Appearance appearance275 = new Appearance();
		Appearance appearance276 = new Appearance();
		Appearance appearance277 = new Appearance();
		Appearance appearance279 = new Appearance();
		Appearance appearance280 = new Appearance();
		Appearance appearance281 = new Appearance();
		Appearance appearance282 = new Appearance();
		Appearance appearance283 = new Appearance();
		Appearance appearance284 = new Appearance();
		Appearance appearance285 = new Appearance();
		Appearance appearance287 = new Appearance();
		Appearance appearance288 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance260 = new Appearance();
		Appearance appearance261 = new Appearance();
		Appearance appearance262 = new Appearance();
		Appearance appearance263 = new Appearance();
		Appearance appearance264 = new Appearance();
		Appearance appearance265 = new Appearance();
		Appearance appearance266 = new Appearance();
		Appearance appearance268 = new Appearance();
		Appearance appearance269 = new Appearance();
		Appearance appearance270 = new Appearance();
		Appearance appearance271 = new Appearance();
		Appearance appearance272 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance233 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance238 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance241 = new Appearance();
		Appearance appearance242 = new Appearance();
		Appearance appearance243 = new Appearance();
		Appearance appearance244 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance247 = new Appearance();
		Appearance appearance248 = new Appearance();
		Appearance appearance249 = new Appearance();
		Appearance appearance250 = new Appearance();
		Appearance appearance251 = new Appearance();
		Appearance appearance252 = new Appearance();
		Appearance appearance253 = new Appearance();
		Appearance appearance254 = new Appearance();
		Appearance appearance255 = new Appearance();
		Appearance appearance257 = new Appearance();
		Appearance appearance258 = new Appearance();
		Appearance appearance259 = new Appearance();
		Appearance appearance286 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance219 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.CommissionAgent.CommissionAgentTradeBill_162));
		GridEXLayout GrdHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdDetailHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance157 = new Appearance();
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
		Appearance appearance78 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance91 = new Appearance();
		this.MainPanel = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.label34 = new System.Windows.Forms.Label();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.grdInvExpForPurchase = new GridEX();
		this.label53 = new System.Windows.Forms.Label();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.panel13 = new System.Windows.Forms.Panel();
		this.grdFreightPurchase = new GridEX();
		this.label43 = new System.Windows.Forms.Label();
		this.tabPage5 = new System.Windows.Forms.TabPage();
		this.grdInvExpForSale = new GridEX();
		this.label54 = new System.Windows.Forms.Label();
		this.tabPage6 = new System.Windows.Forms.TabPage();
		this.panel14 = new System.Windows.Forms.Panel();
		this.grdPackingMaterialSale = new GridEX();
		this.label44 = new System.Windows.Forms.Label();
		this.tabPage7 = new System.Windows.Forms.TabPage();
		this.groupBox6 = new System.Windows.Forms.GroupBox();
		this.CmbCommDrAcCust = new UltraCombo();
		this.CmbCommAgentCust = new UltraCombo();
		this.txtCommPercentCust = new System.Windows.Forms.TextBox();
		this.label63 = new System.Windows.Forms.Label();
		this.txtCommAmountCust = new System.Windows.Forms.TextBox();
		this.label64 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.CmbBrokeryDrAcCust = new UltraCombo();
		this.CmbBrokerCust = new UltraCombo();
		this.txtBrokeryPercentCust = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.txtBrokeryAmountCust = new System.Windows.Forms.TextBox();
		this.label65 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.txtBrokeryAmountSupp = new System.Windows.Forms.TextBox();
		this.txtBrokeryPercentSupp = new System.Windows.Forms.TextBox();
		this.label57 = new System.Windows.Forms.Label();
		this.CmbBrokeryDrAcSupp = new UltraCombo();
		this.CmbBrokerSupp = new UltraCombo();
		this.label56 = new System.Windows.Forms.Label();
		this.label59 = new System.Windows.Forms.Label();
		this.label58 = new System.Windows.Forms.Label();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.txtCommAmountSupp = new System.Windows.Forms.TextBox();
		this.txtCommPercentSupp = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.CmbCommDrAcSupp = new UltraCombo();
		this.CmbCommAgentSupp = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.label60 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.rdSearchByCode = new System.Windows.Forms.RadioButton();
		this.rdSearchByName = new System.Windows.Forms.RadioButton();
		this.btnAddDetail = new System.Windows.Forms.Button();
		this.CmbCropYear = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.CmbItemName = new UltraCombo();
		this.label19 = new System.Windows.Forms.Label();
		this.txtPurGrossWeight = new System.Windows.Forms.TextBox();
		this.txtRefDocNo = new System.Windows.Forms.TextBox();
		this.txtEBTotalPurchase = new System.Windows.Forms.TextBox();
		this.txtSaleAmount = new System.Windows.Forms.TextBox();
		this.label39 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.txtPurchaseAmount = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.CmbPackType = new UltraCombo();
		this.txtRateSale = new System.Windows.Forms.TextBox();
		this.txtRatePurchase = new System.Windows.Forms.TextBox();
		this.label36 = new System.Windows.Forms.Label();
		this.txtVehicleNo = new System.Windows.Forms.TextBox();
		this.txtEBTotalSale = new System.Windows.Forms.TextBox();
		this.label23 = new System.Windows.Forms.Label();
		this.txtDetailRemarks = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.txtAddlessWtSales = new System.Windows.Forms.TextBox();
		this.txtEBPerUnitSale = new System.Windows.Forms.TextBox();
		this.txtQuantity = new System.Windows.Forms.TextBox();
		this.CmbPackUom = new UltraCombo();
		this.CmbRateUom = new UltraCombo();
		this.label22 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.btnCancelDetail = new System.Windows.Forms.Button();
		this.label40 = new System.Windows.Forms.Label();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.label29 = new System.Windows.Forms.Label();
		this.txtBillWeightPurchase = new System.Windows.Forms.TextBox();
		this.txtBillWeightSale = new System.Windows.Forms.TextBox();
		this.txtEBPerUnitPurchase = new System.Windows.Forms.TextBox();
		this.label28 = new System.Windows.Forms.Label();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.groupBox8 = new System.Windows.Forms.GroupBox();
		this.CmbSalePaymentTerm = new UltraCombo();
		this.datSaleDueDate = new System.Windows.Forms.DateTimePicker();
		this.label67 = new System.Windows.Forms.Label();
		this.txtSaleDueDays = new System.Windows.Forms.TextBox();
		this.CmbCustomer = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.txtSaleBillAmount = new System.Windows.Forms.TextBox();
		this.label33 = new System.Windows.Forms.Label();
		this.CmbDeliveryTermSale = new UltraCombo();
		this.label55 = new System.Windows.Forms.Label();
		this.groupBox7 = new System.Windows.Forms.GroupBox();
		this.label16 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbSupplier = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.CmbDeliveryTerms = new UltraCombo();
		this.CmbPaymentTerms = new UltraCombo();
		this.txtDueDate = new System.Windows.Forms.DateTimePicker();
		this.txtPurchaseBillAmount = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtDueDays = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtMainRemarks = new System.Windows.Forms.TextBox();
		this.txtDocDate = new System.Windows.Forms.DateTimePicker();
		this.label15 = new System.Windows.Forms.Label();
		this.CmbTradingAccount = new UltraCombo();
		this.txtBranchSrNo = new System.Windows.Forms.TextBox();
		this.TxtDocNo = new System.Windows.Forms.TextBox();
		this.label38 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label18 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.txtProfitLoss = new System.Windows.Forms.TextBox();
		this.ToolstripPanel = new System.Windows.Forms.Panel();
		this.toolstrip = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.BtnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnPrint170 = new System.Windows.Forms.ToolStripButton();
		this.btnPrint171 = new System.Windows.Forms.ToolStripButton();
		this.btnprint172 = new System.Windows.Forms.ToolStripButton();
		this.BtnSlipNew1706 = new System.Windows.Forms.ToolStripButton();
		this.btnPrint103 = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.GrdHistory = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.GrdDetailHistory = new GridEX();
		this.label42 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label66 = new System.Windows.Forms.Label();
		this.panel20 = new System.Windows.Forms.Panel();
		this.cmbcusthistory = new UltraCombo();
		this.label51 = new System.Windows.Forms.Label();
		this.cmbsupplierhistory = new UltraCombo();
		this.label50 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.txtOrderNoTo = new System.Windows.Forms.TextBox();
		this.txtOrderNoFrom = new System.Windows.Forms.TextBox();
		this.label46 = new System.Windows.Forms.Label();
		this.cmbperemeter = new UltraCombo();
		this.label47 = new System.Windows.Forms.Label();
		this.btnshowHistory = new System.Windows.Forms.Button();
		this.ToDate = new System.Windows.Forms.DateTimePicker();
		this.fromdate = new System.Windows.Forms.DateTimePicker();
		this.label48 = new System.Windows.Forms.Label();
		this.cmbtradAcHistory = new UltraCombo();
		this.label49 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label35 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnGrnFormHistory = new System.Windows.Forms.ToolStripButton();
		this.txtSaleGrossWeight = new System.Windows.Forms.TextBox();
		this.label17 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.MainPanel.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInvExpForPurchase).BeginInit();
		this.tabPage4.SuspendLayout();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdFreightPurchase).BeginInit();
		this.tabPage5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInvExpForSale).BeginInit();
		this.tabPage6.SuspendLayout();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPackingMaterialSale).BeginInit();
		this.tabPage7.SuspendLayout();
		this.groupBox6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCommDrAcCust).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommAgentCust).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryDrAcCust).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokerCust).BeginInit();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryDrAcSupp).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokerSupp).BeginInit();
		this.groupBox5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCommDrAcSupp).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommAgentSupp).BeginInit();
		this.panel3.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).BeginInit();
		this.groupBox3.SuspendLayout();
		this.groupBox8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSalePaymentTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTermSale).BeginInit();
		this.groupBox7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerms).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTerms).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTradingAccount).BeginInit();
		this.panel2.SuspendLayout();
		this.ToolstripPanel.SuspendLayout();
		this.toolstrip.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistory).BeginInit();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdDetailHistory).BeginInit();
		this.panel9.SuspendLayout();
		this.panel20.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbcusthistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsupplierhistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbtradAcHistory).BeginInit();
		this.panel4.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.MainPanel.Controls.Add(this.tabControl1);
		this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.MainPanel.Location = new System.Drawing.Point(0, 0);
		this.MainPanel.Name = "MainPanel";
		this.MainPanel.Size = new System.Drawing.Size(1443, 661);
		this.MainPanel.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1443, 661);
		this.tabControl1.TabIndex = 4;
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1435, 632);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel1.AutoScroll = true;
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.tabControl2);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Controls.Add(this.ToolstripPanel);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1429, 626);
		this.panel1.TabIndex = 0;
		this.panel5.Controls.Add(this.panel8);
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Controls.Add(this.label34);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 469);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1429, 157);
		this.panel5.TabIndex = 541;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 23);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1429, 134);
		this.panel8.TabIndex = 541;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.DynamicFiltering = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1429, 134);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).DoubleClick += new System.EventHandler(grdDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDetail_KeyDown);
		this.label34.BackColor = System.Drawing.Color.Teal;
		this.label34.Dock = System.Windows.Forms.DockStyle.Top;
		this.label34.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label34.ForeColor = System.Drawing.Color.White;
		this.label34.Location = new System.Drawing.Point(0, 0);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(1429, 23);
		this.label34.TabIndex = 1;
		this.label34.Text = "Detail Grid";
		this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Controls.Add(this.tabPage5);
		this.tabControl2.Controls.Add(this.tabPage6);
		this.tabControl2.Controls.Add(this.tabPage7);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Top;
		this.tabControl2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl2.Location = new System.Drawing.Point(0, 304);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1429, 165);
		this.tabControl2.TabIndex = 540;
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grdInvExpForPurchase);
		this.tabPage3.Controls.Add(this.label53);
		this.tabPage3.Location = new System.Drawing.Point(4, 26);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1421, 135);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Purchase Expense";
		this.tabPage3.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdInvExpForPurchase).BackColor = System.Drawing.Color.White;
		this.grdInvExpForPurchase.ColumnAutoResize = true;
		grdInvExpForPurchase_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInvExpForPurchase.DesignTimeLayout = grdInvExpForPurchase_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInvExpForPurchase).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdInvExpForPurchase).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInvExpForPurchase.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInvExpForPurchase).Location = new System.Drawing.Point(3, 25);
		((System.Windows.Forms.Control)(object)this.grdInvExpForPurchase).Name = "grdInvExpForPurchase";
		((ScrollableBase)this.grdInvExpForPurchase).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInvExpForPurchase).Size = new System.Drawing.Size(1415, 107);
		((System.Windows.Forms.Control)(object)this.grdInvExpForPurchase).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdInvExpForPurchase).TabStop = false;
		this.grdInvExpForPurchase.TotalRow = (InheritableBoolean)1;
		this.grdInvExpForPurchase.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInvExpForPurchase).VisualStyle = (VisualStyle)4;
		this.grdInvExpForPurchase.CellUpdated += new ColumnActionEventHandler(grdInvExpForPurchase_CellUpdated);
		this.label53.BackColor = System.Drawing.Color.Teal;
		this.label53.Dock = System.Windows.Forms.DockStyle.Top;
		this.label53.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.ForeColor = System.Drawing.Color.White;
		this.label53.Location = new System.Drawing.Point(3, 3);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(1415, 22);
		this.label53.TabIndex = 3;
		this.label53.Text = "Credit To Supplier And Debit to Trading Account";
		this.label53.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tabPage4.Controls.Add(this.panel13);
		this.tabPage4.Controls.Add(this.label43);
		this.tabPage4.Location = new System.Drawing.Point(4, 26);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(1421, 135);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "Purchase Freight Expense";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.grdFreightPurchase);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(3, 25);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1415, 107);
		this.panel13.TabIndex = 3;
		this.grdFreightPurchase.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdFreightPurchase).BackColor = System.Drawing.Color.White;
		grdFreightPurchase_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdFreightPurchase.DesignTimeLayout = grdFreightPurchase_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdFreightPurchase).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdFreightPurchase).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdFreightPurchase.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdFreightPurchase).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdFreightPurchase).Name = "grdFreightPurchase";
		((ScrollableBase)this.grdFreightPurchase).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdFreightPurchase).Size = new System.Drawing.Size(1415, 107);
		((System.Windows.Forms.Control)(object)this.grdFreightPurchase).TabIndex = 1;
		this.grdFreightPurchase.TotalRow = (InheritableBoolean)1;
		this.grdFreightPurchase.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdFreightPurchase).VisualStyle = (VisualStyle)4;
		this.grdFreightPurchase.CellUpdated += new ColumnActionEventHandler(grdFreight_CellUpdated);
		this.grdFreightPurchase.ColumnButtonClick += new ColumnActionEventHandler(grdFreight_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdFreightPurchase).KeyDown += new System.Windows.Forms.KeyEventHandler(grdFreight_KeyDown);
		this.label43.BackColor = System.Drawing.Color.Teal;
		this.label43.Dock = System.Windows.Forms.DockStyle.Top;
		this.label43.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.ForeColor = System.Drawing.Color.White;
		this.label43.Location = new System.Drawing.Point(3, 3);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(1415, 22);
		this.label43.TabIndex = 2;
		this.label43.Text = "Selected Account Credit , Debit Account On Base of Delivery term(Load=> Debit To Trading Account  && Ponch=> Debit To Supplier Account)";
		this.label43.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tabPage5.Controls.Add((System.Windows.Forms.Control)(object)this.grdInvExpForSale);
		this.tabPage5.Controls.Add(this.label54);
		this.tabPage5.Location = new System.Drawing.Point(4, 26);
		this.tabPage5.Name = "tabPage5";
		this.tabPage5.Size = new System.Drawing.Size(1421, 135);
		this.tabPage5.TabIndex = 2;
		this.tabPage5.Text = "Sale Expense";
		this.tabPage5.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdInvExpForSale).BackColor = System.Drawing.Color.White;
		this.grdInvExpForSale.ColumnAutoResize = true;
		grdInvExpForSale_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInvExpForSale.DesignTimeLayout = grdInvExpForSale_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInvExpForSale).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdInvExpForSale).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInvExpForSale.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInvExpForSale).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grdInvExpForSale).Name = "grdInvExpForSale";
		((ScrollableBase)this.grdInvExpForSale).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInvExpForSale).Size = new System.Drawing.Size(1421, 113);
		((System.Windows.Forms.Control)(object)this.grdInvExpForSale).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.grdInvExpForSale).TabStop = false;
		this.grdInvExpForSale.TotalRow = (InheritableBoolean)1;
		this.grdInvExpForSale.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInvExpForSale).VisualStyle = (VisualStyle)4;
		this.grdInvExpForSale.CellUpdated += new ColumnActionEventHandler(grdInvExpForSale_CellUpdated);
		this.label54.BackColor = System.Drawing.Color.Teal;
		this.label54.Dock = System.Windows.Forms.DockStyle.Top;
		this.label54.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.ForeColor = System.Drawing.Color.White;
		this.label54.Location = new System.Drawing.Point(0, 0);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(1421, 22);
		this.label54.TabIndex = 3;
		this.label54.Text = "Debit To Customer And Credit to Trading Account";
		this.label54.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tabPage6.Controls.Add(this.panel14);
		this.tabPage6.Controls.Add(this.label44);
		this.tabPage6.Location = new System.Drawing.Point(4, 26);
		this.tabPage6.Name = "tabPage6";
		this.tabPage6.Size = new System.Drawing.Size(1421, 135);
		this.tabPage6.TabIndex = 3;
		this.tabPage6.Text = "Packing Material Sale Expense";
		this.tabPage6.UseVisualStyleBackColor = true;
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.grdPackingMaterialSale);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel14.Location = new System.Drawing.Point(0, 22);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1421, 113);
		this.panel14.TabIndex = 4;
		((ScrollableBase)this.grdPackingMaterialSale).BackColor = System.Drawing.Color.White;
		grdPackingMaterialSale_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPackingMaterialSale.DesignTimeLayout = grdPackingMaterialSale_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialSale).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialSale).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPackingMaterialSale.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialSale).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialSale).Name = "grdPackingMaterialSale";
		((ScrollableBase)this.grdPackingMaterialSale).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialSale).Size = new System.Drawing.Size(1421, 113);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialSale).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialSale).TabStop = false;
		this.grdPackingMaterialSale.TotalRow = (InheritableBoolean)1;
		this.grdPackingMaterialSale.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPackingMaterialSale).VisualStyle = (VisualStyle)4;
		this.grdPackingMaterialSale.CellUpdated += new ColumnActionEventHandler(grdpackingMaterial_CellUpdated);
		this.grdPackingMaterialSale.ColumnButtonClick += new ColumnActionEventHandler(grdpackingMaterial_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialSale).KeyDown += new System.Windows.Forms.KeyEventHandler(grdpackingMaterial_KeyDown);
		this.label44.BackColor = System.Drawing.Color.Teal;
		this.label44.Dock = System.Windows.Forms.DockStyle.Top;
		this.label44.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.ForeColor = System.Drawing.Color.White;
		this.label44.Location = new System.Drawing.Point(0, 0);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(1421, 22);
		this.label44.TabIndex = 3;
		this.label44.Text = "Selected Account Credit , Debit Account On Base of Delivery term(Load=> Debit To Customer  && Ponch=> Debit To Trading Account)";
		this.label44.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tabPage7.Controls.Add(this.groupBox6);
		this.tabPage7.Controls.Add(this.groupBox1);
		this.tabPage7.Controls.Add(this.groupBox2);
		this.tabPage7.Controls.Add(this.groupBox5);
		this.tabPage7.Location = new System.Drawing.Point(4, 26);
		this.tabPage7.Name = "tabPage7";
		this.tabPage7.Size = new System.Drawing.Size(1421, 135);
		this.tabPage7.TabIndex = 4;
		this.tabPage7.Text = "Commission & Brokery";
		this.tabPage7.UseVisualStyleBackColor = true;
		this.groupBox6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommDrAcCust);
		this.groupBox6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommAgentCust);
		this.groupBox6.Controls.Add(this.txtCommPercentCust);
		this.groupBox6.Controls.Add(this.label63);
		this.groupBox6.Controls.Add(this.txtCommAmountCust);
		this.groupBox6.Controls.Add(this.label64);
		this.groupBox6.Controls.Add(this.label61);
		this.groupBox6.Controls.Add(this.label62);
		this.groupBox6.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.groupBox6.Location = new System.Drawing.Point(583, 8);
		this.groupBox6.Name = "groupBox6";
		this.groupBox6.Size = new System.Drawing.Size(280, 124);
		this.groupBox6.TabIndex = 2;
		this.groupBox6.TabStop = false;
		this.groupBox6.Text = "Sale Commission";
		this.CmbCommDrAcCust.AllowNull = (DefaultableBoolean)1;
		this.CmbCommDrAcCust.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommDrAcCust.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommDrAcCust.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommDrAcCust).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbCommDrAcCust).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommDrAcCust).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommDrAcCust).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance212).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommDrAcCust).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance212;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommDrAcCust).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance223).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance223).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance223).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommDrAcCust).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance223;
		((UltraGridBase)this.CmbCommDrAcCust).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommDrAcCust).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance234).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommDrAcCust).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance234;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance245).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommDrAcCust).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance245;
		((UltraGridBase)this.CmbCommDrAcCust).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommDrAcCust).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance256).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommDrAcCust).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance256;
		((AppearanceBase)appearance267).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance267).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommDrAcCust).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance267;
		((UltraGridBase)this.CmbCommDrAcCust).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommDrAcCust).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance278).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance278).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance278).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance278).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance278).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommDrAcCust).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance278;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommDrAcCust).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbCommDrAcCust).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommDrAcCust).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommDrAcCust).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbCommDrAcCust).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommDrAcCust).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbCommDrAcCust).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommDrAcCust).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommDrAcCust).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommDrAcCust).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCommDrAcCust.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommDrAcCust).Location = new System.Drawing.Point(67, 76);
		((System.Windows.Forms.Control)(object)this.CmbCommDrAcCust).Name = "CmbCommDrAcCust";
		((System.Windows.Forms.Control)(object)this.CmbCommDrAcCust).Size = new System.Drawing.Size(210, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommDrAcCust).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbCommDrAcCust).Leave += new System.EventHandler(CmbCommDrAcCust_TextChanged);
		this.CmbCommAgentCust.AllowNull = (DefaultableBoolean)1;
		this.CmbCommAgentCust.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommAgentCust.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommAgentCust.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommAgentCust).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbCommAgentCust).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommAgentCust).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommAgentCust).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommAgentCust).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance148;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommAgentCust).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommAgentCust).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbCommAgentCust).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommAgentCust).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommAgentCust).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommAgentCust).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbCommAgentCust).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommAgentCust).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommAgentCust).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance153).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommAgentCust).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbCommAgentCust).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommAgentCust).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance154).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommAgentCust).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommAgentCust).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbCommAgentCust).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommAgentCust).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommAgentCust).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbCommAgentCust).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommAgentCust).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.CmbCommAgentCust).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommAgentCust).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommAgentCust).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommAgentCust).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCommAgentCust.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommAgentCust).Location = new System.Drawing.Point(67, 17);
		((System.Windows.Forms.Control)(object)this.CmbCommAgentCust).Name = "CmbCommAgentCust";
		((System.Windows.Forms.Control)(object)this.CmbCommAgentCust).Size = new System.Drawing.Size(210, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommAgentCust).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbCommAgentCust).Leave += new System.EventHandler(CmbCommAgentCust_TextChanged);
		this.txtCommPercentCust.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommPercentCust.Location = new System.Drawing.Point(67, 48);
		this.txtCommPercentCust.Name = "txtCommPercentCust";
		this.txtCommPercentCust.Size = new System.Drawing.Size(52, 23);
		this.txtCommPercentCust.TabIndex = 1;
		this.txtCommPercentCust.TextChanged += new System.EventHandler(txtBrokeryPercent_TextChanged);
		this.txtCommPercentCust.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCommPercentCust_KeyPress);
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label63.Location = new System.Drawing.Point(121, 52);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(51, 15);
		this.label63.TabIndex = 495;
		this.label63.Text = "Amount";
		this.txtCommAmountCust.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
		this.txtCommAmountCust.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommAmountCust.Location = new System.Drawing.Point(172, 48);
		this.txtCommAmountCust.Name = "txtCommAmountCust";
		this.txtCommAmountCust.ReadOnly = true;
		this.txtCommAmountCust.Size = new System.Drawing.Size(105, 23);
		this.txtCommAmountCust.TabIndex = 2;
		this.txtCommAmountCust.TabStop = false;
		this.txtCommAmountCust.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label64.Location = new System.Drawing.Point(4, 82);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(42, 15);
		this.label64.TabIndex = 467;
		this.label64.Text = "Dr A/c";
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label61.Location = new System.Drawing.Point(4, 23);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(65, 15);
		this.label61.TabIndex = 467;
		this.label61.Text = "Comm A/c";
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label62.Location = new System.Drawing.Point(4, 52);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(56, 15);
		this.label62.TabIndex = 470;
		this.label62.Text = "Comm %";
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrokeryDrAcCust);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrokerCust);
		this.groupBox1.Controls.Add(this.txtBrokeryPercentCust);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.txtBrokeryAmountCust);
		this.groupBox1.Controls.Add(this.label65);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.groupBox1.Location = new System.Drawing.Point(862, 8);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(286, 124);
		this.groupBox1.TabIndex = 3;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Sale Brokery";
		this.CmbBrokeryDrAcCust.AllowNull = (DefaultableBoolean)1;
		this.CmbBrokeryDrAcCust.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrokeryDrAcCust.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrokeryDrAcCust.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrokeryDrAcCust).DisplayLayout.Appearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbBrokeryDrAcCust).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrokeryDrAcCust).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryDrAcCust).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryDrAcCust).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance134;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryDrAcCust).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryDrAcCust).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbBrokeryDrAcCust).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrokeryDrAcCust).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrokeryDrAcCust).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrokeryDrAcCust).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbBrokeryDrAcCust).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrokeryDrAcCust).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryDrAcCust).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrokeryDrAcCust).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbBrokeryDrAcCust).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrokeryDrAcCust).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryDrAcCust).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrokeryDrAcCust).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbBrokeryDrAcCust).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrokeryDrAcCust).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrokeryDrAcCust).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbBrokeryDrAcCust).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrokeryDrAcCust).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbBrokeryDrAcCust).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrokeryDrAcCust).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrokeryDrAcCust).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryDrAcCust).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrokeryDrAcCust.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryDrAcCust).Location = new System.Drawing.Point(72, 76);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryDrAcCust).Name = "CmbBrokeryDrAcCust";
		((System.Windows.Forms.Control)(object)this.CmbBrokeryDrAcCust).Size = new System.Drawing.Size(210, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryDrAcCust).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryDrAcCust).Leave += new System.EventHandler(CmbBrokeryDrAcCust_TextChanged);
		this.CmbBrokerCust.AllowNull = (DefaultableBoolean)1;
		this.CmbBrokerCust.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrokerCust.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrokerCust.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrokerCust).DisplayLayout.Appearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbBrokerCust).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrokerCust).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokerCust).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokerCust).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance161;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokerCust).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokerCust).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbBrokerCust).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrokerCust).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrokerCust).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrokerCust).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbBrokerCust).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrokerCust).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokerCust).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance166).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrokerCust).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbBrokerCust).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrokerCust).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokerCust).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance169).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrokerCust).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbBrokerCust).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrokerCust).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrokerCust).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.CmbBrokerCust).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrokerCust).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.CmbBrokerCust).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrokerCust).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrokerCust).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrokerCust).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrokerCust.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrokerCust).Location = new System.Drawing.Point(72, 17);
		((System.Windows.Forms.Control)(object)this.CmbBrokerCust).Name = "CmbBrokerCust";
		((System.Windows.Forms.Control)(object)this.CmbBrokerCust).Size = new System.Drawing.Size(210, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrokerCust).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbBrokerCust).Leave += new System.EventHandler(CmbBrokerCust_TextChanged);
		this.txtBrokeryPercentCust.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBrokeryPercentCust.Location = new System.Drawing.Point(72, 48);
		this.txtBrokeryPercentCust.Name = "txtBrokeryPercentCust";
		this.txtBrokeryPercentCust.Size = new System.Drawing.Size(52, 23);
		this.txtBrokeryPercentCust.TabIndex = 1;
		this.txtBrokeryPercentCust.TextChanged += new System.EventHandler(txtBrokeryPercent_TextChanged);
		this.txtBrokeryPercentCust.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBrokeryPercent_KeyPress);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(126, 52);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(51, 15);
		this.label14.TabIndex = 495;
		this.label14.Text = "Amount";
		this.txtBrokeryAmountCust.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
		this.txtBrokeryAmountCust.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBrokeryAmountCust.Location = new System.Drawing.Point(177, 48);
		this.txtBrokeryAmountCust.Name = "txtBrokeryAmountCust";
		this.txtBrokeryAmountCust.ReadOnly = true;
		this.txtBrokeryAmountCust.Size = new System.Drawing.Size(105, 23);
		this.txtBrokeryAmountCust.TabIndex = 2;
		this.txtBrokeryAmountCust.TabStop = false;
		this.txtBrokeryAmountCust.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label65.Location = new System.Drawing.Point(4, 82);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(42, 15);
		this.label65.TabIndex = 467;
		this.label65.Text = "Dr A/c";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(4, 23);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(69, 15);
		this.label4.TabIndex = 467;
		this.label4.Text = "Brokery A/c";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(4, 52);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(60, 15);
		this.label7.TabIndex = 470;
		this.label7.Text = "Brokery %";
		this.groupBox2.Controls.Add(this.txtBrokeryAmountSupp);
		this.groupBox2.Controls.Add(this.txtBrokeryPercentSupp);
		this.groupBox2.Controls.Add(this.label57);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrokeryDrAcSupp);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrokerSupp);
		this.groupBox2.Controls.Add(this.label56);
		this.groupBox2.Controls.Add(this.label59);
		this.groupBox2.Controls.Add(this.label58);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.groupBox2.Location = new System.Drawing.Point(288, 8);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(298, 124);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Purchase Brokery";
		this.txtBrokeryAmountSupp.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
		this.txtBrokeryAmountSupp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBrokeryAmountSupp.Location = new System.Drawing.Point(176, 48);
		this.txtBrokeryAmountSupp.Name = "txtBrokeryAmountSupp";
		this.txtBrokeryAmountSupp.ReadOnly = true;
		this.txtBrokeryAmountSupp.Size = new System.Drawing.Size(117, 23);
		this.txtBrokeryAmountSupp.TabIndex = 2;
		this.txtBrokeryAmountSupp.TabStop = false;
		this.txtBrokeryAmountSupp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBrokeryPercentSupp.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
		this.txtBrokeryPercentSupp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBrokeryPercentSupp.Location = new System.Drawing.Point(72, 48);
		this.txtBrokeryPercentSupp.Name = "txtBrokeryPercentSupp";
		this.txtBrokeryPercentSupp.Size = new System.Drawing.Size(52, 23);
		this.txtBrokeryPercentSupp.TabIndex = 1;
		this.txtBrokeryPercentSupp.TextChanged += new System.EventHandler(txtCommPercent_TextChanged);
		this.txtBrokeryPercentSupp.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBrokeryPercentSupp_KeyPress);
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.Location = new System.Drawing.Point(123, 52);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(51, 15);
		this.label57.TabIndex = 493;
		this.label57.Text = "Amount";
		this.CmbBrokeryDrAcSupp.AllowNull = (DefaultableBoolean)1;
		this.CmbBrokeryDrAcSupp.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrokeryDrAcSupp.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrokeryDrAcSupp.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrokeryDrAcSupp).DisplayLayout.Appearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbBrokeryDrAcSupp).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrokeryDrAcSupp).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryDrAcSupp).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryDrAcSupp).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance21;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryDrAcSupp).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryDrAcSupp).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbBrokeryDrAcSupp).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrokeryDrAcSupp).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrokeryDrAcSupp).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrokeryDrAcSupp).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbBrokeryDrAcSupp).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrokeryDrAcSupp).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryDrAcSupp).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance27).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrokeryDrAcSupp).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbBrokeryDrAcSupp).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrokeryDrAcSupp).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryDrAcSupp).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrokeryDrAcSupp).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbBrokeryDrAcSupp).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrokeryDrAcSupp).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrokeryDrAcSupp).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbBrokeryDrAcSupp).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrokeryDrAcSupp).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbBrokeryDrAcSupp).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrokeryDrAcSupp).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrokeryDrAcSupp).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryDrAcSupp).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrokeryDrAcSupp.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryDrAcSupp).Location = new System.Drawing.Point(72, 76);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryDrAcSupp).Name = "CmbBrokeryDrAcSupp";
		((System.Windows.Forms.Control)(object)this.CmbBrokeryDrAcSupp).Size = new System.Drawing.Size(221, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryDrAcSupp).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryDrAcSupp).Leave += new System.EventHandler(CmbBrokeryDrAcSupp_TextChanged);
		this.CmbBrokerSupp.AllowNull = (DefaultableBoolean)1;
		this.CmbBrokerSupp.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrokerSupp.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrokerSupp.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrokerSupp).DisplayLayout.Appearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbBrokerSupp).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrokerSupp).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance173).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance173).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokerSupp).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokerSupp).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance174;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokerSupp).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokerSupp).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbBrokerSupp).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrokerSupp).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrokerSupp).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrokerSupp).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.CmbBrokerSupp).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrokerSupp).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokerSupp).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance180).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrokerSupp).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.CmbBrokerSupp).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrokerSupp).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance181).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance181).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance181).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokerSupp).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrokerSupp).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.CmbBrokerSupp).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrokerSupp).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrokerSupp).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.CmbBrokerSupp).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrokerSupp).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.CmbBrokerSupp).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrokerSupp).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrokerSupp).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrokerSupp).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrokerSupp.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrokerSupp).Location = new System.Drawing.Point(72, 17);
		((System.Windows.Forms.Control)(object)this.CmbBrokerSupp).Name = "CmbBrokerSupp";
		((System.Windows.Forms.Control)(object)this.CmbBrokerSupp).Size = new System.Drawing.Size(221, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrokerSupp).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbBrokerSupp).Leave += new System.EventHandler(CmbCommAgent_TextChanged);
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.Location = new System.Drawing.Point(5, 52);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(60, 15);
		this.label56.TabIndex = 468;
		this.label56.Text = "Brokery %";
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label59.Location = new System.Drawing.Point(5, 82);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(42, 15);
		this.label59.TabIndex = 0;
		this.label59.Text = "Dr A/c";
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.Location = new System.Drawing.Point(5, 23);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(69, 15);
		this.label58.TabIndex = 0;
		this.label58.Text = "Brokery A/c";
		this.groupBox5.Controls.Add(this.txtCommAmountSupp);
		this.groupBox5.Controls.Add(this.txtCommPercentSupp);
		this.groupBox5.Controls.Add(this.label13);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommDrAcSupp);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommAgentSupp);
		this.groupBox5.Controls.Add(this.label5);
		this.groupBox5.Controls.Add(this.label60);
		this.groupBox5.Controls.Add(this.label6);
		this.groupBox5.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.groupBox5.Location = new System.Drawing.Point(3, 8);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(286, 124);
		this.groupBox5.TabIndex = 0;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Purchase Commission";
		this.txtCommAmountSupp.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
		this.txtCommAmountSupp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommAmountSupp.Location = new System.Drawing.Point(166, 48);
		this.txtCommAmountSupp.Name = "txtCommAmountSupp";
		this.txtCommAmountSupp.ReadOnly = true;
		this.txtCommAmountSupp.Size = new System.Drawing.Size(117, 23);
		this.txtCommAmountSupp.TabIndex = 2;
		this.txtCommAmountSupp.TabStop = false;
		this.txtCommAmountSupp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCommPercentSupp.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
		this.txtCommPercentSupp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommPercentSupp.Location = new System.Drawing.Point(62, 48);
		this.txtCommPercentSupp.Name = "txtCommPercentSupp";
		this.txtCommPercentSupp.Size = new System.Drawing.Size(52, 23);
		this.txtCommPercentSupp.TabIndex = 1;
		this.txtCommPercentSupp.TextChanged += new System.EventHandler(txtCommPercent_TextChanged);
		this.txtCommPercentSupp.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCommPercent_KeyPress);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(113, 52);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(51, 15);
		this.label13.TabIndex = 493;
		this.label13.Text = "Amount";
		this.CmbCommDrAcSupp.AllowNull = (DefaultableBoolean)1;
		this.CmbCommDrAcSupp.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommDrAcSupp.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommDrAcSupp.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommDrAcSupp).DisplayLayout.Appearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbCommDrAcSupp).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommDrAcSupp).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommDrAcSupp).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommDrAcSupp).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance121;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommDrAcSupp).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommDrAcSupp).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbCommDrAcSupp).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommDrAcSupp).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommDrAcSupp).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommDrAcSupp).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbCommDrAcSupp).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommDrAcSupp).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommDrAcSupp).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance127).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommDrAcSupp).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbCommDrAcSupp).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommDrAcSupp).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommDrAcSupp).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommDrAcSupp).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbCommDrAcSupp).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommDrAcSupp).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommDrAcSupp).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbCommDrAcSupp).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommDrAcSupp).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbCommDrAcSupp).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommDrAcSupp).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommDrAcSupp).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommDrAcSupp).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCommDrAcSupp.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommDrAcSupp).Location = new System.Drawing.Point(62, 76);
		((System.Windows.Forms.Control)(object)this.CmbCommDrAcSupp).Name = "CmbCommDrAcSupp";
		((System.Windows.Forms.Control)(object)this.CmbCommDrAcSupp).Size = new System.Drawing.Size(221, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommDrAcSupp).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbCommDrAcSupp).Leave += new System.EventHandler(CmbCommDrAcSupp_TextChanged);
		this.CmbCommAgentSupp.AllowNull = (DefaultableBoolean)1;
		this.CmbCommAgentSupp.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommAgentSupp.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommAgentSupp.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommAgentSupp).DisplayLayout.Appearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.CmbCommAgentSupp).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommAgentSupp).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance186).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance186).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommAgentSupp).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommAgentSupp).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance187;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommAgentSupp).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance188).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance188).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommAgentSupp).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.CmbCommAgentSupp).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommAgentSupp).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommAgentSupp).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommAgentSupp).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.CmbCommAgentSupp).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommAgentSupp).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommAgentSupp).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance193).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommAgentSupp).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.CmbCommAgentSupp).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommAgentSupp).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance194).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance194).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance194).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommAgentSupp).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance195).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommAgentSupp).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.CmbCommAgentSupp).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommAgentSupp).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommAgentSupp).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.CmbCommAgentSupp).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommAgentSupp).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.CmbCommAgentSupp).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommAgentSupp).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommAgentSupp).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommAgentSupp).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCommAgentSupp.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommAgentSupp).Location = new System.Drawing.Point(62, 17);
		((System.Windows.Forms.Control)(object)this.CmbCommAgentSupp).Name = "CmbCommAgentSupp";
		((System.Windows.Forms.Control)(object)this.CmbCommAgentSupp).Size = new System.Drawing.Size(221, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommAgentSupp).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbCommAgentSupp).Leave += new System.EventHandler(CmbCommAgent_TextChanged);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(3, 52);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(56, 15);
		this.label5.TabIndex = 468;
		this.label5.Text = "Comm %";
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.Location = new System.Drawing.Point(3, 82);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(42, 15);
		this.label60.TabIndex = 0;
		this.label60.Text = "Dr A/c";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(3, 23);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(60, 15);
		this.label6.TabIndex = 0;
		this.label6.Text = "Comm Ac";
		this.panel3.Controls.Add(this.groupBox4);
		this.panel3.Controls.Add(this.groupBox3);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 52);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1429, 252);
		this.panel3.TabIndex = 0;
		this.groupBox4.Controls.Add(this.txtSaleGrossWeight);
		this.groupBox4.Controls.Add(this.label17);
		this.groupBox4.Controls.Add(this.rdSearchByCode);
		this.groupBox4.Controls.Add(this.rdSearchByName);
		this.groupBox4.Controls.Add(this.btnAddDetail);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYear);
		this.groupBox4.Controls.Add(this.label10);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.groupBox4.Controls.Add(this.label19);
		this.groupBox4.Controls.Add(this.txtPurGrossWeight);
		this.groupBox4.Controls.Add(this.txtRefDocNo);
		this.groupBox4.Controls.Add(this.txtEBTotalPurchase);
		this.groupBox4.Controls.Add(this.txtSaleAmount);
		this.groupBox4.Controls.Add(this.label39);
		this.groupBox4.Controls.Add(this.label41);
		this.groupBox4.Controls.Add(this.label20);
		this.groupBox4.Controls.Add(this.label25);
		this.groupBox4.Controls.Add(this.txtPurchaseAmount);
		this.groupBox4.Controls.Add(this.label11);
		this.groupBox4.Controls.Add(this.label26);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackType);
		this.groupBox4.Controls.Add(this.txtRateSale);
		this.groupBox4.Controls.Add(this.txtRatePurchase);
		this.groupBox4.Controls.Add(this.label36);
		this.groupBox4.Controls.Add(this.txtVehicleNo);
		this.groupBox4.Controls.Add(this.txtEBTotalSale);
		this.groupBox4.Controls.Add(this.label23);
		this.groupBox4.Controls.Add(this.txtDetailRemarks);
		this.groupBox4.Controls.Add(this.label24);
		this.groupBox4.Controls.Add(this.label31);
		this.groupBox4.Controls.Add(this.label21);
		this.groupBox4.Controls.Add(this.label37);
		this.groupBox4.Controls.Add(this.txtAddlessWtSales);
		this.groupBox4.Controls.Add(this.txtEBPerUnitSale);
		this.groupBox4.Controls.Add(this.txtQuantity);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUom);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRateUom);
		this.groupBox4.Controls.Add(this.label22);
		this.groupBox4.Controls.Add(this.label27);
		this.groupBox4.Controls.Add(this.label32);
		this.groupBox4.Controls.Add(this.label9);
		this.groupBox4.Controls.Add(this.btnCancelDetail);
		this.groupBox4.Controls.Add(this.label40);
		this.groupBox4.Controls.Add(this.btnUpdateDetail);
		this.groupBox4.Controls.Add(this.label29);
		this.groupBox4.Controls.Add(this.txtBillWeightPurchase);
		this.groupBox4.Controls.Add(this.txtBillWeightSale);
		this.groupBox4.Controls.Add(this.txtEBPerUnitPurchase);
		this.groupBox4.Controls.Add(this.label28);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(4, 138);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(1232, 112);
		this.groupBox4.TabIndex = 1;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Detail";
		this.rdSearchByCode.AutoSize = true;
		this.rdSearchByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByCode.Location = new System.Drawing.Point(98, 14);
		this.rdSearchByCode.Name = "rdSearchByCode";
		this.rdSearchByCode.Size = new System.Drawing.Size(52, 19);
		this.rdSearchByCode.TabIndex = 539;
		this.rdSearchByCode.TabStop = true;
		this.rdSearchByCode.Text = "Code";
		this.rdSearchByCode.UseVisualStyleBackColor = true;
		this.rdSearchByName.AutoSize = true;
		this.rdSearchByName.Checked = true;
		this.rdSearchByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByName.Location = new System.Drawing.Point(39, 14);
		this.rdSearchByName.Name = "rdSearchByName";
		this.rdSearchByName.Size = new System.Drawing.Size(57, 19);
		this.rdSearchByName.TabIndex = 538;
		this.rdSearchByName.TabStop = true;
		this.rdSearchByName.Text = "Name";
		this.rdSearchByName.UseVisualStyleBackColor = true;
		this.rdSearchByName.CheckedChanged += new System.EventHandler(rdSearchByName_CheckedChanged);
		this.btnAddDetail.BackColor = System.Drawing.Color.Teal;
		this.btnAddDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddDetail.ForeColor = System.Drawing.Color.White;
		this.btnAddDetail.Location = new System.Drawing.Point(1089, 82);
		this.btnAddDetail.Name = "btnAddDetail";
		this.btnAddDetail.Size = new System.Drawing.Size(33, 27);
		this.btnAddDetail.TabIndex = 21;
		this.btnAddDetail.Text = "+";
		this.btnAddDetail.UseVisualStyleBackColor = false;
		this.btnAddDetail.Click += new System.EventHandler(btnAddDetail_Click);
		this.CmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance273).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance273).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance273;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance274).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance274).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance274).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance274).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance274;
		((AppearanceBase)appearance275).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance275;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance276).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance276).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance276).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance276).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance276;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance277).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance277).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance277;
		((AppearanceBase)appearance279).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance279).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance279;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance280).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance280;
		((AppearanceBase)appearance281).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance281).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance281;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance282).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance282).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance282).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance282).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance282).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance282;
		((AppearanceBase)appearance283).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance283;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance284).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance284).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance284;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance285).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance285;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Location = new System.Drawing.Point(203, 34);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Name = "CmbCropYear";
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Size = new System.Drawing.Size(84, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).TabIndex = 1;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(6, 16);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(35, 15);
		this.label10.TabIndex = 2;
		this.label10.Text = "Item ";
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance287).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance287).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance287;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance288).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance288).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance288).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance288).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance288;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance3;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(6, 34);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(196, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Leave += new System.EventHandler(CmbItemName_Leave);
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(288, 16);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(60, 15);
		this.label19.TabIndex = 499;
		this.label19.Text = "Pack Type";
		this.txtPurGrossWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPurGrossWeight.Location = new System.Drawing.Point(529, 36);
		this.txtPurGrossWeight.Name = "txtPurGrossWeight";
		this.txtPurGrossWeight.Size = new System.Drawing.Size(96, 23);
		this.txtPurGrossWeight.TabIndex = 5;
		this.txtPurGrossWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPurGrossWeight.TextChanged += new System.EventHandler(txtGrossWeight_TextChanged);
		this.txtPurGrossWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtWeight_KeyPress);
		this.txtRefDocNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRefDocNo.Location = new System.Drawing.Point(589, 84);
		this.txtRefDocNo.Name = "txtRefDocNo";
		this.txtRefDocNo.Size = new System.Drawing.Size(66, 23);
		this.txtRefDocNo.TabIndex = 18;
		this.txtEBTotalPurchase.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEBTotalPurchase.Location = new System.Drawing.Point(690, 36);
		this.txtEBTotalPurchase.Name = "txtEBTotalPurchase";
		this.txtEBTotalPurchase.Size = new System.Drawing.Size(68, 23);
		this.txtEBTotalPurchase.TabIndex = 7;
		this.txtEBTotalPurchase.Tag = "PurchaseEbTotal";
		this.txtEBTotalPurchase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtEBTotalPurchase.TextChanged += new System.EventHandler(txtEBTotalPurchase_TextChanged);
		this.txtSaleAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSaleAmount.Location = new System.Drawing.Point(492, 84);
		this.txtSaleAmount.Name = "txtSaleAmount";
		this.txtSaleAmount.ReadOnly = true;
		this.txtSaleAmount.Size = new System.Drawing.Size(95, 23);
		this.txtSaleAmount.TabIndex = 17;
		this.txtSaleAmount.TabStop = false;
		this.txtSaleAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(1049, 16);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(74, 15);
		this.label39.TabIndex = 537;
		this.label39.Text = "Sale EB Total";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(6, 64);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(112, 15);
		this.label41.TabIndex = 535;
		this.label41.Text = "Net Sale Bill Weight";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(398, 16);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(61, 15);
		this.label20.TabIndex = 501;
		this.label20.Text = "Pack Uom";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(296, 64);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(102, 15);
		this.label25.TabIndex = 511;
		this.label25.Text = "Purchase Amount";
		this.txtPurchaseAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPurchaseAmount.Location = new System.Drawing.Point(296, 84);
		this.txtPurchaseAmount.Name = "txtPurchaseAmount";
		this.txtPurchaseAmount.ReadOnly = true;
		this.txtPurchaseAmount.Size = new System.Drawing.Size(100, 23);
		this.txtPurchaseAmount.TabIndex = 15;
		this.txtPurchaseAmount.TabStop = false;
		this.txtPurchaseAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(203, 16);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(57, 15);
		this.label11.TabIndex = 497;
		this.label11.Text = "Crop Year";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(495, 64);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(76, 15);
		this.label26.TabIndex = 513;
		this.label26.Text = "Sale Amount";
		this.CmbPackType.AllowNull = (DefaultableBoolean)1;
		this.CmbPackType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance260).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance260).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance260;
		((UltraGridBase)this.CmbPackType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance261).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance261).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance261).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance261).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance261;
		((AppearanceBase)appearance262).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance262;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance263).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance263).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance263).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance263).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance263;
		((UltraGridBase)this.CmbPackType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance264).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance264).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance264;
		((AppearanceBase)appearance265).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance265).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance265;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance266).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance266;
		((AppearanceBase)appearance268).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance268).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance268;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance269).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance269).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance269).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance269).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance269).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance269;
		((AppearanceBase)appearance270).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance270;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance271).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance271).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance271;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance272).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance272;
		((UltraGridBase)this.CmbPackType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackType).Location = new System.Drawing.Point(288, 34);
		((System.Windows.Forms.Control)(object)this.CmbPackType).Name = "CmbPackType";
		((System.Windows.Forms.Control)(object)this.CmbPackType).Size = new System.Drawing.Size(109, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackType).TabIndex = 2;
		this.txtRateSale.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRateSale.Location = new System.Drawing.Point(398, 84);
		this.txtRateSale.Name = "txtRateSale";
		this.txtRateSale.Size = new System.Drawing.Size(92, 23);
		this.txtRateSale.TabIndex = 16;
		this.txtRateSale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRateSale.TextChanged += new System.EventHandler(txtCustomerRate_TextChanged);
		this.txtRateSale.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCustomerRate_KeyPress);
		this.txtRatePurchase.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRatePurchase.Location = new System.Drawing.Point(134, 84);
		this.txtRatePurchase.Name = "txtRatePurchase";
		this.txtRatePurchase.Size = new System.Drawing.Size(88, 23);
		this.txtRatePurchase.TabIndex = 14;
		this.txtRatePurchase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRatePurchase.TextChanged += new System.EventHandler(txtSupplierRate_TextChanged);
		this.txtRatePurchase.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSupplierRate_KeyPress);
		this.txtRatePurchase.Leave += new System.EventHandler(txtRatePurchase_Leave);
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(690, 16);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(70, 15);
		this.label36.TabIndex = 526;
		this.label36.Text = "Pur EB Total";
		this.txtVehicleNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVehicleNo.Location = new System.Drawing.Point(657, 84);
		this.txtVehicleNo.Name = "txtVehicleNo";
		this.txtVehicleNo.Size = new System.Drawing.Size(89, 23);
		this.txtVehicleNo.TabIndex = 19;
		this.txtEBTotalSale.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEBTotalSale.Location = new System.Drawing.Point(1050, 36);
		this.txtEBTotalSale.Name = "txtEBTotalSale";
		this.txtEBTotalSale.Size = new System.Drawing.Size(75, 23);
		this.txtEBTotalSale.TabIndex = 11;
		this.txtEBTotalSale.Tag = "SaleEbTotal";
		this.txtEBTotalSale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtEBTotalSale.TextChanged += new System.EventHandler(txtEBTotalSale_TextChanged);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(134, 64);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(81, 15);
		this.label23.TabIndex = 507;
		this.label23.Text = "Rate Purchase";
		this.txtDetailRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDetailRemarks.Location = new System.Drawing.Point(748, 84);
		this.txtDetailRemarks.Name = "txtDetailRemarks";
		this.txtDetailRemarks.Size = new System.Drawing.Size(340, 23);
		this.txtDetailRemarks.TabIndex = 20;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(398, 64);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(55, 15);
		this.label24.TabIndex = 509;
		this.label24.Text = "Rate Sale";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(759, 16);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(85, 15);
		this.label31.TabIndex = 518;
		this.label31.Text = "Net Pur Bill Wt";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(461, 16);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(53, 15);
		this.label21.TabIndex = 503;
		this.label21.Text = "Quantity";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(626, 16);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(68, 15);
		this.label37.TabIndex = 525;
		this.label37.Text = "Pur EB/Unit";
		this.txtAddlessWtSales.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAddlessWtSales.Location = new System.Drawing.Point(1127, 36);
		this.txtAddlessWtSales.Name = "txtAddlessWtSales";
		this.txtAddlessWtSales.Size = new System.Drawing.Size(90, 23);
		this.txtAddlessWtSales.TabIndex = 12;
		this.txtAddlessWtSales.Tag = "";
		this.txtAddlessWtSales.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAddlessWtSales.TextChanged += new System.EventHandler(txtAddlessWtSales_TextChanged);
		this.txtAddlessWtSales.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAddlessWtSales_KeyPress);
		this.txtEBPerUnitSale.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEBPerUnitSale.Location = new System.Drawing.Point(977, 36);
		this.txtEBPerUnitSale.Name = "txtEBPerUnitSale";
		this.txtEBPerUnitSale.Size = new System.Drawing.Size(72, 23);
		this.txtEBPerUnitSale.TabIndex = 10;
		this.txtEBPerUnitSale.Tag = "SaleEBUnit";
		this.txtEBPerUnitSale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtEBPerUnitSale.TextChanged += new System.EventHandler(txtEBPerUnitSale_TextChanged);
		this.txtEBPerUnitSale.KeyPress += new System.Windows.Forms.KeyPressEventHandler(textBox2_KeyPress);
		this.txtQuantity.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQuantity.Location = new System.Drawing.Point(461, 36);
		this.txtQuantity.Name = "txtQuantity";
		this.txtQuantity.Size = new System.Drawing.Size(67, 23);
		this.txtQuantity.TabIndex = 4;
		this.txtQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQuantity.TextChanged += new System.EventHandler(txtQuantity_TextChanged);
		this.txtQuantity.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQuantity_KeyPress);
		this.CmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance61;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance66).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance67).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance168).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance211).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance213;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Location = new System.Drawing.Point(398, 34);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Name = "CmbPackUom";
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Size = new System.Drawing.Size(62, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Leave += new System.EventHandler(CmbPackUom_Leave);
		this.CmbRateUom.AllowNull = (DefaultableBoolean)1;
		this.CmbRateUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance220).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance221).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance221).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance221).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance221;
		((AppearanceBase)appearance222).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance222;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance224).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance224).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance224).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance225).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance225;
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance226).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance227;
		((AppearanceBase)appearance228).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance228).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance228;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance229).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance229).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance229).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance229).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance229;
		((AppearanceBase)appearance230).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance230;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance231).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance232;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRateUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Location = new System.Drawing.Point(223, 82);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Name = "CmbRateUom";
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Size = new System.Drawing.Size(72, 26);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).TabIndex = 15;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).TextChanged += new System.EventHandler(CmbRateUom_TextChanged);
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(529, 16);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(99, 15);
		this.label22.TabIndex = 505;
		this.label22.Text = "Pur Gross Weight";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(657, 64);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(64, 15);
		this.label27.TabIndex = 520;
		this.label27.Text = "Vehicle No";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(589, 66);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(68, 15);
		this.label32.TabIndex = 522;
		this.label32.Text = "Ref Doc No";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(1127, 16);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(75, 15);
		this.label9.TabIndex = 536;
		this.label9.Text = "Add/Less Wt";
		this.btnCancelDetail.BackColor = System.Drawing.Color.Teal;
		this.btnCancelDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelDetail.ForeColor = System.Drawing.Color.White;
		this.btnCancelDetail.Location = new System.Drawing.Point(1152, 82);
		this.btnCancelDetail.Name = "btnCancelDetail";
		this.btnCancelDetail.Size = new System.Drawing.Size(67, 27);
		this.btnCancelDetail.TabIndex = 23;
		this.btnCancelDetail.Text = "Cancel";
		this.btnCancelDetail.UseVisualStyleBackColor = false;
		this.btnCancelDetail.Visible = false;
		this.btnCancelDetail.Click += new System.EventHandler(btnCancelDetail_Click);
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(977, 16);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(72, 15);
		this.label40.TabIndex = 536;
		this.label40.Text = "Sale EB/Unit";
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(1089, 82);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(71, 27);
		this.btnUpdateDetail.TabIndex = 22;
		this.btnUpdateDetail.Text = "Update ";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(223, 64);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(59, 15);
		this.label29.TabIndex = 528;
		this.label29.Text = "Rate Uom";
		this.txtBillWeightPurchase.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBillWeightPurchase.Location = new System.Drawing.Point(759, 36);
		this.txtBillWeightPurchase.Name = "txtBillWeightPurchase";
		this.txtBillWeightPurchase.ReadOnly = true;
		this.txtBillWeightPurchase.Size = new System.Drawing.Size(114, 23);
		this.txtBillWeightPurchase.TabIndex = 8;
		this.txtBillWeightPurchase.TabStop = false;
		this.txtBillWeightPurchase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBillWeightSale.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBillWeightSale.Location = new System.Drawing.Point(6, 84);
		this.txtBillWeightSale.Name = "txtBillWeightSale";
		this.txtBillWeightSale.ReadOnly = true;
		this.txtBillWeightSale.Size = new System.Drawing.Size(127, 23);
		this.txtBillWeightSale.TabIndex = 13;
		this.txtBillWeightSale.TabStop = false;
		this.txtBillWeightSale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtEBPerUnitPurchase.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEBPerUnitPurchase.Location = new System.Drawing.Point(626, 36);
		this.txtEBPerUnitPurchase.Name = "txtEBPerUnitPurchase";
		this.txtEBPerUnitPurchase.Size = new System.Drawing.Size(63, 23);
		this.txtEBPerUnitPurchase.TabIndex = 6;
		this.txtEBPerUnitPurchase.Tag = "EbUnitPurchase";
		this.txtEBPerUnitPurchase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtEBPerUnitPurchase.TextChanged += new System.EventHandler(txtEBPerUnit_TextChanged);
		this.txtEBPerUnitPurchase.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtEBPerUnit_KeyPress);
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(748, 64);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(52, 15);
		this.label28.TabIndex = 516;
		this.label28.Text = "Remarks";
		this.groupBox3.Controls.Add(this.groupBox8);
		this.groupBox3.Controls.Add(this.groupBox7);
		this.groupBox3.Controls.Add(this.label3);
		this.groupBox3.Controls.Add(this.txtMainRemarks);
		this.groupBox3.Controls.Add(this.txtDocDate);
		this.groupBox3.Controls.Add(this.label15);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTradingAccount);
		this.groupBox3.Controls.Add(this.txtBranchSrNo);
		this.groupBox3.Controls.Add(this.TxtDocNo);
		this.groupBox3.Controls.Add(this.label38);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(4, 2);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1232, 135);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Main";
		this.groupBox8.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSalePaymentTerm);
		this.groupBox8.Controls.Add(this.datSaleDueDate);
		this.groupBox8.Controls.Add(this.label67);
		this.groupBox8.Controls.Add(this.txtSaleDueDays);
		this.groupBox8.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomer);
		this.groupBox8.Controls.Add(this.label12);
		this.groupBox8.Controls.Add(this.txtSaleBillAmount);
		this.groupBox8.Controls.Add(this.label33);
		this.groupBox8.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryTermSale);
		this.groupBox8.Controls.Add(this.label55);
		this.groupBox8.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox8.Location = new System.Drawing.Point(717, 9);
		this.groupBox8.Name = "groupBox8";
		this.groupBox8.Size = new System.Drawing.Size(354, 122);
		this.groupBox8.TabIndex = 5;
		this.groupBox8.TabStop = false;
		this.groupBox8.Text = "Sale";
		this.CmbSalePaymentTerm.AllowNull = (DefaultableBoolean)1;
		this.CmbSalePaymentTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSalePaymentTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSalePaymentTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance233).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance233).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSalePaymentTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.CmbSalePaymentTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSalePaymentTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance235).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance235).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance235).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSalePaymentTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance235;
		((AppearanceBase)appearance236).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSalePaymentTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance236;
		((SpecialBoxBase)((UltraGridBase)this.CmbSalePaymentTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance237).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance237).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance237).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSalePaymentTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.CmbSalePaymentTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSalePaymentTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance238).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSalePaymentTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance238;
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance239).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSalePaymentTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance239;
		((UltraGridBase)this.CmbSalePaymentTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSalePaymentTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSalePaymentTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance240;
		((AppearanceBase)appearance241).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance241).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSalePaymentTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance241;
		((UltraGridBase)this.CmbSalePaymentTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSalePaymentTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance242).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance242).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance242).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance242).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSalePaymentTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance242;
		((AppearanceBase)appearance243).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSalePaymentTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance243;
		((UltraGridBase)this.CmbSalePaymentTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSalePaymentTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance244).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSalePaymentTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.CmbSalePaymentTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance246).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSalePaymentTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance246;
		((UltraGridBase)this.CmbSalePaymentTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSalePaymentTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSalePaymentTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSalePaymentTerm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSalePaymentTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSalePaymentTerm).Location = new System.Drawing.Point(215, 39);
		((System.Windows.Forms.Control)(object)this.CmbSalePaymentTerm).Name = "CmbSalePaymentTerm";
		((System.Windows.Forms.Control)(object)this.CmbSalePaymentTerm).Size = new System.Drawing.Size(134, 26);
		((System.Windows.Forms.Control)(object)this.CmbSalePaymentTerm).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbSalePaymentTerm).TextChanged += new System.EventHandler(CmbSalePaymentTerm_TextChanged);
		this.datSaleDueDate.CustomFormat = "dd-MMM-yyyy";
		this.datSaleDueDate.Enabled = false;
		this.datSaleDueDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datSaleDueDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datSaleDueDate.Location = new System.Drawing.Point(215, 66);
		this.datSaleDueDate.Name = "datSaleDueDate";
		this.datSaleDueDate.Size = new System.Drawing.Size(134, 23);
		this.datSaleDueDate.TabIndex = 4;
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label67.Location = new System.Drawing.Point(1, 70);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(98, 15);
		this.label67.TabIndex = 491;
		this.label67.Text = "Due Days && Date";
		this.txtSaleDueDays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSaleDueDays.Location = new System.Drawing.Point(99, 66);
		this.txtSaleDueDays.Name = "txtSaleDueDays";
		this.txtSaleDueDays.Size = new System.Drawing.Size(115, 23);
		this.txtSaleDueDays.TabIndex = 3;
		this.txtSaleDueDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtSaleDueDays.TextChanged += new System.EventHandler(txtSaleDueDays_TextChanged);
		this.CmbCustomer.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance247).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance247).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance247;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance248).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance248).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance248).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance248;
		((AppearanceBase)appearance249).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance249;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance250).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance250).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance250).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance250).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance250;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance251).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance251;
		((AppearanceBase)appearance252).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance252).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance252;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance253).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance253;
		((AppearanceBase)appearance254).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance254).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance254;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance255).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance255).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance255).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance255).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance255).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance255;
		((AppearanceBase)appearance257).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance257;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance258).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance258).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance258;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance259).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance259;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Location = new System.Drawing.Point(99, 12);
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Name = "CmbCustomer";
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Size = new System.Drawing.Size(250, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomer).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).TextChanged += new System.EventHandler(CmbCustomer_TextChanged);
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Leave += new System.EventHandler(CmbCustomer_Leave);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(1, 18);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(93, 15);
		this.label12.TabIndex = 482;
		this.label12.Text = "Customer Name";
		this.txtSaleBillAmount.Font = new System.Drawing.Font("Verdana", 11f);
		this.txtSaleBillAmount.Location = new System.Drawing.Point(99, 91);
		this.txtSaleBillAmount.Name = "txtSaleBillAmount";
		this.txtSaleBillAmount.ReadOnly = true;
		this.txtSaleBillAmount.Size = new System.Drawing.Size(250, 25);
		this.txtSaleBillAmount.TabIndex = 5;
		this.txtSaleBillAmount.TabStop = false;
		this.txtSaleBillAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(4, 95);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(82, 17);
		this.label33.TabIndex = 497;
		this.label33.Text = "Bill Amount";
		this.CmbDeliveryTermSale.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryTermSale.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryTermSale.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryTermSale.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance286).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance286).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.Appearance = (AppearanceBase)(object)appearance286;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance5;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance10).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance11).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance69).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryTermSale).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermSale).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryTermSale.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermSale).Location = new System.Drawing.Point(99, 39);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermSale).Name = "CmbDeliveryTermSale";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermSale).Size = new System.Drawing.Size(115, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermSale).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTermSale).TextChanged += new System.EventHandler(CmbDeliveryTermSale_TextChanged);
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.Location = new System.Drawing.Point(1, 35);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(93, 32);
		this.label55.TabIndex = 486;
		this.label55.Text = "Delivery && Payment Term";
		this.groupBox7.Controls.Add(this.label16);
		this.groupBox7.Controls.Add(this.label1);
		this.groupBox7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.groupBox7.Controls.Add(this.label8);
		this.groupBox7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryTerms);
		this.groupBox7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentTerms);
		this.groupBox7.Controls.Add(this.txtDueDate);
		this.groupBox7.Controls.Add(this.txtPurchaseBillAmount);
		this.groupBox7.Controls.Add(this.label2);
		this.groupBox7.Controls.Add(this.txtDueDays);
		this.groupBox7.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox7.Location = new System.Drawing.Point(352, 9);
		this.groupBox7.Name = "groupBox7";
		this.groupBox7.Size = new System.Drawing.Size(360, 122);
		this.groupBox7.TabIndex = 4;
		this.groupBox7.TabStop = false;
		this.groupBox7.Text = "Purchase";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(7, 97);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(82, 17);
		this.label16.TabIndex = 498;
		this.label16.Text = "Bill Amount";
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(6, 38);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(93, 33);
		this.label1.TabIndex = 498;
		this.label1.Text = "Delivery && Payment Term";
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance52).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(105, 14);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(251, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TextChanged += new System.EventHandler(CmbSupplier_TextChanged);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Leave += new System.EventHandler(CmbSupplier_Leave);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(6, 20);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(86, 15);
		this.label8.TabIndex = 471;
		this.label8.Text = "Supplier Name";
		this.CmbDeliveryTerms.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryTerms.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryTerms.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryTerms.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance114).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerms).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryTerms.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerms).Location = new System.Drawing.Point(105, 41);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerms).Name = "CmbDeliveryTerms";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerms).Size = new System.Drawing.Size(115, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerms).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerms).TextChanged += new System.EventHandler(CmbDeliveryTerms_TextChanged);
		this.CmbPaymentTerms.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentTerms.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentTerms.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentTerms.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Appearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance199).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance199).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance200;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance202).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance202).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance202).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance203;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance206).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance207).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance207).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance207).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance207).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance207;
		((AppearanceBase)appearance208).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerms).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPaymentTerms.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerms).Location = new System.Drawing.Point(222, 41);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerms).Name = "CmbPaymentTerms";
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerms).Size = new System.Drawing.Size(134, 26);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerms).TabIndex = 2;
		this.CmbPaymentTerms.ValueChanged += new System.EventHandler(CmbPaymentTerms_ValueChanged);
		this.txtDueDate.CustomFormat = "dd-MMM-yyyy";
		this.txtDueDate.Enabled = false;
		this.txtDueDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDueDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDueDate.Location = new System.Drawing.Point(222, 69);
		this.txtDueDate.Name = "txtDueDate";
		this.txtDueDate.Size = new System.Drawing.Size(134, 23);
		this.txtDueDate.TabIndex = 4;
		this.txtPurchaseBillAmount.Font = new System.Drawing.Font("Verdana", 11f);
		this.txtPurchaseBillAmount.Location = new System.Drawing.Point(105, 93);
		this.txtPurchaseBillAmount.Name = "txtPurchaseBillAmount";
		this.txtPurchaseBillAmount.ReadOnly = true;
		this.txtPurchaseBillAmount.Size = new System.Drawing.Size(251, 25);
		this.txtPurchaseBillAmount.TabIndex = 5;
		this.txtPurchaseBillAmount.TabStop = false;
		this.txtPurchaseBillAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPurchaseBillAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBillAmount_KeyPress);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(6, 73);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(98, 15);
		this.label2.TabIndex = 466;
		this.label2.Text = "Due Days && Date";
		this.txtDueDays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDueDays.Location = new System.Drawing.Point(105, 69);
		this.txtDueDays.Name = "txtDueDays";
		this.txtDueDays.Size = new System.Drawing.Size(115, 23);
		this.txtDueDays.TabIndex = 3;
		this.txtDueDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDueDays.TextChanged += new System.EventHandler(txtDueDays_TextChanged);
		this.txtDueDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDueDays_KeyPress);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(3, 16);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(48, 30);
		this.label3.TabIndex = 464;
		this.label3.Text = "Doc No\r\nDate";
		this.txtMainRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMainRemarks.Location = new System.Drawing.Point(71, 70);
		this.txtMainRemarks.Multiline = true;
		this.txtMainRemarks.Name = "txtMainRemarks";
		this.txtMainRemarks.Size = new System.Drawing.Size(275, 59);
		this.txtMainRemarks.TabIndex = 3;
		this.txtDocDate.CustomFormat = "dd-MMM-yyyy";
		this.txtDocDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocDate.Location = new System.Drawing.Point(232, 18);
		this.txtDocDate.Name = "txtDocDate";
		this.txtDocDate.Size = new System.Drawing.Size(114, 22);
		this.txtDocDate.TabIndex = 1;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(3, 81);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(52, 15);
		this.label15.TabIndex = 485;
		this.label15.Text = "Remarks";
		this.CmbTradingAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbTradingAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTradingAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTradingAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTradingAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance190;
		((SpecialBoxBase)((UltraGridBase)this.CmbTradingAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance214;
		((AppearanceBase)appearance215).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance215).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance216).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance216).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance216).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance216).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance216;
		((AppearanceBase)appearance217).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance218).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance218;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance219).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTradingAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTradingAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTradingAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTradingAccount).Location = new System.Drawing.Point(71, 42);
		((System.Windows.Forms.Control)(object)this.CmbTradingAccount).Name = "CmbTradingAccount";
		((System.Windows.Forms.Control)(object)this.CmbTradingAccount).Size = new System.Drawing.Size(275, 26);
		((System.Windows.Forms.Control)(object)this.CmbTradingAccount).TabIndex = 2;
		this.txtBranchSrNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBranchSrNo.Location = new System.Drawing.Point(152, 17);
		this.txtBranchSrNo.Name = "txtBranchSrNo";
		this.txtBranchSrNo.ReadOnly = true;
		this.txtBranchSrNo.Size = new System.Drawing.Size(79, 23);
		this.txtBranchSrNo.TabIndex = 0;
		this.txtBranchSrNo.TabStop = false;
		this.TxtDocNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.TxtDocNo.Location = new System.Drawing.Point(71, 17);
		this.TxtDocNo.Name = "TxtDocNo";
		this.TxtDocNo.ReadOnly = true;
		this.TxtDocNo.Size = new System.Drawing.Size(79, 23);
		this.TxtDocNo.TabIndex = 0;
		this.TxtDocNo.TabStop = false;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(3, 48);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(52, 30);
		this.label38.TabIndex = 530;
		this.label38.Text = "Trading \r\nAccount";
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label18);
		this.panel2.Controls.Add(this.label52);
		this.panel2.Controls.Add(this.ChkBox);
		this.panel2.Controls.Add(this.txtProfitLoss);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.ForeColor = System.Drawing.Color.White;
		this.panel2.Location = new System.Drawing.Point(0, 25);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1429, 27);
		this.panel2.TabIndex = 2;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.Color.White;
		this.label18.Location = new System.Drawing.Point(7, 3);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(244, 19);
		this.label18.TabIndex = 25;
		this.label18.Text = "Commission Agent Trade Bill";
		this.label52.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(1072, 4);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(94, 18);
		this.label52.TabIndex = 497;
		this.label52.Text = "Profit/Loss";
		this.ChkBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBox.AutoSize = true;
		this.ChkBox.Checked = true;
		this.ChkBox.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkBox.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBox.Location = new System.Drawing.Point(1341, 4);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(84, 20);
		this.ChkBox.TabIndex = 531;
		this.ChkBox.Text = "Preview";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.txtProfitLoss.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.txtProfitLoss.Font = new System.Drawing.Font("Verdana", 11f);
		this.txtProfitLoss.Location = new System.Drawing.Point(1170, 1);
		this.txtProfitLoss.Name = "txtProfitLoss";
		this.txtProfitLoss.ReadOnly = true;
		this.txtProfitLoss.Size = new System.Drawing.Size(164, 25);
		this.txtProfitLoss.TabIndex = 13;
		this.txtProfitLoss.TabStop = false;
		this.txtProfitLoss.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.ToolstripPanel.Controls.Add(this.toolstrip);
		this.ToolstripPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.ToolstripPanel.Location = new System.Drawing.Point(0, 0);
		this.ToolstripPanel.Name = "ToolstripPanel";
		this.ToolstripPanel.Size = new System.Drawing.Size(1429, 25);
		this.ToolstripPanel.TabIndex = 0;
		this.toolstrip.Dock = System.Windows.Forms.DockStyle.Fill;
		this.toolstrip.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolstrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[12]
		{
			this.btnnew, this.btnRefresh, this.btnsave, this.btnUpdate, this.BtnDelete, this.btnAttachment, this.btnPrint, this.btnPrint170, this.btnPrint171, this.btnprint172,
			this.BtnSlipNew1706, this.btnPrint103
		});
		this.toolstrip.Location = new System.Drawing.Point(0, 0);
		this.toolstrip.Name = "toolstrip";
		this.toolstrip.Size = new System.Drawing.Size(1429, 25);
		this.toolstrip.TabIndex = 6;
		this.toolstrip.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 22);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 22);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 22);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 22);
		this.btnUpdate.Text = "Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.BtnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.BtnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnDelete.Name = "BtnDelete";
		this.BtnDelete.Size = new System.Drawing.Size(73, 22);
		this.BtnDelete.Text = "Delete";
		this.BtnDelete.Visible = false;
		this.BtnDelete.Click += new System.EventHandler(BtnDelete_Click);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 22);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(85, 22);
		this.btnPrint.Text = "168-Slip";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnPrint170.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint170.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint170.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint170.Name = "btnPrint170";
		this.btnPrint170.Size = new System.Drawing.Size(150, 22);
		this.btnPrint170.Text = "Purchase Slip-170";
		this.btnPrint170.Click += new System.EventHandler(btnPrint170_Click);
		this.btnPrint171.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint171.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint171.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint171.Name = "btnPrint171";
		this.btnPrint171.Size = new System.Drawing.Size(118, 22);
		this.btnPrint171.Text = "Sale Slip-171";
		this.btnPrint171.Click += new System.EventHandler(btnPrint171_Click);
		this.btnprint172.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnprint172.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnprint172.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint172.Name = "btnprint172";
		this.btnprint172.Size = new System.Drawing.Size(140, 22);
		this.btnprint172.Text = "172-Sale Bill Slip";
		this.btnprint172.Click += new System.EventHandler(btnprint172_Click);
		this.BtnSlipNew1706.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnSlipNew1706.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnSlipNew1706.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnSlipNew1706.Name = "BtnSlipNew1706";
		this.BtnSlipNew1706.Size = new System.Drawing.Size(93, 22);
		this.BtnSlipNew1706.Text = "1706-Slip";
		this.BtnSlipNew1706.Click += new System.EventHandler(BtnSlipNew1706_Click);
		this.btnPrint103.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint103.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint103.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint103.Name = "btnPrint103";
		this.btnPrint103.Size = new System.Drawing.Size(92, 22);
		this.btnPrint103.Text = "Print-118";
		this.btnPrint103.Click += new System.EventHandler(btnPrint103_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistory);
		this.tabPage2.Controls.Add(this.panel11);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel20);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.GrdDetailHistory);
		this.tabPage2.Controls.Add(this.panel4);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1435, 585);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.GrdHistory.AllowEdit = (InheritableBoolean)2;
		GrdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistory.DesignTimeLayout = GrdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdHistory.DynamicFiltering = true;
		this.GrdHistory.FilterMode = (FilterMode)2;
		this.GrdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Location = new System.Drawing.Point(3, 152);
		((System.Windows.Forms.Control)(object)this.GrdHistory).Name = "GrdHistory";
		((ScrollableBase)this.GrdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Size = new System.Drawing.Size(1429, 146);
		((System.Windows.Forms.Control)(object)this.GrdHistory).TabIndex = 66;
		this.GrdHistory.TotalRow = (InheritableBoolean)1;
		this.GrdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistory).VisualStyle = (VisualStyle)4;
		this.GrdHistory.ColumnButtonClick += new ColumnActionEventHandler(GrdHistory_ColumnButtonClick);
		this.GrdHistory.SelectionChanged += new System.EventHandler(GrdHistory_SelectionChanged);
		this.GrdHistory.LinkClicked += new ColumnActionEventHandler(GrdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdHistory).DoubleClick += new System.EventHandler(GrdHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.GrdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(GrdHistory_KeyDown);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.ctrlGrdBar3);
		this.panel11.Controls.Add(this.label42);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel11.ForeColor = System.Drawing.Color.White;
		this.panel11.Location = new System.Drawing.Point(3, 298);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1429, 23);
		this.panel11.TabIndex = 65;
		this.GrdDetailHistory.AllowEdit = (InheritableBoolean)2;
		GrdDetailHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdDetailHistory.DesignTimeLayout = GrdDetailHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdDetailHistory).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.GrdDetailHistory.DynamicFiltering = true;
		this.GrdDetailHistory.FilterMode = (FilterMode)2;
		this.GrdDetailHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdDetailHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdDetailHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdDetailHistory).Location = new System.Drawing.Point(3, 321);
		((System.Windows.Forms.Control)(object)this.GrdDetailHistory).Name = "GrdDetailHistory";
		((ScrollableBase)this.GrdDetailHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdDetailHistory).Size = new System.Drawing.Size(1429, 261);
		((System.Windows.Forms.Control)(object)this.GrdDetailHistory).TabIndex = 47;
		this.GrdDetailHistory.TotalRow = (InheritableBoolean)1;
		this.GrdDetailHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdDetailHistory).VisualStyle = (VisualStyle)4;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label42.ForeColor = System.Drawing.Color.White;
		this.label42.Location = new System.Drawing.Point(4, 5);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(191, 14);
		this.label42.TabIndex = 25;
		this.label42.Text = "Detail Of Above Selected Row";
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label66);
		this.panel9.Controls.Add(this.ctrlGrdBar2);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.ForeColor = System.Drawing.Color.White;
		this.panel9.Location = new System.Drawing.Point(3, 125);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1429, 27);
		this.panel9.TabIndex = 64;
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label66.ForeColor = System.Drawing.Color.White;
		this.label66.Location = new System.Drawing.Point(3, 4);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(132, 18);
		this.label66.TabIndex = 25;
		this.label66.Text = "Filtered Records";
		this.panel20.BackColor = System.Drawing.Color.White;
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcusthistory);
		this.panel20.Controls.Add(this.label51);
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.cmbsupplierhistory);
		this.panel20.Controls.Add(this.label50);
		this.panel20.Controls.Add(this.label45);
		this.panel20.Controls.Add(this.txtOrderNoTo);
		this.panel20.Controls.Add(this.txtOrderNoFrom);
		this.panel20.Controls.Add(this.label46);
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.cmbperemeter);
		this.panel20.Controls.Add(this.label47);
		this.panel20.Controls.Add(this.btnshowHistory);
		this.panel20.Controls.Add(this.ToDate);
		this.panel20.Controls.Add(this.fromdate);
		this.panel20.Controls.Add(this.label48);
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.cmbtradAcHistory);
		this.panel20.Controls.Add(this.label49);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(3, 54);
		this.panel20.Margin = new System.Windows.Forms.Padding(2);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(1429, 71);
		this.panel20.TabIndex = 63;
		this.cmbcusthistory.AllowNull = (DefaultableBoolean)1;
		this.cmbcusthistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcusthistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcusthistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcusthistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.cmbcusthistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcusthistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcusthistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcusthistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcusthistory).Location = new System.Drawing.Point(563, 36);
		((System.Windows.Forms.Control)(object)this.cmbcusthistory).Name = "cmbcusthistory";
		((System.Windows.Forms.Control)(object)this.cmbcusthistory).Size = new System.Drawing.Size(219, 26);
		((System.Windows.Forms.Control)(object)this.cmbcusthistory).TabIndex = 6;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(499, 42);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(58, 15);
		this.label51.TabIndex = 602;
		this.label51.Text = "Customer";
		this.cmbsupplierhistory.AllowNull = (DefaultableBoolean)1;
		this.cmbsupplierhistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbsupplierhistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbsupplierhistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbsupplierhistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbsupplierhistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbsupplierhistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbsupplierhistory).Location = new System.Drawing.Point(563, 7);
		((System.Windows.Forms.Control)(object)this.cmbsupplierhistory).Name = "cmbsupplierhistory";
		((System.Windows.Forms.Control)(object)this.cmbsupplierhistory).Size = new System.Drawing.Size(219, 26);
		((System.Windows.Forms.Control)(object)this.cmbsupplierhistory).TabIndex = 5;
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(499, 13);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(51, 15);
		this.label50.TabIndex = 600;
		this.label50.Text = "Supplier";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(334, 42);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(64, 15);
		this.label45.TabIndex = 598;
		this.label45.Text = "Doc No To";
		this.txtOrderNoTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderNoTo.Location = new System.Drawing.Point(415, 38);
		this.txtOrderNoTo.Name = "txtOrderNoTo";
		this.txtOrderNoTo.ShortcutsEnabled = false;
		this.txtOrderNoTo.Size = new System.Drawing.Size(81, 23);
		this.txtOrderNoTo.TabIndex = 4;
		this.txtOrderNoTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOrderNoTo_KeyPress);
		this.txtOrderNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderNoFrom.Location = new System.Drawing.Point(415, 9);
		this.txtOrderNoFrom.Name = "txtOrderNoFrom";
		this.txtOrderNoFrom.ShortcutsEnabled = false;
		this.txtOrderNoFrom.Size = new System.Drawing.Size(81, 23);
		this.txtOrderNoFrom.TabIndex = 3;
		this.txtOrderNoFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOrderNoFrom_KeyPress);
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(334, 13);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(79, 15);
		this.label46.TabIndex = 597;
		this.label46.Text = "Doc No From";
		this.cmbperemeter.AllowNull = (DefaultableBoolean)1;
		this.cmbperemeter.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbperemeter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbperemeter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbperemeter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Location = new System.Drawing.Point(102, 7);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Name = "cmbperemeter";
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).TabIndex = 0;
		this.cmbperemeter.ValueChanged += new System.EventHandler(cmbperemeter_ValueChanged);
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(5, 13);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(60, 15);
		this.label47.TabIndex = 594;
		this.label47.Text = "Date Type";
		this.btnshowHistory.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshowHistory.FlatAppearance.BorderSize = 0;
		this.btnshowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshowHistory.ForeColor = System.Drawing.Color.White;
		this.btnshowHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshowHistory.Location = new System.Drawing.Point(1010, 35);
		this.btnshowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnshowHistory.Name = "btnshowHistory";
		this.btnshowHistory.Size = new System.Drawing.Size(68, 26);
		this.btnshowHistory.TabIndex = 8;
		this.btnshowHistory.Text = "Show";
		this.btnshowHistory.UseVisualStyleBackColor = false;
		this.btnshowHistory.Click += new System.EventHandler(btnshowHistory_Click);
		this.btnshowHistory.Leave += new System.EventHandler(btnshowHistory_Leave);
		this.ToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.CustomFormat = "dd-MMM-yy";
		this.ToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDate.Location = new System.Drawing.Point(217, 38);
		this.ToDate.Name = "ToDate";
		this.ToDate.Size = new System.Drawing.Size(115, 23);
		this.ToDate.TabIndex = 2;
		this.fromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.CustomFormat = "dd-MMM-yy";
		this.fromdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.fromdate.Location = new System.Drawing.Point(102, 38);
		this.fromdate.Name = "fromdate";
		this.fromdate.Size = new System.Drawing.Size(112, 23);
		this.fromdate.TabIndex = 1;
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(5, 42);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(91, 15);
		this.label48.TabIndex = 146;
		this.label48.Text = "Date From && To";
		this.cmbtradAcHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbtradAcHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbtradAcHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbtradAcHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbtradAcHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbtradAcHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbtradAcHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbtradAcHistory).Location = new System.Drawing.Point(884, 7);
		((System.Windows.Forms.Control)(object)this.cmbtradAcHistory).Name = "cmbtradAcHistory";
		((System.Windows.Forms.Control)(object)this.cmbtradAcHistory).Size = new System.Drawing.Size(194, 26);
		((System.Windows.Forms.Control)(object)this.cmbtradAcHistory).TabIndex = 7;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(788, 13);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(95, 15);
		this.label49.TabIndex = 144;
		this.label49.Text = "Trading Account";
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label35);
		this.panel4.Controls.Add(this.toolStrip2);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 3);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1429, 51);
		this.panel4.TabIndex = 0;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.ForeColor = System.Drawing.Color.White;
		this.label35.Location = new System.Drawing.Point(4, 29);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(307, 19);
		this.label35.TabIndex = 25;
		this.label35.Text = "Commission Agent Trade Bill History";
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNewHistory, this.btnGrnFormHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1429, 27);
		this.toolStrip2.TabIndex = 2;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(55, 24);
		this.btnNewHistory.Text = "New";
		this.btnNewHistory.ToolTipText = "Crtl_H";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnGrnFormHistory.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnGrnFormHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnGrnFormHistory.Name = "btnGrnFormHistory";
		this.btnGrnFormHistory.Size = new System.Drawing.Size(73, 24);
		this.btnGrnFormHistory.Text = "Register";
		this.btnGrnFormHistory.ToolTipText = "Crtl_H";
		this.btnGrnFormHistory.Click += new System.EventHandler(btnGrnFormHistory_Click);
		this.txtSaleGrossWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSaleGrossWeight.Location = new System.Drawing.Point(874, 36);
		this.txtSaleGrossWeight.Name = "txtSaleGrossWeight";
		this.txtSaleGrossWeight.Size = new System.Drawing.Size(102, 23);
		this.txtSaleGrossWeight.TabIndex = 9;
		this.txtSaleGrossWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtSaleGrossWeight.TextChanged += new System.EventHandler(txtSaleGrossWeight_TextChanged);
		this.txtSaleGrossWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSaleGrossWeight_KeyPress);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(874, 16);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(103, 15);
		this.label17.TabIndex = 541;
		this.label17.Text = "Sale Gross Weight";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1389, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdDetail;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 540;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1386, 0);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.GrdDetailHistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar3.TabIndex = 26;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1386, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = null;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar2.TabIndex = 24;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1443, 661);
		base.Controls.Add(this.MainPanel);
		base.KeyPreview = true;
		base.Name = "CommissionAgentTradeBill_162";
		this.Text = "Commission Agent Trade Bill";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(CommissionAgentTradeBill_162_FormClosing);
		base.Load += new System.EventHandler(frmCommissionAgentOrder_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(CommissionAgentTradeBill_162_KeyDown);
		this.MainPanel.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.tabControl2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInvExpForPurchase).EndInit();
		this.tabPage4.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdFreightPurchase).EndInit();
		this.tabPage5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInvExpForSale).EndInit();
		this.tabPage6.ResumeLayout(false);
		this.panel14.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPackingMaterialSale).EndInit();
		this.tabPage7.ResumeLayout(false);
		this.groupBox6.ResumeLayout(false);
		this.groupBox6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCommDrAcCust).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommAgentCust).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryDrAcCust).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokerCust).EndInit();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryDrAcSupp).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokerSupp).EndInit();
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCommDrAcSupp).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommAgentSupp).EndInit();
		this.panel3.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).EndInit();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		this.groupBox8.ResumeLayout(false);
		this.groupBox8.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSalePaymentTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomer).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTermSale).EndInit();
		this.groupBox7.ResumeLayout(false);
		this.groupBox7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerms).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTerms).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTradingAccount).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.ToolstripPanel.ResumeLayout(false);
		this.ToolstripPanel.PerformLayout();
		this.toolstrip.ResumeLayout(false);
		this.toolstrip.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistory).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdDetailHistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel20.ResumeLayout(false);
		this.panel20.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbcusthistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsupplierhistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbtradAcHistory).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
