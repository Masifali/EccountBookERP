using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Lookups;
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
namespace Architecture.WinApp.Service.Definition
{

public class frmServicesItemCategory : Form
{
	public UserAccount UserAccount = new UserAccount();

	private DataTable dtHistoryGrid = new DataTable();

	private int RecId;

	private string ScreenName = "frmServicesItemCategory";

	public string ParentCategoryIds = "9";

	public string ClassGroupIds = "7";

	private bool FormInitialized;

	private bool MultiLanguageFeature;

	private IContainer components;

	private Label lblCategoryDescription;

	private Panel panel1;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private Panel panel2;

	private Panel panel3;

	private CheckBox chkstatus;

	private TextBox txtItemCategorySerialTo;

	private TextBox txtItemCategorySerialFrom;

	private TextBox txtItemCategoryDescription;

	private TextBox txtItemCategoryCode;

	private Label lblWipCapital;

	private Label label38;

	private Label label37;

	private Label label36;

	private Panel panel4;

	internal GridEX grdfrm;

	private CtrlGrdBar ctrlGrdBar1;

	internal UltraCombo cmbItemCategoryInventoryAccount;

	private Panel panel5;

	private Label label1;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnMultiLingo;

	private Label label2;

	private ToolStripButton btnShortCutKey;

	public frmServicesItemCategory(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
		InitializeComponentCustom();
		InitializeComponentMethod();
	}

