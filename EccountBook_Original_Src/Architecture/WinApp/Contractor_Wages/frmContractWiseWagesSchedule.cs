using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.ContractorWages;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.ContractorWages;
using Architecture.Model.Inventory;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Contractor_Wages;

public class frmContractWiseWagesSchedule : Form
{
	private int RecId;

	public bool UpdateMode;

	private DropDownBind DDL = new DropDownBind();

	public UserAccount UserAccount = new UserAccount();

	private DataTable dtContr = new DataTable();

	private Rightsobjects formright = new Rightsobjects();

	private bool SubsidiaryAccountAllownOnVouchers;

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private GroupBox groupBox2;

	private Label label1;

	private TextBox txtwagesrate;

	private Label label13;

	private Label label10;

	private Label label;

	private DateTimePicker txtdate;

	internal UltraCombo combAccountName;

	private TextBox txtPackUOMTO;

	private TextBox txtPackUOMFrom;

	private Label label9;

	private Label label18;

	private Label label11;

	private Label label12;

	private Label label16;

	private Label label17;

	private Panel panel6;

	private Label label15;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripSeparator toolStripSeparator;

	private TabPage tabPage2;

	private DateTimePicker txtdateto;

	private Label label2;

	private Label label3;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdWagesSchedule;

	private Panel panel4;

	private Label label5;

	internal UltraCombo Cmbcontractor;

	private Label label6;

	private TextBox txtCompanyRate;

	private Label label8;

	private Label label7;

	private Panel panel8;

	private Button btnshowHistory;

	private DateTimePicker txtToDateHistory;

	private Label label20;

	private DateTimePicker FromDateHistory;

	private Label label21;

	internal UltraCombo CmbWagesAccountHistory;

	private Label label22;

	internal UltraCombo CmbContractoryHistory;

	private Label label23;

	private Label label24;

	private Panel panel12;

	private CtrlGrdBar ctrlGrdBar3;

	private ToolStrip toolStrip2;

	private ToolStripButton BtnRefreshHistory;

	internal GridEX gridhistory;

	private Panel panel1;

	private Label label4;

	private ToolStripButton BtnNewHistory;

	private ToolStripButton btnRefresh;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripButton btnshortcutkeys;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton btnWagesAccount;

