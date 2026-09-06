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
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Inventory_Reports;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Paddy_Purchase;

public class frmGrnForGate : Form
{
	private Attachment AT = new Attachment();

	private int GPID;

	private int updateDetailIndex;

	private int Id;

	private bool UpdateMode;

	private double JuteBagsCut;

	private double PPBagsCut;

	private double OpenBulkCut;

	private bool EmptyBagsWeightCutEditable;

	private bool LabCompulsoryForWeighBridgeAgainstGatePurchase;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private DataTable dtdetail = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtItem = new DataTable();

	private DataTable dtjob = new DataTable();

	private DataTable dtPack = new DataTable();

	private DataTable dtUOM = new DataTable();

	private DataTable dtwh = new DataTable();

	private DataTable dtcity = new DataTable();

	private DataTable dtpo = new DataTable();

	private DataTable dtCrop = new DataTable();

	private DataTable dtEmptyBag = new DataTable();

	private DataTable dtEmpType = new DataTable();

	private DataTable dtBagsCondition = new DataTable();

	private DataTable dtLabInfo = new DataTable();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

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

	private Label label24;

	private Label label25;

	private Label label21;

	private Label label19;

	private TextBox txtbltyno;

	private Label label7;

	private TextBox txtvehno;

	internal UltraCombo CmbTransport;

	private Label label48;

	private Label label26;

	private TextBox txtcarramount;

	private Label label20;

	private TextBox txtsuppwt;

	private Label label6;

	private TextBox txtfctwt;

	internal UltraCombo combsupplier;

	private Label label2;

	private Label label13;

	private Label label18;

	private DateTimePicker DocDate;

	private Label label17;

	private TextBox txtdocno;

	private Label label5;

	internal UltraCombo combpckuom;

	internal UltraCombo CmbCity;

	internal UltraCombo combwr;

	internal UltraCombo combpcktyp;

	internal UltraCombo combjoblot;

	internal UltraCombo combitem;

	private Label label30;

	private Label label27;

	private Label label22;

	private TextBox txtebu;

	private TextBox txtgwt;

	private TextBox txtqty;

	private TextBox txtnetwt;

	private Label label46;

	private Label label28;

	private Label label45;

	private Label label44;

	private Label label43;

	private Label label42;

	private Label label31;

	private Label label41;

	private Label label40;

	private Label label36;

	private Panel panel7;

	private Panel panel8;

	internal GridEX GrdHistory;

	private CtrlGrdBar ctrlGrdBar1;

	internal UltraCombo combcropyear;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private ToolStripButton toolStripButton1;

	private TextBox combgatepass;

	private Panel panel10;

	internal GridEX grd;

	private CheckBox ChkBox;

	private TextBox txtEmptyBagsTotal;

	private Panel panel6;

	internal GridEX grdGp;

	private ToolStripButton CityDefine;

	internal UltraCombo combbrnch;

	internal UltraCombo combproject;

	private Button Add;

	private Panel panel11;

	internal GridEX GrdHistoryDetail;

	private CtrlGrdBar ctrlGrdBar2;

	private ToolStrip toolStrip2;

	private ToolStripButton btnLoadAll;

	internal UltraCombo combvehtyp;

	private ToolStripButton btnDelete;

	private TextBox txtLabNo;

	private ToolStripLabel toolStripLabel1;

	internal GridEX grdEmptyBags;

	private Panel panel9;

	private Panel panel3;

	private CtrlGrdBar ctrlGrdBar3;

	private ToolStripButton btnGrnFormHistory;

	private CtrlGrdBar ctrlGrdBar4;

	internal UltraCombo CmbContractor;

	private Label label1;

	private RichTextBox txtremarks;

	internal UltraCombo CmbStatus;

	private Label label3;

	private Label label4;

	private TextBox txtScaleKart;

	private Label label8;

	private TextBox txtScaleKartDetail;

