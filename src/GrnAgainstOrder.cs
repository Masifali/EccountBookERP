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
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Inventory_Reports;
using Architecture.WinApp.Properties;
using Architecture.WinApp.PurchaseTrading;
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
namespace Architecture.WinApp.Pcc.Purchase
{

public class GrnAgainstOrder : Form
{
	private Rightsobjects formright = new Rightsobjects();

	private int updateDetailIndex;

	private Attachment AT = new Attachment();

	private int Id;

	private bool SubsidiaryAccountAllownOnVouchers;

	private DataTable dtdetail = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private LoadPurchaseOrderWithQty LoadPO;

	private int OrderId;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

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

	private Panel panel5;

	internal UltraCombo CmbPackUom;

	internal UltraCombo CmbCity;

	internal UltraCombo CmbItem;

	private TextBox txtqty;

	private TextBox txtadlswt;

	private TextBox txtnetwt;

	private Label label46;

	private Label label28;

	private Label label35;

	private Label label43;

	private Label label42;

	private Label label41;

	private Label label36;

	private Panel panel7;

	private Panel panel8;

	internal GridEX GrdHistory;

	private ToolStripButton toolStripButton1;

	private Panel panel10;

	private Panel panel11;

	internal GridEX GrdHistoryDetail;

	private CtrlGrdBar ctrlGrdBar2;

	private ToolStrip toolStrip2;

	private ToolStripButton btnLoadAll;

	private ToolStripButton btnDelete;

	private ToolStripButton btnGrnFormHistory;

	internal UltraCombo CmbJobLot;

	private Label label1;

	internal UltraCombo CmbWareHouse;

	private Label label3;

	private ToolStripButton BtnLoadOrder;

	private Label label22;

	private TextBox txtremarksdetail;

	private Panel panel6;

	internal GridEX grd;

	private TextBox txtgwt;

	private Panel panel4;

	private Panel panel13;

	private Label label27;

	private Label label34;

	private Label label15;

	private Label label14;

	private Label label53;

	internal UltraCombo CmbDeliveryTerm;

	private Label label4;

	internal UltraCombo combvehtyp;

	private CheckBox ChkBox;

	private Label label25;

	private Label label24;

	private Label label19;

	private Label label21;

	private TextBox txtremarks;

	private TextBox txtbltyno;

	private TextBox txtvehno;

	private Label label2;

	internal UltraCombo CmbSupplier;

	private DateTimePicker DocDate;

	private Label label5;

	private TextBox txtdocno;

	private Button btnUpdateDetail;

	private Button btnCancelUpdateDetial;

	private Button btnplus;

	private Panel panel9;

	private Panel panel3;

	private Label label6;

	private ToolStripButton btnshortcutkeys;

	private Panel panel12;

	private Panel panel14;

	private Panel panel15;

	private Button btnshow;

	private DateTimePicker ToDate;

	private Label label9;

	private DateTimePicker fromdate;

	private Label label10;

	internal UltraCombo combsupplier;

	private Label label12;

	private Label label16;

	private Panel panel16;

	private Label label20;

	private CtrlGrdBar ctrlGrdBar3;

