using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.ContractorWages;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.ContractorWages;
using Architecture.Model.Inventory;
using Architecture.WinApp.Common;
using Architecture.WinApp.Contractor_Wages;
using Architecture.WinApp.Lookups;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.PartyProcessing;

public class frmWagesBillPartyProcessing : Form
{
	private bool IsRegularOrFreeOfCost;

	private int RECID;

	private int Id;

	private int PreviousContractorId;

	private int PreviousContractorIdStiching;

	public bool UpdateMode;

	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable table = new DataTable();

	private DataTable dtcus = new DataTable();

	private DataTable dtwagesacc = new DataTable();

	private DataTable dtStichingItems = new DataTable();

	private DataTable dtdetail = new DataTable();

	private DataTable dtStiching = new DataTable();

	private DataTable dtpendingGpForWages = new DataTable();

	private bool WagesAmountCalculateOnQty;

	public int RefDocTypeId;

	public int RefDocId;

	public double GrossWeightTotal;

	private bool flag;

	private bool IsReferred;

	private bool StichingWagesCompulsory;

	private bool EnableAddLessOnWagesRegular;

	private double PercentageForRateAddLess;

	private Rightsobjects formright = new Rightsobjects();

	private int[] AryColWidth;

	private IContainer components;

	private Panel panel1;

	private TabControl tabIncomingLabourwages;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel4;

	private Panel panel3;

	private Label label15;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripButton Print;

	private CheckBox chkisapprove;

	private TextBox txtdocnumber;

	private DateTimePicker txtDocdate;

	private Label label6;

	private Label label1;

	internal GridEX grdwagesDetail;

	private Label label29;

	private TextBox txtGrossWeight;

	internal GridEX gridPendingWagesSlip;

	private Panel panel7;

	internal GridEX DataGridHistory;

	private Panel panel10;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label26;

	private Panel panel9;

	private ToolStrip toolStrip2;

	private ToolStripButton btnLoadAll;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton btnRefresh;

	private BackgroundWorker backgroundWorker1;

	private Label label11;

	internal UltraCombo cmbReferenceDocType;

	private TextBox txtDocNo;

	private TextBox txtGpNo;

	private Label label27;

	private Label label3;

	private TextBox txtGRNId;

	private CtrlGrdBar ctrlGrdBar2;

	private TextBox txtEntryType;

	private Label label4;

	private ToolStripButton btnWagesSchedule;

	internal GridEX grdStiching;

	private ToolStripButton btnWagesExempt;

	private Panel panel5;

	private TabControl tabControl2;

	private TabPage tabPage5;

	private Panel panel18;

	private Panel panel8;

	private CtrlGrdBar ctrlGrdBar5;

	private Label label7;

	private TabPage tabPage6;

	private Panel panel19;

	internal GridEX grdDataRetrieve;

	private Panel panel20;

	private CtrlGrdBar ctrlGrdBar6;

	private Label label8;

	private Panel panel6;

	private TabControl tabControl1;

	private TabPage tabPage3;

	private Panel panel13;

	private Panel panel15;

	private Label label2;

	private TabPage tabPage4;

	private Panel panel16;

	private Panel panel17;

	private CtrlGrdBar ctrlGrdBar4;

	private Label label5;

	private Label label9;

	private TextBox txtQty;

	private GroupBox groupBox1;

	internal GridEX GrdHistoryDetail;

	private Label label10;

	private Label label12;

	private Button btnAddContractorValuesForAllDetailWages;

	private Label label13;

	private Button btnAddContractorValuesForAllSticingWages;

	private Panel panel11;

	private GroupBox groupBox2;

	private Button btnShow;

	private Label label50;

	private Label label51;

	private Label label52;

	private Label label53;

	private Label label54;

	internal UltraCombo CmbDocumentTypeHistory;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Panel panel14;

	private Panel panel21;

	private Panel panel12;

	private ToolStripButton btnResetHistory;

	private ToolStripButton btnRefreshHistory;

	private TextBox txtRemarks;

	internal UltraCombo CmbStockParty;

	private Label label14;

