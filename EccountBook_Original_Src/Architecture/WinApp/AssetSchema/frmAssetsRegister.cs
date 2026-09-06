using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.AssetSchema;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.AssetSchema;
using Architecture.Model.Authentication;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.FixedAsset;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Lookups;
using Architecture.WinApp.Properties;
using Architecture.WinApp.StoreManagement;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.AssetSchema;

public class frmAssetsRegister : Form
{
	private Attachment AT = new Attachment();

	private Rightsobjects formrights = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private long RECID;

	private bool UpdateMode;

	private bool flag;

	private string saveDirectory = "";

	private string fileSavePath = "";

	private string ofd = "";

	private string fileSavePath2 = "";

	private string ofd2 = "";

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel4;

	private GroupBox groupBox1;

	private Button btnImageBrowse2;

	private Button btnImageBrowse1;

	private Label label21;

	internal UltraCombo CmbAccomolativeAcc;

	private Label label20;

	internal UltraCombo cmbDepriciationAcc;

	private Label label19;

	internal UltraCombo CmbGlAccount;

	private Label label18;

	internal UltraCombo CmbAssetItem;

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

	internal UltraCombo CmbDepartment;

	private Label label9;

	private TextBox txtAssetLocation;

	private Label label8;

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

	private Label label26;

	private Label label25;

	internal UltraCombo CmbAssetCondition;

	private Label label33;

	private Label label32;

	private Label label31;

	private Label label37;

	private Label label30;

	private Label label35;

	private Label label29;

	private Label label34;

	private TextBox txtDepreciationRate;

	private Label label36;

	internal UltraCombo CmbDepreciationMethod;

	private Label label28;

	private Label label27;

	private Label label42;

	private Label label41;

	private Label label40;

	private Label label39;

	private Label label38;

	internal UltraCombo CmbAssetStatus;

