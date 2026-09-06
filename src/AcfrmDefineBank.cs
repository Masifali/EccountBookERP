using Country = global::Architecture.Model.Country;
using System.Linq;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using SupplierCustomer = global::Architecture.Model.Inventory.SupplierCustomer;
using ChartofAccount = global::Architecture.Model.Accounts.ChartofAccount;
using BankReconciliation = global::Architecture.Model.Accounts.BankReconciliation;

using AccountsBudgetHeader = global::Architecture.Model.Accounts.AccountsBudgetHeader;
namespace Architecture.WinApp
{
using CommonServices = global::Architecture.WinApp.Common.CommonServices;
using Bank = global::Architecture.Model.Bank;
using City = global::Architecture.Model.City;
using CheqBookHeader = global::Architecture.Model.Accounts.CheqBookHeader;
using CheqBookDetail = global::Architecture.Model.Accounts.CheqBookDetail;
using COAAllocation = global::Architecture.Model.Accounts.COAAllocation;
using ChartOfAccountMultiLingo = global::Architecture.Model.Accounts.ChartOfAccountMultiLingo;
using TaxesTypes = global::Architecture.Model.Inventory.TaxesTypes;
using SupplierCustomerShipToAddress = global::Architecture.Model.Inventory.SupplierCustomerShipToAddress;
using AccountsOpeningBalances = global::Architecture.BLL.Accounts.AccountsOpeningBalances;
using ShortCutKeyPopUp = global::Architecture.WinApp.Common.ShortCutKeyPopUp;
using Constants = global::Architecture.WinApp.Common.Constants;
using GridEX_Helper = global::Architecture.WinApp.Common.GridEX_Helper;
using InfragisticsHelper = global::Architecture.WinApp.Common.InfragisticsHelper;
using HierarchicalMode = Janus.Windows.GridEX.HierarchicalMode;
using SelfReferencingAggregationMode = Janus.Windows.GridEX.SelfReferencingAggregationMode;

using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;

using DropDownBind = global::Architecture.WinApp.Common.DropDownBind;
using Rightsobjects = global::Architecture.WinApp.Common.Rightsobjects;

using UserAccount = global::Architecture.Model.UserAccount;

using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using global::Architecture.BLL;
using global::Architecture.Common;
using global::Architecture.Model;
// // using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using grid.User_Conrtols;

public class AcfrmDefineBank : Form
{
	private int RecId;

	public int CountryTypeId;

	public bool RequestedByOtherDocument;

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	public UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Panel panel5;

	private Panel panel4;

	private Panel panel3;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btndelete;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label23;

	internal UltraCombo combcity;

	internal UltraCombo combcountry;

	internal UltraCombo combGlaccount;

	private Label label5;

	private Label label4;

	private Label label12;

	private Label label13;

	private Label label11;

	private Label label8;

	private Label label10;

	private Label label7;

	private Label label2;

	private Label label9;

	private TextBox txtcontact2;

	private TextBox txtemail;

	private TextBox txtotherinfor;

	private TextBox txtemailalternate;

	private TextBox txtcellno;

	private TextBox txtcontact1;

	private TextBox txtbranchcode;

	private TextBox txtbankname;

	private Label label6;

	private Label label1;

	internal GridEX GrdBankDefine;

	private ToolStripButton btnUpdate;

	internal UltraCombo CmbChequeTemplete;

	private Label label14;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnShortCutKeys;

	private Panel panel6;

	private Label label15;

	private GroupBox groupBox1;

	private TextBox txtBankIBANNo;

	private Label label17;

	private TextBox txtBankAccountno;

	private Label label16;

	private Label label19;

	private Label label18;

	private Label label21;

	private Label label20;

	private Label label22;

	public UltraCombo txtishomeland;

	private Label label24;

	private TextBox txtAccountTitle;

	private Label label25;

	private TextBox txtaddress;

	private Label label26;

