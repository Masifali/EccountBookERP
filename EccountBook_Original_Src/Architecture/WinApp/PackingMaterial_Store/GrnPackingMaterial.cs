using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Contractor_Wages;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.PackingMaterial_Store;

public class GrnPackingMaterial : Form
{
	private int Id;

	private int updateDetailIndex;

	private int BillCalculateTypeId;

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool WagesStatus;

	private bool IsGeneralGp;

	private bool WagesActiveOrInActive;

	private bool SubsidiaryAccountAllownOnVouchers;

	private bool WbNotCompulsoryForPmPurchaseOrder;

	private bool FormInitialized;

	private int DocumentTypeId = 701;

	private string ScreenName = "GrnPackingMaterial";

	private DataTable dtdetail = new DataTable();

	private DataTable dtBranch = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtTransporter = new DataTable();

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private LoadPurchaseOrderPM LoadPO;

	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

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

	private Panel panel7;

	private Panel panel8;

	internal GridEX GrdHistory;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStripButton toolStripButton1;

	private ToolStripButton CityDefine;

	private CtrlGrdBar ctrlGrdBar2;

	private ToolStrip toolStrip2;

	private ToolStripButton btnDelete;

	private ToolStripButton btnGrnFormHistory;

	private ToolStripButton BtnLoadOrder;

	private Panel panel3;

	private Panel panel4;

	private TextBox txtGpId;

	private Label label3;

	private Label label7;

	private TextBox txtGpNo;

	private Label label1;

	private Label label34;

	private Label label32;

	private Label label23;

	private Label label14;

	private Label label53;

	internal UltraCombo CmbDeliveryTerm;

	private Label label4;

	internal UltraCombo combvehtyp;

	private CheckBox ChkBox;

	private Label label25;

	private Label label24;

	internal UltraCombo CmbTransport;

	private Label label19;

	private Label label48;

	private Label label21;

	private Label label26;

	private TextBox txtbltyno;

	private Label label11;

	private TextBox txtvehno;

	private TextBox txtcarramount;

	private Label label6;

	private Label label20;

	private TextBox txtsuppwt;

	private TextBox txtfctwt;

	private TextBox txtwtdiff;

	private Label label2;

	internal UltraCombo combsupplier;

	private DateTimePicker DocDate;

	private Label label5;

	private TextBox txtdocno;

	private Label label12;

	private Label label10;

	private Label label9;

	private Label label13;

	private TextBox txtAvgWtPerQty;

	internal GridEX grdGp;

	private Panel panel5;

	private Label label8;

	internal GridEX grd;

	private Panel panel6;

	private Label label15;

	private GroupBox groupBox1;

	private Label label16;

	private CtrlGrdBar ctrlGrdBar3;

	internal GridEX GrdHistoryDetail;

	private Label label17;

	private Label label22;

	private TextBox txtBillCalculateType;

	private Panel panel11;

	private Panel panel23;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private GroupBox groupBox3;

	private RadioButton RadNotReffered;

	private RadioButton RadReffered;

	private Button btnshowHistory;

	private Label label49;

	private RadioButton RadAll;

	private Label label50;

	private Label label51;

	private Label label52;

	private Label label18;

	internal UltraCombo cmbSupplierNameHistory;

	private TextBox txtToDocNoHistory;

	private TextBox txtFromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnRefreshHistory;

	private Label label27;

	private Label label28;

	private ToolStripButton BtnShortCutkeys;

	private TextBox txtremarks;

	private ToolStripButton BtnPrintII;

	private CheckBox ChkPreviewII;

	public GrnPackingMaterial(UserAccount user)
	{
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		List<UserAccountAllocation> userAccountAllocationsList = user.UserAccountAllocationsList;
		if (userAccountAllocationsList != null && userAccountAllocationsList.Count > 0)
		{
			ScreenName = user.UserAccountAllocationsList[0].ScreenName;
		}
		InitializeComponentCustom();
		InitializeComponentMethod();
		FormHelper.FormEventHandler(this);
	}

