using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.BLL.PartyProcessing;
using Architecture.BLL.Reports.Party_Processing;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.PartyProcessing;
using Architecture.Reports;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.PartyProcessing;

public class WorkingReport : Form
{
	private bool DoHaveSaveRight;

	private bool DoHavePrintRights;

	private bool DoHaveUpdateRights;

	private bool DoHaveFieldChooserRights;

	private bool DoHaveSaveLayoutRights;

	private bool DoHaveGridPrintRights;

	private bool DoHaveGridExportRights;

	private bool DoHaveGridGroupExpandRights;

	private bool DoHaveGridGroupCollapseRights;

	private bool DoHaveReportExportRights;

	private bool DoHaveReportPrintRights;

	private bool DoHaveCanViewAllRecordRights;

	private int updateDetailIndex;

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private int RecId;

	private int GPID;

	public static int DocumentTypeId;

	private DataTable dtdetail = new DataTable();

	private DataTable dtEmptyBag = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private List<InvGrnPartyProcessingDetail> lstRemoveRecord = new List<InvGrnPartyProcessingDetail>();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton printToolStripButton;

	private ToolStripButton btnAttachment;

	private Panel panel4;

	private Panel panel5;

	private Label lblvehno;

	private TextBox txtvehno;

	private Label label20;

	private TextBox txtsuppwt;

	private Label lblsupwt;

	private TextBox txtfctwt;

	internal UltraCombo combrefprty;

	internal UltraCombo combstockprty;

	private Label label12;

	private Label label8;

	private DateTimePicker DocDate;

	private Label label17;

	internal UltraCombo CmbPackUom;

	internal UltraCombo CmbWareHouse;

	internal UltraCombo CmbPackingtype;

	internal UltraCombo CmbItem;

	private Label label30;

	private TextBox txtQty;

	private Label label42;

	private Label label41;

	private Label label40;

	private Label label36;

	private Panel panel7;

	private Panel panel8;

	internal GridEX GrdHistory;

	private CtrlGrdBar ctrlGrdBar1;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private ToolStripButton toolStripButton1;

	private Label label16;

	private TextBox txtEbWtOther;

	private Panel panel10;

	internal GridEX grdDetail;

	private CheckBox ChkBox;

	private Panel panel6;

	internal GridEX grdGp;

	private ToolStripButton CityDefine;

	private Button Add;

	private Panel panel11;

	internal GridEX GrdHistoryDetail;

	private CtrlGrdBar ctrlGrdBar2;

	private ToolStrip toolStrip2;

	private ToolStripButton btnLoadAll;

	private ToolStripButton btnDelete;

	private ToolStripLabel lblFormCaption;

	internal GridEX grdEmptyBags;

	private Panel panel9;

	private Panel panel3;

	private CtrlGrdBar ctrlGrdBar3;

	private ToolStripButton btnGrnFormHistory;

	internal UltraCombo CmbGatePass;

	private Label label1;

	private Label label3;

	private TextBox txtCommentsDetail;

	private Label label2;

	private TextBox txtManualReportNo;

	private Label label11;

	internal UltraCombo cmbSupplier;

	private GroupBox groupBox1;

	private Label label13;

	private TextBox textBox5;

	private Button button4;

	internal UltraCombo ultraCombo2;

	private Button button5;

	private Button button6;

	private Label label7;

	private Label label9;

	private TextBox textBox3;

	private Label label10;

	private TextBox textBox4;

	private GroupBox groupBox3;

	private Button button1;

	internal UltraCombo ultraCombo1;

	private Button button2;

	private Button button3;

	private Label label6;

	private Label label5;

	private TextBox textBox2;

	private Label label4;

	private TextBox textBox1;

	private Panel panel13;

	private Panel panel12;