	public frmAssetsRegister(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmAssetsRegister_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1100, 580);
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnSave.Enabled = formrights.DoHaveSaveRight;
			btnUpdate.Enabled = formrights.DoHaveUpdateRights;
			ctrlGrdBar1.mGridChooseFielder.Visible = false;
			ctrlGrdBar1.mGridSaveLayouts.Visible = false;
			ctrlGrdBar1.removeLayoutToolStripMenuItem.Visible = false;
			ItemCategory();
			AssetAccountBind();
			ExpenseAccountFill();
			AssetStatusBind();
			DepartmentBind();
			AssetConditionBind();
			DepreciatonMethodNameBind();
			CmbAssetCategory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemCategory()
	{
		try
		{
			DataTable dt = Category.GetCategoryForComboBind(UserAccount.OrganizationId, UserAccount.CompanyId);
			InfragisticsHelper.BindAndRetainSelection(CmbAssetCategory, dt, "Id", "categoryDescription", "Category Name", AllColumns: true, Conversion.ToInt(CmbAssetCategory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(cmbCategoryHistory, dt, "Id", "categoryDescription", "Category Name", AllColumns: false, Conversion.ToInt(cmbCategoryHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dt != null && dt.Rows.Count > 0)
			{
				((UltraGridBase)CmbAssetCategory).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbAssetCategory).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				((UltraGridBase)CmbAssetCategory).DisplayLayout.Bands[0].Columns[5].Hidden = true;
				((UltraGridBase)CmbAssetCategory).DisplayLayout.Bands[0].Columns[6].Hidden = true;
				((UltraGridBase)CmbAssetCategory).DisplayLayout.Bands[0].Columns[7].Hidden = true;
				((UltraGridBase)CmbAssetCategory).DisplayLayout.Bands[0].Columns[8].Hidden = true;
				((UltraGridBase)CmbAssetCategory).DisplayLayout.Bands[0].Columns[9].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AssetItemFill(int AssetCategoryId)
	{
		try
		{
			DataTable dt = Category.GetAssetItemForComboBind(UserAccount.OrganizationId, UserAccount.CompanyId, AssetCategoryId);
			InfragisticsHelper.BindAndRetainSelection(CmbAssetItem, dt, "Id", "ItemName", "Item Name", AllColumns: true, Conversion.ToInt(CmbAssetItem.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dt != null && dt.Rows.Count > 0)
			{
				((UltraGridBase)CmbAssetItem).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbAssetItem).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				((UltraGridBase)CmbAssetItem).DisplayLayout.Bands[0].Columns[5].Hidden = true;
				((UltraGridBase)CmbAssetItem).DisplayLayout.Bands[0].Columns[6].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AssetAccountBind()
	{
		try
		{
			DataTable dt = DatatableHelper.GetAccountsFromGlobalByTypeIds(new int[1] { 1 });
			InfragisticsHelper.BindAndRetainSelection(CmbAccomolativeAcc, dt, "Id", "AccountTitle", "Accumulative Account", AllColumns: true, Conversion.ToInt(CmbAccomolativeAcc.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbGlAccount, dt, "Id", "AccountTitle", "Assets GL Account", AllColumns: true, Conversion.ToInt(CmbGlAccount.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)CmbAccomolativeAcc).DisplayLayout.Bands[0].Columns[1].Width = 350;
				((UltraGridBase)CmbGlAccount).DisplayLayout.Bands[0].Columns[1].Width = 350;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExpenseAccountFill()
	{
		try
		{
			DataTable dt = DatatableHelper.GetAccountsFromGlobalByTypeIds(new int[6] { 11, 12, 14, 20, 21, 23 });
			InfragisticsHelper.BindAndRetainSelection(CmbExpenseMaintenanceAccount, dt, "Id", "AccountTitle", "Expense Maintenance Account", AllColumns: true, Conversion.ToInt(CmbExpenseMaintenanceAccount.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(cmbDepriciationAcc, dt, "Id", "AccountTitle", "Depreciation Account", AllColumns: true, Conversion.ToInt(cmbDepriciationAcc.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)CmbExpenseMaintenanceAccount).DisplayLayout.Bands[0].Columns[1].Width = 350;
				((UltraGridBase)cmbDepriciationAcc).DisplayLayout.Bands[0].Columns[1].Width = 350;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DepartmentBind()
	{
		try
		{
			DataTable dt = AssetRegister.GetAssetDepartment(UserAccount.OrganizationId, UserAccount.CompanyId);
			InfragisticsHelper.BindAndRetainSelection(CmbDepartment, dt, "Id", "DepartmentName", "Department", AllColumns: true, Conversion.ToInt(CmbDepartment.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AssetStatusBind()
	{
		try
		{
			DataTable dt = AssetRegister.GetAssetStatus();
			InfragisticsHelper.BindAndRetainSelection(CmbAssetStatus, dt, "Id", "StatusName", "Status", AllColumns: true, Conversion.ToInt(CmbAssetStatus.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AssetConditionBind()
	{
		try
		{
			DataTable dt = AssetRegister.GetAssetCondition();
			InfragisticsHelper.BindAndRetainSelection(CmbAssetCondition, dt, "Id", "ConditionName", "Condition Name", AllColumns: true, Conversion.ToInt(CmbAssetCondition.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DepreciatonMethodNameBind()
	{
		try
		{
			DataTable dt = DepreciationMethodSchedule.GetActiveDepreciationMethodSchedule(UserAccount.OrganizationId, UserAccount.CompanyId);
			InfragisticsHelper.BindAndRetainSelection(CmbDepreciationMethod, dt, "depreciationMethodScheduleId", "DepreciatonMethodName", "Depreciaton Method", AllColumns: true, Conversion.ToInt(CmbDepreciationMethod.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbAssetCategory_Leave(object sender, EventArgs e)
	{
		try
		{
			int categoryId = Conversion.ToInt(CmbAssetCategory.Value);
			int itemId = Conversion.ToInt(CmbAssetItem.Value);
			if (categoryId <= 0)
			{
				ClearDepreciationAndAccounts();
				return;
			}
			UltraGridRow selectedRow2 = ((UltraDropDownBase)CmbAssetCategory).SelectedRow;
			CellsCollection selectedRow = ((selectedRow2 != null) ? selectedRow2.Cells : null);
			if (selectedRow == null)
			{
				ClearDepreciationAndAccounts();
				return;
			}
			if (btnSave.Visible && btnSave.Enabled)
			{
				DataTable lst = AssetRegister.GenerateSerailNoByCategoryId(UserAccount.OrganizationId, UserAccount.CompanyId, categoryId);
				if (lst.Rows.Count > 0)
				{
					txtAssetSerialNo.Text = lst.Rows[0]["SerialNo"].ToString();
				}
			}
			AssetItemFill(categoryId);
			CmbDepreciationMethod.Value = Conversion.ToInt(selectedRow["depreciationMethodScheduleId"].Value);
			txtDepreciationRate.Text = Conversion.ToString(selectedRow["depriciationrate"].Value);
			txtUseableLifeMonth.Text = Conversion.ToString(selectedRow["usefullLifeInMonths"].Value);
			if (itemId == 0)
			{
				CmbGlAccount.Value = Conversion.ToInt(selectedRow["GLAccountId"].Value);
				CmbAccomolativeAcc.Value = Conversion.ToInt(selectedRow["accumulatedAccountId"].Value);
				cmbDepriciationAcc.Value = Conversion.ToInt(selectedRow["depriciationAccountId"].Value);
				CmbExpenseMaintenanceAccount.Value = Conversion.ToInt(selectedRow["expenseAccountId"].Value);
			}
			else
			{
				ClearAccountFields();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error loading asset category details: " + ex.Message);
		}
	}

	private void CmbAssetItem_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbAssetItem.Value) <= 0)
			{
				ClearAccountFields();
				return;
			}
			UltraGridRow selectedRow2 = ((UltraDropDownBase)CmbAssetItem).SelectedRow;
			CellsCollection selectedRow = ((selectedRow2 != null) ? selectedRow2.Cells : null);
			if (selectedRow == null)
			{
				ClearAccountFields();
				return;
			}
			CmbGlAccount.Value = Conversion.ToInt(selectedRow["GLAccountId"].Value);
			CmbAccomolativeAcc.Value = Conversion.ToInt(selectedRow["accumulatedDepreciationAcId"].Value);
			cmbDepriciationAcc.Value = Conversion.ToInt(selectedRow["DepreciationExpenseAcId"].Value);
			CmbExpenseMaintenanceAccount.Value = Conversion.ToInt(selectedRow["ExpenseMaintenanceAccountId"].Value);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error loading asset item details: " + ex.Message);
		}
	}

	private void ClearDepreciationAndAccounts()
	{
		CmbDepreciationMethod.Value = "";
		txtDepreciationRate.Text = "";
		txtUseableLifeMonth.Text = "";
		ClearAccountFields();
	}

	private void ClearAccountFields()
	{
		if (Conversion.ToInt(CmbAssetCategory.Value) == 0)
		{
			CmbGlAccount.Value = "";
			CmbAccomolativeAcc.Value = "";
			cmbDepriciationAcc.Value = "";
			CmbExpenseMaintenanceAccount.Value = "";
		}
	}

	private bool FormValidation()
	{
		return FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
		{
			((Control)(object)CmbAssetCategory, "Asset Category", FormHelper.ValidType.INT),
			(txtAssetName, "Asset Name", FormHelper.ValidType.String),
			((Control)(object)CmbAssetCondition, "Asset Condition", FormHelper.ValidType.INT),
			(txtAssetSerialNo, "Asset Serial No", FormHelper.ValidType.String),
			((Control)(object)CmbDepartment, "Asset Department", FormHelper.ValidType.INT),
			(txtPurchasePrice, "Purchase Price", FormHelper.ValidType.Double),
			(txtUseableLifeMonth, "Useable Life Month", FormHelper.ValidType.INT),
			(txtCurrentValue, "Current Value", FormHelper.ValidType.Double),
			((Control)(object)CmbDepreciationMethod, "Depreciation Method", FormHelper.ValidType.INT),
			(txtDepreciationRate, "Depreciation Rate", FormHelper.ValidType.Double),
			((Control)(object)CmbAssetStatus, "Asset Status", FormHelper.ValidType.String),
			((Control)(object)CmbGlAccount, "Asset GL A/c", FormHelper.ValidType.INT),
			((Control)(object)cmbDepriciationAcc, "Depreciation_Expense A/c", FormHelper.ValidType.INT),
			((Control)(object)CmbAccomolativeAcc, "Accumulated_Depreciation A/c", FormHelper.ValidType.INT),
			((Control)(object)CmbExpenseMaintenanceAccount, "Expense Maintenance A/c", FormHelper.ValidType.INT)
		});
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
			AssetRegister fixedAssets = new AssetRegister();
			if (RECID > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				fixedAssets.assetRegisterId = RECID;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			fixedAssets.companyId = UserAccount.CompanyId;
			fixedAssets.organizationId = UserAccount.OrganizationId;
			fixedAssets.branchesId = UserAccount.BranchesId;
			fixedAssets.projectsId = UserAccount.BranchesId;
			fixedAssets.entryUserId = UserAccount.ID;
			fixedAssets.entryDate = DateTime.Now;
			fixedAssets.LastModifyById = UserAccount.ID;
			fixedAssets.LastModifyDate = DateTime.Now;
			fixedAssets.approvedDate = DateTime.Now;
			fixedAssets.approvedUserId = UserAccount.ID;
			fixedAssets.isApproved = false;
			fixedAssets.categoryId = Conversion.ToInt(CmbAssetCategory.Value);
			fixedAssets.itemId = Conversion.ToInt(CmbAssetItem.Value);
			fixedAssets.brandName = Conversion.ToString((object)txtBrandName.Text);
			fixedAssets.assetName = Conversion.ToString((object)txtAssetName.Text);
			fixedAssets.conditionId = Conversion.ToInt(CmbAssetCondition.Value);
			fixedAssets.modelDesc = Conversion.ToString((object)txtModelDesc.Text);
			fixedAssets.assetCode = Conversion.ToString((object)txtAssetSerialNo.Text);
			fixedAssets.serialNo = Conversion.ToString((object)txtAssetSerialNo.Text);
			fixedAssets.departmentId = Conversion.ToInt(CmbDepartment.Value);
			fixedAssets.manufacturerName = Conversion.ToString((object)txtManufacturer.Text);
			fixedAssets.makeDesc = Conversion.ToString((object)txtMakeDesc.Text);
			fixedAssets.locationBranch = Conversion.ToString((object)txtAssetLocation.Text);
			fixedAssets.vendorName = Conversion.ToString((object)txtVendor.Text);
			fixedAssets.purchaseDate = Conversion.ToDateTime((object)txtPurchaseDate.Value);
			fixedAssets.purchasePrice = Conversion.ToDouble((object)txtPurchasePrice.Text);
			fixedAssets.useableLifeMonth = Conversion.ToInt((object)txtUseableLifeMonth.Text);
			fixedAssets.expiryDate = Conversion.ToDateTime((object)txtExpiryDate.Value);
			fixedAssets.currentValue = Conversion.ToDouble((object)txtCurrentValue.Text);
			fixedAssets.assetStatus = Conversion.ToString((object)((Control)(object)CmbAssetStatus).Text);
			fixedAssets.assetsTypeId = "";
			fixedAssets.depreciationMethodScheduleId = Conversion.ToInt(CmbDepreciationMethod.Value);
			fixedAssets.depriciationrate = Conversion.ToDecimal((object)txtDepreciationRate.Text);
			fixedAssets.assetStatus = Conversion.ToString((object)((Control)(object)CmbAssetStatus).Text);
			fixedAssets.glAccountId = Conversion.ToInt(CmbGlAccount.Value);
			fixedAssets.capitalWIPAccountId = Conversion.ToInt(CmbGlAccount.Value);
			fixedAssets.depriciationExpenseAcId = Conversion.ToInt(cmbDepriciationAcc.Value);
			fixedAssets.accumulateddepriciationAcId = Conversion.ToInt(CmbAccomolativeAcc.Value);
			fixedAssets.ExpenseMaintenanceAccountId = Conversion.ToInt(CmbExpenseMaintenanceAccount.Value);
			fixedAssets.pic1Path = Conversion.ToString((object)fileSavePath);
			fixedAssets.pic2Path = Conversion.ToString((object)fileSavePath2);
			AssetRegister.Save(fixedAssets);
			if (RECID > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + fixedAssets.assetName);
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + fixedAssets.assetName);
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
			RECID = 0L;
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
			if (RECID == 0L)
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

	private void ReadById(long Id)
	{
		try
		{
			RECID = Id;
			AssetRegister SC = AssetRegister.GetById(RECID);
			if (SC == null)
			{
				return;
			}
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			UpdateMode = true;
			tabControl1.SelectedIndex = 0;
			CmbAssetCategory.Value = SC.categoryId;
			if (SC.itemId > 0)
			{
				CmbAssetItem.Value = SC.itemId;
			}
			txtBrandName.Text = SC.brandName;
			txtAssetName.Text = SC.assetName;
			txtModelDesc.Text = SC.modelDesc;
			CmbAssetCondition.Value = SC.conditionId;
			txtAssetSerialNo.Text = SC.serialNo;
			CmbDepartment.Value = SC.departmentId;
			txtManufacturer.Text = SC.manufacturerName;
			txtMakeDesc.Text = SC.makeDesc;
			txtAssetLocation.Text = SC.locationBranch;
			txtVendor.Text = SC.vendorName;
			txtPurchaseDate.Value = SC.purchaseDate;
			txtPurchasePrice.Text = SC.purchasePrice.ToString();
			txtUseableLifeMonth.Text = SC.useableLifeMonth.ToString();
			txtExpiryDate.Value = SC.expiryDate;
			txtCurrentValue.Text = SC.currentValue.ToString();
			CmbDepreciationMethod.Value = SC.depreciationMethodScheduleId;
			txtDepreciationRate.Text = SC.depriciationrate.ToString();
			((Control)(object)CmbAssetStatus).Text = SC.assetStatus;
			if (SC.accumulateddepriciationAcId > 0)
			{
				CmbAccomolativeAcc.Value = SC.accumulateddepriciationAcId;
			}
			if (SC.depriciationExpenseAcId > 0)
			{
				cmbDepriciationAcc.Value = SC.depriciationExpenseAcId;
			}
			if (SC.glAccountId > 0)
			{
				CmbGlAccount.Value = SC.glAccountId;
			}
			if (SC.ExpenseMaintenanceAccountId > 0)
			{
				CmbExpenseMaintenanceAccount.Value = SC.ExpenseMaintenanceAccountId;
			}
			fileSavePath = SC.pic1Path;
			if (fileSavePath != "")
			{
				if (File.Exists(fileSavePath))
				{
					picbox1.Image = Image.FromFile(SC.pic1Path);
					fileSavePath = SC.pic1Path;
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
			fileSavePath2 = SC.pic2Path;
			if (fileSavePath2 != "")
			{
				if (File.Exists(fileSavePath2))
				{
					picbox2.Image = Image.FromFile(SC.pic2Path);
					fileSavePath2 = SC.pic2Path;
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
			((Control)(object)CmbAssetCategory).Enabled = false;
			txtAssetName.Focus();
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
		RECID = 0L;
		((Control)(object)CmbAssetCategory).Enabled = true;
		txtUseableLifeMonth.Text = string.Empty;
		((Control)(object)CmbAssetCondition).Text = string.Empty;
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
		((Control)(object)CmbAccomolativeAcc).Text = string.Empty;
		((Control)(object)CmbAssetStatus).Text = string.Empty;
		((Control)(object)CmbDepartment).Text = string.Empty;
		((Control)(object)cmbDepriciationAcc).Text = string.Empty;
		((Control)(object)CmbExpenseMaintenanceAccount).Text = string.Empty;
		((Control)(object)CmbDepreciationMethod).Text = string.Empty;
		((Control)(object)CmbGlAccount).Text = string.Empty;
		txtDepreciationRate.Text = string.Empty;
		ofd = string.Empty;
		fileSavePath = string.Empty;
		picbox1.Image = null;
		ofd2 = string.Empty;
		fileSavePath2 = string.Empty;
		picbox2.Image = null;
		txtAssetName.Focus();
	}

	private void BtnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "AccountsWithCustomGroupId" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			ItemCategory();
			int categoryId = Conversion.ToInt(CmbAssetCategory.Value);
			AssetItemFill(categoryId);
			AssetStatusBind();
			AssetAccountBind();
			ExpenseAccountFill();
			DepartmentBind();
			AssetConditionBind();
			DepreciatonMethodNameBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmAssetsRegister_FormClosing(object sender, FormClosingEventArgs e)
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
			DataTable lst = AssetRegister.FormHistory(obj);
			if (lst != null && lst.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("CategoryId", typeof(int));
				dt.Columns.Add("CategoryDescription", typeof(string));
				dt.Columns.Add("CategoryCode", typeof(string));
				dt.Columns.Add("SerialNo", typeof(string));
				dt.Columns.Add("AssetName", typeof(string));
				dt.Columns.Add("ItemId", typeof(int));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("ItemCode", typeof(string));
				dt.Columns.Add("MakeDesc", typeof(string));
				dt.Columns.Add("ModelDesc", typeof(string));
				dt.Columns.Add("PurchaseDate", typeof(DateTime));
				dt.Columns.Add("PurchasePrice", typeof(decimal));
				dt.Columns.Add("UseableLifeMonth", typeof(int));
				dt.Columns.Add("ExpiryDate", typeof(DateTime));
				dt.Columns.Add("CurrentValue", typeof(decimal));
				dt.Columns.Add("AssetStatus", typeof(string));
				dt.Columns.Add("DepartmentName", typeof(string));
				dt.Columns.Add("AssetCondition", typeof(string));
				dt.Columns.Add("AccumulatedDepreciationAcId", typeof(int));
				dt.Columns.Add("AccumulatedDepreciationAccount", typeof(string));
				dt.Columns.Add("DepreciationExpenseAcId", typeof(int));
				dt.Columns.Add("DepreciationExpenseAccount", typeof(string));
				dt.Columns.Add("CapitalWIPAccountId", typeof(int));
				dt.Columns.Add("GlAccountAccount", typeof(string));
				dt.Columns.Add("ExpenseMaintenanceAccount", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUserName", typeof(string));
				dt.Columns.Add("LastModifyDate", typeof(DateTime));
				dt.Columns.Add("LastModifyUserName", typeof(string));
				foreach (DataRow i in lst.Rows)
				{
					dt.Rows.Add(i["assetRegisterId"], i["categoryId"], i["categoryDescription"], i["categoryCode"], i["serialNo"], i["assetName"], i["itemId"], i["ItemName"], i["ItemCode"], i["makeDesc"], i["modelDesc"], i["purchaseDate"], i["purchasePrice"], i["useableLifeMonth"], i["expiryDate"], i["currentValue"], i["assetStatus"], i["DepartmentName"], i["AssetCondition"], i["accumulateddepriciationAcId"], i["AccumulatedDepreciationAccount"], i["depriciationExpenseAcId"], i["DepreciationExpenseAccount"], i["capitalWIPAccountId"], i["GlAccountAccount"], i["ExpenseMaintenanceAccount"], i["entryDate"], i["EntryUserName"], i["LastModifyDate"], i["LastModifyUserName"]);
				}
				DatagridHistory.DataSource = dt;
				DatagridHistory.RetrieveStructure();
				DatagridHistory.ColumnAutoResize = false;
				DatagridHistory.RootTable.Columns["Id"].Visible = false;
				DatagridHistory.RootTable.Columns["CategoryId"].Visible = false;
				DatagridHistory.RootTable.Columns["ItemId"].Visible = false;
				DatagridHistory.RootTable.Columns["AccumulatedDepreciationAcId"].Visible = false;
				DatagridHistory.RootTable.Columns["DepreciationExpenseAcId"].Visible = false;
				DatagridHistory.RootTable.Columns["CapitalWIPAccountId"].Visible = false;
				GridEX_Helper.GridWrappingAndColumnSettings(DatagridHistory, 2);
				DatagridHistory.RootTable.Columns["PurchaseDate"].FormatString = "dd-MMM-yy";
				DatagridHistory.RootTable.Columns["ExpiryDate"].FormatString = "dd-MMM-yy";
				DatagridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
				DatagridHistory.RootTable.Columns["LastModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
				DatagridHistory.RootTable.Columns["CategoryDescription"].Width = Constants.InventoryConstants.CategoryDescription;
				DatagridHistory.RootTable.Columns["AssetName"].Width = Constants.InventoryConstants.ItemName;
				DatagridHistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
				DatagridHistory.RootTable.Columns["ModelDesc"].Width = Constants.InventoryConstants.ItemCode;
				DatagridHistory.RootTable.Columns["DepartmentName"].Width = Constants.InventoryConstants.OtherDescription;
				DatagridHistory.RootTable.Columns["PurchaseDate"].Width = Constants.InventoryConstants.DateConstant;
				DatagridHistory.RootTable.Columns["PurchasePrice"].Width = Constants.InventoryConstants.AmountConstant;
				DatagridHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
				DatagridHistory.RootTable.Columns["EntryUserName"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
				DatagridHistory.RootTable.Columns["LastModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
				DatagridHistory.RootTable.Columns["LastModifyUserName"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			}
			else
			{
				DatagridHistory.DataSource = null;
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

	private void frmAssetsRegister_KeyDown(object sender, KeyEventArgs e)
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

	private void txtPurchaseDate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateExpiryDate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateExpiryDate()
	{
		try
		{
			_ = txtPurchaseDate.Value;
			if (txtUseableLifeMonth.Text != string.Empty)
			{
				DateTime purchaseDate = Conversion.ToDateTime((object)txtPurchaseDate.Value);
				int useableLifeMonths = Conversion.ToInt((object)txtUseableLifeMonth.Text);
				DateTime expiryDate = purchaseDate.AddMonths(useableLifeMonths);
				txtExpiryDate.Value = expiryDate;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtUseableLifeMonth_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateExpiryDate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtCurrentValue_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtPurchasePrice_KeyPress(object sender, KeyPressEventArgs e)
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
		//IL_042a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Expected O, but got Unknown
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Expected O, but got Unknown
		//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c3: Expected O, but got Unknown
		//IL_0511: Unknown result type (might be due to invalid IL or missing references)
		//IL_051b: Expected O, but got Unknown
		//IL_0553: Unknown result type (might be due to invalid IL or missing references)
		//IL_055d: Expected O, but got Unknown
		//IL_05d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e1: Expected O, but got Unknown
		//IL_05e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ec: Expected O, but got Unknown
		//IL_05ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f7: Expected O, but got Unknown
		//IL_06d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06de: Expected O, but got Unknown
		//IL_06df: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e9: Expected O, but got Unknown
		//IL_0758: Unknown result type (might be due to invalid IL or missing references)
		//IL_0762: Expected O, but got Unknown
		//IL_079a: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a4: Expected O, but got Unknown
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
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
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.AssetSchema.frmAssetsRegister));
		GridEXLayout DatagridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance129 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
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
		this.label33 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.CmbExpenseMaintenanceAccount = new UltraCombo();
		this.label31 = new System.Windows.Forms.Label();
		this.CmbGlAccount = new UltraCombo();
		this.label42 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.cmbDepriciationAcc = new UltraCombo();
		this.label29 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.BtnDefineAssetDepartment = new System.Windows.Forms.Button();
		this.CmbAccomolativeAcc = new UltraCombo();
		this.BtnAssetItemDefine = new System.Windows.Forms.Button();
		this.btnAssetCategory = new System.Windows.Forms.Button();
		this.txtExpiryDate = new System.Windows.Forms.DateTimePicker();
		this.label13 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.CmbAssetItem = new UltraCombo();
		this.txtUseableLifeMonth = new System.Windows.Forms.TextBox();
		this.txtDepreciationRate = new System.Windows.Forms.TextBox();
		this.txtCurrentValue = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.txtPurchasePrice = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.txtPurchaseDate = new System.Windows.Forms.DateTimePicker();
		this.label11 = new System.Windows.Forms.Label();
		this.txtVendor = new System.Windows.Forms.TextBox();
		this.CmbDepreciationMethod = new UltraCombo();
		this.CmbAssetStatus = new UltraCombo();
		this.CmbDepartment = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.txtAssetLocation = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtAssetSerialNo = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.txtManufacturer = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.txtModelDesc = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtMakeDesc = new System.Windows.Forms.TextBox();
		this.label28 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtBrandName = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.txtAssetName = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.CmbAssetCondition = new UltraCombo();
		this.CmbAssetCategory = new UltraCombo();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label24 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.BtnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
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
		((System.ComponentModel.ISupportInitialize)this.CmbGlAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDepriciationAcc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAccomolativeAcc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAssetItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDepreciationMethod).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAssetStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDepartment).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAssetCondition).BeginInit();
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
		this.tabControl1.Size = new System.Drawing.Size(1084, 539);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1076, 510);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.BrandImages);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(3, 55);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1070, 452);
		this.panel4.TabIndex = 0;
		this.BrandImages.Controls.Add(this.btnImageBrowse2);
		this.BrandImages.Controls.Add(this.picbox2);
		this.BrandImages.Controls.Add(this.btnImageBrowse1);
		this.BrandImages.Controls.Add(this.picbox1);
		this.BrandImages.Dock = System.Windows.Forms.DockStyle.Left;
		this.BrandImages.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold);
		this.BrandImages.Location = new System.Drawing.Point(739, 0);
		this.BrandImages.Name = "BrandImages";
		this.BrandImages.Size = new System.Drawing.Size(333, 452);
		this.BrandImages.TabIndex = 1;
		this.BrandImages.TabStop = false;
		this.BrandImages.Text = "Asset Images";
		this.btnImageBrowse2.BackColor = System.Drawing.Color.Teal;
		this.btnImageBrowse2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnImageBrowse2.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Bold);
		this.btnImageBrowse2.ForeColor = System.Drawing.Color.White;
		this.btnImageBrowse2.Location = new System.Drawing.Point(6, 419);
		this.btnImageBrowse2.Name = "btnImageBrowse2";
		this.btnImageBrowse2.Size = new System.Drawing.Size(320, 31);
		this.btnImageBrowse2.TabIndex = 1;
		this.btnImageBrowse2.Text = "Browse";
		this.btnImageBrowse2.UseVisualStyleBackColor = false;
		this.btnImageBrowse2.Click += new System.EventHandler(btnImageBrowse2_Click);
		this.picbox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.picbox2.Location = new System.Drawing.Point(6, 236);
		this.picbox2.Name = "picbox2";
		this.picbox2.Size = new System.Drawing.Size(320, 180);
		this.picbox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picbox2.TabIndex = 36;
		this.picbox2.TabStop = false;
		this.btnImageBrowse1.BackColor = System.Drawing.Color.Teal;
		this.btnImageBrowse1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnImageBrowse1.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Bold);
		this.btnImageBrowse1.ForeColor = System.Drawing.Color.White;
		this.btnImageBrowse1.Location = new System.Drawing.Point(6, 201);
		this.btnImageBrowse1.Name = "btnImageBrowse1";
		this.btnImageBrowse1.Size = new System.Drawing.Size(320, 31);
		this.btnImageBrowse1.TabIndex = 0;
		this.btnImageBrowse1.Text = "Browse";
		this.btnImageBrowse1.UseVisualStyleBackColor = false;
		this.btnImageBrowse1.Click += new System.EventHandler(btnImageBrowse1_Click);
		this.picbox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.picbox1.Location = new System.Drawing.Point(6, 18);
		this.picbox1.Name = "picbox1";
		this.picbox1.Size = new System.Drawing.Size(320, 180);
		this.picbox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picbox1.TabIndex = 35;
		this.picbox1.TabStop = false;
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbExpenseMaintenanceAccount);
		this.groupBox1.Controls.Add(this.label31);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbGlAccount);
		this.groupBox1.Controls.Add(this.label42);
		this.groupBox1.Controls.Add(this.label41);
		this.groupBox1.Controls.Add(this.label40);
		this.groupBox1.Controls.Add(this.label39);
		this.groupBox1.Controls.Add(this.label38);
		this.groupBox1.Controls.Add(this.label37);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDepriciationAcc);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add(this.label34);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add(this.BtnDefineAssetDepartment);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccomolativeAcc);
		this.groupBox1.Controls.Add(this.BtnAssetItemDefine);
		this.groupBox1.Controls.Add(this.btnAssetCategory);
		this.groupBox1.Controls.Add(this.txtExpiryDate);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add(this.label18);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAssetItem);
		this.groupBox1.Controls.Add(this.txtUseableLifeMonth);
		this.groupBox1.Controls.Add(this.txtDepreciationRate);
		this.groupBox1.Controls.Add(this.txtCurrentValue);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.label36);
		this.groupBox1.Controls.Add(this.label16);
		this.groupBox1.Controls.Add(this.txtPurchasePrice);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.txtPurchaseDate);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.txtVendor);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDepreciationMethod);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAssetStatus);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDepartment);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.txtAssetLocation);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.txtAssetSerialNo);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.txtManufacturer);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.txtModelDesc);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.txtMakeDesc);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add(this.label25);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.txtBrandName);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.txtAssetName);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAssetCondition);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAssetCategory);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(739, 452);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.ForeColor = System.Drawing.Color.Red;
		this.label33.Location = new System.Drawing.Point(484, 19);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(15, 13);
		this.label33.TabIndex = 82;
		this.label33.Text = "*";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.Red;
		this.label32.Location = new System.Drawing.Point(484, 47);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(15, 13);
		this.label32.TabIndex = 82;
		this.label32.Text = "*";
		this.CmbExpenseMaintenanceAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbExpenseMaintenanceAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbExpenseMaintenanceAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbExpenseMaintenanceAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbExpenseMaintenanceAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbExpenseMaintenanceAccount).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbExpenseMaintenanceAccount).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbExpenseMaintenanceAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbExpenseMaintenanceAccount).Location = new System.Drawing.Point(504, 287);
		((System.Windows.Forms.Control)(object)this.CmbExpenseMaintenanceAccount).Name = "CmbExpenseMaintenanceAccount";
		this.CmbExpenseMaintenanceAccount.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.CmbExpenseMaintenanceAccount).Size = new System.Drawing.Size(229, 26);
		((System.Windows.Forms.Control)(object)this.CmbExpenseMaintenanceAccount).TabIndex = 26;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.ForeColor = System.Drawing.Color.Red;
		this.label31.Location = new System.Drawing.Point(484, 73);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(15, 13);
		this.label31.TabIndex = 82;
		this.label31.Text = "*";
		this.CmbGlAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbGlAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbGlAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbGlAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbGlAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbGlAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbGlAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbGlAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGlAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance80;
		((SpecialBoxBase)((UltraGridBase)this.CmbGlAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGlAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbGlAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbGlAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbGlAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbGlAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbGlAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbGlAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGlAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance85).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbGlAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbGlAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbGlAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance86).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance86).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance86).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGlAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbGlAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbGlAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbGlAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbGlAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbGlAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbGlAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbGlAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbGlAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbGlAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbGlAccount).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbGlAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbGlAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbGlAccount).Location = new System.Drawing.Point(504, 201);
		((System.Windows.Forms.Control)(object)this.CmbGlAccount).Name = "CmbGlAccount";
		this.CmbGlAccount.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.CmbGlAccount).Size = new System.Drawing.Size(229, 26);
		((System.Windows.Forms.Control)(object)this.CmbGlAccount).TabIndex = 23;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.ForeColor = System.Drawing.Color.Red;
		this.label42.Location = new System.Drawing.Point(484, 294);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(15, 13);
		this.label42.TabIndex = 82;
		this.label42.Text = "*";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.ForeColor = System.Drawing.Color.Red;
		this.label41.Location = new System.Drawing.Point(484, 265);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(15, 13);
		this.label41.TabIndex = 82;
		this.label41.Text = "*";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.ForeColor = System.Drawing.Color.Red;
		this.label40.Location = new System.Drawing.Point(484, 236);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(15, 13);
		this.label40.TabIndex = 82;
		this.label40.Text = "*";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.ForeColor = System.Drawing.Color.Red;
		this.label39.Location = new System.Drawing.Point(484, 208);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(15, 13);
		this.label39.TabIndex = 82;
		this.label39.Text = "*";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.ForeColor = System.Drawing.Color.Red;
		this.label38.Location = new System.Drawing.Point(484, 180);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(15, 13);
		this.label38.TabIndex = 82;
		this.label38.Text = "*";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.ForeColor = System.Drawing.Color.Red;
		this.label37.Location = new System.Drawing.Point(484, 153);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(15, 13);
		this.label37.TabIndex = 82;
		this.label37.Text = "*";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label15.Location = new System.Drawing.Point(360, 285);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(98, 30);
		this.label15.TabIndex = 608;
		this.label15.Text = "Expense\r\nMaintenance A/c";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.ForeColor = System.Drawing.Color.Red;
		this.label30.Location = new System.Drawing.Point(484, 98);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(15, 13);
		this.label30.TabIndex = 82;
		this.label30.Text = "*";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(360, 207);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(43, 15);
		this.label19.TabIndex = 115;
		this.label19.Text = "GL A/c";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.ForeColor = System.Drawing.Color.Red;
		this.label35.Location = new System.Drawing.Point(484, 126);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(15, 13);
		this.label35.TabIndex = 82;
		this.label35.Text = "*";
		this.cmbDepriciationAcc.AllowNull = (DefaultableBoolean)1;
		this.cmbDepriciationAcc.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDepriciationAcc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDepriciationAcc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance93;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance98).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance99).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance99).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance99).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance101).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDepriciationAcc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDepriciationAcc).Enabled = false;
		((System.Windows.Forms.Control)(object)this.cmbDepriciationAcc).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDepriciationAcc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDepriciationAcc).Location = new System.Drawing.Point(504, 258);
		((System.Windows.Forms.Control)(object)this.cmbDepriciationAcc).Name = "cmbDepriciationAcc";
		this.cmbDepriciationAcc.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.cmbDepriciationAcc).Size = new System.Drawing.Size(229, 26);
		((System.Windows.Forms.Control)(object)this.cmbDepriciationAcc).TabIndex = 25;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.ForeColor = System.Drawing.Color.Red;
		this.label29.Location = new System.Drawing.Point(102, 208);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(15, 13);
		this.label29.TabIndex = 82;
		this.label29.Text = "*";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(360, 235);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(102, 15);
		this.label21.TabIndex = 119;
		this.label21.Text = "Accomolative A/c";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(360, 125);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(120, 15);
		this.label34.TabIndex = 96;
		this.label34.Text = "Depreciation Method";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label17.Location = new System.Drawing.Point(360, 179);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(71, 15);
		this.label17.TabIndex = 110;
		this.label17.Text = "Asset Status";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(3, 207);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(96, 15);
		this.label10.TabIndex = 96;
		this.label10.Text = "Asset Deprtment";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(360, 264);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(94, 15);
		this.label20.TabIndex = 117;
		this.label20.Text = "Depriciation A/c";
		this.BtnDefineAssetDepartment.BackColor = System.Drawing.Color.White;
		this.BtnDefineAssetDepartment.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnDefineAssetDepartment.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.BtnDefineAssetDepartment.Location = new System.Drawing.Point(332, 201);
		this.BtnDefineAssetDepartment.Margin = new System.Windows.Forms.Padding(2);
		this.BtnDefineAssetDepartment.Name = "BtnDefineAssetDepartment";
		this.BtnDefineAssetDepartment.Size = new System.Drawing.Size(26, 27);
		this.BtnDefineAssetDepartment.TabIndex = 10;
		this.BtnDefineAssetDepartment.TabStop = false;
		this.BtnDefineAssetDepartment.UseVisualStyleBackColor = false;
		this.BtnDefineAssetDepartment.Click += new System.EventHandler(BtnDefineAssetDepartment_Click);
		this.CmbAccomolativeAcc.AllowNull = (DefaultableBoolean)1;
		this.CmbAccomolativeAcc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccomolativeAcc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccomolativeAcc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccomolativeAcc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbAccomolativeAcc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccomolativeAcc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccomolativeAcc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccomolativeAcc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance53;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccomolativeAcc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccomolativeAcc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbAccomolativeAcc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccomolativeAcc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccomolativeAcc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccomolativeAcc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbAccomolativeAcc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccomolativeAcc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccomolativeAcc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance59).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccomolativeAcc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbAccomolativeAcc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccomolativeAcc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccomolativeAcc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccomolativeAcc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbAccomolativeAcc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccomolativeAcc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccomolativeAcc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbAccomolativeAcc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccomolativeAcc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbAccomolativeAcc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccomolativeAcc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccomolativeAcc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccomolativeAcc).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbAccomolativeAcc).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbAccomolativeAcc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAccomolativeAcc).Location = new System.Drawing.Point(504, 229);
		((System.Windows.Forms.Control)(object)this.CmbAccomolativeAcc).Name = "CmbAccomolativeAcc";
		this.CmbAccomolativeAcc.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.CmbAccomolativeAcc).Size = new System.Drawing.Size(229, 26);
		((System.Windows.Forms.Control)(object)this.CmbAccomolativeAcc).TabIndex = 24;
		this.BtnAssetItemDefine.BackColor = System.Drawing.Color.White;
		this.BtnAssetItemDefine.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnAssetItemDefine.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.BtnAssetItemDefine.Location = new System.Drawing.Point(332, 40);
		this.BtnAssetItemDefine.Margin = new System.Windows.Forms.Padding(2);
		this.BtnAssetItemDefine.Name = "BtnAssetItemDefine";
		this.BtnAssetItemDefine.Size = new System.Drawing.Size(26, 27);
		this.BtnAssetItemDefine.TabIndex = 3;
		this.BtnAssetItemDefine.TabStop = false;
		this.BtnAssetItemDefine.UseVisualStyleBackColor = false;
		this.BtnAssetItemDefine.Click += new System.EventHandler(BtnAssetItemDefine_Click);
		this.btnAssetCategory.BackColor = System.Drawing.Color.White;
		this.btnAssetCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAssetCategory.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnAssetCategory.Location = new System.Drawing.Point(332, 12);
		this.btnAssetCategory.Margin = new System.Windows.Forms.Padding(2);
		this.btnAssetCategory.Name = "btnAssetCategory";
		this.btnAssetCategory.Size = new System.Drawing.Size(26, 27);
		this.btnAssetCategory.TabIndex = 1;
		this.btnAssetCategory.TabStop = false;
		this.btnAssetCategory.UseVisualStyleBackColor = false;
		this.btnAssetCategory.Click += new System.EventHandler(btnAssetCategory_Click);
		this.txtExpiryDate.Enabled = false;
		this.txtExpiryDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExpiryDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtExpiryDate.Location = new System.Drawing.Point(504, 68);
		this.txtExpiryDate.Name = "txtExpiryDate";
		this.txtExpiryDate.Size = new System.Drawing.Size(229, 23);
		this.txtExpiryDate.TabIndex = 18;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(360, 72);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(70, 15);
		this.label13.TabIndex = 102;
		this.label13.Text = "Expiry Date ";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(3, 46);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(63, 15);
		this.label18.TabIndex = 113;
		this.label18.Text = "Asset Item";
		this.CmbAssetItem.AllowNull = (DefaultableBoolean)1;
		this.CmbAssetItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAssetItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAssetItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAssetItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance66;
		((SpecialBoxBase)((UltraGridBase)this.CmbAssetItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance72).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance73).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAssetItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAssetItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAssetItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAssetItem).Location = new System.Drawing.Point(119, 40);
		((System.Windows.Forms.Control)(object)this.CmbAssetItem).Name = "CmbAssetItem";
		((System.Windows.Forms.Control)(object)this.CmbAssetItem).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.CmbAssetItem).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbAssetItem).Leave += new System.EventHandler(CmbAssetItem_Leave);
		this.txtUseableLifeMonth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtUseableLifeMonth.Enabled = false;
		this.txtUseableLifeMonth.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtUseableLifeMonth.Location = new System.Drawing.Point(504, 42);
		this.txtUseableLifeMonth.Name = "txtUseableLifeMonth";
		this.txtUseableLifeMonth.ReadOnly = true;
		this.txtUseableLifeMonth.Size = new System.Drawing.Size(229, 23);
		this.txtUseableLifeMonth.TabIndex = 17;
		this.txtUseableLifeMonth.TextChanged += new System.EventHandler(txtUseableLifeMonth_TextChanged);
		this.txtDepreciationRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDepreciationRate.Enabled = false;
		this.txtDepreciationRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDepreciationRate.Location = new System.Drawing.Point(504, 148);
		this.txtDepreciationRate.Name = "txtDepreciationRate";
		this.txtDepreciationRate.ReadOnly = true;
		this.txtDepreciationRate.Size = new System.Drawing.Size(229, 23);
		this.txtDepreciationRate.TabIndex = 21;
		this.txtCurrentValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCurrentValue.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCurrentValue.Location = new System.Drawing.Point(504, 93);
		this.txtCurrentValue.Name = "txtCurrentValue";
		this.txtCurrentValue.Size = new System.Drawing.Size(229, 23);
		this.txtCurrentValue.TabIndex = 19;
		this.txtCurrentValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCurrentValue_KeyPress);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(360, 46);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(109, 15);
		this.label14.TabIndex = 104;
		this.label14.Text = "Useable Life Month";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(360, 152);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(101, 15);
		this.label36.TabIndex = 108;
		this.label36.Text = "Depreciation Rate";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(360, 97);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(78, 15);
		this.label16.TabIndex = 108;
		this.label16.Text = "Current Value";
		this.txtPurchasePrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPurchasePrice.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPurchasePrice.Location = new System.Drawing.Point(627, 14);
		this.txtPurchasePrice.Name = "txtPurchasePrice";
		this.txtPurchasePrice.Size = new System.Drawing.Size(106, 23);
		this.txtPurchasePrice.TabIndex = 16;
		this.txtPurchasePrice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPurchasePrice_KeyPress);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(360, 18);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(124, 15);
		this.label12.TabIndex = 100;
		this.label12.Text = "Purchase Date && Price";
		this.txtPurchaseDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPurchaseDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtPurchaseDate.Location = new System.Drawing.Point(504, 14);
		this.txtPurchaseDate.Name = "txtPurchaseDate";
		this.txtPurchaseDate.Size = new System.Drawing.Size(122, 23);
		this.txtPurchaseDate.TabIndex = 15;
		this.txtPurchaseDate.ValueChanged += new System.EventHandler(txtPurchaseDate_ValueChanged);
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(3, 319);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(100, 15);
		this.label11.TabIndex = 98;
		this.label11.Text = "Vendor / Supplier";
		this.txtVendor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVendor.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtVendor.Location = new System.Drawing.Point(119, 315);
		this.txtVendor.Name = "txtVendor";
		this.txtVendor.Size = new System.Drawing.Size(237, 23);
		this.txtVendor.TabIndex = 14;
		this.CmbDepreciationMethod.AllowNull = (DefaultableBoolean)1;
		this.CmbDepreciationMethod.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDepreciationMethod.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDepreciationMethod.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDepreciationMethod).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbDepreciationMethod).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDepreciationMethod).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDepreciationMethod).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDepreciationMethod).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance56;
		((SpecialBoxBase)((UltraGridBase)this.CmbDepreciationMethod).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDepreciationMethod).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbDepreciationMethod).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDepreciationMethod).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDepreciationMethod).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDepreciationMethod).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbDepreciationMethod).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDepreciationMethod).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDepreciationMethod).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance111).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDepreciationMethod).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbDepreciationMethod).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDepreciationMethod).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDepreciationMethod).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDepreciationMethod).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbDepreciationMethod).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDepreciationMethod).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDepreciationMethod).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbDepreciationMethod).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDepreciationMethod).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbDepreciationMethod).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDepreciationMethod).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDepreciationMethod).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDepreciationMethod).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbDepreciationMethod).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDepreciationMethod.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDepreciationMethod).Location = new System.Drawing.Point(504, 119);
		((System.Windows.Forms.Control)(object)this.CmbDepreciationMethod).Name = "CmbDepreciationMethod";
		this.CmbDepreciationMethod.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.CmbDepreciationMethod).Size = new System.Drawing.Size(229, 26);
		((System.Windows.Forms.Control)(object)this.CmbDepreciationMethod).TabIndex = 20;
		this.CmbAssetStatus.AllowNull = (DefaultableBoolean)1;
		this.CmbAssetStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAssetStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAssetStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAssetStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbAssetStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAssetStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance121).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance121).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAssetStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAssetStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance123;
		((SpecialBoxBase)((UltraGridBase)this.CmbAssetStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAssetStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbAssetStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAssetStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAssetStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAssetStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbAssetStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAssetStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAssetStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance128).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAssetStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbAssetStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAssetStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAssetStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAssetStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbAssetStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAssetStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAssetStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbAssetStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAssetStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbAssetStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAssetStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAssetStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAssetStatus).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAssetStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAssetStatus).Location = new System.Drawing.Point(504, 173);
		((System.Windows.Forms.Control)(object)this.CmbAssetStatus).Name = "CmbAssetStatus";
		((System.Windows.Forms.Control)(object)this.CmbAssetStatus).Size = new System.Drawing.Size(229, 26);
		((System.Windows.Forms.Control)(object)this.CmbAssetStatus).TabIndex = 22;
		this.CmbDepartment.AllowNull = (DefaultableBoolean)1;
		this.CmbDepartment.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDepartment.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDepartment.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDepartment).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbDepartment).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDepartment).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDepartment).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDepartment).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDepartment.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDepartment).Location = new System.Drawing.Point(119, 201);
		((System.Windows.Forms.Control)(object)this.CmbDepartment).Name = "CmbDepartment";
		((System.Windows.Forms.Control)(object)this.CmbDepartment).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.CmbDepartment).TabIndex = 9;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(3, 293);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(93, 15);
		this.label9.TabIndex = 94;
		this.label9.Text = "Asset Loacation ";
		this.txtAssetLocation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAssetLocation.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAssetLocation.Location = new System.Drawing.Point(119, 289);
		this.txtAssetLocation.Name = "txtAssetLocation";
		this.txtAssetLocation.Size = new System.Drawing.Size(237, 23);
		this.txtAssetLocation.TabIndex = 13;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(3, 152);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(95, 15);
		this.label8.TabIndex = 92;
		this.label8.Text = "Assets Condition";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(3, 179);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(86, 15);
		this.label7.TabIndex = 90;
		this.label7.Text = "Asset Serial No";
		this.txtAssetSerialNo.BackColor = System.Drawing.Color.WhiteSmoke;
		this.txtAssetSerialNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAssetSerialNo.Enabled = false;
		this.txtAssetSerialNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAssetSerialNo.Location = new System.Drawing.Point(119, 175);
		this.txtAssetSerialNo.Name = "txtAssetSerialNo";
		this.txtAssetSerialNo.Size = new System.Drawing.Size(237, 23);
		this.txtAssetSerialNo.TabIndex = 8;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(3, 235);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(79, 15);
		this.label6.TabIndex = 88;
		this.label6.Text = "Manufacturer";
		this.txtManufacturer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtManufacturer.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtManufacturer.Location = new System.Drawing.Point(119, 231);
		this.txtManufacturer.Name = "txtManufacturer";
		this.txtManufacturer.Size = new System.Drawing.Size(237, 23);
		this.txtManufacturer.TabIndex = 11;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(3, 125);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(72, 15);
		this.label4.TabIndex = 86;
		this.label4.Text = "Asset Modal";
		this.txtModelDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtModelDesc.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtModelDesc.Location = new System.Drawing.Point(119, 121);
		this.txtModelDesc.Name = "txtModelDesc";
		this.txtModelDesc.Size = new System.Drawing.Size(237, 23);
		this.txtModelDesc.TabIndex = 6;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(3, 264);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(65, 15);
		this.label3.TabIndex = 84;
		this.label3.Text = "Make Desc";
		this.txtMakeDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMakeDesc.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMakeDesc.Location = new System.Drawing.Point(119, 260);
		this.txtMakeDesc.Name = "txtMakeDesc";
		this.txtMakeDesc.Size = new System.Drawing.Size(237, 23);
		this.txtMakeDesc.TabIndex = 12;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.ForeColor = System.Drawing.Color.Red;
		this.label28.Location = new System.Drawing.Point(102, 180);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(15, 13);
		this.label28.TabIndex = 82;
		this.label28.Text = "*";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.ForeColor = System.Drawing.Color.Red;
		this.label27.Location = new System.Drawing.Point(102, 153);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(15, 13);
		this.label27.TabIndex = 82;
		this.label27.Text = "*";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ForeColor = System.Drawing.Color.Red;
		this.label26.Location = new System.Drawing.Point(102, 98);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(15, 13);
		this.label26.TabIndex = 82;
		this.label26.Text = "*";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.ForeColor = System.Drawing.Color.Red;
		this.label25.Location = new System.Drawing.Point(102, 19);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(15, 13);
		this.label25.TabIndex = 82;
		this.label25.Text = "*";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(3, 72);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(38, 15);
		this.label2.TabIndex = 82;
		this.label2.Text = "Brand";
		this.txtBrandName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBrandName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBrandName.Location = new System.Drawing.Point(119, 68);
		this.txtBrandName.Name = "txtBrandName";
		this.txtBrandName.Size = new System.Drawing.Size(237, 23);
		this.txtBrandName.TabIndex = 4;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(3, 97);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(70, 15);
		this.label1.TabIndex = 80;
		this.label1.Text = "Asset Name";
		this.txtAssetName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAssetName.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAssetName.Location = new System.Drawing.Point(119, 93);
		this.txtAssetName.Name = "txtAssetName";
		this.txtAssetName.Size = new System.Drawing.Size(237, 23);
		this.txtAssetName.TabIndex = 5;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(3, 18);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(85, 15);
		this.label5.TabIndex = 78;
		this.label5.Text = "Asset Category";
		this.CmbAssetCondition.AllowNull = (DefaultableBoolean)1;
		this.CmbAssetCondition.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAssetCondition.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAssetCondition.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAssetCondition).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbAssetCondition).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAssetCondition).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAssetCondition).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAssetCondition).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance105;
		((SpecialBoxBase)((UltraGridBase)this.CmbAssetCondition).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAssetCondition).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbAssetCondition).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAssetCondition).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAssetCondition).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAssetCondition).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbAssetCondition).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAssetCondition).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAssetCondition).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance46).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAssetCondition).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbAssetCondition).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAssetCondition).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance47).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAssetCondition).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAssetCondition).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbAssetCondition).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAssetCondition).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAssetCondition).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbAssetCondition).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAssetCondition).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbAssetCondition).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAssetCondition).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAssetCondition).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAssetCondition).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAssetCondition.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAssetCondition).Location = new System.Drawing.Point(119, 146);
		((System.Windows.Forms.Control)(object)this.CmbAssetCondition).Name = "CmbAssetCondition";
		((System.Windows.Forms.Control)(object)this.CmbAssetCondition).Size = new System.Drawing.Size(237, 26);
		((System.Windows.Forms.Control)(object)this.CmbAssetCondition).TabIndex = 7;
		this.CmbAssetCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbAssetCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAssetCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAssetCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAssetCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance109;
		((SpecialBoxBase)((UltraGridBase)this.CmbAssetCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance115).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance116).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance116).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance116).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance117).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAssetCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAssetCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAssetCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAssetCategory).Location = new System.Drawing.Point(119, 12);
		((System.Windows.Forms.Control)(object)this.CmbAssetCategory).Name = "CmbAssetCategory";
		((System.Windows.Forms.Control)(object)this.CmbAssetCategory).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.CmbAssetCategory).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbAssetCategory).Leave += new System.EventHandler(CmbAssetCategory_Leave);
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label24);
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1070, 52);
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
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.btnNew, this.BtnRefresh, this.btnSave, this.btnUpdate, this.toolStripSeparator2, this.BtnShortCutkeys });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1070, 27);
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
		this.tabPage2.Size = new System.Drawing.Size(1076, 510);
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
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Size = new System.Drawing.Size(1070, 374);
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
		this.panel1.Size = new System.Drawing.Size(1070, 26);
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
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1022, 1);
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
		this.panel3.Size = new System.Drawing.Size(1070, 47);
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
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance40;
		((SpecialBoxBase)((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCategoryHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance132;
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
		this.panel6.Size = new System.Drawing.Size(1070, 57);
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
		this.toolStrip1.Size = new System.Drawing.Size(1070, 27);
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
		base.ClientSize = new System.Drawing.Size(1084, 539);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "frmAssetsRegister";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmAssetsRegister_FormClosing);
		base.Load += new System.EventHandler(frmAssetsRegister_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmAssetsRegister_KeyDown);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.BrandImages.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.picbox2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picbox1).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbExpenseMaintenanceAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGlAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDepriciationAcc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAccomolativeAcc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAssetItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDepreciationMethod).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAssetStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDepartment).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAssetCondition).EndInit();
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
