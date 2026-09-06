using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.StoreManagement;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Inventory.FromWise_Helper_Methods;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
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
namespace Architecture.WinApp.StoreManagement
{

public class PurchaseInvoiceStoreManagement : Form
{
	private Attachment AT = new Attachment();

	private WaitForm waitForm = new WaitForm();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtGrid = new DataTable();

	private DataTable dtFreight = new DataTable();

	private DataTable dtGrdGL = new DataTable();

	private DataTable dtExpGrid = new DataTable();

	private DataTable dthistory = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtBranch = new DataTable();

	private int GrnBaseDocumentTypeId;

	private int DocumentTypeId;

	private int Id;

	private int VoucherHeadId;

	private string ScreenName = "PurchaseInvoiceStoreManagement";

	private bool Approved;

	private bool FormInitialized;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool BranchImplemented;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabForm;

	private Panel panel2;

	private TabPage tabHistory;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton btnPrint;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton btnAttachment;

	private ToolStripButton toolStripButton3;

	private Panel panelpurinv;

	private Label lblFormCaption;

	private Panel panel4;

	private Label label20;

	private ComboBox cmbcommtype;

	private Label label26;

	private Label label11;

	private TextBox txtcommremarks;

	private Label label25;

	private Label label21;

	private TextBox txtcommrate;

	private ComboBox cmbcommuom;

	private Label label19;

	private TextBox txtcommamount;

	private Label label7;

	private Label label6;

	private TextBox txtbillno;

	private DateTimePicker DocDate;

	private Label label17;

	private TextBox txtdocno;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdHistory;

	private CtrlGrdBar ctrlGrdBar2;

	internal GridEX grd;

	private Panel panel7;

	private Panel panel11;

	private Panel panel9;

	internal GridEX grdGLedger;

	internal GridEX grdFreight;

	private TextBox txtBillAmount;

	internal UltraCombo cmbsuppliername;

	internal UltraCombo cmbcommagent;

	private Label label4;

	private TextBox txtSupplierGLId;

	private TextBox txtCommAgentId;

	private Label label9;

	private TextBox txtDeliveryTerm;

	private Label label15;

	private ToolStripButton btnSlipDetail;

	private CheckBox ChkBok;

	private Panel panel14;

	private Panel panel6;

	private ToolStrip toolStrip2;

	private ToolStripSeparator toolStripSeparator1;

	internal UltraCombo CmbTaxAccount;

	private TextBox txtDueDays;

	private Label label22;

	private DateTimePicker DueDate;

	internal GridEX grdDetail;

	private Panel panel8;

	private TextBox txtInvoiceTaxNo;

	private Label lblInvoiceTaxNo;

	private ToolStripButton btnLoadAll;

	private CheckBox chkVoucherPreview;

	private GroupBox groupBox3;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Button btnShowHistory;

	private Label label53;

	private Label label1;

	private Label label12;

	private Label label57;

	internal UltraCombo CmbCustomerHistory;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Panel panel16;

	private Label label24;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnRefreshHistory;

	private Panel panel18;

	private Label label27;

	private Panel panel20;

	private Label label28;

	private Panel panel22;

	private Label label29;

	private ToolStripButton btnFrmRefresh;

	private ToolStripButton btnshortcut;

	private TextBox txtRemarks;

	private Label label5;

	private TextBox txtBranchSrNo;

	private Label label13;

	internal UltraCombo cmbBranchName;

	private Label label30;

	private ToolStripButton btnDelete;

	private Label label16;

	internal UltraCombo CmbPaymentTerm;

	private Panel panel15;

	private Panel panel5;

	internal GridEX grdInvExp;

	private Panel panel10;

	private Label label81;

	private Label label63;

	private Label label65;

	private Label label32;

	private Label label64;

	private Label label62;

	private Label label66;

	private Label label61;

	private TextBox txtItemDiscountAmountFooter;

	private TextBox txtItemAmountWithoutDiscountFooter;

	private TextBox txtItemNetAmountFooter;

	private TextBox txtTaxAmountFooter;

	internal UltraCombo CmbDiscountAccount;

	private Label label84;

	private Label label2;

	private Label label8;

	private TextBox txtExpenseAmount;

	private TextBox txtBillDiscountAmountFooter;

	private Label label14;

	private Label label10;

	private Label label3;

	private Label label23;

	private Label label18;

	private TextBox txtItemAmountPlusTaxAmountFooter;

	private Label label31;

	private Label label34;

	private Label label33;

	private TextBox txtFreightAmuontFooter;

	private Label label36;

	private Label label35;

	private TextBox txtPartyAddLessAmountFooter;

	public PurchaseInvoiceStoreManagement(UserAccount user)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
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

