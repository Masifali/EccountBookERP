using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.FixedAssets;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.FixedAssets;
using Architecture.Model.Inventory;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Lookups;
using Architecture.WinApp.Properties;
using Architecture.WinApp.StoreManagement;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.FixedAsset;

public class frmFixedAssetRegister : Form
{
	private int RECID;

	private bool UpdateMode;

	private bool flag;

	private string saveDirectory = "";

	private string fileSavePath = "";

	private string ofd = "";

	private string fileSavePath2 = "";

	private string ofd2 = "";

	private DataTable coaAllocationBind = new DataTable();

	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formrights = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel4;

	private GroupBox groupBox1;

	private Button btnImageBrowse2;

	private Button btnImageBrowse1;

	private Label label21;

	internal UltraCombo cmbAccomolativeAcc;

	private Label label20;

	internal UltraCombo cmbDepriciationAcc;

	private Label label19;

	internal UltraCombo cmbGlAccount;

	private Label label18;

	internal UltraCombo CmbAssetItem;

	private ComboBox cmbAssetStatus;

	private Label label17;

	private Label label16;

	private TextBox txtCurrentValue;

	private TextBox txtPurchasePrice;

	private Label label14;

	private TextBox txtUseableLifeMonth;

	private Label label13;

	private DateTimePicker txtExpiryDate;

	private Label label12;

	private DateTimePicker txtPurchaseDate;

	private Label label11;

	private TextBox txtVendor;

	private Label label10;

	internal UltraCombo cmbDepartment;

	private Label label9;

	private TextBox txtAssetLocation;

	private Label label8;

	private TextBox txtAssetCondition;

	private Label label7;

	private TextBox txtAssetSerialNo;

	private Label label6;

	private TextBox txtManufacturer;

	private Label label4;

	private TextBox txtModelDesc;

	private Label label3;

	private TextBox txtMakeDesc;

	private Label label2;

	private TextBox txtBrandName;

	private Label label1;

	private TextBox txtAssetName;

	private Label label5;

	internal UltraCombo CmbAssetCategory;

	private Label label24;

	private Panel panel2;

	private TabPage tabPage2;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private ToolStripButton btnSave;

	private ToolStripButton btnUpdate;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripButton btnPrint;

	private Panel panel3;

	private Panel panel6;

	private ToolStrip toolStrip1;

	private ToolStripButton BtnNewHistory;

	private Label label22;

	internal GridEX DatagridHistory;

	private CtrlGrdBar ctrlGrdBar1;

	private GroupBox BrandImages;

	private PictureBox picbox2;

	private PictureBox picbox1;

	private ToolStripButton BtnRefresh;

	private ToolStripButton BtnShortCutkeys;

	private Panel panel1;

	private GroupBox groupBox2;

	internal UltraCombo cmbCategoryHistory;

	private Label label23;

	private RadioButton rdmodifydate;

	private RadioButton rdentrydate;

	private Button btnShow;

	private Label label53;

	private Label label54;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Label label55;

	private ToolStripButton btnRefreshHistory;

	private Button BtnDefineAssetDepartment;

	private Button BtnAssetItemDefine;

	private Button btnAssetCategory;

	internal UltraCombo CmbExpenseMaintenanceAccount;

	private Label label15;

