// using Architecture.Reports;
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
using Architecture.Model.Main;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
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
namespace Architecture.WinApp.Export
{

public class ExportPartyDefines : Form
{
	private int RecId;

	private int RecIdConsignee;

	private bool FormInitialized;

	private bool Confiquration;

	private string saveDirectory = "";

	private string fileSavePath = "";

	private string ofd = "";

	private Rightsobjects formright = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private DataTable table = new DataTable();

	private DataTable dtCountry = new DataTable();

	private DataTable dtProvince = new DataTable();

	private DataTable dtCity = new DataTable();

	private DataTable dtGridForLayout = new DataTable();

	private DataTable dtGrid = new DataTable();

	private DataTable dtGridForLayoutConsignee = new DataTable();

	private DataTable dtGridConsignee = new DataTable();

	public UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel5;

	private Label label20;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStrip toolStrip2;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private ToolStripButton btnPrint;

	private Panel panel6;

	private Button btnAdd;

	private Label label14;

	private Label label2;

	private Label label1;

	private Button browse;

	private PictureBox box;

	private CheckBox chkactivestatus;

	private Label label57;

	private DateTimePicker txtcnicexp;

	private Label label31;

	private Label label32;

	private Label label33;

	private Label label34;

	private Label label35;

	private Label label55;

	private Label label54;

	private TextBox txtcompany;

	private TextBox txttown;

	private Label label53;

	private TextBox txtofficemobile;

	private Label label52;

	private TextBox txttitle;

	private Label label51;

	private TextBox txtstrnno;

	private TextBox txtzipcode;

	private TextBox txtlastname;

	private TextBox txtemail;

	private TextBox txtntnno;

	private Label label37;

	private Label label38;

	private Label label48;

	private Label label47;

	private Label label40;

	private Label label41;

	private Label label42;

	private Label label44;

	private TextBox txtwebsite;

	private Label label43;

	private TextBox txtfirstname;

	private TextBox txtaddress1;

	internal GridEX grdfrm;

	private ToolStripButton btnBankDetail;

	private ToolStripButton btnShipToAddress;

	private ToolStripButton btnSupplierLimits;

	internal UltraCombo CmbAdvanceAc;

	private Label label8;

	private MaskedTextBox txtcnicno;

	private MaskedTextBox txtphone;

	private MaskedTextBox txtpermobile;

	internal UltraCombo cmbprovince;

	private Label label9;

	private Panel panel8;

	private Label label10;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnShortCutKeys;

	internal UltraCombo cmbglac;

	internal UltraCombo cmbcountry;

	internal UltraCombo cmbcity;

	private Label label3;

	private Button btnUpdateRecordsFromGrid;

	private Button btnDefineCountry;

	private Button btnDefineProvince;

	private Button btnDefineCity;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	internal UltraCombo CmbParentAcConsignee;

	private Label label13;

	private Label label5;

	private Label label11;

	private TextBox txtConsigneeName;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNewConsignee;

	private ToolStripButton btnRefreshConsignee;

	private ToolStripButton btnUpdateConsignee;

	private ToolStripButton btnSaveConsignee;

	private ToolStripButton btnPrintConsignee;

	private ToolStripButton btnShortCutKeysConsignee;

	private Panel panel2;

	private Panel panel1;

	private Label label23;

	private Label label22;

	private Button btnDefineCityConsignee;

	internal UltraCombo CmbCityNameConsignee;

	private Label label12;

	private Label label15;

	private Label label16;

	private TextBox txtStrnNoConsignee;

	private TextBox ode;

	private TextBox txtEmailConsignee;

	private TextBox txtNtnNoConsignee;

	private Label label17;

	private Label label18;

	private Label label19;

	private TextBox txtWebPageConsignee;

	private Label label21;

	private TextBox txtAddressConsignee;

	private Button btnCountryDefineConsignee;

	internal UltraCombo CmbCountryNameConsignee;

	private Label label6;

	private GroupBox groupBox1;

	private CheckBox ChkConsigneeStatus;

	private Panel panel3;

	private Button btnMultiRecordsUpdateConsignee;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label24;

	internal GridEX grdExConsignee;

	private TextBox txtZipCodeConsignee;

