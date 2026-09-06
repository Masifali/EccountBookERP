using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.PartyProcessing;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.PartyProcessing;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using CameraControl;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.PartyProcessing;

public class ProductionPartyProcessingBill : Form
{
	private Rightsobjects formrights = new Rightsobjects();

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtOHDetail = new DataTable();

	private DataTable dePMdetail = new DataTable();

	private DataTable dtByProduct = new DataTable();

	private bool UpdateMode;

	private int DefaultDaysToLessFromHistoryFromDate;

	private int RECID;

	private UserAccount UserAccount = new UserAccount();

	private List<InvProductionProcessingBillOutPut> lstRemoveOutputDetailRecord = new List<InvProductionProcessingBillOutPut>();

	private IContainer components;

	private ToolStrip miniToolStrip;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel12;

	internal GridEX GrdHistoryMain;

	private Panel panel8;

	private ToolStrip toolStrip2;

	private Panel panel2;

	private Panel panel4;

	internal UltraCombo CmbJobOrderNo;

	private Label label19;

	private TextBox txtPlanTypeSrNo;

	internal UltraCombo CmbRateUom;

	private Label label11;

	private TextBox txtRemarks;

	internal UltraCombo CmbStockParty;

	private Label label3;

	private TextBox txtRate;

	private Label label2;

	private TextBox txtBillNo;

	private DateTimePicker datBillDate;

	private Label label5;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripButton BtnPrint;

	private ToolStripButton btnattachment;

	private ToolStripButton btnRefresh;

	private Label label23;

	private Panel panel6;

	private Panel panel5;

	internal GridEX grdInput;

	private Panel panel10;

	internal GridEX grdPM;

	internal GridEX grdOH;

	private TextBox txtNoOfBags;

	private Label label1;

	private TextBox txtProcesssingAmount;

	private Label label4;

	private Panel panel14;

	private Label label8;

	private Label label12;

	internal UltraCombo CmbCreditAc;

	private Panel panel18;

	internal GridEX grdByProducts;

	private Panel panel19;

	private Label label13;

	private Panel panel11;

	private Label label6;

	private TextBox txtBPPurchaseTotalAmt;

	private Label label14;

	private Panel panel26;

	internal GridEX grdhistoryOH;

	private Label label18;

	internal GridEX grdHistoryPM;

	private Label label17;

	private Panel panel21;

	internal GridEX grdhistoryBpPurchased;

	private Label label16;

	internal GridEX GrdHistoryInput;

	private Panel panel23;

	private Panel panel20;

	private InfoLabel infoLabel1;

	private Label label20;

	private Panel panel7;

	private Panel panel13;

	private Label label7;

	private Panel panel58;

	private RadioButton RdModifyDate;

	private RadioButton RrDocDate;

	private RadioButton RdEntryDate;

	private GroupBox groupBox11;

	internal UltraCombo CmbJobOrderHistory;

	private Button BtnShowHistory;

	private Label label145;

	private DateTimePicker ToDateHistory;

	private Label label146;

	private Label label147;

	private TextBox ToDocNoHistory;

	private DateTimePicker FromDateHistory;

	private Label label148;

	private Label label149;

	private TextBox FromNoHistory;

	private Panel panel16;

	private Panel panel17;

	private Panel panel9;

	private Panel panel15;

	private Label label9;

	private Panel panel31;

	private Panel panel32;

	private Label label10;

	private Label label15;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnRefreshHistory;

	private CheckBox ChkBox;

	private ToolStripButton btnDelete;

	private RadioButton RdApproveDate;

	private ToolStripButton btnshortcutkeys;

	private CtrlGrdBar ctrlGrdBar1;

	private TextBox txtBillAmount;

	private Label label21;

	private TextBox txtPMAmount;

	private Label label24;

	private TextBox txtOHAmount;

	private Label label22;

	private Label label25;

