using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.Common;
using Architecture.Model;
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
namespace Architecture.WinApp.ExportReports
{

public class frmContractCurrentStatusReport : Form
{
	private DataTable dtHistory = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	public UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Panel panel4;

	private Panel panel3;

	private Panel panel2;

	private ToolStrip tsItemType;

	private ToolStripButton btnnew;

	internal GridEX grdfrm;

	internal UltraCombo cmbItemName;

	private Label label1;

	internal UltraCombo cmbSupplierName;

	private Label label50;

	private Button btnShow;

	private Panel panel5;

	private Label label3;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnShortCutKey;

	private ToolStripButton btnPrintRegister;

	private CtrlGrdBar ctrlGrdBar1;

	internal UltraCombo cmbperemeter;

	private Label label11;

	private DateTimePicker txtdateto;

	private Label label5;

	private DateTimePicker txtdatefrom;

	private Label label51;

	private Label label7;

	private TextBox txtMton;

	private Panel panel6;

	private Panel panel9;

	private Label label8;

	private Panel panel8;

	private Label label6;

	private Panel panel7;

	private Label label4;

	private Label label12;

	private Label label10;

	private Label label9;

	private Label txtBalanceMtons;

	private Label txtForwardedMtons;

	private Label txtTotalMtons;

	private Label txtBalanceFcl;

	private Label txtForwardedFcl;

	private Label txtTotalFcl;

	private Panel panel11;

	internal GridEX grdSupplierWise;

	private Label label14;

	private Panel panel10;

	private Label label13;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel12;

