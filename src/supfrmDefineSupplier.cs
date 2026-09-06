using Country = global::Architecture.Model.Country;
// // using Architecture.Reports;
// using Architecture.WinApp.Helper;
// using Architecture.WinApp.Inventory_Definition;
// using Architecture.WinApp.Lookups;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.General;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using grid.User_Conrtols;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using ChartofAccount = global::Architecture.Model.Accounts.ChartofAccount;
using BankReconciliation = global::Architecture.Model.Accounts.BankReconciliation;

namespace Architecture.WinApp.Supplier_Purchase
{
using FormHelper = Architecture.WinApp.Helper.FormHelper;
using DatatableHelper = Architecture.WinApp.Helper.DatatableHelper;
using BusinessType = global::Architecture.Model.BusinessType;

using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using CommonServices = global::Architecture.WinApp.Common.CommonServices;
using Bank = global::Architecture.Model.Bank;
using City = global::Architecture.BLL.City;
using CheqBookHeader = global::Architecture.Model.Accounts.CheqBookHeader;
using CheqBookDetail = global::Architecture.Model.Accounts.CheqBookDetail;
using COAAllocation = global::Architecture.Model.Accounts.COAAllocation;
using ChartOfAccountMultiLingo = global::Architecture.Model.Accounts.ChartOfAccountMultiLingo;
using TaxesTypes = global::Architecture.Model.Inventory.TaxesTypes;
using SupplierCustomerShipToAddress = global::Architecture.Model.Inventory.SupplierCustomerShipToAddress;
using AccountsOpeningBalances = global::Architecture.BLL.Accounts.AccountsOpeningBalances;
using ChartofAccount = global::Architecture.BLL.Accounts.ChartofAccount;
using ShortCutKeyPopUp = global::Architecture.WinApp.Common.ShortCutKeyPopUp;
using Constants = global::Architecture.WinApp.Common.Constants;
using GridEX_Helper = global::Architecture.WinApp.Common.GridEX_Helper;
using InfragisticsHelper = global::Architecture.WinApp.Common.InfragisticsHelper;
using HierarchicalMode = Janus.Windows.GridEX.HierarchicalMode;
using SelfReferencingAggregationMode = Janus.Windows.GridEX.SelfReferencingAggregationMode;

using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;

public class supfrmDefineSupplier : Form
{
	private Rightsobjects formright = new Rightsobjects();

	public UserAccount UserAccount = new UserAccount();

	private DataTable dtcity = new DataTable();

	private DataTable dtGridRegister = new DataTable();

	private DataTable dtGridRegisterFromDb = new DataTable();

	private bool FormInitialized;

	private string ScreenName = "";

	private int RecId;

	private int GlRecId;

	private bool Configuration;

	private string saveDirectory = "";

	private string fileSavePath = "";

	private string ofd = "";

	private bool AttributesForItemFeature;

	private bool SubsidiaryAccountAllownOnVouchers;

	private IContainer components;

	private Panel panel4;

	private Panel panel3;

	private Label label20;

	private Panel panel2;

	private ToolStrip toolStrip2;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private Panel panel6;

	private Button btnAdd;

	private Label label4;

	private Button browse;

	private PictureBox box;

	private CheckBox chkactivestatus;

	private Label label57;

	private CheckBox chksubac;

	private DateTimePicker txtcnicexp;

	private Label label31;

	private Label label32;

	private Label label56;

	private Label label34;

	private Label label35;

	private Label label55;

	private Label label36;

	private Label label54;

	private TextBox txtcompany;

	private TextBox txttown;

	private Label label53;

	private TextBox txtofficemobile;

	private Label label52;

	private TextBox txtaddress2;

	private TextBox txttitle;

	private Label label51;

	private TextBox txtstrnno;

	private TextBox txtzipcode;

	private TextBox txtlastname;

	private TextBox txtemail;

	private TextBox txtntnno;

	private Label label37;

	private Label label49;

	private Label label38;

	private Label label48;

	private Label label39;

	private Label label47;

	private Label label40;

	private Label label46;

	private Label label41;

	private Label label42;

	private Label label44;

	private TextBox txtwebsite;

	private Label label43;

	private TextBox txtfirstname;

	private TextBox txtaddress1;

	private ToolStripButton btnBankDetail;

	private ToolStripButton btnShipToAddress;

	private ToolStripButton btnSupplierLimits;

	private Label label8;

	internal UltraCombo cmbprovince;

	private MaskedTextBox txtcnicno;

	private MaskedTextBox txtphone;

	internal UltraCombo cmbglac;

	internal UltraCombo cmbgroupdetail;

	internal UltraCombo cmbcity;

	internal UltraCombo cmbcountry;

	internal UltraCombo cmbparentac;

	internal UltraCombo CmbAdvanceAc;

	private TextBox txtpermobile;

	internal UltraCombo cmbDiscountType;

	private Label label9;

	internal UltraCombo cmbPartyType;

	private Label label13;

	private TextBox txtPartyCode;

	private Label label15;

	private ToolStripButton toolStripButton1;

	private TextBox txtManualNo;

	private Label label1;

	private ToolStripButton BtnAttributesAllocation;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	internal GridEX DataGridHistory;

	private Panel panel9;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label22;

	private Panel panel1;

	private GroupBox groupBox1;

	private Button btnshow;

	internal UltraCombo CmbGlAccountRegister;

	private Label label7;

	private Label label21;

	private TextBox txtInvoiceNo;

	private Panel panel8;

	private Label label23;

	private ToolStrip toolStrip3;

	private ToolStripButton BtnResetRegister;

	private ToolStripButton BtnRefreshRegister;

	private ToolStripButton btnRegister;

	private ToolStripButton BtnShortCutKeysRegister;

	internal UltraCombo CmbPartyGroupRegister;

	private Label label3;

	internal UltraCombo CmbCityRegister;

	private Label label2;

	internal UltraCombo CmbPartyTypeRegister;

	private Label label5;

	private ToolStripButton btnRefresh;

	private TextBox txtWhatsAppNo;

	private Label label6;

	private TextBox txtSubPartyTotal;

	private Label label10;

	private Label label11;

	internal UltraCombo CmbBusinessType;

	private Label label12;

	private TextBox txtNickName;

	private GroupBox groupBox2;

	private Label label28;

	private Label label33;

	private Label label30;

	private Label label29;

	private Label LocCNIC;

	private Label LocNTN;

	private Label label25;

	private Label label24;

	private Label label19;

	private Label label18;

	private Label label16;

	private Label label58;

	private Label label50;

	private Button BtnBusinessType;

	private Label label17;

	public supfrmDefineSupplier(UserAccount puserAccount)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = puserAccount;
		InitializeComponentCustom();
		InitializeComponentMethod();
	}

