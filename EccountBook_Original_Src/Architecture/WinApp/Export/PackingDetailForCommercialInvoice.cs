using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Export;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.DAL.Export;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Model.Inventory;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Export;

public class PackingDetailForCommercialInvoice : Form
{
	private Attachment AT = new Attachment();

	private int[] AryColWidth;

	private DataTable dtdetail = new DataTable();

	private int RecId;

	private bool UpdateMode;

	private Rightsobjects formrights = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private double ExchangeRate;

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel1;

	private Panel panel10;

	private Label label23;

	private ToolStrip toolStrip4;

	private ToolStripButton btnNew;

	private ToolStripButton btnsave;

	private ToolStripButton btnupdate;

	private ToolStripButton btnrefersh;

	private ToolStripButton btnAttachment;

	private GroupBox groupBox3;

	private Label label5;

	private Label label67;

	internal UltraCombo Cmbinvoiceno;

	private Label label68;

	internal UltraCombo cmbSupCust;

	private Label label21;

	private Label label57;

	private TextBox txtNoOfContainer;

	private Label label55;

	private TextBox txtNetWeight;

	private Label label16;

	private Label label54;

	private TextBox txtGrossWeight;

	internal UltraCombo cmbLoadingPort;

	internal UltraCombo cmbDestinationPort;

	private Label label56;

	private DateTimePicker txtDocDate;

	private Label label12;

	private DateTimePicker txtIformDate;

	private TextBox txtGdNo;

	private Label label46;

	private TextBox txtdocno;

	internal UltraCombo cmbdeliverytermnew;

	internal UltraCombo cmbfcycode;

	private Label label27;

	private Label label7;

	internal UltraCombo cmbcareiertype;

	private TextBox txtLotRef;

	private Label label22;

	internal UltraCombo cmbOtherCustomer;

	internal UltraCombo cmbOtherDestinationPort;

	internal GridEX grdDetail;

	private Panel panel2;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label1;

	private Panel panel3;

	private GroupBox groupBox1;

	internal GridEX gridDetailHistory;

	private Panel panel4;

	private Label label89;

	internal GridEX DataGridHistory;

	private Panel panel15;

	private ToolStrip toolStrip3;

	private ToolStripButton btnLoadAll;

	private ToolStripButton toolStripButton7;

	private ToolStripSeparator toolStripSeparator1;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label69;

	private CheckBox ChkPrintPreview;

	private TextBox txttotalQtyHeaderForConditions;

	private Label label2;

	private ToolStripButton btnPrint;

	private ToolStripButton btnDelete;

	public PackingDetailForCommercialInvoice(UserAccount user)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void PackingDetailForCommercialInvoice_Load(object sender, EventArgs e)
	{
		AutoScroll = true;
		base.AutoScrollMargin = new Size(20, 20);
		base.AutoScrollMinSize = new Size(980, 700);
		formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
		btnsave.Enabled = formrights.DoHaveSaveRight;
		btnupdate.Enabled = formrights.DoHaveUpdateRights;
		btnDelete.Visible = false;
		dtdetail.Columns.Add("Id", typeof(int));
		dtdetail.Columns.Add("CommercialInvoiceId", typeof(int));
		dtdetail.Columns.Add("CommercialInvoiceDetailId", typeof(int));
		dtdetail.Columns.Add("RefDocumentTypeId", typeof(int));
		dtdetail.Columns.Add("ItemId", typeof(int));
		dtdetail.Columns.Add("ItemName");
		dtdetail.Columns.Add("PackTypeId", typeof(int));
		dtdetail.Columns.Add("PackType");
		dtdetail.Columns.Add("CropYearId", typeof(int));
		dtdetail.Columns.Add("CropYear");
		dtdetail.Columns.Add("WarehouseId", typeof(int));
		dtdetail.Columns.Add("JobLotId", typeof(int));
		dtdetail.Columns.Add("OuterQty", typeof(double));
		dtdetail.Columns.Add("UOMScheduleIdOuter", typeof(int));
		dtdetail.Columns.Add("OuterPackUOM", typeof(string));
		dtdetail.Columns.Add("OuterEquivalent", typeof(double));
		dtdetail.Columns.Add("NetWeight", typeof(double));
		dtdetail.Columns.Add("PackingWeight", typeof(double));
		dtdetail.Columns.Add("PackingWeightTotal", typeof(double));
		dtdetail.Columns.Add("GrossWeight", typeof(double));
		dtdetail.Columns.Add("ContainerNo");
		dtdetail.Columns.Add("SealNo");
		dtdetail.Columns.Add("RatePrice", typeof(double));
		dtdetail.Columns.Add("RateUomId", typeof(int));
		grdDetail.DataSource = dtdetail;
		grdDetail.RetrieveStructure();
		grdSettings();
		CustomerGetAll();
		DeliveryTermFill();
		LoadingPortFill();
		MultiCurrencyfill();
		BindInvoices();
		CarierType();
		Cmbinvoiceno.Focus();
		panel3.Visible = false;
		panel3.Enabled = false;
		((Control)(object)Cmbinvoiceno).Enabled = true;
	}