	public GrnAgainstOrder(UserAccount user)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void Add_Click(object sender, EventArgs e)
	{
		try
		{
			GridEXRow r = grd.GetRow(0);
			if (grd.RowCount > 0 && Conversion.ToInt(r.Cells["OrderId"].Value) > 0)
			{
				throw new Exception("You can not add manual Record beacause record against order exist in Grid");
			}
			if (FormValidationDetail())
			{
				double num = Conversion.ToDouble((object)txtnetwt.Text.Trim());
				double GrossWeight = Conversion.ToDouble((object)txtgwt.Text.Trim());
				if (num > GrossWeight)
				{
					throw new Exception("NetBillWeight cannot greater than Gross Weight Please check");
				}
				dtdetail.Rows.Add(0, 0, 0, 0, Conversion.ToInt(CmbWareHouse.Value), Conversion.ToString((object)((Control)(object)CmbWareHouse).Text), Conversion.ToInt(CmbJobLot.Value), Conversion.ToString((object)((Control)(object)CmbJobLot).Text), Conversion.ToInt(CmbItem.Value), Conversion.ToString((object)((Control)(object)CmbItem).Text), Conversion.ToInt(CmbPackUom.Value), Conversion.ToString((object)((Control)(object)CmbPackUom).Text), Conversion.ToDouble((object)txtqty.Text.Trim()), Conversion.ToDouble((object)txtgwt.Text.Trim()), Conversion.ToDouble((object)txtadlswt.Text.Trim()), Conversion.ToDouble((object)txtnetwt.Text.Trim()), Conversion.ToInt(CmbCity.Value), Conversion.ToString((object)((Control)(object)CmbCity).Text.Trim()), Conversion.ToString((object)txtremarksdetail.Text.Trim()));
				grd.DataSource = dtdetail;
				grd.RetrieveStructure();
				grdSettings();
				ResetDetial();
				Total();
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
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Expected O, but got Unknown
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
			GRN.DocumentTypeId = 1858;
			GRN.DocDate = DocDate.Value;
			GRN.DocNo = Convert.ToInt32(txtdocno.Text.Trim());
			GRN.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			GRN.PartyWeight = 0.0;
			GRN.FactoryWeight = 0.0;
			GRN.VehicleType = Conversion.ToString((object)((Control)(object)combvehtyp).Text);
			GRN.VehicleNo = Conversion.ToString((object)txtvehno.Text.Trim());
			GRN.DeliveryTerm = Conversion.ToString((object)((Control)(object)CmbDeliveryTerm).Text.Trim());
			GRN.BiltyNo = txtbltyno.Text.Trim();
			GRN.TransporterId = 0;
			GRN.CarriageAmount = 0.0;
			GRN.RemarksHeader = txtremarks.Text.Trim();
			GRN.BranchesId = UserAccount.BranchesId;
			GRN.OrganizationId = UserAccount.OrganizationId;
			GRN.CompanyId = UserAccount.CompanyId;
			GRN.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			GRN.EntryDate = DateTime.Now;
			GRN.ModifyDate = DateTime.Now;
			GRN.EntryUser = UserAccount.ID;
			GRN.ModifyUser = UserAccount.ID;
			GRN.invGrnDetail = new List<InvGrnDetail>();
			GRN.InvgrnDetailEmptyBagslist = new List<InvgrnDetailEmptyBags>();
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				InvGrnDetail grnDetail = new InvGrnDetail();
				grnDetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
				grnDetail.PurchaseOrderDetailId = Conversion.ToInt(r.Cells["OrderDetailId"].Value);
				grnDetail.PurchaseOrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
				grnDetail.PurchaseOrder = Conversion.ToInt(r.Cells["OrderNo"].Value);
				grnDetail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				grnDetail.ItemUomId = Conversion.ToInt(r.Cells["UOMId"].Value);
				grnDetail.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
				grnDetail.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
				grnDetail.ItemQty = Conversion.ToDouble(r.Cells["Qty"].Value);
				grnDetail.GrossWeight = Conversion.ToDouble(r.Cells["GrossWight"].Value);
				grnDetail.AdLsWeight = Conversion.ToDouble(r.Cells["AddLesswt"].Value);
				grnDetail.NetBillWeight = Conversion.ToDouble(r.Cells["NetWeight"].Value);
				grnDetail.StockWeight = grnDetail.NetBillWeight;
				grnDetail.CityId = Conversion.ToInt(r.Cells["CityId"].Value);
				grnDetail.AreaCity = Conversion.ToString((object)r.Cells["City"].Text);
				grnDetail.CommentsDetail = Conversion.ToString((object)r.Cells["RemarksDetail"].Text);
				GRN.invGrnDetail.Add(grnDetail);
			}
			GRN.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			GRN.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<InvGrn>(GRN, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			GRN.AttachmentsValues = AttachmentsValues;
			GRN.CustomAttachmentsValues = CustomAttachmentsValues;
			int success = InvGrn.Save(GRN);
			if (Id > 0)
			{
				MessageBox.Show("Record Update Successfully [" + GRN.DocNo + "]");
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
				{
					global::Architecture.BLL.DMSAttachments.RemoveByIdAndNames(Id, base.Name, 1858, 0);
				}
			}
			else
			{
				MessageBox.Show("Record Save Successfully [" + GRN.DocNo + "]");
			}
			reset();
			grd.DataSource = null;
			if (ChkBox.Checked)
			{
				GenerateReport(success);
			}
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

	public void ReadById_Update(int ID)
	{
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			((Control)(object)CmbDeliveryTerm).Enabled = false;
			Id = Conversion.ToInt((object)ID);
			InvGrn invGrn = InvGrn.GetByID(Id);
			txtdocno.Text = invGrn.DocNo.ToString();
			DocDate.Value = invGrn.DocDate;
			tabControl1.SelectedIndex = 0;
			CmbSupplier.Value = invGrn.SupplierCustomerId;
			((Control)(object)CmbSupplier).Enabled = false;
			((Control)(object)combvehtyp).Text = invGrn.VehicleType;
			txtvehno.Text = invGrn.VehicleNo;
			txtbltyno.Text = invGrn.BiltyNo;
			txtremarks.Text = Conversion.ToString((object)invGrn.RemarksHeader);
			((Control)(object)CmbDeliveryTerm).Text = Conversion.ToString((object)invGrn.DeliveryTerm);
			AttachmentsValues = invGrn.AttachmentsValues;
			CustomAttachmentsValues = invGrn.CustomAttachmentsValues;
			new GatePassInward
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			dtdetail.Rows.Clear();
			for (int i = 0; i < invGrn.invGrnDetail.Count; i++)
			{
				dtdetail.Rows.Add(invGrn.invGrnDetail[i].Id, invGrn.invGrnDetail[i].PurchaseOrderDetailId, invGrn.invGrnDetail[i].PurchaseOrderId, invGrn.invGrnDetail[i].PurchaserOrderNo, invGrn.invGrnDetail[i].WarehouseId, invGrn.invGrnDetail[i].WareHouseCode, invGrn.invGrnDetail[i].JobLotId, invGrn.invGrnDetail[i].JobLot, invGrn.invGrnDetail[i].ItemId, invGrn.invGrnDetail[i].Item, invGrn.invGrnDetail[i].ItemUomId, invGrn.invGrnDetail[i].UOMCode, invGrn.invGrnDetail[i].ItemQty, invGrn.invGrnDetail[i].GrossWeight, invGrn.invGrnDetail[i].AdLsWeight, invGrn.invGrnDetail[i].NetBillWeight, invGrn.invGrnDetail[i].CityId, invGrn.invGrnDetail[i].AreaCity, invGrn.invGrnDetail[i].CommentsDetail);
			}
			grd.DataSource = dtdetail;
			grd.RetrieveStructure();
			grdSettings();
			FormHelper.LoadAttachmentsForObject(AT, Id, base.Name);
			DocDate.Focus();
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
		if (((UltraGridBase)CmbSupplier).ActiveRow == null || Conversion.ToInt(CmbSupplier.Value) == 0)
		{
			MessageBox.Show("Supplier Field is Required");
			CmbSupplier.Focus();
			return false;
		}
		if (((Control)(object)CmbDeliveryTerm).Text.Trim() == "" || ((Control)(object)CmbDeliveryTerm).Text.Trim() == "0" || ((Control)(object)CmbDeliveryTerm).Text.Trim() == null)
		{
			MessageBox.Show("DeliveryTerm Field is Required");
			CmbDeliveryTerm.Focus();
			return false;
		}
		if (Conversion.ToInt(combvehtyp.Value) <= 0)
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
		if (txtbltyno.Text.Trim() == "" || txtbltyno.Text.Trim() == "0" || txtbltyno.Text.Trim() == null)
		{
			MessageBox.Show("Bilty No Field is Required");
			txtbltyno.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)CmbWareHouse).ActiveRow == null || Conversion.ToInt(CmbWareHouse.Value) == 0)
		{
			MessageBox.Show("Warehouse Field is Required");
			CmbWareHouse.Focus();
			return false;
		}
		if (((UltraGridBase)CmbJobLot).ActiveRow == null || Conversion.ToInt(CmbJobLot.Value) == 0)
		{
			MessageBox.Show("JobLot Field is Required");
			CmbJobLot.Focus();
			return false;
		}
		if (((UltraGridBase)CmbItem).ActiveRow == null || Conversion.ToInt(CmbItem.Value) == 0)
		{
			MessageBox.Show("Item Name Field is Required");
			CmbItem.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackUom).ActiveRow == null || Conversion.ToInt(CmbPackUom.Value) == 0)
		{
			MessageBox.Show("Pack Unit Field is Required");
			CmbPackUom.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtqty.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Qty Field is Required");
			txtqty.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtgwt.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Gross Weight Field is Required");
			txtgwt.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtnetwt.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Net Bill Weight Field is Required");
			txtnetwt.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCity).ActiveRow == null || Conversion.ToInt(CmbCity.Value) == 0)
		{
			MessageBox.Show("City Field is Required");
			CmbCity.Focus();
			return false;
		}
		return true;
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
				DocumentTypeId = 1858,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			}));
			txtdocno.Text = DocNo.ToString();
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
			if (SubsidiaryAccountAllownOnVouchers)
			{
				DataTable dtsupbyGrpId = CommonServices.GetVendorsAndCustomers(1);
				if (dtsupbyGrpId.Rows.Count > 0)
				{
					DDL.BindDDL(dtsupbyGrpId, CmbSupplier, "Id", "CompanyName", "Supplier Name", ZeroIndex: false);
					DDL.BindDDL(dtsupbyGrpId, combsupplier, "Id", "CompanyName", "Supplier Name", ZeroIndex: false);
					((UltraGridBase)CmbSupplier).DisplayLayout.Bands[0].Columns[3].Hidden = true;
					((UltraGridBase)combsupplier).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				}
			}
			else
			{
				DataTable dtsupbyGrpId2 = CommonServices.SupplierCustomerGetforComboServiceBind();
				if (dtsupbyGrpId2.Rows.Count > 0)
				{
					DDL.BindDDL(dtsupbyGrpId2, CmbSupplier, "Id", "CompanyName", "Supplier Name", ZeroIndex: false);
					DDL.BindDDL(dtsupbyGrpId2, combsupplier, "Id", "CompanyName", "Supplier Name", ZeroIndex: false);
				}
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
			int ItId = Conversion.ToInt(combvehtyp.Value);
			DataTable dtVehicle = VehicleType.GetAll();
			DDL.BindDDLNew(dtVehicle, combvehtyp, "Id", "VehicleDescription", "VehicleDescription", true);
			((UltraGridBase)combvehtyp).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (ItId > 0)
			{
				if (dtVehicle.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId))
				{
					combvehtyp.Value = ItId;
				}
				else
				{
					((Control)(object)combvehtyp).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemNameFill()
	{
		try
		{
			int ItId = Conversion.ToInt(CmbItem.Value);
			DataTable dtitem = CommonServices.ItemGetForComboServiceBindForEngr();
			DDL.BindDDLNew(dtitem, CmbItem, "Id", "ItemName", "Item Name", true);
			((UltraGridBase)CmbItem).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (ItId > 0)
			{
				if (dtitem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId))
				{
					CmbItem.Value = ItId;
				}
				else
				{
					((Control)(object)CmbItem).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackUOMFillWithoutOrder()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		try
		{
			string PackUom = Conversion.ToString((object)((Control)(object)CmbPackUom).Text);
			UOMSchedule val = new UOMSchedule
			{
				ItemId = Conversion.ToInt(CmbItem.Value),
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtsearchby = new DataTable
			{
				Columns = { "Id", "UOMCode", "Equivalent" }
			};
			DataTable dt = UOMSchedule.SearchByObject(val);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtsearchby.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["UOMCode"], dt.Rows[i]["Equivalent"]);
				}
			}
			DDL.BindDDL(dtsearchby, CmbPackUom, "Id", "UOMCode", "Pack UOM", ZeroIndex: true);
			((UltraGridBase)CmbPackUom).DisplayLayout.Bands[0].Columns[1].Width = 75;
			((UltraGridBase)CmbPackUom).DisplayLayout.Bands[0].Columns[2].Width = 85;
			if (PackUom != string.Empty)
			{
				if (dtsearchby.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == PackUom))
				{
					((Control)(object)CmbPackUom).Text = PackUom;
				}
				else
				{
					((Control)(object)CmbPackUom).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CityFill()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		try
		{
			City val = new City
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			int ItId = Conversion.ToInt(CmbCity.Value);
			DataTable dtcityall = new DataTable();
			dtcityall = City.GetAll(val);
			if (dtcityall.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dtcityall, CmbCity, "Id", "CityName", "City Name", true);
			((UltraGridBase)CmbCity).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (ItId > 0)
			{
				if (dtcityall.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId))
				{
					CmbCity.Value = ItId;
				}
				else
				{
					((Control)(object)CmbCity).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JobLotFill()
	{
		try
		{
			int ItId = Conversion.ToInt(CmbJobLot.Value);
			DataTable dt = CommonServices.JobLotGetAllService();
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dt, CmbJobLot, "Id", "JobLotDescription", "JobLotDescription", true);
			((UltraGridBase)CmbJobLot).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (ItId > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId))
				{
					CmbJobLot.Value = ItId;
				}
				else
				{
					((Control)(object)CmbJobLot).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WareHouseFill()
	{
		try
		{
			int ItId = Conversion.ToInt(CmbWareHouse.Value);
			DataTable dt = CommonServices.getActiveWareHouse();
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dt, CmbWareHouse, "Id", "WareHouseName", "WareHouseName", true);
			((UltraGridBase)CmbWareHouse).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (ItId > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId))
				{
					CmbWareHouse.Value = ItId;
				}
				else
				{
					((Control)(object)CmbWareHouse).Text = string.Empty;
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
			double PackUOM = 0.0;
			double Qty = 0.0;
			double GrossWeight = 0.0;
			double NetWeight = 0.0;
			double AddLess = 0.0;
			if (Conversion.ToInt(CmbPackUom.Value) > 0)
			{
				PackUOM = Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value);
			}
			if (txtqty.Text != string.Empty && txtqty.Text != null)
			{
				Qty = Conversion.ToDouble((object)txtqty.Text.Trim());
			}
			GrossWeight = PackUOM * Qty;
			txtgwt.Text = Math.Round(GrossWeight).ToString();
			AddLess = ((!(txtadlswt.Text != string.Empty) || txtadlswt.Text == null) ? 0.0 : Conversion.ToDouble((object)txtadlswt.Text.Trim()));
			NetWeight = GrossWeight - AddLess;
			txtnetwt.Text = Math.Round(NetWeight, 2).ToString();
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
			OrderId = 0;
			((Control)(object)CmbSupplier).Text = string.Empty;
			((UltraGridBase)combvehtyp).Rows[0].Activate();
			txtvehno.Text = string.Empty;
			txtbltyno.Text = string.Empty;
			txtremarks.Text = string.Empty;
			((Control)(object)CmbItem).Text = string.Empty;
			txtqty.Text = string.Empty;
			txtadlswt.Text = string.Empty;
			txtremarksdetail.Text = string.Empty;
			txtnetwt.Text = string.Empty;
			((Control)(object)CmbCity).Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			((Control)(object)CmbDeliveryTerm).Text = string.Empty;
			btnSave.Visible = true;
			DocDate.Focus();
			dtdetail.Clear();
			grd.DataSource = dtdetail;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			((Control)(object)CmbSupplier).Enabled = true;
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			GenerateCode();
			ResetDetial();
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			((Control)(object)CmbDeliveryTerm).Enabled = true;
			((Control)(object)CmbItem).Enabled = true;
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
			((Control)(object)CmbItem).Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			txtqty.Text = string.Empty;
			txtgwt.Text = string.Empty;
			txtadlswt.Text = string.Empty;
			txtnetwt.Text = string.Empty;
			CmbWareHouse.Focus();
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
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["OrderDetailId"].Visible = false;
			grd.RootTable.Columns["OrderId"].Visible = false;
			grd.RootTable.Columns["WarehouseId"].Visible = false;
			grd.RootTable.Columns["JobLotId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["UOMId"].Visible = false;
			grd.RootTable.Columns["CityId"].Visible = false;
			grd.RootTable.Columns["UOM"].Caption = "PackUom";
			grd.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["GrossWight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["AddLesswt"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["GrossWight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AddLesswt"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Qty"].FormatString = "#,#";
			grd.RootTable.Columns["GrossWight"].FormatString = "#,##.##";
			grd.RootTable.Columns["AddLesswt"].FormatString = "#,##.##";
			grd.RootTable.Columns["NetWeight"].FormatString = "#,##.##";
			grd.RootTable.Columns["Qty"].TotalFormatString = "#,#";
			grd.RootTable.Columns["GrossWight"].TotalFormatString = "#,##.##";
			grd.RootTable.Columns["AddLesswt"].TotalFormatString = "#,##.##";
			grd.RootTable.Columns["NetWeight"].TotalFormatString = "#,##.##";
			CommonServices.GridAutoAdjustmentNew(grd);
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "X";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
			grd.RootTable.Columns["Delete"].Width = 20;
			grd.RootTable.Columns["Delete"].Position = 0;
			if (OrderId > 0)
			{
				grd.RootTable.Columns.Add("Add");
				grd.RootTable.Columns["Add"].Key = "Add";
				grd.RootTable.Columns["Add"].Caption = "+";
				grd.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grd.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
				grd.RootTable.Columns["Add"].ButtonText = "+";
				grd.RootTable.Columns["Add"].Width = 20;
				grd.RootTable.Columns["Add"].Position = 1;
				grd.FrozenColumns = 2;
			}
			else
			{
				grd.FrozenColumns = 1;
			}
			ctrlGrdBar3_Load(null, null);
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
			int ItId = Conversion.ToInt(CmbDeliveryTerm.Value);
			DataTable dt = CommonServices.StaticColumnsService("DeliveryTerm");
			DDL.BindDDLNew(dt, CmbDeliveryTerm, "Id", "type", "Delivery Term", true);
			((UltraGridBase)CmbDeliveryTerm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)CmbDeliveryTerm).Rows[1].Activate();
			if (ItId > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId))
				{
					CmbDeliveryTerm.Value = ItId;
				}
				else
				{
					((Control)(object)CmbDeliveryTerm).Text = string.Empty;
				}
			}
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
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMargin = new Size(1016, 775);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnSave.Enabled = formright.DoHaveSaveRight;
			printToolStripButton.Enabled = formright.DoHavePrintRights;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
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
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			dtdetail.Columns.Add("Id", typeof(int));
			dtdetail.Columns.Add("OrderDetailId", typeof(int));
			dtdetail.Columns.Add("OrderId", typeof(int));
			dtdetail.Columns.Add("OrderNo", typeof(int));
			dtdetail.Columns.Add("WarehouseId", typeof(int));
			dtdetail.Columns.Add("Warehouse");
			dtdetail.Columns.Add("JobLotId", typeof(int));
			dtdetail.Columns.Add("JobLot");
			dtdetail.Columns.Add("ItemId", typeof(int));
			dtdetail.Columns.Add("Item");
			dtdetail.Columns.Add("UOMId", typeof(int));
			dtdetail.Columns.Add("UOM");
			dtdetail.Columns.Add("Qty", typeof(double));
			dtdetail.Columns.Add("GrossWight", typeof(double));
			dtdetail.Columns.Add("AddLesswt", typeof(double));
			dtdetail.Columns.Add("NetWeight", typeof(double));
			dtdetail.Columns.Add("CityId", typeof(int));
			dtdetail.Columns.Add("City");
			dtdetail.Columns.Add("RemarksDetail");
			grd.DataSource = dtdetail;
			grd.RetrieveStructure();
			grdSettings();
			GenerateCode();
			SupplierNameFilll();
			DeliveryTerm();
			VehicleTypesFill();
			WareHouseFill();
			JobLotFill();
			CityFill();
			ItemNameFill();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			DocDate.Focus();
			ChkBox.Checked = true;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
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
			grn.FromDate = Conversion.ToDateTime((object)fromdate.Value);
			grn.ToDate = Conversion.ToDateTime((object)ToDate.Value);
			grn.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			grn.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			grn.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			grn.NoOfRecords = NoOfRecords;
			grn.DocumentTypeId = 1858;
			if (!grn.CanViewAllRecord)
			{
				grn.EntryUser = UserAccount.ID;
			}
			DataTable dthis = InvGrn.GetHisoty(grn);
			if (dthis.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("OrderId", typeof(int));
				dt.Columns.Add("OrderNo", typeof(string));
				dt.Columns.Add("SupplierName", typeof(string));
				dt.Columns.Add("DeliveryTerm", typeof(string));
				dt.Columns.Add("VehicleNo", typeof(string));
				dt.Columns.Add("BiltyNo", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				dt.Columns.Add("Remarks", typeof(string));
				dt.Clear();
				for (int i = 0; i < dthis.Rows.Count; i++)
				{
					dt.Rows.Add(Conversion.ToInt(dthis.Rows[i]["Id"]), Conversion.ToInt(dthis.Rows[i]["DocumentTypeId"]), Conversion.ToInt(dthis.Rows[i]["DocNo"]), Conversion.ToDateTime(dthis.Rows[i]["DocDate"]).ToShortDateString(), Conversion.ToInt(dthis.Rows[i]["OrderId"]), Conversion.ToString(dthis.Rows[i]["OrderNo"]), Conversion.ToString(dthis.Rows[i]["SupplierName"]), Conversion.ToString(dthis.Rows[i]["DeliveryTerm"]), Conversion.ToString(dthis.Rows[i]["VehicleNo"]), Conversion.ToString(dthis.Rows[i]["BiltyNo"]), Conversion.ToDateTime(dthis.Rows[i]["EntryDate"]), Conversion.ToString(dthis.Rows[i]["EntryUser"]), Conversion.ToDateTime(dthis.Rows[i]["ModifyDate"]), Conversion.ToString(dthis.Rows[i]["ModifyUser"]), Conversion.ToInt(dthis.Rows[i]["NoOfAttachments"]), Conversion.ToString(dthis.Rows[i]["RemarksHeader"]));
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
			GrdHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			GrdHistory.RootTable.Columns["OrderId"].Visible = false;
			GrdHistory.RootTable.Columns["Id"].Visible = false;
			GrdHistory.RootTable.Columns["OrderNo"].Visible = false;
			GrdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			CommonServices.GridAutoAdjustmentNew(GrdHistory);
			GrdHistory.RootTable.Columns.Add("Edit");
			GrdHistory.RootTable.Columns["Edit"].Caption = "Edit";
			GrdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			GrdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			GrdHistory.RootTable.Columns["Edit"].Width = 40;
			GrdHistory.RootTable.Columns.Add("Print");
			GrdHistory.RootTable.Columns["Print"].Caption = "Print";
			GrdHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			GrdHistory.RootTable.Columns["Print"].ButtonText = "Print";
			GrdHistory.RootTable.Columns["Print"].Width = 40;
			GrdHistory.RootTable.Columns["Edit"].Position = 0;
			GrdHistory.RootTable.Columns["Print"].Position = 1;
			GrdHistory.FrozenColumns = 2;
			ctrlGrdBar2_Load(null, null);
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
					fromdate.Focus();
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
				if (e.Control && e.KeyCode == Keys.S && btnSave.Visible && btnSave.Enabled)
				{
					btnSave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.P && printToolStripButton.Visible && printToolStripButton.Enabled)
				{
					printToolStripButton_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					toolStripButton1_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					DocDate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					AT.Show();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grd).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					CmbWareHouse.Focus();
				}
				if (e.Control && e.KeyCode == Keys.L)
				{
					BtnLoadOrder_Click(null, null);
				}
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				if (e.Control && e.KeyCode == Keys.S)
				{
					btnshow_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)GrdHistory).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					fromdate.Focus();
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
			dt.Rows.Add("Ctrl+S", "For Save When on Entry form and For Show Data when on History Form");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+Shift+Delete", "For Delete");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+L", "For Load Order");
			dt.Rows.Add("Ctrl+Delete", "For Deleting an row of Focused Grid");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On First Column Of Detail(Warehouse)");
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

	private void btnshortcutkeys_Click(object sender, EventArgs e)
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
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
			MessageBox.Show(ex.Message);
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
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
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
			else if (e.Column.Key == "Add")
			{
				GridEXRow item2 = grd.CurrentRow;
				if (item2 != null && (int)item2.RowType == 0)
				{
					DataRow dr = dtdetail.Rows[item2.RowIndex];
					dr["Id"] = 0;
					dtdetail.Rows.Add(dr.ItemArray);
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
				GenerateReport(Conversion.ToInt((object)GrdHistory.CurrentRow.Cells["Id"].Value.ToString()));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null || ((int)item.RowType == 0 && item.Cells.Count > 0))
			{
				updateDetailIndex = item.RowIndex;
				CmbWareHouse.Value = item.Cells["WarehouseId"].Value.ToString();
				CmbJobLot.Value = item.Cells["JobLotId"].Value.ToString();
				CmbItem.Value = Conversion.ToInt(item.Cells["ItemId"].Value);
				CmbPackUom.Value = item.Cells["UOMId"].Value.ToString();
				txtqty.Text = item.Cells["Qty"].Value.ToString();
				txtgwt.Text = item.Cells["GrossWight"].Value.ToString();
				txtadlswt.Text = item.Cells["AddLesswt"].Value.ToString();
				txtnetwt.Text = item.Cells["NetWeight"].Value.ToString();
				CmbCity.Value = Conversion.ToInt(item.Cells["CityId"].Value);
				txtremarksdetail.Text = item.Cells["RemarksDetail"].Text.ToString();
				((Control)(object)CmbItem).Enabled = Conversion.ToInt(item.Cells["OrderId"].Value) <= 0;
				btnplus.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				CmbWareHouse.Focus();
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
				double num = Conversion.ToDouble((object)txtnetwt.Text.Trim());
				double GrossWeight = Conversion.ToDouble((object)txtgwt.Text.Trim());
				if (num > GrossWeight)
				{
					throw new Exception("NetBillWeight cannot greater than Gross Weight Please check");
				}
				dtdetail.Rows[updateDetailIndex]["WarehouseId"] = CmbWareHouse.Value;
				dtdetail.Rows[updateDetailIndex]["Warehouse"] = ((Control)(object)CmbWareHouse).Text;
				dtdetail.Rows[updateDetailIndex]["JobLotId"] = CmbJobLot.Value;
				dtdetail.Rows[updateDetailIndex]["JobLot"] = ((Control)(object)CmbJobLot).Text;
				dtdetail.Rows[updateDetailIndex]["ItemId"] = CmbItem.Value;
				dtdetail.Rows[updateDetailIndex]["Item"] = ((Control)(object)CmbItem).Text;
				dtdetail.Rows[updateDetailIndex]["UOMId"] = CmbPackUom.Value;
				dtdetail.Rows[updateDetailIndex]["UOM"] = ((Control)(object)CmbPackUom).Text;
				dtdetail.Rows[updateDetailIndex]["Qty"] = txtqty.Text;
				dtdetail.Rows[updateDetailIndex]["GrossWight"] = txtgwt.Text;
				dtdetail.Rows[updateDetailIndex]["AddLesswt"] = Conversion.ToDouble((object)txtadlswt.Text.Trim());
				dtdetail.Rows[updateDetailIndex]["NetWeight"] = txtnetwt.Text;
				dtdetail.Rows[updateDetailIndex]["CityId"] = CmbCity.Value;
				dtdetail.Rows[updateDetailIndex]["City"] = ((Control)(object)CmbCity).Text;
				dtdetail.Rows[updateDetailIndex]["RemarksDetail"] = txtremarksdetail.Text;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				((Control)(object)CmbItem).Enabled = true;
				ResetDetial();
				grd.UpdateData();
				Total();
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
			SupplierNameFilll();
			VehicleTypesFill();
			CityFill();
			ItemNameFill();
			WareHouseFill();
			JobLotFill();
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
			CommonServices.GrnAgainstOrderConcrete1858(PrintId);
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

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		btnUpdateDetail.Visible = false;
		btnCancelUpdateDetial.Visible = false;
		btnplus.Visible = true;
	}

	private void btnLoadAll_Click(object sender, EventArgs e)
	{
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
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 1858);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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

	private void BtnLoadOrder_Click(object sender, EventArgs e)
	{
		try
		{
			try
			{
				if (grd.GetRows().Count() == 0)
				{
					LoadPO = new LoadPurchaseOrderWithQty(UserAccount);
					LoadPO.ShowDialog();
					LoadPO.BringToFront();
					LoadPurchaseOrder();
					return;
				}
				if (Conversion.ToInt(grd.GetRow(0).Cells["OrderId"].Value) > 0)
				{
					LoadPO = new LoadPurchaseOrderWithQty(UserAccount);
					((Control)(object)LoadPO.CmbSupplier).Enabled = false;
					LoadPO.ShowDialog();
					LoadPO.BringToFront();
					LoadPurchaseOrder();
					return;
				}
				throw new Exception("You Can't Load Purchase Order Because Direct Entry Already Exist");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message);
		}
	}

	public void LoadPurchaseOrder()
	{
		try
		{
			DataTable invGrn = LoadPO.dtPurchaseOrder;
			if (invGrn.Rows.Count <= 0)
			{
				return;
			}
			if (grd.GetRows().Count() > 0 && Conversion.ToInt(grd.GetRow(0).Cells["OrderId"].Value) != Conversion.ToInt(invGrn.Rows[0]["Id"]))
			{
				throw new Exception("Already Loaded Row's Have Different Order. So you Can't Load Rows Of Different Order!");
			}
			OrderId = Conversion.ToInt(invGrn.Rows[0]["Id"]);
			((Control)(object)CmbSupplier).Enabled = false;
			txtremarks.Text = invGrn.Rows[0]["RemarksHeader"].ToString();
			((Control)(object)CmbDeliveryTerm).Text = invGrn.Rows[0]["DeliveryTerm"].ToString();
			CmbSupplier.Value = invGrn.Rows[0]["OrderSupplierId"];
			for (int i = 0; i < invGrn.Rows.Count; i++)
			{
				bool flag = false;
				if (grd.GetRows().Length != 0)
				{
					GridEXRow[] rows = grd.GetRows();
					for (int j = 0; j < rows.Length; j++)
					{
						if (Conversion.ToInt(rows[j].Cells["OrderDetailId"].Value) == Conversion.ToInt(invGrn.Rows[i]["PoDId"]))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					dtdetail.Rows.Add(0, invGrn.Rows[i]["PoDId"], invGrn.Rows[i]["Id"], invGrn.Rows[i]["DocNo"], 0, "", invGrn.Rows[i]["JobLotId"], invGrn.Rows[i]["JobLotDescription"], invGrn.Rows[i]["OrderItemId"], invGrn.Rows[i]["ItemName"], invGrn.Rows[i]["OrderItemUOMId"], invGrn.Rows[i]["UOMCodeItm"], invGrn.Rows[i]["BalQty"], invGrn.Rows[i]["BalWeight"], 0, invGrn.Rows[i]["BalWeight"], invGrn.Rows[i]["CityId"], invGrn.Rows[i]["CityName"], invGrn.Rows[i]["RemarksDetail"]);
				}
			}
			grd.DataSource = dtdetail;
			grd.RetrieveStructure();
			grdSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GRNDirectAgainstOrder_FormClosing(object sender, FormClosingEventArgs e)
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

	private void GrdHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		try
		{
			if (GrdHistory.CurrentRow == null || (int)GrdHistory.CurrentRow.RowType != 0 || GrdHistory.CurrentRow.Cells.Count <= 0)
			{
				return;
			}
			Id = Conversion.ToInt(GrdHistory.CurrentRow.Cells["Id"].Value);
			InvGrn po = new InvGrn();
			po = InvGrn.GetByID(Conversion.ToInt(GrdHistory.CurrentRow.Cells["Id"].Value));
			if (po != null)
			{
				DataTable dthistorydetail = new DataTable();
				dthistorydetail.Columns.Add("OrderNo");
				dthistorydetail.Columns.Add("WareHouse");
				dthistorydetail.Columns.Add("JobLot");
				dthistorydetail.Columns.Add("Item");
				dthistorydetail.Columns.Add("PackUom");
				dthistorydetail.Columns.Add("Qty", typeof(double));
				dthistorydetail.Columns.Add("GrossWight", typeof(double));
				dthistorydetail.Columns.Add("AddLesswt", typeof(double));
				dthistorydetail.Columns.Add("NetWeight", typeof(double));
				dthistorydetail.Columns.Add("City");
				dthistorydetail.Columns.Add("RemarksDetail");
				dthistorydetail.Clear();
				for (int i = 0; i < po.invGrnDetail.Count; i++)
				{
					dthistorydetail.Rows.Add(po.invGrnDetail[i].PurchaserOrderNo, po.invGrnDetail[i].WareHouseCode, po.invGrnDetail[i].JobLot, po.invGrnDetail[i].Item, po.invGrnDetail[i].UOMCode, po.invGrnDetail[i].ItemQty, po.invGrnDetail[i].GrossWeight, po.invGrnDetail[i].AdLsWeight, po.invGrnDetail[i].NetBillWeight, po.invGrnDetail[i].AreaCity, po.invGrnDetail[i].CommentsDetail);
				}
				GrdHistoryDetail.DataSource = dthistorydetail;
				GrdHistoryDetail.RetrieveStructure();
				DataGridHistoryDetailSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistoryDetailSetting()
	{
		try
		{
			GrdHistoryDetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["Qty"].FormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["GrossWight"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["GrossWight"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["GrossWight"].FormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["GrossWight"].TotalFormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["AddLesswt"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["AddLesswt"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["AddLesswt"].FormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["AddLesswt"].TotalFormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["NetWeight"].FormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustmentNew(GrdHistoryDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
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

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grd.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt(r.Cells["Id"].Value);
				if (grd.CurrentColumn.Key == "Delete")
				{
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
				}
				else if (grd.CurrentColumn.Key == "Add")
				{
					GridEXRow item = grd.CurrentRow;
					if (item != null && (int)item.RowType == 0)
					{
						DataRow dr = dtdetail.Rows[item.RowIndex];
						dr["Id"] = 0;
						dtdetail.Rows.Add(dr.ItemArray);
					}
				}
			}
			if (e.Control && e.KeyCode == Keys.Delete && r != null && (int)r.RowType == 0)
			{
				r.Delete();
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			int ID = 0;
			int documentTypeId = 0;
			GridEXRow item = GrdHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (GrdHistory.GetRows().Count() > 0)
			{
				ID = Conversion.ToInt(item.Cells["Id"].Value);
				documentTypeId = Conversion.ToInt(item.Cells["DocumentTypeId"].Value);
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				ReadById_Update(ID);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (GrdHistory.CurrentColumn.Key == "Edit")
				{
					ReadById_Update(ID);
				}
				else if (GrdHistory.CurrentColumn.Key == "Print")
				{
					GenerateReport(ID);
				}
				else if (GrdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(ID, documentTypeId);
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
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Expected O, but got Unknown
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Expected O, but got Unknown
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Expected O, but got Unknown
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Expected O, but got Unknown
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Expected O, but got Unknown
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Expected O, but got Unknown
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Expected O, but got Unknown
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Expected O, but got Unknown
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Expected O, but got Unknown
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Expected O, but got Unknown
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Expected O, but got Unknown
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Expected O, but got Unknown
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Expected O, but got Unknown
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Expected O, but got Unknown
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Expected O, but got Unknown
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Expected O, but got Unknown
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e7: Expected O, but got Unknown
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Expected O, but got Unknown
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Expected O, but got Unknown
		//IL_0409: Unknown result type (might be due to invalid IL or missing references)
		//IL_0413: Expected O, but got Unknown
		//IL_04e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Expected O, but got Unknown
		//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0505: Expected O, but got Unknown
		//IL_055e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0568: Expected O, but got Unknown
		//IL_0619: Unknown result type (might be due to invalid IL or missing references)
		//IL_0623: Expected O, but got Unknown
		//IL_0650: Unknown result type (might be due to invalid IL or missing references)
		//IL_065a: Expected O, but got Unknown
		//IL_069d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a7: Expected O, but got Unknown
		//IL_0d30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3a: Expected O, but got Unknown
		//IL_5560: Unknown result type (might be due to invalid IL or missing references)
		//IL_556a: Expected O, but got Unknown
		//IL_558e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5598: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
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
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Pcc.Purchase.GrnAgainstOrder));
		GridEXLayout GrdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance121 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label6 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.btnplus = new System.Windows.Forms.Button();
		this.label22 = new System.Windows.Forms.Label();
		this.txtremarksdetail = new System.Windows.Forms.TextBox();
		this.CmbItem = new UltraCombo();
		this.label36 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.CmbJobLot = new UltraCombo();
		this.CmbPackUom = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbWareHouse = new UltraCombo();
		this.CmbCity = new UltraCombo();
		this.label42 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.txtnetwt = new System.Windows.Forms.TextBox();
		this.txtadlswt = new System.Windows.Forms.TextBox();
		this.txtqty = new System.Windows.Forms.TextBox();
		this.txtgwt = new System.Windows.Forms.TextBox();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label27 = new System.Windows.Forms.Label();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.label34 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.CmbDeliveryTerm = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.combvehtyp = new UltraCombo();
		this.label25 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.txtbltyno = new System.Windows.Forms.TextBox();
		this.txtvehno = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.CmbSupplier = new UltraCombo();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.label5 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.BtnLoadOrder = new System.Windows.Forms.ToolStripButton();
		this.printToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel11 = new System.Windows.Forms.Panel();
		this.GrdHistoryDetail = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label20 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel15 = new System.Windows.Forms.Panel();
		this.GrdHistory = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.btnshow = new System.Windows.Forms.Button();
		this.ToDate = new System.Windows.Forms.DateTimePicker();
		this.label9 = new System.Windows.Forms.Label();
		this.fromdate = new System.Windows.Forms.DateTimePicker();
		this.label10 = new System.Windows.Forms.Label();
		this.combsupplier = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.panel12 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label16 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.btnGrnFormHistory = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel3.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCity).BeginInit();
		this.panel4.SuspendLayout();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combvehtyp).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryDetail).BeginInit();
		this.panel16.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel15.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistory).BeginInit();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.combsupplier).BeginInit();
		this.panel12.SuspendLayout();
		this.panel7.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(884, 734);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(884, 734);
		this.tabControl1.TabIndex = 0;
		this.tabPage1.BackColor = System.Drawing.Color.White;
		this.tabPage1.Controls.Add(this.panel10);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(876, 705);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.panel10.Controls.Add(this.panel6);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(2, 264);
		this.panel10.Margin = new System.Windows.Forms.Padding(2);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(872, 439);
		this.panel10.TabIndex = 5;
		this.panel6.Controls.Add(this.panel9);
		this.panel6.Controls.Add(this.panel3);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(872, 439);
		this.panel6.TabIndex = 2;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 26);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(872, 413);
		this.panel9.TabIndex = 586;
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
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(872, 413);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grd).TabStop = false;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ctrlGrdBar3);
		this.panel3.Controls.Add(this.label6);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(872, 26);
		this.panel3.TabIndex = 585;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(832, 2);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar3.MyGrid = this.grd;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 25;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(10, 3);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(58, 19);
		this.label6.TabIndex = 24;
		this.label6.Text = "Detail";
		this.panel5.Controls.Add(this.btnCancelUpdateDetial);
		this.panel5.Controls.Add(this.btnplus);
		this.panel5.Controls.Add(this.label22);
		this.panel5.Controls.Add(this.txtremarksdetail);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItem);
		this.panel5.Controls.Add(this.label36);
		this.panel5.Controls.Add(this.label41);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUom);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWareHouse);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCity);
		this.panel5.Controls.Add(this.label42);
		this.panel5.Controls.Add(this.label43);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Controls.Add(this.label35);
		this.panel5.Controls.Add(this.label28);
		this.panel5.Controls.Add(this.label46);
		this.panel5.Controls.Add(this.txtnetwt);
		this.panel5.Controls.Add(this.txtadlswt);
		this.panel5.Controls.Add(this.txtqty);
		this.panel5.Controls.Add(this.txtgwt);
		this.panel5.Controls.Add(this.btnUpdateDetail);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(2, 149);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(872, 115);
		this.panel5.TabIndex = 0;
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(797, 83);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(65, 26);
		this.btnCancelUpdateDetial.TabIndex = 587;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(724, 83);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(28, 26);
		this.btnplus.TabIndex = 586;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(Add_Click);
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(314, 62);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(52, 15);
		this.label22.TabIndex = 584;
		this.label22.Text = "Remarks";
		this.txtremarksdetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarksdetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarksdetail.Location = new System.Drawing.Point(301, 85);
		this.txtremarksdetail.Name = "txtremarksdetail";
		this.txtremarksdetail.Size = new System.Drawing.Size(420, 23);
		this.txtremarksdetail.TabIndex = 9;
		this.CmbItem.AllowNull = (DefaultableBoolean)1;
		this.CmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance205).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance205;
		((UltraGridBase)this.CmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance206).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance206).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance206;
		((AppearanceBase)appearance207).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance207;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance104).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItem).Location = new System.Drawing.Point(301, 31);
		((System.Windows.Forms.Control)(object)this.CmbItem).Name = "CmbItem";
		((System.Windows.Forms.Control)(object)this.CmbItem).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbItem).TabIndex = 2;
		this.CmbItem.ValueChanged += new System.EventHandler(combitem_Leave);
		((System.Windows.Forms.Control)(object)this.CmbItem).Leave += new System.EventHandler(combitem_Leave);
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label36.Location = new System.Drawing.Point(302, 11);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(67, 15);
		this.label36.TabIndex = 8;
		this.label36.Text = "Item Name";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label41.Location = new System.Drawing.Point(605, 11);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(54, 15);
		this.label41.TabIndex = 19;
		this.label41.Text = "Item Qty";
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance111;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance116).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(168, 31);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(132, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Leave += new System.EventHandler(combpckuom_Leave);
		this.CmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance171;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance172).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance172).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance172).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance173).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance175;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance176).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance177).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance177).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance177).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance177).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance177;
		((AppearanceBase)appearance178).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Location = new System.Drawing.Point(522, 31);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Name = "CmbPackUom";
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Size = new System.Drawing.Size(82, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Leave += new System.EventHandler(combpckuom_Leave);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(171, 11);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(43, 15);
		this.label1.TabIndex = 21;
		this.label1.Text = "JobLot";
		this.CmbWareHouse.AllowNull = (DefaultableBoolean)1;
		this.CmbWareHouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWareHouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWareHouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance182).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance182).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance182;
		((AppearanceBase)appearance183).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance183;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance184).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance184).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance184).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance185).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance185;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance186).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance187;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance188).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance189).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance189).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance189).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance189).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance190).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWareHouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Location = new System.Drawing.Point(5, 31);
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Name = "CmbWareHouse";
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Size = new System.Drawing.Size(162, 26);
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).TabIndex = 0;
		this.CmbCity.AllowNull = (DefaultableBoolean)1;
		this.CmbCity.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.CmbCity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance194).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance194).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance195).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance195;
		((SpecialBoxBase)((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance196).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance196).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance196).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.CmbCity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance197).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance197;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance198).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance200).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance200).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance201).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance201).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance201;
		((AppearanceBase)appearance202).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance203).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.CmbCity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCity).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCity).Location = new System.Drawing.Point(115, 83);
		((System.Windows.Forms.Control)(object)this.CmbCity).Name = "CmbCity";
		((System.Windows.Forms.Control)(object)this.CmbCity).Size = new System.Drawing.Size(185, 26);
		((System.Windows.Forms.Control)(object)this.CmbCity).TabIndex = 8;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label42.Location = new System.Drawing.Point(525, 11);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(63, 15);
		this.label42.TabIndex = 21;
		this.label42.Text = "Pack UOM";
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label43.Location = new System.Drawing.Point(694, 11);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(78, 15);
		this.label43.TabIndex = 23;
		this.label43.Text = "Gross Weight";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(7, 11);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(67, 15);
		this.label3.TabIndex = 25;
		this.label3.Text = "Warehouse";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label35.Location = new System.Drawing.Point(783, 11);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(80, 15);
		this.label35.TabIndex = 25;
		this.label35.Text = "Ad/Ls Weight";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label28.Location = new System.Drawing.Point(114, 61);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(62, 15);
		this.label28.TabIndex = 25;
		this.label28.Text = "City Name";
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label46.Location = new System.Drawing.Point(5, 62);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(68, 15);
		this.label46.TabIndex = 25;
		this.label46.Text = "Net Weight";
		this.txtnetwt.BackColor = System.Drawing.Color.White;
		this.txtnetwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtnetwt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtnetwt.Location = new System.Drawing.Point(5, 85);
		this.txtnetwt.Name = "txtnetwt";
		this.txtnetwt.ReadOnly = true;
		this.txtnetwt.ShortcutsEnabled = false;
		this.txtnetwt.Size = new System.Drawing.Size(108, 23);
		this.txtnetwt.TabIndex = 7;
		this.txtnetwt.TabStop = false;
		this.txtnetwt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtadlswt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtadlswt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtadlswt.Location = new System.Drawing.Point(783, 33);
		this.txtadlswt.Name = "txtadlswt";
		this.txtadlswt.ShortcutsEnabled = false;
		this.txtadlswt.Size = new System.Drawing.Size(80, 23);
		this.txtadlswt.TabIndex = 6;
		this.txtadlswt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtadlswt.TextChanged += new System.EventHandler(txtadlswt_TextChanged_1);
		this.txtadlswt.Leave += new System.EventHandler(txtadlswt_Leave);
		this.txtqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtqty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtqty.Location = new System.Drawing.Point(605, 33);
		this.txtqty.Name = "txtqty";
		this.txtqty.ShortcutsEnabled = false;
		this.txtqty.Size = new System.Drawing.Size(88, 23);
		this.txtqty.TabIndex = 4;
		this.txtqty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtqty.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.txtqty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.txtgwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtgwt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtgwt.Location = new System.Drawing.Point(694, 33);
		this.txtgwt.Name = "txtgwt";
		this.txtgwt.ReadOnly = true;
		this.txtgwt.ShortcutsEnabled = false;
		this.txtgwt.Size = new System.Drawing.Size(88, 23);
		this.txtgwt.TabIndex = 5;
		this.txtgwt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtgwt.TextChanged += new System.EventHandler(txtgwt_TextChanged);
		this.txtgwt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtgwt_KeyPress);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(724, 83);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(72, 26);
		this.btnUpdateDetail.TabIndex = 585;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.panel4.Controls.Add(this.panel13);
		this.panel4.Controls.Add(this.label34);
		this.panel4.Controls.Add(this.label15);
		this.panel4.Controls.Add(this.label14);
		this.panel4.Controls.Add(this.label53);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combvehtyp);
		this.panel4.Controls.Add(this.label25);
		this.panel4.Controls.Add(this.label24);
		this.panel4.Controls.Add(this.label19);
		this.panel4.Controls.Add(this.label21);
		this.panel4.Controls.Add(this.txtremarks);
		this.panel4.Controls.Add(this.txtbltyno);
		this.panel4.Controls.Add(this.txtvehno);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.panel4.Controls.Add(this.DocDate);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Controls.Add(this.txtdocno);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel4.Location = new System.Drawing.Point(2, 33);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(872, 116);
		this.panel4.TabIndex = 0;
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label27);
		this.panel13.Controls.Add(this.ChkBox);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(872, 26);
		this.panel13.TabIndex = 584;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.ForeColor = System.Drawing.Color.White;
		this.label27.Location = new System.Drawing.Point(4, 3);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(214, 19);
		this.label27.TabIndex = 24;
		this.label27.Text = "GOODS RECEIVING NOTE";
		this.ChkBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBox.AutoSize = true;
		this.ChkBox.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBox.ForeColor = System.Drawing.Color.White;
		this.ChkBox.Location = new System.Drawing.Point(747, 4);
		this.ChkBox.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(121, 19);
		this.ChkBox.TabIndex = 0;
		this.ChkBox.Text = "Preview On Save";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.ForeColor = System.Drawing.Color.Red;
		this.label34.Location = new System.Drawing.Point(90, 63);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(14, 17);
		this.label34.TabIndex = 4;
		this.label34.Text = "*";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.Red;
		this.label15.Location = new System.Drawing.Point(701, 35);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(14, 17);
		this.label15.TabIndex = 7;
		this.label15.Text = "*";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.Red;
		this.label14.Location = new System.Drawing.Point(416, 63);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(14, 17);
		this.label14.TabIndex = 6;
		this.label14.Text = "*";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.ForeColor = System.Drawing.Color.Red;
		this.label53.Location = new System.Drawing.Point(416, 35);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(14, 17);
		this.label53.TabIndex = 5;
		this.label53.Text = "*";
		this.CmbDeliveryTerm.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance123).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance123).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance124;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance130).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance131).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance131).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance131).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Location = new System.Drawing.Point(435, 30);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Name = "CmbDeliveryTerm";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Size = new System.Drawing.Size(186, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).TabIndex = 3;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(331, 36);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(80, 15);
		this.label4.TabIndex = 583;
		this.label4.Text = "Delivery Term";
		this.combvehtyp.AllowNull = (DefaultableBoolean)1;
		this.combvehtyp.AutoCompleteMode = (AutoCompleteMode)4;
		this.combvehtyp.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combvehtyp.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Appearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.combvehtyp).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combvehtyp).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combvehtyp).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combvehtyp).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance150;
		((SpecialBoxBase)((UltraGridBase)this.combvehtyp).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance152).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance152).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combvehtyp).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.combvehtyp).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combvehtyp).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance156).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance157).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance157).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance157).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance158).TextHAlignAsString = "Left";
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.combvehtyp).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combvehtyp).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combvehtyp).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combvehtyp).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combvehtyp.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combvehtyp).Location = new System.Drawing.Point(435, 58);
		((System.Windows.Forms.Control)(object)this.combvehtyp).Name = "combvehtyp";
		((System.Windows.Forms.Control)(object)this.combvehtyp).Size = new System.Drawing.Size(186, 26);
		((System.Windows.Forms.Control)(object)this.combvehtyp).TabIndex = 4;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(640, 64);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(48, 15);
		this.label25.TabIndex = 3;
		this.label25.Text = "Bilty No";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(9, 91);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(52, 15);
		this.label24.TabIndex = 0;
		this.label24.Text = "Remarks";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label19.Location = new System.Drawing.Point(640, 36);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(64, 15);
		this.label19.TabIndex = 0;
		this.label19.Text = "Vehicle No";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(331, 64);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(73, 15);
		this.label21.TabIndex = 8;
		this.label21.Text = "Vehicle Type";
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(105, 87);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(758, 23);
		this.txtremarks.TabIndex = 7;
		this.txtbltyno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbltyno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbltyno.Location = new System.Drawing.Point(716, 60);
		this.txtbltyno.Name = "txtbltyno";
		this.txtbltyno.Size = new System.Drawing.Size(146, 23);
		this.txtbltyno.TabIndex = 6;
		this.txtvehno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvehno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtvehno.Location = new System.Drawing.Point(716, 32);
		this.txtvehno.Name = "txtvehno";
		this.txtvehno.Size = new System.Drawing.Size(146, 23);
		this.txtvehno.TabIndex = 5;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(6, 64);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(86, 15);
		this.label2.TabIndex = 0;
		this.label2.Text = "Supplier Name";
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance137;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance143).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance144).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance144).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance144).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(105, 58);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 2;
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(182, 32);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(143, 23);
		this.DocDate.TabIndex = 1;
		this.DocDate.TabStop = false;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(6, 36);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(88, 15);
		this.label5.TabIndex = 3;
		this.label5.Text = "Doc No && Date";
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Enabled = false;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(105, 32);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.Size = new System.Drawing.Size(75, 23);
		this.txtdocno.TabIndex = 0;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(872, 31);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.btnNew, this.toolStripButton1, this.btnSave, this.btnUpdate, this.btnDelete, this.btnAttachment, this.BtnLoadOrder, this.printToolStripButton, this.btnshortcutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(872, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.ToolTipText = "Ctrl_N";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.toolStripButton1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(80, 24);
		this.toolStripButton1.Text = "&Refresh";
		this.toolStripButton1.ToolTipText = "Alt_R";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.ToolTipText = "Ctrl_S";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.ToolTipText = "Ctrl_U";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.icons8_delete_bin_16;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.ToolTipText = "Alt_A";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.BtnLoadOrder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.BtnLoadOrder.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnLoadOrder.Image = (System.Drawing.Image)resources.GetObject("BtnLoadOrder.Image");
		this.BtnLoadOrder.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoadOrder.Name = "BtnLoadOrder";
		this.BtnLoadOrder.Size = new System.Drawing.Size(83, 24);
		this.BtnLoadOrder.Text = "Load Order";
		this.BtnLoadOrder.Click += new System.EventHandler(BtnLoadOrder_Click);
		this.printToolStripButton.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.printToolStripButton.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.printToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.printToolStripButton.Name = "printToolStripButton";
		this.printToolStripButton.Size = new System.Drawing.Size(100, 24);
		this.printToolStripButton.Text = "1858-Print";
		this.printToolStripButton.ToolTipText = "Ctrl_P";
		this.printToolStripButton.Click += new System.EventHandler(printToolStripButton_Click);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.tabPage2.Controls.Add(this.panel11);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(876, 705);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistoryDetail);
		this.panel11.Controls.Add(this.panel16);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(2, 477);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(872, 226);
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
		this.GrdHistoryDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Name = "GrdHistoryDetail";
		((ScrollableBase)this.GrdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Size = new System.Drawing.Size(872, 204);
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).TabIndex = 45;
		this.GrdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.GrdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.label20);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 0);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(872, 22);
		this.panel16.TabIndex = 46;
		this.label20.AutoSize = true;
		this.label20.BackColor = System.Drawing.Color.Teal;
		this.label20.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.ForeColor = System.Drawing.Color.Transparent;
		this.label20.Location = new System.Drawing.Point(5, 4);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(200, 16);
		this.label20.TabIndex = 23;
		this.label20.Text = "Detail Of Above Selected Row";
		this.panel8.Controls.Add(this.panel15);
		this.panel8.Controls.Add(this.panel14);
		this.panel8.Controls.Add(this.panel12);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(2, 30);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(872, 447);
		this.panel8.TabIndex = 1;
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistory);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel15.Location = new System.Drawing.Point(0, 79);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(872, 368);
		this.panel15.TabIndex = 47;
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
		((System.Windows.Forms.Control)(object)this.GrdHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GrdHistory).Name = "GrdHistory";
		((ScrollableBase)this.GrdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Size = new System.Drawing.Size(872, 368);
		((System.Windows.Forms.Control)(object)this.GrdHistory).TabIndex = 44;
		this.GrdHistory.TotalRow = (InheritableBoolean)1;
		this.GrdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistory).VisualStyle = (VisualStyle)4;
		this.GrdHistory.ColumnButtonClick += new ColumnActionEventHandler(GrdHistory_ColumnButtonClick);
		this.GrdHistory.SelectionChanged += new System.EventHandler(GrdHistory_SelectionChanged);
		this.GrdHistory.LinkClicked += new ColumnActionEventHandler(GrdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdHistory).DoubleClick += new System.EventHandler(GrdHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.GrdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(GrdHistory_KeyDown);
		this.panel14.Controls.Add(this.btnshow);
		this.panel14.Controls.Add(this.ToDate);
		this.panel14.Controls.Add(this.label9);
		this.panel14.Controls.Add(this.fromdate);
		this.panel14.Controls.Add(this.label10);
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.combsupplier);
		this.panel14.Controls.Add(this.label12);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 25);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(872, 54);
		this.panel14.TabIndex = 46;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(494, 24);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(68, 26);
		this.btnshow.TabIndex = 158;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.ToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.CustomFormat = "dd-MMM-yy";
		this.ToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDate.Location = new System.Drawing.Point(132, 26);
		this.ToDate.Name = "ToDate";
		this.ToDate.Size = new System.Drawing.Size(119, 23);
		this.ToDate.TabIndex = 153;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(131, 5);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(48, 15);
		this.label9.TabIndex = 162;
		this.label9.Text = "To Date";
		this.fromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.CustomFormat = "dd-MMM-yy";
		this.fromdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.fromdate.Location = new System.Drawing.Point(11, 26);
		this.fromdate.Name = "fromdate";
		this.fromdate.Size = new System.Drawing.Size(119, 23);
		this.fromdate.TabIndex = 152;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(11, 5);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(63, 15);
		this.label10.TabIndex = 161;
		this.label10.Text = "From Date";
		this.combsupplier.AllowNull = (DefaultableBoolean)1;
		this.combsupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.combsupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combsupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combsupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.combsupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combsupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combsupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combsupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance140;
		((SpecialBoxBase)((UltraGridBase)this.combsupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance151).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance151).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combsupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.combsupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combsupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance164).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance165).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance165).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance165).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).TextHAlignAsString = "Left";
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.combsupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combsupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combsupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combsupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combsupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combsupplier).Location = new System.Drawing.Point(252, 24);
		((System.Windows.Forms.Control)(object)this.combsupplier).Name = "combsupplier";
		((System.Windows.Forms.Control)(object)this.combsupplier).Size = new System.Drawing.Size(239, 26);
		((System.Windows.Forms.Control)(object)this.combsupplier).TabIndex = 154;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(254, 4);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(86, 15);
		this.label12.TabIndex = 159;
		this.label12.Text = "Supplier Name";
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.ctrlGrdBar2);
		this.panel12.Controls.Add(this.label16);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(872, 25);
		this.panel12.TabIndex = 45;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(832, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.GrdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 23;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label16.AutoSize = true;
		this.label16.BackColor = System.Drawing.Color.Teal;
		this.label16.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.Transparent;
		this.label16.Location = new System.Drawing.Point(5, 2);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(250, 19);
		this.label16.TabIndex = 23;
		this.label16.Text = "Goods Receiving Note History";
		this.panel7.Controls.Add(this.toolStrip2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(2, 2);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(872, 28);
		this.panel7.TabIndex = 0;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnLoadAll, this.btnGrnFormHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(872, 27);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.btnLoadAll.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(71, 24);
		this.btnLoadAll.Text = "&LoadAll";
		this.btnLoadAll.ToolTipText = "Ctrl_L";
		this.btnLoadAll.Visible = false;
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
		base.ClientSize = new System.Drawing.Size(884, 734);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "GrnAgainstOrder";
		this.Text = "   ";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(GRNDirectAgainstOrder_FormClosing);
		base.Load += new System.EventHandler(InvFrmGRN_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvFrmGRN_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCity).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combvehtyp).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryDetail).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.panel8.ResumeLayout(false);
		this.panel15.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistory).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.combsupplier).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
