using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.HRM.EmployeeManagement;
using Architecture.BLL.HRM.PolicyManagment;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.HRM.EmployeeManagement;
using Architecture.Model.HRM.PolicyManagment;
using Architecture.WinApp.Common;
using Architecture.WinApp.HRM.PolicyManagment;
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
namespace Architecture.WinApp.HRM.EmployeeManagement
{

public class frmgenPartyLocation : Form
{
	public UserAccount UserAccount = new UserAccount();

	private int RecId;

	private DropDownBind DDL = new DropDownBind();

	private bool UpdateMode;

	private IContainer components;

	private ToolStripButton btnsave;

	private ToolStripButton btnupdate;

	private ToolStripButton btnnew;

	private ToolStrip tsItemType;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel2;

	private Panel panel1;

	internal UltraCombo cmbParty;

	private Label label50;

	internal UltraCombo cmbLocationType;

	private Label label3;

	private Label label4;

	private TextBox txtPrefix;

	private Label label2;

	private TextBox txtCode;

	private Label label6;

	private TextBox txtLocationName;

	internal UltraCombo cmbLocationZone;

	private Label label1;

	private CheckBox NeedLogin;

	private CheckBox IsAllowShare;

	private CheckBox IsCreditInvoice;

	private CheckBox IsCashInvoice;

	private Label label5;

	private TextBox txtMobile1;

	private Label label7;

	private TextBox txtMobile2;

	private Label label8;

	private TextBox txtFlatSharePercent;

	private Panel panel4;

	private Label label9;

	private Button btnParty;

	private Button btnLocationtype;

	private Button btnlocationZone;

	private ToolStripButton btnRefresh;

	internal GridEX grdfrm;

	private Panel panel3;

	private Label label10;

