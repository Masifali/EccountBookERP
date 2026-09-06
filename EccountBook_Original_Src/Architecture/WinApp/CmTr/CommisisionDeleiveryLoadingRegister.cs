using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.CmTr.TransactionForm;
using Architecture.Common;
using Architecture.Model;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.CmTr;

public class CommisisionDeleiveryLoadingRegister : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable CmTrData = new DataTable();

	private IContainer components;

	private Panel panel1;

	private ToolStrip toolStrip3;

	private ToolStripButton btnRefresh;

	private ToolStripButton btn555Register;

	private ToolStripButton btn_NewClick;

	private Panel panel2;

	private Label label1;

	private Panel panel3;

	private Label label6;

	internal UltraCombo CmbCustomerName;

	internal UltraCombo CmbItem;

	internal UltraCombo CmbSupplier;

	private Label label9;

	private Label label2;

	private DateTimePicker ToDate;

	private TextBox txtToDocNo;

	private Label label35;

	private TextBox txtFromNo;

	private DateTimePicker Fromdate;

	private Label label27;

	private Label label3;

	internal UltraCombo CmbItemType;

	internal UltraCombo CmbItemCategory;

	private Label label4;

	internal UltraCombo CmbInventoryParentCategories;

	private Label label5;

	private Button btnshow;

	private Panel panel4;

	private Panel panel5;

	private Label label10;

	internal GridEX DataGridHistory;

	private CtrlGrdBar CmTrCtrlGrdBar;

	private GroupBox groupBox1;

	private Label label7;

	internal UltraCombo cmbDateType;

	private ToolStripButton btnShortCutKeys;

	private Label cmbBroker;

	internal UltraCombo cmbbrooker;

	private GroupBox groupBox3;

	private Label label18;

	private Label label19;

	private Label label20;

	private Label lbl;

	private Label label22;

	private Label label23;

	private Label label24;

	private Label label25;

	private Label label26;

	private Label lblBalCusWeight;

	private Label lblBalSupWeight;

	private Label lblBalQty;

	private Label lblCusTotalOutWeight;

	private Label lblSupTotalRecWeight;

	private Label lblTotalRecOutQty;

	private Label lblCusTotalNetWeight;

	private Label lblSupTotalNetWeight;

	private Label lbltotalqty;

	private Label label8;

	private Label label11;

	public CommisisionDeleiveryLoadingRegister(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void CommisisionDeleiveryLoadingRegister_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(922, 570);
			Datetypefill();
			AllCombobind();
			cmbDateType.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllCombobind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtcombo = CommTradeLoadingDelivery.GetDataForDropDownFromCommTradeLoadingDelivery(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtcombo.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtsupplier = new DataTable();
			dtsupplier.Columns.Add("Id");
			dtsupplier.Columns.Add("SupplierName");
			DataTable dtBroker = new DataTable();
			dtBroker.Columns.Add("Id");
			dtBroker.Columns.Add("BrokerName");
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id");
			dtCustomer.Columns.Add("CustomerName");
			DataTable dtitem = new DataTable();
			dtitem.Columns.Add("Id");
			dtitem.Columns.Add("ItemName");
			DataTable dtItemType = new DataTable();
			dtItemType.Columns.Add("Id");
			dtItemType.Columns.Add("ItemType");
			DataTable dtInvParentCategory = new DataTable();
			dtInvParentCategory.Columns.Add("Id");
			dtInvParentCategory.Columns.Add("ParentCategory");
			DataTable dtItemCategory = new DataTable();
			dtItemCategory.Columns.Add("Id");
			dtItemCategory.Columns.Add("ItemCategory");
			for (int i = 0; i < dtcombo.Rows.Count; i++)
			{
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "Supplier")
				{
					dtsupplier.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "Customer")
				{
					dtCustomer.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "Broker")
				{
					dtBroker.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "Item")
				{
					dtitem.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "ItemType")
				{
					dtItemType.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "ItemCategory")
				{
					dtItemCategory.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "ParentCategory")
				{
					dtInvParentCategory.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
			}
			if (dtsupplier.Rows.Count > 0)
			{
				DDL.BindDDL(dtsupplier, CmbSupplier, "Id", "SupplierName", "Supplier Name", ZeroIndex: true);
				((UltraGridBase)CmbSupplier).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtCustomer.Rows.Count > 0)
			{
				DDL.BindDDL(dtCustomer, CmbCustomerName, "Id", "CustomerName", "Customer Name", ZeroIndex: true);
				((UltraGridBase)CmbCustomerName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtBroker.Rows.Count > 0)
			{
				DDL.BindDDL(dtBroker, cmbbrooker, "Id", "BrokerName", "Broker Name", ZeroIndex: true);
				((UltraGridBase)cmbbrooker).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtitem.Rows.Count > 0)
			{
				DDL.BindDDL(dtitem, CmbItem, "Id", "ItemName", "Item Name", ZeroIndex: true);
				((UltraGridBase)CmbItem).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtItemType.Rows.Count > 0)
			{
				DDL.BindDDL(dtItemType, CmbItemType, "Id", "ItemType", "Item Type", ZeroIndex: true);
				((UltraGridBase)CmbItemType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtItemCategory.Rows.Count > 0)
			{
				DDL.BindDDL(dtItemCategory, CmbItemCategory, "Id", "ItemCategory", "Item Category", ZeroIndex: true);
				((UltraGridBase)CmbItemCategory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtInvParentCategory.Rows.Count > 0)
			{
				DDL.BindDDL(dtInvParentCategory, CmbInventoryParentCategories, "Id", "ParentCategory", "Parent Category", ZeroIndex: true);
				((UltraGridBase)CmbInventoryParentCategories).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
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
			DDL.BindDDL(dt, cmbDateType, "Id", "Parameters", "Parameters", ZeroIndex: true);
			((UltraGridBase)cmbDateType).Rows[2].Activate();
			((UltraGridBase)cmbDateType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
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
			txtFromNo.Text = string.Empty;
			txtToDocNo.Text = string.Empty;
			((Control)(object)CmbSupplier).Text = string.Empty;
			((Control)(object)CmbCustomerName).Text = string.Empty;
			((Control)(object)CmbItem).Text = string.Empty;
			((Control)(object)CmbItemCategory).Text = string.Empty;
			((Control)(object)CmbItemType).Text = string.Empty;
			((Control)(object)CmbInventoryParentCategories).Text = string.Empty;
			cmbDateType.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btn_NewClick_Click(object sender, EventArgs e)
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
			AllCombobind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommisisionDeleiveryLoadingRegister_FormClosed(object sender, FormClosedEventArgs e)
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

	private void btnshow_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name == "DataGridHistory")
			{
				cmbDateType.Focus();
			}
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
			gridHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters CmTr = new ReportsParameters();
			CmTr.OrganizationId = UserAccount.OrganizationId;
			CmTr.CompanyId = UserAccount.CompanyId;
			CmTr.BranchesId = UserAccount.BranchesId;
			CmTr.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			CmTr.FromDocNo = Conversion.ToInt((object)txtFromNo.Text.Trim());
			CmTr.ToDocNo = Conversion.ToInt((object)txtToDocNo.Text.Trim());
			CmTr.FromDate = Conversion.ToDateTime((object)Fromdate.Value);
			CmTr.ToDate = Conversion.ToDateTime((object)ToDate.Value);
			CmTr.SupplierId = Conversion.ToInt(CmbSupplier.Value);
			CmTr.CustomerId = Conversion.ToInt(CmbCustomerName.Value);
			CmTr.CommissionAgentId = Conversion.ToInt(cmbbrooker.Value);
			CmTr.ItemId = Conversion.ToInt(CmbItem.Value);
			CmTr.ItemCategoryId = Conversion.ToInt(CmbItemCategory.Value);
			CmTr.ItemTypeId = Conversion.ToInt(CmbItemType.Value);
			CmTr.InventoryParentCategories = Conversion.ToInt(CmbInventoryParentCategories.Value);
			CmTrData = CommTradeLoadingDelivery.CommissionTradeLoadingDeliverySlipAndRegister(CmTr);
			if (CmTrData.Rows.Count > 0)
			{
				DataTable dtgrid = new DataTable();
				dtgrid.Columns.Add("Id", typeof(int));
				dtgrid.Columns.Add("DocNo", typeof(string));
				dtgrid.Columns.Add("DocDate", typeof(DateTime));
				dtgrid.Columns.Add("VehicleNo", typeof(string));
				dtgrid.Columns.Add("BiltyNo", typeof(string));
				dtgrid.Columns.Add("BiltyExpense", typeof(double));
				dtgrid.Columns.Add("TransporterName", typeof(string));
				dtgrid.Columns.Add("VehicleGrossWeight", typeof(double));
				dtgrid.Columns.Add("BrokerName", typeof(string));
				dtgrid.Columns.Add("Item", typeof(string));
				dtgrid.Columns.Add("CropYear", typeof(string));
				dtgrid.Columns.Add("PackingType", typeof(string));
				dtgrid.Columns.Add("UOM", typeof(string));
				dtgrid.Columns.Add("Qty", typeof(double));
				dtgrid.Columns.Add("SupplierName", typeof(string));
				dtgrid.Columns.Add("SupGrossWeight", typeof(double));
				dtgrid.Columns.Add("SupNetWeight", typeof(double));
				dtgrid.Columns.Add("CustomerName", typeof(string));
				dtgrid.Columns.Add("CusGrossWeight", typeof(double));
				dtgrid.Columns.Add("CusNetWeight", typeof(double));
				dtgrid.Columns.Add("EntryUser", typeof(string));
				dtgrid.Columns.Add("ModifyUser", typeof(string));
				dtgrid.Columns.Add("IsApproved", typeof(bool));
				dtgrid.Columns.Add("NoOfAttachments", typeof(int));
				dtgrid.Clear();
				for (int i = 0; i < CmTrData.Rows.Count; i++)
				{
					dtgrid.Rows.Add(Conversion.ToInt(CmTrData.Rows[i]["Id"]), Conversion.ToInt(CmTrData.Rows[i]["DocNo"]), Conversion.ToDateTime(CmTrData.Rows[i]["DocDate"]).ToShortDateString(), Conversion.ToString(CmTrData.Rows[i]["VehicleNo"]), Conversion.ToString(CmTrData.Rows[i]["BiltyNo"]), Conversion.ToDouble(CmTrData.Rows[i]["BiltyExpense"]), Conversion.ToString(CmTrData.Rows[i]["TransporterName"]), Conversion.ToDouble(CmTrData.Rows[i]["VehicleGrossWeight"]), Conversion.ToString(CmTrData.Rows[i]["BrokerName"]), Conversion.ToString(CmTrData.Rows[i]["ItemName"]), Conversion.ToString(CmTrData.Rows[i]["CropYear"]), Conversion.ToString(CmTrData.Rows[i]["PackingType"]), Conversion.ToString(CmTrData.Rows[i]["PackUom"]), Conversion.ToDouble(CmTrData.Rows[i]["Qty"]), Conversion.ToString(CmTrData.Rows[i]["SupplierName"]), Conversion.ToDouble(CmTrData.Rows[i]["SupGrossWeight"]), Conversion.ToDouble(CmTrData.Rows[i]["SupNetWeight"]), Conversion.ToString(CmTrData.Rows[i]["CustomerName"]), Conversion.ToDouble(CmTrData.Rows[i]["CusGrossWeight"]), Conversion.ToDouble(CmTrData.Rows[i]["CusNetWeight"]), Conversion.ToString(CmTrData.Rows[i]["EntryUserName"]), Conversion.ToString(CmTrData.Rows[i]["ModifyUserName"]), Conversion.ToBool(CmTrData.Rows[i]["IsApproved"]), Conversion.ToInt(CmTrData.Rows[i]["NoOfAttachments"]));
				}
				DataGridHistory.DataSource = dtgrid;
				DataGridHistory.RetrieveStructure();
				GridSetting();
			}
			else
			{
				DataGridHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSetting()
	{
		try
		{
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["BiltyExpense"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["BiltyExpense"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["BiltyExpense"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["BiltyExpense"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["VehicleGrossWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["VehicleGrossWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["VehicleGrossWeight"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["VehicleGrossWeight"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["SupGrossWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["SupGrossWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["SupGrossWeight"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["SupGrossWeight"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["SupNetWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["SupNetWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["SupNetWeight"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["SupNetWeight"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["CusGrossWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["CusGrossWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["CusGrossWeight"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["CusGrossWeight"].TotalFormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["CusNetWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["CusNetWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["CusNetWeight"].FormatString = "#,##0.###";
			DataGridHistory.RootTable.Columns["CusNetWeight"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(DataGridHistory);
			DataGridHistory.RootTable.Columns.Add("Print");
			DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridHistory.RootTable.Columns["Print"].Width = 50;
			DataGridHistory.RootTable.Columns["Print"].Position = 0;
			DataGridHistory.FrozenColumns = 1;
			CmTrCtrlGrdBar_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			decimal TotalQty = default(decimal);
			decimal TotalSupWt = default(decimal);
			decimal TotalCusWt = default(decimal);
			decimal RecOutQty = default(decimal);
			decimal RecSupWt = default(decimal);
			decimal OutCusWt = default(decimal);
			decimal BalQty = default(decimal);
			decimal BalSupWt = default(decimal);
			decimal BalCusWt = default(decimal);
			int Id = Conversion.ToInt(r.Cells["Id"].Value);
			if (Id != 0)
			{
				DataRow matchingRow = CmTrData.AsEnumerable().FirstOrDefault((DataRow row) => Conversion.ToInt(row["Id"]) == Id);
				if (matchingRow != null)
				{
					TotalQty = Conversion.ToDecimal(matchingRow["TotalQty"]);
					TotalSupWt = Conversion.ToDecimal(matchingRow["SupTotalNetWeight"]);
					TotalCusWt = Conversion.ToDecimal(matchingRow["CusTotalNetWeight"]);
					RecOutQty = Conversion.ToDecimal(matchingRow["TotalRecOutQty"]);
					RecSupWt = Conversion.ToDecimal(matchingRow["SupTotalRecWeight"]);
					OutCusWt = Conversion.ToDecimal(matchingRow["CusTotalOutWeight"]);
					BalQty = Conversion.ToDecimal(matchingRow["BalQty"]);
					BalSupWt = Conversion.ToDecimal(matchingRow["BalSupWeight"]);
					BalCusWt = Conversion.ToDecimal(matchingRow["BalCusWeight"]);
				}
				lbltotalqty.Text = TotalQty.ToString("##,#.##");
				lblSupTotalNetWeight.Text = TotalSupWt.ToString("##,#.##");
				lblCusTotalNetWeight.Text = TotalCusWt.ToString("##,#.##");
				lblTotalRecOutQty.Text = RecOutQty.ToString("##,#.##");
				lblSupTotalRecWeight.Text = RecSupWt.ToString("##,#.##");
				lblCusTotalOutWeight.Text = OutCusWt.ToString("##,#.##");
				lblBalQty.Text = BalQty.ToString("##,#.##");
				lblBalSupWeight.Text = BalSupWt.ToString("##,#.##");
				lblBalCusWeight.Text = BalCusWt.ToString("##,#.##");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Print")
			{
				GenerateSlip556Print(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
			}
			if (e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), 1702);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), 1702);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmTrCtrlGrdBar_Load(object sender, EventArgs e)
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
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFromNo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtToDocNo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void btn555Register_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (CmTrData == null || CmTrData.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(CmTrData, "1702-CommissionTradeLoadingDelivery_Register.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateSlip556Print(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = CommTradeLoadingDelivery.CommissionTradeLoadingDeliverySlipAndRegister(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = PrintId,
				DocumentTypeId = 1702
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "556-CommissionTradeLoadingDelivery_Slip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommisisionDeleiveryLoadingRegister_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshow_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btn_NewClick_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)DataGridHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				cmbDateType.Focus();
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				btn555Register_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				cmbDateType.Focus();
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
			dt.Rows.Add("Ctrl+S", "For Show");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Alt+2", "For print 554-ProductionRegisterWithActivity(OutPut By Packing Material)");
			dt.Rows.Add("Alt+1", "For Print 553-ProductionRegisterWithActivity");
			dt.Rows.Add("Ctrl+F5", "For Focus On DateType ");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Supplier Customer in Filter");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && DataGridHistory.CurrentColumn != null)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (DataGridHistory.CurrentColumn.Key == "Print")
				{
					GenerateSlip556Print(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
				}
				if (DataGridHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), 1702);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortCutKeys_Click(object sender, EventArgs e)
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

	private void cmbDateType_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbDateType.Value) == 1)
			{
				Fromdate.Value = DateTime.Today;
				btnshow_Click(null, null);
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 2)
			{
				Fromdate.Value = DateTime.Today.AddDays(-7.0);
				btnshow_Click(null, null);
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 3)
			{
				DateTime now = DateTime.Now;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				Fromdate.Value = first;
				ToDate.Value = DateTime.Today;
				btnshow_Click(null, null);
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				Fromdate.Value = first2;
				ToDate.Value = DateTime.Today;
				btnshow_Click(null, null);
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 5)
			{
				Fromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
				btnshow_Click(null, null);
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
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Expected O, but got Unknown
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Expected O, but got Unknown
		//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0409: Expected O, but got Unknown
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Expected O, but got Unknown
		//IL_044c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0456: Expected O, but got Unknown
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Expected O, but got Unknown
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0477: Expected O, but got Unknown
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Expected O, but got Unknown
		//IL_0483: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Expected O, but got Unknown
		//IL_084f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0859: Expected O, but got Unknown
		//IL_087d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0887: Expected O, but got Unknown
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
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
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.CmTr.CommisisionDeleiveryLoadingRegister));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.CmTrCtrlGrdBar = new grid.User_Conrtols.CtrlGrdBar();
		this.label10 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.lblBalCusWeight = new System.Windows.Forms.Label();
		this.lblBalSupWeight = new System.Windows.Forms.Label();
		this.lblBalQty = new System.Windows.Forms.Label();
		this.lblCusTotalOutWeight = new System.Windows.Forms.Label();
		this.lblSupTotalRecWeight = new System.Windows.Forms.Label();
		this.lblTotalRecOutQty = new System.Windows.Forms.Label();
		this.lblCusTotalNetWeight = new System.Windows.Forms.Label();
		this.lblSupTotalNetWeight = new System.Windows.Forms.Label();
		this.lbltotalqty = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.lbl = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.cmbBroker = new System.Windows.Forms.Label();
		this.cmbbrooker = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.cmbDateType = new UltraCombo();
		this.ToDate = new System.Windows.Forms.DateTimePicker();
		this.btnshow = new System.Windows.Forms.Button();
		this.label2 = new System.Windows.Forms.Label();
		this.CmbInventoryParentCategories = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.CmbSupplier = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.CmbItem = new UltraCombo();
		this.CmbItemType = new UltraCombo();
		this.CmbCustomerName = new UltraCombo();
		this.CmbItemCategory = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.txtToDocNo = new System.Windows.Forms.TextBox();
		this.Fromdate = new System.Windows.Forms.DateTimePicker();
		this.label11 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.txtFromNo = new System.Windows.Forms.TextBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btn_NewClick = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btn555Register = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel5.SuspendLayout();
		this.panel3.SuspendLayout();
		this.groupBox3.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbbrooker).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInventoryParentCategories).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCategory).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Controls.Add(this.toolStrip3);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(984, 661);
		this.panel1.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 268);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(984, 393);
		this.panel4.TabIndex = 6;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 32);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(984, 361);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.CmTrCtrlGrdBar);
		this.panel5.Controls.Add(this.label10);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(984, 32);
		this.panel5.TabIndex = 5;
		this.CmTrCtrlGrdBar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.CmTrCtrlGrdBar.FormName = this;
		this.CmTrCtrlGrdBar.Location = new System.Drawing.Point(938, 4);
		this.CmTrCtrlGrdBar.MyGrid = this.DataGridHistory;
		this.CmTrCtrlGrdBar.Name = "CmTrCtrlGrdBar";
		this.CmTrCtrlGrdBar.Size = new System.Drawing.Size(40, 25);
		this.CmTrCtrlGrdBar.TabIndex = 1;
		this.CmTrCtrlGrdBar.Load += new System.EventHandler(CmTrCtrlGrdBar_Load);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.White;
		this.label10.Location = new System.Drawing.Point(7, 7);
		this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(121, 19);
		this.label10.TabIndex = 0;
		this.label10.Text = "Detail History";
		this.panel3.Controls.Add(this.groupBox3);
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 59);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(984, 209);
		this.panel3.TabIndex = 0;
		this.groupBox3.Controls.Add(this.lblBalCusWeight);
		this.groupBox3.Controls.Add(this.lblBalSupWeight);
		this.groupBox3.Controls.Add(this.lblBalQty);
		this.groupBox3.Controls.Add(this.lblCusTotalOutWeight);
		this.groupBox3.Controls.Add(this.lblSupTotalRecWeight);
		this.groupBox3.Controls.Add(this.lblTotalRecOutQty);
		this.groupBox3.Controls.Add(this.lblCusTotalNetWeight);
		this.groupBox3.Controls.Add(this.lblSupTotalNetWeight);
		this.groupBox3.Controls.Add(this.lbltotalqty);
		this.groupBox3.Controls.Add(this.label26);
		this.groupBox3.Controls.Add(this.label18);
		this.groupBox3.Controls.Add(this.label19);
		this.groupBox3.Controls.Add(this.label20);
		this.groupBox3.Controls.Add(this.lbl);
		this.groupBox3.Controls.Add(this.label22);
		this.groupBox3.Controls.Add(this.label23);
		this.groupBox3.Controls.Add(this.label24);
		this.groupBox3.Controls.Add(this.label25);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(578, 6);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(386, 189);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Total Of Selected Rows";
		this.lblBalCusWeight.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.lblBalCusWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblBalCusWeight.Enabled = false;
		this.lblBalCusWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBalCusWeight.Location = new System.Drawing.Point(253, 147);
		this.lblBalCusWeight.Name = "lblBalCusWeight";
		this.lblBalCusWeight.Size = new System.Drawing.Size(129, 23);
		this.lblBalCusWeight.TabIndex = 8;
		this.lblBalCusWeight.Text = "0";
		this.lblBalCusWeight.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblBalSupWeight.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.lblBalSupWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblBalSupWeight.Enabled = false;
		this.lblBalSupWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBalSupWeight.Location = new System.Drawing.Point(253, 97);
		this.lblBalSupWeight.Name = "lblBalSupWeight";
		this.lblBalSupWeight.Size = new System.Drawing.Size(129, 23);
		this.lblBalSupWeight.TabIndex = 7;
		this.lblBalSupWeight.Text = "0";
		this.lblBalSupWeight.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblBalQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblBalQty.Enabled = false;
		this.lblBalQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBalQty.Location = new System.Drawing.Point(253, 44);
		this.lblBalQty.Name = "lblBalQty";
		this.lblBalQty.Size = new System.Drawing.Size(129, 23);
		this.lblBalQty.TabIndex = 6;
		this.lblBalQty.Text = "0";
		this.lblBalQty.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblCusTotalOutWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblCusTotalOutWeight.Enabled = false;
		this.lblCusTotalOutWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCusTotalOutWeight.Location = new System.Drawing.Point(100, 157);
		this.lblCusTotalOutWeight.Name = "lblCusTotalOutWeight";
		this.lblCusTotalOutWeight.Size = new System.Drawing.Size(116, 23);
		this.lblCusTotalOutWeight.TabIndex = 5;
		this.lblCusTotalOutWeight.Text = "0";
		this.lblCusTotalOutWeight.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblSupTotalRecWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblSupTotalRecWeight.Enabled = false;
		this.lblSupTotalRecWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSupTotalRecWeight.Location = new System.Drawing.Point(100, 130);
		this.lblSupTotalRecWeight.Name = "lblSupTotalRecWeight";
		this.lblSupTotalRecWeight.Size = new System.Drawing.Size(116, 23);
		this.lblSupTotalRecWeight.TabIndex = 4;
		this.lblSupTotalRecWeight.Text = "0";
		this.lblSupTotalRecWeight.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblTotalRecOutQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblTotalRecOutQty.Enabled = false;
		this.lblTotalRecOutQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalRecOutQty.Location = new System.Drawing.Point(100, 104);
		this.lblTotalRecOutQty.Name = "lblTotalRecOutQty";
		this.lblTotalRecOutQty.Size = new System.Drawing.Size(116, 23);
		this.lblTotalRecOutQty.TabIndex = 3;
		this.lblTotalRecOutQty.Text = "0";
		this.lblTotalRecOutQty.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblCusTotalNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblCusTotalNetWeight.Enabled = false;
		this.lblCusTotalNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCusTotalNetWeight.Location = new System.Drawing.Point(100, 70);
		this.lblCusTotalNetWeight.Name = "lblCusTotalNetWeight";
		this.lblCusTotalNetWeight.Size = new System.Drawing.Size(116, 23);
		this.lblCusTotalNetWeight.TabIndex = 2;
		this.lblCusTotalNetWeight.Text = "0";
		this.lblCusTotalNetWeight.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lblSupTotalNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblSupTotalNetWeight.Enabled = false;
		this.lblSupTotalNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblSupTotalNetWeight.Location = new System.Drawing.Point(100, 44);
		this.lblSupTotalNetWeight.Name = "lblSupTotalNetWeight";
		this.lblSupTotalNetWeight.Size = new System.Drawing.Size(116, 23);
		this.lblSupTotalNetWeight.TabIndex = 1;
		this.lblSupTotalNetWeight.Text = "0";
		this.lblSupTotalNetWeight.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.lbltotalqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lbltotalqty.Enabled = false;
		this.lbltotalqty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbltotalqty.Location = new System.Drawing.Point(100, 18);
		this.lbltotalqty.Name = "lbltotalqty";
		this.lbltotalqty.Size = new System.Drawing.Size(116, 23);
		this.lbltotalqty.TabIndex = 0;
		this.lbltotalqty.Text = "0";
		this.lbltotalqty.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(253, 126);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(81, 15);
		this.label26.TabIndex = 0;
		this.label26.Text = "BalCusWeight";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(253, 74);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(83, 15);
		this.label18.TabIndex = 0;
		this.label18.Text = "BalSupWeight";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(253, 22);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(42, 15);
		this.label19.TabIndex = 0;
		this.label19.Text = "BalQty";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(7, 161);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(91, 15);
		this.label20.TabIndex = 0;
		this.label20.Text = "Cus Out Weight";
		this.lbl.AutoSize = true;
		this.lbl.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lbl.Location = new System.Drawing.Point(7, 134);
		this.lbl.Name = "lbl";
		this.lbl.Size = new System.Drawing.Size(92, 15);
		this.lbl.TabIndex = 0;
		this.lbl.Text = "Sup Rec Weight";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(7, 108);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(73, 15);
		this.label22.TabIndex = 0;
		this.label22.Text = "Rec/Out Qty";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(7, 74);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(90, 15);
		this.label23.TabIndex = 0;
		this.label23.Text = "Cus Net Weight";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(7, 48);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(92, 15);
		this.label24.TabIndex = 0;
		this.label24.Text = "Sup Net Weight";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(7, 22);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(55, 15);
		this.label25.TabIndex = 0;
		this.label25.Text = "Total Qty";
		this.groupBox1.Controls.Add(this.cmbBroker);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbbrooker);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDateType);
		this.groupBox1.Controls.Add(this.ToDate);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInventoryParentCategories);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItem);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemType);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerName);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemCategory);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.txtToDocNo);
		this.groupBox1.Controls.Add(this.Fromdate);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Controls.Add(this.txtFromNo);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(7, 6);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(573, 189);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.cmbBroker.AutoSize = true;
		this.cmbBroker.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbBroker.Location = new System.Drawing.Point(253, 162);
		this.cmbBroker.Name = "cmbBroker";
		this.cmbBroker.Size = new System.Drawing.Size(41, 15);
		this.cmbBroker.TabIndex = 166;
		this.cmbBroker.Text = "Broker";
		this.cmbbrooker.AllowNull = (DefaultableBoolean)1;
		this.cmbbrooker.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbbrooker.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbbrooker.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbbrooker).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbbrooker).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbbrooker).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbbrooker).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbrooker).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.cmbbrooker).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbrooker).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbbrooker).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbbrooker).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbbrooker).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbbrooker).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbbrooker).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbbrooker).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbrooker).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbbrooker).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbbrooker).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbbrooker).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbrooker).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbbrooker).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbbrooker).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbbrooker).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbbrooker).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbbrooker).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbbrooker).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbbrooker).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbbrooker).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbbrooker).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbbrooker).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbbrooker.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbbrooker).Location = new System.Drawing.Point(316, 156);
		((System.Windows.Forms.Control)(object)this.cmbbrooker).Name = "cmbbrooker";
		((System.Windows.Forms.Control)(object)this.cmbbrooker).Size = new System.Drawing.Size(181, 26);
		((System.Windows.Forms.Control)(object)this.cmbbrooker).TabIndex = 11;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(4, 18);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(60, 15);
		this.label7.TabIndex = 164;
		this.label7.Text = "Date Type";
		this.cmbDateType.AllowNull = (DefaultableBoolean)1;
		this.cmbDateType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmbDateType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance96;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbDateType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbDateType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateType).Location = new System.Drawing.Point(96, 12);
		((System.Windows.Forms.Control)(object)this.cmbDateType).Name = "cmbDateType";
		((System.Windows.Forms.Control)(object)this.cmbDateType).Size = new System.Drawing.Size(151, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateType).TabIndex = 0;
		this.cmbDateType.ValueChanged += new System.EventHandler(cmbDateType_ValueChanged);
		this.ToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.CustomFormat = "dd-MMM-yy";
		this.ToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDate.Location = new System.Drawing.Point(96, 72);
		this.ToDate.Name = "ToDate";
		this.ToDate.Size = new System.Drawing.Size(151, 23);
		this.ToDate.TabIndex = 2;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(503, 158);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(62, 25);
		this.btnshow.TabIndex = 12;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.btnshow.Leave += new System.EventHandler(btnshow_Leave);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(253, 134);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(51, 15);
		this.label2.TabIndex = 145;
		this.label2.Text = "Supplier";
		this.CmbInventoryParentCategories.AllowNull = (DefaultableBoolean)1;
		this.CmbInventoryParentCategories.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInventoryParentCategories.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInventoryParentCategories.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInventoryParentCategories).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbInventoryParentCategories).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInventoryParentCategories).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInventoryParentCategories).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInventoryParentCategories).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.CmbInventoryParentCategories).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInventoryParentCategories).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbInventoryParentCategories).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInventoryParentCategories).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInventoryParentCategories).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInventoryParentCategories).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbInventoryParentCategories).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInventoryParentCategories).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInventoryParentCategories).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance70).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInventoryParentCategories).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbInventoryParentCategories).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInventoryParentCategories).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance71).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInventoryParentCategories).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInventoryParentCategories).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbInventoryParentCategories).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInventoryParentCategories).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInventoryParentCategories).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbInventoryParentCategories).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInventoryParentCategories).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbInventoryParentCategories).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInventoryParentCategories).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInventoryParentCategories).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInventoryParentCategories).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInventoryParentCategories.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInventoryParentCategories).Location = new System.Drawing.Point(96, 156);
		((System.Windows.Forms.Control)(object)this.CmbInventoryParentCategories).Name = "CmbInventoryParentCategories";
		((System.Windows.Forms.Control)(object)this.CmbInventoryParentCategories).Size = new System.Drawing.Size(151, 26);
		((System.Windows.Forms.Control)(object)this.CmbInventoryParentCategories).TabIndex = 5;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(253, 76);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(32, 15);
		this.label9.TabIndex = 144;
		this.label9.Text = "Item";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(4, 162);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(91, 15);
		this.label5.TabIndex = 162;
		this.label5.Text = "Parent Category";
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(316, 128);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(249, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 10;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(253, 47);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(60, 15);
		this.label3.TabIndex = 160;
		this.label3.Text = "Item Type";
		this.CmbItem.AllowNull = (DefaultableBoolean)1;
		this.CmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItem).Location = new System.Drawing.Point(316, 70);
		((System.Windows.Forms.Control)(object)this.CmbItem).Name = "CmbItem";
		((System.Windows.Forms.Control)(object)this.CmbItem).Size = new System.Drawing.Size(249, 26);
		((System.Windows.Forms.Control)(object)this.CmbItem).TabIndex = 8;
		this.CmbItemType.AllowNull = (DefaultableBoolean)1;
		this.CmbItemType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbItemType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance77;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbItemType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance83).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance84).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbItemType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemType).Location = new System.Drawing.Point(316, 41);
		((System.Windows.Forms.Control)(object)this.CmbItemType).Name = "CmbItemType";
		((System.Windows.Forms.Control)(object)this.CmbItemType).Size = new System.Drawing.Size(249, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemType).TabIndex = 7;
		this.CmbCustomerName.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Location = new System.Drawing.Point(316, 99);
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Name = "CmbCustomerName";
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Size = new System.Drawing.Size(249, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).TabIndex = 9;
		this.CmbItemCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance80;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance92).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Location = new System.Drawing.Point(316, 12);
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Name = "CmbItemCategory";
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Size = new System.Drawing.Size(249, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).TabIndex = 6;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(253, 105);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(58, 15);
		this.label6.TabIndex = 146;
		this.label6.Text = "Customer";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(253, 18);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(54, 15);
		this.label4.TabIndex = 159;
		this.label4.Text = "Category";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(4, 76);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(51, 15);
		this.label8.TabIndex = 155;
		this.label8.Text = "To  Date";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(4, 47);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 155;
		this.label27.Text = "From Date";
		this.txtToDocNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNo.Location = new System.Drawing.Point(96, 130);
		this.txtToDocNo.Name = "txtToDocNo";
		this.txtToDocNo.Size = new System.Drawing.Size(151, 23);
		this.txtToDocNo.TabIndex = 4;
		this.txtToDocNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNo_KeyPress);
		this.Fromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Fromdate.CustomFormat = "dd-MMM-yy";
		this.Fromdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Fromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.Fromdate.Location = new System.Drawing.Point(96, 43);
		this.Fromdate.Name = "Fromdate";
		this.Fromdate.Size = new System.Drawing.Size(151, 23);
		this.Fromdate.TabIndex = 1;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(4, 134);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(64, 15);
		this.label11.TabIndex = 156;
		this.label11.Text = "Doc No To";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(4, 105);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(79, 15);
		this.label35.TabIndex = 156;
		this.label35.Text = "Doc No From";
		this.txtFromNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromNo.Location = new System.Drawing.Point(96, 101);
		this.txtFromNo.Name = "txtFromNo";
		this.txtFromNo.Size = new System.Drawing.Size(151, 23);
		this.txtFromNo.TabIndex = 3;
		this.txtFromNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromNo_KeyPress);
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 27);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(984, 32);
		this.panel2.TabIndex = 4;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(7, 7);
		this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(321, 19);
		this.label1.TabIndex = 0;
		this.label1.Text = "Commission Delivery Loading Register";
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btn_NewClick, this.btnRefresh, this.btn555Register, this.btnShortCutKeys });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(984, 27);
		this.toolStrip3.TabIndex = 3;
		this.toolStrip3.Text = "toolStrip3";
		this.btn_NewClick.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btn_NewClick.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btn_NewClick.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btn_NewClick.Name = "btn_NewClick";
		this.btn_NewClick.Size = new System.Drawing.Size(59, 24);
		this.btn_NewClick.Text = "&New";
		this.btn_NewClick.Click += new System.EventHandler(btn_NewClick_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btn555Register.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btn555Register.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btn555Register.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btn555Register.Name = "btn555Register";
		this.btn555Register.Size = new System.Drawing.Size(123, 24);
		this.btn555Register.Text = "1702-Register";
		this.btn555Register.ToolTipText = "1702-Register";
		this.btn555Register.Click += new System.EventHandler(btn555Register_Click);
		this.btnShortCutKeys.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnShortCutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKeys.Image = (System.Drawing.Image)resources.GetObject("btnShortCutKeys.Image");
		this.btnShortCutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeys.Name = "btnShortCutKeys";
		this.btnShortCutKeys.Size = new System.Drawing.Size(106, 24);
		this.btnShortCutKeys.Text = "ShortCut Keys";
		this.btnShortCutKeys.Click += new System.EventHandler(btnShortCutKeys_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(984, 661);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "CommisisionDeleiveryLoadingRegister";
		this.Text = "CommisisionDeleiveryLoadingRegister";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(CommisisionDeleiveryLoadingRegister_FormClosed);
		base.Load += new System.EventHandler(CommisisionDeleiveryLoadingRegister_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(CommisisionDeleiveryLoadingRegister_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbbrooker).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInventoryParentCategories).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCategory).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
