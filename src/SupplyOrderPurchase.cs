// using Architecture.Reports;
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
using Architecture.WinApp.Properties;
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
namespace Architecture.WinApp.Purchase
{

public class SupplyOrderPurchase : Form
{
	private bool DoHaveSaveRight;

	private bool DoHaveUpdateRights;

	private bool DoHaveCanViewAllRecordRights;

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private DataTable dtInvoice = new DataTable();

	private int updateDetailIndex;

	private DropDownBind DDL = new DropDownBind();

	private int Id;

	private bool IsApproved;

	private DataTable table = new DataTable();

	private DataTable dtWh = new DataTable();

	private DataTable dtsuppcus = new DataTable();

	private DataTable dtExport = new DataTable();

	public UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private TabPage tabPage2;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private ToolStripButton btnprint;

	private ToolStripButton btnattachment;

	private Label label23;

	private Panel panel4;

	private Label label33;

	private Label label13;

	private Label label18;

	private DateTimePicker DocDate;

	private Label label17;

	private TextBox txtdocno;

	private Label label5;

	private Panel panel6;

	private Panel panel7;

	private Panel panel9;

	private Panel panel8;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	private Button btnclose;

	private Panel panel12;

	internal GridEX grdhistory;

	private ToolStripButton btnRefresh;

	private ToolStrip toolStrip2;

	private ToolStripButton btnLoadAll;

	internal UltraCombo cmbproject;

	internal UltraCombo cmbbranch;

	private TextBox txtManualRefNo;

	private Label label1;

	private TextBox txtVehicleNo;

	internal GridEX grd;

	private GroupBox groupBox3;

	internal UltraCombo CmbSupplierCustomer;

	private Label label2;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	internal UltraCombo cmbCropYear;

	internal UltraCombo CmbPackUom;

	internal UltraCombo CmbOrderNo;

	internal UltraCombo CmbItemName;

	private Button btnplus;

	private TextBox txtRemarksDetail;

	private Label label47;

	private TextBox txtweight;

	private TextBox txtqty;

	private Label label35;

	private Label label39;

	private Label label38;

	private Label label36;

	private Label label37;

	private TextBox txtEBWtTotal;

	private Label label7;

	private TextBox txtEBWtUnit;

	private Label label6;

	private TextBox txtBiltyNo;

	private Label label4;

	private TextBox txtDestinationLocation;

	private Label label11;

	private TextBox txtRemarks;

	private Label label12;

	private Label label14;

	private TextBox txtGrossWeight;

	private Label label3;

	private TextBox txtMoisture;

	private Label label8;

	private ToolStripButton toolStripButton3;

	private Panel panel5;

	internal GridEX grdPendingSupplyOrder;

	private Button btnLoader;

	private Panel panel10;

	internal GridEX grdDetail;

