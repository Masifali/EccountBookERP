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
using Architecture.BLL.Lab;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.Model.Lab;
using Architecture.Reports;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Lab;

public class InvLabSaleAnalysis : Form
{
	public UserAccount UserAccount = new UserAccount();

	private Attachment AT = new Attachment();

	private Rightsobjects formrights = new Rightsobjects();

	private DataTable dtgpno = new DataTable();

	public DataTable dtViewAttachments = new DataTable();

	private int RecId;

	private int gpId;

	private bool UpdateMode;

	public static Image imgpreview;

	private bool Confiquration;

	private string saveDirectory = "";

	private string fileSavePath = "";

	private string ofd = "";

	private string fileSavePathcookingpic = "";

	private string ofdcookingpic = "";

	private DataTable grdtable = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private IContainer components;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton btnprint;

	private Button btnimg1;

	private PictureBox sampleanalysispicture;

	private ToolStripButton btnSave;

	private ToolStripButton btnUpdate;

	private TextBox txtremarks;

	private ToolStripButton btnNew;

	private ToolStrip btn;

	private ToolStripButton btnattachment;

	private TextBox txtdocno;

	private Label cmpcity;

	private Label label6;

	private Label label5;

	private Label cmbsupplier;

	private Label label10;

	private Label txtsampledate;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grd;

	private TabPage tabHistory;

	internal GridEX grdhistory;

	private Label txt;

	private Label label8;

	private TabControl tabControl1;

	private TabPage tabForm;

	private Panel panel1;

	private Panel panel7;

	private Label label7;

	private DateTimePicker txtdocdate;

	private Label label1;

	private CheckBox chkisaccepted;

	private TextBox txtSampleNo;

	private TextBox txtsupcode;

	private Label label3;

	private Label label2;

	private Button btnimage2;

	private PictureBox cookingpic;

	private Label label4;

	private TextBox txtvehicleno;

	private TextBox txtbiltyno;

	private Label label9;

	private TextBox txtSampleId;

	private TextBox txtGpId;

	private Button btnprv;

	private Button btnimgprevcookingpic;

	private Label label12;

	private TextBox txtcontainerno;

	internal UltraCombo cmbitem;

	internal UltraCombo cmbanalysisgroup;

	private ToolStripButton toolStripButton1;

	internal UltraCombo txtgatepassno;

	internal UltraCombo txtcrop;

	private ToolStripButton txtRefresh;

	private CtrlGrdBar ctrlGrdBarMain;

	private Panel panel3;

	private Panel panel2;

	private Label label11;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label13;

	private CheckBox chkprint;

	private TextBox txtAnalystName;

	private Label label14;

	private TextBox txtAnalyzedBags;

	private TextBox txtSaleOrderId;

	private Panel panel5;

	internal GridEX grdHistoryDetail;

	private Panel panel4;

