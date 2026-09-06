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
using Architecture.BLL.Reports.Party_Processing;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
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
namespace Architecture.WinApp.CommissionAgent
{

public class frmCommissionAgentTradeBill : Form
{
	private Rightsobjects formrights = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private DataTable table = new DataTable();

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	private int RecId;

	private int updateRowIndex;

	private double CommAmount;

	private double BrokeryAmount;

	private List<InvCommAgentTradeBillDetail> lstRemoveRecord = new List<InvCommAgentTradeBillDetail>();

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

	private Label label9;

	private Label label8;

	private Label label7;

	private Label label6;

	private Label label5;

	private Label label4;

	private Label label2;

	private Label label1;

	private TextBox TxtDocNo;

	internal UltraCombo CmbSupplier;

	internal UltraCombo CmbCustomer;

	private TextBox txtMainRemarks;

	internal UltraCombo CmbDeliveryTerms;

	private TextBox txtDueDays;

	internal UltraCombo CmbPaymentTerms;

	private Label label17;

	private Label label16;

	private Label label15;

	private Label label12;

	private DateTimePicker txtDueDate;

	internal UltraCombo CmbBroker;

	private TextBox txtCommPercent;

	internal UltraCombo CmbCommAgent;

	private TextBox txtBrokeryPercent;

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

	private TextBox txtGrossWeight;

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

	private GroupBox groupBox1;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel4;

	private ToolStrip toolStrip2;

	private ToolStripButton btnLoadAll;

	private ToolStripButton btnGrnFormHistory;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel6;

	private Panel panel7;

	internal GridEX GrdHistory;

	internal GridEX GrdDetailHistory;

	private Panel panel5;

	internal GridEX grdDetail;

	private TextBox txtRefDocNo;

	private Label label32;

	private TextBox txtVehicleNo;

	private Label label27;

	private Label label31;

	private TextBox txtBillWeight;

	private TextBox txtPurchaseBillAmount;

	private Label label30;

	internal UltraCombo CmbOrderNo;

	private Label label29;

	private Label label14;

	private TextBox txtBrokeryAmount;

	private Label label13;

	private TextBox txtCommAmount;

	private TextBox txtSaleBillAmount;

	private Label label33;

	public frmCommissionAgentTradeBill(UserAccount pUser)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUser;
	}

