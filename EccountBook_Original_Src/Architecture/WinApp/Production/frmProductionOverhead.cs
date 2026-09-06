using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.Production;
using Architecture.BLL.Reports.Production;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Production;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Production;

public class frmProductionOverhead : Form
{
	private bool UpdateMode;

	private int RecOverHeadId;

	private int VoucherHeadId;

	private int updateDetailIndexOH = -1;

	private bool FIFOCGSFlag;

	private DataTable tableOverHeads = new DataTable();

	private DataTable OverHeadHistorydt = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	public TabControl tabControl1;

	private TabPage tabPage2;

	private Panel panel68;

	private Panel panel31;

	internal GridEX grdDetailOverhead;

	private Panel panel25;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label47;

	private Panel panel9;

	private GroupBox groupBox7;

	private RadioButton RadOutputItemOH;

	private RadioButton RadInputItemOH;

	private Label label99;

	internal UltraCombo cmbRateUomOverHeads;

	private Button btnAddOH;

	private Button BtnCancelOH;

	private Button btnUpdateOH;

	internal UltraCombo CmbPlantFeederOverHead;

	private Label lblRateOverHead;

	private Label label95;

	internal UltraCombo cmbChargestypeOverheads;

	private Label lblRemarksOverHead;

	private Label label85;

	private TextBox txtLedgerRemarksOverHead;

	private TextBox txtItemRateOverHeads;

	private Label label42;

	private Label label3;

	internal UltraCombo cmbChartofAccountOverHead;

	private Label label57;

	private TextBox txtItemQtyOverHead;

	internal UltraCombo CmbBrandNameOverHead;

	internal UltraCombo CmbBrandUomOverHead;

	private Label lblAmountOverHead;

	private TextBox txtExpAmountOverHead;

	private GroupBox groupBox6;

	private Label label102;

	private TextBox txtfinishQtyOh;

	private Label label103;

	private TextBox txtfinishWeightOh;

	private Label label97;

	private TextBox txtInputQtyTotalOH;

	private Label label98;

	private TextBox txtInputWeightTotalOH;

	private Label label38;

	private TextBox txtOutputQtyTotalOH;

	private Label label46;

	private TextBox txtOutputWeightTotalOH;

	internal UltraCombo cmbJobOrderOverHead;

	private TextBox txtDocNoOverHead;

	private Label label56;

	private DateTimePicker txtDocDateOverHead;

	private Label label39;

	private Panel panel69;

	private Label label44;

	private ToolStrip toolStrip5;

	private ToolStripButton btnNewOverHead;

	private ToolStripButton btnRefreshOverHead;

	private ToolStripButton btnSaveOverHead;

	private ToolStripButton btnUpdateOverHead;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripButton btnPrintOverHead;

	private ToolStripButton btnVoucherOH;

	private ToolStripButton BtnLoadFohOverHeads;

	private ToolStripButton btnOverHeadFormulas;

	private TabPage tabPage3;

	internal GridEX grddetailOhHistory;

	private Panel panel27;

	private CtrlGrdBar ctrlGrdBar3;

	private Label label34;

	internal GridEX grdOverHeadHistory;

	private Panel panel8;

	private Label label101;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label89;

	private ToolStrip miniToolStrip;