	private void InvFrmGRN_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 700);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InitializeComponentCustom()
	{
		try
		{
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("CompanyName", typeof(string));
			dtSupplier.Columns.Add("PartyCode", typeof(string));
			dtSupplier.Columns.Add("GlAccountId", typeof(int));
			dtSupplier.Columns.Add("CityId", typeof(int));
			dtSupplier.Columns.Add("CityName", typeof(string));
			dtSupplier.Columns.Add("MobileNo", typeof(string));
			dtTransporter.Columns.Add("Id", typeof(int));
			dtTransporter.Columns.Add("AccountTitle", typeof(string));
			dtTransporter.Columns.Add("AccountCode", typeof(string));
			dtTransporter.Columns.Add("SupplierCustomerId", typeof(int));
			dtdetail.Columns.Add("Id");
			dtdetail.Columns.Add("OrderId");
			dtdetail.Columns.Add("OrderDetailId");
			dtdetail.Columns.Add("OrderDate");
			dtdetail.Columns.Add("OrderNo");
			dtdetail.Columns.Add("ItemId");
			dtdetail.Columns.Add("ItemName");
			dtdetail.Columns.Add("CropYearId", typeof(int));
			dtdetail.Columns.Add("PackingDate", typeof(DateTime));
			dtdetail.Columns.Add("ExpiryDate", typeof(DateTime));
			dtdetail.Columns.Add("PackUOMId");
			dtdetail.Columns.Add("PackUOM");
			dtdetail.Columns.Add("ItemQty", typeof(double));
			dtdetail.Columns.Add("QtyValidate", typeof(double));
			dtdetail.Columns.Add("WtPerQty", typeof(double));
			dtdetail.Columns.Add("GrossWeight", typeof(double));
			dtdetail.Columns.Add("SupplierQty", typeof(double));
			dtdetail.Columns.Add("CityName");
			dtdetail.Columns.Add("WarehouseId", typeof(int));
			dtdetail.Columns.Add("Warehouse");
			dtdetail.Columns.Add("RemarksDetail");
			dtdetail.Columns.Add("ItemConditionId", typeof(int));
			dtdetail.Columns.Add("RackId", typeof(int));
			dtdetail.Columns.Add("RackName", typeof(string));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InitializeComponentMethod()
	{
		Task.Run(delegate
		{
			formright = CommonServices.SetRightsValueInRightsObject(ScreenName);
			string documentNoData = DocumentNoDbCall();
			DataTable all = VehicleType.GetAll();
			DataTable gpData = PendingGpDataDbCall();
			DataTable dataForDropDownFromGrn = InvGrn.GetDataForDropDownFromGrn(UserAccount.OrganizationId, UserAccount.CompanyId, DocumentTypeId.ToString(), (string)null, Conversion.ToString((object)UserAccount.BranchesId), 0);
			return new
			{
				DocumentNoData = documentNoData,
				VehicleTypeData = all,
				GpData = gpData,
				HistoryComboData = dataForDropDownFromGrn
			};
		}).ContinueWith(t =>
		{
			if (!base.IsDisposed)
			{
				if (t.IsFaulted)
				{
					MessageBox.Show("Error occurred during database call.");
				}
				else
				{
					var result = t.Result;
					btnSave.Enabled = formright.DoHaveSaveRight;
					printToolStripButton.Enabled = formright.DoHavePrintRights;
					btnUpdate.Enabled = formright.DoHaveUpdateRights;
					btnDelete.Enabled = formright.DoHaveCanDelete;
					printToolStripButton.Enabled = formright.DoHavePrintRights;
					BtnPrintII.Enabled = formright.DoHavePrintRights;
					btnDelete.Visible = formright.DoHaveCanDelete;
					FormHelper.SetCtrlGrdBarRight(ctrlGrdBar1, formright);
					WagesStatus = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("WagesCompulsoryOnGrnPackingMaterial"));
					WbNotCompulsoryForPmPurchaseOrder = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("WbNotCompulsoryForPmPurchaseOrder"));
					DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"));
					int AcId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("FreightInwardAc"));
					if (Id == 0)
					{
						txtdocno.Text = result.DocumentNoData;
					}
					SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
					WagesActiveOrInActive = GlobalVariables_Helper.GetWagesRefDocumentsStatusById(DocumentTypeId);
					SupplierDtFillFromGlobal();
					BindSupplierName();
					VehicleTypesBind(result.VehicleTypeData);
					TransporterDtFillFromGlobal();
					TransporterAccountFill();
					DeliveryTerm();
					PendingGatePassForGrnLoad(result.GpData);
					if (dtdetail.Columns.Count > 0)
					{
						grd.DataSource = dtdetail;
						grd.RetrieveStructure();
						grdSettings(grd);
					}
					if (AcId > 0 && dtTransporter.AsEnumerable().Any((DataRow r) => Conversion.ToInt(r["Id"]) == AcId))
					{
						CmbTransport.Value = AcId;
					}
					HistoryComboFill(result.HistoryComboData);
					FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
					ToDateHistory.Value = DateTime.Now;
					FormInitialized = true;
					((Control)(object)combsupplier).Enabled = false;
					((Control)(object)CmbDeliveryTerm).Enabled = false;
					ChkBox.Checked = true;
					DocDate.Focus();
				}
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private bool FormValidation()
	{
		if (txtdocno.Text.Trim() == "" || txtdocno.Text.Trim() == "0" || txtdocno.Text.Trim() == null)
		{
			MessageBox.Show("DocNo Field is Required");
			txtdocno.Focus();
			return false;
		}
		if (((UltraGridBase)combsupplier).ActiveRow == null)
		{
			MessageBox.Show("Supplier Field is Required");
			combsupplier.Focus();
			return false;
		}
		if (((Control)(object)CmbDeliveryTerm).Text.Trim() == "" || ((Control)(object)CmbDeliveryTerm).Text.Trim() == "0" || ((Control)(object)CmbDeliveryTerm).Text.Trim() == null)
		{
			MessageBox.Show("DeliveryTerm Field is Required");
			CmbDeliveryTerm.Focus();
			return false;
		}
		if (((UltraGridBase)combvehtyp).ActiveRow == null)
		{
			MessageBox.Show("Vehicle Type Field is Required");
			combvehtyp.Focus();
			return false;
		}
		if (txtvehno.Text.Trim() == "" || txtvehno.Text.Trim() == "0" || txtvehno.Text.Trim() == null)
		{
			MessageBox.Show("Vehicle No Field is Required");
			txtvehno.Focus();
			return false;
		}
		if ((Conversion.ToDouble((object)txtsuppwt.Text.Trim()) == 0.0 || txtsuppwt.Text.Trim() == null) && !WbNotCompulsoryForPmPurchaseOrder)
		{
			MessageBox.Show("Supplier Weight Field is Required");
			txtsuppwt.Focus();
			return false;
		}
		if ((Conversion.ToDouble((object)txtfctwt.Text.Trim()) == 0.0 || txtfctwt.Text.Trim() == null) && !IsGeneralGp && !WbNotCompulsoryForPmPurchaseOrder)
		{
			MessageBox.Show("Factory Weight Field is Required");
			txtfctwt.Focus();
			return false;
		}
		return true;
	}

	private void FillDetailListCommonForInsertAndDelete(InvGrnDetail vd, GridEXRow r)
	{
		int orderId = Conversion.ToInt(r.Cells["OrderId"].Value);
		int warehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
		int itemConditionId = Conversion.ToInt(r.Cells["ItemConditionId"].Value);
		string cityName = Conversion.ToString((object)r.Cells["CityName"].Text);
		vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
		vd.PurchaseOrderId = orderId;
		vd.PurchaseOrderDetailId = Conversion.ToInt(r.Cells["OrderDetailId"].Value);
		vd.PurchaserOrderNo = Conversion.ToInt(r.Cells["OrderNo"].Value);
		vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
		vd.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
		vd.CropYear = Conversion.ToString((object)r.Cells["CropYearId"].Text);
		vd.PackingDate = Conversion.ToDateTime(r.Cells["PackingDate"].Value);
		vd.ExpiryDate = Conversion.ToDateTime(r.Cells["ExpiryDate"].Value);
		vd.ItemUomId = Conversion.ToInt(r.Cells["PackUOMId"].Value);
		vd.ItemQty = Conversion.ToDouble(r.Cells["ItemQty"].Value);
		vd.WarehouseId = warehouseId;
		vd.ItemConditionId = itemConditionId;
		vd.ConditionStatus = Conversion.ToString((object)r.Cells["ItemConditionId"].Text);
		vd.ItemCondition = vd.ConditionStatus;
		vd.AreaCity = cityName;
		double grossWeight = (vd.StockWeight = (vd.GrossWeight = Conversion.ToDouble(r.Cells["GrossWeight"].Value)));
		vd.NetBillWeight = grossWeight;
		vd.AdLsWeight = Conversion.ToDouble(r.Cells["WtPerQty"].Value);
		vd.SupplierQty = Conversion.ToDouble(r.Cells["SupplierQty"].Value);
		if (vd.SupplierQty == 0.0)
		{
			vd.SupplierQty = vd.ItemQty;
		}
		vd.RackId = Conversion.ToInt(r.Cells["RackId"].Value);
		vd.rackName = Conversion.ToString((object)r.Cells["RackId"].Text);
		vd.CommentsDetail = Conversion.ToString((object)r.Cells["RemarksDetail"].Text);
	}

	private void Insert()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Expected O, but got Unknown
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
			if (Conversion.ToDouble((object)txtcarramount.Text.Trim()) > 0.0 && (((UltraGridBase)CmbTransport).ActiveRow == null || Conversion.ToInt(CmbTransport.Value) == 0))
			{
				MessageBox.Show("Transporter Account field required");
				CmbTransport.Focus();
				return;
			}
			if (((UltraGridBase)CmbTransport).ActiveRow != null && Conversion.ToInt(CmbTransport.Value) > 0 && Conversion.ToDouble((object)txtcarramount.Text) == 0.0)
			{
				MessageBox.Show("Freight field required");
				txtcarramount.Focus();
				return;
			}
			GRN.DocumentTypeId = DocumentTypeId;
			GRN.DocDate = DocDate.Value;
			GRN.DocNo = Convert.ToInt32(txtdocno.Text.Trim());
			GRN.SupplierCustomerId = Conversion.ToInt(combsupplier.Value);
			GRN.PartyWeight = Conversion.ToDouble((object)txtsuppwt.Text.Trim());
			GRN.FactoryWeight = Conversion.ToDouble((object)txtfctwt.Text.Trim());
			GRN.VehicleType = Conversion.ToString((object)((Control)(object)combvehtyp).Text);
			GRN.VehicleNo = Conversion.ToString((object)txtvehno.Text.Trim());
			GRN.InwardGatePassId = Conversion.ToInt((object)txtGpId.Text.Trim());
			GRN.GpNo = Conversion.ToInt((object)txtGpNo.Text.Trim());
			GRN.DeliveryTerm = Conversion.ToString((object)((Control)(object)CmbDeliveryTerm).Text.Trim());
			GRN.BiltyNo = txtbltyno.Text.Trim();
			if (Conversion.ToDouble((object)txtcarramount.Text.Trim()) > 0.0)
			{
				if (((UltraGridBase)CmbTransport).ActiveRow == null)
				{
					MessageBox.Show("Transporter Account field required");
					CmbTransport.Focus();
					return;
				}
				if (!SubsidiaryAccountAllownOnVouchers)
				{
					GRN.TransporterId = Conversion.ToInt(CmbTransport.Value);
					GRN.TransporterSupCustId = 0;
					GRN.CarriageAmount = Conversion.ToDouble((object)txtcarramount.Text.Trim());
				}
				else
				{
					GRN.TransporterSupCustId = Conversion.ToInt(((UltraDropDownBase)CmbTransport).SelectedRow.Cells[3].Value);
					GRN.TransporterId = Conversion.ToInt(((UltraDropDownBase)CmbTransport).SelectedRow.Cells[0].Value);
					GRN.CarriageAmount = Conversion.ToDouble((object)txtcarramount.Text.Trim());
				}
			}
			GRN.RemarksHeader = txtremarks.Text.Trim();
			GRN.OrganizationId = UserAccount.OrganizationId;
			GRN.CompanyId = UserAccount.CompanyId;
			GRN.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			GRN.BranchesId = Conversion.ToInt((object)UserAccount.BranchesId);
			GRN.ScreenName = Conversion.ToString((object)base.Name);
			GRN.EntryDate = DateTime.Now;
			GRN.ModifyDate = DateTime.Now;
			GRN.EntryUser = UserAccount.ID;
			GRN.ModifyUser = UserAccount.ID;
			GRN.BillCalculateTypeId = BillCalculateTypeId;
			GRN.invGrnDetail = new List<InvGrnDetail>();
			double GrossWeight = 0.0;
			double SupplierWeight = 0.0;
			double FactoryWeight = 0.0;
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				InvGrnDetail grnDetail = new InvGrnDetail();
				grnDetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
				FillDetailListCommonForInsertAndDelete(grnDetail, r);
				FormHelper.ValidateField(grnDetail.ItemId, "Item", r.RowIndex);
				FormHelper.ValidateField(grnDetail.ItemUomId, "Uom", r.RowIndex);
				FormHelper.ValidateField(grnDetail.ItemQty, "Item Qty", r.RowIndex);
				FormHelper.ValidateField(grnDetail.ItemConditionId, "ItemCondition", r.RowIndex);
				FormHelper.ValidateField(grnDetail.AreaCity, "City", r.RowIndex);
				FormHelper.ValidateField(grnDetail.WarehouseId, "WareHouseName", r.RowIndex);
				FormHelper.ValidateField(grnDetail.RackId, "Rack Name", r.RowIndex);
				GrossWeight += grnDetail.GrossWeight;
				GRN.invGrnDetail.Add(grnDetail);
			}
			FactoryWeight = Conversion.ToDouble((object)txtfctwt.Text.Trim());
			SupplierWeight = Conversion.ToDouble((object)txtsuppwt.Text.Trim());
			if (BillCalculateTypeId == 1 && FactoryWeight > 0.0)
			{
				if ((((Control)(object)CmbDeliveryTerm).Text == "Load" || ((Control)(object)CmbDeliveryTerm).Text == "Load & PartyWeight" || ((Control)(object)CmbDeliveryTerm).Text == "Ponch & PartyWeight") && GrossWeight != SupplierWeight)
				{
					throw new Exception("GrossWeight and Supplier Weight Not Match");
				}
				if ((((Control)(object)CmbDeliveryTerm).Text == "Load & FactoryWeight" || ((Control)(object)CmbDeliveryTerm).Text == "Ponch & FactoryWeight") && GrossWeight != FactoryWeight)
				{
					throw new Exception("GrossWeight and Factory Weight Not Match");
				}
				if (((Control)(object)CmbDeliveryTerm).Text == "Ponch")
				{
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
			}
			GRN.AttachmentsList = new List<DMSAttachments>();
			GRN.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<InvGrn>(GRN, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			GRN.AttachmentsValues = AttachmentsValues;
			GRN.CustomAttachmentsValues = CustomAttachmentsValues;
			int success = InvGrn.Save(GRN);
			if (Id > 0)
			{
				MessageBox.Show("Record Update Successfully [" + GRN.DocNo + "]");
				FormHelper.DeleteAllAttachmentsFromDb(AT, Id, DocumentTypeId, base.Name);
			}
			else
			{
				MessageBox.Show("Record Save Successfully [" + GRN.DocNo + "]");
			}
			if (WagesStatus && WagesActiveOrInActive)
			{
				frmwagesBillHeader frmwagesBillHeader = new frmwagesBillHeader(UserAccount);
				frmwagesBillHeader.RefDocTypeId = DocumentTypeId;
				frmwagesBillHeader.RefDocId = success;
				frmwagesBillHeader.GrossWeightTotal = GrossWeight;
				frmwagesBillHeader.ShowDialog();
				frmwagesBillHeader.BringToFront();
			}
			reset();
			if (ChkBox.Checked)
			{
				GenerateReport(success);
			}
			if (ChkPreviewII.Checked)
			{
				CommonServices.GrnPmSlipWithSubReport214_01(success);
			}
			grd.DataSource = null;
			btnSave.Visible = true;
			txtdocno.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		try
		{
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					InvPurchaseInvoice.RemoveByID(new InvPurchaseInvoice
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocumentTypeId = DocumentTypeId,
						Id = Id,
						EntryUser = UserAccount.ID
					});
					MessageBox.Show("Delete Record Successfully");
					reset();
				}
				return;
			}
			throw new Exception("Record Not Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void ReadById(int ID)
	{
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			((Control)(object)CmbDeliveryTerm).Enabled = false;
			btnDelete.Visible = formright.DoHaveCanDelete;
			Id = Conversion.ToInt((object)ID);
			InvGrn invGrn = InvGrn.GetByID(Id);
			txtdocno.Text = invGrn.DocNo.ToString();
			DocDate.Value = invGrn.DocDate;
			tabControl1.SelectedIndex = 0;
			combsupplier.Value = invGrn.SupplierCustomerId;
			((Control)(object)combsupplier).Enabled = false;
			if (SubsidiaryAccountAllownOnVouchers)
			{
				CmbTransport.Value = invGrn.TransporterSupCustId;
			}
			else
			{
				CmbTransport.Value = invGrn.TransporterId;
			}
			txtcarramount.Text = invGrn.CarriageAmount.ToString();
			txtsuppwt.Text = invGrn.PartyWeight.ToString();
			txtfctwt.Text = invGrn.FactoryWeight.ToString();
			((Control)(object)combvehtyp).Text = invGrn.VehicleType;
			txtvehno.Text = invGrn.VehicleNo;
			txtGpId.Text = Conversion.ToString((object)invGrn.InwardGatePassId);
			txtGpNo.Text = Conversion.ToString((object)invGrn.GpNo);
			txtbltyno.Text = Conversion.ToString((object)invGrn.BiltyNo);
			txtremarks.Text = Conversion.ToString((object)invGrn.RemarksHeader);
			((Control)(object)CmbDeliveryTerm).Text = Conversion.ToString((object)invGrn.DeliveryTerm);
			AttachmentsValues = invGrn.AttachmentsValues;
			CustomAttachmentsValues = invGrn.CustomAttachmentsValues;
			BillCalculateTypeId = invGrn.BillCalculateTypeId;
			if (BillCalculateTypeId == 1)
			{
				txtBillCalculateType.Text = "OnWeight";
			}
			else if (BillCalculateTypeId == 2)
			{
				txtBillCalculateType.Text = "OnQty";
			}
			else
			{
				txtBillCalculateType.Text = "";
			}
			new GatePassInward
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			IsGeneralGp = invGrn.RefDocumentTypeId == 52;
			dtdetail.Rows.Clear();
			FillDetailFromListCommonForReadById(invGrn, dtdetail);
			grd.DataSource = dtdetail;
			grd.RetrieveStructure();
			grdSettings(grd);
			FormHelper.LoadAttachmentsForObject(AT, Id, base.Name);
			DocDate.Focus();
			PendingGatePassForGrnLoad(PendingGpDataDbCall());
			BtnLoadOrder.Enabled = IsGeneralGp;
			BtnLoadOrder.Visible = IsGeneralGp;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailFromListCommonForReadById(InvGrn invGrn, DataTable dtdetail, bool AddExtraColumns = false)
	{
		if (((invGrn != null) ? invGrn.invGrnDetail : null) == null || invGrn.invGrnDetail.Count == 0)
		{
			return;
		}
		foreach (InvGrnDetail detail in invGrn.invGrnDetail)
		{
			DataRow dr = dtdetail.NewRow();
			dr["Id"] = detail.Id;
			dr["OrderId"] = detail.PurchaseOrderId;
			dr["OrderDetailId"] = detail.PurchaseOrderDetailId;
			dr["OrderDate"] = Conversion.ToDateTime((object)detail.PurchaseOrderDate).ToShortDateString();
			dr["OrderNo"] = detail.PurchaserOrderNo;
			dr["ItemId"] = detail.ItemId;
			dr["ItemName"] = detail.Item;
			dr["CropYearId"] = detail.CropYearId;
			dr["PackingDate"] = detail.PackingDate;
			dr["ExpiryDate"] = detail.ExpiryDate;
			dr["PackUOMId"] = detail.ItemUomId;
			dr["PackUOM"] = detail.UOMCode;
			dr["ItemQty"] = detail.ItemQty;
			dr["QtyValidate"] = detail.ItemQty;
			dr["WtPerQty"] = detail.AdLsWeight;
			dr["GrossWeight"] = detail.GrossWeight;
			dr["SupplierQty"] = detail.SupplierQty;
			dr["CityName"] = detail.AreaCity;
			dr["WarehouseId"] = detail.WarehouseId;
			dr["Warehouse"] = detail.WareHouseCode;
			dr["RemarksDetail"] = detail.CommentsDetail;
			dr["ItemConditionId"] = detail.ItemConditionId;
			dr["RackId"] = detail.RackId;
			dr["RackName"] = detail.rackName;
			if (AddExtraColumns)
			{
				dr["CropYear"] = detail.CropYear;
				dr["ItemCondition"] = detail.ItemCondition;
			}
			dtdetail.Rows.Add(dr);
		}
	}

	private void HistoryComboFill(DataTable dt)
	{
		try
		{
			DataTable dtSupplier = new DataTable();
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("Supplier", typeof(string));
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Supplier")
					{
						dtSupplier.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtSupplier, cmbSupplierNameHistory, "Id", "Supplier", "Supplier Name", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private string DocumentNoDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		try
		{
			return InvGrn.GenerateInvGrnCode(new InvGrn
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = DocumentTypeId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			}).ToString();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void SupplierDtFillFromGlobal()
	{
		try
		{
			dtSupplier.Clear();
			List<getGlobalSupplierCustomer> obj = clsGlobalVariables.globalSupplierCustomer;
			if (obj != null && obj.Count > 0)
			{
				for (int i = 0; i < obj.Count; i++)
				{
					dtSupplier.Rows.Add(Conversion.ToInt((object)obj[i].Id), Conversion.ToString((object)obj[i].CompanyName), Conversion.ToString((object)obj[i].PartyCode), Conversion.ToInt((object)obj[i].GlAccountId), Conversion.ToInt((object)obj[i].CityId), Conversion.ToString((object)obj[i].CityName), Conversion.ToString((object)obj[i].MobilePersonal));
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindSupplierName()
	{
		try
		{
			int SupId = Conversion.ToInt(combsupplier.Value);
			if (dtSupplier.Rows.Count > 0)
			{
				InfragisticsHelper.BindAndRetainSelection(combsupplier, dtSupplier, "Id", "CompanyName", "Supplier Name", AllColumns: true, SupId, insertDefaultRow: false, null, null, ActivateRow: false);
				((UltraGridBase)combsupplier).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)combsupplier).DisplayLayout.Bands[0].Columns[4].Hidden = true;
			}
			else
			{
				((Control)(object)combsupplier).Text = string.Empty;
				((UltraGridBase)combsupplier).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void TransporterDtFillFromGlobal()
	{
		try
		{
			List<getGlobalVendorsAndCustomersForTransporter> lst = clsGlobalVariables.getGlobalVendorsAndCustomersForTransporters;
			dtTransporter.Clear();
			if (SubsidiaryAccountAllownOnVouchers)
			{
				if (lst != null && lst.Count > 0)
				{
					for (int i = 0; i < lst.Count; i++)
					{
						dtTransporter.Rows.Add(lst[i].GlAccountId, lst[i].CompanyName, lst[i].PartyCode, lst[i].Id);
					}
				}
				return;
			}
			int[] excludedIds = new int[11]
			{
				2, 4, 10, 11, 12, 13, 14, 15, 20, 21,
				22
			};
			IEnumerable<getGlobalAllAccountsWithCustomGroup> enumerable = clsGlobalVariables.AllAccountsWithCustomGroupId.Where((getGlobalAllAccountsWithCustomGroup r) => !excludedIds.Contains(r.AccountTypeId));
			HashSet<int> existingIds = new HashSet<int>();
			foreach (getGlobalAllAccountsWithCustomGroup acc in enumerable)
			{
				int id = Conversion.ToInt((object)acc.ChartOfAccountId);
				if (!existingIds.Contains(id))
				{
					dtTransporter.Rows.Add(acc.ChartOfAccountId, acc.AccountTitle, acc.AccountCode, 0);
				}
				existingIds.Add(id);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void TransporterAccountFill()
	{
		try
		{
			int Id = Conversion.ToInt(CmbTransport.Value);
			if (dtTransporter.Rows.Count > 0)
			{
				if (SubsidiaryAccountAllownOnVouchers)
				{
					InfragisticsHelper.BindAndRetainSelection(CmbTransport, dtTransporter, "SupplierCustomerId", "AccountTitle", "Transporter Name", AllColumns: true, Id, insertDefaultRow: true);
				}
				else
				{
					InfragisticsHelper.BindAndRetainSelection(CmbTransport, dtTransporter, "Id", "AccountTitle", "Transporter Name", AllColumns: true, Id, insertDefaultRow: true);
				}
				if (((UltraGridBase)CmbTransport).DataSource != null)
				{
					((UltraGridBase)CmbTransport).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void VehicleTypesBind(DataTable dtVehicle)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(combvehtyp, dtVehicle, "Id", "VehicleDescription", "Vehicle Type", AllColumns: true, Conversion.ToInt(combvehtyp.Value), insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeliveryTerm()
	{
		try
		{
			DataTable dtTerm = DeliveryTerm.FormHistory();
			InfragisticsHelper.BindAndRetainSelection(CmbDeliveryTerm, dtTerm, "Id", "Description", "Delivery Term", AllColumns: false, Conversion.ToInt(CmbDeliveryTerm.Value));
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
			Id = 0;
			((Control)(object)combsupplier).Text = string.Empty;
			txtGpNo.Text = string.Empty;
			txtGpId.Text = string.Empty;
			((Control)(object)combvehtyp).Text = string.Empty;
			combvehtyp.Value = string.Empty;
			((Control)(object)CmbTransport).Text = string.Empty;
			txtcarramount.Text = string.Empty;
			txtsuppwt.Text = string.Empty;
			txtfctwt.Text = string.Empty;
			txtwtdiff.Text = string.Empty;
			((UltraGridBase)combvehtyp).Rows[0].Activate();
			txtvehno.Text = string.Empty;
			txtbltyno.Text = string.Empty;
			txtremarks.Text = string.Empty;
			((Control)(object)CmbDeliveryTerm).Text = string.Empty;
			btnSave.Visible = true;
			DocDate.Focus();
			dtdetail.Clear();
			grd.DataSource = dtdetail;
			IsGeneralGp = false;
			BtnLoadOrder.Enabled = false;
			BtnLoadOrder.Visible = false;
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			txtcarramount.Enabled = true;
			((Control)(object)CmbTransport).Enabled = true;
			txtdocno.Text = DocumentNoDbCall();
			PendingGatePassForGrnLoad(PendingGpDataDbCall());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		reset();
	}

	private void GrnPackingMaterial_FormClosing(object sender, FormClosingEventArgs e)
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

	private DataTable PendingGpDataDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		try
		{
			return GatePassInward.PendingGatePassforGRNPackingMaterial(new GatePassInward
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 51,
				Id = Id,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PendingGatePassForGrnLoad(DataTable dtpendingGp)
	{
		try
		{
			if (dtpendingGp.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("RefDocumentTypeId", typeof(int));
				dtcol.Columns.Add("OrderId", typeof(int));
				dtcol.Columns.Add("OrderNo", typeof(int));
				dtcol.Columns.Add("GpSrNo", typeof(int));
				dtcol.Columns.Add("GpDate", typeof(DateTime));
				dtcol.Columns.Add("OrderType", typeof(string));
				dtcol.Columns.Add("SupplierName", typeof(string));
				dtcol.Columns.Add("VehicleType", typeof(string));
				dtcol.Columns.Add("VehicleNo", typeof(string));
				dtcol.Columns.Add("BiltyNo", typeof(string));
				dtcol.Columns.Add("VarietyName", typeof(string));
				dtcol.Columns.Add("Qty", typeof(double));
				dtcol.Columns.Add("SupplierWeight", typeof(double));
				dtcol.Columns.Add("FactoryWeight", typeof(double));
				dtcol.Columns.Add("ReceivedWeight", typeof(double));
				dtcol.Columns.Add("StockWeight", typeof(double));
				dtcol.Columns.Add("NetPaid", typeof(double));
				dtcol.Columns.Add("Status", typeof(string));
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("EntryUser", typeof(string));
				dtcol.Columns.Add("ModifyDate", typeof(DateTime));
				dtcol.Columns.Add("ModifyUser", typeof(string));
				dtcol.Columns.Add("DetailIdsCount", typeof(int));
				dtcol.Columns.Add("FreightOnCash", typeof(double));
				dtcol.Columns.Add("FreightId", typeof(int));
				dtcol.Clear();
				dtcol.BeginLoadData();
				for (int i = 0; i < dtpendingGp.Rows.Count; i++)
				{
					dtcol.Rows.Add(dtpendingGp.Rows[i]["Id"], dtpendingGp.Rows[i]["RefDocumentTypeId"], dtpendingGp.Rows[i]["PurchaseOrderId"], dtpendingGp.Rows[i]["PurchaseOrderNo"], dtpendingGp.Rows[i]["GpSrNo"], Conversion.ToDateTime(dtpendingGp.Rows[i]["GpDate"]).ToShortDateString(), dtpendingGp.Rows[i]["OrderType"], dtpendingGp.Rows[i]["SupplierName"], dtpendingGp.Rows[i]["VehicleType"], dtpendingGp.Rows[i]["VehicleNo"], dtpendingGp.Rows[i]["BiltyNo"], dtpendingGp.Rows[i]["VarietyName"], dtpendingGp.Rows[i]["Qty"], dtpendingGp.Rows[i]["SupplierWeight"], dtpendingGp.Rows[i]["FactoryWeight"], dtpendingGp.Rows[i]["ReceivedWeight"], dtpendingGp.Rows[i]["StockWeight"], dtpendingGp.Rows[i]["NetPaid"], dtpendingGp.Rows[i]["Status"], dtpendingGp.Rows[i]["EntryDate"], dtpendingGp.Rows[i]["EntryUser"], dtpendingGp.Rows[i]["ModifyDate"], dtpendingGp.Rows[i]["ModifyUser"], dtpendingGp.Rows[i]["DetailIdsCount"], dtpendingGp.Rows[i]["FreightOnCash"], dtpendingGp.Rows[i]["FreightId"]);
				}
				dtcol.EndLoadData();
				grdGp.DataSource = dtcol;
				grdGp.RetrieveStructure();
				grdGpSetting();
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

	private void grdGpSetting()
	{
		try
		{
			grdGp.RootTable.Columns["Id"].Visible = false;
			grdGp.RootTable.Columns["OrderId"].Visible = false;
			grdGp.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grdGp.RootTable.Columns["DetailIdsCount"].Visible = false;
			grdGp.RootTable.Columns["FreightOnCash"].Visible = false;
			grdGp.RootTable.Columns["FreightId"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grdGp, 2, 3, null, null, null, ColumnSetting: false);
			grdGp.RootTable.Columns["OrderNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdGp.RootTable.Columns["GpSrNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdGp.RootTable.Columns["GpDate"].Width = Constants.InventoryConstants.DateConstant + 15;
			grdGp.RootTable.Columns["OrderType"].Width = Constants.InventoryConstants.CastingType;
			grdGp.RootTable.Columns["SupplierName"].Width = Constants.InventoryConstants.SupplierName;
			grdGp.RootTable.Columns["VehicleType"].Width = Constants.InventoryConstants.VehicleTypeConstant;
			grdGp.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			grdGp.RootTable.Columns["BiltyNo"].Width = Constants.InventoryConstants.BiltyNoConstant;
			grdGp.RootTable.Columns["VarietyName"].Width = Constants.InventoryConstants.ItemName + 20;
			grdGp.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
			grdGp.RootTable.Columns["SupplierWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdGp.RootTable.Columns["FactoryWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdGp.RootTable.Columns["ReceivedWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdGp.RootTable.Columns["StockWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdGp.RootTable.Columns["NetPaid"].Width = Constants.InventoryConstants.AmountConstant;
			grdGp.RootTable.Columns["Status"].Width = Constants.InventoryConstants.StatusConstant;
			grdGp.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdGp.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 50;
			grdGp.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdGp.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 50;
			grdGp.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdGp.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdGp.RootTable.Columns["VarietyName"].Caption = "Item Name";
			grdGp.RootTable.Columns["GpSrNo"].Caption = "Gp No";
			GridEX_Helper.AddButton(grdGp, "Load", "Load", 45, 0);
			grdGp.FrozenColumns = 1;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGp_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdGp.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "Load")
			{
				LoadGpData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void LoadGpData(int GpId = 0)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = ((GpId == 0) ? grdGp.CurrentRow : GridEX_Helper.GetRowByColumnNameAndValue(grdGp, "Id", GpId));
			if (r != null && (int)r.RowType == 0)
			{
				if (!btnSave.Visible || !btnSave.Enabled)
				{
					throw new Exception("Please Reset the form First...");
				}
				IsGeneralGp = false;
				if (!(Conversion.ToString(r.Cells["Status"].Value) == "Accepted"))
				{
					throw new Exception("Status Not Accepted Please check status");
				}
				if (Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value) == 52)
				{
					IsGeneralGp = true;
				}
				txtGpId.Text = Conversion.ToString(r.Cells["Id"].Value);
				txtGpNo.Text = Conversion.ToString(r.Cells["GpSrNo"].Value);
				txtbltyno.Text = Conversion.ToString(r.Cells["BiltyNo"].Value);
				((Control)(object)combvehtyp).Text = Conversion.ToString(r.Cells["VehicleType"].Value);
				txtvehno.Text = Conversion.ToString(r.Cells["VehicleNo"].Value);
				txtfctwt.Text = Conversion.ToString(r.Cells["FactoryWeight"].Value);
				txtsuppwt.Text = Conversion.ToString(r.Cells["SupplierWeight"].Value);
				double freight = Conversion.ToDouble(r.Cells["NetPaid"].Value);
				if (freight > 0.0)
				{
					txtcarramount.Text = freight.ToString(clsGlobalVariables.stringFormatsingle);
				}
				if (Conversion.ToInt(r.Cells["FreightId"].Value) > 0)
				{
					CmbTransport.Value = Conversion.ToInt(r.Cells["FreightId"].Value);
				}
				if (IsGeneralGp)
				{
					LoadPO = new LoadPurchaseOrderPM(UserAccount);
					LoadPO.DocumentTypeId = 700;
					LoadPO.ShowDialog();
					LoadPO.BringToFront();
					LoadInGridDetail();
				}
				else
				{
					BindGridByOrderId(Conversion.ToInt(r.Cells["OrderId"].Value), Conversion.ToInt((object)txtGpId.Text));
				}
				txtGpNo.Enabled = false;
				txtbltyno.Enabled = false;
				((Control)(object)combvehtyp).Enabled = false;
				txtvehno.Enabled = false;
				txtfctwt.Enabled = false;
				txtsuppwt.Enabled = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			Id = Conversion.ToInt((object)GrdHistory.CurrentRow.Cells["Id"].Value.ToString());
			ReadById(Id);
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
			grn.BranchesId = UserAccount.BranchesId;
			grn.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			grn.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			grn.NoOfRecords = NoOfRecords;
			grn.DocumentTypeId = DocumentTypeId;
			if (!grn.CanViewAllRecord)
			{
				grn.EntryUser = UserAccount.ID;
			}
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					grn.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					grn.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					grn.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					grn.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					grn.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					grn.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					grn.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					grn.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			grn.FromDocNo = Conversion.ToInt((object)txtFromDocNoHistory.Text);
			grn.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text);
			grn.SupplierCustomerId = Conversion.ToInt(cmbSupplierNameHistory.Value);
			if (RadReffered.Checked)
			{
				grn.ActionId = 1;
			}
			else if (RadNotReffered.Checked)
			{
				grn.ActionId = 2;
			}
			DataTable dthis = InvGrn.GetHisoty(grn);
			if (dthis.Rows.Count > 0)
			{
				DataTable dtHisGrid = new DataTable();
				dtHisGrid.Columns.Add("RecordNo", typeof(int));
				dtHisGrid.Columns.Add("Id", typeof(int));
				dtHisGrid.Columns.Add("DocumentTypeId", typeof(int));
				dtHisGrid.Columns.Add("OrderId", typeof(int));
				dtHisGrid.Columns.Add("OrderType");
				dtHisGrid.Columns.Add("OrderNo", typeof(int));
				dtHisGrid.Columns.Add("DocDate", typeof(DateTime));
				dtHisGrid.Columns.Add("DocNo", typeof(int));
				dtHisGrid.Columns.Add("InvoiceNo", typeof(string));
				dtHisGrid.Columns.Add("SupplierName");
				dtHisGrid.Columns.Add("DeliveryTerm");
				dtHisGrid.Columns.Add("GpNoId", typeof(int));
				dtHisGrid.Columns.Add("GpNo", typeof(int));
				dtHisGrid.Columns.Add("VehicleNo");
				dtHisGrid.Columns.Add("BiltyNo");
				dtHisGrid.Columns.Add("FactoryWeight", typeof(double));
				dtHisGrid.Columns.Add("PartyWeight", typeof(double));
				dtHisGrid.Columns.Add("DiffWeight", typeof(double));
				dtHisGrid.Columns.Add("TransporterName");
				dtHisGrid.Columns.Add("FreightAmount", typeof(double));
				dtHisGrid.Columns.Add("EntryDate", typeof(DateTime));
				dtHisGrid.Columns.Add("EntryUser");
				dtHisGrid.Columns.Add("ModifyDate", typeof(DateTime));
				dtHisGrid.Columns.Add("ModifyUser");
				dtHisGrid.Columns.Add("NoOfAttachments");
				dtHisGrid.Columns.Add("RemarksHeader");
				dtHisGrid.Rows.Clear();
				for (int i = 0; i < dthis.Rows.Count; i++)
				{
					dtHisGrid.Rows.Add(dthis.Rows[i]["RecordNo"], dthis.Rows[i]["Id"], dthis.Rows[i]["DocumentTypeId"], dthis.Rows[i]["OrderId"], dthis.Rows[i]["OrderType"], dthis.Rows[i]["OrderNo"], Conversion.ToDateTime(dthis.Rows[i]["DocDate"]).ToShortDateString(), dthis.Rows[i]["DocNo"], dthis.Rows[i]["InvoiceNo"], dthis.Rows[i]["SupplierName"], dthis.Rows[i]["DeliveryTerm"], dthis.Rows[i]["InwardGatePassId"], dthis.Rows[i]["GpNo"], dthis.Rows[i]["VehicleNo"], dthis.Rows[i]["BiltyNo"], dthis.Rows[i]["FactoryWeight"], dthis.Rows[i]["PartyWeight"], Conversion.ToDouble(dthis.Rows[i]["FactoryWeight"]) - Conversion.ToDouble(dthis.Rows[i]["PartyWeight"]), dthis.Rows[i]["Transporter"], dthis.Rows[i]["CarriageAmount"], dthis.Rows[i]["EntryDate"], dthis.Rows[i]["EntryUser"], dthis.Rows[i]["ModifyDate"], dthis.Rows[i]["ModifyUser"], dthis.Rows[i]["NoOfAttachments"], dthis.Rows[i]["RemarksHeader"]);
				}
				GrdHistory.DataSource = dtHisGrid;
				GrdHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				GrdHistory.DataSource = null;
				GrdHistoryDetail.DataSource = null;
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
			List<string> hiddenColumns = new List<string> { "Id", "DocumentTypeId", "RecordNo", "OrderNo", "OrderId", "GpNoId", "OrderType" };
			GridEX_Helper.GridWrappingAndColumnSettings(GrdHistory, 2, 3, hiddenColumns);
			GrdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			GrdHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			GrdHistory.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yyyy";
			GrdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			GrdHistory.RootTable.Columns["OrderType"].Width = Constants.InventoryConstants.StringConstant;
			GrdHistory.RootTable.Columns["OrderNo"].Width = Constants.InventoryConstants.DocNoConstant;
			GrdHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant + 15;
			GrdHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			GrdHistory.RootTable.Columns["InvoiceNo"].Width = Constants.InventoryConstants.DocNoConstant;
			GrdHistory.RootTable.Columns["SupplierName"].Width = Constants.InventoryConstants.SupplierName;
			GrdHistory.RootTable.Columns["DeliveryTerm"].Width = Constants.InventoryConstants.TermConstant;
			GrdHistory.RootTable.Columns["GpNo"].Width = Constants.InventoryConstants.DocNoConstant;
			GrdHistory.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			GrdHistory.RootTable.Columns["BiltyNo"].Width = Constants.InventoryConstants.BiltyNoConstant;
			GrdHistory.RootTable.Columns["FactoryWeight"].Width = Constants.InventoryConstants.WeightConstant;
			GrdHistory.RootTable.Columns["PartyWeight"].Width = Constants.InventoryConstants.WeightConstant;
			GrdHistory.RootTable.Columns["DiffWeight"].Width = Constants.InventoryConstants.WeightConstant;
			GrdHistory.RootTable.Columns["TransporterName"].Width = Constants.InventoryConstants.AccountTitle;
			GrdHistory.RootTable.Columns["FreightAmount"].Width = Constants.InventoryConstants.AmountConstant;
			GrdHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			GrdHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			GrdHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			GrdHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			GrdHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			GrdHistory.RootTable.Columns["RemarksHeader"].Width = Constants.InventoryConstants.RemarksConstants;
			GrdHistory.RootTable.Columns.Add("Edit");
			GrdHistory.RootTable.Columns["Edit"].Caption = "Edit";
			GrdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			GrdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			GrdHistory.RootTable.Columns["Edit"].Width = 35;
			GrdHistory.RootTable.Columns.Add("Print");
			GrdHistory.RootTable.Columns["Print"].Caption = "Print";
			GrdHistory.RootTable.Columns["Print"].ButtonText = "Print";
			GrdHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			GrdHistory.RootTable.Columns["Print"].Width = 40;
			GrdHistory.RootTable.Columns.Add("PrintII");
			GrdHistory.RootTable.Columns["PrintII"].Caption = "PrintII";
			GrdHistory.RootTable.Columns["PrintII"].ButtonText = "PrintII";
			GrdHistory.RootTable.Columns["PrintII"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistory.RootTable.Columns["PrintII"].ButtonStyle = (ButtonStyle)5;
			GrdHistory.RootTable.Columns["PrintII"].Width = 50;
			GrdHistory.RootTable.Columns["Edit"].Position = 0;
			GrdHistory.RootTable.Columns["Print"].Position = 1;
			GrdHistory.RootTable.Columns["PrintII"].Position = 2;
			GrdHistory.FrozenColumns = 3;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
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
			if (e.Column.Key == "NoOfAttachments")
			{
				GridEXRow item = GrdHistory.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), DocumentTypeId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				if (Conversion.ToInt(GrdHistory.CurrentRow.Cells["InvoiceNo"].Value) > 0)
				{
					MessageBox.Show("This Document is reffered in invoice. So you can't update this record.", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				Id = Conversion.ToInt(GrdHistory.CurrentRow.Cells["Id"].Value);
				ReadById(Id);
			}
			if (e.Column.Key == "Print")
			{
				GenerateReport(Conversion.ToInt(GrdHistory.CurrentRow.Cells["Id"].Value));
			}
			if (e.Column.Key == "PrintII")
			{
				CommonServices.GrnPmSlipWithSubReport214_01(Conversion.ToInt(GrdHistory.CurrentRow.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindHistoryDetail(int HisId)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (GrdHistory.CurrentRow == null || (int)GrdHistory.CurrentRow.RowType != 0 || GrdHistory.CurrentRow.Cells.Count <= 0)
			{
				return;
			}
			InvGrn invGrn = InvGrn.GetByID(HisId);
			if (invGrn.invGrnDetail.Count > 0)
			{
				DataTable dthistorydetail = new DataTable();
				dthistorydetail.Columns.Add("OrderId");
				dthistorydetail.Columns.Add("OrderDetailId");
				dthistorydetail.Columns.Add("OrderDate", typeof(DateTime));
				dthistorydetail.Columns.Add("OrderNo", typeof(int));
				dthistorydetail.Columns.Add("ItemName");
				dthistorydetail.Columns.Add("CropYear");
				dthistorydetail.Columns.Add("PackingDate", typeof(DateTime));
				dthistorydetail.Columns.Add("ExpiryDate", typeof(DateTime));
				dthistorydetail.Columns.Add("PackUOM");
				dthistorydetail.Columns.Add("ItemCondition");
				dthistorydetail.Columns.Add("ItemQty", typeof(double));
				dthistorydetail.Columns.Add("WtPerQty", typeof(double));
				dthistorydetail.Columns.Add("GrossWeight", typeof(double));
				dthistorydetail.Columns.Add("CityName");
				dthistorydetail.Columns.Add("Warehouse");
				dthistorydetail.Columns.Add("RackName");
				dthistorydetail.Columns.Add("RemarksDetail");
				for (int i = 0; i < invGrn.invGrnDetail.Count; i++)
				{
					dthistorydetail.Rows.Add(invGrn.invGrnDetail[i].PurchaseOrderId, invGrn.invGrnDetail[i].PurchaseOrderDetailId, Conversion.ToDateTime((object)invGrn.invGrnDetail[i].PurchaseOrderDate).ToShortDateString(), invGrn.invGrnDetail[i].PurchaserOrderNo, invGrn.invGrnDetail[i].Item, invGrn.invGrnDetail[i].CropYear, invGrn.invGrnDetail[i].PackingDate, invGrn.invGrnDetail[i].ExpiryDate, invGrn.invGrnDetail[i].UOMCode, invGrn.invGrnDetail[i].ConditionStatus, invGrn.invGrnDetail[i].ItemQty, invGrn.invGrnDetail[i].AdLsWeight, invGrn.invGrnDetail[i].GrossWeight, invGrn.invGrnDetail[i].AreaCity, invGrn.invGrnDetail[i].WareHouseCode, invGrn.invGrnDetail[i].rackName, invGrn.invGrnDetail[i].CommentsDetail);
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
			GrdHistoryDetail.RootTable.Columns["OrderId"].Visible = false;
			GrdHistoryDetail.RootTable.Columns["OrderDetailId"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(GrdHistoryDetail, 2, 3);
			GrdHistoryDetail.RootTable.Columns["OrderDate"].Width = Constants.InventoryConstants.DateConstant + 10;
			GrdHistoryDetail.RootTable.Columns["OrderNo"].Width = Constants.InventoryConstants.DocNoConstant;
			GrdHistoryDetail.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			GrdHistoryDetail.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			GrdHistoryDetail.RootTable.Columns["PackingDate"].Width = Constants.InventoryConstants.DateConstant;
			GrdHistoryDetail.RootTable.Columns["ExpiryDate"].Width = Constants.InventoryConstants.DateConstant;
			GrdHistoryDetail.RootTable.Columns["PackUOM"].Width = Constants.InventoryConstants.UomConstant;
			GrdHistoryDetail.RootTable.Columns["ItemCondition"].Width = Constants.InventoryConstants.StringConstant;
			GrdHistoryDetail.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			GrdHistoryDetail.RootTable.Columns["WtPerQty"].Width = Constants.InventoryConstants.QtyConstant;
			GrdHistoryDetail.RootTable.Columns["GrossWeight"].Width = Constants.InventoryConstants.WeightConstant;
			GrdHistoryDetail.RootTable.Columns["CityName"].Width = Constants.InventoryConstants.CityName;
			GrdHistoryDetail.RootTable.Columns["Warehouse"].Width = Constants.InventoryConstants.WareHouseName;
			GrdHistoryDetail.RootTable.Columns["RackName"].Width = Constants.InventoryConstants.StringConstant;
			GrdHistoryDetail.RootTable.Columns["RemarksDetail"].Width = Constants.InventoryConstants.RemarksConstants;
			GrdHistoryDetail.RootTable.Columns["PackingDate"].FormatString = "MMM-yyyy";
			GrdHistoryDetail.RootTable.Columns["ExpiryDate"].FormatString = "MMM-yyyy";
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
			FromDateHistory.Focus();
		}
		else if (tabControl1.SelectedIndex == 0)
		{
			DocDate.Focus();
		}
	}

	private void GrdHistory_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			GridEXRow currentRow = GrdHistory.CurrentRow;
			if (currentRow != null && currentRow.Cells.Count > 0)
			{
				int id = Conversion.ToInt(GrdHistory.CurrentRow.Cells["Id"].Value);
				BindHistoryDetail(id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					btnNew_Click(null, null);
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					btnNewHistory_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					toolStripButton1_Click(null, null);
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					btnRefreshHistory_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					if (btnSave.Visible && btnSave.Enabled)
					{
						btnSave_Click(null, null);
					}
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					btnshowHistory_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
			{
				btnUpdate_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.P && tabControl1.SelectedIndex == 0 && printToolStripButton.Visible && printToolStripButton.Enabled)
			{
				printToolStripButton_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.L && tabControl1.SelectedIndex == 1)
			{
				btnLoadAll_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.H && tabControl1.SelectedIndex == 1)
			{
				btnGrnFormHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					if (((Control)(object)grdGp).Focused)
					{
						((Control)(object)grd).Focus();
					}
					else if (((Control)(object)grd).Focused)
					{
						((Control)(object)grdGp).Focus();
					}
					else
					{
						((Control)(object)grdGp).Focus();
					}
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					((Control)(object)GrdHistory).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					DocDate.Focus();
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					FromDateHistory.Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					DocDate.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					FromDateHistory.Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				DocDate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.F10)
			{
				btnAttachment_Click(null, null);
			}
			if (e.Control && e.Shift && e.KeyCode == Keys.Delete && formright.DoHaveCanDelete)
			{
				btnDelete_Click(null, null);
			}
			if (e.Control && e.Alt)
			{
				BtnShortCutkeys_Click(null, null);
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
			if (FormInitialized)
			{
				DataTable dt = (DataTable)GrdHistory.DataSource;
				if (dt != null && dt.Rows.Count > 0)
				{
					GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, GrdHistory, dt);
				}
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
			if (FormInitialized)
			{
				DataTable dt = (DataTable)grd.DataSource;
				if (dt != null && dt.Rows.Count > 0)
				{
					GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grd, dt);
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
			if (FormInitialized)
			{
				DataTable dt = (DataTable)grdGp.DataSource;
				if (dt != null && dt.Rows.Count > 0)
				{
					GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdGp, dt);
				}
			}
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

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void txtsuppwt_TextChanged(object sender, EventArgs e)
	{
		try
		{
			txtfctwt.Text = Conversion.ToString((object)txtsuppwt.Text.Trim());
			TotalSupplierWeight();
			StockWeightUpdateFromGridOnDeliveryTermChange();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtfctwt_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalSupplierWeight();
			StockWeightUpdateFromGridOnDeliveryTermChange();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbDeliveryTerm_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)CmbDeliveryTerm).Text == "Ponch" || ((Control)(object)CmbDeliveryTerm).Text == "Ponch & PartyWeight" || ((Control)(object)CmbDeliveryTerm).Text == "Ponch & FactoryWeight")
			{
				((Control)(object)CmbTransport).Text = string.Empty;
				txtcarramount.Text = "0";
				txtcarramount.Enabled = false;
				((Control)(object)CmbTransport).Enabled = false;
			}
			else
			{
				txtcarramount.Enabled = true;
				((Control)(object)CmbTransport).Enabled = true;
			}
			StockWeightUpdateFromGridOnDeliveryTermChange();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbDeliveryTerm_Leave(object sender, EventArgs e)
	{
		try
		{
			StockWeightUpdateFromGridOnDeliveryTermChange();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StockWeightUpdateFromGridOnDeliveryTermChange()
	{
	}

	private void TotalSupplierWeight()
	{
		try
		{
			if (txtsuppwt.Text != string.Empty && txtfctwt.Text != string.Empty)
			{
				double num = double.Parse(txtsuppwt.Text.Trim());
				double FactoryWeight = double.Parse(txtfctwt.Text.Trim());
				txtwtdiff.Text = (num - FactoryWeight).ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WeightPerQtyCalculate()
	{
		try
		{
			if (grd.CurrentRow.Cells.Count > 0)
			{
				double num = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["GrossWeight"], (AggregateFunction)2).ToString());
				double QtyTotal = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemQty"], (AggregateFunction)2).ToString());
				double AvgWtPerQty = num / QtyTotal;
				txtAvgWtPerQty.Text = Conversion.ToString((object)Math.Round(AvgWtPerQty, 2));
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
			List<string> allServices = new List<string> { "ItemConditions", "AllCities", "CropYear", "RacksWithWarehouseAndItem" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			WagesActiveOrInActive = GlobalVariables_Helper.GetWagesRefDocumentsStatusById(DocumentTypeId);
			WbNotCompulsoryForPmPurchaseOrder = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("WbNotCompulsoryForPmPurchaseOrder"));
			GridComboBind();
			SupplierDtFillFromGlobal();
			BindSupplierName();
			TransporterDtFillFromGlobal();
			TransporterAccountFill();
			VehicleTypesBind(VehicleType.GetAll());
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
			CommonServices.GrnPMSlipReport214(PrintId);
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

	private void CityDefine_Click(object sender, EventArgs e)
	{
		new DefineCity(UserAccount).Show();
	}

	private void btnGrnFormHistory_Click(object sender, EventArgs e)
	{
		try
		{
			GrnPackingMaterialRegister obj = new GrnPackingMaterialRegister(UserAccount);
			if (clsGlobalVariables.ScreenViewReights.Count > 0)
			{
				if (bool.Parse(clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "GrnPackingMaterialRegister").ToList()[0].Value.ToString()))
				{
					obj.Show();
				}
				else
				{
					MessageBox.Show("You dont have Screen Right");
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindGridByOrderId(int OrderId, int GpId)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		try
		{
			dtdetail.Rows.Clear();
			DataTable dt = PurchaseOrder.PurchaseOrderLoaderForPackingMaterial(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 700,
				OrderId = OrderId,
				GpId = GpId
			});
			if (dt.Rows.Count == 0)
			{
				return;
			}
			dtdetail.BeginLoadData();
			int wareHouseConfigId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("PackingMaterialDefaultWarehouse"));
			BillCalculateTypeId = Conversion.ToInt(dt.Rows[0]["BillCalculateTypeId"]);
			if (BillCalculateTypeId == 1)
			{
				txtBillCalculateType.Text = "OnWeight";
			}
			else if (BillCalculateTypeId == 2)
			{
				txtBillCalculateType.Text = "OnQty";
			}
			else
			{
				txtBillCalculateType.Text = "";
			}
			int selectedSupplier = 0;
			foreach (DataRow src in dt.Rows)
			{
				double qty = Conversion.ToDouble(src["BalQty"]);
				double wtPerQty = Conversion.ToDouble(src["WeightPerQty"]);
				DataRow newRow = dtdetail.NewRow();
				newRow["Id"] = 0;
				newRow["OrderId"] = src["Id"];
				newRow["OrderDetailId"] = src["OrderDetailId"];
				newRow["OrderDate"] = Conversion.ToDateTime(src["DocDate"]).ToShortDateString();
				newRow["OrderNo"] = src["DocNo"];
				newRow["ItemId"] = src["ItemId"];
				newRow["ItemName"] = src["ItemName"];
				newRow["CropYearId"] = src["CropYearId"];
				newRow["PackingDate"] = src["PackingDate"];
				newRow["ExpiryDate"] = src["ExpiryDate"];
				newRow["PackUOMId"] = src["ItemUOMId"];
				newRow["PackUOM"] = src["PackUom"];
				newRow["ItemQty"] = qty;
				newRow["QtyValidate"] = qty;
				newRow["WtPerQty"] = wtPerQty;
				newRow["GrossWeight"] = qty * wtPerQty;
				newRow["SupplierQty"] = 0;
				newRow["CityName"] = "";
				newRow["RemarksDetail"] = src["RemarksHeader"];
				newRow["ItemConditionId"] = 0;
				Dictionary<int, List<RacksWithWarehouseAndItem>> dictionary = (from x in clsGlobalVariables.racksWithWarehouseAndItems
					group x by x.ItemId).ToDictionary((IGrouping<int, RacksWithWarehouseAndItem> g) => g.Key, (IGrouping<int, RacksWithWarehouseAndItem> g) => g.ToList());
				int itemId = Conversion.ToInt(src["ItemId"]);
				if (dictionary.TryGetValue(itemId, out var itemData))
				{
					(int, string, int, string) result = ResolveWarehouseAndRack(itemData, wareHouseConfigId);
					if (result.Item1 > 0)
					{
						newRow["WarehouseId"] = result.Item1;
						newRow["Warehouse"] = result.Item2;
					}
					if (result.Item3 > 0)
					{
						newRow["RackId"] = result.Item3;
						newRow["RackName"] = result.Item4;
					}
				}
				dtdetail.Rows.Add(newRow);
				selectedSupplier = Conversion.ToInt(src["OrderSupCustId"]);
			}
			dtdetail.EndLoadData();
			combsupplier.Value = selectedSupplier;
			((Control)(object)CmbDeliveryTerm).Text = Conversion.ToString(dt.Rows[0]["DeliveryTerm"]);
			grd.DataSource = dtdetail;
			grd.RetrieveStructure();
			grdSettings(grd);
			WeightPerQtyCalculate();
			((Control)(object)combsupplier).Enabled = false;
			((Control)(object)CmbDeliveryTerm).Enabled = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridComboBind()
	{
		try
		{
			DataTable dtCity = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalAllCities, (GlobalAllCity x) => x.Id, (GlobalAllCity x) => x.CityName);
			GridEX_Helper.ComboBind(grd.RootTable.Columns["CityName"], dtCity, "Id", "Description", "City Name", 100);
			DataTable dtCrop = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalCropYear, (GlobalCropYear x) => x.Id, (GlobalCropYear x) => x.CropYear);
			GridEX_Helper.ComboBind(grd.RootTable.Columns["CropYearId"], dtCrop, "Id", "Description", "Crop Year", 100);
			DataTable dtCondition = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalItemConditions.Where((GlobalItemCondition r) => r.Id != 4).ToList(), (GlobalItemCondition x) => x.Id, (GlobalItemCondition x) => x.ConditionStatus);
			GridEX_Helper.ComboBind(grd.RootTable.Columns["ItemConditionId"], dtCondition, "Id", "Description", "Item Condition", 90, limitToList: true, editType: true, grd.RootTable.Columns["PackUOM"].Position + 1);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void grdSettings(GridEX gridEX)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "OrderDetailId", "QtyValidate", "OrderId", "Id", "ItemId", "PackUOMId", "WarehouseId", "RackId" };
			Dictionary<string, EditType> editableColumns = null;
			if (((Control)(object)gridEX).Name == "grd")
			{
				editableColumns = new Dictionary<string, EditType>
				{
					{
						"PackingDate",
						(EditType)2
					},
					{
						"ExpiryDate",
						(EditType)2
					},
					{
						"ItemQty",
						(EditType)1
					},
					{
						"GrossWeight",
						(EditType)1
					},
					{
						"SupplierQty",
						(EditType)1
					},
					{
						"RemarksDetail",
						(EditType)1
					}
				};
			}
			GridEX_Helper.GridWrappingAndColumnSettings(gridEX, 2, 3, hiddenColumns, editableColumns);
			grd.RootTable.Columns["PackingDate"].EditType = (EditType)2;
			grd.RootTable.Columns["PackingDate"].FormatString = "MMM-yyyy";
			grd.RootTable.Columns["ExpiryDate"].EditType = (EditType)2;
			grd.RootTable.Columns["ExpiryDate"].FormatString = "MMM-yyyy";
			if (((Control)(object)gridEX).Name == "grd")
			{
				gridEX.ColumnAutoResize = false;
				GridComboBind();
				GridEX_Helper.AddButton(grd, "Delete", "X", 20, 0);
				GridEX_Helper.AddButton(grd, "Add", "+", 20, 1);
				grd.RootTable.Columns["RackName"].Position = grd.RootTable.Columns["Warehouse"].Position + 1;
				ctrlGrdBar1_Load(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (e.Column.Key == "Delete")
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) != 0)
				{
					MessageBox.Show("You Can't Delete Already Saved Row......");
					return;
				}
				if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
			}
			if (e.Column.Key == "Add")
			{
				DataRow dr = dtdetail.Rows[r.RowIndex];
				dtdetail.Rows.Add(dr.ItemArray);
				grd.DataSource = dtdetail;
				if (btnUpdate.Visible && btnUpdate.Enabled)
				{
					dtdetail.Rows[dtdetail.Rows.Count - 1]["Id"] = 0;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				string ColumnName = e.Column.Key;
				if (ColumnName == "ExpiryDate" || ColumnName == "PackingDate")
				{
					Conversion.ToDateTime(item.Cells[ColumnName].Value);
					DateTime selectedDate = Conversion.ToDateTime(item.Cells[ColumnName].Value);
					item.Cells[ColumnName].Value = new DateTime(selectedDate.Year, selectedDate.Month, 1);
				}
				if (e.Column.Key == "ItemQty")
				{
					double num = Conversion.ToDouble((object)item.Cells["ItemQty"].Value.ToString());
					double WtPerQty = Conversion.ToDouble((object)item.Cells["WtPerQty"].Value.ToString());
					double Gw = num * WtPerQty;
					item.Cells["GrossWeight"].Value = Conversion.ToString((object)Gw);
				}
			}
			grd.UpdateData();
			WeightPerQtyCalculate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grd.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grd.CurrentColumn.Key == "Delete")
				{
					if (Conversion.ToInt(r.Cells["Id"].Value) != 0)
					{
						MessageBox.Show("You Can't Delete Already Saved Row......");
						return;
					}
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
				}
				else if (grd.CurrentColumn.Key == "Add")
				{
					DataRow dr = dtdetail.Rows[r.RowIndex];
					dtdetail.Rows.Add(dr.ItemArray);
					grd.DataSource = dtdetail;
					if (btnUpdate.Visible && btnUpdate.Enabled)
					{
						dtdetail.Rows[dtdetail.Rows.Count - 1]["Id"] = 0;
					}
				}
			}
			else if (e.Control && e.KeyCode == Keys.D)
			{
				if (grd.GetRows().Count() > 0)
				{
					DataRow dr2 = dtdetail.Rows[r.RowIndex];
					dtdetail.Rows.Add(dr2.ItemArray);
					grd.DataSource = dtdetail;
					if (btnUpdate.Visible && btnUpdate.Enabled)
					{
						dtdetail.Rows[dtdetail.Rows.Count - 1]["Id"] = 0;
					}
				}
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) != 0)
				{
					MessageBox.Show("You Can't Delete Already Saved Row......");
					return;
				}
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
			}
			if (e.KeyCode != Keys.F1)
			{
				return;
			}
			int WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
			int ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
			int RackId = Conversion.ToInt(r.Cells["RackId"].Value);
			if (grd.CurrentColumn.Key == "WarehouseId" || grd.CurrentColumn.Key == "Warehouse")
			{
				List<RacksWithWarehouseAndItem> list = (from x in clsGlobalVariables.racksWithWarehouseAndItems
					where x.ItemId == ItemId
					group x by x.WarehouseId into g
					select g.First()).ToList();
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("WareHouseName", typeof(string));
				dt.BeginLoadData();
				foreach (RacksWithWarehouseAndItem w in list)
				{
					dt.Rows.Add(w.WarehouseId, w.WareHouseName);
				}
				dt.EndLoadData();
				using (GrdPopUp obj = new GrdPopUp(dt, "Id", "WareHouseName"))
				{
					obj.StartPosition = FormStartPosition.CenterScreen;
					obj.ShowDialog();
					int selectedId2 = Conversion.ToInt((object)obj.ReturnId);
					string selectedName = Conversion.ToString((object)obj.ReturnName);
					r.Cells["WarehouseId"].Value = selectedId2;
					r.Cells["Warehouse"].Value = selectedName;
					List<RacksWithWarehouseAndItem> racks = clsGlobalVariables.racksWithWarehouseAndItems.Where((RacksWithWarehouseAndItem x) => x.ItemId == ItemId && x.WarehouseId == selectedId2).ToList();
					if (RackId > 0)
					{
						if (!racks.Any((RacksWithWarehouseAndItem x) => x.Id == RackId))
						{
							if (racks.Count > 1)
							{
								r.Cells["RackId"].Value = 0;
								r.Cells["RackName"].Value = "";
							}
							else if (racks.Count == 1)
							{
								RacksWithWarehouseAndItem singleRack = racks.First();
								r.Cells["RackId"].Value = singleRack.Id;
								r.Cells["RackName"].Value = singleRack.RackName;
							}
							else
							{
								r.Cells["RackId"].Value = 0;
								r.Cells["RackName"].Value = "";
							}
						}
					}
					else if (racks.Count == 1)
					{
						RacksWithWarehouseAndItem singleRack2 = racks.First();
						r.Cells["RackId"].Value = singleRack2.Id;
						r.Cells["RackName"].Value = singleRack2.RackName;
					}
				}
				grd.UpdateData();
			}
			else
			{
				if (!(grd.CurrentColumn.Key == "RackId") && !(grd.CurrentColumn.Key == "RackName"))
				{
					return;
				}
				List<RacksWithWarehouseAndItem> filteredRacks = (from rr in clsGlobalVariables.racksWithWarehouseAndItems
					where rr.ItemId == ItemId && (WarehouseId == 0 || rr.WarehouseId == WarehouseId)
					select rr into row
					group row by row.Id into g
					select g.First()).ToList();
				if (filteredRacks.Count == 0)
				{
					return;
				}
				DataTable dt2 = new DataTable();
				dt2.Columns.Add("Id", typeof(int));
				dt2.Columns.Add("RackName", typeof(string));
				dt2.Columns.Add("WarehouseId", typeof(int));
				dt2.Columns.Add("WarehouseName", typeof(string));
				dt2.BeginLoadData();
				foreach (RacksWithWarehouseAndItem item in filteredRacks)
				{
					dt2.Rows.Add(item.Id, item.RackName, item.WarehouseId, item.WareHouseName);
				}
				dt2.EndLoadData();
				using (GrdPopUp grdPopUp = new GrdPopUp(dt2, "Id", "RackName", "WarehouseName"))
				{
					grdPopUp.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp.ShowDialog();
					int selectedId = Conversion.ToInt((object)grdPopUp.ReturnId);
					string selectedName2 = Conversion.ToString((object)grdPopUp.ReturnName);
					r.Cells["RackId"].Value = selectedId;
					r.Cells["RackName"].Value = selectedName2;
					if (WarehouseId == 0)
					{
						RacksWithWarehouseAndItem selected = filteredRacks.FirstOrDefault((RacksWithWarehouseAndItem x) => x.Id == selectedId);
						if (selected != null)
						{
							r.Cells["WarehouseId"].Value = selected.WarehouseId;
							r.Cells["Warehouse"].Value = selected.WareHouseName;
						}
					}
				}
				grd.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridDetail()
	{
		try
		{
			if (LoadPO.dtForLoad.Rows.Count == 0)
			{
				return;
			}
			DataTable sourceTable = LoadPO.dtForLoad;
			BillCalculateTypeId = Conversion.ToInt(sourceTable.Rows[0]["BillCalculateTypeId"]);
			if (BillCalculateTypeId == 1)
			{
				txtBillCalculateType.Text = "OnWeight";
			}
			else if (BillCalculateTypeId == 2)
			{
				txtBillCalculateType.Text = "OnQty";
			}
			else
			{
				txtBillCalculateType.Text = "";
			}
			HashSet<int> existingOrderDetailIds = new HashSet<int>();
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow row in rows)
			{
				existingOrderDetailIds.Add(Conversion.ToInt(row.Cells["OrderDetailId"].Value));
			}
			int selectedSupplier = Conversion.ToInt(combsupplier.Value);
			int wareHouseConfigId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("PackingMaterialDefaultWarehouse"));
			dtdetail.BeginLoadData();
			foreach (DataRow src in sourceTable.Rows)
			{
				int orderDetailId = Conversion.ToInt(src["OrderDetailId"]);
				int rowSupplierId = Conversion.ToInt(src["OrderSupCustId"]);
				if (selectedSupplier > 0 && selectedSupplier != rowSupplierId)
				{
					throw new Exception("Data against another Party Already Exist in Detail");
				}
				if (existingOrderDetailIds.Contains(orderDetailId))
				{
					continue;
				}
				double qty = Conversion.ToDouble(src["ItemQty"]);
				double wtPerQty = Conversion.ToDouble(src["WtPerQty"]);
				DataRow newRow = dtdetail.NewRow();
				newRow["Id"] = 0;
				newRow["OrderId"] = src["Id"];
				newRow["OrderDetailId"] = orderDetailId;
				newRow["OrderDate"] = Conversion.ToDateTime(src["OrderDate"]).ToShortDateString();
				newRow["OrderNo"] = src["OrderNo"];
				newRow["ItemId"] = src["OrderItemId"];
				newRow["ItemName"] = src["ItemName"];
				newRow["CropYearId"] = src["CropYearId"];
				newRow["PackingDate"] = src["PackingDate"];
				newRow["ExpiryDate"] = src["ExpiryDate"];
				newRow["PackUOMId"] = src["ItemUomId"];
				newRow["PackUOM"] = src["PackUom"];
				newRow["ItemQty"] = qty;
				newRow["QtyValidate"] = qty;
				newRow["WtPerQty"] = wtPerQty;
				newRow["GrossWeight"] = qty * wtPerQty;
				newRow["SupplierQty"] = 0;
				newRow["CityName"] = "";
				newRow["RemarksDetail"] = src["RemarksHeader"];
				newRow["ItemConditionId"] = 0;
				Dictionary<int, List<RacksWithWarehouseAndItem>> dictionary = (from x in clsGlobalVariables.racksWithWarehouseAndItems
					group x by x.ItemId).ToDictionary((IGrouping<int, RacksWithWarehouseAndItem> g) => g.Key, (IGrouping<int, RacksWithWarehouseAndItem> g) => g.ToList());
				int itemId = Conversion.ToInt(src["OrderItemId"]);
				if (dictionary.TryGetValue(itemId, out var itemData))
				{
					(int, string, int, string) result = ResolveWarehouseAndRack(itemData, wareHouseConfigId);
					if (result.Item1 > 0)
					{
						newRow["WarehouseId"] = result.Item1;
						newRow["Warehouse"] = result.Item2;
					}
					if (result.Item3 > 0)
					{
						newRow["RackId"] = result.Item3;
						newRow["RackName"] = result.Item4;
					}
				}
				dtdetail.Rows.Add(newRow);
				existingOrderDetailIds.Add(orderDetailId);
				selectedSupplier = rowSupplierId;
			}
			dtdetail.EndLoadData();
			combsupplier.Value = selectedSupplier;
			((Control)(object)CmbDeliveryTerm).Text = Conversion.ToString(sourceTable.Rows[0]["DeliveryTerm"]);
			grd.DataSource = dtdetail;
			grd.RetrieveStructure();
			grdSettings(grd);
			WeightPerQtyCalculate();
			((Control)(object)combsupplier).Enabled = false;
			((Control)(object)CmbDeliveryTerm).Enabled = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private (int WarehouseId, string WarehouseName, int RackId, string RackName) ResolveWarehouseAndRack(List<RacksWithWarehouseAndItem> itemData, int configWarehouseId)
	{
		if (itemData == null || itemData.Count == 0)
		{
			return default((int, string, int, string));
		}
		RacksWithWarehouseAndItem baseRack = itemData.FirstOrDefault((RacksWithWarehouseAndItem x) => x.BaseRackId > 0);
		if (baseRack != null)
		{
			return (WarehouseId: baseRack.WarehouseId, WarehouseName: baseRack.WareHouseName, RackId: baseRack.Id, RackName: baseRack.RackName);
		}
		List<RacksWithWarehouseAndItem> warehouses = (from x in itemData
			group x by x.WarehouseId into g
			select g.First()).ToList();
		if (warehouses.Count == 1)
		{
			RacksWithWarehouseAndItem wh2 = warehouses[0];
			List<RacksWithWarehouseAndItem> racks = (from x in itemData
				where x.WarehouseId == wh2.WarehouseId
				group x by x.Id into g
				select g.First()).ToList();
			if (racks.Count == 1)
			{
				return (WarehouseId: wh2.WarehouseId, WarehouseName: wh2.WareHouseName, RackId: racks[0].Id, RackName: racks[0].RackName);
			}
			return (WarehouseId: wh2.WarehouseId, WarehouseName: wh2.WareHouseName, RackId: 0, RackName: null);
		}
		if (configWarehouseId > 0)
		{
			RacksWithWarehouseAndItem wh = warehouses.FirstOrDefault((RacksWithWarehouseAndItem x) => x.WarehouseId == configWarehouseId);
			if (wh != null)
			{
				List<RacksWithWarehouseAndItem> racks2 = (from x in itemData
					where x.WarehouseId == wh.WarehouseId
					group x by x.Id into g
					select g.First()).ToList();
				if (racks2.Count == 1)
				{
					return (WarehouseId: wh.WarehouseId, WarehouseName: wh.WareHouseName, RackId: racks2[0].Id, RackName: racks2[0].RackName);
				}
				return (WarehouseId: wh.WarehouseId, WarehouseName: wh.WareHouseName, RackId: 0, RackName: null);
			}
		}
		return default((int, string, int, string));
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

	private void txtEBUnit_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtEbTotal_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtWtCutUnit_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtWtCutTotal_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtsuppwt_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtfctwt_KeyPress(object sender, KeyPressEventArgs e)
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

	private void BtnLoadOrder_Click(object sender, EventArgs e)
	{
		LoadPO = new LoadPurchaseOrderPM(UserAccount);
		LoadPO.DocumentTypeId = 700;
		LoadPO.ShowDialog();
		LoadPO.BringToFront();
		LoadInGridDetail();
	}

	private void btnshowHistory_Click(object sender, EventArgs e)
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

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryComboFill(InvGrn.GetDataForDropDownFromGrn(UserAccount.OrganizationId, UserAccount.CompanyId, DocumentTypeId.ToString(), (string)null, Conversion.ToString((object)UserAccount.BranchesId), 0));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			ToDateHistory.Value = DateTime.Now;
			txtFromDocNoHistory.Text = string.Empty;
			txtToDocNoHistory.Text = string.Empty;
			((Control)(object)cmbSupplierNameHistory).Text = string.Empty;
			GrdHistory.ClearStructure();
			GrdHistoryDetail.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Control && e.KeyCode == Keys.Space)
			{
				GridEXRow r = GrdHistory.CurrentRow;
				if (r != null && (int)r.RowType == 0 && GrdHistory.CurrentColumn != null)
				{
					if (GrdHistory.CurrentColumn.Key == "Edit")
					{
						if (formright.DoHaveUpdateRights)
						{
							if (Conversion.ToInt(r.Cells["InvoiceNo"].Value) > 0)
							{
								MessageBox.Show("This Document is reffered in invoice. So you can't update this record.", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
								return;
							}
							reset();
							Id = Conversion.ToInt(r.Cells["Id"].Value);
							ReadById(Id);
						}
					}
					else if (GrdHistory.CurrentColumn.Key == "Print")
					{
						GenerateReport(Conversion.ToInt(r.Cells["Id"].Value));
					}
					else if (GrdHistory.CurrentColumn.Key == "PrintII")
					{
						CommonServices.GrnPmSlipWithSubReport214_01(Conversion.ToInt(r.Cells["Id"].Value));
					}
					else if (GrdHistory.CurrentColumn.Key == "NoOfAttachments" && r != null && (int)r.RowType == 0)
					{
						CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["Id"].Value), DocumentTypeId);
					}
				}
			}
			if (!e.Control || e.KeyCode != Keys.Return)
			{
				return;
			}
			GridEXRow row = GrdHistory.CurrentRow;
			if (row != null && (int)row.RowType == 0 && formright.DoHaveUpdateRights)
			{
				if (Conversion.ToInt(row.Cells["InvoiceNo"].Value) > 0)
				{
					MessageBox.Show("This Document is reffered in invoice. So you can't update this record.", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				reset();
				Id = Conversion.ToInt(row.Cells["Id"].Value);
				ReadById(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGp_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdGp.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grdGp.CurrentColumn != null && e.Control && e.KeyCode == Keys.Space && grdGp.CurrentColumn.Key == "Load")
			{
				LoadGpData();
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
			dt.Rows.Add("Ctrl+S", "For Save when in form tab and for show data when in history tab");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+Shift+Delete", "For Delete");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+H", "For History Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On DocDate");
			dt.Rows.Add("Ctrl+Enter", "For Update Record When Focus On Any Grid ");
			dt.Rows.Add("Ctrl+Space", "To Call Function's On Button Or Link When Focus On Any Grid ");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPrintII_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.GrnPmSlipWithSubReport214_01(Id);
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
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Expected O, but got Unknown
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Expected O, but got Unknown
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Expected O, but got Unknown
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Expected O, but got Unknown
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Expected O, but got Unknown
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Expected O, but got Unknown
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Expected O, but got Unknown
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Expected O, but got Unknown
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		//IL_036f: Expected O, but got Unknown
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Expected O, but got Unknown
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Expected O, but got Unknown
		//IL_0533: Unknown result type (might be due to invalid IL or missing references)
		//IL_053d: Expected O, but got Unknown
		//IL_05b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c1: Expected O, but got Unknown
		//IL_0b2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b36: Expected O, but got Unknown
		//IL_0e14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e1e: Expected O, but got Unknown
		//IL_0e2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e35: Expected O, but got Unknown
		//IL_445c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4466: Expected O, but got Unknown
		//IL_448a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4494: Expected O, but got Unknown
		GridEXLayout grdGp_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.PackingMaterial_Store.GrnPackingMaterial));
		GridEXLayout GrdHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdGp = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label8 = new System.Windows.Forms.Label();
		this.grd = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label53 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.txtBillCalculateType = new System.Windows.Forms.TextBox();
		this.combvehtyp = new UltraCombo();
		this.txtbltyno = new System.Windows.Forms.TextBox();
		this.txtvehno = new System.Windows.Forms.TextBox();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.txtGpId = new System.Windows.Forms.TextBox();
		this.combsupplier = new UltraCombo();
		this.label24 = new System.Windows.Forms.Label();
		this.txtAvgWtPerQty = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.CmbDeliveryTerm = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.txtGpNo = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.CmbTransport = new UltraCombo();
		this.label48 = new System.Windows.Forms.Label();
		this.txtwtdiff = new System.Windows.Forms.TextBox();
		this.txtcarramount = new System.Windows.Forms.TextBox();
		this.txtfctwt = new System.Windows.Forms.TextBox();
		this.txtsuppwt = new System.Windows.Forms.TextBox();
		this.label20 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label16 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.printToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.BtnPrintII = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.BtnLoadOrder = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.CityDefine = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.ChkPreviewII = new System.Windows.Forms.CheckBox();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.GrdHistory = new GridEX();
		this.panel23 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label27 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label28 = new System.Windows.Forms.Label();
		this.GrdHistoryDetail = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.RadNotReffered = new System.Windows.Forms.RadioButton();
		this.RadReffered = new System.Windows.Forms.RadioButton();
		this.btnshowHistory = new System.Windows.Forms.Button();
		this.label49 = new System.Windows.Forms.Label();
		this.RadAll = new System.Windows.Forms.RadioButton();
		this.label50 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.cmbSupplierNameHistory = new UltraCombo();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.panel7 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnGrnFormHistory = new System.Windows.Forms.ToolStripButton();
		this.label17 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGp).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel6.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.combvehtyp).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combsupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransport).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistory).BeginInit();
		this.panel23.SuspendLayout();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryDetail).BeginInit();
		this.panel8.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierNameHistory).BeginInit();
		this.panel7.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(966, 657);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(966, 657);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.BackColor = System.Drawing.Color.White;
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(958, 631);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.panel3.AutoScroll = true;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdGp);
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel3.Controls.Add(this.panel6);
		this.panel3.Controls.Add(this.panel4);
		this.panel3.Controls.Add(this.panel2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(2, 2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(954, 627);
		this.panel3.TabIndex = 6;
		this.grdGp.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdGp).BackColor = System.Drawing.Color.White;
		grdGp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdGp.DesignTimeLayout = grdGp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGp).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdGp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdGp.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdGp).Location = new System.Drawing.Point(0, 471);
		((System.Windows.Forms.Control)(object)this.grdGp).Name = "grdGp";
		((ScrollableBase)this.grdGp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGp).Size = new System.Drawing.Size(954, 156);
		((System.Windows.Forms.Control)(object)this.grdGp).TabIndex = 1;
		this.grdGp.TotalRow = (InheritableBoolean)1;
		this.grdGp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGp).VisualStyle = (VisualStyle)4;
		this.grdGp.ColumnButtonClick += new ColumnActionEventHandler(grdGp_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdGp).KeyDown += new System.Windows.Forms.KeyEventHandler(grdGp_KeyDown);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.ctrlGrdBar3);
		this.panel5.Controls.Add(this.label8);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 443);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(954, 28);
		this.panel5.TabIndex = 10;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(913, 3);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdGp;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 2;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(6, 5);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(220, 18);
		this.label8.TabIndex = 0;
		this.label8.Text = "Gate Pass (Pending For Grn)";
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Top;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)1;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxFormatStyle.BackColorGradient = System.Drawing.Color.White;
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 214);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(954, 229);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grd).TabStop = false;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label15);
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 186);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(954, 28);
		this.panel6.TabIndex = 8;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(6, 5);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(53, 18);
		this.label15.TabIndex = 0;
		this.label15.Text = "Detail";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(913, 3);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 1;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 54);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(954, 132);
		this.panel4.TabIndex = 0;
		this.groupBox1.Controls.Add(this.label53);
		this.groupBox1.Controls.Add(this.label34);
		this.groupBox1.Controls.Add(this.txtBillCalculateType);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combvehtyp);
		this.groupBox1.Controls.Add(this.txtbltyno);
		this.groupBox1.Controls.Add(this.txtvehno);
		this.groupBox1.Controls.Add(this.txtremarks);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add(this.txtGpId);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combsupplier);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.txtAvgWtPerQty);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add(this.txtdocno);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.DocDate);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label23);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label25);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add(this.txtGpNo);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTransport);
		this.groupBox1.Controls.Add(this.label48);
		this.groupBox1.Controls.Add(this.txtwtdiff);
		this.groupBox1.Controls.Add(this.txtcarramount);
		this.groupBox1.Controls.Add(this.txtfctwt);
		this.groupBox1.Controls.Add(this.txtsuppwt);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(5, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(946, 127);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main Information";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.ForeColor = System.Drawing.Color.Red;
		this.label53.Location = new System.Drawing.Point(377, 49);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(14, 17);
		this.label53.TabIndex = 5;
		this.label53.Text = "*";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.ForeColor = System.Drawing.Color.Red;
		this.label34.Location = new System.Drawing.Point(85, 49);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(14, 17);
		this.label34.TabIndex = 4;
		this.label34.Text = "*";
		this.txtBillCalculateType.BackColor = System.Drawing.SystemColors.Menu;
		this.txtBillCalculateType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBillCalculateType.Enabled = false;
		this.txtBillCalculateType.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBillCalculateType.Location = new System.Drawing.Point(620, 100);
		this.txtBillCalculateType.Name = "txtBillCalculateType";
		this.txtBillCalculateType.Size = new System.Drawing.Size(118, 23);
		this.txtBillCalculateType.TabIndex = 11;
		this.combvehtyp.AllowNull = (DefaultableBoolean)1;
		this.combvehtyp.AutoCompleteMode = (AutoCompleteMode)4;
		this.combvehtyp.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combvehtyp.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.combvehtyp).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combvehtyp).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combvehtyp).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combvehtyp).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.combvehtyp).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combvehtyp).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.combvehtyp).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combvehtyp).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.combvehtyp).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combvehtyp).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combvehtyp).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combvehtyp).Enabled = false;
		((System.Windows.Forms.Control)(object)this.combvehtyp).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combvehtyp.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combvehtyp).Location = new System.Drawing.Point(620, 17);
		((System.Windows.Forms.Control)(object)this.combvehtyp).Name = "combvehtyp";
		((System.Windows.Forms.Control)(object)this.combvehtyp).Size = new System.Drawing.Size(118, 26);
		((System.Windows.Forms.Control)(object)this.combvehtyp).TabIndex = 8;
		this.txtbltyno.BackColor = System.Drawing.SystemColors.Menu;
		this.txtbltyno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbltyno.Enabled = false;
		this.txtbltyno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbltyno.Location = new System.Drawing.Point(620, 74);
		this.txtbltyno.Name = "txtbltyno";
		this.txtbltyno.Size = new System.Drawing.Size(118, 23);
		this.txtbltyno.TabIndex = 10;
		this.txtvehno.BackColor = System.Drawing.SystemColors.Menu;
		this.txtvehno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvehno.Enabled = false;
		this.txtvehno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtvehno.Location = new System.Drawing.Point(620, 46);
		this.txtvehno.Name = "txtvehno";
		this.txtvehno.Size = new System.Drawing.Size(118, 23);
		this.txtvehno.TabIndex = 9;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(103, 100);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(412, 23);
		this.txtremarks.TabIndex = 595;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(518, 104);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(102, 15);
		this.label22.TabIndex = 594;
		this.label22.Text = "Bill Calculate Type";
		this.txtGpId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGpId.Enabled = false;
		this.txtGpId.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtGpId.Location = new System.Drawing.Point(351, 74);
		this.txtGpId.Name = "txtGpId";
		this.txtGpId.Size = new System.Drawing.Size(25, 23);
		this.txtGpId.TabIndex = 588;
		this.txtGpId.Visible = false;
		this.combsupplier.AllowNull = (DefaultableBoolean)1;
		this.combsupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.combsupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combsupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combsupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.combsupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combsupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combsupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combsupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.combsupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combsupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.combsupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combsupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.combsupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combsupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combsupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combsupplier).Enabled = false;
		((System.Windows.Forms.Control)(object)this.combsupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combsupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combsupplier).Location = new System.Drawing.Point(103, 44);
		((System.Windows.Forms.Control)(object)this.combsupplier).Name = "combsupplier";
		((System.Windows.Forms.Control)(object)this.combsupplier).Size = new System.Drawing.Size(195, 26);
		((System.Windows.Forms.Control)(object)this.combsupplier).TabIndex = 2;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(5, 104);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(52, 15);
		this.label24.TabIndex = 0;
		this.label24.Text = "Remarks";
		this.txtAvgWtPerQty.BackColor = System.Drawing.SystemColors.Menu;
		this.txtAvgWtPerQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAvgWtPerQty.Enabled = false;
		this.txtAvgWtPerQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAvgWtPerQty.Location = new System.Drawing.Point(823, 100);
		this.txtAvgWtPerQty.Name = "txtAvgWtPerQty";
		this.txtAvgWtPerQty.Size = new System.Drawing.Size(115, 23);
		this.txtAvgWtPerQty.TabIndex = 15;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(741, 104);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(71, 15);
		this.label13.TabIndex = 593;
		this.label13.Text = "Avg Wt/Qty";
		this.txtdocno.BackColor = System.Drawing.SystemColors.Menu;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Enabled = false;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(103, 19);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(90, 23);
		this.txtdocno.TabIndex = 0;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(3, 23);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(76, 15);
		this.label5.TabIndex = 3;
		this.label5.Text = "Doc No,Date";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.Red;
		this.label10.Location = new System.Drawing.Point(809, 77);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(14, 17);
		this.label10.TabIndex = 590;
		this.label10.Text = "*";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI", 7.75f, System.Drawing.FontStyle.Bold);
		this.label12.ForeColor = System.Drawing.Color.Red;
		this.label12.Location = new System.Drawing.Point(811, 24);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(12, 13);
		this.label12.TabIndex = 591;
		this.label12.Text = "*";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.Red;
		this.label9.Location = new System.Drawing.Point(609, 49);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(14, 17);
		this.label9.TabIndex = 589;
		this.label9.Text = "*";
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(194, 19);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(104, 23);
		this.DocDate.TabIndex = 1;
		this.DocDate.TabStop = false;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(3, 50);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(86, 15);
		this.label2.TabIndex = 0;
		this.label2.Text = "Supplier Name";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.Red;
		this.label23.Location = new System.Drawing.Point(609, 77);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(14, 17);
		this.label23.TabIndex = 10;
		this.label23.Text = "*";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(302, 50);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(80, 15);
		this.label4.TabIndex = 583;
		this.label4.Text = "Delivery Term";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.Red;
		this.label14.Location = new System.Drawing.Point(609, 22);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(14, 17);
		this.label14.TabIndex = 8;
		this.label14.Text = "*";
		this.CmbDeliveryTerm.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance48;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance53).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance56).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Location = new System.Drawing.Point(392, 44);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Name = "CmbDeliveryTerm";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Size = new System.Drawing.Size(123, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).TabIndex = 6;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.Red;
		this.label3.Location = new System.Drawing.Point(377, 77);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(14, 17);
		this.label3.TabIndex = 587;
		this.label3.Text = "*";
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(518, 70);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(92, 31);
		this.label25.TabIndex = 3;
		this.label25.Text = "Bilty No / Supplier DC No";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(302, 78);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(41, 15);
		this.label7.TabIndex = 585;
		this.label7.Text = "Gp No";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(518, 50);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(64, 15);
		this.label19.TabIndex = 0;
		this.label19.Text = "Vehicle No";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(518, 23);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(73, 15);
		this.label21.TabIndex = 8;
		this.label21.Text = "Vehicle Type";
		this.txtGpNo.BackColor = System.Drawing.SystemColors.Menu;
		this.txtGpNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGpNo.Enabled = false;
		this.txtGpNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGpNo.Location = new System.Drawing.Point(392, 74);
		this.txtGpNo.Name = "txtGpNo";
		this.txtGpNo.Size = new System.Drawing.Size(123, 23);
		this.txtGpNo.TabIndex = 7;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Red;
		this.label1.Location = new System.Drawing.Point(85, 22);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(14, 17);
		this.label1.TabIndex = 584;
		this.label1.Text = "*";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.Red;
		this.label32.Location = new System.Drawing.Point(809, 49);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(14, 17);
		this.label32.TabIndex = 12;
		this.label32.Text = "*";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(741, 78);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(69, 15);
		this.label11.TabIndex = 0;
		this.label11.Text = "Weight Diff";
		this.CmbTransport.AllowNull = (DefaultableBoolean)1;
		this.CmbTransport.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTransport.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTransport.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbTransport).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTransport).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransport).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransport).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransport).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransport).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbTransport).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTransport).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbTransport).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTransport).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTransport).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTransport).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTransport.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTransport).Location = new System.Drawing.Point(103, 72);
		((System.Windows.Forms.Control)(object)this.CmbTransport).Name = "CmbTransport";
		((System.Windows.Forms.Control)(object)this.CmbTransport).Size = new System.Drawing.Size(195, 26);
		((System.Windows.Forms.Control)(object)this.CmbTransport).TabIndex = 3;
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(3, 78);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(102, 15);
		this.label48.TabIndex = 8;
		this.label48.Text = "Transporter Name";
		this.txtwtdiff.BackColor = System.Drawing.SystemColors.Menu;
		this.txtwtdiff.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtwtdiff.Enabled = false;
		this.txtwtdiff.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtwtdiff.Location = new System.Drawing.Point(823, 74);
		this.txtwtdiff.Name = "txtwtdiff";
		this.txtwtdiff.ReadOnly = true;
		this.txtwtdiff.Size = new System.Drawing.Size(115, 23);
		this.txtwtdiff.TabIndex = 14;
		this.txtwtdiff.TabStop = false;
		this.txtcarramount.BackColor = System.Drawing.Color.White;
		this.txtcarramount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcarramount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcarramount.Location = new System.Drawing.Point(392, 19);
		this.txtcarramount.Name = "txtcarramount";
		this.txtcarramount.Size = new System.Drawing.Size(123, 23);
		this.txtcarramount.TabIndex = 5;
		this.txtfctwt.BackColor = System.Drawing.SystemColors.Menu;
		this.txtfctwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfctwt.Enabled = false;
		this.txtfctwt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfctwt.Location = new System.Drawing.Point(823, 46);
		this.txtfctwt.Name = "txtfctwt";
		this.txtfctwt.Size = new System.Drawing.Size(115, 23);
		this.txtfctwt.TabIndex = 13;
		this.txtsuppwt.BackColor = System.Drawing.SystemColors.Menu;
		this.txtsuppwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsuppwt.Enabled = false;
		this.txtsuppwt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtsuppwt.Location = new System.Drawing.Point(823, 19);
		this.txtsuppwt.Name = "txtsuppwt";
		this.txtsuppwt.Size = new System.Drawing.Size(115, 23);
		this.txtsuppwt.TabIndex = 12;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(739, 50);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(71, 15);
		this.label20.TabIndex = 3;
		this.label20.Text = "Fact Weight";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(739, 23);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(77, 15);
		this.label6.TabIndex = 0;
		this.label6.Text = "Supp Weight";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(302, 23);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(70, 15);
		this.label26.TabIndex = 8;
		this.label26.Text = "Freight Amt";
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label16);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Controls.Add(this.ChkPreviewII);
		this.panel2.Controls.Add(this.ChkBox);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(954, 54);
		this.panel2.TabIndex = 0;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label16.ForeColor = System.Drawing.Color.White;
		this.label16.Location = new System.Drawing.Point(1, 30);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(227, 19);
		this.label16.TabIndex = 1;
		this.label16.Text = "Goods Receiving Notes-PM";
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[11]
		{
			this.btnNew, this.toolStripButton1, this.btnUpdate, this.btnSave, this.printToolStripButton, this.BtnPrintII, this.btnAttachment, this.BtnLoadOrder, this.btnDelete, this.CityDefine,
			this.BtnShortCutkeys
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(954, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.ToolTipText = "Ctrl_N";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(80, 24);
		this.toolStripButton1.Text = "&Refresh";
		this.toolStripButton1.ToolTipText = "Alt_R";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.ToolTipText = "Ctrl_U";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.ToolTipText = "Ctrl_S";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.printToolStripButton.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.printToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.printToolStripButton.Name = "printToolStripButton";
		this.printToolStripButton.Size = new System.Drawing.Size(92, 24);
		this.printToolStripButton.Text = "214-Print";
		this.printToolStripButton.ToolTipText = "Ctrl_P";
		this.printToolStripButton.Click += new System.EventHandler(printToolStripButton_Click);
		this.BtnPrintII.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrintII.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrintII.Name = "BtnPrintII";
		this.BtnPrintII.Size = new System.Drawing.Size(116, 24);
		this.BtnPrintII.Text = "214_01-Print";
		this.BtnPrintII.ToolTipText = "Ctrl_P";
		this.BtnPrintII.Click += new System.EventHandler(BtnPrintII_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.ToolTipText = "Alt_A";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.BtnLoadOrder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.BtnLoadOrder.Image = (System.Drawing.Image)resources.GetObject("BtnLoadOrder.Image");
		this.BtnLoadOrder.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoadOrder.Name = "BtnLoadOrder";
		this.BtnLoadOrder.Size = new System.Drawing.Size(83, 24);
		this.BtnLoadOrder.Text = "Load Order";
		this.BtnLoadOrder.Visible = false;
		this.BtnLoadOrder.Click += new System.EventHandler(BtnLoadOrder_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.CityDefine.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.CityDefine.Name = "CityDefine";
		this.CityDefine.Size = new System.Drawing.Size(83, 24);
		this.CityDefine.Text = "Define City";
		this.CityDefine.Click += new System.EventHandler(CityDefine_Click);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.ChkPreviewII.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPreviewII.AutoSize = true;
		this.ChkPreviewII.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPreviewII.ForeColor = System.Drawing.Color.White;
		this.ChkPreviewII.Location = new System.Drawing.Point(850, 31);
		this.ChkPreviewII.Margin = new System.Windows.Forms.Padding(2);
		this.ChkPreviewII.Name = "ChkPreviewII";
		this.ChkPreviewII.Size = new System.Drawing.Size(100, 20);
		this.ChkPreviewII.TabIndex = 16;
		this.ChkPreviewII.Text = "Preview II";
		this.ChkPreviewII.UseVisualStyleBackColor = true;
		this.ChkBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBox.AutoSize = true;
		this.ChkBox.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBox.ForeColor = System.Drawing.Color.White;
		this.ChkBox.Location = new System.Drawing.Point(764, 31);
		this.ChkBox.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(84, 20);
		this.ChkBox.TabIndex = 16;
		this.ChkBox.Text = "Preview";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistory);
		this.tabPage2.Controls.Add(this.panel23);
		this.tabPage2.Controls.Add(this.panel11);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistoryDetail);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(958, 631);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.GrdHistory.AllowEdit = (InheritableBoolean)2;
		GrdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistory.DesignTimeLayout = GrdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdHistory.DynamicFiltering = true;
		this.GrdHistory.FilterMode = (FilterMode)2;
		this.GrdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistory.FrozenColumns = 3;
		this.GrdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Location = new System.Drawing.Point(2, 158);
		((System.Windows.Forms.Control)(object)this.GrdHistory).Name = "GrdHistory";
		((ScrollableBase)this.GrdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Size = new System.Drawing.Size(954, 235);
		((System.Windows.Forms.Control)(object)this.GrdHistory).TabIndex = 0;
		this.GrdHistory.TotalRow = (InheritableBoolean)1;
		this.GrdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistory).VisualStyle = (VisualStyle)4;
		this.GrdHistory.ColumnButtonClick += new ColumnActionEventHandler(GrdHistory_ColumnButtonClick);
		this.GrdHistory.SelectionChanged += new System.EventHandler(GrdHistory_SelectionChanged);
		this.GrdHistory.LinkClicked += new ColumnActionEventHandler(GrdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdHistory).DoubleClick += new System.EventHandler(GrdHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.GrdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(GrdHistory_KeyDown);
		this.panel23.BackColor = System.Drawing.Color.Teal;
		this.panel23.Controls.Add(this.ctrlGrdBar2);
		this.panel23.Controls.Add(this.label27);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel23.Location = new System.Drawing.Point(2, 132);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(954, 26);
		this.panel23.TabIndex = 51;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(914, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.GrdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 23;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label27.ForeColor = System.Drawing.Color.White;
		this.label27.Location = new System.Drawing.Point(5, 3);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(132, 18);
		this.label27.TabIndex = 24;
		this.label27.Text = "Filtered Records";
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label28);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel11.Location = new System.Drawing.Point(2, 393);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(954, 19);
		this.panel11.TabIndex = 48;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label28.ForeColor = System.Drawing.Color.White;
		this.label28.Location = new System.Drawing.Point(1, 1);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(230, 17);
		this.label28.TabIndex = 25;
		this.label28.Text = "Detail of the Selected above row";
		this.GrdHistoryDetail.AllowEdit = (InheritableBoolean)2;
		GrdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistoryDetail.DesignTimeLayout = GrdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.GrdHistoryDetail.DynamicFiltering = true;
		this.GrdHistoryDetail.FilterMode = (FilterMode)2;
		this.GrdHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Location = new System.Drawing.Point(2, 412);
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Name = "GrdHistoryDetail";
		((ScrollableBase)this.GrdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Size = new System.Drawing.Size(954, 217);
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).TabIndex = 1;
		this.GrdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.GrdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel8.BackColor = System.Drawing.Color.White;
		this.panel8.Controls.Add(this.groupBox3);
		this.panel8.Controls.Add(this.rdapproveddate);
		this.panel8.Controls.Add(this.rdentrydate);
		this.panel8.Controls.Add(this.rdmodifydate);
		this.panel8.Controls.Add(this.drdocdate);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(2, 54);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(954, 78);
		this.panel8.TabIndex = 1;
		this.groupBox3.Controls.Add(this.RadNotReffered);
		this.groupBox3.Controls.Add(this.RadReffered);
		this.groupBox3.Controls.Add(this.btnshowHistory);
		this.groupBox3.Controls.Add(this.label49);
		this.groupBox3.Controls.Add(this.RadAll);
		this.groupBox3.Controls.Add(this.label50);
		this.groupBox3.Controls.Add(this.label51);
		this.groupBox3.Controls.Add(this.label52);
		this.groupBox3.Controls.Add(this.label18);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory);
		this.groupBox3.Controls.Add(this.txtToDocNoHistory);
		this.groupBox3.Controls.Add(this.txtFromDocNoHistory);
		this.groupBox3.Controls.Add(this.ToDateHistory);
		this.groupBox3.Controls.Add(this.FromDateHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(3, 2);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(830, 72);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.RadNotReffered.AutoSize = true;
		this.RadNotReffered.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.RadNotReffered.Location = new System.Drawing.Point(720, 45);
		this.RadNotReffered.Name = "RadNotReffered";
		this.RadNotReffered.Size = new System.Drawing.Size(108, 23);
		this.RadNotReffered.TabIndex = 8;
		this.RadNotReffered.Text = "Not Reffered";
		this.RadNotReffered.UseVisualStyleBackColor = true;
		this.RadReffered.AutoSize = true;
		this.RadReffered.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.RadReffered.Location = new System.Drawing.Point(720, 27);
		this.RadReffered.Name = "RadReffered";
		this.RadReffered.Size = new System.Drawing.Size(80, 23);
		this.RadReffered.TabIndex = 7;
		this.RadReffered.Text = "Reffered";
		this.RadReffered.UseVisualStyleBackColor = true;
		this.btnshowHistory.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshowHistory.FlatAppearance.BorderSize = 0;
		this.btnshowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshowHistory.ForeColor = System.Drawing.Color.White;
		this.btnshowHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshowHistory.Location = new System.Drawing.Point(656, 38);
		this.btnshowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnshowHistory.Name = "btnshowHistory";
		this.btnshowHistory.Size = new System.Drawing.Size(57, 26);
		this.btnshowHistory.TabIndex = 5;
		this.btnshowHistory.Text = "Show";
		this.btnshowHistory.UseVisualStyleBackColor = false;
		this.btnshowHistory.Click += new System.EventHandler(btnshowHistory_Click);
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label49.Location = new System.Drawing.Point(446, 20);
		this.label49.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(86, 15);
		this.label49.TabIndex = 142;
		this.label49.Text = "Supplier Name";
		this.RadAll.AutoSize = true;
		this.RadAll.Checked = true;
		this.RadAll.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.RadAll.Location = new System.Drawing.Point(720, 9);
		this.RadAll.Name = "RadAll";
		this.RadAll.Size = new System.Drawing.Size(44, 23);
		this.RadAll.TabIndex = 6;
		this.RadAll.TabStop = true;
		this.RadAll.Text = "All";
		this.RadAll.UseVisualStyleBackColor = true;
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label50.Location = new System.Drawing.Point(362, 20);
		this.label50.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(64, 15);
		this.label50.TabIndex = 141;
		this.label50.Text = "To Doc No";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label51.Location = new System.Drawing.Point(278, 20);
		this.label51.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(79, 15);
		this.label51.TabIndex = 140;
		this.label51.Text = "From Doc No";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label52.Location = new System.Drawing.Point(145, 20);
		this.label52.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(48, 15);
		this.label52.TabIndex = 139;
		this.label52.Text = "To Date";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label18.Location = new System.Drawing.Point(6, 20);
		this.label18.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(63, 15);
		this.label18.TabIndex = 13;
		this.label18.Text = "From Date";
		this.cmbSupplierNameHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplierNameHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplierNameHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplierNameHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance59).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSupplierNameHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Location = new System.Drawing.Point(446, 38);
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Name = "cmbSupplierNameHistory";
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Size = new System.Drawing.Size(208, 26);
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).TabIndex = 4;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(362, 40);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.ShortcutsEnabled = false;
		this.txtToDocNoHistory.Size = new System.Drawing.Size(82, 23);
		this.txtToDocNoHistory.TabIndex = 3;
		this.txtFromDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoHistory.Location = new System.Drawing.Point(278, 40);
		this.txtFromDocNoHistory.Name = "txtFromDocNoHistory";
		this.txtFromDocNoHistory.ShortcutsEnabled = false;
		this.txtFromDocNoHistory.Size = new System.Drawing.Size(82, 23);
		this.txtFromDocNoHistory.TabIndex = 2;
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(145, 40);
		this.ToDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(130, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(6, 40);
		this.FromDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(137, 23);
		this.FromDateHistory.TabIndex = 0;
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(842, 55);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 4;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(842, 20);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 2;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(842, 38);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 3;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(842, 3);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 1;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.toolStrip2);
		this.panel7.Controls.Add(this.label17);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(2, 2);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(954, 52);
		this.panel7.TabIndex = 0;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.btnRefreshHistory, this.btnGrnFormHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(954, 27);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(68, 24);
		this.btnNewHistory.Text = "&Reset";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "&Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.btnGrnFormHistory.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnGrnFormHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnGrnFormHistory.Name = "btnGrnFormHistory";
		this.btnGrnFormHistory.Size = new System.Drawing.Size(99, 24);
		this.btnGrnFormHistory.Text = "GrnHistory";
		this.btnGrnFormHistory.ToolTipText = "Crtl_H";
		this.btnGrnFormHistory.Click += new System.EventHandler(btnGrnFormHistory_Click);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(2, 30);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(290, 19);
		this.label17.TabIndex = 24;
		this.label17.Text = "Goods Receiving Notes-PM History";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(966, 657);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "GrnPackingMaterial";
		this.Text = "   ";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(GrnPackingMaterial_FormClosing);
		base.Load += new System.EventHandler(InvFrmGRN_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvFrmGRN_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdGp).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.combvehtyp).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combsupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransport).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistory).EndInit();
		this.panel23.ResumeLayout(false);
		this.panel23.PerformLayout();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryDetail).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierNameHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
