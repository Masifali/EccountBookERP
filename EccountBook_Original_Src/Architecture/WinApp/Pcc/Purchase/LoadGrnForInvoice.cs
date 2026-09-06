using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.pcc;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.pcc.purchase;

public class LoadGrnForInvoice : Form
{
	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	public DataTable dtLoader = new DataTable();

	public DataTable dtGrid = new DataTable();

	public DataTable dtBindCombo = new DataTable();

	private IContainer components;

	private Panel panel1;

	private Label label23;

	private Panel panel2;

	private Button btngrnlod;

	private DateTimePicker Todate;

	private DateTimePicker FromDate;

	private Label label1;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel3;

	internal GridEX grdMain;

	private Button btnLoadOnInvoice;

	private Label label2;

	internal UltraCombo CmbSupplier;

	private Button btnReset;

	private Label label4;

	private TextBox txtOrderNoFrom;

	private TextBox txtOrderNoTo;

	private GroupBox groupBox1;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnShortcutKeys;

	private ToolStripSeparator toolStripSeparator1;

	private Label label3;

	internal GridEX grdDetail;

	private Panel panel4;

	private Label label5;

	public LoadGrnForInvoice(UserAccount user)
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
			base.AutoScrollMinSize = new Size(980, 700);
			FromDate.Focus();
			FromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			ComboFill();
			PendingDataForLoad();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboFill()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		try
		{
			DataTable dtSupplieerCustomer = new DataTable();
			dtSupplieerCustomer.Columns.Add("Id");
			dtSupplieerCustomer.Columns.Add("name");
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtBindCombo = InvGrn.GetDataForDropDownFromGrn(obj);
			if (dtBindCombo.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtBindCombo.Rows.Count; i++)
			{
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "Supplier_Customer")
				{
					dtSupplieerCustomer.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["name"]);
				}
			}
			if (dtSupplieerCustomer.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtSupplieerCustomer, CmbSupplier, "Id", "name", "PartyName", false);
				((UltraGridBase)CmbSupplier).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
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
			grdMain.ClearStructure();
			grdDetail.ClearStructure();
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
			ComboFill();
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
			PendingDataForLoad();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PendingDataForLoad()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.FromDate = Convert.ToDateTime(FromDate.Value);
			obj.ToDate = Convert.ToDateTime(Todate.Value);
			obj.DocumentTypeId = 1864;
			obj.FromDocNo = Conversion.ToInt((object)txtOrderNoFrom.Text.Trim());
			obj.ToDocNo = Conversion.ToInt((object)txtOrderNoTo.Text.Trim());
			obj.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			dtGrid = InvGrn.GetPendingGrnForInvoice(obj);
			if (dtGrid.Rows.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("GrnTypeId", typeof(int));
			dt.Columns.Add("GrnType", typeof(string));
			dt.Columns.Add("GrnId", typeof(int));
			dt.Columns.Add("GrnDetailId", typeof(int));
			dt.Columns.Add("GrnNo", typeof(string));
			dt.Columns.Add("GrnDate", typeof(DateTime));
			dt.Columns.Add("OrderId", typeof(int));
			dt.Columns.Add("OrderDetailId", typeof(int));
			dt.Columns.Add("OrderNo", typeof(string));
			dt.Columns.Add("SupplierCustomerId", typeof(int));
			dt.Columns.Add("SupplierName", typeof(string));
			dt.Columns.Add("ReferenenceNo", typeof(string));
			dt.Columns.Add("DeliveryTerm", typeof(string));
			dt.Columns.Add("TotalQty", typeof(decimal));
			dt.Columns.Add("TotalWeight", typeof(decimal));
			dt.Columns.Add("TransporterId", typeof(int));
			dt.Columns.Add("Transporter", typeof(string));
			dt.Columns.Add("CarriageAmount", typeof(decimal));
			dt.Columns.Add("VehicleType", typeof(string));
			dt.Columns.Add("VehicleNo", typeof(string));
			dt.Columns.Add("BiltyNo", typeof(string));
			dt.Columns.Add("DriverName", typeof(string));
			dt.Columns.Add("DriverCnic", typeof(string));
			dt.Columns.Add("DriverCell", typeof(string));
			dt.Columns.Add("EntryDate", typeof(DateTime));
			dt.Columns.Add("EntryUser", typeof(string));
			dt.Columns.Add("RemarksHeader", typeof(string));
			dt.Columns.Add("NoOfAttachments", typeof(int));
			foreach (DataRow row in dtGrid.Rows)
			{
				if (!dt.AsEnumerable().Any((DataRow r) => r.Field<int>("GrnId") == Conversion.ToInt(row["Id"])))
				{
					double TotalQty = Conversion.ToDouble(dtGrid.Compute("Sum(ItemQty)", "Id ='" + Conversion.ToString((object)(row["Id"]?.ToString() + "'"))));
					double TotalWeight = Conversion.ToDouble(dtGrid.Compute("Sum(NetWeight)", "Id ='" + Conversion.ToString((object)(row["Id"]?.ToString() + "'"))));
					dt.Rows.Add(row["GrnTypeId"], row["GrnType"], row["Id"], row["DetailId"], row["DocNo"], Conversion.ToDateTime(row["DocDate"]).ToShortDateString(), row["OrderId"], row["OrderDetailId"], row["OrderNo"], row["SupplierCustomerId"], row["CustomerName"], row["ReferenceNo"], row["DeliveryTerm"], TotalQty, TotalWeight, row["TransporterId"], row["TransporterName"], row["FrieghtAmountHeader"], row["VehicleDescription"], row["VehicleNo"], row["BiltyNo"], row["DriverName"], row["DriverCNICNo"], row["DriverCellNo"], row["EntryDate"], row["EntryUserName"], row["RemarksHeader"], row["NoOfAttachments"]);
				}
			}
			grdMain.DataSource = dt;
			grdMain.RetrieveStructure();
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
			grdMain.RootTable.Columns["GrnTypeId"].Visible = false;
			grdMain.RootTable.Columns["GrnId"].Visible = false;
			grdMain.RootTable.Columns["GrnDetailId"].Visible = false;
			grdMain.RootTable.Columns["OrderId"].Visible = false;
			grdMain.RootTable.Columns["OrderDetailId"].Visible = false;
			grdMain.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grdMain.RootTable.Columns["TransporterId"].Visible = false;
			grdMain.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdMain.RootTable.Columns["GrnDate"].FilterEditType = (FilterEditType)6;
			grdMain.RootTable.Columns["TotalQty"].AggregateFunction = (AggregateFunction)2;
			grdMain.RootTable.Columns["TotalQty"].TextAlignment = (TextAlignment)3;
			grdMain.RootTable.Columns["TotalQty"].FormatString = "#,##0.##";
			grdMain.RootTable.Columns["TotalQty"].TotalFormatString = "#,##0.###";
			grdMain.RootTable.Columns["TotalWeight"].AggregateFunction = (AggregateFunction)2;
			grdMain.RootTable.Columns["TotalWeight"].TextAlignment = (TextAlignment)3;
			grdMain.RootTable.Columns["TotalWeight"].FormatString = "#,##0.####";
			grdMain.RootTable.Columns["TotalWeight"].TotalFormatString = "#,##0.####";
			grdMain.RootTable.Columns["CarriageAmount"].AggregateFunction = (AggregateFunction)2;
			grdMain.RootTable.Columns["CarriageAmount"].TextAlignment = (TextAlignment)3;
			grdMain.RootTable.Columns["CarriageAmount"].FormatString = "#,##0.####";
			grdMain.RootTable.Columns["CarriageAmount"].TotalFormatString = "#,##0.####";
			CommonServices.GridAutoAdjustmentNew(grdMain);
			grdMain.RootTable.Columns.Add("Select");
			grdMain.RootTable.Columns["Select"].ActAsSelector = true;
			grdMain.RootTable.Columns["Select"].UseHeaderSelector = true;
			grdMain.RootTable.Columns["Select"].Width = 40;
			grdMain.RootTable.Columns["Select"].Position = 0;
			grdMain.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdMain_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			DetailGridBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailGridBind()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdMain.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				if (item.Cells.Count > 0)
				{
					DataTable dtOrder = new DataTable();
					dtOrder.Columns.Add("Id", typeof(int));
					dtOrder.Columns.Add("DetailId", typeof(int));
					dtOrder.Columns.Add("Warehouse", typeof(string));
					dtOrder.Columns.Add("ItemCode", typeof(string));
					dtOrder.Columns.Add("ItemName", typeof(string));
					dtOrder.Columns.Add("ItemAttributeVarient", typeof(string));
					dtOrder.Columns.Add("VarientEquivalent", typeof(decimal));
					dtOrder.Columns.Add("ItemQty", typeof(decimal));
					dtOrder.Columns.Add("ItemWeight", typeof(decimal));
					dtOrder.Columns.Add("NetWeight", typeof(decimal));
					dtOrder.Columns.Add("JobLot", typeof(string));
					dtOrder.Columns.Add("CityName", typeof(string));
					dtOrder.Columns.Add("RemarksDetail", typeof(string));
					dtOrder.Rows.Clear();
					DataTable resultRows = (from row in dtGrid.AsEnumerable()
						where row.Field<int>("Id") == Conversion.ToInt(item.Cells["GrnId"].Value)
						select row).CopyToDataTable();
					for (int i = 0; i < resultRows.Rows.Count; i++)
					{
						dtOrder.Rows.Add(resultRows.Rows[i]["Id"], resultRows.Rows[i]["DetailId"], resultRows.Rows[i]["WareHouseName"], resultRows.Rows[i]["ItemCode"], resultRows.Rows[i]["ItemName"], resultRows.Rows[i]["ItemAttributeVarient"], resultRows.Rows[i]["VarientEquivalent"], resultRows.Rows[i]["ItemQty"], resultRows.Rows[i]["ItemWeight"], resultRows.Rows[i]["NetWeight"], resultRows.Rows[i]["JobLotDescription"], resultRows.Rows[i]["CityName"], resultRows.Rows[i]["RemarksDetail"]);
					}
					grdDetail.DataSource = dtOrder;
					grdDetail.RetrieveStructure();
					GridDetailSetting();
				}
			}
			else
			{
				grdDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetailSetting()
	{
		try
		{
			grdDetail.RootTable.Columns["Id"].Visible = false;
			grdDetail.RootTable.Columns["DetailId"].Visible = false;
			grdDetail.RootTable.Columns["ItemAttributeVarient"].Caption = "Varient";
			grdDetail.RootTable.Columns["VarientEquivalent"].Caption = "VarientUnit";
			grdDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["VarientEquivalent"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["VarientEquivalent"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["ItemWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemWeight"].FormatString = "##,#.##";
			grdDetail.RootTable.Columns["ItemWeight"].TotalFormatString = "##,#.##";
			grdDetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["NetWeight"].FormatString = "##,#.##";
			grdDetail.RootTable.Columns["NetWeight"].TotalFormatString = "##,#.##";
			CommonServices.GridAutoAdjustmentNew(grdDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnLoadOnInvoice_Click_1(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt((object)grdMain.GetCheckedRows().Count().ToString()) > 0)
			{
				dtLoader.Rows.Clear();
				dtLoader = dtGrid.Clone();
				string Ids = "";
				int orderId = 0;
				int GrnTypeId = 0;
				int FreightAcId = 0;
				int PartyId = 0;
				string DeliveryTerm = "";
				GridEXRow[] checkedRows = grdMain.GetCheckedRows();
				foreach (GridEXRow r2 in checkedRows)
				{
					int Id = Conversion.ToInt(r2.Cells["OrderId"].Value);
					int TypeId = ((Conversion.ToInt(r2.Cells["GrnTypeId"].Value) == 0) ? 1 : 2);
					int TransporterId = Conversion.ToInt(r2.Cells["TransporterId"].Value);
					int SupplierId = Conversion.ToInt(r2.Cells["SupplierCustomerId"].Value);
					string DTerm = Conversion.ToString(r2.Cells["DeliveryTerm"].Value);
					if (GrnTypeId == 0)
					{
						GrnTypeId = TypeId;
					}
					if (GrnTypeId != TypeId)
					{
						dtLoader.Rows.Clear();
						MessageBox.Show("You Can Only Select Rows Of Same Grn Type!");
						return;
					}
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
					if (FreightAcId == 0)
					{
						FreightAcId = TransporterId;
					}
					if (TransporterId != FreightAcId)
					{
						dtLoader.Rows.Clear();
						MessageBox.Show("You Can Only Select Rows Of Same Transporter");
						return;
					}
					if (PartyId == 0)
					{
						PartyId = SupplierId;
					}
					if (PartyId != SupplierId)
					{
						dtLoader.Rows.Clear();
						MessageBox.Show("You Can Only Select Rows Of Same Customer");
						return;
					}
					if (DeliveryTerm == null || DeliveryTerm == string.Empty)
					{
						DeliveryTerm = DTerm;
					}
					if (DeliveryTerm != DTerm)
					{
						dtLoader.Rows.Clear();
						MessageBox.Show("You Can Only Select Rows Of Same Delivery term");
						return;
					}
					Ids = Ids + "," + r2.Cells["GrnId"].Value.ToString();
					GrnTypeId = TypeId;
					orderId = Id;
					FreightAcId = TransporterId;
					PartyId = SupplierId;
					DeliveryTerm = DTerm;
				}
				if (Ids != string.Empty)
				{
					string[] idsArray = Ids.Split(',');
					EnumerableRowCollection<DataRow> rows = from r in dtGrid.AsEnumerable()
						where idsArray.Contains(r.Field<int>("Id").ToString())
						select r;
					dtLoader = (rows.Any() ? rows.CopyToDataTable() : dtGrid.Clone());
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdMain).Name);
			if (fs != null)
			{
				grdMain.LoadLayoutFile((Stream)fs);
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
				((Control)(object)grdMain).Focus();
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
			GridEXRow r = grdMain.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["Id"].Value), 1852);
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
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Expected O, but got Unknown
		//IL_0677: Unknown result type (might be due to invalid IL or missing references)
		//IL_0681: Expected O, but got Unknown
		GridEXLayout grdMain_DesignTimeLayout = new GridEXLayout();
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
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnShortcutKeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.label23 = new System.Windows.Forms.Label();
		this.grdMain = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label3 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.btnReset = new System.Windows.Forms.Button();
		this.txtOrderNoTo = new System.Windows.Forms.TextBox();
		this.btnLoadOnInvoice = new System.Windows.Forms.Button();
		this.CmbSupplier = new UltraCombo();
		this.btngrnlod = new System.Windows.Forms.Button();
		this.label2 = new System.Windows.Forms.Label();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.txtOrderNoFrom = new System.Windows.Forms.TextBox();
		this.Todate = new System.Windows.Forms.DateTimePicker();
		this.label4 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdDetail = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdMain).BeginInit();
		this.panel2.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Controls.Add(this.label23);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1034, 50);
		this.panel1.TabIndex = 0;
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNew, this.btnRefresh, this.btnShortcutKeys, this.toolStripSeparator1 });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1034, 25);
		this.toolStrip1.TabIndex = 27;
		this.toolStrip1.Text = "toolStrip1";
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
		this.btnShortcutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortcutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortcutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcutKeys.Name = "btnShortcutKeys";
		this.btnShortcutKeys.Size = new System.Drawing.Size(122, 22);
		this.btnShortcutKeys.Text = "ShortCut Keys";
		this.btnShortcutKeys.Click += new System.EventHandler(btnShortcutKeys_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.Teal;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(4, 28);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(82, 19);
		this.label23.TabIndex = 24;
		this.label23.Text = "Load Grn";
		this.grdMain.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdMain).BackColor = System.Drawing.Color.White;
		grdMain_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdMain.DesignTimeLayout = grdMain_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdMain).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdMain.FilterMode = (FilterMode)2;
		this.grdMain.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdMain).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdMain.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdMain).Location = new System.Drawing.Point(0, 151);
		((System.Windows.Forms.Control)(object)this.grdMain).Name = "grdMain";
		((ScrollableBase)this.grdMain).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdMain).Size = new System.Drawing.Size(1034, 312);
		((System.Windows.Forms.Control)(object)this.grdMain).TabIndex = 0;
		this.grdMain.TotalRow = (InheritableBoolean)1;
		this.grdMain.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdMain).VisualStyle = (VisualStyle)4;
		this.grdMain.SelectionChanged += new System.EventHandler(grdMain_SelectionChanged);
		this.grdMain.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.groupBox1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 50);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1034, 75);
		this.panel2.TabIndex = 0;
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.btngrnlod);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.btnLoadOnInvoice);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.btnReset);
		this.groupBox1.Controls.Add(this.txtOrderNoTo);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.FromDate);
		this.groupBox1.Controls.Add(this.txtOrderNoFrom);
		this.groupBox1.Controls.Add(this.Todate);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(5, 1);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(781, 70);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(127, 20);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(48, 15);
		this.label3.TabIndex = 12;
		this.label3.Text = "To Date";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(9, 20);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 15);
		this.label1.TabIndex = 12;
		this.label1.Text = "From Date";
		this.btnReset.BackColor = System.Drawing.Color.Teal;
		this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnReset.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnReset.ForeColor = System.Drawing.Color.White;
		this.btnReset.Location = new System.Drawing.Point(584, 37);
		this.btnReset.Name = "btnReset";
		this.btnReset.Size = new System.Drawing.Size(61, 26);
		this.btnReset.TabIndex = 11;
		this.btnReset.Text = "Reset";
		this.btnReset.UseVisualStyleBackColor = false;
		this.btnReset.Click += new System.EventHandler(btnReset_Click);
		this.txtOrderNoTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderNoTo.Location = new System.Drawing.Point(314, 39);
		this.txtOrderNoTo.Name = "txtOrderNoTo";
		this.txtOrderNoTo.ShortcutsEnabled = false;
		this.txtOrderNoTo.Size = new System.Drawing.Size(70, 23);
		this.txtOrderNoTo.TabIndex = 3;
		this.txtOrderNoTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOrderNoTo_KeyPress);
		this.btnLoadOnInvoice.BackColor = System.Drawing.Color.Teal;
		this.btnLoadOnInvoice.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnLoadOnInvoice.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadOnInvoice.ForeColor = System.Drawing.Color.White;
		this.btnLoadOnInvoice.Location = new System.Drawing.Point(646, 37);
		this.btnLoadOnInvoice.Name = "btnLoadOnInvoice";
		this.btnLoadOnInvoice.Size = new System.Drawing.Size(61, 26);
		this.btnLoadOnInvoice.TabIndex = 10;
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
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(386, 37);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(196, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 8;
		this.btngrnlod.BackColor = System.Drawing.Color.Teal;
		this.btngrnlod.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btngrnlod.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btngrnlod.ForeColor = System.Drawing.Color.White;
		this.btngrnlod.Location = new System.Drawing.Point(708, 37);
		this.btngrnlod.Name = "btngrnlod";
		this.btngrnlod.Size = new System.Drawing.Size(61, 26);
		this.btngrnlod.TabIndex = 9;
		this.btngrnlod.Text = "Show";
		this.btngrnlod.UseVisualStyleBackColor = false;
		this.btngrnlod.Click += new System.EventHandler(btngrnlod_Click);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(385, 20);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(86, 15);
		this.label2.TabIndex = 24;
		this.label2.Text = "Supplier Name";
		this.FromDate.CustomFormat = "dd-MM-yy";
		this.FromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(9, 39);
		this.FromDate.Name = "FromDate";
		this.FromDate.Size = new System.Drawing.Size(116, 23);
		this.FromDate.TabIndex = 0;
		this.txtOrderNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderNoFrom.Location = new System.Drawing.Point(249, 39);
		this.txtOrderNoFrom.Name = "txtOrderNoFrom";
		this.txtOrderNoFrom.ShortcutsEnabled = false;
		this.txtOrderNoFrom.Size = new System.Drawing.Size(64, 23);
		this.txtOrderNoFrom.TabIndex = 2;
		this.txtOrderNoFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOrderNoFrom_KeyPress);
		this.Todate.CustomFormat = "dd-MM-yy";
		this.Todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.Todate.Location = new System.Drawing.Point(127, 39);
		this.Todate.Name = "Todate";
		this.Todate.Size = new System.Drawing.Size(121, 23);
		this.Todate.TabIndex = 1;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(249, 20);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(54, 15);
		this.label4.TabIndex = 28;
		this.label4.Text = "No From";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 125);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1034, 26);
		this.panel3.TabIndex = 31;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(985, -1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdMain;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 26);
		this.ctrlGrdBar1.TabIndex = 10;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 486);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1034, 188);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 32;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 463);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1034, 23);
		this.panel4.TabIndex = 33;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(314, 20);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(39, 15);
		this.label5.TabIndex = 29;
		this.label5.Text = "No To";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1034, 674);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		base.Controls.Add(this.panel4);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grdMain);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "LoadGrnForInvoice";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(LoadInvoices_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(LoadPurchaseOrder_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdMain).EndInit();
		this.panel2.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		base.ResumeLayout(false);
	}
}
