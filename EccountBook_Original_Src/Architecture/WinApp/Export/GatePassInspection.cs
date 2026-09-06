using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Reports;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.Export;

public class GatePassInspection : Form
{
	private UserAccount UserAccount = new UserAccount();

	private int RecId;

	private int RecId2;

	private DataTable dtDocFirst = new DataTable();

	private DataTable dtDocSecond = new DataTable();

	private DataTable dataTable = new DataTable();

	private DataTable dtgp = new DataTable();

	private DataTable dtReport = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private int id;

	private bool UpdateMode;

	private bool InsertDocFirstExcuted;

	private bool InsertDocSecondExcuted;

	private bool ValidatationDocFirstExcuted;

	private bool ValidatationDocSecondExcuted;

	private Attachment AT = new Attachment();

	private Attachment AT2 = new Attachment();

	private Rightsobjects formrights = new Rightsobjects();

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnsave;

	private ToolStripButton btnupdate;

	private Label label1;

	private ToolStripButton btnNew;

	private Panel panel4;

	private Label label2;

	private TextBox txtsealno1;

	private Label label3;

	private Label label5;

	private TextBox txtvehicleno1;

	private TextBox txtcontainerno1;

	private Label label4;

	private Panel panel14;

	internal GridEX grdDoc01;

	private Label label18;

	private Label label17;

	private Label label16;

	private Label label15;

	private GroupBox boxcontainer1;

	private TextBox txtinspect1;

	private TextBox txtpict1;

	private TextBox txtverify1;

	private CheckBox chkloading1;

	private GroupBox groupBox3;

	private GroupBox boxcontainer2;

	private CheckBox chkloading2;

	private TextBox txtpict2;

	private TextBox txtverify2;

	private TextBox txtinspect2;

	private TextBox txtcontainer2;

	private Label label6;

	private Label label7;

	private Label label8;

	private Label label9;

	private Label label10;

	private Label label11;

	private TextBox txtseal2;

	private Panel panel7;

	internal GridEX grdDoc02;

	private Panel panel9;

	private Panel panel11;

	private Panel panel12;

	private Panel panel6;

	internal UltraCombo cmbgatepass;

	private ToolStripButton btnAttachmentsForContainer1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private RichTextBox txtremarks2;

	private RichTextBox txtremarks1;

	private ToolStripButton btnShortCutKey;

	private Panel panel5;

	private GroupBox groupBox2;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker FromDateHistory;

	private Button btnShowHistory;

	private Label label73;

	private Label label89;

	private Label label90;

	private Label label91;

	private DateTimePicker ToDateHistory;

	private Panel panel16;

	private Label label69;

	private ToolStrip toolStrip3;

	private ToolStripButton btnResetHistory;

	internal GridEX DataGridHistory;

	private Panel panel15;

	private Label label92;

	private Panel panel8;

	internal GridEX grdDoc02History;

	private Panel panel10;

	private Panel panel13;

	private GroupBox groupBox1;

	private CheckBox chkContainerAccepted02History;

	private TextBox txtPicBy02History;

	private TextBox txtVerifiedBy02History;

	private TextBox txtInspectedBy02History;

	private TextBox txtContainer02History;

	private Label label12;

	private Label label13;

	private Label label14;

	private Label label19;

	private Label label20;

	private Label label21;

	private TextBox txtSeal02History;

	private Panel panel17;

	internal GridEX grdDoc01History;

	private Panel panel18;

	private Panel panel19;

	private GroupBox groupBox4;

	private CheckBox chkContainerAccepted01History;

	private TextBox txtPicBy01History;

	private TextBox txtVerifiedBy01History;

	private TextBox txtInspectedBy01History;

	private TextBox txtContainer01History;

	private Label label22;

	private Label label23;

	private Label label24;

	private Label label25;

	private Label label26;

	private Label label27;

	private TextBox txtSeal01History;

	private Panel panel20;

	private TextBox txtRemarks02History;

	private TextBox txtRemarks01History;

	private ToolStripButton btnAttachmentsForContainer2;

	private ToolStripButton btnPrint;

