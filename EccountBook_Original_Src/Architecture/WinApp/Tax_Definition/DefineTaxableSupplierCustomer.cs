using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.General;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Lookups;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Tax_Definition;

public class DefineTaxableSupplierCustomer : Form
{
	private bool Confiquration;

	private string saveDirectory = "";

	private string fileSavePath = "";

	private string ofd = "";

	private DropDownBind DDL = new DropDownBind();

	private City city = new City();

	private Country country = new Country();

	private StateProvince stateprovince = new StateProvince();

	private DataTable table = new DataTable();

	private DataTable Supplierlst = new DataTable();

	private DataTable dtcity = new DataTable();

	private DataTable dtprovince = new DataTable();

	private int RecId;

	private bool SubsidiaryAccountAllownOnVouchers;

	public UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Panel panel4;

	private Panel panel3;

	private Label label20;

	private Panel panel2;

	private ToolStrip toolStrip2;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private ToolStripButton btnPrint;

	private Panel panel6;

	private Button btnAdd;

	private Label label14;

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

	private Label label45;

	private Label label42;

	private Label label44;

	private TextBox txtwebsite;

	private Label label43;

	private TextBox txtSupplierCode;

	private TextBox txtfirstname;

	private TextBox txtaddress1;

	private Panel panel7;

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

	private ToolStripButton btn293Print;

	internal UltraCombo cmbDiscountType;

	private Label label9;

	private Panel panel9;

	private Panel panel10;

	internal GridEX grdfrm;

	private Panel panel5;

	private Label label12;

	private CtrlGrdBar ctrlGrdBar1;

	private Button btnMoreInfo;

	internal UltraCombo cmbPartyType;

	private Label label13;

	private TextBox txtPartyCode;

	private Label label15;

	private ToolStripButton toolStripButton1;

