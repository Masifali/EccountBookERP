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
using Architecture.BLL.pcc;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Pcc;
using Architecture.Model.pcc;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.pcc;

public class frmConcreteProductionUpdationAfterRelease : Form
{
	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtContractorWages = new DataTable();

	private DataTable dtBOMDataforCombo = new DataTable();

	private DataTable dtUom = new DataTable();

	private DataTable dtRateUom = new DataTable();

	private DataTable dtuomGetAll = new DataTable();

	private DataTable dtHistory = new DataTable();

	private DataTable dtAccounts = new DataTable();

	private DataTable lstitem = new DataTable();

	private DataTable dtAttributes = new DataTable();

	private DataTable dtContr = new DataTable();

	private DataTable dtOrders = new DataTable();

	private DataTable dtType = new DataTable();

	private DataTable DtStock = new DataTable();

	private bool FifoMethod;

	private bool UpdateMode;

	private int RecId;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnUpdate;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton btnPrint;

	private Label label11;

	private TabPage tabPage3;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewHistory;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton btnRegister;

	private GroupBox groupBox2;

	private Label label21;

	internal UltraCombo CmbProductionNo;

	private Panel panel10;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnshortcutkeys;

	private Panel panel38;

	private Label label32;

	private Panel panel45;

	private Panel panel8;

	private Panel panel19;

	internal GridEX grdHistoryInput;

	private Panel panel44;

	internal GridEX DatagridHistory;

	private Panel panel15;

	private Label label17;

	private Panel panel13;

	private GroupBox groupBox1;

	private Label label19;

	internal UltraCombo cmbDateTypeHistory;

	private DateTimePicker txtToDateHistory;

	private Label label28;

	private TextBox txtToDocNoHistory;

	private DateTimePicker txtFromdateHistory;

	private Label label35;

	private TextBox txtFromNoHistory;

	private Label label30;

	internal UltraCombo cmbProductionNoHistory;

	internal UltraCombo CmbBomNoHistory;

	private Label label20;

	internal UltraCombo cmbItemhistory;

	private Label label29;

	private Button btnshow;

	private CtrlGrdBar ctrlGrdBar1;

	private CheckBox ChkPreviewSlip;

	internal GridEX grdHistoryOutput;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator2;

	internal GridEX grdWagesHistory;

	private Panel panel12;

	internal GridEX grdContractorWages;

	private Panel panel5;

	private Label label33;

	private Panel panel17;

	private TabControl tabControl2;

	private TabPage TabWages;

	private Button BtnLoadInputFromBom;

	private Label label14;

	private Panel panel18;

	private Label label40;

	private Label label15;

	internal GridEX grdHistoryOverHead;

	private Panel panel42;

	private Label label34;

	private ToolStripButton btnVoucherReport;

	private CheckBox chkPrintVoucher;

	private RadioButton RadNotReffered;

	private RadioButton RadReffered;

	private RadioButton RadAll;

	internal UltraCombo CmbItem;

	private Label label1;

	private DateTimePicker txtProductionDate;

	private TextBox txtOutputTotalQty;

	internal UltraCombo CmbProductionType;

	private TextBox txtDocNo;

	private Label label2;

	private Label label3;

	private Label label4;