	private void frmCommissionAgentOrder_Load(object sender, EventArgs e)
	{
		try
		{
			AccountsFill();
			PaymentTermsFill();
			ItemFill();
			CropYearFill();
			DeliveryTermsFill();
			OrderNoFill();
			GenerateCode();
			dtViewAttachments.Columns.Add("AttchmentPath");
			table.Columns.Add("Id");
			table.Columns.Add("ItemId");
			table.Columns.Add("ItemName");
			table.Columns.Add("CropYearId");
			table.Columns.Add("CropYear");
			table.Columns.Add("PackTypeId");
			table.Columns.Add("PackTypeDesc");
			table.Columns.Add("UomId");
			table.Columns.Add("UomCode");
			table.Columns.Add("ItemQty", typeof(double));
			table.Columns.Add("GrossWeight", typeof(double));
			table.Columns.Add("BillWeight", typeof(double));
			table.Columns.Add("RatePurchase", typeof(double));
			table.Columns.Add("RateSale", typeof(double));
			table.Columns.Add("PurchaseAmount", typeof(double));
			table.Columns.Add("SaleAmount", typeof(double));
			table.Columns.Add("VehicleNo");
			table.Columns.Add("RefDocNo");
			table.Columns.Add("RemarksDetail");
			TxtDocNo.Focus();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			grdDetail.DataSource = table;
			grdDetail.RetrieveStructure();
			grdSettings();
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			btnLoadAll.Visible = false;
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
			DataTable SuppCust = CommonServices.SupplierCustomerGetAllServiceBind();
			DataTable accounts = new DataTable();
			if (SuppCust.Rows.Count > 0)
			{
				if (accounts.Columns.Count == 0)
				{
					accounts.Columns.Add("Id");
					accounts.Columns.Add("CompanyName");
				}
				accounts.Rows.Clear();
				for (int i = 0; i < SuppCust.Rows.Count; i++)
				{
					accounts.Rows.Add(SuppCust.Rows[i]["Id"], SuppCust.Rows[i]["CompanyName"]);
				}
				if (accounts.Rows.Count > 0)
				{
					DDL.BindDDL(accounts, CmbSupplier, "Id", "CompanyName", "Supplier", ZeroIndex: false);
					DDL.BindDDL(accounts, CmbBroker, "Id", "CompanyName", "Broker A/c", ZeroIndex: false);
					DDL.BindDDL(accounts, CmbCommAgent, "Id", "CompanyName", "Comm Agent", ZeroIndex: false);
					DDL.BindDDL(accounts, CmbCustomer, "Id", "CompanyName", "Customer", ZeroIndex: false);
				}
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
			DataTable dtDueTerms = CommonServices.GetDueTermServiceBind();
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("TermsDescription");
			if (dtDueTerms.Rows.Count > 0)
			{
				for (int i = 0; i < dtDueTerms.Rows.Count; i++)
				{
					dt.Rows.Add(dtDueTerms.Rows[i]["Id"], dtDueTerms.Rows[i]["TermsDescription"]);
				}
			}
			DDL.BindDDL(dt, CmbPaymentTerms, "Id", "TermsDescription", "Payment Terms", ZeroIndex: false);
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
			dt.Rows.Add(3, "Load & PartyWeight");
			dt.Rows.Add(4, "Load & FactoryWeight");
			dt.Rows.Add(5, "Ponch & PartyWeight");
			dt.Rows.Add(6, "Ponch & FactoryWeight");
			DDL.BindDDLNew(dt, CmbDeliveryTerms, "Id", "Name", "Delivery Term", false);
			((UltraGridBase)CmbDeliveryTerms).Rows[0].Activate();
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
		//IL_002c: Expected O, but got Unknown
		try
		{
			int Code = InvCommAgentTradeBill.GenerateCode(new InvCommAgentTradeBill
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			TxtDocNo.Text = Conversion.ToString((object)Code);
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
			DataTable dt = CommonServices.ItemGetForComboServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbItemName, "Id", "ItemName", "Item Name", ZeroIndex: false);
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
			DataTable dtCropYear = CommonServices.CropYearGetAllService();
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("CropYear");
			if (dtCropYear.Rows.Count > 0)
			{
				for (int i = 0; i < dtCropYear.Rows.Count; i++)
				{
					dt.Rows.Add(dtCropYear.Rows[i]["Id"], dtCropYear.Rows[i]["CropYear"]);
				}
			}
			DDL.BindDDL(dt, CmbCropYear, "Id", "CropYear", "Crop Year", ZeroIndex: false);
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
			DataTable dt = CommonServices.InvPackingTypeGetAllService();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbPackType, "Id", "PackTypeDesc", "Pack Type", ZeroIndex: false);
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
			DataTable dt = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(CmbItemName.Value));
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbPackUom, "Id", "UOMCode", "Pack Uom", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OrderNoFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtOrder = InvCommAgentOrder.FormHistory(new InvCommAgentOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("DocNo");
			if (dtOrder.Rows.Count > 0)
			{
				for (int i = 0; i < dtOrder.Rows.Count; i++)
				{
					dt.Rows.Add(dtOrder.Rows[i]["Id"], dtOrder.Rows[i]["DocNo"]);
				}
			}
			DDL.BindDDL(dt, CmbOrderNo, "Id", "DocNo", "Order No", ZeroIndex: false);
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
			throw;
		}
	}

	private void MainFormReset()
	{
		try
		{
			((Control)(object)CmbSupplier).Text = string.Empty;
			((Control)(object)CmbCustomer).Text = string.Empty;
			((Control)(object)CmbCommAgent).Text = string.Empty;
			txtCommPercent.Clear();
			((Control)(object)CmbBroker).Text = string.Empty;
			txtBrokeryPercent.Clear();
			((Control)(object)CmbPaymentTerms).Text = string.Empty;
			txtDueDays.Clear();
			txtDueDate.Text = Conversion.ToString((object)DateTime.Now);
			((Control)(object)CmbDeliveryTerms).Text = string.Empty;
			((Control)(object)CmbOrderNo).Text = string.Empty;
			txtPurchaseBillAmount.Clear();
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
			((Control)(object)CmbItemName).Text = string.Empty;
			((Control)(object)CmbCropYear).Text = string.Empty;
			((Control)(object)CmbPackType).Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			txtQuantity.Clear();
			txtGrossWeight.Clear();
			txtBillWeight.Clear();
			txtRatePurchase.Clear();
			txtRateSale.Clear();
			txtPurchaseAmount.Clear();
			txtSaleAmount.Clear();
			txtVehicleNo.Clear();
			txtRefDocNo.Clear();
			txtDetailRemarks.Clear();
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
			RecId = 0;
			MainFormReset();
			DetailFormReset();
			GenerateCode();
			table.Rows.Clear();
			grdDetail.DataSource = table;
			dtViewAttachments.Clear();
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			btnAddDetail.Visible = true;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
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
			OrderNoFill();
			GenerateCode();
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
		if (((UltraGridBase)CmbSupplier).ActiveRow == null)
		{
			MessageBox.Show("Supplier Field is Required");
			CmbSupplier.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCustomer).ActiveRow == null)
		{
			MessageBox.Show("Customer Field is Required");
			CmbCustomer.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCommAgent).ActiveRow == null)
		{
			MessageBox.Show("Comm Agent Field is Required");
			CmbCommAgent.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtCommPercent.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Comm% Field Required");
			txtCommPercent.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPaymentTerms).ActiveRow == null)
		{
			MessageBox.Show("Payment Term Field is Required");
			CmbPaymentTerms.Focus();
			return false;
		}
		if (Conversion.ToInt((object)txtDueDays.Text.Trim()) == 0)
		{
			MessageBox.Show("Due Days Field Required");
			txtDueDays.Focus();
			return false;
		}
		if (((UltraGridBase)CmbDeliveryTerms).ActiveRow == null)
		{
			MessageBox.Show("Delivery Term Field is Required");
			CmbDeliveryTerms.Focus();
			return false;
		}
		if (((UltraGridBase)CmbOrderNo).ActiveRow == null)
		{
			MessageBox.Show("Order No Field Is Required");
			CmbOrderNo.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtPurchaseBillAmount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Bill Amount Should Be Greater Than 0");
			txtPurchaseBillAmount.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)CmbItemName).ActiveRow == null)
		{
			MessageBox.Show("Item Field Required");
			CmbItemName.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCropYear).ActiveRow == null)
		{
			MessageBox.Show("Crop Year Field Required");
			CmbCropYear.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackType).ActiveRow == null)
		{
			MessageBox.Show("Pack Type Field Required");
			CmbPackType.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackUom).ActiveRow == null)
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
		if (Conversion.ToDouble((object)txtGrossWeight.Text.Trim()) == 0.0)
		{
			MessageBox.Show(" Gross Weight Field is Required");
			txtGrossWeight.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtBillWeight.Text.Trim()) == 0.0)
		{
			MessageBox.Show(" Bill Weight Field is Required");
			txtGrossWeight.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtRatePurchase.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Rate Purchase Field is Required");
			txtRatePurchase.Focus();
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
		return true;
	}

	private void btnAddDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				table.Rows.Add(0, CmbItemName.Value, ((Control)(object)CmbItemName).Text, CmbCropYear.Value, ((Control)(object)CmbCropYear).Text.Trim(), CmbPackType.Value, ((Control)(object)CmbPackType).Text.Trim(), CmbPackUom.Value, ((Control)(object)CmbPackUom).Text.Trim(), Conversion.ToDouble((object)txtQuantity.Text.Trim()), Conversion.ToDouble((object)txtGrossWeight.Text.Trim()), Conversion.ToDouble((object)txtBillWeight.Text.Trim()), Conversion.ToDouble((object)txtRatePurchase.Text.Trim()), Conversion.ToDouble((object)txtRateSale.Text.Trim()), Conversion.ToDouble((object)txtPurchaseAmount.Text.Trim()), Conversion.ToDouble((object)txtSaleAmount.Text.Trim()), txtVehicleNo.Text, txtRefDocNo.Text, txtDetailRemarks.Text.Trim());
				grdDetail.DataSource = table;
				grdDetail.RetrieveStructure();
				grdSettings();
				DetailFormReset();
				CalculateCommAndBrokeryAmount();
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
			grdDetail.RootTable.Columns.Add("Delete");
			grdDetail.RootTable.Columns["Delete"].Key = "Delete";
			grdDetail.RootTable.Columns["Delete"].Caption = "X";
			grdDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdDetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdDetail.RootTable.Columns["Delete"].Width = 20;
			grdDetail.RootTable.Columns["Id"].Visible = false;
			grdDetail.RootTable.Columns["ItemId"].Visible = false;
			grdDetail.RootTable.Columns["CropYearId"].Visible = false;
			grdDetail.RootTable.Columns["PackTypeId"].Visible = false;
			grdDetail.RootTable.Columns["UomId"].Visible = false;
			grdDetail.RootTable.Columns["ItemName"].Width = 250;
			grdDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["PurchaseAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["SaleAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["BillWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["PurchaseAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["SaleAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["BillWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemQty"].FormatString = "#,##.##";
			grdDetail.RootTable.Columns["PurchaseAmount"].FormatString = "#,##.##";
			grdDetail.RootTable.Columns["SaleAmount"].FormatString = "#,##.##";
			grdDetail.RootTable.Columns["GrossWeight"].FormatString = "#,##.##";
			grdDetail.RootTable.Columns["BillWeight"].FormatString = "#,##.##";
			grdDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##.##";
			grdDetail.RootTable.Columns["PurchaseAmount"].TotalFormatString = "#,##.##";
			grdDetail.RootTable.Columns["SaleAmount"].TotalFormatString = "#,##.##";
			grdDetail.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##.##";
			grdDetail.RootTable.Columns["BillWeight"].TotalFormatString = "#,##.##";
			ctrlGrdBar1_Load(null, null);
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

	private void txtDueDays_KeyPress(object sender, KeyPressEventArgs e)
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
			CommonServices.OnlytextdecimelFunction(sender, e);
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

	private void CalculateAmount()
	{
		try
		{
			if (txtRatePurchase.Text != string.Empty && txtQuantity.Text != string.Empty)
			{
				double purchaseRate = Conversion.ToDouble((object)txtRatePurchase.Text);
				double Qty2 = double.Parse(txtQuantity.Text);
				txtPurchaseAmount.Text = (purchaseRate * Qty2).ToString("0,0");
			}
			if (txtRateSale.Text != string.Empty && txtQuantity.Text != string.Empty)
			{
				double SaleRate = Conversion.ToDouble((object)txtRateSale.Text);
				double Qty = double.Parse(txtQuantity.Text);
				txtSaleAmount.Text = (SaleRate * Qty).ToString("0,0");
			}
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

	private void grdDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdDetail.CurrentRow;
			if (item != null)
			{
				updateRowIndex = item.RowIndex;
				CmbItemName.Value = item.Cells["ItemId"].Value.ToString();
				CmbCropYear.Value = item.Cells["CropYearId"].Text.ToString();
				CmbPackType.Value = item.Cells["PackTypeId"].Value.ToString();
				CmbPackUom.Value = item.Cells["UomId"].Value.ToString();
				txtQuantity.Text = item.Cells["ItemQty"].Value.ToString();
				txtGrossWeight.Text = item.Cells["GrossWeight"].Value.ToString();
				txtBillWeight.Text = item.Cells["BillWeight"].Value.ToString();
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
				table.Rows[updateRowIndex]["ItemId"] = CmbItemName.Value;
				table.Rows[updateRowIndex]["ItemName"] = ((Control)(object)CmbItemName).Text;
				table.Rows[updateRowIndex]["CropYearId"] = CmbCropYear.Value;
				table.Rows[updateRowIndex]["CropYear"] = ((Control)(object)CmbCropYear).Text;
				table.Rows[updateRowIndex]["PackTypeId"] = CmbPackType.Value;
				table.Rows[updateRowIndex]["PackTypeDesc"] = ((Control)(object)CmbPackType).Text;
				table.Rows[updateRowIndex]["UomId"] = CmbPackUom.Value;
				table.Rows[updateRowIndex]["UomCode"] = ((Control)(object)CmbPackUom).Text;
				table.Rows[updateRowIndex]["ItemQty"] = Conversion.ToDouble((object)txtQuantity.Text);
				table.Rows[updateRowIndex]["GrossWeight"] = Conversion.ToDouble((object)txtGrossWeight.Text);
				table.Rows[updateRowIndex]["BillWeight"] = Conversion.ToDouble((object)txtGrossWeight.Text);
				table.Rows[updateRowIndex]["RatePurchase"] = Conversion.ToDouble((object)txtRatePurchase.Text.Trim());
				table.Rows[updateRowIndex]["RateSale"] = Conversion.ToDouble((object)txtRateSale.Text);
				table.Rows[updateRowIndex]["PurchaseAmount"] = Conversion.ToDouble((object)txtPurchaseAmount.Text.Trim());
				table.Rows[updateRowIndex]["SaleAmount"] = Conversion.ToDouble((object)txtSaleAmount.Text.Trim());
				table.Rows[updateRowIndex]["VehicleNo"] = txtVehicleNo.Text.Trim();
				table.Rows[updateRowIndex]["RefDocNo"] = txtRefDocNo.Text.Trim();
				table.Rows[updateRowIndex]["RemarksDetail"] = txtDetailRemarks.Text.Trim();
				btnAddDetail.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelDetail.Visible = false;
				DetailFormReset();
				CalculateCommAndBrokeryAmount();
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
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Insert()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Expected O, but got Unknown
		//IL_05cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d4: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			if (grdDetail.GetRows().Count() == 0)
			{
				MessageBox.Show("Grid Record Not Found");
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
			obj.BranchesId = 0;
			obj.ProjectsId = 0;
			obj.DocDate = Conversion.ToDateTime((object)txtDocDate.Value);
			obj.DocNo = Convert.ToInt32(TxtDocNo.Text.Trim());
			obj.SupplierId = Conversion.ToInt(CmbSupplier.Value);
			obj.CustomerId = Conversion.ToInt(CmbCustomer.Value);
			obj.CommissionAgentId = Conversion.ToInt(CmbCommAgent.Value);
			obj.CommRate = Conversion.ToDouble((object)txtCommPercent.Text);
			if (Conversion.ToInt(CmbBroker.Value) > 0)
			{
				obj.BrokerId = Conversion.ToInt(CmbBroker.Value);
			}
			else
			{
				obj.BrokerId = 0;
			}
			if (Conversion.ToDecimal((object)txtBrokeryPercent.Text) > 0m)
			{
				obj.BrokeryRate = Conversion.ToDouble((object)txtBrokeryPercent.Text);
			}
			else
			{
				obj.BrokeryRate = 0.0;
			}
			obj.PaymentTermId = Conversion.ToInt(CmbPaymentTerms.Value);
			obj.DueDays = Conversion.ToInt((object)txtDueDays.Text);
			obj.DueDate = Conversion.ToDateTime((object)txtDueDate.Value);
			obj.DeliveryTerm = Conversion.ToString((object)((Control)(object)CmbDeliveryTerms).Text);
			obj.PurchaseBillAmount = Conversion.ToDouble((object)txtPurchaseBillAmount.Text);
			obj.SaleBillAmount = Conversion.ToDouble((object)txtSaleBillAmount.Text);
			obj.InvCommAgentOrderId = Conversion.ToInt(CmbOrderNo.Value);
			obj.RemarksHeader = txtMainRemarks.Text.Trim();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.EnteryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.ApprovedDate = DateTime.Now;
			obj.EnteryUserId = UserAccount.ID;
			obj.ModifyUserId = UserAccount.ID;
			obj.ApprovalUserId = UserAccount.ID;
			obj.IsApproved = false;
			obj.DocumentTypeId = 160;
			obj.CommAmount = CommAmount;
			obj.BrokeryAmount = BrokeryAmount;
			obj.InvCommAgentTradeBillDetailslist = new List<InvCommAgentTradeBillDetail>();
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow r in rows)
			{
				InvCommAgentTradeBillDetail orderDetail = new InvCommAgentTradeBillDetail();
				orderDetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
				orderDetail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				orderDetail.ItemName = Conversion.ToString((object)r.Cells["ItemName"].Text);
				orderDetail.Crop = Conversion.ToInt(r.Cells["CropYearId"].Value);
				orderDetail.CropYear = Conversion.ToString((object)r.Cells["CropYear"].Text);
				orderDetail.PackingTypeId = Conversion.ToInt(r.Cells["PackTypeId"].Value);
				orderDetail.PackTypeDesc = Conversion.ToString((object)r.Cells["PackTypeDesc"].Text);
				orderDetail.PackUomId = Conversion.ToInt(r.Cells["UomId"].Value);
				orderDetail.PackUom = Conversion.ToString((object)r.Cells["UomCode"].Text);
				orderDetail.Qty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
				orderDetail.GrossWeight = Conversion.ToDecimal(r.Cells["GrossWeight"].Value);
				orderDetail.BillWeight = Conversion.ToDecimal(r.Cells["BillWeight"].Value);
				orderDetail.RatePurchase = Conversion.ToDecimal(r.Cells["RatePurchase"].Value);
				orderDetail.RateSale = Conversion.ToDecimal(r.Cells["RateSale"].Value);
				orderDetail.ItemAmount = Conversion.ToDouble(r.Cells["PurchaseAmount"].Value);
				orderDetail.SaleAmount = Conversion.ToDouble(r.Cells["SaleAmount"].Value);
				orderDetail.VehicleNo = Conversion.ToString(r.Cells["VehicleNo"].Value);
				orderDetail.RefDocNo = Conversion.ToString(r.Cells["RefDocNo"].Value);
				orderDetail.RemarksDetail = Conversion.ToString((object)r.Cells["RemarksDetail"].Text.Trim());
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
			obj.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			for (int j = 0; j < Attachment.dtAttachments.Rows.Count; j++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Attachment.dtAttachments.Rows[j][1].ToString();
				attach.ScreenName = base.Name;
				obj.AttachmentsList.Add(attach);
			}
			if (lstRemoveRecord.Count > 0)
			{
				for (int i = 0; i < lstRemoveRecord.Count; i++)
				{
					obj.InvCommAgentTradeBillDetailslist.Add(lstRemoveRecord[i]);
				}
			}
			InvCommAgentTradeBill.Save(obj);
			if (RecId > 0)
			{
				MessageBox.Show("Receord Update Successfully [ " + obj.DocNo + " ] ");
			}
			else
			{
				MessageBox.Show("Receord Save Successfully  [ " + obj.DocNo + " ] ");
			}
			Reset();
			grdDetail.DataSource = null;
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

	private void grdDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				GridEXRow r = grdDetail.CurrentRow;
				if (btnsave.Visible && btnsave.Enabled)
				{
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
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
					detail.BillWeight = Conversion.ToDecimal(r.Cells["BillWeight"].Value);
					detail.RatePurchase = Conversion.ToDecimal(r.Cells["RatePurchase"].Value);
					detail.RateSale = Conversion.ToDecimal(r.Cells["RateSale"].Value);
					detail.ItemAmount = Conversion.ToDouble(r.Cells["PurchaseAmount"].Value);
					detail.SaleAmount = Conversion.ToDouble(r.Cells["SaleAmount"].Value);
					detail.VehicleNo = Conversion.ToString(r.Cells["VehicleNo"].Value);
					detail.RefDocNo = Conversion.ToString(r.Cells["RefDocNo"].Value);
					detail.RemarksDetail = Conversion.ToString((object)r.Cells["RemarksDetail"].Text.Trim());
					detail.ActionTypeId = 3;
					lstRemoveRecord.Add(detail);
					table.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
				}
			}
			grdDetail.UpdateData();
			CalculateCommAndBrokeryAmount();
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

	private void HistoryGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		try
		{
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 160
			};
			DataTable dtHistory = new DataTable();
			dtHistory = InvCommAgentTradeBill.FormHistory(val);
			if (dtHistory.Rows.Count > 0)
			{
				GrdHistory.DataSource = dtHistory;
				GrdHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				GrdHistory.ClearStructure();
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
			GrdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			GrdHistory.RootTable.Columns.Add("Detail");
			GrdHistory.RootTable.Columns["Detail"].Caption = "Detail";
			GrdHistory.RootTable.Columns["Detail"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistory.RootTable.Columns["Detail"].ButtonStyle = (ButtonStyle)5;
			GrdHistory.RootTable.Columns["Detail"].ButtonText = "Detail";
			GrdHistory.RootTable.Columns["Detail"].Width = 30;
			GrdHistory.RootTable.Columns.Add("Edit");
			GrdHistory.RootTable.Columns["Edit"].Caption = "Edit";
			GrdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			GrdHistory.RootTable.Columns["Edit"].Image = Resources.icons8_pencil_16;
			GrdHistory.RootTable.Columns["Edit"].Width = 25;
			GrdHistory.RootTable.Columns.Add("Print");
			GrdHistory.RootTable.Columns["Print"].Caption = "Print";
			GrdHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			GrdHistory.RootTable.Columns["Print"].Image = Resources.icons8_print_16;
			GrdHistory.RootTable.Columns["Print"].Width = 25;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnLoadAll_Click(object sender, EventArgs e)
	{
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void GrdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Edit")
			{
				Reset();
				RecId = Conversion.ToInt((object)GrdHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById_Update(RecId);
			}
			if (e.Column.Key == "Print")
			{
				DataTable dt = new DataTable();
				dt = GrnGdnReports.GrnAndGdnSlipAndRegister(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeId = 159,
					Id = Conversion.ToInt((object)GrdHistory.CurrentRow.Cells["Id"].Value.ToString())
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("Not Record Found For Display");
				}
				else
				{
					Reporting val = new Reporting();
					val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
					val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
					val.ShowReportWithDataTable(dt, "");
				}
			}
			if (e.Column.Key == "Detail")
			{
				InvCommAgentTradeBill CommAgentBill = InvCommAgentTradeBill.GetByID(Conversion.ToInt(GrdHistory.CurrentRow.Cells["Id"].Value));
				DataTable dthistorydetail = new DataTable();
				dthistorydetail.Columns.Add("ItemName");
				dthistorydetail.Columns.Add("CropYear");
				dthistorydetail.Columns.Add("PackTypeDesc");
				dthistorydetail.Columns.Add("UOMCode");
				dthistorydetail.Columns.Add("Qty", typeof(double));
				dthistorydetail.Columns.Add("GrossWeight", typeof(double));
				dthistorydetail.Columns.Add("BillWeight", typeof(double));
				dthistorydetail.Columns.Add("RatePurchase", typeof(double));
				dthistorydetail.Columns.Add("RateSale", typeof(double));
				dthistorydetail.Columns.Add("PurchaseAmount", typeof(double));
				dthistorydetail.Columns.Add("SaleAmount", typeof(double));
				dthistorydetail.Columns.Add("VehicleNo");
				dthistorydetail.Columns.Add("RefDocNo");
				dthistorydetail.Columns.Add("RemarksDetail");
				dthistorydetail.Clear();
				for (int i = 0; i < CommAgentBill.InvCommAgentTradeBillDetailslist.Count; i++)
				{
					dthistorydetail.Rows.Add(CommAgentBill.InvCommAgentTradeBillDetailslist[i].ItemName, CommAgentBill.InvCommAgentTradeBillDetailslist[i].CropYear, CommAgentBill.InvCommAgentTradeBillDetailslist[i].PackTypeDesc, CommAgentBill.InvCommAgentTradeBillDetailslist[i].PackUom, CommAgentBill.InvCommAgentTradeBillDetailslist[i].Qty, CommAgentBill.InvCommAgentTradeBillDetailslist[i].GrossWeight, CommAgentBill.InvCommAgentTradeBillDetailslist[i].BillWeight, CommAgentBill.InvCommAgentTradeBillDetailslist[i].RatePurchase, CommAgentBill.InvCommAgentTradeBillDetailslist[i].RateSale, CommAgentBill.InvCommAgentTradeBillDetailslist[i].ItemAmount, CommAgentBill.InvCommAgentTradeBillDetailslist[i].SaleAmount, CommAgentBill.InvCommAgentTradeBillDetailslist[i].VehicleNo, CommAgentBill.InvCommAgentTradeBillDetailslist[i].RefDocNo, CommAgentBill.InvCommAgentTradeBillDetailslist[i].RemarksDetail);
				}
				GrdDetailHistory.DataSource = dthistorydetail;
				GrdDetailHistory.RetrieveStructure();
				gridhistorydetail();
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
			GrdDetailHistory.RootTable.Columns["ItemName"].Width = 250;
			GrdDetailHistory.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			GrdDetailHistory.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			GrdDetailHistory.RootTable.Columns["BillWeight"].AggregateFunction = (AggregateFunction)2;
			GrdDetailHistory.RootTable.Columns["PurchaseAmount"].AggregateFunction = (AggregateFunction)2;
			GrdDetailHistory.RootTable.Columns["SaleAmount"].AggregateFunction = (AggregateFunction)2;
			GrdDetailHistory.RootTable.Columns["Qty"].FormatString = "0,0";
			GrdDetailHistory.RootTable.Columns["GrossWeight"].FormatString = "0,0";
			GrdDetailHistory.RootTable.Columns["BillWeight"].FormatString = "0,0";
			GrdDetailHistory.RootTable.Columns["PurchaseAmount"].FormatString = "0,0";
			GrdDetailHistory.RootTable.Columns["SaleAmount"].FormatString = "0,0";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById_Update(int ID)
	{
		try
		{
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			RecId = Conversion.ToInt((object)ID);
			InvCommAgentTradeBill obj = InvCommAgentTradeBill.GetByID(RecId);
			tabControl1.SelectedIndex = 0;
			TxtDocNo.Text = obj.DocNo.ToString();
			txtDocDate.Value = obj.DocDate;
			CmbSupplier.Value = obj.SupplierId;
			CmbCustomer.Value = obj.CustomerId;
			CmbCommAgent.Value = obj.CommissionAgentId;
			txtCommPercent.Text = obj.CommRate.ToString();
			txtCommAmount.Text = obj.CommAmount.ToString();
			CmbBroker.Value = obj.BrokerId;
			txtBrokeryPercent.Text = obj.BrokeryRate.ToString();
			txtBrokeryAmount.Text = obj.BrokeryAmount.ToString();
			CmbPaymentTerms.Value = obj.PaymentTermId;
			txtDueDays.Text = obj.DueDays.ToString();
			txtDueDate.Text = obj.DueDate.ToString();
			CmbOrderNo.Value = obj.InvCommAgentOrderId;
			((Control)(object)CmbDeliveryTerms).Text = obj.DeliveryTerm;
			txtPurchaseBillAmount.Text = obj.PurchaseBillAmount.ToString();
			txtSaleBillAmount.Text = obj.SaleBillAmount.ToString();
			txtMainRemarks.Text = obj.RemarksHeader;
			table.Rows.Clear();
			for (int i = 0; i < obj.InvCommAgentTradeBillDetailslist.Count; i++)
			{
				table.Rows.Add(obj.InvCommAgentTradeBillDetailslist[i].Id, obj.InvCommAgentTradeBillDetailslist[i].ItemId, obj.InvCommAgentTradeBillDetailslist[i].ItemName, obj.InvCommAgentTradeBillDetailslist[i].Crop, obj.InvCommAgentTradeBillDetailslist[i].CropYear, obj.InvCommAgentTradeBillDetailslist[i].PackingTypeId, obj.InvCommAgentTradeBillDetailslist[i].PackTypeDesc, obj.InvCommAgentTradeBillDetailslist[i].PackUomId, obj.InvCommAgentTradeBillDetailslist[i].PackUom, obj.InvCommAgentTradeBillDetailslist[i].Qty, obj.InvCommAgentTradeBillDetailslist[i].GrossWeight, obj.InvCommAgentTradeBillDetailslist[i].BillWeight, obj.InvCommAgentTradeBillDetailslist[i].RatePurchase, obj.InvCommAgentTradeBillDetailslist[i].RateSale, obj.InvCommAgentTradeBillDetailslist[i].ItemAmount, obj.InvCommAgentTradeBillDetailslist[i].SaleAmount, obj.InvCommAgentTradeBillDetailslist[i].VehicleNo, obj.InvCommAgentTradeBillDetailslist[i].RefDocNo, obj.InvCommAgentTradeBillDetailslist[i].RemarksDetail);
			}
			grdDetail.DataSource = table;
			grdDetail.RetrieveStructure();
			grdSettings();
			CalculateCommAndBrokeryAmount();
			List<DMSAttachments> byID = global::Architecture.BLL.DMSAttachments.GetByID(int.Parse(RecId.ToString()), base.Name);
			AT = new Attachment();
			foreach (global::Architecture.Model.DMSAttachments Attachment in byID)
			{
				TempAttachments TA = new TempAttachments();
				TA.Id = 0;
				TA.Path = Attachment.Attachment;
				AT.lst.Add(TA);
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
			if (tabControl1.SelectedIndex == 1)
			{
				HistoryGridFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!(e.Column.Key == "NoOfAttachments"))
			{
				return;
			}
			GridEXRow item = GrdHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			List<global::Architecture.Model.DMSAttachments> dMSList = global::Architecture.BLL.DMSAttachments.GetByID(int.Parse(item.Cells["Id"].Value.ToString().ToString()), base.Name);
			dtViewAttachments.Clear();
			if (dMSList.Count > 0)
			{
				foreach (global::Architecture.Model.DMSAttachments itemList in dMSList)
				{
					dtViewAttachments.Rows.Add(itemList.Attachment);
				}
			}
			else
			{
				dtViewAttachments.Clear();
			}
			new AttachmentView(dtViewAttachments).Show();
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
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

	private void CalculateCommAndBrokeryAmount()
	{
		try
		{
			double commRate = Conversion.ToDouble((object)txtCommPercent.Text);
			double purchaseAmount = Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["PurchaseAmount"], (AggregateFunction)2));
			double BrokeryRate = Conversion.ToDouble((object)txtBrokeryPercent.Text);
			double SaleAmount = Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["SaleAmount"], (AggregateFunction)2));
			if (commRate > 0.0 && purchaseAmount > 0.0)
			{
				CommAmount = commRate * purchaseAmount / 100.0;
				txtCommAmount.Text = CommAmount.ToString("#,#;(#,#);0");
			}
			else
			{
				txtCommAmount.Text = "0";
			}
			if (BrokeryRate > 0.0 && SaleAmount > 0.0)
			{
				BrokeryAmount = BrokeryRate * SaleAmount / 100.0;
				txtBrokeryAmount.Text = BrokeryAmount.ToString("#,#;(#,#);0");
			}
			else
			{
				txtBrokeryAmount.Text = "0";
			}
			txtPurchaseBillAmount.Text = purchaseAmount.ToString("#,#;(#,#);0");
			txtSaleBillAmount.Text = SaleAmount.ToString("#,#;(#,#);0");
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
			throw;
		}
	}

	private void frmCommissionAgentTradeBill_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Expected O, but got Unknown
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Expected O, but got Unknown
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Expected O, but got Unknown
		//IL_0485: Unknown result type (might be due to invalid IL or missing references)
		//IL_048f: Expected O, but got Unknown
		//IL_04b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bb: Expected O, but got Unknown
		//IL_051f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0529: Expected O, but got Unknown
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0534: Expected O, but got Unknown
		//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ce: Expected O, but got Unknown
		//IL_05f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fa: Expected O, but got Unknown
		//IL_0611: Unknown result type (might be due to invalid IL or missing references)
		//IL_061b: Expected O, but got Unknown
		//IL_063d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0647: Expected O, but got Unknown
		//IL_06b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c0: Expected O, but got Unknown
		//IL_06d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e1: Expected O, but got Unknown
		//IL_06e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ec: Expected O, but got Unknown
		//IL_0787: Unknown result type (might be due to invalid IL or missing references)
		//IL_0791: Expected O, but got Unknown
		//IL_079d: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a7: Expected O, but got Unknown
		//IL_0cde: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce8: Expected O, but got Unknown
		//IL_65f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_65fb: Expected O, but got Unknown
		//IL_6608: Unknown result type (might be due to invalid IL or missing references)
		//IL_6612: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
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
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
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
		Appearance appearance78 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.CommissionAgent.frmCommissionAgentTradeBill));
		GridEXLayout GrdDetailHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdHistory_DesignTimeLayout = new GridEXLayout();
		this.MainPanel = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtRefDocNo = new System.Windows.Forms.TextBox();
		this.label32 = new System.Windows.Forms.Label();
		this.txtVehicleNo = new System.Windows.Forms.TextBox();
		this.label27 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.txtBillWeight = new System.Windows.Forms.TextBox();
		this.btnCancelDetail = new System.Windows.Forms.Button();
		this.label19 = new System.Windows.Forms.Label();
		this.txtRatePurchase = new System.Windows.Forms.TextBox();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.label23 = new System.Windows.Forms.Label();
		this.CmbPackType = new UltraCombo();
		this.label22 = new System.Windows.Forms.Label();
		this.btnAddDetail = new System.Windows.Forms.Button();
		this.txtRateSale = new System.Windows.Forms.TextBox();
		this.CmbCropYear = new UltraCombo();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.txtDetailRemarks = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.txtPurchaseAmount = new System.Windows.Forms.TextBox();
		this.txtQuantity = new System.Windows.Forms.TextBox();
		this.label25 = new System.Windows.Forms.Label();
		this.CmbItemName = new UltraCombo();
		this.CmbPackUom = new UltraCombo();
		this.txtSaleAmount = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.txtSaleBillAmount = new System.Windows.Forms.TextBox();
		this.label33 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.txtBrokeryAmount = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.txtCommAmount = new System.Windows.Forms.TextBox();
		this.txtPurchaseBillAmount = new System.Windows.Forms.TextBox();
		this.label30 = new System.Windows.Forms.Label();
		this.CmbOrderNo = new UltraCombo();
		this.label29 = new System.Windows.Forms.Label();
		this.txtMainRemarks = new System.Windows.Forms.TextBox();
		this.txtDocDate = new System.Windows.Forms.DateTimePicker();
		this.CmbCustomer = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbDeliveryTerms = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.txtDueDays = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.CmbPaymentTerms = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.TxtDocNo = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.CmbSupplier = new UltraCombo();
		this.txtDueDate = new System.Windows.Forms.DateTimePicker();
		this.txtBrokeryPercent = new System.Windows.Forms.TextBox();
		this.CmbBroker = new UltraCombo();
		this.CmbCommAgent = new UltraCombo();
		this.txtCommPercent = new System.Windows.Forms.TextBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label18 = new System.Windows.Forms.Label();
		this.ToolstripPanel = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolstrip = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.GrdDetailHistory = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.GrdHistory = new GridEX();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.btnGrnFormHistory = new System.Windows.Forms.ToolStripButton();
		this.MainPanel.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPackType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerms).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTerms).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBroker).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommAgent).BeginInit();
		this.panel2.SuspendLayout();
		this.ToolstripPanel.SuspendLayout();
		this.toolstrip.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdDetailHistory).BeginInit();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistory).BeginInit();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.MainPanel.Controls.Add(this.tabControl1);
		this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.MainPanel.Location = new System.Drawing.Point(0, 0);
		this.MainPanel.Name = "MainPanel";
		this.MainPanel.Size = new System.Drawing.Size(1377, 773);
		this.MainPanel.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1377, 773);
		this.tabControl1.TabIndex = 4;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1369, 747);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.groupBox1);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Controls.Add(this.ToolstripPanel);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1363, 741);
		this.panel1.TabIndex = 0;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 371);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1363, 370);
		this.panel5.TabIndex = 7;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1363, 370);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).DoubleClick += new System.EventHandler(grdDetail_DoubleClick);
		this.groupBox1.Controls.Add(this.txtRefDocNo);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add(this.txtVehicleNo);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.label31);
		this.groupBox1.Controls.Add(this.txtBillWeight);
		this.groupBox1.Controls.Add(this.btnCancelDetail);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add(this.txtRatePurchase);
		this.groupBox1.Controls.Add(this.btnUpdateDetail);
		this.groupBox1.Controls.Add(this.label23);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackType);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add(this.btnAddDetail);
		this.groupBox1.Controls.Add(this.txtRateSale);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYear);
		this.groupBox1.Controls.Add(this.txtGrossWeight);
		this.groupBox1.Controls.Add(this.txtDetailRemarks);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.txtPurchaseAmount);
		this.groupBox1.Controls.Add(this.txtQuantity);
		this.groupBox1.Controls.Add(this.label25);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUom);
		this.groupBox1.Controls.Add(this.txtSaleAmount);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
		this.groupBox1.Location = new System.Drawing.Point(0, 240);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1363, 131);
		this.groupBox1.TabIndex = 6;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Detail ";
		this.txtRefDocNo.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRefDocNo.Location = new System.Drawing.Point(79, 96);
		this.txtRefDocNo.Name = "txtRefDocNo";
		this.txtRefDocNo.Size = new System.Drawing.Size(181, 23);
		this.txtRefDocNo.TabIndex = 12;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(4, 100);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(66, 15);
		this.label32.TabIndex = 522;
		this.label32.Text = "Ref Doc No";
		this.txtVehicleNo.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVehicleNo.Location = new System.Drawing.Point(1130, 70);
		this.txtVehicleNo.Name = "txtVehicleNo";
		this.txtVehicleNo.Size = new System.Drawing.Size(223, 23);
		this.txtVehicleNo.TabIndex = 11;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(1016, 74);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(64, 15);
		this.label27.TabIndex = 520;
		this.label27.Text = "Vehicle No";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(640, 48);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(68, 15);
		this.label31.TabIndex = 518;
		this.label31.Text = "Bill Weight";
		this.txtBillWeight.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBillWeight.Location = new System.Drawing.Point(758, 44);
		this.txtBillWeight.Name = "txtBillWeight";
		this.txtBillWeight.Size = new System.Drawing.Size(234, 23);
		this.txtBillWeight.TabIndex = 6;
		this.btnCancelDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelDetail.Location = new System.Drawing.Point(1097, 96);
		this.btnCancelDetail.Name = "btnCancelDetail";
		this.btnCancelDetail.Size = new System.Drawing.Size(57, 23);
		this.btnCancelDetail.TabIndex = 14;
		this.btnCancelDetail.Text = "Cancel";
		this.btnCancelDetail.UseVisualStyleBackColor = true;
		this.btnCancelDetail.Visible = false;
		this.btnCancelDetail.Click += new System.EventHandler(btnCancelDetail_Click);
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(640, 21);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(60, 15);
		this.label19.TabIndex = 499;
		this.label19.Text = "Pack Type";
		this.txtRatePurchase.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRatePurchase.Location = new System.Drawing.Point(1130, 44);
		this.txtRatePurchase.Name = "txtRatePurchase";
		this.txtRatePurchase.Size = new System.Drawing.Size(223, 23);
		this.txtRatePurchase.TabIndex = 7;
		this.txtRatePurchase.TextChanged += new System.EventHandler(txtSupplierRate_TextChanged);
		this.txtRatePurchase.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSupplierRate_KeyPress);
		this.btnUpdateDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.Location = new System.Drawing.Point(1037, 96);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(58, 23);
		this.btnUpdateDetail.TabIndex = 13;
		this.btnUpdateDetail.Text = "Update ";
		this.btnUpdateDetail.UseVisualStyleBackColor = true;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(1016, 48);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(85, 15);
		this.label23.TabIndex = 507;
		this.label23.Text = "Rate Purchase";
		this.CmbPackType.AllowNull = (DefaultableBoolean)1;
		this.CmbPackType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbPackType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance108;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbPackType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance114).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbPackType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackType).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackType).Location = new System.Drawing.Point(758, 15);
		((System.Windows.Forms.Control)(object)this.CmbPackType).Name = "CmbPackType";
		((System.Windows.Forms.Control)(object)this.CmbPackType).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackType).TabIndex = 2;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(281, 48);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(81, 15);
		this.label22.TabIndex = 505;
		this.label22.Text = "Gross Weight";
		this.btnAddDetail.Location = new System.Drawing.Point(996, 96);
		this.btnAddDetail.Name = "btnAddDetail";
		this.btnAddDetail.Size = new System.Drawing.Size(39, 23);
		this.btnAddDetail.TabIndex = 14;
		this.btnAddDetail.Text = "Add";
		this.btnAddDetail.UseVisualStyleBackColor = true;
		this.btnAddDetail.Click += new System.EventHandler(btnAddDetail_Click);
		this.txtRateSale.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRateSale.Location = new System.Drawing.Point(79, 70);
		this.txtRateSale.Name = "txtRateSale";
		this.txtRateSale.Size = new System.Drawing.Size(181, 23);
		this.txtRateSale.TabIndex = 8;
		this.txtRateSale.TextChanged += new System.EventHandler(txtCustomerRate_TextChanged);
		this.txtRateSale.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCustomerRate_KeyPress);
		this.CmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance121;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance127).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Location = new System.Drawing.Point(393, 15);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Name = "CmbCropYear";
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Size = new System.Drawing.Size(209, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).TabIndex = 1;
		this.txtGrossWeight.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrossWeight.Location = new System.Drawing.Point(393, 44);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.Size = new System.Drawing.Size(209, 23);
		this.txtGrossWeight.TabIndex = 5;
		this.txtGrossWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtWeight_KeyPress);
		this.txtDetailRemarks.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDetailRemarks.Location = new System.Drawing.Point(393, 96);
		this.txtDetailRemarks.Name = "txtDetailRemarks";
		this.txtDetailRemarks.Size = new System.Drawing.Size(599, 23);
		this.txtDetailRemarks.TabIndex = 13;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(4, 74);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(57, 15);
		this.label24.TabIndex = 509;
		this.label24.Text = "Rate Sale";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(281, 21);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(59, 15);
		this.label11.TabIndex = 497;
		this.label11.Text = "Crop Year";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(4, 48);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(54, 15);
		this.label21.TabIndex = 503;
		this.label21.Text = "Quantity";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(281, 99);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(54, 15);
		this.label28.TabIndex = 516;
		this.label28.Text = "Remarks";
		this.txtPurchaseAmount.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPurchaseAmount.Location = new System.Drawing.Point(393, 70);
		this.txtPurchaseAmount.Name = "txtPurchaseAmount";
		this.txtPurchaseAmount.ReadOnly = true;
		this.txtPurchaseAmount.Size = new System.Drawing.Size(209, 23);
		this.txtPurchaseAmount.TabIndex = 9;
		this.txtQuantity.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQuantity.Location = new System.Drawing.Point(79, 44);
		this.txtQuantity.Name = "txtQuantity";
		this.txtQuantity.Size = new System.Drawing.Size(181, 23);
		this.txtQuantity.TabIndex = 4;
		this.txtQuantity.TextChanged += new System.EventHandler(txtQuantity_TextChanged);
		this.txtQuantity.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQuantity_KeyPress);
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(281, 74);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(103, 15);
		this.label25.TabIndex = 511;
		this.label25.Text = "Purchase Amount";
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance27;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance32).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance35).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(79, 15);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(181, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Leave += new System.EventHandler(CmbItemName_Leave);
		this.CmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance68;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance74).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance75).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Location = new System.Drawing.Point(1130, 15);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Name = "CmbPackUom";
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Size = new System.Drawing.Size(223, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabIndex = 3;
		this.txtSaleAmount.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSaleAmount.Location = new System.Drawing.Point(758, 70);
		this.txtSaleAmount.Name = "txtSaleAmount";
		this.txtSaleAmount.ReadOnly = true;
		this.txtSaleAmount.Size = new System.Drawing.Size(234, 23);
		this.txtSaleAmount.TabIndex = 10;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(4, 21);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(65, 15);
		this.label10.TabIndex = 495;
		this.label10.Text = "Item Name";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(1016, 21);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(61, 15);
		this.label20.TabIndex = 501;
		this.label20.Text = "Pack Uom";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(640, 74);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(75, 15);
		this.label26.TabIndex = 513;
		this.label26.Text = "Sale Amount";
		this.panel3.Controls.Add(this.txtSaleBillAmount);
		this.panel3.Controls.Add(this.label33);
		this.panel3.Controls.Add(this.label14);
		this.panel3.Controls.Add(this.txtBrokeryAmount);
		this.panel3.Controls.Add(this.label13);
		this.panel3.Controls.Add(this.txtCommAmount);
		this.panel3.Controls.Add(this.txtPurchaseBillAmount);
		this.panel3.Controls.Add(this.label30);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOrderNo);
		this.panel3.Controls.Add(this.label29);
		this.panel3.Controls.Add(this.txtMainRemarks);
		this.panel3.Controls.Add(this.txtDocDate);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomer);
		this.panel3.Controls.Add(this.label3);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryTerms);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Controls.Add(this.txtDueDays);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentTerms);
		this.panel3.Controls.Add(this.label5);
		this.panel3.Controls.Add(this.label17);
		this.panel3.Controls.Add(this.label6);
		this.panel3.Controls.Add(this.label16);
		this.panel3.Controls.Add(this.label7);
		this.panel3.Controls.Add(this.label15);
		this.panel3.Controls.Add(this.label8);
		this.panel3.Controls.Add(this.label9);
		this.panel3.Controls.Add(this.TxtDocNo);
		this.panel3.Controls.Add(this.label12);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.panel3.Controls.Add(this.txtDueDate);
		this.panel3.Controls.Add(this.txtBrokeryPercent);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBroker);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommAgent);
		this.panel3.Controls.Add(this.txtCommPercent);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 80);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1363, 160);
		this.panel3.TabIndex = 0;
		this.txtSaleBillAmount.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSaleBillAmount.Location = new System.Drawing.Point(802, 88);
		this.txtSaleBillAmount.Name = "txtSaleBillAmount";
		this.txtSaleBillAmount.ReadOnly = true;
		this.txtSaleBillAmount.Size = new System.Drawing.Size(194, 23);
		this.txtSaleBillAmount.TabIndex = 496;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(694, 92);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(97, 15);
		this.label33.TabIndex = 497;
		this.label33.Text = "Sale Bill Amount";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(448, 92);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(94, 15);
		this.label14.TabIndex = 495;
		this.label14.Text = "Brokery Amount";
		this.txtBrokeryAmount.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
		this.txtBrokeryAmount.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBrokeryAmount.Location = new System.Drawing.Point(548, 88);
		this.txtBrokeryAmount.Name = "txtBrokeryAmount";
		this.txtBrokeryAmount.ReadOnly = true;
		this.txtBrokeryAmount.Size = new System.Drawing.Size(126, 23);
		this.txtBrokeryAmount.TabIndex = 8;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(449, 38);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(86, 15);
		this.label13.TabIndex = 493;
		this.label13.Text = "Comm Amount";
		this.txtCommAmount.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
		this.txtCommAmount.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommAmount.Location = new System.Drawing.Point(548, 34);
		this.txtCommAmount.Name = "txtCommAmount";
		this.txtCommAmount.ReadOnly = true;
		this.txtCommAmount.Size = new System.Drawing.Size(126, 23);
		this.txtCommAmount.TabIndex = 6;
		this.txtPurchaseBillAmount.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPurchaseBillAmount.Location = new System.Drawing.Point(1130, 61);
		this.txtPurchaseBillAmount.Name = "txtPurchaseBillAmount";
		this.txtPurchaseBillAmount.ReadOnly = true;
		this.txtPurchaseBillAmount.Size = new System.Drawing.Size(223, 23);
		this.txtPurchaseBillAmount.TabIndex = 14;
		this.txtPurchaseBillAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBillAmount_KeyPress);
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(1005, 65);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(125, 15);
		this.label30.TabIndex = 491;
		this.label30.Text = "Purchase Bill Amount";
		this.CmbOrderNo.AllowNull = (DefaultableBoolean)1;
		this.CmbOrderNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOrderNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOrderNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance95;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance98).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance100).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance101).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance101).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance101).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance103).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbOrderNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Location = new System.Drawing.Point(1130, 6);
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Name = "CmbOrderNo";
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Size = new System.Drawing.Size(223, 26);
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).TabIndex = 12;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(1005, 12);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(57, 15);
		this.label29.TabIndex = 489;
		this.label29.Text = "Order No";
		this.txtMainRemarks.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMainRemarks.Location = new System.Drawing.Point(81, 115);
		this.txtMainRemarks.Name = "txtMainRemarks";
		this.txtMainRemarks.Size = new System.Drawing.Size(593, 23);
		this.txtMainRemarks.TabIndex = 15;
		this.txtDocDate.CustomFormat = "dd-MMM-yyyy";
		this.txtDocDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocDate.Location = new System.Drawing.Point(81, 8);
		this.txtDocDate.Name = "txtDocDate";
		this.txtDocDate.Size = new System.Drawing.Size(178, 23);
		this.txtDocDate.TabIndex = 0;
		this.CmbCustomer.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance55;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance61).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance62).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Location = new System.Drawing.Point(81, 86);
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Name = "CmbCustomer";
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Size = new System.Drawing.Size(178, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomer).TabIndex = 3;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(5, 12);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(56, 15);
		this.label3.TabIndex = 464;
		this.label3.Text = "Doc Date";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(694, 12);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(89, 15);
		this.label1.TabIndex = 465;
		this.label1.Text = "Payment Terms";
		this.CmbDeliveryTerms.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryTerms.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryTerms.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryTerms.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.Appearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance3;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance4;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance9).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance10).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance10).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance10).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance38).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryTerms).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerms).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryTerms.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerms).Location = new System.Drawing.Point(1130, 33);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerms).Name = "CmbDeliveryTerms";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerms).Size = new System.Drawing.Size(223, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerms).TabIndex = 13;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(694, 65);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(56, 15);
		this.label2.TabIndex = 466;
		this.label2.Text = "Due Date";
		this.txtDueDays.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDueDays.Location = new System.Drawing.Point(802, 35);
		this.txtDueDays.Name = "txtDueDays";
		this.txtDueDays.Size = new System.Drawing.Size(194, 23);
		this.txtDueDays.TabIndex = 10;
		this.txtDueDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDueDays_KeyPress);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(277, 65);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(70, 15);
		this.label4.TabIndex = 467;
		this.label4.Text = "Brokery A/c";
		this.CmbPaymentTerms.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentTerms.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentTerms.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentTerms.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Appearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance82;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance87).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance88).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance89).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerms).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPaymentTerms.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerms).Location = new System.Drawing.Point(802, 6);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerms).Name = "CmbPaymentTerms";
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerms).Size = new System.Drawing.Size(194, 26);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerms).TabIndex = 9;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(277, 39);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(68, 15);
		this.label5.TabIndex = 468;
		this.label5.Text = "Comm Rate";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(694, 39);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(58, 15);
		this.label17.TabIndex = 487;
		this.label17.Text = "Due Days";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(277, 12);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(74, 15);
		this.label6.TabIndex = 469;
		this.label6.Text = "Comm Agent";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(1005, 39);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(87, 15);
		this.label16.TabIndex = 486;
		this.label16.Text = "Delivery Terms";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(277, 92);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(76, 15);
		this.label7.TabIndex = 470;
		this.label7.Text = "Brokery Rate";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(4, 118);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(54, 15);
		this.label15.TabIndex = 485;
		this.label15.Text = "Remarks";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(5, 65);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(53, 15);
		this.label8.TabIndex = 471;
		this.label8.Text = "Supplier";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(5, 39);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(46, 15);
		this.label9.TabIndex = 472;
		this.label9.Text = "Doc No";
		this.TxtDocNo.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.TxtDocNo.Location = new System.Drawing.Point(81, 35);
		this.TxtDocNo.Name = "TxtDocNo";
		this.TxtDocNo.ReadOnly = true;
		this.TxtDocNo.Size = new System.Drawing.Size(178, 23);
		this.TxtDocNo.TabIndex = 1;
		this.TxtDocNo.TabStop = false;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(4, 92);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(59, 15);
		this.label12.TabIndex = 482;
		this.label12.Text = "Customer";
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance14;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance19).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(81, 59);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(178, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 2;
		this.txtDueDate.CustomFormat = "dd-MMM-yyyy";
		this.txtDueDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDueDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDueDate.Location = new System.Drawing.Point(802, 61);
		this.txtDueDate.Name = "txtDueDate";
		this.txtDueDate.Size = new System.Drawing.Size(194, 23);
		this.txtDueDate.TabIndex = 11;
		this.txtBrokeryPercent.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBrokeryPercent.Location = new System.Drawing.Point(356, 88);
		this.txtBrokeryPercent.Name = "txtBrokeryPercent";
		this.txtBrokeryPercent.Size = new System.Drawing.Size(84, 23);
		this.txtBrokeryPercent.TabIndex = 7;
		this.txtBrokeryPercent.TextChanged += new System.EventHandler(txtBrokeryPercent_TextChanged);
		this.txtBrokeryPercent.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBrokeryPercent_KeyPress);
		this.CmbBroker.AllowNull = (DefaultableBoolean)1;
		this.CmbBroker.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBroker.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBroker.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBroker).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbBroker).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBroker).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBroker).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBroker).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance58;
		((SpecialBoxBase)((UltraGridBase)this.CmbBroker).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBroker).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbBroker).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBroker).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBroker).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBroker).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbBroker).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBroker).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBroker).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBroker).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbBroker).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBroker).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBroker).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBroker).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbBroker).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBroker).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBroker).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbBroker).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBroker).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbBroker).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBroker).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBroker).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBroker).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBroker.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBroker).Location = new System.Drawing.Point(356, 59);
		((System.Windows.Forms.Control)(object)this.CmbBroker).Name = "CmbBroker";
		((System.Windows.Forms.Control)(object)this.CmbBroker).Size = new System.Drawing.Size(318, 26);
		((System.Windows.Forms.Control)(object)this.CmbBroker).TabIndex = 6;
		this.CmbCommAgent.AllowNull = (DefaultableBoolean)1;
		this.CmbCommAgent.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommAgent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommAgent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance42;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance48).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance49).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommAgent).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCommAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommAgent).Location = new System.Drawing.Point(356, 6);
		((System.Windows.Forms.Control)(object)this.CmbCommAgent).Name = "CmbCommAgent";
		((System.Windows.Forms.Control)(object)this.CmbCommAgent).Size = new System.Drawing.Size(318, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommAgent).TabIndex = 4;
		this.txtCommPercent.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
		this.txtCommPercent.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommPercent.Location = new System.Drawing.Point(356, 35);
		this.txtCommPercent.Name = "txtCommPercent";
		this.txtCommPercent.Size = new System.Drawing.Size(84, 23);
		this.txtCommPercent.TabIndex = 5;
		this.txtCommPercent.TextChanged += new System.EventHandler(txtCommPercent_TextChanged);
		this.txtCommPercent.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCommPercent_KeyPress);
		this.panel2.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel2.Controls.Add(this.label18);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 36);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1363, 44);
		this.panel2.TabIndex = 2;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 18f);
		this.label18.ForeColor = System.Drawing.Color.White;
		this.label18.Location = new System.Drawing.Point(5, 6);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(326, 29);
		this.label18.TabIndex = 25;
		this.label18.Text = "Commission Agent Trade Bill";
		this.ToolstripPanel.Controls.Add(this.ctrlGrdBar1);
		this.ToolstripPanel.Controls.Add(this.toolstrip);
		this.ToolstripPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.ToolstripPanel.Location = new System.Drawing.Point(0, 0);
		this.ToolstripPanel.Name = "ToolstripPanel";
		this.ToolstripPanel.Size = new System.Drawing.Size(1363, 36);
		this.ToolstripPanel.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = null;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1317, 3);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = null;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 29);
		this.ctrlGrdBar1.TabIndex = 540;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolstrip.Dock = System.Windows.Forms.DockStyle.Fill;
		this.toolstrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnnew, this.btnsave, this.btnUpdate, this.btnRefresh, this.btnAttachment });
		this.toolstrip.Location = new System.Drawing.Point(0, 0);
		this.toolstrip.Name = "toolstrip";
		this.toolstrip.Size = new System.Drawing.Size(1363, 36);
		this.toolstrip.TabIndex = 6;
		this.toolstrip.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = (System.Drawing.Image)resources.GetObject("btnnew.Image");
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(55, 33);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnsave.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(55, 33);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(69, 33);
		this.btnUpdate.Text = "Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(50, 33);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(94, 33);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.tabPage2.Controls.Add(this.panel4);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1369, 747);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel4.Controls.Add(this.panel7);
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add(this.ctrlGrdBar2);
		this.panel4.Controls.Add(this.toolStrip2);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(3, 3);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1363, 741);
		this.panel4.TabIndex = 0;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.GrdDetailHistory);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 429);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1363, 312);
		this.panel7.TabIndex = 26;
		this.GrdDetailHistory.AllowEdit = (InheritableBoolean)2;
		GrdDetailHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdDetailHistory.DesignTimeLayout = GrdDetailHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdDetailHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdDetailHistory.DynamicFiltering = true;
		this.GrdDetailHistory.FilterMode = (FilterMode)2;
		this.GrdDetailHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdDetailHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdDetailHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdDetailHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GrdDetailHistory).Name = "GrdDetailHistory";
		((ScrollableBase)this.GrdDetailHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdDetailHistory).Size = new System.Drawing.Size(1363, 312);
		((System.Windows.Forms.Control)(object)this.GrdDetailHistory).TabIndex = 47;
		this.GrdDetailHistory.TotalRow = (InheritableBoolean)1;
		this.GrdDetailHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdDetailHistory).VisualStyle = (VisualStyle)4;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistory);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 27);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1363, 402);
		this.panel6.TabIndex = 25;
		this.GrdHistory.AllowEdit = (InheritableBoolean)2;
		GrdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistory.DesignTimeLayout = GrdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdHistory.DynamicFiltering = true;
		this.GrdHistory.FilterMode = (FilterMode)2;
		this.GrdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GrdHistory).Name = "GrdHistory";
		((ScrollableBase)this.GrdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Size = new System.Drawing.Size(1363, 402);
		((System.Windows.Forms.Control)(object)this.GrdHistory).TabIndex = 46;
		this.GrdHistory.TotalRow = (InheritableBoolean)1;
		this.GrdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistory).VisualStyle = (VisualStyle)4;
		this.GrdHistory.ColumnButtonClick += new ColumnActionEventHandler(GrdHistory_ColumnButtonClick);
		this.GrdHistory.LinkClicked += new ColumnActionEventHandler(GrdHistory_LinkClicked);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = null;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1316, -3);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = null;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 27);
		this.ctrlGrdBar2.TabIndex = 24;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnLoadAll, this.btnGrnFormHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1363, 27);
		this.toolStrip2.TabIndex = 2;
		this.toolStrip2.Text = "toolStrip2";
		this.btnLoadAll.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(71, 24);
		this.btnLoadAll.Text = "&LoadAll";
		this.btnLoadAll.ToolTipText = "Ctrl_L";
		this.btnLoadAll.Click += new System.EventHandler(btnLoadAll_Click);
		this.btnGrnFormHistory.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnGrnFormHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnGrnFormHistory.Name = "btnGrnFormHistory";
		this.btnGrnFormHistory.Size = new System.Drawing.Size(73, 24);
		this.btnGrnFormHistory.Text = "Register";
		this.btnGrnFormHistory.ToolTipText = "Crtl_H";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1377, 773);
		base.Controls.Add(this.MainPanel);
		base.Name = "frmCommissionAgentTradeBill";
		this.Text = "Commission Agent Trade Bill";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmCommissionAgentTradeBill_FormClosing);
		base.Load += new System.EventHandler(frmCommissionAgentOrder_Load);
		this.MainPanel.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPackType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomer).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerms).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTerms).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBroker).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommAgent).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.ToolstripPanel.ResumeLayout(false);
		this.ToolstripPanel.PerformLayout();
		this.toolstrip.ResumeLayout(false);
		this.toolstrip.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdDetailHistory).EndInit();
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistory).EndInit();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