	public frmWagesBillPartyProcessing(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmWagesBillPartyProcessing_Load(object sender, EventArgs e)
	{
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_049c: Expected O, but got Unknown
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 700);
			EnableAddLessOnWagesRegular = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("EnableAddLessOnWagesPartyProccessing"));
			PercentageForRateAddLess = Conversion.ToDouble((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("PercentageForRateAddLessPartyProcessing"));
			tabControl1.TabPages.Remove(tabPage4);
			tabControl2.TabPages.Remove(tabPage6);
			if (RefDocTypeId == 118 || RefDocTypeId == 176)
			{
				tabControl1.TabPages.Add(tabPage4);
			}
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			Print.Enabled = formright.DoHavePrintRights;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			WagesAmountCalculationsConfigurations();
			suppliercustomer();
			accountName();
			GenerateDocNo();
			dtdetail.Columns.Add("SupplierId");
			dtdetail.Columns.Add("ContractorName");
			dtdetail.Columns.Add("WagesId", typeof(int));
			dtdetail.Columns.Add("WagesAccount");
			dtdetail.Columns.Add("WagesType");
			dtdetail.Columns.Add("Date");
			dtdetail.Columns.Add("packingTypeId");
			dtdetail.Columns.Add("packingType");
			dtdetail.Columns.Add("Weight", typeof(double));
			dtdetail.Columns.Add("PackSize");
			dtdetail.Columns.Add("Quantity", typeof(double));
			dtdetail.Columns.Add("WeightCut");
			dtdetail.Columns.Add("BillQty", typeof(double));
			dtdetail.Columns.Add("BillWeight", typeof(double));
			dtdetail.Columns.Add("RateWithoutAddLess");
			dtdetail.Columns.Add("RateAddLess", typeof(double));
			dtdetail.Columns.Add("Rate");
			dtdetail.Columns.Add("Amount", typeof(double));
			dtdetail.Columns.Add("ItemId");
			dtdetail.Columns.Add("Item");
			dtdetail.Columns.Add("jobLotId");
			dtdetail.Columns.Add("jobLot");
			dtdetail.Columns.Add("Crop");
			dtdetail.Columns.Add("MoveFromId");
			dtdetail.Columns.Add("MoveFrom");
			dtdetail.Columns.Add("MoveToId");
			dtdetail.Columns.Add("MoveTo");
			dtdetail.Columns.Add("PurchaseGLAC");
			dtdetail.Columns.Add("WarehouseType");
			dtdetail.Columns.Add("IsCompany", typeof(bool));
			dtdetail.Columns.Add("JobOrderId", typeof(int));
			dtdetail.Columns.Add("RefDocQty", typeof(double));
			dtdetail.Columns.Add("RefDocWeight", typeof(double));
			dtdetail.Columns.Add("RefLineId", typeof(int));
			if (RefDocTypeId > 0 && RefDocId > 0)
			{
				InvContractorWagesBillHeader obj = new InvContractorWagesBillHeader();
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				obj.RefDocumentTypeId = RefDocTypeId;
				obj.RefDocNoId = RefDocId;
				RECID = InvContractorWagesBillHeader.GetIdByRefDocTypeIdAndRefDocId(obj);
				if (RECID > 0)
				{
					ReadOnlyMasterById(RECID);
					if (GrossWeightTotal > 0.0)
					{
						txtGrossWeight.Text = GrossWeightTotal.ToString();
					}
					if (RefDocTypeId != 176)
					{
						tabControl2.TabPages.Remove(tabPage5);
					}
					tabControl2.TabPages.Add(tabPage6);
					RetreivalHistoryDetailGridBind(RECID);
				}
				else
				{
					PendingGrnAndGdn();
					LoadDataForWages();
				}
			}
			else
			{
				PendingTicket();
			}
			DocumentTypeFillForCombo();
			StichingWagesConfig();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StichingWagesConfig()
	{
		try
		{
			if (Conversion.ToInt(cmbReferenceDocType.Value) == 118)
			{
				StichingWagesCompulsory = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("OtherWagesCompulsoryForProductionPartyProcessing"));
			}
			else if (Conversion.ToInt(cmbReferenceDocType.Value) == 176)
			{
				StichingWagesCompulsory = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("OtherWagesCompulsoryForStockConversionPartyProcessing"));
			}
			else
			{
				StichingWagesCompulsory = false;
			}
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
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		try
		{
			int code = InvContractorWagesBillHeader.GenerateCode(new InvContractorWagesBillHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				RefDocumentTypeId = 219,
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

	private void suppliercustomer()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			SupplierCustomer supplier = new SupplierCustomer();
			supplier.OrganizationId = UserAccount.OrganizationId;
			supplier.CompanyId = UserAccount.CompanyId;
			dtcus = SupplierCustomer.ReadByOrganizationCompanyIdForContractorWages(supplier);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void accountName(int DocumentTypeId = 0)
	{
		try
		{
			string Ids = null;
			if (RefDocTypeId == 49 || RefDocTypeId == 217 || DocumentTypeId == 49 || DocumentTypeId == 117)
			{
				Ids = "34,36,44";
			}
			else if (RefDocTypeId == 89 || RefDocTypeId == 118 || DocumentTypeId == 89 || DocumentTypeId == 118)
			{
				Ids = "33,36,44";
			}
			else if (RefDocTypeId == 220 || DocumentTypeId == 220)
			{
				Ids = "33,34";
			}
			dtwagesacc = CommonServices.GetWagesAccount(Ids, 0, 1);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WagesAmountCalculationsConfigurations()
	{
		try
		{
			string Qty = CommonServices.GetConfigurationByOrgCompandConfigDescription("WagesAmountCalculateOnQty");
			if (Qty != string.Empty)
			{
				WagesAmountCalculateOnQty = Conversion.ToBool((object)Qty);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocumentTypeFillForCombo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = InvContractorWagesBillHeader.GetDocumentTypesFromContractorWages(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbDocumentTypeHistory, "Id", "DocumentTypeDescription", "Document Type", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetailLockEditing()
	{
		try
		{
			if (grdwagesDetail.RowCount > 0)
			{
				if (!IsReferred)
				{
					grdwagesDetail.RootTable.Columns["PurchaseGLAC"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["WagesType"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["ContractorName"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["WagesAccount"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["ItemId"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["jobLotId"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["packingTypeId"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["MoveFromId"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["MoveToId"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["BillQty"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["Date"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["packingType"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["jobLot"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["Item"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["MoveFrom"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["MoveTo"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["Crop"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["PackSize"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["Rate"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["Amount"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["BillWeight"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["WagesType"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["WarehouseType"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["JobOrderId"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["WagesId"].EditType = (EditType)4;
					grdwagesDetail.RootTable.Columns["SupplierId"].EditType = (EditType)4;
					grdwagesDetail.RootTable.Columns["Quantity"].EditType = (EditType)1;
					grdwagesDetail.RootTable.Columns["IsCompany"].EditType = (EditType)8;
				}
				else
				{
					grdwagesDetail.RootTable.Columns["PurchaseGLAC"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["WagesType"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["ContractorName"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["WagesAccount"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["ItemId"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["jobLotId"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["packingTypeId"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["MoveFromId"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["MoveToId"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["BillQty"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["Date"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["packingType"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["jobLot"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["Item"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["MoveFrom"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["MoveTo"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["Crop"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["PackSize"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["Rate"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["Amount"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["BillWeight"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["WagesType"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["WarehouseType"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["WagesId"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["JobOrderId"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["SupplierId"].EditType = (EditType)4;
					grdwagesDetail.RootTable.Columns["Quantity"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["Weight"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["WeightCut"].EditType = (EditType)0;
					grdwagesDetail.RootTable.Columns["IsCompany"].EditType = (EditType)0;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailGridSettings()
	{
		//IL_0c75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca1: Expected O, but got Unknown
		try
		{
			WagesItemAndContractorRefresh();
			grdwagesDetail.RootTable.Columns["PurchaseGLAC"].Visible = false;
			grdwagesDetail.RootTable.Columns["WarehouseType"].Visible = false;
			grdwagesDetail.RootTable.Columns["ContractorName"].Visible = false;
			grdwagesDetail.RootTable.Columns["WagesAccount"].Visible = false;
			grdwagesDetail.RootTable.Columns["ItemId"].Visible = false;
			grdwagesDetail.RootTable.Columns["jobLotId"].Visible = false;
			grdwagesDetail.RootTable.Columns["packingTypeId"].Visible = false;
			grdwagesDetail.RootTable.Columns["MoveFromId"].Visible = false;
			grdwagesDetail.RootTable.Columns["MoveToId"].Visible = false;
			grdwagesDetail.RootTable.Columns["BillQty"].Visible = false;
			grdwagesDetail.RootTable.Columns["JobOrderId"].Visible = false;
			grdwagesDetail.RootTable.Columns["MoveTo"].Visible = false;
			grdwagesDetail.RootTable.Columns["RefDocQty"].Visible = false;
			grdwagesDetail.RootTable.Columns["RefDocWeight"].Visible = false;
			grdwagesDetail.RootTable.Columns["RateAddLess"].Visible = EnableAddLessOnWagesRegular;
			grdwagesDetail.RootTable.Columns["RateWithoutAddLess"].Visible = EnableAddLessOnWagesRegular;
			grdwagesDetail.RootTable.Columns["RefLineId"].Visible = Conversion.ToInt(cmbReferenceDocType.Value) == 118 || Conversion.ToInt(cmbReferenceDocType.Value) == 176;
			grdwagesDetail.RootTable.Columns["MoveTo"].Visible = Conversion.ToInt(cmbReferenceDocType.Value) == 220;
			grdwagesDetail.RootTable.Columns["PurchaseGLAC"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["WagesType"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["ContractorName"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["WagesAccount"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["jobLotId"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["packingTypeId"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["MoveFromId"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["MoveToId"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["BillQty"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["JobOrderId"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["RefDocQty"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["RefDocWeight"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["RefLineId"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["RefLineId"].Caption = "RowNo";
			grdwagesDetail.RootTable.Columns["Date"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["packingType"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["jobLot"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["Item"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["MoveFrom"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["MoveTo"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["Crop"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["PackSize"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["Rate"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["BillWeight"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["WagesType"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["WarehouseType"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["RateAddLess"].EditType = (EditType)(EnableAddLessOnWagesRegular ? 1 : 0);
			grdwagesDetail.RootTable.Columns["RateWithoutAddLess"].EditType = (EditType)0;
			grdwagesDetail.RootTable.Columns["Quantity"].EditType = (EditType)1;
			grdwagesDetail.RootTable.Columns["IsCompany"].ColumnType = (ColumnType)3;
			grdwagesDetail.RootTable.Columns["IsCompany"].EditType = (EditType)8;
			grdwagesDetail.RootTable.Columns["IsCompany"].UseHeaderSelector = true;
			grdwagesDetail.RootTable.Columns["Quantity"].AggregateFunction = (AggregateFunction)2;
			grdwagesDetail.RootTable.Columns["Quantity"].TextAlignment = (TextAlignment)3;
			grdwagesDetail.RootTable.Columns["Quantity"].FormatString = "#,##0.####";
			grdwagesDetail.RootTable.Columns["Quantity"].TotalFormatString = "#,##0.####";
			grdwagesDetail.RootTable.Columns["BillQty"].AggregateFunction = (AggregateFunction)2;
			grdwagesDetail.RootTable.Columns["BillQty"].TextAlignment = (TextAlignment)3;
			grdwagesDetail.RootTable.Columns["BillQty"].FormatString = "#,##0.####";
			grdwagesDetail.RootTable.Columns["BillQty"].TotalFormatString = "#,##0.####";
			grdwagesDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdwagesDetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdwagesDetail.RootTable.Columns["Weight"].FormatString = "#,##0.####";
			grdwagesDetail.RootTable.Columns["Weight"].TotalFormatString = "#,##0.####";
			grdwagesDetail.RootTable.Columns["BillWeight"].AggregateFunction = (AggregateFunction)2;
			grdwagesDetail.RootTable.Columns["BillWeight"].TextAlignment = (TextAlignment)3;
			grdwagesDetail.RootTable.Columns["BillWeight"].FormatString = "#,##0.####";
			grdwagesDetail.RootTable.Columns["BillWeight"].TotalFormatString = "#,##0.####";
			grdwagesDetail.RootTable.Columns["RateWithoutAddLess"].AggregateFunction = (AggregateFunction)2;
			grdwagesDetail.RootTable.Columns["RateWithoutAddLess"].TextAlignment = (TextAlignment)3;
			grdwagesDetail.RootTable.Columns["RateWithoutAddLess"].FormatString = "#,##0.####";
			grdwagesDetail.RootTable.Columns["RateWithoutAddLess"].TotalFormatString = "#,##0.####";
			grdwagesDetail.RootTable.Columns["RateAddLess"].AggregateFunction = (AggregateFunction)2;
			grdwagesDetail.RootTable.Columns["RateAddLess"].TextAlignment = (TextAlignment)3;
			grdwagesDetail.RootTable.Columns["RateAddLess"].FormatString = "#,##0.####";
			grdwagesDetail.RootTable.Columns["RateAddLess"].TotalFormatString = "#,##0.####";
			grdwagesDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdwagesDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdwagesDetail.RootTable.Columns["Amount"].FormatString = "#,##0.####";
			grdwagesDetail.RootTable.Columns["Amount"].TotalFormatString = "#,##0.####";
			if (!IsReferred)
			{
				grdwagesDetail.RootTable.Columns.Add("Add");
				grdwagesDetail.RootTable.Columns["Add"].Key = "Add";
				grdwagesDetail.RootTable.Columns["Add"].Caption = "Add";
				grdwagesDetail.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdwagesDetail.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
				grdwagesDetail.RootTable.Columns["Add"].ButtonText = "Add";
				grdwagesDetail.RootTable.Columns["Add"].Width = 40;
				grdwagesDetail.RootTable.Columns.Add("Delete");
				grdwagesDetail.RootTable.Columns["Delete"].Key = "Delete";
				grdwagesDetail.RootTable.Columns["Delete"].Caption = "X";
				grdwagesDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdwagesDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
				grdwagesDetail.RootTable.Columns["Delete"].ButtonText = "X";
				grdwagesDetail.RootTable.Columns["Delete"].Width = 20;
			}
			new GridEXColumn();
			GridEXFormatCondition fc = new GridEXFormatCondition(grdwagesDetail.RootTable.Columns["WagesType"], (ConditionOperator)0, (object)"Free Of Cost");
			fc.FormatStyle.ForeColor = Color.Red;
			grdwagesDetail.RootTable.FormatConditions.Add(fc);
			CommonServices.GridAutoAdjustment(AryColWidth, grdwagesDetail);
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WagesItemAndContractorRefresh()
	{
		try
		{
			grdwagesDetail.RootTable.Columns["SupplierId"].ColumnType = (ColumnType)0;
			grdwagesDetail.RootTable.Columns["SupplierId"].HasValueList = true;
			grdwagesDetail.RootTable.Columns["SupplierId"].LimitToList = true;
			grdwagesDetail.RootTable.Columns["SupplierId"].EditType = (EditType)4;
			grdwagesDetail.RootTable.Columns["SupplierId"].Caption = "Contractor Name";
			grdwagesDetail.RootTable.Columns["SupplierId"].ValueList.PopulateValueList((IEnumerable)dtcus.DefaultView, "Id", "CompanyName");
			grdwagesDetail.RootTable.Columns["SupplierId"].Width = 200;
			grdwagesDetail.RootTable.Columns["WagesId"].ColumnType = (ColumnType)0;
			grdwagesDetail.RootTable.Columns["WagesId"].HasValueList = true;
			grdwagesDetail.RootTable.Columns["WagesId"].LimitToList = true;
			grdwagesDetail.RootTable.Columns["WagesId"].EditType = (EditType)4;
			grdwagesDetail.RootTable.Columns["WagesId"].Caption = "Wages Account";
			grdwagesDetail.RootTable.Columns["WagesId"].ValueList.PopulateValueList((IEnumerable)dtwagesacc.DefaultView, "Id", "WagesAccountName");
			grdwagesDetail.RootTable.Columns["WagesId"].Width = 200;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdwagesDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "IsCompany")
			{
				return;
			}
			GridEXRow selectedRow = grdwagesDetail.CurrentRow;
			double WagesRate = 0.0;
			double AllQtyExceptSelected = (from GridEXRow row in grdwagesDetail.GetRows()
				where row != selectedRow
				select (double)row.Cells["Quantity"].Value).Sum();
			double AllNetWeightExceptSelected = (from GridEXRow row in grdwagesDetail.GetRows()
				where row != selectedRow
				select (double)row.Cells["BillWeight"].Value).Sum();
			double AllGrossWeightExceptSelected = (from GridEXRow row in grdwagesDetail.GetRows()
				where row != selectedRow
				select (double)row.Cells["Weight"].Value).Sum();
			double RegularTotalWeight = Conversion.ToDouble((object)txtGrossWeight.Text) - AllNetWeightExceptSelected;
			double RegularGrossWeight = Conversion.ToDouble((object)txtGrossWeight.Text) - AllGrossWeightExceptSelected;
			double RegularTotalQty = Conversion.ToDouble((object)txtQty.Text) - AllQtyExceptSelected;
			grdwagesDetail.UpdateData();
			GridEXRow item = grdwagesDetail.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				double PreviousWagesRate = Conversion.ToDouble(item.Cells["Rate"].Value);
				item.BeginEdit();
				if (e.Column.Key == "ContractorName" || e.Column.Key == "WagesId" || e.Column.Key == "WagesAccount")
				{
					IsRegularOrFreeOfCost = CommonServices.CheckItemsFreeofcostforWages(Conversion.ToDateTime(item.Cells["Date"].Value), Conversion.ToInt(cmbReferenceDocType.Value), Conversion.ToInt(item.Cells["ItemId"].Value), Conversion.ToInt(item.Cells["WagesId"].Value));
					if (IsRegularOrFreeOfCost)
					{
						item.Cells["Wagestype"].Value = "Free Of Cost";
					}
					else
					{
						item.Cells["Wagestype"].Value = "Regular";
					}
				}
				if ((e.Column.Key == "Quantity" || e.Column.Key == "WeightCut") && Conversion.ToDouble(item.Cells["Quantity"].Value) > 0.0 && Conversion.ToDouble(item.Cells["PackSize"].Value) > 0.0)
				{
					double Qty2 = Conversion.ToDouble(item.Cells["Quantity"].Value);
					if (WagesAmountCalculateOnQty && Qty2 > RegularTotalQty && Conversion.ToInt(cmbReferenceDocType.Value) != 118 && Conversion.ToInt(cmbReferenceDocType.Value) != 176)
					{
						MessageBox.Show("Stiching Total Qty can not be Greater than Wages total Qty");
						item.Cells["Quantity"].Value = Math.Round(RegularTotalQty, 2);
						Qty2 = Math.Round(RegularTotalQty, 2);
					}
					else
					{
						item.Cells["Quantity"].Value = Math.Round(Qty2, 2);
					}
					double WeightCut = Conversion.ToDouble(item.Cells["WeightCut"].Value);
					double PackSize2 = Conversion.ToDouble(item.Cells["PackSize"].Value);
					double Weight = Qty2 * PackSize2;
					if (!WagesAmountCalculateOnQty && Weight > RegularGrossWeight && Conversion.ToInt(cmbReferenceDocType.Value) != 118 && Conversion.ToInt(cmbReferenceDocType.Value) != 176)
					{
						item.Cells["Weight"].Value = Math.Round(RegularGrossWeight, 2);
						MessageBox.Show("Stiching Weight can not be Greater than Wages Gross Weight");
						Weight = Math.Round(RegularGrossWeight, 2);
						Qty2 = Math.Round(Weight / PackSize2, 2);
						item.Cells["Quantity"].Value = Qty2;
					}
					item.Cells["Weight"].Value = Weight.ToString();
					double WeightCutTotal2 = Qty2 * WeightCut;
					double BillWeight4 = Weight - Math.Abs(WeightCutTotal2);
					if (!WagesAmountCalculateOnQty && BillWeight4 > RegularTotalWeight && Conversion.ToInt(cmbReferenceDocType.Value) != 118 && Conversion.ToInt(cmbReferenceDocType.Value) != 176)
					{
						item.Cells["Weight"].Value = Math.Round(RegularGrossWeight, 2);
						item.Cells["BillWeight"].Value = Math.Round(RegularTotalWeight, 2);
						MessageBox.Show("Stiching Total Weight can not be Greater than Wages total Weight");
						item.Cells["Quantity"].Value = Math.Round(Qty2 * PackSize2, 2);
					}
					else
					{
						item.Cells["BillWeight"].Value = Math.Round(BillWeight4, 2);
					}
				}
				if ((e.Column.Key == "Weight" || e.Column.Key == "WeightCut") && Conversion.ToDouble(item.Cells["Weight"].Value) > 0.0 && Conversion.ToDouble(item.Cells["PackSize"].Value) > 0.0)
				{
					double a = Conversion.ToDouble(item.Cells["Weight"].Value);
					if (!WagesAmountCalculateOnQty && a > RegularGrossWeight && Conversion.ToInt(cmbReferenceDocType.Value) != 118 && Conversion.ToInt(cmbReferenceDocType.Value) != 176)
					{
						item.Cells["Weight"].Value = Math.Round(RegularGrossWeight, 2);
						MessageBox.Show("Stiching Weight can not be Greater than Wages Gross Weight");
						a = Math.Round(RegularGrossWeight, 2);
					}
					double b = Conversion.ToDouble(item.Cells["PackSize"].Value);
					double Qty = a / b;
					item.Cells["Quantity"].Value = Math.Round(Qty, 2);
					if (WagesAmountCalculateOnQty && Qty > RegularTotalQty && Conversion.ToInt(cmbReferenceDocType.Value) != 118 && Conversion.ToInt(cmbReferenceDocType.Value) != 176)
					{
						MessageBox.Show("Stiching Total Qty can not be Greater than Wages total Qty");
						item.Cells["Quantity"].Value = Math.Round(RegularTotalQty, 2);
						Qty = Math.Round(RegularTotalQty, 2);
						double PackSize = Conversion.ToDouble(item.Cells["PackSize"].Value);
						item.Cells["Weight"].Value = Math.Round(Qty * PackSize, 2);
					}
					double num = Conversion.ToDouble(item.Cells["Weight"].Value);
					double WeightCutTotal = Conversion.ToDouble(item.Cells["WeightCut"].Value) * Qty;
					double BillWeight3 = num - Math.Abs(WeightCutTotal);
					if (!WagesAmountCalculateOnQty && BillWeight3 > RegularTotalWeight && Conversion.ToInt(cmbReferenceDocType.Value) != 118 && Conversion.ToInt(cmbReferenceDocType.Value) != 176)
					{
						item.Cells["Weight"].Value = Math.Round(RegularGrossWeight, 2);
						item.Cells["BillWeight"].Value = Math.Round(RegularTotalWeight, 2);
						MessageBox.Show("Stiching Total Weight can not be Greater than Wages total Weight");
					}
					else
					{
						item.Cells["BillWeight"].Value = Math.Round(BillWeight3, 2);
					}
				}
				if (e.Column.Key == "WagesId" || e.Column.Key == "SupplierId" || e.Column.Key == "Quantity" || e.Column.Key == "Weight" || e.Column.Key == "WeightCut" || e.Column.Key == "RateAddLess")
				{
					if (Conversion.ToString((object)item.Cells["WagesType"].Text) == "Free Of Cost" || Conversion.ToString((object)item.Cells["WarehouseType"].Text) == "Dryer")
					{
						item.Cells["Rate"].Value = 0;
						item.Cells["Amount"].Value = 0;
					}
					else
					{
						GetWagesSchedule d = CommonServices.GetWagesRate(Conversion.ToDateTime(item.Cells["Date"].Value), Conversion.ToDouble(item.Cells["PackSize"].Value), Conversion.ToInt(item.Cells["WagesId"].Value), Conversion.ToInt(item.Cells["SupplierId"].Value));
						if (d != null)
						{
							WagesRate = d.WagesRate;
						}
					}
				}
				if (!IsReferred)
				{
					if (WagesRate > 0.0)
					{
						item.Cells["Rate"].Value = WagesRate;
						if (Conversion.ToDouble(item.Cells["Rate"].Value) > 0.0 && Conversion.ToDouble(item.Cells["BillWeight"].Value) > 0.0)
						{
							double Quantity = Conversion.ToDouble(item.Cells["Quantity"].Value);
							double Packsize = Conversion.ToDouble(item.Cells["PackSize"].Value);
							double BillWeight = Conversion.ToDouble(item.Cells["BillWeight"].Value);
							double RateWithoutAddLess = Conversion.ToDouble(item.Cells["Rate"].Value);
							item.Cells["RateWithoutAddLess"].Value = RateWithoutAddLess;
							double RateAddLess = Conversion.ToDouble(item.Cells["RateAddLess"].Value);
							if (EnableAddLessOnWagesRegular && e.Column.Key == "RateAddLess")
							{
								if (PercentageForRateAddLess > 0.0)
								{
									double RateAddLessByConfig = PercentageForRateAddLess * RateWithoutAddLess / 100.0;
									bool RateAddlessInMinus = !(RateAddLess > 0.0);
									if (Math.Abs(RateAddLess) > RateAddLessByConfig)
									{
										RateAddLess = (RateAddlessInMinus ? Conversion.ToDouble((object)("-" + RateAddLessByConfig)) : RateAddLessByConfig);
										item.Cells["RateAddLess"].Value = RateAddLess;
										MessageBox.Show($"RateAddLess can not be grater than RateAdLess In config {RateAddLessByConfig}");
									}
								}
								else
								{
									RateAddLess = 0.0;
									item.Cells["RateAddLess"].Value = RateAddLess;
									MessageBox.Show("Please set RateAddLess Percentage in config first...");
								}
							}
							double Rate = RateWithoutAddLess + RateAddLess;
							item.Cells["Rate"].Value = Rate;
							double Amount = ((!WagesAmountCalculateOnQty) ? (BillWeight / Packsize * Rate) : (Quantity * Rate));
							item.Cells["Amount"].Value = Math.Round(Amount, 2);
						}
						else
						{
							item.Cells["Amount"].Value = 0;
						}
					}
					else
					{
						item.Cells["Rate"].Value = 0;
						item.Cells["RateWithoutAddLess"].Value = 0;
						item.Cells["Amount"].Value = 0;
					}
				}
				else if (WagesRate == PreviousWagesRate)
				{
					if (WagesRate > 0.0)
					{
						item.Cells["Rate"].Value = WagesRate;
						if (Conversion.ToDouble(item.Cells["Rate"].Value) > 0.0 && Conversion.ToDouble(item.Cells["BillWeight"].Value) > 0.0)
						{
							double Quantity2 = Conversion.ToDouble(item.Cells["Quantity"].Value);
							double Packsize2 = Conversion.ToDouble(item.Cells["PackSize"].Value);
							double BillWeight2 = Conversion.ToDouble(item.Cells["BillWeight"].Value);
							double RateWithoutAddLess2 = Conversion.ToDouble(item.Cells["Rate"].Value);
							item.Cells["RateWithoutAddLess"].Value = RateWithoutAddLess2;
							double RateAddLess2 = Conversion.ToDouble(item.Cells["RateAddLess"].Value);
							if (EnableAddLessOnWagesRegular && e.Column.Key == "RateAddLess")
							{
								if (PercentageForRateAddLess > 0.0)
								{
									double RateAddLessByConfig2 = PercentageForRateAddLess * RateWithoutAddLess2 / 100.0;
									bool RateAddlessInMinus2 = !(RateAddLess2 > 0.0);
									if (Math.Abs(RateAddLess2) > RateAddLessByConfig2)
									{
										RateAddLess2 = (RateAddlessInMinus2 ? Conversion.ToDouble((object)("-" + RateAddLessByConfig2)) : RateAddLessByConfig2);
										item.Cells["RateAddLess"].Value = RateAddLess2;
										MessageBox.Show($"RateAddLess can not be grater than RateAdLess In config {RateAddLessByConfig2}");
									}
								}
								else
								{
									RateAddLess2 = 0.0;
									item.Cells["RateAddLess"].Value = RateAddLess2;
									MessageBox.Show("Please set RateAddLess Percentage in config first...");
								}
							}
							double Rate2 = RateWithoutAddLess2 + RateAddLess2;
							item.Cells["Rate"].Value = Rate2;
							double Amount2 = ((!WagesAmountCalculateOnQty) ? (BillWeight2 / Packsize2 * Rate2) : (Quantity2 * Rate2));
							item.Cells["Amount"].Value = Math.Round(Amount2, 2);
						}
						else
						{
							item.Cells["Amount"].Value = 0;
						}
					}
					else
					{
						item.Cells["Rate"].Value = 0;
						item.Cells["RateWithoutAddLess"].Value = 0;
						item.Cells["Amount"].Value = 0;
					}
				}
				else
				{
					item.Cells["SupplierId"].Value = PreviousContractorId;
					MessageBox.Show("Contractor You are trying Change has diffrent rate than Previous So it Can not be Change");
				}
				item.EndEdit();
			}
			grdwagesDetail.UpdateData();
			CommonServices.GridAutoAdjustmentNew(grdwagesDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdwagesDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdwagesDetail.CurrentRow;
			if (item == null || (int)item.RowType != 0 || IsReferred)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				if (dtdetail.Rows.Count <= 1)
				{
					MessageBox.Show("You Can Not Delete All rows");
					return;
				}
				item.Delete();
			}
			if (e.Column.Key == "Add")
			{
				AddRowInGLGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInGLGrid()
	{
		try
		{
			double TotalWeight = Conversion.ToDouble((object)txtGrossWeight.Text.Trim());
			double TotalQty = Conversion.ToDouble((object)txtQty.Text.Trim());
			double Weight = 0.0;
			double WeightCut = 0.0;
			double BillWeight = 0.0;
			double Qty = 0.0;
			if (grdwagesDetail.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grdwagesDetail.GetRows();
				foreach (GridEXRow item in rows)
				{
					Weight += Conversion.ToDouble(item.Cells["Weight"].Value);
					WeightCut = Conversion.ToDouble(item.Cells["WeightCut"].Value);
					BillWeight += Conversion.ToDouble(item.Cells["BillWeight"].Value);
					Qty += Conversion.ToDouble(item.Cells["Quantity"].Value);
				}
			}
			double GrossWeight = TotalWeight - Weight;
			double GrossQty = TotalQty - Qty;
			if (GrossWeight > 0.0 || GrossQty > 0.0 || Conversion.ToInt(cmbReferenceDocType.Value) == 118 || Conversion.ToInt(cmbReferenceDocType.Value) == 176)
			{
				if (WagesAmountCalculateOnQty)
				{
					if (GrossQty > 0.0)
					{
						double ItemWeight = GrossQty * Conversion.ToDouble(grdwagesDetail.CurrentRow.Cells["PackSize"].Value);
						double BillQtyPartal = GrossQty;
						double Amount = BillQtyPartal * Conversion.ToDouble(grdwagesDetail.CurrentRow.Cells["Rate"].Value);
						dtdetail.Rows.Add(grdwagesDetail.CurrentRow.Cells["SupplierId"].Value, grdwagesDetail.CurrentRow.Cells["ContractorName"].Value, grdwagesDetail.CurrentRow.Cells["WagesId"].Value, grdwagesDetail.CurrentRow.Cells["WagesAccount"].Value, grdwagesDetail.CurrentRow.Cells["WagesType"].Value, grdwagesDetail.CurrentRow.Cells["Date"].Value, grdwagesDetail.CurrentRow.Cells["packingTypeId"].Value, grdwagesDetail.CurrentRow.Cells["packingType"].Value, Math.Round(ItemWeight, 2), grdwagesDetail.CurrentRow.Cells["PackSize"].Value, Math.Round(BillQtyPartal, 2), grdwagesDetail.CurrentRow.Cells["WeightCut"].Value, grdwagesDetail.CurrentRow.Cells["BillQty"].Value, ItemWeight, grdwagesDetail.CurrentRow.Cells["RateWithoutAddLess"].Value, grdwagesDetail.CurrentRow.Cells["RateAddLess"].Value, grdwagesDetail.CurrentRow.Cells["Rate"].Value, Math.Round(Amount, 2), grdwagesDetail.CurrentRow.Cells["ItemId"].Value, grdwagesDetail.CurrentRow.Cells["Item"].Value, grdwagesDetail.CurrentRow.Cells["jobLotId"].Value, grdwagesDetail.CurrentRow.Cells["jobLot"].Value, grdwagesDetail.CurrentRow.Cells["Crop"].Value, grdwagesDetail.CurrentRow.Cells["MoveFromId"].Value, grdwagesDetail.CurrentRow.Cells["MoveFrom"].Value, grdwagesDetail.CurrentRow.Cells["MoveToId"].Value, grdwagesDetail.CurrentRow.Cells["MoveTo"].Value, grdwagesDetail.CurrentRow.Cells["PurchaseGLAC"].Value, grdwagesDetail.CurrentRow.Cells["WarehouseType"].Value, true, grdwagesDetail.CurrentRow.Cells["JobOrderId"].Value, grdwagesDetail.CurrentRow.Cells["RefDocQty"].Value, grdwagesDetail.CurrentRow.Cells["RefDocWeight"].Value, grdwagesDetail.CurrentRow.Cells["RefLineId"].Value);
						grdwagesDetail.DataSource = dtdetail;
					}
					else if (Conversion.ToInt(cmbReferenceDocType.Value) == 118 || Conversion.ToInt(cmbReferenceDocType.Value) == 176)
					{
						dtdetail.Rows.Add(grdwagesDetail.CurrentRow.Cells["SupplierId"].Value, grdwagesDetail.CurrentRow.Cells["ContractorName"].Value, grdwagesDetail.CurrentRow.Cells["WagesId"].Value, grdwagesDetail.CurrentRow.Cells["WagesAccount"].Value, grdwagesDetail.CurrentRow.Cells["WagesType"].Value, grdwagesDetail.CurrentRow.Cells["Date"].Value, grdwagesDetail.CurrentRow.Cells["packingTypeId"].Value, grdwagesDetail.CurrentRow.Cells["packingType"].Value, grdwagesDetail.CurrentRow.Cells["Weight"].Value, grdwagesDetail.CurrentRow.Cells["PackSize"].Value, grdwagesDetail.CurrentRow.Cells["Quantity"].Value, grdwagesDetail.CurrentRow.Cells["WeightCut"].Value, grdwagesDetail.CurrentRow.Cells["BillQty"].Value, grdwagesDetail.CurrentRow.Cells["BillWeight"].Value, grdwagesDetail.CurrentRow.Cells["RateWithoutAddLess"].Value, grdwagesDetail.CurrentRow.Cells["RateAddLess"].Value, grdwagesDetail.CurrentRow.Cells["Rate"].Value, grdwagesDetail.CurrentRow.Cells["Amount"].Value, grdwagesDetail.CurrentRow.Cells["ItemId"].Value, grdwagesDetail.CurrentRow.Cells["Item"].Value, grdwagesDetail.CurrentRow.Cells["jobLotId"].Value, grdwagesDetail.CurrentRow.Cells["jobLot"].Value, grdwagesDetail.CurrentRow.Cells["Crop"].Value, grdwagesDetail.CurrentRow.Cells["MoveFromId"].Value, grdwagesDetail.CurrentRow.Cells["MoveFrom"].Value, grdwagesDetail.CurrentRow.Cells["MoveToId"].Value, grdwagesDetail.CurrentRow.Cells["MoveTo"].Value, grdwagesDetail.CurrentRow.Cells["PurchaseGLAC"].Value, grdwagesDetail.CurrentRow.Cells["WarehouseType"].Value, true, grdwagesDetail.CurrentRow.Cells["JobOrderId"].Value, grdwagesDetail.CurrentRow.Cells["RefDocQty"].Value, grdwagesDetail.CurrentRow.Cells["RefDocWeight"].Value, grdwagesDetail.CurrentRow.Cells["RefLineId"].Value);
						grdwagesDetail.DataSource = dtdetail;
					}
					else
					{
						MessageBox.Show("Please Check Grid Qty and TotalQty");
					}
				}
				else if (GrossWeight > 0.0)
				{
					double ItemQty = GrossWeight / Conversion.ToDouble(grdwagesDetail.CurrentRow.Cells["PackSize"].Value);
					double BillWeightPartal = GrossWeight - ItemQty * WeightCut;
					double Amount2 = BillWeightPartal / Conversion.ToDouble(grdwagesDetail.CurrentRow.Cells["PackSize"].Value) * Conversion.ToDouble(grdwagesDetail.CurrentRow.Cells["Rate"].Value);
					dtdetail.Rows.Add(grdwagesDetail.CurrentRow.Cells["SupplierId"].Value, grdwagesDetail.CurrentRow.Cells["ContractorName"].Value, grdwagesDetail.CurrentRow.Cells["WagesId"].Value, grdwagesDetail.CurrentRow.Cells["WagesAccount"].Value, grdwagesDetail.CurrentRow.Cells["WagesType"].Value, grdwagesDetail.CurrentRow.Cells["Date"].Value, grdwagesDetail.CurrentRow.Cells["packingTypeId"].Value, grdwagesDetail.CurrentRow.Cells["packingType"].Value, GrossWeight, grdwagesDetail.CurrentRow.Cells["PackSize"].Value, Math.Round(ItemQty, 2), grdwagesDetail.CurrentRow.Cells["WeightCut"].Value, grdwagesDetail.CurrentRow.Cells["BillQty"].Value, BillWeightPartal, grdwagesDetail.CurrentRow.Cells["RateWithoutAddLess"].Value, grdwagesDetail.CurrentRow.Cells["RateAddLess"].Value, grdwagesDetail.CurrentRow.Cells["Rate"].Value, Math.Round(Amount2, 2), grdwagesDetail.CurrentRow.Cells["ItemId"].Value, grdwagesDetail.CurrentRow.Cells["Item"].Value, grdwagesDetail.CurrentRow.Cells["jobLotId"].Value, grdwagesDetail.CurrentRow.Cells["jobLot"].Value, grdwagesDetail.CurrentRow.Cells["Crop"].Value, grdwagesDetail.CurrentRow.Cells["MoveFromId"].Value, grdwagesDetail.CurrentRow.Cells["MoveFrom"].Value, grdwagesDetail.CurrentRow.Cells["MoveToId"].Value, grdwagesDetail.CurrentRow.Cells["MoveTo"].Value, grdwagesDetail.CurrentRow.Cells["PurchaseGLAC"].Value, grdwagesDetail.CurrentRow.Cells["WarehouseType"].Value, true, grdwagesDetail.CurrentRow.Cells["JobOrderId"].Value, grdwagesDetail.CurrentRow.Cells["RefDocQty"].Value, grdwagesDetail.CurrentRow.Cells["RefDocWeight"].Value, grdwagesDetail.CurrentRow.Cells["RefLineId"].Value);
					grdwagesDetail.DataSource = dtdetail;
				}
				else if (Conversion.ToInt(cmbReferenceDocType.Value) == 118 || Conversion.ToInt(cmbReferenceDocType.Value) == 176)
				{
					dtdetail.Rows.Add(grdwagesDetail.CurrentRow.Cells["SupplierId"].Value, grdwagesDetail.CurrentRow.Cells["ContractorName"].Value, grdwagesDetail.CurrentRow.Cells["WagesId"].Value, grdwagesDetail.CurrentRow.Cells["WagesAccount"].Value, grdwagesDetail.CurrentRow.Cells["WagesType"].Value, grdwagesDetail.CurrentRow.Cells["Date"].Value, grdwagesDetail.CurrentRow.Cells["packingTypeId"].Value, grdwagesDetail.CurrentRow.Cells["packingType"].Value, grdwagesDetail.CurrentRow.Cells["Weight"].Value, grdwagesDetail.CurrentRow.Cells["PackSize"].Value, grdwagesDetail.CurrentRow.Cells["Quantity"].Value, grdwagesDetail.CurrentRow.Cells["WeightCut"].Value, grdwagesDetail.CurrentRow.Cells["BillQty"].Value, grdwagesDetail.CurrentRow.Cells["BillWeight"].Value, grdwagesDetail.CurrentRow.Cells["RateWithoutAddLess"].Value, grdwagesDetail.CurrentRow.Cells["RateAddLess"].Value, grdwagesDetail.CurrentRow.Cells["Rate"].Value, grdwagesDetail.CurrentRow.Cells["Amount"].Value, grdwagesDetail.CurrentRow.Cells["ItemId"].Value, grdwagesDetail.CurrentRow.Cells["Item"].Value, grdwagesDetail.CurrentRow.Cells["jobLotId"].Value, grdwagesDetail.CurrentRow.Cells["jobLot"].Value, grdwagesDetail.CurrentRow.Cells["Crop"].Value, grdwagesDetail.CurrentRow.Cells["MoveFromId"].Value, grdwagesDetail.CurrentRow.Cells["MoveFrom"].Value, grdwagesDetail.CurrentRow.Cells["MoveToId"].Value, grdwagesDetail.CurrentRow.Cells["MoveTo"].Value, grdwagesDetail.CurrentRow.Cells["PurchaseGLAC"].Value, grdwagesDetail.CurrentRow.Cells["WarehouseType"].Value, true, grdwagesDetail.CurrentRow.Cells["JobOrderId"].Value, grdwagesDetail.CurrentRow.Cells["RefDocQty"].Value, grdwagesDetail.CurrentRow.Cells["RefDocWeight"].Value, grdwagesDetail.CurrentRow.Cells["RefLineId"].Value);
					grdwagesDetail.DataSource = dtdetail;
				}
				else
				{
					MessageBox.Show("Please Check Grid GrossWeight and TotalGrossWeight");
				}
			}
			else
			{
				MessageBox.Show("Please Check Grid GrossWeight and TotalGrossWeight");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdwagesDetail_EditingCell(object sender, EditingCellEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdwagesDetail.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				PreviousContractorId = Conversion.ToInt(item.Cells["SupplierId"].Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInStichingGrid()
	{
		try
		{
			double TotalWeight = Conversion.ToDouble((object)txtGrossWeight.Text.Trim());
			double TotalQty = Conversion.ToDouble((object)txtQty.Text.Trim());
			double Weight = 0.0;
			double WeightCut = 0.0;
			double BillWeight = 0.0;
			double Qty = 0.0;
			if (grdStiching.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grdStiching.GetRows();
				foreach (GridEXRow item in rows)
				{
					Weight += Conversion.ToDouble(item.Cells["Weight"].Value);
					WeightCut = Conversion.ToDouble(item.Cells["WeightCut"].Value);
					BillWeight += Conversion.ToDouble(item.Cells["BillWeight"].Value);
					Qty += Conversion.ToDouble(item.Cells["Quantity"].Value);
				}
			}
			double GrossWeight = TotalWeight - Weight;
			double GrossQty = TotalQty - Qty;
			if (GrossWeight > 0.0 || GrossQty > 0.0 || Conversion.ToInt(cmbReferenceDocType.Value) == 118 || Conversion.ToInt(cmbReferenceDocType.Value) == 176)
			{
				if (WagesAmountCalculateOnQty)
				{
					if (GrossQty > 0.0)
					{
						double ItemWeight = GrossQty * Conversion.ToDouble(grdStiching.CurrentRow.Cells["PackSize"].Value);
						double BillQtyPartal = GrossQty;
						double Amount = BillQtyPartal * Conversion.ToDouble(grdStiching.CurrentRow.Cells["Rate"].Value);
						dtStiching.Rows.Add(grdStiching.CurrentRow.Cells["SupplierId"].Value, grdStiching.CurrentRow.Cells["ContractorName"].Value, grdStiching.CurrentRow.Cells["WagesId"].Value, grdStiching.CurrentRow.Cells["WagesAccount"].Value, grdwagesDetail.CurrentRow.Cells["WagesType"].Value, grdStiching.CurrentRow.Cells["Date"].Value, grdStiching.CurrentRow.Cells["packingTypeId"].Value, grdStiching.CurrentRow.Cells["packingType"].Value, ItemWeight, grdStiching.CurrentRow.Cells["PackSize"].Value, Math.Round(BillQtyPartal, 2), grdStiching.CurrentRow.Cells["WeightCut"].Value, grdStiching.CurrentRow.Cells["BillQty"].Value, ItemWeight, grdStiching.CurrentRow.Cells["RateWithoutAddLess"].Value, grdStiching.CurrentRow.Cells["RateAddLess"].Value, grdStiching.CurrentRow.Cells["Rate"].Value, Math.Round(Amount, 2), grdStiching.CurrentRow.Cells["ItemId"].Value, grdStiching.CurrentRow.Cells["Item"].Value, grdStiching.CurrentRow.Cells["jobLotId"].Value, grdStiching.CurrentRow.Cells["jobLot"].Value, grdStiching.CurrentRow.Cells["Crop"].Value, grdStiching.CurrentRow.Cells["MoveFromId"].Value, grdStiching.CurrentRow.Cells["MoveFrom"].Value, grdStiching.CurrentRow.Cells["MoveToId"].Value, grdStiching.CurrentRow.Cells["MoveTo"].Value, grdStiching.CurrentRow.Cells["PurchaseGLAC"].Value, grdStiching.CurrentRow.Cells["WarehouseType"].Value, true, grdStiching.CurrentRow.Cells["JobOrderId"].Value, grdStiching.CurrentRow.Cells["RefDocQty"].Value, grdStiching.CurrentRow.Cells["RefDocWeight"].Value, grdStiching.CurrentRow.Cells["RefLineId"].Value);
						grdStiching.DataSource = dtStiching;
					}
					else if (Conversion.ToInt(cmbReferenceDocType.Value) == 118 || Conversion.ToInt(cmbReferenceDocType.Value) == 176)
					{
						dtStiching.Rows.Add(grdStiching.CurrentRow.Cells["SupplierId"].Value, grdStiching.CurrentRow.Cells["ContractorName"].Value, grdStiching.CurrentRow.Cells["WagesId"].Value, grdStiching.CurrentRow.Cells["WagesAccount"].Value, grdwagesDetail.CurrentRow.Cells["WagesType"].Value, grdStiching.CurrentRow.Cells["Date"].Value, grdStiching.CurrentRow.Cells["packingTypeId"].Value, grdStiching.CurrentRow.Cells["packingType"].Value, grdStiching.CurrentRow.Cells["Weight"].Value, grdStiching.CurrentRow.Cells["PackSize"].Value, grdStiching.CurrentRow.Cells["Quantity"].Value, grdStiching.CurrentRow.Cells["WeightCut"].Value, grdStiching.CurrentRow.Cells["BillQty"].Value, grdStiching.CurrentRow.Cells["BillWeight"].Value, grdStiching.CurrentRow.Cells["RateWithoutAddLess"].Value, grdStiching.CurrentRow.Cells["RateAddLess"].Value, grdStiching.CurrentRow.Cells["Rate"].Value, grdStiching.CurrentRow.Cells["Amount"].Value, grdStiching.CurrentRow.Cells["ItemId"].Value, grdStiching.CurrentRow.Cells["Item"].Value, grdStiching.CurrentRow.Cells["jobLotId"].Value, grdStiching.CurrentRow.Cells["jobLot"].Value, grdStiching.CurrentRow.Cells["Crop"].Value, grdStiching.CurrentRow.Cells["MoveFromId"].Value, grdStiching.CurrentRow.Cells["MoveFrom"].Value, grdStiching.CurrentRow.Cells["MoveToId"].Value, grdStiching.CurrentRow.Cells["MoveTo"].Value, grdStiching.CurrentRow.Cells["PurchaseGLAC"].Value, grdStiching.CurrentRow.Cells["WarehouseType"].Value, true, grdStiching.CurrentRow.Cells["JobOrderId"].Value, grdStiching.CurrentRow.Cells["RefDocQty"].Value, grdStiching.CurrentRow.Cells["RefDocWeight"].Value, grdStiching.CurrentRow.Cells["RefLineId"].Value);
						grdStiching.DataSource = dtStiching;
					}
					else
					{
						MessageBox.Show("Please Check Grid Qty and TotalQty");
					}
				}
				else if (GrossWeight > 0.0)
				{
					double ItemQty = GrossWeight / Conversion.ToDouble(grdStiching.CurrentRow.Cells["PackSize"].Value);
					double BillWeightPartal = GrossWeight - ItemQty * WeightCut;
					double Amount2 = BillWeightPartal / Conversion.ToDouble(grdStiching.CurrentRow.Cells["PackSize"].Value) * Conversion.ToDouble(grdStiching.CurrentRow.Cells["Rate"].Value);
					dtStiching.Rows.Add(grdStiching.CurrentRow.Cells["SupplierId"].Value, grdStiching.CurrentRow.Cells["ContractorName"].Value, grdStiching.CurrentRow.Cells["WagesId"].Value, grdStiching.CurrentRow.Cells["WagesAccount"].Value, grdwagesDetail.CurrentRow.Cells["WagesType"].Value, grdStiching.CurrentRow.Cells["Date"].Value, grdStiching.CurrentRow.Cells["packingTypeId"].Value, grdStiching.CurrentRow.Cells["packingType"].Value, GrossWeight, grdStiching.CurrentRow.Cells["PackSize"].Value, Math.Round(ItemQty, 2), grdStiching.CurrentRow.Cells["WeightCut"].Value, grdStiching.CurrentRow.Cells["BillQty"].Value, BillWeightPartal, grdStiching.CurrentRow.Cells["RateWithoutAddLess"].Value, grdStiching.CurrentRow.Cells["RateAddLess"].Value, grdStiching.CurrentRow.Cells["Rate"].Value, Math.Round(Amount2, 2), grdStiching.CurrentRow.Cells["ItemId"].Value, grdStiching.CurrentRow.Cells["Item"].Value, grdStiching.CurrentRow.Cells["jobLotId"].Value, grdStiching.CurrentRow.Cells["jobLot"].Value, grdStiching.CurrentRow.Cells["Crop"].Value, grdStiching.CurrentRow.Cells["MoveFromId"].Value, grdStiching.CurrentRow.Cells["MoveFrom"].Value, grdStiching.CurrentRow.Cells["MoveToId"].Value, grdStiching.CurrentRow.Cells["MoveTo"].Value, grdStiching.CurrentRow.Cells["PurchaseGLAC"].Value, grdStiching.CurrentRow.Cells["WarehouseType"].Value, true, grdStiching.CurrentRow.Cells["JobOrderId"].Value, grdStiching.CurrentRow.Cells["RefDocQty"].Value, grdStiching.CurrentRow.Cells["RefDocWeight"].Value, grdStiching.CurrentRow.Cells["RefLineId"].Value);
					grdStiching.DataSource = dtStiching;
				}
				else if (Conversion.ToInt(cmbReferenceDocType.Value) == 118 || Conversion.ToInt(cmbReferenceDocType.Value) == 176)
				{
					dtStiching.Rows.Add(grdStiching.CurrentRow.Cells["SupplierId"].Value, grdStiching.CurrentRow.Cells["ContractorName"].Value, grdStiching.CurrentRow.Cells["WagesId"].Value, grdStiching.CurrentRow.Cells["WagesAccount"].Value, grdwagesDetail.CurrentRow.Cells["WagesType"].Value, grdStiching.CurrentRow.Cells["Date"].Value, grdStiching.CurrentRow.Cells["packingTypeId"].Value, grdStiching.CurrentRow.Cells["packingType"].Value, grdStiching.CurrentRow.Cells["Weight"].Value, grdStiching.CurrentRow.Cells["PackSize"].Value, grdStiching.CurrentRow.Cells["Quantity"].Value, grdStiching.CurrentRow.Cells["WeightCut"].Value, grdStiching.CurrentRow.Cells["BillQty"].Value, grdStiching.CurrentRow.Cells["BillWeight"].Value, grdStiching.CurrentRow.Cells["RateWithoutAddLess"].Value, grdStiching.CurrentRow.Cells["RateAddLess"].Value, grdStiching.CurrentRow.Cells["Rate"].Value, grdStiching.CurrentRow.Cells["Amount"].Value, grdStiching.CurrentRow.Cells["ItemId"].Value, grdStiching.CurrentRow.Cells["Item"].Value, grdStiching.CurrentRow.Cells["jobLotId"].Value, grdStiching.CurrentRow.Cells["jobLot"].Value, grdStiching.CurrentRow.Cells["Crop"].Value, grdStiching.CurrentRow.Cells["MoveFromId"].Value, grdStiching.CurrentRow.Cells["MoveFrom"].Value, grdStiching.CurrentRow.Cells["MoveToId"].Value, grdStiching.CurrentRow.Cells["MoveTo"].Value, grdStiching.CurrentRow.Cells["PurchaseGLAC"].Value, grdStiching.CurrentRow.Cells["WarehouseType"].Value, true, grdStiching.CurrentRow.Cells["JobOrderId"].Value, grdStiching.CurrentRow.Cells["RefDocQty"].Value, grdStiching.CurrentRow.Cells["RefDocWeight"].Value, grdStiching.CurrentRow.Cells["RefLineId"].Value);
					grdStiching.DataSource = dtStiching;
				}
				else
				{
					MessageBox.Show("Please Check Grid GrossWeight and TotalGrossWeight");
				}
			}
			else
			{
				MessageBox.Show("Please Check Grid GrossWeight and TotalGrossWeight");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdStiching_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdStiching.CurrentRow;
			if (item == null || (int)item.RowType != 0 || IsReferred)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				if (dtStiching.Rows.Count <= 1)
				{
					MessageBox.Show("You Can Not Delete All rows....");
					return;
				}
				item.Delete();
			}
			if (e.Column.Key == "Add")
			{
				AddRowInStichingGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdStiching_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grdStiching.UpdateData();
			GridEXRow selectedRow = grdStiching.CurrentRow;
			double AllQtyExceptSelected = (from GridEXRow row in grdStiching.GetRows()
				where row != selectedRow
				select (double)row.Cells["Quantity"].Value).Sum();
			double AllNetWeightExceptSelected = (from GridEXRow row in grdStiching.GetRows()
				where row != selectedRow
				select (double)row.Cells["BillWeight"].Value).Sum();
			double AllGrossWeightExceptSelected = (from GridEXRow row in grdStiching.GetRows()
				where row != selectedRow
				select (double)row.Cells["Weight"].Value).Sum();
			double RegularTotalWeight = Conversion.ToDouble((object)txtGrossWeight.Text) - AllNetWeightExceptSelected;
			double RegularGrossWeight = Conversion.ToDouble((object)txtGrossWeight.Text) - AllGrossWeightExceptSelected;
			double RegularTotalQty = Conversion.ToDouble((object)txtQty.Text) - AllQtyExceptSelected;
			GridEXRow item = grdStiching.CurrentRow;
			if (e.Column.Key == "ContractorName" || e.Column.Key == "WagesId" || e.Column.Key == "WagesAccount")
			{
				IsRegularOrFreeOfCost = CommonServices.CheckItemsFreeofcostforWages(Conversion.ToDateTime(item.Cells["Date"].Value), Conversion.ToInt(cmbReferenceDocType.Value), Conversion.ToInt(item.Cells["ItemId"].Value), Conversion.ToInt(item.Cells["WagesId"].Value));
				if (IsRegularOrFreeOfCost)
				{
					item.Cells["Wagestype"].Value = "Free Of Cost";
				}
				else
				{
					item.Cells["Wagestype"].Value = "Regular";
				}
			}
			if (e.Column.Key == "WagesId" || e.Column.Key == "SupplierId" || e.Column.Key == "Quantity" || e.Column.Key == "Weight" || e.Column.Key == "WeightCut" || e.Column.Key == "RateAddLess")
			{
				if (Conversion.ToString((object)item.Cells["WagesType"].Text) == "Free Of Cost" || Conversion.ToString(item.Cells["WarehouseType"].Value) == "Dryer")
				{
					item.Cells["Rate"].Value = 0;
					item.Cells["Amount"].Value = 0;
				}
				else
				{
					double PreviousWagesRate = Conversion.ToDouble(item.Cells["Rate"].Value);
					double WagesRate = 0.0;
					GetWagesSchedule d = CommonServices.GetWagesRate(Conversion.ToDateTime(item.Cells["Date"].Value), Conversion.ToDouble(item.Cells["PackSize"].Value), Conversion.ToInt(item.Cells["WagesId"].Value), Conversion.ToInt(item.Cells["SupplierId"].Value));
					if (d != null)
					{
						WagesRate = d.WagesRate;
					}
					if (!IsReferred)
					{
						if (WagesRate > 0.0)
						{
							item.Cells["Rate"].Value = WagesRate;
							if (Conversion.ToDouble(item.Cells["Weight"].Value) > 0.0 && Conversion.ToDouble(item.Cells["PackSize"].Value) > 0.0)
							{
								double num = Conversion.ToDouble(item.Cells["Weight"].Value);
								double b2 = Conversion.ToDouble(item.Cells["PackSize"].Value);
								_ = num / b2;
								double Weight3 = Conversion.ToDouble(item.Cells["Weight"].Value);
								if (!WagesAmountCalculateOnQty && Weight3 > RegularTotalWeight && Conversion.ToInt(cmbReferenceDocType.Value) != 118 && Conversion.ToInt(cmbReferenceDocType.Value) != 176)
								{
									MessageBox.Show("Stiching Total Weight can not be Greater than Wages total Weight");
									item.Cells["BillWeight"].Value = Math.Round(RegularTotalWeight, 2);
								}
								else
								{
									item.Cells["BillWeight"].Value = Math.Round(Weight3, 2);
								}
							}
							if (Conversion.ToDouble(item.Cells["Quantity"].Value) > 0.0 && Conversion.ToDouble(item.Cells["WeightCut"].Value) > 0.0 && Conversion.ToDouble(item.Cells["PackSize"].Value) > 0.0 && Conversion.ToDouble(item.Cells["Weight"].Value) > 0.0)
							{
								double num2 = Conversion.ToDouble(item.Cells["WeightCut"].Value);
								double Qty5 = Conversion.ToDouble(item.Cells["Quantity"].Value);
								double WeightCut4 = num2 * Qty5;
								double BillWeight4 = Conversion.ToDouble(item.Cells["Weight"].Value) - Math.Abs(WeightCut4);
								if (!WagesAmountCalculateOnQty && BillWeight4 > RegularTotalWeight && Conversion.ToInt(cmbReferenceDocType.Value) != 118 && Conversion.ToInt(cmbReferenceDocType.Value) != 176)
								{
									MessageBox.Show("Stiching Total Weight can not be Greater than Wages total Weight");
									item.Cells["BillWeight"].Value = Math.Round(RegularTotalWeight, 2);
									item.Cells["Weight"].Value = Math.Round(RegularGrossWeight, 2);
								}
								else
								{
									item.Cells["BillWeight"].Value = Math.Round(BillWeight4, 2);
								}
								if (WagesAmountCalculateOnQty && Qty5 > RegularTotalQty && Conversion.ToInt(cmbReferenceDocType.Value) != 118 && Conversion.ToInt(cmbReferenceDocType.Value) != 176)
								{
									MessageBox.Show("Stiching Total Qty can not be Greater than Wages total Qty");
									item.Cells["Quantity"].Value = Math.Round(RegularTotalQty, 2);
								}
								else
								{
									item.Cells["Quantity"].Value = Math.Round(Qty5, 2);
								}
							}
							if (Conversion.ToDouble(item.Cells["Rate"].Value) > 0.0 && Conversion.ToDouble(item.Cells["BillWeight"].Value) > 0.0)
							{
								double Packsize3 = Conversion.ToDouble(item.Cells["PackSize"].Value);
								double num3 = Conversion.ToDouble(item.Cells["BillWeight"].Value);
								double RateWithoutAddLess3 = Conversion.ToDouble(item.Cells["Rate"].Value);
								item.Cells["RateWithoutAddLess"].Value = RateWithoutAddLess3;
								double RateAddLess3 = Conversion.ToDouble(item.Cells["RateAddLess"].Value);
								if (EnableAddLessOnWagesRegular && e.Column.Key == "RateAddLess")
								{
									if (PercentageForRateAddLess > 0.0)
									{
										double RateAddLessByConfig3 = PercentageForRateAddLess * RateWithoutAddLess3 / 100.0;
										bool RateAddlessInMinus3 = !(RateAddLess3 > 0.0);
										if (Math.Abs(RateAddLess3) > RateAddLessByConfig3)
										{
											RateAddLess3 = (RateAddlessInMinus3 ? Conversion.ToDouble((object)("-" + RateAddLessByConfig3)) : RateAddLessByConfig3);
											item.Cells["RateAddLess"].Value = RateAddLess3;
											MessageBox.Show($"RateAddLess can not be grater than RateAdLess In config {RateAddLessByConfig3}");
										}
									}
									else
									{
										RateAddLess3 = 0.0;
										item.Cells["RateAddLess"].Value = RateAddLess3;
										MessageBox.Show("Please set RateAddLess Percentage in config first...");
									}
								}
								double Rate3 = RateWithoutAddLess3 + RateAddLess3;
								item.Cells["Rate"].Value = Rate3;
								double Amount3 = num3 / Packsize3 * Rate3;
								item.Cells["Amount"].Value = Math.Round(Amount3, 2);
							}
							else
							{
								item.Cells["Amount"].Value = 0;
							}
						}
						else
						{
							item.Cells["Rate"].Value = 0;
						}
					}
					else if (WagesRate == PreviousWagesRate)
					{
						if (WagesRate > 0.0)
						{
							item.Cells["Rate"].Value = WagesRate;
							if (Conversion.ToDouble(item.Cells["Weight"].Value) > 0.0 && Conversion.ToDouble(item.Cells["PackSize"].Value) > 0.0)
							{
								double num4 = Conversion.ToDouble(item.Cells["Weight"].Value);
								double b3 = Conversion.ToDouble(item.Cells["PackSize"].Value);
								_ = num4 / b3;
								double Weight4 = Conversion.ToDouble(item.Cells["Weight"].Value);
								if (!WagesAmountCalculateOnQty && Weight4 > RegularTotalWeight && Conversion.ToInt(cmbReferenceDocType.Value) != 118 && Conversion.ToInt(cmbReferenceDocType.Value) != 176)
								{
									MessageBox.Show("Stiching Total Weight can not be Greater than Wages total Weight");
									item.Cells["BillWeight"].Value = Math.Round(RegularTotalWeight, 2);
								}
								else
								{
									item.Cells["BillWeight"].Value = Math.Round(Weight4, 2);
								}
							}
							if (Conversion.ToDouble(item.Cells["Quantity"].Value) > 0.0 && Conversion.ToDouble(item.Cells["WeightCut"].Value) > 0.0 && Conversion.ToDouble(item.Cells["PackSize"].Value) > 0.0 && Conversion.ToDouble(item.Cells["Weight"].Value) > 0.0)
							{
								double num5 = Conversion.ToDouble(item.Cells["WeightCut"].Value);
								double Qty6 = Conversion.ToDouble(item.Cells["Quantity"].Value);
								double WeightCut5 = num5 * Qty6;
								double BillWeight5 = Conversion.ToDouble(item.Cells["Weight"].Value) - Math.Abs(WeightCut5);
								if (!WagesAmountCalculateOnQty && BillWeight5 > RegularTotalWeight && Conversion.ToInt(cmbReferenceDocType.Value) != 118 && Conversion.ToInt(cmbReferenceDocType.Value) != 176)
								{
									MessageBox.Show("Stiching Total Weight can not be Greater than Wages total Weight");
									item.Cells["BillWeight"].Value = Math.Round(RegularTotalWeight, 2);
									item.Cells["Weight"].Value = Math.Round(RegularGrossWeight, 2);
								}
								else
								{
									item.Cells["BillWeight"].Value = Math.Round(BillWeight5, 2);
								}
								if (WagesAmountCalculateOnQty && Qty6 > RegularTotalQty && Conversion.ToInt(cmbReferenceDocType.Value) != 118 && Conversion.ToInt(cmbReferenceDocType.Value) != 176)
								{
									MessageBox.Show("Stiching Total Qty can not be Greater than Wages total Qty");
									item.Cells["Quantity"].Value = Math.Round(RegularTotalQty, 2);
								}
								else
								{
									item.Cells["Quantity"].Value = Math.Round(Qty6, 2);
								}
							}
							if (Conversion.ToDouble(item.Cells["Rate"].Value) > 0.0 && Conversion.ToDouble(item.Cells["BillWeight"].Value) > 0.0)
							{
								double Packsize4 = Conversion.ToDouble(item.Cells["PackSize"].Value);
								double num6 = Conversion.ToDouble(item.Cells["BillWeight"].Value);
								double RateWithoutAddLess4 = Conversion.ToDouble(item.Cells["Rate"].Value);
								item.Cells["RateWithoutAddLess"].Value = RateWithoutAddLess4;
								double RateAddLess4 = Conversion.ToDouble(item.Cells["RateAddLess"].Value);
								if (EnableAddLessOnWagesRegular && e.Column.Key == "RateAddLess")
								{
									if (PercentageForRateAddLess > 0.0)
									{
										double RateAddLessByConfig4 = PercentageForRateAddLess * RateWithoutAddLess4 / 100.0;
										bool RateAddlessInMinus4 = !(RateAddLess4 > 0.0);
										if (Math.Abs(RateAddLess4) > RateAddLessByConfig4)
										{
											RateAddLess4 = (RateAddlessInMinus4 ? Conversion.ToDouble((object)("-" + RateAddLessByConfig4)) : RateAddLessByConfig4);
											item.Cells["RateAddLess"].Value = RateAddLess4;
											MessageBox.Show($"RateAddLess can not be grater than RateAdLess In config {RateAddLessByConfig4}");
										}
									}
									else
									{
										RateAddLess4 = 0.0;
										item.Cells["RateAddLess"].Value = RateAddLess4;
										MessageBox.Show("Please set RateAddLess Percentage in config first...");
									}
								}
								double Rate4 = RateWithoutAddLess4 + RateAddLess4;
								item.Cells["Rate"].Value = Rate4;
								double Amount4 = num6 / Packsize4 * Rate4;
								item.Cells["Amount"].Value = Math.Round(Amount4, 2);
							}
							else
							{
								item.Cells["Amount"].Value = 0;
							}
						}
						else
						{
							item.Cells["Rate"].Value = 0;
							item.Cells["RateWithoutAddLess"].Value = 0;
						}
					}
					else
					{
						item.Cells["SupplierId"].Value = PreviousContractorIdStiching;
						MessageBox.Show("Contractor You are trying Change has diffrent rate than Previous So it Can not be Change");
					}
				}
			}
			if (e.Column.Key == "Weight" || e.Column.Key == "WeightCut")
			{
				if (Conversion.ToDouble(item.Cells["Weight"].Value) > 0.0 && Conversion.ToDouble(item.Cells["PackSize"].Value) > 0.0)
				{
					double num7 = Conversion.ToDouble(item.Cells["Weight"].Value);
					double b = Conversion.ToDouble(item.Cells["PackSize"].Value);
					double Qty4 = num7 / b;
					double Weight2 = Conversion.ToDouble(item.Cells["Weight"].Value);
					if (!WagesAmountCalculateOnQty && Weight2 > RegularTotalWeight && Conversion.ToInt(cmbReferenceDocType.Value) != 118 && Conversion.ToInt(cmbReferenceDocType.Value) != 176)
					{
						MessageBox.Show("Stiching Total Weight can not be Greater than Wages total Weight");
						item.Cells["BillWeight"].Value = Math.Round(RegularTotalWeight, 2);
						item.Cells["Weight"].Value = Math.Round(RegularGrossWeight, 2);
					}
					else
					{
						item.Cells["BillWeight"].Value = Math.Round(Weight2, 2);
					}
					if (WagesAmountCalculateOnQty && Qty4 > RegularTotalQty && Conversion.ToInt(cmbReferenceDocType.Value) != 118 && Conversion.ToInt(cmbReferenceDocType.Value) != 176)
					{
						MessageBox.Show("Stiching Total Qty can not be Greater than Wages total Qty");
						item.Cells["Quantity"].Value = Math.Round(RegularTotalQty, 2);
					}
					else
					{
						item.Cells["Quantity"].Value = Math.Round(Qty4, 2);
					}
				}
				if (Conversion.ToDouble(item.Cells["Quantity"].Value) > 0.0 && Conversion.ToDouble(item.Cells["WeightCut"].Value) > 0.0 && Conversion.ToDouble(item.Cells["PackSize"].Value) > 0.0 && Conversion.ToDouble(item.Cells["Weight"].Value) > 0.0)
				{
					double num8 = Conversion.ToDouble(item.Cells["WeightCut"].Value);
					double Qty3 = Conversion.ToDouble(item.Cells["Quantity"].Value);
					double WeightCut3 = num8 * Qty3;
					double BillWeight3 = Conversion.ToDouble(item.Cells["Weight"].Value) - Math.Abs(WeightCut3);
					if (!WagesAmountCalculateOnQty && BillWeight3 > RegularTotalWeight && Conversion.ToInt(cmbReferenceDocType.Value) != 118 && Conversion.ToInt(cmbReferenceDocType.Value) != 176)
					{
						MessageBox.Show("Stiching Total Weight can not be Greater than Wages total Weight");
						item.Cells["BillWeight"].Value = Math.Round(RegularTotalWeight, 2);
						item.Cells["Weight"].Value = Math.Round(RegularGrossWeight, 2);
					}
					else
					{
						item.Cells["BillWeight"].Value = Math.Round(BillWeight3, 2);
					}
					if (WagesAmountCalculateOnQty && Qty3 > RegularTotalQty && Conversion.ToInt(cmbReferenceDocType.Value) != 118 && Conversion.ToInt(cmbReferenceDocType.Value) != 176)
					{
						MessageBox.Show("Stiching Total Qty can not be Greater than Wages total Qty");
						item.Cells["Quantity"].Value = Math.Round(RegularTotalQty, 2);
					}
					else
					{
						item.Cells["Quantity"].Value = Math.Round(Qty3, 2);
					}
				}
				if (Conversion.ToDouble(item.Cells["Rate"].Value) > 0.0 && Conversion.ToDouble(item.Cells["BillWeight"].Value) > 0.0)
				{
					double Packsize2 = Conversion.ToDouble(item.Cells["PackSize"].Value);
					double num9 = Conversion.ToDouble(item.Cells["BillWeight"].Value);
					double RateWithoutAddLess2 = Conversion.ToDouble(item.Cells["Rate"].Value);
					item.Cells["RateWithoutAddLess"].Value = RateWithoutAddLess2;
					double RateAddLess2 = Conversion.ToDouble(item.Cells["RateAddLess"].Value);
					if (EnableAddLessOnWagesRegular && e.Column.Key == "RateAddLess")
					{
						if (PercentageForRateAddLess > 0.0)
						{
							double RateAddLessByConfig2 = PercentageForRateAddLess * RateWithoutAddLess2 / 100.0;
							bool RateAddlessInMinus2 = !(RateAddLess2 > 0.0);
							if (Math.Abs(RateAddLess2) > RateAddLessByConfig2)
							{
								RateAddLess2 = (RateAddlessInMinus2 ? Conversion.ToDouble((object)("-" + RateAddLessByConfig2)) : RateAddLessByConfig2);
								item.Cells["RateAddLess"].Value = RateAddLess2;
								MessageBox.Show($"RateAddLess can not be grater than RateAdLess In config {RateAddLessByConfig2}");
							}
						}
						else
						{
							RateAddLess2 = 0.0;
							item.Cells["RateAddLess"].Value = RateAddLess2;
							MessageBox.Show("Please set RateAddLess Percentage in config first...");
						}
					}
					double Rate2 = RateWithoutAddLess2 + RateAddLess2;
					item.Cells["Rate"].Value = Rate2;
					double Amount2 = num9 / Packsize2 * Rate2;
					item.Cells["Amount"].Value = Math.Round(Amount2, 2);
				}
				else
				{
					item.Cells["Amount"].Value = 0;
				}
			}
			if (e.Column.Key == "Quantity" || e.Column.Key == "WeightCut")
			{
				if (Conversion.ToDouble(item.Cells["Quantity"].Value) > 0.0 && Conversion.ToDouble(item.Cells["PackSize"].Value) > 0.0 && Conversion.ToDouble(item.Cells["Weight"].Value) > 0.0)
				{
					double WeightCut2 = Conversion.ToDouble(item.Cells["WeightCut"].Value);
					double PackSize = Conversion.ToDouble(item.Cells["PackSize"].Value);
					double Qty2 = Conversion.ToDouble(item.Cells["Quantity"].Value);
					double Weight = Qty2 * PackSize;
					double WeightCutTotal = Qty2 * Math.Abs(WeightCut2);
					double BillWeight2 = Weight - Math.Abs(WeightCutTotal);
					if (!WagesAmountCalculateOnQty && BillWeight2 > RegularTotalWeight)
					{
						MessageBox.Show("Stiching Total Weight can not be Greater than Wages total Weight");
						item.Cells["BillWeight"].Value = Math.Round(RegularTotalWeight, 2);
						item.Cells["Weight"].Value = Math.Round(RegularGrossWeight, 2);
					}
					else
					{
						item.Cells["Weight"].Value = Weight.ToString();
						item.Cells["BillWeight"].Value = Math.Round(BillWeight2, 2);
					}
					if (WagesAmountCalculateOnQty && Qty2 > RegularTotalQty)
					{
						MessageBox.Show("Stiching Total Qty can not be Greater than Wages total Qty");
						item.Cells["Quantity"].Value = Math.Round(RegularTotalQty, 2);
					}
					else
					{
						item.Cells["Quantity"].Value = Math.Round(Qty2, 2);
					}
				}
				if (Conversion.ToDouble(item.Cells["Quantity"].Value) > 0.0 && Conversion.ToDouble(item.Cells["WeightCut"].Value) > 0.0 && Conversion.ToDouble(item.Cells["PackSize"].Value) > 0.0 && Conversion.ToDouble(item.Cells["Weight"].Value) > 0.0)
				{
					double num10 = Conversion.ToDouble(item.Cells["WeightCut"].Value);
					double Qty = Conversion.ToDouble(item.Cells["Quantity"].Value);
					double WeightCut = num10 * Qty;
					double BillWeight = Conversion.ToDouble(item.Cells["Weight"].Value) - Math.Abs(WeightCut);
					if (!WagesAmountCalculateOnQty && BillWeight > RegularTotalWeight)
					{
						MessageBox.Show("Stiching Total Weight can not be Greater than Wages total Weight");
						item.Cells["BillWeight"].Value = Math.Round(RegularTotalWeight, 2);
						item.Cells["Weight"].Value = Math.Round(RegularGrossWeight, 2);
					}
					else
					{
						item.Cells["BillWeight"].Value = Math.Round(BillWeight, 2);
					}
				}
				if (Conversion.ToDouble(item.Cells["Rate"].Value) > 0.0 && Conversion.ToDouble(item.Cells["BillWeight"].Value) > 0.0)
				{
					double Packsize = Conversion.ToDouble(item.Cells["PackSize"].Value);
					double num11 = Conversion.ToDouble(item.Cells["BillWeight"].Value);
					double RateWithoutAddLess = Conversion.ToDouble(item.Cells["Rate"].Value);
					double RateAddLess = Conversion.ToDouble(item.Cells["RateAddLess"].Value);
					if (EnableAddLessOnWagesRegular && e.Column.Key == "RateAddLess")
					{
						if (PercentageForRateAddLess > 0.0)
						{
							double RateAddLessByConfig = PercentageForRateAddLess * RateWithoutAddLess / 100.0;
							bool RateAddlessInMinus = !(RateAddLess > 0.0);
							if (Math.Abs(RateAddLess) > RateAddLessByConfig)
							{
								RateAddLess = (RateAddlessInMinus ? Conversion.ToDouble((object)("-" + RateAddLessByConfig)) : RateAddLessByConfig);
								item.Cells["RateAddLess"].Value = RateAddLess;
								MessageBox.Show($"RateAddLess can not be grater than RateAdLess In config {RateAddLessByConfig}");
							}
						}
						else
						{
							RateAddLess = 0.0;
							item.Cells["RateAddLess"].Value = RateAddLess;
							MessageBox.Show("Please set RateAddLess Percentage in config first...");
						}
					}
					double Rate = RateWithoutAddLess + RateAddLess;
					item.Cells["Rate"].Value = Rate;
					double Amount = num11 / Packsize * Rate;
					item.Cells["Amount"].Value = Math.Round(Amount, 2);
				}
				else
				{
					item.Cells["Amount"].Value = 0;
				}
			}
			grdStiching.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridStichingLockEditing()
	{
		try
		{
			if (grdStiching.RowCount > 0)
			{
				if (!IsReferred && grdStiching.RowCount > 0)
				{
					grdStiching.RootTable.Columns["PurchaseGLAC"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["WagesType"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["ContractorName"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["WagesAccount"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["ItemId"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["jobLotId"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["packingTypeId"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["MoveFromId"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["MoveToId"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["Date"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["packingType"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["jobLot"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["Item"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["MoveFrom"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["MoveTo"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["Crop"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["PackSize"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["Rate"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["BillWeight"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["Amount"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["WagesType"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["WarehouseType"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["SupplierId"].EditType = (EditType)4;
					grdStiching.RootTable.Columns["WagesId"].EditType = (EditType)4;
				}
				else
				{
					grdStiching.RootTable.Columns["PurchaseGLAC"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["WagesType"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["ContractorName"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["WagesAccount"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["ItemId"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["jobLotId"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["packingTypeId"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["MoveFromId"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["MoveToId"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["Date"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["packingType"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["jobLot"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["Item"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["MoveFrom"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["MoveTo"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["Crop"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["PackSize"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["Rate"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["BillWeight"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["Amount"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["WagesType"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["WarehouseType"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["SupplierId"].EditType = (EditType)4;
					grdStiching.RootTable.Columns["WagesId"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["Weight"].EditType = (EditType)0;
					grdStiching.RootTable.Columns["Quantity"].EditType = (EditType)0;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void GridStichingSettings()
	{
		//IL_0a96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac2: Expected O, but got Unknown
		try
		{
			grdStiching.RootTable.Columns["PurchaseGLAC"].Visible = false;
			grdStiching.RootTable.Columns["JobOrderId"].Visible = false;
			grdStiching.RootTable.Columns["WarehouseType"].Visible = false;
			grdStiching.RootTable.Columns["ContractorName"].Visible = false;
			grdStiching.RootTable.Columns["WagesAccount"].Visible = false;
			grdStiching.RootTable.Columns["ItemId"].Visible = false;
			grdStiching.RootTable.Columns["jobLotId"].Visible = false;
			grdStiching.RootTable.Columns["packingTypeId"].Visible = false;
			grdStiching.RootTable.Columns["MoveFromId"].Visible = false;
			grdStiching.RootTable.Columns["MoveToId"].Visible = false;
			grdStiching.RootTable.Columns["BillQty"].Visible = false;
			grdStiching.RootTable.Columns["RefDocWeight"].Visible = false;
			grdStiching.RootTable.Columns["RefDocQty"].Visible = false;
			grdStiching.RootTable.Columns["RateAddLess"].Visible = EnableAddLessOnWagesRegular;
			grdStiching.RootTable.Columns["RateWithoutAddLess"].Visible = EnableAddLessOnWagesRegular;
			grdStiching.RootTable.Columns["MoveTo"].Visible = Conversion.ToInt(cmbReferenceDocType.Value) == 220;
			grdStiching.RootTable.Columns["PurchaseGLAC"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["WagesType"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["ContractorName"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["WagesAccount"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["jobLotId"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["packingTypeId"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["MoveFromId"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["MoveToId"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["JobOrderId"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["RefLineId"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["RefDocQty"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["RefDocWeight"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["RefLineId"].Visible = Conversion.ToInt(cmbReferenceDocType.Value) == 112 || Conversion.ToInt(cmbReferenceDocType.Value) == 66;
			grdStiching.RootTable.Columns["RefLineId"].Caption = "RowNo";
			grdStiching.RootTable.Columns["RateAddLess"].EditType = (EditType)(EnableAddLessOnWagesRegular ? 1 : 0);
			grdStiching.RootTable.Columns["RateWithoutAddLess"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["Date"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["packingType"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["jobLot"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["Item"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["MoveFrom"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["MoveTo"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["Crop"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["PackSize"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["Rate"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["BillWeight"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["WagesType"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["WarehouseType"].EditType = (EditType)0;
			grdStiching.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdStiching.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdStiching.RootTable.Columns["Quantity"].AggregateFunction = (AggregateFunction)2;
			grdStiching.RootTable.Columns["BillWeight"].AggregateFunction = (AggregateFunction)2;
			grdStiching.RootTable.Columns["BillQty"].AggregateFunction = (AggregateFunction)2;
			grdStiching.RootTable.Columns["SupplierId"].ColumnType = (ColumnType)0;
			grdStiching.RootTable.Columns["SupplierId"].HasValueList = true;
			grdStiching.RootTable.Columns["SupplierId"].LimitToList = true;
			grdStiching.RootTable.Columns["SupplierId"].EditType = (EditType)4;
			grdStiching.RootTable.Columns["SupplierId"].Caption = "Contractor Name";
			grdStiching.RootTable.Columns["SupplierId"].ValueList.PopulateValueList((IEnumerable)dtcus.DefaultView, "Id", "CompanyName");
			grdStiching.RootTable.Columns["SupplierId"].Width = 200;
			grdStiching.RootTable.Columns["WagesId"].ColumnType = (ColumnType)0;
			grdStiching.RootTable.Columns["WagesId"].HasValueList = true;
			grdStiching.RootTable.Columns["WagesId"].LimitToList = true;
			grdStiching.RootTable.Columns["WagesId"].EditType = (EditType)4;
			grdStiching.RootTable.Columns["WagesId"].Caption = "Wages Account";
			grdStiching.RootTable.Columns["WagesId"].ValueList.PopulateValueList((IEnumerable)dtStichingItems.DefaultView, "Id", "WagesAccountName");
			grdStiching.RootTable.Columns["WagesId"].Width = 200;
			if (!IsReferred)
			{
				grdStiching.RootTable.Columns.Add("Add");
				grdStiching.RootTable.Columns["Add"].Key = "Add";
				grdStiching.RootTable.Columns["Add"].Caption = "Add";
				grdStiching.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdStiching.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
				grdStiching.RootTable.Columns["Add"].ButtonText = "Add";
				grdStiching.RootTable.Columns["Add"].Width = 40;
				grdStiching.RootTable.Columns.Add("Delete");
				grdStiching.RootTable.Columns["Delete"].Key = "Delete";
				grdStiching.RootTable.Columns["Delete"].Caption = "X";
				grdStiching.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdStiching.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
				grdStiching.RootTable.Columns["Delete"].ButtonText = "X";
				grdStiching.RootTable.Columns["Delete"].Width = 20;
			}
			new GridEXColumn();
			GridEXFormatCondition fc = new GridEXFormatCondition(grdStiching.RootTable.Columns["WagesType"], (ConditionOperator)0, (object)"Free Of Cost");
			fc.FormatStyle.ForeColor = Color.Red;
			grdStiching.RootTable.FormatConditions.Add(fc);
			CommonServices.GridAutoAdjustment(AryColWidth, grdStiching);
			ctrlGrdBar4_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdStiching_EditingCell(object sender, EditingCellEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdStiching.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				PreviousContractorIdStiching = Conversion.ToInt(item.Cells["SupplierId"].Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PendingGrnAndGdn()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = RefDocTypeId;
			obj.Id = RefDocId;
			dtpendingGpForWages = InvGrn.GetPendingDocumentsForConractorWagesPartyProcessingByRefIds(obj);
			if (dtpendingGpForWages.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("DocumentTypeDescription");
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("StockPartyId");
				dt.Columns.Add("StockParty");
				dt.Columns.Add("GpNo", typeof(int));
				dt.Columns.Add("VehicleNo");
				dt.Columns.Add("BiltyNo");
				dt.Columns.Add("GrossWeight", typeof(double));
				dt.Columns.Add("TotalQty", typeof(double));
				dt.Columns.Add("JobOrderId", typeof(int));
				dt.Columns.Add("RefDocQty", typeof(double));
				dt.Columns.Add("RefDocWeight", typeof(double));
				dt.Columns.Add("RefLineId", typeof(int));
				Dictionary<int, int> idToRefLineId = new Dictionary<int, int>();
				for (int i = 0; i < dtpendingGpForWages.Rows.Count; i++)
				{
					int currentId = (int)dtpendingGpForWages.Rows[i]["Id"];
					int refLineId = (idToRefLineId[currentId] = ((!idToRefLineId.ContainsKey(currentId)) ? 1 : (idToRefLineId[currentId] + 1)));
					dt.Rows.Add(dtpendingGpForWages.Rows[i]["Id"], dtpendingGpForWages.Rows[i]["DocumentTypeId"], dtpendingGpForWages.Rows[i]["DocumentTypeDescription"], dtpendingGpForWages.Rows[i]["DocDate"], dtpendingGpForWages.Rows[i]["DocNo"], dtpendingGpForWages.Rows[i]["StockPartyId"], dtpendingGpForWages.Rows[i]["StockParty"], dtpendingGpForWages.Rows[i]["GpNo"], dtpendingGpForWages.Rows[i]["VehicleNo"], dtpendingGpForWages.Rows[i]["BiltyNo"], dtpendingGpForWages.Rows[i]["GrossWeight"], dtpendingGpForWages.Rows[i]["TotalQty"], dtpendingGpForWages.Rows[i]["JobOrderId"], dtpendingGpForWages.Rows[i]["GrossWeight"], dtpendingGpForWages.Rows[i]["TotalQty"], refLineId);
				}
				gridPendingWagesSlip.DataSource = dt;
				gridPendingWagesSlip.RetrieveStructure();
				GridPendingTicketSetting();
			}
			else
			{
				gridPendingWagesSlip.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridPendingWagesSlip_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Load")
			{
				LoadDataForWages();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PendingTicket()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			InvGrn obj = new InvGrn();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = RefDocTypeId;
			obj.Id = RefDocId;
			dtpendingGpForWages = InvGrn.GetAllPendingRecordsForConractorWagesPartyProcessing(obj);
			if (dtpendingGpForWages.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("DocumentTypeDescription");
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("StockPartyId");
				dt.Columns.Add("StockParty");
				dt.Columns.Add("GpNo", typeof(int));
				dt.Columns.Add("VehicleNo");
				dt.Columns.Add("BiltyNo");
				dt.Columns.Add("GrossWeight", typeof(double));
				dt.Columns.Add("TotalQty", typeof(double));
				dt.Columns.Add("JobOrderId", typeof(int));
				dt.Columns.Add("RefDocQty", typeof(double));
				dt.Columns.Add("RefDocWeight", typeof(double));
				dt.Columns.Add("RefLineId", typeof(int));
				Dictionary<int, int> idToRefLineId = new Dictionary<int, int>();
				for (int i = 0; i < dtpendingGpForWages.Rows.Count; i++)
				{
					int currentId = (int)dtpendingGpForWages.Rows[i]["Id"];
					int refLineId = (idToRefLineId[currentId] = ((!idToRefLineId.ContainsKey(currentId)) ? 1 : (idToRefLineId[currentId] + 1)));
					dt.Rows.Add(dtpendingGpForWages.Rows[i]["Id"], dtpendingGpForWages.Rows[i]["DocumentTypeId"], dtpendingGpForWages.Rows[i]["DocumentTypeDescription"], dtpendingGpForWages.Rows[i]["DocDate"], dtpendingGpForWages.Rows[i]["DocNo"], dtpendingGpForWages.Rows[i]["StockPartyId"], dtpendingGpForWages.Rows[i]["StockParty"], dtpendingGpForWages.Rows[i]["GpNo"], dtpendingGpForWages.Rows[i]["VehicleNo"], dtpendingGpForWages.Rows[i]["BiltyNo"], dtpendingGpForWages.Rows[i]["GrossWeight"], dtpendingGpForWages.Rows[i]["TotalQty"], dtpendingGpForWages.Rows[i]["JobOrderId"], dtpendingGpForWages.Rows[i]["GrossWeight"], dtpendingGpForWages.Rows[i]["TotalQty"], refLineId);
				}
				gridPendingWagesSlip.DataSource = dt;
				gridPendingWagesSlip.RetrieveStructure();
				GridPendingTicketSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridPendingTicketSetting()
	{
		try
		{
			gridPendingWagesSlip.RootTable.Columns["Id"].Visible = false;
			gridPendingWagesSlip.RootTable.Columns["RefDocQty"].Visible = false;
			gridPendingWagesSlip.RootTable.Columns["RefDocWeight"].Visible = false;
			gridPendingWagesSlip.RootTable.Columns["RefLineId"].Visible = false;
			gridPendingWagesSlip.RootTable.Columns["StockPartyId"].Visible = false;
			gridPendingWagesSlip.RootTable.Columns["DocumentTypeId"].Visible = false;
			gridPendingWagesSlip.RootTable.Columns["JobOrderId"].Visible = false;
			gridPendingWagesSlip.RootTable.Columns["DocumentTypeDescription"].Width = 150;
			gridPendingWagesSlip.RootTable.Columns["GpNo"].Width = 100;
			gridPendingWagesSlip.RootTable.Columns["DocNo"].Width = 100;
			gridPendingWagesSlip.RootTable.Columns["DocDate"].Width = 100;
			gridPendingWagesSlip.RootTable.Columns["GrossWeight"].Width = 150;
			gridPendingWagesSlip.RootTable.Columns["VehicleNo"].Width = 150;
			gridPendingWagesSlip.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			gridPendingWagesSlip.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			gridPendingWagesSlip.RootTable.Columns["GrossWeight"].FormatString = "#,##.##";
			gridPendingWagesSlip.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##.##";
			gridPendingWagesSlip.RootTable.Columns["TotalQty"].AggregateFunction = (AggregateFunction)2;
			gridPendingWagesSlip.RootTable.Columns["TotalQty"].TextAlignment = (TextAlignment)3;
			gridPendingWagesSlip.RootTable.Columns["TotalQty"].FormatString = "#,##.##";
			gridPendingWagesSlip.RootTable.Columns["TotalQty"].TotalFormatString = "#,##.##";
			gridPendingWagesSlip.RootTable.Columns["RefDocQty"].AggregateFunction = (AggregateFunction)2;
			gridPendingWagesSlip.RootTable.Columns["RefDocQty"].TextAlignment = (TextAlignment)3;
			gridPendingWagesSlip.RootTable.Columns["RefDocQty"].FormatString = "#,##.##";
			gridPendingWagesSlip.RootTable.Columns["RefDocQty"].TotalFormatString = "#,##.##";
			gridPendingWagesSlip.RootTable.Columns["RefDocWeight"].AggregateFunction = (AggregateFunction)2;
			gridPendingWagesSlip.RootTable.Columns["RefDocWeight"].TextAlignment = (TextAlignment)3;
			gridPendingWagesSlip.RootTable.Columns["RefDocWeight"].FormatString = "#,##.##";
			gridPendingWagesSlip.RootTable.Columns["RefDocWeight"].TotalFormatString = "#,##.##";
			gridPendingWagesSlip.RootTable.Columns.Add("Load");
			gridPendingWagesSlip.RootTable.Columns["Load"].Caption = "Load";
			gridPendingWagesSlip.RootTable.Columns["Load"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			gridPendingWagesSlip.RootTable.Columns["Load"].ButtonStyle = (ButtonStyle)5;
			gridPendingWagesSlip.RootTable.Columns["Load"].ButtonText = "Load";
			gridPendingWagesSlip.RootTable.Columns["Load"].Width = 50;
			gridPendingWagesSlip.RootTable.Columns["Load"].Position = 0;
			gridPendingWagesSlip.FrozenColumns = 1;
			CommonServices.GridAutoAdjustment(AryColWidth, gridPendingWagesSlip);
			ctrlGrdBar5_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void LoadDataForWages()
	{
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Expected O, but got Unknown
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c1: Expected O, but got Unknown
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Expected O, but got Unknown
		try
		{
			if ((!btnsave.Visible || btnUpdate.Visible || RECID > 0) && RefDocId == 0)
			{
				MessageBox.Show("Record Not Loaded Please Reset Form First");
				return;
			}
			tabControl1.TabPages.Remove(tabPage4);
			int DocumentTypeId = 0;
			DataTable dtStock = new DataTable();
			dtStock.Columns.Add("Id");
			dtStock.Columns.Add("StockParty");
			dtStock.Rows.Add(gridPendingWagesSlip.CurrentRow.Cells["StockPartyId"].Value, gridPendingWagesSlip.CurrentRow.Cells["StockParty"].Value);
			DDL.BindDDL(dtStock, CmbStockParty, "Id", "StockParty", "Stock Party", ZeroIndex: false);
			((UltraGridBase)CmbStockParty).Rows[0].Activate();
			DataTable dtRefDocType = new DataTable();
			dtRefDocType.Columns.Add("Id");
			dtRefDocType.Columns.Add("ReferenceDocType");
			dtRefDocType.Rows.Add(gridPendingWagesSlip.CurrentRow.Cells["DocumentTypeId"].Value, gridPendingWagesSlip.CurrentRow.Cells["DocumentTypeDescription"].Value);
			DDL.BindDDL(dtRefDocType, cmbReferenceDocType, "Id", "ReferenceDocType", "Reference Doc Type", ZeroIndex: false);
			((UltraGridBase)cmbReferenceDocType).Rows[0].Activate();
			DocumentTypeId = Conversion.ToInt(gridPendingWagesSlip.CurrentRow.Cells["DocumentTypeId"].Value);
			txtDocNo.Text = Conversion.ToString(gridPendingWagesSlip.CurrentRow.Cells["DocNo"].Value);
			txtGRNId.Text = Conversion.ToString(gridPendingWagesSlip.CurrentRow.Cells["Id"].Value);
			txtGpNo.Text = Conversion.ToString(gridPendingWagesSlip.CurrentRow.Cells["GpNo"].Value);
			txtGrossWeight.Text = Conversion.ToString(gridPendingWagesSlip.CurrentRow.Cells["GrossWeight"].Value);
			txtQty.Text = Conversion.ToString(gridPendingWagesSlip.CurrentRow.Cells["TotalQty"].Value);
			txtEntryType.Text = Conversion.ToString((object)gridPendingWagesSlip.CurrentRow.Cells["DocumentTypeDescription"].Text.Trim());
			DataTable dtGrnDetail = new DataTable();
			dtdetail.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.Id = Conversion.ToInt(gridPendingWagesSlip.CurrentRow.Cells["Id"].Value);
			obj.DocumentTypeId = Conversion.ToInt(gridPendingWagesSlip.CurrentRow.Cells["DocumentTypeId"].Value);
			if (obj.DocumentTypeId == 176)
			{
				obj.ReqType = txtEntryType.Text;
				InvContractorWagesBillHeader objget = new InvContractorWagesBillHeader();
				objget.OrganizationId = UserAccount.OrganizationId;
				objget.CompanyId = UserAccount.CompanyId;
				objget.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				objget.RefDocumentTypeId = obj.DocumentTypeId;
				objget.RefDocNoId = obj.Id;
				objget.RefDocument = obj.ReqType;
				RECID = InvContractorWagesBillHeader.GetIdByRefDocTypeIdAndRefDocId(objget);
				if (RECID > 0)
				{
					btnUpdate.Visible = true;
					btnsave.Visible = false;
					InvContractorWagesBillHeader head = new InvContractorWagesBillHeader();
					head = InvContractorWagesBillHeader.GetByID(RECID);
					txtDocdate.Value = head.DocDate;
					txtdocnumber.Text = head.DocNo.ToString();
					txtDocNo.Text = head.RefDocNo.ToString();
					txtGRNId.Text = head.RefDocNoId.ToString();
					((Control)(object)cmbReferenceDocType).Enabled = false;
					txtRemarks.Text = head.OtherRemarks;
					txtGpNo.Text = Conversion.ToString((object)head.ScaleSlipNo);
					txtEntryType.Text = Conversion.ToString((object)head.RefDocument);
					chkisapprove.Checked = head.IsAproved;
				}
				else
				{
					btnUpdate.Visible = false;
					btnsave.Visible = true;
				}
			}
			dtGrnDetail = InvGrn.GetDetailForContractorWagesPartyProcessingByDocumentTypeIdAndId(obj);
			if (dtGrnDetail.Rows.Count > 0)
			{
				dtStiching = new DataTable();
				dtStiching = dtdetail.Clone();
				if (WagesAmountCalculateOnQty)
				{
					double GrossWeight = 0.0;
					double Qty = 0.0;
					for (int k = 0; k < dtGrnDetail.Rows.Count; k++)
					{
						IsRegularOrFreeOfCost = CommonServices.CheckItemsFreeofcostforWages(Conversion.ToDateTime(dtGrnDetail.Rows[k]["DocDate"]), Conversion.ToInt(cmbReferenceDocType.Value), Conversion.ToInt(dtGrnDetail.Rows[k]["ItemId"]), 0);
						dtdetail.Rows.Add(0, 0, 0, 0, IsRegularOrFreeOfCost ? "Free Of Cost" : "Regular", Conversion.ToDateTime(dtGrnDetail.Rows[k]["DocDate"]).ToString("dd-MMM-yyyy"), dtGrnDetail.Rows[k]["PackingTypeId"], dtGrnDetail.Rows[k]["PackTypeCode"], Conversion.ToDouble(dtGrnDetail.Rows[k]["Qty"]) * Conversion.ToDouble(dtGrnDetail.Rows[k]["Equivalent"]), dtGrnDetail.Rows[k]["Equivalent"], Conversion.ToDouble(dtGrnDetail.Rows[k]["Qty"]), 0, 0, Conversion.ToDouble(dtGrnDetail.Rows[k]["Qty"]) * Conversion.ToDouble(dtGrnDetail.Rows[k]["Equivalent"]), 0, 0, 0, 0, dtGrnDetail.Rows[k]["ItemId"], dtGrnDetail.Rows[k]["ItemName"], dtGrnDetail.Rows[k]["JlId"], dtGrnDetail.Rows[k]["JobLotDescription"], dtGrnDetail.Rows[k]["CropYear"], dtGrnDetail.Rows[k]["WhFId"], dtGrnDetail.Rows[k]["WarehouseFrom"], dtGrnDetail.Rows[k]["WhTId"], dtGrnDetail.Rows[k]["WareHouseTo"], dtGrnDetail.Rows[k]["PurchaseGLAC"], dtGrnDetail.Rows[k]["WarehouseType"], true, dtGrnDetail.Rows[k]["JobOrderId"], Conversion.ToDouble(dtGrnDetail.Rows[k]["Qty"]), Conversion.ToDouble(dtGrnDetail.Rows[k]["Qty"]) * Conversion.ToDouble(dtGrnDetail.Rows[k]["Equivalent"]), k + 1);
						GrossWeight += Conversion.ToDouble(dtGrnDetail.Rows[k]["GrossWeight"]);
						Qty += Conversion.ToDouble(dtGrnDetail.Rows[k]["Qty"]);
					}
					txtGrossWeight.Text = Conversion.ToString((object)GrossWeight);
					txtQty.Text = Conversion.ToString((object)Qty);
				}
				else
				{
					for (int l = 0; l < dtGrnDetail.Rows.Count; l++)
					{
						IsRegularOrFreeOfCost = CommonServices.CheckItemsFreeofcostforWages(Conversion.ToDateTime(dtGrnDetail.Rows[l]["DocDate"]), Conversion.ToInt(cmbReferenceDocType.Value), Conversion.ToInt(dtGrnDetail.Rows[l]["ItemId"]), 0);
						dtdetail.Rows.Add(0, 0, 0, 0, IsRegularOrFreeOfCost ? "Free Of Cost" : "Regular", Conversion.ToDateTime(dtGrnDetail.Rows[l]["DocDate"]).ToString("dd-MMM-yyyy"), dtGrnDetail.Rows[l]["PackingTypeId"], dtGrnDetail.Rows[l]["PackTypeCode"], dtGrnDetail.Rows[l]["GrossWeight"], dtGrnDetail.Rows[l]["Equivalent"], Conversion.ToDouble(dtGrnDetail.Rows[l]["GrossWeight"]) / Conversion.ToDouble(dtGrnDetail.Rows[l]["Equivalent"]), 0, 0, dtGrnDetail.Rows[l]["GrossWeight"], 0, 0, 0, 0, dtGrnDetail.Rows[l]["ItemId"], dtGrnDetail.Rows[l]["ItemName"], dtGrnDetail.Rows[l]["JlId"], dtGrnDetail.Rows[l]["JobLotDescription"], dtGrnDetail.Rows[l]["CropYear"], dtGrnDetail.Rows[l]["WhFId"], dtGrnDetail.Rows[l]["WarehouseFrom"], dtGrnDetail.Rows[l]["WhTId"], dtGrnDetail.Rows[l]["WareHouseTo"], dtGrnDetail.Rows[l]["PurchaseGLAC"], dtGrnDetail.Rows[l]["WarehouseType"], true, dtGrnDetail.Rows[l]["JobOrderId"], Conversion.ToDouble(dtGrnDetail.Rows[l]["GrossWeight"]) / Conversion.ToDouble(dtGrnDetail.Rows[l]["Equivalent"]), Conversion.ToDouble(dtGrnDetail.Rows[l]["GrossWeight"]), l + 1);
					}
				}
				if (RefDocTypeId == 118 || DocumentTypeId == 118 || RefDocTypeId == 176 || DocumentTypeId == 176)
				{
					dtStichingItems = CommonServices.GetWagesAccount("35", 0, 1);
					for (int j = 0; j < dtdetail.Rows.Count; j++)
					{
						if (Conversion.ToDouble(dtdetail.Rows[j]["PackSize"]) < 100.0)
						{
							DataRow dr2 = dtdetail.Rows[j];
							dtStiching.Rows.Add(dr2.ItemArray);
						}
					}
					if (dtStiching.Rows.Count > 0)
					{
						tabControl1.TabPages.Add(tabPage4);
					}
				}
				if (RefDocTypeId == 220 || DocumentTypeId == 220)
				{
					tabControl1.TabPages.Add(tabPage4);
					dtStichingItems = CommonServices.GetWagesAccount("34", 0, 1);
					for (int i = 0; i < dtdetail.Rows.Count; i++)
					{
						DataRow dr = dtdetail.Rows[i];
						dtStiching.Rows.Add(dr.ItemArray);
					}
				}
				accountName(DocumentTypeId);
				txtDocdate.Value = Conversion.ToDateTime(dtdetail.Rows[0]["Date"]);
				grdwagesDetail.DataSource = dtdetail;
				grdwagesDetail.RetrieveStructure();
				DetailGridSettings();
				grdStiching.DataSource = dtStiching;
				grdStiching.RetrieveStructure();
				GridStichingSettings();
			}
			StichingWagesConfig();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RetreivalHistoryDetailGridBind(int Id)
	{
		try
		{
			InvContractorWagesBillHeader head = InvContractorWagesBillHeader.GetByID(Id);
			if (head != null && head.invContractWagesBillDateil.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("HeaderId", typeof(int));
				dt.Columns.Add("ContractorName");
				dt.Columns.Add("WagesAccount");
				dt.Columns.Add("PackingType");
				dt.Columns.Add("Weight", typeof(double));
				dt.Columns.Add("PackSize");
				dt.Columns.Add("Quantity", typeof(double));
				dt.Columns.Add("WeightCut");
				dt.Columns.Add("BillWeight", typeof(double));
				dt.Columns.Add("RateWithoutAddLess");
				dt.Columns.Add("RateAddLess", typeof(double));
				dt.Columns.Add("Rate");
				dt.Columns.Add("Amount", typeof(double));
				dt.Columns.Add("HeaderWeight", typeof(double));
				dt.Columns.Add("HeaderQty", typeof(double));
				dt.Columns.Add("IsCompany", typeof(bool));
				dt.Columns.Add("JobOrderId", typeof(int));
				dt.Columns.Add("RefDocQty", typeof(double));
				dt.Columns.Add("RefDocWeight", typeof(double));
				dt.Columns.Add("RefLineId", typeof(int));
				for (int i = 0; i < head.invContractWagesBillDateil.Count; i++)
				{
					dt.Rows.Add(head.invContractWagesBillDateil[i].InvContractorWagesBillHeaderId, head.invContractWagesBillDateil[i].CompanyName, head.invContractWagesBillDateil[i].WagesAccountName, head.invContractWagesBillDateil[i].PackTypeDesc, head.invContractWagesBillDateil[i].Weight, head.invContractWagesBillDateil[i].PackSize, head.invContractWagesBillDateil[i].Qty, head.invContractWagesBillDateil[i].WeightCut, head.invContractWagesBillDateil[i].BillWeight, head.invContractWagesBillDateil[i].WageRate - head.invContractWagesBillDateil[i].RateAddLess, head.invContractWagesBillDateil[i].RateAddLess, head.invContractWagesBillDateil[i].WageRate, head.invContractWagesBillDateil[i].WagesAmount, head.WeightTotal, head.QtyTotal, head.invContractWagesBillDateil[i].IsCompany, head.invContractWagesBillDateil[i].JobOrderId, head.invContractWagesBillDateil[i].RefDocQty, head.invContractWagesBillDateil[i].RefDocWeight, head.invContractWagesBillDateil[i].RefLineId);
				}
				grdDataRetrieve.DataSource = dt;
				grdDataRetrieve.RetrieveStructure();
				GridRetrieveSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridRetrieveSettings()
	{
		try
		{
			grdDataRetrieve.AllowDelete = (InheritableBoolean)2;
			grdDataRetrieve.AllowEdit = (InheritableBoolean)2;
			grdDataRetrieve.ColumnAutoResize = true;
			grdDataRetrieve.RootTable.Columns["HeaderId"].Visible = false;
			grdDataRetrieve.RootTable.Columns["HeaderWeight"].Visible = false;
			grdDataRetrieve.RootTable.Columns["HeaderQty"].Visible = false;
			grdDataRetrieve.RootTable.Columns["JobOrderId"].Visible = false;
			grdDataRetrieve.RootTable.Columns["RefLineId"].Visible = false;
			grdDataRetrieve.RootTable.Columns["RefDocQty"].Visible = false;
			grdDataRetrieve.RootTable.Columns["RefDocWeight"].Visible = false;
			grdDataRetrieve.RootTable.Columns["HeaderWeight"].Visible = false;
			grdDataRetrieve.RootTable.Columns["RateWithoutAddLess"].Visible = EnableAddLessOnWagesRegular;
			grdDataRetrieve.RootTable.Columns["RateAddLess"].Visible = EnableAddLessOnWagesRegular;
			grdDataRetrieve.RootTable.Columns["Quantity"].AggregateFunction = (AggregateFunction)2;
			grdDataRetrieve.RootTable.Columns["Quantity"].TextAlignment = (TextAlignment)3;
			grdDataRetrieve.RootTable.Columns["Quantity"].FormatString = "0,0";
			grdDataRetrieve.RootTable.Columns["Quantity"].TotalFormatString = "0,0";
			grdDataRetrieve.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdDataRetrieve.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdDataRetrieve.RootTable.Columns["Weight"].FormatString = "0,0";
			grdDataRetrieve.RootTable.Columns["Weight"].TotalFormatString = "0,0";
			grdDataRetrieve.RootTable.Columns["BillWeight"].AggregateFunction = (AggregateFunction)2;
			grdDataRetrieve.RootTable.Columns["BillWeight"].TextAlignment = (TextAlignment)3;
			grdDataRetrieve.RootTable.Columns["BillWeight"].FormatString = "0,0";
			grdDataRetrieve.RootTable.Columns["BillWeight"].TotalFormatString = "0,0";
			grdDataRetrieve.RootTable.Columns["RateWithoutAddLess"].TextAlignment = (TextAlignment)3;
			grdDataRetrieve.RootTable.Columns["RateWithoutAddLess"].FormatString = "#,##0.##";
			grdDataRetrieve.RootTable.Columns["RateWithoutAddLess"].TotalFormatString = "#,##0.##";
			grdDataRetrieve.RootTable.Columns["RateAddLess"].TextAlignment = (TextAlignment)3;
			grdDataRetrieve.RootTable.Columns["RateAddLess"].FormatString = "#,##0.##";
			grdDataRetrieve.RootTable.Columns["RateAddLess"].TotalFormatString = "#,##0.##";
			grdDataRetrieve.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdDataRetrieve.RootTable.Columns["Rate"].FormatString = "#,##0.##";
			grdDataRetrieve.RootTable.Columns["Rate"].TotalFormatString = "#,##0.##";
			grdDataRetrieve.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdDataRetrieve.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdDataRetrieve.RootTable.Columns["Amount"].FormatString = "0,0";
			grdDataRetrieve.RootTable.Columns["Amount"].TotalFormatString = "0,0";
			grdDataRetrieve.RootTable.Columns["RefDocQty"].AggregateFunction = (AggregateFunction)2;
			grdDataRetrieve.RootTable.Columns["RefDocQty"].TextAlignment = (TextAlignment)3;
			grdDataRetrieve.RootTable.Columns["RefDocQty"].FormatString = "#,##0.##";
			grdDataRetrieve.RootTable.Columns["RefDocQty"].TotalFormatString = "#,##0.##";
			grdDataRetrieve.RootTable.Columns["RefDocWeight"].AggregateFunction = (AggregateFunction)2;
			grdDataRetrieve.RootTable.Columns["RefDocWeight"].TextAlignment = (TextAlignment)3;
			grdDataRetrieve.RootTable.Columns["RefDocWeight"].FormatString = "#,##0.##";
			grdDataRetrieve.RootTable.Columns["RefDocWeight"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustmentNew(grdDataRetrieve);
			ctrlGrdBar6_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		try
		{
			if (ValidationOnformClose("Reset"))
			{
				FormRest();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FormRest()
	{
		try
		{
			RECID = 0;
			PreviousContractorId = 0;
			((Control)(object)cmbReferenceDocType).Text = string.Empty;
			((Control)(object)CmbStockParty).Text = string.Empty;
			((UltraGridBase)CmbStockParty).DataSource = null;
			txtDocNo.Text = string.Empty;
			((UltraGridBase)cmbReferenceDocType).DataSource = null;
			txtGpNo.Text = string.Empty;
			txtRemarks.Text = string.Empty;
			txtQty.Text = string.Empty;
			txtGrossWeight.Text = string.Empty;
			txtGRNId.Text = string.Empty;
			GenerateDocNo();
			dtdetail.Rows.Clear();
			grdwagesDetail.ClearStructure();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			((Control)(object)cmbReferenceDocType).Enabled = true;
			tabPage4.Visible = false;
			if (RefDocTypeId > 0 && RefDocId > 0)
			{
				PendingGrnAndGdn();
			}
			else
			{
				PendingTicket();
			}
			tabControl1.TabPages.Remove(tabPage4);
			tabControl2.TabPages.Remove(tabPage6);
			IsReferred = false;
			GridStichingLockEditing();
			grdDetailLockEditing();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		try
		{
			if (ValidationOnformClose("Close"))
			{
				Close();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		try
		{
			if (ValidationOnformClose("Close"))
			{
				Close();
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
			StichingWagesConfig();
			WagesAmountCalculationsConfigurations();
			suppliercustomer();
			accountName();
			WagesItemAndContractorRefresh();
			grdDetailLockEditing();
			GridStichingLockEditing();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void frmWagesBillPartyProcessing_FormClosing(object sender, FormClosingEventArgs e)
	{
		try
		{
			if (!ValidationOnformClose("Close"))
			{
				e.Cancel = true;
			}
			else
			{
				HistoryStack.Remove(this);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtdocnumber.Text.Trim() == string.Empty || txtdocnumber.Text.Trim() == "0")
		{
			MessageBox.Show("document Number Field Required");
			txtdocnumber.Focus();
			return false;
		}
		if (((Control)(object)cmbReferenceDocType).Text.Trim() == string.Empty || Conversion.ToInt(cmbReferenceDocType.Value) == 0)
		{
			MessageBox.Show("ReferenceDocType Field Required");
			cmbReferenceDocType.Focus();
			return false;
		}
		if (((UltraGridBase)CmbStockParty).ActiveRow == null || Conversion.ToInt(CmbStockParty.Value) == 0)
		{
			MessageBox.Show("Stock Party Field Required");
			CmbStockParty.Focus();
			return false;
		}
		if (txtDocNo.Text.Trim() == string.Empty || txtDocNo.Text.Trim() == "0")
		{
			MessageBox.Show("Doc No Field Required");
			txtDocNo.Focus();
			return false;
		}
		if (txtGRNId.Text.Trim() == string.Empty || txtGRNId.Text.Trim() == "0")
		{
			MessageBox.Show("DocNoId Field Required");
			txtGRNId.Focus();
			return false;
		}
		return true;
	}

	private bool ValidationOnformClose(string ResetOrClose)
	{
		bool Value = true;
		if (grdwagesDetail.RowCount > 0 && grdDataRetrieve.RowCount > 0)
		{
			double CurrentQty = Conversion.ToDouble((object)txtQty.Text);
			double CurrentWeight = Conversion.ToDouble((object)txtGrossWeight.Text);
			double PreviousWeight = Conversion.ToDouble(grdDataRetrieve.GetRow(0).Cells["HeaderWeight"].Value);
			double PreviousQty = Conversion.ToDouble(grdDataRetrieve.GetRow(0).Cells["HeaderQty"].Value);
			if (WagesAmountCalculateOnQty)
			{
				if (CurrentQty != PreviousQty)
				{
					if (MessageBox.Show("Current Qty Does not match with Previous Qty\nIf You " + ResetOrClose + " the Form Previous Saved Record Will Be Deleted\nAre you Sure to Do So???", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return false;
					}
					InvContractorWagesBillHeader.WagesDeleteByRefDocTypeAndId(UserAccount.OrganizationId, UserAccount.CompanyId, RefDocTypeId, RefDocId);
				}
				Value = true;
			}
			else
			{
				if (CurrentWeight != PreviousWeight)
				{
					if (MessageBox.Show("Current Weight Does not match with Previous Weight\nIf You " + ResetOrClose + " the Form Previous Saved Record Will Be Deleted\nAre you Sure to Do So???", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return false;
					}
					InvContractorWagesBillHeader.WagesDeleteByRefDocTypeAndId(UserAccount.OrganizationId, UserAccount.CompanyId, RefDocTypeId, RECID);
				}
				Value = true;
			}
		}
		return Value;
	}

	private void ValidationforRefRowWeight(DataTable dt, string GridName)
	{
		try
		{
			foreach (var item in (from row in dt.AsEnumerable()
				group row by new
				{
					WagesId = ((!row.IsNull("WagesId")) ? Conversion.ToInt((object)row.Field<int>("WagesId")) : 0),
					RefLineId = ((!row.IsNull("RefLineId")) ? Conversion.ToInt((object)row.Field<int>("RefLineId")) : 0),
					RefDocWeight = (row.IsNull("RefDocWeight") ? 0.0 : Conversion.ToDouble((object)row.Field<double>("RefDocWeight")))
				} into @group
				select new
				{
					WagesId = @group.Key.WagesId,
					RefLineId = @group.Key.RefLineId,
					RefDocWeight = @group.Key.RefDocWeight,
					TotalBillWeight = @group.Sum((DataRow row) => (!row.IsNull("BillWeight")) ? row.Field<double>("BillWeight") : 0.0)
				}).ToList())
			{
				if (item.TotalBillWeight > item.RefDocWeight)
				{
					string wagesAccountName = (from row in dtwagesacc.AsEnumerable()
						where row.Field<int>("Id") == item.WagesId
						select row.Field<string>("WagesAccountName")).FirstOrDefault() ?? "";
					throw new Exception("TotalBillWeight against Reference RowNo and Account Should be Equal to or less than Reference Row Weight\n" + $"Here TotalBillWeight ({item.TotalBillWeight}) exceeds Reference Row Weight ({item.RefDocWeight}) for WagesAccount ({wagesAccountName}) and RowNo {item.RefLineId}  in {GridName} Grid");
				}
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void ValidationforRefRowQty(DataTable dt, string GridName)
	{
		try
		{
			foreach (var item in (from row in dt.AsEnumerable()
				group row by new
				{
					WagesId = ((!row.IsNull("WagesId")) ? Conversion.ToInt((object)row.Field<int>("WagesId")) : 0),
					RefLineId = ((!row.IsNull("RefLineId")) ? Conversion.ToInt((object)row.Field<int>("RefLineId")) : 0),
					RefDocQty = (row.IsNull("RefDocQty") ? 0.0 : Conversion.ToDouble((object)row.Field<double>("RefDocQty")))
				} into @group
				select new
				{
					WagesId = @group.Key.WagesId,
					RefLineId = @group.Key.RefLineId,
					RefDocQty = @group.Key.RefDocQty,
					TotalQuantity = @group.Sum((DataRow row) => (!row.IsNull("Quantity")) ? row.Field<double>("Quantity") : 0.0)
				}).ToList())
			{
				if (item.TotalQuantity > item.RefDocQty)
				{
					string wagesAccountName = (from row in dtwagesacc.AsEnumerable()
						where row.Field<int>("Id") == item.WagesId
						select row.Field<string>("WagesAccountName")).FirstOrDefault() ?? "";
					throw new Exception("TotalQty against Reference RowNo and Wages Account Should be Equal to or less than Reference Row Qty\n" + $"Here TotalQty ({item.TotalQuantity}) exceeds Reference Row Qty ({item.RefDocQty}) for WagesAccount ({wagesAccountName}) and RowNo {item.RefLineId} in {GridName} Grid");
				}
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Expected O, but got Unknown
		//IL_0d2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d32: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			InvContractorWagesBillHeader obj = new InvContractorWagesBillHeader();
			if (!WagesAmountCalculateOnQty)
			{
				if (Conversion.ToDouble((object)grdwagesDetail.GetTotal(grdwagesDetail.RootTable.Columns["Weight"], (AggregateFunction)2).ToString()) != Conversion.ToDouble((object)txtGrossWeight.Text))
				{
					MessageBox.Show("Wages Grid Total Weight and GrossWeight not equal please check!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				if (Conversion.ToInt(cmbReferenceDocType.Value) == 118 && Conversion.ToDouble((object)grdStiching.GetTotal(grdStiching.RootTable.Columns["Weight"], (AggregateFunction)2).ToString()) != Conversion.ToDouble((object)txtGrossWeight.Text))
				{
					MessageBox.Show("Other Wages Grid Total Weight and GrossWeight not equal please check!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
			}
			if (RECID > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				obj.Id = RECID;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			if (Conversion.ToInt(cmbReferenceDocType.Value) == 118 || Conversion.ToInt(cmbReferenceDocType.Value) == 176 || RefDocTypeId == 118 || RefDocTypeId == 176)
			{
				if (WagesAmountCalculateOnQty)
				{
					ValidationforRefRowQty(dtdetail, "Regular_Wages");
					ValidationforRefRowQty(dtStiching, "Other_Wages");
				}
				else
				{
					ValidationforRefRowWeight(dtdetail, "Regular_Wages");
					ValidationforRefRowWeight(dtStiching, "Other_Wages");
				}
			}
			obj.CompanyId = UserAccount.CompanyId;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.DocNo = Conversion.ToInt((object)txtdocnumber.Text);
			obj.DocDate = txtDocdate.Value;
			obj.DocumentTypeId = 219;
			obj.StockPartyId = Conversion.ToInt(CmbStockParty.Value);
			obj.RefDocumentTypeId = Conversion.ToInt(cmbReferenceDocType.Value);
			obj.RefDocNoId = Conversion.ToInt((object)txtGRNId.Text);
			obj.RefDocNo = Conversion.ToInt((object)txtDocNo.Text);
			obj.WeightTotal = Conversion.ToDouble((object)txtGrossWeight.Text);
			obj.OtherRemarks = Conversion.ToString((object)txtRemarks.Text);
			obj.EntryUser = Conversion.ToInt((object)UserAccount.ID);
			obj.ModifyUser = Conversion.ToInt((object)UserAccount.ID);
			obj.ScaleSlipNo = Conversion.ToInt((object)txtGpNo.Text.Trim());
			obj.RefDocument = Conversion.ToString((object)txtEntryType.Text.Trim());
			obj.ModifyDate = DateTime.Now;
			obj.EntryDate = DateTime.Now;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (chkisapprove.Checked)
			{
				obj.IsAproved = true;
				obj.ApprovedDate = DateTime.Now;
				obj.ApprovedUserId = Conversion.ToInt((object)UserAccount.ID);
			}
			else
			{
				obj.IsAproved = false;
			}
			obj.invContractWagesBillDateil = new List<InvContractorWagesBillDetail>();
			double Qty = 0.0;
			double weight = 0.0;
			if (grdwagesDetail.GetRows().Count() == 0)
			{
				throw new Exception("Regular Wages Grid... Record Not Found");
			}
			GridEXRow[] rows = grdwagesDetail.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				InvContractorWagesBillDetail wagesbilldetail2 = new InvContractorWagesBillDetail();
				obj.DocDate = Conversion.ToDateTime(r2.Cells["Date"].Value);
				wagesbilldetail2.RefDocDate = Conversion.ToDateTime(r2.Cells["Date"].Value);
				if (obj.DocDate.Date < wagesbilldetail2.RefDocDate.Date)
				{
					throw new Exception("Doc Date Can't be " + obj.DocDate.Date.ToString() + " Beacuse Detail Date " + wagesbilldetail2.RefDocDate.ToString() + " is Greater");
				}
				if (Conversion.ToInt(r2.Cells["WagesId"].Value) == 0 || r2.Cells["WagesId"].Text.ToString() == string.Empty)
				{
					throw new Exception("WagesAccount Field required In Regular Wages Grid...");
				}
				wagesbilldetail2.InvConractorWagesAccountsId = Conversion.ToInt(r2.Cells["WagesId"].Value);
				if (Conversion.ToInt(r2.Cells["Quantity"].Value) == 0 || r2.Cells["Quantity"].Text.ToString() == string.Empty)
				{
					throw new Exception("Quantity Field required In Regular Wages Grid...");
				}
				wagesbilldetail2.Qty = Conversion.ToDouble(r2.Cells["Quantity"].Value);
				wagesbilldetail2.BillQty = Conversion.ToDouble(r2.Cells["Quantity"].Value);
				Qty += wagesbilldetail2.Qty;
				if (Conversion.ToDouble(r2.Cells["Weight"].Value) == 0.0 || r2.Cells["Weight"].Text.ToString() == string.Empty)
				{
					throw new Exception("Weight Field required In Regular Wages Grid...");
				}
				wagesbilldetail2.Weight = Conversion.ToDouble((object)r2.Cells["Weight"].Text);
				wagesbilldetail2.BillWeight = Conversion.ToDouble(r2.Cells["Weight"].Value);
				weight += wagesbilldetail2.Weight;
				if (Conversion.ToDouble(r2.Cells["PackSize"].Value) == 0.0 || r2.Cells["PackSize"].Text.ToString() == string.Empty)
				{
					throw new Exception("PackSize Field required In Regular Wages Grid...");
				}
				wagesbilldetail2.PackSize = Conversion.ToDouble((object)r2.Cells["PackSize"].Text);
				if (Conversion.ToString((object)r2.Cells["WagesType"].Text) == "Free Of Cost" || Conversion.ToString((object)r2.Cells["WarehouseType"].Text) == "Dryer")
				{
					wagesbilldetail2.FreeOfCost = true;
					wagesbilldetail2.WageRate = 0.0;
					wagesbilldetail2.RateAddLess = 0.0;
					wagesbilldetail2.WagesAmount = 0.0;
				}
				else
				{
					wagesbilldetail2.RateAddLess = Conversion.ToDouble((object)r2.Cells["RateAddLess"].Text);
					wagesbilldetail2.WageRate = Conversion.ToDouble((object)r2.Cells["Rate"].Text);
					if (wagesbilldetail2.WageRate == 0.0)
					{
						throw new Exception("Rate Field required In Regular Wages Grid...");
					}
				}
				if (Conversion.ToString((object)r2.Cells["WagesType"].Text) == "Free Of Cost")
				{
					wagesbilldetail2.FreeOfCost = true;
				}
				else
				{
					wagesbilldetail2.FreeOfCost = false;
				}
				if (wagesbilldetail2.FreeOfCost)
				{
					wagesbilldetail2.WagesAmount = 0.0;
				}
				else if (WagesAmountCalculateOnQty)
				{
					wagesbilldetail2.WagesAmount = Math.Round(wagesbilldetail2.Qty * wagesbilldetail2.WageRate, 2);
				}
				else
				{
					wagesbilldetail2.WagesAmount = Math.Round(wagesbilldetail2.Weight / wagesbilldetail2.PackSize * wagesbilldetail2.WageRate, 2);
				}
				if (Conversion.ToInt(r2.Cells["SupplierId"].Value) == 0 || r2.Cells["SupplierId"].Text.ToString() == string.Empty)
				{
					throw new Exception("ContractorAccount Field required In Regular Wages Grid...");
				}
				wagesbilldetail2.ContractorId = Conversion.ToInt(r2.Cells["SupplierId"].Value);
				if (Conversion.ToInt(r2.Cells["ItemId"].Value) == 0 || r2.Cells["ItemId"].Text.ToString() == string.Empty)
				{
					throw new Exception("Item Name Field required In Regular Wages Grid...");
				}
				wagesbilldetail2.ItemId = Conversion.ToInt((object)r2.Cells["ItemId"].Text);
				if (Conversion.ToInt(r2.Cells["PurchaseGLAC"].Value) == 0 || r2.Cells["PurchaseGLAC"].Text.ToString() == string.Empty)
				{
					throw new Exception("PurchaseGLAC Field required In Regular Wages Grid...");
				}
				wagesbilldetail2.PurchaseGLAC = Conversion.ToInt((object)r2.Cells["PurchaseGLAC"].Text);
				wagesbilldetail2.ItemName = Conversion.ToString(r2.Cells["Item"].Value);
				wagesbilldetail2.CompanyName = Conversion.ToString((object)r2.Cells["SupplierId"].Text);
				wagesbilldetail2.WagesAccountName = Conversion.ToString((object)r2.Cells["WagesId"].Text);
				wagesbilldetail2.WagesTypeId = Conversion.ToInt(r2.Cells["WagesType"].Value);
				wagesbilldetail2.JobLotId = Conversion.ToInt((object)r2.Cells["JobLotId"].Text);
				wagesbilldetail2.Crop = Conversion.ToString((object)r2.Cells["Crop"].Text);
				wagesbilldetail2.InvPackingTypeId = Conversion.ToInt((object)r2.Cells["packingTypeId"].Text);
				wagesbilldetail2.WareHouseFromId = Conversion.ToInt((object)r2.Cells["MoveFromId"].Text);
				wagesbilldetail2.WareHouseToId = Conversion.ToInt((object)r2.Cells["MoveToId"].Text);
				wagesbilldetail2.BillQty = Conversion.ToDouble((object)r2.Cells["BillQty"].Text);
				wagesbilldetail2.WeightCut = Conversion.ToDouble((object)r2.Cells["WeightCut"].Text);
				wagesbilldetail2.IsCompany = Conversion.ToBool(r2.Cells["IsCompany"].Value);
				wagesbilldetail2.JobOrderId = Conversion.ToInt(r2.Cells["JobOrderId"].Value);
				wagesbilldetail2.RefDocQty = Conversion.ToDouble((object)r2.Cells["RefDocQty"].Text);
				wagesbilldetail2.RefDocWeight = Conversion.ToDouble((object)r2.Cells["RefDocWeight"].Text);
				wagesbilldetail2.RefLineId = Conversion.ToInt((object)r2.Cells["RefLineId"].Text);
				obj.invContractWagesBillDateil.Add(wagesbilldetail2);
			}
			if (grdStiching.GetRows().Count() == 0 && StichingWagesCompulsory && ((Conversion.ToInt(cmbReferenceDocType.Value) == 176 && !obj.RefDocument.Contains("Issue")) || Conversion.ToInt(cmbReferenceDocType.Value) == 118 || Conversion.ToInt(cmbReferenceDocType.Value) == 220))
			{
				throw new Exception("Stiching Wages Grid... Record Not Found\n Stiching Wages Compulsory Configuration is On");
			}
			if (grdStiching.GetRows().Count() > 0)
			{
				rows = grdStiching.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (!StichingWagesCompulsory && (Conversion.ToInt(r.Cells["WagesId"].Value) == 0 || r.Cells["WagesId"].Text.ToString() == string.Empty) && (Conversion.ToInt(r.Cells["SupplierId"].Value) == 0 || r.Cells["SupplierId"].Text.ToString() == string.Empty))
					{
						continue;
					}
					string SecondGridName = "Other Wages Grid ...";
					InvContractorWagesBillDetail wagesbilldetail = new InvContractorWagesBillDetail();
					if (Conversion.ToInt(r.Cells["WagesId"].Value) == 0 || r.Cells["WagesId"].Text.ToString() == string.Empty)
					{
						throw new Exception("WagesAccount Field required In " + SecondGridName);
					}
					wagesbilldetail.InvConractorWagesAccountsId = Conversion.ToInt(r.Cells["WagesId"].Value);
					if (Conversion.ToInt(r.Cells["Quantity"].Value) == 0 || r.Cells["Quantity"].Text.ToString() == string.Empty)
					{
						throw new Exception("Quantity Field required In " + SecondGridName);
					}
					wagesbilldetail.Qty = Conversion.ToDouble(r.Cells["Quantity"].Value);
					wagesbilldetail.BillQty = Conversion.ToDouble(r.Cells["Quantity"].Value);
					Qty += wagesbilldetail.Qty;
					if (Conversion.ToDouble(r.Cells["Weight"].Value) == 0.0 || r.Cells["Weight"].Text.ToString() == string.Empty)
					{
						throw new Exception("Weight Field required In " + SecondGridName);
					}
					wagesbilldetail.Weight = Conversion.ToDouble((object)r.Cells["Weight"].Text);
					wagesbilldetail.BillWeight = Conversion.ToDouble(r.Cells["Weight"].Value);
					weight += wagesbilldetail.Weight;
					if (Conversion.ToDouble(r.Cells["PackSize"].Value) == 0.0 || r.Cells["PackSize"].Text.ToString() == string.Empty)
					{
						throw new Exception("PackSize Field Required In " + SecondGridName);
					}
					wagesbilldetail.PackSize = Conversion.ToDouble((object)r.Cells["PackSize"].Text);
					if (Conversion.ToString((object)r.Cells["WagesType"].Text) == "Free Of Cost" || Conversion.ToString((object)r.Cells["WarehouseType"].Text) == "Dryer")
					{
						wagesbilldetail.FreeOfCost = true;
						wagesbilldetail.RateAddLess = 0.0;
						wagesbilldetail.WageRate = 0.0;
						wagesbilldetail.WagesAmount = 0.0;
					}
					else
					{
						wagesbilldetail.RateAddLess = Conversion.ToDouble((object)r.Cells["RateAddLess"].Text);
						wagesbilldetail.WageRate = Conversion.ToDouble((object)r.Cells["Rate"].Text);
						if (wagesbilldetail.WageRate == 0.0)
						{
							throw new Exception("Rate Field required In " + SecondGridName);
						}
						if (WagesAmountCalculateOnQty)
						{
							wagesbilldetail.WagesAmount = Math.Round(wagesbilldetail.Qty * wagesbilldetail.WageRate, 2);
						}
						else
						{
							wagesbilldetail.WagesAmount = Math.Round(wagesbilldetail.Weight / wagesbilldetail.PackSize * wagesbilldetail.WageRate, 2);
						}
					}
					if (Conversion.ToInt(r.Cells["SupplierId"].Value) == 0 || r.Cells["SupplierId"].Text.ToString() == string.Empty)
					{
						throw new Exception("ContractorAccount Field required In " + SecondGridName);
					}
					wagesbilldetail.ContractorId = Conversion.ToInt(r.Cells["SupplierId"].Value);
					if (Conversion.ToInt(r.Cells["ItemId"].Value) == 0 || r.Cells["ItemId"].Text.ToString() == string.Empty)
					{
						throw new Exception("Item Name Field required In " + SecondGridName);
					}
					wagesbilldetail.ItemId = Conversion.ToInt((object)r.Cells["ItemId"].Text);
					if (Conversion.ToInt(r.Cells["PurchaseGLAC"].Value) == 0 || r.Cells["PurchaseGLAC"].Text.ToString() == string.Empty)
					{
						throw new Exception("PurchaseGLAC Field required In " + SecondGridName);
					}
					wagesbilldetail.PurchaseGLAC = Conversion.ToInt((object)r.Cells["PurchaseGLAC"].Text);
					wagesbilldetail.ItemName = Conversion.ToString(r.Cells["Item"].Value);
					wagesbilldetail.CompanyName = Conversion.ToString((object)r.Cells["SupplierId"].Text);
					wagesbilldetail.WagesAccountName = Conversion.ToString((object)r.Cells["WagesId"].Text);
					wagesbilldetail.RefDocDate = Conversion.ToDateTime(r.Cells["Date"].Value);
					wagesbilldetail.JobLotId = Conversion.ToInt((object)r.Cells["JobLotId"].Text);
					wagesbilldetail.Crop = Conversion.ToString((object)r.Cells["Crop"].Text);
					wagesbilldetail.InvPackingTypeId = Conversion.ToInt((object)r.Cells["packingTypeId"].Text);
					wagesbilldetail.WareHouseFromId = Conversion.ToInt((object)r.Cells["MoveFromId"].Text);
					wagesbilldetail.WareHouseToId = Conversion.ToInt((object)r.Cells["MoveToId"].Text);
					wagesbilldetail.BillQty = Conversion.ToInt((object)r.Cells["BillQty"].Text);
					wagesbilldetail.WeightCut = Conversion.ToDouble((object)r.Cells["WeightCut"].Text);
					if (Conversion.ToString((object)r.Cells["WagesType"].Text) == "Free Of Cost")
					{
						wagesbilldetail.FreeOfCost = true;
					}
					else
					{
						wagesbilldetail.FreeOfCost = false;
					}
					wagesbilldetail.WagesTypeId = 2;
					wagesbilldetail.IsCompany = Conversion.ToBool(r.Cells["IsCompany"].Value);
					wagesbilldetail.JobOrderId = Conversion.ToInt(r.Cells["JobOrderId"].Value);
					wagesbilldetail.RefDocQty = Conversion.ToDouble((object)r.Cells["RefDocQty"].Text);
					wagesbilldetail.RefDocWeight = Conversion.ToDouble((object)r.Cells["RefDocWeight"].Text);
					wagesbilldetail.RefLineId = Conversion.ToInt((object)r.Cells["RefLineId"].Text);
					obj.invContractWagesBillDateil.Add(wagesbilldetail);
				}
			}
			obj.QtyTotal = Conversion.ToDouble((object)txtQty.Text);
			obj.WeightTotal = Conversion.ToDouble((object)txtGrossWeight.Text);
			InvContractorWagesBillHeader.Save(obj);
			if (RECID > 0)
			{
				MessageBox.Show("Updated SuccessFully  [" + obj.DocNo + "]");
			}
			else
			{
				MessageBox.Show("Saved SuccessFully  [" + obj.DocNo + "]");
			}
			flag = false;
			if (RefDocTypeId > 0 && RefDocId > 0 && RefDocTypeId != 176)
			{
				flag = true;
				Close();
			}
			FormRest();
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
			RECID = 0;
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

	public void ReadById(int ID)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		try
		{
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			tabControl2.TabPages.Remove(tabPage6);
			tabControl1.TabPages.Remove(tabPage4);
			InvContractorWagesBillHeader head = new InvContractorWagesBillHeader();
			RECID = ID;
			head = InvContractorWagesBillHeader.GetByID(RECID);
			tabIncomingLabourwages.SelectedIndex = 0;
			DataTable dt2 = new DataTable();
			dt2.Columns.Add("Id");
			dt2.Columns.Add("Name");
			if (head != null)
			{
				dt2.Rows.Add(head.StockPartyId, head.StockPartyName);
				if (dt2.Rows.Count > 0)
				{
					DDL.BindDDL(dt2, CmbStockParty, "Id", "Name", "Stock Party Name", ZeroIndex: false);
					((UltraGridBase)CmbStockParty).Rows[0].Activate();
				}
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("RefDocumentTypeId");
			dt.Columns.Add("DocumentTypeDescription");
			if (head != null)
			{
				dt.Rows.Add(head.RefDocumentTypeId, head.DocumentTypeDescription);
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbReferenceDocType, "RefDocumentTypeId", "DocumentTypeDescription", "Document Type", ZeroIndex: false);
					((UltraGridBase)cmbReferenceDocType).Rows[0].Activate();
				}
			}
			txtDocdate.Value = head.DocDate;
			txtdocnumber.Text = head.DocNo.ToString();
			txtDocNo.Text = head.RefDocNo.ToString();
			txtGRNId.Text = head.RefDocNoId.ToString();
			((Control)(object)cmbReferenceDocType).Enabled = false;
			txtQty.Text = head.QtyTotal.ToString();
			txtGrossWeight.Text = head.WeightTotal.ToString();
			txtRemarks.Text = head.OtherRemarks;
			txtGpNo.Text = Conversion.ToString((object)head.ScaleSlipNo);
			txtEntryType.Text = Conversion.ToString((object)head.RefDocument);
			chkisapprove.Checked = head.IsAproved;
			dtdetail.Rows.Clear();
			dtStiching = dtdetail.Clone();
			dtStiching.Rows.Clear();
			double GrossWeight = 0.0;
			for (int k = 0; k < head.invContractWagesBillDateil.Count; k++)
			{
				if (head.invContractWagesBillDateil[k].WagesTypeId == 2)
				{
					dtStichingItems = CommonServices.GetWagesAccount("35", 0, 1);
					dtStiching.Rows.Add(head.invContractWagesBillDateil[k].ContractorId, head.invContractWagesBillDateil[k].CompanyName, head.invContractWagesBillDateil[k].InvConractorWagesAccountsId, head.invContractWagesBillDateil[k].WagesTypeId, head.invContractWagesBillDateil[k].FreeOfCost ? "Free Of Cost" : "Regular", Conversion.ToDateTime((object)head.invContractWagesBillDateil[k].RefDocDate).ToString("dd-MMM-yyyy"), head.invContractWagesBillDateil[k].InvPackingTypeId, head.invContractWagesBillDateil[k].PackTypeDesc, head.invContractWagesBillDateil[k].Weight, head.invContractWagesBillDateil[k].PackSize, head.invContractWagesBillDateil[k].Qty, head.invContractWagesBillDateil[k].WeightCut, head.invContractWagesBillDateil[k].BillQty, head.invContractWagesBillDateil[k].BillWeight, head.invContractWagesBillDateil[k].WageRate - head.invContractWagesBillDateil[k].RateAddLess, head.invContractWagesBillDateil[k].RateAddLess, head.invContractWagesBillDateil[k].WageRate, head.invContractWagesBillDateil[k].WagesAmount, head.invContractWagesBillDateil[k].ItemId, head.invContractWagesBillDateil[k].ItemName, head.invContractWagesBillDateil[k].JobLotId, head.invContractWagesBillDateil[k].JobLotDescription, head.invContractWagesBillDateil[k].Crop, head.invContractWagesBillDateil[k].WareHouseFromId, head.invContractWagesBillDateil[k].WareHouseFrom, head.invContractWagesBillDateil[k].WareHouseToId, head.invContractWagesBillDateil[k].WareHouseTo, head.invContractWagesBillDateil[k].PurchaseGLAC, head.invContractWagesBillDateil[k].WarehouseType, head.invContractWagesBillDateil[k].IsCompany, head.invContractWagesBillDateil[k].JobOrderId, head.invContractWagesBillDateil[k].RefDocQty, head.invContractWagesBillDateil[k].RefDocWeight, head.invContractWagesBillDateil[k].RefLineId);
				}
				else if ((head.RefDocumentTypeId == 68 || head.RefDocumentTypeId == 806) && head.invContractWagesBillDateil[k].WagesTypeId == 2)
				{
					dtStichingItems = CommonServices.GetWagesAccount("34", 0, 1);
					dtStiching.Rows.Add(head.invContractWagesBillDateil[k].ContractorId, head.invContractWagesBillDateil[k].CompanyName, head.invContractWagesBillDateil[k].InvConractorWagesAccountsId, head.invContractWagesBillDateil[k].WagesTypeId, head.invContractWagesBillDateil[k].FreeOfCost ? "Free Of Cost" : "Regular", Conversion.ToDateTime((object)head.invContractWagesBillDateil[k].RefDocDate).ToString("dd-MMM-yyyy"), head.invContractWagesBillDateil[k].InvPackingTypeId, head.invContractWagesBillDateil[k].PackTypeDesc, head.invContractWagesBillDateil[k].Weight, head.invContractWagesBillDateil[k].PackSize, head.invContractWagesBillDateil[k].Qty, head.invContractWagesBillDateil[k].WeightCut, head.invContractWagesBillDateil[k].BillQty, head.invContractWagesBillDateil[k].BillWeight, head.invContractWagesBillDateil[k].WageRate - head.invContractWagesBillDateil[k].RateAddLess, head.invContractWagesBillDateil[k].RateAddLess, head.invContractWagesBillDateil[k].WageRate, head.invContractWagesBillDateil[k].WagesAmount, head.invContractWagesBillDateil[k].ItemId, head.invContractWagesBillDateil[k].ItemName, head.invContractWagesBillDateil[k].JobLotId, head.invContractWagesBillDateil[k].JobLotDescription, head.invContractWagesBillDateil[k].Crop, head.invContractWagesBillDateil[k].WareHouseFromId, head.invContractWagesBillDateil[k].WareHouseFrom, head.invContractWagesBillDateil[k].WareHouseToId, head.invContractWagesBillDateil[k].WareHouseTo, head.invContractWagesBillDateil[k].PurchaseGLAC, head.invContractWagesBillDateil[k].WarehouseType, head.invContractWagesBillDateil[k].IsCompany, head.invContractWagesBillDateil[k].JobOrderId, head.invContractWagesBillDateil[k].RefDocQty, head.invContractWagesBillDateil[k].RefDocWeight, head.invContractWagesBillDateil[k].RefLineId);
				}
				else
				{
					dtdetail.Rows.Add(head.invContractWagesBillDateil[k].ContractorId, head.invContractWagesBillDateil[k].CompanyName, head.invContractWagesBillDateil[k].InvConractorWagesAccountsId, head.invContractWagesBillDateil[k].WagesTypeId, head.invContractWagesBillDateil[k].FreeOfCost ? "Free Of Cost" : "Regular", Conversion.ToDateTime((object)head.invContractWagesBillDateil[k].RefDocDate).ToString("dd-MMM-yyyy"), head.invContractWagesBillDateil[k].InvPackingTypeId, head.invContractWagesBillDateil[k].PackTypeDesc, head.invContractWagesBillDateil[k].Weight, head.invContractWagesBillDateil[k].PackSize, head.invContractWagesBillDateil[k].Qty, head.invContractWagesBillDateil[k].WeightCut, head.invContractWagesBillDateil[k].BillQty, head.invContractWagesBillDateil[k].BillWeight, head.invContractWagesBillDateil[k].WageRate - head.invContractWagesBillDateil[k].RateAddLess, head.invContractWagesBillDateil[k].RateAddLess, head.invContractWagesBillDateil[k].WageRate, head.invContractWagesBillDateil[k].WagesAmount, head.invContractWagesBillDateil[k].ItemId, head.invContractWagesBillDateil[k].ItemName, head.invContractWagesBillDateil[k].JobLotId, head.invContractWagesBillDateil[k].JobLotDescription, head.invContractWagesBillDateil[k].Crop, head.invContractWagesBillDateil[k].WareHouseFromId, head.invContractWagesBillDateil[k].WareHouseFrom, head.invContractWagesBillDateil[k].WareHouseToId, head.invContractWagesBillDateil[k].WareHouseTo, head.invContractWagesBillDateil[k].PurchaseGLAC, "", Conversion.ToBool((object)head.invContractWagesBillDateil[k].IsCompany), Conversion.ToInt((object)head.invContractWagesBillDateil[k].JobOrderId), head.invContractWagesBillDateil[k].RefDocQty, head.invContractWagesBillDateil[k].RefDocWeight, head.invContractWagesBillDateil[k].RefLineId);
					GrossWeight += head.invContractWagesBillDateil[k].Weight;
				}
			}
			accountName(head.RefDocumentTypeId);
			txtGrossWeight.Text = GrossWeight.ToString();
			grdwagesDetail.DataSource = dtdetail;
			grdwagesDetail.RetrieveStructure();
			UpdateMode = true;
			DetailGridSettings();
			if (dtStiching.Rows.Count == 0 && ((Conversion.ToInt(cmbReferenceDocType.Value) == 176 && !txtEntryType.Text.Contains("Issue")) || Conversion.ToInt(cmbReferenceDocType.Value) == 118))
			{
				dtStichingItems = CommonServices.GetWagesAccount("34", 0, 1);
				int j;
				for (j = 0; j < dtdetail.Rows.Count; j++)
				{
					if (!dtStiching.AsEnumerable().Any((DataRow row) => row.Field<int>("RefLineId") == Conversion.ToInt(dtdetail.Rows[j]["RefLineId"])) && Conversion.ToDouble(dtdetail.Rows[j]["PackSize"]) < 100.0)
					{
						dtStiching.Rows.Add(0, "", 0, "", dtdetail.Rows[j]["WagesType"], dtdetail.Rows[j]["Date"], dtdetail.Rows[j]["packingTypeId"], dtdetail.Rows[j]["packingType"], dtdetail.Rows[j]["Weight"], dtdetail.Rows[j]["PackSize"], dtdetail.Rows[j]["Quantity"], dtdetail.Rows[j]["WeightCut"], dtdetail.Rows[j]["BillQty"], dtdetail.Rows[j]["BillWeight"], 0, 0, 0, 0, dtdetail.Rows[j]["ItemId"], dtdetail.Rows[j]["Item"], dtdetail.Rows[j]["jobLotId"], dtdetail.Rows[j]["jobLot"], dtdetail.Rows[j]["Crop"], dtdetail.Rows[j]["MoveFromId"], dtdetail.Rows[j]["MoveFrom"], dtdetail.Rows[j]["MoveToId"], dtdetail.Rows[j]["MoveTo"], dtdetail.Rows[j]["PurchaseGLAC"], dtdetail.Rows[j]["WarehouseType"], dtdetail.Rows[j]["IsCompany"], dtdetail.Rows[j]["JobOrderId"], dtdetail.Rows[j]["RefDocQty"], dtdetail.Rows[j]["RefDocWeight"], dtdetail.Rows[j]["RefLineId"]);
					}
				}
			}
			else if (dtStiching.Rows.Count == 0 && Conversion.ToInt(cmbReferenceDocType.Value) == 220)
			{
				dtStichingItems = CommonServices.GetWagesAccount("34", 0, 1);
				int i;
				for (i = 0; i < dtdetail.Rows.Count; i++)
				{
					if (!dtStiching.AsEnumerable().Any((DataRow row) => row.Field<int>("RefLineId") == Conversion.ToInt(dtdetail.Rows[i]["RefLineId"])))
					{
						dtStiching.Rows.Add(0, "", 0, "", dtdetail.Rows[i]["WagesType"], dtdetail.Rows[i]["Date"], dtdetail.Rows[i]["packingTypeId"], dtdetail.Rows[i]["packingType"], dtdetail.Rows[i]["Weight"], dtdetail.Rows[i]["PackSize"], dtdetail.Rows[i]["Quantity"], dtdetail.Rows[i]["WeightCut"], dtdetail.Rows[i]["BillQty"], dtdetail.Rows[i]["BillWeight"], 0, 0, 0, 0, dtdetail.Rows[i]["ItemId"], dtdetail.Rows[i]["Item"], dtdetail.Rows[i]["jobLotId"], dtdetail.Rows[i]["jobLot"], dtdetail.Rows[i]["Crop"], dtdetail.Rows[i]["MoveFromId"], dtdetail.Rows[i]["MoveFrom"], dtdetail.Rows[i]["MoveToId"], dtdetail.Rows[i]["MoveTo"], dtdetail.Rows[i]["PurchaseGLAC"], dtdetail.Rows[i]["WarehouseType"], dtdetail.Rows[i]["IsCompany"], dtdetail.Rows[i]["JobOrderId"], dtdetail.Rows[i]["RefDocQty"], dtdetail.Rows[i]["RefDocWeight"], dtdetail.Rows[i]["RefLineId"]);
					}
				}
			}
			if (dtStiching.Rows.Count > 0)
			{
				tabControl1.TabPages.Add(tabPage4);
			}
			grdStiching.DataSource = dtStiching;
			grdStiching.RetrieveStructure();
			GridStichingSettings();
			grdDetailLockEditing();
			GridStichingLockEditing();
			StichingWagesConfig();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadOnlyMasterById(int ID)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		try
		{
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			InvContractorWagesBillHeader head = new InvContractorWagesBillHeader();
			RECID = ID;
			head = InvContractorWagesBillHeader.GetByID(RECID);
			if (head != null)
			{
				tabIncomingLabourwages.SelectedIndex = 0;
				DataTable dt2 = new DataTable();
				dt2.Columns.Add("StockPartyId");
				dt2.Columns.Add("StockPartyName");
				dt2.Rows.Add(head.StockPartyId, head.StockPartyName);
				if (dt2.Rows.Count > 0)
				{
					DDL.BindDDL(dt2, CmbStockParty, "StockPartyId", "StockPartyName", "Stock Party", ZeroIndex: false);
					((UltraGridBase)CmbStockParty).Rows[0].Activate();
				}
				DataTable dt = new DataTable();
				dt.Columns.Add("RefDocumentTypeId");
				dt.Columns.Add("DocumentTypeDescription");
				dt.Rows.Add(head.RefDocumentTypeId, head.DocumentTypeDescription);
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbReferenceDocType, "RefDocumentTypeId", "DocumentTypeDescription", "Document Type", ZeroIndex: false);
					((UltraGridBase)cmbReferenceDocType).Rows[0].Activate();
				}
				txtDocdate.Value = head.DocDate;
				txtdocnumber.Text = head.DocNo.ToString();
				txtDocNo.Text = head.RefDocNo.ToString();
				txtGRNId.Text = head.RefDocNoId.ToString();
				((Control)(object)cmbReferenceDocType).Enabled = false;
				txtGrossWeight.Text = head.WeightTotal.ToString();
				txtRemarks.Text = head.OtherRemarks;
				txtGpNo.Text = Conversion.ToString((object)head.ScaleSlipNo);
				txtEntryType.Text = Conversion.ToString((object)head.RefDocument);
				chkisapprove.Checked = head.IsAproved;
				PendingGrnAndGdn();
				LoadDataForWages();
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
			if (RefDocTypeId > 0 && RefDocId > 0)
			{
				tabIncomingLabourwages.SelectedIndex = 0;
			}
			else if (tabIncomingLabourwages.SelectedIndex == 1)
			{
				FromDateHistory.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = 219;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (FromDateHistory.Checked)
			{
				obj.FromDate = FromDateHistory.Value;
			}
			if (ToDateHistory.Checked)
			{
				obj.ToDate = ToDateHistory.Value;
			}
			obj.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			obj.RefDocumentTypeId = Conversion.ToInt(CmbDocumentTypeHistory.Value);
			DataTable dt = InvContractorWagesBillHeader.FormHistoryNew(obj);
			if (dt.Rows.Count > 0)
			{
				DataTable dtHist = new DataTable();
				dtHist.Columns.Add("Id", typeof(int));
				dtHist.Columns.Add("DocumentTypeId", typeof(int));
				dtHist.Columns.Add("RefDocumentTypeId", typeof(int));
				dtHist.Columns.Add("RefDocNoId", typeof(int));
				dtHist.Columns.Add("DocumentType", typeof(string));
				dtHist.Columns.Add("DocDate", typeof(DateTime));
				dtHist.Columns.Add("DocNo", typeof(int));
				dtHist.Columns.Add("RefDocNo", typeof(int));
				dtHist.Columns.Add("StockPartyId", typeof(int));
				dtHist.Columns.Add("StockParty", typeof(string));
				dtHist.Columns.Add("QtyTotal", typeof(double));
				dtHist.Columns.Add("WeightTotal", typeof(double));
				dtHist.Columns.Add("WagesAmount", typeof(double));
				dtHist.Columns.Add("OtherRemarks", typeof(string));
				dtHist.Columns.Add("JobOrderId", typeof(int));
				dtHist.Columns.Add("JobOrderNo", typeof(string));
				dtHist.Columns.Add("EntryDate", typeof(DateTime));
				dtHist.Columns.Add("EntryUser", typeof(string));
				dtHist.Columns.Add("ModifyDate", typeof(DateTime));
				dtHist.Columns.Add("ModifyUser", typeof(string));
				dtHist.Columns.Add("IsReferred", typeof(int));
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtHist.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["DocumentTypeId"], dt.Rows[i]["RefDocumentTypeId"], dt.Rows[i]["RefDocNoId"], dt.Rows[i]["DocumentTypeDescription"], dt.Rows[i]["DocDate"], dt.Rows[i]["DocNo"], dt.Rows[i]["RefDocNo"], dt.Rows[i]["StockPartyId"], dt.Rows[i]["StockParty"], dt.Rows[i]["QtyTotal"], dt.Rows[i]["WeightTotal"], dt.Rows[i]["WagesAmount"], dt.Rows[i]["OtherRemarks"], dt.Rows[i]["JobOrderId"], dt.Rows[i]["JobOrderNo"], dt.Rows[i]["EntryDate"], dt.Rows[i]["UserName"], dt.Rows[i]["ModifyDate"], dt.Rows[i]["ModifyUser"], dt.Rows[i]["IsReferred"]);
				}
				DataGridHistory.DataSource = dtHist;
				DataGridHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				DataGridHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void HistoryGridSettings()
	{
		try
		{
			DataGridHistory.AllowDelete = (InheritableBoolean)2;
			DataGridHistory.AllowEdit = (InheritableBoolean)2;
			DataGridHistory.ColumnAutoResize = false;
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["IsReferred"].Visible = false;
			DataGridHistory.RootTable.Columns["JobOrderId"].Visible = false;
			DataGridHistory.RootTable.Columns["StockPartyId"].Visible = false;
			DataGridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["RefDocNoId"].Visible = false;
			DataGridHistory.RootTable.Columns["QtyTotal"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["WeightTotal"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["WagesAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["QtyTotal"].FormatString = "#,#";
			DataGridHistory.RootTable.Columns["WeightTotal"].FormatString = "#,#";
			DataGridHistory.RootTable.Columns["WagesAmount"].FormatString = "#,#";
			DataGridHistory.RootTable.Columns["QtyTotal"].TotalFormatString = "#,#";
			DataGridHistory.RootTable.Columns["WeightTotal"].TotalFormatString = "#,#";
			DataGridHistory.RootTable.Columns["WagesAmount"].TotalFormatString = "#,#";
			DataGridHistory.RootTable.Columns["QtyTotal"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["WeightTotal"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["WagesAmount"].TextAlignment = (TextAlignment)3;
			CommonServices.GridAutoAdjustmentNew(DataGridHistory);
			DataGridHistory.RootTable.Columns.Add("Slip");
			DataGridHistory.RootTable.Columns["Slip"].Caption = "Slip";
			DataGridHistory.RootTable.Columns["Slip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Slip"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Slip"].ButtonText = "Slip";
			DataGridHistory.RootTable.Columns["Slip"].Width = 40;
			DataGridHistory.RootTable.Columns.Add("Edit");
			DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Width = 40;
			DataGridHistory.RootTable.Columns.Add("Voucher");
			DataGridHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
			DataGridHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			DataGridHistory.RootTable.Columns["Voucher"].Width = 60;
			DataGridHistory.RootTable.Columns["Edit"].Position = 0;
			DataGridHistory.RootTable.Columns["Slip"].Position = 1;
			DataGridHistory.RootTable.Columns["Voucher"].Position = 2;
			DataGridHistory.FrozenColumns = 3;
			ctrlGrdBar1_Load_1(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				if (Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["IsReferred"].Value.ToString()) == 1)
				{
					IsReferred = true;
				}
				else
				{
					IsReferred = false;
				}
				ReadById(Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString()));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			if (e.Column.Key == "Voucher")
			{
				CommonServices.VoucherReport_118(CommonServices.VoucherHeadIdGet(Conversion.ToInt(r.Cells["Id"].Value), 219));
			}
			if (e.Column.Key == "Slip")
			{
				SlipPrint_002(Conversion.ToInt((object)r.Cells["Id"].Value.ToString()));
			}
			if (e.Column.Key == "Edit")
			{
				if (Conversion.ToInt((object)r.Cells["IsReferred"].Value.ToString()) == 1)
				{
					IsReferred = true;
				}
				else
				{
					IsReferred = false;
				}
				ReadById(Conversion.ToInt((object)r.Cells["Id"].Value.ToString()));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryDetailGridBind(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			InvContractorWagesBillHeader head = new InvContractorWagesBillHeader();
			head = InvContractorWagesBillHeader.GetByID(Id);
			DataTable dtdetailHistory = new DataTable();
			dtdetailHistory.Columns.Add("HeaderId");
			dtdetailHistory.Columns.Add("ContractorName");
			dtdetailHistory.Columns.Add("WagesAccount");
			dtdetailHistory.Columns.Add("packingType");
			dtdetailHistory.Columns.Add("Weight", typeof(double));
			dtdetailHistory.Columns.Add("PackSize");
			dtdetailHistory.Columns.Add("Quantity", typeof(double));
			dtdetailHistory.Columns.Add("WeightCut");
			dtdetailHistory.Columns.Add("BillWeight", typeof(double));
			dtdetailHistory.Columns.Add("RateWithoutAddLess");
			dtdetailHistory.Columns.Add("RateAddLess", typeof(double));
			dtdetailHistory.Columns.Add("Rate");
			dtdetailHistory.Columns.Add("Amount", typeof(double));
			dtdetailHistory.Columns.Add("IsCompany", typeof(bool));
			for (int i = 0; i < head.invContractWagesBillDateil.Count; i++)
			{
				dtdetailHistory.Rows.Add(head.invContractWagesBillDateil[i].InvContractorWagesBillHeaderId, head.invContractWagesBillDateil[i].CompanyName, head.invContractWagesBillDateil[i].WagesAccountName, head.invContractWagesBillDateil[i].PackTypeDesc, head.invContractWagesBillDateil[i].Weight, head.invContractWagesBillDateil[i].PackSize, head.invContractWagesBillDateil[i].Qty, head.invContractWagesBillDateil[i].WeightCut, head.invContractWagesBillDateil[i].BillWeight, head.invContractWagesBillDateil[i].WageRate - head.invContractWagesBillDateil[i].RateAddLess, head.invContractWagesBillDateil[i].RateAddLess, head.invContractWagesBillDateil[i].WageRate, head.invContractWagesBillDateil[i].WagesAmount, head.invContractWagesBillDateil[i].IsCompany);
			}
			GrdHistoryDetail.DataSource = dtdetailHistory;
			GrdHistoryDetail.RetrieveStructure();
			HistoryDetailGridSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryDetailGridSettings()
	{
		try
		{
			GrdHistoryDetail.AllowDelete = (InheritableBoolean)2;
			GrdHistoryDetail.AllowEdit = (InheritableBoolean)2;
			GrdHistoryDetail.ColumnAutoResize = false;
			GrdHistoryDetail.RootTable.Columns["HeaderId"].Visible = false;
			GrdHistoryDetail.RootTable.Columns["RateAddLess"].Visible = EnableAddLessOnWagesRegular;
			GrdHistoryDetail.RootTable.Columns["RateWithoutAddLess"].Visible = EnableAddLessOnWagesRegular;
			GrdHistoryDetail.RootTable.Columns["RateAddLess"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["Quantity"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["BillWeight"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["RateAddLess"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["Quantity"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["BillWeight"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["RateAddLess"].FormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["RateWithoutAddLess"].FormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["Amount"].FormatString = "0,0";
			GrdHistoryDetail.RootTable.Columns["Weight"].FormatString = "0,0";
			GrdHistoryDetail.RootTable.Columns["Quantity"].FormatString = "0,0";
			GrdHistoryDetail.RootTable.Columns["BillWeight"].FormatString = "0,0";
			GrdHistoryDetail.RootTable.Columns["RateAddLess"].TotalFormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["RateWithoutAddLess"].TotalFormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["Amount"].TotalFormatString = "0,0";
			GrdHistoryDetail.RootTable.Columns["Weight"].TotalFormatString = "0,0";
			GrdHistoryDetail.RootTable.Columns["Quantity"].TotalFormatString = "0,0";
			GrdHistoryDetail.RootTable.Columns["BillWeight"].TotalFormatString = "0,0";
			CommonServices.GridAutoAdjustmentNew(GrdHistoryDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnLoadAll_Click(object sender, EventArgs e)
	{
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				HistoryDetailGridBind(Conversion.ToInt((object)item.Cells["Id"].Value.ToString()));
			}
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
			DocumentTypeFillForCombo();
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
			FromDateHistory.Value = DateTime.Now;
			ToDateHistory.Value = DateTime.Now;
			FromDocNoHistory.Text = string.Empty;
			ToDocNoHistory.Text = string.Empty;
			((Control)(object)CmbDocumentTypeHistory).Text = string.Empty;
			DataGridHistory.ClearStructure();
			GrdHistoryDetail.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			bindHistory();
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DataGridHistory).Name);
			if (fs != null)
			{
				DataGridHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar1_Load_1(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DataGridHistory).Name);
			if (fs != null)
			{
				DataGridHistory.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdwagesDetail).Name);
			if (fs != null)
			{
				grdwagesDetail.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdStiching).Name);
			if (fs != null)
			{
				grdStiching.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar5_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)gridPendingWagesSlip).Name);
			if (fs != null)
			{
				gridPendingWagesSlip.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar6_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdDataRetrieve).Name);
			if (fs != null)
			{
				grdDataRetrieve.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void frmWagesBillPartyProcessing_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				if (tabIncomingLabourwages.SelectedIndex == 0)
				{
					if (btnsave.Visible && btnsave.Enabled)
					{
						btnsave_Click(null, null);
					}
				}
				else if (tabIncomingLabourwages.SelectedIndex == 1)
				{
					btnShow_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				if (tabIncomingLabourwages.SelectedIndex == 0)
				{
					btnnew_Click(null, null);
				}
				else if (tabIncomingLabourwages.SelectedIndex == 1)
				{
					btnResetHistory_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (RefDocTypeId > 0 && RefDocId > 0)
				{
					tabIncomingLabourwages.SelectedIndex = 0;
					cmbReferenceDocType.Focus();
				}
				else if (tabIncomingLabourwages.SelectedIndex == 1)
				{
					tabIncomingLabourwages.SelectedIndex = 0;
					cmbReferenceDocType.Focus();
				}
				else
				{
					tabIncomingLabourwages.SelectedIndex = 1;
					FromDateHistory.Focus();
				}
			}
			if (((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape) && (RefDocTypeId <= 0 || RefDocId <= 0))
			{
				if (!ValidationOnformClose("Close"))
				{
					return;
				}
				Close();
			}
			if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				btnUpdate_Click(null, null);
			}
			if (Print.Visible && Print.Enabled && e.Control && e.KeyCode == Keys.P)
			{
				Print_Click(null, null);
			}
			if (tabIncomingLabourwages.SelectedIndex == 0 && e.Control && e.KeyCode == Keys.D && !IsReferred)
			{
				if (Conversion.ToString(base.ActiveControl.Tag) == "WagesGrid")
				{
					AddRowInGLGrid();
				}
				if (Conversion.ToString(base.ActiveControl.Tag) == "StichingGrid")
				{
					AddRowInStichingGrid();
				}
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				if (tabIncomingLabourwages.SelectedIndex == 0)
				{
					((Control)(object)gridPendingWagesSlip).Focus();
				}
				else if (tabIncomingLabourwages.SelectedIndex == 1)
				{
					((Control)(object)DataGridHistory).Focus();
				}
			}
			if (tabIncomingLabourwages.SelectedIndex == 0 && e.Control && e.KeyCode == Keys.F5)
			{
				txtDocdate.Focus();
			}
			if (tabIncomingLabourwages.SelectedIndex == 0 && e.Control && e.KeyCode == Keys.L)
			{
				LoadDataForWages();
				((Control)(object)grdwagesDetail).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				if (tabIncomingLabourwages.SelectedIndex == 0)
				{
					((Control)(object)grdwagesDetail).Focus();
				}
				else if (tabIncomingLabourwages.SelectedIndex == 1)
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

	private void Print_Click(object sender, EventArgs e)
	{
		try
		{
			SlipPrint_002(RECID);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SlipPrint_002(int PrintId)
	{
		try
		{
			CommonServices.ContractorWagesPartyProcessingSlip(PrintId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnWagesSchedule_Click(object sender, EventArgs e)
	{
		try
		{
			frmContractWagesSchedule frmContractWagesSchedule = new frmContractWagesSchedule(UserAccount);
			frmContractWagesSchedule.Show();
			frmContractWagesSchedule.BringToFront();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnWagesExempt_Click(object sender, EventArgs e)
	{
		try
		{
			new WagesExemptItemSchedule(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddContractorValuesForAllDetailWages_Click(object sender, EventArgs e)
	{
		try
		{
			AddContractorValuesForAllInGrid(grdwagesDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddContractorValuesForAllInGrid(GridEX grid)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grid.CurrentRow == null || (int)grid.CurrentRow.RowType != 0 || grid.CurrentRow.Cells.Count <= 0 || grid.GetRows().Length == 0)
			{
				return;
			}
			GridEXRow row = grid.GetRow(0);
			int ContractorValue = Conversion.ToInt(row.Cells["SupplierId"].Value);
			int WagesAccountValue = Conversion.ToInt(row.Cells["WagesId"].Value);
			if (ContractorValue != 0 || WagesAccountValue != 0)
			{
				GridEXRow[] rows = grid.GetRows();
				foreach (GridEXRow r in rows)
				{
					r.BeginEdit();
					r.Cells["SupplierId"].Value = ContractorValue;
					r.Cells["WagesId"].Value = WagesAccountValue;
					r.Cells["WagesId"].Value = WagesAccountValue;
					double WagesPackSize = Conversion.ToDouble(r.Cells["PackSize"].Value);
					double WagesRate = 0.0;
					GetWagesSchedule d = CommonServices.GetWagesRate(Conversion.ToDateTime(r.Cells["Date"].Value), WagesPackSize, Conversion.ToInt(r.Cells["WagesId"].Value), Conversion.ToInt(r.Cells["SupplierId"].Value));
					if (d != null)
					{
						WagesRate = d.WagesRate;
					}
					if (WagesRate > 0.0)
					{
						r.Cells["Rate"].Value = WagesRate;
						if (Conversion.ToDouble(r.Cells["Weight"].Value) > 0.0 && Conversion.ToDouble(r.Cells["PackSize"].Value) > 0.0)
						{
							double num = Conversion.ToDouble(r.Cells["Weight"].Value);
							double b = Conversion.ToDouble(r.Cells["PackSize"].Value);
							_ = num / b;
							double Weight = Conversion.ToDouble(r.Cells["Weight"].Value);
							r.Cells["BillWeight"].Value = Math.Round(Weight, 2);
						}
						if (Conversion.ToDouble(r.Cells["Quantity"].Value) > 0.0 && Conversion.ToDouble(r.Cells["WeightCut"].Value) > 0.0 && Conversion.ToDouble(r.Cells["PackSize"].Value) > 0.0 && Conversion.ToDouble(r.Cells["Weight"].Value) > 0.0)
						{
							double num2 = Conversion.ToDouble(r.Cells["WeightCut"].Value);
							double Qty = Conversion.ToDouble(r.Cells["Quantity"].Value);
							double WeightCut = num2 * Qty;
							double BillWeight = Conversion.ToDouble(r.Cells["Weight"].Value) - Math.Abs(WeightCut);
							r.Cells["BillWeight"].Value = Math.Round(BillWeight, 2);
						}
						if (Conversion.ToDouble(r.Cells["Rate"].Value) > 0.0 && Conversion.ToDouble(r.Cells["BillWeight"].Value) > 0.0)
						{
							double Packsize = Conversion.ToDouble(r.Cells["PackSize"].Value);
							double num3 = Conversion.ToDouble(r.Cells["BillWeight"].Value);
							double Rate = Conversion.ToDouble(r.Cells["Rate"].Value);
							double Amount = num3 / Packsize * Rate;
							r.Cells["Amount"].Value = Math.Round(Amount, 2);
						}
						else
						{
							r.Cells["Amount"].Value = 0;
						}
					}
					else
					{
						r.Cells["Rate"].Value = 0;
					}
					r.EndEdit();
				}
			}
			else
			{
				MessageBox.Show("First row contains No values Of Contractor And Wages Account.");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddContractorValuesForAllSticingWages_Click(object sender, EventArgs e)
	{
		try
		{
			AddContractorValuesForAllInGrid(grdStiching);
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
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Expected O, but got Unknown
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Expected O, but got Unknown
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Expected O, but got Unknown
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Expected O, but got Unknown
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Expected O, but got Unknown
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Expected O, but got Unknown
		//IL_044c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0456: Expected O, but got Unknown
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Expected O, but got Unknown
		//IL_04db: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e5: Expected O, but got Unknown
		//IL_0c7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c87: Expected O, but got Unknown
		//IL_151d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1527: Expected O, but got Unknown
		//IL_1534: Unknown result type (might be due to invalid IL or missing references)
		//IL_153e: Expected O, but got Unknown
		//IL_154b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1555: Expected O, but got Unknown
		//IL_1aa5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aaf: Expected O, but got Unknown
		//IL_1abc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ac6: Expected O, but got Unknown
		//IL_1ad3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1add: Expected O, but got Unknown
		//IL_3e1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e24: Expected O, but got Unknown
		GridEXLayout gridPendingWagesSlip_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDataRetrieve_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdwagesDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdStiching_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.PartyProcessing.frmWagesBillPartyProcessing));
		GridEXLayout GrdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabIncomingLabourwages = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage5 = new System.Windows.Forms.TabPage();
		this.panel18 = new System.Windows.Forms.Panel();
		this.gridPendingWagesSlip = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		this.label7 = new System.Windows.Forms.Label();
		this.tabPage6 = new System.Windows.Forms.TabPage();
		this.panel19 = new System.Windows.Forms.Panel();
		this.grdDataRetrieve = new GridEX();
		this.panel20 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar6 = new grid.User_Conrtols.CtrlGrdBar();
		this.label8 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel13 = new System.Windows.Forms.Panel();
		this.grdwagesDetail = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label12 = new System.Windows.Forms.Label();
		this.btnAddContractorValuesForAllDetailWages = new System.Windows.Forms.Button();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label2 = new System.Windows.Forms.Label();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.panel16 = new System.Windows.Forms.Panel();
		this.grdStiching = new GridEX();
		this.panel17 = new System.Windows.Forms.Panel();
		this.label13 = new System.Windows.Forms.Label();
		this.btnAddContractorValuesForAllSticingWages = new System.Windows.Forms.Button();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.label5 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.txtDocdate = new System.Windows.Forms.DateTimePicker();
		this.label9 = new System.Windows.Forms.Label();
		this.chkisapprove = new System.Windows.Forms.CheckBox();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.txtdocnumber = new System.Windows.Forms.TextBox();
		this.label29 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbStockParty = new UltraCombo();
		this.cmbReferenceDocType = new UltraCombo();
		this.label14 = new System.Windows.Forms.Label();
		this.txtGpNo = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.txtDocNo = new System.Windows.Forms.TextBox();
		this.txtEntryType = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtGRNId = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.btnWagesSchedule = new System.Windows.Forms.ToolStripButton();
		this.btnWagesExempt = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel21 = new System.Windows.Forms.Panel();
		this.GrdHistoryDetail = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label10 = new System.Windows.Forms.Label();
		this.panel14 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.btnShow = new System.Windows.Forms.Button();
		this.label50 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.CmbDocumentTypeHistory = new UltraCombo();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel10 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label26 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		this.panel1.SuspendLayout();
		this.tabIncomingLabourwages.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel5.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage5.SuspendLayout();
		this.panel18.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.gridPendingWagesSlip).BeginInit();
		this.panel8.SuspendLayout();
		this.tabPage6.SuspendLayout();
		this.panel19.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDataRetrieve).BeginInit();
		this.panel20.SuspendLayout();
		this.panel6.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdwagesDetail).BeginInit();
		this.panel15.SuspendLayout();
		this.tabPage4.SuspendLayout();
		this.panel16.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdStiching).BeginInit();
		this.panel17.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbStockParty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbReferenceDocType).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel21.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryDetail).BeginInit();
		this.panel12.SuspendLayout();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel11.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDocumentTypeHistory).BeginInit();
		this.panel10.SuspendLayout();
		this.panel9.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabIncomingLabourwages);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(964, 661);
		this.panel1.TabIndex = 0;
		this.tabIncomingLabourwages.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabIncomingLabourwages.Controls.Add(this.tabPage1);
		this.tabIncomingLabourwages.Controls.Add(this.tabPage2);
		this.tabIncomingLabourwages.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabIncomingLabourwages.Location = new System.Drawing.Point(0, 0);
		this.tabIncomingLabourwages.Name = "tabIncomingLabourwages";
		this.tabIncomingLabourwages.SelectedIndex = 0;
		this.tabIncomingLabourwages.Size = new System.Drawing.Size(964, 661);
		this.tabIncomingLabourwages.TabIndex = 0;
		this.tabIncomingLabourwages.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.BackColor = System.Drawing.Color.White;
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(956, 635);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.panel5.Controls.Add(this.tabControl2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 453);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(950, 179);
		this.panel5.TabIndex = 9;
		this.tabControl2.Controls.Add(this.tabPage5);
		this.tabControl2.Controls.Add(this.tabPage6);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(950, 179);
		this.tabControl2.TabIndex = 0;
		this.tabPage5.Controls.Add(this.panel18);
		this.tabPage5.Controls.Add(this.panel8);
		this.tabPage5.Location = new System.Drawing.Point(4, 25);
		this.tabPage5.Name = "tabPage5";
		this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage5.Size = new System.Drawing.Size(942, 150);
		this.tabPage5.TabIndex = 0;
		this.tabPage5.Text = "Pending Data For Load";
		this.tabPage5.UseVisualStyleBackColor = true;
		this.panel18.Controls.Add((System.Windows.Forms.Control)(object)this.gridPendingWagesSlip);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel18.Location = new System.Drawing.Point(3, 28);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(936, 119);
		this.panel18.TabIndex = 10;
		this.gridPendingWagesSlip.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.gridPendingWagesSlip).BackColor = System.Drawing.Color.White;
		gridPendingWagesSlip_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.gridPendingWagesSlip.DesignTimeLayout = gridPendingWagesSlip_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.gridPendingWagesSlip).Dock = System.Windows.Forms.DockStyle.Fill;
		this.gridPendingWagesSlip.FilterMode = (FilterMode)2;
		this.gridPendingWagesSlip.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.gridPendingWagesSlip).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.gridPendingWagesSlip.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.gridPendingWagesSlip).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.gridPendingWagesSlip).Name = "gridPendingWagesSlip";
		((ScrollableBase)this.gridPendingWagesSlip).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.gridPendingWagesSlip).Size = new System.Drawing.Size(936, 119);
		((System.Windows.Forms.Control)(object)this.gridPendingWagesSlip).TabIndex = 0;
		this.gridPendingWagesSlip.TotalRow = (InheritableBoolean)1;
		this.gridPendingWagesSlip.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.gridPendingWagesSlip).VisualStyle = (VisualStyle)4;
		this.gridPendingWagesSlip.ColumnButtonClick += new ColumnActionEventHandler(gridPendingWagesSlip_ColumnButtonClick);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.ctrlGrdBar5);
		this.panel8.Controls.Add(this.label7);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(936, 25);
		this.panel8.TabIndex = 9;
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = this;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(889, 1);
		this.ctrlGrdBar5.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar5.MyGrid = this.gridPendingWagesSlip;
		this.ctrlGrdBar5.Name = "ctrlGrdBar5";
		this.ctrlGrdBar5.Size = new System.Drawing.Size(45, 23);
		this.ctrlGrdBar5.TabIndex = 63;
		this.ctrlGrdBar5.Load += new System.EventHandler(ctrlGrdBar5_Load);
		this.label7.AutoSize = true;
		this.label7.BackColor = System.Drawing.Color.Teal;
		this.label7.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label7.Location = new System.Drawing.Point(3, 3);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(394, 16);
		this.label7.TabIndex = 0;
		this.label7.Text = "This grid shows the pending data to load for wages entries.";
		this.tabPage6.Controls.Add(this.panel19);
		this.tabPage6.Controls.Add(this.panel20);
		this.tabPage6.Location = new System.Drawing.Point(4, 25);
		this.tabPage6.Name = "tabPage6";
		this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage6.Size = new System.Drawing.Size(942, 150);
		this.tabPage6.TabIndex = 1;
		this.tabPage6.Text = "Wages Detail";
		this.tabPage6.UseVisualStyleBackColor = true;
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.grdDataRetrieve);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel19.Location = new System.Drawing.Point(3, 30);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(936, 117);
		this.panel19.TabIndex = 12;
		this.grdDataRetrieve.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDataRetrieve).BackColor = System.Drawing.Color.White;
		grdDataRetrieve_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDataRetrieve.DesignTimeLayout = grdDataRetrieve_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDataRetrieve).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDataRetrieve.FilterMode = (FilterMode)2;
		this.grdDataRetrieve.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDataRetrieve).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDataRetrieve.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDataRetrieve).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDataRetrieve).Name = "grdDataRetrieve";
		((ScrollableBase)this.grdDataRetrieve).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDataRetrieve).Size = new System.Drawing.Size(936, 117);
		((System.Windows.Forms.Control)(object)this.grdDataRetrieve).TabIndex = 2;
		this.grdDataRetrieve.TotalRow = (InheritableBoolean)1;
		this.grdDataRetrieve.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDataRetrieve).VisualStyle = (VisualStyle)4;
		this.panel20.BackColor = System.Drawing.Color.Teal;
		this.panel20.Controls.Add(this.ctrlGrdBar6);
		this.panel20.Controls.Add(this.label8);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(3, 3);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(936, 27);
		this.panel20.TabIndex = 11;
		this.ctrlGrdBar6.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar6.FormName = this;
		this.ctrlGrdBar6.Location = new System.Drawing.Point(889, 1);
		this.ctrlGrdBar6.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar6.MyGrid = this.grdDataRetrieve;
		this.ctrlGrdBar6.Name = "ctrlGrdBar6";
		this.ctrlGrdBar6.Size = new System.Drawing.Size(45, 24);
		this.ctrlGrdBar6.TabIndex = 63;
		this.ctrlGrdBar6.Load += new System.EventHandler(ctrlGrdBar6_Load);
		this.label8.AutoSize = true;
		this.label8.BackColor = System.Drawing.Color.Teal;
		this.label8.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label8.Location = new System.Drawing.Point(3, 3);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(428, 16);
		this.label8.TabIndex = 0;
		this.label8.Text = "This grid shows the previous entry details against this document";
		this.panel6.Controls.Add(this.tabControl1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 162);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(950, 291);
		this.panel6.TabIndex = 8;
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Controls.Add(this.tabPage4);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(950, 291);
		this.tabControl1.TabIndex = 0;
		this.tabPage3.Controls.Add(this.panel13);
		this.tabPage3.Controls.Add(this.panel15);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(942, 262);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Regular Wages";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.grdwagesDetail);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(3, 30);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(936, 229);
		this.panel13.TabIndex = 7;
		((ScrollableBase)this.grdwagesDetail).BackColor = System.Drawing.Color.White;
		grdwagesDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdwagesDetail.DesignTimeLayout = grdwagesDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdwagesDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdwagesDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdwagesDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdwagesDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdwagesDetail).Name = "grdwagesDetail";
		((ScrollableBase)this.grdwagesDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdwagesDetail).Size = new System.Drawing.Size(936, 229);
		((System.Windows.Forms.Control)(object)this.grdwagesDetail).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdwagesDetail).Tag = "WagesGrid";
		this.grdwagesDetail.TotalRow = (InheritableBoolean)1;
		this.grdwagesDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdwagesDetail).VisualStyle = (VisualStyle)4;
		this.grdwagesDetail.EditingCell += new EditingCellEventHandler(grdwagesDetail_EditingCell);
		this.grdwagesDetail.CellUpdated += new ColumnActionEventHandler(grdwagesDetail_CellUpdated);
		this.grdwagesDetail.ColumnButtonClick += new ColumnActionEventHandler(grdwagesDetail_ColumnButtonClick);
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.label12);
		this.panel15.Controls.Add(this.btnAddContractorValuesForAllDetailWages);
		this.panel15.Controls.Add(this.ctrlGrdBar2);
		this.panel15.Controls.Add(this.label2);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(3, 3);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(936, 27);
		this.panel15.TabIndex = 6;
		this.label12.AutoSize = true;
		this.label12.BackColor = System.Drawing.Color.Teal;
		this.label12.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label12.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label12.Location = new System.Drawing.Point(449, 6);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(389, 14);
		this.label12.TabIndex = 68;
		this.label12.Text = "Click this button To Apply First Row Values for All other Rows";
		this.btnAddContractorValuesForAllDetailWages.BackColor = System.Drawing.Color.Green;
		this.btnAddContractorValuesForAllDetailWages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddContractorValuesForAllDetailWages.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddContractorValuesForAllDetailWages.ForeColor = System.Drawing.Color.White;
		this.btnAddContractorValuesForAllDetailWages.Location = new System.Drawing.Point(359, 0);
		this.btnAddContractorValuesForAllDetailWages.Name = "btnAddContractorValuesForAllDetailWages";
		this.btnAddContractorValuesForAllDetailWages.Size = new System.Drawing.Size(84, 26);
		this.btnAddContractorValuesForAllDetailWages.TabIndex = 67;
		this.btnAddContractorValuesForAllDetailWages.Text = "Apply All";
		this.btnAddContractorValuesForAllDetailWages.UseVisualStyleBackColor = false;
		this.btnAddContractorValuesForAllDetailWages.Click += new System.EventHandler(btnAddContractorValuesForAllDetailWages_Click);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(889, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdwagesDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 23);
		this.ctrlGrdBar2.TabIndex = 63;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.Teal;
		this.label2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label2.Location = new System.Drawing.Point(3, 6);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(267, 16);
		this.label2.TabIndex = 0;
		this.label2.Text = "In this Grid User Will add Regular Wages";
		this.tabPage4.Controls.Add(this.panel16);
		this.tabPage4.Controls.Add(this.panel17);
		this.tabPage4.Location = new System.Drawing.Point(4, 25);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(942, 262);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "Other Wages";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.panel16.Controls.Add((System.Windows.Forms.Control)(object)this.grdStiching);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel16.Location = new System.Drawing.Point(3, 28);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(936, 231);
		this.panel16.TabIndex = 9;
		((ScrollableBase)this.grdStiching).BackColor = System.Drawing.Color.White;
		grdStiching_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdStiching.DesignTimeLayout = grdStiching_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdStiching).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdStiching).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdStiching.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdStiching).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdStiching).Name = "grdStiching";
		((ScrollableBase)this.grdStiching).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdStiching).Size = new System.Drawing.Size(936, 231);
		((System.Windows.Forms.Control)(object)this.grdStiching).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.grdStiching).Tag = "StichingGrid";
		this.grdStiching.TotalRow = (InheritableBoolean)1;
		this.grdStiching.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdStiching).VisualStyle = (VisualStyle)4;
		this.grdStiching.EditingCell += new EditingCellEventHandler(grdStiching_EditingCell);
		this.grdStiching.CellUpdated += new ColumnActionEventHandler(grdStiching_CellUpdated);
		this.grdStiching.ColumnButtonClick += new ColumnActionEventHandler(grdStiching_ColumnButtonClick);
		this.panel17.BackColor = System.Drawing.Color.Teal;
		this.panel17.Controls.Add(this.label13);
		this.panel17.Controls.Add(this.btnAddContractorValuesForAllSticingWages);
		this.panel17.Controls.Add(this.ctrlGrdBar4);
		this.panel17.Controls.Add(this.label5);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(3, 3);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(936, 25);
		this.panel17.TabIndex = 8;
		this.label13.AutoSize = true;
		this.label13.BackColor = System.Drawing.Color.Teal;
		this.label13.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label13.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label13.Location = new System.Drawing.Point(491, 3);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(389, 14);
		this.label13.TabIndex = 70;
		this.label13.Text = "Click this button To Apply First Row Values for All other Rows";
		this.btnAddContractorValuesForAllSticingWages.BackColor = System.Drawing.Color.Green;
		this.btnAddContractorValuesForAllSticingWages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddContractorValuesForAllSticingWages.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddContractorValuesForAllSticingWages.ForeColor = System.Drawing.Color.White;
		this.btnAddContractorValuesForAllSticingWages.Location = new System.Drawing.Point(402, -1);
		this.btnAddContractorValuesForAllSticingWages.Name = "btnAddContractorValuesForAllSticingWages";
		this.btnAddContractorValuesForAllSticingWages.Size = new System.Drawing.Size(84, 27);
		this.btnAddContractorValuesForAllSticingWages.TabIndex = 69;
		this.btnAddContractorValuesForAllSticingWages.Text = "Apply All";
		this.btnAddContractorValuesForAllSticingWages.UseVisualStyleBackColor = false;
		this.btnAddContractorValuesForAllSticingWages.Click += new System.EventHandler(btnAddContractorValuesForAllSticingWages_Click);
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(889, 1);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar4.MyGrid = this.grdStiching;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(45, 23);
		this.ctrlGrdBar4.TabIndex = 63;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.label5.AutoSize = true;
		this.label5.BackColor = System.Drawing.Color.Teal;
		this.label5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label5.Location = new System.Drawing.Point(3, 3);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(399, 16);
		this.label5.TabIndex = 0;
		this.label5.Text = "In this Grid User Will add Other Wages (Stiching & OutPut etc)";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add(this.txtEntryType);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.txtGRNId);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel4.Location = new System.Drawing.Point(3, 54);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(950, 108);
		this.panel4.TabIndex = 0;
		this.groupBox1.Controls.Add(this.txtRemarks);
		this.groupBox1.Controls.Add(this.txtDocdate);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.chkisapprove);
		this.groupBox1.Controls.Add(this.txtQty);
		this.groupBox1.Controls.Add(this.txtGrossWeight);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.txtdocnumber);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbStockParty);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbReferenceDocType);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.txtGpNo);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.txtDocNo);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(1, 4);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(945, 100);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main Information";
		this.txtRemarks.Location = new System.Drawing.Point(115, 72);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(763, 23);
		this.txtRemarks.TabIndex = 4;
		this.txtDocdate.Enabled = false;
		this.txtDocdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocdate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtDocdate.Location = new System.Drawing.Point(115, 17);
		this.txtDocdate.Name = "txtDocdate";
		this.txtDocdate.Size = new System.Drawing.Size(174, 23);
		this.txtDocdate.TabIndex = 0;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(397, 49);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(55, 15);
		this.label9.TabIndex = 116;
		this.label9.Text = "Total Qty";
		this.chkisapprove.AutoSize = true;
		this.chkisapprove.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkisapprove.Location = new System.Drawing.Point(788, 47);
		this.chkisapprove.Name = "chkisapprove";
		this.chkisapprove.Size = new System.Drawing.Size(90, 19);
		this.chkisapprove.TabIndex = 9;
		this.chkisapprove.Text = "Is Approved";
		this.chkisapprove.UseVisualStyleBackColor = true;
		this.chkisapprove.Visible = false;
		this.txtQty.BackColor = System.Drawing.Color.White;
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Enabled = false;
		this.txtQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQty.Location = new System.Drawing.Point(466, 45);
		this.txtQty.Name = "txtQty";
		this.txtQty.ReadOnly = true;
		this.txtQty.Size = new System.Drawing.Size(86, 23);
		this.txtQty.TabIndex = 6;
		this.txtGrossWeight.BackColor = System.Drawing.Color.White;
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Enabled = false;
		this.txtGrossWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtGrossWeight.Location = new System.Drawing.Point(635, 45);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.ReadOnly = true;
		this.txtGrossWeight.Size = new System.Drawing.Size(101, 23);
		this.txtGrossWeight.TabIndex = 7;
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.White;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(5, 76);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(52, 15);
		this.label6.TabIndex = 59;
		this.label6.Text = "Remarks";
		this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtdocnumber.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtdocnumber.Location = new System.Drawing.Point(292, 17);
		this.txtdocnumber.Name = "txtdocnumber";
		this.txtdocnumber.ReadOnly = true;
		this.txtdocnumber.Size = new System.Drawing.Size(101, 23);
		this.txtdocnumber.TabIndex = 1;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(555, 49);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(78, 15);
		this.label29.TabIndex = 81;
		this.label29.Text = "Gross Weight";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(5, 21);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(88, 15);
		this.label1.TabIndex = 24;
		this.label1.Text = "Doc Date && No";
		this.CmbStockParty.AllowNull = (DefaultableBoolean)1;
		this.CmbStockParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbStockParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbStockParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbStockParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.CmbStockParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance35).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbStockParty).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbStockParty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbStockParty).Location = new System.Drawing.Point(466, 15);
		((System.Windows.Forms.Control)(object)this.CmbStockParty).Name = "CmbStockParty";
		((System.Windows.Forms.Control)(object)this.CmbStockParty).Size = new System.Drawing.Size(270, 26);
		((System.Windows.Forms.Control)(object)this.CmbStockParty).TabIndex = 5;
		this.cmbReferenceDocType.AllowNull = (DefaultableBoolean)1;
		this.cmbReferenceDocType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbReferenceDocType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbReferenceDocType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbReferenceDocType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbReferenceDocType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbReferenceDocType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbReferenceDocType).Location = new System.Drawing.Point(115, 43);
		((System.Windows.Forms.Control)(object)this.cmbReferenceDocType).Name = "cmbReferenceDocType";
		((System.Windows.Forms.Control)(object)this.cmbReferenceDocType).Size = new System.Drawing.Size(174, 26);
		((System.Windows.Forms.Control)(object)this.cmbReferenceDocType).TabIndex = 2;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(397, 21);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(67, 15);
		this.label14.TabIndex = 106;
		this.label14.Text = "Stock Party";
		this.txtGpNo.BackColor = System.Drawing.Color.White;
		this.txtGpNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGpNo.Enabled = false;
		this.txtGpNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGpNo.Location = new System.Drawing.Point(780, 17);
		this.txtGpNo.Name = "txtGpNo";
		this.txtGpNo.ReadOnly = true;
		this.txtGpNo.Size = new System.Drawing.Size(98, 23);
		this.txtGpNo.TabIndex = 8;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(5, 49);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(108, 15);
		this.label11.TabIndex = 106;
		this.label11.Text = "Ref Doc Type && No";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(739, 21);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(41, 15);
		this.label27.TabIndex = 110;
		this.label27.Text = "Gp No";
		this.txtDocNo.BackColor = System.Drawing.Color.White;
		this.txtDocNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNo.Enabled = false;
		this.txtDocNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDocNo.Location = new System.Drawing.Point(292, 45);
		this.txtDocNo.Name = "txtDocNo";
		this.txtDocNo.ReadOnly = true;
		this.txtDocNo.Size = new System.Drawing.Size(101, 23);
		this.txtDocNo.TabIndex = 3;
		this.txtEntryType.BackColor = System.Drawing.Color.White;
		this.txtEntryType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEntryType.Enabled = false;
		this.txtEntryType.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEntryType.Location = new System.Drawing.Point(781, 107);
		this.txtEntryType.Name = "txtEntryType";
		this.txtEntryType.ReadOnly = true;
		this.txtEntryType.Size = new System.Drawing.Size(98, 23);
		this.txtEntryType.TabIndex = 113;
		this.txtEntryType.Visible = false;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(702, 111);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(59, 15);
		this.label4.TabIndex = 114;
		this.label4.Text = "EntryType";
		this.label4.Visible = false;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(702, 137);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(44, 15);
		this.label3.TabIndex = 112;
		this.label3.Text = "GRN Id";
		this.label3.Visible = false;
		this.txtGRNId.BackColor = System.Drawing.Color.White;
		this.txtGRNId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGRNId.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtGRNId.Location = new System.Drawing.Point(781, 133);
		this.txtGRNId.Name = "txtGRNId";
		this.txtGRNId.ReadOnly = true;
		this.txtGRNId.Size = new System.Drawing.Size(98, 23);
		this.txtGRNId.TabIndex = 111;
		this.txtGRNId.Visible = false;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label15);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 32);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(950, 22);
		this.panel3.TabIndex = 5;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.Teal;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(3, 3);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(124, 19);
		this.label15.TabIndex = 0;
		this.label15.Text = "Labour Wages";
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(950, 29);
		this.panel2.TabIndex = 4;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.btnnew, this.btnRefresh, this.btnsave, this.btnUpdate, this.Print, this.btnWagesSchedule, this.btnWagesExempt });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(950, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(60, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(81, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(65, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(79, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(117, 24);
		this.Print.Text = "002_01-&Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.btnWagesSchedule.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnWagesSchedule.Name = "btnWagesSchedule";
		this.btnWagesSchedule.Size = new System.Drawing.Size(120, 24);
		this.btnWagesSchedule.Text = "Wages Schedule";
		this.btnWagesSchedule.Click += new System.EventHandler(btnWagesSchedule_Click);
		this.btnWagesExempt.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnWagesExempt.Name = "btnWagesExempt";
		this.btnWagesExempt.Size = new System.Drawing.Size(143, 24);
		this.btnWagesExempt.Text = "Wages Exempt Item";
		this.btnWagesExempt.Click += new System.EventHandler(btnWagesExempt_Click);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(956, 635);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel7.Controls.Add(this.panel21);
		this.panel7.Controls.Add(this.panel12);
		this.panel7.Controls.Add(this.panel14);
		this.panel7.Controls.Add(this.panel11);
		this.panel7.Controls.Add(this.panel10);
		this.panel7.Controls.Add(this.panel9);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(3, 3);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(950, 629);
		this.panel7.TabIndex = 0;
		this.panel21.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistoryDetail);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel21.Location = new System.Drawing.Point(0, 495);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(950, 134);
		this.panel21.TabIndex = 70;
		this.GrdHistoryDetail.AllowEdit = (InheritableBoolean)2;
		GrdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistoryDetail.DesignTimeLayout = GrdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdHistoryDetail.DynamicFiltering = true;
		this.GrdHistoryDetail.FilterMode = (FilterMode)2;
		this.GrdHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Name = "GrdHistoryDetail";
		((ScrollableBase)this.GrdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Size = new System.Drawing.Size(950, 134);
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).TabIndex = 66;
		this.GrdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.GrdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label10);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 470);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(950, 25);
		this.panel12.TabIndex = 69;
		this.label10.AutoSize = true;
		this.label10.BackColor = System.Drawing.Color.Teal;
		this.label10.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label10.Location = new System.Drawing.Point(3, 4);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(324, 16);
		this.label10.TabIndex = 1;
		this.label10.Text = "This grid shows the Detail of above Selected row";
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 132);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(950, 338);
		this.panel14.TabIndex = 68;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(950, 338);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 63;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		this.panel11.Controls.Add(this.groupBox2);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 56);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(950, 76);
		this.panel11.TabIndex = 67;
		this.groupBox2.Controls.Add(this.btnShow);
		this.groupBox2.Controls.Add(this.label50);
		this.groupBox2.Controls.Add(this.label51);
		this.groupBox2.Controls.Add(this.label52);
		this.groupBox2.Controls.Add(this.label53);
		this.groupBox2.Controls.Add(this.label54);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDocumentTypeHistory);
		this.groupBox2.Controls.Add(this.ToDocNoHistory);
		this.groupBox2.Controls.Add(this.FromDocNoHistory);
		this.groupBox2.Controls.Add(this.ToDateHistory);
		this.groupBox2.Controls.Add(this.FromDateHistory);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(5, 3);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(762, 68);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(698, 38);
		this.btnShow.Margin = new System.Windows.Forms.Padding(2);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(58, 26);
		this.btnShow.TabIndex = 5;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(459, 19);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(92, 15);
		this.label50.TabIndex = 87;
		this.label50.Text = "Document Type";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(372, 19);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(64, 15);
		this.label51.TabIndex = 86;
		this.label51.Text = "To Doc No";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(289, 19);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(79, 15);
		this.label52.TabIndex = 85;
		this.label52.Text = "From Doc No";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(149, 19);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(48, 15);
		this.label53.TabIndex = 84;
		this.label53.Text = "To Date";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(5, 19);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(63, 15);
		this.label54.TabIndex = 83;
		this.label54.Text = "From Date";
		this.CmbDocumentTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbDocumentTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDocumentTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDocumentTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance29).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance30).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDocumentTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDocumentTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDocumentTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDocumentTypeHistory).Location = new System.Drawing.Point(459, 38);
		((System.Windows.Forms.Control)(object)this.CmbDocumentTypeHistory).Name = "CmbDocumentTypeHistory";
		((System.Windows.Forms.Control)(object)this.CmbDocumentTypeHistory).Size = new System.Drawing.Size(236, 26);
		((System.Windows.Forms.Control)(object)this.CmbDocumentTypeHistory).TabIndex = 4;
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(372, 40);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(85, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(289, 40);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(81, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(149, 40);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(138, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(5, 40);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(143, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.ctrlGrdBar1);
		this.panel10.Controls.Add(this.label26);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 29);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(950, 27);
		this.panel10.TabIndex = 61;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(911, 3);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 62;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load_1);
		this.label26.AutoSize = true;
		this.label26.BackColor = System.Drawing.Color.Teal;
		this.label26.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label26.Location = new System.Drawing.Point(6, 3);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(187, 19);
		this.label26.TabIndex = 0;
		this.label26.Text = "Labour Wages History";
		this.panel9.Controls.Add(this.toolStrip2);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(950, 29);
		this.panel9.TabIndex = 0;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.toolStripSeparator1, this.btnResetHistory, this.btnRefreshHistory, this.btnLoadAll });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(950, 27);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnResetHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnResetHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetHistory.Name = "btnResetHistory";
		this.btnResetHistory.Size = new System.Drawing.Size(69, 24);
		this.btnResetHistory.Text = "Reset";
		this.btnResetHistory.Click += new System.EventHandler(btnResetHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(81, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.btnLoadAll.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadAll.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(83, 24);
		this.btnLoadAll.Text = "Load All";
		this.btnLoadAll.Visible = false;
		this.btnLoadAll.Click += new System.EventHandler(btnLoadAll_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(964, 661);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmWagesBillPartyProcessing";
		this.Text = "Contractor Labour Wages";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmWagesBillPartyProcessing_FormClosing);
		base.Load += new System.EventHandler(frmWagesBillPartyProcessing_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmWagesBillPartyProcessing_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabIncomingLabourwages.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage5.ResumeLayout(false);
		this.panel18.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.gridPendingWagesSlip).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.tabPage6.ResumeLayout(false);
		this.panel19.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDataRetrieve).EndInit();
		this.panel20.ResumeLayout(false);
		this.panel20.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdwagesDetail).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.tabPage4.ResumeLayout(false);
		this.panel16.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdStiching).EndInit();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbStockParty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbReferenceDocType).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel21.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryDetail).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel14.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel11.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDocumentTypeHistory).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