	private void InitializeComponentCustom()
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(900, 400);
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
			ScreenName = base.Name;
			MultiLanguageFeature = CommonServices.GetERPFeatureById(7);
			return new
			{
				HistoryData = HistoryDbCall()
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
				btnMultiLingo.Visible = MultiLanguageFeature;
				AccounttBind(DatatableHelper.GetAccountsFromGlobalByTypeIds(new int[2] { 20, 21 }));
				HistoryGridBind(result.HistoryData);
				FormInitialized = true;
				txtItemCategoryCode.Focus();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void AccounttBind(DataTable dtAccount)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(cmbItemCategoryInventoryAccount, dtAccount, "Id", "AccountTitle", "Account Title", AllColumns: true, Conversion.ToInt(cmbItemCategoryInventoryAccount.Value), insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Insert()
	{
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Expected O, but got Unknown
		try
		{
			if (FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
			{
				(txtItemCategoryCode, "Code", FormHelper.ValidType.String),
				(txtItemCategoryDescription, "Description", FormHelper.ValidType.String),
				(txtItemCategorySerialFrom, "Serial From", FormHelper.ValidType.String),
				(txtItemCategorySerialTo, "Serial To", FormHelper.ValidType.String),
				((Control)(object)cmbItemCategoryInventoryAccount, "Expense Account", FormHelper.ValidType.INT)
			}) && FormHelper.ConfirmAction((RecId == 0) ? "Are you sure to Save" : "Are you sure to Update"))
			{
				ItemCategory.Save(new ItemCategory
				{
					Id = RecId,
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					EntryDate = DateTime.Now,
					ModifyDate = DateTime.Now,
					EntryUser = UserAccount.ID,
					ModifyUser = UserAccount.ID,
					CategoryCode = Conversion.ToString((object)txtItemCategoryCode.Text),
					CategoryDescription = Conversion.ToString((object)txtItemCategoryDescription.Text),
					SerialFrom = Conversion.ToInt((object)txtItemCategorySerialFrom.Text),
					SerialTo = Conversion.ToInt((object)txtItemCategorySerialTo.Text),
					RevenueAccountId = Conversion.ToInt(cmbItemCategoryInventoryAccount.Value),
					CGSAccountId = Conversion.ToInt(cmbItemCategoryInventoryAccount.Value),
					InventoryAccountId = Conversion.ToInt(cmbItemCategoryInventoryAccount.Value),
					CategoryStatus = Conversion.ToBool((object)chkstatus.Checked),
					ItemClassGroupId = 7,
					InventoryParentCategoriesId = 9,
					CategoryAttributeList = new List<CategoryAttribute>()
				});
				if (RecId > 0)
				{
					MessageBox.Show("Record Update Successfully.");
				}
				else
				{
					MessageBox.Show("Record Save Successfully.");
				}
				Reset();
			}
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

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("Record not update because RecId not found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadById(int Id)
	{
		try
		{
			RecId = Id;
			ItemCategory cat = ItemCategory.GetByID(RecId);
			if (cat == null)
			{
				MessageBox.Show("Record Not Found");
				return;
			}
			btnsave.Visible = false;
			btnupdate.Visible = true;
			txtItemCategoryCode.Text = Conversion.ToString((object)cat.CategoryCode);
			txtItemCategoryDescription.Text = Conversion.ToString((object)cat.CategoryDescription);
			txtItemCategorySerialFrom.Text = Conversion.ToString((object)cat.SerialFrom);
			txtItemCategorySerialTo.Text = Conversion.ToString((object)cat.SerialTo);
			cmbItemCategoryInventoryAccount.Value = Conversion.ToInt((object)cat.InventoryAccountId);
			chkstatus.Checked = Conversion.ToBool((object)cat.CategoryStatus);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable HistoryDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			return ItemCategory.FormHistory(new ItemCategory
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				CategoryCode = ParentCategoryIds
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void HistoryGridBind(DataTable dt)
	{
		try
		{
			if (dt.Rows.Count > 0)
			{
				if (dtHistoryGrid.Columns.Count == 0)
				{
					dtHistoryGrid.Columns.Add("Id", typeof(int));
					dtHistoryGrid.Columns.Add("CategoryCode", typeof(string));
					dtHistoryGrid.Columns.Add("CategoryDescription", typeof(string));
					dtHistoryGrid.Columns.Add("SerialFrom", typeof(int));
					dtHistoryGrid.Columns.Add("SerialTo", typeof(int));
					dtHistoryGrid.Columns.Add("ExpenseAccountId", typeof(int));
					dtHistoryGrid.Columns.Add("ExpenseAccountTitle", typeof(string));
					dtHistoryGrid.Columns.Add("CategoryStatus", typeof(string));
				}
				dtHistoryGrid.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtHistoryGrid.Rows.Add(Conversion.ToInt(dt.Rows[i]["Id"]), Conversion.ToString(dt.Rows[i]["categoryCode"]), Conversion.ToString(dt.Rows[i]["CategoryDescription"]), Conversion.ToInt(dt.Rows[i]["SerialFrom"]), Conversion.ToInt(dt.Rows[i]["SerialTo"]), Conversion.ToInt(dt.Rows[i]["InventoryAccountId"]), Conversion.ToString(dt.Rows[i]["InventoryAccountTitle"]), Conversion.ToString(dt.Rows[i]["CategoryStatus"]));
				}
				grdfrm.DataSource = dtHistoryGrid;
				grdfrm.RetrieveStructure();
				gridsetting();
			}
			else
			{
				grdfrm.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridsetting()
	{
		try
		{
			grdfrm.RootTable.Columns["Id"].Visible = false;
			grdfrm.RootTable.Columns["ExpenseAccountId"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grdfrm, 2, 3);
			grdfrm.RootTable.Columns["CategoryCode"].Width = 100;
			grdfrm.RootTable.Columns["CategoryDescription"].Width = 150;
			grdfrm.RootTable.Columns["SerialFrom"].Width = 100;
			grdfrm.RootTable.Columns["SerialTo"].Width = 100;
			grdfrm.RootTable.Columns["ExpenseAccountTitle"].Width = 150;
			grdfrm.RootTable.Columns["CategoryStatus"].Width = 100;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdfrm.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				ReadById(CurrId);
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
			RecId = 0;
			txtItemCategoryCode.Clear();
			txtItemCategoryDescription.Clear();
			txtItemCategorySerialFrom.Clear();
			txtItemCategorySerialTo.Clear();
			((Control)(object)cmbItemCategoryInventoryAccount).Text = string.Empty;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			HistoryGridBind(HistoryDbCall());
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
			MessageBox.Show(ex.ToString());
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "AccountsWithCustomGroupId" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			AccounttBind(DatatableHelper.GetAccountsFromGlobalByTypeIds(new int[2] { 20, 21 }));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmServicesItemCategory_FormClosing(object sender, FormClosingEventArgs e)
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, grdfrm, dtHistoryGrid);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void txtItemCategorySerialFrom_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtItemCategorySerialTo_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void btnMultiLingo_Click(object sender, EventArgs e)
	{
		try
		{
			new ItemCategoryMultiLanguage(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmServicesItemCategory_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Return)
		{
			SendKeys.Send("{TAB}");
		}
		else
		{
			if (!e.Control)
			{
				return;
			}
			switch (e.KeyCode)
			{
			case Keys.N:
				btnnew_Click(null, null);
				break;
			case Keys.R:
				btnRefresh_Click(null, null);
				break;
			case Keys.S:
				if (btnsave.Visible && btnsave.Enabled)
				{
					btnsave_Click(null, null);
				}
				break;
			case Keys.U:
				if (btnupdate.Visible && btnupdate.Enabled)
				{
					btnUpdate_Click(null, null);
				}
				break;
			case Keys.Up:
				txtItemCategoryCode.Focus();
				break;
			case Keys.Down:
				((Control)(object)grdfrm).Focus();
				break;
			case Keys.E:
				Close();
				break;
			}
			if (e.Alt)
			{
				MakeShortCutKeys();
			}
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
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Category Code");
			dt.Rows.Add("Ctrl+ArrowDown", "When In History Tab For Focus On Grid history");
			dt.Rows.Add("Ctrl+Enter", "When Focus On Any Grid For Update Record");
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

	private void grdfrm_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyCode == Keys.Return && !e.Control)
			{
				SendKeys.Send("{TAB}");
				return;
			}
			GridEXRow currentRow = grdfrm.CurrentRow;
			if (currentRow != null && (int)currentRow.RowType == 0 && e.Control && e.KeyCode == Keys.Return)
			{
				grdfrm_DoubleClick(null, null);
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
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Service.Definition.frmServicesItemCategory));
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		this.lblCategoryDescription = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnMultiLingo = new System.Windows.Forms.ToolStripButton();
		this.grdfrm = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.txtItemCategoryCode = new System.Windows.Forms.TextBox();
		this.chkstatus = new System.Windows.Forms.CheckBox();
		this.cmbItemCategoryInventoryAccount = new UltraCombo();
		this.label38 = new System.Windows.Forms.Label();
		this.lblWipCapital = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtItemCategorySerialTo = new System.Windows.Forms.TextBox();
		this.txtItemCategorySerialFrom = new System.Windows.Forms.TextBox();
		this.label37 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.txtItemCategoryDescription = new System.Windows.Forms.TextBox();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label1 = new System.Windows.Forms.Label();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel2.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCategoryInventoryAccount).BeginInit();
		this.panel4.SuspendLayout();
		this.panel5.SuspendLayout();
		base.SuspendLayout();
		this.lblCategoryDescription.AutoSize = true;
		this.lblCategoryDescription.BackColor = System.Drawing.Color.Teal;
		this.lblCategoryDescription.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCategoryDescription.ForeColor = System.Drawing.Color.White;
		this.lblCategoryDescription.Location = new System.Drawing.Point(5, 2);
		this.lblCategoryDescription.Name = "lblCategoryDescription";
		this.lblCategoryDescription.Size = new System.Drawing.Size(127, 19);
		this.lblCategoryDescription.TabIndex = 5;
		this.lblCategoryDescription.Text = "Item Category";
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(964, 29);
		this.panel1.TabIndex = 66;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.btnnew, this.btnRefresh, this.btnupdate, this.btnsave, this.btnShortCutKey, this.btnMultiLingo });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(964, 27);
		this.toolStrip1.TabIndex = 67;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnMultiLingo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnMultiLingo.Image = (System.Drawing.Image)resources.GetObject("btnMultiLingo.Image");
		this.btnMultiLingo.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnMultiLingo.Name = "btnMultiLingo";
		this.btnMultiLingo.Size = new System.Drawing.Size(101, 24);
		this.btnMultiLingo.Text = "Multi Lingo";
		this.btnMultiLingo.Click += new System.EventHandler(btnMultiLingo_Click);
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.grdfrm.GroupTotals = (GroupTotals)2;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 23);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(964, 429);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 0;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdfrm).DoubleClick += new System.EventHandler(grdfrm_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdfrm).KeyDown += new System.Windows.Forms.KeyEventHandler(grdfrm_KeyDown);
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.lblCategoryDescription);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 29);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(964, 25);
		this.panel2.TabIndex = 67;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.txtItemCategoryCode);
		this.panel3.Controls.Add(this.chkstatus);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemCategoryInventoryAccount);
		this.panel3.Controls.Add(this.label38);
		this.panel3.Controls.Add(this.lblWipCapital);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Controls.Add(this.txtItemCategorySerialTo);
		this.panel3.Controls.Add(this.txtItemCategorySerialFrom);
		this.panel3.Controls.Add(this.label37);
		this.panel3.Controls.Add(this.label36);
		this.panel3.Controls.Add(this.txtItemCategoryDescription);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 54);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(964, 60);
		this.panel3.TabIndex = 0;
		this.txtItemCategoryCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemCategoryCode.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemCategoryCode.Location = new System.Drawing.Point(74, 5);
		this.txtItemCategoryCode.Name = "txtItemCategoryCode";
		this.txtItemCategoryCode.Size = new System.Drawing.Size(220, 23);
		this.txtItemCategoryCode.TabIndex = 0;
		this.chkstatus.AutoSize = true;
		this.chkstatus.Checked = true;
		this.chkstatus.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkstatus.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkstatus.Location = new System.Drawing.Point(747, 33);
		this.chkstatus.Name = "chkstatus";
		this.chkstatus.Size = new System.Drawing.Size(71, 19);
		this.chkstatus.TabIndex = 5;
		this.chkstatus.Text = "Is Active";
		this.chkstatus.UseVisualStyleBackColor = true;
		this.cmbItemCategoryInventoryAccount.AllowNull = (DefaultableBoolean)1;
		this.cmbItemCategoryInventoryAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemCategoryInventoryAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemCategoryInventoryAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance27;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance32).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemCategoryInventoryAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemCategoryInventoryAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemCategoryInventoryAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemCategoryInventoryAccount).Location = new System.Drawing.Point(589, 3);
		((System.Windows.Forms.Control)(object)this.cmbItemCategoryInventoryAccount).Name = "cmbItemCategoryInventoryAccount";
		((System.Windows.Forms.Control)(object)this.cmbItemCategoryInventoryAccount).Size = new System.Drawing.Size(229, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemCategoryInventoryAccount).TabIndex = 4;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(297, 9);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(67, 15);
		this.label38.TabIndex = 2;
		this.label38.Text = "Serial From";
		this.lblWipCapital.AutoSize = true;
		this.lblWipCapital.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblWipCapital.Location = new System.Drawing.Point(485, 9);
		this.lblWipCapital.Name = "lblWipCapital";
		this.lblWipCapital.Size = new System.Drawing.Size(98, 15);
		this.lblWipCapital.TabIndex = 5;
		this.lblWipCapital.Text = "Expense Account";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(297, 35);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(52, 15);
		this.label2.TabIndex = 2;
		this.label2.Text = "Serial To";
		this.txtItemCategorySerialTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemCategorySerialTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemCategorySerialTo.Location = new System.Drawing.Point(367, 31);
		this.txtItemCategorySerialTo.Name = "txtItemCategorySerialTo";
		this.txtItemCategorySerialTo.Size = new System.Drawing.Size(115, 23);
		this.txtItemCategorySerialTo.TabIndex = 3;
		this.txtItemCategorySerialTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtItemCategorySerialTo_KeyPress);
		this.txtItemCategorySerialFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemCategorySerialFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemCategorySerialFrom.Location = new System.Drawing.Point(367, 5);
		this.txtItemCategorySerialFrom.Name = "txtItemCategorySerialFrom";
		this.txtItemCategorySerialFrom.Size = new System.Drawing.Size(115, 23);
		this.txtItemCategorySerialFrom.TabIndex = 2;
		this.txtItemCategorySerialFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtItemCategorySerialFrom_KeyPress);
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(5, 35);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(68, 15);
		this.label37.TabIndex = 1;
		this.label37.Text = "Description";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(5, 9);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(34, 15);
		this.label36.TabIndex = 0;
		this.label36.Text = "Code";
		this.txtItemCategoryDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemCategoryDescription.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemCategoryDescription.Location = new System.Drawing.Point(74, 31);
		this.txtItemCategoryDescription.Name = "txtItemCategoryDescription";
		this.txtItemCategoryDescription.Size = new System.Drawing.Size(220, 23);
		this.txtItemCategoryDescription.TabIndex = 1;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 114);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(964, 452);
		this.panel4.TabIndex = 69;
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(964, 23);
		this.panel5.TabIndex = 68;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(919, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(40, 19);
		this.ctrlGrdBar1.TabIndex = 68;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.Teal;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(4, 2);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(190, 19);
		this.label1.TabIndex = 5;
		this.label1.Text = "Item Category History";
		this.btnShortCutKey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(964, 566);
		base.Controls.Add(this.panel4);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmServicesItemCategory";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmServicesItemCategory_FormClosing);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmServicesItemCategory_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCategoryInventoryAccount).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