	public frmGrnForGate(UserAccount user)
	{
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void Add_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				double num = Conversion.ToDouble((object)txtnetwt.Text.Trim());
				double GrossWeight = Conversion.ToDouble((object)txtgwt.Text.Trim());
				if (num > GrossWeight)
				{
					throw new Exception("NetBillWeight cannot greater than Gross Weight Please check");
				}
				dtdetail.Rows.Add(combitem.Value, ((Control)(object)combitem).Text, ((Control)(object)combcropyear).Text, combjoblot.Value, ((Control)(object)combjoblot).Text, combpcktyp.Value, ((Control)(object)combpcktyp).Text, ((Control)(object)combpckuom).Text.Trim(), combpckuom.Value, Conversion.ToDouble((object)txtqty.Text.Trim()), Conversion.ToDouble((object)txtgwt.Text.Trim()), Conversion.ToDouble((object)txtScaleKartDetail.Text.Trim()), Conversion.ToDouble((object)txtebu.Text.Trim()), Conversion.ToDouble((object)txtEmptyBagsTotal.Text.Trim()), Conversion.ToDouble((object)txtnetwt.Text.Trim()), combwr.Value, txtLabNo.Text, ((Control)(object)CmbCity).Text, CmbContractor.Value, ((Control)(object)CmbContractor).Text.Trim());
				grd.DataSource = dtdetail;
				grd.RetrieveStructure();
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
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Expected O, but got Unknown
		//IL_0932: Unknown result type (might be due to invalid IL or missing references)
		//IL_0939: Expected O, but got Unknown
		//IL_09f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fb: Expected O, but got Unknown
		//IL_075b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0762: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			if (grd.GetRows().Count() == 0)
			{
				MessageBox.Show("Grid Record Not Found");
				return;
			}
			InvGrn GRN = new InvGrn();
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				GRN.Id = Id;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			GRN.DocumentTypeId = 165;
			GRN.DocDate = DocDate.Value;
			GRN.DocNo = Convert.ToInt32(txtdocno.Text.Trim());
			GRN.SupplierCustomerId = Conversion.ToInt(combsupplier.Value);
			GRN.GpNo = Conversion.ToInt((object)combgatepass.Text);
			GRN.PartyWeight = Conversion.ToDouble((object)txtsuppwt.Text.Trim());
			GRN.FactoryWeight = Conversion.ToDouble((object)txtfctwt.Text.Trim());
			GRN.ScaleKart = Conversion.ToDouble((object)txtScaleKart.Text.Trim());
			GRN.VehicleType = Conversion.ToString((object)((Control)(object)combvehtyp).Text);
			GRN.VehicleNo = Conversion.ToString((object)txtvehno.Text.Trim());
			GRN.DeliveryTerm = "Ponch & FactoryWeight";
			GRN.BiltyNo = txtbltyno.Text.Trim();
			GRN.Status = Conversion.ToString((object)((Control)(object)CmbStatus).Text);
			if (Conversion.ToDouble((object)txtcarramount.Text.Trim()) > 0.0)
			{
				if (((UltraGridBase)CmbTransport).ActiveRow == null)
				{
					MessageBox.Show("Transporter Account field required");
					CmbTransport.Focus();
					return;
				}
				GRN.TransporterId = Conversion.ToInt(CmbTransport.Value);
				GRN.CarriageAmount = Conversion.ToDouble((object)txtcarramount.Text.Trim());
			}
			GRN.RemarksHeader = txtremarks.Text.Trim();
			GRN.BranchesId = Conversion.ToInt(combbrnch.Value);
			GRN.ProjectsId = Conversion.ToInt(combproject.Value);
			GRN.InwardGatePassId = Conversion.ToInt((object)GPID);
			GRN.OrganizationId = UserAccount.OrganizationId;
			GRN.CompanyId = UserAccount.CompanyId;
			GRN.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			GRN.EntryDate = DateTime.Now;
			GRN.ModifyDate = DateTime.Now;
			GRN.EntryUser = UserAccount.ID;
			GRN.ModifyUser = UserAccount.ID;
			GRN.invGrnDetail = new List<InvGrnDetail>();
			GRN.InvgrnDetailEmptyBagslist = new List<InvgrnDetailEmptyBags>();
			double GrnQty = 0.0;
			double GrossWeight = 0.0;
			double EmptyBagsRecQty = 0.0;
			double EmptyBagsPurQty = 0.0;
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				InvGrnDetail grnDetail = new InvGrnDetail();
				grnDetail.ContractorId = Conversion.ToInt(r2.Cells["ContractorId"].Value);
				grnDetail.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
				grnDetail.CropYear = Conversion.ToString(r2.Cells["CropYear"].Value);
				grnDetail.JobLotId = Conversion.ToInt(r2.Cells["JobId"].Value);
				grnDetail.PackingTypeId = Conversion.ToInt(r2.Cells["PackingTypeId"].Value);
				grnDetail.ItemQty = Conversion.ToDouble(r2.Cells["Qty"].Value);
				GrnQty += grnDetail.ItemQty;
				grnDetail.ItemUomId = Conversion.ToInt(r2.Cells["UOMId"].Value);
				grnDetail.GrossWeight = Conversion.ToDouble(r2.Cells["GrossWeight"].Value);
				GrossWeight += grnDetail.GrossWeight;
				grnDetail.ScaleKart = Conversion.ToDouble(r2.Cells["ScaleKart"].Value);
				grnDetail.EBWPerUnit = Conversion.ToDouble(r2.Cells["EbUnit"].Value);
				grnDetail.EBWTotal = Conversion.ToDouble(r2.Cells["EbTotal"].Value);
				grnDetail.NetBillWeight = Conversion.ToDouble(r2.Cells["NetWeight"].Value);
				grnDetail.StockWeight = grnDetail.NetBillWeight;
				grnDetail.WarehouseId = Conversion.ToInt(r2.Cells["WareHouse"].Value);
				grnDetail.LabReportRef = Conversion.ToString((object)r2.Cells["LabNo"].Text);
				grnDetail.AreaCity = Conversion.ToString((object)r2.Cells["City"].Text);
				GRN.invGrnDetail.Add(grnDetail);
			}
			double FactoryWeight = Conversion.ToDouble((object)txtfctwt.Text.Trim());
			if (GrossWeight != FactoryWeight)
			{
				throw new Exception("GrossWeight and Factory Weight Not Match");
			}
			if (grdEmptyBags.RowCount > 0)
			{
				int EBStatus = 0;
				rows = grdEmptyBags.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (Conversion.ToInt(item.Cells["Type"].Value) > 0 && Conversion.ToInt(item.Cells["ItemId"].Value) > 0)
					{
						if (Conversion.ToInt(item.Cells["ItemId"].Value) == 0)
						{
							MessageBox.Show("Please Select Item First");
							return;
						}
						if (Conversion.ToInt(item.Cells["Type"].Value) == 0)
						{
							MessageBox.Show("Please Select Type First");
							return;
						}
						if (Conversion.ToInt(item.Cells["Condition"].Value) == 0)
						{
							MessageBox.Show("Please Select Bags_Condition First");
							return;
						}
						EmptyBagsRecQty += Conversion.ToDouble(item.Cells["RecQty"].Value);
						EmptyBagsPurQty += Conversion.ToDouble(item.Cells["PurQty"].Value);
					}
				}
				double EmpBagsQty = EmptyBagsRecQty + EmptyBagsPurQty;
				if (GrnQty != EmpBagsQty)
				{
					throw new Exception("Empty Bags Quantity must be equal to GrnQty");
				}
				rows = grdEmptyBags.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.ToInt(r.Cells["ItemId"].Value) <= 0 || Conversion.ToInt(r.Cells["Type"].Value) <= 0)
					{
						continue;
					}
					InvgrnDetailEmptyBags obj = new InvgrnDetailEmptyBags();
					obj.PurchaseOrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
					obj.TypeId = Conversion.ToInt(r.Cells["Type"].Value);
					obj.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					obj.BagsCondition = Conversion.ToInt(r.Cells["Condition"].Value);
					obj.ReceivedQty = Conversion.ToDouble(r.Cells["RecQty"].Value);
					obj.PurchaseQty = Conversion.ToDouble(r.Cells["PurQty"].Value);
					obj.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
					EBStatus = obj.TypeId;
					if (obj.ReceivedQty > 0.0 || obj.PurchaseQty > 0.0)
					{
						GRN.InvgrnDetailEmptyBagslist.Add(obj);
					}
					switch (EBStatus)
					{
					case 1:
						if (EmptyBagsRecQty == 0.0 && EmptyBagsPurQty == 0.0)
						{
							throw new Exception("Received Qty Or Purchase Qty Required in EmptyBags grid");
						}
						break;
					case 2:
					case 3:
						if (EmptyBagsPurQty == 0.0)
						{
							throw new Exception("Purchase Qty Required in EmptyBags grid");
						}
						break;
					case 4:
					case 5:
						if (EmptyBagsRecQty == 0.0)
						{
							throw new Exception("Received Qty Required in EmptyBags grid");
						}
						break;
					}
					if (EBStatus > 0)
					{
						double Ebqty = EmptyBagsRecQty + EmptyBagsPurQty;
						if (GrnQty != Ebqty)
						{
							throw new Exception("Empty Bags Quantity must be equal to GrnQty");
						}
					}
				}
			}
			GRN.AttachmentsList = new List<DMSAttachments>();
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Attachment.dtAttachments.Rows[i][1].ToString();
				attach.UploadedFileCustomName = Attachment.dtAttachments.Rows[i]["CustomFileName"].ToString();
				attach.ScreenName = base.Name;
				GRN.AttachmentsList.Add(attach);
			}
			GRN.AttachmentsValues = AttachmentsValues;
			GRN.CustomAttachmentsValues = CustomAttachmentsValues;
			if (AT.RemovedAttachmentListInUpdateCase.Count > 0)
			{
				GRN.DeleteAttachmentsList = new List<DMSAttachments>();
				for (int j = 0; j < AT.RemovedAttachmentListInUpdateCase.Count; j++)
				{
					DMSAttachments delete = new DMSAttachments();
					delete.Attachment = Path.GetFileName(AT.RemovedAttachmentListInUpdateCase[j].Path);
					delete.ScreenName = base.Name;
					GRN.DeleteAttachmentsList.Add(delete);
				}
			}
			int success = InvGrn.Save(GRN);
			if (Id > 0)
			{
				MessageBox.Show("Record Update Successfully [" + GRN.DocNo + "]");
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && Attachment.dtAttachments.Rows.Count == 0)
				{
					DMSAttachments.RemoveByIdAndNames(Id, base.Name, 165, 0);
				}
			}
			else
			{
				MessageBox.Show("Record Save Successfully [" + GRN.DocNo + "]");
			}
			reset();
			if (!ChkBox.Checked)
			{
				return;
			}
			try
			{
				GenerateReport(success);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		try
		{
			Id = 0;
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
			if (Id == 0)
			{
				throw new Exception("Record not update because Id not found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void ReadById_Update(int ID)
	{
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Expected O, but got Unknown
		try
		{
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			UpdateMode = true;
			Id = Conversion.ToInt((object)ID);
			InvGrn invGrn = InvGrn.GetByID(Id);
			combbrnch.Value = invGrn.BranchesId;
			combproject.Value = invGrn.ProjectsId;
			txtdocno.Text = invGrn.DocNo.ToString();
			DocDate.Value = invGrn.DocDate;
			tabControl1.SelectedIndex = 0;
			combsupplier.Value = invGrn.SupplierCustomerId;
			CmbTransport.Value = invGrn.TransporterId;
			txtcarramount.Text = invGrn.CarriageAmount.ToString();
			txtsuppwt.Text = invGrn.PartyWeight.ToString();
			txtfctwt.Text = invGrn.FactoryWeight.ToString();
			txtScaleKart.Text = Conversion.ToString((object)invGrn.ScaleKart);
			combgatepass.Text = invGrn.GpNo.ToString();
			((Control)(object)combvehtyp).Text = invGrn.VehicleType;
			txtvehno.Text = invGrn.VehicleNo;
			txtbltyno.Text = invGrn.BiltyNo;
			txtremarks.Text = invGrn.RemarksHeader;
			AttachmentsValues = invGrn.AttachmentsValues;
			CustomAttachmentsValues = invGrn.CustomAttachmentsValues;
			GPID = Conversion.ToInt((object)invGrn.InwardGatePassId);
			if (LabCompulsoryForWeighBridgeAgainstGatePurchase)
			{
				GatePassInward obj = new GatePassInward();
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.Id = GPID;
				dtLabInfo = GatePassInward.GetLabDataByGpIdForGrnGate(obj);
				if (dtLabInfo.Rows.Count > 0)
				{
					DDL.BindDDLNew(dtLabInfo, combitem, "ItemId", "ItemName", "ItemName", false);
					((UltraGridBase)combitem).DisplayLayout.Bands[0].Columns[1].Width = 500;
				}
				else
				{
					((Control)(object)combitem).Text = string.Empty;
					((UltraGridBase)combitem).DataSource = null;
				}
			}
			dtdetail.Rows.Clear();
			double GrossWeight = 0.0;
			for (int j = 0; j < invGrn.invGrnDetail.Count; j++)
			{
				dtdetail.Rows.Add(invGrn.invGrnDetail[j].ItemId, invGrn.invGrnDetail[j].Item, invGrn.invGrnDetail[j].CropYear, invGrn.invGrnDetail[j].JobLotId, invGrn.invGrnDetail[j].JobLot, invGrn.invGrnDetail[j].PackingTypeId, invGrn.invGrnDetail[j].PackingType, invGrn.invGrnDetail[j].UOM, invGrn.invGrnDetail[j].ItemUomId, invGrn.invGrnDetail[j].ItemQty, invGrn.invGrnDetail[j].GrossWeight, invGrn.invGrnDetail[j].ScaleKart, invGrn.invGrnDetail[j].EBWPerUnit, invGrn.invGrnDetail[j].EBWTotal, invGrn.invGrnDetail[j].NetBillWeight, invGrn.invGrnDetail[j].WarehouseId, invGrn.invGrnDetail[j].LabReportRef, invGrn.invGrnDetail[j].AreaCity, invGrn.invGrnDetail[j].ContractorId, invGrn.invGrnDetail[j].Contractor);
				GrossWeight += invGrn.invGrnDetail[j].GrossWeight;
			}
			dtEmptyBag.Rows.Clear();
			for (int i = 0; i < invGrn.InvgrnDetailEmptyBagslist.Count; i++)
			{
				dtEmptyBag.Rows.Add(invGrn.InvgrnDetailEmptyBagslist[i].PurchaseOrderId, invGrn.InvgrnDetailEmptyBagslist[i].TypeId, invGrn.InvgrnDetailEmptyBagslist[i].ItemId, invGrn.InvgrnDetailEmptyBagslist[i].BagsCondition, invGrn.InvgrnDetailEmptyBagslist[i].ReceivedQty, invGrn.InvgrnDetailEmptyBagslist[i].PurchaseQty, invGrn.InvgrnDetailEmptyBagslist[i].Remarks);
			}
			grd.DataSource = dtdetail;
			grd.RetrieveStructure();
			grdSettings();
			if (dtEmptyBag.Rows.Count == 0)
			{
				AddRowInvEmptyBagsGrid();
			}
			grdEmptyBags.DataSource = dtEmptyBag;
			grdEmptyBags.RetrieveStructure();
			grdEmptyBagsSettings();
			List<DMSAttachments> byID = DMSAttachments.GetByID(int.Parse(Id.ToString()), base.Name);
			AT = new Attachment();
			foreach (DMSAttachments Attachment in byID)
			{
				TempAttachments TA = new TempAttachments();
				TA.Id = 0;
				TA.ActionId = 2;
				TA.Path = Attachment.Attachment;
				TA.CustomFileName = Attachment.UploadedFileCustomName;
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
		if (txtdocno.Text.Trim() == "" || txtdocno.Text.Trim() == "0" || txtdocno.Text.Trim() == null)
		{
			MessageBox.Show("DocNo Field is Required");
			txtdocno.Focus();
			return false;
		}
		if (((UltraGridBase)combbrnch).ActiveRow == null)
		{
			MessageBox.Show("Branch Field is Required");
			combbrnch.Focus();
			return false;
		}
		if (((UltraGridBase)combproject).ActiveRow == null)
		{
			MessageBox.Show("Project Field is Required");
			combproject.Focus();
			return false;
		}
		if (((UltraGridBase)combsupplier).ActiveRow == null)
		{
			MessageBox.Show("Supplier Field is Required");
			combsupplier.Focus();
			return false;
		}
		if (Conversion.ToInt((object)combgatepass.Text.Trim()) == 0)
		{
			MessageBox.Show("Gatepass Field is Required");
			combgatepass.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtsuppwt.Text.Trim()) == 0.0 || txtsuppwt.Text.Trim() == null)
		{
			MessageBox.Show("Supplier Weight Field is Required");
			txtsuppwt.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtfctwt.Text.Trim()) == 0.0 || txtfctwt.Text.Trim() == null)
		{
			MessageBox.Show("Factory Weight Field is Required");
			txtfctwt.Focus();
			return false;
		}
		if (((UltraGridBase)CmbStatus).ActiveRow == null)
		{
			MessageBox.Show("Status Field is Required");
			CmbStatus.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)combitem).ActiveRow == null)
		{
			MessageBox.Show("Item Name Field is Required");
			combitem.Focus();
			return false;
		}
		if (((UltraGridBase)combcropyear).ActiveRow == null)
		{
			MessageBox.Show("Crop Year Field is Required");
			combcropyear.Focus();
			return false;
		}
		if (((UltraGridBase)combjoblot).ActiveRow == null)
		{
			MessageBox.Show("Job/Lot Field is Required");
			combjoblot.Focus();
			return false;
		}
		if (((UltraGridBase)combpcktyp).ActiveRow == null)
		{
			MessageBox.Show("Packing Type Field is Required");
			combpcktyp.Focus();
			return false;
		}
		if (txtqty.Text.Trim() == "" || Conversion.ToDouble((object)txtqty.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Qty Field is Required");
			txtqty.Focus();
			return false;
		}
		if (Conversion.ToInt(combpckuom.Value) == 0)
		{
			MessageBox.Show("Pack Unit Field is Required");
			combpckuom.Focus();
			return false;
		}
		if (txtgwt.Text.Trim() == "" || Conversion.ToDouble((object)txtgwt.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Gross Weight Field is Required");
			txtgwt.Focus();
			return false;
		}
		if (txtnetwt.Text.Trim() == "" || Conversion.ToDouble((object)txtnetwt.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Net Bill Weight Field is Required");
			txtnetwt.Focus();
			return false;
		}
		if (((UltraGridBase)combwr).ActiveRow == null)
		{
			MessageBox.Show("Warehouse Field is Required");
			combwr.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCity).ActiveRow == null)
		{
			MessageBox.Show("City Field is Required");
			CmbCity.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtScaleKart.Text) > 0.0 && Conversion.ToDouble((object)txtScaleKartDetail.Text) == 0.0)
		{
			MessageBox.Show("Scale Kart is Required");
			txtScaleKartDetail.Focus();
			return false;
		}
		if (((UltraGridBase)CmbContractor).ActiveRow == null)
		{
			MessageBox.Show("Contractor Field is Required");
			CmbContractor.Focus();
			return false;
		}
		return true;
	}

	private void defaultConfiquration()
	{
		try
		{
			List<ConfigrationsAllocation> lstJob = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Job/Lot").ToList();
			if (lstJob.Count > 0)
			{
				int JobId = Conversion.ToInt((object)lstJob[0].ConfigKey.ToString());
				combjoblot.Value = JobId;
			}
			List<ConfigrationsAllocation> lstPackingType = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Paking Type").ToList();
			if (lstPackingType.Count > 0)
			{
				int PackingTypeId = Conversion.ToInt((object)lstPackingType[0].ConfigKey.ToString());
				combpcktyp.Value = PackingTypeId;
			}
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "FreightInwardAc").ToList();
			if (list.Count > 0)
			{
				int TransporterId = Conversion.ToInt((object)list[0].ConfigKey.ToString());
				CmbTransport.Value = TransporterId;
			}
		}
		catch (Exception)
		{
			MessageBox.Show("Confiquration Error");
		}
	}

	private void GetConfiguration()
	{
		try
		{
			DataTable dtConfiq = CommonServices.GetMultipleConfigurationsByConfigDescriptions("WeightCutForJuteBags,WeightCutForPPBags,WeightCutForOpenBulk,EmptyBagsWeightCutEditableOnGRN,LabCompulsoryForWeighBridgeAgainstGatePurchase");
			if (dtConfiq.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtConfiq.Rows.Count; i++)
			{
				if (Conversion.ToString(dtConfiq.Rows[i]["ConfigDescription"]) == "WeightCutForJuteBags")
				{
					JuteBagsCut = Conversion.ToDouble(dtConfiq.Rows[i]["ConfigKey"]);
				}
				if (Conversion.ToString(dtConfiq.Rows[i]["ConfigDescription"]) == "WeightCutForPPBags")
				{
					PPBagsCut = Conversion.ToDouble(dtConfiq.Rows[i]["ConfigKey"]);
				}
				if (Conversion.ToString(dtConfiq.Rows[i]["ConfigDescription"]) == "WeightCutForOpenBulk")
				{
					OpenBulkCut = Conversion.ToDouble(dtConfiq.Rows[i]["ConfigKey"]);
				}
				if (Conversion.ToString(dtConfiq.Rows[i]["ConfigDescription"]) == "EmptyBagsWeightCutEditableOnGRN")
				{
					EmptyBagsWeightCutEditable = Conversion.ToBool(dtConfiq.Rows[i]["ConfigKey"]);
				}
				if (Conversion.ToString(dtConfiq.Rows[i]["ConfigDescription"]) == "LabCompulsoryForWeighBridgeAgainstGatePurchase")
				{
					LabCompulsoryForWeighBridgeAgainstGatePurchase = Conversion.ToBool(dtConfiq.Rows[i]["ConfigKey"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateCode()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		try
		{
			int DocNo = Conversion.ToInt((object)InvGrn.GenerateInvGrnCode(new InvGrn
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 165,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			}));
			txtdocno.Text = DocNo.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BranchFill()
	{
		try
		{
			DataTable dtbra = CommonServices.BrancheServiceBind();
			if (dtbra.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtbra, combbrnch, "Id", "BranchName", "Branch Name", false);
				((UltraGridBase)combbrnch).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ProjectFill()
	{
		try
		{
			DataTable dt = CommonServices.ProjectServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, combproject, "Id", "ProjectName", "Project Name", false);
			}
			((UltraGridBase)combproject).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierNameFilll()
	{
		try
		{
			DataTable dt = CommonServices.GetSupplierustomerByCustomerGroupId("1,2,3");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, combsupplier, "Id", "CompanyName", "Supplier Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ContractorBind()
	{
		try
		{
			DataTable dt = CommonServices.GetSupplierustomerByCustomerGroupId("8");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbContractor, "Id", "CompanyName", "Contractor", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TransportFill()
	{
		try
		{
			DataTable dtAccountList = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("3");
			if (dtAccountList.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtAccountList, CmbTransport, "Id", "AccountTitle", "Transport Name", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Gatepass()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		try
		{
			DataTable dtpendingGp = GatePassInward.GetPandingGatePassforInvGrnForGatePurchase(new GatePassInward
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 51,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (dtpendingGp.Rows.Count > 0)
			{
				grdGp.DataSource = dtpendingGp;
				grdGp.RetrieveStructure();
				GRNGridSetting();
			}
			else
			{
				grdGp.ClearStructure();
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
			grdGp.AllowEdit = (InheritableBoolean)2;
			grdGp.AllowDelete = (InheritableBoolean)2;
			grdGp.RootTable.Columns["Id"].Visible = false;
			grdGp.RootTable.Columns["GpSrNo"].ColumnType = (ColumnType)5;
			grdGp.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdGp.RootTable.Columns["SupplierWeight"].AggregateFunction = (AggregateFunction)2;
			grdGp.RootTable.Columns["FactoryWeight"].AggregateFunction = (AggregateFunction)2;
			grdGp.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdGp.RootTable.Columns["SupplierWeight"].TextAlignment = (TextAlignment)3;
			grdGp.RootTable.Columns["FactoryWeight"].TextAlignment = (TextAlignment)3;
			grdGp.RootTable.Columns["Qty"].FormatString = "0,0";
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

	private void VehicleTypesFill()
	{
		try
		{
			DataTable dtVehicle = VehicleType.GetAll();
			if (dtVehicle.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtVehicle, combvehtyp, "Id", "VehicleDescription", "VehicleDescription", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CropYear()
	{
		try
		{
			DataTable lst = CommonServices.CropYearGetAllService();
			if (lst.Rows.Count > 0)
			{
				DDL.BindDDLNew(lst, combcropyear, "Id", "CropYear", "Crop Year", false);
				((UltraGridBase)combcropyear).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JobLotFillWithoutOrder()
	{
		try
		{
			DataTable lst = CommonServices.JobLotGetAllService();
			if (lst.Rows.Count > 0)
			{
				DDL.BindDDLNew(lst, combjoblot, "Id", "JobLotDescription", "JobLot Description", false);
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
			if (dtpkt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("PackTypeDesc");
			for (int i = 0; i < dtpkt.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtpkt.Rows[i]["Id"]) == 1 || Conversion.ToInt(dtpkt.Rows[i]["Id"]) == 2 || Conversion.ToInt(dtpkt.Rows[i]["Id"]) == 5)
				{
					dt.Rows.Add(dtpkt.Rows[i]["Id"], dtpkt.Rows[i]["PackTypeDesc"]);
				}
			}
			DDL.BindDDL(dt, combpcktyp, "Id", "PackTypeDesc", "PackingType", ZeroIndex: false);
			((UltraGridBase)combpcktyp).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackUOMFillWithoutOrder()
	{
		try
		{
			DataTable dt = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(combitem.Value));
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, combpckuom, "Id", "Equivalent", "PackUom", ZeroIndex: false);
				((UltraGridBase)combpckuom).DisplayLayout.Bands[0].Columns[1].Hidden = true;
			}
			else
			{
				((Control)(object)combpckuom).Text = string.Empty;
				((UltraGridBase)combpckuom).DataSource = null;
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
			dtwh = CommonServices.getActiveWareHouse();
			if (dtwh.Rows.Count > 0)
			{
				DDL.BindDDL(dtwh, combwr, "Id", "WareHouseName", "Godown Name", ZeroIndex: false);
				((UltraGridBase)combwr).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CityFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		try
		{
			City val = new City
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtcityall = new DataTable();
			dtcityall = City.GetAll(val);
			if (dtcityall.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtcityall, CmbCity, "Id", "CityName", "City Name", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Status()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Status");
			dt.Rows.Add(1, "Accepted");
			dt.Rows.Add(2, "Rejected");
			DDL.BindDDL(dt, CmbStatus, "Id", "Status", "Status", ZeroIndex: false);
			((UltraGridBase)CmbStatus).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GatePassRecordFill()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Expected O, but got Unknown
		try
		{
			DocDate.Focus();
			DataTable dtgpForGRn = GatePassInward.GetByGpNoForGRN(new GatePassInward
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt((object)GPID),
				DocumentTypeId = 51,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (dtgpForGRn.Rows.Count <= 0)
			{
				return;
			}
			GPID = Conversion.ToInt(dtgpForGRn.Rows[0]["Id"]);
			((Control)(object)combvehtyp).Text = dtgpForGRn.Rows[0]["VehicleType"].ToString();
			txtvehno.Text = dtgpForGRn.Rows[0]["VehicleNo"].ToString();
			txtbltyno.Text = dtgpForGRn.Rows[0]["BiltyNo"].ToString();
			txtremarks.Text = dtgpForGRn.Rows[0]["OtherRemarks"].ToString();
			txtcarramount.Text = dtgpForGRn.Rows[0]["NetPaid"].ToString();
			txtqty.Text = dtgpForGRn.Rows[0]["NoOfPackages"].ToString();
			((Control)(object)combwr).Text = dtgpForGRn.Rows[0]["WareHouseName"].ToString();
			combsupplier.Value = Conversion.ToInt(dtgpForGRn.Rows[0]["SupplierCustomerId"]);
			CmbCity.Value = Conversion.ToInt(dtgpForGRn.Rows[0]["CityId"]);
			txtScaleKart.Text = Conversion.ToString(dtgpForGRn.Rows[0]["ScaleKart"]);
			if (LabCompulsoryForWeighBridgeAgainstGatePurchase)
			{
				GatePassInward obj = new GatePassInward();
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.Id = GPID;
				dtLabInfo = GatePassInward.GetLabDataByGpIdForGrnGate(obj);
				if (dtLabInfo.Rows.Count > 0)
				{
					DDL.BindDDLNew(dtLabInfo, combitem, "ItemId", "ItemName", "ItemName", false);
					((UltraGridBase)combitem).DisplayLayout.Bands[0].Columns[1].Width = 500;
				}
				else
				{
					((Control)(object)combitem).Text = string.Empty;
					((UltraGridBase)combitem).DataSource = null;
				}
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
			if (Conversion.ToInt((object)combgatepass.Text) <= 0)
			{
				return;
			}
			PackUOMFillWithoutOrder();
			if (dtLabInfo.Rows.Count > 0 && ((UltraGridBase)combitem).ActiveRow != null)
			{
				DataTable selectedTable = (from r in dtLabInfo.AsEnumerable()
					where r.Field<int>("ItemId") == Conversion.ToInt(combitem.Value)
					select r).CopyToDataTable();
				txtLabNo.Text = Conversion.ToString(selectedTable.Rows[0]["DocNo"]);
				((Control)(object)combcropyear).Text = Conversion.ToString(selectedTable.Rows[0]["Crop"]);
				txtgwt.Text = Conversion.ToString(selectedTable.Rows[0]["NetWeight"]);
			}
			else
			{
				txtgwt.Text = "0";
				txtLabNo.Text = string.Empty;
			}
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
			double Qty = 0.0;
			double GrossWeight = 0.0;
			double EBUnit = 0.0;
			double EBTotal = 0.0;
			double FactoryWeight = 0.0;
			double ScaleKart = 0.0;
			double ScKartDetail = 0.0;
			if (txtfctwt.Text != string.Empty && txtfctwt.Text != null)
			{
				FactoryWeight = double.Parse(txtfctwt.Text.Trim().ToString());
			}
			if (txtsuppwt.Text != string.Empty && txtsuppwt.Text != null)
			{
				double.Parse(txtsuppwt.Text.Trim().ToString());
			}
			if (txtScaleKart.Text != string.Empty && txtScaleKart.Text != null)
			{
				ScaleKart = double.Parse(txtScaleKart.Text.Trim().ToString());
			}
			if (((UltraGridBase)combpckuom).ActiveRow != null)
			{
				Conversion.ToDouble(((UltraDropDownBase)combpckuom).SelectedRow.Cells[2].Value);
			}
			if (txtqty.Text != string.Empty && txtqty.Text != null)
			{
				Qty = double.Parse(txtqty.Text.Trim().ToString());
			}
			if (txtgwt.Text != string.Empty && txtgwt.Text != null)
			{
				GrossWeight = double.Parse(txtgwt.Text.Trim().ToString());
			}
			if (GrossWeight > 0.0 && FactoryWeight > 0.0 && ScaleKart > 0.0)
			{
				ScKartDetail = GrossWeight / FactoryWeight * ScaleKart;
				txtScaleKartDetail.Text = Math.Round(ScKartDetail, 2).ToString();
			}
			else
			{
				txtScaleKartDetail.Text = ScKartDetail.ToString();
			}
			if (txtebu.Text != string.Empty && txtebu.Text != null)
			{
				EBUnit = double.Parse(txtebu.Text.Trim().ToString());
			}
			if (EBUnit > 0.0 && Qty > 0.0)
			{
				EBTotal = EBUnit * Qty;
			}
			txtEmptyBagsTotal.Text = Math.Round(EBTotal, 2).ToString();
			if (txtnetwt.Text != string.Empty && txtnetwt.Text != null)
			{
				double.Parse(txtnetwt.Text.Trim().ToString());
			}
			double Weight = 0.0;
			Weight = GrossWeight - ScKartDetail - EBTotal;
			txtnetwt.Text = Math.Round(Weight, 2).ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EmptyBagsTypeFill()
	{
		try
		{
			dtEmpType = CommonServices.StaticColumnsService("PurchaseOrderEmptyBagsType");
			dtBagsCondition = CommonServices.StaticColumnsService("EmptyBagsCondition");
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
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			GPID = 0;
			((Control)(object)combsupplier).Text = string.Empty;
			((Control)(object)CmbTransport).Text = string.Empty;
			txtcarramount.Text = string.Empty;
			txtsuppwt.Text = string.Empty;
			txtfctwt.Text = string.Empty;
			combgatepass.Text = string.Empty;
			((UltraGridBase)combvehtyp).Rows[0].Activate();
			txtvehno.Text = string.Empty;
			txtbltyno.Text = string.Empty;
			txtremarks.Text = string.Empty;
			((UltraGridBase)combitem).DataSource = null;
			((Control)(object)combitem).Text = string.Empty;
			txtqty.Text = string.Empty;
			txtebu.Text = string.Empty;
			txtEmptyBagsTotal.Text = string.Empty;
			txtScaleKart.Text = string.Empty;
			txtnetwt.Text = string.Empty;
			txtLabNo.Text = string.Empty;
			((Control)(object)CmbCity).Text = string.Empty;
			((Control)(object)combpckuom).Text = string.Empty;
			txtgwt.Text = string.Empty;
			((Control)(object)combpckuom).Text = string.Empty;
			((UltraGridBase)combpckuom).DataSource = null;
			btnSave.Visible = true;
			DocDate.Focus();
			dtdetail.Rows.Clear();
			dtLabInfo.Rows.Clear();
			grd.DataSource = dtdetail;
			dtEmptyBag.Rows.Clear();
			AddRowInvEmptyBagsGrid();
			grdEmptyBags.RetrieveStructure();
			grdEmptyBagsSettings();
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			Add.Visible = true;
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			GenerateCode();
			Gatepass();
			defaultConfiquration();
			txtgwt.Enabled = true;
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
			txtqty.Text = string.Empty;
			txtgwt.Text = string.Empty;
			txtebu.Text = string.Empty;
			txtEmptyBagsTotal.Text = string.Empty;
			txtnetwt.Text = string.Empty;
			((Control)(object)combpckuom).Text = string.Empty;
			txtLabNo.Text = string.Empty;
			txtScaleKartDetail.Text = string.Empty;
			combitem.Focus();
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
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "X";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
			grd.RootTable.Columns["Delete"].Width = 20;
			grd.RootTable.Columns["ContractorId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["UOMId"].Visible = false;
			grd.RootTable.Columns["JobId"].Visible = false;
			grd.RootTable.Columns["PackingTypeId"].Visible = false;
			grd.RootTable.Columns["UOM"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["UOM"].HasValueList = true;
			grd.RootTable.Columns["UOM"].LimitToList = true;
			grd.RootTable.Columns["UOM"].EditType = (EditType)4;
			grd.RootTable.Columns["UOM"].Caption = "PackUom";
			grd.RootTable.Columns["UOM"].ValueList.PopulateValueList((IEnumerable)dtUOM.DefaultView, "Id", "Equivalent");
			grd.RootTable.Columns["WareHouse"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["WareHouse"].HasValueList = true;
			grd.RootTable.Columns["WareHouse"].LimitToList = true;
			grd.RootTable.Columns["WareHouse"].EditType = (EditType)4;
			grd.RootTable.Columns["WareHouse"].Caption = "WareHouse Name";
			grd.RootTable.Columns["WareHouse"].ValueList.PopulateValueList((IEnumerable)dtwh.DefaultView, "Id", "WareHouseName");
			grd.RootTable.Columns["City"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["City"].HasValueList = true;
			grd.RootTable.Columns["City"].LimitToList = true;
			grd.RootTable.Columns["City"].EditType = (EditType)4;
			grd.RootTable.Columns["City"].Caption = "City Name";
			grd.RootTable.Columns["City"].ValueList.PopulateValueList((IEnumerable)dtcity.DefaultView, "Id", "CityName");
			grd.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ScaleKart"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["EbUnit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["EbTotal"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["EbUnit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["EbTotal"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ScaleKart"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Qty"].FormatString = "#,##.##";
			grd.RootTable.Columns["GrossWeight"].FormatString = "#,##.##";
			grd.RootTable.Columns["EbUnit"].FormatString = "#,##.##";
			grd.RootTable.Columns["EbTotal"].FormatString = "#,##.##";
			grd.RootTable.Columns["NetWeight"].FormatString = "#,##.##";
			grd.RootTable.Columns["ScaleKart"].FormatString = "#,##.##";
			grd.RootTable.Columns["Qty"].TotalFormatString = "#,##.##";
			grd.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##.##";
			grd.RootTable.Columns["EbUnit"].TotalFormatString = "#,##.##";
			grd.RootTable.Columns["EbTotal"].TotalFormatString = "#,##.##";
			grd.RootTable.Columns["NetWeight"].TotalFormatString = "#,##.##";
			grd.RootTable.Columns["ScaleKart"].TotalFormatString = "#,##.##";
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInvEmptyBagsGrid()
	{
		dtEmptyBag.Rows.Add(0, 0, 0, 0, 0, 0, "");
		grdEmptyBags.DataSource = dtEmptyBag;
	}

	private void EmptyBagsGridRefresh()
	{
		try
		{
			DataTable dt = CommonServices.GetItemByItemTypeId("14");
			grdEmptyBags.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			grdEmptyBags.RootTable.Columns["ItemId"].HasValueList = true;
			grdEmptyBags.RootTable.Columns["ItemId"].LimitToList = true;
			grdEmptyBags.RootTable.Columns["ItemId"].EditType = (EditType)4;
			grdEmptyBags.RootTable.Columns["ItemId"].Caption = "Item Name";
			grdEmptyBags.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dt.DefaultView, "Id", "ItemName");
			grdEmptyBags.RootTable.Columns["ItemId"].Width = 250;
			grdEmptyBags.RootTable.Columns["Type"].ColumnType = (ColumnType)0;
			grdEmptyBags.RootTable.Columns["Type"].HasValueList = true;
			grdEmptyBags.RootTable.Columns["Type"].LimitToList = true;
			grdEmptyBags.RootTable.Columns["Type"].EditType = (EditType)4;
			grdEmptyBags.RootTable.Columns["Type"].Caption = "EmptyBagsType";
			grdEmptyBags.RootTable.Columns["Type"].ValueList.PopulateValueList((IEnumerable)dtEmpType.DefaultView, "Id", "Type");
			grdEmptyBags.RootTable.Columns["Type"].Width = 80;
			grdEmptyBags.RootTable.Columns["Condition"].ColumnType = (ColumnType)0;
			grdEmptyBags.RootTable.Columns["Condition"].HasValueList = true;
			grdEmptyBags.RootTable.Columns["Condition"].LimitToList = true;
			grdEmptyBags.RootTable.Columns["Condition"].EditType = (EditType)4;
			grdEmptyBags.RootTable.Columns["Condition"].Caption = "Bags_Condition";
			grdEmptyBags.RootTable.Columns["Condition"].ValueList.PopulateValueList((IEnumerable)dtBagsCondition.DefaultView, "Id", "Type");
			grdEmptyBags.RootTable.Columns["Condition"].Width = 60;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmptyBagsSettings()
	{
		try
		{
			EmptyBagsGridRefresh();
			grdEmptyBags.RootTable.Columns["OrderId"].EditType = (EditType)0;
			grdEmptyBags.RootTable.Columns["OrderId"].Visible = false;
			grdEmptyBags.RootTable.Columns["Condition"].Width = 120;
			grdEmptyBags.RootTable.Columns["RecQty"].AggregateFunction = (AggregateFunction)2;
			grdEmptyBags.RootTable.Columns["PurQty"].AggregateFunction = (AggregateFunction)2;
			grdEmptyBags.RootTable.Columns.Add("Add");
			grdEmptyBags.RootTable.Columns["Add"].Key = "Add";
			grdEmptyBags.RootTable.Columns["Add"].Caption = "+";
			grdEmptyBags.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdEmptyBags.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdEmptyBags.RootTable.Columns["Add"].ButtonText = "+";
			grdEmptyBags.RootTable.Columns["Add"].Width = 20;
			grdEmptyBags.RootTable.Columns.Add("Delete");
			grdEmptyBags.RootTable.Columns["Delete"].Key = "Delete";
			grdEmptyBags.RootTable.Columns["Delete"].Caption = "X";
			grdEmptyBags.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdEmptyBags.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdEmptyBags.RootTable.Columns["Delete"].ButtonText = "X";
			grdEmptyBags.RootTable.Columns["Delete"].Width = 20;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvFrmGRN_Load(object sender, EventArgs e)
	{
		try
		{
			GetConfiguration();
			txtebu.Text = "0";
			txtScaleKart.Text = "0";
			txtebu.Enabled = false;
			txtScaleKart.Enabled = false;
			if (EmptyBagsWeightCutEditable)
			{
				txtebu.Enabled = true;
			}
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnSave.Enabled = formright.DoHaveSaveRight;
			printToolStripButton.Enabled = formright.DoHavePrintRights;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			dtdetail.Columns.Add("ItemId");
			dtdetail.Columns.Add("Item");
			dtdetail.Columns.Add("CropYear");
			dtdetail.Columns.Add("JobId");
			dtdetail.Columns.Add("Job");
			dtdetail.Columns.Add("PackingTypeId");
			dtdetail.Columns.Add("PackingType");
			dtdetail.Columns.Add("UOM");
			dtdetail.Columns.Add("UOMId");
			dtdetail.Columns.Add("Qty", typeof(double));
			dtdetail.Columns.Add("GrossWeight", typeof(double));
			dtdetail.Columns.Add("ScaleKart", typeof(double));
			dtdetail.Columns.Add("EbUnit", typeof(double));
			dtdetail.Columns.Add("EbTotal", typeof(double));
			dtdetail.Columns.Add("NetWeight", typeof(double));
			dtdetail.Columns.Add("WareHouse");
			dtdetail.Columns.Add("LabNo");
			dtdetail.Columns.Add("City");
			dtdetail.Columns.Add("ContractorId");
			dtdetail.Columns.Add("Contractor");
			grd.DataSource = dtdetail;
			dtpo.Columns.Add("Id");
			dtpo.Columns.Add("DocNo");
			dtItem.Columns.Add("Id");
			dtItem.Columns.Add("ItemName");
			dtjob.Columns.Add("Id");
			dtjob.Columns.Add("JobLotDescription");
			dtPack.Columns.Add("Id");
			dtPack.Columns.Add("PackTypeDesc");
			dtUOM.Columns.Add("Id");
			dtUOM.Columns.Add("Equivalent");
			dtwh.Columns.Add("Id");
			dtwh.Columns.Add("WareHouseName");
			dtcity.Columns.Add("Id");
			dtcity.Columns.Add("CityName");
			dtCrop.Columns.Add("Id");
			dtCrop.Columns.Add("CropYear");
			EmptyBagsTypeFill();
			dtEmptyBag.Columns.Add("OrderId");
			dtEmptyBag.Columns.Add("Type");
			dtEmptyBag.Columns.Add("ItemId");
			dtEmptyBag.Columns.Add("Condition");
			dtEmptyBag.Columns.Add("RecQty", typeof(double));
			dtEmptyBag.Columns.Add("PurQty", typeof(double));
			dtEmptyBag.Columns.Add("Remarks");
			AddRowInvEmptyBagsGrid();
			grdEmptyBags.RetrieveStructure();
			grdEmptyBagsSettings();
			BranchFill();
			ProjectFill();
			GenerateCode();
			SupplierNameFilll();
			ContractorBind();
			TransportFill();
			VehicleTypesFill();
			CropYear();
			Status();
			JobLotFillWithoutOrder();
			PackingTypeFill();
			WarehouseFill();
			CityFill();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			DocDate.Focus();
			Gatepass();
			ChkBox.Checked = true;
			defaultConfiquration();
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
			grn.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			grn.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			grn.NoOfRecords = NoOfRecords;
			grn.DocumentTypeId = 165;
			if (!grn.CanViewAllRecord)
			{
				grn.EntryUser = UserAccount.ID;
			}
			DataTable dthis = new DataTable();
			dthis = InvGrn.GetHisoty(grn);
			if (dthis.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("DocDate");
				dt.Columns.Add("DocNo");
				dt.Columns.Add("SupplierName");
				dt.Columns.Add("GpNo");
				dt.Columns.Add("InwardGatePassId");
				dt.Columns.Add("VehicleNo");
				dt.Columns.Add("BiltyNo");
				dt.Columns.Add("FactoryWeight", typeof(double));
				dt.Columns.Add("PartyWeight", typeof(double));
				dt.Columns.Add("TransporterName");
				dt.Columns.Add("Freight", typeof(double));
				dt.Columns.Add("Remarks");
				dt.Columns.Add("EntryDate");
				dt.Columns.Add("EntryUser");
				dt.Columns.Add("ModifyDate");
				dt.Columns.Add("ModifyUser");
				dt.Columns.Add("NoOfAttachments");
				for (int i = 0; i < dthis.Rows.Count; i++)
				{
					dt.Rows.Add(dthis.Rows[i]["Id"], dthis.Rows[i]["DocDate"], dthis.Rows[i]["DocNo"], dthis.Rows[i]["SupplierName"], dthis.Rows[i]["GpNo"], dthis.Rows[i]["InwardGatePassId"], dthis.Rows[i]["VehicleNo"], dthis.Rows[i]["BiltyNo"], dthis.Rows[i]["FactoryWeight"], dthis.Rows[i]["PartyWeight"], dthis.Rows[i]["Transporter"], dthis.Rows[i]["CarriageAmount"], dthis.Rows[i]["RemarksHeader"], dthis.Rows[i]["EntryDate"], dthis.Rows[i]["EntryUser"], dthis.Rows[i]["ModifyDate"], dthis.Rows[i]["ModifyUser"], dthis.Rows[i]["NoOfAttachments"]);
				}
				GrdHistory.DataSource = dt;
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
			GrdHistory.AllowDelete = (InheritableBoolean)2;
			GrdHistory.AllowEdit = (InheritableBoolean)2;
			GrdHistory.ColumnAutoResize = false;
			GrdHistory.RootTable.Columns["Id"].Visible = false;
			GrdHistory.RootTable.Columns["InwardGatePassId"].Visible = false;
			GrdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			GrdHistory.RootTable.Columns["GpNo"].ColumnType = (ColumnType)5;
			GrdHistory.RootTable.Columns["SupplierName"].Width = 250;
			GrdHistory.RootTable.Columns.Add("Edit");
			GrdHistory.RootTable.Columns["Edit"].Caption = "Edit";
			GrdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			GrdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			GrdHistory.RootTable.Columns["Edit"].Width = 50;
			GrdHistory.RootTable.Columns.Add("Print");
			GrdHistory.RootTable.Columns["Print"].Caption = "Print";
			GrdHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			GrdHistory.RootTable.Columns["Print"].ButtonText = "Print";
			GrdHistory.RootTable.Columns["Print"].Width = 50;
			GrdHistory.RootTable.Columns["Edit"].Position = 0;
			GrdHistory.RootTable.Columns["Print"].Position = 1;
			GrdHistory.FrozenColumns = 2;
			((ScrollableBase)GrdHistory).ScrollBars = (ScrollBars)4;
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
				GPID = Conversion.ToInt(grdGp.CurrentRow.Cells["Id"].Value);
				combgatepass.Text = Conversion.ToString(grdGp.CurrentRow.Cells["GpSrNo"].Value);
				txtsuppwt.Text = Conversion.ToString(grdGp.CurrentRow.Cells["SupplierWeight"].Value);
				txtfctwt.Text = Conversion.ToString(grdGp.CurrentRow.Cells["FactoryWeight"].Value);
				if (Conversion.ToDouble((object)txtsuppwt.Text) == 0.0)
				{
					txtsuppwt.Text = Conversion.ToString(grdGp.CurrentRow.Cells["FactoryWeight"].Value);
				}
				if (btnSave.Visible && btnSave.Enabled)
				{
					GatePassRecordFill();
				}
			}
			if (e.Control && e.KeyCode == Keys.L && tabControl1.SelectedIndex == 1)
			{
				btnLoadAll_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.H && tabControl1.SelectedIndex == 1)
			{
				btnGrnFormHistory_Click(null, null);
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
			if (tabControl1.SelectedIndex == 0 && printToolStripButton.Visible && printToolStripButton.Enabled && e.Control && e.KeyCode == Keys.P)
			{
				printToolStripButton_Click(null, null);
			}
			if (tabControl1.SelectedIndex == 1 && e.Control && e.KeyCode == Keys.P)
			{
				GenerateReport(Conversion.ToInt((object)GrdHistory.CurrentRow.Cells["Id"].Value.ToString()));
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				combitem.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grd).Focus();
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
				Id = Conversion.ToInt((object)GrdHistory.CurrentRow.Cells[1].Value.ToString());
				ReadById_Update(Id);
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
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grd).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grd).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
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

	private void btnNew_Click(object sender, EventArgs e)
	{
		reset();
	}

	private void txtqty_TextChanged(object sender, EventArgs e)
	{
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
			Id = Conversion.ToInt((object)GrdHistory.CurrentRow.Cells["Id"].Value.ToString());
			ReadById_Update(Id);
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
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grd.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void GrdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				Id = Conversion.ToInt((object)GrdHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById_Update(Id);
			}
			if (e.Column.Key == "Print")
			{
				CommonServices.GrnSlipWithSubReports(Conversion.ToInt(GrdHistory.CurrentRow.Cells["Id"].Value));
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
			GrdHistoryDetail.AllowEdit = (InheritableBoolean)2;
			GrdHistoryDetail.AllowDelete = (InheritableBoolean)2;
			GrdHistoryDetail.ColumnAutoResize = true;
			GrdHistoryDetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["EbUnit"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["EbTotal"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["WareHouse"].ColumnType = (ColumnType)0;
			GrdHistoryDetail.RootTable.Columns["WareHouse"].HasValueList = true;
			GrdHistoryDetail.RootTable.Columns["WareHouse"].LimitToList = true;
			GrdHistoryDetail.RootTable.Columns["WareHouse"].EditType = (EditType)4;
			GrdHistoryDetail.RootTable.Columns["WareHouse"].Caption = "WareHouse Name";
			GrdHistoryDetail.RootTable.Columns["WareHouse"].ValueList.PopulateValueList((IEnumerable)dtwh.DefaultView, "Id", "WareHouseName");
			GrdHistoryDetail.RootTable.Columns["Qty"].FormatString = "0,0";
			GrdHistoryDetail.RootTable.Columns["EbUnit"].FormatString = "0,0";
			GrdHistoryDetail.RootTable.Columns["GrossWeight"].FormatString = "0,0";
			GrdHistoryDetail.RootTable.Columns["EbTotal"].FormatString = "0,0";
			GrdHistoryDetail.RootTable.Columns["NetWeight"].FormatString = "0,0";
			ctrlGrdBar4_Load(null, null);
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
			GridEXRow item = grd.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				combitem.Value = item.Cells["ItemId"].Value.ToString();
				PackUOMFillWithoutOrder();
				((Control)(object)combcropyear).Text = item.Cells["CropYear"].Text.ToString();
				combjoblot.Value = item.Cells["JobId"].Value.ToString();
				combpcktyp.Value = item.Cells["PackingTypeId"].Value.ToString();
				txtqty.Text = item.Cells["Qty"].Value.ToString();
				combpckuom.Value = item.Cells["UOMId"].Value.ToString();
				txtgwt.Text = item.Cells["GrossWeight"].Value.ToString();
				txtebu.Text = item.Cells["EbUnit"].Value.ToString();
				txtEmptyBagsTotal.Text = item.Cells["EbTotal"].Value.ToString();
				txtScaleKartDetail.Text = item.Cells["ScaleKart"].Value.ToString();
				txtnetwt.Text = item.Cells["NetWeight"].Value.ToString();
				combwr.Value = item.Cells["WareHouse"].Value.ToString();
				txtLabNo.Text = item.Cells["LabNo"].Value.ToString();
				((Control)(object)CmbCity).Text = item.Cells["City"].Text.ToString();
				CmbContractor.Value = item.Cells["ContractorId"].Value.ToString();
				Add.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				combitem.Focus();
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
				double GrossWeight = Conversion.ToDouble((object)txtgwt.Text.Trim());
				if (Conversion.ToDouble((object)txtnetwt.Text.Trim()) > GrossWeight)
				{
					throw new Exception("NetBillWeight cannot be greater than GrossWeight Please check");
				}
				dtdetail.Rows[updateDetailIndex]["ItemId"] = combitem.Value;
				dtdetail.Rows[updateDetailIndex]["Item"] = ((Control)(object)combitem).Text;
				dtdetail.Rows[updateDetailIndex]["CropYear"] = ((Control)(object)combcropyear).Text;
				dtdetail.Rows[updateDetailIndex]["JobId"] = combjoblot.Value;
				dtdetail.Rows[updateDetailIndex]["Job"] = ((Control)(object)combjoblot).Text;
				dtdetail.Rows[updateDetailIndex]["PackingTypeId"] = combpcktyp.Value;
				dtdetail.Rows[updateDetailIndex]["PackingType"] = ((Control)(object)combpcktyp).Text;
				dtdetail.Rows[updateDetailIndex]["Qty"] = txtqty.Text;
				dtdetail.Rows[updateDetailIndex]["UOM"] = ((Control)(object)combpckuom).Text;
				dtdetail.Rows[updateDetailIndex]["UOMId"] = combpckuom.Value;
				dtdetail.Rows[updateDetailIndex]["GrossWeight"] = txtgwt.Text;
				dtdetail.Rows[updateDetailIndex]["ScaleKart"] = Conversion.ToDouble((object)txtScaleKartDetail.Text);
				dtdetail.Rows[updateDetailIndex]["EbUnit"] = Conversion.ToDouble((object)txtebu.Text.Trim());
				dtdetail.Rows[updateDetailIndex]["EbTotal"] = Conversion.ToDouble((object)txtEmptyBagsTotal.Text);
				dtdetail.Rows[updateDetailIndex]["NetWeight"] = txtnetwt.Text;
				dtdetail.Rows[updateDetailIndex]["WareHouse"] = combwr.Value;
				dtdetail.Rows[updateDetailIndex]["LabNo"] = txtLabNo.Text;
				dtdetail.Rows[updateDetailIndex]["City"] = ((Control)(object)CmbCity).Text;
				dtdetail.Rows[updateDetailIndex]["ContractorId"] = CmbContractor.Value;
				dtdetail.Rows[updateDetailIndex]["Contractor"] = ((Control)(object)CmbContractor).Text;
				Add.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				ResetDetial();
			}
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
			GetConfiguration();
			SupplierNameFilll();
			TransportFill();
			VehicleTypesFill();
			CropYear();
			JobLotFillWithoutOrder();
			PackingTypeFill();
			WarehouseFill();
			CityFill();
			EmptyBagsGridRefresh();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateReport(int PrintId)
	{
		try
		{
			CommonServices.GrnSlipWithSubReports(PrintId);
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
			GenerateReport(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtdocno_TextChanged(object sender, EventArgs e)
	{
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
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = GrdHistory.CurrentRow;
			if (e.Column.Key == "NoOfAttachments" && item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 165);
			}
			if (e.Column.Key == "GpNo")
			{
				CommonServices.GatePassInwardWithDetailAndLabReport(Conversion.ToInt(item.Cells["InwardGatePassId"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
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

	private void grdGp_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (btnSave.Visible && btnSave.Enabled && e.Column.Key == "Load")
			{
				GPID = Conversion.ToInt(grdGp.CurrentRow.Cells["Id"].Value);
				combgatepass.Text = Conversion.ToString(grdGp.CurrentRow.Cells["GpSrNo"].Value);
				txtsuppwt.Text = Conversion.ToString(grdGp.CurrentRow.Cells["SupplierWeight"].Value);
				txtfctwt.Text = Conversion.ToString(grdGp.CurrentRow.Cells["FactoryWeight"].Value);
				if (Conversion.ToDouble((object)txtsuppwt.Text) == 0.0)
				{
					txtsuppwt.Text = Conversion.ToString(grdGp.CurrentRow.Cells["FactoryWeight"].Value);
				}
				if (btnSave.Visible && btnSave.Enabled)
				{
					GatePassRecordFill();
				}
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

	private void btnGrnFormHistory_Click(object sender, EventArgs e)
	{
		try
		{
			frmGRNHistory obj = new frmGRNHistory(UserAccount);
			if (clsGlobalVariables.ScreenViewReights.Count > 0)
			{
				if (bool.Parse(clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "frmGRNHistory").ToList()[0].Value.ToString()))
				{
					obj.ShowDialog();
				}
				else
				{
					MessageBox.Show("Please Check Screen Rights");
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combpcktyp_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(combpcktyp.Value) == 1)
			{
				txtebu.Text = Conversion.ToString((object)JuteBagsCut);
			}
			if (Conversion.ToInt(combpcktyp.Value) == 2)
			{
				txtebu.Text = Conversion.ToString((object)PPBagsCut);
			}
			if (Conversion.ToInt(combpcktyp.Value) == 5)
			{
				txtebu.Text = Conversion.ToString((object)OpenBulkCut);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
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

	private void grdEmptyBags_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow r = grdEmptyBags.CurrentRow;
			if (Conversion.ToInt(r.Cells["PurQty"].Value) < 0)
			{
				r.BeginEdit();
				r.Cells["PurQty"].Value = 0.ToString();
				r.EndEdit();
				MessageBox.Show("PurQty cannot be less than Zero");
			}
			if (Conversion.ToInt(r.Cells["RecQty"].Value) < 0)
			{
				r.BeginEdit();
				r.Cells["RecQty"].Value = 0.ToString();
				r.EndEdit();
				MessageBox.Show("RecQty cannot be less than Zero");
			}
			if (e.Column.Key == "PurQty" || e.Column.Key == "Type")
			{
				CalculationPurchaseAgainstWeight();
			}
			if ((Conversion.ToInt(r.Cells["Type"].Value) == 4 || Conversion.ToInt(r.Cells["Type"].Value) == 5) && e.Column.Key == "PurQty")
			{
				r.BeginEdit();
				r.Cells["PurQty"].Value = 0.ToString();
				r.EndEdit();
				MessageBox.Show("Retained or Returned Stock you cannot be purchase");
			}
			if (Conversion.ToInt(r.Cells["Type"].Value) == 2 && e.Column.Key == "RecQty")
			{
				r.Cells["RecQty"].Value = 0;
				MessageBox.Show("You cannot be add value RecQty because EmptyBagsType is Purchase Against Weight");
			}
			grdEmptyBags.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CalculationPurchaseAgainstWeight()
	{
		try
		{
			grdEmptyBags.UpdateData();
			double Qty = 0.0;
			double GrossWeight = 0.0;
			double EBUnit = 0.0;
			double EBTotal = 0.0;
			double ScaleKart = 0.0;
			double AddLess = 0.0;
			if (txtfctwt.Text != string.Empty && txtfctwt.Text != null)
			{
				double.Parse(txtfctwt.Text.Trim().ToString());
			}
			if (txtsuppwt.Text != string.Empty && txtsuppwt.Text != null)
			{
				double.Parse(txtsuppwt.Text.Trim().ToString());
			}
			double AllocateQty = 0.0;
			double PurQty = 0.0;
			double grdQty = 0.0;
			double EBQtyLessFromBillWeight = 0.0;
			for (int i = 0; i < dtEmptyBag.Rows?.Count; i++)
			{
				if (Conversion.ToInt(dtEmptyBag.Rows[i]["Type"]) == 2)
				{
					PurQty += Conversion.ToDouble(dtEmptyBag.Rows[i]["PurQty"]);
				}
			}
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow item in rows)
			{
				double TotalPurchaseAgainstWeightAddinBillWeight = 0.0;
				item.BeginEdit();
				Conversion.ToDouble(item.Cells["UOM"].Value);
				Qty = Conversion.ToDouble(item.Cells["Qty"].Value);
				GrossWeight = Conversion.ToDouble(item.Cells["GrossWeight"].Value);
				EBUnit = Conversion.ToDouble(item.Cells["EbUnit"].Value);
				ScaleKart = Conversion.ToDouble(item.Cells["ScaleKart"].Value);
				if (EBUnit > 0.0)
				{
					grdQty = Conversion.ToDouble(item.Cells["Qty"].Value);
					if (PurQty > 0.0)
					{
						PurQty -= AllocateQty;
						if (PurQty > grdQty)
						{
							EBQtyLessFromBillWeight = grdQty;
						}
						else if (grdQty > PurQty)
						{
							EBQtyLessFromBillWeight = PurQty;
						}
						if (PurQty == grdQty)
						{
							EBQtyLessFromBillWeight = grdQty;
						}
						if (PurQty > 0.0)
						{
							TotalPurchaseAgainstWeightAddinBillWeight = EBQtyLessFromBillWeight * EBUnit;
						}
					}
				}
				if (EBUnit > 0.0 && Qty > 0.0)
				{
					item.Cells["EbTotal"].Value = EBUnit * Qty;
					EBTotal = Conversion.ToDouble(item.Cells["EbTotal"].Value);
				}
				else
				{
					item.Cells["EbTotal"].Value = 0;
					EBTotal = 0.0;
				}
				double Weight = 0.0;
				Weight = GrossWeight - ScaleKart - EBTotal + AddLess + TotalPurchaseAgainstWeightAddinBillWeight;
				item.Cells["NetWeight"].Value = Math.Round(Weight, 2).ToString();
				AllocateQty = EBQtyLessFromBillWeight;
				item.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGp_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = grdGp.CurrentRow;
			if (item != null && e.Column.Key == "GpSrNo")
			{
				CommonServices.GatePassInwardWithDetailAndLabReport(Conversion.ToInt(item.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar4_Load(object sender, EventArgs e)
	{
		try
		{
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)GrdHistoryDetail).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)GrdHistoryDetail).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				GrdHistoryDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void GrdHistory_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			if (GrdHistory.CurrentRow.Cells.Count > 0)
			{
				InvGrn invGrn = InvGrn.GetByID(Conversion.ToInt(GrdHistory.CurrentRow.Cells["Id"].Value));
				DataTable dthistorydetail = new DataTable();
				dthistorydetail.Columns.Add("Item");
				dthistorydetail.Columns.Add("CropYear");
				dthistorydetail.Columns.Add("Job");
				dthistorydetail.Columns.Add("PackingType");
				dthistorydetail.Columns.Add("UOM");
				dthistorydetail.Columns.Add("Qty", typeof(double));
				dthistorydetail.Columns.Add("GrossWeight", typeof(double));
				dthistorydetail.Columns.Add("EbUnit", typeof(double));
				dthistorydetail.Columns.Add("EbTotal", typeof(double));
				dthistorydetail.Columns.Add("NetWeight", typeof(double));
				dthistorydetail.Columns.Add("WareHouse");
				dthistorydetail.Columns.Add("LabNo");
				dthistorydetail.Columns.Add("City");
				dthistorydetail.Columns.Add("Contractor");
				dthistorydetail.Clear();
				for (int i = 0; i < invGrn.invGrnDetail.Count; i++)
				{
					dthistorydetail.Rows.Add(invGrn.invGrnDetail[i].Item, invGrn.invGrnDetail[i].CropYear, invGrn.invGrnDetail[i].JobLot, invGrn.invGrnDetail[i].PackingType, invGrn.invGrnDetail[i].UOM, invGrn.invGrnDetail[i].ItemQty, invGrn.invGrnDetail[i].GrossWeight, invGrn.invGrnDetail[i].EBWPerUnit, invGrn.invGrnDetail[i].EBWTotal, invGrn.invGrnDetail[i].NetBillWeight, invGrn.invGrnDetail[i].WarehouseId, invGrn.invGrnDetail[i].LabReportRef, invGrn.invGrnDetail[i].AreaCity, invGrn.invGrnDetail[i].Contractor);
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
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Expected O, but got Unknown
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Expected O, but got Unknown
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Expected O, but got Unknown
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Expected O, but got Unknown
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Expected O, but got Unknown
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Expected O, but got Unknown
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Expected O, but got Unknown
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Expected O, but got Unknown
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Expected O, but got Unknown
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Expected O, but got Unknown
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Expected O, but got Unknown
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Expected O, but got Unknown
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Expected O, but got Unknown
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Expected O, but got Unknown
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Expected O, but got Unknown
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Expected O, but got Unknown
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Expected O, but got Unknown
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Expected O, but got Unknown
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Expected O, but got Unknown
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Expected O, but got Unknown
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Expected O, but got Unknown
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Expected O, but got Unknown
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Expected O, but got Unknown
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Expected O, but got Unknown
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Expected O, but got Unknown
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Expected O, but got Unknown
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Expected O, but got Unknown
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Expected O, but got Unknown
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Expected O, but got Unknown
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Expected O, but got Unknown
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Expected O, but got Unknown
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Expected O, but got Unknown
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Expected O, but got Unknown
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Expected O, but got Unknown
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Expected O, but got Unknown
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Expected O, but got Unknown
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Expected O, but got Unknown
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Expected O, but got Unknown
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Expected O, but got Unknown
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Expected O, but got Unknown
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Expected O, but got Unknown
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Expected O, but got Unknown
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Expected O, but got Unknown
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Expected O, but got Unknown
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Expected O, but got Unknown
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Expected O, but got Unknown
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Expected O, but got Unknown
		//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Expected O, but got Unknown
		//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Expected O, but got Unknown
		//IL_0510: Unknown result type (might be due to invalid IL or missing references)
		//IL_051a: Expected O, but got Unknown
		//IL_0526: Unknown result type (might be due to invalid IL or missing references)
		//IL_0530: Expected O, but got Unknown
		//IL_0552: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Expected O, but got Unknown
		//IL_0589: Unknown result type (might be due to invalid IL or missing references)
		//IL_0593: Expected O, but got Unknown
		//IL_059f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a9: Expected O, but got Unknown
		//IL_05c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ca: Expected O, but got Unknown
		//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e0: Expected O, but got Unknown
		//IL_05ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f6: Expected O, but got Unknown
		//IL_0602: Unknown result type (might be due to invalid IL or missing references)
		//IL_060c: Expected O, but got Unknown
		//IL_0618: Unknown result type (might be due to invalid IL or missing references)
		//IL_0622: Expected O, but got Unknown
		//IL_06c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d2: Expected O, but got Unknown
		//IL_06e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f3: Expected O, but got Unknown
		//IL_06f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fe: Expected O, but got Unknown
		//IL_06ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0709: Expected O, but got Unknown
		//IL_0741: Unknown result type (might be due to invalid IL or missing references)
		//IL_074b: Expected O, but got Unknown
		//IL_07d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07da: Expected O, but got Unknown
		//IL_08b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c1: Expected O, but got Unknown
		//IL_08cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d7: Expected O, but got Unknown
		//IL_0f98: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa2: Expected O, but got Unknown
		//IL_0faf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb9: Expected O, but got Unknown
		//IL_110e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1118: Expected O, but got Unknown
		//IL_1125: Unknown result type (might be due to invalid IL or missing references)
		//IL_112f: Expected O, but got Unknown
		//IL_12d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_12db: Expected O, but got Unknown
		//IL_782c: Unknown result type (might be due to invalid IL or missing references)
		//IL_7836: Expected O, but got Unknown
		//IL_785a: Unknown result type (might be due to invalid IL or missing references)
		//IL_7864: Expected O, but got Unknown
		GridEXLayout grdGp_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdEmptyBags_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
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
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
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
		Appearance appearance77 = new Appearance();
		Appearance appearance79 = new Appearance();
		GridEXLayout GrdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdHistory_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdGp = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdEmptyBags = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label8 = new System.Windows.Forms.Label();
		this.txtScaleKartDetail = new System.Windows.Forms.TextBox();
		this.CmbContractor = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.txtLabNo = new System.Windows.Forms.TextBox();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.combitem = new UltraCombo();
		this.label36 = new System.Windows.Forms.Label();
		this.combcropyear = new UltraCombo();
		this.label40 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.combpckuom = new UltraCombo();
		this.label31 = new System.Windows.Forms.Label();
		this.CmbCity = new UltraCombo();
		this.label42 = new System.Windows.Forms.Label();
		this.combwr = new UltraCombo();
		this.label43 = new System.Windows.Forms.Label();
		this.combpcktyp = new UltraCombo();
		this.label44 = new System.Windows.Forms.Label();
		this.combjoblot = new UltraCombo();
		this.label45 = new System.Windows.Forms.Label();
		this.Add = new System.Windows.Forms.Button();
		this.label28 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.txtnetwt = new System.Windows.Forms.TextBox();
		this.txtEmptyBagsTotal = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.txtqty = new System.Windows.Forms.TextBox();
		this.txtebu = new System.Windows.Forms.TextBox();
		this.txtgwt = new System.Windows.Forms.TextBox();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.txtScaleKart = new System.Windows.Forms.TextBox();
		this.CmbStatus = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.txtremarks = new System.Windows.Forms.RichTextBox();
		this.combvehtyp = new UltraCombo();
		this.combbrnch = new UltraCombo();
		this.combproject = new UltraCombo();
		this.combgatepass = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.label25 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.CmbTransport = new UltraCombo();
		this.label19 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.txtbltyno = new System.Windows.Forms.TextBox();
		this.txtvehno = new System.Windows.Forms.TextBox();
		this.txtcarramount = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.txtsuppwt = new System.Windows.Forms.TextBox();
		this.txtfctwt = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.combsupplier = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.label18 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label17 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
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
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
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
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbContractor).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combitem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combcropyear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combpckuom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combwr).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combpcktyp).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combjoblot).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combvehtyp).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combbrnch).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combproject).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransport).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combsupplier).BeginInit();
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
		this.panel6.Location = new System.Drawing.Point(2, 434);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1236, 274);
		this.panel6.TabIndex = 6;
		this.panel9.Controls.Add(this.ctrlGrdBar3);
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdGp);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(617, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(619, 274);
		this.panel9.TabIndex = 11;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(571, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdGp;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(45, 29);
		this.ctrlGrdBar3.TabIndex = 23;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.grdGp.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdGp).BackColor = System.Drawing.Color.White;
		grdGp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdGp.DesignTimeLayout = grdGp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGp).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdGp.FilterMode = (FilterMode)2;
		this.grdGp.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdGp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdGp.GroupByBoxFormatStyle.BackColorGradient = System.Drawing.Color.White;
		this.grdGp.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdGp).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdGp).Name = "grdGp";
		((ScrollableBase)this.grdGp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGp).Size = new System.Drawing.Size(619, 274);
		((System.Windows.Forms.Control)(object)this.grdGp).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdGp).TabStop = false;
		this.grdGp.TotalRow = (InheritableBoolean)1;
		this.grdGp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGp).VisualStyle = (VisualStyle)4;
		this.grdGp.ColumnButtonClick += new ColumnActionEventHandler(grdGp_ColumnButtonClick);
		this.grdGp.LinkClicked += new ColumnActionEventHandler(grdGp_LinkClicked);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdEmptyBags);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(617, 274);
		this.panel3.TabIndex = 10;
		((ScrollableBase)this.grdEmptyBags).BackColor = System.Drawing.Color.White;
		this.grdEmptyBags.ColumnAutoResize = true;
		grdEmptyBags_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdEmptyBags.DesignTimeLayout = grdEmptyBags_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdEmptyBags.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Name = "grdEmptyBags";
		((ScrollableBase)this.grdEmptyBags).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Size = new System.Drawing.Size(617, 274);
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).TabIndex = 0;
		this.grdEmptyBags.TotalRow = (InheritableBoolean)1;
		this.grdEmptyBags.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEmptyBags).VisualStyle = (VisualStyle)4;
		this.grdEmptyBags.CellUpdated += new ColumnActionEventHandler(grdEmptyBags_CellUpdated);
		this.grdEmptyBags.ColumnButtonClick += new ColumnActionEventHandler(grdEmptyBags_ColumnButtonClick);
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(2, 267);
		this.panel10.Margin = new System.Windows.Forms.Padding(2);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1236, 167);
		this.panel10.TabIndex = 5;
		this.grd.AllowDelete = (InheritableBoolean)1;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)1;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxFormatStyle.BackColorGradient = System.Drawing.Color.White;
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1236, 167);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grd).TabStop = false;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		this.panel5.Controls.Add(this.label8);
		this.panel5.Controls.Add(this.txtScaleKartDetail);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbContractor);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Controls.Add(this.txtLabNo);
		this.panel5.Controls.Add(this.btnCancelUpdateDetial);
		this.panel5.Controls.Add(this.btnUpdateDetail);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.combitem);
		this.panel5.Controls.Add(this.label36);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.combcropyear);
		this.panel5.Controls.Add(this.label40);
		this.panel5.Controls.Add(this.label41);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.combpckuom);
		this.panel5.Controls.Add(this.label31);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCity);
		this.panel5.Controls.Add(this.label42);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.combwr);
		this.panel5.Controls.Add(this.label43);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.combpcktyp);
		this.panel5.Controls.Add(this.label44);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.combjoblot);
		this.panel5.Controls.Add(this.label45);
		this.panel5.Controls.Add(this.Add);
		this.panel5.Controls.Add(this.label28);
		this.panel5.Controls.Add(this.label30);
		this.panel5.Controls.Add(this.label46);
		this.panel5.Controls.Add(this.label27);
		this.panel5.Controls.Add(this.txtnetwt);
		this.panel5.Controls.Add(this.txtEmptyBagsTotal);
		this.panel5.Controls.Add(this.label22);
		this.panel5.Controls.Add(this.txtqty);
		this.panel5.Controls.Add(this.txtebu);
		this.panel5.Controls.Add(this.txtgwt);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(2, 151);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1236, 116);
		this.panel5.TabIndex = 1;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(884, 9);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(60, 15);
		this.label8.TabIndex = 49;
		this.label8.Text = "ScaleKart";
		this.txtScaleKartDetail.BackColor = System.Drawing.Color.White;
		this.txtScaleKartDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtScaleKartDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtScaleKartDetail.Location = new System.Drawing.Point(887, 31);
		this.txtScaleKartDetail.Name = "txtScaleKartDetail";
		this.txtScaleKartDetail.ReadOnly = true;
		this.txtScaleKartDetail.Size = new System.Drawing.Size(72, 21);
		this.txtScaleKartDetail.TabIndex = 7;
		this.txtScaleKartDetail.TabStop = false;
		this.CmbContractor.AllowNull = (DefaultableBoolean)1;
		this.CmbContractor.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbContractor.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbContractor.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbContractor).DisplayLayout.Appearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbContractor).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbContractor).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance101).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance101).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractor).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractor).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance102;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractor).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractor).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbContractor).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbContractor).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbContractor).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbContractor).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbContractor).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbContractor).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractor).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance108).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbContractor).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbContractor).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbContractor).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance109).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractor).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbContractor).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbContractor).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbContractor).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbContractor).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbContractor).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbContractor).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbContractor).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbContractor).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbContractor).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbContractor).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbContractor.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbContractor).Location = new System.Drawing.Point(508, 85);
		((System.Windows.Forms.Control)(object)this.CmbContractor).Name = "CmbContractor";
		((System.Windows.Forms.Control)(object)this.CmbContractor).Size = new System.Drawing.Size(221, 24);
		((System.Windows.Forms.Control)(object)this.CmbContractor).TabIndex = 14;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(509, 61);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 15);
		this.label1.TabIndex = 47;
		this.label1.Text = "Contractor";
		this.txtLabNo.BackColor = System.Drawing.Color.White;
		this.txtLabNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLabNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLabNo.Location = new System.Drawing.Point(242, 87);
		this.txtLabNo.Name = "txtLabNo";
		this.txtLabNo.ReadOnly = true;
		this.txtLabNo.Size = new System.Drawing.Size(81, 21);
		this.txtLabNo.TabIndex = 12;
		this.txtLabNo.TabStop = false;
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(831, 84);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(65, 26);
		this.btnCancelUpdateDetial.TabIndex = 17;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.White;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.Location = new System.Drawing.Point(763, 84);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(65, 26);
		this.btnUpdateDetail.TabIndex = 16;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.combitem.AllowNull = (DefaultableBoolean)1;
		this.combitem.AutoCompleteMode = (AutoCompleteMode)4;
		this.combitem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combitem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combitem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.combitem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combitem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combitem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance155;
		((SpecialBoxBase)((UltraGridBase)this.combitem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance156).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance156).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.combitem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combitem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance157).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combitem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance159).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combitem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.combitem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combitem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance161).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance162).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance162;
		((AppearanceBase)appearance163).TextHAlignAsString = "Left";
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combitem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.combitem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combitem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.combitem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combitem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combitem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combitem).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combitem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combitem).Location = new System.Drawing.Point(8, 29);
		((System.Windows.Forms.Control)(object)this.combitem).Name = "combitem";
		((System.Windows.Forms.Control)(object)this.combitem).Size = new System.Drawing.Size(220, 24);
		((System.Windows.Forms.Control)(object)this.combitem).TabIndex = 0;
		this.combitem.ValueChanged += new System.EventHandler(combitem_Leave);
		((System.Windows.Forms.Control)(object)this.combitem).Leave += new System.EventHandler(combitem_Leave);
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(8, 9);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(68, 15);
		this.label36.TabIndex = 8;
		this.label36.Text = "Item Name";
		this.combcropyear.AllowNull = (DefaultableBoolean)1;
		this.combcropyear.AutoCompleteMode = (AutoCompleteMode)4;
		this.combcropyear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combcropyear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combcropyear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.combcropyear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combcropyear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combcropyear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcropyear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance92;
		((SpecialBoxBase)((UltraGridBase)this.combcropyear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance103).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance103).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcropyear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.combcropyear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combcropyear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance147).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance158).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance158).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance158).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.combcropyear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combcropyear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combcropyear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combcropyear).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combcropyear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combcropyear).Location = new System.Drawing.Point(229, 29);
		((System.Windows.Forms.Control)(object)this.combcropyear).Name = "combcropyear";
		((System.Windows.Forms.Control)(object)this.combcropyear).Size = new System.Drawing.Size(121, 24);
		((System.Windows.Forms.Control)(object)this.combcropyear).TabIndex = 1;
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(482, 9);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(76, 15);
		this.label40.TabIndex = 17;
		this.label40.Text = "Packing type";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(704, 9);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(24, 15);
		this.label41.TabIndex = 19;
		this.label41.Text = "Qty";
		this.combpckuom.AllowNull = (DefaultableBoolean)1;
		this.combpckuom.AutoCompleteMode = (AutoCompleteMode)4;
		this.combpckuom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combpckuom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combpckuom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.combpckuom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combpckuom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combpckuom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpckuom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance89;
		((SpecialBoxBase)((UltraGridBase)this.combpckuom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance90).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance90).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpckuom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.combpckuom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combpckuom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combpckuom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combpckuom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.combpckuom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combpckuom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpckuom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance95).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combpckuom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.combpckuom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combpckuom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance96).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpckuom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).TextHAlignAsString = "Left";
		((UltraGridBase)this.combpckuom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.combpckuom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combpckuom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combpckuom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.combpckuom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combpckuom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.combpckuom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combpckuom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combpckuom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combpckuom).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combpckuom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combpckuom).Location = new System.Drawing.Point(606, 29);
		((System.Windows.Forms.Control)(object)this.combpckuom).Name = "combpckuom";
		((System.Windows.Forms.Control)(object)this.combpckuom).Size = new System.Drawing.Size(97, 24);
		((System.Windows.Forms.Control)(object)this.combpckuom).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.combpckuom).Leave += new System.EventHandler(combpckuom_Leave);
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(239, 61);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(47, 15);
		this.label31.TabIndex = 17;
		this.label31.Text = "Lab No";
		this.CmbCity.AllowNull = (DefaultableBoolean)1;
		this.CmbCity.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbCity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbCity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance82).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance83).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbCity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCity).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCity).Location = new System.Drawing.Point(325, 85);
		((System.Windows.Forms.Control)(object)this.CmbCity).Name = "CmbCity";
		((System.Windows.Forms.Control)(object)this.CmbCity).Size = new System.Drawing.Size(180, 24);
		((System.Windows.Forms.Control)(object)this.CmbCity).TabIndex = 13;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(609, 9);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(66, 15);
		this.label42.TabIndex = 21;
		this.label42.Text = "Pack UOM";
		this.combwr.AllowNull = (DefaultableBoolean)1;
		this.combwr.AutoCompleteMode = (AutoCompleteMode)4;
		this.combwr.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combwr.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combwr).DisplayLayout.Appearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.combwr).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combwr).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combwr).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combwr).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance116;
		((SpecialBoxBase)((UltraGridBase)this.combwr).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combwr).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.combwr).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combwr).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combwr).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combwr).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.combwr).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combwr).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combwr).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance121).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combwr).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.combwr).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combwr).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combwr).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).TextHAlignAsString = "Left";
		((UltraGridBase)this.combwr).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.combwr).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combwr).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combwr).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.combwr).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combwr).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.combwr).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combwr).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combwr).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combwr).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combwr.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combwr).Location = new System.Drawing.Point(9, 85);
		((System.Windows.Forms.Control)(object)this.combwr).Name = "combwr";
		((System.Windows.Forms.Control)(object)this.combwr).Size = new System.Drawing.Size(227, 24);
		((System.Windows.Forms.Control)(object)this.combwr).TabIndex = 11;
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(788, 9);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(80, 15);
		this.label43.TabIndex = 23;
		this.label43.Text = "Gross Weight";
		this.combpcktyp.AllowNull = (DefaultableBoolean)1;
		this.combpcktyp.AutoCompleteMode = (AutoCompleteMode)4;
		this.combpcktyp.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combpcktyp.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combpcktyp).DisplayLayout.Appearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.combpcktyp).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combpcktyp).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combpcktyp).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpcktyp).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance129;
		((SpecialBoxBase)((UltraGridBase)this.combpcktyp).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpcktyp).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.combpcktyp).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combpcktyp).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combpcktyp).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combpcktyp).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.combpcktyp).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combpcktyp).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpcktyp).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance134).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combpcktyp).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.combpcktyp).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combpcktyp).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance135).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance135).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance135).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpcktyp).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance137).TextHAlignAsString = "Left";
		((UltraGridBase)this.combpcktyp).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.combpcktyp).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combpcktyp).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combpcktyp).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.combpcktyp).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combpcktyp).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.combpcktyp).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combpcktyp).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combpcktyp).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combpcktyp).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combpcktyp.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combpcktyp).Location = new System.Drawing.Point(481, 29);
		((System.Windows.Forms.Control)(object)this.combpcktyp).Name = "combpcktyp";
		((System.Windows.Forms.Control)(object)this.combpcktyp).Size = new System.Drawing.Size(121, 24);
		((System.Windows.Forms.Control)(object)this.combpcktyp).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.combpcktyp).Leave += new System.EventHandler(combpcktyp_Leave);
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(959, 9);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(56, 15);
		this.label44.TabIndex = 23;
		this.label44.Text = "E.b / Unit";
		this.combjoblot.AllowNull = (DefaultableBoolean)1;
		this.combjoblot.AutoCompleteMode = (AutoCompleteMode)4;
		this.combjoblot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combjoblot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combjoblot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.combjoblot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combjoblot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combjoblot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combjoblot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance142;
		((SpecialBoxBase)((UltraGridBase)this.combjoblot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combjoblot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.combjoblot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combjoblot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combjoblot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance145).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combjoblot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.combjoblot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combjoblot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combjoblot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance148).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combjoblot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.combjoblot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combjoblot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance149).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combjoblot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).TextHAlignAsString = "Left";
		((UltraGridBase)this.combjoblot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.combjoblot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combjoblot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combjoblot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.combjoblot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combjoblot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.combjoblot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combjoblot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combjoblot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combjoblot).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combjoblot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combjoblot).Location = new System.Drawing.Point(355, 29);
		((System.Windows.Forms.Control)(object)this.combjoblot).Name = "combjoblot";
		((System.Windows.Forms.Control)(object)this.combjoblot).Size = new System.Drawing.Size(121, 24);
		((System.Windows.Forms.Control)(object)this.combjoblot).TabIndex = 2;
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(1033, 9);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(55, 15);
		this.label45.TabIndex = 23;
		this.label45.Text = "E.b Total";
		this.Add.BackColor = System.Drawing.Color.White;
		this.Add.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Add.Location = new System.Drawing.Point(733, 85);
		this.Add.Margin = new System.Windows.Forms.Padding(2);
		this.Add.Name = "Add";
		this.Add.Size = new System.Drawing.Size(29, 25);
		this.Add.TabIndex = 15;
		this.Add.Text = "+";
		this.Add.UseVisualStyleBackColor = false;
		this.Add.Click += new System.EventHandler(Add_Click);
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(326, 61);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(26, 15);
		this.label28.TabIndex = 25;
		this.label28.Text = "City";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(11, 61);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(118, 15);
		this.label30.TabIndex = 41;
		this.label30.Text = "UnLoading Location";
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(1108, 9);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(67, 15);
		this.label46.TabIndex = 25;
		this.label46.Text = "Net Weight";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(357, 9);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(47, 15);
		this.label27.TabIndex = 41;
		this.label27.Text = "Job/Lot";
		this.txtnetwt.BackColor = System.Drawing.Color.White;
		this.txtnetwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtnetwt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtnetwt.Location = new System.Drawing.Point(1109, 31);
		this.txtnetwt.Name = "txtnetwt";
		this.txtnetwt.ReadOnly = true;
		this.txtnetwt.Size = new System.Drawing.Size(92, 21);
		this.txtnetwt.TabIndex = 10;
		this.txtnetwt.TabStop = false;
		this.txtEmptyBagsTotal.BackColor = System.Drawing.SystemColors.Control;
		this.txtEmptyBagsTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEmptyBagsTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEmptyBagsTotal.Location = new System.Drawing.Point(1029, 31);
		this.txtEmptyBagsTotal.Name = "txtEmptyBagsTotal";
		this.txtEmptyBagsTotal.ReadOnly = true;
		this.txtEmptyBagsTotal.Size = new System.Drawing.Size(78, 21);
		this.txtEmptyBagsTotal.TabIndex = 9;
		this.txtEmptyBagsTotal.TabStop = false;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(233, 9);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(61, 15);
		this.label22.TabIndex = 41;
		this.label22.Text = "Crop Year";
		this.txtqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtqty.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtqty.Location = new System.Drawing.Point(705, 31);
		this.txtqty.Name = "txtqty";
		this.txtqty.Size = new System.Drawing.Size(80, 21);
		this.txtqty.TabIndex = 5;
		this.txtqty.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.txtqty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.txtebu.BackColor = System.Drawing.Color.White;
		this.txtebu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtebu.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtebu.Location = new System.Drawing.Point(962, 31);
		this.txtebu.Name = "txtebu";
		this.txtebu.Size = new System.Drawing.Size(64, 21);
		this.txtebu.TabIndex = 8;
		this.txtebu.TextChanged += new System.EventHandler(txtebu_TextChanged_1);
		this.txtebu.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtebu_KeyPress);
		this.txtgwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtgwt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtgwt.Location = new System.Drawing.Point(788, 31);
		this.txtgwt.Name = "txtgwt";
		this.txtgwt.Size = new System.Drawing.Size(96, 21);
		this.txtgwt.TabIndex = 6;
		this.txtgwt.TextChanged += new System.EventHandler(txtgwt_TextChanged);
		this.txtgwt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtgwt_KeyPress);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add(this.txtScaleKart);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbStatus);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.txtremarks);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combvehtyp);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combbrnch);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combproject);
		this.panel4.Controls.Add(this.combgatepass);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add(this.ChkBox);
		this.panel4.Controls.Add(this.label25);
		this.panel4.Controls.Add(this.label24);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTransport);
		this.panel4.Controls.Add(this.label19);
		this.panel4.Controls.Add(this.label48);
		this.panel4.Controls.Add(this.label21);
		this.panel4.Controls.Add(this.label26);
		this.panel4.Controls.Add(this.txtbltyno);
		this.panel4.Controls.Add(this.txtvehno);
		this.panel4.Controls.Add(this.txtcarramount);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.label20);
		this.panel4.Controls.Add(this.txtsuppwt);
		this.panel4.Controls.Add(this.txtfctwt);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combsupplier);
		this.panel4.Controls.Add(this.label13);
		this.panel4.Controls.Add(this.DocDate);
		this.panel4.Controls.Add(this.label18);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Controls.Add(this.txtdocno);
		this.panel4.Controls.Add(this.label17);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 33);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1236, 118);
		this.panel4.TabIndex = 0;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(1031, 38);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(63, 15);
		this.label4.TabIndex = 21;
		this.label4.Text = "Scale Kart";
		this.txtScaleKart.BackColor = System.Drawing.SystemColors.Control;
		this.txtScaleKart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtScaleKart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtScaleKart.Location = new System.Drawing.Point(1109, 34);
		this.txtScaleKart.Name = "txtScaleKart";
		this.txtScaleKart.ReadOnly = true;
		this.txtScaleKart.Size = new System.Drawing.Size(75, 21);
		this.txtScaleKart.TabIndex = 14;
		this.txtScaleKart.TabStop = false;
		this.CmbStatus.AllowNull = (DefaultableBoolean)1;
		this.CmbStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbStatus).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbStatus).Location = new System.Drawing.Point(945, 34);
		((System.Windows.Forms.Control)(object)this.CmbStatus).Name = "CmbStatus";
		((System.Windows.Forms.Control)(object)this.CmbStatus).Size = new System.Drawing.Size(81, 24);
		((System.Windows.Forms.Control)(object)this.CmbStatus).TabIndex = 13;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(860, 39);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(41, 15);
		this.label3.TabIndex = 20;
		this.label3.Text = "Status";
		this.txtremarks.Location = new System.Drawing.Point(650, 62);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(534, 48);
		this.txtremarks.TabIndex = 15;
		this.txtremarks.Text = "";
		this.combvehtyp.AllowNull = (DefaultableBoolean)1;
		this.combvehtyp.AutoCompleteMode = (AutoCompleteMode)4;
		this.combvehtyp.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combvehtyp.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Appearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.combvehtyp).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combvehtyp).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combvehtyp).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combvehtyp).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance168;
		((SpecialBoxBase)((UltraGridBase)this.combvehtyp).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combvehtyp).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.combvehtyp).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combvehtyp).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.combvehtyp).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combvehtyp).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combvehtyp).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combvehtyp).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combvehtyp.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combvehtyp).Location = new System.Drawing.Point(650, 6);
		((System.Windows.Forms.Control)(object)this.combvehtyp).Name = "combvehtyp";
		((System.Windows.Forms.Control)(object)this.combvehtyp).Size = new System.Drawing.Size(202, 24);
		((System.Windows.Forms.Control)(object)this.combvehtyp).TabIndex = 8;
		this.combbrnch.AllowNull = (DefaultableBoolean)1;
		this.combbrnch.AutoCompleteMode = (AutoCompleteMode)4;
		this.combbrnch.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combbrnch.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combbrnch).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.combbrnch).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combbrnch).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combbrnch).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combbrnch).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.combbrnch).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combbrnch).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.combbrnch).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combbrnch).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.combbrnch).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combbrnch).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combbrnch).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combbrnch).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combbrnch.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combbrnch).Location = new System.Drawing.Point(75, 6);
		((System.Windows.Forms.Control)(object)this.combbrnch).Name = "combbrnch";
		((System.Windows.Forms.Control)(object)this.combbrnch).Size = new System.Drawing.Size(161, 24);
		((System.Windows.Forms.Control)(object)this.combbrnch).TabIndex = 0;
		this.combproject.AllowNull = (DefaultableBoolean)1;
		this.combproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.combproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.combproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.combproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.combproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.combproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.combproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.combproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.combproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.combproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.combproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combproject).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combproject).Location = new System.Drawing.Point(75, 34);
		((System.Windows.Forms.Control)(object)this.combproject).Name = "combproject";
		((System.Windows.Forms.Control)(object)this.combproject).Size = new System.Drawing.Size(161, 24);
		((System.Windows.Forms.Control)(object)this.combproject).TabIndex = 1;
		this.combgatepass.BackColor = System.Drawing.SystemColors.Control;
		this.combgatepass.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.combgatepass.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combgatepass.Location = new System.Drawing.Point(341, 8);
		this.combgatepass.Name = "combgatepass";
		this.combgatepass.ReadOnly = true;
		this.combgatepass.Size = new System.Drawing.Size(202, 21);
		this.combgatepass.TabIndex = 4;
		this.combgatepass.TabStop = false;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(242, 11);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(63, 15);
		this.label7.TabIndex = 0;
		this.label7.Text = "Gate Pass";
		this.ChkBox.AutoSize = true;
		this.ChkBox.Location = new System.Drawing.Point(1187, 92);
		this.ChkBox.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(47, 17);
		this.ChkBox.TabIndex = 16;
		this.ChkBox.Text = "Print";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(740, 39);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(48, 15);
		this.label25.TabIndex = 3;
		this.label25.Text = "Bilty No";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(551, 62);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(57, 15);
		this.label24.TabIndex = 0;
		this.label24.Text = "Remarks";
		this.CmbTransport.AllowNull = (DefaultableBoolean)1;
		this.CmbTransport.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTransport.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTransport.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbTransport).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTransport).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransport).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransport).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransport).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransport).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbTransport).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTransport).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbTransport).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTransport).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTransport).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTransport).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTransport.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTransport).Location = new System.Drawing.Point(341, 61);
		((System.Windows.Forms.Control)(object)this.CmbTransport).Name = "CmbTransport";
		((System.Windows.Forms.Control)(object)this.CmbTransport).Size = new System.Drawing.Size(201, 24);
		((System.Windows.Forms.Control)(object)this.CmbTransport).TabIndex = 6;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(551, 39);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(66, 15);
		this.label19.TabIndex = 0;
		this.label19.Text = "Vehicle No";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(242, 66);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(70, 15);
		this.label48.TabIndex = 8;
		this.label48.Text = "Transporter";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(551, 11);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(76, 15);
		this.label21.TabIndex = 8;
		this.label21.Text = "Vehicle Type";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(242, 92);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(90, 15);
		this.label26.TabIndex = 8;
		this.label26.Text = "Freight Amount";
		this.txtbltyno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbltyno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbltyno.Location = new System.Drawing.Point(798, 36);
		this.txtbltyno.Name = "txtbltyno";
		this.txtbltyno.Size = new System.Drawing.Size(53, 21);
		this.txtbltyno.TabIndex = 10;
		this.txtvehno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvehno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtvehno.Location = new System.Drawing.Point(650, 36);
		this.txtvehno.Name = "txtvehno";
		this.txtvehno.Size = new System.Drawing.Size(82, 21);
		this.txtvehno.TabIndex = 9;
		this.txtcarramount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcarramount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcarramount.Location = new System.Drawing.Point(341, 89);
		this.txtcarramount.Name = "txtcarramount";
		this.txtcarramount.Size = new System.Drawing.Size(202, 21);
		this.txtcarramount.TabIndex = 7;
		this.txtcarramount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcarramount_KeyPress);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(860, 11);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(77, 15);
		this.label6.TabIndex = 0;
		this.label6.Text = "Supp Weight";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(1031, 11);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(71, 15);
		this.label20.TabIndex = 3;
		this.label20.Text = "Fact Weight";
		this.txtsuppwt.BackColor = System.Drawing.SystemColors.Control;
		this.txtsuppwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsuppwt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtsuppwt.Location = new System.Drawing.Point(945, 8);
		this.txtsuppwt.Name = "txtsuppwt";
		this.txtsuppwt.ReadOnly = true;
		this.txtsuppwt.Size = new System.Drawing.Size(81, 21);
		this.txtsuppwt.TabIndex = 11;
		this.txtsuppwt.TabStop = false;
		this.txtfctwt.BackColor = System.Drawing.SystemColors.Control;
		this.txtfctwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfctwt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfctwt.Location = new System.Drawing.Point(1109, 8);
		this.txtfctwt.Name = "txtfctwt";
		this.txtfctwt.ReadOnly = true;
		this.txtfctwt.Size = new System.Drawing.Size(75, 21);
		this.txtfctwt.TabIndex = 12;
		this.txtfctwt.TabStop = false;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(242, 39);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(90, 15);
		this.label2.TabIndex = 0;
		this.label2.Text = "Supplier Name";
		this.combsupplier.AllowNull = (DefaultableBoolean)1;
		this.combsupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.combsupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combsupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combsupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.combsupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combsupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combsupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combsupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance69;
		((SpecialBoxBase)((UltraGridBase)this.combsupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combsupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.combsupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combsupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance74).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance75).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).TextHAlignAsString = "Left";
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.combsupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combsupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combsupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combsupplier).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combsupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combsupplier).Location = new System.Drawing.Point(341, 34);
		((System.Windows.Forms.Control)(object)this.combsupplier).Name = "combsupplier";
		((System.Windows.Forms.Control)(object)this.combsupplier).Size = new System.Drawing.Size(201, 24);
		((System.Windows.Forms.Control)(object)this.combsupplier).TabIndex = 5;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(9, 11);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(46, 15);
		this.label13.TabIndex = 0;
		this.label13.Text = "Branch";
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(75, 89);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(160, 21);
		this.DocDate.TabIndex = 3;
		this.DocDate.TabStop = false;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(9, 39);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(45, 15);
		this.label18.TabIndex = 0;
		this.label18.Text = "Project";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(9, 66);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(48, 15);
		this.label5.TabIndex = 3;
		this.label5.Text = "Doc No";
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(75, 63);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(161, 21);
		this.txtdocno.TabIndex = 2;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(9, 92);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(58, 15);
		this.label17.TabIndex = 3;
		this.label17.Text = "Doc Date";
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1236, 31);
		this.panel2.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1188, -1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 29);
		this.ctrlGrdBar1.TabIndex = 22;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.toolStripLabel1, this.btnNew, this.btnUpdate, this.btnSave, this.printToolStripButton, this.btnAttachment, this.toolStripButton1, this.CityDefine, this.btnDelete });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1236, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripLabel1.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripLabel1.Name = "toolStripLabel1";
		this.toolStripLabel1.Size = new System.Drawing.Size(215, 24);
		this.toolStripLabel1.Text = "Goods Receiving Notes             ";
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
		this.printToolStripButton.Text = "211-Print";
		this.printToolStripButton.ToolTipText = "Ctrl_P";
		this.printToolStripButton.Click += new System.EventHandler(printToolStripButton_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(94, 24);
		this.btnAttachment.Text = "&Attachment";
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
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
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
		((ScrollableBase)this.GrdHistory).ScrollBars = (ScrollBars)4;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Size = new System.Drawing.Size(1236, 354);
		((System.Windows.Forms.Control)(object)this.GrdHistory).TabIndex = 44;
		this.GrdHistory.TotalRow = (InheritableBoolean)1;
		this.GrdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistory).VisualStyle = (VisualStyle)4;
		this.GrdHistory.ColumnButtonClick += new ColumnActionEventHandler(GrdHistory_ColumnButtonClick);
		this.GrdHistory.SelectionChanged += new System.EventHandler(GrdHistory_SelectionChanged);
		this.GrdHistory.LinkClicked += new ColumnActionEventHandler(GrdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdHistory).DoubleClick += new System.EventHandler(GrdHistory_DoubleClick);
		this.panel7.Controls.Add(this.ctrlGrdBar4);
		this.panel7.Controls.Add(this.ctrlGrdBar2);
		this.panel7.Controls.Add(this.toolStrip2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(2, 2);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1236, 28);
		this.panel7.TabIndex = 0;
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(1187, 0);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar4.MyGrid = this.GrdHistoryDetail;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(45, 27);
		this.ctrlGrdBar4.TabIndex = 24;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1132, 0);
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
		this.btnGrnFormHistory.Size = new System.Drawing.Size(88, 24);
		this.btnGrnFormHistory.Text = "GrnHistory";
		this.btnGrnFormHistory.ToolTipText = "Crtl_H";
		this.btnGrnFormHistory.Click += new System.EventHandler(btnGrnFormHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoSize = true;
		base.ClientSize = new System.Drawing.Size(1248, 736);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "frmGrnForGate";
		this.Text = "Goods Receiving Notes";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(InvFrmGRN_Load);
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
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbContractor).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combitem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combcropyear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combpckuom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combwr).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combpcktyp).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combjoblot).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combvehtyp).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combbrnch).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combproject).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransport).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combsupplier).EndInit();
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
