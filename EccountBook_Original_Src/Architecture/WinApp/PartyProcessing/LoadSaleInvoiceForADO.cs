using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.PartyProcessing;

public class LoadSaleInvoiceForADO : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DataTable dtRecordsFromDb = new DataTable();

	private DataTable dtGrid = new DataTable();

	public DataTable dtLoader = new DataTable();

	private bool FormInitialized;

	private bool AllowMultipleOrPartialInvoicingForADO;

	private IContainer components;

	private Panel panel1;

	private Label label23;

	private Panel panel2;

	private Button BtnShow;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel3;

	internal GridEX grd;

	private Button btnLoadOnInvoice;

	private DateTimePicker Todate;

	private DateTimePicker FromDate;

	private Label label2;

	private Label label3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnRefresh;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripButton btnShortcutKeys;

	private ToolStripSeparator toolStripSeparator1;

	internal UltraCombo CmbCustomer;

	private Label label1;

	public LoadSaleInvoiceForADO(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InitializeComponentCustom();
		InitializeComponentMethod();
	}

	private void InitializeComponentCustom()
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 500);
			FromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InitializeComponentMethod()
	{
		Task.Run(delegate
		{
			DataTable comboData = ComboDbCall();
			PendingDataDbCall();
			return new
			{
				ComboData = comboData
			};
		}).ContinueWith(t =>
		{
			if (t.IsFaulted)
			{
				MessageBox.Show("Error occurred during database call.");
			}
			else
			{
				var result = t.Result;
				ctrlGrdBar1.mGridChooseFielder.Visible = false;
				ctrlGrdBar1.mGridSaveLayouts.Visible = false;
				ctrlGrdBar1.removeLayoutToolStripMenuItem.Visible = false;
				AllowMultipleOrPartialInvoicingForADO = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("AllowMultipleOrPartialInvoicingForADO"));
				ComboBind(result.ComboData);
				PendingDataGridBind();
				FromDate.Focus();
			}
			FormInitialized = true;
			FromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			Todate.Value = DateTime.Now;
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private DataTable ComboDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		try
		{
			ReportsParameters parameters = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				AppId = UserAccount.AppId,
				UserId = UserAccount.ID,
				DocumentTypeIds = "95,171",
				Activity = "Supplier"
			};
			if (parameters.UserId == 0 || parameters.AppId == 0)
			{
				parameters.AppId = UserAccount.AppId;
				parameters.UserId = UserAccount.ID;
			}
			return InvSaleInvoice.AllComboBindAgainstSaleInvoice(parameters);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ComboBind(DataTable dt)
	{
		InfragisticsHelper.BindAndRetainSelection(CmbCustomer, dt, "Id", "ReferenceName", "Customer", AllColumns: false, Conversion.ToInt(CmbCustomer.Value), insertDefaultRow: false, null, null, ActivateRow: false);
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		try
		{
			FromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			Todate.Value = DateTime.Now;
			grd.DataSource = null;
			FromDate.Focus();
			AllowMultipleOrPartialInvoicingForADO = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("AllowMultipleOrPartialInvoicingForADO"));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			ComboBind(ComboDbCall());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnShow_Click(object sender, EventArgs e)
	{
		try
		{
			PendingDataDbCall();
			PendingDataGridBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void PendingDataDbCall()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		try
		{
			dtRecordsFromDb.Clear();
			ReportsParameters gdn = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				FromDate = Conversion.ToDateTime((object)FromDate.Value),
				ToDate = Conversion.ToDateTime((object)Todate.Value),
				SupplierCustomerId = Conversion.ToInt(CmbCustomer.Value)
			};
			dtRecordsFromDb = InvSaleInvoice.PendingReservedSaleInvoiceForAdvanceDeliveryOrder(gdn);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void PendingDataGridBind()
	{
		try
		{
			if (dtRecordsFromDb.Rows.Count > 0)
			{
				if (dtGrid.Columns.Count == 0)
				{
					dtGrid.Columns.Add("Id", typeof(int));
					dtGrid.Columns.Add("DetailId", typeof(int));
					dtGrid.Columns.Add("DocumentTypeId", typeof(int));
					dtGrid.Columns.Add("DocType", typeof(string));
					dtGrid.Columns.Add("DocDate", typeof(DateTime));
					dtGrid.Columns.Add("DocNo", typeof(int));
					dtGrid.Columns.Add("ManualBillNo", typeof(string));
					dtGrid.Columns.Add("SupplierCustomerId", typeof(int));
					dtGrid.Columns.Add("CustomerName", typeof(string));
					dtGrid.Columns.Add("BillAmount", typeof(decimal));
					dtGrid.Columns.Add("WareHouseName", typeof(string));
					dtGrid.Columns.Add("ItemId", typeof(int));
					dtGrid.Columns.Add("ItemCode", typeof(string));
					dtGrid.Columns.Add("ItemName", typeof(string));
					dtGrid.Columns.Add("CropYear", typeof(string));
					dtGrid.Columns.Add("JobLot", typeof(string));
					dtGrid.Columns.Add("PackingType", typeof(string));
					dtGrid.Columns.Add("PackUom", typeof(string));
					dtGrid.Columns.Add("InvoiceQty", typeof(string));
					dtGrid.Columns.Add("GdnQty", typeof(string));
					dtGrid.Columns.Add("OutstandingDoQty", typeof(string));
					dtGrid.Columns.Add("AvailableQtyForDo", typeof(string));
					dtGrid.Columns.Add("InvoiceWeight", typeof(string));
					dtGrid.Columns.Add("GdnWeight", typeof(string));
					dtGrid.Columns.Add("OutstandingDoWeight", typeof(string));
					dtGrid.Columns.Add("AvailableWeightForDo", typeof(string));
					dtGrid.Columns.Add("NetStockWeight", typeof(decimal));
					dtGrid.Columns.Add("ItemRate", typeof(decimal));
					dtGrid.Columns.Add("RateUom", typeof(string));
					dtGrid.Columns.Add("ItemAmount", typeof(decimal));
					dtGrid.Columns.Add("RemarksHeader", typeof(string));
				}
				dtGrid.Rows.Clear();
				foreach (DataRow row in dtRecordsFromDb.Rows)
				{
					dtGrid.Rows.Add(row["Id"], row["DetailId"], row["DocumentTypeId"], row["DocumentType"], row["DocDate"], row["DocNo"], row["ManualBillNo"], row["SupplierCustomerId"], row["CustomerName"], row["BillAmount"], row["WareHouseName"], row["ItemId"], row["ItemCode"], row["ItemName"], row["CropYear"], row["JobLotDescription"], row["PackTypeDesc"], row["PackUom"], row["InvoiceQty"], row["GdnQty"], row["OutStandingDoQty"], row["AvailableForDoDoQty"], row["InvoiceWeight"], row["GdnWeight"], row["OutStandingDoWeight"], row["AvailableForDoDoWeight"], row["NetStockWeight"], row["ItemRate"], row["RateUom"], row["ItemAmount"], row["RemarksHeader"]);
				}
				grd.DataSource = dtGrid;
				grd.RetrieveStructure();
				grdSettingsForGrnPending();
			}
			else
			{
				grd.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdSettingsForGrnPending()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "DetailId", "DocumentTypeId", "SupplierCustomerId", "ItemId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 3, 2, hiddenColumns);
			grd.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["DocType"].Width = Constants.InventoryConstants.DocumentTypeCode;
			grd.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grd.RootTable.Columns["ManualBillNo"].Width = Constants.InventoryConstants.DocNoConstant - 8;
			grd.RootTable.Columns["CustomerName"].Width = Constants.InventoryConstants.SupplierName;
			grd.RootTable.Columns["BillAmount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grd.RootTable.Columns["RemarksHeader"].Width = Constants.InventoryConstants.RemarksConstants;
			grd.RootTable.Columns["WareHouseName"].Width = Constants.InventoryConstants.WareHouseName;
			grd.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode - 20;
			grd.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grd.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			grd.RootTable.Columns["JobLot"].Width = Constants.InventoryConstants.JobLotConstant;
			grd.RootTable.Columns["PackingType"].Width = Constants.InventoryConstants.PackingType - 10;
			grd.RootTable.Columns["InvoiceQty"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["GdnQty"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["OutstandingDoQty"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["AvailableQtyForDo"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["InvoiceWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["GdnWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["OutstandingDoWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["AvailableWeightForDo"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["NetStockWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["ItemRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["RateUom"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grd.RootTable.Columns["GdnQty"].Visible = AllowMultipleOrPartialInvoicingForADO;
			grd.RootTable.Columns["OutstandingDoQty"].Visible = AllowMultipleOrPartialInvoicingForADO;
			grd.RootTable.Columns["AvailableQtyForDo"].Visible = AllowMultipleOrPartialInvoicingForADO;
			grd.RootTable.Columns["GdnWeight"].Visible = AllowMultipleOrPartialInvoicingForADO;
			grd.RootTable.Columns["OutstandingDoWeight"].Visible = AllowMultipleOrPartialInvoicingForADO;
			grd.RootTable.Columns["AvailableWeightForDo"].Visible = AllowMultipleOrPartialInvoicingForADO;
			GridEX_Helper.AddSelectButton(grd, "Select", 30, 0);
			grd.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				_ = grd.CurrentColumn;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grd, dtGrid);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnLoad(object sender, EventArgs e)
	{
		try
		{
			GridEXRow[] checkedRows = grd.GetCheckedRows();
			if (!checkedRows.Any())
			{
				throw new Exception("Check the row first");
			}
			int firstId = 0;
			List<int> idList = new List<int>();
			GridEXRow[] array = checkedRows;
			foreach (GridEXRow row in array)
			{
				int comparisonId = (AllowMultipleOrPartialInvoicingForADO ? Conversion.ToInt(row.Cells["SupplierCustomerId"].Value) : Conversion.ToInt(row.Cells["Id"].Value));
				if (comparisonId != 0)
				{
					if (firstId == 0)
					{
						firstId = comparisonId;
					}
					else if (firstId != comparisonId)
					{
						dtLoader.Rows.Clear();
						MessageBox.Show(AllowMultipleOrPartialInvoicingForADO ? "Sorry! You can only select rows of the same customer." : "Sorry! You can only select rows of the same invoice.");
						return;
					}
					int currentId = (AllowMultipleOrPartialInvoicingForADO ? Conversion.ToInt(row.Cells["DetailId"].Value) : Conversion.ToInt(row.Cells["Id"].Value));
					idList.Add(currentId);
				}
			}
			if (idList.Any())
			{
				EnumerableRowCollection<DataRow> rows = from r in dtRecordsFromDb.AsEnumerable()
					where (!AllowMultipleOrPartialInvoicingForADO) ? idList.Contains(r.Field<int>("Id")) : idList.Contains(r.Field<int>("DetailId"))
					select r;
				dtLoader = (rows.Any() ? rows.CopyToDataTable() : dtRecordsFromDb.Clone());
			}
			Hide();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

	private void LoadSaleInvoiceForADO_KeyDown(object sender, KeyEventArgs e)
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
				BtnShow_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.L)
			{
				BtnLoad(null, null);
			}
			else if ((!e.Control || e.KeyCode != Keys.N) && (!e.Control || e.KeyCode != Keys.R))
			{
				if (e.Control && e.KeyCode == Keys.F5)
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
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grd.CurrentColumn != null && e.Control)
			{
				_ = e.KeyCode;
				_ = 32;
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
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Expected O, but got Unknown
		//IL_06dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e7: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.PartyProcessing.LoadSaleInvoiceForADO));
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
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnShortcutKeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.label23 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grd = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.CmbCustomer = new UltraCombo();
		this.Todate = new System.Windows.Forms.DateTimePicker();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.btnLoadOnInvoice = new System.Windows.Forms.Button();
		this.BtnShow = new System.Windows.Forms.Button();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel1.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomer).BeginInit();
		this.panel3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Controls.Add(this.label23);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1037, 51);
		this.panel1.TabIndex = 0;
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnNew, this.btnRefresh, this.toolStripSeparator2, this.btnShortcutKeys, this.toolStripSeparator1 });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1037, 25);
		this.toolStrip1.TabIndex = 28;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = (System.Drawing.Image)resources.GetObject("btnNew.Image");
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 22);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = (System.Drawing.Image)resources.GetObject("btnRefresh.Image");
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 22);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
		this.btnShortcutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortcutKeys.Image = (System.Drawing.Image)resources.GetObject("btnShortcutKeys.Image");
		this.btnShortcutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcutKeys.Name = "btnShortcutKeys";
		this.btnShortcutKeys.Size = new System.Drawing.Size(122, 22);
		this.btnShortcutKeys.Text = "ShortCut Keys";
		this.btnShortcutKeys.Click += new System.EventHandler(btnShortcutKeys_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(1, 28);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(239, 19);
		this.label23.TabIndex = 24;
		this.label23.Text = "Pending Invoices For Return";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(995, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 23);
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
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 116);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1037, 560);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomer);
		this.panel2.Controls.Add(this.Todate);
		this.panel2.Controls.Add(this.FromDate);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Controls.Add(this.label3);
		this.panel2.Controls.Add(this.btnLoadOnInvoice);
		this.panel2.Controls.Add(this.BtnShow);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 51);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1037, 40);
		this.panel2.TabIndex = 0;
		this.CmbCustomer.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Location = new System.Drawing.Point(461, 5);
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Name = "CmbCustomer";
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Size = new System.Drawing.Size(249, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomer).TabIndex = 2;
		this.Todate.CustomFormat = "dd-MMM-yy";
		this.Todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.Todate.Location = new System.Drawing.Point(263, 8);
		this.Todate.Name = "Todate";
		this.Todate.Size = new System.Drawing.Size(135, 23);
		this.Todate.TabIndex = 1;
		this.FromDate.CustomFormat = "dd-MMM-yy";
		this.FromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(73, 8);
		this.FromDate.Name = "FromDate";
		this.FromDate.Size = new System.Drawing.Size(135, 23);
		this.FromDate.TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(404, 12);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(61, 15);
		this.label1.TabIndex = 15;
		this.label1.Text = "Customer ";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(209, 12);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(48, 15);
		this.label2.TabIndex = 15;
		this.label2.Text = "To Date";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(5, 12);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(63, 15);
		this.label3.TabIndex = 16;
		this.label3.Text = "From Date";
		this.btnLoadOnInvoice.BackColor = System.Drawing.Color.Teal;
		this.btnLoadOnInvoice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnLoadOnInvoice.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadOnInvoice.ForeColor = System.Drawing.Color.White;
		this.btnLoadOnInvoice.Location = new System.Drawing.Point(776, 7);
		this.btnLoadOnInvoice.Name = "btnLoadOnInvoice";
		this.btnLoadOnInvoice.Size = new System.Drawing.Size(58, 25);
		this.btnLoadOnInvoice.TabIndex = 4;
		this.btnLoadOnInvoice.Text = "Load";
		this.btnLoadOnInvoice.UseVisualStyleBackColor = false;
		this.btnLoadOnInvoice.Click += new System.EventHandler(BtnLoad);
		this.BtnShow.BackColor = System.Drawing.Color.Teal;
		this.BtnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShow.ForeColor = System.Drawing.Color.White;
		this.BtnShow.Location = new System.Drawing.Point(712, 7);
		this.BtnShow.Name = "BtnShow";
		this.BtnShow.Size = new System.Drawing.Size(64, 25);
		this.BtnShow.TabIndex = 3;
		this.BtnShow.Text = "Search";
		this.BtnShow.UseVisualStyleBackColor = false;
		this.BtnShow.Click += new System.EventHandler(BtnShow_Click);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 91);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1037, 25);
		this.panel3.TabIndex = 31;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1037, 676);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.Name = "LoadSaleInvoiceForADO";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(LoadSaleInvoiceForADO_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomer).EndInit();
		this.panel3.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
