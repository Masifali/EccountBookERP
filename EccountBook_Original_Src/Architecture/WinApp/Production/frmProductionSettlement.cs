using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Production;
using Architecture.BLL.Reports.Production;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Common;
using Architecture.Model.Production;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Inventory_Reports;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.Production;

public class frmProductionSettlement : Form
{
	public class IdEventArgs : EventArgs
	{
		public int Id { get; }

		public IdEventArgs(int id)
		{
			Id = id;
		}
	}

	private double AvgRate;

	private double AvgRateWithoutExp;

	private bool UpdateMode;

	private bool FIFOCGSFlag;

	private bool SaleCostingJobOrderWise;

	private bool JobOrderCreatewithoutRates;

	public bool OpenInProductionTab;

	public int JobOrderId;

	public bool RequestedByOtherDocument;

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	internal GridEX grdOutPutSettlement;

	private Panel panel36;

	private Panel panel39;

	internal GridEX grdPackingMaterialSettlement;

	private Panel panel38;

	internal GridEX grdOverHeadSettlement;

	private Panel panel37;

	private Label label10;

	private Label label8;

	private Panel panel34;

	private Button btnAllocateExportInvoiceToJobOrder;

	private Label label171;

	private Label label7;

	private Button BtnByProductSettlementPopUp;

	internal GridEX grdInputsettlement;

	private Panel panel40;

	private Panel panel51;

	private Panel panel52;

	private Label label22;

	private Label label25;

	private Label label14;

	private Label label31;

	private Label label17;

	private Label label18;

	private Label label59;

	private TextBox txtItemQtyRM;

	private TextBox txtItemQtyBP;

	private TextBox txtBPOverHead;

	private TextBox txtNetWeightRM;

	private TextBox txtAmountFG;

	private TextBox txtBPTotalAmountWithExpenses;

	private TextBox txtNetWeightFG;

	private TextBox txtNetWeightBP;

	private TextBox txtAmountBP;

	private TextBox txtBPPMCharges;

	private Label label21;

	private TextBox txtAmountRM;

	private TextBox txtItemQtyFG;

	private Panel panelDifferenceAmounts;

	private TextBox txtDifferenceAmount;

	private Label label26;

	private Panel panel75;

	private Label label28;

	private TextBox txtFinishAmountAgainstJobOrder;

	private TextBox txtActualFinishGoodsAmount;

	private Label label29;

	private Label label51;

	private Panel panel55;

	private Panel panel56;

	private Label label58;

	private TextBox txtAvgRateFG;

	private Label label94;

	private TextBox txtFinishGoodRateWithOutExp;

	private Label label114;

	private Panel panel53;

	private Panel panel54;

	private Label label115;

	private TextBox txtOverHeadsSettlement;

	private TextBox txtPackingMaterialSettlement;

	private Label label116;

	private Label label117;

	private Panel panel49;

	private Label label5;

	private Panel panel32;

	private DateTimePicker txtSettlementDate;

	private CheckBox chkAllJobOrderOnSettlement;

	private Button btnSettlementVoucher;

	internal UltraCombo CmbDifferenceAccountTitle;

	private Label label113;

	private Button BtnPrint615A;

	private Button btn615Report;

	private Button btnApproveSettlement;

	private Button BtnPrint613B;

	private Button BtnPrint613A;

	private Button btn613SummaryReport;

	private Button btnRecoveryReport;

	private Button btnUpdateSettlement;

	private Button btnApplyAvgRateFG;

	private Button btnResetSettlement;

	private Button btnGeneralSettlement;

	internal UltraCombo CmbJobOrderSettlement;

	private Label label169;

	private Label label12;

	private ToolStrip miniToolStrip;

	internal UltraCombo CmbBrandNamePackingMaterial;

	internal UltraCombo CmbBrandUomPackingMaterial;

	internal UltraCombo cmbItemPackingMaterial;

	internal UltraCombo CmbWarehousePackingMaterial;

	internal UltraCombo CmbPlantFeederPackingMaterial;

	private CheckBox chkIncludeWages;

	public event EventHandler<IdEventArgs> PackingMaterialReadById;

	public event EventHandler<IdEventArgs> OverHeadReadById;

