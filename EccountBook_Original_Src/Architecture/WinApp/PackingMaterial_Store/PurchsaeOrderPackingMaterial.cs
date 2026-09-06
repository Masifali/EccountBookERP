using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.PackingMaterial_Store;

public class PurchsaeOrderPackingMaterial : Form
{
	private int ItemId;

	private int DetailPoId;

	public static int Id;

	private int RecId;

	private int DocumentTypeId;

	private int updateDetailIndex = -1;

	private string OrderDetailRemoveIds = "";

	private DataTable table = new DataTable();

	private DataTable tablehistory = new DataTable();

	private DataTable dtitem = new DataTable();

	private DataTable Supplierlst = new DataTable();

	private DataTable listUOM = new DataTable();

	private DataTable dtBranch = new DataTable();

	private bool TaxPercntEditableOrNot;

	private bool PurchaseOrderBranchWise;

	private bool PackingMaterialPlanning_MrpFeature;

	private int DefaultDaysToLessFromHistoryFromDate;

	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

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

	private ToolStripButton btnUpdate;

	private ToolStripButton btnsave;

	private ToolStripButton btnprint;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton btnattachment;

	private Label label23;

	private Panel panel4;

	private GroupBox groupBox1;

	private TextBox txtcatsr;

	private Label label33;

	private Label label2;

	private Label label12;

	private ComboBox combordercat;

	private TextBox txtsupprefno;

	private Label label7;

	private Label label1;

	private Label label8;

	private Label label13;

	private Label label18;

	private DateTimePicker DocDate;

	private Label label17;

	private TextBox txtdocno;

	private Label label26;

	private TextBox txtduedays;

	private Label label24;

	private DateTimePicker duedate;

	private Label label32;

	private DateTimePicker deliverystartdate;

	private Label label30;

	private DateTimePicker validate;

	private Label label27;

	private Label label28;

	private TextBox txtdeliverydays;

	private Panel panel5;

	private GroupBox groupBox3;

	private Button btnplus;

	private TextBox txtremarksdetail;

	private Label label47;

	private TextBox txtRate;

	private TextBox txtItemAmount;

	private TextBox txtqty;

	private Label label40;

	private Label label44;

	private Label label41;

	private Label label38;

	private Label label36;

	private Label label37;

	private Panel panel6;

	private Panel panel7;

	internal UltraCombo combsuppname;

	internal UltraCombo combitem;

	internal UltraCombo combitempck;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grd;

	private CtrlGrdBar ctrlGrdBar2;

	private ComboBox combamounttype;

	private Label label42;

	private Button btnUpdateDetail;

	private Button btnCancelUpdateDetial;

	private TextBox txtBagWeightCut;

	private Label label9;

	private TextBox txtBagPrice;

	private Label label14;

	private Label label15;

	internal GridEX grdhistory;

	private Label label31;

	private ToolStripButton toolStripButton1;

	private Panel panel14;

	private CheckBox ChkBoxPrint;

	internal UltraCombo CmbAgreementNo;

	private Label label4;

	private ComboBox combruom;

	private Label label25;

	private ComboBox combcommtype;

	private Label label21;

	private Label label20;

	private Label label19;

	private TextBox txtcommremarks;

	private TextBox txtCommrate;

	private TextBox txtcommamount;

	private Label label11;

	internal UltraCombo combsalesman;

	internal UltraCombo combrateuom;

	internal GridEX grdDetail;

	internal UltraCombo cmbbranch1;

	internal UltraCombo combproject1;

	internal UltraCombo combcomp1;

	private TextBox txtEqivalent;

	internal UltraCombo CmbStatus;

	internal UltraCombo combdeliverytrm;

	internal UltraCombo combpttrm;

	private RichTextBox txtremarks;

	private TextBox txtTotalWeight;

	private Label label5;

	private TextBox txtWtPerQty;

	private Label label3;

	private ToolStripButton BtnSaveAs;

	private TextBox txtTaxAmount;

	private Label label16;

	private TextBox txtTaxPercnt;

	private Label label10;

	internal UltraCombo CmbTaxName;

	private Label label6;

	private TextBox txtTotalAmount;

	private Label label22;

	internal UltraCombo CmbBillType;

	private Label label34;

	private Panel panel10;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnRefreshHistory;

	private Panel panel8;

	private GroupBox groupBox2;

	private Panel panel11;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Button btnshow;

	private Label label35;

	private Label label39;

	private Label label43;

	private Label label62;

	private Label label63;

	internal UltraCombo cmbSupplierNameHistory;

	private TextBox txtToDocNoHistory;

	private TextBox txtFromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Panel panel12;

	private Panel panel9;

	private Label label45;

	private ToolStripButton BtnShortCutkeys;

	private Label label46;

	private TextBox txtBranchSrNo;

	private Label label29;

	internal UltraCombo cmbBranchName;

	private Label label48;

	private ToolStripButton btnLoader;

	private TextBox txtDocNoShow;

