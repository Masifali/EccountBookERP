using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.BLL.Production;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Production;
using Architecture.Reports;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Production;

public class frmBoilerConsumption : Form
{
	public UserAccount UserAccount = new UserAccount();

	private Rightsobjects formrights = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private Attachment AT = new Attachment();

	private DataTable dt = new DataTable();

	private int RecId;

	private IContainer components;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnAttachment;

	private ToolStripButton btnRefresh;

	private Label label1;

	private TextBox txtDocNo;

	private DateTimePicker txtdocdate;

	internal UltraCombo cmbBoilerName;

	private DateTimePicker txtOffTime;

	private DateTimePicker txtOnTime;

	private TextBox txtWaterWeightKG;

	private TextBox txtStartReading;

	private TextBox txtEndReading;

	private TextBox SteamWeightKG;

	private TextBox txtDrierSteamUse;

	private Label label18;

	private Label label17;

	private Label label16;

	private TextBox txtMoisture;

	private TextBox txtBagSize;

	private TextBox txtQty;

	private TextBox txtTWeightMann;

	private TextBox txtTWeightKG;

	private TextBox WeightPerVehicle;

	internal UltraCombo cmbVehicleType;

	private TextBox txtNoOfVehicles;

	private Label label15;

	private Label label14;

	private Label label13;

	private Label label12;

	private Label label11;

	private Label label10;

	private Label label9;

	private Label label8;

	private Label label7;

	private Label label6;

	private Label label5;

	private Label label4;

	private Label label3;

	private Label label2;

	internal GridEX grdHistory;

	private ToolStripButton btnPrint;

	private Button BtnAddBoilerName;

	private TextBox txtRemarks;

	private Label label19;

	private Panel panel1;

	private CtrlGrdBar ctrlGrdBar1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel4;

	private Panel panel6;

	internal GridEX GridRegister;

	private GroupBox groupBox1;

	private Label label23;

	private TextBox txtDocNoTo;

	private DateTimePicker txtDateFrom;

	private Label label24;

	internal UltraCombo CmbBoilerNameRegister;

	private Label label21;

	internal UltraCombo CmbVehicleTypeRegister;

	private Label label20;

	private TextBox txtDocNoFrom;

	private Label label25;

	private Label label22;

	private DateTimePicker txtDateTo;

	private Button BtnShow;

	private Panel panel7;

	private ToolStrip toolStrip2;

	private ToolStripButton toolStripButton1;

	private ToolStripButton toolStripButton6;

	private ToolStripButton toolStripButton7;

	private Panel panel3;

	private Panel panel9;

	private Panel panel8;

	private Panel panel5;

	public frmBoilerConsumption(UserAccount PuserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = PuserAccount;
	}