	public frmConcreteProductionUpdationAfterRelease(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmBillOfMaterial_Manufacturing_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 750);
			FifoMethod = CommonServices.GetERPFeatureById(5);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			btnPrint.Enabled = formright.DoHavePrintRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			dtAccounts.Columns.Add("Id");
			dtAccounts.Columns.Add("AccountTitle");
			dtUom.Columns.Add("Id", typeof(int));
			dtUom.Columns.Add("ItemUOM", typeof(double));
			dtUom.Columns.Add("ItemUOMCode", typeof(string));
			dtRateUom.Columns.Add("RateId", typeof(int));
			dtRateUom.Columns.Add("RateUOM", typeof(double));
			dtRateUom.Columns.Add("RateUOMCode", typeof(string));
			dtFillsForGrids();
			BOMNoAndItemFill();
			uomgetAll();
			Datetypefill();
			AllCombobind();
			dtContractorWages.Columns.Add("Id", typeof(int));
			dtContractorWages.Columns.Add("BomWagesId", typeof(int));
			dtContractorWages.Columns.Add("ContractorId", typeof(int));
			dtContractorWages.Columns.Add("ContractorName");
			dtContractorWages.Columns.Add("ContractorWagesRateScheduleId", typeof(int));
			dtContractorWages.Columns.Add("ServiceActivity");
			dtContractorWages.Columns.Add("BomQty", typeof(decimal));
			dtContractorWages.Columns.Add("Qty", typeof(decimal));
			dtContractorWages.Columns.Add("Rate", typeof(decimal));
			dtContractorWages.Columns.Add("Amount", typeof(decimal));
			dtContractorWages.Columns.Add("NewRate", typeof(decimal));
			dtContractorWages.Columns.Add("NewAmount", typeof(decimal));
			dtContractorWages.Columns.Add("Remarks");
			AddRowInContractorWagesGrid();
			grdContractorWages.RetrieveStructure();
			grdContractorWagesGridSetting();
			CmbProductionNo.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void dtFillsForGrids()
	{
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		try
		{
			dtType.Clear();
			dtType.Rows.Add(1, "Auto Rate");
			dtType.Rows.Add(1, "Manual Rate");
			lstitem.Clear();
			lstitem = CommonServices.GetItemByItemTypeId("45,46,47");
			dtAttributes.Clear();
			dtAttributes = ItemAttributeVarient.GetAllForCombo(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToInt(CmbItem.Value));
			dtOrders.Clear();
			SaleOrder obj = new SaleOrder();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtOrders = SaleOrder.GetSaleOrderNo(obj);
			dtContr.Clear();
			SupplierCustomer supplier = new SupplierCustomer();
			supplier.OrganizationId = UserAccount.OrganizationId;
			supplier.CompanyId = UserAccount.CompanyId;
			dtContr = SupplierCustomer.ReadByOrganizationCompanyIdForContractorWages(supplier);
			DataTable dt = CommonServices.CoaAllocationGetAllServiceBind();
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			dtAccounts.Rows.Clear();
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 3 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 6 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 8 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 11 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 12 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 13 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 14 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 20 || Conversion.ToInt(dt.Rows[i]["AccountTypeId"]) == 21)
				{
					dtAccounts.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["AccountTitle"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BOMNobyItemFill(int ItemId)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		try
		{
			int BomId = Conversion.ToInt(CmbProductionNo.Value);
			DataTable dtBomNo = BomHeader.GetBomNoByItemId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				ItemId = ItemId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 1851
			});
			if (dtBomNo.Rows.Count > 0)
			{
				if (dtBomNo.Rows.Count <= 0)
				{
					return;
				}
				DDL.BindDDL(dtBomNo, CmbProductionNo, "Id", "BomName", "Bom No", ZeroIndex: true);
				((UltraGridBase)CmbProductionNo).DisplayLayout.Bands[0].Columns[1].Width = 350;
				((UltraGridBase)CmbProductionNo).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbProductionNo).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				if (BomId > 0)
				{
					if (dtBomNo.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == BomId))
					{
						CmbProductionNo.Value = BomId;
					}
					else
					{
						((Control)(object)CmbProductionNo).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbProductionNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BOMNoAndItemFill()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			dtBOMDataforCombo.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = 1851;
			dtBOMDataforCombo = BomHeader.GetAllBomNoAndItems(obj);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void uomgetAll()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			UOMSchedule obj = new UOMSchedule();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtuomGetAll = UOMSchedule.Getall(obj);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnLoadInputFromBom_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbProductionNo.Value) == 0)
			{
				CmbProductionNo.Focus();
				throw new Exception("Please Select Bom No First");
			}
			if (Conversion.ToInt(CmbItem.Value) == 0)
			{
				CmbItem.Focus();
				throw new Exception("Please Select Item First");
			}
			FillInputDetailFromBOM(Conversion.ToInt(CmbProductionNo.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillInputDetailFromBOM(int BOMId)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		try
		{
			dtContractorWages.Clear();
			grdContractorWages.ClearStructure();
			BomHeader bOMHeader = new BomHeader();
			bOMHeader = BomHeader.ReadById(BOMId);
			if (bOMHeader != null)
			{
				if (bOMHeader.BomWagesDetailList.Count > 0)
				{
					for (int i = 0; i < bOMHeader.BomWagesDetailList.Count; i++)
					{
						dtContractorWages.Rows.Add(0, bOMHeader.BomWagesDetailList[i].Id, 0, "", bOMHeader.BomWagesDetailList[i].ContractorWagesRateScheduleId, bOMHeader.BomWagesDetailList[i].ServiceActivity, bOMHeader.BomWagesDetailList[i].ItemQty, bOMHeader.BomWagesDetailList[i].Rate, bOMHeader.BomWagesDetailList[i].Amount, bOMHeader.BomWagesDetailList[i].Remarks);
					}
				}
				else
				{
					AddRowInContractorWagesGrid();
				}
				grdContractorWages.DataSource = dtContractorWages;
				grdContractorWages.RetrieveStructure();
				grdContractorWagesGridSetting();
				UpdateWagesRates();
			}
			if ((Conversion.ToInt(CmbProductionType.Value) == 46 || Conversion.ToInt(CmbProductionType.Value) == 47) && Conversion.ToInt(CmbItem.Value) > 0 && Conversion.ToDecimal((object)txtOutputTotalQty) > 0m)
			{
				UpdateWagesQtyIncaseOfSlabProduction(IsSlab: true);
			}
			else
			{
				UpdateWagesQtyIncaseOfSlabProduction(IsSlab: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateWagesQtyIncaseOfSlabProduction(bool IsSlab)
	{
		try
		{
			if (IsSlab)
			{
				if (grdContractorWages.RowCount <= 0)
				{
					throw new Exception("Wages Grid Record not Found");
				}
				string sumofOutPutItemQty = txtOutputTotalQty.Text;
				GridEXRow[] rows = grdContractorWages.GetRows();
				foreach (GridEXRow r in rows)
				{
					r.BeginEdit();
					decimal AvgRate = Conversion.ToDecimal(r.Cells["Rate"].Value);
					decimal FinalQty = Conversion.ToDecimal((object)r.Cells["BomQty"].Value.ToString()) * Conversion.ToDecimal((object)sumofOutPutItemQty);
					r.Cells["Qty"].Value = FinalQty;
					if (FinalQty > 0m && AvgRate > 0m)
					{
						decimal Amount = FinalQty * AvgRate;
						r.Cells["Amount"].Value = Amount;
					}
					else
					{
						r.Cells["Amount"].Value = 0;
					}
					r.EndEdit();
				}
			}
			else
			{
				if (grdContractorWages.RowCount <= 0)
				{
					return;
				}
				GridEXRow[] rows = grdContractorWages.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					r2.BeginEdit();
					decimal AvgRate2 = Conversion.ToDecimal(r2.Cells["Rate"].Value);
					decimal FinalQty2 = Conversion.ToDecimal((object)r2.Cells["BomQty"].Value.ToString());
					r2.Cells["Qty"].Value = FinalQty2;
					if (FinalQty2 > 0m && AvgRate2 > 0m)
					{
						decimal Amount2 = FinalQty2 * AvgRate2;
						r2.Cells["Amount"].Value = Amount2;
					}
					else
					{
						r2.Cells["Amount"].Value = 0;
					}
					r2.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((Control)(object)CmbProductionNo).Text.Trim() == string.Empty || Conversion.ToInt(CmbProductionNo.Value) == 0)
		{
			MessageBox.Show("ProductionNo Feild Required");
			CmbProductionNo.Focus();
			return false;
		}
		if (((Control)(object)CmbProductionType).Text.Trim() == string.Empty || Conversion.ToInt(CmbProductionType.Value) == 0)
		{
			MessageBox.Show("Production Type Feild Required");
			CmbProductionType.Focus();
			return false;
		}
		if (((Control)(object)CmbItem).Text.Trim() == string.Empty || Conversion.ToInt(CmbItem.Value) == 0)
		{
			MessageBox.Show("Item Feild Required");
			CmbItem.Focus();
			return false;
		}
		return true;
	}

	private void AddRowInContractorWagesGrid()
	{
		dtContractorWages.Rows.Add(0, 0, 0, "", 0, "", 0, 0, 0, 0, 0, 0, "");
		grdContractorWages.DataSource = dtContractorWages;
	}

	private void grdContractorWagesComboFill()
	{
	}

	private void grdContractorWagesGridSetting()
	{
		try
		{
			grdContractorWagesComboFill();
			grdContractorWages.RootTable.Columns["Id"].Visible = false;
			grdContractorWages.RootTable.Columns["BomWagesId"].Visible = false;
			grdContractorWages.RootTable.Columns["ContractorId"].Visible = false;
			grdContractorWages.RootTable.Columns["ContractorWagesRateScheduleId"].Visible = false;
			grdContractorWages.RootTable.Columns["Id"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["BomWagesId"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["ContractorId"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["ContractorName"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["ContractorWagesRateScheduleId"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["ServiceActivity"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["Qty"].EditType = (EditType)1;
			grdContractorWages.RootTable.Columns["Rate"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["NewRate"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["NewAmount"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["BomQty"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["BomQty"].AggregateFunction = (AggregateFunction)2;
			grdContractorWages.RootTable.Columns["BomQty"].TextAlignment = (TextAlignment)3;
			grdContractorWages.RootTable.Columns["BomQty"].FormatString = "##,#.####";
			grdContractorWages.RootTable.Columns["BomQty"].TotalFormatString = "##,#.####";
			grdContractorWages.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdContractorWages.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdContractorWages.RootTable.Columns["Qty"].FormatString = "##,#.####";
			grdContractorWages.RootTable.Columns["Qty"].TotalFormatString = "##,#.####";
			grdContractorWages.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdContractorWages.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdContractorWages.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdContractorWages.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdContractorWages.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdContractorWages.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdContractorWages.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdContractorWages.RootTable.Columns["NewRate"].TextAlignment = (TextAlignment)3;
			grdContractorWages.RootTable.Columns["NewRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdContractorWages.RootTable.Columns["NewRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdContractorWages.RootTable.Columns["NewAmount"].TextAlignment = (TextAlignment)3;
			grdContractorWages.RootTable.Columns["NewAmount"].AggregateFunction = (AggregateFunction)2;
			grdContractorWages.RootTable.Columns["NewAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdContractorWages.RootTable.Columns["NewAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdContractorWages);
			grdContractorWages.RootTable.Columns.Add("Delete");
			grdContractorWages.RootTable.Columns["Delete"].Key = "Delete";
			grdContractorWages.RootTable.Columns["Delete"].Caption = "X";
			grdContractorWages.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdContractorWages.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdContractorWages.RootTable.Columns["Delete"].ButtonText = "X";
			grdContractorWages.RootTable.Columns["Delete"].Width = 40;
			grdContractorWages.RootTable.Columns["Delete"].Position = 0;
			grdContractorWages.RootTable.Columns.Add("Add");
			grdContractorWages.RootTable.Columns["Add"].Key = "Add";
			grdContractorWages.RootTable.Columns["Add"].Caption = "+";
			grdContractorWages.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdContractorWages.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdContractorWages.RootTable.Columns["Add"].ButtonText = "+";
			grdContractorWages.RootTable.Columns["Add"].Width = 40;
			grdContractorWages.RootTable.Columns["Add"].Position = 1;
			grdContractorWages.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdContractorWages_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdContractorWages.UpdateData();
			GridEXRow item = grdContractorWages.CurrentRow;
			if (e.Column.Key == "Qty" && item != null && (int)item.RowType == 0)
			{
				decimal Amount = default(decimal);
				decimal Iuom = default(decimal);
				if (!(item.Cells["Qty"].Value.ToString() != string.Empty) || !(item.Cells["Rate"].Value.ToString() != string.Empty))
				{
					throw new Exception("Qty And NewRate Should be greater than zero...");
				}
				decimal num = Conversion.ToDecimal((object)item.Cells["Qty"].Value.ToString());
				Iuom = Conversion.ToDecimal((object)item.Cells["NewRate"].Text.ToString());
				Amount = num * Iuom;
				item.Cells["NewAmount"].Value = Amount;
			}
			grdContractorWages.UpdateData();
			CommonServices.GridAutoAdjustmentNew(grdContractorWages);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdContractorWages_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdContractorWages.CurrentRow;
				dtContractorWages.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtContractorWages.Rows.Count == 0)
				{
					AddRowInContractorWagesGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInContractorWagesGrid();
			}
			grdContractorWages.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		FromReset();
	}

	public void FromReset()
	{
		try
		{
			RecId = 0;
			((Control)(object)CmbProductionNo).Text = string.Empty;
			dtContractorWages.Rows.Clear();
			grdContractorWages.ClearStructure();
			btnUpdate.Visible = true;
			UpdateMode = true;
			AddRowInContractorWagesGrid();
			grdContractorWages.RetrieveStructure();
			grdContractorWagesGridSetting();
			CmbProductionNo.Focus();
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
			BOMNoAndItemFill();
			dtFillsForGrids();
			uomgetAll();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmConcreteProductionUpdationAfterRelease_FormClosed(object sender, FormClosedEventArgs e)
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

	private void Insert()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			if (grdContractorWages.GetRows().Count() == 0)
			{
				MessageBox.Show("Grid Record Not Found...\nGrid (Wages) Should have atleast one record");
				return;
			}
			ProductionHeader ProductionHeader = new ProductionHeader();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				ProductionHeader.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			ProductionHeader.CompanyId = UserAccount.CompanyId;
			ProductionHeader.BranchesId = UserAccount.BranchesId;
			ProductionHeader.ProjectsId = UserAccount.BranchesId;
			ProductionHeader.OrganizationId = UserAccount.OrganizationId;
			ProductionHeader.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			ProductionHeader.DocumentTypeId = 1850;
			ProductionHeader.EntryUserId = UserAccount.ID;
			ProductionHeader.EntryDate = DateTime.Now;
			ProductionHeader.ModifyDate = DateTime.Now;
			ProductionHeader.ModifyUserId = UserAccount.ID;
			ProductionHeader.ApprovedUserId = UserAccount.ID;
			ProductionHeader.ApprovedDate = DateTime.Now;
			ProductionHeader.IsApproved = false;
			ProductionHeader.ProductionInputDetailList = new List<ProductionInputDetail>();
			ProductionHeader.ProductionOverHeadsDetailList = new List<ProductionOverHeadsDetail>();
			ProductionHeader.ProductionOutPutDetailList = new List<ProductionOutPutDetail>();
			ProductionHeader.ProductionWagesDetailList = new List<ProductionWagesDetail>();
			ProductionHeader.AttachmentsList = new List<DMSAttachments>();
			GridEXRow[] rows = grdContractorWages.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt(r.Cells["ContractorWagesRateScheduleId"].Value) > 0)
				{
					ProductionWagesDetail d = new ProductionWagesDetail();
					if (r.Cells["ContractorWagesRateScheduleId"].Text == string.Empty || Conversion.ToInt(r.Cells["ContractorWagesRateScheduleId"].Value) == 0)
					{
						MessageBox.Show("Service Activity Field Required in Grid Contractor Wages Row No: " + (r.RowIndex + 1));
						return;
					}
					d.Id = Conversion.ToInt(r.Cells["Id"].Value);
					d.BomWagesId = Conversion.ToInt(r.Cells["BomWagesId"].Value);
					d.ContractorWagesRateScheduleId = Conversion.ToInt(r.Cells["ContractorWagesRateScheduleId"].Value);
					d.ItemId = Conversion.ToInt(CmbItem.Value);
					if (r.Cells["ContractorId"].Text == string.Empty || Conversion.ToInt(r.Cells["ContractorId"].Value) == 0)
					{
						MessageBox.Show("Contractor  Field Required in Grid Contractor Wages Row No: " + (r.RowIndex + 1));
						return;
					}
					d.ContractorId = Conversion.ToInt(r.Cells["ContractorId"].Value);
					if (r.Cells["Amount"].Text == string.Empty || Conversion.ToDecimal(r.Cells["Amount"].Value) == 0m)
					{
						MessageBox.Show("Amount Field Required in Grid Contractor Wages Row No: " + (r.RowIndex + 1));
						return;
					}
					d.Qty = Conversion.ToDecimal((object)r.Cells["Qty"].Text);
					d.BomQty = Conversion.ToDecimal((object)r.Cells["BomQty"].Text);
					d.Rate = Conversion.ToDecimal((object)r.Cells["Rate"].Text);
					d.Amount = Conversion.ToDecimal((object)r.Cells["Amount"].Text);
					d.Remarks = Conversion.ToString((object)r.Cells["Remarks"].Text);
					ProductionHeader.ProductionWagesDetailList.Add(d);
				}
			}
			int success = ProductionHeader.Save(ProductionHeader);
			ProductionHeader.Id = success;
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + ProductionHeader.DocNo);
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + ProductionHeader.DocNo);
			}
			FromReset();
			if (ChkPreviewSlip.Checked && formright.DoHavePrintRights)
			{
				CommonServices.ProductionSlipConcrete(ProductionHeader.Id);
			}
			if (chkPrintVoucher.Checked && formright.DoHavePrintRights)
			{
				VoucherReprot(ProductionHeader.Id);
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

	public void ReadById(int Id)
	{
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					ProductionHeader.DeleteByID(UserAccount.ID, RecId);
					MessageBox.Show("Delete Record Seccessfully");
					FromReset();
				}
			}
			else
			{
				MessageBox.Show("No record found to Delete");
			}
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
			CommonServices.ProductionSlipConcrete(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnVoucherReport_Click(object sender, EventArgs e)
	{
		try
		{
			VoucherReprot(RecId);
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
			CommonServices.VoucherReport_118(Conversion.ToInt((object)CommonServices.VoucherHeadIdGet(Id, 1850)), 1850);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllCombobind()
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		try
		{
			int Idtype = Conversion.ToInt(cmbProductionNoHistory.Value);
			int Idname = Conversion.ToInt(CmbBomNoHistory.Value);
			int Iditem = Conversion.ToInt(cmbItemhistory.Value);
			DataTable dtcombo = ProductionHeader.GetDataForDropDownFromProductionHeader(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtcombo.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtProductionNo = new DataTable();
			dtProductionNo.Columns.Add("Id", typeof(int));
			dtProductionNo.Columns.Add("ProductionNo");
			DataTable dtBOMName = new DataTable();
			dtBOMName.Columns.Add("Id", typeof(int));
			dtBOMName.Columns.Add("BOMName");
			DataTable dtitem = new DataTable();
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName");
			for (int i = 0; i < dtcombo.Rows.Count; i++)
			{
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "ProductionNo")
				{
					dtProductionNo.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "BOMName")
				{
					dtBOMName.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "ItemHeader")
				{
					dtitem.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
			}
			if (dtProductionNo.Rows.Count > 0)
			{
				DDL.BindDDL(dtProductionNo, cmbProductionNoHistory, "Id", "ProductionNo", "Production No", ZeroIndex: true);
				((UltraGridBase)cmbProductionNoHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (Idtype > 0)
				{
					if (dtitem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Idtype))
					{
						cmbProductionNoHistory.Value = Idtype;
					}
					else
					{
						((Control)(object)cmbProductionNoHistory).Text = string.Empty;
					}
				}
			}
			if (dtBOMName.Rows.Count > 0)
			{
				DDL.BindDDL(dtBOMName, CmbBomNoHistory, "Id", "BOMName", "BOM Name", ZeroIndex: true);
				((UltraGridBase)CmbBomNoHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (Idname > 0)
				{
					if (dtitem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Idname))
					{
						CmbBomNoHistory.Value = Idname;
					}
					else
					{
						((Control)(object)CmbBomNoHistory).Text = string.Empty;
					}
				}
			}
			if (dtitem.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtitem, cmbItemhistory, "Id", "ItemName", "Item Name", ZeroIndex: true);
			((UltraGridBase)cmbItemhistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (Iditem > 0)
			{
				if (dtitem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Iditem))
				{
					cmbItemhistory.Value = Iditem;
				}
				else
				{
					((Control)(object)cmbItemhistory).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Datetypefill()
	{
		try
		{
			DataTable dt = CommonServices.DateType();
			DDL.BindDDL(dt, cmbDateTypeHistory, "Id", "Parameters", "Parameters", ZeroIndex: true);
			((UltraGridBase)cmbDateTypeHistory).Rows[2].Activate();
			((UltraGridBase)cmbDateTypeHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbDateTypeHistory_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbDateTypeHistory.Value) == 1)
			{
				txtFromdateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 2)
			{
				txtFromdateHistory.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 3)
			{
				DateTime now = DateTime.Now;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				txtFromdateHistory.Value = first;
				txtToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				txtFromdateHistory.Value = first2;
				txtToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 5)
			{
				txtFromdateHistory.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
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
			Datetypefill();
			AllCombobind();
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
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
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			dtHistory.Clear();
			ReportsParameters header = new ReportsParameters();
			header.CompanyId = UserAccount.CompanyId;
			header.OrganizationId = UserAccount.OrganizationId;
			header.BranchesId = UserAccount.BranchesId;
			header.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			header.DocumentTypeId = 1850;
			header.FromDocNo = Conversion.ToInt((object)txtFromNoHistory.Text.Trim());
			header.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text.Trim());
			header.FromDate = Conversion.ToDateTime((object)txtFromdateHistory.Value);
			header.ToDate = Conversion.ToDateTime((object)txtToDateHistory.Value);
			header.Id = Conversion.ToInt(cmbProductionNoHistory.Value);
			header.BOMHeaderId = Conversion.ToInt(CmbBomNoHistory.Value);
			header.ItemId = Conversion.ToInt(cmbItemhistory.Value);
			header.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (RadReffered.Checked)
			{
				header.ActionId = 1;
			}
			else if (RadNotReffered.Checked)
			{
				header.ActionId = 2;
			}
			else if (RadAll.Checked)
			{
				header.ActionId = 0;
			}
			dtHistory = ProductionHeader.FormHistory(header);
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("RecordNo", typeof(int));
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("DocumentTypeId", typeof(int));
				dtcol.Columns.Add("DocNo", typeof(string));
				dtcol.Columns.Add("DocDate", typeof(DateTime));
				dtcol.Columns.Add("ProductionNo", typeof(string));
				dtcol.Columns.Add("ReleaseNo", typeof(int));
				dtcol.Columns.Add("ProductionType", typeof(string));
				dtcol.Columns.Add("PlantName", typeof(string));
				dtcol.Columns.Add("WorkOrderId", typeof(int));
				dtcol.Columns.Add("BomHeaderId", typeof(int));
				dtcol.Columns.Add("BomHeaderNo", typeof(string));
				dtcol.Columns.Add("ItemId", typeof(int));
				dtcol.Columns.Add("ItemName", typeof(string));
				dtcol.Columns.Add("ItemCode", typeof(string));
				dtcol.Columns.Add("UOM", typeof(string));
				dtcol.Columns.Add("ItemQty", typeof(decimal));
				dtcol.Columns.Add("WipWareHouseName", typeof(string));
				dtcol.Columns.Add("WIPAccountTitle", typeof(string));
				dtcol.Columns.Add("RemarksHeader", typeof(string));
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("EntryUserName", typeof(string));
				dtcol.Columns.Add("ModifyDate", typeof(DateTime));
				dtcol.Columns.Add("ModifyUserName", typeof(string));
				dtcol.Columns.Add("IsApproved", typeof(bool));
				dtcol.Columns.Add("ApprovedDate", typeof(DateTime));
				dtcol.Columns.Add("NoOfAttachments", typeof(int));
				dtcol.Rows.Clear();
				foreach (DataRow row in dtHistory.Rows)
				{
					dtcol.Rows.Add(row["RecordNo"], row["Id"], row["DocumentTypeId"], row["DocNo"], row["DocDate"], row["ProductionNo"], row["ReleaseNo"], row["ProductionType"], row["PlantName"], row["WorkOrderId"], row["BomHeaderId"], row["BomHeaderNo"], row["ItemId"], row["ItemName"], row["ItemCode"], row["UOM"], row["ItemQty"], row["WareHouseName"], row["WIPAccountTitle"], row["RemarksHeader"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"], row["IsApproved"], row["ApprovedDate"], row["NoOfAttachments"]);
				}
				DatagridHistory.DataSource = dtcol;
				DatagridHistory.RetrieveStructure();
				HistoryGridSetting();
			}
			else
			{
				DatagridHistory.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSetting()
	{
		try
		{
			DatagridHistory.RootTable.Columns["Id"].Visible = false;
			DatagridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DatagridHistory.RootTable.Columns["WorkOrderId"].Visible = false;
			DatagridHistory.RootTable.Columns["BomHeaderId"].Visible = false;
			DatagridHistory.RootTable.Columns["ItemId"].Visible = false;
			DatagridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DatagridHistory.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			DatagridHistory.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			DatagridHistory.RootTable.Columns["ItemQty"].FormatString = "##,#.####";
			DatagridHistory.RootTable.Columns["ItemQty"].TotalFormatString = "##,#.####";
			CommonServices.GridAutoAdjustmentNew(DatagridHistory);
			DatagridHistory.RootTable.Columns.Add("Print");
			DatagridHistory.RootTable.Columns["Print"].Key = "Print";
			DatagridHistory.RootTable.Columns["Print"].Caption = "Print";
			DatagridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DatagridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DatagridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DatagridHistory.RootTable.Columns["Print"].Width = 50;
			DatagridHistory.RootTable.Columns["Print"].Position = 0;
			DatagridHistory.RootTable.Columns.Add("Voucher");
			DatagridHistory.RootTable.Columns["Voucher"].Key = "Voucher";
			DatagridHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
			DatagridHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DatagridHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			DatagridHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			DatagridHistory.RootTable.Columns["Voucher"].Width = 70;
			DatagridHistory.RootTable.Columns["Voucher"].Position = 1;
			DatagridHistory.RootTable.Columns.Add("Edit");
			DatagridHistory.RootTable.Columns["Edit"].Key = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DatagridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DatagridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].Width = 50;
			DatagridHistory.RootTable.Columns["Edit"].Position = 2;
			DatagridHistory.FrozenColumns = 3;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			if (formright.DoHaveUpdateRights)
			{
				if (Conversion.ToInt(DatagridHistory.CurrentRow.Cells["ReleaseNo"].Value) > 0)
				{
					MessageBox.Show("You can't update record because this document is referred in release", "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else
				{
					ReadById(Conversion.ToInt((object)DatagridHistory.CurrentRow.Cells["Id"].Value.ToString()));
				}
				return;
			}
			throw new Exception("ypu don't have updae rights...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Print")
			{
				CommonServices.ProductionSlipConcrete(Conversion.ToInt((object)DatagridHistory.CurrentRow.Cells["Id"].Value.ToString()));
			}
			if (e.Column.Key == "Voucher")
			{
				VoucherReprot(Conversion.ToInt((object)DatagridHistory.CurrentRow.Cells["Id"].Value.ToString()));
			}
			if (e.Column.Key == "Edit")
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("ypu don't have updae rights...");
				}
				if (Conversion.ToInt(DatagridHistory.CurrentRow.Cells["ReleaseNo"].Value) > 0)
				{
					MessageBox.Show("You can't update record because this document is referred in release", "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else
				{
					ReadById(Conversion.ToInt((object)DatagridHistory.CurrentRow.Cells["Id"].Value.ToString()));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DatagridHistory.CurrentRow.Cells["Id"].Value), 1850);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				DatagridHistoryDetailbind(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistoryDetailbind(int Id)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		try
		{
			grdHistoryInput.ClearStructure();
			grdHistoryOutput.ClearStructure();
			grdHistoryOverHead.ClearStructure();
			grdWagesHistory.ClearStructure();
			ProductionHeader ProductionHeader = new ProductionHeader();
			ProductionHeader = ProductionHeader.ReadById(Id);
			if (ProductionHeader == null)
			{
				return;
			}
			DataTable dtInputHistory = new DataTable();
			DataTable dtOutPutByProductHistory = new DataTable();
			DataTable dtOverHeadHistory = new DataTable();
			DataTable dtWagesHistory = new DataTable();
			new DataTable();
			dtInputHistory.Columns.Add("Id", typeof(int));
			dtInputHistory.Columns.Add("WarehouseName", typeof(string));
			dtInputHistory.Columns.Add("JobLot", typeof(string));
			dtInputHistory.Columns.Add("ItemId", typeof(int));
			dtInputHistory.Columns.Add("ItemName", typeof(string));
			dtInputHistory.Columns.Add("Uom", typeof(string));
			dtInputHistory.Columns.Add("ItemQty", typeof(decimal));
			dtInputHistory.Columns.Add("NetWeight", typeof(decimal));
			dtInputHistory.Columns.Add("Rate", typeof(decimal));
			dtInputHistory.Columns.Add("RateUom", typeof(string));
			dtInputHistory.Columns.Add("Amount", typeof(decimal));
			dtOutPutByProductHistory.Columns.Add("Id", typeof(int));
			dtOutPutByProductHistory.Columns.Add("EntryType", typeof(string));
			dtOutPutByProductHistory.Columns.Add("SaleOrderId", typeof(int));
			dtOutPutByProductHistory.Columns.Add("SaleOrder", typeof(int));
			dtOutPutByProductHistory.Columns.Add("WarehouseName", typeof(string));
			dtOutPutByProductHistory.Columns.Add("JobLot", typeof(string));
			dtOutPutByProductHistory.Columns.Add("ItemId", typeof(int));
			dtOutPutByProductHistory.Columns.Add("ItemName", typeof(string));
			dtOutPutByProductHistory.Columns.Add("AttributeVarient", typeof(string));
			dtOutPutByProductHistory.Columns.Add("VarientUnit", typeof(string));
			dtOutPutByProductHistory.Columns.Add("Uom", typeof(string));
			dtOutPutByProductHistory.Columns.Add("ItemQty", typeof(decimal));
			dtOutPutByProductHistory.Columns.Add("VarientQty", typeof(decimal));
			dtOutPutByProductHistory.Columns.Add("NetWeight", typeof(decimal));
			dtOutPutByProductHistory.Columns.Add("Rate", typeof(decimal));
			dtOutPutByProductHistory.Columns.Add("RateUom", typeof(string));
			dtOutPutByProductHistory.Columns.Add("Amount", typeof(decimal));
			dtOverHeadHistory.Columns.Add("Id", typeof(int));
			dtOverHeadHistory.Columns.Add("OverHeadType", typeof(string));
			dtOverHeadHistory.Columns.Add("OverHeadAccount", typeof(string));
			dtOverHeadHistory.Columns.Add("Percentage", typeof(decimal));
			dtOverHeadHistory.Columns.Add("Qty", typeof(decimal));
			dtOverHeadHistory.Columns.Add("Rate", typeof(decimal));
			dtOverHeadHistory.Columns.Add("Amount", typeof(decimal));
			dtOverHeadHistory.Columns.Add("OhRemarks", typeof(string));
			dtWagesHistory.Columns.Add("Id", typeof(int));
			dtWagesHistory.Columns.Add("ContractorName");
			dtWagesHistory.Columns.Add("ItemName");
			dtWagesHistory.Columns.Add("ServiceActivity");
			dtWagesHistory.Columns.Add("Qty", typeof(decimal));
			dtWagesHistory.Columns.Add("Rate", typeof(decimal));
			dtWagesHistory.Columns.Add("Amount", typeof(decimal));
			dtWagesHistory.Columns.Add("Remarks");
			if (ProductionHeader.ProductionInputDetailList.Count > 0)
			{
				for (int l = 0; l < ProductionHeader.ProductionInputDetailList.Count; l++)
				{
					dtInputHistory.Rows.Add(ProductionHeader.ProductionInputDetailList[l].Id, ProductionHeader.ProductionInputDetailList[l].WareHouseName, ProductionHeader.ProductionInputDetailList[l].JobLotDescription, ProductionHeader.ProductionInputDetailList[l].ItemId, ProductionHeader.ProductionInputDetailList[l].ItemName, ProductionHeader.ProductionInputDetailList[l].ItemUomCode, ProductionHeader.ProductionInputDetailList[l].ItemQty, ProductionHeader.ProductionInputDetailList[l].NetWeight, ProductionHeader.ProductionInputDetailList[l].Rate, ProductionHeader.ProductionInputDetailList[l].RateUomCode, ProductionHeader.ProductionInputDetailList[l].Amount);
				}
			}
			else
			{
				grdHistoryInput.ClearStructure();
			}
			if (ProductionHeader.ProductionOutPutDetailList.Count > 0)
			{
				for (int k = 0; k < ProductionHeader.ProductionOutPutDetailList.Count; k++)
				{
					dtOutPutByProductHistory.Rows.Add(ProductionHeader.ProductionOutPutDetailList[k].Id, ProductionHeader.ProductionOutPutDetailList[k].EntryType, ProductionHeader.ProductionOutPutDetailList[k].SaleOrderId, ProductionHeader.ProductionOutPutDetailList[k].SaleOrderNo, ProductionHeader.ProductionOutPutDetailList[k].WareHouseName, ProductionHeader.ProductionOutPutDetailList[k].JobLotDescription, ProductionHeader.ProductionOutPutDetailList[k].ItemId, ProductionHeader.ProductionOutPutDetailList[k].ItemName, ProductionHeader.ProductionOutPutDetailList[k].ItemAttribute, ProductionHeader.ProductionOutPutDetailList[k].VarientEquivalent, ProductionHeader.ProductionOutPutDetailList[k].ItemUomCode, ProductionHeader.ProductionOutPutDetailList[k].ItemQty, ProductionHeader.ProductionOutPutDetailList[k].VarientQty, ProductionHeader.ProductionOutPutDetailList[k].NetWeight, ProductionHeader.ProductionOutPutDetailList[k].Rate, ProductionHeader.ProductionOutPutDetailList[k].RateUomCode, ProductionHeader.ProductionOutPutDetailList[k].Amount);
				}
			}
			else
			{
				grdHistoryOutput.ClearStructure();
			}
			if (ProductionHeader.ProductionOverHeadsDetailList.Count > 0)
			{
				for (int j = 0; j < ProductionHeader.ProductionOverHeadsDetailList.Count; j++)
				{
					dtOverHeadHistory.Rows.Add(ProductionHeader.ProductionOverHeadsDetailList[j].Id, ProductionHeader.ProductionOverHeadsDetailList[j].OverHeadTypeId, ProductionHeader.ProductionOverHeadsDetailList[j].OverHeadAccountTitle, ProductionHeader.ProductionOverHeadsDetailList[j].PercentOfTotalInput, ProductionHeader.ProductionOverHeadsDetailList[j].Qty, ProductionHeader.ProductionOverHeadsDetailList[j].Rate, ProductionHeader.ProductionOverHeadsDetailList[j].Amount, ProductionHeader.ProductionOverHeadsDetailList[j].OhRemarks);
				}
			}
			else
			{
				grdHistoryOverHead.ClearStructure();
			}
			if (ProductionHeader.ProductionWagesDetailList.Count > 0)
			{
				for (int i = 0; i < ProductionHeader.ProductionWagesDetailList.Count; i++)
				{
					dtWagesHistory.Rows.Add(ProductionHeader.ProductionWagesDetailList[i].Id, ProductionHeader.ProductionWagesDetailList[i].ContractorName, ProductionHeader.ProductionWagesDetailList[i].ItemName, ProductionHeader.ProductionWagesDetailList[i].ServiceActivity, ProductionHeader.ProductionWagesDetailList[i].Qty, ProductionHeader.ProductionWagesDetailList[i].Rate, ProductionHeader.ProductionWagesDetailList[i].Amount, ProductionHeader.ProductionWagesDetailList[i].Remarks);
				}
			}
			else
			{
				grdWagesHistory.ClearStructure();
			}
			grdHistoryInput.DataSource = dtInputHistory;
			grdHistoryInput.RetrieveStructure();
			grdHistoryInputSetting();
			grdHistoryOutput.DataSource = dtOutPutByProductHistory;
			grdHistoryOutput.RetrieveStructure();
			grdHistoryOutputSetting();
			grdHistoryOverHead.DataSource = dtOverHeadHistory;
			grdHistoryOverHead.RetrieveStructure();
			grdHistoryOverHeadSetting();
			grdWagesHistory.DataSource = dtWagesHistory;
			grdWagesHistory.RetrieveStructure();
			grdWagesHistorySetting();
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
			grdHistoryInput.RootTable.Columns["Id"].Visible = false;
			grdHistoryInput.RootTable.Columns["ItemId"].Visible = false;
			grdHistoryInput.RootTable.Columns["NetWeight"].Visible = false;
			grdHistoryInput.RootTable.Columns["JobLot"].Visible = false;
			grdHistoryInput.RootTable.Columns["RateUom"].Visible = false;
			grdHistoryInput.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryInput.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdHistoryInput.RootTable.Columns["ItemQty"].FormatString = "##,#.####";
			grdHistoryInput.RootTable.Columns["ItemQty"].TotalFormatString = "##,#.####";
			grdHistoryInput.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)3;
			grdHistoryInput.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdHistoryInput.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryInput.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryInput.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryInput.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryInput.RootTable.Columns["NetWeight"].FormatString = "##,#.####";
			grdHistoryInput.RootTable.Columns["NetWeight"].TotalFormatString = "##,#.####";
			grdHistoryInput.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdHistoryInput.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdHistoryInput.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistoryInput.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdHistoryInput);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryOutputSetting()
	{
		try
		{
			grdHistoryOutput.RootTable.Columns["Id"].Visible = false;
			grdHistoryOutput.RootTable.Columns["ItemId"].Visible = false;
			grdHistoryOutput.RootTable.Columns["EntryType"].Visible = false;
			grdHistoryOutput.RootTable.Columns["SaleOrderId"].Visible = false;
			grdHistoryOutput.RootTable.Columns["NetWeight"].Visible = false;
			grdHistoryOutput.RootTable.Columns["RateUom"].Visible = false;
			grdHistoryOutput.RootTable.Columns["Uom"].Visible = false;
			grdHistoryOutput.RootTable.Columns["JobLot"].Visible = false;
			grdHistoryOutput.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOutput.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdHistoryOutput.RootTable.Columns["ItemQty"].FormatString = "##,#.####";
			grdHistoryOutput.RootTable.Columns["ItemQty"].TotalFormatString = "##,#.####";
			grdHistoryOutput.RootTable.Columns["VarientQty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOutput.RootTable.Columns["VarientQty"].TextAlignment = (TextAlignment)3;
			grdHistoryOutput.RootTable.Columns["VarientQty"].FormatString = "##,#.####";
			grdHistoryOutput.RootTable.Columns["VarientQty"].TotalFormatString = "##,#.####";
			grdHistoryOutput.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOutput.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdHistoryOutput.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryOutput.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryOutput.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryOutput.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOutput.RootTable.Columns["NetWeight"].FormatString = "##,#.####";
			grdHistoryOutput.RootTable.Columns["NetWeight"].TotalFormatString = "##,#.####";
			grdHistoryOutput.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdHistoryOutput.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOutput.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistoryOutput.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdHistoryOutput);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryOverHeadSetting()
	{
		try
		{
			grdHistoryOverHead.RootTable.Columns["Id"].Visible = false;
			grdHistoryOverHead.RootTable.Columns["OverHeadType"].Visible = false;
			grdHistoryOverHead.RootTable.Columns["Percentage"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOverHead.RootTable.Columns["Percentage"].TextAlignment = (TextAlignment)3;
			grdHistoryOverHead.RootTable.Columns["Percentage"].FormatString = "##,#.####";
			grdHistoryOverHead.RootTable.Columns["Percentage"].TotalFormatString = "##,#.####";
			grdHistoryOverHead.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOverHead.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdHistoryOverHead.RootTable.Columns["Qty"].FormatString = "##,#.####";
			grdHistoryOverHead.RootTable.Columns["Qty"].TotalFormatString = "##,#.####";
			grdHistoryOverHead.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdHistoryOverHead.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryOverHead.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryOverHead.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdHistoryOverHead.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOverHead.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistoryOverHead.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdHistoryOverHead);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdWagesHistorySetting()
	{
		try
		{
			grdWagesHistory.RootTable.Columns["Id"].Visible = false;
			grdWagesHistory.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdWagesHistory.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdWagesHistory.RootTable.Columns["Qty"].FormatString = "##,#.####";
			grdWagesHistory.RootTable.Columns["Qty"].TotalFormatString = "##,#.####";
			grdWagesHistory.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdWagesHistory.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdWagesHistory.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdWagesHistory.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdWagesHistory.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdWagesHistory.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdWagesHistory.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdWagesHistory);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnHistoryRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)cmbDateTypeHistory).Text = string.Empty;
			((Control)(object)cmbItemhistory).Text = string.Empty;
			((Control)(object)cmbProductionNoHistory).Text = string.Empty;
			((Control)(object)CmbBomNoHistory).Text = string.Empty;
			grdHistoryOverHead.ClearStructure();
			grdHistoryOutput.ClearStructure();
			grdHistoryInput.ClearStructure();
			cmbDateTypeHistory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmBillOfMaterial_Manufacturing_KeyDown(object sender, KeyEventArgs e)
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
					CmbProductionNo.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					((Control)(object)DatagridHistory).Focus();
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
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled && UpdateMode)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.P && btnPrint.Visible && btnPrint.Enabled)
				{
					btnPrint_Click(null, null);
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
					CmbProductionNo.Focus();
				}
				if (e.Control)
				{
					_ = e.KeyCode;
					_ = 123;
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdContractorWages).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					CmbProductionNo.Focus();
				}
			}
			else
			{
				if (tabControl1.SelectedIndex != 1)
				{
					return;
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)DatagridHistory).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.S)
				{
					btnshow_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					((Control)(object)grdHistoryOutput).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grdHistoryOutput).Focused)
					{
						((Control)(object)grdHistoryInput).Focus();
					}
					else if (((Control)(object)grdHistoryInput).Focused)
					{
						((Control)(object)grdWagesHistory).Focus();
					}
					else if (((Control)(object)grdWagesHistory).Focused)
					{
						((Control)(object)grdHistoryOverHead).Focus();
					}
					else if (((Control)(object)grdHistoryOverHead).Focused)
					{
						((Control)(object)grdHistoryOutput).Focus();
					}
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
			dt.Rows.Add("F1", "For Combo Lookup on Current Column of any Focused Grid");
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

	private void grdContractorWages_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdContractorWages.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdContractorWages.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grdContractorWages.CurrentColumn.Key == "Add")
				{
					AddRowInContractorWagesGrid();
				}
				if (grdContractorWages.CurrentColumn.Key == "Delete")
				{
					dtContractorWages.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					if (dtContractorWages.Rows.Count == 0)
					{
						AddRowInContractorWagesGrid();
					}
				}
				grdContractorWages.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInContractorWagesGrid();
				grdContractorWages.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				dtContractorWages.Rows[r.RowIndex].Delete();
				if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				if (dtContractorWages.Rows.Count == 0)
				{
					AddRowInContractorWagesGrid();
				}
				grdContractorWages.UpdateData();
			}
			if (e.KeyCode != Keys.F1)
			{
				return;
			}
			int Id = 0;
			string Name = "";
			if (grdContractorWages.CurrentColumn != null && (grdContractorWages.CurrentColumn.Key == "ContractorId" || grdContractorWages.CurrentColumn.Key == "ContractorName"))
			{
				GrdPopUp obj2 = new GrdPopUp(dtContr, "Id", "CompanyName");
				obj2.StartPosition = FormStartPosition.CenterScreen;
				obj2.ShowDialog();
				obj2.BringToFront();
				Id = Conversion.ToInt((object)obj2.ReturnId);
				Name = Conversion.ToString((object)obj2.ReturnName);
				r.Cells["ContractorId"].Value = Id;
				r.Cells["ContractorName"].Value = Name;
				string ServiceActivityId = Conversion.ToString(r.Cells["ServiceActivity"].Value);
				DataTable dt2 = ContractorWagesRateSchedule.GetServiceActivityByItemId(1, Conversion.ToInt(CmbItem.Value), Conversion.ToDateTime((object)txtProductionDate.Value), 0, Id);
				if (dt2.Rows.Count > 0)
				{
					DataRow foundRow2 = dt2.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<string>("WagesActivity") == ServiceActivityId);
					decimal obj3 = ((foundRow2 != null) ? Conversion.ToDecimal(foundRow2["Rate"]) : 0m);
					int ContractorWagesRateScheduleId = ((foundRow2 != null) ? Conversion.ToInt(foundRow2["ContractorWagesRateScheduleId"]) : 0);
					r.Cells["ContractorWagesRateScheduleId"].Value = ContractorWagesRateScheduleId;
					decimal Qty2 = ((foundRow2 != null) ? Conversion.ToDecimal(foundRow2["Qty"]) : 0m);
					decimal RatePerQty2 = obj3 / Qty2;
					r.Cells["Rate"].Value = RatePerQty2;
					decimal CurrRowQty2 = Conversion.ToInt(r.Cells["Qty"].Value);
					decimal Amount2 = RatePerQty2 * CurrRowQty2;
					r.Cells["Amount"].Value = Amount2;
				}
			}
			if (grdContractorWages.CurrentColumn != null && grdContractorWages.CurrentColumn.Key == "ServiceActivity")
			{
				int ContrId = Conversion.ToInt(r.Cells["ContractorId"].Value);
				DataTable dt = ContractorWagesRateSchedule.GetServiceActivityByItemId(1, Conversion.ToInt(CmbItem.Value), Conversion.ToDateTime((object)txtProductionDate.Value), 0, ContrId);
				GrdPopUp obj = new GrdPopUp(dt, "ContractorWagesRateScheduleId", "WagesActivity");
				obj.StartPosition = FormStartPosition.CenterScreen;
				obj.ShowDialog();
				obj.BringToFront();
				Id = Conversion.ToInt((object)obj.ReturnId);
				Name = Conversion.ToString((object)obj.ReturnName);
				r.Cells["ContractorWagesRateScheduleId"].Value = Id;
				r.Cells["ServiceActivity"].Value = Name;
				if (Id > 0)
				{
					DataRow foundRow = dt.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("ContractorWagesRateScheduleId") == Id);
					decimal Qty = ((foundRow != null) ? Conversion.ToDecimal(foundRow["Qty"]) : 0m);
					decimal RatePerQty = ((foundRow != null) ? Conversion.ToDecimal(foundRow["Rate"]) : 0m) / Qty;
					r.Cells["Rate"].Value = RatePerQty;
					decimal CurrRowQty = Conversion.ToInt(r.Cells["Qty"].Value);
					decimal Amount = RatePerQty * CurrRowQty;
					r.Cells["Amount"].Value = Amount;
				}
				else
				{
					r.Cells["Rate"].Value = 0;
					r.Cells["Amount"].Value = 0;
				}
			}
			grdContractorWages.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || DatagridHistory.CurrentColumn == null)
			{
				return;
			}
			int Id = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (DatagridHistory.CurrentColumn.Key == "Edit")
				{
					if (!formright.DoHaveUpdateRights)
					{
						throw new Exception("you don't have update rights...");
					}
					if (Conversion.ToInt(DatagridHistory.CurrentRow.Cells["ReleaseNo"].Value) > 0)
					{
						MessageBox.Show("You can't update record because this document is referred in release", "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					ReadById(Conversion.ToInt((object)Id));
				}
				if (DatagridHistory.CurrentColumn.Key == "Print")
				{
					CommonServices.ProductionSlipConcrete(Conversion.ToInt(DatagridHistory.CurrentRow.Cells["Id"].Value));
				}
				if (DatagridHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, 1850);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("you don't have update rights...");
				}
				if (Conversion.ToInt(DatagridHistory.CurrentRow.Cells["ReleaseNo"].Value) > 0)
				{
					MessageBox.Show("You can't update record because this document is referred in release", "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else
				{
					ReadById(Conversion.ToInt((object)Id));
				}
			}
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DatagridHistory).Name);
			if (fs != null)
			{
				DatagridHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateWagesRates()
	{
		try
		{
			if (grdContractorWages.RowCount <= 0)
			{
				return;
			}
			GridEXRow[] rows = grdContractorWages.GetRows();
			foreach (GridEXRow r in rows)
			{
				int ContrId = Conversion.ToInt(r.Cells["ContractorId"].Value);
				int ServiceActivityId = Conversion.ToInt(r.Cells["ContractorWagesRateScheduleId"].Value);
				r.BeginEdit();
				if (ServiceActivityId > 0)
				{
					DataTable dt = ContractorWagesRateSchedule.GetServiceActivityByItemId(1, Conversion.ToInt(CmbItem.Value), Conversion.ToDateTime((object)txtProductionDate.Value), ServiceActivityId, ContrId);
					if (dt.Rows.Count > 0)
					{
						DataRow foundRow = dt.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("ContractorWagesRateScheduleId") == ServiceActivityId);
						decimal Qty = ((foundRow != null) ? Conversion.ToDecimal(foundRow["Qty"]) : 0m);
						decimal RatePerQty = ((foundRow != null) ? Conversion.ToDecimal(foundRow["Rate"]) : 0m) / Qty;
						r.Cells["Rate"].Value = RatePerQty;
						decimal CurrRowQty = Conversion.ToInt(r.Cells["Qty"].Value);
						decimal Amount = RatePerQty * CurrRowQty;
						r.Cells["Amount"].Value = Amount;
					}
					else
					{
						r.Cells["Rate"].Value = 0;
						r.Cells["Amount"].Value = 0;
					}
				}
				else
				{
					r.Cells["Rate"].Value = 0;
					r.Cells["Amount"].Value = 0;
				}
				r.EndEdit();
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
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Expected O, but got Unknown
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Expected O, but got Unknown
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Expected O, but got Unknown
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Expected O, but got Unknown
		//IL_0441: Unknown result type (might be due to invalid IL or missing references)
		//IL_044b: Expected O, but got Unknown
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Expected O, but got Unknown
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0498: Expected O, but got Unknown
		//IL_04af: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b9: Expected O, but got Unknown
		//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04da: Expected O, but got Unknown
		//IL_053e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0548: Expected O, but got Unknown
		//IL_055f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Expected O, but got Unknown
		//IL_056a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Expected O, but got Unknown
		//IL_058b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0595: Expected O, but got Unknown
		//IL_0c7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c87: Expected O, but got Unknown
		//IL_0c94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9e: Expected O, but got Unknown
		//IL_336e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3378: Expected O, but got Unknown
		//IL_339c: Unknown result type (might be due to invalid IL or missing references)
		//IL_33a6: Expected O, but got Unknown
		GridEXLayout grdContractorWages_DesignTimeLayout = new GridEXLayout();
		Appearance appearance35 = new Appearance();
		Appearance appearance39 = new Appearance();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		GridEXLayout grdHistoryOverHead_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryInput_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdWagesHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryOutput_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DatagridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel17 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.TabWages = new System.Windows.Forms.TabPage();
		this.grdContractorWages = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label33 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.BtnLoadInputFromBom = new System.Windows.Forms.Button();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.txtDocNo = new System.Windows.Forms.TextBox();
		this.txtOutputTotalQty = new System.Windows.Forms.TextBox();
		this.CmbProductionType = new UltraCombo();
		this.txtProductionDate = new System.Windows.Forms.DateTimePicker();
		this.CmbItem = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.CmbProductionNo = new UltraCombo();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnVoucherReport = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.label21 = new System.Windows.Forms.Label();
		this.chkPrintVoucher = new System.Windows.Forms.CheckBox();
		this.ChkPreviewSlip = new System.Windows.Forms.CheckBox();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel45 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdHistoryOverHead = new GridEX();
		this.panel42 = new System.Windows.Forms.Panel();
		this.label34 = new System.Windows.Forms.Label();
		this.grdHistoryInput = new GridEX();
		this.panel44 = new System.Windows.Forms.Panel();
		this.label14 = new System.Windows.Forms.Label();
		this.panel19 = new System.Windows.Forms.Panel();
		this.grdWagesHistory = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.label40 = new System.Windows.Forms.Label();
		this.grdHistoryOutput = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.DatagridHistory = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label17 = new System.Windows.Forms.Label();
		this.panel13 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.RadNotReffered = new System.Windows.Forms.RadioButton();
		this.RadReffered = new System.Windows.Forms.RadioButton();
		this.RadAll = new System.Windows.Forms.RadioButton();
		this.btnshow = new System.Windows.Forms.Button();
		this.cmbItemhistory = new UltraCombo();
		this.label29 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.cmbProductionNoHistory = new UltraCombo();
		this.CmbBomNoHistory = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.cmbDateTypeHistory = new UltraCombo();
		this.txtToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label28 = new System.Windows.Forms.Label();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromdateHistory = new System.Windows.Forms.DateTimePicker();
		this.label35 = new System.Windows.Forms.Label();
		this.txtFromNoHistory = new System.Windows.Forms.TextBox();
		this.panel38 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnRegister = new System.Windows.Forms.ToolStripButton();
		this.label32 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel17.SuspendLayout();
		this.panel10.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.TabWages.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdContractorWages).BeginInit();
		this.panel5.SuspendLayout();
		this.panel2.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionNo).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel45.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryOverHead).BeginInit();
		this.panel42.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryInput).BeginInit();
		this.panel44.SuspendLayout();
		this.panel19.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdWagesHistory).BeginInit();
		this.panel18.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryOutput).BeginInit();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).BeginInit();
		this.panel15.SuspendLayout();
		this.panel13.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemhistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbProductionNoHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBomNoHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).BeginInit();
		this.panel38.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(964, 711);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(964, 711);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel17);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(956, 682);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel17.Controls.Add(this.panel10);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel17.Location = new System.Drawing.Point(3, 178);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(950, 501);
		this.panel17.TabIndex = 4;
		this.panel10.Controls.Add(this.tabControl2);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(950, 501);
		this.panel10.TabIndex = 3;
		this.tabControl2.Controls.Add(this.TabWages);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(950, 501);
		this.tabControl2.TabIndex = 0;
		this.TabWages.Controls.Add((System.Windows.Forms.Control)(object)this.grdContractorWages);
		this.TabWages.Controls.Add(this.panel5);
		this.TabWages.Location = new System.Drawing.Point(4, 25);
		this.TabWages.Name = "TabWages";
		this.TabWages.Padding = new System.Windows.Forms.Padding(3);
		this.TabWages.Size = new System.Drawing.Size(942, 472);
		this.TabWages.TabIndex = 1;
		this.TabWages.Text = "Contractor Wages";
		this.TabWages.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdContractorWages).BackColor = System.Drawing.Color.White;
		grdContractorWages_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdContractorWages.DesignTimeLayout = grdContractorWages_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdContractorWages.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Location = new System.Drawing.Point(3, 30);
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Name = "grdContractorWages";
		((ScrollableBase)this.grdContractorWages).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Size = new System.Drawing.Size(936, 439);
		((System.Windows.Forms.Control)(object)this.grdContractorWages).TabIndex = 0;
		this.grdContractorWages.TotalRow = (InheritableBoolean)1;
		this.grdContractorWages.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdContractorWages).VisualStyle = (VisualStyle)4;
		this.grdContractorWages.CellUpdated += new ColumnActionEventHandler(grdContractorWages_CellUpdated);
		this.grdContractorWages.ColumnButtonClick += new ColumnActionEventHandler(grdContractorWages_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdContractorWages).KeyDown += new System.Windows.Forms.KeyEventHandler(grdContractorWages_KeyDown);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label33);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(936, 27);
		this.panel5.TabIndex = 13;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.ForeColor = System.Drawing.Color.White;
		this.label33.Location = new System.Drawing.Point(12, 5);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(127, 16);
		this.label33.TabIndex = 126;
		this.label33.Text = "Contractor Wages";
		this.panel2.Controls.Add(this.BtnLoadInputFromBom);
		this.panel2.Controls.Add(this.groupBox2);
		this.panel2.Controls.Add(this.label11);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbProductionNo);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 58);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(950, 120);
		this.panel2.TabIndex = 0;
		this.BtnLoadInputFromBom.BackColor = System.Drawing.Color.Teal;
		this.BtnLoadInputFromBom.FlatAppearance.BorderSize = 0;
		this.BtnLoadInputFromBom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnLoadInputFromBom.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnLoadInputFromBom.ForeColor = System.Drawing.Color.White;
		this.BtnLoadInputFromBom.Location = new System.Drawing.Point(210, 45);
		this.BtnLoadInputFromBom.Name = "BtnLoadInputFromBom";
		this.BtnLoadInputFromBom.Size = new System.Drawing.Size(99, 28);
		this.BtnLoadInputFromBom.TabIndex = 1;
		this.BtnLoadInputFromBom.Text = "Load Wages";
		this.BtnLoadInputFromBom.UseVisualStyleBackColor = false;
		this.BtnLoadInputFromBom.Click += new System.EventHandler(BtnLoadInputFromBom_Click);
		this.groupBox2.Controls.Add(this.txtDocNo);
		this.groupBox2.Controls.Add(this.txtOutputTotalQty);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbProductionType);
		this.groupBox2.Controls.Add(this.txtProductionDate);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItem);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.groupBox2.Location = new System.Drawing.Point(315, 1);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(613, 72);
		this.groupBox2.TabIndex = 2;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Selected Production Information";
		this.txtDocNo.Location = new System.Drawing.Point(100, 17);
		this.txtDocNo.Name = "txtDocNo";
		this.txtDocNo.Size = new System.Drawing.Size(80, 22);
		this.txtDocNo.TabIndex = 0;
		this.txtOutputTotalQty.Location = new System.Drawing.Point(424, 44);
		this.txtOutputTotalQty.Name = "txtOutputTotalQty";
		this.txtOutputTotalQty.Size = new System.Drawing.Size(183, 22);
		this.txtOutputTotalQty.TabIndex = 4;
		this.CmbProductionType.AllowNull = (DefaultableBoolean)1;
		this.CmbProductionType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbProductionType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbProductionType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance40;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance46).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance47).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbProductionType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Location = new System.Drawing.Point(424, 15);
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Name = "CmbProductionType";
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Size = new System.Drawing.Size(183, 26);
		((System.Windows.Forms.Control)(object)this.CmbProductionType).TabIndex = 3;
		this.txtProductionDate.CustomFormat = "dd-mmm-yyyy";
		this.txtProductionDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtProductionDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtProductionDate.Location = new System.Drawing.Point(183, 17);
		this.txtProductionDate.Name = "txtProductionDate";
		this.txtProductionDate.Size = new System.Drawing.Size(135, 23);
		this.txtProductionDate.TabIndex = 1;
		this.CmbItem.AllowNull = (DefaultableBoolean)1;
		this.CmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance53;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance59).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItem).Location = new System.Drawing.Point(100, 42);
		((System.Windows.Forms.Control)(object)this.CmbItem).Name = "CmbItem";
		((System.Windows.Forms.Control)(object)this.CmbItem).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.CmbItem).TabIndex = 2;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(5, 21);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(88, 15);
		this.label2.TabIndex = 130;
		this.label2.Text = "Doc No && Date";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(323, 48);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(67, 15);
		this.label3.TabIndex = 130;
		this.label3.Text = "Output Qty";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(5, 48);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(94, 15);
		this.label4.TabIndex = 130;
		this.label4.Text = "Production Item";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(323, 21);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(94, 15);
		this.label1.TabIndex = 130;
		this.label1.Text = "Production Type";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(5, 16);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(85, 15);
		this.label11.TabIndex = 130;
		this.label11.Text = "Production No";
		this.CmbProductionNo.AllowNull = (DefaultableBoolean)1;
		this.CmbProductionNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbProductionNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbProductionNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance56;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance68).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbProductionNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbProductionNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbProductionNo).Location = new System.Drawing.Point(91, 10);
		((System.Windows.Forms.Control)(object)this.CmbProductionNo).Name = "CmbProductionNo";
		((System.Windows.Forms.Control)(object)this.CmbProductionNo).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.CmbProductionNo).TabIndex = 0;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Controls.Add(this.label21);
		this.panel3.Controls.Add(this.chkPrintVoucher);
		this.panel3.Controls.Add(this.ChkPreviewSlip);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(950, 55);
		this.panel3.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[10] { this.btnnew, this.btnRefresh, this.toolStripSeparator3, this.btnUpdate, this.toolStripSeparator, this.btnVoucherReport, this.btnPrint, this.toolStripSeparator5, this.btnshortcutkeys, this.toolStripSeparator2 });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(950, 27);
		this.toolStrip1.TabIndex = 2;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(60, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(81, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(79, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.btnVoucherReport.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnVoucherReport.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnVoucherReport.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnVoucherReport.Name = "btnVoucherReport";
		this.btnVoucherReport.Size = new System.Drawing.Size(116, 24);
		this.btnVoucherReport.Text = "118-Voucher";
		this.btnVoucherReport.Click += new System.EventHandler(btnVoucherReport_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(101, 24);
		this.btnPrint.Text = "1850-&Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(127, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.White;
		this.label21.Location = new System.Drawing.Point(7, 30);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(97, 19);
		this.label21.TabIndex = 126;
		this.label21.Text = "Production";
		this.chkPrintVoucher.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkPrintVoucher.AutoSize = true;
		this.chkPrintVoucher.Checked = true;
		this.chkPrintVoucher.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkPrintVoucher.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPrintVoucher.ForeColor = System.Drawing.Color.White;
		this.chkPrintVoucher.Location = new System.Drawing.Point(714, 32);
		this.chkPrintVoucher.Name = "chkPrintVoucher";
		this.chkPrintVoucher.Size = new System.Drawing.Size(125, 20);
		this.chkPrintVoucher.TabIndex = 15;
		this.chkPrintVoucher.Text = "Voucher Print";
		this.chkPrintVoucher.UseVisualStyleBackColor = true;
		this.ChkPreviewSlip.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPreviewSlip.AutoSize = true;
		this.ChkPreviewSlip.Checked = true;
		this.ChkPreviewSlip.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkPreviewSlip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPreviewSlip.ForeColor = System.Drawing.Color.White;
		this.ChkPreviewSlip.Location = new System.Drawing.Point(852, 32);
		this.ChkPreviewSlip.Name = "ChkPreviewSlip";
		this.ChkPreviewSlip.Size = new System.Drawing.Size(91, 20);
		this.ChkPreviewSlip.TabIndex = 16;
		this.ChkPreviewSlip.Text = "Slip Print";
		this.ChkPreviewSlip.UseVisualStyleBackColor = true;
		this.tabPage3.Controls.Add(this.panel45);
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.DatagridHistory);
		this.tabPage3.Controls.Add(this.panel15);
		this.tabPage3.Controls.Add(this.panel13);
		this.tabPage3.Controls.Add(this.panel38);
		this.tabPage3.Location = new System.Drawing.Point(4, 4);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(956, 682);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "History";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel45.Controls.Add(this.panel8);
		this.panel45.Controls.Add(this.panel19);
		this.panel45.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel45.Location = new System.Drawing.Point(0, 375);
		this.panel45.Name = "panel45";
		this.panel45.Size = new System.Drawing.Size(956, 307);
		this.panel45.TabIndex = 55;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryOverHead);
		this.panel8.Controls.Add(this.panel42);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryInput);
		this.panel8.Controls.Add(this.panel44);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(645, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(311, 307);
		this.panel8.TabIndex = 50;
		this.grdHistoryOverHead.AllowEdit = (InheritableBoolean)2;
		grdHistoryOverHead_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryOverHead.DesignTimeLayout = grdHistoryOverHead_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryOverHead).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryOverHead.DynamicFiltering = true;
		this.grdHistoryOverHead.FilterMode = (FilterMode)2;
		this.grdHistoryOverHead.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryOverHead).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryOverHead.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryOverHead).Location = new System.Drawing.Point(0, 215);
		((System.Windows.Forms.Control)(object)this.grdHistoryOverHead).Name = "grdHistoryOverHead";
		((ScrollableBase)this.grdHistoryOverHead).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryOverHead).Size = new System.Drawing.Size(311, 92);
		((System.Windows.Forms.Control)(object)this.grdHistoryOverHead).TabIndex = 1;
		this.grdHistoryOverHead.TotalRow = (InheritableBoolean)1;
		this.grdHistoryOverHead.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryOverHead).VisualStyle = (VisualStyle)4;
		this.panel42.BackColor = System.Drawing.Color.Teal;
		this.panel42.Controls.Add(this.label34);
		this.panel42.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel42.Location = new System.Drawing.Point(0, 196);
		this.panel42.Name = "panel42";
		this.panel42.Size = new System.Drawing.Size(311, 19);
		this.panel42.TabIndex = 58;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.ForeColor = System.Drawing.Color.White;
		this.label34.Location = new System.Drawing.Point(3, 0);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(79, 16);
		this.label34.TabIndex = 127;
		this.label34.Text = "OverHeads";
		this.grdHistoryInput.AllowEdit = (InheritableBoolean)2;
		grdHistoryInput_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryInput.DesignTimeLayout = grdHistoryInput_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdHistoryInput.DynamicFiltering = true;
		this.grdHistoryInput.FilterMode = (FilterMode)2;
		this.grdHistoryInput.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryInput.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).Location = new System.Drawing.Point(0, 19);
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).Name = "grdHistoryInput";
		((ScrollableBase)this.grdHistoryInput).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).Size = new System.Drawing.Size(311, 177);
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).TabIndex = 0;
		this.grdHistoryInput.TotalRow = (InheritableBoolean)1;
		this.grdHistoryInput.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryInput).VisualStyle = (VisualStyle)4;
		this.panel44.BackColor = System.Drawing.Color.Teal;
		this.panel44.Controls.Add(this.label14);
		this.panel44.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel44.Location = new System.Drawing.Point(0, 0);
		this.panel44.Name = "panel44";
		this.panel44.Size = new System.Drawing.Size(311, 19);
		this.panel44.TabIndex = 49;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(2, 1);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(223, 16);
		this.label14.TabIndex = 128;
		this.label14.Text = "Input Material (Consumed Items)";
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.grdWagesHistory);
		this.panel19.Controls.Add(this.panel18);
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryOutput);
		this.panel19.Controls.Add(this.panel12);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel19.Location = new System.Drawing.Point(0, 0);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(645, 307);
		this.panel19.TabIndex = 4;
		this.grdWagesHistory.AllowEdit = (InheritableBoolean)2;
		grdWagesHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdWagesHistory.DesignTimeLayout = grdWagesHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdWagesHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdWagesHistory.DynamicFiltering = true;
		this.grdWagesHistory.FilterMode = (FilterMode)2;
		this.grdWagesHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdWagesHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdWagesHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdWagesHistory).Location = new System.Drawing.Point(0, 215);
		((System.Windows.Forms.Control)(object)this.grdWagesHistory).Name = "grdWagesHistory";
		((ScrollableBase)this.grdWagesHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdWagesHistory).Size = new System.Drawing.Size(645, 92);
		((System.Windows.Forms.Control)(object)this.grdWagesHistory).TabIndex = 1;
		this.grdWagesHistory.TotalRow = (InheritableBoolean)1;
		this.grdWagesHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdWagesHistory).VisualStyle = (VisualStyle)4;
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.label40);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(0, 196);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(645, 19);
		this.panel18.TabIndex = 55;
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.ForeColor = System.Drawing.Color.White;
		this.label40.Location = new System.Drawing.Point(3, 0);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(127, 16);
		this.label40.TabIndex = 127;
		this.label40.Text = "Contractor Wages";
		this.grdHistoryOutput.AllowEdit = (InheritableBoolean)2;
		grdHistoryOutput_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryOutput.DesignTimeLayout = grdHistoryOutput_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdHistoryOutput.DynamicFiltering = true;
		this.grdHistoryOutput.FilterMode = (FilterMode)2;
		this.grdHistoryOutput.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryOutput.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Location = new System.Drawing.Point(0, 19);
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Name = "grdHistoryOutput";
		((ScrollableBase)this.grdHistoryOutput).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Size = new System.Drawing.Size(645, 177);
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).TabIndex = 0;
		this.grdHistoryOutput.TotalRow = (InheritableBoolean)1;
		this.grdHistoryOutput.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryOutput).VisualStyle = (VisualStyle)4;
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label15);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(645, 19);
		this.panel12.TabIndex = 52;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(6, 1);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(171, 16);
		this.label15.TabIndex = 127;
		this.label15.Text = "OutPut (Produced Items)";
		this.DatagridHistory.AllowEdit = (InheritableBoolean)2;
		DatagridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DatagridHistory.DesignTimeLayout = DatagridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Dock = System.Windows.Forms.DockStyle.Top;
		this.DatagridHistory.DynamicFiltering = true;
		this.DatagridHistory.FilterMode = (FilterMode)2;
		this.DatagridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DatagridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Location = new System.Drawing.Point(0, 159);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Name = "DatagridHistory";
		((ScrollableBase)this.DatagridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Size = new System.Drawing.Size(956, 216);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).TabIndex = 1;
		this.DatagridHistory.TotalRow = (InheritableBoolean)1;
		this.DatagridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DatagridHistory).VisualStyle = (VisualStyle)4;
		this.DatagridHistory.ColumnButtonClick += new ColumnActionEventHandler(DatagridHistory_ColumnButtonClick);
		this.DatagridHistory.SelectionChanged += new System.EventHandler(DatagridHistory_SelectionChanged);
		this.DatagridHistory.LinkClicked += new ColumnActionEventHandler(DatagridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).DoubleClick += new System.EventHandler(DatagridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DatagridHistory_KeyDown);
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.ctrlGrdBar1);
		this.panel15.Controls.Add(this.label17);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 136);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(956, 23);
		this.panel15.TabIndex = 53;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = null;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(909, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(9, 6, 9, 6);
		this.ctrlGrdBar1.MyGrid = null;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(42, 22);
		this.ctrlGrdBar1.TabIndex = 128;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(8, 3);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(55, 16);
		this.label17.TabIndex = 127;
		this.label17.Text = "History";
		this.panel13.BackColor = System.Drawing.Color.White;
		this.panel13.Controls.Add(this.groupBox1);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 51);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(956, 85);
		this.panel13.TabIndex = 0;
		this.groupBox1.Controls.Add(this.RadNotReffered);
		this.groupBox1.Controls.Add(this.RadReffered);
		this.groupBox1.Controls.Add(this.RadAll);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemhistory);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbProductionNoHistory);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBomNoHistory);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory);
		this.groupBox1.Controls.Add(this.txtToDateHistory);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.txtToDocNoHistory);
		this.groupBox1.Controls.Add(this.txtFromdateHistory);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Controls.Add(this.txtFromNoHistory);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(5, 1);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1084, 76);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.RadNotReffered.AutoSize = true;
		this.RadNotReffered.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.RadNotReffered.Location = new System.Drawing.Point(974, 49);
		this.RadNotReffered.Name = "RadNotReffered";
		this.RadNotReffered.Size = new System.Drawing.Size(108, 23);
		this.RadNotReffered.TabIndex = 11;
		this.RadNotReffered.Text = "Not Reffered";
		this.RadNotReffered.UseVisualStyleBackColor = true;
		this.RadReffered.AutoSize = true;
		this.RadReffered.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.RadReffered.Location = new System.Drawing.Point(974, 31);
		this.RadReffered.Name = "RadReffered";
		this.RadReffered.Size = new System.Drawing.Size(80, 23);
		this.RadReffered.TabIndex = 10;
		this.RadReffered.Text = "Reffered";
		this.RadReffered.UseVisualStyleBackColor = true;
		this.RadAll.AutoSize = true;
		this.RadAll.Checked = true;
		this.RadAll.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.RadAll.Location = new System.Drawing.Point(974, 13);
		this.RadAll.Name = "RadAll";
		this.RadAll.Size = new System.Drawing.Size(44, 23);
		this.RadAll.TabIndex = 9;
		this.RadAll.TabStop = true;
		this.RadAll.Text = "All";
		this.RadAll.UseVisualStyleBackColor = true;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(906, 46);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(62, 25);
		this.btnshow.TabIndex = 8;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.cmbItemhistory.AllowNull = (DefaultableBoolean)1;
		this.cmbItemhistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemhistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemhistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemhistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance71;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemhistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance77).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance78).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance79).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemhistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemhistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemhistory).Location = new System.Drawing.Point(700, 45);
		((System.Windows.Forms.Control)(object)this.cmbItemhistory).Name = "cmbItemhistory";
		((System.Windows.Forms.Control)(object)this.cmbItemhistory).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemhistory).TabIndex = 7;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(612, 51);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(67, 15);
		this.label29.TabIndex = 180;
		this.label29.Text = "Item Name";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(612, 22);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(85, 15);
		this.label30.TabIndex = 178;
		this.label30.Text = "Production No";
		this.cmbProductionNoHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbProductionNoHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbProductionNoHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbProductionNoHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance84;
		((SpecialBoxBase)((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbProductionNoHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbProductionNoHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbProductionNoHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbProductionNoHistory).Location = new System.Drawing.Point(700, 16);
		((System.Windows.Forms.Control)(object)this.cmbProductionNoHistory).Name = "cmbProductionNoHistory";
		((System.Windows.Forms.Control)(object)this.cmbProductionNoHistory).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.cmbProductionNoHistory).TabIndex = 6;
		this.CmbBomNoHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbBomNoHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBomNoHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBomNoHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBomNoHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBomNoHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBomNoHistory).Location = new System.Drawing.Point(411, 45);
		((System.Windows.Forms.Control)(object)this.CmbBomNoHistory).Name = "CmbBomNoHistory";
		((System.Windows.Forms.Control)(object)this.CmbBomNoHistory).Size = new System.Drawing.Size(195, 26);
		((System.Windows.Forms.Control)(object)this.CmbBomNoHistory).TabIndex = 5;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(322, 51);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(56, 15);
		this.label20.TabIndex = 176;
		this.label20.Text = "BOM No.";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(5, 22);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(60, 15);
		this.label19.TabIndex = 174;
		this.label19.Text = "Date Type";
		this.cmbDateTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbDateTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Location = new System.Drawing.Point(97, 16);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Name = "cmbDateTypeHistory";
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Size = new System.Drawing.Size(224, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).TabIndex = 0;
		this.cmbDateTypeHistory.ValueChanged += new System.EventHandler(cmbDateTypeHistory_ValueChanged);
		this.txtToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.CustomFormat = "dd-MMM-yy";
		this.txtToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateHistory.Location = new System.Drawing.Point(210, 47);
		this.txtToDateHistory.Name = "txtToDateHistory";
		this.txtToDateHistory.Size = new System.Drawing.Size(111, 23);
		this.txtToDateHistory.TabIndex = 2;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(5, 51);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(91, 15);
		this.label28.TabIndex = 171;
		this.label28.Text = "Date From && To";
		this.txtToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(509, 18);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.Size = new System.Drawing.Size(97, 23);
		this.txtToDocNoHistory.TabIndex = 4;
		this.txtFromdateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.CustomFormat = "dd-MMM-yy";
		this.txtFromdateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromdateHistory.Location = new System.Drawing.Point(97, 47);
		this.txtFromdateHistory.Name = "txtFromdateHistory";
		this.txtFromdateHistory.Size = new System.Drawing.Size(111, 23);
		this.txtFromdateHistory.TabIndex = 1;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(322, 22);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(88, 15);
		this.label35.TabIndex = 173;
		this.label35.Text = "Doc From && To";
		this.txtFromNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromNoHistory.Location = new System.Drawing.Point(411, 18);
		this.txtFromNoHistory.Name = "txtFromNoHistory";
		this.txtFromNoHistory.Size = new System.Drawing.Size(97, 23);
		this.txtFromNoHistory.TabIndex = 3;
		this.panel38.BackColor = System.Drawing.Color.Teal;
		this.panel38.Controls.Add(this.toolStrip2);
		this.panel38.Controls.Add(this.label32);
		this.panel38.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel38.Location = new System.Drawing.Point(0, 0);
		this.panel38.Name = "panel38";
		this.panel38.Size = new System.Drawing.Size(956, 51);
		this.panel38.TabIndex = 48;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.toolStripSeparator1, this.btnRegister });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(956, 27);
		this.toolStrip2.TabIndex = 3;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(60, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnHistoryRefresh_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnRegister.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRegister.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRegister.Name = "btnRegister";
		this.btnRegister.Size = new System.Drawing.Size(62, 24);
		this.btnRegister.Text = "&Print";
		this.btnRegister.Visible = false;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.White;
		this.label32.Location = new System.Drawing.Point(3, 29);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(170, 19);
		this.label32.TabIndex = 127;
		this.label32.Text = "Production History  ";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(964, 711);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmConcreteProductionUpdationAfterRelease";
		this.Text = "Concrete Production";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmConcreteProductionUpdationAfterRelease_FormClosed);
		base.Load += new System.EventHandler(frmBillOfMaterial_Manufacturing_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmBillOfMaterial_Manufacturing_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel17.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.TabWages.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdContractorWages).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionNo).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage3.ResumeLayout(false);
		this.panel45.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryOverHead).EndInit();
		this.panel42.ResumeLayout(false);
		this.panel42.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryInput).EndInit();
		this.panel44.ResumeLayout(false);
		this.panel44.PerformLayout();
		this.panel19.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdWagesHistory).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryOutput).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel13.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemhistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbProductionNoHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBomNoHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).EndInit();
		this.panel38.ResumeLayout(false);
		this.panel38.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
