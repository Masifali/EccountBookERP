using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Main.Commission;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Main.Commission;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.WholeSale.Commission;

public class CustomerCommissionPolicy : Form
{
	private int RId;

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtGrid = new DataTable();

	private Rightsobjects formrights = new Rightsobjects();

	public UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Panel panel4;

	private DateTimePicker dtpEffectedDate;

	private TextBox txtCommissionPercentage;

	private Label label6;

	private Label label3;

	private Label label4;

	private Label label13;

	private Panel panel2;

	private ToolStrip tsPriceSchedule;

	private ToolStripButton BtnNew;

	private ToolStripButton BtnEdit;

	private ToolStripButton BtnSave;

	private BindingNavigator miniToolStrip;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel5;

	internal UltraCombo CmbCommissionType;

	private CheckBox chkIsActive;

	internal UltraCombo cmbCustomer;

	private Label label2;

	internal UltraCombo CmbCommissionItem;

	private Label label1;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton btnAllocateBrandToType;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripButton btnAllocateDiscountItemToType;

	internal GridEX grd;

	private Panel panel6;

	private Label label5;

	private ToolStripButton BtnRefresh;

	private ToolStripButton btnshortcut;

	internal UltraCombo CmbIncentiveAccount;

	private Label label7;

	public CustomerCommissionPolicy(UserAccount user)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void InvfrmPriceSchedule_Load(object sender, EventArgs e)
	{
		try
		{
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			BtnSave.Enabled = formrights.DoHaveSaveRight;
			BtnEdit.Enabled = formrights.DoHaveUpdateRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formrights.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
			CommissionTypeFill();
			CustomerBind();
			GridFill();
			BtnEdit.Visible = false;
			dtpEffectedDate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionTypeFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(CmbCommissionType.Value);
			DataTable dt = CommissionType.FormHistory(new CommissionType
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbCommissionType, "Id", "CommissionTypeDescription", "CommissionType Type", true);
				if (Id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbCommissionType.Value = Id;
					}
					else
					{
						((Control)(object)CmbCommissionType).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbCommissionType).Text = string.Empty;
				((UltraGridBase)CmbCommissionType).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CustomerBind()
	{
		try
		{
			int Id = Conversion.ToInt(cmbCustomer.Value);
			int IncId = Conversion.ToInt(CmbIncentiveAccount.Value);
			DataTable DtCustomer = CommonServices.GetSupplierustomerByCustomerGroupId("");
			if (DtCustomer.Rows.Count > 0)
			{
				DDL.BindDDL(DtCustomer, cmbCustomer, "Id", "CompanyName", "Customer Name", ZeroIndex: true);
				DDL.BindDDL(DtCustomer, CmbIncentiveAccount, "Id", "CompanyName", "Incentive Account", ZeroIndex: true);
				if (Id > 0)
				{
					if (DtCustomer.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						cmbCustomer.Value = Id;
					}
					else
					{
						((Control)(object)cmbCustomer).Text = string.Empty;
					}
				}
				if (IncId > 0)
				{
					if (DtCustomer.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == IncId))
					{
						CmbIncentiveAccount.Value = IncId;
					}
					else
					{
						((Control)(object)CmbIncentiveAccount).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbCustomer).Text = string.Empty;
				((UltraGridBase)cmbCustomer).DataSource = null;
				((Control)(object)CmbIncentiveAccount).Text = string.Empty;
				((UltraGridBase)CmbIncentiveAccount).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionItemsByCommissionTypeId(int CommissionTypeId)
	{
		try
		{
			int organizationId = UserAccount.OrganizationId;
			int CompId = UserAccount.CompanyId;
			DataTable dt = AllocateCommissionItemToCommissionType.CommissionItemsFromAllocationByCommissionTypeId(organizationId, CompId, CommissionTypeId);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbCommissionItem, "CommissionItemId", "CommissionItemName", "Commission Item", ZeroIndex: true);
				((UltraGridBase)CmbCommissionItem).DisplayLayout.Bands[0].Columns[0].Hidden = true;
				((UltraGridBase)CmbCommissionItem).DisplayLayout.Bands[0].Columns[1].Hidden = true;
				((UltraGridBase)CmbCommissionItem).DisplayLayout.Bands[0].Columns[4].Hidden = true;
			}
			else
			{
				((Control)(object)CmbCommissionItem).Text = string.Empty;
				((UltraGridBase)CmbCommissionItem).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbDiscountType_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbCommissionType).ActiveRow != null || Conversion.ToInt(CmbCommissionType.Value) > 0)
			{
				CommissionItemsByCommissionTypeId(Conversion.ToInt(CmbCommissionType.Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbDiscItem_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)CmbCommissionItem).Enabled && ((UltraGridBase)CmbCommissionItem).ActiveRow != null)
			{
				txtCommissionPercentage.Text = Conversion.ToString(((UltraDropDownBase)CmbCommissionItem).SelectedRow.Cells[3].Value);
				txtCommissionPercentage.Enabled = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void cmbDiscItem_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)CmbCommissionItem).Enabled && ((UltraGridBase)CmbCommissionItem).ActiveRow != null)
			{
				txtCommissionPercentage.Text = Conversion.ToString(((UltraDropDownBase)CmbCommissionItem).SelectedRow.Cells[3].Value);
				txtCommissionPercentage.Enabled = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)cmbCustomer).ActiveRow == null || Conversion.ToInt(cmbCustomer.Value) == 0)
		{
			MessageBox.Show("Customer Name Field is Required");
			cmbCustomer.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCommissionType).ActiveRow == null || Conversion.ToInt(CmbCommissionType.Value) == 0)
		{
			MessageBox.Show("Commission Type Field is Required");
			CmbCommissionType.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtCommissionPercentage.Text) == 0.0)
		{
			MessageBox.Show("Commission Rate Field is Required");
			txtCommissionPercentage.Focus();
			return false;
		}
		if (((UltraGridBase)CmbIncentiveAccount).ActiveRow == null || Conversion.ToInt(CmbIncentiveAccount.Value) == 0)
		{
			MessageBox.Show("Incentive Account Field is Required");
			CmbIncentiveAccount.Focus();
			return false;
		}
		return true;
	}

	private void BtnSave_Click(object sender, EventArgs e)
	{
		try
		{
			RId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			if (RId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			CustomerCommissionPolicy val = new CustomerCommissionPolicy
			{
				CustomerCommissionPolicyList = new List<CustomerCommissionPolicy>()
			};
			CustomerCommissionPolicy CustomerCommissionPolicy = new CustomerCommissionPolicy();
			CustomerCommissionPolicy.OrganizationId = UserAccount.OrganizationId;
			CustomerCommissionPolicy.CompanyId = UserAccount.CompanyId;
			CustomerCommissionPolicy.CustomerId = Conversion.ToInt(cmbCustomer.Value);
			CustomerCommissionPolicy.CustomerIncentivePartyId = Conversion.ToInt(CmbIncentiveAccount.Value);
			CustomerCommissionPolicy.EffectiveDate = dtpEffectedDate.Value;
			CustomerCommissionPolicy.CommissionType = "Percent";
			CustomerCommissionPolicy.CommissionTypeId = Conversion.ToInt(CmbCommissionType.Value);
			CustomerCommissionPolicy.CommissionItemsId = Conversion.ToInt(CmbCommissionItem.Value);
			CustomerCommissionPolicy.EntryUserId = UserAccount.ID;
			CustomerCommissionPolicy.ModifyUserId = UserAccount.ID;
			CustomerCommissionPolicy.EntryDate = DateTime.Now;
			CustomerCommissionPolicy.ModifyDate = DateTime.Now;
			CustomerCommissionPolicy.Id = RId;
			CustomerCommissionPolicy.PolicyTypeId = 1;
			if (chkIsActive.Checked)
			{
				CustomerCommissionPolicy.IsActive = true;
			}
			else
			{
				CustomerCommissionPolicy.IsActive = false;
			}
			CustomerCommissionPolicy.CommissionRate = Conversion.ToDouble((object)txtCommissionPercentage.Text.Trim());
			val.CustomerCommissionPolicyList.Add(CustomerCommissionPolicy);
			CustomerCommissionPolicy.Save(val);
			if (RId > 0)
			{
				MessageBox.Show("Data Update Successfully....");
			}
			else
			{
				MessageBox.Show("Data Save Successfully....");
			}
			reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "ErrorMessage", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ReadById(int Id)
	{
		try
		{
			RId = Id;
			CustomerCommissionPolicy d = CustomerCommissionPolicy.GetByID(RId);
			if (d != null)
			{
				cmbCustomer.Value = d.CustomerId;
				CmbIncentiveAccount.Value = d.CustomerIncentivePartyId;
				dtpEffectedDate.Value = Conversion.ToDateTime((object)d.EffectiveDate);
				CmbCommissionType.Value = Conversion.ToInt((object)d.CommissionTypeId);
				cmbDiscountType_Leave(null, null);
				txtCommissionPercentage.Text = Conversion.ToString((object)d.CommissionRate);
				CmbCommissionItem.Value = Conversion.ToInt((object)d.CommissionItemsId);
				chkIsActive.Checked = d.IsActive;
				BtnSave.Visible = false;
				BtnEdit.Visible = true;
				dtpEffectedDate.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnEdit_Click(object sender, EventArgs e)
	{
		try
		{
			if (RId == 0)
			{
				throw new Exception("Record not update because Id not found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters DiscountPolicy = new ReportsParameters();
			DiscountPolicy.OrganizationId = UserAccount.OrganizationId;
			DiscountPolicy.CompanyId = UserAccount.CompanyId;
			DiscountPolicy.PolicyTypeId = 1;
			dtGrid = CustomerCommissionPolicy.FormHistory(DiscountPolicy);
			if (dtGrid.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("CustomerId", typeof(int));
				dt.Columns.Add("CustomerName", typeof(string));
				dt.Columns.Add("EffectiveDate", typeof(DateTime));
				dt.Columns.Add("CommissionType", typeof(string));
				dt.Columns.Add("CommissionRate", typeof(decimal));
				dt.Columns.Add("IsActive", typeof(bool));
				dt.Columns.Add("CommissionItemsId", typeof(int));
				dt.Columns.Add("CommissionItemName", typeof(string));
				dt.Columns.Add("CommissionTypeId", typeof(int));
				dt.Columns.Add("CommissionTypeCode", typeof(string));
				dt.Columns.Add("CommissionPerncentage", typeof(decimal));
				dt.Columns.Add("IncentiveAccount", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUserName", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ModifyUserName", typeof(string));
				foreach (DataRow row in dtGrid.Rows)
				{
					dt.Rows.Add(row["Id"], row["CustomerId"], row["CustomerName"], row["EffectiveDate"], row["CommissionType"], row["CommissionRate"], row["IsActive"], row["CommissionItemsId"], row["CommissionItemName"], row["CommissionTypeId"], row["CommissionTypeCode"], row["CommissionPerncentage"], row["IncentiveParty"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"]);
				}
				grd.DataSource = dt;
				grd.RetrieveStructure();
				GrdSetting();
			}
			else
			{
				grd.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdSetting()
	{
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["CustomerId"].Visible = false;
			grd.RootTable.Columns["CommissionItemsId"].Visible = false;
			grd.RootTable.Columns["CommissionTypeId"].Visible = false;
			grd.RootTable.Columns["CommissionPerncentage"].Visible = false;
			grd.RootTable.Columns["CommissionPerncentage"].Caption = "CommissionPercentage";
			grd.RootTable.Columns["CustomerName"].Width = 250;
			grd.RootTable.Columns["IncentiveAccount"].Width = 250;
			grd.RootTable.Columns["CommissionPerncentage"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["CommissionPerncentage"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["CommissionPerncentage"].FormatString = "##,#.###";
			grd.RootTable.Columns["CommissionPerncentage"].TotalFormatString = "##,#.###";
			grd.RootTable.Columns["CommissionRate"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["CommissionRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["CommissionRate"].FormatString = "##,#.###";
			grd.RootTable.Columns["CommissionRate"].TotalFormatString = "##,#.###";
			grd.RootTable.Columns.Add("Edit");
			grd.RootTable.Columns["Edit"].Key = "Edit";
			grd.RootTable.Columns["Edit"].Caption = "Edit";
			grd.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Edit"].ButtonText = "Edit";
			grd.RootTable.Columns["Edit"].Width = 40;
			grd.RootTable.Columns["Edit"].Position = 0;
			grd.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
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

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "Edit")
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

	private void BtnNew_Click(object sender, EventArgs e)
	{
		try
		{
			reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void reset()
	{
		try
		{
			((Control)(object)cmbCustomer).Text = string.Empty;
			((Control)(object)CmbIncentiveAccount).Text = string.Empty;
			((Control)(object)CmbCommissionType).Text = string.Empty;
			((Control)(object)CmbCommissionItem).Text = string.Empty;
			txtCommissionPercentage.Clear();
			BtnSave.Visible = true;
			BtnEdit.Visible = false;
			chkIsActive.Checked = true;
			GridFill();
			dtpEffectedDate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CustomerCommissionPolicy_FormClosing(object sender, FormClosingEventArgs e)
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

	private void BtnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			CommissionTypeFill();
			CustomerBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvfrmPriceSchedule_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (BtnSave.Visible && BtnSave.Enabled && e.Control && e.KeyCode == Keys.S)
			{
				BtnSave_Click(null, null);
			}
			if (BtnEdit.Visible && BtnEdit.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				BtnEdit_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				dtpEffectedDate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				dtpEffectedDate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grd).Focus();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				BtnNew_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
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
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+F5", "For Focus on Discount Item Name");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Discount Item Name");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshortcut_Click(object sender, EventArgs e)
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grd).Name);
			if (fs != null)
			{
				grd.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void txtDiscountRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void btnAllocateBrandToType_Click(object sender, EventArgs e)
	{
		try
		{
			new AllocateBrandItemToCommissionType(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAllocateDiscountItemToType_Click(object sender, EventArgs e)
	{
		try
		{
			new AllocateCommissionItemToCommissionType(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void chkIsActive_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name != null && base.ActiveControl.Name == "grd")
			{
				dtpEffectedDate.Focus();
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
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Expected O, but got Unknown
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected O, but got Unknown
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Expected O, but got Unknown
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Expected O, but got Unknown
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Expected O, but got Unknown
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Expected O, but got Unknown
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Expected O, but got Unknown
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Expected O, but got Unknown
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Expected O, but got Unknown
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Expected O, but got Unknown
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Expected O, but got Unknown
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Expected O, but got Unknown
		//IL_0595: Unknown result type (might be due to invalid IL or missing references)
		//IL_059f: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.WholeSale.Commission.CustomerCommissionPolicy));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label5 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.CmbIncentiveAccount = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.CmbCommissionItem = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.cmbCustomer = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtCommissionPercentage = new System.Windows.Forms.TextBox();
		this.chkIsActive = new System.Windows.Forms.CheckBox();
		this.label4 = new System.Windows.Forms.Label();
		this.CmbCommissionType = new UltraCombo();
		this.dtpEffectedDate = new System.Windows.Forms.DateTimePicker();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label13 = new System.Windows.Forms.Label();
		this.tsPriceSchedule = new System.Windows.Forms.ToolStrip();
		this.BtnNew = new System.Windows.Forms.ToolStripButton();
		this.BtnRefresh = new System.Windows.Forms.ToolStripButton();
		this.BtnSave = new System.Windows.Forms.ToolStripButton();
		this.BtnEdit = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnAllocateBrandToType = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnAllocateDiscountItemToType = new System.Windows.Forms.ToolStripButton();
		this.btnshortcut = new System.Windows.Forms.ToolStripButton();
		this.miniToolStrip = new System.Windows.Forms.BindingNavigator(this.components);
		this.panel1.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel6.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbIncentiveAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionType).BeginInit();
		this.panel2.SuspendLayout();
		this.tsPriceSchedule.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.miniToolStrip).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1042, 694);
		this.panel1.TabIndex = 0;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 121);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1042, 573);
		this.panel5.TabIndex = 3;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 27);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1042, 546);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Controls.Add(this.label5);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1042, 27);
		this.panel6.TabIndex = 2;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(996, -1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(43, 28);
		this.ctrlGrdBar1.TabIndex = 0;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label5.AutoSize = true;
		this.label5.BackColor = System.Drawing.Color.Teal;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(8, 3);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(67, 19);
		this.label5.TabIndex = 0;
		this.label5.Text = "History";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbIncentiveAccount);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommissionItem);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCustomer);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add(this.txtCommissionPercentage);
		this.panel4.Controls.Add(this.chkIsActive);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommissionType);
		this.panel4.Controls.Add(this.dtpEffectedDate);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 55);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1042, 66);
		this.panel4.TabIndex = 2;
		this.CmbIncentiveAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbIncentiveAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbIncentiveAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbIncentiveAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbIncentiveAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbIncentiveAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbIncentiveAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbIncentiveAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbIncentiveAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbIncentiveAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbIncentiveAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbIncentiveAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbIncentiveAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbIncentiveAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbIncentiveAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbIncentiveAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbIncentiveAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbIncentiveAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbIncentiveAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbIncentiveAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbIncentiveAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbIncentiveAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbIncentiveAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbIncentiveAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbIncentiveAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbIncentiveAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbIncentiveAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbIncentiveAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbIncentiveAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbIncentiveAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbIncentiveAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbIncentiveAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbIncentiveAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbIncentiveAccount).Location = new System.Drawing.Point(765, 34);
		((System.Windows.Forms.Control)(object)this.CmbIncentiveAccount).Name = "CmbIncentiveAccount";
		((System.Windows.Forms.Control)(object)this.CmbIncentiveAccount).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbIncentiveAccount).TabIndex = 6;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(654, 40);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(104, 15);
		this.label7.TabIndex = 23;
		this.label7.Text = "Incentive Account";
		this.CmbCommissionItem.AllowNull = (DefaultableBoolean)1;
		this.CmbCommissionItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommissionItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommissionItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommissionItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbCommissionItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommissionItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbCommissionItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommissionItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommissionItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommissionItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbCommissionItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommissionItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommissionItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbCommissionItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommissionItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommissionItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbCommissionItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommissionItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommissionItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbCommissionItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommissionItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbCommissionItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommissionItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommissionItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommissionItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCommissionItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommissionItem).Location = new System.Drawing.Point(427, 34);
		((System.Windows.Forms.Control)(object)this.CmbCommissionItem).Name = "CmbCommissionItem";
		((System.Windows.Forms.Control)(object)this.CmbCommissionItem).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommissionItem).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbCommissionItem).TextChanged += new System.EventHandler(cmbDiscItem_TextChanged);
		((System.Windows.Forms.Control)(object)this.CmbCommissionItem).Leave += new System.EventHandler(cmbDiscItem_Leave);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(324, 40);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(101, 15);
		this.label1.TabIndex = 21;
		this.label1.Text = "Commission Item";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(7, 11);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(78, 15);
		this.label6.TabIndex = 17;
		this.label6.Text = "Effected Date";
		this.cmbCustomer.AllowNull = (DefaultableBoolean)1;
		this.cmbCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCustomer).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCustomer).Location = new System.Drawing.Point(102, 34);
		((System.Windows.Forms.Control)(object)this.cmbCustomer).Name = "cmbCustomer";
		((System.Windows.Forms.Control)(object)this.cmbCustomer).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.cmbCustomer).TabIndex = 1;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(324, 11);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(101, 15);
		this.label3.TabIndex = 17;
		this.label3.Text = "Commission Type";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(7, 40);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(93, 15);
		this.label2.TabIndex = 19;
		this.label2.Text = "Customer Name";
		this.txtCommissionPercentage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommissionPercentage.Enabled = false;
		this.txtCommissionPercentage.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommissionPercentage.Location = new System.Drawing.Point(765, 7);
		this.txtCommissionPercentage.Name = "txtCommissionPercentage";
		this.txtCommissionPercentage.Size = new System.Drawing.Size(143, 23);
		this.txtCommissionPercentage.TabIndex = 4;
		this.txtCommissionPercentage.TabStop = false;
		this.txtCommissionPercentage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDiscountRate_KeyPress);
		this.chkIsActive.AutoSize = true;
		this.chkIsActive.Checked = true;
		this.chkIsActive.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkIsActive.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkIsActive.Location = new System.Drawing.Point(914, 9);
		this.chkIsActive.Name = "chkIsActive";
		this.chkIsActive.Size = new System.Drawing.Size(71, 19);
		this.chkIsActive.TabIndex = 5;
		this.chkIsActive.Text = "Is Active";
		this.chkIsActive.UseVisualStyleBackColor = true;
		this.chkIsActive.Leave += new System.EventHandler(chkIsActive_Leave);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(654, 11);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(86, 15);
		this.label4.TabIndex = 15;
		this.label4.Text = "Commission %";
		this.CmbCommissionType.AllowNull = (DefaultableBoolean)1;
		this.CmbCommissionType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommissionType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommissionType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommissionType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCommissionType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommissionType).Location = new System.Drawing.Point(427, 5);
		((System.Windows.Forms.Control)(object)this.CmbCommissionType).Name = "CmbCommissionType";
		((System.Windows.Forms.Control)(object)this.CmbCommissionType).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommissionType).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbCommissionType).Leave += new System.EventHandler(cmbDiscountType_Leave);
		this.dtpEffectedDate.CustomFormat = "dd-MMM-yyyy";
		this.dtpEffectedDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.dtpEffectedDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.dtpEffectedDate.Location = new System.Drawing.Point(102, 7);
		this.dtpEffectedDate.Name = "dtpEffectedDate";
		this.dtpEffectedDate.Size = new System.Drawing.Size(220, 23);
		this.dtpEffectedDate.TabIndex = 0;
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label13);
		this.panel2.Controls.Add(this.tsPriceSchedule);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1042, 55);
		this.panel2.TabIndex = 0;
		this.label13.AutoSize = true;
		this.label13.BackColor = System.Drawing.Color.Teal;
		this.label13.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(2, 29);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(242, 19);
		this.label13.TabIndex = 0;
		this.label13.Text = "Customer Commission Policy";
		this.tsPriceSchedule.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsPriceSchedule.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.BtnNew, this.BtnRefresh, this.BtnSave, this.BtnEdit, this.toolStripSeparator1, this.btnAllocateBrandToType, this.toolStripSeparator2, this.btnAllocateDiscountItemToType, this.btnshortcut });
		this.tsPriceSchedule.Location = new System.Drawing.Point(0, 0);
		this.tsPriceSchedule.Name = "tsPriceSchedule";
		this.tsPriceSchedule.Size = new System.Drawing.Size(1042, 27);
		this.tsPriceSchedule.TabIndex = 3;
		this.tsPriceSchedule.Text = "toolStrip1";
		this.BtnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNew.Name = "BtnNew";
		this.BtnNew.Size = new System.Drawing.Size(59, 24);
		this.BtnNew.Text = "&New";
		this.BtnNew.Click += new System.EventHandler(BtnNew_Click);
		this.BtnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefresh.Name = "BtnRefresh";
		this.BtnRefresh.Size = new System.Drawing.Size(80, 24);
		this.BtnRefresh.Text = "&Refresh";
		this.BtnRefresh.Click += new System.EventHandler(BtnRefresh_Click);
		this.BtnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnSave.Image = (System.Drawing.Image)resources.GetObject("BtnSave.Image");
		this.BtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnSave.Name = "BtnSave";
		this.BtnSave.Size = new System.Drawing.Size(64, 24);
		this.BtnSave.Text = "&Save";
		this.BtnSave.Click += new System.EventHandler(BtnSave_Click);
		this.BtnEdit.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnEdit.Image = (System.Drawing.Image)resources.GetObject("BtnEdit.Image");
		this.BtnEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnEdit.Name = "BtnEdit";
		this.BtnEdit.Size = new System.Drawing.Size(78, 24);
		this.BtnEdit.Text = "&Update";
		this.BtnEdit.Click += new System.EventHandler(BtnEdit_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnAllocateBrandToType.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnAllocateBrandToType.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAllocateBrandToType.Image = (System.Drawing.Image)resources.GetObject("btnAllocateBrandToType.Image");
		this.btnAllocateBrandToType.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAllocateBrandToType.Name = "btnAllocateBrandToType";
		this.btnAllocateBrandToType.Size = new System.Drawing.Size(244, 24);
		this.btnAllocateBrandToType.Text = "Allocate Brand To Commission Type";
		this.btnAllocateBrandToType.Click += new System.EventHandler(btnAllocateBrandToType_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnAllocateDiscountItemToType.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnAllocateDiscountItemToType.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAllocateDiscountItemToType.Image = (System.Drawing.Image)resources.GetObject("btnAllocateDiscountItemToType.Image");
		this.btnAllocateDiscountItemToType.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAllocateDiscountItemToType.Name = "btnAllocateDiscountItemToType";
		this.btnAllocateDiscountItemToType.Size = new System.Drawing.Size(317, 24);
		this.btnAllocateDiscountItemToType.Text = "Allocate Commission Item To Commission Type";
		this.btnAllocateDiscountItemToType.Click += new System.EventHandler(btnAllocateDiscountItemToType_Click);
		this.btnshortcut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcut.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcut.Name = "btnshortcut";
		this.btnshortcut.Size = new System.Drawing.Size(126, 24);
		this.btnshortcut.Text = "ShortCut Keys";
		this.btnshortcut.Click += new System.EventHandler(btnshortcut_Click);
		this.miniToolStrip.AccessibleName = "New item selection";
		this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
		this.miniToolStrip.AddNewItem = null;
		this.miniToolStrip.AutoSize = false;
		this.miniToolStrip.CanOverflow = false;
		this.miniToolStrip.CountItem = null;
		this.miniToolStrip.DeleteItem = null;
		this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.miniToolStrip.Location = new System.Drawing.Point(283, 0);
		this.miniToolStrip.MoveFirstItem = null;
		this.miniToolStrip.MoveLastItem = null;
		this.miniToolStrip.MoveNextItem = null;
		this.miniToolStrip.MovePreviousItem = null;
		this.miniToolStrip.Name = "miniToolStrip";
		this.miniToolStrip.PositionItem = null;
		this.miniToolStrip.Size = new System.Drawing.Size(1623, 27);
		this.miniToolStrip.TabIndex = 7;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1042, 694);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "CustomerCommissionPolicy";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(CustomerCommissionPolicy_FormClosing);
		base.Load += new System.EventHandler(InvfrmPriceSchedule_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvfrmPriceSchedule_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbIncentiveAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomer).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionType).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.tsPriceSchedule.ResumeLayout(false);
		this.tsPriceSchedule.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.miniToolStrip).EndInit();
		base.ResumeLayout(false);
	}
}