	public frmProductionSettlement(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmFoodProduction_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1280, 820);
			JobOrderCreatewithoutRates = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("JobOrderCreatewithoutRates"));
			SaleCostingJobOrderWise = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("SaleCostingJobOrderWise"));
			BtnByProductSettlementPopUp.Visible = JobOrderCreatewithoutRates || SaleCostingJobOrderWise;
			label171.Visible = JobOrderCreatewithoutRates || SaleCostingJobOrderWise;
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			FIFOCGSFlag = CommonServices.GetERPFeatureById(5);
			if (FIFOCGSFlag)
			{
				((Control)(object)CmbDifferenceAccountTitle).Visible = true;
			}
			else
			{
				((Control)(object)CmbDifferenceAccountTitle).Visible = false;
			}
			CmbJobOrderSettlement.Focus();
			stockAccoutnFill();
			JobOrderNoFillForSettlement(1);
			if (RequestedByOtherDocument && JobOrderId > 0)
			{
				CmbJobOrderSettlement.Value = JobOrderId;
				CmbJobOrderSettlement_Leave(null, null);
				btnGeneralSettlement_Click(null, null);
				btnApplyAvgRateFG_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void stockAccoutnFill()
	{
		try
		{
			DataTable dtStockAc = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("4,12");
			if (dtStockAc != null && dtStockAc.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtStockAc, CmbDifferenceAccountTitle, "Id", "AccountTitle", "Difference Account", false);
				((UltraGridBase)CmbDifferenceAccountTitle).DisplayLayout.Bands[0].Columns[1].Width = 450;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JobOrderNoFillForSettlement(int ActionId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		try
		{
			DataTable dtPJO = InvProductionJobOrder.GetJobOrderAll(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				ActionId = ActionId
			});
			if (dtPJO.Rows.Count > 0)
			{
				DDL.BindDDL(dtPJO, CmbJobOrderSettlement, "Id", "PlanCode", "JobOrderNo", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmFoodProduction_KeyDown(object sender, KeyEventArgs e)
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
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VoucherReprot(int Id)
	{
		try
		{
			CommonServices.VoucherReport_118(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnGeneralSettlement_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0c6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c73: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c84: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c95: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cbb: Expected O, but got Unknown
		//IL_1033: Unknown result type (might be due to invalid IL or missing references)
		//IL_1038: Unknown result type (might be due to invalid IL or missing references)
		//IL_1049: Unknown result type (might be due to invalid IL or missing references)
		//IL_105a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1070: Unknown result type (might be due to invalid IL or missing references)
		//IL_1080: Expected O, but got Unknown
		try
		{
			DataTable dtSettlement = InvFoodProductionReports.InvFoodProductionSettlement(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				JobOrderId = Conversion.ToInt(CmbJobOrderSettlement.Value),
				Activity = "InPutOutPut"
			});
			if (dtSettlement.Rows.Count > 0)
			{
				DataTable dtInput = new DataTable();
				dtInput.Columns.Add("DetailId", typeof(int));
				dtInput.Columns.Add("Id", typeof(int));
				dtInput.Columns.Add("RefDocumentTypeId", typeof(int));
				dtInput.Columns.Add("RefDocIdNo", typeof(int));
				dtInput.Columns.Add("RefDocSubIdNo", typeof(int));
				dtInput.Columns.Add("WIPAccountId", typeof(int));
				dtInput.Columns.Add("EntryType");
				dtInput.Columns.Add("DocDate", typeof(DateTime));
				dtInput.Columns.Add("DocNo", typeof(int));
				dtInput.Columns.Add("WareHouseId", typeof(int));
				dtInput.Columns.Add("WareHouse");
				dtInput.Columns.Add("ItemId", typeof(int));
				dtInput.Columns.Add("ItemName");
				dtInput.Columns.Add("ItemUOMId", typeof(int));
				dtInput.Columns.Add("ItemUOM");
				dtInput.Columns.Add("CropYear");
				dtInput.Columns.Add("JobLotId", typeof(int));
				dtInput.Columns.Add("JobLot");
				dtInput.Columns.Add("PackingTypeId", typeof(int));
				dtInput.Columns.Add("PackingType");
				dtInput.Columns.Add("Quantity", typeof(double));
				dtInput.Columns.Add("Weight", typeof(double));
				dtInput.Columns.Add("Rate", typeof(double));
				dtInput.Columns.Add("RateUOMId", typeof(int));
				dtInput.Columns.Add("RateUOM");
				dtInput.Columns.Add("ItemAmount", typeof(double));
				dtInput.Columns.Add("Remarks");
				dtInput.Columns.Add("WipAccount");
				dtInput.Columns.Add("RefJobOrderId", typeof(int));
				dtInput.Columns.Add("RefJobOrder", typeof(string));
				dtInput.Columns.Add("RefJobStatus", typeof(string));
				dtInput.Columns.Add("InvoiceNo", typeof(int));
				dtInput.Columns.Add("InvoiceApproved", typeof(int));
				dtInput.Columns.Add("InvoiceApprovedStatus", typeof(string));
				DataTable dtOutPut = new DataTable();
				dtOutPut.Columns.Add("Id");
				dtOutPut.Columns.Add("DetailId");
				dtOutPut.Columns.Add("WIPAccountId");
				dtOutPut.Columns.Add("EntryType");
				dtOutPut.Columns.Add("DocDate");
				dtOutPut.Columns.Add("DocNo");
				dtOutPut.Columns.Add("WareHouseId");
				dtOutPut.Columns.Add("WareHouse");
				dtOutPut.Columns.Add("ItemId");
				dtOutPut.Columns.Add("ItemName");
				dtOutPut.Columns.Add("ItemUOMId");
				dtOutPut.Columns.Add("ItemUOM");
				dtOutPut.Columns.Add("CropYear");
				dtOutPut.Columns.Add("JobLotId");
				dtOutPut.Columns.Add("JobLot");
				dtOutPut.Columns.Add("PackingTypeId");
				dtOutPut.Columns.Add("PackingType");
				dtOutPut.Columns.Add("Quantity", typeof(double));
				dtOutPut.Columns.Add("Weight", typeof(double));
				dtOutPut.Columns.Add("Rate", typeof(double));
				dtOutPut.Columns.Add("ItemAmount", typeof(double));
				dtOutPut.Columns.Add("RateWithoutExp", typeof(double));
				dtOutPut.Columns.Add("AmountWithoutExp", typeof(double));
				dtOutPut.Columns.Add("NetRate", typeof(double));
				dtOutPut.Columns.Add("RateUOMId");
				dtOutPut.Columns.Add("RateUOM");
				dtOutPut.Columns.Add("ItemPmCost", typeof(double));
				dtOutPut.Columns.Add("GeneralPmCost", typeof(double));
				dtOutPut.Columns.Add("ItemOhCost", typeof(double));
				dtOutPut.Columns.Add("GeneralOhCost", typeof(double));
				dtOutPut.Columns.Add("TotalAmount", typeof(double));
				dtOutPut.Columns.Add("Remarks");
				dtOutPut.Columns.Add("Status");
				dtOutPut.Columns.Add("ProductionApprovalStatus");
				dtInput.BeginLoadData();
				dtOutPut.BeginLoadData();
				dtInput.Rows.Clear();
				foreach (DataRow row in dtSettlement.Rows)
				{
					double qty = Conversion.ToDouble(row["Qty"]);
					double weight = Conversion.ToDouble(row["Weight"]);
					double rate = Conversion.ToDouble(row["Rate"]);
					double amount = Conversion.ToDouble(row["Amount"]);
					string entryType = Conversion.ToString(row["EntryType"]);
					string entryTypeDetail = Conversion.ToString(row["EntryTypeDetail"]);
					if (entryType == "Input" || entryTypeDetail == "ReturnToGodown")
					{
						if (entryTypeDetail == "ReturnToGodown")
						{
							qty = 0.0 - qty;
							weight = 0.0 - weight;
							amount = 0.0 - amount;
						}
						dtInput.Rows.Add(row["DetailId"], row["Id"], row["RefDocumentTypeId"], row["RefDocNoId"], row["RefDocSubIdNo"], row["WIPAccountId"], row["EntryTypeDetail"], Conversion.ToDateTime(row["DocDate"]).ToString("dd-MMM-yy"), row["DocCode"], row["WarehouseId"], row["WareHouseName"], row["ItemId"], row["ItemName"], row["ItemUomId"], row["ItemUom"], row["CropBatch"], row["JobLotId"], row["JobLotDescription"], row["PackingtypeId"], row["PackTypeDesc"], qty, weight, rate, row["RateUOMId"], row["RateUom"], amount, row["Remarks"], row["AccountTitle"], row["RefJobOrderId"], row["RefJobOrder"], row["RefJobStatus"], row["InvoiceNo"], row["InvoiceApproved"], row["InvoiceApprovedStatus"]);
					}
					if (entryType == "Output" && entryTypeDetail != "ReturnToGodown")
					{
						dtOutPut.Rows.Add(row["Id"], row["DetailId"], row["WIPAccountId"], row["EntryTypeDetail"], Conversion.ToDateTime(row["DocDate"]).ToString("dd-MMM-yy"), row["DocCode"], row["WarehouseId"], row["WareHouseName"], row["ItemId"], row["ItemName"], row["ItemUomId"], row["ItemUom"], row["CropBatch"], row["JobLotId"], row["JobLotDescription"], row["PackingtypeId"], row["PackTypeDesc"], row["Qty"], row["Weight"], row["Rate"], row["Amount"], row["Rate"], row["Amount"], row["NetRate"], row["RateUOMId"], row["RateUom"], row["ItemPmCost"], row["GeneralPmCost"], row["ItemOhCost"], row["GeneralOhCost"], row["TotalAmount"], row["Remarks"], row["Status"], row["ProductionApprovalStatus"]);
					}
				}
				dtInput.EndLoadData();
				dtOutPut.EndLoadData();
				grdInputsettlement.DataSource = dtInput;
				grdInputsettlement.RetrieveStructure();
				grdInputSettelmentSetting();
				grdOutPutSettlement.DataSource = dtOutPut;
				grdOutPutSettlement.RetrieveStructure();
				grdOutPutSettelmentSetting();
			}
			else
			{
				grdInputsettlement.DataSource = null;
				grdOutPutSettlement.DataSource = null;
			}
			DataTable dtOverHeads = InvFoodProductionReports.InvFoodProductionSettlement(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				JobOrderId = Conversion.ToInt(CmbJobOrderSettlement.Value),
				Activity = "OverHeads"
			});
			if (dtOverHeads != null && dtOverHeads.Rows.Count > 0)
			{
				DataTable dtHead = new DataTable();
				dtHead.Columns.Add("Id", typeof(int));
				dtHead.Columns.Add("CharOfAccountId", typeof(int));
				dtHead.Columns.Add("DocumentTypeId", typeof(int));
				dtHead.Columns.Add("TransRef", typeof(string));
				dtHead.Columns.Add("DocDate", typeof(DateTime));
				dtHead.Columns.Add("DocNo", typeof(int));
				dtHead.Columns.Add("AccountTitle", typeof(string));
				dtHead.Columns.Add("ItemQty", typeof(double));
				dtHead.Columns.Add("ItemRate", typeof(double));
				dtHead.Columns.Add("Amount", typeof(double));
				dtHead.Columns.Add("BrandId", typeof(int));
				dtHead.Columns.Add("BrandName", typeof(string));
				dtHead.Columns.Add("BrandUomId", typeof(int));
				dtHead.Columns.Add("BrandUom", typeof(double));
				for (int j = 0; j < dtOverHeads.Rows.Count; j++)
				{
					dtHead.Rows.Add(dtOverHeads.Rows[j]["Id"], dtOverHeads.Rows[j]["CharOfAccountId"], dtOverHeads.Rows[j]["DocumentTypeId"], dtOverHeads.Rows[j]["TransRef"], dtOverHeads.Rows[j]["DocDate"], dtOverHeads.Rows[j]["DocNo"], dtOverHeads.Rows[j]["AccountTitle"], dtOverHeads.Rows[j]["ItemQty"], dtOverHeads.Rows[j]["ItemRate"], dtOverHeads.Rows[j]["Amount"], dtOverHeads.Rows[j]["BrandId"], dtOverHeads.Rows[j]["BrandName"], dtOverHeads.Rows[j]["BrandUomId"], dtOverHeads.Rows[j]["BrandUom"]);
				}
				grdOverHeadSettlement.DataSource = dtHead;
				grdOverHeadSettlement.RetrieveStructure();
				grdOverHeadsSettelmentSetting();
			}
			else
			{
				grdOverHeadSettlement.DataSource = null;
			}
			DataTable dtPack = InvFoodProductionReports.InvFoodProductionSettlement(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				JobOrderId = Conversion.ToInt(CmbJobOrderSettlement.Value),
				Activity = "PackingMaterial"
			});
			if (dtPack != null && dtPack.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("ItemId", typeof(int));
				dt.Columns.Add("itemUomSchId", typeof(int));
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("ItemUom", typeof(string));
				dt.Columns.Add("BrandId", typeof(int));
				dt.Columns.Add("BrandUomId", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("Qty", typeof(double));
				dt.Columns.Add("Rate", typeof(double));
				dt.Columns.Add("Amount", typeof(double));
				dt.Columns.Add("Remarks", typeof(string));
				dt.Columns.Add("BrandName", typeof(string));
				dt.Columns.Add("BrandUom", typeof(double));
				for (int i = 0; i < dtPack.Rows.Count; i++)
				{
					dt.Rows.Add(dtPack.Rows[i]["Id"], dtPack.Rows[i]["ItemId"], dtPack.Rows[i]["itemUomSchId"], dtPack.Rows[i]["DocumentTypeId"], dtPack.Rows[i]["ItemUom"], dtPack.Rows[i]["BrandId"], dtPack.Rows[i]["BrandUomId"], dtPack.Rows[i]["DocDate"], dtPack.Rows[i]["DocNo"], dtPack.Rows[i]["ItemName"], dtPack.Rows[i]["Qty"], dtPack.Rows[i]["Rate"], dtPack.Rows[i]["Amount"], dtPack.Rows[i]["Remarks"], dtPack.Rows[i]["BrandName"], dtPack.Rows[i]["BrandUom"]);
				}
				grdPackingMaterialSettlement.DataSource = dt;
				grdPackingMaterialSettlement.RetrieveStructure();
				grdPackingMaterialSettelmentSetting();
			}
			else
			{
				grdPackingMaterialSettlement.DataSource = null;
			}
			CalcualteFinishGoodsRateAndAmounts();
			TotalActualFinishAndJobOrderAmountsCalculate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalActualFinishAndJobOrderAmountsCalculate()
	{
		try
		{
			double JobOrderAmount = 0.0;
			double FinishGoodsAmount = 0.0;
			GridEXRow[] rows = grdOutPutSettlement.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (r.Cells["EntryType"].Value.ToString() == "FinishGoods" && r.Cells["Status"].Value.ToString() == "Used")
				{
					JobOrderAmount += Conversion.ToDouble(r.Cells["ItemAmount"].Value);
					FinishGoodsAmount += Conversion.ToDouble(r.Cells["TotalAmount"].Value);
				}
			}
			txtActualFinishGoodsAmount.Text = Math.Round(FinishGoodsAmount, 0, MidpointRounding.AwayFromZero).ToString("0,0");
			txtFinishAmountAgainstJobOrder.Text = Math.Round(JobOrderAmount, 0, MidpointRounding.AwayFromZero).ToString("0,0");
			double DifferanceAmount = FinishGoodsAmount - JobOrderAmount;
			txtDifferenceAmount.Text = Math.Round(DifferanceAmount, 0, MidpointRounding.AwayFromZero).ToString("0,0");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInputSettelmentSetting()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			foreach (GridEXColumn item in (JanusCollectionBase)grdInputsettlement.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdInputsettlement.RootTable.HeaderLines = 2;
			grdInputsettlement.RootTable.Columns["WipAccount"].Caption = "Wip/Lot Account";
			grdInputsettlement.RootTable.Columns["EntryType"].Width = 55;
			grdInputsettlement.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grdInputsettlement.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdInputsettlement.RootTable.Columns["WareHouse"].Width = 120;
			grdInputsettlement.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdInputsettlement.RootTable.Columns["ItemUOM"].Width = 50;
			grdInputsettlement.RootTable.Columns["CropYear"].Width = 60;
			grdInputsettlement.RootTable.Columns["JobLot"].Width = Constants.InventoryConstants.JobLotConstant;
			grdInputsettlement.RootTable.Columns["PackingType"].Width = 70;
			grdInputsettlement.RootTable.Columns["Quantity"].Width = Constants.InventoryConstants.QtyConstant;
			grdInputsettlement.RootTable.Columns["Weight"].Width = Constants.InventoryConstants.WeightConstant;
			grdInputsettlement.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdInputsettlement.RootTable.Columns["RateUOM"].Width = 50;
			grdInputsettlement.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdInputsettlement.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdInputsettlement.RootTable.Columns["WipAccount"].Width = Constants.InventoryConstants.RemarksConstants;
			grdInputsettlement.RootTable.Columns["RefJobOrder"].Width = 70;
			grdInputsettlement.RootTable.Columns["RefJobStatus"].Width = 70;
			grdInputsettlement.RootTable.Columns["InvoiceNo"].Width = 70;
			grdInputsettlement.RootTable.Columns["InvoiceApprovedStatus"].Width = 70;
			grdInputsettlement.RootTable.Columns["DetailId"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["Id"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["RefDocumentTypeId"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["RefDocIdNo"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["RefDocSubIdNo"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["WIPAccountId"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["EntryType"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["DocDate"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["DocNo"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["WareHouseId"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["WareHouse"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["ItemUOMId"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["ItemUOM"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["CropYear"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["JobLotId"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["JobLot"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["PackingTypeId"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["PackingType"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["Quantity"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["Weight"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["Rate"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["RateUOMId"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["RateUOM"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["ItemAmount"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["Remarks"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["WipAccount"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["RefJobOrderId"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["RefJobOrder"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["RefJobStatus"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["InvoiceNo"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["InvoiceApproved"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["InvoiceApprovedStatus"].EditType = (EditType)0;
			grdInputsettlement.RootTable.Columns["Id"].Visible = false;
			grdInputsettlement.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grdInputsettlement.RootTable.Columns["RefDocIdNo"].Visible = false;
			grdInputsettlement.RootTable.Columns["RefDocSubIdNo"].Visible = false;
			grdInputsettlement.RootTable.Columns["ItemId"].Visible = false;
			grdInputsettlement.RootTable.Columns["JobLotId"].Visible = false;
			grdInputsettlement.RootTable.Columns["WarehouseId"].Visible = false;
			grdInputsettlement.RootTable.Columns["PackingTypeId"].Visible = false;
			grdInputsettlement.RootTable.Columns["ItemUOMId"].Visible = false;
			grdInputsettlement.RootTable.Columns["RateUOMId"].Visible = false;
			grdInputsettlement.RootTable.Columns["RefJobOrderId"].Visible = false;
			grdInputsettlement.RootTable.Columns["DetailId"].Visible = false;
			grdInputsettlement.RootTable.Columns["WIPAccountId"].Visible = false;
			grdInputsettlement.RootTable.Columns["InvoiceApproved"].Visible = false;
			grdInputsettlement.RootTable.Columns["RefJobOrder"].Visible = SaleCostingJobOrderWise;
			grdInputsettlement.RootTable.Columns["RefJobStatus"].Visible = SaleCostingJobOrderWise;
			grdInputsettlement.RootTable.Columns["InvoiceNo"].Visible = SaleCostingJobOrderWise;
			grdInputsettlement.RootTable.Columns["InvoiceApprovedStatus"].Visible = SaleCostingJobOrderWise;
			grdInputsettlement.RootTable.Columns["Quantity"].AggregateFunction = (AggregateFunction)2;
			grdInputsettlement.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdInputsettlement.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdInputsettlement.RootTable.Columns["Quantity"].FormatString = "#,#";
			grdInputsettlement.RootTable.Columns["Weight"].FormatString = "#,#";
			grdInputsettlement.RootTable.Columns["Rate"].FormatString = "#,#.##";
			grdInputsettlement.RootTable.Columns["ItemAmount"].FormatString = "#,#";
			grdInputsettlement.RootTable.Columns["Quantity"].TotalFormatString = "#,#";
			grdInputsettlement.RootTable.Columns["Weight"].TotalFormatString = "#,#";
			grdInputsettlement.RootTable.Columns["ItemAmount"].TotalFormatString = "#,#";
			grdInputsettlement.RootTable.Columns["Rate"].TotalFormatString = "#,#.##";
			grdInputsettlement.RootTable.Columns["Quantity"].TextAlignment = (TextAlignment)3;
			grdInputsettlement.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdInputsettlement.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOutPutSettelmentSetting()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			foreach (GridEXColumn item in (JanusCollectionBase)grdOutPutSettlement.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdOutPutSettlement.RootTable.HeaderLines = 2;
			grdOutPutSettlement.RootTable.Columns["EntryType"].Width = Constants.InventoryConstants.DiscountType;
			grdOutPutSettlement.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grdOutPutSettlement.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdOutPutSettlement.RootTable.Columns["WareHouse"].Width = 120;
			grdOutPutSettlement.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdOutPutSettlement.RootTable.Columns["ItemUOM"].Width = 50;
			grdOutPutSettlement.RootTable.Columns["CropYear"].Width = 60;
			grdOutPutSettlement.RootTable.Columns["JobLot"].Width = Constants.InventoryConstants.JobLotConstant;
			grdOutPutSettlement.RootTable.Columns["PackingType"].Width = 70;
			grdOutPutSettlement.RootTable.Columns["Quantity"].Width = Constants.InventoryConstants.QtyConstant;
			grdOutPutSettlement.RootTable.Columns["Weight"].Width = Constants.InventoryConstants.WeightConstant;
			grdOutPutSettlement.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant + 15;
			grdOutPutSettlement.RootTable.Columns["RateUOM"].Width = 50;
			grdOutPutSettlement.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdOutPutSettlement.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdOutPutSettlement.RootTable.Columns["ItemPmCost"].Width = Constants.InventoryConstants.ExRateConstant;
			grdOutPutSettlement.RootTable.Columns["GeneralPmCost"].Width = Constants.InventoryConstants.ExRateConstant;
			grdOutPutSettlement.RootTable.Columns["ItemOhCost"].Width = Constants.InventoryConstants.ExRateConstant;
			grdOutPutSettlement.RootTable.Columns["GeneralOhCost"].Width = Constants.InventoryConstants.ExRateConstant;
			grdOutPutSettlement.RootTable.Columns["NetRate"].Width = Constants.InventoryConstants.ExRateConstant + 15;
			grdOutPutSettlement.RootTable.Columns["TotalAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdOutPutSettlement.RootTable.Columns["RateWithoutExp"].Width = Constants.InventoryConstants.AmountConstant;
			grdOutPutSettlement.RootTable.Columns["AmountWithoutExp"].Width = Constants.InventoryConstants.AmountConstant;
			grdOutPutSettlement.RootTable.Columns["Id"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["WIPAccountId"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["DetailId"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["EntryType"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["DocDate"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["DocNo"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["WareHouseId"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["WareHouse"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["ItemUOMId"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["ItemUOM"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["CropYear"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["JobLotId"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["JobLot"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["PackingTypeId"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["PackingType"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["Quantity"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["Weight"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["Rate"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["RateWithoutExp"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["NetRate"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["RateUOMId"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["RateUOM"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["ItemAmount"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["Remarks"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["ItemPmCost"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["GeneralPmCost"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["ItemOhCost"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["GeneralOhCost"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["TotalAmount"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["AmountWithoutExp"].EditType = (EditType)0;
			grdOutPutSettlement.RootTable.Columns["Status"].EditType = (EditType)0;
			if (SaleCostingJobOrderWise && !FIFOCGSFlag)
			{
				grdOutPutSettlement.RootTable.Columns["Rate"].EditType = (EditType)1;
			}
			grdOutPutSettlement.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["EntryType"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)6;
			grdOutPutSettlement.RootTable.Columns["DocNo"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["WareHouseId"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["WareHouse"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["ItemId"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["ItemName"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["ItemUOMId"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["ItemUOM"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["CropYear"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["JobLotId"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["JobLot"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["PackingTypeId"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["PackingType"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["Quantity"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["Weight"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["Rate"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["RateUOMId"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["RateUOM"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["ItemAmount"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["Remarks"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["ItemPmCost"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["GeneralPmCost"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["ItemOhCost"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["GeneralOhCost"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["NetRate"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["TotalAmount"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["RateWithoutExp"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["AmountWithoutExp"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["Status"].FilterEditType = (FilterEditType)1;
			grdOutPutSettlement.RootTable.Columns["Id"].Visible = false;
			grdOutPutSettlement.RootTable.Columns["WIPAccountId"].Visible = false;
			grdOutPutSettlement.RootTable.Columns["DetailId"].Visible = false;
			grdOutPutSettlement.RootTable.Columns["WareHouseId"].Visible = false;
			grdOutPutSettlement.RootTable.Columns["ItemId"].Visible = false;
			grdOutPutSettlement.RootTable.Columns["ItemUOMId"].Visible = false;
			grdOutPutSettlement.RootTable.Columns["JobLotId"].Visible = false;
			grdOutPutSettlement.RootTable.Columns["PackingTypeId"].Visible = false;
			grdOutPutSettlement.RootTable.Columns["RateUOMId"].Visible = false;
			grdOutPutSettlement.RootTable.Columns["ProductionApprovalStatus"].Visible = false;
			grdOutPutSettlement.RootTable.Columns["Rate"].Caption = "Job Order Rate";
			grdOutPutSettlement.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdOutPutSettlement.RootTable.Columns["Quantity"].AggregateFunction = (AggregateFunction)2;
			grdOutPutSettlement.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdOutPutSettlement.RootTable.Columns["ItemPmCost"].AggregateFunction = (AggregateFunction)2;
			grdOutPutSettlement.RootTable.Columns["GeneralPmCost"].AggregateFunction = (AggregateFunction)2;
			grdOutPutSettlement.RootTable.Columns["ItemOhCost"].AggregateFunction = (AggregateFunction)2;
			grdOutPutSettlement.RootTable.Columns["GeneralOhCost"].AggregateFunction = (AggregateFunction)2;
			grdOutPutSettlement.RootTable.Columns["TotalAmount"].AggregateFunction = (AggregateFunction)2;
			grdOutPutSettlement.RootTable.Columns["AmountWithoutExp"].AggregateFunction = (AggregateFunction)2;
			grdOutPutSettlement.RootTable.Columns["Rate"].FormatString = "#,#.###";
			grdOutPutSettlement.RootTable.Columns["RateWithoutExp"].FormatString = "#,#";
			grdOutPutSettlement.RootTable.Columns["NetRate"].FormatString = "#,#.###";
			grdOutPutSettlement.RootTable.Columns["Weight"].FormatString = "#,#";
			grdOutPutSettlement.RootTable.Columns["Quantity"].FormatString = "#,#";
			grdOutPutSettlement.RootTable.Columns["ItemAmount"].FormatString = "#,##0.###";
			grdOutPutSettlement.RootTable.Columns["ItemPmCost"].FormatString = "#,#";
			grdOutPutSettlement.RootTable.Columns["GeneralPmCost"].FormatString = "#,#";
			grdOutPutSettlement.RootTable.Columns["ItemOhCost"].FormatString = "#,#";
			grdOutPutSettlement.RootTable.Columns["GeneralOhCost"].FormatString = "#,#";
			grdOutPutSettlement.RootTable.Columns["TotalAmount"].FormatString = "#,##0.###";
			grdOutPutSettlement.RootTable.Columns["AmountWithoutExp"].FormatString = "#,##0.###";
			grdOutPutSettlement.RootTable.Columns["Rate"].TotalFormatString = "#,#.###";
			grdOutPutSettlement.RootTable.Columns["RateWithoutExp"].TotalFormatString = "#,#.###";
			grdOutPutSettlement.RootTable.Columns["NetRate"].TotalFormatString = "#,#.###";
			grdOutPutSettlement.RootTable.Columns["Weight"].TotalFormatString = "#,#";
			grdOutPutSettlement.RootTable.Columns["Quantity"].TotalFormatString = "#,#";
			grdOutPutSettlement.RootTable.Columns["ItemAmount"].TotalFormatString = "#,##0.###";
			grdOutPutSettlement.RootTable.Columns["ItemPmCost"].TotalFormatString = "#,#";
			grdOutPutSettlement.RootTable.Columns["GeneralPmCost"].TotalFormatString = "#,#";
			grdOutPutSettlement.RootTable.Columns["ItemOhCost"].TotalFormatString = "#,#";
			grdOutPutSettlement.RootTable.Columns["GeneralOhCost"].TotalFormatString = "#,#";
			grdOutPutSettlement.RootTable.Columns["TotalAmount"].TotalFormatString = "#,##0.###";
			grdOutPutSettlement.RootTable.Columns["AmountWithoutExp"].TotalFormatString = "#,##0.###";
			grdOutPutSettlement.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdOutPutSettlement.RootTable.Columns["Quantity"].TextAlignment = (TextAlignment)3;
			grdOutPutSettlement.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdOutPutSettlement.RootTable.Columns["ItemPmCost"].TextAlignment = (TextAlignment)3;
			grdOutPutSettlement.RootTable.Columns["GeneralPmCost"].TextAlignment = (TextAlignment)3;
			grdOutPutSettlement.RootTable.Columns["ItemOhCost"].TextAlignment = (TextAlignment)3;
			grdOutPutSettlement.RootTable.Columns["GeneralOhCost"].TextAlignment = (TextAlignment)3;
			grdOutPutSettlement.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdOutPutSettlement.RootTable.Columns["NetRate"].TextAlignment = (TextAlignment)3;
			grdOutPutSettlement.RootTable.Columns["TotalAmount"].TextAlignment = (TextAlignment)3;
			grdOutPutSettlement.RootTable.Columns["RateWithoutExp"].TextAlignment = (TextAlignment)3;
			grdOutPutSettlement.RootTable.Columns["AmountWithoutExp"].TextAlignment = (TextAlignment)3;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOverHeadsSettelmentSetting()
	{
		try
		{
			grdOverHeadSettlement.RootTable.Columns["TransRef"].Width = 90;
			grdOverHeadSettlement.RootTable.Columns["DocDate"].Width = 80;
			grdOverHeadSettlement.RootTable.Columns["DocNo"].Width = 65;
			grdOverHeadSettlement.RootTable.Columns["AccountTitle"].Width = 120;
			grdOverHeadSettlement.RootTable.Columns["ItemQty"].Width = 70;
			grdOverHeadSettlement.RootTable.Columns["ItemRate"].Width = 70;
			grdOverHeadSettlement.RootTable.Columns["Amount"].Width = 90;
			grdOverHeadSettlement.RootTable.Columns["BrandName"].Width = 100;
			grdOverHeadSettlement.RootTable.Columns["BrandUom"].Width = 80;
			grdOverHeadSettlement.RootTable.Columns["Id"].Visible = false;
			grdOverHeadSettlement.RootTable.Columns["CharOfAccountId"].Visible = false;
			grdOverHeadSettlement.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdOverHeadSettlement.RootTable.Columns["BrandId"].Visible = false;
			grdOverHeadSettlement.RootTable.Columns["BrandUomId"].Visible = false;
			grdOverHeadSettlement.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdOverHeadSettlement.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdOverHeadSettlement.RootTable.Columns["ItemRate"].AggregateFunction = (AggregateFunction)3;
			grdOverHeadSettlement.RootTable.Columns["ItemQty"].FormatString = "#,#";
			grdOverHeadSettlement.RootTable.Columns["Amount"].FormatString = "#,#";
			grdOverHeadSettlement.RootTable.Columns["ItemRate"].FormatString = "#,#.##";
			grdOverHeadSettlement.RootTable.Columns["ItemQty"].TotalFormatString = "#,#";
			grdOverHeadSettlement.RootTable.Columns["Amount"].TotalFormatString = "#,#";
			grdOverHeadSettlement.RootTable.Columns["ItemRate"].TotalFormatString = "#,#.##";
			grdOverHeadSettlement.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdOverHeadSettlement.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdOverHeadSettlement.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdOverHeadSettlement.RootTable.Columns["DocNo"].ColumnType = (ColumnType)5;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackingMaterialSettelmentSetting()
	{
		try
		{
			grdPackingMaterialSettlement.RootTable.Columns["DocDate"].Width = 80;
			grdPackingMaterialSettlement.RootTable.Columns["DocNo"].Width = 60;
			grdPackingMaterialSettlement.RootTable.Columns["ItemName"].Width = 125;
			grdPackingMaterialSettlement.RootTable.Columns["Qty"].Width = 70;
			grdPackingMaterialSettlement.RootTable.Columns["Rate"].Width = 70;
			grdPackingMaterialSettlement.RootTable.Columns["Amount"].Width = 90;
			grdPackingMaterialSettlement.RootTable.Columns["Remarks"].Width = 130;
			grdPackingMaterialSettlement.RootTable.Columns["BrandName"].Width = 120;
			grdPackingMaterialSettlement.RootTable.Columns["BrandUom"].Width = 80;
			grdPackingMaterialSettlement.RootTable.Columns["Id"].Visible = false;
			grdPackingMaterialSettlement.RootTable.Columns["ItemId"].Visible = false;
			grdPackingMaterialSettlement.RootTable.Columns["itemUomSchId"].Visible = false;
			grdPackingMaterialSettlement.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdPackingMaterialSettlement.RootTable.Columns["ItemUom"].Visible = false;
			grdPackingMaterialSettlement.RootTable.Columns["BrandId"].Visible = false;
			grdPackingMaterialSettlement.RootTable.Columns["BrandUomId"].Visible = false;
			grdPackingMaterialSettlement.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdPackingMaterialSettlement.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdPackingMaterialSettlement.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)3;
			grdPackingMaterialSettlement.RootTable.Columns["Qty"].FormatString = "#,#";
			grdPackingMaterialSettlement.RootTable.Columns["Amount"].FormatString = "#,#";
			grdPackingMaterialSettlement.RootTable.Columns["Rate"].FormatString = "#,#.##";
			grdPackingMaterialSettlement.RootTable.Columns["Qty"].TotalFormatString = "#,#";
			grdPackingMaterialSettlement.RootTable.Columns["Amount"].TotalFormatString = "#,#";
			grdPackingMaterialSettlement.RootTable.Columns["Rate"].TotalFormatString = "#,#.##";
			grdPackingMaterialSettlement.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdPackingMaterialSettlement.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdPackingMaterialSettlement.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdPackingMaterialSettlement.RootTable.Columns["DocNo"].ColumnType = (ColumnType)5;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOverHeadSettlement_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdOverHeadSettlement.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			int Id = Conversion.ToInt(CmbJobOrderSettlement.Value);
			Conversion.ToInt(r.Cells["Id"].Value);
			switch (Conversion.ToInt(r.Cells["DocumentTypeId"].Value))
			{
			case 80:
			case 112:
			{
				frmEvaulationDetailWagesReports frmEvaulationDetailWagesReports = new frmEvaulationDetailWagesReports(UserAccount);
				frmEvaulationDetailWagesReports.Show();
				frmEvaulationDetailWagesReports.CmbJobOrderNo.Value = Id;
				frmEvaulationDetailWagesReports.GridFill();
				frmEvaulationDetailWagesReports.BringToFront();
				break;
			}
			case 110:
			{
				if (OpenInProductionTab)
				{
					this.OverHeadReadById?.Invoke(this, new IdEventArgs(Id));
					break;
				}
				frmProductionOverhead obj = new frmProductionOverhead(UserAccount);
				obj.BringToFront();
				obj.Show();
				obj.ReadByIdOverHeadJobOrderWise(Id);
				break;
			}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackingMaterialSettlement_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(grdPackingMaterialSettlement.CurrentRow.Cells["Id"].Value);
			if (OpenInProductionTab)
			{
				this.PackingMaterialReadById(this, new IdEventArgs(Id));
				return;
			}
			frmProductionPackingMaterial obj = new frmProductionPackingMaterial(UserAccount);
			obj.BringToFront();
			obj.Show();
			obj.ReadByIdPackingMaterial(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOutPutSettlement_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grdOutPutSettlement.UpdateData();
			GridEXRow item = grdOutPutSettlement.CurrentRow;
			if (e.Column.Key == "Rate" && Conversion.ToString(item.Cells["EntryType"].Value) == "ByProduct")
			{
				double Weight = Conversion.ToDouble((object)item.Cells["Weight"].Value.ToString());
				double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
				item.Cells["RateWithoutExp"].Value = Rate;
				item.Cells["NetRate"].Value = Rate;
				double RateUOM = Conversion.ToDouble((object)item.Cells["RateUOM"].Value.ToString());
				if (Weight > 0.0 && RateUOM > 0.0 && Rate > 0.0)
				{
					double Amount = Weight / RateUOM * Rate;
					item.Cells["ItemAmount"].Value = Amount;
					item.Cells["TotalAmount"].Value = Amount;
				}
				else
				{
					item.Cells["ItemAmount"].Value = 0;
					item.Cells["TotalAmount"].Value = 0;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnApplyAvgRateFG_Click(object sender, EventArgs e)
	{
		try
		{
			if (grdOutPutSettlement.GetRows().Count() == 0)
			{
				throw new Exception("OutPut grid records not found please check....");
			}
			CalcualteFinishGoodsRateAndAmounts();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalcualteFinishGoodsRateAndAmounts()
	{
		try
		{
			double RMQty = 0.0;
			double RMWeight = 0.0;
			double RMAmount = 0.0;
			double ReturnToGodownQty = 0.0;
			double ReturnToGodownWeight = 0.0;
			double ReturnToGodownAmount = 0.0;
			double BPQty = 0.0;
			double BPWeight = 0.0;
			double BPAmount = 0.0;
			double ByProductOverHeads = 0.0;
			double ByProductPackingMaterial = 0.0;
			double FGQty = 0.0;
			double FGWeight = 0.0;
			double FGAmount = 0.0;
			double PMAmount = 0.0;
			double OverHeadAmount = 0.0;
			GridEXRow[] rows = grdInputsettlement.GetRows();
			foreach (GridEXRow item2 in rows)
			{
				if (Conversion.ToString(item2.Cells["EntryType"].Value) == "Issue")
				{
					RMQty += GridHelper.GetDoubleValue(item2, "Quantity");
					RMWeight += GridHelper.GetDoubleValue(item2, "Weight");
					RMAmount += GridHelper.GetDoubleValue(item2, "ItemAmount");
				}
				else
				{
					ReturnToGodownQty += Math.Abs(GridHelper.GetDoubleValue(item2, "Quantity"));
					ReturnToGodownWeight += Math.Abs(GridHelper.GetDoubleValue(item2, "Weight"));
					ReturnToGodownAmount += Math.Abs(GridHelper.GetDoubleValue(item2, "ItemAmount"));
				}
			}
			txtItemQtyRM.Text = (RMQty - ReturnToGodownQty).ToString("#,#");
			txtNetWeightRM.Text = (RMWeight - ReturnToGodownWeight).ToString("#,#");
			txtAmountRM.Text = (RMAmount - ReturnToGodownAmount).ToString("#,#");
			rows = grdOutPutSettlement.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (Conversion.ToString(item.Cells["EntryType"].Value) == "FinishGoods")
				{
					FGQty += GridHelper.GetDoubleValue(item, "Quantity");
					FGWeight += GridHelper.GetDoubleValue(item, "Weight");
					FGAmount += GridHelper.GetDoubleValue(item, "ItemAmount");
				}
				else
				{
					BPQty += GridHelper.GetDoubleValue(item, "Quantity");
					BPWeight += GridHelper.GetDoubleValue(item, "Weight");
					BPAmount += GridHelper.GetDoubleValue(item, "ItemAmount");
					ByProductOverHeads += GridHelper.GetDoubleValue(item, "GeneralOhCost") + GridHelper.GetDoubleValue(item, "ItemOhCost");
					ByProductPackingMaterial += GridHelper.GetDoubleValue(item, "GeneralPmCost") + GridHelper.GetDoubleValue(item, "ItemPmCost");
				}
			}
			double BpTotalAmountWithExpenses = BPAmount + ByProductOverHeads + ByProductPackingMaterial;
			txtItemQtyBP.Text = BPQty.ToString("#,#");
			txtNetWeightBP.Text = BPWeight.ToString("#,#");
			txtAmountBP.Text = BPAmount.ToString("#,#");
			txtBPOverHead.Text = ByProductOverHeads.ToString("#,#");
			txtBPPMCharges.Text = ByProductPackingMaterial.ToString("#,#");
			txtBPTotalAmountWithExpenses.Text = BpTotalAmountWithExpenses.ToString("#,#");
			PMAmount = grdPackingMaterialSettlement.GetRows().Sum((GridEXRow row) => GridHelper.GetDoubleValue(row, "Amount"));
			txtPackingMaterialSettlement.Text = PMAmount.ToString("#,#");
			OverHeadAmount = grdOverHeadSettlement.GetRows().Sum((GridEXRow row) => GridHelper.GetDoubleValue(row, "Amount"));
			txtOverHeadsSettlement.Text = OverHeadAmount.ToString("#,#");
			double totalCostWithExpenses = RMAmount + PMAmount + OverHeadAmount - (ReturnToGodownAmount + BpTotalAmountWithExpenses);
			double totalCostWithoutExpenses = RMAmount - (ReturnToGodownAmount + BpTotalAmountWithExpenses);
			AvgRate = ((FGWeight > 0.0) ? (totalCostWithExpenses / FGWeight) : 0.0);
			AvgRateWithoutExp = ((FGWeight > 0.0) ? (totalCostWithoutExpenses / FGWeight) : 0.0);
			txtAvgRateFG.Text = AvgRate.ToString("N3");
			txtFinishGoodRateWithOutExp.Text = Math.Round(AvgRateWithoutExp, 0, MidpointRounding.AwayFromZero).ToString("0");
			txtItemQtyFG.Text = FGQty.ToString("#,#");
			txtNetWeightFG.Text = FGWeight.ToString("#,#");
			txtAmountFG.Text = Math.Round(totalCostWithExpenses, 0, MidpointRounding.AwayFromZero).ToString("0,0");
			ProporationFinishGoodsSettlement();
			if (Conversion.ToDouble((object)txtAmountFG.Text) > 0.0)
			{
				btnApproveSettlement.Visible = true;
			}
			else
			{
				btnApproveSettlement.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ProporationFinishGoodsSettlement()
	{
		try
		{
			double totalCostWithExpenses = 0.0;
			if (txtAvgRateFG.Text == string.Empty || txtAvgRateFG.Text == "0")
			{
				MessageBox.Show("Finish Goods AvgRate Not Found Please check!");
				return;
			}
			grdOutPutSettlement.UpdateData();
			GridEXRow[] rows = grdOutPutSettlement.GetRows();
			foreach (GridEXRow r in rows)
			{
				double Weight = 0.0;
				double JobOrderRate = 0.0;
				double Rate = 0.0;
				double RateUOM = 0.0;
				double ItemAmount = 0.0;
				double ItemPmCost = 0.0;
				double GeneralPmCost = 0.0;
				double ItemOhCost = 0.0;
				double GeneralOhCost = 0.0;
				double RiceAmountWithOutExp = 0.0;
				double FinishAmountWithExp = 0.0;
				double BrandRate = 0.0;
				double ByProductAmountWithExp = 0.0;
				double ByProductRate = 0.0;
				r.BeginEdit();
				if (Conversion.ToInt(r.Cells["RateUOMId"].Value) == 0)
				{
					DataTable dt = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r.Cells["ItemId"].Value));
					if (dt.Rows.Count > 0)
					{
						DataRow[] dr = dt.Select("Equivalent='" + Conversion.ToInt((object)40) + "'");
						if (dr.Length == 0)
						{
							MessageBox.Show("40KG RateUom not define please check");
							return;
						}
						r.Cells["RateUOMId"].Value = Conversion.ToInt(dr[0].ItemArray[0]);
						r.Cells["RateUOM"].Value = Conversion.ToInt(dr[0].ItemArray[2]);
					}
				}
				if (r.Cells["EntryType"].Value.ToString() == "FinishGoods")
				{
					Weight = Conversion.ToDouble((object)r.Cells["Weight"].Value.ToString());
					JobOrderRate = Conversion.ToDouble(r.Cells["Rate"].Value);
					Rate = Conversion.ToDouble((object)AvgRateWithoutExp);
					RateUOM = Conversion.ToDouble((object)r.Cells["RateUOM"].Value.ToString());
					ItemPmCost = Conversion.ToDouble((object)r.Cells["ItemPmCost"].Value.ToString());
					GeneralPmCost = Conversion.ToDouble((object)r.Cells["GeneralPmCost"].Value.ToString());
					ItemOhCost = Conversion.ToDouble((object)r.Cells["ItemOhCost"].Value.ToString());
					GeneralOhCost = Conversion.ToDouble((object)r.Cells["GeneralOhCost"].Value.ToString());
					RiceAmountWithOutExp = Weight * Rate;
					FinishAmountWithExp = RiceAmountWithOutExp + ItemPmCost + GeneralPmCost + ItemOhCost + GeneralOhCost;
					BrandRate = FinishAmountWithExp / Weight * RateUOM;
					if (Conversion.ToDouble(r.Cells["ItemAmount"].Value) == 0.0)
					{
						ItemAmount = Weight / RateUOM * JobOrderRate;
						r.Cells["ItemAmount"].Value = ItemAmount;
					}
					r.Cells["RateWithoutExp"].Value = Rate * RateUOM;
					r.Cells["AmountWithoutExp"].Value = RiceAmountWithOutExp.ToString();
					r.Cells["NetRate"].Value = BrandRate;
					r.Cells["TotalAmount"].Value = FinishAmountWithExp.ToString();
					totalCostWithExpenses += FinishAmountWithExp;
					r.EndEdit();
				}
				else if (r.Cells["EntryType"].Value.ToString() != "FinishGoods")
				{
					Weight = Conversion.ToDouble((object)r.Cells["Weight"].Value.ToString());
					Rate = Conversion.ToDouble((object)r.Cells["Rate"].Value.ToString());
					RateUOM = Conversion.ToDouble((object)r.Cells["RateUOM"].Value.ToString());
					ItemPmCost = Conversion.ToDouble((object)r.Cells["ItemPmCost"].Value.ToString());
					GeneralPmCost = Conversion.ToDouble((object)r.Cells["GeneralPmCost"].Value.ToString());
					ItemOhCost = Conversion.ToDouble((object)r.Cells["ItemOhCost"].Value.ToString());
					GeneralOhCost = Conversion.ToDouble((object)r.Cells["GeneralOhCost"].Value.ToString());
					RiceAmountWithOutExp = Weight / RateUOM * Rate;
					ByProductAmountWithExp = RiceAmountWithOutExp + ItemPmCost + GeneralPmCost + ItemOhCost + GeneralOhCost;
					ByProductRate = ByProductAmountWithExp / Weight * RateUOM;
					r.Cells["RateWithoutExp"].Value = Rate;
					r.Cells["AmountWithoutExp"].Value = RiceAmountWithOutExp.ToString();
					r.Cells["NetRate"].Value = ByProductRate;
					r.Cells["TotalAmount"].Value = ByProductAmountWithExp.ToString();
					r.EndEdit();
				}
			}
			grdOutPutSettelmentSetting();
			txtAmountFG.Text = Math.Round(totalCostWithExpenses, 0, MidpointRounding.AwayFromZero).ToString("0,0");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		grdOutPutSettlement.UpdateData();
	}

	private void ResetSettlement()
	{
		try
		{
			btnApproveSettlement.Visible = false;
			txtItemQtyRM.Text = string.Empty;
			txtItemQtyBP.Text = string.Empty;
			txtItemQtyFG.Text = string.Empty;
			txtNetWeightRM.Text = string.Empty;
			txtNetWeightBP.Text = string.Empty;
			txtNetWeightFG.Text = string.Empty;
			txtAmountRM.Text = string.Empty;
			txtAmountBP.Text = string.Empty;
			txtAmountFG.Text = string.Empty;
			txtBPOverHead.Text = string.Empty;
			txtBPPMCharges.Text = string.Empty;
			txtActualFinishGoodsAmount.Text = string.Empty;
			txtFinishAmountAgainstJobOrder.Text = string.Empty;
			txtDifferenceAmount.Text = string.Empty;
			((Control)(object)CmbDifferenceAccountTitle).Text = string.Empty;
			txtBPTotalAmountWithExpenses.Text = string.Empty;
			txtOverHeadsSettlement.Text = string.Empty;
			txtPackingMaterialSettlement.Text = string.Empty;
			txtAvgRateFG.Text = string.Empty;
			txtFinishGoodRateWithOutExp.Text = string.Empty;
			((Control)(object)CmbJobOrderSettlement).Text = string.Empty;
			grdInputsettlement.ClearStructure();
			grdOutPutSettlement.ClearStructure();
			grdOverHeadSettlement.ClearStructure();
			grdPackingMaterialSettlement.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnResetSettlement_Click(object sender, EventArgs e)
	{
		try
		{
			ResetSettlement();
			JobOrderNoFillForSettlement(1);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool UpdateSettlement()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		bool UpdateSuccssfulFlag = false;
		try
		{
			InvFoodProduction obj = new InvFoodProduction();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.InvJobOrderId = Conversion.ToInt(CmbJobOrderSettlement.Value);
			obj.invFoodProductionDetails = new List<InvFoodProductionDetail>();
			GridEXRow[] rows = grdOutPutSettlement.GetRows();
			foreach (GridEXRow r in rows)
			{
				InvFoodProductionDetail Conversiondetail = new InvFoodProductionDetail();
				if (Conversion.ToDouble(r.Cells["Weight"].Value) > 0.0)
				{
					Conversiondetail.Qty = Conversion.ToDouble((object)r.Cells["Quantity"].Text);
					Conversiondetail.Weight = Conversion.ToDouble(r.Cells["Weight"].Value);
					if (Conversion.ToString(r.Cells["ProductionApprovalStatus"].Value) == "UnApproved")
					{
						string itemName = Conversion.ToString(r.Cells["ItemName"].Value);
						int docNo = Conversion.ToInt(r.Cells["DocNo"].Value);
						MessageBox.Show($"Settlement cannot be processed because the Production Output transaction is still pending approval.\n\nItem: {itemName}\nDocument No: {docNo}\nQty: {Conversiondetail.Qty}\nWeight: {Conversiondetail.Weight}", "Approval Pending", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						return false;
					}
					Conversiondetail.Id = Conversion.ToInt(r.Cells["DetailId"].Value);
					Conversiondetail.InvFoodProductionId = Conversion.ToInt(r.Cells["Id"].Value);
					Conversiondetail.EntryType = Conversion.ToString((object)r.Cells["EntryType"].Text);
					Conversiondetail.RefDocNoId = 0;
					if (r.Cells["WareHouseId"].Text == string.Empty || Conversion.ToInt(r.Cells["WareHouseId"].Value) == 0)
					{
						MessageBox.Show("WareHouse Field Require in Output Detail");
						return false;
					}
					Conversiondetail.WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
					if (r.Cells["ItemId"].Text == string.Empty || Conversion.ToInt(r.Cells["ItemId"].Value) == 0)
					{
						MessageBox.Show("Item Field Require in Output Detail");
						return false;
					}
					Conversiondetail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					Conversiondetail.ItemUomId = Conversion.ToInt(r.Cells["ItemUOMId"].Value);
					Conversiondetail.CropBatch = Conversion.ToString((object)r.Cells["CropYear"].Text);
					if (r.Cells["JobLotId"].Text == string.Empty || r.Cells["JobLotId"].Text == "0")
					{
						MessageBox.Show("JobLot Field Require in Output Detail");
						return false;
					}
					Conversiondetail.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					Conversiondetail.PackingtypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
					if (Conversiondetail.Weight == 0.0)
					{
						throw new Exception("Weight Field Required");
					}
					Conversiondetail.Rate = Conversion.ToDouble(r.Cells["Rate"].Value);
					Conversiondetail.NetRate = ((Conversiondetail.EntryType == "FinishGoods" && r.Cells["Status"].Value.ToString() == "Used") ? Conversiondetail.Rate : Conversion.ToDouble(r.Cells["NetRate"].Value));
					Conversiondetail.RateUOMId = Conversion.ToInt(r.Cells["RateUOMId"].Value);
					Conversiondetail.InvProductionJobOrderId = Conversion.ToInt(CmbJobOrderSettlement.Value);
					Conversiondetail.Amount = Conversion.ToDouble(r.Cells["ItemAmount"].Value);
					Conversiondetail.Rate = ((Conversiondetail.Rate > 0.0) ? Conversiondetail.Rate : Conversion.ToDouble(r.Cells["RateWithoutExp"].Value));
					Conversiondetail.Amount = ((Conversiondetail.Amount > 0.0) ? Conversiondetail.Amount : Conversion.ToDouble(r.Cells["AmountWithoutExp"].Value));
					Conversiondetail.ItemPmCost = Conversion.ToDouble(r.Cells["ItemPmCost"].Value);
					Conversiondetail.GeneralPmCost = Conversion.ToDouble(r.Cells["GeneralPmCost"].Value);
					Conversiondetail.ItemOhCost = Conversion.ToDouble(r.Cells["ItemOhCost"].Value);
					Conversiondetail.GeneralOhCost = Conversion.ToDouble(r.Cells["GeneralOhCost"].Value);
					Conversiondetail.TotalAmount = ((Conversiondetail.EntryType == "FinishGoods" && r.Cells["Status"].Value.ToString() == "Used") ? Conversiondetail.Amount : Conversion.ToDouble(r.Cells["TotalAmount"].Value));
					if (Conversiondetail.Rate <= 0.0)
					{
						throw new Exception("Rate Field Required");
					}
					if (Conversiondetail.NetRate <= 0.0)
					{
						throw new Exception("NetRate Field Required");
					}
					if (Conversiondetail.RateUOMId == 0)
					{
						throw new Exception("RateUom Field Required");
					}
					if (Conversiondetail.Amount <= 0.0)
					{
						throw new Exception("ItemAmount must be greater than zero Please Check");
					}
					if (Conversiondetail.TotalAmount <= 0.0)
					{
						throw new Exception("TotalAmount must be greater than zero Please Check");
					}
					Conversiondetail.VoucherHeadId = Conversion.ToInt(r.Cells["WIPAccountId"].Value);
					Conversiondetail.Remarks = Conversion.ToString((object)r.Cells["Remarks"].Text);
					obj.invFoodProductionDetails.Add(Conversiondetail);
				}
			}
			InvFoodProduction.UpdateSettlement(obj);
			return true;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void InPutUpdateSettlement()
	{
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		try
		{
			HashSet<int> matchedIds = new HashSet<int>();
			GridEXRow[] rows = grdInputsettlement.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if ((Conversion.ToInt(r2.Cells["RefDocumentTypeId"].Value) == 112 && Conversion.ToInt(r2.Cells["RefJobOrderId"].Value) > 0) || Conversion.ToInt(r2.Cells["RefDocumentTypeId"].Value) == 56 || Conversion.ToInt(r2.Cells["RefDocumentTypeId"].Value) == 57 || Conversion.ToInt(r2.Cells["RefDocumentTypeId"].Value) == 40)
				{
					int id = Conversion.ToInt(r2.Cells["Id"].Value);
					matchedIds.Add(id);
				}
			}
			InvFoodProduction obj = new InvFoodProduction();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.invFoodProductionDetails = new List<InvFoodProductionDetail>();
			rows = grdInputsettlement.GetRows();
			foreach (GridEXRow r in rows)
			{
				int currentId = Conversion.ToInt(r.Cells["Id"].Value);
				InvFoodProductionDetail detail = new InvFoodProductionDetail();
				if (matchedIds.Contains(currentId))
				{
					detail.Id = Conversion.ToInt(r.Cells["DetailId"].Value);
					detail.InvFoodProductionId = Conversion.ToInt(r.Cells["Id"].Value);
					detail.EntryType = Conversion.ToString((object)r.Cells["EntryType"].Text);
					detail.RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
					detail.RefDocNoId = Conversion.ToInt(r.Cells["RefDocIdNo"].Value);
					detail.RefDocSubIdNo = Conversion.ToInt(r.Cells["RefDocSubIdNo"].Value);
					detail.WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
					detail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					detail.ItemUomId = Conversion.ToInt(r.Cells["ItemUOMId"].Value);
					detail.CropBatch = Conversion.ToString((object)r.Cells["CropYear"].Text);
					detail.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					detail.PackingtypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
					detail.Qty = Conversion.ToDouble((object)r.Cells["Quantity"].Text);
					detail.Weight = Conversion.ToDouble(r.Cells["Weight"].Value);
					detail.Rate = Conversion.ToDouble(r.Cells["Rate"].Value);
					if (detail.Rate == 0.0)
					{
						throw new Exception("Rate Field Required");
					}
					detail.NetRate = detail.Rate;
					detail.RateUOMId = Conversion.ToInt(r.Cells["RateUOMId"].Value);
					if (detail.RateUOMId == 0)
					{
						throw new Exception("RateUom Field Required");
					}
					detail.InvProductionJobOrderId = Conversion.ToInt(CmbJobOrderSettlement.Value);
					detail.Amount = Conversion.ToDouble(r.Cells["ItemAmount"].Value);
					if (detail.Amount == 0.0)
					{
						throw new Exception("ItemAmount must be greater than zero Please Check");
					}
					detail.TotalAmount = detail.Amount;
					detail.VoucherHeadId = Conversion.ToInt(r.Cells["WIPAccountId"].Value);
					detail.Remarks = Conversion.ToString((object)r.Cells["Remarks"].Text);
					obj.invFoodProductionDetails.Add(detail);
				}
			}
			InvFoodProduction.InPutUpdateSettlement(obj);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void SettlementFinancials()
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Expected O, but got Unknown
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Expected O, but got Unknown
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Expected O, but got Unknown
		try
		{
			double DifferenceAmount = Conversion.ToDouble((object)txtDifferenceAmount.Text);
			if (DifferenceAmount != 0.0)
			{
				if (((UltraGridBase)CmbJobOrderSettlement).ActiveRow == null)
				{
					MessageBox.Show("JobOrder Settlement Field is Required");
					CmbJobOrderSettlement.Focus();
					return;
				}
				if (((UltraGridBase)CmbDifferenceAccountTitle).ActiveRow == null)
				{
					MessageBox.Show("Difference Account Field is Required");
					CmbDifferenceAccountTitle.Focus();
					return;
				}
				int WorkInProccessAcId = 0;
				DataTable dt = InvProductionJobOrder.GetGlAccountsByJobOrderId(new InvProductionJobOrder
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = Conversion.ToInt(CmbJobOrderSettlement.Value)
				});
				if (dt == null || dt.Rows.Count <= 0)
				{
					throw new Exception("WIP Account Not Found");
				}
				WorkInProccessAcId = Conversion.ToInt(dt.Rows[0]["WorkInProccessAcId"]);
				if (WorkInProccessAcId == Conversion.ToInt(CmbDifferenceAccountTitle.Value))
				{
					throw new Exception("WIP Account and Difference Account are the same. Please verify.");
				}
				int VoucherId = CommonServices.VoucherHeadIdGet(Conversion.ToInt(CmbJobOrderSettlement.Value), 142);
				VoucherHead obj = new VoucherHead();
				obj.Id = VoucherId;
				obj.DocumentTypeSrNo = Conversion.ToInt(CmbJobOrderSettlement.Value);
				obj.ManualBillNo = Conversion.ToString((object)((Control)(object)CmbJobOrderSettlement).Text);
				obj.VoucherDate = Conversion.ToDateTime((object)txtSettlementDate.Value);
				obj.VoucherCode = obj.VoucherCode;
				obj.DocumentTypeId = 142;
				obj.RefAccountId = WorkInProccessAcId;
				obj.AgainstAccountId = Conversion.ToInt(CmbDifferenceAccountTitle.Value);
				obj.EntryDate = DateTime.Now;
				obj.ModifyDate = DateTime.Now;
				obj.EntryUser = UserAccount.ID;
				obj.ModifyUser = UserAccount.ID;
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				obj.BranchId = UserAccount.BranchesId;
				obj.voucherDetailList = new List<VoucherDetail>();
				string remarks = "";
				remarks = "Finish Goods ActualAmount " + txtActualFinishGoodsAmount.Text + " Finish Goods Amount against JobOrder " + ((Control)(object)CmbJobOrderSettlement).Text.Trim().ToString() + "  " + txtFinishAmountAgainstJobOrder.Text + " Difference Amount " + txtDifferenceAmount.Text;
				obj.VoucherAmount = Math.Abs(DifferenceAmount);
				VoucherDetail vd = new VoucherDetail();
				vd.AccountId = Conversion.ToInt(CmbDifferenceAccountTitle.Value);
				vd.AgainstAccountId = Conversion.ToInt((object)WorkInProccessAcId);
				vd.Comments = Conversion.ToString((object)remarks);
				if (DifferenceAmount > 0.0)
				{
					vd.DebitAmount = Conversion.ToDouble((object)DifferenceAmount);
				}
				else
				{
					vd.CreditAmount = Conversion.ToDouble((object)DifferenceAmount);
				}
				vd.OrderNo = Conversion.ToInt(CmbJobOrderSettlement.Value);
				vd.BranchesId = obj.BranchId;
				obj.voucherDetailList.Add(vd);
				VoucherDetail vd2 = new VoucherDetail();
				vd2.AccountId = Conversion.ToInt((object)WorkInProccessAcId);
				vd2.AgainstAccountId = Conversion.ToInt(CmbDifferenceAccountTitle.Value);
				vd2.Comments = Conversion.ToString((object)remarks);
				if (DifferenceAmount > 0.0)
				{
					vd2.CreditAmount = Conversion.ToDouble((object)DifferenceAmount);
				}
				else
				{
					vd2.DebitAmount = Conversion.ToDouble((object)DifferenceAmount);
				}
				vd2.OrderNo = Conversion.ToInt(CmbJobOrderSettlement.Value);
				vd2.BranchesId = obj.BranchId;
				obj.voucherDetailList.Add(vd2);
				DataTable dtValidate = InvFoodProduction.ProductionSettlementWeightValidations(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToInt(CmbJobOrderSettlement.Value));
				if (dtValidate != null && dtValidate.Rows.Count > 0)
				{
					double InPutWeight = Conversion.ToDouble(dtValidate.Rows[0]["InPutWeight"]);
					double OutPutWeight = Conversion.ToDouble(dtValidate.Rows[0]["OutPutWeight"]);
					double DiffWeight = Conversion.ToDouble(dtValidate.Rows[0]["DiffWeight"]);
					double DiffPercentage = Conversion.ToDouble(dtValidate.Rows[0]["DiffPercentage"]);
					int num = Conversion.ToInt(dtValidate.Rows[0]["EntryStatusId"]);
					string Message = Conversion.ToString(dtValidate.Rows[0]["ErrorMessage"]);
					if (num != 1)
					{
						throw new Exception(Message);
					}
					DataTable dtSpecialApproval = InvProductionJobOrder.CheckJobOrderOrderIsSpecialApproved(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToInt(CmbJobOrderSettlement.Value));
					if (dtSpecialApproval != null && dtSpecialApproval.Rows.Count > 0)
					{
						int SpecialApprovalId = Conversion.ToInt(dtSpecialApproval.Rows[0]["SpecialApprovalId"]);
						int SpecialApprovalUserId = Conversion.ToInt(dtSpecialApproval.Rows[0]["SpecialApprovalUserId"]);
						if (SpecialApprovalId == 0 && SpecialApprovalUserId == 0)
						{
							if (MessageBox.Show(Message + "\nYou need to reduce this weight difference or request special approval for this job order. Do you want to request special approval for this job order???", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
							{
								return;
							}
							InvProductionJobOrder.RequestFromSettlementForJobOrderForSpecialApproval(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToInt(CmbJobOrderSettlement.Value), UserAccount.ID, Message);
							throw new Exception("Job Order Sent for special approval.");
						}
						if (SpecialApprovalId == 1 && SpecialApprovalUserId == 0)
						{
							Message = "This Job Order is currently in Special Approval. You cannot settle it until the special approval is granted";
							throw new Exception(Message);
						}
					}
					else if (DiffPercentage > 50.0)
					{
						Message = $"The difference between input weight ({InPutWeight}) and output weight ({OutPutWeight}) " + "is greater than 50%.\n" + $"Difference weight: {DiffWeight} and Difference Percentage: {DiffPercentage}%\n" + $"The entry is in {100.0 - DiffPercentage}% loss, which cannot be settled even after special approval.";
						throw new Exception(Message);
					}
				}
				if (InvFoodProduction.SetDataForSettlementFinancials(obj) > 0)
				{
					MessageBox.Show("Record Save Successfully");
				}
				else
				{
					MessageBox.Show("Record Update Successfully");
				}
			}
			ResetSettlement();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnUpdateSettlement_Click(object sender, EventArgs e)
	{
		try
		{
			if (grdOutPutSettlement.GetRows().Count() == 0)
			{
				throw new Exception("OutPut grid records not found please check....");
			}
			if (!((DataTable)grdOutPutSettlement.DataSource).AsEnumerable().Any((DataRow row) => row.Field<string>("EntryType") == "FinishGoods"))
			{
				throw new Exception("Finish Goods records not found. Please check.");
			}
			if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			if (FIFOCGSFlag && !SaleCostingJobOrderWise)
			{
				double DifferenceAmount = Conversion.ToDouble((object)txtDifferenceAmount.Text);
				if ((from x in ((DataTable)grdOutPutSettlement.DataSource).AsEnumerable()
					where x.Field<string>("EntryType") == "FinishGoods" && x.Field<string>("Status") == "NotUsed"
					select x).Count() > 0)
				{
					if (UpdateSettlement())
					{
						SettlementFinancials();
					}
				}
				else if (DifferenceAmount != 0.0)
				{
					SettlementFinancials();
				}
				else
				{
					ResetSettlement();
				}
			}
			else if (SaleCostingJobOrderWise)
			{
				DataTable dt = (DataTable)grdInputsettlement.DataSource;
				if ((from x in dt.AsEnumerable()
					where x.Field<int>("RefJobOrderId") > 0 && x.Field<string>("RefJobStatus") == "UnSettled"
					select x).Count() > 0)
				{
					MessageBox.Show("Some related Job Orders have already been used in Production Input but are still pending settlement. Please settle and approve them first to continue.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				if ((from x in dt.AsEnumerable()
					where x.Field<int>("InvoiceNo") > 0 && x.Field<int>("InvoiceApproved") == 0
					select x).Count() > 0)
				{
					MessageBox.Show("You cannot proceed because some invoices have not been approved yet. Please review and approve them first.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				if (dt.AsEnumerable().Count((DataRow x) => (Conversion.ToInt(x["RefDocumentTypeId"]) == 112 && Conversion.ToInt(x["RefJobOrderId"]) > 0) || Conversion.ToInt(x["RefDocumentTypeId"]) == 56 || Conversion.ToInt(x["RefDocumentTypeId"]) == 57 || Conversion.ToInt(x["RefDocumentTypeId"]) == 40) > 0)
				{
					InPutUpdateSettlement();
				}
				UpdateSettlement();
				ResetSettlement();
			}
			else
			{
				UpdateSettlement();
				ResetSettlement();
			}
			if (chkAllJobOrderOnSettlement.Checked)
			{
				JobOrderNoFillForSettlement(0);
			}
			else
			{
				JobOrderNoFillForSettlement(1);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btn613SummaryReport_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.JobOrderId = Conversion.ToInt(CmbJobOrderSettlement.Value);
			obj.ActionId = (chkIncludeWages.Checked ? 1 : obj.ActionId);
			DataTable dtSummerReport = InvFoodProductionReports.InvFoodProduction_Summery2_Rpt(obj);
			if (dtSummerReport.Rows.Count > 0 && dtSummerReport != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtSummerReport, "613-InvRptProductionSummeryWithExpValues.rpt");
			}
			else
			{
				MessageBox.Show("Record Not Found For DisPlay");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPrint613A_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.JobOrderId = Conversion.ToInt(CmbJobOrderSettlement.Value);
			obj.ActionId = (chkIncludeWages.Checked ? 1 : obj.ActionId);
			DataTable dtSummerReport = InvFoodProductionReports.ProductionBeforSettlement_Summery2(obj);
			if (dtSummerReport.Rows.Count > 0 && dtSummerReport != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtSummerReport, "613_01-ProductionBeforSettlement_Summery2.rpt");
			}
			else
			{
				MessageBox.Show("Record Not Found For DisPlay");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btn615Report_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.JobOrderId = Conversion.ToInt(CmbJobOrderSettlement.Value);
			obj.ActionId = (chkIncludeWages.Checked ? 1 : obj.ActionId);
			DataTable dtSummerReport = InvFoodProductionReports.InvFoodProduction_Summery3_Rpt(obj);
			if (dtSummerReport.Rows.Count > 0 && dtSummerReport != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtSummerReport, "615-InvRptProductionSummery.rpt");
			}
			else
			{
				MessageBox.Show("Record Not Found For DisPlay");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPrint615A_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.JobOrderId = Conversion.ToInt(CmbJobOrderSettlement.Value);
			obj.ActionId = (chkIncludeWages.Checked ? 1 : obj.ActionId);
			DataTable dtSummerReport = InvFoodProductionReports.ProductionBeforSettlement_Summery3(obj);
			if (dtSummerReport.Rows.Count > 0 && dtSummerReport != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtSummerReport, "615_01-ProductionBeforSettlement_Summery3.rpt");
			}
			else
			{
				MessageBox.Show("Record Not Found For DisPlay");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRecoveryReport_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.JobOrderId = Conversion.ToInt(CmbJobOrderSettlement.Value);
			obj.ActionId = (chkIncludeWages.Checked ? 1 : obj.ActionId);
			DataTable dtSummerReport = InvFoodProductionReports.InvFoodProductionRecoverySummeryReport(obj);
			if (dtSummerReport.Rows.Count > 0 && dtSummerReport != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtSummerReport, "602A-InvRptProductionSummeryWithValues.rpt");
			}
			else
			{
				MessageBox.Show("Record Not Found For DisPlay");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnApproveSettlement_Click(object sender, EventArgs e)
	{
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Expected O, but got Unknown
		try
		{
			if (SaleCostingJobOrderWise)
			{
				DataTable dt = (DataTable)grdInputsettlement.DataSource;
				if ((from x in dt.AsEnumerable()
					where x.Field<int>("RefJobOrderId") > 0 && x.Field<string>("RefJobStatus") == "UnSettled"
					select x).Count() > 0)
				{
					MessageBox.Show("Some related Job Orders have already been used in Production Input but are still pending settlement. Please settle and approve them first to continue.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				if ((from x in dt.AsEnumerable()
					where x.Field<int>("InvoiceNo") > 0 && x.Field<int>("InvoiceApproved") == 0
					select x).Count() > 0)
				{
					MessageBox.Show("You cannot proceed because some invoices have not been approved yet. Please review and approve them first.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
			}
			DataRow pendingRow = ((DataTable)grdOutPutSettlement.DataSource).AsEnumerable().FirstOrDefault((DataRow x) => x.Field<string>("ProductionApprovalStatus") == "UnApproved");
			if (pendingRow != null)
			{
				string itemName = Convert.ToString(pendingRow["ItemName"]);
				string docNo = Convert.ToString(pendingRow["DocNo"]);
				MessageBox.Show("Settlement cannot be processed because the Production Output transaction is still pending approval.\n\nItem: " + itemName + "\nDocument No: " + docNo, "Approval Pending", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else if (MessageBox.Show("Are you sure to Approve Settlement?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList detail = new ApprovalList();
				detail.OrganizationId = UserAccount.OrganizationId;
				detail.CompanyId = UserAccount.CompanyId;
				detail.Id = Conversion.ToInt(CmbJobOrderSettlement.Value);
				detail.EntryUser = UserAccount.ID;
				val.AllApprovalLists.Add(detail);
				InvProductionJobOrder.JobOrderApproveList(val);
				MessageBox.Show("Record Approved Successfully");
				ResetSettlement();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void FoodProductionWithValues_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnSettlementVoucher_Click(object sender, EventArgs e)
	{
		try
		{
			int VoucherId = CommonServices.VoucherHeadIdGet(Conversion.ToInt(CmbJobOrderSettlement.Value), 142);
			if (VoucherId > 0)
			{
				CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherId, 142);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void chkAllJobOrderOnSettlement_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (chkAllJobOrderOnSettlement.Checked)
			{
				JobOrderNoFillForSettlement(0);
			}
			else
			{
				JobOrderNoFillForSettlement(1);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbJobOrderSettlement_Leave(object sender, EventArgs e)
	{
		try
		{
			DateTime SettlementDate = DateTime.Now;
			if (((Control)(object)CmbJobOrderSettlement).Text != string.Empty && ((UltraGridBase)CmbJobOrderSettlement).ActiveRow != null && Conversion.ToInt(CmbJobOrderSettlement.Value) != 0)
			{
				SettlementDate = Conversion.ToDateTime(((UltraDropDownBase)CmbJobOrderSettlement).SelectedRow.Cells["SettlementDate"].Value);
			}
			txtSettlementDate.Value = SettlementDate;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPlantFeederPackingMaterial_Leave(object sender, EventArgs e)
	{
	}

	private void grdOutPutSettlement_Click(object sender, EventArgs e)
	{
		try
		{
			if (SaleCostingJobOrderWise && !FIFOCGSFlag)
			{
				grdOutPutSettlement.UpdateData();
				if (Conversion.ToString(grdOutPutSettlement.CurrentRow.Cells["EntryType"].Value) != "ByProduct")
				{
					grdOutPutSettlement.AllowEdit = (InheritableBoolean)2;
				}
				else
				{
					grdOutPutSettlement.AllowEdit = (InheritableBoolean)1;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOutPutSettlement_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			GridEXRow row = grdOutPutSettlement.CurrentRow;
			if (!((Control)(object)grdOutPutSettlement).Focused)
			{
				return;
			}
			if (e.KeyCode == Keys.Up)
			{
				int rowIndex = -1;
				if (row.RowIndex != 0)
				{
					rowIndex = row.RowIndex - 1;
				}
				if (Conversion.ToString(grdOutPutSettlement.GetRow(rowIndex).Cells["EntryType"].Value) != "ByProduct")
				{
					grdOutPutSettlement.AllowEdit = (InheritableBoolean)2;
				}
				else
				{
					grdOutPutSettlement.AllowEdit = (InheritableBoolean)1;
				}
			}
			else if (e.KeyCode == Keys.Down)
			{
				int rowIndex2 = -1;
				if (row.RowIndex != grdOutPutSettlement.GetRows().Count() - 1)
				{
					rowIndex2 = row.RowIndex + 1;
				}
				if (Conversion.ToString(grdOutPutSettlement.GetRow(rowIndex2).Cells["EntryType"].Value) != "ByProduct")
				{
					grdOutPutSettlement.AllowEdit = (InheritableBoolean)2;
				}
				else
				{
					grdOutPutSettlement.AllowEdit = (InheritableBoolean)1;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnByProductSettlementPopUp_Click(object sender, EventArgs e)
	{
		try
		{
			DataTable averagedByProductData = GetAveragedByProductData(grdOutPutSettlement);
			List<string> columnsToHide = new List<string> { "ItemId" };
			Dictionary<string, EditType> editableCols = new Dictionary<string, EditType> { 
			{
				"Rate/Kg",
				(EditType)1
			} };
			GrdPopUpToReturndt popup = new GrdPopUpToReturndt(averagedByProductData, selection: false, returnFullGrdDt: true, columnsToHide, editableCols)
			{
				StartPosition = FormStartPosition.CenterScreen,
				WindowState = FormWindowState.Normal,
				Text = "Please double-click any row to update values."
			};
			popup.ShowDialog();
			popup.BringToFront();
			DataTable dtReturn = popup.dtReturn;
			if (dtReturn != null && dtReturn.Rows.Count > 0)
			{
				ApplyUpdatedRatesToGrid(popup.dtReturn, grdOutPutSettlement);
				CalcualteFinishGoodsRateAndAmounts();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable GetAveragedByProductData(GridEX grid)
	{
		DataTable dt = new DataTable();
		dt.Columns.Add("ItemId", typeof(int));
		dt.Columns.Add("ItemName", typeof(string));
		dt.Columns.Add("Qty", typeof(double));
		dt.Columns.Add("Weight", typeof(double));
		dt.Columns.Add("AvgRate/Kg", typeof(double));
		dt.Columns.Add("Rate/Kg", typeof(double));
		dt.Columns.Add("RateTotal", typeof(double));
		GridEXRow[] dataRows = grid.GetDataRows();
		foreach (GridEXRow row in dataRows)
		{
			if (!(Conversion.ToString(row.Cells["EntryType"].Value) != "ByProduct"))
			{
				int itemId = Conversion.ToInt(row.Cells["ItemId"].Value);
				string itemName = Conversion.ToString(row.Cells["ItemName"].Value);
				double qty = Conversion.ToDouble(row.Cells["Quantity"].Value);
				double weight = Conversion.ToDouble(row.Cells["Weight"].Value);
				double rate = Conversion.ToDouble(row.Cells["Rate"].Value);
				double rateUom = Conversion.ToDouble(row.Cells["RateUOM"].Value);
				double ratePerKg = ((rateUom > 0.0) ? (rate / rateUom) : 0.0);
				double rateTotal = ratePerKg * weight;
				DataRow existingRow = dt.AsEnumerable().FirstOrDefault((DataRow r) => Conversion.ToInt(r["ItemId"]) == itemId);
				if (existingRow != null)
				{
					existingRow["Qty"] = Conversion.ToDouble(existingRow["Qty"]) + qty;
					existingRow["Weight"] = Conversion.ToDouble(existingRow["Weight"]) + weight;
					existingRow["RateTotal"] = Conversion.ToDouble(existingRow["RateTotal"]) + rateTotal;
					double totalWeight = Conversion.ToDouble(existingRow["Weight"]);
					double totalRateValue = Conversion.ToDouble(existingRow["RateTotal"]);
					existingRow["AvgRate/Kg"] = ((totalWeight > 0.0) ? (totalRateValue / totalWeight) : 0.0);
					existingRow["Rate/Kg"] = ((totalWeight > 0.0) ? (totalRateValue / totalWeight) : 0.0);
				}
				else
				{
					dt.Rows.Add(itemId, itemName, qty, weight, ratePerKg, ratePerKg, rateTotal);
				}
			}
		}
		dt.Columns.Remove("RateTotal");
		return dt;
	}

	private void ApplyUpdatedRatesToGrid(DataTable dtReturn, GridEX grid)
	{
		GridEXRow[] dataRows = grid.GetDataRows();
		foreach (GridEXRow row in dataRows)
		{
			if (!(Conversion.ToString(row.Cells["EntryType"].Value) != "ByProduct"))
			{
				int itemId = Conversion.ToInt(row.Cells["ItemId"].Value);
				DataRow matchedRow = dtReturn.AsEnumerable().FirstOrDefault((DataRow r) => r.Field<int>("ItemId") == itemId);
				if (matchedRow != null)
				{
					double num = Conversion.ToDouble(matchedRow["Rate/Kg"]);
					double rateUOM = Conversion.ToDouble(row.Cells["RateUOM"].Value);
					double rate = num * rateUOM;
					double weight = Conversion.ToDouble(row.Cells["Weight"].Value);
					double amount = ((weight > 0.0 && rateUOM > 0.0 && rate > 0.0) ? (weight / rateUOM * rate) : 0.0);
					row.BeginEdit();
					row.Cells["Rate"].Value = rate;
					row.Cells["RateWithoutExp"].Value = rate;
					row.Cells["NetRate"].Value = rate;
					row.Cells["ItemAmount"].Value = amount;
					row.Cells["TotalAmount"].Value = amount;
					row.EndEdit();
				}
			}
		}
	}

	private void btnAllocateExportInvoiceToJobOrder_Click(object sender, EventArgs e)
	{
		try
		{
			new ProductionOutputAllocationWithExportInvoice(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPrint613B_Click(object sender, EventArgs e)
	{
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			int Id = Conversion.ToInt(CmbJobOrderSettlement.Value);
			int ActionId = (chkIncludeWages.Checked ? 1 : 0);
			DataTable dt = InvFoodProductionReports.ProductionSettlement_WithReferenceDocumentDetailReport(UserAccount.OrganizationId, UserAccount.CompanyId, Id, ActionId);
			if (dt.Rows.Count > 0 && dt != null)
			{
				List<SubReportObject> lstSubReports = new List<SubReportObject>();
				SubReportObject SubReport1 = new SubReportObject(ProductionOutputAllocationWithExportInvoice.ProductionOutputAllocationWithExportInvoice_SlipAndRegister(Id, 0), "612-InvRptProductionDetailWithExpValues.rpt");
				lstSubReports.Add(SubReport1);
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTableSubReprt(dt, "613_02_ProductionSettlement_WithReferenceDocumentDetailReport.rpt", lstSubReports);
			}
			else
			{
				MessageBox.Show("Record Not Found For DisPlay");
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
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Expected O, but got Unknown
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Expected O, but got Unknown
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Expected O, but got Unknown
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Expected O, but got Unknown
		//IL_055c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Expected O, but got Unknown
		//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f5: Expected O, but got Unknown
		//IL_0617: Unknown result type (might be due to invalid IL or missing references)
		//IL_0621: Expected O, but got Unknown
		//IL_0622: Unknown result type (might be due to invalid IL or missing references)
		//IL_062c: Expected O, but got Unknown
		//IL_062d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0637: Expected O, but got Unknown
		//IL_0638: Unknown result type (might be due to invalid IL or missing references)
		//IL_0642: Expected O, but got Unknown
		//IL_0643: Unknown result type (might be due to invalid IL or missing references)
		//IL_064d: Expected O, but got Unknown
		//IL_096f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0979: Expected O, but got Unknown
		//IL_0b94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9e: Expected O, but got Unknown
		//IL_0ced: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf7: Expected O, but got Unknown
		GridEXLayout grdOutPutSettlement_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPackingMaterialSettlement_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOverHeadSettlement_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInputsettlement_DesignTimeLayout = new GridEXLayout();
		Appearance appearance152 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.grdOutPutSettlement = new GridEX();
		this.panel36 = new System.Windows.Forms.Panel();
		this.panel39 = new System.Windows.Forms.Panel();
		this.grdPackingMaterialSettlement = new GridEX();
		this.panel38 = new System.Windows.Forms.Panel();
		this.grdOverHeadSettlement = new GridEX();
		this.panel37 = new System.Windows.Forms.Panel();
		this.label10 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.panel40 = new System.Windows.Forms.Panel();
		this.panel51 = new System.Windows.Forms.Panel();
		this.panel52 = new System.Windows.Forms.Panel();
		this.label22 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label59 = new System.Windows.Forms.Label();
		this.txtItemQtyRM = new System.Windows.Forms.TextBox();
		this.txtItemQtyBP = new System.Windows.Forms.TextBox();
		this.txtBPOverHead = new System.Windows.Forms.TextBox();
		this.txtNetWeightRM = new System.Windows.Forms.TextBox();
		this.txtAmountFG = new System.Windows.Forms.TextBox();
		this.txtBPTotalAmountWithExpenses = new System.Windows.Forms.TextBox();
		this.txtNetWeightFG = new System.Windows.Forms.TextBox();
		this.txtNetWeightBP = new System.Windows.Forms.TextBox();
		this.txtAmountBP = new System.Windows.Forms.TextBox();
		this.txtBPPMCharges = new System.Windows.Forms.TextBox();
		this.label21 = new System.Windows.Forms.Label();
		this.txtAmountRM = new System.Windows.Forms.TextBox();
		this.txtItemQtyFG = new System.Windows.Forms.TextBox();
		this.panelDifferenceAmounts = new System.Windows.Forms.Panel();
		this.txtDifferenceAmount = new System.Windows.Forms.TextBox();
		this.label26 = new System.Windows.Forms.Label();
		this.panel75 = new System.Windows.Forms.Panel();
		this.label28 = new System.Windows.Forms.Label();
		this.txtFinishAmountAgainstJobOrder = new System.Windows.Forms.TextBox();
		this.txtActualFinishGoodsAmount = new System.Windows.Forms.TextBox();
		this.label29 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.panel55 = new System.Windows.Forms.Panel();
		this.panel56 = new System.Windows.Forms.Panel();
		this.label58 = new System.Windows.Forms.Label();
		this.txtAvgRateFG = new System.Windows.Forms.TextBox();
		this.label94 = new System.Windows.Forms.Label();
		this.txtFinishGoodRateWithOutExp = new System.Windows.Forms.TextBox();
		this.label114 = new System.Windows.Forms.Label();
		this.panel53 = new System.Windows.Forms.Panel();
		this.panel54 = new System.Windows.Forms.Panel();
		this.label115 = new System.Windows.Forms.Label();
		this.txtOverHeadsSettlement = new System.Windows.Forms.TextBox();
		this.txtPackingMaterialSettlement = new System.Windows.Forms.TextBox();
		this.label116 = new System.Windows.Forms.Label();
		this.label117 = new System.Windows.Forms.Label();
		this.panel34 = new System.Windows.Forms.Panel();
		this.btnAllocateExportInvoiceToJobOrder = new System.Windows.Forms.Button();
		this.label171 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.BtnByProductSettlementPopUp = new System.Windows.Forms.Button();
		this.grdInputsettlement = new GridEX();
		this.panel49 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.panel32 = new System.Windows.Forms.Panel();
		this.txtSettlementDate = new System.Windows.Forms.DateTimePicker();
		this.chkIncludeWages = new System.Windows.Forms.CheckBox();
		this.chkAllJobOrderOnSettlement = new System.Windows.Forms.CheckBox();
		this.btnSettlementVoucher = new System.Windows.Forms.Button();
		this.CmbDifferenceAccountTitle = new UltraCombo();
		this.label113 = new System.Windows.Forms.Label();
		this.BtnPrint615A = new System.Windows.Forms.Button();
		this.btn615Report = new System.Windows.Forms.Button();
		this.btnApproveSettlement = new System.Windows.Forms.Button();
		this.BtnPrint613B = new System.Windows.Forms.Button();
		this.BtnPrint613A = new System.Windows.Forms.Button();
		this.btn613SummaryReport = new System.Windows.Forms.Button();
		this.btnRecoveryReport = new System.Windows.Forms.Button();
		this.btnUpdateSettlement = new System.Windows.Forms.Button();
		this.btnApplyAvgRateFG = new System.Windows.Forms.Button();
		this.btnResetSettlement = new System.Windows.Forms.Button();
		this.btnGeneralSettlement = new System.Windows.Forms.Button();
		this.CmbJobOrderSettlement = new UltraCombo();
		this.label169 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.CmbBrandNamePackingMaterial = new UltraCombo();
		this.CmbBrandUomPackingMaterial = new UltraCombo();
		this.cmbItemPackingMaterial = new UltraCombo();
		this.CmbWarehousePackingMaterial = new UltraCombo();
		this.CmbPlantFeederPackingMaterial = new UltraCombo();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOutPutSettlement).BeginInit();
		this.panel36.SuspendLayout();
		this.panel39.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPackingMaterialSettlement).BeginInit();
		this.panel38.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOverHeadSettlement).BeginInit();
		this.panel37.SuspendLayout();
		this.panel40.SuspendLayout();
		this.panel51.SuspendLayout();
		this.panel52.SuspendLayout();
		this.panelDifferenceAmounts.SuspendLayout();
		this.panel75.SuspendLayout();
		this.panel55.SuspendLayout();
		this.panel56.SuspendLayout();
		this.panel53.SuspendLayout();
		this.panel54.SuspendLayout();
		this.panel34.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInputsettlement).BeginInit();
		this.panel49.SuspendLayout();
		this.panel32.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDifferenceAccountTitle).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderSettlement).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandNamePackingMaterial).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandUomPackingMaterial).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemPackingMaterial).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehousePackingMaterial).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantFeederPackingMaterial).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdOutPutSettlement);
		this.panel1.Controls.Add(this.panel36);
		this.panel1.Controls.Add(this.panel40);
		this.panel1.Controls.Add(this.panel34);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdInputsettlement);
		this.panel1.Controls.Add(this.panel49);
		this.panel1.Controls.Add(this.panel32);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1164, 739);
		this.panel1.TabIndex = 0;
		((ScrollableBase)this.grdOutPutSettlement).BackColor = System.Drawing.Color.White;
		grdOutPutSettlement_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOutPutSettlement.DesignTimeLayout = grdOutPutSettlement_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOutPutSettlement).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdOutPutSettlement.FilterMode = (FilterMode)2;
		this.grdOutPutSettlement.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdOutPutSettlement).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOutPutSettlement.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOutPutSettlement).Location = new System.Drawing.Point(0, 277);
		((System.Windows.Forms.Control)(object)this.grdOutPutSettlement).Name = "grdOutPutSettlement";
		((ScrollableBase)this.grdOutPutSettlement).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOutPutSettlement).Size = new System.Drawing.Size(1164, 140);
		((System.Windows.Forms.Control)(object)this.grdOutPutSettlement).TabIndex = 2;
		this.grdOutPutSettlement.TotalRow = (InheritableBoolean)1;
		this.grdOutPutSettlement.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOutPutSettlement).VisualStyle = (VisualStyle)4;
		this.grdOutPutSettlement.CellUpdated += new ColumnActionEventHandler(grdOutPutSettlement_CellUpdated);
		((System.Windows.Forms.Control)(object)this.grdOutPutSettlement).Click += new System.EventHandler(grdOutPutSettlement_Click);
		((System.Windows.Forms.Control)(object)this.grdOutPutSettlement).KeyDown += new System.Windows.Forms.KeyEventHandler(grdOutPutSettlement_KeyDown);
		this.panel36.Controls.Add(this.panel39);
		this.panel36.Controls.Add(this.panel38);
		this.panel36.Controls.Add(this.panel37);
		this.panel36.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel36.Location = new System.Drawing.Point(0, 417);
		this.panel36.Name = "panel36";
		this.panel36.Size = new System.Drawing.Size(1164, 143);
		this.panel36.TabIndex = 7;
		this.panel39.Controls.Add((System.Windows.Forms.Control)(object)this.grdPackingMaterialSettlement);
		this.panel39.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel39.Location = new System.Drawing.Point(628, 23);
		this.panel39.Name = "panel39";
		this.panel39.Size = new System.Drawing.Size(536, 120);
		this.panel39.TabIndex = 2;
		this.grdPackingMaterialSettlement.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPackingMaterialSettlement).BackColor = System.Drawing.Color.White;
		grdPackingMaterialSettlement_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPackingMaterialSettlement.DesignTimeLayout = grdPackingMaterialSettlement_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialSettlement).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialSettlement).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPackingMaterialSettlement.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialSettlement).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialSettlement).Name = "grdPackingMaterialSettlement";
		((ScrollableBase)this.grdPackingMaterialSettlement).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialSettlement).Size = new System.Drawing.Size(536, 120);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterialSettlement).TabIndex = 0;
		this.grdPackingMaterialSettlement.TotalRow = (InheritableBoolean)1;
		this.grdPackingMaterialSettlement.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPackingMaterialSettlement).VisualStyle = (VisualStyle)4;
		this.grdPackingMaterialSettlement.LinkClicked += new ColumnActionEventHandler(grdPackingMaterialSettlement_LinkClicked);
		this.panel38.Controls.Add((System.Windows.Forms.Control)(object)this.grdOverHeadSettlement);
		this.panel38.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel38.Location = new System.Drawing.Point(0, 23);
		this.panel38.Name = "panel38";
		this.panel38.Size = new System.Drawing.Size(628, 120);
		this.panel38.TabIndex = 1;
		this.grdOverHeadSettlement.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdOverHeadSettlement).BackColor = System.Drawing.Color.White;
		grdOverHeadSettlement_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOverHeadSettlement.DesignTimeLayout = grdOverHeadSettlement_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOverHeadSettlement).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdOverHeadSettlement).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOverHeadSettlement.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOverHeadSettlement).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdOverHeadSettlement).Name = "grdOverHeadSettlement";
		((ScrollableBase)this.grdOverHeadSettlement).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOverHeadSettlement).Size = new System.Drawing.Size(628, 120);
		((System.Windows.Forms.Control)(object)this.grdOverHeadSettlement).TabIndex = 0;
		this.grdOverHeadSettlement.TotalRow = (InheritableBoolean)1;
		this.grdOverHeadSettlement.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOverHeadSettlement).VisualStyle = (VisualStyle)4;
		this.grdOverHeadSettlement.LinkClicked += new ColumnActionEventHandler(grdOverHeadSettlement_LinkClicked);
		this.panel37.BackColor = System.Drawing.Color.Teal;
		this.panel37.Controls.Add(this.label10);
		this.panel37.Controls.Add(this.label8);
		this.panel37.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel37.Location = new System.Drawing.Point(0, 0);
		this.panel37.Name = "panel37";
		this.panel37.Size = new System.Drawing.Size(1164, 23);
		this.panel37.TabIndex = 0;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.White;
		this.label10.Location = new System.Drawing.Point(639, 4);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(99, 15);
		this.label10.TabIndex = 4;
		this.label10.Text = "Packing Material";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(10, 4);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(72, 15);
		this.label8.TabIndex = 3;
		this.label8.Text = "Over Heads";
		this.panel40.Controls.Add(this.panel51);
		this.panel40.Controls.Add(this.panelDifferenceAmounts);
		this.panel40.Controls.Add(this.panel55);
		this.panel40.Controls.Add(this.panel53);
		this.panel40.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel40.Location = new System.Drawing.Point(0, 560);
		this.panel40.Name = "panel40";
		this.panel40.Size = new System.Drawing.Size(1164, 179);
		this.panel40.TabIndex = 3;
		this.panel51.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel51.Controls.Add(this.panel52);
		this.panel51.Controls.Add(this.label17);
		this.panel51.Controls.Add(this.label18);
		this.panel51.Controls.Add(this.label59);
		this.panel51.Controls.Add(this.txtItemQtyRM);
		this.panel51.Controls.Add(this.txtItemQtyBP);
		this.panel51.Controls.Add(this.txtBPOverHead);
		this.panel51.Controls.Add(this.txtNetWeightRM);
		this.panel51.Controls.Add(this.txtAmountFG);
		this.panel51.Controls.Add(this.txtBPTotalAmountWithExpenses);
		this.panel51.Controls.Add(this.txtNetWeightFG);
		this.panel51.Controls.Add(this.txtNetWeightBP);
		this.panel51.Controls.Add(this.txtAmountBP);
		this.panel51.Controls.Add(this.txtBPPMCharges);
		this.panel51.Controls.Add(this.label21);
		this.panel51.Controls.Add(this.txtAmountRM);
		this.panel51.Controls.Add(this.txtItemQtyFG);
		this.panel51.Location = new System.Drawing.Point(4, 3);
		this.panel51.Name = "panel51";
		this.panel51.Size = new System.Drawing.Size(426, 171);
		this.panel51.TabIndex = 13;
		this.panel52.BackColor = System.Drawing.Color.Teal;
		this.panel52.Controls.Add(this.label22);
		this.panel52.Controls.Add(this.label25);
		this.panel52.Controls.Add(this.label14);
		this.panel52.Controls.Add(this.label31);
		this.panel52.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel52.Location = new System.Drawing.Point(0, 0);
		this.panel52.Name = "panel52";
		this.panel52.Size = new System.Drawing.Size(424, 24);
		this.panel52.TabIndex = 0;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.White;
		this.label22.Location = new System.Drawing.Point(2, 4);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(79, 17);
		this.label22.TabIndex = 5;
		this.label22.Text = "Description";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.ForeColor = System.Drawing.Color.White;
		this.label25.Location = new System.Drawing.Point(113, 4);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(58, 17);
		this.label25.TabIndex = 2;
		this.label25.Text = "ItemQty";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(198, 4);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(75, 17);
		this.label14.TabIndex = 4;
		this.label14.Text = "NetWeight";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.ForeColor = System.Drawing.Color.White;
		this.label31.Location = new System.Drawing.Point(312, 4);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(58, 17);
		this.label31.TabIndex = 4;
		this.label31.Text = "Amount";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(2, 41);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(75, 15);
		this.label17.TabIndex = 1;
		this.label17.Text = "Raw Material";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(2, 73);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(95, 15);
		this.label18.TabIndex = 1;
		this.label18.Text = "By Product - Exp";
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label59.Location = new System.Drawing.Point(2, 105);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(95, 15);
		this.label59.TabIndex = 1;
		this.label59.Text = "BP+OH+PM Exp";
		this.txtItemQtyRM.BackColor = System.Drawing.Color.White;
		this.txtItemQtyRM.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemQtyRM.Location = new System.Drawing.Point(109, 37);
		this.txtItemQtyRM.Name = "txtItemQtyRM";
		this.txtItemQtyRM.ReadOnly = true;
		this.txtItemQtyRM.Size = new System.Drawing.Size(82, 23);
		this.txtItemQtyRM.TabIndex = 0;
		this.txtItemQtyBP.BackColor = System.Drawing.Color.White;
		this.txtItemQtyBP.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemQtyBP.Location = new System.Drawing.Point(109, 69);
		this.txtItemQtyBP.Name = "txtItemQtyBP";
		this.txtItemQtyBP.ReadOnly = true;
		this.txtItemQtyBP.Size = new System.Drawing.Size(82, 23);
		this.txtItemQtyBP.TabIndex = 3;
		this.txtBPOverHead.BackColor = System.Drawing.Color.White;
		this.txtBPOverHead.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBPOverHead.Location = new System.Drawing.Point(109, 101);
		this.txtBPOverHead.Name = "txtBPOverHead";
		this.txtBPOverHead.ReadOnly = true;
		this.txtBPOverHead.Size = new System.Drawing.Size(82, 23);
		this.txtBPOverHead.TabIndex = 6;
		this.txtNetWeightRM.BackColor = System.Drawing.Color.White;
		this.txtNetWeightRM.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeightRM.Location = new System.Drawing.Point(195, 37);
		this.txtNetWeightRM.Name = "txtNetWeightRM";
		this.txtNetWeightRM.ReadOnly = true;
		this.txtNetWeightRM.Size = new System.Drawing.Size(107, 23);
		this.txtNetWeightRM.TabIndex = 1;
		this.txtAmountFG.BackColor = System.Drawing.Color.White;
		this.txtAmountFG.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmountFG.Location = new System.Drawing.Point(305, 133);
		this.txtAmountFG.Name = "txtAmountFG";
		this.txtAmountFG.ReadOnly = true;
		this.txtAmountFG.Size = new System.Drawing.Size(116, 23);
		this.txtAmountFG.TabIndex = 11;
		this.txtBPTotalAmountWithExpenses.BackColor = System.Drawing.Color.White;
		this.txtBPTotalAmountWithExpenses.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBPTotalAmountWithExpenses.Location = new System.Drawing.Point(305, 101);
		this.txtBPTotalAmountWithExpenses.Name = "txtBPTotalAmountWithExpenses";
		this.txtBPTotalAmountWithExpenses.ReadOnly = true;
		this.txtBPTotalAmountWithExpenses.Size = new System.Drawing.Size(116, 23);
		this.txtBPTotalAmountWithExpenses.TabIndex = 8;
		this.txtNetWeightFG.BackColor = System.Drawing.Color.White;
		this.txtNetWeightFG.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeightFG.Location = new System.Drawing.Point(195, 133);
		this.txtNetWeightFG.Name = "txtNetWeightFG";
		this.txtNetWeightFG.ReadOnly = true;
		this.txtNetWeightFG.Size = new System.Drawing.Size(107, 23);
		this.txtNetWeightFG.TabIndex = 10;
		this.txtNetWeightBP.BackColor = System.Drawing.Color.White;
		this.txtNetWeightBP.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeightBP.Location = new System.Drawing.Point(195, 69);
		this.txtNetWeightBP.Name = "txtNetWeightBP";
		this.txtNetWeightBP.ReadOnly = true;
		this.txtNetWeightBP.Size = new System.Drawing.Size(107, 23);
		this.txtNetWeightBP.TabIndex = 4;
		this.txtAmountBP.BackColor = System.Drawing.Color.White;
		this.txtAmountBP.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmountBP.Location = new System.Drawing.Point(305, 69);
		this.txtAmountBP.Name = "txtAmountBP";
		this.txtAmountBP.ReadOnly = true;
		this.txtAmountBP.Size = new System.Drawing.Size(116, 23);
		this.txtAmountBP.TabIndex = 5;
		this.txtBPPMCharges.BackColor = System.Drawing.Color.White;
		this.txtBPPMCharges.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBPPMCharges.Location = new System.Drawing.Point(195, 101);
		this.txtBPPMCharges.Name = "txtBPPMCharges";
		this.txtBPPMCharges.ReadOnly = true;
		this.txtBPPMCharges.Size = new System.Drawing.Size(107, 23);
		this.txtBPPMCharges.TabIndex = 7;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(2, 137);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(75, 15);
		this.label21.TabIndex = 1;
		this.label21.Text = "Finish Goods";
		this.txtAmountRM.BackColor = System.Drawing.Color.White;
		this.txtAmountRM.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmountRM.Location = new System.Drawing.Point(305, 37);
		this.txtAmountRM.Name = "txtAmountRM";
		this.txtAmountRM.ReadOnly = true;
		this.txtAmountRM.Size = new System.Drawing.Size(116, 23);
		this.txtAmountRM.TabIndex = 2;
		this.txtItemQtyFG.BackColor = System.Drawing.Color.White;
		this.txtItemQtyFG.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemQtyFG.Location = new System.Drawing.Point(109, 133);
		this.txtItemQtyFG.Name = "txtItemQtyFG";
		this.txtItemQtyFG.ReadOnly = true;
		this.txtItemQtyFG.Size = new System.Drawing.Size(82, 23);
		this.txtItemQtyFG.TabIndex = 9;
		this.panelDifferenceAmounts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panelDifferenceAmounts.Controls.Add(this.txtDifferenceAmount);
		this.panelDifferenceAmounts.Controls.Add(this.label26);
		this.panelDifferenceAmounts.Controls.Add(this.panel75);
		this.panelDifferenceAmounts.Controls.Add(this.txtFinishAmountAgainstJobOrder);
		this.panelDifferenceAmounts.Controls.Add(this.txtActualFinishGoodsAmount);
		this.panelDifferenceAmounts.Controls.Add(this.label29);
		this.panelDifferenceAmounts.Controls.Add(this.label51);
		this.panelDifferenceAmounts.Location = new System.Drawing.Point(715, 3);
		this.panelDifferenceAmounts.Name = "panelDifferenceAmounts";
		this.panelDifferenceAmounts.Size = new System.Drawing.Size(260, 171);
		this.panelDifferenceAmounts.TabIndex = 12;
		this.txtDifferenceAmount.BackColor = System.Drawing.Color.White;
		this.txtDifferenceAmount.Font = new System.Drawing.Font("Verdana", 11f);
		this.txtDifferenceAmount.Location = new System.Drawing.Point(8, 137);
		this.txtDifferenceAmount.Name = "txtDifferenceAmount";
		this.txtDifferenceAmount.ReadOnly = true;
		this.txtDifferenceAmount.Size = new System.Drawing.Size(235, 25);
		this.txtDifferenceAmount.TabIndex = 2;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5f, System.Drawing.FontStyle.Bold);
		this.label26.Location = new System.Drawing.Point(8, 118);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(122, 17);
		this.label26.TabIndex = 13;
		this.label26.Text = "Difference Amount";
		this.panel75.BackColor = System.Drawing.Color.Teal;
		this.panel75.Controls.Add(this.label28);
		this.panel75.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel75.Location = new System.Drawing.Point(0, 0);
		this.panel75.Name = "panel75";
		this.panel75.Size = new System.Drawing.Size(258, 20);
		this.panel75.TabIndex = 11;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.ForeColor = System.Drawing.Color.White;
		this.label28.Location = new System.Drawing.Point(6, 0);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(72, 17);
		this.label28.TabIndex = 6;
		this.label28.Text = "Difference";
		this.txtFinishAmountAgainstJobOrder.BackColor = System.Drawing.Color.White;
		this.txtFinishAmountAgainstJobOrder.Font = new System.Drawing.Font("Verdana", 11f);
		this.txtFinishAmountAgainstJobOrder.Location = new System.Drawing.Point(8, 91);
		this.txtFinishAmountAgainstJobOrder.Name = "txtFinishAmountAgainstJobOrder";
		this.txtFinishAmountAgainstJobOrder.ReadOnly = true;
		this.txtFinishAmountAgainstJobOrder.Size = new System.Drawing.Size(235, 25);
		this.txtFinishAmountAgainstJobOrder.TabIndex = 1;
		this.txtActualFinishGoodsAmount.BackColor = System.Drawing.Color.White;
		this.txtActualFinishGoodsAmount.Font = new System.Drawing.Font("Verdana", 11f);
		this.txtActualFinishGoodsAmount.Location = new System.Drawing.Point(8, 42);
		this.txtActualFinishGoodsAmount.Name = "txtActualFinishGoodsAmount";
		this.txtActualFinishGoodsAmount.ReadOnly = true;
		this.txtActualFinishGoodsAmount.Size = new System.Drawing.Size(235, 25);
		this.txtActualFinishGoodsAmount.TabIndex = 0;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5f, System.Drawing.FontStyle.Bold);
		this.label29.Location = new System.Drawing.Point(8, 24);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(139, 17);
		this.label29.TabIndex = 1;
		this.label29.Text = "Actual Finish Amount";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5f, System.Drawing.FontStyle.Bold);
		this.label51.Location = new System.Drawing.Point(8, 72);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(240, 17);
		this.label51.TabIndex = 1;
		this.label51.Text = "Finish Amount against Job Order Rate";
		this.panel55.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel55.Controls.Add(this.panel56);
		this.panel55.Controls.Add(this.txtAvgRateFG);
		this.panel55.Controls.Add(this.label94);
		this.panel55.Controls.Add(this.txtFinishGoodRateWithOutExp);
		this.panel55.Controls.Add(this.label114);
		this.panel55.Location = new System.Drawing.Point(436, 89);
		this.panel55.Name = "panel55";
		this.panel55.Size = new System.Drawing.Size(274, 85);
		this.panel55.TabIndex = 11;
		this.panel56.BackColor = System.Drawing.Color.Teal;
		this.panel56.Controls.Add(this.label58);
		this.panel56.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel56.Location = new System.Drawing.Point(0, 0);
		this.panel56.Name = "panel56";
		this.panel56.Size = new System.Drawing.Size(272, 20);
		this.panel56.TabIndex = 12;
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.ForeColor = System.Drawing.Color.White;
		this.label58.Location = new System.Drawing.Point(6, 1);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(197, 17);
		this.label58.TabIndex = 6;
		this.label58.Text = "Finish Good Average Rate / Kg";
		this.txtAvgRateFG.BackColor = System.Drawing.Color.White;
		this.txtAvgRateFG.Font = new System.Drawing.Font("Verdana", 11.75f);
		this.txtAvgRateFG.Location = new System.Drawing.Point(117, 53);
		this.txtAvgRateFG.Name = "txtAvgRateFG";
		this.txtAvgRateFG.ReadOnly = true;
		this.txtAvgRateFG.Size = new System.Drawing.Size(150, 27);
		this.txtAvgRateFG.TabIndex = 1;
		this.label94.AutoSize = true;
		this.label94.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label94.Location = new System.Drawing.Point(4, 26);
		this.label94.Name = "label94";
		this.label94.Size = new System.Drawing.Size(90, 19);
		this.label94.TabIndex = 8;
		this.label94.Text = "WithOut Exp";
		this.txtFinishGoodRateWithOutExp.BackColor = System.Drawing.Color.White;
		this.txtFinishGoodRateWithOutExp.Font = new System.Drawing.Font("Verdana", 11.75f);
		this.txtFinishGoodRateWithOutExp.Location = new System.Drawing.Point(117, 22);
		this.txtFinishGoodRateWithOutExp.Name = "txtFinishGoodRateWithOutExp";
		this.txtFinishGoodRateWithOutExp.ReadOnly = true;
		this.txtFinishGoodRateWithOutExp.Size = new System.Drawing.Size(150, 27);
		this.txtFinishGoodRateWithOutExp.TabIndex = 0;
		this.label114.AutoSize = true;
		this.label114.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label114.Location = new System.Drawing.Point(4, 57);
		this.label114.Name = "label114";
		this.label114.Size = new System.Drawing.Size(66, 19);
		this.label114.TabIndex = 8;
		this.label114.Text = "With Exp";
		this.panel53.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel53.Controls.Add(this.panel54);
		this.panel53.Controls.Add(this.txtOverHeadsSettlement);
		this.panel53.Controls.Add(this.txtPackingMaterialSettlement);
		this.panel53.Controls.Add(this.label116);
		this.panel53.Controls.Add(this.label117);
		this.panel53.Location = new System.Drawing.Point(436, 3);
		this.panel53.Name = "panel53";
		this.panel53.Size = new System.Drawing.Size(274, 85);
		this.panel53.TabIndex = 10;
		this.panel54.BackColor = System.Drawing.Color.Teal;
		this.panel54.Controls.Add(this.label115);
		this.panel54.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel54.Location = new System.Drawing.Point(0, 0);
		this.panel54.Name = "panel54";
		this.panel54.Size = new System.Drawing.Size(272, 20);
		this.panel54.TabIndex = 11;
		this.label115.AutoSize = true;
		this.label115.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label115.ForeColor = System.Drawing.Color.White;
		this.label115.Location = new System.Drawing.Point(6, 0);
		this.label115.Name = "label115";
		this.label115.Size = new System.Drawing.Size(233, 17);
		this.label115.TabIndex = 6;
		this.label115.Text = "OverHeads && Packing Mateial Totals";
		this.txtOverHeadsSettlement.BackColor = System.Drawing.Color.White;
		this.txtOverHeadsSettlement.Font = new System.Drawing.Font("Verdana", 11.75f);
		this.txtOverHeadsSettlement.Location = new System.Drawing.Point(117, 23);
		this.txtOverHeadsSettlement.Name = "txtOverHeadsSettlement";
		this.txtOverHeadsSettlement.ReadOnly = true;
		this.txtOverHeadsSettlement.Size = new System.Drawing.Size(150, 27);
		this.txtOverHeadsSettlement.TabIndex = 0;
		this.txtPackingMaterialSettlement.BackColor = System.Drawing.Color.White;
		this.txtPackingMaterialSettlement.Font = new System.Drawing.Font("Verdana", 11.75f);
		this.txtPackingMaterialSettlement.Location = new System.Drawing.Point(117, 54);
		this.txtPackingMaterialSettlement.Name = "txtPackingMaterialSettlement";
		this.txtPackingMaterialSettlement.ReadOnly = true;
		this.txtPackingMaterialSettlement.Size = new System.Drawing.Size(150, 27);
		this.txtPackingMaterialSettlement.TabIndex = 1;
		this.label116.AutoSize = true;
		this.label116.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label116.Location = new System.Drawing.Point(5, 27);
		this.label116.Name = "label116";
		this.label116.Size = new System.Drawing.Size(81, 19);
		this.label116.TabIndex = 1;
		this.label116.Text = "Over Heads";
		this.label117.AutoSize = true;
		this.label117.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label117.Location = new System.Drawing.Point(5, 58);
		this.label117.Name = "label117";
		this.label117.Size = new System.Drawing.Size(114, 19);
		this.label117.TabIndex = 1;
		this.label117.Text = "Packing Material";
		this.panel34.BackColor = System.Drawing.Color.Teal;
		this.panel34.Controls.Add(this.btnAllocateExportInvoiceToJobOrder);
		this.panel34.Controls.Add(this.label171);
		this.panel34.Controls.Add(this.label7);
		this.panel34.Controls.Add(this.BtnByProductSettlementPopUp);
		this.panel34.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel34.Location = new System.Drawing.Point(0, 255);
		this.panel34.Name = "panel34";
		this.panel34.Size = new System.Drawing.Size(1164, 22);
		this.panel34.TabIndex = 6;
		this.btnAllocateExportInvoiceToJobOrder.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.btnAllocateExportInvoiceToJobOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAllocateExportInvoiceToJobOrder.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnAllocateExportInvoiceToJobOrder.ForeColor = System.Drawing.Color.White;
		this.btnAllocateExportInvoiceToJobOrder.Location = new System.Drawing.Point(206, 0);
		this.btnAllocateExportInvoiceToJobOrder.Margin = new System.Windows.Forms.Padding(2);
		this.btnAllocateExportInvoiceToJobOrder.Name = "btnAllocateExportInvoiceToJobOrder";
		this.btnAllocateExportInvoiceToJobOrder.Size = new System.Drawing.Size(259, 22);
		this.btnAllocateExportInvoiceToJobOrder.TabIndex = 5;
		this.btnAllocateExportInvoiceToJobOrder.Text = "Export Invoice Allocate To JobOrder";
		this.btnAllocateExportInvoiceToJobOrder.UseVisualStyleBackColor = false;
		this.btnAllocateExportInvoiceToJobOrder.Click += new System.EventHandler(btnAllocateExportInvoiceToJobOrder_Click);
		this.label171.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label171.AutoSize = true;
		this.label171.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label171.ForeColor = System.Drawing.Color.White;
		this.label171.Location = new System.Drawing.Point(755, 4);
		this.label171.Name = "label171";
		this.label171.Size = new System.Drawing.Size(237, 15);
		this.label171.TabIndex = 2;
		this.label171.Text = "Click Update By-Product Rates Item Wise";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(3, 4);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(188, 15);
		this.label7.TabIndex = 2;
		this.label7.Text = "OutPut By-Product & Finish Goods";
		this.BtnByProductSettlementPopUp.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnByProductSettlementPopUp.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnByProductSettlementPopUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnByProductSettlementPopUp.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnByProductSettlementPopUp.ForeColor = System.Drawing.Color.White;
		this.BtnByProductSettlementPopUp.Location = new System.Drawing.Point(998, 0);
		this.BtnByProductSettlementPopUp.Margin = new System.Windows.Forms.Padding(2);
		this.BtnByProductSettlementPopUp.Name = "BtnByProductSettlementPopUp";
		this.BtnByProductSettlementPopUp.Size = new System.Drawing.Size(165, 22);
		this.BtnByProductSettlementPopUp.TabIndex = 4;
		this.BtnByProductSettlementPopUp.Text = "BY-Product Item Wise";
		this.BtnByProductSettlementPopUp.UseVisualStyleBackColor = false;
		this.BtnByProductSettlementPopUp.Click += new System.EventHandler(BtnByProductSettlementPopUp_Click);
		((ScrollableBase)this.grdInputsettlement).BackColor = System.Drawing.Color.White;
		grdInputsettlement_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInputsettlement.DesignTimeLayout = grdInputsettlement_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInputsettlement).Dock = System.Windows.Forms.DockStyle.Top;
		((System.Windows.Forms.Control)(object)this.grdInputsettlement).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInputsettlement.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInputsettlement).Location = new System.Drawing.Point(0, 85);
		((System.Windows.Forms.Control)(object)this.grdInputsettlement).Name = "grdInputsettlement";
		((ScrollableBase)this.grdInputsettlement).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInputsettlement).Size = new System.Drawing.Size(1164, 170);
		((System.Windows.Forms.Control)(object)this.grdInputsettlement).TabIndex = 1;
		this.grdInputsettlement.TotalRow = (InheritableBoolean)1;
		this.grdInputsettlement.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInputsettlement).VisualStyle = (VisualStyle)4;
		this.panel49.BackColor = System.Drawing.Color.Teal;
		this.panel49.Controls.Add(this.label5);
		this.panel49.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel49.Location = new System.Drawing.Point(0, 64);
		this.panel49.Name = "panel49";
		this.panel49.Size = new System.Drawing.Size(1164, 21);
		this.panel49.TabIndex = 2;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(3, 3);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(131, 16);
		this.label5.TabIndex = 1;
		this.label5.Text = "Input Raw Material";
		this.panel32.BackColor = System.Drawing.Color.Transparent;
		this.panel32.Controls.Add(this.txtSettlementDate);
		this.panel32.Controls.Add(this.chkIncludeWages);
		this.panel32.Controls.Add(this.chkAllJobOrderOnSettlement);
		this.panel32.Controls.Add(this.btnSettlementVoucher);
		this.panel32.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDifferenceAccountTitle);
		this.panel32.Controls.Add(this.label113);
		this.panel32.Controls.Add(this.BtnPrint615A);
		this.panel32.Controls.Add(this.btn615Report);
		this.panel32.Controls.Add(this.btnApproveSettlement);
		this.panel32.Controls.Add(this.BtnPrint613B);
		this.panel32.Controls.Add(this.BtnPrint613A);
		this.panel32.Controls.Add(this.btn613SummaryReport);
		this.panel32.Controls.Add(this.btnRecoveryReport);
		this.panel32.Controls.Add(this.btnUpdateSettlement);
		this.panel32.Controls.Add(this.btnApplyAvgRateFG);
		this.panel32.Controls.Add(this.btnResetSettlement);
		this.panel32.Controls.Add(this.btnGeneralSettlement);
		this.panel32.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobOrderSettlement);
		this.panel32.Controls.Add(this.label169);
		this.panel32.Controls.Add(this.label12);
		this.panel32.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel32.Location = new System.Drawing.Point(0, 0);
		this.panel32.Name = "panel32";
		this.panel32.Size = new System.Drawing.Size(1164, 64);
		this.panel32.TabIndex = 0;
		this.txtSettlementDate.CustomFormat = "dd-MMM-yy";
		this.txtSettlementDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSettlementDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtSettlementDate.Location = new System.Drawing.Point(424, 34);
		this.txtSettlementDate.Name = "txtSettlementDate";
		this.txtSettlementDate.Size = new System.Drawing.Size(123, 23);
		this.txtSettlementDate.TabIndex = 3;
		this.chkIncludeWages.AutoSize = true;
		this.chkIncludeWages.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
		this.chkIncludeWages.Checked = true;
		this.chkIncludeWages.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkIncludeWages.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkIncludeWages.Location = new System.Drawing.Point(1105, 3);
		this.chkIncludeWages.Name = "chkIncludeWages";
		this.chkIncludeWages.Size = new System.Drawing.Size(64, 57);
		this.chkIncludeWages.TabIndex = 1;
		this.chkIncludeWages.Text = "Include \r\nWages \r\nIn Print";
		this.chkIncludeWages.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.chkIncludeWages.UseVisualStyleBackColor = true;
		this.chkIncludeWages.CheckedChanged += new System.EventHandler(chkAllJobOrderOnSettlement_CheckedChanged);
		this.chkAllJobOrderOnSettlement.AutoSize = true;
		this.chkAllJobOrderOnSettlement.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkAllJobOrderOnSettlement.Location = new System.Drawing.Point(121, 8);
		this.chkAllJobOrderOnSettlement.Name = "chkAllJobOrderOnSettlement";
		this.chkAllJobOrderOnSettlement.Size = new System.Drawing.Size(112, 21);
		this.chkAllJobOrderOnSettlement.TabIndex = 1;
		this.chkAllJobOrderOnSettlement.Text = "All Job Orders";
		this.chkAllJobOrderOnSettlement.UseVisualStyleBackColor = true;
		this.chkAllJobOrderOnSettlement.CheckedChanged += new System.EventHandler(chkAllJobOrderOnSettlement_CheckedChanged);
		this.btnSettlementVoucher.BackColor = System.Drawing.Color.Teal;
		this.btnSettlementVoucher.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnSettlementVoucher.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnSettlementVoucher.ForeColor = System.Drawing.Color.White;
		this.btnSettlementVoucher.Location = new System.Drawing.Point(708, 32);
		this.btnSettlementVoucher.Margin = new System.Windows.Forms.Padding(2);
		this.btnSettlementVoucher.Name = "btnSettlementVoucher";
		this.btnSettlementVoucher.Size = new System.Drawing.Size(75, 26);
		this.btnSettlementVoucher.TabIndex = 9;
		this.btnSettlementVoucher.Text = "Voucher";
		this.btnSettlementVoucher.UseVisualStyleBackColor = false;
		this.btnSettlementVoucher.Click += new System.EventHandler(btnSettlementVoucher_Click);
		this.CmbDifferenceAccountTitle.AllowNull = (DefaultableBoolean)1;
		this.CmbDifferenceAccountTitle.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDifferenceAccountTitle.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDifferenceAccountTitle.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.Appearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance214;
		((SpecialBoxBase)((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance191).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance191).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance192).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance193).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance157).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance158).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance158).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance158).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance159).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDifferenceAccountTitle).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDifferenceAccountTitle).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDifferenceAccountTitle.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDifferenceAccountTitle).Location = new System.Drawing.Point(237, 32);
		((System.Windows.Forms.Control)(object)this.CmbDifferenceAccountTitle).Name = "CmbDifferenceAccountTitle";
		((System.Windows.Forms.Control)(object)this.CmbDifferenceAccountTitle).Size = new System.Drawing.Size(185, 26);
		((System.Windows.Forms.Control)(object)this.CmbDifferenceAccountTitle).TabIndex = 2;
		this.label113.AutoSize = true;
		this.label113.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label113.ForeColor = System.Drawing.Color.Black;
		this.label113.Location = new System.Drawing.Point(234, 11);
		this.label113.Name = "label113";
		this.label113.Size = new System.Drawing.Size(117, 15);
		this.label113.TabIndex = 190;
		this.label113.Text = "Difference Account";
		this.BtnPrint615A.BackColor = System.Drawing.Color.Teal;
		this.BtnPrint615A.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnPrint615A.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnPrint615A.ForeColor = System.Drawing.Color.White;
		this.BtnPrint615A.Location = new System.Drawing.Point(997, 32);
		this.BtnPrint615A.Margin = new System.Windows.Forms.Padding(2);
		this.BtnPrint615A.Name = "BtnPrint615A";
		this.BtnPrint615A.Size = new System.Drawing.Size(105, 26);
		this.BtnPrint615A.TabIndex = 15;
		this.BtnPrint615A.Text = "615_A_Print";
		this.BtnPrint615A.UseVisualStyleBackColor = false;
		this.BtnPrint615A.Click += new System.EventHandler(BtnPrint615A_Click);
		this.btn615Report.BackColor = System.Drawing.Color.Teal;
		this.btn615Report.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn615Report.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btn615Report.ForeColor = System.Drawing.Color.White;
		this.btn615Report.Location = new System.Drawing.Point(997, 4);
		this.btn615Report.Margin = new System.Windows.Forms.Padding(2);
		this.btn615Report.Name = "btn615Report";
		this.btn615Report.Size = new System.Drawing.Size(105, 26);
		this.btn615Report.TabIndex = 14;
		this.btn615Report.Text = "615_Print";
		this.btn615Report.UseVisualStyleBackColor = false;
		this.btn615Report.Click += new System.EventHandler(btn615Report_Click);
		this.btnApproveSettlement.BackColor = System.Drawing.Color.Teal;
		this.btnApproveSettlement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnApproveSettlement.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnApproveSettlement.ForeColor = System.Drawing.Color.White;
		this.btnApproveSettlement.Location = new System.Drawing.Point(633, 32);
		this.btnApproveSettlement.Margin = new System.Windows.Forms.Padding(2);
		this.btnApproveSettlement.Name = "btnApproveSettlement";
		this.btnApproveSettlement.Size = new System.Drawing.Size(75, 26);
		this.btnApproveSettlement.TabIndex = 7;
		this.btnApproveSettlement.Text = "Approve";
		this.btnApproveSettlement.UseVisualStyleBackColor = false;
		this.btnApproveSettlement.Visible = false;
		this.btnApproveSettlement.Click += new System.EventHandler(btnApproveSettlement_Click);
		this.BtnPrint613B.BackColor = System.Drawing.Color.Teal;
		this.BtnPrint613B.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnPrint613B.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnPrint613B.ForeColor = System.Drawing.Color.White;
		this.BtnPrint613B.Location = new System.Drawing.Point(892, 32);
		this.BtnPrint613B.Margin = new System.Windows.Forms.Padding(2);
		this.BtnPrint613B.Name = "BtnPrint613B";
		this.BtnPrint613B.Size = new System.Drawing.Size(105, 26);
		this.BtnPrint613B.TabIndex = 13;
		this.BtnPrint613B.Text = "613_B_Print";
		this.BtnPrint613B.UseVisualStyleBackColor = false;
		this.BtnPrint613B.Click += new System.EventHandler(BtnPrint613B_Click);
		this.BtnPrint613A.BackColor = System.Drawing.Color.Teal;
		this.BtnPrint613A.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnPrint613A.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnPrint613A.ForeColor = System.Drawing.Color.White;
		this.BtnPrint613A.Location = new System.Drawing.Point(892, 4);
		this.BtnPrint613A.Margin = new System.Windows.Forms.Padding(2);
		this.BtnPrint613A.Name = "BtnPrint613A";
		this.BtnPrint613A.Size = new System.Drawing.Size(105, 26);
		this.BtnPrint613A.TabIndex = 12;
		this.BtnPrint613A.Text = "613_A_Print";
		this.BtnPrint613A.UseVisualStyleBackColor = false;
		this.BtnPrint613A.Click += new System.EventHandler(BtnPrint613A_Click);
		this.btn613SummaryReport.BackColor = System.Drawing.Color.Teal;
		this.btn613SummaryReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btn613SummaryReport.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btn613SummaryReport.ForeColor = System.Drawing.Color.White;
		this.btn613SummaryReport.Location = new System.Drawing.Point(792, 32);
		this.btn613SummaryReport.Margin = new System.Windows.Forms.Padding(2);
		this.btn613SummaryReport.Name = "btn613SummaryReport";
		this.btn613SummaryReport.Size = new System.Drawing.Size(100, 26);
		this.btn613SummaryReport.TabIndex = 11;
		this.btn613SummaryReport.Text = "613_Print";
		this.btn613SummaryReport.UseVisualStyleBackColor = false;
		this.btn613SummaryReport.Click += new System.EventHandler(btn613SummaryReport_Click);
		this.btnRecoveryReport.BackColor = System.Drawing.Color.Teal;
		this.btnRecoveryReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnRecoveryReport.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnRecoveryReport.ForeColor = System.Drawing.Color.White;
		this.btnRecoveryReport.Location = new System.Drawing.Point(792, 4);
		this.btnRecoveryReport.Margin = new System.Windows.Forms.Padding(2);
		this.btnRecoveryReport.Name = "btnRecoveryReport";
		this.btnRecoveryReport.Size = new System.Drawing.Size(100, 26);
		this.btnRecoveryReport.TabIndex = 10;
		this.btnRecoveryReport.Text = "602A_Print";
		this.btnRecoveryReport.UseVisualStyleBackColor = false;
		this.btnRecoveryReport.Click += new System.EventHandler(btnRecoveryReport_Click);
		this.btnUpdateSettlement.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateSettlement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateSettlement.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnUpdateSettlement.ForeColor = System.Drawing.Color.White;
		this.btnUpdateSettlement.Location = new System.Drawing.Point(633, 4);
		this.btnUpdateSettlement.Margin = new System.Windows.Forms.Padding(2);
		this.btnUpdateSettlement.Name = "btnUpdateSettlement";
		this.btnUpdateSettlement.Size = new System.Drawing.Size(75, 26);
		this.btnUpdateSettlement.TabIndex = 6;
		this.btnUpdateSettlement.Text = "Update";
		this.btnUpdateSettlement.UseVisualStyleBackColor = false;
		this.btnUpdateSettlement.Click += new System.EventHandler(btnUpdateSettlement_Click);
		this.btnApplyAvgRateFG.BackColor = System.Drawing.Color.Teal;
		this.btnApplyAvgRateFG.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnApplyAvgRateFG.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnApplyAvgRateFG.ForeColor = System.Drawing.Color.White;
		this.btnApplyAvgRateFG.Location = new System.Drawing.Point(549, 32);
		this.btnApplyAvgRateFG.Margin = new System.Windows.Forms.Padding(2);
		this.btnApplyAvgRateFG.Name = "btnApplyAvgRateFG";
		this.btnApplyAvgRateFG.Size = new System.Drawing.Size(85, 26);
		this.btnApplyAvgRateFG.TabIndex = 5;
		this.btnApplyAvgRateFG.Text = "ApplyRate";
		this.btnApplyAvgRateFG.UseVisualStyleBackColor = false;
		this.btnApplyAvgRateFG.Click += new System.EventHandler(btnApplyAvgRateFG_Click);
		this.btnResetSettlement.BackColor = System.Drawing.Color.Teal;
		this.btnResetSettlement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnResetSettlement.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnResetSettlement.ForeColor = System.Drawing.Color.White;
		this.btnResetSettlement.Location = new System.Drawing.Point(708, 4);
		this.btnResetSettlement.Margin = new System.Windows.Forms.Padding(2);
		this.btnResetSettlement.Name = "btnResetSettlement";
		this.btnResetSettlement.Size = new System.Drawing.Size(75, 26);
		this.btnResetSettlement.TabIndex = 8;
		this.btnResetSettlement.Text = "Reset";
		this.btnResetSettlement.UseVisualStyleBackColor = false;
		this.btnResetSettlement.Click += new System.EventHandler(btnResetSettlement_Click);
		this.btnGeneralSettlement.BackColor = System.Drawing.Color.Teal;
		this.btnGeneralSettlement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnGeneralSettlement.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnGeneralSettlement.ForeColor = System.Drawing.Color.White;
		this.btnGeneralSettlement.Location = new System.Drawing.Point(549, 4);
		this.btnGeneralSettlement.Margin = new System.Windows.Forms.Padding(2);
		this.btnGeneralSettlement.Name = "btnGeneralSettlement";
		this.btnGeneralSettlement.Size = new System.Drawing.Size(85, 26);
		this.btnGeneralSettlement.TabIndex = 4;
		this.btnGeneralSettlement.Text = "Generate";
		this.btnGeneralSettlement.UseVisualStyleBackColor = false;
		this.btnGeneralSettlement.Click += new System.EventHandler(btnGeneralSettlement_Click);
		this.CmbJobOrderSettlement.AllowNull = (DefaultableBoolean)1;
		this.CmbJobOrderSettlement.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobOrderSettlement.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobOrderSettlement.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance218).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.Appearance = (AppearanceBase)(object)appearance218;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance219).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance219).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance219).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance219).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance219;
		((AppearanceBase)appearance220).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance220;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance221).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance221).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance221).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance222).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance222;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance223).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance223;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance224;
		((AppearanceBase)appearance225).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance225).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance226).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance226).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance226).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance226).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance226;
		((AppearanceBase)appearance227).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance227;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance228).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance228;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobOrderSettlement).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderSettlement).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobOrderSettlement.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderSettlement).Location = new System.Drawing.Point(8, 32);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderSettlement).Name = "CmbJobOrderSettlement";
		((System.Windows.Forms.Control)(object)this.CmbJobOrderSettlement).Size = new System.Drawing.Size(227, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderSettlement).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderSettlement).Leave += new System.EventHandler(CmbJobOrderSettlement_Leave);
		this.label169.AutoSize = true;
		this.label169.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label169.ForeColor = System.Drawing.Color.Black;
		this.label169.Location = new System.Drawing.Point(421, 11);
		this.label169.Name = "label169";
		this.label169.Size = new System.Drawing.Size(101, 15);
		this.label169.TabIndex = 119;
		this.label169.Text = "Settlement Date";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.Black;
		this.label12.Location = new System.Drawing.Point(8, 11);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(81, 15);
		this.label12.TabIndex = 119;
		this.label12.Text = "Job Order No";
		this.miniToolStrip.AccessibleName = "New item selection";
		this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
		this.miniToolStrip.AutoSize = false;
		this.miniToolStrip.BackColor = System.Drawing.Color.White;
		this.miniToolStrip.CanOverflow = false;
		this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.miniToolStrip.Location = new System.Drawing.Point(148, 4);
		this.miniToolStrip.Name = "miniToolStrip";
		this.miniToolStrip.Size = new System.Drawing.Size(1356, 27);
		this.miniToolStrip.TabIndex = 5;
		this.CmbBrandNamePackingMaterial.AllowNull = (DefaultableBoolean)1;
		this.CmbBrandNamePackingMaterial.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrandNamePackingMaterial.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrandNamePackingMaterial.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Appearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance207).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance207).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance207).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance207;
		((AppearanceBase)appearance208).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance208;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance209).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance209).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance209).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance210).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance210;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance211).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance212;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance150).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance151).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance151).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance151).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance215).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance216).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrandNamePackingMaterial).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrandNamePackingMaterial).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrandNamePackingMaterial.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrandNamePackingMaterial).Location = new System.Drawing.Point(667, 36);
		((System.Windows.Forms.Control)(object)this.CmbBrandNamePackingMaterial).Name = "CmbBrandNamePackingMaterial";
		((System.Windows.Forms.Control)(object)this.CmbBrandNamePackingMaterial).Size = new System.Drawing.Size(185, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrandNamePackingMaterial).TabIndex = 3;
		this.CmbBrandUomPackingMaterial.AllowNull = (DefaultableBoolean)1;
		this.CmbBrandUomPackingMaterial.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrandUomPackingMaterial.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrandUomPackingMaterial.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Appearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance180).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance180).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance180;
		((AppearanceBase)appearance181).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance181;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance182).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance182).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance182).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance183).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance183;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance184).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance185;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance186).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance187).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance187).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance187).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance187).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance187;
		((AppearanceBase)appearance188).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance189).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrandUomPackingMaterial).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrandUomPackingMaterial).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrandUomPackingMaterial.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrandUomPackingMaterial).Location = new System.Drawing.Point(855, 36);
		((System.Windows.Forms.Control)(object)this.CmbBrandUomPackingMaterial).Name = "CmbBrandUomPackingMaterial";
		((System.Windows.Forms.Control)(object)this.CmbBrandUomPackingMaterial).Size = new System.Drawing.Size(77, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrandUomPackingMaterial).TabIndex = 4;
		this.cmbItemPackingMaterial.AllowNull = (DefaultableBoolean)1;
		this.cmbItemPackingMaterial.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemPackingMaterial.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemPackingMaterial.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Appearance = (AppearanceBase)(object)appearance194;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance195).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance195).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance195).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance195;
		((AppearanceBase)appearance196).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance196;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance197).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance197).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance197).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance198).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance198;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance199).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance200;
		((AppearanceBase)appearance201).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance201).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance202).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance202).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance202).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance202).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance202;
		((AppearanceBase)appearance203).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance204).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance205;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemPackingMaterial).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemPackingMaterial.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemPackingMaterial).Location = new System.Drawing.Point(420, 36);
		((System.Windows.Forms.Control)(object)this.cmbItemPackingMaterial).Name = "cmbItemPackingMaterial";
		((System.Windows.Forms.Control)(object)this.cmbItemPackingMaterial).Size = new System.Drawing.Size(240, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemPackingMaterial).TabIndex = 2;
		this.CmbWarehousePackingMaterial.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehousePackingMaterial.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehousePackingMaterial.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehousePackingMaterial.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Appearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance168).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance168).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance168;
		((AppearanceBase)appearance169).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance169;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance170).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance171;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance172).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance174).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance175).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance175).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance175;
		((AppearanceBase)appearance176).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance177).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehousePackingMaterial).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehousePackingMaterial).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWarehousePackingMaterial.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehousePackingMaterial).Location = new System.Drawing.Point(231, 36);
		((System.Windows.Forms.Control)(object)this.CmbWarehousePackingMaterial).Name = "CmbWarehousePackingMaterial";
		((System.Windows.Forms.Control)(object)this.CmbWarehousePackingMaterial).Size = new System.Drawing.Size(183, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehousePackingMaterial).TabIndex = 1;
		this.CmbPlantFeederPackingMaterial.AllowNull = (DefaultableBoolean)1;
		this.CmbPlantFeederPackingMaterial.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPlantFeederPackingMaterial.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPlantFeederPackingMaterial.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.Appearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance155;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance156).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance156).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance162).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance162).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance163).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance163).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance163).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPlantFeederPackingMaterial).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederPackingMaterial).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPlantFeederPackingMaterial.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederPackingMaterial).Location = new System.Drawing.Point(6, 36);
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederPackingMaterial).Name = "CmbPlantFeederPackingMaterial";
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederPackingMaterial).Size = new System.Drawing.Size(219, 26);
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederPackingMaterial).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederPackingMaterial).Leave += new System.EventHandler(CmbPlantFeederPackingMaterial_Leave);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(1164, 739);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmProductionSettlement";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(FoodProductionWithValues_FormClosing);
		base.Load += new System.EventHandler(frmFoodProduction_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmFoodProduction_KeyDown);
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOutPutSettlement).EndInit();
		this.panel36.ResumeLayout(false);
		this.panel39.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPackingMaterialSettlement).EndInit();
		this.panel38.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOverHeadSettlement).EndInit();
		this.panel37.ResumeLayout(false);
		this.panel37.PerformLayout();
		this.panel40.ResumeLayout(false);
		this.panel51.ResumeLayout(false);
		this.panel51.PerformLayout();
		this.panel52.ResumeLayout(false);
		this.panel52.PerformLayout();
		this.panelDifferenceAmounts.ResumeLayout(false);
		this.panelDifferenceAmounts.PerformLayout();
		this.panel75.ResumeLayout(false);
		this.panel75.PerformLayout();
		this.panel55.ResumeLayout(false);
		this.panel55.PerformLayout();
		this.panel56.ResumeLayout(false);
		this.panel56.PerformLayout();
		this.panel53.ResumeLayout(false);
		this.panel53.PerformLayout();
		this.panel54.ResumeLayout(false);
		this.panel54.PerformLayout();
		this.panel34.ResumeLayout(false);
		this.panel34.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInputsettlement).EndInit();
		this.panel49.ResumeLayout(false);
		this.panel49.PerformLayout();
		this.panel32.ResumeLayout(false);
		this.panel32.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDifferenceAccountTitle).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderSettlement).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandNamePackingMaterial).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandUomPackingMaterial).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemPackingMaterial).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehousePackingMaterial).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantFeederPackingMaterial).EndInit();
		base.ResumeLayout(false);
	}
}