	private void FormReset()
	{
		try
		{
			((Control)(object)Cmbinvoiceno).Enabled = true;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			btnDelete.Visible = false;
			UpdateMode = false;
			RecId = 0;
			((UltraGridBase)cmbcareiertype).Rows[0].Activate();
			txtDocDate.Value = DateTime.Now;
			((Control)(object)cmbSupCust).Text = string.Empty;
			((Control)(object)cmbdeliverytermnew).Text = string.Empty;
			((Control)(object)cmbLoadingPort).Text = string.Empty;
			((Control)(object)cmbDestinationPort).Text = string.Empty;
			((Control)(object)Cmbinvoiceno).Text = string.Empty;
			txtLotRef.Text = string.Empty;
			((Control)(object)cmbfcycode).Text = string.Empty;
			txtGrossWeight.Text = string.Empty;
			txtNetWeight.Text = string.Empty;
			txtNoOfContainer.Text = string.Empty;
			txtGdNo.Text = string.Empty;
			dtdetail.Clear();
			grdDetail.ClearStructure();
			Cmbinvoiceno.Focus();
			panel3.Visible = false;
			AT = new Attachment();
			BindInvoices();
			((Control)(object)Cmbinvoiceno).Text = string.Empty;
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
			FormReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnrefersh_Click(object sender, EventArgs e)
	{
		try
		{
			BindInvoices();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		try
		{
			grdDetail.RootTable.Columns["Id"].Visible = false;
			grdDetail.RootTable.Columns["CommercialInvoiceId"].Visible = false;
			grdDetail.RootTable.Columns["CommercialInvoiceDetailId"].Visible = false;
			grdDetail.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grdDetail.RootTable.Columns["RatePrice"].Visible = false;
			grdDetail.RootTable.Columns["RateUomId"].Visible = false;
			grdDetail.RootTable.Columns["ItemId"].Visible = false;
			grdDetail.RootTable.Columns["PackTypeId"].Visible = false;
			grdDetail.RootTable.Columns["CropYearId"].Visible = false;
			grdDetail.RootTable.Columns["UOMScheduleIdOuter"].Visible = false;
			grdDetail.RootTable.Columns["OuterEquivalent"].Visible = false;
			grdDetail.RootTable.Columns["OuterQty"].Caption = "Qty";
			grdDetail.RootTable.Columns["OuterPackUOM"].Caption = "Pack Uom";
			grdDetail.RootTable.Columns["Id"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["RatePrice"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["RateUomId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["CommercialInvoiceId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["CommercialInvoiceDetailId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["RefDocumentTypeId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["PackTypeId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["PackType"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["CropYearId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["CropYear"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["UOMScheduleIdOuter"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["OuterPackUOM"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["OuterEquivalent"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["PackingWeightTotal"].EditType = (EditType)1;
			grdDetail.RootTable.Columns["GrossWeight"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["NetWeight"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["OuterQty"].EditType = (EditType)1;
			grdDetail.RootTable.Columns["PackingWeight"].EditType = (EditType)1;
			grdDetail.RootTable.Columns["ContainerNo"].EditType = (EditType)1;
			grdDetail.RootTable.Columns["SealNo"].EditType = (EditType)1;
			grdDetail.RootTable.Columns["OuterQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["OuterQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["OuterQty"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["OuterQty"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["NetWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["GrossWeight"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["PackingWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["PackingWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["PackingWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["PackingWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["PackingWeightTotal"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["PackingWeightTotal"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["PackingWeightTotal"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["PackingWeightTotal"].TotalFormatString = "#,##0.##";
			DataTable dtwh = CommonServices.WareHouseGetAllService();
			grdDetail.RootTable.Columns["WarehouseId"].ColumnType = (ColumnType)0;
			grdDetail.RootTable.Columns["WarehouseId"].HasValueList = true;
			grdDetail.RootTable.Columns["WarehouseId"].LimitToList = true;
			grdDetail.RootTable.Columns["WarehouseId"].EditType = (EditType)4;
			grdDetail.RootTable.Columns["WarehouseId"].Caption = "WareHouse Name";
			grdDetail.RootTable.Columns["WarehouseId"].ValueList.PopulateValueList((IEnumerable)dtwh.DefaultView, "Id", "WareHouseName");
			DataTable dtJob = CommonServices.JobLotGetAllService();
			grdDetail.RootTable.Columns["JobLotId"].ColumnType = (ColumnType)0;
			grdDetail.RootTable.Columns["JobLotId"].HasValueList = true;
			grdDetail.RootTable.Columns["JobLotId"].LimitToList = true;
			grdDetail.RootTable.Columns["JobLotId"].EditType = (EditType)4;
			grdDetail.RootTable.Columns["JobLotId"].Caption = "JobLot Name";
			grdDetail.RootTable.Columns["JobLotId"].ValueList.PopulateValueList((IEnumerable)dtJob.DefaultView, "Id", "JobLotDescription");
			grdDetail.RootTable.Columns.Add("Delete");
			grdDetail.RootTable.Columns["Delete"].Caption = "X";
			grdDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdDetail.RootTable.Columns["Delete"].Width = 40;
			grdDetail.RootTable.Columns["Delete"].Position = 0;
			grdDetail.RootTable.Columns.Add("Add");
			grdDetail.RootTable.Columns["Add"].Key = "Add";
			grdDetail.RootTable.Columns["Add"].Caption = "Add";
			grdDetail.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetail.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdDetail.RootTable.Columns["Add"].ButtonText = "Add";
			grdDetail.RootTable.Columns["Add"].Width = 40;
			grdDetail.RootTable.Columns["Add"].Position = 1;
			grdDetail.FrozenColumns = 2;
			CommonServices.GridAutoAdjustment(AryColWidth, grdDetail);
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Unknown result type (might be due to invalid IL or missing references)
		//IL_060b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow selectedRow = grdDetail.CurrentRow;
			double AllOuterQtyExceptSelected = (from GridEXRow row in grdDetail.GetRows()
				where row != selectedRow
				select (double)row.Cells["OuterQty"].Value).Sum();
			double AllNetWeightExceptSelected = (from GridEXRow row in grdDetail.GetRows()
				where row != selectedRow
				select (double)row.Cells["NetWeight"].Value).Sum();
			double AllGrossWeightExceptSelected = (from GridEXRow row in grdDetail.GetRows()
				where row != selectedRow
				select (double)row.Cells["GrossWeight"].Value).Sum();
			double RegularTotalNetWeight = Conversion.ToDouble((object)txtNetWeight.Text) - AllNetWeightExceptSelected;
			double RegularTotalGrossWeight = Conversion.ToDouble((object)txtGrossWeight.Text) - AllGrossWeightExceptSelected;
			double RegularTotalQty = Conversion.ToDouble((object)txttotalQtyHeaderForConditions.Text) - AllOuterQtyExceptSelected;
			grdDetail.UpdateData();
			GridEXRow item = grdDetail.CurrentRow;
			if (e.Column.Key == "NetWeight" && item != null && (int)item.RowType == 0 && item.Cells["GrossWeight"].Value.ToString() != string.Empty && item.Cells["NetWeight"].Value.ToString() != string.Empty)
			{
				double NetWeight = Conversion.ToDouble((object)item.Cells["NetWeight"].Value.ToString());
				if (NetWeight > RegularTotalNetWeight)
				{
					MessageBox.Show("NetWeight Of this row can not be Greater than total Balance NetWeight");
					item.Cells["NetWeight"].Value = Math.Round(RegularTotalNetWeight, 2);
					item.Cells["GrossWeight"].Value = Math.Round(RegularTotalGrossWeight, 2);
				}
				else
				{
					double PackingWeightTotal4 = Conversion.ToDouble((object)item.Cells["PackingWeightTotal"].Value.ToString());
					item.Cells["NetWeight"].Value = NetWeight.ToString();
					item.Cells["GrossWeight"].Value = NetWeight + PackingWeightTotal4;
				}
			}
			if (e.Column.Key == "GrossWeight" && item != null && (int)item.RowType == 0 && item.Cells["GrossWeight"].Value.ToString() != string.Empty && item.Cells["NetWeight"].Value.ToString() != string.Empty)
			{
				double GrossWeight = Conversion.ToDouble((object)item.Cells["GrossWeight"].Value.ToString());
				if (GrossWeight > RegularTotalGrossWeight)
				{
					MessageBox.Show("GrossWeight Of this row can not be Greater than total Balance GrossWeight");
					item.Cells["GrossWeight"].Value = Math.Round(RegularTotalGrossWeight, 2);
					item.Cells["NetWeight"].Value = Math.Round(RegularTotalNetWeight, 2);
				}
				else
				{
					double PackingWeightTotal3 = Conversion.ToDouble((object)item.Cells["PackingWeightTotal"].Value.ToString());
					item.Cells["GrossWeight"].Value = GrossWeight.ToString();
					item.Cells["NetWeight"].Value = GrossWeight - PackingWeightTotal3;
				}
			}
			if ((e.Column.Key == "PackingWeight" || e.Column.Key == "OuterQty") && item != null && (int)item.RowType == 0 && item.Cells["GrossWeight"].Value.ToString() != string.Empty && item.Cells["NetWeight"].Value.ToString() != string.Empty)
			{
				double OuterEquivalent = Conversion.ToDouble((object)item.Cells["OuterEquivalent"].Value.ToString());
				double PackingWeight2 = Conversion.ToDouble((object)item.Cells["PackingWeight"].Value.ToString());
				double OuterQty2 = Conversion.ToDouble((object)item.Cells["OuterQty"].Value.ToString());
				if (OuterQty2 > RegularTotalQty)
				{
					MessageBox.Show("Qty Of this row can not be Greater than total Balance Qty");
					item.Cells["OuterQty"].Value = Math.Round(RegularTotalQty, 2);
					OuterQty2 = RegularTotalQty;
				}
				else
				{
					item.Cells["OuterQty"].Value = Math.Round(OuterQty2, 2);
				}
				double PackingWeightTotal2 = OuterQty2 * PackingWeight2;
				item.Cells["NetWeight"].Value = (OuterQty2 * OuterEquivalent).ToString();
				double num = Conversion.ToDouble((object)item.Cells["NetWeight"].Value.ToString());
				item.Cells["PackingWeightTotal"].Value = PackingWeightTotal2;
				item.Cells["GrossWeight"].Value = (num + PackingWeightTotal2).ToString();
			}
			if (e.Column.Key == "PackingWeightTotal" && item != null && (int)item.RowType == 0 && item.Cells["GrossWeight"].Value.ToString() != string.Empty && item.Cells["NetWeight"].Value.ToString() != string.Empty)
			{
				double num2 = Conversion.ToDouble((object)item.Cells["NetWeight"].Value.ToString());
				double PackingWeightTotal = Conversion.ToDouble((object)item.Cells["PackingWeightTotal"].Value.ToString());
				double OuterQty = Conversion.ToDouble((object)item.Cells["OuterQty"].Value.ToString());
				item.Cells["GrossWeight"].Value = (num2 + PackingWeightTotal).ToString();
				double PackingWeight = PackingWeightTotal / OuterQty;
				item.Cells["PackingWeight"].Value = PackingWeight.ToString();
			}
			grdDetail.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				if (btnsave.Enabled && btnsave.Visible)
				{
					if (MessageBox.Show("Are you sure to Delete the row?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
					{
						return;
					}
					GridEXRow item = grdDetail.CurrentRow;
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
				}
				else
				{
					MessageBox.Show("Can not Delete Row in Update Mode...");
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInGrid();
			}
			grdDetail.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInGrid()
	{
		try
		{
			double TotalGrossWeight = Conversion.ToDouble((object)txtGrossWeight.Text.Trim());
			double GrossWeightDetail = 0.0;
			double TotalQty = Conversion.ToDouble((object)txttotalQtyHeaderForConditions.Text.Trim());
			double PackingWeight = 0.0;
			double TotalNetWeight = Conversion.ToDouble((object)txtNetWeight.Text.Trim());
			double NetWeightDetail = 0.0;
			double Qty = 0.0;
			if (grdDetail.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow item in rows)
				{
					GrossWeightDetail += Conversion.ToDouble(item.Cells["GrossWeight"].Value);
					PackingWeight = Conversion.ToDouble(item.Cells["PackingWeightTotal"].Value);
					NetWeightDetail += Conversion.ToDouble(item.Cells["NetWeight"].Value);
					Qty += Conversion.ToDouble(item.Cells["OuterQty"].Value);
				}
			}
			double GrossWeight = TotalGrossWeight - GrossWeightDetail;
			double NetWeight = TotalNetWeight - NetWeightDetail;
			double GrossQty = TotalQty - Qty;
			if ((GrossWeight > 0.0 && NetWeight > 0.0) || GrossQty > 0.0)
			{
				if (GrossWeight > 0.0)
				{
					_ = GrossWeight / Conversion.ToDouble(grdDetail.CurrentRow.Cells["OuterEquivalent"].Value);
					dtdetail.Rows.Add(0, grdDetail.CurrentRow.Cells["CommercialInvoiceId"].Value, grdDetail.CurrentRow.Cells["CommercialInvoiceDetailId"].Value, grdDetail.CurrentRow.Cells["RefDocumentTypeId"].Value, grdDetail.CurrentRow.Cells["ItemId"].Value, grdDetail.CurrentRow.Cells["ItemName"].Value, grdDetail.CurrentRow.Cells["PackTypeId"].Value, grdDetail.CurrentRow.Cells["PackType"].Value, grdDetail.CurrentRow.Cells["CropYearId"].Value, grdDetail.CurrentRow.Cells["CropYear"].Value, grdDetail.CurrentRow.Cells["WarehouseId"].Value, grdDetail.CurrentRow.Cells["JobLotId"].Value, Math.Round(GrossQty, 2), grdDetail.CurrentRow.Cells["UOMScheduleIdOuter"].Value, grdDetail.CurrentRow.Cells["OuterPackUOM"].Value, grdDetail.CurrentRow.Cells["OuterEquivalent"].Value, NetWeight, grdDetail.CurrentRow.Cells["PackingWeight"].Value, Conversion.ToDouble(grdDetail.CurrentRow.Cells["PackingWeight"].Value) * GrossQty, GrossWeight, grdDetail.CurrentRow.Cells["ContainerNo"].Value, grdDetail.CurrentRow.Cells["SealNo"].Value, grdDetail.CurrentRow.Cells["RatePrice"].Value, grdDetail.CurrentRow.Cells["RateUomId"].Value);
					grdDetail.DataSource = dtdetail;
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

	private void GridCalculations()
	{
		try
		{
			GridEXRow selectedRow = grdDetail.CurrentRow;
			double AllOuterQtyExceptSelected = (from GridEXRow row in grdDetail.GetRows()
				where row != selectedRow
				select (double)row.Cells["OuterQty"].Value).Sum();
			Conversion.ToDouble((object)txttotalQtyHeaderForConditions.Text);
			grdDetail.UpdateData();
			GridEXRow item = grdDetail.CurrentRow;
			if (item.Cells["GrossWeight"].Value.ToString() != string.Empty && item.Cells["NetWeight"].Value.ToString() != string.Empty)
			{
				double OuterEquivalent = Conversion.ToDouble((object)item.Cells["OuterEquivalent"].Value.ToString());
				double PackingWeight = Conversion.ToDouble((object)item.Cells["PackingWeight"].Value.ToString());
				double OuterQty = Conversion.ToDouble((object)item.Cells["OuterQty"].Value.ToString());
				double PackingWeightTotal = OuterQty * PackingWeight;
				item.Cells["NetWeight"].Value = (OuterQty * OuterEquivalent).ToString();
				double num = Conversion.ToDouble((object)item.Cells["NetWeight"].Value.ToString());
				item.Cells["PackingWeightTotal"].Value = PackingWeightTotal;
				item.Cells["GrossWeight"].Value = (num + PackingWeightTotal).ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CustomerGetAll()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable Supplierlst = SupplierCustomer.ReadByOrganizationCompanyIdForExport(new SupplierCustomer
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (Supplierlst.Rows.Count > 0)
			{
				DDL.BindDDLNew(Supplierlst, cmbSupCust, "Id", "CompanyName", "Supplier Name", false);
				((UltraGridBase)cmbSupCust).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeliveryTermFill()
	{
		try
		{
			DataTable dtDeliveryTerms = ExImDeliveryTerm.Getall();
			if (dtDeliveryTerms.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtDeliveryTerms, cmbdeliverytermnew, "Id", "Code", "DeliveryTerms", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadingPortFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = SeaPorts.Getall(new SeaPorts
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbLoadingPort, "Id", "PortName", "Loading Port", false);
				DDL.BindDDLNew(dt, cmbDestinationPort, "Id", "PortName", "Destination Port", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MultiCurrencyfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = MultiCurrency.GetAll(new MultiCurrency
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbfcycode, "Id", "CurrencyName", "CurrencyName", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CarierType()
	{
		try
		{
			DataTable dtcariertype = new DataTable();
			dtcariertype.Columns.Add("Id");
			dtcariertype.Columns.Add("Type");
			dtcariertype.Rows.Add(1, "By Sea");
			dtcariertype.Rows.Add(2, "By Air");
			dtcariertype.Rows.Add(3, "By Road");
			DDL.BindDDL(dtcariertype, cmbcareiertype, "Id", "Type", "Carier type", ZeroIndex: false);
			((UltraGridBase)cmbcareiertype).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindInvoices()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			DataTable dtPreIN = ExImInvoice.GetCommercialInvoicenumberForPackingDetail(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 211
			});
			if (dtPreIN.Rows.Count > 0)
			{
				DDL.BindDDL(dtPreIN, Cmbinvoiceno, "Id", "InvoiceNo", "Commercial Invoice_Number", ZeroIndex: false);
				((UltraGridBase)Cmbinvoiceno).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			else
			{
				((UltraGridBase)Cmbinvoiceno).DataSource = null;
				((Control)(object)Cmbinvoiceno).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void CommercialInvoiceDataByInvoiceId(int InvoiceId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ExImInvoice PIH = new ExImInvoice();
			PIH = ExImInvoice.GetByID(InvoiceId);
			if (PIH != null)
			{
				tabControl1.SelectedIndex = 0;
				txtdocno.Text = PIH.DocCode.ToString();
				txtDocDate.Value = Conversion.ToDateTime((object)PIH.DocDate);
				cmbSupCust.Value = PIH.SupplierCustomerId;
				cmbDestinationPort.Value = PIH.DestinationPortId;
				cmbOtherCustomer.Value = PIH.OtherCustomerId;
				cmbOtherDestinationPort.Value = PIH.OtherDestinationPortId;
				txtLotRef.Text = PIH.LotNoRef;
				cmbdeliverytermnew.Value = PIH.DeliveryTermId;
				cmbLoadingPort.Value = PIH.LoadingPortId;
				cmbDestinationPort.Value = PIH.DestinationPortId;
				ExchangeRate = Conversion.ToDouble((object)PIH.ConversionRate);
				((Control)(object)cmbcareiertype).Text = PIH.CarierType;
				cmbfcycode.Value = PIH.FcurrencyId;
				txtGrossWeight.Text = PIH.GrossWeight.ToString();
				txtNetWeight.Text = PIH.NetWeight.ToString();
				txtNoOfContainer.Text = PIH.NoOfContainers.ToString();
				txtGdNo.Text = PIH.EFormNo;
				txtIformDate.Value = PIH.EFormDate;
				double TotalQty = 0.0;
				dtdetail.Rows.Clear();
				for (int i = 0; i < PIH.ExImInvoiceDetail.Count; i++)
				{
					dtdetail.Rows.Add(0, PIH.ExImInvoiceDetail[i].ExImInvoiceId, PIH.ExImInvoiceDetail[i].Id, 211, PIH.ExImInvoiceDetail[i].ItemId, PIH.ExImInvoiceDetail[i].ItemName, PIH.ExImInvoiceDetail[i].PackingMaterialTypeId, PIH.ExImInvoiceDetail[i].PackMaterilaType, PIH.ExImInvoiceDetail[i].CropYearId, PIH.ExImInvoiceDetail[i].CropYear, 0, PIH.ExImInvoiceDetail[i].JobLotId, PIH.ExImInvoiceDetail[i].OuterQty, PIH.ExImInvoiceDetail[i].OuterQtyUomId, PIH.ExImInvoiceDetail[i].OuterUOM, PIH.ExImInvoiceDetail[i].OuterEquivalent, PIH.ExImInvoiceDetail[i].NetWeight, PIH.ExImInvoiceDetail[i].PackingWeight, PIH.ExImInvoiceDetail[i].TotalPackingWeight, PIH.ExImInvoiceDetail[i].GrossWeight, "", "", PIH.ExImInvoiceDetail[i].RatePrice, PIH.ExImInvoiceDetail[i].RateUomId);
					TotalQty += PIH.ExImInvoiceDetail[i].OuterQty;
				}
				txttotalQtyHeaderForConditions.Text = TotalQty.ToString();
				grdDetail.DataSource = dtdetail;
				grdDetail.RetrieveStructure();
				grdSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Cmbinvoiceno_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)Cmbinvoiceno).ActiveRow != null && Conversion.ToInt(Cmbinvoiceno.Value) != 0)
			{
				panel3.Visible = true;
				panel3.Enabled = true;
				CommercialInvoiceDataByInvoiceId(Conversion.ToInt(Cmbinvoiceno.Value));
			}
			else
			{
				grdDetail.ClearStructure();
				dtdetail.Clear();
				panel3.Visible = false;
				panel3.Enabled = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdDetail).Name);
			if (fs != null)
			{
				grdDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
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

	private void btnAttachment_Click(object sender, EventArgs e)
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

	private void btnupdate_Click(object sender, EventArgs e)
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
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		try
		{
			btnsave.Visible = false;
			btnupdate.Visible = true;
			btnDelete.Visible = formrights.DoHaveCanDelete;
			btnDelete.Enabled = formrights.DoHaveCanDelete;
			ExImInvoice PIH = new ExImInvoice();
			RecId = Id;
			PIH = ExImInvoice.GetByID(Id);
			if (PIH != null)
			{
				DataTable dtinv = new DataTable();
				dtinv.Columns.Add("Id");
				dtinv.Columns.Add("InvoiceNo");
				dtinv.Rows.Add(PIH.Id, PIH.InvoiceNo);
				if (dtinv.Rows.Count > 0)
				{
					DDL.BindDDL(dtinv, Cmbinvoiceno, "Id", "InvoiceNo", "Commercial Invoice_Number", ZeroIndex: false);
					((UltraGridBase)Cmbinvoiceno).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				}
				tabControl1.SelectedIndex = 0;
				txtdocno.Text = PIH.DocCode.ToString();
				txtDocDate.Value = Conversion.ToDateTime((object)PIH.DocDate);
				cmbSupCust.Value = PIH.SupplierCustomerId;
				cmbDestinationPort.Value = PIH.DestinationPortId;
				cmbOtherCustomer.Value = PIH.OtherCustomerId;
				cmbOtherDestinationPort.Value = PIH.OtherDestinationPortId;
				txtLotRef.Text = PIH.LotNoRef;
				cmbdeliverytermnew.Value = PIH.DeliveryTermId;
				cmbLoadingPort.Value = PIH.LoadingPortId;
				cmbDestinationPort.Value = PIH.DestinationPortId;
				Cmbinvoiceno.Value = PIH.Id;
				((Control)(object)cmbcareiertype).Text = PIH.CarierType;
				cmbfcycode.Value = PIH.FcurrencyId;
				txtGrossWeight.Text = PIH.GrossWeight.ToString();
				txtNetWeight.Text = PIH.NetWeight.ToString();
				txtNoOfContainer.Text = PIH.NoOfContainers.ToString();
				txtGdNo.Text = PIH.EFormNo;
				txtIformDate.Value = PIH.EFormDate;
				double TotalQty = 0.0;
				dtdetail.Rows.Clear();
				panel3.Visible = true;
				for (int i = 0; i < PIH.ExImPackingListDetailList.Count; i++)
				{
					dtdetail.Rows.Add(PIH.ExImPackingListDetailList[i].Id, PIH.ExImPackingListDetailList[i].ExImInvoiceId, PIH.ExImPackingListDetailList[i].ExImInvoicePackingDetailId, PIH.ExImPackingListDetailList[i].RefDocumentTypeId, PIH.ExImPackingListDetailList[i].ItemId, PIH.ExImPackingListDetailList[i].ItemName, PIH.ExImPackingListDetailList[i].ExImPackMaterilaTypeId, PIH.ExImPackingListDetailList[i].PackingMaterial, PIH.ExImPackingListDetailList[i].InvCropYearId, PIH.ExImPackingListDetailList[i].CropYear, PIH.ExImPackingListDetailList[i].WarehouseId, PIH.ExImPackingListDetailList[i].JobLotid, PIH.ExImPackingListDetailList[i].OuterQty, PIH.ExImPackingListDetailList[i].UOMScheduleIdOuter, PIH.ExImPackingListDetailList[i].OuterUomCode, PIH.ExImPackingListDetailList[i].OuterUomEquivalent, PIH.ExImPackingListDetailList[i].NetWeight, PIH.ExImPackingListDetailList[i].PackingWeight, PIH.ExImPackingListDetailList[i].PackingWeightTotal, PIH.ExImPackingListDetailList[i].GrossWeight, PIH.ExImPackingListDetailList[i].ContainerNo, PIH.ExImPackingListDetailList[i].SealNo, PIH.ExImPackingListDetailList[i].RatePrice, PIH.ExImPackingListDetailList[i].RateUomId);
					TotalQty += PIH.ExImPackingListDetailList[i].OuterQty;
				}
				txttotalQtyHeaderForConditions.Text = TotalQty.ToString();
				grdDetail.DataSource = dtdetail;
				grdDetail.RetrieveStructure();
				grdSettings();
				((Control)(object)Cmbinvoiceno).Enabled = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Expected O, but got Unknown
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Expected O, but got Unknown
		//IL_07da: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e1: Expected O, but got Unknown
		try
		{
			if (grdDetail.GetRows().Count() <= 0)
			{
				MessageBox.Show("Grid Record not found");
				return;
			}
			if (RecId == 0)
			{
				if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
			}
			else if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			double Netweight = 0.0;
			double Grossweight = 0.0;
			double Qty = 0.0;
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow item in rows)
			{
				Netweight += Conversion.ToDouble(item.Cells["NetWeight"].Value);
				Grossweight += Conversion.ToDouble((object)Conversion.ToDouble(item.Cells["GrossWeight"].Value).ToString("#,##0.##"));
				Qty += Conversion.ToDouble(item.Cells["OuterQty"].Value);
			}
			double value = Math.Round(Netweight) - Math.Round(Conversion.ToDouble((object)txtNetWeight.Text), 2);
			double diffGross = Math.Round(Grossweight) - Math.Round(Conversion.ToDouble((object)txtGrossWeight.Text), 2);
			if (Math.Abs(value) > 0.99)
			{
				MessageBox.Show("Total NetWeight Of Detail grid Can not be greater than Invoice NetWeight...");
				return;
			}
			if (Math.Abs(diffGross) > 0.99)
			{
				MessageBox.Show("Total GrossWeight Of Detail grid Can not be greater than Invoice GrossWeight...");
				return;
			}
			ExImPackingListDetail PIH = new ExImPackingListDetail();
			if (RecId > 0)
			{
				PIH.Id = RecId;
			}
			PIH.ExImPackingListDetailList = new List<ExImPackingListDetail>();
			rows = grdDetail.GetRows();
			foreach (GridEXRow r in rows)
			{
				ExImPackingListDetail vd = new ExImPackingListDetail();
				vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
				vd.ExImInvoiceId = Conversion.ToInt(r.Cells["CommercialInvoiceId"].Value);
				vd.RefDocumentTypeId = 211;
				vd.ExImInvoicePackingDetailId = Conversion.ToInt(r.Cells["CommercialInvoiceDetailId"].Value);
				if (Conversion.ToInt(r.Cells["ItemId"].Value) == 0)
				{
					MessageBox.Show("Item Field is Required in Grid...\n In Row No" + r.RowIndex + 1);
					return;
				}
				vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				if (Conversion.ToInt(r.Cells["CropYearId"].Value) == 0)
				{
					MessageBox.Show("CropYear Field is Required in Grid...\n In Row No" + r.RowIndex + 1);
					return;
				}
				vd.InvCropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
				vd.CropYear = Conversion.ToString(r.Cells["CropYear"].Value);
				if (Conversion.ToInt(r.Cells["JobLotId"].Value) == 0)
				{
					MessageBox.Show("JobLot Field is Required in Grid...\n In Row No" + r.RowIndex + 1);
					return;
				}
				vd.JobLotid = Conversion.ToInt(r.Cells["JobLotId"].Value);
				if (Conversion.ToInt(r.Cells["PackTypeId"].Value) == 0)
				{
					MessageBox.Show("PackType Field is Required in Grid...\n In Row No" + r.RowIndex + 1);
					return;
				}
				vd.ExImPackMaterilaTypeId = Conversion.ToInt(r.Cells["PackTypeId"].Value);
				if (Conversion.ToDouble(r.Cells["OuterQty"].Value) == 0.0)
				{
					MessageBox.Show("OuterQty Field is Required in Grid...\n In Row No" + r.RowIndex + 1);
					return;
				}
				vd.InnerQty = Conversion.ToDouble(r.Cells["OuterQty"].Value);
				vd.UOMScheduleIdInner = Conversion.ToInt(r.Cells["UOMScheduleIdOuter"].Value);
				vd.OuterQty = Conversion.ToDouble(r.Cells["OuterQty"].Value);
				if (Conversion.ToInt(r.Cells["UOMScheduleIdOuter"].Value) == 0)
				{
					MessageBox.Show("OuterUom Field is Required in Grid...\n In Row No" + r.RowIndex + 1);
					return;
				}
				vd.UOMScheduleIdOuter = Conversion.ToInt(r.Cells["UOMScheduleIdOuter"].Value);
				if (Conversion.ToDouble(r.Cells["NetWeight"].Value) == 0.0)
				{
					MessageBox.Show("NetWeight Field is Required in Grid...\n In Row No" + r.RowIndex + 1);
					return;
				}
				vd.NetWeight = Conversion.ToDouble(r.Cells["NetWeight"].Value);
				vd.PackingWeight = Conversion.ToDouble(r.Cells["PackingWeight"].Value);
				vd.PackingWeightTotal = Conversion.ToDouble(r.Cells["PackingWeightTotal"].Value);
				if (Conversion.ToDouble(r.Cells["GrossWeight"].Value) == 0.0)
				{
					MessageBox.Show("GrossWeight Field is Required in Grid...\n In Row No" + r.RowIndex + 1);
					return;
				}
				vd.GrossWeight = Conversion.ToDouble(r.Cells["GrossWeight"].Value);
				vd.ContainerNo = Conversion.ToString((object)r.Cells["ContainerNo"].Text);
				vd.SealNo = Conversion.ToString((object)r.Cells["SealNo"].Text);
				if (Conversion.ToInt(r.Cells["WarehouseId"].Value) == 0)
				{
					MessageBox.Show("Warehouse Field is Required in Grid...\n In Row No" + r.RowIndex + 1);
					return;
				}
				vd.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
				vd.RatePrice = Conversion.ToDouble(r.Cells["RatePrice"].Value);
				vd.RateUomId = Conversion.ToInt(r.Cells["RateUomId"].Value);
				vd.SortNo = Conversion.ToInt((object)1);
				if (vd.Id > 0)
				{
					vd.ActionTypeId = 2;
				}
				else
				{
					vd.ActionTypeId = 1;
				}
				PIH.ExImPackingListDetailList.Add(vd);
			}
			PIH.AttachmentsList = new List<DMSAttachments>();
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Path.GetFileName(Attachment.dtAttachments.Rows[i][1].ToString());
				attach.ScreenName = base.Name;
				PIH.AttachmentsList.Add(attach);
				string fileSavePath = Path.Combine(clsGlobalVariables.AttachmentFolderPath, attach.Attachment);
				if (!File.Exists(fileSavePath))
				{
					File.Copy(Attachment.dtAttachments.Rows[i][1].ToString(), fileSavePath, overwrite: true);
				}
			}
			int success = ExImInvoice.SetDataForCommercialInvoicePackingList(PIH, UserAccount.OrganizationId, UserAccount.CompanyId, 211, Conversion.ToInt(Cmbinvoiceno.Value), Conversion.ToDateTime((object)txtDocDate.Value), Conversion.ToInt((object)txtdocno.Text), Conversion.ToInt(cmbSupCust.Value), UserAccount.BranchesId, UserAccount.ID, ExchangeRate, clsGlobalVariables.ActiveYr.Id);
			success = Conversion.ToInt(Cmbinvoiceno.Value);
			if (RecId == 0)
			{
				MessageBox.Show("Save SuccessFully");
			}
			if (RecId > 0)
			{
				MessageBox.Show("Update SuccessFully");
			}
			FormReset();
			if (ChkPrintPreview.Checked)
			{
				CommonServices.ExportCommercialInvoicePackingdetailListSlip(success);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			HistoryGridFill(50);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				ReadById(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
			}
			if (e.Column.Key == "Slip")
			{
				CommonServices.ExportCommercialInvoicePackingdetailListSlip(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			GetDetailByHeaderId();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailByHeaderId()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Expected O, but got Unknown
		try
		{
			if (DataGridHistory.CurrentRow == null || (int)DataGridHistory.CurrentRow.RowType != 0 || DataGridHistory.CurrentRow.Cells.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("ItemId", typeof(int));
			dt.Columns.Add("ItemName");
			dt.Columns.Add("PackType");
			dt.Columns.Add("CropYear");
			dt.Columns.Add("WarehouseName", typeof(string));
			dt.Columns.Add("JobLot", typeof(string));
			dt.Columns.Add("OuterQty", typeof(double));
			dt.Columns.Add("OuterPackUOM", typeof(string));
			dt.Columns.Add("NetWeight", typeof(double));
			dt.Columns.Add("PackingWeight", typeof(double));
			dt.Columns.Add("PackingWeightTotal", typeof(double));
			dt.Columns.Add("GrossWeight", typeof(double));
			dt.Columns.Add("ContainerNo");
			dt.Columns.Add("SealNo");
			ExImInvoice PIH = new ExImInvoice();
			PIH = ExImInvoice.GetByID(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
			if (PIH.ExImPackingListDetailList.Count > 0)
			{
				for (int i = 0; i < PIH.ExImPackingListDetailList.Count; i++)
				{
					dt.Rows.Add(PIH.ExImPackingListDetailList[i].Id, PIH.ExImPackingListDetailList[i].ItemId, PIH.ExImPackingListDetailList[i].ItemName, PIH.ExImPackingListDetailList[i].PackingMaterial, PIH.ExImPackingListDetailList[i].CropYear, PIH.ExImPackingListDetailList[i].WareHouseName, PIH.ExImPackingListDetailList[i].JobLotDescription, PIH.ExImPackingListDetailList[i].OuterQty, PIH.ExImPackingListDetailList[i].OuterUomCode, PIH.ExImPackingListDetailList[i].NetWeight, PIH.ExImPackingListDetailList[i].PackingWeight, PIH.ExImPackingListDetailList[i].PackingWeightTotal, PIH.ExImPackingListDetailList[i].GrossWeight, PIH.ExImPackingListDetailList[i].ContainerNo, PIH.ExImPackingListDetailList[i].SealNo);
				}
				gridDetailHistory.DataSource = dt;
				gridDetailHistory.RetrieveStructure();
				gridDetailHistory.RootTable.Columns["Id"].Visible = false;
				gridDetailHistory.RootTable.Columns["ItemId"].Visible = false;
				CommonServices.GridAutoAdjustment(AryColWidth, gridDetailHistory);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			ReadById(Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString()));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridFill(int NoofRecords = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		try
		{
			DataTable dtgrd = ExImInvoice.FormHistoryForPackingDetailList(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				NoOfRecords = NoofRecords,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Ids = "211"
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("VoucherHeadId");
			dt.Columns.Add("InvoiceNo");
			dt.Columns.Add("DocCode", typeof(int));
			dt.Columns.Add("DocDate", typeof(DateTime));
			dt.Columns.Add("CustomerName");
			dt.Columns.Add("NoOfContainers", typeof(double));
			dt.Columns.Add("GrossWeight", typeof(double));
			dt.Columns.Add("NetWeight", typeof(double));
			dt.Columns.Add("FcyAmount", typeof(double));
			dt.Columns.Add("Add/Less", typeof(double));
			dt.Columns.Add("TotalAmount", typeof(double));
			dt.Columns.Add("LoadingPort");
			dt.Columns.Add("DestinationPort");
			dt.Columns.Add("NoOfAttachments");
			for (int i = 0; i < dtgrd.Rows.Count; i++)
			{
				dt.Rows.Add(dtgrd.Rows[i]["Id"], dtgrd.Rows[i]["VoucherHeadId"], dtgrd.Rows[i]["InvoiceNo"], dtgrd.Rows[i]["DocCode"], Conversion.ToDateTime(dtgrd.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dtgrd.Rows[i]["Customer"], dtgrd.Rows[i]["NoOfContainers"], dtgrd.Rows[i]["GrossWeight"], dtgrd.Rows[i]["NetWeight"], dtgrd.Rows[i]["FCurrencyAmount"], dtgrd.Rows[i]["AddLessAmount"], dtgrd.Rows[i]["TotalAmount"], dtgrd.Rows[i]["LoadingPort"], dtgrd.Rows[i]["DestinationPort"], dtgrd.Rows[i]["NoOfAttachments"]);
			}
			DataGridHistory.DataSource = dt;
			DataGridHistory.RetrieveStructure();
			HistoryGridSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSettings()
	{
		try
		{
			DataGridHistory.AllowEdit = (InheritableBoolean)2;
			DataGridHistory.AllowDelete = (InheritableBoolean)2;
			DataGridHistory.RootTable.Columns["VoucherHeadId"].Visible = false;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].Visible = false;
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["Add/Less"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["NoOfContainers"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["TotalAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["Add/Less"].TotalFormatString = "0,0";
			DataGridHistory.RootTable.Columns["NoOfContainers"].TotalFormatString = "0,0";
			DataGridHistory.RootTable.Columns["GrossWeight"].TotalFormatString = "#,#.###";
			DataGridHistory.RootTable.Columns["NetWeight"].TotalFormatString = "#,#.###";
			DataGridHistory.RootTable.Columns["FcyAmount"].TotalFormatString = "#,#.###";
			DataGridHistory.RootTable.Columns["TotalAmount"].TotalFormatString = "#,#.###";
			DataGridHistory.RootTable.Columns["Add/Less"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["NoOfContainers"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["GrossWeight"].FormatString = "#,#.###";
			DataGridHistory.RootTable.Columns["NetWeight"].FormatString = "#,#.###";
			DataGridHistory.RootTable.Columns["FcyAmount"].FormatString = "#,#.###";
			DataGridHistory.RootTable.Columns["TotalAmount"].FormatString = "#,#.###";
			DataGridHistory.RootTable.Columns.Add("Edit");
			DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Width = 70;
			DataGridHistory.RootTable.Columns["Edit"].Position = 0;
			DataGridHistory.RootTable.Columns.Add("Slip");
			DataGridHistory.RootTable.Columns["Slip"].Caption = "Slip";
			DataGridHistory.RootTable.Columns["Slip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Slip"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Slip"].ButtonText = "Slip";
			DataGridHistory.RootTable.Columns["Slip"].Width = 70;
			DataGridHistory.RootTable.Columns["Slip"].Position = 1;
			DataGridHistory.FrozenColumns = 2;
			CommonServices.GridAutoAdjustment(AryColWidth, DataGridHistory);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadAll_Click(object sender, EventArgs e)
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

	private void btnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.ExportCommercialInvoicePackingdetailListSlip(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackingDetailForCommercialInvoice_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0 && !UpdateMode)
			{
				Insert();
			}
			if (e.Control && e.KeyCode == Keys.N && tabControl1.SelectedIndex == 0)
			{
				FormReset();
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
			if (e.Control && e.KeyCode == Keys.U && tabControl1.SelectedIndex == 0)
			{
				Insert();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		try
		{
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					InvPurchaseInvoice.RemoveByID(new InvPurchaseInvoice
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						EntryUser = UserAccount.ID,
						DocumentTypeId = 211,
						Id = RecId
					});
					MessageBox.Show("Delete Export Packing list Successfully");
					FormReset();
				}
			}
			else
			{
				MessageBox.Show("Record Not Found For Deletion");
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
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Expected O, but got Unknown
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Expected O, but got Unknown
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Expected O, but got Unknown
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Expected O, but got Unknown
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Expected O, but got Unknown
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Expected O, but got Unknown
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Expected O, but got Unknown
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Expected O, but got Unknown
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Expected O, but got Unknown
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Expected O, but got Unknown
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Expected O, but got Unknown
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Expected O, but got Unknown
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Expected O, but got Unknown
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Expected O, but got Unknown
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Expected O, but got Unknown
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Expected O, but got Unknown
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Expected O, but got Unknown
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Expected O, but got Unknown
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Expected O, but got Unknown
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Expected O, but got Unknown
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Expected O, but got Unknown
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Expected O, but got Unknown
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Expected O, but got Unknown
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Expected O, but got Unknown
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Expected O, but got Unknown
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dc: Expected O, but got Unknown
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e7: Expected O, but got Unknown
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_043f: Expected O, but got Unknown
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Expected O, but got Unknown
		//IL_0461: Unknown result type (might be due to invalid IL or missing references)
		//IL_046b: Expected O, but got Unknown
		//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c3: Expected O, but got Unknown
		//IL_0548: Unknown result type (might be due to invalid IL or missing references)
		//IL_0552: Expected O, but got Unknown
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_0573: Expected O, but got Unknown
		//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c0: Expected O, but got Unknown
		//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cb: Expected O, but got Unknown
		//IL_09d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09da: Expected O, but got Unknown
		//IL_09e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f1: Expected O, but got Unknown
		//IL_4315: Unknown result type (might be due to invalid IL or missing references)
		//IL_431f: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
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
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
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
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		GridEXLayout gridDetailHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Export.PackingDetailForCommercialInvoice));
		Appearance appearance208 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
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
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grdDetail = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.label5 = new System.Windows.Forms.Label();
		this.cmbSupCust = new UltraCombo();
		this.label21 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.txtNoOfContainer = new System.Windows.Forms.TextBox();
		this.label55 = new System.Windows.Forms.Label();
		this.txtNetWeight = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.cmbLoadingPort = new UltraCombo();
		this.cmbDestinationPort = new UltraCombo();
		this.label56 = new System.Windows.Forms.Label();
		this.txtDocDate = new System.Windows.Forms.DateTimePicker();
		this.label12 = new System.Windows.Forms.Label();
		this.txtIformDate = new System.Windows.Forms.DateTimePicker();
		this.txtGdNo = new System.Windows.Forms.TextBox();
		this.label46 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.cmbdeliverytermnew = new UltraCombo();
		this.cmbfcycode = new UltraCombo();
		this.label27 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.cmbcareiertype = new UltraCombo();
		this.txtLotRef = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txttotalQtyHeaderForConditions = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.ChkPrintPreview = new System.Windows.Forms.CheckBox();
		this.Cmbinvoiceno = new UltraCombo();
		this.label67 = new System.Windows.Forms.Label();
		this.label68 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.toolStrip4 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnrefersh = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.gridDetailHistory = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label89 = new System.Windows.Forms.Label();
		this.DataGridHistory = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.label69 = new System.Windows.Forms.Label();
		this.cmbOtherCustomer = new UltraCombo();
		this.cmbOtherDestinationPort = new UltraCombo();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel2.SuspendLayout();
		this.panel3.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbSupCust).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLoadingPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDestinationPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdeliverytermnew).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbfcycode).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcareiertype).BeginInit();
		this.panel1.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.Cmbinvoiceno).BeginInit();
		this.panel10.SuspendLayout();
		this.toolStrip4.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.gridDetailHistory).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel15.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbOtherCustomer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbOtherDestinationPort).BeginInit();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(964, 661);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Controls.Add(this.panel10);
		this.tabPage1.Controls.Add(this.toolStrip4);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(956, 632);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(3, 294);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdDetail).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(950, 335);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 60;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.CellUpdated += new ColumnActionEventHandler(grdDetail_CellUpdated);
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.ctrlGrdBar2);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 268);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(950, 26);
		this.panel2.TabIndex = 59;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(3, 3);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(231, 18);
		this.label1.TabIndex = 25;
		this.label1.Text = "Detail Of Commercial Invoice ";
		this.panel3.Controls.Add(this.groupBox3);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 120);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(950, 148);
		this.panel3.TabIndex = 58;
		this.groupBox3.Controls.Add(this.label5);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupCust);
		this.groupBox3.Controls.Add(this.label21);
		this.groupBox3.Controls.Add(this.label57);
		this.groupBox3.Controls.Add(this.txtNoOfContainer);
		this.groupBox3.Controls.Add(this.label55);
		this.groupBox3.Controls.Add(this.txtNetWeight);
		this.groupBox3.Controls.Add(this.label16);
		this.groupBox3.Controls.Add(this.label54);
		this.groupBox3.Controls.Add(this.txtGrossWeight);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLoadingPort);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDestinationPort);
		this.groupBox3.Controls.Add(this.label56);
		this.groupBox3.Controls.Add(this.txtDocDate);
		this.groupBox3.Controls.Add(this.label12);
		this.groupBox3.Controls.Add(this.txtIformDate);
		this.groupBox3.Controls.Add(this.txtGdNo);
		this.groupBox3.Controls.Add(this.label46);
		this.groupBox3.Controls.Add(this.txtdocno);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbdeliverytermnew);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbfcycode);
		this.groupBox3.Controls.Add(this.label27);
		this.groupBox3.Controls.Add(this.label7);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcareiertype);
		this.groupBox3.Controls.Add(this.txtLotRef);
		this.groupBox3.Controls.Add(this.label22);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(4, 3);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(938, 137);
		this.groupBox3.TabIndex = 2;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Invoice Information";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(6, 23);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(88, 15);
		this.label5.TabIndex = 592;
		this.label5.Text = "Doc No && Date";
		this.cmbSupCust.AllowNull = (DefaultableBoolean)1;
		this.cmbSupCust.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupCust.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupCust.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Appearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance197).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance197).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupCust).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance197;
		((AppearanceBase)appearance198).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance198;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupCust).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance199).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance199).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance199).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance200;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance202;
		((AppearanceBase)appearance203).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance203).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance204).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance204).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance204).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance204).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance204;
		((AppearanceBase)appearance205).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance205;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupCust).Enabled = false;
		((System.Windows.Forms.Control)(object)this.cmbSupCust).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbSupCust.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupCust).Location = new System.Drawing.Point(123, 46);
		((System.Windows.Forms.Control)(object)this.cmbSupCust).Name = "cmbSupCust";
		((System.Windows.Forms.Control)(object)this.cmbSupCust).Size = new System.Drawing.Size(264, 26);
		((System.Windows.Forms.Control)(object)this.cmbSupCust).TabIndex = 4;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(6, 52);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(116, 15);
		this.label21.TabIndex = 78;
		this.label21.Text = "Customer / Importer";
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label57.Location = new System.Drawing.Point(669, 52);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(93, 15);
		this.label57.TabIndex = 552;
		this.label57.Text = "No.Of Container";
		this.txtNoOfContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNoOfContainer.Enabled = false;
		this.txtNoOfContainer.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNoOfContainer.Location = new System.Drawing.Point(766, 48);
		this.txtNoOfContainer.Name = "txtNoOfContainer";
		this.txtNoOfContainer.Size = new System.Drawing.Size(163, 23);
		this.txtNoOfContainer.TabIndex = 26;
		this.txtNoOfContainer.Text = "0";
		this.txtNoOfContainer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label55.Location = new System.Drawing.Point(669, 110);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(86, 15);
		this.label55.TabIndex = 554;
		this.label55.Text = "Net Weight KG";
		this.txtNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeight.Enabled = false;
		this.txtNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeight.Location = new System.Drawing.Point(766, 106);
		this.txtNetWeight.Name = "txtNetWeight";
		this.txtNetWeight.ReadOnly = true;
		this.txtNetWeight.Size = new System.Drawing.Size(163, 23);
		this.txtNetWeight.TabIndex = 28;
		this.txtNetWeight.Text = "0";
		this.txtNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label16.AutoSize = true;
		this.label16.BackColor = System.Drawing.Color.White;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(396, 110);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(93, 15);
		this.label16.TabIndex = 533;
		this.label16.Text = "Destination Port";
		this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label54.Location = new System.Drawing.Point(669, 81);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(96, 15);
		this.label54.TabIndex = 553;
		this.label54.Text = "Gross Weight KG";
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Enabled = false;
		this.txtGrossWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrossWeight.Location = new System.Drawing.Point(766, 77);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.Size = new System.Drawing.Size(163, 23);
		this.txtGrossWeight.TabIndex = 27;
		this.txtGrossWeight.Text = "0";
		this.txtGrossWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.cmbLoadingPort.AllowNull = (DefaultableBoolean)1;
		this.cmbLoadingPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLoadingPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLoadingPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance123;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance128).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Enabled = false;
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLoadingPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Location = new System.Drawing.Point(495, 75);
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Name = "cmbLoadingPort";
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Size = new System.Drawing.Size(163, 26);
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).TabIndex = 21;
		this.cmbDestinationPort.AllowNull = (DefaultableBoolean)1;
		this.cmbDestinationPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDestinationPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDestinationPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance135;
		((SpecialBoxBase)((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Enabled = false;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDestinationPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Location = new System.Drawing.Point(495, 104);
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Name = "cmbDestinationPort";
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Size = new System.Drawing.Size(163, 26);
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).TabIndex = 22;
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label56.Location = new System.Drawing.Point(396, 81);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(75, 15);
		this.label56.TabIndex = 534;
		this.label56.Text = "Loading Port";
		this.txtDocDate.CustomFormat = "dd-MMM-yy";
		this.txtDocDate.Enabled = false;
		this.txtDocDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocDate.Location = new System.Drawing.Point(225, 19);
		this.txtDocDate.Name = "txtDocDate";
		this.txtDocDate.Size = new System.Drawing.Size(162, 23);
		this.txtDocDate.TabIndex = 1;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(396, 52);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(80, 15);
		this.label12.TabIndex = 526;
		this.label12.Text = "Delivery Term";
		this.txtIformDate.CustomFormat = "dd-MMM-yy";
		this.txtIformDate.Enabled = false;
		this.txtIformDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtIformDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtIformDate.Location = new System.Drawing.Point(274, 106);
		this.txtIformDate.Name = "txtIformDate";
		this.txtIformDate.Size = new System.Drawing.Size(113, 23);
		this.txtIformDate.TabIndex = 20;
		this.txtGdNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGdNo.Enabled = false;
		this.txtGdNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtGdNo.Location = new System.Drawing.Point(123, 106);
		this.txtGdNo.Name = "txtGdNo";
		this.txtGdNo.Size = new System.Drawing.Size(145, 23);
		this.txtGdNo.TabIndex = 19;
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label46.Location = new System.Drawing.Point(6, 110);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(83, 15);
		this.label46.TabIndex = 561;
		this.label46.Text = "GD No && Date";
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Enabled = false;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtdocno.Location = new System.Drawing.Point(123, 19);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(101, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.TabStop = false;
		this.cmbdeliverytermnew.AllowNull = (DefaultableBoolean)1;
		this.cmbdeliverytermnew.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbdeliverytermnew.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbdeliverytermnew.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Appearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance111;
		((SpecialBoxBase)((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance116).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbdeliverytermnew).Enabled = false;
		((System.Windows.Forms.Control)(object)this.cmbdeliverytermnew).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbdeliverytermnew.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbdeliverytermnew).Location = new System.Drawing.Point(495, 46);
		((System.Windows.Forms.Control)(object)this.cmbdeliverytermnew).Name = "cmbdeliverytermnew";
		((System.Windows.Forms.Control)(object)this.cmbdeliverytermnew).Size = new System.Drawing.Size(163, 26);
		((System.Windows.Forms.Control)(object)this.cmbdeliverytermnew).TabIndex = 12;
		this.cmbfcycode.AllowNull = (DefaultableBoolean)1;
		this.cmbfcycode.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbfcycode.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbfcycode.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Appearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance158).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance158).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbfcycode).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance159).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance159;
		((SpecialBoxBase)((UltraGridBase)this.cmbfcycode).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance160).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance164).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance165).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance165).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance165).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbfcycode).Enabled = false;
		((System.Windows.Forms.Control)(object)this.cmbfcycode).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbfcycode.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbfcycode).Location = new System.Drawing.Point(766, 17);
		((System.Windows.Forms.Control)(object)this.cmbfcycode).Name = "cmbfcycode";
		((System.Windows.Forms.Control)(object)this.cmbfcycode).Size = new System.Drawing.Size(163, 26);
		((System.Windows.Forms.Control)(object)this.cmbfcycode).TabIndex = 15;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label27.Location = new System.Drawing.Point(672, 23);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(55, 15);
		this.label27.TabIndex = 537;
		this.label27.Text = "Fcy Code";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(6, 81);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(66, 15);
		this.label7.TabIndex = 57;
		this.label7.Text = "Lot Ref No.";
		this.cmbcareiertype.AllowNull = (DefaultableBoolean)1;
		this.cmbcareiertype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcareiertype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcareiertype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance146).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance146).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcareiertype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance147;
		((SpecialBoxBase)((UltraGridBase)this.cmbcareiertype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance148).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance148).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance152).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance153).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcareiertype).Enabled = false;
		((System.Windows.Forms.Control)(object)this.cmbcareiertype).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcareiertype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcareiertype).Location = new System.Drawing.Point(495, 17);
		((System.Windows.Forms.Control)(object)this.cmbcareiertype).Name = "cmbcareiertype";
		((System.Windows.Forms.Control)(object)this.cmbcareiertype).Size = new System.Drawing.Size(163, 26);
		((System.Windows.Forms.Control)(object)this.cmbcareiertype).TabIndex = 13;
		this.txtLotRef.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLotRef.Enabled = false;
		this.txtLotRef.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtLotRef.Location = new System.Drawing.Point(123, 77);
		this.txtLotRef.Name = "txtLotRef";
		this.txtLotRef.Size = new System.Drawing.Size(264, 23);
		this.txtLotRef.TabIndex = 14;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(396, 23);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(65, 15);
		this.label22.TabIndex = 542;
		this.label22.Text = "Carier Type";
		this.panel1.Controls.Add(this.groupBox1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 56);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(950, 64);
		this.panel1.TabIndex = 7;
		this.groupBox1.Controls.Add(this.txttotalQtyHeaderForConditions);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.ChkPrintPreview);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.Cmbinvoiceno);
		this.groupBox1.Controls.Add(this.label67);
		this.groupBox1.Controls.Add(this.label68);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(5, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(754, 52);
		this.groupBox1.TabIndex = 58;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filter Commercial Invoice";
		this.txttotalQtyHeaderForConditions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txttotalQtyHeaderForConditions.Enabled = false;
		this.txttotalQtyHeaderForConditions.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txttotalQtyHeaderForConditions.Location = new System.Drawing.Point(645, 19);
		this.txttotalQtyHeaderForConditions.Name = "txttotalQtyHeaderForConditions";
		this.txttotalQtyHeaderForConditions.Size = new System.Drawing.Size(101, 23);
		this.txttotalQtyHeaderForConditions.TabIndex = 593;
		this.txttotalQtyHeaderForConditions.Visible = false;
		this.label2.AutoSize = true;
		this.label2.Enabled = false;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(584, 22);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(55, 15);
		this.label2.TabIndex = 594;
		this.label2.Text = "Total Qty";
		this.label2.Visible = false;
		this.ChkPrintPreview.AutoSize = true;
		this.ChkPrintPreview.Checked = true;
		this.ChkPrintPreview.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkPrintPreview.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.ChkPrintPreview.Location = new System.Drawing.Point(457, 22);
		this.ChkPrintPreview.Name = "ChkPrintPreview";
		this.ChkPrintPreview.Size = new System.Drawing.Size(67, 19);
		this.ChkPrintPreview.TabIndex = 58;
		this.ChkPrintPreview.Text = "Preview";
		this.ChkPrintPreview.UseVisualStyleBackColor = true;
		this.Cmbinvoiceno.AllowNull = (DefaultableBoolean)1;
		this.Cmbinvoiceno.AutoCompleteMode = (AutoCompleteMode)4;
		this.Cmbinvoiceno.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.Cmbinvoiceno.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance184).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.Appearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance185).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance185).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance185;
		((AppearanceBase)appearance186).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance186;
		((SpecialBoxBase)((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance187).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance187).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance188;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance190;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance191).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance192).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance192).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance192).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance192).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).TextHAlignAsString = "Left";
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance194;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.Cmbinvoiceno).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.Cmbinvoiceno).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.Cmbinvoiceno.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.Cmbinvoiceno).Location = new System.Drawing.Point(88, 19);
		((System.Windows.Forms.Control)(object)this.Cmbinvoiceno).Name = "Cmbinvoiceno";
		((System.Windows.Forms.Control)(object)this.Cmbinvoiceno).Size = new System.Drawing.Size(292, 26);
		((System.Windows.Forms.Control)(object)this.Cmbinvoiceno).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.Cmbinvoiceno).TextChanged += new System.EventHandler(Cmbinvoiceno_TextChanged);
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label67.Location = new System.Drawing.Point(6, 25);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(65, 15);
		this.label67.TabIndex = 57;
		this.label67.Text = "Invoice No";
		this.label68.AutoSize = true;
		this.label68.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label68.ForeColor = System.Drawing.Color.Red;
		this.label68.Location = new System.Drawing.Point(72, 24);
		this.label68.Name = "label68";
		this.label68.Size = new System.Drawing.Size(14, 17);
		this.label68.TabIndex = 50;
		this.label68.Text = "*";
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.label23);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 30);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(950, 26);
		this.panel10.TabIndex = 5;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(3, 3);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(300, 19);
		this.label23.TabIndex = 25;
		this.label23.Text = "Packing List for Commercial Invoice";
		this.toolStrip4.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip4.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.btnNew, this.btnsave, this.btnupdate, this.btnrefersh, this.btnAttachment, this.btnPrint, this.btnDelete });
		this.toolStrip4.Location = new System.Drawing.Point(3, 3);
		this.toolStrip4.Name = "toolStrip4";
		this.toolStrip4.Size = new System.Drawing.Size(950, 27);
		this.toolStrip4.TabIndex = 6;
		this.toolStrip4.Text = "toolStrip4";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnrefersh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnrefersh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnrefersh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnrefersh.Name = "btnrefersh";
		this.btnrefersh.Size = new System.Drawing.Size(80, 24);
		this.btnrefersh.Text = "&Refresh";
		this.btnrefersh.Click += new System.EventHandler(btnrefersh_Click);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Visible = false;
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(92, 24);
		this.btnPrint.Text = "Print-550";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.gridDetailHistory);
		this.tabPage2.Controls.Add(this.panel4);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.panel15);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(956, 632);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.gridDetailHistory.AllowEdit = (InheritableBoolean)2;
		gridDetailHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.gridDetailHistory.DesignTimeLayout = gridDetailHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.gridDetailHistory.DynamicFiltering = true;
		this.gridDetailHistory.FilterMode = (FilterMode)2;
		this.gridDetailHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.gridDetailHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Location = new System.Drawing.Point(3, 479);
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Name = "gridDetailHistory";
		((ScrollableBase)this.gridDetailHistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.gridDetailHistory).Office2007CustomColor = System.Drawing.Color.LimeGreen;
		((ScrollableBase)this.gridDetailHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Size = new System.Drawing.Size(950, 150);
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).TabIndex = 56;
		this.gridDetailHistory.TotalRow = (InheritableBoolean)1;
		this.gridDetailHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.gridDetailHistory).VisualStyle = (VisualStyle)4;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label89);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 452);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(950, 27);
		this.panel4.TabIndex = 55;
		this.label89.AutoSize = true;
		this.label89.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label89.ForeColor = System.Drawing.Color.White;
		this.label89.Location = new System.Drawing.Point(5, 6);
		this.label89.Name = "label89";
		this.label89.Size = new System.Drawing.Size(211, 17);
		this.label89.TabIndex = 46;
		this.label89.Text = "Detail Of Above Selected Row";
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Top;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(3, 59);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.DataGridHistory).Office2007CustomColor = System.Drawing.Color.LimeGreen;
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(950, 393);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 54;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		this.panel15.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel15.Controls.Add(this.toolStrip3);
		this.panel15.Controls.Add(this.ctrlGrdBar1);
		this.panel15.Controls.Add(this.label69);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(3, 3);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(950, 56);
		this.panel15.TabIndex = 53;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnLoadAll, this.toolStripButton7, this.toolStripSeparator1 });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(950, 27);
		this.toolStrip3.TabIndex = 58;
		this.toolStrip3.Text = "toolStrip2";
		this.btnLoadAll.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadAll.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(77, 24);
		this.btnLoadAll.Text = "LoadAll";
		this.btnLoadAll.Click += new System.EventHandler(btnLoadAll_Click);
		this.toolStripButton7.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton7.Image = (System.Drawing.Image)resources.GetObject("toolStripButton7.Image");
		this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton7.Name = "toolStripButton7";
		this.toolStripButton7.Size = new System.Drawing.Size(61, 24);
		this.toolStripButton7.Text = "&Print";
		this.toolStripButton7.Visible = false;
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.label69.AutoSize = true;
		this.label69.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label69.ForeColor = System.Drawing.Color.White;
		this.label69.Location = new System.Drawing.Point(6, 30);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(335, 19);
		this.label69.TabIndex = 1;
		this.label69.Text = "Packing List Commercial Invoice History";
		this.cmbOtherCustomer.AllowNull = (DefaultableBoolean)1;
		this.cmbOtherCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbOtherCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance208).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance101).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance101).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance102;
		((SpecialBoxBase)((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance103).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance103).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance107).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance108).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance181).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbOtherCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbOtherCustomer).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbOtherCustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbOtherCustomer).Location = new System.Drawing.Point(87, 19);
		((System.Windows.Forms.Control)(object)this.cmbOtherCustomer).Name = "cmbOtherCustomer";
		((System.Windows.Forms.Control)(object)this.cmbOtherCustomer).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.cmbOtherCustomer).TabIndex = 0;
		this.cmbOtherDestinationPort.AllowNull = (DefaultableBoolean)1;
		this.cmbOtherDestinationPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbOtherDestinationPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbOtherDestinationPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance171;
		((SpecialBoxBase)((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance172).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance172).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance172).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance173).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance175;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance176).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance177).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance177).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance177).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance177).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance177;
		((AppearanceBase)appearance178).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbOtherDestinationPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbOtherDestinationPort).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbOtherDestinationPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbOtherDestinationPort).Location = new System.Drawing.Point(87, 49);
		((System.Windows.Forms.Control)(object)this.cmbOtherDestinationPort).Name = "cmbOtherDestinationPort";
		((System.Windows.Forms.Control)(object)this.cmbOtherDestinationPort).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.cmbOtherDestinationPort).TabIndex = 1;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(896, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(52, 24);
		this.ctrlGrdBar2.TabIndex = 26;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(901, 28);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 25);
		this.ctrlGrdBar1.TabIndex = 45;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(69, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(964, 661);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "PackingDetailForCommercialInvoice";
		this.Text = "PackingDetailForCommercialInvoice";
		base.Load += new System.EventHandler(PackingDetailForCommercialInvoice_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(PackingDetailForCommercialInvoice_KeyDown);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabPage1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbSupCust).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLoadingPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDestinationPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdeliverytermnew).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbfcycode).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcareiertype).EndInit();
		this.panel1.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.Cmbinvoiceno).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.toolStrip4.ResumeLayout(false);
		this.toolStrip4.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.gridDetailHistory).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbOtherCustomer).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbOtherDestinationPort).EndInit();
		base.ResumeLayout(false);
	}
}
