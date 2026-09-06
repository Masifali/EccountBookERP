using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Lab;
using Architecture.BLL.QCL;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.QCL;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.QCL;

public class frmQcDeductionPolicy : Form
{
	private UserAccount UserAccount = new UserAccount();

	private Rightsobjects formright = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private Attachment AT = new Attachment();

	private DataTable table = new DataTable();

	private bool Approved;

	private int updateDetailIndex = -1;

	private bool UpdateMode;

	private bool FormInitialized;

	private int RecId;

	private int DefaultDaysToLessFromHistoryFromDate;

	private List<QcDeductionPolicyDetail> lstRemoveRecord = new List<QcDeductionPolicyDetail>();

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabForm;

	internal GridEX grd;

	private Panel panel1;

	private Panel panel7;

	private TextBox txtremarks;

	private Label label4;

	private Label label7;

	private TabPage tabHistory;

	private DateTimePicker txtAffectiveDate;

	private Label label2;

	private DateTimePicker TxtAffectiveDateTo;

	internal UltraCombo CmbAnalysisParameter;

	private Label label3;

	private TextBox TxtDecRate;

	private Label label11;

	private TextBox txtrangeTo;

	private Label label9;

	private TextBox txtRangeFrom;

	private Label label5;

	private Button btnAdd;

	private ToolStrip btn;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton btnattachment;

	private ToolStripSeparator toolStripSeparator;

	private Panel panel8;

	private Label label14;

	private Label label13;

	private Label label17;

	private Label label16;

	private Label label15;

	private Label label18;

	private Label label19;

	private Button btnCancelDetail;

	private Button btnUpdateDetail;

	private Label label21;

	private Panel panel2;

	private TextBox txtPolicyName;

	private Label label6;

	private Label label23;

	private Label label1;

	private Label label22;

	internal UltraCombo CmbAnalysisGroup;

	private Label label28;

	private Label label26;

	private Label label25;

	private Label label8;

	private Label label27;

	private TextBox txtDeductionWt;

	private TextBox txtDeductionQty;

	internal UltraCombo CmbDeductionWeightUom;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnDelete;

	private ToolStripButton BtnShortCutkeys;

	private Panel panel9;

	private Label label55;

	internal GridEX grdhistory;

	private Panel panel3;

	private Panel panel14;

	private Label label12;

	private GroupBox groupBox2;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton rdentrydate;

	private Button btnShow;

	private Label label53;

	private Label label54;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private ToolStrip toolStrip2;

	private ToolStripButton btnResetHistory;

	private Panel panel13;

	private Label label49;

	internal GridEX DataGridHistoryDetail;

	private Label label20;

	internal UltraCombo ultraCombo2;

