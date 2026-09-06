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
using Architecture.BLL.Steel.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.Model.Steel.Inventory;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Contractor_Wages;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Inventory_Reports;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Steel.Purchase;

public class InvFrmGRN_St : Form
{
	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private List<InvGrnDetail> lstRemoveRecord = new List<InvGrnDetail>();

	private int[] AryColWidth;

	private int GPID;

	private int FreightOnCash;

	private int FreightId;

	private int RefDocumentTypeId;

	private double GpQty;

	private int updateDetailIndex;

	private int Id;

	private bool UpdateMode;

	private bool WagesStatus;

	private bool AddLessWeightEditable;

	private bool LabCompulsoryForWeighBridgeAgainstGatePurchase;

	private bool IsApproved;

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtdetail = new DataTable();

	private DataTable dtitem = new DataTable();

	private DataTable dtwh = new DataTable();

	private DataTable dtcity = new DataTable();

	private DataTable dtsupbyGrpId = new DataTable();

	private DataTable dtAccountList = new DataTable();

	private DataTable dtAllPendingData = new DataTable();

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

	private TextBox txtRemarks;

	private Label label21;

	private Label label19;

	private TextBox txtBiltyNo;

	private Label label7;

	private TextBox txtVehicleNo;

	internal UltraCombo CmbTransport;

	private Label label48;

	private Label label26;

	private TextBox txtCarrAmount;

	private Label label20;

	private TextBox txtSuppWeight;

	private Label label6;

	private TextBox txtFactoryWeight;

	private Label lblRecwt;

	private TextBox txtBalWeight;

	internal UltraCombo combrefprty;

	internal UltraCombo combstockprty;

	internal UltraCombo CmbSupplier;

	private Label label2;

	private Label label12;

	private Label label10;

	private Label label8;

	private DateTimePicker DocDate;

	private Label label17;

	private TextBox txtdocno;

	private Label label5;

	internal UltraCombo CmbPackUom;

	internal UltraCombo CmbCityName;

	internal UltraCombo CmbWarehouse;

	internal UltraCombo CmbPackingType;

	internal UltraCombo CmbJobLot;

	internal UltraCombo CmbItemName;

	private Label label30;

	private Label label27;

	private TextBox txtGrossWeight;

	private TextBox txtqty;

	private TextBox txtAddLessWeight;

	private TextBox txtStockWeight;

	private Label label29;

	private TextBox txtNetWeight;

	private Label label46;

	private Label label28;

	private Label label35;

	private Label label43;

	private Label label42;

	private Label label31;

	private Label label41;

	private Label label40;

	private Label label36;

	private Label label37;

	private Panel panel7;

	private Panel panel8;

	internal GridEX GrdHistory;

	private CtrlGrdBar ctrlGrdBar1;

	internal UltraCombo CmbOrderNo;

	private Label label9;

	private Label label3;

	private TextBox txtWeighttCutTotal;

	private TextBox txtWeighttCut;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private ToolStripButton toolStripButton1;

	private Label label15;

	private TextBox txtDeliverTerm;

	private TextBox CmbGatePass;

	private Panel panel10;

	internal GridEX grd;

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

	internal UltraCombo CmbVehicleType;

	private ToolStripButton btnDelete;

	private TextBox txtLabNo;

	private TextBox txtGrnWeight;

	private Panel panel9;

	private CtrlGrdBar ctrlGrdBar3;

	private Label label1;

	private TextBox txtAnaylstName;

	private TextBox txtDeductionKg;

	private TextBox txtReceivedWeight;

	private Label lblBalwt;

	private CtrlGrdBar ctrlGrdBar4;

	private Panel panel13;

	private Panel panel12;

	private Panel panel16;

	private Panel panel17;

	private Panel panel19;

	private Panel panel18;

	private Panel panel21;

	private Panel panel20;

	private Label label4;

	private Label label14;

	private Label label23;

	private Panel panel3;

	private Label label11;

	private Label label13;