	public frmgenPartyLocation(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void PartyFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable lst = genParty.Getall(new genParty
			{
				OrginizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Party");
			for (int i = 0; i < lst.Rows.Count; i++)
			{
				dt.Rows.Add(lst.Rows[i]["PartyId"], lst.Rows[i]["PartyName"]);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbParty, "Id", "Party", "Party", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LocationTypeFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		try
		{
			DataTable lst = genProfile.GetByProfileTypeId(new genProfile
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ProfilePrefix = "All",
				ProfileTypeId = 63
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("ProfileId");
			dt.Columns.Add("ProfileName");
			for (int i = 0; i < lst.Rows.Count; i++)
			{
				dt.Rows.Add(lst.Rows[i]["ProfileId"], lst.Rows[i]["ProfileName"]);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbLocationType, "ProfileId", "ProfileName", "Location Type", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LocationZoneFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		try
		{
			DataTable lst = genProfile.GetByProfileTypeId(new genProfile
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ProfilePrefix = "All",
				ProfileTypeId = 64
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("ProfileId");
			dt.Columns.Add("ProfileName");
			for (int i = 0; i < lst.Rows.Count; i++)
			{
				dt.Rows.Add(lst.Rows[i]["ProfileId"], lst.Rows[i]["ProfileName"]);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbLocationZone, "ProfileId", "ProfileName", "Location Zone", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void clear()
	{
		btnsave.Visible = true;
		btnupdate.Visible = false;
		UpdateMode = false;
		((Control)(object)cmbLocationType).Text = string.Empty;
		((Control)(object)cmbParty).Text = string.Empty;
		((Control)(object)cmbLocationZone).Text = string.Empty;
		txtCode.Text = string.Empty;
		txtPrefix.Text = string.Empty;
		txtLocationName.Text = string.Empty;
		IsAllowShare.Checked = false;
		NeedLogin.Checked = false;
		IsCashInvoice.Checked = false;
		IsCreditInvoice.Checked = false;
		txtMobile1.Text = string.Empty;
		txtMobile2.Text = string.Empty;
		txtFlatSharePercent.Text = string.Empty;
		cmbParty.Focus();
		gridfill();
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		clear();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		PartyFill();
		LocationTypeFill();
		LocationZoneFill();
	}

	private bool formvalidation()
	{
		if (((Control)(object)cmbParty).Text.Trim() == "")
		{
			MessageBox.Show("Please Select Party");
			cmbParty.Focus();
			return false;
		}
		if (((Control)(object)cmbLocationType).Text.Trim() == "")
		{
			MessageBox.Show("Please Select Location Type");
			cmbLocationType.Focus();
			return false;
		}
		if (txtLocationName.Text.Trim() == "")
		{
			MessageBox.Show("Please Insert Location Name");
			txtLocationName.Focus();
			return false;
		}
		return true;
	}

	private void BenifitDefine_Load(object sender, EventArgs e)
	{
		AutoScroll = true;
		base.AutoScrollMargin = new Size(20, 20);
		base.AutoScrollMinSize = new Size(870, 460);
		btnsave.Visible = true;
		btnupdate.Visible = false;
		gridfill();
		PartyFill();
		LocationTypeFill();
		LocationZoneFill();
		cmbParty.Focus();
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Expected O, but got Unknown
		if (!formvalidation())
		{
			return;
		}
		try
		{
			if (genPartyLocation.Save(new genPartyLocation
			{
				PartyId = Conversion.ToInt(cmbParty.Value),
				PartyLocationTypeProfileId = Conversion.ToInt(cmbLocationType.Value),
				PartyLocationZoneProfileId = Conversion.ToInt(cmbLocationZone.Value),
				PartyLocationName = Conversion.ToString((object)txtLocationName.Text.Trim()),
				PartyLocationCode = Conversion.ToString((object)txtCode.Text.Trim()),
				PartyLocationPrefix = Conversion.ToString((object)txtPrefix.Text.Trim()),
				Mobile1 = Conversion.ToString((object)txtMobile1.Text.Trim()),
				Mobile2 = Conversion.ToString((object)txtMobile2.Text.Trim()),
				FlatSharePercent = Conversion.ToDecimal((object)txtFlatSharePercent.Text.Trim()),
				IsAllowShare = IsAllowShare.Checked,
				IsCashInvoice = IsCashInvoice.Checked,
				IsCreditInvoice = IsCreditInvoice.Checked,
				NeedLogin = NeedLogin.Checked,
				ActionTypeId = 1,
				AddressId = 0,
				AlteredOn = DateTime.Now,
				CompanyId = UserAccount.CompanyId,
				OrganizationId = UserAccount.OrganizationId,
				CreatedOn = DateTime.Now,
				AlteredById = Conversion.ToInt((object)UserAccount.ID),
				CreatedById = Conversion.ToInt((object)UserAccount.ID),
				UserLogId = Conversion.ToInt((object)UserAccount.ID)
			}) > 0)
			{
				MessageBox.Show("Save Successfully");
				clear();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		if (!formvalidation())
		{
			return;
		}
		try
		{
			if (genPartyLocation.Save(new genPartyLocation
			{
				PartyLocationId = Conversion.ToInt((object)RecId),
				PartyId = Conversion.ToInt(cmbParty.Value),
				PartyLocationTypeProfileId = Conversion.ToInt(cmbLocationType.Value),
				PartyLocationZoneProfileId = Conversion.ToInt(cmbLocationZone.Value),
				PartyLocationName = Conversion.ToString((object)txtLocationName.Text.Trim()),
				PartyLocationCode = Conversion.ToString((object)txtCode.Text.Trim()),
				PartyLocationPrefix = Conversion.ToString((object)txtPrefix.Text.Trim()),
				Mobile1 = Conversion.ToString((object)txtMobile1.Text.Trim()),
				Mobile2 = Conversion.ToString((object)txtMobile2.Text.Trim()),
				FlatSharePercent = Conversion.ToDecimal((object)txtFlatSharePercent.Text.Trim()),
				IsAllowShare = IsAllowShare.Checked,
				IsCashInvoice = IsCashInvoice.Checked,
				IsCreditInvoice = IsCreditInvoice.Checked,
				NeedLogin = NeedLogin.Checked,
				ActionTypeId = 1,
				AddressId = 0,
				AlteredOn = DateTime.Now,
				CompanyId = UserAccount.CompanyId,
				OrganizationId = UserAccount.OrganizationId,
				CreatedOn = DateTime.Now,
				AlteredById = Conversion.ToInt((object)UserAccount.ID),
				CreatedById = Conversion.ToInt((object)UserAccount.ID),
				UserLogId = Conversion.ToInt((object)UserAccount.ID)
			}) > 0)
			{
				MessageBox.Show("Update Successfully");
				clear();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable getList = genPartyLocation.Getall(new genPartyLocation
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			DataTable table = new DataTable();
			table.Columns.Add("Id");
			table.Columns.Add("PartyName");
			table.Columns.Add("LocationType");
			table.Columns.Add("LocationZone");
			table.Columns.Add("LocationName");
			table.Columns.Add("Code");
			table.Columns.Add("Prefix");
			table.Columns.Add("Mobile1");
			table.Columns.Add("Mobile2");
			table.Columns.Add("FlatSharePercent", typeof(double));
			table.Columns.Add("NeedLogin", typeof(bool));
			table.Columns.Add("IsAllowShare", typeof(bool));
			table.Columns.Add("IsCreditInvoice", typeof(bool));
			table.Columns.Add("IsCashInvoice", typeof(bool));
			table.Rows.Clear();
			for (int i = 0; i < getList.Rows.Count; i++)
			{
				table.Rows.Add(getList.Rows[i]["PartyLocationId"], getList.Rows[i]["PartyName"], getList.Rows[i]["PartyLocationType"], getList.Rows[i]["PartyLocationZone"], getList.Rows[i]["PartyLocationName"], getList.Rows[i]["PartyLocationCode"], getList.Rows[i]["PartyLocationPrefix"], getList.Rows[i]["Mobile1"], getList.Rows[i]["Mobile2"], getList.Rows[i]["FlatSharePercent"], getList.Rows[i]["NeedLogin"], getList.Rows[i]["IsAllowShare"], getList.Rows[i]["IsCreditInvoice"], getList.Rows[i]["IsCashInvoice"]);
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

	private void GridSetting()
	{
		try
		{
			grdfrm.RootTable.Columns["Id"].Visible = false;
			grdfrm.RootTable.Columns["NeedLogin"].ColumnType = (ColumnType)3;
			grdfrm.RootTable.Columns["IsAllowShare"].ColumnType = (ColumnType)3;
			grdfrm.RootTable.Columns["IsCreditInvoice"].ColumnType = (ColumnType)3;
			grdfrm.RootTable.Columns["IsCashInvoice"].ColumnType = (ColumnType)3;
			grdfrm.RootTable.Columns["PartyName"].Width = 200;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_DoubleClick(object sender, EventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		try
		{
			btnsave.Visible = false;
			btnupdate.Visible = true;
			UpdateMode = true;
			genPartyLocation cat = new genPartyLocation();
			RecId = Conversion.ToInt((object)grdfrm.CurrentRow.Cells["Id"].Value.ToString());
			cat = genPartyLocation.GetByID(RecId);
			cmbParty.Value = Conversion.ToString((object)cat.PartyId);
			cmbLocationType.Value = Conversion.ToString((object)cat.PartyLocationTypeProfileId);
			cmbLocationZone.Value = Conversion.ToString((object)cat.PartyLocationZoneProfileId);
			txtLocationName.Text = Conversion.ToString((object)cat.PartyLocationName);
			txtCode.Text = Conversion.ToString((object)cat.PartyLocationCode);
			txtPrefix.Text = Conversion.ToString((object)cat.PartyLocationPrefix);
			txtMobile1.Text = Conversion.ToString((object)cat.Mobile1);
			txtMobile2.Text = Conversion.ToString((object)cat.Mobile2);
			txtFlatSharePercent.Text = Conversion.ToString((object)cat.FlatSharePercent);
			IsAllowShare.Checked = Conversion.ToBool((object)cat.IsAllowShare);
			NeedLogin.Checked = Conversion.ToBool((object)cat.NeedLogin);
			IsCreditInvoice.Checked = Conversion.ToBool((object)cat.IsCreditInvoice);
			IsCashInvoice.Checked = Conversion.ToBool((object)cat.IsCashInvoice);
			cmbParty.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BenifitDefine_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S && !UpdateMode)
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
			if (e.Control && e.KeyCode == Keys.U && UpdateMode)
			{
				btnupdate_Click(null, null);
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

	private void btnParty_Click(object sender, EventArgs e)
	{
		try
		{
			new frmgenParty(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLocationtype_Click(object sender, EventArgs e)
	{
		try
		{
			ProfileDefine profileDefine = new ProfileDefine(UserAccount);
			profileDefine.cmbprofiletype.Value = 63;
			profileDefine.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnlocationZone_Click(object sender, EventArgs e)
	{
		try
		{
			ProfileDefine profileDefine = new ProfileDefine(UserAccount);
			profileDefine.cmbprofiletype.Value = 64;
			profileDefine.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFlatSharePercent_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtMobile2_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtMobile1_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
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
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Expected O, but got Unknown
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Expected O, but got Unknown
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Expected O, but got Unknown
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.HRM.EmployeeManagement.frmgenPartyLocation));
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
		Appearance appearance23 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
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
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label50 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.label8 = new System.Windows.Forms.Label();
		this.txtFlatSharePercent = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.txtMobile2 = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.txtMobile1 = new System.Windows.Forms.TextBox();
		this.IsCashInvoice = new System.Windows.Forms.CheckBox();
		this.IsCreditInvoice = new System.Windows.Forms.CheckBox();
		this.IsAllowShare = new System.Windows.Forms.CheckBox();
		this.NeedLogin = new System.Windows.Forms.CheckBox();
		this.label6 = new System.Windows.Forms.Label();
		this.txtLocationName = new System.Windows.Forms.TextBox();
		this.cmbLocationZone = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtPrefix = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtCode = new System.Windows.Forms.TextBox();
		this.cmbLocationType = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.cmbParty = new UltraCombo();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.btnlocationZone = new System.Windows.Forms.Button();
		this.btnLocationtype = new System.Windows.Forms.Button();
		this.btnParty = new System.Windows.Forms.Button();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label10 = new System.Windows.Forms.Label();
		this.grdfrm = new GridEX();
		this.tsItemType.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLocationZone).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLocationType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbParty).BeginInit();
		this.panel2.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		base.SuspendLayout();
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(65, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(79, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(60, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnnew, this.btnupdate, this.btnsave, this.btnRefresh });
		this.tsItemType.Location = new System.Drawing.Point(0, 0);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(854, 27);
		this.tsItemType.TabIndex = 1;
		this.tsItemType.Text = "toolStrip4";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(804, -1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 27);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(13, 35);
		this.label50.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(34, 15);
		this.label50.TabIndex = 2;
		this.label50.Text = "Party";
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.btnParty);
		this.panel1.Controls.Add(this.btnLocationtype);
		this.panel1.Controls.Add(this.btnlocationZone);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.label8);
		this.panel1.Controls.Add(this.txtFlatSharePercent);
		this.panel1.Controls.Add(this.label7);
		this.panel1.Controls.Add(this.txtMobile2);
		this.panel1.Controls.Add(this.label5);
		this.panel1.Controls.Add(this.txtMobile1);
		this.panel1.Controls.Add(this.IsCashInvoice);
		this.panel1.Controls.Add(this.IsCreditInvoice);
		this.panel1.Controls.Add(this.IsAllowShare);
		this.panel1.Controls.Add(this.NeedLogin);
		this.panel1.Controls.Add(this.label6);
		this.panel1.Controls.Add(this.txtLocationName);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLocationZone);
		this.panel1.Controls.Add(this.label1);
		this.panel1.Controls.Add(this.label4);
		this.panel1.Controls.Add(this.txtPrefix);
		this.panel1.Controls.Add(this.label2);
		this.panel1.Controls.Add(this.txtCode);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLocationType);
		this.panel1.Controls.Add(this.label3);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbParty);
		this.panel1.Controls.Add(this.label50);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 27);
		this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(854, 181);
		this.panel1.TabIndex = 0;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(13, 133);
		this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(102, 15);
		this.label8.TabIndex = 25;
		this.label8.Text = "Flat Share Percent";
		this.txtFlatSharePercent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFlatSharePercent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFlatSharePercent.Location = new System.Drawing.Point(13, 151);
		this.txtFlatSharePercent.Margin = new System.Windows.Forms.Padding(4);
		this.txtFlatSharePercent.Name = "txtFlatSharePercent";
		this.txtFlatSharePercent.Size = new System.Drawing.Size(208, 23);
		this.txtFlatSharePercent.TabIndex = 11;
		this.txtFlatSharePercent.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFlatSharePercent_KeyPress);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(643, 86);
		this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(51, 15);
		this.label7.TabIndex = 23;
		this.label7.Text = "Mobile2";
		this.txtMobile2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMobile2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMobile2.Location = new System.Drawing.Point(643, 102);
		this.txtMobile2.Margin = new System.Windows.Forms.Padding(4);
		this.txtMobile2.Name = "txtMobile2";
		this.txtMobile2.Size = new System.Drawing.Size(199, 23);
		this.txtMobile2.TabIndex = 10;
		this.txtMobile2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtMobile2_KeyPress);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(438, 86);
		this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(49, 15);
		this.label5.TabIndex = 21;
		this.label5.Text = "Mobile1";
		this.txtMobile1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMobile1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMobile1.Location = new System.Drawing.Point(438, 102);
		this.txtMobile1.Margin = new System.Windows.Forms.Padding(4);
		this.txtMobile1.Name = "txtMobile1";
		this.txtMobile1.Size = new System.Drawing.Size(202, 23);
		this.txtMobile1.TabIndex = 9;
		this.txtMobile1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtMobile1_KeyPress);
		this.IsCashInvoice.AutoSize = true;
		this.IsCashInvoice.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.IsCashInvoice.Location = new System.Drawing.Point(341, 155);
		this.IsCashInvoice.Name = "IsCashInvoice";
		this.IsCashInvoice.Size = new System.Drawing.Size(105, 19);
		this.IsCashInvoice.TabIndex = 15;
		this.IsCashInvoice.Text = "Is Cash Invoice";
		this.IsCashInvoice.UseVisualStyleBackColor = true;
		this.IsCreditInvoice.AutoSize = true;
		this.IsCreditInvoice.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.IsCreditInvoice.Location = new System.Drawing.Point(341, 135);
		this.IsCreditInvoice.Name = "IsCreditInvoice";
		this.IsCreditInvoice.Size = new System.Drawing.Size(111, 19);
		this.IsCreditInvoice.TabIndex = 14;
		this.IsCreditInvoice.Text = "Is Credit Invoice";
		this.IsCreditInvoice.UseVisualStyleBackColor = true;
		this.IsAllowShare.AutoSize = true;
		this.IsAllowShare.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.IsAllowShare.Location = new System.Drawing.Point(229, 155);
		this.IsAllowShare.Name = "IsAllowShare";
		this.IsAllowShare.Size = new System.Drawing.Size(101, 19);
		this.IsAllowShare.TabIndex = 13;
		this.IsAllowShare.Text = "Is Allow Share";
		this.IsAllowShare.UseVisualStyleBackColor = true;
		this.NeedLogin.AutoSize = true;
		this.NeedLogin.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.NeedLogin.Location = new System.Drawing.Point(229, 135);
		this.NeedLogin.Name = "NeedLogin";
		this.NeedLogin.Size = new System.Drawing.Size(87, 19);
		this.NeedLogin.TabIndex = 12;
		this.NeedLogin.Text = "Need Login";
		this.NeedLogin.UseVisualStyleBackColor = true;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(643, 35);
		this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(88, 15);
		this.label6.TabIndex = 19;
		this.label6.Text = "Location Name";
		this.txtLocationName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLocationName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLocationName.Location = new System.Drawing.Point(643, 56);
		this.txtLocationName.Margin = new System.Windows.Forms.Padding(4);
		this.txtLocationName.Name = "txtLocationName";
		this.txtLocationName.Size = new System.Drawing.Size(199, 23);
		this.txtLocationName.TabIndex = 6;
		this.cmbLocationZone.AllowNull = (DefaultableBoolean)1;
		this.cmbLocationZone.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLocationZone.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLocationZone.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLocationZone).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbLocationZone).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLocationZone).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLocationZone).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLocationZone).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbLocationZone).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLocationZone).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbLocationZone).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLocationZone).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLocationZone).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLocationZone).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbLocationZone).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLocationZone).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLocationZone).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLocationZone).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbLocationZone).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLocationZone).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLocationZone).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLocationZone).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbLocationZone).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLocationZone).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLocationZone).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbLocationZone).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLocationZone).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbLocationZone).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLocationZone).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLocationZone).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLocationZone).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLocationZone.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLocationZone).Location = new System.Drawing.Point(438, 54);
		((System.Windows.Forms.Control)(object)this.cmbLocationZone).Name = "cmbLocationZone";
		((System.Windows.Forms.Control)(object)this.cmbLocationZone).Size = new System.Drawing.Size(171, 26);
		((System.Windows.Forms.Control)(object)this.cmbLocationZone).TabIndex = 4;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(438, 35);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(83, 15);
		this.label1.TabIndex = 17;
		this.label1.Text = "Location Zone";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(225, 86);
		this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(86, 15);
		this.label4.TabIndex = 12;
		this.label4.Text = "Location Prefix";
		this.txtPrefix.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPrefix.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPrefix.Location = new System.Drawing.Point(225, 102);
		this.txtPrefix.Margin = new System.Windows.Forms.Padding(4);
		this.txtPrefix.Name = "txtPrefix";
		this.txtPrefix.Size = new System.Drawing.Size(209, 23);
		this.txtPrefix.TabIndex = 8;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(13, 86);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(83, 15);
		this.label2.TabIndex = 10;
		this.label2.Text = "Location Code";
		this.txtCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCode.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCode.Location = new System.Drawing.Point(13, 102);
		this.txtCode.Margin = new System.Windows.Forms.Padding(4);
		this.txtCode.Name = "txtCode";
		this.txtCode.Size = new System.Drawing.Size(208, 23);
		this.txtCode.TabIndex = 7;
		this.cmbLocationType.AllowNull = (DefaultableBoolean)1;
		this.cmbLocationType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLocationType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLocationType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance35).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLocationType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Location = new System.Drawing.Point(225, 54);
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Name = "cmbLocationType";
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Size = new System.Drawing.Size(178, 26);
		((System.Windows.Forms.Control)(object)this.cmbLocationType).TabIndex = 2;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(225, 35);
		this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(81, 15);
		this.label3.TabIndex = 7;
		this.label3.Text = "Location Type";
		this.cmbParty.AllowNull = (DefaultableBoolean)1;
		this.cmbParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.cmbParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbParty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbParty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbParty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbParty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbParty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbParty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbParty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbParty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbParty).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbParty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbParty).Location = new System.Drawing.Point(13, 54);
		((System.Windows.Forms.Control)(object)this.cmbParty).Name = "cmbParty";
		((System.Windows.Forms.Control)(object)this.cmbParty).Size = new System.Drawing.Size(178, 26);
		((System.Windows.Forms.Control)(object)this.cmbParty).TabIndex = 0;
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(0, 208);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(854, 153);
		this.panel2.TabIndex = 21;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label9);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(854, 28);
		this.panel4.TabIndex = 26;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(4, 4);
		this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(127, 19);
		this.label9.TabIndex = 8;
		this.label9.Text = "Party Location";
		this.btnlocationZone.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnlocationZone.Location = new System.Drawing.Point(611, 54);
		this.btnlocationZone.Name = "btnlocationZone";
		this.btnlocationZone.Size = new System.Drawing.Size(30, 26);
		this.btnlocationZone.TabIndex = 5;
		this.btnlocationZone.TabStop = false;
		this.btnlocationZone.UseVisualStyleBackColor = true;
		this.btnlocationZone.Click += new System.EventHandler(btnlocationZone_Click);
		this.btnLocationtype.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnLocationtype.Location = new System.Drawing.Point(406, 54);
		this.btnLocationtype.Name = "btnLocationtype";
		this.btnLocationtype.Size = new System.Drawing.Size(30, 26);
		this.btnLocationtype.TabIndex = 3;
		this.btnLocationtype.TabStop = false;
		this.btnLocationtype.UseVisualStyleBackColor = true;
		this.btnLocationtype.Click += new System.EventHandler(btnLocationtype_Click);
		this.btnParty.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnParty.Location = new System.Drawing.Point(194, 54);
		this.btnParty.Name = "btnParty";
		this.btnParty.Size = new System.Drawing.Size(30, 26);
		this.btnParty.TabIndex = 1;
		this.btnParty.TabStop = false;
		this.btnParty.UseVisualStyleBackColor = true;
		this.btnParty.Click += new System.EventHandler(btnParty_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label10);
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(854, 26);
		this.panel3.TabIndex = 20;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label10.ForeColor = System.Drawing.Color.White;
		this.label10.Location = new System.Drawing.Point(4, 4);
		this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(58, 17);
		this.label10.TabIndex = 8;
		this.label10.Text = "History";
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupByBoxVisible = false;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.grdfrm).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(854, 127);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 21;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdfrm).DoubleClick += new System.EventHandler(grdfrm_DoubleClick);
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(854, 361);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.tsItemType);
		this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmgenPartyLocation";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(BenifitDefine_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(BenifitDefine_KeyDown);
		this.tsItemType.ResumeLayout(false);
		this.tsItemType.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLocationZone).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLocationType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbParty).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

}
