using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Lab;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Lab;
using Architecture.WinApp.Common;
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
namespace Architecture.WinApp.Lab
{

public class InvLabStandardPolicyForDeduction : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable table = new DataTable();

	private bool Approved;

	private int updateDetailIndex;

	private bool UpdateMode;

	public static int RecId;

	public static int Id;

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabForm;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grd;

	private Panel panel1;

	private Panel panel7;

	internal UltraCombo cmbitem;

	private CheckBox chkIsaccepted;

	private TextBox txtremarks;

	private Label label4;

	private Label label8;

	private Label label7;

	private Label label1;

	private TabPage tabHistory;

	private Panel panel4;

	internal GridEX grdhistory;

	private DateTimePicker txtAffectiveDate;

	private Label label2;

	private DateTimePicker TxtAffectiveDateTo;

	internal UltraCombo cmbparameter;

	private Label label3;

	internal UltraCombo cmbweight;

	private Label label12;

	private TextBox TxtDecValue;

	private Label label11;

	internal UltraCombo cmbdeduction;

	private Label label10;

	private TextBox txtrangeTo;

	private Label label9;

	private TextBox txtRangeFrom;

	private Label label5;

	private Button btnAdd;

	private ToolStrip btn;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton btnprint;

	private ToolStripButton btnattachment;

	private ToolStripSeparator toolStripSeparator;

	internal UltraCombo cmbApplyOn;

	private Panel panel8;

	private Label label14;

	private Label label13;

	private Label label6;

	private Label label30;

	private Label label17;

	private Label label16;

	private Label label15;

	private Label label18;

	private Label label20;

	private Label label19;

	private Button btnCancelDetail;

	private Button btnUpdateDetail;

	private Label label21;

	private Panel panel2;