	public WorkingReport(UserAccount user)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void workingReport_Load(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			List<tblUserRights> lstRights = tblUserRights.GetByUserId(new tblUserRights
			{
				UserId = UserAccount.ID,
				ScreenName = base.Name,
				RightName = UserAccount.RoleName
			});
			if (lstRights.Count > 0)
			{
				List<tblUserRights> newList = lstRights.Where((tblUserRights x) => x.RightName == "Save").ToList();
				DoHaveSaveRight = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Print").ToList();
				DoHavePrintRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Update").ToList();
				DoHaveUpdateRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Field Chooser").ToList();
				DoHaveFieldChooserRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Save Layout").ToList();
				DoHaveSaveLayoutRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Grid Print").ToList();
				DoHaveGridPrintRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Grid Export").ToList();
				DoHaveGridExportRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Group Collapse").ToList();
				DoHaveGridGroupCollapseRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Group Expand").ToList();
				DoHaveGridGroupExpandRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Report Export").ToList();
				DoHaveReportExportRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Report Print").ToList();
				DoHaveReportPrintRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "CanView AllRecord").ToList();
				DoHaveCanViewAllRecordRights = Conversion.ToBool((object)newList[0].Value.ToString());
			}
			btnSave.Enabled = DoHaveSaveRight;
			printToolStripButton.Enabled = DoHavePrintRights;
			btnUpdate.Enabled = DoHaveUpdateRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = DoHaveGridGroupExpandRights;
			dtViewAttachments.Columns.Add("AttchmentPath");
			dtdetail.Columns.Add("Id");
			dtdetail.Columns.Add("ItemId");
			dtdetail.Columns.Add("Item");
			dtdetail.Columns.Add("CropYearId");
			dtdetail.Columns.Add("CropYear");
			dtdetail.Columns.Add("JobId");
			dtdetail.Columns.Add("Job");
			dtdetail.Columns.Add("PackingTypeId");
			dtdetail.Columns.Add("PackingType");
			dtdetail.Columns.Add("PackUomId");
			dtdetail.Columns.Add("PackUom");
			dtdetail.Columns.Add("Qty", typeof(double));
			dtdetail.Columns.Add("GrossWight", typeof(double));
			dtdetail.Columns.Add("EbUnit", typeof(double));
			dtdetail.Columns.Add("EbTotal", typeof(double));
			dtdetail.Columns.Add("AddLesswt", typeof(double));
			dtdetail.Columns.Add("StockWeight", typeof(double));
			dtdetail.Columns.Add("WareHouseId");
			dtdetail.Columns.Add("WareHouseName");
			dtdetail.Columns.Add("CityId");
			dtdetail.Columns.Add("CityName");
			dtdetail.Columns.Add("Comments");
			grdDetail.DataSource = dtdetail;
			grdDetail.RetrieveStructure();
			grdSettings();
			dtEmptyBag.Columns.Add("ItemId");
			dtEmptyBag.Columns.Add("Qty", typeof(double));
			dtEmptyBag.Columns.Add("Remarks");
			AddRowInvEmptyBagsGrid();
			grdEmptyBags.RetrieveStructure();
			grdEmptyBagsSettings();
			PackingTypeFill();
			WarehouseFill();
			GatePassfill();
			ItemNameFill();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			DocDate.Focus();
			ChkBox.Checked = true;
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
			if (FormValidationDetail())
			{
				dtdetail.Rows.Add(0, txtManualReportNo.Text.Trim(), DocDate, cmbSupplier.Value, ((Control)(object)cmbSupplier).Text, CmbItem.Value, ((Control)(object)CmbItem).Text, CmbPackUom.Value, ((Control)(object)CmbPackUom).Text.Trim(), Conversion.ToDouble((object)txtQty.Text.Trim()), CmbPackingtype.Value, ((Control)(object)CmbPackingtype).Text, CmbWareHouse.Value, ((Control)(object)CmbWareHouse).Text.Trim(), txtCommentsDetail.Text.Trim());
				grdDetail.DataSource = dtdetail;
				grdDetail.RetrieveStructure();
				grdSettings();
				ResetDetial();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Expected O, but got Unknown
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_048f: Expected O, but got Unknown
		//IL_054b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0552: Expected O, but got Unknown
		//IL_067d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0682: Unknown result type (might be due to invalid IL or missing references)
		//IL_0693: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06af: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bc: Expected O, but got Unknown
		//IL_06dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fd: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!FormValidation())
			{
				return;
			}
			if (grdDetail.GetRows().Count() == 0)
			{
				MessageBox.Show("Grid Record Not Found");
				return;
			}
			InvGrnPartyProcessing obj = new InvGrnPartyProcessing();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				obj.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			obj.DocumentTypeId = DocumentTypeId;
			obj.DocDate = DocDate.Value;
			obj.SupplierCustomerId = Conversion.ToInt(CmbGatePass.Value);
			obj.PartyWeight = Conversion.ToDouble((object)txtsuppwt.Text.Trim());
			obj.FactoryWeight = Conversion.ToDouble((object)txtfctwt.Text.Trim());
			obj.VehicleNo = Conversion.ToString((object)txtvehno.Text.Trim());
			obj.GatePassId = Conversion.ToInt((object)GPID);
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.PostDate = DateTime.Now;
			obj.EntryUser = UserAccount.ID;
			obj.ModifyUser = UserAccount.ID;
			obj.InvGrnPartyProcessingDetailslist = new List<InvGrnPartyProcessingDetail>();
			obj.InvGrnEmptyBagsPartyProcessingslist = new List<InvGrnEmptyBagsPartyProcessing>();
			double GrossWeight = 0.0;
			double RiceQty = 0.0;
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				InvGrnPartyProcessingDetail grnDetail = new InvGrnPartyProcessingDetail();
				grnDetail.Id = Conversion.ToInt(r2.Cells["Id"].Value);
				grnDetail.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
				grnDetail.CropYearId = Conversion.ToInt(r2.Cells["CropYearId"].Value);
				grnDetail.JobLotId = Conversion.ToInt(r2.Cells["JobId"].Value);
				grnDetail.PackingTypeId = Conversion.ToInt(r2.Cells["PackingTypeId"].Value);
				grnDetail.ItemQty = Conversion.ToDouble(r2.Cells["Qty"].Value);
				grnDetail.ItemUomId = Conversion.ToInt(r2.Cells["PackUomId"].Value);
				grnDetail.GrossWeight = Conversion.ToDouble(r2.Cells["GrossWight"].Value);
				GrossWeight += grnDetail.GrossWeight;
				RiceQty += grnDetail.ItemQty;
				grnDetail.EBWPerUnit = Conversion.ToDouble(r2.Cells["EbUnit"].Value);
				grnDetail.EBWTotal = Conversion.ToDouble(r2.Cells["EbTotal"].Value);
				grnDetail.AdLsWeight = Conversion.ToDouble(r2.Cells["AddLesswt"].Value);
				grnDetail.StockWeight = Conversion.ToDouble(r2.Cells["StockWeight"].Value);
				grnDetail.NetBillWeight = grnDetail.StockWeight;
				grnDetail.WarehouseId = Conversion.ToInt(r2.Cells["WareHouseId"].Value);
				grnDetail.CityId = Conversion.ToInt(r2.Cells["CityId"].Value);
				grnDetail.CommentsDetail = Conversion.ToString((object)r2.Cells["Comments"].Text.Trim());
				if (grnDetail.Id > 0)
				{
					grnDetail.ActionTypeId = 2;
				}
				else
				{
					grnDetail.ActionTypeId = 1;
				}
				obj.InvGrnPartyProcessingDetailslist.Add(grnDetail);
			}
			double Factoryweight = Conversion.ToDouble((object)txtfctwt.Text.Trim());
			if (DocumentTypeId != 89 && GPID > 0 && GrossWeight != Factoryweight)
			{
				throw new Exception("Sum of GrossWeight not equal to FactoryWeight Please Check");
			}
			double EBQty = 0.0;
			rows = grdEmptyBags.GetRows();
			foreach (GridEXRow r in rows)
			{
				InvGrnEmptyBagsPartyProcessing invGrnEmpty = new InvGrnEmptyBagsPartyProcessing();
				invGrnEmpty.DocumentTypeId = DocumentTypeId;
				invGrnEmpty.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				invGrnEmpty.ItemQty = Conversion.ToDouble(r.Cells["Qty"].Value);
				invGrnEmpty.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
				obj.InvGrnEmptyBagsPartyProcessingslist.Add(invGrnEmpty);
				EBQty += invGrnEmpty.ItemQty;
			}
			if (EBQty > RiceQty)
			{
				throw new Exception("EmptyBags Qty cannot be greater than RiceQty");
			}
			obj.AttachmentsList = new List<DMSAttachments>();
			for (int j = 0; j < Attachment.dtAttachments.Rows.Count; j++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Attachment.dtAttachments.Rows[j][1].ToString();
				if (DocumentTypeId == 49)
				{
					attach.ScreenName = "GoodsReceivingNotesPartyProcessing";
				}
				if (DocumentTypeId == 89)
				{
					attach.ScreenName = "GoodsDispatchNotesPartyProcessing";
				}
				obj.AttachmentsList.Add(attach);
			}
			if (lstRemoveRecord.Count > 0)
			{
				for (int i = 0; i < lstRemoveRecord.Count; i++)
				{
					obj.InvGrnPartyProcessingDetailslist.Add(lstRemoveRecord[i]);
				}
			}
			int success = InvGrnPartyProcessing.Save(obj);
			if (RecId > 0)
			{
				MessageBox.Show("Receord Update Successfully [" + obj.DocNo + "]");
			}
			else
			{
				MessageBox.Show("Receord Save Successfully [" + obj.DocNo + "]");
			}
			reset();
			if (ChkBox.Checked)
			{
				try
				{
					DataTable dt = new DataTable();
					dt = GrnGdnReports.GrnAndGdnSlipAndRegister(new ReportsParameters
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocumentTypeId = DocumentTypeId,
						Id = success
					});
					if (dt == null || dt.Rows.Count == 0)
					{
						MessageBox.Show("Not Record Found For Display");
					}
					else
					{
						Reporting val = new Reporting();
						val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
						val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
						val.ShowReportWithDataTable(dt, "333-InvRptGoodsReceiptsNotesRiceSlip.rpt");
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
			grdDetail.DataSource = null;
			btnSave.Visible = true;
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
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

	public void ReadById_Update(int ID)
	{
		try
		{
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			RecId = Conversion.ToInt((object)ID);
			InvGrnPartyProcessing obj = InvGrnPartyProcessing.GetByID(RecId);
			DocDate.Value = obj.DocDate;
			tabControl1.SelectedIndex = 0;
			CmbGatePass.Value = obj.SupplierCustomerId;
			txtsuppwt.Text = obj.PartyWeight.ToString();
			txtfctwt.Text = obj.FactoryWeight.ToString();
			txtvehno.Text = obj.VehicleNo;
			GPID = Conversion.ToInt((object)obj.GatePassId);
			dtdetail.Rows.Clear();
			for (int j = 0; j < obj.InvGrnPartyProcessingDetailslist.Count; j++)
			{
				dtdetail.Rows.Add(obj.InvGrnPartyProcessingDetailslist[j].Id, obj.InvGrnPartyProcessingDetailslist[j].ItemId, obj.InvGrnPartyProcessingDetailslist[j].ItemName, obj.InvGrnPartyProcessingDetailslist[j].CropYearId, obj.InvGrnPartyProcessingDetailslist[j].CropYear, obj.InvGrnPartyProcessingDetailslist[j].JobLotId, obj.InvGrnPartyProcessingDetailslist[j].JobLotDescription, obj.InvGrnPartyProcessingDetailslist[j].PackingTypeId, obj.InvGrnPartyProcessingDetailslist[j].PackTypeDesc, obj.InvGrnPartyProcessingDetailslist[j].ItemUomId, obj.InvGrnPartyProcessingDetailslist[j].UOMCode, obj.InvGrnPartyProcessingDetailslist[j].ItemQty, obj.InvGrnPartyProcessingDetailslist[j].GrossWeight, obj.InvGrnPartyProcessingDetailslist[j].EBWPerUnit, obj.InvGrnPartyProcessingDetailslist[j].EBWTotal, obj.InvGrnPartyProcessingDetailslist[j].AdLsWeight, obj.InvGrnPartyProcessingDetailslist[j].StockWeight, obj.InvGrnPartyProcessingDetailslist[j].WarehouseId, obj.InvGrnPartyProcessingDetailslist[j].WareHouseName, obj.InvGrnPartyProcessingDetailslist[j].CityId, obj.InvGrnPartyProcessingDetailslist[j].CityName, obj.InvGrnPartyProcessingDetailslist[j].CommentsDetail);
			}
			grdDetail.DataSource = dtdetail;
			grdDetail.RetrieveStructure();
			grdSettings();
			dtEmptyBag.Clear();
			for (int i = 0; i < obj.InvGrnEmptyBagsPartyProcessingslist.Count; i++)
			{
				dtEmptyBag.Rows.Add(obj.InvGrnEmptyBagsPartyProcessingslist[i].ItemId, obj.InvGrnEmptyBagsPartyProcessingslist[i].ItemQty, obj.InvGrnEmptyBagsPartyProcessingslist[i].Remarks);
			}
			if (dtEmptyBag.Rows.Count == 0)
			{
				AddRowInvEmptyBagsGrid();
			}
			grdEmptyBags.DataSource = dtEmptyBag;
			grdEmptyBags.RetrieveStructure();
			grdEmptyBagsSettings();
			List<DMSAttachments> byID = DMSAttachments.GetByID(int.Parse(RecId.ToString()), base.Name);
			AT = new Attachment();
			foreach (DMSAttachments Attachment in byID)
			{
				TempAttachments TA = new TempAttachments();
				TA.Id = 0;
				TA.Path = Attachment.Attachment;
				AT.lst.Add(TA);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)CmbGatePass).ActiveRow == null)
		{
			MessageBox.Show("Field is Required");
			CmbGatePass.Focus();
			return false;
		}
		if (GPID > 0 && (Conversion.ToDouble((object)txtfctwt.Text.Trim()) == 0.0 || txtfctwt.Text.Trim() == null))
		{
			MessageBox.Show("Factory Weight Field is Required");
			txtfctwt.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)CmbItem).ActiveRow == null)
		{
			MessageBox.Show("Item Name Field is Required");
			CmbItem.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackingtype).ActiveRow == null)
		{
			MessageBox.Show("Packing Type Field is Required");
			CmbPackingtype.Focus();
			return false;
		}
		if (txtQty.Text.Trim() == "" || Conversion.ToDouble((object)txtQty.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Qty Field is Required");
			txtQty.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackUom).ActiveRow == null)
		{
			MessageBox.Show("Pack Unit Field is Required");
			CmbPackUom.Focus();
			return false;
		}
		if (((UltraGridBase)CmbWareHouse).ActiveRow == null)
		{
			MessageBox.Show("Warehouse Field is Required");
			CmbWareHouse.Focus();
			return false;
		}
		if (((UltraGridBase)cmbSupplier).ActiveRow == null)
		{
			MessageBox.Show("Supplier Field is Required");
			cmbSupplier.Focus();
			return false;
		}
		return true;
	}

	private void loadonGdnvisibleitem(bool value)
	{
		try
		{
			txtvehno.ReadOnly = value;
			txtsuppwt.ReadOnly = value;
			txtfctwt.ReadOnly = value;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Supplierbind()
	{
		try
		{
			DataTable dtsupplier = CommonServices.SupplierCustomerGetAllServiceBind();
			if (dtsupplier.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtsupplier, cmbSupplier, "Id", "CompanyName", "Supplier Name", false);
				((UltraGridBase)cmbSupplier).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GRNGridSetting()
	{
		try
		{
			grdGp.RootTable.Columns["Id"].Visible = false;
			grdGp.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdGp.RootTable.Columns["SupplierWeight"].AggregateFunction = (AggregateFunction)2;
			grdGp.RootTable.Columns["FactoryWeight"].AggregateFunction = (AggregateFunction)2;
			grdGp.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdGp.RootTable.Columns["SupplierWeight"].TextAlignment = (TextAlignment)3;
			grdGp.RootTable.Columns["FactoryWeight"].TextAlignment = (TextAlignment)3;
			grdGp.RootTable.Columns["ItemQty"].FormatString = "0,0";
			grdGp.RootTable.Columns["SupplierWeight"].FormatString = "0,0";
			grdGp.RootTable.Columns["FactoryWeight"].FormatString = "0,0";
			grdGp.RootTable.Columns.Add("Load");
			grdGp.RootTable.Columns["Load"].Caption = "Load";
			grdGp.RootTable.Columns["Load"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdGp.RootTable.Columns["Load"].ButtonStyle = (ButtonStyle)5;
			grdGp.RootTable.Columns["Load"].ButtonText = "Load";
			grdGp.RootTable.Columns["Load"].Width = 40;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ItemNameFill()
	{
		try
		{
			DataTable dt = CommonServices.ReadAllItems();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbItem, "Id", "ItemName", "ItemName", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackingTypeFill()
	{
		try
		{
			DataTable dtpkt = new DataTable();
			dtpkt = InvPackingType.Getall();
			if (dtpkt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtpkt, CmbPackingtype, "Id", "PackTypeDesc", "PackingType", false);
				((UltraGridBase)CmbPackingtype).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackUOMFillWithoutOrder()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			UOMSchedule val = new UOMSchedule
			{
				ItemId = Conversion.ToInt(CmbItem.Value),
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtsearchby = new DataTable();
			dtsearchby = UOMSchedule.SearchByObject(val);
			if (dtsearchby.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtsearchby, CmbPackUom, "Id", "Equivalent", "Item Pack UOM", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WarehouseFill()
	{
		try
		{
			DataTable dtwh = CommonServices.WareHousePartyProcessingGetAllService();
			if (dtwh.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtwh, CmbWareHouse, "Id", "WareHouseName", "Godown Name", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GatePassfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			new GatePassInward
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtGatepass = new DataTable();
			if (dtGatepass.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtGatepass, CmbGatePass, "Id", "DocNo", "Gate Pass#", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GatePassRecordFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			GatePassPartyProcessing obj = new GatePassPartyProcessing();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.GpSrNo = Conversion.ToInt((object)((Control)(object)CmbGatePass).Text.Trim());
			obj.FinancialYearId = Conversion.ToInt((object)clsGlobalVariables.ActiveYr.Id);
			if (DocumentTypeId == 49)
			{
				obj.DocumentTypeId = 54;
			}
			if (DocumentTypeId == 89)
			{
				obj.DocumentTypeId = 55;
			}
			DataTable dtgpForGRn = GatePassPartyProcessing.ReadByGpNoForInvGrnPartyProcessing(obj);
			if (dtgpForGRn.Rows.Count > 0)
			{
				txtvehno.Text = dtgpForGRn.Rows[0]["VehicleNo"].ToString();
				txtQty.Text = dtgpForGRn.Rows[0]["ItemQty"].ToString();
				txtsuppwt.Text = dtgpForGRn.Rows[0]["SupplierWeight"].ToString();
				txtfctwt.Text = dtgpForGRn.Rows[0]["FactoryWeight"].ToString();
				CmbGatePass.Value = dtgpForGRn.Rows[0]["SupplierCustomerId"].ToString();
				((Control)(object)CmbItem).Text = dtgpForGRn.Rows[0]["VarietyName"].ToString();
				loadonGdnvisibleitem(value: true);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combitem_Leave(object sender, EventArgs e)
	{
		try
		{
			PackUOMFillWithoutOrder();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Total()
	{
		try
		{
			if (((Control)(object)CmbPackUom).Text != string.Empty)
			{
				double.Parse(((Control)(object)CmbPackUom).Text.Trim().ToString());
			}
			if (txtQty.Text != string.Empty && txtQty.Text != null)
			{
				double.Parse(txtQty.Text.Trim().ToString());
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalWeight()
	{
		try
		{
			double PackUOM = 0.0;
			double Qty = 0.0;
			if (((Control)(object)CmbPackUom).Text != string.Empty)
			{
				PackUOM = double.Parse(((Control)(object)CmbPackUom).Text.ToString());
			}
			if (txtQty.Text != string.Empty)
			{
				Qty = double.Parse(txtQty.Text.ToString());
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalSupplierWeight()
	{
		try
		{
			if (txtsuppwt.Text != string.Empty && txtfctwt.Text != string.Empty)
			{
				double.Parse(txtsuppwt.Text.Trim());
				double FactoryWeight = double.Parse(txtfctwt.Text.Trim());
			}
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
			RecId = 0;
			((Control)(object)combstockprty).Text = string.Empty;
			((Control)(object)combrefprty).Text = string.Empty;
			txtsuppwt.Text = string.Empty;
			txtfctwt.Text = string.Empty;
			txtvehno.Text = string.Empty;
			((Control)(object)CmbItem).Text = string.Empty;
			txtQty.Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			DocDate.Focus();
			dtdetail.Clear();
			grdDetail.DataSource = dtdetail;
			dtEmptyBag.Rows.Clear();
			AddRowInvEmptyBagsGrid();
			grdEmptyBags.RetrieveStructure();
			grdEmptyBagsSettings();
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			Add.Visible = true;
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			if (DocumentTypeId == 89)
			{
				loadonGdnvisibleitem(value: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetDetial()
	{
		try
		{
			txtQty.Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			txtCommentsDetail.Text = string.Empty;
			CmbItem.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		try
		{
			grdDetail.RootTable.Columns.Add("Delete");
			grdDetail.RootTable.Columns["Delete"].Key = "Delete";
			grdDetail.RootTable.Columns["Delete"].Caption = "X";
			grdDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdDetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdDetail.RootTable.Columns["Delete"].Width = 20;
			grdDetail.RootTable.Columns["Id"].Visible = false;
			grdDetail.RootTable.Columns["ItemId"].Visible = false;
			grdDetail.RootTable.Columns["CropYearId"].Visible = false;
			grdDetail.RootTable.Columns["CityId"].Visible = false;
			grdDetail.RootTable.Columns["PackUomId"].Visible = false;
			grdDetail.RootTable.Columns["JobId"].Visible = false;
			grdDetail.RootTable.Columns["PackingTypeId"].Visible = false;
			grdDetail.RootTable.Columns["WareHouseId"].Visible = false;
			grdDetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["GrossWight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["EbUnit"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["EbTotal"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["AddLesswt"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["GrossWight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["EbUnit"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["EbTotal"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["AddLesswt"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Qty"].FormatString = "#,##.##";
			grdDetail.RootTable.Columns["GrossWight"].FormatString = "#,##.##";
			grdDetail.RootTable.Columns["EbUnit"].FormatString = "#,##.##";
			grdDetail.RootTable.Columns["EbTotal"].FormatString = "#,##.##";
			grdDetail.RootTable.Columns["AddLesswt"].FormatString = "#,##.##";
			grdDetail.RootTable.Columns["StockWeight"].FormatString = "#,##.##";
			grdDetail.RootTable.Columns["Qty"].TotalFormatString = "#,##.##";
			grdDetail.RootTable.Columns["GrossWight"].TotalFormatString = "#,##.##";
			grdDetail.RootTable.Columns["EbUnit"].TotalFormatString = "#,##.##";
			grdDetail.RootTable.Columns["EbTotal"].TotalFormatString = "#,##.##";
			grdDetail.RootTable.Columns["AddLesswt"].TotalFormatString = "#,##.##";
			grdDetail.RootTable.Columns["StockWeight"].TotalFormatString = "#,##.##";
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInvEmptyBagsGrid()
	{
		dtEmptyBag.Rows.Add(0, 0, "");
		grdEmptyBags.DataSource = dtEmptyBag;
	}

	private void grdEmptyBagsSettings()
	{
		try
		{
			DataTable dt = CommonServices.GetItemPartyProcessing("7");
			grdEmptyBags.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			grdEmptyBags.RootTable.Columns["ItemId"].HasValueList = true;
			grdEmptyBags.RootTable.Columns["ItemId"].LimitToList = true;
			grdEmptyBags.RootTable.Columns["ItemId"].EditType = (EditType)4;
			grdEmptyBags.RootTable.Columns["ItemId"].Caption = "Item Name";
			grdEmptyBags.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dt.DefaultView, "Id", "ItemName");
			grdEmptyBags.RootTable.Columns["ItemId"].Width = 250;
			grdEmptyBags.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdEmptyBags.RootTable.Columns.Add("Delete");
			grdEmptyBags.RootTable.Columns["Delete"].Key = "Delete";
			grdEmptyBags.RootTable.Columns["Delete"].Caption = "X";
			grdEmptyBags.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdEmptyBags.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdEmptyBags.RootTable.Columns["Delete"].ButtonText = "X";
			grdEmptyBags.RootTable.Columns["Delete"].Width = 20;
			grdEmptyBags.RootTable.Columns.Add("Add");
			grdEmptyBags.RootTable.Columns["Add"].Key = "Add";
			grdEmptyBags.RootTable.Columns["Add"].Caption = "+";
			grdEmptyBags.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdEmptyBags.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdEmptyBags.RootTable.Columns["Add"].ButtonText = "+";
			grdEmptyBags.RootTable.Columns["Add"].Width = 20;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridFill(int NoOfRecords = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters grn = new ReportsParameters();
			grn.OrganizationId = UserAccount.OrganizationId;
			grn.CompanyId = UserAccount.CompanyId;
			grn.CanViewAllRecord = DoHaveCanViewAllRecordRights;
			grn.NoOfRecords = NoOfRecords;
			grn.DocumentTypeId = DocumentTypeId;
			if (!grn.CanViewAllRecord)
			{
				grn.EntryUser = UserAccount.ID;
			}
			DataTable dthis = new DataTable();
			dthis = InvGrnPartyProcessing.FormHistory(grn);
			if (dthis.Rows.Count > 0)
			{
				GrdHistory.DataSource = dthis;
				GrdHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				GrdHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSettings()
	{
		try
		{
			GrdHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			GrdHistory.RootTable.Columns["RecordNo"].Visible = false;
			GrdHistory.RootTable.Columns["Id"].Visible = false;
			GrdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			GrdHistory.RootTable.Columns.Add("Detail");
			GrdHistory.RootTable.Columns["Detail"].Caption = "Detail";
			GrdHistory.RootTable.Columns["Detail"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistory.RootTable.Columns["Detail"].ButtonStyle = (ButtonStyle)5;
			GrdHistory.RootTable.Columns["Detail"].ButtonText = "Detail";
			GrdHistory.RootTable.Columns["Detail"].Width = 30;
			GrdHistory.RootTable.Columns.Add("Edit");
			GrdHistory.RootTable.Columns["Edit"].Caption = "Edit";
			GrdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			GrdHistory.RootTable.Columns["Edit"].Image = Resources.icons8_pencil_16;
			GrdHistory.RootTable.Columns["Edit"].Width = 25;
			GrdHistory.RootTable.Columns.Add("Print");
			GrdHistory.RootTable.Columns["Print"].Caption = "Print";
			GrdHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			GrdHistory.RootTable.Columns["Print"].Image = Resources.icons8_print_16;
			GrdHistory.RootTable.Columns["Print"].Width = 25;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void InvFrmGRN_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.Return && tabControl1.SelectedIndex == 0 && btnSave.Visible && btnSave.Enabled)
			{
				if (!(Conversion.ToString(grdGp.CurrentRow.Cells["Status"].Value) == "Accepted"))
				{
					MessageBox.Show("Status Not Accepted Please check status");
					return;
				}
				GPID = Conversion.ToInt(grdGp.CurrentRow.Cells["Id"].Value);
				GatePassRecordFill();
			}
			if (e.Control && e.KeyCode == Keys.L && tabControl1.SelectedIndex == 1)
			{
				btnLoadAll_Click(null, null);
			}
			if (btnSave.Visible && btnSave.Enabled && e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0)
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
			if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				btnUpdate_Click(null, null);
			}
			if (printToolStripButton.Visible && printToolStripButton.Enabled && e.Control && e.KeyCode == Keys.P)
			{
				printToolStripButton_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				CmbItem.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdDetail).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				((Control)(object)grdGp).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Left)
			{
				((Control)(object)grdEmptyBags).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Return && tabControl1.SelectedIndex == 1)
			{
				RecId = Conversion.ToInt((object)GrdHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById_Update(RecId);
			}
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
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)GrdHistory).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)GrdHistory).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				GrdHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdDetail).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdDetail).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				grdDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		reset();
	}

	private void txtqty_TextChanged(object sender, EventArgs e)
	{
		if (btnSave.Visible && btnSave.Enabled)
		{
			TotalWeight();
		}
		Total();
	}

	private void txtebu_TextChanged(object sender, EventArgs e)
	{
		Total();
	}

	private void GrdHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			RecId = Conversion.ToInt((object)GrdHistory.CurrentRow.Cells["Id"].Value.ToString());
			ReadById_Update(RecId);
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

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				GridEXRow r = grdDetail.CurrentRow;
				if (btnSave.Visible && btnSave.Enabled)
				{
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
				}
				else
				{
					InvGrnPartyProcessingDetail grnDetail = new InvGrnPartyProcessingDetail();
					grnDetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
					grnDetail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					grnDetail.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
					grnDetail.JobLotId = Conversion.ToInt(r.Cells["JobId"].Value);
					grnDetail.PackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
					grnDetail.ItemQty = Conversion.ToDouble(r.Cells["Qty"].Value);
					grnDetail.ItemUomId = Conversion.ToInt(r.Cells["PackUomId"].Value);
					grnDetail.GrossWeight = Conversion.ToDouble(r.Cells["GrossWight"].Value);
					grnDetail.EBWPerUnit = Conversion.ToDouble(r.Cells["EbUnit"].Value);
					grnDetail.EBWTotal = Conversion.ToDouble(r.Cells["EbTotal"].Value);
					grnDetail.AdLsWeight = Conversion.ToDouble(r.Cells["AddLesswt"].Value);
					grnDetail.StockWeight = Conversion.ToDouble(r.Cells["StockWeight"].Value);
					grnDetail.NetBillWeight = grnDetail.StockWeight;
					grnDetail.WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
					grnDetail.CityId = Conversion.ToInt(r.Cells["CityId"].Value);
					grnDetail.CommentsDetail = Conversion.ToString((object)r.Cells["Comments"].Text.Trim());
					grnDetail.ActionTypeId = 3;
					lstRemoveRecord.Add(grnDetail);
					dtdetail.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
				}
			}
			grdDetail.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void GrdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Edit")
			{
				reset();
				RecId = Conversion.ToInt((object)GrdHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById_Update(RecId);
			}
			if (e.Column.Key == "Print")
			{
				DataTable dt = new DataTable();
				dt = GrnGdnReports.GrnAndGdnSlipAndRegister(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeId = DocumentTypeId,
					Id = Conversion.ToInt((object)GrdHistory.CurrentRow.Cells["Id"].Value.ToString())
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("Not Record Found For Display");
				}
				else
				{
					Reporting val = new Reporting();
					val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
					val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
					val.ShowReportWithDataTable(dt, "333-InvRptGoodsReceiptsNotesRiceSlip.rpt");
				}
			}
			if (e.Column.Key == "Detail")
			{
				InvGrn invGrn = InvGrn.GetByID(Conversion.ToInt(GrdHistory.CurrentRow.Cells[1].Value));
				DataTable dthistorydetail = new DataTable();
				dthistorydetail.Columns.Add("Order");
				dthistorydetail.Columns.Add("Item");
				dthistorydetail.Columns.Add("CropYear");
				dthistorydetail.Columns.Add("Job");
				dthistorydetail.Columns.Add("PackingType");
				dthistorydetail.Columns.Add("UOM");
				dthistorydetail.Columns.Add("Qty", typeof(double));
				dthistorydetail.Columns.Add("GrossWight", typeof(double));
				dthistorydetail.Columns.Add("EbUnit", typeof(double));
				dthistorydetail.Columns.Add("EbTotal", typeof(double));
				dthistorydetail.Columns.Add("WtCut", typeof(double));
				dthistorydetail.Columns.Add("WtCutTotal", typeof(double));
				dthistorydetail.Columns.Add("AddLesswt", typeof(double));
				dthistorydetail.Columns.Add("NetWeight", typeof(double));
				dthistorydetail.Columns.Add("StockWeight", typeof(double));
				dthistorydetail.Columns.Add("WareHouse");
				dthistorydetail.Columns.Add("LabNo");
				dthistorydetail.Columns.Add("City");
				dthistorydetail.Clear();
				for (int i = 0; i < invGrn.invGrnDetail.Count; i++)
				{
					dthistorydetail.Rows.Add(invGrn.invGrnDetail[i].PurchaseOrder, invGrn.invGrnDetail[i].Item, invGrn.invGrnDetail[i].CropYear, invGrn.invGrnDetail[i].JobLot, invGrn.invGrnDetail[i].PackingType, "", invGrn.invGrnDetail[i].ItemQty, invGrn.invGrnDetail[i].GrossWeight, invGrn.invGrnDetail[i].EBWPerUnit, invGrn.invGrnDetail[i].EBWTotal, invGrn.invGrnDetail[i].WtCut, invGrn.invGrnDetail[i].WtCutTotal, invGrn.invGrnDetail[i].AdLsWeight, invGrn.invGrnDetail[i].NetBillWeight, invGrn.invGrnDetail[i].StockWeight, invGrn.invGrnDetail[i].WarehouseId, invGrn.invGrnDetail[i].LabReportRef, invGrn.invGrnDetail[i].AreaCity);
				}
				GrdHistoryDetail.DataSource = dthistorydetail;
				GrdHistoryDetail.RetrieveStructure();
				gridhistorydetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridhistorydetail()
	{
		try
		{
			GrdHistoryDetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["EbUnit"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["GrossWight"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["EbTotal"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["AddLesswt"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["Qty"].FormatString = "0,0";
			GrdHistoryDetail.RootTable.Columns["EbUnit"].FormatString = "0,0";
			GrdHistoryDetail.RootTable.Columns["GrossWight"].FormatString = "0,0";
			GrdHistoryDetail.RootTable.Columns["EbTotal"].FormatString = "0,0";
			GrdHistoryDetail.RootTable.Columns["AddLesswt"].FormatString = "0,0";
			GrdHistoryDetail.RootTable.Columns["StockWeight"].FormatString = "0,0";
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
			HistoryGridFill(50);
		}
	}

	private void combpckuom_Leave(object sender, EventArgs e)
	{
		if (btnSave.Visible && btnSave.Enabled)
		{
			TotalWeight();
		}
		Total();
	}

	private void txtgwt_TextChanged(object sender, EventArgs e)
	{
		Total();
	}

	private void txtadlswt_TextChanged(object sender, EventArgs e)
	{
		Total();
	}

	private void txtadlswt_Leave(object sender, EventArgs e)
	{
		Total();
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdDetail.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				CmbItem.Value = item.Cells["ItemId"].Value.ToString();
				CmbPackingtype.Value = item.Cells["PackingTypeId"].Value.ToString();
				txtQty.Text = item.Cells["Qty"].Value.ToString();
				CmbPackUom.Value = item.Cells["PackUomId"].Value.ToString();
				CmbWareHouse.Value = item.Cells["WareHouseId"].Value.ToString();
				txtCommentsDetail.Text = item.Cells["Comments"].Text.ToString();
				Add.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				CmbItem.Focus();
			}
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
			if (FormValidationDetail())
			{
				dtdetail.Rows[updateDetailIndex]["ItemId"] = CmbItem.Value;
				dtdetail.Rows[updateDetailIndex]["Item"] = ((Control)(object)CmbItem).Text;
				dtdetail.Rows[updateDetailIndex]["PackingTypeId"] = CmbPackingtype.Value;
				dtdetail.Rows[updateDetailIndex]["PackingType"] = ((Control)(object)CmbPackingtype).Text;
				dtdetail.Rows[updateDetailIndex]["Qty"] = txtQty.Text;
				dtdetail.Rows[updateDetailIndex]["PackUom"] = ((Control)(object)CmbPackUom).Text;
				dtdetail.Rows[updateDetailIndex]["PackUomId"] = CmbPackUom.Value;
				dtdetail.Rows[updateDetailIndex]["WareHouseId"] = CmbWareHouse.Value;
				dtdetail.Rows[updateDetailIndex]["WareHouseName"] = ((Control)(object)CmbWareHouse).Text;
				dtdetail.Rows[updateDetailIndex]["Comments"] = txtCommentsDetail.Text.Trim();
				Add.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				ResetDetial();
				double gridWeight = 0.0;
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow item in rows)
				{
					gridWeight += Conversion.ToDouble(item.Cells["GrossWight"].Value);
				}
				Conversion.ToDouble((object)txtfctwt.Text.Trim());
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtsuppwt_TextChanged(object sender, EventArgs e)
	{
		TotalSupplierWeight();
	}

	private void txtfctwt_TextChanged(object sender, EventArgs e)
	{
		TotalSupplierWeight();
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		try
		{
			Supplierbind();
			PackingTypeFill();
			PackUOMFillWithoutOrder();
			WarehouseFill();
			GatePassfill();
			ItemNameFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateReport()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = new DataTable();
			dt = GrnGdnReports.GrnAndGdnSlipAndRegister(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = DocumentTypeId,
				Id = RecId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "333-InvRptGoodsReceiptsNotesRiceSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void printToolStripButton_Click(object sender, EventArgs e)
	{
		try
		{
			GenerateReport();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtEbWtOther_TextChanged(object sender, EventArgs e)
	{
		Total();
	}

	private void txtebu_TextChanged_1(object sender, EventArgs e)
	{
		Total();
	}

	private void CityDefine_Click(object sender, EventArgs e)
	{
		new DefineCity(UserAccount).Show();
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		Add.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelUpdateDetial.Visible = false;
	}

	private void btnLoadAll_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!(e.Column.Key == "NoOfAttachments"))
			{
				return;
			}
			GridEXRow item = GrdHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			List<DMSAttachments> dMSList = DMSAttachments.GetByID(int.Parse(item.Cells["Id"].Value.ToString().ToString()), base.Name);
			dtViewAttachments.Clear();
			if (dMSList.Count > 0)
			{
				foreach (DMSAttachments itemList in dMSList)
				{
					dtViewAttachments.Rows.Add(itemList.Attachment);
				}
			}
			else
			{
				dtViewAttachments.Clear();
			}
			new AttachmentView(dtViewAttachments).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGp_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (btnSave.Visible && btnSave.Enabled && e.Column.Key == "Load")
			{
				if (!(Conversion.ToString(grdGp.CurrentRow.Cells["Status"].Value) == "Accepted"))
				{
					throw new Exception("Status Not Accepted Please check status");
				}
				GPID = Conversion.ToInt(grdGp.CurrentRow.Cells["Id"].Value);
				GatePassRecordFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdGp).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdGp).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				grdGp.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void txtadlswt_TextChanged_1(object sender, EventArgs e)
	{
		Total();
	}

	private void txtgwt_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtebu_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtwtcut_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtcarramount_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtqty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void grdEmptyBags_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdEmptyBags.CurrentRow;
				dtEmptyBag.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtEmptyBag.Rows.Count == 0)
				{
					AddRowInvEmptyBagsGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInvEmptyBagsGrid();
			}
			grdEmptyBags.UpdateData();
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
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Expected O, but got Unknown
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Expected O, but got Unknown
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Expected O, but got Unknown
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Expected O, but got Unknown
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Expected O, but got Unknown
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Expected O, but got Unknown
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Expected O, but got Unknown
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Expected O, but got Unknown
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_046f: Expected O, but got Unknown
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Expected O, but got Unknown
		//IL_04e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f3: Expected O, but got Unknown
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Expected O, but got Unknown
		//IL_052b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Expected O, but got Unknown
		//IL_0541: Unknown result type (might be due to invalid IL or missing references)
		//IL_054b: Expected O, but got Unknown
		//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cf: Expected O, but got Unknown
		//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0606: Expected O, but got Unknown
		//IL_061d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0627: Expected O, but got Unknown
		//IL_066a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0674: Expected O, but got Unknown
		//IL_0725: Unknown result type (might be due to invalid IL or missing references)
		//IL_072f: Expected O, but got Unknown
		//IL_073b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0745: Expected O, but got Unknown
		//IL_0d4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d56: Expected O, but got Unknown
		//IL_0e9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea9: Expected O, but got Unknown
		//IL_25e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_25f0: Expected O, but got Unknown
		//IL_644c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6456: Expected O, but got Unknown
		//IL_6463: Unknown result type (might be due to invalid IL or missing references)
		//IL_646d: Expected O, but got Unknown
		GridEXLayout grdGp_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdEmptyBags_DesignTimeLayout = new GridEXLayout();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
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
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		GridEXLayout GrdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdHistory_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grdGp = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdEmptyBags = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label13 = new System.Windows.Forms.Label();
		this.textBox5 = new System.Windows.Forms.TextBox();
		this.button4 = new System.Windows.Forms.Button();
		this.ultraCombo2 = new UltraCombo();
		this.button5 = new System.Windows.Forms.Button();
		this.button6 = new System.Windows.Forms.Button();
		this.label7 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.textBox3 = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.textBox4 = new System.Windows.Forms.TextBox();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.button1 = new System.Windows.Forms.Button();
		this.ultraCombo1 = new UltraCombo();
		this.button2 = new System.Windows.Forms.Button();
		this.button3 = new System.Windows.Forms.Button();
		this.label6 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.textBox2 = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.textBox1 = new System.Windows.Forms.TextBox();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.CmbPackUom = new UltraCombo();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.label30 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.Add = new System.Windows.Forms.Button();
		this.cmbSupplier = new UltraCombo();
		this.CmbPackingtype = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.CmbWareHouse = new UltraCombo();
		this.txtManualReportNo = new System.Windows.Forms.TextBox();
		this.label42 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.txtCommentsDetail = new System.Windows.Forms.TextBox();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.label41 = new System.Windows.Forms.Label();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.label40 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.CmbItem = new UltraCombo();
		this.label16 = new System.Windows.Forms.Label();
		this.txtEbWtOther = new System.Windows.Forms.TextBox();
		this.panel4 = new System.Windows.Forms.Panel();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.CmbGatePass = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.lblvehno = new System.Windows.Forms.Label();
		this.combrefprty = new UltraCombo();
		this.txtvehno = new System.Windows.Forms.TextBox();
		this.lblsupwt = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.txtsuppwt = new System.Windows.Forms.TextBox();
		this.txtfctwt = new System.Windows.Forms.TextBox();
		this.combstockprty = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.lblFormCaption = new System.Windows.Forms.ToolStripLabel();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.printToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.CityDefine = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel11 = new System.Windows.Forms.Panel();
		this.GrdHistoryDetail = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.GrdHistory = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.btnGrnFormHistory = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGp).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEmptyBags).BeginInit();
		this.panel10.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo2).BeginInit();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo1).BeginInit();
		this.panel5.SuspendLayout();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingtype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbGatePass).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combrefprty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combstockprty).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryDetail).BeginInit();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistory).BeginInit();
		this.panel7.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1248, 736);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1248, 736);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.BackColor = System.Drawing.Color.White;
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.panel10);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(1240, 710);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.panel6.Controls.Add(this.panel9);
		this.panel6.Controls.Add(this.panel3);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(2, 421);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1236, 287);
		this.panel6.TabIndex = 6;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdGp);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(617, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(619, 287);
		this.panel9.TabIndex = 11;
		this.grdGp.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdGp).BackColor = System.Drawing.Color.White;
		grdGp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdGp.DesignTimeLayout = grdGp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGp).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdGp.FilterMode = (FilterMode)2;
		this.grdGp.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdGp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdGp.GroupByBoxFormatStyle.BackColorGradient = System.Drawing.Color.White;
		this.grdGp.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdGp).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdGp).Name = "grdGp";
		((ScrollableBase)this.grdGp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGp).Size = new System.Drawing.Size(619, 287);
		((System.Windows.Forms.Control)(object)this.grdGp).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdGp).TabStop = false;
		this.grdGp.TotalRow = (InheritableBoolean)1;
		this.grdGp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGp).VisualStyle = (VisualStyle)4;
		this.grdGp.ColumnButtonClick += new ColumnActionEventHandler(grdGp_ColumnButtonClick);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdEmptyBags);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(617, 287);
		this.panel3.TabIndex = 10;
		((ScrollableBase)this.grdEmptyBags).BackColor = System.Drawing.Color.White;
		this.grdEmptyBags.ColumnAutoResize = true;
		grdEmptyBags_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdEmptyBags.DesignTimeLayout = grdEmptyBags_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdEmptyBags.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Name = "grdEmptyBags";
		((ScrollableBase)this.grdEmptyBags).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Size = new System.Drawing.Size(617, 266);
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).TabIndex = 0;
		this.grdEmptyBags.TotalRow = (InheritableBoolean)1;
		this.grdEmptyBags.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEmptyBags).VisualStyle = (VisualStyle)4;
		this.grdEmptyBags.ColumnButtonClick += new ColumnActionEventHandler(grdEmptyBags_ColumnButtonClick);
		this.panel10.Controls.Add(this.groupBox1);
		this.panel10.Controls.Add(this.groupBox3);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(2, 323);
		this.panel10.Margin = new System.Windows.Forms.Padding(2);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1236, 98);
		this.panel10.TabIndex = 5;
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add(this.textBox5);
		this.groupBox1.Controls.Add(this.button4);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.ultraCombo2);
		this.groupBox1.Controls.Add(this.button5);
		this.groupBox1.Controls.Add(this.button6);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.textBox3);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.textBox4);
		this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 10.2f);
		this.groupBox1.Location = new System.Drawing.Point(621, 15);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(607, 81);
		this.groupBox1.TabIndex = 57;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Packing Material OwnerShip";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(218, 27);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(78, 15);
		this.label13.TabIndex = 58;
		this.label13.Text = "Recieved Qty";
		this.textBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.textBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox5.Location = new System.Drawing.Point(221, 50);
		this.textBox5.Name = "textBox5";
		this.textBox5.Size = new System.Drawing.Size(97, 21);
		this.textBox5.TabIndex = 57;
		this.button4.BackColor = System.Drawing.Color.White;
		this.button4.FlatAppearance.BorderSize = 0;
		this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.button4.Location = new System.Drawing.Point(541, 46);
		this.button4.Name = "button4";
		this.button4.Size = new System.Drawing.Size(65, 26);
		this.button4.TabIndex = 55;
		this.button4.Text = "Cancel";
		this.button4.UseVisualStyleBackColor = false;
		this.button4.Visible = false;
		this.ultraCombo2.AllowNull = (DefaultableBoolean)1;
		this.ultraCombo2.AutoCompleteMode = (AutoCompleteMode)4;
		this.ultraCombo2.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.ultraCombo2.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Appearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo2).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance8;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo2).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance14).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).TextHAlignAsString = "Left";
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.ultraCombo2).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ultraCombo2.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.ultraCombo2).Location = new System.Drawing.Point(6, 48);
		((System.Windows.Forms.Control)(object)this.ultraCombo2).Name = "ultraCombo2";
		((System.Windows.Forms.Control)(object)this.ultraCombo2).Size = new System.Drawing.Size(109, 24);
		((System.Windows.Forms.Control)(object)this.ultraCombo2).TabIndex = 19;
		this.button5.BackColor = System.Drawing.Color.White;
		this.button5.FlatAppearance.BorderSize = 0;
		this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.button5.Location = new System.Drawing.Point(473, 46);
		this.button5.Name = "button5";
		this.button5.Size = new System.Drawing.Size(65, 26);
		this.button5.TabIndex = 54;
		this.button5.Text = "Update";
		this.button5.UseVisualStyleBackColor = false;
		this.button5.Visible = false;
		this.button6.BackColor = System.Drawing.Color.White;
		this.button6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.button6.Location = new System.Drawing.Point(437, 47);
		this.button6.Margin = new System.Windows.Forms.Padding(2);
		this.button6.Name = "button6";
		this.button6.Size = new System.Drawing.Size(29, 25);
		this.button6.TabIndex = 53;
		this.button6.Text = "+";
		this.button6.UseVisualStyleBackColor = false;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(6, 28);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(33, 15);
		this.label7.TabIndex = 20;
		this.label7.Text = "Type";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(118, 28);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(78, 15);
		this.label9.TabIndex = 56;
		this.label9.Text = "Packing item";
		this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox3.Location = new System.Drawing.Point(121, 50);
		this.textBox3.Name = "textBox3";
		this.textBox3.Size = new System.Drawing.Size(94, 21);
		this.textBox3.TabIndex = 55;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(321, 28);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(86, 15);
		this.label10.TabIndex = 54;
		this.label10.Text = "Purchased Qty";
		this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.textBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox4.Location = new System.Drawing.Point(324, 50);
		this.textBox4.Name = "textBox4";
		this.textBox4.Size = new System.Drawing.Size(104, 21);
		this.textBox4.TabIndex = 53;
		this.groupBox3.Controls.Add(this.button1);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.ultraCombo1);
		this.groupBox3.Controls.Add(this.button2);
		this.groupBox3.Controls.Add(this.button3);
		this.groupBox3.Controls.Add(this.label6);
		this.groupBox3.Controls.Add(this.label5);
		this.groupBox3.Controls.Add(this.textBox2);
		this.groupBox3.Controls.Add(this.label4);
		this.groupBox3.Controls.Add(this.textBox1);
		this.groupBox3.Font = new System.Drawing.Font("Segoe UI", 10.2f);
		this.groupBox3.Location = new System.Drawing.Point(10, 13);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(574, 81);
		this.groupBox3.TabIndex = 52;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Contractor";
		this.button1.BackColor = System.Drawing.Color.White;
		this.button1.FlatAppearance.BorderSize = 0;
		this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.button1.Location = new System.Drawing.Point(487, 46);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(65, 26);
		this.button1.TabIndex = 55;
		this.button1.Text = "Cancel";
		this.button1.UseVisualStyleBackColor = false;
		this.button1.Visible = false;
		this.ultraCombo1.AllowNull = (DefaultableBoolean)1;
		this.ultraCombo1.AutoCompleteMode = (AutoCompleteMode)4;
		this.ultraCombo1.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.ultraCombo1.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance86;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance97).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance97).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance45).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).TextHAlignAsString = "Left";
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ultraCombo1.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Location = new System.Drawing.Point(6, 48);
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Name = "ultraCombo1";
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Size = new System.Drawing.Size(177, 24);
		((System.Windows.Forms.Control)(object)this.ultraCombo1).TabIndex = 19;
		this.button2.BackColor = System.Drawing.Color.White;
		this.button2.FlatAppearance.BorderSize = 0;
		this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.button2.Location = new System.Drawing.Point(419, 46);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(65, 26);
		this.button2.TabIndex = 54;
		this.button2.Text = "Update";
		this.button2.UseVisualStyleBackColor = false;
		this.button2.Visible = false;
		this.button3.BackColor = System.Drawing.Color.White;
		this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.button3.Location = new System.Drawing.Point(389, 47);
		this.button3.Margin = new System.Windows.Forms.Padding(2);
		this.button3.Name = "button3";
		this.button3.Size = new System.Drawing.Size(29, 25);
		this.button3.TabIndex = 53;
		this.button3.Text = "+";
		this.button3.UseVisualStyleBackColor = false;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(6, 28);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(63, 15);
		this.label6.TabIndex = 20;
		this.label6.Text = "Contractor";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(186, 28);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(61, 15);
		this.label5.TabIndex = 56;
		this.label5.Text = "Pack Size";
		this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox2.Location = new System.Drawing.Point(189, 51);
		this.textBox2.Name = "textBox2";
		this.textBox2.Size = new System.Drawing.Size(94, 21);
		this.textBox2.TabIndex = 55;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(286, 28);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(24, 15);
		this.label4.TabIndex = 54;
		this.label4.Text = "Qty";
		this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox1.Location = new System.Drawing.Point(289, 51);
		this.textBox1.Name = "textBox1";
		this.textBox1.Size = new System.Drawing.Size(92, 21);
		this.textBox1.TabIndex = 53;
		this.panel5.Controls.Add(this.panel13);
		this.panel5.Controls.Add(this.panel12);
		this.panel5.Controls.Add(this.label16);
		this.panel5.Controls.Add(this.txtEbWtOther);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(2, 83);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1236, 240);
		this.panel5.TabIndex = 1;
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel13.Location = new System.Drawing.Point(3, 109);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1233, 129);
		this.panel13.TabIndex = 59;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)1;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxFormatStyle.BackColorGradient = System.Drawing.Color.White;
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1233, 129);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdDetail).TabStop = false;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).DoubleClick += new System.EventHandler(grd_DoubleClick);
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUom);
		this.panel12.Controls.Add(this.txtQty);
		this.panel12.Controls.Add(this.label30);
		this.panel12.Controls.Add(this.label11);
		this.panel12.Controls.Add(this.Add);
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupplier);
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackingtype);
		this.panel12.Controls.Add(this.label2);
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWareHouse);
		this.panel12.Controls.Add(this.txtManualReportNo);
		this.panel12.Controls.Add(this.label42);
		this.panel12.Controls.Add(this.label3);
		this.panel12.Controls.Add(this.label17);
		this.panel12.Controls.Add(this.txtCommentsDetail);
		this.panel12.Controls.Add(this.DocDate);
		this.panel12.Controls.Add(this.label41);
		this.panel12.Controls.Add(this.btnCancelUpdateDetial);
		this.panel12.Controls.Add(this.label40);
		this.panel12.Controls.Add(this.label36);
		this.panel12.Controls.Add(this.btnUpdateDetail);
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItem);
		this.panel12.Location = new System.Drawing.Point(1, 1);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1235, 109);
		this.panel12.TabIndex = 58;
		this.CmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance55;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance3).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Location = new System.Drawing.Point(638, 28);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Name = "CmbPackUom";
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Size = new System.Drawing.Size(121, 24);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Leave += new System.EventHandler(combpckuom_Leave);
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQty.Location = new System.Drawing.Point(763, 30);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(94, 21);
		this.txtQty.TabIndex = 6;
		this.txtQty.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.txtQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(992, 7);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(70, 15);
		this.label30.TabIndex = 41;
		this.label30.Text = "Warehouse";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(239, 7);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(87, 15);
		this.label11.TabIndex = 51;
		this.label11.Text = "Supplier Name";
		this.Add.BackColor = System.Drawing.Color.White;
		this.Add.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Add.Location = new System.Drawing.Point(357, 76);
		this.Add.Margin = new System.Windows.Forms.Padding(2);
		this.Add.Name = "Add";
		this.Add.Size = new System.Drawing.Size(29, 25);
		this.Add.TabIndex = 19;
		this.Add.Text = "+";
		this.Add.UseVisualStyleBackColor = false;
		this.Add.Click += new System.EventHandler(Add_Click);
		this.cmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance61;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance66).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance67).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Location = new System.Drawing.Point(238, 27);
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Name = "cmbSupplier";
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Size = new System.Drawing.Size(139, 26);
		((System.Windows.Forms.Control)(object)this.cmbSupplier).TabIndex = 50;
		this.CmbPackingtype.AllowNull = (DefaultableBoolean)1;
		this.CmbPackingtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackingtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackingtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance90;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance95).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance96).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance98).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackingtype).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackingtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackingtype).Location = new System.Drawing.Point(863, 28);
		((System.Windows.Forms.Control)(object)this.CmbPackingtype).Name = "CmbPackingtype";
		((System.Windows.Forms.Control)(object)this.CmbPackingtype).Size = new System.Drawing.Size(121, 24);
		((System.Windows.Forms.Control)(object)this.CmbPackingtype).TabIndex = 4;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(12, 7);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(108, 15);
		this.label2.TabIndex = 49;
		this.label2.Text = "Manual Report No";
		this.CmbWareHouse.AllowNull = (DefaultableBoolean)1;
		this.CmbWareHouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWareHouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWareHouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance77;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance82).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance83).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWareHouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Location = new System.Drawing.Point(990, 28);
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Name = "CmbWareHouse";
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Size = new System.Drawing.Size(238, 24);
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).TabIndex = 15;
		this.txtManualReportNo.BackColor = System.Drawing.Color.White;
		this.txtManualReportNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtManualReportNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtManualReportNo.Location = new System.Drawing.Point(7, 30);
		this.txtManualReportNo.Name = "txtManualReportNo";
		this.txtManualReportNo.ReadOnly = true;
		this.txtManualReportNo.Size = new System.Drawing.Size(118, 21);
		this.txtManualReportNo.TabIndex = 48;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(639, 7);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(66, 15);
		this.label42.TabIndex = 21;
		this.label42.Text = "Pack UOM";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(6, 56);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(67, 15);
		this.label3.TabIndex = 47;
		this.label3.Text = "Comments";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(141, 7);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(33, 15);
		this.label17.TabIndex = 3;
		this.label17.Text = "Date";
		this.txtCommentsDetail.Location = new System.Drawing.Point(9, 78);
		this.txtCommentsDetail.Name = "txtCommentsDetail";
		this.txtCommentsDetail.Size = new System.Drawing.Size(341, 20);
		this.txtCommentsDetail.TabIndex = 18;
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(137, 30);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(95, 21);
		this.DocDate.TabIndex = 3;
		this.DocDate.TabStop = false;
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(767, 7);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(24, 15);
		this.label41.TabIndex = 19;
		this.label41.Text = "Qty";
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(455, 75);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(65, 26);
		this.btnCancelUpdateDetial.TabIndex = 21;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(866, 7);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(80, 15);
		this.label40.TabIndex = 17;
		this.label40.Text = "Packing Type";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(386, 7);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(68, 15);
		this.label36.TabIndex = 8;
		this.label36.Text = "Item Name";
		this.btnUpdateDetail.BackColor = System.Drawing.Color.White;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.Location = new System.Drawing.Point(387, 75);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(65, 26);
		this.btnUpdateDetail.TabIndex = 20;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.CmbItem.AllowNull = (DefaultableBoolean)1;
		this.CmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance110;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance111).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance111).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance116).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItem).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItem).Location = new System.Drawing.Point(383, 28);
		((System.Windows.Forms.Control)(object)this.CmbItem).Name = "CmbItem";
		((System.Windows.Forms.Control)(object)this.CmbItem).Size = new System.Drawing.Size(249, 24);
		((System.Windows.Forms.Control)(object)this.CmbItem).TabIndex = 1;
		this.CmbItem.ValueChanged += new System.EventHandler(combitem_Leave);
		((System.Windows.Forms.Control)(object)this.CmbItem).Leave += new System.EventHandler(combitem_Leave);
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(1285, 44);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(22, 15);
		this.label16.TabIndex = 45;
		this.label16.Text = "Eb";
		this.label16.Visible = false;
		this.txtEbWtOther.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEbWtOther.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEbWtOther.Location = new System.Drawing.Point(1282, 66);
		this.txtEbWtOther.Name = "txtEbWtOther";
		this.txtEbWtOther.Size = new System.Drawing.Size(57, 21);
		this.txtEbWtOther.TabIndex = 8;
		this.txtEbWtOther.TabStop = false;
		this.txtEbWtOther.Visible = false;
		this.txtEbWtOther.TextChanged += new System.EventHandler(txtEbWtOther_TextChanged);
		this.panel4.Controls.Add(this.ChkBox);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbGatePass);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.lblvehno);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combrefprty);
		this.panel4.Controls.Add(this.txtvehno);
		this.panel4.Controls.Add(this.lblsupwt);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add(this.label20);
		this.panel4.Controls.Add(this.txtsuppwt);
		this.panel4.Controls.Add(this.txtfctwt);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combstockprty);
		this.panel4.Controls.Add(this.label12);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 33);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1236, 50);
		this.panel4.TabIndex = 0;
		this.ChkBox.AutoSize = true;
		this.ChkBox.Location = new System.Drawing.Point(998, 16);
		this.ChkBox.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(47, 17);
		this.ChkBox.TabIndex = 18;
		this.ChkBox.Text = "Print";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.CmbGatePass.AllowNull = (DefaultableBoolean)1;
		this.CmbGatePass.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbGatePass.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbGatePass.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbGatePass).DisplayLayout.Appearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbGatePass).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbGatePass).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance51).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance51).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbGatePass).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGatePass).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance52;
		((SpecialBoxBase)((UltraGridBase)this.CmbGatePass).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGatePass).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbGatePass).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbGatePass).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbGatePass).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbGatePass).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbGatePass).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbGatePass).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGatePass).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance102).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbGatePass).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbGatePass).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbGatePass).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance103).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance103).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance103).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGatePass).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbGatePass).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbGatePass).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbGatePass).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbGatePass).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbGatePass).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbGatePass).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbGatePass).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbGatePass).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbGatePass).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbGatePass).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbGatePass.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbGatePass).Location = new System.Drawing.Point(95, 12);
		((System.Windows.Forms.Control)(object)this.CmbGatePass).Name = "CmbGatePass";
		((System.Windows.Forms.Control)(object)this.CmbGatePass).Size = new System.Drawing.Size(200, 24);
		((System.Windows.Forms.Control)(object)this.CmbGatePass).TabIndex = 8;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(10, 17);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(70, 15);
		this.label1.TabIndex = 8;
		this.label1.Text = "Gate Pass#";
		this.lblvehno.AutoSize = true;
		this.lblvehno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblvehno.Location = new System.Drawing.Point(308, 17);
		this.lblvehno.Name = "lblvehno";
		this.lblvehno.Size = new System.Drawing.Size(66, 15);
		this.lblvehno.TabIndex = 0;
		this.lblvehno.Text = "Vehicle No";
		this.combrefprty.AllowNull = (DefaultableBoolean)1;
		this.combrefprty.AutoCompleteMode = (AutoCompleteMode)4;
		this.combrefprty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combrefprty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combrefprty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.combrefprty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combrefprty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combrefprty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combrefprty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance21;
		((SpecialBoxBase)((UltraGridBase)this.combrefprty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combrefprty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.combrefprty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combrefprty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance27).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).TextHAlignAsString = "Left";
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.combrefprty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combrefprty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combrefprty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combrefprty).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combrefprty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combrefprty).Location = new System.Drawing.Point(384, 190);
		((System.Windows.Forms.Control)(object)this.combrefprty).Name = "combrefprty";
		((System.Windows.Forms.Control)(object)this.combrefprty).Size = new System.Drawing.Size(201, 24);
		((System.Windows.Forms.Control)(object)this.combrefprty).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.combrefprty).TabStop = false;
		((System.Windows.Forms.Control)(object)this.combrefprty).Visible = false;
		this.txtvehno.BackColor = System.Drawing.SystemColors.Control;
		this.txtvehno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvehno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtvehno.Location = new System.Drawing.Point(387, 14);
		this.txtvehno.Name = "txtvehno";
		this.txtvehno.ReadOnly = true;
		this.txtvehno.Size = new System.Drawing.Size(131, 21);
		this.txtvehno.TabIndex = 11;
		this.txtvehno.TabStop = false;
		this.lblsupwt.AutoSize = true;
		this.lblsupwt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblsupwt.Location = new System.Drawing.Point(524, 17);
		this.lblsupwt.Name = "lblsupwt";
		this.lblsupwt.Size = new System.Drawing.Size(77, 15);
		this.lblsupwt.TabIndex = 0;
		this.lblsupwt.Text = "Supp Weight";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(283, 167);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(67, 15);
		this.label8.TabIndex = 3;
		this.label8.Text = "Stock Party";
		this.label8.Visible = false;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(753, 17);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(71, 15);
		this.label20.TabIndex = 3;
		this.label20.Text = "Fact Weight";
		this.txtsuppwt.BackColor = System.Drawing.SystemColors.Control;
		this.txtsuppwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsuppwt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtsuppwt.Location = new System.Drawing.Point(612, 14);
		this.txtsuppwt.Name = "txtsuppwt";
		this.txtsuppwt.ReadOnly = true;
		this.txtsuppwt.Size = new System.Drawing.Size(135, 21);
		this.txtsuppwt.TabIndex = 13;
		this.txtsuppwt.TabStop = false;
		this.txtsuppwt.TextChanged += new System.EventHandler(txtsuppwt_TextChanged);
		this.txtfctwt.BackColor = System.Drawing.SystemColors.Control;
		this.txtfctwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfctwt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfctwt.Location = new System.Drawing.Point(830, 14);
		this.txtfctwt.Name = "txtfctwt";
		this.txtfctwt.ReadOnly = true;
		this.txtfctwt.Size = new System.Drawing.Size(139, 21);
		this.txtfctwt.TabIndex = 14;
		this.txtfctwt.TabStop = false;
		this.txtfctwt.TextChanged += new System.EventHandler(txtfctwt_TextChanged);
		this.combstockprty.AllowNull = (DefaultableBoolean)1;
		this.combstockprty.AutoCompleteMode = (AutoCompleteMode)4;
		this.combstockprty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combstockprty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combstockprty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.combstockprty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combstockprty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combstockprty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combstockprty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance34;
		((SpecialBoxBase)((UltraGridBase)this.combstockprty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combstockprty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.combstockprty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combstockprty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance40).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).TextHAlignAsString = "Left";
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.combstockprty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combstockprty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combstockprty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combstockprty).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combstockprty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combstockprty).Location = new System.Drawing.Point(384, 163);
		((System.Windows.Forms.Control)(object)this.combstockprty).Name = "combstockprty";
		((System.Windows.Forms.Control)(object)this.combstockprty).Size = new System.Drawing.Size(201, 24);
		((System.Windows.Forms.Control)(object)this.combstockprty).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.combstockprty).TabStop = false;
		((System.Windows.Forms.Control)(object)this.combstockprty).Visible = false;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(283, 194);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(56, 15);
		this.label12.TabIndex = 0;
		this.label12.Text = "Ref Party";
		this.label12.Visible = false;
		this.panel2.Controls.Add(this.ctrlGrdBar3);
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1236, 31);
		this.panel2.TabIndex = 0;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1187, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdGp;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(45, 29);
		this.ctrlGrdBar3.TabIndex = 23;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1128, -1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdDetail;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 29);
		this.ctrlGrdBar1.TabIndex = 22;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.lblFormCaption, this.btnNew, this.btnUpdate, this.btnSave, this.printToolStripButton, this.btnAttachment, this.toolStripButton1, this.CityDefine, this.btnDelete });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1236, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.lblFormCaption.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFormCaption.Name = "lblFormCaption";
		this.lblFormCaption.Size = new System.Drawing.Size(113, 24);
		this.lblFormCaption.Text = "Working Report";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 24);
		this.btnNew.Text = "&New";
		this.btnNew.ToolTipText = "Ctrl_N";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(69, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.ToolTipText = "Ctrl_U";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(55, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.ToolTipText = "Ctrl_S";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.printToolStripButton.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.printToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.printToolStripButton.Name = "printToolStripButton";
		this.printToolStripButton.Size = new System.Drawing.Size(79, 24);
		this.printToolStripButton.Text = "333-Print";
		this.printToolStripButton.ToolTipText = "Ctrl_P";
		this.printToolStripButton.Click += new System.EventHandler(printToolStripButton_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(94, 24);
		this.btnAttachment.Text = "&Attechment";
		this.btnAttachment.ToolTipText = "Alt_A";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(50, 24);
		this.toolStripButton1.Text = "&Refresh";
		this.toolStripButton1.ToolTipText = "Alt_R";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.CityDefine.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.CityDefine.Name = "CityDefine";
		this.CityDefine.Size = new System.Drawing.Size(32, 24);
		this.CityDefine.Text = "City";
		this.CityDefine.Click += new System.EventHandler(CityDefine_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.icons8_cut_16;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(64, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.tabPage2.Controls.Add(this.panel11);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(1240, 710);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistoryDetail);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(2, 384);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1236, 324);
		this.panel11.TabIndex = 2;
		this.GrdHistoryDetail.AllowEdit = (InheritableBoolean)2;
		GrdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistoryDetail.DesignTimeLayout = GrdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdHistoryDetail.DynamicFiltering = true;
		this.GrdHistoryDetail.FilterMode = (FilterMode)2;
		this.GrdHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Name = "GrdHistoryDetail";
		((ScrollableBase)this.GrdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Size = new System.Drawing.Size(1236, 324);
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).TabIndex = 45;
		this.GrdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.GrdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistory);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(2, 30);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1236, 354);
		this.panel8.TabIndex = 1;
		this.GrdHistory.AllowEdit = (InheritableBoolean)2;
		GrdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistory.DesignTimeLayout = GrdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdHistory.DynamicFiltering = true;
		this.GrdHistory.FilterMode = (FilterMode)2;
		this.GrdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GrdHistory).Name = "GrdHistory";
		((ScrollableBase)this.GrdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Size = new System.Drawing.Size(1236, 354);
		((System.Windows.Forms.Control)(object)this.GrdHistory).TabIndex = 44;
		this.GrdHistory.TotalRow = (InheritableBoolean)1;
		this.GrdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistory).VisualStyle = (VisualStyle)4;
		this.GrdHistory.ColumnButtonClick += new ColumnActionEventHandler(GrdHistory_ColumnButtonClick);
		this.GrdHistory.LinkClicked += new ColumnActionEventHandler(GrdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdHistory).DoubleClick += new System.EventHandler(GrdHistory_DoubleClick);
		this.panel7.Controls.Add(this.ctrlGrdBar2);
		this.panel7.Controls.Add(this.toolStrip2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(2, 2);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1236, 28);
		this.panel7.TabIndex = 0;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1187, -3);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.GrdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 27);
		this.ctrlGrdBar2.TabIndex = 23;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnLoadAll, this.btnGrnFormHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1236, 27);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.btnLoadAll.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(71, 24);
		this.btnLoadAll.Text = "&LoadAll";
		this.btnLoadAll.ToolTipText = "Ctrl_L";
		this.btnLoadAll.Click += new System.EventHandler(btnLoadAll_Click);
		this.btnGrnFormHistory.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnGrnFormHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnGrnFormHistory.Name = "btnGrnFormHistory";
		this.btnGrnFormHistory.Size = new System.Drawing.Size(73, 24);
		this.btnGrnFormHistory.Text = "Register";
		this.btnGrnFormHistory.ToolTipText = "Crtl_H";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1248, 736);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "WorkingReport";
		this.Text = "   Working Report";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvFrmGRN_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdGp).EndInit();
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdEmptyBags).EndInit();
		this.panel10.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo2).EndInit();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo1).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel13.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingtype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbGatePass).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combrefprty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combstockprty).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryDetail).EndInit();
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