	private void BoilerName()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtbnk = BoilerName.GetAll(new InvBoilerName
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtbnk.Rows.Count > 0)
			{
				DDL.BindDDL(dtbnk, cmbBoilerName, "Id", "BoilerName", "Boiler Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocumentNoFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			int Code = BoilerConsumption.GenerateCode(new BoilerConsumption
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 141
			});
			if (Code > 0)
			{
				txtDocNo.Text = Code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VehicleTypesFill()
	{
		try
		{
			((UltraGridBase)cmbVehicleType).DataSource = VehicleType.GetAll();
			((UltraDropDownBase)cmbVehicleType).DisplayMember = "VehicleDescription";
			((UltraDropDownBase)cmbVehicleType).ValueMember = "Id";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool formvalidation()
	{
		if (txtDocNo.Text.Trim() == "" || txtDocNo.Text.Trim() == "0")
		{
			MessageBox.Show("Doc No field required");
			txtDocNo.Focus();
			return false;
		}
		if (txtdocdate.Text.Trim() == "" || txtDocNo.Text.Trim() == "0")
		{
			MessageBox.Show("Doc Date field required");
			txtdocdate.Focus();
			return false;
		}
		if (((UltraGridBase)cmbBoilerName).ActiveRow == null)
		{
			MessageBox.Show("Please Select Boiler name");
			cmbBoilerName.Focus();
			return false;
		}
		if (((UltraGridBase)cmbVehicleType).ActiveRow == null)
		{
			MessageBox.Show("Please Select Vehicle Type");
			cmbVehicleType.Focus();
			return false;
		}
		if (txtNoOfVehicles.Text.Trim() == "" || txtNoOfVehicles.Text.Trim() == "0")
		{
			MessageBox.Show(" No Of Vehicles field required");
			txtNoOfVehicles.Focus();
			return false;
		}
		if (WeightPerVehicle.Text.Trim() == "" || WeightPerVehicle.Text.Trim() == "0")
		{
			MessageBox.Show(" Weight per  Vehicle field required");
			WeightPerVehicle.Focus();
			return false;
		}
		if (txtTWeightKG.Text.Trim() == "" || txtTWeightKG.Text.Trim() == "0")
		{
			MessageBox.Show(" Weight Should be Greater than 0 ");
			return false;
		}
		if (txtQty.Text.Trim() == "" || txtQty.Text.Trim() == "0")
		{
			MessageBox.Show(" Quantity filed Required ");
			txtQty.Focus();
			return false;
		}
		if (txtBagSize.Text.Trim() == "" || txtBagSize.Text.Trim() == "0")
		{
			MessageBox.Show(" Pack size  filed Required ");
			txtBagSize.Focus();
			return false;
		}
		return true;
	}

	private void frmBoilerConsumption_Load(object sender, EventArgs e)
	{
		try
		{
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Visible = formrights.DoHaveSaveRight;
			btnUpdate.Visible = formrights.DoHaveUpdateRights;
			btnPrint.Visible = formrights.DoHavePrintRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formrights.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
			DocumentNoFill();
			BoilerName();
			VehicleTypesFill();
			HistoryFill();
			ctrlGrdBar1.Visible = true;
			btnUpdate.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnAddBoilerName_Click(object sender, EventArgs e)
	{
		new DefineBoilerName(UserAccount).Show();
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		formReset();
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
			BoilerConsumption obj = new BoilerConsumption();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				obj.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocNo = Convert.ToInt32(txtDocNo.Text);
			obj.DocDate = Convert.ToDateTime(txtdocdate.Value);
			obj.IsApproved = false;
			obj.DocumentTypeId = 141;
			obj.Remarks = txtRemarks.Text;
			obj.EntryDate = DateTime.Now;
			obj.EntryUser = UserAccount.ID;
			obj.ModifyDate = DateTime.Now;
			obj.ModifyUser = UserAccount.ID;
			obj.ApprovedDate = DateTime.Now;
			obj.ApprovedUser = UserAccount.ID;
			obj.OnTime = Convert.ToDateTime(txtOnTime.Text);
			obj.OffTime = Convert.ToDateTime(txtOffTime.Text);
			obj.BoilerId = Convert.ToInt32(cmbBoilerName.Value);
			obj.VehicleType = Convert.ToInt32(cmbVehicleType.Value);
			obj.WtPerVehcile = Convert.ToDecimal(WeightPerVehicle.Text);
			obj.NoOfVehicle = Convert.ToInt32(txtNoOfVehicles.Text);
			obj.TotalWeight = Convert.ToDecimal(txtTWeightKG.Text);
			obj.TotalWeightMann = Convert.ToDecimal(txtTWeightMann.Text);
			obj.QtyBags = Convert.ToDecimal(txtQty.Text);
			obj.Uom = Convert.ToInt32(txtBagSize.Text);
			obj.MoisturePaddy = Convert.ToDecimal(txtMoisture.Text);
			obj.DrierSteamUse = Convert.ToDecimal(txtDrierSteamUse.Text);
			obj.StartReading = Convert.ToDecimal(txtStartReading.Text);
			obj.EndReading = Convert.ToDecimal(txtEndReading.Text);
			obj.WaterWeight = Convert.ToDecimal(txtWaterWeightKG.Text);
			obj.SteamWeight = Convert.ToDecimal(SteamWeightKG.Text);
			obj.Remarks = txtRemarks.Text;
			obj.AttachmentsList = new List<DMSAttachments>();
			obj.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<BoilerConsumption>(obj, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			BoilerConsumption.Save(obj);
			if (RecId == 0)
			{
				MessageBox.Show("Record Save SuccessFully" + obj.DocNo);
			}
			else
			{
				MessageBox.Show("Record Update SuccessFully" + obj.DocNo);
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, 141, base.Name);
			}
			formReset();
			HistoryFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
	}

	private void formReset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			RecId = 0;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			txtRemarks.Text = string.Empty;
			txtDocNo.Focus();
			DocumentNoFill();
			Reset();
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
			((Control)(object)cmbBoilerName).Text = string.Empty;
			((Control)(object)cmbVehicleType).Text = string.Empty;
			txtNoOfVehicles.Text = string.Empty;
			WeightPerVehicle.Clear();
			txtTWeightKG.Text = string.Empty;
			txtTWeightMann.Clear();
			txtQty.Text = string.Empty;
			txtBagSize.Text = string.Empty;
			txtMoisture.Text = string.Empty;
			txtDrierSteamUse.Clear();
			txtOnTime.Text = Convert.ToString(DateTime.Now);
			txtOffTime.Text = Convert.ToString(DateTime.Now);
			txtStartReading.Text = string.Empty;
			txtEndReading.Text = string.Empty;
			txtWaterWeightKG.Text = string.Empty;
			SteamWeightKG.Clear();
			txtRemarks.Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click_1(object sender, EventArgs e)
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

	private void HistoryFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			DataTable dt = BoilerConsumption.FormHistory(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 141
			});
			if (dt.Rows.Count > 0 && dt != null)
			{
				grdHistory.DataSource = dt;
				grdHistory.RetrieveStructure();
				GridSetting();
			}
			else
			{
				grdHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridSetting()
	{
		try
		{
			grdHistory.AllowEdit = (InheritableBoolean)2;
			grdHistory.AllowDelete = (InheritableBoolean)2;
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["QtyBags"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["QtyBags"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["QtyBags"].FormatString = "#,#";
			grdHistory.RootTable.Columns["QtyBags"].TotalFormatString = "#,#";
			grdHistory.RootTable.Columns["OnTime"].FormatString = "HH:mmmm:ss tt";
			grdHistory.RootTable.Columns["OffTime"].FormatString = "HH:mmm:ss tt";
			grdHistory.RootTable.Columns.Add("Print");
			grdHistory.RootTable.Columns["Print"].Caption = "Print";
			grdHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Print"].ButtonText = "Print";
			grdHistory.RootTable.Columns["Print"].Width = 50;
			grdHistory.RootTable.Columns["MoisturePaddy"].FormatString = "0.##,##";
			grdHistory.RootTable.Columns["DrierSteamUse"].FormatString = "0.##,##";
			grdHistory.RootTable.Columns["StartReading"].FormatString = "0.##,##";
			grdHistory.RootTable.Columns["EndReading"].FormatString = "0.##,##";
			grdHistory.RootTable.Columns["SteamWeight"].FormatString = "0.##,##";
			grdHistory.RootTable.Columns["WaterWeight"].FormatString = "0.##,##";
			grdHistory.RootTable.Columns["WtPerVehcile"].FormatString = "0.##,##";
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void txtNoOfVehicles_Leave(object sender, EventArgs e)
	{
		try
		{
			if (!(txtNoOfVehicles.Text == "") && Convert.ToInt32(txtNoOfVehicles.Text) > 0)
			{
				CalculateWeight();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WeightPerVehicle_Leave(object sender, EventArgs e)
	{
		try
		{
			if (!(WeightPerVehicle.Text == "") && Convert.ToDouble(WeightPerVehicle.Text) > 0.0)
			{
				CalculateWeight();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateWeight()
	{
		try
		{
			int noOfVehicles = Convert.ToInt32(txtNoOfVehicles.Text);
			if (!(WeightPerVehicle.Text == ""))
			{
				double weightPerVehicle = Convert.ToDouble(WeightPerVehicle.Text);
				if (noOfVehicles > 0 && weightPerVehicle > 0.0)
				{
					double totalWeightKgs = (double)noOfVehicles * weightPerVehicle;
					double totalWeightMann = totalWeightKgs / 40.0;
					txtTWeightKG.Text = Convert.ToString(totalWeightKgs);
					txtTWeightMann.Text = Convert.ToString(totalWeightMann);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtStartReading_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtEndReading_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtWaterWeightKG_KeyPress(object sender, KeyPressEventArgs e)
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

	private void SteamWeightKG_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtNoOfVehicles_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtBagSize_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtMoisture_KeyPress(object sender, KeyPressEventArgs e)
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

	private void WeightPerVehicle_KeyPress(object sender, KeyPressEventArgs e)
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

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Print")
			{
				BoilerConsumptionSlip(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BoilerConsumptionSlip(int PrintId)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("Record Not Found For Display");
				return;
			}
			DataTable dt = BoilerConsumption.BoilerConsumptionHistory(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 141,
				Id = PrintId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("Record Not Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "461-BoilerConsumption_Slip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value), 141);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null)
			{
				BoilerConsumption boilerConsumption = new BoilerConsumption();
				RecId = Conversion.ToInt(item.Cells["Id"].Value);
				boilerConsumption = BoilerConsumption.GetByID(RecId);
				if (boilerConsumption != null)
				{
					btnsave.Visible = false;
					btnUpdate.Visible = true;
					txtDocNo.Text = Conversion.ToString((object)boilerConsumption.DocNo);
					txtdocdate.Text = Conversion.ToString((object)boilerConsumption.DocDate);
					cmbBoilerName.Value = Conversion.ToInt((object)boilerConsumption.BoilerId);
					BoilerName();
					cmbVehicleType.Value = Conversion.ToInt((object)boilerConsumption.VehicleType);
					VehicleTypesFill();
					txtNoOfVehicles.Text = Conversion.ToString((object)boilerConsumption.NoOfVehicle);
					WeightPerVehicle.Text = Conversion.ToString((object)boilerConsumption.WtPerVehcile);
					txtTWeightKG.Text = Conversion.ToString((object)boilerConsumption.TotalWeight);
					txtTWeightMann.Text = Conversion.ToString((object)boilerConsumption.TotalWeightMann);
					txtQty.Text = Conversion.ToString((object)boilerConsumption.QtyBags);
					txtBagSize.Text = Conversion.ToString((object)boilerConsumption.Uom);
					txtMoisture.Text = Conversion.ToString((object)boilerConsumption.MoisturePaddy);
					txtDrierSteamUse.Text = Conversion.ToString((object)boilerConsumption.DrierSteamUse);
					txtOnTime.Text = Conversion.ToString((object)boilerConsumption.OnTime);
					txtOffTime.Text = Conversion.ToString((object)boilerConsumption.OffTime);
					txtStartReading.Text = Conversion.ToString((object)boilerConsumption.StartReading);
					txtEndReading.Text = Conversion.ToString((object)boilerConsumption.EndReading);
					txtWaterWeightKG.Text = Conversion.ToString((object)boilerConsumption.WaterWeight);
					SteamWeightKG.Text = Conversion.ToString((object)boilerConsumption.SteamWeight);
					txtRemarks.Text = Conversion.ToString((object)boilerConsumption.Remarks);
					FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click_1(object sender, EventArgs e)
	{
		Insert();
	}

	private void btnNew_Click_1(object sender, EventArgs e)
	{
		try
		{
			formReset();
			HistoryFill();
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
			BoilerName();
			VehicleTypesFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
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

	private void toolStripButton2_Click(object sender, EventArgs e)
	{
		try
		{
			BoilerConsumptionSlip(RecId);
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
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdHistory).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdHistory).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				grdHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	private void grdHistory_FormattingRow(object sender, RowLoadEventArgs e)
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
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Expected O, but got Unknown
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Expected O, but got Unknown
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Expected O, but got Unknown
		//IL_0421: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Expected O, but got Unknown
		//IL_049a: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a4: Expected O, but got Unknown
		//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Expected O, but got Unknown
		//IL_2366: Unknown result type (might be due to invalid IL or missing references)
		//IL_2370: Expected O, but got Unknown
		//IL_237d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2387: Expected O, but got Unknown
		//IL_2394: Unknown result type (might be due to invalid IL or missing references)
		//IL_239e: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Production.frmBoilerConsumption));
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GridRegister_DesignTimeLayout = new GridEXLayout();
		Appearance appearance19 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
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
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.BtnAddBoilerName = new System.Windows.Forms.Button();
		this.txtOffTime = new System.Windows.Forms.DateTimePicker();
		this.txtOnTime = new System.Windows.Forms.DateTimePicker();
		this.txtWaterWeightKG = new System.Windows.Forms.TextBox();
		this.txtStartReading = new System.Windows.Forms.TextBox();
		this.txtEndReading = new System.Windows.Forms.TextBox();
		this.SteamWeightKG = new System.Windows.Forms.TextBox();
		this.txtDrierSteamUse = new System.Windows.Forms.TextBox();
		this.label18 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.txtMoisture = new System.Windows.Forms.TextBox();
		this.txtBagSize = new System.Windows.Forms.TextBox();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.txtTWeightMann = new System.Windows.Forms.TextBox();
		this.txtTWeightKG = new System.Windows.Forms.TextBox();
		this.WeightPerVehicle = new System.Windows.Forms.TextBox();
		this.cmbVehicleType = new UltraCombo();
		this.txtNoOfVehicles = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.cmbBoilerName = new UltraCombo();
		this.txtdocdate = new System.Windows.Forms.DateTimePicker();
		this.txtDocNo = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.grdHistory = new GridEX();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
		this.panel6 = new System.Windows.Forms.Panel();
		this.GridRegister = new GridEX();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.BtnShow = new System.Windows.Forms.Button();
		this.txtDocNoFrom = new System.Windows.Forms.TextBox();
		this.label25 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.txtDateTo = new System.Windows.Forms.DateTimePicker();
		this.label23 = new System.Windows.Forms.Label();
		this.txtDocNoTo = new System.Windows.Forms.TextBox();
		this.txtDateFrom = new System.Windows.Forms.DateTimePicker();
		this.label24 = new System.Windows.Forms.Label();
		this.CmbBoilerNameRegister = new UltraCombo();
		this.label21 = new System.Windows.Forms.Label();
		this.CmbVehicleTypeRegister = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbVehicleType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBoilerName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel5.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel7.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridRegister).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBoilerNameRegister).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVehicleTypeRegister).BeginInit();
		base.SuspendLayout();
		this.toolStrip1.BackColor = System.Drawing.Color.Teal;
		this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.btnNew, this.btnsave, this.btnUpdate, this.btnAttachment, this.btnRefresh, this.btnPrint });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1182, 37);
		this.toolStrip1.TabIndex = 1;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.ForeColor = System.Drawing.Color.White;
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(57, 34);
		this.btnNew.Text = "New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click_1);
		this.btnsave.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.ForeColor = System.Drawing.Color.White;
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(56, 34);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click_1);
		this.btnUpdate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.ForeColor = System.Drawing.Color.White;
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(71, 34);
		this.btnUpdate.Text = "Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click_1);
		this.btnAttachment.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.ForeColor = System.Drawing.Color.White;
		this.btnAttachment.Image = (System.Drawing.Image)resources.GetObject("btnAttachment.Image");
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.White;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(96, 34);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnRefresh.AccessibleName = "btnRefresh";
		this.btnRefresh.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.ForeColor = System.Drawing.Color.White;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(53, 34);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnPrint.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.ForeColor = System.Drawing.Color.White;
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(105, 34);
		this.btnPrint.Text = "461-Print Slip";
		this.btnPrint.Click += new System.EventHandler(toolStripButton2_Click);
		this.txtRemarks.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtRemarks.Location = new System.Drawing.Point(10, 116);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(1166, 25);
		this.txtRemarks.TabIndex = 16;
		this.label19.AutoSize = true;
		this.label19.Location = new System.Drawing.Point(10, 100);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(49, 13);
		this.label19.TabIndex = 41;
		this.label19.Text = "Remarks";
		this.BtnAddBoilerName.BackColor = System.Drawing.Color.White;
		this.BtnAddBoilerName.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.BtnAddBoilerName.Location = new System.Drawing.Point(453, 23);
		this.BtnAddBoilerName.Name = "BtnAddBoilerName";
		this.BtnAddBoilerName.Size = new System.Drawing.Size(29, 24);
		this.BtnAddBoilerName.TabIndex = 40;
		this.BtnAddBoilerName.TabStop = false;
		this.BtnAddBoilerName.UseVisualStyleBackColor = false;
		this.BtnAddBoilerName.Click += new System.EventHandler(BtnAddBoilerName_Click);
		this.txtOffTime.CustomFormat = "hh:mm tt";
		this.txtOffTime.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtOffTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtOffTime.Location = new System.Drawing.Point(605, 71);
		this.txtOffTime.Name = "txtOffTime";
		this.txtOffTime.Size = new System.Drawing.Size(99, 25);
		this.txtOffTime.TabIndex = 11;
		this.txtOffTime.Value = new System.DateTime(2022, 11, 14, 15, 7, 5, 0);
		this.txtOnTime.CustomFormat = "hh:mm tt";
		this.txtOnTime.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtOnTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtOnTime.Location = new System.Drawing.Point(484, 71);
		this.txtOnTime.Name = "txtOnTime";
		this.txtOnTime.Size = new System.Drawing.Size(115, 25);
		this.txtOnTime.TabIndex = 10;
		this.txtOnTime.Value = new System.DateTime(2022, 11, 14, 15, 6, 52, 0);
		this.txtWaterWeightKG.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtWaterWeightKG.Location = new System.Drawing.Point(939, 71);
		this.txtWaterWeightKG.Name = "txtWaterWeightKG";
		this.txtWaterWeightKG.Size = new System.Drawing.Size(116, 25);
		this.txtWaterWeightKG.TabIndex = 14;
		this.txtWaterWeightKG.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtWaterWeightKG_KeyPress);
		this.txtStartReading.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtStartReading.Location = new System.Drawing.Point(710, 71);
		this.txtStartReading.Name = "txtStartReading";
		this.txtStartReading.Size = new System.Drawing.Size(109, 25);
		this.txtStartReading.TabIndex = 12;
		this.txtStartReading.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtStartReading_KeyPress);
		this.txtEndReading.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtEndReading.Location = new System.Drawing.Point(825, 71);
		this.txtEndReading.Name = "txtEndReading";
		this.txtEndReading.Size = new System.Drawing.Size(108, 25);
		this.txtEndReading.TabIndex = 13;
		this.txtEndReading.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtEndReading_KeyPress);
		this.SteamWeightKG.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.SteamWeightKG.Location = new System.Drawing.Point(1061, 71);
		this.SteamWeightKG.Name = "SteamWeightKG";
		this.SteamWeightKG.Size = new System.Drawing.Size(115, 25);
		this.SteamWeightKG.TabIndex = 15;
		this.SteamWeightKG.KeyPress += new System.Windows.Forms.KeyPressEventHandler(SteamWeightKG_KeyPress);
		this.txtDrierSteamUse.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtDrierSteamUse.Location = new System.Drawing.Point(235, 71);
		this.txtDrierSteamUse.Name = "txtDrierSteamUse";
		this.txtDrierSteamUse.Size = new System.Drawing.Size(247, 25);
		this.txtDrierSteamUse.TabIndex = 9;
		this.label18.AutoSize = true;
		this.label18.Location = new System.Drawing.Point(1051, 55);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(98, 13);
		this.label18.TabIndex = 30;
		this.label18.Text = "Steam Weight (KG)";
		this.label17.AutoSize = true;
		this.label17.Location = new System.Drawing.Point(936, 55);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(97, 13);
		this.label17.TabIndex = 29;
		this.label17.Text = "Water Weight (KG)";
		this.label16.AutoSize = true;
		this.label16.Location = new System.Drawing.Point(824, 55);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(69, 13);
		this.label16.TabIndex = 28;
		this.label16.Text = "End Reading";
		this.txtMoisture.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtMoisture.Location = new System.Drawing.Point(130, 71);
		this.txtMoisture.Name = "txtMoisture";
		this.txtMoisture.Size = new System.Drawing.Size(99, 25);
		this.txtMoisture.TabIndex = 8;
		this.txtMoisture.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtMoisture_KeyPress);
		this.txtBagSize.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtBagSize.Location = new System.Drawing.Point(10, 71);
		this.txtBagSize.Name = "txtBagSize";
		this.txtBagSize.Size = new System.Drawing.Size(114, 25);
		this.txtBagSize.TabIndex = 7;
		this.txtBagSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBagSize_KeyPress);
		this.txtQty.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtQty.Location = new System.Drawing.Point(1061, 22);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(115, 25);
		this.txtQty.TabIndex = 6;
		this.txtQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQty_KeyPress);
		this.txtTWeightMann.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtTWeightMann.Location = new System.Drawing.Point(940, 22);
		this.txtTWeightMann.Name = "txtTWeightMann";
		this.txtTWeightMann.ReadOnly = true;
		this.txtTWeightMann.Size = new System.Drawing.Size(115, 25);
		this.txtTWeightMann.TabIndex = 7;
		this.txtTWeightKG.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtTWeightKG.Location = new System.Drawing.Point(825, 22);
		this.txtTWeightKG.Name = "txtTWeightKG";
		this.txtTWeightKG.ReadOnly = true;
		this.txtTWeightKG.Size = new System.Drawing.Size(109, 25);
		this.txtTWeightKG.TabIndex = 6;
		this.WeightPerVehicle.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.WeightPerVehicle.Location = new System.Drawing.Point(709, 22);
		this.WeightPerVehicle.Name = "WeightPerVehicle";
		this.WeightPerVehicle.Size = new System.Drawing.Size(110, 25);
		this.WeightPerVehicle.TabIndex = 5;
		this.WeightPerVehicle.KeyPress += new System.Windows.Forms.KeyPressEventHandler(WeightPerVehicle_KeyPress);
		this.WeightPerVehicle.Leave += new System.EventHandler(WeightPerVehicle_Leave);
		this.cmbVehicleType.AllowNull = (DefaultableBoolean)1;
		this.cmbVehicleType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbVehicleType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbVehicleType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbVehicleType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance11;
		((SpecialBoxBase)((UltraGridBase)this.cmbVehicleType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbVehicleType).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbVehicleType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbVehicleType).Location = new System.Drawing.Point(487, 22);
		((System.Windows.Forms.Control)(object)this.cmbVehicleType).Name = "cmbVehicleType";
		((System.Windows.Forms.Control)(object)this.cmbVehicleType).Size = new System.Drawing.Size(112, 24);
		((System.Windows.Forms.Control)(object)this.cmbVehicleType).TabIndex = 3;
		this.txtNoOfVehicles.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtNoOfVehicles.Location = new System.Drawing.Point(605, 22);
		this.txtNoOfVehicles.Name = "txtNoOfVehicles";
		this.txtNoOfVehicles.Size = new System.Drawing.Size(99, 25);
		this.txtNoOfVehicles.TabIndex = 4;
		this.txtNoOfVehicles.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNoOfVehicles_KeyPress);
		this.txtNoOfVehicles.Leave += new System.EventHandler(txtNoOfVehicles_Leave);
		this.label15.AutoSize = true;
		this.label15.Location = new System.Drawing.Point(127, 6);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(44, 13);
		this.label15.TabIndex = 19;
		this.label15.Text = "Doc No";
		this.label14.AutoSize = true;
		this.label14.Location = new System.Drawing.Point(232, 6);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(64, 13);
		this.label14.TabIndex = 18;
		this.label14.Text = "Boiler Name";
		this.label13.AutoSize = true;
		this.label13.Location = new System.Drawing.Point(484, 6);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(69, 13);
		this.label13.TabIndex = 17;
		this.label13.Text = "Vehicle Type";
		this.label12.AutoSize = true;
		this.label12.Location = new System.Drawing.Point(602, 6);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(73, 13);
		this.label12.TabIndex = 16;
		this.label12.Text = "No Of Vehicle";
		this.label11.AutoSize = true;
		this.label11.Location = new System.Drawing.Point(937, 6);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(107, 13);
		this.label11.TabIndex = 15;
		this.label11.Text = "Totla Weight (Mann )";
		this.label10.AutoSize = true;
		this.label10.Location = new System.Drawing.Point(1061, 6);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(50, 13);
		this.label10.TabIndex = 14;
		this.label10.Text = "Bags Qty";
		this.label9.AutoSize = true;
		this.label9.Location = new System.Drawing.Point(242, 55);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(84, 13);
		this.label9.TabIndex = 13;
		this.label9.Text = "Drier Steam Use";
		this.label8.AutoSize = true;
		this.label8.Location = new System.Drawing.Point(481, 55);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(47, 13);
		this.label8.TabIndex = 12;
		this.label8.Text = "On Time";
		this.label7.AutoSize = true;
		this.label7.Location = new System.Drawing.Point(598, 55);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(47, 13);
		this.label7.TabIndex = 11;
		this.label7.Text = "Off Time";
		this.label6.AutoSize = true;
		this.label6.Location = new System.Drawing.Point(717, 55);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(72, 13);
		this.label6.TabIndex = 10;
		this.label6.Text = "Start Reading";
		this.label5.AutoSize = true;
		this.label5.Location = new System.Drawing.Point(829, 6);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(92, 13);
		this.label5.TabIndex = 9;
		this.label5.Text = "Total Weight (KG)";
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(10, 55);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(55, 13);
		this.label4.TabIndex = 8;
		this.label4.Text = "Pack Size";
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(127, 55);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(80, 13);
		this.label3.TabIndex = 7;
		this.label3.Text = "Paddy Moisture";
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(710, 6);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(98, 13);
		this.label2.TabIndex = 6;
		this.label2.Text = "Weight Per Vehicle";
		this.cmbBoilerName.AllowNull = (DefaultableBoolean)1;
		this.cmbBoilerName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBoilerName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBoilerName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBoilerName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbBoilerName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBoilerName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBoilerName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBoilerName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance8;
		((SpecialBoxBase)((UltraGridBase)this.cmbBoilerName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBoilerName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbBoilerName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBoilerName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBoilerName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBoilerName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbBoilerName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBoilerName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBoilerName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance14).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBoilerName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbBoilerName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBoilerName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBoilerName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBoilerName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbBoilerName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBoilerName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBoilerName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbBoilerName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBoilerName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbBoilerName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBoilerName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBoilerName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBoilerName).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbBoilerName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBoilerName).Location = new System.Drawing.Point(235, 22);
		((System.Windows.Forms.Control)(object)this.cmbBoilerName).Name = "cmbBoilerName";
		((System.Windows.Forms.Control)(object)this.cmbBoilerName).Size = new System.Drawing.Size(215, 24);
		((System.Windows.Forms.Control)(object)this.cmbBoilerName).TabIndex = 2;
		this.txtdocdate.CustomFormat = "dd-MMM-yyyy";
		this.txtdocdate.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtdocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdocdate.Location = new System.Drawing.Point(10, 22);
		this.txtdocdate.Name = "txtdocdate";
		this.txtdocdate.Size = new System.Drawing.Size(114, 25);
		this.txtdocdate.TabIndex = 0;
		this.txtdocdate.Value = new System.DateTime(2022, 11, 14, 15, 6, 26, 0);
		this.txtDocNo.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtDocNo.Location = new System.Drawing.Point(130, 22);
		this.txtDocNo.Name = "txtDocNo";
		this.txtDocNo.Size = new System.Drawing.Size(99, 25);
		this.txtDocNo.TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(7, 6);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(53, 13);
		this.label1.TabIndex = 0;
		this.label1.Text = "Doc Date";
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdHistory).BackColor = System.Drawing.Color.White;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1182, 440);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdHistory).TabStop = false;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.FormattingRow += new RowLoadEventHandler(grdHistory_FormattingRow);
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1196, 657);
		this.panel1.TabIndex = 41;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1196, 657);
		this.tabControl1.TabIndex = 1;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1188, 631);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel3.Controls.Add(this.panel9);
		this.panel3.Controls.Add(this.panel8);
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1182, 625);
		this.panel3.TabIndex = 4;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 185);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1182, 440);
		this.panel9.TabIndex = 2;
		this.panel8.Controls.Add(this.txtRemarks);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBoilerName);
		this.panel8.Controls.Add(this.label18);
		this.panel8.Controls.Add(this.label13);
		this.panel8.Controls.Add(this.label6);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.cmbVehicleType);
		this.panel8.Controls.Add(this.label7);
		this.panel8.Controls.Add(this.label19);
		this.panel8.Controls.Add(this.txtDrierSteamUse);
		this.panel8.Controls.Add(this.txtNoOfVehicles);
		this.panel8.Controls.Add(this.label17);
		this.panel8.Controls.Add(this.label1);
		this.panel8.Controls.Add(this.label5);
		this.panel8.Controls.Add(this.WeightPerVehicle);
		this.panel8.Controls.Add(this.label8);
		this.panel8.Controls.Add(this.BtnAddBoilerName);
		this.panel8.Controls.Add(this.SteamWeightKG);
		this.panel8.Controls.Add(this.label15);
		this.panel8.Controls.Add(this.label16);
		this.panel8.Controls.Add(this.txtDocNo);
		this.panel8.Controls.Add(this.label4);
		this.panel8.Controls.Add(this.txtTWeightKG);
		this.panel8.Controls.Add(this.label9);
		this.panel8.Controls.Add(this.txtOffTime);
		this.panel8.Controls.Add(this.txtEndReading);
		this.panel8.Controls.Add(this.label14);
		this.panel8.Controls.Add(this.txtMoisture);
		this.panel8.Controls.Add(this.txtdocdate);
		this.panel8.Controls.Add(this.label3);
		this.panel8.Controls.Add(this.txtTWeightMann);
		this.panel8.Controls.Add(this.label10);
		this.panel8.Controls.Add(this.txtOnTime);
		this.panel8.Controls.Add(this.txtStartReading);
		this.panel8.Controls.Add(this.label12);
		this.panel8.Controls.Add(this.txtBagSize);
		this.panel8.Controls.Add(this.label2);
		this.panel8.Controls.Add(this.txtQty);
		this.panel8.Controls.Add(this.label11);
		this.panel8.Controls.Add(this.txtWaterWeightKG);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 37);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1182, 148);
		this.panel8.TabIndex = 1;
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Controls.Add(this.toolStrip1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1182, 37);
		this.panel5.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = null;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1130, 5);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = null;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 25);
		this.ctrlGrdBar1.TabIndex = 21;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.tabPage2.Controls.Add(this.panel4);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1188, 631);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Register";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel4.Controls.Add(this.panel7);
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(3, 3);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1182, 625);
		this.panel4.TabIndex = 0;
		this.panel7.Controls.Add(this.toolStrip2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1182, 32);
		this.panel7.TabIndex = 2;
		this.toolStrip2.BackColor = System.Drawing.Color.Teal;
		this.toolStrip2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.toolStripButton1, this.toolStripButton6, this.toolStripButton7 });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1182, 32);
		this.toolStrip2.TabIndex = 2;
		this.toolStrip2.Text = "toolStrip2";
		this.toolStripButton1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton1.ForeColor = System.Drawing.Color.White;
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(57, 29);
		this.toolStripButton1.Text = "New";
		this.toolStripButton6.AccessibleName = "btnRefresh";
		this.toolStripButton6.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton6.ForeColor = System.Drawing.Color.White;
		this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton6.Name = "toolStripButton6";
		this.toolStripButton6.Size = new System.Drawing.Size(53, 29);
		this.toolStripButton6.Text = "Refresh";
		this.toolStripButton7.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton7.ForeColor = System.Drawing.Color.White;
		this.toolStripButton7.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton7.Name = "toolStripButton7";
		this.toolStripButton7.Size = new System.Drawing.Size(131, 29);
		this.toolStripButton7.Text = "462-Print Register";
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.GridRegister);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel6.Location = new System.Drawing.Point(0, 99);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1182, 526);
		this.panel6.TabIndex = 1;
		this.GridRegister.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.GridRegister).BackColor = System.Drawing.Color.White;
		GridRegister_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridRegister.DesignTimeLayout = GridRegister_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridRegister).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridRegister.FilterMode = (FilterMode)2;
		this.GridRegister.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridRegister).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridRegister.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GridRegister).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GridRegister).Name = "GridRegister";
		((ScrollableBase)this.GridRegister).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridRegister).Size = new System.Drawing.Size(1182, 526);
		((System.Windows.Forms.Control)(object)this.GridRegister).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.GridRegister).TabStop = false;
		this.GridRegister.TotalRow = (InheritableBoolean)1;
		this.GridRegister.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridRegister).VisualStyle = (VisualStyle)4;
		this.groupBox1.Controls.Add(this.BtnShow);
		this.groupBox1.Controls.Add(this.txtDocNoFrom);
		this.groupBox1.Controls.Add(this.label25);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add(this.txtDateTo);
		this.groupBox1.Controls.Add(this.label23);
		this.groupBox1.Controls.Add(this.txtDocNoTo);
		this.groupBox1.Controls.Add(this.txtDateFrom);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBoilerNameRegister);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbVehicleTypeRegister);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(1, 34);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1179, 69);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.BtnShow.BackColor = System.Drawing.Color.Teal;
		this.BtnShow.ForeColor = System.Drawing.Color.White;
		this.BtnShow.Location = new System.Drawing.Point(904, 32);
		this.BtnShow.Name = "BtnShow";
		this.BtnShow.Size = new System.Drawing.Size(75, 26);
		this.BtnShow.TabIndex = 33;
		this.BtnShow.Text = "Show";
		this.BtnShow.UseVisualStyleBackColor = false;
		this.txtDocNoFrom.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtDocNoFrom.Location = new System.Drawing.Point(246, 34);
		this.txtDocNoFrom.Multiline = true;
		this.txtDocNoFrom.Name = "txtDocNoFrom";
		this.txtDocNoFrom.Size = new System.Drawing.Size(99, 24);
		this.txtDocNoFrom.TabIndex = 31;
		this.label25.AutoSize = true;
		this.label25.Location = new System.Drawing.Point(243, 14);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(87, 16);
		this.label25.TabIndex = 32;
		this.label25.Text = "Doc No From";
		this.label22.AutoSize = true;
		this.label22.Location = new System.Drawing.Point(126, 14);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(58, 16);
		this.label22.TabIndex = 29;
		this.label22.Text = "Date To";
		this.txtDateTo.CustomFormat = "dd-MMM-yyyy";
		this.txtDateTo.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDateTo.Location = new System.Drawing.Point(126, 34);
		this.txtDateTo.Name = "txtDateTo";
		this.txtDateTo.Size = new System.Drawing.Size(114, 25);
		this.txtDateTo.TabIndex = 30;
		this.txtDateTo.Value = new System.DateTime(2022, 11, 14, 15, 6, 26, 0);
		this.label23.AutoSize = true;
		this.label23.Location = new System.Drawing.Point(6, 14);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(74, 16);
		this.label23.TabIndex = 25;
		this.label23.Text = "Date From";
		this.txtDocNoTo.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtDocNoTo.Location = new System.Drawing.Point(351, 34);
		this.txtDocNoTo.Multiline = true;
		this.txtDocNoTo.Name = "txtDocNoTo";
		this.txtDocNoTo.Size = new System.Drawing.Size(105, 24);
		this.txtDocNoTo.TabIndex = 27;
		this.txtDateFrom.CustomFormat = "dd-MMM-yyyy";
		this.txtDateFrom.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDateFrom.Location = new System.Drawing.Point(6, 34);
		this.txtDateFrom.Name = "txtDateFrom";
		this.txtDateFrom.Size = new System.Drawing.Size(114, 25);
		this.txtDateFrom.TabIndex = 26;
		this.txtDateFrom.Value = new System.DateTime(2022, 11, 14, 15, 6, 26, 0);
		this.label24.AutoSize = true;
		this.label24.Location = new System.Drawing.Point(354, 14);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(71, 16);
		this.label24.TabIndex = 28;
		this.label24.Text = "Doc No To";
		this.CmbBoilerNameRegister.AllowNull = (DefaultableBoolean)1;
		this.CmbBoilerNameRegister.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBoilerNameRegister.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBoilerNameRegister.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBoilerNameRegister).DisplayLayout.Appearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbBoilerNameRegister).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBoilerNameRegister).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBoilerNameRegister).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBoilerNameRegister).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance22;
		((SpecialBoxBase)((UltraGridBase)this.CmbBoilerNameRegister).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBoilerNameRegister).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbBoilerNameRegister).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBoilerNameRegister).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBoilerNameRegister).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBoilerNameRegister).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbBoilerNameRegister).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBoilerNameRegister).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBoilerNameRegister).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance27).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBoilerNameRegister).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbBoilerNameRegister).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBoilerNameRegister).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBoilerNameRegister).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBoilerNameRegister).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbBoilerNameRegister).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBoilerNameRegister).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBoilerNameRegister).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbBoilerNameRegister).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBoilerNameRegister).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbBoilerNameRegister).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBoilerNameRegister).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBoilerNameRegister).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBoilerNameRegister).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBoilerNameRegister.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBoilerNameRegister).Location = new System.Drawing.Point(462, 34);
		((System.Windows.Forms.Control)(object)this.CmbBoilerNameRegister).Name = "CmbBoilerNameRegister";
		((System.Windows.Forms.Control)(object)this.CmbBoilerNameRegister).Size = new System.Drawing.Size(215, 24);
		((System.Windows.Forms.Control)(object)this.CmbBoilerNameRegister).TabIndex = 21;
		this.label21.AutoSize = true;
		this.label21.Location = new System.Drawing.Point(459, 14);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(83, 16);
		this.label21.TabIndex = 22;
		this.label21.Text = "Boiler Name";
		this.CmbVehicleTypeRegister.AllowNull = (DefaultableBoolean)1;
		this.CmbVehicleTypeRegister.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbVehicleTypeRegister.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbVehicleTypeRegister.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbVehicleTypeRegister).DisplayLayout.Appearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbVehicleTypeRegister).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbVehicleTypeRegister).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbVehicleTypeRegister).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVehicleTypeRegister).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance35;
		((SpecialBoxBase)((UltraGridBase)this.CmbVehicleTypeRegister).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVehicleTypeRegister).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbVehicleTypeRegister).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbVehicleTypeRegister).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbVehicleTypeRegister).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbVehicleTypeRegister).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbVehicleTypeRegister).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbVehicleTypeRegister).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVehicleTypeRegister).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance40).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbVehicleTypeRegister).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbVehicleTypeRegister).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbVehicleTypeRegister).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance42).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVehicleTypeRegister).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbVehicleTypeRegister).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbVehicleTypeRegister).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbVehicleTypeRegister).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbVehicleTypeRegister).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbVehicleTypeRegister).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbVehicleTypeRegister).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbVehicleTypeRegister).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbVehicleTypeRegister).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbVehicleTypeRegister).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbVehicleTypeRegister).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbVehicleTypeRegister.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbVehicleTypeRegister).Location = new System.Drawing.Point(683, 34);
		((System.Windows.Forms.Control)(object)this.CmbVehicleTypeRegister).Name = "CmbVehicleTypeRegister";
		((System.Windows.Forms.Control)(object)this.CmbVehicleTypeRegister).Size = new System.Drawing.Size(215, 24);
		((System.Windows.Forms.Control)(object)this.CmbVehicleTypeRegister).TabIndex = 19;
		this.label20.AutoSize = true;
		this.label20.Location = new System.Drawing.Point(680, 14);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(89, 16);
		this.label20.TabIndex = 20;
		this.label20.Text = "Vehicle Type";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1196, 657);
		base.Controls.Add(this.panel1);
		base.Name = "frmBoilerConsumption";
		this.Text = "Boiler Consumption";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmBoilerConsumption_Load);
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbVehicleType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBoilerName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridRegister).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBoilerNameRegister).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVehicleTypeRegister).EndInit();
		base.ResumeLayout(false);
	}
}