	public GatePassInspection(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void GatePassInspection_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMargin = new Size(975, 700);
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formrights.DoHaveSaveRight;
			btnupdate.Enabled = formrights.DoHaveUpdateRights;
			dtDocFirst.Columns.Add("Id", typeof(int));
			dtDocFirst.Columns.Add("VCIParameterId", typeof(int));
			dtDocFirst.Columns.Add("CategoryId", typeof(int));
			dtDocFirst.Columns.Add("Category", typeof(string));
			dtDocFirst.Columns.Add("InspectionPoint", typeof(string));
			dtDocFirst.Columns.Add("Status", typeof(bool));
			dtDocFirst.Columns.Add("Remarks", typeof(string));
			dtDocSecond.Columns.Add("Id", typeof(int));
			dtDocSecond.Columns.Add("VCIParameterId", typeof(int));
			dtDocSecond.Columns.Add("CategoryId", typeof(int));
			dtDocSecond.Columns.Add("Category", typeof(string));
			dtDocSecond.Columns.Add("InspectionPoint", typeof(string));
			dtDocSecond.Columns.Add("Status", typeof(bool));
			dtDocSecond.Columns.Add("Remarks", typeof(string));
			Gatepassfill(0);
			grdDocFirstfill();
			grdDocSecondfill();
			cmbgatepass.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void Gatepassfill(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			GatePassOutward gpi = new GatePassOutward();
			gpi.DocumentTypeId = 91;
			gpi.CompanyId = UserAccount.CompanyId;
			gpi.OrganizationId = UserAccount.OrganizationId;
			gpi.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			gpi.Id = Id;
			dtgp = GatePassOutward.AllGatePassNOForGPInspection(gpi);
			if (dtgp.Rows.Count > 0)
			{
				DDL.BindDDL(dtgp, cmbgatepass, "Id", "GpSrNo", "Gate Pass", ZeroIndex: false);
				((UltraGridBase)cmbgatepass).DisplayLayout.Bands[0].Columns[1].Width = 100;
				((UltraGridBase)cmbgatepass).DisplayLayout.Bands[0].Columns[5].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbgatepass_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbgatepass).ActiveRow != null && ((Control)(object)cmbgatepass).Text != string.Empty)
			{
				ReadById(Conversion.ToInt(cmbgatepass.Value));
				txtvehicleno1.Text = Conversion.ToString((object)((UltraDropDownBase)cmbgatepass).SelectedRow.Cells["VehicleNo"].Text);
			}
			else
			{
				ResetForReadById();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadById(int Id)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Expected O, but got Unknown
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Expected O, but got Unknown
		try
		{
			btnsave.Visible = false;
			btnupdate.Visible = true;
			RecId = 0;
			RecId2 = 0;
			tabControl1.SelectedIndex = 0;
			ExImVCITransaction exImLc = new ExImVCITransaction();
			exImLc.ExImVCITransactionList = ExImVCITransaction.ReadByGatePassId(Id);
			if (exImLc.ExImVCITransactionList.Count > 0)
			{
				if (exImLc.ExImVCITransactionList[0] != null)
				{
					RecId = Conversion.ToInt((object)exImLc.ExImVCITransactionList[0].Id);
					txtcontainerno1.Text = Conversion.ToString((object)exImLc.ExImVCITransactionList[0].Container1No);
					txtsealno1.Text = Conversion.ToString((object)exImLc.ExImVCITransactionList[0].Seal1No);
					txtinspect1.Text = Conversion.ToString((object)exImLc.ExImVCITransactionList[0].InspectedBy);
					txtverify1.Text = Conversion.ToString((object)exImLc.ExImVCITransactionList[0].VerifiedBy);
					txtpict1.Text = Conversion.ToString((object)exImLc.ExImVCITransactionList[0].PictureTakenBy);
					txtpict1.Text = Conversion.ToString((object)exImLc.ExImVCITransactionList[0].PictureTakenBy);
					txtremarks1.Text = Conversion.ToString((object)exImLc.ExImVCITransactionList[0].InspectorRemarks);
					chkloading1.Checked = Conversion.ToBool((object)exImLc.ExImVCITransactionList[0].IsContainerAccepted);
					if (RecId != 0)
					{
						ExImVCITransaction Obj1 = new ExImVCITransaction();
						Obj1 = ExImVCITransaction.GetByID(RecId);
						if (Obj1 != null)
						{
							dtDocFirst.Clear();
							for (int j = 0; j < Obj1.TransactionDetail.Count; j++)
							{
								dtDocFirst.Rows.Add(Obj1.TransactionDetail[j].Id, Obj1.TransactionDetail[j].VCIParameter, Obj1.TransactionDetail[j].VCICategoryId, Obj1.TransactionDetail[j].VciCategoryDescription, Obj1.TransactionDetail[j].VciParameterDescription, Obj1.TransactionDetail[j].InspectionStatus, Obj1.TransactionDetail[j].Remarks);
							}
							grdDoc01.DataSource = dtDocFirst;
							grdDoc01.RetrieveStructure();
							grdDocFirstsetting();
							FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
						}
						else
						{
							grdDocFirstfill();
						}
					}
					else
					{
						grdDocFirstfill();
					}
				}
				else
				{
					grdDocFirstfill();
				}
				if (exImLc.ExImVCITransactionList.Count > 1)
				{
					RecId2 = Conversion.ToInt((object)exImLc.ExImVCITransactionList[1].Id);
					txtcontainer2.Text = Conversion.ToString((object)exImLc.ExImVCITransactionList[1].Container2No);
					txtseal2.Text = Conversion.ToString((object)exImLc.ExImVCITransactionList[1].Seal2No);
					txtinspect2.Text = Conversion.ToString((object)exImLc.ExImVCITransactionList[1].InspectedBy);
					txtverify2.Text = Conversion.ToString((object)exImLc.ExImVCITransactionList[1].VerifiedBy);
					txtpict2.Text = Conversion.ToString((object)exImLc.ExImVCITransactionList[1].PictureTakenBy);
					txtpict2.Text = Conversion.ToString((object)exImLc.ExImVCITransactionList[1].PictureTakenBy);
					txtremarks2.Text = Conversion.ToString((object)exImLc.ExImVCITransactionList[1].InspectorRemarks);
					chkloading2.Checked = Conversion.ToBool((object)exImLc.ExImVCITransactionList[1].IsContainerAccepted);
					if (RecId2 != 0)
					{
						ExImVCITransaction Obj2 = new ExImVCITransaction();
						Obj2 = ExImVCITransaction.GetByID(RecId2);
						if (Obj2 != null)
						{
							dtDocSecond.Clear();
							for (int i = 0; i < Obj2.TransactionDetail.Count; i++)
							{
								dtDocSecond.Rows.Add(Obj2.TransactionDetail[i].Id, Obj2.TransactionDetail[i].VCIParameter, Obj2.TransactionDetail[i].VCICategoryId, Obj2.TransactionDetail[i].VciCategoryDescription, Obj2.TransactionDetail[i].VciParameterDescription, Obj2.TransactionDetail[i].InspectionStatus, Obj2.TransactionDetail[i].Remarks);
							}
							grdDoc02.DataSource = dtDocSecond;
							grdDoc02.RetrieveStructure();
							grdDocSecondsetting();
						}
						else
						{
							grdDocSecondfill();
						}
						FormHelper.LoadAttachmentsForObject(AT2, RecId2, base.Name);
					}
					else
					{
						grdDocSecondfill();
					}
				}
				else
				{
					grdDocSecondfill();
				}
				Id = ((RecId > 0) ? RecId : RecId2);
			}
			else
			{
				ResetForReadById();
				grdDocFirstfill();
				grdDocSecondfill();
				GetcontainserNoFromGp(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InsertDocFirst()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Expected O, but got Unknown
		try
		{
			ExImVCITransaction obj = new ExImVCITransaction();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					throw new Exception();
				}
				obj.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				throw new Exception();
			}
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchId = UserAccount.BranchesId;
			obj.DocumentTypeId = 180;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.ApprovedDate = DateTime.Now;
			obj.EntryUserId = UserAccount.ID;
			obj.ModifyUserId = UserAccount.ID;
			obj.GatePassOutwardId = Conversion.ToInt(cmbgatepass.Value);
			obj.Container1No = Conversion.ToString((object)txtcontainerno1.Text);
			obj.Seal1No = Conversion.ToString((object)txtsealno1.Text);
			obj.InspectedBy = Conversion.ToString((object)txtinspect1.Text);
			obj.VerifiedBy = Conversion.ToString((object)txtverify1.Text);
			obj.PictureTakenBy = Conversion.ToString((object)txtpict1.Text);
			obj.PictureTakenBy = Conversion.ToString((object)txtpict1.Text);
			obj.InspectorRemarks = Conversion.ToString((object)txtremarks1.Text);
			obj.IsContainerAccepted = Conversion.ToBool((object)chkloading1.Checked);
			obj.TransactionDetail = new List<ExImVCITransactionDetail>();
			GridEXRow[] dataRows = grdDoc01.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				ExImVCITransactionDetail TransDetail = new ExImVCITransactionDetail();
				TransDetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
				TransDetail.VCIParameter = Conversion.ToInt(r.Cells["VCIParameterId"].Value);
				TransDetail.VCICategoryId = Conversion.ToInt(r.Cells["CategoryId"].Value);
				TransDetail.InspectionStatus = Conversion.ToInt(r.Cells["Status"].Value);
				TransDetail.Remarks = Conversion.ToString((object)r.Cells["Remarks"].Text);
				obj.TransactionDetail.Add(TransDetail);
			}
			obj.AttachmentsList = new List<DMSAttachments>();
			obj.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<ExImVCITransaction>(obj, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			ExImVCITransaction.save(obj);
			if (RecId > 0)
			{
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, 180, base.Name);
			}
			ResetgrdDocFirst();
			InsertDocFirstExcuted = true;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)cmbgatepass).Text == "" && Conversion.ToInt(cmbgatepass.Value) == 0)
			{
				MessageBox.Show("Gate Pass No Field is Required");
				cmbgatepass.Focus();
			}
			else if ((!(txtcontainerno1.Text != "") || FormValidationDocFirst()) && (!(txtcontainer2.Text != "") || FormValidationDocSecond()))
			{
				if (ValidatationDocFirstExcuted)
				{
					RecId = 0;
					InsertDocFirst();
				}
				if (ValidatationDocSecondExcuted)
				{
					RecId2 = 0;
					InsertDocSecond();
				}
				if (InsertDocFirstExcuted || InsertDocSecondExcuted)
				{
					MessageBox.Show("Record Save Successfully...");
					ResetMain();
				}
				else
				{
					MessageBox.Show("Container No field is Required...");
					txtcontainerno1.Focus();
				}
			}
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
			if (((Control)(object)cmbgatepass).Text == "" && Conversion.ToInt(cmbgatepass.Value) == 0)
			{
				MessageBox.Show("Gate Pass No Field is Required");
				cmbgatepass.Focus();
			}
			else if ((!(txtcontainerno1.Text != "") || FormValidationDocFirst()) && (!(txtcontainer2.Text != "") || FormValidationDocSecond()))
			{
				if (ValidatationDocFirstExcuted)
				{
					InsertDocFirst();
				}
				if (ValidatationDocSecondExcuted)
				{
					InsertDocSecond();
				}
				if (InsertDocFirstExcuted || InsertDocSecondExcuted)
				{
					MessageBox.Show("Record Updated Successfully...");
					ResetMain();
				}
				else
				{
					MessageBox.Show("Container No fields is Required...");
					txtcontainerno1.Focus();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InsertDocSecond()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Expected O, but got Unknown
		try
		{
			ExImVCITransaction obj = new ExImVCITransaction();
			if (RecId2 > 0)
			{
				obj.Id = RecId2;
			}
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchId = UserAccount.BranchesId;
			obj.DocumentTypeId = 180;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.ApprovedDate = DateTime.Now;
			obj.EntryUserId = UserAccount.ID;
			obj.ModifyUserId = UserAccount.ID;
			obj.GatePassOutwardId = Conversion.ToInt(cmbgatepass.Value);
			obj.Container2No = Conversion.ToString((object)txtcontainer2.Text);
			obj.Seal2No = Conversion.ToString((object)txtseal2.Text);
			obj.Container1No = Conversion.ToString((object)txtcontainer2.Text);
			obj.Seal1No = Conversion.ToString((object)txtseal2.Text);
			obj.InspectedBy = Conversion.ToString((object)txtinspect2.Text);
			obj.VerifiedBy = Conversion.ToString((object)txtverify2.Text);
			obj.PictureTakenBy = Conversion.ToString((object)txtpict2.Text);
			obj.PictureTakenBy = Conversion.ToString((object)txtpict2.Text);
			obj.InspectorRemarks = Conversion.ToString((object)txtremarks2.Text);
			obj.IsContainerAccepted = Conversion.ToBool((object)chkloading2.Checked);
			obj.TransactionDetail = new List<ExImVCITransactionDetail>();
			GridEXRow[] dataRows = grdDoc02.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				ExImVCITransactionDetail TransDetail = new ExImVCITransactionDetail();
				TransDetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
				TransDetail.VCIParameter = Conversion.ToInt(r.Cells["VCIParameterId"].Value);
				TransDetail.VCICategoryId = Conversion.ToInt(r.Cells["CategoryId"].Value);
				TransDetail.InspectionStatus = Conversion.ToInt(r.Cells["Status"].Value);
				TransDetail.Remarks = Conversion.ToString((object)r.Cells["Remarks"].Text);
				obj.TransactionDetail.Add(TransDetail);
			}
			obj.AttachmentsList = new List<DMSAttachments>();
			obj.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<ExImVCITransaction>(obj, base.Name, AT2, "AttachmentsList", "DeleteAttachmentsList");
			ExImVCITransaction.save(obj);
			if (RecId2 > 0)
			{
				FormHelper.DeleteAllAttachmentsFromDb(AT2, RecId, 180, base.Name);
			}
			ResetgrdDocSecond();
			InsertDocSecondExcuted = true;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private bool FormValidationDocFirst()
	{
		if (txtinspect1.Text == "")
		{
			MessageBox.Show("Inspected By Field is Required");
			txtinspect1.Focus();
			return false;
		}
		if (txtverify1.Text == "")
		{
			MessageBox.Show("Verified By Field is Required");
			txtverify1.Focus();
			return false;
		}
		if (txtpict1.Text == "")
		{
			MessageBox.Show("Picture Taken By Field is Required");
			txtpict1.Focus();
			return false;
		}
		if (grdDoc01.GetDataRows().Count() > 0)
		{
			if (grdDoc01.GetDataRows().Count() == 0)
			{
				MessageBox.Show("Grid 01 Is Empty Please Check...");
				return false;
			}
			bool HaveCheckedRow = false;
			GridEXRow[] dataRows = grdDoc01.GetDataRows();
			for (int i = 0; i < dataRows.Length; i++)
			{
				if (Conversion.ToInt(dataRows[i].Cells["Status"].Value) == 1)
				{
					HaveCheckedRow = true;
				}
			}
			if (!HaveCheckedRow)
			{
				MessageBox.Show("AtLeast one Row Should be Checked in Grid 01");
				return false;
			}
			ValidatationDocFirstExcuted = true;
			return true;
		}
		ValidatationDocFirstExcuted = true;
		return true;
	}

	private bool FormValidationDocSecond()
	{
		if (txtinspect2.Text == "")
		{
			MessageBox.Show("Inspected By Field is Required");
			txtinspect2.Focus();
			return false;
		}
		if (txtverify2.Text == "")
		{
			MessageBox.Show("Verified By Field is Required");
			txtverify2.Focus();
			return false;
		}
		if (txtpict2.Text == "")
		{
			MessageBox.Show("Picture Taken By Field is Required");
			txtpict2.Focus();
			return false;
		}
		if (grdDoc02.GetDataRows().Count() > 0)
		{
			if (grdDoc02.GetDataRows().Count() == 0)
			{
				MessageBox.Show("Grid 02 Is Empty Please Check...");
				return false;
			}
			bool HaveCheckedRow = false;
			GridEXRow[] dataRows = grdDoc02.GetDataRows();
			for (int i = 0; i < dataRows.Length; i++)
			{
				if (Conversion.ToInt(dataRows[i].Cells["Status"].Value) == 1)
				{
					HaveCheckedRow = true;
				}
			}
			if (!HaveCheckedRow)
			{
				MessageBox.Show("AtLeast one Row Should be Checked in Grid 02");
				return false;
			}
			ValidatationDocSecondExcuted = true;
			return true;
		}
		ValidatationDocSecondExcuted = true;
		return true;
	}

	private void ResetMain()
	{
		try
		{
			AT = new Attachment();
			AT2 = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			RecId = 0;
			RecId2 = 0;
			txtvehicleno1.Text = string.Empty;
			txtcontainerno1.Text = string.Empty;
			txtcontainer2.Text = string.Empty;
			txtsealno1.Text = string.Empty;
			txtseal2.Text = string.Empty;
			((Control)(object)cmbgatepass).Text = string.Empty;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			ValidatationDocSecondExcuted = false;
			ValidatationDocFirstExcuted = false;
			InsertDocSecondExcuted = false;
			InsertDocFirstExcuted = false;
			cmbgatepass.Focus();
			Gatepassfill(0);
			ResetgrdDocFirst();
			ResetgrdDocSecond();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetForReadById()
	{
		try
		{
			AT = new Attachment();
			AT2 = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			RecId = 0;
			RecId2 = 0;
			txtcontainerno1.Text = string.Empty;
			txtcontainer2.Text = string.Empty;
			txtsealno1.Text = string.Empty;
			txtseal2.Text = string.Empty;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			InsertDocSecondExcuted = false;
			InsertDocFirstExcuted = false;
			ResetgrdDocFirst();
			ResetgrdDocSecond();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GatePassInspection_FormClosed(object sender, FormClosedEventArgs e)
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

	private void btnNew_Click(object sender, EventArgs e)
	{
		try
		{
			ResetMain();
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
			Gatepassfill(0);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetgrdDocFirst()
	{
		GridEXRow[] dataRows = grdDoc01.GetDataRows();
		foreach (GridEXRow obj in dataRows)
		{
			obj.BeginEdit();
			obj.Cells["Status"].Value = false;
			obj.Cells["Remarks"].Value = string.Empty;
			obj.EndEdit();
		}
	}

	private void ResetgrdDocSecond()
	{
		GridEXRow[] dataRows = grdDoc02.GetDataRows();
		foreach (GridEXRow obj in dataRows)
		{
			obj.BeginEdit();
			obj.Cells["Status"].Value = false;
			obj.Cells["Remarks"].Value = string.Empty;
			obj.EndEdit();
		}
	}

	private void grdDocFirstfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = ExImVCIParameter.GetAll(new ExImVCIParameter
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				dtDocFirst.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtDocFirst.Rows.Add(0, dt.Rows[i]["VCIParameterId"], dt.Rows[i]["ExImVCICartegoryId"], dt.Rows[i]["VciCategoryDescription"], dt.Rows[i]["VciParameterDescription"], 0, "");
				}
				grdDoc01.DataSource = dtDocFirst;
				grdDoc01.RetrieveStructure();
				grdDocFirstsetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDocFirstsetting()
	{
		try
		{
			grdDoc01.RootTable.Columns["Id"].Visible = false;
			grdDoc01.RootTable.Columns["VCIParameterId"].Visible = false;
			grdDoc01.RootTable.Columns["CategoryId"].Visible = false;
			grdDoc01.RootTable.Groups.Add("Category");
			grdDoc01.RootTable.Columns["Category"].Visible = false;
			grdDoc01.RootTable.Columns["Status"].EditType = (EditType)0;
			grdDoc01.RootTable.Columns["Remarks"].EditType = (EditType)0;
			grdDoc01.RootTable.Columns["Category"].EditType = (EditType)0;
			grdDoc01.RootTable.Columns["InspectionPoint"].EditType = (EditType)0;
			grdDoc01.RootTable.Columns["Status"].EditType = (EditType)8;
			grdDoc01.RootTable.Columns["Remarks"].EditType = (EditType)1;
			CommonServices.GridAutoAdjustmentNew(grdDoc01);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDocSecondfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt1 = ExImVCIParameter.GetAll(new ExImVCIParameter
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt1.Rows.Count > 0)
			{
				dtDocSecond.Clear();
				for (int i = 0; i < dt1.Rows.Count; i++)
				{
					dtDocSecond.Rows.Add(0, dt1.Rows[i]["VCIParameterId"], dt1.Rows[i]["ExImVCICartegoryId"], dt1.Rows[i]["VciCategoryDescription"], dt1.Rows[i]["VciParameterDescription"], 0, "");
				}
				grdDoc02.DataSource = dtDocSecond;
				grdDoc02.RetrieveStructure();
				grdDocSecondsetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetcontainserNoFromGp(int GpId)
	{
		try
		{
			if (GpId > 0)
			{
				GatePassOutward dt1 = GatePassOutward.GetByID(GpId);
				if (dt1 != null)
				{
					txtcontainerno1.Text = Conversion.ToString((object)dt1.Container);
					txtcontainer2.Text = Conversion.ToString((object)dt1.Container1);
				}
			}
			else
			{
				txtcontainerno1.Text = string.Empty;
				txtcontainer2.Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDocSecondsetting()
	{
		try
		{
			grdDoc02.RootTable.Columns["Id"].Visible = false;
			grdDoc02.RootTable.Columns["VCIParameterId"].Visible = false;
			grdDoc02.RootTable.Columns["CategoryId"].Visible = false;
			grdDoc02.RootTable.Groups.Add("Category");
			grdDoc02.RootTable.Columns["Category"].Visible = false;
			grdDoc02.RootTable.Columns["Status"].EditType = (EditType)0;
			grdDoc02.RootTable.Columns["Remarks"].EditType = (EditType)0;
			grdDoc02.RootTable.Columns["Category"].EditType = (EditType)0;
			grdDoc02.RootTable.Columns["InspectionPoint"].EditType = (EditType)0;
			grdDoc02.RootTable.Columns["Status"].EditType = (EditType)8;
			grdDoc02.RootTable.Columns["Remarks"].EditType = (EditType)1;
			CommonServices.GridAutoAdjustmentNew(grdDoc02);
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
			AT.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GatePassInspection_KeyDown(object sender, KeyEventArgs e)
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
					cmbgatepass.Focus();
					tabControl1.SelectedIndex = 0;
				}
				else
				{
					FromDateHistory.Focus();
					tabControl1.SelectedIndex = 1;
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
				if (e.Control && e.KeyCode == Keys.S && btnsave.Visible && btnsave.Enabled && tabControl1.SelectedIndex == 0)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnupdate.Visible && btnupdate.Enabled && (UpdateMode = true))
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					cmbgatepass.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdDoc01).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					cmbgatepass.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grdDoc01).Focused)
					{
						((Control)(object)grdDoc02).Focus();
					}
					else
					{
						((Control)(object)grdDoc01).Focus();
					}
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					AT.Show();
				}
				if (e.Control && e.KeyCode == Keys.P)
				{
					btnPrint_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F11)
				{
					AT2.Show();
				}
				return;
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnShowHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnResetHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				FromDateHistory.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)DataGridHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				FromDateHistory.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)DataGridHistory).Focused)
				{
					((Control)(object)grdDoc01History).Focus();
				}
				else if (((Control)(object)grdDoc01History).Focused)
				{
					((Control)(object)grdDoc02History).Focus();
				}
				else
				{
					((Control)(object)DataGridHistory).Focus();
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
			dt.Rows.Add("Ctrl+F5", "For Focus on GatePass");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grd Left");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On GatePass in GatePass Inspection");
			dt.Rows.Add("Ctrl+ArrowRight", "For Focus On Grid Right");
			dt.Rows.Add("Ctrl+Enter", "When Focus On Any Grid For Update Record");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortCutKey_Click(object sender, EventArgs e)
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

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0 || DataGridHistory.CurrentColumn == null)
			{
				return;
			}
			int GpId = Conversion.ToInt(r.Cells["GatePassOutwardId"].Value);
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (DataGridHistory.CurrentColumn != null && DataGridHistory.CurrentColumn.Key == "GpSlip")
				{
					CommonServices.OutwardGatePassWithWb(GpId);
				}
				if (DataGridHistory.CurrentColumn != null && DataGridHistory.CurrentColumn.Key == "Edit")
				{
					Gatepassfill(GpId);
					cmbgatepass.Value = GpId;
					cmbgatepass_Leave(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.End)
			{
				Gatepassfill(GpId);
				cmbgatepass.Value = GpId;
				cmbgatepass_Leave(null, null);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				CommonServices.OutwardGatePassWithWb(GpId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FromDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ToDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void FormHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			if (FromDateHistory.Checked)
			{
				obj.FromDate = FromDateHistory.Value;
			}
			if (ToDateHistory.Checked)
			{
				obj.ToDate = ToDateHistory.Value;
			}
			obj.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			DataTable dataTable1 = ExImVCITransaction.GatePassInformationForHistory(obj);
			if (dataTable1.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("GatePassOutwardId", typeof(int));
				dt.Columns.Add("GpDate", typeof(DateTime));
				dt.Columns.Add("GpSrNo", typeof(int));
				dt.Columns.Add("VehicleNo", typeof(string));
				dt.Columns.Add("FactoryWeight", typeof(double));
				dt.Columns.Add("Container1No", typeof(string));
				dt.Columns.Add("Container2No", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				dt.Clear();
				for (int i = 0; i < dataTable1.Rows.Count; i++)
				{
					dt.Rows.Add(dataTable1.Rows[i]["GatePassOutwardId"], dataTable1.Rows[i]["GpDate"], dataTable1.Rows[i]["GpSrNo"], dataTable1.Rows[i]["VehicleNo"], dataTable1.Rows[i]["FactoryWeight"], dataTable1.Rows[i]["Container1No"], dataTable1.Rows[i]["Container2No"], dataTable1.Rows[i]["EntryDate"], dataTable1.Rows[i]["EntryUser"], dataTable1.Rows[i]["ModifyDate"], dataTable1.Rows[i]["ModifyUser"]);
				}
				DataGridHistory.DataSource = dt;
				DataGridHistory.RetrieveStructure();
				DataGridHistorySetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistorySetting()
	{
		try
		{
			DataGridHistory.RootTable.Columns["GatePassOutwardId"].Visible = false;
			DataGridHistory.RootTable.Columns["GpDate"].FormatString = "dd-MM-yyyy";
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["FactoryWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FactoryWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FactoryWeight"].FormatString = "#,##.##";
			DataGridHistory.RootTable.Columns["FactoryWeight"].TotalFormatString = "#,##.##";
			CommonServices.GridAutoAdjustmentNew(DataGridHistory);
			DataGridHistory.RootTable.Columns.Add("GpSlip");
			DataGridHistory.RootTable.Columns["GpSlip"].Caption = "GpSlip";
			DataGridHistory.RootTable.Columns["GpSlip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["GpSlip"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["GpSlip"].ButtonText = "GpSlip";
			DataGridHistory.RootTable.Columns["GpSlip"].Width = 70;
			DataGridHistory.RootTable.Columns["GpSlip"].Position = 0;
			DataGridHistory.RootTable.Columns.Add("Edit");
			DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Width = 50;
			DataGridHistory.RootTable.Columns["Edit"].Position = 1;
			DataGridHistory.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillHistoryDetailByGpId(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ExImVCITransaction exImLc = new ExImVCITransaction();
			exImLc.ExImVCITransactionList = ExImVCITransaction.ReadByGatePassId(Id);
			if (exImLc.ExImVCITransactionList.Count > 0)
			{
				if (exImLc.ExImVCITransactionList[0] != null)
				{
					int Doc01Id = Conversion.ToInt((object)exImLc.ExImVCITransactionList[0].Id);
					txtContainer01History.Text = Conversion.ToString((object)exImLc.ExImVCITransactionList[0].Container1No);
					txtSeal01History.Text = Conversion.ToString((object)exImLc.ExImVCITransactionList[0].Seal1No);
					txtInspectedBy01History.Text = Conversion.ToString((object)exImLc.ExImVCITransactionList[0].InspectedBy);
					txtVerifiedBy01History.Text = Conversion.ToString((object)exImLc.ExImVCITransactionList[0].VerifiedBy);
					txtPicBy01History.Text = Conversion.ToString((object)exImLc.ExImVCITransactionList[0].PictureTakenBy);
					txtRemarks01History.Text = Conversion.ToString((object)exImLc.ExImVCITransactionList[0].InspectorRemarks);
					chkContainerAccepted01History.Checked = Conversion.ToBool((object)exImLc.ExImVCITransactionList[0].IsContainerAccepted);
					if (Doc01Id != 0)
					{
						ExImVCITransaction Obj1 = ExImVCITransaction.GetByID(Doc01Id);
						if (Obj1 != null)
						{
							DataTable dt2 = new DataTable();
							dt2.Columns.Add("Id", typeof(int));
							dt2.Columns.Add("VCIParameterId", typeof(int));
							dt2.Columns.Add("CategoryId", typeof(int));
							dt2.Columns.Add("Category", typeof(string));
							dt2.Columns.Add("InspectionPoint", typeof(string));
							dt2.Columns.Add("Status", typeof(bool));
							dt2.Columns.Add("Remarks", typeof(string));
							dt2.Clear();
							for (int j = 0; j < Obj1.TransactionDetail.Count; j++)
							{
								if (Obj1.TransactionDetail[j].InspectionStatus == 1)
								{
									dt2.Rows.Add(Obj1.TransactionDetail[j].Id, Obj1.TransactionDetail[j].VCIParameter, Obj1.TransactionDetail[j].VCICategoryId, Obj1.TransactionDetail[j].VciCategoryDescription, Obj1.TransactionDetail[j].VciParameterDescription, Obj1.TransactionDetail[j].InspectionStatus, Obj1.TransactionDetail[j].Remarks);
								}
							}
							grdDoc01History.DataSource = dt2;
							grdDoc01History.RetrieveStructure();
							grdDocFirstHistorysetting();
						}
						else
						{
							grdDoc01History.ClearStructure();
						}
					}
					else
					{
						grdDoc01History.ClearStructure();
					}
				}
				else
				{
					grdDoc01History.ClearStructure();
				}
				if (exImLc.ExImVCITransactionList.Count > 1)
				{
					int Doc02Id = Conversion.ToInt((object)exImLc.ExImVCITransactionList[1].Id);
					txtContainer02History.Text = Conversion.ToString((object)exImLc.ExImVCITransactionList[1].Container2No);
					txtSeal02History.Text = Conversion.ToString((object)exImLc.ExImVCITransactionList[1].Seal2No);
					txtInspectedBy02History.Text = Conversion.ToString((object)exImLc.ExImVCITransactionList[1].InspectedBy);
					txtVerifiedBy02History.Text = Conversion.ToString((object)exImLc.ExImVCITransactionList[1].VerifiedBy);
					txtPicBy02History.Text = Conversion.ToString((object)exImLc.ExImVCITransactionList[1].PictureTakenBy);
					txtRemarks02History.Text = Conversion.ToString((object)exImLc.ExImVCITransactionList[1].InspectorRemarks);
					chkContainerAccepted02History.Checked = Conversion.ToBool((object)exImLc.ExImVCITransactionList[1].IsContainerAccepted);
					if (Doc02Id != 0)
					{
						ExImVCITransaction Obj2 = ExImVCITransaction.GetByID(Doc02Id);
						if (Obj2 != null)
						{
							DataTable dt = new DataTable();
							dt.Columns.Add("Id", typeof(int));
							dt.Columns.Add("VCIParameterId", typeof(int));
							dt.Columns.Add("CategoryId", typeof(int));
							dt.Columns.Add("Category", typeof(string));
							dt.Columns.Add("InspectionPoint", typeof(string));
							dt.Columns.Add("Status", typeof(bool));
							dt.Columns.Add("Remarks", typeof(string));
							dt.Clear();
							for (int i = 0; i < Obj2.TransactionDetail.Count; i++)
							{
								if (Obj2.TransactionDetail[i].InspectionStatus == 1)
								{
									dt.Rows.Add(Obj2.TransactionDetail[i].Id, Obj2.TransactionDetail[i].VCIParameter, Obj2.TransactionDetail[i].VCICategoryId, Obj2.TransactionDetail[i].VciCategoryDescription, Obj2.TransactionDetail[i].VciParameterDescription, Obj2.TransactionDetail[i].InspectionStatus, Obj2.TransactionDetail[i].Remarks);
								}
							}
							grdDoc02History.DataSource = dt;
							grdDoc02History.RetrieveStructure();
							grdDocSecondHistorysetting();
						}
						else
						{
							grdDoc02History.ClearStructure();
						}
					}
					else
					{
						grdDoc02History.ClearStructure();
					}
				}
				else
				{
					grdDoc02History.ClearStructure();
				}
			}
			else
			{
				grdDoc01History.ClearStructure();
				grdDoc02History.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FormHistory();
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
			FromDocNoHistory.Clear();
			ToDocNoHistory.Clear();
			txtContainer01History.Text = string.Empty;
			txtSeal01History.Text = string.Empty;
			txtInspectedBy01History.Text = string.Empty;
			txtPicBy01History.Text = string.Empty;
			txtVerifiedBy01History.Text = string.Empty;
			chkContainerAccepted01History.Text = string.Empty;
			txtRemarks01History.Text = string.Empty;
			txtContainer02History.Text = string.Empty;
			txtSeal02History.Text = string.Empty;
			txtInspectedBy02History.Text = string.Empty;
			txtPicBy02History.Text = string.Empty;
			txtVerifiedBy02History.Text = string.Empty;
			chkContainerAccepted02History.Text = string.Empty;
			txtRemarks02History.Text = string.Empty;
			DataGridHistory.DataSource = null;
			grdDoc01History.DataSource = null;
			grdDoc02History.DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdDocSecondHistorysetting()
	{
		try
		{
			grdDoc02History.RootTable.Columns["Id"].Visible = false;
			grdDoc02History.RootTable.Columns["VCIParameterId"].Visible = false;
			grdDoc02History.RootTable.Columns["CategoryId"].Visible = false;
			CommonServices.GridAutoAdjustmentNew(grdDoc02History);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDocFirstHistorysetting()
	{
		try
		{
			grdDoc01History.RootTable.Columns["Id"].Visible = false;
			grdDoc01History.RootTable.Columns["VCIParameterId"].Visible = false;
			grdDoc01History.RootTable.Columns["CategoryId"].Visible = false;
			CommonServices.GridAutoAdjustmentNew(grdDoc01History);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int GpId = Conversion.ToInt(r.Cells["GatePassOutwardId"].Value);
				if (e.Column.Key == "GpSlip")
				{
					CommonServices.OutwardGatePassWithWb(GpId);
				}
				if (e.Column.Key == "Edit")
				{
					Gatepassfill(GpId);
					cmbgatepass.Value = GpId;
					cmbgatepass_Leave(null, null);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int GpId = Conversion.ToInt(r.Cells["GatePassOutwardId"].Value);
				FillHistoryDetailByGpId(GpId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			cmbgatepass.Focus();
		}
		else
		{
			FromDateHistory.Focus();
		}
	}

	private void btnAttachmentsForContainer2_Click(object sender, EventArgs e)
	{
		try
		{
			AT2.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataSet ds = ExImVCITransaction.ExImVCITransaction_SlipAndRegister(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				GpId = Conversion.ToInt(cmbgatepass.Value)
			});
			DataTable dt = ds?.Tables?[0]?.Copy();
			if (dt.Rows.Count > 0)
			{
				DatatableHelper.ReplaceCompLogoColumn(dt, (ds.Tables[1].Rows.Count > 0) ? ds.Tables[1] : null);
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.ShowReportWithDataTable(dt, "244-GatePassInspectionSlip.rpt");
				return;
			}
			throw new Exception("Record not found for display");
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
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Expected O, but got Unknown
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Expected O, but got Unknown
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Expected O, but got Unknown
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Expected O, but got Unknown
		//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Expected O, but got Unknown
		//IL_3015: Unknown result type (might be due to invalid IL or missing references)
		//IL_301f: Expected O, but got Unknown
		GridEXLayout grdDoc02_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDoc01_DesignTimeLayout = new GridEXLayout();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
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
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDoc02History_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDoc01History_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grdDoc02 = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel11 = new System.Windows.Forms.Panel();
		this.boxcontainer2 = new System.Windows.Forms.GroupBox();
		this.txtremarks2 = new System.Windows.Forms.RichTextBox();
		this.chkloading2 = new System.Windows.Forms.CheckBox();
		this.txtpict2 = new System.Windows.Forms.TextBox();
		this.txtverify2 = new System.Windows.Forms.TextBox();
		this.txtinspect2 = new System.Windows.Forms.TextBox();
		this.txtcontainer2 = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.txtseal2 = new System.Windows.Forms.TextBox();
		this.panel12 = new System.Windows.Forms.Panel();
		this.grdDoc01 = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel14 = new System.Windows.Forms.Panel();
		this.boxcontainer1 = new System.Windows.Forms.GroupBox();
		this.txtremarks1 = new System.Windows.Forms.RichTextBox();
		this.chkloading1 = new System.Windows.Forms.CheckBox();
		this.txtpict1 = new System.Windows.Forms.TextBox();
		this.txtverify1 = new System.Windows.Forms.TextBox();
		this.txtinspect1 = new System.Windows.Forms.TextBox();
		this.txtcontainerno1 = new System.Windows.Forms.TextBox();
		this.label18 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.txtsealno1 = new System.Windows.Forms.TextBox();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.cmbgatepass = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.txtvehicleno1 = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnAttachmentsForContainer1 = new System.Windows.Forms.ToolStripButton();
		this.btnAttachmentsForContainer2 = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.DataGridHistory = new GridEX();
		this.panel20 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdDoc02History = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.chkContainerAccepted02History = new System.Windows.Forms.CheckBox();
		this.txtRemarks02History = new System.Windows.Forms.TextBox();
		this.txtPicBy02History = new System.Windows.Forms.TextBox();
		this.txtVerifiedBy02History = new System.Windows.Forms.TextBox();
		this.txtInspectedBy02History = new System.Windows.Forms.TextBox();
		this.txtContainer02History = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.txtSeal02History = new System.Windows.Forms.TextBox();
		this.panel17 = new System.Windows.Forms.Panel();
		this.grdDoc01History = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.panel19 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.chkContainerAccepted01History = new System.Windows.Forms.CheckBox();
		this.txtRemarks01History = new System.Windows.Forms.TextBox();
		this.txtPicBy01History = new System.Windows.Forms.TextBox();
		this.txtVerifiedBy01History = new System.Windows.Forms.TextBox();
		this.txtInspectedBy01History = new System.Windows.Forms.TextBox();
		this.txtContainer01History = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.txtSeal01History = new System.Windows.Forms.TextBox();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label92 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.label73 = new System.Windows.Forms.Label();
		this.label89 = new System.Windows.Forms.Label();
		this.label90 = new System.Windows.Forms.Label();
		this.label91 = new System.Windows.Forms.Label();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label69 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDoc02).BeginInit();
		this.panel11.SuspendLayout();
		this.boxcontainer2.SuspendLayout();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDoc01).BeginInit();
		this.panel14.SuspendLayout();
		this.boxcontainer1.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbgatepass).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel20.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDoc02History).BeginInit();
		this.panel13.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDoc01History).BeginInit();
		this.panel19.SuspendLayout();
		this.groupBox4.SuspendLayout();
		this.panel15.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.panel16.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(959, 661);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(959, 661);
		this.tabControl1.TabIndex = 1;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(951, 632);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(945, 626);
		this.panel2.TabIndex = 0;
		this.panel4.Controls.Add(this.panel7);
		this.panel4.Controls.Add(this.panel12);
		this.panel4.Controls.Add(this.groupBox3);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 57);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(945, 569);
		this.panel4.TabIndex = 12;
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdDoc02);
		this.panel7.Controls.Add(this.panel9);
		this.panel7.Controls.Add(this.panel11);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(469, 54);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(476, 515);
		this.panel7.TabIndex = 22;
		this.grdDoc02.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdDoc02).BackColor = System.Drawing.Color.White;
		grdDoc02_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDoc02.DesignTimeLayout = grdDoc02_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDoc02).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdDoc02).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDoc02.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDoc02).Location = new System.Drawing.Point(0, 163);
		((System.Windows.Forms.Control)(object)this.grdDoc02).Name = "grdDoc02";
		((ScrollableBase)this.grdDoc02).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDoc02).Size = new System.Drawing.Size(476, 352);
		((System.Windows.Forms.Control)(object)this.grdDoc02).TabIndex = 0;
		this.grdDoc02.TotalRow = (InheritableBoolean)1;
		this.grdDoc02.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDoc02).VisualStyle = (VisualStyle)4;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 136);
		this.panel9.Margin = new System.Windows.Forms.Padding(2);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(476, 27);
		this.panel9.TabIndex = 5;
		this.panel11.BackColor = System.Drawing.Color.White;
		this.panel11.Controls.Add(this.boxcontainer2);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Margin = new System.Windows.Forms.Padding(2);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(476, 136);
		this.panel11.TabIndex = 4;
		this.boxcontainer2.Controls.Add(this.txtremarks2);
		this.boxcontainer2.Controls.Add(this.chkloading2);
		this.boxcontainer2.Controls.Add(this.txtpict2);
		this.boxcontainer2.Controls.Add(this.txtverify2);
		this.boxcontainer2.Controls.Add(this.txtinspect2);
		this.boxcontainer2.Controls.Add(this.txtcontainer2);
		this.boxcontainer2.Controls.Add(this.label6);
		this.boxcontainer2.Controls.Add(this.label7);
		this.boxcontainer2.Controls.Add(this.label8);
		this.boxcontainer2.Controls.Add(this.label9);
		this.boxcontainer2.Controls.Add(this.label10);
		this.boxcontainer2.Controls.Add(this.label11);
		this.boxcontainer2.Controls.Add(this.txtseal2);
		this.boxcontainer2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.boxcontainer2.Location = new System.Drawing.Point(3, 3);
		this.boxcontainer2.Name = "boxcontainer2";
		this.boxcontainer2.Size = new System.Drawing.Size(466, 130);
		this.boxcontainer2.TabIndex = 2;
		this.boxcontainer2.TabStop = false;
		this.boxcontainer2.Text = "Container  02 Information";
		this.txtremarks2.Location = new System.Drawing.Point(101, 100);
		this.txtremarks2.Name = "txtremarks2";
		this.txtremarks2.Size = new System.Drawing.Size(361, 23);
		this.txtremarks2.TabIndex = 6;
		this.txtremarks2.Text = "";
		this.chkloading2.AutoSize = true;
		this.chkloading2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkloading2.Location = new System.Drawing.Point(251, 75);
		this.chkloading2.Name = "chkloading2";
		this.chkloading2.Size = new System.Drawing.Size(196, 19);
		this.chkloading2.TabIndex = 5;
		this.chkloading2.Text = "Container Accepted For Loading";
		this.chkloading2.UseVisualStyleBackColor = true;
		this.txtpict2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtpict2.Location = new System.Drawing.Point(101, 73);
		this.txtpict2.Margin = new System.Windows.Forms.Padding(2);
		this.txtpict2.Name = "txtpict2";
		this.txtpict2.Size = new System.Drawing.Size(145, 23);
		this.txtpict2.TabIndex = 4;
		this.txtverify2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtverify2.Location = new System.Drawing.Point(315, 45);
		this.txtverify2.Margin = new System.Windows.Forms.Padding(2);
		this.txtverify2.Name = "txtverify2";
		this.txtverify2.Size = new System.Drawing.Size(147, 23);
		this.txtverify2.TabIndex = 3;
		this.txtinspect2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtinspect2.Location = new System.Drawing.Point(101, 45);
		this.txtinspect2.Margin = new System.Windows.Forms.Padding(2);
		this.txtinspect2.Name = "txtinspect2";
		this.txtinspect2.Size = new System.Drawing.Size(145, 23);
		this.txtinspect2.TabIndex = 2;
		this.txtcontainer2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcontainer2.Location = new System.Drawing.Point(101, 18);
		this.txtcontainer2.Margin = new System.Windows.Forms.Padding(2);
		this.txtcontainer2.Name = "txtcontainer2";
		this.txtcontainer2.Size = new System.Drawing.Size(145, 23);
		this.txtcontainer2.TabIndex = 0;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.Black;
		this.label6.Location = new System.Drawing.Point(7, 101);
		this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(52, 15);
		this.label6.TabIndex = 16;
		this.label6.Text = "Remarks";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.Black;
		this.label7.Location = new System.Drawing.Point(7, 77);
		this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(94, 15);
		this.label7.TabIndex = 15;
		this.label7.Text = "Picture Taken By";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.Black;
		this.label8.Location = new System.Drawing.Point(251, 22);
		this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(39, 15);
		this.label8.TabIndex = 10;
		this.label8.Text = "Seal #";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.Black;
		this.label9.Location = new System.Drawing.Point(251, 49);
		this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(63, 15);
		this.label9.TabIndex = 14;
		this.label9.Text = "Verified By";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.Black;
		this.label10.Location = new System.Drawing.Point(7, 22);
		this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(68, 15);
		this.label10.TabIndex = 5;
		this.label10.Text = "Container #";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.Black;
		this.label11.Location = new System.Drawing.Point(7, 49);
		this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(75, 15);
		this.label11.TabIndex = 13;
		this.label11.Text = "Inspected By";
		this.txtseal2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtseal2.Location = new System.Drawing.Point(315, 18);
		this.txtseal2.Margin = new System.Windows.Forms.Padding(2);
		this.txtseal2.Name = "txtseal2";
		this.txtseal2.Size = new System.Drawing.Size(147, 23);
		this.txtseal2.TabIndex = 1;
		this.panel12.BackColor = System.Drawing.Color.White;
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.grdDoc01);
		this.panel12.Controls.Add(this.panel6);
		this.panel12.Controls.Add(this.panel14);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel12.Location = new System.Drawing.Point(0, 54);
		this.panel12.Margin = new System.Windows.Forms.Padding(2);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(469, 515);
		this.panel12.TabIndex = 21;
		this.grdDoc01.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdDoc01).BackColor = System.Drawing.Color.White;
		grdDoc01_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDoc01.DesignTimeLayout = grdDoc01_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDoc01).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdDoc01).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDoc01.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDoc01).Location = new System.Drawing.Point(0, 163);
		((System.Windows.Forms.Control)(object)this.grdDoc01).Name = "grdDoc01";
		((ScrollableBase)this.grdDoc01).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDoc01).Size = new System.Drawing.Size(469, 352);
		((System.Windows.Forms.Control)(object)this.grdDoc01).TabIndex = 0;
		this.grdDoc01.TotalRow = (InheritableBoolean)1;
		this.grdDoc01.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDoc01).VisualStyle = (VisualStyle)4;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 136);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(469, 27);
		this.panel6.TabIndex = 5;
		this.panel14.BackColor = System.Drawing.Color.White;
		this.panel14.Controls.Add(this.boxcontainer1);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Margin = new System.Windows.Forms.Padding(2);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(469, 136);
		this.panel14.TabIndex = 4;
		this.boxcontainer1.Controls.Add(this.txtremarks1);
		this.boxcontainer1.Controls.Add(this.chkloading1);
		this.boxcontainer1.Controls.Add(this.txtpict1);
		this.boxcontainer1.Controls.Add(this.txtverify1);
		this.boxcontainer1.Controls.Add(this.txtinspect1);
		this.boxcontainer1.Controls.Add(this.txtcontainerno1);
		this.boxcontainer1.Controls.Add(this.label18);
		this.boxcontainer1.Controls.Add(this.label17);
		this.boxcontainer1.Controls.Add(this.label5);
		this.boxcontainer1.Controls.Add(this.label16);
		this.boxcontainer1.Controls.Add(this.label3);
		this.boxcontainer1.Controls.Add(this.label15);
		this.boxcontainer1.Controls.Add(this.txtsealno1);
		this.boxcontainer1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.boxcontainer1.Location = new System.Drawing.Point(4, 3);
		this.boxcontainer1.Name = "boxcontainer1";
		this.boxcontainer1.Size = new System.Drawing.Size(462, 128);
		this.boxcontainer1.TabIndex = 1;
		this.boxcontainer1.TabStop = false;
		this.boxcontainer1.Text = "Container  01 Information";
		this.txtremarks1.Location = new System.Drawing.Point(100, 100);
		this.txtremarks1.Name = "txtremarks1";
		this.txtremarks1.Size = new System.Drawing.Size(357, 23);
		this.txtremarks1.TabIndex = 6;
		this.txtremarks1.Text = "";
		this.chkloading1.AutoSize = true;
		this.chkloading1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkloading1.Location = new System.Drawing.Point(250, 76);
		this.chkloading1.Name = "chkloading1";
		this.chkloading1.Size = new System.Drawing.Size(196, 19);
		this.chkloading1.TabIndex = 5;
		this.chkloading1.Text = "Container Accepted For Loading";
		this.chkloading1.UseVisualStyleBackColor = true;
		this.txtpict1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtpict1.Location = new System.Drawing.Point(100, 74);
		this.txtpict1.Margin = new System.Windows.Forms.Padding(2);
		this.txtpict1.Name = "txtpict1";
		this.txtpict1.Size = new System.Drawing.Size(145, 23);
		this.txtpict1.TabIndex = 4;
		this.txtverify1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtverify1.Location = new System.Drawing.Point(310, 46);
		this.txtverify1.Margin = new System.Windows.Forms.Padding(2);
		this.txtverify1.Name = "txtverify1";
		this.txtverify1.Size = new System.Drawing.Size(147, 23);
		this.txtverify1.TabIndex = 3;
		this.txtinspect1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtinspect1.Location = new System.Drawing.Point(100, 46);
		this.txtinspect1.Margin = new System.Windows.Forms.Padding(2);
		this.txtinspect1.Name = "txtinspect1";
		this.txtinspect1.Size = new System.Drawing.Size(145, 23);
		this.txtinspect1.TabIndex = 2;
		this.txtcontainerno1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcontainerno1.Location = new System.Drawing.Point(100, 19);
		this.txtcontainerno1.Margin = new System.Windows.Forms.Padding(2);
		this.txtcontainerno1.Name = "txtcontainerno1";
		this.txtcontainerno1.Size = new System.Drawing.Size(145, 23);
		this.txtcontainerno1.TabIndex = 0;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.Color.Black;
		this.label18.Location = new System.Drawing.Point(5, 100);
		this.label18.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(52, 15);
		this.label18.TabIndex = 16;
		this.label18.Text = "Remarks";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.Black;
		this.label17.Location = new System.Drawing.Point(5, 78);
		this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(94, 15);
		this.label17.TabIndex = 15;
		this.label17.Text = "Picture Taken By";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.Black;
		this.label5.Location = new System.Drawing.Point(246, 23);
		this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(39, 15);
		this.label5.TabIndex = 10;
		this.label5.Text = "Seal #";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.Black;
		this.label16.Location = new System.Drawing.Point(247, 50);
		this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(63, 15);
		this.label16.TabIndex = 14;
		this.label16.Text = "Verified By";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.Black;
		this.label3.Location = new System.Drawing.Point(5, 23);
		this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(68, 15);
		this.label3.TabIndex = 5;
		this.label3.Text = "Container #";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.Black;
		this.label15.Location = new System.Drawing.Point(5, 50);
		this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(75, 15);
		this.label15.TabIndex = 13;
		this.label15.Text = "Inspected By";
		this.txtsealno1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtsealno1.Location = new System.Drawing.Point(310, 19);
		this.txtsealno1.Margin = new System.Windows.Forms.Padding(2);
		this.txtsealno1.Name = "txtsealno1";
		this.txtsealno1.Size = new System.Drawing.Size(147, 23);
		this.txtsealno1.TabIndex = 1;
		this.groupBox3.BackColor = System.Drawing.Color.White;
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbgatepass);
		this.groupBox3.Controls.Add(this.label2);
		this.groupBox3.Controls.Add(this.txtvehicleno1);
		this.groupBox3.Controls.Add(this.label4);
		this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(0, 0);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(945, 54);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Gate Pass Information";
		this.cmbgatepass.AllowNull = (DefaultableBoolean)1;
		this.cmbgatepass.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbgatepass.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbgatepass.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbgatepass).DisplayLayout.Appearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbgatepass).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbgatepass).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance86).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance86).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbgatepass).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbgatepass).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance87;
		((SpecialBoxBase)((UltraGridBase)this.cmbgatepass).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbgatepass).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbgatepass).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbgatepass).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbgatepass).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbgatepass).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbgatepass).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbgatepass).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbgatepass).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance92).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbgatepass).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbgatepass).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbgatepass).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbgatepass).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbgatepass).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmbgatepass).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbgatepass).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbgatepass).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbgatepass).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbgatepass).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbgatepass).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbgatepass).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbgatepass).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbgatepass).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbgatepass.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbgatepass).Location = new System.Drawing.Point(100, 20);
		((System.Windows.Forms.Control)(object)this.cmbgatepass).Name = "cmbgatepass";
		((System.Windows.Forms.Control)(object)this.cmbgatepass).Size = new System.Drawing.Size(145, 26);
		((System.Windows.Forms.Control)(object)this.cmbgatepass).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbgatepass).Leave += new System.EventHandler(cmbgatepass_Leave);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.Black;
		this.label2.Location = new System.Drawing.Point(5, 26);
		this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(73, 15);
		this.label2.TabIndex = 4;
		this.label2.Text = "GatePass No";
		this.txtvehicleno1.Font = new System.Drawing.Font("Verdana", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtvehicleno1.Location = new System.Drawing.Point(310, 21);
		this.txtvehicleno1.Margin = new System.Windows.Forms.Padding(2);
		this.txtvehicleno1.Name = "txtvehicleno1";
		this.txtvehicleno1.ReadOnly = true;
		this.txtvehicleno1.Size = new System.Drawing.Size(147, 24);
		this.txtvehicleno1.TabIndex = 1;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.Black;
		this.label4.Location = new System.Drawing.Point(246, 26);
		this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(55, 15);
		this.label4.TabIndex = 8;
		this.label4.Text = "Vehicle #";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 31);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(945, 26);
		this.panel3.TabIndex = 3;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(5, 5);
		this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(232, 17);
		this.label1.TabIndex = 0;
		this.label1.Text = "Vehicle Cargo Inspection Report";
		this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.btnNew, this.btnRefresh, this.btnsave, this.btnupdate, this.btnAttachmentsForContainer1, this.btnAttachmentsForContainer2, this.btnShortCutKey, this.btnPrint });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(945, 31);
		this.toolStrip1.TabIndex = 2;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.BackColor = System.Drawing.Color.White;
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.ForeColor = System.Drawing.Color.Black;
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(63, 28);
		this.btnNew.Text = "New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.BackColor = System.Drawing.Color.White;
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.ForeColor = System.Drawing.Color.Black;
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(84, 28);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.ForeColor = System.Drawing.Color.Black;
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(68, 28);
		this.btnsave.Text = "Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.ForeColor = System.Drawing.Color.Black;
		this.btnupdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(82, 28);
		this.btnupdate.Text = "Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnAttachmentsForContainer1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachmentsForContainer1.ForeColor = System.Drawing.Color.Black;
		this.btnAttachmentsForContainer1.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachmentsForContainer1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachmentsForContainer1.Name = "btnAttachmentsForContainer1";
		this.btnAttachmentsForContainer1.Size = new System.Drawing.Size(201, 28);
		this.btnAttachmentsForContainer1.Text = "Attachments Container 1";
		this.btnAttachmentsForContainer1.Click += new System.EventHandler(toolStripButton1_Click);
		this.btnAttachmentsForContainer2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachmentsForContainer2.ForeColor = System.Drawing.Color.Black;
		this.btnAttachmentsForContainer2.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachmentsForContainer2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachmentsForContainer2.Name = "btnAttachmentsForContainer2";
		this.btnAttachmentsForContainer2.Size = new System.Drawing.Size(201, 28);
		this.btnAttachmentsForContainer2.Text = "Attachments Container 2";
		this.btnAttachmentsForContainer2.Click += new System.EventHandler(btnAttachmentsForContainer2_Click);
		this.btnShortCutKey.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKey.ForeColor = System.Drawing.Color.Black;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(93, 28);
		this.btnShortCutKey.Text = "ShorCut Key";
		this.btnShortCutKey.Visible = false;
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(96, 28);
		this.btnPrint.Text = "Print-244";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.panel20);
		this.tabPage2.Controls.Add(this.panel15);
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Controls.Add(this.panel16);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(951, 632);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(3, 120);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.DataGridHistory).Office2007CustomColor = System.Drawing.Color.LimeGreen;
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(945, 161);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel20.Controls.Add(this.panel8);
		this.panel20.Controls.Add(this.panel17);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel20.Location = new System.Drawing.Point(3, 281);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(945, 348);
		this.panel20.TabIndex = 53;
		this.panel8.BackColor = System.Drawing.Color.White;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdDoc02History);
		this.panel8.Controls.Add(this.panel10);
		this.panel8.Controls.Add(this.panel13);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(469, 0);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(476, 348);
		this.panel8.TabIndex = 52;
		this.grdDoc02History.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDoc02History).BackColor = System.Drawing.Color.White;
		grdDoc02History_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDoc02History.DesignTimeLayout = grdDoc02History_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDoc02History).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdDoc02History).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDoc02History.GroupByBoxVisible = false;
		this.grdDoc02History.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDoc02History).Location = new System.Drawing.Point(0, 141);
		((System.Windows.Forms.Control)(object)this.grdDoc02History).Name = "grdDoc02History";
		((ScrollableBase)this.grdDoc02History).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDoc02History).Size = new System.Drawing.Size(476, 207);
		((System.Windows.Forms.Control)(object)this.grdDoc02History).TabIndex = 0;
		this.grdDoc02History.TotalRow = (InheritableBoolean)1;
		this.grdDoc02History.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDoc02History).VisualStyle = (VisualStyle)4;
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 121);
		this.panel10.Margin = new System.Windows.Forms.Padding(2);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(476, 20);
		this.panel10.TabIndex = 5;
		this.panel13.BackColor = System.Drawing.Color.White;
		this.panel13.Controls.Add(this.groupBox1);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Margin = new System.Windows.Forms.Padding(2);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(476, 121);
		this.panel13.TabIndex = 4;
		this.groupBox1.Controls.Add(this.chkContainerAccepted02History);
		this.groupBox1.Controls.Add(this.txtRemarks02History);
		this.groupBox1.Controls.Add(this.txtPicBy02History);
		this.groupBox1.Controls.Add(this.txtVerifiedBy02History);
		this.groupBox1.Controls.Add(this.txtInspectedBy02History);
		this.groupBox1.Controls.Add(this.txtContainer02History);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add(this.txtSeal02History);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.groupBox1.Location = new System.Drawing.Point(3, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(466, 116);
		this.groupBox1.TabIndex = 2;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Container  02 Information";
		this.chkContainerAccepted02History.AutoSize = true;
		this.chkContainerAccepted02History.Enabled = false;
		this.chkContainerAccepted02History.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkContainerAccepted02History.Location = new System.Drawing.Point(251, 67);
		this.chkContainerAccepted02History.Name = "chkContainerAccepted02History";
		this.chkContainerAccepted02History.Size = new System.Drawing.Size(196, 19);
		this.chkContainerAccepted02History.TabIndex = 5;
		this.chkContainerAccepted02History.Text = "Container Accepted For Loading";
		this.chkContainerAccepted02History.UseVisualStyleBackColor = true;
		this.txtRemarks02History.Enabled = false;
		this.txtRemarks02History.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks02History.Location = new System.Drawing.Point(101, 90);
		this.txtRemarks02History.Margin = new System.Windows.Forms.Padding(2);
		this.txtRemarks02History.Name = "txtRemarks02History";
		this.txtRemarks02History.Size = new System.Drawing.Size(361, 23);
		this.txtRemarks02History.TabIndex = 4;
		this.txtPicBy02History.Enabled = false;
		this.txtPicBy02History.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPicBy02History.Location = new System.Drawing.Point(101, 65);
		this.txtPicBy02History.Margin = new System.Windows.Forms.Padding(2);
		this.txtPicBy02History.Name = "txtPicBy02History";
		this.txtPicBy02History.Size = new System.Drawing.Size(145, 23);
		this.txtPicBy02History.TabIndex = 4;
		this.txtVerifiedBy02History.Enabled = false;
		this.txtVerifiedBy02History.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVerifiedBy02History.Location = new System.Drawing.Point(315, 40);
		this.txtVerifiedBy02History.Margin = new System.Windows.Forms.Padding(2);
		this.txtVerifiedBy02History.Name = "txtVerifiedBy02History";
		this.txtVerifiedBy02History.Size = new System.Drawing.Size(147, 23);
		this.txtVerifiedBy02History.TabIndex = 3;
		this.txtInspectedBy02History.Enabled = false;
		this.txtInspectedBy02History.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInspectedBy02History.Location = new System.Drawing.Point(101, 40);
		this.txtInspectedBy02History.Margin = new System.Windows.Forms.Padding(2);
		this.txtInspectedBy02History.Name = "txtInspectedBy02History";
		this.txtInspectedBy02History.Size = new System.Drawing.Size(145, 23);
		this.txtInspectedBy02History.TabIndex = 2;
		this.txtContainer02History.Enabled = false;
		this.txtContainer02History.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtContainer02History.Location = new System.Drawing.Point(101, 15);
		this.txtContainer02History.Margin = new System.Windows.Forms.Padding(2);
		this.txtContainer02History.Name = "txtContainer02History";
		this.txtContainer02History.Size = new System.Drawing.Size(145, 23);
		this.txtContainer02History.TabIndex = 0;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.Black;
		this.label12.Location = new System.Drawing.Point(7, 94);
		this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(52, 15);
		this.label12.TabIndex = 16;
		this.label12.Text = "Remarks";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.Black;
		this.label13.Location = new System.Drawing.Point(7, 69);
		this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(94, 15);
		this.label13.TabIndex = 15;
		this.label13.Text = "Picture Taken By";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.Black;
		this.label14.Location = new System.Drawing.Point(251, 19);
		this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(39, 15);
		this.label14.TabIndex = 10;
		this.label14.Text = "Seal #";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.Black;
		this.label19.Location = new System.Drawing.Point(251, 44);
		this.label19.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(63, 15);
		this.label19.TabIndex = 14;
		this.label19.Text = "Verified By";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.ForeColor = System.Drawing.Color.Black;
		this.label20.Location = new System.Drawing.Point(7, 19);
		this.label20.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(68, 15);
		this.label20.TabIndex = 5;
		this.label20.Text = "Container #";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.Black;
		this.label21.Location = new System.Drawing.Point(7, 44);
		this.label21.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(75, 15);
		this.label21.TabIndex = 13;
		this.label21.Text = "Inspected By";
		this.txtSeal02History.Enabled = false;
		this.txtSeal02History.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSeal02History.Location = new System.Drawing.Point(315, 15);
		this.txtSeal02History.Margin = new System.Windows.Forms.Padding(2);
		this.txtSeal02History.Name = "txtSeal02History";
		this.txtSeal02History.Size = new System.Drawing.Size(147, 23);
		this.txtSeal02History.TabIndex = 1;
		this.panel17.BackColor = System.Drawing.Color.White;
		this.panel17.Controls.Add((System.Windows.Forms.Control)(object)this.grdDoc01History);
		this.panel17.Controls.Add(this.panel18);
		this.panel17.Controls.Add(this.panel19);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel17.Location = new System.Drawing.Point(0, 0);
		this.panel17.Margin = new System.Windows.Forms.Padding(2);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(469, 348);
		this.panel17.TabIndex = 51;
		this.grdDoc01History.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDoc01History).BackColor = System.Drawing.Color.White;
		grdDoc01History_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDoc01History.DesignTimeLayout = grdDoc01History_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDoc01History).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdDoc01History).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDoc01History.GroupByBoxVisible = false;
		this.grdDoc01History.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDoc01History).Location = new System.Drawing.Point(0, 141);
		((System.Windows.Forms.Control)(object)this.grdDoc01History).Name = "grdDoc01History";
		((ScrollableBase)this.grdDoc01History).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDoc01History).Size = new System.Drawing.Size(469, 207);
		((System.Windows.Forms.Control)(object)this.grdDoc01History).TabIndex = 0;
		this.grdDoc01History.TotalRow = (InheritableBoolean)1;
		this.grdDoc01History.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDoc01History).VisualStyle = (VisualStyle)4;
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(0, 121);
		this.panel18.Margin = new System.Windows.Forms.Padding(2);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(469, 20);
		this.panel18.TabIndex = 5;
		this.panel19.BackColor = System.Drawing.Color.White;
		this.panel19.Controls.Add(this.groupBox4);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel19.Location = new System.Drawing.Point(0, 0);
		this.panel19.Margin = new System.Windows.Forms.Padding(2);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(469, 121);
		this.panel19.TabIndex = 4;
		this.groupBox4.Controls.Add(this.chkContainerAccepted01History);
		this.groupBox4.Controls.Add(this.txtRemarks01History);
		this.groupBox4.Controls.Add(this.txtPicBy01History);
		this.groupBox4.Controls.Add(this.txtVerifiedBy01History);
		this.groupBox4.Controls.Add(this.txtInspectedBy01History);
		this.groupBox4.Controls.Add(this.txtContainer01History);
		this.groupBox4.Controls.Add(this.label22);
		this.groupBox4.Controls.Add(this.label23);
		this.groupBox4.Controls.Add(this.label24);
		this.groupBox4.Controls.Add(this.label25);
		this.groupBox4.Controls.Add(this.label26);
		this.groupBox4.Controls.Add(this.label27);
		this.groupBox4.Controls.Add(this.txtSeal01History);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.groupBox4.Location = new System.Drawing.Point(4, 3);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(462, 116);
		this.groupBox4.TabIndex = 1;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Container  01 Information";
		this.chkContainerAccepted01History.AutoSize = true;
		this.chkContainerAccepted01History.Enabled = false;
		this.chkContainerAccepted01History.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkContainerAccepted01History.Location = new System.Drawing.Point(250, 67);
		this.chkContainerAccepted01History.Name = "chkContainerAccepted01History";
		this.chkContainerAccepted01History.Size = new System.Drawing.Size(196, 19);
		this.chkContainerAccepted01History.TabIndex = 5;
		this.chkContainerAccepted01History.Text = "Container Accepted For Loading";
		this.chkContainerAccepted01History.UseVisualStyleBackColor = true;
		this.txtRemarks01History.Enabled = false;
		this.txtRemarks01History.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks01History.Location = new System.Drawing.Point(100, 90);
		this.txtRemarks01History.Margin = new System.Windows.Forms.Padding(2);
		this.txtRemarks01History.Name = "txtRemarks01History";
		this.txtRemarks01History.Size = new System.Drawing.Size(357, 23);
		this.txtRemarks01History.TabIndex = 4;
		this.txtPicBy01History.Enabled = false;
		this.txtPicBy01History.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPicBy01History.Location = new System.Drawing.Point(100, 65);
		this.txtPicBy01History.Margin = new System.Windows.Forms.Padding(2);
		this.txtPicBy01History.Name = "txtPicBy01History";
		this.txtPicBy01History.Size = new System.Drawing.Size(145, 23);
		this.txtPicBy01History.TabIndex = 4;
		this.txtVerifiedBy01History.Enabled = false;
		this.txtVerifiedBy01History.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVerifiedBy01History.Location = new System.Drawing.Point(310, 40);
		this.txtVerifiedBy01History.Margin = new System.Windows.Forms.Padding(2);
		this.txtVerifiedBy01History.Name = "txtVerifiedBy01History";
		this.txtVerifiedBy01History.Size = new System.Drawing.Size(147, 23);
		this.txtVerifiedBy01History.TabIndex = 3;
		this.txtInspectedBy01History.Enabled = false;
		this.txtInspectedBy01History.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInspectedBy01History.Location = new System.Drawing.Point(100, 40);
		this.txtInspectedBy01History.Margin = new System.Windows.Forms.Padding(2);
		this.txtInspectedBy01History.Name = "txtInspectedBy01History";
		this.txtInspectedBy01History.Size = new System.Drawing.Size(145, 23);
		this.txtInspectedBy01History.TabIndex = 2;
		this.txtContainer01History.Enabled = false;
		this.txtContainer01History.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtContainer01History.Location = new System.Drawing.Point(100, 15);
		this.txtContainer01History.Margin = new System.Windows.Forms.Padding(2);
		this.txtContainer01History.Name = "txtContainer01History";
		this.txtContainer01History.Size = new System.Drawing.Size(145, 23);
		this.txtContainer01History.TabIndex = 0;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.Black;
		this.label22.Location = new System.Drawing.Point(5, 94);
		this.label22.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(52, 15);
		this.label22.TabIndex = 16;
		this.label22.Text = "Remarks";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.Black;
		this.label23.Location = new System.Drawing.Point(5, 69);
		this.label23.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(94, 15);
		this.label23.TabIndex = 15;
		this.label23.Text = "Picture Taken By";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.Color.Black;
		this.label24.Location = new System.Drawing.Point(246, 19);
		this.label24.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(39, 15);
		this.label24.TabIndex = 10;
		this.label24.Text = "Seal #";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.ForeColor = System.Drawing.Color.Black;
		this.label25.Location = new System.Drawing.Point(247, 44);
		this.label25.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(63, 15);
		this.label25.TabIndex = 14;
		this.label25.Text = "Verified By";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ForeColor = System.Drawing.Color.Black;
		this.label26.Location = new System.Drawing.Point(5, 19);
		this.label26.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(68, 15);
		this.label26.TabIndex = 5;
		this.label26.Text = "Container #";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.ForeColor = System.Drawing.Color.Black;
		this.label27.Location = new System.Drawing.Point(5, 44);
		this.label27.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(75, 15);
		this.label27.TabIndex = 13;
		this.label27.Text = "Inspected By";
		this.txtSeal01History.Enabled = false;
		this.txtSeal01History.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSeal01History.Location = new System.Drawing.Point(310, 15);
		this.txtSeal01History.Margin = new System.Windows.Forms.Padding(2);
		this.txtSeal01History.Name = "txtSeal01History";
		this.txtSeal01History.Size = new System.Drawing.Size(147, 23);
		this.txtSeal01History.TabIndex = 1;
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.label92);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(3, 99);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(945, 21);
		this.panel15.TabIndex = 50;
		this.label92.AutoSize = true;
		this.label92.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label92.ForeColor = System.Drawing.Color.White;
		this.label92.Location = new System.Drawing.Point(2, 3);
		this.label92.Name = "label92";
		this.label92.Size = new System.Drawing.Size(119, 17);
		this.label92.TabIndex = 1;
		this.label92.Text = "Filtered Records";
		this.panel5.Controls.Add(this.groupBox2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 55);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(945, 44);
		this.panel5.TabIndex = 48;
		this.groupBox2.Controls.Add(this.ToDocNoHistory);
		this.groupBox2.Controls.Add(this.FromDocNoHistory);
		this.groupBox2.Controls.Add(this.FromDateHistory);
		this.groupBox2.Controls.Add(this.btnShowHistory);
		this.groupBox2.Controls.Add(this.label73);
		this.groupBox2.Controls.Add(this.label89);
		this.groupBox2.Controls.Add(this.label90);
		this.groupBox2.Controls.Add(this.label91);
		this.groupBox2.Controls.Add(this.ToDateHistory);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(5, 0);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(932, 44);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(653, 16);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(514, 16);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(85, 16);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(142, 23);
		this.FromDateHistory.TabIndex = 0;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(736, 15);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 25);
		this.btnShowHistory.TabIndex = 4;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.Location = new System.Drawing.Point(595, 20);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(57, 15);
		this.label73.TabIndex = 86;
		this.label73.Text = "To GP No";
		this.label89.AutoSize = true;
		this.label89.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label89.Location = new System.Drawing.Point(442, 20);
		this.label89.Name = "label89";
		this.label89.Size = new System.Drawing.Size(72, 15);
		this.label89.TabIndex = 85;
		this.label89.Text = "From GP No";
		this.label90.AutoSize = true;
		this.label90.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label90.Location = new System.Drawing.Point(230, 20);
		this.label90.Name = "label90";
		this.label90.Size = new System.Drawing.Size(66, 15);
		this.label90.TabIndex = 84;
		this.label90.Text = "To GP Date";
		this.label91.AutoSize = true;
		this.label91.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label91.Location = new System.Drawing.Point(5, 20);
		this.label91.Name = "label91";
		this.label91.Size = new System.Drawing.Size(81, 15);
		this.label91.TabIndex = 83;
		this.label91.Text = "From GP Date";
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(297, 16);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(142, 23);
		this.ToDateHistory.TabIndex = 1;
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.label69);
		this.panel16.Controls.Add(this.toolStrip3);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(3, 3);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(945, 52);
		this.panel16.TabIndex = 47;
		this.label69.AutoSize = true;
		this.label69.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label69.ForeColor = System.Drawing.Color.White;
		this.label69.Location = new System.Drawing.Point(2, 29);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(274, 19);
		this.label69.TabIndex = 1;
		this.label69.Text = "Vehicle Cargo Inspection History";
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnResetHistory });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(945, 27);
		this.toolStrip3.TabIndex = 52;
		this.toolStrip3.Text = "toolStrip2";
		this.btnResetHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnResetHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetHistory.Name = "btnResetHistory";
		this.btnResetHistory.Size = new System.Drawing.Size(59, 24);
		this.btnResetHistory.Text = "&New";
		this.btnResetHistory.Click += new System.EventHandler(btnResetHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(959, 661);
		base.Controls.Add(this.panel1);
		this.ForeColor = System.Drawing.Color.Black;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "GatePassInspection";
		this.Text = "Eccountbook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(GatePassInspection_FormClosed);
		base.Load += new System.EventHandler(GatePassInspection_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(GatePassInspection_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDoc02).EndInit();
		this.panel11.ResumeLayout(false);
		this.boxcontainer2.ResumeLayout(false);
		this.boxcontainer2.PerformLayout();
		this.panel12.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDoc01).EndInit();
		this.panel14.ResumeLayout(false);
		this.boxcontainer1.ResumeLayout(false);
		this.boxcontainer1.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbgatepass).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel20.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDoc02History).EndInit();
		this.panel13.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.panel17.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDoc01History).EndInit();
		this.panel19.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
