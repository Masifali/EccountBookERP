using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.CommissionAgent.Helper_Methods;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Production;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.StoreManagement;

public class frmSaleOrderStoreAndPm : Form
{
	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formrights = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtGrid = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtBranch = new DataTable();

	private DataTable dtItem = new DataTable();

	private DataTable dtCondition = new DataTable();

	private DataTable dtJobLot = new DataTable();

	private int Id;

	private string ScreenName = "";

	private int DocumentTypeId = 90;

	private int updateDetailIndex = -1;

	private bool FormInitialized;

	private bool Approved;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool ItemSearchWithNameOrCode;

	private bool BranchWiseSaleOrder;

	private static bool CalculateByPerItemSecondaryQty = true;

	private static bool CalculateBySecondaryUomQty = false;

	private static bool CalculateBySecondaryUomRate = true;

	private static bool CalculateByItemRate = false;

	private bool _isUpdatingRates;

	private bool _isUpdatingSecondary;

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel1;

	private Panel panel4;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton btnAttachment;

	private TabPage tabPage2;

	private Panel panel8;

	internal GridEX grd;

	private TextBox txtCommAgentId;

	private TextBox txtSupplierGLId;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdHistory;

	private ToolStripButton btnSlip;

	private Label label51;

	private TextBox txtOrderAmount;

	private TextBox txtremarks;

	private Label label24;

	private TextBox txtduedays;

	private DateTimePicker duedate;

	private Label label7;

	private Label label5;

	private DateTimePicker DocDate;

	private TextBox txtdocno;

	private ToolStripButton btnRefresh;

	private CtrlGrdBar ctrlGrdBar2;

	private CheckBox ChkPrintslip;

	private CheckBox ChkResetOnSave;

	internal UltraCombo CmbCommType;

	internal UltraCombo CmbCommUom;

	internal UltraCombo CmbCommissionAgent;

	private Label label20;

	private Label label26;

	private Label label11;

	private Label label25;

	private Label label21;

	private TextBox txtCommRate;

	private Label label19;

	private TextBox txtCommissionAmount;

	private GroupBox groupBox1;

	private Button btnCancel;

	private Button btnUpdateDetail;

	private Button btnAdd;

	internal UltraCombo CmbRateUOM;

	internal UltraCombo CmbPackUOM;

	internal UltraCombo CmbJobLot;

	internal UltraCombo CmbItemName;

	private Label label42;

	private TextBox txtRate;

	private Label label40;

	private TextBox txtQty;

	private Label label38;

	private Label label32;

	private Label label33;

	private Label label28;

	private Label label22;

	private TextBox txtAmount;

	private Label label14;

	private GroupBox groupBox2;

	private TextBox txtCommissionRemarks;

	private Panel panel11;

	private Label toolStripLabel1;

	private Label label1;

	private TextBox txtRemarksDetail;

	private Label label4;

	private Label label8;

	internal GridEX grdDetail;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnRefreshHistory;

	private Panel panel2;

	private Panel panel10;

	private Panel panel15;

	private Panel panel9;

	private Label label12;

	private Panel panel14;

	private GroupBox groupBox4;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Button btnShowHistory;

	private Label label10;

	private Label label57;

	private Label label58;

	private Label label64;

	private Label label65;

	internal UltraCombo CmbCustomerHistory;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	internal UltraCombo CmbSupplierName;

	private ToolStripButton btnDelete;

	internal UltraCombo CmbTaxType;

	private Label lblTotalAmount;

	private TextBox txtTaxPercent;

	private TextBox txtTotalAmount;

	private Label lblTaxPercent;

	private TextBox txtTaxAmount;

	private Label lblTaxTypes;

	private Label lblTaxAmount;

	private Label label15;

	private Label label16;

	internal UltraCombo CmbPaymentTerm;

	private ToolStripButton btnshortcutkeys;

	private RadioButton rdbtnItemCode;

	private RadioButton rdbtnItemName;

	private Label label2;

	internal UltraCombo CmbDeliveryTerm;

	private TextBox txtManualbillno;

	private Label label6;

	private Label label3;

	internal UltraCombo CmbOrderCategory;

	private Label label9;

	private TextBox txtBranchSrNo;

	internal UltraCombo cmbBranchName;

	private Label label13;

	private ToolStripButton BtnSaveAs;

	internal UltraCombo CmbSecondaryUom;

	private Label label23;

	private Label label17;

	private TextBox txtPerItemWeight;

	private Label label18;

	private TextBox txtSecondaryUomQty;

	private Label label27;

	private TextBox txtSecondaryUomRate;

	internal UltraCombo CmbItemCondition;

	private Label label29;

	private TextBox lblBalance;

	private Label label30;

	private ToolStripButton btnIssuanceLoad;