	public PurchsaeOrderPackingMaterial(UserAccount user)
	{
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void PurchsaeOrder_Load(object sender, EventArgs e)
	{
		try
		{
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnprint.Enabled = formright.DoHavePrintRights;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			PackingMaterialPlanning_MrpFeature = CommonServices.GetERPFeatureById(23);
			TaxPercntEditableOrNot = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("TaxPercentEditable"));
			txtTaxPercnt.Enabled = TaxPercntEditableOrNot;
			PurchaseOrderBranchWise = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("PurchaseOrderBranchWise"));
			btnLoader.Visible = PackingMaterialPlanning_MrpFeature;
			dtBranch.Columns.Add("Id", typeof(int));
			dtBranch.Columns.Add("BranchName", typeof(string));
			table.Columns.Add("Id");
			table.Columns.Add("ItemId");
			table.Columns.Add("ItemName");
			table.Columns.Add("PackUomId");
			table.Columns.Add("PackUom");
			table.Columns.Add("ItemQty", typeof(double));
			table.Columns.Add("WeightPerQty", typeof(double));
			table.Columns.Add("TotalWeight", typeof(double));
			table.Columns.Add("ItemRate", typeof(double));
			table.Columns.Add("RateUomId");
			table.Columns.Add("RateUom");
			table.Columns.Add("RateEquivalent", typeof(double));
			table.Columns.Add("Amount", typeof(double));
			table.Columns.Add("TaxNameId", typeof(int));
			table.Columns.Add("TaxName", typeof(string));
			table.Columns.Add("Tax%", typeof(double));
			table.Columns.Add("TaxAmount", typeof(double));
			table.Columns.Add("TotalAmount", typeof(double));
			table.Columns.Add("Remarks");
			grd.DataSource = table;
			dtitem.Columns.Add("Id");
			dtitem.Columns.Add("ItemName");
			dtitem.Columns.Add("ItemCode");
			DocumentTypeId = 700;
			ItemDetailFill();
			ChkBoxPrint.Checked = true;
			DeliveryTerm();
			PaymentTerms();
			OrderStatus();
			CompanyFill();
			BranchFill();
			BranchSrNoFill();
			ProjectFill();
			SupplierNameFill();
			DocumentNoFill();
			BillTypeComboFill();
			UOMFill();
			DocDate.Focus();
			((UltraGridBase)combdeliverytrm).Rows[0].Activate();
			btnUpdate.Visible = false;
			BtnSaveAs.Visible = false;
			btnsave.Visible = true;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			combcommtype.Enabled = false;
			txtCommrate.Enabled = false;
			combruom.Enabled = false;
			txtcommamount.Enabled = false;
			txtcommremarks.Enabled = false;
			((UltraGridBase)CmbStatus).Rows[0].Activate();
			((UltraGridBase)CmbBillType).Rows[0].Activate();
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			HistoryComboFill();
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			ToDateHistory.Value = DateTime.Now;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)cmbbranch1).ActiveRow == null)
		{
			MessageBox.Show("branch field is required");
			cmbbranch1.Focus();
			return false;
		}
		if (((UltraGridBase)combproject1).ActiveRow == null)
		{
			MessageBox.Show("Project Field is Required");
			combproject1.Focus();
			return false;
		}
		if (((UltraGridBase)combsuppname).ActiveRow == null)
		{
			MessageBox.Show("Supplier Name Field is Required");
			combsuppname.Focus();
			return false;
		}
		if (((UltraGridBase)combpttrm).ActiveRow == null)
		{
			MessageBox.Show("Payment Term Field is Required");
			combpttrm.Focus();
			return false;
		}
		if (((UltraGridBase)combdeliverytrm).ActiveRow == null)
		{
			MessageBox.Show("Delivery Term Field is Required");
			combdeliverytrm.Focus();
			return false;
		}
		if (((UltraGridBase)CmbBillType).ActiveRow == null)
		{
			MessageBox.Show("Bill Type Field is Required");
			CmbBillType.Focus();
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
		if (((UltraGridBase)combitempck).ActiveRow == null)
		{
			MessageBox.Show("Pack Uom Field is Required");
			combitempck.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtqty.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Item Qty Field is Required");
			txtqty.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtRate.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Item Rate Field is Required");
			txtRate.Focus();
			return false;
		}
		if (((UltraGridBase)combrateuom).ActiveRow == null)
		{
			MessageBox.Show("Rate UOM Field is Required");
			combrateuom.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtItemAmount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("ItemAmount Field is Required");
			txtItemAmount.Focus();
			return false;
		}
		return true;
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			PurchaseOrder po = new PurchaseOrder();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				po.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			if (grd.RowCount > 0)
			{
				po.DocumentTypeId = DocumentTypeId;
				po.BranchesId = UserAccount.BranchesId;
				po.ProjectsId = Conversion.ToInt(combproject1.Value);
				po.DocDate = DocDate.Value;
				po.DocNo = Conversion.ToInt((object)txtdocno.Text);
				po.BranchSrNo = Conversion.ToInt((object)txtBranchSrNo.Text);
				po.OrderCatagoryId = 2;
				po.OrderSupCustId = Conversion.ToInt(combsuppname.Value);
				po.SupplierRefNo = Conversion.ToString((object)txtsupprefno.Text);
				po.RemarksHeader = Conversion.ToString((object)txtremarks.Text.Trim());
				po.PaymentTermsId = Conversion.ToInt(combpttrm.Value);
				po.OrderDueDays = Conversion.ToInt((object)txtduedays.Text);
				po.OrderDueDate = duedate.Value;
				po.OrderExpiryDate = validate.Value;
				po.DeliveryTerm = Conversion.ToString((object)((Control)(object)combdeliverytrm).Text);
				po.DeliveryStartDate = deliverystartdate.Value;
				po.DeliveryDays = Conversion.ToInt((object)txtdeliverydays.Text);
				po.OrderStatus = "Open";
				po.BrokerAgentSupCustId = Conversion.ToInt(combsalesman.Value);
				po.OrganizationId = UserAccount.OrganizationId;
				po.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				po.CompanyId = UserAccount.CompanyId;
				po.EntryUser = UserAccount.ID;
				po.ModifyUser = UserAccount.ID;
				po.EntryDate = DateTime.Now;
				po.ModifyDate = DateTime.Now;
				po.BillCalculateTypeId = Conversion.ToInt(CmbBillType.Value);
				po.OrderDetailRemoveIds = OrderDetailRemoveIds;
				po.purchaseOrderDetailList = new List<PurchaseOrderDetail>();
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					PurchaseOrderDetail vd = new PurchaseOrderDetail();
					vd.Id = ((RecId > 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
					vd.OrderItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					if (vd.OrderItemId == 0)
					{
						throw new Exception($"Item Required in grid row#{r.RowIndex + 1}");
					}
					vd.OrderItemUOMId = Conversion.ToInt(r.Cells["PackUomId"].Value);
					if (vd.OrderItemUOMId == 0)
					{
						throw new Exception($"Pack Uom Required in grid row#{r.RowIndex + 1}");
					}
					vd.UOMCode = Conversion.ToString(r.Cells["PackUom"].Value);
					vd.UOMCode = Conversion.ToString(r.Cells["PackUom"].Value);
					vd.OrderItemQty = Conversion.ToDouble(r.Cells["ItemQty"].Value);
					if (vd.OrderItemQty == 0.0)
					{
						throw new Exception($"Qty Required in grid row#{r.RowIndex + 1}");
					}
					vd.WeightPerQty = Conversion.ToDouble(r.Cells["WeightPerQty"].Value);
					vd.NetWeight = Conversion.ToDouble(r.Cells["TotalWeight"].Value);
					if (vd.NetWeight == 0.0)
					{
						throw new Exception($"Total Weight Required in grid row#{r.RowIndex + 1}");
					}
					vd.OrderItemRate = Conversion.ToDouble(r.Cells["ItemRate"].Value);
					if (vd.OrderItemRate == 0.0)
					{
						throw new Exception($"Item Rate Required in grid row#{r.RowIndex + 1}");
					}
					vd.OrderItemRateUOMId = Conversion.ToInt(r.Cells["RateUomId"].Value);
					if (vd.OrderItemRateUOMId == 0)
					{
						throw new Exception($"Rate Uom Required in grid row#{r.RowIndex + 1}");
					}
					vd.Amount = Conversion.ToDouble(r.Cells["Amount"].Value);
					if (vd.Amount == 0.0)
					{
						throw new Exception($"Amount Required in grid row#{r.RowIndex + 1}");
					}
					vd.TaxNameId = Conversion.ToInt(r.Cells["TaxNameId"].Value);
					vd.TaxPercent = Conversion.ToDouble(r.Cells["Tax%"].Value);
					vd.TaxAmount = Conversion.ToDouble(r.Cells["TaxAmount"].Value);
					if (vd.TaxPercent > 0.0 && vd.TaxAmount > 0.0 && vd.TaxNameId == 0)
					{
						throw new Exception($"Tax Name required in grid row#{r.RowIndex + 1}");
					}
					if (vd.TaxPercent == 0.0 && vd.TaxAmount == 0.0 && vd.TaxNameId > 0)
					{
						throw new Exception($"Tax percent required in grid row#{r.RowIndex + 1}");
					}
					vd.TotalAmount = Conversion.ToDouble(r.Cells["TotalAmount"].Value);
					vd.OrderRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
					vd.TaxableStatus = ((vd.TaxNameId > 0) ? ((vd.TaxPercent > 0.0) ? ((vd.TaxAmount > 0.0) ? "true" : "false") : "false") : "false");
					po.purchaseOrderDetailList.Add(vd);
				}
				po.AttachmentsList = new List<DMSAttachments>();
				po.DeleteAttachmentsList = new List<DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<PurchaseOrder>(po, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
				int success = PurchaseOrder.Save(po);
				if (RecId > 0)
				{
					MessageBox.Show("Data Update Successfully....[" + po.DocNo + "]");
					FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, DocumentTypeId, base.Name);
				}
				else
				{
					MessageBox.Show("Data Save Successfully....[" + po.DocNo + "]");
				}
				Reset();
				if (ChkBoxPrint.Checked)
				{
					CommonServices.PurchaseOrderPackingMaterialSlip215(success);
				}
			}
			else
			{
				MessageBox.Show("Grid Record Not Found");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "DatabaseError", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnSaveAs_Click(object sender, EventArgs e)
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

	public void ReadById(int ID)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			PurchaseOrder po = new PurchaseOrder();
			RecId = Conversion.ToInt((object)ID);
			po = PurchaseOrder.GetByID(RecId);
			if (po != null)
			{
				OrderDetailRemoveIds = "";
				btnsave.Visible = false;
				BtnSaveAs.Visible = false;
				btnUpdate.Visible = true;
				tabControl1.SelectedIndex = 0;
				DocDate.Value = po.DocDate;
				txtdocno.Text = Conversion.ToString((object)po.DocNo);
				txtDocNoShow.Text = " PO-" + Conversion.ToString((object)po.DocNo);
				txtBranchSrNo.Text = Conversion.ToString((object)po.BranchSrNo);
				combsuppname.Value = Conversion.ToInt((object)po.OrderSupCustId);
				txtsupprefno.Text = Conversion.ToString((object)po.SupplierRefNo);
				txtremarks.Text = Conversion.ToString((object)po.RemarksHeader);
				combpttrm.Value = Conversion.ToInt((object)po.PaymentTermsId);
				txtduedays.Text = Conversion.ToString((object)po.OrderDueDays);
				duedate.Value = po.OrderDueDate;
				validate.Value = po.OrderExpiryDate;
				((Control)(object)combdeliverytrm).Text = Conversion.ToString((object)po.DeliveryTerm);
				deliverystartdate.Value = po.DeliveryStartDate;
				txtdeliverydays.Text = Conversion.ToString((object)po.DeliveryDays);
				CmbBillType.Value = Conversion.ToInt((object)po.BillCalculateTypeId);
				table.Clear();
				for (int i = 0; i < po.purchaseOrderDetailList.Count; i++)
				{
					table.Rows.Add(po.purchaseOrderDetailList[i].Id, po.purchaseOrderDetailList[i].OrderItemId, po.purchaseOrderDetailList[i].ItemName, po.purchaseOrderDetailList[i].OrderItemUOMId, po.purchaseOrderDetailList[i].UOMCode, po.purchaseOrderDetailList[i].OrderItemQty, po.purchaseOrderDetailList[i].WeightPerQty, po.purchaseOrderDetailList[i].NetWeight, po.purchaseOrderDetailList[i].OrderItemRate, po.purchaseOrderDetailList[i].OrderItemRateUOMId, po.purchaseOrderDetailList[i].RateUom, po.purchaseOrderDetailList[i].EquivalentRate, Math.Round(po.purchaseOrderDetailList[i].Amount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero), po.purchaseOrderDetailList[i].TaxNameId, po.purchaseOrderDetailList[i].TaxName, po.purchaseOrderDetailList[i].TaxPercent, po.purchaseOrderDetailList[i].TaxAmount, po.purchaseOrderDetailList[i].TotalAmount, po.purchaseOrderDetailList[i].OrderRemarks);
				}
				grd.DataSource = table;
				grd.RetrieveStructure();
				grdSettings();
				FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CompanyFill()
	{
		try
		{
			DataTable dt = CommonServices.CompanyServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, combcomp1, "Id", "CompName", "Comapny Name", false);
			}
			((UltraGridBase)combcomp1).Rows[0].Activate();
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
			DataTable dt = CommonServices.BrancheServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbbranch1, "Id", "BranchName", "Branch Name", ZeroIndex: false);
			}
			((UltraGridBase)cmbbranch1).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BranchSrNoFill()
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
			int code = PurchaseOrder.GeneratePurchaseOrderBranchCodeByDocId(new PurchaseOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 700,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (code > 0)
			{
				txtBranchSrNo.Text = code.ToString();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void HistoryComboFill()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		try
		{
			dtBranch.Clear();
			((UltraGridBase)cmbBranchName).DataSource = null;
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtSupplier = new DataTable
			{
				Columns = 
				{
					{
						"Id",
						typeof(int)
					},
					{
						"Supplier",
						typeof(string)
					}
				}
			};
			val.BranchesIds = Conversion.ToString((object)UserAccount.BranchesId);
			val.DocumentTypeIds = "700";
			DataTable dt = PurchaseOrder.GetDataForDropDownFromPurchaseOrder(val);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Supplier")
					{
						dtSupplier.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Branch")
					{
						dtBranch.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			if (dtBranch.Rows.Count > 0)
			{
				DDL.BindDDL(dtBranch, cmbBranchName, "Id", "BranchName", "Branch Name", ZeroIndex: true);
				UltraGridColumn obj = ((UltraGridBase)cmbBranchName).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj).Key = "Selected";
				((HeaderBase)obj.Header).Caption = string.Empty;
				obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj.DataType = typeof(bool);
				((HeaderBase)obj.Header).VisiblePosition = 0;
				cmbBranchName.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				((UltraGridBase)cmbBranchName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((Control)(object)cmbBranchName).Text = UserAccount.BranchName.ToString();
			}
			DDL.BindDDL(dtSupplier, cmbSupplierNameHistory, "Id", "Supplier", "Supplier Name", ZeroIndex: false);
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
				DDL.BindDDLNew(dt, combproject1, "Id", "ProjectName", "Project Name", false);
			}
			((UltraGridBase)combproject1).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierNameFill()
	{
		try
		{
			Supplierlst = CommonServices.SupplierCustomerGetforComboServiceBind();
			if (Supplierlst.Rows.Count > 0)
			{
				DDL.BindDDLNew(Supplierlst, combsuppname, "Id", "CompanyName", "Supplier Name", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PaymentTerms()
	{
		try
		{
			DataTable dtdue = CommonServices.GetDueTermServiceBind();
			if (dtdue.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtdue, combpttrm, "Id", "TermsDescription", "Payment Term", false);
				((UltraGridBase)combpttrm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BillTypeComboFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Type");
			dt.Rows.Add(1, "On Weight");
			dt.Rows.Add(2, "On Qty");
			DDL.BindDDL(dt, CmbBillType, "Id", "Type", "Bill Type", ZeroIndex: false);
			((UltraGridBase)CmbBillType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
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
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("DeliveryTerm");
			dt.Rows.Add(1, "Load");
			dt.Rows.Add(2, "Ponch");
			DDL.BindDDL(dt, combdeliverytrm, "Id", "DeliveryTerm", "Delivery Term", ZeroIndex: false);
			((UltraGridBase)combdeliverytrm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)combdeliverytrm).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OrderStatus()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Status");
			dt.Rows.Add(1, "Open");
			dt.Rows.Add(2, "Complete");
			dt.Rows.Add(3, "Cancel");
			DDL.BindDDL(dt, CmbStatus, "Id", "Status", "Order Status", ZeroIndex: false);
			((UltraGridBase)CmbStatus).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocumentNoFill()
	{
		try
		{
			int DocNO = CommonServices.PurchaseOrderGenerateCode(DocumentTypeId);
			if (DocNO > 0)
			{
				txtdocno.Text = DocNO.ToString();
				txtDocNoShow.Text = "PO-" + DocNO;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDetailFill()
	{
		try
		{
			DataTable lst = CommonServices.GetItemByItemTypeId("14");
			if (lst.Rows.Count > 0)
			{
				DDL.BindDDL(lst, combitem, "Id", "ItemName", "Item Name", ZeroIndex: false);
				((UltraGridBase)combitem).DisplayLayout.Bands[0].Columns[1].Width = 500;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UOMFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			UOMSchedule obj = new UOMSchedule();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			listUOM = UOMSchedule.Getall(obj);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindRateUomAndItemPackUom()
	{
		try
		{
			string PackUom = ((Control)(object)combitempck).Text;
			string RateUom = ((Control)(object)combrateuom).Text;
			((Control)(object)combitempck).Text = string.Empty;
			((Control)(object)combrateuom).Text = string.Empty;
			DataTable dtUom = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(combitem.Value));
			if (dtUom.Rows.Count > 0)
			{
				DDL.BindDDL(dtUom, combitempck, "Id", "UOMCode", "PackUom", ZeroIndex: false);
				((UltraGridBase)combitempck).DisplayLayout.Bands[0].Columns[1].Width = 75;
				((UltraGridBase)combitempck).DisplayLayout.Bands[0].Columns[2].Width = 85;
				DDL.BindDDL(dtUom, combrateuom, "Id", "UOMCode", "RateUom", ZeroIndex: false);
				((UltraGridBase)combrateuom).DisplayLayout.Bands[0].Columns[1].Width = 75;
				((UltraGridBase)combrateuom).DisplayLayout.Bands[0].Columns[2].Width = 85;
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == PackUom))
				{
					((Control)(object)combitempck).Text = PackUom;
				}
				else
				{
					((Control)(object)combitempck).Text = string.Empty;
				}
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == RateUom))
				{
					((Control)(object)combrateuom).Text = RateUom;
				}
				else
				{
					((Control)(object)combrateuom).Text = string.Empty;
				}
			}
			else
			{
				((Control)(object)combitempck).Text = string.Empty;
				((Control)(object)combrateuom).Text = string.Empty;
				((UltraGridBase)combitempck).DataSource = null;
				((UltraGridBase)combrateuom).DataSource = null;
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
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		try
		{
			SupplierNameFill();
			ItemDetailFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnplus_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				table.Rows.Add(0, combitem.Value, ((Control)(object)combitem).Text.Trim(), combitempck.Value, ((Control)(object)combitempck).Text.Trim(), Conversion.ToDouble((object)txtqty.Text.Trim()), Conversion.ToDouble((object)txtWtPerQty.Text.Trim()), Conversion.ToDouble((object)txtTotalWeight.Text.Trim()), Conversion.ToDouble((object)txtRate.Text.Trim()), combrateuom.Value, ((Control)(object)combrateuom).Text.Trim(), Conversion.ToDouble(((UltraDropDownBase)combrateuom).SelectedRow.Cells[2].Value), Conversion.ToDouble((object)txtItemAmount.Text), CmbTaxName.Value, ((Control)(object)CmbTaxName).Text, Conversion.ToDouble((object)txtTaxPercnt.Text), Conversion.ToDouble((object)txtTaxAmount.Text), Conversion.ToDouble((object)txtTotalAmount.Text), txtremarksdetail.Text.Trim());
				grd.DataSource = table;
				grd.RetrieveStructure();
				grdSettings();
				ResetDetail();
			}
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
			List<string> hiddenColumns = new List<string> { "Id", "ItemId", "PackUomId", "RateUomId", "RateEquivalent", "TaxNameId" };
			Dictionary<string, EditType> editCol = new Dictionary<string, EditType>
			{
				{
					"ItemQty",
					(EditType)1
				},
				{
					"WeightPerQty",
					(EditType)1
				},
				{
					"ItemRate",
					(EditType)1
				},
				{
					"Remarks",
					(EditType)1
				}
			};
			if (TaxPercntEditableOrNot)
			{
				editCol.Add("Tax%", (EditType)1);
			}
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 2, 3, hiddenColumns, editCol);
			grd.RootTable.Columns["WeightPerQty"].Caption = "Wt / Qty";
			grd.RootTable.Columns["Amount"].Caption = "Item Amount";
			grd.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grd.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["WeightPerQty"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["TotalWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["ItemRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["RateUom"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["TaxName"].Width = Constants.InventoryConstants.DiscountType;
			grd.RootTable.Columns["Tax%"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["TaxAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["TotalAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			GridEX_Helper.AddButton(grd, "Delete", "X", 20);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0 || !(e.Column.Key == "Delete"))
			{
				return;
			}
			if (updateDetailIndex == Conversion.ToInt((object)r.RowIndex))
			{
				throw new Exception("You can't delete this detail because its in Update Mode");
			}
			int CountIds = (from row in table.AsEnumerable()
				where row.Field<int>("Id") > 0
				select row).Count();
			int id = Conversion.ToInt(r.Cells["Id"].Value);
			if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			if (id > 0)
			{
				if (CountIds <= 1)
				{
					throw new Exception("All rows Can not be Deleted in Update Mode");
				}
				OrderDetailRemoveIds = OrderDetailRemoveIds + "," + id;
			}
			r.Delete();
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
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
				DetailPoId = Conversion.ToInt(item.Cells["Id"].Value);
				combitem.Value = item.Cells["ItemId"].Value.ToString();
				ItemId = Conversion.ToInt((object)item.Cells["ItemId"].Value.ToString());
				combitem_Leave_1(null, null);
				combitempck.Value = item.Cells["PackUomId"].Value.ToString();
				((Control)(object)combitempck).Text = item.Cells["PackUom"].Text.ToString();
				txtqty.Text = Conversion.ToDouble(item.Cells["ItemQty"].Value).ToString("#,##0.###");
				txtWtPerQty.Text = Conversion.ToDouble(item.Cells["WeightPerQty"].Value).ToString("#,##0.###");
				txtTotalWeight.Text = Conversion.ToDouble(item.Cells["TotalWeight"].Value).ToString("#,##0.###");
				combrateuom.Value = item.Cells["RateUomId"].Value.ToString();
				((Control)(object)combrateuom).Text = item.Cells["RateUom"].Text.ToString();
				txtRate.Text = Conversion.ToDouble(item.Cells["ItemRate"].Value).ToString(clsGlobalVariables.DecimalRateFormate);
				txtItemAmount.Text = Conversion.ToDouble(item.Cells["Amount"].Value).ToString(clsGlobalVariables.stringFormatsingle);
				if (Conversion.ToInt(item.Cells["TaxNameId"].Value) > 0)
				{
					CmbTaxName.Value = Conversion.ToInt(item.Cells["TaxNameId"].Value);
				}
				txtTaxPercnt.Text = Conversion.ToDouble(item.Cells["Tax%"].Value).ToString("#,##0.##");
				txtTaxAmount.Text = Conversion.ToDouble(item.Cells["TaxAmount"].Value).ToString(clsGlobalVariables.stringFormatsingle);
				txtTotalAmount.Text = Conversion.ToDouble(item.Cells["TotalAmount"].Value).ToString(clsGlobalVariables.stringFormatsingle);
				txtremarksdetail.Text = item.Cells["Remarks"].Value.ToString();
				btnplus.Visible = false;
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

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		grd.UpdateData();
		GridEXRow r = grd.CurrentRow;
		if (r != null && (int)r.RowType == 0)
		{
			switch (e.Column.Key)
			{
			case "ItemQty":
			case "WeightPerQty":
				CalculateWeight(r);
				CalculateAmount(r);
				CalculateTaxAmount(r);
				break;
			case "ItemRate":
			case "RateEquivalent":
				CalculateAmount(r);
				CalculateTaxAmount(r);
				break;
			case "Tax%":
			case "TaxAmount":
				CalculateTaxAmount(r);
				break;
			}
		}
	}

	private void CalculateWeight(GridEXRow r)
	{
		double itemQty = Conversion.ToDouble(r.Cells["ItemQty"].Value);
		double weightPerQty = Conversion.ToDouble(r.Cells["WeightPerQty"].Value);
		double totalWeight = ((itemQty > 0.0 && weightPerQty > 0.0) ? (weightPerQty * itemQty) : 0.0);
		r.BeginEdit();
		r.Cells["TotalWeight"].Value = totalWeight;
		r.EndEdit();
	}

	private void CalculateAmount(GridEXRow r)
	{
		double itemQty = Conversion.ToDouble(r.Cells["ItemQty"].Value);
		double totalWeight = Conversion.ToDouble(r.Cells["TotalWeight"].Value);
		double itemRate = Conversion.ToDouble(r.Cells["ItemRate"].Value);
		double rateEquivalent = Conversion.ToDouble(r.Cells["RateEquivalent"].Value);
		int billTypeId = Conversion.ToInt(CmbBillType.Value);
		double itemAmount = 0.0;
		if (itemQty > 0.0 && rateEquivalent > 0.0 && itemRate > 0.0 && totalWeight > 0.0)
		{
			switch (billTypeId)
			{
			case 1:
				itemAmount = totalWeight / rateEquivalent * itemRate;
				break;
			case 2:
				itemAmount = itemQty / rateEquivalent * itemRate;
				break;
			}
		}
		itemAmount = Math.Round(itemAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
		r.BeginEdit();
		r.Cells["Amount"].Value = itemAmount;
		r.Cells["TotalAmount"].Value = itemAmount;
		r.EndEdit();
	}

	private void CalculateTaxAmount(GridEXRow r)
	{
		double itemAmount = Conversion.ToDouble(r.Cells["Amount"].Value);
		double taxPercent = Conversion.ToDouble(r.Cells["Tax%"].Value);
		double taxAmount = 0.0;
		double totalAmount = itemAmount;
		if (taxPercent > 100.0)
		{
			taxPercent = 0.0;
			MessageBox.Show("Tax% cannot be greater than 100. It has been reset to 0.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
		if (taxPercent > 0.0 && itemAmount > 0.0)
		{
			taxAmount = itemAmount * taxPercent / 100.0;
			totalAmount = itemAmount + taxAmount;
		}
		else
		{
			taxPercent = 0.0;
			taxAmount = 0.0;
		}
		taxAmount = Math.Round(taxAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
		totalAmount = Math.Round(totalAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
		r.BeginEdit();
		r.Cells["Tax%"].Value = taxPercent;
		r.Cells["TaxAmount"].Value = taxAmount;
		r.Cells["TotalAmount"].Value = totalAmount;
		r.EndEdit();
	}

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				ItemId = 0;
				table.Rows[updateDetailIndex]["Id"] = DetailPoId;
				table.Rows[updateDetailIndex]["ItemId"] = combitem.Value;
				table.Rows[updateDetailIndex]["ItemName"] = ((Control)(object)combitem).Text;
				table.Rows[updateDetailIndex]["PackUomId"] = combitempck.Value;
				table.Rows[updateDetailIndex]["PackUom"] = ((Control)(object)combitempck).Text;
				table.Rows[updateDetailIndex]["ItemQty"] = txtqty.Text;
				table.Rows[updateDetailIndex]["WeightPerQty"] = txtWtPerQty.Text;
				table.Rows[updateDetailIndex]["TotalWeight"] = txtTotalWeight.Text;
				table.Rows[updateDetailIndex]["RateUomId"] = combrateuom.Value;
				table.Rows[updateDetailIndex]["RateUom"] = ((Control)(object)combrateuom).Text;
				table.Rows[updateDetailIndex]["RateEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)combrateuom).SelectedRow.Cells[2].Value);
				table.Rows[updateDetailIndex]["ItemRate"] = Conversion.ToDouble((object)txtRate.Text);
				table.Rows[updateDetailIndex]["Amount"] = Conversion.ToDouble((object)txtItemAmount.Text);
				table.Rows[updateDetailIndex]["TaxNameId"] = Conversion.ToInt(CmbTaxName.Value);
				table.Rows[updateDetailIndex]["TaxName"] = ((Control)(object)CmbTaxName).Text;
				table.Rows[updateDetailIndex]["Tax%"] = Conversion.ToDouble((object)txtTaxPercnt.Text);
				table.Rows[updateDetailIndex]["TaxAmount"] = Conversion.ToDouble((object)txtTaxAmount.Text);
				table.Rows[updateDetailIndex]["TotalAmount"] = Conversion.ToDouble((object)txtTotalAmount.Text);
				table.Rows[updateDetailIndex]["Remarks"] = txtremarksdetail.Text;
				grd.DataSource = table;
				ResetDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		btnplus.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelUpdateDetial.Visible = false;
	}

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grd.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grd.CurrentColumn.Key == "Delete")
				{
					if (updateDetailIndex == Conversion.ToInt((object)r.RowIndex))
					{
						throw new Exception("You can't delete this detail because its in Update Mode");
					}
					int CountIds2 = (from row in table.AsEnumerable()
						where row.Field<int>("Id") > 0
						select row).Count();
					int id2 = Conversion.ToInt(r.Cells["Id"].Value);
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					if (id2 > 0)
					{
						if (CountIds2 <= 1)
						{
							throw new Exception("All rows Can not be Deleted in Update Mode");
						}
						OrderDetailRemoveIds = OrderDetailRemoveIds + "," + id2;
					}
					r.Delete();
					grd.UpdateData();
				}
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (updateDetailIndex == Conversion.ToInt((object)r.RowIndex))
				{
					throw new Exception("You can't delete this detail because its in Update Mode");
				}
				int CountIds = (from row in table.AsEnumerable()
					where row.Field<int>("Id") > 0
					select row).Count();
				int id = Conversion.ToInt(r.Cells["Id"].Value);
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				if (id > 0)
				{
					if (CountIds <= 1)
					{
						throw new Exception("All rows Can not be Deleted in Update Mode");
					}
					OrderDetailRemoveIds = OrderDetailRemoveIds + "," + id;
				}
				r.Delete();
				grd.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
			}
			if (e.KeyCode != Keys.F1)
			{
				return;
			}
			int Id = 0;
			string NameCode = "";
			if (grd.CurrentColumn.Key == "PackUomId" || grd.CurrentColumn.Key == "PackUom" || grd.CurrentColumn.Key == "PackEquivalent")
			{
				DataTable dt3 = CommonServices.dtUomFromGloablUomScheduleByItemId(Conversion.ToInt(r.Cells["ItemId"].Value));
				if (dt3.Rows.Count > 0)
				{
					GrdPopUp obj3 = new GrdPopUp(dt3, "Id", "UOMCode", "Equivalent");
					obj3.StartPosition = FormStartPosition.CenterScreen;
					obj3.ShowDialog();
					obj3.BringToFront();
					Id = Conversion.ToInt((object)obj3.ReturnId);
					NameCode = Conversion.ToString((object)obj3.ReturnName);
					r.Cells["PackUomId"].Value = Id;
					r.Cells["PackUom"].Value = NameCode;
				}
				grd.UpdateData();
			}
			if (grd.CurrentColumn.Key == "RateUomId" || grd.CurrentColumn.Key == "RateUom" || grd.CurrentColumn.Key == "RateEquivalent")
			{
				DataTable dt2 = CommonServices.dtUomFromGloablUomScheduleByItemId(Conversion.ToInt(r.Cells["ItemId"].Value));
				if (dt2.Rows.Count > 0)
				{
					GrdPopUp obj2 = new GrdPopUp(dt2, "Id", "UOMCode", "Equivalent");
					obj2.StartPosition = FormStartPosition.CenterScreen;
					obj2.ShowDialog();
					obj2.BringToFront();
					Id = Conversion.ToInt((object)obj2.ReturnId);
					NameCode = Conversion.ToString((object)obj2.ReturnName);
					r.Cells["RateUomId"].Value = Id;
					r.Cells["RateUom"].Value = NameCode;
					decimal Uom = default(decimal);
					DataRow firstRow = (from row in dt2.AsEnumerable()
						where row.Field<int>("Id") == Id
						select row).FirstOrDefault();
					if (firstRow != null)
					{
						Uom = Conversion.ToDecimal(firstRow["Equivalent"]);
					}
					r.Cells["RateEquivalent"].Value = Uom;
				}
				grd.UpdateData();
				CalculateAmount(r);
				CalculateTaxAmount(r);
			}
			if (!(grd.CurrentColumn.Key == "TaxName") && !(grd.CurrentColumn.Key == "TaxNameId"))
			{
				return;
			}
			double returnPercent = 0.0;
			DataTable dt = CommonServices.GetTaxTypeIdAndPercentByItemId(Conversion.ToInt(r.Cells["ItemId"].Value), DocDate.Value);
			if (dt.Rows.Count > 0)
			{
				GrdPopUp obj = new GrdPopUp(dt, "TaxNameId", "TaxName", "TaxPercent");
				obj.StartPosition = FormStartPosition.CenterScreen;
				obj.ShowDialog();
				obj.BringToFront();
				Id = Conversion.ToInt((object)obj.ReturnId);
				NameCode = Conversion.ToString((object)obj.ReturnName);
				returnPercent = Conversion.ToDouble((object)obj.ReturnCode);
				r.Cells["TaxNameId"].Value = Id;
				r.Cells["TaxName"].Value = NameCode;
				if (!TaxPercntEditableOrNot || Id == 0)
				{
					r.Cells["Tax%"].Value = returnPercent;
				}
				else if (Conversion.ToDouble(r.Cells["Tax%"].Value) == 0.0)
				{
					r.Cells["Tax%"].Value = returnPercent;
				}
			}
			grd.UpdateData();
			CalculateTaxAmount(r);
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
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			Id = 0;
			RecId = 0;
			ItemId = 0;
			DetailPoId = 0;
			OrderDetailRemoveIds = "";
			((Control)(object)combsuppname).Text = string.Empty;
			((Control)(object)combsalesman).Text = string.Empty;
			txtsupprefno.Text = string.Empty;
			txtduedays.Text = string.Empty;
			txtdeliverydays.Text = string.Empty;
			txtdeliverydays.Text = string.Empty;
			combcommtype.SelectedIndex = -1;
			((UltraGridBase)combdeliverytrm).Rows[0].Activate();
			txtCommrate.Text = string.Empty;
			txtcommamount.Text = string.Empty;
			txtcommremarks.Text = string.Empty;
			ResetDetail();
			grd.ClearStructure();
			table.Clear();
			DocumentNoFill();
			BranchSrNoFill();
			DocDate.Focus();
			BtnSaveAs.Visible = false;
			btnUpdate.Visible = false;
			btnsave.Visible = true;
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
			updateDetailIndex = -1;
			((Control)(object)combitem).Text = string.Empty;
			((Control)(object)combitempck).Text = string.Empty;
			txtqty.Text = string.Empty;
			((Control)(object)combrateuom).Text = string.Empty;
			txtRate.Text = string.Empty;
			txtItemAmount.Text = string.Empty;
			txtWtPerQty.Text = string.Empty;
			txtTotalWeight.Text = string.Empty;
			((Control)(object)CmbTaxName).Text = string.Empty;
			txtTaxPercnt.Clear();
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			combcommtype.Enabled = false;
			txtCommrate.Enabled = false;
			combruom.Enabled = false;
			txtcommamount.Enabled = false;
			txtcommremarks.Enabled = false;
			combitem.Focus();
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
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					btnnew_Click(null, null);
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
					if (btnsave.Visible && btnsave.Enabled)
					{
						btnsave_Click(null, null);
					}
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					btnshow_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
			{
				btnupdate_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
			{
				btnprint_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				DocDate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.F10)
			{
				btnattachment_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F12)
			{
				BtnSaveAs_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					((Control)(object)grd).Focus();
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					((Control)(object)grdhistory).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					combitem.Focus();
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

	private void grdhistory_DoubleClick_1(object sender, EventArgs e)
	{
		try
		{
			ReadById(Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString()));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void getUpdateForHistory(int ID)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			PurchaseOrder po = new PurchaseOrder();
			po = PurchaseOrder.GetByID(ID);
			if (po != null)
			{
				DataTable table = new DataTable();
				table.Columns.Add("Item");
				table.Columns.Add("PackUOM");
				table.Columns.Add("ItemQty", typeof(double));
				table.Columns.Add("WeightPerQty", typeof(double));
				table.Columns.Add("TotalWeight", typeof(double));
				table.Columns.Add("ItemRate", typeof(double));
				table.Columns.Add("RateUOM");
				table.Columns.Add("ItemAmount", typeof(double));
				table.Columns.Add("TaxName");
				table.Columns.Add("Tax%", typeof(double));
				table.Columns.Add("TaxAmount", typeof(double));
				table.Columns.Add("TotalAmount", typeof(double));
				table.Columns.Add("Remarks");
				table.Clear();
				for (int i = 0; i < po.purchaseOrderDetailList.Count; i++)
				{
					table.Rows.Add(po.purchaseOrderDetailList[i].ItemName, po.purchaseOrderDetailList[i].UOMDescription, po.purchaseOrderDetailList[i].OrderItemQty, po.purchaseOrderDetailList[i].WeightPerQty, po.purchaseOrderDetailList[i].NetWeight, po.purchaseOrderDetailList[i].OrderItemRate, po.purchaseOrderDetailList[i].RateUom, Math.Round(po.purchaseOrderDetailList[i].Amount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero), po.purchaseOrderDetailList[i].TaxName, po.purchaseOrderDetailList[i].TaxPercent, po.purchaseOrderDetailList[i].TaxAmount, po.purchaseOrderDetailList[i].TotalAmount, po.purchaseOrderDetailList[i].OrderRemarks);
				}
				grdDetail.DataSource = table;
				grdDetail.RetrieveStructure();
				grdDetailSettings();
			}
			else
			{
				grdDetail.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridhistoryfill(int NoOfRecords = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = 700;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			obj.NoOfRecords = NoOfRecords;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			obj.FromDocNo = Conversion.ToInt((object)txtFromDocNoHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text);
			obj.SupplierCustomerId = Conversion.ToInt(cmbSupplierNameHistory.Value);
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				obj.BranchesIds = BranchIds;
				DataTable dt = PurchaseOrder.PurchaseOrderPackingMaterialFormHistory(obj);
				if (dt.Rows.Count > 0)
				{
					DataTable dtGrid = new DataTable();
					dtGrid.Columns.Add("RecordNo", typeof(int));
					dtGrid.Columns.Add("Id");
					dtGrid.Columns.Add("DocumentTypeId");
					dtGrid.Columns.Add("DocDate", typeof(DateTime));
					dtGrid.Columns.Add("DocNo", typeof(int));
					dtGrid.Columns.Add("BranchSrNo", typeof(int));
					dtGrid.Columns.Add("BranchName", typeof(string));
					dtGrid.Columns.Add("SupplierName");
					dtGrid.Columns.Add("OrderQty", typeof(double));
					dtGrid.Columns.Add("PaymentTerm");
					dtGrid.Columns.Add("DueDays", typeof(int));
					dtGrid.Columns.Add("DueDate", typeof(DateTime));
					dtGrid.Columns.Add("DeliveryTerm");
					dtGrid.Columns.Add("DeliveryDays", typeof(int));
					dtGrid.Columns.Add("DeliveryStartDate", typeof(DateTime));
					dtGrid.Columns.Add("OrderExpiryDate", typeof(DateTime));
					dtGrid.Columns.Add("RemarksHeader");
					dtGrid.Columns.Add("EntryDate", typeof(DateTime));
					dtGrid.Columns.Add("EntryUser");
					dtGrid.Columns.Add("ModifyDate", typeof(DateTime));
					dtGrid.Columns.Add("ModifyUser");
					dtGrid.Columns.Add("ApprovedDate", typeof(DateTime));
					dtGrid.Columns.Add("ApprovedUser");
					dtGrid.Columns.Add("IsApproved");
					dtGrid.Columns.Add("OrderStatus");
					dtGrid.Columns.Add("NoOfAttachments");
					dtGrid.Rows.Clear();
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						dtGrid.Rows.Add(dt.Rows[i]["RecordNo"], dt.Rows[i]["Id"], dt.Rows[i]["DocumentTypeId"], Conversion.ToDateTime(dt.Rows[i]["DocDate"]).ToShortDateString(), dt.Rows[i]["DocNo"], Conversion.ToInt(dt.Rows[i]["BranchSrNo"]), Conversion.ToString(dt.Rows[i]["BranchName"]), dt.Rows[i]["SupplierName"], dt.Rows[i]["OrderQty"], dt.Rows[i]["TermsDescription"], dt.Rows[i]["DueDays"], Conversion.ToDateTime(dt.Rows[i]["DueDate"]).ToShortDateString(), dt.Rows[i]["DeliveryTerm"], dt.Rows[i]["DeliveryDays"], Conversion.ToDateTime(dt.Rows[i]["DeliveryStartDate"]).ToShortDateString(), Conversion.ToDateTime(dt.Rows[i]["OrderExpiryDate"]).ToShortDateString(), dt.Rows[i]["RemarksHeader"], dt.Rows[i]["EntryDate"], dt.Rows[i]["UserName"], dt.Rows[i]["ModifyDate"], dt.Rows[i]["ModifyUserName"], dt.Rows[i]["PostDate"], dt.Rows[i]["ApprovedUserName"], dt.Rows[i]["IsAproved"], dt.Rows[i]["OrderStatus"], dt.Rows[i]["NoOfAttachments"]);
					}
					grdhistory.DataSource = dtGrid;
					grdhistory.RetrieveStructure();
					HistoryGridSettings();
				}
				else
				{
					grdhistory.DataSource = null;
				}
				return;
			}
			cmbBranchName.Focus();
			throw new Exception("Select Branch First");
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

	private void HistoryGridSettings()
	{
		try
		{
			if (PurchaseOrderBranchWise)
			{
				grdhistory.RootTable.Columns["BranchSrNo"].Visible = false;
				grdhistory.RootTable.Columns["BranchName"].Visible = false;
			}
			grdhistory.RootTable.Columns["Id"].Visible = false;
			grdhistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdhistory.RootTable.Columns["RecordNo"].Visible = false;
			grdhistory.RootTable.Columns["IsApproved"].Visible = false;
			grdhistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdhistory.RootTable.Columns["DocDate"].Width = 70;
			grdhistory.RootTable.Columns["DocNo"].Width = 50;
			grdhistory.RootTable.Columns["SupplierName"].Width = 190;
			grdhistory.RootTable.Columns["OrderQty"].Width = 70;
			grdhistory.RootTable.Columns["PaymentTerm"].Width = 95;
			grdhistory.RootTable.Columns["DueDays"].Width = 70;
			grdhistory.RootTable.Columns["DueDate"].Width = 80;
			grdhistory.RootTable.Columns["DeliveryTerm"].Width = 100;
			grdhistory.RootTable.Columns["DeliveryDays"].Width = 100;
			grdhistory.RootTable.Columns["DeliveryStartDate"].Width = 130;
			grdhistory.RootTable.Columns["OrderExpiryDate"].Width = 120;
			grdhistory.RootTable.Columns["RemarksHeader"].Width = 130;
			grdhistory.RootTable.Columns["EntryDate"].Width = 90;
			grdhistory.RootTable.Columns["EntryUser"].Width = 90;
			grdhistory.RootTable.Columns["ModifyDate"].Width = 90;
			grdhistory.RootTable.Columns["ModifyUser"].Width = 90;
			grdhistory.RootTable.Columns["ApprovedDate"].Width = 130;
			grdhistory.RootTable.Columns["ApprovedUser"].Width = 130;
			grdhistory.RootTable.Columns["IsApproved"].Width = 90;
			grdhistory.RootTable.Columns["OrderStatus"].Width = 90;
			grdhistory.RootTable.Columns["NoOfAttachments"].Width = 90;
			grdhistory.RootTable.Columns.Add("SaveAs");
			grdhistory.RootTable.Columns["SaveAs"].Caption = "SaveAs";
			grdhistory.RootTable.Columns["SaveAs"].ButtonText = "SaveAs";
			grdhistory.RootTable.Columns["SaveAs"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["SaveAs"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["SaveAs"].Width = 60;
			grdhistory.RootTable.Columns.Add("Print");
			grdhistory.RootTable.Columns["Print"].Caption = "Print";
			grdhistory.RootTable.Columns["Print"].ButtonText = "Print";
			grdhistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Print"].Width = 60;
			grdhistory.RootTable.Columns.Add("Edit");
			grdhistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdhistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdhistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Edit"].Width = 60;
			grdhistory.RootTable.Columns["Print"].Position = 0;
			grdhistory.RootTable.Columns["SaveAs"].Position = 1;
			grdhistory.RootTable.Columns["Edit"].Position = 2;
			grdhistory.FrozenColumns = 3;
			grdhistory.RootTable.Columns["OrderQty"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["OrderQty"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["OrderQty"].FormatString = "#,##0.###";
			grdhistory.RootTable.Columns["OrderQty"].TotalFormatString = "#,##0.###";
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				Reset();
				Id = Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value);
				ReadById(Id);
			}
			if (e.Column.Key == "SaveAs")
			{
				Reset();
				Id = Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value);
				ReadById(Id);
				btnsave.Visible = false;
				btnUpdate.Visible = false;
				BtnSaveAs.Visible = true;
				if (grd.GetRows().Length != 0)
				{
					grd.UpdateData();
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow obj in rows)
					{
						obj.BeginEdit();
						obj.Cells["Id"].Value = 0;
						obj.EndEdit();
					}
					grd.UpdateData();
				}
			}
			if (e.Column.Key == "Print")
			{
				Id = Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value);
				GenerateReport(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsearch_Click(object sender, EventArgs e)
	{
		gridhistoryfill();
	}

	private void grdDetailSettings()
	{
		try
		{
			grdDetail.RootTable.Columns["WeightPerQty"].Caption = "Wt / Qty";
			GridEX_Helper.GridWrappingAndColumnSettings(grdDetail, 2, 3);
			grdDetail.RootTable.Columns["Item"].Width = Constants.InventoryConstants.ItemName;
			grdDetail.RootTable.Columns["PackUOM"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdDetail.RootTable.Columns["WeightPerQty"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["TotalWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["ItemRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdDetail.RootTable.Columns["RateUOM"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["TaxName"].Width = Constants.InventoryConstants.DiscountType;
			grdDetail.RootTable.Columns["Tax%"].Width = Constants.InventoryConstants.ExRateConstant;
			grdDetail.RootTable.Columns["TaxAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["TotalAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["WeightPerQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["WeightPerQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["WeightPerQty"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["WeightPerQty"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["TotalWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["TotalWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["TotalWeight"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["TotalWeight"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["Tax%"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Tax%"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["TaxAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["TaxAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["TaxAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["TaxAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["TotalAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["TotalAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["TotalAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["TotalAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grdhistory.CurrentRow;
			if (row != null && (int)row.RowType == 0 && row.Cells.Count > 0)
			{
				int IdHistry = Conversion.ToInt(row.Cells["Id"].Value);
				getUpdateForHistory(Conversion.ToInt((object)IdHistry));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdhistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 700);
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

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
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

	private void CalculateWeight()
	{
		try
		{
			if (Conversion.ToDouble((object)txtqty.Text) > 0.0 && Conversion.ToDouble((object)txtWtPerQty.Text) > 0.0)
			{
				double Qty = Conversion.ToDouble((object)txtqty.Text);
				double TotalWeight = Conversion.ToDouble((object)txtWtPerQty.Text) * Qty;
				txtTotalWeight.Text = TotalWeight.ToString("#,##0.###");
			}
			else
			{
				txtTotalWeight.Text = "";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateItemAount()
	{
		try
		{
			if (((UltraGridBase)combrateuom).ActiveRow != null && ((UltraGridBase)CmbBillType).ActiveRow != null)
			{
				double Qty = Conversion.ToDouble((object)txtqty.Text);
				double Weight = Conversion.ToDouble((object)txtTotalWeight.Text);
				double Rate = Conversion.ToDouble((object)txtRate.Text);
				double RateUOM = Conversion.ToDouble(((UltraDropDownBase)combrateuom).SelectedRow.Cells[2].Value);
				if (Qty > 0.0 && RateUOM > 0.0 && Rate > 0.0 && Weight > 0.0)
				{
					double ItemAmount = 0.0;
					if (Conversion.ToInt(CmbBillType.Value) == 1)
					{
						ItemAmount = Weight / RateUOM * Rate;
					}
					else if (Conversion.ToInt(CmbBillType.Value) == 2)
					{
						ItemAmount = Qty / RateUOM * Rate;
					}
					txtItemAmount.Text = Math.Round(ItemAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
					txtTotalAmount.Text = Math.Round(ItemAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
				}
				else
				{
					txtItemAmount.Text = "0";
					txtTotalAmount.Text = "0";
				}
				CalculateTaxAmount();
			}
			else
			{
				txtItemAmount.Text = "0";
				txtTotalAmount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateTaxAmount()
	{
		try
		{
			double ItemAmount = Conversion.ToDouble((object)txtItemAmount.Text);
			if (((UltraGridBase)CmbTaxName).ActiveRow != null)
			{
				double TaxPercent = 0.0;
				if (!TaxPercntEditableOrNot)
				{
					txtTaxPercnt.Text = Conversion.ToDouble(((UltraDropDownBase)CmbTaxName).SelectedRow.Cells[2].Value).ToString("#,##0.###");
					TaxPercent = Conversion.ToDouble(((UltraDropDownBase)CmbTaxName).SelectedRow.Cells[2].Value);
				}
				else if (Conversion.ToDouble((object)txtTaxPercnt.Text) == 0.0)
				{
					txtTaxPercnt.Text = Conversion.ToDouble(((UltraDropDownBase)CmbTaxName).SelectedRow.Cells[2].Value).ToString("#,##0.###");
					TaxPercent = Conversion.ToDouble(((UltraDropDownBase)CmbTaxName).SelectedRow.Cells[2].Value);
				}
				else
				{
					TaxPercent = Conversion.ToDouble((object)txtTaxPercnt.Text);
				}
				double TaxAmt = ItemAmount * TaxPercent / 100.0;
				txtTaxAmount.Text = Math.Round(TaxAmt, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
				double TotalAmount = ItemAmount + TaxAmt;
				txtTotalAmount.Text = Math.Round(TotalAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
			}
			else if (btnsave.Enabled && btnsave.Visible)
			{
				txtTaxPercnt.Text = "0";
				txtTaxAmount.Text = "0";
				txtTotalAmount.Text = Conversion.ToDouble((object)txtItemAmount.Text).ToString(clsGlobalVariables.stringFormatsingle);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combamounttype_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (combamounttype.Text == "QTY")
			{
				((Control)(object)combrateuom).Text = "1";
				((Control)(object)combrateuom).Enabled = false;
			}
			else
			{
				((Control)(object)combrateuom).Enabled = true;
			}
			CalculateWeight();
			CalculateItemAount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combitempck_Leave(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
			CalculateItemAount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtqty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
			CalculateItemAount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtWtPerQty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
			CalculateItemAount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CommonServices.CommasApplyWhileTyping(sender, e);
			CalculateWeight();
			CalculateItemAount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combrateuom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
			CalculateItemAount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combrateuom_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
			CalculateItemAount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnattachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void GenerateReport(int Id)
	{
		try
		{
			CommonServices.PurchaseOrderPackingMaterialSlip215(Id);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void combitem_Leave_1(object sender, EventArgs e)
	{
		try
		{
			bindRateUomAndItemPackUom();
			if (((UltraGridBase)combitem).ActiveRow != null)
			{
				GetTaxTypeIdAndTaxPercent();
				return;
			}
			((Control)(object)CmbTaxName).Text = string.Empty;
			((UltraGridBase)CmbTaxName).DataSource = null;
			txtTaxPercnt.Clear();
			txtTaxAmount.Clear();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetTaxTypeIdAndTaxPercent()
	{
		try
		{
			DataTable dt = CommonServices.GetTaxTypeIdAndPercentByItemId(Conversion.ToInt(combitem.Value), DocDate.Value);
			if (dt.Rows.Count > 0)
			{
				DataTable dtTax = new DataTable();
				dtTax.Columns.Add("Id");
				dtTax.Columns.Add("TaxType");
				dtTax.Columns.Add("TaxPrcnt");
				dtTax.Rows.Add(dt.Rows[0]["TaxNameId"], dt.Rows[0]["TaxName"], dt.Rows[0]["TaxPercent"]);
				DDL.BindDDL(dtTax, CmbTaxName, "Id", "TaxType", "Tax Name", ZeroIndex: false);
				((UltraGridBase)CmbTaxName).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbTaxName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			else
			{
				((Control)(object)CmbTaxName).Text = string.Empty;
				((UltraGridBase)CmbTaxName).DataSource = null;
				txtTaxPercnt.Text = "";
				txtTaxAmount.Text = "";
			}
			if (updateDetailIndex > -1 && Conversion.ToInt(grd.CurrentRow.Cells["TaxNameId"].Value) > 0)
			{
				CmbTaxName.Value = Conversion.ToInt(grd.CurrentRow.Cells["TaxNameId"].Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtduedays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (txtduedays.Text.Trim() != string.Empty && txtduedays.Text != null)
			{
				duedate.Value = DocDate.Value.AddDays(Conversion.ToDouble((object)txtduedays.Text.Trim()));
			}
			else
			{
				duedate.Value = DocDate.Value;
			}
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
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtWtPerQty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtduedays_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtTaxPercnt_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void CmbTaxName_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbTaxName).ActiveRow != null)
			{
				CalculateTaxAmount();
				return;
			}
			txtTaxPercnt.Text = "";
			txtTaxAmount.Text = "";
			txtTotalAmount.Text = Conversion.ToDouble((object)txtItemAmount.Text).ToString(clsGlobalVariables.stringFormatsingle);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtTaxPercnt_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateTaxAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocDate_Leave(object sender, EventArgs e)
	{
		try
		{
			if (deliverystartdate.Value.Date < DocDate.Value.Date)
			{
				deliverystartdate.Value = DocDate.Value;
				txtduedays_TextChanged(null, null);
				MessageBox.Show("Delivery Start Date Can't Be Less Than Doc Date");
			}
			else
			{
				deliverystartdate.Value = DocDate.Value;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable ChangedTaxPercentFromGridByDatechange(string ItemIds)
	{
		DataTable dt = new DataTable();
		try
		{
			dt = CommonServices.GetTaxScheduleDetailbyItemIds(ItemIds, DocDate.Value);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return dt;
	}

	private void CmbBillType_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbBillType).ActiveRow == null)
			{
				return;
			}
			CalculateItemAount();
			if (grd.GetRows().Count() <= 0)
			{
				return;
			}
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow item in rows)
			{
				item.BeginEdit();
				double qty = Conversion.ToDouble(item.Cells["ItemQty"].Value);
				double weight = Conversion.ToDouble(item.Cells["TotalWeight"].Value);
				double RateEquivalent = Conversion.ToDouble(item.Cells["RateEquivalent"].Value);
				double rate = Conversion.ToDouble(item.Cells["ItemRate"].Value);
				double amount = 0.0;
				if (Conversion.ToInt(CmbBillType.Value) == 1)
				{
					amount = weight / RateEquivalent * rate;
				}
				else if (Conversion.ToInt(CmbBillType.Value) == 2)
				{
					amount = qty / RateEquivalent * rate;
				}
				item.Cells["Amount"].Value = Math.Round(amount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				double TaxPercent = Conversion.ToDouble(item.Cells["Tax%"].Value);
				double TaxAmount = 0.0;
				double TotalAmount = 0.0;
				if (amount > 0.0 && TaxPercent > 0.0)
				{
					TaxAmount = amount * TaxPercent / 100.0;
					item.Cells["TaxAmount"].Value = Math.Round(TaxAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				}
				TotalAmount = amount + TaxAmount;
				item.Cells["TotalAmount"].Value = Math.Round(TotalAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PurchsaeOrderPackingMaterial_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			gridhistoryfill();
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
			grdhistory.ClearStructure();
			grdDetail.ClearStructure();
			drdocdate.Checked = true;
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
			HistoryComboFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFromDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtToDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
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
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+S", "For Save when in form tab and for show data when in history tab");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+F12", "For Save As");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Item Combo in Detail Box");
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

	private void grdhistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdhistory.CurrentColumn == null)
			{
				return;
			}
			int Id = Conversion.ToInt(r.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdhistory.CurrentColumn.Key == "Edit")
				{
					if (!formright.DoHaveUpdateRights)
					{
						throw new Exception("ypu don't have updae rights...");
					}
					RecId = Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value);
					ReadById(Id);
				}
				else if (grdhistory.CurrentColumn.Key == "Print")
				{
					if (formright.DoHavePrintRights)
					{
						GenerateReport(Id);
					}
				}
				else if (grdhistory.CurrentColumn.Key == "SaveAs")
				{
					RecId = Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value);
					ReadById(RecId);
					btnsave.Visible = false;
					btnUpdate.Visible = false;
					BtnSaveAs.Visible = true;
				}
				if (grdhistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("ypu don't have updae rights...");
				}
				ReadById(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combpttrm_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(combpttrm.Value) == 1)
			{
				txtduedays.Text = string.Empty;
				txtduedays.Enabled = false;
				duedate.Value = DocDate.Value;
			}
			else if (Conversion.ToInt(combpttrm.Value) == 2)
			{
				txtduedays.Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void deliverystartdate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (deliverystartdate.Value.Date < DocDate.Value.Date)
			{
				deliverystartdate.Value = DocDate.Value;
				throw new Exception("Delivery Start Date Can't Be Less Than Doc Date");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnLoader_Click(object sender, EventArgs e)
	{
		try
		{
			using frmPackingMaterialRequirementPlanning loader = new frmPackingMaterialRequirementPlanning(UserAccount);
			loader.RequestedByOtherDocument = true;
			loader.ShowSelectButton = true;
			loader.chkOnlyRequiredItems.Checked = true;
			loader.ShowDialog();
			LoadInGridDetail(loader.dtLoader);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridDetail(DataTable dt)
	{
		try
		{
			if (dt.Rows.Count > 0)
			{
				HashSet<int> existingDetailIds = new HashSet<int>(from r in grd.GetRows()
					select Conversion.ToInt(r.Cells["ItemId"].Value));
				foreach (DataRow dr in dt.Rows)
				{
					int ItemId = Conversion.ToInt(dr["PMItemId"]);
					if (existingDetailIds.Add(ItemId))
					{
						DataRow newRow = table.NewRow();
						newRow["ItemId"] = dr["PMItemId"];
						newRow["ItemName"] = dr["ItemName"];
						newRow["PackUomId"] = dr["PackUomId"];
						newRow["PackUom"] = dr["UOMCode"];
						newRow["ItemQty"] = dr["MoreRequired"];
						newRow["WeightPerQty"] = dr["WeightPerQty"];
						newRow["TotalWeight"] = Conversion.ToDouble(dr["MoreRequired"]) * Conversion.ToDouble(dr["WeightPerQty"]);
						newRow["ItemRate"] = 0;
						newRow["RateUomId"] = dr["PackUomId"];
						newRow["RateUom"] = dr["UOMCode"];
						newRow["RateEquivalent"] = dr["Equivalent"];
						table.Rows.Add(newRow);
					}
				}
			}
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
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
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Expected O, but got Unknown
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Expected O, but got Unknown
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Expected O, but got Unknown
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Expected O, but got Unknown
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Expected O, but got Unknown
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Expected O, but got Unknown
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Expected O, but got Unknown
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Expected O, but got Unknown
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Expected O, but got Unknown
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Expected O, but got Unknown
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Expected O, but got Unknown
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Expected O, but got Unknown
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Expected O, but got Unknown
		//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Expected O, but got Unknown
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Expected O, but got Unknown
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Expected O, but got Unknown
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Expected O, but got Unknown
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Expected O, but got Unknown
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Expected O, but got Unknown
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Expected O, but got Unknown
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Expected O, but got Unknown
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Expected O, but got Unknown
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Expected O, but got Unknown
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Expected O, but got Unknown
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_0507: Expected O, but got Unknown
		//IL_0507: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Expected O, but got Unknown
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0515: Expected O, but got Unknown
		//IL_0515: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Expected O, but got Unknown
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Expected O, but got Unknown
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Expected O, but got Unknown
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Expected O, but got Unknown
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		//IL_0538: Expected O, but got Unknown
		//IL_0538: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Expected O, but got Unknown
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0546: Expected O, but got Unknown
		//IL_0546: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Expected O, but got Unknown
		//IL_054d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0554: Expected O, but got Unknown
		//IL_0554: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Expected O, but got Unknown
		//IL_055b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Expected O, but got Unknown
		//IL_059a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a4: Expected O, but got Unknown
		//IL_0613: Unknown result type (might be due to invalid IL or missing references)
		//IL_061d: Expected O, but got Unknown
		//IL_0660: Unknown result type (might be due to invalid IL or missing references)
		//IL_066a: Expected O, but got Unknown
		//IL_0681: Unknown result type (might be due to invalid IL or missing references)
		//IL_068b: Expected O, but got Unknown
		//IL_068c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0696: Expected O, but got Unknown
		//IL_071b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0725: Expected O, but got Unknown
		//IL_0752: Unknown result type (might be due to invalid IL or missing references)
		//IL_075c: Expected O, but got Unknown
		//IL_0773: Unknown result type (might be due to invalid IL or missing references)
		//IL_077d: Expected O, but got Unknown
		//IL_077e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0788: Expected O, but got Unknown
		//IL_07aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b4: Expected O, but got Unknown
		//IL_084f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0859: Expected O, but got Unknown
		//IL_0865: Unknown result type (might be due to invalid IL or missing references)
		//IL_086f: Expected O, but got Unknown
		//IL_08a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b1: Expected O, but got Unknown
		//IL_08bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c7: Expected O, but got Unknown
		//IL_0a5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a69: Expected O, but got Unknown
		//IL_0a8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a95: Expected O, but got Unknown
		//IL_0aac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab6: Expected O, but got Unknown
		//IL_0b30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3a: Expected O, but got Unknown
		//IL_0ba9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb3: Expected O, but got Unknown
		//IL_11ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_11b8: Expected O, but got Unknown
		//IL_11c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_11cf: Expected O, but got Unknown
		//IL_88c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_88d1: Expected O, but got Unknown
		//IL_88f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_88ff: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance150 = new Appearance();
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
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
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
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
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
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.PackingMaterial_Store.PurchsaeOrderPackingMaterial));
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel14 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.txtTotalAmount = new System.Windows.Forms.TextBox();
		this.txtTaxPercnt = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.txtTaxAmount = new System.Windows.Forms.TextBox();
		this.txtItemAmount = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.CmbTaxName = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.txtTotalWeight = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.txtWtPerQty = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtEqivalent = new System.Windows.Forms.TextBox();
		this.combrateuom = new UltraCombo();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.combitempck = new UltraCombo();
		this.combitem = new UltraCombo();
		this.btnplus = new System.Windows.Forms.Button();
		this.txtremarksdetail = new System.Windows.Forms.TextBox();
		this.label47 = new System.Windows.Forms.Label();
		this.txtRate = new System.Windows.Forms.TextBox();
		this.txtqty = new System.Windows.Forms.TextBox();
		this.label40 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.txtBagPrice = new System.Windows.Forms.TextBox();
		this.CmbStatus = new UltraCombo();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label29 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.txtBranchSrNo = new System.Windows.Forms.TextBox();
		this.CmbBillType = new UltraCombo();
		this.label34 = new System.Windows.Forms.Label();
		this.txtremarks = new System.Windows.Forms.RichTextBox();
		this.combdeliverytrm = new UltraCombo();
		this.combpttrm = new UltraCombo();
		this.ChkBoxPrint = new System.Windows.Forms.CheckBox();
		this.label26 = new System.Windows.Forms.Label();
		this.txtduedays = new System.Windows.Forms.TextBox();
		this.combsuppname = new UltraCombo();
		this.label32 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.deliverystartdate = new System.Windows.Forms.DateTimePicker();
		this.label33 = new System.Windows.Forms.Label();
		this.duedate = new System.Windows.Forms.DateTimePicker();
		this.label30 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.txtsupprefno = new System.Windows.Forms.TextBox();
		this.label28 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.txtdeliverydays = new System.Windows.Forms.TextBox();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.label17 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.cmbbranch1 = new UltraCombo();
		this.combruom = new System.Windows.Forms.ComboBox();
		this.combproject1 = new UltraCombo();
		this.label14 = new System.Windows.Forms.Label();
		this.combamounttype = new System.Windows.Forms.ComboBox();
		this.label25 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.combcomp1 = new UltraCombo();
		this.combcommtype = new System.Windows.Forms.ComboBox();
		this.CmbAgreementNo = new UltraCombo();
		this.label21 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.txtBagWeightCut = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.txtcommremarks = new System.Windows.Forms.TextBox();
		this.txtCommrate = new System.Windows.Forms.TextBox();
		this.txtcommamount = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.txtcatsr = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.combordercat = new System.Windows.Forms.ComboBox();
		this.validate = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.BtnSaveAs = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.btnLoader = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label45 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.grdhistory = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.cmbBranchName = new UltraCombo();
		this.label48 = new System.Windows.Forms.Label();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnshow = new System.Windows.Forms.Button();
		this.label35 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.label63 = new System.Windows.Forms.Label();
		this.cmbSupplierNameHistory = new UltraCombo();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel10 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label15 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.combsalesman = new UltraCombo();
		this.txtDocNoShow = new System.Windows.Forms.TextBox();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel5.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combrateuom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combitempck).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combitem).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbStatus).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBillType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combdeliverytrm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combpttrm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combsuppname).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbranch1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combproject1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combcomp1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAgreementNo).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel9.SuspendLayout();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel8.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierNameHistory).BeginInit();
		this.panel10.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.combsalesman).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1145, 663);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1145, 663);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(1137, 637);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add(this.panel14);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(2, 358);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1133, 277);
		this.panel6.TabIndex = 4;
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1133, 277);
		this.panel14.TabIndex = 0;
		this.grd.AllowDelete = (InheritableBoolean)1;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1133, 277);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grd).TabStop = false;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel5.Controls.Add(this.groupBox3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(2, 233);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1133, 125);
		this.panel5.TabIndex = 3;
		this.groupBox3.BackColor = System.Drawing.Color.White;
		this.groupBox3.Controls.Add(this.txtTotalAmount);
		this.groupBox3.Controls.Add(this.txtTaxPercnt);
		this.groupBox3.Controls.Add(this.label10);
		this.groupBox3.Controls.Add(this.label22);
		this.groupBox3.Controls.Add(this.txtTaxAmount);
		this.groupBox3.Controls.Add(this.txtItemAmount);
		this.groupBox3.Controls.Add(this.label16);
		this.groupBox3.Controls.Add(this.label44);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTaxName);
		this.groupBox3.Controls.Add(this.label6);
		this.groupBox3.Controls.Add(this.txtTotalWeight);
		this.groupBox3.Controls.Add(this.label5);
		this.groupBox3.Controls.Add(this.txtWtPerQty);
		this.groupBox3.Controls.Add(this.label3);
		this.groupBox3.Controls.Add(this.txtEqivalent);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.combrateuom);
		this.groupBox3.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox3.Controls.Add(this.btnUpdateDetail);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.combitempck);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.combitem);
		this.groupBox3.Controls.Add(this.btnplus);
		this.groupBox3.Controls.Add(this.txtremarksdetail);
		this.groupBox3.Controls.Add(this.label47);
		this.groupBox3.Controls.Add(this.txtRate);
		this.groupBox3.Controls.Add(this.txtqty);
		this.groupBox3.Controls.Add(this.label40);
		this.groupBox3.Controls.Add(this.label41);
		this.groupBox3.Controls.Add(this.label38);
		this.groupBox3.Controls.Add(this.label36);
		this.groupBox3.Controls.Add(this.label37);
		this.groupBox3.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(3, 3);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(926, 120);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Detail";
		this.txtTotalAmount.BackColor = System.Drawing.SystemColors.Menu;
		this.txtTotalAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalAmount.Location = new System.Drawing.Point(174, 92);
		this.txtTotalAmount.Name = "txtTotalAmount";
		this.txtTotalAmount.ReadOnly = true;
		this.txtTotalAmount.Size = new System.Drawing.Size(103, 23);
		this.txtTotalAmount.TabIndex = 11;
		this.txtTotalAmount.TabStop = false;
		this.txtTotalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTaxPercnt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTaxPercnt.Location = new System.Drawing.Point(7, 92);
		this.txtTaxPercnt.Name = "txtTaxPercnt";
		this.txtTaxPercnt.ShortcutsEnabled = false;
		this.txtTaxPercnt.Size = new System.Drawing.Size(68, 23);
		this.txtTaxPercnt.TabIndex = 9;
		this.txtTaxPercnt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTaxPercnt.TextChanged += new System.EventHandler(txtTaxPercnt_TextChanged);
		this.txtTaxPercnt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtTaxPercnt_KeyPress);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(7, 72);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(35, 15);
		this.label10.TabIndex = 50;
		this.label10.Text = "Tax%";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(174, 72);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(80, 15);
		this.label22.TabIndex = 225;
		this.label22.Text = "Total Amount";
		this.txtTaxAmount.BackColor = System.Drawing.SystemColors.Menu;
		this.txtTaxAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTaxAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTaxAmount.Location = new System.Drawing.Point(76, 92);
		this.txtTaxAmount.Name = "txtTaxAmount";
		this.txtTaxAmount.ReadOnly = true;
		this.txtTaxAmount.Size = new System.Drawing.Size(97, 23);
		this.txtTaxAmount.TabIndex = 10;
		this.txtTaxAmount.TabStop = false;
		this.txtTaxAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtItemAmount.BackColor = System.Drawing.SystemColors.Menu;
		this.txtItemAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemAmount.Location = new System.Drawing.Point(713, 40);
		this.txtItemAmount.Name = "txtItemAmount";
		this.txtItemAmount.ReadOnly = true;
		this.txtItemAmount.Size = new System.Drawing.Size(101, 23);
		this.txtItemAmount.TabIndex = 7;
		this.txtItemAmount.TabStop = false;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(76, 72);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(72, 15);
		this.label16.TabIndex = 223;
		this.label16.Text = "Tax Amount";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(713, 20);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(79, 15);
		this.label44.TabIndex = 23;
		this.label44.Text = "Item Amount";
		this.CmbTaxName.AllowNull = (DefaultableBoolean)1;
		this.CmbTaxName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTaxName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTaxName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance113;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance119).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTaxName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTaxName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTaxName).Location = new System.Drawing.Point(816, 38);
		((System.Windows.Forms.Control)(object)this.CmbTaxName).Name = "CmbTaxName";
		((System.Windows.Forms.Control)(object)this.CmbTaxName).Size = new System.Drawing.Size(104, 26);
		((System.Windows.Forms.Control)(object)this.CmbTaxName).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.CmbTaxName).Leave += new System.EventHandler(CmbTaxName_Leave);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(816, 20);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(60, 15);
		this.label6.TabIndex = 48;
		this.label6.Text = "Tax Name";
		this.txtTotalWeight.BackColor = System.Drawing.SystemColors.Menu;
		this.txtTotalWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalWeight.Location = new System.Drawing.Point(472, 40);
		this.txtTotalWeight.Name = "txtTotalWeight";
		this.txtTotalWeight.ReadOnly = true;
		this.txtTotalWeight.Size = new System.Drawing.Size(94, 23);
		this.txtTotalWeight.TabIndex = 4;
		this.txtTotalWeight.TabStop = false;
		this.txtTotalWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(472, 20);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(75, 15);
		this.label5.TabIndex = 46;
		this.label5.Text = "Total Weight";
		this.txtWtPerQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWtPerQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWtPerQty.Location = new System.Drawing.Point(408, 40);
		this.txtWtPerQty.Name = "txtWtPerQty";
		this.txtWtPerQty.Size = new System.Drawing.Size(62, 23);
		this.txtWtPerQty.TabIndex = 3;
		this.txtWtPerQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtWtPerQty.TextChanged += new System.EventHandler(txtWtPerQty_TextChanged);
		this.txtWtPerQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtWtPerQty_KeyPress);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(405, 20);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(70, 15);
		this.label3.TabIndex = 44;
		this.label3.Text = "Weight/Qty";
		this.txtEqivalent.Location = new System.Drawing.Point(1129, 56);
		this.txtEqivalent.Name = "txtEqivalent";
		this.txtEqivalent.Size = new System.Drawing.Size(100, 25);
		this.txtEqivalent.TabIndex = 42;
		this.txtEqivalent.TabStop = false;
		this.txtEqivalent.Visible = false;
		this.combrateuom.AllowNull = (DefaultableBoolean)1;
		this.combrateuom.AutoCompleteMode = (AutoCompleteMode)4;
		this.combrateuom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combrateuom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combrateuom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.combrateuom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combrateuom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance152).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance152).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combrateuom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combrateuom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance153;
		((SpecialBoxBase)((UltraGridBase)this.combrateuom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combrateuom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.combrateuom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combrateuom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance158).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance159).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance159).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance159).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance159;
		((AppearanceBase)appearance161).TextHAlignAsString = "Left";
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance162).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.combrateuom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combrateuom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combrateuom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combrateuom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combrateuom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combrateuom).Location = new System.Drawing.Point(642, 38);
		((System.Windows.Forms.Control)(object)this.combrateuom).Name = "combrateuom";
		((System.Windows.Forms.Control)(object)this.combrateuom).Size = new System.Drawing.Size(70, 26);
		((System.Windows.Forms.Control)(object)this.combrateuom).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.combrateuom).TextChanged += new System.EventHandler(combrateuom_TextChanged);
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(855, 92);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(62, 22);
		this.btnCancelUpdateDetial.TabIndex = 15;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(787, 92);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(66, 22);
		this.btnUpdateDetail.TabIndex = 14;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.combitempck.AllowNull = (DefaultableBoolean)1;
		this.combitempck.AutoCompleteMode = (AutoCompleteMode)4;
		this.combitempck.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combitempck.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combitempck).DisplayLayout.Appearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.combitempck).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combitempck).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance165).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance165).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combitempck).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitempck).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance166;
		((SpecialBoxBase)((UltraGridBase)this.combitempck).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitempck).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.combitempck).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combitempck).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance168;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance169).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance172).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance173).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance173).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance173).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).TextHAlignAsString = "Left";
		((UltraGridBase)this.combitempck).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance175).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.combitempck).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combitempck).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combitempck).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combitempck).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combitempck.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combitempck).Location = new System.Drawing.Point(260, 38);
		((System.Windows.Forms.Control)(object)this.combitempck).Name = "combitempck";
		((System.Windows.Forms.Control)(object)this.combitempck).Size = new System.Drawing.Size(80, 26);
		((System.Windows.Forms.Control)(object)this.combitempck).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.combitempck).Leave += new System.EventHandler(combitempck_Leave);
		this.combitem.AllowNull = (DefaultableBoolean)1;
		this.combitem.AutoCompleteMode = (AutoCompleteMode)4;
		this.combitem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combitem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combitem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.combitem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combitem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance139).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance139).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combitem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance140;
		((SpecialBoxBase)((UltraGridBase)this.combitem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.combitem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combitem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combitem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combitem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.combitem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combitem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance145).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance146).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance146).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance146).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance147).TextHAlignAsString = "Left";
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combitem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.combitem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combitem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.combitem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combitem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combitem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combitem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combitem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combitem).Location = new System.Drawing.Point(7, 38);
		((System.Windows.Forms.Control)(object)this.combitem).Name = "combitem";
		((System.Windows.Forms.Control)(object)this.combitem).Size = new System.Drawing.Size(252, 26);
		((System.Windows.Forms.Control)(object)this.combitem).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.combitem).Leave += new System.EventHandler(combitem_Leave_1);
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(758, 92);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(27, 22);
		this.btnplus.TabIndex = 13;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.txtremarksdetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarksdetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarksdetail.Location = new System.Drawing.Point(279, 92);
		this.txtremarksdetail.Name = "txtremarksdetail";
		this.txtremarksdetail.Size = new System.Drawing.Size(475, 23);
		this.txtremarksdetail.TabIndex = 12;
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(279, 72);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(52, 15);
		this.label47.TabIndex = 25;
		this.label47.Text = "Remarks";
		this.txtRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRate.Location = new System.Drawing.Point(567, 40);
		this.txtRate.Name = "txtRate";
		this.txtRate.Size = new System.Drawing.Size(74, 23);
		this.txtRate.TabIndex = 5;
		this.txtRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRate.TextChanged += new System.EventHandler(txtRate_TextChanged);
		this.txtRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRate_KeyPress);
		this.txtqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtqty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtqty.Location = new System.Drawing.Point(342, 40);
		this.txtqty.Name = "txtqty";
		this.txtqty.Size = new System.Drawing.Size(64, 23);
		this.txtqty.TabIndex = 2;
		this.txtqty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtqty.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.txtqty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(567, 20);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(58, 15);
		this.label40.TabIndex = 17;
		this.label40.Text = "Item Rate";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(642, 20);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(61, 15);
		this.label41.TabIndex = 19;
		this.label41.Text = "Rate UOM";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(342, 20);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(54, 15);
		this.label38.TabIndex = 13;
		this.label38.Text = "Item Qty";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(260, 20);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(63, 15);
		this.label36.TabIndex = 8;
		this.label36.Text = "Pack UOM";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(8, 20);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(67, 15);
		this.label37.TabIndex = 9;
		this.label37.Text = "Item Name";
		this.panel4.Controls.Add(this.txtBagPrice);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbStatus);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbbranch1);
		this.panel4.Controls.Add(this.combruom);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combproject1);
		this.panel4.Controls.Add(this.label14);
		this.panel4.Controls.Add(this.combamounttype);
		this.panel4.Controls.Add(this.label25);
		this.panel4.Controls.Add(this.label42);
		this.panel4.Controls.Add(this.label31);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combcomp1);
		this.panel4.Controls.Add(this.combcommtype);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAgreementNo);
		this.panel4.Controls.Add(this.label21);
		this.panel4.Controls.Add(this.label9);
		this.panel4.Controls.Add(this.label20);
		this.panel4.Controls.Add(this.txtBagWeightCut);
		this.panel4.Controls.Add(this.label19);
		this.panel4.Controls.Add(this.txtcommremarks);
		this.panel4.Controls.Add(this.txtCommrate);
		this.panel4.Controls.Add(this.txtcommamount);
		this.panel4.Controls.Add(this.label11);
		this.panel4.Controls.Add(this.label13);
		this.panel4.Controls.Add(this.label18);
		this.panel4.Controls.Add(this.txtcatsr);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add(this.combordercat);
		this.panel4.Controls.Add(this.validate);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add(this.label27);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel4.Location = new System.Drawing.Point(2, 56);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1133, 177);
		this.panel4.TabIndex = 2;
		this.txtBagPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBagPrice.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBagPrice.Location = new System.Drawing.Point(1649, 192);
		this.txtBagPrice.Name = "txtBagPrice";
		this.txtBagPrice.Size = new System.Drawing.Size(74, 25);
		this.txtBagPrice.TabIndex = 9;
		this.txtBagPrice.Visible = false;
		this.CmbStatus.AllowNull = (DefaultableBoolean)1;
		this.CmbStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbStatus).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbStatus).Location = new System.Drawing.Point(1045, 55);
		((System.Windows.Forms.Control)(object)this.CmbStatus).Name = "CmbStatus";
		((System.Windows.Forms.Control)(object)this.CmbStatus).Size = new System.Drawing.Size(192, 24);
		((System.Windows.Forms.Control)(object)this.CmbStatus).TabIndex = 13;
		((System.Windows.Forms.Control)(object)this.CmbStatus).Visible = false;
		this.groupBox1.BackColor = System.Drawing.Color.White;
		this.groupBox1.Controls.Add(this.txtDocNoShow);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add(this.label46);
		this.groupBox1.Controls.Add(this.txtBranchSrNo);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBillType);
		this.groupBox1.Controls.Add(this.label34);
		this.groupBox1.Controls.Add(this.txtremarks);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combdeliverytrm);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combpttrm);
		this.groupBox1.Controls.Add(this.ChkBoxPrint);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add(this.txtduedays);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combsuppname);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.deliverystartdate);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.duedate);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.txtsupprefno);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.txtdeliverydays);
		this.groupBox1.Controls.Add(this.DocDate);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.txtdocno);
		this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, 4);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(926, 167);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(312, 81);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(57, 15);
		this.label29.TabIndex = 586;
		this.label29.Text = "Due Date";
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label46.Location = new System.Drawing.Point(5, 51);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(88, 15);
		this.label46.TabIndex = 585;
		this.label46.Text = "Branch Doc No";
		this.txtBranchSrNo.BackColor = System.Drawing.Color.White;
		this.txtBranchSrNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBranchSrNo.Enabled = false;
		this.txtBranchSrNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBranchSrNo.Location = new System.Drawing.Point(104, 47);
		this.txtBranchSrNo.Name = "txtBranchSrNo";
		this.txtBranchSrNo.Size = new System.Drawing.Size(203, 23);
		this.txtBranchSrNo.TabIndex = 2;
		this.txtBranchSrNo.TabStop = false;
		this.CmbBillType.AllowNull = (DefaultableBoolean)1;
		this.CmbBillType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBillType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBillType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbBillType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBillType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBillType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBillType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.CmbBillType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBillType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbBillType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBillType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbBillType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBillType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBillType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBillType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBillType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBillType).Location = new System.Drawing.Point(683, 76);
		((System.Windows.Forms.Control)(object)this.CmbBillType).Name = "CmbBillType";
		((System.Windows.Forms.Control)(object)this.CmbBillType).Size = new System.Drawing.Size(136, 26);
		((System.Windows.Forms.Control)(object)this.CmbBillType).TabIndex = 12;
		((System.Windows.Forms.Control)(object)this.CmbBillType).TextChanged += new System.EventHandler(CmbBillType_TextChanged);
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(574, 82);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(51, 15);
		this.label34.TabIndex = 335;
		this.label34.Text = "Bill Type";
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(103, 135);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(463, 26);
		this.txtremarks.TabIndex = 5;
		this.txtremarks.Text = "";
		this.combdeliverytrm.AllowNull = (DefaultableBoolean)1;
		this.combdeliverytrm.AutoCompleteMode = (AutoCompleteMode)4;
		this.combdeliverytrm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combdeliverytrm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance177).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance178).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance178).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance178).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance179).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance179;
		((SpecialBoxBase)((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance180).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance180).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance181).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance183).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance184;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance185).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance186).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance186).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance186).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance187).TextHAlignAsString = "Left";
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combdeliverytrm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Location = new System.Drawing.Point(418, 104);
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Name = "combdeliverytrm";
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Size = new System.Drawing.Size(148, 26);
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).TabIndex = 9;
		this.combpttrm.AllowNull = (DefaultableBoolean)1;
		this.combpttrm.AutoCompleteMode = (AutoCompleteMode)4;
		this.combpttrm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combpttrm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combpttrm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.combpttrm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combpttrm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.combpttrm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combpttrm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.combpttrm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combpttrm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combpttrm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combpttrm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combpttrm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combpttrm).Location = new System.Drawing.Point(418, 17);
		((System.Windows.Forms.Control)(object)this.combpttrm).Name = "combpttrm";
		((System.Windows.Forms.Control)(object)this.combpttrm).Size = new System.Drawing.Size(148, 26);
		((System.Windows.Forms.Control)(object)this.combpttrm).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.combpttrm).TextChanged += new System.EventHandler(combpttrm_TextChanged);
		this.ChkBoxPrint.AutoSize = true;
		this.ChkBoxPrint.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBoxPrint.Location = new System.Drawing.Point(824, 80);
		this.ChkBoxPrint.Name = "ChkBoxPrint";
		this.ChkBoxPrint.Size = new System.Drawing.Size(67, 19);
		this.ChkBoxPrint.TabIndex = 12;
		this.ChkBoxPrint.Text = "Preview";
		this.ChkBoxPrint.UseVisualStyleBackColor = true;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(312, 23);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(89, 15);
		this.label26.TabIndex = 8;
		this.label26.Text = "Payment Terms";
		this.txtduedays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtduedays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtduedays.Location = new System.Drawing.Point(418, 48);
		this.txtduedays.Name = "txtduedays";
		this.txtduedays.Size = new System.Drawing.Size(148, 23);
		this.txtduedays.TabIndex = 7;
		this.txtduedays.TextChanged += new System.EventHandler(txtduedays_TextChanged);
		this.txtduedays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtduedays_KeyPress);
		this.combsuppname.AllowNull = (DefaultableBoolean)1;
		this.combsuppname.AutoCompleteMode = (AutoCompleteMode)4;
		this.combsuppname.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combsuppname.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combsuppname).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.combsuppname).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combsuppname).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combsuppname).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combsuppname).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.combsuppname).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combsuppname).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.combsuppname).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combsuppname).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.combsuppname).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combsuppname).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combsuppname).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combsuppname).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combsuppname.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combsuppname).Location = new System.Drawing.Point(104, 75);
		((System.Windows.Forms.Control)(object)this.combsuppname).Name = "combsuppname";
		((System.Windows.Forms.Control)(object)this.combsuppname).Size = new System.Drawing.Size(203, 26);
		((System.Windows.Forms.Control)(object)this.combsuppname).TabIndex = 3;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(574, 22);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(79, 15);
		this.label32.TabIndex = 3;
		this.label32.Text = "Delivery Days";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(312, 52);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(61, 15);
		this.label24.TabIndex = 0;
		this.label24.Text = "Due Days ";
		this.deliverystartdate.CustomFormat = "dd-MMM-yyyy";
		this.deliverystartdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.deliverystartdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.deliverystartdate.Location = new System.Drawing.Point(683, 48);
		this.deliverystartdate.Name = "deliverystartdate";
		this.deliverystartdate.Size = new System.Drawing.Size(136, 23);
		this.deliverystartdate.TabIndex = 11;
		this.deliverystartdate.ValueChanged += new System.EventHandler(deliverystartdate_ValueChanged);
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(9, 141);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(52, 15);
		this.label33.TabIndex = 8;
		this.label33.Text = "Remarks";
		this.duedate.CustomFormat = "dd-MMM-yyyy";
		this.duedate.Enabled = false;
		this.duedate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.duedate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.duedate.Location = new System.Drawing.Point(418, 77);
		this.duedate.Name = "duedate";
		this.duedate.Size = new System.Drawing.Size(148, 23);
		this.duedate.TabIndex = 8;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(312, 110);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(80, 15);
		this.label30.TabIndex = 8;
		this.label30.Text = "Delivery Term";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(7, 81);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(86, 15);
		this.label12.TabIndex = 0;
		this.label12.Text = "Supplier Name";
		this.txtsupprefno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsupprefno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtsupprefno.Location = new System.Drawing.Point(104, 106);
		this.txtsupprefno.Name = "txtsupprefno";
		this.txtsupprefno.Size = new System.Drawing.Size(203, 23);
		this.txtsupprefno.TabIndex = 4;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(574, 52);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(106, 15);
		this.label28.TabIndex = 0;
		this.label28.Text = "Delivery Start Date";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(7, 110);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(90, 15);
		this.label8.TabIndex = 3;
		this.label8.Text = "Supplier Ref No";
		this.txtdeliverydays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdeliverydays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdeliverydays.Location = new System.Drawing.Point(683, 18);
		this.txtdeliverydays.Name = "txtdeliverydays";
		this.txtdeliverydays.Size = new System.Drawing.Size(136, 23);
		this.txtdeliverydays.TabIndex = 10;
		this.DocDate.CustomFormat = "dd-MMM-yyyy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(177, 19);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(130, 23);
		this.DocDate.TabIndex = 1;
		this.DocDate.TabStop = false;
		this.DocDate.Leave += new System.EventHandler(DocDate_Leave);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(7, 23);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(88, 15);
		this.label17.TabIndex = 3;
		this.label17.Text = "Doc No && Date";
		this.txtdocno.BackColor = System.Drawing.SystemColors.Menu;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(104, 19);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(71, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.TabStop = false;
		this.cmbbranch1.AllowNull = (DefaultableBoolean)1;
		this.cmbbranch1.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbbranch1.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbbranch1.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Appearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbbranch1).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance126;
		((SpecialBoxBase)((UltraGridBase)this.cmbbranch1).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance132).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance133).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbbranch1).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbbranch1.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbbranch1).Location = new System.Drawing.Point(1045, 2);
		((System.Windows.Forms.Control)(object)this.cmbbranch1).Name = "cmbbranch1";
		((System.Windows.Forms.Control)(object)this.cmbbranch1).Size = new System.Drawing.Size(192, 24);
		((System.Windows.Forms.Control)(object)this.cmbbranch1).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbbranch1).Visible = false;
		this.combruom.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.combruom.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.combruom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.combruom.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combruom.FormattingEnabled = true;
		this.combruom.Items.AddRange(new object[4] { "40", "50", "60", "100" });
		this.combruom.Location = new System.Drawing.Point(1355, 57);
		this.combruom.Name = "combruom";
		this.combruom.Size = new System.Drawing.Size(99, 25);
		this.combruom.TabIndex = 4;
		this.combruom.Visible = false;
		this.combproject1.AllowNull = (DefaultableBoolean)1;
		this.combproject1.AutoCompleteMode = (AutoCompleteMode)4;
		this.combproject1.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combproject1.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combproject1).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.combproject1).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combproject1).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combproject1).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combproject1).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance116;
		((SpecialBoxBase)((UltraGridBase)this.combproject1).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance127).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance127).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combproject1).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.combproject1).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combproject1).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance171).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance171).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance182).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance182).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance182).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance182;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.combproject1).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.combproject1).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combproject1).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combproject1).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combproject1).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combproject1.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combproject1).Location = new System.Drawing.Point(1045, 29);
		((System.Windows.Forms.Control)(object)this.combproject1).Name = "combproject1";
		((System.Windows.Forms.Control)(object)this.combproject1).Size = new System.Drawing.Size(192, 24);
		((System.Windows.Forms.Control)(object)this.combproject1).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.combproject1).Visible = false;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(1647, 170);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(62, 17);
		this.label14.TabIndex = 32;
		this.label14.Text = "Bag Price";
		this.label14.Visible = false;
		this.combamounttype.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.combamounttype.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.combamounttype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.combamounttype.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combamounttype.FormattingEnabled = true;
		this.combamounttype.Items.AddRange(new object[2] { "Weight", "Qty" });
		this.combamounttype.Location = new System.Drawing.Point(1666, 140);
		this.combamounttype.Name = "combamounttype";
		this.combamounttype.Size = new System.Drawing.Size(119, 25);
		this.combamounttype.TabIndex = 7;
		this.combamounttype.Visible = false;
		this.combamounttype.SelectedIndexChanged += new System.EventHandler(combamounttype_SelectedIndexChanged);
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(1243, 120);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(100, 17);
		this.label25.TabIndex = 3;
		this.label25.Text = "Comm Remarks";
		this.label25.Visible = false;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(1663, 117);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(84, 17);
		this.label42.TabIndex = 27;
		this.label42.Text = "Amount Type";
		this.label42.Visible = false;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(947, 60);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(39, 15);
		this.label31.TabIndex = 8;
		this.label31.Text = "Status";
		this.label31.Visible = false;
		this.combcomp1.AllowNull = (DefaultableBoolean)1;
		this.combcomp1.AutoCompleteMode = (AutoCompleteMode)4;
		this.combcomp1.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combcomp1.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combcomp1).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.combcomp1).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combcomp1).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combcomp1).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcomp1).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.combcomp1).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcomp1).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.combcomp1).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combcomp1).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.combcomp1).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combcomp1).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combcomp1).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combcomp1).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combcomp1.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combcomp1).Location = new System.Drawing.Point(1045, 134);
		((System.Windows.Forms.Control)(object)this.combcomp1).Name = "combcomp1";
		((System.Windows.Forms.Control)(object)this.combcomp1).Size = new System.Drawing.Size(192, 24);
		((System.Windows.Forms.Control)(object)this.combcomp1).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.combcomp1).TabStop = false;
		((System.Windows.Forms.Control)(object)this.combcomp1).Visible = false;
		this.combcommtype.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.combcommtype.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.combcommtype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.combcommtype.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combcommtype.FormattingEnabled = true;
		this.combcommtype.Items.AddRange(new object[3] { "Flat", "Percent", "Comm Weight" });
		this.combcommtype.Location = new System.Drawing.Point(1355, -1);
		this.combcommtype.Name = "combcommtype";
		this.combcommtype.Size = new System.Drawing.Size(99, 25);
		this.combcommtype.TabIndex = 2;
		this.combcommtype.Visible = false;
		this.CmbAgreementNo.AllowNull = (DefaultableBoolean)1;
		this.CmbAgreementNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAgreementNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAgreementNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance190).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance191).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance191).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAgreementNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance191;
		((AppearanceBase)appearance192).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance192;
		((SpecialBoxBase)((UltraGridBase)this.CmbAgreementNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAgreementNo).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAgreementNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAgreementNo).Location = new System.Drawing.Point(1045, 82);
		((System.Windows.Forms.Control)(object)this.CmbAgreementNo).Name = "CmbAgreementNo";
		((System.Windows.Forms.Control)(object)this.CmbAgreementNo).Size = new System.Drawing.Size(192, 24);
		((System.Windows.Forms.Control)(object)this.CmbAgreementNo).TabIndex = 9;
		((System.Windows.Forms.Control)(object)this.CmbAgreementNo).TabStop = false;
		((System.Windows.Forms.Control)(object)this.CmbAgreementNo).Visible = false;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(1243, 61);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(69, 17);
		this.label21.TabIndex = 8;
		this.label21.Text = "Rate UOM";
		this.label21.Visible = false;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(1728, 170);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(47, 17);
		this.label9.TabIndex = 13;
		this.label9.Text = "Wt Cut";
		this.label9.Visible = false;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(1243, 3);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(77, 17);
		this.label20.TabIndex = 3;
		this.label20.Text = "Comm Type";
		this.label20.Visible = false;
		this.txtBagWeightCut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBagWeightCut.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBagWeightCut.Location = new System.Drawing.Point(1725, 192);
		this.txtBagWeightCut.Name = "txtBagWeightCut";
		this.txtBagWeightCut.Size = new System.Drawing.Size(74, 25);
		this.txtBagWeightCut.TabIndex = 10;
		this.txtBagWeightCut.Visible = false;
		this.txtBagWeightCut.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(1243, 90);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(95, 17);
		this.label19.TabIndex = 0;
		this.label19.Text = "Comm Amount";
		this.label19.Visible = false;
		this.txtcommremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommremarks.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommremarks.Location = new System.Drawing.Point(1355, 115);
		this.txtcommremarks.Multiline = true;
		this.txtcommremarks.Name = "txtcommremarks";
		this.txtcommremarks.Size = new System.Drawing.Size(99, 57);
		this.txtcommremarks.TabIndex = 6;
		this.txtcommremarks.Visible = false;
		this.txtCommrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommrate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommrate.Location = new System.Drawing.Point(1355, 28);
		this.txtCommrate.Name = "txtCommrate";
		this.txtCommrate.Size = new System.Drawing.Size(99, 23);
		this.txtCommrate.TabIndex = 3;
		this.txtCommrate.Visible = false;
		this.txtcommamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommamount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommamount.Location = new System.Drawing.Point(1355, 86);
		this.txtcommamount.Name = "txtcommamount";
		this.txtcommamount.ReadOnly = true;
		this.txtcommamount.Size = new System.Drawing.Size(99, 23);
		this.txtcommamount.TabIndex = 5;
		this.txtcommamount.Visible = false;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(1243, 32);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(76, 17);
		this.label11.TabIndex = 0;
		this.label11.Text = "Comm Rate";
		this.label11.Visible = false;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(947, 7);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(44, 15);
		this.label13.TabIndex = 0;
		this.label13.Text = "Branch";
		this.label13.Visible = false;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(947, 34);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(44, 15);
		this.label18.TabIndex = 0;
		this.label18.Text = "Project";
		this.label18.Visible = false;
		this.txtcatsr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcatsr.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcatsr.Location = new System.Drawing.Point(1045, 190);
		this.txtcatsr.Name = "txtcatsr";
		this.txtcatsr.ReadOnly = true;
		this.txtcatsr.Size = new System.Drawing.Size(192, 21);
		this.txtcatsr.TabIndex = 4;
		this.txtcatsr.TabStop = false;
		this.txtcatsr.Visible = false;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(947, 166);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(88, 15);
		this.label7.TabIndex = 0;
		this.label7.Text = "Order Catagory";
		this.label7.Visible = false;
		this.combordercat.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.combordercat.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.combordercat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.combordercat.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combordercat.FormattingEnabled = true;
		this.combordercat.Location = new System.Drawing.Point(1045, 162);
		this.combordercat.Name = "combordercat";
		this.combordercat.Size = new System.Drawing.Size(192, 23);
		this.combordercat.TabIndex = 3;
		this.combordercat.TabStop = false;
		this.combordercat.Visible = false;
		this.validate.CustomFormat = "dd-MMM-yyyy";
		this.validate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.validate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.validate.Location = new System.Drawing.Point(1045, 110);
		this.validate.Name = "validate";
		this.validate.Size = new System.Drawing.Size(192, 21);
		this.validate.TabIndex = 15;
		this.validate.TabStop = false;
		this.validate.Visible = false;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(947, 193);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(68, 15);
		this.label2.TabIndex = 8;
		this.label2.Text = "Catagory Sr";
		this.label2.Visible = false;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(947, 113);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(72, 15);
		this.label27.TabIndex = 0;
		this.label27.Text = "Validite Date";
		this.label27.Visible = false;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(947, 139);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(59, 15);
		this.label1.TabIndex = 0;
		this.label1.Text = "Company";
		this.label1.Visible = false;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(947, 87);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(82, 15);
		this.label4.TabIndex = 0;
		this.label4.Text = "AgreementNo";
		this.label4.Visible = false;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1133, 29);
		this.panel3.TabIndex = 1;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1289, 4);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 5);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(162, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "PURCHASE ORDER";
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1133, 25);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[10] { this.btnnew, this.toolStripButton1, this.btnUpdate, this.BtnSaveAs, this.btnsave, this.btnprint, this.toolStripSeparator, this.btnattachment, this.BtnShortCutkeys, this.btnLoader });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1133, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.toolStripButton1.Image = (System.Drawing.Image)resources.GetObject("toolStripButton1.Image");
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(80, 24);
		this.toolStripButton1.Text = "Refresh";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Click += new System.EventHandler(btnupdate_Click);
		this.BtnSaveAs.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.BtnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnSaveAs.Name = "BtnSaveAs";
		this.BtnSaveAs.Size = new System.Drawing.Size(80, 24);
		this.BtnSaveAs.Text = "&SaveAs";
		this.BtnSaveAs.Click += new System.EventHandler(BtnSaveAs_Click);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnprint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(92, 24);
		this.btnprint.Text = "215-&Print";
		this.btnprint.Click += new System.EventHandler(btnprint_Click);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.btnLoader.Image = Architecture.WinApp.Properties.Resources.download;
		this.btnLoader.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoader.Name = "btnLoader";
		this.btnLoader.Size = new System.Drawing.Size(112, 24);
		this.btnLoader.Text = "MRP Loader ";
		this.btnLoader.Click += new System.EventHandler(btnLoader_Click);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(1137, 637);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel7.Controls.Add(this.panel12);
		this.panel7.Controls.Add(this.panel9);
		this.panel7.Controls.Add(this.panel11);
		this.panel7.Controls.Add(this.panel8);
		this.panel7.Controls.Add(this.panel10);
		this.panel7.Controls.Add(this.toolStrip2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(2, 2);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1133, 633);
		this.panel7.TabIndex = 0;
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel12.Location = new System.Drawing.Point(0, 493);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1133, 140);
		this.panel12.TabIndex = 48;
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
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1133, 140);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 46;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label45);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 466);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1133, 27);
		this.panel9.TabIndex = 47;
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.ForeColor = System.Drawing.Color.White;
		this.label45.Location = new System.Drawing.Point(4, 3);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(242, 19);
		this.label45.TabIndex = 1;
		this.label45.Text = "Detail of above selected row";
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 127);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1133, 339);
		this.panel11.TabIndex = 5;
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
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1133, 339);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 45;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.grdhistory.SelectionChanged += new System.EventHandler(grdhistory_SelectionChanged);
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick_1);
		((System.Windows.Forms.Control)(object)this.grdhistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdhistory_KeyDown);
		this.panel8.Controls.Add(this.groupBox2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 54);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1133, 73);
		this.panel8.TabIndex = 4;
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchName);
		this.groupBox2.Controls.Add(this.label48);
		this.groupBox2.Controls.Add(this.rdapproveddate);
		this.groupBox2.Controls.Add(this.rdmodifydate);
		this.groupBox2.Controls.Add(this.drdocdate);
		this.groupBox2.Controls.Add(this.rdentrydate);
		this.groupBox2.Controls.Add(this.btnshow);
		this.groupBox2.Controls.Add(this.label35);
		this.groupBox2.Controls.Add(this.label39);
		this.groupBox2.Controls.Add(this.label43);
		this.groupBox2.Controls.Add(this.label62);
		this.groupBox2.Controls.Add(this.label63);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory);
		this.groupBox2.Controls.Add(this.txtToDocNoHistory);
		this.groupBox2.Controls.Add(this.txtFromDocNoHistory);
		this.groupBox2.Controls.Add(this.ToDateHistory);
		this.groupBox2.Controls.Add(this.FromDateHistory);
		this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(0, 0);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1130, 73);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.cmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance106).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance107).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Location = new System.Drawing.Point(3, 36);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Name = "cmbBranchName";
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Size = new System.Drawing.Size(183, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).TabIndex = 0;
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(3, 18);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(79, 15);
		this.label48.TabIndex = 145;
		this.label48.Text = "Branch Name";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(1018, 42);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 9;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(1018, 22);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(937, 22);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 6;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(937, 42);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 7;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(878, 35);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(55, 26);
		this.btnshow.TabIndex = 6;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label35.Location = new System.Drawing.Point(651, 18);
		this.label35.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(86, 15);
		this.label35.TabIndex = 157;
		this.label35.Text = "Supplier Name";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label39.Location = new System.Drawing.Point(555, 18);
		this.label39.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(64, 15);
		this.label39.TabIndex = 156;
		this.label39.Text = "To Doc No";
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label43.Location = new System.Drawing.Point(460, 18);
		this.label43.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(79, 15);
		this.label43.TabIndex = 155;
		this.label43.Text = "From Doc No";
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label62.Location = new System.Drawing.Point(325, 18);
		this.label62.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(48, 15);
		this.label62.TabIndex = 154;
		this.label62.Text = "To Date";
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label63.Location = new System.Drawing.Point(189, 18);
		this.label63.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(63, 15);
		this.label63.TabIndex = 153;
		this.label63.Text = "From Date";
		this.cmbSupplierNameHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplierNameHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplierNameHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplierNameHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSupplierNameHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Location = new System.Drawing.Point(651, 36);
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Name = "cmbSupplierNameHistory";
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Size = new System.Drawing.Size(224, 26);
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).TabIndex = 5;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(555, 38);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.ShortcutsEnabled = false;
		this.txtToDocNoHistory.Size = new System.Drawing.Size(92, 23);
		this.txtToDocNoHistory.TabIndex = 4;
		this.txtToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoHistory_KeyPress);
		this.txtFromDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoHistory.Location = new System.Drawing.Point(460, 38);
		this.txtFromDocNoHistory.Name = "txtFromDocNoHistory";
		this.txtFromDocNoHistory.ShortcutsEnabled = false;
		this.txtFromDocNoHistory.Size = new System.Drawing.Size(92, 23);
		this.txtFromDocNoHistory.TabIndex = 3;
		this.txtFromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNoHistory_KeyPress);
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(325, 38);
		this.ToDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(130, 23);
		this.ToDateHistory.TabIndex = 2;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(189, 38);
		this.FromDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(130, 23);
		this.FromDateHistory.TabIndex = 1;
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.ctrlGrdBar2);
		this.panel10.Controls.Add(this.label15);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 27);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1133, 27);
		this.panel10.TabIndex = 3;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1095, 3);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdhistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(3, 3);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(197, 19);
		this.label15.TabIndex = 0;
		this.label15.Text = "Purchase Order History";
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNewHistory, this.btnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1133, 27);
		this.toolStrip2.TabIndex = 2;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnRefreshHistory.Image = (System.Drawing.Image)resources.GetObject("btnRefreshHistory.Image");
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.combsalesman.AllowNull = (DefaultableBoolean)1;
		this.combsalesman.AutoCompleteMode = (AutoCompleteMode)4;
		this.combsalesman.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combsalesman.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combsalesman).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.combsalesman).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combsalesman).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combsalesman).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combsalesman).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.combsalesman).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combsalesman).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.combsalesman).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combsalesman).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.combsalesman).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combsalesman).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combsalesman).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combsalesman).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combsalesman.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combsalesman).Location = new System.Drawing.Point(117, 13);
		((System.Windows.Forms.Control)(object)this.combsalesman).Name = "combsalesman";
		((System.Windows.Forms.Control)(object)this.combsalesman).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.combsalesman).TabIndex = 1;
		this.txtDocNoShow.BackColor = System.Drawing.Color.White;
		this.txtDocNoShow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoShow.Enabled = false;
		this.txtDocNoShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoShow.Location = new System.Drawing.Point(104, 19);
		this.txtDocNoShow.Name = "txtDocNoShow";
		this.txtDocNoShow.ReadOnly = true;
		this.txtDocNoShow.Size = new System.Drawing.Size(72, 23);
		this.txtDocNoShow.TabIndex = 595;
		this.txtDocNoShow.TabStop = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1145, 663);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "PurchsaeOrderPackingMaterial";
		this.Text = "PurchsaeOrder";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(PurchsaeOrderPackingMaterial_FormClosing);
		base.Load += new System.EventHandler(PurchsaeOrder_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(PurchsaeOrder_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel14.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel5.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combrateuom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combitempck).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combitem).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbStatus).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBillType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combdeliverytrm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combpttrm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combsuppname).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbranch1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combproject1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combcomp1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAgreementNo).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel12.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel11.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierNameHistory).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.combsalesman).EndInit();
		base.ResumeLayout(false);
	}
}