	public frmQcDeductionPolicy(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmQcDeductionPolicy_Load(object sender, EventArgs e)
	{
		try
		{
			FormInitialized = true;
			AutoScroll = true;
			base.AutoScrollMargin = new Size(5, 5);
			base.AutoScrollMinSize = new Size(1200, 500);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnSave.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			btnDelete.Enabled = formright.DoHaveCanDelete;
			AnalysisGroup();
			UOM();
			table.Columns.Add("Id", typeof(int));
			table.Columns.Add("AnalysisParameterId", typeof(int));
			table.Columns.Add("AnalysisParameter");
			table.Columns.Add("RangeFrom", typeof(double));
			table.Columns.Add("RangeTo", typeof(double));
			table.Columns.Add("DeductionWeight", typeof(double));
			table.Columns.Add("Uom", typeof(int));
			table.Columns.Add("DeductionQty", typeof(double));
			table.Columns.Add("DeductionRate", typeof(double));
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			txtPolicyName.Focus();
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			ToDateHistory.Value = DateTime.Now;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AnalysisGroup()
	{
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(CmbAnalysisGroup.Value);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("AnalysisGroupDescription", typeof(string));
			dt.Columns.Add("GroupTypeId", typeof(int));
			dt.Columns.Add("GroupType", typeof(string));
			DataTable dtLAG = InvLabAnalysisGroup.GetAllOrById(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtLAG.Rows.Count > 0)
			{
				for (int i = 0; i < dtLAG.Rows.Count; i++)
				{
					dt.Rows.Add(dtLAG.Rows[i]["Id"], dtLAG.Rows[i]["AnalysisGroupDescription"], dtLAG.Rows[i]["GroupType"], dtLAG.Rows[i]["InvParentCateDescription"]);
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbAnalysisGroup, "Id", "AnalysisGroupDescription", "Analysis Group", ZeroIndex: false);
				((UltraGridBase)CmbAnalysisGroup).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				if (Id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbAnalysisGroup.Value = Id;
					}
					else
					{
						((Control)(object)CmbAnalysisGroup).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbAnalysisGroup).Text = string.Empty;
				((UltraGridBase)CmbAnalysisGroup).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AnalysisParamerterByGroup(int GroupId)
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(CmbAnalysisParameter.Value);
			DataTable dt = new DataTable();
			dt.Columns.Add("AnalysisParameterId", typeof(int));
			dt.Columns.Add("AnalysisParameter", typeof(string));
			DataTable dtLAG = InvLabGroupAnalysisStandards.GetParametersFromGroupStandards(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = GroupId
			});
			if (dtLAG.Rows.Count > 0)
			{
				for (int i = 0; i < dtLAG.Rows.Count; i++)
				{
					dt.Rows.Add(dtLAG.Rows[i]["InvLabAnalysisItemsId"], dtLAG.Rows[i]["AnalysisParameterDescription"]);
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbAnalysisParameter, "AnalysisParameterId", "AnalysisParameter", "Analysis Parameter", ZeroIndex: false);
				if (Id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("AnalysisParameterId") == Id))
					{
						CmbAnalysisParameter.Value = Id;
					}
					else
					{
						((Control)(object)CmbAnalysisParameter).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbAnalysisParameter).Text = string.Empty;
				((UltraGridBase)CmbAnalysisParameter).DataSource = null;
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
			int Id = Conversion.ToInt(CmbDeductionWeightUom.Value);
			DataTable dtUOM = CommonServices.StaticColumnsService("GetUom");
			if (dtUOM.Rows.Count > 0)
			{
				DDL.BindDDL(dtUOM, CmbDeductionWeightUom, "Id", "type", "Weight Kg", ZeroIndex: false);
				if (Id > 0)
				{
					if (dtUOM.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbDeductionWeightUom.Value = Id;
					}
					else
					{
						((Control)(object)CmbDeductionWeightUom).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbDeductionWeightUom).Text = string.Empty;
				((UltraGridBase)CmbDeductionWeightUom).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbAnalysisGroup_Leave(object sender, EventArgs e)
	{
		try
		{
			int GroupId = Conversion.ToInt(CmbAnalysisGroup.Value);
			if (GroupId > 0)
			{
				AnalysisParamerterByGroup(GroupId);
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
					if (Conversion.ToInt(item.Cells["AnalysisParameterId"].Value) == Conversion.ToInt(CmbAnalysisParameter.Value))
					{
						if (IsBetween(Conversion.ToDouble((object)txtRangeFrom.Text.Trim()), Conversion.ToDouble(item.Cells["RangeFrom"].Value), Conversion.ToDouble(item.Cells["RangeTo"].Value)))
						{
							throw new Exception("RangeFrom Already Exist in another row");
						}
						if (IsBetween(Conversion.ToDouble((object)txtrangeTo.Text.Trim()), Conversion.ToDouble(item.Cells["RangeFrom"].Value), Conversion.ToDouble(item.Cells["RangeTo"].Value)))
						{
							throw new Exception("RangeTo Already Exist in another row");
						}
					}
				}
			}
			table.Rows.Add(0, Conversion.ToInt(CmbAnalysisParameter.Value), Conversion.ToString((object)((Control)(object)CmbAnalysisParameter).Text), Conversion.ToDouble((object)txtRangeFrom.Text), Conversion.ToDouble((object)txtrangeTo.Text), Conversion.ToDouble((object)txtDeductionWt.Text), Conversion.ToInt(CmbDeductionWeightUom.Value), Conversion.ToDouble((object)txtDeductionQty.Text), Conversion.ToDouble((object)TxtDecRate.Text));
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			((Control)(object)CmbAnalysisParameter).Text = string.Empty;
			txtRangeFrom.Text = string.Empty;
			txtrangeTo.Text = string.Empty;
			TxtDecRate.Text = string.Empty;
			((Control)(object)CmbDeductionWeightUom).Text = string.Empty;
			CmbAnalysisParameter.Focus();
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
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["AnalysisParameterId"].Visible = false;
			grd.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				GridEXColumn column = item;
				column.HeaderAlignment = (TextAlignment)2;
				column.EditType = (EditType)0;
				if (column.Type == typeof(double) || column.Type == typeof(decimal))
				{
					ConfigureNumericalColumn(column);
				}
			}
			grd.RootTable.Columns["AnalysisParameter"].Width = Constants.InventoryConstants.AccountTitle;
			grd.RootTable.Columns["RangeFrom"].Width = Constants.InventoryConstants.RangeConstant;
			grd.RootTable.Columns["RangeTo"].Width = Constants.InventoryConstants.RangeConstant;
			grd.RootTable.Columns["DeductionWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["Uom"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["DeductionQty"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["DeductionRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "X";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
			grd.RootTable.Columns["Delete"].Width = 30;
			grd.RootTable.Columns["Delete"].Position = 0;
			grd.RootTable.Columns.Add("Edit");
			grd.RootTable.Columns["Edit"].Key = "Edit";
			grd.RootTable.Columns["Edit"].Caption = "Edit";
			grd.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Edit"].ButtonText = "Edit";
			grd.RootTable.Columns["Edit"].Width = 60;
			grd.RootTable.Columns["Edit"].Position = 1;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ConfigureNumericalColumn(GridEXColumn column)
	{
		if (column.Key.Contains("Amount"))
		{
			column.AggregateFunction = (AggregateFunction)2;
			column.TextAlignment = (TextAlignment)3;
			column.FormatString = clsGlobalVariables.stringFormatsingle;
			column.TotalFormatString = clsGlobalVariables.stringFormatsingle;
		}
		else if (column.Key.Contains("Rate"))
		{
			column.TextAlignment = (TextAlignment)3;
			column.FormatString = clsGlobalVariables.DecimalRateFormate;
		}
		else
		{
			column.AggregateFunction = (AggregateFunction)2;
			column.TextAlignment = (TextAlignment)3;
			column.FormatString = "#,##0.##";
			column.TotalFormatString = "#,##0.##";
		}
	}

	private void DeleteDetailRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			if (updateDetailIndex == -1)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
					{
						QcDeductionPolicyDetail vd = new QcDeductionPolicyDetail();
						vd.QcDeductionPolicyDetailId = Conversion.ToInt(r.Cells["Id"].Value);
						vd.AnalysisParameterId = Conversion.ToInt(r.Cells["AnalysisParameterId"].Value);
						vd.RangeFrom = Conversion.ToDecimal(r.Cells["RangeFrom"].Value);
						vd.RangeTo = Conversion.ToDecimal(r.Cells["RangeTo"].Value);
						vd.DeductionParameterId = Conversion.ToInt(r.Cells["DeductionOnId"].Value);
						vd.dedWeight = Conversion.ToDecimal(r.Cells["DeductionWeight"].Value);
						vd.dedWtUom = Conversion.ToInt(r.Cells["Uom"].Value);
						vd.dedOnQtyKg = Conversion.ToDouble(r.Cells["DeductionQty"].Value);
						vd.dedOnRateRs = Conversion.ToDecimal(r.Cells["DeductionRate"].Value);
						vd.ActionTypeId = 3;
						lstRemoveRecord.Add(vd);
						table.Rows[r.RowIndex].Delete();
						r.Delete();
						grd.UpdateData();
					}
				}
				else
				{
					r.Delete();
					grd.UpdateData();
				}
				return;
			}
			throw new Exception("Please Reset the Detail First...");
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
			if (!FormValidationDetail())
			{
				return;
			}
			if (grd.GetRows().Length != 0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (Conversion.ToInt(item.Cells["AnalysisParameterId"].Value) == Conversion.ToInt(CmbAnalysisParameter.Value) && item.RowIndex != updateDetailIndex)
					{
						if (IsBetween(Conversion.ToDouble((object)txtRangeFrom.Text.Trim()), Conversion.ToDouble(item.Cells["RangeFrom"].Value), Conversion.ToDouble(item.Cells["RangeTo"].Value)))
						{
							throw new Exception("RangeFrom Already Exist in another row");
						}
						if (IsBetween(Conversion.ToDouble((object)txtrangeTo.Text.Trim()), Conversion.ToDouble(item.Cells["RangeFrom"].Value), Conversion.ToDouble(item.Cells["RangeTo"].Value)))
						{
							throw new Exception("RangeTo Already Exist in another row");
						}
					}
				}
			}
			table.Rows[updateDetailIndex]["AnalysisParameterId"] = Conversion.ToInt(CmbAnalysisParameter.Value);
			table.Rows[updateDetailIndex]["AnalysisParameter"] = Conversion.ToString((object)((Control)(object)CmbAnalysisParameter).Text);
			table.Rows[updateDetailIndex]["RangeFrom"] = Conversion.ToDouble((object)txtRangeFrom.Text);
			table.Rows[updateDetailIndex]["RangeTo"] = Conversion.ToDouble((object)txtrangeTo.Text);
			table.Rows[updateDetailIndex]["DeductionWeight"] = Conversion.ToDouble((object)txtDeductionWt.Text);
			table.Rows[updateDetailIndex]["Uom"] = Conversion.ToInt(CmbDeductionWeightUom.Value);
			table.Rows[updateDetailIndex]["DeductionQty"] = Conversion.ToDouble((object)txtDeductionQty.Text);
			table.Rows[updateDetailIndex]["DeductionRate"] = Conversion.ToDouble((object)TxtDecRate.Text);
			grd.DataSource = table;
			ResetDetail();
			CmbAnalysisParameter.Focus();
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
			ResetDetail();
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
				CmbAnalysisParameter.Value = item.Cells["AnalysisParameterId"].Value.ToString();
				((Control)(object)CmbAnalysisParameter).Text = item.Cells["AnalysisParameter"].Value.ToString();
				txtRangeFrom.Text = item.Cells["RangeFrom"].Value.ToString();
				txtrangeTo.Text = item.Cells["RangeTo"].Value.ToString();
				txtDeductionWt.Text = item.Cells["DeductionWeight"].Value.ToString();
				((Control)(object)CmbDeductionWeightUom).Text = item.Cells["Uom"].Value.ToString();
				txtDeductionQty.Text = item.Cells["DeductionQty"].Value.ToString();
				TxtDecRate.Text = item.Cells["DeductionRate"].Value.ToString();
				btnAdd.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelDetail.Visible = true;
				CmbAnalysisParameter.Focus();
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
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteDetailRow(r);
				}
				if (e.Column.Key == "Edit")
				{
					grd_DoubleClick(null, null);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (string.IsNullOrEmpty(txtPolicyName.Text) || txtPolicyName.Text.Trim() == "0")
		{
			MessageBox.Show("PolicyName field is required");
			txtPolicyName.Focus();
			return false;
		}
		if (((UltraGridBase)CmbAnalysisGroup).ActiveRow == null || Conversion.ToInt(CmbAnalysisGroup.Value) == 0)
		{
			MessageBox.Show("AnalysisGroup field is required");
			CmbAnalysisGroup.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)CmbAnalysisParameter).ActiveRow == null || Conversion.ToInt(CmbAnalysisParameter.Value) == 0)
		{
			MessageBox.Show("Parameter Field is Required");
			CmbAnalysisParameter.Focus();
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
		if (Conversion.ToDouble((object)txtRangeFrom.Text.Trim()) >= Conversion.ToDouble((object)txtrangeTo.Text.Trim()))
		{
			MessageBox.Show("Range From Must Be less Then Range To");
			txtRangeFrom.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtDeductionWt.Text) <= 0.0)
		{
			MessageBox.Show("Deduction Weight Field is Required");
			txtDeductionWt.Focus();
			return false;
		}
		if (((UltraGridBase)CmbDeductionWeightUom).ActiveRow == null || Conversion.ToInt(CmbDeductionWeightUom.Value) == 0)
		{
			MessageBox.Show("Deduction UOM Field is Required");
			CmbDeductionWeightUom.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtDeductionQty.Text) <= 0.0)
		{
			MessageBox.Show("Deduction Qty Field is Required");
			txtDeductionQty.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)TxtDecRate.Text) <= 0.0)
		{
			MessageBox.Show("Deduction Value Field is Required");
			TxtDecRate.Focus();
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
				throw new Exception("Record cannot be updated because RecId not found");
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
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Expected O, but got Unknown
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
			QcDeductionPolicyHeader obj = new QcDeductionPolicyHeader();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				obj.QcDeductionPolicyHeaderId = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			obj.PolicyName = Conversion.ToString((object)txtPolicyName.Text);
			obj.InvLabGroupAnalysisStandardsId = Conversion.ToInt(CmbAnalysisGroup.Value);
			obj.EffectiveFrom = Conversion.ToDateTime((object)txtAffectiveDate.Value);
			obj.EfffectiveTo = Conversion.ToDateTime((object)TxtAffectiveDateTo.Value);
			obj.RemarksHeader = Conversion.ToString((object)txtremarks.Text);
			obj.CompanyId = UserAccount.CompanyId;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.EntryDate = DateTime.Now;
			obj.EntryUserId = UserAccount.ID;
			obj.ModifyUserId = UserAccount.ID;
			obj.ApprovedUserId = UserAccount.ID;
			obj.ModifyDate = DateTime.Now;
			obj.ApprovedDate = DateTime.Now;
			obj.QcDeductionPolicyDetailList = new List<QcDeductionPolicyDetail>();
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				QcDeductionPolicyDetail vd = new QcDeductionPolicyDetail();
				if (RecId == 0)
				{
					vd.ActionTypeId = 1;
				}
				else
				{
					vd.QcDeductionPolicyDetailId = Conversion.ToInt(r.Cells["Id"].Value);
					if (vd.QcDeductionPolicyDetailId > 0)
					{
						vd.ActionTypeId = 2;
					}
					else
					{
						vd.ActionTypeId = 1;
					}
				}
				if (Conversion.ToInt(r.Cells["AnalysisParameterId"].Value) == 0)
				{
					throw new Exception($"AnalysisParameter Field Required in Grid Row no : {r.RowIndex + 1}");
				}
				vd.AnalysisParameterId = Conversion.ToInt(r.Cells["AnalysisParameterId"].Value);
				vd.RangeFrom = Conversion.ToDecimal(r.Cells["RangeFrom"].Value);
				vd.RangeTo = Conversion.ToDecimal(r.Cells["RangeTo"].Value);
				vd.dedWeight = Conversion.ToDecimal(r.Cells["DeductionWeight"].Value);
				vd.dedWtUom = Conversion.ToInt(r.Cells["Uom"].Value);
				vd.dedOnQtyKg = Conversion.ToDouble(r.Cells["DeductionQty"].Value);
				vd.dedOnRateRs = Conversion.ToDecimal(r.Cells["DeductionRate"].Value);
				obj.QcDeductionPolicyDetailList.Add(vd);
			}
			if (obj.QcDeductionPolicyDetailList.Count == 0)
			{
				throw new Exception("At least enter value/quantity in one of the rows");
			}
			if (RecId > 0 && lstRemoveRecord.Count > 0)
			{
				for (int i = 0; i < lstRemoveRecord.Count; i++)
				{
					obj.QcDeductionPolicyDetailList.Add(lstRemoveRecord[i]);
				}
			}
			obj.AttachmentsList = new List<DMSAttachments>();
			obj.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<QcDeductionPolicyHeader>(obj, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			QcDeductionPolicyHeader.Save(obj);
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + obj.PolicyName);
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, 0, base.Name);
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + obj.PolicyName);
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

	public void ReadById(int Id)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		try
		{
			RecId = Conversion.ToInt((object)Id);
			QcDeductionPolicyHeader lab = new QcDeductionPolicyHeader();
			lab = QcDeductionPolicyHeader.GetByID(Id);
			if (lab != null)
			{
				tabControl1.SelectedIndex = 0;
				btnSave.Visible = false;
				btnUpdate.Visible = true;
				btnDelete.Visible = true;
				txtPolicyName.Text = Conversion.ToString((object)lab.PolicyName);
				CmbAnalysisGroup.Value = Conversion.ToInt((object)lab.InvLabGroupAnalysisStandardsId);
				txtremarks.Text = Conversion.ToString((object)lab.RemarksHeader);
				txtAffectiveDate.Value = Conversion.ToDateTime((object)lab.EffectiveFrom);
				TxtAffectiveDateTo.Value = Conversion.ToDateTime((object)lab.EfffectiveTo);
				CmbAnalysisGroup_Leave(null, null);
				table.Rows.Clear();
				for (int i = 0; i < lab.QcDeductionPolicyDetailList.Count; i++)
				{
					table.Rows.Add(lab.QcDeductionPolicyDetailList[i].QcDeductionPolicyDetailId, lab.QcDeductionPolicyDetailList[i].AnalysisParameterId, lab.QcDeductionPolicyDetailList[i].AnalysisParameter, lab.QcDeductionPolicyDetailList[i].RangeFrom, lab.QcDeductionPolicyDetailList[i].RangeTo, lab.QcDeductionPolicyDetailList[i].dedWeight, lab.QcDeductionPolicyDetailList[i].dedWtUom, lab.QcDeductionPolicyDetailList[i].dedOnQtyKg, lab.QcDeductionPolicyDetailList[i].dedOnRateRs);
				}
				grd.DataSource = table;
				grd.RetrieveStructure();
				grdSettings();
				FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (Approved)
			{
				throw new Exception("Record Not Delete because Record has approved");
			}
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					QcDeductionPolicyHeader.DeleteByID(RecId, UserAccount.ID);
					MessageBox.Show("Delete Record Successfully");
					Reset();
				}
			}
			else
			{
				MessageBox.Show("Record Not Found");
			}
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			AnalysisGroup();
			UOM();
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
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			lstRemoveRecord.Clear();
			Approved = false;
			RecId = 0;
			UpdateMode = false;
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			txtPolicyName.Text = string.Empty;
			txtremarks.Text = string.Empty;
			txtPolicyName.Focus();
			table.Rows.Clear();
			grd.ClearStructure();
			ResetDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetDetail()
	{
		try
		{
			updateDetailIndex = -1;
			((Control)(object)CmbAnalysisParameter).Text = string.Empty;
			txtRangeFrom.Text = string.Empty;
			txtrangeTo.Text = string.Empty;
			txtDeductionWt.Text = string.Empty;
			txtDeductionQty.Text = string.Empty;
			TxtDecRate.Text = string.Empty;
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmQcDeductionPolicy_FormClosing(object sender, FormClosingEventArgs e)
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

	private void txtRangeFrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtrangeTo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void TxtDecValue_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtDeductionQty_KeyPress(object sender, KeyPressEventArgs e)
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
				txtPolicyName.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnResetHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			ToDateHistory.Value = DateTime.Now;
			grdhistory.DataSource = null;
			DataGridHistoryDetail.DataSource = null;
			rdentrydate.Checked = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
	}

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			gridhistoryfill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void gridhistoryfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters reports = new ReportsParameters();
			reports.OrganizationId = UserAccount.OrganizationId;
			reports.CompanyId = UserAccount.CompanyId;
			reports.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!reports.CanViewAllRecord)
			{
				reports.EntryUser = UserAccount.ID;
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					reports.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					reports.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					reports.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					reports.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					reports.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					reports.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			DataTable dtGridRegister = QcDeductionPolicyHeader.FormHistory(reports);
			if (dtGridRegister.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("InvLabGroupAnalysisStandardsId", typeof(int));
				dtcol.Columns.Add("AnalysisGroup", typeof(string));
				dtcol.Columns.Add("PolicyName", typeof(string));
				dtcol.Columns.Add("EffectiveFrom", typeof(DateTime));
				dtcol.Columns.Add("EffectiveTo", typeof(DateTime));
				dtcol.Columns.Add("RemarksHeader", typeof(string));
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("EntryUser", typeof(string));
				dtcol.Columns.Add("ModifyDate", typeof(string));
				dtcol.Columns.Add("ModifyUser", typeof(string));
				dtcol.Columns.Add("IsApproved", typeof(string));
				dtcol.Columns.Add("ApprovedDate", typeof(string));
				dtcol.Columns.Add("ApprovedUser", typeof(string));
				dtcol.Columns.Add("NoOfAttachments", typeof(int));
				dtcol.Rows.Clear();
				foreach (DataRow i in dtGridRegister.Rows)
				{
					dtcol.Rows.Add(i["QcDeductionPolicyHeaderId"], i["InvLabGroupAnalysisStandardsId"], i["InvLabAnalysisGroup"], i["PolicyName"], i["EffectiveFrom"], i["EfffectiveTo"], i["RemarksHeader"], i["EntryDate"], i["EntryUser"], Conversion.CheckDateTimeNull(Conversion.ToDateTime(i["ModifyDate"])) ? "" : Conversion.ToDateTime(i["ModifyDate"]).ToShortDateString(), i["ModifyUser"], Conversion.ToBool(i["IsApproved"]) ? "Approved" : "Not Approved", Conversion.CheckDateTimeNull(Conversion.ToDateTime(i["ApprovedDate"])) ? "" : Conversion.ToDateTime(i["ApprovedDate"]).ToShortDateString(), i["ApprovedUser"], i["NoOfAttachments"]);
				}
				grdhistory.DataSource = dtcol;
				grdhistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				grdhistory.ClearStructure();
				DataGridHistoryDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void HistoryGridSettings()
	{
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Expected O, but got Unknown
		try
		{
			grdhistory.RootTable.Columns["Id"].Visible = false;
			grdhistory.RootTable.Columns["InvLabGroupAnalysisStandardsId"].Visible = false;
			grdhistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdhistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdhistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdhistory.RootTable.HeaderLines = 3;
			foreach (GridEXColumn item in (JanusCollectionBase)grdhistory.RootTable.Columns)
			{
				GridEXColumn column = item;
				column.HeaderAlignment = (TextAlignment)2;
				if (column.Type == typeof(int) || column.Type == typeof(double) || column.Type == typeof(decimal))
				{
					ConfigureNumericalColumn(column);
				}
			}
			grdhistory.RootTable.Columns["AnalysisGroup"].Width = Constants.InventoryConstants.PlantName;
			grdhistory.RootTable.Columns["PolicyName"].Width = Constants.InventoryConstants.ItemName;
			grdhistory.RootTable.Columns["EffectiveFrom"].Width = Constants.InventoryConstants.DateConstant;
			grdhistory.RootTable.Columns["EffectiveTo"].Width = Constants.InventoryConstants.DateConstant;
			grdhistory.RootTable.Columns["RemarksHeader"].Width = Constants.InventoryConstants.RemarksConstants;
			grdhistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateConstant;
			grdhistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			grdhistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateConstant;
			grdhistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants;
			grdhistory.RootTable.Columns["IsApproved"].Width = Constants.InventoryConstants.IsApproved;
			grdhistory.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateConstant;
			grdhistory.RootTable.Columns["ApprovedUser"].Width = Constants.InventoryConstants.ModifyUserConstants;
			grdhistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			grdhistory.RootTable.Columns.Add("Edit");
			grdhistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdhistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdhistory.RootTable.Columns["Edit"].Width = 40;
			grdhistory.RootTable.Columns["Edit"].Position = 0;
			grdhistory.FrozenColumns = 1;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdhistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "Edit")
				{
					Reset();
					RecId = CurrId;
					ReadById(RecId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdhistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByScreenName(Conversion.ToInt(item.Cells["Id"].Value), base.Name);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				RecId = CurrId;
				ReadById(RecId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				GridDetailBind();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailBind()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			QcDeductionPolicyHeader po = QcDeductionPolicyHeader.GetByID(Conversion.ToInt(r.Cells["Id"].Value));
			if (po != null)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("AnalysisParameterId", typeof(int));
				dt.Columns.Add("AnalysisParameter");
				dt.Columns.Add("RangeFrom", typeof(double));
				dt.Columns.Add("RangeTo", typeof(double));
				dt.Columns.Add("DeductionWeight", typeof(double));
				dt.Columns.Add("Uom", typeof(int));
				dt.Columns.Add("DeductionQty", typeof(double));
				dt.Columns.Add("DeductionRate", typeof(double));
				for (int i = 0; i < po.QcDeductionPolicyDetailList.Count; i++)
				{
					dt.Rows.Add(po.QcDeductionPolicyDetailList[i].QcDeductionPolicyDetailId, po.QcDeductionPolicyDetailList[i].AnalysisParameterId, po.QcDeductionPolicyDetailList[i].AnalysisParameter, po.QcDeductionPolicyDetailList[i].RangeFrom, po.QcDeductionPolicyDetailList[i].RangeTo, po.QcDeductionPolicyDetailList[i].dedWeight, po.QcDeductionPolicyDetailList[i].dedWtUom, po.QcDeductionPolicyDetailList[i].dedOnQtyKg, po.QcDeductionPolicyDetailList[i].dedOnRateRs);
				}
				DataGridHistoryDetail.DataSource = dt;
				DataGridHistoryDetail.RetrieveStructure();
				DataGridHistoryDetailSetting();
			}
			else
			{
				DataGridHistoryDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistoryDetailSetting()
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		try
		{
			DataGridHistoryDetail.RootTable.Columns["Id"].Visible = false;
			DataGridHistoryDetail.RootTable.Columns["AnalysisParameterId"].Visible = false;
			DataGridHistoryDetail.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)DataGridHistoryDetail.RootTable.Columns)
			{
				GridEXColumn column = item;
				column.HeaderAlignment = (TextAlignment)2;
				column.EditType = (EditType)0;
				if (column.Type == typeof(double) || column.Type == typeof(decimal))
				{
					ConfigureNumericalColumn(column);
				}
			}
			DataGridHistoryDetail.RootTable.Columns["AnalysisParameter"].Width = Constants.InventoryConstants.AccountTitle;
			DataGridHistoryDetail.RootTable.Columns["RangeFrom"].Width = Constants.InventoryConstants.RangeConstant;
			DataGridHistoryDetail.RootTable.Columns["RangeTo"].Width = Constants.InventoryConstants.RangeConstant;
			DataGridHistoryDetail.RootTable.Columns["DeductionWeight"].Width = Constants.InventoryConstants.WeightConstant;
			DataGridHistoryDetail.RootTable.Columns["Uom"].Width = Constants.InventoryConstants.UomConstant;
			DataGridHistoryDetail.RootTable.Columns["DeductionQty"].Width = Constants.InventoryConstants.QtyConstant;
			DataGridHistoryDetail.RootTable.Columns["DeductionRate"].Width = Constants.InventoryConstants.ExRateConstant;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnattachment_Click(object sender, EventArgs e)
	{
		try
		{
			AT.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmQcDeductionPolicy_KeyDown(object sender, KeyEventArgs e)
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
					txtPolicyName.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					FromDateHistory.Focus();
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
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					txtPolicyName.Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					btnattachment_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grd).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					txtPolicyName.Focus();
				}
				else if (e.Control && e.Shift && e.KeyCode == Keys.Delete && btnDelete.Visible && btnDelete.Enabled)
				{
					btnDelete_Click(null, null);
				}
			}
			else
			{
				if (e.Control && e.KeyCode == Keys.S)
				{
					btnShow_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnResetHistory_Click(null, null);
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
					((Control)(object)grdhistory).Focus();
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
			dt.Rows.Add("Ctrl+F5", "For Focus on Policy Name");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid when focus in form tab and for focus on history grid when in history tab");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus on Parameter in Detail Grid when in Form tab and For focus on FromDate in history tab");
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

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grd.CurrentColumn.Key == "Delete")
				{
					DeleteDetailRow(r);
				}
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteDetailRow(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grdhistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int CuurID = Conversion.ToInt(item.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Return)
			{
				Reset();
				ReadById(CuurID);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdhistory.CurrentColumn.Key == "Edit")
				{
					Reset();
					ReadById(CuurID);
				}
				else if (grdhistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByScreenName(CuurID, base.Name);
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
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Expected O, but got Unknown
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Expected O, but got Unknown
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Expected O, but got Unknown
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Expected O, but got Unknown
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Expected O, but got Unknown
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Expected O, but got Unknown
		//IL_04b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bb: Expected O, but got Unknown
		//IL_084a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0854: Expected O, but got Unknown
		//IL_337d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3387: Expected O, but got Unknown
		//IL_33ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_33b5: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance5 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		GridEXLayout DataGridHistoryDetail_DesignTimeLayout = new GridEXLayout();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabForm = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.btnAdd = new System.Windows.Forms.Button();
		this.btnCancelDetail = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.label28 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.txtRangeFrom = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.txtrangeTo = new System.Windows.Forms.TextBox();
		this.txtDeductionWt = new System.Windows.Forms.TextBox();
		this.txtDeductionQty = new System.Windows.Forms.TextBox();
		this.TxtDecRate = new System.Windows.Forms.TextBox();
		this.CmbAnalysisParameter = new UltraCombo();
		this.CmbDeductionWeightUom = new UltraCombo();
		this.panel7 = new System.Windows.Forms.Panel();
		this.txtPolicyName = new System.Windows.Forms.TextBox();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.TxtAffectiveDateTo = new System.Windows.Forms.DateTimePicker();
		this.txtAffectiveDate = new System.Windows.Forms.DateTimePicker();
		this.label1 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.CmbAnalysisGroup = new UltraCombo();
		this.panel1 = new System.Windows.Forms.Panel();
		this.btn = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.label21 = new System.Windows.Forms.Label();
		this.tabHistory = new System.Windows.Forms.TabPage();
		this.grdhistory = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label55 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label12 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label20 = new System.Windows.Forms.Label();
		this.ultraCombo2 = new UltraCombo();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnShow = new System.Windows.Forms.Button();
		this.label53 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label49 = new System.Windows.Forms.Label();
		this.DataGridHistoryDetail = new GridEX();
		this.tabControl1.SuspendLayout();
		this.tabForm.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAnalysisParameter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeductionWeightUom).BeginInit();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAnalysisGroup).BeginInit();
		this.panel1.SuspendLayout();
		this.btn.SuspendLayout();
		this.tabHistory.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel9.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel14.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo2).BeginInit();
		this.toolStrip2.SuspendLayout();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistoryDetail).BeginInit();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabForm);
		this.tabControl1.Controls.Add(this.tabHistory);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(941, 511);
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
		this.tabForm.Size = new System.Drawing.Size(933, 482);
		this.tabForm.TabIndex = 0;
		this.tabForm.Text = "Form";
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 219);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(927, 260);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 2;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 198);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(927, 21);
		this.panel2.TabIndex = 81;
		this.panel8.BackColor = System.Drawing.Color.White;
		this.panel8.Controls.Add(this.btnAdd);
		this.panel8.Controls.Add(this.btnCancelDetail);
		this.panel8.Controls.Add(this.btnUpdateDetail);
		this.panel8.Controls.Add(this.label28);
		this.panel8.Controls.Add(this.label19);
		this.panel8.Controls.Add(this.label26);
		this.panel8.Controls.Add(this.label18);
		this.panel8.Controls.Add(this.label17);
		this.panel8.Controls.Add(this.label16);
		this.panel8.Controls.Add(this.label15);
		this.panel8.Controls.Add(this.label25);
		this.panel8.Controls.Add(this.label5);
		this.panel8.Controls.Add(this.label8);
		this.panel8.Controls.Add(this.label27);
		this.panel8.Controls.Add(this.label11);
		this.panel8.Controls.Add(this.txtRangeFrom);
		this.panel8.Controls.Add(this.label3);
		this.panel8.Controls.Add(this.label9);
		this.panel8.Controls.Add(this.txtrangeTo);
		this.panel8.Controls.Add(this.txtDeductionWt);
		this.panel8.Controls.Add(this.txtDeductionQty);
		this.panel8.Controls.Add(this.TxtDecRate);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAnalysisParameter);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeductionWeightUom);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 117);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(927, 81);
		this.panel8.TabIndex = 1;
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(739, 48);
		this.btnAdd.Margin = new System.Windows.Forms.Padding(2);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(40, 25);
		this.btnAdd.TabIndex = 7;
		this.btnAdd.Text = "+";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(Add_Click);
		this.btnCancelDetail.BackColor = System.Drawing.Color.Teal;
		this.btnCancelDetail.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.btnCancelDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelDetail.ForeColor = System.Drawing.Color.White;
		this.btnCancelDetail.Location = new System.Drawing.Point(816, 49);
		this.btnCancelDetail.Name = "btnCancelDetail";
		this.btnCancelDetail.Size = new System.Drawing.Size(74, 23);
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
		this.btnUpdateDetail.Location = new System.Drawing.Point(740, 49);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(74, 23);
		this.btnUpdateDetail.TabIndex = 7;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.ForeColor = System.Drawing.Color.Red;
		this.label28.Location = new System.Drawing.Point(602, 23);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(14, 17);
		this.label28.TabIndex = 89;
		this.label28.Text = "*";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.Red;
		this.label19.Location = new System.Drawing.Point(716, 21);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(14, 17);
		this.label19.TabIndex = 89;
		this.label19.Text = "*";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ForeColor = System.Drawing.Color.Red;
		this.label26.Location = new System.Drawing.Point(416, 23);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(14, 17);
		this.label26.TabIndex = 88;
		this.label26.Text = "*";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.Color.Red;
		this.label18.Location = new System.Drawing.Point(489, 23);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(14, 17);
		this.label18.TabIndex = 88;
		this.label18.Text = "*";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.Red;
		this.label17.Location = new System.Drawing.Point(297, 23);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(14, 17);
		this.label17.TabIndex = 87;
		this.label17.Text = "*";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.Red;
		this.label16.Location = new System.Drawing.Point(230, 23);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(14, 17);
		this.label16.TabIndex = 86;
		this.label16.Text = "*";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.Red;
		this.label15.Location = new System.Drawing.Point(117, 23);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(14, 17);
		this.label15.TabIndex = 85;
		this.label15.Text = "*";
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(447, 15);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(33, 32);
		this.label25.TabIndex = 50;
		this.label25.Text = "Uom";
		this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(161, 15);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(71, 32);
		this.label5.TabIndex = 45;
		this.label5.Text = "Range From";
		this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(330, 15);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(80, 32);
		this.label8.TabIndex = 49;
		this.label8.Text = "Deduction On Wt (Kg)";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(519, 15);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(85, 32);
		this.label27.TabIndex = 49;
		this.label27.Text = "Deduction On Qty (Kg)";
		this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(618, 15);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(109, 32);
		this.label11.TabIndex = 49;
		this.label11.Text = "Deduction On Rate(Rs)";
		this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtRangeFrom.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRangeFrom.Location = new System.Drawing.Point(161, 49);
		this.txtRangeFrom.Name = "txtRangeFrom";
		this.txtRangeFrom.Size = new System.Drawing.Size(83, 23);
		this.txtRangeFrom.TabIndex = 1;
		this.txtRangeFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRangeFrom_KeyPress);
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(5, 15);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(107, 32);
		this.label3.TabIndex = 1;
		this.label3.Text = "Analysis Parameter";
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(245, 15);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(56, 32);
		this.label9.TabIndex = 47;
		this.label9.Text = "Range To";
		this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtrangeTo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtrangeTo.Location = new System.Drawing.Point(245, 49);
		this.txtrangeTo.Name = "txtrangeTo";
		this.txtrangeTo.Size = new System.Drawing.Size(83, 23);
		this.txtrangeTo.TabIndex = 2;
		this.txtrangeTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtrangeTo_KeyPress);
		this.txtDeductionWt.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDeductionWt.Location = new System.Drawing.Point(330, 49);
		this.txtDeductionWt.Name = "txtDeductionWt";
		this.txtDeductionWt.Size = new System.Drawing.Size(115, 23);
		this.txtDeductionWt.TabIndex = 3;
		this.txtDeductionWt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(TxtDecValue_KeyPress);
		this.txtDeductionQty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDeductionQty.Location = new System.Drawing.Point(519, 49);
		this.txtDeductionQty.Name = "txtDeductionQty";
		this.txtDeductionQty.Size = new System.Drawing.Size(98, 23);
		this.txtDeductionQty.TabIndex = 5;
		this.txtDeductionQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDeductionQty_KeyPress);
		this.TxtDecRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.TxtDecRate.Location = new System.Drawing.Point(618, 49);
		this.TxtDecRate.Name = "TxtDecRate";
		this.TxtDecRate.Size = new System.Drawing.Size(120, 23);
		this.TxtDecRate.TabIndex = 6;
		this.TxtDecRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(TxtDecValue_KeyPress);
		this.CmbAnalysisParameter.AllowNull = (DefaultableBoolean)1;
		this.CmbAnalysisParameter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAnalysisParameter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAnalysisParameter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAnalysisParameter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbAnalysisParameter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAnalysisParameter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAnalysisParameter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAnalysisParameter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance32;
		((SpecialBoxBase)((UltraGridBase)this.CmbAnalysisParameter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAnalysisParameter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbAnalysisParameter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAnalysisParameter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAnalysisParameter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAnalysisParameter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbAnalysisParameter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAnalysisParameter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAnalysisParameter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance10).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAnalysisParameter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbAnalysisParameter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAnalysisParameter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance11).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAnalysisParameter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAnalysisParameter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbAnalysisParameter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAnalysisParameter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAnalysisParameter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbAnalysisParameter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAnalysisParameter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbAnalysisParameter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAnalysisParameter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAnalysisParameter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAnalysisParameter).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbAnalysisParameter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAnalysisParameter).Location = new System.Drawing.Point(5, 47);
		((System.Windows.Forms.Control)(object)this.CmbAnalysisParameter).Name = "CmbAnalysisParameter";
		((System.Windows.Forms.Control)(object)this.CmbAnalysisParameter).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.CmbAnalysisParameter).TabIndex = 0;
		this.CmbDeductionWeightUom.AllowNull = (DefaultableBoolean)1;
		this.CmbDeductionWeightUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeductionWeightUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeductionWeightUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeductionWeightUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbDeductionWeightUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeductionWeightUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeductionWeightUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeductionWeightUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeductionWeightUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeductionWeightUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbDeductionWeightUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeductionWeightUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeductionWeightUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeductionWeightUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbDeductionWeightUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeductionWeightUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeductionWeightUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance45).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeductionWeightUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbDeductionWeightUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeductionWeightUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeductionWeightUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeductionWeightUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbDeductionWeightUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeductionWeightUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeductionWeightUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbDeductionWeightUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeductionWeightUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbDeductionWeightUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeductionWeightUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeductionWeightUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeductionWeightUom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbDeductionWeightUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeductionWeightUom).Location = new System.Drawing.Point(447, 47);
		((System.Windows.Forms.Control)(object)this.CmbDeductionWeightUom).Name = "CmbDeductionWeightUom";
		((System.Windows.Forms.Control)(object)this.CmbDeductionWeightUom).Size = new System.Drawing.Size(70, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeductionWeightUom).TabIndex = 4;
		this.panel7.Controls.Add(this.txtPolicyName);
		this.panel7.Controls.Add(this.txtremarks);
		this.panel7.Controls.Add(this.label14);
		this.panel7.Controls.Add(this.label13);
		this.panel7.Controls.Add(this.label2);
		this.panel7.Controls.Add(this.label6);
		this.panel7.Controls.Add(this.label23);
		this.panel7.Controls.Add(this.TxtAffectiveDateTo);
		this.panel7.Controls.Add(this.txtAffectiveDate);
		this.panel7.Controls.Add(this.label1);
		this.panel7.Controls.Add(this.label4);
		this.panel7.Controls.Add(this.label7);
		this.panel7.Controls.Add(this.label22);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.panel7.Location = new System.Drawing.Point(3, 56);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(927, 61);
		this.panel7.TabIndex = 0;
		this.txtPolicyName.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPolicyName.Location = new System.Drawing.Point(104, 5);
		this.txtPolicyName.Name = "txtPolicyName";
		this.txtPolicyName.Size = new System.Drawing.Size(207, 23);
		this.txtPolicyName.TabIndex = 0;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtremarks.Location = new System.Drawing.Point(623, 5);
		this.txtremarks.Multiline = true;
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(300, 51);
		this.txtremarks.TabIndex = 4;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.Red;
		this.label14.Location = new System.Drawing.Point(422, 35);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(14, 17);
		this.label14.TabIndex = 84;
		this.label14.Text = "*";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.Red;
		this.label13.Location = new System.Drawing.Point(422, 8);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(14, 17);
		this.label13.TabIndex = 83;
		this.label13.Text = "*";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(315, 36);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(96, 15);
		this.label2.TabIndex = 43;
		this.label2.Text = "Effective Date To";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.Red;
		this.label6.Location = new System.Drawing.Point(84, 8);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(14, 17);
		this.label6.TabIndex = 85;
		this.label6.Text = "*";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.Red;
		this.label23.Location = new System.Drawing.Point(89, 35);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(14, 17);
		this.label23.TabIndex = 85;
		this.label23.Text = "*";
		this.TxtAffectiveDateTo.CustomFormat = "dd-MMM-yyy";
		this.TxtAffectiveDateTo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.TxtAffectiveDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.TxtAffectiveDateTo.Location = new System.Drawing.Point(436, 32);
		this.TxtAffectiveDateTo.Name = "TxtAffectiveDateTo";
		this.TxtAffectiveDateTo.Size = new System.Drawing.Size(136, 23);
		this.TxtAffectiveDateTo.TabIndex = 3;
		this.txtAffectiveDate.CustomFormat = "dd-MMM-yyy";
		this.txtAffectiveDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAffectiveDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtAffectiveDate.Location = new System.Drawing.Point(436, 5);
		this.txtAffectiveDate.Name = "txtAffectiveDate";
		this.txtAffectiveDate.Size = new System.Drawing.Size(136, 23);
		this.txtAffectiveDate.TabIndex = 2;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(6, 9);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(74, 15);
		this.label1.TabIndex = 0;
		this.label1.Text = "Policy Name";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(315, 9);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(111, 15);
		this.label4.TabIndex = 0;
		this.label4.Text = "Effective Date From";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(573, 9);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(52, 15);
		this.label7.TabIndex = 0;
		this.label7.Text = "Remarks";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(5, 36);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(86, 15);
		this.label22.TabIndex = 1;
		this.label22.Text = "Analysis Group";
		this.CmbAnalysisGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbAnalysisGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAnalysisGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAnalysisGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance37;
		((SpecialBoxBase)((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance42).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance43).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbAnalysisGroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup).Location = new System.Drawing.Point(104, 30);
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup).Name = "CmbAnalysisGroup";
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup).Leave += new System.EventHandler(CmbAnalysisGroup_Leave);
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.btn);
		this.panel1.Controls.Add(this.label21);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(927, 53);
		this.panel1.TabIndex = 38;
		this.btn.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btn.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.btn.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.btnNew, this.btnRefresh, this.btnSave, this.btnUpdate, this.btnDelete, this.btnattachment, this.toolStripSeparator, this.BtnShortCutkeys });
		this.btn.Location = new System.Drawing.Point(0, 0);
		this.btn.Name = "btn";
		this.btn.Size = new System.Drawing.Size(927, 27);
		this.btn.TabIndex = 34;
		this.btn.Text = "toolStrip1";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
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
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.White;
		this.label21.Location = new System.Drawing.Point(3, 30);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(365, 19);
		this.label21.TabIndex = 1;
		this.label21.Text = "Lab Analysis Standard Policy for Deductions";
		this.tabHistory.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.tabHistory.Controls.Add(this.panel9);
		this.tabHistory.Controls.Add(this.panel3);
		this.tabHistory.Controls.Add(this.toolStrip2);
		this.tabHistory.Controls.Add(this.panel13);
		this.tabHistory.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail);
		this.tabHistory.Location = new System.Drawing.Point(4, 4);
		this.tabHistory.Name = "tabHistory";
		this.tabHistory.Padding = new System.Windows.Forms.Padding(3);
		this.tabHistory.Size = new System.Drawing.Size(933, 482);
		this.tabHistory.TabIndex = 1;
		this.tabHistory.Text = "History";
		this.tabHistory.UseVisualStyleBackColor = true;
		this.grdhistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdhistory).BackColor = System.Drawing.Color.White;
		grdhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistory.DesignTimeLayout = grdhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistory.DynamicFiltering = true;
		this.grdhistory.FilterMode = (FilterMode)2;
		this.grdhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(3, 126);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(927, 176);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 51;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.grdhistory.SelectionChanged += new System.EventHandler(grdhistory_SelectionChanged);
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdhistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdhistory_KeyDown);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label55);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 104);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(927, 22);
		this.panel9.TabIndex = 54;
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label55.ForeColor = System.Drawing.Color.White;
		this.label55.Location = new System.Drawing.Point(1, 1);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(132, 18);
		this.label55.TabIndex = 0;
		this.label55.Text = "Filtered Records";
		this.panel3.Controls.Add(this.panel14);
		this.panel3.Controls.Add(this.groupBox2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 30);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(927, 74);
		this.panel3.TabIndex = 50;
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.label12);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(927, 25);
		this.panel14.TabIndex = 50;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label12.ForeColor = System.Drawing.Color.White;
		this.label12.Location = new System.Drawing.Point(3, 3);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(207, 19);
		this.label12.TabIndex = 0;
		this.label12.Text = "Deduction Policy History";
		this.groupBox2.Controls.Add(this.ToDateHistory);
		this.groupBox2.Controls.Add(this.label20);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.ultraCombo2);
		this.groupBox2.Controls.Add(this.rdapproveddate);
		this.groupBox2.Controls.Add(this.rdmodifydate);
		this.groupBox2.Controls.Add(this.rdentrydate);
		this.groupBox2.Controls.Add(this.btnShow);
		this.groupBox2.Controls.Add(this.label53);
		this.groupBox2.Controls.Add(this.label54);
		this.groupBox2.Controls.Add(this.FromDateHistory);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(2, 25);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(948, 48);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(246, 16);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(134, 23);
		this.ToDateHistory.TabIndex = 6;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(383, 20);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(86, 15);
		this.label20.TabIndex = 15;
		this.label20.Text = "Analysis Group";
		this.ultraCombo2.AllowNull = (DefaultableBoolean)1;
		this.ultraCombo2.AutoCompleteMode = (AutoCompleteMode)4;
		this.ultraCombo2.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.ultraCombo2.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Appearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo2).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance30;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo2).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance22).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).TextHAlignAsString = "Left";
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.ultraCombo2).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.ultraCombo2.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.ultraCombo2).Location = new System.Drawing.Point(470, 14);
		((System.Windows.Forms.Control)(object)this.ultraCombo2).Name = "ultraCombo2";
		((System.Windows.Forms.Control)(object)this.ultraCombo2).Size = new System.Drawing.Size(216, 26);
		((System.Windows.Forms.Control)(object)this.ultraCombo2).TabIndex = 14;
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(835, 11);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 9;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(752, 28);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Checked = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(752, 11);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 7;
		this.rdentrydate.TabStop = true;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(689, 15);
		this.btnShow.Margin = new System.Windows.Forms.Padding(2);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(58, 25);
		this.btnShow.TabIndex = 13;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(199, 20);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(48, 15);
		this.label53.TabIndex = 5;
		this.label53.Text = "To Date";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(1, 20);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(63, 15);
		this.label54.TabIndex = 3;
		this.label54.Text = "From Date";
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(65, 16);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(134, 23);
		this.FromDateHistory.TabIndex = 4;
		this.toolStrip2.BackColor = System.Drawing.Color.Transparent;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnResetHistory });
		this.toolStrip2.Location = new System.Drawing.Point(3, 3);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(927, 27);
		this.toolStrip2.TabIndex = 52;
		this.toolStrip2.Text = "toolStrip2";
		this.btnResetHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetHistory.Name = "btnResetHistory";
		this.btnResetHistory.Size = new System.Drawing.Size(68, 24);
		this.btnResetHistory.Text = "Reset";
		this.btnResetHistory.Click += new System.EventHandler(btnResetHistory_Click);
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label49);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel13.Location = new System.Drawing.Point(3, 302);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(927, 19);
		this.panel13.TabIndex = 55;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label49.ForeColor = System.Drawing.Color.White;
		this.label49.Location = new System.Drawing.Point(2, 3);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(269, 14);
		this.label49.TabIndex = 0;
		this.label49.Text = "Detail Information Of Above Selected Row";
		this.DataGridHistoryDetail.AllowDelete = (InheritableBoolean)1;
		this.DataGridHistoryDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.DataGridHistoryDetail).BackColor = System.Drawing.Color.White;
		DataGridHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistoryDetail.DesignTimeLayout = DataGridHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.DataGridHistoryDetail.DynamicFiltering = true;
		this.DataGridHistoryDetail.FilterMode = (FilterMode)2;
		this.DataGridHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Location = new System.Drawing.Point(3, 321);
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Name = "DataGridHistoryDetail";
		((ScrollableBase)this.DataGridHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Size = new System.Drawing.Size(927, 158);
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).TabIndex = 53;
		this.DataGridHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.DataGridHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistoryDetail).VisualStyle = (VisualStyle)4;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(941, 511);
		base.Controls.Add(this.tabControl1);
		base.Name = "frmQcDeductionPolicy";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmQcDeductionPolicy_FormClosing);
		base.Load += new System.EventHandler(frmQcDeductionPolicy_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmQcDeductionPolicy_KeyDown);
		this.tabControl1.ResumeLayout(false);
		this.tabForm.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAnalysisParameter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeductionWeightUom).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAnalysisGroup).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.btn.ResumeLayout(false);
		this.btn.PerformLayout();
		this.tabHistory.ResumeLayout(false);
		this.tabHistory.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo2).EndInit();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistoryDetail).EndInit();
		base.ResumeLayout(false);
	}
}
