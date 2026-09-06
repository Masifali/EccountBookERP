// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.BLL.Pos;
using Architecture.BLL.Production;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.Model.POS;
using Architecture.WinApp.Common;
using Architecture.WinApp.Inventory_Definition;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;
using GroupTotals = Janus.Windows.GridEX.GroupTotals;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.WholeSale
{

public class frmItemSalesPricingSchedule : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtItem = new DataTable();

	private DataTable dtGrid = new DataTable();

	private DataTable dtForFinalPrice = new DataTable();

	private DataTable OriginalFinalPriceData = new DataTable();

	private Rightsobjects formrights = new Rightsobjects();

	private int priceTypeId;

	private int[] _AryColWidth;

	private int RecId;

	private double RoundingFactor;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private Panel panel5;

	private Panel panel4;

	private Panel panel3;

	private TabPage tabPage2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripSeparator toolStripSeparator;

	private Panel panel6;

	private Label label15;

	private Panel panel7;

	private TextBox txtDescription;

	private Label label9;

	internal UltraCombo cmbItem;

	private Label label3;

	private Label label10;

	private DateTimePicker datEffectedDate;

	private Button btnAdd;

	internal GridEX grd;

	private Panel panel9;

	internal GridEX DataGridHistory;

	private DateTimePicker datEffectedTime;

	internal UltraCombo cmbItemCategory;

	internal UltraCombo cmbItemType;

	private Label label4;

	private TextBox txtMarginPercent;

	private Label label5;

	private TextBox txtPackingUom;

	private Panel panel11;

	private Panel panel10;

	internal GridEX grdFinal;

	private ToolStripButton btnAllocateBrandtoDiscount;

	private Panel panel8;

	private ToolStrip toolStrip2;

	private ToolStripButton btnPrint;

	private Panel panel12;

	private Button BtnShowHistoryReport;

	private DateTimePicker txtEffectedDateForHistory;

	private Label label6;

	private Label label2;

	private Panel panel13;

	internal GridEX GrdForFinalPrice;

	private Panel panel17;

	private Panel panel16;

	private Label label8;

	private Panel panel15;

	private Panel panel14;

	private Label label7;

	private Panel panel18;

	private Panel panel20;

	private Panel panel24;

	private Panel panel22;

	private Label label12;

	private Panel panel19;

	private Panel panel23;

	private Panel panel21;

	private Label label11;

	private Label label13;

	internal UltraCombo cmbDiscType;

	private ToolStripButton btnPrint324A;

	private Label label24;

	internal UltraCombo CmbCropYear;

	private Label label1;

	internal UltraCombo CmbCropYearHistory;

	private Label label14;

	internal UltraCombo CmbItemNameHistory;

	private Button BtnUpdateHis;

	public frmItemSalesPricingSchedule(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmItemPricingSchedule_Load(object sender, EventArgs e)
	{
		try
		{
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formrights.DoHaveSaveRight;
			btnUpdate.Enabled = formrights.DoHaveUpdateRights;
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "RoundingForItemPricing").ToList();
			if (list.Count > 0)
			{
				RoundingFactor = Conversion.ToDouble((object)list[0].ConfigKey.ToString());
			}
			if (Conversion.ToString(base.Tag).Trim() == "frmPosRetailPricing")
			{
				priceTypeId = 7;
			}
			else
			{
				priceTypeId = 6;
			}
			FillDiscountTypes();
			ItemFill();
			BindCropYear();
			dtGrid.Columns.Add("Id");
			dtGrid.Columns.Add("BrandId");
			dtGrid.Columns.Add("BrandName");
			dtGrid.Columns.Add("BrandCropYearId", typeof(int));
			dtGrid.Columns.Add("BrandCropYear");
			dtGrid.Columns.Add("ItemId");
			dtGrid.Columns.Add("ItemName");
			dtGrid.Columns.Add("CropYearId", typeof(double));
			dtGrid.Columns.Add("CropYear", typeof(string));
			dtGrid.Columns.Add("PreviousRate", typeof(double));
			dtGrid.Columns.Add("ItemRate", typeof(double));
			dtGrid.Columns.Add("RateChangeable");
			dtGrid.Columns.Add("Percent");
			dtGrid.Columns.Add("Prcnt", typeof(double));
			dtGrid.Columns.Add("PercentRate", typeof(double));
			dtGrid.Columns.Add("Expenses", typeof(double));
			dtGrid.Columns.Add("PackingRate", typeof(double));
			dtGrid.Columns.Add("SemiEffectedDate", typeof(DateTime));
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			GridSetting();
			BindCropYearForHistory();
			BindItemsForHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemFill()
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		try
		{
			dtItem.Rows.Clear();
			((Control)(object)cmbItem).Text = string.Empty;
			((UltraGridBase)cmbItem).DataSource = null;
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DiscountTypeId = Conversion.ToInt(cmbDiscType.Value);
			dtItem = ItemPricingSchedule.GetItemsFromRecipe(obj);
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDL(dtItem, cmbItem, "Id", "ItemName", "Item Name", ZeroIndex: false);
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 400;
				UltraGridColumn obj2 = ((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj2).Key = "Selected";
				((HeaderBase)obj2.Header).Caption = string.Empty;
				obj2.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj2.DataType = typeof(bool);
				((HeaderBase)obj2.Header).VisiblePosition = 0;
				cmbItem.DropDownStyle = (UltraComboStyle)1;
				((UltraGridBase)cmbItem).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				cmbItem.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)cmbItem.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)cmbItem.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)cmbItem.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDiscountTypes()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = InvAllocateDiscItemToDiscType.GetDiscountTypes(new InvAllocateDiscItemToDiscType
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbDiscType, "Id", "DiscountTypeCode", "Discount Type", false);
				((UltraGridBase)cmbDiscType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindCropYear()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			DataTable dt = InvManufacturingBOMHeader.GetDataForDropDownFromInvManufacturingBOMHeader(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Activity = "CropYear"
			});
			if (dt != null && dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbCropYear, "Id", "ReferenceName", "Crop Year", ZeroIndex: false);
				((UltraGridBase)CmbCropYear).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindCropYearForHistory()
	{
		try
		{
			DataTable dt = ItemPricingSchedule.getCropYearFromPricingSchedule(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (dt != null && dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbCropYearHistory, "Id", "CropYear", "Crop Year", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindItemsForHistory()
	{
		try
		{
			DataTable dt = ItemPricingSchedule.getItemsFromPricingSchedule(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (dt != null && dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbItemNameHistory, "Id", "ItemName", "Item Name", ZeroIndex: false);
			}
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
			datEffectedDate.Enabled = true;
			datEffectedTime.Enabled = true;
			((Control)(object)cmbDiscType).Text = string.Empty;
			((Control)(object)cmbItem).Text = string.Empty;
			((Control)(object)cmbItemCategory).Text = string.Empty;
			((Control)(object)cmbItemType).Text = string.Empty;
			txtDescription.Text = string.Empty;
			txtMarginPercent.Text = string.Empty;
			datEffectedDate.Value = DateTime.Now;
			dtGrid.Rows.Clear();
			grdFinal.ClearStructure();
			grd.ClearStructure();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			RecId = 0;
			btnAdd.Enabled = true;
			ItemFill();
			cmbItem.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
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

	private void btnsave_Click(object sender, EventArgs e)
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

	private void Insert()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Expected O, but got Unknown
		try
		{
			ItemPricingSchedule obj = new ItemPricingSchedule();
			if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			obj.SalesPricingScheduleBreakupsList = new List<SalesPricingScheduleBreakup>();
			obj.ItemPricingScheduleslist = new List<ItemPricingSchedule>();
			GridEXRow[] rows = grdFinal.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				ItemPricingSchedule obj3 = new ItemPricingSchedule();
				obj3.CompanyId = UserAccount.CompanyId;
				obj3.OrganizationId = UserAccount.OrganizationId;
				obj3.BranchesId = UserAccount.BranchesId;
				obj3.EntryUser = UserAccount.ID;
				obj3.ModifyUser = UserAccount.ID;
				obj3.EntryDate = DateTime.Now;
				obj3.ModifyDate = DateTime.Now;
				obj3.EffectedDate = Conversion.ToDateTime((object)datEffectedDate.Value);
				string EffDate = Conversion.ToString((object)obj3.EffectedDate.ToString("dd/MMM/yyyy"));
				obj3.EffectedDate = Convert.ToDateTime(EffDate + " " + datEffectedTime.Value.TimeOfDay.ToString());
				obj3.Margin = Conversion.ToDouble((object)txtMarginPercent.Text);
				obj3.PackingUom = Conversion.ToInt((object)txtPackingUom.Text);
				if (Conversion.ToInt(r2.Cells["BrandId"].Value) == 0)
				{
					MessageBox.Show("Item Required");
					return;
				}
				obj3.ItemId = Conversion.ToInt(r2.Cells["BrandId"].Value);
				obj3.CropYearId = Conversion.ToInt(r2.Cells["CropYearId"].Value);
				obj3.ItemPrice = Conversion.ToDouble(r2.Cells["FinalRate"].Value);
				obj3.PackingRate = Conversion.ToDouble(r2.Cells["PackingRate"].Value);
				obj3.ScheduleDesc = Conversion.ToString((object)txtDescription.Text);
				obj3.ChangedPriceLookUpId = priceTypeId;
				obj3.PriceTypeId = priceTypeId;
				obj.ItemPricingScheduleslist.Add(obj3);
			}
			rows = grd.GetDataRows();
			foreach (GridEXRow r in rows)
			{
				SalesPricingScheduleBreakup obj2 = new SalesPricingScheduleBreakup();
				obj2.CompanyId = UserAccount.CompanyId;
				obj2.OrganizationId = UserAccount.OrganizationId;
				obj2.ScheduleDescription = txtDescription.Text;
				obj2.BrandEffectedDate = Conversion.ToDateTime((object)datEffectedDate.Value);
				obj2.DiscountTypeId = Conversion.ToInt((object)((Control)(object)cmbDiscType).Text);
				obj2.MarginPercent = Conversion.ToDecimal((object)txtMarginPercent.Text);
				obj2.RateUom = Conversion.ToDecimal((object)txtPackingUom.Text);
				obj2.BrandEffectedDate = Conversion.ToDateTime((object)datEffectedDate.Value);
				obj2.SemiEffectedDate = Conversion.ToDateTime(r.Cells["SemiEffectedDate"].Value);
				obj2.RateChangeable = Conversion.ToBool(r.Cells["RateChangeable"].Value);
				obj2.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				obj2.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
				obj2.BrandCropYearId = Conversion.ToInt(r.Cells["BrandCropYearId"].Value);
				obj2.BrandId = Conversion.ToInt(r.Cells["BrandId"].Value);
				obj2.BrandRate = Conversion.ToDecimal(r.Cells["PackingRate"].Value);
				obj2.SemiRate = Conversion.ToDecimal(r.Cells["ItemRate"].Value);
				obj2.PackingRate = Conversion.ToDecimal(r.Cells["PackingRate"].Value);
				obj2.PercentRate = Conversion.ToDecimal(r.Cells["PercentRate"].Value);
				obj2.Percent = Conversion.ToDecimal(r.Cells["Prcnt"].Value);
				obj2.Expenses = Conversion.ToDecimal(r.Cells["Expenses"].Value);
				obj.SalesPricingScheduleBreakupsList.Add(obj2);
			}
			ItemPricingSchedule.Save(obj);
			if (RecId > 0)
			{
				MessageBox.Show("Update Successfully");
			}
			else
			{
				MessageBox.Show("Save Successfully");
			}
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void frmItemPricingSchedule_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (btnsave.Visible && btnsave.Enabled && e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					btnnew_Click(null, null);
				}
				else
				{
					btnnew_Click(null, null);
				}
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
			if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				btnUpdate_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (e.Column.Key == "Print")
			{
				DataTable dt = ItemPricingSchedule.FinalItemSalePriceListSlip(new ItemPricingSchedule
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					EffectedDate = Conversion.ToDateTime(item.Cells["EffectedDate"].Value)
				});
				if (dt.Rows.Count == 0 || dt == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "324-SalesItemPricingList.rpt");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridHistoySetting()
	{
		try
		{
			DataGridHistory.ColumnAutoResize = true;
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["BrandId"].Visible = false;
			DataGridHistory.RootTable.Columns["ItemId"].Visible = false;
			DataGridHistory.RootTable.Columns["PreviousRate"].Visible = false;
			DataGridHistory.RootTable.Columns["RateChangeable"].Visible = false;
			DataGridHistory.RootTable.Columns["BrandCropYearId"].Visible = false;
			DataGridHistory.RootTable.HeaderLines = 2;
			DataGridHistory.RootTable.Columns["ItemName"].Width = 250;
			DataGridHistory.RootTable.Columns["ItemRate"].Width = 120;
			DataGridHistory.RootTable.Columns["Percent"].Width = 80;
			DataGridHistory.RootTable.Columns["PercentRate"].Width = 100;
			DataGridHistory.RootTable.Columns["Expenses"].Width = 100;
			DataGridHistory.RootTable.Columns["PackingRate"].Width = 120;
			DataGridHistory.RootTable.Columns["PreviousRate"].Width = 120;
			DataGridHistory.RootTable.Columns["PackingRate"].Caption = "Rate " + Conversion.ToString((object)txtPackingUom.Text.Trim()) + " KG";
			DataGridHistory.RootTable.Columns["Expenses"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["ItemRate"].HeaderAlignment = (TextAlignment)2;
			DataGridHistory.RootTable.Columns["PercentRate"].HeaderAlignment = (TextAlignment)2;
			DataGridHistory.RootTable.Columns["PackingRate"].HeaderAlignment = (TextAlignment)2;
			DataGridHistory.RootTable.Columns["PreviousRate"].HeaderAlignment = (TextAlignment)2;
			DataGridHistory.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["PercentRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["Expenses"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["PackingRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["PreviousRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ItemRate"].FormatString = "#,#";
			DataGridHistory.RootTable.Columns["PercentRate"].FormatString = "#,#";
			DataGridHistory.RootTable.Columns["Expenses"].FormatString = "#,#";
			DataGridHistory.RootTable.Columns["PackingRate"].FormatString = "#,#";
			DataGridHistory.RootTable.Columns["PreviousRate"].FormatString = "#,#";
			DataGridHistory.RootTable.Columns["ItemRate"].TotalFormatString = "#,#";
			DataGridHistory.RootTable.Columns["PercentRate"].TotalFormatString = "#,#";
			DataGridHistory.RootTable.Columns["Expenses"].TotalFormatString = "#,#";
			DataGridHistory.RootTable.Columns["PackingRate"].TotalFormatString = "#,#.#";
			DataGridHistory.RootTable.Columns["PreviousRate"].TotalFormatString = "#,#";
			DataGridHistory.RootTable.Groups.Add("BrandName");
			DataGridHistory.RootTable.Columns["BrandName"].Visible = false;
			DataGridHistory.RootTable.GroupTotals = (GroupTotals)2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnAdd_Click(object sender, EventArgs e)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		try
		{
			datEffectedDate.Enabled = false;
			datEffectedTime.Enabled = false;
			if (Conversion.ToDouble((object)txtPackingUom.Text) == 0.0)
			{
				throw new Exception("Packing Uom Field Required");
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.EffectedDate = Conversion.ToDateTime((object)datEffectedDate.Value);
			obj.DiscountTypeId = Conversion.ToInt(cmbDiscType.Value);
			obj.CropYearId = Conversion.ToInt(CmbCropYear.Value);
			string ItemIds = null;
			if (Conversion.ToString((object)((Control)(object)cmbItem).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbItem).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtItem.Select("ItemName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						ItemIds = ItemIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				obj.ItemIds = ItemIds;
			}
			dtGrid.Rows.Clear();
			grdFinal.ClearStructure();
			DataTable dt = ItemPricingScheduleForRice.GetRecipsForFinalList(obj);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					double PackingRate = Conversion.ToDouble(dt.Rows[i]["PercentRate"]) / 40.0 * Conversion.ToDouble((object)txtPackingUom.Text);
					dtGrid.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["BrandId"], dt.Rows[i]["BrandName"], dt.Rows[i]["BrandCropYearId"], dt.Rows[i]["BrandCropYear"], dt.Rows[i]["sItemId"], dt.Rows[i]["ItemName"], dt.Rows[i]["CropYearId"], dt.Rows[i]["CropYear"], dt.Rows[i]["PreviousRate"], dt.Rows[i]["ItemRate"], dt.Rows[i]["RateChangeable"], dt.Rows[i]["Percentage"], dt.Rows[i]["Prcnt"], dt.Rows[i]["PercentRate"], dt.Rows[i]["Expenses"], PackingRate + Conversion.ToDouble(dt.Rows[i]["Expenses"]), dt.Rows[i]["SemiEffectedDate"]);
				}
				grd.DataSource = dtGrid;
				grd.RetrieveStructure();
				GridSetting();
				FinalAmountCalculate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSetting()
	{
		//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0514: Unknown result type (might be due to invalid IL or missing references)
		//IL_051a: Expected O, but got Unknown
		try
		{
			grd.ColumnAutoResize = true;
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["BrandCropYearId"].Visible = false;
			grd.RootTable.Columns["BrandCropYear"].Visible = false;
			grd.RootTable.Columns["BrandId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["PreviousRate"].Visible = false;
			grd.RootTable.Columns["RateChangeable"].Visible = false;
			grd.RootTable.Columns["SemiEffectedDate"].Visible = false;
			grd.RootTable.Columns["CropYearId"].Visible = false;
			grd.RootTable.Columns["Prcnt"].Visible = false;
			grd.RootTable.Columns["ItemName"].Width = 250;
			grd.RootTable.Columns["ItemRate"].Width = 120;
			grd.RootTable.Columns["Percent"].Width = 60;
			grd.RootTable.Columns["PercentRate"].Width = 120;
			grd.RootTable.Columns["Expenses"].Width = 100;
			grd.RootTable.Columns["PackingRate"].Width = 120;
			grd.RootTable.Columns["PreviousRate"].Width = 120;
			grd.RootTable.Columns["PackingRate"].Caption = "Rate " + Conversion.ToString((object)txtPackingUom.Text.Trim()) + " KG";
			grd.RootTable.Columns["Expenses"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["PercentRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Expenses"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["PackingRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["PreviousRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemRate"].FormatString = "#,#";
			grd.RootTable.Columns["PercentRate"].FormatString = "#,#";
			grd.RootTable.Columns["Expenses"].FormatString = "#,#";
			grd.RootTable.Columns["PackingRate"].FormatString = "#,#";
			grd.RootTable.Columns["PreviousRate"].FormatString = "#,#";
			grd.RootTable.Columns["ItemRate"].TotalFormatString = "#,#";
			grd.RootTable.Columns["PercentRate"].TotalFormatString = "#,#";
			grd.RootTable.Columns["Expenses"].TotalFormatString = "#,#";
			grd.RootTable.Columns["PackingRate"].TotalFormatString = "#,#.#";
			grd.RootTable.Columns["PreviousRate"].TotalFormatString = "#,#";
			grd.RootTable.Groups.Add("BrandName");
			grd.RootTable.Columns["BrandName"].Visible = false;
			grd.RootTable.GroupTotals = (GroupTotals)2;
			new GridEXColumn();
			GridEXFormatCondition fc = new GridEXFormatCondition(grd.RootTable.Columns["RateChangeable"], (ConditionOperator)0, (object)1);
			fc.FormatStyle.ForeColor = Color.Red;
			grd.RootTable.FormatConditions.Add(fc);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FinalGridFill()
	{
		try
		{
			if (grd.GetRows().Length == 0)
			{
				return;
			}
			DataTable dtFinal = new DataTable();
			dtFinal.Columns.Add("BrandId");
			dtFinal.Columns.Add("BrandName");
			dtFinal.Columns.Add("CropYearId");
			dtFinal.Columns.Add("CropYear");
			dtFinal.Columns.Add("PackingRate", typeof(double));
			dtFinal.Columns.Add("FinalRate", typeof(double));
			int BrndId = 0;
			int BrandCropId = 0;
			int bCropId = 0;
			int bItemId = 0;
			double RoundingFactorHalf = Math.Round(RoundingFactor / 2.0, 2);
			GridEXRow[] dataRows = grd.GetDataRows();
			foreach (GridEXRow item in dataRows)
			{
				double FinalPrice = 0.0;
				bItemId = Conversion.ToInt(item.Cells["BrandId"].Value);
				bCropId = Conversion.ToInt(item.Cells["BrandCropYearId"].Value);
				if (BrndId == 0 && BrandCropId == 0)
				{
					EnumerableRowCollection<DataRow> rows = from r in dtGrid.AsEnumerable()
						where r.Field<string>("BrandId") == Conversion.ToString(item.Cells["BrandId"].Value) && r.Field<int>("BrandCropYearId") == Conversion.ToInt(item.Cells["BrandCropYearId"].Value)
						select r;
					FinalPrice = (rows.Any() ? rows.CopyToDataTable() : dtGrid.Clone()).AsEnumerable().Sum((DataRow row) => row.Field<double>("PackingRate"));
					double LetestRate = 0.0;
					double num = FinalPrice % 100.0;
					double lastTwoDigCompare = Math.Round(num / RoundingFactor, 0) * RoundingFactor;
					double lastTwoDigdiff = num - lastTwoDigCompare;
					LetestRate = ((!(lastTwoDigdiff > 0.0)) ? UltimateRoundingFunctionCeiling(FinalPrice, RoundingFactor, 0.0) : ((!(lastTwoDigdiff >= RoundingFactorHalf)) ? UltimateRoundingFunctionFloor(FinalPrice, RoundingFactor, 0.0) : UltimateRoundingFunctionCeiling(FinalPrice, RoundingFactor, 0.0)));
					BrndId = Conversion.ToInt(item.Cells["BrandId"].Value);
					BrandCropId = Conversion.ToInt(item.Cells["BrandCropYearId"].Value);
					dtFinal.Rows.Add(item.Cells["BrandId"].Value, item.Cells["BrandName"].Value, item.Cells["BrandCropYearId"].Value, item.Cells["BrandCropYear"].Value, FinalPrice, LetestRate);
				}
				else if (BrndId != bItemId || BrandCropId != bCropId)
				{
					EnumerableRowCollection<DataRow> rows2 = from r in dtGrid.AsEnumerable()
						where r.Field<string>("BrandId") == Conversion.ToString(item.Cells["BrandId"].Value) && r.Field<int>("BrandCropYearId") == Conversion.ToInt(item.Cells["BrandCropYearId"].Value)
						select r;
					FinalPrice = (rows2.Any() ? rows2.CopyToDataTable() : dtGrid.Clone()).AsEnumerable().Sum((DataRow row) => row.Field<double>("PackingRate"));
					double LetestRate2 = 0.0;
					double num2 = FinalPrice % 100.0;
					double lastTwoDigCompare2 = Math.Round(num2 / RoundingFactor, 0) * RoundingFactor;
					double lastTwoDigdiff2 = num2 - lastTwoDigCompare2;
					LetestRate2 = ((!(lastTwoDigdiff2 > 0.0)) ? UltimateRoundingFunctionCeiling(FinalPrice, RoundingFactor, 0.0) : ((!(lastTwoDigdiff2 >= RoundingFactorHalf)) ? UltimateRoundingFunctionFloor(FinalPrice, RoundingFactor, 0.0) : UltimateRoundingFunctionCeiling(FinalPrice, RoundingFactor, 0.0)));
					dtFinal.Rows.Add(item.Cells["BrandId"].Value, item.Cells["BrandName"].Value, item.Cells["BrandCropYearId"].Value, item.Cells["BrandCropYear"].Value, FinalPrice, LetestRate2);
					BrndId = Conversion.ToInt(item.Cells["BrandId"].Value);
					BrandCropId = Conversion.ToInt(item.Cells["BrandCropYearId"].Value);
				}
			}
			grdFinal.ColumnAutoResize = true;
			grdFinal.DataSource = dtFinal;
			grdFinal.RetrieveStructure();
			grdFinal.RootTable.Columns["CropYearId"].Visible = false;
			grdFinal.RootTable.Columns["BrandId"].Visible = false;
			grdFinal.RootTable.Columns["PackingRate"].Visible = false;
			grdFinal.RootTable.Columns["BrandName"].Width = 200;
			grdFinal.RootTable.Columns["CropYear"].Width = 70;
			grdFinal.RootTable.Columns["FinalRate"].Width = 120;
			grdFinal.RootTable.Columns["FinalRate"].Caption = "Base Rate";
			grdFinal.RootTable.Columns["FinalRate"].TextAlignment = (TextAlignment)3;
			grdFinal.RootTable.Columns["FinalRate"].FormatString = "#,#";
			grdFinal.RootTable.Columns["PackingRate"].TextAlignment = (TextAlignment)3;
			grdFinal.RootTable.Columns["PackingRate"].FormatString = "#,#.#";
			grdFinal.RootTable.Columns["PackingRate"].Caption = "Rate " + Conversion.ToString((object)txtPackingUom.Text.Trim()) + " KG";
			CommonServices.GridAutoAdjustment(_AryColWidth, grd);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPackingUom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtMarginPercent_KeyPress(object sender, KeyPressEventArgs e)
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

	private void FinalAmountCalculate()
	{
		try
		{
			if (grd.GetRows().Length != 0)
			{
				GridEXRow[] dataRows = grd.GetDataRows();
				foreach (GridEXRow obj in dataRows)
				{
					obj.BeginEdit();
					double PackingUom = Conversion.ToDouble((object)txtPackingUom.Text.Trim());
					double PrecentRate = Conversion.ToDouble(obj.Cells["PercentRate"].Value);
					double Expenses = Conversion.ToDouble(obj.Cells["Expenses"].Value);
					double SemiNetRate = PrecentRate / 40.0 * PackingUom + Expenses;
					double Margin = Conversion.ToDouble((object)txtMarginPercent.Text.Trim());
					if (Margin > 100.0)
					{
						txtMarginPercent.Text = "0";
						throw new Exception("Margin cannot be greater than 100% please check");
					}
					double MarginValue = SemiNetRate;
					if (Margin > 0.0)
					{
						double Formula = Conversion.ToDouble((object)("0." + (100.0 - Margin)));
						MarginValue = SemiNetRate / Formula;
					}
					obj.Cells["PackingRate"].Value = MarginValue.ToString();
					obj.EndEdit();
				}
				FinalGridFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		grd.UpdateData();
	}

	private void cmbItem_TextChanged(object sender, EventArgs e)
	{
	}

	private void txtMarginPercent_TextChanged(object sender, EventArgs e)
	{
		try
		{
			FinalAmountCalculate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private double UltimateRoundingFunctionFloor(double amountToRound, double nearstOf, double fairness)
	{
		return Math.Floor(amountToRound / nearstOf + fairness) * nearstOf;
	}

	private double UltimateRoundingFunctionCeiling(double amountToRound, double nearstOf, double fairness)
	{
		return Math.Ceiling(amountToRound / nearstOf + fairness) * nearstOf;
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = ItemPricingSchedule.FinalItemSalePriceListSlip(new ItemPricingSchedule
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				EffectedDate = txtEffectedDateForHistory.Value,
				CropYearId = Conversion.ToInt(CmbCropYearHistory.Value),
				ItemId = Conversion.ToInt(CmbItemNameHistory.Value)
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "324-SalesItemPricingList.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnShowHistoryReport_Click(object sender, EventArgs e)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_08dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e8: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			OriginalFinalPriceData.Clear();
			ItemPricingSchedule obj = new ItemPricingSchedule();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.EffectedDate = txtEffectedDateForHistory.Value;
			obj.CropYearId = Conversion.ToInt(CmbCropYearHistory.Value);
			obj.ItemId = Conversion.ToInt(CmbItemNameHistory.Value);
			DataTable dt = ItemPricingSchedule.FinalItemSalePriceListSlip(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				DataGridHistory.ClearStructure();
				GrdForFinalPrice.ClearStructure();
				throw new Exception("Record Not Found For Display");
			}
			DataTable dtHistory = new DataTable();
			dtHistory.Columns.Add("Id");
			dtHistory.Columns.Add("BrandId");
			dtHistory.Columns.Add("BrandName");
			dtHistory.Columns.Add("BrandCropYearId");
			dtHistory.Columns.Add("ItemId");
			dtHistory.Columns.Add("ItemName");
			dtHistory.Columns.Add("CropYear");
			dtHistory.Columns.Add("PreviousRate", typeof(double));
			dtHistory.Columns.Add("ItemRate", typeof(double));
			dtHistory.Columns.Add("RateChangeable");
			dtHistory.Columns.Add("Percent");
			dtHistory.Columns.Add("PercentRate", typeof(double));
			dtHistory.Columns.Add("Expenses", typeof(double));
			dtHistory.Columns.Add("PackingRate", typeof(double));
			for (int j = 0; j < dt.Rows.Count; j++)
			{
				dtHistory.Rows.Add(dt.Rows[j]["Id"], dt.Rows[j]["BrandId"], dt.Rows[j]["BrandName"], dt.Rows[j]["BrandCropYearId"], dt.Rows[j]["sItemId"], dt.Rows[j]["ItemName"], dt.Rows[j]["CropYear"], dt.Rows[j]["PreviousRate"], dt.Rows[j]["ItemRate"], dt.Rows[j]["RateChangeable"], dt.Rows[j]["Percentage"], dt.Rows[j]["PercentRate"], dt.Rows[j]["Expenses"], dt.Rows[j]["PackingRateDetail"]);
			}
			DataGridHistory.DataSource = dtHistory;
			DataGridHistory.RetrieveStructure();
			GridHistoySetting();
			dtForFinalPrice.Rows.Clear();
			if (dtForFinalPrice.Columns.Count == 0)
			{
				dtForFinalPrice.Columns.Add("BrandId");
				dtForFinalPrice.Columns.Add("BrandName");
				dtForFinalPrice.Columns.Add("BrandCropYearId");
				dtForFinalPrice.Columns.Add("BrandCropYear");
				dtForFinalPrice.Columns.Add("Margin");
				dtForFinalPrice.Columns.Add("PreviousRate", typeof(double));
				dtForFinalPrice.Columns.Add("PackingRate", typeof(double));
				dtForFinalPrice.Columns.Add("BaseRate", typeof(double));
				dtForFinalPrice.Columns.Add("IsActive", typeof(bool));
				dtForFinalPrice.Columns.Add("CompName");
				dtForFinalPrice.Columns.Add("CompAddress");
				dtForFinalPrice.Columns.Add("CompEmailA");
				dtForFinalPrice.Columns.Add("CompEmailB");
				dtForFinalPrice.Columns.Add("CompLogoImage");
				dtForFinalPrice.Columns.Add("ReportingRemarks");
			}
			int i;
			for (i = 0; i < dt.Rows.Count; i++)
			{
				if (!dtForFinalPrice.AsEnumerable().Any((DataRow row) => row.Field<string>("BrandId") == Conversion.ToString(dt.Rows[i]["BrandId"]) && row.Field<string>("BrandCropYearId") == Conversion.ToString(dt.Rows[i]["BrandCropYearId"])))
				{
					dtForFinalPrice.Rows.Add(dt.Rows[i]["BrandId"], dt.Rows[i]["BrandName"], dt.Rows[i]["BrandCropYearId"], dt.Rows[i]["BrandCropYear"], dt.Rows[i]["Margin"], dt.Rows[i]["PreviousRate"], dt.Rows[i]["PackingRate"], dt.Rows[i]["MessageRate"], dt.Rows[i]["IsActive"], dt.Rows[i]["CompName"], dt.Rows[i]["CompAddress"], dt.Rows[i]["CompEmailA"], dt.Rows[i]["CompEmailB"], dt.Rows[i]["CompLogoImage"], dt.Rows[i]["ReportingRemarks"]);
				}
			}
			GrdForFinalPrice.DataSource = dtForFinalPrice;
			GrdForFinalPrice.RetrieveStructure();
			OriginalFinalPriceData = dtForFinalPrice.Copy();
			GrdForFinalPrice.RootTable.Columns["BrandId"].Visible = false;
			GrdForFinalPrice.RootTable.Columns["BrandCropYearId"].Visible = false;
			GrdForFinalPrice.RootTable.Columns["BrandCropYear"].Caption = "Crop Year";
			GrdForFinalPrice.AllowEdit = (InheritableBoolean)1;
			GrdForFinalPrice.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)GrdForFinalPrice.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
				item.EditType = (EditType)0;
				item.FilterEditType = (FilterEditType)1;
			}
			GrdForFinalPrice.RootTable.Columns["IsActive"].EditType = (EditType)8;
			GrdForFinalPrice.RootTable.Columns["BrandName"].Width = 250;
			GrdForFinalPrice.RootTable.Columns["Margin"].Width = 50;
			GrdForFinalPrice.RootTable.Columns["PreviousRate"].Width = 70;
			GrdForFinalPrice.RootTable.Columns["PackingRate"].Width = 70;
			GrdForFinalPrice.RootTable.Columns["BaseRate"].Width = 70;
			GrdForFinalPrice.RootTable.Columns["BrandCropYear"].Width = 60;
			GrdForFinalPrice.RootTable.Columns["IsActive"].ColumnType = (ColumnType)3;
			GrdForFinalPrice.RootTable.Columns["IsActive"].Width = 60;
			GrdForFinalPrice.RootTable.Columns["PreviousRate"].HeaderAlignment = (TextAlignment)2;
			GrdForFinalPrice.RootTable.Columns["PackingRate"].HeaderAlignment = (TextAlignment)2;
			GrdForFinalPrice.RootTable.Columns["BaseRate"].HeaderAlignment = (TextAlignment)2;
			GrdForFinalPrice.RootTable.Columns["BrandCropYear"].HeaderAlignment = (TextAlignment)2;
			GrdForFinalPrice.RootTable.Columns["Margin"].HeaderAlignment = (TextAlignment)2;
			GrdForFinalPrice.RootTable.Columns["PreviousRate"].TextAlignment = (TextAlignment)3;
			GrdForFinalPrice.RootTable.Columns["PackingRate"].TextAlignment = (TextAlignment)3;
			GrdForFinalPrice.RootTable.Columns["BaseRate"].TextAlignment = (TextAlignment)3;
			GrdForFinalPrice.RootTable.Columns["Margin"].TextAlignment = (TextAlignment)2;
			GrdForFinalPrice.RootTable.Columns["PreviousRate"].FormatString = "#,#";
			GrdForFinalPrice.RootTable.Columns["PackingRate"].FormatString = "#,#.#";
			GrdForFinalPrice.RootTable.Columns["BaseRate"].FormatString = "#,#";
			GrdForFinalPrice.RootTable.Columns["CompName"].Visible = false;
			GrdForFinalPrice.RootTable.Columns["CompAddress"].Visible = false;
			GrdForFinalPrice.RootTable.Columns["CompEmailA"].Visible = false;
			GrdForFinalPrice.RootTable.Columns["CompEmailB"].Visible = false;
			GrdForFinalPrice.RootTable.Columns["CompLogoImage"].Visible = false;
			GrdForFinalPrice.RootTable.Columns["ReportingRemarks"].Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnAllocateBrandtoDiscount_Click(object sender, EventArgs e)
	{
		try
		{
			new InvAllocateBrandItemToDiscType(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbDiscType_Leave(object sender, EventArgs e)
	{
		try
		{
			ItemFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmItemSalesPricingSchedule_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnPrint324A_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtForFinalPrice.Rows.Count == 0 || dtForFinalPrice == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtForFinalPrice, "324A_SalesPricingList.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateIsActiveFromHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		try
		{
			ItemPricingSchedule obj = new ItemPricingSchedule();
			if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			List<DataRow> SaveList = ((DataTable)GrdForFinalPrice.DataSource).AsEnumerable().Where(delegate(DataRow originalRow)
			{
				DataRow dataRow = OriginalFinalPriceData.AsEnumerable().FirstOrDefault((DataRow row) => Conversion.ToString((object)row.Field<string>("BrandId")) == Conversion.ToString((object)originalRow.Field<string>("BrandId")) && Conversion.ToString((object)row.Field<string>("BrandCropYearId")) == Conversion.ToString((object)originalRow.Field<string>("BrandCropYearId")));
				return dataRow != null && originalRow.Field<bool>("IsActive") != dataRow.Field<bool>("IsActive");
			}).ToList();
			obj.ItemPricingScheduleslist = new List<ItemPricingSchedule>();
			for (int i = 0; i < SaveList.Count; i++)
			{
				ItemPricingSchedule obj2 = new ItemPricingSchedule();
				obj2.CompanyId = UserAccount.CompanyId;
				obj2.OrganizationId = UserAccount.OrganizationId;
				obj2.BranchesId = UserAccount.BranchesId;
				obj2.ItemId = Conversion.ToInt(SaveList[i]["BrandId"]);
				obj2.CropYearId = Conversion.ToInt(SaveList[i]["BrandCropYearId"]);
				obj2.IsActive = Conversion.ToBool(SaveList[i]["IsActive"]);
				obj.ItemPricingScheduleslist.Add(obj2);
			}
			ItemPricingSchedule.ItemSalePriceList_ActiveInActive(obj);
			MessageBox.Show("Updated Successfully");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUpdateHis_Click(object sender, EventArgs e)
	{
		try
		{
			UpdateIsActiveFromHistory();
			BtnShowHistoryReport_Click(null, null);
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
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
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
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Expected O, but got Unknown
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Expected O, but got Unknown
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Expected O, but got Unknown
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Expected O, but got Unknown
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Expected O, but got Unknown
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Expected O, but got Unknown
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Expected O, but got Unknown
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Expected O, but got Unknown
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Expected O, but got Unknown
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Expected O, but got Unknown
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Expected O, but got Unknown
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Expected O, but got Unknown
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Expected O, but got Unknown
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Expected O, but got Unknown
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Expected O, but got Unknown
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Expected O, but got Unknown
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Expected O, but got Unknown
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Expected O, but got Unknown
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Expected O, but got Unknown
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Expected O, but got Unknown
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Expected O, but got Unknown
		//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Expected O, but got Unknown
		//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fe: Expected O, but got Unknown
		//IL_050a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0514: Expected O, but got Unknown
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Expected O, but got Unknown
		//IL_0578: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Expected O, but got Unknown
		//IL_3127: Unknown result type (might be due to invalid IL or missing references)
		//IL_3131: Expected O, but got Unknown
		GridEXLayout grdFinal_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.WholeSale.frmItemSalesPricingSchedule));
		GridEXLayout GrdForFinalPrice_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel11 = new System.Windows.Forms.Panel();
		this.panel17 = new System.Windows.Forms.Panel();
		this.grdFinal = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label8 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel15 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label7 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label24 = new System.Windows.Forms.Label();
		this.CmbCropYear = new UltraCombo();
		this.cmbDiscType = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.txtPackingUom = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.txtMarginPercent = new System.Windows.Forms.TextBox();
		this.datEffectedTime = new System.Windows.Forms.DateTimePicker();
		this.btnAdd = new System.Windows.Forms.Button();
		this.cmbItem = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.datEffectedDate = new System.Windows.Forms.DateTimePicker();
		this.label3 = new System.Windows.Forms.Label();
		this.txtDescription = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnAllocateBrandtoDiscount = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.panel18 = new System.Windows.Forms.Panel();
		this.panel20 = new System.Windows.Forms.Panel();
		this.panel24 = new System.Windows.Forms.Panel();
		this.GrdForFinalPrice = new GridEX();
		this.panel22 = new System.Windows.Forms.Panel();
		this.label12 = new System.Windows.Forms.Label();
		this.panel19 = new System.Windows.Forms.Panel();
		this.panel23 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel21 = new System.Windows.Forms.Panel();
		this.label11 = new System.Windows.Forms.Label();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label14 = new System.Windows.Forms.Label();
		this.CmbItemNameHistory = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbCropYearHistory = new UltraCombo();
		this.BtnShowHistoryReport = new System.Windows.Forms.Button();
		this.txtEffectedDateForHistory = new System.Windows.Forms.DateTimePicker();
		this.label6 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnPrint324A = new System.Windows.Forms.ToolStripButton();
		this.cmbItemCategory = new UltraCombo();
		this.cmbItemType = new UltraCombo();
		this.BtnUpdateHis = new System.Windows.Forms.Button();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel11.SuspendLayout();
		this.panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdFinal).BeginInit();
		this.panel16.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel15.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel14.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDiscType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel13.SuspendLayout();
		this.panel18.SuspendLayout();
		this.panel20.SuspendLayout();
		this.panel24.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdForFinalPrice).BeginInit();
		this.panel22.SuspendLayout();
		this.panel19.SuspendLayout();
		this.panel23.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel21.SuspendLayout();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemNameHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYearHistory).BeginInit();
		this.panel8.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemType).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1142, 719);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1142, 719);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1134, 693);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel7);
		this.panel2.Controls.Add(this.panel5);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1128, 687);
		this.panel2.TabIndex = 0;
		this.panel7.Controls.Add(this.panel11);
		this.panel7.Controls.Add(this.panel10);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 159);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1128, 528);
		this.panel7.TabIndex = 3;
		this.panel11.AutoScroll = true;
		this.panel11.Controls.Add(this.panel17);
		this.panel11.Controls.Add(this.panel16);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel11.Location = new System.Drawing.Point(613, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(516, 528);
		this.panel11.TabIndex = 2;
		this.panel17.Controls.Add((System.Windows.Forms.Control)(object)this.grdFinal);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel17.Location = new System.Drawing.Point(0, 26);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(516, 502);
		this.panel17.TabIndex = 4;
		this.grdFinal.AllowEdit = (InheritableBoolean)2;
		this.grdFinal.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdFinal_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdFinal.DesignTimeLayout = grdFinal_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdFinal).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdFinal.DynamicFiltering = true;
		this.grdFinal.FilterMode = (FilterMode)2;
		this.grdFinal.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdFinal).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdFinal.GroupByBoxVisible = false;
		this.grdFinal.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdFinal).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdFinal).Name = "grdFinal";
		((ScrollableBase)this.grdFinal).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdFinal).Size = new System.Drawing.Size(516, 502);
		((System.Windows.Forms.Control)(object)this.grdFinal).TabIndex = 0;
		this.grdFinal.TotalRow = (InheritableBoolean)1;
		this.grdFinal.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdFinal).VisualStyle = (VisualStyle)4;
		this.panel16.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel16.Controls.Add(this.label8);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 0);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(516, 26);
		this.panel16.TabIndex = 3;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(7, 7);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(146, 17);
		this.label8.TabIndex = 26;
		this.label8.Text = "Item Sales Price List";
		this.panel10.Controls.Add(this.panel15);
		this.panel10.Controls.Add(this.panel14);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(613, 528);
		this.panel10.TabIndex = 1;
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel15.Location = new System.Drawing.Point(0, 26);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(613, 502);
		this.panel15.TabIndex = 3;
		this.grd.AllowEdit = (InheritableBoolean)2;
		this.grd.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(613, 502);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.panel14.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel14.Controls.Add(this.label7);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(613, 26);
		this.panel14.TabIndex = 2;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(7, 6);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(133, 17);
		this.label7.TabIndex = 26;
		this.label7.Text = "Item Recipe Detail";
		this.panel5.Controls.Add(this.label24);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYear);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDiscType);
		this.panel5.Controls.Add(this.label5);
		this.panel5.Controls.Add(this.txtPackingUom);
		this.panel5.Controls.Add(this.label4);
		this.panel5.Controls.Add(this.txtMarginPercent);
		this.panel5.Controls.Add(this.datEffectedTime);
		this.panel5.Controls.Add(this.btnAdd);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.panel5.Controls.Add(this.label9);
		this.panel5.Controls.Add(this.label13);
		this.panel5.Controls.Add(this.datEffectedDate);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Controls.Add(this.txtDescription);
		this.panel5.Controls.Add(this.label10);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 61);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1128, 98);
		this.panel5.TabIndex = 0;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(9, 71);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(57, 15);
		this.label24.TabIndex = 584;
		this.label24.Text = "Crop Year";
		this.CmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance58;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance65).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance68).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Location = new System.Drawing.Point(137, 65);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Name = "CmbCropYear";
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Size = new System.Drawing.Size(227, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).TabIndex = 3;
		this.cmbDiscType.AllowNull = (DefaultableBoolean)1;
		this.cmbDiscType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDiscType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDiscType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDiscType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbDiscType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDiscType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDiscType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDiscType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.cmbDiscType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDiscType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbDiscType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDiscType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDiscType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDiscType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbDiscType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDiscType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDiscType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance78).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDiscType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbDiscType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDiscType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance84).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDiscType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDiscType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbDiscType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDiscType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDiscType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbDiscType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDiscType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbDiscType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDiscType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDiscType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDiscType).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDiscType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDiscType).Location = new System.Drawing.Point(466, 10);
		((System.Windows.Forms.Control)(object)this.cmbDiscType).Name = "cmbDiscType";
		((System.Windows.Forms.Control)(object)this.cmbDiscType).Size = new System.Drawing.Size(249, 24);
		((System.Windows.Forms.Control)(object)this.cmbDiscType).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.cmbDiscType).Leave += new System.EventHandler(cmbDiscType_Leave);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(548, 71);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(78, 15);
		this.label5.TabIndex = 582;
		this.label5.Text = "Packing Uom";
		this.txtPackingUom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackingUom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackingUom.Location = new System.Drawing.Point(632, 67);
		this.txtPackingUom.Name = "txtPackingUom";
		this.txtPackingUom.Size = new System.Drawing.Size(83, 23);
		this.txtPackingUom.TabIndex = 7;
		this.txtPackingUom.Text = "50";
		this.txtPackingUom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPackingUom_KeyPress);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(376, 71);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(58, 15);
		this.label4.TabIndex = 580;
		this.label4.Text = "Margin %";
		this.txtMarginPercent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMarginPercent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMarginPercent.Location = new System.Drawing.Point(466, 67);
		this.txtMarginPercent.Name = "txtMarginPercent";
		this.txtMarginPercent.Size = new System.Drawing.Size(77, 23);
		this.txtMarginPercent.TabIndex = 6;
		this.txtMarginPercent.TextChanged += new System.EventHandler(txtMarginPercent_TextChanged);
		this.txtMarginPercent.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtMarginPercent_KeyPress);
		this.datEffectedTime.CustomFormat = "";
		this.datEffectedTime.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datEffectedTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
		this.datEffectedTime.Location = new System.Drawing.Point(265, 40);
		this.datEffectedTime.Name = "datEffectedTime";
		this.datEffectedTime.Size = new System.Drawing.Size(99, 23);
		this.datEffectedTime.TabIndex = 2;
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(720, 65);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(61, 26);
		this.btnAdd.TabIndex = 8;
		this.btnAdd.Text = "Show";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance37;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance35).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance51).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(466, 38);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(249, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.cmbItem).TextChanged += new System.EventHandler(cmbItem_TextChanged);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(9, 15);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(120, 15);
		this.label9.TabIndex = 574;
		this.label9.Text = "Schedule Description";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(376, 15);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(83, 15);
		this.label13.TabIndex = 573;
		this.label13.Text = "Discount Type";
		this.datEffectedDate.CustomFormat = "dd-MMM-yyyy";
		this.datEffectedDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datEffectedDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datEffectedDate.Location = new System.Drawing.Point(137, 40);
		this.datEffectedDate.Name = "datEffectedDate";
		this.datEffectedDate.Size = new System.Drawing.Size(126, 23);
		this.datEffectedDate.TabIndex = 1;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(376, 44);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(67, 15);
		this.label3.TabIndex = 573;
		this.label3.Text = "Item Name";
		this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDescription.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDescription.Location = new System.Drawing.Point(137, 11);
		this.txtDescription.Name = "txtDescription";
		this.txtDescription.Size = new System.Drawing.Size(227, 23);
		this.txtDescription.TabIndex = 0;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(9, 44);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(78, 15);
		this.label10.TabIndex = 576;
		this.label10.Text = "Effected Date";
		this.panel4.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel4.Controls.Add(this.label15);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 28);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1128, 33);
		this.panel4.TabIndex = 1;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(7, 7);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(235, 19);
		this.label15.TabIndex = 26;
		this.label15.Text = "Item Sales Pricing Schedule";
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1128, 28);
		this.panel3.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnnew, this.btnsave, this.btnUpdate, this.toolStripSeparator, this.btnAllocateBrandtoDiscount });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1128, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
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
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.btnAllocateBrandtoDiscount.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAllocateBrandtoDiscount.Name = "btnAllocateBrandtoDiscount";
		this.btnAllocateBrandtoDiscount.Size = new System.Drawing.Size(259, 24);
		this.btnAllocateBrandtoDiscount.Text = "Allocate Brand Item to Discount Type";
		this.btnAllocateBrandtoDiscount.Click += new System.EventHandler(btnAllocateBrandtoDiscount_Click);
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1134, 693);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add(this.panel9);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1128, 687);
		this.panel6.TabIndex = 0;
		this.panel9.BackColor = System.Drawing.Color.White;
		this.panel9.Controls.Add(this.panel13);
		this.panel9.Controls.Add(this.panel12);
		this.panel9.Controls.Add(this.panel8);
		this.panel9.Controls.Add(this.toolStrip2);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1128, 687);
		this.panel9.TabIndex = 1;
		this.panel13.Controls.Add(this.panel18);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(0, 99);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1128, 588);
		this.panel13.TabIndex = 11;
		this.panel18.Controls.Add(this.panel20);
		this.panel18.Controls.Add(this.panel19);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel18.Location = new System.Drawing.Point(0, 0);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1128, 588);
		this.panel18.TabIndex = 3;
		this.panel20.Controls.Add(this.panel24);
		this.panel20.Controls.Add(this.panel22);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel20.Location = new System.Drawing.Point(700, 0);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(428, 588);
		this.panel20.TabIndex = 1;
		this.panel24.Controls.Add((System.Windows.Forms.Control)(object)this.GrdForFinalPrice);
		this.panel24.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel24.Location = new System.Drawing.Point(0, 26);
		this.panel24.Name = "panel24";
		this.panel24.Size = new System.Drawing.Size(428, 562);
		this.panel24.TabIndex = 5;
		this.GrdForFinalPrice.AllowEdit = (InheritableBoolean)2;
		GrdForFinalPrice_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdForFinalPrice.DesignTimeLayout = GrdForFinalPrice_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdForFinalPrice).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdForFinalPrice.DynamicFiltering = true;
		this.GrdForFinalPrice.FilterMode = (FilterMode)2;
		this.GrdForFinalPrice.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdForFinalPrice).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdForFinalPrice.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdForFinalPrice).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GrdForFinalPrice).Name = "GrdForFinalPrice";
		((ScrollableBase)this.GrdForFinalPrice).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdForFinalPrice).Size = new System.Drawing.Size(428, 562);
		((System.Windows.Forms.Control)(object)this.GrdForFinalPrice).TabIndex = 2;
		this.GrdForFinalPrice.TotalRow = (InheritableBoolean)1;
		this.GrdForFinalPrice.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdForFinalPrice).VisualStyle = (VisualStyle)4;
		this.panel22.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel22.Controls.Add(this.label12);
		this.panel22.Controls.Add(this.BtnUpdateHis);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel22.Location = new System.Drawing.Point(0, 0);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(428, 26);
		this.panel22.TabIndex = 4;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label12.ForeColor = System.Drawing.Color.White;
		this.label12.Location = new System.Drawing.Point(7, 7);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(146, 17);
		this.label12.TabIndex = 26;
		this.label12.Text = "Item Sales Price List";
		this.panel19.Controls.Add(this.panel23);
		this.panel19.Controls.Add(this.panel21);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel19.Location = new System.Drawing.Point(0, 0);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(700, 588);
		this.panel19.TabIndex = 0;
		this.panel23.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel23.Location = new System.Drawing.Point(0, 26);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(700, 562);
		this.panel23.TabIndex = 4;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		this.DataGridHistory.ColumnAutoResize = true;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(700, 562);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.panel21.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel21.Controls.Add(this.label11);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel21.Location = new System.Drawing.Point(0, 0);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(700, 26);
		this.panel21.TabIndex = 3;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label11.ForeColor = System.Drawing.Color.White;
		this.label11.Location = new System.Drawing.Point(7, 6);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(133, 17);
		this.label11.TabIndex = 26;
		this.label11.Text = "Item Recipe Detail";
		this.panel12.Controls.Add(this.label14);
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemNameHistory);
		this.panel12.Controls.Add(this.label1);
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYearHistory);
		this.panel12.Controls.Add(this.BtnShowHistoryReport);
		this.panel12.Controls.Add(this.txtEffectedDateForHistory);
		this.panel12.Controls.Add(this.label6);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 61);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1128, 38);
		this.panel12.TabIndex = 10;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(448, 13);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(67, 15);
		this.label14.TabIndex = 588;
		this.label14.Text = "Item Name";
		this.CmbItemNameHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemNameHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemNameHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemNameHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance43;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance49).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance50).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance54).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemNameHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).Location = new System.Drawing.Point(519, 7);
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).Name = "CmbItemNameHistory";
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).Size = new System.Drawing.Size(227, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).TabIndex = 2;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(240, 13);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(57, 15);
		this.label1.TabIndex = 1;
		this.label1.Text = "Crop Year";
		this.CmbCropYearHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYearHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYearHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYearHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance73;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance79).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYearHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCropYearHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYearHistory).Location = new System.Drawing.Point(309, 7);
		((System.Windows.Forms.Control)(object)this.CmbCropYearHistory).Name = "CmbCropYearHistory";
		((System.Windows.Forms.Control)(object)this.CmbCropYearHistory).Size = new System.Drawing.Size(133, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYearHistory).TabIndex = 1;
		this.BtnShowHistoryReport.BackColor = System.Drawing.Color.Teal;
		this.BtnShowHistoryReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShowHistoryReport.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShowHistoryReport.ForeColor = System.Drawing.Color.White;
		this.BtnShowHistoryReport.Location = new System.Drawing.Point(752, 7);
		this.BtnShowHistoryReport.Name = "BtnShowHistoryReport";
		this.BtnShowHistoryReport.Size = new System.Drawing.Size(62, 26);
		this.BtnShowHistoryReport.TabIndex = 3;
		this.BtnShowHistoryReport.Text = "Show";
		this.BtnShowHistoryReport.UseVisualStyleBackColor = false;
		this.BtnShowHistoryReport.Click += new System.EventHandler(BtnShowHistoryReport_Click);
		this.txtEffectedDateForHistory.CustomFormat = "dd-MMM-yyyy";
		this.txtEffectedDateForHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEffectedDateForHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtEffectedDateForHistory.Location = new System.Drawing.Point(96, 9);
		this.txtEffectedDateForHistory.Name = "txtEffectedDateForHistory";
		this.txtEffectedDateForHistory.Size = new System.Drawing.Size(139, 23);
		this.txtEffectedDateForHistory.TabIndex = 0;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(9, 13);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(78, 15);
		this.label6.TabIndex = 578;
		this.label6.Text = "Effected Date";
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.ForeColor = System.Drawing.Color.White;
		this.panel8.Location = new System.Drawing.Point(0, 27);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1128, 34);
		this.panel8.TabIndex = 9;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(8, 7);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(250, 19);
		this.label2.TabIndex = 0;
		this.label2.Text = "Item Pricing Schedule History";
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnPrint, this.btnPrint324A });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1128, 27);
		this.toolStrip2.TabIndex = 8;
		this.toolStrip2.Text = "toolStrip2";
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(221, 24);
		this.btnPrint.Text = "Print-324 ItemSalePricingList";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnPrint324A.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint324A.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint324A.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint324A.Name = "btnPrint324A";
		this.btnPrint324A.Size = new System.Drawing.Size(101, 24);
		this.btnPrint324A.Text = "Print-324A";
		this.btnPrint324A.Click += new System.EventHandler(btnPrint324A_Click);
		this.cmbItemCategory.AllowNull = (DefaultableBoolean)1;
		this.cmbItemCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance8;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance24).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance25).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Location = new System.Drawing.Point(90, 15);
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Name = "cmbItemCategory";
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Size = new System.Drawing.Size(271, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).TabIndex = 0;
		this.cmbItemType.AllowNull = (DefaultableBoolean)1;
		this.cmbItemType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbItemType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance14;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbItemType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance19).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemType).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemType).Location = new System.Drawing.Point(90, 43);
		((System.Windows.Forms.Control)(object)this.cmbItemType).Name = "cmbItemType";
		((System.Windows.Forms.Control)(object)this.cmbItemType).Size = new System.Drawing.Size(271, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemType).TabIndex = 1;
		this.BtnUpdateHis.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnUpdateHis.BackColor = System.Drawing.Color.White;
		this.BtnUpdateHis.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateHis.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnUpdateHis.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateHis.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.BtnUpdateHis.Location = new System.Drawing.Point(393, 0);
		this.BtnUpdateHis.Name = "BtnUpdateHis";
		this.BtnUpdateHis.Size = new System.Drawing.Size(32, 26);
		this.BtnUpdateHis.TabIndex = 3;
		this.BtnUpdateHis.UseVisualStyleBackColor = false;
		this.BtnUpdateHis.Click += new System.EventHandler(BtnUpdateHis_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1142, 719);
		base.Controls.Add(this.panel1);
		base.Name = "frmItemSalesPricingSchedule";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmItemSalesPricingSchedule_FormClosing);
		base.Load += new System.EventHandler(frmItemPricingSchedule_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmItemPricingSchedule_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		this.panel17.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdFinal).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.panel10.ResumeLayout(false);
		this.panel15.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDiscType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel13.ResumeLayout(false);
		this.panel18.ResumeLayout(false);
		this.panel20.ResumeLayout(false);
		this.panel24.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdForFinalPrice).EndInit();
		this.panel22.ResumeLayout(false);
		this.panel22.PerformLayout();
		this.panel19.ResumeLayout(false);
		this.panel23.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel21.ResumeLayout(false);
		this.panel21.PerformLayout();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemNameHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYearHistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemType).EndInit();
		base.ResumeLayout(false);
	}
}

}
