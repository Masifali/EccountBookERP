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
using Architecture.Model.Inventory;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Contractor_Wages;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Production;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.StoreManagement;

public class frmStockTransferManual : Form
{
	private Rightsobjects formrights = new Rightsobjects();

	private int[] AryColWidth;

	private DropDownBind DDL = new DropDownBind();

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private int updateDetailIndex;

	private bool IsApproved;

	public int RecId;

	private bool WagesStatus;

	private bool StockTransferFinancialEffectIsActive;

	private int DefaultDaysToLessFromHistoryFromDate;

	private DataTable table = new DataTable();

	private DataTable dtExp = new DataTable();

	private DataTable dtItem = new DataTable();

	private DataTable dtAccountlst = new DataTable();

	private bool FIFOCGSFlag;

	private bool MultiBranchFeature;

	private bool WagesActiveOrInActive;

	private string DetailRowsRemoveIds = "";

	private int TransferTypeId;

	public UserAccount UserAccount = new UserAccount();

	private LoadavailableTransactionsForIssuance LoadTransactions;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private TabPage tabPage2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private ToolStripButton btnprint;

	private Panel panel4;

	private Label label33;

	private Label label13;

	private Label label18;

	private DateTimePicker DocDate;

	private Label label17;

	private TextBox txtdocno;

	private Label label5;

	private Button btnplus;

	private TextBox txtQty;

	private Label label45;

	private Label label38;

	private Label label36;

	private Label label37;

	private Panel panel6;

	internal UltraCombo cmbCropYear;

	internal UltraCombo cmbPackingType;

	internal UltraCombo CmbPackUom;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	private Button btnUpdateDetail;

	private Button btnCancelUpdateDetial;

	internal GridEX grdhistory;

	private ToolStripButton btnRefresh;

	internal UltraCombo combproject1;

	internal UltraCombo cmbbranch1;

	internal UltraCombo cmbItemName;

	private Label label2;

	internal UltraCombo CmbWareHouseFrom;

	private Label label3;

	internal GridEX grd;

	private Label label6;

	private Label label4;

	private Label label1;

	private TextBox txtHeadNetWeight;

	private TextBox txtRemarksDetail;

	private Label label12;

	internal UltraCombo cmbWareHouseTo;

	private Label label11;

	private TextBox txtDetailNetWeight;

	private Label label10;

	private TextBox txtAdLsWeight;

	private Label label9;

	private TextBox txtGrossWeight;

	private Label label8;

	internal UltraCombo cmbJobLot;

	private Label label7;

	private Label label14;

	internal UltraCombo CmbTransferType;

	private Panel panel3;

	private CtrlGrdBar ctrlGrdBar4;

	private CtrlGrdBar ctrlGrdBar5;

	private TextBox txtWorkingReportNo;

	private Label label15;

	private CheckBox ChkBoxPrintPreview;

	private TextBox txtEbTotal;

	private Label label19;

	private TextBox txtEbUnit;

	private Label label16;

	private Label label20;

	private TextBox txtAvailableStock;

	private ToolStripButton btnLoadInvoices;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private TabPage tabPage4;

	internal GridEX grdOtherCharges;

	private ToolStripButton btnAttachment;

	internal UltraCombo CmbJobLotTo;

	private Label label21;

	private TextBox txtAvgRate;

	private Label lblAvgRate;

	private TextBox txtAmount;

	private Label lblAmount;

	private Panel panel12;

	private Label label24;

	private Panel panel5;

	private Panel panel11;

	private Label label23;

	private Panel panel14;

	private Panel panel13;

	private Label label22;

	private GroupBox groupBox2;

	private GroupBox groupBox1;

	private TextBox cmbGatePass;

	private TextBox cmbTicketNo;

	private Button BtnSameUpdateWarehouse;

	private Panel PanelMessage;

	private ToolStrip ToolStripMessage;

	private ToolStripButton BtnMessage;

	private CheckBox chkUpdateJobLot;

	private CheckBox chkUpdateWareHouse;

	private GroupBox groupBox3;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Button btnshow;

	private Label label27;

	private Label label28;

	private Label label43;

	private Label label62;

	private Label label63;

	internal UltraCombo cmbTransactiontypeHistory;

	private TextBox txtToDocNoHistory;

	private TextBox txtFromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Panel panel8;

	private Panel panel10;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnRefreshHistory;

	private Panel panel21;

	internal GridEX GridDetailHistory;

	private Panel panel18;

	private Label label29;

	private CtrlGrdBar ctrlGrdBar3;

	private Panel panel17;

	private Panel panel20;

	private Label label25;

	private CtrlGrdBar ctrlGrdBar7;

	private Panel panel16;

	private Label label26;

	private Label label30;

	private TextBox txtRemarksHead;

	private ToolStripButton btnDelete;

	private ToolStripButton btnGenerateRate;