	public ProductionPartyProcessingBill(UserAccount user)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmProductionJobOrder_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1100, 600);
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formrights.DoHaveSaveRight;
			btnUpdate.Enabled = formrights.DoHaveUpdateRights;
			BtnPrint.Enabled = formrights.DoHavePrintRights;
			ChkBox.Enabled = formrights.DoHavePrintRights;
			btnDelete.Enabled = formrights.DoHaveCanDelete;
			dtViewAttachments.Columns.Add("AttchmentPath");
			dtByProduct.Columns.Add("Id", typeof(int));
			dtByProduct.Columns.Add("ItemId", typeof(int));
			dtByProduct.Columns.Add("ItemCode");
			dtByProduct.Columns.Add("ItemName");
			dtByProduct.Columns.Add("PackSize", typeof(double));
			dtByProduct.Columns.Add("Qty", typeof(double));
			dtByProduct.Columns.Add("StockWeight", typeof(double));
			dtByProduct.Columns.Add("PurchaseQty", typeof(double));
			dtByProduct.Columns.Add("PurchaseWeight", typeof(double));
			dtByProduct.Columns.Add("PurchaseRate", typeof(double));
			dtByProduct.Columns.Add("RateUom", typeof(double));
			dtByProduct.Columns.Add("Amount", typeof(double));
			dtByProduct.Columns.Add("DebitAccountId", typeof(int));
			dtByProduct.Columns.Add("Remarks");
			dePMdetail.Columns.Add("ItemId", typeof(int));
			dePMdetail.Columns.Add("Qty", typeof(double));
			dePMdetail.Columns.Add("Rate", typeof(double));
			dePMdetail.Columns.Add("Amount", typeof(double));
			dePMdetail.Columns.Add("Remarks");
			AddRowInPackingMaterialGrid();
			grdPM.RetrieveStructure();
			grdPackingMaterialSettings();
			dtOHDetail.Columns.Add("AccountId", typeof(int));
			dtOHDetail.Columns.Add("Qty", typeof(double));
			dtOHDetail.Columns.Add("Rate", typeof(double));
			dtOHDetail.Columns.Add("Amount", typeof(double));
			dtOHDetail.Columns.Add("Remarks");
			AddRowInExpenseGrid();
			grdOH.RetrieveStructure();
			grdExpenseSettings();
			AccountTitleFill();
			GenerateCode();
			RateUomBind();
			JobOrderNoFill();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			datBillDate.Focus();
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			HistoryCombosFill();
			ToDateHistory.Value = DateTime.Now;
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
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		try
		{
			int code = InvProductionProcessingBill.GenerateCode(new InvProductionProcessingBill
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 123
			});
			if (code > 0)
			{
				txtBillNo.Text = code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RateUomBind()
	{
		try
		{
			DataTable dt = UOM.UOMStaticAll();
			if (dt.Rows.Count > 0)
			{
				DataTable dtUom = new DataTable();
				dtUom.Columns.Add("Id");
				dtUom.Columns.Add("RateUom");
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtUom.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["Equivalent"]);
				}
				DDL.BindDDL(dtUom, CmbRateUom, "Id", "RateUom", "Rate UOM", ZeroIndex: false);
				((UltraGridBase)CmbRateUom).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JobOrderNoFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		try
		{
			int ID = Conversion.ToInt(CmbJobOrderNo.Value);
			DataTable dtPJO = InvProductionJobOrderPartyProcessing.GetJobOrderNoForInvFoodProduction(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 120
			});
			if (dtPJO.Rows.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("PlanCode");
			for (int i = 0; i < dtPJO.Rows.Count; i++)
			{
				dt.Rows.Add(dtPJO.Rows[i]["Id"], dtPJO.Rows[i]["PlanCode"]);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbJobOrderNo, "Id", "PlanCode", "JobOrderNo", ZeroIndex: true);
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbJobOrderNo.Value = ID;
					}
					else
					{
						((Control)(object)CmbJobOrderNo).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbJobOrderNo).Text = string.Empty;
				((UltraGridBase)CmbJobOrderNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountTitleFill()
	{
		try
		{
			int ID = Conversion.ToInt(CmbCreditAc.Value);
			DataTable dt = CommonServices.CoaAllocationAccountTitleByAccountTypeIds(null, "2,15,11,12,13,14,20,21,22");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbCreditAc, "Id", "AccountTitle", "Credit Account", ZeroIndex: true);
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbCreditAc.Value = ID;
					}
					else
					{
						((Control)(object)CmbCreditAc).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbCreditAc).Text = string.Empty;
				((UltraGridBase)CmbCreditAc).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbJobOrderNo_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		try
		{
			DataTable dt = InvProductionJobOrderPartyProcessing.GetGlAccountsByJobOrderId(new InvProductionJobOrderPartyProcessing
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(CmbJobOrderNo.Value),
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbStockParty, "StockPartyId", "StockParty", "StockParty", false);
				((UltraGridBase)CmbStockParty).Rows[0].Activate();
			}
			else
			{
				((UltraGridBase)CmbStockParty).DataSource = null;
			}
			GetInPutForProcessingBill();
			ByProductGrdFill();
			BillAmountCalculations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbJobOrderNo_TextChanged(object sender, EventArgs e)
	{
		try
		{
			GetInPutForProcessingBill();
			BillAmountCalculations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtBillNo.Text.Trim() == string.Empty || txtBillNo.Text.Trim() == "0")
		{
			MessageBox.Show("BillNo Field Required");
			txtBillNo.Focus();
			return false;
		}
		if (txtRate.Text == string.Empty || txtRate.Text == "0")
		{
			MessageBox.Show("Rate Field Required");
			txtRate.Focus();
			return false;
		}
		if (((UltraGridBase)CmbStockParty).ActiveRow == null || Conversion.ToInt(CmbStockParty.Value) == 0)
		{
			MessageBox.Show("StockParty Field Required");
			CmbStockParty.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCreditAc).ActiveRow == null || Conversion.ToInt(CmbCreditAc.Value) == 0)
		{
			MessageBox.Show("CreditAccount Field Required");
			CmbCreditAc.Focus();
			return false;
		}
		if (((UltraGridBase)CmbRateUom).ActiveRow == null || Conversion.ToInt(CmbRateUom.Value) == 0)
		{
			MessageBox.Show("RateUom Field Required");
			CmbRateUom.Focus();
			return false;
		}
		if (((UltraGridBase)CmbJobOrderNo).ActiveRow == null || Conversion.ToInt(CmbJobOrderNo.Value) == 0)
		{
			MessageBox.Show("JobOrder Field Required");
			CmbJobOrderNo.Focus();
			return false;
		}
		return true;
	}

	private void GetInPutForProcessingBill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		try
		{
			DataTable dt = InvProductionProcessingBill.GetInPutForProcessingBill(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 117,
				JobOrderId = Conversion.ToInt(CmbJobOrderNo.Value)
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dtTarget = new DataTable();
				dtTarget.Columns.Add("ItemId", typeof(int));
				dtTarget.Columns.Add("ItemName", typeof(string));
				dtTarget.Columns.Add("ItemCode", typeof(string));
				dtTarget.Columns.Add("Uom", typeof(decimal));
				dtTarget.Columns.Add("Qty", typeof(decimal));
				dtTarget.Columns.Add("StockWeight", typeof(decimal));
				foreach (DataRow row in dt.Rows)
				{
					dtTarget.Rows.Add(row["ItemId"], row["ItemName"], row["ItemCode"], row["UomEquivalent"], row["Qty"], row["StockWeight"]);
				}
				grdInput.DataSource = dtTarget;
				grdInput.RetrieveStructure();
				grdInputSetting();
			}
			else
			{
				grdInput.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInputSetting()
	{
		try
		{
			grdInput.RootTable.Columns["ItemId"].Visible = false;
			CommonServices.GridAutoAdjustmentNew(grdInput);
			grdInput.RootTable.Columns["ItemName"].Width = 250;
			grdInput.RootTable.Columns["Uom"].TextAlignment = (TextAlignment)3;
			grdInput.RootTable.Columns["Uom"].FormatString = "#,##.###";
			grdInput.RootTable.Columns["Uom"].TotalFormatString = "#,##.###";
			grdInput.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdInput.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdInput.RootTable.Columns["Qty"].FormatString = "#,##.###";
			grdInput.RootTable.Columns["Qty"].TotalFormatString = "#,##.###";
			grdInput.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			grdInput.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			grdInput.RootTable.Columns["StockWeight"].FormatString = "#,##.###";
			grdInput.RootTable.Columns["StockWeight"].TotalFormatString = "#,##.###";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ByProductGrdFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		try
		{
			DataTable dt = InvProductionProcessingBill.GetOutPutForProcessingBillByJobOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 118,
				JobOrderId = Conversion.ToInt(CmbJobOrderNo.Value),
				Activity = "ByProduct"
			});
			if (dt.Rows.Count > 0)
			{
				dtByProduct.Rows.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtByProduct.Rows.Add(0, dt.Rows[i]["ItemId"], dt.Rows[i]["ItemCode"], dt.Rows[i]["ItemName"], dt.Rows[i]["UomCode"], dt.Rows[i]["Qty"], dt.Rows[i]["StockWeight"], 0, 0, 0, 40, 0, 0, "");
				}
				grdByProducts.DataSource = dtByProduct;
				grdByProducts.RetrieveStructure();
				grdByProductSetting();
			}
			else
			{
				grdByProducts.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdByProductSetting()
	{
		try
		{
			DataTable dtByProductsAc = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("4,6");
			grdByProducts.RootTable.Columns["Id"].Visible = false;
			grdByProducts.RootTable.Columns["ItemId"].Visible = false;
			grdByProducts.RootTable.Columns["Id"].EditType = (EditType)0;
			grdByProducts.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdByProducts.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			grdByProducts.RootTable.Columns["PackSize"].EditType = (EditType)0;
			grdByProducts.RootTable.Columns["Qty"].EditType = (EditType)0;
			grdByProducts.RootTable.Columns["StockWeight"].EditType = (EditType)0;
			grdByProducts.RootTable.Columns["RateUom"].EditType = (EditType)0;
			grdByProducts.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdByProducts.RootTable.Columns["DebitAccountId"].ColumnType = (ColumnType)0;
			grdByProducts.RootTable.Columns["DebitAccountId"].HasValueList = true;
			grdByProducts.RootTable.Columns["DebitAccountId"].LimitToList = true;
			grdByProducts.RootTable.Columns["DebitAccountId"].EditType = (EditType)4;
			grdByProducts.RootTable.Columns["DebitAccountId"].Caption = "Debit Account";
			grdByProducts.RootTable.Columns["DebitAccountId"].ValueList.PopulateValueList((IEnumerable)dtByProductsAc.DefaultView, "Id", "AccountTitle");
			grdByProducts.RootTable.Columns["DebitAccountId"].Width = 200;
			grdByProducts.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdByProducts.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdByProducts.RootTable.Columns["Qty"].FormatString = "#,##0.##";
			grdByProducts.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			grdByProducts.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			grdByProducts.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			grdByProducts.RootTable.Columns["StockWeight"].FormatString = "#,##0.##";
			grdByProducts.RootTable.Columns["StockWeight"].TotalFormatString = "#,##0.##";
			grdByProducts.RootTable.Columns["PurchaseRate"].AggregateFunction = (AggregateFunction)3;
			grdByProducts.RootTable.Columns["PurchaseRate"].TextAlignment = (TextAlignment)3;
			grdByProducts.RootTable.Columns["PurchaseRate"].FormatString = "#,##0.##";
			grdByProducts.RootTable.Columns["PurchaseRate"].TotalFormatString = "#,##0.##";
			grdByProducts.RootTable.Columns["PurchaseQty"].AggregateFunction = (AggregateFunction)2;
			grdByProducts.RootTable.Columns["PurchaseQty"].TextAlignment = (TextAlignment)3;
			grdByProducts.RootTable.Columns["PurchaseQty"].FormatString = "#,##0.##";
			grdByProducts.RootTable.Columns["PurchaseQty"].TotalFormatString = "#,##0.##";
			grdByProducts.RootTable.Columns["PurchaseWeight"].AggregateFunction = (AggregateFunction)2;
			grdByProducts.RootTable.Columns["PurchaseWeight"].TextAlignment = (TextAlignment)3;
			grdByProducts.RootTable.Columns["PurchaseWeight"].FormatString = "#,##0.##";
			grdByProducts.RootTable.Columns["PurchaseWeight"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustmentNew(grdByProducts);
			grdByProducts.RootTable.Columns["ItemName"].Width = 170;
			grdByProducts.RootTable.Columns["Remarks"].Width = 160;
			grdByProducts.RootTable.Columns.Add("Delete");
			grdByProducts.RootTable.Columns["Delete"].Key = "Delete";
			grdByProducts.RootTable.Columns["Delete"].Caption = "X";
			grdByProducts.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdByProducts.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdByProducts.RootTable.Columns["Delete"].ButtonText = "X";
			grdByProducts.RootTable.Columns["Delete"].Width = 20;
			grdByProducts.RootTable.Columns["Delete"].Position = 0;
			grdByProducts.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdByProducts_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		try
		{
			GridEXRow r = grdByProducts.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					InvProductionProcessingBillOutPut d = new InvProductionProcessingBillOutPut();
					d.Id = Conversion.ToInt(r.Cells["Id"].Value);
					d.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					d.ItemUomId = Conversion.ToInt(r.Cells["PackSize"].Value);
					d.Qty = Conversion.ToDecimal(r.Cells["PurchaseQty"].Value);
					d.Weight = Conversion.ToDecimal(r.Cells["PurchaseWeight"].Value);
					d.Rate = Conversion.ToDecimal(r.Cells["PurchaseRate"].Value);
					d.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
					d.DebitAccountId = Conversion.ToInt(r.Cells["DebitAccountId"].Value);
					d.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
					d.ActionTypeId = 3;
					lstRemoveOutputDetailRecord.Add(d);
					dtByProduct.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				else
				{
					dtByProduct.Rows[r.RowIndex].Delete();
					r.Delete();
				}
			}
			grdByProducts.UpdateData();
			BillAmountCalculations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdByProducts_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdByProducts.UpdateData();
			GridEXRow r = grdByProducts.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "PurchaseQty" && r.Cells["PurchaseQty"].Value.ToString() != string.Empty && r.Cells["PackSize"].Value.ToString() != string.Empty)
			{
				if (Conversion.ToDouble((object)r.Cells["PurchaseQty"].Value.ToString()) <= Conversion.ToDouble((object)r.Cells["Qty"].Value.ToString()))
				{
					double num = Conversion.ToDouble((object)r.Cells["PurchaseQty"].Value.ToString());
					double PackSize = Conversion.ToDouble((object)r.Cells["PackSize"].Value.ToString());
					double PurchaseWeight = num * PackSize;
					r.Cells["PurchaseWeight"].Value = PurchaseWeight;
				}
				else
				{
					MessageBox.Show("Purchase Qty Can not be Greater Than Stock Qty");
					r.Cells["PurchaseQty"].Value = r.Cells["Qty"].Value;
					r.Cells["PurchaseWeight"].Value = r.Cells["StockWeight"].Value;
				}
			}
			if (e.Column.Key == "PurchaseRate" || e.Column.Key == "RateUom" || e.Column.Key == "PurchaseWeight" || e.Column.Key == "PurchaseQty" || e.Column.Key == "PurchaseRate")
			{
				if (Conversion.ToDouble((object)r.Cells["PurchaseWeight"].Value.ToString()) <= Conversion.ToDouble((object)r.Cells["StockWeight"].Value.ToString()))
				{
					string obj = r.Cells["PurchaseWeight"].Value.ToString();
					string EquivalentPoRate = r.Cells["RateUom"].Text.ToString();
					string ItemRate = r.Cells["PurchaseRate"].Value.ToString();
					double ItemAmount = Conversion.ToDouble((object)obj) / Conversion.ToDouble((object)EquivalentPoRate) * Conversion.ToDouble((object)ItemRate);
					r.Cells["Amount"].Value = ItemAmount;
				}
				else
				{
					MessageBox.Show("Purchase Weight Can not be Greater Than Stock Weight");
					r.Cells["PurchaseWeight"].Value = r.Cells["StockWeight"].Value;
				}
			}
			grdByProducts.UpdateData();
			BillAmountCalculations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInPackingMaterialGrid()
	{
		dePMdetail.Rows.Add(0, 0, 0, 0, "");
		grdPM.DataSource = dePMdetail;
	}

	private void grdPackingMaterialSettings()
	{
		try
		{
			DataTable dt = Item.ReadAllForItemsForPartyProcessingPM(UserAccount.OrganizationId, UserAccount.CompanyId);
			grdPM.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			grdPM.RootTable.Columns["ItemId"].HasValueList = true;
			grdPM.RootTable.Columns["ItemId"].LimitToList = true;
			grdPM.RootTable.Columns["ItemId"].EditType = (EditType)4;
			grdPM.RootTable.Columns["ItemId"].Caption = "Item Name";
			grdPM.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dt.DefaultView, "Id", "ItemName");
			grdPM.RootTable.Columns["ItemId"].Width = 200;
			grdPM.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdPM.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdPM.RootTable.Columns["Qty"].FormatString = "#,##";
			grdPM.RootTable.Columns["Qty"].TotalFormatString = "#,##";
			grdPM.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdPM.RootTable.Columns["Rate"].FormatString = "#.##";
			grdPM.RootTable.Columns["Rate"].TotalFormatString = "#.##";
			grdPM.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdPM.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdPM.RootTable.Columns["Amount"].FormatString = "#,##";
			grdPM.RootTable.Columns["Amount"].TotalFormatString = "#,##";
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
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPM.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					dePMdetail.Rows[r.RowIndex].Delete();
					r.Delete();
					if (dePMdetail.Rows.Count == 0)
					{
						AddRowInPackingMaterialGrid();
					}
				}
				if (e.Column.Key == "Add")
				{
					AddRowInPackingMaterialGrid();
				}
			}
			grdPM.UpdateData();
			BillAmountCalculations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackingMaterial_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdPM.UpdateData();
			GridEXRow r = grdPM.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Qty" || e.Column.Key == "Rate")
				{
					double num = Conversion.ToDouble(r.Cells["Qty"].Value);
					double Rate = Conversion.ToDouble(r.Cells["Rate"].Value);
					double Amount = num * Rate;
					r.Cells["Amount"].Value = Amount;
					r.EndEdit();
				}
				grdPM.UpdateData();
				BillAmountCalculations();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInExpenseGrid()
	{
		dtOHDetail.Rows.Add(0, 0, 0, 0, "");
		grdOH.DataSource = dtOHDetail;
	}

	private void grdExpenseSettings()
	{
		try
		{
			DataTable dt = CommonServices.CoaAllocationAccountTitleByAccountTypeIds(null, "2,15,22");
			grdOH.RootTable.Columns["AccountId"].ColumnType = (ColumnType)0;
			grdOH.RootTable.Columns["AccountId"].HasValueList = true;
			grdOH.RootTable.Columns["AccountId"].LimitToList = true;
			grdOH.RootTable.Columns["AccountId"].EditType = (EditType)4;
			grdOH.RootTable.Columns["AccountId"].Caption = "AccountTitle";
			grdOH.RootTable.Columns["AccountId"].ValueList.PopulateValueList((IEnumerable)dt.DefaultView, "Id", "AccountTitle");
			grdOH.RootTable.Columns["AccountId"].Width = 250;
			grdOH.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdOH.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdOH.RootTable.Columns["Amount"].FormatString = "#,##";
			grdOH.RootTable.Columns["Amount"].TotalFormatString = "#,##";
			grdOH.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdOH.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdOH.RootTable.Columns["Qty"].FormatString = "#,##";
			grdOH.RootTable.Columns["Qty"].TotalFormatString = "#,##";
			grdOH.RootTable.Columns.Add("Delete");
			grdOH.RootTable.Columns["Delete"].Key = "Delete";
			grdOH.RootTable.Columns["Delete"].Caption = "X";
			grdOH.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOH.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdOH.RootTable.Columns["Delete"].ButtonText = "X";
			grdOH.RootTable.Columns["Delete"].Width = 30;
			grdOH.RootTable.Columns["Delete"].Position = 0;
			grdOH.RootTable.Columns.Add("Add");
			grdOH.RootTable.Columns["Add"].Key = "Add";
			grdOH.RootTable.Columns["Add"].Caption = "+";
			grdOH.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOH.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdOH.RootTable.Columns["Add"].ButtonText = "+";
			grdOH.RootTable.Columns["Add"].Width = 30;
			grdOH.RootTable.Columns["Add"].Position = 1;
			grdOH.FrozenColumns = 1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOverHead_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdOH.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					dtOHDetail.Rows[r.RowIndex].Delete();
					r.Delete();
					if (dtOHDetail.Rows.Count == 0)
					{
						AddRowInExpenseGrid();
					}
				}
				if (e.Column.Key == "Add")
				{
					dtOHDetail.Rows.Add(0, 0, 0, 0, "");
					grdOH.DataSource = dtOHDetail;
				}
				BillAmountCalculations();
			}
			grdOH.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOverHead_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdOH.UpdateData();
			GridEXRow r = grdOH.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Qty" || e.Column.Key == "Rate")
				{
					double num = Conversion.ToDouble(r.Cells["Qty"].Value);
					double Rate = Conversion.ToDouble(r.Cells["Rate"].Value);
					double Amount = num * Rate;
					r.Cells["Amount"].Value = Amount;
					r.EndEdit();
				}
				grdOH.UpdateData();
				BillAmountCalculations();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Expected O, but got Unknown
		//IL_0741: Unknown result type (might be due to invalid IL or missing references)
		//IL_0748: Expected O, but got Unknown
		//IL_0950: Unknown result type (might be due to invalid IL or missing references)
		//IL_0957: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			InvProductionProcessingBill obj = new InvProductionProcessingBill();
			if (RECID > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				obj.Id = RECID;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.BranchesId = UserAccount.BranchesId;
			obj.ProjectsId = UserAccount.BranchesId;
			obj.DocumentTypeId = 123;
			obj.ScreenName = base.Name;
			obj.EntryUserId = UserAccount.ID;
			obj.ModifyUserId = UserAccount.ID;
			obj.ApprovedUserId = UserAccount.ID;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.ApprovedDate = DateTime.Now;
			obj.IsApproved = false;
			obj.BillNo = Conversion.ToInt((object)txtBillNo.Text);
			obj.BillDate = datBillDate.Value;
			obj.InvProductionJobOrderId = Conversion.ToInt(CmbJobOrderNo.Value);
			obj.StockPartyId = Conversion.ToInt(CmbStockParty.Value);
			obj.CreditAccountId = Conversion.ToInt(CmbCreditAc.Value);
			obj.RateUomId = Conversion.ToInt((object)((Control)(object)CmbRateUom).Text);
			obj.Rate = Conversion.ToDecimal((object)txtRate.Text);
			obj.Qty = Conversion.ToDecimal((object)txtNoOfBags.Text);
			obj.Weight = Conversion.ToDecimal((object)txtNoOfBags.Text);
			obj.Amount = Conversion.ToDecimal((object)txtProcesssingAmount.Text);
			obj.TotalBPPurchaseAmt = Conversion.ToDecimal((object)txtBPPurchaseTotalAmt.Text);
			obj.RemarksHeader = txtRemarks.Text;
			obj.InvProductionProcessingBillOHslist = new List<InvProductionProcessingBillOH>();
			obj.InvProductionProcessingBillPMslist = new List<InvProductionProcessingBillPM>();
			obj.InvProductionProcessingBillOutPutslist = new List<InvProductionProcessingBillOutPut>();
			GridEXRow[] rows = grdByProducts.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (Conversion.ToDecimal(item.Cells["Amount"].Value) > 0m && Conversion.ToInt(item.Cells["DebitAccountId"].Value) > 0)
				{
					InvProductionProcessingBillOutPut billOutBP = new InvProductionProcessingBillOutPut();
					if (Conversion.ToInt(item.Cells["ItemId"].Value) == 0)
					{
						MessageBox.Show("Please Select an Item In OutPut By Product Grid on row no :" + (item.RowIndex + 1));
						return;
					}
					if (Conversion.ToDouble(item.Cells["PackSize"].Value) == 0.0)
					{
						MessageBox.Show("Please Enter PackSize In OutPut By Product Grid on row no :" + (item.RowIndex + 1));
						return;
					}
					if (Conversion.ToDouble(item.Cells["PurchaseQty"].Value) == 0.0)
					{
						MessageBox.Show("Please Enter Purchase Qty In OutPut By Product Grid on row no :" + (item.RowIndex + 1));
						return;
					}
					if (Conversion.ToDouble(item.Cells["PurchaseWeight"].Value) == 0.0)
					{
						MessageBox.Show("Please Enter Purchase Weight In OutPut By Product Grid on row no :" + (item.RowIndex + 1));
						return;
					}
					if (Conversion.ToDouble(item.Cells["PurchaseRate"].Value) == 0.0)
					{
						MessageBox.Show("Rate Is Required In OutPut By Product Grid on row no :" + (item.RowIndex + 1));
						return;
					}
					if (Conversion.ToDouble(item.Cells["RateUom"].Value) == 0.0)
					{
						MessageBox.Show("RateUom Is Required In OutPut By Product Grid on row no :" + (item.RowIndex + 1));
						return;
					}
					if (Conversion.ToDouble(item.Cells["Amount"].Value) == 0.0)
					{
						MessageBox.Show("Amount Is Required In OutPut By Product Grid on row no :" + (item.RowIndex + 1));
						return;
					}
					if (Conversion.ToInt(item.Cells["DebitAccountId"].Value) == 0)
					{
						MessageBox.Show("Please Select an Debit Account In OutPut By Product Grid on row no :" + (item.RowIndex + 1));
						return;
					}
					billOutBP.Id = Conversion.ToInt(item.Cells["Id"].Value);
					billOutBP.ActionTypeId = ((billOutBP.Id <= 0) ? 1 : 2);
					billOutBP.ItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
					billOutBP.ItemUomId = Conversion.ToInt(item.Cells["PackSize"].Value);
					billOutBP.Qty = Conversion.ToDecimal(item.Cells["PurchaseQty"].Value);
					billOutBP.Weight = Conversion.ToDecimal(item.Cells["PurchaseWeight"].Value);
					billOutBP.Rate = Conversion.ToDecimal(item.Cells["PurchaseRate"].Value);
					billOutBP.Amount = Conversion.ToDecimal(item.Cells["Amount"].Value);
					billOutBP.DebitAccountId = Conversion.ToInt(item.Cells["DebitAccountId"].Value);
					billOutBP.Remarks = Conversion.ToString(item.Cells["Remarks"].Value);
					billOutBP.SortNo = item.RowIndex + 1;
					obj.InvProductionProcessingBillOutPutslist.Add(billOutBP);
				}
			}
			if (RECID > 0 && lstRemoveOutputDetailRecord.Count > 0)
			{
				for (int i = 0; i < lstRemoveOutputDetailRecord.Count; i++)
				{
					obj.InvProductionProcessingBillOutPutslist.Add(lstRemoveOutputDetailRecord[i]);
				}
			}
			rows = grdOH.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToDouble(r.Cells["Amount"].Value) > 0.0 && Conversion.ToInt(r.Cells["AccountId"].Value) == 0)
				{
					throw new Exception("Please Select an Account Against OverHead First");
				}
				if (Conversion.ToInt(r.Cells["AccountId"].Value) > 0)
				{
					InvProductionProcessingBillOH billOH = new InvProductionProcessingBillOH();
					billOH.AccountId = Conversion.ToInt(r.Cells["AccountId"].Value);
					billOH.Qty = Conversion.ToDecimal(r.Cells["Qty"].Value);
					billOH.Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
					billOH.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
					billOH.OHRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
					billOH.SortNo = r.RowIndex + 1;
					obj.InvProductionProcessingBillOHslist.Add(billOH);
				}
			}
			rows = grdPM.GetRows();
			foreach (GridEXRow item2 in rows)
			{
				if (!(Conversion.ToDecimal(item2.Cells["Amount"].Value) > 0m))
				{
					continue;
				}
				if (Conversion.ToInt(item2.Cells["ItemId"].Value) == 0)
				{
					throw new Exception("Please Select an Item Against PM Grid on row no :" + (item2.RowIndex + 1));
				}
				if (Conversion.ToInt(item2.Cells["ItemId"].Value) > 0)
				{
					if ((double)Conversion.ToInt(item2.Cells["Qty"].Value) == 0.0)
					{
						throw new Exception("Please Select an Qty Against PM Grid on row no :" + (item2.RowIndex + 1));
					}
					if ((double)Conversion.ToInt(item2.Cells["Rate"].Value) == 0.0)
					{
						throw new Exception("Please Select an Rate Against PM Grid on row no :" + (item2.RowIndex + 1));
					}
					InvProductionProcessingBillPM billPM = new InvProductionProcessingBillPM();
					billPM.ItemId = Conversion.ToInt(item2.Cells["ItemId"].Value);
					billPM.Qty = Conversion.ToDecimal(item2.Cells["Qty"].Value);
					billPM.Rate = Conversion.ToDecimal(item2.Cells["Rate"].Value);
					billPM.Amount = Conversion.ToDecimal(item2.Cells["Amount"].Value);
					billPM.Remarks = Conversion.ToString(item2.Cells["Remarks"].Value);
					billPM.SortNo = item2.RowIndex + 1;
					obj.InvProductionProcessingBillPMslist.Add(billPM);
				}
			}
			obj.AttachmentsList = new List<DMSAttachments>();
			obj.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<InvProductionProcessingBill>(obj, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			int success = InvProductionProcessingBill.Save(obj);
			if (RECID == 0)
			{
				MessageBox.Show("Record Save Successfully" + obj.BillNo);
			}
			else
			{
				MessageBox.Show("Record Update Successfully" + obj.BillNo);
				UpdateMode = false;
				FormHelper.DeleteAllAttachmentsFromDb(AT, RECID, 123, base.Name);
			}
			Reset();
			if (ChkBox.Checked)
			{
				CrystalReportPrint_Helper.ProductionPartyProcessingBillReport_611(Conversion.ToInt((object)obj.InvProductionJobOrderId), success);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "DatabaseError", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (RECID == 0)
			{
				MessageBox.Show("Record Id Not Found", "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				InvProductionProcessingBill.DeleteByID(UserAccount.ID, RECID);
				MessageBox.Show("Delete Record Successfully");
				Reset();
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
			if (RECID == 0)
			{
				throw new Exception("Record Not Update Because Record Not Found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadById(int Id)
	{
		try
		{
			RECID = Id;
			lstRemoveOutputDetailRecord.Clear();
			InvProductionProcessingBill obj = InvProductionProcessingBill.GetByID(RECID);
			if (obj == null)
			{
				return;
			}
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
			UpdateMode = true;
			tabControl1.SelectedIndex = 0;
			txtBillNo.Text = Conversion.ToString((object)obj.BillNo);
			datBillDate.Value = obj.BillDate;
			CmbJobOrderNo.Value = Conversion.ToInt((object)obj.InvProductionJobOrderId);
			CmbJobOrderNo_Leave(null, null);
			CmbStockParty.Value = obj.StockPartyId;
			CmbCreditAc.Value = obj.CreditAccountId;
			txtNoOfBags.Text = Conversion.ToDouble((object)obj.Weight).ToString("#,##.###");
			txtRate.Text = Conversion.ToDouble((object)obj.Rate).ToString("#,##.###");
			((Control)(object)CmbRateUom).Text = Conversion.ToString((object)obj.RateUomId);
			txtBPPurchaseTotalAmt.Text = Conversion.ToDouble((object)obj.TotalBPPurchaseAmt).ToString("#,##.###");
			txtProcesssingAmount.Text = Conversion.ToDouble((object)obj.Amount).ToString("#,##.###");
			txtRemarks.Text = obj.RemarksHeader;
			GridEXRow[] rows = grdByProducts.GetRows();
			foreach (GridEXRow item in rows)
			{
				for (int k = 0; k < obj.InvProductionProcessingBillOutPutslist.Count; k++)
				{
					if (Conversion.ToInt(item.Cells["ItemId"].Value) == obj.InvProductionProcessingBillOutPutslist[k].ItemId)
					{
						item.BeginEdit();
						item.Cells["PurchaseQty"].Value = obj.InvProductionProcessingBillOutPutslist[k].Qty;
						item.Cells["PurchaseWeight"].Value = obj.InvProductionProcessingBillOutPutslist[k].Weight;
						item.Cells["PurchaseRate"].Value = obj.InvProductionProcessingBillOutPutslist[k].Rate;
						item.Cells["Amount"].Value = obj.InvProductionProcessingBillOutPutslist[k].Amount;
						item.Cells["DebitAccountId"].Value = obj.InvProductionProcessingBillOutPutslist[k].DebitAccountId;
						item.Cells["Remarks"].Value = obj.InvProductionProcessingBillOutPutslist[k].Remarks;
						item.EndEdit();
					}
				}
			}
			dePMdetail.Rows.Clear();
			for (int j = 0; j < obj.InvProductionProcessingBillPMslist.Count; j++)
			{
				dePMdetail.Rows.Add(obj.InvProductionProcessingBillPMslist[j].ItemId, obj.InvProductionProcessingBillPMslist[j].Qty, obj.InvProductionProcessingBillPMslist[j].Rate, obj.InvProductionProcessingBillPMslist[j].Amount, obj.InvProductionProcessingBillPMslist[j].Remarks);
			}
			if (dePMdetail.Rows.Count == 0)
			{
				AddRowInPackingMaterialGrid();
			}
			grdPM.DataSource = dePMdetail;
			grdPM.RetrieveStructure();
			grdPackingMaterialSettings();
			dtOHDetail.Rows.Clear();
			for (int i = 0; i < obj.InvProductionProcessingBillOHslist.Count; i++)
			{
				dtOHDetail.Rows.Add(obj.InvProductionProcessingBillOHslist[i].AccountId, obj.InvProductionProcessingBillOHslist[i].Qty, obj.InvProductionProcessingBillOHslist[i].Rate, obj.InvProductionProcessingBillOHslist[i].Amount, obj.InvProductionProcessingBillOHslist[i].OHRemarks);
			}
			if (dtOHDetail.Rows.Count == 0)
			{
				AddRowInExpenseGrid();
			}
			grdOH.DataSource = dtOHDetail;
			grdOH.RetrieveStructure();
			grdExpenseSettings();
			FormHelper.LoadAttachmentsForObject(AT, RECID, base.Name);
			BillAmountCalculations();
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

	private void Reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Clear();
			lstRemoveOutputDetailRecord.Clear();
			datBillDate.Value = DateTime.Now;
			txtRate.Text = string.Empty;
			txtRemarks.Text = string.Empty;
			((Control)(object)CmbJobOrderNo).Text = string.Empty;
			((UltraGridBase)CmbRateUom).Rows[0].Activate();
			((Control)(object)CmbStockParty).Text = string.Empty;
			txtNoOfBags.Text = string.Empty;
			txtProcesssingAmount.Text = string.Empty;
			((Control)(object)CmbCreditAc).Text = string.Empty;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			btnsave.Visible = true;
			UpdateMode = false;
			GenerateCode();
			grdInput.ClearStructure();
			dtByProduct.Clear();
			grdByProducts.ClearStructure();
			dePMdetail.Clear();
			grdPM.ClearStructure();
			AddRowInPackingMaterialGrid();
			grdPM.RetrieveStructure();
			grdPackingMaterialSettings();
			dtOHDetail.Clear();
			grdOH.ClearStructure();
			AddRowInExpenseGrid();
			grdOH.RetrieveStructure();
			grdExpenseSettings();
			datBillDate.Focus();
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			JobOrderNoFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ProductionPartyProcessingBill_FormClosing(object sender, FormClosingEventArgs e)
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

	private void Print_Click(object sender, EventArgs e)
	{
		try
		{
			CrystalReportPrint_Helper.ProductionPartyProcessingBillReport_611(Conversion.ToInt(CmbJobOrderNo.Value), RECID);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GrdHistoryMain).Name);
		if (fs != null)
		{
			GrdHistoryMain.LoadLayoutFile((Stream)fs);
			fs.Close();
			fs.Dispose();
		}
	}

	private void BillAmountCalculations()
	{
		try
		{
			double Rate = 0.0;
			double RateUom = 0.0;
			double NoofBags = 0.0;
			double ProcessingAmount = 0.0;
			double InputgrdWeightTotal = 0.0;
			double PMgrdTotal = 0.0;
			double OHgrdTotal = 0.0;
			double BPPurchaseTotalAmount = 0.0;
			if (((UltraGridBase)CmbRateUom).ActiveRow != null && Conversion.ToInt(CmbRateUom.Value) > 0)
			{
				Rate = Conversion.ToDouble((object)txtRate.Text);
				RateUom = Conversion.ToDouble((object)((Control)(object)CmbRateUom).Text);
			}
			if (grdInput.RowCount > 0)
			{
				InputgrdWeightTotal = Conversion.ToDouble((object)grdInput.GetTotal(grdInput.RootTable.Columns["StockWeight"], (AggregateFunction)2).ToString());
			}
			if (grdPM.RowCount > 0)
			{
				PMgrdTotal = Conversion.ToDouble((object)grdPM.GetTotal(grdPM.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
			}
			if (grdOH.RowCount > 0)
			{
				OHgrdTotal = Conversion.ToDouble((object)grdOH.GetTotal(grdOH.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
			}
			if (grdByProducts.GetRows().Length != 0)
			{
				BPPurchaseTotalAmount = Conversion.ToDouble((object)grdByProducts.GetTotal(grdByProducts.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
			}
			NoofBags = Conversion.ToDouble((object)(InputgrdWeightTotal / RateUom));
			txtNoOfBags.Text = NoofBags.ToString("#,##");
			ProcessingAmount = NoofBags * Rate;
			txtProcesssingAmount.Text = Math.Round(ProcessingAmount).ToString("#,##");
			txtPMAmount.Text = PMgrdTotal.ToString("#,##");
			txtOHAmount.Text = OHgrdTotal.ToString("#,##");
			txtBPPurchaseTotalAmt.Text = BPPurchaseTotalAmount.ToString("#,##");
			txtBillAmount.Text = (ProcessingAmount + PMgrdTotal + OHgrdTotal - BPPurchaseTotalAmount).ToString("#,##");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			BillAmountCalculations();
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
			BillAmountCalculations();
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
		//IL_0068: Expected O, but got Unknown
		try
		{
			DataTable dtStock = new DataTable();
			dtStock.Columns.Add("Id", typeof(int));
			dtStock.Columns.Add("name", typeof(string));
			DataTable dt = InvProductionProcessingBill.GetDataForDropDownFromInvProductionProcessingBill(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "InvJobOrder")
					{
						dtStock.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtStock, CmbJobOrderHistory, "Id", "name", "Job Order", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindHistoryGrid()
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
			obj.DocumentTypeId = 123;
			obj.CanViewAllRecord = formrights.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
			if (RrDocDate.Checked)
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
			else if (RdEntryDate.Checked)
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
			else if (RdModifyDate.Checked)
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
			else if (RdApproveDate.Checked)
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
			obj.FromDocNo = Conversion.ToInt((object)FromNoHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			obj.InvJobOrderId = Conversion.ToInt(CmbJobOrderHistory.Value);
			DataTable dt = InvProductionProcessingBill.FormHistory(obj);
			if (dt.Rows.Count > 0)
			{
				DataTable dtTarget = new DataTable();
				dtTarget.Columns.Add("Id", typeof(int));
				dtTarget.Columns.Add("BillDate", typeof(DateTime));
				dtTarget.Columns.Add("BillNo", typeof(int));
				dtTarget.Columns.Add("DocumentTypeId", typeof(int));
				dtTarget.Columns.Add("InvProductionJobOrderId", typeof(int));
				dtTarget.Columns.Add("JobOrderNo", typeof(string));
				dtTarget.Columns.Add("STockPartyId", typeof(int));
				dtTarget.Columns.Add("StockParty", typeof(string));
				dtTarget.Columns.Add("CreditAccountId", typeof(int));
				dtTarget.Columns.Add("CreditAccount", typeof(string));
				dtTarget.Columns.Add("Qty", typeof(decimal));
				dtTarget.Columns.Add("Weight", typeof(decimal));
				dtTarget.Columns.Add("Rate", typeof(decimal));
				dtTarget.Columns.Add("RateUom", typeof(string));
				dtTarget.Columns.Add("Amount", typeof(decimal));
				dtTarget.Columns.Add("TotalBPPurchaseAmt", typeof(decimal));
				dtTarget.Columns.Add("RemarksHeader", typeof(string));
				dtTarget.Columns.Add("ApprovalStatus", typeof(string));
				dtTarget.Columns.Add("EntryDate", typeof(DateTime));
				dtTarget.Columns.Add("EntryUserName", typeof(string));
				dtTarget.Columns.Add("ModifyDate", typeof(DateTime));
				dtTarget.Columns.Add("ModifyUserName", typeof(string));
				dtTarget.Columns.Add("ApprovedDate", typeof(DateTime));
				dtTarget.Columns.Add("ApprovedUserName", typeof(string));
				dtTarget.Columns.Add("NoOfAttachments", typeof(int));
				foreach (DataRow row in dt.Rows)
				{
					dtTarget.Rows.Add(row["Id"], row["BillDate"], row["BillNo"], row["DocumentTypeId"], row["InvProductionJobOrderId"], row["JobOrderNo"], row["STockPartyId"], row["StockParty"], row["CreditAccountId"], row["CreditAccount"], row["Qty"], row["Weight"], row["Rate"], row["RateUomCode"], row["Amount"], row["TotalBPPurchaseAmt"], row["RemarksHeader"], row["ApprovalStatus"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"], row["ApprovedDate"], row["ApprovedUserName"], row["NoOfAttachments"]);
				}
				GrdHistoryMain.DataSource = dtTarget;
				GrdHistoryMain.RetrieveStructure();
				HgridSetting();
			}
			else
			{
				GrdHistoryMain.ClearStructure();
				GrdHistoryInput.ClearStructure();
				grdhistoryBpPurchased.ClearStructure();
				grdHistoryPM.ClearStructure();
				grdhistoryOH.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HgridSetting()
	{
		try
		{
			GrdHistoryMain.RootTable.Columns["Id"].Visible = false;
			GrdHistoryMain.RootTable.Columns["InvProductionJobOrderId"].Visible = false;
			GrdHistoryMain.RootTable.Columns["DocumentTypeId"].Visible = false;
			GrdHistoryMain.RootTable.Columns["CreditAccountId"].Visible = false;
			GrdHistoryMain.RootTable.Columns["STockPartyId"].Visible = false;
			GrdHistoryMain.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			GrdHistoryMain.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			GrdHistoryMain.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			GrdHistoryMain.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			GrdHistoryMain.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			GrdHistoryMain.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			GrdHistoryMain.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			GrdHistoryMain.RootTable.Columns["TotalBPPurchaseAmt"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryMain.RootTable.Columns["TotalBPPurchaseAmt"].TextAlignment = (TextAlignment)3;
			GrdHistoryMain.RootTable.Columns["TotalBPPurchaseAmt"].FormatString = clsGlobalVariables.stringFormatsingle;
			GrdHistoryMain.RootTable.Columns["TotalBPPurchaseAmt"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GrdHistoryMain.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryMain.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			GrdHistoryMain.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GrdHistoryMain.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GrdHistoryMain.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryMain.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			GrdHistoryMain.RootTable.Columns["Weight"].FormatString = "#,##0.###";
			GrdHistoryMain.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
			GrdHistoryMain.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryMain.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			GrdHistoryMain.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			GrdHistoryMain.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			GrdHistoryMain.RootTable.Columns.Add("Edit");
			GrdHistoryMain.RootTable.Columns["Edit"].Caption = "Edit";
			GrdHistoryMain.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistoryMain.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			GrdHistoryMain.RootTable.Columns["Edit"].ButtonText = "Edit";
			GrdHistoryMain.RootTable.Columns["Edit"].Width = 50;
			GrdHistoryMain.RootTable.Columns["Edit"].Position = 0;
			GrdHistoryMain.RootTable.Columns.Add("Print");
			GrdHistoryMain.RootTable.Columns["Print"].Caption = "Print";
			GrdHistoryMain.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistoryMain.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			GrdHistoryMain.RootTable.Columns["Print"].ButtonText = "Print";
			GrdHistoryMain.RootTable.Columns["Print"].Width = 50;
			GrdHistoryMain.RootTable.Columns["Print"].Position = 1;
			GrdHistoryMain.RootTable.Columns.Add("Voucher");
			GrdHistoryMain.RootTable.Columns["Voucher"].Caption = "Voucher";
			GrdHistoryMain.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistoryMain.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			GrdHistoryMain.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			GrdHistoryMain.RootTable.Columns["Voucher"].Width = 80;
			GrdHistoryMain.RootTable.Columns["Voucher"].Position = 2;
			GrdHistoryMain.FrozenColumns = 3;
			ctrlGrdBar1_Load(null, null);
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
			datBillDate.Focus();
		}
	}

	private void btnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			ToDateHistory.Value = DateTime.Now;
			FromNoHistory.Text = string.Empty;
			ToDocNoHistory.Text = string.Empty;
			((Control)(object)CmbJobOrderHistory).Text = string.Empty;
			GrdHistoryMain.ClearStructure();
			GrdHistoryInput.ClearStructure();
			grdhistoryBpPurchased.ClearStructure();
			grdHistoryPM.ClearStructure();
			grdhistoryOH.ClearStructure();
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
			HistoryCombosFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			BindHistoryGrid();
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
			GridEXRow r = GrdHistoryMain.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				ReadById(CurrId);
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
			GridEXRow r = GrdHistoryMain.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				int JobOrderId = Conversion.ToInt(r.Cells["InvProductionJobOrderId"].Value);
				if (e.Column.Key == "Edit")
				{
					RECID = CurrId;
					ReadById(RECID);
				}
				if (e.Column.Key == "Print")
				{
					CrystalReportPrint_Helper.ProductionPartyProcessingBillReport_611(JobOrderId, CurrId);
				}
				if (e.Column.Key == "Voucher")
				{
					CommonServices.VoucherReport_118(CommonServices.VoucherHeadIdGet(CurrId, 123));
				}
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
			GridEXRow r = GrdHistoryMain.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int id = Conversion.ToInt(r.Cells["Id"].Value);
				Conversion.ToInt(r.Cells["InvProductionJobOrderId"].Value);
				CommonServices.GetNoofAttachmentsByScreenName(id, base.Name);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = GrdHistoryMain.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				int JobOrderId = Conversion.ToInt(r.Cells["InvProductionJobOrderId"].Value);
				BindHistoryDetail(CurrId, JobOrderId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindHistoryDetail(int Id, int JobOrderId)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		try
		{
			InvProductionProcessingBill obj = InvProductionProcessingBill.GetByID(Id);
			if (obj != null)
			{
				DataTable dt = InvProductionProcessingBill.GetInPutForProcessingBill(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id,
					DocumentTypeId = 117,
					JobOrderId = JobOrderId
				});
				if (dt.Rows.Count > 0)
				{
					DataTable dtTarget = new DataTable();
					dtTarget.Columns.Add("ItemId", typeof(int));
					dtTarget.Columns.Add("ItemName", typeof(string));
					dtTarget.Columns.Add("ItemCode", typeof(string));
					dtTarget.Columns.Add("Uom", typeof(decimal));
					dtTarget.Columns.Add("Qty", typeof(decimal));
					dtTarget.Columns.Add("StockWeight", typeof(decimal));
					foreach (DataRow row in dt.Rows)
					{
						dtTarget.Rows.Add(row["ItemId"], row["ItemName"], row["ItemCode"], row["UomEquivalent"], row["Qty"], row["StockWeight"]);
					}
					GrdHistoryInput.DataSource = dtTarget;
					GrdHistoryInput.RetrieveStructure();
					grdHistoryInputSetting();
				}
				else
				{
					GrdHistoryInput.ClearStructure();
				}
				if (obj.InvProductionProcessingBillOutPutslist.Count > 0)
				{
					DataTable dtOutPutHistory = new DataTable();
					dtOutPutHistory.Columns.Add("ItemCode");
					dtOutPutHistory.Columns.Add("ItemName");
					dtOutPutHistory.Columns.Add("Qty", typeof(double));
					dtOutPutHistory.Columns.Add("UOM");
					dtOutPutHistory.Columns.Add("Weight", typeof(double));
					dtOutPutHistory.Columns.Add("Rate", typeof(double));
					dtOutPutHistory.Columns.Add("Amount", typeof(double));
					dtOutPutHistory.Columns.Add("Remarks");
					dtOutPutHistory.Columns.Add("DebitAccount");
					dtOutPutHistory.Rows.Clear();
					for (int k = 0; k < obj.InvProductionProcessingBillOutPutslist.Count; k++)
					{
						dtOutPutHistory.Rows.Add(obj.InvProductionProcessingBillOutPutslist[k].ItemCode, obj.InvProductionProcessingBillOutPutslist[k].ItemName, obj.InvProductionProcessingBillOutPutslist[k].Qty, obj.InvProductionProcessingBillOutPutslist[k].ItemUomId, obj.InvProductionProcessingBillOutPutslist[k].Weight, obj.InvProductionProcessingBillOutPutslist[k].Rate, obj.InvProductionProcessingBillOutPutslist[k].Amount, obj.InvProductionProcessingBillOutPutslist[k].Remarks, obj.InvProductionProcessingBillOutPutslist[k].DebitAccount);
					}
					grdhistoryBpPurchased.DataSource = dtOutPutHistory;
					grdhistoryBpPurchased.RetrieveStructure();
					HistoryGridOutPutBPSetting();
				}
				else
				{
					grdhistoryBpPurchased.ClearStructure();
				}
				if (obj.InvProductionProcessingBillPMslist.Count > 0)
				{
					DataTable dtHistoryPM = new DataTable();
					dtHistoryPM.Columns.Add("ItemCode");
					dtHistoryPM.Columns.Add("ItemName");
					dtHistoryPM.Columns.Add("Qty", typeof(double));
					dtHistoryPM.Columns.Add("Rate");
					dtHistoryPM.Columns.Add("Amount", typeof(double));
					dtHistoryPM.Columns.Add("Remarks");
					dtHistoryPM.Rows.Clear();
					grdHistoryPM.ClearStructure();
					for (int j = 0; j < obj.InvProductionProcessingBillPMslist.Count; j++)
					{
						dtHistoryPM.Rows.Add(obj.InvProductionProcessingBillPMslist[j].ItemCode, obj.InvProductionProcessingBillPMslist[j].ItemName, obj.InvProductionProcessingBillPMslist[j].Qty, obj.InvProductionProcessingBillPMslist[j].Rate, obj.InvProductionProcessingBillPMslist[j].Amount, obj.InvProductionProcessingBillPMslist[j].Remarks);
					}
					grdHistoryPM.DataSource = dtHistoryPM;
					grdHistoryPM.RetrieveStructure();
					HistoryGridPMSetting();
				}
				else
				{
					grdHistoryPM.ClearStructure();
				}
				if (obj.InvProductionProcessingBillOHslist.Count > 0)
				{
					DataTable dtHistoryOH = new DataTable();
					dtHistoryOH.Columns.Add("Account");
					dtHistoryOH.Columns.Add("Qty", typeof(double));
					dtHistoryOH.Columns.Add("Rate");
					dtHistoryOH.Columns.Add("Amount", typeof(double));
					dtHistoryOH.Columns.Add("Remarks");
					dtHistoryOH.Rows.Clear();
					grdhistoryOH.ClearStructure();
					for (int i = 0; i < obj.InvProductionProcessingBillOHslist.Count; i++)
					{
						dtHistoryOH.Rows.Add(obj.InvProductionProcessingBillOHslist[i].AccountTitle, obj.InvProductionProcessingBillOHslist[i].Qty, obj.InvProductionProcessingBillOHslist[i].Rate, obj.InvProductionProcessingBillOHslist[i].Amount, obj.InvProductionProcessingBillOHslist[i].OHRemarks);
					}
					grdhistoryOH.DataSource = dtHistoryOH;
					grdhistoryOH.RetrieveStructure();
					HistoryGridOHSetting();
				}
				else
				{
					grdhistoryOH.ClearStructure();
				}
			}
			else
			{
				GrdHistoryInput.ClearStructure();
				grdhistoryBpPurchased.ClearStructure();
				grdHistoryPM.ClearStructure();
				grdhistoryOH.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridPMSetting()
	{
		try
		{
			grdHistoryPM.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdHistoryPM.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdHistoryPM.RootTable.Columns["Amount"].FormatString = "#,0##.##";
			grdHistoryPM.RootTable.Columns["Amount"].TotalFormatString = "#,0##.##";
			grdHistoryPM.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryPM.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdHistoryPM.RootTable.Columns["Qty"].FormatString = "#,0##.##";
			grdHistoryPM.RootTable.Columns["Qty"].TotalFormatString = "#,0##.##";
			CommonServices.GridAutoAdjustmentNew(grdHistoryPM);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridOutPutBPSetting()
	{
		try
		{
			grdhistoryBpPurchased.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdhistoryBpPurchased.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdhistoryBpPurchased.RootTable.Columns["Amount"].FormatString = "#,##.##";
			grdhistoryBpPurchased.RootTable.Columns["Amount"].TotalFormatString = "#,##.##";
			grdhistoryBpPurchased.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdhistoryBpPurchased.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdhistoryBpPurchased.RootTable.Columns["Qty"].FormatString = "#,##.##";
			grdhistoryBpPurchased.RootTable.Columns["Qty"].TotalFormatString = "#,##.##";
			grdhistoryBpPurchased.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdhistoryBpPurchased.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdhistoryBpPurchased.RootTable.Columns["Weight"].FormatString = "#,##.##";
			grdhistoryBpPurchased.RootTable.Columns["Weight"].TotalFormatString = "#,##.##";
			CommonServices.GridAutoAdjustmentNew(grdhistoryBpPurchased);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridOHSetting()
	{
		try
		{
			grdhistoryOH.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdhistoryOH.RootTable.Columns["Rate"].FormatString = "#,##.###";
			grdhistoryOH.RootTable.Columns["Rate"].TotalFormatString = "#,##.###";
			grdhistoryOH.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdhistoryOH.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdhistoryOH.RootTable.Columns["Amount"].FormatString = "#,##.##";
			grdhistoryOH.RootTable.Columns["Amount"].TotalFormatString = "#,##.##";
			grdhistoryOH.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdhistoryOH.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdhistoryOH.RootTable.Columns["Qty"].FormatString = "#,##.##";
			grdhistoryOH.RootTable.Columns["Qty"].TotalFormatString = "#,##.##";
			CommonServices.GridAutoAdjustmentNew(grdhistoryOH);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryInputSetting()
	{
		try
		{
			GrdHistoryInput.RootTable.Columns["ItemId"].Visible = false;
			GrdHistoryInput.RootTable.Columns["Uom"].TextAlignment = (TextAlignment)3;
			GrdHistoryInput.RootTable.Columns["Uom"].FormatString = "#,##.###";
			GrdHistoryInput.RootTable.Columns["Uom"].TotalFormatString = "#,##.###";
			GrdHistoryInput.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryInput.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			GrdHistoryInput.RootTable.Columns["Qty"].FormatString = "#,##.###";
			GrdHistoryInput.RootTable.Columns["Qty"].TotalFormatString = "#,##.###";
			GrdHistoryInput.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryInput.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			GrdHistoryInput.RootTable.Columns["StockWeight"].FormatString = "#,##.###";
			GrdHistoryInput.RootTable.Columns["StockWeight"].TotalFormatString = "#,##.###";
			CommonServices.GridAutoAdjustmentNew(GrdHistoryInput);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FromNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void frmProductionJobOrder_KeyDown(object sender, KeyEventArgs e)
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
					datBillDate.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					((Control)(object)GrdHistoryMain).Focus();
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
					datBillDate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					datBillDate.Focus();
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
				if (e.Control && e.KeyCode == Keys.Left)
				{
					if (((Control)(object)grdInput).Focused)
					{
						((Control)(object)grdByProducts).Focus();
					}
					else if (((Control)(object)grdByProducts).Focused)
					{
						((Control)(object)grdPM).Focus();
					}
					else if (((Control)(object)grdPM).Focused)
					{
						((Control)(object)grdOH).Focus();
					}
					else
					{
						((Control)(object)grdInput).Focus();
					}
				}
				return;
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
			if (e.Control && e.KeyCode == Keys.Left)
			{
				if (((Control)(object)GrdHistoryMain).Focused)
				{
					((Control)(object)GrdHistoryInput).Focus();
				}
				else if (((Control)(object)GrdHistoryInput).Focused)
				{
					((Control)(object)grdhistoryBpPurchased).Focus();
				}
				else if (((Control)(object)grdhistoryBpPurchased).Focused)
				{
					((Control)(object)grdHistoryPM).Focus();
				}
				else if (((Control)(object)grdHistoryPM).Focused)
				{
					((Control)(object)grdhistoryOH).Focus();
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
			int JobOrderId = Conversion.ToInt(r.Cells["InvProductionJobOrderId"].Value);
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (GrdHistoryMain.CurrentColumn.Key == "Edit")
				{
					RECID = CurrId;
					ReadById(RECID);
				}
				if (GrdHistoryMain.CurrentColumn.Key == "Print")
				{
					CrystalReportPrint_Helper.ProductionPartyProcessingBillReport_611(JobOrderId, CurrId);
				}
				if (GrdHistoryMain.CurrentColumn.Key == "Voucher")
				{
					CommonServices.VoucherReport_118(CommonServices.VoucherHeadIdGet(CurrId, 123));
				}
				if (GrdHistoryMain.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByScreenName(CurrId, base.Name);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				RECID = CurrId;
				ReadById(RECID);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				CrystalReportPrint_Helper.ProductionPartyProcessingBillReport_611(JobOrderId, CurrId);
			}
			((Control)(object)GrdHistoryMain).Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdByProducts_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		try
		{
			GridEXRow r = grdByProducts.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdByProducts.CurrentColumn == null)
			{
				return;
			}
			Conversion.ToInt(r.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Space && grdByProducts.CurrentColumn.Key == "Delete")
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					InvProductionProcessingBillOutPut d2 = new InvProductionProcessingBillOutPut();
					d2.Id = Conversion.ToInt(r.Cells["Id"].Value);
					d2.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					d2.ItemUomId = Conversion.ToInt(r.Cells["PackSize"].Value);
					d2.Qty = Conversion.ToDecimal(r.Cells["PurchaseQty"].Value);
					d2.Weight = Conversion.ToDecimal(r.Cells["PurchaseWeight"].Value);
					d2.Rate = Conversion.ToDecimal(r.Cells["PurchaseRate"].Value);
					d2.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
					d2.DebitAccountId = Conversion.ToInt(r.Cells["DebitAccountId"].Value);
					d2.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
					d2.ActionTypeId = 3;
					lstRemoveOutputDetailRecord.Add(d2);
					dtByProduct.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				else
				{
					dtByProduct.Rows[r.RowIndex].Delete();
					r.Delete();
				}
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					InvProductionProcessingBillOutPut d = new InvProductionProcessingBillOutPut();
					d.Id = Conversion.ToInt(r.Cells["Id"].Value);
					d.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					d.ItemUomId = Conversion.ToInt(r.Cells["PackSize"].Value);
					d.Qty = Conversion.ToDecimal(r.Cells["PurchaseQty"].Value);
					d.Weight = Conversion.ToDecimal(r.Cells["PurchaseWeight"].Value);
					d.Rate = Conversion.ToDecimal(r.Cells["PurchaseRate"].Value);
					d.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
					d.DebitAccountId = Conversion.ToInt(r.Cells["DebitAccountId"].Value);
					d.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
					d.ActionTypeId = 3;
					lstRemoveOutputDetailRecord.Add(d);
					dtByProduct.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				else
				{
					dtByProduct.Rows[r.RowIndex].Delete();
					r.Delete();
				}
			}
			BillAmountCalculations();
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
				if (grdPM.CurrentColumn.Key == "Delete")
				{
					dePMdetail.Rows[r.RowIndex].Delete();
					r.Delete();
					if (dePMdetail.Rows.Count == 0)
					{
						AddRowInPackingMaterialGrid();
					}
				}
				if (grdPM.CurrentColumn.Key == "Add")
				{
					AddRowInPackingMaterialGrid();
				}
				grdPM.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				dePMdetail.Rows[r.RowIndex].Delete();
				r.Delete();
				if (dePMdetail.Rows.Count == 0)
				{
					AddRowInPackingMaterialGrid();
				}
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInPackingMaterialGrid();
			}
			BillAmountCalculations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOH_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdOH.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdOH.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdOH.CurrentColumn.Key == "Delete")
				{
					dtOHDetail.Rows[r.RowIndex].Delete();
					r.Delete();
					if (dtOHDetail.Rows.Count == 0)
					{
						AddRowInExpenseGrid();
					}
				}
				if (grdOH.CurrentColumn.Key == "Add")
				{
					dtOHDetail.Rows.Add(0, 0, 0, 0, "");
					grdOH.DataSource = dtOHDetail;
				}
				grdOH.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				dtOHDetail.Rows[r.RowIndex].Delete();
				r.Delete();
				if (dtOHDetail.Rows.Count == 0)
				{
					AddRowInExpenseGrid();
				}
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				dtOHDetail.Rows.Add(0, 0, 0, 0, "");
				grdOH.DataSource = dtOHDetail;
			}
			BillAmountCalculations();
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
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Expected O, but got Unknown
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Expected O, but got Unknown
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Expected O, but got Unknown
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Expected O, but got Unknown
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Expected O, but got Unknown
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Expected O, but got Unknown
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Expected O, but got Unknown
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Expected O, but got Unknown
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Expected O, but got Unknown
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Expected O, but got Unknown
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Expected O, but got Unknown
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Expected O, but got Unknown
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Expected O, but got Unknown
		//IL_046f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Expected O, but got Unknown
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Expected O, but got Unknown
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d1: Expected O, but got Unknown
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Expected O, but got Unknown
		//IL_05fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0605: Expected O, but got Unknown
		//IL_0627: Unknown result type (might be due to invalid IL or missing references)
		//IL_0631: Expected O, but got Unknown
		//IL_065e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0668: Expected O, but got Unknown
		//IL_068a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0694: Expected O, but got Unknown
		//IL_0d47: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d51: Expected O, but got Unknown
		//IL_0d5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d68: Expected O, but got Unknown
		//IL_3ac1: Unknown result type (might be due to invalid IL or missing references)
		//IL_3acb: Expected O, but got Unknown
		//IL_3ad8: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ae2: Expected O, but got Unknown
		//IL_3d80: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d8a: Expected O, but got Unknown
		//IL_3d97: Unknown result type (might be due to invalid IL or missing references)
		//IL_3da1: Expected O, but got Unknown
		//IL_40a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_40b2: Expected O, but got Unknown
		//IL_40d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_40e0: Expected O, but got Unknown
		GridEXLayout grdByProducts_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInput_DesignTimeLayout = new GridEXLayout();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
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
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
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
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.PartyProcessing.ProductionPartyProcessingBill));
		GridEXLayout grdOH_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPM_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdHistoryMain_DesignTimeLayout = new GridEXLayout();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		GridEXLayout grdhistoryBpPurchased_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdHistoryInput_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdhistoryOH_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryPM_DesignTimeLayout = new GridEXLayout();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grdByProducts = new GridEX();
		this.panel19 = new System.Windows.Forms.Panel();
		this.infoLabel1 = new CameraControl.InfoLabel();
		this.label13 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label19 = new System.Windows.Forms.Label();
		this.txtPlanTypeSrNo = new System.Windows.Forms.TextBox();
		this.grdInput = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label6 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.txtBillAmount = new System.Windows.Forms.TextBox();
		this.label21 = new System.Windows.Forms.Label();
		this.txtPMAmount = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.txtOHAmount = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.txtBPPurchaseTotalAmt = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.txtBillNo = new System.Windows.Forms.TextBox();
		this.CmbJobOrderNo = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.datBillDate = new System.Windows.Forms.DateTimePicker();
		this.label12 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.CmbRateUom = new UltraCombo();
		this.txtProcesssingAmount = new System.Windows.Forms.TextBox();
		this.txtNoOfBags = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.txtRate = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.CmbCreditAc = new UltraCombo();
		this.CmbStockParty = new UltraCombo();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.label20 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.BtnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.panel18 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grdOH = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label7 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdPM = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label8 = new System.Windows.Forms.Label();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.GrdHistoryMain = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label10 = new System.Windows.Forms.Label();
		this.panel58 = new System.Windows.Forms.Panel();
		this.RdApproveDate = new System.Windows.Forms.RadioButton();
		this.RdModifyDate = new System.Windows.Forms.RadioButton();
		this.RrDocDate = new System.Windows.Forms.RadioButton();
		this.RdEntryDate = new System.Windows.Forms.RadioButton();
		this.groupBox11 = new System.Windows.Forms.GroupBox();
		this.CmbJobOrderHistory = new UltraCombo();
		this.BtnShowHistory = new System.Windows.Forms.Button();
		this.label145 = new System.Windows.Forms.Label();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label146 = new System.Windows.Forms.Label();
		this.label147 = new System.Windows.Forms.Label();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label148 = new System.Windows.Forms.Label();
		this.label149 = new System.Windows.Forms.Label();
		this.FromNoHistory = new System.Windows.Forms.TextBox();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.panel20 = new System.Windows.Forms.Panel();
		this.panel16 = new System.Windows.Forms.Panel();
		this.grdhistoryBpPurchased = new GridEX();
		this.panel17 = new System.Windows.Forms.Panel();
		this.label16 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.GrdHistoryInput = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.panel21 = new System.Windows.Forms.Panel();
		this.panel31 = new System.Windows.Forms.Panel();
		this.grdhistoryOH = new GridEX();
		this.panel32 = new System.Windows.Forms.Panel();
		this.label18 = new System.Windows.Forms.Label();
		this.panel26 = new System.Windows.Forms.Panel();
		this.grdHistoryPM = new GridEX();
		this.panel23 = new System.Windows.Forms.Panel();
		this.label17 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdByProducts).BeginInit();
		this.panel19.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInput).BeginInit();
		this.panel11.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStockParty).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.panel18.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOH).BeginInit();
		this.panel13.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPM).BeginInit();
		this.panel14.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryMain).BeginInit();
		this.panel12.SuspendLayout();
		this.panel58.SuspendLayout();
		this.groupBox11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderHistory).BeginInit();
		this.panel8.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.panel20.SuspendLayout();
		this.panel16.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistoryBpPurchased).BeginInit();
		this.panel17.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryInput).BeginInit();
		this.panel15.SuspendLayout();
		this.panel21.SuspendLayout();
		this.panel31.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistoryOH).BeginInit();
		this.panel32.SuspendLayout();
		this.panel26.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryPM).BeginInit();
		this.panel23.SuspendLayout();
		base.SuspendLayout();
		this.miniToolStrip.AccessibleName = "New item selection";
		this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
		this.miniToolStrip.AutoSize = false;
		this.miniToolStrip.CanOverflow = false;
		this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.miniToolStrip.Location = new System.Drawing.Point(250, 4);
		this.miniToolStrip.Name = "miniToolStrip";
		this.miniToolStrip.Size = new System.Drawing.Size(1363, 27);
		this.miniToolStrip.TabIndex = 3;
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1108, 561);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1108, 561);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel18);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1100, 532);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 165);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1094, 199);
		this.panel2.TabIndex = 0;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdByProducts);
		this.panel6.Controls.Add(this.panel19);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(539, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(555, 199);
		this.panel6.TabIndex = 178;
		grdByProducts_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdByProducts.DesignTimeLayout = grdByProducts_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdByProducts).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdByProducts.DynamicFiltering = true;
		this.grdByProducts.FilterRowButtonStyle = (FilterRowButtonStyle)1;
		((System.Windows.Forms.Control)(object)this.grdByProducts).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdByProducts.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdByProducts).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grdByProducts).Name = "grdByProducts";
		((ScrollableBase)this.grdByProducts).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdByProducts).Size = new System.Drawing.Size(555, 177);
		((System.Windows.Forms.Control)(object)this.grdByProducts).TabIndex = 0;
		this.grdByProducts.TotalRow = (InheritableBoolean)1;
		this.grdByProducts.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdByProducts).VisualStyle = (VisualStyle)4;
		this.grdByProducts.CellUpdated += new ColumnActionEventHandler(grdByProducts_CellUpdated);
		this.grdByProducts.ColumnButtonClick += new ColumnActionEventHandler(grdByProducts_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdByProducts).KeyDown += new System.Windows.Forms.KeyEventHandler(grdByProducts_KeyDown);
		this.panel19.BackColor = System.Drawing.Color.Teal;
		this.panel19.Controls.Add(this.infoLabel1);
		this.panel19.Controls.Add(this.label13);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel19.Location = new System.Drawing.Point(0, 0);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(555, 22);
		this.panel19.TabIndex = 2;
		this.infoLabel1.AutoSize = true;
		this.infoLabel1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.infoLabel1.ForeColor = System.Drawing.Color.White;
		this.infoLabel1.Location = new System.Drawing.Point(203, 5);
		this.infoLabel1.Name = "infoLabel1";
		this.infoLabel1.Size = new System.Drawing.Size(272, 15);
		this.infoLabel1.TabIndex = 1;
		this.infoLabel1.Text = "Please remove the row that you don't want to save";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(8, 4);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(134, 16);
		this.label13.TabIndex = 0;
		this.label13.Text = "OutPut By Products";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.label19);
		this.panel4.Controls.Add(this.txtPlanTypeSrNo);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdInput);
		this.panel4.Controls.Add(this.panel11);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(539, 199);
		this.panel4.TabIndex = 61;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(1213, 35);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(97, 16);
		this.label19.TabIndex = 165;
		this.label19.Text = "PlanTypeSrNo";
		this.label19.Visible = false;
		this.txtPlanTypeSrNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPlanTypeSrNo.Enabled = false;
		this.txtPlanTypeSrNo.Location = new System.Drawing.Point(1216, 7);
		this.txtPlanTypeSrNo.Name = "txtPlanTypeSrNo";
		this.txtPlanTypeSrNo.Size = new System.Drawing.Size(85, 23);
		this.txtPlanTypeSrNo.TabIndex = 164;
		this.txtPlanTypeSrNo.Visible = false;
		this.grdInput.AllowEdit = (InheritableBoolean)2;
		this.grdInput.ColumnAutoResize = true;
		grdInput_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInput.DesignTimeLayout = grdInput_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInput).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdInput.DynamicFiltering = true;
		this.grdInput.FilterRowButtonStyle = (FilterRowButtonStyle)1;
		((System.Windows.Forms.Control)(object)this.grdInput).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInput.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInput).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grdInput).Name = "grdInput";
		((ScrollableBase)this.grdInput).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInput).Size = new System.Drawing.Size(539, 177);
		((System.Windows.Forms.Control)(object)this.grdInput).TabIndex = 0;
		this.grdInput.TotalRow = (InheritableBoolean)1;
		this.grdInput.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInput).VisualStyle = (VisualStyle)4;
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label6);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(539, 22);
		this.panel11.TabIndex = 2;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(2, 4);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(83, 16);
		this.label6.TabIndex = 0;
		this.label6.Text = "InPut Detail";
		this.panel5.Controls.Add(this.txtBillAmount);
		this.panel5.Controls.Add(this.label21);
		this.panel5.Controls.Add(this.txtPMAmount);
		this.panel5.Controls.Add(this.label24);
		this.panel5.Controls.Add(this.txtOHAmount);
		this.panel5.Controls.Add(this.label22);
		this.panel5.Controls.Add(this.txtBPPurchaseTotalAmt);
		this.panel5.Controls.Add(this.label14);
		this.panel5.Controls.Add(this.txtBillNo);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobOrderNo);
		this.panel5.Controls.Add(this.label5);
		this.panel5.Controls.Add(this.label23);
		this.panel5.Controls.Add(this.datBillDate);
		this.panel5.Controls.Add(this.label12);
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRateUom);
		this.panel5.Controls.Add(this.txtProcesssingAmount);
		this.panel5.Controls.Add(this.txtNoOfBags);
		this.panel5.Controls.Add(this.label4);
		this.panel5.Controls.Add(this.txtRate);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Controls.Add(this.label11);
		this.panel5.Controls.Add(this.label25);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Controls.Add(this.txtRemarks);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCreditAc);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbStockParty);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 54);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1094, 111);
		this.panel5.TabIndex = 0;
		this.txtBillAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBillAmount.Enabled = false;
		this.txtBillAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBillAmount.Location = new System.Drawing.Point(955, 84);
		this.txtBillAmount.Name = "txtBillAmount";
		this.txtBillAmount.Size = new System.Drawing.Size(134, 23);
		this.txtBillAmount.TabIndex = 13;
		this.txtBillAmount.TabStop = false;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(862, 88);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(70, 15);
		this.label21.TabIndex = 178;
		this.label21.Text = "Bill Amount";
		this.txtPMAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPMAmount.Enabled = false;
		this.txtPMAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPMAmount.Location = new System.Drawing.Point(955, 33);
		this.txtPMAmount.Name = "txtPMAmount";
		this.txtPMAmount.Size = new System.Drawing.Size(134, 23);
		this.txtPMAmount.TabIndex = 11;
		this.txtPMAmount.TabStop = false;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(862, 37);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(95, 15);
		this.label24.TabIndex = 178;
		this.label24.Text = "Packing Material";
		this.txtOHAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOHAmount.Enabled = false;
		this.txtOHAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtOHAmount.Location = new System.Drawing.Point(955, 5);
		this.txtOHAmount.Name = "txtOHAmount";
		this.txtOHAmount.Size = new System.Drawing.Size(134, 23);
		this.txtOHAmount.TabIndex = 10;
		this.txtOHAmount.TabStop = false;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(862, 9);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(63, 15);
		this.label22.TabIndex = 178;
		this.label22.Text = "Over Head";
		this.txtBPPurchaseTotalAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBPPurchaseTotalAmt.Enabled = false;
		this.txtBPPurchaseTotalAmt.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBPPurchaseTotalAmt.Location = new System.Drawing.Point(955, 59);
		this.txtBPPurchaseTotalAmt.Name = "txtBPPurchaseTotalAmt";
		this.txtBPPurchaseTotalAmt.Size = new System.Drawing.Size(134, 23);
		this.txtBPPurchaseTotalAmt.TabIndex = 12;
		this.txtBPPurchaseTotalAmt.TabStop = false;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(862, 63);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(76, 15);
		this.label14.TabIndex = 178;
		this.label14.Text = "BP Total Amt";
		this.txtBillNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBillNo.Enabled = false;
		this.txtBillNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBillNo.Location = new System.Drawing.Point(80, 5);
		this.txtBillNo.Name = "txtBillNo";
		this.txtBillNo.Size = new System.Drawing.Size(82, 23);
		this.txtBillNo.TabIndex = 0;
		this.CmbJobOrderNo.AllowNull = (DefaultableBoolean)1;
		this.CmbJobOrderNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobOrderNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobOrderNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance131).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance131).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance132;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance133).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance137).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance138).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance139).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNo).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbJobOrderNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNo).Location = new System.Drawing.Point(80, 32);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNo).Name = "CmbJobOrderNo";
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNo).Size = new System.Drawing.Size(208, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNo).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNo).TextChanged += new System.EventHandler(CmbJobOrderNo_TextChanged);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNo).Leave += new System.EventHandler(CmbJobOrderNo_Leave);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(4, 9);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(51, 15);
		this.label5.TabIndex = 63;
		this.label5.Text = "Bill Date";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(4, 38);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(78, 15);
		this.label23.TabIndex = 176;
		this.label23.Text = "Job Order No";
		this.datBillDate.CustomFormat = "dd-MMM-yy";
		this.datBillDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.datBillDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datBillDate.Location = new System.Drawing.Point(164, 5);
		this.datBillDate.Name = "datBillDate";
		this.datBillDate.Size = new System.Drawing.Size(124, 23);
		this.datBillDate.TabIndex = 1;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(292, 38);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(86, 15);
		this.label12.TabIndex = 71;
		this.label12.Text = "Credit Account";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(292, 9);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(67, 15);
		this.label2.TabIndex = 71;
		this.label2.Text = "Stock Party";
		this.CmbRateUom.AllowNull = (DefaultableBoolean)1;
		this.CmbRateUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance144;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance145).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance145).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance145).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance148;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance149).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance150).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance150).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance150).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbRateUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Location = new System.Drawing.Point(730, 31);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Name = "CmbRateUom";
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Size = new System.Drawing.Size(127, 26);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).TextChanged += new System.EventHandler(CmbRateUom_TextChanged);
		this.txtProcesssingAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtProcesssingAmount.Enabled = false;
		this.txtProcesssingAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtProcesssingAmount.Location = new System.Drawing.Point(730, 84);
		this.txtProcesssingAmount.Name = "txtProcesssingAmount";
		this.txtProcesssingAmount.Size = new System.Drawing.Size(127, 23);
		this.txtProcesssingAmount.TabIndex = 9;
		this.txtProcesssingAmount.TabStop = false;
		this.txtNoOfBags.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNoOfBags.Enabled = false;
		this.txtNoOfBags.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtNoOfBags.Location = new System.Drawing.Point(730, 59);
		this.txtNoOfBags.Name = "txtNoOfBags";
		this.txtNoOfBags.Size = new System.Drawing.Size(127, 23);
		this.txtNoOfBags.TabIndex = 8;
		this.txtNoOfBags.TabStop = false;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(613, 88);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(116, 15);
		this.label4.TabIndex = 75;
		this.label4.Text = "Processsing Amount";
		this.txtRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRate.Location = new System.Drawing.Point(730, 5);
		this.txtRate.Name = "txtRate";
		this.txtRate.Size = new System.Drawing.Size(127, 23);
		this.txtRate.TabIndex = 6;
		this.txtRate.TextChanged += new System.EventHandler(txtRate_TextChanged);
		this.txtRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRate_KeyPress);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(613, 63);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(65, 15);
		this.label1.TabIndex = 75;
		this.label1.Text = "No of Bags";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(4, 66);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(52, 15);
		this.label11.TabIndex = 153;
		this.label11.Text = "Remarks";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(613, 37);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(61, 15);
		this.label25.TabIndex = 75;
		this.label25.Text = "Rate_Uom";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(613, 9);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(90, 15);
		this.label3.TabIndex = 75;
		this.label3.Text = "Processing Rate";
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRemarks.Location = new System.Drawing.Point(80, 62);
		this.txtRemarks.Multiline = true;
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(528, 45);
		this.txtRemarks.TabIndex = 3;
		this.CmbCreditAc.AllowNull = (DefaultableBoolean)1;
		this.CmbCreditAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCreditAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCreditAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance155).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance155).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance156;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance157).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance157).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance157).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance158).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance159).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance101).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance102).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance103).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCreditAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCreditAc).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCreditAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCreditAc).Location = new System.Drawing.Point(378, 32);
		((System.Windows.Forms.Control)(object)this.CmbCreditAc).Name = "CmbCreditAc";
		((System.Windows.Forms.Control)(object)this.CmbCreditAc).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.CmbCreditAc).TabIndex = 5;
		this.CmbStockParty.AllowNull = (DefaultableBoolean)1;
		this.CmbStockParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbStockParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbStockParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbStockParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance108;
		((SpecialBoxBase)((UltraGridBase)this.CmbStockParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance114).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance115).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbStockParty).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbStockParty).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbStockParty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbStockParty).Location = new System.Drawing.Point(378, 3);
		((System.Windows.Forms.Control)(object)this.CmbStockParty).Name = "CmbStockParty";
		((System.Windows.Forms.Control)(object)this.CmbStockParty).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.CmbStockParty).TabIndex = 4;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ChkBox);
		this.panel3.Controls.Add(this.label20);
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1094, 51);
		this.panel3.TabIndex = 0;
		this.ChkBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBox.AutoSize = true;
		this.ChkBox.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBox.ForeColor = System.Drawing.Color.White;
		this.ChkBox.Location = new System.Drawing.Point(971, 29);
		this.ChkBox.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(122, 20);
		this.ChkBox.TabIndex = 58;
		this.ChkBox.Text = "Print Preview";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.ForeColor = System.Drawing.Color.White;
		this.label20.Location = new System.Drawing.Point(3, 29);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(127, 19);
		this.label20.TabIndex = 0;
		this.label20.Text = "Processing Bill";
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.btnnew, this.btnRefresh, this.btnsave, this.btnUpdate, this.btnDelete, this.btnattachment, this.BtnPrint, this.btnshortcutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1094, 27);
		this.toolStrip1.TabIndex = 57;
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
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
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
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.btnattachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.BtnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint.Name = "BtnPrint";
		this.BtnPrint.Size = new System.Drawing.Size(92, 24);
		this.BtnPrint.Text = "611-&Print";
		this.BtnPrint.Click += new System.EventHandler(Print_Click);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.panel18.Controls.Add(this.panel7);
		this.panel18.Controls.Add(this.panel10);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel18.Location = new System.Drawing.Point(3, 364);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1094, 165);
		this.panel18.TabIndex = 3;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdOH);
		this.panel7.Controls.Add(this.panel13);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(539, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(555, 165);
		this.panel7.TabIndex = 4;
		((ScrollableBase)this.grdOH).BackColor = System.Drawing.Color.White;
		grdOH_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOH.DesignTimeLayout = grdOH_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOH).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdOH).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOH.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOH).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grdOH).Name = "grdOH";
		((ScrollableBase)this.grdOH).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOH).Size = new System.Drawing.Size(555, 143);
		((System.Windows.Forms.Control)(object)this.grdOH).TabIndex = 0;
		this.grdOH.TotalRow = (InheritableBoolean)1;
		this.grdOH.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOH).VisualStyle = (VisualStyle)4;
		this.grdOH.CellUpdated += new ColumnActionEventHandler(grdOverHead_CellUpdated);
		this.grdOH.ColumnButtonClick += new ColumnActionEventHandler(grdOverHead_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdOH).KeyDown += new System.Windows.Forms.KeyEventHandler(grdOH_KeyDown);
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label7);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(555, 22);
		this.panel13.TabIndex = 3;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(3, 3);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(82, 16);
		this.label7.TabIndex = 1;
		this.label7.Text = "Over Heads";
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdPM);
		this.panel10.Controls.Add(this.panel14);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(539, 165);
		this.panel10.TabIndex = 3;
		this.grdPM.AutomaticSort = false;
		((ScrollableBase)this.grdPM).BackColor = System.Drawing.Color.White;
		this.grdPM.ColumnAutoResize = true;
		grdPM_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPM.DesignTimeLayout = grdPM_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPM).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPM.FilterRowButtonStyle = (FilterRowButtonStyle)1;
		((System.Windows.Forms.Control)(object)this.grdPM).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPM.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPM).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grdPM).Name = "grdPM";
		((ScrollableBase)this.grdPM).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPM).Size = new System.Drawing.Size(539, 143);
		((System.Windows.Forms.Control)(object)this.grdPM).TabIndex = 0;
		this.grdPM.TotalRow = (InheritableBoolean)1;
		this.grdPM.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPM).VisualStyle = (VisualStyle)4;
		this.grdPM.CellUpdated += new ColumnActionEventHandler(grdPackingMaterial_CellUpdated);
		this.grdPM.ColumnButtonClick += new ColumnActionEventHandler(grdPackingMaterial_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPM).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPM_KeyDown);
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.label8);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(539, 22);
		this.panel14.TabIndex = 3;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(2, 4);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(113, 16);
		this.label8.TabIndex = 1;
		this.label8.Text = "Packing Material";
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistoryMain);
		this.tabPage2.Controls.Add(this.panel12);
		this.tabPage2.Controls.Add(this.panel58);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Controls.Add(this.panel20);
		this.tabPage2.Controls.Add(this.panel21);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1100, 532);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.GrdHistoryMain.AllowEdit = (InheritableBoolean)2;
		GrdHistoryMain_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistoryMain.DesignTimeLayout = GrdHistoryMain_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistoryMain).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdHistoryMain.DynamicFiltering = true;
		this.GrdHistoryMain.FilterRowButtonStyle = (FilterRowButtonStyle)1;
		((System.Windows.Forms.Control)(object)this.GrdHistoryMain).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistoryMain.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdHistoryMain).Location = new System.Drawing.Point(3, 126);
		((System.Windows.Forms.Control)(object)this.GrdHistoryMain).Name = "GrdHistoryMain";
		((ScrollableBase)this.GrdHistoryMain).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistoryMain).Size = new System.Drawing.Size(1094, 132);
		((System.Windows.Forms.Control)(object)this.GrdHistoryMain).TabIndex = 1;
		this.GrdHistoryMain.TotalRow = (InheritableBoolean)1;
		this.GrdHistoryMain.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistoryMain).VisualStyle = (VisualStyle)4;
		this.GrdHistoryMain.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.GrdHistoryMain.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.GrdHistoryMain.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdHistoryMain).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.ctrlGrdBar1);
		this.panel12.Controls.Add(this.label10);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(3, 105);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1094, 21);
		this.panel12.TabIndex = 2;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = null;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1042, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
		this.ctrlGrdBar1.MyGrid = null;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(48, 20);
		this.ctrlGrdBar1.TabIndex = 72;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label10.ForeColor = System.Drawing.Color.White;
		this.label10.Location = new System.Drawing.Point(3, 2);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(119, 17);
		this.label10.TabIndex = 0;
		this.label10.Text = "Filtered Records";
		this.panel58.Controls.Add(this.RdApproveDate);
		this.panel58.Controls.Add(this.RdModifyDate);
		this.panel58.Controls.Add(this.RrDocDate);
		this.panel58.Controls.Add(this.RdEntryDate);
		this.panel58.Controls.Add(this.groupBox11);
		this.panel58.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel58.Location = new System.Drawing.Point(3, 55);
		this.panel58.Name = "panel58";
		this.panel58.Size = new System.Drawing.Size(1094, 50);
		this.panel58.TabIndex = 0;
		this.RdApproveDate.AutoSize = true;
		this.RdApproveDate.Font = new System.Drawing.Font("Segoe UI Semibold", 8f, System.Drawing.FontStyle.Bold);
		this.RdApproveDate.Location = new System.Drawing.Point(1002, 25);
		this.RdApproveDate.Name = "RdApproveDate";
		this.RdApproveDate.Size = new System.Drawing.Size(96, 17);
		this.RdApproveDate.TabIndex = 3;
		this.RdApproveDate.Text = "Approve Date";
		this.RdApproveDate.UseVisualStyleBackColor = true;
		this.RdModifyDate.AutoSize = true;
		this.RdModifyDate.Font = new System.Drawing.Font("Segoe UI Semibold", 8f, System.Drawing.FontStyle.Bold);
		this.RdModifyDate.Location = new System.Drawing.Point(1001, 7);
		this.RdModifyDate.Name = "RdModifyDate";
		this.RdModifyDate.Size = new System.Drawing.Size(89, 17);
		this.RdModifyDate.TabIndex = 3;
		this.RdModifyDate.Text = "Modify Date";
		this.RdModifyDate.UseVisualStyleBackColor = true;
		this.RrDocDate.AutoSize = true;
		this.RrDocDate.Checked = true;
		this.RrDocDate.Font = new System.Drawing.Font("Segoe UI Semibold", 8f, System.Drawing.FontStyle.Bold);
		this.RrDocDate.Location = new System.Drawing.Point(927, 7);
		this.RrDocDate.Name = "RrDocDate";
		this.RrDocDate.Size = new System.Drawing.Size(72, 17);
		this.RrDocDate.TabIndex = 1;
		this.RrDocDate.TabStop = true;
		this.RrDocDate.Text = "Doc Date";
		this.RrDocDate.UseVisualStyleBackColor = true;
		this.RdEntryDate.AutoSize = true;
		this.RdEntryDate.Font = new System.Drawing.Font("Segoe UI Semibold", 8f, System.Drawing.FontStyle.Bold);
		this.RdEntryDate.Location = new System.Drawing.Point(927, 25);
		this.RdEntryDate.Name = "RdEntryDate";
		this.RdEntryDate.Size = new System.Drawing.Size(78, 17);
		this.RdEntryDate.TabIndex = 2;
		this.RdEntryDate.Text = "Entry Date";
		this.RdEntryDate.UseVisualStyleBackColor = true;
		this.groupBox11.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobOrderHistory);
		this.groupBox11.Controls.Add(this.BtnShowHistory);
		this.groupBox11.Controls.Add(this.label145);
		this.groupBox11.Controls.Add(this.ToDateHistory);
		this.groupBox11.Controls.Add(this.label146);
		this.groupBox11.Controls.Add(this.label147);
		this.groupBox11.Controls.Add(this.ToDocNoHistory);
		this.groupBox11.Controls.Add(this.FromDateHistory);
		this.groupBox11.Controls.Add(this.label148);
		this.groupBox11.Controls.Add(this.label149);
		this.groupBox11.Controls.Add(this.FromNoHistory);
		this.groupBox11.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox11.Location = new System.Drawing.Point(1, 2);
		this.groupBox11.Name = "groupBox11";
		this.groupBox11.Size = new System.Drawing.Size(924, 46);
		this.groupBox11.TabIndex = 0;
		this.groupBox11.TabStop = false;
		this.groupBox11.Text = "Filters";
		this.CmbJobOrderHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbJobOrderHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobOrderHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobOrderHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance120;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance121).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance121).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance125).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance126).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance126).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance126).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobOrderHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderHistory).Location = new System.Drawing.Point(680, 14);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderHistory).Name = "CmbJobOrderHistory";
		((System.Windows.Forms.Control)(object)this.CmbJobOrderHistory).Size = new System.Drawing.Size(183, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderHistory).TabIndex = 4;
		this.BtnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.BtnShowHistory.FlatAppearance.BorderSize = 0;
		this.BtnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShowHistory.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnShowHistory.ForeColor = System.Drawing.Color.White;
		this.BtnShowHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnShowHistory.Location = new System.Drawing.Point(865, 15);
		this.BtnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.BtnShowHistory.Name = "BtnShowHistory";
		this.BtnShowHistory.Size = new System.Drawing.Size(54, 25);
		this.BtnShowHistory.TabIndex = 5;
		this.BtnShowHistory.Text = "Show";
		this.BtnShowHistory.UseVisualStyleBackColor = false;
		this.BtnShowHistory.Click += new System.EventHandler(BtnShowHistory_Click);
		this.label145.AutoSize = true;
		this.label145.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label145.Location = new System.Drawing.Point(622, 20);
		this.label145.Name = "label145";
		this.label145.Size = new System.Drawing.Size(59, 15);
		this.label145.TabIndex = 178;
		this.label145.Text = "Job Order";
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(252, 16);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(133, 23);
		this.ToDateHistory.TabIndex = 1;
		this.label146.AutoSize = true;
		this.label146.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label146.Location = new System.Drawing.Point(201, 20);
		this.label146.Name = "label146";
		this.label146.Size = new System.Drawing.Size(48, 15);
		this.label146.TabIndex = 171;
		this.label146.Text = "Date To";
		this.label147.AutoSize = true;
		this.label147.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label147.Location = new System.Drawing.Point(2, 20);
		this.label147.Name = "label147";
		this.label147.Size = new System.Drawing.Size(63, 15);
		this.label147.TabIndex = 171;
		this.label147.Text = "Date From";
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(558, 16);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.Size = new System.Drawing.Size(62, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(66, 16);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(133, 23);
		this.FromDateHistory.TabIndex = 0;
		this.label148.AutoSize = true;
		this.label148.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label148.Location = new System.Drawing.Point(513, 20);
		this.label148.Name = "label148";
		this.label148.Size = new System.Drawing.Size(45, 15);
		this.label148.TabIndex = 173;
		this.label148.Text = "Doc To";
		this.label149.AutoSize = true;
		this.label149.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label149.Location = new System.Drawing.Point(388, 20);
		this.label149.Name = "label149";
		this.label149.Size = new System.Drawing.Size(60, 15);
		this.label149.TabIndex = 173;
		this.label149.Text = "Doc From";
		this.FromNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromNoHistory.Location = new System.Drawing.Point(449, 16);
		this.FromNoHistory.Name = "FromNoHistory";
		this.FromNoHistory.Size = new System.Drawing.Size(62, 23);
		this.FromNoHistory.TabIndex = 2;
		this.FromNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromNoHistory_KeyPress);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label15);
		this.panel8.Controls.Add(this.toolStrip2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1094, 52);
		this.panel8.TabIndex = 0;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(2, 27);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(190, 19);
		this.label15.TabIndex = 0;
		this.label15.Text = "Processing Bill History";
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNewHistory, this.btnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1094, 27);
		this.toolStrip2.TabIndex = 58;
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
		this.panel20.Controls.Add(this.panel16);
		this.panel20.Controls.Add(this.panel9);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel20.Location = new System.Drawing.Point(3, 258);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(1094, 147);
		this.panel20.TabIndex = 62;
		this.panel16.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistoryBpPurchased);
		this.panel16.Controls.Add(this.panel17);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel16.Location = new System.Drawing.Point(536, 0);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(558, 147);
		this.panel16.TabIndex = 66;
		this.grdhistoryBpPurchased.AllowEdit = (InheritableBoolean)2;
		grdhistoryBpPurchased_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistoryBpPurchased.DesignTimeLayout = grdhistoryBpPurchased_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistoryBpPurchased).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistoryBpPurchased.DynamicFiltering = true;
		this.grdhistoryBpPurchased.FilterRowButtonStyle = (FilterRowButtonStyle)1;
		((System.Windows.Forms.Control)(object)this.grdhistoryBpPurchased).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistoryBpPurchased.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdhistoryBpPurchased).Location = new System.Drawing.Point(0, 19);
		((System.Windows.Forms.Control)(object)this.grdhistoryBpPurchased).Name = "grdhistoryBpPurchased";
		((ScrollableBase)this.grdhistoryBpPurchased).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistoryBpPurchased).Size = new System.Drawing.Size(558, 128);
		((System.Windows.Forms.Control)(object)this.grdhistoryBpPurchased).TabIndex = 0;
		this.grdhistoryBpPurchased.TotalRow = (InheritableBoolean)1;
		this.grdhistoryBpPurchased.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistoryBpPurchased).VisualStyle = (VisualStyle)4;
		this.panel17.BackColor = System.Drawing.Color.Teal;
		this.panel17.Controls.Add(this.label16);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(0, 0);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(558, 19);
		this.panel17.TabIndex = 0;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.White;
		this.label16.Location = new System.Drawing.Point(2, 1);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(191, 16);
		this.label16.TabIndex = 1;
		this.label16.Text = "Purchased By Product Detail";
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistoryInput);
		this.panel9.Controls.Add(this.panel15);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(536, 147);
		this.panel9.TabIndex = 65;
		this.GrdHistoryInput.AllowEdit = (InheritableBoolean)2;
		this.GrdHistoryInput.ColumnAutoResize = true;
		GrdHistoryInput_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistoryInput.DesignTimeLayout = GrdHistoryInput_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistoryInput).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdHistoryInput.DynamicFiltering = true;
		this.GrdHistoryInput.FilterRowButtonStyle = (FilterRowButtonStyle)1;
		((System.Windows.Forms.Control)(object)this.GrdHistoryInput).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistoryInput.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdHistoryInput).Location = new System.Drawing.Point(0, 19);
		((System.Windows.Forms.Control)(object)this.GrdHistoryInput).Name = "GrdHistoryInput";
		((ScrollableBase)this.GrdHistoryInput).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistoryInput).Size = new System.Drawing.Size(536, 128);
		((System.Windows.Forms.Control)(object)this.GrdHistoryInput).TabIndex = 0;
		this.GrdHistoryInput.TotalRow = (InheritableBoolean)1;
		this.GrdHistoryInput.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistoryInput).VisualStyle = (VisualStyle)4;
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.label9);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 0);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(536, 19);
		this.panel15.TabIndex = 0;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(3, 2);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(83, 16);
		this.label9.TabIndex = 0;
		this.label9.Text = "InPut Detail";
		this.panel21.Controls.Add(this.panel31);
		this.panel21.Controls.Add(this.panel26);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel21.Location = new System.Drawing.Point(3, 405);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(1094, 124);
		this.panel21.TabIndex = 63;
		this.panel31.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistoryOH);
		this.panel31.Controls.Add(this.panel32);
		this.panel31.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel31.Location = new System.Drawing.Point(537, 0);
		this.panel31.Name = "panel31";
		this.panel31.Size = new System.Drawing.Size(557, 124);
		this.panel31.TabIndex = 65;
		this.grdhistoryOH.AllowEdit = (InheritableBoolean)2;
		this.grdhistoryOH.ColumnAutoResize = true;
		grdhistoryOH_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistoryOH.DesignTimeLayout = grdhistoryOH_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistoryOH).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistoryOH.DynamicFiltering = true;
		this.grdhistoryOH.FilterRowButtonStyle = (FilterRowButtonStyle)1;
		((System.Windows.Forms.Control)(object)this.grdhistoryOH).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistoryOH.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdhistoryOH).Location = new System.Drawing.Point(0, 19);
		((System.Windows.Forms.Control)(object)this.grdhistoryOH).Name = "grdhistoryOH";
		((ScrollableBase)this.grdhistoryOH).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistoryOH).Size = new System.Drawing.Size(557, 105);
		((System.Windows.Forms.Control)(object)this.grdhistoryOH).TabIndex = 0;
		this.grdhistoryOH.TotalRow = (InheritableBoolean)1;
		this.grdhistoryOH.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistoryOH).VisualStyle = (VisualStyle)4;
		this.panel32.BackColor = System.Drawing.Color.Teal;
		this.panel32.Controls.Add(this.label18);
		this.panel32.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel32.Location = new System.Drawing.Point(0, 0);
		this.panel32.Name = "panel32";
		this.panel32.Size = new System.Drawing.Size(557, 19);
		this.panel32.TabIndex = 0;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.Color.White;
		this.label18.Location = new System.Drawing.Point(2, 0);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(123, 16);
		this.label18.TabIndex = 1;
		this.label18.Text = "Over Heads Detail";
		this.panel26.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryPM);
		this.panel26.Controls.Add(this.panel23);
		this.panel26.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel26.Location = new System.Drawing.Point(0, 0);
		this.panel26.Name = "panel26";
		this.panel26.Size = new System.Drawing.Size(537, 124);
		this.panel26.TabIndex = 64;
		this.grdHistoryPM.AllowEdit = (InheritableBoolean)2;
		this.grdHistoryPM.ColumnAutoResize = true;
		grdHistoryPM_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryPM.DesignTimeLayout = grdHistoryPM_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryPM).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryPM.DynamicFiltering = true;
		this.grdHistoryPM.FilterRowButtonStyle = (FilterRowButtonStyle)1;
		((System.Windows.Forms.Control)(object)this.grdHistoryPM).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryPM.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryPM).Location = new System.Drawing.Point(0, 19);
		((System.Windows.Forms.Control)(object)this.grdHistoryPM).Name = "grdHistoryPM";
		((ScrollableBase)this.grdHistoryPM).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryPM).Size = new System.Drawing.Size(537, 105);
		((System.Windows.Forms.Control)(object)this.grdHistoryPM).TabIndex = 0;
		this.grdHistoryPM.TotalRow = (InheritableBoolean)1;
		this.grdHistoryPM.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryPM).VisualStyle = (VisualStyle)4;
		this.panel23.BackColor = System.Drawing.Color.Teal;
		this.panel23.Controls.Add(this.label17);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel23.Location = new System.Drawing.Point(0, 0);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(537, 19);
		this.panel23.TabIndex = 0;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(2, 1);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(154, 16);
		this.label17.TabIndex = 1;
		this.label17.Text = "Packing Material Detail";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1108, 561);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "ProductionPartyProcessingBill";
		this.Text = "Processing Bill      ";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ProductionPartyProcessingBill_FormClosing);
		base.Load += new System.EventHandler(frmProductionJobOrder_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmProductionJobOrder_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdByProducts).EndInit();
		this.panel19.ResumeLayout(false);
		this.panel19.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInput).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStockParty).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel18.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOH).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPM).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryMain).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel58.ResumeLayout(false);
		this.panel58.PerformLayout();
		this.groupBox11.ResumeLayout(false);
		this.groupBox11.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderHistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.panel20.ResumeLayout(false);
		this.panel16.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistoryBpPurchased).EndInit();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryInput).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel21.ResumeLayout(false);
		this.panel31.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistoryOH).EndInit();
		this.panel32.ResumeLayout(false);
		this.panel32.PerformLayout();
		this.panel26.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryPM).EndInit();
		this.panel23.ResumeLayout(false);
		this.panel23.PerformLayout();
		base.ResumeLayout(false);
	}
}
