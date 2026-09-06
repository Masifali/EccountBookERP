// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.pcc;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Common;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
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
namespace Architecture.WinApp.pcc
{

public class frmConcreteProductionReport : Form
{
	private Attachment AT = new Attachment();

	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtHistory = new DataTable();

	private DataTable dtHistoryInput = new DataTable();

	private DataTable dtHistoryOutput = new DataTable();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage3;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewHistory;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton btnRegister;

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

	internal UltraCombo CmbProductionType;

	internal UltraCombo CmbPlant;

	private Label label29;

	private Button btnshow;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdHistoryOutput;

	private Panel panel12;

	private Label label14;

	private Label label15;

	private Label label1;

	private Label label2;

	private ToolStripButton BtnRefresh;

	private ToolStripButton btnshortcutkeys;

	public frmConcreteProductionReport(UserAccount user)
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
			Datetypefill();
			AllCombobind();
			cmbDateTypeHistory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllCombobind()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		try
		{
			int Idtype = Conversion.ToInt(CmbProductionType.Value);
			int Iditem = Conversion.ToInt(CmbPlant.Value);
			DataTable dtcombo = ProductionHeader.GetDataForDropDownFromProductionHeader(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtcombo.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtProductionType = new DataTable();
			dtProductionType.Columns.Add("Id", typeof(int));
			dtProductionType.Columns.Add("Name");
			DataTable dtPlant = new DataTable();
			dtPlant.Columns.Add("Id", typeof(int));
			dtPlant.Columns.Add("Name");
			for (int i = 0; i < dtcombo.Rows.Count; i++)
			{
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "ProductionType")
				{
					dtProductionType.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "Plant")
				{
					dtPlant.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
			}
			if (dtProductionType.Rows.Count > 0)
			{
				DDL.BindDDL(dtProductionType, CmbProductionType, "Id", "Name", "Production Type", ZeroIndex: true);
				if (Idtype > 0)
				{
					if (dtProductionType.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Idtype))
					{
						CmbProductionType.Value = Idtype;
					}
					else
					{
						((Control)(object)CmbProductionType).Text = string.Empty;
					}
				}
			}
			if (dtPlant.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtPlant, CmbPlant, "Id", "Name", "Plant Name", ZeroIndex: true);
			if (Iditem > 0)
			{
				if (dtPlant.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Iditem))
				{
					CmbPlant.Value = Iditem;
				}
				else
				{
					((Control)(object)CmbPlant).Text = string.Empty;
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

	private void frmConcreteProductionReport_FormClosed(object sender, FormClosedEventArgs e)
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

	private void btnHistoryRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)cmbDateTypeHistory).Text = string.Empty;
			((Control)(object)CmbPlant).Text = string.Empty;
			((Control)(object)CmbProductionType).Text = string.Empty;
			grdHistoryOutput.ClearStructure();
			grdHistoryInput.ClearStructure();
			cmbDateTypeHistory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefresh_Click(object sender, EventArgs e)
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
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		try
		{
			dtHistory.Clear();
			dtHistoryInput.Clear();
			dtHistoryOutput.Clear();
			DataSet dtSet = ProductionHeader.Production_Report(new ReportsParameters
			{
				CompanyId = UserAccount.CompanyId,
				OrganizationId = UserAccount.OrganizationId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 1850,
				FromDocNo = Conversion.ToInt((object)txtFromNoHistory.Text.Trim()),
				ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text.Trim()),
				FromDate = Conversion.ToDateTime((object)txtFromdateHistory.Value),
				ToDate = Conversion.ToDateTime((object)txtToDateHistory.Value),
				ProductionType = Conversion.ToInt(CmbProductionType.Value),
				PlantId = Conversion.ToInt(CmbPlant.Value)
			});
			if (dtSet.Tables.Count > 0)
			{
				dtHistory = dtSet.Tables[0];
				dtHistoryInput = dtSet.Tables[1];
				dtHistoryOutput = dtSet.Tables[2];
				if (dtHistory.Rows.Count > 0)
				{
					DataTable dtTarget = new DataTable();
					dtTarget.Columns.Add("ProductionTypeId", typeof(int));
					dtTarget.Columns.Add("ProductionType", typeof(string));
					dtTarget.Columns.Add("ItemId", typeof(int));
					dtTarget.Columns.Add("ItemName", typeof(string));
					dtTarget.Columns.Add("ItemCode", typeof(string));
					dtTarget.Columns.Add("ItemUomId", typeof(int));
					dtTarget.Columns.Add("ItemUom", typeof(string));
					dtTarget.Columns.Add("ItemQty", typeof(decimal));
					dtTarget.Columns.Add("TotalInputAmount", typeof(decimal));
					dtTarget.Columns.Add("TotalAutoRateOutputAmount", typeof(decimal));
					dtTarget.Columns.Add("TotalManualRateOutputAmount", typeof(decimal));
					dtTarget.Columns.Add("TotalOverHeadAmount", typeof(decimal));
					dtTarget.Columns.Add("TotalWagesAmount", typeof(decimal));
					dtTarget.Columns.Add("NetFinishAmount", typeof(decimal));
					foreach (DataRow row3 in dtHistory.Rows)
					{
						dtTarget.Rows.Add(row3["ProductionTypeId"], row3["ProductionType"], row3["ItemId"], row3["ItemName"], row3["ItemCode"], row3["ItemUomId"], row3["ItemUOM"], row3["ItemQty"], row3["TotalInputAmount"], row3["TotalAutoRateOutputAmount"], row3["TotalManualRateOutputAmount"], row3["TotalOverHeadAmount"], row3["TotalWagesAmount"], row3["NetFinishAmount"]);
					}
					DatagridHistory.DataSource = dtTarget;
					DatagridHistory.RetrieveStructure();
					HistoryGridSetting();
				}
				else
				{
					DatagridHistory.DataSource = null;
				}
				if (dtHistoryInput.Rows.Count > 0)
				{
					DataTable dt2 = new DataTable();
					dt2.Columns.Add("ItemId", typeof(int));
					dt2.Columns.Add("ItemName", typeof(string));
					dt2.Columns.Add("ItemCode", typeof(string));
					dt2.Columns.Add("ItemUomId", typeof(int));
					dt2.Columns.Add("ItemUom", typeof(string));
					dt2.Columns.Add("ItemQty", typeof(decimal));
					dt2.Columns.Add("NetWeight", typeof(decimal));
					dt2.Columns.Add("AvgRate", typeof(decimal));
					dt2.Columns.Add("Amount", typeof(decimal));
					foreach (DataRow row2 in dtHistoryInput.Rows)
					{
						dt2.Rows.Add(row2["ItemId"], row2["ItemName"], row2["ItemCode"], row2["ItemUomId"], row2["UOMCode"], row2["ItemQty"], row2["NetWeight"], row2["AvgRate"], row2["Amount"]);
					}
					grdHistoryInput.DataSource = dt2;
					grdHistoryInput.RetrieveStructure();
					grdHistoryInputSetting();
				}
				else
				{
					grdHistoryInput.ClearStructure();
				}
				if (dtHistoryOutput.Rows.Count > 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("ProductionTypeId", typeof(int));
					dt.Columns.Add("ProductionType", typeof(string));
					dt.Columns.Add("ItemId", typeof(int));
					dt.Columns.Add("ItemName", typeof(string));
					dt.Columns.Add("ItemCode", typeof(string));
					dt.Columns.Add("ItemAttributeVarientId", typeof(int));
					dt.Columns.Add("Variant", typeof(string));
					dt.Columns.Add("ItemQty", typeof(decimal));
					dt.Columns.Add("VariantQty", typeof(decimal));
					dt.Columns.Add("NetWeight", typeof(decimal));
					dt.Columns.Add("Rate/VariantQty", typeof(decimal));
					dt.Columns.Add("AvgRate", typeof(decimal));
					dt.Columns.Add("Amount", typeof(decimal));
					foreach (DataRow row in dtHistoryOutput.Rows)
					{
						dt.Rows.Add(row["ProductionTypeId"], row["ProductionType"], row["ItemId"], row["ItemName"], row["ItemCode"], row["ItemAttributeVarientId"], row["AttributeVarient"], row["ItemQty"], row["VarientQty"], row["NetWeight"], (Conversion.ToDecimal(row["VarientQty"]) > 0m) ? (Conversion.ToDecimal(row["AvgRate"]) / Conversion.ToDecimal(row["VarientQty"])) : 0m, row["AvgRate"], row["Amount"]);
					}
					grdHistoryOutput.DataSource = dt;
					grdHistoryOutput.RetrieveStructure();
					grdHistoryOutputSetting();
				}
				else
				{
					grdHistoryOutput.ClearStructure();
				}
			}
			else
			{
				DatagridHistory.DataSource = null;
				grdHistoryInput.ClearStructure();
				grdHistoryOutput.ClearStructure();
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
			DatagridHistory.RootTable.Columns["ProductionTypeId"].Visible = false;
			DatagridHistory.RootTable.Columns["ItemId"].Visible = false;
			DatagridHistory.RootTable.Columns["ItemUomId"].Visible = false;
			DatagridHistory.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			DatagridHistory.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			DatagridHistory.RootTable.Columns["ItemQty"].FormatString = "##,#.####";
			DatagridHistory.RootTable.Columns["ItemQty"].TotalFormatString = "##,#.####";
			DatagridHistory.RootTable.Columns["TotalInputAmount"].AggregateFunction = (AggregateFunction)2;
			DatagridHistory.RootTable.Columns["TotalInputAmount"].TextAlignment = (TextAlignment)3;
			DatagridHistory.RootTable.Columns["TotalInputAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DatagridHistory.RootTable.Columns["TotalInputAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DatagridHistory.RootTable.Columns["TotalAutoRateOutputAmount"].AggregateFunction = (AggregateFunction)2;
			DatagridHistory.RootTable.Columns["TotalAutoRateOutputAmount"].TextAlignment = (TextAlignment)3;
			DatagridHistory.RootTable.Columns["TotalAutoRateOutputAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DatagridHistory.RootTable.Columns["TotalAutoRateOutputAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DatagridHistory.RootTable.Columns["TotalManualRateOutputAmount"].AggregateFunction = (AggregateFunction)2;
			DatagridHistory.RootTable.Columns["TotalManualRateOutputAmount"].TextAlignment = (TextAlignment)3;
			DatagridHistory.RootTable.Columns["TotalManualRateOutputAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DatagridHistory.RootTable.Columns["TotalManualRateOutputAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DatagridHistory.RootTable.Columns["TotalOverHeadAmount"].AggregateFunction = (AggregateFunction)2;
			DatagridHistory.RootTable.Columns["TotalOverHeadAmount"].TextAlignment = (TextAlignment)3;
			DatagridHistory.RootTable.Columns["TotalOverHeadAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DatagridHistory.RootTable.Columns["TotalOverHeadAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DatagridHistory.RootTable.Columns["TotalWagesAmount"].AggregateFunction = (AggregateFunction)2;
			DatagridHistory.RootTable.Columns["TotalWagesAmount"].TextAlignment = (TextAlignment)3;
			DatagridHistory.RootTable.Columns["TotalWagesAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DatagridHistory.RootTable.Columns["TotalWagesAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DatagridHistory.RootTable.Columns["NetFinishAmount"].AggregateFunction = (AggregateFunction)2;
			DatagridHistory.RootTable.Columns["NetFinishAmount"].TextAlignment = (TextAlignment)3;
			DatagridHistory.RootTable.Columns["NetFinishAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DatagridHistory.RootTable.Columns["NetFinishAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(DatagridHistory);
			ctrlGrdBar1_Load(null, null);
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
			grdHistoryInput.RootTable.Columns["ItemId"].Visible = false;
			grdHistoryInput.RootTable.Columns["ItemUomId"].Visible = false;
			grdHistoryInput.RootTable.Columns["NetWeight"].Visible = false;
			grdHistoryInput.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryInput.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdHistoryInput.RootTable.Columns["ItemQty"].FormatString = "##,#.####";
			grdHistoryInput.RootTable.Columns["ItemQty"].TotalFormatString = "##,#.####";
			grdHistoryInput.RootTable.Columns["AvgRate"].AggregateFunction = (AggregateFunction)3;
			grdHistoryInput.RootTable.Columns["AvgRate"].TextAlignment = (TextAlignment)3;
			grdHistoryInput.RootTable.Columns["AvgRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryInput.RootTable.Columns["AvgRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
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
			grdHistoryOutput.RootTable.Columns["ProductionTypeId"].Visible = false;
			grdHistoryOutput.RootTable.Columns["ItemAttributeVarientId"].Visible = false;
			grdHistoryOutput.RootTable.Columns["ItemId"].Visible = false;
			grdHistoryOutput.RootTable.Columns["NetWeight"].Visible = false;
			grdHistoryOutput.GroupByBoxVisible = true;
			grdHistoryOutput.RootTable.Columns["ProductionType"].Visible = false;
			grdHistoryOutput.RootTable.Groups.Add("ProductionType");
			grdHistoryOutput.GroupTotals = (GroupTotals)2;
			grdHistoryOutput.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOutput.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdHistoryOutput.RootTable.Columns["ItemQty"].FormatString = "##,#.####";
			grdHistoryOutput.RootTable.Columns["ItemQty"].TotalFormatString = "##,#.####";
			grdHistoryOutput.RootTable.Columns["VariantQty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOutput.RootTable.Columns["VariantQty"].TextAlignment = (TextAlignment)3;
			grdHistoryOutput.RootTable.Columns["VariantQty"].FormatString = "##,#.####";
			grdHistoryOutput.RootTable.Columns["VariantQty"].TotalFormatString = "##,#.####";
			grdHistoryOutput.RootTable.Columns["Rate/VariantQty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOutput.RootTable.Columns["Rate/VariantQty"].TextAlignment = (TextAlignment)3;
			grdHistoryOutput.RootTable.Columns["Rate/VariantQty"].FormatString = "##,#.####";
			grdHistoryOutput.RootTable.Columns["Rate/VariantQty"].TotalFormatString = "##,#.####";
			grdHistoryOutput.RootTable.Columns["AvgRate"].AggregateFunction = (AggregateFunction)2;
			grdHistoryOutput.RootTable.Columns["AvgRate"].TextAlignment = (TextAlignment)3;
			grdHistoryOutput.RootTable.Columns["AvgRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryOutput.RootTable.Columns["AvgRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
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

	private void DatagridHistory_DoubleClick(object sender, EventArgs e)
	{
	}

	private void DatagridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Print")
			{
				CommonServices.ProductionSlipConcrete(Conversion.ToInt((object)DatagridHistory.CurrentRow.Cells["Id"].Value.ToString()));
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

	private void btnRegister_Click(object sender, EventArgs e)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtHistory == null || dtHistory.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject1 = new SubReportObject(dtHistoryInput, "Production_Input_SubReport.rpt");
			SubReportObject subReportObject2 = new SubReportObject(dtHistoryOutput, "Production_Output_SubReport.rpt");
			lstSubReports.Add(subReportObject1);
			lstSubReports.Add(subReportObject2);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dtHistory, "1850_01_Production_Report.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFromNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtToDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void frmBillOfMaterial_Manufacturing_KeyDown(object sender, KeyEventArgs e)
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
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)DatagridHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshow_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				btnRegister_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				((Control)(object)grdHistoryOutput).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)grdHistoryOutput).Focused)
				{
					((Control)(object)grdHistoryInput).Focus();
				}
				else if (((Control)(object)grdHistoryInput).Focused)
				{
					((Control)(object)grdHistoryOutput).Focus();
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
			dt.Rows.Add("Ctrl+S", "For Show Data");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Date Type");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Main Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On on Date Type");
			dt.Rows.Add("Ctrl+ArrowRight", "to change focus from one grid to another");
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
		MakeShortCutKeys();
	}

	private void DatagridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && DatagridHistory.CurrentColumn != null)
			{
				if (e.Control)
				{
					_ = e.KeyCode;
					_ = 32;
				}
				if (e.Control)
				{
					_ = e.KeyCode;
					_ = 13;
				}
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
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Expected O, but got Unknown
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Expected O, but got Unknown
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Expected O, but got Unknown
		//IL_0c00: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c0a: Expected O, but got Unknown
		//IL_0c17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c21: Expected O, but got Unknown
		GridEXLayout grdHistoryInput_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryOutput_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DatagridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
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
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel45 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdHistoryInput = new GridEX();
		this.panel44 = new System.Windows.Forms.Panel();
		this.label14 = new System.Windows.Forms.Label();
		this.panel19 = new System.Windows.Forms.Panel();
		this.grdHistoryOutput = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.DatagridHistory = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label17 = new System.Windows.Forms.Label();
		this.panel13 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtFromNoHistory = new System.Windows.Forms.TextBox();
		this.CmbProductionType = new UltraCombo();
		this.txtToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.txtFromdateHistory = new System.Windows.Forms.DateTimePicker();
		this.cmbDateTypeHistory = new UltraCombo();
		this.btnshow = new System.Windows.Forms.Button();
		this.CmbPlant = new UltraCombo();
		this.label29 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.panel38 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.BtnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnRegister = new System.Windows.Forms.ToolStripButton();
		this.label32 = new System.Windows.Forms.Label();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel45.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryInput).BeginInit();
		this.panel44.SuspendLayout();
		this.panel19.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryOutput).BeginInit();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).BeginInit();
		this.panel15.SuspendLayout();
		this.panel13.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPlant).BeginInit();
		this.panel38.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1184, 711);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1184, 711);
		this.tabControl1.TabIndex = 0;
		this.tabPage3.Controls.Add(this.panel45);
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.DatagridHistory);
		this.tabPage3.Controls.Add(this.panel15);
		this.tabPage3.Controls.Add(this.panel13);
		this.tabPage3.Controls.Add(this.panel38);
		this.tabPage3.Location = new System.Drawing.Point(4, 4);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(1176, 682);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "History";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel45.Controls.Add(this.panel8);
		this.panel45.Controls.Add(this.panel19);
		this.panel45.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel45.Location = new System.Drawing.Point(0, 343);
		this.panel45.Name = "panel45";
		this.panel45.Size = new System.Drawing.Size(1176, 339);
		this.panel45.TabIndex = 55;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryInput);
		this.panel8.Controls.Add(this.panel44);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(645, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(531, 339);
		this.panel8.TabIndex = 50;
		this.grdHistoryInput.AllowEdit = (InheritableBoolean)2;
		grdHistoryInput_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryInput.DesignTimeLayout = grdHistoryInput_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryInput.DynamicFiltering = true;
		this.grdHistoryInput.FilterMode = (FilterMode)2;
		this.grdHistoryInput.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryInput.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).Location = new System.Drawing.Point(0, 19);
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).Name = "grdHistoryInput";
		((ScrollableBase)this.grdHistoryInput).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).Size = new System.Drawing.Size(531, 320);
		((System.Windows.Forms.Control)(object)this.grdHistoryInput).TabIndex = 0;
		this.grdHistoryInput.TotalRow = (InheritableBoolean)1;
		this.grdHistoryInput.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryInput).VisualStyle = (VisualStyle)4;
		this.panel44.BackColor = System.Drawing.Color.Teal;
		this.panel44.Controls.Add(this.label14);
		this.panel44.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel44.Location = new System.Drawing.Point(0, 0);
		this.panel44.Name = "panel44";
		this.panel44.Size = new System.Drawing.Size(531, 19);
		this.panel44.TabIndex = 49;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(2, 1);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(223, 16);
		this.label14.TabIndex = 128;
		this.label14.Text = "Input Material (Consumed Items)";
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryOutput);
		this.panel19.Controls.Add(this.panel12);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel19.Location = new System.Drawing.Point(0, 0);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(645, 339);
		this.panel19.TabIndex = 4;
		this.grdHistoryOutput.AllowEdit = (InheritableBoolean)2;
		grdHistoryOutput_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryOutput.DesignTimeLayout = grdHistoryOutput_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryOutput.DynamicFiltering = true;
		this.grdHistoryOutput.FilterMode = (FilterMode)2;
		this.grdHistoryOutput.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryOutput.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Location = new System.Drawing.Point(0, 19);
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Name = "grdHistoryOutput";
		((ScrollableBase)this.grdHistoryOutput).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryOutput).Size = new System.Drawing.Size(645, 320);
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
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Location = new System.Drawing.Point(0, 127);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Name = "DatagridHistory";
		((ScrollableBase)this.DatagridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Size = new System.Drawing.Size(1176, 216);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).TabIndex = 0;
		this.DatagridHistory.TotalRow = (InheritableBoolean)1;
		this.DatagridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DatagridHistory).VisualStyle = (VisualStyle)4;
		this.DatagridHistory.ColumnButtonClick += new ColumnActionEventHandler(DatagridHistory_ColumnButtonClick);
		this.DatagridHistory.LinkClicked += new ColumnActionEventHandler(DatagridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).DoubleClick += new System.EventHandler(DatagridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DatagridHistory_KeyDown);
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.ctrlGrdBar1);
		this.panel15.Controls.Add(this.label17);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 104);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1176, 23);
		this.panel15.TabIndex = 53;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1129, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(9, 6, 9, 6);
		this.ctrlGrdBar1.MyGrid = this.DatagridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(42, 22);
		this.ctrlGrdBar1.TabIndex = 128;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(8, 3);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(38, 16);
		this.label17.TabIndex = 127;
		this.label17.Text = "Main";
		this.panel13.BackColor = System.Drawing.Color.White;
		this.panel13.Controls.Add(this.groupBox1);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 51);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1176, 53);
		this.panel13.TabIndex = 0;
		this.groupBox1.Controls.Add(this.txtFromNoHistory);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbProductionType);
		this.groupBox1.Controls.Add(this.txtToDateHistory);
		this.groupBox1.Controls.Add(this.txtFromdateHistory);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPlant);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.txtToDocNoHistory);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(2, 1);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1172, 47);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.txtFromNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromNoHistory.Location = new System.Drawing.Point(573, 18);
		this.txtFromNoHistory.Name = "txtFromNoHistory";
		this.txtFromNoHistory.Size = new System.Drawing.Size(55, 23);
		this.txtFromNoHistory.TabIndex = 3;
		this.txtFromNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromNoHistory_KeyPress);
		this.CmbProductionType.AllowNull = (DefaultableBoolean)1;
		this.CmbProductionType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbProductionType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbProductionType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance135;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance104).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbProductionType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbProductionType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Location = new System.Drawing.Point(823, 16);
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Name = "CmbProductionType";
		((System.Windows.Forms.Control)(object)this.CmbProductionType).Size = new System.Drawing.Size(121, 26);
		((System.Windows.Forms.Control)(object)this.CmbProductionType).TabIndex = 5;
		this.txtToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.CustomFormat = "dd-MMM-yy";
		this.txtToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateHistory.Location = new System.Drawing.Point(401, 18);
		this.txtToDateHistory.Name = "txtToDateHistory";
		this.txtToDateHistory.Size = new System.Drawing.Size(111, 23);
		this.txtToDateHistory.TabIndex = 2;
		this.txtFromdateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.CustomFormat = "dd-MMM-yy";
		this.txtFromdateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromdateHistory.Location = new System.Drawing.Point(244, 18);
		this.txtFromdateHistory.Name = "txtFromdateHistory";
		this.txtFromdateHistory.Size = new System.Drawing.Size(111, 23);
		this.txtFromdateHistory.TabIndex = 1;
		this.cmbDateTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbDateTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance111;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance116).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Location = new System.Drawing.Point(62, 16);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Name = "cmbDateTypeHistory";
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Size = new System.Drawing.Size(120, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).TabIndex = 0;
		this.cmbDateTypeHistory.ValueChanged += new System.EventHandler(cmbDateTypeHistory_ValueChanged);
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(1110, 17);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(56, 25);
		this.btnshow.TabIndex = 7;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.CmbPlant.AllowNull = (DefaultableBoolean)1;
		this.CmbPlant.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPlant.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPlant.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPlant).DisplayLayout.Appearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbPlant).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPlant).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlant).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlant).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance123;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlant).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlant).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbPlant).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPlant).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPlant).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPlant).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbPlant).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPlant).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlant).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance128).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPlant).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbPlant).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPlant).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlant).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPlant).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbPlant).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPlant).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPlant).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbPlant).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPlant).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbPlant).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPlant).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPlant).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPlant).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPlant.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPlant).Location = new System.Drawing.Point(985, 16);
		((System.Windows.Forms.Control)(object)this.CmbPlant).Name = "CmbPlant";
		((System.Windows.Forms.Control)(object)this.CmbPlant).Size = new System.Drawing.Size(122, 26);
		((System.Windows.Forms.Control)(object)this.CmbPlant).TabIndex = 6;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(948, 22);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(34, 15);
		this.label29.TabIndex = 180;
		this.label29.Text = "Plant";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(733, 22);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(91, 15);
		this.label30.TabIndex = 178;
		this.label30.Text = "ProductionType";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(3, 22);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(60, 15);
		this.label19.TabIndex = 174;
		this.label19.Text = "Date Type";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(357, 22);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(48, 15);
		this.label1.TabIndex = 171;
		this.label1.Text = "Date To";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(184, 22);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(63, 15);
		this.label28.TabIndex = 171;
		this.label28.Text = "Date From";
		this.txtToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(676, 18);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.Size = new System.Drawing.Size(55, 23);
		this.txtToDocNoHistory.TabIndex = 4;
		this.txtToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoHistory_KeyPress);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(629, 22);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(45, 15);
		this.label2.TabIndex = 173;
		this.label2.Text = "Doc To";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(514, 22);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(60, 15);
		this.label35.TabIndex = 173;
		this.label35.Text = "Doc From";
		this.panel38.BackColor = System.Drawing.Color.Teal;
		this.panel38.Controls.Add(this.toolStrip2);
		this.panel38.Controls.Add(this.label32);
		this.panel38.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel38.Location = new System.Drawing.Point(0, 0);
		this.panel38.Name = "panel38";
		this.panel38.Size = new System.Drawing.Size(1176, 51);
		this.panel38.TabIndex = 48;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnNewHistory, this.BtnRefresh, this.toolStripSeparator1, this.btnRegister, this.btnshortcutkeys });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1176, 27);
		this.toolStrip2.TabIndex = 3;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(60, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnHistoryRefresh_Click);
		this.BtnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefresh.Name = "BtnRefresh";
		this.BtnRefresh.Size = new System.Drawing.Size(81, 24);
		this.BtnRefresh.Text = "&Refresh";
		this.BtnRefresh.Click += new System.EventHandler(BtnRefresh_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnRegister.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRegister.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRegister.Name = "btnRegister";
		this.btnRegister.Size = new System.Drawing.Size(125, 24);
		this.btnRegister.Text = "1850_01-&Print";
		this.btnRegister.Click += new System.EventHandler(btnRegister_Click);
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.White;
		this.label32.Location = new System.Drawing.Point(3, 29);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(158, 19);
		this.label32.TabIndex = 127;
		this.label32.Text = "Production Report";
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(127, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1184, 711);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmConcreteProductionReport";
		this.Text = "Concrete Production";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmConcreteProductionReport_FormClosed);
		base.Load += new System.EventHandler(frmBillOfMaterial_Manufacturing_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmBillOfMaterial_Manufacturing_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		this.panel45.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryInput).EndInit();
		this.panel44.ResumeLayout(false);
		this.panel44.PerformLayout();
		this.panel19.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryOutput).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel13.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPlant).EndInit();
		this.panel38.ResumeLayout(false);
		this.panel38.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