	public frmContractWiseWagesSchedule(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmContractWagesSchedule_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(910, 430);
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar3.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar3.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar3.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar3.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar3.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar3.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ContractorFill();
			ComboBindFromWagesSchedule();
			if (Conversion.ToInt(combAccountName.Value) > 0 || Conversion.ToInt(Cmbcontractor.Value) > 0)
			{
				BindgrdWagesSchedule(Conversion.ToInt(combAccountName.Value), Conversion.ToInt(Cmbcontractor.Value));
			}
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			txtCompanyRate.Enabled = false;
			txtdate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ContractorFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		try
		{
			int ItId = Conversion.ToInt(Cmbcontractor.Value);
			SupplierCustomer supplier = new SupplierCustomer();
			supplier.OrganizationId = UserAccount.OrganizationId;
			supplier.CompanyId = UserAccount.CompanyId;
			dtContr = SupplierCustomer.ReadByOrganizationCompanyIdForContractorWages(supplier);
			if (dtContr.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtContr, Cmbcontractor, "Id", "CompanyName", "Contractor Name", true);
				((UltraGridBase)Cmbcontractor).DisplayLayout.Bands[0].Columns[1].Width = 350;
				if (ItId > 0)
				{
					if (dtContr.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId))
					{
						Cmbcontractor.Value = ItId;
					}
					else
					{
						((Control)(object)Cmbcontractor).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)Cmbcontractor).Text = string.Empty;
				((UltraGridBase)Cmbcontractor).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboBindFromWagesSchedule()
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		try
		{
			int IdContr = Conversion.ToInt(CmbContractoryHistory.Value);
			int IdWageAC = Conversion.ToInt(combAccountName.Value);
			int IdWageAcH = Conversion.ToInt(CmbWagesAccountHistory.Value);
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ActionId = 1
			};
			DataTable dtContractor = new DataTable
			{
				Columns = 
				{
					{
						"Id",
						typeof(int)
					},
					"name"
				}
			};
			DataTable dtwagAc = new DataTable
			{
				Columns = 
				{
					{
						"Id",
						typeof(int)
					},
					"name"
				}
			};
			DataTable dt = InvContractorWagesSchedule.DropDownFillFromWagesSchedule(val);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Contractor")
					{
						dtContractor.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "WagesAccount")
					{
						dtwagAc.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
				if (dtContractor.Rows.Count > 0)
				{
					DDL.BindDDL(dtContractor, CmbContractoryHistory, "Id", "name", "Contractor Name", ZeroIndex: true);
					if (IdContr > 0)
					{
						if (dtContractor.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == IdContr))
						{
							CmbContractoryHistory.Value = IdContr;
						}
						else
						{
							((Control)(object)CmbContractoryHistory).Text = string.Empty;
						}
					}
				}
				else
				{
					((UltraGridBase)CmbContractoryHistory).DataSource = null;
				}
				if (dtwagAc.Rows.Count > 0)
				{
					DDL.BindDDL(dtwagAc, CmbWagesAccountHistory, "Id", "name", "Wages Account", ZeroIndex: true);
					DDL.BindDDL(dtwagAc, combAccountName, "Id", "name", "Wages Account", ZeroIndex: true);
					if (IdWageAC > 0)
					{
						if (dtwagAc.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == IdWageAC))
						{
							combAccountName.Value = IdWageAC;
						}
						else
						{
							((Control)(object)combAccountName).Text = string.Empty;
						}
					}
					if (IdWageAcH > 0)
					{
						if (dtwagAc.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == IdWageAcH))
						{
							CmbWagesAccountHistory.Value = IdWageAcH;
						}
						else
						{
							((Control)(object)CmbWagesAccountHistory).Text = string.Empty;
						}
					}
				}
				else
				{
					((UltraGridBase)combAccountName).DataSource = null;
					((UltraGridBase)CmbWagesAccountHistory).DataSource = null;
				}
			}
			else
			{
				((UltraGridBase)CmbContractoryHistory).DataSource = null;
				((UltraGridBase)combAccountName).DataSource = null;
				((UltraGridBase)CmbWagesAccountHistory).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPackUOMFrom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(combAccountName.Value) > 0 || Conversion.ToDouble((object)txtPackUOMFrom.Text) > 0.0)
			{
				double WagesRate = 0.0;
				GetWagesSchedule d = CommonServices.GetWagesRate(Conversion.ToDateTime((object)txtdate.Value), Conversion.ToDouble((object)txtPackUOMFrom.Text), Conversion.ToInt(combAccountName.Value), Conversion.ToInt(Cmbcontractor.Value));
				if (d != null)
				{
					WagesRate = d.WagesRate;
				}
				if (WagesRate > 0.0)
				{
					txtCompanyRate.Text = WagesRate.ToString(clsGlobalVariables.DecimalRateFormate);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Cmbcontractor_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(combAccountName.Value) > 0 || Conversion.ToInt(Cmbcontractor.Value) > 0)
			{
				BindgrdWagesSchedule(Conversion.ToInt(combAccountName.Value), Conversion.ToInt(Cmbcontractor.Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combAccountName_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(combAccountName.Value) > 0 || Conversion.ToDouble((object)txtPackUOMFrom.Text) > 0.0)
			{
				double WagesRate = 0.0;
				GetWagesSchedule d = CommonServices.GetWagesRate(Conversion.ToDateTime((object)txtdate.Value), Conversion.ToDouble((object)txtPackUOMFrom.Text), Conversion.ToInt(combAccountName.Value), Conversion.ToInt(Cmbcontractor.Value));
				if (d != null)
				{
					WagesRate = d.WagesRate;
				}
				if (WagesRate > 0.0)
				{
					txtCompanyRate.Text = WagesRate.ToString(clsGlobalVariables.DecimalRateFormate);
				}
			}
			if (Conversion.ToInt(combAccountName.Value) > 0 || Conversion.ToInt(Cmbcontractor.Value) > 0)
			{
				BindgrdWagesSchedule(Conversion.ToInt(combAccountName.Value), Conversion.ToInt(Cmbcontractor.Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)Cmbcontractor).ActiveRow == null || Conversion.ToInt(Cmbcontractor.Value) == 0)
		{
			MessageBox.Show("contractor Field is Required");
			Cmbcontractor.Focus();
			return false;
		}
		if (((UltraGridBase)combAccountName).ActiveRow == null || Conversion.ToInt(combAccountName.Value) == 0)
		{
			MessageBox.Show("Account Field is Required");
			combAccountName.Focus();
			return false;
		}
		if (txtPackUOMFrom.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtPackUOMFrom.Text) == 0.0)
		{
			MessageBox.Show("PackUOMFrom Field Required");
			txtPackUOMFrom.Focus();
			return false;
		}
		if (txtPackUOMTO.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtPackUOMTO.Text) == 0.0)
		{
			MessageBox.Show("PackUOMTo Field Required");
			txtPackUOMTO.Focus();
			return false;
		}
		if (txtwagesrate.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtwagesrate.Text) == 0.0)
		{
			MessageBox.Show("Wages Rate Field Required");
			txtwagesrate.Focus();
			return false;
		}
		return true;
	}

	private void btnshowHistory_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Text == "")
			{
				FromDateHistory.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtwagesrate_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Text == "")
			{
				txtdate.Focus();
			}
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
			ContractorFill();
			ComboBindFromWagesSchedule();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmContractWagesSchedule_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void FormRest()
	{
		try
		{
			RecId = 0;
			txtPackUOMTO.Text = "0";
			txtwagesrate.Text = "0";
			txtPackUOMFrom.Text = "0";
			Cmbcontractor.Focus();
			grdWagesSchedule.ClearStructure();
			btnUpdate.Visible = false;
			btnsave.Visible = true;
			UpdateMode = false;
			if (Conversion.ToInt(combAccountName.Value) > 0 || Conversion.ToInt(Cmbcontractor.Value) > 0)
			{
				BindgrdWagesSchedule(Conversion.ToInt(combAccountName.Value), Conversion.ToInt(Cmbcontractor.Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		FormRest();
	}

	private void BindgrdWagesSchedule(int WagesAccountId = 0, int ContractorId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		try
		{
			DataTable dtHistory = InvContractorWagesSchedule.GetAll(new InvContractorWagesSchedule
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				InvConractorWagesAccountsId = WagesAccountId,
				ContractorId = ContractorId,
				ActionId = 1
			})?.Tables[0];
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("EffectedDate", typeof(DateTime));
				dt.Columns.Add("EffectedDateTo", typeof(DateTime));
				dt.Columns.Add("InvConractorWagesAccountsId", typeof(int));
				dt.Columns.Add("WagesAccountName", typeof(string));
				dt.Columns.Add("ContractorId", typeof(int));
				dt.Columns.Add("ContractName", typeof(string));
				dt.Columns.Add("PackUomFrom", typeof(string));
				dt.Columns.Add("PackUomTo", typeof(string));
				dt.Columns.Add("RateUom", typeof(string));
				dt.Columns.Add("CompanyRate", typeof(decimal));
				dt.Columns.Add("WagesRate", typeof(decimal));
				dt.Columns.Add("IsApproved", typeof(bool));
				dt.Columns.Add("EntryUserId", typeof(string));
				dt.Columns.Add("EnteredBy", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("ModifyBy", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ApprovedBy", typeof(string));
				dt.Columns.Add("ApprovedDate", typeof(DateTime));
				foreach (DataRow row in dtHistory.Rows)
				{
					dt.Rows.Add(row["Id"], row["EffectedDate"], row["EffectedDateTo"], row["InvConractorWagesAccountsId"], row["WagesAccountName"], row["ContractorId"], row["ContractName"], row["PackUomFrom"], row["PackUomTo"], row["RateUom"], row["CompanyRate"], row["WageRate"], row["IsApproved"], row["EntryUserId"], row["EntryUser"], row["EntryDate"], row["ModifyUser"], row["ModifyDate"], row["ApprovedUser"], row["ApprovedDate"]);
				}
				grdWagesSchedule.DataSource = dt;
				grdWagesSchedule.RetrieveStructure();
				grdScheduleSetings();
			}
			else
			{
				grdWagesSchedule.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdScheduleSetings()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "InvConractorWagesAccountsId", "ContractorId", "RateUom", "CompanyRate", "IsApproved", "EntryUserId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdWagesSchedule, 2, 2, hiddenColumns);
			grdWagesSchedule.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdWagesSchedule.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdWagesSchedule.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdWagesSchedule.RootTable.Columns["PackUomFrom"].Width = Constants.InventoryConstants.UomConstant + 20;
			grdWagesSchedule.RootTable.Columns["PackUomTo"].Width = Constants.InventoryConstants.UomConstant + 20;
			grdWagesSchedule.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdWagesSchedule.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdWagesSchedule.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdWagesSchedule.RootTable.Columns["WagesAccountName"].Caption = "Labour / Wages Activity";
			GridEX_Helper.AddButton(grdWagesSchedule, "Edit", "Edit", 40, 0);
			GridEX_Helper.AddButton(grdWagesSchedule, "Approve/UnApprove", "Approve", 70, ((DataTable)grdWagesSchedule.DataSource).Columns.Count - 1);
			grdWagesSchedule.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdWagesSchedule_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdWagesSchedule.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				RecId = Conversion.ToInt(r.Cells["Id"].Value);
				ReadbyId(RecId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdWagesSchedule_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdWagesSchedule.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int Id = Conversion.ToInt(r.Cells["Id"].Value);
				int UserId = Conversion.ToInt(r.Cells["EntryUserId"].Value);
				bool Approved = Conversion.ToBool(r.Cells["IsApproved"].Value);
				if (e.Column.Key == "Edit")
				{
					RecId = Id;
					ReadbyId(RecId);
				}
				else if (e.Column.Key == "Approve/UnApprove" && !Approved)
				{
					ApproveUnApprove(Id, "Approve", UserId, "Form");
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ApproveUnApprove(int Id, string ReqType, int EntryUserId, string RequestByFormOrHistory = "")
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Expected O, but got Unknown
		try
		{
			if (UserAccount.RoleName != "Admin" && EntryUserId == UserAccount.ID)
			{
				throw new Exception("You can't update Status!");
			}
			if (MessageBox.Show("Are you sure to " + ReqType + "?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList detail = new ApprovalList();
				detail.OrganizationId = UserAccount.OrganizationId;
				detail.CompanyId = UserAccount.CompanyId;
				detail.EntryUser = UserAccount.ID;
				detail.Id = Id;
				detail.ReqType = ((ReqType == "Approve") ? "Ap" : ((ReqType == "UnApprove") ? "UP" : null));
				val.AllApprovalLists.Add(detail);
				InvContractorWagesSchedule.ApproveUnApprove(val);
				MessageBox.Show("Record Updated Successfully!");
				if (RequestByFormOrHistory == "History")
				{
					BindGridHistory();
				}
				if (RequestByFormOrHistory == "Form")
				{
					BindgrdWagesSchedule(Conversion.ToInt(combAccountName.Value));
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
			InvContractorWagesSchedule wagesSchedule = new InvContractorWagesSchedule();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				wagesSchedule.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			wagesSchedule.OrganizationId = UserAccount.OrganizationId;
			wagesSchedule.CompanyId = UserAccount.CompanyId;
			wagesSchedule.EntryUserId = UserAccount.ID;
			wagesSchedule.ModifyUserId = UserAccount.ID;
			wagesSchedule.EffectedDate = Conversion.ToDateTime((object)txtdate.Value);
			wagesSchedule.EffectedDateTo = Conversion.ToDateTime((object)txtdateto.Value);
			wagesSchedule.ContractorId = Conversion.ToInt(Cmbcontractor.Value);
			wagesSchedule.InvConractorWagesAccountsId = Conversion.ToInt(combAccountName.Value);
			wagesSchedule.PackUomTo = Conversion.ToDouble((object)txtPackUOMTO.Text);
			wagesSchedule.PackUomFrom = Conversion.ToDouble((object)txtPackUOMFrom.Text);
			wagesSchedule.RateUom = Conversion.ToDouble((object)txtPackUOMFrom.Text);
			wagesSchedule.WageRate = Conversion.ToDouble((object)txtwagesrate.Text);
			wagesSchedule.CompanyRate = Conversion.ToDouble((object)txtCompanyRate.Text);
			int Success = InvContractorWagesSchedule.Save(wagesSchedule);
			wagesSchedule.Id = Success;
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully.");
			}
			else
			{
				MessageBox.Show("Data Save Successfully.");
			}
			FormRest();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ReadbyId(int Id)
	{
		try
		{
			RecId = Id;
			InvContractorWagesSchedule obj = InvContractorWagesSchedule.GetByID(RecId);
			if (obj != null)
			{
				tabControl1.SelectedIndex = 0;
				txtdate.Value = Conversion.ToDateTime((object)obj.EffectedDate);
				txtdateto.Value = Conversion.ToDateTime((object)obj.EffectedDateTo);
				combAccountName.Value = Conversion.ToInt((object)obj.InvConractorWagesAccountsId);
				Cmbcontractor.Value = Conversion.ToInt((object)obj.ContractorId);
				txtPackUOMFrom.Text = obj.PackUomFrom.ToString();
				txtPackUOMTO.Text = obj.PackUomTo.ToString();
				txtCompanyRate.Text = obj.CompanyRate.ToString();
				txtwagesrate.Text = obj.WageRate.ToString();
				UpdateMode = true;
				btnsave.Visible = false;
				btnUpdate.Visible = true;
				BindgrdWagesSchedule(Conversion.ToInt(combAccountName.Value), Conversion.ToInt(Cmbcontractor.Value));
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdWagesSchedule).Name);
			if (fs != null)
			{
				grdWagesSchedule.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)gridhistory).Name);
			if (fs != null)
			{
				gridhistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabWagesSchedule_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			FromDateHistory.Focus();
		}
		else
		{
			txtdate.Focus();
		}
	}

	private void Resethistory()
	{
		try
		{
			FromDateHistory.Text = string.Empty;
			txtToDateHistory.Text = string.Empty;
			((Control)(object)CmbContractoryHistory).Text = string.Empty;
			((Control)(object)CmbWagesAccountHistory).Text = string.Empty;
			FromDateHistory.Focus();
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
			Resethistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			ComboBindFromWagesSchedule();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			BindGridHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindGridHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			InvContractorWagesSchedule obj = new InvContractorWagesSchedule();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			if (FromDateHistory.Checked)
			{
				obj.EffectedDate = Conversion.ToDateTime((object)FromDateHistory.Value);
			}
			if (txtToDateHistory.Checked)
			{
				obj.EffectedDateTo = Conversion.ToDateTime((object)txtToDateHistory.Value);
			}
			obj.ContractorId = Conversion.ToInt(CmbContractoryHistory.Value);
			obj.InvConractorWagesAccountsId = Conversion.ToInt(CmbWagesAccountHistory.Value);
			obj.ActionId = 1;
			DataTable dtHistory = InvContractorWagesSchedule.GetAll(obj)?.Tables[0];
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("EffectedDate", typeof(DateTime));
				dt.Columns.Add("EffectedDateTo", typeof(DateTime));
				dt.Columns.Add("InvConractorWagesAccountsId", typeof(int));
				dt.Columns.Add("WagesAccountName", typeof(string));
				dt.Columns.Add("ContractorId", typeof(int));
				dt.Columns.Add("ContractName", typeof(string));
				dt.Columns.Add("PackUomFrom", typeof(string));
				dt.Columns.Add("PackUomTo", typeof(string));
				dt.Columns.Add("RateUom", typeof(string));
				dt.Columns.Add("CompanyRate", typeof(decimal));
				dt.Columns.Add("WagesRate", typeof(decimal));
				dt.Columns.Add("IsApproved", typeof(bool));
				dt.Columns.Add("EntryUserId", typeof(string));
				dt.Columns.Add("EnteredBy", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("ModifyBy", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ApprovedBy", typeof(string));
				dt.Columns.Add("ApprovedDate", typeof(DateTime));
				foreach (DataRow row in dtHistory.Rows)
				{
					dt.Rows.Add(row["Id"], row["EffectedDate"], row["EffectedDateTo"], row["InvConractorWagesAccountsId"], row["WagesAccountName"], row["ContractorId"], row["ContractName"], row["PackUomFrom"], row["PackUomTo"], row["RateUom"], row["CompanyRate"], row["WageRate"], row["IsApproved"], row["EntryUserId"], row["EntryUser"], row["EntryDate"], row["ModifyUser"], row["ModifyDate"], row["ApprovedUser"], row["ApprovedDate"]);
				}
				gridhistory.DataSource = dt;
				gridhistory.RetrieveStructure();
				HistorySetings();
			}
			else
			{
				gridhistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistorySetings()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "InvConractorWagesAccountsId", "ContractorId", "RateUom", "CompanyRate", "IsApproved", "EntryUserId" };
			GridEX_Helper.GridWrappingAndColumnSettings(gridhistory, 2, 2, hiddenColumns);
			gridhistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			gridhistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			gridhistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			gridhistory.RootTable.Columns["PackUomFrom"].Width = Constants.InventoryConstants.UomConstant + 20;
			gridhistory.RootTable.Columns["PackUomTo"].Width = Constants.InventoryConstants.UomConstant + 20;
			gridhistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			gridhistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			gridhistory.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			gridhistory.RootTable.Columns["WagesAccountName"].Caption = "Labour / Wages Activity";
			GridEX_Helper.AddButton(gridhistory, "Edit", "Edit", 40, 0);
			GridEX_Helper.AddButton(gridhistory, "Approve/UnApprove", "Approve", 70, ((DataTable)gridhistory.DataSource).Columns.Count - 1);
			gridhistory.FrozenColumns = 1;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void gridhistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = gridhistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				RecId = Conversion.ToInt(r.Cells["Id"].Value);
				ReadbyId(RecId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void gridhistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = gridhistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int Id = Conversion.ToInt(r.Cells["Id"].Value);
				int UserId = Conversion.ToInt(r.Cells["EntryUserId"].Value);
				bool Approved = Conversion.ToBool(r.Cells["IsApproved"].Value);
				if (e.Column.Key == "Edit")
				{
					RecId = Conversion.ToInt(r.Cells["Id"].Value);
					ReadbyId(RecId);
				}
				else if (e.Column.Key == "Approve/UnApprove" && !Approved)
				{
					ApproveUnApprove(Id, "Approve", UserId, "History");
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtPackUOMFrom_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void txtPackUOMTO_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void txtCompanyRate_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void txtwagesrate_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void frmContractWagesSchedule_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					txtdate.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					FromDateHistory.Focus();
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
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
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					txtdate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdWagesSchedule).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					Cmbcontractor.Focus();
				}
				if (e.Control)
				{
					_ = e.KeyCode;
					_ = 121;
				}
				if (e.Control)
				{
					_ = e.KeyCode;
					_ = 80;
				}
				if (e.Alt && e.KeyCode != Keys.NumPad1 && e.KeyCode != Keys.D1)
				{
				}
			}
			else
			{
				if (e.Control && e.KeyCode == Keys.S)
				{
					btnshowHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					FromDateHistory.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					FromDateHistory.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)gridhistory).Focus();
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
			dt.Rows.Add("Alt+3", "For Print Voucher 103");
			dt.Rows.Add("Ctrl+F5", "For Focus on DocDate");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+D", "For Adding an row in Focused Grid");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
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

	private void btnShortcutKeys_Click(object sender, EventArgs e)
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

	private void grdWagesSchedule_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdWagesSchedule.CurrentRow;
			if (item != null && (int)item.RowType == 0 && grdWagesSchedule.CurrentColumn != null)
			{
				if (e.Control && e.KeyCode == Keys.Space && grdWagesSchedule.CurrentColumn != null && grdWagesSchedule.CurrentColumn.Key == "Edit")
				{
					grdWagesSchedule_DoubleClick(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Return)
				{
					grdWagesSchedule_DoubleClick(null, null);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridhistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = gridhistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && gridhistory.CurrentColumn != null)
			{
				if (e.Control && e.KeyCode == Keys.Space && gridhistory.CurrentColumn != null && gridhistory.CurrentColumn.Key == "Edit")
				{
					gridhistory_DoubleClick(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Return)
				{
					gridhistory_DoubleClick(null, null);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnWagesAccount_Click(object sender, EventArgs e)
	{
		try
		{
			new frmContractorWagesAccount(UserAccount).Show();
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
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Expected O, but got Unknown
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Expected O, but got Unknown
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Expected O, but got Unknown
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Expected O, but got Unknown
		//IL_076e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0778: Expected O, but got Unknown
		//IL_26b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_26be: Expected O, but got Unknown
		GridEXLayout grdWagesSchedule_DesignTimeLayout = new GridEXLayout();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
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
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Contractor_Wages.frmContractWiseWagesSchedule));
		GridEXLayout gridhistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance13 = new Appearance();
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
		Appearance appearance24 = new Appearance();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grdWagesSchedule = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel2 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.txtdateto = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtCompanyRate = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.txtwagesrate = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label = new System.Windows.Forms.Label();
		this.txtdate = new System.Windows.Forms.DateTimePicker();
		this.Cmbcontractor = new UltraCombo();
		this.combAccountName = new UltraCombo();
		this.txtPackUOMTO = new System.Windows.Forms.TextBox();
		this.txtPackUOMFrom = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.gridhistory = new GridEX();
		this.panel1 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label4 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.btnshowHistory = new System.Windows.Forms.Button();
		this.txtToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label20 = new System.Windows.Forms.Label();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label21 = new System.Windows.Forms.Label();
		this.CmbWagesAccountHistory = new UltraCombo();
		this.label22 = new System.Windows.Forms.Label();
		this.CmbContractoryHistory = new UltraCombo();
		this.label23 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.panel12 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnWagesAccount = new System.Windows.Forms.ToolStripButton();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdWagesSchedule).BeginInit();
		this.panel4.SuspendLayout();
		this.panel2.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.Cmbcontractor).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combAccountName).BeginInit();
		this.panel6.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.gridhistory).BeginInit();
		this.panel1.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWagesAccountHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContractoryHistory).BeginInit();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(832, 411);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabWagesSchedule_SelectedIndexChanged);
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grdWagesSchedule);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.toolStrip1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(824, 382);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.grdWagesSchedule.AllowEdit = (InheritableBoolean)2;
		grdWagesSchedule_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdWagesSchedule.DesignTimeLayout = grdWagesSchedule_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdWagesSchedule).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdWagesSchedule).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdWagesSchedule.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdWagesSchedule).Location = new System.Drawing.Point(3, 200);
		((System.Windows.Forms.Control)(object)this.grdWagesSchedule).Name = "grdWagesSchedule";
		((ScrollableBase)this.grdWagesSchedule).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdWagesSchedule).Size = new System.Drawing.Size(818, 179);
		((System.Windows.Forms.Control)(object)this.grdWagesSchedule).TabIndex = 0;
		this.grdWagesSchedule.TotalRow = (InheritableBoolean)1;
		this.grdWagesSchedule.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdWagesSchedule).VisualStyle = (VisualStyle)4;
		this.grdWagesSchedule.ColumnButtonClick += new ColumnActionEventHandler(grdWagesSchedule_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdWagesSchedule).DoubleClick += new System.EventHandler(grdWagesSchedule_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdWagesSchedule).KeyDown += new System.Windows.Forms.KeyEventHandler(grdWagesSchedule_KeyDown);
		this.panel4.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Controls.Add(this.ctrlGrdBar1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 179);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(818, 21);
		this.panel4.TabIndex = 70;
		this.label5.AutoSize = true;
		this.label5.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label5.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label5.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label5.Location = new System.Drawing.Point(3, 3);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(298, 17);
		this.label5.TabIndex = 0;
		this.label5.Text = "History of Selected Account or Contractor";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(765, -2);
		this.ctrlGrdBar1.MyGrid = this.grdWagesSchedule;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(47, 23);
		this.ctrlGrdBar1.TabIndex = 69;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.groupBox2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 57);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(818, 122);
		this.panel2.TabIndex = 67;
		this.groupBox2.Controls.Add(this.txtdateto);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add(this.txtCompanyRate);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add(this.txtwagesrate);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.label13);
		this.groupBox2.Controls.Add(this.label);
		this.groupBox2.Controls.Add(this.txtdate);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.Cmbcontractor);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.combAccountName);
		this.groupBox2.Controls.Add(this.txtPackUOMTO);
		this.groupBox2.Controls.Add(this.txtPackUOMFrom);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add(this.label18);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add(this.label16);
		this.groupBox2.Controls.Add(this.label17);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(6, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(871, 117);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main";
		this.txtdateto.CustomFormat = "dd-MMM-yyyy";
		this.txtdateto.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtdateto.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdateto.Location = new System.Drawing.Point(159, 40);
		this.txtdateto.Name = "txtdateto";
		this.txtdateto.Size = new System.Drawing.Size(153, 23);
		this.txtdateto.TabIndex = 1;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.Red;
		this.label2.Location = new System.Drawing.Point(267, 16);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(15, 20);
		this.label2.TabIndex = 24;
		this.label2.Text = "*";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(159, 19);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(94, 15);
		this.label3.TabIndex = 23;
		this.label3.Text = "Effected Date To";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Red;
		this.label1.Location = new System.Drawing.Point(685, 16);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(15, 20);
		this.label1.TabIndex = 21;
		this.label1.Text = "*";
		this.txtCompanyRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCompanyRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCompanyRate.Location = new System.Drawing.Point(315, 87);
		this.txtCompanyRate.Name = "txtCompanyRate";
		this.txtCompanyRate.Size = new System.Drawing.Size(135, 23);
		this.txtCompanyRate.TabIndex = 6;
		this.txtCompanyRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCompanyRate_KeyPress);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.Red;
		this.label8.Location = new System.Drawing.Point(541, 66);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(15, 20);
		this.label8.TabIndex = 19;
		this.label8.Text = "*";
		this.txtwagesrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtwagesrate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtwagesrate.Location = new System.Drawing.Point(453, 87);
		this.txtwagesrate.Name = "txtwagesrate";
		this.txtwagesrate.Size = new System.Drawing.Size(135, 23);
		this.txtwagesrate.TabIndex = 7;
		this.txtwagesrate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtwagesrate_KeyPress);
		this.txtwagesrate.Leave += new System.EventHandler(txtwagesrate_Leave);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(453, 69);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(69, 15);
		this.label7.TabIndex = 18;
		this.label7.Text = "Wages Rate";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.Red;
		this.label13.Location = new System.Drawing.Point(407, 66);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(15, 20);
		this.label13.TabIndex = 19;
		this.label13.Text = "*";
		this.label.AutoSize = true;
		this.label.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label.Location = new System.Drawing.Point(315, 69);
		this.label.Name = "label";
		this.label.Size = new System.Drawing.Size(84, 15);
		this.label.TabIndex = 18;
		this.label.Text = "Company Rate";
		this.txtdate.CustomFormat = "dd-MMM-yyyy";
		this.txtdate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdate.Location = new System.Drawing.Point(3, 40);
		this.txtdate.Name = "txtdate";
		this.txtdate.Size = new System.Drawing.Size(153, 23);
		this.txtdate.TabIndex = 0;
		this.Cmbcontractor.AllowNull = (DefaultableBoolean)1;
		this.Cmbcontractor.AutoCompleteMode = (AutoCompleteMode)4;
		this.Cmbcontractor.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.Cmbcontractor.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.Cmbcontractor).DisplayLayout.Appearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.Cmbcontractor).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.Cmbcontractor).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.Cmbcontractor).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.Cmbcontractor).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance39;
		((SpecialBoxBase)((UltraGridBase)this.Cmbcontractor).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.Cmbcontractor).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.Cmbcontractor).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.Cmbcontractor).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.Cmbcontractor).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.Cmbcontractor).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.Cmbcontractor).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.Cmbcontractor).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.Cmbcontractor).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance44).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.Cmbcontractor).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.Cmbcontractor).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.Cmbcontractor).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.Cmbcontractor).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).TextHAlignAsString = "Left";
		((UltraGridBase)this.Cmbcontractor).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.Cmbcontractor).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.Cmbcontractor).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.Cmbcontractor).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.Cmbcontractor).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.Cmbcontractor).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.Cmbcontractor).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.Cmbcontractor).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.Cmbcontractor).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.Cmbcontractor).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.Cmbcontractor.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.Cmbcontractor).Location = new System.Drawing.Point(315, 38);
		((System.Windows.Forms.Control)(object)this.Cmbcontractor).Name = "Cmbcontractor";
		((System.Windows.Forms.Control)(object)this.Cmbcontractor).Size = new System.Drawing.Size(273, 26);
		((System.Windows.Forms.Control)(object)this.Cmbcontractor).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.Cmbcontractor).Leave += new System.EventHandler(Cmbcontractor_Leave);
		this.combAccountName.AllowNull = (DefaultableBoolean)1;
		this.combAccountName.AutoCompleteMode = (AutoCompleteMode)4;
		this.combAccountName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combAccountName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combAccountName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.combAccountName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combAccountName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combAccountName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combAccountName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance51;
		((SpecialBoxBase)((UltraGridBase)this.combAccountName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combAccountName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.combAccountName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combAccountName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combAccountName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combAccountName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.combAccountName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combAccountName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combAccountName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance56).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combAccountName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.combAccountName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combAccountName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combAccountName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).TextHAlignAsString = "Left";
		((UltraGridBase)this.combAccountName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.combAccountName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combAccountName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combAccountName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.combAccountName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combAccountName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.combAccountName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combAccountName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combAccountName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combAccountName).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combAccountName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combAccountName).Location = new System.Drawing.Point(591, 38);
		((System.Windows.Forms.Control)(object)this.combAccountName).Name = "combAccountName";
		((System.Windows.Forms.Control)(object)this.combAccountName).Size = new System.Drawing.Size(273, 26);
		((System.Windows.Forms.Control)(object)this.combAccountName).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.combAccountName).Leave += new System.EventHandler(combAccountName_Leave);
		this.txtPackUOMTO.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackUOMTO.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPackUOMTO.Location = new System.Drawing.Point(159, 87);
		this.txtPackUOMTO.Name = "txtPackUOMTO";
		this.txtPackUOMTO.Size = new System.Drawing.Size(153, 23);
		this.txtPackUOMTO.TabIndex = 5;
		this.txtPackUOMTO.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPackUOMTO_KeyPress);
		this.txtPackUOMFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackUOMFrom.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPackUOMFrom.Location = new System.Drawing.Point(3, 87);
		this.txtPackUOMFrom.Name = "txtPackUOMFrom";
		this.txtPackUOMFrom.Size = new System.Drawing.Size(153, 23);
		this.txtPackUOMFrom.TabIndex = 4;
		this.txtPackUOMFrom.TextChanged += new System.EventHandler(txtPackUOMFrom_TextChanged);
		this.txtPackUOMFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPackUOMFrom_KeyPress);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(315, 19);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(98, 15);
		this.label6.TabIndex = 11;
		this.label6.Text = "Contractor Name";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.Red;
		this.label9.Location = new System.Drawing.Point(253, 66);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(15, 20);
		this.label9.TabIndex = 12;
		this.label9.Text = "*";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(588, 19);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(91, 15);
		this.label18.TabIndex = 11;
		this.label18.Text = "Wages Account";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.Red;
		this.label11.Location = new System.Drawing.Point(109, 66);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(15, 20);
		this.label11.TabIndex = 10;
		this.label11.Text = "*";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(3, 69);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(94, 15);
		this.label12.TabIndex = 9;
		this.label12.Text = "Pack UOM From";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.Red;
		this.label16.Location = new System.Drawing.Point(110, 16);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(15, 20);
		this.label16.TabIndex = 6;
		this.label16.Text = "*";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(3, 19);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(109, 15);
		this.label17.TabIndex = 4;
		this.label17.Text = "Effected Date From";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(159, 69);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(79, 15);
		this.label10.TabIndex = 20;
		this.label10.Text = "Pack UOM To";
		this.panel6.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel6.Controls.Add(this.label15);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 30);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(818, 27);
		this.panel6.TabIndex = 66;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(4, 4);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(277, 19);
		this.label15.TabIndex = 0;
		this.label15.Text = "Contractor Wise Wages Schedule";
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.btnnew, this.btnRefresh, this.toolStripSeparator2, this.btnsave, this.btnUpdate, this.toolStripSeparator, this.btnshortcutkeys, this.toolStripSeparator1, this.btnWagesAccount });
		this.toolStrip1.Location = new System.Drawing.Point(3, 3);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(818, 27);
		this.toolStrip1.TabIndex = 65;
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
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnShortcutKeys_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.gridhistory);
		this.tabPage2.Controls.Add(this.panel1);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(824, 382);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.gridhistory.AllowEdit = (InheritableBoolean)2;
		gridhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.gridhistory.DesignTimeLayout = gridhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.gridhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.gridhistory.FilterMode = (FilterMode)2;
		this.gridhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.gridhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.gridhistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.gridhistory).Location = new System.Drawing.Point(3, 155);
		((System.Windows.Forms.Control)(object)this.gridhistory).Name = "gridhistory";
		((ScrollableBase)this.gridhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.gridhistory).Size = new System.Drawing.Size(818, 224);
		((System.Windows.Forms.Control)(object)this.gridhistory).TabIndex = 1;
		this.gridhistory.TotalRow = (InheritableBoolean)1;
		this.gridhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.gridhistory).VisualStyle = (VisualStyle)4;
		this.gridhistory.ColumnButtonClick += new ColumnActionEventHandler(gridhistory_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.gridhistory).DoubleClick += new System.EventHandler(gridhistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.gridhistory).KeyDown += new System.Windows.Forms.KeyEventHandler(gridhistory_KeyDown);
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.ctrlGrdBar3);
		this.panel1.Controls.Add(this.label4);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 126);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(818, 29);
		this.panel1.TabIndex = 71;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(771, 0);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.gridhistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(43, 30);
		this.ctrlGrdBar3.TabIndex = 21;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label4.AutoSize = true;
		this.label4.BackColor = System.Drawing.Color.Teal;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.Transparent;
		this.label4.Location = new System.Drawing.Point(3, 5);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(142, 19);
		this.label4.TabIndex = 22;
		this.label4.Text = "Filtered Records";
		this.panel8.BackColor = System.Drawing.Color.White;
		this.panel8.Controls.Add(this.btnshowHistory);
		this.panel8.Controls.Add(this.txtToDateHistory);
		this.panel8.Controls.Add(this.label20);
		this.panel8.Controls.Add(this.FromDateHistory);
		this.panel8.Controls.Add(this.label21);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWagesAccountHistory);
		this.panel8.Controls.Add(this.label22);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.CmbContractoryHistory);
		this.panel8.Controls.Add(this.label23);
		this.panel8.Controls.Add(this.label24);
		this.panel8.Controls.Add(this.panel12);
		this.panel8.Controls.Add(this.toolStrip2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(818, 123);
		this.panel8.TabIndex = 0;
		this.btnshowHistory.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshowHistory.FlatAppearance.BorderSize = 0;
		this.btnshowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshowHistory.ForeColor = System.Drawing.Color.White;
		this.btnshowHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshowHistory.Location = new System.Drawing.Point(703, 92);
		this.btnshowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnshowHistory.Name = "btnshowHistory";
		this.btnshowHistory.Size = new System.Drawing.Size(68, 26);
		this.btnshowHistory.TabIndex = 4;
		this.btnshowHistory.Text = "Show";
		this.btnshowHistory.UseVisualStyleBackColor = false;
		this.btnshowHistory.Click += new System.EventHandler(btnshowHistory_Click);
		this.btnshowHistory.Leave += new System.EventHandler(btnshowHistory_Leave);
		this.txtToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.CustomFormat = "dd-MMM-yy";
		this.txtToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateHistory.Location = new System.Drawing.Point(76, 92);
		this.txtToDateHistory.Name = "txtToDateHistory";
		this.txtToDateHistory.ShowCheckBox = true;
		this.txtToDateHistory.Size = new System.Drawing.Size(183, 23);
		this.txtToDateHistory.TabIndex = 1;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(4, 96);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(48, 15);
		this.label20.TabIndex = 147;
		this.label20.Text = "Date To";
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(76, 63);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(183, 23);
		this.FromDateHistory.TabIndex = 0;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(4, 67);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(63, 15);
		this.label21.TabIndex = 146;
		this.label21.Text = "Date From";
		this.CmbWagesAccountHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbWagesAccountHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWagesAccountHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWagesAccountHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance27;
		((SpecialBoxBase)((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance32).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWagesAccountHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWagesAccountHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWagesAccountHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWagesAccountHistory).Location = new System.Drawing.Point(362, 90);
		((System.Windows.Forms.Control)(object)this.CmbWagesAccountHistory).Name = "CmbWagesAccountHistory";
		((System.Windows.Forms.Control)(object)this.CmbWagesAccountHistory).Size = new System.Drawing.Size(336, 26);
		((System.Windows.Forms.Control)(object)this.CmbWagesAccountHistory).TabIndex = 3;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(265, 96);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(91, 15);
		this.label22.TabIndex = 145;
		this.label22.Text = "Wages Account";
		this.CmbContractoryHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbContractoryHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbContractoryHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbContractoryHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbContractoryHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbContractoryHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbContractoryHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbContractoryHistory).Location = new System.Drawing.Point(362, 61);
		((System.Windows.Forms.Control)(object)this.CmbContractoryHistory).Name = "CmbContractoryHistory";
		((System.Windows.Forms.Control)(object)this.CmbContractoryHistory).Size = new System.Drawing.Size(336, 26);
		((System.Windows.Forms.Control)(object)this.CmbContractoryHistory).TabIndex = 2;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(265, 67);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(98, 15);
		this.label23.TabIndex = 144;
		this.label23.Text = "Contractor Name";
		this.label24.AutoSize = true;
		this.label24.BackColor = System.Drawing.Color.Teal;
		this.label24.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.Color.Transparent;
		this.label24.Location = new System.Drawing.Point(6, 33);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(204, 19);
		this.label24.TabIndex = 22;
		this.label24.Text = "Wages Schedule History";
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 27);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(818, 31);
		this.panel12.TabIndex = 23;
		this.toolStrip2.BackColor = System.Drawing.Color.White;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.BtnNewHistory, this.BtnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(818, 27);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.BtnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewHistory.Name = "BtnNewHistory";
		this.BtnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.BtnNewHistory.Text = "New";
		this.BtnNewHistory.Click += new System.EventHandler(btnResetHistory_Click);
		this.BtnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshHistory.Name = "BtnRefreshHistory";
		this.BtnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshHistory.Text = "&Refresh";
		this.BtnRefreshHistory.Click += new System.EventHandler(BtnRefreshHistory_Click);
		this.btnWagesAccount.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnWagesAccount.Name = "btnWagesAccount";
		this.btnWagesAccount.Size = new System.Drawing.Size(114, 24);
		this.btnWagesAccount.Text = "Wages Account";
		this.btnWagesAccount.Click += new System.EventHandler(btnWagesAccount_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(832, 411);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "frmContractWiseWagesSchedule";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Wages_Account_Schedule";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmContractWagesSchedule_FormClosing);
		base.Load += new System.EventHandler(frmContractWagesSchedule_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmContractWagesSchedule_KeyDown);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabPage1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdWagesSchedule).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.Cmbcontractor).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combAccountName).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.gridhistory).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWagesAccountHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContractoryHistory).EndInit();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