	public InvFrmGRN_St(UserAccount user)
	{
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void InvFrmGRN_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1060, 800);
			GetConfiguration();
			CmbSupplier.ReadOnly = true;
			txtSuppWeight.ReadOnly = true;
			txtAddLessWeight.Text = "0";
			txtAddLessWeight.Enabled = false;
			if (AddLessWeightEditable)
			{
				txtAddLessWeight.Enabled = true;
			}
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnSave.Enabled = formright.DoHaveSaveRight;
			printToolStripButton.Enabled = formright.DoHavePrintRights;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			btnDelete.Visible = formright.DoHaveCanDelete;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar2.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar2.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar2.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar2.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar2.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar3.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar3.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar3.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar3.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar3.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar3.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar4.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar4.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar4.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar4.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar4.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar4.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			dtViewAttachments.Columns.Add("AttchmentPath");
			dtdetail.Columns.Add("Id");
			dtdetail.Columns.Add("OrderDetailId");
			dtdetail.Columns.Add("OrderId");
			dtdetail.Columns.Add("OrderNo");
			dtdetail.Columns.Add("ItemId");
			dtdetail.Columns.Add("ItemName");
			dtdetail.Columns.Add("JobLotId");
			dtdetail.Columns.Add("JobLot");
			dtdetail.Columns.Add("PackingTypeId");
			dtdetail.Columns.Add("PackingType");
			dtdetail.Columns.Add("PackUom");
			dtdetail.Columns.Add("PackUomId");
			dtdetail.Columns.Add("ItemQty", typeof(double));
			dtdetail.Columns.Add("GrossWeight", typeof(double));
			dtdetail.Columns.Add("WtCut", typeof(double));
			dtdetail.Columns.Add("WtCutTotal", typeof(double));
			dtdetail.Columns.Add("AddLesswt", typeof(double));
			dtdetail.Columns.Add("NetWeight", typeof(double));
			dtdetail.Columns.Add("StockWeight", typeof(double));
			dtdetail.Columns.Add("Warehouse");
			dtdetail.Columns.Add("LabNo");
			dtdetail.Columns.Add("City");
			grd.DataSource = dtdetail;
			dtwh.Columns.Add("Id");
			dtwh.Columns.Add("WareHouseName");
			GenerateCode();
			SupplierNameFilll();
			TransportFill();
			VehicleTypesFill();
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Insert()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Expected O, but got Unknown
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
			GRN.DocumentTypeId = 1501;
			GRN.DocDate = DocDate.Value;
			GRN.DocNo = Convert.ToInt32(txtdocno.Text.Trim());
			GRN.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			GRN.GpNo = Conversion.ToInt((object)CmbGatePass.Text);
			GRN.PartyWeight = Conversion.ToDecimal((object)txtSuppWeight.Text.Trim());
			GRN.FactoryWeight = Conversion.ToDecimal((object)txtFactoryWeight.Text.Trim());
			GRN.VehicleTypeId = Conversion.ToInt(CmbVehicleType.Value);
			GRN.VehicleNo = Conversion.ToString((object)txtVehicleNo.Text.Trim());
			GRN.DeliveryTerm = Conversion.ToString((object)txtDeliverTerm.Text.Trim());
			GRN.BiltyNo = txtBiltyNo.Text.Trim();
			if (Conversion.ToDouble((object)txtCarrAmount.Text.Trim()) > 0.0)
			{
				if (((UltraGridBase)CmbTransport).ActiveRow == null)
				{
					MessageBox.Show("Transporter Account field required");
					CmbTransport.Focus();
					return;
				}
				GRN.TransporterId = Conversion.ToInt(CmbTransport.Value);
				GRN.CarriageAmount = Conversion.ToDecimal((object)txtCarrAmount.Text.Trim());
			}
			GRN.RemarksHeader = txtRemarks.Text.Trim();
			GRN.BranchesId = Conversion.ToInt((object)UserAccount.BranchesId);
			GRN.InwardGatePassId = Conversion.ToInt((object)GPID);
			GRN.OrganizationId = UserAccount.OrganizationId;
			GRN.CompanyId = UserAccount.CompanyId;
			GRN.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			GRN.ApprovedDate = DateTime.Now;
			GRN.EntryDate = DateTime.Now;
			GRN.ModifyDate = DateTime.Now;
			GRN.EntryUserId = UserAccount.ID;
			GRN.ModifyUserId = UserAccount.ID;
			GRN.invGrnDetail = new List<InvGrnDetail>();
			decimal GrnWeight = Conversion.ToDecimal((object)txtGrnWeight.Text.Trim());
			decimal GrossWeight = default(decimal);
			if (Id > 0 && lstRemoveRecord.Count > 0)
			{
				for (int i = 0; i < lstRemoveRecord.Count; i++)
				{
					GRN.invGrnDetail.Add(lstRemoveRecord[i]);
				}
			}
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				InvGrnDetail grnDetail = new InvGrnDetail();
				grnDetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
				grnDetail.PurchaseOrderDetailId = Conversion.ToInt(r.Cells["OrderDetailId"].Value);
				grnDetail.PurchaseOrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
				grnDetail.PurchaserOrderNo = Conversion.ToInt(r.Cells["OrderNo"].Value);
				grnDetail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				grnDetail.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
				grnDetail.PackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
				grnDetail.ItemQty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
				grnDetail.ItemUomId = Conversion.ToInt(r.Cells["PackUomId"].Value);
				grnDetail.GrossWeight = Conversion.ToDecimal(r.Cells["GrossWeight"].Value);
				GrossWeight += grnDetail.GrossWeight;
				grnDetail.WtCut = Conversion.ToDecimal(r.Cells["WtCut"].Value);
				grnDetail.WtCutTotal = Conversion.ToDecimal(r.Cells["WtCutTotal"].Value);
				grnDetail.AdLsWeight = Conversion.ToDecimal(r.Cells["AddLesswt"].Value);
				grnDetail.NetBillWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
				grnDetail.StockWeight = Conversion.ToDecimal(r.Cells["StockWeight"].Value);
				grnDetail.WarehouseId = Conversion.ToInt(r.Cells["Warehouse"].Value);
				grnDetail.LabReportRef = Conversion.ToString((object)r.Cells["LabNo"].Text);
				grnDetail.CityId = Conversion.ToInt(r.Cells["City"].Value);
				if (grnDetail.Id > 0)
				{
					grnDetail.ActionTypeId = 2;
				}
				else
				{
					grnDetail.ActionTypeId = 1;
				}
				GRN.invGrnDetail.Add(grnDetail);
			}
			if (GrossWeight != GrnWeight)
			{
				throw new Exception("GrossWeight and GrnWeight Not Match");
			}
			decimal SupplierWeight = default(decimal);
			decimal FactoryWeight = default(decimal);
			if (txtDeliverTerm.Text == "Load" || txtDeliverTerm.Text == "Load & PartyWeight" || txtDeliverTerm.Text == "Ponch & PartyWeight")
			{
				SupplierWeight = Conversion.ToDecimal((object)txtSuppWeight.Text.Trim());
				if (GrossWeight != SupplierWeight)
				{
					throw new Exception("GrossWeight and Supplier Weight Not Match");
				}
			}
			if (txtDeliverTerm.Text == "Load & FactoryWeight" || txtDeliverTerm.Text == "Ponch & FactoryWeight")
			{
				FactoryWeight = Conversion.ToDecimal((object)txtFactoryWeight.Text.Trim());
				if (GrossWeight != FactoryWeight)
				{
					throw new Exception("GrossWeight and Factory Weight Not Match");
				}
			}
			if (txtDeliverTerm.Text == "Ponch")
			{
				SupplierWeight = Conversion.ToDecimal((object)txtSuppWeight.Text.Trim());
				FactoryWeight = Conversion.ToDecimal((object)txtFactoryWeight.Text.Trim());
				if (SupplierWeight > FactoryWeight)
				{
					if (GrossWeight != FactoryWeight)
					{
						throw new Exception("GrossWeight and Factory Weight Not Match");
					}
				}
				else if (GrossWeight != SupplierWeight)
				{
					throw new Exception("GrossWeight and Supplier Weight Not Match");
				}
			}
			GRN.AttachmentsList = new List<DMSAttachments>();
			GRN.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<InvGrn>(GRN, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			int success = InvGrn.Save(GRN);
			if (Id > 0)
			{
				MessageBox.Show("Record Update Successfully [" + GRN.DocNo + "]");
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
				{
					DMSAttachments.RemoveByIdAndNames(Id, base.Name, 1501, 0);
				}
			}
			else
			{
				MessageBox.Show("Record Save Successfully [" + GRN.DocNo + "]");
			}
			if (WagesStatus)
			{
				frmwagesBillHeader frmwagesBillHeader = new frmwagesBillHeader(UserAccount);
				frmwagesBillHeader.RefDocTypeId = 46;
				frmwagesBillHeader.RefDocId = success;
				frmwagesBillHeader.GrossWeightTotal = Conversion.ToDouble((object)GrossWeight);
				frmwagesBillHeader.ShowDialog();
				frmwagesBillHeader.BringToFront();
			}
			reset();
			if (ChkBox.Checked)
			{
				try
				{
					GenerateReport(success);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
			grd.DataSource = null;
			btnSave.Visible = true;
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

	private void btnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (IsApproved)
			{
				throw new Exception("Record Can't Be Deleted beacause Record has approved");
			}
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					InvGrn.DeleteById(Id, UserAccount.ID);
					MessageBox.Show("Record Deleted Seccessfully");
					reset();
				}
				return;
			}
			throw new Exception("No Record Found For Delete");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void ReadById(int ID)
	{
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Expected O, but got Unknown
		try
		{
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			UpdateMode = true;
			lstRemoveRecord.Clear();
			Id = Conversion.ToInt((object)ID);
			InvGrn invGrn = InvGrn.GetByID(Id);
			txtdocno.Text = invGrn.DocNo.ToString();
			DocDate.Value = invGrn.DocDate;
			tabControl1.SelectedIndex = 0;
			CmbSupplier.Value = invGrn.SupplierCustomerId;
			CmbSupplier.ReadOnly = true;
			CmbTransport.Value = invGrn.TransporterId;
			txtCarrAmount.Text = invGrn.CarriageAmount.ToString(clsGlobalVariables.stringFormatsingle);
			txtSuppWeight.Text = invGrn.PartyWeight.ToString("#,##0.##");
			txtFactoryWeight.Text = invGrn.FactoryWeight.ToString("#,##0.##");
			CmbGatePass.Text = invGrn.GpNo.ToString();
			CmbVehicleType.Value = invGrn.VehicleTypeId;
			txtVehicleNo.Text = invGrn.VehicleNo;
			txtBiltyNo.Text = invGrn.BiltyNo;
			txtRemarks.Text = invGrn.RemarksHeader;
			txtDeliverTerm.Text = Conversion.ToString((object)invGrn.DeliveryTerm);
			GPID = Conversion.ToInt((object)invGrn.InwardGatePassId);
			RefDocumentTypeId = invGrn.RefDocumentTypeId;
			FreightId = invGrn.FreightId;
			FreightOnCash = invGrn.FreightId;
			TransporterAccountDisable();
			IsApproved = invGrn.IsApproved;
			if (RefDocumentTypeId == 106)
			{
				if (LabCompulsoryForWeighBridgeAgainstGatePurchase)
				{
					GatePassInward obj = new GatePassInward();
					obj.OrganizationId = UserAccount.OrganizationId;
					obj.CompanyId = UserAccount.CompanyId;
					obj.Id = GPID;
					dtLabInfo = GatePassInward.GetDataByGpId(obj);
					if (dtLabInfo.Rows.Count > 0)
					{
						DDL.BindDDLNew(dtLabInfo, CmbItemName, "ItemId", "ItemName", "ItemName", false);
						((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[1].Width = 500;
					}
					else
					{
						((Control)(object)CmbItemName).Text = string.Empty;
						((UltraGridBase)CmbItemName).DataSource = null;
					}
				}
				else
				{
					ItemsBindWithoutPurchaseOrder();
				}
			}
			dtdetail.Clear();
			decimal GrossWeight = default(decimal);
			for (int i = 0; i < invGrn.invGrnDetail.Count; i++)
			{
				dtdetail.Rows.Add(invGrn.invGrnDetail[i].Id, invGrn.invGrnDetail[i].PurchaseOrderDetailId, invGrn.invGrnDetail[i].PurchaseOrderId, invGrn.invGrnDetail[i].PurchaserOrderNo, invGrn.invGrnDetail[i].ItemId, invGrn.invGrnDetail[i].ItemName, invGrn.invGrnDetail[i].JobLotId, invGrn.invGrnDetail[i].JobLotDescription, invGrn.invGrnDetail[i].PackingTypeId, invGrn.invGrnDetail[i].PackingType, invGrn.invGrnDetail[i].UOMCode, invGrn.invGrnDetail[i].ItemUomId, invGrn.invGrnDetail[i].ItemQty, invGrn.invGrnDetail[i].GrossWeight, invGrn.invGrnDetail[i].WtCut, invGrn.invGrnDetail[i].WtCutTotal, invGrn.invGrnDetail[i].AdLsWeight, invGrn.invGrnDetail[i].NetBillWeight, invGrn.invGrnDetail[i].StockWeight, invGrn.invGrnDetail[i].WarehouseId, invGrn.invGrnDetail[i].LabReportRef, invGrn.invGrnDetail[i].CityId);
				GrossWeight += invGrn.invGrnDetail[i].GrossWeight;
			}
			if (RefDocumentTypeId != 105 && RefDocumentTypeId != 106)
			{
				txtDeliverTerm.Text = invGrn.DeliveryTerm;
			}
			txtGrnWeight.Text = GrossWeight.ToString();
			grd.DataSource = dtdetail;
			grd.RetrieveStructure();
			grdSettings();
			FormHelper.LoadAttachmentsForObject(AT, Id, base.Name);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
		if (((UltraGridBase)CmbSupplier).ActiveRow == null)
		{
			MessageBox.Show("Supplier Field is Required");
			CmbSupplier.Focus();
			return false;
		}
		if (RefDocumentTypeId == 1500 && (txtDeliverTerm.Text.Trim() == "" || txtDeliverTerm.Text.Trim() == "0" || txtDeliverTerm.Text.Trim() == null))
		{
			MessageBox.Show("DeliveryTerm Field is Required");
			txtDeliverTerm.Focus();
			return false;
		}
		if (Conversion.ToInt((object)CmbGatePass.Text.Trim()) == 0)
		{
			MessageBox.Show("Gatepass Field is Required");
			CmbGatePass.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtSuppWeight.Text.Trim()) == 0.0 || txtSuppWeight.Text.Trim() == null)
		{
			MessageBox.Show("Supplier Weight Field is Required");
			txtSuppWeight.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtFactoryWeight.Text.Trim()) == 0.0 || txtFactoryWeight.Text.Trim() == null)
		{
			MessageBox.Show("Factory Weight Field is Required");
			txtFactoryWeight.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (RefDocumentTypeId == 1500 && ((UltraGridBase)CmbOrderNo).ActiveRow == null)
		{
			MessageBox.Show("Please Select an Order No First");
			CmbOrderNo.Focus();
			return false;
		}
		if (((UltraGridBase)CmbItemName).ActiveRow == null)
		{
			MessageBox.Show("Item Name Field is Required");
			CmbItemName.Focus();
			return false;
		}
		if (((UltraGridBase)CmbJobLot).ActiveRow == null)
		{
			MessageBox.Show("Job/Lot Field is Required");
			CmbJobLot.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackingType).ActiveRow == null)
		{
			MessageBox.Show("Packing Type Field is Required");
			CmbPackingType.Focus();
			return false;
		}
		if (txtqty.Text.Trim() == "" || Conversion.ToDouble((object)txtqty.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Qty Field is Required");
			txtqty.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackUom).ActiveRow == null)
		{
			MessageBox.Show("Pack Unit Field is Required");
			CmbPackUom.Focus();
			return false;
		}
		if (txtGrossWeight.Text.Trim() == "" || Conversion.ToDecimal((object)txtGrossWeight.Text.Trim()) == 0m)
		{
			MessageBox.Show("Gross Weight Field is Required");
			txtGrossWeight.Focus();
			return false;
		}
		if (txtNetWeight.Text.Trim() == "" || Conversion.ToDouble((object)txtNetWeight.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Net Bill Weight Field is Required");
			txtNetWeight.Focus();
			return false;
		}
		if (txtStockWeight.Text.Trim() == "" || Conversion.ToDouble((object)txtStockWeight.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Stock Weight Field is Required");
			txtStockWeight.Focus();
			return false;
		}
		if (((UltraGridBase)CmbWarehouse).ActiveRow == null)
		{
			MessageBox.Show("Warehouse Field is Required");
			CmbWarehouse.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCityName).ActiveRow == null)
		{
			MessageBox.Show("City Field is Required");
			CmbCityName.Focus();
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
				CmbJobLot.Value = JobId;
			}
			List<ConfigrationsAllocation> lstPackingType = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Paking Type").ToList();
			if (lstPackingType.Count > 0)
			{
				int PackingTypeId = Conversion.ToInt((object)lstPackingType[0].ConfigKey.ToString());
				CmbPackingType.Value = PackingTypeId;
			}
			List<ConfigrationsAllocation> lstWareHouse = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Warehouse").ToList();
			if (lstWareHouse.Count > 0)
			{
				int WareHouseId = Conversion.ToInt((object)lstWareHouse[0].ConfigKey.ToString());
				CmbWarehouse.Value = WareHouseId;
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
			MessageBox.Show("Confiquration Error", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GetConfiguration()
	{
		try
		{
			DataTable dtConfiq = CommonServices.GetMultipleConfigurationsByConfigDescriptions("WeightCutForJuteBags,WeightCutForPPBags,WeightCutForOpenBulk,EmptyBagsWeightCutEditableOnGRN,AddLessWeightCutEditableOnGRN,ContractorWagesCompulsoryBeforeInvoices,EmptyBagsInofrmationCompulsoryOnGRN,LabCompulsoryForWeighBridgeAgainstGatePurchase");
			if (dtConfiq.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtConfiq.Rows.Count; i++)
			{
				if (Conversion.ToString(dtConfiq.Rows[i]["ConfigDescription"]) == "AddLessWeightCutEditableOnGRN")
				{
					AddLessWeightEditable = Conversion.ToBool(dtConfiq.Rows[i]["ConfigKey"]);
				}
				if (Conversion.ToString(dtConfiq.Rows[i]["ConfigDescription"]) == "ContractorWagesCompulsoryBeforeInvoices")
				{
					WagesStatus = Conversion.ToBool(dtConfiq.Rows[i]["ConfigKey"]);
				}
				if (Conversion.ToString(dtConfiq.Rows[i]["ConfigDescription"]) == "LabCompulsoryForWeighBridgeAgainstGatePurchase")
				{
					LabCompulsoryForWeighBridgeAgainstGatePurchase = Conversion.ToBool(dtConfiq.Rows[i]["ConfigKey"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GenerateCode()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		try
		{
			int DocNo = Conversion.ToInt((object)InvGrn.GenerateInvGrnCode(new InvGrn
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 1501,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			}));
			txtdocno.Text = DocNo.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void SupplierNameFilll()
	{
		try
		{
			dtsupbyGrpId.Rows.Clear();
			dtsupbyGrpId = CommonServices.SupplierCustomerGetforComboServiceBind();
			if (dtsupbyGrpId.Rows.Count > 0)
			{
				DDL.BindDDL(dtsupbyGrpId, CmbSupplier, "Id", "CompanyName", "Supplier Name", ZeroIndex: false);
				DDL.BindDDL(dtsupbyGrpId, combstockprty, "Id", "CompanyName", "Supplier Name", ZeroIndex: false);
				DDL.BindDDL(dtsupbyGrpId, combrefprty, "Id", "CompanyName", "Supplier Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void TransportFill()
	{
		try
		{
			dtAccountList.Rows.Clear();
			dtAccountList = CommonServices.CoaAllocationGetAllServiceBind();
			if (dtAccountList.Rows.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("AccountTitle");
			for (int i = 0; i < dtAccountList.Rows.Count; i++)
			{
				if (Convert.ToInt32(dtAccountList.Rows[i]["AccountTypeId"]) != 2 && Convert.ToInt32(dtAccountList.Rows[i]["AccountTypeId"]) != 4 && Convert.ToInt32(dtAccountList.Rows[i]["AccountTypeId"]) != 10 && Convert.ToInt32(dtAccountList.Rows[i]["AccountTypeId"]) != 11 && Convert.ToInt32(dtAccountList.Rows[i]["AccountTypeId"]) != 12 && Convert.ToInt32(dtAccountList.Rows[i]["AccountTypeId"]) != 15)
				{
					dt.Rows.Add(dtAccountList.Rows[i]["Id"], dtAccountList.Rows[i]["AccountTitle"]);
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbTransport, "Id", "AccountTitle", "Supplier Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void VehicleTypesFill()
	{
		try
		{
			DataTable dtVehicle = VehicleType.GetAll();
			if (dtVehicle.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtVehicle, CmbVehicleType, "Id", "VehicleDescription", "VehicleDescription", false);
				((UltraGridBase)CmbVehicleType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ItemNameFill()
	{
		try
		{
			dtitem.Rows.Clear();
			int PurchaseOrderId = Conversion.ToInt(CmbOrderNo.Value);
			dtitem = PurchaseOrder.ReadByPurchaseOrderIDNOrderItemId(PurchaseOrderId);
			if (dtitem.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("OrderItemId");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("PODetailId");
				for (int i = 0; i < dtitem.Rows.Count; i++)
				{
					dt.Rows.Add(dtitem.Rows[i]["OrderItemId"], dtitem.Rows[i]["ItemName"], dtitem.Rows[i]["Id"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, CmbItemName, "OrderItemId", "ItemName", "ItemName", ZeroIndex: false);
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[1].Width = 300;
					((UltraGridBase)CmbItemName).Rows[0].Activate();
				}
				CmbCityName.Value = Conversion.ToString(dtitem.Rows[0]["CityId"]);
				txtDeliverTerm.Text = Conversion.ToString(dtitem.Rows[0]["DeliveryTerm"]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ItemsBindWithoutPurchaseOrder()
	{
		try
		{
			DataTable dtitem = CommonServices.AllItemsBindForSteel();
			if (dtitem.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtitem, CmbItemName, "Id", "ItemName", "ItemName", false);
				((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[1].Width = 500;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void JobLotFillWithoutOrder()
	{
		try
		{
			DataTable lst = CommonServices.JobLotGetAllService();
			if (lst.Rows.Count > 0)
			{
				DDL.BindDDLNew(lst, CmbJobLot, "Id", "JobLotDescription", "JobLot Description", false);
				((UltraGridBase)CmbJobLot).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void PackingTypeFill()
	{
		try
		{
			DataTable dtpkt = CommonServices.InvPackingTypeGetAllService();
			if (dtpkt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtpkt, CmbPackingType, "Id", "PackTypeDesc", "Packing Type", false);
				((UltraGridBase)CmbPackingType).DisplayLayout.Bands[0].Columns[1].Width = 170;
				((UltraGridBase)CmbPackingType).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void PackUOMFillWithoutOrder()
	{
		try
		{
			string Packuom = ((Control)(object)CmbPackUom).Text;
			((Control)(object)CmbPackUom).Text = string.Empty;
			DataTable dtUom = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(CmbItemName.Value));
			if (dtUom.Rows.Count > 0)
			{
				DDL.BindDDL(dtUom, CmbPackUom, "Id", "UOMCode", "PackUOM", ZeroIndex: false);
				((UltraGridBase)CmbPackUom).DisplayLayout.Bands[0].Columns[1].Width = 80;
				((UltraGridBase)CmbPackUom).DisplayLayout.Bands[0].Columns[2].Width = 90;
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Packuom))
				{
					((Control)(object)CmbPackUom).Text = Packuom;
				}
				else
				{
					((Control)(object)CmbPackUom).Text = string.Empty;
				}
			}
			else
			{
				((Control)(object)CmbPackUom).Text = string.Empty;
				((UltraGridBase)CmbPackUom).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void WarehouseFill()
	{
		try
		{
			dtwh.Rows.Clear();
			dtwh = CommonServices.getActiveWareHouse();
			if (dtwh.Rows.Count > 0)
			{
				DDL.BindDDL(dtwh, CmbWarehouse, "Id", "WareHouseName", "Godown Name", ZeroIndex: false);
				((UltraGridBase)CmbWarehouse).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)CmbWarehouse).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CityFill()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtcity.Rows.Clear();
			City obj = new City();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtcity = City.GetAll(obj);
			if (dtcity.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtcity, CmbCityName, "Id", "CityName", "City Name", false);
				((UltraGridBase)CmbCityName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void TransporterAccountDisable()
	{
		try
		{
			if (FreightOnCash > 0)
			{
				List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "FreightInwardAc").ToList();
				if (list.Count > 0)
				{
					int TransporterId = Conversion.ToInt((object)list[0].ConfigKey.ToString());
					CmbTransport.Value = TransporterId;
				}
			}
			else
			{
				((Control)(object)CmbTransport).Enabled = true;
			}
			if (FreightId > 0)
			{
				((Control)(object)CmbTransport).Enabled = false;
				txtCarrAmount.Enabled = false;
			}
			else
			{
				txtCarrAmount.Enabled = true;
				((Control)(object)CmbTransport).Enabled = true;
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
			if (Conversion.ToInt((object)CmbGatePass.Text) <= 0)
			{
				return;
			}
			PackUOMFillWithoutOrder();
			if (((UltraGridBase)CmbOrderNo).ActiveRow != null)
			{
				if (grd.GetRows().Count() == 0)
				{
					if (txtDeliverTerm.Text == "Load" || txtDeliverTerm.Text == "Load & PartyWeight" || txtDeliverTerm.Text == "Ponch & PartyWeight")
					{
						txtGrossWeight.Text = txtSuppWeight.Text.Trim();
						txtGrnWeight.Text = txtSuppWeight.Text.Trim();
					}
					if (txtDeliverTerm.Text == "Load & FactoryWeight" || txtDeliverTerm.Text == "Ponch & FactoryWeight")
					{
						txtGrossWeight.Text = txtFactoryWeight.Text.Trim();
						txtGrnWeight.Text = txtFactoryWeight.Text.Trim();
					}
					if (txtDeliverTerm.Text == "Ponch")
					{
						double SupplierWeight = double.Parse(txtSuppWeight.Text.Trim());
						double FactoryWeight = double.Parse(txtFactoryWeight.Text.Trim());
						if (SupplierWeight > FactoryWeight)
						{
							txtGrnWeight.Text = FactoryWeight.ToString();
							txtGrossWeight.Text = FactoryWeight.ToString();
						}
						else
						{
							txtGrnWeight.Text = SupplierWeight.ToString();
							txtGrossWeight.Text = SupplierWeight.ToString();
						}
					}
				}
				DataTable resultRows = (from row in dtitem.AsEnumerable()
					where row.Field<int>("OrderItemId") == Conversion.ToInt(CmbItemName.Value)
					select row).CopyToDataTable();
				if (resultRows.Rows.Count > 0)
				{
					CmbPackUom.Value = resultRows.Rows[0]["OrderItemUOMId"].ToString();
					CmbJobLot.Value = resultRows.Rows[0]["JobLotId"].ToString();
				}
			}
			else
			{
				if (grd.GetRows().Count() == 0)
				{
					decimal SupplierWeight2 = default(decimal);
					if (txtSuppWeight.Text.Trim() != string.Empty)
					{
						SupplierWeight2 = Conversion.ToDecimal((object)txtSuppWeight.Text.Trim());
					}
					if (txtFactoryWeight.Text.Trim() != string.Empty)
					{
						Conversion.ToDecimal((object)txtFactoryWeight.Text.Trim());
					}
					if (txtDeliverTerm.Text == "Load")
					{
						txtGrnWeight.Text = SupplierWeight2.ToString();
						txtGrossWeight.Text = SupplierWeight2.ToString();
					}
				}
				if (RefDocumentTypeId == 106)
				{
					if (dtLabInfo.Rows.Count > 0 && ((UltraGridBase)CmbItemName).ActiveRow != null)
					{
						DataTable selectedTable = (from r in dtLabInfo.AsEnumerable()
							where r.Field<int>("ItemId") == Conversion.ToInt(CmbItemName.Value)
							select r).CopyToDataTable();
						txtWeighttCut.Text = Conversion.ToString(selectedTable.Rows[0]["DeductionWeight"]);
						txtAnaylstName.Text = Conversion.ToString(selectedTable.Rows[0]["AnalystName"]);
						txtDeductionKg.Text = Conversion.ToString(selectedTable.Rows[0]["DeductionWeight"]);
						txtLabNo.Text = Conversion.ToString(selectedTable.Rows[0]["DocNo"]);
					}
					else
					{
						txtWeighttCut.Text = "0";
						txtAnaylstName.Text = "0";
						txtDeductionKg.Text = "0";
						txtLabNo.Text = string.Empty;
					}
				}
			}
			if (updateDetailIndex < 0 && grd.GetRows().Count() > 0)
			{
				decimal gross = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["GrossWeight"], (AggregateFunction)2));
				gross = Conversion.ToDecimal((object)txtGrnWeight.Text) - gross;
				if (gross == 0m)
				{
					txtGrossWeight.Text = "0";
				}
				else
				{
					txtGrossWeight.Text = gross.ToString();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void comborderno_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbOrderNo).ActiveRow != null)
			{
				ItemNameFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void combgatepass_Leave(object sender, EventArgs e)
	{
		try
		{
			if (btnSave.Visible && btnSave.Enabled)
			{
				GatePassRecordFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CityDefine_Click(object sender, EventArgs e)
	{
		new DefineCity(UserAccount).Show();
	}

	private void reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			lstRemoveRecord.Clear();
			IsApproved = false;
			updateDetailIndex = -1;
			CmbSupplier.ReadOnly = true;
			txtSuppWeight.ReadOnly = true;
			((Control)(object)CmbTransport).Enabled = true;
			txtCarrAmount.Enabled = true;
			GPID = 0;
			txtGrnWeight.Text = string.Empty;
			((Control)(object)CmbSupplier).Text = string.Empty;
			((Control)(object)combstockprty).Text = string.Empty;
			((Control)(object)combrefprty).Text = string.Empty;
			txtAnaylstName.Text = string.Empty;
			txtDeductionKg.Text = string.Empty;
			((Control)(object)CmbTransport).Text = string.Empty;
			txtCarrAmount.Text = string.Empty;
			txtSuppWeight.Text = string.Empty;
			txtFactoryWeight.Text = string.Empty;
			txtReceivedWeight.Text = string.Empty;
			txtBalWeight.Text = string.Empty;
			CmbGatePass.Text = string.Empty;
			((UltraGridBase)CmbVehicleType).Rows[0].Activate();
			txtVehicleNo.Text = string.Empty;
			txtBiltyNo.Text = string.Empty;
			txtRemarks.Text = string.Empty;
			((UltraGridBase)CmbOrderNo).DataSource = null;
			((Control)(object)CmbOrderNo).Text = string.Empty;
			((UltraGridBase)CmbItemName).DataSource = null;
			((Control)(object)CmbItemName).Text = string.Empty;
			txtqty.Text = string.Empty;
			txtAddLessWeight.Text = string.Empty;
			txtNetWeight.Text = string.Empty;
			txtStockWeight.Text = string.Empty;
			txtLabNo.Text = string.Empty;
			((Control)(object)CmbCityName).Text = string.Empty;
			txtWeighttCut.Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			txtDeliverTerm.Text = string.Empty;
			txtGrossWeight.Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			((UltraGridBase)CmbPackUom).DataSource = null;
			btnSave.Visible = true;
			DocDate.Focus();
			dtdetail.Rows.Clear();
			dtLabInfo.Rows.Clear();
			grd.DataSource = dtdetail;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			Add.Visible = true;
			((Control)(object)CmbOrderNo).Enabled = true;
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			GenerateCode();
			Gatepass();
			SupplierNameFilll();
			defaultConfiquration();
			txtGrossWeight.Enabled = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ResetDetial()
	{
		try
		{
			txtqty.Text = string.Empty;
			txtGrossWeight.Text = string.Empty;
			txtAddLessWeight.Text = string.Empty;
			txtNetWeight.Text = string.Empty;
			txtStockWeight.Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			txtLabNo.Text = string.Empty;
			CmbItemName.Focus();
			updateDetailIndex = -1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		reset();
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		try
		{
			GetConfiguration();
			SupplierNameFilll();
			TransportFill();
			VehicleTypesFill();
			JobLotFillWithoutOrder();
			PackingTypeFill();
			WarehouseFill();
			CityFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Total()
	{
		try
		{
			decimal Qty = default(decimal);
			decimal GrossWeight = default(decimal);
			decimal WeightCut = default(decimal);
			decimal WeightCutTotal = default(decimal);
			decimal AddLess = default(decimal);
			decimal FactoryWeight = default(decimal);
			decimal SupplierWeight = default(decimal);
			if (txtFactoryWeight.Text != string.Empty && txtFactoryWeight.Text != null)
			{
				FactoryWeight = Conversion.ToDecimal((object)txtFactoryWeight.Text.Trim().ToString());
			}
			if (txtSuppWeight.Text != string.Empty && txtSuppWeight.Text != null)
			{
				SupplierWeight = Conversion.ToDecimal((object)txtSuppWeight.Text.Trim().ToString());
			}
			if (((Control)(object)CmbPackUom).Text != string.Empty)
			{
				Conversion.ToDecimal(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value);
			}
			if (txtqty.Text != string.Empty && txtqty.Text != null)
			{
				Qty = Conversion.ToDecimal((object)txtqty.Text.Trim().ToString());
			}
			if (txtGrossWeight.Text != string.Empty && txtGrossWeight.Text != null)
			{
				GrossWeight = Conversion.ToDecimal((object)txtGrossWeight.Text.Trim().ToString());
			}
			if (txtWeighttCut.Text != string.Empty && txtWeighttCut.Text != null)
			{
				WeightCut = Conversion.ToDecimal((object)txtWeighttCut.Text.Trim().ToString());
				txtWeighttCutTotal.Text = (Qty * WeightCut).ToString("#,##0.##");
			}
			else
			{
				txtWeighttCutTotal.Text = "0";
			}
			if (txtWeighttCutTotal.Text != string.Empty && txtWeighttCutTotal.Text != null)
			{
				WeightCutTotal = Conversion.ToDecimal((object)txtWeighttCutTotal.Text.Trim().ToString());
			}
			AddLess = ((!(txtAddLessWeight.Text != string.Empty) || txtAddLessWeight.Text == null) ? default(decimal) : Conversion.ToDecimal((object)txtAddLessWeight.Text.Trim()));
			if (txtNetWeight.Text != string.Empty && txtNetWeight.Text != null)
			{
				Conversion.ToDecimal((object)txtNetWeight.Text.Trim().ToString());
			}
			decimal Weight = default(decimal);
			Weight = GrossWeight - WeightCutTotal + AddLess;
			txtNetWeight.Text = Math.Round(Weight, 2).ToString("#,##0.##");
			if (!(SupplierWeight > 0m) || !(FactoryWeight > 0m) || !(GrossWeight > 0m))
			{
				return;
			}
			if (txtDeliverTerm.Text == "Load" || txtDeliverTerm.Text == "Load & PartyWeight" || txtDeliverTerm.Text == "Ponch & PartyWeight")
			{
				decimal StockWt4 = FactoryWeight * GrossWeight / SupplierWeight;
				StockWt4 -= WeightCutTotal;
				txtStockWeight.Text = Math.Round(StockWt4, 2).ToString("#,##0.##");
			}
			if (txtDeliverTerm.Text == "Ponch")
			{
				if (SupplierWeight > FactoryWeight)
				{
					decimal StockWt2 = FactoryWeight * GrossWeight / FactoryWeight;
					StockWt2 -= WeightCutTotal;
					txtStockWeight.Text = Math.Round(StockWt2, 2).ToString("#,##0.##");
				}
				else
				{
					decimal StockWt3 = FactoryWeight * GrossWeight / SupplierWeight;
					StockWt3 -= WeightCutTotal;
					txtStockWeight.Text = Math.Round(StockWt3, 2).ToString("#,##0.##");
				}
			}
			if (txtDeliverTerm.Text == "Ponch & FactoryWeight" || txtDeliverTerm.Text == "Load & FactoryWeight")
			{
				decimal StockWt = FactoryWeight * GrossWeight / FactoryWeight;
				StockWt -= WeightCutTotal;
				txtStockWeight.Text = Math.Round(StockWt, 2).ToString("#,##0.##");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void TotalSupplierWeight()
	{
		try
		{
			if (RefDocumentTypeId != 105)
			{
				if (txtSuppWeight.Text != string.Empty && txtFactoryWeight.Text != string.Empty)
				{
					double num = Conversion.ToDouble((object)txtSuppWeight.Text.Trim());
					double FactoryWeight = Conversion.ToDouble((object)txtFactoryWeight.Text.Trim());
					txtBalWeight.Text = (num - FactoryWeight).ToString();
				}
			}
			else if (txtSuppWeight.Text != string.Empty && txtFactoryWeight.Text != string.Empty)
			{
				double num2 = Conversion.ToDouble((object)txtSuppWeight.Text.Trim());
				double ReceivedWeight = Conversion.ToDouble((object)txtReceivedWeight.Text.Trim());
				txtBalWeight.Text = (num2 - ReceivedWeight).ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtqty_TextChanged(object sender, EventArgs e)
	{
		Total();
	}

	private void txtebu_TextChanged(object sender, EventArgs e)
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
		txtAddLessWeight.Text = Conversion.ToDecimal((object)txtAddLessWeight.Text).ToString("#,##0.####");
		Total();
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

	private void txtsuppwt_TextChanged(object sender, EventArgs e)
	{
		TotalSupplierWeight();
	}

	private void txtfctwt_TextChanged(object sender, EventArgs e)
	{
		TotalSupplierWeight();
	}

	private void combpckuom_Leave(object sender, EventArgs e)
	{
		Total();
	}

	private void txtadlswt_TextChanged_1(object sender, EventArgs e)
	{
		Total();
	}

	private void txtqty_Leave(object sender, EventArgs e)
	{
	}

	private void txtgwt_Leave(object sender, EventArgs e)
	{
		txtGrossWeight.Text = Conversion.ToDecimal((object)txtGrossWeight.Text).ToString("#,##0.##");
	}

	private void Add_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				double num = Conversion.ToDouble((object)txtNetWeight.Text.Trim());
				double GrossWeight = Conversion.ToDouble((object)txtGrossWeight.Text.Trim());
				if (num > GrossWeight)
				{
					throw new Exception("NetWeight cannot greater than Gross Weight Please check");
				}
				int OrderDetailId = 0;
				((Control)(object)CmbOrderNo).Enabled = false;
				if (RefDocumentTypeId == 1500)
				{
					OrderDetailId = Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells[2].Value);
				}
				dtdetail.Rows.Add(0, OrderDetailId, CmbOrderNo.Value, ((Control)(object)CmbOrderNo).Text, CmbItemName.Value, ((Control)(object)CmbItemName).Text, CmbJobLot.Value, ((Control)(object)CmbJobLot).Text, CmbPackingType.Value, ((Control)(object)CmbPackingType).Text, ((Control)(object)CmbPackUom).Text.Trim(), CmbPackUom.Value, Conversion.ToDouble((object)txtqty.Text.Trim()), Conversion.ToDouble((object)txtGrossWeight.Text.Trim()), Conversion.ToDouble((object)txtWeighttCut.Text.Trim()), Conversion.ToDouble((object)txtWeighttCutTotal.Text.Trim()), Conversion.ToDouble((object)txtAddLessWeight.Text.Trim()), Conversion.ToDouble((object)txtNetWeight.Text.Trim()), Conversion.ToDouble((object)txtStockWeight.Text.Trim()), CmbWarehouse.Value, txtLabNo.Text, CmbCityName.Value);
				grd.DataSource = dtdetail;
				grd.RetrieveStructure();
				grdSettings();
				ResetDetial();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		ResetDetial();
		Add.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelUpdateDetial.Visible = false;
	}

	private void grdSettings()
	{
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["OrderDetailId"].Visible = false;
			grd.RootTable.Columns["OrderId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["JobLotId"].Visible = false;
			grd.RootTable.Columns["PackingTypeId"].Visible = false;
			grd.RootTable.Columns["PackUomId"].Visible = false;
			grd.RootTable.Columns["Warehouse"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["Warehouse"].HasValueList = true;
			grd.RootTable.Columns["Warehouse"].LimitToList = true;
			grd.RootTable.Columns["Warehouse"].EditType = (EditType)4;
			grd.RootTable.Columns["Warehouse"].Caption = "WareHouse Name";
			grd.RootTable.Columns["Warehouse"].ValueList.PopulateValueList((IEnumerable)dtwh.DefaultView, "Id", "WareHouseName");
			grd.RootTable.Columns["City"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["City"].HasValueList = true;
			grd.RootTable.Columns["City"].LimitToList = true;
			grd.RootTable.Columns["City"].EditType = (EditType)4;
			grd.RootTable.Columns["City"].Caption = "City Name";
			grd.RootTable.Columns["City"].ValueList.PopulateValueList((IEnumerable)dtcity.DefaultView, "Id", "CityName");
			grd.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["WtCut"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["WtCutTotal"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["AddLesswt"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["WtCut"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["WtCutTotal"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AddLesswt"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grd.RootTable.Columns["GrossWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["WtCut"].FormatString = "#,##0.##";
			grd.RootTable.Columns["WtCutTotal"].FormatString = "#,##0.##";
			grd.RootTable.Columns["AddLesswt"].FormatString = "#,##0.##";
			grd.RootTable.Columns["NetWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["StockWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["WtCut"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["WtCutTotal"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["AddLesswt"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["StockWeight"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustment(AryColWidth, grd);
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "X";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
			grd.RootTable.Columns["Delete"].Width = 20;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!(e.Column.Key == "Delete") || MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			GridEXRow item = grd.CurrentRow;
			if (btnSave.Visible && btnSave.Enabled)
			{
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
			}
			else if (Conversion.ToInt(item.Cells["Id"].Value) > 0)
			{
				InvGrnDetail grnDetail = new InvGrnDetail();
				grnDetail.Id = Conversion.ToInt(item.Cells["Id"].Value);
				grnDetail.PurchaseOrderDetailId = Conversion.ToInt(item.Cells["OrderDetailId"].Value);
				grnDetail.PurchaseOrderId = Conversion.ToInt(item.Cells["OrderId"].Value);
				grnDetail.PurchaserOrderNo = Conversion.ToInt(item.Cells["OrderNo"].Value);
				grnDetail.ItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
				grnDetail.JobLotId = Conversion.ToInt(item.Cells["JobLotId"].Value);
				grnDetail.PackingTypeId = Conversion.ToInt(item.Cells["PackingTypeId"].Value);
				grnDetail.ItemQty = Conversion.ToDecimal(item.Cells["ItemQty"].Value);
				grnDetail.ItemUomId = Conversion.ToInt(item.Cells["PackUomId"].Value);
				grnDetail.GrossWeight = Conversion.ToDecimal(item.Cells["GrossWeight"].Value);
				grnDetail.WtCut = Conversion.ToDecimal(item.Cells["WtCut"].Value);
				grnDetail.WtCutTotal = Conversion.ToDecimal(item.Cells["WtCutTotal"].Value);
				grnDetail.AdLsWeight = Conversion.ToDecimal(item.Cells["AddLesswt"].Value);
				grnDetail.NetBillWeight = Conversion.ToDecimal(item.Cells["NetWeight"].Value);
				grnDetail.StockWeight = Conversion.ToDecimal(item.Cells["StockWeight"].Value);
				grnDetail.WarehouseId = Conversion.ToInt(item.Cells["Warehouse"].Value);
				grnDetail.LabReportRef = Conversion.ToString((object)item.Cells["LabNo"].Text);
				grnDetail.CityId = Conversion.ToInt(item.Cells["City"].Value);
				grnDetail.ActionTypeId = 3;
				lstRemoveRecord.Add(grnDetail);
				dtdetail.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				grd.UpdateData();
			}
			else if (item != null && (int)item.RowType == 0)
			{
				item.Delete();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item == null)
			{
				return;
			}
			updateDetailIndex = item.RowIndex;
			if (RefDocumentTypeId == 1500)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("OrderId");
				dt.Columns.Add("OrderNo");
				dt.Clear();
				dt.Rows.Add(item.Cells["OrderId"].Value.ToString(), item.Cells["OrderNo"].Text.ToString());
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, CmbOrderNo, "OrderId", "OrderNo", "Order No", ZeroIndex: false);
					((UltraGridBase)CmbOrderNo).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
					((UltraGridBase)CmbOrderNo).Rows[0].Activate();
					comborderno_Leave(null, null);
				}
			}
			else
			{
				ItemsBindWithoutPurchaseOrder();
			}
			CmbItemName.Value = item.Cells["ItemId"].Value.ToString();
			CmbJobLot.Value = item.Cells["JobLotId"].Value.ToString();
			CmbPackingType.Value = item.Cells["PackingTypeId"].Value.ToString();
			txtqty.Text = item.Cells["ItemQty"].Value.ToString();
			CmbPackUom.Value = item.Cells["PackUomId"].Value.ToString();
			txtGrossWeight.Text = Conversion.ToDecimal(item.Cells["GrossWeight"].Value).ToString("#,##0.##");
			txtWeighttCut.Text = item.Cells["WtCut"].Value.ToString();
			txtWeighttCutTotal.Text = Conversion.ToDecimal(item.Cells["WtCutTotal"].Value).ToString("#,##0.##");
			txtAddLessWeight.Text = item.Cells["AddLesswt"].Value.ToString();
			txtNetWeight.Text = Conversion.ToDecimal(item.Cells["NetWeight"].Value).ToString("#,##0.##");
			txtStockWeight.Text = Conversion.ToDecimal(item.Cells["StockWeight"].Value).ToString("#,##0.##");
			CmbWarehouse.Value = item.Cells["Warehouse"].Value.ToString();
			txtLabNo.Text = item.Cells["LabNo"].Value.ToString();
			((Control)(object)CmbCityName).Text = item.Cells["City"].Text.ToString();
			Add.Visible = false;
			btnUpdateDetail.Visible = true;
			btnCancelUpdateDetial.Visible = true;
			CmbItemName.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				Conversion.ToDecimal((object)txtStockWeight.Text.Trim());
				decimal num = Conversion.ToDecimal((object)txtNetWeight.Text.Trim());
				decimal GrossWeight = Conversion.ToDecimal((object)txtGrnWeight.Text.Trim());
				if (num > GrossWeight)
				{
					throw new Exception("NetWeight cannot greater than Gross Weight Please check");
				}
				if (RefDocumentTypeId == 1500)
				{
					dtdetail.Rows[updateDetailIndex]["OrderDetailId"] = ((UltraDropDownBase)CmbItemName).SelectedRow.Cells[2].Value;
				}
				dtdetail.Rows[updateDetailIndex]["OrderId"] = CmbOrderNo.Value;
				dtdetail.Rows[updateDetailIndex]["OrderNo"] = ((Control)(object)CmbOrderNo).Text;
				dtdetail.Rows[updateDetailIndex]["ItemId"] = CmbItemName.Value;
				dtdetail.Rows[updateDetailIndex]["ItemName"] = ((Control)(object)CmbItemName).Text;
				dtdetail.Rows[updateDetailIndex]["JobLotId"] = CmbJobLot.Value;
				dtdetail.Rows[updateDetailIndex]["JobLot"] = ((Control)(object)CmbJobLot).Text;
				dtdetail.Rows[updateDetailIndex]["PackingTypeId"] = CmbPackingType.Value;
				dtdetail.Rows[updateDetailIndex]["PackingType"] = ((Control)(object)CmbPackingType).Text;
				dtdetail.Rows[updateDetailIndex]["ItemQty"] = txtqty.Text;
				dtdetail.Rows[updateDetailIndex]["PackUom"] = ((Control)(object)CmbPackUom).Text;
				dtdetail.Rows[updateDetailIndex]["PackUomId"] = CmbPackUom.Value;
				dtdetail.Rows[updateDetailIndex]["GrossWeight"] = Conversion.ToDecimal((object)txtGrossWeight.Text.Trim());
				dtdetail.Rows[updateDetailIndex]["WtCut"] = Conversion.ToDouble((object)txtWeighttCut.Text.Trim());
				dtdetail.Rows[updateDetailIndex]["WtCutTotal"] = Conversion.ToDouble((object)txtWeighttCutTotal.Text.Trim());
				dtdetail.Rows[updateDetailIndex]["AddLesswt"] = Conversion.ToDouble((object)txtAddLessWeight.Text.Trim());
				dtdetail.Rows[updateDetailIndex]["NetWeight"] = Conversion.ToDecimal((object)txtNetWeight.Text.Trim());
				dtdetail.Rows[updateDetailIndex]["StockWeight"] = Conversion.ToDecimal((object)txtStockWeight.Text.Trim());
				dtdetail.Rows[updateDetailIndex]["Warehouse"] = CmbWarehouse.Value;
				dtdetail.Rows[updateDetailIndex]["LabNo"] = txtLabNo.Text;
				dtdetail.Rows[updateDetailIndex]["City"] = CmbCityName.Value;
				Add.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				ResetDetial();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			grn.BranchesId = UserAccount.BranchesId;
			grn.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			grn.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			grn.NoOfRecords = NoOfRecords;
			grn.DocumentTypeId = 1501;
			if (!grn.CanViewAllRecord)
			{
				grn.EntryUser = UserAccount.ID;
			}
			DataTable dthis = new DataTable();
			dthis = InvGrn.FormHistory(grn);
			if (dthis.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("GpId");
				dt.Columns.Add("GpNo", typeof(int));
				dt.Columns.Add("SupplierName");
				dt.Columns.Add("Transporter");
				dt.Columns.Add("FreightAmount", typeof(double));
				dt.Columns.Add("TotalQty", typeof(double));
				dt.Columns.Add("PartyWeight", typeof(double));
				dt.Columns.Add("FactoryWeight", typeof(double));
				dt.Columns.Add("VehicleType");
				dt.Columns.Add("VehicleNo");
				dt.Columns.Add("BiltyNo");
				dt.Columns.Add("DeliveryTerm");
				dt.Columns.Add("EntryUser");
				dt.Columns.Add("ModifyUser");
				dt.Columns.Add("IsApproved");
				dt.Columns.Add("Remarks");
				dt.Columns.Add("NoOfAttachments");
				for (int i = 0; i < dthis.Rows.Count; i++)
				{
					dt.Rows.Add(dthis.Rows[i]["Id"], dthis.Rows[i]["DocNo"], Conversion.ToDateTime(dthis.Rows[i]["DocDate"]).ToShortDateString(), dthis.Rows[i]["InwardGatePassId"], dthis.Rows[i]["GpNo"], dthis.Rows[i]["SupplierName"], dthis.Rows[i]["TranspoerName"], dthis.Rows[i]["CarriageAmount"], dthis.Rows[i]["TotalQty"], dthis.Rows[i]["PartyWeight"], dthis.Rows[i]["FactoryWeight"], dthis.Rows[i]["VehicleDescription"], dthis.Rows[i]["VehicleNo"], dthis.Rows[i]["BiltyNo"], dthis.Rows[i]["DeliveryTerm"], dthis.Rows[i]["EntryUserName"], dthis.Rows[i]["ModifyUserName"], Conversion.ToBool(dthis.Rows[i]["IsApproved"]) ? "Approved" : "Not Approved", dthis.Rows[i]["RemarksHeader"], dthis.Rows[i]["NoOfAttachments"]);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HistoryGridSettings()
	{
		try
		{
			GrdHistory.RootTable.Columns["Id"].Visible = false;
			GrdHistory.RootTable.Columns["GpId"].Visible = false;
			GrdHistory.RootTable.Columns["GpNo"].ColumnType = (ColumnType)5;
			GrdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			GrdHistory.RootTable.Columns["FreightAmount"].AggregateFunction = (AggregateFunction)2;
			GrdHistory.RootTable.Columns["FreightAmount"].TextAlignment = (TextAlignment)3;
			GrdHistory.RootTable.Columns["FreightAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GrdHistory.RootTable.Columns["FreightAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GrdHistory.RootTable.Columns["TotalQty"].AggregateFunction = (AggregateFunction)2;
			GrdHistory.RootTable.Columns["TotalQty"].TextAlignment = (TextAlignment)3;
			GrdHistory.RootTable.Columns["TotalQty"].FormatString = "#,##0.##";
			GrdHistory.RootTable.Columns["TotalQty"].TotalFormatString = "#,##0.##";
			GrdHistory.RootTable.Columns["PartyWeight"].AggregateFunction = (AggregateFunction)2;
			GrdHistory.RootTable.Columns["PartyWeight"].TextAlignment = (TextAlignment)3;
			GrdHistory.RootTable.Columns["PartyWeight"].FormatString = "#,##0.##";
			GrdHistory.RootTable.Columns["PartyWeight"].TotalFormatString = "#,##0.##";
			GrdHistory.RootTable.Columns["FactoryWeight"].AggregateFunction = (AggregateFunction)2;
			GrdHistory.RootTable.Columns["FactoryWeight"].TextAlignment = (TextAlignment)3;
			GrdHistory.RootTable.Columns["FactoryWeight"].FormatString = "#,##0.##";
			GrdHistory.RootTable.Columns["FactoryWeight"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustment(AryColWidth, GrdHistory);
			GrdHistory.RootTable.Columns.Add("Edit");
			GrdHistory.RootTable.Columns["Edit"].Caption = "Edit";
			GrdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			GrdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			GrdHistory.RootTable.Columns["Edit"].Width = 40;
			GrdHistory.RootTable.Columns.Add("Print");
			GrdHistory.RootTable.Columns["Print"].Caption = "Print";
			GrdHistory.RootTable.Columns["Print"].ButtonText = "Print";
			GrdHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			GrdHistory.RootTable.Columns["Print"].Width = 40;
			GrdHistory.RootTable.Columns["Print"].Position = 0;
			GrdHistory.RootTable.Columns["Edit"].Position = 1;
			GrdHistory.FrozenColumns = 2;
			if (WagesStatus)
			{
				GrdHistory.RootTable.Columns.Add("WagesView");
				GrdHistory.RootTable.Columns["WagesView"].Caption = "WagesView";
				GrdHistory.RootTable.Columns["WagesView"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				GrdHistory.RootTable.Columns["WagesView"].ButtonStyle = (ButtonStyle)5;
				GrdHistory.RootTable.Columns["WagesView"].ButtonText = "WagesEdit";
				GrdHistory.RootTable.Columns["WagesView"].Width = 80;
			}
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GrdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				reset();
				Id = Conversion.ToInt((object)GrdHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(Id);
			}
			if (e.Column.Key == "Print")
			{
				CommonServices.GrnSlipSteelWithSubReports(Conversion.ToInt(GrdHistory.CurrentRow.Cells["Id"].Value));
			}
			if (e.Column.Key == "WagesView" && WagesStatus)
			{
				frmwagesBillHeader frmwagesBillHeader = new frmwagesBillHeader(UserAccount);
				frmwagesBillHeader.RefDocTypeId = 1501;
				frmwagesBillHeader.RefDocId = Conversion.ToInt(GrdHistory.CurrentRow.Cells["Id"].Value);
				frmwagesBillHeader.ShowDialog();
				frmwagesBillHeader.BringToFront();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void gridhistorydetail()
	{
		try
		{
			GrdHistoryDetail.RootTable.Columns["OrderId"].Visible = false;
			GrdHistoryDetail.RootTable.Columns["OrderNo"].Width = 70;
			GrdHistoryDetail.RootTable.Columns["ItemName"].Width = 200;
			GrdHistoryDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["GrossWeight"].FormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["WtCut"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["WtCut"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["WtCut"].FormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["WtCut"].TotalFormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["WtCutTotal"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["WtCutTotal"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["WtCutTotal"].FormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["WtCutTotal"].TotalFormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["AddLesswt"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["AddLesswt"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["AddLesswt"].FormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["AddLesswt"].TotalFormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["NetWeight"].FormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["StockWeight"].FormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["StockWeight"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustment(AryColWidth, GrdHistoryDetail);
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

	private void GrdHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			Id = Conversion.ToInt((object)GrdHistory.CurrentRow.Cells[1].Value.ToString());
			ReadById(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
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
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt((object)item.Cells["Id"].Value.ToString()), 1501);
			}
			if (e.Column.Key == "WagesNo")
			{
				CommonServices.ContractorWagesBill_SlipandRegister_002(Conversion.ToInt(item.Cells["WagesId"].Value));
			}
			if (e.Column.Key == "GpNo")
			{
				CommonServices.GatePassInwardSlipAndRegisterReport(Conversion.ToInt(item.Cells["GpId"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GrdHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (GrdHistory.CurrentRow != null && (int)GrdHistory.CurrentRow.RowType == 0 && GrdHistory.CurrentRow.Cells.Count > 0)
			{
				InvGrn invGrn = InvGrn.GetByID(Conversion.ToInt(GrdHistory.CurrentRow.Cells["Id"].Value));
				DataTable dthistorydetail = new DataTable();
				dthistorydetail.Columns.Add("OrderId");
				dthistorydetail.Columns.Add("OrderNo");
				dthistorydetail.Columns.Add("ItemName");
				dthistorydetail.Columns.Add("JobLot");
				dthistorydetail.Columns.Add("PackingType");
				dthistorydetail.Columns.Add("PackUom");
				dthistorydetail.Columns.Add("ItemQty", typeof(double));
				dthistorydetail.Columns.Add("GrossWeight", typeof(double));
				dthistorydetail.Columns.Add("WtCut", typeof(double));
				dthistorydetail.Columns.Add("WtCutTotal", typeof(double));
				dthistorydetail.Columns.Add("AddLesswt", typeof(double));
				dthistorydetail.Columns.Add("NetWeight", typeof(double));
				dthistorydetail.Columns.Add("StockWeight", typeof(double));
				dthistorydetail.Columns.Add("WareHouse");
				dthistorydetail.Columns.Add("LabNo");
				dthistorydetail.Columns.Add("CityName");
				dthistorydetail.Clear();
				for (int i = 0; i < invGrn.invGrnDetail.Count; i++)
				{
					dthistorydetail.Rows.Add(invGrn.invGrnDetail[i].PurchaseOrderId, invGrn.invGrnDetail[i].PurchaserOrderNo, invGrn.invGrnDetail[i].ItemName, invGrn.invGrnDetail[i].JobLotDescription, invGrn.invGrnDetail[i].PackingType, invGrn.invGrnDetail[i].UOMCode, invGrn.invGrnDetail[i].ItemQty, invGrn.invGrnDetail[i].GrossWeight, invGrn.invGrnDetail[i].WtCut, invGrn.invGrnDetail[i].WtCutTotal, invGrn.invGrnDetail[i].AdLsWeight, invGrn.invGrnDetail[i].NetBillWeight, invGrn.invGrnDetail[i].StockWeight, invGrn.invGrnDetail[i].WareHouseName, invGrn.invGrnDetail[i].LabReportRef, invGrn.invGrnDetail[i].CityName);
				}
				GrdHistoryDetail.DataSource = dthistorydetail;
				GrdHistoryDetail.RetrieveStructure();
				gridhistorydetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GenerateReport(int PrintId)
	{
		try
		{
			CommonServices.GrnSlipSteelWithSubReports(PrintId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Gatepass()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		try
		{
			DataTable dtpendingGp = InvGrn.GetPendingGatepassForGrn(new GatePassInward
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GRNGridSetting()
	{
		try
		{
			grdGp.AllowEdit = (InheritableBoolean)2;
			grdGp.AllowDelete = (InheritableBoolean)2;
			grdGp.RootTable.Columns["Id"].Visible = false;
			grdGp.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grdGp.RootTable.Columns["FreightOnCash"].Visible = false;
			grdGp.RootTable.Columns["FreightId"].Visible = false;
			grdGp.RootTable.Columns["GpSrNo"].ColumnType = (ColumnType)5;
			grdGp.RootTable.Columns["VarietyName"].Caption = "Item Name";
			grdGp.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdGp.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdGp.RootTable.Columns["Qty"].FormatString = "#,##0.##";
			grdGp.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			grdGp.RootTable.Columns["SupplierWeight"].AggregateFunction = (AggregateFunction)2;
			grdGp.RootTable.Columns["SupplierWeight"].TextAlignment = (TextAlignment)3;
			grdGp.RootTable.Columns["SupplierWeight"].FormatString = "#,##0.##";
			grdGp.RootTable.Columns["SupplierWeight"].TotalFormatString = "#,##0.##";
			grdGp.RootTable.Columns["FactoryWeight"].AggregateFunction = (AggregateFunction)2;
			grdGp.RootTable.Columns["FactoryWeight"].TextAlignment = (TextAlignment)3;
			grdGp.RootTable.Columns["FactoryWeight"].FormatString = "#,##0.##";
			grdGp.RootTable.Columns["FactoryWeight"].TotalFormatString = "#,##0.##";
			grdGp.RootTable.Columns["NetPaid"].AggregateFunction = (AggregateFunction)2;
			grdGp.RootTable.Columns["NetPaid"].TextAlignment = (TextAlignment)3;
			grdGp.RootTable.Columns["NetPaid"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdGp.RootTable.Columns["NetPaid"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustment(AryColWidth, grdGp);
			grdGp.RootTable.Columns.Add("Load");
			grdGp.RootTable.Columns["Load"].Caption = "Load";
			grdGp.RootTable.Columns["Load"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdGp.RootTable.Columns["Load"].ButtonStyle = (ButtonStyle)5;
			grdGp.RootTable.Columns["Load"].ButtonText = "Load";
			grdGp.RootTable.Columns["Load"].Width = 40;
			grdGp.RootTable.Columns["Load"].Position = 0;
			grdGp.RootTable.Columns["GpSrNo"].Position = 1;
			grdGp.FrozenColumns = 2;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdGp_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (btnSave.Visible && btnSave.Enabled && e.Column.Key == "Load")
			{
				GpQty = 0.0;
				FreightOnCash = 0;
				FreightId = 0;
				if (!(Conversion.ToString(grdGp.CurrentRow.Cells["Status"].Value) == "Accepted"))
				{
					throw new Exception("Status Not Accepted Please check status");
				}
				GPID = Conversion.ToInt(grdGp.CurrentRow.Cells["Id"].Value);
				FreightOnCash = Conversion.ToInt(grdGp.CurrentRow.Cells["FreightOnCash"].Value);
				FreightId = Conversion.ToInt(grdGp.CurrentRow.Cells["FreightId"].Value);
				GpQty = Conversion.ToDouble(grdGp.CurrentRow.Cells["Qty"].Value);
				CmbGatePass.Text = Conversion.ToString(grdGp.CurrentRow.Cells["GpSrNo"].Value);
				txtSuppWeight.ReadOnly = true;
				txtFactoryWeight.ReadOnly = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdGp_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = grdGp.CurrentRow;
			if (item != null && e.Column.Key == "GpSrNo")
			{
				if (Conversion.ToInt(item.Cells["DetailIdsCount"].Value) > 0)
				{
					CommonServices.GatePassInwardWithDetailAndLabReport(Conversion.ToInt(item.Cells["Id"].Value));
				}
				else
				{
					CommonServices.GatePassInwardSlipAndRegisterReport(Conversion.ToInt(item.Cells["Id"].Value));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GatePassRecordFill()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0528: Unknown result type (might be due to invalid IL or missing references)
		//IL_052f: Expected O, but got Unknown
		try
		{
			TransporterAccountDisable();
			RefDocumentTypeId = 0;
			CmbOrderNo.ReadOnly = false;
			DataTable dtgpForGRn = GatePassInward.GetByGpNoForGRN(new GatePassInward
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt((object)GPID),
				DocumentTypeId = 51,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (dtgpForGRn.Rows.Count > 0)
			{
				((Control)(object)CmbVehicleType).Enabled = false;
				txtVehicleNo.Enabled = false;
				txtBiltyNo.Enabled = false;
				GPID = Conversion.ToInt(dtgpForGRn.Rows[0]["Id"]);
				((Control)(object)CmbVehicleType).Text = dtgpForGRn.Rows[0]["VehicleType"].ToString();
				txtVehicleNo.Text = dtgpForGRn.Rows[0]["VehicleNo"].ToString();
				txtBiltyNo.Text = dtgpForGRn.Rows[0]["BiltyNo"].ToString();
				txtRemarks.Text = dtgpForGRn.Rows[0]["OtherRemarks"].ToString();
				txtCarrAmount.Text = Conversion.ToDecimal(dtgpForGRn.Rows[0]["NetPaid"]).ToString(clsGlobalVariables.stringFormatsingle);
				txtqty.Text = dtgpForGRn.Rows[0]["NoOfPackages"].ToString();
				txtSuppWeight.Text = Conversion.ToDecimal(dtgpForGRn.Rows[0]["SupplierWeight"]).ToString("#,##0.##");
				txtFactoryWeight.Text = Conversion.ToDecimal(dtgpForGRn.Rows[0]["FactoryWeight"]).ToString("#,##0.##");
				txtBalWeight.Text = Conversion.ToDecimal(dtgpForGRn.Rows[0]["DifferenceWeight"]).ToString("#,##0.##");
				if (Conversion.ToString(dtgpForGRn.Rows[0]["WareHouseName"]) != "")
				{
					((Control)(object)CmbWarehouse).Text = dtgpForGRn.Rows[0]["WareHouseName"].ToString();
				}
				RefDocumentTypeId = Conversion.ToInt(dtgpForGRn.Rows[0]["RefDocumentTypeId"]);
				txtAnaylstName.Text = dtgpForGRn.Rows[0]["AnalystName"].ToString();
				txtDeductionKg.Text = Conversion.ToDecimal(dtgpForGRn.Rows[0]["DeductionWeight"]).ToString("#,##0.##");
				txtWeighttCut.Text = Conversion.ToDecimal(dtgpForGRn.Rows[0]["DeductionWeight"]).ToString("#,##0.##");
				CmbSupplier.Value = Conversion.ToInt(dtgpForGRn.Rows[0]["SupplierCustomerId"]);
				if (RefDocumentTypeId == 1500)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("PurchaseOrderId");
					dt.Columns.Add("SupplierContractCode");
					dt.Clear();
					for (int i = 0; i < dtgpForGRn.Rows.Count; i++)
					{
						dt.Rows.Add(dtgpForGRn.Rows[i]["PurchaseOrderId"], dtgpForGRn.Rows[i]["SupplierContractCode"]);
					}
					if (dt.Rows.Count > 0)
					{
						DDL.BindDDL(dt, CmbOrderNo, "PurchaseOrderId", "SupplierContractCode", "Order No", ZeroIndex: false);
						((UltraGridBase)CmbOrderNo).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
						((UltraGridBase)CmbOrderNo).Rows[0].Activate();
						comborderno_Leave(null, null);
						CmbOrderNo.Focus();
					}
				}
				else
				{
					if (RefDocumentTypeId != 106)
					{
						return;
					}
					decimal SupplierWeight = default(decimal);
					decimal FactoryWeight = default(decimal);
					decimal BalWeight = default(decimal);
					if (txtSuppWeight.Text.Trim() != string.Empty)
					{
						SupplierWeight = Conversion.ToDecimal((object)txtSuppWeight.Text.Trim());
					}
					if (txtFactoryWeight.Text.Trim() != string.Empty)
					{
						FactoryWeight = Conversion.ToDecimal((object)txtFactoryWeight.Text.Trim());
					}
					if (RefDocumentTypeId == 106)
					{
						if (LabCompulsoryForWeighBridgeAgainstGatePurchase)
						{
							GatePassInward obj = new GatePassInward();
							obj.OrganizationId = UserAccount.OrganizationId;
							obj.CompanyId = UserAccount.CompanyId;
							obj.Id = GPID;
							dtLabInfo = GatePassInward.GetDataByGpId(obj);
							if (dtLabInfo.Rows.Count > 0)
							{
								DDL.BindDDLNew(dtLabInfo, CmbItemName, "ItemId", "ItemName", "ItemName", false);
								((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[1].Width = 500;
							}
							else
							{
								((Control)(object)CmbItemName).Text = string.Empty;
								((UltraGridBase)CmbItemName).DataSource = null;
							}
						}
						else
						{
							ItemsBindWithoutPurchaseOrder();
						}
						txtDeliverTerm.Text = "Ponch";
						if (txtDeliverTerm.Text == "Ponch")
						{
							if (SupplierWeight > FactoryWeight)
							{
								txtGrnWeight.Text = FactoryWeight.ToString();
								BalWeight = FactoryWeight;
							}
							else
							{
								txtGrnWeight.Text = SupplierWeight.ToString();
								BalWeight = SupplierWeight;
							}
							txtBalWeight.Text = BalWeight.ToString();
							txtGrossWeight.Text = BalWeight.ToString();
						}
					}
					CmbSupplier.ReadOnly = false;
					CmbOrderNo.ReadOnly = true;
				}
			}
			else
			{
				((Control)(object)CmbVehicleType).Enabled = true;
				txtVehicleNo.Enabled = true;
				txtBiltyNo.Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdGp).Name);
			if (fs != null)
			{
				grdGp.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar4_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GrdHistoryDetail).Name);
			if (fs != null)
			{
				GrdHistoryDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GrdHistory).Name);
			if (fs != null)
			{
				GrdHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void InvFrmGRN_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.Return && tabControl1.SelectedIndex == 0 && btnSave.Visible && btnSave.Enabled && grdGp.GetRows().Count() > 0)
			{
				GPID = 0;
				FreightOnCash = 0;
				FreightId = 0;
				if (!(Conversion.ToString(grdGp.CurrentRow.Cells["Status"].Value) == "Accepted"))
				{
					MessageBox.Show("Status Not Accepted Please check status");
					return;
				}
				GPID = Conversion.ToInt(grdGp.CurrentRow.Cells["Id"].Value);
				FreightOnCash = Conversion.ToInt(grdGp.CurrentRow.Cells["FreightOnCash"].Value);
				FreightId = Conversion.ToInt(grdGp.CurrentRow.Cells["FreightId"].Value);
				CmbGatePass.Text = Conversion.ToString(grdGp.CurrentRow.Cells["GpSrNo"].Value);
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
				CmbItemName.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					((Control)(object)grd).Focus();
				}
				else
				{
					((Control)(object)GrdHistory).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Right && tabControl1.SelectedIndex == 0)
			{
				((Control)(object)grdGp).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Return && tabControl1.SelectedIndex == 1)
			{
				Id = Conversion.ToInt((object)GrdHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtAddLessWeight_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			if (!char.IsDigit(e.KeyChar) && e.KeyChar != '-' && e.KeyChar != '.')
			{
				e.Handled = true;
			}
			if (e.KeyChar == '\b')
			{
				e.Handled = false;
			}
			if (e.KeyChar == '.' && txtAddLessWeight.Text.Contains("."))
			{
				e.Handled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtCarrAmount_Leave(object sender, EventArgs e)
	{
		try
		{
			txtCarrAmount.Text = Conversion.ToDecimal((object)txtCarrAmount.Text).ToString(clsGlobalVariables.stringFormatsingle);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtWeighttCut_Leave(object sender, EventArgs e)
	{
		try
		{
			txtWeighttCut.Text = Conversion.ToDecimal((object)txtWeighttCut.Text).ToString("#,##0.####");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void InvFrmGRN_St_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_044b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Expected O, but got Unknown
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Expected O, but got Unknown
		//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fa: Expected O, but got Unknown
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Expected O, but got Unknown
		//IL_0548: Unknown result type (might be due to invalid IL or missing references)
		//IL_0552: Expected O, but got Unknown
		//IL_055e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0568: Expected O, but got Unknown
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Expected O, but got Unknown
		//IL_058a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0594: Expected O, but got Unknown
		//IL_0595: Unknown result type (might be due to invalid IL or missing references)
		//IL_059f: Expected O, but got Unknown
		//IL_067c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0686: Expected O, but got Unknown
		//IL_06c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d3: Expected O, but got Unknown
		//IL_0716: Unknown result type (might be due to invalid IL or missing references)
		//IL_0720: Expected O, but got Unknown
		//IL_079a: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a4: Expected O, but got Unknown
		//IL_07bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c5: Expected O, but got Unknown
		//IL_08a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ac: Expected O, but got Unknown
		//IL_08d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e3: Expected O, but got Unknown
		//IL_0f93: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f9d: Expected O, but got Unknown
		//IL_0faa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb4: Expected O, but got Unknown
		//IL_138c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1396: Expected O, but got Unknown
		//IL_7e01: Unknown result type (might be due to invalid IL or missing references)
		//IL_7e0b: Expected O, but got Unknown
		//IL_7e2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_7e39: Expected O, but got Unknown
		GridEXLayout grdGp_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
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
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
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
		GridEXLayout GrdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdHistory_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.grdGp = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel17 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel5 = new System.Windows.Forms.Panel();
		this.txtLabNo = new System.Windows.Forms.TextBox();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.CmbItemName = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.CmbOrderNo = new UltraCombo();
		this.label41 = new System.Windows.Forms.Label();
		this.CmbPackUom = new UltraCombo();
		this.label31 = new System.Windows.Forms.Label();
		this.CmbCityName = new UltraCombo();
		this.label42 = new System.Windows.Forms.Label();
		this.CmbWarehouse = new UltraCombo();
		this.label43 = new System.Windows.Forms.Label();
		this.CmbPackingType = new UltraCombo();
		this.CmbJobLot = new UltraCombo();
		this.Add = new System.Windows.Forms.Button();
		this.label35 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.txtNetWeight = new System.Windows.Forms.TextBox();
		this.label29 = new System.Windows.Forms.Label();
		this.txtStockWeight = new System.Windows.Forms.TextBox();
		this.txtWeighttCutTotal = new System.Windows.Forms.TextBox();
		this.txtAddLessWeight = new System.Windows.Forms.TextBox();
		this.txtWeighttCut = new System.Windows.Forms.TextBox();
		this.txtqty = new System.Windows.Forms.TextBox();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label11 = new System.Windows.Forms.Label();
		this.lblBalwt = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtGrnWeight = new System.Windows.Forms.TextBox();
		this.CmbVehicleType = new UltraCombo();
		this.CmbGatePass = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.label15 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.CmbTransport = new UltraCombo();
		this.label19 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.txtDeliverTerm = new System.Windows.Forms.TextBox();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.label26 = new System.Windows.Forms.Label();
		this.combrefprty = new UltraCombo();
		this.txtBiltyNo = new System.Windows.Forms.TextBox();
		this.lblRecwt = new System.Windows.Forms.Label();
		this.txtVehicleNo = new System.Windows.Forms.TextBox();
		this.txtCarrAmount = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.txtSuppWeight = new System.Windows.Forms.TextBox();
		this.txtFactoryWeight = new System.Windows.Forms.TextBox();
		this.txtReceivedWeight = new System.Windows.Forms.TextBox();
		this.txtBalWeight = new System.Windows.Forms.TextBox();
		this.combstockprty = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.CmbSupplier = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.label5 = new System.Windows.Forms.Label();
		this.txtAnaylstName = new System.Windows.Forms.TextBox();
		this.txtDeductionKg = new System.Windows.Forms.TextBox();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label17 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.printToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.CityDefine = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel11 = new System.Windows.Forms.Panel();
		this.panel21 = new System.Windows.Forms.Panel();
		this.GrdHistoryDetail = new GridEX();
		this.panel20 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel19 = new System.Windows.Forms.Panel();
		this.GrdHistory = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.label13 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel7 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGp).BeginInit();
		this.panel12.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel16.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCityName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbVehicleType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransport).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combrefprty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combstockprty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel11.SuspendLayout();
		this.panel21.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryDetail).BeginInit();
		this.panel20.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel19.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistory).BeginInit();
		this.panel18.SuspendLayout();
		this.panel7.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1046, 745);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1046, 745);
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
		this.tabPage1.Size = new System.Drawing.Size(1038, 719);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.panel6.Controls.Add(this.panel9);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(2, 485);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1034, 232);
		this.panel6.TabIndex = 6;
		this.panel9.Controls.Add(this.panel13);
		this.panel9.Controls.Add(this.panel12);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1034, 232);
		this.panel9.TabIndex = 11;
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.grdGp);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(0, 35);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1034, 197);
		this.panel13.TabIndex = 2;
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
		((System.Windows.Forms.Control)(object)this.grdGp).Size = new System.Drawing.Size(1034, 197);
		((System.Windows.Forms.Control)(object)this.grdGp).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdGp).TabStop = false;
		this.grdGp.TotalRow = (InheritableBoolean)1;
		this.grdGp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGp).VisualStyle = (VisualStyle)4;
		this.grdGp.ColumnButtonClick += new ColumnActionEventHandler(grdGp_ColumnButtonClick);
		this.grdGp.LinkClicked += new ColumnActionEventHandler(grdGp_LinkClicked);
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label4);
		this.panel12.Controls.Add(this.ctrlGrdBar3);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1034, 35);
		this.panel12.TabIndex = 1;
		this.label4.AutoEllipsis = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(2, 7);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(941, 26);
		this.label4.TabIndex = 24;
		this.label4.Text = "This Grid show Pending Gatepasses For Grn Entry,User can Load GatePasses With Accepted Status for Entry From This grid.Gatepasses With Open Status Will Not Load For Entry";
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(991, 2);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdGp;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(39, 27);
		this.ctrlGrdBar3.TabIndex = 23;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.panel10.Controls.Add(this.panel17);
		this.panel10.Controls.Add(this.panel16);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(2, 318);
		this.panel10.Margin = new System.Windows.Forms.Padding(2);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1034, 167);
		this.panel10.TabIndex = 5;
		this.panel17.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel17.Location = new System.Drawing.Point(0, 27);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1034, 140);
		this.panel17.TabIndex = 4;
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
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1034, 140);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grd).TabStop = false;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.label23);
		this.panel16.Controls.Add(this.label14);
		this.panel16.Controls.Add(this.ctrlGrdBar1);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 0);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1034, 27);
		this.panel16.TabIndex = 3;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(3, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(97, 19);
		this.label23.TabIndex = 27;
		this.label23.Text = "Detail Grid";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(3, 5);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(0, 15);
		this.label14.TabIndex = 26;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(990, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(39, 23);
		this.ctrlGrdBar1.TabIndex = 22;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel5.Controls.Add(this.txtLabNo);
		this.panel5.Controls.Add(this.btnCancelUpdateDetial);
		this.panel5.Controls.Add(this.btnUpdateDetail);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.panel5.Controls.Add(this.label9);
		this.panel5.Controls.Add(this.label37);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Controls.Add(this.label36);
		this.panel5.Controls.Add(this.label40);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOrderNo);
		this.panel5.Controls.Add(this.label41);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUom);
		this.panel5.Controls.Add(this.label31);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCityName);
		this.panel5.Controls.Add(this.label42);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouse);
		this.panel5.Controls.Add(this.label43);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackingType);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.panel5.Controls.Add(this.Add);
		this.panel5.Controls.Add(this.label35);
		this.panel5.Controls.Add(this.label28);
		this.panel5.Controls.Add(this.label30);
		this.panel5.Controls.Add(this.label46);
		this.panel5.Controls.Add(this.label27);
		this.panel5.Controls.Add(this.txtNetWeight);
		this.panel5.Controls.Add(this.label29);
		this.panel5.Controls.Add(this.txtStockWeight);
		this.panel5.Controls.Add(this.txtWeighttCutTotal);
		this.panel5.Controls.Add(this.txtAddLessWeight);
		this.panel5.Controls.Add(this.txtWeighttCut);
		this.panel5.Controls.Add(this.txtqty);
		this.panel5.Controls.Add(this.txtGrossWeight);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel5.Location = new System.Drawing.Point(2, 204);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1034, 114);
		this.panel5.TabIndex = 0;
		this.txtLabNo.BackColor = System.Drawing.Color.White;
		this.txtLabNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLabNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLabNo.Location = new System.Drawing.Point(617, 81);
		this.txtLabNo.Name = "txtLabNo";
		this.txtLabNo.Size = new System.Drawing.Size(73, 23);
		this.txtLabNo.TabIndex = 13;
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(963, 80);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(58, 24);
		this.btnCancelUpdateDetial.TabIndex = 17;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(900, 80);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(62, 24);
		this.btnUpdateDetail.TabIndex = 16;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance68;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance123).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(89, 28);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 1;
		this.CmbItemName.ValueChanged += new System.EventHandler(combitem_Leave);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Leave += new System.EventHandler(combitem_Leave);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(941, 9);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(73, 15);
		this.label9.TabIndex = 44;
		this.label9.Text = "Wt Cut Total";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(7, 9);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(56, 15);
		this.label37.TabIndex = 0;
		this.label37.Text = "Order No";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(873, 9);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(46, 15);
		this.label3.TabIndex = 43;
		this.label3.Text = "Wt/Cut";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(92, 9);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(67, 15);
		this.label36.TabIndex = 8;
		this.label36.Text = "Item Name";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(480, 9);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(75, 15);
		this.label40.TabIndex = 17;
		this.label40.Text = "Packing type";
		this.CmbOrderNo.AllowNull = (DefaultableBoolean)1;
		this.CmbOrderNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOrderNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOrderNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance52;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance58).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbOrderNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Location = new System.Drawing.Point(6, 28);
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Name = "CmbOrderNo";
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Size = new System.Drawing.Size(81, 26);
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Leave += new System.EventHandler(comborderno_Leave);
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(696, 9);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(54, 15);
		this.label41.TabIndex = 19;
		this.label41.Text = "Item Qty";
		this.CmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance65;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance71).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Location = new System.Drawing.Point(602, 28);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Name = "CmbPackUom";
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Size = new System.Drawing.Size(93, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Leave += new System.EventHandler(combpckuom_Leave);
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(619, 61);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(45, 15);
		this.label31.TabIndex = 17;
		this.label31.Text = "Lab No";
		this.CmbCityName.AllowNull = (DefaultableBoolean)1;
		this.CmbCityName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCityName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCityName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbCityName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCityName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance78;
		((SpecialBoxBase)((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbCityName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCityName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance84).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance85).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance85).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance85).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbCityName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCityName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCityName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCityName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCityName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCityName).Location = new System.Drawing.Point(691, 79);
		((System.Windows.Forms.Control)(object)this.CmbCityName).Name = "CmbCityName";
		((System.Windows.Forms.Control)(object)this.CmbCityName).Size = new System.Drawing.Size(179, 26);
		((System.Windows.Forms.Control)(object)this.CmbCityName).TabIndex = 14;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(600, 9);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(63, 15);
		this.label42.TabIndex = 21;
		this.label42.Text = "Pack UOM";
		this.CmbWarehouse.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance92;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance97).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance98).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance98).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance98).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWarehouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Location = new System.Drawing.Point(326, 79);
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Name = "CmbWarehouse";
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Size = new System.Drawing.Size(290, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).TabIndex = 12;
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(777, 9);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(78, 15);
		this.label43.TabIndex = 23;
		this.label43.Text = "Gross Weight";
		this.CmbPackingType.AllowNull = (DefaultableBoolean)1;
		this.CmbPackingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance105;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance110).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance111).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance111).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance111).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance113).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Location = new System.Drawing.Point(479, 28);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Name = "CmbPackingType";
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Size = new System.Drawing.Size(121, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).TabIndex = 3;
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance118;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance124).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance125).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(311, 28);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(166, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 2;
		this.Add.BackColor = System.Drawing.Color.Teal;
		this.Add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.Add.Font = new System.Drawing.Font("MingLiU_HKSCS-ExtB", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Add.ForeColor = System.Drawing.Color.White;
		this.Add.Location = new System.Drawing.Point(871, 79);
		this.Add.Margin = new System.Windows.Forms.Padding(2);
		this.Add.Name = "Add";
		this.Add.Size = new System.Drawing.Size(29, 26);
		this.Add.TabIndex = 15;
		this.Add.Text = "+";
		this.Add.UseVisualStyleBackColor = false;
		this.Add.Click += new System.EventHandler(Add_Click);
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(5, 61);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(73, 15);
		this.label35.TabIndex = 25;
		this.label35.Text = "Add Less Wt";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(696, 61);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(62, 15);
		this.label28.TabIndex = 25;
		this.label28.Text = "City Name";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(329, 61);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(102, 15);
		this.label30.TabIndex = 41;
		this.label30.Text = "Warehouse Name";
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(104, 61);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(68, 15);
		this.label46.TabIndex = 25;
		this.label46.Text = "Net Weight";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(313, 9);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(48, 15);
		this.label27.TabIndex = 41;
		this.label27.Text = "Job/Lot";
		this.txtNetWeight.BackColor = System.Drawing.Color.White;
		this.txtNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeight.Location = new System.Drawing.Point(104, 81);
		this.txtNetWeight.Name = "txtNetWeight";
		this.txtNetWeight.ReadOnly = true;
		this.txtNetWeight.Size = new System.Drawing.Size(112, 23);
		this.txtNetWeight.TabIndex = 10;
		this.txtNetWeight.TabStop = false;
		this.txtNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(220, 61);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(79, 15);
		this.label29.TabIndex = 25;
		this.label29.Text = "Stock Weight";
		this.txtStockWeight.BackColor = System.Drawing.Color.White;
		this.txtStockWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtStockWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtStockWeight.Location = new System.Drawing.Point(217, 81);
		this.txtStockWeight.Name = "txtStockWeight";
		this.txtStockWeight.ReadOnly = true;
		this.txtStockWeight.Size = new System.Drawing.Size(108, 23);
		this.txtStockWeight.TabIndex = 11;
		this.txtStockWeight.TabStop = false;
		this.txtStockWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtWeighttCutTotal.BackColor = System.Drawing.Color.White;
		this.txtWeighttCutTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWeighttCutTotal.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWeighttCutTotal.Location = new System.Drawing.Point(942, 30);
		this.txtWeighttCutTotal.Name = "txtWeighttCutTotal";
		this.txtWeighttCutTotal.ReadOnly = true;
		this.txtWeighttCutTotal.Size = new System.Drawing.Size(79, 23);
		this.txtWeighttCutTotal.TabIndex = 8;
		this.txtWeighttCutTotal.TabStop = false;
		this.txtWeighttCutTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAddLessWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAddLessWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAddLessWeight.Location = new System.Drawing.Point(6, 81);
		this.txtAddLessWeight.Name = "txtAddLessWeight";
		this.txtAddLessWeight.Size = new System.Drawing.Size(97, 23);
		this.txtAddLessWeight.TabIndex = 9;
		this.txtAddLessWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAddLessWeight.TextChanged += new System.EventHandler(txtadlswt_TextChanged_1);
		this.txtAddLessWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAddLessWeight_KeyPress);
		this.txtAddLessWeight.Leave += new System.EventHandler(txtadlswt_Leave);
		this.txtWeighttCut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWeighttCut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWeighttCut.Location = new System.Drawing.Point(875, 30);
		this.txtWeighttCut.Name = "txtWeighttCut";
		this.txtWeighttCut.Size = new System.Drawing.Size(66, 23);
		this.txtWeighttCut.TabIndex = 7;
		this.txtWeighttCut.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtWeighttCut.TextChanged += new System.EventHandler(txtebu_TextChanged);
		this.txtWeighttCut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtwtcut_KeyPress);
		this.txtWeighttCut.Leave += new System.EventHandler(txtWeighttCut_Leave);
		this.txtqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtqty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtqty.Location = new System.Drawing.Point(696, 30);
		this.txtqty.Name = "txtqty";
		this.txtqty.Size = new System.Drawing.Size(83, 23);
		this.txtqty.TabIndex = 5;
		this.txtqty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtqty.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.txtqty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.txtqty.Leave += new System.EventHandler(txtqty_Leave);
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrossWeight.Location = new System.Drawing.Point(780, 30);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.Size = new System.Drawing.Size(94, 23);
		this.txtGrossWeight.TabIndex = 6;
		this.txtGrossWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtGrossWeight.TextChanged += new System.EventHandler(txtgwt_TextChanged);
		this.txtGrossWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtgwt_KeyPress);
		this.txtGrossWeight.Leave += new System.EventHandler(txtgwt_Leave);
		this.panel4.Controls.Add(this.panel3);
		this.panel4.Controls.Add(this.lblBalwt);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.txtGrnWeight);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbVehicleType);
		this.panel4.Controls.Add(this.CmbGatePass);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add(this.ChkBox);
		this.panel4.Controls.Add(this.label15);
		this.panel4.Controls.Add(this.label25);
		this.panel4.Controls.Add(this.label24);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTransport);
		this.panel4.Controls.Add(this.label19);
		this.panel4.Controls.Add(this.label48);
		this.panel4.Controls.Add(this.label21);
		this.panel4.Controls.Add(this.txtDeliverTerm);
		this.panel4.Controls.Add(this.txtRemarks);
		this.panel4.Controls.Add(this.label26);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combrefprty);
		this.panel4.Controls.Add(this.txtBiltyNo);
		this.panel4.Controls.Add(this.lblRecwt);
		this.panel4.Controls.Add(this.txtVehicleNo);
		this.panel4.Controls.Add(this.txtCarrAmount);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add(this.label20);
		this.panel4.Controls.Add(this.txtSuppWeight);
		this.panel4.Controls.Add(this.txtFactoryWeight);
		this.panel4.Controls.Add(this.txtReceivedWeight);
		this.panel4.Controls.Add(this.txtBalWeight);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combstockprty);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add(this.label12);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.panel4.Controls.Add(this.label10);
		this.panel4.Controls.Add(this.DocDate);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Controls.Add(this.txtAnaylstName);
		this.panel4.Controls.Add(this.txtDeductionKg);
		this.panel4.Controls.Add(this.txtdocno);
		this.panel4.Controls.Add(this.label17);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel4.Location = new System.Drawing.Point(2, 33);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1034, 171);
		this.panel4.TabIndex = 0;
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.label11);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1034, 28);
		this.panel3.TabIndex = 22;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.White;
		this.label11.Location = new System.Drawing.Point(7, 4);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(200, 19);
		this.label11.TabIndex = 0;
		this.label11.Text = "Goods Receiving Notes ";
		this.lblBalwt.AutoSize = true;
		this.lblBalwt.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBalwt.Location = new System.Drawing.Point(842, 205);
		this.lblBalwt.Name = "lblBalwt";
		this.lblBalwt.Size = new System.Drawing.Size(65, 15);
		this.lblBalwt.TabIndex = 21;
		this.lblBalwt.Text = "Bal Weight";
		this.lblBalwt.Visible = false;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(654, 35);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(85, 15);
		this.label1.TabIndex = 20;
		this.label1.Text = "Deduction(Kg)";
		this.txtGrnWeight.BackColor = System.Drawing.Color.White;
		this.txtGrnWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrnWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrnWeight.Location = new System.Drawing.Point(989, 234);
		this.txtGrnWeight.Name = "txtGrnWeight";
		this.txtGrnWeight.ReadOnly = true;
		this.txtGrnWeight.Size = new System.Drawing.Size(201, 21);
		this.txtGrnWeight.TabIndex = 19;
		this.txtGrnWeight.TabStop = false;
		this.txtGrnWeight.Visible = false;
		this.txtGrnWeight.WordWrap = false;
		this.CmbVehicleType.AllowNull = (DefaultableBoolean)1;
		this.CmbVehicleType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbVehicleType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbVehicleType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbVehicleType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance131;
		((SpecialBoxBase)((UltraGridBase)this.CmbVehicleType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance132).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance132).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance137).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance138).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance139).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbVehicleType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbVehicleType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbVehicleType).Location = new System.Drawing.Point(414, 55);
		((System.Windows.Forms.Control)(object)this.CmbVehicleType).Name = "CmbVehicleType";
		((System.Windows.Forms.Control)(object)this.CmbVehicleType).Size = new System.Drawing.Size(236, 26);
		((System.Windows.Forms.Control)(object)this.CmbVehicleType).TabIndex = 7;
		this.CmbGatePass.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.CmbGatePass.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.CmbGatePass.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbGatePass.Location = new System.Drawing.Point(414, 31);
		this.CmbGatePass.Name = "CmbGatePass";
		this.CmbGatePass.ReadOnly = true;
		this.CmbGatePass.Size = new System.Drawing.Size(236, 23);
		this.CmbGatePass.TabIndex = 6;
		this.CmbGatePass.TabStop = false;
		this.CmbGatePass.TextChanged += new System.EventHandler(combgatepass_Leave);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(335, 34);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(57, 15);
		this.label7.TabIndex = 0;
		this.label7.Text = "Gate Pass";
		this.ChkBox.AutoSize = true;
		this.ChkBox.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBox.Location = new System.Drawing.Point(942, 141);
		this.ChkBox.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(67, 19);
		this.ChkBox.TabIndex = 15;
		this.ChkBox.Text = "Preview";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(7, 87);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(80, 15);
		this.label15.TabIndex = 0;
		this.label15.Text = "Delivery Term";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(497, 87);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(49, 15);
		this.label25.TabIndex = 3;
		this.label25.Text = "Bilty No";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(654, 61);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(52, 15);
		this.label24.TabIndex = 0;
		this.label24.Text = "Remarks";
		this.CmbTransport.AllowNull = (DefaultableBoolean)1;
		this.CmbTransport.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTransport.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTransport.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Appearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbTransport).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTransport).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransport).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransport).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance144;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransport).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransport).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbTransport).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTransport).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbTransport).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTransport).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTransport).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTransport).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTransport.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTransport).Location = new System.Drawing.Point(100, 108);
		((System.Windows.Forms.Control)(object)this.CmbTransport).Name = "CmbTransport";
		((System.Windows.Forms.Control)(object)this.CmbTransport).Size = new System.Drawing.Size(228, 26);
		((System.Windows.Forms.Control)(object)this.CmbTransport).TabIndex = 4;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(335, 87);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(64, 15);
		this.label19.TabIndex = 0;
		this.label19.Text = "Vehicle No";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(7, 114);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(67, 15);
		this.label48.TabIndex = 8;
		this.label48.Text = "Transporter";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(335, 61);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(73, 15);
		this.label21.TabIndex = 8;
		this.label21.Text = "Vehicle Type";
		this.txtDeliverTerm.BackColor = System.Drawing.SystemColors.Menu;
		this.txtDeliverTerm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDeliverTerm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDeliverTerm.Location = new System.Drawing.Point(100, 83);
		this.txtDeliverTerm.Name = "txtDeliverTerm";
		this.txtDeliverTerm.ReadOnly = true;
		this.txtDeliverTerm.Size = new System.Drawing.Size(228, 23);
		this.txtDeliverTerm.TabIndex = 3;
		this.txtDeliverTerm.TabStop = false;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(740, 56);
		this.txtRemarks.Multiline = true;
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(196, 106);
		this.txtRemarks.TabIndex = 14;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(7, 140);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(91, 15);
		this.label26.TabIndex = 8;
		this.label26.Text = "Freight Amount";
		this.combrefprty.AllowNull = (DefaultableBoolean)1;
		this.combrefprty.AutoCompleteMode = (AutoCompleteMode)4;
		this.combrefprty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combrefprty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combrefprty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.combrefprty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combrefprty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combrefprty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combrefprty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.combrefprty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combrefprty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.combrefprty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combrefprty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.combrefprty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combrefprty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combrefprty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combrefprty).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combrefprty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combrefprty).Location = new System.Drawing.Point(989, 288);
		((System.Windows.Forms.Control)(object)this.combrefprty).Name = "combrefprty";
		((System.Windows.Forms.Control)(object)this.combrefprty).Size = new System.Drawing.Size(201, 24);
		((System.Windows.Forms.Control)(object)this.combrefprty).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.combrefprty).TabStop = false;
		((System.Windows.Forms.Control)(object)this.combrefprty).Visible = false;
		this.txtBiltyNo.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.txtBiltyNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBiltyNo.Enabled = false;
		this.txtBiltyNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBiltyNo.Location = new System.Drawing.Point(568, 83);
		this.txtBiltyNo.Name = "txtBiltyNo";
		this.txtBiltyNo.Size = new System.Drawing.Size(82, 23);
		this.txtBiltyNo.TabIndex = 9;
		this.lblRecwt.AutoSize = true;
		this.lblRecwt.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblRecwt.Location = new System.Drawing.Point(666, 205);
		this.lblRecwt.Name = "lblRecwt";
		this.lblRecwt.Size = new System.Drawing.Size(75, 15);
		this.lblRecwt.TabIndex = 0;
		this.lblRecwt.Text = "Recd Weight";
		this.lblRecwt.Visible = false;
		this.txtVehicleNo.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.txtVehicleNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVehicleNo.Enabled = false;
		this.txtVehicleNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVehicleNo.Location = new System.Drawing.Point(414, 83);
		this.txtVehicleNo.Name = "txtVehicleNo";
		this.txtVehicleNo.Size = new System.Drawing.Size(82, 23);
		this.txtVehicleNo.TabIndex = 8;
		this.txtCarrAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCarrAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCarrAmount.Location = new System.Drawing.Point(100, 136);
		this.txtCarrAmount.Name = "txtCarrAmount";
		this.txtCarrAmount.Size = new System.Drawing.Size(228, 23);
		this.txtCarrAmount.TabIndex = 5;
		this.txtCarrAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCarrAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcarramount_KeyPress);
		this.txtCarrAmount.Leave += new System.EventHandler(txtCarrAmount_Leave);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(335, 114);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(77, 15);
		this.label6.TabIndex = 0;
		this.label6.Text = "Supp Weight";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(888, 265);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(67, 15);
		this.label8.TabIndex = 3;
		this.label8.Text = "Stock Party";
		this.label8.Visible = false;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(497, 114);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(71, 15);
		this.label20.TabIndex = 3;
		this.label20.Text = "Fact Weight";
		this.txtSuppWeight.BackColor = System.Drawing.SystemColors.Menu;
		this.txtSuppWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSuppWeight.Enabled = false;
		this.txtSuppWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSuppWeight.Location = new System.Drawing.Point(414, 110);
		this.txtSuppWeight.Name = "txtSuppWeight";
		this.txtSuppWeight.Size = new System.Drawing.Size(81, 23);
		this.txtSuppWeight.TabIndex = 10;
		this.txtSuppWeight.TabStop = false;
		this.txtSuppWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtSuppWeight.TextChanged += new System.EventHandler(txtsuppwt_TextChanged);
		this.txtFactoryWeight.BackColor = System.Drawing.SystemColors.Menu;
		this.txtFactoryWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFactoryWeight.Enabled = false;
		this.txtFactoryWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFactoryWeight.Location = new System.Drawing.Point(568, 110);
		this.txtFactoryWeight.Name = "txtFactoryWeight";
		this.txtFactoryWeight.ReadOnly = true;
		this.txtFactoryWeight.Size = new System.Drawing.Size(82, 23);
		this.txtFactoryWeight.TabIndex = 11;
		this.txtFactoryWeight.TabStop = false;
		this.txtFactoryWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFactoryWeight.TextChanged += new System.EventHandler(txtfctwt_TextChanged);
		this.txtReceivedWeight.BackColor = System.Drawing.SystemColors.Menu;
		this.txtReceivedWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtReceivedWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtReceivedWeight.Location = new System.Drawing.Point(759, 201);
		this.txtReceivedWeight.Name = "txtReceivedWeight";
		this.txtReceivedWeight.ReadOnly = true;
		this.txtReceivedWeight.Size = new System.Drawing.Size(81, 23);
		this.txtReceivedWeight.TabIndex = 9;
		this.txtReceivedWeight.TabStop = false;
		this.txtReceivedWeight.Visible = false;
		this.txtBalWeight.BackColor = System.Drawing.SystemColors.Menu;
		this.txtBalWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBalWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalWeight.Location = new System.Drawing.Point(913, 201);
		this.txtBalWeight.Name = "txtBalWeight";
		this.txtBalWeight.ReadOnly = true;
		this.txtBalWeight.Size = new System.Drawing.Size(82, 23);
		this.txtBalWeight.TabIndex = 12;
		this.txtBalWeight.TabStop = false;
		this.txtBalWeight.Visible = false;
		this.combstockprty.AllowNull = (DefaultableBoolean)1;
		this.combstockprty.AutoCompleteMode = (AutoCompleteMode)4;
		this.combstockprty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combstockprty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combstockprty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.combstockprty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combstockprty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combstockprty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combstockprty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.combstockprty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combstockprty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.combstockprty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combstockprty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.combstockprty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combstockprty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combstockprty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combstockprty).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combstockprty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combstockprty).Location = new System.Drawing.Point(989, 261);
		((System.Windows.Forms.Control)(object)this.combstockprty).Name = "combstockprty";
		((System.Windows.Forms.Control)(object)this.combstockprty).Size = new System.Drawing.Size(201, 24);
		((System.Windows.Forms.Control)(object)this.combstockprty).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.combstockprty).TabStop = false;
		((System.Windows.Forms.Control)(object)this.combstockprty).Visible = false;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(7, 61);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(86, 15);
		this.label2.TabIndex = 0;
		this.label2.Text = "Supplier Name";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(888, 292);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(56, 15);
		this.label12.TabIndex = 0;
		this.label12.Text = "Ref Party";
		this.label12.Visible = false;
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(100, 55);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(228, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 2;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(335, 140);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(78, 15);
		this.label10.TabIndex = 8;
		this.label10.Text = "AnalystName";
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(182, 31);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(146, 23);
		this.DocDate.TabIndex = 1;
		this.DocDate.TabStop = false;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(7, 34);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(48, 15);
		this.label5.TabIndex = 3;
		this.label5.Text = "Doc No";
		this.txtAnaylstName.BackColor = System.Drawing.SystemColors.Menu;
		this.txtAnaylstName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAnaylstName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAnaylstName.Location = new System.Drawing.Point(414, 136);
		this.txtAnaylstName.Name = "txtAnaylstName";
		this.txtAnaylstName.ReadOnly = true;
		this.txtAnaylstName.Size = new System.Drawing.Size(236, 23);
		this.txtAnaylstName.TabIndex = 12;
		this.txtAnaylstName.TabStop = false;
		this.txtDeductionKg.BackColor = System.Drawing.SystemColors.Menu;
		this.txtDeductionKg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDeductionKg.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDeductionKg.Location = new System.Drawing.Point(740, 31);
		this.txtDeductionKg.Name = "txtDeductionKg";
		this.txtDeductionKg.ReadOnly = true;
		this.txtDeductionKg.Size = new System.Drawing.Size(196, 23);
		this.txtDeductionKg.TabIndex = 13;
		this.txtDeductionKg.TabStop = false;
		this.txtDeductionKg.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(100, 31);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(80, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(998, 327);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(57, 15);
		this.label17.TabIndex = 3;
		this.label17.Text = "Doc Date";
		this.label17.Visible = false;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1034, 31);
		this.panel2.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.btnNew, this.btnSave, this.btnUpdate, this.btnDelete, this.toolStripButton1, this.btnAttachment, this.printToolStripButton, this.CityDefine });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1034, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 24);
		this.btnNew.Text = "&New";
		this.btnNew.ToolTipText = "Ctrl_N";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(55, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.ToolTipText = "Ctrl_S";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(69, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.ToolTipText = "Ctrl_U";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.icons8_cut_16;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(64, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(70, 24);
		this.toolStripButton1.Text = "&Refresh";
		this.toolStripButton1.ToolTipText = "Alt_R";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(94, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.ToolTipText = "Alt_A";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.printToolStripButton.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.printToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.printToolStripButton.Name = "printToolStripButton";
		this.printToolStripButton.Size = new System.Drawing.Size(85, 24);
		this.printToolStripButton.Text = "1503-Print";
		this.printToolStripButton.ToolTipText = "Ctrl_P";
		this.printToolStripButton.Click += new System.EventHandler(printToolStripButton_Click);
		this.CityDefine.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.CityDefine.Name = "CityDefine";
		this.CityDefine.Size = new System.Drawing.Size(69, 24);
		this.CityDefine.Text = "Define City";
		this.CityDefine.Click += new System.EventHandler(CityDefine_Click);
		this.tabPage2.Controls.Add(this.panel11);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(1038, 719);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel11.Controls.Add(this.panel21);
		this.panel11.Controls.Add(this.panel20);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(2, 384);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1034, 333);
		this.panel11.TabIndex = 2;
		this.panel21.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistoryDetail);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel21.Location = new System.Drawing.Point(0, 30);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(1034, 303);
		this.panel21.TabIndex = 47;
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
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Size = new System.Drawing.Size(1034, 303);
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).TabIndex = 45;
		this.GrdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.GrdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel20.BackColor = System.Drawing.Color.Teal;
		this.panel20.Controls.Add(this.ctrlGrdBar4);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(0, 0);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(1034, 30);
		this.panel20.TabIndex = 46;
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(985, 3);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar4.MyGrid = this.GrdHistoryDetail;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(45, 24);
		this.ctrlGrdBar4.TabIndex = 24;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.panel8.Controls.Add(this.panel19);
		this.panel8.Controls.Add(this.panel18);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(2, 30);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1034, 354);
		this.panel8.TabIndex = 1;
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistory);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel19.Location = new System.Drawing.Point(0, 32);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(1034, 322);
		this.panel19.TabIndex = 46;
		this.GrdHistory.AllowEdit = (InheritableBoolean)2;
		GrdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistory.DesignTimeLayout = GrdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdHistory.DynamicFiltering = true;
		this.GrdHistory.FilterMode = (FilterMode)2;
		this.GrdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GrdHistory).Name = "GrdHistory";
		((ScrollableBase)this.GrdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Size = new System.Drawing.Size(1034, 322);
		((System.Windows.Forms.Control)(object)this.GrdHistory).TabIndex = 44;
		this.GrdHistory.TotalRow = (InheritableBoolean)1;
		this.GrdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistory).VisualStyle = (VisualStyle)4;
		this.GrdHistory.ColumnButtonClick += new ColumnActionEventHandler(GrdHistory_ColumnButtonClick);
		this.GrdHistory.SelectionChanged += new System.EventHandler(GrdHistory_SelectionChanged);
		this.GrdHistory.LinkClicked += new ColumnActionEventHandler(GrdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdHistory).DoubleClick += new System.EventHandler(GrdHistory_DoubleClick);
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.label13);
		this.panel18.Controls.Add(this.ctrlGrdBar2);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(0, 0);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1034, 32);
		this.panel18.TabIndex = 45;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(8, 6);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(258, 19);
		this.label13.TabIndex = 24;
		this.label13.Text = "Goods Receiving Notes History";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(985, 3);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.GrdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 24);
		this.ctrlGrdBar2.TabIndex = 23;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel7.Controls.Add(this.toolStrip2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(2, 2);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1034, 28);
		this.panel7.TabIndex = 0;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnLoadAll });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1034, 27);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.btnLoadAll.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(71, 24);
		this.btnLoadAll.Text = "&LoadAll";
		this.btnLoadAll.ToolTipText = "Ctrl_L";
		this.btnLoadAll.Click += new System.EventHandler(btnLoadAll_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1046, 745);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "InvFrmGRN_St";
		this.Text = "Goods Receiving Notes";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InvFrmGRN_St_FormClosing);
		base.Load += new System.EventHandler(InvFrmGRN_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvFrmGRN_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdGp).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		this.panel17.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCityName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbVehicleType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransport).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combrefprty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combstockprty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		this.panel21.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryDetail).EndInit();
		this.panel20.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel19.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistory).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