	public InvLabStandardPolicyForDeduction(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void Lab_Load(object sender, EventArgs e)
	{
		try
		{
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			btnUpdate.Visible = false;
			btnSave.Visible = true;
			cmbitem.Focus();
			ApplyOnFill();
			ItemBind();
			LabItemPerameterFill();
			DeductionTypeFill();
			UOM();
			table.Columns.Add("PerameterId");
			table.Columns.Add("AnalysisPerameter");
			table.Columns.Add("RangeFrom", typeof(double));
			table.Columns.Add("RangeTo", typeof(double));
			table.Columns.Add("DeductionOn");
			table.Columns.Add("DeductionValue", typeof(double));
			table.Columns.Add("Weight");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemBind()
	{
		try
		{
			DataTable dtItem = CommonServices.ItemGetForComboServiceBind();
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDL(dtItem, cmbitem, "Id", "ItemName", "Item Name", ZeroIndex: false);
				((UltraGridBase)cmbitem).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LabItemPerameterFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		try
		{
			InvLabAnalysisItems val = new InvLabAnalysisItems
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtgetAllItem = new DataTable();
			dtgetAllItem = InvLabAnalysisItems.GetAllOrById(val);
			if (dtgetAllItem.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtgetAllItem, cmbparameter, "Id", "AnalysisParameterDescription", "Analysis Perameter", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ApplyOnFill()
	{
		try
		{
			DataTable dt = CommonServices.StaticColumnsService("LabApplyOn");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbApplyOn, "Id", "Type", "ApplyOn", ZeroIndex: false);
				((UltraGridBase)cmbApplyOn).Rows[0].Activate();
			}
			else
			{
				((UltraGridBase)cmbApplyOn).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeductionTypeFill()
	{
		try
		{
			DataTable dt = CommonServices.StaticColumnsService("LabDeductionOn");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbdeduction, "Id", "type", "DeductionOn", ZeroIndex: false);
			}
			else
			{
				((UltraGridBase)cmbdeduction).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UOM()
	{
		try
		{
			DataTable dtUOM = CommonServices.StaticColumnsService("GetUom");
			if (dtUOM.Rows.Count > 0)
			{
				DDL.BindDDL(dtUOM, cmbweight, "Id", "type", "Weight Kg", ZeroIndex: false);
			}
			else
			{
				((UltraGridBase)cmbweight).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Add_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationDetail())
			{
				return;
			}
			if (grd.GetRows().Length != 0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (Conversion.ToInt(item.Cells["PerameterId"].Value) == Conversion.ToInt(cmbparameter.Value) && Conversion.ToString(item.Cells["DeductionOn"].Value) != Conversion.ToString((object)((Control)(object)cmbdeduction).Text.Trim()))
					{
						throw new Exception("This parameter deduction on must be same please check");
					}
					if (Conversion.ToInt(item.Cells["PerameterId"].Value) == Conversion.ToInt(cmbparameter.Value))
					{
						if (IsBetween(Conversion.ToDouble((object)txtRangeFrom.Text.Trim()), Conversion.ToDouble(item.Cells["RangeFrom"].Value), Conversion.ToDouble(item.Cells["RangeTo"].Value)))
						{
							throw new Exception("Exist");
						}
						if (IsBetween(Conversion.ToDouble((object)txtrangeTo.Text.Trim()), Conversion.ToDouble(item.Cells["RangeFrom"].Value), Conversion.ToDouble(item.Cells["RangeTo"].Value)))
						{
							throw new Exception("Exist");
						}
					}
				}
			}
			table.Rows.Add(cmbparameter.Value, ((Control)(object)cmbparameter).Text.Trim(), txtRangeFrom.Text.Trim(), txtrangeTo.Text.Trim(), ((Control)(object)cmbdeduction).Text.Trim(), TxtDecValue.Text.Trim(), ((Control)(object)cmbweight).Text.Trim());
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			((Control)(object)cmbparameter).Text = string.Empty;
			txtRangeFrom.Text = string.Empty;
			txtrangeTo.Text = string.Empty;
			((Control)(object)cmbdeduction).Text = string.Empty;
			TxtDecValue.Text = string.Empty;
			((Control)(object)cmbweight).Text = string.Empty;
			cmbparameter.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public bool IsBetween(double item, double start, double end)
	{
		bool status = false;
		try
		{
			status = item >= start || item <= end;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return status;
	}

	private void grdSettings()
	{
		try
		{
			grd.AllowEdit = (InheritableBoolean)2;
			grd.RootTable.Columns["PerameterId"].Visible = false;
			grd.ColumnAutoResize = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			table.Rows[updateDetailIndex]["PerameterId"] = cmbparameter.Value;
			table.Rows[updateDetailIndex]["AnalysisPerameter"] = ((Control)(object)cmbparameter).Text;
			table.Rows[updateDetailIndex]["RangeFrom"] = txtRangeFrom.Text;
			table.Rows[updateDetailIndex]["RangeTo"] = txtrangeTo.Text;
			table.Rows[updateDetailIndex]["DeductionOn"] = ((Control)(object)cmbdeduction).Text;
			table.Rows[updateDetailIndex]["DeductionValue"] = TxtDecValue.Text;
			table.Rows[updateDetailIndex]["Weight"] = ((Control)(object)cmbweight).Text;
			grd.DataSource = table;
			grd.RetrieveStructure();
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			DetailReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelDetail_Click(object sender, EventArgs e)
	{
		try
		{
			DetailReset();
			btnUpdateDetail.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				cmbparameter.Value = item.Cells["PerameterId"].Value.ToString();
				((Control)(object)cmbparameter).Text = item.Cells["AnalysisPerameter"].Value.ToString();
				txtRangeFrom.Text = item.Cells["RangeFrom"].Value.ToString();
				txtrangeTo.Text = item.Cells["RangeTo"].Value.ToString();
				((Control)(object)cmbdeduction).Text = item.Cells["DeductionOn"].Value.ToString();
				TxtDecValue.Text = item.Cells["DeductionValue"].Value.ToString();
				((Control)(object)cmbweight).Text = item.Cells["Weight"].Value.ToString();
				btnAdd.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelDetail.Visible = true;
				cmbparameter.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)cmbApplyOn).ActiveRow == null)
		{
			MessageBox.Show("ApplyOn field is required");
			cmbApplyOn.Focus();
			return false;
		}
		if (((UltraGridBase)cmbitem).ActiveRow == null)
		{
			MessageBox.Show("ItemName Field is Required");
			cmbitem.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)cmbparameter).ActiveRow == null)
		{
			MessageBox.Show("Parameter Field is Required");
			cmbparameter.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtRangeFrom.Text.Trim()) >= Conversion.ToDouble((object)txtrangeTo.Text.Trim()))
		{
			MessageBox.Show("Range From Must Be less Then Range To Thank You");
			txtRangeFrom.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtRangeFrom.Text.Trim()) <= 0.0)
		{
			MessageBox.Show("Range From Field is Required");
			txtRangeFrom.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtrangeTo.Text.Trim()) <= 0.0)
		{
			MessageBox.Show("Range To Field is Required");
			txtrangeTo.Focus();
			return false;
		}
		if (((UltraGridBase)cmbdeduction).ActiveRow == null)
		{
			MessageBox.Show("Deduction On Field is Required");
			cmbdeduction.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)TxtDecValue.Text) <= 0.0)
		{
			MessageBox.Show("Deduction Value Field is Required");
			cmbdeduction.Focus();
			return false;
		}
		if (((UltraGridBase)cmbweight).ActiveRow == null)
		{
			MessageBox.Show("Weight Kg Field is Required");
			cmbweight.Focus();
			return false;
		}
		return true;
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("Recod cannot be updated because RecId not found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Expected O, but got Unknown
		try
		{
			if (grd.GetRows().Count() == 0)
			{
				throw new Exception("Grid Record Not Found");
			}
			if (!FormValidation())
			{
				return;
			}
			InvLabAnalysisStandardDeductionPolicyHeader objheader = new InvLabAnalysisStandardDeductionPolicyHeader();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				objheader.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			objheader.PolicyApplyOn = Conversion.ToString((object)((Control)(object)cmbApplyOn).Text);
			objheader.ItemId = Conversion.ToInt(cmbitem.Value);
			objheader.EffectiveFrom = Conversion.ToDateTime((object)txtAffectiveDate.Value);
			objheader.EfffectiveTo = Conversion.ToDateTime((object)TxtAffectiveDateTo.Value);
			objheader.RemarksHeader = Conversion.ToString((object)txtremarks.Text);
			objheader.CompanyId = UserAccount.CompanyId;
			objheader.OrganizationId = UserAccount.OrganizationId;
			objheader.EntryDate = DateTime.Now;
			objheader.EntryUserId = UserAccount.ID;
			objheader.ModifyDate = DateTime.Now;
			objheader.ApprovedDate = DateTime.Now;
			objheader.LabAnalysisStandardDetail = new List<InvLabAnalysisStandardDeductionPolicyDetail>();
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				InvLabAnalysisStandardDeductionPolicyDetail labdetail = new InvLabAnalysisStandardDeductionPolicyDetail();
				labdetail.AnalysisParameterId = Conversion.ToInt(r.Cells["PerameterId"].Value);
				labdetail.RangeFrom = Conversion.ToDecimal(r.Cells["RangeFrom"].Value);
				labdetail.RangeTo = Conversion.ToDecimal((object)r.Cells["RangeTo"].Text);
				labdetail.DeductFrom = Conversion.ToString((object)r.Cells["DeductionOn"].Text);
				labdetail.DedValue = Conversion.ToDecimal((object)r.Cells["DeductionValue"].Text);
				labdetail.WeightKg = Conversion.ToDouble((object)r.Cells["Weight"].Text);
				objheader.LabAnalysisStandardDetail.Add(labdetail);
			}
			InvLabAnalysisStandardDeductionPolicyHeader.Save(objheader);
			if (RecId > 0)
			{
				MessageBox.Show("Update SuccessFully");
			}
			else
			{
				MessageBox.Show("Save SuccessFully");
			}
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		try
		{
			Approved = false;
			RecId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void getUpdate(int Id)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			RecId = Conversion.ToInt((object)Id);
			InvLabAnalysisStandardDeductionPolicyHeader lab = new InvLabAnalysisStandardDeductionPolicyHeader();
			lab = InvLabAnalysisStandardDeductionPolicyHeader.GetById(Id);
			if (lab != null)
			{
				btnSave.Visible = false;
				btnUpdate.Visible = true;
				tabControl1.SelectedIndex = 0;
				((Control)(object)cmbApplyOn).Text = Conversion.ToString((object)lab.PolicyApplyOn);
				cmbitem.Value = Conversion.ToInt((object)lab.ItemId);
				txtAffectiveDate.Value = Conversion.ToDateTime((object)lab.EffectiveFrom);
				TxtAffectiveDateTo.Value = Conversion.ToDateTime((object)lab.EfffectiveTo);
				txtremarks.Text = Conversion.ToString((object)lab.RemarksHeader);
				table.Rows.Clear();
				for (int i = 0; i < lab.LabAnalysisStandardDetail.Count; i++)
				{
					table.Rows.Add(lab.LabAnalysisStandardDetail[i].AnalysisParameterId, lab.LabAnalysisStandardDetail[i].AnalysisParameterDescription, lab.LabAnalysisStandardDetail[i].RangeFrom, lab.LabAnalysisStandardDetail[i].RangeTo, lab.LabAnalysisStandardDetail[i].DeductFrom, lab.LabAnalysisStandardDetail[i].DedValue, lab.LabAnalysisStandardDetail[i].WeightKg);
				}
				grd.DataSource = table;
				grd.RetrieveStructure();
				grdSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void DetailReset()
	{
		try
		{
			cmbparameter.Focus();
			((Control)(object)cmbparameter).Text = string.Empty;
			txtRangeFrom.Text = string.Empty;
			txtrangeTo.Text = string.Empty;
			((Control)(object)cmbdeduction).Text = string.Empty;
			TxtDecValue.Text = string.Empty;
			((Control)(object)cmbweight).Text = string.Empty;
			btnUpdateDetail.Visible = false;
			btnAdd.Visible = true;
			btnCancelDetail.Visible = false;
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
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			table.Rows.Clear();
			grd.ClearStructure();
			cmbitem.Focus();
			((Control)(object)cmbitem).Text = string.Empty;
			txtremarks.Text = string.Empty;
			UpdateMode = false;
			RecId = 0;
			DetailReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		try
		{
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRangeFrom_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextdecimelFunction(sender, e);
	}

	private void txtrangeTo_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextdecimelFunction(sender, e);
	}

	private void TxtDecValue_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextdecimelFunction(sender, e);
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			gridhistoryfill();
		}
	}

	private void gridhistoryfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = InvLabAnalysisStandardDeductionPolicyHeader.GetAll(new InvLabAnalysisStandardDeductionPolicyHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				grdhistory.DataSource = dt;
				grdhistory.RetrieveStructure();
				grdhistory.RootTable.Columns["Id"].Visible = false;
				grdhistory.ColumnAutoResize = true;
				grdhistory.AllowDelete = (InheritableBoolean)2;
				grdhistory.AllowEdit = (InheritableBoolean)2;
			}
			else
			{
				grdhistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			Id = Conversion.ToInt((object)grdhistory.CurrentRow.Cells[0].Value.ToString());
			getUpdate(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnattachment_Click(object sender, EventArgs e)
	{
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
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Expected O, but got Unknown
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Expected O, but got Unknown
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Expected O, but got Unknown
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Expected O, but got Unknown
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Expected O, but got Unknown
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Expected O, but got Unknown
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f5: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance45 = new Appearance();
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabForm = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.btnAdd = new System.Windows.Forms.Button();
		this.btnCancelDetail = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.label20 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.cmbdeduction = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.txtRangeFrom = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.txtrangeTo = new System.Windows.Forms.TextBox();
		this.TxtDecValue = new System.Windows.Forms.TextBox();
		this.cmbparameter = new UltraCombo();
		this.cmbweight = new UltraCombo();
		this.panel7 = new System.Windows.Forms.Panel();
		this.cmbApplyOn = new UltraCombo();
		this.cmbitem = new UltraCombo();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.TxtAffectiveDateTo = new System.Windows.Forms.DateTimePicker();
		this.txtAffectiveDate = new System.Windows.Forms.DateTimePicker();
		this.chkIsaccepted = new System.Windows.Forms.CheckBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.btn = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.label21 = new System.Windows.Forms.Label();
		this.tabHistory = new System.Windows.Forms.TabPage();
		this.grdhistory = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.tabControl1.SuspendLayout();
		this.tabForm.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbdeduction).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbparameter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbweight).BeginInit();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbApplyOn).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).BeginInit();
		this.panel1.SuspendLayout();
		this.btn.SuspendLayout();
		this.tabHistory.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabForm);
		this.tabControl1.Controls.Add(this.tabHistory);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(742, 428);
		this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
		this.tabControl1.TabIndex = 1;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabForm.BackColor = System.Drawing.Color.White;
		this.tabForm.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabForm.Controls.Add(this.panel2);
		this.tabForm.Controls.Add(this.panel8);
		this.tabForm.Controls.Add(this.panel7);
		this.tabForm.Controls.Add(this.panel1);
		this.tabForm.Location = new System.Drawing.Point(4, 4);
		this.tabForm.Name = "tabForm";
		this.tabForm.Padding = new System.Windows.Forms.Padding(3);
		this.tabForm.Size = new System.Drawing.Size(734, 399);
		this.tabForm.TabIndex = 0;
		this.tabForm.Text = "Form";
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 224);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(728, 172);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 2;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 203);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(728, 21);
		this.panel2.TabIndex = 81;
		this.panel8.BackColor = System.Drawing.Color.White;
		this.panel8.Controls.Add(this.btnAdd);
		this.panel8.Controls.Add(this.btnCancelDetail);
		this.panel8.Controls.Add(this.btnUpdateDetail);
		this.panel8.Controls.Add(this.label20);
		this.panel8.Controls.Add(this.label19);
		this.panel8.Controls.Add(this.label18);
		this.panel8.Controls.Add(this.label17);
		this.panel8.Controls.Add(this.label16);
		this.panel8.Controls.Add(this.label15);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.cmbdeduction);
		this.panel8.Controls.Add(this.label12);
		this.panel8.Controls.Add(this.label5);
		this.panel8.Controls.Add(this.label11);
		this.panel8.Controls.Add(this.txtRangeFrom);
		this.panel8.Controls.Add(this.label3);
		this.panel8.Controls.Add(this.label9);
		this.panel8.Controls.Add(this.label10);
		this.panel8.Controls.Add(this.txtrangeTo);
		this.panel8.Controls.Add(this.TxtDecValue);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.cmbparameter);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.cmbweight);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 146);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(728, 57);
		this.panel8.TabIndex = 1;
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(640, 27);
		this.btnAdd.Margin = new System.Windows.Forms.Padding(2);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(40, 24);
		this.btnAdd.TabIndex = 6;
		this.btnAdd.Text = "+";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(Add_Click);
		this.btnCancelDetail.BackColor = System.Drawing.Color.Teal;
		this.btnCancelDetail.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.btnCancelDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelDetail.ForeColor = System.Drawing.Color.White;
		this.btnCancelDetail.Location = new System.Drawing.Point(640, 4);
		this.btnCancelDetail.Name = "btnCancelDetail";
		this.btnCancelDetail.Size = new System.Drawing.Size(83, 22);
		this.btnCancelDetail.TabIndex = 8;
		this.btnCancelDetail.Text = "Cancel";
		this.btnCancelDetail.UseVisualStyleBackColor = false;
		this.btnCancelDetail.Visible = false;
		this.btnCancelDetail.Click += new System.EventHandler(btnCancelDetail_Click);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(640, 28);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(83, 22);
		this.btnUpdateDetail.TabIndex = 7;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.ForeColor = System.Drawing.Color.Red;
		this.label20.Location = new System.Drawing.Point(623, 7);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(14, 17);
		this.label20.TabIndex = 90;
		this.label20.Text = "*";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.Red;
		this.label19.Location = new System.Drawing.Point(531, 7);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(14, 17);
		this.label19.TabIndex = 89;
		this.label19.Text = "*";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.Color.Red;
		this.label18.Location = new System.Drawing.Point(414, 7);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(14, 17);
		this.label18.TabIndex = 88;
		this.label18.Text = "*";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.Red;
		this.label17.Location = new System.Drawing.Point(318, 7);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(14, 17);
		this.label17.TabIndex = 87;
		this.label17.Text = "*";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.Red;
		this.label16.Location = new System.Drawing.Point(239, 7);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(14, 17);
		this.label16.TabIndex = 86;
		this.label16.Text = "*";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.Red;
		this.label15.Location = new System.Drawing.Point(113, 7);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(14, 17);
		this.label15.TabIndex = 85;
		this.label15.Text = "*";
		this.cmbdeduction.AllowNull = (DefaultableBoolean)1;
		this.cmbdeduction.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbdeduction.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbdeduction.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbdeduction).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbdeduction).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbdeduction).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbdeduction).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdeduction).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbdeduction).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdeduction).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbdeduction).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbdeduction).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbdeduction).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbdeduction).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbdeduction).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbdeduction).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdeduction).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance24).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbdeduction).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbdeduction).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbdeduction).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance25).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdeduction).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbdeduction).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbdeduction).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbdeduction).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbdeduction).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbdeduction).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbdeduction).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbdeduction).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbdeduction).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbdeduction).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbdeduction).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbdeduction.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbdeduction).Location = new System.Drawing.Point(334, 26);
		((System.Windows.Forms.Control)(object)this.cmbdeduction).Name = "cmbdeduction";
		((System.Windows.Forms.Control)(object)this.cmbdeduction).Size = new System.Drawing.Size(94, 26);
		((System.Windows.Forms.Control)(object)this.cmbdeduction).TabIndex = 3;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(547, 8);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(63, 15);
		this.label12.TabIndex = 50;
		this.label12.Text = "Weight Kg";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(170, 8);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(71, 15);
		this.label5.TabIndex = 45;
		this.label5.Text = "Range From";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(430, 8);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(95, 15);
		this.label11.TabIndex = 49;
		this.label11.Text = "Deduction Value";
		this.txtRangeFrom.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRangeFrom.Location = new System.Drawing.Point(170, 28);
		this.txtRangeFrom.Name = "txtRangeFrom";
		this.txtRangeFrom.Size = new System.Drawing.Size(83, 23);
		this.txtRangeFrom.TabIndex = 1;
		this.txtRangeFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRangeFrom_KeyPress);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(5, 8);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(107, 15);
		this.label3.TabIndex = 1;
		this.label3.Text = "Analysis Parameter";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(256, 8);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(56, 15);
		this.label9.TabIndex = 47;
		this.label9.Text = "Range To";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(334, 8);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(82, 15);
		this.label10.TabIndex = 11;
		this.label10.Text = "Deduction On";
		this.txtrangeTo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtrangeTo.Location = new System.Drawing.Point(256, 28);
		this.txtrangeTo.Name = "txtrangeTo";
		this.txtrangeTo.Size = new System.Drawing.Size(76, 23);
		this.txtrangeTo.TabIndex = 2;
		this.txtrangeTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtrangeTo_KeyPress);
		this.TxtDecValue.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.TxtDecValue.Location = new System.Drawing.Point(430, 28);
		this.TxtDecValue.Name = "TxtDecValue";
		this.TxtDecValue.Size = new System.Drawing.Size(115, 23);
		this.TxtDecValue.TabIndex = 4;
		this.TxtDecValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(TxtDecValue_KeyPress);
		this.cmbparameter.AllowNull = (DefaultableBoolean)1;
		this.cmbparameter.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbparameter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbparameter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbparameter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbparameter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbparameter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbparameter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbparameter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance48;
		((SpecialBoxBase)((UltraGridBase)this.cmbparameter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbparameter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbparameter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbparameter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbparameter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbparameter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbparameter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbparameter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbparameter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance21).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbparameter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbparameter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbparameter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance23).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbparameter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance34).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbparameter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbparameter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbparameter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbparameter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbparameter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbparameter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbparameter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbparameter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbparameter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbparameter).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbparameter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbparameter).Location = new System.Drawing.Point(5, 26);
		((System.Windows.Forms.Control)(object)this.cmbparameter).Name = "cmbparameter";
		((System.Windows.Forms.Control)(object)this.cmbparameter).Size = new System.Drawing.Size(162, 26);
		((System.Windows.Forms.Control)(object)this.cmbparameter).TabIndex = 0;
		this.cmbweight.AllowNull = (DefaultableBoolean)1;
		this.cmbweight.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbweight.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbweight.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbweight).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbweight).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbweight).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbweight).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbweight).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance11;
		((SpecialBoxBase)((UltraGridBase)this.cmbweight).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbweight).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbweight).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbweight).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbweight).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbweight).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbweight).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbweight).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbweight).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance59).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbweight).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbweight).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbweight).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbweight).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbweight).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbweight).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbweight).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbweight).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbweight).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbweight).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbweight).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbweight).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbweight).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbweight).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbweight.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbweight).Location = new System.Drawing.Point(547, 26);
		((System.Windows.Forms.Control)(object)this.cmbweight).Name = "cmbweight";
		((System.Windows.Forms.Control)(object)this.cmbweight).Size = new System.Drawing.Size(90, 26);
		((System.Windows.Forms.Control)(object)this.cmbweight).TabIndex = 5;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbApplyOn);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbitem);
		this.panel7.Controls.Add(this.txtremarks);
		this.panel7.Controls.Add(this.label14);
		this.panel7.Controls.Add(this.label13);
		this.panel7.Controls.Add(this.label6);
		this.panel7.Controls.Add(this.label30);
		this.panel7.Controls.Add(this.label2);
		this.panel7.Controls.Add(this.TxtAffectiveDateTo);
		this.panel7.Controls.Add(this.txtAffectiveDate);
		this.panel7.Controls.Add(this.chkIsaccepted);
		this.panel7.Controls.Add(this.label4);
		this.panel7.Controls.Add(this.label8);
		this.panel7.Controls.Add(this.label7);
		this.panel7.Controls.Add(this.label1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.panel7.Location = new System.Drawing.Point(3, 56);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(728, 90);
		this.panel7.TabIndex = 0;
		this.cmbApplyOn.AllowNull = (DefaultableBoolean)1;
		this.cmbApplyOn.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbApplyOn.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbApplyOn.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbApplyOn).DisplayLayout.Appearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbApplyOn).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbApplyOn).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbApplyOn).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbApplyOn).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance51;
		((SpecialBoxBase)((UltraGridBase)this.cmbApplyOn).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbApplyOn).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbApplyOn).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbApplyOn).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbApplyOn).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbApplyOn).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbApplyOn).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbApplyOn).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbApplyOn).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance57).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbApplyOn).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbApplyOn).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbApplyOn).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance58).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbApplyOn).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance6).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbApplyOn).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbApplyOn).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbApplyOn).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbApplyOn).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbApplyOn).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbApplyOn).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbApplyOn).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbApplyOn).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbApplyOn).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbApplyOn).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbApplyOn.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbApplyOn).Location = new System.Drawing.Point(85, 5);
		((System.Windows.Forms.Control)(object)this.cmbApplyOn).Name = "cmbApplyOn";
		((System.Windows.Forms.Control)(object)this.cmbApplyOn).Size = new System.Drawing.Size(268, 26);
		((System.Windows.Forms.Control)(object)this.cmbApplyOn).TabIndex = 0;
		this.cmbitem.AllowNull = (DefaultableBoolean)1;
		this.cmbitem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbitem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbitem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbitem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbitem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance31;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance40).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbitem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbitem).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbitem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbitem).Location = new System.Drawing.Point(85, 33);
		((System.Windows.Forms.Control)(object)this.cmbitem).Name = "cmbitem";
		((System.Windows.Forms.Control)(object)this.cmbitem).Size = new System.Drawing.Size(268, 26);
		((System.Windows.Forms.Control)(object)this.cmbitem).TabIndex = 1;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtremarks.Location = new System.Drawing.Point(85, 61);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(552, 23);
		this.txtremarks.TabIndex = 2;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.Red;
		this.label14.Location = new System.Drawing.Point(464, 38);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(14, 17);
		this.label14.TabIndex = 84;
		this.label14.Text = "*";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.Red;
		this.label13.Location = new System.Drawing.Point(464, 10);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(14, 17);
		this.label13.TabIndex = 83;
		this.label13.Text = "*";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.Red;
		this.label6.Location = new System.Drawing.Point(71, 38);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(14, 17);
		this.label6.TabIndex = 82;
		this.label6.Text = "*";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.ForeColor = System.Drawing.Color.Red;
		this.label30.Location = new System.Drawing.Point(71, 10);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(14, 17);
		this.label30.TabIndex = 81;
		this.label30.Text = "*";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(356, 39);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(96, 15);
		this.label2.TabIndex = 43;
		this.label2.Text = "Effective Date To";
		this.TxtAffectiveDateTo.CustomFormat = "dd-MMM-yyy";
		this.TxtAffectiveDateTo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.TxtAffectiveDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.TxtAffectiveDateTo.Location = new System.Drawing.Point(478, 35);
		this.TxtAffectiveDateTo.Name = "TxtAffectiveDateTo";
		this.TxtAffectiveDateTo.Size = new System.Drawing.Size(159, 23);
		this.TxtAffectiveDateTo.TabIndex = 4;
		this.txtAffectiveDate.CustomFormat = "dd-MMM-yyy";
		this.txtAffectiveDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAffectiveDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtAffectiveDate.Location = new System.Drawing.Point(478, 7);
		this.txtAffectiveDate.Name = "txtAffectiveDate";
		this.txtAffectiveDate.Size = new System.Drawing.Size(159, 23);
		this.txtAffectiveDate.TabIndex = 3;
		this.chkIsaccepted.AutoSize = true;
		this.chkIsaccepted.Checked = true;
		this.chkIsaccepted.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkIsaccepted.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.chkIsaccepted.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
		this.chkIsaccepted.Location = new System.Drawing.Point(640, 63);
		this.chkIsaccepted.Name = "chkIsaccepted";
		this.chkIsaccepted.Size = new System.Drawing.Size(71, 19);
		this.chkIsaccepted.TabIndex = 5;
		this.chkIsaccepted.Text = "Is Active";
		this.chkIsaccepted.UseVisualStyleBackColor = true;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(356, 11);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(111, 15);
		this.label4.TabIndex = 0;
		this.label4.Text = "Effective Date From";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(5, 39);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(67, 15);
		this.label8.TabIndex = 0;
		this.label8.Text = "Item Name";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(5, 65);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(52, 15);
		this.label7.TabIndex = 0;
		this.label7.Text = "Remarks";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(5, 11);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(57, 15);
		this.label1.TabIndex = 0;
		this.label1.Text = "Apply On";
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.btn);
		this.panel1.Controls.Add(this.label21);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(728, 53);
		this.panel1.TabIndex = 38;
		this.btn.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btn.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.btn.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.btnNew, this.btnSave, this.btnUpdate, this.btnattachment, this.toolStripSeparator, this.btnprint });
		this.btn.Location = new System.Drawing.Point(0, 0);
		this.btn.Name = "btn";
		this.btn.Size = new System.Drawing.Size(728, 27);
		this.btn.TabIndex = 34;
		this.btn.Text = "toolStrip1";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnprint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(61, 24);
		this.btnprint.Text = "&Print";
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Visible = false;
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.White;
		this.label21.Location = new System.Drawing.Point(3, 30);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(365, 19);
		this.label21.TabIndex = 1;
		this.label21.Text = "Lab Analysis Standard Policy for Deductions";
		this.tabHistory.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.tabHistory.Controls.Add(this.panel4);
		this.tabHistory.Location = new System.Drawing.Point(4, 4);
		this.tabHistory.Name = "tabHistory";
		this.tabHistory.Padding = new System.Windows.Forms.Padding(3);
		this.tabHistory.Size = new System.Drawing.Size(734, 399);
		this.tabHistory.TabIndex = 1;
		this.tabHistory.Text = "History";
		this.tabHistory.UseVisualStyleBackColor = true;
		this.grdhistory.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdhistory).BackColor = System.Drawing.Color.White;
		grdhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistory.DesignTimeLayout = grdhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistory).Dock = System.Windows.Forms.DockStyle.Top;
		((System.Windows.Forms.Control)(object)this.grdhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(3, 50);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(728, 346);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 2;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 3);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(728, 47);
		this.panel4.TabIndex = 1;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(742, 428);
		base.Controls.Add(this.tabControl1);
		base.Name = "InvLabStandardPolicyForDeduction";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(Lab_Load);
		this.tabControl1.ResumeLayout(false);
		this.tabForm.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbdeduction).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbparameter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbweight).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbApplyOn).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.btn.ResumeLayout(false);
		this.btn.PerformLayout();
		this.tabHistory.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		base.ResumeLayout(false);
	}
}

}