	public frmFixedAssetRegister(UserAccount user)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmFixedAssetRegister_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 700);
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnSave.Enabled = formrights.DoHaveSaveRight;
			btnUpdate.Enabled = formrights.DoHaveUpdateRights;
			btnPrint.Enabled = formrights.DoHavePrintRights;
			ctrlGrdBar1.mGridChooseFielder.Visible = false;
			ctrlGrdBar1.mGridSaveLayouts.Visible = false;
			ctrlGrdBar1.removeLayoutToolStripMenuItem.Visible = false;
			ItemCategory();
			AllAccountsWithTypeIdDbCall();
			DepriciationAccountFill();
			GlAndAccomolativeAccountBind();
			ExpenseMaintenanceAccountFill();
			ItemFill();
			Department();
			CmbAssetCategory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllAccountsWithTypeIdDbCall()
	{
		try
		{
			coaAllocationBind.Clear();
			coaAllocationBind = CommonServices.Accounts_GetAccountTitleByAccountTypeIds();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable GetAccountByTypeIdFromAllAccountsWithTypeId(string accountTypeIds)
	{
		try
		{
			int result;
			HashSet<int> accountTypeIdSet = new HashSet<int>((from id in accountTypeIds.Split(',')
				select id.Trim() into id
				where int.TryParse(id, out result)
				select id).Select(int.Parse));
			DataTable dt = new DataTable();
			dt.Columns.AddRange(new DataColumn[5]
			{
				new DataColumn("Id", typeof(int)),
				new DataColumn("AccountTitle", typeof(string)),
				new DataColumn("AccountCode", typeof(string)),
				new DataColumn("ParentAccountTitle", typeof(string)),
				new DataColumn("AccountClass", typeof(string))
			});
			foreach (DataRow dr in coaAllocationBind.Rows)
			{
				int accountTypeId = Conversion.ToInt(dr["AccountTypeId"]);
				if (accountTypeIdSet.Contains(accountTypeId))
				{
					dt.Rows.Add(dr["Id"], dr["AccountTitle"], dr["AccountCode"], dr["ParentAccountTitle"], dr["AccountClass"]);
				}
			}
			return dt;
		}
		catch (Exception ex)
		{
			throw new Exception("Error while filtering accounts by type ID: " + ex.Message, ex);
		}
	}

	private void DepriciationAccountFill()
	{
		try
		{
			DataTable dt = GetAccountByTypeIdFromAllAccountsWithTypeId("14");
			InfragisticsHelper.BindAndRetainSelection(cmbDepriciationAcc, dt, "Id", "AccountTitle", "Depreciation Account", AllColumns: true, Conversion.ToInt(cmbDepriciationAcc.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)cmbDepriciationAcc).DisplayLayout.Bands[0].Columns[1].Width = 350;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GlAndAccomolativeAccountBind()
	{
		try
		{
			DataTable dt = GetAccountByTypeIdFromAllAccountsWithTypeId("1");
			InfragisticsHelper.BindAndRetainSelection(cmbAccomolativeAcc, dt, "Id", "AccountTitle", "Accumulative Account", AllColumns: true, Conversion.ToInt(cmbAccomolativeAcc.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(cmbGlAccount, dt, "Id", "AccountTitle", "Assets GL Account", AllColumns: true, Conversion.ToInt(cmbGlAccount.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)cmbAccomolativeAcc).DisplayLayout.Bands[0].Columns[1].Width = 350;
				((UltraGridBase)cmbGlAccount).DisplayLayout.Bands[0].Columns[1].Width = 350;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExpenseMaintenanceAccountFill()
	{
		try
		{
			DataTable dt = GetAccountByTypeIdFromAllAccountsWithTypeId("23");
			InfragisticsHelper.BindAndRetainSelection(CmbExpenseMaintenanceAccount, dt, "Id", "AccountTitle", "Expense Maintenance Account", AllColumns: true, Conversion.ToInt(CmbExpenseMaintenanceAccount.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)CmbExpenseMaintenanceAccount).DisplayLayout.Bands[0].Columns[1].Width = 350;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ItemFill()
	{
		try
		{
			int Id = Conversion.ToInt(CmbAssetItem.Value);
			DataTable dtitem = CommonServices.ReadAllItemsByParentId(5);
			if (dtitem.Rows.Count > 0)
			{
				DDL.BindDDL(dtitem, CmbAssetItem, "Id", "ItemName", "Item Name", ZeroIndex: false);
				if (Id > 0)
				{
					if (dtitem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbAssetItem.Value = Id;
					}
					else
					{
						((Control)(object)CmbAssetItem).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbAssetItem).DataSource = null;
				((Control)(object)CmbAssetItem).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemCategory()
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		try
		{
			DataTable dtCategory = new DataTable();
			dtCategory.Columns.Add("Id", typeof(int));
			dtCategory.Columns.Add("Category");
			dtCategory.Clear();
			int Id = Conversion.ToInt(CmbAssetCategory.Value);
			int Id2 = Conversion.ToInt(cmbCategoryHistory.Value);
			DataTable dt = ItemCategory.Getall(new ItemCategory
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				CategoryCode = "5"
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				dtCategory.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["CategoryDescription"]);
			}
			if (dtCategory.Rows.Count > 0)
			{
				DDL.BindDDL(dtCategory, CmbAssetCategory, "Id", "Category", "Category Name", ZeroIndex: true);
				DDL.BindDDL(dtCategory, cmbCategoryHistory, "Id", "Category", "Category Name", ZeroIndex: true);
				if (Id > 0)
				{
					if (dtCategory.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbAssetCategory.Value = Id;
					}
					else
					{
						((Control)(object)CmbAssetCategory).Text = string.Empty;
					}
				}
				if (Id2 > 0)
				{
					if (dtCategory.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id2))
					{
						cmbCategoryHistory.Value = Id2;
					}
					else
					{
						((Control)(object)cmbCategoryHistory).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbAssetCategory).DataSource = null;
				((Control)(object)CmbAssetCategory).Text = string.Empty;
				((UltraGridBase)cmbCategoryHistory).DataSource = null;
				((Control)(object)cmbCategoryHistory).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Department()
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		try
		{
			DataTable dtDepartment = new DataTable();
			dtDepartment.Columns.Add("Id", typeof(int));
			dtDepartment.Columns.Add("Department");
			dtDepartment.Clear();
			int Id = Conversion.ToInt(cmbDepartment.Value);
			DataTable lst = Department.GetAll(new Department
			{
				CompanyId = UserAccount.CompanyId,
				OrganizationId = UserAccount.OrganizationId
			});
			for (int i = 0; i < lst.Rows.Count; i++)
			{
				dtDepartment.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["DepartmentName"]);
			}
			if (dtDepartment.Rows.Count > 0)
			{
				DDL.BindDDL(dtDepartment, cmbDepartment, "Id", "Department", "Department", ZeroIndex: true);
				if (Id > 0)
				{
					if (dtDepartment.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						cmbDepartment.Value = Id;
					}
					else
					{
						((Control)(object)cmbDepartment).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)cmbDepartment).DataSource = null;
				((Control)(object)cmbDepartment).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)CmbAssetCategory).ActiveRow == null || Conversion.ToInt(CmbAssetCategory.Value) == 0)
		{
			MessageBox.Show("Asset Category Field is Required");
			CmbAssetCategory.Focus();
			return false;
		}
		if (((UltraGridBase)CmbAssetItem).ActiveRow == null || Conversion.ToInt(CmbAssetItem.Value) == 0)
		{
			MessageBox.Show("Asset Item Field is Required");
			CmbAssetItem.Focus();
			return false;
		}
		if (txtBrandName.Text.Trim() == "")
		{
			MessageBox.Show("Brand Name Field is Required");
			txtBrandName.Focus();
			return false;
		}
		if (txtAssetName.Text.Trim() == "")
		{
			MessageBox.Show("Asset Name Field is Required");
			txtAssetName.Focus();
			return false;
		}
		if (((UltraGridBase)cmbGlAccount).ActiveRow == null || Conversion.ToInt(cmbGlAccount.Value) == 0)
		{
			MessageBox.Show("Asset GL A/c Field is Required");
			cmbGlAccount.Focus();
			return false;
		}
		if (((UltraGridBase)cmbDepriciationAcc).ActiveRow == null || Conversion.ToInt(cmbDepriciationAcc.Value) == 0)
		{
			MessageBox.Show("Depreciation_Expense A/c Field is Required");
			cmbDepriciationAcc.Focus();
			return false;
		}
		if (((UltraGridBase)cmbAccomolativeAcc).ActiveRow == null || Conversion.ToInt(cmbAccomolativeAcc.Value) == 0)
		{
			MessageBox.Show("Accumulated_Depreciation A/c Filed is Required");
			cmbAccomolativeAcc.Focus();
			return false;
		}
		return true;
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
			FixedAssetsRegister fixedAssets = new FixedAssetsRegister();
			if (RECID > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				fixedAssets.Id = RECID;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			fixedAssets.CompanyId = UserAccount.CompanyId;
			fixedAssets.OrganizationId = UserAccount.OrganizationId;
			fixedAssets.BranchesId = UserAccount.BranchesId;
			fixedAssets.ProjectsId = UserAccount.BranchesId;
			fixedAssets.EntryUserId = UserAccount.ID;
			fixedAssets.EntryDate = DateTime.Now;
			fixedAssets.ApprovedDate = DateTime.Now;
			fixedAssets.ApprovedUserId = UserAccount.ID;
			fixedAssets.IsApproved = false;
			fixedAssets.FixedAssetsCategoryId = Conversion.ToInt(CmbAssetCategory.Value);
			fixedAssets.ItemId = Conversion.ToInt(CmbAssetItem.Value);
			fixedAssets.Brand = Conversion.ToString((object)txtBrandName.Text);
			fixedAssets.AssetName = Conversion.ToString((object)txtAssetName.Text);
			fixedAssets.ModelDesc = Conversion.ToString((object)txtModelDesc.Text);
			fixedAssets.AssetCondition = Conversion.ToString((object)txtAssetCondition.Text);
			fixedAssets.AssetSerialNo = Conversion.ToString((object)txtAssetSerialNo.Text);
			fixedAssets.AssetsDepartmentId = Conversion.ToInt(cmbDepartment.Value);
			fixedAssets.Manufacturer = Conversion.ToString((object)txtManufacturer.Text);
			fixedAssets.MakeDesc = Conversion.ToString((object)txtMakeDesc.Text);
			fixedAssets.AssetLocationId = Conversion.ToString((object)txtAssetLocation.Text);
			fixedAssets.Vendor = Conversion.ToString((object)txtVendor.Text);
			fixedAssets.PurchaseDate = Conversion.ToDateTime((object)txtPurchaseDate.Value);
			fixedAssets.PurchasePrice = Conversion.ToDouble((object)txtPurchasePrice.Text);
			fixedAssets.ExpiryDate = Conversion.ToDateTime((object)txtExpiryDate.Value);
			fixedAssets.CurrentValue = Conversion.ToDouble((object)txtCurrentValue.Text);
			fixedAssets.UseableLifeMonth = Conversion.ToInt((object)txtUseableLifeMonth.Text);
			fixedAssets.AssetStatus = Conversion.ToString((object)cmbAssetStatus.Text);
			fixedAssets.AssetsType = "";
			fixedAssets.AssetGLAccountId = Conversion.ToInt(cmbGlAccount.Value);
			fixedAssets.AssetDepriciationAcId = Conversion.ToInt(cmbDepriciationAcc.Value);
			fixedAssets.AssetAcmltvAcId = Conversion.ToInt(cmbAccomolativeAcc.Value);
			fixedAssets.ExpenseMaintenanceAccountId = Conversion.ToInt(CmbExpenseMaintenanceAccount.Value);
			fixedAssets.Pic1Path = Conversion.ToString((object)fileSavePath);
			fixedAssets.Pic2Path = Conversion.ToString((object)fileSavePath2);
			FixedAssetsRegister.Save(fixedAssets);
			if (RECID > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + fixedAssets.AssetName);
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + fixedAssets.AssetName);
			}
			if (ofd != null && ofd != string.Empty && File.Exists(ofd))
			{
				File.Copy(ofd, fileSavePath, overwrite: true);
				ofd = string.Empty;
				fileSavePath = string.Empty;
				picbox1.Image = null;
				picbox1.Invalidate();
			}
			if (ofd2 != null && ofd2 != string.Empty && File.Exists(ofd2))
			{
				File.Copy(ofd2, fileSavePath2, overwrite: true);
				ofd2 = string.Empty;
				fileSavePath2 = string.Empty;
				picbox2.Image = null;
				picbox2.Invalidate();
			}
			FormReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		try
		{
			RECID = 0;
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
			if (RECID == 0)
			{
				throw new Exception("Record Not Update because RecId Not Found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ReadById(int Id)
	{
		try
		{
			RECID = Id;
			FixedAssetsRegister SC = FixedAssetsRegister.GetById(RECID);
			if (SC == null)
			{
				return;
			}
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			UpdateMode = true;
			tabControl1.SelectedIndex = 0;
			CmbAssetCategory.Value = SC.FixedAssetsCategoryId;
			CmbAssetItem.Value = SC.ItemId;
			txtBrandName.Text = SC.Brand;
			txtAssetName.Text = SC.AssetName;
			txtModelDesc.Text = SC.ModelDesc;
			txtAssetCondition.Text = SC.AssetCondition;
			txtAssetSerialNo.Text = SC.AssetSerialNo;
			cmbDepartment.Value = SC.AssetsDepartmentId;
			txtManufacturer.Text = SC.Manufacturer;
			txtMakeDesc.Text = SC.MakeDesc;
			txtAssetLocation.Text = SC.AssetLocationId;
			txtVendor.Text = SC.Vendor;
			txtPurchaseDate.Value = SC.PurchaseDate;
			txtPurchasePrice.Text = SC.PurchasePrice.ToString();
			txtExpiryDate.Value = SC.ExpiryDate;
			txtCurrentValue.Text = SC.CurrentValue.ToString();
			txtUseableLifeMonth.Text = SC.UseableLifeMonth.ToString();
			cmbAssetStatus.Text = SC.AssetStatus;
			cmbAccomolativeAcc.Value = SC.AssetAcmltvAcId;
			cmbDepriciationAcc.Value = SC.AssetDepriciationAcId;
			cmbGlAccount.Value = SC.AssetGLAccountId;
			if (SC.ExpenseMaintenanceAccountId > 0)
			{
				CmbExpenseMaintenanceAccount.Value = SC.ExpenseMaintenanceAccountId;
			}
			fileSavePath = SC.Pic1Path;
			if (fileSavePath != "")
			{
				if (File.Exists(fileSavePath))
				{
					picbox1.Image = Image.FromFile(SC.Pic1Path);
					fileSavePath = SC.Pic1Path;
				}
				else
				{
					fileSavePath = "";
					picbox1.Image = null;
					picbox1.Invalidate();
				}
			}
			else
			{
				fileSavePath = "";
				picbox1.Image = null;
				picbox1.Invalidate();
			}
			fileSavePath2 = SC.Pic2Path;
			if (fileSavePath2 != "")
			{
				if (File.Exists(fileSavePath2))
				{
					picbox2.Image = Image.FromFile(SC.Pic2Path);
					fileSavePath2 = SC.Pic2Path;
				}
				else
				{
					fileSavePath2 = "";
					picbox2.Image = null;
					picbox2.Invalidate();
				}
			}
			else
			{
				fileSavePath2 = "";
				picbox2.Image = null;
				picbox2.Invalidate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		FormReset();
	}

	public void FormReset()
	{
		AT = new Attachment();
		Attachment.dtAttachments.Rows.Clear();
		btnSave.Visible = true;
		btnUpdate.Visible = false;
		UpdateMode = false;
		txtAssetCondition.Text = string.Empty;
		txtAssetLocation.Text = string.Empty;
		txtAssetSerialNo.Text = string.Empty;
		txtAssetName.Text = string.Empty;
		txtBrandName.Text = string.Empty;
		txtCurrentValue.Text = string.Empty;
		txtExpiryDate.Value = DateTime.Now;
		txtMakeDesc.Text = string.Empty;
		txtManufacturer.Text = string.Empty;
		txtModelDesc.Text = string.Empty;
		txtPurchaseDate.Value = DateTime.Now;
		txtPurchasePrice.Text = string.Empty;
		txtVendor.Text = string.Empty;
		((Control)(object)cmbAccomolativeAcc).Text = string.Empty;
		((Control)(object)CmbAssetCategory).Text = string.Empty;
		cmbAssetStatus.Text = string.Empty;
		((Control)(object)cmbDepartment).Text = string.Empty;
		((Control)(object)cmbDepriciationAcc).Text = string.Empty;
		((Control)(object)CmbExpenseMaintenanceAccount).Text = string.Empty;
		((Control)(object)cmbGlAccount).Text = string.Empty;
		((Control)(object)CmbAssetItem).Text = string.Empty;
		CmbAssetCategory.Focus();
		ofd = string.Empty;
		fileSavePath = string.Empty;
		picbox1.Image = null;
		ofd2 = string.Empty;
		fileSavePath2 = string.Empty;
		picbox2.Image = null;
	}

	private void BtnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			ItemCategory();
			AllAccountsWithTypeIdDbCall();
			DepriciationAccountFill();
			GlAndAccomolativeAccountBind();
			ExpenseMaintenanceAccountFill();
			ItemFill();
			Department();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmFixedAssetRegister_FormClosing(object sender, FormClosingEventArgs e)
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

	private void BtnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			ToDateHistory.Value = DateTime.Now;
			DatagridHistory.DataSource = null;
			rdentrydate.Checked = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			ItemCategory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnShow_Click_1(object sender, EventArgs e)
	{
		try
		{
			Gridbind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void Gridbind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.CompanyId = UserAccount.CompanyId;
			obj.OrganizationId = UserAccount.OrganizationId;
			if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.ModifyDateTo = ToDateHistory.Value;
				}
			}
			obj.ItemCategoryId = Conversion.ToInt(cmbCategoryHistory.Value);
			DataTable lst = FixedAssetsRegister.GetHistory(obj);
			if (lst != null && lst.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("Category");
				dt.Columns.Add("AssetItemName");
				dt.Columns.Add("AssetName");
				dt.Columns.Add("Model");
				dt.Columns.Add("Brand");
				dt.Columns.Add("Department");
				dt.Columns.Add("Vendor");
				dt.Columns.Add("PurchaseDate");
				dt.Columns.Add("AssetCondition");
				dt.Columns.Add("PurchasePrice");
				dt.Columns.Add("Manufacturer");
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("ApprovedDate", typeof(DateTime));
				dt.Columns.Add("ApprovedUser", typeof(string));
				foreach (DataRow i in lst.Rows)
				{
					dt.Rows.Add(i["Id"], i["AssetsCategory"], i["ItemName"], i["AssetName"], i["ModelDesc"], i["Brand"], i["DepartmentName"], i["Vendor"], i["PurchaseDate"], i["AssetCondition"], i["PurchasePrice"], i["Manufacturer"], i["EntryDate"], i["EntryUser"], i["ApprovedDate"], i["ModifyUser"]);
				}
				DatagridHistory.DataSource = dt;
				DatagridHistory.RetrieveStructure();
				DatagridHistory.ColumnAutoResize = false;
				DatagridHistory.RootTable.Columns["Id"].Visible = false;
				DatagridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
				DatagridHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yy hh:mm tt";
				DatagridHistory.RootTable.HeaderLines = 2;
				foreach (GridEXColumn item in (JanusCollectionBase)DatagridHistory.RootTable.Columns)
				{
					item.HeaderAlignment = (TextAlignment)2;
				}
				DatagridHistory.RootTable.Columns["Category"].Width = Constants.InventoryConstants.CategoryDescription;
				DatagridHistory.RootTable.Columns["AssetItemName"].Width = Constants.InventoryConstants.ItemName;
				DatagridHistory.RootTable.Columns["AssetName"].Width = Constants.InventoryConstants.ItemName;
				DatagridHistory.RootTable.Columns["Model"].Width = Constants.InventoryConstants.ItemCode;
				DatagridHistory.RootTable.Columns["Brand"].Width = Constants.InventoryConstants.ItemName;
				DatagridHistory.RootTable.Columns["Department"].Width = Constants.InventoryConstants.OtherDescription;
				DatagridHistory.RootTable.Columns["Vendor"].Width = Constants.InventoryConstants.SupplierName;
				DatagridHistory.RootTable.Columns["PurchaseDate"].Width = Constants.InventoryConstants.DateConstant;
				DatagridHistory.RootTable.Columns["AssetCondition"].Width = Constants.InventoryConstants.StatusConstant;
				DatagridHistory.RootTable.Columns["PurchasePrice"].Width = Constants.InventoryConstants.AmountConstant;
				DatagridHistory.RootTable.Columns["Manufacturer"].Width = Constants.InventoryConstants.SupplierName;
				DatagridHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
				DatagridHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
				DatagridHistory.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
				DatagridHistory.RootTable.Columns["ApprovedUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			}
			else
			{
				DatagridHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				ReadById(CurrId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 1)
			{
				FromDateHistory.Focus();
			}
			else
			{
				CmbAssetItem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void definition()
	{
		string value = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Attachment Folder Path").ToList()[0].ConfigKey.ToString();
		if (value != null)
		{
			saveDirectory = value;
			flag = true;
		}
		else
		{
			MessageBox.Show("Please Create the Path in Configuration");
		}
	}

	private void btnImageBrowse1_Click(object sender, EventArgs e)
	{
		try
		{
			definition();
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "(*.jpg; *.jpeg; *.gif; *.PNG; *.png)| *.jpg; *.jpeg; *.gif; *.PNG;*.png; ";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				ofd = openFileDialog.FileName;
				string fileName = Path.GetFileNameWithoutExtension(ofd);
				string exten = Path.GetExtension(ofd);
				string date = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
				fileName = fileName + date + exten;
				fileSavePath = Path.Combine(saveDirectory, fileName);
				picbox1.Image = new Bitmap(openFileDialog.FileName);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnImageBrowse2_Click(object sender, EventArgs e)
	{
		try
		{
			definition();
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "(*.jpg; *.jpeg; *.gif; *.PNG; *.png)| *.jpg; *.jpeg; *.gif; *.PNG;*.png; ";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				ofd2 = openFileDialog.FileName;
				string fileName = Path.GetFileNameWithoutExtension(ofd2);
				string exten = Path.GetExtension(ofd2);
				string date = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
				fileName = fileName + date + exten;
				fileSavePath2 = Path.Combine(saveDirectory, fileName);
				picbox2.Image = new Bitmap(openFileDialog.FileName);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
	}

	private void frmFixedAssetRegister_KeyDown(object sender, KeyEventArgs e)
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
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					CmbAssetItem.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					((Control)(object)DatagridHistory).Focus();
				}
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.S && btnSave.Visible && btnSave.Enabled)
				{
					btnSave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					BtnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					txtBrandName.Focus();
				}
				if (e.Control)
				{
					_ = e.KeyCode;
					_ = 121;
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					btnImageBrowse1.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					CmbAssetItem.Focus();
				}
			}
			else
			{
				if (e.Control && e.KeyCode == Keys.S)
				{
					btnShow_Click_1(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					BtnNewHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefreshHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					FromDateHistory.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)DatagridHistory).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					FromDateHistory.Focus();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShortCutkeys_Click(object sender, EventArgs e)
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
			dt.Rows.Add("Ctrl+S", "For Save in Form Tab and For Show History in History Tab");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print 273");
			dt.Rows.Add("Alt+1", "For Print 273");
			dt.Rows.Add("Ctrl+F1", "For Open Stock Report");
			dt.Rows.Add("Ctrl+F2", "For Open Stock Report With Value");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+F12", "For SaveAs");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid when focus in form tab and for focus on history grid when in history tab");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus on Item in Detail Grid when in Form tab and For focus on FromDate in history tab");
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

	private void DatagridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = DatagridHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int CuurID = Conversion.ToInt(item.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Return)
			{
				FormReset();
				ReadById(CuurID);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (DatagridHistory.CurrentColumn != null && DatagridHistory.CurrentColumn.Key == "Edit")
				{
					FormReset();
					ReadById(CuurID);
				}
				else if ((DatagridHistory.CurrentColumn == null || !(DatagridHistory.CurrentColumn.Key == "Print")) && DatagridHistory.CurrentColumn != null)
				{
					_ = DatagridHistory.CurrentColumn.Key == "NoOfAttachments";
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAssetCategory_Click(object sender, EventArgs e)
	{
		try
		{
			frmItemCatagoryStore frmItemCatagoryStore = new frmItemCatagoryStore(UserAccount);
			frmItemCatagoryStore.FormTypeId = 3;
			frmItemCatagoryStore.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnDefineAssetDepartment_Click(object sender, EventArgs e)
	{
		try
		{
			new Define_Department(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnAssetItemDefine_Click(object sender, EventArgs e)
	{
		try
		{
			new frmDefineAssets(UserAccount).Show();
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
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Expected O, but got Unknown
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Expected O, but got Unknown
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Expected O, but got Unknown
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Expected O, but got Unknown
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Expected O, but got Unknown
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Expected O, but got Unknown
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Expected O, but got Unknown
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Expected O, but got Unknown
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Expected O, but got Unknown
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Expected O, but got Unknown
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Expected O, but got Unknown
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Expected O, but got Unknown
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Expected O, but got Unknown
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Expected O, but got Unknown
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Expected O, but got Unknown
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Expected O, but got Unknown
		//IL_0441: Unknown result type (might be due to invalid IL or missing references)
		//IL_044b: Expected O, but got Unknown
		//IL_0507: Unknown result type (might be due to invalid IL or missing references)
		//IL_0511: Expected O, but got Unknown
		//IL_058b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0595: Expected O, but got Unknown
		//IL_05cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d7: Expected O, but got Unknown
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.FixedAsset.frmFixedAssetRegister));
		GridEXLayout DatagridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance93 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel4 = new System.Windows.Forms.Panel();
		this.BrandImages = new System.Windows.Forms.GroupBox();
		this.btnImageBrowse2 = new System.Windows.Forms.Button();
		this.picbox2 = new System.Windows.Forms.PictureBox();
		this.btnImageBrowse1 = new System.Windows.Forms.Button();
		this.picbox1 = new System.Windows.Forms.PictureBox();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.CmbExpenseMaintenanceAccount = new UltraCombo();
		this.label15 = new System.Windows.Forms.Label();
		this.BtnDefineAssetDepartment = new System.Windows.Forms.Button();
		this.BtnAssetItemDefine = new System.Windows.Forms.Button();
		this.btnAssetCategory = new System.Windows.Forms.Button();
		this.label21 = new System.Windows.Forms.Label();
		this.cmbAccomolativeAcc = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.cmbDepriciationAcc = new UltraCombo();
		this.label19 = new System.Windows.Forms.Label();
		this.cmbGlAccount = new UltraCombo();
		this.label18 = new System.Windows.Forms.Label();
		this.CmbAssetItem = new UltraCombo();
		this.cmbAssetStatus = new System.Windows.Forms.ComboBox();
		this.label17 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.txtCurrentValue = new System.Windows.Forms.TextBox();
		this.txtPurchasePrice = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.txtUseableLifeMonth = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.txtExpiryDate = new System.Windows.Forms.DateTimePicker();
		this.label12 = new System.Windows.Forms.Label();
		this.txtPurchaseDate = new System.Windows.Forms.DateTimePicker();
		this.label11 = new System.Windows.Forms.Label();
		this.txtVendor = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.cmbDepartment = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.txtAssetLocation = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.txtAssetCondition = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.txtAssetSerialNo = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.txtManufacturer = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.txtModelDesc = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtMakeDesc = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtBrandName = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.txtAssetName = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.CmbAssetCategory = new UltraCombo();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label24 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.BtnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.DatagridHistory = new GridEX();
		this.panel1 = new System.Windows.Forms.Panel();
		this.label55 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.cmbCategoryHistory = new UltraCombo();
		this.label23 = new System.Windows.Forms.Label();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnShow = new System.Windows.Forms.Button();
		this.label53 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label22 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.BtnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel4.SuspendLayout();
		this.BrandImages.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picbox2).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picbox1).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbExpenseMaintenanceAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAccomolativeAcc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDepriciationAcc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGlAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAssetItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDepartment).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAssetCategory).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).BeginInit();
		this.panel1.SuspendLayout();
		this.panel3.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCategoryHistory).BeginInit();
		this.panel6.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(944, 661);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(936, 632);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.BrandImages);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(3, 55);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(930, 574);
		this.panel4.TabIndex = 0;
		this.BrandImages.Controls.Add(this.btnImageBrowse2);
		this.BrandImages.Controls.Add(this.picbox2);
		this.BrandImages.Controls.Add(this.btnImageBrowse1);
		this.BrandImages.Controls.Add(this.picbox1);
		this.BrandImages.Dock = System.Windows.Forms.DockStyle.Top;
		this.BrandImages.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold);
		this.BrandImages.Location = new System.Drawing.Point(352, 0);
		this.BrandImages.Name = "BrandImages";
		this.BrandImages.Size = new System.Drawing.Size(578, 296);
		this.BrandImages.TabIndex = 1;
		this.BrandImages.TabStop = false;
		this.BrandImages.Text = "Asset Images";
		this.btnImageBrowse2.BackColor = System.Drawing.Color.Teal;
		this.btnImageBrowse2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnImageBrowse2.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Bold);
		this.btnImageBrowse2.ForeColor = System.Drawing.Color.White;
		this.btnImageBrowse2.Location = new System.Drawing.Point(293, 261);
		this.btnImageBrowse2.Name = "btnImageBrowse2";
		this.btnImageBrowse2.Size = new System.Drawing.Size(279, 31);
		this.btnImageBrowse2.TabIndex = 1;
		this.btnImageBrowse2.Text = "Browse";
		this.btnImageBrowse2.UseVisualStyleBackColor = false;
		this.btnImageBrowse2.Click += new System.EventHandler(btnImageBrowse2_Click);
		this.picbox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.picbox2.Location = new System.Drawing.Point(293, 18);
		this.picbox2.Name = "picbox2";
		this.picbox2.Size = new System.Drawing.Size(279, 241);
		this.picbox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picbox2.TabIndex = 36;
		this.picbox2.TabStop = false;
		this.btnImageBrowse1.BackColor = System.Drawing.Color.Teal;
		this.btnImageBrowse1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnImageBrowse1.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Bold);
		this.btnImageBrowse1.ForeColor = System.Drawing.Color.White;
		this.btnImageBrowse1.Location = new System.Drawing.Point(6, 261);
		this.btnImageBrowse1.Name = "btnImageBrowse1";
		this.btnImageBrowse1.Size = new System.Drawing.Size(279, 31);
		this.btnImageBrowse1.TabIndex = 0;
		this.btnImageBrowse1.Text = "Browse";
		this.btnImageBrowse1.UseVisualStyleBackColor = false;
		this.btnImageBrowse1.Click += new System.EventHandler(btnImageBrowse1_Click);
		this.picbox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.picbox1.Location = new System.Drawing.Point(6, 18);
		this.picbox1.Name = "picbox1";
		this.picbox1.Size = new System.Drawing.Size(279, 241);
		this.picbox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picbox1.TabIndex = 35;
		this.picbox1.TabStop = false;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbExpenseMaintenanceAccount);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Controls.Add(this.BtnDefineAssetDepartment);
		this.groupBox1.Controls.Add(this.BtnAssetItemDefine);
		this.groupBox1.Controls.Add(this.btnAssetCategory);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbAccomolativeAcc);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDepriciationAcc);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbGlAccount);
		this.groupBox1.Controls.Add(this.label18);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAssetItem);
		this.groupBox1.Controls.Add(this.cmbAssetStatus);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.label16);
		this.groupBox1.Controls.Add(this.txtCurrentValue);
		this.groupBox1.Controls.Add(this.txtPurchasePrice);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.txtUseableLifeMonth);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add(this.txtExpiryDate);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.txtPurchaseDate);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.txtVendor);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDepartment);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.txtAssetLocation);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.txtAssetCondition);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.txtAssetSerialNo);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.txtManufacturer);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.txtModelDesc);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.txtMakeDesc);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.txtBrandName);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.txtAssetName);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAssetCategory);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(352, 574);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.CmbExpenseMaintenanceAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbExpenseMaintenanceAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbExpenseMaintenanceAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbExpenseMaintenanceAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance12;
		((SpecialBoxBase)((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance17).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance20).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbExpenseMaintenanceAccount).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbExpenseMaintenanceAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbExpenseMaintenanceAccount).Location = new System.Drawing.Point(109, 526);
		((System.Windows.Forms.Control)(object)this.CmbExpenseMaintenanceAccount).Name = "CmbExpenseMaintenanceAccount";
		((System.Windows.Forms.Control)(object)this.CmbExpenseMaintenanceAccount).Size = new System.Drawing.Size(237, 26);
		((System.Windows.Forms.Control)(object)this.CmbExpenseMaintenanceAccount).TabIndex = 21;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label15.Location = new System.Drawing.Point(5, 524);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(98, 30);
		this.label15.TabIndex = 608;
		this.label15.Text = "Expense\r\nMaintenance A/c";
		this.BtnDefineAssetDepartment.BackColor = System.Drawing.Color.White;
		this.BtnDefineAssetDepartment.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnDefineAssetDepartment.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.BtnDefineAssetDepartment.Location = new System.Drawing.Point(320, 193);
		this.BtnDefineAssetDepartment.Margin = new System.Windows.Forms.Padding(2);
		this.BtnDefineAssetDepartment.Name = "BtnDefineAssetDepartment";
		this.BtnDefineAssetDepartment.Size = new System.Drawing.Size(26, 27);
		this.BtnDefineAssetDepartment.TabIndex = 10;
		this.BtnDefineAssetDepartment.TabStop = false;
		this.BtnDefineAssetDepartment.UseVisualStyleBackColor = false;
		this.BtnDefineAssetDepartment.Click += new System.EventHandler(BtnDefineAssetDepartment_Click);
		this.BtnAssetItemDefine.BackColor = System.Drawing.Color.White;
		this.BtnAssetItemDefine.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnAssetItemDefine.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.BtnAssetItemDefine.Location = new System.Drawing.Point(320, 40);
		this.BtnAssetItemDefine.Margin = new System.Windows.Forms.Padding(2);
		this.BtnAssetItemDefine.Name = "BtnAssetItemDefine";
		this.BtnAssetItemDefine.Size = new System.Drawing.Size(26, 27);
		this.BtnAssetItemDefine.TabIndex = 10;
		this.BtnAssetItemDefine.TabStop = false;
		this.BtnAssetItemDefine.UseVisualStyleBackColor = false;
		this.BtnAssetItemDefine.Click += new System.EventHandler(BtnAssetItemDefine_Click);
		this.btnAssetCategory.BackColor = System.Drawing.Color.White;
		this.btnAssetCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAssetCategory.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnAssetCategory.Location = new System.Drawing.Point(320, 13);
		this.btnAssetCategory.Margin = new System.Windows.Forms.Padding(2);
		this.btnAssetCategory.Name = "btnAssetCategory";
		this.btnAssetCategory.Size = new System.Drawing.Size(26, 27);
		this.btnAssetCategory.TabIndex = 10;
		this.btnAssetCategory.TabStop = false;
		this.btnAssetCategory.UseVisualStyleBackColor = false;
		this.btnAssetCategory.Click += new System.EventHandler(btnAssetCategory_Click);
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(5, 478);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(102, 15);
		this.label21.TabIndex = 119;
		this.label21.Text = "Accomolative A/c";
		this.cmbAccomolativeAcc.AllowNull = (DefaultableBoolean)1;
		this.cmbAccomolativeAcc.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbAccomolativeAcc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbAccomolativeAcc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbAccomolativeAcc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbAccomolativeAcc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbAccomolativeAcc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccomolativeAcc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccomolativeAcc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance37;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccomolativeAcc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccomolativeAcc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbAccomolativeAcc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbAccomolativeAcc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbAccomolativeAcc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbAccomolativeAcc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbAccomolativeAcc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbAccomolativeAcc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccomolativeAcc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance43).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbAccomolativeAcc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbAccomolativeAcc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbAccomolativeAcc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccomolativeAcc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbAccomolativeAcc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbAccomolativeAcc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbAccomolativeAcc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbAccomolativeAcc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbAccomolativeAcc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbAccomolativeAcc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbAccomolativeAcc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbAccomolativeAcc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbAccomolativeAcc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbAccomolativeAcc).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbAccomolativeAcc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbAccomolativeAcc).Location = new System.Drawing.Point(109, 472);
		((System.Windows.Forms.Control)(object)this.cmbAccomolativeAcc).Name = "cmbAccomolativeAcc";
		((System.Windows.Forms.Control)(object)this.cmbAccomolativeAcc).Size = new System.Drawing.Size(237, 26);
		((System.Windows.Forms.Control)(object)this.cmbAccomolativeAcc).TabIndex = 19;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(5, 505);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(94, 15);
		this.label20.TabIndex = 117;
		this.label20.Text = "Depriciation A/c";
		this.cmbDepriciationAcc.AllowNull = (DefaultableBoolean)1;
		this.cmbDepriciationAcc.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDepriciationAcc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDepriciationAcc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance77;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance82).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance83).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance85).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDepriciationAcc).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDepriciationAcc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDepriciationAcc).Location = new System.Drawing.Point(109, 499);
		((System.Windows.Forms.Control)(object)this.cmbDepriciationAcc).Name = "cmbDepriciationAcc";
		((System.Windows.Forms.Control)(object)this.cmbDepriciationAcc).Size = new System.Drawing.Size(237, 26);
		((System.Windows.Forms.Control)(object)this.cmbDepriciationAcc).TabIndex = 20;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(5, 451);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(43, 15);
		this.label19.TabIndex = 115;
		this.label19.Text = "GL A/c";
		this.cmbGlAccount.AllowNull = (DefaultableBoolean)1;
		this.cmbGlAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbGlAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbGlAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance63).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance63).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbGlAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance64;
		((SpecialBoxBase)((UltraGridBase)this.cmbGlAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance69).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance70).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbGlAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbGlAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbGlAccount).Location = new System.Drawing.Point(109, 445);
		((System.Windows.Forms.Control)(object)this.cmbGlAccount).Name = "cmbGlAccount";
		((System.Windows.Forms.Control)(object)this.cmbGlAccount).Size = new System.Drawing.Size(237, 26);
		((System.Windows.Forms.Control)(object)this.cmbGlAccount).TabIndex = 18;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(5, 46);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(63, 15);
		this.label18.TabIndex = 113;
		this.label18.Text = "Asset Item";
		this.CmbAssetItem.AllowNull = (DefaultableBoolean)1;
		this.CmbAssetItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAssetItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAssetItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAssetItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance50;
		((SpecialBoxBase)((UltraGridBase)this.CmbAssetItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance56).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAssetItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAssetItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAssetItem).Location = new System.Drawing.Point(109, 40);
		((System.Windows.Forms.Control)(object)this.CmbAssetItem).Name = "CmbAssetItem";
		((System.Windows.Forms.Control)(object)this.CmbAssetItem).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.CmbAssetItem).TabIndex = 1;
		this.cmbAssetStatus.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbAssetStatus.FormattingEnabled = true;
		this.cmbAssetStatus.Location = new System.Drawing.Point(109, 420);
		this.cmbAssetStatus.Name = "cmbAssetStatus";
		this.cmbAssetStatus.Size = new System.Drawing.Size(237, 24);
		this.cmbAssetStatus.TabIndex = 17;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label17.Location = new System.Drawing.Point(5, 425);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(71, 15);
		this.label17.TabIndex = 110;
		this.label17.Text = "Asset Status";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(5, 374);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(78, 15);
		this.label16.TabIndex = 108;
		this.label16.Text = "Current Value";
		this.txtCurrentValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCurrentValue.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCurrentValue.Location = new System.Drawing.Point(109, 370);
		this.txtCurrentValue.Name = "txtCurrentValue";
		this.txtCurrentValue.Size = new System.Drawing.Size(237, 23);
		this.txtCurrentValue.TabIndex = 15;
		this.txtPurchasePrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPurchasePrice.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPurchasePrice.Location = new System.Drawing.Point(232, 321);
		this.txtPurchasePrice.Name = "txtPurchasePrice";
		this.txtPurchasePrice.Size = new System.Drawing.Size(114, 23);
		this.txtPurchasePrice.TabIndex = 13;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(5, 399);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(103, 15);
		this.label14.TabIndex = 104;
		this.label14.Text = "UseableLifeMonth";
		this.txtUseableLifeMonth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtUseableLifeMonth.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtUseableLifeMonth.Location = new System.Drawing.Point(109, 395);
		this.txtUseableLifeMonth.Name = "txtUseableLifeMonth";
		this.txtUseableLifeMonth.Size = new System.Drawing.Size(237, 23);
		this.txtUseableLifeMonth.TabIndex = 16;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(5, 349);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(70, 15);
		this.label13.TabIndex = 102;
		this.label13.Text = "Expiry Date ";
		this.txtExpiryDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExpiryDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtExpiryDate.Location = new System.Drawing.Point(109, 345);
		this.txtExpiryDate.Name = "txtExpiryDate";
		this.txtExpiryDate.Size = new System.Drawing.Size(237, 23);
		this.txtExpiryDate.TabIndex = 14;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(5, 317);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(98, 30);
		this.label12.TabIndex = 100;
		this.label12.Text = "Purchase Date && \r\nPrice";
		this.txtPurchaseDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPurchaseDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtPurchaseDate.Location = new System.Drawing.Point(109, 321);
		this.txtPurchaseDate.Name = "txtPurchaseDate";
		this.txtPurchaseDate.Size = new System.Drawing.Size(122, 23);
		this.txtPurchaseDate.TabIndex = 12;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(5, 300);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(45, 15);
		this.label11.TabIndex = 98;
		this.label11.Text = "Vendor";
		this.txtVendor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVendor.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtVendor.Location = new System.Drawing.Point(109, 296);
		this.txtVendor.Name = "txtVendor";
		this.txtVendor.Size = new System.Drawing.Size(237, 23);
		this.txtVendor.TabIndex = 11;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(5, 199);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(96, 15);
		this.label10.TabIndex = 96;
		this.label10.Text = "Asset Deprtment";
		this.cmbDepartment.AllowNull = (DefaultableBoolean)1;
		this.cmbDepartment.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDepartment.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDepartment.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance40;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance51).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance51).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance91).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance92).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDepartment.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Location = new System.Drawing.Point(109, 193);
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Name = "cmbDepartment";
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.cmbDepartment).TabIndex = 7;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(5, 275);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(93, 15);
		this.label9.TabIndex = 94;
		this.label9.Text = "Asset Loacation ";
		this.txtAssetLocation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAssetLocation.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAssetLocation.Location = new System.Drawing.Point(109, 271);
		this.txtAssetLocation.Name = "txtAssetLocation";
		this.txtAssetLocation.Size = new System.Drawing.Size(237, 23);
		this.txtAssetLocation.TabIndex = 10;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(5, 147);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(95, 15);
		this.label8.TabIndex = 92;
		this.label8.Text = "Assets Condition";
		this.txtAssetCondition.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAssetCondition.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAssetCondition.Location = new System.Drawing.Point(109, 143);
		this.txtAssetCondition.Name = "txtAssetCondition";
		this.txtAssetCondition.Size = new System.Drawing.Size(237, 23);
		this.txtAssetCondition.TabIndex = 5;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(5, 172);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(86, 15);
		this.label7.TabIndex = 90;
		this.label7.Text = "Asset Serial No";
		this.txtAssetSerialNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAssetSerialNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAssetSerialNo.Location = new System.Drawing.Point(109, 168);
		this.txtAssetSerialNo.Name = "txtAssetSerialNo";
		this.txtAssetSerialNo.Size = new System.Drawing.Size(237, 23);
		this.txtAssetSerialNo.TabIndex = 6;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(5, 225);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(79, 15);
		this.label6.TabIndex = 88;
		this.label6.Text = "Manufacturer";
		this.txtManufacturer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtManufacturer.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtManufacturer.Location = new System.Drawing.Point(109, 221);
		this.txtManufacturer.Name = "txtManufacturer";
		this.txtManufacturer.Size = new System.Drawing.Size(237, 23);
		this.txtManufacturer.TabIndex = 8;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(5, 122);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(70, 15);
		this.label4.TabIndex = 86;
		this.label4.Text = "Model Desc";
		this.txtModelDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtModelDesc.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtModelDesc.Location = new System.Drawing.Point(109, 118);
		this.txtModelDesc.Name = "txtModelDesc";
		this.txtModelDesc.Size = new System.Drawing.Size(237, 23);
		this.txtModelDesc.TabIndex = 4;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(5, 250);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(65, 15);
		this.label3.TabIndex = 84;
		this.label3.Text = "Make Desc";
		this.txtMakeDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMakeDesc.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMakeDesc.Location = new System.Drawing.Point(109, 246);
		this.txtMakeDesc.Name = "txtMakeDesc";
		this.txtMakeDesc.Size = new System.Drawing.Size(237, 23);
		this.txtMakeDesc.TabIndex = 9;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(5, 72);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(38, 15);
		this.label2.TabIndex = 82;
		this.label2.Text = "Brand";
		this.txtBrandName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBrandName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBrandName.Location = new System.Drawing.Point(109, 68);
		this.txtBrandName.Name = "txtBrandName";
		this.txtBrandName.Size = new System.Drawing.Size(237, 23);
		this.txtBrandName.TabIndex = 2;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(5, 97);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(70, 15);
		this.label1.TabIndex = 80;
		this.label1.Text = "Asset Name";
		this.txtAssetName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAssetName.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAssetName.Location = new System.Drawing.Point(109, 93);
		this.txtAssetName.Name = "txtAssetName";
		this.txtAssetName.Size = new System.Drawing.Size(237, 23);
		this.txtAssetName.TabIndex = 3;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(5, 18);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(85, 15);
		this.label5.TabIndex = 78;
		this.label5.Text = "Asset Category";
		this.CmbAssetCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbAssetCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAssetCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAssetCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAssetCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance89;
		((SpecialBoxBase)((UltraGridBase)this.CmbAssetCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance90).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance90).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance30).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAssetCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAssetCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAssetCategory).Location = new System.Drawing.Point(109, 12);
		((System.Windows.Forms.Control)(object)this.CmbAssetCategory).Name = "CmbAssetCategory";
		((System.Windows.Forms.Control)(object)this.CmbAssetCategory).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.CmbAssetCategory).TabIndex = 0;
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label24);
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(930, 52);
		this.panel2.TabIndex = 0;
		this.label24.AutoSize = true;
		this.label24.BackColor = System.Drawing.Color.Teal;
		this.label24.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label24.Location = new System.Drawing.Point(1, 29);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(185, 19);
		this.label24.TabIndex = 5;
		this.label24.Text = "Fixed Assets Register";
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.btnNew, this.BtnRefresh, this.btnSave, this.btnUpdate, this.toolStripSeparator2, this.btnPrint, this.BtnShortCutkeys });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(930, 27);
		this.toolStrip3.TabIndex = 60;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.BtnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefresh.Name = "BtnRefresh";
		this.BtnRefresh.Size = new System.Drawing.Size(80, 24);
		this.BtnRefresh.Text = "Refresh";
		this.BtnRefresh.Click += new System.EventHandler(BtnRefresh_Click);
		this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Image = (System.Drawing.Image)resources.GetObject("btnSave.Image");
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(61, 24);
		this.btnPrint.Text = "&Print";
		this.btnPrint.Visible = false;
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DatagridHistory);
		this.tabPage2.Controls.Add(this.panel1);
		this.tabPage2.Controls.Add(this.panel3);
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(936, 632);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.DatagridHistory.AllowEdit = (InheritableBoolean)2;
		this.DatagridHistory.ColumnAutoResize = true;
		DatagridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DatagridHistory.DesignTimeLayout = DatagridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DatagridHistory.DynamicFiltering = true;
		this.DatagridHistory.FilterMode = (FilterMode)2;
		this.DatagridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DatagridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Location = new System.Drawing.Point(3, 133);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Name = "DatagridHistory";
		((ScrollableBase)this.DatagridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Size = new System.Drawing.Size(930, 496);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).TabIndex = 0;
		this.DatagridHistory.TotalRow = (InheritableBoolean)1;
		this.DatagridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DatagridHistory).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).DoubleClick += new System.EventHandler(DatagridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DatagridHistory_KeyDown);
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.label55);
		this.panel1.Controls.Add(this.ctrlGrdBar1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 107);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(930, 26);
		this.panel1.TabIndex = 47;
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label55.ForeColor = System.Drawing.Color.White;
		this.label55.Location = new System.Drawing.Point(3, 3);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(132, 18);
		this.label55.TabIndex = 62;
		this.label55.Text = "Filtered Records";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(882, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.DatagridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 23);
		this.ctrlGrdBar1.TabIndex = 61;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel3.Controls.Add(this.groupBox2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 60);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(930, 47);
		this.panel3.TabIndex = 0;
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCategoryHistory);
		this.groupBox2.Controls.Add(this.label23);
		this.groupBox2.Controls.Add(this.rdmodifydate);
		this.groupBox2.Controls.Add(this.rdentrydate);
		this.groupBox2.Controls.Add(this.btnShow);
		this.groupBox2.Controls.Add(this.label53);
		this.groupBox2.Controls.Add(this.label54);
		this.groupBox2.Controls.Add(this.ToDateHistory);
		this.groupBox2.Controls.Add(this.FromDateHistory);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(3, 3);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(894, 43);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.cmbCategoryHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbCategoryHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCategoryHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCategoryHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance24;
		((SpecialBoxBase)((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance3).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCategoryHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbCategoryHistory).Location = new System.Drawing.Point(470, 12);
		((System.Windows.Forms.Control)(object)this.cmbCategoryHistory).Name = "cmbCategoryHistory";
		((System.Windows.Forms.Control)(object)this.cmbCategoryHistory).Size = new System.Drawing.Size(188, 26);
		((System.Windows.Forms.Control)(object)this.cmbCategoryHistory).TabIndex = 2;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(385, 18);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(85, 15);
		this.label23.TabIndex = 0;
		this.label23.Text = "Asset Category";
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(805, 16);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 5;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Checked = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(722, 16);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 4;
		this.rdentrydate.TabStop = true;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(661, 13);
		this.btnShow.Margin = new System.Windows.Forms.Padding(2);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(58, 25);
		this.btnShow.TabIndex = 3;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click_1);
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(201, 18);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(48, 15);
		this.label53.TabIndex = 5;
		this.label53.Text = "To Date";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(3, 18);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(63, 15);
		this.label54.TabIndex = 3;
		this.label54.Text = "From Date";
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(249, 14);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(133, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(67, 14);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(133, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label22);
		this.panel6.Controls.Add(this.toolStrip1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(930, 57);
		this.panel6.TabIndex = 0;
		this.label22.AutoSize = true;
		this.label22.BackColor = System.Drawing.Color.Teal;
		this.label22.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.White;
		this.label22.Location = new System.Drawing.Point(5, 34);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(67, 19);
		this.label22.TabIndex = 5;
		this.label22.Text = "History";
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.BtnNewHistory, this.btnRefreshHistory });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(930, 27);
		this.toolStrip1.TabIndex = 60;
		this.toolStrip1.Text = "toolStrip1";
		this.BtnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewHistory.Name = "BtnNewHistory";
		this.BtnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.BtnNewHistory.Text = "&New";
		this.BtnNewHistory.Click += new System.EventHandler(BtnNewHistory_Click);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(944, 661);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "frmFixedAssetRegister";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmFixedAssetRegister_FormClosing);
		base.Load += new System.EventHandler(frmFixedAssetRegister_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmFixedAssetRegister_KeyDown);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.BrandImages.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.picbox2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picbox1).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbExpenseMaintenanceAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAccomolativeAcc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDepriciationAcc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGlAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAssetItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDepartment).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAssetCategory).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCategoryHistory).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