	public SupplyOrderPurchase(UserAccount user)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void PurchsaeOrder_Load(object sender, EventArgs e)
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
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Update").ToList();
				DoHaveUpdateRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "CanView AllRecord").ToList();
				DoHaveCanViewAllRecordRights = Conversion.ToBool((object)newList[0].Value.ToString());
			}
			btnsave.Enabled = DoHaveSaveRight;
			btnupdate.Enabled = DoHaveUpdateRights;
			dtViewAttachments.Columns.Add("AttchmentPath");
			table.Columns.Add("PoDetailId");
			table.Columns.Add("PoHeaderId");
			table.Columns.Add("OrderNo");
			table.Columns.Add("SupplierCustomerId");
			table.Columns.Add("SupplierName");
			table.Columns.Add("ItemId");
			table.Columns.Add("ItemName");
			table.Columns.Add("CropYear");
			table.Columns.Add("PackSizeId");
			table.Columns.Add("PackSize");
			table.Columns.Add("Qty", typeof(double));
			table.Columns.Add("NetWeight", typeof(double));
			table.Columns.Add("EbWtUnit", typeof(double));
			table.Columns.Add("EbWtTotal", typeof(double));
			table.Columns.Add("GrossWeight", typeof(double));
			table.Columns.Add("Moisture");
			table.Columns.Add("RemarksDetail");
			grd.DataSource = table;
			if (dtWh.Columns.Count == 0)
			{
				dtWh.Columns.Add("Id");
				dtWh.Columns.Add("WareHouseName");
			}
			BranchFill();
			ProjectFill();
			OrderNoFill();
			SupplierFill();
			ItemFill();
			CropYearFill();
			DocumentNoFill();
			DocDate.Focus();
			btnupdate.Visible = false;
			btnsave.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			PendingPurchaseOrder();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((Control)(object)cmbbranch).Text.Trim() == string.Empty)
		{
			MessageBox.Show("branch field is required");
			cmbbranch.Focus();
			return false;
		}
		if (((Control)(object)cmbproject).Text.Trim() == string.Empty)
		{
			MessageBox.Show("Project Field is Required");
			cmbproject.Focus();
			return false;
		}
		if (txtdocno.Text.Trim() == string.Empty || txtdocno.Text.Trim() == "0")
		{
			MessageBox.Show("DocNo Field is Required");
			txtdocno.Focus();
			return false;
		}
		if (txtManualRefNo.Text.Trim() == string.Empty || txtManualRefNo.Text.Trim() == "0")
		{
			MessageBox.Show("Manual Ref No Field is Required");
			txtManualRefNo.Focus();
			return false;
		}
		if (txtVehicleNo.Text.Trim() == string.Empty || txtVehicleNo.Text.Trim() == "0")
		{
			MessageBox.Show("Vehicle No Field is Required");
			txtVehicleNo.Focus();
			return false;
		}
		if (txtBiltyNo.Text.Trim() == string.Empty || txtBiltyNo.Text.Trim() == "0")
		{
			MessageBox.Show("Bilty No Field is Required");
			txtBiltyNo.Focus();
			return false;
		}
		return true;
	}

	private void btnplus_Click(object sender, EventArgs e)
	{
		try
		{
			table.Rows.Add(0, CmbOrderNo.Value, ((Control)(object)CmbOrderNo).Text.Trim(), CmbSupplierCustomer.Value, ((Control)(object)CmbSupplierCustomer).Text.Trim(), CmbItemName.Value, ((Control)(object)CmbItemName).Text.Trim(), ((Control)(object)cmbCropYear).Text.Trim(), CmbPackUom.Value, ((Control)(object)CmbPackUom).Text.Trim(), Conversion.ToDouble((object)txtqty.Text.Trim()), Conversion.ToDouble((object)txtweight.Text.Trim()), Conversion.ToDouble((object)txtEBWtUnit.Text), Conversion.ToDouble((object)txtEBWtTotal.Text), Conversion.ToDouble((object)txtGrossWeight.Text.Trim()), Conversion.ToDouble((object)txtMoisture.Text.Trim()), txtRemarksDetail.Text.Trim());
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			ResetDetail();
			CmbOrderNo.Focus();
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
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Expected O, but got Unknown
		//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fc: Expected O, but got Unknown
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
			InvSupplyOrder so = new InvSupplyOrder();
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				so.Id = Id;
				so.ModifyUser = UserAccount.ID;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			so.BranchesId = Conversion.ToInt(cmbbranch.Value);
			so.ProjectsId = Conversion.ToInt(cmbproject.Value);
			so.DocNo = Conversion.ToInt((object)txtdocno.Text);
			so.DocDate = Conversion.ToDateTime((object)DocDate.Value);
			so.ManualRefNo = Conversion.ToString((object)txtManualRefNo.Text);
			so.VehicleNos = Conversion.ToString((object)txtVehicleNo.Text);
			so.BiltyNos = Conversion.ToString((object)txtBiltyNo.Text);
			so.DestinationsLocation = Conversion.ToString((object)txtDestinationLocation.Text);
			so.RemarksHeader = Conversion.ToString((object)txtRemarks.Text);
			so.DocumentTypeId = 104;
			so.OrganizationId = UserAccount.OrganizationId;
			so.CompanyId = UserAccount.CompanyId;
			so.EntryUser = UserAccount.ID;
			so.EntryDate = DateTime.Now;
			so.ModifyUser = UserAccount.ID;
			so.ModifyDate = DateTime.Now;
			so.InvSupplyOrderDetailList = new List<InvSupplyOrderDetail>();
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				InvSupplyOrderDetail sd = new InvSupplyOrderDetail();
				sd.PoDetailId = Conversion.ToInt(r.Cells["PoDetailId"].Value);
				if (Conversion.ToInt(r.Cells["PoHeaderId"].Value) == 0 || r.Cells["PoHeaderId"].Value == null)
				{
					MessageBox.Show("Please Check Order #");
					return;
				}
				sd.PoHeaderId = Conversion.ToInt(r.Cells["PoHeaderId"].Value);
				sd.OrderNo = Conversion.ToInt(r.Cells["OrderNo"].Value);
				if (Conversion.ToInt(r.Cells["SupplierCustomerId"].Value) == 0 || r.Cells["SupplierCustomerId"].Value == null)
				{
					MessageBox.Show("Plese Check Supplier");
					return;
				}
				sd.SupplierCustomerId = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
				if (Conversion.ToInt(r.Cells["ItemId"].Value) == 0 || r.Cells["ItemId"].Value == null)
				{
					MessageBox.Show("Plese Check Item");
					return;
				}
				sd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				if (Conversion.ToString((object)r.Cells["CropYear"].Text) == string.Empty)
				{
					MessageBox.Show("Plese Check CropYear");
					return;
				}
				sd.CropYear = Conversion.ToString(r.Cells["CropYear"].Value);
				if (Conversion.ToInt(r.Cells["PackSizeId"].Value) == 0 || r.Cells["PackSizeId"].Value == null)
				{
					MessageBox.Show("Plese Check Pack Size");
					return;
				}
				sd.PackSizeId = Conversion.ToInt(r.Cells["PackSizeId"].Value);
				if (Conversion.ToInt(r.Cells["Qty"].Value) == 0 || r.Cells["Qty"].Value == null)
				{
					MessageBox.Show("Plese Check Qty");
					return;
				}
				sd.Qty = Conversion.ToDouble(r.Cells["Qty"].Value);
				if (Conversion.ToInt(r.Cells["NetWeight"].Value) == 0 || r.Cells["NetWeight"].Value == null)
				{
					MessageBox.Show("Plese Check NetWeight");
					return;
				}
				sd.NetWeight = Conversion.ToDouble(r.Cells["NetWeight"].Value);
				sd.EbWtUnit = Conversion.ToDouble(r.Cells["EbWtUnit"].Value);
				sd.EbWtTotal = Conversion.ToDouble(r.Cells["EbWtTotal"].Value);
				if (Conversion.ToInt(r.Cells["GrossWeight"].Value) == 0 || r.Cells["GrossWeight"].Value == null)
				{
					MessageBox.Show("Plese Check GrossWeight");
					return;
				}
				sd.GrossWeight = Conversion.ToDouble(r.Cells["GrossWeight"].Value);
				sd.Moisture = Conversion.ToDouble(r.Cells["Moisture"].Value);
				sd.RemarksDetail = Conversion.ToString(r.Cells["RemarksDetail"].Value);
				so.InvSupplyOrderDetailList.Add(sd);
			}
			so.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Attachment.dtAttachments.Rows[i][1].ToString();
				attach.ScreenName = base.Name;
				so.AttachmentsList.Add(attach);
			}
			InvSupplyOrder.Save(so);
			if (Id > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + so.DocNo);
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + so.DocNo);
			}
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
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

	private void btnupdate_Click(object sender, EventArgs e)
	{
		try
		{
			Insert();
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
			DataTable BranchList = CommonServices.BrancheServiceBind();
			if (BranchList.Rows.Count > 0)
			{
				DDL.BindDDLNew(BranchList, cmbbranch, "Id", "BranchName", "Branch Name", false);
				((UltraGridBase)cmbbranch).Rows[0].Activate();
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
				DDL.BindDDLNew(dt, cmbproject, "Id", "ProjectName", "Project Name", false);
				((UltraGridBase)cmbproject).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OrderNoFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtOrderNo = InvSupplyOrder.ReadPurchaseOrderForSupplyOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtOrderNo.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtOrderNo, CmbOrderNo, "PoHeaderId", "OrderNo", "WareHouseName", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierFill()
	{
		try
		{
			DataTable dt = CommonServices.SupplierCustomerGetAllServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbSupplierCustomer, "Id", "CompanyName", "Supplier", ZeroIndex: false);
			}
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
			DataTable dtItem = CommonServices.ItemGetForComboServiceBind();
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDL(dtItem, CmbItemName, "Id", "ItemName", "Item", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CropYearFill()
	{
		try
		{
			DataTable dtCropYear = CommonServices.CropYearGetAllService();
			if (dtCropYear.Rows.Count > 0)
			{
				DDL.BindDDL(dtCropYear, cmbCropYear, "Id", "CropYear", "CropYear", ZeroIndex: false);
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
		//IL_0034: Expected O, but got Unknown
		try
		{
			int code = InvSupplyOrder.GenerateCode(new InvSupplyOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 104
			});
			if (code > 0)
			{
				txtdocno.Text = code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindRateUomAndItemPackUom(int ID)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		try
		{
			DataTable dtuomsch = UOMSchedule.SearchByObject(new UOMSchedule
			{
				ItemId = Conversion.ToInt((object)ID),
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtuomsch.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtuomsch, CmbPackUom, "Id", "Equivalent", "Item Pack Unit", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
		ResetDetail();
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void Reset()
	{
		try
		{
			Id = 0;
			IsApproved = false;
			((UltraGridBase)cmbbranch).Rows[0].Activate();
			((UltraGridBase)cmbproject).Rows[0].Activate();
			DocDate.Focus();
			((Control)(object)CmbItemName).Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			txtqty.Text = string.Empty;
			txtweight.Text = string.Empty;
			((Control)(object)CmbOrderNo).Text = string.Empty;
			((Control)(object)cmbCropYear).Text = string.Empty;
			txtVehicleNo.Text = string.Empty;
			txtManualRefNo.Text = string.Empty;
			txtBiltyNo.Text = string.Empty;
			txtDestinationLocation.Text = string.Empty;
			txtRemarks.Text = string.Empty;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			DocumentNoFill();
			table.Rows.Clear();
			grd.DataSource = table;
			CmbOrderNo.ReadOnly = false;
			CmbSupplierCustomer.ReadOnly = false;
			CmbItemName.ReadOnly = false;
			cmbCropYear.ReadOnly = false;
			PendingPurchaseOrder();
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
			((Control)(object)CmbItemName).Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			((UltraGridBase)CmbPackUom).DataSource = null;
			txtqty.Text = string.Empty;
			txtweight.Text = string.Empty;
			((Control)(object)CmbOrderNo).Text = string.Empty;
			((Control)(object)cmbCropYear).Text = string.Empty;
			txtRemarksDetail.Text = string.Empty;
			txtEBWtTotal.Text = string.Empty;
			txtEBWtUnit.Text = string.Empty;
			txtEBWtTotal.Text = string.Empty;
			((Control)(object)CmbSupplierCustomer).Text = string.Empty;
			txtGrossWeight.Text = string.Empty;
			txtMoisture.Text = string.Empty;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void grdSettings()
	{
		try
		{
			grd.RootTable.Columns["OrderNo"].Width = 40;
			grd.RootTable.Columns["CropYear"].Width = 40;
			grd.RootTable.Columns["PackSize"].Width = 40;
			grd.RootTable.Columns["Moisture"].Width = 40;
			grd.RootTable.Columns["Qty"].Width = 40;
			grd.RootTable.Columns["NetWeight"].Width = 40;
			grd.RootTable.Columns["EbWtUnit"].Width = 40;
			grd.RootTable.Columns["EbWtTotal"].Width = 40;
			grd.RootTable.Columns["GrossWeight"].Width = 40;
			grd.RootTable.Columns["PoDetailId"].Visible = false;
			grd.RootTable.Columns["PoHeaderId"].Visible = false;
			grd.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["PackSizeId"].Visible = false;
			grd.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Qty"].FormatString = "0,0";
			grd.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetWeight"].FormatString = "0,0";
			grd.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["EbWtUnit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["EbWtUnit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["EbWtUnit"].FormatString = "#,#.##";
			grd.RootTable.Columns["EbWtUnit"].TotalFormatString = "#,#.##";
			grd.RootTable.Columns["EbWtTotal"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["EbWtTotal"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["EbWtTotal"].FormatString = "#,#.##";
			grd.RootTable.Columns["EbWtTotal"].TotalFormatString = "#,#.##";
			grd.RootTable.Columns["GrossWeight"].FormatString = "#,#";
			grd.RootTable.Columns["GrossWeight"].TotalFormatString = "#,#";
			grd.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "Delete";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "Delete";
			grd.RootTable.Columns["Delete"].Width = 20;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PurchsaeOrder_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.KeyCode == Keys.Delete && tabControl1.SelectedIndex == 1)
			{
				GridEXRow[] checkedRows = grdhistory.GetCheckedRows();
				for (int i = 0; i < checkedRows.Length; i++)
				{
					int.Parse(checkedRows[i].Cells["Id"].Value.ToString());
				}
			}
			if (btnsave.Visible && btnsave.Enabled && e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N && tabControl1.SelectedIndex == 0)
			{
				btnnew_Click(null, null);
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
			if (btnupdate.Visible && btnupdate.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				btnupdate_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_DoubleClick_1(object sender, EventArgs e)
	{
		try
		{
			Id = Conversion.ToInt((object)grdhistory.CurrentRow.Cells[1].Value.ToString());
			ReadById(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int ID)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			InvSupplyOrder so = new InvSupplyOrder();
			so = InvSupplyOrder.GetByID(ID);
			Id = ID;
			so.Id = Id;
			tabControl1.SelectedIndex = 0;
			cmbbranch.Value = Conversion.ToInt((object)so.BranchesId);
			cmbproject.Value = Conversion.ToInt((object)so.ProjectsId);
			DocDate.Value = Conversion.ToDateTime((object)so.DocDate);
			txtdocno.Text = Conversion.ToString((object)so.DocNo);
			cmbbranch.Value = Conversion.ToInt((object)so.BranchesId);
			cmbproject.Value = Conversion.ToInt((object)so.ProjectsId);
			txtdocno.Text = Conversion.ToString((object)so.DocNo);
			DocDate.Value = Conversion.ToDateTime((object)so.DocDate);
			txtVehicleNo.Text = Conversion.ToString((object)so.VehicleNos);
			txtManualRefNo.Text = Conversion.ToString((object)so.ManualRefNo);
			txtBiltyNo.Text = Conversion.ToString((object)so.BiltyNos);
			txtDestinationLocation.Text = Conversion.ToString((object)so.DestinationsLocation);
			txtRemarks.Text = Conversion.ToString((object)so.RemarksHeader);
			table.Clear();
			for (int i = 0; i < so.InvSupplyOrderDetailList.Count; i++)
			{
				table.Rows.Add(so.InvSupplyOrderDetailList[i].PoDetailId, so.InvSupplyOrderDetailList[i].PoHeaderId, so.InvSupplyOrderDetailList[i].OrderNo, so.InvSupplyOrderDetailList[i].SupplierCustomerId, so.InvSupplyOrderDetailList[i].SupplierName, so.InvSupplyOrderDetailList[i].ItemId, so.InvSupplyOrderDetailList[i].ItemName, so.InvSupplyOrderDetailList[i].CropYear, so.InvSupplyOrderDetailList[i].PackSizeId, so.InvSupplyOrderDetailList[i].PackSize, so.InvSupplyOrderDetailList[i].Qty, so.InvSupplyOrderDetailList[i].NetWeight, so.InvSupplyOrderDetailList[i].EbWtUnit, so.InvSupplyOrderDetailList[i].EbWtTotal, so.InvSupplyOrderDetailList[i].GrossWeight, so.InvSupplyOrderDetailList[i].Moisture, so.InvSupplyOrderDetailList[i].RemarksDetail);
			}
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			List<DMSAttachments> byID = global::Architecture.BLL.DMSAttachments.GetByID(int.Parse(Id.ToString()), base.Name);
			AT = new Attachment();
			foreach (global::Architecture.Model.DMSAttachments Attachment in byID)
			{
				TempAttachments TA = new TempAttachments();
				TA.Id = 0;
				TA.Path = Attachment.Attachment;
				AT.lst.Add(TA);
			}
			bindRateUomAndItemPackUom(Conversion.ToInt(CmbItemName.Value));
			btnsave.Visible = false;
			btnupdate.Visible = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridhistoryfill()
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
			DataTable dthistoryfill = new DataTable();
			dthistoryfill = InvSupplyOrder.SupplyOrderFormHistory(val);
			if (dthistoryfill.Rows.Count > 0)
			{
				grdhistory.DataSource = dthistoryfill;
				grdhistory.RetrieveStructure();
				HistoryGridSettings();
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
			grdhistory.RootTable.Columns["Id"].Visible = false;
			grdhistory.RootTable.Columns["IsApproved"].Visible = false;
			grdhistory.RootTable.Columns["EntryDate"].Visible = false;
			grdhistory.RootTable.Columns["EntryUser"].Visible = false;
			grdhistory.RootTable.Columns["ModifyDate"].Visible = false;
			grdhistory.RootTable.Columns["ModifyUser"].Visible = false;
			grdhistory.RootTable.Columns["PostDate"].Visible = false;
			grdhistory.RootTable.Columns["PostUser"].Visible = false;
			grdhistory.RootTable.Columns.Add("Print");
			grdhistory.RootTable.Columns["Print"].Caption = "Print";
			grdhistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Print"].ButtonText = "Print";
			grdhistory.RootTable.Columns["Print"].Width = 50;
			grdhistory.RootTable.Columns.Add("Edit");
			grdhistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdhistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdhistory.RootTable.Columns["Edit"].Width = 50;
			grdhistory.RootTable.Columns.Add("Detail");
			grdhistory.RootTable.Columns["Detail"].Caption = "Detail";
			grdhistory.RootTable.Columns["Detail"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Detail"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Detail"].ButtonText = "Detail";
			grdhistory.RootTable.Columns["Detail"].Width = 50;
			ctrlGrdBar2_Load(null, null);
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

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdhistory).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdhistory).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
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

	private void changegrossweight()
	{
		if (Conversion.ToDouble((object)txtweight.Text) > 0.0)
		{
			if (Conversion.ToDouble((object)txtEBWtTotal.Text) > 0.0)
			{
				double GrossWeight = Conversion.ToDouble((object)txtweight.Text) + Conversion.ToDouble((object)txtEBWtTotal.Text);
				txtGrossWeight.Text = Conversion.ToString((object)GrossWeight);
			}
			else
			{
				txtGrossWeight.Text = Conversion.ToString((object)txtweight.Text);
			}
		}
	}

	private void calculateTotalQuantity()
	{
		try
		{
			if (Conversion.ToDouble((object)txtqty.Text) > 0.0 && Conversion.ToInt((object)((Control)(object)CmbPackUom).Text) > 0)
			{
				double netWeight = Conversion.ToDouble((object)txtqty.Text) * (double)Conversion.ToInt((object)((Control)(object)CmbPackUom).Text);
				txtweight.Text = Conversion.ToString((object)netWeight);
				changegrossweight();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void handleEbUnitChange()
	{
		try
		{
			if (Conversion.ToDouble((object)txtqty.Text) > 0.0)
			{
				double EbWtTotal = Conversion.ToDouble((object)txtqty.Text) * Conversion.ToDouble((object)txtEBWtUnit.Text);
				txtEBWtTotal.Text = Conversion.ToString((object)EbWtTotal);
				changegrossweight();
			}
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
				CmbItemName_TextChanged(null, null);
				CmbPackUom_TextChanged(null, null);
				CmbOrderNo.Value = item.Cells["PoHeaderId"].Value.ToString();
				((Control)(object)CmbOrderNo).Text = item.Cells["OrderNo"].Text.ToString();
				CmbSupplierCustomer.Value = Conversion.ToInt(item.Cells["SupplierCustomerId"].Value);
				CmbItemName.Value = item.Cells["ItemId"].Value.ToString();
				((Control)(object)cmbCropYear).Text = item.Cells["CropYear"].Text.ToString();
				CmbPackUom.Value = item.Cells["PackSizeId"].Value.ToString();
				txtqty.Text = item.Cells["Qty"].Text.ToString();
				txtweight.Text = item.Cells["NetWeight"].Value.ToString();
				txtEBWtUnit.Text = item.Cells["EbWtUnit"].Value.ToString();
				txtEBWtTotal.Text = item.Cells["EbWtTotal"].Value.ToString();
				txtGrossWeight.Text = item.Cells["GrossWeight"].Value.ToString();
				txtMoisture.Text = item.Cells["Moisture"].Value.ToString();
				txtRemarksDetail.Text = item.Cells["RemarksDetail"].Value.ToString();
				btnplus.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				CmbOrderNo.ReadOnly = true;
				CmbSupplierCustomer.ReadOnly = true;
				CmbItemName.ReadOnly = true;
				cmbCropYear.ReadOnly = true;
				CmbPackUom.Focus();
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
			table.Rows[updateDetailIndex]["PoDetailId"] = grd.CurrentRow.Cells["PoDetailId"].Value;
			table.Rows[updateDetailIndex]["PoHeaderId"] = CmbOrderNo.Value;
			table.Rows[updateDetailIndex]["OrderNo"] = ((Control)(object)CmbOrderNo).Text;
			table.Rows[updateDetailIndex]["SupplierCustomerId"] = CmbSupplierCustomer.Value;
			table.Rows[updateDetailIndex]["SupplierName"] = ((Control)(object)CmbSupplierCustomer).Text;
			table.Rows[updateDetailIndex]["ItemId"] = CmbItemName.Value;
			table.Rows[updateDetailIndex]["ItemName"] = ((Control)(object)CmbItemName).Text;
			table.Rows[updateDetailIndex]["CropYear"] = ((Control)(object)cmbCropYear).Text.Trim();
			table.Rows[updateDetailIndex]["PackSizeId"] = CmbPackUom.Value;
			table.Rows[updateDetailIndex]["PackSize"] = ((Control)(object)CmbPackUom).Text;
			table.Rows[updateDetailIndex]["Qty"] = txtqty.Text;
			table.Rows[updateDetailIndex]["NetWeight"] = txtweight.Text;
			table.Rows[updateDetailIndex]["EbWtUnit"] = txtEBWtUnit.Text;
			table.Rows[updateDetailIndex]["EbWtTotal"] = txtEBWtTotal.Text;
			table.Rows[updateDetailIndex]["GrossWeight"] = txtGrossWeight.Text;
			table.Rows[updateDetailIndex]["Moisture"] = txtMoisture.Text;
			table.Rows[updateDetailIndex]["RemarksDetail"] = txtRemarksDetail.Text;
			grd.DataSource = table;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			ResetDetail();
			CmbPackUom.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combitem_ValueChanged(object sender, EventArgs e)
	{
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		ResetDetail();
	}

	private void btnattachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			gridhistoryfill();
		}
	}

	private void txtdocno_TextChanged(object sender, EventArgs e)
	{
	}

	private void btnprint_Click(object sender, EventArgs e)
	{
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			BranchFill();
			ProjectFill();
			bindRateUomAndItemPackUom(Conversion.ToInt(CmbItemName.Value));
			SupplierFill();
			ItemFill();
			OrderNoFill();
			CropYearFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadAll_Click(object sender, EventArgs e)
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
			DataTable dthistoryfill = new DataTable();
			dthistoryfill = InvSupplyOrder.SupplyOrderFormHistory(val);
			if (dthistoryfill.Rows.Count > 0)
			{
				grdhistory.DataSource = dthistoryfill;
				grdhistory.RetrieveStructure();
				HistoryGridSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdhistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			List<global::Architecture.Model.DMSAttachments> dMSList = global::Architecture.BLL.DMSAttachments.GetByID(int.Parse(item.Cells["Id"].Value.ToString().ToString()), base.Name);
			dtViewAttachments.Clear();
			if (dMSList.Count > 0)
			{
				foreach (global::Architecture.Model.DMSAttachments itemList in dMSList)
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

	private void txtTotalQty_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtPackingWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			handleEbUnitChange();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtqty_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtPackingWeight_KeyPress(object sender, KeyPressEventArgs e)
	{
	}

	private void grdhistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Edit")
			{
				Id = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(Id);
			}
			if (e.Column.Key == "Print")
			{
				int PrintId = 0;
				PrintId = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString());
				DataTable dt = InvSupplyOrder.SupplyOrderSlipandRegister(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = PrintId
				});
				if (dt.Rows.Count == 0 || dt == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "330-InvSupplyOrderSlip.rpt");
			}
			if (e.Column.Key == "Detail")
			{
				DetaliGridFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbItemName_TextChanged(object sender, EventArgs e)
	{
		try
		{
			bindRateUomAndItemPackUom(Conversion.ToInt(CmbItemName.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbOrderNo_TextChanged(object sender, EventArgs e)
	{
	}

	private void CmbPackUom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			calculateTotalQuantity();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PendingPurchaseOrder()
	{
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("PoDetailId");
			dt.Columns.Add("PoHeaderId");
			dt.Columns.Add("OrderNo");
			dt.Columns.Add("SupplierCustomerId");
			dt.Columns.Add("SupplierName");
			dt.Columns.Add("ItemId");
			dt.Columns.Add("ItemName");
			dt.Columns.Add("CropYear");
			dt.Columns.Add("PackSizeId");
			dt.Columns.Add("PackSize");
			dt.Columns.Add("Qty", typeof(double));
			dt.Columns.Add("NetWeight", typeof(double));
			dt.Columns.Add("EbWtUnit", typeof(double));
			dt.Columns.Add("EbWtTotal", typeof(double));
			dt.Columns.Add("GrossWeight", typeof(double));
			dt.Columns.Add("Moisture");
			dt.Columns.Add("RemarksDetail");
			DataTable dtPending = InvSupplyOrder.ReadPurchaseOrderForSupplyOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtPending.Rows.Count > 0)
			{
				for (int i = 0; i < dtPending.Rows.Count; i++)
				{
					dt.Rows.Add(dtPending.Rows[i]["PoDetailId"], dtPending.Rows[i]["PoHeaderId"], dtPending.Rows[i]["OrderNo"], dtPending.Rows[i]["SupplierCustomerId"], dtPending.Rows[i]["SupplierName"], dtPending.Rows[i]["ItemId"], dtPending.Rows[i]["ItemName"], dtPending.Rows[i]["CropYear"], dtPending.Rows[i]["PackSizeId"], dtPending.Rows[i]["PackSize"], dtPending.Rows[i]["Qty"], dtPending.Rows[i]["NetWeight"], dtPending.Rows[i]["EbWtUnit"], dtPending.Rows[i]["EbWtTotal"], dtPending.Rows[i]["GrossWeight"], dtPending.Rows[i]["Moisture"], dtPending.Rows[i]["OrderRemarks"]);
				}
				grdPendingSupplyOrder.DataSource = dt;
				grdPendingSupplyOrder.RetrieveStructure();
				grdPendingSetting();
			}
			else
			{
				grdPendingSupplyOrder.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingSetting()
	{
		try
		{
			grdPendingSupplyOrder.RootTable.Columns["PoDetailId"].Visible = false;
			grdPendingSupplyOrder.RootTable.Columns["PoHeaderId"].Visible = false;
			grdPendingSupplyOrder.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grdPendingSupplyOrder.RootTable.Columns["ItemId"].Visible = false;
			grdPendingSupplyOrder.RootTable.Columns["PackSizeId"].Visible = false;
			grdPendingSupplyOrder.RootTable.Columns.Add("Select");
			grdPendingSupplyOrder.RootTable.Columns["Select"].Key = "Select";
			grdPendingSupplyOrder.RootTable.Columns["Select"].ActAsSelector = true;
			grdPendingSupplyOrder.RootTable.Columns["Select"].UseHeaderSelector = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void abc()
	{
		try
		{
			GridEXRow[] checkedRows = grdPendingSupplyOrder.GetCheckedRows();
			foreach (GridEXRow loaderGridRow in checkedRows)
			{
				bool flag = false;
				GridEXRow[] dataRows = grd.GetDataRows();
				foreach (GridEXRow detailGridRow in dataRows)
				{
					if (loaderGridRow.Cells["PoDetailId"].Value.ToString() == detailGridRow.Cells["PoDetailId"].Value.ToString())
					{
						flag = true;
					}
				}
				if (!flag && !flag)
				{
					table.Rows.Add(loaderGridRow.Cells["PoDetailId"].Value, loaderGridRow.Cells["PoHeaderId"].Value, loaderGridRow.Cells["OrderNo"].Value, loaderGridRow.Cells["SupplierCustomerId"].Value, loaderGridRow.Cells["SupplierName"].Value, loaderGridRow.Cells["ItemId"].Value, loaderGridRow.Cells["ItemName"].Value, loaderGridRow.Cells["CropYear"].Value, loaderGridRow.Cells["PackSizeId"].Value, loaderGridRow.Cells["PackSize"].Value, loaderGridRow.Cells["Qty"].Value, loaderGridRow.Cells["NetWeight"].Value, loaderGridRow.Cells["EbWtUnit"].Value, loaderGridRow.Cells["EbWtTotal"].Value, loaderGridRow.Cells["GrossWeight"].Value, loaderGridRow.Cells["Moisture"].Value, loaderGridRow.Cells["RemarksDetail"].Value);
					grd.DataSource = table;
					grd.RetrieveStructure();
					grdSettings();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoader_Click(object sender, EventArgs e)
	{
		abc();
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete" && MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				GridEXRow item = grd.CurrentRow;
				table.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetaliGridFill()
	{
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Order #");
			dt.Columns.Add("Supplier");
			dt.Columns.Add("Item");
			dt.Columns.Add("CropYear");
			dt.Columns.Add("PackSize");
			dt.Columns.Add("Qty", typeof(double));
			dt.Columns.Add("Weight", typeof(double));
			dt.Columns.Add("EBWtUnit", typeof(double));
			dt.Columns.Add("EBWtTotal", typeof(double));
			dt.Columns.Add("GrossWeight", typeof(double));
			dt.Columns.Add("Moisture");
			dt.Columns.Add("Remarks");
			int num = Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value);
			InvSupplyOrder obj = new InvSupplyOrder();
			obj = InvSupplyOrder.GetByID(num);
			if (obj.InvSupplyOrderDetailList.Count > 0)
			{
				for (int i = 0; i < obj.InvSupplyOrderDetailList.Count; i++)
				{
					dt.Rows.Add(obj.InvSupplyOrderDetailList[i].OrderNo, obj.InvSupplyOrderDetailList[i].SupplierName, obj.InvSupplyOrderDetailList[i].ItemName, obj.InvSupplyOrderDetailList[i].CropYear, obj.InvSupplyOrderDetailList[i].PackSize, obj.InvSupplyOrderDetailList[i].Qty, obj.InvSupplyOrderDetailList[i].NetWeight, obj.InvSupplyOrderDetailList[i].EbWtUnit, obj.InvSupplyOrderDetailList[i].EbWtTotal, obj.InvSupplyOrderDetailList[i].GrossWeight, obj.InvSupplyOrderDetailList[i].Moisture, obj.InvSupplyOrderDetailList[i].RemarksDetail);
				}
				grdDetail.DataSource = dt;
				grdDetail.RetrieveStructure();
				GridDetailSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailSetting()
	{
		try
		{
			grdDetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Qty"].FormatString = "0,0";
			grdDetail.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Weight"].FormatString = "0,0";
			grdDetail.RootTable.Columns["Weight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["EBWtUnit"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["EBWtUnit"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["EBWtUnit"].FormatString = "0,0";
			grdDetail.RootTable.Columns["EBWtUnit"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["EBWtTotal"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["EBWtTotal"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["EBWtTotal"].FormatString = "0,0";
			grdDetail.RootTable.Columns["EBWtTotal"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["GrossWeight"].FormatString = "0,0";
			grdDetail.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.##";
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
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Expected O, but got Unknown
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Expected O, but got Unknown
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Expected O, but got Unknown
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Expected O, but got Unknown
		//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Expected O, but got Unknown
		//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Expected O, but got Unknown
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e0: Expected O, but got Unknown
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Expected O, but got Unknown
		//IL_0470: Unknown result type (might be due to invalid IL or missing references)
		//IL_047a: Expected O, but got Unknown
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Expected O, but got Unknown
		//IL_0583: Unknown result type (might be due to invalid IL or missing references)
		//IL_058d: Expected O, but got Unknown
		//IL_0599: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a3: Expected O, but got Unknown
		//IL_0c05: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c0f: Expected O, but got Unknown
		//IL_4d33: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d3d: Expected O, but got Unknown
		//IL_4d4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d54: Expected O, but got Unknown
		GridEXLayout grdPendingSupplyOrder_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance134 = new Appearance();
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
		Appearance appearance145 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
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
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Purchase.SupplyOrderPurchase));
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdPendingSupplyOrder = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.txtDestinationLocation = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.txtBiltyNo = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.btnLoader = new System.Windows.Forms.Button();
		this.txtMoisture = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.txtEBWtTotal = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.txtEBWtUnit = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.CmbSupplierCustomer = new UltraCombo();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.label2 = new System.Windows.Forms.Label();
		this.CmbPackUom = new UltraCombo();
		this.label36 = new System.Windows.Forms.Label();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.cmbCropYear = new UltraCombo();
		this.CmbOrderNo = new UltraCombo();
		this.CmbItemName = new UltraCombo();
		this.btnplus = new System.Windows.Forms.Button();
		this.txtRemarksDetail = new System.Windows.Forms.TextBox();
		this.label47 = new System.Windows.Forms.Label();
		this.txtweight = new System.Windows.Forms.TextBox();
		this.txtqty = new System.Windows.Forms.TextBox();
		this.label35 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.txtVehicleNo = new System.Windows.Forms.TextBox();
		this.txtManualRefNo = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.cmbproject = new UltraCombo();
		this.cmbbranch = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.label33 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.btnclose = new System.Windows.Forms.Button();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.grdhistory = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPendingSupplyOrder).BeginInit();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel4.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierCustomer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbproject).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbranch).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel8.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1155, 724);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1155, 724);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(1147, 698);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdPendingSupplyOrder);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(2, 474);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1143, 222);
		this.panel5.TabIndex = 5;
		this.grdPendingSupplyOrder.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPendingSupplyOrder).BackColor = System.Drawing.Color.White;
		this.grdPendingSupplyOrder.ColumnAutoResize = true;
		grdPendingSupplyOrder_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPendingSupplyOrder.DesignTimeLayout = grdPendingSupplyOrder_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPendingSupplyOrder).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPendingSupplyOrder.FilterMode = (FilterMode)2;
		((System.Windows.Forms.Control)(object)this.grdPendingSupplyOrder).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPendingSupplyOrder.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPendingSupplyOrder).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdPendingSupplyOrder).Name = "grdPendingSupplyOrder";
		((ScrollableBase)this.grdPendingSupplyOrder).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPendingSupplyOrder).Size = new System.Drawing.Size(1143, 222);
		((System.Windows.Forms.Control)(object)this.grdPendingSupplyOrder).TabIndex = 1;
		this.grdPendingSupplyOrder.TotalRow = (InheritableBoolean)1;
		this.grdPendingSupplyOrder.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPendingSupplyOrder).VisualStyle = (VisualStyle)4;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(2, 303);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1143, 171);
		this.panel6.TabIndex = 4;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		this.grd.ColumnAutoResize = true;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1143, 171);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.txtRemarks);
		this.panel4.Controls.Add(this.label12);
		this.panel4.Controls.Add(this.txtDestinationLocation);
		this.panel4.Controls.Add(this.label11);
		this.panel4.Controls.Add(this.txtBiltyNo);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add(this.groupBox3);
		this.panel4.Controls.Add(this.txtVehicleNo);
		this.panel4.Controls.Add(this.txtManualRefNo);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbproject);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbbranch);
		this.panel4.Controls.Add(this.label13);
		this.panel4.Controls.Add(this.label18);
		this.panel4.Controls.Add(this.DocDate);
		this.panel4.Controls.Add(this.label33);
		this.panel4.Controls.Add(this.label17);
		this.panel4.Controls.Add(this.txtdocno);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 68);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1143, 235);
		this.panel4.TabIndex = 2;
		this.txtRemarks.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(80, 69);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(1047, 23);
		this.txtRemarks.TabIndex = 8;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(12, 73);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(54, 15);
		this.label12.TabIndex = 34;
		this.label12.Text = "Remarks";
		this.txtDestinationLocation.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDestinationLocation.Location = new System.Drawing.Point(693, 42);
		this.txtDestinationLocation.Name = "txtDestinationLocation";
		this.txtDestinationLocation.Size = new System.Drawing.Size(434, 23);
		this.txtDestinationLocation.TabIndex = 7;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(569, 46);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(118, 15);
		this.label11.TabIndex = 32;
		this.label11.Text = "Destination Location";
		this.txtBiltyNo.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBiltyNo.Location = new System.Drawing.Point(352, 42);
		this.txtBiltyNo.Name = "txtBiltyNo";
		this.txtBiltyNo.Size = new System.Drawing.Size(211, 23);
		this.txtBiltyNo.TabIndex = 6;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(299, 46);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(50, 15);
		this.label4.TabIndex = 30;
		this.label4.Text = "Bilty No";
		this.groupBox3.BackColor = System.Drawing.Color.White;
		this.groupBox3.Controls.Add(this.btnLoader);
		this.groupBox3.Controls.Add(this.txtMoisture);
		this.groupBox3.Controls.Add(this.label8);
		this.groupBox3.Controls.Add(this.txtGrossWeight);
		this.groupBox3.Controls.Add(this.label3);
		this.groupBox3.Controls.Add(this.label14);
		this.groupBox3.Controls.Add(this.txtEBWtTotal);
		this.groupBox3.Controls.Add(this.label7);
		this.groupBox3.Controls.Add(this.txtEBWtUnit);
		this.groupBox3.Controls.Add(this.label6);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer);
		this.groupBox3.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox3.Controls.Add(this.label2);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUom);
		this.groupBox3.Controls.Add(this.label36);
		this.groupBox3.Controls.Add(this.btnUpdateDetail);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCropYear);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOrderNo);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.groupBox3.Controls.Add(this.btnplus);
		this.groupBox3.Controls.Add(this.txtRemarksDetail);
		this.groupBox3.Controls.Add(this.label47);
		this.groupBox3.Controls.Add(this.txtweight);
		this.groupBox3.Controls.Add(this.txtqty);
		this.groupBox3.Controls.Add(this.label35);
		this.groupBox3.Controls.Add(this.label39);
		this.groupBox3.Controls.Add(this.label38);
		this.groupBox3.Controls.Add(this.label37);
		this.groupBox3.Font = new System.Drawing.Font("Segoe UI", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(6, 98);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1121, 133);
		this.groupBox3.TabIndex = 28;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Detail";
		this.btnLoader.BackColor = System.Drawing.Color.White;
		this.btnLoader.FlatAppearance.BorderSize = 0;
		this.btnLoader.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoader.Location = new System.Drawing.Point(1018, 76);
		this.btnLoader.Name = "btnLoader";
		this.btnLoader.Size = new System.Drawing.Size(97, 45);
		this.btnLoader.TabIndex = 49;
		this.btnLoader.Text = "Load";
		this.btnLoader.UseVisualStyleBackColor = false;
		this.btnLoader.Click += new System.EventHandler(btnLoader_Click);
		this.txtMoisture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMoisture.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMoisture.Location = new System.Drawing.Point(335, 96);
		this.txtMoisture.Name = "txtMoisture";
		this.txtMoisture.Size = new System.Drawing.Size(112, 25);
		this.txtMoisture.TabIndex = 10;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(335, 76);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(54, 15);
		this.label8.TabIndex = 48;
		this.label8.Text = "Moisture";
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Enabled = false;
		this.txtGrossWeight.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrossWeight.Location = new System.Drawing.Point(215, 96);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.Size = new System.Drawing.Size(118, 25);
		this.txtGrossWeight.TabIndex = 9;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(215, 76);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(77, 15);
		this.label3.TabIndex = 46;
		this.label3.Text = "Gross Weight";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(129, 21);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(50, 15);
		this.label14.TabIndex = 44;
		this.label14.Text = "Supplier";
		this.txtEBWtTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEBWtTotal.Enabled = false;
		this.txtEBWtTotal.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEBWtTotal.Location = new System.Drawing.Point(107, 96);
		this.txtEBWtTotal.Name = "txtEBWtTotal";
		this.txtEBWtTotal.Size = new System.Drawing.Size(106, 25);
		this.txtEBWtTotal.TabIndex = 8;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(107, 76);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(66, 15);
		this.label7.TabIndex = 39;
		this.label7.Text = "EB Wt Total";
		this.txtEBWtUnit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEBWtUnit.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEBWtUnit.Location = new System.Drawing.Point(11, 96);
		this.txtEBWtUnit.Name = "txtEBWtUnit";
		this.txtEBWtUnit.Size = new System.Drawing.Size(94, 25);
		this.txtEBWtUnit.TabIndex = 7;
		this.txtEBWtUnit.TextChanged += new System.EventHandler(txtPackingWeight_TextChanged);
		this.txtEBWtUnit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPackingWeight_KeyPress);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(11, 76);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(63, 15);
		this.label6.TabIndex = 37;
		this.label6.Text = "EB Wt Unit";
		this.CmbSupplierCustomer.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplierCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplierCustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplierCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance135).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance135).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance136;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance137).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance137).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance141).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance142).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance142).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance142).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplierCustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Location = new System.Drawing.Point(132, 40);
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Name = "CmbSupplierCustomer";
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).TabIndex = 1;
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Segoe UI", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(957, 96);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(55, 26);
		this.btnCancelUpdateDetial.TabIndex = 14;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(8, 21);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(47, 15);
		this.label2.TabIndex = 31;
		this.label2.Text = "Order #";
		this.CmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance146).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance146).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance147;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance148).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance148).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance152).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance153).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance202).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance203).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Location = new System.Drawing.Point(733, 40);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Name = "CmbPackUom";
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Size = new System.Drawing.Size(101, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TextChanged += new System.EventHandler(CmbPackUom_TextChanged);
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(733, 21);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(55, 15);
		this.label36.TabIndex = 8;
		this.label36.Text = "Pack Size";
		this.btnUpdateDetail.BackColor = System.Drawing.Color.White;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Segoe UI", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.Location = new System.Drawing.Point(899, 96);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(56, 26);
		this.btnUpdateDetail.TabIndex = 13;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.cmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.cmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance123;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance128).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance131).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Location = new System.Drawing.Point(605, 40);
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Name = "cmbCropYear";
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Size = new System.Drawing.Size(124, 26);
		((System.Windows.Forms.Control)(object)this.cmbCropYear).TabIndex = 3;
		this.CmbOrderNo.AllowNull = (DefaultableBoolean)1;
		this.CmbOrderNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOrderNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOrderNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance190).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance191).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance191).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance191;
		((AppearanceBase)appearance192).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance192;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance193).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance193).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance193).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance194).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance195).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance196;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance197).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance198).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance198).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance198).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance198;
		((AppearanceBase)appearance199).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance200).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbOrderNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Location = new System.Drawing.Point(11, 40);
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Name = "CmbOrderNo";
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Size = new System.Drawing.Size(118, 26);
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).TextChanged += new System.EventHandler(CmbOrderNo_TextChanged);
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance155).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance155).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance156;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance169).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance169).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance169).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance172;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance173).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance174).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance174).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance174).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance174;
		((AppearanceBase)appearance175).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(368, 40);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).TextChanged += new System.EventHandler(CmbItemName_TextChanged);
		this.btnplus.BackColor = System.Drawing.Color.White;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.Location = new System.Drawing.Point(859, 96);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(39, 26);
		this.btnplus.TabIndex = 12;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Visible = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.txtRemarksDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksDetail.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDetail.Location = new System.Drawing.Point(450, 96);
		this.txtRemarksDetail.Name = "txtRemarksDetail";
		this.txtRemarksDetail.Size = new System.Drawing.Size(407, 25);
		this.txtRemarksDetail.TabIndex = 11;
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(450, 76);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(52, 15);
		this.label47.TabIndex = 25;
		this.label47.Text = "Remarks";
		this.txtweight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtweight.Enabled = false;
		this.txtweight.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtweight.Location = new System.Drawing.Point(922, 40);
		this.txtweight.Name = "txtweight";
		this.txtweight.Size = new System.Drawing.Size(90, 25);
		this.txtweight.TabIndex = 6;
		this.txtqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtqty.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtqty.Location = new System.Drawing.Point(837, 40);
		this.txtqty.Name = "txtqty";
		this.txtqty.Size = new System.Drawing.Size(82, 25);
		this.txtqty.TabIndex = 5;
		this.txtqty.TextChanged += new System.EventHandler(CmbPackUom_TextChanged);
		this.txtqty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(605, 21);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(58, 15);
		this.label35.TabIndex = 25;
		this.label35.Text = "Crop Year";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(922, 21);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(45, 15);
		this.label39.TabIndex = 15;
		this.label39.Text = "Weight";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(837, 21);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(26, 15);
		this.label38.TabIndex = 13;
		this.label38.Text = "Qty";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(368, 21);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(66, 15);
		this.label37.TabIndex = 9;
		this.label37.Text = "Item Name";
		this.txtVehicleNo.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVehicleNo.Location = new System.Drawing.Point(80, 42);
		this.txtVehicleNo.Name = "txtVehicleNo";
		this.txtVehicleNo.Size = new System.Drawing.Size(211, 23);
		this.txtVehicleNo.TabIndex = 5;
		this.txtManualRefNo.BackColor = System.Drawing.Color.White;
		this.txtManualRefNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtManualRefNo.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtManualRefNo.Location = new System.Drawing.Point(1036, 13);
		this.txtManualRefNo.Name = "txtManualRefNo";
		this.txtManualRefNo.Size = new System.Drawing.Size(91, 23);
		this.txtManualRefNo.TabIndex = 4;
		this.txtManualRefNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtTotalQty_KeyPress);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(942, 17);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(88, 15);
		this.label1.TabIndex = 25;
		this.label1.Text = "Manual Ref No";
		this.cmbproject.AllowNull = (DefaultableBoolean)1;
		this.cmbproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.cmbproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance158).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance158).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance159).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance159;
		((SpecialBoxBase)((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance160).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.cmbproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance164).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance165).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance165).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance165).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.cmbproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbproject).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbproject).Location = new System.Drawing.Point(352, 11);
		((System.Windows.Forms.Control)(object)this.cmbproject).Name = "cmbproject";
		((System.Windows.Forms.Control)(object)this.cmbproject).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.cmbproject).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbproject).TabStop = false;
		this.cmbbranch.AllowNull = (DefaultableBoolean)1;
		this.cmbbranch.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbbranch.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbbranch.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance178).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Appearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.cmbbranch).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbbranch).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbbranch).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbranch).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance180;
		((SpecialBoxBase)((UltraGridBase)this.cmbbranch).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance181).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance181).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance181).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbranch).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.cmbbranch).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbbranch).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance182).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance182;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance183).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance184;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance185).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance186).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance186).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance186).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance187).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.cmbbranch).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbbranch).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbbranch).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbbranch).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbbranch.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbbranch).Location = new System.Drawing.Point(80, 11);
		((System.Windows.Forms.Control)(object)this.cmbbranch).Name = "cmbbranch";
		((System.Windows.Forms.Control)(object)this.cmbbranch).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.cmbbranch).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbbranch).TabStop = false;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(12, 17);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(46, 15);
		this.label13.TabIndex = 0;
		this.label13.Text = "Branch";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(298, 17);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(46, 15);
		this.label18.TabIndex = 0;
		this.label18.Text = "Project";
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(775, 13);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(161, 23);
		this.DocDate.TabIndex = 3;
		this.DocDate.TabStop = false;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(12, 46);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(64, 15);
		this.label33.TabIndex = 8;
		this.label33.Text = "Vehicle No";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(713, 17);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(56, 15);
		this.label17.TabIndex = 3;
		this.label17.Text = "Doc Date";
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Enabled = false;
		this.txtdocno.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(614, 13);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.Size = new System.Drawing.Size(94, 23);
		this.txtdocno.TabIndex = 2;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(565, 17);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(46, 15);
		this.label5.TabIndex = 3;
		this.label5.Text = "Doc No";
		this.panel3.BackColor = System.Drawing.Color.FromArgb(32, 168, 216);
		this.panel3.Controls.Add(this.btnclose);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1143, 41);
		this.panel3.TabIndex = 1;
		this.btnclose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnclose.BackColor = System.Drawing.Color.FromArgb(32, 168, 216);
		this.btnclose.FlatAppearance.BorderSize = 0;
		this.btnclose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnclose.Image = (System.Drawing.Image)resources.GetObject("btnclose.Image");
		this.btnclose.Location = new System.Drawing.Point(1095, 6);
		this.btnclose.Margin = new System.Windows.Forms.Padding(2);
		this.btnclose.Name = "btnclose";
		this.btnclose.Size = new System.Drawing.Size(44, 28);
		this.btnclose.TabIndex = 2;
		this.btnclose.UseVisualStyleBackColor = false;
		this.btnclose.Click += new System.EventHandler(btnclose_Click);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(8, 8);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(207, 24);
		this.label23.TabIndex = 0;
		this.label23.Text = "Purchase Supply Order";
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1143, 25);
		this.panel2.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.btnnew, this.btnupdate, this.btnsave, this.btnprint, this.toolStripButton3, this.btnattachment, this.btnRefresh });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1143, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(55, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnupdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(69, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(55, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnprint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(79, 24);
		this.btnprint.Text = "262-Print";
		this.btnprint.Visible = false;
		this.btnprint.Click += new System.EventHandler(btnprint_Click);
		this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton3.Name = "toolStripButton3";
		this.toolStripButton3.Size = new System.Drawing.Size(56, 24);
		this.toolStripButton3.Text = "Load &PO";
		this.toolStripButton3.Visible = false;
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(94, 24);
		this.btnattachment.Text = "&Attechment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(50, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(1147, 698);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel7.Controls.Add(this.panel9);
		this.panel7.Controls.Add(this.panel8);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(2, 2);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1143, 694);
		this.panel7.TabIndex = 0;
		this.panel9.Controls.Add(this.panel10);
		this.panel9.Controls.Add(this.panel12);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 29);
		this.panel9.Margin = new System.Windows.Forms.Padding(2);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1143, 665);
		this.panel9.TabIndex = 1;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(0, 393);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1143, 272);
		this.panel10.TabIndex = 47;
		this.grdDetail.AllowDelete = (InheritableBoolean)1;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1143, 272);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 46;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1143, 393);
		this.panel12.TabIndex = 46;
		this.grdhistory.AllowDelete = (InheritableBoolean)1;
		this.grdhistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdhistory).BackColor = System.Drawing.Color.White;
		grdhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistory.DesignTimeLayout = grdhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistory.DynamicFiltering = true;
		this.grdhistory.FilterMode = (FilterMode)2;
		this.grdhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1143, 393);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 45;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick_1);
		this.panel8.Controls.Add(this.toolStrip2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1143, 29);
		this.panel8.TabIndex = 0;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnLoadAll });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1143, 25);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(51, 22);
		this.btnLoadAll.Text = "&LoadAll";
		this.btnLoadAll.Click += new System.EventHandler(btnLoadAll_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1155, 724);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "SupplyOrderPurchase";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(PurchsaeOrder_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(PurchsaeOrder_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPendingSupplyOrder).EndInit();
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierCustomer).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbproject).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbranch).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel12.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