	private void InvfrmPurchaseInvoice_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(1100, 800);
			txtItemDiscountAmountFooter.Enabled = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private async void InitializeComponentCustom()
	{
		try
		{
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("CompanyName", typeof(string));
			dtSupplier.Columns.Add("PartyCode", typeof(string));
			dtSupplier.Columns.Add("GlAccountId", typeof(int));
			dtSupplier.Columns.Add("CityId", typeof(int));
			dtSupplier.Columns.Add("CityName", typeof(string));
			dtSupplier.Columns.Add("MobileNo", typeof(string));
			dtBranch.Columns.Add("Id", typeof(int));
			dtBranch.Columns.Add("BranchName", typeof(string));
			frmPurchaseInvoiceStoreManagement_Helper.InitializeDetailtable(dtGrid);
			dtExpGrid.Columns.Add("Id", typeof(int));
			dtExpGrid.Columns.Add("ItemId", typeof(int));
			dtExpGrid.Columns.Add("Qty", typeof(double));
			dtExpGrid.Columns.Add("Rate", typeof(double));
			dtExpGrid.Columns.Add("Amount", typeof(double));
			dtExpGrid.Columns.Add("Remarks");
			dtGrdGL.Columns.Add("AccountId");
			dtGrdGL.Columns.Add("Remarks");
			dtGrdGL.Columns.Add("Percentage");
			dtGrdGL.Columns.Add("Qty");
			dtGrdGL.Columns.Add("Rate");
			dtGrdGL.Columns.Add("Debit", typeof(double));
			dtGrdGL.Columns.Add("Credit", typeof(double));
			dtFreight.Columns.Add("InvGrnId");
			dtFreight.Columns.Add("Transporter");
			dtFreight.Columns.Add("Freight", typeof(double));
			dtFreight.Columns.Add("Debit", typeof(double));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private async void InitializeComponentMethod()
	{
		Task.Run(delegate
		{
			DocumentTypeId = ((ScreenName == "PurchaseInvoiceStoreManagement") ? 64 : 58);
			formright = CommonServices.SetRightsValueInRightsObject(ScreenName);
			HistoryBranchComboDbCall();
			DataTable historyComboData = HistoryComboDbCall();
			string documentNoData = Conversion.ToString((object)CommonServices.PurchaseInvoiceGenerateCode(DocumentTypeId));
			string branchSrNoData = Conversion.ToString((object)CommonServices.PurchaseInvoiceGenerateCodeBranch(DocumentTypeId, UserAccount.BranchesId));
			string taxDocumentNoData = Conversion.ToString((object)CommonServices.GenerateSalesTaxNo(IsTaxable: true));
			return new
			{
				HistoryComboData = historyComboData,
				DocumentNoData = documentNoData,
				BranchSrNoData = branchSrNoData,
				TaxDocumentNoData = taxDocumentNoData
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
				btnSave.Enabled = formright.DoHaveSaveRight;
				btnUpdate.Enabled = formright.DoHaveUpdateRights;
				btnDelete.Enabled = formright.DoHaveCanDelete;
				btnPrint.Enabled = formright.DoHavePrintRights;
				FormHelper.SetCtrlGrdBarRight(ctrlGrdBar1, formright);
				FormHelper.SetCtrlGrdBarRight(ctrlGrdBar2, formright);
				ctrlGrdBar2.mGridSaveLayouts.Visible = false;
				ctrlGrdBar2.removeLayoutToolStripMenuItem.Visible = false;
				GetConfigurationsFromGlobal();
				if (Id == 0)
				{
					txtdocno.Text = result.DocumentNoData;
					txtBranchSrNo.Text = result.BranchSrNoData;
					txtInvoiceTaxNo.Text = result.TaxDocumentNoData;
				}
				SupplierDtFillFromGlobal();
				BindSupplierName();
				TaxAccountFillFromGlobal();
				PaymentTermBindFromGlobal();
				OtherItemdtDbCall();
				GetConfigurationsFromGlobalandBind();
				BindGrids();
				HistoryBranchComboBind();
				HistoryComboBind(result.HistoryComboData);
				FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
				ToDateHistory.Value = DateTime.Now;
				FormInitialized = true;
				DocDate.Focus();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void GetConfigurationsFromGlobal()
	{
		try
		{
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"));
			BranchImplemented = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("PurchaseInvoiceBranchWise"));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetConfigurationsFromGlobalandBind()
	{
	}

	private void SupplierDtFillFromGlobal()
	{
		try
		{
			dtSupplier.Clear();
			List<getGlobalSupplierCustomer> obj = clsGlobalVariables.globalSupplierCustomer;
			if (obj != null && obj.Count > 0)
			{
				for (int i = 0; i < obj.Count; i++)
				{
					dtSupplier.Rows.Add(Conversion.ToInt((object)obj[i].Id), Conversion.ToString((object)obj[i].CompanyName), Conversion.ToString((object)obj[i].PartyCode), Conversion.ToInt((object)obj[i].GlAccountId), Conversion.ToInt((object)obj[i].CityId), Conversion.ToString((object)obj[i].CityName), Conversion.ToString((object)obj[i].MobilePersonal));
				}
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
			int SupId = Conversion.ToInt(cmbsuppliername.Value);
			InfragisticsHelper.BindAndRetainSelection(cmbsuppliername, dtSupplier, "Id", "CompanyName", "SupplierName", AllColumns: true, SupId, insertDefaultRow: false, null, null, ActivateRow: false);
			if (dtSupplier.Rows.Count > 0)
			{
				((UltraGridBase)cmbsuppliername).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)cmbsuppliername).DisplayLayout.Bands[0].Columns[4].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void TaxAccountFillFromGlobal()
	{
		try
		{
			DataTable dt = DatatableHelper.GetTaxAccountsFromGlobal();
			InfragisticsHelper.BindAndRetainSelection(CmbTaxAccount, dt, "Id", "AccountTitle", "Account Title", AllColumns: true, Conversion.ToInt(CmbTaxAccount.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			DataTable dt2 = DatatableHelper.GetAccountsFromGlobalByTypeIds(new int[5] { 11, 13, 20, 21, 22 });
			InfragisticsHelper.BindAndRetainSelection(CmbDiscountAccount, dt2, "Id", "AccountTitle", "Account Title", AllColumns: true, Conversion.ToInt(CmbDiscountAccount.Value), insertDefaultRow: false, null, null, ActivateRow: false);
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

	private void OtherItemdtDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			InventoryItemsOther obj = new InventoryItemsOther();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtOtherItem = InventoryItemsOther.GetAll(obj);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindGrids(HashSet<int> ids = null)
	{
		try
		{
			if (ids == null)
			{
				ids = new HashSet<int> { 1, 2, 3, 4 };
			}
			if (ids.Contains(1))
			{
				DataTable dataTable = dtGrid;
				if (dataTable != null && dataTable.Columns.Count > 0)
				{
					grd.DataSource = dtGrid;
					grd.RetrieveStructure();
					grdSettings();
				}
			}
			if (ids.Contains(2))
			{
				DataTable dataTable2 = dtFreight;
				if (dataTable2 != null && dataTable2.Columns.Count > 0)
				{
					if (dtFreight.Rows.Count == 0)
					{
						AddRowInFreightGrid();
					}
					grdFreight.RetrieveStructure();
					grdFreightSettings();
				}
			}
			if (ids.Contains(3))
			{
				DataTable dataTable3 = dtGrdGL;
				if (dataTable3 != null && dataTable3.Columns.Count > 0)
				{
					if (dtGrdGL.Rows.Count == 0)
					{
						AddRowInGLGrid();
					}
					grdGLedger.RetrieveStructure();
					gridGLSettings();
				}
			}
			if (!ids.Contains(4))
			{
				return;
			}
			DataTable dataTable4 = dtExpGrid;
			if (dataTable4 != null && dataTable4.Columns.Count > 0)
			{
				if (dtExpGrid.Rows.Count == 0)
				{
					AddRowInvExpGrid();
				}
				grdInvExp.RetrieveStructure();
				grdInvExpSettings();
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
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"Rate",
					(EditType)(GrnBaseDocumentTypeId == 2)
				},
				{
					"DiscountAmount",
					(EditType)1
				},
				{
					"Remarks",
					(EditType)1
				}
			};
			frmPurchaseInvoiceStoreManagement_Helper.DetailGridCommonSetting(grd, editableColumns, GrnBaseDocumentTypeId);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.UpdateData();
			GridEXRow row = grd.CurrentRow;
			if (row != null && (int)row.RowType == 0)
			{
				string col = e.Column.Key;
				if (col == "Rate")
				{
					decimal qty = Conversion.ToDecimal(row.Cells["ItemQty"].Value);
					decimal rate = Conversion.ToDecimal(row.Cells["Rate"].Value);
					row.Cells["ItemAmountWithoutDiscount"].Value = qty * rate;
				}
				if (col == "Rate" || col == "DiscountAmount")
				{
					RecalculateRow(row);
				}
				grd.UpdateData();
				BillAmount();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RecalculateRow(GridEXRow row)
	{
		decimal num = Conversion.ToDecimal(row.Cells["ItemAmountWithoutDiscount"].Value);
		decimal discount = Conversion.ToDecimal(row.Cells["DiscountAmount"].Value);
		decimal itemAmount = num - discount;
		row.Cells["ItemAmount"].Value = itemAmount;
		decimal taxPercent = Conversion.ToDecimal(row.Cells["TaxPercent"].Value);
		row.Cells["TaxAmount"].Value = itemAmount * taxPercent / 100m;
		decimal expense = Conversion.ToDecimal(row.Cells["ExpenseAmount"].Value);
		decimal freights = Conversion.ToDecimal(row.Cells["Freights"].Value);
		row.Cells["BillAmount"].Value = itemAmount + expense + freights;
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = ((GridEX)((sender is GridEX) ? sender : null)).CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count != 0)
			{
				if (e.Column.Key == "PoAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["PurchaseOrderId"].Value), Conversion.ToInt(r.Cells["PurchaseOrderDocumentTypeId"].Value));
				}
				else if (e.Column.Key == "DemandAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["PurchaseDemandId"].Value), Conversion.ToInt(r.Cells["PurchaseDemandDocumentTypeId"].Value));
				}
				else if (e.Column.Key == "PreBillAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["PreBillId"].Value), Conversion.ToInt(r.Cells["PreBillDocumentTypeId"].Value));
				}
				else if (e.Column.Key == "DeliveryChallanAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["DeliveryChallanId"].Value), Conversion.ToInt(r.Cells["DeliveryChallanDocumentTypeId"].Value));
				}
				else if (e.Column.Key == "GrnAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["InvGrnId"].Value), Conversion.ToInt(r.Cells["InvGrnDocumentTypeId"].Value));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridGLSettings()
	{
		try
		{
			grdGLedger.RootTable.Columns["Credit"].AggregateFunction = (AggregateFunction)2;
			grdGLedger.RootTable.Columns["Debit"].AggregateFunction = (AggregateFunction)2;
			int[] excludedIds = new int[9] { 2, 11, 12, 13, 14, 15, 20, 21, 22 };
			DataTable dtAccountlst = DatatableHelper.GetAccountsFromGlobalByTypeIds(null, excludedIds);
			GridEX_Helper.ComboBind(grdGLedger.RootTable.Columns["AccountId"], dtAccountlst, "Id", "AccountTitle", "Account");
			GridEX_Helper.AddButton(grdGLedger, "Add", "+", 20, 0);
			GridEX_Helper.AddButton(grdGLedger, "Delete", "X", 20, 0);
			grdGLedger.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInGLGrid()
	{
		dtGrdGL.Rows.Add(0, "", 0, 0, 0, 0, 0);
		grdGLedger.DataSource = dtGrdGL;
	}

	private void grdGLedger_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		grdGLedger.UpdateData();
		GridEXRow item = grdGLedger.CurrentRow;
		if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item != null && (int)item.RowType == 0 && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
		{
			double Qty = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
			double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
			item.Cells["Credit"].Value = Qty * Rate;
			item.Cells["Debit"].Value = 0;
			item.Cells["Percentage"].Value = 0;
		}
		if (e.Column.Key == "Percentage" && item != null && (int)item.RowType == 0 && item.Cells["Percentage"].Value.ToString() != string.Empty)
		{
			double columnSum = GridEX_Helper.GetColumnSum(grd, "ItemAmount");
			double Percentage = Conversion.ToDouble((object)item.Cells["Percentage"].Value.ToString());
			double TotlaPercentage = columnSum / 100.0 * Percentage;
			if (TotlaPercentage > 0.0)
			{
				item.Cells["Credit"].Value = Math.Round(TotlaPercentage);
				item.Cells["Debit"].Value = 0;
			}
			else
			{
				item.Cells["Debit"].Value = Math.Abs(Math.Round(TotlaPercentage));
				item.Cells["Credit"].Value = 0;
			}
			item.Cells["Qty"].Value = 0;
			item.Cells["Rate"].Value = 0;
		}
		if (e.Column.Key == "Credit" && item != null && (int)item.RowType == 0 && Conversion.ToDouble((object)item.Cells["Debit"].Value.ToString()) > 0.0)
		{
			item.Cells["Credit"].Value = 0;
			MessageBox.Show("Debit Side is aleady added");
		}
		if (e.Column.Key == "Debit" && item != null && (int)item.RowType == 0 && Conversion.ToDouble((object)item.Cells["Credit"].Value.ToString()) > 0.0)
		{
			item.Cells["Debit"].Value = 0;
			MessageBox.Show("Credit Side is aleady added");
		}
		grdGLedger.UpdateData();
		BillAmount();
		ExpProportion();
	}

	private void grdGLedger_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdGLedger.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Column.Key == "Add")
				{
					AddRowInGLGrid();
				}
				if (e.Column.Key == "Delete")
				{
					dtGrdGL.Rows[item.RowIndex].Delete();
					item.Delete();
					if (dtGrdGL.Rows.Count == 0)
					{
						AddRowInGLGrid();
					}
				}
				grdGLedger.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		BillAmount();
	}

	private void grdFreightSettings()
	{
		try
		{
			grdFreight.RootTable.Columns["InvGrnId"].Visible = false;
			grdFreight.RootTable.Columns["Debit"].Visible = false;
			grdFreight.RootTable.Columns["Freight"].Caption = "Credit";
			grdFreight.RootTable.Columns["Freight"].Width = 60;
			grdFreight.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
			int[] excludedIds = new int[9] { 2, 11, 12, 13, 14, 15, 20, 21, 22 };
			DataTable dtAccountlst = DatatableHelper.GetAccountsFromGlobalByTypeIds(null, excludedIds);
			GridEX_Helper.ComboBind(grdFreight.RootTable.Columns["Transporter"], dtAccountlst, "Id", "AccountTitle", "Transporter");
			GridEX_Helper.AddButton(grdFreight, "Add", "+", 20, 0);
			GridEX_Helper.AddButton(grdFreight, "Delete", "X", 20, 0);
			grdFreight.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInFreightGrid()
	{
		dtFreight.Rows.Add(0, 0, 0, 0);
		grdFreight.DataSource = dtFreight;
	}

	private void grdFreight_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdFreight.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Add")
			{
				AddRowInFreightGrid();
			}
			if (e.Column.Key == "Delete")
			{
				if (Conversion.ToInt(item.Cells["InvGrnId"].Value) > 0)
				{
					throw new Exception("Record cannot be deleted because this record againt grn");
				}
				dtFreight.Rows[item.RowIndex].Delete();
				item.Delete();
				if (dtFreight.Rows.Count == 0)
				{
					AddRowInFreightGrid();
				}
			}
			grdFreight.UpdateData();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFreight_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		grdFreight.UpdateData();
		GridEXRow item = grdFreight.CurrentRow;
		if (e.Column.Key == "Freight" && item != null && (int)item.RowType == 0 && Conversion.ToDouble((object)item.Cells["Debit"].Value.ToString()) > 0.0)
		{
			item.Cells["Freight"].Value = 0;
			MessageBox.Show("Debit Side is aleady added");
		}
		if (e.Column.Key == "Debit" && item != null && (int)item.RowType == 0 && Conversion.ToDouble((object)item.Cells["Freight"].Value.ToString()) > 0.0)
		{
			item.Cells["Debit"].Value = 0;
			MessageBox.Show("Credit Side is aleady added");
		}
		grdFreight.UpdateData();
		BillAmount();
		FreightProportion();
	}

	private void grdFreight_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdFreight.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int Id = Conversion.ToInt(r.Cells["InvGrnId"].Value);
				grdFreight.AllowEdit = (InheritableBoolean)((Id == 0) ? 1 : 2);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AddRowInvExpGrid()
	{
		dtExpGrid.Rows.Add(0, 0, 0, 0, 0, "");
		grdInvExp.DataSource = dtExpGrid;
	}

	private void DeleteRowInExpenseGrid(GridEXRow item)
	{
		try
		{
			dtExpGrid.Rows[item.RowIndex].Delete();
			item.Delete();
			if (dtExpGrid.Rows.Count == 0)
			{
				AddRowInvExpGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExpenseGridCombBind()
	{
		try
		{
			GridEX_Helper.ComboBind(grdInvExp.RootTable.Columns["ItemId"], dtOtherItem, "Id", "OtherItemName", "Other Item Name");
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
			List<string> hiddenColumns = new List<string> { "Id", "OrderId", "OrderExpId" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"Qty",
					(EditType)1
				},
				{
					"Rate",
					(EditType)1
				},
				{
					"Amount",
					(EditType)1
				},
				{
					"Remarks",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdInvExp, 1, 3, hiddenColumns, editableColumns);
			ExpenseGridCombBind();
			grdInvExp.RootTable.Columns["Qty"].Width = 70;
			grdInvExp.RootTable.Columns["Rate"].Width = 70;
			grdInvExp.RootTable.Columns["Amount"].Width = 90;
			GridEX_Helper.AddButton(grdInvExp, "Delete", "X", 20, 0);
			GridEX_Helper.AddButton(grdInvExp, "Add", "+", 20, 1);
			grdInvExp.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdInvExp.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteRowInExpenseGrid(item);
				}
				if (e.Column.Key == "Add")
				{
					AddRowInvExpGrid();
				}
			}
			grdInvExp.UpdateData();
			BillAmount();
			ExpProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grdInvExp.CurrentRow;
			if (row != null && (int)row.RowType == 0)
			{
				switch (e.Column.Key)
				{
				default:
					return;
				case "Qty":
				case "Rate":
					UpdateAmount(row);
					break;
				case "Amount":
					UpdateRate(row);
					break;
				}
				grdInvExp.UpdateData();
				BillAmount();
				ExpProportion();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateAmount(GridEXRow row)
	{
		double qty = Conversion.ToDouble(row.Cells["Qty"].Value);
		double rate = Conversion.ToDouble(row.Cells["Rate"].Value);
		row.Cells["Amount"].Value = qty * rate;
	}

	private void UpdateRate(GridEXRow row)
	{
		double amount = Conversion.ToDouble(row.Cells["Amount"].Value);
		double qty = Conversion.ToDouble(row.Cells["Qty"].Value);
		row.Cells["Rate"].Value = ((qty == 0.0) ? 0.0 : (amount / qty));
	}

	private void grdInvExp_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow r = grdInvExp.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdInvExp.CurrentColumn != null && grdInvExp.CurrentColumn.Key == "Add")
				{
					AddRowInvExpGrid();
					grdInvExp.UpdateData();
				}
				if (grdInvExp.CurrentColumn != null && grdInvExp.CurrentColumn.Key == "Delete")
				{
					DeleteRowInExpenseGrid(r);
					grdInvExp.UpdateData();
				}
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteRowInExpenseGrid(r);
				grdInvExp.UpdateData();
			}
			else if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInvExpGrid();
				grdInvExp.UpdateData();
			}
			BillAmount();
			ExpProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbcommtype_Leave(object sender, EventArgs e)
	{
		BillAmount();
	}

	private void txtcommrate_TextChanged(object sender, EventArgs e)
	{
		BillAmount();
	}

	private void cmbcommuom_Leave(object sender, EventArgs e)
	{
		BillAmount();
	}

	private void BillAmount()
	{
		try
		{
			double itemAmountWithoutDiscount = Math.Round(GridEX_Helper.GetColumnSum(grd, "ItemAmountWithoutDiscount"));
			double itemDiscount = Math.Round(GridEX_Helper.GetColumnSum(grd, "DiscountAmount"));
			double itemNetAmount = Math.Round(GridEX_Helper.GetColumnSum(grd, "ItemAmount"));
			double taxAmount = Math.Round(GridEX_Helper.GetColumnSum(grd, "TaxAmount"));
			double itemPlusTax = itemNetAmount + taxAmount;
			txtItemAmountWithoutDiscountFooter.Text = itemAmountWithoutDiscount.ToString();
			if (base.ActiveControl == null || base.ActiveControl.AccessibleName != "ItemDiscountAmountFooter")
			{
				txtItemDiscountAmountFooter.Text = itemDiscount.ToString();
			}
			txtItemNetAmountFooter.Text = itemNetAmount.ToString();
			txtTaxAmountFooter.Text = taxAmount.ToString();
			txtItemAmountPlusTaxAmountFooter.Text = itemPlusTax.ToString();
			double totalExpense = Math.Round(GridEX_Helper.GetColumnSum(grdInvExp, "Amount"));
			txtExpenseAmount.Text = totalExpense.ToString();
			double billAmount = itemPlusTax + totalExpense;
			double journalDebit = 0.0;
			double journalCredit = 0.0;
			GridEXRow[] rows;
			if (((UltraGridBase)cmbsuppliername).ActiveRow != null)
			{
				rows = grdGLedger.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					int accountId = Conversion.ToInt(r2.Cells["AccountId"].Value);
					if (accountId > 0)
					{
						if (txtSupplierGLId.Text == accountId.ToString())
						{
							r2.BeginEdit();
							r2.Cells["AccountId"].Value = "0";
							throw new Exception("You cannot select supplier Account");
						}
						journalDebit += Conversion.ToDouble(r2.Cells["Debit"].Value);
						journalCredit += Conversion.ToDouble(r2.Cells["Credit"].Value);
					}
				}
			}
			double partyAddLess = journalDebit - journalCredit;
			txtPartyAddLessAmountFooter.Text = partyAddLess.ToString();
			billAmount += partyAddLess;
			bool eRPFeatureById = CommonServices.GetERPFeatureById(4);
			int supplierId = Conversion.ToInt(cmbsuppliername.Value);
			int supplierAltId = (eRPFeatureById ? supplierId : Conversion.ToInt((object)txtSupplierGLId.Text));
			double freightAmount = 0.0;
			rows = grdFreight.GetRows();
			foreach (GridEXRow r in rows)
			{
				int transporterId = Conversion.ToInt(r.Cells["Transporter"].Value);
				if (transporterId != 0 && transporterId == supplierAltId)
				{
					freightAmount += Conversion.ToDouble(r.Cells["Freight"].Value);
				}
			}
			txtFreightAmuontFooter.Text = freightAmount.ToString();
			billAmount += freightAmount;
			double billDiscountAmount = Conversion.ToDouble((object)txtBillDiscountAmountFooter.Text);
			billAmount -= billDiscountAmount;
			txtBillAmount.Text = Math.Round(billAmount).ToString();
			BillProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void FreightProportion()
	{
		try
		{
			double NetWeight = GridEX_Helper.GetColumnSum(grd, "ItemQty");
			double CreditForFreight = GridEX_Helper.GetColumnSum(grdFreight, "Freight");
			double DebitForFreight = GridEX_Helper.GetColumnSum(grdFreight, "Debit");
			if (DebitForFreight < CreditForFreight)
			{
				double TotalDiffernce = CreditForFreight - DebitForFreight;
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double Weight = Conversion.ToDouble((object)obj.Cells["ItemQty"].Value.ToString());
					obj.Cells["Freights"].Value = Math.Round(TotalDiffernce) / NetWeight * Weight;
				}
			}
			else
			{
				if (DebitForFreight < CreditForFreight)
				{
					double TotalDiffernce3 = CreditForFreight - DebitForFreight;
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow obj2 in rows)
					{
						obj2.BeginEdit();
						obj2.Cells["Freights"].Value = TotalDiffernce3;
					}
				}
				if (DebitForFreight == CreditForFreight)
				{
					double TotalDiffernce2 = CreditForFreight - DebitForFreight;
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow obj3 in rows)
					{
						obj3.BeginEdit();
						obj3.Cells["Freights"].Value = TotalDiffernce2;
					}
				}
				if (DebitForFreight > CreditForFreight)
				{
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow obj4 in rows)
					{
						obj4.BeginEdit();
						obj4.Cells["Freights"].Value = 0;
					}
				}
			}
			BillProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void BillProportion()
	{
		try
		{
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				r.BeginEdit();
				r.Cells["BillAmount"].Value = Conversion.ToDouble((object)r.Cells["ItemAmount"].Value.ToString()) + Conversion.ToDouble((object)r.Cells["Freights"].Value.ToString()) + Conversion.ToDouble((object)r.Cells["ExpenseAmount"].Value.ToString());
				r.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ItemDiscountProportion()
	{
		try
		{
			if (base.ActiveControl != null && !(base.ActiveControl.AccessibleName != "ItemDiscountAmountFooter"))
			{
				double TotalDiscount = Conversion.ToDouble((object)txtItemDiscountAmountFooter.Text);
				double totalQty = GridEX_Helper.GetColumnSum(grd, "ItemQty");
				bool hasExpense = TotalDiscount > 0.0 && totalQty > 0.0;
				double ratePerQty = (hasExpense ? (TotalDiscount / totalQty) : 0.0);
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					r.BeginEdit();
					double qty = Conversion.ToDouble(r.Cells["ItemQty"].Value);
					r.Cells["DiscountAmount"].Value = (hasExpense ? (ratePerQty * qty) : 0.0);
					RecalculateRow(r);
					r.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExpProportion()
	{
	}

	private void txtBillDiscountAmountFooter_TextChanged(object sender, EventArgs e)
	{
		BillAmount();
	}

	private void txtItemDiscountAmountFooter_TextChanged(object sender, EventArgs e)
	{
		ItemDiscountProportion();
		BillAmount();
	}

	private void txtdocno_TextChanged(object sender, EventArgs e)
	{
	}

	private void Insert()
	{
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Expected O, but got Unknown
		//IL_0693: Unknown result type (might be due to invalid IL or missing references)
		//IL_069a: Expected O, but got Unknown
		//IL_0906: Unknown result type (might be due to invalid IL or missing references)
		//IL_090d: Expected O, but got Unknown
		//IL_09f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fe: Expected O, but got Unknown
		//IL_0b4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b53: Expected O, but got Unknown
		try
		{
			if (grd.GetRows().Count() == 0)
			{
				throw new Exception("Detail Record Not Found");
			}
			if (!FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
			{
				((Control)(object)cmbsuppliername, "Supplier", FormHelper.ValidType.INT),
				(txtdocno, "Doc No", FormHelper.ValidType.INT),
				((Control)(object)CmbPaymentTerm, "Payment Term", FormHelper.ValidType.INT),
				(txtBillAmount, "Bill amount", FormHelper.ValidType.Double)
			}))
			{
				return;
			}
			int num = Conversion.ToInt(CmbPaymentTerm.Value);
			int dueDays = Conversion.ToInt((object)txtDueDays.Text);
			if (num == 2 && dueDays == 0)
			{
				MessageBox.Show("Due Days Field is Required");
				txtDueDays.Focus();
				return;
			}
			int discountAccountId = Conversion.ToInt(CmbDiscountAccount.Value);
			double discountAmount = (string.IsNullOrWhiteSpace(txtBillDiscountAmountFooter.Text) ? 0.0 : Conversion.ToDouble((object)txtBillDiscountAmountFooter.Text));
			if (discountAccountId > 0 || discountAmount > 0.0)
			{
				if (discountAccountId == 0)
				{
					MessageBox.Show("Discount Account Field is Required");
					CmbDiscountAccount.Focus();
					return;
				}
				if (discountAmount == 0.0)
				{
					MessageBox.Show("Discount Amount Field is Required");
					txtBillDiscountAmountFooter.Focus();
					return;
				}
			}
			bool hasTax = dtGrid.AsEnumerable().Any((DataRow x) => Conversion.ToInt(x["TaxNameId"]) > 0);
			if (hasTax && !FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
			{
				((Control)(object)CmbTaxAccount, "Tax Account", FormHelper.ValidType.INT),
				(txtInvoiceTaxNo, "Tax Invoice No", FormHelper.ValidType.INT)
			}))
			{
				return;
			}
			GridEXRow[] rows = grdGLedger.GetRows();
			foreach (GridEXRow obj2 in rows)
			{
				double debit = Conversion.ToDouble(obj2.Cells["Debit"].Value);
				double credit = Conversion.ToDouble(obj2.Cells["Credit"].Value);
				int accountId = Conversion.ToInt(obj2.Cells["AccountId"].Value);
				if ((debit > 0.0 || credit > 0.0) && accountId == 0)
				{
					MessageBox.Show("Please Select an Account Against JL First");
					return;
				}
			}
			rows = grdFreight.GetRows();
			foreach (GridEXRow obj3 in rows)
			{
				double freight = Conversion.ToDouble(obj3.Cells["Freight"].Value);
				int transporter = Conversion.ToInt(obj3.Cells["Transporter"].Value);
				if (freight > 0.0 && transporter == 0)
				{
					MessageBox.Show("Please Select an Account Against Freight First");
					return;
				}
			}
			double num2 = Conversion.ToDouble((object)txtcommamount.Text);
			int commissionAgentId = Conversion.ToInt(cmbcommagent.Value);
			if (num2 > 0.0 && commissionAgentId == 0)
			{
				MessageBox.Show("Please Select Commission Agent Account First");
			}
			else
			{
				if (!FormHelper.ConfirmAction((Id == 0) ? "Are you sure to Save" : "Are you sure to Update"))
				{
					return;
				}
				InvPurchaseInvoice obj = new InvPurchaseInvoice();
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.BranchesId = UserAccount.BranchesId;
				obj.ProjectsId = 0;
				obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				obj.DocumentTypeId = DocumentTypeId;
				obj.DocumentTypeSrNo = GrnBaseDocumentTypeId;
				obj.ScreenName = Conversion.ToString((object)ScreenName);
				obj.EntryDate = DateTime.Now;
				obj.ModifyDate = DateTime.Now;
				obj.EntryUser = UserAccount.ID;
				obj.ModifyUser = UserAccount.ID;
				obj.Id = Id;
				obj.DocNo = Conversion.ToInt((object)txtdocno.Text);
				obj.BranchSrNo = Conversion.ToInt((object)txtBranchSrNo.Text);
				obj.DocDate = DocDate.Value;
				obj.SupplierCustomerId = Conversion.ToInt(cmbsuppliername.Value);
				obj.ManualBillNo = txtbillno.Text.Trim();
				obj.SalesTaxNo = (hasTax ? Conversion.ToInt((object)txtInvoiceTaxNo.Text) : 0);
				obj.IsTaxable = hasTax;
				obj.ReferencePartyId = Conversion.ToInt(CmbTaxAccount.Value);
				obj.TaxAccountId = Conversion.ToInt(CmbTaxAccount.Value);
				obj.SupplierInvoiceDate = DueDate.Value;
				obj.DueDate = DueDate.Value;
				obj.DueDays = Conversion.ToInt((object)txtDueDays.Text);
				obj.PaymentTermsId = Conversion.ToInt(CmbPaymentTerm.Value);
				obj.SupplierInvoiceNo = Conversion.ToInt((object)txtDueDays.Text);
				obj.RemarksHeader = txtRemarks.Text.Trim();
				obj.CommissionAgentId = Conversion.ToInt(cmbcommagent.Value);
				obj.CommissionType = cmbcommtype.Text.Trim();
				obj.UomScheduleIdCmRate = Conversion.ToString((object)cmbcommuom.Text);
				obj.CommRate = Conversion.ToDouble((object)txtcommrate.Text);
				obj.CommAmount = Conversion.ToDouble((object)txtcommamount.Text);
				obj.CommissionRemarks = txtcommremarks.Text.Trim();
				obj.DiscountAccountId = Conversion.ToInt(CmbDiscountAccount.Value);
				obj.DiscountAmount = Conversion.ToDouble((object)txtBillDiscountAmountFooter.Text.Trim());
				obj.BillAmount = Conversion.ToDouble((object)txtBillAmount.Text.Trim());
				obj.invPurchaseInvoiceDetailList = new List<InvPurchaseInvoiceDetail>();
				obj.InvPurchaseInvoiceExpList = new List<InvPurchaseInvoiceExpense>();
				obj.InvPurchaseInvoiceFreightList = new List<InvPurchaseInvoiceFreight>();
				obj.InvPurchaseInvoicejournalList = new List<InvPurchaseInvoiceJournal>();
				int LineId = 1;
				rows = grd.GetRows();
				foreach (GridEXRow r4 in rows)
				{
					InvPurchaseInvoiceDetail vd = new InvPurchaseInvoiceDetail();
					vd.LineId = LineId;
					vd.Id = ((Id != 0) ? Conversion.ToInt(r4.Cells["Id"].Value) : 0);
					FillDetailListCommonForInsertAndDelete(vd, r4);
					FormHelper.ValidateField(vd.WarehouseId, "WareHouse", r4.RowIndex);
					FormHelper.ValidateField(vd.ItemId, "Item", r4.RowIndex);
					FormHelper.ValidateField(vd.ItemUOMId, "Pack Uom", r4.RowIndex);
					FormHelper.ValidateField(vd.ItemConditionId, "Item Condition", r4.RowIndex);
					FormHelper.ValidateField(vd.RackId, "Rack Name", r4.RowIndex);
					FormHelper.ValidateField(vd.ItemQty, "Qty", r4.RowIndex);
					FormHelper.ValidateField(vd.ItemRate, "Rate", r4.RowIndex);
					FormHelper.ValidateField(vd.ItemAmount, "Amount", r4.RowIndex);
					if (vd.TaxNameId > 0 || vd.TaxPercent > 0.0 || vd.TaxAmount > 0.0)
					{
						FormHelper.ValidateField(vd.TaxNameId, "Tax Name", r4.RowIndex);
						FormHelper.ValidateField(vd.TaxPercent, "Tax Percent", r4.RowIndex);
						FormHelper.ValidateField(vd.TaxAmount, "Tax Amount", r4.RowIndex);
					}
					FormHelper.ValidateField(vd.BillAmount, "Bill Amount", r4.RowIndex);
					obj.invPurchaseInvoiceDetailList.Add(vd);
					LineId++;
				}
				rows = grdFreight.GetRows();
				foreach (GridEXRow r3 in rows)
				{
					if (Conversion.ToInt((object)r3.Cells["Transporter"].Value.ToString()) != 0)
					{
						InvPurchaseInvoiceFreight PF = new InvPurchaseInvoiceFreight();
						PF.InvGrnId = Conversion.ToInt(r3.Cells["InvGrnId"].Value);
						PF.TansporterId = Conversion.ToInt(r3.Cells["Transporter"].Value);
						PF.FreightAmount = Conversion.ToInt(r3.Cells["Freight"].Value);
						PF.Debit = Conversion.ToInt(r3.Cells["Debit"].Value);
						obj.InvPurchaseInvoiceFreightList.Add(PF);
					}
				}
				rows = grdGLedger.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					if (Conversion.ToInt((object)r2.Cells["AccountId"].Value.ToString()) != 0)
					{
						InvPurchaseInvoiceJournal PJ = new InvPurchaseInvoiceJournal();
						PJ.ChartofAccountId = Conversion.ToInt(r2.Cells["AccountId"].Value);
						PJ.JvRemarks = Conversion.ToString(r2.Cells["Remarks"].Value);
						PJ.JvPrcnt = Conversion.ToDouble(r2.Cells["Percentage"].Value);
						PJ.JvQty = Conversion.ToDouble(r2.Cells["Qty"].Value);
						PJ.JvRate = Conversion.ToDouble(r2.Cells["Rate"].Value);
						PJ.JvDebit = Conversion.ToDouble(r2.Cells["Debit"].Value);
						PJ.JvCredit = Conversion.ToDouble(r2.Cells["Credit"].Value);
						obj.InvPurchaseInvoicejournalList.Add(PJ);
					}
				}
				rows = grdInvExp.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.ToInt((object)r.Cells["ItemId"].Value.ToString()) != 0)
					{
						InvPurchaseInvoiceExpense PE = new InvPurchaseInvoiceExpense();
						if (Conversion.ToDouble(r.Cells["Amount"].Value) == 0.0)
						{
							throw new Exception($"Amount Field Required in Expense Grid Row no : {r.RowIndex + 1}");
						}
						PE.Id = Conversion.ToInt(r.Cells["Id"].Value);
						PE.PurchaseOrderId = 0;
						PE.PurchaseOrderSupplierExpId = 0;
						PE.InvRevExpItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						PE.OtherItemName = Conversion.ToString((object)r.Cells["ItemId"].Text);
						PE.Qty = Conversion.ToInt(r.Cells["Qty"].Value);
						PE.Rate = Conversion.ToDouble(r.Cells["Rate"].Value);
						PE.Amount = Conversion.ToDouble(r.Cells["Amount"].Value);
						PE.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
						obj.InvPurchaseInvoiceExpList.Add(PE);
					}
				}
				obj.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				obj.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<InvPurchaseInvoice>(obj, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
				obj.AttachmentsValues = AttachmentsValues;
				obj.CustomAttachmentsValues = CustomAttachmentsValues;
				int sucess = InvPurchaseInvoice.Save(obj);
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
				if (ChkBok.Checked)
				{
					OpenSlip(sucess, DocumentTypeId);
				}
				if (chkVoucherPreview.Checked)
				{
					CommonServices.AcRptPurchaseSalesVoucherSlip_103(CommonServices.VoucherHeadIdGet(sucess, DocumentTypeId), DocumentTypeId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void FillDetailListCommonForInsertAndDelete(InvPurchaseInvoiceDetail pd, GridEXRow r)
	{
		pd.InvGrnDetailId = Conversion.ToInt(r.Cells["InvGrnDetailId"].Value);
		pd.InvGrnId = Conversion.ToInt(r.Cells["InvGrnId"].Value);
		pd.GrnNo = Conversion.ToInt(r.Cells["GrnNo"].Value);
		pd.PurchaseOrderDocumentTypeId = Conversion.ToInt(r.Cells["PurchaseOrderDocumentTypeId"].Value);
		pd.PurchaseOrderId = Conversion.ToInt(r.Cells["PurchaseOrderId"].Value);
		pd.PurchaseOrder = Conversion.ToInt(r.Cells["PurchaseOrderNo"].Value);
		pd.PurchaseDemandDocumentTypeId = Conversion.ToInt(r.Cells["PurchaseDemandDocumentTypeId"].Value);
		pd.PurchaseDemandId = Conversion.ToInt(r.Cells["PurchaseDemandId"].Value);
		pd.PurchaseDemandNo = Conversion.ToInt(r.Cells["PurchaseDemandNo"].Value);
		pd.PreBillDocumentTypeId = Conversion.ToInt(r.Cells["PreBillDocumentTypeId"].Value);
		pd.PreBillId = Conversion.ToInt(r.Cells["PreBillId"].Value);
		pd.PreBillNo = Conversion.ToInt(r.Cells["PreBillNo"].Value);
		pd.DeliveryChallanDocumentTypeId = Conversion.ToInt(r.Cells["DeliveryChallanDocumentTypeId"].Value);
		pd.DeliveryChallanId = Conversion.ToInt(r.Cells["DeliveryChallanId"].Value);
		pd.DeliveryChallanNo = Conversion.ToInt(r.Cells["DeliveryChallanNo"].Value);
		pd.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
		pd.WareHouseName = Conversion.ToString(r.Cells["WareHouseName"].Value);
		pd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
		pd.ItemName = Conversion.ToString(r.Cells["ItemName"].Value);
		pd.ItemUOMId = Conversion.ToInt(r.Cells["ItemUomId"].Value);
		pd.UOMCodeItem = Conversion.ToString(r.Cells["UOMCodeItem"].Value);
		pd.ItemConditionId = Conversion.ToInt(r.Cells["ItemConditionId"].Value);
		pd.ItemCondition = Conversion.ToString(r.Cells["ItemCondition"].Value);
		pd.RackId = FormHelper.GetInt(r, "RackId");
		pd.rackName = FormHelper.GetString(r, "RackName");
		double qty = (pd.NetBillWeight = (pd.GrossWeight = (pd.ItemQty = Conversion.ToDouble(r.Cells["ItemQty"].Value))));
		pd.NetStockWeight = qty;
		pd.ItemRate = Conversion.ToDouble(r.Cells["Rate"].Value);
		pd.ItemAmountWithoutDiscount = Conversion.ToDecimal(r.Cells["ItemAmountWithoutDiscount"].Value);
		pd.DiscountAmount = Conversion.ToDouble(r.Cells["DiscountAmount"].Value);
		pd.ItemAmount = Conversion.ToDouble(r.Cells["ItemAmount"].Value);
		pd.TaxNameId = Conversion.ToInt(r.Cells["TaxNameId"].Value);
		pd.TaxDescriptions = Conversion.ToString(r.Cells["TaxName"].Value);
		pd.TaxPercent = Conversion.ToDouble(r.Cells["TaxPercent"].Value);
		pd.TaxAmount = Conversion.ToDouble(r.Cells["TaxAmount"].Value);
		pd.BillAmount = Conversion.ToDouble(r.Cells["BillAmount"].Value);
		pd.FreightAmount = Conversion.ToDouble(r.Cells["Freights"].Value);
		pd.ExpenseAmount = Conversion.ToDouble(r.Cells["ExpenseAmount"].Value);
		pd.NoOfAttachmentsPO = Conversion.ToInt(r.Cells["PoAttachments"].Value);
		pd.NoOfAttachmentsPD = Conversion.ToInt(r.Cells["DemandAttachments"].Value);
		pd.NoOfAttachmentsPreBill = Conversion.ToInt(r.Cells["PreBillAttachments"].Value);
		pd.NoOfAttachmentsDC = Conversion.ToInt(r.Cells["DeliveryChallanAttachments"].Value);
		pd.GrnDocumentTypeId = Conversion.ToInt(r.Cells["InvGrnDocumentTypeId"].Value);
		pd.DeliveryChallanDocumentTypeId = Conversion.ToInt(r.Cells["DeliveryChallanDocumentTypeId"].Value);
		pd.PreBillDocumentTypeId = Conversion.ToInt(r.Cells["PreBillDocumentTypeId"].Value);
		pd.PurchaseDemandDocumentTypeId = Conversion.ToInt(r.Cells["PurchaseDemandDocumentTypeId"].Value);
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
				throw new Exception("Record Not Update because Record has approved");
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

	public void ReadById(int ID)
	{
		try
		{
			Reset();
			Id = ID;
			InvPurchaseInvoice Or = InvPurchaseInvoice.GetByID(ID);
			if (Or == null || Or.invPurchaseInvoiceDetailList == null)
			{
				MessageBox.Show("No Record Found");
				return;
			}
			tabControl1.SelectedIndex = 0;
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
			txtBranchSrNo.Text = Conversion.ToString((object)Or.BranchSrNo);
			txtdocno.Text = Or.DocNo.ToString();
			DocDate.Value = Or.DocDate;
			cmbsuppliername.Value = Or.SupplierCustomerId;
			txtbillno.Text = Or.ManualBillNo;
			CmbTaxAccount.Value = Or.ReferencePartyId;
			txtInvoiceTaxNo.Text = Conversion.ToString((object)Or.SalesTaxNo);
			CmbPaymentTerm.Value = Conversion.ToInt((object)Or.PaymentTermsId);
			txtDueDays.Text = Conversion.ToString((object)Or.SupplierInvoiceNo);
			DueDate.Value = Or.SupplierInvoiceDate;
			txtBillAmount.Text = Conversion.ToString((object)Or.BillAmount);
			txtRemarks.Text = Conversion.ToString((object)Or.RemarksHeader);
			cmbcommagent.Value = Or.CommissionAgentId;
			cmbcommtype.Text = Conversion.ToString((object)Or.CommissionType);
			txtcommrate.Text = Conversion.ToString((object)Or.CommRate);
			cmbcommuom.Text = Conversion.ToString((object)Or.UomScheduleIdCmRate);
			txtcommamount.Text = Conversion.ToString((object)Or.CommAmount);
			txtcommremarks.Text = Conversion.ToString((object)Or.CommissionRemarks);
			CmbDiscountAccount.Value = Conversion.ToInt((object)Or.DiscountAccountId);
			txtBillDiscountAmountFooter.Text = Conversion.ToString((object)Or.DiscountAmount);
			Approved = Or.IsApproved;
			GrnBaseDocumentTypeId = Or.DocumentTypeSrNo;
			AttachmentsValues = Or.AttachmentsValues;
			CustomAttachmentsValues = Or.CustomAttachmentsValues;
			VoucherHeadIdGet();
			dtGrid.Rows.Clear();
			frmPurchaseInvoiceStoreManagement_Helper.FillDetailFromListCommonForReadById(Or, dtGrid);
			dtFreight.Rows.Clear();
			for (int j = 0; j < Or.InvPurchaseInvoiceFreightList.Count; j++)
			{
				dtFreight.Rows.Add(Or.InvPurchaseInvoiceFreightList[j].InvGrnId, Or.InvPurchaseInvoiceFreightList[j].TansporterId, Or.InvPurchaseInvoiceFreightList[j].FreightAmount, Or.InvPurchaseInvoiceFreightList[j].Debit);
			}
			dtGrdGL.Rows.Clear();
			for (int i = 0; i < Or.InvPurchaseInvoicejournalList.Count; i++)
			{
				dtGrdGL.Rows.Add(Or.InvPurchaseInvoicejournalList[i].ChartofAccountId, Or.InvPurchaseInvoicejournalList[i].JvRemarks, Or.InvPurchaseInvoicejournalList[i].JvPrcnt, Or.InvPurchaseInvoicejournalList[i].JvQty, Or.InvPurchaseInvoicejournalList[i].JvRate, Or.InvPurchaseInvoicejournalList[i].JvDebit, Or.InvPurchaseInvoicejournalList[i].JvCredit);
			}
			dtExpGrid.Rows.Clear();
			foreach (InvPurchaseInvoiceExpense d in Or.InvPurchaseInvoiceExpList)
			{
				dtExpGrid.Rows.Add(d.Id, d.InvRevExpItemId, d.Qty, d.Rate, d.Amount, d.Remarks);
			}
			BindGrids();
			FormHelper.LoadAttachmentsForObject(AT, Id, base.Name);
			BillAmount();
			FreightProportion();
			ExpProportion();
			BillProportion();
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

	private void btnSave_Click(object sender, EventArgs e)
	{
		try
		{
			Id = 0;
			Insert();
		}
		catch (Exception ex)
		{
			waitForm.Close();
			MessageBox.Show(ex.Message);
		}
	}

	private void PurchaseInvoiceStoreManagement_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnNew_Click(object sender, EventArgs e)
	{
		try
		{
			waitForm.Show();
			Application.DoEvents();
			Reset();
			waitForm.Close();
		}
		catch (Exception ex)
		{
			waitForm.Close();
			MessageBox.Show(ex.Message);
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
			Approved = false;
			Id = 0;
			GrnBaseDocumentTypeId = 0;
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			((Control)(object)cmbsuppliername).Enabled = true;
			((Control)(object)cmbsuppliername).Text = string.Empty;
			txtbillno.Text = string.Empty;
			((Control)(object)CmbPaymentTerm).Text = string.Empty;
			txtDueDays.Text = "0";
			txtRemarks.Text = string.Empty;
			((Control)(object)cmbcommagent).Text = string.Empty;
			cmbcommtype.Text = string.Empty;
			txtcommrate.Text = string.Empty;
			txtcommamount.Text = string.Empty;
			cmbcommuom.Text = string.Empty;
			txtcommremarks.Text = string.Empty;
			txtDeliveryTerm.Text = string.Empty;
			txtItemAmountWithoutDiscountFooter.Text = string.Empty;
			txtItemDiscountAmountFooter.Text = string.Empty;
			txtItemNetAmountFooter.Text = string.Empty;
			((Control)(object)CmbTaxAccount).Text = string.Empty;
			txtTaxAmountFooter.Text = string.Empty;
			txtItemAmountPlusTaxAmountFooter.Text = string.Empty;
			txtExpenseAmount.Text = string.Empty;
			((Control)(object)CmbDiscountAccount).Text = string.Empty;
			txtBillDiscountAmountFooter.Text = string.Empty;
			txtFreightAmuontFooter.Text = string.Empty;
			txtPartyAddLessAmountFooter.Text = string.Empty;
			txtBillAmount.Text = string.Empty;
			dtGrid.Rows.Clear();
			dtGrdGL.Rows.Clear();
			dtFreight.Rows.Clear();
			dtExpGrid.Rows.Clear();
			BindGrids();
			DocDate.Focus();
			txtdocno.Text = Conversion.ToString((object)CommonServices.PurchaseInvoiceGenerateCode(DocumentTypeId));
			txtInvoiceTaxNo.Text = Conversion.ToString((object)CommonServices.GenerateSalesTaxNo(IsTaxable: true));
			txtBranchSrNo.Text = Conversion.ToString((object)CommonServices.PurchaseInvoiceGenerateCodeBranch(DocumentTypeId, UserAccount.BranchesId));
			DocDate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnFrmRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			waitForm.Show();
			Application.DoEvents();
			List<string> allServices = new List<string> { "BranchesAllocatedToUser", "PaymentTerm", "AccountsWithCustomGroupId", "SupplierCustomerLists", "VendorsAndCustomersForTransporters" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			GetConfigurationsFromGlobal();
			SupplierDtFillFromGlobal();
			BindSupplierName();
			PaymentTermBindFromGlobal();
			TaxAccountFillFromGlobal();
			OtherItemdtDbCall();
			GetConfigurationsFromGlobalandBind();
			waitForm.Close();
		}
		catch (Exception ex)
		{
			waitForm.Close();
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton3_Click_1(object sender, EventArgs e)
	{
		try
		{
			using frmPendingGrnStoreLoader LoadOrder = new frmPendingGrnStoreLoader(UserAccount);
			LoadOrder.ShowDialog();
			LoadInGridDetailFromGrn(LoadOrder.dtLoader);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridDetailFromGrn(DataTable dtLoader)
	{
		try
		{
			if (dtLoader == null || dtLoader.Rows.Count == 0)
			{
				if (grd.GetRows().Count() == 0)
				{
					GrnBaseDocumentTypeId = 0;
				}
				return;
			}
			string FrtIds = string.Join(",", (from r in dtLoader.AsEnumerable()
				where r["Id"] != DBNull.Value
				select Conversion.ToInt(r["Id"])).Distinct());
			string DeliveryChallanIds = string.Join(",", (from r in dtLoader.AsEnumerable()
				where r["DeliveryChallanId"] != DBNull.Value
				select Conversion.ToInt(r["DeliveryChallanId"])).Distinct());
			DataRow firstLoaderRow = dtLoader.Rows[0];
			int currentPartyId = Conversion.ToInt(firstLoaderRow["SupplierCustomerId"]);
			int newBaseDocumentTypeId = Conversion.ToInt(firstLoaderRow["BaseDocumentTypeId"]);
			if (GrnBaseDocumentTypeId > 0 && GrnBaseDocumentTypeId != newBaseDocumentTypeId)
			{
				string oldDoc = ((GrnBaseDocumentTypeId == 1) ? "Grn Against Purchase_Order" : ((GrnBaseDocumentTypeId == 2) ? "Grn Against Purchase_Demand" : ((GrnBaseDocumentTypeId == 3) ? "Grn Against DeliveryChallan" : "")));
				string newDoc = newBaseDocumentTypeId switch
				{
					3 => "Grn Against DeliveryChallan", 
					2 => "Grn Against Purchase_Demand", 
					1 => "Grn Against Purchase_Order", 
					_ => "", 
				};
				MessageBox.Show("Data of " + oldDoc + " already exists. Please Reset the Form before loading data for " + newDoc + ".", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			GrnBaseDocumentTypeId = newBaseDocumentTypeId;
			if (grd.GetRows().Count() > 0)
			{
				int currentOrderId = ((GrnBaseDocumentTypeId == 1) ? Conversion.ToInt(firstLoaderRow["PurchaseOrderHeaderId"]) : ((GrnBaseDocumentTypeId == 2 || GrnBaseDocumentTypeId == 3) ? Conversion.ToInt(firstLoaderRow["PurchaseDemandHeaderId"]) : 0));
				GridEXRow firstGridRow = grd.GetDataRows().FirstOrDefault();
				int existingOrderId = ((firstGridRow != null) ? Conversion.ToInt(firstGridRow.Cells["DemondId"].Value) : 0);
				if (existingOrderId != 0 && existingOrderId != currentOrderId)
				{
					string oldDoc2 = ((GrnBaseDocumentTypeId == 1) ? "Grn Against Purchase_Order" : ((GrnBaseDocumentTypeId == 2) ? "Grn Against Purchase_Demand" : ((GrnBaseDocumentTypeId == 3) ? "Grn Against DeliveryChallan" : "")));
					MessageBox.Show("Data for " + oldDoc2 + " already exists. Please Reset the Form before loading data for another type of Grn.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				int existingPartyId = Conversion.ToInt(cmbsuppliername.Value);
				if (existingPartyId != 0 && existingPartyId != currentPartyId)
				{
					MessageBox.Show("Data for another party already exists. Please Reset the Form before loading data for a different party.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}
			cmbsuppliername.Value = currentPartyId;
			((Control)(object)cmbsuppliername).Enabled = false;
			txtRemarks.Text = Conversion.ToString(firstLoaderRow["RemarksHeader"]);
			txtBillDiscountAmountFooter.Text = Conversion.ToDouble(firstLoaderRow["PurchasePreBillBillDiscount"]).ToString("#,##.###");
			HashSet<int> existingChallanDetailIds = new HashSet<int>();
			foreach (DataRow r2 in dtGrid.Rows)
			{
				existingChallanDetailIds.Add(Conversion.ToInt(r2["InvGrnDetailId"]));
			}
			foreach (DataRow row in dtLoader.Rows)
			{
				int detailId = Conversion.ToInt(row["DetailId"]);
				if (!existingChallanDetailIds.Contains(detailId))
				{
					existingChallanDetailIds.Add(detailId);
					DataRow dr = dtGrid.NewRow();
					dr["Id"] = 0;
					dr["InvGrnId"] = row["Id"];
					dr["InvGrnDetailId"] = detailId;
					dr["InvGrnDocumentTypeId"] = row["DocumentTypeId"];
					dr["GrnNo"] = row["DocNo"];
					dr["PurchaseOrderDocumentTypeId"] = row["PurchaseOrderDocumentTypeId"];
					dr["PurchaseOrderId"] = row["PurchaseOrderHeaderId"];
					dr["PurchaseOrderNo"] = row["PurchaseOrderNo"];
					dr["PurchaseDemandDocumentTypeId"] = row["PurchaseDemandDocumentTypeId"];
					dr["PurchaseDemandId"] = row["PurchaseDemandHeaderId"];
					dr["PurchaseDemandNo"] = row["PurchaseDemandNo"];
					dr["PreBillDocumentTypeId"] = row["PurchasePreBillDocumentTypeId"];
					dr["PreBillId"] = row["PurchasePreBillId"];
					dr["PreBillNo"] = row["PurchasePreBillNo"];
					dr["DeliveryChallanDocumentTypeId"] = row["DeliveryChallanDocumentTypeId"];
					dr["DeliveryChallanId"] = row["DeliveryChallanId"];
					dr["DeliveryChallanNo"] = row["DeliveryChallanNo"];
					dr["ItemId"] = row["ItemId"];
					dr["ItemName"] = row["ItemName"];
					dr["ItemUomId"] = row["UomId"];
					dr["UOMCodeItem"] = row["Uom"];
					double itemQty = Conversion.ToDouble(row["ItemQty"]);
					dr["ItemQty"] = itemQty;
					dr["WarehouseId"] = row["WareHouseId"];
					dr["WareHouseName"] = row["WareHouseName"];
					dr["ItemConditionId"] = row["ItemConditionId"];
					dr["ItemCondition"] = row["ItemCondition"];
					dr["RackId"] = row["RackId"];
					dr["RackName"] = row["rackName"];
					double rate = Conversion.ToDouble(row["ItemRate"]);
					dr["Rate"] = rate;
					double PurchasePreBillItemDiscount = Conversion.ToDouble(row["PurchasePreBillItemDiscount"]);
					dr["DiscountAmount"] = PurchasePreBillItemDiscount;
					double ItemAmountWithoutDiscount = Conversion.ToDouble(row["ItemAmount"]);
					dr["ItemAmountWithoutDiscount"] = ItemAmountWithoutDiscount;
					double itemAmount = ItemAmountWithoutDiscount - PurchasePreBillItemDiscount;
					dr["ItemAmount"] = itemAmount;
					dr["TaxNameId"] = row["PurchaseOrderTaxNameId"];
					dr["TaxName"] = row["PurchaseOrderTaxName"];
					double taxPercent = Conversion.ToDouble(row["PurchaseOrderTaxPercent"]);
					dr["TaxPercent"] = taxPercent;
					double taxAmount = itemAmount * taxPercent / 100.0;
					dr["TaxAmount"] = taxAmount;
					dr["BillAmount"] = 0;
					dr["Freights"] = 0;
					dr["ExpenseAmount"] = 0;
					dr["GrnAttachments"] = row["NoOfAttachments"];
					dr["PoAttachments"] = row["NoOfAttachmentsPO"];
					dr["DemandAttachments"] = row["NoOfAttachmentsPD"];
					dr["PreBillAttachments"] = row["NoOfAttachmentsPreBill"];
					dr["DeliveryChallanAttachments"] = row["NoOfAttachmentsDC"];
					dtGrid.Rows.Add(dr);
				}
			}
			LoadFreightData(FrtIds);
			LoadExpData(DeliveryChallanIds);
			BindGrids();
			BillAmount();
			FreightProportion();
			ExpProportion();
			BillProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void LoadFreightData(string ID)
	{
		DataTable item = InvPurchaseInvoice.GetTransporterAndFreightFromGrn(ID);
		if (item == null || item.Rows.Count <= 0)
		{
			return;
		}
		dtFreight.Rows.Clear();
		foreach (DataRow row in item.Rows)
		{
			int mainId = row.Field<int?>("MainId").GetValueOrDefault();
			int transporter = row.Field<int?>("Transporter").GetValueOrDefault();
			double carriageAmount = row.Field<double?>("CarriageAmount").GetValueOrDefault();
			dtFreight.Rows.Add(mainId, transporter, carriageAmount, 0);
		}
	}

	private void LoadExpData(string poIds)
	{
		try
		{
			if (string.IsNullOrWhiteSpace(poIds))
			{
				return;
			}
			DataTable expItems = DeliveryChallanHeader.DeliveryChallanHeader_ExpenseDetailByIds(poIds);
			if (expItems == null || expItems.Rows.Count == 0)
			{
				return;
			}
			dtExpGrid.Clear();
			foreach (DataRow row in expItems.Rows)
			{
				DataRow newRow = dtExpGrid.NewRow();
				newRow["Id"] = 0;
				newRow["ItemId"] = row["OtherItemId"];
				newRow["Qty"] = Conversion.ToDouble(row["Qty"]);
				newRow["Rate"] = Conversion.ToDouble(row["Rate"]);
				newRow["Amount"] = Conversion.ToDouble(row["Amount"]);
				newRow["Remarks"] = Conversion.ToString(row["Remarks"]);
				dtExpGrid.Rows.Add(newRow);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message, ex);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 1)
			{
				DocDate.Focus();
			}
			else
			{
				FromDateHistory.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryBranchComboDbCall()
	{
		try
		{
			dtBranch.Clear();
			if (BranchImplemented)
			{
				dtBranch.Rows.Add(UserAccount.BranchesId, UserAccount.BranchName);
				return;
			}
			DataTable dt = InvPurchaseInvoice.GetBranchsAllocatedToUserFromPurchaseInvoice(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, DocumentTypeId);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtBranch.Rows.Add(dt.Rows[i]["BranchId"], dt.Rows[i]["BranchName"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryBranchComboBind()
	{
		try
		{
			((UltraGridBase)cmbBranchName).DataSource = null;
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

	private DataTable HistoryComboDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		try
		{
			return InvPurchaseInvoice.AllComboBindAgainstPurchaseInvoice(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				AppId = UserAccount.AppId,
				UserId = UserAccount.ID,
				DocumentTypeIds = DocumentTypeId.ToString()
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void HistoryComboBind(DataTable dt)
	{
		try
		{
			DataTable dtSupplier = new DataTable();
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("Name", typeof(string));
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Supplier")
					{
						dtSupplier.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbCustomerHistory, dtSupplier, "Id", "Name", "Supplier Name", AllColumns: true, Conversion.ToInt(CmbCustomerHistory.Value), insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryBranchComboDbCall();
			HistoryBranchComboBind();
			HistoryComboBind(HistoryComboDbCall());
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
			FromDocNoHistory.Text = string.Empty;
			ToDocNoHistory.Text = string.Empty;
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			grdHistory.ClearStructure();
			grdDetail.ClearStructure();
			drdocdate.Checked = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShowHistory_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "")
			{
				FromDateHistory.Focus();
			}
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
			GetAll();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetAll()
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		try
		{
			if (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) == string.Empty)
			{
				cmbBranchName.Focus();
				throw new Exception("Select branch first");
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = DocumentTypeId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
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
			string BranchIds = InfragisticsHelper.GetSelectedIdsFromMultiSelectionCombo(Conversion.ToString((object)((Control)(object)cmbBranchName).Text), dtBranch, "BranchName");
			obj.BranchesIds = BranchIds;
			DataTable dthistoryfromdb = InvPurchaseInvoice.FormHistory(obj);
			if (dthistoryfromdb.Rows.Count > 0)
			{
				DataTable dataTable = dthistory;
				if (dataTable != null && dataTable.Columns.Count == 0)
				{
					dthistory.Columns.Add("Id", typeof(int));
					dthistory.Columns.Add("DocumentTypeId", typeof(int));
					dthistory.Columns.Add("DocNo", typeof(int));
					dthistory.Columns.Add("OrderNo", typeof(int));
					dthistory.Columns.Add("DocDate", typeof(DateTime));
					dthistory.Columns.Add("BranchSrNo", typeof(int));
					dthistory.Columns.Add("BranchName", typeof(string));
					dthistory.Columns.Add("PurchaseAgainst", typeof(string));
					dthistory.Columns.Add("DueDays", typeof(int));
					dthistory.Columns.Add("DueDate", typeof(DateTime));
					dthistory.Columns.Add("ManualBillNo", typeof(string));
					dthistory.Columns.Add("SupplierName", typeof(string));
					dthistory.Columns.Add("ReferencePartyName", typeof(string));
					dthistory.Columns.Add("CommissionAgent", typeof(string));
					dthistory.Columns.Add("CommissionType", typeof(string));
					dthistory.Columns.Add("CommRate", typeof(double));
					dthistory.Columns.Add("CommAmount", typeof(double));
					dthistory.Columns.Add("CommissionRemarks", typeof(string));
					dthistory.Columns.Add("BillAmount", typeof(double));
					dthistory.Columns.Add("CurrencyName", typeof(string));
					dthistory.Columns.Add("ExchangeRate", typeof(double));
					dthistory.Columns.Add("FcyAmount", typeof(double));
					dthistory.Columns.Add("BillType", typeof(string));
					dthistory.Columns.Add("ApprovedStatus", typeof(string));
					dthistory.Columns.Add("EntryUser", typeof(string));
					dthistory.Columns.Add("EntryDate", typeof(DateTime));
					dthistory.Columns.Add("ModifyUser", typeof(string));
					dthistory.Columns.Add("ModifyDate", typeof(DateTime));
					dthistory.Columns.Add("ApprovedUser", typeof(string));
					dthistory.Columns.Add("ApprovedDate", typeof(DateTime));
					dthistory.Columns.Add("RemarksHeader", typeof(string));
					dthistory.Columns.Add("NoOfAttachments", typeof(int));
				}
				dthistory.Clear();
				HashSet<int> addedIds = new HashSet<int>();
				foreach (DataRow r in dthistoryfromdb.Rows)
				{
					int id = Conversion.ToInt(r["Id"]);
					if (addedIds.Add(id))
					{
						dthistory.Rows.Add(r["Id"], r["DocumentTypeId"], r["DocNo"], r["OrderNo"], Conversion.ToDateTime(r["DocDate"]), r["BranchSrNo"], r["BranchName"], r["PurchaseAgainst"], r["DueDays"], Conversion.ToDateTime(r["DueDate"]), r["ManualBillNo"], r["SupplierName"], r["ReferencePartyName"], r["CommissionAgent"], r["CommissionType"], Conversion.ToDouble(r["CommRate"]), Conversion.ToDouble(r["CommAmount"]), r["CommissionRemarks"], Conversion.ToDouble(r["BillAmount"]), r["CurrencyName"], Conversion.ToDouble(r["ExchangeRate"]), Conversion.ToDouble(r["FcyAmount"]), r["BillType"], r["ApprovedStatus"], r["EntryUser"], r["EntryDate"], r["ModifyUser"], r["ModifyDate"], r["ApprovedUser"], r["PostDate"], r["RemarksHeader"], Conversion.ToInt(r["NoOfAttachments"]));
					}
				}
				grdHistory.DataSource = dthistory;
				grdHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				grdHistory.DataSource = null;
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
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdHistory.RootTable.Columns["OrderNo"].Visible = false;
			grdHistory.RootTable.Columns["PurchaseAgainst"].Visible = false;
			grdHistory.RootTable.Columns["CommissionAgent"].Visible = false;
			grdHistory.RootTable.Columns["CommissionType"].Visible = false;
			grdHistory.RootTable.Columns["CommRate"].Visible = false;
			grdHistory.RootTable.Columns["CommAmount"].Visible = false;
			grdHistory.RootTable.Columns["CommissionRemarks"].Visible = false;
			grdHistory.RootTable.Columns["FcyAmount"].Visible = false;
			grdHistory.RootTable.Columns["ExchangeRate"].Visible = false;
			grdHistory.RootTable.Columns["BranchSrNo"].Visible = !BranchImplemented;
			grdHistory.RootTable.Columns["BranchName"].Visible = !BranchImplemented;
			GridEX_Helper.GridWrappingAndColumnSettings(grdHistory, 2, 3);
			grdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["DocNo"].Width = 55;
			grdHistory.RootTable.Columns["OrderNo"].Width = 65;
			grdHistory.RootTable.Columns["DocDate"].Width = 85;
			grdHistory.RootTable.Columns["PurchaseAgainst"].Width = 120;
			grdHistory.RootTable.Columns["DueDays"].Width = 65;
			grdHistory.RootTable.Columns["DueDate"].Width = 85;
			grdHistory.RootTable.Columns["ManualBillNo"].Width = 100;
			grdHistory.RootTable.Columns["SupplierName"].Width = 140;
			grdHistory.RootTable.Columns["ReferencePartyName"].Width = 140;
			grdHistory.RootTable.Columns["CommissionAgent"].Width = 120;
			grdHistory.RootTable.Columns["CommissionType"].Width = 100;
			grdHistory.RootTable.Columns["CommRate"].Width = 80;
			grdHistory.RootTable.Columns["CommAmount"].Width = 90;
			grdHistory.RootTable.Columns["CommissionRemarks"].Width = 130;
			grdHistory.RootTable.Columns["BillAmount"].Width = 110;
			grdHistory.RootTable.Columns["CurrencyName"].Width = 100;
			grdHistory.RootTable.Columns["ExchangeRate"].Width = 75;
			grdHistory.RootTable.Columns["FcyAmount"].Width = 100;
			grdHistory.RootTable.Columns["BillType"].Width = 85;
			grdHistory.RootTable.Columns["ApprovedStatus"].Width = 100;
			grdHistory.RootTable.Columns["EntryUser"].Width = 110;
			grdHistory.RootTable.Columns["EntryDate"].Width = 145;
			grdHistory.RootTable.Columns["ModifyUser"].Width = 110;
			grdHistory.RootTable.Columns["ModifyDate"].Width = 145;
			grdHistory.RootTable.Columns["ApprovedUser"].Width = 110;
			grdHistory.RootTable.Columns["ApprovedDate"].Width = 145;
			grdHistory.RootTable.Columns["RemarksHeader"].Width = 150;
			grdHistory.RootTable.Columns["NoOfAttachments"].Width = 95;
			GridEX_Helper.AddButton(grdHistory, "Print", "Print", 40, 0);
			GridEX_Helper.AddButton(grdHistory, "Edit", "Edit", 40, 1);
			GridEX_Helper.AddButton(grdHistory, "Voucher", "Voucher", 60, 2);
			GridEX_Helper.AddButton(grdHistory, "AddAttachment", "Add Attachment", 110);
			grdHistory.FrozenColumns = 3;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				ReadById(Conversion.ToInt(item.Cells["Id"].Value));
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
			if (item != null && (int)item.RowType == 0)
			{
				int Currid = Conversion.ToInt(item.Cells["Id"].Value);
				int DocumentTypeId = Conversion.ToInt(item.Cells["DocumentTypeId"].Value);
				if (e.Column.Key == "Print")
				{
					OpenSlip(Currid, DocumentTypeId);
				}
				else if (e.Column.Key == "Voucher")
				{
					CommonServices.AcRptPurchaseSalesVoucherSlip_103(CommonServices.VoucherHeadIdGet(Currid, DocumentTypeId), DocumentTypeId);
				}
				else if (e.Column.Key == "Edit")
				{
					ReadById(Currid);
				}
				else if (e.Column.Key == "AddAttachment")
				{
					AttachmentAddingFromHistory attachmentAddingFromHistory = new AttachmentAddingFromHistory(UserAccount, Currid, DocumentTypeId, base.Name);
					attachmentAddingFromHistory.BringToFront();
					attachmentAddingFromHistory.ShowDialog();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
				int id = Conversion.ToInt(item.Cells["Id"].Value);
				int DocumentTypeId = Conversion.ToInt(item.Cells["DocumentTypeId"].Value);
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(id, DocumentTypeId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadAll_Click(object sender, EventArgs e)
	{
		try
		{
			GetAll();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				InvPurchaseInvoice invGrn = InvPurchaseInvoice.GetByID(Conversion.ToInt((object)r.Cells["Id"].Value.ToString()));
				if (invGrn == null)
				{
					grdDetail.DataSource = null;
					return;
				}
				DataTable dtDetailHistory = dtGrid.Clone();
				frmPurchaseInvoiceStoreManagement_Helper.FillDetailFromListCommonForReadById(invGrn, dtDetailHistory, AddExtraColumns: true);
				grdDetail.DataSource = dtDetailHistory;
				grdDetail.RetrieveStructure();
				grddetailhistorySettings(invGrn.DocumentTypeSrNo);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grddetailhistorySettings(int GrnBaseDocumentTypeId)
	{
		try
		{
			frmPurchaseInvoiceStoreManagement_Helper.DetailGridCommonSetting(grdDetail, null, GrnBaseDocumentTypeId);
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grd, dtGrid);
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdHistory, dthistory);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, DocumentTypeId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VoucherHeadIdGet()
	{
		try
		{
			VoucherHeadId = CommonServices.VoucherHeadIdGet(Id, DocumentTypeId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OpenSlip(int PrintId, int documenttypeId)
	{
		try
		{
			if (documenttypeId == 64)
			{
				CommonServices.StorePurchaseInvoiceSlip230(PrintId);
			}
			else
			{
				CommonServices.PurchaseInvoiceStoreBillWithtax_238(PrintId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSlipDetail_Click(object sender, EventArgs e)
	{
		try
		{
			OpenSlip(Id, DocumentTypeId);
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
			if (txtDueDays.Text.Trim() != string.Empty && txtDueDays.Text != null)
			{
				DueDate.Value = DateTime.Now.AddDays(Conversion.ToDouble((object)txtDueDays.Text.Trim()));
			}
			else
			{
				DueDate.Value = DateTime.Now;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToInt(dt.Rows[i]["ItemId"]) == Conversion.ToInt(item.Cells["ItemId"].Value))
					{
						flag = true;
						item.Cells["TaxNameId"].Value = Conversion.ToInt(dt.Rows[i]["TaxNameId"]);
						item.Cells["TaxName"].Value = Conversion.ToString(dt.Rows[i]["TaxName"]);
						TaxPercent = Conversion.ToDouble(dt.Rows[i]["TaxPercent"]);
						item.Cells["TaxPercent"].Value = TaxPercent;
						ItemAmount = Conversion.ToDouble(item.Cells["ItemAmount"].Value);
						if (ItemAmount > 0.0 && TaxPercent > 0.0)
						{
							TaxAmount = ItemAmount * TaxPercent / 100.0;
							item.Cells["TaxAmount"].Value = TaxAmount;
						}
						TotalAmount = ItemAmount + TaxAmount + Conversion.ToDouble(item.Cells["Freights"].Value);
						item.Cells["BillAmount"].Value = TotalAmount;
					}
				}
				if (!flag)
				{
					item.Cells["TaxNameId"].Value = 0;
					item.Cells["TaxName"].Value = 0;
					item.Cells["TaxPercent"].Value = 0;
					item.Cells["TaxAmount"].Value = 0;
					item.Cells["BillAmount"].Value = Conversion.ToDouble(item.Cells["ItemAmount"].Value) + Conversion.ToDouble(item.Cells["Freights"].Value);
				}
				item.EndEdit();
			}
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbsuppliername_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtSupplier.Rows.Count > 0 && Conversion.ToInt(cmbsuppliername.Value) > 0)
			{
				DataRow[] dr = dtSupplier.Select("Id = " + Conversion.ToInt(cmbsuppliername.Value));
				txtSupplierGLId.Text = Conversion.ToString(dr[0]["GlAccountId"]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshortcut_Click(object sender, EventArgs e)
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

	private void InvfrmPurchaseInvoice_KeyDown(object sender, KeyEventArgs e)
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
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnFrmRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.S && btnSave.Visible && btnSave.Enabled)
				{
					btnSave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					DocDate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					cmbsuppliername.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grd).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Left)
				{
					((Control)(object)grdFreight).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					((Control)(object)grdGLedger).Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					AT.Show();
				}
				if (e.Alt && (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1))
				{
					btnSlipDetail_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.NumPad2 || e.KeyCode == Keys.D2))
				{
					btnPrint_Click(null, null);
				}
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNewHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefreshHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.S)
				{
					btnShowHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					FromDateHistory.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdHistory).Focus();
				}
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
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Alt+1", "For Print Slip");
			dt.Rows.Add("Alt+2", "For Print Voucher");
			dt.Rows.Add("Ctrl+F5", "For Focus on Supplier Name");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+D", "For Adding an row in Focused Grid");
			dt.Rows.Add("Ctrl+Delete", "For Deleting an row of Focused Grid");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Supplier Name in Main Box");
			dt.Rows.Add("Ctrl+ArrowRight", "For Change Focus from one Grid To Freight Grid");
			dt.Rows.Add("Ctrl+ArrowRight", "For Change Focus from one Grid To GLedger Grid");
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
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Expected O, but got Unknown
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Expected O, but got Unknown
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Expected O, but got Unknown
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Expected O, but got Unknown
		//IL_0473: Unknown result type (might be due to invalid IL or missing references)
		//IL_047d: Expected O, but got Unknown
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Expected O, but got Unknown
		//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d5: Expected O, but got Unknown
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e0: Expected O, but got Unknown
		//IL_0565: Unknown result type (might be due to invalid IL or missing references)
		//IL_056f: Expected O, but got Unknown
		//IL_0728: Unknown result type (might be due to invalid IL or missing references)
		//IL_0732: Expected O, but got Unknown
		//IL_0749: Unknown result type (might be due to invalid IL or missing references)
		//IL_0753: Expected O, but got Unknown
		//IL_0796: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a0: Expected O, but got Unknown
		//IL_080f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0819: Expected O, but got Unknown
		//IL_0d5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d66: Expected O, but got Unknown
		//IL_0d73: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7d: Expected O, but got Unknown
		//IL_2111: Unknown result type (might be due to invalid IL or missing references)
		//IL_211b: Expected O, but got Unknown
		//IL_2128: Unknown result type (might be due to invalid IL or missing references)
		//IL_2132: Expected O, but got Unknown
		//IL_2fde: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fe8: Expected O, but got Unknown
		//IL_2ff5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fff: Expected O, but got Unknown
		//IL_326d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3277: Expected O, but got Unknown
		//IL_3284: Unknown result type (might be due to invalid IL or missing references)
		//IL_328e: Expected O, but got Unknown
		//IL_6207: Unknown result type (might be due to invalid IL or missing references)
		//IL_6211: Expected O, but got Unknown
		//IL_6235: Unknown result type (might be due to invalid IL or missing references)
		//IL_623f: Expected O, but got Unknown
		//IL_6471: Unknown result type (might be due to invalid IL or missing references)
		//IL_647b: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		GridEXLayout grdInvExp_DesignTimeLayout = new GridEXLayout();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		GridEXLayout grdGLedger_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdFreight_DesignTimeLayout = new GridEXLayout();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance39 = new Appearance();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance14 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
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
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.StoreManagement.PurchaseInvoiceStoreManagement));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabForm = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label27 = new System.Windows.Forms.Label();
		this.panel15 = new System.Windows.Forms.Panel();
		this.CmbDiscountAccount = new UltraCombo();
		this.label63 = new System.Windows.Forms.Label();
		this.label65 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label64 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.label66 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.txtItemDiscountAmountFooter = new System.Windows.Forms.TextBox();
		this.txtItemAmountWithoutDiscountFooter = new System.Windows.Forms.TextBox();
		this.txtItemNetAmountFooter = new System.Windows.Forms.TextBox();
		this.txtTaxAmountFooter = new System.Windows.Forms.TextBox();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdInvExp = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label81 = new System.Windows.Forms.Label();
		this.label84 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.txtPartyAddLessAmountFooter = new System.Windows.Forms.TextBox();
		this.txtItemAmountPlusTaxAmountFooter = new System.Windows.Forms.TextBox();
		this.txtFreightAmuontFooter = new System.Windows.Forms.TextBox();
		this.txtExpenseAmount = new System.Windows.Forms.TextBox();
		this.txtBillDiscountAmountFooter = new System.Windows.Forms.TextBox();
		this.CmbTaxAccount = new UltraCombo();
		this.txtBillAmount = new System.Windows.Forms.TextBox();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel11 = new System.Windows.Forms.Panel();
		this.grdGLedger = new GridEX();
		this.panel22 = new System.Windows.Forms.Panel();
		this.label29 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grdFreight = new GridEX();
		this.panel20 = new System.Windows.Forms.Panel();
		this.label28 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.CmbPaymentTerm = new UltraCombo();
		this.cmbsuppliername = new UltraCombo();
		this.label16 = new System.Windows.Forms.Label();
		this.txtbillno = new System.Windows.Forms.TextBox();
		this.txtBranchSrNo = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.txtInvoiceTaxNo = new System.Windows.Forms.TextBox();
		this.lblInvoiceTaxNo = new System.Windows.Forms.Label();
		this.txtDueDays = new System.Windows.Forms.TextBox();
		this.label31 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.DueDate = new System.Windows.Forms.DateTimePicker();
		this.txtDeliveryTerm = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.cmbcommagent = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.cmbcommtype = new System.Windows.Forms.ComboBox();
		this.label26 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.txtcommremarks = new System.Windows.Forms.TextBox();
		this.label25 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtcommrate = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.cmbcommuom = new System.Windows.Forms.ComboBox();
		this.label19 = new System.Windows.Forms.Label();
		this.txtcommamount = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label17 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panelpurinv = new System.Windows.Forms.Panel();
		this.lblFormCaption = new System.Windows.Forms.Label();
		this.ChkBok = new System.Windows.Forms.CheckBox();
		this.chkVoucherPreview = new System.Windows.Forms.CheckBox();
		this.txtSupplierGLId = new System.Windows.Forms.TextBox();
		this.txtCommAgentId = new System.Windows.Forms.TextBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnFrmRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnSlipDetail = new System.Windows.Forms.ToolStripButton();
		this.btnshortcut = new System.Windows.Forms.ToolStripButton();
		this.tabHistory = new System.Windows.Forms.TabPage();
		this.grdHistory = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label24 = new System.Windows.Forms.Label();
		this.grdDetail = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel14 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.cmbBranchName = new UltraCombo();
		this.label30 = new System.Windows.Forms.Label();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.label53 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabForm.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel18.SuspendLayout();
		this.panel15.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDiscountAccount).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).BeginInit();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxAccount).BeginInit();
		this.panel7.SuspendLayout();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGLedger).BeginInit();
		this.panel22.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdFreight).BeginInit();
		this.panel20.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsuppliername).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommagent).BeginInit();
		this.panel3.SuspendLayout();
		this.panelpurinv.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabHistory.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel16.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel8.SuspendLayout();
		this.panel14.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel6.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1084, 761);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabForm);
		this.tabControl1.Controls.Add(this.tabHistory);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1084, 761);
		this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
		this.tabControl1.TabIndex = 8;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabForm.BackColor = System.Drawing.Color.White;
		this.tabForm.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabForm.Controls.Add(this.panel18);
		this.tabForm.Controls.Add(this.panel15);
		this.tabForm.Controls.Add(this.panel7);
		this.tabForm.Controls.Add(this.panel4);
		this.tabForm.Controls.Add(this.panel3);
		this.tabForm.Controls.Add(this.panel2);
		this.tabForm.Location = new System.Drawing.Point(4, 4);
		this.tabForm.Name = "tabForm";
		this.tabForm.Padding = new System.Windows.Forms.Padding(3);
		this.tabForm.Size = new System.Drawing.Size(1076, 732);
		this.tabForm.TabIndex = 0;
		this.tabForm.Text = "Form";
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 308);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1070, 160);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.ctrlGrdBar1);
		this.panel18.Controls.Add(this.label27);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(3, 284);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1070, 24);
		this.panel18.TabIndex = 27;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1027, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(41, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.ForeColor = System.Drawing.Color.White;
		this.label27.Location = new System.Drawing.Point(6, 3);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(78, 16);
		this.label27.TabIndex = 24;
		this.label27.Text = "Item Detail";
		this.panel15.BackColor = System.Drawing.Color.White;
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDiscountAccount);
		this.panel15.Controls.Add(this.label63);
		this.panel15.Controls.Add(this.label65);
		this.panel15.Controls.Add(this.label14);
		this.panel15.Controls.Add(this.label10);
		this.panel15.Controls.Add(this.label23);
		this.panel15.Controls.Add(this.label36);
		this.panel15.Controls.Add(this.label34);
		this.panel15.Controls.Add(this.label3);
		this.panel15.Controls.Add(this.label32);
		this.panel15.Controls.Add(this.label64);
		this.panel15.Controls.Add(this.label62);
		this.panel15.Controls.Add(this.label66);
		this.panel15.Controls.Add(this.label61);
		this.panel15.Controls.Add(this.txtItemDiscountAmountFooter);
		this.panel15.Controls.Add(this.txtItemAmountWithoutDiscountFooter);
		this.panel15.Controls.Add(this.txtItemNetAmountFooter);
		this.panel15.Controls.Add(this.txtTaxAmountFooter);
		this.panel15.Controls.Add(this.panel5);
		this.panel15.Controls.Add(this.label84);
		this.panel15.Controls.Add(this.label18);
		this.panel15.Controls.Add(this.label35);
		this.panel15.Controls.Add(this.label33);
		this.panel15.Controls.Add(this.label2);
		this.panel15.Controls.Add(this.label8);
		this.panel15.Controls.Add(this.txtPartyAddLessAmountFooter);
		this.panel15.Controls.Add(this.txtItemAmountPlusTaxAmountFooter);
		this.panel15.Controls.Add(this.txtFreightAmuontFooter);
		this.panel15.Controls.Add(this.txtExpenseAmount);
		this.panel15.Controls.Add(this.txtBillDiscountAmountFooter);
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTaxAccount);
		this.panel15.Controls.Add(this.txtBillAmount);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel15.Location = new System.Drawing.Point(3, 468);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1070, 261);
		this.panel15.TabIndex = 2;
		this.CmbDiscountAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbDiscountAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDiscountAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDiscountAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance13;
		((SpecialBoxBase)((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance19).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDiscountAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDiscountAccount).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbDiscountAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDiscountAccount).Location = new System.Drawing.Point(607, 137);
		((System.Windows.Forms.Control)(object)this.CmbDiscountAccount).Name = "CmbDiscountAccount";
		((System.Windows.Forms.Control)(object)this.CmbDiscountAccount).Size = new System.Drawing.Size(254, 26);
		((System.Windows.Forms.Control)(object)this.CmbDiscountAccount).TabIndex = 7;
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label63.Location = new System.Drawing.Point(752, 4);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(83, 15);
		this.label63.TabIndex = 241;
		this.label63.Text = "Item Discount";
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label65.Location = new System.Drawing.Point(864, 25);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(21, 18);
		this.label65.TabIndex = 242;
		this.label65.Text = "=";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(862, 239);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(21, 18);
		this.label14.TabIndex = 243;
		this.label14.Text = "=";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(862, 141);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(21, 18);
		this.label10.TabIndex = 243;
		this.label10.Text = "+";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(862, 81);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(21, 18);
		this.label23.TabIndex = 243;
		this.label23.Text = "+";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(864, 200);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(21, 18);
		this.label36.TabIndex = 243;
		this.label36.Text = "+";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(864, 171);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(21, 18);
		this.label34.TabIndex = 243;
		this.label34.Text = "+";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(864, 112);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(21, 18);
		this.label3.TabIndex = 243;
		this.label3.Text = "+";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(864, 52);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(21, 18);
		this.label32.TabIndex = 243;
		this.label32.Text = "+";
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label64.Location = new System.Drawing.Point(734, 25);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(15, 18);
		this.label64.TabIndex = 245;
		this.label64.Text = "-";
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label62.Location = new System.Drawing.Point(607, 4);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(79, 15);
		this.label62.TabIndex = 246;
		this.label62.Text = "Item Amount";
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label66.Location = new System.Drawing.Point(887, 4);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(101, 15);
		this.label66.TabIndex = 247;
		this.label66.Text = "Net Item Amount";
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label61.Location = new System.Drawing.Point(494, 46);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(88, 30);
		this.label61.TabIndex = 248;
		this.label61.Text = "Tax Account \r\n&&  Amount (Cr)";
		this.txtItemDiscountAmountFooter.BackColor = System.Drawing.Color.White;
		this.txtItemDiscountAmountFooter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemDiscountAmountFooter.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemDiscountAmountFooter.Location = new System.Drawing.Point(752, 23);
		this.txtItemDiscountAmountFooter.Name = "txtItemDiscountAmountFooter";
		this.txtItemDiscountAmountFooter.AccessibleName = "ItemDiscountAmountFooter";
		this.txtItemDiscountAmountFooter.Size = new System.Drawing.Size(107, 23);
		this.txtItemDiscountAmountFooter.TabIndex = 1;
		this.txtItemDiscountAmountFooter.TabStop = false;
		this.txtItemDiscountAmountFooter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtItemDiscountAmountFooter.TextChanged += new System.EventHandler(txtItemDiscountAmountFooter_TextChanged);
		this.txtItemAmountWithoutDiscountFooter.BackColor = System.Drawing.SystemColors.Control;
		this.txtItemAmountWithoutDiscountFooter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemAmountWithoutDiscountFooter.Enabled = false;
		this.txtItemAmountWithoutDiscountFooter.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemAmountWithoutDiscountFooter.Location = new System.Drawing.Point(607, 23);
		this.txtItemAmountWithoutDiscountFooter.Name = "txtItemAmountWithoutDiscountFooter";
		this.txtItemAmountWithoutDiscountFooter.Size = new System.Drawing.Size(125, 23);
		this.txtItemAmountWithoutDiscountFooter.TabIndex = 0;
		this.txtItemAmountWithoutDiscountFooter.TabStop = false;
		this.txtItemAmountWithoutDiscountFooter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtItemNetAmountFooter.BackColor = System.Drawing.SystemColors.Control;
		this.txtItemNetAmountFooter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemNetAmountFooter.Enabled = false;
		this.txtItemNetAmountFooter.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemNetAmountFooter.Location = new System.Drawing.Point(887, 23);
		this.txtItemNetAmountFooter.Name = "txtItemNetAmountFooter";
		this.txtItemNetAmountFooter.Size = new System.Drawing.Size(180, 23);
		this.txtItemNetAmountFooter.TabIndex = 2;
		this.txtItemNetAmountFooter.TabStop = false;
		this.txtItemNetAmountFooter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTaxAmountFooter.BackColor = System.Drawing.SystemColors.Control;
		this.txtTaxAmountFooter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTaxAmountFooter.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Bold);
		this.txtTaxAmountFooter.Location = new System.Drawing.Point(887, 48);
		this.txtTaxAmountFooter.Name = "txtTaxAmountFooter";
		this.txtTaxAmountFooter.Size = new System.Drawing.Size(180, 27);
		this.txtTaxAmountFooter.TabIndex = 4;
		this.txtTaxAmountFooter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdInvExp);
		this.panel5.Controls.Add(this.panel10);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(492, 261);
		this.panel5.TabIndex = 1;
		((ScrollableBase)this.grdInvExp).BackColor = System.Drawing.Color.White;
		grdInvExp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInvExp.DesignTimeLayout = grdInvExp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInvExp.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Location = new System.Drawing.Point(0, 20);
		((System.Windows.Forms.Control)(object)this.grdInvExp).Name = "grdInvExp";
		((ScrollableBase)this.grdInvExp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Size = new System.Drawing.Size(492, 241);
		((System.Windows.Forms.Control)(object)this.grdInvExp).TabIndex = 0;
		this.grdInvExp.TotalRow = (InheritableBoolean)1;
		this.grdInvExp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInvExp).VisualStyle = (VisualStyle)4;
		this.grdInvExp.CellUpdated += new ColumnActionEventHandler(grdInvExp_CellUpdated);
		this.grdInvExp.ColumnButtonClick += new ColumnActionEventHandler(grdInvExp_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdInvExp).KeyDown += new System.Windows.Forms.KeyEventHandler(grdInvExp_KeyDown);
		this.panel10.BackColor = System.Drawing.Color.White;
		this.panel10.Controls.Add(this.label81);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.ForeColor = System.Drawing.Color.Teal;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(492, 20);
		this.panel10.TabIndex = 36;
		this.label81.AutoSize = true;
		this.label81.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label81.ForeColor = System.Drawing.Color.Teal;
		this.label81.Location = new System.Drawing.Point(3, 3);
		this.label81.Name = "label81";
		this.label81.Size = new System.Drawing.Size(329, 14);
		this.label81.TabIndex = 25;
		this.label81.Text = "Other Expenses Credit to Party && Charge to Product";
		this.label84.BackColor = System.Drawing.SystemColors.Control;
		this.label84.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label84.Location = new System.Drawing.Point(724, 233);
		this.label84.Name = "label84";
		this.label84.Size = new System.Drawing.Size(137, 24);
		this.label84.TabIndex = 37;
		this.label84.Text = "Net Bill Amount";
		this.label84.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label18.BackColor = System.Drawing.SystemColors.Control;
		this.label18.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label18.Location = new System.Drawing.Point(637, 78);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(224, 24);
		this.label18.TabIndex = 239;
		this.label18.Text = "Item Net Amount + Tax Amount";
		this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label35.BackColor = System.Drawing.SystemColors.Control;
		this.label35.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label35.Location = new System.Drawing.Point(660, 197);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(201, 24);
		this.label35.TabIndex = 239;
		this.label35.Text = "Total Party AddLess Amount";
		this.label35.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label33.BackColor = System.Drawing.SystemColors.Control;
		this.label33.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label33.Location = new System.Drawing.Point(660, 168);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(201, 24);
		this.label33.TabIndex = 239;
		this.label33.Text = "Total Party Freight Amount";
		this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label2.BackColor = System.Drawing.SystemColors.Control;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(691, 109);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(170, 24);
		this.label2.TabIndex = 239;
		this.label2.Text = "Total Expense Amount";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label8.AutoSize = true;
		this.label8.BackColor = System.Drawing.Color.White;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(492, 129);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(115, 45);
		this.label8.TabIndex = 239;
		this.label8.Text = "Discount Account &&\r\nDiscount Amount\r\n(Cr)";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtPartyAddLessAmountFooter.BackColor = System.Drawing.SystemColors.Control;
		this.txtPartyAddLessAmountFooter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPartyAddLessAmountFooter.Enabled = false;
		this.txtPartyAddLessAmountFooter.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Bold);
		this.txtPartyAddLessAmountFooter.Location = new System.Drawing.Point(887, 196);
		this.txtPartyAddLessAmountFooter.Name = "txtPartyAddLessAmountFooter";
		this.txtPartyAddLessAmountFooter.Size = new System.Drawing.Size(180, 27);
		this.txtPartyAddLessAmountFooter.TabIndex = 6;
		this.txtPartyAddLessAmountFooter.TabStop = false;
		this.txtPartyAddLessAmountFooter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtItemAmountPlusTaxAmountFooter.BackColor = System.Drawing.SystemColors.Control;
		this.txtItemAmountPlusTaxAmountFooter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemAmountPlusTaxAmountFooter.Enabled = false;
		this.txtItemAmountPlusTaxAmountFooter.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Bold);
		this.txtItemAmountPlusTaxAmountFooter.Location = new System.Drawing.Point(887, 77);
		this.txtItemAmountPlusTaxAmountFooter.Name = "txtItemAmountPlusTaxAmountFooter";
		this.txtItemAmountPlusTaxAmountFooter.Size = new System.Drawing.Size(180, 27);
		this.txtItemAmountPlusTaxAmountFooter.TabIndex = 5;
		this.txtItemAmountPlusTaxAmountFooter.TabStop = false;
		this.txtItemAmountPlusTaxAmountFooter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFreightAmuontFooter.BackColor = System.Drawing.SystemColors.Control;
		this.txtFreightAmuontFooter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFreightAmuontFooter.Enabled = false;
		this.txtFreightAmuontFooter.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Bold);
		this.txtFreightAmuontFooter.Location = new System.Drawing.Point(887, 167);
		this.txtFreightAmuontFooter.Name = "txtFreightAmuontFooter";
		this.txtFreightAmuontFooter.Size = new System.Drawing.Size(180, 27);
		this.txtFreightAmuontFooter.TabIndex = 6;
		this.txtFreightAmuontFooter.TabStop = false;
		this.txtFreightAmuontFooter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExpenseAmount.BackColor = System.Drawing.SystemColors.Control;
		this.txtExpenseAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExpenseAmount.Enabled = false;
		this.txtExpenseAmount.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Bold);
		this.txtExpenseAmount.Location = new System.Drawing.Point(887, 108);
		this.txtExpenseAmount.Name = "txtExpenseAmount";
		this.txtExpenseAmount.Size = new System.Drawing.Size(180, 27);
		this.txtExpenseAmount.TabIndex = 6;
		this.txtExpenseAmount.TabStop = false;
		this.txtExpenseAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBillDiscountAmountFooter.BackColor = System.Drawing.Color.White;
		this.txtBillDiscountAmountFooter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBillDiscountAmountFooter.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Bold);
		this.txtBillDiscountAmountFooter.Location = new System.Drawing.Point(887, 137);
		this.txtBillDiscountAmountFooter.Name = "txtBillDiscountAmountFooter";
		this.txtBillDiscountAmountFooter.Size = new System.Drawing.Size(180, 27);
		this.txtBillDiscountAmountFooter.TabIndex = 8;
		this.txtBillDiscountAmountFooter.TabStop = false;
		this.txtBillDiscountAmountFooter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBillDiscountAmountFooter.TextChanged += new System.EventHandler(txtBillDiscountAmountFooter_TextChanged);
		this.CmbTaxAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbTaxAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTaxAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTaxAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance60;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance65).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTaxAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Location = new System.Drawing.Point(607, 48);
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Name = "CmbTaxAccount";
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Size = new System.Drawing.Size(254, 26);
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).TabIndex = 3;
		this.txtBillAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBillAmount.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Bold);
		this.txtBillAmount.Location = new System.Drawing.Point(887, 232);
		this.txtBillAmount.Name = "txtBillAmount";
		this.txtBillAmount.ReadOnly = true;
		this.txtBillAmount.Size = new System.Drawing.Size(180, 27);
		this.txtBillAmount.TabIndex = 9;
		this.txtBillAmount.TabStop = false;
		this.panel7.Controls.Add(this.panel11);
		this.panel7.Controls.Add(this.panel9);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 144);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1070, 140);
		this.panel7.TabIndex = 2;
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.grdGLedger);
		this.panel11.Controls.Add(this.panel22);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(482, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(588, 140);
		this.panel11.TabIndex = 2;
		((ScrollableBase)this.grdGLedger).BackColor = System.Drawing.Color.White;
		this.grdGLedger.ColumnAutoResize = true;
		grdGLedger_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdGLedger.DesignTimeLayout = grdGLedger_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdGLedger.GroupByBoxVisible = false;
		this.grdGLedger.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Location = new System.Drawing.Point(0, 20);
		((System.Windows.Forms.Control)(object)this.grdGLedger).Name = "grdGLedger";
		((ScrollableBase)this.grdGLedger).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Size = new System.Drawing.Size(588, 120);
		((System.Windows.Forms.Control)(object)this.grdGLedger).TabIndex = 0;
		this.grdGLedger.TotalRow = (InheritableBoolean)1;
		this.grdGLedger.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGLedger).VisualStyle = (VisualStyle)4;
		this.grdGLedger.CellUpdated += new ColumnActionEventHandler(grdGLedger_CellUpdated);
		this.grdGLedger.ColumnButtonClick += new ColumnActionEventHandler(grdGLedger_ColumnButtonClick);
		this.panel22.BackColor = System.Drawing.Color.Teal;
		this.panel22.Controls.Add(this.label29);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel22.Location = new System.Drawing.Point(0, 0);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(588, 20);
		this.panel22.TabIndex = 29;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.ForeColor = System.Drawing.Color.White;
		this.label29.Location = new System.Drawing.Point(6, 2);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(102, 16);
		this.label29.TabIndex = 24;
		this.label29.Text = "Party AddLess";
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdFreight);
		this.panel9.Controls.Add(this.panel20);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(482, 140);
		this.panel9.TabIndex = 0;
		((ScrollableBase)this.grdFreight).BackColor = System.Drawing.Color.White;
		this.grdFreight.ColumnAutoResize = true;
		grdFreight_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdFreight.DesignTimeLayout = grdFreight_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdFreight).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdFreight).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdFreight.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdFreight).Location = new System.Drawing.Point(0, 20);
		((System.Windows.Forms.Control)(object)this.grdFreight).Name = "grdFreight";
		((ScrollableBase)this.grdFreight).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdFreight).Size = new System.Drawing.Size(482, 120);
		((System.Windows.Forms.Control)(object)this.grdFreight).TabIndex = 0;
		this.grdFreight.TotalRow = (InheritableBoolean)1;
		this.grdFreight.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdFreight).VisualStyle = (VisualStyle)4;
		this.grdFreight.CellUpdated += new ColumnActionEventHandler(grdFreight_CellUpdated);
		this.grdFreight.ColumnButtonClick += new ColumnActionEventHandler(grdFreight_ColumnButtonClick);
		this.grdFreight.SelectionChanged += new System.EventHandler(grdFreight_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.grdFreight).Click += new System.EventHandler(grdFreight_SelectionChanged);
		this.panel20.BackColor = System.Drawing.Color.Teal;
		this.panel20.Controls.Add(this.label28);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(0, 0);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(482, 20);
		this.panel20.TabIndex = 28;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.ForeColor = System.Drawing.Color.White;
		this.label28.Location = new System.Drawing.Point(3, 2);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(445, 16);
		this.label28.TabIndex = 24;
		this.label28.Text = "Credit To Selected Account and Charged To Product (Freight Detail)";
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentTerm);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbsuppliername);
		this.panel4.Controls.Add(this.label16);
		this.panel4.Controls.Add(this.txtbillno);
		this.panel4.Controls.Add(this.txtBranchSrNo);
		this.panel4.Controls.Add(this.label13);
		this.panel4.Controls.Add(this.txtInvoiceTaxNo);
		this.panel4.Controls.Add(this.lblInvoiceTaxNo);
		this.panel4.Controls.Add(this.txtDueDays);
		this.panel4.Controls.Add(this.label31);
		this.panel4.Controls.Add(this.label22);
		this.panel4.Controls.Add(this.DueDate);
		this.panel4.Controls.Add(this.txtDeliveryTerm);
		this.panel4.Controls.Add(this.label15);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcommagent);
		this.panel4.Controls.Add(this.label20);
		this.panel4.Controls.Add(this.cmbcommtype);
		this.panel4.Controls.Add(this.label26);
		this.panel4.Controls.Add(this.label11);
		this.panel4.Controls.Add(this.txtcommremarks);
		this.panel4.Controls.Add(this.label25);
		this.panel4.Controls.Add(this.label21);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add(this.txtcommrate);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add(this.cmbcommuom);
		this.panel4.Controls.Add(this.label19);
		this.panel4.Controls.Add(this.txtcommamount);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.txtRemarks);
		this.panel4.Controls.Add(this.DocDate);
		this.panel4.Controls.Add(this.txtdocno);
		this.panel4.Controls.Add(this.label17);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel4.Location = new System.Drawing.Point(3, 58);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1070, 86);
		this.panel4.TabIndex = 0;
		this.CmbPaymentTerm.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance84;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance2).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance2).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance2).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance3;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance6).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPaymentTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Location = new System.Drawing.Point(625, 32);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Name = "CmbPaymentTerm";
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Size = new System.Drawing.Size(250, 26);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).TabIndex = 8;
		this.cmbsuppliername.AllowNull = (DefaultableBoolean)1;
		this.cmbsuppliername.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbsuppliername.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbsuppliername.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Appearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsuppliername).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance72;
		((SpecialBoxBase)((UltraGridBase)this.cmbsuppliername).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance77).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance78).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance79).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbsuppliername.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).Location = new System.Drawing.Point(625, 5);
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).Name = "cmbsuppliername";
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).Size = new System.Drawing.Size(250, 26);
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).TabIndex = 7;
		this.cmbsuppliername.ValueChanged += new System.EventHandler(cmbsuppliername_ValueChanged);
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(541, 38);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(84, 15);
		this.label16.TabIndex = 114;
		this.label16.Text = "Payment Term";
		this.txtbillno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbillno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbillno.Location = new System.Drawing.Point(300, 34);
		this.txtbillno.Name = "txtbillno";
		this.txtbillno.Size = new System.Drawing.Size(238, 23);
		this.txtbillno.TabIndex = 6;
		this.txtBranchSrNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBranchSrNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBranchSrNo.Location = new System.Drawing.Point(150, 7);
		this.txtBranchSrNo.Name = "txtBranchSrNo";
		this.txtBranchSrNo.ReadOnly = true;
		this.txtBranchSrNo.Size = new System.Drawing.Size(60, 23);
		this.txtBranchSrNo.TabIndex = 2;
		this.txtBranchSrNo.TabStop = false;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(5, 38);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(57, 15);
		this.label13.TabIndex = 4;
		this.label13.Text = "Doc Date";
		this.txtInvoiceTaxNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInvoiceTaxNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceTaxNo.Location = new System.Drawing.Point(300, 7);
		this.txtInvoiceTaxNo.Name = "txtInvoiceTaxNo";
		this.txtInvoiceTaxNo.ReadOnly = true;
		this.txtInvoiceTaxNo.Size = new System.Drawing.Size(238, 23);
		this.txtInvoiceTaxNo.TabIndex = 5;
		this.txtInvoiceTaxNo.TabStop = false;
		this.lblInvoiceTaxNo.AutoSize = true;
		this.lblInvoiceTaxNo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblInvoiceTaxNo.Location = new System.Drawing.Point(213, 11);
		this.lblInvoiceTaxNo.Name = "lblInvoiceTaxNo";
		this.lblInvoiceTaxNo.Size = new System.Drawing.Size(86, 15);
		this.lblInvoiceTaxNo.TabIndex = 18;
		this.lblInvoiceTaxNo.Text = "Invoice Tax No";
		this.txtDueDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDueDays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDueDays.Location = new System.Drawing.Point(936, 7);
		this.txtDueDays.Name = "txtDueDays";
		this.txtDueDays.Size = new System.Drawing.Size(130, 23);
		this.txtDueDays.TabIndex = 9;
		this.txtDueDays.Tag = "int";
		this.txtDueDays.TextChanged += new System.EventHandler(txtDueDays_TextChanged);
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(876, 38);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(57, 15);
		this.label31.TabIndex = 10;
		this.label31.Text = "Due Date";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(876, 11);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(58, 15);
		this.label22.TabIndex = 10;
		this.label22.Text = "Due Days";
		this.DueDate.CustomFormat = "dd-MMM-yy";
		this.DueDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.DueDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DueDate.Location = new System.Drawing.Point(936, 34);
		this.DueDate.Name = "DueDate";
		this.DueDate.Size = new System.Drawing.Size(130, 22);
		this.DueDate.TabIndex = 10;
		this.DueDate.TabStop = false;
		this.txtDeliveryTerm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDeliveryTerm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDeliveryTerm.Location = new System.Drawing.Point(923, 127);
		this.txtDeliveryTerm.Name = "txtDeliveryTerm";
		this.txtDeliveryTerm.ReadOnly = true;
		this.txtDeliveryTerm.Size = new System.Drawing.Size(72, 22);
		this.txtDeliveryTerm.TabIndex = 27;
		this.txtDeliveryTerm.TabStop = false;
		this.txtDeliveryTerm.Visible = false;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(847, 127);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(36, 15);
		this.label15.TabIndex = 28;
		this.label15.Text = "Term";
		this.label15.Visible = false;
		this.cmbcommagent.AllowNull = (DefaultableBoolean)1;
		this.cmbcommagent.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcommagent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcommagent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance1;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance30).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance48).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcommagent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Location = new System.Drawing.Point(756, 154);
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Name = "cmbcommagent";
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Size = new System.Drawing.Size(72, 24);
		((System.Windows.Forms.Control)(object)this.cmbcommagent).TabIndex = 11;
		((System.Windows.Forms.Control)(object)this.cmbcommagent).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Visible = false;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(680, 158);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(78, 15);
		this.label20.TabIndex = 3;
		this.label20.Text = "Comm Agent";
		this.label20.Visible = false;
		this.cmbcommtype.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cmbcommtype.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cmbcommtype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cmbcommtype.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcommtype.FormattingEnabled = true;
		this.cmbcommtype.Items.AddRange(new object[3] { "Flat", "Percentage", "Comm Weight" });
		this.cmbcommtype.Location = new System.Drawing.Point(923, 155);
		this.cmbcommtype.Name = "cmbcommtype";
		this.cmbcommtype.Size = new System.Drawing.Size(72, 23);
		this.cmbcommtype.TabIndex = 12;
		this.cmbcommtype.TabStop = false;
		this.cmbcommtype.Visible = false;
		this.cmbcommtype.SelectedIndexChanged += new System.EventHandler(cmbcommtype_Leave);
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(680, 212);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(49, 15);
		this.label26.TabIndex = 8;
		this.label26.Text = "Amount";
		this.label26.Visible = false;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(834, 156);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(73, 15);
		this.label11.TabIndex = 0;
		this.label11.Text = "Comm Type";
		this.label11.Visible = false;
		this.txtcommremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommremarks.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommremarks.Location = new System.Drawing.Point(923, 210);
		this.txtcommremarks.Name = "txtcommremarks";
		this.txtcommremarks.Size = new System.Drawing.Size(72, 21);
		this.txtcommremarks.TabIndex = 18;
		this.txtcommremarks.TabStop = false;
		this.txtcommremarks.Visible = false;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(834, 210);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(57, 15);
		this.label25.TabIndex = 3;
		this.label25.Text = "Remarks";
		this.label25.Visible = false;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(680, 186);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(73, 15);
		this.label21.TabIndex = 8;
		this.label21.Text = "Comm Rate";
		this.label21.Visible = false;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(541, 11);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(69, 15);
		this.label7.TabIndex = 6;
		this.label7.Text = "Bill To Party";
		this.txtcommrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommrate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommrate.Location = new System.Drawing.Point(756, 183);
		this.txtcommrate.Name = "txtcommrate";
		this.txtcommrate.Size = new System.Drawing.Size(72, 22);
		this.txtcommrate.TabIndex = 13;
		this.txtcommrate.TabStop = false;
		this.txtcommrate.Visible = false;
		this.txtcommrate.TextChanged += new System.EventHandler(txtcommrate_TextChanged);
		this.label4.AutoSize = true;
		this.label4.BackColor = System.Drawing.Color.White;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(4, 61);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(67, 20);
		this.label4.TabIndex = 16;
		this.label4.Text = "Remarks";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.cmbcommuom.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cmbcommuom.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cmbcommuom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cmbcommuom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcommuom.FormattingEnabled = true;
		this.cmbcommuom.Items.AddRange(new object[7] { "1", "5", "25", "40", "50", "60", "100" });
		this.cmbcommuom.Location = new System.Drawing.Point(923, 182);
		this.cmbcommuom.Name = "cmbcommuom";
		this.cmbcommuom.Size = new System.Drawing.Size(72, 23);
		this.cmbcommuom.TabIndex = 14;
		this.cmbcommuom.TabStop = false;
		this.cmbcommuom.Visible = false;
		this.cmbcommuom.SelectedIndexChanged += new System.EventHandler(cmbcommuom_Leave);
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(834, 184);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(76, 15);
		this.label19.TabIndex = 0;
		this.label19.Text = "Comm UOM";
		this.label19.Visible = false;
		this.txtcommamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommamount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommamount.Location = new System.Drawing.Point(756, 210);
		this.txtcommamount.Name = "txtcommamount";
		this.txtcommamount.ReadOnly = true;
		this.txtcommamount.Size = new System.Drawing.Size(72, 22);
		this.txtcommamount.TabIndex = 15;
		this.txtcommamount.TabStop = false;
		this.txtcommamount.Visible = false;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(213, 38);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(66, 15);
		this.label6.TabIndex = 8;
		this.label6.Text = "Manual No";
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(79, 60);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(988, 23);
		this.txtRemarks.TabIndex = 4;
		this.DocDate.CustomFormat = "dd-MM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(80, 34);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(130, 22);
		this.DocDate.TabIndex = 3;
		this.DocDate.TabStop = false;
		this.DocDate.Leave += new System.EventHandler(DocDate_Leave);
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(79, 7);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(70, 23);
		this.txtdocno.TabIndex = 1;
		this.txtdocno.TabStop = false;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(5, 3);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(77, 30);
		this.label17.TabIndex = 0;
		this.label17.Text = "Doc No && \r\nBranch Sr No";
		this.panel3.Controls.Add(this.panelpurinv);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 31);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1070, 27);
		this.panel3.TabIndex = 1;
		this.panelpurinv.BackColor = System.Drawing.Color.Teal;
		this.panelpurinv.Controls.Add(this.lblFormCaption);
		this.panelpurinv.Controls.Add(this.ChkBok);
		this.panelpurinv.Controls.Add(this.chkVoucherPreview);
		this.panelpurinv.Controls.Add(this.txtSupplierGLId);
		this.panelpurinv.Controls.Add(this.txtCommAgentId);
		this.panelpurinv.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelpurinv.Location = new System.Drawing.Point(0, 0);
		this.panelpurinv.Name = "panelpurinv";
		this.panelpurinv.Size = new System.Drawing.Size(1070, 26);
		this.panelpurinv.TabIndex = 26;
		this.lblFormCaption.AutoSize = true;
		this.lblFormCaption.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFormCaption.ForeColor = System.Drawing.Color.White;
		this.lblFormCaption.Location = new System.Drawing.Point(6, 4);
		this.lblFormCaption.Name = "lblFormCaption";
		this.lblFormCaption.Size = new System.Drawing.Size(147, 19);
		this.lblFormCaption.TabIndex = 24;
		this.lblFormCaption.Text = "Purchase Invoice";
		this.ChkBok.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBok.AutoSize = true;
		this.ChkBok.Checked = true;
		this.ChkBok.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkBok.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBok.ForeColor = System.Drawing.Color.White;
		this.ChkBok.Location = new System.Drawing.Point(953, 4);
		this.ChkBok.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBok.Name = "ChkBok";
		this.ChkBok.Size = new System.Drawing.Size(114, 20);
		this.ChkBok.TabIndex = 1;
		this.ChkBok.Text = "Slip Preview";
		this.ChkBok.UseVisualStyleBackColor = true;
		this.chkVoucherPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkVoucherPreview.AutoSize = true;
		this.chkVoucherPreview.Checked = true;
		this.chkVoucherPreview.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkVoucherPreview.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkVoucherPreview.ForeColor = System.Drawing.Color.White;
		this.chkVoucherPreview.Location = new System.Drawing.Point(800, 4);
		this.chkVoucherPreview.Margin = new System.Windows.Forms.Padding(2);
		this.chkVoucherPreview.Name = "chkVoucherPreview";
		this.chkVoucherPreview.Size = new System.Drawing.Size(148, 20);
		this.chkVoucherPreview.TabIndex = 0;
		this.chkVoucherPreview.Text = "Voucher Preview";
		this.chkVoucherPreview.UseVisualStyleBackColor = true;
		this.txtSupplierGLId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierGLId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierGLId.Location = new System.Drawing.Point(631, 2);
		this.txtSupplierGLId.Name = "txtSupplierGLId";
		this.txtSupplierGLId.Size = new System.Drawing.Size(21, 22);
		this.txtSupplierGLId.TabIndex = 16;
		this.txtSupplierGLId.Visible = false;
		this.txtCommAgentId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommAgentId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommAgentId.Location = new System.Drawing.Point(610, 2);
		this.txtCommAgentId.Name = "txtCommAgentId";
		this.txtCommAgentId.Size = new System.Drawing.Size(21, 22);
		this.txtCommAgentId.TabIndex = 17;
		this.txtCommAgentId.Visible = false;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1070, 28);
		this.panel2.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[12]
		{
			this.toolStripSeparator1, this.btnNew, this.btnFrmRefresh, this.btnSave, this.btnUpdate, this.btnDelete, this.toolStripSeparator, this.btnAttachment, this.toolStripButton3, this.btnPrint,
			this.btnSlipDetail, this.btnshortcut
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
		this.toolStrip1.Size = new System.Drawing.Size(1070, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnFrmRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnFrmRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnFrmRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnFrmRefresh.Name = "btnFrmRefresh";
		this.btnFrmRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnFrmRefresh.Text = "&Refresh";
		this.btnFrmRefresh.Click += new System.EventHandler(btnFrmRefresh_Click);
		this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(64, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripButton3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton3.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton3.Name = "toolStripButton3";
		this.toolStripButton3.Size = new System.Drawing.Size(93, 24);
		this.toolStripButton3.Text = "Load &GRN";
		this.toolStripButton3.Click += new System.EventHandler(toolStripButton3_Click_1);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(115, 24);
		this.btnPrint.Text = "103-Voucher";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnSlipDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSlipDetail.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSlipDetail.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSlipDetail.Name = "btnSlipDetail";
		this.btnSlipDetail.Size = new System.Drawing.Size(85, 24);
		this.btnSlipDetail.Text = "230-Slip";
		this.btnSlipDetail.Click += new System.EventHandler(btnSlipDetail_Click);
		this.btnshortcut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcut.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcut.Name = "btnshortcut";
		this.btnshortcut.Size = new System.Drawing.Size(126, 24);
		this.btnshortcut.Text = "ShortCut Keys";
		this.btnshortcut.Click += new System.EventHandler(btnshortcut_Click);
		this.tabHistory.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.tabHistory.Controls.Add(this.panel16);
		this.tabHistory.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.tabHistory.Controls.Add(this.panel8);
		this.tabHistory.Controls.Add(this.panel14);
		this.tabHistory.Controls.Add(this.panel6);
		this.tabHistory.Location = new System.Drawing.Point(4, 4);
		this.tabHistory.Name = "tabHistory";
		this.tabHistory.Padding = new System.Windows.Forms.Padding(3);
		this.tabHistory.Size = new System.Drawing.Size(1076, 732);
		this.tabHistory.TabIndex = 1;
		this.tabHistory.Text = "History";
		this.tabHistory.UseVisualStyleBackColor = true;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(3, 150);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1070, 346);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 0;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(grdHistory_SelectionChanged);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.label24);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel16.Location = new System.Drawing.Point(3, 496);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1070, 20);
		this.panel16.TabIndex = 47;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.Color.White;
		this.label24.Location = new System.Drawing.Point(4, 1);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(200, 16);
		this.label24.TabIndex = 24;
		this.label24.Text = "Detail Of Above Selected Row";
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		this.grdDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(3, 516);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1070, 213);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label5);
		this.panel8.Controls.Add(this.ctrlGrdBar2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 128);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1070, 22);
		this.panel8.TabIndex = 45;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(3, 1);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(132, 18);
		this.label5.TabIndex = 24;
		this.label5.Text = "Filtered Records";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1024, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel14.BackColor = System.Drawing.Color.White;
		this.panel14.Controls.Add(this.groupBox3);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(3, 55);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1070, 73);
		this.panel14.TabIndex = 29;
		this.groupBox3.Controls.Add(this.FromDateHistory);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchName);
		this.groupBox3.Controls.Add(this.label30);
		this.groupBox3.Controls.Add(this.rdapproveddate);
		this.groupBox3.Controls.Add(this.rdmodifydate);
		this.groupBox3.Controls.Add(this.drdocdate);
		this.groupBox3.Controls.Add(this.rdentrydate);
		this.groupBox3.Controls.Add(this.btnShowHistory);
		this.groupBox3.Controls.Add(this.label53);
		this.groupBox3.Controls.Add(this.label1);
		this.groupBox3.Controls.Add(this.label12);
		this.groupBox3.Controls.Add(this.label57);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox3.Controls.Add(this.ToDocNoHistory);
		this.groupBox3.Controls.Add(this.FromDocNoHistory);
		this.groupBox3.Controls.Add(this.ToDateHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(0, 2);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1015, 70);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(82, 42);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(140, 22);
		this.FromDateHistory.TabIndex = 0;
		this.cmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance25;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance31).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Location = new System.Drawing.Point(82, 14);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Name = "cmbBranchName";
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Size = new System.Drawing.Size(281, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).TabIndex = 146;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(4, 20);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(79, 15);
		this.label30.TabIndex = 147;
		this.label30.Text = "Branch Name";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(910, 44);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 9;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(910, 18);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(829, 18);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 6;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(829, 44);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 7;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(761, 40);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 5;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.btnShowHistory.Leave += new System.EventHandler(btnShowHistory_Leave);
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(524, 20);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(58, 15);
		this.label53.TabIndex = 87;
		this.label53.Text = "Customer";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(366, 46);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(64, 15);
		this.label1.TabIndex = 86;
		this.label1.Text = "To Doc No";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(366, 20);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(79, 15);
		this.label12.TabIndex = 85;
		this.label12.Text = "From Doc No";
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.Location = new System.Drawing.Point(4, 46);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(79, 15);
		this.label57.TabIndex = 83;
		this.label57.Text = "From,To Date";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance53).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(588, 14);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(235, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 4;
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(445, 42);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(75, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.Tag = "int";
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(445, 16);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(75, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.Tag = "int";
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(223, 42);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(140, 22);
		this.ToDateHistory.TabIndex = 1;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label9);
		this.panel6.Controls.Add(this.toolStrip2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1070, 52);
		this.panel6.TabIndex = 28;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(0, 30);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(254, 19);
		this.label9.TabIndex = 24;
		this.label9.Text = "PURCHASE INVOICE HISTORY";
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.btnRefreshHistory, this.btnLoadAll });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
		this.toolStrip2.Size = new System.Drawing.Size(1070, 27);
		this.toolStrip2.TabIndex = 2;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(51, 24);
		this.btnLoadAll.Text = "LoadAll";
		this.btnLoadAll.Visible = false;
		this.btnLoadAll.Click += new System.EventHandler(btnLoadAll_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1084, 761);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "PurchaseInvoiceStoreManagement";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(PurchaseInvoiceStoreManagement_FormClosing);
		base.Load += new System.EventHandler(InvfrmPurchaseInvoice_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvfrmPurchaseInvoice_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabForm.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDiscountAccount).EndInit();
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxAccount).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdGLedger).EndInit();
		this.panel22.ResumeLayout(false);
		this.panel22.PerformLayout();
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdFreight).EndInit();
		this.panel20.ResumeLayout(false);
		this.panel20.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsuppliername).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommagent).EndInit();
		this.panel3.ResumeLayout(false);
		this.panelpurinv.ResumeLayout(false);
		this.panelpurinv.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabHistory.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel14.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