	public frmSaleOrderStoreAndPm(UserAccount user)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		List<UserAccountAllocation> userAccountAllocationsList = user.UserAccountAllocationsList;
		if (userAccountAllocationsList != null && userAccountAllocationsList.Count > 0)
		{
			ScreenName = user.UserAccountAllocationsList[0].ScreenName;
		}
		InitializeComponentCustom();
		InitializeComponentMethod();
		FormHelper.FormEventHandler(this);
	}

	private void InvfrmPurchasedirectInvoice_Load(object sender, EventArgs e)
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
			dtBranch.Columns.Add("Id", typeof(int));
			dtBranch.Columns.Add("BranchName", typeof(string));
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
			dtGrid.Columns.Add("Id", typeof(int));
			dtGrid.Columns.Add("ItemId", typeof(int));
			dtGrid.Columns.Add("ItemCode");
			dtGrid.Columns.Add("Item");
			dtGrid.Columns.Add("JobLotId", typeof(int));
			dtGrid.Columns.Add("ItemConditionId", typeof(int));
			dtGrid.Columns.Add("PackUOMId", typeof(int));
			dtGrid.Columns.Add("PackUOM");
			dtGrid.Columns.Add("ItemQty", typeof(double));
			dtGrid.Columns.Add("SecondaryUomId", typeof(int));
			dtGrid.Columns.Add("SecondaryUom", typeof(string));
			dtGrid.Columns.Add("SecondaryUomQty", typeof(double));
			dtGrid.Columns.Add("PerItemWeight", typeof(double));
			dtGrid.Columns.Add("SecondaryUomItemRate", typeof(double));
			dtGrid.Columns.Add("ItemRate", typeof(double));
			dtGrid.Columns.Add("RateUOMId", typeof(int));
			dtGrid.Columns.Add("RateUOM");
			dtGrid.Columns.Add("ItemAmount", typeof(double));
			dtGrid.Columns.Add("TaxNameId", typeof(int));
			dtGrid.Columns.Add("TaxName");
			dtGrid.Columns.Add("TaxPercent", typeof(double));
			dtGrid.Columns.Add("TaxAmount", typeof(double));
			dtGrid.Columns.Add("TotalAmount", typeof(double));
			dtGrid.Columns.Add("Remarks", typeof(string));
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
			formrights = CommonServices.SetRightsValueInRightsObject(ScreenName);
			DataTable historyComboData = HistoryComboDbCall(NotValidateBranch: true);
			string documentNoData = DocumentNoDbCall();
			string branchSrNoData = BranchSrNoDbCall();
			return new
			{
				HistoryComboData = historyComboData,
				DocumentNoData = documentNoData,
				BranchSrNoData = branchSrNoData
			};
		}).ContinueWith(t =>
		{
			if (!base.IsDisposed)
			{
				if (t.IsFaulted)
				{
					MessageBox.Show("Error occurred during database call.");
				}
				else
				{
					var result = t.Result;
					btnSave.Enabled = formrights.DoHaveSaveRight;
					btnUpdate.Enabled = formrights.DoHaveUpdateRights;
					btnDelete.Enabled = formrights.DoHaveCanDelete;
					btnSlip.Enabled = formrights.DoHavePrintRights;
					FormHelper.SetCtrlGrdBarRight(ctrlGrdBar1, formrights);
					FormHelper.SetCtrlGrdBarRight(ctrlGrdBar2, formrights);
					ctrlGrdBar2.mGridSaveLayouts.Visible = false;
					ctrlGrdBar2.removeLayoutToolStripMenuItem.Visible = false;
					GetConfigurationsFromGlobal();
					if (Id == 0)
					{
						txtdocno.Text = result.DocumentNoData;
						txtBranchSrNo.Text = result.BranchSrNoData;
					}
					SupplierDtFillFromGlobal();
					BindCustomer();
					PaymentTermBindFromGlobal();
					DeliveryTermFill();
					OrderCatagoryfill();
					CommissionUOMFill();
					CommissionTypeFill();
					CommonBindings.JoblotBindFromGlobal(dtJobLot, CmbJobLot);
					CommonBindings.ItemConditionBindFromGlobal(dtCondition, CmbItemCondition);
					ItemDtsFillFromGlobal();
					ItemNameBind();
					GetConfigurationsFromGlobalandBind();
					BindGrids();
					HistoryBranchComboFill();
					HistoryComboBind(result.HistoryComboData);
					FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
					ToDateHistory.Value = DateTime.Now;
					FormInitialized = true;
					DocDate.Focus();
				}
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void GetConfigurationsFromGlobal()
	{
		try
		{
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"));
			ItemSearchWithNameOrCode = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ItemSearchByCode"));
			BranchWiseSaleOrder = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("SaleOrderBranchWise"));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetConfigurationsFromGlobalandBind()
	{
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
			return SaleOrder.GeneratePurchaseOrderCodeByDocId(new SaleOrder
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

	private string BranchSrNoDbCall()
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
			return SaleOrder.GenerateSaleOrderBranchCodeByDocId(new SaleOrder
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

	private void SupplierDtFillFromGlobal()
	{
		try
		{
			dtSupplier.Clear();
			List<getGlobalSupplierCustomer> customers = clsGlobalVariables.globalSupplierCustomer.ToList();
			if (customers == null || customers.Count == 0)
			{
				return;
			}
			foreach (getGlobalSupplierCustomer item in customers)
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

	private void BindCustomer()
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbSupplierName, dtSupplier, "Id", "CompanyName", "Customer Name", AllColumns: true, Conversion.ToInt(CmbSupplierName.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbCommissionAgent, dtSupplier, "Id", "CompanyName", "Commission Agent Name", AllColumns: true, Conversion.ToInt(CmbCommissionAgent.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dtSupplier.Rows.Count > 0)
			{
				((UltraGridBase)CmbSupplierName).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbCommissionAgent).DisplayLayout.Bands[0].Columns[3].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OrderCatagoryfill()
	{
		try
		{
			DataTable dt = InvOrderCategory.GetForStoreAndPm();
			InfragisticsHelper.BindAndRetainSelection(CmbOrderCategory, dt, "Id", "OrderCategoryName", "Order Category", AllColumns: false, Conversion.ToInt(CmbOrderCategory.Value));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DeliveryTermFill()
	{
		try
		{
			DataTable dtTerm = DeliveryTerm.FormHistory();
			InfragisticsHelper.BindAndRetainSelection(CmbDeliveryTerm, dtTerm, "Id", "Description", "Delivery Term", AllColumns: false, Conversion.ToInt(CmbDeliveryTerm.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PaymentTermBindFromGlobal()
	{
		try
		{
			DataTable dtTerm = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalPaymentTerm, (InvDueTerms x) => x.Id, (InvDueTerms x) => x.TermsDescription);
			InfragisticsHelper.BindAndRetainSelection(CmbPaymentTerm, dtTerm, "Id", "Description", "Payment Term", AllColumns: true, Conversion.ToInt(CmbPaymentTerm.Value));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
			InfragisticsHelper.BindAndRetainSelection(CmbCommUom, dt, "Id", "UOM", "UOM", AllColumns: true, Conversion.ToInt(CmbCommUom.Value), insertDefaultRow: false, null, null, ActivateRow: false);
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
			InfragisticsHelper.BindAndRetainSelection(CmbCommType, dt, "Id", "CommissionType", "CommissionType", AllColumns: true, Conversion.ToInt(CmbCommType.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbOrderCategory_Leave(object sender, EventArgs e)
	{
		try
		{
			ItemDtsFillFromGlobal();
			ItemNameBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDtsFillFromGlobal()
	{
		try
		{
			dtItem.Clear();
			List<getGlobalAllItems> allItems = clsGlobalVariables.getGlobalAllItems;
			if (allItems == null || !allItems.Any())
			{
				return;
			}
			List<getGlobalAllItems> FilterItems = allItems;
			switch (Conversion.ToInt(CmbOrderCategory.Value))
			{
			case 2:
				FilterItems = allItems.Where((getGlobalAllItems x) => x.ItemTypeOfTypeId == 17).ToList();
				break;
			case 9:
				FilterItems = allItems.Where((getGlobalAllItems x) => x.ItemTypeOfTypeId == 14).ToList();
				break;
			}
			foreach (getGlobalAllItems row in FilterItems)
			{
				dtItem.Rows.Add(row.Id, row.ItemName, row.ItemCode, row.ItemCategoryId, row.ItemCategory);
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
			int Id = Conversion.ToInt(CmbItemName.Value);
			string ValueMember = (rdbtnItemName.Checked ? "ItemName" : "ItemCode");
			string DisplayMember = (rdbtnItemName.Checked ? "Item Name" : "Item Code");
			InfragisticsHelper.BindAndRetainSelection(CmbItemName, dtItem, "Id", ValueMember, DisplayMember, AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: false);
			if (dtItem.Rows.Count > 0)
			{
				((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[3].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void comItem_Leave(object sender, EventArgs e)
	{
		try
		{
			int ItemId = Conversion.ToInt(CmbItemName.Value);
			CommonBindings.ItemUomFromGlobalBind(ItemId, CmbPackUOM, CmbRateUOM, CmbSecondaryUom);
			if (((UltraGridBase)CmbItemName).ActiveRow != null && ItemId > 0)
			{
				TaxTypeBind(TaxTypeDbCall(ItemId, DocDate.Value));
			}
			else
			{
				((Control)(object)CmbTaxType).Text = string.Empty;
				((UltraGridBase)CmbTaxType).DataSource = null;
				txtTaxPercent.Clear();
				txtTaxAmount.Clear();
			}
			AvailableStockGetByItem();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rdbtnItemName_CheckedChanged(object sender, EventArgs e)
	{
		ItemNameBind();
	}

	private DataTable TaxTypeDbCall(int ItemId, DateTime DocDate)
	{
		try
		{
			DataTable dtTax = new DataTable();
			dtTax.Columns.Add("Id");
			dtTax.Columns.Add("TaxType");
			dtTax.Columns.Add("TaxPrcnt");
			DataTable dt = CommonServices.GetTaxTypeIdAndPercentByItemId(ItemId, DocDate);
			if (dt.Rows.Count > 0)
			{
				dtTax.Rows.Add(dt.Rows[0]["TaxNameId"], dt.Rows[0]["TaxName"], dt.Rows[0]["TaxPercent"]);
			}
			return dtTax;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void TaxTypeBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbTaxType, dt, "Id", "TaxType", "Tax Name", AllColumns: true, Conversion.ToInt(CmbTaxType.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (updateDetailIndex > -1 && grd.CurrentRow != null && Conversion.ToInt(grd.CurrentRow.Cells["TaxNameId"].Value) > 0)
			{
				CmbTaxType.Value = Conversion.ToInt(grd.CurrentRow.Cells["TaxNameId"].Value);
			}
			CmbTaxType_Leave(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbTaxType_Leave(object sender, EventArgs e)
	{
		try
		{
			CalculateTaxAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPaymentTerm_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)CmbPaymentTerm).Text == "Cash" || ((Control)(object)CmbPaymentTerm).Text == "Advance")
			{
				txtduedays.Enabled = false;
				txtduedays.Text = string.Empty;
				duedate.Value = DocDate.Value;
			}
			else
			{
				txtduedays.Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidationDetila()
	{
		List<(Control, string, FormHelper.ValidType)> controlsToValidate = new List<(Control, string, FormHelper.ValidType)>
		{
			((Control)(object)CmbOrderCategory, "Order Category", FormHelper.ValidType.INT),
			((Control)(object)CmbItemName, "Item Name", FormHelper.ValidType.INT),
			((Control)(object)CmbPackUOM, "Pack Unit", FormHelper.ValidType.INT),
			((Control)(object)CmbJobLot, "Job/Lot", FormHelper.ValidType.INT),
			(txtQty, "Qty", FormHelper.ValidType.Double),
			(txtRate, "Rate", FormHelper.ValidType.Double),
			((Control)(object)CmbRateUOM, "Rate UOM", FormHelper.ValidType.INT),
			(txtAmount, "Amount", FormHelper.ValidType.Double)
		};
		if (Conversion.ToInt(CmbTaxType.Value) > 0 || Conversion.ToDouble((object)txtTaxPercent.Text) > 0.0 || Conversion.ToDouble((object)txtTaxAmount.Text) > 0.0)
		{
			if (((UltraGridBase)CmbTaxType).ActiveRow == null || Conversion.ToInt(CmbTaxType.Value) == 0)
			{
				MessageBox.Show("Tax Name Field is Required");
				CmbTaxType.Focus();
				return false;
			}
			if (Conversion.ToDouble((object)txtTaxPercent.Text) == 0.0)
			{
				MessageBox.Show("Tax Percent Field is Required");
				txtTaxPercent.Focus();
				return false;
			}
			if (Conversion.ToDouble((object)txtTaxAmount.Text) == 0.0)
			{
				MessageBox.Show("Tax Amount Field is Required");
				txtTaxAmount.Focus();
				return false;
			}
		}
		if (!FormHelper.ValidateControls(controlsToValidate))
		{
			return false;
		}
		return true;
	}

	private void BindGrids(HashSet<int> ids = null)
	{
		try
		{
			if (ids == null)
			{
				ids = new HashSet<int> { 1 };
			}
			if (ids.Contains(1))
			{
				DataTable dataTable = dtGrid;
				if (dataTable != null && dataTable.Columns.Count > 0)
				{
					grd.DataSource = dtGrid;
					grd.RetrieveStructure();
					grdSettings(grd);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetila())
			{
				DataRow dr = dtGrid.NewRow();
				FillDetailRow(dr);
				dtGrid.Rows.Add(dr);
				BindGrids(new HashSet<int> { 1 });
				TotalCommissionAmount();
				CalculateOrderAmount();
				EnableOrDisableOrderCategory();
				ResetDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateOrderAmount()
	{
		try
		{
			double OrderAmount = Conversion.ToDouble((object)GridEX_Helper.GetColumnSum(grd, "TotalAmount"));
			txtOrderAmount.Text = OrderAmount.ToString(clsGlobalVariables.stringFormatsingle);
		}
		catch (Exception)
		{
			throw new Exception();
		}
	}

	private void EnableOrDisableOrderCategory()
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				((Control)(object)CmbOrderCategory).Enabled = false;
			}
			else
			{
				((Control)(object)CmbOrderCategory).Enabled = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetila() && updateDetailIndex >= 0 && updateDetailIndex < dtGrid.Rows.Count)
			{
				DataRow dr = dtGrid.Rows[updateDetailIndex];
				FillDetailRow(dr);
				BindGrids(new HashSet<int> { 1 });
				TotalCommissionAmount();
				CalculateOrderAmount();
				ResetDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailRow(DataRow dr)
	{
		dr["ItemId"] = Conversion.ToInt(CmbItemName.Value);
		dr["ItemCode"] = Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemCode"].Value);
		dr["Item"] = Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemName"].Value);
		dr["JobLotId"] = Conversion.ToInt(CmbJobLot.Value);
		dr["ItemConditionId"] = Conversion.ToInt(CmbItemCondition.Value);
		dr["PackUOMId"] = Conversion.ToInt(CmbPackUOM.Value);
		dr["PackUOM"] = Conversion.ToString((object)((Control)(object)CmbPackUOM).Text.Trim());
		double qty = Conversion.ToDouble((object)txtQty.Text);
		dr["ItemQty"] = qty;
		dr["SecondaryUomId"] = Conversion.ToInt(CmbSecondaryUom.Value);
		dr["SecondaryUom"] = Conversion.ToString((object)((Control)(object)CmbSecondaryUom).Text);
		dr["SecondaryUomQty"] = Conversion.ToDouble((object)txtSecondaryUomQty.Text.Trim());
		dr["PerItemWeight"] = Conversion.ToDouble((object)txtPerItemWeight.Text.Trim());
		dr["SecondaryUomItemRate"] = Conversion.ToDouble((object)txtSecondaryUomRate.Text.Trim());
		dr["ItemRate"] = Conversion.ToDouble((object)txtRate.Text);
		dr["RateUOMId"] = Conversion.ToInt(CmbRateUOM.Value);
		dr["RateUOM"] = Conversion.ToString((object)((Control)(object)CmbRateUOM).Text);
		double itemAmount = Conversion.ToDouble((object)txtAmount.Text.Trim());
		dr["ItemAmount"] = itemAmount;
		int taxId = Conversion.ToInt(CmbTaxType.Value);
		double taxPercent = Conversion.ToInt((object)txtTaxPercent.Text);
		double taxAmount = itemAmount * taxPercent / 100.0;
		double totalAmount = itemAmount + taxAmount;
		dr["TaxNameId"] = taxId;
		dr["TaxName"] = ((taxId > 0) ? Conversion.ToString((object)((Control)(object)CmbTaxType).Text) : string.Empty);
		dr["TaxPercent"] = taxPercent;
		dr["TaxAmount"] = taxAmount;
		dr["TotalAmount"] = totalAmount;
		dr["Remarks"] = Conversion.ToString((object)txtRemarksDetail.Text);
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				updateDetailIndex = item.RowIndex;
				int ItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
				CmbItemName.Value = ItemId;
				comItem_Leave(null, null);
				CmbJobLot.Value = item.Cells["JobLotId"].Value.ToString();
				CmbItemCondition.Value = Conversion.ToInt(item.Cells["ItemConditionId"].Value);
				CmbPackUOM.Value = item.Cells["PackUOMId"].Value.ToString();
				txtQty.Text = item.Cells["ItemQty"].Value.ToString();
				CmbSecondaryUom.Value = item.Cells["SecondaryUomId"].Value.ToString();
				txtSecondaryUomQty.Text = item.Cells["SecondaryUomQty"].Text.ToString();
				txtPerItemWeight.Text = item.Cells["PerItemWeight"].Text.ToString();
				txtSecondaryUomRate.Text = item.Cells["SecondaryUomItemRate"].Text.ToString();
				txtRate.Text = item.Cells["ItemRate"].Value.ToString();
				CmbRateUOM.Value = item.Cells["RateUOMId"].Value.ToString();
				txtAmount.Text = item.Cells["ItemAmount"].Value.ToString();
				CmbTaxType.Value = item.Cells["TaxNameId"].Value;
				txtTaxPercent.Text = Conversion.ToString(item.Cells["TaxPercent"].Value);
				txtRemarksDetail.Text = item.Cells["Remarks"].Value.ToString();
				btnAdd.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancel.Visible = true;
				CmbItemName.Focus();
				CalculateAmount();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		try
		{
			ResetDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings(GridEX gridEX)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "ItemId", "PackUOMId", "SecondaryUomId", "RateUOMId", "TaxNameId" };
			Dictionary<string, EditType> editableColumns = null;
			if (((Control)(object)gridEX).Name == "grd")
			{
				editableColumns = new Dictionary<string, EditType> { 
				{
					"Remarks",
					(EditType)1
				} };
			}
			GridEX_Helper.GridWrappingAndColumnSettings(gridEX, 2, 3, hiddenColumns, editableColumns);
			gridEX.RootTable.Columns["SecondaryUomQty"].Caption = "Secondary Uom Weight";
			gridEX.RootTable.Columns["Item"].Width = Constants.InventoryConstants.ItemName;
			gridEX.RootTable.Columns["PackUOM"].Width = Constants.InventoryConstants.UomConstant;
			gridEX.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			gridEX.RootTable.Columns["ItemRate"].Width = Constants.InventoryConstants.ExRateConstant;
			gridEX.RootTable.Columns["RateUOM"].Width = Constants.InventoryConstants.UomConstant;
			gridEX.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			gridEX.RootTable.Columns["TaxName"].Width = Constants.InventoryConstants.DiscountType;
			gridEX.RootTable.Columns["TaxPercent"].Width = Constants.InventoryConstants.ExRateConstant;
			gridEX.RootTable.Columns["TaxAmount"].Width = Constants.InventoryConstants.AmountConstant;
			gridEX.RootTable.Columns["TotalAmount"].Width = Constants.InventoryConstants.AmountConstant;
			gridEX.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			GridEX_Helper.SetColumnWidth(gridEX, "SecondaryUom", Constants.InventoryConstants.UomConstant + 20);
			GridEX_Helper.SetColumnWidth(gridEX, "SecondaryUomQty", Constants.InventoryConstants.WeightConstant + 10);
			GridEX_Helper.SetColumnWidth(gridEX, "SecondaryUomItemRate", Constants.InventoryConstants.ExRateConstant + 20);
			if (((Control)(object)gridEX).Name == "grd")
			{
				DetailGridComboBind();
				GridEX_Helper.AddButton(gridEX, "Delete", "X", 20, 0);
				GridEX_Helper.AddButton(gridEX, "Edit", "Edit", 40, 1);
				gridEX.FrozenColumns = 2;
				ctrlGrdBar1_Load(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailGridComboBind()
	{
		try
		{
			GridEX_Helper.ComboBind(grd.RootTable.Columns["ItemConditionId"], dtCondition, "Id", "ItemCondition", "Item Condition", Constants.InventoryConstants.JobLotConstant);
			GridEX_Helper.ComboBind(grd.RootTable.Columns["JobLotId"], dtJobLot, "Id", "JobLot", "Job Lot", Constants.InventoryConstants.JobLotConstant);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteDetailRow(r);
					EnableOrDisableOrderCategory();
				}
				if (e.Column.Key == "Edit")
				{
					grd_DoubleClick(null, null);
				}
				grd.UpdateData();
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
					dtGrid.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				else
				{
					dtGrid.Rows[r.RowIndex].Delete();
					r.Delete();
				}
			}
			TotalCommissionAmount();
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count == 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grd.CurrentColumn.Key == "Delete")
				{
					DeleteDetailRow(r);
				}
				if (grd.CurrentColumn.Key == "Edit")
				{
					grd_DoubleClick(null, null);
				}
				grd.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteDetailRow(r);
				grd.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		try
		{
			if (Approved)
			{
				throw new Exception("Record cannot be Delete because Record has approved");
			}
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					InvPurchaseInvoice.RemoveByID(new InvPurchaseInvoice
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocumentTypeId = DocumentTypeId,
						Id = Id,
						EntryUser = UserAccount.ID
					});
					MessageBox.Show("Delete Record Successfully");
					Reset();
				}
				return;
			}
			throw new Exception("RecordId Not Found.....");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Insert()
	{
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected O, but got Unknown
		//IL_0453: Unknown result type (might be due to invalid IL or missing references)
		//IL_045a: Expected O, but got Unknown
		try
		{
			if (grd.GetRows().Count() == 0)
			{
				throw new Exception("Detail Record Not Found");
			}
			if (!FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
			{
				(txtdocno, "Doc No", FormHelper.ValidType.INT),
				((Control)(object)CmbSupplierName, "Customer", FormHelper.ValidType.INT),
				((Control)(object)CmbOrderCategory, "Order Category", FormHelper.ValidType.INT),
				((Control)(object)CmbDeliveryTerm, "Delivery Term", FormHelper.ValidType.INT),
				((Control)(object)CmbPaymentTerm, "Payment Term", FormHelper.ValidType.INT)
			}))
			{
				return;
			}
			if (Conversion.ToInt(CmbPaymentTerm.Value) == 2 && Conversion.ToInt((object)txtduedays.Text) == 0)
			{
				MessageBox.Show("Due Days Field is Required");
				txtduedays.Focus();
			}
			else if (Conversion.ToDouble((object)txtCommissionAmount.Text) > 0.0 && Conversion.ToInt(CmbCommissionAgent.Value) == 0)
			{
				MessageBox.Show("Please Select Commission Agent Account First");
			}
			else
			{
				if (!FormHelper.ConfirmAction((Id == 0) ? "Are you sure to Save" : "Are you sure to Update"))
				{
					return;
				}
				SaleOrder obj = new SaleOrder();
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.BranchesId = UserAccount.BranchesId;
				obj.ProjectsId = 0;
				obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				obj.DocumentTypeId = DocumentTypeId;
				obj.EntryDate = DateTime.Now;
				obj.ModifyDate = DateTime.Now;
				obj.DeliveryStartDate = DateTime.Now;
				obj.OrderExpiryDate = DateTime.Now;
				obj.PostDate = DateTime.Now;
				obj.EntryUser = UserAccount.ID;
				obj.ModifyUser = UserAccount.ID;
				obj.OrderStatus = "Open";
				obj.Id = Id;
				obj.DocNo = Conversion.ToInt((object)txtdocno.Text);
				obj.DocDate = DocDate.Value;
				obj.DeliveryDays = 1;
				obj.DeliveryStartDate = DocDate.Value.AddDays(1.0);
				obj.OrderSupCustId = Conversion.ToInt(CmbSupplierName.Value);
				obj.SupplierRefNo = txtManualbillno.Text.Trim();
				bool hasTax = dtGrid.AsEnumerable().Any((DataRow x) => Conversion.ToInt(x["TaxNameId"]) > 0);
				obj.OrderTaxable = (hasTax ? "true" : "false");
				obj.OrderDueDate = duedate.Value;
				obj.OrderCatagoryId = Conversion.ToInt(CmbOrderCategory.Value);
				obj.DeliveryTerm = Conversion.ToString((object)((Control)(object)CmbDeliveryTerm).Text);
				obj.PaymentTermsId = Conversion.ToInt(CmbPaymentTerm.Value);
				obj.OrderDueDays = Conversion.ToInt((object)txtduedays.Text);
				obj.RemarksHeader = txtremarks.Text.Trim();
				obj.BrokerAgentSupCustId = Conversion.ToInt(CmbCommissionAgent.Value);
				obj.CommissionType = ((Control)(object)CmbCommType).Text.Trim();
				obj.UomScheduleIdCmRate = Conversion.ToInt((object)((Control)(object)CmbCommUom).Text);
				obj.CommRate = Conversion.ToDouble((object)txtCommRate.Text);
				obj.CommAmount = Conversion.ToDouble((object)txtCommissionAmount.Text);
				obj.CommissionRemarks = txtCommissionRemarks.Text.Trim();
				obj.OrderAmount = Conversion.ToDouble((object)txtOrderAmount.Text.Trim());
				obj.CurrencyId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Base Currency"));
				obj.ExchangeRate = Conversion.ToDouble((object)GlobalVariables_Helper.GetConfigValueFromGlobal("BaseCurrencyRate"));
				obj.FcyAmount = obj.ExchangeRate * obj.OrderAmount;
				obj.SaleOrderDetailList = new List<SaleOrderDetail>();
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					SaleOrderDetail vd = new SaleOrderDetail();
					vd.Id = ((Id != 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
					vd.ActionTypeId = ((vd.Id <= 0) ? 1 : 2);
					FillDetailListCommonForInsertAndDelete(vd, r);
					FormHelper.ValidateField(vd.OrderItemId, "Item", r.RowIndex);
					FormHelper.ValidateField(vd.OrderItemUOMId, "Pack Uom", r.RowIndex);
					FormHelper.ValidateField(vd.JobLotId, "JobLot", r.RowIndex);
					FormHelper.ValidateField(vd.OrderItemQty, "Qty", r.RowIndex);
					FormHelper.ValidateField(vd.OrderItemRate, "Rate", r.RowIndex);
					FormHelper.ValidateField(vd.OrderItemRateUOMId, "Rate Uom", r.RowIndex);
					FormHelper.ValidateField(vd.Amount, "Amount", r.RowIndex);
					if (vd.TaxNameId > 0 || vd.TaxPercent > 0.0 || vd.TaxAmount > 0.0)
					{
						FormHelper.ValidateField(vd.TaxNameId, "Tax Name", r.RowIndex);
						FormHelper.ValidateField(vd.TaxPercent, "Tax Percent", r.RowIndex);
						FormHelper.ValidateField(vd.TaxAmount, "Tax Amount", r.RowIndex);
					}
					obj.SaleOrderDetailList.Add(vd);
				}
				obj.AttachmentsList = new List<DMSAttachments>();
				obj.DeleteAttachmentsList = new List<DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<SaleOrder>(obj, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
				obj.AttachmentsValues = AttachmentsValues;
				obj.CustomAttachmentsValues = CustomAttachmentsValues;
				int sucess = SaleOrder.Save(obj);
				if (Id == 0)
				{
					MessageBox.Show("Record Saved Successfully [" + obj.DocNo + "] ");
				}
				else
				{
					MessageBox.Show("Record Update Successfully [" + obj.DocNo + "] ");
					FormHelper.DeleteAllAttachmentsFromDb(AT, Id, DocumentTypeId, base.Name);
				}
				Reset();
				if (ChkPrintslip.Checked)
				{
					CommonServices.SaleInvoiceStorePMSlip(sucess, DocumentTypeId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void saveToolStripButton_Click(object sender, EventArgs e)
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

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (Approved)
			{
				throw new Exception("Record Not Update because Record has approved");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnSaveAs_Click(object sender, EventArgs e)
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

	public void ReadById(int ID)
	{
		try
		{
			Reset();
			Id = ID;
			SaleOrder Or = SaleOrder.GetByID(Id);
			if (Or == null || Or.SaleOrderDetailList == null)
			{
				MessageBox.Show("No Record Found");
				return;
			}
			tabControl1.SelectedIndex = 0;
			btnSave.Visible = false;
			BtnSaveAs.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
			txtdocno.Text = Or.DocNo.ToString();
			DocDate.Value = Or.DocDate;
			CmbSupplierName.Value = Or.OrderSupCustId;
			txtManualbillno.Text = Or.SupplierRefNo;
			CmbOrderCategory.Value = Or.OrderCatagoryId;
			((Control)(object)CmbDeliveryTerm).Text = Conversion.ToString((object)Or.DeliveryTerm);
			CmbPaymentTerm.Value = Conversion.ToInt((object)Or.PaymentTermsId);
			txtduedays.Text = Conversion.ToString((object)Or.OrderDueDays);
			duedate.Value = Or.OrderDueDate;
			txtOrderAmount.Text = Conversion.ToString((object)Or.OrderAmount);
			txtremarks.Text = Conversion.ToString((object)Or.RemarksHeader);
			CmbCommissionAgent.Value = Or.BrokerAgentSupCustId;
			((Control)(object)CmbCommType).Text = Conversion.ToString((object)Or.CommissionType);
			txtCommRate.Text = Conversion.ToString((object)Or.CommRate);
			((Control)(object)CmbCommUom).Text = Conversion.ToString((object)Or.UomScheduleIdCmRate);
			txtCommissionAmount.Text = Conversion.ToString((object)Or.CommAmount);
			txtCommissionRemarks.Text = Conversion.ToString((object)Or.CommissionRemarks);
			Approved = Or.IsAproved;
			AttachmentsValues = Or.AttachmentsValues;
			CustomAttachmentsValues = Or.CustomAttachmentsValues;
			dtGrid.Rows.Clear();
			FillDetailFromListCommonForReadById(Or, dtGrid);
			BindGrids();
			ItemDtsFillFromGlobal();
			ItemNameBind();
			EnableOrDisableOrderCategory();
			CalculateOrderAmount();
			FormHelper.LoadAttachmentsForObject(AT, Id, base.Name);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailListCommonForInsertAndDelete(SaleOrderDetail pd, GridEXRow r)
	{
		pd.OrderItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
		pd.ItemCodeNew = Conversion.ToString(r.Cells["ItemCode"].Value);
		pd.ItemName = Conversion.ToString(r.Cells["Item"].Value);
		pd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
		pd.JobLotDescription = Conversion.ToString((object)r.Cells["JobLotId"].Text);
		pd.ItemConditionId = Conversion.ToInt(r.Cells["ItemConditionId"].Value);
		pd.ItemCondition = Conversion.ToString((object)r.Cells["ItemConditionId"].Text);
		pd.OrderItemUOMId = Conversion.ToInt(r.Cells["PackUOMId"].Value);
		pd.UOMCode = Conversion.ToString(r.Cells["PackUOM"].Value);
		double qty = (pd.OrderItemQty = Conversion.ToDouble(r.Cells["ItemQty"].Value));
		pd.NetWeight = qty;
		pd.SecondaryUomId = Conversion.ToInt(r.Cells["SecondaryUomId"].Value);
		pd.SecondaryUomCode = Conversion.ToString((object)r.Cells["SecondaryUom"].Text);
		pd.SecondaryUomQty = Conversion.ToDecimal(r.Cells["SecondaryUomQty"].Value);
		pd.PerItemSecondaryUomQty = Conversion.ToDouble(r.Cells["PerItemWeight"].Value);
		pd.SecondaryUomItemRate = Conversion.ToDecimal(r.Cells["SecondaryUomItemRate"].Value);
		pd.OrderItemRate = Conversion.ToDouble(r.Cells["ItemRate"].Value);
		pd.OrderItemRateUOMId = Conversion.ToInt(r.Cells["RateUOMId"].Value);
		pd.RateUom = Conversion.ToString(r.Cells["RateUOM"].Value);
		pd.EquivalentRate = Conversion.ToDouble(r.Cells["RateUOM"].Value);
		pd.Amount = Conversion.ToDouble(r.Cells["ItemAmount"].Value);
		pd.TaxNameId = Conversion.ToInt(r.Cells["TaxNameId"].Value);
		pd.TaxName = Conversion.ToString(r.Cells["TaxName"].Value);
		pd.TaxPercent = Conversion.ToDouble(r.Cells["TaxPercent"].Value);
		pd.TaxAmount = Conversion.ToDouble(r.Cells["TaxAmount"].Value);
		pd.TotalAmount = Conversion.ToDouble(r.Cells["TotalAmount"].Value);
	}

	private void FillDetailFromListCommonForReadById(SaleOrder inv, DataTable dt, bool ExtraColumns = false)
	{
		if (((inv != null) ? inv.SaleOrderDetailList : null) == null || inv.SaleOrderDetailList.Count == 0)
		{
			return;
		}
		foreach (SaleOrderDetail detail in inv.SaleOrderDetailList)
		{
			DataRow dr = dt.NewRow();
			dr["Id"] = detail.Id;
			dr["ItemId"] = detail.OrderItemId;
			dr["ItemCode"] = detail.ItemCodeNew;
			dr["Item"] = detail.ItemName;
			dr["JobLotId"] = detail.JobLotId;
			dr["ItemConditionId"] = detail.ItemConditionId;
			dr["PackUOMId"] = detail.OrderItemUOMId;
			dr["PackUOM"] = detail.UOMCode;
			dr["ItemQty"] = detail.OrderItemQty;
			dr["SecondaryUomId"] = detail.SecondaryUomId;
			dr["SecondaryUom"] = detail.SecondaryUomCode;
			dr["SecondaryUomQty"] = detail.SecondaryUomQty;
			dr["PerItemWeight"] = detail.PerItemSecondaryUomQty;
			dr["SecondaryUomItemRate"] = detail.SecondaryUomItemRate;
			dr["ItemRate"] = detail.OrderItemRate;
			dr["RateUOMId"] = detail.OrderItemRateUOMId;
			dr["RateUOM"] = detail.RateUom;
			dr["ItemAmount"] = detail.Amount;
			dr["TaxNameId"] = detail.TaxNameId;
			dr["TaxName"] = detail.TaxName;
			dr["TaxPercent"] = detail.TaxPercent;
			dr["TaxAmount"] = detail.TaxAmount;
			dr["TotalAmount"] = detail.Amount + detail.TaxAmount;
			if (ExtraColumns)
			{
				dr["ItemCondition"] = detail.ItemCondition;
				dr["JobLot"] = detail.JobLotDescription;
			}
			dr["Remarks"] = detail.OrderRemarks;
			dt.Rows.Add(dr);
		}
	}

	private void Reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			Id = 0;
			((Control)(object)CmbOrderCategory).Enabled = true;
			Approved = false;
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			BtnSaveAs.Visible = false;
			_isUpdatingRates = false;
			_isUpdatingSecondary = false;
			((Control)(object)CmbSupplierName).Text = string.Empty;
			txtManualbillno.Text = string.Empty;
			txtremarks.Text = string.Empty;
			((Control)(object)CmbPaymentTerm).Text = string.Empty;
			txtduedays.Text = "0";
			((Control)(object)CmbCommissionAgent).Text = string.Empty;
			((Control)(object)CmbCommType).Text = string.Empty;
			txtCommissionRemarks.Text = string.Empty;
			txtCommRate.Text = string.Empty;
			txtCommissionAmount.Text = string.Empty;
			((Control)(object)CmbCommUom).Text = string.Empty;
			txtOrderAmount.Text = string.Empty;
			ResetDetail();
			dtGrid.Rows.Clear();
			BindGrids();
			txtdocno.Text = DocumentNoDbCall();
			txtBranchSrNo.Text = BranchSrNoDbCall();
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
			updateDetailIndex = -1;
			((Control)(object)CmbJobLot).Text = string.Empty;
			((Control)(object)CmbItemName).Text = string.Empty;
			((Control)(object)CmbPackUOM).Text = string.Empty;
			txtQty.Text = string.Empty;
			((Control)(object)CmbSecondaryUom).Text = string.Empty;
			txtSecondaryUomQty.Text = string.Empty;
			txtPerItemWeight.Text = string.Empty;
			txtSecondaryUomRate.Text = string.Empty;
			txtRate.Text = string.Empty;
			((Control)(object)CmbRateUOM).Text = string.Empty;
			txtAmount.Text = string.Empty;
			((Control)(object)CmbTaxType).Text = string.Empty;
			((UltraGridBase)CmbTaxType).DataSource = null;
			txtTaxPercent.Text = "";
			txtTaxAmount.Text = "";
			txtTotalAmount.Text = "";
			if (ChkResetOnSave.Checked)
			{
				OptionResetFields();
			}
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancel.Visible = false;
			CmbItemName.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OptionResetFields()
	{
		((Control)(object)CmbItemName).Text = string.Empty;
		((Control)(object)CmbJobLot).Text = string.Empty;
		CmbItemName.Focus();
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
			List<string> allServices = new List<string> { "AccountsWithCustomGroupId", "PaymentTerm", "UomSchedule", "SupplierCustomerLists", "VendorsAndCustomersForTransporters", "WarehousesWithBranches", "AllItems", "JobLot", "ItemConditions" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			GetConfigurationsFromGlobal();
			SupplierDtFillFromGlobal();
			BindCustomer();
			OrderCatagoryfill();
			DeliveryTermFill();
			PaymentTermBindFromGlobal();
			CommonBindings.JoblotBindFromGlobal(dtJobLot, CmbJobLot);
			CommonBindings.ItemConditionBindFromGlobal(dtCondition, CmbItemCondition);
			ItemDtsFillFromGlobal();
			ItemNameBind();
			GetConfigurationsFromGlobalandBind();
			DetailGridComboBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvfrmInvSaleInvoiceDirectPackingMaterial_FormClosing(object sender, FormClosingEventArgs e)
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

	private void CalculateAmount()
	{
		UltraCombo cmbRateUOM = CmbRateUOM;
		double rateUOM = ((((cmbRateUOM != null) ? ((UltraDropDownBase)cmbRateUOM).SelectedRow : null) != null) ? Conversion.ToDouble(((UltraDropDownBase)CmbRateUOM).SelectedRow.Cells[2].Value) : 0.0);
		double rate = Conversion.ToDouble((object)txtRate.Text);
		double qty = Conversion.ToDouble((object)txtQty.Text);
		double itemAmount = ((qty > 0.0 && rateUOM > 0.0 && rate > 0.0) ? (qty / rateUOM * rate) : 0.0);
		txtAmount.Text = Math.Round(itemAmount, 3).ToString();
		CalculateTaxAmount();
	}

	private void CalculateTaxAmount()
	{
		double num = Conversion.ToDouble((object)txtAmount.Text);
		double taxPercent = Conversion.ToDouble((object)txtTaxPercent.Text);
		UltraCombo cmbTaxType = CmbTaxType;
		taxPercent = ((((cmbTaxType != null) ? ((UltraDropDownBase)cmbTaxType).SelectedRow : null) == null) ? 0.0 : Conversion.ToDouble(((UltraDropDownBase)CmbTaxType).SelectedRow.Cells[2].Value));
		txtTaxPercent.Text = taxPercent.ToString("#,##.###");
		double taxAmount = num * taxPercent / 100.0;
		double totalAmount = num + taxAmount;
		txtTaxAmount.Text = Math.Round(taxAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
		txtTotalAmount.Text = Math.Round(totalAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
	}

	private void comPackUOM_Leave(object sender, EventArgs e)
	{
		try
		{
			CalculateAmount();
			AvailableStockGetByItem();
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
			CalculateAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtSecondaryUomQty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			SecondaryUomQtyAndPerItemSecondaryQtyCalculations();
			ItemRateAndSecondaryRateCalculations();
			CalculateAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtSecondaryUomRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			ItemRateAndSecondaryRateCalculations();
			CalculateAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPerItemWeight_TextChanged(object sender, EventArgs e)
	{
		SecondaryUomQtyAndPerItemSecondaryQtyCalculations();
	}

	private void SecondaryUomQtyAndPerItemSecondaryQtyCalculations()
	{
		if (_isUpdatingSecondary)
		{
			return;
		}
		try
		{
			_isUpdatingSecondary = true;
			double perItem = Conversion.ToDouble((object)txtPerItemWeight.Text);
			double totalSecondary = Conversion.ToDouble((object)txtSecondaryUomQty.Text);
			double qty = Conversion.ToDouble((object)txtQty.Text);
			if (qty <= 0.0)
			{
				txtPerItemWeight.Text = "0";
				txtSecondaryUomQty.Text = "0";
				return;
			}
			string active = base.ActiveControl?.AccessibleName;
			if (active == txtPerItemWeight.AccessibleName)
			{
				double result = Math.Round(perItem * qty, 3);
				txtSecondaryUomQty.Text = result.ToString("#,##0.###");
				CalculateByPerItemSecondaryQty = true;
				CalculateBySecondaryUomQty = false;
			}
			else if (active == txtSecondaryUomQty.AccessibleName)
			{
				double result2 = Math.Round(totalSecondary / qty, 3);
				txtPerItemWeight.Text = result2.ToString("#,##0.###");
				CalculateByPerItemSecondaryQty = false;
				CalculateBySecondaryUomQty = true;
			}
			else if (CalculateBySecondaryUomQty)
			{
				double result3 = Math.Round(totalSecondary / qty, 3);
				txtPerItemWeight.Text = result3.ToString("#,##0.###");
			}
			else if (CalculateByPerItemSecondaryQty)
			{
				double result4 = Math.Round(perItem * qty, 3);
				txtSecondaryUomQty.Text = result4.ToString("#,##0.###");
			}
		}
		finally
		{
			_isUpdatingSecondary = false;
		}
	}

	private void ItemRateAndSecondaryRateCalculations()
	{
		if (_isUpdatingRates)
		{
			return;
		}
		try
		{
			_isUpdatingRates = true;
			double itemRate = Conversion.ToDouble((object)txtRate.Text);
			double secondaryRate = Conversion.ToDouble((object)txtSecondaryUomRate.Text);
			double secondaryQty = Conversion.ToDouble((object)txtSecondaryUomQty.Text);
			double qty = Conversion.ToDouble((object)txtQty.Text);
			if (qty <= 0.0 || secondaryQty <= 0.0)
			{
				txtRate.Text = "0";
				txtSecondaryUomRate.Text = "0";
				return;
			}
			string active = base.ActiveControl?.AccessibleName;
			if (active == txtRate.AccessibleName)
			{
				double result = Math.Round(itemRate * (qty / secondaryQty), 3);
				txtSecondaryUomRate.Text = result.ToString("#,##0.###");
				CalculateByItemRate = true;
				CalculateBySecondaryUomRate = false;
			}
			else if (active == txtSecondaryUomRate.AccessibleName)
			{
				double result2 = Math.Round(secondaryRate * secondaryQty / qty, 3);
				txtRate.Text = result2.ToString("#,##0.###");
				CalculateByItemRate = false;
				CalculateBySecondaryUomRate = true;
			}
			else if (CalculateBySecondaryUomRate)
			{
				double result3 = Math.Round(secondaryRate * secondaryQty / qty, 3);
				txtRate.Text = result3.ToString("#,##0.###");
			}
			else if (CalculateByItemRate)
			{
				double result4 = Math.Round(itemRate * (qty / secondaryQty), 3);
				txtSecondaryUomRate.Text = result4.ToString("#,##0.###");
			}
		}
		finally
		{
			_isUpdatingRates = false;
		}
	}

	private void txtRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			ItemRateAndSecondaryRateCalculations();
			CalculateAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comRateUOM_Leave(object sender, EventArgs e)
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

	private void txtTaxPercent_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateTaxAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSupplierName_Leave(object sender, EventArgs e)
	{
		try
		{
			if (dtSupplier.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtSupplier.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtSupplier.Rows[i]["Id"]) == Conversion.ToInt(CmbSupplierName.Value))
				{
					txtSupplierGLId.Text = dtSupplier.Rows[i]["GlAccountId"].ToString();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalCommissionAmount()
	{
		try
		{
			if (txtCommRate.Text != string.Empty && ((Control)(object)CmbCommType).Text == "Flat")
			{
				double CommRate3 = Conversion.ToDouble((object)txtCommRate.Text.Trim());
				txtCommissionAmount.Text = CommRate3.ToString();
			}
			if (txtCommRate.Text != string.Empty && (((Control)(object)CmbCommType).Text == "Percent" || ((Control)(object)CmbCommType).Text == "Percentage"))
			{
				double CommRate2 = Conversion.ToDouble((object)txtCommRate.Text.Trim());
				double CommAmt2 = GridEX_Helper.GetColumnSum(grd, "ItemAmount") * CommRate2 / 100.0;
				txtCommissionAmount.Text = Math.Round(CommAmt2, 3).ToString();
			}
			if (txtCommRate.Text != string.Empty && ((Control)(object)CmbCommType).Text == "Comm Weight")
			{
				double CommRate = Conversion.ToDouble((object)txtCommRate.Text.Trim());
				double columnSum = GridEX_Helper.GetColumnSum(grd, "NetBillWeight");
				double UOM = Conversion.ToDouble((object)((Control)(object)CmbCommUom).Text.Trim());
				double CommAmt = columnSum / UOM * CommRate;
				txtCommissionAmount.Text = Math.Round(CommAmt, 3).ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combcommAgent_Leave(object sender, EventArgs e)
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

	private void txtcommrate_TextChanged(object sender, EventArgs e)
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

	private void combcommUOM_Leave_1(object sender, EventArgs e)
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

	private void combCommType_Leave_1(object sender, EventArgs e)
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

	private void AvailableStockGetByItem()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Expected O, but got Unknown
		try
		{
			lblBalance.Text = "0";
			int ItemId = Conversion.ToInt(CmbItemName.Value);
			if (ItemId != 0)
			{
				DataTable dt = GetAvgRatesAndStockInHand.GetAvgRateQtyAndStockInHand(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					ItemId = ItemId,
					DocDate = Conversion.ToDateTime((object)DocDate.Value),
					JobLotId = Conversion.ToInt(CmbJobLot.Value),
					ItemConditionId = Conversion.ToInt(CmbItemCondition.Value),
					RecId = Id,
					DocumentTypeId = ((Id > 0) ? DocumentTypeId : 0)
				});
				if (dt.Rows.Count > 0 && dt != null)
				{
					double ItemQty = Math.Round(Conversion.ToDouble(dt.Rows[0]["QtyInHand"]), 2);
					lblBalance.Text = Conversion.ToString((object)ItemQty);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comjobLot_Leave(object sender, EventArgs e)
	{
		AvailableStockGetByItem();
	}

	private void CmbItemCondition_Leave(object sender, EventArgs e)
	{
		AvailableStockGetByItem();
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 1)
			{
				FromDateHistory.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryBranchComboFill()
	{
		try
		{
			dtBranch.Clear();
			((UltraGridBase)cmbBranchName).DataSource = null;
			if (BranchWiseSaleOrder)
			{
				dtBranch.Rows.Add(UserAccount.BranchesId, UserAccount.BranchName);
			}
			else
			{
				DataTable dt = SaleOrder.GetBranchesAllocatedToUserFromSaleOrder(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, 81);
				if (dt.Rows.Count > 0)
				{
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						dtBranch.Rows.Add(dt.Rows[i]["BranchId"], dt.Rows[i]["BranchName"]);
					}
				}
			}
			if (dtBranch.Rows.Count > 0)
			{
				DDL.BindDDL(dtBranch, cmbBranchName, "Id", "BranchName", "Branch Name", ZeroIndex: true);
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

	private DataTable HistoryComboDbCall(bool NotValidateBranch = false)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.AppId = UserAccount.AppId;
			obj.UserId = UserAccount.ID;
			obj.DocumentTypeIds = DocumentTypeId.ToString();
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
			}
			else if (!NotValidateBranch)
			{
				cmbBranchName.Focus();
				throw new Exception("Select branch first");
			}
			return SaleOrder.GetDataForDropDownFromSaleOrder(obj);
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
			DataTable dtST = new DataTable();
			dtST.Columns.Add("Id", typeof(int));
			dtST.Columns.Add("Name");
			if (dtcombo.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtcombo.Rows.Count; i++)
			{
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "Customer")
				{
					dtST.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbCustomerHistory, dtST, "Id", "Name", "Customer Name", AllColumns: true, Conversion.ToInt(CmbCustomerHistory.Value), insertDefaultRow: true, null, null, ActivateRow: false);
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
			FromDateHistory.Value = DateTime.Now;
			ToDateHistory.Value = DateTime.Now;
			FromDocNoHistory.Clear();
			ToDocNoHistory.Clear();
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			grdHistory.DataSource = null;
			grdDetail.DataSource = null;
			drdocdate.Checked = true;
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
			HistoryBranchComboFill();
			HistoryComboBind(HistoryComboDbCall());
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
			ReportsParameters PI = new ReportsParameters();
			PI.OrganizationId = UserAccount.OrganizationId;
			PI.CompanyId = UserAccount.CompanyId;
			PI.DocumentTypeId = DocumentTypeId;
			PI.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			PI.CanViewAllRecord = formrights.DoHaveCanViewAllRecordRights;
			if (!PI.CanViewAllRecord)
			{
				PI.EntryUser = UserAccount.ID;
			}
			PI.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					PI.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					PI.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					PI.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					PI.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					PI.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					PI.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					PI.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					PI.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			PI.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			PI.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			PI.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
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
				PI.BranchesIds = BranchIds;
				DataTable dtGetAll = SaleOrder.GetHisoty(PI);
				if (dtGetAll.Rows.Count > 0)
				{
					DataTable dthistory = new DataTable();
					dthistory.Columns.Clear();
					dthistory.Columns.Add("Id", typeof(int));
					dthistory.Columns.Add("BranchSrNo", typeof(int));
					dthistory.Columns.Add("BranchName");
					dthistory.Columns.Add("DocNo", typeof(int));
					dthistory.Columns.Add("DocDate", typeof(DateTime));
					dthistory.Columns.Add("ManualRefNo");
					dthistory.Columns.Add("CustomerName");
					dthistory.Columns.Add("OrderAmount", typeof(double));
					dthistory.Columns.Add("EntryDate", typeof(DateTime));
					dthistory.Columns.Add("EntryUser", typeof(string));
					dthistory.Columns.Add("ModifyUser");
					dthistory.Columns.Add("ModifyDate", typeof(DateTime));
					dthistory.Columns.Add("ApprovedUser", typeof(string));
					dthistory.Columns.Add("ApprovedDate", typeof(DateTime));
					dthistory.Columns.Add("Remarks");
					dthistory.Columns.Add("NoOfAttachments", typeof(int));
					dthistory.Clear();
					for (int i = 0; i < dtGetAll.Rows.Count; i++)
					{
						dthistory.Rows.Add(dtGetAll.Rows[i]["Id"], dtGetAll.Rows[i]["BranchSrNo"], dtGetAll.Rows[i]["BranchName"], dtGetAll.Rows[i]["DocNo"], dtGetAll.Rows[i]["DocDate"], dtGetAll.Rows[i]["SupplierRefNo"], dtGetAll.Rows[i]["CustomerName"], dtGetAll.Rows[i]["OrderAmount"], dtGetAll.Rows[i]["EntryDate"], dtGetAll.Rows[i]["UserName"], dtGetAll.Rows[i]["ModifyUserName"], dtGetAll.Rows[i]["ModifyDate"], dtGetAll.Rows[i]["ApprovedUser"], dtGetAll.Rows[i]["PostDate"], dtGetAll.Rows[i]["RemarksHeader"], dtGetAll.Rows[i]["NoOfAttachments"]);
					}
					grdHistory.DataSource = dthistory;
					grdHistory.RetrieveStructure();
					HistoryGridSettings();
				}
				else
				{
					grdHistory.ClearStructure();
				}
				return;
			}
			cmbBranchName.Focus();
			throw new Exception("Select branch first");
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
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["DocDate"].FormatString = "dd-MM-yyyy";
			grdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			GridEX_Helper.GridWrappingAndColumnSettings(grdHistory, 2, 3);
			grdHistory.RootTable.Columns["BranchSrNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdHistory.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grdHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant + 10;
			grdHistory.RootTable.Columns["ManualRefNo"].Width = Constants.InventoryConstants.ManualBillNo;
			grdHistory.RootTable.Columns["CustomerName"].Width = Constants.InventoryConstants.SupplierName + 20;
			grdHistory.RootTable.Columns["OrderAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			grdHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			grdHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdHistory.RootTable.Columns["ApprovedUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			grdHistory.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdHistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			GridEX_Helper.AddButton(grdHistory, "View", "Slip", 35, 0);
			GridEX_Helper.AddButton(grdHistory, "Edit", "Edit", 35, 1);
			GridEX_Helper.AddButton(grdHistory, "SaveAs", "Save As", 60, 2);
			grdHistory.FrozenColumns = 3;
			GridEX_Helper.AddButton(grdHistory, "AddAttachment", "Add Attachment", 120);
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByScreenName(Conversion.ToInt(item.Cells["Id"].Value), base.Name);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int currid = Conversion.ToInt(item.Cells["Id"].Value);
			if (e.Column.Key == "Edit")
			{
				ReadById(currid);
			}
			else if (e.Column.Key == "View")
			{
				CommonServices.SaleOrderSlip_90(currid, DocumentTypeId);
			}
			if (e.Column.Key == "SaveAs")
			{
				Reset();
				Id = Conversion.ToInt(item.Cells["Id"].Value);
				ReadById(Id);
				btnSave.Visible = false;
				btnUpdate.Visible = false;
				btnDelete.Visible = false;
				BtnSaveAs.Visible = true;
				if (grd.GetRows().Length != 0)
				{
					grd.UpdateData();
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow obj in rows)
					{
						obj.BeginEdit();
						obj.Cells["Id"].Value = 0;
						obj.EndEdit();
					}
					grd.UpdateData();
				}
				AT.lst.Clear();
			}
			else if (e.Column.Key == "AddAttachment")
			{
				AttachmentAddingFromHistory attachmentAddingFromHistory = new AttachmentAddingFromHistory(UserAccount, currid, DocumentTypeId, base.Name);
				attachmentAddingFromHistory.BringToFront();
				attachmentAddingFromHistory.ShowDialog();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			Id = Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value);
			ReadById(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_SelectionChanged(object sender, EventArgs e)
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
			SaleOrder Obj = SaleOrder.GetByID(Id);
			if (Obj != null && Obj.SaleOrderDetailList.Count == 0)
			{
				grdDetail.ClearStructure();
				return;
			}
			DataTable dtDetailHistory = dtGrid.Clone();
			dtDetailHistory.Columns.Add("ItemCondition");
			dtDetailHistory.Columns.Add("JobLot");
			FillDetailFromListCommonForReadById(Obj, dtDetailHistory);
			grdDetail.DataSource = dtDetailHistory;
			grdDetail.RetrieveStructure();
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
			grdSettings(grdDetail);
			grdDetail.RootTable.Columns["ItemConditionId"].Visible = false;
			grdDetail.RootTable.Columns["JobLotId"].Visible = false;
			grdDetail.RootTable.Columns["ItemCondition"].Position = grdDetail.RootTable.Columns["ItemConditionId"].Position + 1;
			grdDetail.RootTable.Columns["JobLot"].Position = grdDetail.RootTable.Columns["JobLotId"].Position + 1;
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

	private void btnSlip_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.SaleOrderSlip_90(Id, DocumentTypeId);
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdHistory).Name);
			if (fs != null)
			{
				grdHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void DocDate_Leave(object sender, EventArgs e)
	{
		try
		{
			if (grd.RowCount <= 0)
			{
				return;
			}
			string ItemIds = "";
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				ItemIds = ItemIds + "," + r.Cells["ItemId"].Value;
			}
			DataTable dt = CommonServices.GetTaxScheduleDetailbyItemIds(ItemIds, DocDate.Value);
			rows = grd.GetRows();
			foreach (GridEXRow item in rows)
			{
				item.BeginEdit();
				double TaxPercent = 0.0;
				double ItemAmount = 0.0;
				double TaxAmount = 0.0;
				double TotalAmount = 0.0;
				bool flag = false;
				ItemAmount = Conversion.ToDouble(item.Cells["ItemAmount"].Value);
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToInt(dt.Rows[i]["ItemId"]) == Conversion.ToInt(item.Cells["ItemId"].Value))
					{
						flag = true;
						item.Cells["TaxNameId"].Value = Conversion.ToInt(dt.Rows[i]["TaxNameId"]);
						item.Cells["TaxName"].Value = Conversion.ToString(dt.Rows[i]["TaxName"]);
						TaxPercent = Conversion.ToDouble(dt.Rows[i]["TaxPercent"]);
						item.Cells["TaxPercent"].Value = TaxPercent;
						if (ItemAmount > 0.0 && TaxPercent > 0.0)
						{
							TaxAmount = ItemAmount * TaxPercent / 100.0;
						}
						item.Cells["TaxAmount"].Value = TaxAmount;
						TotalAmount = ItemAmount + TaxAmount;
						item.Cells["TotalAmount"].Value = TotalAmount;
					}
				}
				if (!flag)
				{
					item.Cells["TaxNameId"].Value = 0;
					item.Cells["TaxName"].Value = 0;
					item.Cells["TaxPercent"].Value = 0;
					item.Cells["TaxAmount"].Value = 0;
					item.Cells["TotalAmount"].Value = ItemAmount;
				}
				item.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtduedays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DueDateCalculate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DueDateCalculate()
	{
		try
		{
			if (txtduedays.Text.Trim() != string.Empty && txtduedays.Text != null)
			{
				duedate.Value = DocDate.Value.AddDays(Conversion.ToDouble((object)txtduedays.Text.Trim()));
			}
			else
			{
				duedate.Value = DateTime.Now;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocDate_ValueChanged(object sender, EventArgs e)
	{
		DueDateCalculate();
		AvailableStockGetByItem();
	}

	private void InvfrmPurchasedirectInvoice_KeyDown_1(object sender, KeyEventArgs e)
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
					if (formrights.DoHaveCanDelete && btnDelete.Enabled)
					{
						btnDelete_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.S)
				{
					if (btnSave.Visible && btnSave.Enabled)
					{
						saveToolStripButton_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.U)
				{
					if (btnUpdate.Visible && btnUpdate.Enabled)
					{
						btnUpdate_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.P)
				{
					if (btnSlip.Visible && btnSlip.Enabled)
					{
						btnSlip_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.N)
				{
					btnNew_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.F5)
				{
					DocDate.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.F10)
				{
					AT.Show();
				}
				else if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grd).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					DocDate.Focus();
				}
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdDetail).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.S)
				{
					btnShowHistory_Click(null, null);
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

	private void grdHistory_KeyDown(object sender, KeyEventArgs e)
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
					if (!formrights.DoHaveUpdateRights)
					{
						throw new Exception("you don't have update rights...");
					}
					ReadById(Conversion.ToInt((object)CurrId));
				}
				else if (grdHistory.CurrentColumn.Key == "Print")
				{
					if (!formrights.DoHavePrintRights)
					{
						throw new Exception("you don't have print rights...");
					}
					CommonServices.SaleInvoiceStorePMSlip(CurrId, DocumentTypeId);
				}
				else if (grdHistory.CurrentColumn.Key == "Voucher")
				{
					if (!formrights.DoHavePrintRights)
					{
						throw new Exception("you don't have print rights...");
					}
					CommonServices.VoucherReport_118(CommonServices.VoucherHeadIdGet(CurrId, DocumentTypeId));
				}
				else if (grdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(CurrId, DocumentTypeId);
				}
				else if (grdHistory.CurrentColumn.Key == "AddAttachment")
				{
					AttachmentAddingFromHistory attachmentAddingFromHistory = new AttachmentAddingFromHistory(UserAccount, CurrId, DocumentTypeId, base.Name);
					attachmentAddingFromHistory.BringToFront();
					attachmentAddingFromHistory.ShowDialog();
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (!formrights.DoHaveUpdateRights)
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

	private void btnIssuanceLoad_Click(object sender, EventArgs e)
	{
		try
		{
			using LoadavailableTransactionsForConversionStore loader = new LoadavailableTransactionsForConversionStore(UserAccount);
			loader.ParentCategoryId = 8;
			loader.ItemConditionId = 4;
			loader.BringToFront();
			loader.ShowDialog();
			LoadDataDetailfromAvailableStock(loader.dtLoader);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadDataDetailfromAvailableStock(DataTable dt)
	{
		try
		{
			if (dt.Rows.Count > 0)
			{
				HashSet<(int, int, int)> existingDetailIds = new HashSet<(int, int, int)>(from r in grd.GetRows()
					select (Conversion.ToInt(r.Cells["ItemId"].Value), Conversion.ToInt(r.Cells["JobLotId"].Value), Conversion.ToInt(r.Cells["ItemConditionId"].Value)));
				foreach (DataRow dr in dt.Rows)
				{
					int itemId = Conversion.ToInt(dr["ItemId"]);
					int itemConditionId = Conversion.ToInt(dr["ItemConditionId"]);
					int jobLotId = Conversion.ToInt(CmbJobLot.Value);
					(int, int, int) key = (itemId, jobLotId, itemConditionId);
					if (existingDetailIds.Add(key))
					{
						DataRow newRow = dtGrid.NewRow();
						IEnumerable<GloablUomSchedule> source = clsGlobalVariables.globalUomSchedule.Where((GloablUomSchedule r) => r.ItemId == itemId && r.BaseRateUom);
						int RateUOMId = source.Select((GloablUomSchedule r) => r.Id).FirstOrDefault();
						string RateUOM = source.Select((GloablUomSchedule r) => r.UOMCode).FirstOrDefault();
						double AvailableStock = Conversion.ToDouble(dr["AvailableStock"]);
						newRow["ItemId"] = itemId;
						newRow["Item"] = dr["ItemName"];
						newRow["ItemCode"] = dr["ItemCode"];
						newRow["JobLotId"] = jobLotId;
						newRow["ItemConditionId"] = itemConditionId;
						newRow["PackUOMId"] = dr["PackUomId"];
						newRow["PackUOM"] = dr["UOMCode"];
						newRow["ItemQty"] = AvailableStock;
						newRow["SecondaryUomId"] = dr["SecondaryUomId"];
						newRow["SecondaryUom"] = dr["SecondaryUom"];
						newRow["SecondaryUomQty"] = dr["AvgSecondaryUomQty"];
						newRow["PerItemWeight"] = ((AvailableStock > 0.0) ? (Conversion.ToDouble(dr["AvgSecondaryUomQty"]) / AvailableStock) : 0.0);
						newRow["SecondaryUomItemRate"] = 0;
						newRow["ItemRate"] = 0;
						newRow["RateUOMId"] = RateUOMId;
						newRow["RateUOM"] = RateUOM;
						newRow["ItemAmount"] = 0;
						newRow["TaxNameId"] = 0;
						newRow["TaxName"] = "";
						newRow["TaxPercent"] = 0;
						newRow["TaxAmount"] = 0;
						newRow["TotalAmount"] = 0;
						newRow["Remarks"] = "";
						dtGrid.Rows.Add(newRow);
					}
				}
			}
			BindGrids();
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
		//IL_0573: Unknown result type (might be due to invalid IL or missing references)
		//IL_057d: Expected O, but got Unknown
		//IL_05ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f6: Expected O, but got Unknown
		//IL_062e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0638: Expected O, but got Unknown
		//IL_065a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0664: Expected O, but got Unknown
		//IL_0686: Unknown result type (might be due to invalid IL or missing references)
		//IL_0690: Expected O, but got Unknown
		//IL_069c: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a6: Expected O, but got Unknown
		//IL_06c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d2: Expected O, but got Unknown
		//IL_076d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0777: Expected O, but got Unknown
		//IL_07db: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e5: Expected O, but got Unknown
		//IL_07f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fb: Expected O, but got Unknown
		//IL_0807: Unknown result type (might be due to invalid IL or missing references)
		//IL_0811: Expected O, but got Unknown
		//IL_0812: Unknown result type (might be due to invalid IL or missing references)
		//IL_081c: Expected O, but got Unknown
		//IL_086a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0874: Expected O, but got Unknown
		//IL_0875: Unknown result type (might be due to invalid IL or missing references)
		//IL_087f: Expected O, but got Unknown
		//IL_08c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cc: Expected O, but got Unknown
		//IL_09e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ea: Expected O, but got Unknown
		//IL_0a0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a16: Expected O, but got Unknown
		//IL_0a59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a63: Expected O, but got Unknown
		//IL_0ab1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abb: Expected O, but got Unknown
		//IL_0fdd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe7: Expected O, but got Unknown
		//IL_85b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_85bd: Expected O, but got Unknown
		//IL_85e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_85eb: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
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
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
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
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
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
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
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
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel1 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label1 = new System.Windows.Forms.Label();
		this.btnCancel = new System.Windows.Forms.Button();
		this.lblTotalAmount = new System.Windows.Forms.Label();
		this.btnAdd = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.CmbTaxType = new UltraCombo();
		this.txtTaxPercent = new System.Windows.Forms.TextBox();
		this.txtRemarksDetail = new System.Windows.Forms.TextBox();
		this.txtTotalAmount = new System.Windows.Forms.TextBox();
		this.lblTaxTypes = new System.Windows.Forms.Label();
		this.lblTaxPercent = new System.Windows.Forms.Label();
		this.CmbItemCondition = new UltraCombo();
		this.txtTaxAmount = new System.Windows.Forms.TextBox();
		this.label29 = new System.Windows.Forms.Label();
		this.lblTaxAmount = new System.Windows.Forms.Label();
		this.CmbSecondaryUom = new UltraCombo();
		this.rdbtnItemCode = new System.Windows.Forms.RadioButton();
		this.label23 = new System.Windows.Forms.Label();
		this.rdbtnItemName = new System.Windows.Forms.RadioButton();
		this.CmbRateUOM = new UltraCombo();
		this.label17 = new System.Windows.Forms.Label();
		this.CmbItemName = new UltraCombo();
		this.label40 = new System.Windows.Forms.Label();
		this.txtPerItemWeight = new System.Windows.Forms.TextBox();
		this.label32 = new System.Windows.Forms.Label();
		this.CmbJobLot = new UltraCombo();
		this.label18 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.lblBalance = new System.Windows.Forms.TextBox();
		this.txtSecondaryUomQty = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.txtSecondaryUomRate = new System.Windows.Forms.TextBox();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.txtRate = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.label38 = new System.Windows.Forms.Label();
		this.CmbPackUOM = new UltraCombo();
		this.label28 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.txtduedays = new System.Windows.Forms.TextBox();
		this.txtBranchSrNo = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.txtSupplierGLId = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtCommAgentId = new System.Windows.Forms.TextBox();
		this.CmbOrderCategory = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.CmbDeliveryTerm = new UltraCombo();
		this.label16 = new System.Windows.Forms.Label();
		this.CmbPaymentTerm = new UltraCombo();
		this.CmbSupplierName = new UltraCombo();
		this.label15 = new System.Windows.Forms.Label();
		this.txtCommissionRemarks = new System.Windows.Forms.TextBox();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label51 = new System.Windows.Forms.Label();
		this.txtOrderAmount = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.CmbCommType = new UltraCombo();
		this.CmbCommUom = new UltraCombo();
		this.label25 = new System.Windows.Forms.Label();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtManualbillno = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.CmbCommissionAgent = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.duedate = new System.Windows.Forms.DateTimePicker();
		this.label11 = new System.Windows.Forms.Label();
		this.txtCommissionAmount = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.txtCommRate = new System.Windows.Forms.TextBox();
		this.label21 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.toolStripLabel1 = new System.Windows.Forms.Label();
		this.ChkPrintslip = new System.Windows.Forms.CheckBox();
		this.ChkResetOnSave = new System.Windows.Forms.CheckBox();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.BtnSaveAs = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnSlip = new System.Windows.Forms.ToolStripButton();
		this.btnIssuanceLoad = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel15 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label12 = new System.Windows.Forms.Label();
		this.panel14 = new System.Windows.Forms.Panel();
		this.grdHistory = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.cmbBranchName = new UltraCombo();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.label13 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.label58 = new System.Windows.Forms.Label();
		this.label64 = new System.Windows.Forms.Label();
		this.label65 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel10 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label8 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel8.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCondition).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSecondaryUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUOM).BeginInit();
		this.panel3.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionAgent).BeginInit();
		this.panel11.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel15.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel9.SuspendLayout();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel2.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel10.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1076, 656);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1068, 627);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel1.Controls.Add(this.panel8);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1062, 621);
		this.panel1.TabIndex = 7;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 351);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1062, 270);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label4);
		this.panel8.Controls.Add(this.ctrlGrdBar1);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 327);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1062, 24);
		this.panel8.TabIndex = 32;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(3, 3);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(46, 17);
		this.label4.TabIndex = 26;
		this.label4.Text = "Detail";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1021, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 33;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 204);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1062, 123);
		this.panel4.TabIndex = 30;
		this.groupBox1.BackColor = System.Drawing.Color.White;
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.btnCancel);
		this.groupBox1.Controls.Add(this.lblTotalAmount);
		this.groupBox1.Controls.Add(this.btnAdd);
		this.groupBox1.Controls.Add(this.btnUpdateDetail);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTaxType);
		this.groupBox1.Controls.Add(this.txtTaxPercent);
		this.groupBox1.Controls.Add(this.txtRemarksDetail);
		this.groupBox1.Controls.Add(this.txtTotalAmount);
		this.groupBox1.Controls.Add(this.lblTaxTypes);
		this.groupBox1.Controls.Add(this.lblTaxPercent);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemCondition);
		this.groupBox1.Controls.Add(this.txtTaxAmount);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add(this.lblTaxAmount);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSecondaryUom);
		this.groupBox1.Controls.Add(this.rdbtnItemCode);
		this.groupBox1.Controls.Add(this.label23);
		this.groupBox1.Controls.Add(this.rdbtnItemName);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRateUOM);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.groupBox1.Controls.Add(this.label40);
		this.groupBox1.Controls.Add(this.txtPerItemWeight);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.groupBox1.Controls.Add(this.label18);
		this.groupBox1.Controls.Add(this.label42);
		this.groupBox1.Controls.Add(this.lblBalance);
		this.groupBox1.Controls.Add(this.txtSecondaryUomQty);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add(this.txtSecondaryUomRate);
		this.groupBox1.Controls.Add(this.txtAmount);
		this.groupBox1.Controls.Add(this.txtRate);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add(this.txtQty);
		this.groupBox1.Controls.Add(this.label38);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUOM);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1010, 118);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Detail";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(599, 71);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(52, 15);
		this.label1.TabIndex = 111;
		this.label1.Text = "Remarks";
		this.btnCancel.BackColor = System.Drawing.Color.Teal;
		this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancel.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Bold);
		this.btnCancel.ForeColor = System.Drawing.Color.White;
		this.btnCancel.Location = new System.Drawing.Point(936, 68);
		this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Size = new System.Drawing.Size(67, 23);
		this.btnCancel.TabIndex = 20;
		this.btnCancel.Text = "Cancel";
		this.btnCancel.UseVisualStyleBackColor = false;
		this.btnCancel.Visible = false;
		this.btnCancel.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.lblTotalAmount.AutoSize = true;
		this.lblTotalAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalAmount.Location = new System.Drawing.Point(497, 71);
		this.lblTotalAmount.Name = "lblTotalAmount";
		this.lblTotalAmount.Size = new System.Drawing.Size(80, 15);
		this.lblTotalAmount.TabIndex = 144;
		this.lblTotalAmount.Text = "Total Amount";
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Bold);
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(936, 90);
		this.btnAdd.Margin = new System.Windows.Forms.Padding(2);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(42, 23);
		this.btnAdd.TabIndex = 18;
		this.btnAdd.Text = "+";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Bold);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(936, 90);
		this.btnUpdateDetail.Margin = new System.Windows.Forms.Padding(2);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(67, 23);
		this.btnUpdateDetail.TabIndex = 19;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.CmbTaxType.AllowNull = (DefaultableBoolean)1;
		this.CmbTaxType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTaxType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTaxType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTaxType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Location = new System.Drawing.Point(247, 88);
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Name = "CmbTaxType";
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Size = new System.Drawing.Size(101, 26);
		((System.Windows.Forms.Control)(object)this.CmbTaxType).TabIndex = 13;
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Leave += new System.EventHandler(CmbTaxType_Leave);
		this.txtTaxPercent.BackColor = System.Drawing.Color.White;
		this.txtTaxPercent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTaxPercent.Location = new System.Drawing.Point(350, 90);
		this.txtTaxPercent.Name = "txtTaxPercent";
		this.txtTaxPercent.Size = new System.Drawing.Size(55, 23);
		this.txtTaxPercent.TabIndex = 14;
		this.txtTaxPercent.TabStop = false;
		this.txtTaxPercent.Tag = "double";
		this.txtTaxPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTaxPercent.TextChanged += new System.EventHandler(txtTaxPercent_TextChanged);
		this.txtRemarksDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDetail.Location = new System.Drawing.Point(599, 90);
		this.txtRemarksDetail.Name = "txtRemarksDetail";
		this.txtRemarksDetail.Size = new System.Drawing.Size(336, 23);
		this.txtRemarksDetail.TabIndex = 17;
		this.txtTotalAmount.BackColor = System.Drawing.SystemColors.Control;
		this.txtTotalAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalAmount.Enabled = false;
		this.txtTotalAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalAmount.Location = new System.Drawing.Point(497, 90);
		this.txtTotalAmount.Name = "txtTotalAmount";
		this.txtTotalAmount.ReadOnly = true;
		this.txtTotalAmount.Size = new System.Drawing.Size(100, 23);
		this.txtTotalAmount.TabIndex = 16;
		this.txtTotalAmount.Text = "0";
		this.txtTotalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.lblTaxTypes.AutoSize = true;
		this.lblTaxTypes.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTaxTypes.Location = new System.Drawing.Point(246, 71);
		this.lblTaxTypes.Name = "lblTaxTypes";
		this.lblTaxTypes.Size = new System.Drawing.Size(53, 15);
		this.lblTaxTypes.TabIndex = 141;
		this.lblTaxTypes.Text = "Tax Type";
		this.lblTaxPercent.AutoSize = true;
		this.lblTaxPercent.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTaxPercent.Location = new System.Drawing.Point(350, 71);
		this.lblTaxPercent.Name = "lblTaxPercent";
		this.lblTaxPercent.Size = new System.Drawing.Size(38, 15);
		this.lblTaxPercent.TabIndex = 142;
		this.lblTaxPercent.Text = "Tax %";
		this.CmbItemCondition.AllowNull = (DefaultableBoolean)1;
		this.CmbItemCondition.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemCondition.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemCondition.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCondition).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance116;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCondition).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance127).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance127).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance171).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance171).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance182).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance182).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance182).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance182;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemCondition.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Location = new System.Drawing.Point(281, 42);
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Name = "CmbItemCondition";
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Size = new System.Drawing.Size(140, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Leave += new System.EventHandler(CmbItemCondition_Leave);
		this.txtTaxAmount.BackColor = System.Drawing.SystemColors.Control;
		this.txtTaxAmount.Enabled = false;
		this.txtTaxAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTaxAmount.Location = new System.Drawing.Point(406, 90);
		this.txtTaxAmount.Name = "txtTaxAmount";
		this.txtTaxAmount.ReadOnly = true;
		this.txtTaxAmount.Size = new System.Drawing.Size(90, 23);
		this.txtTaxAmount.TabIndex = 15;
		this.txtTaxAmount.Tag = "double";
		this.txtTaxAmount.Text = "0";
		this.txtTaxAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(281, 21);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(59, 15);
		this.label29.TabIndex = 133;
		this.label29.Text = "Condition";
		this.lblTaxAmount.AutoSize = true;
		this.lblTaxAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTaxAmount.Location = new System.Drawing.Point(404, 71);
		this.lblTaxAmount.Name = "lblTaxAmount";
		this.lblTaxAmount.Size = new System.Drawing.Size(72, 15);
		this.lblTaxAmount.TabIndex = 143;
		this.lblTaxAmount.Text = "Tax Amount";
		this.CmbSecondaryUom.AllowNull = (DefaultableBoolean)1;
		this.CmbSecondaryUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSecondaryUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSecondaryUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance164).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance164).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance164;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance168;
		((SpecialBoxBase)((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance169).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance169).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance169).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance172).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance174).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance175).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance175).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance175;
		((AppearanceBase)appearance176).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance162).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSecondaryUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSecondaryUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSecondaryUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSecondaryUom).Location = new System.Drawing.Point(717, 42);
		((System.Windows.Forms.Control)(object)this.CmbSecondaryUom).Name = "CmbSecondaryUom";
		((System.Windows.Forms.Control)(object)this.CmbSecondaryUom).Size = new System.Drawing.Size(80, 26);
		((System.Windows.Forms.Control)(object)this.CmbSecondaryUom).TabIndex = 6;
		this.rdbtnItemCode.AutoSize = true;
		this.rdbtnItemCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemCode.Location = new System.Drawing.Point(89, 19);
		this.rdbtnItemCode.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemCode.Name = "rdbtnItemCode";
		this.rdbtnItemCode.Size = new System.Drawing.Size(52, 19);
		this.rdbtnItemCode.TabIndex = 158;
		this.rdbtnItemCode.Text = "Code";
		this.rdbtnItemCode.UseVisualStyleBackColor = true;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(726, 13);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(63, 30);
		this.label23.TabIndex = 128;
		this.label23.Text = "Secondary\r\nUom";
		this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.rdbtnItemName.AutoSize = true;
		this.rdbtnItemName.Checked = true;
		this.rdbtnItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemName.Location = new System.Drawing.Point(36, 19);
		this.rdbtnItemName.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemName.Name = "rdbtnItemName";
		this.rdbtnItemName.Size = new System.Drawing.Size(57, 19);
		this.rdbtnItemName.TabIndex = 157;
		this.rdbtnItemName.TabStop = true;
		this.rdbtnItemName.Text = "Name";
		this.rdbtnItemName.UseVisualStyleBackColor = true;
		this.rdbtnItemName.CheckedChanged += new System.EventHandler(rdbtnItemName_CheckedChanged);
		this.CmbRateUOM.AllowNull = (DefaultableBoolean)1;
		this.CmbRateUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbRateUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance113;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbRateUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbRateUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance119).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbRateUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbRateUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbRateUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbRateUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateUOM).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRateUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateUOM).Location = new System.Drawing.Point(75, 88);
		((System.Windows.Forms.Control)(object)this.CmbRateUOM).Name = "CmbRateUOM";
		((System.Windows.Forms.Control)(object)this.CmbRateUOM).Size = new System.Drawing.Size(80, 26);
		((System.Windows.Forms.Control)(object)this.CmbRateUOM).TabIndex = 11;
		((System.Windows.Forms.Control)(object)this.CmbRateUOM).Leave += new System.EventHandler(comRateUOM_Leave);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(878, 13);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(55, 30);
		this.label17.TabIndex = 131;
		this.label17.Text = "Per Item \r\nWeight";
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance126;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance132).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance133).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(3, 42);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Leave += new System.EventHandler(comItem_Leave);
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(682, 359);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(39, 18);
		this.label40.TabIndex = 51;
		this.label40.Text = "Rate";
		this.txtPerItemWeight.AccessibleName = "PerItemWeight";
		this.txtPerItemWeight.BackColor = System.Drawing.Color.White;
		this.txtPerItemWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPerItemWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPerItemWeight.Location = new System.Drawing.Point(875, 44);
		this.txtPerItemWeight.Name = "txtPerItemWeight";
		this.txtPerItemWeight.Size = new System.Drawing.Size(60, 23);
		this.txtPerItemWeight.TabIndex = 8;
		this.txtPerItemWeight.Tag = "double";
		this.txtPerItemWeight.TextChanged += new System.EventHandler(txtPerItemWeight_TextChanged);
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label32.Location = new System.Drawing.Point(3, 21);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(32, 15);
		this.label32.TabIndex = 39;
		this.label32.Text = "Item";
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance152).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance152).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance153;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance158).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance159).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance159).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance159).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance159;
		((AppearanceBase)appearance161).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(423, 42);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(140, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Leave += new System.EventHandler(comjobLot_Leave);
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(804, 13);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(75, 30);
		this.label18.TabIndex = 129;
		this.label18.Text = "Secondary\r\nUom Weight";
		this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label42.Location = new System.Drawing.Point(3, 71);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(58, 15);
		this.label42.TabIndex = 55;
		this.label42.Text = "Item Rate";
		this.lblBalance.BackColor = System.Drawing.SystemColors.Control;
		this.lblBalance.Enabled = false;
		this.lblBalance.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBalance.Location = new System.Drawing.Point(205, 44);
		this.lblBalance.Name = "lblBalance";
		this.lblBalance.ReadOnly = true;
		this.lblBalance.Size = new System.Drawing.Size(75, 23);
		this.lblBalance.TabIndex = 1;
		this.lblBalance.Tag = "double";
		this.lblBalance.Text = "0";
		this.lblBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtSecondaryUomQty.AccessibleName = "SecondaryUomQty";
		this.txtSecondaryUomQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSecondaryUomQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSecondaryUomQty.Location = new System.Drawing.Point(798, 44);
		this.txtSecondaryUomQty.Name = "txtSecondaryUomQty";
		this.txtSecondaryUomQty.Size = new System.Drawing.Size(75, 23);
		this.txtSecondaryUomQty.TabIndex = 7;
		this.txtSecondaryUomQty.Tag = "double";
		this.txtSecondaryUomQty.TextChanged += new System.EventHandler(txtSecondaryUomQty_TextChanged);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(156, 71);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(51, 15);
		this.label14.TabIndex = 21;
		this.label14.Text = "Amount";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(936, 13);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 30);
		this.label27.TabIndex = 130;
		this.label27.Text = "Secondary\r\nUom Rate";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label33.Location = new System.Drawing.Point(423, 21);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(46, 15);
		this.label33.TabIndex = 37;
		this.label33.Text = "Job Lot";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(215, 13);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(55, 30);
		this.label30.TabIndex = 143;
		this.label30.Text = "Available\r\nStock";
		this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtSecondaryUomRate.AccessibleName = "SecondaryUomRate";
		this.txtSecondaryUomRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSecondaryUomRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSecondaryUomRate.Location = new System.Drawing.Point(936, 44);
		this.txtSecondaryUomRate.Name = "txtSecondaryUomRate";
		this.txtSecondaryUomRate.Size = new System.Drawing.Size(70, 23);
		this.txtSecondaryUomRate.TabIndex = 9;
		this.txtSecondaryUomRate.Tag = "double";
		this.txtSecondaryUomRate.TextChanged += new System.EventHandler(txtSecondaryUomRate_TextChanged);
		this.txtAmount.BackColor = System.Drawing.SystemColors.Control;
		this.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmount.Enabled = false;
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmount.Location = new System.Drawing.Point(156, 90);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.ReadOnly = true;
		this.txtAmount.Size = new System.Drawing.Size(90, 23);
		this.txtAmount.TabIndex = 12;
		this.txtAmount.TabStop = false;
		this.txtAmount.Tag = "double";
		this.txtRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRate.Location = new System.Drawing.Point(3, 90);
		this.txtRate.Name = "txtRate";
		this.txtRate.Size = new System.Drawing.Size(70, 23);
		this.txtRate.TabIndex = 10;
		this.txtRate.Tag = "double";
		this.txtRate.TextChanged += new System.EventHandler(txtRate_TextChanged);
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(85, 71);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(61, 15);
		this.label22.TabIndex = 27;
		this.label22.Text = "Rate UOM";
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQty.Location = new System.Drawing.Point(645, 44);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(70, 23);
		this.txtQty.TabIndex = 5;
		this.txtQty.Tag = "double";
		this.txtQty.TextChanged += new System.EventHandler(txtQty_TextChanged);
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label38.Location = new System.Drawing.Point(645, 21);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(26, 15);
		this.label38.TabIndex = 47;
		this.label38.Text = "Qty";
		this.CmbPackUOM.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance139).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance139).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance140;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance145).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance146).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance146).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance146).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance147).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUOM).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUOM).Location = new System.Drawing.Point(564, 42);
		((System.Windows.Forms.Control)(object)this.CmbPackUOM).Name = "CmbPackUOM";
		((System.Windows.Forms.Control)(object)this.CmbPackUOM).Size = new System.Drawing.Size(80, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUOM).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbPackUOM).Leave += new System.EventHandler(comPackUOM_Leave);
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label28.Location = new System.Drawing.Point(564, 21);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(63, 15);
		this.label28.TabIndex = 29;
		this.label28.Text = "Pack UOM";
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.groupBox2);
		this.panel3.Controls.Add(this.panel11);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel3.Location = new System.Drawing.Point(0, 25);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1062, 179);
		this.panel3.TabIndex = 0;
		this.groupBox2.Controls.Add(this.txtduedays);
		this.groupBox2.Controls.Add(this.txtBranchSrNo);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add(this.txtSupplierGLId);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Controls.Add(this.txtCommAgentId);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOrderCategory);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm);
		this.groupBox2.Controls.Add(this.label16);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentTerm);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplierName);
		this.groupBox2.Controls.Add(this.label15);
		this.groupBox2.Controls.Add(this.txtCommissionRemarks);
		this.groupBox2.Controls.Add(this.txtdocno);
		this.groupBox2.Controls.Add(this.label51);
		this.groupBox2.Controls.Add(this.txtOrderAmount);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add(this.DocDate);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommType);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommUom);
		this.groupBox2.Controls.Add(this.label25);
		this.groupBox2.Controls.Add(this.txtremarks);
		this.groupBox2.Controls.Add(this.label24);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.txtManualbillno);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommissionAgent);
		this.groupBox2.Controls.Add(this.label20);
		this.groupBox2.Controls.Add(this.label26);
		this.groupBox2.Controls.Add(this.duedate);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add(this.txtCommissionAmount);
		this.groupBox2.Controls.Add(this.label19);
		this.groupBox2.Controls.Add(this.txtCommRate);
		this.groupBox2.Controls.Add(this.label21);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(2, 28);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1011, 151);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main Information";
		this.txtduedays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtduedays.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtduedays.Location = new System.Drawing.Point(402, 71);
		this.txtduedays.Name = "txtduedays";
		this.txtduedays.Size = new System.Drawing.Size(60, 23);
		this.txtduedays.TabIndex = 7;
		this.txtduedays.Tag = "int";
		this.txtduedays.Text = "0";
		this.txtduedays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtduedays.TextChanged += new System.EventHandler(txtduedays_TextChanged);
		this.txtBranchSrNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBranchSrNo.Enabled = false;
		this.txtBranchSrNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBranchSrNo.Location = new System.Drawing.Point(210, 17);
		this.txtBranchSrNo.Name = "txtBranchSrNo";
		this.txtBranchSrNo.Size = new System.Drawing.Size(95, 23);
		this.txtBranchSrNo.TabIndex = 118;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(5, 48);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(57, 15);
		this.label9.TabIndex = 117;
		this.label9.Text = "Doc Date";
		this.txtSupplierGLId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierGLId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierGLId.Location = new System.Drawing.Point(668, 69);
		this.txtSupplierGLId.Multiline = true;
		this.txtSupplierGLId.Name = "txtSupplierGLId";
		this.txtSupplierGLId.Size = new System.Drawing.Size(16, 26);
		this.txtSupplierGLId.TabIndex = 61;
		this.txtSupplierGLId.Visible = false;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(4, 127);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(87, 15);
		this.label3.TabIndex = 116;
		this.label3.Text = "Order Category";
		this.txtCommAgentId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommAgentId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommAgentId.Location = new System.Drawing.Point(668, 41);
		this.txtCommAgentId.Multiline = true;
		this.txtCommAgentId.Name = "txtCommAgentId";
		this.txtCommAgentId.Size = new System.Drawing.Size(16, 26);
		this.txtCommAgentId.TabIndex = 62;
		this.txtCommAgentId.Visible = false;
		this.CmbOrderCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbOrderCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOrderCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOrderCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance165).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance165).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance166;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOrderCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbOrderCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOrderCategory).Location = new System.Drawing.Point(95, 121);
		((System.Windows.Forms.Control)(object)this.CmbOrderCategory).Name = "CmbOrderCategory";
		((System.Windows.Forms.Control)(object)this.CmbOrderCategory).Size = new System.Drawing.Size(212, 26);
		((System.Windows.Forms.Control)(object)this.CmbOrderCategory).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbOrderCategory).Leave += new System.EventHandler(CmbOrderCategory_Leave);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(311, 21);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(80, 15);
		this.label2.TabIndex = 114;
		this.label2.Text = "Delivery Term";
		this.CmbDeliveryTerm.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Location = new System.Drawing.Point(402, 15);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Name = "CmbDeliveryTerm";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Size = new System.Drawing.Size(187, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).TabIndex = 5;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(311, 48);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(84, 15);
		this.label16.TabIndex = 112;
		this.label16.Text = "Payment Term";
		this.CmbPaymentTerm.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance177).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance178).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance178).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance178).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance179).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance179;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance180).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance180).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance181).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance183).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance184;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance185).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance186).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance186).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance186).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance187).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPaymentTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Location = new System.Drawing.Point(402, 42);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Name = "CmbPaymentTerm";
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Size = new System.Drawing.Size(187, 26);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).TextChanged += new System.EventHandler(CmbPaymentTerm_TextChanged);
		this.CmbSupplierName.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplierName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplierName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplierName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance190).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance191).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance191).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance191;
		((AppearanceBase)appearance192).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance192;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbSupplierName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Location = new System.Drawing.Point(95, 69);
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Name = "CmbSupplierName";
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Size = new System.Drawing.Size(212, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).TabIndex = 2;
		this.CmbSupplierName.ValueChanged += new System.EventHandler(CmbSupplierName_Leave);
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Leave += new System.EventHandler(CmbSupplierName_Leave);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label15.Location = new System.Drawing.Point(311, 75);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(92, 15);
		this.label15.TabIndex = 111;
		this.label15.Text = "Due Days , Date";
		this.txtCommissionRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommissionRemarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCommissionRemarks.Location = new System.Drawing.Point(686, 97);
		this.txtCommissionRemarks.Name = "txtCommissionRemarks";
		this.txtCommissionRemarks.Size = new System.Drawing.Size(222, 23);
		this.txtCommissionRemarks.TabIndex = 15;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Enabled = false;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(95, 17);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.Size = new System.Drawing.Size(114, 23);
		this.txtdocno.TabIndex = 0;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(311, 101);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(84, 15);
		this.label51.TabIndex = 103;
		this.label51.Text = "Order Amount";
		this.txtOrderAmount.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderAmount.Location = new System.Drawing.Point(402, 95);
		this.txtOrderAmount.Name = "txtOrderAmount";
		this.txtOrderAmount.ReadOnly = true;
		this.txtOrderAmount.Size = new System.Drawing.Size(187, 26);
		this.txtOrderAmount.TabIndex = 9;
		this.txtOrderAmount.TabStop = false;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(4, 21);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(88, 15);
		this.label5.TabIndex = 88;
		this.label5.Text = "Doc && Branch#";
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(96, 44);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(209, 22);
		this.DocDate.TabIndex = 1;
		this.DocDate.ValueChanged += new System.EventHandler(DocDate_ValueChanged);
		this.DocDate.Leave += new System.EventHandler(DocDate_Leave);
		this.CmbCommType.AllowNull = (DefaultableBoolean)1;
		this.CmbCommType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbCommType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbCommType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbCommType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCommType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommType).Location = new System.Drawing.Point(686, 69);
		((System.Windows.Forms.Control)(object)this.CmbCommType).Name = "CmbCommType";
		((System.Windows.Forms.Control)(object)this.CmbCommType).Size = new System.Drawing.Size(82, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommType).TabIndex = 13;
		((System.Windows.Forms.Control)(object)this.CmbCommType).Leave += new System.EventHandler(combCommType_Leave_1);
		this.CmbCommUom.AllowNull = (DefaultableBoolean)1;
		this.CmbCommUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommUom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCommUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommUom).Location = new System.Drawing.Point(828, 42);
		((System.Windows.Forms.Control)(object)this.CmbCommUom).Name = "CmbCommUom";
		((System.Windows.Forms.Control)(object)this.CmbCommUom).Size = new System.Drawing.Size(80, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommUom).TabIndex = 12;
		((System.Windows.Forms.Control)(object)this.CmbCommUom).Leave += new System.EventHandler(combcommUOM_Leave_1);
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(593, 101);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(91, 15);
		this.label25.TabIndex = 95;
		this.label25.Text = "Comm Remarks";
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(402, 123);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(506, 23);
		this.txtremarks.TabIndex = 16;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(311, 127);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(52, 15);
		this.label24.TabIndex = 102;
		this.label24.Text = "Remarks";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(4, 75);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(93, 15);
		this.label7.TabIndex = 65;
		this.label7.Text = "Customer Name";
		this.txtManualbillno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtManualbillno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtManualbillno.Location = new System.Drawing.Point(95, 97);
		this.txtManualbillno.Name = "txtManualbillno";
		this.txtManualbillno.Size = new System.Drawing.Size(212, 23);
		this.txtManualbillno.TabIndex = 3;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(4, 101);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(86, 15);
		this.label6.TabIndex = 64;
		this.label6.Text = "Manual Ref No";
		this.CmbCommissionAgent.AllowNull = (DefaultableBoolean)1;
		this.CmbCommissionAgent.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommissionAgent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommissionAgent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCommissionAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).Location = new System.Drawing.Point(686, 15);
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).Name = "CmbCommissionAgent";
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).Size = new System.Drawing.Size(222, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).TabIndex = 10;
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).Leave += new System.EventHandler(combcommAgent_Leave);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label20.Location = new System.Drawing.Point(593, 21);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(78, 15);
		this.label20.TabIndex = 94;
		this.label20.Text = "Comm Agent";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label26.Location = new System.Drawing.Point(774, 75);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(51, 15);
		this.label26.TabIndex = 96;
		this.label26.Text = "Amount";
		this.duedate.CustomFormat = "dd-MM-yy";
		this.duedate.Enabled = false;
		this.duedate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.duedate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.duedate.Location = new System.Drawing.Point(464, 71);
		this.duedate.Name = "duedate";
		this.duedate.Size = new System.Drawing.Size(125, 23);
		this.duedate.TabIndex = 8;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(593, 75);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(71, 15);
		this.label11.TabIndex = 92;
		this.label11.Text = "Comm Type";
		this.txtCommissionAmount.BackColor = System.Drawing.Color.White;
		this.txtCommissionAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommissionAmount.Enabled = false;
		this.txtCommissionAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCommissionAmount.Location = new System.Drawing.Point(828, 71);
		this.txtCommissionAmount.Name = "txtCommissionAmount";
		this.txtCommissionAmount.ReadOnly = true;
		this.txtCommissionAmount.Size = new System.Drawing.Size(80, 23);
		this.txtCommissionAmount.TabIndex = 14;
		this.txtCommissionAmount.TabStop = false;
		this.txtCommissionAmount.Tag = "double";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label19.Location = new System.Drawing.Point(774, 48);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(35, 15);
		this.label19.TabIndex = 93;
		this.label19.Text = "UOM";
		this.txtCommRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCommRate.ForeColor = System.Drawing.SystemColors.WindowText;
		this.txtCommRate.Location = new System.Drawing.Point(686, 44);
		this.txtCommRate.Name = "txtCommRate";
		this.txtCommRate.Size = new System.Drawing.Size(82, 23);
		this.txtCommRate.TabIndex = 11;
		this.txtCommRate.Tag = "double";
		this.txtCommRate.TextChanged += new System.EventHandler(txtcommrate_TextChanged);
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(593, 48);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(69, 15);
		this.label21.TabIndex = 97;
		this.label21.Text = "Comm Rate";
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.toolStripLabel1);
		this.panel11.Controls.Add(this.ChkPrintslip);
		this.panel11.Controls.Add(this.ChkResetOnSave);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1062, 26);
		this.panel11.TabIndex = 108;
		this.toolStripLabel1.AutoSize = true;
		this.toolStripLabel1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripLabel1.ForeColor = System.Drawing.Color.White;
		this.toolStripLabel1.Location = new System.Drawing.Point(3, 3);
		this.toolStripLabel1.Name = "toolStripLabel1";
		this.toolStripLabel1.Size = new System.Drawing.Size(95, 19);
		this.toolStripLabel1.TabIndex = 25;
		this.toolStripLabel1.Text = "Sale Order";
		this.ChkPrintslip.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrintslip.AutoSize = true;
		this.ChkPrintslip.Checked = true;
		this.ChkPrintslip.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkPrintslip.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrintslip.ForeColor = System.Drawing.Color.White;
		this.ChkPrintslip.Location = new System.Drawing.Point(974, 5);
		this.ChkPrintslip.Margin = new System.Windows.Forms.Padding(2);
		this.ChkPrintslip.Name = "ChkPrintslip";
		this.ChkPrintslip.Size = new System.Drawing.Size(87, 18);
		this.ChkPrintslip.TabIndex = 17;
		this.ChkPrintslip.TabStop = false;
		this.ChkPrintslip.Text = "Print Slip";
		this.ChkPrintslip.UseVisualStyleBackColor = true;
		this.ChkResetOnSave.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkResetOnSave.AutoSize = true;
		this.ChkResetOnSave.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkResetOnSave.ForeColor = System.Drawing.Color.White;
		this.ChkResetOnSave.Location = new System.Drawing.Point(857, 5);
		this.ChkResetOnSave.Margin = new System.Windows.Forms.Padding(2);
		this.ChkResetOnSave.Name = "ChkResetOnSave";
		this.ChkResetOnSave.Size = new System.Drawing.Size(115, 18);
		this.ChkResetOnSave.TabIndex = 18;
		this.ChkResetOnSave.TabStop = false;
		this.ChkResetOnSave.Text = "Reset On Add";
		this.ChkResetOnSave.UseVisualStyleBackColor = true;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[10] { this.btnNew, this.btnRefresh, this.btnSave, this.BtnSaveAs, this.btnUpdate, this.btnDelete, this.btnAttachment, this.btnSlip, this.btnIssuanceLoad, this.btnshortcutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1062, 25);
		this.toolStrip1.TabIndex = 26;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 22);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 22);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(60, 22);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(saveToolStripButton_Click);
		this.BtnSaveAs.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.BtnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnSaveAs.Name = "BtnSaveAs";
		this.BtnSaveAs.Size = new System.Drawing.Size(76, 22);
		this.BtnSaveAs.Text = "&SaveAs";
		this.BtnSaveAs.Visible = false;
		this.BtnSaveAs.Click += new System.EventHandler(BtnSaveAs_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(74, 22);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(69, 22);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(105, 22);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnSlip.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSlip.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSlip.Name = "btnSlip";
		this.btnSlip.Size = new System.Drawing.Size(80, 22);
		this.btnSlip.Text = "90-Print";
		this.btnSlip.Click += new System.EventHandler(btnSlip_Click);
		this.btnIssuanceLoad.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnIssuanceLoad.Image = Architecture.WinApp.Properties.Resources.download;
		this.btnIssuanceLoad.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnIssuanceLoad.Name = "btnIssuanceLoad";
		this.btnIssuanceLoad.Size = new System.Drawing.Size(164, 22);
		this.btnIssuanceLoad.Text = "Load Available Stock";
		this.btnIssuanceLoad.Click += new System.EventHandler(btnIssuanceLoad_Click);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(122, 22);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.tabPage2.Controls.Add(this.panel15);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel14);
		this.tabPage2.Controls.Add(this.panel2);
		this.tabPage2.Controls.Add(this.panel10);
		this.tabPage2.Controls.Add(this.toolStrip3);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1068, 627);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel15.Location = new System.Drawing.Point(3, 481);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1062, 143);
		this.panel15.TabIndex = 55;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
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
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1062, 143);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 49;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label12);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 461);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1062, 20);
		this.panel9.TabIndex = 54;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.White;
		this.label12.Location = new System.Drawing.Point(4, 2);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(200, 16);
		this.label12.TabIndex = 35;
		this.label12.Text = "Detail Of Above Selected Row";
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(3, 124);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1062, 337);
		this.panel14.TabIndex = 53;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1062, 337);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 45;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(grdHistory_SelectionChanged);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.panel2.Controls.Add(this.groupBox4);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 51);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1062, 73);
		this.panel2.TabIndex = 52;
		this.groupBox4.Controls.Add(this.rdapproveddate);
		this.groupBox4.Controls.Add(this.rdentrydate);
		this.groupBox4.Controls.Add(this.rdmodifydate);
		this.groupBox4.Controls.Add(this.btnShowHistory);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchName);
		this.groupBox4.Controls.Add(this.drdocdate);
		this.groupBox4.Controls.Add(this.label13);
		this.groupBox4.Controls.Add(this.label10);
		this.groupBox4.Controls.Add(this.label57);
		this.groupBox4.Controls.Add(this.label58);
		this.groupBox4.Controls.Add(this.label64);
		this.groupBox4.Controls.Add(this.label65);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox4.Controls.Add(this.ToDocNoHistory);
		this.groupBox4.Controls.Add(this.FromDocNoHistory);
		this.groupBox4.Controls.Add(this.ToDateHistory);
		this.groupBox4.Controls.Add(this.FromDateHistory);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(2, 5);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(1055, 65);
		this.groupBox4.TabIndex = 1;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(952, 37);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 9;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(871, 37);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 8;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(952, 13);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(804, 33);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 6;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.cmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance106).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance107).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Location = new System.Drawing.Point(6, 33);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Name = "cmbBranchName";
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Size = new System.Drawing.Size(163, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).TabIndex = 0;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(871, 13);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 7;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(6, 15);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(79, 15);
		this.label13.TabIndex = 3;
		this.label13.Text = "Branch Name";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(604, 15);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(58, 15);
		this.label10.TabIndex = 87;
		this.label10.Text = "Customer";
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.Location = new System.Drawing.Point(524, 15);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(64, 15);
		this.label57.TabIndex = 86;
		this.label57.Text = "To Doc No";
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.Location = new System.Drawing.Point(444, 15);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(79, 15);
		this.label58.TabIndex = 85;
		this.label58.Text = "From Doc No";
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label64.Location = new System.Drawing.Point(307, 15);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(48, 15);
		this.label64.TabIndex = 84;
		this.label64.Text = "To Date";
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label65.Location = new System.Drawing.Point(170, 15);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(63, 15);
		this.label65.TabIndex = 83;
		this.label65.Text = "From Date";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(604, 33);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(199, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 5;
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(524, 35);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.ToDocNoHistory.TabIndex = 4;
		this.ToDocNoHistory.Tag = "int";
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(444, 35);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.FromDocNoHistory.TabIndex = 3;
		this.FromDocNoHistory.Tag = "int";
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(307, 35);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(136, 23);
		this.ToDateHistory.TabIndex = 2;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(170, 35);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(136, 23);
		this.FromDateHistory.TabIndex = 1;
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.ctrlGrdBar2);
		this.panel10.Controls.Add(this.label8);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 28);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1062, 23);
		this.panel10.TabIndex = 51;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1022, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 34;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(3, 2);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(158, 19);
		this.label8.TabIndex = 35;
		this.label8.Text = "Sale Order History";
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNewHistory, this.btnRefreshHistory });
		this.toolStrip3.Location = new System.Drawing.Point(3, 3);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1062, 25);
		this.toolStrip3.TabIndex = 50;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(55, 22);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(76, 22);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1076, 656);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "frmSaleOrderStoreAndPm";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InvfrmInvSaleInvoiceDirectPackingMaterial_FormClosing);
		base.Load += new System.EventHandler(InvfrmPurchasedirectInvoice_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvfrmPurchasedirectInvoice_KeyDown_1);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCondition).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSecondaryUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUOM).EndInit();
		this.panel3.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionAgent).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.tabPage2.PerformLayout();
		this.panel15.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel14.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel2.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