	public InvLabSaleAnalysis(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void InvLabPurchaseAnalysis_Load(object sender, EventArgs e)
	{
		try
		{
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnSave.Enabled = formrights.DoHaveSaveRight;
			btnUpdate.Enabled = formrights.DoHaveUpdateRights;
			btnprint.Enabled = formrights.DoHavePrintRights;
			dtViewAttachments.Columns.Add("AttchmentPath");
			txtgatepassno.Focus();
			grdtable.Clear();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			ItemFill();
			GenerateCode();
			AnalysisGroup();
			historygridfill();
			GatepassNofill();
			CropYearFill();
			grdtable.Columns.Add("LabstandardanalysisId");
			grdtable.Columns.Add("AnalysisPerameter");
			grdtable.Columns.Add("Min");
			grdtable.Columns.Add("MaxValue");
			grdtable.Columns.Add("AnalysisResult");
			grdtable.Columns.Add("Remarks");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateCode()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			InvLabAnalysisSaleHeader code = new InvLabAnalysisSaleHeader();
			code.OrganizationId = UserAccount.OrganizationId;
			code.CompanyId = UserAccount.CompanyId;
			txtdocno.Text = InvLabAnalysisSaleHeader.GenerateCode(code).ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GatepassNofill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = GatePassOutward.GetGPNoForSaleLabAnalaysis(new GatePassOutward
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dtGp = new DataTable();
				dtGp.Columns.Add("Id");
				dtGp.Columns.Add("GpSrNo");
				dtGp.Columns.Add("VehicleNo");
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtGp.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["GpSrNo"], dt.Rows[i]["VehicleNo"]);
				}
				if (dtGp.Rows.Count > 0)
				{
					DDL.BindDDL(dtGp, txtgatepassno, "Id", "GpSrNo", "GpSrNo", ZeroIndex: false);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtgatepassno_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)txtgatepassno).ActiveRow != null)
			{
				ItemFill();
				dtgpno = CommonServices.GetByGpNo(91, Conversion.ToInt(txtgatepassno.Value));
				if (dtgpno.Rows.Count > 0)
				{
					txtGpId.Text = Conversion.ToInt(dtgpno.Rows[0]["Id"]).ToString();
					txtsupcode.Text = dtgpno.Rows[0]["SupplierContractCode"].ToString();
					txtSaleOrderId.Text = dtgpno.Rows[0]["SaleOrderId"].ToString();
					txtbiltyno.Text = dtgpno.Rows[0]["BiltyNo"].ToString();
					txtvehicleno.Text = dtgpno.Rows[0]["VehicleNo"].ToString();
					txtcontainerno.Text = dtgpno.Rows[0]["Container"].ToString();
				}
				else
				{
					txtGpId.Text = string.Empty;
					txtsupcode.Text = string.Empty;
					txtbiltyno.Text = string.Empty;
					txtvehicleno.Text = string.Empty;
				}
			}
			else
			{
				txtGpId.Text = string.Empty;
				txtsupcode.Text = string.Empty;
				txtbiltyno.Text = string.Empty;
				txtvehicleno.Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SampleNoId()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			InvLabSampleAnalysisHeader val = new InvLabSampleAnalysisHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocNo = Conversion.ToInt((object)txtSampleNo.Text)
			};
			DataTable dtgetIdbyDocNo = new DataTable();
			dtgetIdbyDocNo = InvLabSampleAnalysisHeader.GetIdFromDocNo(val);
			if (dtgetIdbyDocNo.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtgetIdbyDocNo.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtgetIdbyDocNo.Rows[i]["DocNo"]) == Conversion.ToInt((object)txtSampleNo.Text))
				{
					txtSampleId.Text = Conversion.ToInt(dtgetIdbyDocNo.Rows[0]["Id"]).ToString();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AnalysisGroup()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		try
		{
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtLAG = new DataTable();
			dtLAG = InvLabAnalysisGroup.GetAllOrById(val);
			DataTable dtLabAnalysisGroup = new DataTable();
			dtLabAnalysisGroup.Columns.Add("Id");
			dtLabAnalysisGroup.Columns.Add("AnalysisGroup");
			for (int i = 0; i < dtLAG.Rows.Count; i++)
			{
				dtLabAnalysisGroup.Rows.Add(dtLAG.Rows[i]["Id"], dtLAG.Rows[i]["AnalysisGroupDescription"]);
			}
			if (dtLabAnalysisGroup.Rows.Count > 0)
			{
				DDL.BindDDL(dtLabAnalysisGroup, cmbanalysisgroup, "Id", "AnalysisGroup", "Analysis Group", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbanalysisgroup_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			InvLabSampleAnalysisHeader val = new InvLabSampleAnalysisHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(cmbanalysisgroup.Value)
			};
			DataTable dtlabsampleAH = new DataTable();
			dtlabsampleAH = InvLabSampleAnalysisHeader.ReadByAnalysisGroupId(val);
			grdtable.Clear();
			if (dtlabsampleAH.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtlabsampleAH.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtlabsampleAH.Rows[i]["InvLabAnalysisGroup"]) == Conversion.ToInt(cmbanalysisgroup.Value))
				{
					grdtable.Rows.Add(dtlabsampleAH.Rows[i]["labgroupstandardId"], dtlabsampleAH.Rows[i]["AnalysisParameterDescription"], dtlabsampleAH.Rows[i]["MinValue"], dtlabsampleAH.Rows[i]["MaxValue"], string.Empty, string.Empty);
				}
			}
			grd.DataSource = grdtable;
			grd.RetrieveStructure();
			grdsetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemFill()
	{
		try
		{
			DataTable dt = Item.GetItemsFromDoAndSoByGpId(Conversion.ToInt(txtgatepassno.Value));
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbitem, "Id", "ItemName", "ItemName", ZeroIndex: false);
				return;
			}
			((Control)(object)cmbitem).Text = string.Empty;
			((UltraGridBase)cmbitem).DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CropYearFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable lst = InvCropYear.Getall(new InvCropYear
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (lst.Rows.Count > 0)
			{
				DDL.BindDDLNew(lst, txtcrop, "Id", "CropYear", "Crop Year", false);
				((UltraGridBase)txtcrop).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool formvalidation()
	{
		if (((Control)(object)txtgatepassno).Text.Trim() == "")
		{
			MessageBox.Show("Please Select GatePass No");
			txtgatepassno.Focus();
			return false;
		}
		if (txtAnalystName.Text.Trim() == "")
		{
			MessageBox.Show("Please Select Analyst Name");
			txtAnalystName.Focus();
			return false;
		}
		if (((Control)(object)cmbanalysisgroup).Text.Trim() == "")
		{
			MessageBox.Show("Please Select Analysis Group No");
			cmbanalysisgroup.Focus();
			return false;
		}
		return true;
	}

	private void New()
	{
		try
		{
			GenerateCode();
			UpdateMode = false;
			grdtable.Clear();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			((Control)(object)txtgatepassno).Text = "";
			txtbiltyno.Text = "";
			txtvehicleno.Text = "";
			txtsupcode.Text = "";
			txtSampleNo.Text = "";
			((Control)(object)cmbitem).Text = "";
			((Control)(object)cmbanalysisgroup).Text = "";
			txtAnalyzedBags.Text = "";
			txtcontainerno.Text = "";
			txtremarks.Text = "";
			txtAnalystName.Text = "";
			cookingpic.Image = null;
			cookingpic.Invalidate();
			sampleanalysispicture.Image = null;
			sampleanalysispicture.Invalidate();
			((Control)(object)cmbitem).Enabled = true;
			ItemFill();
			CropYearFill();
			((Control)(object)cmbanalysisgroup).Enabled = true;
			((Control)(object)txtgatepassno).Enabled = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Refresh1()
	{
		AnalysisGroup();
		historygridfill();
		GatepassNofill();
		CropYearFill();
		txtgatepassno_Leave(null, null);
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		New();
	}

	private void txtRefresh_Click(object sender, EventArgs e)
	{
		Refresh1();
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		RecId = 0;
		Insert();
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		grd.UpdateData();
	}

	private void Insert()
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Expected O, but got Unknown
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Expected O, but got Unknown
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Expected O, but got Unknown
		try
		{
			if (!formvalidation())
			{
				return;
			}
			if (RecId > 0)
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
			if (grd.RowCount > 0)
			{
				InvLabAnalysisSaleHeader labheader = new InvLabAnalysisSaleHeader();
				labheader.Id = RecId;
				labheader.DocumentTypeId = 304;
				labheader.DocNo = Conversion.ToInt((object)txtdocno.Text);
				labheader.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
				labheader.GatePassOutwardId = Conversion.ToInt((object)txtGpId.Text);
				labheader.SupCustCode = Conversion.ToString((object)txtsupcode.Text);
				labheader.SaleOrderId = Conversion.ToInt((object)txtSaleOrderId.Text);
				labheader.InvLabSampleLogRegisterId = Conversion.ToInt((object)txtSampleId.Text);
				labheader.IsAccepted = Conversion.ToBool((object)chkisaccepted.Checked);
				labheader.ItemId = Conversion.ToInt(cmbitem.Value);
				labheader.Crop = Conversion.ToString((object)((Control)(object)txtcrop).Text);
				labheader.InvLabAnalysisGroup = Conversion.ToInt(cmbanalysisgroup.Value);
				labheader.RemarksHeader = Conversion.ToString((object)txtremarks.Text);
				labheader.analystName = Conversion.ToString((object)txtAnalystName.Text);
				labheader.AnalyzedBags = Conversion.ToDouble((object)txtAnalyzedBags.Text);
				labheader.AnalysisPic = Conversion.ToString((object)fileSavePath);
				labheader.CookingPic = Conversion.ToString((object)fileSavePathcookingpic);
				labheader.ContainerNo1 = Conversion.ToString((object)txtcontainerno.Text);
				labheader.OrganizationId = UserAccount.OrganizationId;
				labheader.CompanyId = UserAccount.CompanyId;
				labheader.ModifyUser = UserAccount.ID;
				labheader.LabSampleDetail = new List<InvLabAnalysisSaleDetail>();
				labheader.dmsAttachment = new List<DMSAttachments>();
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					InvLabAnalysisSaleDetail labdetail = new InvLabAnalysisSaleDetail();
					labdetail.InvLabGroupAnalysisStandardsId = Conversion.ToInt(r.Cells["LabstandardanalysisId"].Value);
					labdetail.InAnalysisResult = Conversion.ToDouble(r.Cells["AnalysisResult"].Value);
					labdetail.RemarksDetail = Conversion.ToString((object)r.Cells["Remarks"].Text);
					labheader.LabSampleDetail.Add(labdetail);
				}
				DMSAttachments dms = new DMSAttachments();
				dms.RefDocumentNo = labheader.Id;
				dms.ScreenName = base.Name;
				DMSAttachments.RemoveById(dms.RefDocumentNo, dms.ScreenName);
				for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
				{
					DMSAttachments attach = new DMSAttachments();
					attach.Attachment = Attachment.dtAttachments.Rows[i][1].ToString();
					attach.ScreenName = base.Name;
					labheader.dmsAttachment.Add(attach);
				}
				int success = InvLabAnalysisSaleHeader.Save(labheader);
				if (success > 0)
				{
					MessageBox.Show("Record Update Successfully [" + success + "] ");
				}
				else
				{
					MessageBox.Show("Record Saved Successfully [" + success + "] ");
				}
			}
			else
			{
				MessageBox.Show("Grid Record Not Found");
			}
			if (ofd != "" && ofd != string.Empty)
			{
				File.Copy(ofd, fileSavePath, overwrite: true);
				sampleanalysispicture.Image = null;
				sampleanalysispicture.Invalidate();
				ofd = "";
			}
			if (ofdcookingpic != "" && ofdcookingpic != string.Empty)
			{
				File.Copy(ofdcookingpic, fileSavePathcookingpic, overwrite: true);
				cookingpic.Image = null;
				cookingpic.Invalidate();
				ofdcookingpic = "";
			}
			if (chkprint.Checked)
			{
				GenerateReport(RecId);
			}
			New();
			txtgatepassno.Focus();
			historygridfill();
			UpdateMode = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		if (RecId == 0)
		{
			throw new Exception("Record Not Update beacause Record Id not found");
		}
		Insert();
	}

	private void grdhistory_DoubleClick(object sender, EventArgs e)
	{
		ReadById(Conversion.ToInt((object)grdhistory.CurrentRow.Cells[0].Value.ToString()));
	}

	private void ReadById(int Id)
	{
		try
		{
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			new List<InvLabAnalysisSaleHeader>();
			RecId = Id;
			DataTable dtgetbyId = new DataTable();
			dtgetbyId = InvLabAnalysisSaleHeader.GetById(RecId);
			tabControl1.SelectedIndex = 0;
			if (dtgetbyId.Rows.Count <= 0)
			{
				return;
			}
			txtdocno.Text = Conversion.ToString(dtgetbyId.Rows[0]["SAHDocNo"]);
			txtbiltyno.Text = Conversion.ToString(dtgetbyId.Rows[0]["BiltyNo"]);
			txtvehicleno.Text = Conversion.ToString(dtgetbyId.Rows[0]["VehicleNo"]);
			((Control)(object)txtgatepassno).Text = Conversion.ToString(dtgetbyId.Rows[0]["GpSrNo"]);
			txtGpId.Text = Conversion.ToString(dtgetbyId.Rows[0]["GatePassOutwardId"]);
			txtsupcode.Text = Conversion.ToString(dtgetbyId.Rows[0]["SAHSupCustCode"]);
			txtSaleOrderId.Text = Conversion.ToString(dtgetbyId.Rows[0]["SaleOrderId"]);
			txtSampleNo.Text = Conversion.ToString(dtgetbyId.Rows[0]["SampleAnalysisNo"]);
			txtSampleId.Text = Conversion.ToString(dtgetbyId.Rows[0]["SAHSampleLogRegisterId"]);
			chkisaccepted.Checked = Conversion.ToBool(dtgetbyId.Rows[0]["SAHIsAccepted"]);
			cmbitem.Value = Conversion.ToInt(dtgetbyId.Rows[0]["SAHItmId"]);
			((Control)(object)cmbitem).Text = Conversion.ToString(dtgetbyId.Rows[0]["ItemName"]);
			((Control)(object)txtcrop).Text = Conversion.ToString(dtgetbyId.Rows[0]["SAHCropYear"]);
			txtcontainerno.Text = Conversion.ToString(dtgetbyId.Rows[0]["ContainerNo1"]);
			((Control)(object)cmbanalysisgroup).Text = Conversion.ToString(dtgetbyId.Rows[0]["AnalysisGroupDescription"]);
			cmbanalysisgroup.Value = Conversion.ToInt(dtgetbyId.Rows[0]["SAHAnalysisGroup"]);
			txtremarks.Text = Conversion.ToString(dtgetbyId.Rows[0]["RemarksHeader"]);
			txtGpId.Text = Conversion.ToString(dtgetbyId.Rows[0]["GatePassOutwardId"]);
			txtAnalystName.Text = Conversion.ToString(dtgetbyId.Rows[0]["AnaLystName"]);
			txtAnalyzedBags.Text = Conversion.ToString(dtgetbyId.Rows[0]["AnalyzedBags"]);
			((Control)(object)txtgatepassno).Enabled = false;
			fileSavePath = dtgetbyId.Rows[0]["SAHAnalysisPic"].ToString();
			fileSavePathcookingpic = dtgetbyId.Rows[0]["SAHCookingPic"].ToString();
			if (fileSavePath != "")
			{
				if (File.Exists(fileSavePath))
				{
					sampleanalysispicture.Image = Image.FromFile(dtgetbyId.Rows[0]["SAHAnalysisPic"].ToString());
				}
				else
				{
					fileSavePath = "";
					sampleanalysispicture.Image = null;
					sampleanalysispicture.Invalidate();
				}
			}
			else
			{
				fileSavePath = "";
				sampleanalysispicture.Image = null;
				sampleanalysispicture.Invalidate();
			}
			if (fileSavePathcookingpic != "")
			{
				if (File.Exists(fileSavePathcookingpic))
				{
					cookingpic.Image = Image.FromFile(dtgetbyId.Rows[0]["SAHCookingPic"].ToString());
				}
				else
				{
					fileSavePathcookingpic = "";
					cookingpic.Image = null;
					cookingpic.Invalidate();
				}
			}
			else
			{
				fileSavePathcookingpic = "";
				cookingpic.Image = null;
				cookingpic.Invalidate();
			}
			grdtable.Clear();
			for (int i = 0; i < dtgetbyId.Rows.Count; i++)
			{
				grdtable.Rows.Add(dtgetbyId.Rows[i]["InvLabGroupAnalysisStandardsId"], dtgetbyId.Rows[i]["AnalysisParameterDescription"], dtgetbyId.Rows[i]["MinValue"], dtgetbyId.Rows[i]["MaxValue"], dtgetbyId.Rows[i]["InAnalysisResult"], dtgetbyId.Rows[i]["RemarksDetail"]);
			}
			grd.DataSource = grdtable;
			grd.RetrieveStructure();
			grdsetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
			throw;
		}
	}

	private void grdhistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				RecId = Conversion.ToInt((object)grdhistory.CurrentRow.Cells[0].Value.ToString());
				ReadById(RecId);
			}
			if (e.Column.Key == "Print")
			{
				GenerateReport(Conversion.ToInt((object)grdhistory.CurrentRow.Cells[0].Value.ToString()));
			}
			if (e.Column.Key == "Detail")
			{
				int num = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString());
				DataTable dtHistoryDetail = new DataTable();
				dtHistoryDetail = InvLabAnalysisSaleHeader.GetById(num);
				grdtable.Clear();
				for (int i = 0; i < dtHistoryDetail.Rows.Count; i++)
				{
					grdtable.Rows.Add(dtHistoryDetail.Rows[i]["InvLabGroupAnalysisStandardsId"], dtHistoryDetail.Rows[i]["AnalysisParameterDescription"], dtHistoryDetail.Rows[i]["MinValue"], dtHistoryDetail.Rows[i]["MaxValue"], dtHistoryDetail.Rows[i]["InAnalysisResult"], dtHistoryDetail.Rows[i]["RemarksDetail"]);
				}
				grdHistoryDetail.DataSource = grdtable;
				grdHistoryDetail.RetrieveStructure();
				grdHistoryDetail.RootTable.Columns["LabstandardanalysisId"].Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdsetting()
	{
		try
		{
			grd.RootTable.Columns["LabstandardanalysisId"].EditType = (EditType)0;
			grd.RootTable.Columns["AnalysisPerameter"].EditType = (EditType)0;
			grd.RootTable.Columns["Min"].EditType = (EditType)0;
			grd.RootTable.Columns["MaxValue"].EditType = (EditType)0;
			grd.RootTable.Columns["AnalysisPerameter"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["Min"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["MaxValue"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["LabstandardanalysisId"].Visible = false;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void historygridfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		try
		{
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				NoOfRecords = 50,
				CanViewAllRecord = formrights.DoHaveCanViewAllRecordRights
			};
			DataTable dtgetall = new DataTable();
			dtgetall = InvLabAnalysisSaleHeader.GetAll(val);
			DataTable table = new DataTable();
			table.Columns.Add("Id");
			table.Columns.Add("DocNo");
			table.Columns.Add("AnalysisItem");
			table.Columns.Add("AnlysisGroup");
			table.Columns.Add("GatePass#");
			table.Columns.Add("CustomerCode");
			table.Columns.Add("BiltyNo");
			table.Columns.Add("VehicleNo");
			table.Columns.Add("CropYear");
			table.Columns.Add("EntryUser");
			table.Columns.Add("Remarks");
			table.Clear();
			grdhistory.DataSource = table;
			for (int i = 0; i < dtgetall.Rows.Count; i++)
			{
				table.Rows.Add(dtgetall.Rows[i]["Id"], dtgetall.Rows[i]["DocNo"], dtgetall.Rows[i]["ItemName"], dtgetall.Rows[i]["AnalysisGroupDescription"], dtgetall.Rows[i]["GpSrNo"], dtgetall.Rows[i]["SupCustCode"], dtgetall.Rows[i]["BiltyNo"], dtgetall.Rows[i]["VehicleNo"], dtgetall.Rows[i]["Crop"], dtgetall.Rows[i]["UserNameEusr"], dtgetall.Rows[i]["RemarksHeader"]);
			}
			grdhistory.DataSource = table;
			grdhistory.RetrieveStructure();
			grdhistorysetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void grdhistorysetting()
	{
		try
		{
			grdhistory.RootTable.Columns["DocNo"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["AnalysisItem"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["AnlysisGroup"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["GatePass#"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["CustomerCode"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["BiltyNo"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["VehicleNo"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["CropYear"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["EntryUser"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["Remarks"].EditType = (EditType)0;
			grdhistory.RootTable.Columns["DocNo"].FilterEditType = (FilterEditType)1;
			grdhistory.RootTable.Columns["AnalysisItem"].FilterEditType = (FilterEditType)1;
			grdhistory.RootTable.Columns["AnlysisGroup"].FilterEditType = (FilterEditType)1;
			grdhistory.RootTable.Columns["GatePass#"].FilterEditType = (FilterEditType)1;
			grdhistory.RootTable.Columns["CustomerCode"].FilterEditType = (FilterEditType)1;
			grdhistory.RootTable.Columns["BiltyNo"].FilterEditType = (FilterEditType)1;
			grdhistory.RootTable.Columns["VehicleNo"].FilterEditType = (FilterEditType)1;
			grdhistory.RootTable.Columns["CropYear"].FilterEditType = (FilterEditType)1;
			grdhistory.RootTable.Columns["EntryUser"].FilterEditType = (FilterEditType)1;
			grdhistory.RootTable.Columns["Remarks"].FilterEditType = (FilterEditType)1;
			grdhistory.RootTable.Columns["DocNo"].Width = 60;
			grdhistory.RootTable.Columns["AnalysisItem"].Width = 250;
			grdhistory.RootTable.Columns["AnlysisGroup"].Width = 250;
			grdhistory.RootTable.Columns["GatePass#"].Width = 80;
			grdhistory.RootTable.Columns["CustomerCode"].Width = 130;
			grdhistory.RootTable.Columns["BiltyNo"].Width = 70;
			grdhistory.RootTable.Columns["VehicleNo"].Width = 100;
			grdhistory.RootTable.Columns["CropYear"].Width = 120;
			grdhistory.RootTable.Columns["EntryUser"].Width = 150;
			grdhistory.RootTable.Columns["Remarks"].Width = 300;
			grdhistory.RootTable.Columns["Id"].Visible = false;
			grdhistory.RootTable.Columns.Add("Edit");
			grdhistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdhistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdhistory.RootTable.Columns["Edit"].Width = 50;
			grdhistory.RootTable.Columns.Add("Print");
			grdhistory.RootTable.Columns["Print"].Caption = "Print";
			grdhistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Print"].ButtonText = "Print";
			grdhistory.RootTable.Columns["Print"].Width = 50;
			grdhistory.RootTable.Columns.Add("Detail");
			grdhistory.RootTable.Columns["Detail"].Caption = "Detail";
			grdhistory.RootTable.Columns["Detail"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Detail"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Detail"].ButtonText = "Detail";
			grdhistory.RootTable.Columns["Detail"].Width = 60;
			grdhistory.RootTable.Columns["Detail"].Position = 0;
			grdhistory.RootTable.Columns["Print"].Position = 1;
			grdhistory.RootTable.Columns["Edit"].Position = 2;
			ctrlGrdBarMain_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvLabPurchaseAnalysis_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Return)
		{
			SendKeys.Send("{TAB}");
		}
		if (e.KeyCode == Keys.Delete && tabControl1.SelectedIndex == 1)
		{
			GridEXRow[] checkedRows = grd.GetCheckedRows();
			for (int i = 0; i < checkedRows.Length; i++)
			{
				int.Parse(checkedRows[i].Cells["Id"].Value.ToString());
			}
		}
		if (btnSave.Visible && e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0)
		{
			btnSave_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.N && tabControl1.SelectedIndex == 0)
		{
			btnNew_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.T)
		{
			if (tabControl1.SelectedIndex == 1)
			{
				tabControl1.SelectedIndex = 0;
			}
			else
			{
				tabControl1.SelectedIndex = 1;
			}
		}
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
		if (btnUpdate.Visible && e.Control && e.KeyCode == Keys.U && (UpdateMode = true))
		{
			btnUpdate_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.P)
		{
			btnprint_Click(null, null);
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

	private void btnimg1_Click(object sender, EventArgs e)
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
					sampleanalysispicture.Image = new Bitmap(openFileDialog.FileName);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnimage2_Click(object sender, EventArgs e)
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
					ofdcookingpic = openFileDialog.FileName;
					string fileName = Path.GetFileNameWithoutExtension(ofdcookingpic);
					string exten = Path.GetExtension(ofdcookingpic);
					string date = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
					fileName = fileName + date + exten;
					fileSavePathcookingpic = Path.Combine(saveDirectory, fileName);
					cookingpic.Image = new Bitmap(openFileDialog.FileName);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnprint_Click(object sender, EventArgs e)
	{
		try
		{
			GenerateReport(RecId);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		historygridfill();
	}

	private void btnprv_Click(object sender, EventArgs e)
	{
		LabImagesPreview labImagesPreview = new LabImagesPreview();
		imgpreview = sampleanalysispicture.Image;
		labImagesPreview.Show();
	}

	private void btnimgprevcookingpic_Click(object sender, EventArgs e)
	{
		LabImagesPreview labImagesPreview = new LabImagesPreview();
		imgpreview = cookingpic.Image;
		labImagesPreview.Show();
	}

	private void btnattachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void GenerateReport(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InvLabAnalysisSaleHeader.SaleAnalysisPrint(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Id
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "658-RptInvLabSaleAnalysisSlip.rpt");
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

	private void ctrlGrdBarMain_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdhistory).Name);
			if (fs != null)
			{
				grdhistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void InvLabSaleAnalysis_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Expected O, but got Unknown
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Expected O, but got Unknown
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Expected O, but got Unknown
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Expected O, but got Unknown
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Expected O, but got Unknown
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Expected O, but got Unknown
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Expected O, but got Unknown
		//IL_0e6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e79: Expected O, but got Unknown
		//IL_1282: Unknown result type (might be due to invalid IL or missing references)
		//IL_128c: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Lab.InvLabSaleAnalysis));
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
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
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnimg1 = new System.Windows.Forms.Button();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.btn = new System.Windows.Forms.ToolStrip();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.txtRefresh = new System.Windows.Forms.ToolStripButton();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.cmpcity = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.cmbsupplier = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.txtsampledate = new System.Windows.Forms.Label();
		this.grd = new GridEX();
		this.tabHistory = new System.Windows.Forms.TabPage();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdHistoryDetail = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdhistory = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label11 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.txt = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabForm = new System.Windows.Forms.TabPage();
		this.ctrlGrdBarMain = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.txtSaleOrderId = new System.Windows.Forms.TextBox();
		this.txtAnalyzedBags = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.txtAnalystName = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.chkprint = new System.Windows.Forms.CheckBox();
		this.txtgatepassno = new UltraCombo();
		this.txtcrop = new UltraCombo();
		this.cmbitem = new UltraCombo();
		this.cmbanalysisgroup = new UltraCombo();
		this.btnimgprevcookingpic = new System.Windows.Forms.Button();
		this.btnprv = new System.Windows.Forms.Button();
		this.txtSampleId = new System.Windows.Forms.TextBox();
		this.txtGpId = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtcontainerno = new System.Windows.Forms.TextBox();
		this.txtvehicleno = new System.Windows.Forms.TextBox();
		this.txtbiltyno = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.chkisaccepted = new System.Windows.Forms.CheckBox();
		this.btnimage2 = new System.Windows.Forms.Button();
		this.cookingpic = new System.Windows.Forms.PictureBox();
		this.sampleanalysispicture = new System.Windows.Forms.PictureBox();
		this.txtSampleNo = new System.Windows.Forms.TextBox();
		this.txtsupcode = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.txtdocdate = new System.Windows.Forms.DateTimePicker();
		this.label1 = new System.Windows.Forms.Label();
		this.btn.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.tabHistory.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel2.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabForm.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.txtgatepassno).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.txtcrop).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbanalysisgroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cookingpic).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.sampleanalysispicture).BeginInit();
		base.SuspendLayout();
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.btnimg1.Font = new System.Drawing.Font("Segoe UI", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnimg1.Location = new System.Drawing.Point(703, 153);
		this.btnimg1.Name = "btnimg1";
		this.btnimg1.Size = new System.Drawing.Size(64, 27);
		this.btnimg1.TabIndex = 14;
		this.btnimg1.Text = "Browse";
		this.btnimg1.UseVisualStyleBackColor = true;
		this.btnimg1.Click += new System.EventHandler(btnimg1_Click);
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtremarks.Location = new System.Drawing.Point(116, 155);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(455, 25);
		this.txtremarks.TabIndex = 11;
		this.btn.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.btn.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.toolStripButton1, this.btnNew, this.btnUpdate, this.btnSave, this.btnprint, this.btnattachment, this.toolStripSeparator, this.txtRefresh });
		this.btn.Location = new System.Drawing.Point(3, 3);
		this.btn.Name = "btn";
		this.btn.Size = new System.Drawing.Size(1213, 27);
		this.btn.TabIndex = 0;
		this.btn.Text = "toolStrip1";
		this.toolStripButton1.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(128, 24);
		this.toolStripButton1.Text = "Sale Analysis       ";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(69, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(55, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnprint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(79, 24);
		this.btnprint.Text = "658-&Print";
		this.btnprint.Click += new System.EventHandler(btnprint_Click);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(94, 24);
		this.btnattachment.Text = "&Attechment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.txtRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.txtRefresh.Image = (System.Drawing.Image)resources.GetObject("txtRefresh.Image");
		this.txtRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.txtRefresh.Name = "txtRefresh";
		this.txtRefresh.Size = new System.Drawing.Size(50, 24);
		this.txtRefresh.Text = "Refresh";
		this.txtRefresh.Click += new System.EventHandler(txtRefresh_Click);
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Enabled = false;
		this.txtdocno.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtdocno.Location = new System.Drawing.Point(116, 13);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.Size = new System.Drawing.Size(220, 25);
		this.txtdocno.TabIndex = 0;
		this.cmpcity.AutoSize = true;
		this.cmpcity.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmpcity.Location = new System.Drawing.Point(354, 44);
		this.cmpcity.Name = "cmpcity";
		this.cmpcity.Size = new System.Drawing.Size(99, 17);
		this.cmpcity.TabIndex = 0;
		this.cmpcity.Text = "Customer Code";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label6.Location = new System.Drawing.Point(577, 156);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(75, 17);
		this.label6.TabIndex = 12;
		this.label6.Text = "Is Accepted";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label5.Location = new System.Drawing.Point(354, 126);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(92, 17);
		this.label5.TabIndex = 0;
		this.label5.Text = "Analyst  Name";
		this.cmbsupplier.AutoSize = true;
		this.cmbsupplier.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbsupplier.Location = new System.Drawing.Point(17, 44);
		this.cmbsupplier.Name = "cmbsupplier";
		this.cmbsupplier.Size = new System.Drawing.Size(77, 17);
		this.cmbsupplier.TabIndex = 0;
		this.cmbsupplier.Text = "Gate Pass #";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label10.Location = new System.Drawing.Point(354, 102);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(95, 17);
		this.label10.TabIndex = 0;
		this.label10.Text = "Analysis Group";
		this.txtsampledate.AutoSize = true;
		this.txtsampledate.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtsampledate.Location = new System.Drawing.Point(354, 17);
		this.txtsampledate.Name = "txtsampledate";
		this.txtsampledate.Size = new System.Drawing.Size(62, 17);
		this.txtsampledate.TabIndex = 0;
		this.txtsampledate.Text = "Doc Date";
		this.grd.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1213, 379);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.tabHistory.Controls.Add(this.panel3);
		this.tabHistory.Controls.Add(this.panel2);
		this.tabHistory.Location = new System.Drawing.Point(4, 4);
		this.tabHistory.Name = "tabHistory";
		this.tabHistory.Padding = new System.Windows.Forms.Padding(3);
		this.tabHistory.Size = new System.Drawing.Size(1219, 618);
		this.tabHistory.TabIndex = 1;
		this.tabHistory.Text = "History";
		this.tabHistory.UseVisualStyleBackColor = true;
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Controls.Add(this.panel4);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(3, 35);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1213, 580);
		this.panel3.TabIndex = 3;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryDetail);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 363);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1213, 217);
		this.panel5.TabIndex = 4;
		this.grdHistoryDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdHistoryDetail).BackColor = System.Drawing.Color.White;
		grdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryDetail.DesignTimeLayout = grdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryDetail.DynamicFiltering = true;
		this.grdHistoryDetail.FilterMode = (FilterMode)2;
		this.grdHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Name = "grdHistoryDetail";
		((ScrollableBase)this.grdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Size = new System.Drawing.Size(1213, 217);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).TabIndex = 3;
		this.grdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.grdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1213, 363);
		this.panel4.TabIndex = 3;
		((ScrollableBase)this.grdhistory).BackColor = System.Drawing.Color.White;
		grdhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistory.DesignTimeLayout = grdhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistory.DynamicFiltering = true;
		this.grdhistory.FilterMode = (FilterMode)2;
		this.grdhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistory.FrozenColumns = 3;
		this.grdhistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1213, 363);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 2;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label11);
		this.panel2.Controls.Add(this.ctrlGrdBar2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1213, 32);
		this.panel2.TabIndex = 2;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Calibri", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.White;
		this.label11.Location = new System.Drawing.Point(5, 6);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(224, 26);
		this.label11.TabIndex = 41;
		this.label11.Text = "Sale Lab Analysis History";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1162, 6);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdhistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar2.TabIndex = 40;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.txt.AutoSize = true;
		this.txt.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txt.Location = new System.Drawing.Point(17, 71);
		this.txt.Name = "txt";
		this.txt.Size = new System.Drawing.Size(53, 17);
		this.txt.TabIndex = 0;
		this.txt.Text = "Bilty No";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label8.Location = new System.Drawing.Point(354, 71);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(72, 17);
		this.label8.TabIndex = 0;
		this.label8.Text = "Item Name";
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabForm);
		this.tabControl1.Controls.Add(this.tabHistory);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1227, 644);
		this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabForm.BackColor = System.Drawing.Color.White;
		this.tabForm.Controls.Add(this.ctrlGrdBarMain);
		this.tabForm.Controls.Add(this.ctrlGrdBar1);
		this.tabForm.Controls.Add(this.panel1);
		this.tabForm.Controls.Add(this.panel7);
		this.tabForm.Controls.Add(this.btn);
		this.tabForm.Location = new System.Drawing.Point(4, 4);
		this.tabForm.Name = "tabForm";
		this.tabForm.Padding = new System.Windows.Forms.Padding(3);
		this.tabForm.Size = new System.Drawing.Size(1219, 618);
		this.tabForm.TabIndex = 0;
		this.tabForm.Text = "Form";
		this.ctrlGrdBarMain.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBarMain.FormName = this;
		this.ctrlGrdBarMain.Location = new System.Drawing.Point(1167, 4);
		this.ctrlGrdBarMain.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBarMain.MyGrid = this.grd;
		this.ctrlGrdBarMain.Name = "ctrlGrdBarMain";
		this.ctrlGrdBarMain.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBarMain.TabIndex = 39;
		this.ctrlGrdBarMain.Load += new System.EventHandler(ctrlGrdBarMain_Load);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = null;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1268, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 26);
		this.ctrlGrdBar1.TabIndex = 35;
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(3, 236);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1213, 379);
		this.panel1.TabIndex = 38;
		this.panel7.Controls.Add(this.txtSaleOrderId);
		this.panel7.Controls.Add(this.txtAnalyzedBags);
		this.panel7.Controls.Add(this.label14);
		this.panel7.Controls.Add(this.txtAnalystName);
		this.panel7.Controls.Add(this.label13);
		this.panel7.Controls.Add(this.chkprint);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.txtgatepassno);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.txtcrop);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbitem);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbanalysisgroup);
		this.panel7.Controls.Add(this.btnimgprevcookingpic);
		this.panel7.Controls.Add(this.btnprv);
		this.panel7.Controls.Add(this.txtSampleId);
		this.panel7.Controls.Add(this.txtGpId);
		this.panel7.Controls.Add(this.label9);
		this.panel7.Controls.Add(this.label12);
		this.panel7.Controls.Add(this.label4);
		this.panel7.Controls.Add(this.txtcontainerno);
		this.panel7.Controls.Add(this.txtvehicleno);
		this.panel7.Controls.Add(this.txtbiltyno);
		this.panel7.Controls.Add(this.label3);
		this.panel7.Controls.Add(this.label2);
		this.panel7.Controls.Add(this.chkisaccepted);
		this.panel7.Controls.Add(this.btnimage2);
		this.panel7.Controls.Add(this.btnimg1);
		this.panel7.Controls.Add(this.cookingpic);
		this.panel7.Controls.Add(this.sampleanalysispicture);
		this.panel7.Controls.Add(this.txtSampleNo);
		this.panel7.Controls.Add(this.txtsupcode);
		this.panel7.Controls.Add(this.txtremarks);
		this.panel7.Controls.Add(this.txtdocno);
		this.panel7.Controls.Add(this.cmpcity);
		this.panel7.Controls.Add(this.label6);
		this.panel7.Controls.Add(this.label5);
		this.panel7.Controls.Add(this.txt);
		this.panel7.Controls.Add(this.cmbsupplier);
		this.panel7.Controls.Add(this.label10);
		this.panel7.Controls.Add(this.txtsampledate);
		this.panel7.Controls.Add(this.label8);
		this.panel7.Controls.Add(this.label7);
		this.panel7.Controls.Add(this.txtdocdate);
		this.panel7.Controls.Add(this.label1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.panel7.Location = new System.Drawing.Point(3, 30);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1213, 206);
		this.panel7.TabIndex = 0;
		this.txtSaleOrderId.Location = new System.Drawing.Point(1133, 71);
		this.txtSaleOrderId.Margin = new System.Windows.Forms.Padding(2);
		this.txtSaleOrderId.Name = "txtSaleOrderId";
		this.txtSaleOrderId.Size = new System.Drawing.Size(26, 25);
		this.txtSaleOrderId.TabIndex = 45;
		this.txtSaleOrderId.Visible = false;
		this.txtAnalyzedBags.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAnalyzedBags.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtAnalyzedBags.Location = new System.Drawing.Point(275, 129);
		this.txtAnalyzedBags.Name = "txtAnalyzedBags";
		this.txtAnalyzedBags.Size = new System.Drawing.Size(61, 25);
		this.txtAnalyzedBags.TabIndex = 44;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label14.Location = new System.Drawing.Point(183, 131);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(92, 17);
		this.label14.TabIndex = 43;
		this.label14.Text = "Analyzed Bags";
		this.txtAnalystName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAnalystName.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtAnalystName.Location = new System.Drawing.Point(462, 126);
		this.txtAnalystName.Name = "txtAnalystName";
		this.txtAnalystName.Size = new System.Drawing.Size(220, 25);
		this.txtAnalystName.TabIndex = 41;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label13.Location = new System.Drawing.Point(577, 176);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(52, 17);
		this.label13.TabIndex = 40;
		this.label13.Text = "Preview";
		this.chkprint.AutoSize = true;
		this.chkprint.Checked = true;
		this.chkprint.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkprint.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.chkprint.ForeColor = System.Drawing.Color.White;
		this.chkprint.Location = new System.Drawing.Point(667, 177);
		this.chkprint.Name = "chkprint";
		this.chkprint.Size = new System.Drawing.Size(15, 14);
		this.chkprint.TabIndex = 39;
		this.chkprint.UseVisualStyleBackColor = true;
		this.txtgatepassno.AllowNull = (DefaultableBoolean)1;
		this.txtgatepassno.AutoCompleteMode = (AutoCompleteMode)4;
		this.txtgatepassno.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.txtgatepassno.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.txtgatepassno).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.txtgatepassno).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.txtgatepassno).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.txtgatepassno).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.txtgatepassno).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.txtgatepassno).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.txtgatepassno).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.txtgatepassno).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.txtgatepassno).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.txtgatepassno).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.txtgatepassno).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.txtgatepassno).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.txtgatepassno).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.txtgatepassno).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.txtgatepassno).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.txtgatepassno).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.txtgatepassno).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.txtgatepassno).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.txtgatepassno).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.txtgatepassno).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.txtgatepassno).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.txtgatepassno).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.txtgatepassno).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.txtgatepassno).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.txtgatepassno).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.txtgatepassno).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.txtgatepassno).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.txtgatepassno).Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtgatepassno.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.txtgatepassno).Location = new System.Drawing.Point(116, 40);
		((System.Windows.Forms.Control)(object)this.txtgatepassno).Name = "txtgatepassno";
		((System.Windows.Forms.Control)(object)this.txtgatepassno).Size = new System.Drawing.Size(220, 25);
		((System.Windows.Forms.Control)(object)this.txtgatepassno).TabIndex = 37;
		((System.Windows.Forms.Control)(object)this.txtgatepassno).Leave += new System.EventHandler(txtgatepassno_Leave);
		this.txtcrop.AllowNull = (DefaultableBoolean)1;
		this.txtcrop.AutoCompleteMode = (AutoCompleteMode)4;
		this.txtcrop.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.txtcrop.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.txtcrop).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.txtcrop).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.txtcrop).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.txtcrop).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.txtcrop).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.txtcrop).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.txtcrop).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.txtcrop).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.txtcrop).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.txtcrop).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.txtcrop).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.txtcrop).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.txtcrop).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.txtcrop).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.txtcrop).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.txtcrop).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.txtcrop).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.txtcrop).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.txtcrop).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.txtcrop).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.txtcrop).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.txtcrop).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.txtcrop).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.txtcrop).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.txtcrop).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.txtcrop).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.txtcrop).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.txtcrop).Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcrop.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.txtcrop).Location = new System.Drawing.Point(116, 98);
		((System.Windows.Forms.Control)(object)this.txtcrop).Name = "txtcrop";
		((System.Windows.Forms.Control)(object)this.txtcrop).Size = new System.Drawing.Size(220, 25);
		((System.Windows.Forms.Control)(object)this.txtcrop).TabIndex = 36;
		this.cmbitem.AllowNull = (DefaultableBoolean)1;
		this.cmbitem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbitem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbitem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbitem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbitem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbitem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbitem).Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbitem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbitem).Location = new System.Drawing.Point(462, 67);
		((System.Windows.Forms.Control)(object)this.cmbitem).Name = "cmbitem";
		((System.Windows.Forms.Control)(object)this.cmbitem).Size = new System.Drawing.Size(220, 25);
		((System.Windows.Forms.Control)(object)this.cmbitem).TabIndex = 9;
		this.cmbanalysisgroup.AllowNull = (DefaultableBoolean)1;
		this.cmbanalysisgroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbanalysisgroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbanalysisgroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbanalysisgroup).Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbanalysisgroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbanalysisgroup).Location = new System.Drawing.Point(462, 98);
		((System.Windows.Forms.Control)(object)this.cmbanalysisgroup).Name = "cmbanalysisgroup";
		((System.Windows.Forms.Control)(object)this.cmbanalysisgroup).Size = new System.Drawing.Size(220, 25);
		((System.Windows.Forms.Control)(object)this.cmbanalysisgroup).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.cmbanalysisgroup).Leave += new System.EventHandler(cmbanalysisgroup_Leave);
		this.btnimgprevcookingpic.Font = new System.Drawing.Font("Segoe UI", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnimgprevcookingpic.Location = new System.Drawing.Point(980, 153);
		this.btnimgprevcookingpic.Name = "btnimgprevcookingpic";
		this.btnimgprevcookingpic.Size = new System.Drawing.Size(71, 27);
		this.btnimgprevcookingpic.TabIndex = 17;
		this.btnimgprevcookingpic.Text = "Preview";
		this.btnimgprevcookingpic.UseVisualStyleBackColor = true;
		this.btnimgprevcookingpic.Click += new System.EventHandler(btnimgprevcookingpic_Click);
		this.btnprv.Font = new System.Drawing.Font("Segoe UI", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnprv.Location = new System.Drawing.Point(791, 153);
		this.btnprv.Name = "btnprv";
		this.btnprv.Size = new System.Drawing.Size(70, 27);
		this.btnprv.TabIndex = 15;
		this.btnprv.Text = "Preview";
		this.btnprv.UseVisualStyleBackColor = true;
		this.btnprv.Click += new System.EventHandler(btnprv_Click);
		this.txtSampleId.Location = new System.Drawing.Point(1133, 31);
		this.txtSampleId.Margin = new System.Windows.Forms.Padding(2);
		this.txtSampleId.Name = "txtSampleId";
		this.txtSampleId.Size = new System.Drawing.Size(26, 25);
		this.txtSampleId.TabIndex = 35;
		this.txtSampleId.Visible = false;
		this.txtGpId.Location = new System.Drawing.Point(1097, 31);
		this.txtGpId.Margin = new System.Windows.Forms.Padding(2);
		this.txtGpId.Name = "txtGpId";
		this.txtGpId.Size = new System.Drawing.Size(26, 25);
		this.txtGpId.TabIndex = 34;
		this.txtGpId.Visible = false;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label9.Location = new System.Drawing.Point(17, 102);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(66, 17);
		this.label9.TabIndex = 33;
		this.label9.Text = "Crop Year";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label12.Location = new System.Drawing.Point(17, 131);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(86, 17);
		this.label12.TabIndex = 32;
		this.label12.Text = "Container No";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label4.Location = new System.Drawing.Point(190, 71);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(70, 17);
		this.label4.TabIndex = 32;
		this.label4.Text = "Vehicle No";
		this.txtcontainerno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcontainerno.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtcontainerno.Location = new System.Drawing.Point(115, 127);
		this.txtcontainerno.Name = "txtcontainerno";
		this.txtcontainerno.Size = new System.Drawing.Size(68, 25);
		this.txtcontainerno.TabIndex = 6;
		this.txtvehicleno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvehicleno.Enabled = false;
		this.txtvehicleno.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtvehicleno.Location = new System.Drawing.Point(261, 67);
		this.txtvehicleno.Name = "txtvehicleno";
		this.txtvehicleno.Size = new System.Drawing.Size(75, 25);
		this.txtvehicleno.TabIndex = 5;
		this.txtbiltyno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbiltyno.Enabled = false;
		this.txtbiltyno.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtbiltyno.Location = new System.Drawing.Point(116, 67);
		this.txtbiltyno.Name = "txtbiltyno";
		this.txtbiltyno.Size = new System.Drawing.Size(73, 25);
		this.txtbiltyno.TabIndex = 4;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Times New Roman", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(931, 2);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(71, 15);
		this.label3.TabIndex = 29;
		this.label3.Text = "Cooking Pic";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Times New Roman", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(737, 3);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(74, 15);
		this.label2.TabIndex = 28;
		this.label2.Text = "Analysis Pic";
		this.chkisaccepted.AutoSize = true;
		this.chkisaccepted.Checked = true;
		this.chkisaccepted.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkisaccepted.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.chkisaccepted.ForeColor = System.Drawing.Color.White;
		this.chkisaccepted.Location = new System.Drawing.Point(667, 158);
		this.chkisaccepted.Name = "chkisaccepted";
		this.chkisaccepted.Size = new System.Drawing.Size(15, 14);
		this.chkisaccepted.TabIndex = 13;
		this.chkisaccepted.UseVisualStyleBackColor = true;
		this.btnimage2.Font = new System.Drawing.Font("Segoe UI", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnimage2.Location = new System.Drawing.Point(899, 153);
		this.btnimage2.Name = "btnimage2";
		this.btnimage2.Size = new System.Drawing.Size(61, 27);
		this.btnimage2.TabIndex = 16;
		this.btnimage2.Text = "Browse";
		this.btnimage2.UseVisualStyleBackColor = true;
		this.btnimage2.Click += new System.EventHandler(btnimage2_Click);
		this.cookingpic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.cookingpic.Location = new System.Drawing.Point(899, 21);
		this.cookingpic.Name = "cookingpic";
		this.cookingpic.Size = new System.Drawing.Size(152, 127);
		this.cookingpic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.cookingpic.TabIndex = 27;
		this.cookingpic.TabStop = false;
		this.sampleanalysispicture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.sampleanalysispicture.Location = new System.Drawing.Point(703, 21);
		this.sampleanalysispicture.Name = "sampleanalysispicture";
		this.sampleanalysispicture.Size = new System.Drawing.Size(158, 127);
		this.sampleanalysispicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.sampleanalysispicture.TabIndex = 27;
		this.sampleanalysispicture.TabStop = false;
		this.txtSampleNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSampleNo.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtSampleNo.Location = new System.Drawing.Point(1097, 67);
		this.txtSampleNo.Name = "txtSampleNo";
		this.txtSampleNo.Size = new System.Drawing.Size(26, 25);
		this.txtSampleNo.TabIndex = 7;
		this.txtSampleNo.Visible = false;
		this.txtsupcode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsupcode.Enabled = false;
		this.txtsupcode.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtsupcode.Location = new System.Drawing.Point(462, 40);
		this.txtsupcode.Name = "txtsupcode";
		this.txtsupcode.Size = new System.Drawing.Size(220, 25);
		this.txtsupcode.TabIndex = 3;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label7.Location = new System.Drawing.Point(17, 158);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(58, 17);
		this.label7.TabIndex = 0;
		this.label7.Text = "Remarks";
		this.txtdocdate.CustomFormat = "dd-MM-yy";
		this.txtdocdate.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtdocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdocdate.Location = new System.Drawing.Point(462, 13);
		this.txtdocdate.Name = "txtdocdate";
		this.txtdocdate.Size = new System.Drawing.Size(220, 25);
		this.txtdocdate.TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label1.Location = new System.Drawing.Point(17, 17);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(89, 17);
		this.label1.TabIndex = 0;
		this.label1.Text = "Document No";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1227, 644);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "InvLabSaleAnalysis";
		this.Text = "Lab Sale Analysis";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InvLabSaleAnalysis_FormClosing);
		base.Load += new System.EventHandler(InvLabPurchaseAnalysis_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvLabPurchaseAnalysis_KeyDown);
		this.btn.ResumeLayout(false);
		this.btn.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.tabHistory.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).EndInit();
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.tabControl1.ResumeLayout(false);
		this.tabForm.ResumeLayout(false);
		this.tabForm.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.txtgatepassno).EndInit();
		((System.ComponentModel.ISupportInitialize)this.txtcrop).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbanalysisgroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cookingpic).EndInit();
		((System.ComponentModel.ISupportInitialize)this.sampleanalysispicture).EndInit();
		base.ResumeLayout(false);
	}
}