	public frmContractCurrentStatusReport(UserAccount pUserAccount)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void frmExportShipingLineBookingRpt_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1400, 700);
			ParameterFill();
			((UltraGridBase)cmbperemeter).Rows[1].Activate();
			cmbperemeter.Focus();
			AllComboBind();
			GridBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ParameterFill()
	{
		try
		{
			DataTable dt = CommonServices.DateType();
			DDL.BindDDL(dt, cmbperemeter, "Id", "Parameters", "Date Type", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllComboBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = ExImLcOrder.GetDataForDropDownFromExportContract(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id");
			dtCustomer.Columns.Add("CustomerName");
			DataTable dtItem = new DataTable();
			dtItem.Columns.Add("Id");
			dtItem.Columns.Add("Item");
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToString(dt.Rows[i]["ActivityType"]) == "Customer")
				{
					dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
				}
				else if (Conversion.ToString(dt.Rows[i]["ActivityType"]) == "Items")
				{
					dtItem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
				}
			}
			if (dtCustomer.Rows.Count > 0)
			{
				DDL.BindDDL(dtCustomer, cmbSupplierName, "Id", "CustomerName", "Customer Name", ZeroIndex: false);
			}
			else
			{
				((UltraGridBase)cmbSupplierName).DataSource = null;
			}
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDL(dtItem, cmbItemName, "Id", "Item", "Item Name", ZeroIndex: false);
			}
			else
			{
				((UltraGridBase)cmbItemName).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			GridBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void GridBind()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtHistory.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FromDate = txtdatefrom.Value;
			obj.ToDate = txtdateto.Value;
			obj.SupplierCustomerId = Conversion.ToInt(cmbSupplierName.Value);
			obj.ItemId = Conversion.ToInt(cmbItemName.Value);
			obj.NetWeight = Conversion.ToDouble((object)txtMton.Text);
			dtHistory = ExImLcOrder.ExImLcContractDashbaordAReport(obj);
			if (dtHistory.Rows.Count > 0)
			{
				txtTotalFcl.Text = Conversion.ToDouble(dtHistory.Rows[0]["TotFclQty"]).ToString("#,##0.####");
				txtForwardedFcl.Text = Conversion.ToDouble(dtHistory.Rows[0]["FwdFclQty"]).ToString("#,##0.####");
				txtBalanceFcl.Text = Conversion.ToDouble(dtHistory.Rows[0]["BalFclQty"]).ToString("#,##0.####");
				txtTotalMtons.Text = Conversion.ToDouble(dtHistory.Rows[0]["TotMTons"]).ToString("#,##0.####");
				txtForwardedMtons.Text = Conversion.ToDouble(dtHistory.Rows[0]["FwdMTons"]).ToString("#,##0.####");
				txtBalanceMtons.Text = Conversion.ToDouble(dtHistory.Rows[0]["BalMtons"]).ToString("#,##0.####");
				EnumerableRowCollection<DataRow> rows = from r in dtHistory.AsEnumerable()
					where r.Field<string>("ActivityDescription") == "CustomerWiseTotal"
					select r;
				DataTable SupplierWise = (rows.Any() ? rows.CopyToDataTable() : dtHistory.Clone());
				EnumerableRowCollection<DataRow> rows2 = from r in dtHistory.AsEnumerable()
					where r.Field<string>("ActivityDescription") == "ItemWiseTotal"
					select r;
				DataTable ItemWise = (rows2.Any() ? rows2.CopyToDataTable() : dtHistory.Clone());
				if (SupplierWise.Rows.Count > 0)
				{
					DataTable dt2 = new DataTable();
					dt2.Columns.Add("Id");
					dt2.Columns.Add("Customer", typeof(string));
					dt2.Columns.Add("TotalFcl", typeof(double));
					dt2.Columns.Add("TotalMTons", typeof(double));
					dt2.Columns.Add("FwdFcl", typeof(double));
					dt2.Columns.Add("FwdMTons", typeof(double));
					dt2.Columns.Add("BalFcl", typeof(double));
					dt2.Columns.Add("BalMTons", typeof(double));
					for (int j = 0; j < SupplierWise.Rows.Count; j++)
					{
						dt2.Rows.Add(SupplierWise.Rows[j]["TitleId"], SupplierWise.Rows[j]["TitleDescription"], SupplierWise.Rows[j]["TotFclQty"], SupplierWise.Rows[j]["TotMTons"], SupplierWise.Rows[j]["FwdFclQty"], SupplierWise.Rows[j]["FwdMTons"], SupplierWise.Rows[j]["BalFclQty"], SupplierWise.Rows[j]["BalMtons"]);
					}
					grdSupplierWise.DataSource = dt2;
					grdSupplierWise.RetrieveStructure();
					grdSupplierWise.RootTable.Columns["Id"].Visible = false;
					grdSupplierWise.RootTable.Columns["Customer"].Width = 300;
					grdSupplierWise.RootTable.Columns["TotalFcl"].Width = 80;
					grdSupplierWise.RootTable.Columns["TotalMTons"].Width = 80;
					grdSupplierWise.RootTable.Columns["FwdFcl"].Width = 80;
					grdSupplierWise.RootTable.Columns["FwdMTons"].Width = 80;
					grdSupplierWise.RootTable.Columns["BalFcl"].Width = 80;
					grdSupplierWise.RootTable.Columns["BalMTons"].Width = 80;
					grdSupplierWise.RootTable.Columns["TotalFcl"].AggregateFunction = (AggregateFunction)2;
					grdSupplierWise.RootTable.Columns["TotalFcl"].TextAlignment = (TextAlignment)3;
					grdSupplierWise.RootTable.Columns["TotalFcl"].FormatString = "#,##0.####";
					grdSupplierWise.RootTable.Columns["TotalFcl"].TotalFormatString = "#,##0.####";
					grdSupplierWise.RootTable.Columns["TotalMTons"].AggregateFunction = (AggregateFunction)2;
					grdSupplierWise.RootTable.Columns["TotalMTons"].TextAlignment = (TextAlignment)3;
					grdSupplierWise.RootTable.Columns["TotalMTons"].FormatString = "#,##0.####";
					grdSupplierWise.RootTable.Columns["TotalMTons"].TotalFormatString = "#,##0.####";
					grdSupplierWise.RootTable.Columns["FwdFcl"].AggregateFunction = (AggregateFunction)2;
					grdSupplierWise.RootTable.Columns["FwdFcl"].TextAlignment = (TextAlignment)3;
					grdSupplierWise.RootTable.Columns["FwdFcl"].FormatString = "#,##0.####";
					grdSupplierWise.RootTable.Columns["FwdFcl"].TotalFormatString = "#,##0.####";
					grdSupplierWise.RootTable.Columns["FwdMTons"].AggregateFunction = (AggregateFunction)2;
					grdSupplierWise.RootTable.Columns["FwdMTons"].TextAlignment = (TextAlignment)3;
					grdSupplierWise.RootTable.Columns["FwdMTons"].FormatString = "#,##0.####";
					grdSupplierWise.RootTable.Columns["FwdMTons"].TotalFormatString = "#,##0.####";
					grdSupplierWise.RootTable.Columns["BalFcl"].AggregateFunction = (AggregateFunction)2;
					grdSupplierWise.RootTable.Columns["BalFcl"].TextAlignment = (TextAlignment)3;
					grdSupplierWise.RootTable.Columns["BalFcl"].FormatString = "#,##0.####";
					grdSupplierWise.RootTable.Columns["BalFcl"].TotalFormatString = "#,##0.####";
					grdSupplierWise.RootTable.Columns["BalMTons"].AggregateFunction = (AggregateFunction)2;
					grdSupplierWise.RootTable.Columns["BalMTons"].TextAlignment = (TextAlignment)3;
					grdSupplierWise.RootTable.Columns["BalMTons"].FormatString = "#,##0.####";
					grdSupplierWise.RootTable.Columns["BalMTons"].TotalFormatString = "#,##0.####";
					ctrlGrdBar2_Load(null, null);
				}
				else
				{
					grdSupplierWise.ClearStructure();
				}
				if (ItemWise.Rows.Count > 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("Id");
					dt.Columns.Add("Item", typeof(string));
					dt.Columns.Add("TotalFcl", typeof(double));
					dt.Columns.Add("TotalMTons", typeof(double));
					dt.Columns.Add("FwdFcl", typeof(double));
					dt.Columns.Add("FwdMTons", typeof(double));
					dt.Columns.Add("BalFcl", typeof(double));
					dt.Columns.Add("BalMTons", typeof(double));
					for (int i = 0; i < ItemWise.Rows.Count; i++)
					{
						dt.Rows.Add(ItemWise.Rows[i]["TitleId"], ItemWise.Rows[i]["TitleDescription"], ItemWise.Rows[i]["TotFclQty"], ItemWise.Rows[i]["TotMTons"], ItemWise.Rows[i]["FwdFclQty"], ItemWise.Rows[i]["FwdMTons"], ItemWise.Rows[i]["BalFclQty"], ItemWise.Rows[i]["BalMtons"]);
					}
					grdfrm.DataSource = dt;
					grdfrm.RetrieveStructure();
					grdfrm.RootTable.Columns["Id"].Visible = false;
					grdfrm.RootTable.Columns["Item"].Width = 222;
					grdfrm.RootTable.Columns["TotalFcl"].Width = 80;
					grdfrm.RootTable.Columns["TotalMTons"].Width = 80;
					grdfrm.RootTable.Columns["FwdFcl"].Width = 80;
					grdfrm.RootTable.Columns["FwdMTons"].Width = 80;
					grdfrm.RootTable.Columns["BalFcl"].Width = 80;
					grdfrm.RootTable.Columns["BalMTons"].Width = 80;
					grdfrm.RootTable.Columns["TotalFcl"].AggregateFunction = (AggregateFunction)2;
					grdfrm.RootTable.Columns["TotalFcl"].TextAlignment = (TextAlignment)3;
					grdfrm.RootTable.Columns["TotalFcl"].FormatString = "#,##0.####";
					grdfrm.RootTable.Columns["TotalFcl"].TotalFormatString = "#,##0.####";
					grdfrm.RootTable.Columns["TotalMTons"].AggregateFunction = (AggregateFunction)2;
					grdfrm.RootTable.Columns["TotalMTons"].TextAlignment = (TextAlignment)3;
					grdfrm.RootTable.Columns["TotalMTons"].FormatString = "#,##0.####";
					grdfrm.RootTable.Columns["TotalMTons"].TotalFormatString = "#,##0.####";
					grdfrm.RootTable.Columns["FwdFcl"].AggregateFunction = (AggregateFunction)2;
					grdfrm.RootTable.Columns["FwdFcl"].TextAlignment = (TextAlignment)3;
					grdfrm.RootTable.Columns["FwdFcl"].FormatString = "#,##0.####";
					grdfrm.RootTable.Columns["FwdFcl"].TotalFormatString = "#,##0.####";
					grdfrm.RootTable.Columns["FwdMTons"].AggregateFunction = (AggregateFunction)2;
					grdfrm.RootTable.Columns["FwdMTons"].TextAlignment = (TextAlignment)3;
					grdfrm.RootTable.Columns["FwdMTons"].FormatString = "#,##0.####";
					grdfrm.RootTable.Columns["FwdMTons"].TotalFormatString = "#,##0.####";
					grdfrm.RootTable.Columns["BalFcl"].AggregateFunction = (AggregateFunction)2;
					grdfrm.RootTable.Columns["BalFcl"].TextAlignment = (TextAlignment)3;
					grdfrm.RootTable.Columns["BalFcl"].FormatString = "#,##0.####";
					grdfrm.RootTable.Columns["BalFcl"].TotalFormatString = "#,##0.####";
					grdfrm.RootTable.Columns["BalMTons"].AggregateFunction = (AggregateFunction)2;
					grdfrm.RootTable.Columns["BalMTons"].TextAlignment = (TextAlignment)3;
					grdfrm.RootTable.Columns["BalMTons"].FormatString = "#,##0.####";
					grdfrm.RootTable.Columns["BalMTons"].TotalFormatString = "#,##0.####";
					ctrlGrdBar1_Load(null, null);
				}
				else
				{
					grdfrm.ClearStructure();
				}
			}
			else
			{
				grdSupplierWise.ClearStructure();
				grdfrm.ClearStructure();
			}
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

	public void Reset()
	{
		try
		{
			((Control)(object)cmbSupplierName).Text = string.Empty;
			((Control)(object)cmbItemName).Text = string.Empty;
			txtMton.Text = 24.ToString();
			cmbSupplierName.Focus();
			grdfrm.ClearStructure();
			grdSupplierWise.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrintRegister_Click(object sender, EventArgs e)
	{
		try
		{
			ShowRegister();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ShowRegister()
	{
	}

	private void frmExportShipingLineBookingRpt_KeyDown(object sender, KeyEventArgs e)
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
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				btnPrintRegister_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnShow_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				cmbSupplierName.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				if (((Control)(object)grdfrm).Focused)
				{
					((Control)(object)grdSupplierWise).Focus();
				}
				else
				{
					((Control)(object)grdfrm).Focus();
				}
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

	private void btnclose_Click(object sender, EventArgs e)
	{
		try
		{
			Close();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortCutKey_Click(object sender, EventArgs e)
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

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+S", "For Showing Data");
			dt.Rows.Add("Ctrl+F5", "For Focus on Customer");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grids");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
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
			AllComboBind();
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdfrm).Name);
			if (fs != null)
			{
				grdfrm.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbperemeter_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbperemeter.Value) == 1)
			{
				txtdatefrom.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 2)
			{
				txtdatefrom.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				txtdatefrom.Value = first;
				txtdateto.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				txtdatefrom.Value = first2;
				txtdateto.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 5)
			{
				txtdatefrom.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdSupplierWise).Name);
			if (fs != null)
			{
				grdSupplierWise.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmContractCurrentStatusReport_FormClosed(object sender, FormClosedEventArgs e)
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
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Expected O, but got Unknown
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Expected O, but got Unknown
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Expected O, but got Unknown
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Expected O, but got Unknown
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Expected O, but got Unknown
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
		Appearance appearance18 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel11 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdSupplierWise = new GridEX();
		this.label14 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.grdfrm = new GridEX();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label13 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.btnShow = new System.Windows.Forms.Button();
		this.txtMton = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.txtBalanceMtons = new System.Windows.Forms.Label();
		this.txtForwardedMtons = new System.Windows.Forms.Label();
		this.txtTotalMtons = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.txtBalanceFcl = new System.Windows.Forms.Label();
		this.txtForwardedFcl = new System.Windows.Forms.Label();
		this.txtTotalFcl = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label12 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.cmbperemeter = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.txtdateto = new System.Windows.Forms.DateTimePicker();
		this.label5 = new System.Windows.Forms.Label();
		this.txtdatefrom = new System.Windows.Forms.DateTimePicker();
		this.label51 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.cmbItemName = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.cmbSupplierName = new UltraCombo();
		this.label50 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnPrintRegister = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSupplierWise).BeginInit();
		this.panel10.SuspendLayout();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel3.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierName).BeginInit();
		this.panel2.SuspendLayout();
		this.tsItemType.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1397, 612);
		this.panel1.TabIndex = 0;
		this.panel4.Controls.Add(this.panel11);
		this.panel4.Controls.Add(this.panel10);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 196);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1397, 416);
		this.panel4.TabIndex = 2;
		this.panel11.Controls.Add(this.ctrlGrdBar2);
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.grdSupplierWise);
		this.panel11.Controls.Add(this.label14);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(707, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(690, 416);
		this.panel11.TabIndex = 5;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.BackColor = System.Drawing.Color.White;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(649, 2);
		this.ctrlGrdBar2.MyGrid = this.grdSupplierWise;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 11;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.grdSupplierWise.AllowEdit = (InheritableBoolean)2;
		this.grdSupplierWise.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.grdSupplierWise).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdSupplierWise.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdSupplierWise).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSupplierWise.GroupByBoxVisible = false;
		this.grdSupplierWise.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdSupplierWise).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.grdSupplierWise).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdSupplierWise).Name = "grdSupplierWise";
		((ScrollableBase)this.grdSupplierWise).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdSupplierWise).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdSupplierWise).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSupplierWise).Size = new System.Drawing.Size(690, 390);
		((System.Windows.Forms.Control)(object)this.grdSupplierWise).TabIndex = 4;
		this.grdSupplierWise.TotalRow = (InheritableBoolean)1;
		this.grdSupplierWise.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSupplierWise).VisualStyle = (VisualStyle)4;
		this.label14.BackColor = System.Drawing.Color.Teal;
		this.label14.Dock = System.Windows.Forms.DockStyle.Top;
		this.label14.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(0, 0);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(690, 26);
		this.label14.TabIndex = 2;
		this.label14.Text = "Customer Wise";
		this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel10.Controls.Add(this.panel12);
		this.panel10.Controls.Add(this.ctrlGrdBar1);
		this.panel10.Controls.Add(this.label13);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(707, 416);
		this.panel10.TabIndex = 4;
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel12.Location = new System.Drawing.Point(0, 26);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(707, 390);
		this.panel12.TabIndex = 11;
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		this.grdfrm.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupByBoxVisible = false;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdfrm).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(707, 390);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 3;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.BackColor = System.Drawing.Color.White;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(665, 2);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 10;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label13.BackColor = System.Drawing.Color.Teal;
		this.label13.Dock = System.Windows.Forms.DockStyle.Top;
		this.label13.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(0, 0);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(707, 26);
		this.label13.TabIndex = 1;
		this.label13.Text = "Item Wise";
		this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.label7);
		this.panel3.Controls.Add(this.btnShow);
		this.panel3.Controls.Add(this.txtMton);
		this.panel3.Controls.Add(this.panel6);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbperemeter);
		this.panel3.Controls.Add(this.label11);
		this.panel3.Controls.Add(this.txtdateto);
		this.panel3.Controls.Add(this.label5);
		this.panel3.Controls.Add(this.txtdatefrom);
		this.panel3.Controls.Add(this.label51);
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemName);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupplierName);
		this.panel3.Controls.Add(this.label50);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1397, 169);
		this.panel3.TabIndex = 1;
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(275, 33);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(59, 26);
		this.btnShow.TabIndex = 7;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.txtMton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMton.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMton.Location = new System.Drawing.Point(64, 35);
		this.txtMton.Name = "txtMton";
		this.txtMton.Size = new System.Drawing.Size(210, 23);
		this.txtMton.TabIndex = 5;
		this.txtMton.Text = "24";
		this.txtMton.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(5, 35);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(54, 15);
		this.label7.TabIndex = 596;
		this.label7.Text = "Fcl Mton";
		this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel6.Controls.Add(this.panel9);
		this.panel6.Controls.Add(this.panel8);
		this.panel6.Controls.Add(this.panel7);
		this.panel6.Location = new System.Drawing.Point(8, 61);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(326, 105);
		this.panel6.TabIndex = 597;
		this.panel9.Controls.Add(this.txtBalanceMtons);
		this.panel9.Controls.Add(this.txtForwardedMtons);
		this.panel9.Controls.Add(this.txtTotalMtons);
		this.panel9.Controls.Add(this.label8);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel9.Location = new System.Drawing.Point(200, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(124, 103);
		this.panel9.TabIndex = 3;
		this.txtBalanceMtons.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceMtons.Location = new System.Drawing.Point(6, 80);
		this.txtBalanceMtons.Name = "txtBalanceMtons";
		this.txtBalanceMtons.Size = new System.Drawing.Size(117, 16);
		this.txtBalanceMtons.TabIndex = 6;
		this.txtBalanceMtons.Text = "0";
		this.txtBalanceMtons.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.txtForwardedMtons.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtForwardedMtons.Location = new System.Drawing.Point(6, 55);
		this.txtForwardedMtons.Name = "txtForwardedMtons";
		this.txtForwardedMtons.Size = new System.Drawing.Size(117, 16);
		this.txtForwardedMtons.TabIndex = 5;
		this.txtForwardedMtons.Text = "0";
		this.txtForwardedMtons.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.txtTotalMtons.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalMtons.Location = new System.Drawing.Point(6, 30);
		this.txtTotalMtons.Name = "txtTotalMtons";
		this.txtTotalMtons.Size = new System.Drawing.Size(117, 16);
		this.txtTotalMtons.TabIndex = 4;
		this.txtTotalMtons.Text = "0";
		this.txtTotalMtons.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label8.Dock = System.Windows.Forms.DockStyle.Top;
		this.label8.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.Teal;
		this.label8.Location = new System.Drawing.Point(0, 0);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(124, 28);
		this.label8.TabIndex = 1;
		this.label8.Text = "MTons";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel8.Controls.Add(this.txtBalanceFcl);
		this.panel8.Controls.Add(this.txtForwardedFcl);
		this.panel8.Controls.Add(this.txtTotalFcl);
		this.panel8.Controls.Add(this.label6);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel8.Location = new System.Drawing.Point(102, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(98, 103);
		this.panel8.TabIndex = 2;
		this.txtBalanceFcl.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceFcl.Location = new System.Drawing.Point(6, 80);
		this.txtBalanceFcl.Name = "txtBalanceFcl";
		this.txtBalanceFcl.Size = new System.Drawing.Size(86, 16);
		this.txtBalanceFcl.TabIndex = 6;
		this.txtBalanceFcl.Text = "0";
		this.txtBalanceFcl.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.txtForwardedFcl.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtForwardedFcl.Location = new System.Drawing.Point(6, 55);
		this.txtForwardedFcl.Name = "txtForwardedFcl";
		this.txtForwardedFcl.Size = new System.Drawing.Size(86, 16);
		this.txtForwardedFcl.TabIndex = 5;
		this.txtForwardedFcl.Text = "0";
		this.txtForwardedFcl.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.txtTotalFcl.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalFcl.Location = new System.Drawing.Point(6, 30);
		this.txtTotalFcl.Name = "txtTotalFcl";
		this.txtTotalFcl.Size = new System.Drawing.Size(86, 16);
		this.txtTotalFcl.TabIndex = 4;
		this.txtTotalFcl.Text = "0";
		this.txtTotalFcl.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label6.Dock = System.Windows.Forms.DockStyle.Top;
		this.label6.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.Teal;
		this.label6.Location = new System.Drawing.Point(0, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(98, 28);
		this.label6.TabIndex = 1;
		this.label6.Text = "Fcl";
		this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel7.Controls.Add(this.label12);
		this.panel7.Controls.Add(this.label10);
		this.panel7.Controls.Add(this.label9);
		this.panel7.Controls.Add(this.label4);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(102, 103);
		this.panel7.TabIndex = 1;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(17, 81);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(57, 16);
		this.label12.TabIndex = 3;
		this.label12.Text = "Balance";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(8, 56);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(76, 16);
		this.label10.TabIndex = 2;
		this.label10.Text = "Forwarded";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(26, 31);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(39, 16);
		this.label9.TabIndex = 1;
		this.label9.Text = "Total";
		this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label4.Dock = System.Windows.Forms.DockStyle.Top;
		this.label4.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.Teal;
		this.label4.Location = new System.Drawing.Point(0, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(102, 28);
		this.label4.TabIndex = 0;
		this.label4.Text = "Description";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.cmbperemeter.AllowNull = (DefaultableBoolean)1;
		this.cmbperemeter.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbperemeter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbperemeter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbperemeter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Location = new System.Drawing.Point(868, 376);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Name = "cmbperemeter";
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Size = new System.Drawing.Size(123, 26);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).TabIndex = 0;
		this.cmbperemeter.ValueChanged += new System.EventHandler(cmbperemeter_ValueChanged);
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(797, 382);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(60, 15);
		this.label11.TabIndex = 594;
		this.label11.Text = "Date Type";
		this.txtdateto.CustomFormat = "dd-MMM-yy";
		this.txtdateto.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdateto.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdateto.Location = new System.Drawing.Point(868, 434);
		this.txtdateto.Name = "txtdateto";
		this.txtdateto.Size = new System.Drawing.Size(123, 23);
		this.txtdateto.TabIndex = 2;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(797, 438);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(48, 15);
		this.label5.TabIndex = 592;
		this.label5.Text = "Date To";
		this.txtdatefrom.CustomFormat = "dd-MMM-yy";
		this.txtdatefrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdatefrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdatefrom.Location = new System.Drawing.Point(868, 406);
		this.txtdatefrom.Name = "txtdatefrom";
		this.txtdatefrom.Size = new System.Drawing.Size(123, 23);
		this.txtdatefrom.TabIndex = 1;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(797, 410);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(63, 15);
		this.label51.TabIndex = 590;
		this.label51.Text = "Date From";
		this.panel5.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1397, 29);
		this.panel5.TabIndex = 20;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(4, 4);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(270, 19);
		this.label3.TabIndex = 0;
		this.label3.Text = "Export Contracts Current Status";
		this.cmbItemName.AllowNull = (DefaultableBoolean)1;
		this.cmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance35).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemName).Location = new System.Drawing.Point(1138, 403);
		((System.Windows.Forms.Control)(object)this.cmbItemName).Name = "cmbItemName";
		((System.Windows.Forms.Control)(object)this.cmbItemName).Size = new System.Drawing.Size(197, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemName).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.cmbItemName).Visible = false;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(1038, 409);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(67, 15);
		this.label1.TabIndex = 17;
		this.label1.Text = "Item Name";
		this.label1.Visible = false;
		this.cmbSupplierName.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplierName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplierName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplierName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance21;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance27).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSupplierName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).Location = new System.Drawing.Point(1138, 375);
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).Name = "cmbSupplierName";
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).Size = new System.Drawing.Size(197, 26);
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).Visible = false;
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(1038, 381);
		this.label50.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(93, 15);
		this.label50.TabIndex = 16;
		this.label50.Text = "Customer Name";
		this.label50.Visible = false;
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.tsItemType);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1397, 27);
		this.panel2.TabIndex = 0;
		this.tsItemType.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnnew, this.btnRefresh, this.btnPrintRegister, this.btnShortCutKey });
		this.tsItemType.Location = new System.Drawing.Point(0, 0);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(1397, 27);
		this.tsItemType.TabIndex = 3;
		this.tsItemType.Text = "toolStrip4";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(58, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnPrintRegister.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintRegister.Name = "btnPrintRegister";
		this.btnPrintRegister.Size = new System.Drawing.Size(61, 24);
		this.btnPrintRegister.Text = "Print";
		this.btnPrintRegister.Visible = false;
		this.btnPrintRegister.Click += new System.EventHandler(btnPrintRegister_Click);
		this.btnShortCutKey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1397, 612);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmContractCurrentStatusReport";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmContractCurrentStatusReport_FormClosed);
		base.Load += new System.EventHandler(frmExportShipingLineBookingRpt_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmExportShipingLineBookingRpt_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSupplierWise).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierName).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.tsItemType.ResumeLayout(false);
		this.tsItemType.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
