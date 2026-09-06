using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Lab;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Common;
using Architecture.Reports;
using Architecture.WinApp.Commentaries;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Lab;

public class LabDataVehicleWiseByParent : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable LabData = new DataTable();

	private DataTable DS_Table01 = new DataTable();

	private DataTable DS_Table00 = new DataTable();

	public int ParentCategoryId;

	public int ItemId;

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private ToolStripButton btnShortCut;

	private Panel panel5;

	private Label label3;

	private Panel panel3;

	private GroupBox groupBox1;

	internal UltraCombo CmbParentCategory;

	private Label label9;

	internal UltraCombo CmbSupplier;

	private Label label1;

	private Label label27;

	public TextBox txtOrderNo;

	private Button btnshow;

	private Label label2;

	public TextBox txtMoistureFrom;

	private Label label4;

	public TextBox txtMoistureTo;

	internal GridEX grdLabSummary;

	private Panel panel4;

	private CtrlGrdBar ctrlGrdBar1;

	internal UltraCombo CmbItemName;

	private Label label5;

	private Panel panel7;

	internal GridEX grdLabDetail;

	private Label label6;

	private Label label7;

	private Panel panel9;

	private Panel panel8;

	public DateTimePicker datFromDate;

	public DateTimePicker datToDate;

	private Label label8;

	private ToolStripButton Print;

	private Label label10;

	public TextBox txtSlab1From;

	public TextBox txtSlab1To;

	public TextBox txtSlab3To;

	private Label label12;

	public TextBox txtSlab3From;

	public TextBox txtSlab2To;

	private Label label11;

	public TextBox txtSlab2From;

	public LabDataVehicleWiseByParent(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void LabDataVehicleWiseByParent_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(978, 732);
			ctrlGrdBar1.mGridChooseFielder.Visible = false;
			ctrlGrdBar1.mGridSaveLayouts.Visible = false;
			ctrlGrdBar1.removeLayoutToolStripMenuItem.Visible = false;
			datFromDate.Focus();
			datFromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			ParentCategoryFill();
			if (ParentCategoryId > 0 && ItemId > 0)
			{
				CmbParentCategory.Value = ParentCategoryId;
				CmbParentCategory_Leave(null, null);
				CmbItemName.Value = ItemId;
				gridHistory();
			}
			else
			{
				((UltraGridBase)CmbParentCategory).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ParentCategoryFill()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		try
		{
			DataTable dtParentCat = new DataTable();
			dtParentCat.Columns.Add("Id");
			dtParentCat.Columns.Add("name");
			DataTable dt = InvLabAnalysisPurchaseHeader.GetDataForDropDownFromLabPurchaseAnaylsis(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Activity = "ParentCategories",
				DocumentTypeIds = "303"
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtParentCat.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
			}
			DDL.BindDDL(dtParentCat, CmbParentCategory, "Id", "name", "Parent Categories", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryComboFill()
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		try
		{
			DataTable dtSupp = new DataTable();
			dtSupp.Columns.Add("Id");
			dtSupp.Columns.Add("name");
			DataTable dtItem = new DataTable();
			dtItem.Columns.Add("Id");
			dtItem.Columns.Add("name");
			DataTable dt = InvLabAnalysisPurchaseHeader.GetDataForDropDownFromLabPurchaseAnaylsis(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Activity = "",
				DocumentTypeIds = "303",
				ParentId = Conversion.ToInt(CmbParentCategory.Value),
				BranchesIds = UserAccount.BranchesId.ToString()
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Supplier")
					{
						dtSupp.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Item")
					{
						dtItem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtSupp, CmbSupplier, "Id", "name", "Supplier Name", ZeroIndex: true);
			((UltraGridBase)CmbSupplier).Rows[0].Activate();
			DDL.BindDDL(dtItem, CmbItemName, "Id", "name", "Item Name", ZeroIndex: true);
			((UltraGridBase)CmbItemName).Rows[0].Activate();
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
			((Control)(object)CmbSupplier).Text = string.Empty;
			txtOrderNo.Text = string.Empty;
			txtOrderNo.Text = string.Empty;
			txtMoistureFrom.Text = string.Empty;
			txtMoistureTo.Text = string.Empty;
			txtSlab1From.Text = string.Empty;
			txtSlab2From.Text = string.Empty;
			txtSlab3From.Text = string.Empty;
			txtSlab1To.Text = string.Empty;
			txtSlab2To.Text = string.Empty;
			txtSlab3To.Text = string.Empty;
			((Control)(object)CmbParentCategory).Text = string.Empty;
			datFromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			datFromDate.Focus();
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
			Reset();
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
			if (((UltraGridBase)CmbParentCategory).ActiveRow == null)
			{
				MessageBox.Show("Please Select Parent Category First...");
			}
			else
			{
				gridHistory();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridHistory()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		try
		{
			DataSet ds = new DataSet();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.InventoryParentCategories = Conversion.ToInt(CmbParentCategory.Value);
			obj.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			obj.ItemId = Conversion.ToInt(CmbItemName.Value);
			obj.OrderNo = Conversion.ToString((object)txtOrderNo.Text.Trim());
			if (datFromDate.Checked)
			{
				obj.FromDate = Conversion.ToDateTime((object)datFromDate.Value);
			}
			if (datToDate.Checked)
			{
				obj.ToDate = Conversion.ToDateTime((object)datToDate.Value);
			}
			obj.FromDocNo = Conversion.ToInt((object)txtMoistureFrom.Text.Trim());
			obj.ToDocNo = Conversion.ToInt((object)txtMoistureTo.Text.Trim());
			obj.SlabFrom1 = Conversion.ToInt((object)txtSlab1From.Text.Trim());
			obj.SlabFrom2 = Conversion.ToInt((object)txtSlab2From.Text.Trim());
			obj.SlabFrom3 = Conversion.ToInt((object)txtSlab3From.Text.Trim());
			obj.SlabTo1 = Conversion.ToInt((object)txtSlab1To.Text.Trim());
			obj.SlabTo2 = Conversion.ToInt((object)txtSlab2To.Text.Trim());
			obj.SlabTo3 = Conversion.ToInt((object)txtSlab3To.Text.Trim());
			ds = InvLabAnalysisPurchaseHeader.LabDataVehicleWiseByParent(obj);
			if (ds.Tables.Count > 0)
			{
				DS_Table00 = ds.Tables[0];
				DS_Table01 = ds.Tables[1];
				if (DS_Table00.Rows.Count > 0)
				{
					DataTable dtLabDetail = new DataTable();
					dtLabDetail.Columns.Add("Id", typeof(int));
					dtLabDetail.Columns.Add("GatePassInwardId", typeof(int));
					dtLabDetail.Columns.Add("PurchaseType", typeof(string));
					dtLabDetail.Columns.Add("LabDate", typeof(DateTime));
					dtLabDetail.Columns.Add("LabNo", typeof(int));
					dtLabDetail.Columns.Add("OrderId", typeof(int));
					dtLabDetail.Columns.Add("OrderNo", typeof(string));
					dtLabDetail.Columns.Add("SupplierCustomerId", typeof(int));
					dtLabDetail.Columns.Add("Supplier", typeof(string));
					dtLabDetail.Columns.Add("VehicleNo", typeof(string));
					dtLabDetail.Columns.Add("ItemId", typeof(int));
					dtLabDetail.Columns.Add("ItemName", typeof(string));
					dtLabDetail.Columns.Add("AnalysisQty", typeof(double));
					dtLabDetail.Columns.Add("NetWeight", typeof(double));
					dtLabDetail.Columns.Add("QtyForWtCut", typeof(double));
					dtLabDetail.Columns.Add("WeightCut", typeof(double));
					dtLabDetail.Columns.Add("ItemRate", typeof(double));
					dtLabDetail.Columns.Add("RateCut", typeof(double));
					dtLabDetail.Columns.Add("NetRate", typeof(double));
					dtLabDetail.Columns.Add("Moisture", typeof(double));
					dtLabDetail.Columns.Add("Empty Shell / Trash", typeof(double));
					dtLabDetail.Columns.Add("Dust/Stone", typeof(double));
					dtLabDetail.Columns.Add("Broken", typeof(double));
					dtLabDetail.Columns.Add("AGL", typeof(double));
					dtLabDetail.Columns.Add("Damage", typeof(double));
					dtLabDetail.Columns.Add("Chooba", typeof(double));
					for (int j = 0; j < DS_Table00.Rows.Count; j++)
					{
						dtLabDetail.Rows.Add(Conversion.ToInt(DS_Table00.Rows[j]["Id"]), Conversion.ToInt(DS_Table00.Rows[j]["GatePassInwardId"]), Conversion.ToString(DS_Table00.Rows[j]["PurchaseType"]), Conversion.ToDateTime(DS_Table00.Rows[j]["LabDate"]), Conversion.ToInt(DS_Table00.Rows[j]["LabNo"]), Conversion.ToInt(DS_Table00.Rows[j]["OrderId"]), Conversion.ToString(DS_Table00.Rows[j]["OrderNo"]), Conversion.ToInt(DS_Table00.Rows[j]["SupplierCustomerId"]), Conversion.ToString(DS_Table00.Rows[j]["Supplier"]), Conversion.ToString(DS_Table00.Rows[j]["VehicleNo"]), Conversion.ToInt(DS_Table00.Rows[j]["ItemId"]), Conversion.ToString(DS_Table00.Rows[j]["ItemName"]), Conversion.ToDouble(DS_Table00.Rows[j]["AnalysisQty"]), Conversion.ToDouble(DS_Table00.Rows[j]["NetWeight"]), Conversion.ToDouble(DS_Table00.Rows[j]["QtyForWtCut"]), Conversion.ToDouble(DS_Table00.Rows[j]["WeightCut"]), Conversion.ToDouble(DS_Table00.Rows[j]["ItemRate"]), Conversion.ToDouble(DS_Table00.Rows[j]["RateCut"]), Conversion.ToDouble(DS_Table00.Rows[j]["NetRate"]), Conversion.ToDouble(DS_Table00.Rows[j]["Moisture"]), Conversion.ToDouble(DS_Table00.Rows[j]["Empty Shell / Trash"]), Conversion.ToDouble(DS_Table00.Rows[j]["Dust/Stone"]), Conversion.ToDouble(DS_Table00.Rows[j]["Broken"]), Conversion.ToDouble(DS_Table00.Rows[j]["AGL"]), Conversion.ToDouble(DS_Table00.Rows[j]["Damage"]), Conversion.ToDouble(DS_Table00.Rows[j]["Chooba"]));
					}
					grdLabDetail.DataSource = dtLabDetail;
					GridSetting();
				}
				else
				{
					grdLabDetail.DataSource = null;
				}
				if (DS_Table01.Rows.Count > 0)
				{
					DataTable dtSummary = new DataTable();
					dtSummary.Columns.Add("ItemId", typeof(string));
					dtSummary.Columns.Add("ItemName", typeof(string));
					dtSummary.Columns.Add("TotalAnalysisQty", typeof(double));
					dtSummary.Columns.Add("TotalNetWeight", typeof(double));
					dtSummary.Columns.Add("AvgBroken", typeof(double));
					dtSummary.Columns.Add("AvgMoisture", typeof(double));
					dtSummary.Columns.Add("WeightedAvgBroken", typeof(double));
					dtSummary.Columns.Add("WeightedAvgMoisture", typeof(double));
					dtSummary.Columns.Add("MinMoisture", typeof(double));
					dtSummary.Columns.Add("MaxMoisture", typeof(double));
					dtSummary.Columns.Add("MinBroken", typeof(double));
					dtSummary.Columns.Add("MaxBroken", typeof(double));
					dtSummary.Columns.Add("Slab1", typeof(double));
					dtSummary.Columns.Add("Slab2", typeof(double));
					dtSummary.Columns.Add("Slab3", typeof(double));
					for (int i = 0; i < DS_Table01.Rows.Count; i++)
					{
						dtSummary.Rows.Add(Conversion.ToInt(DS_Table01.Rows[i]["ItemId"]), Conversion.ToString(DS_Table01.Rows[i]["ItemName"]), Conversion.ToDouble(DS_Table01.Rows[i]["TotalAnalysisQty"]), Conversion.ToDouble(DS_Table01.Rows[i]["TotalNetWeight"]), Conversion.ToDouble(DS_Table01.Rows[i]["AvgBroken"]), Conversion.ToDouble(DS_Table01.Rows[i]["AvgMoisture"]), Conversion.ToDouble(DS_Table01.Rows[i]["WeightedAvgBroken"]), Conversion.ToDouble(DS_Table01.Rows[i]["WeightedAvgMoisture"]), Conversion.ToDouble(DS_Table01.Rows[i]["MinMoisture"]), Conversion.ToDouble(DS_Table01.Rows[i]["MaxMoisture"]), Conversion.ToDouble(DS_Table01.Rows[i]["MinBroken"]), Conversion.ToDouble(DS_Table01.Rows[i]["MaxBroken"]), Conversion.ToDouble(DS_Table01.Rows[i]["WeightedAvgSlab1"]), Conversion.ToDouble(DS_Table01.Rows[i]["WeightedAvgSlab2"]), Conversion.ToDouble(DS_Table01.Rows[i]["WeightedAvgSlab3"]));
					}
					grdLabSummary.DataSource = dtSummary;
					GridSummarySetting();
				}
				else
				{
					grdLabSummary.DataSource = null;
				}
			}
			else
			{
				grdLabDetail.DataSource = null;
				grdLabSummary.DataSource = null;
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
			grdLabDetail.RootTable.Groups.Remove(grdLabDetail.RootTable.Groups["ItemName"]);
			grdLabDetail.RootTable.Groups.Add("ItemName");
			grdLabDetail.RootTable.ColumnSets["ItemName"].Visible = false;
			grdLabDetail.RootTable.GroupTotals = (GroupTotals)2;
			grdLabDetail.RootTable.Columns["ItemId"].Visible = false;
			grdLabDetail.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grdLabDetail.RootTable.Columns["Id"].Visible = false;
			grdLabDetail.RootTable.Columns["GatePassInwardId"].Visible = false;
			grdLabDetail.RootTable.Columns["OrderId"].Visible = false;
			grdLabDetail.RootTable.ColumnSets["QtyForWtCut"].Visible = false;
			grdLabDetail.RootTable.ColumnSets["Broken"].Visible = false;
			grdLabDetail.RootTable.ColumnSets["AGL"].Visible = false;
			grdLabDetail.RootTable.ColumnSets["Damage"].Visible = false;
			grdLabDetail.RootTable.ColumnSets["Empty Shell / Trash"].Visible = false;
			grdLabDetail.RootTable.ColumnSets["Dust/Stone"].Visible = false;
			grdLabDetail.RootTable.ColumnSets["Chooba"].Visible = false;
			if (((Control)(object)CmbParentCategory).Text == "Paddy")
			{
				grdLabDetail.RootTable.ColumnSets["Empty Shell / Trash"].Visible = true;
				grdLabDetail.RootTable.ColumnSets["Chooba"].Visible = true;
				grdLabDetail.RootTable.ColumnSets["Damage"].Visible = true;
			}
			if (((Control)(object)CmbParentCategory).Text == "Rice")
			{
				grdLabDetail.RootTable.ColumnSets["Broken"].Visible = true;
				grdLabDetail.RootTable.ColumnSets["AGL"].Visible = true;
				grdLabDetail.RootTable.ColumnSets["Damage"].Visible = true;
			}
			grdLabDetail.RootTable.Columns["LabDate"].FormatString = "dd-MMM-yy";
			grdLabDetail.RootTable.Columns["LabDate"].Width = Constants.ExportConstants.DateConstant;
			grdLabDetail.RootTable.Columns["LabDate"].TextAlignment = (TextAlignment)2;
			grdLabDetail.RootTable.Columns["LabNo"].ColumnType = (ColumnType)5;
			grdLabDetail.RootTable.Columns["OrderNo"].ColumnType = (ColumnType)5;
			grdLabDetail.RootTable.Columns["PurchaseType"].Width = Constants.ExportConstants.InvoiceNoConstant;
			grdLabDetail.RootTable.Columns["LabNo"].Width = Constants.ExportConstants.DocNoConstant;
			grdLabDetail.RootTable.Columns["OrderNo"].Width = Constants.ExportConstants.DocNoConstant;
			grdLabDetail.RootTable.Columns["Supplier"].Width = Constants.ExportConstants.StringConstant;
			grdLabDetail.RootTable.Columns["ItemName"].Width = Constants.ExportConstants.StringConstant;
			grdLabDetail.RootTable.Columns["VehicleNo"].Width = Constants.ExportConstants.VehicleNoConstant;
			grdLabDetail.RootTable.Columns["AnalysisQty"].Width = Constants.ExportConstants.QtyConstant;
			grdLabDetail.RootTable.Columns["NetWeight"].Width = Constants.ExportConstants.WeightConstant;
			grdLabDetail.RootTable.Columns["QtyForWtCut"].Width = Constants.ExportConstants.QtyConstant;
			grdLabDetail.RootTable.Columns["WeightCut"].Width = Constants.ExportConstants.WeightConstant;
			grdLabDetail.RootTable.Columns["ItemRate"].Width = Constants.ExportConstants.ExRateConstant;
			grdLabDetail.RootTable.Columns["RateCut"].Width = Constants.ExportConstants.FclConstant;
			grdLabDetail.RootTable.Columns["NetRate"].Width = Constants.ExportConstants.FclConstant;
			grdLabDetail.RootTable.Columns["Moisture"].Width = Constants.ExportConstants.FclConstant;
			grdLabDetail.RootTable.Columns["Broken"].Width = Constants.ExportConstants.FclConstant;
			grdLabDetail.RootTable.Columns["AGL"].Width = Constants.ExportConstants.FclConstant;
			grdLabDetail.RootTable.Columns["Damage"].Width = Constants.ExportConstants.FclConstant;
			grdLabDetail.RootTable.Columns["Empty Shell / Trash"].Width = Constants.ExportConstants.CurrentStatus;
			grdLabDetail.RootTable.Columns["Dust/Stone"].Width = Constants.ExportConstants.FclConstant;
			grdLabDetail.RootTable.Columns["Chooba"].Width = Constants.ExportConstants.FclConstant;
			grdLabDetail.RootTable.Columns["Broken"].TextAlignment = (TextAlignment)3;
			grdLabDetail.RootTable.Columns["AGL"].TextAlignment = (TextAlignment)3;
			grdLabDetail.RootTable.Columns["Damage"].TextAlignment = (TextAlignment)3;
			grdLabDetail.RootTable.Columns["Empty Shell / Trash"].TextAlignment = (TextAlignment)3;
			grdLabDetail.RootTable.Columns["Dust/Stone"].TextAlignment = (TextAlignment)3;
			grdLabDetail.RootTable.Columns["Chooba"].TextAlignment = (TextAlignment)3;
			grdLabDetail.RootTable.Columns["AnalysisQty"].AggregateFunction = (AggregateFunction)2;
			grdLabDetail.RootTable.Columns["AnalysisQty"].TextAlignment = (TextAlignment)3;
			grdLabDetail.RootTable.Columns["AnalysisQty"].FormatString = "#,#.##";
			grdLabDetail.RootTable.Columns["AnalysisQty"].TotalFormatString = "#,#.##";
			grdLabDetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdLabDetail.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdLabDetail.RootTable.Columns["NetWeight"].FormatString = "#,#";
			grdLabDetail.RootTable.Columns["NetWeight"].TotalFormatString = "#,#";
			grdLabDetail.RootTable.Columns["QtyForWtCut"].AggregateFunction = (AggregateFunction)2;
			grdLabDetail.RootTable.Columns["QtyForWtCut"].TextAlignment = (TextAlignment)3;
			grdLabDetail.RootTable.Columns["QtyForWtCut"].FormatString = "#,#";
			grdLabDetail.RootTable.Columns["QtyForWtCut"].TotalFormatString = "#,#";
			grdLabDetail.RootTable.Columns["WeightCut"].AggregateFunction = (AggregateFunction)2;
			grdLabDetail.RootTable.Columns["WeightCut"].TextAlignment = (TextAlignment)3;
			grdLabDetail.RootTable.Columns["WeightCut"].FormatString = "#,#.##";
			grdLabDetail.RootTable.Columns["WeightCut"].TotalFormatString = "#,#.##";
			grdLabDetail.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdLabDetail.RootTable.Columns["ItemRate"].FormatString = "#,#.##";
			grdLabDetail.RootTable.Columns["ItemRate"].TotalFormatString = "#,#.##";
			grdLabDetail.RootTable.Columns["RateCut"].AggregateFunction = (AggregateFunction)2;
			grdLabDetail.RootTable.Columns["RateCut"].TextAlignment = (TextAlignment)3;
			grdLabDetail.RootTable.Columns["RateCut"].FormatString = "#,#.##";
			grdLabDetail.RootTable.Columns["RateCut"].TotalFormatString = "#,#.##";
			grdLabDetail.RootTable.Columns["NetRate"].AggregateFunction = (AggregateFunction)3;
			grdLabDetail.RootTable.Columns["NetRate"].TextAlignment = (TextAlignment)3;
			grdLabDetail.RootTable.Columns["NetRate"].FormatString = "#,#.##";
			grdLabDetail.RootTable.Columns["NetRate"].TotalFormatString = "#,#.##";
			grdLabDetail.RootTable.Columns["Moisture"].AggregateFunction = (AggregateFunction)3;
			grdLabDetail.RootTable.Columns["Moisture"].TextAlignment = (TextAlignment)3;
			grdLabDetail.RootTable.Columns["Moisture"].FormatString = "#,#.##";
			grdLabDetail.RootTable.Columns["Moisture"].TotalFormatString = "#,#.##";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSummarySetting()
	{
		try
		{
			grdLabSummary.RootTable.Columns["ItemId"].Visible = false;
			grdLabSummary.RootTable.ColumnSets["AvgBroken"].Visible = false;
			grdLabSummary.RootTable.ColumnSets["WeightedAvgBroken"].Visible = false;
			grdLabSummary.RootTable.ColumnSets["AvgMoisture"].Visible = false;
			grdLabSummary.RootTable.ColumnSets["WeightedAvgMoisture"].Visible = false;
			grdLabSummary.RootTable.ColumnSets["MinMoisture"].Visible = false;
			grdLabSummary.RootTable.ColumnSets["MaxMoisture"].Visible = false;
			grdLabSummary.RootTable.ColumnSets["MinBroken"].Visible = false;
			grdLabSummary.RootTable.ColumnSets["MaxBroken"].Visible = false;
			grdLabSummary.RootTable.ColumnSets["Slab1"].Visible = false;
			grdLabSummary.RootTable.ColumnSets["Slab2"].Visible = false;
			grdLabSummary.RootTable.ColumnSets["Slab3"].Visible = false;
			if (((Control)(object)CmbParentCategory).Text == "Paddy")
			{
				grdLabSummary.RootTable.ColumnSets["AvgMoisture"].Visible = true;
				grdLabSummary.RootTable.ColumnSets["WeightedAvgMoisture"].Visible = true;
				grdLabSummary.RootTable.ColumnSets["MinMoisture"].Visible = true;
				grdLabSummary.RootTable.ColumnSets["MaxMoisture"].Visible = true;
				grdLabSummary.RootTable.ColumnSets["Slab1"].Visible = true;
				grdLabSummary.RootTable.ColumnSets["Slab2"].Visible = true;
				grdLabSummary.RootTable.ColumnSets["Slab3"].Visible = true;
				grdLabSummary.RootTable.ColumnSets["WeightedAvgMoisture"].Caption = "WtAvgMoisture";
			}
			if (((Control)(object)CmbParentCategory).Text == "Rice")
			{
				grdLabSummary.RootTable.ColumnSets["AvgBroken"].Visible = true;
				grdLabSummary.RootTable.ColumnSets["WeightedAvgBroken"].Visible = true;
				grdLabSummary.RootTable.ColumnSets["MinBroken"].Visible = true;
				grdLabSummary.RootTable.ColumnSets["MaxBroken"].Visible = true;
			}
			grdLabSummary.RootTable.Columns["ItemName"].Width = 220;
			grdLabSummary.RootTable.Columns["TotalAnalysisQty"].Width = Constants.ExportConstants.QtyConstant;
			grdLabSummary.RootTable.Columns["TotalNetWeight"].Width = Constants.ExportConstants.WeightConstant;
			grdLabSummary.RootTable.Columns["AvgMoisture"].Width = Constants.ExportConstants.FcyConstant;
			grdLabSummary.RootTable.Columns["WeightedAvgMoisture"].Width = Constants.ExportConstants.WeightConstant;
			grdLabSummary.RootTable.Columns["AvgBroken"].Width = Constants.ExportConstants.FcyConstant;
			grdLabSummary.RootTable.Columns["WeightedAvgBroken"].Width = Constants.ExportConstants.WeightConstant;
			grdLabSummary.RootTable.Columns["MinMoisture"].Width = Constants.ExportConstants.FcyConstant;
			grdLabSummary.RootTable.Columns["MaxMoisture"].Width = Constants.ExportConstants.FcyConstant;
			grdLabSummary.RootTable.Columns["MinBroken"].Width = Constants.ExportConstants.FcyConstant;
			grdLabSummary.RootTable.Columns["MaxBroken"].Width = Constants.ExportConstants.FcyConstant;
			grdLabSummary.RootTable.Columns["Slab1"].Width = Constants.ExportConstants.FclConstant;
			grdLabSummary.RootTable.Columns["Slab2"].Width = Constants.ExportConstants.FclConstant;
			grdLabSummary.RootTable.Columns["Slab3"].Width = Constants.ExportConstants.FclConstant;
			grdLabSummary.RootTable.Columns["TotalAnalysisQty"].AggregateFunction = (AggregateFunction)2;
			grdLabSummary.RootTable.Columns["TotalAnalysisQty"].TextAlignment = (TextAlignment)3;
			grdLabSummary.RootTable.Columns["TotalAnalysisQty"].FormatString = "#,#.##";
			grdLabSummary.RootTable.Columns["TotalAnalysisQty"].TotalFormatString = "#,#.##";
			grdLabSummary.RootTable.Columns["TotalNetWeight"].AggregateFunction = (AggregateFunction)2;
			grdLabSummary.RootTable.Columns["TotalNetWeight"].TextAlignment = (TextAlignment)3;
			grdLabSummary.RootTable.Columns["TotalNetWeight"].FormatString = "#,#.##";
			grdLabSummary.RootTable.Columns["TotalNetWeight"].TotalFormatString = "#,#.##";
			grdLabSummary.RootTable.Columns["AvgBroken"].AggregateFunction = (AggregateFunction)3;
			grdLabSummary.RootTable.Columns["AvgBroken"].TextAlignment = (TextAlignment)3;
			grdLabSummary.RootTable.Columns["AvgBroken"].FormatString = "#,#.##";
			grdLabSummary.RootTable.Columns["AvgBroken"].TotalFormatString = "#,#.##";
			grdLabSummary.RootTable.Columns["WeightedAvgBroken"].AggregateFunction = (AggregateFunction)3;
			grdLabSummary.RootTable.Columns["WeightedAvgBroken"].TextAlignment = (TextAlignment)3;
			grdLabSummary.RootTable.Columns["WeightedAvgBroken"].FormatString = "#,#.##";
			grdLabSummary.RootTable.Columns["WeightedAvgBroken"].TotalFormatString = "#,#.##";
			grdLabSummary.RootTable.Columns["AvgMoisture"].AggregateFunction = (AggregateFunction)3;
			grdLabSummary.RootTable.Columns["AvgMoisture"].TextAlignment = (TextAlignment)3;
			grdLabSummary.RootTable.Columns["AvgMoisture"].FormatString = "#,#.##";
			grdLabSummary.RootTable.Columns["AvgMoisture"].TotalFormatString = "#,#.##";
			grdLabSummary.RootTable.Columns["WeightedAvgMoisture"].AggregateFunction = (AggregateFunction)3;
			grdLabSummary.RootTable.Columns["WeightedAvgMoisture"].TextAlignment = (TextAlignment)3;
			grdLabSummary.RootTable.Columns["WeightedAvgMoisture"].FormatString = "#,#.##";
			grdLabSummary.RootTable.Columns["WeightedAvgMoisture"].TotalFormatString = "#,#.##";
			grdLabSummary.RootTable.Columns["MinMoisture"].AggregateFunction = (AggregateFunction)3;
			grdLabSummary.RootTable.Columns["MinMoisture"].TextAlignment = (TextAlignment)3;
			grdLabSummary.RootTable.Columns["MinMoisture"].FormatString = "#,#.##";
			grdLabSummary.RootTable.Columns["MinMoisture"].TotalFormatString = "#,#.##";
			grdLabSummary.RootTable.Columns["MaxMoisture"].AggregateFunction = (AggregateFunction)3;
			grdLabSummary.RootTable.Columns["MaxMoisture"].TextAlignment = (TextAlignment)3;
			grdLabSummary.RootTable.Columns["MaxMoisture"].FormatString = "#,#.##";
			grdLabSummary.RootTable.Columns["MaxMoisture"].TotalFormatString = "#,#.##";
			grdLabSummary.RootTable.Columns["MinBroken"].AggregateFunction = (AggregateFunction)3;
			grdLabSummary.RootTable.Columns["MinBroken"].TextAlignment = (TextAlignment)3;
			grdLabSummary.RootTable.Columns["MinBroken"].FormatString = "#,#.##";
			grdLabSummary.RootTable.Columns["MinBroken"].TotalFormatString = "#,#.##";
			grdLabSummary.RootTable.Columns["MaxBroken"].AggregateFunction = (AggregateFunction)3;
			grdLabSummary.RootTable.Columns["MaxBroken"].TextAlignment = (TextAlignment)3;
			grdLabSummary.RootTable.Columns["MaxBroken"].FormatString = "#,#.##";
			grdLabSummary.RootTable.Columns["MaxBroken"].TotalFormatString = "#,#.##";
			grdLabSummary.RootTable.Columns["Slab1"].AggregateFunction = (AggregateFunction)3;
			grdLabSummary.RootTable.Columns["Slab1"].TextAlignment = (TextAlignment)3;
			grdLabSummary.RootTable.Columns["Slab1"].FormatString = "#,#.##";
			grdLabSummary.RootTable.Columns["Slab1"].TotalFormatString = "#,#.##";
			grdLabSummary.RootTable.Columns["Slab2"].AggregateFunction = (AggregateFunction)3;
			grdLabSummary.RootTable.Columns["Slab2"].TextAlignment = (TextAlignment)3;
			grdLabSummary.RootTable.Columns["Slab2"].FormatString = "#,#.##";
			grdLabSummary.RootTable.Columns["Slab2"].TotalFormatString = "#,#.##";
			grdLabSummary.RootTable.Columns["Slab3"].AggregateFunction = (AggregateFunction)3;
			grdLabSummary.RootTable.Columns["Slab3"].TextAlignment = (TextAlignment)3;
			grdLabSummary.RootTable.Columns["Slab3"].FormatString = "#,#.##";
			grdLabSummary.RootTable.Columns["Slab3"].TotalFormatString = "#,#.##";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmlabdatavehiclewise_KeyDown(object sender, KeyEventArgs e)
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
				HistoryStack.Remove(this);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshow_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdLabSummary).Focus();
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
			dt.Rows.Add("Ctrl+S", "For show data");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on From Date");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
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

	private void btnShortCut_Click(object sender, EventArgs e)
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdLabDetail).Name);
			if (fs != null)
			{
				grdLabDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbParentCategory_Leave(object sender, EventArgs e)
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

	private void Print_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			InvLabAnalysisPurchaseHeader.LabDataVehicleWiseByParent(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				InventoryParentCategories = Conversion.ToInt(CmbParentCategory.Value)
			});
			if (DS_Table01 == null || DS_Table01.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(DS_Table01, "LabDataVehicleWiseSummary_Report.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(DS_Table00, "665-LabDataVehicleWiseByParentIdRegister.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdLabDetail_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "LabNo")
			{
				CommonServices.LabAnalysisReport653(Conversion.ToInt(grdLabDetail.CurrentRow.Cells["Id"].Value));
			}
			else
			{
				if (!(e.Column.Key == "OrderNo"))
				{
					return;
				}
				int OrderId = Conversion.ToInt(grdLabDetail.CurrentRow.Cells["OrderId"].Value);
				if (UserAccount.RoleName == "Admin" && OrderId > 0)
				{
					new ActivityDetails(UserAccount, OrderId).Show();
				}
				else
				{
					if (OrderId <= 0 || clsGlobalVariables.ScreenViewReights.Count <= 0)
					{
						return;
					}
					List<tblUserRights> newList = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "ActivityDetails").ToList();
					if (newList.Count > 0)
					{
						if (Conversion.ToBool((object)newList[0].Value.ToString()))
						{
							new ActivityDetails(UserAccount, OrderId).Show();
						}
						else
						{
							MessageBox.Show("you don't have Screen Rights");
						}
					}
					else
					{
						MessageBox.Show("you don't have Screen Rights");
					}
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdLabDetail_FormattingRow(object sender, RowLoadEventArgs e)
	{
	}

	private void LabDataVehicleWiseByParent_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
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
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
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
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Expected O, but got Unknown
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Expected O, but got Unknown
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Expected O, but got Unknown
		//IL_05ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f6: Expected O, but got Unknown
		//IL_0603: Unknown result type (might be due to invalid IL or missing references)
		//IL_060d: Expected O, but got Unknown
		GridEXLayout grdLabDetail_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Lab.LabDataVehicleWiseByParent));
		GridEXLayout grdLabSummary_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grdLabDetail = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label8 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdLabSummary = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtSlab3To = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.txtSlab3From = new System.Windows.Forms.TextBox();
		this.txtSlab2To = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.txtSlab2From = new System.Windows.Forms.TextBox();
		this.txtSlab1To = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.txtSlab1From = new System.Windows.Forms.TextBox();
		this.datFromDate = new System.Windows.Forms.DateTimePicker();
		this.datToDate = new System.Windows.Forms.DateTimePicker();
		this.label7 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.CmbItemName = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtMoistureTo = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtMoistureFrom = new System.Windows.Forms.TextBox();
		this.CmbParentCategory = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.CmbSupplier = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.txtOrderNo = new System.Windows.Forms.TextBox();
		this.btnshow = new System.Windows.Forms.Button();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.btnShortCut = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdLabDetail).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdLabSummary).BeginInit();
		this.panel3.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel8.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		this.panel5.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.panel7);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1118, 648);
		this.panel1.TabIndex = 0;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdLabDetail);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 272);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1118, 376);
		this.panel7.TabIndex = 26;
		this.grdLabDetail.AllowEdit = (InheritableBoolean)2;
		grdLabDetail_DesignTimeLayout.LayoutString = resources.GetString("grdLabDetail_DesignTimeLayout.LayoutString");
		this.grdLabDetail.DesignTimeLayout = grdLabDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdLabDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdLabDetail.DynamicFiltering = true;
		this.grdLabDetail.FilterMode = (FilterMode)2;
		this.grdLabDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdLabDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdLabDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdLabDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdLabDetail).Name = "grdLabDetail";
		((ScrollableBase)this.grdLabDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdLabDetail).Size = new System.Drawing.Size(1118, 376);
		((System.Windows.Forms.Control)(object)this.grdLabDetail).TabIndex = 24;
		this.grdLabDetail.TotalRow = (InheritableBoolean)1;
		this.grdLabDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdLabDetail).VisualStyle = (VisualStyle)4;
		this.grdLabDetail.FormattingRow += new RowLoadEventHandler(grdLabDetail_FormattingRow);
		this.grdLabDetail.LinkClicked += new ColumnActionEventHandler(grdLabDetail_LinkClicked);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add(this.ctrlGrdBar1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 248);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1118, 24);
		this.panel4.TabIndex = 24;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(4, 2);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(63, 19);
		this.label8.TabIndex = 1;
		this.label8.Text = "Detail ";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(2193, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdLabSummary;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(41, 22);
		this.ctrlGrdBar1.TabIndex = 3;
		this.grdLabSummary.AllowEdit = (InheritableBoolean)2;
		grdLabSummary_DesignTimeLayout.LayoutString = resources.GetString("grdLabSummary_DesignTimeLayout.LayoutString");
		this.grdLabSummary.DesignTimeLayout = grdLabSummary_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdLabSummary).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdLabSummary.DynamicFiltering = true;
		this.grdLabSummary.FilterMode = (FilterMode)2;
		this.grdLabSummary.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdLabSummary).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdLabSummary.GroupByBoxVisible = false;
		this.grdLabSummary.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdLabSummary).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdLabSummary).Name = "grdLabSummary";
		((ScrollableBase)this.grdLabSummary).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdLabSummary).Size = new System.Drawing.Size(504, 190);
		((System.Windows.Forms.Control)(object)this.grdLabSummary).TabIndex = 23;
		this.grdLabSummary.TotalRow = (InheritableBoolean)1;
		this.grdLabSummary.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdLabSummary).VisualStyle = (VisualStyle)4;
		this.panel3.Controls.Add(this.panel9);
		this.panel3.Controls.Add(this.panel8);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 58);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1118, 190);
		this.panel3.TabIndex = 22;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdLabSummary);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(614, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(504, 190);
		this.panel9.TabIndex = 1;
		this.panel8.Controls.Add(this.groupBox1);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(614, 190);
		this.panel8.TabIndex = 0;
		this.groupBox1.Controls.Add(this.txtSlab3To);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.txtSlab3From);
		this.groupBox1.Controls.Add(this.txtSlab2To);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.txtSlab2From);
		this.groupBox1.Controls.Add(this.txtSlab1To);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.txtSlab1From);
		this.groupBox1.Controls.Add(this.datFromDate);
		this.groupBox1.Controls.Add(this.datToDate);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.txtMoistureTo);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.txtMoistureFrom);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbParentCategory);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.txtOrderNo);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(8, 4);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(600, 182);
		this.groupBox1.TabIndex = 1;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.txtSlab3To.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSlab3To.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSlab3To.Location = new System.Drawing.Point(485, 123);
		this.txtSlab3To.Name = "txtSlab3To";
		this.txtSlab3To.ShortcutsEnabled = false;
		this.txtSlab3To.Size = new System.Drawing.Size(103, 23);
		this.txtSlab3To.TabIndex = 13;
		this.txtSlab3To.Text = "30";
		this.txtSlab3To.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(278, 127);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(96, 15);
		this.label12.TabIndex = 157;
		this.label12.Text = "Slab3 From && To";
		this.txtSlab3From.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSlab3From.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSlab3From.Location = new System.Drawing.Point(380, 123);
		this.txtSlab3From.Name = "txtSlab3From";
		this.txtSlab3From.ShortcutsEnabled = false;
		this.txtSlab3From.Size = new System.Drawing.Size(103, 23);
		this.txtSlab3From.TabIndex = 12;
		this.txtSlab3From.Text = "25";
		this.txtSlab3From.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtSlab2To.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSlab2To.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSlab2To.Location = new System.Drawing.Point(485, 98);
		this.txtSlab2To.Name = "txtSlab2To";
		this.txtSlab2To.ShortcutsEnabled = false;
		this.txtSlab2To.Size = new System.Drawing.Size(103, 23);
		this.txtSlab2To.TabIndex = 11;
		this.txtSlab2To.Text = "24";
		this.txtSlab2To.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(278, 102);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(96, 15);
		this.label11.TabIndex = 154;
		this.label11.Text = "Slab2 From && To";
		this.txtSlab2From.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSlab2From.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSlab2From.Location = new System.Drawing.Point(380, 98);
		this.txtSlab2From.Name = "txtSlab2From";
		this.txtSlab2From.ShortcutsEnabled = false;
		this.txtSlab2From.Size = new System.Drawing.Size(103, 23);
		this.txtSlab2From.TabIndex = 10;
		this.txtSlab2From.Text = "23";
		this.txtSlab2From.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtSlab1To.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSlab1To.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSlab1To.Location = new System.Drawing.Point(485, 73);
		this.txtSlab1To.Name = "txtSlab1To";
		this.txtSlab1To.ShortcutsEnabled = false;
		this.txtSlab1To.Size = new System.Drawing.Size(103, 23);
		this.txtSlab1To.TabIndex = 9;
		this.txtSlab1To.Text = "22";
		this.txtSlab1To.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(278, 77);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(94, 15);
		this.label10.TabIndex = 151;
		this.label10.Text = "Slab1 From && To";
		this.txtSlab1From.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSlab1From.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSlab1From.Location = new System.Drawing.Point(380, 73);
		this.txtSlab1From.Name = "txtSlab1From";
		this.txtSlab1From.ShortcutsEnabled = false;
		this.txtSlab1From.Size = new System.Drawing.Size(103, 23);
		this.txtSlab1From.TabIndex = 8;
		this.txtSlab1From.Text = "20";
		this.txtSlab1From.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.datFromDate.CustomFormat = "dd-MMM-yy";
		this.datFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFromDate.Location = new System.Drawing.Point(98, 20);
		this.datFromDate.Name = "datFromDate";
		this.datFromDate.ShowCheckBox = true;
		this.datFromDate.Size = new System.Drawing.Size(177, 23);
		this.datFromDate.TabIndex = 0;
		this.datToDate.CustomFormat = "dd-MMM-yy";
		this.datToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datToDate.Location = new System.Drawing.Point(98, 48);
		this.datToDate.Name = "datToDate";
		this.datToDate.ShowCheckBox = true;
		this.datToDate.Size = new System.Drawing.Size(177, 23);
		this.datToDate.TabIndex = 1;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(5, 52);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(48, 15);
		this.label7.TabIndex = 149;
		this.label7.Text = "To Date";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(5, 24);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(63, 15);
		this.label6.TabIndex = 148;
		this.label6.Text = "From Date";
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(380, 18);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(208, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 6;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(278, 24);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(67, 15);
		this.label5.TabIndex = 145;
		this.label5.Text = "Item Name";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(5, 127);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(70, 15);
		this.label4.TabIndex = 143;
		this.label4.Text = "Moisture To";
		this.txtMoistureTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMoistureTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMoistureTo.Location = new System.Drawing.Point(98, 123);
		this.txtMoistureTo.Name = "txtMoistureTo";
		this.txtMoistureTo.ShortcutsEnabled = false;
		this.txtMoistureTo.Size = new System.Drawing.Size(177, 23);
		this.txtMoistureTo.TabIndex = 4;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(5, 102);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(85, 15);
		this.label2.TabIndex = 141;
		this.label2.Text = "Moisture From";
		this.txtMoistureFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMoistureFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMoistureFrom.Location = new System.Drawing.Point(98, 98);
		this.txtMoistureFrom.Name = "txtMoistureFrom";
		this.txtMoistureFrom.ShortcutsEnabled = false;
		this.txtMoistureFrom.Size = new System.Drawing.Size(177, 23);
		this.txtMoistureFrom.TabIndex = 3;
		this.CmbParentCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbParentCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbParentCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbParentCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance35).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbParentCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Location = new System.Drawing.Point(98, 150);
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Name = "CmbParentCategory";
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Size = new System.Drawing.Size(177, 26);
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Leave += new System.EventHandler(CmbParentCategory_Leave);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(5, 154);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(91, 15);
		this.label9.TabIndex = 121;
		this.label9.Text = "Parent Category";
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(380, 46);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(208, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 7;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(278, 52);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(86, 15);
		this.label1.TabIndex = 121;
		this.label1.Text = "Supplier Name";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(5, 77);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(56, 15);
		this.label27.TabIndex = 119;
		this.label27.Text = "Order No";
		this.txtOrderNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOrderNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderNo.Location = new System.Drawing.Point(98, 73);
		this.txtOrderNo.Name = "txtOrderNo";
		this.txtOrderNo.ShortcutsEnabled = false;
		this.txtOrderNo.Size = new System.Drawing.Size(177, 23);
		this.txtOrderNo.TabIndex = 2;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(530, 151);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(58, 23);
		this.btnshow.TabIndex = 14;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 31);
		this.panel5.Name = "panel5";
		this.panel5.RightToLeft = System.Windows.Forms.RightToLeft.No;
		this.panel5.Size = new System.Drawing.Size(1118, 27);
		this.panel5.TabIndex = 21;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(4, 3);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(244, 19);
		this.label3.TabIndex = 0;
		this.label3.Text = "Purchase Analysis By Vehicle";
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1118, 31);
		this.panel2.TabIndex = 0;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNew, this.Print, this.btnShortCut });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1118, 27);
		this.toolStrip3.TabIndex = 3;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.Print.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(115, 24);
		this.Print.Text = "665-&Register";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.btnShortCut.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCut.Name = "btnShortCut";
		this.btnShortCut.Size = new System.Drawing.Size(126, 24);
		this.btnShortCut.Text = "ShortCut Keys";
		this.btnShortCut.Click += new System.EventHandler(btnShortCut_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1118, 648);
		base.Controls.Add(this.panel1);
		base.Name = "LabDataVehicleWiseByParent";
		this.Text = "LabDataVehicleWiseByParent";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(LabDataVehicleWiseByParent_FormClosing);
		base.Load += new System.EventHandler(LabDataVehicleWiseByParent_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmlabdatavehiclewise_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdLabDetail).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdLabSummary).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