	public ExportPartyDefines(UserAccount puserAccount)
	{
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = puserAccount;
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		try
		{
			btnsave.Visible = true;
			Reset();
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
			if (!formvalidation())
			{
				return;
			}
			SupplierCustomer supcus = new SupplierCustomer();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				supcus.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			supcus.CompanyName = Conversion.ToString((object)txtcompany.Text.Trim());
			supcus.ReportingTitle = Conversion.ToString((object)txtcompany.Text.Trim());
			supcus.GlAccountId = Conversion.ToInt(cmbglac.Value);
			supcus.CustomerGroupId = 7;
			supcus.NTN_No = Conversion.ToString((object)txtntnno.Text.Trim());
			supcus.STRN_No = Conversion.ToString((object)txtstrnno.Text.Trim());
			supcus.Title = Conversion.ToString((object)txttitle.Text.Trim());
			supcus.FirstName = Conversion.ToString((object)txtfirstname.Text.Trim());
			supcus.LastName = Conversion.ToString((object)txtlastname.Text.Trim());
			supcus.CNIC = Conversion.ToString((object)txtcnicno.Text.Trim());
			supcus.CNIC_EXPIRY_DATE = Conversion.ToDateTime((object)txtcnicexp.Value);
			supcus.Address1 = Conversion.ToString((object)txtaddress1.Text.Trim());
			supcus.Status = Conversion.ToBool((object)chkactivestatus.Text.Trim());
			supcus.CountryId = Conversion.ToInt(cmbcountry.Value);
			supcus.CityId = Conversion.ToInt(cmbcity.Value);
			supcus.Town = Conversion.ToString((object)txttown.Text.Trim());
			supcus.ZipCode = Conversion.ToString((object)txtzipcode.Text.Trim());
			supcus.Phone = Conversion.ToString((object)txtphone.Text.Trim());
			supcus.MobileOffice = Conversion.ToString((object)txtofficemobile.Text.Trim());
			supcus.MobilePersonal = Conversion.ToString((object)txtpermobile.Text.Trim());
			supcus.Email = Conversion.ToString((object)txtemail.Text.Trim());
			supcus.WebPage = Conversion.ToString((object)txtwebsite.Text.Trim());
			supcus.AdvanceGlAcId = Conversion.ToInt(CmbAdvanceAc.Value);
			supcus.EntryUser = UserAccount.ID;
			supcus.ModifyUser = UserAccount.ID;
			supcus.OrganizationId = UserAccount.OrganizationId;
			supcus.CompanyId = UserAccount.CompanyId;
			supcus.AdvanceGlAcId = Conversion.ToInt(CmbAdvanceAc.Value);
			supcus.StateProvinceId = Conversion.ToInt(cmbprovince.Value);
			supcus.ParentsSupCustId = 0;
			supcus.IsSubSupCust = false;
			supcus.PictureURL = Path.GetFileName(fileSavePath);
			int sucess = SupplierCustomer.Save(supcus);
			supcus.Id = sucess;
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully.... ");
			}
			else
			{
				MessageBox.Show("Data Save Successfully.... ");
			}
			if (ofd != null && ofd != string.Empty)
			{
				File.Copy(ofd, fileSavePath, overwrite: true);
				ofd = string.Empty;
				fileSavePath = string.Empty;
				box.Image = null;
				box.Invalidate();
			}
			datagridviewform();
			Reset();
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

	private void cmbglacfill(int GlrecId = 0)
	{
		try
		{
			int id = Conversion.ToInt(cmbglac.Value);
			int idAdv = Conversion.ToInt(CmbAdvanceAc.Value);
			DataTable dt = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("22", null, 0, "", "", 1, GlrecId);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbglac, "Id", "AccountTitle", "GL Account", ZeroIndex: true);
				((UltraGridBase)cmbglac).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				DDL.BindDDL(dt, CmbAdvanceAc, "Id", "AccountTitle", "Account Title", ZeroIndex: true);
				((UltraGridBase)cmbglac).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)CmbAdvanceAc).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				CommonServices.SetComboValue(cmbglac, "Id", id, dt);
				CommonServices.SetComboValue(CmbAdvanceAc, "Id", idAdv, dt);
			}
			else
			{
				((Control)(object)cmbglac).Text = string.Empty;
				((Control)(object)CmbAdvanceAc).Text = string.Empty;
				((UltraGridBase)cmbglac).DataSource = null;
				((UltraGridBase)CmbAdvanceAc).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Provincefill()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		try
		{
			int id = Conversion.ToInt(cmbprovince.Value);
			dtProvince.Rows.Clear();
			StateProvince obj = new StateProvince();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtProvince = StateProvince.GetAll(obj);
			if (dtProvince.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtProvince, cmbprovince, "Id", "Description", "Province", true);
				((UltraGridBase)cmbprovince).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				CommonServices.SetComboValue(cmbprovince, "Id", id, dtProvince);
			}
			else
			{
				((Control)(object)cmbprovince).Text = string.Empty;
				((UltraGridBase)cmbprovince).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ParentAccountfill()
	{
		try
		{
			int Consignee = Conversion.ToInt(CmbParentAcConsignee.Value);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("CompanyName", typeof(string));
			dt.Columns.Add("PartyCode", typeof(string));
			dt.Columns.Add("GlAccountId", typeof(int));
			List<getGlobalAllSupplierCustomer> list = clsGlobalVariables.globalAllSupplierCustomer.Where((getGlobalAllSupplierCustomer r) => r.CustomerGroupId == 7).ToList();
			if (list == null || list.Count == 0)
			{
				return;
			}
			foreach (getGlobalAllSupplierCustomer item in list)
			{
				object[] rowData = new object[4]
				{
					Conversion.ToInt((object)item.Id),
					Conversion.ToString((object)item.CompanyName),
					Conversion.ToString((object)item.PartyCode),
					Conversion.ToInt((object)item.GlAccountId)
				};
				dt.Rows.Add(rowData);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbParentAcConsignee, "Id", "CompanyName", "Parent Ac", ZeroIndex: true);
				CommonServices.SetComboValue(CmbParentAcConsignee, "Id", Consignee, dt);
			}
			else
			{
				((Control)(object)CmbParentAcConsignee).Text = string.Empty;
				((UltraGridBase)CmbParentAcConsignee).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbcountryfill()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			dtCountry.Rows.Clear();
			int id = Conversion.ToInt(cmbcountry.Value);
			int ConsCountryId = Conversion.ToInt(CmbCountryNameConsignee.Value);
			Country country = new Country();
			country.OrganizationId = UserAccount.OrganizationId;
			country.CompanyId = UserAccount.CompanyId;
			dtCountry = country.GetAll(country);
			if (dtCountry.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("Description", typeof(string));
				for (int i = 0; i < dtCountry.Rows.Count; i++)
				{
					dt.Rows.Add(dtCountry.Rows[i]["Id"], dtCountry.Rows[i]["Description"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbcountry, "Id", "Description", "Country Name", ZeroIndex: true);
					DDL.BindDDL(dt, CmbCountryNameConsignee, "Id", "Description", "Country Name", ZeroIndex: true);
					((UltraGridBase)cmbcountry).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
					((UltraGridBase)CmbCountryNameConsignee).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
					CommonServices.SetComboValue(cmbcountry, "Id", id, dt);
					CommonServices.SetComboValue(CmbCountryNameConsignee, "Id", ConsCountryId, dt);
				}
			}
			else
			{
				((Control)(object)cmbcountry).Text = string.Empty;
				((Control)(object)CmbCountryNameConsignee).Text = string.Empty;
				((UltraGridBase)cmbcountry).DataSource = null;
				((UltraGridBase)CmbCountryNameConsignee).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CityFill()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			dtCity.Rows.Clear();
			int Id = Conversion.ToInt(cmbcity.Value);
			int ConsigneeCityId = Conversion.ToInt(CmbCityNameConsignee.Value);
			City obj = new City();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtCity = City.GetAll(obj);
			if (dtCity.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCity, cmbcity, "Id", "CityName", "City Name", true);
				((UltraGridBase)cmbcity).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				CommonServices.SetComboValue(cmbcity, "Id", Id, dtCity);
				DDL.BindDDLNew(dtCity, CmbCityNameConsignee, "Id", "CityName", "City Name", true);
				((UltraGridBase)CmbCityNameConsignee).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				CommonServices.SetComboValue(CmbCityNameConsignee, "Id", ConsigneeCityId, dtCity);
			}
			else
			{
				((Control)(object)cmbcity).Text = string.Empty;
				((Control)(object)CmbCityNameConsignee).Text = string.Empty;
				((UltraGridBase)cmbcity).DataSource = null;
				((UltraGridBase)CmbCityNameConsignee).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbcountry_Leave(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)cmbcity).Enabled = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void datagridviewform()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtGrid.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.CustomerGroupId = 7;
			dtGrid = SupplierCustomer.FormHistory(obj);
			if (dtGrid.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("ParentsSupCustId", typeof(int));
				dt.Columns.Add("CustomerGroupId", typeof(int));
				dt.Columns.Add("GlAccountId", typeof(int));
				dt.Columns.Add("GlAccount", typeof(string));
				dt.Columns.Add("CompanyName", typeof(string));
				dt.Columns.Add("PersonName", typeof(string));
				dt.Columns.Add("Mobile", typeof(string));
				dt.Columns.Add("Country", typeof(int));
				dt.Columns.Add("Province", typeof(int));
				dt.Columns.Add("City", typeof(int));
				dt.Columns.Add("Email", typeof(string));
				dt.Columns.Add("EORI", typeof(string));
				dt.Columns.Add("VATNo", typeof(string));
				dt.Columns.Add("ZipCode", typeof(string));
				dt.Columns.Add("WebPage", typeof(string));
				dt.Columns.Add("Address", typeof(string));
				dt.Columns.Add("ParentAccountTitle", typeof(string));
				dt.Columns.Add("Status", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				dt.Columns.Add("PictureName", typeof(string));
				dt.Rows.Clear();
				for (int i = 0; i < dtGrid.Rows.Count; i++)
				{
					dt.Rows.Add(dtGrid.Rows[i]["Id"], dtGrid.Rows[i]["ParentsSupCustId"], dtGrid.Rows[i]["CustomerGroupId"], dtGrid.Rows[i]["GlAccountId"], dtGrid.Rows[i]["AccountTitle"], dtGrid.Rows[i]["CompanyName"], dtGrid.Rows[i]["FirstName"], dtGrid.Rows[i]["MobilePersonal"], dtGrid.Rows[i]["CountryId"], dtGrid.Rows[i]["StateProvinceId"], dtGrid.Rows[i]["CityId"], dtGrid.Rows[i]["Email"], dtGrid.Rows[i]["STRN_No"], dtGrid.Rows[i]["NTN_No"], dtGrid.Rows[i]["ZipCode"], dtGrid.Rows[i]["WebPage"], dtGrid.Rows[i]["Address1"], dtGrid.Rows[i]["ParentAccountTitle"], dtGrid.Rows[i]["Status"], dtGrid.Rows[i]["EntryDate"], dtGrid.Rows[i]["EntryUserName"], dtGrid.Rows[i]["ModifyDate"], dtGrid.Rows[i]["ModifyUserName"], dtGrid.Rows[i]["PictureName"]);
				}
				grdfrm.DataSource = dt;
				dtGridForLayout = dt.Copy();
				grdfrm.RetrieveStructure();
				GridSetting();
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
			txtaddress1.Clear();
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
			((Control)(object)CmbAdvanceAc).Text = string.Empty;
			((Control)(object)cmbprovince).Text = string.Empty;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			cmbglac.Focus();
			box.Image = null;
			box.Invalidate();
			cmbglacfill();
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
			grdfrm.AllowEdit = (InheritableBoolean)1;
			grdfrm.RootTable.RowHeight = 30;
			List<string> hiddenColumns = new List<string> { "Id", "ParentsSupCustId", "CustomerGroupId", "GlAccountId", "PictureName" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"CompanyName",
					(EditType)1
				},
				{
					"PersonName",
					(EditType)1
				},
				{
					"Mobile",
					(EditType)1
				},
				{
					"Country",
					(EditType)5
				},
				{
					"Province",
					(EditType)5
				},
				{
					"City",
					(EditType)5
				},
				{
					"Email",
					(EditType)1
				},
				{
					"EORI",
					(EditType)1
				},
				{
					"VATNo",
					(EditType)1
				},
				{
					"ZipCode",
					(EditType)1
				},
				{
					"WebPage",
					(EditType)1
				},
				{
					"Address",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdfrm, 2, 3, hiddenColumns, editableColumns);
			grdfrm.RootTable.Columns["CompanyName"].Caption = "Party Name";
			grdfrm.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdfrm.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdfrm.RootTable.Columns["GlAccount"].Width = Constants.ExportConstants.PartyName;
			grdfrm.RootTable.Columns["CompanyName"].Width = Constants.ExportConstants.PartyName;
			grdfrm.RootTable.Columns["PersonName"].Width = Constants.ExportConstants.PartyName;
			grdfrm.RootTable.Columns["Mobile"].Width = Constants.ExportConstants.MobileNo;
			grdfrm.RootTable.Columns["Country"].Width = Constants.ExportConstants.Country;
			grdfrm.RootTable.Columns["Province"].Width = Constants.ExportConstants.Province;
			grdfrm.RootTable.Columns["City"].Width = Constants.ExportConstants.City;
			grdfrm.RootTable.Columns["Email"].Width = Constants.ExportConstants.Email;
			grdfrm.RootTable.Columns["EORI"].Width = Constants.ExportConstants.StringConstant;
			grdfrm.RootTable.Columns["VATNo"].Width = Constants.ExportConstants.StringConstant;
			grdfrm.RootTable.Columns["ZipCode"].Width = Constants.ExportConstants.StringConstant;
			grdfrm.RootTable.Columns["WebPage"].Width = Constants.ExportConstants.StringConstant;
			grdfrm.RootTable.Columns["Address"].Width = Constants.ExportConstants.StringConstant;
			grdfrm.RootTable.Columns["ParentAccountTitle"].Width = Constants.ExportConstants.StringConstant;
			grdfrm.RootTable.Columns["EntryDate"].Width = Constants.ExportConstants.DateTimeConstantddMMM;
			grdfrm.RootTable.Columns["EntryUser"].Width = Constants.ExportConstants.EntryUserConstants;
			grdfrm.RootTable.Columns["ModifyDate"].Width = Constants.ExportConstants.DateTimeConstantddMMM;
			grdfrm.RootTable.Columns["ModifyUser"].Width = Constants.ExportConstants.ModifyUserConstants;
			grdfrm.RootTable.Columns["GlAccount"].WordWrap = true;
			grdfrm.RootTable.Columns["CompanyName"].WordWrap = true;
			grdfrm.RootTable.Columns["PersonName"].WordWrap = true;
			grdfrm.RootTable.Columns["Address"].WordWrap = true;
			grdfrm.RootTable.Columns["ParentAccountTitle"].WordWrap = true;
			grdComboBind();
			grdfrm.RootTable.Columns["Id"].Visible = false;
			grdfrm.RootTable.Columns["CustomerGroupId"].Visible = false;
			((Control)(object)grdfrm).Refresh();
			ctrlGrdBar1_Load(null, null);
			GridEX_Helper.AddButton(grdfrm, "Update", "Update", 50);
			if (formright.DoHavePrintRights)
			{
				GridEX_Helper.AddButton(grdfrm, "Print", "Print", 50, 1);
				grdfrm.FrozenColumns = 2;
			}
			else
			{
				grdfrm.FrozenColumns = 1;
			}
			if (formright.DoHaveUpdateRights)
			{
				GridEX_Helper.AddSelectButton(grdfrm, "Select", 40, 0);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdComboBind()
	{
		try
		{
			GridEX_Helper.ComboBind(grdfrm.RootTable.Columns["Country"], dtCountry, "Id", "Description", "Country");
			GridEX_Helper.ComboBind(grdfrm.RootTable.Columns["Province"], dtProvince, "Id", "Description", "Province");
			GridEX_Helper.ComboBind(grdfrm.RootTable.Columns["City"], dtCity, "Id", "CityName", "City");
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
			if (((Control)(object)cmbglac).Text.Trim() == "")
			{
				MessageBox.Show("GL Account Field is Required!!!");
				cmbglac.Focus();
				return false;
			}
			if (txtcompany.Text.Trim() == "")
			{
				MessageBox.Show("Company Name Field is Required!!!");
				txtcompany.Focus();
				return false;
			}
			if (txtfirstname.Text.Trim() == "")
			{
				MessageBox.Show("Person Name Field is Required!!!");
				txtfirstname.Focus();
				return false;
			}
			if (!chkactivestatus.Checked && RecId == 0)
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

	private bool formValidationConsignee()
	{
		try
		{
			if (((Control)(object)CmbParentAcConsignee).Text.Trim() == "")
			{
				MessageBox.Show("Parent Account Field is Required!!!");
				CmbParentAcConsignee.Focus();
				return false;
			}
			if (txtConsigneeName.Text.Trim() == "")
			{
				MessageBox.Show("Consignee Name Field is Required!!!");
				txtcompany.Focus();
				return false;
			}
			if (!ChkConsigneeStatus.Checked && RecIdConsignee == 0)
			{
				MessageBox.Show("Please check the Active Status!!!");
				ChkConsigneeStatus.Focus();
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
			base.AutoScrollMinSize = new Size(1052, 738);
			FormInitialized = true;
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnupdate.Enabled = formright.DoHaveUpdateRights;
			btnPrint.Enabled = formright.DoHavePrintRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			cmbglacfill();
			ParentAccountfill();
			cmbcountryfill();
			Provincefill();
			CityFill();
			datagridviewform();
			ConsigneeHistory();
			cmbglac.Focus();
			tabControl1.SelectedIndex = 0;
			btnUpdateConsignee.Visible = false;
			btnupdate.Visible = false;
			((Control)(object)cmbcity).Enabled = true;
			btnUpdateRecordsFromGrid.Visible = formright.DoHaveUpdateRights;
			btnMultiRecordsUpdateConsignee.Visible = formright.DoHaveUpdateRights;
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
			txtcompany.Text = Conversion.ToString((object)supcus.CompanyName);
			cmbglacfill(supcus.GlAccountId);
			cmbglac.Value = Conversion.ToInt((object)supcus.GlAccountId);
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
			chkactivestatus.Text = Conversion.ToString((object)supcus.Status);
			cmbcountry.Value = Conversion.ToString((object)supcus.CountryId);
			cmbcity.Value = Conversion.ToString((object)supcus.CityId);
			txttown.Text = Conversion.ToString((object)supcus.Town);
			txtzipcode.Text = Conversion.ToString((object)supcus.ZipCode);
			txtphone.Text = Conversion.ToString((object)supcus.Phone);
			txtofficemobile.Text = Conversion.ToString((object)supcus.MobileOffice);
			txtpermobile.Text = Conversion.ToString((object)supcus.MobilePersonal);
			txtemail.Text = Conversion.ToString((object)supcus.Email);
			txtwebsite.Text = Conversion.ToString((object)supcus.WebPage);
			CmbAdvanceAc.Value = Conversion.ToInt((object)supcus.AdvanceGlAcId);
			cmbprovince.Value = Conversion.ToInt((object)supcus.StateProvinceId);
			fileSavePath = Path.GetFileName(supcus.PictureURL);
			if (!string.IsNullOrWhiteSpace(fileSavePath))
			{
				fileSavePath = Path.Combine(clsGlobalVariables.AttachmentFolderPath, fileSavePath);
				if (File.Exists(fileSavePath))
				{
					box.Image = Image.FromFile(fileSavePath);
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
			datagridviewform();
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
			ReadById(Conversion.ToInt(grdfrm.CurrentRow.Cells["Id"].Value));
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
			CommonServices.SupplierResgister_292(0);
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
		try
		{
			Close();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdfrm.CurrentRow;
			if (item != null && (int)item.RowType == 0 && grdfrm.CurrentColumn != null)
			{
				if (e.Column.Key == "Print")
				{
					CommonServices.InvRptSupplierSlip_293(Conversion.ToInt(item.Cells["Id"].Value));
				}
				else if (e.Column.Key == "Update")
				{
					RecordUpdate(item);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RecordUpdate(GridEXRow item)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Expected O, but got Unknown
		try
		{
			SupplierCustomer.Save(new SupplierCustomer
			{
				Id = Conversion.ToInt(item.Cells["Id"].Value),
				GlAccountId = Conversion.ToInt(item.Cells["GlAccountId"].Value),
				CompanyName = Conversion.ToString(item.Cells["CompanyName"].Value),
				ReportingTitle = Conversion.ToString(item.Cells["CompanyName"].Value),
				CustomerGroupId = 7,
				FirstName = Conversion.ToString(item.Cells["PersonName"].Value),
				MobilePersonal = Conversion.ToString(item.Cells["Mobile"].Value),
				CountryId = Conversion.ToInt(item.Cells["Country"].Value),
				StateProvinceId = Conversion.ToInt(item.Cells["Province"].Value),
				CityId = Conversion.ToInt(item.Cells["City"].Value),
				Email = Conversion.ToString(item.Cells["Email"].Value),
				STRN_No = Conversion.ToString(item.Cells["EORI"].Value),
				NTN_No = Conversion.ToString(item.Cells["VATNo"].Value),
				ZipCode = Conversion.ToString(item.Cells["ZipCode"].Value),
				WebPage = Conversion.ToString(item.Cells["WebPage"].Value),
				Address1 = Conversion.ToString(item.Cells["Address"].Value),
				Status = Conversion.ToBool(item.Cells["Status"].Value),
				EntryUser = UserAccount.ID,
				ModifyUser = UserAccount.ID,
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			MessageBox.Show("Record Updated Successfully!");
			datagridviewform();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			btnsave_Click(null, null);
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
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (btnsave.Visible && e.Control && e.KeyCode == Keys.S)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (btnupdate.Visible && e.Control && e.KeyCode == Keys.U)
				{
					btnupdate_Click(null, null);
				}
				if (formright.DoHavePrintRights && e.Control && e.KeyCode == Keys.P)
				{
					btnPrint_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdfrm).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					cmbglac.Focus();
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					cmbglac.Focus();
				}
				if (e.Control && e.Alt)
				{
					MakeShortCutKeys();
				}
				if (e.Control && e.KeyCode == Keys.Return)
				{
					ReadById(Conversion.ToInt((object)grdfrm.CurrentRow.Cells["Id"].Value.ToString()));
					cmbglac.Focus();
				}
				if (e.Control && e.KeyCode == Keys.F1)
				{
					new SupfrmBankDetail(UserAccount).Show();
				}
				if (e.Control && e.KeyCode == Keys.F2)
				{
					new SupfrmShipToAddress(UserAccount).Show();
				}
				if (e.Control && e.KeyCode == Keys.F3)
				{
					new SupfrmSupplierLimits(UserAccount).Show();
				}
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				if (btnSaveConsignee.Visible && e.Control && e.KeyCode == Keys.S)
				{
					btnSaveConsignee_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNewConsignee_Click(null, null);
				}
				if (btnupdate.Visible && e.Control && e.KeyCode == Keys.U)
				{
					btnUpdateConsignee_Click(null, null);
				}
				if (formright.DoHavePrintRights && e.Control && e.KeyCode == Keys.P)
				{
					btnPrintConsignee_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefreshConsignee_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdExConsignee).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					CmbParentAcConsignee.Focus();
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					CmbParentAcConsignee.Focus();
				}
				if (e.Control && e.Alt)
				{
					MakeShortCutKeysConsignee();
				}
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					tabControl1.SelectedIndex = 1;
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
				}
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
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on GL Account");
			dt.Rows.Add("Ctrl+F1", "For Show Bank Detail");
			dt.Rows.Add("Ctrl+F2", "For Show Ship To Address");
			dt.Rows.Add("Ctrl+F3", "For Show Supplier Limits");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On GL Account in Export Party Define");
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

	private void btnBankDetail_Click(object sender, EventArgs e)
	{
		try
		{
			new SupfrmBankDetail(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShipToAddress_Click(object sender, EventArgs e)
	{
		try
		{
			new SupfrmShipToAddress(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSupplierLimits_Click(object sender, EventArgs e)
	{
		try
		{
			new SupfrmSupplierLimits(UserAccount).Show();
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
			DataTable lst = CommonServices.SupplierCustomerGetAllServiceBind();
			if (lst.Rows.Count > 0)
			{
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					if (Conversion.ToInt(lst.Rows[i]["GlAccountId"]) == Conversion.ToInt(cmbglac.Value))
					{
						RecId = Conversion.ToInt(lst.Rows[i]["Id"]);
						ReadById(RecId);
					}
				}
			}
			txtcompany.Focus();
		}
		catch (Exception)
		{
			MessageBox.Show("Id Not Found");
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdfrm, dtGridForLayout);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExportPartyDefines_FormClosing(object sender, FormClosingEventArgs e)
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
			DatatableHelper.GlobalSupplierCustomerListsFillDbCall(UserAccount.OrganizationId, UserAccount.CompanyId);
			cmbglacfill();
			ParentAccountfill();
			cmbcountryfill();
			Provincefill();
			CityFill();
			datagridviewform();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = grdfrm.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdfrm.CurrentColumn == null)
			{
				return;
			}
			int id = Conversion.ToInt(r.Cells["Id"].Value);
			if (formright.DoHavePrintRights && grdfrm.CurrentColumn.Key == "Print")
			{
				DataTable dt = GeneralReprots.SupplierCustomerRegister(new ReportsParameters
				{
					SupplierCustomerId = Conversion.ToInt((object)id)
				});
				if (dt.Rows.Count == 0 || dt == null)
				{
					MessageBox.Show("Record Not Found For Display");
				}
				else
				{
					Reporting val = new Reporting();
					val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
					val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
					val.ShowReportWithDataTable(dt, "293-InvRptSupplierSlip.rpt");
				}
			}
			if (grdfrm.CurrentColumn.Key == "Update")
			{
				RecordUpdate(r);
			}
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

	private void chkactivestatus_Leave(object sender, EventArgs e)
	{
		try
		{
			cmbglac.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateRecordsFromGrid_Click(object sender, EventArgs e)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		try
		{
			if (grdfrm.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please Select Rows first to update multi rows");
			}
			if (MessageBox.Show("Are you sure to Update the Records?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			GridEXRow[] checkedRows = grdfrm.GetCheckedRows();
			foreach (GridEXRow item in checkedRows)
			{
				SupplierCustomer obj = new SupplierCustomer();
				obj.Id = Conversion.ToInt(item.Cells["Id"].Value);
				obj.GlAccountId = Conversion.ToInt(item.Cells["GlAccountId"].Value);
				obj.ParentsSupCustId = Conversion.ToInt(item.Cells["ParentsSupCustId"].Value);
				obj.CompanyName = Conversion.ToString(item.Cells["CompanyName"].Value);
				obj.ReportingTitle = Conversion.ToString(item.Cells["CompanyName"].Value);
				obj.CustomerGroupId = 7;
				obj.FirstName = Conversion.ToString(item.Cells["PersonName"].Value);
				obj.MobilePersonal = Conversion.ToString(item.Cells["Mobile"].Value);
				obj.CountryId = Conversion.ToInt(item.Cells["Country"].Value);
				obj.StateProvinceId = Conversion.ToInt(item.Cells["Province"].Value);
				obj.CityId = Conversion.ToInt(item.Cells["City"].Value);
				obj.Email = Conversion.ToString(item.Cells["Email"].Value);
				obj.STRN_No = Conversion.ToString(item.Cells["EORI"].Value);
				obj.NTN_No = Conversion.ToString(item.Cells["VATNo"].Value);
				obj.ZipCode = Conversion.ToString(item.Cells["ZipCode"].Value);
				obj.WebPage = Conversion.ToString(item.Cells["WebPage"].Value);
				obj.Address1 = Conversion.ToString(item.Cells["Address"].Value);
				obj.Status = Conversion.ToBool(item.Cells["Status"].Value);
				if (obj.ParentsSupCustId > 0)
				{
					obj.IsSubSupCust = true;
				}
				obj.PictureURL = Path.GetFileName(Conversion.ToString(item.Cells["PictureName"].Value));
				obj.EntryUser = UserAccount.ID;
				obj.ModifyUser = UserAccount.ID;
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				SupplierCustomer.Save(obj);
			}
			MessageBox.Show("Record's Updated Successfully!");
			datagridviewform();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDefineCountry_Click(object sender, EventArgs e)
	{
		try
		{
			OpenDefineCountryForm();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OpenDefineCountryForm()
	{
		try
		{
			DefineCountry defineCountry = new DefineCountry(UserAccount);
			defineCountry.BringToFront();
			defineCountry.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDefineProvince_Click(object sender, EventArgs e)
	{
		try
		{
			OpenProvinceForm();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OpenProvinceForm()
	{
		try
		{
			DefineProvince defineProvince = new DefineProvince(UserAccount);
			defineProvince.BringToFront();
			defineProvince.ShowDialog();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnDefineCity_Click(object sender, EventArgs e)
	{
		try
		{
			OpenCityForm();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OpenCityForm()
	{
		try
		{
			DefineCity defineCity = new DefineCity(UserAccount);
			defineCity.BringToFront();
			defineCity.ShowDialog();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ResetConsigneeFields()
	{
		try
		{
			((Control)(object)CmbParentAcConsignee).Text = string.Empty;
			txtConsigneeName.Clear();
			((Control)(object)CmbCountryNameConsignee).Text = string.Empty;
			((Control)(object)CmbCityNameConsignee).Text = string.Empty;
			txtStrnNoConsignee.Clear();
			txtNtnNoConsignee.Clear();
			txtEmailConsignee.Clear();
			txtWebPageConsignee.Clear();
			txtZipCodeConsignee.Clear();
			txtAddressConsignee.Clear();
			btnSaveConsignee.Visible = true;
			btnUpdateConsignee.Visible = false;
			CmbParentAcConsignee.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNewConsignee_Click(object sender, EventArgs e)
	{
		try
		{
			btnSaveConsignee.Visible = true;
			ResetConsigneeFields();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshConsignee_Click(object sender, EventArgs e)
	{
		try
		{
			DatatableHelper.GlobalSupplierCustomerListsFillDbCall(UserAccount.OrganizationId, UserAccount.CompanyId);
			ParentAccountfill();
			cmbcountryfill();
			CityFill();
			ConsigneeHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ConsigneeHistory()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		try
		{
			dtGrid.Rows.Clear();
			DataTable dtHistory = SupplierCustomer.FormHistory(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				CustomerGroupId = 7
			});
			EnumerableRowCollection<DataRow> rows = from r in dtHistory.AsEnumerable()
				where r.Field<int>("ParentsSupCustId") > 0 && r.Field<int>("Id") != r.Field<int>("ParentsSupCustId")
				select r;
			dtGrid = (rows.Any() ? rows.CopyToDataTable() : dtHistory.Clone());
			if (dtGrid.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("ParentsSupCustId", typeof(int));
				dt.Columns.Add("ParentAccountTitle", typeof(string));
				dt.Columns.Add("GlAccountId", typeof(int));
				dt.Columns.Add("GlAccount", typeof(string));
				dt.Columns.Add("CompanyName", typeof(string));
				dt.Columns.Add("Country", typeof(int));
				dt.Columns.Add("City", typeof(int));
				dt.Columns.Add("EORI", typeof(string));
				dt.Columns.Add("VATNo", typeof(string));
				dt.Columns.Add("Email", typeof(string));
				dt.Columns.Add("WebPage", typeof(string));
				dt.Columns.Add("ZipCode", typeof(string));
				dt.Columns.Add("Address", typeof(string));
				dt.Columns.Add("Status", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				dt.Rows.Clear();
				for (int i = 0; i < dtGrid.Rows.Count; i++)
				{
					dt.Rows.Add(dtGrid.Rows[i]["Id"], dtGrid.Rows[i]["ParentsSupCustId"], dtGrid.Rows[i]["ParentAccountTitle"], dtGrid.Rows[i]["GlAccountId"], dtGrid.Rows[i]["AccountTitle"], dtGrid.Rows[i]["CompanyName"], dtGrid.Rows[i]["CountryId"], dtGrid.Rows[i]["CityId"], dtGrid.Rows[i]["STRN_No"], dtGrid.Rows[i]["NTN_No"], dtGrid.Rows[i]["Email"], dtGrid.Rows[i]["WebPage"], dtGrid.Rows[i]["ZipCode"], dtGrid.Rows[i]["Address1"], dtGrid.Rows[i]["Status"], dtGrid.Rows[i]["EntryDate"], dtGrid.Rows[i]["EntryUserName"], dtGrid.Rows[i]["ModifyDate"], dtGrid.Rows[i]["ModifyUserName"]);
				}
				grdExConsignee.DataSource = dt;
				dtGridForLayoutConsignee = dt.Copy();
				grdExConsignee.RetrieveStructure();
				GridSettingConsignee();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridSettingConsignee()
	{
		try
		{
			grdExConsignee.AllowEdit = (InheritableBoolean)1;
			grdExConsignee.RootTable.RowHeight = 30;
			List<string> hiddenColumns = new List<string> { "Id", "ParentsSupCustId", "GlAccountId" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"CompanyName",
					(EditType)1
				},
				{
					"Country",
					(EditType)5
				},
				{
					"City",
					(EditType)5
				},
				{
					"Email",
					(EditType)1
				},
				{
					"EORI",
					(EditType)1
				},
				{
					"VATNo",
					(EditType)1
				},
				{
					"ZipCode",
					(EditType)1
				},
				{
					"WebPage",
					(EditType)1
				},
				{
					"Address",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdExConsignee, 2, 3, hiddenColumns, editableColumns);
			grdExConsignee.RootTable.Columns["CompanyName"].Caption = "Consignee";
			grdExConsignee.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdExConsignee.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdExConsignee.RootTable.Columns["ParentAccountTitle"].Width = Constants.ExportConstants.PartyName;
			grdExConsignee.RootTable.Columns["GlAccount"].Width = Constants.ExportConstants.PartyName;
			grdExConsignee.RootTable.Columns["CompanyName"].Width = Constants.ExportConstants.PartyName;
			grdExConsignee.RootTable.Columns["Country"].Width = Constants.ExportConstants.Country;
			grdExConsignee.RootTable.Columns["City"].Width = Constants.ExportConstants.City;
			grdExConsignee.RootTable.Columns["EORI"].Width = Constants.ExportConstants.StringConstant;
			grdExConsignee.RootTable.Columns["VATNo"].Width = Constants.ExportConstants.StringConstant;
			grdExConsignee.RootTable.Columns["Email"].Width = Constants.ExportConstants.Email;
			grdExConsignee.RootTable.Columns["WebPage"].Width = Constants.ExportConstants.StringConstant;
			grdExConsignee.RootTable.Columns["ZipCode"].Width = Constants.ExportConstants.StringConstant;
			grdExConsignee.RootTable.Columns["Address"].Width = Constants.ExportConstants.StringConstant;
			grdExConsignee.RootTable.Columns["EntryDate"].Width = Constants.ExportConstants.DateTimeConstantddMMM;
			grdExConsignee.RootTable.Columns["EntryUser"].Width = Constants.ExportConstants.EntryUserConstants;
			grdExConsignee.RootTable.Columns["ModifyDate"].Width = Constants.ExportConstants.DateTimeConstantddMMM;
			grdExConsignee.RootTable.Columns["ModifyUser"].Width = Constants.ExportConstants.ModifyUserConstants;
			grdExConsignee.RootTable.Columns["ParentAccountTitle"].WordWrap = true;
			grdExConsignee.RootTable.Columns["GlAccount"].WordWrap = true;
			grdExConsignee.RootTable.Columns["CompanyName"].WordWrap = true;
			grdExConsignee.RootTable.Columns["Address"].WordWrap = true;
			grdConsigneeComboBind();
			((Control)(object)grdExConsignee).Refresh();
			ctrlGrdBar2_Load(null, null);
			GridEX_Helper.AddSelectButton(grdExConsignee, "Select", 40, 0);
			if (formright.DoHavePrintRights)
			{
				GridEX_Helper.AddButton(grdExConsignee, "Print", "Print", 50, 1);
				grdExConsignee.FrozenColumns = 2;
			}
			else
			{
				grdExConsignee.FrozenColumns = 1;
			}
			if (formright.DoHaveUpdateRights)
			{
				GridEX_Helper.AddButton(grdExConsignee, "Update", "Update", 50);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdConsigneeComboBind()
	{
		try
		{
			GridEX_Helper.ComboBind(grdExConsignee.RootTable.Columns["Country"], dtCountry, "Id", "Description", "Country");
			GridEX_Helper.ComboBind(grdExConsignee.RootTable.Columns["City"], dtCity, "Id", "CityName", "City");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnUpdateConsignee_Click(object sender, EventArgs e)
	{
		try
		{
			InsertConsignee();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSaveConsignee_Click(object sender, EventArgs e)
	{
		try
		{
			RecIdConsignee = 0;
			InsertConsignee();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InsertConsignee()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (!formValidationConsignee())
			{
				return;
			}
			SupplierCustomer supcus = new SupplierCustomer();
			if (RecIdConsignee > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				supcus.Id = RecIdConsignee;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			supcus.ParentsSupCustId = Conversion.ToInt(CmbParentAcConsignee.Value);
			supcus.CompanyName = Conversion.ToString((object)txtConsigneeName.Text.Trim());
			supcus.GlAccountId = Conversion.ToInt(((UltraDropDownBase)CmbParentAcConsignee).SelectedRow.Cells["GlAccountId"].Value);
			supcus.ReportingTitle = Conversion.ToString((object)txtConsigneeName.Text.Trim());
			supcus.CustomerGroupId = 7;
			supcus.CountryId = Conversion.ToInt(CmbCountryNameConsignee.Value);
			supcus.CityId = Conversion.ToInt(CmbCityNameConsignee.Value);
			supcus.STRN_No = Conversion.ToString((object)txtStrnNoConsignee.Text.Trim());
			supcus.NTN_No = Conversion.ToString((object)txtNtnNoConsignee.Text.Trim());
			supcus.Email = Conversion.ToString((object)txtEmailConsignee.Text.Trim());
			supcus.WebPage = Conversion.ToString((object)txtWebPageConsignee.Text.Trim());
			supcus.ZipCode = Conversion.ToString((object)txtZipCodeConsignee.Text.Trim());
			supcus.Address1 = Conversion.ToString((object)txtAddressConsignee.Text.Trim());
			supcus.Status = Conversion.ToBool((object)ChkConsigneeStatus.Text.Trim());
			supcus.IsSubSupCust = true;
			supcus.EntryUser = UserAccount.ID;
			supcus.ModifyUser = UserAccount.ID;
			supcus.OrganizationId = UserAccount.OrganizationId;
			supcus.CompanyId = UserAccount.CompanyId;
			int success = SupplierCustomer.Save(supcus);
			supcus.Id = success;
			if (RecIdConsignee > 0)
			{
				MessageBox.Show("Data Update Successfully.... ");
			}
			else
			{
				MessageBox.Show("Data Save Successfully.... ");
			}
			ConsigneeHistory();
			ResetConsigneeFields();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnPrintConsignee_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.ConsigneeRegister_292_01();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnMultiRecordsUpdateConsignee_Click(object sender, EventArgs e)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Expected O, but got Unknown
		try
		{
			if (grdExConsignee.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please Select Rows first to update multi rows");
			}
			if (MessageBox.Show("Are you sure to Update the Records?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				GridEXRow[] checkedRows = grdExConsignee.GetCheckedRows();
				foreach (GridEXRow item in checkedRows)
				{
					SupplierCustomer.Save(new SupplierCustomer
					{
						Id = Conversion.ToInt(item.Cells["Id"].Value),
						GlAccountId = Conversion.ToInt(item.Cells["GlAccountId"].Value),
						ParentsSupCustId = Conversion.ToInt(item.Cells["ParentsSupCustId"].Value),
						CompanyName = Conversion.ToString(item.Cells["CompanyName"].Value),
						ReportingTitle = Conversion.ToString(item.Cells["CompanyName"].Value),
						CustomerGroupId = 7,
						CountryId = Conversion.ToInt(item.Cells["Country"].Value),
						CityId = Conversion.ToInt(item.Cells["City"].Value),
						Email = Conversion.ToString(item.Cells["Email"].Value),
						STRN_No = Conversion.ToString(item.Cells["EORI"].Value),
						NTN_No = Conversion.ToString(item.Cells["VATNo"].Value),
						ZipCode = Conversion.ToString(item.Cells["ZipCode"].Value),
						WebPage = Conversion.ToString(item.Cells["WebPage"].Value),
						Address1 = Conversion.ToString(item.Cells["Address"].Value),
						Status = Conversion.ToBool(item.Cells["Status"].Value),
						IsSubSupCust = true,
						EntryUser = UserAccount.ID,
						ModifyUser = UserAccount.ID,
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId
					});
				}
				MessageBox.Show("Record's Updated Successfully!");
				ConsigneeHistory();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RecordUpdateConsignee(GridEXRow item)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Expected O, but got Unknown
		try
		{
			SupplierCustomer.Save(new SupplierCustomer
			{
				Id = Conversion.ToInt(item.Cells["Id"].Value),
				GlAccountId = Conversion.ToInt(item.Cells["GlAccountId"].Value),
				ParentsSupCustId = Conversion.ToInt(item.Cells["ParentsSupCustId"].Value),
				CompanyName = Conversion.ToString(item.Cells["CompanyName"].Value),
				ReportingTitle = Conversion.ToString(item.Cells["CompanyName"].Value),
				CustomerGroupId = 7,
				CountryId = Conversion.ToInt(item.Cells["Country"].Value),
				CityId = Conversion.ToInt(item.Cells["City"].Value),
				Email = Conversion.ToString(item.Cells["Email"].Value),
				STRN_No = Conversion.ToString(item.Cells["EORI"].Value),
				NTN_No = Conversion.ToString(item.Cells["VATNo"].Value),
				ZipCode = Conversion.ToString(item.Cells["ZipCode"].Value),
				WebPage = Conversion.ToString(item.Cells["WebPage"].Value),
				Address1 = Conversion.ToString(item.Cells["Address"].Value),
				Status = Conversion.ToBool(item.Cells["Status"].Value),
				EntryUser = UserAccount.ID,
				ModifyUser = UserAccount.ID,
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			MessageBox.Show("Record Updated Successfully!");
			ConsigneeHistory();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdExConsignee_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdExConsignee.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				ReadByIdConsignee(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadByIdConsignee(int ID)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		try
		{
			btnSaveConsignee.Visible = false;
			btnUpdateConsignee.Visible = true;
			SupplierCustomer supcus = new SupplierCustomer();
			RecIdConsignee = ID;
			supcus = SupplierCustomer.GetByID(RecId);
			if (supcus != null)
			{
				CmbParentAcConsignee.Value = supcus.ParentsSupCustId;
				txtConsigneeName.Text = Conversion.ToString((object)supcus.CompanyName);
				CmbCountryNameConsignee.Value = Conversion.ToString((object)supcus.CountryId);
				cmbcity.Value = Conversion.ToString((object)supcus.CityId);
				txtStrnNoConsignee.Text = Conversion.ToString((object)supcus.STRN_No);
				txtNtnNoConsignee.Text = Conversion.ToString((object)supcus.NTN_No);
				txtzipcode.Text = Conversion.ToString((object)supcus.ZipCode);
				txtEmailConsignee.Text = Conversion.ToString((object)supcus.Email);
				txtWebPageConsignee.Text = Conversion.ToString((object)supcus.WebPage);
				txtAddressConsignee.Text = Conversion.ToString((object)supcus.Address1);
				ChkConsigneeStatus.Text = Conversion.ToString((object)supcus.Status);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdExConsignee_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdExConsignee.CurrentRow;
			if (item != null && (int)item.RowType == 0 && grdExConsignee.CurrentColumn != null)
			{
				if (e.Column.Key == "Print")
				{
					CommonServices.InvRptSupplierSlip_293(Conversion.ToInt(item.Cells["Id"].Value));
				}
				else if (e.Column.Key == "Update")
				{
					RecordUpdateConsignee(item);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdExConsignee_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = grdExConsignee.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grdExConsignee.CurrentColumn != null)
			{
				int Id = Conversion.ToInt(r.Cells["Id"].Value);
				if (formright.DoHavePrintRights && grdExConsignee.CurrentColumn.Key == "Print")
				{
					CommonServices.InvRptSupplierSlip_293(Id);
				}
				if (grdExConsignee.CurrentColumn.Key == "Update")
				{
					RecordUpdateConsignee(r);
				}
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdExConsignee, dtGridForLayoutConsignee);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton9_Click(object sender, EventArgs e)
	{
		try
		{
			MakeShortCutKeysConsignee();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MakeShortCutKeysConsignee()
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
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Parent Account");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Parent Account");
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

	private void btnCountryDefineConsignee_Click(object sender, EventArgs e)
	{
		try
		{
			OpenDefineCountryForm();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDefineCityConsignee_Click(object sender, EventArgs e)
	{
		try
		{
			OpenCityForm();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		grdfrm.UpdateData();
	}

	private void grdExConsignee_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		grdExConsignee.UpdateData();
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
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Expected O, but got Unknown
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Expected O, but got Unknown
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Expected O, but got Unknown
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Expected O, but got Unknown
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Expected O, but got Unknown
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Expected O, but got Unknown
		//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f1: Expected O, but got Unknown
		//IL_06c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cd: Expected O, but got Unknown
		//IL_0710: Unknown result type (might be due to invalid IL or missing references)
		//IL_071a: Expected O, but got Unknown
		//IL_0726: Unknown result type (might be due to invalid IL or missing references)
		//IL_0730: Expected O, but got Unknown
		//IL_09f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fb: Expected O, but got Unknown
		//IL_0a08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a12: Expected O, but got Unknown
		//IL_4b28: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b32: Expected O, but got Unknown
		//IL_4b3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b49: Expected O, but got Unknown
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
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
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Export.ExportPartyDefines));
		this.grdfrm = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.btnUpdateRecordsFromGrid = new System.Windows.Forms.Button();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label10 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.btnDefineCountry = new System.Windows.Forms.Button();
		this.btnDefineProvince = new System.Windows.Forms.Button();
		this.btnDefineCity = new System.Windows.Forms.Button();
		this.label3 = new System.Windows.Forms.Label();
		this.cmbcity = new UltraCombo();
		this.cmbcountry = new UltraCombo();
		this.cmbglac = new UltraCombo();
		this.cmbprovince = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.txtpermobile = new System.Windows.Forms.MaskedTextBox();
		this.txtphone = new System.Windows.Forms.MaskedTextBox();
		this.txtcnicno = new System.Windows.Forms.MaskedTextBox();
		this.CmbAdvanceAc = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.btnAdd = new System.Windows.Forms.Button();
		this.label14 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.browse = new System.Windows.Forms.Button();
		this.box = new System.Windows.Forms.PictureBox();
		this.chkactivestatus = new System.Windows.Forms.CheckBox();
		this.label57 = new System.Windows.Forms.Label();
		this.txtcnicexp = new System.Windows.Forms.DateTimePicker();
		this.label31 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label55 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.txtcompany = new System.Windows.Forms.TextBox();
		this.txttown = new System.Windows.Forms.TextBox();
		this.label53 = new System.Windows.Forms.Label();
		this.txtofficemobile = new System.Windows.Forms.TextBox();
		this.label52 = new System.Windows.Forms.Label();
		this.txttitle = new System.Windows.Forms.TextBox();
		this.label51 = new System.Windows.Forms.Label();
		this.txtstrnno = new System.Windows.Forms.TextBox();
		this.txtzipcode = new System.Windows.Forms.TextBox();
		this.txtlastname = new System.Windows.Forms.TextBox();
		this.txtemail = new System.Windows.Forms.TextBox();
		this.txtntnno = new System.Windows.Forms.TextBox();
		this.label37 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.txtwebsite = new System.Windows.Forms.TextBox();
		this.label43 = new System.Windows.Forms.Label();
		this.txtfirstname = new System.Windows.Forms.TextBox();
		this.txtaddress1 = new System.Windows.Forms.TextBox();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label20 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnBankDetail = new System.Windows.Forms.ToolStripButton();
		this.btnShipToAddress = new System.Windows.Forms.ToolStripButton();
		this.btnSupplierLimits = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.grdExConsignee = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.btnMultiRecordsUpdateConsignee = new System.Windows.Forms.Button();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label24 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.ChkConsigneeStatus = new System.Windows.Forms.CheckBox();
		this.txtConsigneeName = new System.Windows.Forms.TextBox();
		this.label18 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.txtAddressConsignee = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.txtZipCodeConsignee = new System.Windows.Forms.TextBox();
		this.btnDefineCityConsignee = new System.Windows.Forms.Button();
		this.label11 = new System.Windows.Forms.Label();
		this.txtWebPageConsignee = new System.Windows.Forms.TextBox();
		this.label21 = new System.Windows.Forms.Label();
		this.txtStrnNoConsignee = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.CmbCityNameConsignee = new UltraCombo();
		this.txtNtnNoConsignee = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.txtEmailConsignee = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.CmbParentAcConsignee = new UltraCombo();
		this.label17 = new System.Windows.Forms.Label();
		this.CmbCountryNameConsignee = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.btnCountryDefineConsignee = new System.Windows.Forms.Button();
		this.panel1 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNewConsignee = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshConsignee = new System.Windows.Forms.ToolStripButton();
		this.btnUpdateConsignee = new System.Windows.Forms.ToolStripButton();
		this.btnSaveConsignee = new System.Windows.Forms.ToolStripButton();
		this.btnPrintConsignee = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKeysConsignee = new System.Windows.Forms.ToolStripButton();
		this.ode = new System.Windows.Forms.TextBox();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel8.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbcity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcountry).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbglac).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbprovince).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAdvanceAc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.box).BeginInit();
		this.panel5.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdExConsignee).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCityNameConsignee).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentAcConsignee).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCountryNameConsignee).BeginInit();
		this.panel1.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdfrm).BackColor = System.Drawing.Color.White;
		this.grdfrm.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupTotals = (GroupTotals)2;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(3, 337);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(902, 295);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.grdfrm).TabStop = false;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		this.grdfrm.CellUpdated += new ColumnActionEventHandler(grdfrm_CellUpdated);
		this.grdfrm.ColumnButtonClick += new ColumnActionEventHandler(grdfrm_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdfrm).DoubleClick += new System.EventHandler(grdfrm_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdfrm).KeyDown += new System.Windows.Forms.KeyEventHandler(grdfrm_KeyDown);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.btnUpdateRecordsFromGrid);
		this.panel8.Controls.Add(this.ctrlGrdBar1);
		this.panel8.Controls.Add(this.label10);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 309);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(902, 28);
		this.panel8.TabIndex = 1;
		this.btnUpdateRecordsFromGrid.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnUpdateRecordsFromGrid.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateRecordsFromGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateRecordsFromGrid.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateRecordsFromGrid.ForeColor = System.Drawing.Color.White;
		this.btnUpdateRecordsFromGrid.Location = new System.Drawing.Point(792, 1);
		this.btnUpdateRecordsFromGrid.Name = "btnUpdateRecordsFromGrid";
		this.btnUpdateRecordsFromGrid.Size = new System.Drawing.Size(66, 25);
		this.btnUpdateRecordsFromGrid.TabIndex = 225;
		this.btnUpdateRecordsFromGrid.Text = "Update";
		this.btnUpdateRecordsFromGrid.UseVisualStyleBackColor = false;
		this.btnUpdateRecordsFromGrid.Click += new System.EventHandler(btnUpdateRecordsFromGrid_Click);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(860, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 21;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label10.AutoSize = true;
		this.label10.BackColor = System.Drawing.Color.Teal;
		this.label10.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label10.Location = new System.Drawing.Point(6, 4);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(67, 19);
		this.label10.TabIndex = 0;
		this.label10.Text = "History";
		this.panel6.Controls.Add(this.btnDefineCountry);
		this.panel6.Controls.Add(this.btnDefineProvince);
		this.panel6.Controls.Add(this.btnDefineCity);
		this.panel6.Controls.Add(this.label3);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcity);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcountry);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.cmbglac);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.cmbprovince);
		this.panel6.Controls.Add(this.label9);
		this.panel6.Controls.Add(this.txtpermobile);
		this.panel6.Controls.Add(this.txtphone);
		this.panel6.Controls.Add(this.txtcnicno);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAdvanceAc);
		this.panel6.Controls.Add(this.label8);
		this.panel6.Controls.Add(this.btnAdd);
		this.panel6.Controls.Add(this.label14);
		this.panel6.Controls.Add(this.label2);
		this.panel6.Controls.Add(this.label1);
		this.panel6.Controls.Add(this.browse);
		this.panel6.Controls.Add(this.box);
		this.panel6.Controls.Add(this.chkactivestatus);
		this.panel6.Controls.Add(this.label57);
		this.panel6.Controls.Add(this.txtcnicexp);
		this.panel6.Controls.Add(this.label31);
		this.panel6.Controls.Add(this.label32);
		this.panel6.Controls.Add(this.label33);
		this.panel6.Controls.Add(this.label34);
		this.panel6.Controls.Add(this.label35);
		this.panel6.Controls.Add(this.label55);
		this.panel6.Controls.Add(this.label54);
		this.panel6.Controls.Add(this.txtcompany);
		this.panel6.Controls.Add(this.txttown);
		this.panel6.Controls.Add(this.label53);
		this.panel6.Controls.Add(this.txtofficemobile);
		this.panel6.Controls.Add(this.label52);
		this.panel6.Controls.Add(this.txttitle);
		this.panel6.Controls.Add(this.label51);
		this.panel6.Controls.Add(this.txtstrnno);
		this.panel6.Controls.Add(this.txtzipcode);
		this.panel6.Controls.Add(this.txtlastname);
		this.panel6.Controls.Add(this.txtemail);
		this.panel6.Controls.Add(this.txtntnno);
		this.panel6.Controls.Add(this.label37);
		this.panel6.Controls.Add(this.label38);
		this.panel6.Controls.Add(this.label48);
		this.panel6.Controls.Add(this.label47);
		this.panel6.Controls.Add(this.label40);
		this.panel6.Controls.Add(this.label41);
		this.panel6.Controls.Add(this.label42);
		this.panel6.Controls.Add(this.label44);
		this.panel6.Controls.Add(this.txtwebsite);
		this.panel6.Controls.Add(this.label43);
		this.panel6.Controls.Add(this.txtfirstname);
		this.panel6.Controls.Add(this.txtaddress1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel6.Location = new System.Drawing.Point(3, 58);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(902, 251);
		this.panel6.TabIndex = 3;
		this.btnDefineCountry.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnDefineCountry.Location = new System.Drawing.Point(630, 30);
		this.btnDefineCountry.Name = "btnDefineCountry";
		this.btnDefineCountry.Size = new System.Drawing.Size(26, 26);
		this.btnDefineCountry.TabIndex = 227;
		this.btnDefineCountry.TabStop = false;
		this.btnDefineCountry.UseVisualStyleBackColor = true;
		this.btnDefineCountry.Click += new System.EventHandler(btnDefineCountry_Click);
		this.btnDefineProvince.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnDefineProvince.Location = new System.Drawing.Point(630, 57);
		this.btnDefineProvince.Name = "btnDefineProvince";
		this.btnDefineProvince.Size = new System.Drawing.Size(26, 26);
		this.btnDefineProvince.TabIndex = 226;
		this.btnDefineProvince.TabStop = false;
		this.btnDefineProvince.UseVisualStyleBackColor = true;
		this.btnDefineProvince.Click += new System.EventHandler(btnDefineProvince_Click);
		this.btnDefineCity.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnDefineCity.Location = new System.Drawing.Point(440, 143);
		this.btnDefineCity.Name = "btnDefineCity";
		this.btnDefineCity.Size = new System.Drawing.Size(26, 26);
		this.btnDefineCity.TabIndex = 225;
		this.btnDefineCity.TabStop = false;
		this.btnDefineCity.UseVisualStyleBackColor = true;
		this.btnDefineCity.Click += new System.EventHandler(btnDefineCity_Click);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.Red;
		this.label3.Location = new System.Drawing.Point(92, 62);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(13, 16);
		this.label3.TabIndex = 224;
		this.label3.Text = "*";
		this.cmbcity.AllowNull = (DefaultableBoolean)1;
		this.cmbcity.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbcity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance70;
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
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance79).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbcity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcity).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbcity).Location = new System.Drawing.Point(110, 143);
		((System.Windows.Forms.Control)(object)this.cmbcity).Name = "cmbcity";
		((System.Windows.Forms.Control)(object)this.cmbcity).Size = new System.Drawing.Size(330, 26);
		((System.Windows.Forms.Control)(object)this.cmbcity).TabIndex = 9;
		this.cmbcountry.AllowNull = (DefaultableBoolean)1;
		this.cmbcountry.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcountry.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcountry.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Appearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbcountry).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcountry).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance85).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance85).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcountry).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcountry).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance87;
		((SpecialBoxBase)((UltraGridBase)this.cmbcountry).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcountry).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbcountry).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcountry).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance92).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcountry).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbcountry).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcountry).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcountry).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcountry).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbcountry).Location = new System.Drawing.Point(427, 30);
		((System.Windows.Forms.Control)(object)this.cmbcountry).Name = "cmbcountry";
		((System.Windows.Forms.Control)(object)this.cmbcountry).Size = new System.Drawing.Size(203, 26);
		((System.Windows.Forms.Control)(object)this.cmbcountry).TabIndex = 6;
		this.cmbglac.AllowNull = (DefaultableBoolean)1;
		this.cmbglac.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbglac.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbglac.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbglac).DisplayLayout.Appearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbglac).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbglac).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbglac).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbglac).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance43;
		((SpecialBoxBase)((UltraGridBase)this.cmbglac).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbglac).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbglac).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbglac).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance62).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance63).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance63).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance63).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbglac).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbglac).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbglac).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbglac).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbglac).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbglac).Location = new System.Drawing.Point(110, 4);
		((System.Windows.Forms.Control)(object)this.cmbglac).Name = "cmbglac";
		((System.Windows.Forms.Control)(object)this.cmbglac).Size = new System.Drawing.Size(254, 26);
		((System.Windows.Forms.Control)(object)this.cmbglac).TabIndex = 0;
		this.cmbprovince.AllowNull = (DefaultableBoolean)2;
		this.cmbprovince.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbprovince.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbprovince.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Appearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbprovince).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbprovince).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbprovince).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbprovince).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance31;
		((SpecialBoxBase)((UltraGridBase)this.cmbprovince).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbprovince).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbprovince).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbprovince).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance36).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance37).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbprovince).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbprovince).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbprovince).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbprovince).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbprovince).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbprovince.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbprovince).Location = new System.Drawing.Point(427, 57);
		((System.Windows.Forms.Control)(object)this.cmbprovince).Name = "cmbprovince";
		((System.Windows.Forms.Control)(object)this.cmbprovince).Size = new System.Drawing.Size(203, 26);
		((System.Windows.Forms.Control)(object)this.cmbprovince).TabIndex = 7;
		this.label9.AutoSize = true;
		this.label9.BackColor = System.Drawing.Color.White;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(367, 63);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(53, 15);
		this.label9.TabIndex = 223;
		this.label9.Text = "Province";
		this.txtpermobile.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtpermobile.Location = new System.Drawing.Point(427, 6);
		this.txtpermobile.Name = "txtpermobile";
		this.txtpermobile.Size = new System.Drawing.Size(229, 23);
		this.txtpermobile.TabIndex = 5;
		this.txtphone.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtphone.Location = new System.Drawing.Point(400, 440);
		this.txtphone.Mask = "(999) 000-0000";
		this.txtphone.Name = "txtphone";
		this.txtphone.Size = new System.Drawing.Size(188, 22);
		this.txtphone.TabIndex = 15;
		this.txtcnicno.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcnicno.Location = new System.Drawing.Point(715, 362);
		this.txtcnicno.Margin = new System.Windows.Forms.Padding(2);
		this.txtcnicno.Mask = "00000-0000000-0";
		this.txtcnicno.Name = "txtcnicno";
		this.txtcnicno.Size = new System.Drawing.Size(187, 23);
		this.txtcnicno.TabIndex = 6;
		this.CmbAdvanceAc.AllowNull = (DefaultableBoolean)2;
		this.CmbAdvanceAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAdvanceAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAdvanceAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance19;
		((SpecialBoxBase)((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance24).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance25).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAdvanceAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAdvanceAc).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAdvanceAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAdvanceAc).Location = new System.Drawing.Point(970, 468);
		((System.Windows.Forms.Control)(object)this.CmbAdvanceAc).Name = "CmbAdvanceAc";
		((System.Windows.Forms.Control)(object)this.CmbAdvanceAc).Size = new System.Drawing.Size(205, 24);
		((System.Windows.Forms.Control)(object)this.CmbAdvanceAc).TabIndex = 21;
		this.label8.AutoSize = true;
		this.label8.BackColor = System.Drawing.Color.White;
		this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(878, 470);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(80, 16);
		this.label8.TabIndex = 217;
		this.label8.Text = "Advance Ac";
		this.btnAdd.Location = new System.Drawing.Point(908, 361);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(56, 23);
		this.btnAdd.TabIndex = 7;
		this.btnAdd.Text = "Save";
		this.btnAdd.UseVisualStyleBackColor = true;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(8, 389);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(96, 19);
		this.label14.TabIndex = 190;
		this.label14.Text = "More Details";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.Red;
		this.label2.Location = new System.Drawing.Point(92, 35);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(13, 16);
		this.label2.TabIndex = 0;
		this.label2.Text = "*";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Red;
		this.label1.Location = new System.Drawing.Point(92, 9);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(13, 16);
		this.label1.TabIndex = 161;
		this.label1.Text = "*";
		this.browse.BackColor = System.Drawing.Color.Teal;
		this.browse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.browse.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.browse.ForeColor = System.Drawing.Color.White;
		this.browse.Location = new System.Drawing.Point(759, 112);
		this.browse.Name = "browse";
		this.browse.Size = new System.Drawing.Size(72, 30);
		this.browse.TabIndex = 8;
		this.browse.Text = "Browse";
		this.browse.UseVisualStyleBackColor = false;
		this.browse.Click += new System.EventHandler(browse_Click);
		this.box.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.box.Location = new System.Drawing.Point(698, 5);
		this.box.Name = "box";
		this.box.Size = new System.Drawing.Size(194, 106);
		this.box.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.box.TabIndex = 215;
		this.box.TabStop = false;
		this.chkactivestatus.AutoSize = true;
		this.chkactivestatus.Checked = true;
		this.chkactivestatus.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkactivestatus.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
		this.chkactivestatus.Location = new System.Drawing.Point(613, 87);
		this.chkactivestatus.Name = "chkactivestatus";
		this.chkactivestatus.Size = new System.Drawing.Size(31, 20);
		this.chkactivestatus.TabIndex = 8;
		this.chkactivestatus.Text = " ";
		this.chkactivestatus.UseVisualStyleBackColor = true;
		this.chkactivestatus.Leave += new System.EventHandler(chkactivestatus_Leave);
		this.label57.AutoSize = true;
		this.label57.BackColor = System.Drawing.Color.White;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.Location = new System.Drawing.Point(16, 36);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(74, 15);
		this.label57.TabIndex = 154;
		this.label57.Text = "Comp Name";
		this.txtcnicexp.CustomFormat = "dd-MM-yy";
		this.txtcnicexp.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcnicexp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtcnicexp.Location = new System.Drawing.Point(110, 411);
		this.txtcnicexp.Name = "txtcnicexp";
		this.txtcnicexp.Size = new System.Drawing.Size(203, 25);
		this.txtcnicexp.TabIndex = 12;
		this.label31.AutoSize = true;
		this.label31.BackColor = System.Drawing.Color.White;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(367, 10);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(51, 15);
		this.label31.TabIndex = 179;
		this.label31.Text = "Mobile#";
		this.label32.AutoSize = true;
		this.label32.BackColor = System.Drawing.Color.White;
		this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(878, 443);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(40, 16);
		this.label32.TabIndex = 207;
		this.label32.Text = "Town";
		this.label33.AutoSize = true;
		this.label33.BackColor = System.Drawing.Color.White;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(568, 90);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(40, 15);
		this.label33.TabIndex = 3;
		this.label33.Text = "Active";
		this.label34.AutoSize = true;
		this.label34.BackColor = System.Drawing.Color.White;
		this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(637, 364);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(59, 16);
		this.label34.TabIndex = 203;
		this.label34.Text = "CNIC No";
		this.label35.AutoSize = true;
		this.label35.BackColor = System.Drawing.Color.White;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(20, 201);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(33, 15);
		this.label35.TabIndex = 200;
		this.label35.Text = "EORI";
		this.label55.AutoSize = true;
		this.label55.BackColor = System.Drawing.Color.White;
		this.label55.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.Location = new System.Drawing.Point(318, 470);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(33, 16);
		this.label55.TabIndex = 213;
		this.label55.Text = "Title";
		this.label54.AutoSize = true;
		this.label54.BackColor = System.Drawing.Color.White;
		this.label54.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(12, 415);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(78, 16);
		this.label54.TabIndex = 204;
		this.label54.Text = "CNIC Expiry";
		this.txtcompany.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcompany.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcompany.Location = new System.Drawing.Point(110, 32);
		this.txtcompany.Name = "txtcompany";
		this.txtcompany.Size = new System.Drawing.Size(254, 23);
		this.txtcompany.TabIndex = 1;
		this.txttown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txttown.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txttown.Location = new System.Drawing.Point(970, 439);
		this.txttown.Name = "txttown";
		this.txttown.Size = new System.Drawing.Size(205, 25);
		this.txttown.TabIndex = 17;
		this.label53.AutoSize = true;
		this.label53.BackColor = System.Drawing.Color.White;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(367, 36);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(52, 15);
		this.label53.TabIndex = 163;
		this.label53.Text = "Country ";
		this.txtofficemobile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtofficemobile.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtofficemobile.Location = new System.Drawing.Point(684, 439);
		this.txtofficemobile.Name = "txtofficemobile";
		this.txtofficemobile.Size = new System.Drawing.Size(188, 25);
		this.txtofficemobile.TabIndex = 16;
		this.label52.AutoSize = true;
		this.label52.BackColor = System.Drawing.Color.White;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(481, 149);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(69, 15);
		this.label52.TabIndex = 208;
		this.label52.Text = "postal Code";
		this.txttitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txttitle.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txttitle.Location = new System.Drawing.Point(400, 467);
		this.txttitle.Name = "txttitle";
		this.txttitle.Size = new System.Drawing.Size(188, 25);
		this.txttitle.TabIndex = 19;
		this.label51.AutoSize = true;
		this.label51.BackColor = System.Drawing.Color.White;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(20, 175);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(39, 15);
		this.label51.TabIndex = 211;
		this.label51.Text = "Email ";
		this.txtstrnno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtstrnno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtstrnno.Location = new System.Drawing.Point(110, 197);
		this.txtstrnno.Name = "txtstrnno";
		this.txtstrnno.Size = new System.Drawing.Size(356, 23);
		this.txtstrnno.TabIndex = 11;
		this.txtzipcode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtzipcode.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtzipcode.Location = new System.Drawing.Point(566, 145);
		this.txtzipcode.Name = "txtzipcode";
		this.txtzipcode.Size = new System.Drawing.Size(327, 23);
		this.txtzipcode.TabIndex = 13;
		this.txtlastname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtlastname.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtlastname.Location = new System.Drawing.Point(970, 383);
		this.txtlastname.Name = "txtlastname";
		this.txtlastname.Size = new System.Drawing.Size(205, 25);
		this.txtlastname.TabIndex = 11;
		this.txtemail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtemail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtemail.Location = new System.Drawing.Point(110, 171);
		this.txtemail.Name = "txtemail";
		this.txtemail.Size = new System.Drawing.Size(356, 23);
		this.txtemail.TabIndex = 10;
		this.txtntnno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtntnno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtntnno.Location = new System.Drawing.Point(566, 197);
		this.txtntnno.Name = "txtntnno";
		this.txtntnno.Size = new System.Drawing.Size(327, 23);
		this.txtntnno.TabIndex = 15;
		this.label37.AutoSize = true;
		this.label37.BackColor = System.Drawing.Color.White;
		this.label37.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(596, 443);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(85, 16);
		this.label37.TabIndex = 210;
		this.label37.Text = "Office Mobile";
		this.label38.AutoSize = true;
		this.label38.BackColor = System.Drawing.Color.White;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(20, 149);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(27, 15);
		this.label38.TabIndex = 173;
		this.label38.Text = "City";
		this.label48.AutoSize = true;
		this.label48.BackColor = System.Drawing.Color.White;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(16, 63);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(78, 15);
		this.label48.TabIndex = 201;
		this.label48.Text = "Person Name";
		this.label47.AutoSize = true;
		this.label47.BackColor = System.Drawing.Color.White;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(20, 227);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(49, 15);
		this.label47.TabIndex = 205;
		this.label47.Text = "Address";
		this.label40.AutoSize = true;
		this.label40.BackColor = System.Drawing.Color.White;
		this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(886, 387);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(72, 16);
		this.label40.TabIndex = 202;
		this.label40.Text = "Last Name";
		this.label41.AutoSize = true;
		this.label41.BackColor = System.Drawing.Color.White;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(481, 201);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(75, 15);
		this.label41.TabIndex = 199;
		this.label41.Text = "VAT Number";
		this.label42.AutoSize = true;
		this.label42.BackColor = System.Drawing.Color.White;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(16, 10);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(66, 15);
		this.label42.TabIndex = 158;
		this.label42.Text = "Gl Account";
		this.label44.AutoSize = true;
		this.label44.BackColor = System.Drawing.Color.White;
		this.label44.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(318, 443);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(46, 16);
		this.label44.TabIndex = 209;
		this.label44.Text = "Phone";
		this.txtwebsite.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtwebsite.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtwebsite.Location = new System.Drawing.Point(566, 171);
		this.txtwebsite.Name = "txtwebsite";
		this.txtwebsite.Size = new System.Drawing.Size(327, 23);
		this.txtwebsite.TabIndex = 14;
		this.label43.AutoSize = true;
		this.label43.BackColor = System.Drawing.Color.White;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(481, 175);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(74, 15);
		this.label43.TabIndex = 214;
		this.label43.Text = "Website:URL";
		this.txtfirstname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfirstname.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfirstname.Location = new System.Drawing.Point(110, 59);
		this.txtfirstname.Name = "txtfirstname";
		this.txtfirstname.Size = new System.Drawing.Size(254, 23);
		this.txtfirstname.TabIndex = 2;
		this.txtaddress1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtaddress1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtaddress1.Location = new System.Drawing.Point(110, 223);
		this.txtaddress1.Name = "txtaddress1";
		this.txtaddress1.Size = new System.Drawing.Size(783, 23);
		this.txtaddress1.TabIndex = 12;
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label20);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 30);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(902, 28);
		this.panel5.TabIndex = 0;
		this.label20.AutoSize = true;
		this.label20.BackColor = System.Drawing.Color.Teal;
		this.label20.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label20.Location = new System.Drawing.Point(6, 4);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(184, 19);
		this.label20.TabIndex = 0;
		this.label20.Text = "Export Parties Define";
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.btnnew, this.btnRefresh, this.btnupdate, this.btnsave, this.btnPrint, this.btnBankDetail, this.btnShipToAddress, this.btnSupplierLimits, this.btnShortCutKeys });
		this.toolStrip2.Location = new System.Drawing.Point(3, 3);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(902, 27);
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
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
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
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(92, 24);
		this.btnPrint.Text = "292-Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnBankDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnBankDetail.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnBankDetail.Name = "btnBankDetail";
		this.btnBankDetail.Size = new System.Drawing.Size(84, 24);
		this.btnBankDetail.Text = "Bank Detail";
		this.btnBankDetail.Click += new System.EventHandler(btnBankDetail_Click);
		this.btnShipToAddress.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShipToAddress.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShipToAddress.Name = "btnShipToAddress";
		this.btnShipToAddress.Size = new System.Drawing.Size(107, 24);
		this.btnShipToAddress.Text = "ShipToAddress";
		this.btnShipToAddress.Click += new System.EventHandler(btnShipToAddress_Click);
		this.btnSupplierLimits.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSupplierLimits.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSupplierLimits.Name = "btnSupplierLimits";
		this.btnSupplierLimits.Size = new System.Drawing.Size(100, 24);
		this.btnSupplierLimits.Text = "SupplierLimits";
		this.btnSupplierLimits.Click += new System.EventHandler(btnSupplierLimits_Click);
		this.btnShortCutKeys.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnShortCutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKeys.Image = (System.Drawing.Image)resources.GetObject("btnShortCutKeys.Image");
		this.btnShortCutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeys.Name = "btnShortCutKeys";
		this.btnShortCutKeys.Size = new System.Drawing.Size(111, 24);
		this.btnShortCutKeys.Text = "ShortCut  Keys";
		this.btnShortCutKeys.Click += new System.EventHandler(btnShortCutKeys_Click);
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(916, 664);
		this.tabControl1.TabIndex = 1;
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.tabPage1.Controls.Add(this.panel8);
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.toolStrip2);
		this.tabPage1.Location = new System.Drawing.Point(4, 25);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(908, 635);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Define Party";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdExConsignee);
		this.tabPage2.Controls.Add(this.panel3);
		this.tabPage2.Controls.Add(this.panel2);
		this.tabPage2.Controls.Add(this.panel1);
		this.tabPage2.Controls.Add(this.toolStrip1);
		this.tabPage2.Location = new System.Drawing.Point(4, 25);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(908, 635);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Define Consignee/Notify";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.grdExConsignee.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdExConsignee).BackColor = System.Drawing.Color.White;
		this.grdExConsignee.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.grdExConsignee).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdExConsignee.FilterMode = (FilterMode)2;
		this.grdExConsignee.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdExConsignee).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdExConsignee.GroupTotals = (GroupTotals)2;
		this.grdExConsignee.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdExConsignee).Location = new System.Drawing.Point(3, 238);
		((System.Windows.Forms.Control)(object)this.grdExConsignee).Name = "grdExConsignee";
		((ScrollableBase)this.grdExConsignee).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdExConsignee).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdExConsignee).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdExConsignee).Size = new System.Drawing.Size(902, 394);
		((System.Windows.Forms.Control)(object)this.grdExConsignee).TabIndex = 250;
		((System.Windows.Forms.Control)(object)this.grdExConsignee).TabStop = false;
		this.grdExConsignee.TotalRow = (InheritableBoolean)1;
		this.grdExConsignee.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdExConsignee).VisualStyle = (VisualStyle)4;
		this.grdExConsignee.CellUpdated += new ColumnActionEventHandler(grdExConsignee_CellUpdated);
		this.grdExConsignee.ColumnButtonClick += new ColumnActionEventHandler(grdExConsignee_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdExConsignee).DoubleClick += new System.EventHandler(grdExConsignee_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdExConsignee).KeyDown += new System.Windows.Forms.KeyEventHandler(grdExConsignee_KeyDown);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.btnMultiRecordsUpdateConsignee);
		this.panel3.Controls.Add(this.ctrlGrdBar2);
		this.panel3.Controls.Add(this.label24);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 210);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(902, 28);
		this.panel3.TabIndex = 249;
		this.btnMultiRecordsUpdateConsignee.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnMultiRecordsUpdateConsignee.BackColor = System.Drawing.Color.Teal;
		this.btnMultiRecordsUpdateConsignee.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnMultiRecordsUpdateConsignee.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnMultiRecordsUpdateConsignee.ForeColor = System.Drawing.Color.White;
		this.btnMultiRecordsUpdateConsignee.Location = new System.Drawing.Point(792, 0);
		this.btnMultiRecordsUpdateConsignee.Name = "btnMultiRecordsUpdateConsignee";
		this.btnMultiRecordsUpdateConsignee.Size = new System.Drawing.Size(66, 25);
		this.btnMultiRecordsUpdateConsignee.TabIndex = 225;
		this.btnMultiRecordsUpdateConsignee.Text = "Update";
		this.btnMultiRecordsUpdateConsignee.UseVisualStyleBackColor = false;
		this.btnMultiRecordsUpdateConsignee.Click += new System.EventHandler(btnMultiRecordsUpdateConsignee_Click);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(860, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
		this.ctrlGrdBar2.MyGrid = this.grdExConsignee;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label24.AutoSize = true;
		this.label24.BackColor = System.Drawing.Color.Teal;
		this.label24.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label24.Location = new System.Drawing.Point(6, 4);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(67, 19);
		this.label24.TabIndex = 0;
		this.label24.Text = "History";
		this.panel2.Controls.Add(this.groupBox1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 58);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(902, 152);
		this.panel2.TabIndex = 248;
		this.groupBox1.Controls.Add(this.ChkConsigneeStatus);
		this.groupBox1.Controls.Add(this.txtConsigneeName);
		this.groupBox1.Controls.Add(this.label18);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Controls.Add(this.txtAddressConsignee);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.txtZipCodeConsignee);
		this.groupBox1.Controls.Add(this.btnDefineCityConsignee);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.txtWebPageConsignee);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add(this.txtStrnNoConsignee);
		this.groupBox1.Controls.Add(this.label16);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCityNameConsignee);
		this.groupBox1.Controls.Add(this.txtNtnNoConsignee);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.txtEmailConsignee);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbParentAcConsignee);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCountryNameConsignee);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.btnCountryDefineConsignee);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(789, 152);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Entry Feilds";
		this.ChkConsigneeStatus.AutoSize = true;
		this.ChkConsigneeStatus.Checked = true;
		this.ChkConsigneeStatus.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkConsigneeStatus.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkConsigneeStatus.ForeColor = System.Drawing.Color.Black;
		this.ChkConsigneeStatus.Location = new System.Drawing.Point(727, 126);
		this.ChkConsigneeStatus.Name = "ChkConsigneeStatus";
		this.ChkConsigneeStatus.Size = new System.Drawing.Size(59, 19);
		this.ChkConsigneeStatus.TabIndex = 10;
		this.ChkConsigneeStatus.Text = "Active";
		this.ChkConsigneeStatus.UseVisualStyleBackColor = true;
		this.txtConsigneeName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtConsigneeName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtConsigneeName.Location = new System.Drawing.Point(90, 44);
		this.txtConsigneeName.Name = "txtConsigneeName";
		this.txtConsigneeName.Size = new System.Drawing.Size(235, 23);
		this.txtConsigneeName.TabIndex = 1;
		this.label18.AutoSize = true;
		this.label18.BackColor = System.Drawing.Color.White;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(6, 128);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(49, 15);
		this.label18.TabIndex = 241;
		this.label18.Text = "Address";
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.White;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(332, 103);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(69, 15);
		this.label15.TabIndex = 242;
		this.label15.Text = "postal Code";
		this.txtAddressConsignee.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAddressConsignee.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAddressConsignee.Location = new System.Drawing.Point(90, 124);
		this.txtAddressConsignee.Name = "txtAddressConsignee";
		this.txtAddressConsignee.Size = new System.Drawing.Size(634, 23);
		this.txtAddressConsignee.TabIndex = 4;
		this.label12.AutoSize = true;
		this.label12.BackColor = System.Drawing.Color.White;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(332, 23);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(33, 15);
		this.label12.TabIndex = 240;
		this.label12.Text = "EORI";
		this.txtZipCodeConsignee.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtZipCodeConsignee.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtZipCodeConsignee.Location = new System.Drawing.Point(414, 99);
		this.txtZipCodeConsignee.Name = "txtZipCodeConsignee";
		this.txtZipCodeConsignee.Size = new System.Drawing.Size(309, 23);
		this.txtZipCodeConsignee.TabIndex = 9;
		this.btnDefineCityConsignee.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnDefineCityConsignee.Location = new System.Drawing.Point(299, 97);
		this.btnDefineCityConsignee.Name = "btnDefineCityConsignee";
		this.btnDefineCityConsignee.Size = new System.Drawing.Size(26, 26);
		this.btnDefineCityConsignee.TabIndex = 245;
		this.btnDefineCityConsignee.TabStop = false;
		this.btnDefineCityConsignee.UseVisualStyleBackColor = true;
		this.btnDefineCityConsignee.Click += new System.EventHandler(btnDefineCityConsignee_Click);
		this.label11.AutoSize = true;
		this.label11.BackColor = System.Drawing.Color.White;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(6, 48);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(62, 15);
		this.label11.TabIndex = 165;
		this.label11.Text = "Consignee";
		this.txtWebPageConsignee.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWebPageConsignee.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWebPageConsignee.Location = new System.Drawing.Point(414, 71);
		this.txtWebPageConsignee.Name = "txtWebPageConsignee";
		this.txtWebPageConsignee.Size = new System.Drawing.Size(309, 23);
		this.txtWebPageConsignee.TabIndex = 8;
		this.label21.AutoSize = true;
		this.label21.BackColor = System.Drawing.Color.White;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(332, 75);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(74, 15);
		this.label21.TabIndex = 244;
		this.label21.Text = "Website:URL";
		this.txtStrnNoConsignee.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtStrnNoConsignee.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtStrnNoConsignee.Location = new System.Drawing.Point(414, 19);
		this.txtStrnNoConsignee.Name = "txtStrnNoConsignee";
		this.txtStrnNoConsignee.Size = new System.Drawing.Size(115, 23);
		this.txtStrnNoConsignee.TabIndex = 5;
		this.label16.AutoSize = true;
		this.label16.BackColor = System.Drawing.Color.White;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(332, 48);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(39, 15);
		this.label16.TabIndex = 243;
		this.label16.Text = "Email ";
		this.CmbCityNameConsignee.AllowNull = (DefaultableBoolean)1;
		this.CmbCityNameConsignee.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCityNameConsignee.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCityNameConsignee.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCityNameConsignee).DisplayLayout.Appearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbCityNameConsignee).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCityNameConsignee).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCityNameConsignee).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCityNameConsignee).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.CmbCityNameConsignee).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCityNameConsignee).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbCityNameConsignee).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCityNameConsignee).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCityNameConsignee).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCityNameConsignee).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbCityNameConsignee).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCityNameConsignee).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCityNameConsignee).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance52).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCityNameConsignee).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbCityNameConsignee).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCityNameConsignee).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCityNameConsignee).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCityNameConsignee).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbCityNameConsignee).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCityNameConsignee).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCityNameConsignee).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbCityNameConsignee).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCityNameConsignee).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbCityNameConsignee).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCityNameConsignee).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCityNameConsignee).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCityNameConsignee).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbCityNameConsignee).Location = new System.Drawing.Point(90, 97);
		((System.Windows.Forms.Control)(object)this.CmbCityNameConsignee).Name = "CmbCityNameConsignee";
		((System.Windows.Forms.Control)(object)this.CmbCityNameConsignee).Size = new System.Drawing.Size(208, 26);
		((System.Windows.Forms.Control)(object)this.CmbCityNameConsignee).TabIndex = 3;
		this.txtNtnNoConsignee.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNtnNoConsignee.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNtnNoConsignee.Location = new System.Drawing.Point(611, 19);
		this.txtNtnNoConsignee.Name = "txtNtnNoConsignee";
		this.txtNtnNoConsignee.Size = new System.Drawing.Size(113, 23);
		this.txtNtnNoConsignee.TabIndex = 6;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.Red;
		this.label22.Location = new System.Drawing.Point(77, 22);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(13, 16);
		this.label22.TabIndex = 246;
		this.label22.Text = "*";
		this.label19.AutoSize = true;
		this.label19.BackColor = System.Drawing.Color.White;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(531, 23);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(75, 15);
		this.label19.TabIndex = 239;
		this.label19.Text = "VAT Number";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.Red;
		this.label5.Location = new System.Drawing.Point(77, 47);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(13, 16);
		this.label5.TabIndex = 163;
		this.label5.Text = "*";
		this.txtEmailConsignee.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEmailConsignee.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEmailConsignee.Location = new System.Drawing.Point(414, 44);
		this.txtEmailConsignee.Name = "txtEmailConsignee";
		this.txtEmailConsignee.Size = new System.Drawing.Size(309, 23);
		this.txtEmailConsignee.TabIndex = 7;
		this.label13.AutoSize = true;
		this.label13.BackColor = System.Drawing.Color.White;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(6, 23);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(58, 15);
		this.label13.TabIndex = 221;
		this.label13.Text = "Parent Ac";
		this.CmbParentAcConsignee.AllowNull = (DefaultableBoolean)1;
		this.CmbParentAcConsignee.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbParentAcConsignee.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbParentAcConsignee.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParentAcConsignee).DisplayLayout.Appearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbParentAcConsignee).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParentAcConsignee).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentAcConsignee).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentAcConsignee).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance5;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentAcConsignee).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentAcConsignee).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbParentAcConsignee).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentAcConsignee).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentAcConsignee).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentAcConsignee).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbParentAcConsignee).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParentAcConsignee).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentAcConsignee).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParentAcConsignee).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbParentAcConsignee).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParentAcConsignee).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentAcConsignee).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance13).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentAcConsignee).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbParentAcConsignee).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentAcConsignee).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentAcConsignee).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbParentAcConsignee).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentAcConsignee).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbParentAcConsignee).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParentAcConsignee).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParentAcConsignee).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParentAcConsignee).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbParentAcConsignee).Location = new System.Drawing.Point(90, 17);
		((System.Windows.Forms.Control)(object)this.CmbParentAcConsignee).Name = "CmbParentAcConsignee";
		((System.Windows.Forms.Control)(object)this.CmbParentAcConsignee).Size = new System.Drawing.Size(235, 26);
		((System.Windows.Forms.Control)(object)this.CmbParentAcConsignee).TabIndex = 0;
		this.label17.AutoSize = true;
		this.label17.BackColor = System.Drawing.Color.White;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(6, 103);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(62, 15);
		this.label17.TabIndex = 238;
		this.label17.Text = "City Name";
		this.CmbCountryNameConsignee.AllowNull = (DefaultableBoolean)1;
		this.CmbCountryNameConsignee.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCountryNameConsignee.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCountryNameConsignee.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCountryNameConsignee).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbCountryNameConsignee).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCountryNameConsignee).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCountryNameConsignee).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCountryNameConsignee).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance56;
		((SpecialBoxBase)((UltraGridBase)this.CmbCountryNameConsignee).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCountryNameConsignee).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbCountryNameConsignee).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCountryNameConsignee).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCountryNameConsignee).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCountryNameConsignee).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbCountryNameConsignee).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCountryNameConsignee).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCountryNameConsignee).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance10).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCountryNameConsignee).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbCountryNameConsignee).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCountryNameConsignee).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance16).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCountryNameConsignee).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance71).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCountryNameConsignee).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbCountryNameConsignee).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCountryNameConsignee).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCountryNameConsignee).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbCountryNameConsignee).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCountryNameConsignee).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbCountryNameConsignee).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCountryNameConsignee).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCountryNameConsignee).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCountryNameConsignee).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbCountryNameConsignee).Location = new System.Drawing.Point(90, 69);
		((System.Windows.Forms.Control)(object)this.CmbCountryNameConsignee).Name = "CmbCountryNameConsignee";
		((System.Windows.Forms.Control)(object)this.CmbCountryNameConsignee).Size = new System.Drawing.Size(208, 26);
		((System.Windows.Forms.Control)(object)this.CmbCountryNameConsignee).TabIndex = 2;
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.White;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(6, 75);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(84, 15);
		this.label6.TabIndex = 229;
		this.label6.Text = "Country Name";
		this.btnCountryDefineConsignee.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnCountryDefineConsignee.Location = new System.Drawing.Point(299, 69);
		this.btnCountryDefineConsignee.Name = "btnCountryDefineConsignee";
		this.btnCountryDefineConsignee.Size = new System.Drawing.Size(26, 26);
		this.btnCountryDefineConsignee.TabIndex = 230;
		this.btnCountryDefineConsignee.TabStop = false;
		this.btnCountryDefineConsignee.UseVisualStyleBackColor = true;
		this.btnCountryDefineConsignee.Click += new System.EventHandler(btnCountryDefineConsignee_Click);
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.label23);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 30);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(902, 28);
		this.panel1.TabIndex = 247;
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.Teal;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label23.Location = new System.Drawing.Point(6, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(236, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "Consignee/Notify Definition";
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.btnNewConsignee, this.btnRefreshConsignee, this.btnUpdateConsignee, this.btnSaveConsignee, this.btnPrintConsignee, this.btnShortCutKeysConsignee });
		this.toolStrip1.Location = new System.Drawing.Point(3, 3);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(902, 27);
		this.toolStrip1.TabIndex = 1;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNewConsignee.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewConsignee.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewConsignee.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewConsignee.Name = "btnNewConsignee";
		this.btnNewConsignee.Size = new System.Drawing.Size(59, 24);
		this.btnNewConsignee.Text = "&New";
		this.btnNewConsignee.Click += new System.EventHandler(btnNewConsignee_Click);
		this.btnRefreshConsignee.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshConsignee.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshConsignee.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshConsignee.Name = "btnRefreshConsignee";
		this.btnRefreshConsignee.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshConsignee.Text = "&Refresh";
		this.btnRefreshConsignee.Click += new System.EventHandler(btnRefreshConsignee_Click);
		this.btnUpdateConsignee.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateConsignee.Image = (System.Drawing.Image)resources.GetObject("btnUpdateConsignee.Image");
		this.btnUpdateConsignee.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdateConsignee.Name = "btnUpdateConsignee";
		this.btnUpdateConsignee.Size = new System.Drawing.Size(78, 24);
		this.btnUpdateConsignee.Text = "&Update";
		this.btnUpdateConsignee.Click += new System.EventHandler(btnUpdateConsignee_Click);
		this.btnSaveConsignee.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSaveConsignee.Image = (System.Drawing.Image)resources.GetObject("btnSaveConsignee.Image");
		this.btnSaveConsignee.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveConsignee.Name = "btnSaveConsignee";
		this.btnSaveConsignee.Size = new System.Drawing.Size(64, 24);
		this.btnSaveConsignee.Text = "&Save";
		this.btnSaveConsignee.Click += new System.EventHandler(btnSaveConsignee_Click);
		this.btnPrintConsignee.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrintConsignee.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintConsignee.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintConsignee.Name = "btnPrintConsignee";
		this.btnPrintConsignee.Size = new System.Drawing.Size(116, 24);
		this.btnPrintConsignee.Text = "292_01-Print";
		this.btnPrintConsignee.ToolTipText = "292_01-InvRptSupplierRegister";
		this.btnPrintConsignee.Click += new System.EventHandler(btnPrintConsignee_Click);
		this.btnShortCutKeysConsignee.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnShortCutKeysConsignee.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKeysConsignee.Image = (System.Drawing.Image)resources.GetObject("btnShortCutKeysConsignee.Image");
		this.btnShortCutKeysConsignee.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeysConsignee.Name = "btnShortCutKeysConsignee";
		this.btnShortCutKeysConsignee.Size = new System.Drawing.Size(111, 24);
		this.btnShortCutKeysConsignee.Text = "ShortCut  Keys";
		this.btnShortCutKeysConsignee.Click += new System.EventHandler(toolStripButton9_Click);
		this.ode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ode.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ode.Location = new System.Drawing.Point(414, 99);
		this.ode.Name = "ode";
		this.ode.Size = new System.Drawing.Size(309, 23);
		this.ode.TabIndex = 232;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(916, 664);
		base.Controls.Add(this.tabControl1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "ExportPartyDefines";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ExportPartyDefines_FormClosing);
		base.Load += new System.EventHandler(supfrmDefineSupplier1_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(supfrmDefineSupplier1_KeyDown);
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbcity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcountry).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbglac).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbprovince).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAdvanceAc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.box).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabPage1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.tabPage2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdExConsignee).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCityNameConsignee).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentAcConsignee).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCountryNameConsignee).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