	public frmProductionOverhead(UserAccount user)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
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
			if (clsGlobalVariables.ScreenViewReights.Any((tblUserRights dd) => dd.ScreenName == "FoodProductionWithValues"))
			{
				formright = CommonServices.SetRightsValueInRightsObject("FoodProductionWithValues");
			}
			else
			{
				formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			}
			btnSaveOverHead.Enabled = formright.DoHaveSaveRight;
			btnUpdateOH.Enabled = formright.DoHaveUpdateRights;
			btnPrintOverHead.Enabled = formright.DoHavePrintRights;
			toolStrip5.Enabled = formright.DoHavePrintRights;
			btnVoucherOH.Enabled = formright.DoHavePrintRights;
			FIFOCGSFlag = CommonServices.GetERPFeatureById(5);
			tableOverHeads.Columns.Add("Id", typeof(int));
			tableOverHeads.Columns.Add("DocDate", typeof(DateTime));
			tableOverHeads.Columns.Add("DocNo", typeof(int));
			tableOverHeads.Columns.Add("InvProductionJobOrderId", typeof(int));
			tableOverHeads.Columns.Add("PlantId", typeof(int));
			tableOverHeads.Columns.Add("Plant", typeof(string));
			tableOverHeads.Columns.Add("ChargesTypeId", typeof(int));
			tableOverHeads.Columns.Add("ChargesType", typeof(string));
			tableOverHeads.Columns.Add("ChartOfAccountId", typeof(int));
			tableOverHeads.Columns.Add("ChartOfAccount", typeof(string));
			tableOverHeads.Columns.Add("BrandId", typeof(int));
			tableOverHeads.Columns.Add("Brand", typeof(string));
			tableOverHeads.Columns.Add("BrandUomId", typeof(int));
			tableOverHeads.Columns.Add("BrandUom", typeof(string));
			tableOverHeads.Columns.Add("ItemQty", typeof(double));
			tableOverHeads.Columns.Add("ItemRate", typeof(double));
			tableOverHeads.Columns.Add("RateUomId", typeof(int));
			tableOverHeads.Columns.Add("RateUom", typeof(string));
			tableOverHeads.Columns.Add("Amount", typeof(double));
			tableOverHeads.Columns.Add("Remarks", typeof(string));
			tableOverHeads.Columns.Add("DetailDocumentTypeId", typeof(int));
			grdDetailOverhead.DataSource = tableOverHeads;
			grdDetailOverhead.RetrieveStructure();
			grdDetailOverheadSettings();
			GenerateCodeofOverHead();
			ChartOfAccountOverHead();
			UOMOverHead();
			ChargestypeOverHeadsOverHead();
			JobOrderNoFill();
			btnAddOH.Visible = true;
			btnUpdateOH.Visible = false;
			BtnCancelOH.Visible = false;
			if (!formright.DoHaveCanRateandAmount)
			{
				btnVoucherOH.Visible = false;
			}
			txtDocDateOverHead.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetBrandItemsForPackingAndOverHeads(int InvJobOrderId, int ActionId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			InvFoodProduction obj = new InvFoodProduction();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.InvJobOrderId = InvJobOrderId;
			obj.ActionId = ActionId;
			if (ActionId == 0)
			{
				obj.DocumentTypeId = (RadInputItemOH.Checked ? 80 : 112);
			}
			DataTable lst = InvFoodProduction.GetBrandItemsForPackingAndOverHeads(obj);
			if (lst.Rows.Count > 0)
			{
				DDL.BindDDL(lst, CmbBrandNameOverHead, "ItemId", "ItemName", "ItemName", ZeroIndex: false);
				return;
			}
			((Control)(object)CmbBrandNameOverHead).Text = string.Empty;
			((UltraGridBase)CmbBrandNameOverHead).DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetItemUomByItemIdFromFinishGoods(int InvJobOrderId, int ItemId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		try
		{
			DataTable dt = InvFoodProduction.GetItemUomByItemIdFromFinishGoods(new InvFoodProduction
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				InvJobOrderId = InvJobOrderId,
				WIPItemId = ItemId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbBrandUomOverHead, "ItemUomId", "Equivalent", "PackUom", ZeroIndex: false);
				return;
			}
			((Control)(object)CmbBrandUomOverHead).Text = string.Empty;
			((UltraGridBase)CmbBrandUomOverHead).DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JobOrderNoFill()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		try
		{
			int jobOhId = Conversion.ToInt(cmbJobOrderOverHead.Value);
			DataTable dtPJO = InvProductionJobOrder.JobOrdersForProduction(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId
			});
			if (dtPJO.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("PlanCode");
				dt.Columns.Add("DocumentTypeId", typeof(int));
				for (int i = 0; i < dtPJO.Rows.Count; i++)
				{
					dt.Rows.Add(dtPJO.Rows[i]["Id"], dtPJO.Rows[i]["PlanCode"], dtPJO.Rows[i]["DocumentTypeId"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbJobOrderOverHead, "Id", "PlanCode", "JobOrderNo", ZeroIndex: false);
					((UltraGridBase)cmbJobOrderOverHead).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				}
				if (jobOhId > 0)
				{
					cmbJobOrderOverHead.Value = jobOhId;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetPlantFeeder(int JobOrderId)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		try
		{
			int JobOHId = Conversion.ToInt(CmbPlantFeederOverHead.Value);
			DataTable dt = InvProductionJobOrder.PlantsForProductionByJobOrderId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				JobOrderId = JobOrderId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbPlantFeederOverHead, "PlantId", "PlantName", "Plant Name", false);
				if (dt.Rows.Count == 1)
				{
					((UltraGridBase)CmbPlantFeederOverHead).Rows[0].Activate();
				}
				if (JobOHId > 0)
				{
					CmbPlantFeederOverHead.Value = JobOHId;
				}
			}
			else
			{
				((Control)(object)CmbPlantFeederOverHead).Text = string.Empty;
				((UltraGridBase)CmbPlantFeederOverHead).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GetVoucherHeadId(int Id, int DocumentTypeId)
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
			if (tabControl1.SelectedIndex == 0)
			{
				if (btnSaveOverHead.Visible && btnSaveOverHead.Enabled && e.Control && e.KeyCode == Keys.S)
				{
					btnSaveOverHead_Click(null, null);
				}
				if (btnUpdateOverHead.Visible && btnUpdateOverHead.Enabled && e.Control && e.KeyCode == Keys.U)
				{
					btnUpdateOverHead_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNewOverHead_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefreshOverHead_Click(null, null);
				}
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

	private void CaluculatonOfOverHeads()
	{
		try
		{
			if (Conversion.ToInt(cmbChargestypeOverheads.Value) == 1)
			{
				if (Conversion.ToInt(CmbBrandNameOverHead.Value) > 0 && ((UltraGridBase)CmbBrandNameOverHead).ActiveRow != null && Conversion.ToInt(CmbBrandUomOverHead.Value) > 0 && ((UltraGridBase)CmbBrandUomOverHead).ActiveRow != null)
				{
					if (txtItemQtyOverHead.Text != string.Empty && txtItemRateOverHeads.Text != string.Empty)
					{
						double Amount2 = double.Parse(txtItemQtyOverHead.Text) * double.Parse(txtItemRateOverHeads.Text);
						txtExpAmountOverHead.Text = Math.Round(Amount2).ToString();
					}
				}
				else if (txtItemQtyOverHead.Text != string.Empty && txtItemRateOverHeads.Text != string.Empty && ((UltraGridBase)cmbRateUomOverHeads).ActiveRow != null)
				{
					double num = double.Parse(txtInputWeightTotalOH.Text);
					double Equivalent = Conversion.ToDouble(((UltraDropDownBase)cmbRateUomOverHeads).SelectedRow.Cells[2].Value);
					double Rate = double.Parse(txtItemRateOverHeads.Text);
					double fAmount = num / Equivalent * Rate;
					txtExpAmountOverHead.Text = Math.Round(fAmount).ToString();
				}
			}
			else if (txtItemQtyOverHead.Text != string.Empty && txtItemRateOverHeads.Text != string.Empty)
			{
				double Amount = double.Parse(txtItemQtyOverHead.Text) * double.Parse(txtItemRateOverHeads.Text);
				txtExpAmountOverHead.Text = Math.Round(Amount).ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtItemQtyOverHead_TextChanged(object sender, EventArgs e)
	{
		CaluculatonOfOverHeads();
	}

	private void txtItemRateOverHeads_TextChanged(object sender, EventArgs e)
	{
		CaluculatonOfOverHeads();
	}

	private void GenerateCodeofOverHead()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		try
		{
			int code = InvFoodProductionOverHeads.GenerateCode(new InvFoodProduction
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (code > 0)
			{
				txtDocNoOverHead.Text = code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ChargestypeOverHeadsOverHead()
	{
		try
		{
			DataTable dtChargestype = new DataTable();
			dtChargestype.Columns.Add("Id");
			dtChargestype.Columns.Add("Type");
			dtChargestype.Rows.Add(1, "Processing Charges");
			dtChargestype.Rows.Add(2, "Other Charges");
			if (dtChargestype.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtChargestype, cmbChargestypeOverheads, "Id", "Type", "Charges Type", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ChartOfAccountOverHead()
	{
		try
		{
			DataTable coaList = CommonServices.CoaAllocationGetAllServiceBind();
			if (coaList.Rows.Count <= 0)
			{
				return;
			}
			HashSet<int> allowedTypes = new HashSet<int> { 6, 8, 11, 12, 13, 14, 20, 21 };
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("AccountTitle");
			foreach (DataRow row in coaList.Rows)
			{
				int accountTypeId = Conversion.ToInt(row["AccountTypeId"]);
				int plNoteId = Conversion.ToInt(row["PLNoteId"]);
				if (allowedTypes.Contains(accountTypeId) || (accountTypeId == 10 && plNoteId == 3))
				{
					dt.Rows.Add(row["Id"], row["AccountTitle"]);
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbChartofAccountOverHead, "Id", "AccountTitle", "Account Title", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UOMOverHead()
	{
		try
		{
			DataTable dt = UOM.UOMStaticAll();
			if (dt.Rows.Count > 0)
			{
				DataTable dtUom = new DataTable();
				dtUom.Columns.Add("Id");
				dtUom.Columns.Add("UOM");
				dtUom.Columns.Add("Equivalent");
				dtUom.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtUom.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["UOMCode"], dt.Rows[i]["Equivalent"]);
				}
				DDL.BindDDL(dtUom, cmbRateUomOverHeads, "Id", "UOM", "Rate UOM", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbJobOrderOverHead_Leave(object sender, EventArgs e)
	{
		try
		{
			GetBrandItemsForPackingAndOverHeads(Conversion.ToInt(cmbJobOrderOverHead.Value));
			GetPlantFeeder(Conversion.ToInt(cmbJobOrderOverHead.Value));
			GetTotlInPutOut();
			CaluculatonOfOverHeads();
			RefreshPlantInOhGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetTotlInPutOut()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			DataTable dt = InvFoodProduction.GetInPutTotalQtyandWeightByJobOrderId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				JobOrderId = Conversion.ToInt(cmbJobOrderOverHead.Value)
			});
			if (dt.Rows.Count > 0)
			{
				double InQty = 0.0;
				double InWeight = 0.0;
				double OutQty = 0.0;
				double OutWeight = 0.0;
				double FinsishOutQty = 0.0;
				double FinishOutWeight = 0.0;
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["EntryTypeOutPut"]) == "FinishGoods")
					{
						FinsishOutQty += Conversion.ToDouble(dt.Rows[i]["ItemQty"]);
						FinishOutWeight += Conversion.ToDouble(dt.Rows[i]["StockWeight"]);
					}
					if (Conversion.ToString(dt.Rows[i]["EntryType"]) == "Input")
					{
						InQty += Conversion.ToDouble(dt.Rows[i]["ItemQty"]);
						InWeight += Conversion.ToDouble(dt.Rows[i]["StockWeight"]);
					}
					else
					{
						OutQty += Conversion.ToDouble(dt.Rows[i]["ItemQty"]);
						OutWeight += Conversion.ToDouble(dt.Rows[i]["StockWeight"]);
					}
				}
				txtInputQtyTotalOH.Text = InQty.ToString("#,#");
				txtInputWeightTotalOH.Text = InWeight.ToString("#,#");
				txtOutputQtyTotalOH.Text = OutQty.ToString("#,#");
				txtOutputWeightTotalOH.Text = OutWeight.ToString("#,#");
				txtfinishQtyOh.Text = FinsishOutQty.ToString("#,#");
				txtfinishWeightOh.Text = FinishOutWeight.ToString("#,#");
			}
			else
			{
				txtInputQtyTotalOH.Text = "0";
				txtInputWeightTotalOH.Text = "0";
				txtOutputQtyTotalOH.Text = "0";
				txtOutputWeightTotalOH.Text = "0";
				txtfinishQtyOh.Text = "0";
				txtfinishWeightOh.Text = "0";
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbBrandNameOverHead_Leave(object sender, EventArgs e)
	{
		try
		{
			GetItemUomByItemIdFromFinishGoods(Conversion.ToInt(cmbJobOrderOverHead.Value), Conversion.ToInt(CmbBrandNameOverHead.Value));
			GetOutPutQtyByJobOrderandItemIdOverHeads();
			CaluculatonOfOverHeads();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetOutPutQtyByJobOrderandItemIdOverHeads()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Expected O, but got Unknown
		try
		{
			DataTable dt = InvFoodProduction.GetOutPutQtyWeightFoodProductionForPmOh(new InvFoodProductionPackingMaterial
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				InvProductionJobOrderId = Conversion.ToInt(cmbJobOrderOverHead.Value),
				BrandId = Conversion.ToInt(CmbBrandNameOverHead.Value),
				BrandUomId = Conversion.ToInt(CmbBrandUomOverHead.Value),
				DocumentTypeId = (RadInputItemOH.Checked ? 80 : 112)
			});
			if (dt.Rows.Count > 0)
			{
				double OutQty = (RadInputItemOH.Checked ? Conversion.ToDouble(dt.Compute("SUM(BalanceWeight)", string.Empty)) : Conversion.ToDouble(dt.Compute("SUM(ItemQty)", string.Empty)));
				txtItemQtyOverHead.Text = OutQty.ToString("0,0");
			}
			else
			{
				txtItemQtyOverHead.Text = "";
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbBrandUomOverHead_Leave(object sender, EventArgs e)
	{
		GetOutPutQtyByJobOrderandItemIdOverHeads();
		CaluculatonOfOverHeads();
	}

	private void OverHeadInsert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		try
		{
			if (!FormValidationOfOverHead())
			{
				return;
			}
			InvFoodProductionOverHeads obj = new InvFoodProductionOverHeads();
			obj.InvFoodProductionOverHeadsList = new List<InvFoodProductionOverHeads>();
			if (RecOverHeadId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				obj.Id = RecOverHeadId;
			}
			else
			{
				if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				GenerateCodeofOverHead();
			}
			if (grdDetailOverhead.RowCount > 0)
			{
				GridEXRow[] rows = grdDetailOverhead.GetRows();
				foreach (GridEXRow r in rows)
				{
					InvFoodProductionOverHeads OverHeaddetail = new InvFoodProductionOverHeads();
					if (Conversion.ToInt(r.Cells["ItemQty"].Value) <= 0 && !(Conversion.ToDouble(r.Cells["ItemRate"].Value) > 0.0) && !(Conversion.ToDouble(r.Cells["Amount"].Value) > 0.0))
					{
						continue;
					}
					OverHeaddetail.OrganizationId = UserAccount.OrganizationId;
					OverHeaddetail.CompanyId = UserAccount.CompanyId;
					OverHeaddetail.BranchesId = UserAccount.BranchesId;
					OverHeaddetail.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
					OverHeaddetail.DocumentTypeId = 110;
					OverHeaddetail.EntryDate = DateTime.Now;
					OverHeaddetail.EntryUser = UserAccount.ID;
					OverHeaddetail.ModifyDate = DateTime.Now;
					OverHeaddetail.ModifyUser = UserAccount.ID;
					OverHeaddetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
					OverHeaddetail.DocNo = Conversion.ToInt(r.Cells["DocNo"].Value);
					OverHeaddetail.DocDate = Conversion.ToDateTime(r.Cells["DocDate"].Value);
					OverHeaddetail.InvProductionJobOrderId = Conversion.ToInt(cmbJobOrderOverHead.Value);
					if (Conversion.ToInt(((UltraDropDownBase)cmbJobOrderOverHead).SelectedRow.Cells["DocumentTypeId"].Value) == 401)
					{
						OverHeaddetail.BaseDocumentTypeId = 2;
					}
					OverHeaddetail.PlantId = Conversion.ToInt(r.Cells["PlantId"].Value);
					OverHeaddetail.ChargesTypeId = Conversion.ToInt(r.Cells["ChargesTypeId"].Value);
					OverHeaddetail.CharOfAccountId = Conversion.ToInt(r.Cells["ChartOfAccountId"].Value);
					OverHeaddetail.BrandId = Conversion.ToInt(r.Cells["BrandId"].Value);
					OverHeaddetail.BrandUomId = Conversion.ToInt(r.Cells["BrandUomId"].Value);
					OverHeaddetail.ItemQty = Conversion.ToDouble(r.Cells["ItemQty"].Value);
					OverHeaddetail.ItemRate = Conversion.ToDouble(r.Cells["ItemRate"].Value);
					OverHeaddetail.RateUomId = Conversion.ToInt(r.Cells["RateUomId"].Value);
					OverHeaddetail.Amount = Conversion.ToDouble(r.Cells["Amount"].Value);
					OverHeaddetail.ohRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
					OverHeaddetail.DetailDocumentTypeId = Conversion.ToInt(r.Cells["DetailDocumentTypeId"].Value);
					if (OverHeaddetail.PlantId == 0)
					{
						MessageBox.Show("Plant Field Required in Detail");
						return;
					}
					if (OverHeaddetail.ChargesTypeId == 0)
					{
						MessageBox.Show("ChargesType Field Require in Detail");
						return;
					}
					if (OverHeaddetail.CharOfAccountId == 0)
					{
						MessageBox.Show("OverHead Account Field Require in Detail");
						return;
					}
					if (OverHeaddetail.BrandId == 0 && OverHeaddetail.BrandUomId != 0)
					{
						MessageBox.Show("OutPut Item Field Require in Detail");
						return;
					}
					if (OverHeaddetail.BrandUomId == 0 && OverHeaddetail.BrandId != 0)
					{
						MessageBox.Show("OutPut UomField Require in Detail");
						return;
					}
					if (OverHeaddetail.ChargesTypeId != 3)
					{
						if (OverHeaddetail.ItemQty == 0.0 && OverHeaddetail.ItemRate != 0.0)
						{
							MessageBox.Show("ItemQty Required in Detail");
							return;
						}
						if (OverHeaddetail.ItemRate == 0.0 && OverHeaddetail.ItemQty != 0.0)
						{
							MessageBox.Show("ItemRate Required in Detail");
							return;
						}
						if (OverHeaddetail.RateUomId == 0 && OverHeaddetail.BrandId == 0 && OverHeaddetail.BrandUomId == 0 && OverHeaddetail.ChargesTypeId == 1)
						{
							MessageBox.Show("RateUom Required in Detail");
							return;
						}
					}
					if (OverHeaddetail.Amount == 0.0)
					{
						MessageBox.Show("Amount Required in Detail");
						return;
					}
					obj.InvFoodProductionOverHeadsList.Add(OverHeaddetail);
				}
			}
			if (obj.InvFoodProductionOverHeadsList.Count == 0)
			{
				throw new Exception("Grid Record not found please check!");
			}
			InvFoodProductionOverHeads.Save(obj);
			if (RecOverHeadId > 0)
			{
				MessageBox.Show("Update Successfully");
			}
			else
			{
				MessageBox.Show("Save Successfully");
			}
			FormResetOverHead();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSaveOverHead_Click(object sender, EventArgs e)
	{
		try
		{
			RecOverHeadId = 0;
			OverHeadInsert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateOverHead_Click(object sender, EventArgs e)
	{
		try
		{
			OverHeadInsert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadByIdOverHeadJobOrderWise(int JobOrderID)
	{
		try
		{
			btnSaveOverHead.Visible = false;
			btnUpdateOverHead.Visible = true;
			DataTable dtSC = InvFoodProductionOverHeads.GetByJobOrderID(JobOrderID);
			if (dtSC != null && dtSC.Rows.Count > 0)
			{
				if (Conversion.ToString(dtSC.Rows[0]["JobOrderStatus"]) == "Approved")
				{
					throw new Exception("You Can't Update Approved Record");
				}
				if (Conversion.ToString(dtSC.Rows[0]["PlanStatus"]) != "In Process")
				{
					throw new Exception("You Can't Update Record with " + Conversion.ToString(dtSC.Rows[0]["PlanStatus"]) + " JobPlanStatus");
				}
				tabControl1.SelectedIndex = 0;
				txtDocNoOverHead.Text = Conversion.ToInt(dtSC.Rows[0]["DocNo"]).ToString();
				txtDocDateOverHead.Value = Conversion.ToDateTime(dtSC.Rows[0]["DocDate"]);
				RecOverHeadId = Conversion.ToInt(dtSC.Rows[0]["InvProductionJobOrderId"]);
				cmbJobOrderOverHead.Value = Conversion.ToInt(dtSC.Rows[0]["InvProductionJobOrderId"]);
				cmbJobOrderOverHead_Leave(null, null);
				tableOverHeads.Rows.Clear();
				if (tableOverHeads.Columns.Count == 0)
				{
					tableOverHeads.Columns.Add("Id", typeof(int));
					tableOverHeads.Columns.Add("DocDate", typeof(DateTime));
					tableOverHeads.Columns.Add("DocNo", typeof(int));
					tableOverHeads.Columns.Add("InvProductionJobOrderId", typeof(int));
					tableOverHeads.Columns.Add("PlantId", typeof(int));
					tableOverHeads.Columns.Add("Plant", typeof(string));
					tableOverHeads.Columns.Add("ChargesTypeId", typeof(int));
					tableOverHeads.Columns.Add("ChargesType", typeof(string));
					tableOverHeads.Columns.Add("ChartOfAccountId", typeof(int));
					tableOverHeads.Columns.Add("ChartOfAccount", typeof(string));
					tableOverHeads.Columns.Add("BrandId", typeof(int));
					tableOverHeads.Columns.Add("Brand", typeof(string));
					tableOverHeads.Columns.Add("BrandUomId", typeof(int));
					tableOverHeads.Columns.Add("BrandUom", typeof(string));
					tableOverHeads.Columns.Add("ItemQty", typeof(double));
					tableOverHeads.Columns.Add("ItemRate", typeof(double));
					tableOverHeads.Columns.Add("RateUomId", typeof(int));
					tableOverHeads.Columns.Add("RateUom", typeof(string));
					tableOverHeads.Columns.Add("Amount", typeof(double));
					tableOverHeads.Columns.Add("Remarks", typeof(string));
					tableOverHeads.Columns.Add("DetailDocumentTypeId", typeof(int));
				}
				for (int i = 0; i < dtSC.Rows.Count; i++)
				{
					tableOverHeads.Rows.Add(dtSC.Rows[i]["Id"], Conversion.ToDateTime(dtSC.Rows[i]["DocDate"]).ToShortDateString(), dtSC.Rows[i]["DocNo"].ToString(), dtSC.Rows[i]["InvProductionJobOrderId"].ToString(), dtSC.Rows[i]["PlantId"], dtSC.Rows[i]["PlantName"], dtSC.Rows[i]["ChargesTypeId"], dtSC.Rows[i]["ChargesType"], dtSC.Rows[i]["CharOfAccountId"], dtSC.Rows[i]["AccountTitle"], dtSC.Rows[i]["BrandId"], dtSC.Rows[i]["BrandName"], dtSC.Rows[i]["BrandUomId"], dtSC.Rows[i]["BrandUom"], dtSC.Rows[i]["ItemQty"], dtSC.Rows[i]["ItemRate"], dtSC.Rows[i]["RateUomId"], dtSC.Rows[i]["RateUom"], dtSC.Rows[i]["Amount"], dtSC.Rows[i]["ohRemarks"], dtSC.Rows[i]["DetailDocumentTypeId"]);
				}
				grdDetailOverhead.DataSource = tableOverHeads;
				grdDetailOverhead.RetrieveStructure();
				grdDetailOverheadSettings();
				GetVoucherHeadId(RecOverHeadId, 110);
				cmbChartofAccountOverHead.Focus();
				UpdateMode = true;
				RefreshPlantInOhGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshOverHead_Click(object sender, EventArgs e)
	{
		try
		{
			UOMOverHead();
			ChargestypeOverHeadsOverHead();
			JobOrderNoFill();
			ChartOfAccountOverHead();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNewOverHead_Click(object sender, EventArgs e)
	{
		FormResetOverHead();
	}

	private void FormResetOverHead()
	{
		try
		{
			RecOverHeadId = 0;
			VoucherHeadId = 0;
			btnUpdateOverHead.Visible = false;
			UpdateMode = false;
			btnSaveOverHead.Visible = true;
			((Control)(object)cmbJobOrderOverHead).Enabled = true;
			((Control)(object)CmbPlantFeederOverHead).Enabled = true;
			txtDocDateOverHead.Enabled = true;
			((Control)(object)cmbJobOrderOverHead).Text = string.Empty;
			txtOutputQtyTotalOH.Text = string.Empty;
			txtOutputWeightTotalOH.Text = string.Empty;
			txtInputQtyTotalOH.Text = string.Empty;
			txtInputWeightTotalOH.Text = string.Empty;
			((Control)(object)CmbPlantFeederOverHead).Text = string.Empty;
			tableOverHeads.Rows.Clear();
			grdDetailOverhead.DataSource = null;
			ResetdetailOverHead();
			GenerateCodeofOverHead();
			cmbJobOrderOverHead.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetdetailOverHead()
	{
		try
		{
			((Control)(object)cmbChargestypeOverheads).Text = string.Empty;
			((Control)(object)cmbChartofAccountOverHead).Text = string.Empty;
			((Control)(object)CmbBrandNameOverHead).Text = string.Empty;
			((Control)(object)CmbBrandUomOverHead).Text = string.Empty;
			txtItemQtyOverHead.Text = string.Empty;
			txtItemRateOverHeads.Text = string.Empty;
			txtExpAmountOverHead.Text = string.Empty;
			txtLedgerRemarksOverHead.Text = string.Empty;
			btnAddOH.Visible = true;
			btnUpdateOH.Visible = false;
			BtnCancelOH.Visible = false;
			CmbPlantFeederOverHead.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidationOfDetailPortionOverheads()
	{
		if (((UltraGridBase)cmbJobOrderOverHead).ActiveRow == null)
		{
			MessageBox.Show("job Order Field Required");
			cmbJobOrderOverHead.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPlantFeederOverHead).ActiveRow == null)
		{
			MessageBox.Show("Plant Field Required");
			CmbPlantFeederOverHead.Focus();
			return false;
		}
		if (((UltraGridBase)cmbChartofAccountOverHead).ActiveRow == null)
		{
			MessageBox.Show("OverHead Account Field Required");
			cmbChartofAccountOverHead.Focus();
			return false;
		}
		if (((UltraGridBase)CmbBrandNameOverHead).ActiveRow != null && ((UltraGridBase)CmbBrandUomOverHead).ActiveRow == null)
		{
			MessageBox.Show("outPut ItemUOM Field Required");
			CmbBrandUomOverHead.Focus();
			return false;
		}
		if (((UltraGridBase)cmbRateUomOverHeads).ActiveRow == null && ((UltraGridBase)CmbBrandNameOverHead).ActiveRow == null && ((UltraGridBase)CmbBrandUomOverHead).ActiveRow == null)
		{
			MessageBox.Show("RateUOM Field Required");
			cmbRateUomOverHeads.Focus();
			return false;
		}
		if (txtExpAmountOverHead.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtExpAmountOverHead.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Amount Field Required");
			txtExpAmountOverHead.Focus();
			return false;
		}
		return true;
	}

	private void btnAddOH_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationOfDetailPortionOverheads())
			{
				tableOverHeads.Rows.Add(0, Conversion.ToDateTime((object)txtDocDateOverHead.Value).ToShortDateString(), Conversion.ToInt((object)txtDocNoOverHead.Text.Trim()), Conversion.ToInt(cmbJobOrderOverHead.Value), Conversion.ToInt(CmbPlantFeederOverHead.Value), Conversion.ToString((object)((Control)(object)CmbPlantFeederOverHead).Text.Trim()), Conversion.ToInt(cmbChargestypeOverheads.Value), Conversion.ToString((object)((Control)(object)cmbChargestypeOverheads).Text.Trim()), Conversion.ToInt(cmbChartofAccountOverHead.Value), Conversion.ToString((object)((Control)(object)cmbChartofAccountOverHead).Text.Trim()), Conversion.ToInt(CmbBrandNameOverHead.Value), Conversion.ToString((object)((Control)(object)CmbBrandNameOverHead).Text.Trim()), Conversion.ToInt(CmbBrandUomOverHead.Value), Conversion.ToString((object)((Control)(object)CmbBrandUomOverHead).Text.Trim()), Conversion.ToDouble((object)txtItemQtyOverHead.Text.Trim()), Conversion.ToDouble((object)txtItemRateOverHeads.Text.Trim()), Conversion.ToInt(cmbRateUomOverHeads.Value), Conversion.ToString((object)((Control)(object)cmbRateUomOverHeads).Text.Trim()), Conversion.ToDouble((object)txtExpAmountOverHead.Text.Trim()), Conversion.ToString((object)txtLedgerRemarksOverHead.Text.Trim()), RadInputItemOH.Checked ? 80 : 112);
				grdDetailOverhead.DataSource = tableOverHeads;
				grdDetailOverhead.RetrieveStructure();
				ResetdetailOverHead();
				grdDetailOverheadSettings();
				JobOrderOverHeadLock();
				CmbPlantFeederOverHead.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JobOrderOverHeadLock()
	{
		try
		{
			if (tableOverHeads.Rows.Count > 0)
			{
				((Control)(object)cmbJobOrderOverHead).Enabled = false;
				cmbJobOrderOverHead.Value = tableOverHeads.Rows[0]["InvProductionJobOrderId"];
			}
			else
			{
				((Control)(object)cmbJobOrderOverHead).Enabled = true;
				((Control)(object)CmbPlantFeederOverHead).Enabled = true;
				txtDocDateOverHead.Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateOH_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationOfDetailPortionOverheads())
			{
				tableOverHeads.Rows[updateDetailIndexOH]["PlantId"] = Conversion.ToInt(CmbPlantFeederOverHead.Value);
				tableOverHeads.Rows[updateDetailIndexOH]["Plant"] = Conversion.ToString((object)((Control)(object)CmbPlantFeederOverHead).Text);
				tableOverHeads.Rows[updateDetailIndexOH]["ChargesTypeId"] = Conversion.ToInt(cmbChargestypeOverheads.Value);
				tableOverHeads.Rows[updateDetailIndexOH]["ChartOfAccountId"] = Conversion.ToInt(cmbChartofAccountOverHead.Value);
				tableOverHeads.Rows[updateDetailIndexOH]["ChartOfAccount"] = Conversion.ToString((object)((Control)(object)cmbChartofAccountOverHead).Text);
				tableOverHeads.Rows[updateDetailIndexOH]["BrandId"] = Conversion.ToInt(CmbBrandNameOverHead.Value);
				tableOverHeads.Rows[updateDetailIndexOH]["Brand"] = Conversion.ToString((object)((Control)(object)CmbBrandNameOverHead).Text);
				GetItemUomByItemIdFromFinishGoods(Conversion.ToInt(cmbJobOrderOverHead.Value), Conversion.ToInt(CmbBrandNameOverHead.Value));
				tableOverHeads.Rows[updateDetailIndexOH]["BrandUomId"] = Conversion.ToInt(CmbBrandUomOverHead.Value);
				tableOverHeads.Rows[updateDetailIndexOH]["BrandUom"] = Conversion.ToString((object)((Control)(object)CmbBrandUomOverHead).Text);
				tableOverHeads.Rows[updateDetailIndexOH]["ItemQty"] = Conversion.ToDouble((object)txtItemQtyOverHead.Text.Trim());
				tableOverHeads.Rows[updateDetailIndexOH]["ItemRate"] = Conversion.ToDouble((object)txtItemRateOverHeads.Text.Trim());
				tableOverHeads.Rows[updateDetailIndexOH]["RateUomId"] = Conversion.ToInt(cmbRateUomOverHeads.Value);
				tableOverHeads.Rows[updateDetailIndexOH]["RateUom"] = Conversion.ToString((object)((Control)(object)cmbRateUomOverHeads).Text);
				tableOverHeads.Rows[updateDetailIndexOH]["Amount"] = Conversion.ToDouble((object)txtExpAmountOverHead.Text.Trim());
				tableOverHeads.Rows[updateDetailIndexOH]["Remarks"] = txtLedgerRemarksOverHead.Text.Trim();
				tableOverHeads.Rows[updateDetailIndexOH]["DetailDocumentTypeId"] = (RadInputItemOH.Checked ? 80 : 112);
				ResetdetailOverHead();
				CommonServices.GridAutoAdjustmentNew(grdDetailOverhead);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetailOverhead_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdDetailOverhead.CurrentRow;
			if (item == null || ((int)item.RowType != 0 && item.Cells.Count == 0))
			{
				return;
			}
			if (Conversion.ToInt(item.Cells["ChargesTypeId"].Value) == 3)
			{
				MessageBox.Show("you can't Edit Foh Expense Row");
				return;
			}
			updateDetailIndexOH = item.RowIndex;
			CmbPlantFeederOverHead.Value = item.Cells["PlantId"].Value;
			((Control)(object)CmbPlantFeederOverHead).Text = item.Cells["Plant"].Text.ToString();
			cmbChargestypeOverheads.Value = item.Cells["ChargesTypeId"].Value;
			((Control)(object)cmbChargestypeOverheads).Text = item.Cells["ChargesType"].Text.ToString();
			cmbChartofAccountOverHead.Value = item.Cells["ChartOfAccountId"].Value;
			((Control)(object)cmbChartofAccountOverHead).Text = item.Cells["ChartOfAccount"].Text.ToString();
			if (Conversion.ToInt(item.Cells["DetailDocumentTypeId"].Value) == 80)
			{
				RadInputItemOH.Checked = true;
			}
			else
			{
				RadOutputItemOH.Checked = true;
			}
			CmbBrandNameOverHead.Value = item.Cells["BrandId"].Value;
			((Control)(object)CmbBrandNameOverHead).Text = item.Cells["Brand"].Text.ToString();
			CmbBrandNameOverHead_Leave(null, null);
			CmbBrandUomOverHead.Value = item.Cells["BrandUomId"].Value;
			((Control)(object)CmbBrandUomOverHead).Text = item.Cells["BrandUom"].Text.ToString();
			txtItemQtyOverHead.Text = item.Cells["ItemQty"].Text.ToString();
			txtItemRateOverHeads.Text = item.Cells["ItemRate"].Text.ToString();
			cmbRateUomOverHeads.Value = item.Cells["RateUomId"].Value;
			((Control)(object)cmbRateUomOverHeads).Text = item.Cells["RateUom"].Text.ToString();
			txtExpAmountOverHead.Text = item.Cells["Amount"].Text.ToString();
			txtLedgerRemarksOverHead.Text = item.Cells["Remarks"].Text.ToString();
			btnAddOH.Visible = false;
			btnUpdateOH.Visible = true;
			BtnCancelOH.Visible = true;
			CmbPlantFeederOverHead.Focus();
			JobOrderOverHeadLock();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnCancelOH_Click(object sender, EventArgs e)
	{
		ResetdetailOverHead();
	}

	private void grdDetailOverheadSettings()
	{
		try
		{
			List<string> hiddenColumns = new List<string>
			{
				"Id", "ChartOfAccountId", "PlantId", "RateUomId", "BrandUomId", "BrandId", "InvProductionJobOrderId", "ChargesTypeId", "DocDate", "DocNo",
				"DetailDocumentTypeId"
			};
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"DocDate",
					(EditType)5
				},
				{
					"Remarks",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdDetailOverhead, 2, 3, hiddenColumns, editableColumns);
			GridEX_Helper.AddButton(grdDetailOverhead, "Delete", "X", 20, 0);
			grdDetailOverhead.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetailOverhead_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				if (!btnSaveOverHead.Enabled || !btnSaveOverHead.Visible)
				{
					MessageBox.Show("You Can not Delete Record In Update Mode");
					return;
				}
				GridEXRow item = grdDetailOverhead.CurrentRow;
				tableOverHeads.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
			}
			grdDetailOverhead.UpdateData();
			JobOrderOverHeadLock();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl4_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			BindGridHistoryOverHead();
		}
	}

	private void BindGridHistoryOverHead()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			OverHeadHistorydt.Rows.Clear();
			InvFoodProduction obj = new InvFoodProduction();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			OverHeadHistorydt = InvFoodProductionOverHeads.GetAll(obj);
			if (OverHeadHistorydt.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Clear();
				dt.Columns.Add("InvProductionJobOrderId", typeof(int));
				dt.Columns.Add("ProductionJobOrder", typeof(string));
				dt.Columns.Add("TotalItemQty", typeof(double));
				dt.Columns.Add("TotalExpense", typeof(double));
				dt.Columns.Add("AverageItemRate", typeof(double));
				dt.Columns.Add("IsApproved");
				dt.Columns.Add("JobPlanStatus");
				foreach (var row2 in (from row in OverHeadHistorydt.AsEnumerable()
					group row by row["InvProductionJobOrderId"]).Select(delegate(IGrouping<object, DataRow> group)
				{
					double totalItemQty = group.Sum((DataRow row) => Conversion.ToDouble(row["ItemQty"]));
					double totalExpense = group.Sum((DataRow row) => Conversion.ToDouble(row["Amount"]));
					double averageItemRate = group.Average((DataRow row) => Conversion.ToDouble(row["ItemRate"]));
					return new
					{
						InvProductionJobOrderId = group.Key,
						ProductionJobOrder = group.First()["PlanCode"],
						TotalItemQty = totalItemQty,
						TotalExpense = totalExpense,
						AverageItemRate = averageItemRate,
						IsApproved = group.First()["JobOrderStatus"],
						JobPlanStatus = group.First()["JobPlanStatus"]
					};
				}))
				{
					dt.Rows.Add(row2.InvProductionJobOrderId, row2.ProductionJobOrder, row2.TotalItemQty, row2.TotalExpense, row2.AverageItemRate, row2.IsApproved, row2.JobPlanStatus);
				}
				grdOverHeadHistory.DataSource = dt;
				grdOverHeadHistory.RetrieveStructure();
				OverHeadGridSetting();
			}
			else
			{
				grdOverHeadHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OverHeadGridSetting()
	{
		try
		{
			grdOverHeadHistory.RootTable.Columns["InvProductionJobOrderId"].Visible = false;
			grdOverHeadHistory.RootTable.Columns["ProductionJobOrder"].EditType = (EditType)0;
			grdOverHeadHistory.RootTable.Columns["IsApproved"].EditType = (EditType)0;
			grdOverHeadHistory.RootTable.Columns["ProductionJobOrder"].FilterEditType = (FilterEditType)1;
			grdOverHeadHistory.RootTable.Columns["IsApproved"].FilterEditType = (FilterEditType)1;
			grdOverHeadHistory.RootTable.Columns["TotalExpense"].AggregateFunction = (AggregateFunction)2;
			grdOverHeadHistory.RootTable.Columns["TotalExpense"].TextAlignment = (TextAlignment)3;
			grdOverHeadHistory.RootTable.Columns["TotalExpense"].FormatString = "#,##0.###";
			grdOverHeadHistory.RootTable.Columns["TotalExpense"].TotalFormatString = "#,##0.###";
			grdOverHeadHistory.RootTable.Columns["TotalItemQty"].AggregateFunction = (AggregateFunction)2;
			grdOverHeadHistory.RootTable.Columns["TotalItemQty"].TextAlignment = (TextAlignment)3;
			grdOverHeadHistory.RootTable.Columns["TotalItemQty"].FormatString = "#,##0.####";
			grdOverHeadHistory.RootTable.Columns["TotalItemQty"].TotalFormatString = "#,##0.####";
			grdOverHeadHistory.RootTable.Columns["AverageItemRate"].AggregateFunction = (AggregateFunction)3;
			grdOverHeadHistory.RootTable.Columns["AverageItemRate"].TextAlignment = (TextAlignment)3;
			grdOverHeadHistory.RootTable.Columns["AverageItemRate"].FormatString = "#,#.##";
			grdOverHeadHistory.RootTable.Columns.Add("Edit");
			grdOverHeadHistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdOverHeadHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdOverHeadHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOverHeadHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdOverHeadHistory.RootTable.Columns["Edit"].Width = 50;
			grdOverHeadHistory.RootTable.Columns["Edit"].Position = 0;
			grdOverHeadHistory.FrozenColumns = 1;
			CommonServices.GridAutoAdjustmentNew(grdOverHeadHistory);
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOverHeadHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			btnSaveOverHead.Visible = false;
			btnUpdateOverHead.Visible = true;
			if (Conversion.ToString(grdOverHeadHistory.CurrentRow.Cells["IsApproved"].Value) == "Approved")
			{
				throw new Exception("You Can't Update Approved Record");
			}
			if (Conversion.ToString(grdOverHeadHistory.CurrentRow.Cells["JobPlanStatus"].Value) != "In Process")
			{
				throw new Exception("You Can't Update Record with " + Conversion.ToString(grdOverHeadHistory.CurrentRow.Cells["JobPlanStatus"].Value) + " JobPlanStatus");
			}
			RecOverHeadId = Conversion.ToInt((object)grdOverHeadHistory.CurrentRow.Cells["InvProductionJobOrderId"].Value.ToString());
			ReadByIdOverHeadJobOrderWise(RecOverHeadId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdOverHead_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			btnSaveOverHead.Visible = false;
			btnUpdateOverHead.Visible = true;
			if (Conversion.ToString(grdOverHeadHistory.CurrentRow.Cells["IsApproved"].Value) == "Approved")
			{
				throw new Exception("You Can't Update Approved Record");
			}
			if (Conversion.ToString(grdOverHeadHistory.CurrentRow.Cells["JobPlanStatus"].Value) != "In Process")
			{
				throw new Exception("You Can't Update Record with " + Conversion.ToString(grdOverHeadHistory.CurrentRow.Cells["JobPlanStatus"].Value) + " JobPlanStatus");
			}
			RecOverHeadId = Conversion.ToInt((object)grdOverHeadHistory.CurrentRow.Cells["InvProductionJobOrderId"].Value.ToString());
			ReadByIdOverHeadJobOrderWise(RecOverHeadId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdOverHeadHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Invalid comparison between Unknown and I4
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		GridEXRow Item = grdOverHeadHistory.CurrentRow;
		if (Item == null || (int)Item.RowType == 5 || (int)Item.RowType != 0 || Item.Cells.Count <= 0)
		{
			return;
		}
		DataTable dtDetail = InvFoodProductionOverHeads.GetByJobOrderID(Conversion.ToInt(Item.Cells["InvProductionJobOrderId"].Value));
		if (dtDetail.Rows.Count > 0)
		{
			DataTable dt = new DataTable();
			dt.Clear();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("InvProductionJobOrderId", typeof(int));
			dt.Columns.Add("DocNo", typeof(int));
			dt.Columns.Add("DocDate", typeof(DateTime));
			dt.Columns.Add("JobOrderNo", typeof(string));
			dt.Columns.Add("PlantName", typeof(string));
			dt.Columns.Add("ChargesType", typeof(string));
			dt.Columns.Add("GLAccount", typeof(string));
			dt.Columns.Add("BrandName", typeof(string));
			dt.Columns.Add("BrandUom", typeof(string));
			dt.Columns.Add("ItemQty", typeof(double));
			dt.Columns.Add("ItemRate", typeof(double));
			dt.Columns.Add("Amount", typeof(double));
			dt.Columns.Add("Remarks", typeof(string));
			dt.Columns.Add("IsApproved", typeof(string));
			for (int i = 0; i < dtDetail.Rows.Count; i++)
			{
				dt.Rows.Add(dtDetail.Rows[i]["Id"], dtDetail.Rows[i]["InvProductionJobOrderId"], dtDetail.Rows[i]["DocNo"], Conversion.ToDateTime(dtDetail.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dtDetail.Rows[i]["PlanCode"], dtDetail.Rows[i]["PlantName"], dtDetail.Rows[i]["ChargesType"], dtDetail.Rows[i]["AccountTitle"], dtDetail.Rows[i]["BrandName"], dtDetail.Rows[i]["BrandUom"], dtDetail.Rows[i]["ItemQty"], dtDetail.Rows[i]["ItemRate"], dtDetail.Rows[i]["Amount"], dtDetail.Rows[i]["ohRemarks"], dtDetail.Rows[i]["JobOrderStatus"]);
			}
			grddetailOhHistory.DataSource = dt;
			grddetailOhHistory.RetrieveStructure();
			GrddetailOhHistoryGridSetting();
		}
	}

	private void GrddetailOhHistoryGridSetting()
	{
		try
		{
			grddetailOhHistory.RootTable.Columns["Id"].Visible = false;
			grddetailOhHistory.RootTable.Columns["InvProductionJobOrderId"].Visible = false;
			grddetailOhHistory.RootTable.Columns["DocNo"].FilterEditType = (FilterEditType)1;
			grddetailOhHistory.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)6;
			grddetailOhHistory.RootTable.Columns["JobOrderNo"].FilterEditType = (FilterEditType)1;
			grddetailOhHistory.RootTable.Columns["GLAccount"].FilterEditType = (FilterEditType)1;
			grddetailOhHistory.RootTable.Columns["Remarks"].FilterEditType = (FilterEditType)1;
			grddetailOhHistory.RootTable.Columns["Amount"].FilterEditType = (FilterEditType)1;
			grddetailOhHistory.RootTable.Columns["BrandName"].FilterEditType = (FilterEditType)1;
			grddetailOhHistory.RootTable.Columns["BrandUom"].FilterEditType = (FilterEditType)1;
			grddetailOhHistory.RootTable.Columns["PlantName"].FilterEditType = (FilterEditType)1;
			grddetailOhHistory.RootTable.Columns["IsApproved"].FilterEditType = (FilterEditType)1;
			grddetailOhHistory.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grddetailOhHistory.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grddetailOhHistory.RootTable.Columns["Amount"].FormatString = "#,#";
			grddetailOhHistory.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grddetailOhHistory.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grddetailOhHistory.RootTable.Columns["ItemQty"].FormatString = "#,#";
			grddetailOhHistory.RootTable.Columns["ItemRate"].AggregateFunction = (AggregateFunction)2;
			grddetailOhHistory.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grddetailOhHistory.RootTable.Columns["ItemRate"].FormatString = "#,#.##";
			CommonServices.GridAutoAdjustmentNew(grddetailOhHistory);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidationOfOverHead()
	{
		if (txtDocNoOverHead.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Doc No. Field Required");
			txtDocNoOverHead.Focus();
			return false;
		}
		if (((UltraGridBase)cmbJobOrderOverHead).ActiveRow == null)
		{
			MessageBox.Show("Job Order No. Field Required");
			cmbJobOrderOverHead.Focus();
			return false;
		}
		return true;
	}

	private void btnVoucherOH_Click_1(object sender, EventArgs e)
	{
		try
		{
			VoucherReprot(VoucherHeadId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrintOverHead_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InvFoodProductionReports.InvFoodProductionPackingAndOverHeadReportByJobOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				JobOrderId = Conversion.ToInt(grdDetailOverhead.CurrentRow.Cells["InvProductionJobOrderId"].Value),
				ReportType = "OverHead"
			});
			if (dt.Rows.Count == 0)
			{
				MessageBox.Show("Record Not Found For DisPlay");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "606-InvFoodProductionPackingAndOverHeadReportByJobOrder.rpt");
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdDetailOverhead).Name);
			if (fs != null)
			{
				grdDetailOverhead.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdOverHeadHistory).Name);
			if (fs != null)
			{
				grdOverHeadHistory.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grddetailOhHistory).Name);
			if (fs != null)
			{
				grddetailOhHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void cmbChargestypeOverheads_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(cmbChargestypeOverheads.Value) == 1)
			{
				txtExpAmountOverHead.Enabled = false;
			}
			else
			{
				txtExpAmountOverHead.Enabled = true;
			}
			CaluculatonOfOverHeads();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbRateUomOverHeads_Leave(object sender, EventArgs e)
	{
		CaluculatonOfOverHeads();
	}

	private void txtItemQtyOverHead_Leave(object sender, EventArgs e)
	{
		txtItemQtyOverHead.Text = Conversion.ToDouble((object)txtItemQtyOverHead.Text).ToString("#,##0");
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

	private void txtDocDateOverHead_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (grdDetailOverhead.RowCount > 0)
			{
				GridEXRow[] rows = grdDetailOverhead.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					obj.Cells["DocDate"].Value = Conversion.ToDateTime((object)txtDocDateOverHead.Value);
					obj.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnOverHeadFormulas_Click(object sender, EventArgs e)
	{
		try
		{
			MakeOHformulas();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MakeOHformulas()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Formula For");
			dt.Columns.Add("Description");
			dt.Rows.Add("Amount (Processing Charges)", "if Brand Name,Brand Uom is selected and Item Qty and Item Rate has value greater than zero ---> Amount will be equal to (ItemQty * ItemRate) else if Item Qty and ItemRate has value grater than zero  and Rate Uom is selected Amount will be equal to((Total Input Weight / Rate UOm) *Item Rate)");
			dt.Rows.Add("Amount (Other Charges)", "if Item Qty and ItemRate has value grater than zero Amount will be equal to (Item Qty * Item Rate),and Amount Will be Editable");
			FormulasPopUp formulasPopUp = new FormulasPopUp(dt);
			formulasPopUp.StartPosition = FormStartPosition.CenterScreen;
			formulasPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnLoadFohOverHeads_Click(object sender, EventArgs e)
	{
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Expected O, but got Unknown
		try
		{
			if (((UltraGridBase)cmbJobOrderOverHead).ActiveRow == null || Conversion.ToInt(cmbJobOrderOverHead.Value) == 0)
			{
				MessageBox.Show("Job Order No. Field Required");
				cmbJobOrderOverHead.Focus();
				return;
			}
			if (((UltraGridBase)CmbPlantFeederOverHead).ActiveRow == null || Conversion.ToInt(CmbPlantFeederOverHead.Value) == 0)
			{
				MessageBox.Show("Plant Field Required");
				CmbPlantFeederOverHead.Focus();
				return;
			}
			DataTable dataTable = ProdcutionFohAllocateToJobOrder.ProdcutionFohAllocateToJobOrder_DataForOverHeads(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				BranchesId = clsGlobalVariables.UserAccount.BranchesId,
				JobOrderId = Conversion.ToInt(cmbJobOrderOverHead.Value),
				PlantId = Conversion.ToInt(CmbPlantFeederOverHead.Value),
				DocDate = Conversion.ToDateTime((object)txtDocDateOverHead.Value)
			});
			if (dataTable != null && dataTable.Rows.Count == 0)
			{
				throw new Exception("No Data Found For This JobOrder, Plant And DocDate");
			}
			List<string> columnsToHide = new List<string> { "AccountId", "JobOrderId", "PlantId" };
			using GrdPopUpToReturndt popup = new GrdPopUpToReturndt(dataTable, selection: true, returnFullGrdDt: false, columnsToHide)
			{
				StartPosition = FormStartPosition.CenterScreen,
				WindowState = FormWindowState.Maximized,
				Text = "Please double-click any row to Load Data."
			};
			popup.ShowDialog();
			popup.BringToFront();
			DataTable dtReturn = popup.dtReturn;
			if (dtReturn != null && dtReturn.Rows.Count > 0)
			{
				LoadInDetailOverHead(popup.dtReturn);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInDetailOverHead(DataTable dt)
	{
		try
		{
			HashSet<string> existingKeys = new HashSet<string>(from row in tableOverHeads.AsEnumerable()
				select string.Format("{0}_{1}_{2:yyyyMMdd}_{3}", row["InvProductionJobOrderId"], row["PlantId"], Convert.ToDateTime(row["DocDate"]).Date, row["ChartOfAccountId"]));
			DateTime docDate = Conversion.ToDateTime((object)txtDocDateOverHead.Value);
			int docNo = Conversion.ToInt((object)txtDocNoOverHead.Text);
			foreach (DataRow dr in dt.Rows)
			{
				string key = string.Format("{0}_{1}_{2:yyyyMMdd}_{3}", dr["JobOrderId"], dr["PlantId"], docDate.Date, dr["AccountId"]);
				if (existingKeys.Add(key))
				{
					tableOverHeads.Rows.Add(0, docDate, docNo, dr["JobOrderId"], dr["PlantId"], dr["PlantName"], 3, "FOH Expenses", dr["AccountId"], dr["AccountTitle"], 0, "", 0, "", 0, 0, 0, "", dr["ConsumedFohAmount"], "", 112);
				}
			}
			grdDetailOverhead.DataSource = tableOverHeads;
			grdDetailOverhead.RetrieveStructure();
			grdDetailOverheadSettings();
			txtDocDateOverHead.Enabled = false;
			((Control)(object)cmbJobOrderOverHead).Enabled = false;
			((Control)(object)CmbPlantFeederOverHead).Enabled = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RefreshPlantInOhGrid()
	{
		try
		{
			if (grdDetailOverhead.RowCount <= 0 || !(((UltraGridBase)CmbPlantFeederOverHead).DataSource is DataTable dtPlant) || dtPlant.Rows.Count <= 0)
			{
				return;
			}
			HashSet<int> validPlantIds = new HashSet<int>(from row in dtPlant.AsEnumerable()
				select Conversion.ToInt(row["PlantId"]));
			GridEXRow[] rows = grdDetailOverhead.GetRows();
			foreach (GridEXRow row2 in rows)
			{
				int plantId = Conversion.ToInt(row2.Cells["PlantId"].Value);
				if (!validPlantIds.Contains(plantId))
				{
					row2.BeginEdit();
					row2.Cells["PlantId"].Value = 0;
					row2.Cells["Plant"].Value = "";
					row2.EndEdit();
				}
			}
			grdDetailOverhead.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RadOutputItemOH_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			GetBrandItemsForPackingAndOverHeads(Conversion.ToInt(cmbJobOrderOverHead.Value));
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
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Expected O, but got Unknown
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Expected O, but got Unknown
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Expected O, but got Unknown
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Expected O, but got Unknown
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Expected O, but got Unknown
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Expected O, but got Unknown
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Expected O, but got Unknown
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Expected O, but got Unknown
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Expected O, but got Unknown
		//IL_0545: Unknown result type (might be due to invalid IL or missing references)
		//IL_054f: Expected O, but got Unknown
		//IL_0571: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Expected O, but got Unknown
		//IL_0a9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa7: Expected O, but got Unknown
		//IL_4bdd: Unknown result type (might be due to invalid IL or missing references)
		//IL_4be7: Expected O, but got Unknown
		GridEXLayout grdDetailOverhead_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance70 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
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
		Appearance appearance82 = new Appearance();
		Appearance appearance35 = new Appearance();
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
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance22 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Production.frmProductionOverhead));
		GridEXLayout grddetailOhHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOverHeadHistory_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel68 = new System.Windows.Forms.Panel();
		this.panel31 = new System.Windows.Forms.Panel();
		this.grdDetailOverhead = new GridEX();
		this.panel25 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label47 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.groupBox7 = new System.Windows.Forms.GroupBox();
		this.RadOutputItemOH = new System.Windows.Forms.RadioButton();
		this.RadInputItemOH = new System.Windows.Forms.RadioButton();
		this.label99 = new System.Windows.Forms.Label();
		this.cmbRateUomOverHeads = new UltraCombo();
		this.btnAddOH = new System.Windows.Forms.Button();
		this.BtnCancelOH = new System.Windows.Forms.Button();
		this.btnUpdateOH = new System.Windows.Forms.Button();
		this.CmbPlantFeederOverHead = new UltraCombo();
		this.lblRateOverHead = new System.Windows.Forms.Label();
		this.label95 = new System.Windows.Forms.Label();
		this.cmbChargestypeOverheads = new UltraCombo();
		this.lblRemarksOverHead = new System.Windows.Forms.Label();
		this.label85 = new System.Windows.Forms.Label();
		this.txtLedgerRemarksOverHead = new System.Windows.Forms.TextBox();
		this.txtItemRateOverHeads = new System.Windows.Forms.TextBox();
		this.label42 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.cmbChartofAccountOverHead = new UltraCombo();
		this.label57 = new System.Windows.Forms.Label();
		this.txtItemQtyOverHead = new System.Windows.Forms.TextBox();
		this.CmbBrandNameOverHead = new UltraCombo();
		this.CmbBrandUomOverHead = new UltraCombo();
		this.lblAmountOverHead = new System.Windows.Forms.Label();
		this.txtExpAmountOverHead = new System.Windows.Forms.TextBox();
		this.groupBox6 = new System.Windows.Forms.GroupBox();
		this.label102 = new System.Windows.Forms.Label();
		this.txtfinishQtyOh = new System.Windows.Forms.TextBox();
		this.label103 = new System.Windows.Forms.Label();
		this.txtfinishWeightOh = new System.Windows.Forms.TextBox();
		this.label97 = new System.Windows.Forms.Label();
		this.txtInputQtyTotalOH = new System.Windows.Forms.TextBox();
		this.label98 = new System.Windows.Forms.Label();
		this.txtInputWeightTotalOH = new System.Windows.Forms.TextBox();
		this.label38 = new System.Windows.Forms.Label();
		this.txtOutputQtyTotalOH = new System.Windows.Forms.TextBox();
		this.label46 = new System.Windows.Forms.Label();
		this.txtOutputWeightTotalOH = new System.Windows.Forms.TextBox();
		this.cmbJobOrderOverHead = new UltraCombo();
		this.txtDocNoOverHead = new System.Windows.Forms.TextBox();
		this.label56 = new System.Windows.Forms.Label();
		this.txtDocDateOverHead = new System.Windows.Forms.DateTimePicker();
		this.label39 = new System.Windows.Forms.Label();
		this.panel69 = new System.Windows.Forms.Panel();
		this.label44 = new System.Windows.Forms.Label();
		this.toolStrip5 = new System.Windows.Forms.ToolStrip();
		this.btnNewOverHead = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshOverHead = new System.Windows.Forms.ToolStripButton();
		this.btnSaveOverHead = new System.Windows.Forms.ToolStripButton();
		this.btnUpdateOverHead = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnPrintOverHead = new System.Windows.Forms.ToolStripButton();
		this.btnVoucherOH = new System.Windows.Forms.ToolStripButton();
		this.BtnLoadFohOverHeads = new System.Windows.Forms.ToolStripButton();
		this.btnOverHeadFormulas = new System.Windows.Forms.ToolStripButton();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.grddetailOhHistory = new GridEX();
		this.panel27 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label34 = new System.Windows.Forms.Label();
		this.grdOverHeadHistory = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label101 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label89 = new System.Windows.Forms.Label();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel68.SuspendLayout();
		this.panel31.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetailOverhead).BeginInit();
		this.panel25.SuspendLayout();
		this.panel9.SuspendLayout();
		this.groupBox7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbRateUomOverHeads).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantFeederOverHead).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbChargestypeOverheads).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbChartofAccountOverHead).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandNameOverHead).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandUomOverHead).BeginInit();
		this.groupBox6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbJobOrderOverHead).BeginInit();
		this.panel69.SuspendLayout();
		this.toolStrip5.SuspendLayout();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grddetailOhHistory).BeginInit();
		this.panel27.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOverHeadHistory).BeginInit();
		this.panel8.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1111, 639);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1111, 639);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl4_SelectedIndexChanged);
		this.tabPage2.Controls.Add(this.panel68);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1103, 610);
		this.tabPage2.TabIndex = 0;
		this.tabPage2.Text = "Form";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel68.Controls.Add(this.panel31);
		this.panel68.Controls.Add(this.panel25);
		this.panel68.Controls.Add(this.panel9);
		this.panel68.Controls.Add(this.panel69);
		this.panel68.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel68.Location = new System.Drawing.Point(3, 3);
		this.panel68.Name = "panel68";
		this.panel68.Size = new System.Drawing.Size(1097, 604);
		this.panel68.TabIndex = 0;
		this.panel31.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetailOverhead);
		this.panel31.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel31.Location = new System.Drawing.Point(0, 277);
		this.panel31.Name = "panel31";
		this.panel31.Size = new System.Drawing.Size(1097, 327);
		this.panel31.TabIndex = 5;
		this.grdDetailOverhead.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetailOverhead).BackColor = System.Drawing.Color.White;
		grdDetailOverhead_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetailOverhead.DesignTimeLayout = grdDetailOverhead_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetailOverhead).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetailOverhead.FilterMode = (FilterMode)2;
		this.grdDetailOverhead.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetailOverhead).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetailOverhead.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetailOverhead).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetailOverhead).Name = "grdDetailOverhead";
		((ScrollableBase)this.grdDetailOverhead).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetailOverhead).Size = new System.Drawing.Size(1097, 327);
		((System.Windows.Forms.Control)(object)this.grdDetailOverhead).TabIndex = 1;
		this.grdDetailOverhead.TotalRow = (InheritableBoolean)1;
		this.grdDetailOverhead.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetailOverhead).VisualStyle = (VisualStyle)4;
		this.grdDetailOverhead.ColumnButtonClick += new ColumnActionEventHandler(grdDetailOverhead_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetailOverhead).DoubleClick += new System.EventHandler(grdDetailOverhead_DoubleClick);
		this.panel25.BackColor = System.Drawing.Color.Teal;
		this.panel25.Controls.Add(this.ctrlGrdBar1);
		this.panel25.Controls.Add(this.label47);
		this.panel25.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel25.Location = new System.Drawing.Point(0, 251);
		this.panel25.Name = "panel25";
		this.panel25.Size = new System.Drawing.Size(1097, 26);
		this.panel25.TabIndex = 4;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1057, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdDetailOverhead;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar1.TabIndex = 180;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label47.ForeColor = System.Drawing.Color.White;
		this.label47.Location = new System.Drawing.Point(3, 3);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(130, 17);
		this.label47.TabIndex = 179;
		this.label47.Text = "Over Heads Detail";
		this.panel9.Controls.Add(this.groupBox7);
		this.panel9.Controls.Add(this.groupBox6);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 54);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1097, 197);
		this.panel9.TabIndex = 3;
		this.groupBox7.Controls.Add(this.RadOutputItemOH);
		this.groupBox7.Controls.Add(this.RadInputItemOH);
		this.groupBox7.Controls.Add(this.label99);
		this.groupBox7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbRateUomOverHeads);
		this.groupBox7.Controls.Add(this.btnAddOH);
		this.groupBox7.Controls.Add(this.BtnCancelOH);
		this.groupBox7.Controls.Add(this.btnUpdateOH);
		this.groupBox7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPlantFeederOverHead);
		this.groupBox7.Controls.Add(this.lblRateOverHead);
		this.groupBox7.Controls.Add(this.label95);
		this.groupBox7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbChargestypeOverheads);
		this.groupBox7.Controls.Add(this.lblRemarksOverHead);
		this.groupBox7.Controls.Add(this.label85);
		this.groupBox7.Controls.Add(this.txtLedgerRemarksOverHead);
		this.groupBox7.Controls.Add(this.txtItemRateOverHeads);
		this.groupBox7.Controls.Add(this.label42);
		this.groupBox7.Controls.Add(this.label3);
		this.groupBox7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbChartofAccountOverHead);
		this.groupBox7.Controls.Add(this.label57);
		this.groupBox7.Controls.Add(this.txtItemQtyOverHead);
		this.groupBox7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrandNameOverHead);
		this.groupBox7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrandUomOverHead);
		this.groupBox7.Controls.Add(this.lblAmountOverHead);
		this.groupBox7.Controls.Add(this.txtExpAmountOverHead);
		this.groupBox7.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox7.Location = new System.Drawing.Point(4, 76);
		this.groupBox7.Name = "groupBox7";
		this.groupBox7.Size = new System.Drawing.Size(982, 116);
		this.groupBox7.TabIndex = 1;
		this.groupBox7.TabStop = false;
		this.groupBox7.Text = "Detail";
		this.RadOutputItemOH.AutoSize = true;
		this.RadOutputItemOH.Checked = true;
		this.RadOutputItemOH.Location = new System.Drawing.Point(662, 15);
		this.RadOutputItemOH.Name = "RadOutputItemOH";
		this.RadOutputItemOH.Size = new System.Drawing.Size(70, 20);
		this.RadOutputItemOH.TabIndex = 219;
		this.RadOutputItemOH.TabStop = true;
		this.RadOutputItemOH.Text = "Output";
		this.RadOutputItemOH.UseVisualStyleBackColor = true;
		this.RadOutputItemOH.CheckedChanged += new System.EventHandler(RadOutputItemOH_CheckedChanged);
		this.RadInputItemOH.AutoSize = true;
		this.RadInputItemOH.Location = new System.Drawing.Point(732, 15);
		this.RadInputItemOH.Name = "RadInputItemOH";
		this.RadInputItemOH.Size = new System.Drawing.Size(133, 20);
		this.RadInputItemOH.TabIndex = 219;
		this.RadInputItemOH.Text = "Input Item Name";
		this.RadInputItemOH.UseVisualStyleBackColor = true;
		this.label99.AutoSize = true;
		this.label99.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label99.Location = new System.Drawing.Point(156, 66);
		this.label99.Name = "label99";
		this.label99.Size = new System.Drawing.Size(56, 15);
		this.label99.TabIndex = 218;
		this.label99.Text = "RateUom";
		this.cmbRateUomOverHeads.AllowNull = (DefaultableBoolean)1;
		this.cmbRateUomOverHeads.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbRateUomOverHeads.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbRateUomOverHeads.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbRateUomOverHeads).DisplayLayout.Appearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbRateUomOverHeads).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbRateUomOverHeads).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbRateUomOverHeads).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbRateUomOverHeads).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance61;
		((SpecialBoxBase)((UltraGridBase)this.cmbRateUomOverHeads).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbRateUomOverHeads).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbRateUomOverHeads).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbRateUomOverHeads).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbRateUomOverHeads).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbRateUomOverHeads).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbRateUomOverHeads).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbRateUomOverHeads).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbRateUomOverHeads).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance66).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbRateUomOverHeads).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbRateUomOverHeads).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbRateUomOverHeads).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance67).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbRateUomOverHeads).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbRateUomOverHeads).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbRateUomOverHeads).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbRateUomOverHeads).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbRateUomOverHeads).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbRateUomOverHeads).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbRateUomOverHeads).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbRateUomOverHeads).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbRateUomOverHeads).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbRateUomOverHeads).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbRateUomOverHeads).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbRateUomOverHeads.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbRateUomOverHeads).Location = new System.Drawing.Point(152, 83);
		((System.Windows.Forms.Control)(object)this.cmbRateUomOverHeads).Name = "cmbRateUomOverHeads";
		((System.Windows.Forms.Control)(object)this.cmbRateUomOverHeads).Size = new System.Drawing.Size(69, 26);
		((System.Windows.Forms.Control)(object)this.cmbRateUomOverHeads).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.cmbRateUomOverHeads).Leave += new System.EventHandler(cmbRateUomOverHeads_Leave);
		this.btnAddOH.BackColor = System.Drawing.Color.Teal;
		this.btnAddOH.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddOH.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddOH.ForeColor = System.Drawing.Color.White;
		this.btnAddOH.Location = new System.Drawing.Point(839, 83);
		this.btnAddOH.Margin = new System.Windows.Forms.Padding(2);
		this.btnAddOH.Name = "btnAddOH";
		this.btnAddOH.Size = new System.Drawing.Size(38, 26);
		this.btnAddOH.TabIndex = 10;
		this.btnAddOH.Text = "+";
		this.btnAddOH.UseVisualStyleBackColor = false;
		this.btnAddOH.Click += new System.EventHandler(btnAddOH_Click);
		this.BtnCancelOH.BackColor = System.Drawing.Color.Teal;
		this.BtnCancelOH.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancelOH.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnCancelOH.ForeColor = System.Drawing.Color.White;
		this.BtnCancelOH.Location = new System.Drawing.Point(907, 83);
		this.BtnCancelOH.Name = "BtnCancelOH";
		this.BtnCancelOH.Size = new System.Drawing.Size(67, 26);
		this.BtnCancelOH.TabIndex = 12;
		this.BtnCancelOH.Text = "Cancel";
		this.BtnCancelOH.UseVisualStyleBackColor = false;
		this.BtnCancelOH.Click += new System.EventHandler(BtnCancelOH_Click);
		this.btnUpdateOH.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateOH.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateOH.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateOH.ForeColor = System.Drawing.Color.White;
		this.btnUpdateOH.Location = new System.Drawing.Point(839, 83);
		this.btnUpdateOH.Name = "btnUpdateOH";
		this.btnUpdateOH.Size = new System.Drawing.Size(70, 26);
		this.btnUpdateOH.TabIndex = 11;
		this.btnUpdateOH.Text = "Update ";
		this.btnUpdateOH.UseVisualStyleBackColor = false;
		this.btnUpdateOH.Click += new System.EventHandler(btnUpdateOH_Click);
		this.CmbPlantFeederOverHead.AllowNull = (DefaultableBoolean)1;
		this.CmbPlantFeederOverHead.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPlantFeederOverHead.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPlantFeederOverHead.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.Appearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance25;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance30).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPlantFeederOverHead).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederOverHead).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPlantFeederOverHead.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederOverHead).Location = new System.Drawing.Point(3, 36);
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederOverHead).Name = "CmbPlantFeederOverHead";
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederOverHead).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.CmbPlantFeederOverHead).TabIndex = 0;
		this.lblRateOverHead.AutoSize = true;
		this.lblRateOverHead.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblRateOverHead.Location = new System.Drawing.Point(79, 66);
		this.lblRateOverHead.Name = "lblRateOverHead";
		this.lblRateOverHead.Size = new System.Drawing.Size(30, 15);
		this.lblRateOverHead.TabIndex = 5;
		this.lblRateOverHead.Text = "Rate";
		this.label95.AutoSize = true;
		this.label95.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label95.Location = new System.Drawing.Point(222, 18);
		this.label95.Name = "label95";
		this.label95.Size = new System.Drawing.Size(77, 15);
		this.label95.TabIndex = 180;
		this.label95.Text = "Charges Type";
		this.cmbChargestypeOverheads.AllowNull = (DefaultableBoolean)1;
		this.cmbChargestypeOverheads.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbChargestypeOverheads.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbChargestypeOverheads.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbChargestypeOverheads).DisplayLayout.Appearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbChargestypeOverheads).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbChargestypeOverheads).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbChargestypeOverheads).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbChargestypeOverheads).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance73;
		((SpecialBoxBase)((UltraGridBase)this.cmbChargestypeOverheads).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbChargestypeOverheads).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbChargestypeOverheads).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbChargestypeOverheads).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbChargestypeOverheads).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbChargestypeOverheads).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbChargestypeOverheads).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbChargestypeOverheads).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbChargestypeOverheads).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance78).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbChargestypeOverheads).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbChargestypeOverheads).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbChargestypeOverheads).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance79).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbChargestypeOverheads).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbChargestypeOverheads).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbChargestypeOverheads).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbChargestypeOverheads).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbChargestypeOverheads).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbChargestypeOverheads).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbChargestypeOverheads).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbChargestypeOverheads).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbChargestypeOverheads).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbChargestypeOverheads).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbChargestypeOverheads).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbChargestypeOverheads.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbChargestypeOverheads).Location = new System.Drawing.Point(222, 36);
		((System.Windows.Forms.Control)(object)this.cmbChargestypeOverheads).Name = "cmbChargestypeOverheads";
		((System.Windows.Forms.Control)(object)this.cmbChargestypeOverheads).Size = new System.Drawing.Size(209, 26);
		((System.Windows.Forms.Control)(object)this.cmbChargestypeOverheads).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbChargestypeOverheads).Leave += new System.EventHandler(cmbChargestypeOverheads_Leave);
		this.lblRemarksOverHead.AutoSize = true;
		this.lblRemarksOverHead.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblRemarksOverHead.Location = new System.Drawing.Point(336, 66);
		this.lblRemarksOverHead.Name = "lblRemarksOverHead";
		this.lblRemarksOverHead.Size = new System.Drawing.Size(91, 15);
		this.lblRemarksOverHead.TabIndex = 62;
		this.lblRemarksOverHead.Text = "Ledger Remarks";
		this.label85.AutoSize = true;
		this.label85.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label85.Location = new System.Drawing.Point(3, 18);
		this.label85.Name = "label85";
		this.label85.Size = new System.Drawing.Size(69, 15);
		this.label85.TabIndex = 213;
		this.label85.Text = "Plant Name";
		this.txtLedgerRemarksOverHead.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLedgerRemarksOverHead.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLedgerRemarksOverHead.Location = new System.Drawing.Point(336, 85);
		this.txtLedgerRemarksOverHead.Name = "txtLedgerRemarksOverHead";
		this.txtLedgerRemarksOverHead.Size = new System.Drawing.Size(500, 23);
		this.txtLedgerRemarksOverHead.TabIndex = 9;
		this.txtItemRateOverHeads.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemRateOverHeads.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemRateOverHeads.Location = new System.Drawing.Point(75, 85);
		this.txtItemRateOverHeads.Name = "txtItemRateOverHeads";
		this.txtItemRateOverHeads.Size = new System.Drawing.Size(75, 23);
		this.txtItemRateOverHeads.TabIndex = 6;
		this.txtItemRateOverHeads.TextChanged += new System.EventHandler(txtItemRateOverHeads_TextChanged);
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(893, 18);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(61, 15);
		this.label42.TabIndex = 124;
		this.label42.Text = "Item Uom";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(3, 66);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(54, 15);
		this.label3.TabIndex = 118;
		this.label3.Text = "Item Qty";
		this.cmbChartofAccountOverHead.AllowNull = (DefaultableBoolean)1;
		this.cmbChartofAccountOverHead.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbChartofAccountOverHead.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbChartofAccountOverHead.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance37;
		((SpecialBoxBase)((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance42).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance43).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbChartofAccountOverHead).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbChartofAccountOverHead.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbChartofAccountOverHead).Location = new System.Drawing.Point(432, 36);
		((System.Windows.Forms.Control)(object)this.cmbChartofAccountOverHead).Name = "cmbChartofAccountOverHead";
		((System.Windows.Forms.Control)(object)this.cmbChartofAccountOverHead).Size = new System.Drawing.Size(228, 26);
		((System.Windows.Forms.Control)(object)this.cmbChartofAccountOverHead).TabIndex = 2;
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.Location = new System.Drawing.Point(434, 18);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(130, 15);
		this.label57.TabIndex = 60;
		this.label57.Text = "OverHead Account (Cr)";
		this.txtItemQtyOverHead.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemQtyOverHead.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemQtyOverHead.Location = new System.Drawing.Point(3, 85);
		this.txtItemQtyOverHead.Name = "txtItemQtyOverHead";
		this.txtItemQtyOverHead.Size = new System.Drawing.Size(70, 23);
		this.txtItemQtyOverHead.TabIndex = 5;
		this.txtItemQtyOverHead.TextChanged += new System.EventHandler(txtItemQtyOverHead_TextChanged);
		this.txtItemQtyOverHead.Leave += new System.EventHandler(txtItemQtyOverHead_Leave);
		this.CmbBrandNameOverHead.AllowNull = (DefaultableBoolean)1;
		this.CmbBrandNameOverHead.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrandNameOverHead.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrandNameOverHead.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Appearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance49;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance54).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance55).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance56).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrandNameOverHead).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrandNameOverHead).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrandNameOverHead.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrandNameOverHead).Location = new System.Drawing.Point(662, 36);
		((System.Windows.Forms.Control)(object)this.CmbBrandNameOverHead).Name = "CmbBrandNameOverHead";
		((System.Windows.Forms.Control)(object)this.CmbBrandNameOverHead).Size = new System.Drawing.Size(227, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrandNameOverHead).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbBrandNameOverHead).Leave += new System.EventHandler(CmbBrandNameOverHead_Leave);
		this.CmbBrandUomOverHead.AllowNull = (DefaultableBoolean)1;
		this.CmbBrandUomOverHead.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrandUomOverHead.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrandUomOverHead.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Appearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance85;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance86).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance86).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance93).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance94).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrandUomOverHead).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrandUomOverHead).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrandUomOverHead.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrandUomOverHead).Location = new System.Drawing.Point(891, 36);
		((System.Windows.Forms.Control)(object)this.CmbBrandUomOverHead).Name = "CmbBrandUomOverHead";
		((System.Windows.Forms.Control)(object)this.CmbBrandUomOverHead).Size = new System.Drawing.Size(82, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrandUomOverHead).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbBrandUomOverHead).Leave += new System.EventHandler(CmbBrandUomOverHead_Leave);
		this.lblAmountOverHead.AutoSize = true;
		this.lblAmountOverHead.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAmountOverHead.Location = new System.Drawing.Point(222, 66);
		this.lblAmountOverHead.Name = "lblAmountOverHead";
		this.lblAmountOverHead.Size = new System.Drawing.Size(51, 15);
		this.lblAmountOverHead.TabIndex = 60;
		this.lblAmountOverHead.Text = "Amount";
		this.txtExpAmountOverHead.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExpAmountOverHead.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExpAmountOverHead.Location = new System.Drawing.Point(222, 85);
		this.txtExpAmountOverHead.Name = "txtExpAmountOverHead";
		this.txtExpAmountOverHead.Size = new System.Drawing.Size(113, 23);
		this.txtExpAmountOverHead.TabIndex = 8;
		this.groupBox6.Controls.Add(this.label102);
		this.groupBox6.Controls.Add(this.txtfinishQtyOh);
		this.groupBox6.Controls.Add(this.label103);
		this.groupBox6.Controls.Add(this.txtfinishWeightOh);
		this.groupBox6.Controls.Add(this.label97);
		this.groupBox6.Controls.Add(this.txtInputQtyTotalOH);
		this.groupBox6.Controls.Add(this.label98);
		this.groupBox6.Controls.Add(this.txtInputWeightTotalOH);
		this.groupBox6.Controls.Add(this.label38);
		this.groupBox6.Controls.Add(this.txtOutputQtyTotalOH);
		this.groupBox6.Controls.Add(this.label46);
		this.groupBox6.Controls.Add(this.txtOutputWeightTotalOH);
		this.groupBox6.Controls.Add((System.Windows.Forms.Control)(object)this.cmbJobOrderOverHead);
		this.groupBox6.Controls.Add(this.txtDocNoOverHead);
		this.groupBox6.Controls.Add(this.label56);
		this.groupBox6.Controls.Add(this.txtDocDateOverHead);
		this.groupBox6.Controls.Add(this.label39);
		this.groupBox6.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox6.Location = new System.Drawing.Point(4, 3);
		this.groupBox6.Name = "groupBox6";
		this.groupBox6.Size = new System.Drawing.Size(982, 72);
		this.groupBox6.TabIndex = 0;
		this.groupBox6.TabStop = false;
		this.groupBox6.Text = "Main";
		this.label102.AutoSize = true;
		this.label102.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label102.Location = new System.Drawing.Point(747, 44);
		this.label102.Name = "label102";
		this.label102.Size = new System.Drawing.Size(109, 15);
		this.label102.TabIndex = 188;
		this.label102.Text = "Total Finish Weight";
		this.label102.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.txtfinishQtyOh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfinishQtyOh.Location = new System.Drawing.Point(862, 14);
		this.txtfinishQtyOh.Name = "txtfinishQtyOh";
		this.txtfinishQtyOh.ReadOnly = true;
		this.txtfinishQtyOh.Size = new System.Drawing.Size(106, 23);
		this.txtfinishQtyOh.TabIndex = 186;
		this.txtfinishQtyOh.TabStop = false;
		this.label103.AutoSize = true;
		this.label103.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label103.Location = new System.Drawing.Point(747, 18);
		this.label103.Name = "label103";
		this.label103.Size = new System.Drawing.Size(89, 15);
		this.label103.TabIndex = 189;
		this.label103.Text = "Total Finish Qty";
		this.label103.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.txtfinishWeightOh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfinishWeightOh.Location = new System.Drawing.Point(862, 40);
		this.txtfinishWeightOh.Name = "txtfinishWeightOh";
		this.txtfinishWeightOh.ReadOnly = true;
		this.txtfinishWeightOh.Size = new System.Drawing.Size(106, 23);
		this.txtfinishWeightOh.TabIndex = 187;
		this.txtfinishWeightOh.TabStop = false;
		this.label97.AutoSize = true;
		this.label97.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label97.Location = new System.Drawing.Point(301, 44);
		this.label97.Name = "label97";
		this.label97.Size = new System.Drawing.Size(107, 15);
		this.label97.TabIndex = 184;
		this.label97.Text = "Total InPut Weight";
		this.label97.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.txtInputQtyTotalOH.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInputQtyTotalOH.Location = new System.Drawing.Point(410, 14);
		this.txtInputQtyTotalOH.Name = "txtInputQtyTotalOH";
		this.txtInputQtyTotalOH.ReadOnly = true;
		this.txtInputQtyTotalOH.Size = new System.Drawing.Size(106, 23);
		this.txtInputQtyTotalOH.TabIndex = 182;
		this.txtInputQtyTotalOH.TabStop = false;
		this.label98.AutoSize = true;
		this.label98.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label98.Location = new System.Drawing.Point(302, 18);
		this.label98.Name = "label98";
		this.label98.Size = new System.Drawing.Size(87, 15);
		this.label98.TabIndex = 185;
		this.label98.Text = "Total InPut Qty";
		this.label98.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.txtInputWeightTotalOH.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInputWeightTotalOH.Location = new System.Drawing.Point(410, 40);
		this.txtInputWeightTotalOH.Name = "txtInputWeightTotalOH";
		this.txtInputWeightTotalOH.ReadOnly = true;
		this.txtInputWeightTotalOH.Size = new System.Drawing.Size(106, 23);
		this.txtInputWeightTotalOH.TabIndex = 183;
		this.txtInputWeightTotalOH.TabStop = false;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(518, 44);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(116, 15);
		this.label38.TabIndex = 177;
		this.label38.Text = "Total OutPut Weight";
		this.label38.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.txtOutputQtyTotalOH.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOutputQtyTotalOH.Location = new System.Drawing.Point(637, 14);
		this.txtOutputQtyTotalOH.Name = "txtOutputQtyTotalOH";
		this.txtOutputQtyTotalOH.ReadOnly = true;
		this.txtOutputQtyTotalOH.Size = new System.Drawing.Size(106, 23);
		this.txtOutputQtyTotalOH.TabIndex = 175;
		this.txtOutputQtyTotalOH.TabStop = false;
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(518, 18);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(96, 15);
		this.label46.TabIndex = 178;
		this.label46.Text = "Total OutPut Qty";
		this.label46.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.txtOutputWeightTotalOH.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOutputWeightTotalOH.Location = new System.Drawing.Point(637, 40);
		this.txtOutputWeightTotalOH.Name = "txtOutputWeightTotalOH";
		this.txtOutputWeightTotalOH.ReadOnly = true;
		this.txtOutputWeightTotalOH.Size = new System.Drawing.Size(106, 23);
		this.txtOutputWeightTotalOH.TabIndex = 176;
		this.txtOutputWeightTotalOH.TabStop = false;
		this.cmbJobOrderOverHead.AllowNull = (DefaultableBoolean)1;
		this.cmbJobOrderOverHead.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbJobOrderOverHead.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbJobOrderOverHead.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Appearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance21;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance90).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance91).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance19).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbJobOrderOverHead).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbJobOrderOverHead.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbJobOrderOverHead).Location = new System.Drawing.Point(85, 38);
		((System.Windows.Forms.Control)(object)this.cmbJobOrderOverHead).Name = "cmbJobOrderOverHead";
		((System.Windows.Forms.Control)(object)this.cmbJobOrderOverHead).Size = new System.Drawing.Size(214, 26);
		((System.Windows.Forms.Control)(object)this.cmbJobOrderOverHead).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbJobOrderOverHead).Leave += new System.EventHandler(cmbJobOrderOverHead_Leave);
		this.txtDocNoOverHead.BackColor = System.Drawing.Color.WhiteSmoke;
		this.txtDocNoOverHead.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoOverHead.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoOverHead.Location = new System.Drawing.Point(84, 14);
		this.txtDocNoOverHead.Name = "txtDocNoOverHead";
		this.txtDocNoOverHead.ReadOnly = true;
		this.txtDocNoOverHead.Size = new System.Drawing.Size(66, 23);
		this.txtDocNoOverHead.TabIndex = 0;
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.Location = new System.Drawing.Point(4, 18);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(48, 15);
		this.label56.TabIndex = 56;
		this.label56.Text = "Doc No";
		this.txtDocDateOverHead.CustomFormat = "dd-MMM-yy";
		this.txtDocDateOverHead.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocDateOverHead.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocDateOverHead.Location = new System.Drawing.Point(151, 14);
		this.txtDocDateOverHead.Name = "txtDocDateOverHead";
		this.txtDocDateOverHead.Size = new System.Drawing.Size(148, 23);
		this.txtDocDateOverHead.TabIndex = 0;
		this.txtDocDateOverHead.ValueChanged += new System.EventHandler(txtDocDateOverHead_ValueChanged);
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(5, 44);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(78, 15);
		this.label39.TabIndex = 116;
		this.label39.Text = "Job Order No";
		this.panel69.BackColor = System.Drawing.Color.Teal;
		this.panel69.Controls.Add(this.label44);
		this.panel69.Controls.Add(this.toolStrip5);
		this.panel69.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel69.Location = new System.Drawing.Point(0, 0);
		this.panel69.Name = "panel69";
		this.panel69.Size = new System.Drawing.Size(1097, 54);
		this.panel69.TabIndex = 2;
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Tahoma", 11.75f, System.Drawing.FontStyle.Bold);
		this.label44.ForeColor = System.Drawing.Color.White;
		this.label44.Location = new System.Drawing.Point(3, 31);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(102, 19);
		this.label44.TabIndex = 178;
		this.label44.Text = "Over Heads";
		this.toolStrip5.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip5.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip5.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.btnNewOverHead, this.btnRefreshOverHead, this.btnSaveOverHead, this.btnUpdateOverHead, this.toolStripSeparator4, this.btnPrintOverHead, this.btnVoucherOH, this.BtnLoadFohOverHeads, this.btnOverHeadFormulas });
		this.toolStrip5.Location = new System.Drawing.Point(0, 0);
		this.toolStrip5.Name = "toolStrip5";
		this.toolStrip5.Size = new System.Drawing.Size(1097, 27);
		this.toolStrip5.TabIndex = 58;
		this.toolStrip5.Text = "toolStrip5";
		this.btnNewOverHead.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewOverHead.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewOverHead.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewOverHead.Name = "btnNewOverHead";
		this.btnNewOverHead.Size = new System.Drawing.Size(59, 24);
		this.btnNewOverHead.Text = "&New";
		this.btnNewOverHead.Click += new System.EventHandler(btnNewOverHead_Click);
		this.btnRefreshOverHead.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshOverHead.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshOverHead.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshOverHead.Name = "btnRefreshOverHead";
		this.btnRefreshOverHead.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshOverHead.Text = "Refresh";
		this.btnRefreshOverHead.Click += new System.EventHandler(btnRefreshOverHead_Click);
		this.btnSaveOverHead.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSaveOverHead.Image = (System.Drawing.Image)resources.GetObject("btnSaveOverHead.Image");
		this.btnSaveOverHead.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveOverHead.Name = "btnSaveOverHead";
		this.btnSaveOverHead.Size = new System.Drawing.Size(64, 24);
		this.btnSaveOverHead.Text = "&Save";
		this.btnSaveOverHead.Click += new System.EventHandler(btnSaveOverHead_Click);
		this.btnUpdateOverHead.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateOverHead.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdateOverHead.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdateOverHead.Name = "btnUpdateOverHead";
		this.btnUpdateOverHead.Size = new System.Drawing.Size(78, 24);
		this.btnUpdateOverHead.Text = "&Update";
		this.btnUpdateOverHead.Visible = false;
		this.btnUpdateOverHead.Click += new System.EventHandler(btnUpdateOverHead_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.btnPrintOverHead.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrintOverHead.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintOverHead.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintOverHead.Name = "btnPrintOverHead";
		this.btnPrintOverHead.Size = new System.Drawing.Size(92, 24);
		this.btnPrintOverHead.Text = "&Print-606";
		this.btnPrintOverHead.Click += new System.EventHandler(btnPrintOverHead_Click);
		this.btnVoucherOH.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnVoucherOH.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnVoucherOH.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnVoucherOH.Name = "btnVoucherOH";
		this.btnVoucherOH.Size = new System.Drawing.Size(115, 24);
		this.btnVoucherOH.Text = "118-Voucher";
		this.btnVoucherOH.Click += new System.EventHandler(btnVoucherOH_Click_1);
		this.BtnLoadFohOverHeads.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnLoadFohOverHeads.Image = Architecture.WinApp.Properties.Resources.download;
		this.BtnLoadFohOverHeads.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoadFohOverHeads.Name = "BtnLoadFohOverHeads";
		this.BtnLoadFohOverHeads.Size = new System.Drawing.Size(124, 24);
		this.BtnLoadFohOverHeads.Text = "FOH Expenses";
		this.BtnLoadFohOverHeads.Click += new System.EventHandler(BtnLoadFohOverHeads_Click);
		this.btnOverHeadFormulas.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnOverHeadFormulas.Image = Architecture.WinApp.Properties.Resources.Taxation;
		this.btnOverHeadFormulas.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnOverHeadFormulas.Name = "btnOverHeadFormulas";
		this.btnOverHeadFormulas.Size = new System.Drawing.Size(89, 24);
		this.btnOverHeadFormulas.Text = "Formulas";
		this.btnOverHeadFormulas.Click += new System.EventHandler(btnOverHeadFormulas_Click);
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grddetailOhHistory);
		this.tabPage3.Controls.Add(this.panel27);
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grdOverHeadHistory);
		this.tabPage3.Controls.Add(this.panel8);
		this.tabPage3.Location = new System.Drawing.Point(4, 4);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1103, 610);
		this.tabPage3.TabIndex = 1;
		this.tabPage3.Text = "History";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.grddetailOhHistory.AllowEdit = (InheritableBoolean)2;
		grddetailOhHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grddetailOhHistory.DesignTimeLayout = grddetailOhHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grddetailOhHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grddetailOhHistory.DynamicFiltering = true;
		this.grddetailOhHistory.FilterMode = (FilterMode)2;
		this.grddetailOhHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grddetailOhHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grddetailOhHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grddetailOhHistory).Location = new System.Drawing.Point(3, 383);
		((System.Windows.Forms.Control)(object)this.grddetailOhHistory).Name = "grddetailOhHistory";
		((ScrollableBase)this.grddetailOhHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grddetailOhHistory).Size = new System.Drawing.Size(1097, 224);
		((System.Windows.Forms.Control)(object)this.grddetailOhHistory).TabIndex = 1;
		this.grddetailOhHistory.TotalRow = (InheritableBoolean)1;
		this.grddetailOhHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grddetailOhHistory).VisualStyle = (VisualStyle)4;
		this.panel27.BackColor = System.Drawing.Color.Teal;
		this.panel27.Controls.Add(this.ctrlGrdBar3);
		this.panel27.Controls.Add(this.label34);
		this.panel27.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel27.Location = new System.Drawing.Point(3, 357);
		this.panel27.Name = "panel27";
		this.panel27.Size = new System.Drawing.Size(1097, 26);
		this.panel27.TabIndex = 2;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1057, 2);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grddetailOhHistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar3.TabIndex = 177;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label34.ForeColor = System.Drawing.Color.White;
		this.label34.Location = new System.Drawing.Point(3, 4);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(181, 17);
		this.label34.TabIndex = 178;
		this.label34.Text = "Selected Job Order Detail";
		this.grdOverHeadHistory.AllowEdit = (InheritableBoolean)2;
		grdOverHeadHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOverHeadHistory.DesignTimeLayout = grdOverHeadHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOverHeadHistory).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdOverHeadHistory.DynamicFiltering = true;
		this.grdOverHeadHistory.FilterMode = (FilterMode)2;
		this.grdOverHeadHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdOverHeadHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOverHeadHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdOverHeadHistory).Location = new System.Drawing.Point(3, 35);
		((System.Windows.Forms.Control)(object)this.grdOverHeadHistory).Name = "grdOverHeadHistory";
		((ScrollableBase)this.grdOverHeadHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOverHeadHistory).Size = new System.Drawing.Size(1097, 322);
		((System.Windows.Forms.Control)(object)this.grdOverHeadHistory).TabIndex = 0;
		this.grdOverHeadHistory.TotalRow = (InheritableBoolean)1;
		this.grdOverHeadHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOverHeadHistory).VisualStyle = (VisualStyle)4;
		this.grdOverHeadHistory.ColumnButtonClick += new ColumnActionEventHandler(grdOverHeadHistory_ColumnButtonClick);
		this.grdOverHeadHistory.SelectionChanged += new System.EventHandler(grdOverHeadHistory_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.grdOverHeadHistory).DoubleClick += new System.EventHandler(grdOverHead_DoubleClick);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label101);
		this.panel8.Controls.Add(this.ctrlGrdBar2);
		this.panel8.Controls.Add(this.label89);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1097, 32);
		this.panel8.TabIndex = 0;
		this.label101.AutoSize = true;
		this.label101.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label101.ForeColor = System.Drawing.Color.White;
		this.label101.Location = new System.Drawing.Point(174, 7);
		this.label101.Name = "label101";
		this.label101.Size = new System.Drawing.Size(125, 17);
		this.label101.TabIndex = 179;
		this.label101.Text = "(Job Order Wise)";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1055, 4);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdOverHeadHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar2.TabIndex = 177;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label89.AutoSize = true;
		this.label89.Font = new System.Drawing.Font("Tahoma", 11.75f, System.Drawing.FontStyle.Bold);
		this.label89.ForeColor = System.Drawing.Color.White;
		this.label89.Location = new System.Drawing.Point(3, 6);
		this.label89.Name = "label89";
		this.label89.Size = new System.Drawing.Size(165, 19);
		this.label89.TabIndex = 178;
		this.label89.Text = "Over Heads History";
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
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(1111, 639);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmProductionOverhead";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(FoodProductionWithValues_FormClosing);
		base.Load += new System.EventHandler(frmFoodProduction_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmFoodProduction_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage2.ResumeLayout(false);
		this.panel68.ResumeLayout(false);
		this.panel31.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetailOverhead).EndInit();
		this.panel25.ResumeLayout(false);
		this.panel25.PerformLayout();
		this.panel9.ResumeLayout(false);
		this.groupBox7.ResumeLayout(false);
		this.groupBox7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbRateUomOverHeads).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantFeederOverHead).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbChargestypeOverheads).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbChartofAccountOverHead).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandNameOverHead).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandUomOverHead).EndInit();
		this.groupBox6.ResumeLayout(false);
		this.groupBox6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbJobOrderOverHead).EndInit();
		this.panel69.ResumeLayout(false);
		this.panel69.PerformLayout();
		this.toolStrip5.ResumeLayout(false);
		this.toolStrip5.PerformLayout();
		this.tabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grddetailOhHistory).EndInit();
		this.panel27.ResumeLayout(false);
		this.panel27.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOverHeadHistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		base.ResumeLayout(false);
	}
}
