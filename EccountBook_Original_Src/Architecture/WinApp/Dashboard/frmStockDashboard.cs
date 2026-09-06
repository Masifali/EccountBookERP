using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Reports.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Inventory_Stocks_Report;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

namespace Architecture.WinApp.Dashboard;

public class frmStockDashboard : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private bool BranchFeature;

	private IContainer components;

	private Panel panel1;

	private Panel panel3;

	private Panel panel2;

	private Panel pnlPendingInwardGatePass;

	private Label label1;

	private Button btnsearch;

	private DateTimePicker datFromDate;

	private DateTimePicker datToDate;

	private Label label17;

	private Label label18;

	private Label label23;

	private Label label65;

	private Label label22;

	private Label label66;

	private Label label67;

	private Label label20;

	private Label label68;

	private Label txtRawBalAmount;

	private Label txtRawOutAmount;

	private Label txtRawInAmount;

	private Label txtRawOpAmount;

	private Label txtPMBalAmt;

	private Label txtPMOutAmt;

	private Label txtPMInAmt;

	private Label txtPMBalQty;

	private Label txtPMOpAmt;

	private Label txtPMOutQty;

	private Label txtPMInQty;

	private Label txtPMOpQty;

	private Label txtBPBalAmt;

	private Label txtBPOutAmt;

	private Label txtBPInAmt;

	private Label txtBPBalQty;

	private Label txtBPOpAmt;

	private Label txtBPOutQty;

	private Label txtBPInQty;

	private Label txtBPOpQty;

	private Label txtFGBalAmt;

	private Label txtFGOutAmt;

	private Label txtFGInAmt;

	private Label txtFGBalQty;

	private Label txtFGOpAmt;

	private Label txtFGOutQty;

	private Label txtFGInQty;

	private Label txtFGOpQty;

	private Panel panel4;

	private Label txtRawBalAmtPdy;

	private Label txtRawOutAmtPdy;

	private Label txtRawInAmtPdy;

	private Label txtRawBalQtyPdy;

	private Label txtRawOpAmtPdy;

	private Label txtRawOutQtyPdy;

	private Label txtRawInQtyPdy;

	private Label txtRawOpQtyPdy;

	private RadioButton rdSaleValue;

	private RadioButton rdStockValue;

	private LinkLabel linkLabel3;

	private LinkLabel linkLabel1;

	private LinkLabel linkLabel2;

	private LinkLabel linkLabel4;

	private LinkLabel linkLabel5;

	private LinkLabel linkLabel6;

	private Label txtSTBalAmt;

	private Label txtSTOutAmt;

	private Label txtSTInAmt;

	private Label txtSTBalQty;

	private Label txtSTOpAmt;

	private Label txtSTOutQty;

	private Label txtSTInQty;

	private Label txtSTOpQty;

	private Panel panel6;

	private Label txtRawBalWeight;

	private Label txtRawOutWeight;

	private Label txtRawInWeight;

	private Label txtRawOpWeight;

	private Panel panel7;

	private Label label35;

	private Label label2;

	private Label label21;

	private Label txtRawBalQty;

	private Label txtRawOutQty;

	private Label txtRawInQty;

	private Label txtRawOpQty;

	private Label txtRawBalWeightPdy;

	private Label txtRawOutWeightPdy;

	private Label txtRawInWeightPdy;

	private Label txtRawOPWeightPdy;

	private Panel panel8;

	private Label label36;

	private Label label44;

	private Label label47;

	private Label label52;

	private Label label53;

	private Label label7;

	private Panel panel9;

	private Label label13;

	private Label label14;

	private Label label19;

	private Label label12;

	private Panel panel5;

	private Label label6;

	private Label label26;

	private Label txtStoreOpWeight;

	private Label txtStoreInWeight;

	private Label txtStoreOutWeight;

	private Label txtStoreBalWeight;

	private Panel panel19;

	private Label label32;

	private Label label33;

	private Label label34;

	private Label label50;

	private Label label64;

	private Panel panel20;

	private Label label71;

	private Label label73;

	private Label label74;

	private Panel panel16;

	private Label label4;

	private Label label40;

	private Label txtPackMaterialOpWeight;

	private Label txtPackMaterialInWeight;

	private Label txtPackMaterialOutWeight;

	private Label txtPackMaterialBalWeight;

	private Panel panel17;

	private Label label48;

	private Label label49;

	private Label label51;

	private Label label83;

	private Label label84;

	private Panel panel18;

	private Label label85;

	private Label label86;

	private Label label87;

	private Label label82;

	private Panel panel13;

	private Label label81;

	private Label txtFinishGoodsOpWeight;

	private Label txtFinishGoodsInWeight;

	private Label txtFinishGoodsOutWeight;

	private Label txtFinishGoodsBalWeight;

	private Panel panel14;

	private Label label59;

	private Label label60;

	private Label label61;

	private Label label62;

	private Label label63;

	private Panel panel15;

	private Label label69;

	private Label label72;

	private Label label75;

	private Panel panel10;

	private Label label80;

	private Label label78;

	private Label txtByProductOPWeight;

	private Label txtByProductInWeight;

	private Label txtByProductOutWeight;

	private Label txtByProductBalWeight;

	private Panel panel12;

	private Label label9;

	private Label label10;

	private Label label11;

	private Label label15;

	private Label label16;

	private Panel panel11;

	private Label label3;

	private Label label5;

	private Label label8;

	private Button btnRefresh;

	internal UltraCombo CmbBranches;

	private Label Branches;

	public frmStockDashboard(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmPendingWorksRpt_Load(object sender, EventArgs e)
	{
		try
		{
			datFromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			datFromDate.Focus();
			BranchFeature = CommonServices.GetERPFeatureById(11);
			BranchesFill();
			GridBind();
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
			DataTable dtBranch = BranchesAllocationToUser.GetBranchsAllocatedToUser(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID);
			if (dtBranch.Rows.Count > 0)
			{
				DDL.BindDDL(dtBranch, CmbBranches, "BranchId", "BranchName", "Branch Name", ZeroIndex: false);
				UltraGridColumn obj = ((UltraGridBase)CmbBranches).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj).Key = "Selected";
				((HeaderBase)obj.Header).Caption = string.Empty;
				obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj.DataType = typeof(bool);
				((HeaderBase)obj.Header).VisiblePosition = 0;
				CmbBranches.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)CmbBranches.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)CmbBranches.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)CmbBranches.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				((UltraGridBase)CmbBranches).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDate = datFromDate.Value;
			obj.ToDate = datToDate.Value;
			obj.stockUOM = 0;
			obj.PackingTypeId = 0;
			if (rdSaleValue.Checked)
			{
				obj.ActionId = 1;
			}
			else
			{
				obj.ActionId = 0;
			}
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)CmbBranches).Text) != string.Empty)
			{
				DataTable dtBranch = (DataTable)((UltraGridBase)CmbBranches).DataSource;
				string[] array = (Conversion.ToString((object)((Control)(object)CmbBranches).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				obj.BranchesIds = (BranchFeature ? BranchIds : "");
			}
			DataTable dtStock = StocksReport.stockReportWithValues(obj);
			if (dtStock.Rows.Count > 0)
			{
				double RawOpQty = 0.0;
				double RawInQty = 0.0;
				double RawOutQty = 0.0;
				double RawBalQty = 0.0;
				double RawOpWt = 0.0;
				double RawInWt = 0.0;
				double RawOutWt = 0.0;
				double RawBalWt = 0.0;
				double RawOpAmt = 0.0;
				double RawInAmt = 0.0;
				double RawOutAmt = 0.0;
				double RawBalAmt = 0.0;
				double RawOpQtypdy = 0.0;
				double RawInQtypdy = 0.0;
				double RawOutQtypdy = 0.0;
				double RawBalQtypdy = 0.0;
				double RawOpWtpdy = 0.0;
				double RawInWtpdy = 0.0;
				double RawOutWtpdy = 0.0;
				double RawBalWtpdy = 0.0;
				double RawOpAmtpdy = 0.0;
				double RawInAmtpdy = 0.0;
				double RawOutAmtpdy = 0.0;
				double RawBalAmtpdy = 0.0;
				double BPOpQty = 0.0;
				double BPInQty = 0.0;
				double BPOutQty = 0.0;
				double BPBalQty = 0.0;
				double BPOpWt = 0.0;
				double BPInWt = 0.0;
				double BPOutWt = 0.0;
				double BPBalWt = 0.0;
				double BPOpAmt = 0.0;
				double BPInAmt = 0.0;
				double BPOutAmt = 0.0;
				double BPBalAmt = 0.0;
				double FGOpQty = 0.0;
				double FGInQty = 0.0;
				double FGOutQty = 0.0;
				double FGBalQty = 0.0;
				double FGOpWt = 0.0;
				double FGInWt = 0.0;
				double FGOutWt = 0.0;
				double FGBalWt = 0.0;
				double FGOpAmt = 0.0;
				double FGInAmt = 0.0;
				double FGOutAmt = 0.0;
				double FGBalAmt = 0.0;
				double PMOpQty = 0.0;
				double PMInQty = 0.0;
				double PMOutQty = 0.0;
				double PMBalQty = 0.0;
				double PMOpWt = 0.0;
				double PMInWt = 0.0;
				double PMOutWt = 0.0;
				double PMBalWt = 0.0;
				double PMOpAmt = 0.0;
				double PMInAmt = 0.0;
				double PMOutAmt = 0.0;
				double PMBalAmt = 0.0;
				double STOpQty = 0.0;
				double STInQty = 0.0;
				double STOutQty = 0.0;
				double STBalQty = 0.0;
				double STOpWt = 0.0;
				double STInWt = 0.0;
				double STOutWt = 0.0;
				double STBalWt = 0.0;
				double STOpAmt = 0.0;
				double STInAmt = 0.0;
				double STOutAmt = 0.0;
				double STBalAmt = 0.0;
				for (int i = 0; i < dtStock.Rows.Count; i++)
				{
					if (Conversion.ToInt(dtStock.Rows[i]["ItemClassGroupId"]) == 1 && Conversion.ToInt(dtStock.Rows[i]["InventoryParentCategoriesId"]) == 2)
					{
						RawOpQty += Conversion.ToDouble(dtStock.Rows[i]["Op_Qty"]);
						RawInQty += Conversion.ToDouble(dtStock.Rows[i]["QtyIn"]);
						RawOutQty += Conversion.ToDouble(dtStock.Rows[i]["QtyOut"]);
						RawBalQty += Conversion.ToDouble(dtStock.Rows[i]["BalQty"]);
						RawOpWt += Conversion.ToDouble(dtStock.Rows[i]["Op_Weight"]);
						RawInWt += Conversion.ToDouble(dtStock.Rows[i]["WeightIn"]);
						RawOutWt += Conversion.ToDouble(dtStock.Rows[i]["WeightOut"]);
						RawBalWt += Conversion.ToDouble(dtStock.Rows[i]["BalWeight"]);
						RawOpAmt += Conversion.ToDouble(dtStock.Rows[i]["Op_Amount"]);
						RawInAmt += Conversion.ToDouble(dtStock.Rows[i]["AmountIn"]);
						RawOutAmt += Conversion.ToDouble(dtStock.Rows[i]["AmountOut"]);
						RawBalAmt += Conversion.ToDouble(dtStock.Rows[i]["BalAmount"]);
					}
					else if (Conversion.ToInt(dtStock.Rows[i]["ItemClassGroupId"]) == 1 && Conversion.ToInt(dtStock.Rows[i]["InventoryParentCategoriesId"]) == 1)
					{
						RawOpQtypdy += Conversion.ToDouble(dtStock.Rows[i]["Op_Qty"]);
						RawInQtypdy += Conversion.ToDouble(dtStock.Rows[i]["QtyIn"]);
						RawOutQtypdy += Conversion.ToDouble(dtStock.Rows[i]["QtyOut"]);
						RawBalQtypdy += Conversion.ToDouble(dtStock.Rows[i]["BalQty"]);
						RawOpWtpdy += Conversion.ToDouble(dtStock.Rows[i]["Op_Weight"]);
						RawInWtpdy += Conversion.ToDouble(dtStock.Rows[i]["WeightIn"]);
						RawOutWtpdy += Conversion.ToDouble(dtStock.Rows[i]["WeightOut"]);
						RawBalWtpdy += Conversion.ToDouble(dtStock.Rows[i]["BalWeight"]);
						RawOpAmtpdy += Conversion.ToDouble(dtStock.Rows[i]["Op_Amount"]);
						RawInAmtpdy += Conversion.ToDouble(dtStock.Rows[i]["AmountIn"]);
						RawOutAmtpdy += Conversion.ToDouble(dtStock.Rows[i]["AmountOut"]);
						RawBalAmtpdy += Conversion.ToDouble(dtStock.Rows[i]["BalAmount"]);
					}
					else if (Conversion.ToInt(dtStock.Rows[i]["ItemClassGroupId"]) == 2 && Conversion.ToInt(dtStock.Rows[i]["InventoryParentCategoriesId"]) == 4)
					{
						BPOpQty += Conversion.ToDouble(dtStock.Rows[i]["Op_Qty"]);
						BPInQty += Conversion.ToDouble(dtStock.Rows[i]["QtyIn"]);
						BPOutQty += Conversion.ToDouble(dtStock.Rows[i]["QtyOut"]);
						BPBalQty += Conversion.ToDouble(dtStock.Rows[i]["BalQty"]);
						BPOpWt += Conversion.ToDouble(dtStock.Rows[i]["Op_Weight"]);
						BPInWt += Conversion.ToDouble(dtStock.Rows[i]["WeightIn"]);
						BPOutWt += Conversion.ToDouble(dtStock.Rows[i]["WeightOut"]);
						BPBalWt += Conversion.ToDouble(dtStock.Rows[i]["BalWeight"]);
						BPOpAmt += Conversion.ToDouble(dtStock.Rows[i]["Op_Amount"]);
						BPInAmt += Conversion.ToDouble(dtStock.Rows[i]["AmountIn"]);
						BPOutAmt += Conversion.ToDouble(dtStock.Rows[i]["AmountOut"]);
						BPBalAmt += Conversion.ToDouble(dtStock.Rows[i]["BalAmount"]);
					}
					else if (Conversion.ToInt(dtStock.Rows[i]["ItemClassGroupId"]) == 3 && Conversion.ToInt(dtStock.Rows[i]["InventoryParentCategoriesId"]) == 2)
					{
						FGOpQty += Conversion.ToDouble(dtStock.Rows[i]["Op_Qty"]);
						FGInQty += Conversion.ToDouble(dtStock.Rows[i]["QtyIn"]);
						FGOutQty += Conversion.ToDouble(dtStock.Rows[i]["QtyOut"]);
						FGBalQty += Conversion.ToDouble(dtStock.Rows[i]["BalQty"]);
						FGOpWt += Conversion.ToDouble(dtStock.Rows[i]["Op_Weight"]);
						FGInWt += Conversion.ToDouble(dtStock.Rows[i]["WeightIn"]);
						FGOutWt += Conversion.ToDouble(dtStock.Rows[i]["WeightOut"]);
						FGBalWt += Conversion.ToDouble(dtStock.Rows[i]["BalWeight"]);
						FGOpAmt += Conversion.ToDouble(dtStock.Rows[i]["Op_Amount"]);
						FGInAmt += Conversion.ToDouble(dtStock.Rows[i]["AmountIn"]);
						FGOutAmt += Conversion.ToDouble(dtStock.Rows[i]["AmountOut"]);
						FGBalAmt += Conversion.ToDouble(dtStock.Rows[i]["BalAmount"]);
					}
					else if (Conversion.ToInt(dtStock.Rows[i]["ItemClassGroupId"]) == 4 && Conversion.ToInt(dtStock.Rows[i]["InventoryParentCategoriesId"]) == 7)
					{
						PMOpQty += Conversion.ToDouble(dtStock.Rows[i]["Op_Qty"]);
						PMInQty += Conversion.ToDouble(dtStock.Rows[i]["QtyIn"]);
						PMOutQty += Conversion.ToDouble(dtStock.Rows[i]["QtyOut"]);
						PMBalQty += Conversion.ToDouble(dtStock.Rows[i]["BalQty"]);
						PMOpWt += Conversion.ToDouble(dtStock.Rows[i]["Op_Weight"]);
						PMInWt += Conversion.ToDouble(dtStock.Rows[i]["WeightIn"]);
						PMOutWt += Conversion.ToDouble(dtStock.Rows[i]["WeightOut"]);
						PMBalWt += Conversion.ToDouble(dtStock.Rows[i]["BalWeight"]);
						PMOpAmt += Conversion.ToDouble(dtStock.Rows[i]["Op_Amount"]);
						PMInAmt += Conversion.ToDouble(dtStock.Rows[i]["AmountIn"]);
						PMOutAmt += Conversion.ToDouble(dtStock.Rows[i]["AmountOut"]);
						PMBalAmt += Conversion.ToDouble(dtStock.Rows[i]["BalAmount"]);
					}
					else if (Conversion.ToInt(dtStock.Rows[i]["ItemClassGroupId"]) == 5 && Conversion.ToInt(dtStock.Rows[i]["InventoryParentCategoriesId"]) == 8)
					{
						STOpQty += Conversion.ToDouble(dtStock.Rows[i]["Op_Qty"]);
						STInQty += Conversion.ToDouble(dtStock.Rows[i]["QtyIn"]);
						STOutQty += Conversion.ToDouble(dtStock.Rows[i]["QtyOut"]);
						STBalQty += Conversion.ToDouble(dtStock.Rows[i]["BalQty"]);
						STOpWt += Conversion.ToDouble(dtStock.Rows[i]["Op_Weight"]);
						STInWt += Conversion.ToDouble(dtStock.Rows[i]["WeightIn"]);
						STOutWt += Conversion.ToDouble(dtStock.Rows[i]["WeightOut"]);
						STBalWt += Conversion.ToDouble(dtStock.Rows[i]["BalWeight"]);
						STOpAmt += Conversion.ToDouble(dtStock.Rows[i]["Op_Amount"]);
						STInAmt += Conversion.ToDouble(dtStock.Rows[i]["AmountIn"]);
						STOutAmt += Conversion.ToDouble(dtStock.Rows[i]["AmountOut"]);
						STBalAmt += Conversion.ToDouble(dtStock.Rows[i]["BalAmount"]);
					}
				}
				txtRawOpQty.Text = RawOpQty.ToString("#,#;(#,#);0");
				txtRawInQty.Text = RawInQty.ToString("#,#;(#,#);0");
				txtRawOutQty.Text = RawOutQty.ToString("#,#;(#,#);0");
				txtRawBalQty.Text = RawBalQty.ToString("#,#;(#,#);0");
				txtRawOpWeight.Text = RawOpWt.ToString("#,#;(#,#);0");
				txtRawInWeight.Text = RawInWt.ToString("#,#;(#,#);0");
				txtRawOutWeight.Text = RawOutWt.ToString("#,#;(#,#);0");
				txtRawBalWeight.Text = RawBalWt.ToString("#,#;(#,#);0");
				txtRawOpAmount.Text = RawOpAmt.ToString("#,#;(#,#);0");
				txtRawInAmount.Text = RawInAmt.ToString("#,#;(#,#);0");
				txtRawOutAmount.Text = RawOutAmt.ToString("#,#;(#,#);0");
				txtRawBalAmount.Text = RawBalAmt.ToString("#,#;(#,#);0");
				txtRawOpQtyPdy.Text = RawOpQtypdy.ToString("#,#;(#,#);0");
				txtRawInQtyPdy.Text = RawInQtypdy.ToString("#,#;(#,#);0");
				txtRawOutQtyPdy.Text = RawOutQtypdy.ToString("#,#;(#,#);0");
				txtRawBalQtyPdy.Text = RawBalQtypdy.ToString("#,#;(#,#);0");
				txtRawOPWeightPdy.Text = RawOpWtpdy.ToString("#,#;(#,#);0");
				txtRawInWeightPdy.Text = RawInWtpdy.ToString("#,#;(#,#);0");
				txtRawOutWeightPdy.Text = RawOutWtpdy.ToString("#,#;(#,#);0");
				txtRawBalWeightPdy.Text = RawBalWtpdy.ToString("#,#;(#,#);0");
				txtRawOpAmtPdy.Text = RawOpAmtpdy.ToString("#,#;(#,#);0");
				txtRawInAmtPdy.Text = RawInAmtpdy.ToString("#,#;(#,#);0");
				txtRawOutAmtPdy.Text = RawOutAmtpdy.ToString("#,#;(#,#);0");
				txtRawBalAmtPdy.Text = RawBalAmtpdy.ToString("#,#;(#,#);0");
				txtBPOpQty.Text = BPOpQty.ToString("#,#;(#,#);0");
				txtBPInQty.Text = BPInQty.ToString("#,#;(#,#);0");
				txtBPOutQty.Text = BPOutQty.ToString("#,#;(#,#);0");
				txtBPBalQty.Text = BPBalQty.ToString("#,#;(#,#);0");
				txtByProductOPWeight.Text = BPOpWt.ToString("#,#;(#,#);0");
				txtByProductInWeight.Text = BPInWt.ToString("#,#;(#,#);0");
				txtByProductOutWeight.Text = BPOutWt.ToString("#,#;(#,#);0");
				txtByProductBalWeight.Text = BPBalWt.ToString("#,#;(#,#);0");
				txtBPOpAmt.Text = BPOpAmt.ToString("#,#;(#,#);0");
				txtBPInAmt.Text = BPInAmt.ToString("#,#;(#,#);0");
				txtBPOutAmt.Text = BPOutAmt.ToString("#,#;(#,#);0");
				txtBPBalAmt.Text = BPBalAmt.ToString("#,#;(#,#);0");
				txtFGOpQty.Text = FGOpQty.ToString("#,#;(#,#);0");
				txtFGInQty.Text = FGInQty.ToString("#,#;(#,#);0");
				txtFGOutQty.Text = FGOutQty.ToString("#,#;(#,#);0");
				txtFGBalQty.Text = FGBalQty.ToString("#,#;(#,#);0");
				txtFinishGoodsOpWeight.Text = FGOpWt.ToString("#,#;(#,#);0");
				txtFinishGoodsInWeight.Text = FGInWt.ToString("#,#;(#,#);0");
				txtFinishGoodsOutWeight.Text = FGOutWt.ToString("#,#;(#,#);0");
				txtFinishGoodsBalWeight.Text = FGBalWt.ToString("#,#;(#,#);0");
				txtFGOpAmt.Text = FGOpAmt.ToString("#,#;(#,#);0");
				txtFGInAmt.Text = FGInAmt.ToString("#,#;(#,#);0");
				txtFGOutAmt.Text = FGOutAmt.ToString("#,#;(#,#);0");
				txtFGBalAmt.Text = FGBalAmt.ToString("#,#;(#,#);0");
				txtPMOpQty.Text = PMOpQty.ToString("#,#;(#,#);0");
				txtPMInQty.Text = PMInQty.ToString("#,#;(#,#);0");
				txtPMOutQty.Text = PMOutQty.ToString("#,#;(#,#);0");
				txtPMBalQty.Text = PMBalQty.ToString("#,#;(#,#);0");
				txtPackMaterialOpWeight.Text = PMOpWt.ToString("#,#;(#,#);0");
				txtPackMaterialInWeight.Text = PMInWt.ToString("#,#;(#,#);0");
				txtPackMaterialOutWeight.Text = PMOutWt.ToString("#,#;(#,#);0");
				txtPackMaterialBalWeight.Text = PMBalWt.ToString("#,#;(#,#);0");
				txtPMOpAmt.Text = PMOpAmt.ToString("#,#;(#,#);0");
				txtPMInAmt.Text = PMInAmt.ToString("#,#;(#,#);0");
				txtPMOutAmt.Text = PMOutAmt.ToString("#,#;(#,#);0");
				txtPMBalAmt.Text = PMBalAmt.ToString("#,#;(#,#);0");
				txtSTOpQty.Text = STOpQty.ToString("#,#;(#,#);0");
				txtSTInQty.Text = STInQty.ToString("#,#;(#,#);0");
				txtSTOutQty.Text = STOutQty.ToString("#,#;(#,#);0");
				txtSTBalQty.Text = STBalQty.ToString("#,#;(#,#);0");
				txtStoreOpWeight.Text = STOpWt.ToString("#,#;(#,#);0");
				txtStoreInWeight.Text = STInWt.ToString("#,#;(#,#);0");
				txtStoreOutWeight.Text = STOutWt.ToString("#,#;(#,#);0");
				txtStoreBalWeight.Text = STBalWt.ToString("#,#;(#,#);0");
				txtSTOpAmt.Text = STOpAmt.ToString("#,#;(#,#);0");
				txtSTInAmt.Text = STInAmt.ToString("#,#;(#,#);0");
				txtSTOutAmt.Text = STOutAmt.ToString("#,#;(#,#);0");
				txtSTBalAmt.Text = STBalAmt.ToString("#,#;(#,#);0");
			}
			else
			{
				Reset();
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
			rdStockValue.Checked = true;
			txtRawOpQty.Text = "0";
			txtRawInQty.Text = "0";
			txtRawOutQty.Text = "0";
			txtRawBalQty.Text = "0";
			txtRawOpWeight.Text = "0";
			txtRawInWeight.Text = "0";
			txtRawOutWeight.Text = "0";
			txtRawBalWeight.Text = "0";
			txtRawOpAmount.Text = "0";
			txtRawInAmount.Text = "0";
			txtRawOutAmount.Text = "0";
			txtRawBalAmount.Text = "0";
			txtRawOpQtyPdy.Text = "0";
			txtRawInQtyPdy.Text = "0";
			txtRawOutQtyPdy.Text = "0";
			txtRawBalQtyPdy.Text = "0";
			txtRawOPWeightPdy.Text = "0";
			txtRawInWeightPdy.Text = "0";
			txtRawOutWeightPdy.Text = "0";
			txtRawBalWeightPdy.Text = "0";
			txtRawOpAmtPdy.Text = "0";
			txtRawInAmtPdy.Text = "0";
			txtRawOutAmtPdy.Text = "0";
			txtRawBalAmtPdy.Text = "0";
			txtBPOpQty.Text = "0";
			txtBPInQty.Text = "0";
			txtBPOutQty.Text = "0";
			txtBPBalQty.Text = "0";
			txtByProductOPWeight.Text = "0";
			txtByProductInWeight.Text = "0";
			txtByProductOutWeight.Text = "0";
			txtByProductBalWeight.Text = "0";
			txtBPOpAmt.Text = "0";
			txtBPInAmt.Text = "0";
			txtBPOutAmt.Text = "0";
			txtBPBalAmt.Text = "0";
			txtFGOpQty.Text = "0";
			txtFGInQty.Text = "0";
			txtFGOutQty.Text = "0";
			txtFGBalQty.Text = "0";
			txtFinishGoodsOpWeight.Text = "0";
			txtFinishGoodsInWeight.Text = "0";
			txtFinishGoodsOutWeight.Text = "0";
			txtFinishGoodsBalWeight.Text = "0";
			txtFGOpAmt.Text = "0";
			txtFGInAmt.Text = "0";
			txtFGOutAmt.Text = "0";
			txtFGBalAmt.Text = "0";
			txtPMOpQty.Text = "0";
			txtPMInQty.Text = "0";
			txtPMOutQty.Text = "0";
			txtPMBalQty.Text = "0";
			txtPackMaterialOpWeight.Text = "0";
			txtPackMaterialInWeight.Text = "0";
			txtPackMaterialOutWeight.Text = "0";
			txtPackMaterialBalWeight.Text = "0";
			txtPMOpAmt.Text = "0";
			txtPMInAmt.Text = "0";
			txtPMOutAmt.Text = "0";
			txtPMBalAmt.Text = "0";
			txtSTOpQty.Text = "0";
			txtSTInQty.Text = "0";
			txtSTOutQty.Text = "0";
			txtSTBalQty.Text = "0";
			txtStoreOpWeight.Text = "0";
			txtStoreInWeight.Text = "0";
			txtStoreOutWeight.Text = "0";
			txtStoreBalWeight.Text = "0";
			txtSTOpAmt.Text = "0";
			txtSTInAmt.Text = "0";
			txtSTOutAmt.Text = "0";
			txtSTBalAmt.Text = "0";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsearch_Click(object sender, EventArgs e)
	{
		try
		{
			GridBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		try
		{
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)CmbBranches).Text) != string.Empty)
			{
				DataTable dtBranch = (DataTable)((UltraGridBase)CmbBranches).DataSource;
				string[] array = (Conversion.ToString((object)((Control)(object)CmbBranches).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
			}
			frmStockReportWithValues obj = new frmStockReportWithValues(UserAccount)
			{
				FromDate = datFromDate.Value,
				ToDate = datToDate.Value
			};
			obj.cmbMainType.Value = 2;
			obj.ParentCategoryIds = "2";
			obj.CmbClassGroup.Value = 1;
			obj.BranchesIds = (BranchFeature ? BranchIds : "");
			obj.RequestFromAnotherForm = true;
			obj.ShowDialog();
			obj.BringToFront();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		try
		{
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)CmbBranches).Text) != string.Empty)
			{
				DataTable dtBranch = (DataTable)((UltraGridBase)CmbBranches).DataSource;
				string[] array = (Conversion.ToString((object)((Control)(object)CmbBranches).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
			}
			frmStockReportWithValues obj = new frmStockReportWithValues(UserAccount)
			{
				FromDate = datFromDate.Value,
				ToDate = datToDate.Value
			};
			obj.cmbMainType.Value = 1;
			obj.ParentCategoryIds = "1";
			obj.CmbClassGroup.Value = 1;
			obj.BranchesIds = (BranchFeature ? BranchIds : "");
			obj.RequestFromAnotherForm = true;
			obj.ShowDialog();
			obj.BringToFront();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		try
		{
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)CmbBranches).Text) != string.Empty)
			{
				DataTable dtBranch = (DataTable)((UltraGridBase)CmbBranches).DataSource;
				string[] array = (Conversion.ToString((object)((Control)(object)CmbBranches).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
			}
			frmStockReportWithValues obj = new frmStockReportWithValues(UserAccount)
			{
				FromDate = datFromDate.Value,
				ToDate = datToDate.Value
			};
			obj.cmbMainType.Value = 4;
			obj.ParentCategoryIds = "4";
			obj.CmbClassGroup.Value = 2;
			obj.BranchesIds = (BranchFeature ? BranchIds : "");
			obj.RequestFromAnotherForm = true;
			obj.ShowDialog();
			obj.BringToFront();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		try
		{
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)CmbBranches).Text) != string.Empty)
			{
				DataTable dtBranch = (DataTable)((UltraGridBase)CmbBranches).DataSource;
				string[] array = (Conversion.ToString((object)((Control)(object)CmbBranches).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
			}
			frmStockReportWithValues obj = new frmStockReportWithValues(UserAccount)
			{
				FromDate = datFromDate.Value,
				ToDate = datToDate.Value
			};
			obj.CmbClassGroup.Value = 3;
			obj.cmbMainType.Value = 2;
			obj.ParentCategoryIds = "2";
			obj.BranchesIds = (BranchFeature ? BranchIds : "");
			obj.RequestFromAnotherForm = true;
			obj.ShowDialog();
			obj.BringToFront();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		try
		{
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)CmbBranches).Text) != string.Empty)
			{
				DataTable dtBranch = (DataTable)((UltraGridBase)CmbBranches).DataSource;
				string[] array = (Conversion.ToString((object)((Control)(object)CmbBranches).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
			}
			frmStockReportWithValues obj = new frmStockReportWithValues(UserAccount)
			{
				FromDate = datFromDate.Value,
				ToDate = datToDate.Value
			};
			obj.cmbMainType.Value = 7;
			obj.ParentCategoryIds = "7";
			obj.CmbClassGroup.Value = 4;
			obj.BranchesIds = (BranchFeature ? BranchIds : "");
			obj.RequestFromAnotherForm = true;
			obj.ShowDialog();
			obj.BringToFront();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		try
		{
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)CmbBranches).Text) != string.Empty)
			{
				DataTable dtBranch = (DataTable)((UltraGridBase)CmbBranches).DataSource;
				string[] array = (Conversion.ToString((object)((Control)(object)CmbBranches).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
			}
			frmStockReportWithValues obj = new frmStockReportWithValues(UserAccount)
			{
				FromDate = datFromDate.Value,
				ToDate = datToDate.Value
			};
			obj.cmbMainType.Value = 8;
			obj.ParentCategoryIds = "8";
			obj.CmbClassGroup.Value = 5;
			obj.BranchesIds = (BranchFeature ? BranchIds : "");
			obj.RequestFromAnotherForm = true;
			obj.ShowDialog();
			obj.BringToFront();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmStockDashboard_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_079a: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a4: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Dashboard.frmStockDashboard));
		Appearance appearance37 = new Appearance();
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
		Appearance appearance48 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.btnRefresh = new System.Windows.Forms.Button();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label6 = new System.Windows.Forms.Label();
		this.linkLabel6 = new System.Windows.Forms.LinkLabel();
		this.label26 = new System.Windows.Forms.Label();
		this.txtStoreOpWeight = new System.Windows.Forms.Label();
		this.txtStoreInWeight = new System.Windows.Forms.Label();
		this.txtStoreOutWeight = new System.Windows.Forms.Label();
		this.txtStoreBalWeight = new System.Windows.Forms.Label();
		this.panel19 = new System.Windows.Forms.Panel();
		this.label32 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.label64 = new System.Windows.Forms.Label();
		this.panel20 = new System.Windows.Forms.Panel();
		this.label71 = new System.Windows.Forms.Label();
		this.label73 = new System.Windows.Forms.Label();
		this.label74 = new System.Windows.Forms.Label();
		this.txtSTOpQty = new System.Windows.Forms.Label();
		this.txtSTBalAmt = new System.Windows.Forms.Label();
		this.txtSTInQty = new System.Windows.Forms.Label();
		this.txtSTOutQty = new System.Windows.Forms.Label();
		this.txtSTOutAmt = new System.Windows.Forms.Label();
		this.txtSTBalQty = new System.Windows.Forms.Label();
		this.txtSTOpAmt = new System.Windows.Forms.Label();
		this.txtSTInAmt = new System.Windows.Forms.Label();
		this.panel16 = new System.Windows.Forms.Panel();
		this.linkLabel5 = new System.Windows.Forms.LinkLabel();
		this.label4 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.txtPackMaterialOpWeight = new System.Windows.Forms.Label();
		this.txtPackMaterialInWeight = new System.Windows.Forms.Label();
		this.txtPackMaterialOutWeight = new System.Windows.Forms.Label();
		this.txtPackMaterialBalWeight = new System.Windows.Forms.Label();
		this.panel17 = new System.Windows.Forms.Panel();
		this.label48 = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.label83 = new System.Windows.Forms.Label();
		this.label84 = new System.Windows.Forms.Label();
		this.txtPMBalAmt = new System.Windows.Forms.Label();
		this.panel18 = new System.Windows.Forms.Panel();
		this.label85 = new System.Windows.Forms.Label();
		this.label86 = new System.Windows.Forms.Label();
		this.label87 = new System.Windows.Forms.Label();
		this.txtPMOpQty = new System.Windows.Forms.Label();
		this.txtPMOutAmt = new System.Windows.Forms.Label();
		this.txtPMInQty = new System.Windows.Forms.Label();
		this.txtPMOutQty = new System.Windows.Forms.Label();
		this.txtPMInAmt = new System.Windows.Forms.Label();
		this.txtPMBalQty = new System.Windows.Forms.Label();
		this.txtPMOpAmt = new System.Windows.Forms.Label();
		this.label82 = new System.Windows.Forms.Label();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label81 = new System.Windows.Forms.Label();
		this.linkLabel4 = new System.Windows.Forms.LinkLabel();
		this.txtFinishGoodsOpWeight = new System.Windows.Forms.Label();
		this.txtFinishGoodsInWeight = new System.Windows.Forms.Label();
		this.txtFinishGoodsOutWeight = new System.Windows.Forms.Label();
		this.txtFinishGoodsBalWeight = new System.Windows.Forms.Label();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label59 = new System.Windows.Forms.Label();
		this.label60 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.label63 = new System.Windows.Forms.Label();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label69 = new System.Windows.Forms.Label();
		this.label72 = new System.Windows.Forms.Label();
		this.label75 = new System.Windows.Forms.Label();
		this.txtFGOpQty = new System.Windows.Forms.Label();
		this.txtFGInQty = new System.Windows.Forms.Label();
		this.txtFGBalAmt = new System.Windows.Forms.Label();
		this.txtFGOutQty = new System.Windows.Forms.Label();
		this.txtFGBalQty = new System.Windows.Forms.Label();
		this.txtFGOutAmt = new System.Windows.Forms.Label();
		this.txtFGOpAmt = new System.Windows.Forms.Label();
		this.txtFGInAmt = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label80 = new System.Windows.Forms.Label();
		this.label78 = new System.Windows.Forms.Label();
		this.txtByProductOPWeight = new System.Windows.Forms.Label();
		this.txtByProductInWeight = new System.Windows.Forms.Label();
		this.txtByProductOutWeight = new System.Windows.Forms.Label();
		this.txtByProductBalWeight = new System.Windows.Forms.Label();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.linkLabel2 = new System.Windows.Forms.LinkLabel();
		this.txtBPOpQty = new System.Windows.Forms.Label();
		this.txtBPInQty = new System.Windows.Forms.Label();
		this.txtBPOutQty = new System.Windows.Forms.Label();
		this.txtBPBalQty = new System.Windows.Forms.Label();
		this.txtBPOpAmt = new System.Windows.Forms.Label();
		this.txtBPInAmt = new System.Windows.Forms.Label();
		this.txtBPOutAmt = new System.Windows.Forms.Label();
		this.txtBPBalAmt = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label36 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label21 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label20 = new System.Windows.Forms.Label();
		this.label65 = new System.Windows.Forms.Label();
		this.label66 = new System.Windows.Forms.Label();
		this.label68 = new System.Windows.Forms.Label();
		this.label67 = new System.Windows.Forms.Label();
		this.rdSaleValue = new System.Windows.Forms.RadioButton();
		this.rdStockValue = new System.Windows.Forms.RadioButton();
		this.btnsearch = new System.Windows.Forms.Button();
		this.datFromDate = new System.Windows.Forms.DateTimePicker();
		this.datToDate = new System.Windows.Forms.DateTimePicker();
		this.label17 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label7 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label13 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.linkLabel1 = new System.Windows.Forms.LinkLabel();
		this.txtRawBalWeightPdy = new System.Windows.Forms.Label();
		this.txtRawOutWeightPdy = new System.Windows.Forms.Label();
		this.txtRawInWeightPdy = new System.Windows.Forms.Label();
		this.txtRawOPWeightPdy = new System.Windows.Forms.Label();
		this.txtRawBalAmtPdy = new System.Windows.Forms.Label();
		this.txtRawOutAmtPdy = new System.Windows.Forms.Label();
		this.txtRawInAmtPdy = new System.Windows.Forms.Label();
		this.txtRawBalQtyPdy = new System.Windows.Forms.Label();
		this.txtRawOpAmtPdy = new System.Windows.Forms.Label();
		this.txtRawOutQtyPdy = new System.Windows.Forms.Label();
		this.txtRawInQtyPdy = new System.Windows.Forms.Label();
		this.txtRawOpQtyPdy = new System.Windows.Forms.Label();
		this.pnlPendingInwardGatePass = new System.Windows.Forms.Panel();
		this.txtRawBalWeight = new System.Windows.Forms.Label();
		this.txtRawOutWeight = new System.Windows.Forms.Label();
		this.txtRawInWeight = new System.Windows.Forms.Label();
		this.txtRawOpWeight = new System.Windows.Forms.Label();
		this.linkLabel3 = new System.Windows.Forms.LinkLabel();
		this.txtRawBalAmount = new System.Windows.Forms.Label();
		this.txtRawOutAmount = new System.Windows.Forms.Label();
		this.txtRawInAmount = new System.Windows.Forms.Label();
		this.txtRawBalQty = new System.Windows.Forms.Label();
		this.txtRawOpAmount = new System.Windows.Forms.Label();
		this.txtRawOutQty = new System.Windows.Forms.Label();
		this.txtRawInQty = new System.Windows.Forms.Label();
		this.txtRawOpQty = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbBranches = new UltraCombo();
		this.Branches = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel19.SuspendLayout();
		this.panel20.SuspendLayout();
		this.panel16.SuspendLayout();
		this.panel17.SuspendLayout();
		this.panel18.SuspendLayout();
		this.panel13.SuspendLayout();
		this.panel14.SuspendLayout();
		this.panel15.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel12.SuspendLayout();
		this.panel11.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel9.SuspendLayout();
		this.pnlPendingInwardGatePass.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranches).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1094, 683);
		this.panel1.TabIndex = 0;
		this.panel3.AutoScroll = true;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranches);
		this.panel3.Controls.Add(this.Branches);
		this.panel3.Controls.Add(this.btnRefresh);
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Controls.Add(this.panel16);
		this.panel3.Controls.Add(this.label82);
		this.panel3.Controls.Add(this.panel13);
		this.panel3.Controls.Add(this.panel10);
		this.panel3.Controls.Add(this.panel8);
		this.panel3.Controls.Add(this.panel7);
		this.panel3.Controls.Add(this.label35);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Controls.Add(this.panel6);
		this.panel3.Controls.Add(this.rdSaleValue);
		this.panel3.Controls.Add(this.rdStockValue);
		this.panel3.Controls.Add(this.btnsearch);
		this.panel3.Controls.Add(this.datFromDate);
		this.panel3.Controls.Add(this.datToDate);
		this.panel3.Controls.Add(this.label17);
		this.panel3.Controls.Add(this.label18);
		this.panel3.Controls.Add(this.panel4);
		this.panel3.Controls.Add(this.pnlPendingInwardGatePass);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 58);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1094, 625);
		this.panel3.TabIndex = 1;
		this.btnRefresh.BackColor = System.Drawing.Color.White;
		this.btnRefresh.FlatAppearance.BorderSize = 0;
		this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnRefresh.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = (System.Drawing.Image)resources.GetObject("btnRefresh.Image");
		this.btnRefresh.Location = new System.Drawing.Point(1037, 14);
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(44, 40);
		this.btnRefresh.TabIndex = 6;
		this.btnRefresh.UseVisualStyleBackColor = false;
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.panel5.BackColor = System.Drawing.SystemColors.ButtonHighlight;
		this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel5.Controls.Add(this.label6);
		this.panel5.Controls.Add(this.linkLabel6);
		this.panel5.Controls.Add(this.label26);
		this.panel5.Controls.Add(this.txtStoreOpWeight);
		this.panel5.Controls.Add(this.txtStoreInWeight);
		this.panel5.Controls.Add(this.txtStoreOutWeight);
		this.panel5.Controls.Add(this.txtStoreBalWeight);
		this.panel5.Controls.Add(this.panel19);
		this.panel5.Controls.Add(this.panel20);
		this.panel5.Controls.Add(this.txtSTOpQty);
		this.panel5.Controls.Add(this.txtSTBalAmt);
		this.panel5.Controls.Add(this.txtSTInQty);
		this.panel5.Controls.Add(this.txtSTOutQty);
		this.panel5.Controls.Add(this.txtSTOutAmt);
		this.panel5.Controls.Add(this.txtSTBalQty);
		this.panel5.Controls.Add(this.txtSTOpAmt);
		this.panel5.Controls.Add(this.txtSTInAmt);
		this.panel5.Cursor = System.Windows.Forms.Cursors.Hand;
		this.panel5.Location = new System.Drawing.Point(562, 414);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(521, 153);
		this.panel5.TabIndex = 62;
		this.label6.BackColor = System.Drawing.Color.LightSeaGreen;
		this.label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.label6.Location = new System.Drawing.Point(109, 121);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(411, 3);
		this.label6.TabIndex = 52;
		this.linkLabel6.BackColor = System.Drawing.Color.Teal;
		this.linkLabel6.Cursor = System.Windows.Forms.Cursors.Hand;
		this.linkLabel6.Font = new System.Drawing.Font("Calibri", 18f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.linkLabel6.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
		this.linkLabel6.LinkColor = System.Drawing.Color.White;
		this.linkLabel6.Location = new System.Drawing.Point(0, 0);
		this.linkLabel6.Name = "linkLabel6";
		this.linkLabel6.Size = new System.Drawing.Size(520, 36);
		this.linkLabel6.TabIndex = 0;
		this.linkLabel6.TabStop = true;
		this.linkLabel6.Text = "Store";
		this.linkLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.linkLabel6.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel6_LinkClicked);
		this.label26.BackColor = System.Drawing.Color.LightSeaGreen;
		this.label26.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.label26.Location = new System.Drawing.Point(109, 91);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(411, 3);
		this.label26.TabIndex = 47;
		this.txtStoreOpWeight.AutoEllipsis = true;
		this.txtStoreOpWeight.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtStoreOpWeight.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtStoreOpWeight.Location = new System.Drawing.Point(115, 96);
		this.txtStoreOpWeight.Name = "txtStoreOpWeight";
		this.txtStoreOpWeight.Size = new System.Drawing.Size(106, 22);
		this.txtStoreOpWeight.TabIndex = 48;
		this.txtStoreOpWeight.Text = " 0";
		this.txtStoreOpWeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtStoreInWeight.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtStoreInWeight.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtStoreInWeight.Location = new System.Drawing.Point(227, 96);
		this.txtStoreInWeight.Name = "txtStoreInWeight";
		this.txtStoreInWeight.Size = new System.Drawing.Size(93, 22);
		this.txtStoreInWeight.TabIndex = 49;
		this.txtStoreInWeight.Text = " 0";
		this.txtStoreInWeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtStoreOutWeight.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtStoreOutWeight.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtStoreOutWeight.Location = new System.Drawing.Point(326, 96);
		this.txtStoreOutWeight.Name = "txtStoreOutWeight";
		this.txtStoreOutWeight.Size = new System.Drawing.Size(93, 22);
		this.txtStoreOutWeight.TabIndex = 50;
		this.txtStoreOutWeight.Text = " 0";
		this.txtStoreOutWeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtStoreBalWeight.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtStoreBalWeight.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtStoreBalWeight.Location = new System.Drawing.Point(425, 96);
		this.txtStoreBalWeight.Name = "txtStoreBalWeight";
		this.txtStoreBalWeight.Size = new System.Drawing.Size(93, 22);
		this.txtStoreBalWeight.TabIndex = 51;
		this.txtStoreBalWeight.Text = " 0";
		this.txtStoreBalWeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.panel19.BackColor = System.Drawing.Color.LightGray;
		this.panel19.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel19.Controls.Add(this.label32);
		this.panel19.Controls.Add(this.label33);
		this.panel19.Controls.Add(this.label34);
		this.panel19.Controls.Add(this.label50);
		this.panel19.Controls.Add(this.label64);
		this.panel19.Location = new System.Drawing.Point(0, 36);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(520, 28);
		this.panel19.TabIndex = 37;
		this.label32.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.Black;
		this.label32.Location = new System.Drawing.Point(9, 5);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(88, 17);
		this.label32.TabIndex = 32;
		this.label32.Text = "Description";
		this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.ForeColor = System.Drawing.Color.Black;
		this.label33.Location = new System.Drawing.Point(456, 5);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(55, 17);
		this.label33.TabIndex = 35;
		this.label33.Text = "Balance";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.ForeColor = System.Drawing.Color.Black;
		this.label34.Location = new System.Drawing.Point(364, 5);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(31, 17);
		this.label34.TabIndex = 34;
		this.label34.Text = "Out";
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.ForeColor = System.Drawing.Color.Black;
		this.label50.Location = new System.Drawing.Point(126, 5);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(61, 17);
		this.label50.TabIndex = 32;
		this.label50.Text = "Opening";
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label64.ForeColor = System.Drawing.Color.Black;
		this.label64.Location = new System.Drawing.Point(249, 5);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(20, 17);
		this.label64.TabIndex = 33;
		this.label64.Text = "In";
		this.panel20.BackColor = System.Drawing.Color.LightGray;
		this.panel20.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel20.Controls.Add(this.label71);
		this.panel20.Controls.Add(this.label73);
		this.panel20.Controls.Add(this.label74);
		this.panel20.Location = new System.Drawing.Point(0, 61);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(109, 91);
		this.panel20.TabIndex = 40;
		this.label71.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label71.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label71.ForeColor = System.Drawing.Color.Black;
		this.label71.Location = new System.Drawing.Point(-2, 0);
		this.label71.Name = "label71";
		this.label71.Size = new System.Drawing.Size(110, 30);
		this.label71.TabIndex = 33;
		this.label71.Text = "Qty";
		this.label71.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label73.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label73.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.ForeColor = System.Drawing.Color.Black;
		this.label73.Location = new System.Drawing.Point(-2, 30);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(110, 30);
		this.label73.TabIndex = 34;
		this.label73.Text = "Weight";
		this.label73.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label74.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label74.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label74.ForeColor = System.Drawing.Color.Black;
		this.label74.Location = new System.Drawing.Point(-2, 60);
		this.label74.Name = "label74";
		this.label74.Size = new System.Drawing.Size(110, 30);
		this.label74.TabIndex = 35;
		this.label74.Text = "Amount";
		this.label74.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtSTOpQty.AutoEllipsis = true;
		this.txtSTOpQty.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtSTOpQty.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSTOpQty.Location = new System.Drawing.Point(115, 66);
		this.txtSTOpQty.Name = "txtSTOpQty";
		this.txtSTOpQty.Size = new System.Drawing.Size(106, 22);
		this.txtSTOpQty.TabIndex = 36;
		this.txtSTOpQty.Text = " 0";
		this.txtSTOpQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtSTBalAmt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtSTBalAmt.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSTBalAmt.Location = new System.Drawing.Point(425, 124);
		this.txtSTBalAmt.Name = "txtSTBalAmt";
		this.txtSTBalAmt.Size = new System.Drawing.Size(93, 22);
		this.txtSTBalAmt.TabIndex = 47;
		this.txtSTBalAmt.Text = " 0";
		this.txtSTBalAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtSTInQty.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtSTInQty.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSTInQty.Location = new System.Drawing.Point(227, 66);
		this.txtSTInQty.Name = "txtSTInQty";
		this.txtSTInQty.Size = new System.Drawing.Size(93, 22);
		this.txtSTInQty.TabIndex = 41;
		this.txtSTInQty.Text = " 0";
		this.txtSTInQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtSTOutQty.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtSTOutQty.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSTOutQty.Location = new System.Drawing.Point(326, 66);
		this.txtSTOutQty.Name = "txtSTOutQty";
		this.txtSTOutQty.Size = new System.Drawing.Size(93, 22);
		this.txtSTOutQty.TabIndex = 42;
		this.txtSTOutQty.Text = " 0";
		this.txtSTOutQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtSTOutAmt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtSTOutAmt.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSTOutAmt.Location = new System.Drawing.Point(326, 124);
		this.txtSTOutAmt.Name = "txtSTOutAmt";
		this.txtSTOutAmt.Size = new System.Drawing.Size(93, 22);
		this.txtSTOutAmt.TabIndex = 44;
		this.txtSTOutAmt.Text = " 0";
		this.txtSTOutAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtSTBalQty.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtSTBalQty.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSTBalQty.Location = new System.Drawing.Point(425, 66);
		this.txtSTBalQty.Name = "txtSTBalQty";
		this.txtSTBalQty.Size = new System.Drawing.Size(93, 22);
		this.txtSTBalQty.TabIndex = 45;
		this.txtSTBalQty.Text = " 0";
		this.txtSTBalQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtSTOpAmt.AutoEllipsis = true;
		this.txtSTOpAmt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtSTOpAmt.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSTOpAmt.Location = new System.Drawing.Point(115, 124);
		this.txtSTOpAmt.Name = "txtSTOpAmt";
		this.txtSTOpAmt.Size = new System.Drawing.Size(106, 22);
		this.txtSTOpAmt.TabIndex = 38;
		this.txtSTOpAmt.Text = " 0";
		this.txtSTOpAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtSTInAmt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtSTInAmt.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSTInAmt.Location = new System.Drawing.Point(227, 124);
		this.txtSTInAmt.Name = "txtSTInAmt";
		this.txtSTInAmt.Size = new System.Drawing.Size(93, 22);
		this.txtSTInAmt.TabIndex = 40;
		this.txtSTInAmt.Text = " 0";
		this.txtSTInAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.panel16.BackColor = System.Drawing.SystemColors.ButtonHighlight;
		this.panel16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel16.Controls.Add(this.linkLabel5);
		this.panel16.Controls.Add(this.label4);
		this.panel16.Controls.Add(this.label40);
		this.panel16.Controls.Add(this.txtPackMaterialOpWeight);
		this.panel16.Controls.Add(this.txtPackMaterialInWeight);
		this.panel16.Controls.Add(this.txtPackMaterialOutWeight);
		this.panel16.Controls.Add(this.txtPackMaterialBalWeight);
		this.panel16.Controls.Add(this.panel17);
		this.panel16.Controls.Add(this.txtPMBalAmt);
		this.panel16.Controls.Add(this.panel18);
		this.panel16.Controls.Add(this.txtPMOpQty);
		this.panel16.Controls.Add(this.txtPMOutAmt);
		this.panel16.Controls.Add(this.txtPMInQty);
		this.panel16.Controls.Add(this.txtPMOutQty);
		this.panel16.Controls.Add(this.txtPMInAmt);
		this.panel16.Controls.Add(this.txtPMBalQty);
		this.panel16.Controls.Add(this.txtPMOpAmt);
		this.panel16.Cursor = System.Windows.Forms.Cursors.Hand;
		this.panel16.Location = new System.Drawing.Point(13, 414);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(521, 153);
		this.panel16.TabIndex = 53;
		this.linkLabel5.BackColor = System.Drawing.Color.Teal;
		this.linkLabel5.Cursor = System.Windows.Forms.Cursors.Hand;
		this.linkLabel5.Font = new System.Drawing.Font("Calibri", 18f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.linkLabel5.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
		this.linkLabel5.LinkColor = System.Drawing.Color.White;
		this.linkLabel5.Location = new System.Drawing.Point(-1, 0);
		this.linkLabel5.Name = "linkLabel5";
		this.linkLabel5.Size = new System.Drawing.Size(520, 36);
		this.linkLabel5.TabIndex = 0;
		this.linkLabel5.TabStop = true;
		this.linkLabel5.Text = "Packing Material";
		this.linkLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.linkLabel5.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel5_LinkClicked);
		this.label4.BackColor = System.Drawing.Color.LightSeaGreen;
		this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.label4.Location = new System.Drawing.Point(109, 121);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(411, 3);
		this.label4.TabIndex = 52;
		this.label40.BackColor = System.Drawing.Color.LightSeaGreen;
		this.label40.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.label40.Location = new System.Drawing.Point(109, 91);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(411, 3);
		this.label40.TabIndex = 47;
		this.txtPackMaterialOpWeight.AutoEllipsis = true;
		this.txtPackMaterialOpWeight.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtPackMaterialOpWeight.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackMaterialOpWeight.Location = new System.Drawing.Point(109, 96);
		this.txtPackMaterialOpWeight.Name = "txtPackMaterialOpWeight";
		this.txtPackMaterialOpWeight.Size = new System.Drawing.Size(98, 22);
		this.txtPackMaterialOpWeight.TabIndex = 48;
		this.txtPackMaterialOpWeight.Text = " 0";
		this.txtPackMaterialOpWeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtPackMaterialInWeight.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtPackMaterialInWeight.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackMaterialInWeight.Location = new System.Drawing.Point(216, 96);
		this.txtPackMaterialInWeight.Name = "txtPackMaterialInWeight";
		this.txtPackMaterialInWeight.Size = new System.Drawing.Size(93, 22);
		this.txtPackMaterialInWeight.TabIndex = 49;
		this.txtPackMaterialInWeight.Text = " 0";
		this.txtPackMaterialInWeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtPackMaterialOutWeight.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtPackMaterialOutWeight.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackMaterialOutWeight.Location = new System.Drawing.Point(319, 96);
		this.txtPackMaterialOutWeight.Name = "txtPackMaterialOutWeight";
		this.txtPackMaterialOutWeight.Size = new System.Drawing.Size(93, 22);
		this.txtPackMaterialOutWeight.TabIndex = 50;
		this.txtPackMaterialOutWeight.Text = " 0";
		this.txtPackMaterialOutWeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtPackMaterialBalWeight.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtPackMaterialBalWeight.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackMaterialBalWeight.Location = new System.Drawing.Point(422, 96);
		this.txtPackMaterialBalWeight.Name = "txtPackMaterialBalWeight";
		this.txtPackMaterialBalWeight.Size = new System.Drawing.Size(93, 22);
		this.txtPackMaterialBalWeight.TabIndex = 51;
		this.txtPackMaterialBalWeight.Text = " 0";
		this.txtPackMaterialBalWeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.panel17.BackColor = System.Drawing.Color.LightGray;
		this.panel17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel17.Controls.Add(this.label48);
		this.panel17.Controls.Add(this.label49);
		this.panel17.Controls.Add(this.label51);
		this.panel17.Controls.Add(this.label83);
		this.panel17.Controls.Add(this.label84);
		this.panel17.Location = new System.Drawing.Point(0, 36);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(520, 28);
		this.panel17.TabIndex = 37;
		this.label48.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.ForeColor = System.Drawing.Color.Black;
		this.label48.Location = new System.Drawing.Point(9, 5);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(88, 17);
		this.label48.TabIndex = 32;
		this.label48.Text = "Description";
		this.label48.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.ForeColor = System.Drawing.Color.Black;
		this.label49.Location = new System.Drawing.Point(456, 5);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(55, 17);
		this.label49.TabIndex = 35;
		this.label49.Text = "Balance";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.ForeColor = System.Drawing.Color.Black;
		this.label51.Location = new System.Drawing.Point(364, 5);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(31, 17);
		this.label51.TabIndex = 34;
		this.label51.Text = "Out";
		this.label83.AutoSize = true;
		this.label83.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label83.ForeColor = System.Drawing.Color.Black;
		this.label83.Location = new System.Drawing.Point(126, 5);
		this.label83.Name = "label83";
		this.label83.Size = new System.Drawing.Size(61, 17);
		this.label83.TabIndex = 32;
		this.label83.Text = "Opening";
		this.label84.AutoSize = true;
		this.label84.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label84.ForeColor = System.Drawing.Color.Black;
		this.label84.Location = new System.Drawing.Point(249, 5);
		this.label84.Name = "label84";
		this.label84.Size = new System.Drawing.Size(20, 17);
		this.label84.TabIndex = 33;
		this.label84.Text = "In";
		this.txtPMBalAmt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtPMBalAmt.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPMBalAmt.Location = new System.Drawing.Point(422, 127);
		this.txtPMBalAmt.Name = "txtPMBalAmt";
		this.txtPMBalAmt.Size = new System.Drawing.Size(93, 22);
		this.txtPMBalAmt.TabIndex = 47;
		this.txtPMBalAmt.Text = " 0";
		this.txtPMBalAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.panel18.BackColor = System.Drawing.Color.LightGray;
		this.panel18.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel18.Controls.Add(this.label85);
		this.panel18.Controls.Add(this.label86);
		this.panel18.Controls.Add(this.label87);
		this.panel18.Location = new System.Drawing.Point(0, 61);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(109, 91);
		this.panel18.TabIndex = 40;
		this.label85.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label85.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label85.ForeColor = System.Drawing.Color.Black;
		this.label85.Location = new System.Drawing.Point(-2, 0);
		this.label85.Name = "label85";
		this.label85.Size = new System.Drawing.Size(110, 30);
		this.label85.TabIndex = 33;
		this.label85.Text = "Qty";
		this.label85.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label86.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label86.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label86.ForeColor = System.Drawing.Color.Black;
		this.label86.Location = new System.Drawing.Point(-2, 30);
		this.label86.Name = "label86";
		this.label86.Size = new System.Drawing.Size(110, 30);
		this.label86.TabIndex = 34;
		this.label86.Text = "Weight";
		this.label86.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label87.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label87.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label87.ForeColor = System.Drawing.Color.Black;
		this.label87.Location = new System.Drawing.Point(-2, 60);
		this.label87.Name = "label87";
		this.label87.Size = new System.Drawing.Size(110, 30);
		this.label87.TabIndex = 35;
		this.label87.Text = "Amount";
		this.label87.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtPMOpQty.AutoEllipsis = true;
		this.txtPMOpQty.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtPMOpQty.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPMOpQty.Location = new System.Drawing.Point(109, 66);
		this.txtPMOpQty.Name = "txtPMOpQty";
		this.txtPMOpQty.Size = new System.Drawing.Size(98, 22);
		this.txtPMOpQty.TabIndex = 36;
		this.txtPMOpQty.Text = " 0";
		this.txtPMOpQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtPMOutAmt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtPMOutAmt.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPMOutAmt.Location = new System.Drawing.Point(319, 127);
		this.txtPMOutAmt.Name = "txtPMOutAmt";
		this.txtPMOutAmt.Size = new System.Drawing.Size(93, 22);
		this.txtPMOutAmt.TabIndex = 44;
		this.txtPMOutAmt.Text = " 0";
		this.txtPMOutAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtPMInQty.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtPMInQty.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPMInQty.Location = new System.Drawing.Point(216, 66);
		this.txtPMInQty.Name = "txtPMInQty";
		this.txtPMInQty.Size = new System.Drawing.Size(93, 22);
		this.txtPMInQty.TabIndex = 41;
		this.txtPMInQty.Text = " 0";
		this.txtPMInQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtPMOutQty.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtPMOutQty.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPMOutQty.Location = new System.Drawing.Point(319, 66);
		this.txtPMOutQty.Name = "txtPMOutQty";
		this.txtPMOutQty.Size = new System.Drawing.Size(93, 22);
		this.txtPMOutQty.TabIndex = 42;
		this.txtPMOutQty.Text = " 0";
		this.txtPMOutQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtPMInAmt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtPMInAmt.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPMInAmt.Location = new System.Drawing.Point(216, 127);
		this.txtPMInAmt.Name = "txtPMInAmt";
		this.txtPMInAmt.Size = new System.Drawing.Size(93, 22);
		this.txtPMInAmt.TabIndex = 40;
		this.txtPMInAmt.Text = " 0";
		this.txtPMInAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtPMBalQty.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtPMBalQty.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPMBalQty.Location = new System.Drawing.Point(422, 66);
		this.txtPMBalQty.Name = "txtPMBalQty";
		this.txtPMBalQty.Size = new System.Drawing.Size(93, 22);
		this.txtPMBalQty.TabIndex = 45;
		this.txtPMBalQty.Text = " 0";
		this.txtPMBalQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtPMOpAmt.AutoEllipsis = true;
		this.txtPMOpAmt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtPMOpAmt.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPMOpAmt.Location = new System.Drawing.Point(109, 127);
		this.txtPMOpAmt.Name = "txtPMOpAmt";
		this.txtPMOpAmt.Size = new System.Drawing.Size(98, 22);
		this.txtPMOpAmt.TabIndex = 38;
		this.txtPMOpAmt.Text = " 0";
		this.txtPMOpAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label82.BackColor = System.Drawing.Color.LightSeaGreen;
		this.label82.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.label82.Location = new System.Drawing.Point(672, 357);
		this.label82.Name = "label82";
		this.label82.Size = new System.Drawing.Size(410, 3);
		this.label82.TabIndex = 53;
		this.panel13.BackColor = System.Drawing.SystemColors.ButtonHighlight;
		this.panel13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel13.Controls.Add(this.label81);
		this.panel13.Controls.Add(this.linkLabel4);
		this.panel13.Controls.Add(this.txtFinishGoodsOpWeight);
		this.panel13.Controls.Add(this.txtFinishGoodsInWeight);
		this.panel13.Controls.Add(this.txtFinishGoodsOutWeight);
		this.panel13.Controls.Add(this.txtFinishGoodsBalWeight);
		this.panel13.Controls.Add(this.panel14);
		this.panel13.Controls.Add(this.panel15);
		this.panel13.Controls.Add(this.txtFGOpQty);
		this.panel13.Controls.Add(this.txtFGInQty);
		this.panel13.Controls.Add(this.txtFGBalAmt);
		this.panel13.Controls.Add(this.txtFGOutQty);
		this.panel13.Controls.Add(this.txtFGBalQty);
		this.panel13.Controls.Add(this.txtFGOutAmt);
		this.panel13.Controls.Add(this.txtFGOpAmt);
		this.panel13.Controls.Add(this.txtFGInAmt);
		this.panel13.Cursor = System.Windows.Forms.Cursors.Hand;
		this.panel13.Location = new System.Drawing.Point(562, 235);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(521, 153);
		this.panel13.TabIndex = 52;
		this.label81.BackColor = System.Drawing.Color.LightSeaGreen;
		this.label81.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.label81.Location = new System.Drawing.Point(109, 91);
		this.label81.Name = "label81";
		this.label81.Size = new System.Drawing.Size(411, 3);
		this.label81.TabIndex = 53;
		this.linkLabel4.BackColor = System.Drawing.Color.Teal;
		this.linkLabel4.Cursor = System.Windows.Forms.Cursors.Hand;
		this.linkLabel4.Font = new System.Drawing.Font("Calibri", 18f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.linkLabel4.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
		this.linkLabel4.LinkColor = System.Drawing.Color.White;
		this.linkLabel4.Location = new System.Drawing.Point(0, 0);
		this.linkLabel4.Name = "linkLabel4";
		this.linkLabel4.Size = new System.Drawing.Size(520, 36);
		this.linkLabel4.TabIndex = 0;
		this.linkLabel4.TabStop = true;
		this.linkLabel4.Text = "Finish Goods";
		this.linkLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.linkLabel4.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel4_LinkClicked);
		this.txtFinishGoodsOpWeight.AutoEllipsis = true;
		this.txtFinishGoodsOpWeight.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtFinishGoodsOpWeight.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFinishGoodsOpWeight.Location = new System.Drawing.Point(115, 96);
		this.txtFinishGoodsOpWeight.Name = "txtFinishGoodsOpWeight";
		this.txtFinishGoodsOpWeight.Size = new System.Drawing.Size(106, 22);
		this.txtFinishGoodsOpWeight.TabIndex = 48;
		this.txtFinishGoodsOpWeight.Text = " 0";
		this.txtFinishGoodsOpWeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtFinishGoodsInWeight.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtFinishGoodsInWeight.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFinishGoodsInWeight.Location = new System.Drawing.Point(227, 96);
		this.txtFinishGoodsInWeight.Name = "txtFinishGoodsInWeight";
		this.txtFinishGoodsInWeight.Size = new System.Drawing.Size(93, 22);
		this.txtFinishGoodsInWeight.TabIndex = 49;
		this.txtFinishGoodsInWeight.Text = " 0";
		this.txtFinishGoodsInWeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtFinishGoodsOutWeight.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtFinishGoodsOutWeight.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFinishGoodsOutWeight.Location = new System.Drawing.Point(326, 96);
		this.txtFinishGoodsOutWeight.Name = "txtFinishGoodsOutWeight";
		this.txtFinishGoodsOutWeight.Size = new System.Drawing.Size(93, 22);
		this.txtFinishGoodsOutWeight.TabIndex = 50;
		this.txtFinishGoodsOutWeight.Text = " 0";
		this.txtFinishGoodsOutWeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtFinishGoodsBalWeight.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtFinishGoodsBalWeight.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFinishGoodsBalWeight.Location = new System.Drawing.Point(425, 96);
		this.txtFinishGoodsBalWeight.Name = "txtFinishGoodsBalWeight";
		this.txtFinishGoodsBalWeight.Size = new System.Drawing.Size(93, 22);
		this.txtFinishGoodsBalWeight.TabIndex = 51;
		this.txtFinishGoodsBalWeight.Text = " 0";
		this.txtFinishGoodsBalWeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.panel14.BackColor = System.Drawing.Color.LightGray;
		this.panel14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel14.Controls.Add(this.label59);
		this.panel14.Controls.Add(this.label60);
		this.panel14.Controls.Add(this.label61);
		this.panel14.Controls.Add(this.label62);
		this.panel14.Controls.Add(this.label63);
		this.panel14.Location = new System.Drawing.Point(0, 36);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(520, 28);
		this.panel14.TabIndex = 37;
		this.label59.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label59.ForeColor = System.Drawing.Color.Black;
		this.label59.Location = new System.Drawing.Point(9, 5);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(88, 17);
		this.label59.TabIndex = 32;
		this.label59.Text = "Description";
		this.label59.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.ForeColor = System.Drawing.Color.Black;
		this.label60.Location = new System.Drawing.Point(456, 5);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(55, 17);
		this.label60.TabIndex = 35;
		this.label60.Text = "Balance";
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label61.ForeColor = System.Drawing.Color.Black;
		this.label61.Location = new System.Drawing.Point(364, 5);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(31, 17);
		this.label61.TabIndex = 34;
		this.label61.Text = "Out";
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label62.ForeColor = System.Drawing.Color.Black;
		this.label62.Location = new System.Drawing.Point(126, 5);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(61, 17);
		this.label62.TabIndex = 32;
		this.label62.Text = "Opening";
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label63.ForeColor = System.Drawing.Color.Black;
		this.label63.Location = new System.Drawing.Point(249, 5);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(20, 17);
		this.label63.TabIndex = 33;
		this.label63.Text = "In";
		this.panel15.BackColor = System.Drawing.Color.LightGray;
		this.panel15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel15.Controls.Add(this.label69);
		this.panel15.Controls.Add(this.label72);
		this.panel15.Controls.Add(this.label75);
		this.panel15.Location = new System.Drawing.Point(0, 61);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(109, 91);
		this.panel15.TabIndex = 40;
		this.label69.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label69.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label69.ForeColor = System.Drawing.Color.Black;
		this.label69.Location = new System.Drawing.Point(-2, 0);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(110, 30);
		this.label69.TabIndex = 33;
		this.label69.Text = "Qty";
		this.label69.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label72.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label72.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label72.ForeColor = System.Drawing.Color.Black;
		this.label72.Location = new System.Drawing.Point(-2, 30);
		this.label72.Name = "label72";
		this.label72.Size = new System.Drawing.Size(110, 30);
		this.label72.TabIndex = 34;
		this.label72.Text = "Weight";
		this.label72.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label75.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label75.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label75.ForeColor = System.Drawing.Color.Black;
		this.label75.Location = new System.Drawing.Point(-2, 60);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(110, 30);
		this.label75.TabIndex = 35;
		this.label75.Text = "Amount";
		this.label75.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtFGOpQty.AutoEllipsis = true;
		this.txtFGOpQty.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtFGOpQty.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFGOpQty.Location = new System.Drawing.Point(115, 67);
		this.txtFGOpQty.Name = "txtFGOpQty";
		this.txtFGOpQty.Size = new System.Drawing.Size(106, 22);
		this.txtFGOpQty.TabIndex = 36;
		this.txtFGOpQty.Text = " 0";
		this.txtFGOpQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtFGInQty.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtFGInQty.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFGInQty.Location = new System.Drawing.Point(227, 67);
		this.txtFGInQty.Name = "txtFGInQty";
		this.txtFGInQty.Size = new System.Drawing.Size(93, 22);
		this.txtFGInQty.TabIndex = 41;
		this.txtFGInQty.Text = " 0";
		this.txtFGInQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtFGBalAmt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtFGBalAmt.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFGBalAmt.Location = new System.Drawing.Point(425, 126);
		this.txtFGBalAmt.Name = "txtFGBalAmt";
		this.txtFGBalAmt.Size = new System.Drawing.Size(93, 22);
		this.txtFGBalAmt.TabIndex = 47;
		this.txtFGBalAmt.Text = " 0";
		this.txtFGBalAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtFGOutQty.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtFGOutQty.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFGOutQty.Location = new System.Drawing.Point(326, 67);
		this.txtFGOutQty.Name = "txtFGOutQty";
		this.txtFGOutQty.Size = new System.Drawing.Size(93, 22);
		this.txtFGOutQty.TabIndex = 42;
		this.txtFGOutQty.Text = " 0";
		this.txtFGOutQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtFGBalQty.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtFGBalQty.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFGBalQty.Location = new System.Drawing.Point(425, 67);
		this.txtFGBalQty.Name = "txtFGBalQty";
		this.txtFGBalQty.Size = new System.Drawing.Size(93, 22);
		this.txtFGBalQty.TabIndex = 45;
		this.txtFGBalQty.Text = " 0";
		this.txtFGBalQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtFGOutAmt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtFGOutAmt.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFGOutAmt.Location = new System.Drawing.Point(326, 126);
		this.txtFGOutAmt.Name = "txtFGOutAmt";
		this.txtFGOutAmt.Size = new System.Drawing.Size(93, 22);
		this.txtFGOutAmt.TabIndex = 44;
		this.txtFGOutAmt.Text = " 0";
		this.txtFGOutAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtFGOpAmt.AutoEllipsis = true;
		this.txtFGOpAmt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtFGOpAmt.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFGOpAmt.Location = new System.Drawing.Point(115, 126);
		this.txtFGOpAmt.Name = "txtFGOpAmt";
		this.txtFGOpAmt.Size = new System.Drawing.Size(106, 22);
		this.txtFGOpAmt.TabIndex = 38;
		this.txtFGOpAmt.Text = " 0";
		this.txtFGOpAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtFGInAmt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtFGInAmt.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFGInAmt.Location = new System.Drawing.Point(227, 126);
		this.txtFGInAmt.Name = "txtFGInAmt";
		this.txtFGInAmt.Size = new System.Drawing.Size(93, 22);
		this.txtFGInAmt.TabIndex = 40;
		this.txtFGInAmt.Text = " 0";
		this.txtFGInAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.panel10.BackColor = System.Drawing.SystemColors.ButtonHighlight;
		this.panel10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel10.Controls.Add(this.label80);
		this.panel10.Controls.Add(this.label78);
		this.panel10.Controls.Add(this.txtByProductOPWeight);
		this.panel10.Controls.Add(this.txtByProductInWeight);
		this.panel10.Controls.Add(this.txtByProductOutWeight);
		this.panel10.Controls.Add(this.txtByProductBalWeight);
		this.panel10.Controls.Add(this.panel12);
		this.panel10.Controls.Add(this.panel11);
		this.panel10.Controls.Add(this.linkLabel2);
		this.panel10.Controls.Add(this.txtBPOpQty);
		this.panel10.Controls.Add(this.txtBPInQty);
		this.panel10.Controls.Add(this.txtBPOutQty);
		this.panel10.Controls.Add(this.txtBPBalQty);
		this.panel10.Controls.Add(this.txtBPOpAmt);
		this.panel10.Controls.Add(this.txtBPInAmt);
		this.panel10.Controls.Add(this.txtBPOutAmt);
		this.panel10.Controls.Add(this.txtBPBalAmt);
		this.panel10.Cursor = System.Windows.Forms.Cursors.Hand;
		this.panel10.Location = new System.Drawing.Point(13, 235);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(521, 153);
		this.panel10.TabIndex = 41;
		this.label80.BackColor = System.Drawing.Color.LightSeaGreen;
		this.label80.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.label80.Location = new System.Drawing.Point(109, 121);
		this.label80.Name = "label80";
		this.label80.Size = new System.Drawing.Size(411, 3);
		this.label80.TabIndex = 52;
		this.label78.BackColor = System.Drawing.Color.LightSeaGreen;
		this.label78.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.label78.Location = new System.Drawing.Point(109, 91);
		this.label78.Name = "label78";
		this.label78.Size = new System.Drawing.Size(411, 3);
		this.label78.TabIndex = 47;
		this.txtByProductOPWeight.AutoEllipsis = true;
		this.txtByProductOPWeight.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtByProductOPWeight.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtByProductOPWeight.Location = new System.Drawing.Point(109, 96);
		this.txtByProductOPWeight.Name = "txtByProductOPWeight";
		this.txtByProductOPWeight.Size = new System.Drawing.Size(98, 22);
		this.txtByProductOPWeight.TabIndex = 48;
		this.txtByProductOPWeight.Text = " 0";
		this.txtByProductOPWeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtByProductInWeight.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtByProductInWeight.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtByProductInWeight.Location = new System.Drawing.Point(216, 96);
		this.txtByProductInWeight.Name = "txtByProductInWeight";
		this.txtByProductInWeight.Size = new System.Drawing.Size(93, 22);
		this.txtByProductInWeight.TabIndex = 49;
		this.txtByProductInWeight.Text = " 0";
		this.txtByProductInWeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtByProductOutWeight.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtByProductOutWeight.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtByProductOutWeight.Location = new System.Drawing.Point(319, 96);
		this.txtByProductOutWeight.Name = "txtByProductOutWeight";
		this.txtByProductOutWeight.Size = new System.Drawing.Size(93, 22);
		this.txtByProductOutWeight.TabIndex = 50;
		this.txtByProductOutWeight.Text = " 0";
		this.txtByProductOutWeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtByProductBalWeight.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtByProductBalWeight.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtByProductBalWeight.Location = new System.Drawing.Point(422, 96);
		this.txtByProductBalWeight.Name = "txtByProductBalWeight";
		this.txtByProductBalWeight.Size = new System.Drawing.Size(93, 22);
		this.txtByProductBalWeight.TabIndex = 51;
		this.txtByProductBalWeight.Text = " 0";
		this.txtByProductBalWeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.panel12.BackColor = System.Drawing.Color.LightGray;
		this.panel12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel12.Controls.Add(this.label9);
		this.panel12.Controls.Add(this.label10);
		this.panel12.Controls.Add(this.label11);
		this.panel12.Controls.Add(this.label15);
		this.panel12.Controls.Add(this.label16);
		this.panel12.Location = new System.Drawing.Point(0, 36);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(520, 28);
		this.panel12.TabIndex = 37;
		this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.Black;
		this.label9.Location = new System.Drawing.Point(9, 5);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(88, 17);
		this.label9.TabIndex = 32;
		this.label9.Text = "Description";
		this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.Black;
		this.label10.Location = new System.Drawing.Point(456, 5);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(55, 17);
		this.label10.TabIndex = 35;
		this.label10.Text = "Balance";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.Black;
		this.label11.Location = new System.Drawing.Point(364, 5);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(31, 17);
		this.label11.TabIndex = 34;
		this.label11.Text = "Out";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.Black;
		this.label15.Location = new System.Drawing.Point(126, 5);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(61, 17);
		this.label15.TabIndex = 32;
		this.label15.Text = "Opening";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.Black;
		this.label16.Location = new System.Drawing.Point(249, 5);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(20, 17);
		this.label16.TabIndex = 33;
		this.label16.Text = "In";
		this.panel11.BackColor = System.Drawing.Color.LightGray;
		this.panel11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel11.Controls.Add(this.label3);
		this.panel11.Controls.Add(this.label5);
		this.panel11.Controls.Add(this.label8);
		this.panel11.Location = new System.Drawing.Point(0, 61);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(109, 91);
		this.panel11.TabIndex = 40;
		this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.Black;
		this.label3.Location = new System.Drawing.Point(-2, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(110, 30);
		this.label3.TabIndex = 33;
		this.label3.Text = "Qty";
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.Black;
		this.label5.Location = new System.Drawing.Point(-2, 30);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(110, 30);
		this.label5.TabIndex = 34;
		this.label5.Text = "Weight";
		this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label8.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.Black;
		this.label8.Location = new System.Drawing.Point(-2, 60);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(110, 30);
		this.label8.TabIndex = 35;
		this.label8.Text = "Amount";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.linkLabel2.BackColor = System.Drawing.Color.Teal;
		this.linkLabel2.Cursor = System.Windows.Forms.Cursors.Hand;
		this.linkLabel2.Font = new System.Drawing.Font("Calibri", 18f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.linkLabel2.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
		this.linkLabel2.LinkColor = System.Drawing.Color.White;
		this.linkLabel2.Location = new System.Drawing.Point(-1, 0);
		this.linkLabel2.Name = "linkLabel2";
		this.linkLabel2.Size = new System.Drawing.Size(520, 36);
		this.linkLabel2.TabIndex = 0;
		this.linkLabel2.TabStop = true;
		this.linkLabel2.Text = "By Product";
		this.linkLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel2_LinkClicked);
		this.txtBPOpQty.AutoEllipsis = true;
		this.txtBPOpQty.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtBPOpQty.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBPOpQty.Location = new System.Drawing.Point(109, 67);
		this.txtBPOpQty.Name = "txtBPOpQty";
		this.txtBPOpQty.Size = new System.Drawing.Size(98, 22);
		this.txtBPOpQty.TabIndex = 36;
		this.txtBPOpQty.Text = " 0";
		this.txtBPOpQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtBPInQty.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtBPInQty.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBPInQty.Location = new System.Drawing.Point(216, 67);
		this.txtBPInQty.Name = "txtBPInQty";
		this.txtBPInQty.Size = new System.Drawing.Size(93, 22);
		this.txtBPInQty.TabIndex = 41;
		this.txtBPInQty.Text = " 0";
		this.txtBPInQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtBPOutQty.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtBPOutQty.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBPOutQty.Location = new System.Drawing.Point(319, 67);
		this.txtBPOutQty.Name = "txtBPOutQty";
		this.txtBPOutQty.Size = new System.Drawing.Size(93, 22);
		this.txtBPOutQty.TabIndex = 42;
		this.txtBPOutQty.Text = " 0";
		this.txtBPOutQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtBPBalQty.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtBPBalQty.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBPBalQty.Location = new System.Drawing.Point(422, 67);
		this.txtBPBalQty.Name = "txtBPBalQty";
		this.txtBPBalQty.Size = new System.Drawing.Size(93, 22);
		this.txtBPBalQty.TabIndex = 45;
		this.txtBPBalQty.Text = " 0";
		this.txtBPBalQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtBPOpAmt.AutoEllipsis = true;
		this.txtBPOpAmt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtBPOpAmt.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBPOpAmt.Location = new System.Drawing.Point(109, 126);
		this.txtBPOpAmt.Name = "txtBPOpAmt";
		this.txtBPOpAmt.Size = new System.Drawing.Size(98, 22);
		this.txtBPOpAmt.TabIndex = 38;
		this.txtBPOpAmt.Text = " 0";
		this.txtBPOpAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtBPInAmt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtBPInAmt.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBPInAmt.Location = new System.Drawing.Point(216, 126);
		this.txtBPInAmt.Name = "txtBPInAmt";
		this.txtBPInAmt.Size = new System.Drawing.Size(93, 22);
		this.txtBPInAmt.TabIndex = 40;
		this.txtBPInAmt.Text = " 0";
		this.txtBPInAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtBPOutAmt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtBPOutAmt.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBPOutAmt.Location = new System.Drawing.Point(319, 126);
		this.txtBPOutAmt.Name = "txtBPOutAmt";
		this.txtBPOutAmt.Size = new System.Drawing.Size(93, 22);
		this.txtBPOutAmt.TabIndex = 44;
		this.txtBPOutAmt.Text = " 0";
		this.txtBPOutAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtBPBalAmt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtBPBalAmt.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBPBalAmt.Location = new System.Drawing.Point(422, 126);
		this.txtBPBalAmt.Name = "txtBPBalAmt";
		this.txtBPBalAmt.Size = new System.Drawing.Size(93, 22);
		this.txtBPBalAmt.TabIndex = 47;
		this.txtBPBalAmt.Text = " 0";
		this.txtBPBalAmt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.panel8.BackColor = System.Drawing.Color.LightGray;
		this.panel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel8.Controls.Add(this.label36);
		this.panel8.Controls.Add(this.label44);
		this.panel8.Controls.Add(this.label47);
		this.panel8.Controls.Add(this.label52);
		this.panel8.Controls.Add(this.label53);
		this.panel8.Location = new System.Drawing.Point(563, 99);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(520, 28);
		this.panel8.TabIndex = 37;
		this.label36.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.ForeColor = System.Drawing.Color.Black;
		this.label36.Location = new System.Drawing.Point(9, 5);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(88, 17);
		this.label36.TabIndex = 32;
		this.label36.Text = "Description";
		this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.ForeColor = System.Drawing.Color.Black;
		this.label44.Location = new System.Drawing.Point(456, 5);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(55, 17);
		this.label44.TabIndex = 35;
		this.label44.Text = "Balance";
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.ForeColor = System.Drawing.Color.Black;
		this.label47.Location = new System.Drawing.Point(364, 5);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(31, 17);
		this.label47.TabIndex = 34;
		this.label47.Text = "Out";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.ForeColor = System.Drawing.Color.Black;
		this.label52.Location = new System.Drawing.Point(126, 5);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(61, 17);
		this.label52.TabIndex = 32;
		this.label52.Text = "Opening";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.ForeColor = System.Drawing.Color.Black;
		this.label53.Location = new System.Drawing.Point(249, 5);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(20, 17);
		this.label53.TabIndex = 33;
		this.label53.Text = "In";
		this.panel7.BackColor = System.Drawing.Color.LightGray;
		this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel7.Controls.Add(this.label21);
		this.panel7.Controls.Add(this.label22);
		this.panel7.Controls.Add(this.label23);
		this.panel7.Location = new System.Drawing.Point(13, 126);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(109, 91);
		this.panel7.TabIndex = 39;
		this.label21.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label21.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.Black;
		this.label21.Location = new System.Drawing.Point(-2, 0);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(110, 30);
		this.label21.TabIndex = 33;
		this.label21.Text = "Qty";
		this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label22.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.Black;
		this.label22.Location = new System.Drawing.Point(-2, 30);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(110, 30);
		this.label22.TabIndex = 34;
		this.label22.Text = "Weight";
		this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label23.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.Black;
		this.label23.Location = new System.Drawing.Point(-2, 60);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(110, 30);
		this.label23.TabIndex = 35;
		this.label23.Text = "Amount";
		this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label35.BackColor = System.Drawing.Color.LightSeaGreen;
		this.label35.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.label35.Location = new System.Drawing.Point(14, 186);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(519, 3);
		this.label35.TabIndex = 38;
		this.label2.BackColor = System.Drawing.Color.LightSeaGreen;
		this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.label2.Location = new System.Drawing.Point(14, 156);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(519, 3);
		this.label2.TabIndex = 37;
		this.panel6.BackColor = System.Drawing.Color.LightGray;
		this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel6.Controls.Add(this.label20);
		this.panel6.Controls.Add(this.label65);
		this.panel6.Controls.Add(this.label66);
		this.panel6.Controls.Add(this.label68);
		this.panel6.Controls.Add(this.label67);
		this.panel6.Location = new System.Drawing.Point(13, 99);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(520, 28);
		this.panel6.TabIndex = 36;
		this.label20.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.ForeColor = System.Drawing.Color.Black;
		this.label20.Location = new System.Drawing.Point(9, 5);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(88, 17);
		this.label20.TabIndex = 32;
		this.label20.Text = "Description";
		this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label65.ForeColor = System.Drawing.Color.Black;
		this.label65.Location = new System.Drawing.Point(456, 5);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(55, 17);
		this.label65.TabIndex = 35;
		this.label65.Text = "Balance";
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label66.ForeColor = System.Drawing.Color.Black;
		this.label66.Location = new System.Drawing.Point(364, 5);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(31, 17);
		this.label66.TabIndex = 34;
		this.label66.Text = "Out";
		this.label68.AutoSize = true;
		this.label68.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label68.ForeColor = System.Drawing.Color.Black;
		this.label68.Location = new System.Drawing.Point(126, 5);
		this.label68.Name = "label68";
		this.label68.Size = new System.Drawing.Size(61, 17);
		this.label68.TabIndex = 32;
		this.label68.Text = "Opening";
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label67.ForeColor = System.Drawing.Color.Black;
		this.label67.Location = new System.Drawing.Point(249, 5);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(20, 17);
		this.label67.TabIndex = 33;
		this.label67.Text = "In";
		this.rdSaleValue.AutoSize = true;
		this.rdSaleValue.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.rdSaleValue.Location = new System.Drawing.Point(536, 34);
		this.rdSaleValue.Name = "rdSaleValue";
		this.rdSaleValue.Size = new System.Drawing.Size(96, 23);
		this.rdSaleValue.TabIndex = 4;
		this.rdSaleValue.TabStop = true;
		this.rdSaleValue.Text = "Sale Value";
		this.rdSaleValue.UseVisualStyleBackColor = true;
		this.rdStockValue.AutoSize = true;
		this.rdStockValue.Checked = true;
		this.rdStockValue.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdStockValue.Location = new System.Drawing.Point(536, 6);
		this.rdStockValue.Name = "rdStockValue";
		this.rdStockValue.Size = new System.Drawing.Size(106, 23);
		this.rdStockValue.TabIndex = 3;
		this.rdStockValue.TabStop = true;
		this.rdStockValue.Text = "Stock Value";
		this.rdStockValue.UseVisualStyleBackColor = true;
		this.btnsearch.BackColor = System.Drawing.Color.FromArgb(128, 128, 255);
		this.btnsearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btnsearch.FlatAppearance.BorderSize = 0;
		this.btnsearch.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnsearch.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsearch.ForeColor = System.Drawing.Color.White;
		this.btnsearch.Location = new System.Drawing.Point(648, 23);
		this.btnsearch.Name = "btnsearch";
		this.btnsearch.Size = new System.Drawing.Size(75, 29);
		this.btnsearch.TabIndex = 5;
		this.btnsearch.Text = "Show";
		this.btnsearch.UseVisualStyleBackColor = false;
		this.btnsearch.Click += new System.EventHandler(btnsearch_Click);
		this.datFromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.CustomFormat = "dd-MMM-yyy";
		this.datFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFromDate.Location = new System.Drawing.Point(15, 32);
		this.datFromDate.Name = "datFromDate";
		this.datFromDate.Size = new System.Drawing.Size(134, 23);
		this.datFromDate.TabIndex = 0;
		this.datToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.CustomFormat = "dd-MMM-yyy";
		this.datToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datToDate.Location = new System.Drawing.Point(152, 32);
		this.datToDate.Name = "datToDate";
		this.datToDate.Size = new System.Drawing.Size(150, 23);
		this.datToDate.TabIndex = 2;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(17, 4);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(76, 19);
		this.label17.TabIndex = 17;
		this.label17.Text = "Date From";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(154, 4);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(59, 19);
		this.label18.TabIndex = 18;
		this.label18.Text = "Date To";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add(this.panel9);
		this.panel4.Controls.Add(this.label12);
		this.panel4.Controls.Add(this.linkLabel1);
		this.panel4.Controls.Add(this.txtRawBalWeightPdy);
		this.panel4.Controls.Add(this.txtRawOutWeightPdy);
		this.panel4.Controls.Add(this.txtRawInWeightPdy);
		this.panel4.Controls.Add(this.txtRawOPWeightPdy);
		this.panel4.Controls.Add(this.txtRawBalAmtPdy);
		this.panel4.Controls.Add(this.txtRawOutAmtPdy);
		this.panel4.Controls.Add(this.txtRawInAmtPdy);
		this.panel4.Controls.Add(this.txtRawBalQtyPdy);
		this.panel4.Controls.Add(this.txtRawOpAmtPdy);
		this.panel4.Controls.Add(this.txtRawOutQtyPdy);
		this.panel4.Controls.Add(this.txtRawInQtyPdy);
		this.panel4.Controls.Add(this.txtRawOpQtyPdy);
		this.panel4.Cursor = System.Windows.Forms.Cursors.Hand;
		this.panel4.Location = new System.Drawing.Point(562, 64);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(521, 153);
		this.panel4.TabIndex = 0;
		this.label7.BackColor = System.Drawing.Color.LightSeaGreen;
		this.label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.label7.Location = new System.Drawing.Point(109, 121);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(410, 3);
		this.label7.TabIndex = 46;
		this.panel9.BackColor = System.Drawing.Color.LightGray;
		this.panel9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel9.Controls.Add(this.label13);
		this.panel9.Controls.Add(this.label14);
		this.panel9.Controls.Add(this.label19);
		this.panel9.Location = new System.Drawing.Point(0, 61);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(109, 91);
		this.panel9.TabIndex = 40;
		this.label13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.Black;
		this.label13.Location = new System.Drawing.Point(-2, 0);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(110, 30);
		this.label13.TabIndex = 33;
		this.label13.Text = "Qty";
		this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label14.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.Black;
		this.label14.Location = new System.Drawing.Point(-2, 30);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(110, 30);
		this.label14.TabIndex = 34;
		this.label14.Text = "Weight";
		this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label19.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label19.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.Black;
		this.label19.Location = new System.Drawing.Point(-2, 60);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(110, 30);
		this.label19.TabIndex = 35;
		this.label19.Text = "Amount";
		this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label12.BackColor = System.Drawing.Color.LightSeaGreen;
		this.label12.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.label12.Location = new System.Drawing.Point(1, 91);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(519, 3);
		this.label12.TabIndex = 40;
		this.linkLabel1.BackColor = System.Drawing.Color.Teal;
		this.linkLabel1.Cursor = System.Windows.Forms.Cursors.Hand;
		this.linkLabel1.Font = new System.Drawing.Font("Calibri", 18f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.linkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
		this.linkLabel1.LinkColor = System.Drawing.Color.White;
		this.linkLabel1.Location = new System.Drawing.Point(0, -1);
		this.linkLabel1.Name = "linkLabel1";
		this.linkLabel1.Size = new System.Drawing.Size(520, 36);
		this.linkLabel1.TabIndex = 0;
		this.linkLabel1.TabStop = true;
		this.linkLabel1.Text = "Raw Material Paddy";
		this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel1_LinkClicked);
		this.txtRawBalWeightPdy.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtRawBalWeightPdy.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRawBalWeightPdy.Location = new System.Drawing.Point(425, 96);
		this.txtRawBalWeightPdy.Name = "txtRawBalWeightPdy";
		this.txtRawBalWeightPdy.Size = new System.Drawing.Size(93, 22);
		this.txtRawBalWeightPdy.TabIndex = 45;
		this.txtRawBalWeightPdy.Text = " 0";
		this.txtRawBalWeightPdy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtRawOutWeightPdy.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtRawOutWeightPdy.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRawOutWeightPdy.Location = new System.Drawing.Point(326, 96);
		this.txtRawOutWeightPdy.Name = "txtRawOutWeightPdy";
		this.txtRawOutWeightPdy.Size = new System.Drawing.Size(93, 22);
		this.txtRawOutWeightPdy.TabIndex = 43;
		this.txtRawOutWeightPdy.Text = " 0";
		this.txtRawOutWeightPdy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtRawInWeightPdy.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtRawInWeightPdy.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRawInWeightPdy.Location = new System.Drawing.Point(227, 96);
		this.txtRawInWeightPdy.Name = "txtRawInWeightPdy";
		this.txtRawInWeightPdy.Size = new System.Drawing.Size(93, 22);
		this.txtRawInWeightPdy.TabIndex = 42;
		this.txtRawInWeightPdy.Text = " 0";
		this.txtRawInWeightPdy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtRawOPWeightPdy.AutoEllipsis = true;
		this.txtRawOPWeightPdy.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtRawOPWeightPdy.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRawOPWeightPdy.Location = new System.Drawing.Point(115, 96);
		this.txtRawOPWeightPdy.Name = "txtRawOPWeightPdy";
		this.txtRawOPWeightPdy.Size = new System.Drawing.Size(106, 22);
		this.txtRawOPWeightPdy.TabIndex = 41;
		this.txtRawOPWeightPdy.Text = " 0";
		this.txtRawOPWeightPdy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtRawBalAmtPdy.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtRawBalAmtPdy.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRawBalAmtPdy.Location = new System.Drawing.Point(425, 126);
		this.txtRawBalAmtPdy.Name = "txtRawBalAmtPdy";
		this.txtRawBalAmtPdy.Size = new System.Drawing.Size(93, 22);
		this.txtRawBalAmtPdy.TabIndex = 31;
		this.txtRawBalAmtPdy.Text = " 0";
		this.txtRawBalAmtPdy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtRawOutAmtPdy.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtRawOutAmtPdy.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRawOutAmtPdy.Location = new System.Drawing.Point(326, 126);
		this.txtRawOutAmtPdy.Name = "txtRawOutAmtPdy";
		this.txtRawOutAmtPdy.Size = new System.Drawing.Size(93, 22);
		this.txtRawOutAmtPdy.TabIndex = 30;
		this.txtRawOutAmtPdy.Text = " 0";
		this.txtRawOutAmtPdy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtRawInAmtPdy.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtRawInAmtPdy.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRawInAmtPdy.Location = new System.Drawing.Point(227, 126);
		this.txtRawInAmtPdy.Name = "txtRawInAmtPdy";
		this.txtRawInAmtPdy.Size = new System.Drawing.Size(93, 22);
		this.txtRawInAmtPdy.TabIndex = 29;
		this.txtRawInAmtPdy.Text = " 0";
		this.txtRawInAmtPdy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtRawBalQtyPdy.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtRawBalQtyPdy.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRawBalQtyPdy.Location = new System.Drawing.Point(425, 66);
		this.txtRawBalQtyPdy.Name = "txtRawBalQtyPdy";
		this.txtRawBalQtyPdy.Size = new System.Drawing.Size(93, 22);
		this.txtRawBalQtyPdy.TabIndex = 31;
		this.txtRawBalQtyPdy.Text = " 0";
		this.txtRawBalQtyPdy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtRawOpAmtPdy.AutoEllipsis = true;
		this.txtRawOpAmtPdy.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtRawOpAmtPdy.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRawOpAmtPdy.Location = new System.Drawing.Point(115, 126);
		this.txtRawOpAmtPdy.Name = "txtRawOpAmtPdy";
		this.txtRawOpAmtPdy.Size = new System.Drawing.Size(106, 22);
		this.txtRawOpAmtPdy.TabIndex = 28;
		this.txtRawOpAmtPdy.Text = " 0";
		this.txtRawOpAmtPdy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtRawOutQtyPdy.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtRawOutQtyPdy.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRawOutQtyPdy.Location = new System.Drawing.Point(326, 66);
		this.txtRawOutQtyPdy.Name = "txtRawOutQtyPdy";
		this.txtRawOutQtyPdy.Size = new System.Drawing.Size(93, 22);
		this.txtRawOutQtyPdy.TabIndex = 30;
		this.txtRawOutQtyPdy.Text = " 0";
		this.txtRawOutQtyPdy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtRawInQtyPdy.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtRawInQtyPdy.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRawInQtyPdy.Location = new System.Drawing.Point(227, 66);
		this.txtRawInQtyPdy.Name = "txtRawInQtyPdy";
		this.txtRawInQtyPdy.Size = new System.Drawing.Size(93, 22);
		this.txtRawInQtyPdy.TabIndex = 29;
		this.txtRawInQtyPdy.Text = " 0";
		this.txtRawInQtyPdy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtRawOpQtyPdy.AutoEllipsis = true;
		this.txtRawOpQtyPdy.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtRawOpQtyPdy.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRawOpQtyPdy.Location = new System.Drawing.Point(115, 66);
		this.txtRawOpQtyPdy.Name = "txtRawOpQtyPdy";
		this.txtRawOpQtyPdy.Size = new System.Drawing.Size(106, 22);
		this.txtRawOpQtyPdy.TabIndex = 28;
		this.txtRawOpQtyPdy.Text = " 0";
		this.txtRawOpQtyPdy.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.pnlPendingInwardGatePass.BackColor = System.Drawing.SystemColors.ButtonHighlight;
		this.pnlPendingInwardGatePass.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pnlPendingInwardGatePass.Controls.Add(this.txtRawBalWeight);
		this.pnlPendingInwardGatePass.Controls.Add(this.txtRawOutWeight);
		this.pnlPendingInwardGatePass.Controls.Add(this.txtRawInWeight);
		this.pnlPendingInwardGatePass.Controls.Add(this.txtRawOpWeight);
		this.pnlPendingInwardGatePass.Controls.Add(this.linkLabel3);
		this.pnlPendingInwardGatePass.Controls.Add(this.txtRawBalAmount);
		this.pnlPendingInwardGatePass.Controls.Add(this.txtRawOutAmount);
		this.pnlPendingInwardGatePass.Controls.Add(this.txtRawInAmount);
		this.pnlPendingInwardGatePass.Controls.Add(this.txtRawBalQty);
		this.pnlPendingInwardGatePass.Controls.Add(this.txtRawOpAmount);
		this.pnlPendingInwardGatePass.Controls.Add(this.txtRawOutQty);
		this.pnlPendingInwardGatePass.Controls.Add(this.txtRawInQty);
		this.pnlPendingInwardGatePass.Controls.Add(this.txtRawOpQty);
		this.pnlPendingInwardGatePass.Cursor = System.Windows.Forms.Cursors.Hand;
		this.pnlPendingInwardGatePass.Location = new System.Drawing.Point(13, 64);
		this.pnlPendingInwardGatePass.Name = "pnlPendingInwardGatePass";
		this.pnlPendingInwardGatePass.Size = new System.Drawing.Size(521, 153);
		this.pnlPendingInwardGatePass.TabIndex = 0;
		this.txtRawBalWeight.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtRawBalWeight.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRawBalWeight.Location = new System.Drawing.Point(422, 96);
		this.txtRawBalWeight.Name = "txtRawBalWeight";
		this.txtRawBalWeight.Size = new System.Drawing.Size(93, 22);
		this.txtRawBalWeight.TabIndex = 40;
		this.txtRawBalWeight.Text = " 0";
		this.txtRawBalWeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtRawOutWeight.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtRawOutWeight.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRawOutWeight.Location = new System.Drawing.Point(319, 96);
		this.txtRawOutWeight.Name = "txtRawOutWeight";
		this.txtRawOutWeight.Size = new System.Drawing.Size(93, 22);
		this.txtRawOutWeight.TabIndex = 39;
		this.txtRawOutWeight.Text = " 0";
		this.txtRawOutWeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtRawInWeight.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtRawInWeight.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRawInWeight.Location = new System.Drawing.Point(216, 96);
		this.txtRawInWeight.Name = "txtRawInWeight";
		this.txtRawInWeight.Size = new System.Drawing.Size(93, 22);
		this.txtRawInWeight.TabIndex = 38;
		this.txtRawInWeight.Text = " 0";
		this.txtRawInWeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtRawOpWeight.AutoEllipsis = true;
		this.txtRawOpWeight.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtRawOpWeight.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRawOpWeight.Location = new System.Drawing.Point(109, 96);
		this.txtRawOpWeight.Name = "txtRawOpWeight";
		this.txtRawOpWeight.Size = new System.Drawing.Size(98, 22);
		this.txtRawOpWeight.TabIndex = 37;
		this.txtRawOpWeight.Text = " 0";
		this.txtRawOpWeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.linkLabel3.BackColor = System.Drawing.Color.Teal;
		this.linkLabel3.Cursor = System.Windows.Forms.Cursors.Hand;
		this.linkLabel3.Font = new System.Drawing.Font("Calibri", 18f, System.Drawing.FontStyle.Bold);
		this.linkLabel3.ForeColor = System.Drawing.Color.DarkGray;
		this.linkLabel3.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
		this.linkLabel3.LinkColor = System.Drawing.Color.White;
		this.linkLabel3.Location = new System.Drawing.Point(0, -1);
		this.linkLabel3.Name = "linkLabel3";
		this.linkLabel3.Size = new System.Drawing.Size(520, 36);
		this.linkLabel3.TabIndex = 0;
		this.linkLabel3.TabStop = true;
		this.linkLabel3.Text = "Raw Material Rice";
		this.linkLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel3_LinkClicked);
		this.txtRawBalAmount.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtRawBalAmount.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRawBalAmount.Location = new System.Drawing.Point(422, 126);
		this.txtRawBalAmount.Name = "txtRawBalAmount";
		this.txtRawBalAmount.Size = new System.Drawing.Size(93, 22);
		this.txtRawBalAmount.TabIndex = 31;
		this.txtRawBalAmount.Text = " 0";
		this.txtRawBalAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtRawOutAmount.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtRawOutAmount.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRawOutAmount.Location = new System.Drawing.Point(319, 126);
		this.txtRawOutAmount.Name = "txtRawOutAmount";
		this.txtRawOutAmount.Size = new System.Drawing.Size(93, 22);
		this.txtRawOutAmount.TabIndex = 30;
		this.txtRawOutAmount.Text = " 0";
		this.txtRawOutAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtRawInAmount.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtRawInAmount.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRawInAmount.Location = new System.Drawing.Point(216, 126);
		this.txtRawInAmount.Name = "txtRawInAmount";
		this.txtRawInAmount.Size = new System.Drawing.Size(93, 22);
		this.txtRawInAmount.TabIndex = 29;
		this.txtRawInAmount.Text = " 0";
		this.txtRawInAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtRawBalQty.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtRawBalQty.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRawBalQty.Location = new System.Drawing.Point(422, 66);
		this.txtRawBalQty.Name = "txtRawBalQty";
		this.txtRawBalQty.Size = new System.Drawing.Size(93, 22);
		this.txtRawBalQty.TabIndex = 31;
		this.txtRawBalQty.Text = " 0";
		this.txtRawBalQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtRawOpAmount.AutoEllipsis = true;
		this.txtRawOpAmount.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtRawOpAmount.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRawOpAmount.Location = new System.Drawing.Point(109, 126);
		this.txtRawOpAmount.Name = "txtRawOpAmount";
		this.txtRawOpAmount.Size = new System.Drawing.Size(98, 22);
		this.txtRawOpAmount.TabIndex = 28;
		this.txtRawOpAmount.Text = " 0";
		this.txtRawOpAmount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtRawOutQty.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtRawOutQty.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRawOutQty.Location = new System.Drawing.Point(319, 66);
		this.txtRawOutQty.Name = "txtRawOutQty";
		this.txtRawOutQty.Size = new System.Drawing.Size(93, 22);
		this.txtRawOutQty.TabIndex = 30;
		this.txtRawOutQty.Text = " 0";
		this.txtRawOutQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtRawInQty.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtRawInQty.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRawInQty.Location = new System.Drawing.Point(216, 66);
		this.txtRawInQty.Name = "txtRawInQty";
		this.txtRawInQty.Size = new System.Drawing.Size(93, 22);
		this.txtRawInQty.TabIndex = 29;
		this.txtRawInQty.Text = " 0";
		this.txtRawInQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtRawOpQty.AutoEllipsis = true;
		this.txtRawOpQty.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.txtRawOpQty.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRawOpQty.Location = new System.Drawing.Point(109, 66);
		this.txtRawOpQty.Name = "txtRawOpQty";
		this.txtRawOpQty.Size = new System.Drawing.Size(98, 22);
		this.txtRawOpQty.TabIndex = 28;
		this.txtRawOpQty.Text = " 0";
		this.txtRawOpQty.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Font = new System.Drawing.Font("Calibri", 26.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel2.ForeColor = System.Drawing.Color.White;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1094, 58);
		this.panel2.TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Calibri", 24f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(300, 9);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(558, 39);
		this.label1.TabIndex = 0;
		this.label1.Text = "Item Class Group Wise Stocks DashBoard";
		this.CmbBranches.AllowNull = (DefaultableBoolean)1;
		this.CmbBranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbBranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance39;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbBranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance44).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbBranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranches).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbBranches).Location = new System.Drawing.Point(303, 30);
		((System.Windows.Forms.Control)(object)this.CmbBranches).Name = "CmbBranches";
		((System.Windows.Forms.Control)(object)this.CmbBranches).Size = new System.Drawing.Size(227, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranches).TabIndex = 593;
		this.Branches.AutoSize = true;
		this.Branches.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Branches.ForeColor = System.Drawing.Color.Black;
		this.Branches.Location = new System.Drawing.Point(305, 6);
		this.Branches.Name = "Branches";
		this.Branches.Size = new System.Drawing.Size(55, 15);
		this.Branches.TabIndex = 594;
		this.Branches.Text = "Branches";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1094, 683);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmStockDashboard";
		this.Text = "Stock DashBoard";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmStockDashboard_FormClosing);
		base.Load += new System.EventHandler(frmPendingWorksRpt_Load);
		this.panel1.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel19.ResumeLayout(false);
		this.panel19.PerformLayout();
		this.panel20.ResumeLayout(false);
		this.panel16.ResumeLayout(false);
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		this.panel18.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.panel15.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel11.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.pnlPendingInwardGatePass.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranches).EndInit();
		base.ResumeLayout(false);
	}
}