	public frmStockTransferManual(UserAccount user)
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void PurchsaeOrder_Load(object sender, EventArgs e)
	{
		try
		{
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formrights.DoHaveSaveRight;
			btnupdate.Enabled = formrights.DoHaveUpdateRights;
			btnDelete.Enabled = formrights.DoHaveCanDelete;
			btnDelete.Visible = false;
			lblAvgRate.Visible = false;
			lblAmount.Visible = false;
			txtAvgRate.Visible = false;
			txtAmount.Visible = false;
			tabControl2.TabPages.Remove(tabPage4);
			StockTransferFinancialEffectIsActive = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("StockTransferFinancialEffectIsActive"));
			WagesStatus = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("WagesCompulsoryOnStockTransfer"));
			if (StockTransferFinancialEffectIsActive)
			{
				tabControl2.TabPages.Add(tabPage4);
				lblAvgRate.Visible = true;
				lblAmount.Visible = true;
				txtAvgRate.Visible = true;
				txtAmount.Visible = true;
			}
			FIFOCGSFlag = CommonServices.GetERPFeatureById(5);
			MultiBranchFeature = CommonServices.GetERPFeatureById(11);
			if (TransferTypeId == 0)
			{
				((Control)(object)CmbTransferType).Enabled = true;
				HistoryComboFill();
				((Control)(object)cmbTransactiontypeHistory).Enabled = true;
			}
			else if (TransferTypeId == 51)
			{
				CmbTransferType.Value = 1;
				((Control)(object)CmbTransferType).Enabled = false;
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("TransferType");
				dt.Rows.Add(1, "Inward");
				DDL.BindDDL(dt, cmbTransactiontypeHistory, "Id", "TransferType", "TransferType", ZeroIndex: false);
				((Control)(object)cmbTransactiontypeHistory).Enabled = false;
				cmbTransactiontypeHistory.Value = 1;
			}
			else if (TransferTypeId == 91)
			{
				((Control)(object)CmbTransferType).Enabled = false;
				CmbTransferType.Value = 2;
				DataTable dt2 = new DataTable();
				dt2.Columns.Add("Id");
				dt2.Columns.Add("TransferType");
				dt2.Rows.Add(2, "Outward");
				DDL.BindDDL(dt2, cmbTransactiontypeHistory, "Id", "TransferType", "TransferType", ZeroIndex: false);
				((Control)(object)cmbTransactiontypeHistory).Enabled = false;
				cmbTransactiontypeHistory.Value = 2;
			}
			else if (TransferTypeId == 74)
			{
				((Control)(object)CmbTransferType).Enabled = false;
				CmbTransferType.Value = 3;
				DataTable dt3 = new DataTable();
				dt3.Columns.Add("Id");
				dt3.Columns.Add("TransferType");
				dt3.Rows.Add(3, "MoveOrder");
				DDL.BindDDL(dt3, cmbTransactiontypeHistory, "Id", "TransferType", "TransferType", ZeroIndex: false);
				((Control)(object)cmbTransactiontypeHistory).Enabled = false;
				cmbTransactiontypeHistory.Value = 3;
			}
			WagesRefDocumentsStatus obj = clsGlobalVariables.WagesRefDocumentsStatusList.Find((WagesRefDocumentsStatus row) => row.RefDocumentTypeId == 806);
			WagesActiveOrInActive = obj != null && obj.IsActive;
			dtViewAttachments.Columns.Add("AttchmentPath");
			table.Columns.Add("Id");
			table.Columns.Add("RefDocumentTypeId");
			table.Columns.Add("RefDocNoId");
			table.Columns.Add("RefDocSubIdNo");
			table.Columns.Add("ItemId");
			table.Columns.Add("ItemName");
			table.Columns.Add("PackTypeId");
			table.Columns.Add("PackType");
			table.Columns.Add("CropYear");
			table.Columns.Add("JobLotId");
			table.Columns.Add("JobLot");
			table.Columns.Add("JobLotIdTo");
			table.Columns.Add("JobLotTo");
			table.Columns.Add("QTY", typeof(double));
			table.Columns.Add("PackUOMId");
			table.Columns.Add("PackUOM");
			table.Columns.Add("GrossWeight", typeof(double));
			table.Columns.Add("EbUnit", typeof(double));
			table.Columns.Add("EbTotal", typeof(double));
			table.Columns.Add("Ad/LsWeight", typeof(double));
			table.Columns.Add("NetWeight", typeof(double));
			table.Columns.Add("BalQty", typeof(double));
			table.Columns.Add("BalWeight", typeof(double));
			table.Columns.Add("WareHouseFromId");
			table.Columns.Add("WareHouseFrom");
			table.Columns.Add("WareHouseToId");
			table.Columns.Add("WareHouseTo");
			table.Columns.Add("Remarks");
			table.Columns.Add("ItemRate", typeof(double));
			table.Columns.Add("RateUOMId");
			table.Columns.Add("RateUOM");
			table.Columns.Add("ItemAmount", typeof(double));
			table.Columns.Add("Expense", typeof(double));
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			dtAccountlst.Columns.Add("Id");
			dtAccountlst.Columns.Add("AccountTitle");
			AccountsFill();
			dtExp.Columns.Add("Account");
			dtExp.Columns.Add("Percentage");
			dtExp.Columns.Add("Qty", typeof(double));
			dtExp.Columns.Add("Rate", typeof(double));
			dtExp.Columns.Add("Amount", typeof(double));
			dtExp.Columns.Add("Remarks");
			AddRowInExpenseGrid();
			grdOtherCharges.RetrieveStructure();
			grdExpenseSettings();
			CombProjectFill();
			DocumentNoFill();
			WareHouseFill();
			GetPackingType();
			TransferTypeFill();
			GetCropYear();
			GetJobLot();
			ItemDetailFill();
			DocDate.Focus();
			btnupdate.Visible = false;
			btnsave.Visible = true;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			ChkBoxPrintPreview.Checked = true;
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			HistoryComboFill();
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
		if (txtdocno.Text.Trim() == string.Empty || txtdocno.Text.Trim() == "0")
		{
			MessageBox.Show("DocNo Field is Required");
			txtdocno.Focus();
			return false;
		}
		if (((UltraGridBase)CmbTransferType).ActiveRow == null)
		{
			MessageBox.Show("Transfer Type Field is Required");
			CmbTransferType.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)CmbWareHouseFrom).ActiveRow == null)
		{
			MessageBox.Show("WarehouseFrom field is required");
			CmbWareHouseFrom.Focus();
			return false;
		}
		if (((UltraGridBase)cmbWareHouseTo).ActiveRow == null)
		{
			MessageBox.Show("WarehouseTo field is required");
			cmbWareHouseTo.Focus();
			return false;
		}
		if (((UltraGridBase)cmbItemName).ActiveRow == null)
		{
			MessageBox.Show("Item Name field is required");
			cmbItemName.Focus();
			return false;
		}
		if (((UltraGridBase)cmbPackingType).ActiveRow == null)
		{
			MessageBox.Show("PackingType field is required");
			cmbPackingType.Focus();
			return false;
		}
		if (((UltraGridBase)cmbCropYear).ActiveRow == null)
		{
			MessageBox.Show("ItemName field is required");
			cmbCropYear.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackUom).ActiveRow == null)
		{
			MessageBox.Show("PackUOM field is required");
			CmbPackUom.Focus();
			return false;
		}
		if (((UltraGridBase)cmbJobLot).ActiveRow == null)
		{
			MessageBox.Show("JobLot From field is required");
			cmbJobLot.Focus();
			return false;
		}
		if (((UltraGridBase)CmbJobLotTo).ActiveRow == null)
		{
			MessageBox.Show("JobLotTo field is required");
			CmbJobLotTo.Focus();
			return false;
		}
		if (txtQty.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtQty.Text) == 0.0)
		{
			MessageBox.Show("Qty field is required");
			txtQty.Focus();
			return false;
		}
		if (txtGrossWeight.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtGrossWeight.Text) == 0.0)
		{
			MessageBox.Show("GrossWeight field is required");
			txtGrossWeight.Focus();
			return false;
		}
		if (StockTransferFinancialEffectIsActive)
		{
			if (txtAvgRate.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtAvgRate.Text) == 0.0)
			{
				MessageBox.Show("AvgRate field is required");
				txtAvgRate.Focus();
				return false;
			}
			if (txtAmount.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtAmount.Text) == 0.0)
			{
				MessageBox.Show("Amount field is required");
				txtAmount.Focus();
				return false;
			}
		}
		if (txtDetailNetWeight.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtDetailNetWeight.Text) == 0.0)
		{
			MessageBox.Show("NetWeight field is required");
			txtDetailNetWeight.Focus();
			return false;
		}
		return true;
	}

	private void AccountsFill()
	{
		try
		{
			DataTable dtcoa = CommonServices.CoaAllocationGetAllServiceBind();
			if (dtcoa.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtcoa.Rows.Count; i++)
			{
				if (Convert.ToInt32(dtcoa.Rows[i]["AccountTypeId"]) != 2 && Convert.ToInt32(dtcoa.Rows[i]["AccountTypeId"]) != 11 && Convert.ToInt32(dtcoa.Rows[i]["AccountTypeId"]) != 12 && Convert.ToInt32(dtcoa.Rows[i]["AccountTypeId"]) != 15)
				{
					dtAccountlst.Rows.Add(dtcoa.Rows[i]["Id"], dtcoa.Rows[i]["AccountTitle"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInExpenseGrid()
	{
		dtExp.Rows.Add(0, 0, 0, 0, 0, "");
		grdOtherCharges.DataSource = dtExp;
	}

	private void grdExpenseSettings()
	{
		try
		{
			grdOtherCharges.RootTable.Columns["Amount"].Caption = "Credit";
			grdOtherCharges.RootTable.Columns["Account"].Caption = "AccountTitle";
			grdOtherCharges.RootTable.Columns["Percentage"].Caption = "%";
			grdOtherCharges.RootTable.Columns["Account"].ColumnType = (ColumnType)0;
			grdOtherCharges.RootTable.Columns["Account"].HasValueList = true;
			grdOtherCharges.RootTable.Columns["Account"].LimitToList = true;
			grdOtherCharges.RootTable.Columns["Account"].EditType = (EditType)4;
			grdOtherCharges.RootTable.Columns["Account"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "Id", "AccountTitle");
			grdOtherCharges.RootTable.Columns["Account"].Width = 150;
			grdOtherCharges.RootTable.Columns["Remarks"].Width = 120;
			grdOtherCharges.RootTable.Columns["Amount"].Width = 80;
			grdOtherCharges.RootTable.Columns["Percentage"].Width = 50;
			grdOtherCharges.RootTable.Columns.Add("Delete");
			grdOtherCharges.RootTable.Columns["Delete"].Key = "Delete";
			grdOtherCharges.RootTable.Columns["Delete"].Caption = "X";
			grdOtherCharges.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOtherCharges.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdOtherCharges.RootTable.Columns["Delete"].ButtonText = "X";
			grdOtherCharges.RootTable.Columns["Delete"].Width = 20;
			grdOtherCharges.RootTable.Columns.Add("Add");
			grdOtherCharges.RootTable.Columns["Add"].Key = "Add";
			grdOtherCharges.RootTable.Columns["Add"].Caption = "+";
			grdOtherCharges.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOtherCharges.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdOtherCharges.RootTable.Columns["Add"].ButtonText = "+";
			grdOtherCharges.RootTable.Columns["Add"].Width = 20;
			grdOtherCharges.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdOtherCharges.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdOtherCharges.RootTable.Columns["Amount"].FormatString = "#,#.###";
			grdOtherCharges.RootTable.Columns["Amount"].TotalFormatString = "#,#.###";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OtherExpensesProportion()
	{
		try
		{
			if (grd.GetRows().Length != 0)
			{
				double NetWeight = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["NetWeight"], (AggregateFunction)2).ToString());
				double CreditForExpense = Conversion.ToDouble((object)grdOtherCharges.GetTotal(grdOtherCharges.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
				if (CreditForExpense > 0.0)
				{
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow obj in rows)
					{
						obj.BeginEdit();
						double Weight = Conversion.ToDouble((object)obj.Cells["NetWeight"].Value.ToString());
						obj.Cells["Expense"].Value = CreditForExpense / NetWeight * Weight;
					}
				}
				else
				{
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow obj2 in rows)
					{
						obj2.BeginEdit();
						obj2.Cells["Expense"].Value = 0;
					}
				}
			}
			grd.UpdateData();
			grdOtherCharges.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OtherChargesAmountUpdate()
	{
		try
		{
			GridEXRow[] rows = grdOtherCharges.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (Conversion.ToDouble(item.Cells["Amount"].Value) > 0.0 && item.Cells["Percentage"].Value.ToString() != string.Empty)
				{
					item.BeginEdit();
					double num = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2).ToString());
					double Percentage = Conversion.ToDouble((object)item.Cells["Percentage"].Value.ToString());
					double TotlaPercentage = num / 100.0 * Percentage;
					if (TotlaPercentage > 0.0)
					{
						item.Cells["Amount"].Value = Conversion.ToDouble((object)TotlaPercentage);
					}
					item.Cells["Qty"].Value = 0;
					item.Cells["Rate"].Value = 0;
					item.EndEdit();
				}
			}
			grdOtherCharges.UpdateData();
			OtherExpensesProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		btnplus.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelUpdateDetial.Visible = false;
	}

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				table.Rows[updateDetailIndex]["ItemId"] = cmbItemName.Value;
				table.Rows[updateDetailIndex]["ItemName"] = ((Control)(object)cmbItemName).Text;
				table.Rows[updateDetailIndex]["PackTypeId"] = cmbPackingType.Value;
				table.Rows[updateDetailIndex]["PackType"] = ((Control)(object)cmbPackingType).Text;
				table.Rows[updateDetailIndex]["CropYear"] = ((Control)(object)cmbCropYear).Text;
				table.Rows[updateDetailIndex]["JobLotId"] = cmbJobLot.Value;
				table.Rows[updateDetailIndex]["JobLot"] = ((Control)(object)cmbJobLot).Text;
				table.Rows[updateDetailIndex]["JobLotIdTo"] = CmbJobLotTo.Value;
				table.Rows[updateDetailIndex]["JobLotTo"] = ((Control)(object)CmbJobLotTo).Text;
				table.Rows[updateDetailIndex]["QTY"] = Conversion.ToDouble((object)txtQty.Text);
				table.Rows[updateDetailIndex]["PackUOMId"] = CmbPackUom.Value;
				table.Rows[updateDetailIndex]["PackUOM"] = ((Control)(object)CmbPackUom).Text;
				table.Rows[updateDetailIndex]["GrossWeight"] = Conversion.ToDouble((object)txtGrossWeight.Text);
				table.Rows[updateDetailIndex]["EbUnit"] = Conversion.ToDouble((object)txtEbUnit.Text);
				table.Rows[updateDetailIndex]["EbTotal"] = Conversion.ToDouble((object)txtEbTotal.Text);
				table.Rows[updateDetailIndex]["Ad/LsWeight"] = Conversion.ToDouble((object)txtAdLsWeight.Text);
				table.Rows[updateDetailIndex]["NetWeight"] = Conversion.ToDouble((object)txtDetailNetWeight.Text);
				table.Rows[updateDetailIndex]["WareHouseFromId"] = CmbWareHouseFrom.Value;
				table.Rows[updateDetailIndex]["WareHouseFrom"] = ((Control)(object)CmbWareHouseFrom).Text;
				table.Rows[updateDetailIndex]["WareHouseToId"] = cmbWareHouseTo.Value;
				table.Rows[updateDetailIndex]["WareHouseTo"] = ((Control)(object)cmbWareHouseTo).Text;
				table.Rows[updateDetailIndex]["Remarks"] = txtRemarksDetail.Text;
				if (Conversion.ToInt(table.Rows[updateDetailIndex]["RefDocumentTypeId"]) == 0)
				{
					table.Rows[updateDetailIndex]["ItemRate"] = Conversion.ToDouble((object)txtAvgRate.Text);
					table.Rows[updateDetailIndex]["ItemAmount"] = Conversion.ToDouble((object)txtAmount.Text);
				}
				grd.DataSource = table;
				btnplus.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				ItemAmountCalculation();
				FactoryWeightCalculation();
				ResetDetail();
				if (((Control)(object)CmbTransferType).Text == "Outward")
				{
					((UltraGridBase)cmbItemName).DataSource = null;
				}
				OtherChargesAmountUpdate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				if (Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) > 0)
				{
					((Control)(object)CmbWareHouseFrom).Enabled = false;
					((Control)(object)cmbItemName).Enabled = false;
					((Control)(object)CmbPackUom).Enabled = false;
					((Control)(object)cmbCropYear).Enabled = false;
					((Control)(object)cmbJobLot).Enabled = false;
				}
				else
				{
					txtAvgRate.Text = item.Cells["ItemRate"].Value.ToString();
					txtAmount.Text = item.Cells["ItemAmount"].Value.ToString();
					((Control)(object)CmbWareHouseFrom).Enabled = true;
					((Control)(object)cmbItemName).Enabled = true;
					((Control)(object)CmbPackUom).Enabled = true;
					((Control)(object)cmbCropYear).Enabled = true;
					((Control)(object)cmbJobLot).Enabled = true;
				}
				CmbWareHouseFrom.Value = item.Cells["WareHouseFromId"].Value.ToString();
				((Control)(object)CmbWareHouseFrom).Text = item.Cells["WareHouseFrom"].Value.ToString();
				cmbWareHouseTo.Value = item.Cells["WareHouseToId"].Value.ToString();
				((Control)(object)cmbWareHouseTo).Text = item.Cells["WareHouseTo"].Value.ToString();
				cmbItemName.Value = item.Cells["ItemId"].Value.ToString();
				cmbPackingType.Value = item.Cells["PackTypeId"].Value.ToString();
				((Control)(object)cmbPackingType).Text = item.Cells["PackType"].Text.ToString();
				((Control)(object)cmbCropYear).Text = item.Cells["CropYear"].Value.ToString();
				cmbJobLot.Value = item.Cells["JobLotId"].Value.ToString();
				((Control)(object)cmbJobLot).Text = item.Cells["JobLot"].Value.ToString();
				CmbJobLotTo.Value = item.Cells["JobLotIdTo"].Value.ToString();
				((Control)(object)CmbJobLotTo).Text = item.Cells["JobLotTo"].Value.ToString();
				txtQty.Text = item.Cells["QTY"].Value.ToString();
				bindRateUomAndItemPackUom(Conversion.ToInt(cmbItemName.Value));
				CmbPackUom.Value = item.Cells["PackUOMId"].Value.ToString();
				((Control)(object)CmbPackUom).Text = item.Cells["PackUOM"].Value.ToString();
				txtGrossWeight.Text = item.Cells["GrossWeight"].Value.ToString();
				txtEbUnit.Text = item.Cells["EbUnit"].Value.ToString();
				txtEbTotal.Text = item.Cells["EbTotal"].Value.ToString();
				txtAdLsWeight.Text = item.Cells["Ad/LsWeight"].Value.ToString();
				txtDetailNetWeight.Text = item.Cells["NetWeight"].Value.ToString();
				txtAvgRate.Text = item.Cells["ItemRate"].Value.ToString();
				txtAmount.Text = item.Cells["ItemAmount"].Value.ToString();
				txtRemarksDetail.Text = item.Cells["Remarks"].Value.ToString();
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

	private void btnplus_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationDetail())
			{
				return;
			}
			if (grd.GetRows().Length != 0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value) > 0 || Conversion.ToInt(r.Cells["RefDocNoId"].Value) > 0 || Conversion.ToInt(r.Cells["RefDocSubIdNo"].Value) > 0)
					{
						throw new Exception("Record cannot be add in grid because some rows already exist in grid of Loader");
					}
				}
			}
			table.Rows.Add(0, 0, 0, 0, cmbItemName.Value, ((Control)(object)cmbItemName).Text.Trim(), cmbPackingType.Value, ((Control)(object)cmbPackingType).Text.Trim(), ((Control)(object)cmbCropYear).Text.Trim(), cmbJobLot.Value, ((Control)(object)cmbJobLot).Text.Trim(), CmbJobLotTo.Value, ((Control)(object)CmbJobLotTo).Text.Trim(), Conversion.ToDouble((object)txtQty.Text.Trim()), CmbPackUom.Value, ((Control)(object)CmbPackUom).Text.Trim(), Conversion.ToDouble((object)txtGrossWeight.Text.Trim()), Conversion.ToDouble((object)txtEbUnit.Text.Trim()), Conversion.ToDouble((object)txtEbTotal.Text.Trim()), Conversion.ToDouble((object)txtAdLsWeight.Text.Trim()), Conversion.ToDouble((object)txtDetailNetWeight.Text.Trim()), 0, 0, CmbWareHouseFrom.Value, ((Control)(object)CmbWareHouseFrom).Text.Trim(), cmbWareHouseTo.Value, ((Control)(object)cmbWareHouseTo).Text.Trim(), txtRemarksDetail.Text.Trim(), Conversion.ToDouble((object)txtAvgRate.Text), 0, 40, Conversion.ToDouble((object)txtAmount.Text), 0);
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			OtherExpensesProportion();
			FactoryWeightCalculation();
			ResetDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void Insert()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Expected O, but got Unknown
		//IL_0816: Unknown result type (might be due to invalid IL or missing references)
		//IL_081d: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			if (grd.GetRows().Count() == 0)
			{
				MessageBox.Show("Grid Record Not Found");
				return;
			}
			InvStockTransferHeader sh = new InvStockTransferHeader();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				sh.Id = RecId;
				sh.ModifyUser = UserAccount.ID;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			GridEXRow[] rows = grdOtherCharges.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				if (!(Conversion.ToDouble(r3.Cells["Amount"].Value) > 0.0))
				{
					continue;
				}
				if (Conversion.ToInt(r3.Cells["Account"].Value) == 0)
				{
					throw new Exception("Please Select an Account Against OtherCharges Amount First");
				}
				if (Conversion.ToDouble(r3.Cells["Amount"].Value) > 0.0)
				{
					string FrRemarks = Conversion.ToString(r3.Cells["Remarks"].Value);
					if (FrRemarks == string.Empty || FrRemarks == null)
					{
						tabControl2.SelectedIndex = 1;
						((Control)(object)grdOtherCharges).Focus();
						throw new Exception("Grid Charge to Product Remarks required Please Check");
					}
				}
			}
			sh.DocumentTypeId = 806;
			sh.BranchesId = Conversion.ToInt((object)UserAccount.BranchesId);
			sh.ProjectsId = Conversion.ToInt((object)UserAccount.BranchesId);
			sh.DocDate = Conversion.ToDateTime((object)DocDate.Value);
			sh.DocNo = Conversion.ToInt((object)txtdocno.Text);
			sh.NoOfBranches = 0;
			sh.FromBranchId = 0;
			sh.ToBranchId = 0;
			sh.RemarksHeader = Conversion.ToString((object)txtRemarksHead.Text);
			sh.GatePassId = Conversion.ToInt((object)cmbGatePass.Text);
			sh.WbTicketId = Conversion.ToInt((object)cmbTicketNo.Text);
			sh.WbNetWeight = Conversion.ToDouble((object)txtHeadNetWeight.Text);
			sh.TransferType = Conversion.ToString((object)((Control)(object)CmbTransferType).Text);
			sh.OrganizationId = UserAccount.OrganizationId;
			sh.CompanyId = UserAccount.CompanyId;
			sh.EntryUser = UserAccount.ID;
			sh.ModifyUser = UserAccount.ID;
			sh.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (DetailRowsRemoveIds != null && DetailRowsRemoveIds != string.Empty)
			{
				sh.DetailRowsRemoveIds = DetailRowsRemoveIds;
			}
			sh.InvStockTransferDetail = new List<InvStockTransferDetail>();
			double GrossWeight = 0.0;
			rows = grd.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				InvStockTransferDetail vd = new InvStockTransferDetail();
				vd.Id = Conversion.ToInt(r2.Cells["Id"].Value);
				vd.RefDocumentTypeId = Conversion.ToInt(r2.Cells["RefDocumentTypeId"].Value);
				vd.RefDocNoId = Conversion.ToInt(r2.Cells["RefDocNoId"].Value);
				vd.RefDocSubIdNo = Conversion.ToInt(r2.Cells["RefDocSubIdNo"].Value);
				if (vd.RefDocumentTypeId > 0 && Conversion.ToDouble(r2.Cells["NetWeight"].Value) > Conversion.ToDouble(r2.Cells["BalWeight"].Value))
				{
					throw new Exception("NetWeight cannot be greater than BalWeight Please Check!");
				}
				if (Conversion.ToDouble(r2.Cells["ItemId"].Value) == 0.0)
				{
					throw new Exception("Item Name Field Required In Detail Grid...");
				}
				vd.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
				if (Conversion.ToDouble(r2.Cells["JobLotId"].Value) == 0.0)
				{
					throw new Exception("JobLot From Field Required In Detail Grid...");
				}
				vd.JobLotId = Conversion.ToInt(r2.Cells["JobLotId"].Value);
				if (Conversion.ToDouble(r2.Cells["JobLotIdTo"].Value) == 0.0)
				{
					throw new Exception("JobLot To Field Required In Detail Grid...");
				}
				vd.JobLotIdTo = Conversion.ToInt(r2.Cells["JobLotIdTo"].Value);
				if (Conversion.ToInt(r2.Cells["WareHouseFromId"].Value) == 0)
				{
					throw new Exception("WareHouseFrom  Field Required In Detail Grid...");
				}
				vd.FromWarehouseId = Conversion.ToInt(r2.Cells["WareHouseFromId"].Value);
				if (Conversion.ToInt(r2.Cells["WareHouseToId"].Value) == 0)
				{
					throw new Exception("WareHouseTo Field Required In Detail Grid...");
				}
				vd.ToWarehouseId = Conversion.ToInt(r2.Cells["WareHouseToId"].Value);
				vd.InvPackingTypeId = Conversion.ToInt(r2.Cells["PackTypeId"].Value);
				vd.CropYear = Conversion.ToString(r2.Cells["CropYear"].Value);
				vd.Qty = Conversion.ToDouble(r2.Cells["QTY"].Value);
				vd.PackUomId = Conversion.ToInt(r2.Cells["PackUOMId"].Value);
				vd.GrossWeight = Conversion.ToDouble(r2.Cells["GrossWeight"].Value);
				GrossWeight += vd.GrossWeight;
				vd.AdLsWeight = Conversion.ToDouble(r2.Cells["Ad/LsWeight"].Value);
				vd.EbUnit = Conversion.ToDouble(r2.Cells["EbUnit"].Value);
				vd.EbTotal = Conversion.ToDouble(r2.Cells["EbTotal"].Value);
				vd.NetWeight = Conversion.ToDouble(r2.Cells["NetWeight"].Value);
				vd.ItemRate = Conversion.ToDouble(r2.Cells["ItemRate"].Value);
				vd.RateUomId = Conversion.ToInt(r2.Cells["RateUOMId"].Value);
				vd.ItemAmount = Conversion.ToDouble(r2.Cells["ItemAmount"].Value);
				vd.ExpenseAmount = Conversion.ToDouble(r2.Cells["Expense"].Value);
				vd.RemarksDetail = Conversion.ToString((object)r2.Cells["Remarks"].Text);
				if (StockTransferFinancialEffectIsActive)
				{
					if (vd.ItemRate <= 0.0)
					{
						throw new Exception("AvgRate field required");
					}
					if (vd.ItemAmount <= 0.0)
					{
						throw new Exception("Amount field required");
					}
				}
				sh.InvStockTransferDetail.Add(vd);
			}
			sh.InvStockTransferExpense = new List<InvStockTransferExpense>();
			rows = grdOtherCharges.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt((object)r.Cells["Account"].Value.ToString()) != 0)
				{
					InvStockTransferExpense PF = new InvStockTransferExpense();
					PF.AccountId = Conversion.ToInt(r.Cells["Account"].Value);
					PF.Percentage = Conversion.ToDouble(r.Cells["Percentage"].Value);
					PF.Qty = Conversion.ToDouble(r.Cells["Qty"].Value);
					PF.Rate = Conversion.ToDouble(r.Cells["Rate"].Value);
					PF.Amount = Conversion.ToDouble(r.Cells["Amount"].Value);
					PF.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
					sh.InvStockTransferExpense.Add(PF);
				}
			}
			if (Conversion.ToDouble((object)txtHeadNetWeight.Text.Trim()) != GrossWeight)
			{
				MessageBox.Show("GrossWeight and WeighBridge Weight Not Match", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			sh.AttachmentsList = new List<DMSAttachments>();
			sh.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<InvStockTransferHeader>(sh, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			int success = InvStockTransferHeader.Save(sh);
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + sh.DocNo);
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, 806, base.Name);
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + sh.DocNo);
			}
			if (WagesStatus && WagesActiveOrInActive)
			{
				frmwagesBillHeader frmwagesBillHeader = new frmwagesBillHeader(UserAccount);
				frmwagesBillHeader.RefDocTypeId = 806;
				frmwagesBillHeader.RefDocId = success;
				frmwagesBillHeader.GrossWeightTotal = Conversion.ToDouble((object)txtHeadNetWeight.Text);
				frmwagesBillHeader.ShowDialog();
				frmwagesBillHeader.BringToFront();
			}
			Reset();
			if (ChkBoxPrintPreview.Checked)
			{
				CommonServices.StockTransferSlip407(success);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

	private void btnupdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (IsApproved)
			{
				MessageBox.Show("Approved Record Not Update");
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

	public void ReadById(int ID)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		try
		{
			RecId = ID;
			DetailRowsRemoveIds = "";
			InvStockTransferHeader Stock = new InvStockTransferHeader();
			Stock = InvStockTransferHeader.GetByID(ID);
			cmbbranch1.Value = Conversion.ToInt((object)Stock.BranchesId);
			combproject1.Value = Conversion.ToInt((object)Stock.ProjectsId);
			tabControl1.SelectedIndex = 0;
			DocDate.Value = Conversion.ToDateTime((object)Stock.DocDate);
			txtdocno.Text = Conversion.ToString((object)Stock.DocNo);
			txtRemarksHead.Text = Conversion.ToString((object)Stock.RemarksHeader);
			txtHeadNetWeight.Text = Conversion.ToString((object)Stock.WbNetWeight);
			cmbGatePass.Text = Conversion.ToString((object)Stock.GatePassId);
			cmbTicketNo.Text = Conversion.ToString((object)Stock.WbTicketId);
			((Control)(object)CmbTransferType).Text = Conversion.ToString((object)Stock.TransferType);
			txtWorkingReportNo.Text = Conversion.ToString((object)Stock.WorkingReportNo);
			IsApproved = Stock.IsApproved;
			table.Clear();
			for (int j = 0; j < Stock.InvStockTransferDetail.Count; j++)
			{
				table.Rows.Add(Stock.InvStockTransferDetail[j].Id, Stock.InvStockTransferDetail[j].RefDocumentTypeId, Stock.InvStockTransferDetail[j].RefDocNoId, Stock.InvStockTransferDetail[j].RefDocSubIdNo, Stock.InvStockTransferDetail[j].ItemId, Stock.InvStockTransferDetail[j].ItemName, Stock.InvStockTransferDetail[j].InvPackingTypeId, Stock.InvStockTransferDetail[j].PackTypeDesc, Stock.InvStockTransferDetail[j].CropYear, Stock.InvStockTransferDetail[j].JobLotId, Stock.InvStockTransferDetail[j].JobLotDescription, Stock.InvStockTransferDetail[j].JobLotIdTo, Stock.InvStockTransferDetail[j].JobLotTo, Stock.InvStockTransferDetail[j].Qty, Stock.InvStockTransferDetail[j].PackUomId, Stock.InvStockTransferDetail[j].Equivalent, Stock.InvStockTransferDetail[j].GrossWeight, Stock.InvStockTransferDetail[j].EbUnit, Stock.InvStockTransferDetail[j].EbTotal, Stock.InvStockTransferDetail[j].AdLsWeight, Stock.InvStockTransferDetail[j].NetWeight, Stock.InvStockTransferDetail[j].Qty, Stock.InvStockTransferDetail[j].NetWeight, Stock.InvStockTransferDetail[j].FromWarehouseId, Stock.InvStockTransferDetail[j].FromWareHouseName, Stock.InvStockTransferDetail[j].ToWarehouseId, Stock.InvStockTransferDetail[j].ToWareHouseName, Stock.InvStockTransferDetail[j].RemarksDetail, Stock.InvStockTransferDetail[j].ItemRate, Stock.InvStockTransferDetail[j].RateUomId, Stock.InvStockTransferDetail[j].RateUom, Stock.InvStockTransferDetail[j].ItemAmount, Stock.InvStockTransferDetail[j].ExpenseAmount);
			}
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			dtExp.Rows.Clear();
			for (int i = 0; i < Stock.InvStockTransferExpense.Count; i++)
			{
				dtExp.Rows.Add(Stock.InvStockTransferExpense[i].AccountId, Stock.InvStockTransferExpense[i].Percentage, Stock.InvStockTransferExpense[i].Qty, Stock.InvStockTransferExpense[i].Rate, Stock.InvStockTransferExpense[i].Amount, Stock.InvStockTransferExpense[i].Remarks);
			}
			if (dtExp.Rows.Count == 0)
			{
				AddRowInExpenseGrid();
			}
			grdOtherCharges.DataSource = dtExp;
			grdOtherCharges.RetrieveStructure();
			grdExpenseSettings();
			btnsave.Visible = false;
			btnupdate.Visible = true;
			btnDelete.Visible = formrights.DoHaveCanDelete;
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CombProjectFill()
	{
		try
		{
			DataTable dt = CommonServices.ProjectServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, combproject1, "Id", "ProjectName", "Project Name", false);
				((UltraGridBase)combproject1).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WareHouseFill()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		try
		{
			int ID = Conversion.ToInt(CmbWareHouseFrom.Value);
			int ID2 = Conversion.ToInt(cmbWareHouseTo.Value);
			DataTable dt = WarehousesAllocationToBranch.GetWarehousesAllocatedToBranchByBranchId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dt, CmbWareHouseFrom, "Id", "WareHouseName", "WareHouseName", ZeroIndex: true);
			DDL.BindDDL(dt, cmbWareHouseTo, "Id", "WareHouseName", "WareHouseName", ZeroIndex: true);
			if (ID > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
				{
					CmbWareHouseFrom.Value = ID;
				}
				else
				{
					((Control)(object)CmbWareHouseFrom).Text = string.Empty;
				}
			}
			if (ID2 > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID2))
				{
					cmbWareHouseTo.Value = ID2;
				}
				else
				{
					((Control)(object)cmbWareHouseTo).Text = string.Empty;
				}
			}
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
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		try
		{
			int code = InvStockTransferHeader.GenerateCode(new InvStockTransferHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 806,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
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

	private void ItemDetailFill()
	{
		try
		{
			dtItem = CommonServices.ReadAllItems();
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtItem, cmbItemName, "Id", "ItemName", "Item Name", false);
				((UltraGridBase)cmbItemName).DisplayLayout.Bands[0].Columns[1].Width = 500;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void bindRateUomAndItemPackUom(int ID)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		try
		{
			DataTable dtuomsch = UOMSchedule.SearchByObject(new UOMSchedule
			{
				ItemId = Conversion.ToInt((object)ID),
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtuomsch.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtuomsch, CmbPackUom, "Id", "Equivalent", "Item Pack Unit", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetPackingType()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			new InvPackingType();
			DataTable dt = InvPackingType.Getall();
			if (dt.Rows.Count > 0 && dt != null)
			{
				DDL.BindDDLNew(dt, cmbPackingType, "Id", "PackTypeDesc", "Packing Type", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetCropYear()
	{
		try
		{
			DataTable dt = CommonServices.CropYearGetAllService();
			if (dt.Rows.Count > 0 && dt != null)
			{
				DDL.BindDDLNew(dt, cmbCropYear, "Id", "CropYear", "Crop Year", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TransferTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("TransferType");
			dt.Rows.Add(1, "Inward");
			dt.Rows.Add(2, "Outward");
			dt.Rows.Add(3, "MoveOrder");
			DDL.BindDDLNew(dt, CmbTransferType, "Id", "TransferType", "TransferType", false);
			((UltraGridBase)CmbTransferType).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetJobLot()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		try
		{
			int ID = Conversion.ToInt(cmbJobLot.Value);
			int ID2 = Conversion.ToInt(CmbJobLotTo.Value);
			DataTable dt = JobLotsAllocationToBranch.GetJobLotsAllocatedToBranchByBranchId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId
			});
			if (dt.Rows.Count <= 0 || dt == null)
			{
				return;
			}
			DDL.BindDDLNew(dt, cmbJobLot, "Id", "JobLotDescription", "Job Lot From", true);
			DDL.BindDDLNew(dt, CmbJobLotTo, "Id", "JobLotDescription", "Job Lot To", true);
			if (ID > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
				{
					cmbJobLot.Value = ID;
				}
				else
				{
					((Control)(object)cmbJobLot).Text = string.Empty;
				}
			}
			if (ID2 > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID2))
				{
					CmbJobLotTo.Value = ID2;
				}
				else
				{
					((Control)(object)CmbJobLotTo).Text = string.Empty;
				}
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
			WagesRefDocumentsStatus obj = clsGlobalVariables.WagesRefDocumentsStatusList.Find((WagesRefDocumentsStatus row) => row.RefDocumentTypeId == 806);
			WagesActiveOrInActive = obj != null && obj.IsActive;
			WareHouseFill();
			GetJobLot();
			CombProjectFill();
			ItemDetailFill();
			GetPackingType();
			GetCropYear();
			bindRateUomAndItemPackUom(Conversion.ToInt(cmbItemName.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
		ResetDetail();
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void Reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			DetailRowsRemoveIds = "";
			RecId = 0;
			IsApproved = false;
			((Control)(object)cmbCropYear).Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			cmbGatePass.Text = string.Empty;
			cmbTicketNo.Text = string.Empty;
			txtHeadNetWeight.Text = string.Empty;
			((Control)(object)cmbPackingType).Text = string.Empty;
			txtRemarksHead.Text = string.Empty;
			txtWorkingReportNo.Text = string.Empty;
			txtAvgRate.Text = string.Empty;
			txtAmount.Text = string.Empty;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			btnDelete.Visible = false;
			dtExp.Rows.Clear();
			AddRowInExpenseGrid();
			grdOtherCharges.RetrieveStructure();
			grdExpenseSettings();
			((Control)(object)CmbWareHouseFrom).Enabled = true;
			((Control)(object)cmbItemName).Enabled = true;
			((Control)(object)CmbPackUom).Enabled = true;
			((Control)(object)cmbCropYear).Enabled = true;
			((Control)(object)cmbJobLot).Enabled = true;
			DocumentNoFill();
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
			cmbItemName.Focus();
			((Control)(object)cmbCropYear).Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			txtDetailNetWeight.Text = string.Empty;
			((Control)(object)cmbItemName).Text = string.Empty;
			((Control)(object)cmbJobLot).Text = string.Empty;
			txtGrossWeight.Text = string.Empty;
			txtAdLsWeight.Text = string.Empty;
			((Control)(object)CmbWareHouseFrom).Text = string.Empty;
			((Control)(object)cmbWareHouseTo).Text = string.Empty;
			txtQty.Text = string.Empty;
			((Control)(object)cmbPackingType).Text = string.Empty;
			txtRemarksDetail.Text = string.Empty;
			((Control)(object)CmbJobLotTo).Text = string.Empty;
			txtAvgRate.Text = string.Empty;
			txtAmount.Text = string.Empty;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
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
			grd.ColumnAutoResize = false;
			GridEXRow item = grd.CurrentRow;
			if (item != null && Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) == 0)
			{
				grd.RootTable.Columns["BalQty"].Visible = false;
				grd.RootTable.Columns["BalWeight"].Visible = false;
			}
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grd.RootTable.Columns["RefDocNoId"].Visible = false;
			grd.RootTable.Columns["RefDocSubIdNo"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["PackTypeId"].Visible = false;
			grd.RootTable.Columns["JobLotId"].Visible = false;
			grd.RootTable.Columns["PackUOMId"].Visible = false;
			grd.RootTable.Columns["WareHouseFromId"].Visible = false;
			grd.RootTable.Columns["WareHouseToId"].Visible = false;
			grd.RootTable.Columns["RateUOMId"].Visible = false;
			grd.RootTable.Columns["JobLotIdTo"].Visible = false;
			if (!StockTransferFinancialEffectIsActive)
			{
				grd.RootTable.Columns["ItemRate"].Visible = false;
				grd.RootTable.Columns["ItemAmount"].Visible = false;
				grd.RootTable.Columns["Expense"].Visible = false;
				grd.RootTable.Columns["RateUOM"].Visible = false;
			}
			grd.RootTable.Columns["BalQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BalWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["QTY"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["EbUnit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["EbTotal"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Ad/LsWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Expense"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BalQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BalWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["QTY"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["EbUnit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["EbTotal"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Ad/LsWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Expense"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemRate"].FormatString = "#,#.###";
			grd.RootTable.Columns["BalQty"].FormatString = "#,#";
			grd.RootTable.Columns["BalWeight"].FormatString = "#,#.##";
			grd.RootTable.Columns["QTY"].FormatString = "#,#";
			grd.RootTable.Columns["GrossWeight"].FormatString = "#,#.##";
			grd.RootTable.Columns["EbUnit"].FormatString = "#,#.##";
			grd.RootTable.Columns["EbTotal"].FormatString = "#,#.##";
			grd.RootTable.Columns["Ad/LsWeight"].FormatString = "#,#.##";
			grd.RootTable.Columns["NetWeight"].FormatString = "#,#.##";
			grd.RootTable.Columns["ItemAmount"].FormatString = "#,#.###";
			grd.RootTable.Columns["ItemRate"].TotalFormatString = "#,#.###";
			grd.RootTable.Columns["BalQty"].TotalFormatString = "#,#";
			grd.RootTable.Columns["BalWeight"].TotalFormatString = "#,#.##";
			grd.RootTable.Columns["QTY"].TotalFormatString = "#,#";
			grd.RootTable.Columns["GrossWeight"].TotalFormatString = "#,#.##";
			grd.RootTable.Columns["EbUnit"].TotalFormatString = "#,#.##";
			grd.RootTable.Columns["EbTotal"].TotalFormatString = "#,#.##";
			grd.RootTable.Columns["Ad/LsWeight"].TotalFormatString = "#,#.##";
			grd.RootTable.Columns["NetWeight"].TotalFormatString = "#,#.##";
			grd.RootTable.Columns["ItemAmount"].TotalFormatString = "#,#.###";
			grd.RootTable.Columns["Expense"].TotalFormatString = "#,#.###";
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

	private void PurchsaeOrder_KeyDown(object sender, KeyEventArgs e)
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
			if (btnsave.Visible && btnsave.Enabled && e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N && tabControl1.SelectedIndex == 0)
			{
				btnnew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.P && tabControl1.SelectedIndex == 0)
			{
				btnprint_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
				}
				else
				{
					tabControl1.SelectedIndex = 1;
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (btnupdate.Visible && btnupdate.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				btnupdate_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				FromDateHistory.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Return && tabControl1.SelectedIndex == 1)
			{
				RecId = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(RecId);
			}
			if (e.Control && e.KeyCode == Keys.F5 && tabControl1.SelectedIndex == 0)
			{
				DocDate.Focus();
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
	}

	private void grdhistory_DoubleClick_1(object sender, EventArgs e)
	{
		try
		{
			RecId = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString());
			ReadById(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryComboFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		try
		{
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeIds = "68"
			};
			DataTable dtTransferType = new DataTable
			{
				Columns = 
				{
					{
						"Id",
						typeof(int)
					},
					{
						"TransferType",
						typeof(string)
					}
				}
			};
			DataTable dt = InvStockTransferHeader.AllComboAgainstStockTransfer(val);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "TransferType")
					{
						dtTransferType.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtTransferType, cmbTransactiontypeHistory, "Id", "TransferType", "TransferType", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridhistoryfill(int NoOfRecords = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = 806;
			obj.NoOfRecords = NoOfRecords;
			obj.CanViewAllRecord = formrights.DoHaveCanViewAllRecordRights;
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
			obj.FromDocNo = Conversion.ToInt((object)txtFromDocNoHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text);
			obj.TransferType = Conversion.ToString((object)((Control)(object)cmbTransactiontypeHistory).Text);
			DataTable dthistoryfill = InvStockTransferHeader.FormHistory(obj);
			if (dthistoryfill.Rows.Count > 0)
			{
				DataTable dtGridHistory = new DataTable();
				dtGridHistory.Columns.Add("Id", typeof(int));
				dtGridHistory.Columns.Add("DocDate", typeof(DateTime));
				dtGridHistory.Columns.Add("DocNo", typeof(int));
				dtGridHistory.Columns.Add("TransType");
				dtGridHistory.Columns.Add("GPNo", typeof(string));
				dtGridHistory.Columns.Add("TicketNo", typeof(string));
				dtGridHistory.Columns.Add("WBWeight", typeof(double));
				dtGridHistory.Columns.Add("Remarks");
				dtGridHistory.Columns.Add("FromBranchName", typeof(string));
				dtGridHistory.Columns.Add("ToBranchName", typeof(string));
				dtGridHistory.Columns.Add("EntryDate", typeof(DateTime));
				dtGridHistory.Columns.Add("EntryUser");
				dtGridHistory.Columns.Add("ModifyDate", typeof(DateTime));
				dtGridHistory.Columns.Add("ModifyUser");
				dtGridHistory.Columns.Add("NoOfAttachments", typeof(int));
				dtGridHistory.Clear();
				for (int i = 0; i < dthistoryfill.Rows.Count; i++)
				{
					dtGridHistory.Rows.Add(dthistoryfill.Rows[i]["Id"], Conversion.ToDateTime(dthistoryfill.Rows[i]["DocDate"]).ToShortDateString(), dthistoryfill.Rows[i]["DocNo"], dthistoryfill.Rows[i]["TransferType"], dthistoryfill.Rows[i]["GatePassId"], dthistoryfill.Rows[i]["WBTicketId"], dthistoryfill.Rows[i]["WbNetWeight"], dthistoryfill.Rows[i]["RemarksHeader"], dthistoryfill.Rows[i]["FromBranchName"], dthistoryfill.Rows[i]["ToBranchName"], dthistoryfill.Rows[i]["EntryDate"], dthistoryfill.Rows[i]["EntryUserName"], dthistoryfill.Rows[i]["ModifyDate"], dthistoryfill.Rows[i]["ModifyUserName"], dthistoryfill.Rows[i]["NoOfAttachments"]);
				}
				grdhistory.DataSource = dtGridHistory;
				grdhistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				grdhistory.ClearStructure();
				GridDetailHistory.ClearStructure();
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
			grdhistory.RootTable.Columns["Id"].Visible = false;
			grdhistory.RootTable.Columns["FromBranchName"].Visible = MultiBranchFeature;
			grdhistory.RootTable.Columns["ToBranchName"].Visible = MultiBranchFeature;
			GridEX_Helper.GridWrappingAndColumnSettings(grdhistory, 2, 3, null, null, null, ColumnSetting: false);
			grdhistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grdhistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdhistory.RootTable.Columns["TransType"].Width = Constants.InventoryConstants.DiscountType;
			grdhistory.RootTable.Columns["GPNo"].Width = Constants.InventoryConstants.GpNoConstant + 15;
			grdhistory.RootTable.Columns["TicketNo"].Width = Constants.InventoryConstants.DocNoConstant + 15;
			grdhistory.RootTable.Columns["WBWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdhistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdhistory.RootTable.Columns["FromBranchName"].Width = Constants.InventoryConstants.BranchName + 20;
			grdhistory.RootTable.Columns["ToBranchName"].Width = Constants.InventoryConstants.BranchName + 20;
			grdhistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdhistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			grdhistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdhistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			grdhistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			grdhistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["WBWeight"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["WBWeight"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["WBWeight"].FormatString = "#,##0,.##";
			grdhistory.RootTable.Columns["WBWeight"].TotalFormatString = "#,##0.##";
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdhistory.RootTable.Columns.Add("Edit");
			grdhistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdhistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdhistory.RootTable.Columns["Edit"].Width = 40;
			grdhistory.RootTable.Columns["Edit"].Position = 0;
			grdhistory.RootTable.Columns.Add("Print");
			grdhistory.RootTable.Columns["Print"].Caption = "Print";
			grdhistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Print"].ButtonText = "Print";
			grdhistory.RootTable.Columns["Print"].Width = 40;
			grdhistory.RootTable.Columns["Print"].Position = 1;
			grdhistory.FrozenColumns = 2;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdhistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdhistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 806);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				Reset();
				RecId = Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value);
				ReadById(RecId);
			}
			if (e.Column.Key == "Print")
			{
				CommonServices.StockTransferSlip407(Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value));
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
			gridhistoryfill();
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

	private void ctrlGrdBar4_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdOtherCharges).Name);
			if (fs != null)
			{
				grdOtherCharges.LoadLayoutFile((Stream)fs);
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

	private void btnprint_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.StockTransferSlip407(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AvailableStockGetByItem()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		try
		{
			DataTable dt = CommonServices.GetWeightCurrStockByItem(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				WarehouseId = Conversion.ToInt(CmbWareHouseFrom.Value),
				ItemId = Conversion.ToInt(cmbItemName.Value),
				JobLotId = Conversion.ToInt(cmbJobLot.Value),
				CropYear = Conversion.ToString((object)((Control)(object)cmbCropYear).Text.Trim()),
				ToDate = Conversion.ToDateTime((object)DocDate.Value)
			});
			if (dt.Rows.Count > 0)
			{
				txtAvailableStock.Text = dt.Rows[0]["AvailableStock"].ToString();
			}
			else
			{
				txtAvailableStock.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAdLsWeight_TextChanged(object sender, EventArgs e)
	{
		WeightCalculation();
	}

	private void WeightCalculation()
	{
		try
		{
			double GrossWeight = 0.0;
			double Qty = 0.0;
			double EbUnit = 0.0;
			double EbTotal = 0.0;
			double addless = 0.0;
			if (txtQty.Text.Trim() != string.Empty && txtQty.Text != "0")
			{
				Qty = Conversion.ToDouble((object)txtQty.Text.Trim());
			}
			if (txtGrossWeight.Text.Trim() != string.Empty && txtGrossWeight.Text != "0")
			{
				GrossWeight = Conversion.ToDouble((object)txtGrossWeight.Text.Trim());
			}
			addless = ((!(txtAdLsWeight.Text.Trim() != string.Empty)) ? 0.0 : Conversion.ToDouble((object)txtAdLsWeight.Text.Trim()));
			if (txtEbUnit.Text.Trim() != string.Empty)
			{
				EbUnit = Conversion.ToDouble((object)txtEbUnit.Text.Trim());
			}
			else
			{
				EbUnit = 0.0;
				txtEbTotal.Text = "0";
			}
			if (EbUnit > 0.0)
			{
				EbTotal = Qty * EbUnit;
				txtEbTotal.Text = Conversion.ToString((object)EbTotal);
			}
			if (txtQty.Text.Trim() != string.Empty && Conversion.ToInt(CmbPackUom.Value) != 0 && txtGrossWeight.Text.Trim() != string.Empty)
			{
				txtDetailNetWeight.Text = (GrossWeight - EbTotal + addless).ToString();
			}
			AvgAmountCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrossWeightCalculation()
	{
		try
		{
			double GrossWeight = 0.0;
			if (txtQty.Text.Trim() != string.Empty && Conversion.ToInt(CmbPackUom.Value) != 0)
			{
				double num = Conversion.ToDouble((object)txtQty.Text.Trim());
				double UOM = Conversion.ToDouble((object)((Control)(object)CmbPackUom).Text.Trim());
				txtGrossWeight.Text = (num * UOM).ToString();
			}
			AvgAmountCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FactoryWeightCalculation()
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				double FactoryWeight = Conversion.ToDouble(grd.GetTotal(grd.RootTable.Columns["GrossWeight"], (AggregateFunction)2));
				if (FactoryWeight > 0.0)
				{
					txtHeadNetWeight.Text = FactoryWeight.ToString("#,##0.###");
				}
				else
				{
					txtHeadNetWeight.Text = Conversion.ToString((object)0);
				}
			}
			else
			{
				txtHeadNetWeight.Text = Conversion.ToString((object)0);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQty_TextChanged(object sender, EventArgs e)
	{
		GrossWeightCalculation();
		WeightCalculation();
	}

	private void CmbPackUom_TextChanged(object sender, EventArgs e)
	{
		GrossWeightCalculation();
		WeightCalculation();
	}

	private void txtGrossWeight_TextChanged(object sender, EventArgs e)
	{
		WeightCalculation();
	}

	private void cmbItemName_Leave(object sender, EventArgs e)
	{
		try
		{
			bindRateUomAndItemPackUom(Conversion.ToInt(cmbItemName.Value));
			AvailableStockGetByItem();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtdocno_Leave(object sender, EventArgs e)
	{
	}

	private void AvgAmountCalculation()
	{
		try
		{
			if (Conversion.ToDouble((object)txtDetailNetWeight.Text) > 0.0 && Conversion.ToDouble((object)txtAvgRate.Text) > 0.0)
			{
				double rate = Conversion.ToDouble((object)txtAvgRate.Text);
				double Amount = Conversion.ToDouble((object)txtDetailNetWeight.Text) / 40.0 * rate;
				txtAmount.Text = Amount.ToString("#,#");
			}
			else
			{
				txtAmount.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemAmountCalculation()
	{
		try
		{
			if (grd.GetRows().Length != 0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow item in rows)
				{
					item.BeginEdit();
					double NetWeight = Conversion.ToDouble(item.Cells["NetWeight"].Value);
					double RateUOM = Conversion.ToDouble(item.Cells["RateUOM"].Value);
					double ItemRate = Conversion.ToDouble(item.Cells["ItemRate"].Value);
					double ItemAmount = NetWeight / RateUOM * ItemRate;
					if (NetWeight > 0.0 && RateUOM > 0.0 && ItemRate > 0.0)
					{
						item.Cells["ItemAmount"].Value = ItemAmount.ToString();
					}
					else
					{
						item.Cells["ItemAmount"].Value = 0;
					}
					item.BeginEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		grd.UpdateData();
	}

	private void cmbCropYear_TextChanged(object sender, EventArgs e)
	{
		AvailableStockGetByItem();
		GetAvgRate();
	}

	private void cmbJobLot_TextChanged(object sender, EventArgs e)
	{
		AvailableStockGetByItem();
		GetAvgRate();
	}

	private void txtEbUnit_TextChanged(object sender, EventArgs e)
	{
		WeightCalculation();
	}

	private void GetAvgRate()
	{
		try
		{
			double rate = CommonServices.AvgRateOnlyForCGS(Conversion.ToInt(cmbItemName.Value), Conversion.ToDateTime((object)DocDate.Value), 806, RecId, Conversion.ToInt(cmbJobLot.Value), Conversion.ToInt(cmbCropYear.Value), null, Conversion.ToInt(CmbWareHouseFrom.Value)) * 40.0;
			if (rate > 0.0)
			{
				txtAvgRate.Text = Conversion.ToString((object)rate);
				AvgAmountCalculation();
			}
			else
			{
				txtAvgRate.Text = 0.ToString();
				txtAmount.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbItemName_TextChanged(object sender, EventArgs e)
	{
		try
		{
			GetAvgRate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbWareHouseFrom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			AvailableStockGetByItem();
			GetAvgRate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAvgRate_TextChanged(object sender, EventArgs e)
	{
		AvgAmountCalculation();
	}

	private void btnLoadInvoices_Click(object sender, EventArgs e)
	{
		try
		{
			LoadTransactions = new LoadavailableTransactionsForIssuance(UserAccount);
			LoadTransactions.ShowDialog();
			LoadTransactions.BringToFront();
			LoadDataDetailfromPurchaseInvoivce();
			FactoryWeightCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadDataDetailfromPurchaseInvoivce()
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dtpurchaseinvoice = LoadTransactions.dtIssuance;
			if (dtpurchaseinvoice.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtpurchaseinvoice.Rows.Count; i++)
			{
				bool flag = false;
				if (grd.RowCount > 0)
				{
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow item in rows)
					{
						if (item != null && (int)item.RowType == 0)
						{
							if (Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) == 0 || Conversion.ToInt(item.Cells["RefDocNoId"].Value) == 0)
							{
								item.Delete();
							}
							if (grd.GetRows().Length != 0 && Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocumentTypeId"]) == Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) && Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocIdNo"]) == Conversion.ToInt(item.Cells["RefDocNoId"].Value) && Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocSubIdNo"]) == Conversion.ToInt(item.Cells["RefDocSubIdNo"].Value))
							{
								item.BeginEdit();
								double BalQTY = Conversion.ToDouble(item.Cells["QTY"].Value);
								double BalWeight = Conversion.ToDouble(item.Cells["BalWeight"].Value);
								item.Cells["QTY"].Value = Conversion.ToDouble((object)BalQTY) + Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["ItemQty"]);
								item.Cells["BalWeight"].Value = Conversion.ToDouble((object)BalWeight) + Conversion.ToDouble(dtpurchaseinvoice.Rows[i]["Weight"]);
								item.EndEdit();
								flag = true;
							}
						}
					}
				}
				if (!flag && !flag)
				{
					table.Rows.Add(0, dtpurchaseinvoice.Rows[i]["RefDocumentTypeId"], dtpurchaseinvoice.Rows[i]["RefDocIdNo"], dtpurchaseinvoice.Rows[i]["RefDocSubIdNo"], dtpurchaseinvoice.Rows[i]["ItemId"], dtpurchaseinvoice.Rows[i]["ItemName"], dtpurchaseinvoice.Rows[i]["PackingTypeId"], dtpurchaseinvoice.Rows[i]["PackTypeCode"], dtpurchaseinvoice.Rows[i]["CropYear"], dtpurchaseinvoice.Rows[i]["JobLotId"], dtpurchaseinvoice.Rows[i]["JobLotCode"], dtpurchaseinvoice.Rows[i]["JobLotId"], dtpurchaseinvoice.Rows[i]["JobLotCode"], dtpurchaseinvoice.Rows[i]["ItemQty"], dtpurchaseinvoice.Rows[i]["ItemUOMId"], dtpurchaseinvoice.Rows[i]["ItemUOM"], dtpurchaseinvoice.Rows[i]["Weight"], 0, 0, 0, dtpurchaseinvoice.Rows[i]["Weight"], dtpurchaseinvoice.Rows[i]["ItemQty"], dtpurchaseinvoice.Rows[i]["Weight"], dtpurchaseinvoice.Rows[i]["WareHouseId"], dtpurchaseinvoice.Rows[i]["WareHouse"], 0, "", "", dtpurchaseinvoice.Rows[i]["ItemRate"], dtpurchaseinvoice.Rows[i]["RateUOMId"], dtpurchaseinvoice.Rows[i]["Equivalent"], dtpurchaseinvoice.Rows[i]["ItemAmount"]);
				}
			}
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			OtherChargesAmountUpdate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOtherCharges_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdOtherCharges.UpdateData();
			GridEXRow item = grdOtherCharges.CurrentRow;
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item != null && (int)item.RowType == 0 && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
			{
				double Qty = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
				double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
				item.Cells["Amount"].Value = Qty * Rate;
				item.Cells["Percentage"].Value = 0;
			}
			if (e.Column.Key == "Percentage" && item != null && (int)item.RowType == 0 && item.Cells["Percentage"].Value.ToString() != string.Empty)
			{
				double num = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2).ToString());
				double Percentage = Conversion.ToDouble((object)item.Cells["Percentage"].Value.ToString());
				double TotlaPercentage = num / 100.0 * Percentage;
				if (TotlaPercentage > 0.0)
				{
					item.Cells["Amount"].Value = Conversion.ToDouble((object)TotlaPercentage);
				}
				item.Cells["Qty"].Value = 0;
				item.Cells["Rate"].Value = 0;
			}
			grdOtherCharges.UpdateData();
			OtherExpensesProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOtherCharges_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdOtherCharges.CurrentRow;
				dtExp.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtExp.Rows.Count == 0)
				{
					AddRowInExpenseGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInExpenseGrid();
			}
			grdOtherCharges.UpdateData();
			OtherExpensesProportion();
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

	private void txtHeadNetWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void cmbTicketNo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void cmbGatePass_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtGrossWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtEbUnit_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtAdLsWeight_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			if (!char.IsDigit(e.KeyChar) && e.KeyChar != '-' && e.KeyChar != '.')
			{
				e.Handled = true;
			}
			if (e.KeyChar == '\b')
			{
				e.Handled = false;
			}
			if (e.KeyChar == '.' && txtAdLsWeight.Text.Contains("."))
			{
				e.Handled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAvailableStock_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtAvgRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void grdhistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		try
		{
			GridEXRow row = grdhistory.CurrentRow;
			if (row == null || (int)row.RowType != 0 || row.Cells.Count <= 0)
			{
				return;
			}
			int num = Conversion.ToInt(row.Cells["Id"].Value);
			InvStockTransferHeader obj = new InvStockTransferHeader();
			obj = InvStockTransferHeader.GetByID(num);
			if (obj.InvStockTransferDetail != null && obj.InvStockTransferDetail.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("FromWarehouse");
				dt.Columns.Add("ToWarehouse");
				dt.Columns.Add("JobLotFrom");
				dt.Columns.Add("JobLotTo");
				dt.Columns.Add("CropYear");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("PackingType");
				dt.Columns.Add("Qty", typeof(double));
				dt.Columns.Add("PackUom");
				dt.Columns.Add("GrossWeight", typeof(double));
				dt.Columns.Add("EbUnit", typeof(double));
				dt.Columns.Add("EbTotal", typeof(double));
				dt.Columns.Add("AddLessWt", typeof(double));
				dt.Columns.Add("StockWeight", typeof(double));
				dt.Columns.Add("RemarksDetail");
				for (int i = 0; i < obj.InvStockTransferDetail.Count; i++)
				{
					dt.Rows.Add(obj.InvStockTransferDetail[i].FromWareHouseName, obj.InvStockTransferDetail[i].ToWareHouseName, obj.InvStockTransferDetail[i].JobLotDescription, obj.InvStockTransferDetail[i].JobLotTo, obj.InvStockTransferDetail[i].CropYear, obj.InvStockTransferDetail[i].ItemName, obj.InvStockTransferDetail[i].PackTypeDesc, obj.InvStockTransferDetail[i].Qty, obj.InvStockTransferDetail[i].PackUom, obj.InvStockTransferDetail[i].GrossWeight, obj.InvStockTransferDetail[i].EbUnit, obj.InvStockTransferDetail[i].EbTotal, obj.InvStockTransferDetail[i].AdLsWeight, obj.InvStockTransferDetail[i].NetWeight, obj.InvStockTransferDetail[i].RemarksDetail);
				}
				GridDetailHistory.DataSource = dt;
				GridDetailHistory.RetrieveStructure();
				GridEX_Helper.GridWrappingAndColumnSettings(GridDetailHistory, 2, 3, null, null, null, ColumnSetting: false);
				GridDetailHistory.RootTable.Columns["FromWarehouse"].Width = Constants.InventoryConstants.WareHouseName;
				GridDetailHistory.RootTable.Columns["ToWarehouse"].Width = Constants.InventoryConstants.WareHouseName;
				GridDetailHistory.RootTable.Columns["JobLotFrom"].Width = Constants.InventoryConstants.JobLotConstant;
				GridDetailHistory.RootTable.Columns["JobLotTo"].Width = Constants.InventoryConstants.JobLotConstant;
				GridDetailHistory.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
				GridDetailHistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
				GridDetailHistory.RootTable.Columns["PackingType"].Width = Constants.InventoryConstants.PackingType;
				GridDetailHistory.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
				GridDetailHistory.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
				GridDetailHistory.RootTable.Columns["GrossWeight"].Width = Constants.InventoryConstants.WeightConstant;
				GridDetailHistory.RootTable.Columns["EbUnit"].Width = Constants.InventoryConstants.EbWt;
				GridDetailHistory.RootTable.Columns["EbTotal"].Width = Constants.InventoryConstants.EbWt;
				GridDetailHistory.RootTable.Columns["AddLessWt"].Width = Constants.InventoryConstants.WeightConstant;
				GridDetailHistory.RootTable.Columns["StockWeight"].Width = Constants.InventoryConstants.WeightConstant;
				GridDetailHistory.RootTable.Columns["RemarksDetail"].Width = Constants.InventoryConstants.RemarksConstants + 100;
				GridDetailHistory.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
				GridDetailHistory.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
				GridDetailHistory.RootTable.Columns["Qty"].FormatString = "#,##0.##";
				GridDetailHistory.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
				GridDetailHistory.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
				GridDetailHistory.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
				GridDetailHistory.RootTable.Columns["GrossWeight"].FormatString = "#,##0.##";
				GridDetailHistory.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.##";
				GridDetailHistory.RootTable.Columns["EbUnit"].AggregateFunction = (AggregateFunction)2;
				GridDetailHistory.RootTable.Columns["EbUnit"].TextAlignment = (TextAlignment)3;
				GridDetailHistory.RootTable.Columns["EbUnit"].FormatString = "#,##0.##";
				GridDetailHistory.RootTable.Columns["EbUnit"].TotalFormatString = "#,##0.##";
				GridDetailHistory.RootTable.Columns["EbTotal"].AggregateFunction = (AggregateFunction)2;
				GridDetailHistory.RootTable.Columns["EbTotal"].TextAlignment = (TextAlignment)3;
				GridDetailHistory.RootTable.Columns["EbTotal"].FormatString = "#,##0.##";
				GridDetailHistory.RootTable.Columns["EbTotal"].TotalFormatString = "#,##0.##";
				GridDetailHistory.RootTable.Columns["AddLessWt"].AggregateFunction = (AggregateFunction)2;
				GridDetailHistory.RootTable.Columns["AddLessWt"].TextAlignment = (TextAlignment)3;
				GridDetailHistory.RootTable.Columns["AddLessWt"].FormatString = "#,##0.##";
				GridDetailHistory.RootTable.Columns["AddLessWt"].TotalFormatString = "#,##0.##";
				GridDetailHistory.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
				GridDetailHistory.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
				GridDetailHistory.RootTable.Columns["StockWeight"].FormatString = "#,##0.##";
				GridDetailHistory.RootTable.Columns["StockWeight"].TotalFormatString = "#,##0.##";
				ctrlGrdBar6_Load(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar6_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GridDetailHistory).Name);
			if (fs != null)
			{
				GridDetailHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmStockTransferManual_FormClosing(object sender, FormClosingEventArgs e)
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

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && e.Column.Key == "Delete" && MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) == 0)
				{
					r.Delete();
				}
				else
				{
					DetailRowsRemoveIds = DetailRowsRemoveIds + "," + Conversion.ToString(r.Cells["Id"].Value);
					r.Delete();
				}
				OtherExpensesProportion();
				FactoryWeightCalculation();
				grd.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnSameUpdateWarehouse_Click(object sender, EventArgs e)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!chkUpdateWareHouse.Checked && !chkUpdateJobLot.Checked)
			{
				throw new Exception("Please Checked atleast one value (warehouse,JobLot) value to Update");
			}
			if (grd.GetDataRows().Count() > 0)
			{
				GridEXRow rowsToValidate = grd.GetDataRows().FirstOrDefault();
				if (rowsToValidate == null || (int)rowsToValidate.RowType != 0 || rowsToValidate.Cells.Count <= 0)
				{
					return;
				}
				if (Conversion.ToInt(rowsToValidate.Cells["WareHouseToId"].Value) != 0)
				{
					int wareId = 0;
					string wareName = "";
					wareId = Conversion.ToInt(rowsToValidate.Cells["WareHouseToId"].Value);
					wareName = Conversion.ToString(rowsToValidate.Cells["WareHouseTo"].Value);
					int JobId = 0;
					string jobName = "";
					JobId = Conversion.ToInt(rowsToValidate.Cells["JobLotIdTo"].Value);
					jobName = Conversion.ToString(rowsToValidate.Cells["JobLotTo"].Value);
					GridEXRow[] dataRows = grd.GetDataRows();
					foreach (GridEXRow r in dataRows)
					{
						r.BeginEdit();
						if (chkUpdateWareHouse.Checked)
						{
							r.Cells["WareHouseToId"].Value = wareId;
							r.Cells["WareHouseTo"].Value = wareName;
						}
						if (chkUpdateJobLot.Checked)
						{
							r.Cells["JobLotIdTo"].Value = JobId;
							r.Cells["JobLotTo"].Value = jobName;
						}
						r.EndEdit();
					}
					return;
				}
				throw new Exception("WareHouseTo Field is Empty in Grid Row No: 01");
			}
			throw new Exception("Grid Record not Found");
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
			gridhistoryfill();
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
			txtFromDocNoHistory.Text = string.Empty;
			txtToDocNoHistory.Text = string.Empty;
			((Control)(object)cmbTransactiontypeHistory).Text = string.Empty;
			grdhistory.ClearStructure();
			GridDetailHistory.ClearStructure();
			drdocdate.Checked = true;
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
			HistoryComboFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		try
		{
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					InvPurchaseInvoice.RemoveByID(new InvPurchaseInvoice
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocumentTypeId = 806,
						Id = RecId,
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
			MessageBox.Show(ex.Message);
		}
	}

	private void DocDate_ValueChanged(object sender, EventArgs e)
	{
	}

	private void AvgRateRecalculateOnDocDateChange()
	{
		try
		{
			if (grd.RowCount <= 0 || FIFOCGSFlag)
			{
				return;
			}
			GridEXRow[] dataRows = grd.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				r.BeginEdit();
				int RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
				int RefDocNoId = Conversion.ToInt(r.Cells["RefDocNoId"].Value);
				int RefDocSubIdNo = Conversion.ToInt(r.Cells["RefDocSubIdNo"].Value);
				int ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				int JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
				int WareHouseId = Conversion.ToInt(r.Cells["WareHouseFromId"].Value);
				string CropYear = Conversion.ToString(r.Cells["CropYear"].Value);
				if (RefDocumentTypeId != 0 || RefDocNoId != 0 || RefDocSubIdNo != 0)
				{
					break;
				}
				double rate = CommonServices.AvgRateOnlyForCGS(ItemId, Conversion.ToDateTime((object)DocDate.Value), 806, RecId, JobLotId, 0, CropYear, WareHouseId) * 40.0;
				if (rate > 0.0)
				{
					r.Cells["ItemRate"].Value = rate;
					double Amount = Conversion.ToDouble(r.Cells["NetWeight"].Value) / 40.0 * rate;
					r.Cells["ItemAmount"].Value = Amount;
				}
				r.EndEdit();
			}
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnGenerateRate_Click(object sender, EventArgs e)
	{
		try
		{
			AvgRateRecalculateOnDocDateChange();
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
		//IL_0472: Unknown result type (might be due to invalid IL or missing references)
		//IL_047c: Expected O, but got Unknown
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Expected O, but got Unknown
		//IL_0585: Unknown result type (might be due to invalid IL or missing references)
		//IL_058f: Expected O, but got Unknown
		//IL_05c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d1: Expected O, but got Unknown
		//IL_05f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fd: Expected O, but got Unknown
		//IL_0609: Unknown result type (might be due to invalid IL or missing references)
		//IL_0613: Expected O, but got Unknown
		//IL_061f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0629: Expected O, but got Unknown
		//IL_064b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0655: Expected O, but got Unknown
		//IL_0656: Unknown result type (might be due to invalid IL or missing references)
		//IL_0660: Expected O, but got Unknown
		//IL_066c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0676: Expected O, but got Unknown
		//IL_0769: Unknown result type (might be due to invalid IL or missing references)
		//IL_0773: Expected O, but got Unknown
		//IL_07c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cb: Expected O, but got Unknown
		//IL_07cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d6: Expected O, but got Unknown
		//IL_0887: Unknown result type (might be due to invalid IL or missing references)
		//IL_0891: Expected O, but got Unknown
		//IL_08be: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c8: Expected O, but got Unknown
		//IL_096e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0978: Expected O, but got Unknown
		//IL_11b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c0: Expected O, but got Unknown
		//IL_19bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_19c9: Expected O, but got Unknown
		//IL_19d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_19e0: Expected O, but got Unknown
		//IL_781f: Unknown result type (might be due to invalid IL or missing references)
		//IL_7829: Expected O, but got Unknown
		//IL_784d: Unknown result type (might be due to invalid IL or missing references)
		//IL_7857: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOtherCharges_DesignTimeLayout = new GridEXLayout();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
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
		Appearance appearance35 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance53 = new Appearance();
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
		Appearance appearance39 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
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
		Appearance appearance122 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		GridEXLayout GridDetailHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.StoreManagement.frmStockTransferManual));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.PanelMessage = new System.Windows.Forms.Panel();
		this.ToolStripMessage = new System.Windows.Forms.ToolStrip();
		this.BtnMessage = new System.Windows.Forms.ToolStripButton();
		this.chkUpdateJobLot = new System.Windows.Forms.CheckBox();
		this.chkUpdateWareHouse = new System.Windows.Forms.CheckBox();
		this.BtnSameUpdateWarehouse = new System.Windows.Forms.Button();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		this.label23 = new System.Windows.Forms.Label();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.panel14 = new System.Windows.Forms.Panel();
		this.grdOtherCharges = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.label22 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.btnplus = new System.Windows.Forms.Button();
		this.label3 = new System.Windows.Forms.Label();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.lblAmount = new System.Windows.Forms.Label();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.label20 = new System.Windows.Forms.Label();
		this.CmbWareHouseFrom = new UltraCombo();
		this.txtAvgRate = new System.Windows.Forms.TextBox();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.txtAvailableStock = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.lblAvgRate = new System.Windows.Forms.Label();
		this.CmbPackUom = new UltraCombo();
		this.txtEbTotal = new System.Windows.Forms.TextBox();
		this.label21 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.cmbJobLot = new UltraCombo();
		this.txtEbUnit = new System.Windows.Forms.TextBox();
		this.CmbJobLotTo = new UltraCombo();
		this.label16 = new System.Windows.Forms.Label();
		this.cmbPackingType = new UltraCombo();
		this.txtRemarksDetail = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.cmbCropYear = new UltraCombo();
		this.cmbItemName = new UltraCombo();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.cmbWareHouseTo = new UltraCombo();
		this.label37 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.label36 = new System.Windows.Forms.Label();
		this.txtAdLsWeight = new System.Windows.Forms.TextBox();
		this.txtDetailNetWeight = new System.Windows.Forms.TextBox();
		this.label45 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label30 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.txtWorkingReportNo = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.cmbGatePass = new System.Windows.Forms.TextBox();
		this.cmbTicketNo = new System.Windows.Forms.TextBox();
		this.txtHeadNetWeight = new System.Windows.Forms.TextBox();
		this.CmbTransferType = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtRemarksHead = new System.Windows.Forms.TextBox();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label24 = new System.Windows.Forms.Label();
		this.ChkBoxPrintPreview = new System.Windows.Forms.CheckBox();
		this.combproject1 = new UltraCombo();
		this.cmbbranch1 = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnLoadInvoices = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel21 = new System.Windows.Forms.Panel();
		this.GridDetailHistory = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.label29 = new System.Windows.Forms.Label();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel17 = new System.Windows.Forms.Panel();
		this.grdhistory = new GridEX();
		this.panel20 = new System.Windows.Forms.Panel();
		this.label25 = new System.Windows.Forms.Label();
		this.ctrlGrdBar7 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel16 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnshow = new System.Windows.Forms.Button();
		this.label27 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.label63 = new System.Windows.Forms.Label();
		this.cmbTransactiontypeHistory = new UltraCombo();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label26 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnGenerateRate = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel3.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel11.SuspendLayout();
		this.PanelMessage.SuspendLayout();
		this.ToolStripMessage.SuspendLayout();
		this.tabPage4.SuspendLayout();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOtherCharges).BeginInit();
		this.panel13.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouseFrom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLotTo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackingType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouseTo).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTransferType).BeginInit();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.combproject1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbranch1).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel21.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetailHistory).BeginInit();
		this.panel18.SuspendLayout();
		this.panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel20.SuspendLayout();
		this.panel16.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbTransactiontypeHistory).BeginInit();
		this.panel10.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(964, 661);
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
		this.tabControl1.Size = new System.Drawing.Size(964, 661);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(956, 632);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add(this.panel3);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(2, 315);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(952, 315);
		this.panel6.TabIndex = 4;
		this.panel3.Controls.Add(this.tabControl2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(952, 315);
		this.panel3.TabIndex = 1;
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(952, 315);
		this.tabControl2.TabIndex = 0;
		this.tabPage3.Controls.Add(this.panel5);
		this.tabPage3.Controls.Add(this.panel11);
		this.tabPage3.Font = new System.Drawing.Font("Segoe UI", 10.75f);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(944, 286);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Detail";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 29);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(938, 254);
		this.panel5.TabIndex = 54;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		this.grd.ColumnAutoResize = true;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(938, 254);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.PanelMessage);
		this.panel11.Controls.Add(this.chkUpdateJobLot);
		this.panel11.Controls.Add(this.chkUpdateWareHouse);
		this.panel11.Controls.Add(this.BtnSameUpdateWarehouse);
		this.panel11.Controls.Add(this.ctrlGrdBar5);
		this.panel11.Controls.Add(this.label23);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(3, 3);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(938, 26);
		this.panel11.TabIndex = 53;
		this.PanelMessage.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.PanelMessage.BackColor = System.Drawing.Color.White;
		this.PanelMessage.Controls.Add(this.ToolStripMessage);
		this.PanelMessage.Location = new System.Drawing.Point(509, 3);
		this.PanelMessage.Name = "PanelMessage";
		this.PanelMessage.Size = new System.Drawing.Size(25, 23);
		this.PanelMessage.TabIndex = 135;
		this.ToolStripMessage.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ToolStripMessage.Dock = System.Windows.Forms.DockStyle.None;
		this.ToolStripMessage.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.ToolStripMessage.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.BtnMessage });
		this.ToolStripMessage.Location = new System.Drawing.Point(0, 0);
		this.ToolStripMessage.Name = "ToolStripMessage";
		this.ToolStripMessage.Size = new System.Drawing.Size(26, 25);
		this.ToolStripMessage.TabIndex = 0;
		this.ToolStripMessage.Text = "toolStrip3";
		this.BtnMessage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.BtnMessage.Image = Architecture.WinApp.Properties.Resources.info_i;
		this.BtnMessage.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnMessage.Name = "BtnMessage";
		this.BtnMessage.Size = new System.Drawing.Size(23, 22);
		this.BtnMessage.ToolTipText = "Click to 'Update Same In All Rows' Button To Update Checked Column (Warehouse or JobLot) In All rows";
		this.chkUpdateJobLot.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkUpdateJobLot.AutoSize = true;
		this.chkUpdateJobLot.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkUpdateJobLot.ForeColor = System.Drawing.Color.White;
		this.chkUpdateJobLot.Location = new System.Drawing.Point(636, 5);
		this.chkUpdateJobLot.Name = "chkUpdateJobLot";
		this.chkUpdateJobLot.Size = new System.Drawing.Size(63, 19);
		this.chkUpdateJobLot.TabIndex = 133;
		this.chkUpdateJobLot.TabStop = false;
		this.chkUpdateJobLot.Text = "JobLot";
		this.chkUpdateJobLot.UseVisualStyleBackColor = true;
		this.chkUpdateWareHouse.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkUpdateWareHouse.AutoSize = true;
		this.chkUpdateWareHouse.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkUpdateWareHouse.ForeColor = System.Drawing.Color.White;
		this.chkUpdateWareHouse.Location = new System.Drawing.Point(543, 5);
		this.chkUpdateWareHouse.Name = "chkUpdateWareHouse";
		this.chkUpdateWareHouse.Size = new System.Drawing.Size(91, 19);
		this.chkUpdateWareHouse.TabIndex = 134;
		this.chkUpdateWareHouse.TabStop = false;
		this.chkUpdateWareHouse.Text = "WareHouse";
		this.chkUpdateWareHouse.UseVisualStyleBackColor = true;
		this.BtnSameUpdateWarehouse.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnSameUpdateWarehouse.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnSameUpdateWarehouse.FlatAppearance.BorderSize = 0;
		this.BtnSameUpdateWarehouse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnSameUpdateWarehouse.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnSameUpdateWarehouse.ForeColor = System.Drawing.Color.White;
		this.BtnSameUpdateWarehouse.Location = new System.Drawing.Point(703, 3);
		this.BtnSameUpdateWarehouse.Name = "BtnSameUpdateWarehouse";
		this.BtnSameUpdateWarehouse.Size = new System.Drawing.Size(188, 21);
		this.BtnSameUpdateWarehouse.TabIndex = 38;
		this.BtnSameUpdateWarehouse.Text = "Update Same In All Rows";
		this.BtnSameUpdateWarehouse.UseVisualStyleBackColor = false;
		this.BtnSameUpdateWarehouse.Click += new System.EventHandler(BtnSameUpdateWarehouse_Click);
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = this;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(894, 1);
		this.ctrlGrdBar5.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
		this.ctrlGrdBar5.MyGrid = this.grd;
		this.ctrlGrdBar5.Name = "ctrlGrdBar5";
		this.ctrlGrdBar5.Size = new System.Drawing.Size(40, 24);
		this.ctrlGrdBar5.TabIndex = 25;
		this.ctrlGrdBar5.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 5);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(46, 17);
		this.label23.TabIndex = 35;
		this.label23.Text = "Detail";
		this.tabPage4.Controls.Add(this.panel14);
		this.tabPage4.Controls.Add(this.panel13);
		this.tabPage4.Font = new System.Drawing.Font("Segoe UI", 10.75f);
		this.tabPage4.Location = new System.Drawing.Point(4, 25);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(944, 286);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "Other Charges";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.grdOtherCharges);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel14.Location = new System.Drawing.Point(3, 29);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(938, 254);
		this.panel14.TabIndex = 54;
		((ScrollableBase)this.grdOtherCharges).BackColor = System.Drawing.Color.White;
		this.grdOtherCharges.ColumnAutoResize = true;
		grdOtherCharges_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOtherCharges.DesignTimeLayout = grdOtherCharges_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOtherCharges).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdOtherCharges).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOtherCharges.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOtherCharges).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdOtherCharges).Name = "grdOtherCharges";
		((ScrollableBase)this.grdOtherCharges).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOtherCharges).Size = new System.Drawing.Size(938, 254);
		((System.Windows.Forms.Control)(object)this.grdOtherCharges).TabIndex = 0;
		this.grdOtherCharges.TotalRow = (InheritableBoolean)1;
		this.grdOtherCharges.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOtherCharges).VisualStyle = (VisualStyle)4;
		this.grdOtherCharges.CellUpdated += new ColumnActionEventHandler(grdOtherCharges_CellUpdated);
		this.grdOtherCharges.ColumnButtonClick += new ColumnActionEventHandler(grdOtherCharges_ColumnButtonClick);
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.ctrlGrdBar4);
		this.panel13.Controls.Add(this.label22);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(3, 3);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(938, 26);
		this.panel13.TabIndex = 53;
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(890, 0);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
		this.ctrlGrdBar4.MyGrid = this.grdOtherCharges;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(45, 25);
		this.ctrlGrdBar4.TabIndex = 24;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label22.ForeColor = System.Drawing.Color.White;
		this.label22.Location = new System.Drawing.Point(5, 5);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(151, 17);
		this.label22.TabIndex = 35;
		this.label22.Text = "Other Charges Detail";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox2);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add(this.panel12);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combproject1);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbbranch1);
		this.panel4.Controls.Add(this.label13);
		this.panel4.Controls.Add(this.label18);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 30);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(952, 285);
		this.panel4.TabIndex = 0;
		this.groupBox2.Controls.Add(this.btnplus);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Controls.Add(this.txtAmount);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add(this.lblAmount);
		this.groupBox2.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox2.Controls.Add(this.label20);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom);
		this.groupBox2.Controls.Add(this.txtAvgRate);
		this.groupBox2.Controls.Add(this.btnUpdateDetail);
		this.groupBox2.Controls.Add(this.txtAvailableStock);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.lblAvgRate);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUom);
		this.groupBox2.Controls.Add(this.txtEbTotal);
		this.groupBox2.Controls.Add(this.label21);
		this.groupBox2.Controls.Add(this.label19);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbJobLot);
		this.groupBox2.Controls.Add(this.txtEbUnit);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLotTo);
		this.groupBox2.Controls.Add(this.label16);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackingType);
		this.groupBox2.Controls.Add(this.txtRemarksDetail);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCropYear);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemName);
		this.groupBox2.Controls.Add(this.txtGrossWeight);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbWareHouseTo);
		this.groupBox2.Controls.Add(this.label37);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add(this.txtQty);
		this.groupBox2.Controls.Add(this.label36);
		this.groupBox2.Controls.Add(this.txtAdLsWeight);
		this.groupBox2.Controls.Add(this.txtDetailNetWeight);
		this.groupBox2.Controls.Add(this.label45);
		this.groupBox2.Controls.Add(this.label38);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(4, 131);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(936, 150);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Detail";
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(802, 120);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(39, 26);
		this.btnplus.TabIndex = 18;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(3, 15);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(100, 15);
		this.label3.TabIndex = 0;
		this.label3.Text = "WareHouse From";
		this.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAmount.Location = new System.Drawing.Point(828, 79);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.ReadOnly = true;
		this.txtAmount.Size = new System.Drawing.Size(103, 23);
		this.txtAmount.TabIndex = 16;
		this.txtAmount.TabStop = false;
		this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(739, 15);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(67, 15);
		this.label2.TabIndex = 0;
		this.label2.Text = "Item Name";
		this.lblAmount.AutoSize = true;
		this.lblAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAmount.Location = new System.Drawing.Point(828, 61);
		this.lblAmount.Name = "lblAmount";
		this.lblAmount.Size = new System.Drawing.Size(51, 15);
		this.lblAmount.TabIndex = 51;
		this.lblAmount.Text = "Amount";
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(869, 120);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(62, 26);
		this.btnCancelUpdateDetial.TabIndex = 20;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(648, 61);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(88, 15);
		this.label20.TabIndex = 19;
		this.label20.Text = "Available Stock";
		this.CmbWareHouseFrom.AllowNull = (DefaultableBoolean)1;
		this.CmbWareHouseFrom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWareHouseFrom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWareHouseFrom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance135;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance89).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance90).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance90).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance90).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbWareHouseFrom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom).Location = new System.Drawing.Point(3, 31);
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom).Name = "CmbWareHouseFrom";
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom).Size = new System.Drawing.Size(171, 26);
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom).Leave += new System.EventHandler(CmbWareHouseFrom_TextChanged);
		this.txtAvgRate.BackColor = System.Drawing.Color.White;
		this.txtAvgRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAvgRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAvgRate.Location = new System.Drawing.Point(739, 79);
		this.txtAvgRate.Name = "txtAvgRate";
		this.txtAvgRate.Size = new System.Drawing.Size(88, 23);
		this.txtAvgRate.TabIndex = 15;
		this.txtAvgRate.Text = "0";
		this.txtAvgRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAvgRate.TextChanged += new System.EventHandler(txtAvgRate_TextChanged);
		this.txtAvgRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAvgRate_KeyPress);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(802, 120);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(64, 26);
		this.btnUpdateDetail.TabIndex = 19;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.txtAvailableStock.BackColor = System.Drawing.Color.White;
		this.txtAvailableStock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAvailableStock.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAvailableStock.Location = new System.Drawing.Point(649, 79);
		this.txtAvailableStock.Name = "txtAvailableStock";
		this.txtAvailableStock.ReadOnly = true;
		this.txtAvailableStock.Size = new System.Drawing.Size(89, 23);
		this.txtAvailableStock.TabIndex = 14;
		this.txtAvailableStock.TabStop = false;
		this.txtAvailableStock.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAvailableStock.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAvailableStock_KeyPress);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(354, 15);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(77, 15);
		this.label7.TabIndex = 35;
		this.label7.Text = "Job Lot From";
		this.lblAvgRate.AutoSize = true;
		this.lblAvgRate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAvgRate.Location = new System.Drawing.Point(739, 61);
		this.lblAvgRate.Name = "lblAvgRate";
		this.lblAvgRate.Size = new System.Drawing.Size(54, 15);
		this.lblAvgRate.TabIndex = 49;
		this.lblAvgRate.Text = "Avg Rate";
		this.CmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Location = new System.Drawing.Point(102, 77);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Name = "CmbPackUom";
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Size = new System.Drawing.Size(70, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TextChanged += new System.EventHandler(CmbPackUom_TextChanged);
		this.txtEbTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEbTotal.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEbTotal.Location = new System.Drawing.Point(418, 79);
		this.txtEbTotal.Name = "txtEbTotal";
		this.txtEbTotal.ReadOnly = true;
		this.txtEbTotal.Size = new System.Drawing.Size(81, 23);
		this.txtEbTotal.TabIndex = 11;
		this.txtEbTotal.TabStop = false;
		this.txtEbTotal.Text = "0";
		this.txtEbTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(501, 15);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(62, 15);
		this.label21.TabIndex = 35;
		this.label21.Text = "Job Lot To";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(417, 61);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(46, 15);
		this.label19.TabIndex = 47;
		this.label19.Text = "EbTotal";
		this.cmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.cmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Location = new System.Drawing.Point(354, 31);
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Name = "cmbJobLot";
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Size = new System.Drawing.Size(145, 26);
		((System.Windows.Forms.Control)(object)this.cmbJobLot).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Leave += new System.EventHandler(cmbJobLot_TextChanged);
		this.txtEbUnit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEbUnit.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEbUnit.Location = new System.Drawing.Point(354, 79);
		this.txtEbUnit.Name = "txtEbUnit";
		this.txtEbUnit.Size = new System.Drawing.Size(63, 23);
		this.txtEbUnit.TabIndex = 10;
		this.txtEbUnit.Text = "0";
		this.txtEbUnit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtEbUnit.TextChanged += new System.EventHandler(txtEbUnit_TextChanged);
		this.txtEbUnit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtEbUnit_KeyPress);
		this.CmbJobLotTo.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLotTo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLotTo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLotTo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLotTo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLotTo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance95).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLotTo).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbJobLotTo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLotTo).Location = new System.Drawing.Point(501, 31);
		((System.Windows.Forms.Control)(object)this.CmbJobLotTo).Name = "CmbJobLotTo";
		((System.Windows.Forms.Control)(object)this.CmbJobLotTo).Size = new System.Drawing.Size(146, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLotTo).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbJobLotTo).Leave += new System.EventHandler(cmbJobLot_TextChanged);
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(353, 61);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(42, 15);
		this.label16.TabIndex = 45;
		this.label16.Text = "EbUnit";
		this.cmbPackingType.AllowNull = (DefaultableBoolean)1;
		this.cmbPackingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance42;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance48).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance49).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPackingType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbPackingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackingType).Location = new System.Drawing.Point(3, 77);
		((System.Windows.Forms.Control)(object)this.cmbPackingType).Name = "cmbPackingType";
		((System.Windows.Forms.Control)(object)this.cmbPackingType).Size = new System.Drawing.Size(97, 26);
		((System.Windows.Forms.Control)(object)this.cmbPackingType).TabIndex = 6;
		this.txtRemarksDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDetail.Location = new System.Drawing.Point(3, 122);
		this.txtRemarksDetail.Name = "txtRemarksDetail";
		this.txtRemarksDetail.Size = new System.Drawing.Size(796, 23);
		this.txtRemarksDetail.TabIndex = 17;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(252, 61);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(78, 15);
		this.label8.TabIndex = 37;
		this.label8.Text = "Gross Weight";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(3, 106);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(52, 15);
		this.label12.TabIndex = 16;
		this.label12.Text = "Remarks";
		this.cmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.cmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance86).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance86).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance87;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance103).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance104).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance105).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Location = new System.Drawing.Point(649, 31);
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Name = "cmbCropYear";
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Size = new System.Drawing.Size(89, 26);
		((System.Windows.Forms.Control)(object)this.cmbCropYear).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Leave += new System.EventHandler(cmbCropYear_TextChanged);
		this.cmbItemName.AllowNull = (DefaultableBoolean)1;
		this.cmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance55;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance61).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance62).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemName).Location = new System.Drawing.Point(739, 31);
		((System.Windows.Forms.Control)(object)this.cmbItemName).Name = "cmbItemName";
		((System.Windows.Forms.Control)(object)this.cmbItemName).Size = new System.Drawing.Size(192, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemName).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.cmbItemName).TextChanged += new System.EventHandler(cmbItemName_TextChanged);
		((System.Windows.Forms.Control)(object)this.cmbItemName).Leave += new System.EventHandler(cmbItemName_Leave);
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtGrossWeight.Location = new System.Drawing.Point(247, 79);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.Size = new System.Drawing.Size(105, 23);
		this.txtGrossWeight.TabIndex = 9;
		this.txtGrossWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtGrossWeight.TextChanged += new System.EventHandler(txtGrossWeight_TextChanged);
		this.txtGrossWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGrossWeight_KeyPress);
		this.cmbWareHouseTo.AllowNull = (DefaultableBoolean)1;
		this.cmbWareHouseTo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWareHouseTo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWareHouseTo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance56;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance79).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbWareHouseTo).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbWareHouseTo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbWareHouseTo).Location = new System.Drawing.Point(176, 31);
		((System.Windows.Forms.Control)(object)this.cmbWareHouseTo).Name = "cmbWareHouseTo";
		((System.Windows.Forms.Control)(object)this.cmbWareHouseTo).Size = new System.Drawing.Size(176, 26);
		((System.Windows.Forms.Control)(object)this.cmbWareHouseTo).TabIndex = 1;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(649, 15);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(57, 15);
		this.label37.TabIndex = 9;
		this.label37.Text = "Crop Year";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(501, 61);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(57, 15);
		this.label9.TabIndex = 39;
		this.label9.Text = "Ad/Ls Wt";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(176, 15);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(85, 15);
		this.label11.TabIndex = 43;
		this.label11.Text = "WareHouse To";
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtQty.Location = new System.Drawing.Point(176, 79);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(68, 23);
		this.txtQty.TabIndex = 8;
		this.txtQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQty.TextChanged += new System.EventHandler(txtQty_TextChanged);
		this.txtQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(99, 61);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(63, 15);
		this.label36.TabIndex = 8;
		this.label36.Text = "Pack UOM";
		this.txtAdLsWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAdLsWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAdLsWeight.Location = new System.Drawing.Point(501, 79);
		this.txtAdLsWeight.Name = "txtAdLsWeight";
		this.txtAdLsWeight.Size = new System.Drawing.Size(56, 23);
		this.txtAdLsWeight.TabIndex = 12;
		this.txtAdLsWeight.Text = "0";
		this.txtAdLsWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAdLsWeight.TextChanged += new System.EventHandler(txtAdLsWeight_TextChanged);
		this.txtAdLsWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAdLsWeight_KeyPress);
		this.txtDetailNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDetailNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDetailNetWeight.Location = new System.Drawing.Point(559, 79);
		this.txtDetailNetWeight.Name = "txtDetailNetWeight";
		this.txtDetailNetWeight.ReadOnly = true;
		this.txtDetailNetWeight.Size = new System.Drawing.Size(88, 23);
		this.txtDetailNetWeight.TabIndex = 13;
		this.txtDetailNetWeight.TabStop = false;
		this.txtDetailNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(3, 61);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(77, 15);
		this.label45.TabIndex = 23;
		this.label45.Text = "Packing Type";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(176, 61);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(26, 15);
		this.label38.TabIndex = 13;
		this.label38.Text = "Qty";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(559, 61);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(79, 15);
		this.label10.TabIndex = 41;
		this.label10.Text = "Stock Weight";
		this.groupBox1.Controls.Add(this.DocDate);
		this.groupBox1.Controls.Add(this.txtdocno);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.txtWorkingReportNo);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.cmbGatePass);
		this.groupBox1.Controls.Add(this.cmbTicketNo);
		this.groupBox1.Controls.Add(this.txtHeadNetWeight);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTransferType);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.txtRemarksHead);
		this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 34);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(936, 96);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(60, 43);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(111, 22);
		this.DocDate.TabIndex = 1;
		this.DocDate.TabStop = false;
		this.DocDate.ValueChanged += new System.EventHandler(DocDate_ValueChanged);
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtdocno.Location = new System.Drawing.Point(60, 15);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(111, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_Leave);
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label30.Location = new System.Drawing.Point(4, 47);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(57, 15);
		this.label30.TabIndex = 3;
		this.label30.Text = "Doc Date";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label17.Location = new System.Drawing.Point(4, 19);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(48, 15);
		this.label17.TabIndex = 3;
		this.label17.Text = "Doc No";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label33.Location = new System.Drawing.Point(175, 47);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(58, 15);
		this.label33.TabIndex = 8;
		this.label33.Text = "ReportNo";
		this.txtWorkingReportNo.BackColor = System.Drawing.Color.White;
		this.txtWorkingReportNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWorkingReportNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtWorkingReportNo.Location = new System.Drawing.Point(238, 43);
		this.txtWorkingReportNo.Name = "txtWorkingReportNo";
		this.txtWorkingReportNo.Size = new System.Drawing.Size(209, 23);
		this.txtWorkingReportNo.TabIndex = 4;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label15.Location = new System.Drawing.Point(4, 72);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(52, 15);
		this.label15.TabIndex = 17;
		this.label15.Text = "Remarks";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(175, 19);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(63, 15);
		this.label14.TabIndex = 16;
		this.label14.Text = "Trans.Type";
		this.cmbGatePass.BackColor = System.Drawing.Color.White;
		this.cmbGatePass.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.cmbGatePass.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbGatePass.Location = new System.Drawing.Point(541, 15);
		this.cmbGatePass.Name = "cmbGatePass";
		this.cmbGatePass.Size = new System.Drawing.Size(114, 23);
		this.cmbGatePass.TabIndex = 5;
		this.cmbGatePass.KeyPress += new System.Windows.Forms.KeyPressEventHandler(cmbGatePass_KeyPress);
		this.cmbTicketNo.BackColor = System.Drawing.Color.White;
		this.cmbTicketNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.cmbTicketNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbTicketNo.Location = new System.Drawing.Point(541, 43);
		this.cmbTicketNo.Name = "cmbTicketNo";
		this.cmbTicketNo.Size = new System.Drawing.Size(114, 23);
		this.cmbTicketNo.TabIndex = 6;
		this.cmbTicketNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(cmbTicketNo_KeyPress);
		this.txtHeadNetWeight.BackColor = System.Drawing.Color.White;
		this.txtHeadNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtHeadNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtHeadNetWeight.Location = new System.Drawing.Point(541, 68);
		this.txtHeadNetWeight.Name = "txtHeadNetWeight";
		this.txtHeadNetWeight.Size = new System.Drawing.Size(114, 23);
		this.txtHeadNetWeight.TabIndex = 7;
		this.txtHeadNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtHeadNetWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtHeadNetWeight_KeyPress);
		this.CmbTransferType.AllowNull = (DefaultableBoolean)1;
		this.CmbTransferType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTransferType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTransferType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransferType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance111;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransferType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance116).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTransferType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbTransferType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTransferType).Location = new System.Drawing.Point(238, 13);
		((System.Windows.Forms.Control)(object)this.CmbTransferType).Name = "CmbTransferType";
		((System.Windows.Forms.Control)(object)this.CmbTransferType).Size = new System.Drawing.Size(209, 26);
		((System.Windows.Forms.Control)(object)this.CmbTransferType).TabIndex = 3;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(451, 19);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(64, 15);
		this.label1.TabIndex = 12;
		this.label1.Text = "Gate Pass#";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(451, 72);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(88, 15);
		this.label6.TabIndex = 14;
		this.label6.Text = "Factory Weight";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(451, 47);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(65, 15);
		this.label4.TabIndex = 13;
		this.label4.Text = "Ticket No#";
		this.txtRemarksHead.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksHead.Location = new System.Drawing.Point(60, 68);
		this.txtRemarksHead.Name = "txtRemarksHead";
		this.txtRemarksHead.Size = new System.Drawing.Size(387, 23);
		this.txtRemarksHead.TabIndex = 2;
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label24);
		this.panel12.Controls.Add(this.ChkBoxPrintPreview);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(952, 29);
		this.panel12.TabIndex = 53;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label24.ForeColor = System.Drawing.Color.White;
		this.label24.Location = new System.Drawing.Point(5, 3);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(128, 19);
		this.label24.TabIndex = 35;
		this.label24.Text = "Stock Transfer";
		this.ChkBoxPrintPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBoxPrintPreview.AutoSize = true;
		this.ChkBoxPrintPreview.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBoxPrintPreview.ForeColor = System.Drawing.Color.White;
		this.ChkBoxPrintPreview.Location = new System.Drawing.Point(863, 5);
		this.ChkBoxPrintPreview.Name = "ChkBoxPrintPreview";
		this.ChkBoxPrintPreview.Size = new System.Drawing.Size(84, 20);
		this.ChkBoxPrintPreview.TabIndex = 7;
		this.ChkBoxPrintPreview.Text = "Preview";
		this.ChkBoxPrintPreview.UseVisualStyleBackColor = true;
		this.combproject1.AllowNull = (DefaultableBoolean)1;
		this.combproject1.AutoCompleteMode = (AutoCompleteMode)4;
		this.combproject1.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combproject1.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combproject1).DisplayLayout.Appearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.combproject1).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combproject1).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance97).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance97).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combproject1).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance98).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combproject1).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance98;
		((SpecialBoxBase)((UltraGridBase)this.combproject1).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance123).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance123).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combproject1).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.combproject1).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combproject1).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance127).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).TextHAlignAsString = "Left";
		((UltraGridBase)this.combproject1).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.combproject1).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combproject1).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combproject1).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combproject1).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combproject1.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combproject1).Location = new System.Drawing.Point(1042, 42);
		((System.Windows.Forms.Control)(object)this.combproject1).Name = "combproject1";
		((System.Windows.Forms.Control)(object)this.combproject1).Size = new System.Drawing.Size(111, 26);
		((System.Windows.Forms.Control)(object)this.combproject1).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.combproject1).TabStop = false;
		((System.Windows.Forms.Control)(object)this.combproject1).Visible = false;
		this.cmbbranch1.AllowNull = (DefaultableBoolean)1;
		this.cmbbranch1.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbbranch1.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbbranch1.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbbranch1).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance80;
		((SpecialBoxBase)((UltraGridBase)this.cmbbranch1).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance99).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance130).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance140).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance140).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance140).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbbranch1).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbbranch1.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbbranch1).Location = new System.Drawing.Point(1042, 68);
		((System.Windows.Forms.Control)(object)this.cmbbranch1).Name = "cmbbranch1";
		((System.Windows.Forms.Control)(object)this.cmbbranch1).Size = new System.Drawing.Size(111, 26);
		((System.Windows.Forms.Control)(object)this.cmbbranch1).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbbranch1).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbbranch1).Visible = false;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(976, 74);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(44, 15);
		this.label13.TabIndex = 0;
		this.label13.Text = "Branch";
		this.label13.Visible = false;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label18.Location = new System.Drawing.Point(984, 48);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(44, 15);
		this.label18.TabIndex = 0;
		this.label18.Text = "Project";
		this.label18.Visible = false;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(988, 38);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(48, 15);
		this.label5.TabIndex = 3;
		this.label5.Text = "Doc No";
		this.label5.Visible = false;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(952, 28);
		this.panel2.TabIndex = 0;
		this.toolStrip1.BackColor = System.Drawing.Color.White;
		this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.btnnew, this.btnRefresh, this.btnupdate, this.btnsave, this.btnDelete, this.btnprint, this.btnAttachment, this.btnLoadInvoices, this.btnGenerateRate });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(799, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnupdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnprint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(92, 24);
		this.btnprint.Text = "407-Print";
		this.btnprint.Click += new System.EventHandler(btnprint_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnLoadInvoices.Image = (System.Drawing.Image)resources.GetObject("btnLoadInvoices.Image");
		this.btnLoadInvoices.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadInvoices.Name = "btnLoadInvoices";
		this.btnLoadInvoices.Size = new System.Drawing.Size(75, 24);
		this.btnLoadInvoices.Text = "Loader";
		this.btnLoadInvoices.Click += new System.EventHandler(btnLoadInvoices_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(956, 632);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel8.Controls.Add(this.panel21);
		this.panel8.Controls.Add(this.panel18);
		this.panel8.Controls.Add(this.panel17);
		this.panel8.Controls.Add(this.panel20);
		this.panel8.Controls.Add(this.panel16);
		this.panel8.Controls.Add(this.panel10);
		this.panel8.Controls.Add(this.toolStrip2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(2, 2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(952, 628);
		this.panel8.TabIndex = 1;
		this.panel21.Controls.Add((System.Windows.Forms.Control)(object)this.GridDetailHistory);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel21.Location = new System.Drawing.Point(0, 416);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(952, 212);
		this.panel21.TabIndex = 53;
		this.GridDetailHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.GridDetailHistory).BackColor = System.Drawing.Color.White;
		GridDetailHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridDetailHistory.DesignTimeLayout = GridDetailHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridDetailHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridDetailHistory.DynamicFiltering = true;
		this.GridDetailHistory.FilterMode = (FilterMode)2;
		this.GridDetailHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridDetailHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridDetailHistory.GroupByBoxVisible = false;
		this.GridDetailHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GridDetailHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GridDetailHistory).Name = "GridDetailHistory";
		((ScrollableBase)this.GridDetailHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridDetailHistory).Size = new System.Drawing.Size(952, 212);
		((System.Windows.Forms.Control)(object)this.GridDetailHistory).TabIndex = 47;
		this.GridDetailHistory.TotalRow = (InheritableBoolean)1;
		this.GridDetailHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridDetailHistory).VisualStyle = (VisualStyle)4;
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.label29);
		this.panel18.Controls.Add(this.ctrlGrdBar3);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(0, 394);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(952, 22);
		this.panel18.TabIndex = 52;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label29.ForeColor = System.Drawing.Color.White;
		this.label29.Location = new System.Drawing.Point(4, 5);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(269, 14);
		this.label29.TabIndex = 37;
		this.label29.Text = "Detail Information Of Above Selected Row";
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(912, 2);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.GridDetailHistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 20);
		this.ctrlGrdBar3.TabIndex = 24;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar6_Load);
		this.panel17.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(0, 144);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(952, 250);
		this.panel17.TabIndex = 51;
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
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(952, 250);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 45;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.grdhistory.SelectionChanged += new System.EventHandler(grdhistory_SelectionChanged);
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick_1);
		this.panel20.BackColor = System.Drawing.Color.Teal;
		this.panel20.Controls.Add(this.label25);
		this.panel20.Controls.Add(this.ctrlGrdBar7);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(0, 121);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(952, 23);
		this.panel20.TabIndex = 50;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label25.ForeColor = System.Drawing.Color.White;
		this.label25.Location = new System.Drawing.Point(3, 3);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(132, 18);
		this.label25.TabIndex = 0;
		this.label25.Text = "Filtered Records";
		this.ctrlGrdBar7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar7.FormName = this;
		this.ctrlGrdBar7.Location = new System.Drawing.Point(912, 0);
		this.ctrlGrdBar7.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar7.MyGrid = this.grdhistory;
		this.ctrlGrdBar7.Name = "ctrlGrdBar7";
		this.ctrlGrdBar7.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar7.TabIndex = 23;
		this.ctrlGrdBar7.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.panel16.Controls.Add(this.groupBox3);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 54);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(952, 67);
		this.panel16.TabIndex = 5;
		this.groupBox3.Controls.Add(this.rdapproveddate);
		this.groupBox3.Controls.Add(this.rdmodifydate);
		this.groupBox3.Controls.Add(this.drdocdate);
		this.groupBox3.Controls.Add(this.rdentrydate);
		this.groupBox3.Controls.Add(this.btnshow);
		this.groupBox3.Controls.Add(this.label27);
		this.groupBox3.Controls.Add(this.label28);
		this.groupBox3.Controls.Add(this.label43);
		this.groupBox3.Controls.Add(this.label62);
		this.groupBox3.Controls.Add(this.label63);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbTransactiontypeHistory);
		this.groupBox3.Controls.Add(this.txtToDocNoHistory);
		this.groupBox3.Controls.Add(this.txtFromDocNoHistory);
		this.groupBox3.Controls.Add(this.ToDateHistory);
		this.groupBox3.Controls.Add(this.FromDateHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(3, 1);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(944, 66);
		this.groupBox3.TabIndex = 2;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(831, 43);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 9;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(831, 23);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(750, 23);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 6;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(750, 43);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 7;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(673, 36);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(72, 26);
		this.btnshow.TabIndex = 5;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label27.Location = new System.Drawing.Point(473, 19);
		this.label27.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(77, 15);
		this.label27.TabIndex = 142;
		this.label27.Text = "Transfer Type";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label28.Location = new System.Drawing.Point(371, 19);
		this.label28.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(64, 15);
		this.label28.TabIndex = 141;
		this.label28.Text = "To Doc No";
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label43.Location = new System.Drawing.Point(269, 19);
		this.label43.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(79, 15);
		this.label43.TabIndex = 140;
		this.label43.Text = "From Doc No";
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label62.Location = new System.Drawing.Point(136, 19);
		this.label62.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(48, 15);
		this.label62.TabIndex = 139;
		this.label62.Text = "To Date";
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label63.Location = new System.Drawing.Point(5, 20);
		this.label63.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(63, 15);
		this.label63.TabIndex = 13;
		this.label63.Text = "From Date";
		this.cmbTransactiontypeHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbTransactiontypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbTransactiontypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbTransactiontypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbTransactiontypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbTransactiontypeHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbTransactiontypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbTransactiontypeHistory).Location = new System.Drawing.Point(473, 36);
		((System.Windows.Forms.Control)(object)this.cmbTransactiontypeHistory).Name = "cmbTransactiontypeHistory";
		((System.Windows.Forms.Control)(object)this.cmbTransactiontypeHistory).Size = new System.Drawing.Size(194, 26);
		((System.Windows.Forms.Control)(object)this.cmbTransactiontypeHistory).TabIndex = 4;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(371, 38);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.ShortcutsEnabled = false;
		this.txtToDocNoHistory.Size = new System.Drawing.Size(100, 23);
		this.txtToDocNoHistory.TabIndex = 3;
		this.txtFromDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoHistory.Location = new System.Drawing.Point(269, 38);
		this.txtFromDocNoHistory.Name = "txtFromDocNoHistory";
		this.txtFromDocNoHistory.ShortcutsEnabled = false;
		this.txtFromDocNoHistory.Size = new System.Drawing.Size(100, 23);
		this.txtFromDocNoHistory.TabIndex = 2;
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(136, 38);
		this.ToDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(131, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(5, 38);
		this.FromDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(128, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.label26);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.ForeColor = System.Drawing.Color.Teal;
		this.panel10.Location = new System.Drawing.Point(0, 27);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(952, 27);
		this.panel10.TabIndex = 4;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label26.ForeColor = System.Drawing.Color.White;
		this.label26.Location = new System.Drawing.Point(4, 3);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(196, 19);
		this.label26.TabIndex = 1;
		this.label26.Text = "Stock Transfer  History";
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNewHistory, this.btnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(952, 27);
		this.toolStrip2.TabIndex = 3;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(68, 24);
		this.btnNewHistory.Text = "&Reset";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "&Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.btnGenerateRate.Image = Architecture.WinApp.Properties.Resources.Taxation;
		this.btnGenerateRate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnGenerateRate.Name = "btnGenerateRate";
		this.btnGenerateRate.Size = new System.Drawing.Size(126, 24);
		this.btnGenerateRate.Text = "Generate Rate";
		this.btnGenerateRate.Click += new System.EventHandler(btnGenerateRate_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(964, 661);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "frmStockTransferManual";
		this.Text = "Stock Transfer";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmStockTransferManual_FormClosing);
		base.Load += new System.EventHandler(PurchsaeOrder_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(PurchsaeOrder_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.PanelMessage.ResumeLayout(false);
		this.PanelMessage.PerformLayout();
		this.ToolStripMessage.ResumeLayout(false);
		this.ToolStripMessage.PerformLayout();
		this.tabPage4.ResumeLayout(false);
		this.panel14.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOtherCharges).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouseFrom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLotTo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackingType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouseTo).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTransferType).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.combproject1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbranch1).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel21.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridDetailHistory).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		this.panel17.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel20.ResumeLayout(false);
		this.panel20.PerformLayout();
		this.panel16.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbTransactiontypeHistory).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
