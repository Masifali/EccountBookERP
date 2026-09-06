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
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.PurchaseForSalt;

public class LoadPurchaseOrder : Form
{
	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	public DataTable dtLoader = new DataTable();

	private IContainer components;

	private Panel panel1;

	private Label label23;

	private Panel panel2;

	private Button btngrnlod;

	private DateTimePicker Todate;

	private DateTimePicker FromDate;

	private Label label13;

	private Label label1;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel3;

	internal GridEX grd;

	private Button btnLoadOnInvoice;

	private Label label2;

	internal UltraCombo CmbSupplier;

	private Button btnReset;

	private Label label4;

	private TextBox txtOrderNoFrom;

	private TextBox txtOrderNoTo;

	private Label label3;

	private GroupBox groupBox1;

	private ToolStrip toolStrip1;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripButton btnNew;

	private ToolStripButton btnRefresh;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripButton btnShortcutKeys;

	private ToolStripSeparator toolStripSeparator1;

	public LoadPurchaseOrder(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void LoadInvoices_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 500);
			dtLoader.Columns.Add("DocumentTypeId", typeof(int));
			dtLoader.Columns.Add("DetailId", typeof(int));
			dtLoader.Columns.Add("Id", typeof(int));
			dtLoader.Columns.Add("DocDate", typeof(DateTime));
			dtLoader.Columns.Add("DocNo", typeof(int));
			dtLoader.Columns.Add("SupplierCustomerId", typeof(int));
			dtLoader.Columns.Add("SupplierName", typeof(string));
			dtLoader.Columns.Add("CommissionAgentId", typeof(int));
			dtLoader.Columns.Add("CommissionType", typeof(string));
			dtLoader.Columns.Add("CommRate", typeof(decimal));
			dtLoader.Columns.Add("CommUom", typeof(decimal));
			dtLoader.Columns.Add("CommAmount", typeof(decimal));
			dtLoader.Columns.Add("CommissionRemarks", typeof(string));
			dtLoader.Columns.Add("DeliveryTerm", typeof(string));
			dtLoader.Columns.Add("PaymentTermsId", typeof(int));
			dtLoader.Columns.Add("DueDate", typeof(DateTime));
			dtLoader.Columns.Add("DueDays", typeof(int));
			dtLoader.Columns.Add("JobLotId", typeof(int));
			dtLoader.Columns.Add("JobLot", typeof(string));
			dtLoader.Columns.Add("ItemId", typeof(int));
			dtLoader.Columns.Add("ItemName", typeof(string));
			dtLoader.Columns.Add("ItemCode", typeof(string));
			dtLoader.Columns.Add("ItemUOMId", typeof(int));
			dtLoader.Columns.Add("PackUomCode", typeof(string));
			dtLoader.Columns.Add("PackUomEquivalent", typeof(decimal));
			dtLoader.Columns.Add("ItemRate", typeof(decimal));
			dtLoader.Columns.Add("RateUOMId", typeof(int));
			dtLoader.Columns.Add("RateUomCode", typeof(string));
			dtLoader.Columns.Add("RateUomEquivalent", typeof(decimal));
			dtLoader.Columns.Add("ItemQty", typeof(decimal));
			dtLoader.Columns.Add("NetWeight", typeof(decimal));
			dtLoader.Columns.Add("TotalAmount", typeof(decimal));
			dtLoader.Columns.Add("RecQty", typeof(decimal));
			dtLoader.Columns.Add("RecWeight", typeof(decimal));
			dtLoader.Columns.Add("RecAmount", typeof(decimal));
			dtLoader.Columns.Add("BalQty", typeof(decimal));
			dtLoader.Columns.Add("BalNetWeight", typeof(decimal));
			dtLoader.Columns.Add("BalAmount", typeof(decimal));
			FromDate.Focus();
			FromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			SupplierNameFilll();
			PendingPurchaseOrderRegularForLoad();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierNameFilll()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(CmbSupplier.Value);
			DataTable dt = SupplierCustomer.GetSupplierCustomerFromOrderByInvoice(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dt, CmbSupplier, "OrderSupCustId", "CompanyName", "Supplier Name", ZeroIndex: true);
			if (Id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					CmbSupplier.Value = Id;
				}
				else
				{
					((Control)(object)CmbSupplier).Text = string.Empty;
				}
			}
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
			FromDate.Focus();
			((Control)(object)CmbSupplier).Text = string.Empty;
			txtOrderNoFrom.Text = string.Empty;
			txtOrderNoTo.Text = string.Empty;
			PendingPurchaseOrderRegularForLoad();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		dtLoader = null;
		Hide();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			SupplierNameFilll();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btngrnlod_Click(object sender, EventArgs e)
	{
		try
		{
			PendingPurchaseOrderRegularForLoad();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PendingPurchaseOrderRegularForLoad()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		try
		{
			DataTable dtloadPO = PurchaseOrder.LoadPurchaseOrderOnInvoiceForSalt(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				FromDate = Convert.ToDateTime(FromDate.Value),
				ToDate = Convert.ToDateTime(Todate.Value),
				DocumentTypeId = 1800,
				FromDocNo = Conversion.ToInt((object)txtOrderNoFrom.Text.Trim()),
				ToDocNo = Conversion.ToInt((object)txtOrderNoTo.Text.Trim()),
				SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value)
			});
			if (dtloadPO.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtcol = new DataTable();
			dtcol.Columns.Add("DocumentTypeId", typeof(int));
			dtcol.Columns.Add("DetailId", typeof(int));
			dtcol.Columns.Add("Id", typeof(int));
			dtcol.Columns.Add("DocDate", typeof(DateTime));
			dtcol.Columns.Add("DocNo", typeof(int));
			dtcol.Columns.Add("SupplierCustomerId", typeof(int));
			dtcol.Columns.Add("SupplierName", typeof(string));
			dtcol.Columns.Add("CommissionAgentId", typeof(int));
			dtcol.Columns.Add("CommissionAgent", typeof(string));
			dtcol.Columns.Add("CommissionType", typeof(string));
			dtcol.Columns.Add("CommRate", typeof(decimal));
			dtcol.Columns.Add("CommUom", typeof(decimal));
			dtcol.Columns.Add("CommAmount", typeof(decimal));
			dtcol.Columns.Add("CommissionRemarks", typeof(string));
			dtcol.Columns.Add("DeliveryTerm", typeof(string));
			dtcol.Columns.Add("PaymentTermsId", typeof(int));
			dtcol.Columns.Add("PaymentTerm", typeof(string));
			dtcol.Columns.Add("DueDate", typeof(DateTime));
			dtcol.Columns.Add("DueDays", typeof(int));
			dtcol.Columns.Add("JobLotId", typeof(int));
			dtcol.Columns.Add("JobLot", typeof(string));
			dtcol.Columns.Add("ItemId", typeof(int));
			dtcol.Columns.Add("ItemName", typeof(string));
			dtcol.Columns.Add("ItemCode", typeof(string));
			dtcol.Columns.Add("ItemUOMId", typeof(int));
			dtcol.Columns.Add("PackUomCode", typeof(string));
			dtcol.Columns.Add("PackUomEquivalent", typeof(decimal));
			dtcol.Columns.Add("ItemRate", typeof(decimal));
			dtcol.Columns.Add("RateUOMId", typeof(int));
			dtcol.Columns.Add("RateUomCode", typeof(string));
			dtcol.Columns.Add("RateUomEquivalent", typeof(decimal));
			dtcol.Columns.Add("ItemQty", typeof(decimal));
			dtcol.Columns.Add("NetWeight", typeof(decimal));
			dtcol.Columns.Add("TotalAmount", typeof(decimal));
			dtcol.Columns.Add("RecQty", typeof(decimal));
			dtcol.Columns.Add("RecWeight", typeof(decimal));
			dtcol.Columns.Add("RecAmount", typeof(decimal));
			dtcol.Columns.Add("BalQty", typeof(decimal));
			dtcol.Columns.Add("BalNetWeight", typeof(decimal));
			dtcol.Columns.Add("BalAmount", typeof(decimal));
			dtcol.Columns.Add("EntryDate", typeof(DateTime));
			dtcol.Columns.Add("EntryUserName", typeof(string));
			dtcol.Columns.Add("ModifyDate", typeof(DateTime));
			dtcol.Columns.Add("ModifyUserName", typeof(string));
			dtcol.Columns.Add("NoOfAttachments", typeof(int));
			dtcol.Rows.Clear();
			foreach (DataRow row in dtloadPO.Rows)
			{
				dtcol.Rows.Add(row["DocumentTypeId"], row["DetailId"], row["Id"], row["DocDate"], row["DocNo"], row["SupplierCustomerId"], row["SupplierName"], row["CommissionAgentId"], row["CommissionAgent"], row["CommissionType"], row["CommRate"], row["CommUom"], row["CommAmount"], row["CommissionRemarks"], row["DeliveryTerm"], row["PaymentTermsId"], row["PaymentTerm"], row["OrderDueDate"], row["OrderDueDays"], row["JobLotId"], row["JobLotDescription"], row["OrderItemId"], row["ItemName"], row["ItemCode"], row["OrderItemUOMId"], row["PackUomCode"], row["PackUomEquivalent"], row["OrderItemRate"], row["OrderItemRateUOMId"], row["RateUomCode"], row["RateUomEquivalent"], row["OrderItemQty"], row["NetWeight"], row["TotalAmount"], row["RecQty"], row["RecWeight"], row["RecAmount"], row["BalQty"], row["BalNetWeight"], row["BalAmount"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"], row["NoOfAttachments"]);
			}
			grd.DataSource = dtcol;
			grd.RetrieveStructure();
			grdSettings();
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
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grd.RootTable.Columns["DocumentTypeId"].Visible = false;
			grd.RootTable.Columns["DetailId"].Visible = false;
			grd.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["JobLotId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["ItemUOMId"].Visible = false;
			grd.RootTable.Columns["PackUomEquivalent"].Visible = false;
			grd.RootTable.Columns["RateUOMId"].Visible = false;
			grd.RootTable.Columns["RateUomEquivalent"].Visible = false;
			grd.RootTable.Columns["CommissionAgentId"].Visible = false;
			grd.RootTable.Columns["CommissionAgent"].Visible = false;
			grd.RootTable.Columns["CommAmount"].Visible = false;
			grd.RootTable.Columns["CommissionType"].Visible = false;
			grd.RootTable.Columns["CommUom"].Visible = false;
			grd.RootTable.Columns["CommRate"].Visible = false;
			grd.RootTable.Columns["CommAmount"].Visible = false;
			grd.RootTable.Columns["CommissionRemarks"].Visible = false;
			grd.RootTable.Columns["PaymentTermsId"].Visible = false;
			grd.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)6;
			grd.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemQty"].FormatString = "##,#.##";
			grd.RootTable.Columns["ItemQty"].TotalFormatString = "##,#.##";
			grd.RootTable.Columns["PackUomEquivalent"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["PackUomEquivalent"].FormatString = "##,#.##";
			grd.RootTable.Columns["PackUomEquivalent"].TotalFormatString = "##,#.##";
			grd.RootTable.Columns["RateUomEquivalent"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RateUomEquivalent"].FormatString = "##,#.##";
			grd.RootTable.Columns["RateUomEquivalent"].TotalFormatString = "##,#.##";
			grd.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetWeight"].FormatString = "##,#.##";
			grd.RootTable.Columns["NetWeight"].TotalFormatString = "##,#.##";
			grd.RootTable.Columns["TotalAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["TotalAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["TotalAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["TotalAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["CommRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["CommRate"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["CommRate"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["CommUom"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["CommUom"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["CommUom"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["CommAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["CommAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["CommAmount"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["CommAmount"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["ItemRate"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["ItemRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["RecQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["RecQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RecQty"].FormatString = "##,#.##";
			grd.RootTable.Columns["RecQty"].TotalFormatString = "##,#.##";
			grd.RootTable.Columns["RecWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["RecWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RecWeight"].FormatString = "##,#.##";
			grd.RootTable.Columns["RecWeight"].TotalFormatString = "##,#.##";
			grd.RootTable.Columns["RecAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["RecAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RecAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["RecAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["BalQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BalQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BalQty"].FormatString = "##,#.##";
			grd.RootTable.Columns["BalQty"].TotalFormatString = "##,#.##";
			grd.RootTable.Columns["BalNetWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BalNetWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BalNetWeight"].FormatString = "##,#.##";
			grd.RootTable.Columns["BalNetWeight"].TotalFormatString = "##,#.##";
			grd.RootTable.Columns["BalAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BalAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BalAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["BalAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grd);
			grd.RootTable.Columns.Add("Select");
			grd.RootTable.Columns["Select"].ActAsSelector = true;
			grd.RootTable.Columns["Select"].UseHeaderSelector = true;
			grd.RootTable.Columns["Select"].Width = 40;
			grd.RootTable.Columns["Select"].Position = 0;
			grd.RootTable.Columns["NoOfAttachments"].Position = 1;
			grd.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadOnInvoice_Click_1(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt((object)grd.GetCheckedRows().Count().ToString()) > 0)
			{
				int orderId = 0;
				dtLoader.Rows.Clear();
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					int Id = Conversion.ToInt(r.Cells["Id"].Value);
					if (orderId == 0)
					{
						orderId = Id;
					}
					if (orderId != Id)
					{
						dtLoader.Rows.Clear();
						MessageBox.Show("You Can Only Select Rows Of Same Order");
						return;
					}
					dtLoader.Rows.Add(Conversion.ToInt(r.Cells["DocumentTypeId"].Value), Conversion.ToInt(r.Cells["DetailId"].Value), Conversion.ToInt(r.Cells["Id"].Value), Conversion.ToDateTime(r.Cells["DocDate"].Value), Conversion.ToInt(r.Cells["DocNo"].Value), Conversion.ToInt(r.Cells["SupplierCustomerId"].Value), Conversion.ToString(r.Cells["SupplierName"].Value), Conversion.ToInt(r.Cells["CommissionAgentId"].Value), Conversion.ToString(r.Cells["CommissionType"].Value), Conversion.ToDecimal(r.Cells["CommRate"].Value), Conversion.ToDecimal(r.Cells["CommUom"].Value), Conversion.ToDecimal(r.Cells["CommAmount"].Value), Conversion.ToString(r.Cells["CommissionRemarks"].Value), Conversion.ToString(r.Cells["DeliveryTerm"].Value), Conversion.ToInt(r.Cells["PaymentTermsId"].Value), Conversion.ToDateTime(r.Cells["DueDate"].Value).ToShortDateString(), Conversion.ToInt(r.Cells["DueDays"].Value), Conversion.ToInt(r.Cells["JobLotId"].Value), Conversion.ToString(r.Cells["JobLot"].Value), Conversion.ToInt(r.Cells["ItemId"].Value), Conversion.ToString(r.Cells["ItemName"].Value), Conversion.ToString(r.Cells["ItemCode"].Value), Conversion.ToInt(r.Cells["ItemUOMId"].Value), Conversion.ToString(r.Cells["PackUomCode"].Value), Conversion.ToDecimal(r.Cells["PackUomEquivalent"].Value), Conversion.ToInt(r.Cells["RateUOMId"].Value), Conversion.ToDecimal(r.Cells["ItemRate"].Value), Conversion.ToString(r.Cells["RateUomCode"].Value), Conversion.ToDecimal(r.Cells["RateUomEquivalent"].Value), Conversion.ToDecimal(r.Cells["ItemQty"].Value), Conversion.ToDecimal(r.Cells["NetWeight"].Value), Conversion.ToDecimal(r.Cells["TotalAmount"].Value), Conversion.ToDecimal(r.Cells["RecQty"].Value), Conversion.ToDecimal(r.Cells["RecWeight"].Value), Conversion.ToDecimal(r.Cells["RecAmount"].Value), Conversion.ToDecimal(r.Cells["BalQty"].Value), Conversion.ToDecimal(r.Cells["BalNetWeight"].Value), Conversion.ToDecimal(r.Cells["BalAmount"].Value));
					orderId = Conversion.ToInt(r.Cells["Id"].Value);
				}
				Hide();
			}
			else
			{
				MessageBox.Show("Check the Row first");
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grd).Name);
			if (fs != null)
			{
				grd.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtOrderNoFrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtOrderNoTo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+L", "To Press Load Button");
			dt.Rows.Add("Ctrl+S", "For Search");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
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

	private void btnShortcutKeys_Click(object sender, EventArgs e)
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

	private void LoadPurchaseOrder_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			else if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				dtLoader = null;
				Hide();
			}
			else if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			else if (e.Control && e.KeyCode == Keys.S)
			{
				btngrnlod_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.L)
			{
				btnLoadOnInvoice_Click_1(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.N)
			{
				btnReset_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.F5)
			{
				FromDate.Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grd).Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Up)
			{
				FromDate.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["Id"].Value), 1800);
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
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected O, but got Unknown
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Expected O, but got Unknown
		//IL_0776: Unknown result type (might be due to invalid IL or missing references)
		//IL_0780: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.PurchaseForSalt.LoadPurchaseOrder));
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnShortcutKeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.label23 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grd = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label1 = new System.Windows.Forms.Label();
		this.btnReset = new System.Windows.Forms.Button();
		this.txtOrderNoTo = new System.Windows.Forms.TextBox();
		this.btnLoadOnInvoice = new System.Windows.Forms.Button();
		this.CmbSupplier = new UltraCombo();
		this.btngrnlod = new System.Windows.Forms.Button();
		this.label2 = new System.Windows.Forms.Label();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.txtOrderNoFrom = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.Todate = new System.Windows.Forms.DateTimePicker();
		this.label4 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel1.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel2.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		this.panel3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Controls.Add(this.label23);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(964, 50);
		this.panel1.TabIndex = 0;
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.toolStripSeparator6, this.btnNew, this.btnRefresh, this.toolStripSeparator2, this.btnShortcutKeys, this.toolStripSeparator1 });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(964, 25);
		this.toolStrip1.TabIndex = 27;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 22);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnReset_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 22);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
		this.btnShortcutKeys.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnShortcutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortcutKeys.Image = (System.Drawing.Image)resources.GetObject("btnShortcutKeys.Image");
		this.btnShortcutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcutKeys.Name = "btnShortcutKeys";
		this.btnShortcutKeys.Size = new System.Drawing.Size(106, 22);
		this.btnShortcutKeys.Text = "ShortCut Keys";
		this.btnShortcutKeys.Click += new System.EventHandler(btnShortcutKeys_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.Teal;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 27);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(178, 19);
		this.label23.TabIndex = 24;
		this.label23.Text = "Purchase Order Load";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(915, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 26);
		this.ctrlGrdBar1.TabIndex = 10;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grd.AllowDelete = (InheritableBoolean)1;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 156);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(964, 305);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.groupBox1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 50);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(964, 77);
		this.panel2.TabIndex = 0;
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.btnReset);
		this.groupBox1.Controls.Add(this.txtOrderNoTo);
		this.groupBox1.Controls.Add(this.btnLoadOnInvoice);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.groupBox1.Controls.Add(this.btngrnlod);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.FromDate);
		this.groupBox1.Controls.Add(this.txtOrderNoFrom);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.Todate);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(5, -1);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(747, 75);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(7, 21);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 15);
		this.label1.TabIndex = 12;
		this.label1.Text = "From Date";
		this.btnReset.BackColor = System.Drawing.Color.Teal;
		this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnReset.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnReset.ForeColor = System.Drawing.Color.White;
		this.btnReset.Location = new System.Drawing.Point(674, 44);
		this.btnReset.Name = "btnReset";
		this.btnReset.Size = new System.Drawing.Size(61, 26);
		this.btnReset.TabIndex = 7;
		this.btnReset.Text = "Reset";
		this.btnReset.UseVisualStyleBackColor = false;
		this.btnReset.Click += new System.EventHandler(btnReset_Click);
		this.txtOrderNoTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderNoTo.Location = new System.Drawing.Point(281, 46);
		this.txtOrderNoTo.Name = "txtOrderNoTo";
		this.txtOrderNoTo.Size = new System.Drawing.Size(114, 23);
		this.txtOrderNoTo.TabIndex = 3;
		this.txtOrderNoTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOrderNoTo_KeyPress);
		this.btnLoadOnInvoice.BackColor = System.Drawing.Color.Teal;
		this.btnLoadOnInvoice.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnLoadOnInvoice.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadOnInvoice.ForeColor = System.Drawing.Color.White;
		this.btnLoadOnInvoice.Location = new System.Drawing.Point(608, 44);
		this.btnLoadOnInvoice.Name = "btnLoadOnInvoice";
		this.btnLoadOnInvoice.Size = new System.Drawing.Size(64, 26);
		this.btnLoadOnInvoice.TabIndex = 6;
		this.btnLoadOnInvoice.Text = "Load";
		this.btnLoadOnInvoice.UseVisualStyleBackColor = false;
		this.btnLoadOnInvoice.Click += new System.EventHandler(btnLoadOnInvoice_Click_1);
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(493, 15);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(243, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 4;
		this.btngrnlod.BackColor = System.Drawing.Color.Teal;
		this.btngrnlod.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btngrnlod.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btngrnlod.ForeColor = System.Drawing.Color.White;
		this.btngrnlod.Location = new System.Drawing.Point(536, 44);
		this.btngrnlod.Name = "btngrnlod";
		this.btngrnlod.Size = new System.Drawing.Size(70, 26);
		this.btngrnlod.TabIndex = 5;
		this.btngrnlod.Text = "Search";
		this.btngrnlod.UseVisualStyleBackColor = false;
		this.btngrnlod.Click += new System.EventHandler(btngrnlod_Click);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(401, 21);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(86, 15);
		this.label2.TabIndex = 24;
		this.label2.Text = "Supplier Name";
		this.FromDate.CustomFormat = "dd-MM-yy";
		this.FromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(73, 17);
		this.FromDate.Name = "FromDate";
		this.FromDate.Size = new System.Drawing.Size(116, 23);
		this.FromDate.TabIndex = 0;
		this.txtOrderNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderNoFrom.Location = new System.Drawing.Point(281, 17);
		this.txtOrderNoFrom.Name = "txtOrderNoFrom";
		this.txtOrderNoFrom.Size = new System.Drawing.Size(114, 23);
		this.txtOrderNoFrom.TabIndex = 2;
		this.txtOrderNoFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOrderNoFrom_KeyPress);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(7, 50);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(48, 15);
		this.label13.TabIndex = 11;
		this.label13.Text = "To Date";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(194, 50);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(72, 15);
		this.label3.TabIndex = 28;
		this.label3.Text = "Order No To";
		this.Todate.CustomFormat = "dd-MM-yy";
		this.Todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.Todate.Location = new System.Drawing.Point(73, 46);
		this.Todate.Name = "Todate";
		this.Todate.Size = new System.Drawing.Size(116, 23);
		this.Todate.TabIndex = 1;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(194, 21);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(87, 15);
		this.label4.TabIndex = 28;
		this.label4.Text = "Order No From";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 127);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(964, 29);
		this.panel3.TabIndex = 31;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(964, 461);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.Name = "LoadPurchaseOrder";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(LoadInvoices_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(LoadPurchaseOrder_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel2.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		this.panel3.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