	private void InitializeComponentCustom()
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1100, 800);
			dtcity.Columns.Add("Id");
			dtcity.Columns.Add("CityName");
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
			ScreenName = Conversion.ToString(base.Tag);
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			AttributesForItemFeature = CommonServices.GetERPFeatureById(13);
			formright = CommonServices.SetRightsValueInRightsObject(ScreenName);
			DataTable comboData = GetComboData();
			DataTable partyTypeData = PartyTypeDbCall();
			DataTable supplierGroupData = SupplierGroupDBCall();
			DataTable discountPolicyData = DiscountPolicyDbCall();
			DataTable countryData = CountryDtDbCall();
			DataTable businessTypeData = global::Architecture.BLL.BusinessType.ComboBind(UserAccount.OrganizationId, UserAccount.CompanyId);
			return new
			{
				HistoryComboData = comboData,
				PartyTypeData = partyTypeData,
				SupplierGroupData = supplierGroupData,
				DiscountPolicyData = discountPolicyData,
				CountryData = countryData,
				BusinessTypeData = businessTypeData
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
				btnsave.Enabled = formright.DoHaveSaveRight;
				btnAdd.Enabled = formright.DoHaveSaveRight;
				btnupdate.Enabled = formright.DoHaveUpdateRights;
				ctrlGrdBar2.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
				ctrlGrdBar2.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
				ctrlGrdBar2.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
				ctrlGrdBar2.mGridExport.Enabled = formright.DoHaveGridExportRights;
				ctrlGrdBar2.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
				ctrlGrdBar2.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
				BtnAttributesAllocation.Visible = AttributesForItemFeature;
				if (SubsidiaryAccountAllownOnVouchers)
				{
					if (ScreenName == "frmDefineSupplierOnly")
					{
						cmbPartyType.Value = 1;
						((Control)(object)cmbPartyType).Enabled = false;
					}
					else if (ScreenName == "frmDefineCustomerOnly")
					{
						cmbPartyType.Value = 2;
						((Control)(object)cmbPartyType).Enabled = false;
					}
				}
				PartyTypeBind(result.PartyTypeData);
				SupplierGroupBind(result.SupplierGroupData);
				DiscountPolicyBind(result.DiscountPolicyData);
				CountryBind(result.CountryData);
				CityDtFillFromGlobalAndBind();
				BusinessTypeBind(result.BusinessTypeData);
				GetConfigurationsFromGlobalAndBindValuesInColumns();
				BindSupplierName(SupplierDtGetFromGlobal());
				AllComboBind(result.HistoryComboData);
				FormInitialized = true;
				((Control)(object)cmbparentac).Enabled = false;
				txtcompany.Focus();
				btnupdate.Visible = false;
				((Control)(object)cmbprovince).Enabled = true;
				((Control)(object)cmbcity).Enabled = true;
				cmbPartyType.Focus();
				((Control)(object)cmbDiscountType).Enabled = false;
				((Control)(object)cmbDiscountType).Text = string.Empty;
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void GetConfigurationsFromGlobalAndBindValuesInColumns()
	{
		try
		{
			int CityId = Conversion.ToInt((object)"0");
			if (CityId > 0)
			{
				cmbcity.Value = CityId;
			}
			int partyGroupId = Conversion.ToInt((object)"0");
			if (CityId > 0)
			{
				cmbgroupdetail.Value = partyGroupId;
			}
			else
			{
				((UltraGridBase)cmbgroupdetail).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Configuration Error" + ex.Message);
		}
	}

	private DataTable PartyTypeDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return global::Architecture.BLL.Inventory.SupplierCustomer.PartyTypeGetAll(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PartyTypeBind(DataTable dtParty)
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("SupplierCustomerType");
			dt.Columns.Add("Prefix");
			foreach (DataRow dr in dtParty.Rows)
			{
				dt.Rows.Add(dr["Id"], dr["SupplierCustomerType"], dr["Prefix"]);
			}
			InfragisticsHelper.BindAndRetainSelection(cmbPartyType, dt, "Id", "SupplierCustomerType", "Party Type", AllColumns: true, Conversion.ToInt(cmbPartyType.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)cmbPartyType).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbPartyType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (!SubsidiaryAccountAllownOnVouchers)
				{
					((UltraGridBase)cmbPartyType).Rows[0].Activate();
				}
			}
			cmbPartyType_Leave(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetPartyCodeDbCall(int PartyId)
	{
		try
		{
			string Code = "";
			if (PartyId > 0)
			{
				Code = global::Architecture.BLL.Inventory.SupplierCustomer.GeneratePartyCode(UserAccount.OrganizationId, UserAccount.CompanyId, PartyId);
			}
			txtPartyCode.Text = Code;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AcountsBindByPartyType(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(cmbglac, dt, "Id", "AccountTitle", "Account Title", AllColumns: true, Conversion.ToInt(cmbglac.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbAdvanceAc, dt, "Id", "AccountTitle", "Account Title", AllColumns: true, Conversion.ToInt(CmbAdvanceAc.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable SupplierGroupDBCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return global::Architecture.BLL.Inventory.CustomerGroup.GetAll(new global::Architecture.Model.Inventory.CustomerGroup
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void SupplierGroupBind(DataTable dtCG)
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Description");
			if (dtCG.Rows.Count > 0)
			{
				foreach (DataRow dr in dtCG.Rows)
				{
					dt.Rows.Add(dr["Id"], dr["Description"]);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(cmbgroupdetail, dt, "Id", "Description", "Customer Group", AllColumns: true, Conversion.ToInt(cmbgroupdetail.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable DiscountPolicyDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return global::Architecture.BLL.Inventory.DiscountPolicy.GetDiscountPolicyForCustomers(new global::Architecture.Model.ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DiscountPolicyBind(DataTable dtCG)
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("PolicyName");
			dt.Columns.Add("DiscountType");
			dt.Columns.Add("DiscountRate");
			dt.Columns.Add("EffectiveDate");
			if (dtCG.Rows.Count > 0)
			{
				foreach (DataRow dr in dtCG.Rows)
				{
					dt.Rows.Add(dr["Id"], dr["PolicyName"], dr["DiscountType"], dr["DiscountRate"], dr["EffectiveDate"]);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(cmbDiscountType, dt, "Id", "PolicyName", "Discount Policy", AllColumns: true, Conversion.ToInt(cmbDiscountType.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable CountryDtDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return country.GetAll(new Country
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CountryBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(cmbcountry, dt, "Id", "Description", "Country", AllColumns: false, Conversion.ToInt(cmbcountry.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)cmbcountry).Rows[1].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CityDtFillFromGlobalAndBind()
	{
		try
		{
			List<GlobalAllCity> lst = clsGlobalVariables.globalAllCities;
			DatatableHelper.PopulateDataTable(dtcity, lst, (GlobalAllCity x) => x.Id, (GlobalAllCity x) => x.CityName);
			int Id = Conversion.ToInt(cmbcity.Value);
			InfragisticsHelper.BindAndRetainSelection(cmbcity, dtcity, "Id", "CityName", "City Name", AllColumns: true, Id, insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable SupplierDtGetFromGlobal()
	{
		try
		{
			DataTable dtSupplier = new DataTable();
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("CompanyName", typeof(string));
			dtSupplier.Columns.Add("PartyCode", typeof(string));
			dtSupplier.Columns.Add("CityName", typeof(string));
			dtSupplier.Columns.Add("MobileNo", typeof(string));
			foreach (getGlobalAllSupplierCustomer item in clsGlobalVariables.globalAllSupplierCustomer)
			{
				dtSupplier.Rows.Add(Conversion.ToInt((object)item.Id), Conversion.ToString((object)item.CompanyName), Conversion.ToString((object)item.PartyCode), Conversion.ToString((object)item.CityName), Conversion.ToString((object)item.MobilePersonal));
			}
			return dtSupplier;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindSupplierName(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(cmbparentac, dt, "Id", "CompanyName", "Party Name", AllColumns: true, Conversion.ToInt(cmbparentac.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BusinessTypeBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbBusinessType, dt, "Id", "BusinessTypeName", "Business Type", AllColumns: false, Conversion.ToInt(CmbBusinessType.Value), insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void cmbPartyType_Leave(object sender, EventArgs e)
	{
		try
		{
			int PartyId = Conversion.ToInt(cmbPartyType.Value);
			DataTable dataTable = new DataTable();
			if (PartyId > 0)
			{
				dataTable = global::Architecture.BLL.Inventory.SupplierCustomer.GetAccountsForSupplierCustomerByPartyTypeId(UserAccount.OrganizationId, UserAccount.CompanyId, PartyId, GlRecId);
			}
			AcountsBindByPartyType(dataTable);
			GetPartyCodeDbCall(PartyId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbgroupdetail_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(cmbgroupdetail.Value) == 3)
			{
				((Control)(object)cmbDiscountType).Enabled = true;
				DiscountPolicyBind(DiscountPolicyDbCall());
			}
			else
			{
				((UltraGridBase)cmbDiscountType).DataSource = null;
				((Control)(object)cmbDiscountType).Text = string.Empty;
				((Control)(object)cmbDiscountType).Enabled = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void chksubac_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)cmbparentac).Enabled = chksubac.Checked;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbglac_Leave(object sender, EventArgs e)
	{
		try
		{
			_ = SubsidiaryAccountAllownOnVouchers;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SaveImageFile()
	{
		if (!string.IsNullOrWhiteSpace(ofd) && File.Exists(ofd))
		{
			File.Copy(ofd, fileSavePath, overwrite: true);
			ofd = string.Empty;
			fileSavePath = string.Empty;
			box.Image = null;
			box.Invalidate();
		}
	}

	private void Insert()
	{
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_045e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_051b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0536: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Unknown result type (might be due to invalid IL or missing references)
		//IL_056c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0587: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_060e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0629: Unknown result type (might be due to invalid IL or missing references)
		//IL_063f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0655: Unknown result type (might be due to invalid IL or missing references)
		//IL_0670: Unknown result type (might be due to invalid IL or missing references)
		//IL_067c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0688: Expected O, but got Unknown
		try
		{
			List<(Control, string, FormHelper.ValidType)> controlsToValidate = new List<(Control, string, FormHelper.ValidType)>
			{
				((Control)(object)cmbPartyType, "Party Type", FormHelper.ValidType.INT),
				(txtPartyCode, "Party Code", FormHelper.ValidType.String),
				(txtcompany, "Company / Business Name", FormHelper.ValidType.String),
				((Control)(object)cmbglac, "GL Account", FormHelper.ValidType.INT),
				((Control)(object)cmbgroupdetail, "Party Group", FormHelper.ValidType.INT),
				((Control)(object)CmbBusinessType, "Company / Individual", FormHelper.ValidType.INT),
				(txtaddress1, "Address", FormHelper.ValidType.String),
				(txtpermobile, "Phone Number", FormHelper.ValidType.String),
				(txtWhatsAppNo, "WhatsApp Number", FormHelper.ValidType.String),
				((Control)(object)cmbcountry, "Country", FormHelper.ValidType.INT),
				((Control)(object)cmbcity, "City", FormHelper.ValidType.INT)
			};
			if (chksubac.Checked)
			{
				int index = controlsToValidate.FindIndex(((Control control, string label, FormHelper.ValidType) x) => x.control == cmbglac);
				controlsToValidate.Insert((index != -1) ? (index + 1) : controlsToValidate.Count, ((Control)(object)cmbparentac, "Parent Account", FormHelper.ValidType.INT));
			}
			if (Convert.ToInt32(CmbBusinessType.Value) == 1)
			{
				controlsToValidate.Add((txtntnno, "NTN #", FormHelper.ValidType.String));
			}
			else
			{
				controlsToValidate.Add((txtcnicno, "CNIC Number", FormHelper.ValidType.String));
			}
			if (!FormHelper.ValidateControls(controlsToValidate))
			{
				return;
			}
			if (btnsave.Visible && btnsave.Enabled && !chkactivestatus.Checked)
			{
				MessageBox.Show("Please check the Active Status!!!");
				chkactivestatus.Focus();
			}
			else if (FormHelper.ConfirmAction((RecId == 0) ? "Are you sure to Save" : "Are you sure to Update"))
			{
				global::Architecture.BLL.Inventory.SupplierCustomer.Save(new global::Architecture.Model.Inventory.SupplierCustomer
				{
					Id = Conversion.ToInt((object)RecId),
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					EntryUser = UserAccount.ID,
					ModifyUser = UserAccount.ID,
					EntryDate = DateTime.Now,
					ModifyDate = DateTime.Now,
					PostDate = DateTime.Now,
					PostState = false,
					FTN_No = "0",
					CreditLimit = 0.0,
					IsDebitCredit = false,
					DebitCreditAmount = 0.0,
					PartyTypeId = Conversion.ToInt(cmbPartyType.Value),
					PartyTypePrefix = ((RecId > 0) ? Conversion.ToString(((UltraDropDownBase)cmbPartyType).SelectedRow.Cells[2].Value) : Conversion.ToString((object)txtPartyCode.Text)),
					GlAccountId = Conversion.ToInt(cmbglac.Value),
					NickName = Conversion.ToString((object)txtNickName.Text.Trim()),
					CompanyName = Conversion.ToString((object)txtcompany.Text.Trim()),
// 					ReportingTitle = Conversion.ToString((object)txtcompany.Text.Trim()),
					ManualPartyCode = txtManualNo.Text,
					SupCustCode = txtManualNo.Text.ToLower().Trim(),
					BusinessTypeId = Conversion.ToInt(CmbBusinessType.Value),
					CompanyIndividuals = Conversion.ToString((object)((Control)(object)CmbBusinessType).Text.Trim()),
					CustomerGroupId = Conversion.ToInt(cmbgroupdetail.Value),
					DiscountPolicyId = Conversion.ToInt(cmbDiscountType.Value),
					CountryId = Conversion.ToInt(cmbcountry.Value),
					StateProvinceId = Conversion.ToInt(cmbprovince.Value),
					CityId = Conversion.ToInt(cmbcity.Value),
					MobilePersonal = Conversion.ToString((object)txtpermobile.Text.Trim()),
					WhatsAppNo = Conversion.ToString((object)txtWhatsAppNo.Text.Trim()),
					IsSubSupCust = Conversion.ToBool((object)chksubac.Checked),
					FirstName = Conversion.ToString((object)txtfirstname.Text.Trim()),
					LastName = Conversion.ToString((object)txtlastname.Text.Trim()),
					Title = Conversion.ToString((object)txttitle.Text.Trim()),
					CNIC = Conversion.ToString((object)txtcnicno.Text.Trim()),
					CNIC_EXPIRY_DATE = Conversion.ToDateTime((object)txtcnicexp.Value),
					Address1 = Conversion.ToString((object)txtaddress1.Text.Trim()),
					Address2 = Conversion.ToString((object)txtaddress2.Text.Trim()),
					Email = Conversion.ToString((object)txtemail.Text.Trim()),
					Phone = Conversion.ToString((object)txtphone.Text.Trim()),
					MobileOffice = Conversion.ToString((object)txtofficemobile.Text.Trim()),
					WebPage = Conversion.ToString((object)txtwebsite.Text.Trim()),
					Town = Conversion.ToString((object)txttown.Text.Trim()),
					STRN_No = Conversion.ToString((object)txtstrnno.Text.Trim()),
					NTN_No = Conversion.ToString((object)txtntnno.Text.Trim()),
					ZipCode = Conversion.ToString((object)txtzipcode.Text.Trim()),
					AdvanceGlAcId = Conversion.ToInt(CmbAdvanceAc.Value),
					ParentsSupCustId = Conversion.ToInt(cmbparentac.Value),
					Status = Conversion.ToBool((object)chkactivestatus.Checked),
					PictureURL = fileSavePath,
					IsTaxable = false
				});
				if (RecId > 0)
				{
					MessageBox.Show("Update Successfully");
				}
				else
				{
					MessageBox.Show("Data Save Successfully");
				}
				SaveImageFile();
				FormReset();
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

	private void btnupdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("Rec Id not found...");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int ID)
	{
		try
		{
			RecId = ID;
			global::Architecture.Model.Inventory.SupplierCustomer supcus = global::Architecture.BLL.Inventory.SupplierCustomer.GetByID(RecId);
			if (supcus == null)
			{
				return;
			}
			btnsave.Visible = false;
			btnupdate.Visible = true;
			btnAdd.Visible = false;
			tabControl1.SelectedIndex = 0;
			cmbPartyType.Value = Conversion.ToInt((object)supcus.PartyTypeId);
			GlRecId = supcus.GlAccountId;
			cmbPartyType_Leave(null, null);
			((Control)(object)cmbPartyType).Enabled = false;
			cmbglac.Value = Conversion.ToInt((object)supcus.GlAccountId);
			txtcompany.Text = Conversion.ToString((object)supcus.CompanyName);
			txtNickName.Text = Conversion.ToString((object)supcus.NickName);
			txtManualNo.Text = Conversion.ToString((object)supcus.ManualPartyCode);
			cmbgroupdetail.Value = Conversion.ToInt((object)supcus.CustomerGroupId);
			cmbDiscountType.Value = Conversion.ToInt((object)supcus.DiscountPolicyId);
			cmbcountry.Value = Conversion.ToString((object)supcus.CountryId);
			cmbprovince.Value = Conversion.ToString((object)supcus.StateProvinceId);
			cmbcity.Value = Conversion.ToString((object)supcus.CityId);
			txtpermobile.Text = Conversion.ToString((object)supcus.MobilePersonal);
			txtWhatsAppNo.Text = Conversion.ToString((object)supcus.WhatsAppNo);
			chkactivestatus.Checked = Conversion.ToBool((object)supcus.Status);
			((Control)(object)CmbBusinessType).Text = Conversion.ToString((object)supcus.CompanyIndividuals);
			cmbparentac.Value = Conversion.ToString((object)supcus.ParentsSupCustId);
			txtfirstname.Text = Conversion.ToString((object)supcus.FirstName);
			txtlastname.Text = Conversion.ToString((object)supcus.LastName);
			txttitle.Text = Conversion.ToString((object)supcus.Title);
			txtcnicno.Text = Conversion.ToString((object)supcus.CNIC);
			if (!Conversion.CheckDateTimeNull(supcus.CNIC_EXPIRY_DATE))
			{
				txtcnicexp.Value = Conversion.ToDateTime((object)supcus.CNIC_EXPIRY_DATE);
			}
			txtaddress1.Text = Conversion.ToString((object)supcus.Address1);
			txtaddress2.Text = Conversion.ToString((object)supcus.Address2);
			txtemail.Text = Conversion.ToString((object)supcus.Email);
			txtphone.Text = Conversion.ToString((object)supcus.Phone);
			txtofficemobile.Text = Conversion.ToString((object)supcus.MobileOffice);
			txtwebsite.Text = Conversion.ToString((object)supcus.WebPage);
			txttown.Text = Conversion.ToString((object)supcus.Town);
			txtstrnno.Text = Conversion.ToString((object)supcus.STRN_No);
			txtntnno.Text = Conversion.ToString((object)supcus.NTN_No);
			txtzipcode.Text = Conversion.ToString((object)supcus.ZipCode);
			txtPartyCode.Text = supcus.PartyTypePrefix;
			cmbparentac.Value = Conversion.ToInt((object)supcus.ParentsSupCustId);
			CmbAdvanceAc.Value = Conversion.ToInt((object)supcus.AdvanceGlAcId);
			chksubac.Text = Conversion.ToString((object)supcus.IsSubSupCust);
			fileSavePath = supcus.PictureURL;
			if (fileSavePath != "")
			{
				if (File.Exists(fileSavePath))
				{
					box.Image = Image.FromFile(supcus.PictureURL);
					fileSavePath = supcus.PictureURL;
				}
				else
				{
					fileSavePath = "";
					box.Image = null;
					box.Invalidate();
				}
			}
			else
			{
				fileSavePath = "";
				box.Image = null;
				box.Invalidate();
			}
			cmbgroupdetail_Leave(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAdd_Click(object sender, EventArgs e)
	{
		btnsave_Click(null, null);
	}

	private void btnnew_Click(object sender, EventArgs e)
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

	private void FormReset()
	{
		try
		{
			RecId = 0;
			GlRecId = 0;
			((Control)(object)cmbPartyType).Enabled = true;
			txtaddress1.Clear();
			txtaddress2.Clear();
			txtcnicno.Clear();
			txtcompany.Clear();
			txtNickName.Clear();
			txtManualNo.Clear();
			txtemail.Clear();
			txtfirstname.Clear();
			txtlastname.Clear();
			txtntnno.Clear();
			txtofficemobile.Clear();
			txtpermobile.Clear();
			txtphone.Clear();
			txtWhatsAppNo.Clear();
			txtstrnno.Clear();
			txttitle.Clear();
			txttitle.Clear();
			txttown.Clear();
			txtwebsite.Clear();
			txtzipcode.Clear();
			((Control)(object)cmbglac).Text = string.Empty;
			((Control)(object)CmbBusinessType).Text = string.Empty;
			((Control)(object)cmbgroupdetail).Text = string.Empty;
			((Control)(object)cmbparentac).Text = string.Empty;
			((Control)(object)cmbprovince).Text = string.Empty;
			((Control)(object)CmbAdvanceAc).Text = string.Empty;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			box.Image = null;
			box.Invalidate();
			DiscountPolicyBind(DiscountPolicyDbCall());
			cmbPartyType.Focus();
			if (SubsidiaryAccountAllownOnVouchers)
			{
				if (Conversion.ToString(base.Tag) == "frmDefineSupplierOnly")
				{
					cmbPartyType.Value = 1;
					((Control)(object)cmbPartyType).Enabled = false;
				}
				else if (Conversion.ToString(base.Tag) == "frmDefineCustomerOnly")
				{
					cmbPartyType.Value = 2;
					((Control)(object)cmbPartyType).Enabled = false;
				}
			}
			ofd = string.Empty;
			fileSavePath = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void supfrmDefineSupplier_FormClosing(object sender, FormClosingEventArgs e)
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
			List<string> allServices = new List<string> { "SupplierCustomerLists", "VendorsAndCustomersForTransporters", "AllCities" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			PartyTypeBind(PartyTypeDbCall());
			SupplierGroupBind(SupplierGroupDBCall());
			DiscountPolicyBind(DiscountPolicyDbCall());
			CountryBind(CountryDtDbCall());
			CityDtFillFromGlobalAndBind();
			GetConfigurationsFromGlobalAndBindValuesInColumns();
			BindSupplierName(SupplierDtGetFromGlobal());
			BusinessTypeBind(global::Architecture.BLL.BusinessType.ComboBind(UserAccount.OrganizationId, UserAccount.CompanyId));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void definition()
	{
		try
		{
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Attachment Folder Path").ToList();
			if (list.Count > 0)
			{
				string value = list[0].ConfigKey.ToString();
				if (value != null)
				{
					saveDirectory = value;
					Configuration = true;
				}
			}
			else
			{
				MessageBox.Show("Please Map the Path in Configuration");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void browse_Click(object sender, EventArgs e)
	{
		try
		{
			definition();
			if (Configuration)
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = "(*.jpg; *.jpeg; *.PNG; *.png)| *.jpg; *.jpeg;  *.PNG;*.png; ";
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					ofd = openFileDialog.FileName;
					string fileName = Path.GetFileNameWithoutExtension(ofd);
					string exten = Path.GetExtension(ofd);
					string date = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
					fileName = fileName + date + exten;
					fileSavePath = Path.Combine(saveDirectory, fileName);
					box.Image = new Bitmap(openFileDialog.FileName);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Print(int PrintId)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				throw new Exception("Record Not Found For Display");
			}
			EnumerableRowCollection<DataRow> source = from r in dtGridRegisterFromDb.AsEnumerable()
				where r.Field<int>("Id") == PrintId
				select r;
			if (!source.Any())
			{
				throw new Exception("Record Not Found For Display");
			}
			DataTable dt = source.CopyToDataTable();
// 			Reporting val = new Reporting();
			// val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
			// val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
			// val.ShowReportWithDataTable(dt, "293-InvRptSupplierSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRegister_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtGridRegisterFromDb.Rows.Count == 0 || dtGridRegisterFromDb == null)
			{
				MessageBox.Show("Record Not Found For Display");
				return;
			}
// 			Reporting val = new Reporting();
			// val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			// val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			// val.ShowReportWithDataTable(dtGridRegisterFromDb, "292-InvRptSupplierRegister.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnBankDetail_Click(object sender, EventArgs e)
	{
// 		new SupfrmBankDetail(UserAccount).Show();
	}

	private void btnShipToAddress_Click(object sender, EventArgs e)
	{
// 		new SupfrmShipToAddress(UserAccount).Show();
	}

	private void btnSupplierLimits_Click(object sender, EventArgs e)
	{
// 		new SupfrmSupplierLimits(UserAccount).Show();
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		try
		{
// 			new SupplierCustomerMultiLanguage(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnAttributesAllocation_Click(object sender, EventArgs e)
	{
		try
		{
			MessageBox.Show("Attributes Allocation screen is under development.");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnBusinessType_Click(object sender, EventArgs e)
	{
		try
		{
// 			new DefineBusinessType(UserAccount).Show();
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
			if (FormInitialized)
			{
// 				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, DataGridHistory, dtGridRegister);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void supfrmDefineSupplier1_KeyDown(object sender, KeyEventArgs e)
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
				HistoryStack.Remove(this);
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.S && btnsave.Visible && btnsave.Enabled)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnupdate.Visible && btnupdate.Enabled)
				{
					btnupdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)DataGridHistory).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					txtcompany.Focus();
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					cmbPartyType.Focus();
				}
				return;
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				toolStripButton1_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshow_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)DataGridHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				CmbGlAccountRegister.Focus();
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				CmbPartyTypeRegister.Focus();
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				btnRegister_Click(null, null);
			}
			if ((e.Alt && e.KeyCode == Keys.D1) || (e.Alt && e.KeyCode == Keys.NumPad1))
			{
				btnRegister_Click(null, null);
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
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Date Type");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			if (tabControl1.SelectedIndex == 0)
			{
				dt.Rows.Add("Ctrl+S", "For Save");
				dt.Rows.Add("Ctrl+U", "For Update");
				dt.Rows.Add("Ctrl+F5", "For Focus on Party Type");
				dt.Rows.Add("Ctrl+ArrowUp", "For Focus On CompanyName");
				dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			}
			else
			{
				dt.Rows.Add("Ctrl+S", "For show data");
				dt.Rows.Add("Ctrl+F5", "For Focus on Party Type");
				dt.Rows.Add("Ctrl+ArrowUp", "For Focus on Gl Account");
				dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
				dt.Rows.Add("Ctrl+Space", "When Focus On Any Grid To Call Function's On Button Or Link");
			}
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShortCutKeysRegister_Click(object sender, EventArgs e)
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

	private DataTable GetComboData()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return global::Architecture.BLL.Inventory.SupplierCustomer.GetDataForDropDownFrom_SupplierCustomer(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AllComboBind(DataTable dt)
	{
		try
		{
			if (dt.Rows.Count > 0)
			{
				int PId = Conversion.ToInt(CmbPartyTypeRegister.Value);
				int GId = Conversion.ToInt(CmbPartyGroupRegister.Value);
				int CId = Conversion.ToInt(CmbCityRegister.Value);
				int AId = Conversion.ToInt(CmbGlAccountRegister.Value);
				DataTable dtPartyType = FilterData(dt, "PartyType");
				DataTable dtPartyGroup = FilterData(dt, "PartyGroup");
				DataTable dtCity = FilterData(dt, "CityName");
				DataTable dtGlAccount = FilterData(dt, "GlAccount");
				InfragisticsHelper.BindAndRetainSelection(CmbPartyTypeRegister, dtPartyType, "Id", "name", "Party Type", AllColumns: false, PId);
				InfragisticsHelper.BindAndRetainSelection(CmbPartyGroupRegister, dtPartyGroup, "Id", "name", "Party Group", AllColumns: false, GId, insertDefaultRow: false, null, null, ActivateRow: false);
				InfragisticsHelper.BindAndRetainSelection(CmbCityRegister, dtCity, "Id", "name", "City Name", AllColumns: false, CId, insertDefaultRow: false, null, null, ActivateRow: false);
				InfragisticsHelper.BindAndRetainSelection(CmbGlAccountRegister, dtGlAccount, "Id", "name", "Gl Account", AllColumns: false, AId, insertDefaultRow: false, null, null, ActivateRow: false);
			}
			else
			{
				((Control)(object)CmbPartyTypeRegister).Text = string.Empty;
				((UltraGridBase)CmbPartyTypeRegister).DataSource = null;
				((Control)(object)CmbPartyGroupRegister).Text = string.Empty;
				((UltraGridBase)CmbPartyGroupRegister).DataSource = null;
				((Control)(object)CmbCityRegister).Text = string.Empty;
				((UltraGridBase)CmbCityRegister).DataSource = null;
				((Control)(object)CmbGlAccountRegister).Text = string.Empty;
				((UltraGridBase)CmbGlAccountRegister).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private DataTable FilterData(DataTable source, string activityType)
	{
		EnumerableRowCollection<DataRow> filteredRows = from row in source.AsEnumerable()
			where row.Field<string>("Activity") == activityType
			select row;
		if (!filteredRows.Any())
		{
			return source.Clone();
		}
		return filteredRows.CopyToDataTable();
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			RegisterGridBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RegisterGridBind()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			dtGridRegisterFromDb.Clear();
			ReportsParameters sup = new ReportsParameters();
			sup.OrganizationId = UserAccount.OrganizationId;
			sup.CompanyId = UserAccount.CompanyId;
			sup.PartyTypeId = Conversion.ToInt(CmbPartyTypeRegister.Value);
			sup.AccountId = Conversion.ToInt(CmbGlAccountRegister.Value);
			sup.CustomerGroupId = Conversion.ToInt(CmbPartyGroupRegister.Value);
			sup.CityId = Conversion.ToInt(CmbCityRegister.Value);
			sup.IsTaxable = false;
			dtGridRegisterFromDb = GeneralReprots.SupplierCustomerRegister(sup);
			FillGrid(DataGridHistory, dtGridRegister, dtGridRegisterFromDb);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillGrid(GridEX grd, DataTable dtGrid, DataTable dtFromDB)
	{
		try
		{
			grd.DataSource = null;
			if (dtFromDB.Rows.Count <= 0)
			{
				return;
			}
			if (dtGrid.Columns.Count == 0)
			{
				dtGrid = PopulateDataTable();
			}
			dtGrid.Clear();
			foreach (DataRow dr in dtFromDB.Rows)
			{
// 				dtGrid.Rows.Add(dr["Id"], dr["PartyType"], dr["PartyTypePrefixNo"], dr["CompanyName"], dr["NickName"], dr["AccountCode"], dr["ManualPartyCode"], dr["AccountTitle"], dr["ReportingTitle"], dr["CustomerGroup"], dr["Address1"], dr["Address2"], dr["CNIC"], dr["Email"], dr["MobilePersonal"], dr["CompanyIndividuals"], dr["PolicyName"], dr["EntryUser"], dr["EntryDate"], dr["ModifyUser"], dr["ModifyDate"], dr["DescriptionCity"]);
			}
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			GridSetting(grd);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSetting(GridEX grd)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "ReportingTitle", "PolicyName" };
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 3, 2, hiddenColumns);
			GridEX_Helper.AddButton(grd, "Edit", "Edit", 40, 0);
			GridEX_Helper.AddButton(grd, "View", "View", 40, 0);
			grd.RootTable.Columns["View"].Position = 0;
			grd.RootTable.Columns["Edit"].Position = 1;
			grd.FrozenColumns = 2;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private DataTable PopulateDataTable()
	{
		return new DataTable
		{
			Columns = 
			{
				{
					"Id",
					typeof(int)
				},
				"PartyType",
				"PartyCode",
				"PartyName",
				"NickName",
				"AccountCode",
				"ManualNo",
				"LedgerAccount",
// 				"ReportingTitle",
				"PartyGroup",
				"Address",
				"Address1",
				"CNIC",
				"Email",
				"Phone",
				"CompanyIndividuals",
				"PolicyName",
				"EntryUser",
				{
					"EntryDate",
					typeof(DateTime)
				},
				"ModifyUser",
				"ModifyDate",
				"CityName"
			}
		};
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
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

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "View")
				{
					Print(CurrId);
				}
				if (e.Column.Key == "Edit")
				{
					ReadById(CurrId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefreshRegister_Click(object sender, EventArgs e)
	{
		try
		{
			AllComboBind(GetComboData());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnResetRegister_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)CmbPartyGroupRegister).Text = "";
			((Control)(object)CmbCityRegister).Text = "";
			((Control)(object)CmbGlAccountRegister).Text = "";
			((Control)(object)CmbBusinessType).Text = "";
			DataGridHistory.DataSource = null;
			dtGridRegister.Clear();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbparentac_TextChanged(object sender, EventArgs e)
	{
		try
		{
			int count = clsGlobalVariables.globalAllSupplierCustomer.Where((getGlobalAllSupplierCustomer r) => r.IsSubSupCust && r.ParentsSupCustId == Conversion.ToInt(cmbparentac.Value)).Count();
			txtSubPartyTotal.Text = count.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbCompanyIndividuals_SelectedValueChanged(object sender, EventArgs e)
	{
		if (CmbBusinessType.Value != null)
		{
			int value = Convert.ToInt32(CmbBusinessType.Value);
			LocNTN.Visible = value == 1;
			LocCNIC.Visible = value != 1;
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
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Expected O, but got Unknown
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Expected O, but got Unknown
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Expected O, but got Unknown
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Expected O, but got Unknown
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Expected O, but got Unknown
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Expected O, but got Unknown
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Expected O, but got Unknown
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Expected O, but got Unknown
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Expected O, but got Unknown
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Expected O, but got Unknown
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Expected O, but got Unknown
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Expected O, but got Unknown
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Expected O, but got Unknown
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Expected O, but got Unknown
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Expected O, but got Unknown
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Expected O, but got Unknown
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Expected O, but got Unknown
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Expected O, but got Unknown
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Expected O, but got Unknown
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Expected O, but got Unknown
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Expected O, but got Unknown
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Expected O, but got Unknown
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Expected O, but got Unknown
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Expected O, but got Unknown
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Expected O, but got Unknown
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Expected O, but got Unknown
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Expected O, but got Unknown
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Expected O, but got Unknown
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Expected O, but got Unknown
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Expected O, but got Unknown
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Expected O, but got Unknown
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Expected O, but got Unknown
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Expected O, but got Unknown
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Expected O, but got Unknown
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Expected O, but got Unknown
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Expected O, but got Unknown
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Expected O, but got Unknown
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Expected O, but got Unknown
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Expected O, but got Unknown
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Expected O, but got Unknown
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Expected O, but got Unknown
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Expected O, but got Unknown
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Expected O, but got Unknown
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Expected O, but got Unknown
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Expected O, but got Unknown
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Expected O, but got Unknown
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Expected O, but got Unknown
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Expected O, but got Unknown
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Expected O, but got Unknown
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Expected O, but got Unknown
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Expected O, but got Unknown
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Expected O, but got Unknown
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Expected O, but got Unknown
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Expected O, but got Unknown
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Expected O, but got Unknown
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Expected O, but got Unknown
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Expected O, but got Unknown
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Expected O, but got Unknown
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Expected O, but got Unknown
		//IL_040b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Expected O, but got Unknown
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Expected O, but got Unknown
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Expected O, but got Unknown
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Expected O, but got Unknown
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Expected O, but got Unknown
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Expected O, but got Unknown
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Expected O, but got Unknown
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Expected O, but got Unknown
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Expected O, but got Unknown
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Expected O, but got Unknown
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Expected O, but got Unknown
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Expected O, but got Unknown
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Expected O, but got Unknown
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Expected O, but got Unknown
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Expected O, but got Unknown
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Expected O, but got Unknown
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Expected O, but got Unknown
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Expected O, but got Unknown
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Expected O, but got Unknown
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Expected O, but got Unknown
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Expected O, but got Unknown
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Expected O, but got Unknown
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Expected O, but got Unknown
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Expected O, but got Unknown
		//IL_0505: Unknown result type (might be due to invalid IL or missing references)
		//IL_050f: Expected O, but got Unknown
		//IL_051b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0525: Expected O, but got Unknown
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		//IL_053b: Expected O, but got Unknown
		//IL_0552: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Expected O, but got Unknown
		//IL_055d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0567: Expected O, but got Unknown
		//IL_05e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Expected O, but got Unknown
		//IL_0720: Unknown result type (might be due to invalid IL or missing references)
		//IL_072a: Expected O, but got Unknown
		//IL_0736: Unknown result type (might be due to invalid IL or missing references)
		//IL_0740: Expected O, but got Unknown
		//IL_074c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0756: Expected O, but got Unknown
		//IL_0925: Unknown result type (might be due to invalid IL or missing references)
		//IL_092f: Expected O, but got Unknown
		//IL_0972: Unknown result type (might be due to invalid IL or missing references)
		//IL_097c: Expected O, but got Unknown
		//IL_0988: Unknown result type (might be due to invalid IL or missing references)
		//IL_0992: Expected O, but got Unknown
		//IL_099e: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a8: Expected O, but got Unknown
		//IL_09b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09be: Expected O, but got Unknown
		//IL_7195: Unknown result type (might be due to invalid IL or missing references)
		//IL_719f: Expected O, but got Unknown
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance124 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Supplier_Purchase.supfrmDefineSupplier));
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
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
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.BtnBusinessType = new System.Windows.Forms.Button();
		this.txtaddress1 = new System.Windows.Forms.TextBox();
		this.CmbBusinessType = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.txtSubPartyTotal = new System.Windows.Forms.TextBox();
		this.txtManualNo = new System.Windows.Forms.TextBox();
		this.CmbAdvanceAc = new UltraCombo();
		this.txtcnicno = new System.Windows.Forms.MaskedTextBox();
		this.cmbparentac = new UltraCombo();
		this.txtPartyCode = new System.Windows.Forms.TextBox();
		this.cmbPartyType = new UltraCombo();
		this.txtstrnno = new System.Windows.Forms.TextBox();
		this.txtntnno = new System.Windows.Forms.TextBox();
		this.cmbcity = new UltraCombo();
		this.cmbglac = new UltraCombo();
		this.txtcnicexp = new System.Windows.Forms.DateTimePicker();
		this.txtNickName = new System.Windows.Forms.TextBox();
		this.txtcompany = new System.Windows.Forms.TextBox();
		this.txtaddress2 = new System.Windows.Forms.TextBox();
		this.label43 = new System.Windows.Forms.Label();
		this.txtfirstname = new System.Windows.Forms.TextBox();
		this.txtwebsite = new System.Windows.Forms.TextBox();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.btnAdd = new System.Windows.Forms.Button();
		this.box = new System.Windows.Forms.PictureBox();
		this.browse = new System.Windows.Forms.Button();
		this.cmbDiscountType = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.txttown = new System.Windows.Forms.TextBox();
		this.txtzipcode = new System.Windows.Forms.TextBox();
		this.label32 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.txtemail = new System.Windows.Forms.TextBox();
		this.label51 = new System.Windows.Forms.Label();
		this.txtWhatsAppNo = new System.Windows.Forms.TextBox();
		this.label34 = new System.Windows.Forms.Label();
		this.txtphone = new System.Windows.Forms.MaskedTextBox();
		this.label40 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.txtofficemobile = new System.Windows.Forms.TextBox();
		this.label48 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtlastname = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.txttitle = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.label55 = new System.Windows.Forms.Label();
		this.txtpermobile = new System.Windows.Forms.TextBox();
		this.label49 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.cmbgroupdetail = new UltraCombo();
		this.label54 = new System.Windows.Forms.Label();
		this.cmbcountry = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.cmbprovince = new UltraCombo();
		this.label28 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label58 = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.LocCNIC = new System.Windows.Forms.Label();
		this.LocNTN = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.chkactivestatus = new System.Windows.Forms.CheckBox();
		this.label12 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.chksubac = new System.Windows.Forms.CheckBox();
		this.label53 = new System.Windows.Forms.Label();
		this.label56 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label20 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnBankDetail = new System.Windows.Forms.ToolStripButton();
		this.btnShipToAddress = new System.Windows.Forms.ToolStripButton();
		this.btnSupplierLimits = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.BtnAttributesAllocation = new System.Windows.Forms.ToolStripButton();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.DataGridHistory = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label22 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.btnshow = new System.Windows.Forms.Button();
		this.CmbPartyGroupRegister = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.CmbCityRegister = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.CmbPartyTypeRegister = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.CmbGlAccountRegister = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.txtInvoiceNo = new System.Windows.Forms.TextBox();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.BtnResetRegister = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshRegister = new System.Windows.Forms.ToolStripButton();
		this.btnRegister = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutKeysRegister = new System.Windows.Forms.ToolStripButton();
		this.label17 = new System.Windows.Forms.Label();
		this.panel4.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBusinessType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAdvanceAc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbparentac).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPartyType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbglac).BeginInit();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.box).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDiscountType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbgroupdetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcountry).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbprovince).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel9.SuspendLayout();
		this.panel1.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPartyGroupRegister).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCityRegister).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPartyTypeRegister).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGlAccountRegister).BeginInit();
		this.panel8.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add(this.panel3);
		this.panel4.Controls.Add(this.panel2);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(3, 3);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1020, 714);
		this.panel4.TabIndex = 1;
		this.panel6.Controls.Add(this.BtnBusinessType);
		this.panel6.Controls.Add(this.txtaddress1);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBusinessType);
		this.panel6.Controls.Add(this.label11);
		this.panel6.Controls.Add(this.txtSubPartyTotal);
		this.panel6.Controls.Add(this.txtManualNo);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAdvanceAc);
		this.panel6.Controls.Add(this.txtcnicno);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.cmbparentac);
		this.panel6.Controls.Add(this.txtPartyCode);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPartyType);
		this.panel6.Controls.Add(this.txtstrnno);
		this.panel6.Controls.Add(this.txtntnno);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcity);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.cmbglac);
		this.panel6.Controls.Add(this.txtcnicexp);
		this.panel6.Controls.Add(this.txtNickName);
		this.panel6.Controls.Add(this.txtcompany);
		this.panel6.Controls.Add(this.txtaddress2);
		this.panel6.Controls.Add(this.label43);
		this.panel6.Controls.Add(this.txtfirstname);
		this.panel6.Controls.Add(this.txtwebsite);
		this.panel6.Controls.Add(this.groupBox2);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDiscountType);
		this.panel6.Controls.Add(this.label9);
		this.panel6.Controls.Add(this.label47);
		this.panel6.Controls.Add(this.label44);
		this.panel6.Controls.Add(this.label10);
		this.panel6.Controls.Add(this.label37);
		this.panel6.Controls.Add(this.txttown);
		this.panel6.Controls.Add(this.txtzipcode);
		this.panel6.Controls.Add(this.label32);
		this.panel6.Controls.Add(this.label52);
		this.panel6.Controls.Add(this.txtemail);
		this.panel6.Controls.Add(this.label51);
		this.panel6.Controls.Add(this.txtWhatsAppNo);
		this.panel6.Controls.Add(this.label34);
		this.panel6.Controls.Add(this.txtphone);
		this.panel6.Controls.Add(this.label40);
		this.panel6.Controls.Add(this.label6);
		this.panel6.Controls.Add(this.label41);
		this.panel6.Controls.Add(this.txtofficemobile);
		this.panel6.Controls.Add(this.label48);
		this.panel6.Controls.Add(this.label1);
		this.panel6.Controls.Add(this.txtlastname);
		this.panel6.Controls.Add(this.label15);
		this.panel6.Controls.Add(this.txttitle);
		this.panel6.Controls.Add(this.label13);
		this.panel6.Controls.Add(this.label55);
		this.panel6.Controls.Add(this.txtpermobile);
		this.panel6.Controls.Add(this.label49);
		this.panel6.Controls.Add(this.label35);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.cmbgroupdetail);
		this.panel6.Controls.Add(this.label54);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcountry);
		this.panel6.Controls.Add(this.label8);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.cmbprovince);
		this.panel6.Controls.Add(this.label28);
		this.panel6.Controls.Add(this.label33);
		this.panel6.Controls.Add(this.label30);
		this.panel6.Controls.Add(this.label58);
		this.panel6.Controls.Add(this.label50);
		this.panel6.Controls.Add(this.label29);
		this.panel6.Controls.Add(this.LocCNIC);
		this.panel6.Controls.Add(this.LocNTN);
		this.panel6.Controls.Add(this.label25);
		this.panel6.Controls.Add(this.label24);
		this.panel6.Controls.Add(this.label19);
		this.panel6.Controls.Add(this.label18);
		this.panel6.Controls.Add(this.label17);
		this.panel6.Controls.Add(this.label16);
		this.panel6.Controls.Add(this.label4);
		this.panel6.Controls.Add(this.chkactivestatus);
		this.panel6.Controls.Add(this.label12);
		this.panel6.Controls.Add(this.label57);
		this.panel6.Controls.Add(this.label31);
		this.panel6.Controls.Add(this.label36);
		this.panel6.Controls.Add(this.chksubac);
		this.panel6.Controls.Add(this.label53);
		this.panel6.Controls.Add(this.label56);
		this.panel6.Controls.Add(this.label38);
		this.panel6.Controls.Add(this.label39);
		this.panel6.Controls.Add(this.label46);
		this.panel6.Controls.Add(this.label42);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Font = new System.Drawing.Font("Segoe UI", 9f);
		this.panel6.Location = new System.Drawing.Point(0, 53);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1020, 452);
		this.panel6.TabIndex = 0;
		this.BtnBusinessType.BackColor = System.Drawing.Color.Transparent;
		this.BtnBusinessType.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnBusinessType.ForeColor = System.Drawing.Color.White;
		this.BtnBusinessType.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.BtnBusinessType.Location = new System.Drawing.Point(357, 268);
		this.BtnBusinessType.Name = "BtnBusinessType";
		this.BtnBusinessType.Size = new System.Drawing.Size(33, 26);
		this.BtnBusinessType.TabIndex = 12;
		this.BtnBusinessType.UseVisualStyleBackColor = false;
		this.BtnBusinessType.Visible = false;
		this.BtnBusinessType.Click += new System.EventHandler(BtnBusinessType_Click);
		this.txtaddress1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtaddress1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtaddress1.Location = new System.Drawing.Point(172, 401);
		this.txtaddress1.Name = "txtaddress1";
		this.txtaddress1.Size = new System.Drawing.Size(519, 23);
		this.txtaddress1.TabIndex = 17;
		this.CmbBusinessType.AllowNull = (DefaultableBoolean)1;
		this.CmbBusinessType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBusinessType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBusinessType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBusinessType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbBusinessType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBusinessType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBusinessType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBusinessType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance21;
		((SpecialBoxBase)((UltraGridBase)this.CmbBusinessType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBusinessType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbBusinessType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBusinessType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBusinessType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBusinessType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbBusinessType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBusinessType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBusinessType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance27).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBusinessType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbBusinessType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBusinessType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBusinessType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBusinessType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbBusinessType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBusinessType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBusinessType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbBusinessType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBusinessType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbBusinessType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBusinessType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBusinessType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBusinessType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBusinessType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBusinessType).Location = new System.Drawing.Point(172, 268);
		((System.Windows.Forms.Control)(object)this.CmbBusinessType).Name = "CmbBusinessType";
		((System.Windows.Forms.Control)(object)this.CmbBusinessType).Size = new System.Drawing.Size(183, 26);
		((System.Windows.Forms.Control)(object)this.CmbBusinessType).TabIndex = 11;
		((System.Windows.Forms.Control)(object)this.CmbBusinessType).TextChanged += new System.EventHandler(cmbCompanyIndividuals_SelectedValueChanged);
		this.label11.AutoSize = true;
		this.label11.BackColor = System.Drawing.Color.White;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(235, 219);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(95, 15);
		this.label11.TabIndex = 231;
		this.label11.Text = "Total Sub Parties";
		this.txtSubPartyTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSubPartyTotal.Enabled = false;
		this.txtSubPartyTotal.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSubPartyTotal.Location = new System.Drawing.Point(332, 215);
		this.txtSubPartyTotal.Name = "txtSubPartyTotal";
		this.txtSubPartyTotal.Size = new System.Drawing.Size(58, 23);
		this.txtSubPartyTotal.TabIndex = 9;
		this.txtManualNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtManualNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtManualNo.Location = new System.Drawing.Point(172, 56);
		this.txtManualNo.Name = "txtManualNo";
		this.txtManualNo.Size = new System.Drawing.Size(218, 23);
		this.txtManualNo.TabIndex = 2;
		this.CmbAdvanceAc.AllowNull = (DefaultableBoolean)1;
		this.CmbAdvanceAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAdvanceAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAdvanceAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance12).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAdvanceAc).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAdvanceAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAdvanceAc).Location = new System.Drawing.Point(172, 159);
		((System.Windows.Forms.Control)(object)this.CmbAdvanceAc).Name = "CmbAdvanceAc";
		((System.Windows.Forms.Control)(object)this.CmbAdvanceAc).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.CmbAdvanceAc).TabIndex = 6;
		this.txtcnicno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcnicno.Location = new System.Drawing.Point(172, 325);
		this.txtcnicno.Margin = new System.Windows.Forms.Padding(2);
		this.txtcnicno.Mask = "00000-0000000-0";
		this.txtcnicno.Name = "txtcnicno";
		this.txtcnicno.Size = new System.Drawing.Size(218, 23);
		this.txtcnicno.TabIndex = 14;
		this.cmbparentac.AllowNull = (DefaultableBoolean)1;
		this.cmbparentac.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbparentac.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbparentac.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Appearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.cmbparentac).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbparentac).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance139).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance139).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbparentac).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbparentac).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance140;
		((SpecialBoxBase)((UltraGridBase)this.cmbparentac).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbparentac).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.cmbparentac).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbparentac).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance146).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.cmbparentac).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbparentac).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbparentac).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbparentac).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbparentac.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbparentac).Location = new System.Drawing.Point(172, 240);
		((System.Windows.Forms.Control)(object)this.cmbparentac).Name = "cmbparentac";
		((System.Windows.Forms.Control)(object)this.cmbparentac).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.cmbparentac).TabIndex = 10;
		((System.Windows.Forms.Control)(object)this.cmbparentac).TextChanged += new System.EventHandler(cmbparentac_TextChanged);
		this.txtPartyCode.Enabled = false;
		this.txtPartyCode.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPartyCode.Location = new System.Drawing.Point(172, 31);
		this.txtPartyCode.Name = "txtPartyCode";
		this.txtPartyCode.Size = new System.Drawing.Size(218, 23);
		this.txtPartyCode.TabIndex = 1;
		this.cmbPartyType.AllowNull = (DefaultableBoolean)1;
		this.cmbPartyType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPartyType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPartyType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPartyType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance34;
		((SpecialBoxBase)((UltraGridBase)this.cmbPartyType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance40).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPartyType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPartyType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPartyType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPartyType).Location = new System.Drawing.Point(172, 3);
		((System.Windows.Forms.Control)(object)this.cmbPartyType).Name = "cmbPartyType";
		((System.Windows.Forms.Control)(object)this.cmbPartyType).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.cmbPartyType).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbPartyType).Leave += new System.EventHandler(cmbPartyType_Leave);
		this.txtstrnno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtstrnno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtstrnno.Location = new System.Drawing.Point(172, 376);
		this.txtstrnno.Name = "txtstrnno";
		this.txtstrnno.Size = new System.Drawing.Size(218, 23);
		this.txtstrnno.TabIndex = 16;
		this.txtntnno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtntnno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtntnno.Location = new System.Drawing.Point(172, 296);
		this.txtntnno.Name = "txtntnno";
		this.txtntnno.Size = new System.Drawing.Size(218, 23);
		this.txtntnno.TabIndex = 13;
		this.cmbcity.AllowNull = (DefaultableBoolean)1;
		this.cmbcity.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbcity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance71;
		((SpecialBoxBase)((UltraGridBase)this.cmbcity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbcity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance76).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance77).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance78).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbcity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcity).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcity).Location = new System.Drawing.Point(503, 323);
		((System.Windows.Forms.Control)(object)this.cmbcity).Name = "cmbcity";
		((System.Windows.Forms.Control)(object)this.cmbcity).Size = new System.Drawing.Size(188, 26);
		((System.Windows.Forms.Control)(object)this.cmbcity).TabIndex = 31;
		this.cmbglac.AllowNull = (DefaultableBoolean)1;
		this.cmbglac.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbglac.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbglac.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbglac).DisplayLayout.Appearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbglac).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbglac).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbglac).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbglac).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance85;
		((SpecialBoxBase)((UltraGridBase)this.cmbglac).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance86).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance86).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbglac).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbglac).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbglac).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance90).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance92).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbglac).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbglac).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbglac).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbglac).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbglac.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbglac).Location = new System.Drawing.Point(172, 131);
		((System.Windows.Forms.Control)(object)this.cmbglac).Name = "cmbglac";
		((System.Windows.Forms.Control)(object)this.cmbglac).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.cmbglac).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.cmbglac).Leave += new System.EventHandler(cmbglac_Leave);
		this.txtcnicexp.CustomFormat = "dd-MM-yy";
		this.txtcnicexp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcnicexp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtcnicexp.Location = new System.Drawing.Point(172, 351);
		this.txtcnicexp.Name = "txtcnicexp";
		this.txtcnicexp.Size = new System.Drawing.Size(218, 23);
		this.txtcnicexp.TabIndex = 15;
		this.txtNickName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNickName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNickName.Location = new System.Drawing.Point(172, 106);
		this.txtNickName.Name = "txtNickName";
		this.txtNickName.Size = new System.Drawing.Size(218, 23);
		this.txtNickName.TabIndex = 4;
		this.txtcompany.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcompany.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcompany.Location = new System.Drawing.Point(172, 81);
		this.txtcompany.Name = "txtcompany";
		this.txtcompany.Size = new System.Drawing.Size(218, 23);
		this.txtcompany.TabIndex = 3;
		this.txtaddress2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtaddress2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtaddress2.Location = new System.Drawing.Point(172, 426);
		this.txtaddress2.Name = "txtaddress2";
		this.txtaddress2.Size = new System.Drawing.Size(519, 23);
		this.txtaddress2.TabIndex = 18;
		this.txtaddress2.TabStop = false;
		this.txtaddress2.Visible = false;
		this.label43.AutoSize = true;
		this.label43.BackColor = System.Drawing.Color.White;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(397, 246);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(74, 15);
		this.label43.TabIndex = 214;
		this.label43.Text = "Website:URL";
		this.txtfirstname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfirstname.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtfirstname.Location = new System.Drawing.Point(503, 56);
		this.txtfirstname.Name = "txtfirstname";
		this.txtfirstname.Size = new System.Drawing.Size(188, 23);
		this.txtfirstname.TabIndex = 21;
		this.txtwebsite.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtwebsite.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtwebsite.Location = new System.Drawing.Point(503, 242);
		this.txtwebsite.Name = "txtwebsite";
		this.txtwebsite.Size = new System.Drawing.Size(188, 23);
		this.txtwebsite.TabIndex = 28;
		this.groupBox2.Controls.Add(this.btnAdd);
		this.groupBox2.Controls.Add(this.box);
		this.groupBox2.Controls.Add(this.browse);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(696, 5);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(324, 215);
		this.groupBox2.TabIndex = 33;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Party - Profile Picture";
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(234, 181);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(61, 25);
		this.btnAdd.TabIndex = 1;
		this.btnAdd.Text = "Save";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Visible = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.box.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.box.Location = new System.Drawing.Point(3, 16);
		this.box.Name = "box";
		this.box.Size = new System.Drawing.Size(319, 162);
		this.box.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.box.TabIndex = 215;
		this.box.TabStop = false;
		this.browse.BackColor = System.Drawing.Color.Teal;
		this.browse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.browse.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.browse.ForeColor = System.Drawing.Color.White;
		this.browse.Location = new System.Drawing.Point(105, 180);
		this.browse.Name = "browse";
		this.browse.Size = new System.Drawing.Size(123, 26);
		this.browse.TabIndex = 0;
		this.browse.Text = "Profile Picture";
		this.browse.UseVisualStyleBackColor = false;
		this.browse.Click += new System.EventHandler(browse_Click);
		this.cmbDiscountType.AllowNull = (DefaultableBoolean)1;
		this.cmbDiscountType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDiscountType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDiscountType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDiscountType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance48;
		((SpecialBoxBase)((UltraGridBase)this.cmbDiscountType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance53).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDiscountType).Enabled = false;
		((System.Windows.Forms.Control)(object)this.cmbDiscountType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbDiscountType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDiscountType).Location = new System.Drawing.Point(503, 3);
		((System.Windows.Forms.Control)(object)this.cmbDiscountType).Name = "cmbDiscountType";
		((System.Windows.Forms.Control)(object)this.cmbDiscountType).Size = new System.Drawing.Size(188, 26);
		((System.Windows.Forms.Control)(object)this.cmbDiscountType).TabIndex = 19;
		this.label9.AutoSize = true;
		this.label9.BackColor = System.Drawing.Color.White;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(397, 9);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(83, 15);
		this.label9.TabIndex = 219;
		this.label9.Text = "Discount Type";
		this.label47.AutoSize = true;
		this.label47.BackColor = System.Drawing.Color.White;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(9, 405);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(49, 15);
		this.label47.TabIndex = 205;
		this.label47.Text = "Address";
		this.label44.AutoSize = true;
		this.label44.BackColor = System.Drawing.Color.White;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(397, 110);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(41, 15);
		this.label44.TabIndex = 209;
		this.label44.Text = "Phone";
		this.label10.AutoSize = true;
		this.label10.BackColor = System.Drawing.Color.White;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(9, 274);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(139, 15);
		this.label10.TabIndex = 230;
		this.label10.Text = "Is Company / Individuals";
		this.label37.AutoSize = true;
		this.label37.BackColor = System.Drawing.Color.White;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(397, 137);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(86, 15);
		this.label37.TabIndex = 210;
		this.label37.Text = "Office Mobile#";
		this.txttown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txttown.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txttown.Location = new System.Drawing.Point(503, 351);
		this.txttown.Name = "txttown";
		this.txttown.Size = new System.Drawing.Size(188, 23);
		this.txttown.TabIndex = 32;
		this.txtzipcode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtzipcode.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtzipcode.Location = new System.Drawing.Point(503, 376);
		this.txtzipcode.Name = "txtzipcode";
		this.txtzipcode.Size = new System.Drawing.Size(111, 23);
		this.txtzipcode.TabIndex = 33;
		this.label32.AutoSize = true;
		this.label32.BackColor = System.Drawing.Color.White;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(397, 355);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(36, 15);
		this.label32.TabIndex = 207;
		this.label32.Text = "Town";
		this.label52.AutoSize = true;
		this.label52.BackColor = System.Drawing.Color.White;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(397, 380);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(54, 15);
		this.label52.TabIndex = 208;
		this.label52.Text = "Zip Code";
		this.txtemail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtemail.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtemail.Location = new System.Drawing.Point(503, 215);
		this.txtemail.Name = "txtemail";
		this.txtemail.Size = new System.Drawing.Size(188, 23);
		this.txtemail.TabIndex = 27;
		this.label51.AutoSize = true;
		this.label51.BackColor = System.Drawing.Color.White;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(397, 219);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(39, 15);
		this.label51.TabIndex = 211;
		this.label51.Text = "Email ";
		this.txtWhatsAppNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtWhatsAppNo.Location = new System.Drawing.Point(503, 189);
		this.txtWhatsAppNo.Name = "txtWhatsAppNo";
		this.txtWhatsAppNo.Size = new System.Drawing.Size(188, 23);
		this.txtWhatsAppNo.TabIndex = 26;
		this.label34.AutoSize = true;
		this.label34.BackColor = System.Drawing.Color.White;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(9, 329);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(81, 15);
		this.label34.TabIndex = 203;
		this.label34.Text = "CNIC Number";
		this.txtphone.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtphone.Location = new System.Drawing.Point(503, 106);
		this.txtphone.Mask = "(999) 000-0000";
		this.txtphone.Name = "txtphone";
		this.txtphone.Size = new System.Drawing.Size(188, 23);
		this.txtphone.TabIndex = 23;
		this.label40.AutoSize = true;
		this.label40.BackColor = System.Drawing.Color.White;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(397, 85);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(63, 15);
		this.label40.TabIndex = 202;
		this.label40.Text = "Last Name";
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.White;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(397, 193);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(73, 15);
		this.label6.TabIndex = 227;
		this.label6.Text = "Whats App#";
		this.label41.AutoSize = true;
		this.label41.BackColor = System.Drawing.Color.White;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(9, 300);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(42, 15);
		this.label41.TabIndex = 199;
		this.label41.Text = "NTN #";
		this.txtofficemobile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtofficemobile.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtofficemobile.Location = new System.Drawing.Point(503, 133);
		this.txtofficemobile.Name = "txtofficemobile";
		this.txtofficemobile.Size = new System.Drawing.Size(188, 23);
		this.txtofficemobile.TabIndex = 24;
		this.label48.AutoSize = true;
		this.label48.BackColor = System.Drawing.Color.White;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(397, 60);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(64, 15);
		this.label48.TabIndex = 201;
		this.label48.Text = "First Name";
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.White;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(9, 60);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(107, 15);
		this.label1.TabIndex = 219;
		this.label1.Text = "Manual Party Code";
		this.txtlastname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtlastname.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtlastname.Location = new System.Drawing.Point(503, 81);
		this.txtlastname.Name = "txtlastname";
		this.txtlastname.Size = new System.Drawing.Size(188, 23);
		this.txtlastname.TabIndex = 22;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.White;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(9, 35);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(64, 15);
		this.label15.TabIndex = 225;
		this.label15.Text = "Party Code";
		this.txttitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txttitle.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txttitle.Location = new System.Drawing.Point(503, 31);
		this.txttitle.Name = "txttitle";
		this.txttitle.Size = new System.Drawing.Size(188, 23);
		this.txttitle.TabIndex = 20;
		this.label13.AutoSize = true;
		this.label13.BackColor = System.Drawing.Color.White;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(9, 9);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(62, 15);
		this.label13.TabIndex = 223;
		this.label13.Text = "Party Type";
		this.label55.AutoSize = true;
		this.label55.BackColor = System.Drawing.Color.White;
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.Location = new System.Drawing.Point(397, 35);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(84, 15);
		this.label55.TabIndex = 213;
		this.label55.Text = "Title Mr. / Mrs.";
		this.txtpermobile.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtpermobile.Location = new System.Drawing.Point(503, 161);
		this.txtpermobile.Name = "txtpermobile";
		this.txtpermobile.Size = new System.Drawing.Size(188, 23);
		this.txtpermobile.TabIndex = 25;
		this.label49.AutoSize = true;
		this.label49.BackColor = System.Drawing.Color.White;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(9, 246);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(106, 15);
		this.label49.TabIndex = 198;
		this.label49.Text = "Parent Party Name";
		this.label35.AutoSize = true;
		this.label35.BackColor = System.Drawing.Color.White;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(9, 380);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(47, 15);
		this.label35.TabIndex = 200;
		this.label35.Text = "STRN #";
		this.cmbgroupdetail.AllowNull = (DefaultableBoolean)1;
		this.cmbgroupdetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbgroupdetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbgroupdetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbgroupdetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance151;
		((SpecialBoxBase)((UltraGridBase)this.cmbgroupdetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance63).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbgroupdetail).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbgroupdetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbgroupdetail).Location = new System.Drawing.Point(172, 187);
		((System.Windows.Forms.Control)(object)this.cmbgroupdetail).Name = "cmbgroupdetail";
		((System.Windows.Forms.Control)(object)this.cmbgroupdetail).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.cmbgroupdetail).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.cmbgroupdetail).Leave += new System.EventHandler(cmbgroupdetail_Leave);
		this.label54.AutoSize = true;
		this.label54.BackColor = System.Drawing.Color.White;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(9, 355);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(69, 15);
		this.label54.TabIndex = 204;
		this.label54.Text = "CNIC Expiry";
		this.cmbcountry.AllowNull = (DefaultableBoolean)1;
		this.cmbcountry.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcountry.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcountry.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Appearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.cmbcountry).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcountry).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance155).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance155).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcountry).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcountry).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance156;
		((SpecialBoxBase)((UltraGridBase)this.cmbcountry).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance157).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance157).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance157).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcountry).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.cmbcountry).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcountry).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance158).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance159).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance161).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance162).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance162;
		((AppearanceBase)appearance164).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.cmbcountry).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcountry).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcountry).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcountry).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbcountry.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcountry).Location = new System.Drawing.Point(503, 268);
		((System.Windows.Forms.Control)(object)this.cmbcountry).Name = "cmbcountry";
		((System.Windows.Forms.Control)(object)this.cmbcountry).Size = new System.Drawing.Size(188, 26);
		((System.Windows.Forms.Control)(object)this.cmbcountry).TabIndex = 29;
		this.label8.AutoSize = true;
		this.label8.BackColor = System.Drawing.Color.White;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(9, 165);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(101, 15);
		this.label8.TabIndex = 217;
		this.label8.Text = "Advance Account";
		this.cmbprovince.AllowNull = (DefaultableBoolean)2;
		this.cmbprovince.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbprovince.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbprovince.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Appearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbprovince).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbprovince).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbprovince).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbprovince).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance24;
		((SpecialBoxBase)((UltraGridBase)this.cmbprovince).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbprovince).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbprovince).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbprovince).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance98).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance80).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance91).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmbprovince).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbprovince).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbprovince).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbprovince).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbprovince.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbprovince).Location = new System.Drawing.Point(503, 295);
		((System.Windows.Forms.Control)(object)this.cmbprovince).Name = "cmbprovince";
		((System.Windows.Forms.Control)(object)this.cmbprovince).Size = new System.Drawing.Size(188, 26);
		((System.Windows.Forms.Control)(object)this.cmbprovince).TabIndex = 30;
		((System.Windows.Forms.Control)(object)this.cmbprovince).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbprovince).Visible = false;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.ForeColor = System.Drawing.Color.Red;
		this.label28.Location = new System.Drawing.Point(157, 192);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(13, 16);
		this.label28.TabIndex = 171;
		this.label28.Text = "*";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.ForeColor = System.Drawing.Color.Red;
		this.label33.Location = new System.Drawing.Point(157, 404);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(13, 16);
		this.label33.TabIndex = 171;
		this.label33.Text = "*";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.ForeColor = System.Drawing.Color.Red;
		this.label30.Location = new System.Drawing.Point(484, 164);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(13, 16);
		this.label30.TabIndex = 171;
		this.label30.Text = "*";
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.ForeColor = System.Drawing.Color.Red;
		this.label58.Location = new System.Drawing.Point(484, 273);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(13, 16);
		this.label58.TabIndex = 171;
		this.label58.Text = "*";
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.ForeColor = System.Drawing.Color.Red;
		this.label50.Location = new System.Drawing.Point(484, 192);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(13, 16);
		this.label50.TabIndex = 171;
		this.label50.Text = "*";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.ForeColor = System.Drawing.Color.Red;
		this.label29.Location = new System.Drawing.Point(484, 328);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(13, 16);
		this.label29.TabIndex = 171;
		this.label29.Text = "*";
		this.LocCNIC.AutoSize = true;
		this.LocCNIC.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.LocCNIC.ForeColor = System.Drawing.Color.Red;
		this.LocCNIC.Location = new System.Drawing.Point(157, 328);
		this.LocCNIC.Name = "LocCNIC";
		this.LocCNIC.Size = new System.Drawing.Size(13, 16);
		this.LocCNIC.TabIndex = 171;
		this.LocCNIC.Text = "*";
		this.LocNTN.AutoSize = true;
		this.LocNTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.LocNTN.ForeColor = System.Drawing.Color.Red;
		this.LocNTN.Location = new System.Drawing.Point(157, 299);
		this.LocNTN.Name = "LocNTN";
		this.LocNTN.Size = new System.Drawing.Size(13, 16);
		this.LocNTN.TabIndex = 171;
		this.LocNTN.Text = "*";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.ForeColor = System.Drawing.Color.Red;
		this.label25.Location = new System.Drawing.Point(157, 273);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(13, 16);
		this.label25.TabIndex = 171;
		this.label25.Text = "*";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.Color.Red;
		this.label24.Location = new System.Drawing.Point(157, 136);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(13, 16);
		this.label24.TabIndex = 171;
		this.label24.Text = "*";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.Red;
		this.label19.Location = new System.Drawing.Point(157, 109);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(13, 16);
		this.label19.TabIndex = 171;
		this.label19.Text = "*";
		this.label19.Visible = false;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.Color.Red;
		this.label18.Location = new System.Drawing.Point(157, 84);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(13, 16);
		this.label18.TabIndex = 171;
		this.label18.Text = "*";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.Red;
		this.label16.Location = new System.Drawing.Point(157, 34);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(13, 16);
		this.label16.TabIndex = 171;
		this.label16.Text = "*";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.Red;
		this.label4.Location = new System.Drawing.Point(157, 8);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(13, 16);
		this.label4.TabIndex = 171;
		this.label4.Text = "*";
		this.chkactivestatus.AutoSize = true;
		this.chkactivestatus.Checked = true;
		this.chkactivestatus.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkactivestatus.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkactivestatus.ForeColor = System.Drawing.Color.Black;
		this.chkactivestatus.Location = new System.Drawing.Point(620, 378);
		this.chkactivestatus.Name = "chkactivestatus";
		this.chkactivestatus.Size = new System.Drawing.Size(71, 19);
		this.chkactivestatus.TabIndex = 34;
		this.chkactivestatus.Text = "Is Active";
		this.chkactivestatus.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
		this.chkactivestatus.UseVisualStyleBackColor = true;
		this.label12.AutoSize = true;
		this.label12.BackColor = System.Drawing.Color.White;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(9, 110);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(102, 15);
		this.label12.TabIndex = 154;
		this.label12.Text = "Alias / Nick Name";
		this.label57.AutoSize = true;
		this.label57.BackColor = System.Drawing.Color.White;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.Location = new System.Drawing.Point(9, 85);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(149, 15);
		this.label57.TabIndex = 154;
		this.label57.Text = "Company / Buisness Name";
		this.label31.AutoSize = true;
		this.label31.BackColor = System.Drawing.Color.White;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(397, 165);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(51, 15);
		this.label31.TabIndex = 179;
		this.label31.Text = "Mobile#";
		this.label36.AutoSize = true;
		this.label36.BackColor = System.Drawing.Color.White;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(9, 193);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(70, 15);
		this.label36.TabIndex = 192;
		this.label36.Text = "Party Group";
		this.chksubac.AutoSize = true;
		this.chksubac.ForeColor = System.Drawing.Color.White;
		this.chksubac.Location = new System.Drawing.Point(172, 217);
		this.chksubac.Name = "chksubac";
		this.chksubac.Size = new System.Drawing.Size(29, 19);
		this.chksubac.TabIndex = 8;
		this.chksubac.Text = " ";
		this.chksubac.UseVisualStyleBackColor = true;
		this.chksubac.Click += new System.EventHandler(chksubac_Click);
		this.label53.AutoSize = true;
		this.label53.BackColor = System.Drawing.Color.White;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(397, 274);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(84, 15);
		this.label53.TabIndex = 163;
		this.label53.Text = "Country Name";
		this.label56.AutoSize = true;
		this.label56.BackColor = System.Drawing.Color.White;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.Location = new System.Drawing.Point(9, 219);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(70, 15);
		this.label56.TabIndex = 17;
		this.label56.Text = "Is Sub Party";
		this.label38.AutoSize = true;
		this.label38.BackColor = System.Drawing.Color.White;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(397, 329);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(62, 15);
		this.label38.TabIndex = 173;
		this.label38.Text = "City Name";
		this.label39.AutoSize = true;
		this.label39.BackColor = System.Drawing.Color.White;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label39.Location = new System.Drawing.Point(9, 431);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(59, 15);
		this.label39.TabIndex = 206;
		this.label39.Text = "Address 2";
		this.label39.Visible = false;
		this.label46.AutoSize = true;
		this.label46.BackColor = System.Drawing.Color.White;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label46.Location = new System.Drawing.Point(397, 300);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(53, 15);
		this.label46.TabIndex = 168;
		this.label46.Text = "Province";
		this.label46.Visible = false;
		this.label42.AutoSize = true;
		this.label42.BackColor = System.Drawing.Color.White;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(9, 137);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(134, 15);
		this.label42.TabIndex = 158;
		this.label42.Text = "General Ledger Account";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label20);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 25);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1020, 28);
		this.panel3.TabIndex = 1;
		this.label20.AutoSize = true;
		this.label20.BackColor = System.Drawing.Color.Teal;
		this.label20.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label20.Location = new System.Drawing.Point(10, 5);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(125, 19);
		this.label20.TabIndex = 0;
		this.label20.Text = "Define Parties";
		this.panel2.Controls.Add(this.toolStrip2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1020, 25);
		this.panel2.TabIndex = 0;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.btnnew, this.btnRefresh, this.btnupdate, this.btnsave, this.btnBankDetail, this.btnShipToAddress, this.btnSupplierLimits, this.toolStripButton1, this.BtnAttributesAllocation });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1020, 27);
		this.toolStrip2.TabIndex = 0;
		this.toolStrip2.Text = "toolStrip2";
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
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnBankDetail.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnBankDetail.Name = "btnBankDetail";
		this.btnBankDetail.Size = new System.Drawing.Size(84, 24);
		this.btnBankDetail.Text = "Bank Detail";
		this.btnBankDetail.Click += new System.EventHandler(btnBankDetail_Click);
		this.btnShipToAddress.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShipToAddress.Name = "btnShipToAddress";
		this.btnShipToAddress.Size = new System.Drawing.Size(107, 24);
		this.btnShipToAddress.Text = "ShipToAddress";
		this.btnShipToAddress.Click += new System.EventHandler(btnShipToAddress_Click);
		this.btnSupplierLimits.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSupplierLimits.Name = "btnSupplierLimits";
		this.btnSupplierLimits.Size = new System.Drawing.Size(83, 24);
		this.btnSupplierLimits.Text = "PartyLimits";
		this.btnSupplierLimits.Click += new System.EventHandler(btnSupplierLimits_Click);
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(150, 24);
		this.toolStripButton1.Text = "Party Multi Language";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.BtnAttributesAllocation.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnAttributesAllocation.Image = Architecture.WinApp.Properties.Resources.Account_Reports;
		this.BtnAttributesAllocation.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnAttributesAllocation.Name = "BtnAttributesAllocation";
		this.BtnAttributesAllocation.Size = new System.Drawing.Size(191, 24);
		this.BtnAttributesAllocation.Text = "Item Allocation To Party";
		this.BtnAttributesAllocation.Click += new System.EventHandler(BtnAttributesAllocation_Click);
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1034, 749);
		this.tabControl1.TabIndex = 1;
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1026, 720);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel1);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1026, 720);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Register";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		this.DataGridHistory.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(3, 158);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1020, 559);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.ctrlGrdBar2);
		this.panel9.Controls.Add(this.label22);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.ForeColor = System.Drawing.Color.White;
		this.panel9.Location = new System.Drawing.Point(3, 133);
		this.panel9.Margin = new System.Windows.Forms.Padding(4);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1020, 25);
		this.panel9.TabIndex = 8;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(979, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar2.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 22);
		this.ctrlGrdBar2.TabIndex = 8;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label22.AutoSize = true;
		this.label22.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label22.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.White;
		this.label22.Location = new System.Drawing.Point(4, 2);
		this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(142, 19);
		this.label22.TabIndex = 3;
		this.label22.Text = "Filtered Records";
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.groupBox1);
		this.panel1.Controls.Add(this.label21);
		this.panel1.Controls.Add(this.txtInvoiceNo);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 56);
		this.panel1.Margin = new System.Windows.Forms.Padding(4);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1020, 77);
		this.panel1.TabIndex = 9;
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPartyGroupRegister);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCityRegister);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPartyTypeRegister);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbGlAccountRegister);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 4);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(941, 71);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(604, 41);
		this.btnshow.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(62, 25);
		this.btnshow.TabIndex = 4;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.CmbPartyGroupRegister.AllowNull = (DefaultableBoolean)1;
		this.CmbPartyGroupRegister.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPartyGroupRegister.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPartyGroupRegister.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPartyGroupRegister).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbPartyGroupRegister).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPartyGroupRegister).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance152).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance152).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPartyGroupRegister).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPartyGroupRegister).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance153;
		((SpecialBoxBase)((UltraGridBase)this.CmbPartyGroupRegister).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance163).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance163).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPartyGroupRegister).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.CmbPartyGroupRegister).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPartyGroupRegister).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPartyGroupRegister).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPartyGroupRegister).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbPartyGroupRegister).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPartyGroupRegister).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPartyGroupRegister).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance3).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPartyGroupRegister).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbPartyGroupRegister).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPartyGroupRegister).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPartyGroupRegister).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance14).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPartyGroupRegister).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbPartyGroupRegister).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPartyGroupRegister).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPartyGroupRegister).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbPartyGroupRegister).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPartyGroupRegister).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbPartyGroupRegister).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPartyGroupRegister).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPartyGroupRegister).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPartyGroupRegister).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPartyGroupRegister.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPartyGroupRegister).Location = new System.Drawing.Point(373, 12);
		((System.Windows.Forms.Control)(object)this.CmbPartyGroupRegister).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.CmbPartyGroupRegister).Name = "CmbPartyGroupRegister";
		((System.Windows.Forms.Control)(object)this.CmbPartyGroupRegister).Size = new System.Drawing.Size(224, 26);
		((System.Windows.Forms.Control)(object)this.CmbPartyGroupRegister).TabIndex = 2;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(301, 18);
		this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(70, 15);
		this.label3.TabIndex = 147;
		this.label3.Text = "Party Group";
		this.CmbCityRegister.AllowNull = (DefaultableBoolean)1;
		this.CmbCityRegister.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCityRegister.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCityRegister.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCityRegister).DisplayLayout.Appearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbCityRegister).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCityRegister).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCityRegister).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCityRegister).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance101;
		((SpecialBoxBase)((UltraGridBase)this.CmbCityRegister).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance103).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance103).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCityRegister).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbCityRegister).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCityRegister).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCityRegister).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCityRegister).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbCityRegister).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCityRegister).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCityRegister).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance107).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCityRegister).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbCityRegister).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCityRegister).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance108).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCityRegister).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCityRegister).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbCityRegister).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCityRegister).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCityRegister).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbCityRegister).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCityRegister).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbCityRegister).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCityRegister).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCityRegister).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCityRegister).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCityRegister.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCityRegister).Location = new System.Drawing.Point(373, 40);
		((System.Windows.Forms.Control)(object)this.CmbCityRegister).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.CmbCityRegister).Name = "CmbCityRegister";
		((System.Windows.Forms.Control)(object)this.CmbCityRegister).Size = new System.Drawing.Size(224, 26);
		((System.Windows.Forms.Control)(object)this.CmbCityRegister).TabIndex = 3;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(301, 46);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(62, 15);
		this.label2.TabIndex = 147;
		this.label2.Text = "City Name";
		this.CmbPartyTypeRegister.AllowNull = (DefaultableBoolean)1;
		this.CmbPartyTypeRegister.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPartyTypeRegister.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPartyTypeRegister.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPartyTypeRegister).DisplayLayout.Appearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbPartyTypeRegister).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPartyTypeRegister).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPartyTypeRegister).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPartyTypeRegister).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance115;
		((SpecialBoxBase)((UltraGridBase)this.CmbPartyTypeRegister).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance116).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance116).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPartyTypeRegister).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbPartyTypeRegister).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPartyTypeRegister).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPartyTypeRegister).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPartyTypeRegister).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbPartyTypeRegister).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPartyTypeRegister).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPartyTypeRegister).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance120).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPartyTypeRegister).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbPartyTypeRegister).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPartyTypeRegister).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance121).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance121).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance121).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPartyTypeRegister).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance122).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPartyTypeRegister).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbPartyTypeRegister).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPartyTypeRegister).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPartyTypeRegister).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbPartyTypeRegister).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPartyTypeRegister).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbPartyTypeRegister).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPartyTypeRegister).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPartyTypeRegister).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPartyTypeRegister).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPartyTypeRegister.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPartyTypeRegister).Location = new System.Drawing.Point(73, 12);
		((System.Windows.Forms.Control)(object)this.CmbPartyTypeRegister).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.CmbPartyTypeRegister).Name = "CmbPartyTypeRegister";
		((System.Windows.Forms.Control)(object)this.CmbPartyTypeRegister).Size = new System.Drawing.Size(224, 26);
		((System.Windows.Forms.Control)(object)this.CmbPartyTypeRegister).TabIndex = 0;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(6, 18);
		this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(62, 15);
		this.label5.TabIndex = 147;
		this.label5.Text = "Party Type";
		this.CmbGlAccountRegister.AllowNull = (DefaultableBoolean)1;
		this.CmbGlAccountRegister.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbGlAccountRegister.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbGlAccountRegister.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbGlAccountRegister).DisplayLayout.Appearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbGlAccountRegister).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbGlAccountRegister).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance127).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance127).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbGlAccountRegister).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGlAccountRegister).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance128;
		((SpecialBoxBase)((UltraGridBase)this.CmbGlAccountRegister).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGlAccountRegister).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbGlAccountRegister).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbGlAccountRegister).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbGlAccountRegister).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbGlAccountRegister).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbGlAccountRegister).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbGlAccountRegister).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGlAccountRegister).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance133).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbGlAccountRegister).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbGlAccountRegister).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbGlAccountRegister).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGlAccountRegister).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance136).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbGlAccountRegister).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbGlAccountRegister).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbGlAccountRegister).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbGlAccountRegister).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbGlAccountRegister).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbGlAccountRegister).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbGlAccountRegister).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbGlAccountRegister).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbGlAccountRegister).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbGlAccountRegister).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbGlAccountRegister.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbGlAccountRegister).Location = new System.Drawing.Point(73, 40);
		((System.Windows.Forms.Control)(object)this.CmbGlAccountRegister).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.CmbGlAccountRegister).Name = "CmbGlAccountRegister";
		((System.Windows.Forms.Control)(object)this.CmbGlAccountRegister).Size = new System.Drawing.Size(224, 26);
		((System.Windows.Forms.Control)(object)this.CmbGlAccountRegister).TabIndex = 1;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(6, 46);
		this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(66, 15);
		this.label7.TabIndex = 147;
		this.label7.Text = "Gl Account";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(1304, 251);
		this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(64, 15);
		this.label21.TabIndex = 154;
		this.label21.Text = "Invoice No";
		this.label21.Visible = false;
		this.txtInvoiceNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceNo.Location = new System.Drawing.Point(1308, 273);
		this.txtInvoiceNo.Margin = new System.Windows.Forms.Padding(4);
		this.txtInvoiceNo.Name = "txtInvoiceNo";
		this.txtInvoiceNo.Size = new System.Drawing.Size(228, 22);
		this.txtInvoiceNo.TabIndex = 5;
		this.txtInvoiceNo.Visible = false;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label23);
		this.panel8.Controls.Add(this.toolStrip3);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Margin = new System.Windows.Forms.Padding(4);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1020, 53);
		this.panel8.TabIndex = 10;
		this.label23.AutoSize = true;
		this.label23.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(4, 30);
		this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(128, 19);
		this.label23.TabIndex = 3;
		this.label23.Text = "Parties Report";
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.BtnResetRegister, this.BtnRefreshRegister, this.btnRegister, this.BtnShortCutKeysRegister });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1020, 27);
		this.toolStrip3.TabIndex = 11;
		this.toolStrip3.Text = "toolStrip3";
		this.BtnResetRegister.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnResetRegister.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnResetRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnResetRegister.Name = "BtnResetRegister";
		this.BtnResetRegister.Size = new System.Drawing.Size(59, 24);
		this.BtnResetRegister.Text = "&New";
		this.BtnResetRegister.Click += new System.EventHandler(BtnResetRegister_Click);
		this.BtnRefreshRegister.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshRegister.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshRegister.Name = "BtnRefreshRegister";
		this.BtnRefreshRegister.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshRegister.Text = "&Refresh";
		this.BtnRefreshRegister.Click += new System.EventHandler(BtnRefreshRegister_Click);
		this.btnRegister.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRegister.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRegister.Name = "btnRegister";
		this.btnRegister.Size = new System.Drawing.Size(115, 24);
		this.btnRegister.Text = "292-Register";
		this.btnRegister.Click += new System.EventHandler(btnRegister_Click);
		this.BtnShortCutKeysRegister.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortCutKeysRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutKeysRegister.Name = "BtnShortCutKeysRegister";
		this.BtnShortCutKeysRegister.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutKeysRegister.Text = "ShortCut Keys";
		this.BtnShortCutKeysRegister.Click += new System.EventHandler(BtnShortCutKeysRegister_Click);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.Red;
		this.label17.Location = new System.Drawing.Point(157, 59);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(13, 16);
		this.label17.TabIndex = 171;
		this.label17.Text = "*";
		this.label17.Visible = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1034, 749);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "supfrmDefineSupplier";
		this.Text = "Eccountbook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(supfrmDefineSupplier_FormClosing);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(supfrmDefineSupplier1_KeyDown);
		this.panel4.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBusinessType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAdvanceAc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbparentac).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPartyType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbglac).EndInit();
		this.groupBox2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.box).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDiscountType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbgroupdetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcountry).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbprovince).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPartyGroupRegister).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCityRegister).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPartyTypeRegister).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGlAccountRegister).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
	}
}
}