	public AcfrmDefineBank(UserAccount pUserAccount)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void AcfrmDefineBank_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1000, 500);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			CountryTypeFill();
			ChequeTempleteFill();
			CountryComboFill();
			CityFill();
			BankDefineGridFill();
			CombGlAccountNameFill();
			txtishomeland.Focus();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			if (!RequestedByOtherDocument)
			{
				((UltraGridBase)txtishomeland).Rows[2].Activate();
			}
			else if (CountryTypeId > 0)
			{
				txtishomeland.Value = CountryTypeId;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CountryComboFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = global::Architecture.BLL.Country.GetAll(new global::Architecture.Model.Country
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, combcountry, "Id", "Description", "Country Name", true);
				((UltraGridBase)combcountry).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CombGlAccountNameFill()
	{
		try
		{
			DataTable dt = new DataTable();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, combGlaccount, "Id", "AccountTitle", "Account Title", ZeroIndex: true);
				((UltraGridBase)combGlaccount).DisplayLayout.Bands[0].Columns[1].Width = 350;
			}
			else
			{
				((Control)(object)combGlaccount).Text = string.Empty;
				((UltraGridBase)combGlaccount).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CountryTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			dt.Rows.Add(1, "Foreign Country");
			dt.Rows.Add(2, "Home Country");
			DDL.BindDDLNew(dt, txtishomeland, "Id", "Name", "Country Name", true);
			((UltraGridBase)txtishomeland).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ChequeTempleteFill()
	{
		try
		{
			DataTable dt = new DataTable();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbChequeTemplete, "Id", "type", "Cheque Templete", true);
				((UltraGridBase)CmbChequeTemplete).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CityFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = global::Architecture.BLL.City.GetAll(new City
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, combcity, "Id", "CityName", "City Name", true);
				((UltraGridBase)combcity).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((Control)(object)txtishomeland).Text.Trim() == string.Empty || Conversion.ToInt(txtishomeland.Value) == 0)
		{
			MessageBox.Show("Home Land Name Field Required");
			txtishomeland.Focus();
			return false;
		}
		if (txtbankname.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Bank Name Field Required");
			txtbankname.Focus();
			return false;
		}
		if (txtbranchcode.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Branch Code Field Required");
			txtbranchcode.Focus();
			return false;
		}
		if (txtBankAccountno.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Bank Account No Field Required");
			txtBankAccountno.Focus();
			return false;
		}
		if (txtBankIBANNo.Text.Trim() == string.Empty)
		{
			MessageBox.Show("BankIBANNo Field Required");
			txtBankIBANNo.Focus();
			return false;
		}
		if (Conversion.ToInt(txtishomeland.Value) == 2 && (((UltraGridBase)CmbChequeTemplete).ActiveRow == null || Conversion.ToInt(CmbChequeTemplete.Value) == 0))
		{
			MessageBox.Show("Cheque Template Field Required");
			CmbChequeTemplete.Focus();
			return false;
		}
		if (Conversion.ToInt(txtishomeland.Value) == 2 && (((UltraGridBase)combGlaccount).ActiveRow == null || Conversion.ToInt(combGlaccount.Value) == 0))
		{
			MessageBox.Show("GL Account Field Required");
			combGlaccount.Focus();
			return false;
		}
		return true;
	}

	private void BankFromRefresh()
	{
		try
		{
			RecId = 0;
			((Control)(object)txtishomeland).Text = string.Empty;
			txtbankname.Text = string.Empty;
			txtbranchcode.Text = string.Empty;
			txtBankAccountno.Text = string.Empty;
			txtAccountTitle.Text = string.Empty;
			txtBankIBANNo.Text = string.Empty;
			txtcontact1.Text = string.Empty;
			txtcontact2.Text = string.Empty;
			txtcellno.Text = string.Empty;
			txtemail.Text = string.Empty;
			txtemailalternate.Text = string.Empty;
			combcountry.Value = string.Empty;
			txtotherinfor.Text = string.Empty;
			((Control)(object)combGlaccount).Text = string.Empty;
			txtaddress.Text = string.Empty;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			BankDefineGridFill();
			txtishomeland.Focus();
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
			BankFromRefresh();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click_1(object sender, EventArgs e)
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

	private void AcfrmDefineBank_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			ChequeTempleteFill();
			CountryComboFill();
			CityFill();
			CombGlAccountNameFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			global::Architecture.Model.Bank bnk = new global::Architecture.Model.Bank();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				bnk.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			bnk.IsHomeland = Conversion.ToString((object)((Control)(object)txtishomeland).Text.Trim());
			bnk.BranchName = Conversion.ToString((object)txtbankname.Text.Trim());
			bnk.BranchCode = Conversion.ToString((object)txtbranchcode.Text.Trim());
			bnk.BankAccountNo = Conversion.ToString((object)txtBankAccountno.Text);
			bnk.BankAccountTitle = Conversion.ToString((object)txtAccountTitle.Text);
			bnk.BankIBANNo = Conversion.ToString((object)txtBankIBANNo.Text);
			bnk.Contact1Tel = Conversion.ToString((object)txtcontact1.Text.Trim());
			bnk.Contact2Tel = Conversion.ToString((object)txtcontact2.Text.Trim());
			bnk.Contact3Mobile = Conversion.ToString((object)txtcellno.Text.Trim());
			bnk.emailPrimery = Conversion.ToString((object)txtemail.Text.Trim());
			bnk.emailAlternate = Conversion.ToString((object)txtemailalternate.Text.Trim());
			bnk.CountryId = Conversion.ToInt(combcountry.Value);
			bnk.BranchCity = Conversion.ToInt(combcity.Value);
			bnk.OtherInfo = Conversion.ToString((object)txtotherinfor.Text.Trim());
			bnk.ChequeTemplete = Conversion.ToString((object)((Control)(object)CmbChequeTemplete).Text);
			bnk.ChartOfAccountId = Conversion.ToInt(combGlaccount.Value);
			bnk.BranchAddress = Conversion.ToString((object)txtaddress.Text.Trim());
			bnk.OrganizationId = UserAccount.OrganizationId;
			bnk.CompanyId = UserAccount.CompanyId;
			bnk.EntryUser = UserAccount.ID;
			bnk.ModifyUser = UserAccount.ID;
			global::Architecture.BLL.Bank.Save(bnk);
			if (RecId > 0)
			{
				MessageBox.Show("Record Update Successfully...");
			}
			else
			{
				MessageBox.Show("Record Save Successfully...");
			}
			BankDefineGridFill();
			BankFromRefresh();
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
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btndelete_Click(object sender, EventArgs e)
	{
	}

	private void GrdBankDefine_DoubleClick_1(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			global::Architecture.Model.Bank bnk = new global::Architecture.Model.Bank();
			RecId = Conversion.ToInt((object)GrdBankDefine.CurrentRow.Cells["Id"].Value.ToString());
			bnk = global::Architecture.BLL.Bank.GetByID(RecId);
			if (bnk != null)
			{
				btnsave.Visible = false;
				btnUpdate.Visible = true;
				((Control)(object)txtishomeland).Text = bnk.IsHomeland;
				txtbranchcode.Text = bnk.BranchCode;
				txtbankname.Text = bnk.BranchName;
				txtBankAccountno.Text = Conversion.ToString((object)bnk.BankAccountNo);
				txtAccountTitle.Text = Conversion.ToString((object)bnk.BankAccountTitle);
				txtBankIBANNo.Text = Conversion.ToString((object)bnk.BankIBANNo);
				txtcontact1.Text = bnk.Contact1Tel;
				txtcontact2.Text = bnk.Contact2Tel;
				txtcellno.Text = bnk.Contact3Mobile;
				txtemail.Text = bnk.emailPrimery;
				txtemailalternate.Text = bnk.emailAlternate;
				combcountry.Value = Conversion.ToInt((object)bnk.CountryId);
				combcity.Value = Conversion.ToInt((object)bnk.BranchCity);
				txtotherinfor.Text = bnk.OtherInfo;
				((Control)(object)CmbChequeTemplete).Text = Conversion.ToString((object)bnk.ChequeTemplete);
				combGlaccount.Value = Conversion.ToInt((object)bnk.ChartOfAccountId);
				txtaddress.Text = bnk.BranchAddress;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BankDefineGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = global::Architecture.BLL.Bank.GetAllDt(new global::Architecture.Model.Bank
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DataTable DataTable = new DataTable();
				DataTable.Columns.Add("Id", typeof(int));
				DataTable.Columns.Add("BranchName", typeof(string));
				DataTable.Columns.Add("BranchCode", typeof(string));
				DataTable.Columns.Add("BankAccountNo", typeof(string));
				DataTable.Columns.Add("BankAccountTitle", typeof(string));
				DataTable.Columns.Add("BankIBANNo", typeof(string));
				DataTable.Columns.Add("Phone No (1)", typeof(string));
				DataTable.Columns.Add("Phone No (2)", typeof(string));
				DataTable.Columns.Add("Mobile No", typeof(string));
				DataTable.Columns.Add("EmailAlternate", typeof(string));
				DataTable.Columns.Add("IsHomeland", typeof(string));
				DataTable.Columns.Add("CountryName", typeof(string));
				DataTable.Columns.Add("CityName", typeof(string));
				DataTable.Columns.Add("Address", typeof(string));
				DataTable.Columns.Add("ChequeTemplete", typeof(string));
				DataTable.Columns.Add("SWIFTCode", typeof(string));
				DataTable.Columns.Add("ChartOfAccountId", typeof(int));
				DataTable.Columns.Add("AccountCode", typeof(string));
				DataTable.Columns.Add("AccountTitle", typeof(string));
				DataTable.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					DataTable.Rows.Add(Conversion.ToInt(dt.Rows[i]["Id"]), Conversion.ToString(dt.Rows[i]["BranchName"]), Conversion.ToString(dt.Rows[i]["BranchCode"]), Conversion.ToString(dt.Rows[i]["BankAccountNo"]), Conversion.ToString(dt.Rows[i]["BankAccountTitle"]), Conversion.ToString(dt.Rows[i]["BankIBANNo"]), Conversion.ToString(dt.Rows[i]["Contact1Tel"]), Conversion.ToString(dt.Rows[i]["Contact2Tel"]), Conversion.ToString(dt.Rows[i]["Contact3Mobile"]), Conversion.ToString(dt.Rows[i]["emailAlternate"]), Conversion.ToString(dt.Rows[i]["IsHomeland"]), Conversion.ToString(dt.Rows[i]["Country"]), Conversion.ToString(dt.Rows[i]["City"]), Conversion.ToString(dt.Rows[i]["BranchAddress"]), Conversion.ToString(dt.Rows[i]["ChequeTemplete"]), Conversion.ToString(dt.Rows[i]["OtherInfo"]), Conversion.ToInt(dt.Rows[i]["ChartOfAccountId"]), Conversion.ToString(dt.Rows[i]["AccountCode"]), Conversion.ToString(dt.Rows[i]["AccountTitle"]));
				}
				GrdBankDefine.DataSource = DataTable;
				GrdBankDefine.RetrieveStructure();
				GridSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettings()
	{
		try
		{
			GrdBankDefine.RootTable.Columns["Id"].Visible = false;
			GrdBankDefine.RootTable.Columns["ChartOfAccountId"].Visible = false;
			GrdBankDefine.DefaultFilterRowComparison = (FilterConditionOperator)7;
			GridEX_Helper.GridWrappingAndColumnSettings(GrdBankDefine, 2, 3);
			GrdBankDefine.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BankName;
			GrdBankDefine.RootTable.Columns["BranchCode"].Width = Constants.InventoryConstants.AccountCode;
			GrdBankDefine.RootTable.Columns["BankAccountNo"].Width = Constants.InventoryConstants.AccountCode + 30;
			GrdBankDefine.RootTable.Columns["BankAccountTitle"].Width = Constants.InventoryConstants.AccountTitle + 30;
			GrdBankDefine.RootTable.Columns["BankIBANNo"].Width = Constants.InventoryConstants.AccountCode + 60;
			GrdBankDefine.RootTable.Columns["Phone No (1)"].Width = 100;
			GrdBankDefine.RootTable.Columns["Phone No (2)"].Width = 100;
			GrdBankDefine.RootTable.Columns["Mobile No"].Width = Constants.InventoryConstants.MobileNo;
			GrdBankDefine.RootTable.Columns["EmailAlternate"].Width = 120;
			GrdBankDefine.RootTable.Columns["IsHomeland"].Width = 80;
			GrdBankDefine.RootTable.Columns["CountryName"].Width = Constants.InventoryConstants.StringConstant;
			GrdBankDefine.RootTable.Columns["CityName"].Width = Constants.InventoryConstants.CityName;
			GrdBankDefine.RootTable.Columns["Address"].Width = Constants.InventoryConstants.AddressConstant;
			GrdBankDefine.RootTable.Columns["ChequeTemplete"].Width = 100;
			GrdBankDefine.RootTable.Columns["SWIFTCode"].Width = 90;
			GrdBankDefine.RootTable.Columns["AccountCode"].Width = Constants.InventoryConstants.AccountCode;
			GrdBankDefine.RootTable.Columns["AccountTitle"].Width = Constants.InventoryConstants.AccountTitle;
			if (formright.DoHaveUpdateRights)
			{
				GrdBankDefine.RootTable.Columns.Add("Edit");
				GrdBankDefine.RootTable.Columns["Edit"].Caption = "Edit";
				GrdBankDefine.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				GrdBankDefine.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
				GrdBankDefine.RootTable.Columns["Edit"].ButtonText = "Edit";
				GrdBankDefine.RootTable.Columns["Edit"].Width = 40;
				GrdBankDefine.RootTable.Columns["Edit"].Position = 0;
				GrdBankDefine.FrozenColumns = 1;
			}
			ctrlGrdBar1_Load_1(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdBankDefine_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GrdBankDefine_DoubleClick_1(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar1_Load_1(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = null;
			if (fs != null)
			{
				GrdBankDefine.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AcfrmDefineBank_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Expected O, but got Unknown
		if (e.KeyData == Keys.Return)
		{
			SendKeys.Send("{TAB}");
		}
		if (formright.DoHaveSaveRight && btnsave.Visible && btnsave.Enabled && e.Control && e.KeyCode == Keys.S)
		{
			btnsave_Click(null, null);
		}
		if (formright.DoHaveUpdateRights && btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
		{
			btnUpdate_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.N)
		{
			btnnew_Click(null, null);
		}
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
		if (e.Control && e.KeyCode == Keys.F5)
		{
			txtishomeland.Focus();
		}
		if (e.Control && e.KeyCode == Keys.Delete)
		{
			btndelete_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.R)
		{
			btnRefresh_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.Down)
		{
			((Control)(object)GrdBankDefine).Focus();
		}
		if (e.Control && e.KeyCode == Keys.Up)
		{
			txtishomeland.Focus();
		}
		if (e.Control && e.Alt)
		{
			MakeShortCutKeys();
		}
		if (formright.DoHaveUpdateRights && e.Control && e.KeyCode == Keys.Return)
		{
			global::Architecture.Model.Bank bnk = new global::Architecture.Model.Bank();
			RecId = Conversion.ToInt((object)GrdBankDefine.CurrentRow.Cells["Id"].Value.ToString());
			bnk = global::Architecture.BLL.Bank.GetByID(RecId);
			if (bnk != null)
			{
				btnsave.Visible = false;
				btnUpdate.Visible = true;
				txtbranchcode.Text = bnk.BranchCode;
				txtbankname.Text = bnk.BranchName;
				txtaddress.Text = bnk.BranchAddress;
				combcity.Value = Conversion.ToInt((object)bnk.BranchCity);
				combcountry.Value = Conversion.ToInt((object)bnk.CountryId);
				txtcontact1.Text = bnk.Contact1Tel;
				txtcontact2.Text = bnk.Contact2Tel;
				txtcellno.Text = bnk.Contact3Mobile;
				txtemail.Text = bnk.emailPrimery;
				txtemailalternate.Text = bnk.emailAlternate;
				((Control)(object)txtishomeland).Text = bnk.IsHomeland;
				txtotherinfor.Text = bnk.OtherInfo;
				combGlaccount.Value = Conversion.ToInt((object)bnk.ChartOfAccountId);
				((Control)(object)CmbChequeTemplete).Text = Conversion.ToString((object)bnk.ChequeTemplete);
			}
		}
	}

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+F5", "For Focus on To Company");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On From Company Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On To Company in Form");
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

	private void btnShortCutKeys_Click(object sender, EventArgs e)
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

	private void GrdBankDefine_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = GrdBankDefine.CurrentRow;
			if (r != null && (int)r.RowType == 0 && GrdBankDefine.CurrentColumn != null)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (GrdBankDefine.CurrentColumn.Key == "Edit")
				{
					GrdBankDefine_DoubleClick_1(null, null);
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
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Expected O, but got Unknown
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Expected O, but got Unknown
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Expected O, but got Unknown
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Expected O, but got Unknown
		//IL_06f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0703: Expected O, but got Unknown
		GridEXLayout GrdBankDefine_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
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
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.AcfrmDefineBank));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.GrdBankDefine = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label15 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label22 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.txtishomeland = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.txtbankname = new System.Windows.Forms.TextBox();
		this.CmbChequeTemplete = new UltraCombo();
		this.label14 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtBankIBANNo = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.txtotherinfor = new System.Windows.Forms.TextBox();
		this.label17 = new System.Windows.Forms.Label();
		this.combGlaccount = new UltraCombo();
		this.combcity = new UltraCombo();
		this.txtBankAccountno = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.txtbranchcode = new System.Windows.Forms.TextBox();
		this.combcountry = new UltraCombo();
		this.txtemailalternate = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtemail = new System.Windows.Forms.TextBox();
		this.txtcontact1 = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.txtcellno = new System.Windows.Forms.TextBox();
		this.txtcontact2 = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btndelete = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.label24 = new System.Windows.Forms.Label();
		this.txtAccountTitle = new System.Windows.Forms.TextBox();
		this.label25 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.txtaddress = new System.Windows.Forms.TextBox();
		this.panel1.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdBankDefine).BeginInit();
		this.panel6.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.txtishomeland).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbChequeTemplete).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combGlaccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combcity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combcountry).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1097, 661);
		this.panel1.TabIndex = 0;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.GrdBankDefine);
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 238);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1097, 423);
		this.panel5.TabIndex = 3;
		this.GrdBankDefine.AllowEdit = (InheritableBoolean)2;
		GrdBankDefine_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdBankDefine.DesignTimeLayout = GrdBankDefine_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdBankDefine).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdBankDefine.FilterMode = (FilterMode)2;
		this.GrdBankDefine.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdBankDefine).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdBankDefine.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GrdBankDefine).Location = new System.Drawing.Point(0, 28);
		((System.Windows.Forms.Control)(object)this.GrdBankDefine).Name = "GrdBankDefine";
		((ScrollableBase)this.GrdBankDefine).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdBankDefine).Size = new System.Drawing.Size(1097, 395);
		((System.Windows.Forms.Control)(object)this.GrdBankDefine).TabIndex = 0;
		this.GrdBankDefine.TotalRow = (InheritableBoolean)1;
		this.GrdBankDefine.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdBankDefine).VisualStyle = (VisualStyle)4;
		this.GrdBankDefine.ColumnButtonClick += new ColumnActionEventHandler(GrdBankDefine_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.GrdBankDefine).DoubleClick += new System.EventHandler(GrdBankDefine_DoubleClick_1);
		((System.Windows.Forms.Control)(object)this.GrdBankDefine).KeyDown += new System.Windows.Forms.KeyEventHandler(GrdBankDefine_KeyDown);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Controls.Add(this.label15);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1097, 28);
		this.panel6.TabIndex = 2;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1054, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.GrdBankDefine;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(41, 24);
		this.ctrlGrdBar1.TabIndex = 21;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load_1);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(4, 4);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(118, 19);
		this.label15.TabIndex = 25;
		this.label15.Text = "Bank  History";
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel4.Location = new System.Drawing.Point(0, 55);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1097, 183);
		this.panel4.TabIndex = 0;
		this.groupBox1.Controls.Add(this.txtaddress);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.txtAccountTitle);
		this.groupBox1.Controls.Add(this.label25);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add(this.label18);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.txtishomeland);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.txtbankname);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbChequeTemplete);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.txtBankIBANNo);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.txtotherinfor);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combGlaccount);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combcity);
		this.groupBox1.Controls.Add(this.txtBankAccountno);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.label16);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.txtbranchcode);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combcountry);
		this.groupBox1.Controls.Add(this.txtemailalternate);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.txtemail);
		this.groupBox1.Controls.Add(this.txtcontact1);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.txtcellno);
		this.groupBox1.Controls.Add(this.txtcontact2);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(6, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(974, 179);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.Red;
		this.label22.Location = new System.Drawing.Point(102, 20);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(13, 16);
		this.label22.TabIndex = 166;
		this.label22.Text = "*";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.Red;
		this.label21.Location = new System.Drawing.Point(102, 153);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(13, 16);
		this.label21.TabIndex = 165;
		this.label21.Text = "*";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.ForeColor = System.Drawing.Color.Red;
		this.label20.Location = new System.Drawing.Point(102, 102);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(13, 16);
		this.label20.TabIndex = 164;
		this.label20.Text = "*";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.Red;
		this.label19.Location = new System.Drawing.Point(102, 74);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(13, 16);
		this.label19.TabIndex = 163;
		this.label19.Text = "*";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.Color.Red;
		this.label18.Location = new System.Drawing.Point(102, 47);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(13, 16);
		this.label18.TabIndex = 162;
		this.label18.Text = "*";
		this.txtishomeland.AllowNull = (DefaultableBoolean)1;
		this.txtishomeland.AutoCompleteMode = (AutoCompleteMode)4;
		this.txtishomeland.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.txtishomeland.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.txtishomeland).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.txtishomeland).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.txtishomeland).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.txtishomeland).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.txtishomeland).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.txtishomeland).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.txtishomeland).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.txtishomeland).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.txtishomeland).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.txtishomeland).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.txtishomeland).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.txtishomeland).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.txtishomeland).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.txtishomeland).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance59).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.txtishomeland).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.txtishomeland).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.txtishomeland).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.txtishomeland).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.txtishomeland).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.txtishomeland).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.txtishomeland).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.txtishomeland).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.txtishomeland).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.txtishomeland).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.txtishomeland).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.txtishomeland).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.txtishomeland).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.txtishomeland).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtishomeland.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.txtishomeland).Location = new System.Drawing.Point(117, 15);
		((System.Windows.Forms.Control)(object)this.txtishomeland).Name = "txtishomeland";
		((System.Windows.Forms.Control)(object)this.txtishomeland).Size = new System.Drawing.Size(227, 26);
		((System.Windows.Forms.Control)(object)this.txtishomeland).TabIndex = 0;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(4, 21);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(81, 15);
		this.label9.TabIndex = 15;
		this.label9.Text = "Is Home Land";
		this.txtbankname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbankname.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbankname.Location = new System.Drawing.Point(117, 44);
		this.txtbankname.Name = "txtbankname";
		this.txtbankname.Size = new System.Drawing.Size(227, 23);
		this.txtbankname.TabIndex = 1;
		this.CmbChequeTemplete.AllowNull = (DefaultableBoolean)1;
		this.CmbChequeTemplete.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbChequeTemplete.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbChequeTemplete.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbChequeTemplete).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbChequeTemplete).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbChequeTemplete).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbChequeTemplete).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChequeTemplete).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.CmbChequeTemplete).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbChequeTemplete).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbChequeTemplete).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbChequeTemplete).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbChequeTemplete).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbChequeTemplete).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbChequeTemplete).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbChequeTemplete).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChequeTemplete).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbChequeTemplete).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbChequeTemplete).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbChequeTemplete).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbChequeTemplete).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbChequeTemplete).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbChequeTemplete).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbChequeTemplete).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbChequeTemplete).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbChequeTemplete).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbChequeTemplete).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbChequeTemplete).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbChequeTemplete).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbChequeTemplete).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbChequeTemplete).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbChequeTemplete.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbChequeTemplete).Location = new System.Drawing.Point(719, 69);
		((System.Windows.Forms.Control)(object)this.CmbChequeTemplete).Name = "CmbChequeTemplete";
		((System.Windows.Forms.Control)(object)this.CmbChequeTemplete).Size = new System.Drawing.Size(249, 26);
		((System.Windows.Forms.Control)(object)this.CmbChequeTemplete).TabIndex = 14;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(616, 75);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(99, 15);
		this.label14.TabIndex = 39;
		this.label14.Text = "Cheque Templete";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(4, 48);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(68, 15);
		this.label1.TabIndex = 21;
		this.label1.Text = "Bank Name";
		this.txtBankIBANNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBankIBANNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBankIBANNo.Location = new System.Drawing.Point(117, 150);
		this.txtBankIBANNo.Name = "txtBankIBANNo";
		this.txtBankIBANNo.Size = new System.Drawing.Size(227, 23);
		this.txtBankIBANNo.TabIndex = 5;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(616, 48);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(73, 15);
		this.label12.TabIndex = 23;
		this.label12.Text = "SWIFT Code";
		this.txtotherinfor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtotherinfor.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtotherinfor.Location = new System.Drawing.Point(719, 44);
		this.txtotherinfor.Name = "txtotherinfor";
		this.txtotherinfor.Size = new System.Drawing.Size(249, 23);
		this.txtotherinfor.TabIndex = 13;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(4, 154);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(82, 15);
		this.label17.TabIndex = 13;
		this.label17.Text = "IBAN Number";
		this.combGlaccount.AllowNull = (DefaultableBoolean)1;
		this.combGlaccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.combGlaccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combGlaccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combGlaccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.combGlaccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combGlaccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combGlaccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combGlaccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.combGlaccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combGlaccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.combGlaccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combGlaccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combGlaccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combGlaccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.combGlaccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combGlaccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combGlaccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance52).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combGlaccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.combGlaccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combGlaccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combGlaccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).TextHAlignAsString = "Left";
		((UltraGridBase)this.combGlaccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.combGlaccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combGlaccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combGlaccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.combGlaccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combGlaccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.combGlaccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combGlaccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combGlaccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combGlaccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combGlaccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combGlaccount).Location = new System.Drawing.Point(719, 97);
		((System.Windows.Forms.Control)(object)this.combGlaccount).Name = "combGlaccount";
		((System.Windows.Forms.Control)(object)this.combGlaccount).Size = new System.Drawing.Size(249, 26);
		((System.Windows.Forms.Control)(object)this.combGlaccount).TabIndex = 15;
		this.combcity.AllowNull = (DefaultableBoolean)1;
		this.combcity.AutoCompleteMode = (AutoCompleteMode)4;
		this.combcity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combcity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combcity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.combcity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combcity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combcity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.combcity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.combcity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combcity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combcity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combcity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.combcity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combcity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combcity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.combcity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combcity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.combcity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.combcity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combcity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combcity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.combcity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combcity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.combcity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combcity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combcity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combcity).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combcity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combcity).Location = new System.Drawing.Point(719, 15);
		((System.Windows.Forms.Control)(object)this.combcity).Name = "combcity";
		((System.Windows.Forms.Control)(object)this.combcity).Size = new System.Drawing.Size(249, 26);
		((System.Windows.Forms.Control)(object)this.combcity).TabIndex = 12;
		this.txtBankAccountno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBankAccountno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBankAccountno.Location = new System.Drawing.Point(117, 99);
		this.txtBankAccountno.Name = "txtBankAccountno";
		this.txtBankAccountno.Size = new System.Drawing.Size(227, 23);
		this.txtBankAccountno.TabIndex = 3;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(616, 103);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(69, 15);
		this.label13.TabIndex = 24;
		this.label13.Text = "GL Account";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(348, 128);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(87, 15);
		this.label11.TabIndex = 20;
		this.label11.Text = "Email Alternate";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(4, 103);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(99, 15);
		this.label16.TabIndex = 13;
		this.label16.Text = "Account Number";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(616, 21);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(62, 15);
		this.label5.TabIndex = 28;
		this.label5.Text = "City Name";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(348, 103);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(36, 15);
		this.label10.TabIndex = 18;
		this.label10.Text = "Email";
		this.txtbranchcode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbranchcode.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbranchcode.Location = new System.Drawing.Point(117, 71);
		this.txtbranchcode.Name = "txtbranchcode";
		this.txtbranchcode.Size = new System.Drawing.Size(227, 23);
		this.txtbranchcode.TabIndex = 2;
		this.combcountry.AllowNull = (DefaultableBoolean)1;
		this.combcountry.AutoCompleteMode = (AutoCompleteMode)4;
		this.combcountry.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combcountry.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combcountry).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.combcountry).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combcountry).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combcountry).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcountry).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.combcountry).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcountry).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.combcountry).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combcountry).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combcountry).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combcountry).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.combcountry).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combcountry).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcountry).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combcountry).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.combcountry).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combcountry).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcountry).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.combcountry).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.combcountry).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combcountry).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combcountry).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.combcountry).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combcountry).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.combcountry).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combcountry).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combcountry).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combcountry).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combcountry.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combcountry).Location = new System.Drawing.Point(436, 148);
		((System.Windows.Forms.Control)(object)this.combcountry).Name = "combcountry";
		((System.Windows.Forms.Control)(object)this.combcountry).Size = new System.Drawing.Size(176, 26);
		((System.Windows.Forms.Control)(object)this.combcountry).TabIndex = 11;
		this.txtemailalternate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtemailalternate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtemailalternate.Location = new System.Drawing.Point(436, 124);
		this.txtemailalternate.Name = "txtemailalternate";
		this.txtemailalternate.Size = new System.Drawing.Size(176, 23);
		this.txtemailalternate.TabIndex = 10;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(4, 75);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(74, 15);
		this.label2.TabIndex = 13;
		this.label2.Text = "Branch Code";
		this.txtemail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtemail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtemail.Location = new System.Drawing.Point(436, 99);
		this.txtemail.Name = "txtemail";
		this.txtemail.Size = new System.Drawing.Size(176, 23);
		this.txtemail.TabIndex = 9;
		this.txtcontact1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcontact1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcontact1.Location = new System.Drawing.Point(436, 17);
		this.txtcontact1.Name = "txtcontact1";
		this.txtcontact1.Size = new System.Drawing.Size(176, 23);
		this.txtcontact1.TabIndex = 6;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(348, 154);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(84, 15);
		this.label4.TabIndex = 29;
		this.label4.Text = "Country Name";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(348, 21);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(76, 15);
		this.label7.TabIndex = 17;
		this.label7.Text = "Phone No (1)";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(348, 48);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(78, 15);
		this.label8.TabIndex = 19;
		this.label8.Text = "Phone No (2)";
		this.txtcellno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcellno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcellno.Location = new System.Drawing.Point(436, 71);
		this.txtcellno.Name = "txtcellno";
		this.txtcellno.Size = new System.Drawing.Size(176, 23);
		this.txtcellno.TabIndex = 8;
		this.txtcontact2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcontact2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcontact2.Location = new System.Drawing.Point(436, 44);
		this.txtcontact2.Name = "txtcontact2";
		this.txtcontact2.Size = new System.Drawing.Size(176, 23);
		this.txtcontact2.TabIndex = 7;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(348, 75);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(63, 15);
		this.label6.TabIndex = 16;
		this.label6.Text = "Mobile No";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1097, 28);
		this.panel3.TabIndex = 1;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(4, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(108, 19);
		this.label23.TabIndex = 25;
		this.label23.Text = "Bank Define";
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1097, 27);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.btnnew, this.btnRefresh, this.btnUpdate, this.btnsave, this.btndelete, this.btnShortCutKeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1097, 27);
		this.toolStrip1.TabIndex = 2;
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
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btndelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btndelete.Image = Architecture.WinApp.Properties.Resources.icons8_cut_16;
		this.btndelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btndelete.Name = "btndelete";
		this.btndelete.Size = new System.Drawing.Size(73, 24);
		this.btndelete.Text = "&Delete";
		this.btndelete.Visible = false;
		this.btndelete.Click += new System.EventHandler(btndelete_Click);
		this.btnShortCutKeys.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnShortCutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKeys.Image = (System.Drawing.Image)resources.GetObject("btnShortCutKeys.Image");
		this.btnShortCutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeys.Name = "btnShortCutKeys";
		this.btnShortCutKeys.Size = new System.Drawing.Size(106, 24);
		this.btnShortCutKeys.Text = "ShortCut Keys";
		this.btnShortCutKeys.Click += new System.EventHandler(btnShortCutKeys_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.Color.Red;
		this.label24.Location = new System.Drawing.Point(102, 127);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(13, 16);
		this.label24.TabIndex = 169;
		this.label24.Text = "*";
		this.txtAccountTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAccountTitle.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAccountTitle.Location = new System.Drawing.Point(117, 124);
		this.txtAccountTitle.Name = "txtAccountTitle";
		this.txtAccountTitle.Size = new System.Drawing.Size(227, 23);
		this.txtAccountTitle.TabIndex = 4;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(4, 128);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(78, 15);
		this.label25.TabIndex = 168;
		this.label25.Text = "Account Title";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(616, 128);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(49, 15);
		this.label26.TabIndex = 170;
		this.label26.Text = "Address";
		this.txtaddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtaddress.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtaddress.Location = new System.Drawing.Point(719, 124);
		this.txtaddress.Multiline = true;
		this.txtaddress.Name = "txtaddress";
		this.txtaddress.Size = new System.Drawing.Size(249, 50);
		this.txtaddress.TabIndex = 16;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1097, 661);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.KeyPreview = true;
		base.Name = "AcfrmDefineBank";
		this.Text = "Define Bank";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(AcfrmDefineBank_FormClosing);
		base.Load += new System.EventHandler(AcfrmDefineBank_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(AcfrmDefineBank_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdBankDefine).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.txtishomeland).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbChequeTemplete).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combGlaccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combcity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combcountry).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}

}