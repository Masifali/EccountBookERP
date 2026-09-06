using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.WinApp.Common;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Sale;

public class LoadSaleOrderForIssuanceWithAvailableStock : Form
{
	private DropDownBind DDL = new DropDownBind();

	private DataTable dtItemdetail = new DataTable();

	private GridEXRow item;

	public static int SupplierId;

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Panel panelheading;

	private Label label23;

	private Panel panel2;

	private Button btngrnlod;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel3;

	internal GridEX grdItem;

	internal UltraCombo CmbCustomerName;

	private Label label1;

	internal UltraCombo CmbOrderNo;

	private Label OrderNo;

	private Panel panel5;

	private Panel panel4;

	private Panel panel6;

	internal GridEX grdFinialRecord;

	internal GridEX grdAvaliableStock;

	private Button btnLoadUpdateGrid;

	private Button btnReset;

	private CtrlGrdBar ctrlGrdBar3;

	private CtrlGrdBar ctrlGrdBar2;

	public LoadSaleOrderForIssuanceWithAvailableStock(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void GetItemDetailRecordByOrderId()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			DataTable dtItemRecord = SaleOrder.GetOrderDetailRecordByOrderId(new SaleOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(CmbOrderNo.Value)
			});
			if (dtItemRecord.Rows.Count > 0)
			{
				grdItem.DataSource = dtItemRecord;
				grdItem.RetrieveStructure();
				grdSettingsForGrnPending();
			}
			else
			{
				grdItem.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OrderNoBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			DataTable dt = SaleOrder.GetSaleOrderNo(new SaleOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				OrderSupCustId = Conversion.ToInt(CmbCustomerName.Value)
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbOrderNo, "Id", "DocNo", "OrderNo", ZeroIndex: false);
				((UltraGridBase)CmbOrderNo).DisplayLayout.Bands[0].Columns[1].Width = 120;
			}
			else
			{
				((Control)(object)CmbOrderNo).Text = string.Empty;
				((UltraGridBase)CmbOrderNo).DataSource = null;
				grdItem.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CustomerNameFill()
	{
		try
		{
			DataTable dt = CommonServices.SupplierCustomerGetforComboServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbCustomerName, "Id", "CompanyName", "Customer Name", ZeroIndex: false);
				((UltraGridBase)CmbCustomerName).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsForGrnPending()
	{
		try
		{
			grdItem.RootTable.Columns["Id"].Visible = false;
			grdItem.RootTable.Columns["SaleOrderId"].Visible = false;
			grdItem.RootTable.Columns["OrderItemId"].Visible = false;
			grdItem.RootTable.Columns["OrderItemUOMId"].Visible = false;
			grdItem.RootTable.Columns["OrderItemRateUOMId"].Visible = false;
			grdItem.RootTable.Columns["JobLotId"].Visible = false;
			grdItem.RootTable.Columns["ItemName"].Width = 210;
			grdItem.RootTable.Columns["Equivalent"].Width = 60;
			grdItem.RootTable.Columns["OrderItemQty"].Width = 60;
			grdItem.RootTable.Columns["OrderItemRate"].Width = 60;
			grdItem.RootTable.Columns["Crop"].Width = 60;
			grdItem.RootTable.Columns["RateUOM"].Width = 60;
			grdItem.RootTable.Columns["Equivalent"].Caption = "PackUom";
			grdItem.RootTable.Columns["OrderItemQty"].Caption = "ItemQty";
			grdItem.RootTable.Columns["OrderItemRate"].Caption = "ItemRate";
			grdItem.RootTable.Columns["OrderItemQty"].AggregateFunction = (AggregateFunction)2;
			grdItem.RootTable.Columns.Add("Add");
			grdItem.RootTable.Columns["Add"].Key = "Add";
			grdItem.RootTable.Columns["Add"].Caption = "Add New";
			grdItem.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdItem.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdItem.RootTable.Columns["Add"].ButtonText = "Add";
			grdItem.RootTable.Columns["Add"].Width = 30;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdItem).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdItem).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				grdItem.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Hide();
	}

	private void btngrnlod_Click(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbOrderNo).ActiveRow != null)
			{
				((Control)(object)CmbCustomerName).Enabled = false;
				GetItemDetailRecordByOrderId();
			}
			else
			{
				grdItem.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void frmLoadGRN_Load_1(object sender, EventArgs e)
	{
		try
		{
			if (SaleInvoiceDirectwithLoader.SupplierIdMain > 0)
			{
				CmbCustomerName.Value = SaleInvoiceDirectwithLoader.SupplierIdMain;
				((Control)(object)CmbCustomerName).Enabled = false;
			}
			CmbCustomerName.Focus();
			if (SaleInvoiceDirectwithLoader.dtGrid.Rows.Count > 0)
			{
				grdFinialRecord.DataSource = SaleInvoiceDirectwithLoader.dtGrid;
				grdFinialRecord.RetrieveStructure();
				grdFinialSettings();
			}
			CustomerNameFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void frmLoadSaleOrder_FormClosing(object sender, FormClosingEventArgs e)
	{
	}

	private void CmbCustomerName_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbCustomerName).ActiveRow != null)
			{
				OrderNoBind();
				return;
			}
			((Control)(object)CmbOrderNo).Text = string.Empty;
			((UltraGridBase)CmbOrderNo).DataSource = null;
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
			grdAvaliableStock.RootTable.Columns["ItemName"].Width = 250;
			grdAvaliableStock.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grdAvaliableStock.RootTable.Columns["RefDocIdNo"].Visible = false;
			grdAvaliableStock.RootTable.Columns["RefDocSubIdNo"].Visible = false;
			grdAvaliableStock.RootTable.Columns["ItemId"].Visible = false;
			grdAvaliableStock.RootTable.Columns["ItemUom"].Visible = false;
			grdAvaliableStock.RootTable.Columns["JobLotId"].Visible = false;
			grdAvaliableStock.RootTable.Columns["InvPackingTypeId"].Visible = false;
			grdAvaliableStock.RootTable.Columns["WareHouseId"].Visible = false;
			grdAvaliableStock.RootTable.Columns["RateUomId"].Visible = false;
			grdAvaliableStock.RootTable.Columns["QtyIn"].Visible = false;
			grdAvaliableStock.RootTable.Columns["QtyOut"].Visible = false;
			grdAvaliableStock.RootTable.Columns["WeightIn"].Visible = false;
			grdAvaliableStock.RootTable.Columns["WeightOut"].Visible = false;
			grdAvaliableStock.RootTable.Columns["AVgRate"].Visible = false;
			grdAvaliableStock.RootTable.Columns["QtyIn"].AggregateFunction = (AggregateFunction)2;
			grdAvaliableStock.RootTable.Columns["QtyOut"].AggregateFunction = (AggregateFunction)2;
			grdAvaliableStock.RootTable.Columns["QtyBalance"].AggregateFunction = (AggregateFunction)2;
			grdAvaliableStock.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdAvaliableStock.RootTable.Columns["WeightIn"].AggregateFunction = (AggregateFunction)2;
			grdAvaliableStock.RootTable.Columns["WeightOut"].AggregateFunction = (AggregateFunction)2;
			grdAvaliableStock.RootTable.Columns["WeightBalance"].AggregateFunction = (AggregateFunction)2;
			grdAvaliableStock.RootTable.Columns["QtyIn"].TextAlignment = (TextAlignment)3;
			grdAvaliableStock.RootTable.Columns["QtyOut"].TextAlignment = (TextAlignment)3;
			grdAvaliableStock.RootTable.Columns["QtyBalance"].TextAlignment = (TextAlignment)3;
			grdAvaliableStock.RootTable.Columns["WeightIn"].TextAlignment = (TextAlignment)3;
			grdAvaliableStock.RootTable.Columns["WeightOut"].TextAlignment = (TextAlignment)3;
			grdAvaliableStock.RootTable.Columns["WeightBalance"].TextAlignment = (TextAlignment)3;
			grdAvaliableStock.RootTable.Columns["AVgRate"].TextAlignment = (TextAlignment)3;
			grdAvaliableStock.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdAvaliableStock.RootTable.Columns["AVgRate"].FormatString = "#,#.##";
			grdAvaliableStock.RootTable.Columns["QtyIn"].FormatString = "#,#";
			grdAvaliableStock.RootTable.Columns["QtyOut"].FormatString = "#,#";
			grdAvaliableStock.RootTable.Columns["QtyBalance"].FormatString = "#,#";
			grdAvaliableStock.RootTable.Columns["WeightIn"].FormatString = "#,#";
			grdAvaliableStock.RootTable.Columns["WeightOut"].FormatString = "#,#";
			grdAvaliableStock.RootTable.Columns["WeightBalance"].FormatString = "#,#";
			grdAvaliableStock.RootTable.Columns["ItemAmount"].FormatString = "#,#";
			grdAvaliableStock.RootTable.Columns["QtyIn"].TotalFormatString = "#,#";
			grdAvaliableStock.RootTable.Columns["QtyOut"].TotalFormatString = "#,#";
			grdAvaliableStock.RootTable.Columns["QtyBalance"].TotalFormatString = "#,#";
			grdAvaliableStock.RootTable.Columns["WeightIn"].TotalFormatString = "#,#";
			grdAvaliableStock.RootTable.Columns["WeightOut"].TotalFormatString = "#,#";
			grdAvaliableStock.RootTable.Columns["WeightBalance"].TotalFormatString = "#,#";
			grdAvaliableStock.RootTable.Columns["ItemAmount"].TotalFormatString = "#,#";
			grdAvaliableStock.RootTable.Columns.Add("Select");
			grdAvaliableStock.RootTable.Columns["Select"].ActAsSelector = true;
			grdAvaliableStock.RootTable.Columns["Select"].UseHeaderSelector = true;
			grdAvaliableStock.RootTable.Columns["Select"].Width = 40;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdItem_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		try
		{
			if (e.Column.Key == "Add")
			{
				item = grdItem.CurrentRow;
				DataTable dt = InventoryStockEvalautionDetail.GetAvailableTransactionsForIssuance(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					ItemId = Conversion.ToInt(item.Cells["OrderItemId"].Value),
					JobLotId = Conversion.ToInt(item.Cells["JobLotId"].Value),
					CropYear = Conversion.ToString(item.Cells["Crop"].Value)
				});
				if (dt.Rows.Count > 0)
				{
					SupplierId = Conversion.ToInt(CmbCustomerName.Value);
					grdAvaliableStock.DataSource = dt;
					grdAvaliableStock.RetrieveStructure();
					grdSettings();
				}
				else
				{
					SupplierId = 0;
					grdAvaliableStock.ClearStructure();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadUpdateGrid_Click(object sender, EventArgs e)
	{
		try
		{
			if (int.Parse(grdAvaliableStock.GetCheckedRows().Count().ToString()) > 0)
			{
				GridEXRow[] checkedRows = grdAvaliableStock.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					SaleInvoiceDirectwithLoader.dtGrid.Rows.Add(Conversion.ToInt(CmbOrderNo.Value), Conversion.ToString((object)((Control)(object)CmbOrderNo).Text.Trim()), Conversion.ToInt(item.Cells["Id"].Value), r.Cells["RefDocumentTypeId"].Value.ToString(), r.Cells["RefDocIdNo"].Value.ToString(), r.Cells["RefDocSubIdNo"].Value.ToString(), Conversion.ToInt(r.Cells["WarehouseId"].Value), r.Cells["WareHouseCode"].Value.ToString(), Conversion.ToInt(r.Cells["ItemId"].Value), r.Cells["ItemName"].Value.ToString(), Conversion.ToInt(r.Cells["ItemUom"].Value), Conversion.ToInt(r.Cells["PackSize"].Value), r.Cells["CropBatch"].Value.ToString(), Conversion.ToInt(r.Cells["JobLotId"].Value), r.Cells["JobLotCode"].Value.ToString(), Conversion.ToInt(r.Cells["InvPackingTypeId"].Value), r.Cells["PackingType"].Value.ToString(), Conversion.ToDouble(r.Cells["QtyBalance"].Value), Conversion.ToDouble(r.Cells["QtyBalance"].Value), Conversion.ToDouble(r.Cells["WeightBalance"].Value), Conversion.ToDouble(r.Cells["WeightBalance"].Value), 0, 0, 0, 0, 0, Conversion.ToDouble(r.Cells["WeightBalance"].Value), Conversion.ToDouble(r.Cells["WeightBalance"].Value), Conversion.ToDouble(r.Cells["AVgRate"].Value), Conversion.ToInt(item.Cells["OrderItemRate"].Value), Conversion.ToInt(item.Cells["OrderItemRateUOMId"].Value), Conversion.ToDouble(r.Cells["Equivalent"].Value), 0, 0, Conversion.ToDouble(r.Cells["WeightBalance"].Value) / Conversion.ToDouble(r.Cells["Equivalent"].Value) * (double)Conversion.ToInt(item.Cells["OrderItemRate"].Value), 0, "", Conversion.ToDouble(r.Cells["WeightBalance"].Value) / Conversion.ToDouble(r.Cells["Equivalent"].Value) * (double)Conversion.ToInt(item.Cells["OrderItemRate"].Value), 0, 0, 0);
				}
				grdFinialRecord.DataSource = SaleInvoiceDirectwithLoader.dtGrid;
				grdFinialRecord.RetrieveStructure();
				grdFinialSettings();
				checkedRows = grdAvaliableStock.GetCheckedRows();
				for (int i = 0; i < checkedRows.Length; i++)
				{
					checkedRows[i].Delete();
				}
			}
			else
			{
				MessageBox.Show("No Row is Selected");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFinialSettings()
	{
		try
		{
			grdFinialRecord.RootTable.Columns["OrderId"].Visible = false;
			grdFinialRecord.RootTable.Columns["SaleOrderDetailId"].Visible = false;
			grdFinialRecord.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grdFinialRecord.RootTable.Columns["RefDocIdNo"].Visible = false;
			grdFinialRecord.RootTable.Columns["RefDocSubIdNo"].Visible = false;
			grdFinialRecord.RootTable.Columns["ItemId"].Visible = false;
			grdFinialRecord.RootTable.Columns["PackingTypeId"].Visible = false;
			grdFinialRecord.RootTable.Columns["ItemUOMId"].Visible = false;
			grdFinialRecord.RootTable.Columns["WarehouseId"].Visible = false;
			grdFinialRecord.RootTable.Columns["RateUOMId"].Visible = false;
			grdFinialRecord.RootTable.Columns["JobLotId"].Visible = false;
			grdFinialRecord.RootTable.Columns["CgsRate"].Visible = false;
			grdFinialRecord.RootTable.Columns["Expense"].Visible = false;
			grdFinialRecord.RootTable.Columns["Journal"].Visible = false;
			grdFinialRecord.RootTable.Columns["Commission"].Visible = false;
			grdFinialRecord.RootTable.Columns["OrderId"].EditType = (EditType)0;
			grdFinialRecord.RootTable.Columns["OrderNo"].EditType = (EditType)0;
			grdFinialRecord.RootTable.Columns["SaleOrderDetailId"].EditType = (EditType)0;
			grdFinialRecord.RootTable.Columns["RefDocumentTypeId"].EditType = (EditType)0;
			grdFinialRecord.RootTable.Columns["RefDocIdNo"].EditType = (EditType)0;
			grdFinialRecord.RootTable.Columns["RefDocSubIdNo"].EditType = (EditType)0;
			grdFinialRecord.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdFinialRecord.RootTable.Columns["PackingTypeId"].EditType = (EditType)0;
			grdFinialRecord.RootTable.Columns["ItemUOMId"].EditType = (EditType)0;
			grdFinialRecord.RootTable.Columns["WarehouseId"].EditType = (EditType)0;
			grdFinialRecord.RootTable.Columns["RateUOMId"].EditType = (EditType)0;
			grdFinialRecord.RootTable.Columns["JobLotId"].EditType = (EditType)0;
			grdFinialRecord.RootTable.Columns["WareHouse"].EditType = (EditType)0;
			grdFinialRecord.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdFinialRecord.RootTable.Columns["ItemUOM"].EditType = (EditType)0;
			grdFinialRecord.RootTable.Columns["CropYear"].EditType = (EditType)0;
			grdFinialRecord.RootTable.Columns["JobLot"].EditType = (EditType)0;
			grdFinialRecord.RootTable.Columns["PackingType"].EditType = (EditType)0;
			grdFinialRecord.RootTable.Columns["BalQty"].EditType = (EditType)0;
			grdFinialRecord.RootTable.Columns["ItemQty"].EditType = (EditType)1;
			grdFinialRecord.RootTable.Columns["BalWeight"].EditType = (EditType)0;
			grdFinialRecord.RootTable.Columns["GrossWeight"].EditType = (EditType)1;
			grdFinialRecord.RootTable.Columns["EmptyBags"].EditType = (EditType)1;
			grdFinialRecord.RootTable.Columns["EmptyBagsTotal"].EditType = (EditType)0;
			grdFinialRecord.RootTable.Columns["WeightCut"].EditType = (EditType)1;
			grdFinialRecord.RootTable.Columns["WeightCutTotal"].EditType = (EditType)0;
			grdFinialRecord.RootTable.Columns["AddLss"].EditType = (EditType)1;
			grdFinialRecord.RootTable.Columns["NetBillWeight"].EditType = (EditType)0;
			grdFinialRecord.RootTable.Columns["StockWeight"].EditType = (EditType)0;
			grdFinialRecord.RootTable.Columns["CgsRate"].EditType = (EditType)0;
			grdFinialRecord.RootTable.Columns["Rate"].EditType = (EditType)0;
			grdFinialRecord.RootTable.Columns["RateUOM"].EditType = (EditType)0;
			grdFinialRecord.RootTable.Columns["RateCut"].EditType = (EditType)1;
			grdFinialRecord.RootTable.Columns["RateCutTotal"].EditType = (EditType)0;
			grdFinialRecord.RootTable.Columns["ItemAmount"].EditType = (EditType)0;
			grdFinialRecord.RootTable.Columns["GpNo"].EditType = (EditType)1;
			grdFinialRecord.RootTable.Columns["VehicleNo"].EditType = (EditType)1;
			grdFinialRecord.RootTable.Columns["BillAmount"].EditType = (EditType)0;
			grdFinialRecord.RootTable.Columns["Expense"].EditType = (EditType)0;
			grdFinialRecord.RootTable.Columns["Journal"].EditType = (EditType)0;
			grdFinialRecord.RootTable.Columns["Commission"].EditType = (EditType)0;
			grdFinialRecord.RootTable.Columns["BalQty"].AggregateFunction = (AggregateFunction)2;
			grdFinialRecord.RootTable.Columns["BalWeight"].AggregateFunction = (AggregateFunction)2;
			grdFinialRecord.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdFinialRecord.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdFinialRecord.RootTable.Columns["EmptyBagsTotal"].AggregateFunction = (AggregateFunction)2;
			grdFinialRecord.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			grdFinialRecord.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			grdFinialRecord.RootTable.Columns["WeightCut"].AggregateFunction = (AggregateFunction)2;
			grdFinialRecord.RootTable.Columns["WeightCutTotal"].AggregateFunction = (AggregateFunction)2;
			grdFinialRecord.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdFinialRecord.RootTable.Columns["RateCutTotal"].AggregateFunction = (AggregateFunction)2;
			grdFinialRecord.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grdFinialRecord.RootTable.Columns["Expense"].AggregateFunction = (AggregateFunction)2;
			grdFinialRecord.RootTable.Columns["Journal"].AggregateFunction = (AggregateFunction)2;
			grdFinialRecord.RootTable.Columns["Commission"].AggregateFunction = (AggregateFunction)2;
			grdFinialRecord.RootTable.Columns["GrossWeight"].FormatString = "#,#";
			grdFinialRecord.RootTable.Columns["NetBillWeight"].FormatString = "#,#";
			grdFinialRecord.RootTable.Columns["StockWeight"].FormatString = "#,#";
			grdFinialRecord.RootTable.Columns["Rate"].FormatString = "#,#.##";
			grdFinialRecord.RootTable.Columns["ItemAmount"].FormatString = "#,#";
			grdFinialRecord.RootTable.Columns["RateCut"].FormatString = "#,#.##";
			grdFinialRecord.RootTable.Columns["RateCutTotal"].FormatString = "#,#";
			grdFinialRecord.RootTable.Columns["BillAmount"].FormatString = "#,#";
			grdFinialRecord.RootTable.Columns["Expense"].FormatString = "#,#";
			grdFinialRecord.RootTable.Columns["Journal"].FormatString = "#,#";
			grdFinialRecord.RootTable.Columns["Commission"].FormatString = "#,#";
			grdFinialRecord.RootTable.Columns["GrossWeight"].TotalFormatString = "#,#";
			grdFinialRecord.RootTable.Columns["NetBillWeight"].TotalFormatString = "#,#";
			grdFinialRecord.RootTable.Columns["StockWeight"].TotalFormatString = "#,#";
			grdFinialRecord.RootTable.Columns["Rate"].TotalFormatString = "#,#.##";
			grdFinialRecord.RootTable.Columns["ItemAmount"].TotalFormatString = "#,#";
			grdFinialRecord.RootTable.Columns["RateCut"].TotalFormatString = "#,#.##";
			grdFinialRecord.RootTable.Columns["RateCutTotal"].TotalFormatString = "#,#";
			grdFinialRecord.RootTable.Columns["BillAmount"].TotalFormatString = "#,#";
			grdFinialRecord.RootTable.Columns["Expense"].TotalFormatString = "#,#";
			grdFinialRecord.RootTable.Columns["Journal"].TotalFormatString = "#,#";
			grdFinialRecord.RootTable.Columns["Commission"].TotalFormatString = "#,#";
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnReset_Click(object sender, EventArgs e)
	{
		try
		{
			SaleInvoiceDirectwithLoader.SupplierIdMain = 0;
			CmbCustomerName.Focus();
			((Control)(object)CmbCustomerName).Enabled = true;
			SaleInvoiceDirectwithLoader.dtGrid.Rows.Clear();
			((Control)(object)CmbCustomerName).Text = string.Empty;
			((Control)(object)CmbOrderNo).Text = string.Empty;
			grdItem.ClearStructure();
			grdAvaliableStock.ClearStructure();
			grdFinialRecord.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFinialRecord_DeletingRecord(object sender, RowActionCancelEventArgs e)
	{
		try
		{
			GridEXRow DeleteRow = grdFinialRecord.CurrentRow;
			if (DeleteRow == null)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("SaleOrderId");
			dt.Columns.Add("OrderItemId");
			dt.Columns.Add("OrderItemUOMId");
			dt.Columns.Add("ItemName");
			dt.Columns.Add("Equivalent");
			dt.Columns.Add("Crop");
			dt.Columns.Add("JobLotDescription");
			dt.Columns.Add("OrderItemQty");
			dt.Columns.Add("NetWeight");
			dt.Columns.Add("OrderItemRate");
			dt.Columns.Add("RateUOM");
			dt.Columns.Add("OrderItemRateUOMId");
			dt.Columns.Add("Amount");
			dt.Columns.Add("JobLotId");
			dt.Columns.Add("CityArea");
			dt.Columns.Add("LabSampleNo");
			dt.Columns.Add("AmountCalcType");
			dt.Columns.Add("TaxableStatus");
			dt.Columns.Add("OrderRemarks");
			dt.Columns.Add("BagWeight");
			dt.Columns.Add("BagPrice");
			for (int i = 0; i < dtItemdetail.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtItemdetail.Rows[i]["Id"]) == Conversion.ToInt(DeleteRow.Cells["SaleOrderDetailId"].Value))
				{
					dt.Rows.Add(dtItemdetail.Rows[i][0], dtItemdetail.Rows[i][1], dtItemdetail.Rows[i][2], dtItemdetail.Rows[i][3], dtItemdetail.Rows[i][4], dtItemdetail.Rows[i][5], dtItemdetail.Rows[i][6], dtItemdetail.Rows[i][7], dtItemdetail.Rows[i][8], dtItemdetail.Rows[i][9], dtItemdetail.Rows[i][10], dtItemdetail.Rows[i][11], dtItemdetail.Rows[i][12], dtItemdetail.Rows[i][13], dtItemdetail.Rows[i][14], dtItemdetail.Rows[i][15], dtItemdetail.Rows[i][16], dtItemdetail.Rows[i][17], dtItemdetail.Rows[i][18], dtItemdetail.Rows[i][19], dtItemdetail.Rows[i][20], dtItemdetail.Rows[i][21]);
				}
			}
			grdItem.DataSource = dt;
			grdItem.RetrieveStructure();
			grdSettingsForGrnPending();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFinialRecord_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdFinialRecord.UpdateData();
			GridEXRow item = grdFinialRecord.CurrentRow;
			if (e.Column.Key == "ItemQty" && item != null && (int)item.RowType == 0)
			{
				double PackUom = Conversion.ToDouble(item.Cells["ItemUOM"].Value);
				double ItemQty3 = Conversion.ToDouble(item.Cells["ItemQty"].Value);
				if (PackUom > 0.0 && ItemQty3 > 0.0)
				{
					item.Cells["GrossWeight"].Value = ItemQty3 * PackUom;
					double WeightCutTotal5 = Conversion.ToDouble(item.Cells["WeightCutTotal"].Value);
					double EmptyBagsTotal5 = Conversion.ToDouble(item.Cells["EmptyBagsTotal"].Value);
					double num = Conversion.ToDouble(item.Cells["GrossWeight"].Value);
					double AddLss5 = Conversion.ToDouble(item.Cells["AddLss"].Value);
					double NetWeight5 = num - EmptyBagsTotal5 - WeightCutTotal5 + AddLss5;
					item.Cells["NetBillWeight"].Value = NetWeight5;
					item.Cells["StockWeight"].Value = NetWeight5;
					double Rate5 = Conversion.ToDouble(item.Cells["Rate"].Value);
					double RateUom5 = Conversion.ToDouble(item.Cells["RateUOM"].Value);
					double ItemAmount5 = NetWeight5 / RateUom5 * Rate5;
					item.Cells["ItemAmount"].Value = ItemAmount5;
					item.Cells["BillAmount"].Value = ItemAmount5;
				}
			}
			if (e.Column.Key == "EmptyBags")
			{
				double ItemQty2 = Conversion.ToDouble(item.Cells["ItemQty"].Value);
				double EmptyBags = Conversion.ToDouble(item.Cells["EmptyBags"].Value);
				item.Cells["EmptyBagsTotal"].Value = ItemQty2 * EmptyBags;
				double WeightCutTotal4 = Conversion.ToDouble(item.Cells["WeightCutTotal"].Value);
				double EmptyBagsTotal4 = Conversion.ToDouble(item.Cells["EmptyBagsTotal"].Value);
				double num2 = Conversion.ToDouble(item.Cells["GrossWeight"].Value);
				double AddLss4 = Conversion.ToDouble(item.Cells["AddLss"].Value);
				double NetWeight4 = num2 - EmptyBagsTotal4 - WeightCutTotal4 + AddLss4;
				item.Cells["NetBillWeight"].Value = NetWeight4;
				item.Cells["StockWeight"].Value = NetWeight4;
				double Rate4 = Conversion.ToDouble(item.Cells["Rate"].Value);
				double RateUom4 = Conversion.ToDouble(item.Cells["RateUOM"].Value);
				double ItemAmount4 = NetWeight4 / RateUom4 * Rate4;
				item.Cells["ItemAmount"].Value = ItemAmount4;
				item.Cells["BillAmount"].Value = ItemAmount4;
			}
			if (e.Column.Key == "WeightCut")
			{
				double ItemQty = Conversion.ToDouble(item.Cells["ItemQty"].Value);
				double WeightCut = Conversion.ToDouble(item.Cells["WeightCut"].Value);
				item.Cells["WeightCutTotal"].Value = ItemQty * WeightCut;
				double WeightCutTotal3 = Conversion.ToDouble(item.Cells["WeightCutTotal"].Value);
				double EmptyBagsTotal3 = Conversion.ToDouble(item.Cells["EmptyBagsTotal"].Value);
				double num3 = Conversion.ToDouble(item.Cells["GrossWeight"].Value);
				double AddLss3 = Conversion.ToDouble(item.Cells["AddLss"].Value);
				double NetWeight3 = num3 - EmptyBagsTotal3 - WeightCutTotal3 + AddLss3;
				item.Cells["NetBillWeight"].Value = NetWeight3;
				item.Cells["StockWeight"].Value = NetWeight3;
				double Rate3 = Conversion.ToDouble(item.Cells["Rate"].Value);
				double RateUom3 = Conversion.ToDouble(item.Cells["RateUOM"].Value);
				double ItemAmount3 = NetWeight3 / RateUom3 * Rate3;
				item.Cells["ItemAmount"].Value = ItemAmount3;
				item.Cells["BillAmount"].Value = ItemAmount3;
			}
			if (e.Column.Key == "GrossWeight")
			{
				double WeightCutTotal2 = Conversion.ToDouble(item.Cells["WeightCutTotal"].Value);
				double EmptyBagsTotal2 = Conversion.ToDouble(item.Cells["EmptyBagsTotal"].Value);
				double num4 = Conversion.ToDouble(item.Cells["GrossWeight"].Value);
				double AddLss2 = Conversion.ToDouble(item.Cells["AddLss"].Value);
				double NetWeight2 = num4 - EmptyBagsTotal2 - WeightCutTotal2 + AddLss2;
				item.Cells["NetBillWeight"].Value = NetWeight2;
				item.Cells["StockWeight"].Value = NetWeight2;
				double Rate2 = Conversion.ToDouble(item.Cells["Rate"].Value);
				double RateUom2 = Conversion.ToDouble(item.Cells["RateUOM"].Value);
				double ItemAmount2 = NetWeight2 / RateUom2 * Rate2;
				item.Cells["ItemAmount"].Value = ItemAmount2;
				item.Cells["BillAmount"].Value = ItemAmount2;
			}
			if (e.Column.Key == "AddLss")
			{
				double WeightCutTotal = Conversion.ToDouble(item.Cells["WeightCutTotal"].Value);
				double EmptyBagsTotal = Conversion.ToDouble(item.Cells["EmptyBagsTotal"].Value);
				double num5 = Conversion.ToDouble(item.Cells["GrossWeight"].Value);
				double AddLss = Conversion.ToDouble(item.Cells["AddLss"].Value);
				double NetWeight = num5 - EmptyBagsTotal - WeightCutTotal + AddLss;
				item.Cells["NetBillWeight"].Value = NetWeight;
				item.Cells["StockWeight"].Value = NetWeight;
				double Rate = Conversion.ToDouble(item.Cells["Rate"].Value);
				double RateUom = Conversion.ToDouble(item.Cells["RateUOM"].Value);
				double ItemAmount = NetWeight / RateUom * Rate;
				item.Cells["ItemAmount"].Value = ItemAmount;
				item.Cells["BillAmount"].Value = ItemAmount;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdFinialRecord).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdFinialRecord).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				grdFinialRecord.LoadLayoutFile((Stream)fs);
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
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdAvaliableStock).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdAvaliableStock).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				grdAvaliableStock.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
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
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Expected O, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Expected O, but got Unknown
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Expected O, but got Unknown
		//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Expected O, but got Unknown
		//IL_0503: Unknown result type (might be due to invalid IL or missing references)
		//IL_050d: Expected O, but got Unknown
		//IL_08b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c3: Expected O, but got Unknown
		GridEXLayout grdFinialRecord_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdAvaliableStock_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdItem_DesignTimeLayout = new GridEXLayout();
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.panelheading = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdFinialRecord = new GridEX();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdAvaliableStock = new GridEX();
		this.label23 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdItem = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.btnReset = new System.Windows.Forms.Button();
		this.btnLoadUpdateGrid = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbOrderNo = new UltraCombo();
		this.OrderNo = new System.Windows.Forms.Label();
		this.CmbCustomerName = new UltraCombo();
		this.btngrnlod = new System.Windows.Forms.Button();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel1.SuspendLayout();
		this.panelheading.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdFinialRecord).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdAvaliableStock).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdItem).BeginInit();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerName).BeginInit();
		this.panel3.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel6.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panelheading);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1073, 36);
		this.panel1.TabIndex = 0;
		this.panelheading.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panelheading.Controls.Add(this.ctrlGrdBar3);
		this.panelheading.Controls.Add(this.ctrlGrdBar2);
		this.panelheading.Controls.Add(this.label23);
		this.panelheading.Controls.Add(this.ctrlGrdBar1);
		this.panelheading.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelheading.Location = new System.Drawing.Point(0, 0);
		this.panelheading.Name = "panelheading";
		this.panelheading.Size = new System.Drawing.Size(1073, 36);
		this.panelheading.TabIndex = 29;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1028, 3);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdFinialRecord;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(45, 26);
		this.ctrlGrdBar3.TabIndex = 26;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.grdFinialRecord.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdFinialRecord).BackColor = System.Drawing.Color.White;
		grdFinialRecord_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdFinialRecord.DesignTimeLayout = grdFinialRecord_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdFinialRecord).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdFinialRecord.FilterMode = (FilterMode)2;
		this.grdFinialRecord.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdFinialRecord).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdFinialRecord.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdFinialRecord).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdFinialRecord).Name = "grdFinialRecord";
		((ScrollableBase)this.grdFinialRecord).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdFinialRecord).Size = new System.Drawing.Size(1073, 280);
		((System.Windows.Forms.Control)(object)this.grdFinialRecord).TabIndex = 4;
		this.grdFinialRecord.TotalRow = (InheritableBoolean)1;
		this.grdFinialRecord.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdFinialRecord).VisualStyle = (VisualStyle)4;
		this.grdFinialRecord.DeletingRecord += new RowActionCancelEventHandler(grdFinialRecord_DeletingRecord);
		this.grdFinialRecord.CellUpdated += new ColumnActionEventHandler(grdFinialRecord_CellUpdated);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(979, 3);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdAvaliableStock;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 26);
		this.ctrlGrdBar2.TabIndex = 25;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.grdAvaliableStock.AllowDelete = (InheritableBoolean)1;
		this.grdAvaliableStock.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdAvaliableStock).BackColor = System.Drawing.Color.White;
		grdAvaliableStock_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdAvaliableStock.DesignTimeLayout = grdAvaliableStock_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdAvaliableStock).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdAvaliableStock.FilterMode = (FilterMode)2;
		this.grdAvaliableStock.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdAvaliableStock).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdAvaliableStock.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdAvaliableStock).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdAvaliableStock).Name = "grdAvaliableStock";
		((ScrollableBase)this.grdAvaliableStock).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdAvaliableStock).Size = new System.Drawing.Size(521, 196);
		((System.Windows.Forms.Control)(object)this.grdAvaliableStock).TabIndex = 3;
		this.grdAvaliableStock.TotalRow = (InheritableBoolean)1;
		this.grdAvaliableStock.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdAvaliableStock).VisualStyle = (VisualStyle)4;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(6, 5);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(148, 24);
		this.label23.TabIndex = 24;
		this.label23.Text = "Sale Order Load";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(931, 3);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdItem;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 26);
		this.ctrlGrdBar1.TabIndex = 10;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grdItem.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdItem).BackColor = System.Drawing.Color.White;
		grdItem_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdItem.DesignTimeLayout = grdItem_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdItem).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdItem).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdItem.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdItem).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdItem).Name = "grdItem";
		((ScrollableBase)this.grdItem).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdItem).Size = new System.Drawing.Size(552, 196);
		((System.Windows.Forms.Control)(object)this.grdItem).TabIndex = 2;
		this.grdItem.TotalRow = (InheritableBoolean)1;
		this.grdItem.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdItem).VisualStyle = (VisualStyle)4;
		this.grdItem.ColumnButtonClick += new ColumnActionEventHandler(grdItem_ColumnButtonClick);
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.btnReset);
		this.panel2.Controls.Add(this.btnLoadUpdateGrid);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOrderNo);
		this.panel2.Controls.Add(this.OrderNo);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerName);
		this.panel2.Controls.Add(this.btngrnlod);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 36);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1073, 64);
		this.panel2.TabIndex = 30;
		this.btnReset.BackColor = System.Drawing.Color.White;
		this.btnReset.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnReset.Location = new System.Drawing.Point(575, 29);
		this.btnReset.Name = "btnReset";
		this.btnReset.Size = new System.Drawing.Size(51, 26);
		this.btnReset.TabIndex = 22;
		this.btnReset.Text = "Reset";
		this.btnReset.UseVisualStyleBackColor = false;
		this.btnReset.Click += new System.EventHandler(btnReset_Click);
		this.btnLoadUpdateGrid.BackColor = System.Drawing.Color.White;
		this.btnLoadUpdateGrid.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadUpdateGrid.Location = new System.Drawing.Point(470, 29);
		this.btnLoadUpdateGrid.Name = "btnLoadUpdateGrid";
		this.btnLoadUpdateGrid.Size = new System.Drawing.Size(102, 26);
		this.btnLoadUpdateGrid.TabIndex = 21;
		this.btnLoadUpdateGrid.Text = "LoadFinalData";
		this.btnLoadUpdateGrid.UseVisualStyleBackColor = false;
		this.btnLoadUpdateGrid.Click += new System.EventHandler(btnLoadUpdateGrid_Click);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(8, 8);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(91, 15);
		this.label1.TabIndex = 20;
		this.label1.Text = "CustomerName";
		this.CmbOrderNo.AllowNull = (DefaultableBoolean)1;
		this.CmbOrderNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOrderNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOrderNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance158).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance158).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance159).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance159;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance160).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance164).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance165).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance165).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance165).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbOrderNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Location = new System.Drawing.Point(290, 29);
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Name = "CmbOrderNo";
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Size = new System.Drawing.Size(112, 26);
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).TabIndex = 3;
		this.OrderNo.AutoSize = true;
		this.OrderNo.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.OrderNo.Location = new System.Drawing.Point(287, 8);
		this.OrderNo.Name = "OrderNo";
		this.OrderNo.Size = new System.Drawing.Size(56, 15);
		this.OrderNo.TabIndex = 19;
		this.OrderNo.Text = "Order No";
		this.CmbCustomerName.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance171;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance172).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance172).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance172).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance173).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance175;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance176).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance177).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance177).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance177).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance177).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance177;
		((AppearanceBase)appearance178).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Location = new System.Drawing.Point(11, 29);
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Name = "CmbCustomerName";
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Size = new System.Drawing.Size(276, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).TextChanged += new System.EventHandler(CmbCustomerName_TextChanged);
		this.btngrnlod.BackColor = System.Drawing.Color.White;
		this.btngrnlod.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btngrnlod.Location = new System.Drawing.Point(406, 29);
		this.btngrnlod.Name = "btngrnlod";
		this.btngrnlod.Size = new System.Drawing.Size(61, 26);
		this.btngrnlod.TabIndex = 4;
		this.btngrnlod.Text = "Search";
		this.btngrnlod.UseVisualStyleBackColor = false;
		this.btngrnlod.Click += new System.EventHandler(btngrnlod_Click);
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Controls.Add(this.panel4);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 100);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1073, 196);
		this.panel3.TabIndex = 31;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdAvaliableStock);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(552, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(521, 196);
		this.panel5.TabIndex = 4;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdItem);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(552, 196);
		this.panel4.TabIndex = 3;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdFinialRecord);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 296);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1073, 280);
		this.panel6.TabIndex = 32;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1073, 576);
		base.Controls.Add(this.panel6);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.Name = "LoadSaleOrderForIssuanceWithAvailableStock";
		this.Text = "SaleOrderLoad";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmLoadSaleOrder_FormClosing);
		base.Load += new System.EventHandler(frmLoadGRN_Load_1);
		this.panel1.ResumeLayout(false);
		this.panelheading.ResumeLayout(false);
		this.panelheading.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdFinialRecord).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdAvaliableStock).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdItem).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerName).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