	public DefineTaxableSupplierCustomer(UserAccount puserAccount)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = puserAccount;
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		try
		{
			Refresh();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		try
		{
			SupplierCustomer supcus = null;
			if (formvalidation() && MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				if (supcus == null)
				{
					supcus = new SupplierCustomer();
				}
				supcus.CompanyName = Conversion.ToString((object)txtcompany.Text.Trim());
				supcus.ReportingTitle = Conversion.ToString((object)txtcompany.Text.Trim());
				supcus.GlAccountId = Conversion.ToInt(cmbglac.Value);
				supcus.CustomerGroupId = Conversion.ToInt(cmbgroupdetail.Value);
				supcus.IsSubSupCust = Conversion.ToBool((object)chksubac.Text.Trim());
				supcus.ParentsSupCustId = Conversion.ToInt(cmbparentac.Value);
				supcus.NTN_No = Conversion.ToString((object)txtntnno.Text.Trim());
				supcus.STRN_No = Conversion.ToString((object)txtstrnno.Text.Trim());
				supcus.Title = Conversion.ToString((object)txttitle.Text.Trim());
				supcus.FirstName = Conversion.ToString((object)txtfirstname.Text.Trim());
				supcus.LastName = Conversion.ToString((object)txtlastname.Text.Trim());
				supcus.CNIC = Conversion.ToString((object)txtcnicno.Text.Trim());
				supcus.CNIC_EXPIRY_DATE = Conversion.ToDateTime((object)txtcnicexp.Value);
				supcus.Address1 = Conversion.ToString((object)txtaddress1.Text.Trim());
				supcus.Address2 = Conversion.ToString((object)txtaddress2.Text.Trim());
				supcus.Status = Conversion.ToBool((object)chkactivestatus.Checked);
				supcus.CountryId = Conversion.ToInt(cmbcountry.Value);
				supcus.DiscountPolicyId = Conversion.ToInt(cmbDiscountType.Value);
				supcus.StateProvinceId = Conversion.ToInt(cmbprovince.Value);
				supcus.CityId = Conversion.ToInt(cmbcity.Value);
				supcus.Town = Conversion.ToString((object)txttown.Text.Trim());
				supcus.ZipCode = Conversion.ToString((object)txtzipcode.Text.Trim());
				supcus.Phone = Conversion.ToString((object)txtphone.Text.Trim());
				supcus.MobileOffice = Conversion.ToString((object)txtofficemobile.Text.Trim());
				supcus.MobilePersonal = Conversion.ToString((object)txtpermobile.Text.Trim());
				supcus.Email = Conversion.ToString((object)txtemail.Text.Trim());
				supcus.SupCustCode = txtSupplierCode.Text.ToLower().Trim();
				supcus.WebPage = Conversion.ToString((object)txtwebsite.Text.Trim());
				supcus.AdvanceGlAcId = Conversion.ToInt(CmbAdvanceAc.Value);
				supcus.PartyTypeId = Conversion.ToInt(cmbPartyType.Value);
				supcus.PartyTypePrefix = Conversion.ToString((object)txtPartyCode.Text);
				supcus.PictureURL = fileSavePath;
				supcus.OrganizationId = UserAccount.OrganizationId;
				supcus.CompanyId = UserAccount.CompanyId;
				supcus.EntryUser = UserAccount.ID;
				supcus.IsTaxable = true;
				SupplierCustomer.Save(supcus);
				MessageBox.Show("Data Save Successfully");
				if (ofd != null && ofd != string.Empty)
				{
					File.Copy(ofd, fileSavePath, overwrite: true);
					ofd = string.Empty;
					fileSavePath = string.Empty;
					box.Image = null;
					box.Invalidate();
				}
				Refresh();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		try
		{
			if (RecId == 0)
			{
				throw new Exception("Record Id not found");
			}
			SupplierCustomer supcus = new SupplierCustomer();
			if (!formvalidation() || MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			supcus.Id = Conversion.ToInt((object)RecId);
			supcus.OrganizationId = UserAccount.OrganizationId;
			supcus.CompanyId = UserAccount.CompanyId;
			supcus.ModifyUser = UserAccount.ID;
			supcus.PostDate = DateTime.Now;
			supcus.PostState = false;
			supcus.FTN_No = "0";
			supcus.CreditLimit = 0.0;
			supcus.IsDebitCredit = false;
			supcus.DebitCreditAmount = 0.0;
			supcus.CompanyName = Conversion.ToString((object)txtcompany.Text.Trim());
			supcus.ReportingTitle = Conversion.ToString((object)txtcompany.Text.Trim());
			supcus.GlAccountId = Conversion.ToInt(cmbglac.Value);
			supcus.CustomerGroupId = Conversion.ToInt(cmbgroupdetail.Value);
			supcus.IsSubSupCust = Conversion.ToBool((object)chksubac.Text.Trim());
			supcus.ParentsSupCustId = Conversion.ToInt(cmbparentac.Value);
			supcus.NTN_No = Conversion.ToString((object)txtntnno.Text.Trim());
			supcus.STRN_No = Conversion.ToString((object)txtstrnno.Text.Trim());
			supcus.Title = Conversion.ToString((object)txttitle.Text.Trim());
			supcus.FirstName = Conversion.ToString((object)txtfirstname.Text.Trim());
			supcus.LastName = Conversion.ToString((object)txtlastname.Text.Trim());
			supcus.CNIC = Conversion.ToString((object)txtcnicno.Text.Trim());
			supcus.CNIC_EXPIRY_DATE = Conversion.ToDateTime((object)txtcnicexp.Value);
			supcus.Address1 = Conversion.ToString((object)txtaddress1.Text.Trim());
			supcus.Address2 = Conversion.ToString((object)txtaddress2.Text.Trim());
			supcus.CountryId = Conversion.ToInt(cmbcountry.Value);
			supcus.DiscountPolicyId = Conversion.ToInt(cmbDiscountType.Value);
			supcus.StateProvinceId = Conversion.ToInt(cmbprovince.Value);
			supcus.CityId = Conversion.ToInt(cmbcity.Value);
			supcus.Town = Conversion.ToString((object)txttown.Text.Trim());
			supcus.ZipCode = Conversion.ToString((object)txtzipcode.Text.Trim());
			supcus.Phone = Conversion.ToString((object)txtphone.Text.Trim());
			supcus.MobileOffice = Conversion.ToString((object)txtofficemobile.Text.Trim());
			supcus.MobilePersonal = Conversion.ToString((object)txtpermobile.Text.Trim());
			supcus.Email = Conversion.ToString((object)txtemail.Text.Trim());
			supcus.Status = Conversion.ToBool((object)chkactivestatus.Checked);
			supcus.SupCustCode = txtSupplierCode.Text.Trim();
			supcus.WebPage = Conversion.ToString((object)txtwebsite.Text.Trim());
			supcus.PictureURL = Conversion.ToString((object)fileSavePath);
			supcus.AdvanceGlAcId = Conversion.ToInt(CmbAdvanceAc.Value);
			supcus.PartyTypeId = Conversion.ToInt(cmbPartyType.Value);
			supcus.PartyTypePrefix = Conversion.ToString(((UltraDropDownBase)cmbPartyType).SelectedRow.Cells[2].Value);
			supcus.IsTaxable = true;
			if (SupplierCustomer.Save(supcus) > 0)
			{
				MessageBox.Show("Update Successfully");
				if (ofd != null && ofd != string.Empty)
				{
					File.Copy(ofd, fileSavePath, overwrite: true);
					box.Image = null;
					box.Invalidate();
					ofd = "";
				}
			}
			btnsave.Visible = true;
			btnupdate.Visible = false;
			datagridviewform();
			Refresh();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbglacfill()
	{
	}

	private void cmbcountryfill()
	{
		try
		{
			country.OrganizationId = UserAccount.OrganizationId;
			country.CompanyId = UserAccount.CompanyId;
			DataTable dtCountry = country.GetAll(country);
			if (dtCountry.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("Description");
				for (int i = 0; i < dtCountry.Rows.Count; i++)
				{
					dt.Rows.Add(dtCountry.Rows[i]["Id"], dtCountry.Rows[i]["Description"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbcountry, "Id", "Description", "Country Name", ZeroIndex: true);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbsupgroupsfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtCG = CustomerGroup.GetAll(new CustomerGroup
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtCG.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("Description");
				for (int i = 0; i < dtCG.Rows.Count; i++)
				{
					dt.Rows.Add(dtCG.Rows[i]["Id"], dtCG.Rows[i]["Description"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbgroupdetail, "Id", "Description", "Customer Group", ZeroIndex: false);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DiscountPolicyFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtCG = DiscountPolicy.GetDiscountPolicyForCustomers(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtCG.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("PolicyName");
				dt.Columns.Add("DiscountType");
				dt.Columns.Add("DiscountRate");
				dt.Columns.Add("EffectiveDate");
				for (int i = 0; i < dtCG.Rows.Count; i++)
				{
					dt.Rows.Add(dtCG.Rows[i]["Id"], dtCG.Rows[i]["PolicyName"], dtCG.Rows[i]["DiscountType"], dtCG.Rows[i]["DiscountRate"], dtCG.Rows[i]["EffectiveDate"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbDiscountType, "Id", "PolicyName", "Discount Policy", ZeroIndex: false);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierNameFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		SupplierCustomer sup = new SupplierCustomer();
		sup.OrganizationId = UserAccount.OrganizationId;
		sup.CompanyId = UserAccount.CompanyId;
		Supplierlst = SupplierCustomer.Getall(sup);
	}

	private void parentacfill()
	{
		try
		{
			if (Supplierlst.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("CompanyName");
				for (int i = 0; i < Supplierlst.Rows.Count; i++)
				{
					dt.Rows.Add(Supplierlst.Rows[i]["Id"], Supplierlst.Rows[i]["CompanyName"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbparentac, "Id", "CompanyName", "Supplier Name", ZeroIndex: true);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void datagridviewform()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters sup = new ReportsParameters();
			sup.OrganizationId = UserAccount.OrganizationId;
			sup.CompanyId = UserAccount.CompanyId;
			if (!((Control)(object)cmbPartyType).Enabled)
			{
				sup.PartyTypeId = Conversion.ToInt(cmbPartyType.Value);
			}
			sup.IsTaxable = true;
			DataTable getList = SupplierCustomer.FormHistory(sup);
			table.Clear();
			grdfrm.DataSource = table;
			for (int i = 0; i < getList.Rows.Count; i++)
			{
				table.Rows.Add(getList.Rows[i]["Id"], getList.Rows[i]["PartyType"], getList.Rows[i]["PartyTypePrefixNo"], getList.Rows[i]["CompanyName"], getList.Rows[i]["AccountCode"], getList.Rows[i]["AccountTitle"], getList.Rows[i]["ReportingTitle"], getList.Rows[i]["CustomerGroup"], getList.Rows[i]["Address1"], getList.Rows[i]["Address2"], getList.Rows[i]["CNIC"], getList.Rows[i]["Email"], getList.Rows[i]["MobilePersonal"], getList.Rows[i]["PolicyName"], getList.Rows[i]["EntryUserName"], getList.Rows[i]["EntryDate"], getList.Rows[i]["ModifyUserName"], getList.Rows[i]["ModifyDate"]);
			}
			grdfrm.DataSource = table;
			grdfrm.RetrieveStructure();
			GridSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private new void Refresh()
	{
		try
		{
			RecId = 0;
			((Control)(object)cmbPartyType).Enabled = true;
			txtaddress1.Clear();
			txtaddress2.Clear();
			txtcnicno.Clear();
			txtcompany.Clear();
			txtemail.Clear();
			txtfirstname.Clear();
			txtlastname.Clear();
			txtntnno.Clear();
			txtofficemobile.Clear();
			txtpermobile.Clear();
			txtphone.Clear();
			txtstrnno.Clear();
			txttitle.Clear();
			txttitle.Clear();
			txttown.Clear();
			txtwebsite.Clear();
			txtzipcode.Clear();
			((Control)(object)cmbglac).Text = string.Empty;
			((Control)(object)cmbcity).Text = string.Empty;
			((Control)(object)cmbcountry).Text = string.Empty;
			((Control)(object)cmbgroupdetail).Text = string.Empty;
			((Control)(object)cmbparentac).Text = string.Empty;
			((Control)(object)cmbprovince).Text = string.Empty;
			((Control)(object)CmbAdvanceAc).Text = string.Empty;
			txtSupplierCode.Clear();
			btnsave.Visible = true;
			btnupdate.Visible = false;
			box.Image = null;
			box.Invalidate();
			DiscountPolicyFill();
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
			datagridviewform();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSetting()
	{
		try
		{
			grdfrm.RootTable.Columns["Id"].Visible = false;
			grdfrm.RootTable.Columns["ReportingTitle"].Visible = false;
			grdfrm.RootTable.Columns["PolicyName"].Visible = false;
			grdfrm.RootTable.Columns["SupplierName"].Caption = "Party Name";
			grdfrm.RootTable.Columns["CustomerGroup"].Caption = "Party Group";
			CommonServices.GridAutoAdjustmentNew(grdfrm);
			grdfrm.RootTable.Columns.Add("Edit");
			grdfrm.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdfrm.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdfrm.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdfrm.RootTable.Columns["Edit"].Width = 40;
			grdfrm.RootTable.Columns.Add("View");
			grdfrm.RootTable.Columns["View"].Caption = "Print";
			grdfrm.RootTable.Columns["View"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdfrm.RootTable.Columns["View"].ButtonStyle = (ButtonStyle)5;
			grdfrm.RootTable.Columns["View"].ButtonText = "Print";
			grdfrm.RootTable.Columns["View"].Width = 40;
			grdfrm.RootTable.Columns["View"].Position = 0;
			grdfrm.RootTable.Columns["Edit"].Position = 1;
			grdfrm.FrozenColumns = 2;
			ctrlGrdBar1_Load_1(null, null);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void chksubac_Click(object sender, EventArgs e)
	{
		try
		{
			if (chksubac.Checked)
			{
				((Control)(object)cmbparentac).Enabled = true;
			}
			else
			{
				((Control)(object)cmbparentac).Enabled = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool formvalidation()
	{
		try
		{
			if (((UltraGridBase)cmbPartyType).ActiveRow == null)
			{
				MessageBox.Show("Party Type Field Is Required!!!");
				cmbPartyType.Focus();
				return false;
			}
			if (txtPartyCode.Text.Trim() == string.Empty || txtPartyCode.Text.Trim() == null)
			{
				MessageBox.Show("Please Enter Party Code!!!");
				txtPartyCode.Focus();
				return false;
			}
			if (((Control)(object)cmbglac).Text.Trim() == string.Empty || Conversion.ToInt(cmbglac.Value) == 0)
			{
				MessageBox.Show("Please Select GL Account!!!");
				cmbglac.Focus();
				return false;
			}
			if (txtcompany.Text.Trim() == string.Empty || txtcompany.Text.Trim() == null)
			{
				MessageBox.Show("Please Enter Company Name!!!");
				txtcompany.Focus();
				return false;
			}
			if (((Control)(object)cmbcountry).Text.Trim() == string.Empty || Conversion.ToInt(cmbcountry.Value) == 0)
			{
				MessageBox.Show("Please Select Country!!!");
				cmbcountry.Focus();
				return false;
			}
			if (((Control)(object)cmbcity).Text.Trim() == string.Empty || Conversion.ToInt(cmbcity.Value) == 0)
			{
				MessageBox.Show("Please Select City!!!");
				cmbcity.Focus();
				return false;
			}
			if (txtpermobile.Text.Trim() == string.Empty || txtpermobile.Text == null)
			{
				MessageBox.Show("Please Enter Phone Number");
				txtpermobile.Focus();
				return false;
			}
			if (btnsave.Visible && btnsave.Enabled && !chkactivestatus.Checked)
			{
				MessageBox.Show("Please check the Active Status!!!");
				chkactivestatus.Focus();
				return false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return true;
	}

	private void supfrmDefineSupplier1_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 700);
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			table.Columns.Add("Id", typeof(int));
			table.Columns.Add("PartyType");
			table.Columns.Add("PartyCode");
			table.Columns.Add("SupplierName");
			table.Columns.Add("AccountCode");
			table.Columns.Add("LedgerAccount");
			table.Columns.Add("ReportingTitle");
			table.Columns.Add("CustomerGroup");
			table.Columns.Add("Adderess");
			table.Columns.Add("Adderess1");
			table.Columns.Add("CNIC");
			table.Columns.Add("Email");
			table.Columns.Add("Phone");
			table.Columns.Add("PolicyName");
			table.Columns.Add("EntryUser");
			table.Columns.Add("EntryDate");
			table.Columns.Add("ModifyUser");
			table.Columns.Add("ModifyDate");
			CityFill();
			SupplierNameFill();
			cmbglacfill();
			cmbcountryfill();
			cmbsupgroupsfill();
			parentacfill();
			((Control)(object)cmbparentac).Enabled = false;
			txtcompany.Focus();
			btnupdate.Visible = false;
			((Control)(object)cmbprovince).Enabled = true;
			((Control)(object)cmbcity).Enabled = true;
			((UltraGridBase)cmbgroupdetail).Rows[0].Activate();
			cmbPartyType.Focus();
			PartyType();
			DiscountPolicyFill();
			((Control)(object)cmbDiscountType).Enabled = false;
			((Control)(object)cmbDiscountType).Text = string.Empty;
			panel7.Visible = false;
			btnMoreInfo.Text = "+";
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
			datagridviewform();
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
					Confiquration = true;
				}
			}
			else
			{
				MessageBox.Show("Please Map the Path in Configration");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int ID)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		try
		{
			btnsave.Visible = false;
			btnupdate.Visible = true;
			btnAdd.Visible = false;
			SupplierCustomer supcus = new SupplierCustomer();
			RecId = Conversion.ToInt((object)ID);
			supcus = SupplierCustomer.GetByID(RecId);
			cmbPartyType.Value = Conversion.ToInt((object)supcus.PartyTypeId);
			cmbPartyType_Leave(null, null);
			((Control)(object)cmbPartyType).Enabled = false;
			cmbglac.Value = Conversion.ToInt((object)supcus.GlAccountId);
			txtcompany.Text = Conversion.ToString((object)supcus.CompanyName);
			cmbgroupdetail.Value = Conversion.ToInt((object)supcus.CustomerGroupId);
			chksubac.Text = Conversion.ToString((object)supcus.IsSubSupCust);
			cmbparentac.Value = Conversion.ToInt((object)supcus.ParentsSupCustId);
			cmbDiscountType.Value = Conversion.ToInt((object)supcus.DiscountPolicyId);
			txtntnno.Text = Conversion.ToString((object)supcus.NTN_No);
			txtstrnno.Text = Conversion.ToString((object)supcus.STRN_No);
			txttitle.Text = Conversion.ToString((object)supcus.Title);
			txtfirstname.Text = Conversion.ToString((object)supcus.FirstName);
			txtlastname.Text = Conversion.ToString((object)supcus.LastName);
			txtcnicno.Text = Conversion.ToString((object)supcus.CNIC);
			if (!Conversion.CheckDateTimeNull(supcus.CNIC_EXPIRY_DATE))
			{
				txtcnicexp.Value = Conversion.ToDateTime((object)supcus.CNIC_EXPIRY_DATE);
			}
			txtaddress1.Text = Conversion.ToString((object)supcus.Address1);
			txtaddress2.Text = Conversion.ToString((object)supcus.Address2);
			chkactivestatus.Checked = Conversion.ToBool((object)supcus.Status);
			cmbcountry.Value = Conversion.ToString((object)supcus.CountryId);
			cmbprovince.Value = Conversion.ToString((object)supcus.StateProvinceId);
			cmbcity.Value = Conversion.ToString((object)supcus.CityId);
			txttown.Text = Conversion.ToString((object)supcus.Town);
			txtzipcode.Text = Conversion.ToString((object)supcus.ZipCode);
			txtphone.Text = Conversion.ToString((object)supcus.Phone);
			txtofficemobile.Text = Conversion.ToString((object)supcus.MobileOffice);
			txtpermobile.Text = Conversion.ToString((object)supcus.MobilePersonal);
			txtemail.Text = Conversion.ToString((object)supcus.Email);
			txtSupplierCode.Text = supcus.SupCustCode;
			txtwebsite.Text = Conversion.ToString((object)supcus.WebPage);
			CmbAdvanceAc.Value = Conversion.ToInt((object)supcus.AdvanceGlAcId);
			txtPartyCode.Text = supcus.PartyTypePrefix;
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

	private void grdfrm_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			ReadById(Conversion.ToInt((object)grdfrm.CurrentRow.Cells[0].Value.ToString()));
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = GeneralReprots.SupplierCustomerRegister(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				CityId = Conversion.ToInt(cmbcity.Value)
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("Record Not Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "292-InvRptSupplierRegister.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Print(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = GeneralReprots.SupplierCustomerRegister(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				SupplierCustomerId = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("Record Not Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "293-InvRptSupplierSlip.rpt");
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
			if (Confiquration)
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

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void grdfrm_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "View")
			{
				GridEXRow item = grdfrm.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					int PrintId = Conversion.ToInt(item.Cells["Id"].Value);
					Print(PrintId);
				}
			}
			if (e.Column.Key == "Edit")
			{
				ReadById(Conversion.ToInt((object)grdfrm.CurrentRow.Cells[0].Value.ToString()));
			}
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

	private void CityFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			City obj = new City();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtcity = City.GetAll(obj);
			if (dtcity.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("CityName");
				for (int i = 0; i < dtcity.Rows.Count; i++)
				{
					dt.Rows.Add(dtcity.Rows[i]["Id"], dtcity.Rows[i]["CityName"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbcity, "Id", "CityName", "Country Name", ZeroIndex: true);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GlAcountFillByPartyType(int PartyTypeId)
	{
		try
		{
			((UltraGridBase)cmbglac).DataSource = null;
			((Control)(object)cmbglac).Text = string.Empty;
			((UltraGridBase)CmbAdvanceAc).DataSource = null;
			((Control)(object)CmbAdvanceAc).Text = string.Empty;
			DataTable dt = SupplierCustomer.GetAccountsForSupplierCustomerByPartyTypeId(UserAccount.OrganizationId, UserAccount.CompanyId, PartyTypeId, 0);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbglac, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
				DDL.BindDDL(dt, CmbAdvanceAc, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
				return;
			}
			((UltraGridBase)cmbglac).DataSource = null;
			((Control)(object)cmbglac).Text = string.Empty;
			((UltraGridBase)CmbAdvanceAc).DataSource = null;
			((Control)(object)CmbAdvanceAc).Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PartyType()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtParty = SupplierCustomer.PartyTypeGetAll(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtParty.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("SupplierCustomerType");
				dt.Columns.Add("Prefix");
				for (int i = 0; i < dtParty.Rows.Count; i++)
				{
					dt.Rows.Add(dtParty.Rows[i]["Id"], dtParty.Rows[i]["SupplierCustomerType"], dtParty.Rows[i]["Prefix"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbPartyType, "Id", "SupplierCustomerType", "Party Type", ZeroIndex: false);
					((UltraGridBase)cmbPartyType).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					((UltraGridBase)cmbPartyType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
					((UltraGridBase)cmbPartyType).Rows[0].Activate();
					cmbPartyType_Leave(null, null);
				}
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
			if (btnsave.Visible && btnsave.Enabled && e.Control && e.KeyCode == Keys.S)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (btnupdate.Visible && btnupdate.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				btnupdate_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				btnPrint_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnBankDetail_Click(object sender, EventArgs e)
	{
		new SupfrmBankDetail(UserAccount).Show();
	}

	private void btnShipToAddress_Click(object sender, EventArgs e)
	{
		new SupfrmShipToAddress(UserAccount).Show();
	}

	private void btnSupplierLimits_Click(object sender, EventArgs e)
	{
		new SupfrmSupplierLimits(UserAccount).Show();
	}

	private void cmbglac_Leave(object sender, EventArgs e)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		try
		{
			if (SubsidiaryAccountAllownOnVouchers)
			{
				return;
			}
			DataTable lst = SupplierCustomer.Getall(new SupplierCustomer
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (lst.Rows.Count <= 0)
			{
				return;
			}
			int DataId = 0;
			for (int i = 0; i < lst.Rows.Count; i++)
			{
				if (Conversion.ToInt(lst.Rows[i]["GlAccountId"]) == Conversion.ToInt(cmbglac.Value))
				{
					DataId = Conversion.ToInt(lst.Rows[i]["Id"]);
				}
				if (DataId > 0)
				{
					break;
				}
			}
			if (DataId <= 0)
			{
				return;
			}
			SupplierCustomer supcus = new SupplierCustomer();
			supcus = SupplierCustomer.GetByID(DataId);
			if (supcus == null)
			{
				return;
			}
			txtcompany.Text = Conversion.ToString((object)supcus.CompanyName);
			cmbglac.Value = Conversion.ToInt((object)supcus.GlAccountId);
			cmbgroupdetail.Value = Conversion.ToInt((object)supcus.CustomerGroupId);
			chksubac.Text = Conversion.ToString((object)supcus.IsSubSupCust);
			cmbparentac.Value = Conversion.ToInt((object)supcus.ParentsSupCustId);
			txtntnno.Text = Conversion.ToString((object)supcus.NTN_No);
			txtstrnno.Text = Conversion.ToString((object)supcus.STRN_No);
			txttitle.Text = Conversion.ToString((object)supcus.Title);
			txtfirstname.Text = Conversion.ToString((object)supcus.FirstName);
			txtlastname.Text = Conversion.ToString((object)supcus.LastName);
			txtcnicno.Text = Conversion.ToString((object)supcus.CNIC);
			if (!Conversion.CheckDateTimeNull(supcus.CNIC_EXPIRY_DATE))
			{
				txtcnicexp.Value = Conversion.ToDateTime((object)supcus.CNIC_EXPIRY_DATE);
			}
			txtaddress1.Text = Conversion.ToString((object)supcus.Address1);
			txtaddress2.Text = Conversion.ToString((object)supcus.Address2);
			chkactivestatus.Checked = Conversion.ToBool((object)supcus.Status);
			cmbcountry.Value = Conversion.ToString((object)supcus.CountryId);
			cmbprovince.Value = Conversion.ToString((object)supcus.StateProvinceId);
			cmbcity.Value = Conversion.ToString((object)supcus.CityId);
			txttown.Text = Conversion.ToString((object)supcus.Town);
			txtzipcode.Text = Conversion.ToString((object)supcus.ZipCode);
			txtphone.Text = Conversion.ToString((object)supcus.Phone);
			txtofficemobile.Text = Conversion.ToString((object)supcus.MobileOffice);
			txtpermobile.Text = Conversion.ToString((object)supcus.MobilePersonal);
			txtemail.Text = Conversion.ToString((object)supcus.Email);
			txtSupplierCode.Text = supcus.SupCustCode;
			txtwebsite.Text = Conversion.ToString((object)supcus.WebPage);
			CmbAdvanceAc.Value = Conversion.ToInt((object)supcus.AdvanceGlAcId);
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
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btn293Print_Click(object sender, EventArgs e)
	{
		try
		{
			Print(RecId);
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
				DiscountPolicyFill();
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

	private void btnMoreInfo_Click(object sender, EventArgs e)
	{
		try
		{
			if (panel7.Visible)
			{
				panel7.Visible = false;
				btnMoreInfo.Text = "+";
			}
			else
			{
				panel7.Visible = true;
				btnMoreInfo.Text = "-";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbPartyType_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbPartyType).ActiveRow != null)
			{
				GlAcountFillByPartyType(Conversion.ToInt(cmbPartyType.Value));
				GetPartyCode();
				return;
			}
			((UltraGridBase)cmbglac).DataSource = null;
			((Control)(object)cmbglac).Text = string.Empty;
			((UltraGridBase)CmbAdvanceAc).DataSource = null;
			((Control)(object)CmbAdvanceAc).Text = string.Empty;
			txtPartyCode.Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetPartyCode()
	{
		try
		{
			string Code = SupplierCustomer.GeneratePartyCode(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToInt(cmbPartyType.Value));
			txtPartyCode.Text = Code;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
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

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		try
		{
			new SupplierCustomerMultiLanguage(UserAccount).Show();
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
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Expected O, but got Unknown
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Expected O, but got Unknown
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b1: Expected O, but got Unknown
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Expected O, but got Unknown
		//IL_0533: Unknown result type (might be due to invalid IL or missing references)
		//IL_053d: Expected O, but got Unknown
		//IL_0554: Unknown result type (might be due to invalid IL or missing references)
		//IL_055e: Expected O, but got Unknown
		//IL_055f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Expected O, but got Unknown
		//IL_056a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Expected O, but got Unknown
		//IL_0575: Unknown result type (might be due to invalid IL or missing references)
		//IL_057f: Expected O, but got Unknown
		//IL_0580: Unknown result type (might be due to invalid IL or missing references)
		//IL_058a: Expected O, but got Unknown
		//IL_0afb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b05: Expected O, but got Unknown
		Appearance appearance52 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Tax_Definition.DefineTaxableSupplierCustomer));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdfrm = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label12 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel7 = new System.Windows.Forms.Panel();
		this.txtfirstname = new System.Windows.Forms.TextBox();
		this.txtaddress1 = new System.Windows.Forms.TextBox();
		this.txtSupplierCode = new System.Windows.Forms.TextBox();
		this.label43 = new System.Windows.Forms.Label();
		this.CmbAdvanceAc = new UltraCombo();
		this.txtwebsite = new System.Windows.Forms.TextBox();
		this.cmbparentac = new UltraCombo();
		this.label44 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.txtphone = new System.Windows.Forms.MaskedTextBox();
		this.label48 = new System.Windows.Forms.Label();
		this.txtcnicno = new System.Windows.Forms.MaskedTextBox();
		this.label49 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.txtntnno = new System.Windows.Forms.TextBox();
		this.txtemail = new System.Windows.Forms.TextBox();
		this.txtlastname = new System.Windows.Forms.TextBox();
		this.txtzipcode = new System.Windows.Forms.TextBox();
		this.txtstrnno = new System.Windows.Forms.TextBox();
		this.label51 = new System.Windows.Forms.Label();
		this.txttitle = new System.Windows.Forms.TextBox();
		this.label52 = new System.Windows.Forms.Label();
		this.txtofficemobile = new System.Windows.Forms.TextBox();
		this.txttown = new System.Windows.Forms.TextBox();
		this.label54 = new System.Windows.Forms.Label();
		this.label55 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.chksubac = new System.Windows.Forms.CheckBox();
		this.label56 = new System.Windows.Forms.Label();
		this.txtcnicexp = new System.Windows.Forms.DateTimePicker();
		this.label32 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.txtPartyCode = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.cmbPartyType = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.btnMoreInfo = new System.Windows.Forms.Button();
		this.cmbDiscountType = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.txtpermobile = new System.Windows.Forms.TextBox();
		this.cmbgroupdetail = new UltraCombo();
		this.cmbcity = new UltraCombo();
		this.cmbcountry = new UltraCombo();
		this.cmbglac = new UltraCombo();
		this.cmbprovince = new UltraCombo();
		this.btnAdd = new System.Windows.Forms.Button();
		this.label14 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.browse = new System.Windows.Forms.Button();
		this.box = new System.Windows.Forms.PictureBox();
		this.chkactivestatus = new System.Windows.Forms.CheckBox();
		this.label57 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.txtcompany = new System.Windows.Forms.TextBox();
		this.label53 = new System.Windows.Forms.Label();
		this.txtaddress2 = new System.Windows.Forms.TextBox();
		this.label38 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label20 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btn293Print = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnBankDetail = new System.Windows.Forms.ToolStripButton();
		this.btnShipToAddress = new System.Windows.Forms.ToolStripButton();
		this.btnSupplierLimits = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel5.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAdvanceAc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbparentac).BeginInit();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPartyType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDiscountType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbgroupdetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcountry).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbglac).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbprovince).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.box).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(964, 661);
		this.panel1.TabIndex = 0;
		this.panel4.Controls.Add(this.panel9);
		this.panel4.Controls.Add(this.panel7);
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add(this.panel3);
		this.panel4.Controls.Add(this.panel2);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(964, 661);
		this.panel4.TabIndex = 1;
		this.panel9.Controls.Add(this.panel10);
		this.panel9.Controls.Add(this.panel5);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 427);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(964, 234);
		this.panel9.TabIndex = 5;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(0, 28);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(964, 206);
		this.panel10.TabIndex = 4;
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdfrm).BackColor = System.Drawing.Color.White;
		this.grdfrm.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupTotals = (GroupTotals)2;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(964, 206);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 0;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		this.grdfrm.ColumnButtonClick += new ColumnActionEventHandler(grdfrm_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdfrm).DoubleClick += new System.EventHandler(grdfrm_DoubleClick);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label12);
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(964, 28);
		this.panel5.TabIndex = 3;
		this.label12.AutoSize = true;
		this.label12.BackColor = System.Drawing.Color.Teal;
		this.label12.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label12.Location = new System.Drawing.Point(3, 3);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(125, 19);
		this.label12.TabIndex = 0;
		this.label12.Text = "Define Parties";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(906, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(53, 26);
		this.ctrlGrdBar1.TabIndex = 21;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load_1);
		this.panel7.Controls.Add(this.txtfirstname);
		this.panel7.Controls.Add(this.txtaddress1);
		this.panel7.Controls.Add(this.txtSupplierCode);
		this.panel7.Controls.Add(this.label43);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAdvanceAc);
		this.panel7.Controls.Add(this.txtwebsite);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbparentac);
		this.panel7.Controls.Add(this.label44);
		this.panel7.Controls.Add(this.label45);
		this.panel7.Controls.Add(this.label41);
		this.panel7.Controls.Add(this.label40);
		this.panel7.Controls.Add(this.label47);
		this.panel7.Controls.Add(this.txtphone);
		this.panel7.Controls.Add(this.label48);
		this.panel7.Controls.Add(this.txtcnicno);
		this.panel7.Controls.Add(this.label49);
		this.panel7.Controls.Add(this.label37);
		this.panel7.Controls.Add(this.label8);
		this.panel7.Controls.Add(this.txtntnno);
		this.panel7.Controls.Add(this.txtemail);
		this.panel7.Controls.Add(this.txtlastname);
		this.panel7.Controls.Add(this.txtzipcode);
		this.panel7.Controls.Add(this.txtstrnno);
		this.panel7.Controls.Add(this.label51);
		this.panel7.Controls.Add(this.txttitle);
		this.panel7.Controls.Add(this.label52);
		this.panel7.Controls.Add(this.txtofficemobile);
		this.panel7.Controls.Add(this.txttown);
		this.panel7.Controls.Add(this.label54);
		this.panel7.Controls.Add(this.label55);
		this.panel7.Controls.Add(this.label35);
		this.panel7.Controls.Add(this.label34);
		this.panel7.Controls.Add(this.chksubac);
		this.panel7.Controls.Add(this.label56);
		this.panel7.Controls.Add(this.txtcnicexp);
		this.panel7.Controls.Add(this.label32);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 240);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(964, 187);
		this.panel7.TabIndex = 1;
		this.txtfirstname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfirstname.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfirstname.Location = new System.Drawing.Point(90, 12);
		this.txtfirstname.Name = "txtfirstname";
		this.txtfirstname.Size = new System.Drawing.Size(188, 23);
		this.txtfirstname.TabIndex = 0;
		this.txtaddress1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtaddress1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtaddress1.Location = new System.Drawing.Point(90, 154);
		this.txtaddress1.Name = "txtaddress1";
		this.txtaddress1.Size = new System.Drawing.Size(439, 23);
		this.txtaddress1.TabIndex = 5;
		this.txtSupplierCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierCode.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierCode.Location = new System.Drawing.Point(607, 98);
		this.txtSupplierCode.Name = "txtSupplierCode";
		this.txtSupplierCode.Size = new System.Drawing.Size(145, 23);
		this.txtSupplierCode.TabIndex = 14;
		this.label43.AutoSize = true;
		this.label43.BackColor = System.Drawing.Color.White;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(289, 102);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(74, 15);
		this.label43.TabIndex = 214;
		this.label43.Text = "Website:URL";
		this.CmbAdvanceAc.AllowNull = (DefaultableBoolean)1;
		this.CmbAdvanceAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAdvanceAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAdvanceAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance58;
		((SpecialBoxBase)((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance63).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAdvanceAc).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAdvanceAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAdvanceAc).Location = new System.Drawing.Point(607, 124);
		((System.Windows.Forms.Control)(object)this.CmbAdvanceAc).Name = "CmbAdvanceAc";
		((System.Windows.Forms.Control)(object)this.CmbAdvanceAc).Size = new System.Drawing.Size(145, 26);
		((System.Windows.Forms.Control)(object)this.CmbAdvanceAc).TabIndex = 15;
		this.txtwebsite.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtwebsite.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtwebsite.Location = new System.Drawing.Point(368, 98);
		this.txtwebsite.Name = "txtwebsite";
		this.txtwebsite.Size = new System.Drawing.Size(161, 23);
		this.txtwebsite.TabIndex = 9;
		this.cmbparentac.AllowNull = (DefaultableBoolean)1;
		this.cmbparentac.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbparentac.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbparentac.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Appearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbparentac).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbparentac).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbparentac).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbparentac).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance8;
		((SpecialBoxBase)((UltraGridBase)this.cmbparentac).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbparentac).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbparentac).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbparentac).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance14).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbparentac).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbparentac).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbparentac).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbparentac).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbparentac).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbparentac.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbparentac).Location = new System.Drawing.Point(607, 152);
		((System.Windows.Forms.Control)(object)this.cmbparentac).Name = "cmbparentac";
		((System.Windows.Forms.Control)(object)this.cmbparentac).Size = new System.Drawing.Size(145, 26);
		((System.Windows.Forms.Control)(object)this.cmbparentac).TabIndex = 16;
		this.label44.AutoSize = true;
		this.label44.BackColor = System.Drawing.Color.White;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(289, 44);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(41, 15);
		this.label44.TabIndex = 209;
		this.label44.Text = "Phone";
		this.label45.AutoSize = true;
		this.label45.BackColor = System.Drawing.Color.White;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(536, 102);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(64, 15);
		this.label45.TabIndex = 212;
		this.label45.Text = "Party Code";
		this.label41.AutoSize = true;
		this.label41.BackColor = System.Drawing.Color.White;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(536, 44);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(42, 15);
		this.label41.TabIndex = 199;
		this.label41.Text = "NTN #";
		this.label40.AutoSize = true;
		this.label40.BackColor = System.Drawing.Color.White;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(6, 44);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(63, 15);
		this.label40.TabIndex = 202;
		this.label40.Text = "Last Name";
		this.label47.AutoSize = true;
		this.label47.BackColor = System.Drawing.Color.White;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(6, 158);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(49, 15);
		this.label47.TabIndex = 205;
		this.label47.Text = "Address";
		this.txtphone.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtphone.Location = new System.Drawing.Point(368, 40);
		this.txtphone.Mask = "(999) 000-0000";
		this.txtphone.Name = "txtphone";
		this.txtphone.Size = new System.Drawing.Size(161, 23);
		this.txtphone.TabIndex = 7;
		this.label48.AutoSize = true;
		this.label48.BackColor = System.Drawing.Color.White;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(6, 16);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(64, 15);
		this.label48.TabIndex = 201;
		this.label48.Text = "First Name";
		this.txtcnicno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcnicno.Location = new System.Drawing.Point(90, 98);
		this.txtcnicno.Margin = new System.Windows.Forms.Padding(2);
		this.txtcnicno.Mask = "00000-0000000-0";
		this.txtcnicno.Name = "txtcnicno";
		this.txtcnicno.Size = new System.Drawing.Size(188, 23);
		this.txtcnicno.TabIndex = 3;
		this.label49.AutoSize = true;
		this.label49.BackColor = System.Drawing.Color.White;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(536, 158);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(58, 15);
		this.label49.TabIndex = 198;
		this.label49.Text = "Parent Ac";
		this.label37.AutoSize = true;
		this.label37.BackColor = System.Drawing.Color.White;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(289, 73);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(79, 15);
		this.label37.TabIndex = 210;
		this.label37.Text = "Office Mobile";
		this.label8.AutoSize = true;
		this.label8.BackColor = System.Drawing.Color.White;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(536, 130);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(70, 15);
		this.label8.TabIndex = 217;
		this.label8.Text = "Advance Ac";
		this.txtntnno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtntnno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtntnno.Location = new System.Drawing.Point(607, 40);
		this.txtntnno.Name = "txtntnno";
		this.txtntnno.Size = new System.Drawing.Size(145, 23);
		this.txtntnno.TabIndex = 12;
		this.txtemail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtemail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtemail.Location = new System.Drawing.Point(368, 12);
		this.txtemail.Name = "txtemail";
		this.txtemail.Size = new System.Drawing.Size(161, 23);
		this.txtemail.TabIndex = 6;
		this.txtlastname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtlastname.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtlastname.Location = new System.Drawing.Point(90, 40);
		this.txtlastname.Name = "txtlastname";
		this.txtlastname.Size = new System.Drawing.Size(188, 23);
		this.txtlastname.TabIndex = 1;
		this.txtzipcode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtzipcode.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtzipcode.Location = new System.Drawing.Point(607, 69);
		this.txtzipcode.Name = "txtzipcode";
		this.txtzipcode.Size = new System.Drawing.Size(145, 23);
		this.txtzipcode.TabIndex = 13;
		this.txtstrnno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtstrnno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtstrnno.Location = new System.Drawing.Point(607, 12);
		this.txtstrnno.Name = "txtstrnno";
		this.txtstrnno.Size = new System.Drawing.Size(145, 23);
		this.txtstrnno.TabIndex = 11;
		this.label51.AutoSize = true;
		this.label51.BackColor = System.Drawing.Color.White;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(289, 16);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(39, 15);
		this.label51.TabIndex = 211;
		this.label51.Text = "Email ";
		this.txttitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txttitle.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txttitle.Location = new System.Drawing.Point(90, 69);
		this.txttitle.Name = "txttitle";
		this.txttitle.Size = new System.Drawing.Size(188, 23);
		this.txttitle.TabIndex = 2;
		this.label52.AutoSize = true;
		this.label52.BackColor = System.Drawing.Color.White;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(536, 73);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(54, 15);
		this.label52.TabIndex = 208;
		this.label52.Text = "Zip Code";
		this.txtofficemobile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtofficemobile.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtofficemobile.Location = new System.Drawing.Point(368, 69);
		this.txtofficemobile.Name = "txtofficemobile";
		this.txtofficemobile.Size = new System.Drawing.Size(161, 23);
		this.txtofficemobile.TabIndex = 8;
		this.txttown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txttown.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txttown.Location = new System.Drawing.Point(368, 126);
		this.txttown.Name = "txttown";
		this.txttown.Size = new System.Drawing.Size(161, 23);
		this.txttown.TabIndex = 10;
		this.label54.AutoSize = true;
		this.label54.BackColor = System.Drawing.Color.White;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(6, 130);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(69, 15);
		this.label54.TabIndex = 204;
		this.label54.Text = "CNIC Expiry";
		this.label55.AutoSize = true;
		this.label55.BackColor = System.Drawing.Color.White;
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.Location = new System.Drawing.Point(6, 73);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(30, 15);
		this.label55.TabIndex = 213;
		this.label55.Text = "Title";
		this.label35.AutoSize = true;
		this.label35.BackColor = System.Drawing.Color.White;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(536, 16);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(47, 15);
		this.label35.TabIndex = 200;
		this.label35.Text = "STRN #";
		this.label34.AutoSize = true;
		this.label34.BackColor = System.Drawing.Color.White;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(6, 102);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(81, 15);
		this.label34.TabIndex = 203;
		this.label34.Text = "CNIC Number";
		this.chksubac.AutoSize = true;
		this.chksubac.ForeColor = System.Drawing.Color.White;
		this.chksubac.Location = new System.Drawing.Point(835, 158);
		this.chksubac.Name = "chksubac";
		this.chksubac.Size = new System.Drawing.Size(29, 17);
		this.chksubac.TabIndex = 17;
		this.chksubac.Text = " ";
		this.chksubac.UseVisualStyleBackColor = true;
		this.chksubac.Click += new System.EventHandler(chksubac_Click);
		this.label56.AutoSize = true;
		this.label56.BackColor = System.Drawing.Color.White;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.Location = new System.Drawing.Point(756, 159);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(76, 15);
		this.label56.TabIndex = 17;
		this.label56.Text = "Sub Account";
		this.txtcnicexp.CustomFormat = "dd-MM-yy";
		this.txtcnicexp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcnicexp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtcnicexp.Location = new System.Drawing.Point(90, 126);
		this.txtcnicexp.Name = "txtcnicexp";
		this.txtcnicexp.Size = new System.Drawing.Size(188, 23);
		this.txtcnicexp.TabIndex = 4;
		this.label32.AutoSize = true;
		this.label32.BackColor = System.Drawing.Color.White;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(289, 130);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(36, 15);
		this.label32.TabIndex = 207;
		this.label32.Text = "Town";
		this.panel6.Controls.Add(this.txtPartyCode);
		this.panel6.Controls.Add(this.label15);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPartyType);
		this.panel6.Controls.Add(this.label13);
		this.panel6.Controls.Add(this.btnMoreInfo);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDiscountType);
		this.panel6.Controls.Add(this.label9);
		this.panel6.Controls.Add(this.txtpermobile);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.cmbgroupdetail);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcity);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcountry);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.cmbglac);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.cmbprovince);
		this.panel6.Controls.Add(this.btnAdd);
		this.panel6.Controls.Add(this.label14);
		this.panel6.Controls.Add(this.label4);
		this.panel6.Controls.Add(this.browse);
		this.panel6.Controls.Add(this.box);
		this.panel6.Controls.Add(this.chkactivestatus);
		this.panel6.Controls.Add(this.label57);
		this.panel6.Controls.Add(this.label31);
		this.panel6.Controls.Add(this.label36);
		this.panel6.Controls.Add(this.txtcompany);
		this.panel6.Controls.Add(this.label53);
		this.panel6.Controls.Add(this.txtaddress2);
		this.panel6.Controls.Add(this.label38);
		this.panel6.Controls.Add(this.label39);
		this.panel6.Controls.Add(this.label46);
		this.panel6.Controls.Add(this.label42);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Font = new System.Drawing.Font("Segoe UI", 9f);
		this.panel6.Location = new System.Drawing.Point(0, 53);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(964, 187);
		this.panel6.TabIndex = 0;
		this.txtPartyCode.Enabled = false;
		this.txtPartyCode.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPartyCode.Location = new System.Drawing.Point(91, 38);
		this.txtPartyCode.Name = "txtPartyCode";
		this.txtPartyCode.Size = new System.Drawing.Size(236, 23);
		this.txtPartyCode.TabIndex = 1;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.White;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(8, 42);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(64, 15);
		this.label15.TabIndex = 225;
		this.label15.Text = "Party Code";
		this.cmbPartyType.AllowNull = (DefaultableBoolean)1;
		this.cmbPartyType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPartyType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPartyType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPartyType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance74;
		((SpecialBoxBase)((UltraGridBase)this.cmbPartyType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPartyType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPartyType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPartyType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPartyType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPartyType).Location = new System.Drawing.Point(91, 8);
		((System.Windows.Forms.Control)(object)this.cmbPartyType).Name = "cmbPartyType";
		((System.Windows.Forms.Control)(object)this.cmbPartyType).Size = new System.Drawing.Size(236, 26);
		((System.Windows.Forms.Control)(object)this.cmbPartyType).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbPartyType).Leave += new System.EventHandler(cmbPartyType_Leave);
		this.label13.AutoSize = true;
		this.label13.BackColor = System.Drawing.Color.White;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(8, 14);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(62, 15);
		this.label13.TabIndex = 223;
		this.label13.Text = "Party Type";
		this.btnMoreInfo.BackColor = System.Drawing.Color.Teal;
		this.btnMoreInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnMoreInfo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnMoreInfo.ForeColor = System.Drawing.Color.White;
		this.btnMoreInfo.Location = new System.Drawing.Point(111, 157);
		this.btnMoreInfo.Name = "btnMoreInfo";
		this.btnMoreInfo.Size = new System.Drawing.Size(32, 25);
		this.btnMoreInfo.TabIndex = 12;
		this.btnMoreInfo.Text = "-";
		this.btnMoreInfo.UseVisualStyleBackColor = false;
		this.btnMoreInfo.Click += new System.EventHandler(btnMoreInfo_Click);
		this.cmbDiscountType.AllowNull = (DefaultableBoolean)1;
		this.cmbDiscountType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDiscountType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDiscountType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDiscountType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance77;
		((SpecialBoxBase)((UltraGridBase)this.cmbDiscountType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance99).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance3).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDiscountType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDiscountType).Enabled = false;
		((System.Windows.Forms.Control)(object)this.cmbDiscountType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDiscountType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDiscountType).Location = new System.Drawing.Point(425, 8);
		((System.Windows.Forms.Control)(object)this.cmbDiscountType).Name = "cmbDiscountType";
		((System.Windows.Forms.Control)(object)this.cmbDiscountType).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.cmbDiscountType).TabIndex = 5;
		this.label9.AutoSize = true;
		this.label9.BackColor = System.Drawing.Color.White;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(335, 14);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(83, 15);
		this.label9.TabIndex = 219;
		this.label9.Text = "Discount Type";
		this.txtpermobile.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtpermobile.Location = new System.Drawing.Point(425, 94);
		this.txtpermobile.Name = "txtpermobile";
		this.txtpermobile.Size = new System.Drawing.Size(165, 23);
		this.txtpermobile.TabIndex = 8;
		this.cmbgroupdetail.AllowNull = (DefaultableBoolean)1;
		this.cmbgroupdetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbgroupdetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbgroupdetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbgroupdetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance55;
		((SpecialBoxBase)((UltraGridBase)this.cmbgroupdetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance43).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbgroupdetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbgroupdetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbgroupdetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbgroupdetail).Location = new System.Drawing.Point(91, 122);
		((System.Windows.Forms.Control)(object)this.cmbgroupdetail).Name = "cmbgroupdetail";
		((System.Windows.Forms.Control)(object)this.cmbgroupdetail).Size = new System.Drawing.Size(236, 26);
		((System.Windows.Forms.Control)(object)this.cmbgroupdetail).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.cmbgroupdetail).Leave += new System.EventHandler(cmbgroupdetail_Leave);
		this.cmbcity.AllowNull = (DefaultableBoolean)1;
		this.cmbcity.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbcity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance97;
		((SpecialBoxBase)((UltraGridBase)this.cmbcity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance98).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance98).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance98).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbcity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance103).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance104).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance106).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbcity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcity).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcity).Location = new System.Drawing.Point(425, 64);
		((System.Windows.Forms.Control)(object)this.cmbcity).Name = "cmbcity";
		((System.Windows.Forms.Control)(object)this.cmbcity).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.cmbcity).TabIndex = 7;
		this.cmbcountry.AllowNull = (DefaultableBoolean)1;
		this.cmbcountry.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcountry.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcountry.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Appearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbcountry).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcountry).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcountry).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcountry).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.cmbcountry).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcountry).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbcountry).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcountry).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance89).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbcountry).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcountry).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcountry).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcountry).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcountry.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcountry).Location = new System.Drawing.Point(425, 36);
		((System.Windows.Forms.Control)(object)this.cmbcountry).Name = "cmbcountry";
		((System.Windows.Forms.Control)(object)this.cmbcountry).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.cmbcountry).TabIndex = 6;
		this.cmbglac.AllowNull = (DefaultableBoolean)1;
		this.cmbglac.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbglac.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbglac.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbglac).DisplayLayout.Appearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbglac).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbglac).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbglac).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbglac).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance51;
		((SpecialBoxBase)((UltraGridBase)this.cmbglac).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbglac).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbglac).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbglac).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance68).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance69).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance69;
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
		((System.Windows.Forms.Control)(object)this.cmbglac).Location = new System.Drawing.Point(91, 64);
		((System.Windows.Forms.Control)(object)this.cmbglac).Name = "cmbglac";
		((System.Windows.Forms.Control)(object)this.cmbglac).Size = new System.Drawing.Size(236, 26);
		((System.Windows.Forms.Control)(object)this.cmbglac).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbglac).Leave += new System.EventHandler(cmbglac_Leave);
		this.cmbprovince.AllowNull = (DefaultableBoolean)2;
		this.cmbprovince.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbprovince.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbprovince.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbprovince).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbprovince).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbprovince).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbprovince).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.cmbprovince).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbprovince).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbprovince).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbprovince).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance34).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance35).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbprovince).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbprovince).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbprovince).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbprovince).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbprovince.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbprovince).Location = new System.Drawing.Point(448, 458);
		((System.Windows.Forms.Control)(object)this.cmbprovince).Name = "cmbprovince";
		((System.Windows.Forms.Control)(object)this.cmbprovince).Size = new System.Drawing.Size(188, 24);
		((System.Windows.Forms.Control)(object)this.cmbprovince).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.cmbprovince).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbprovince).Visible = false;
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(520, 123);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(70, 25);
		this.btnAdd.TabIndex = 10;
		this.btnAdd.Text = "Save";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(9, 160);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(96, 19);
		this.label14.TabIndex = 190;
		this.label14.Text = "More Details";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.Red;
		this.label4.Location = new System.Drawing.Point(421, 459);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(13, 16);
		this.label4.TabIndex = 171;
		this.label4.Text = "*";
		this.label4.Visible = false;
		this.browse.BackColor = System.Drawing.Color.Teal;
		this.browse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.browse.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.browse.ForeColor = System.Drawing.Color.White;
		this.browse.Location = new System.Drawing.Point(714, 156);
		this.browse.Name = "browse";
		this.browse.Size = new System.Drawing.Size(75, 26);
		this.browse.TabIndex = 11;
		this.browse.Text = "Browse";
		this.browse.UseVisualStyleBackColor = false;
		this.browse.Click += new System.EventHandler(browse_Click);
		this.box.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.box.Location = new System.Drawing.Point(607, 8);
		this.box.Name = "box";
		this.box.Size = new System.Drawing.Size(288, 143);
		this.box.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.box.TabIndex = 215;
		this.box.TabStop = false;
		this.chkactivestatus.AutoSize = true;
		this.chkactivestatus.Checked = true;
		this.chkactivestatus.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkactivestatus.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkactivestatus.ForeColor = System.Drawing.Color.Black;
		this.chkactivestatus.Location = new System.Drawing.Point(425, 123);
		this.chkactivestatus.Name = "chkactivestatus";
		this.chkactivestatus.Size = new System.Drawing.Size(71, 19);
		this.chkactivestatus.TabIndex = 9;
		this.chkactivestatus.Text = "Is Active";
		this.chkactivestatus.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
		this.chkactivestatus.UseVisualStyleBackColor = true;
		this.label57.AutoSize = true;
		this.label57.BackColor = System.Drawing.Color.White;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.Location = new System.Drawing.Point(8, 98);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(74, 15);
		this.label57.TabIndex = 154;
		this.label57.Text = "Comp Name";
		this.label31.AutoSize = true;
		this.label31.BackColor = System.Drawing.Color.White;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(335, 98);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(51, 15);
		this.label31.TabIndex = 179;
		this.label31.Text = "Mobile#";
		this.label36.AutoSize = true;
		this.label36.BackColor = System.Drawing.Color.White;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(8, 128);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(70, 15);
		this.label36.TabIndex = 192;
		this.label36.Text = "Party Group";
		this.txtcompany.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcompany.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcompany.Location = new System.Drawing.Point(91, 94);
		this.txtcompany.Name = "txtcompany";
		this.txtcompany.Size = new System.Drawing.Size(236, 23);
		this.txtcompany.TabIndex = 3;
		this.label53.AutoSize = true;
		this.label53.BackColor = System.Drawing.Color.White;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(335, 42);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(84, 15);
		this.label53.TabIndex = 163;
		this.label53.Text = "Country Name";
		this.txtaddress2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtaddress2.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtaddress2.Location = new System.Drawing.Point(794, 458);
		this.txtaddress2.Name = "txtaddress2";
		this.txtaddress2.Size = new System.Drawing.Size(188, 25);
		this.txtaddress2.TabIndex = 18;
		this.txtaddress2.TabStop = false;
		this.txtaddress2.Visible = false;
		this.label38.AutoSize = true;
		this.label38.BackColor = System.Drawing.Color.White;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(335, 70);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(62, 15);
		this.label38.TabIndex = 173;
		this.label38.Text = "City Name";
		this.label39.AutoSize = true;
		this.label39.BackColor = System.Drawing.Color.White;
		this.label39.Font = new System.Drawing.Font("Segoe UI", 9f);
		this.label39.Location = new System.Drawing.Point(706, 461);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(58, 15);
		this.label39.TabIndex = 206;
		this.label39.Text = "Address 2";
		this.label39.Visible = false;
		this.label46.AutoSize = true;
		this.label46.BackColor = System.Drawing.Color.White;
		this.label46.Font = new System.Drawing.Font("Segoe UI", 9f);
		this.label46.Location = new System.Drawing.Point(362, 459);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(53, 15);
		this.label46.TabIndex = 168;
		this.label46.Text = "Province";
		this.label46.Visible = false;
		this.label42.AutoSize = true;
		this.label42.BackColor = System.Drawing.Color.White;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(8, 70);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(66, 15);
		this.label42.TabIndex = 158;
		this.label42.Text = "Gl Account";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label20);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 25);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(964, 28);
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
		this.panel2.Size = new System.Drawing.Size(964, 25);
		this.panel2.TabIndex = 0;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.btnnew, this.btnupdate, this.btnsave, this.btn293Print, this.btnPrint, this.btnBankDetail, this.btnShipToAddress, this.btnSupplierLimits, this.toolStripButton1 });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(964, 27);
		this.toolStrip2.TabIndex = 0;
		this.toolStrip2.Text = "toolStrip2";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btn293Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btn293Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btn293Print.Name = "btn293Print";
		this.btn293Print.Size = new System.Drawing.Size(85, 24);
		this.btn293Print.Text = "293-Slip";
		this.btn293Print.Click += new System.EventHandler(btn293Print_Click);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(115, 24);
		this.btnPrint.Text = "292-Register";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
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
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(964, 661);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "supfrmDefineSupplier";
		this.Text = "Eccountbook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(supfrmDefineSupplier_FormClosing);
		base.Load += new System.EventHandler(supfrmDefineSupplier1_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(supfrmDefineSupplier1_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAdvanceAc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbparentac).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPartyType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDiscountType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbgroupdetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcountry).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbglac).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbprovince).EndInit();
		((System.ComponentModel.ISupportInitialize)this.box).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
