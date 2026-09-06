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
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
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
namespace Architecture.WinApp.PurchaseTrading
{

public class PurchsaeOrderWithQty : Form
{
	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private Rightsobjects formrights = new Rightsobjects();

	private int updateDetailIndex;

	private int ItemId;

	private int DetailPoId;

	private DropDownBind DDL = new DropDownBind();

	private int RecId;

	public bool IsTaxable;

	private int DocumentTypeId;

	private int[] AryColWidth;

	private DataTable table = new DataTable();

	private DataTable tablehistory = new DataTable();

	private DataTable dtitem = new DataTable();

	private DataTable Supplierlst = new DataTable();

	private DataTable listUOM = new DataTable();

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

	private DateTimePicker duedate;

	private DateTimePicker deliverystartdate;

	private Label label30;

	private Label label29;

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

	private TextBox txtweight;

	private TextBox txtqty;

	private Label label40;

	private Label lblTaxTypes;

	private Label label44;

	private Label label41;

	private Label label39;

	private Label label38;

	private Label label36;

	private Label label37;

	private Panel panel6;

	internal UltraCombo combsuppname;

	internal UltraCombo combitem;

	internal UltraCombo combjob;

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

	internal UltraCombo CmbTaxType;

	private Label label34;

	private TextBox txtTaxPercent;

	private Label lblTaxPercent;

	private TextBox txtTotalAmount;

	private Label lblTotalAmount;

	private Label lblTaxAmount;

	private TextBox txtTaxAmount;

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

	private Label label6;

	private TextBox txtRetailPrice;

	private Label label10;

	internal UltraCombo combrateuom;

	internal GridEX grdDetail;

	internal UltraCombo cmbbranch1;

	internal UltraCombo combproject1;

	internal UltraCombo combcomp1;

	private TextBox txtEqivalent;

	internal UltraCombo CmbWholeSale;

	internal UltraCombo CmbStatus;

	internal UltraCombo combdeliverytrm;

	internal UltraCombo combpttrm;

	private RichTextBox txtremarks;

	private Label label3;

	private CtrlGrdBar ctrlGrdBar3;

	internal UltraCombo CmbCity;

	private Label label5;

	private ToolStripButton BtnSaveAs;

	private Label label16;

	internal UltraCombo CmbBillType;

	private Label label22;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnRefreshHistory;

	private Panel panel7;

	private Panel panel12;

	private GroupBox groupBox2;

	private Button btnshow;

	private Label label59;

	private Label label60;

	private Label label61;

	private Label label62;

	private Label label63;

	internal UltraCombo cmbSupplierNameHistory;

	private TextBox txtToDocNoHistory;

	private TextBox txtFromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Panel panel8;

	private Panel panel9;

	private Panel panel10;

	private RadioButton rdpqtymodify;

	private RadioButton rdpqtydoc;

	private RadioButton rdpqtyentry;

	public PurchsaeOrderWithQty(UserAccount user)
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
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
		if (((UltraGridBase)CmbBillType).ActiveRow == null)
		{
			MessageBox.Show("Bill Type Field is Required");
			CmbBillType.Focus();
			return false;
		}
		if (((UltraGridBase)combpttrm).ActiveRow == null)
		{
			MessageBox.Show("Payment Term Field is Required");
			combpttrm.Focus();
			return false;
		}
		if (((Control)(object)combpttrm).Text == "Credit" && Conversion.ToInt((object)txtduedays.Text) == 0)
		{
			MessageBox.Show("Due Days Field is Required");
			txtduedays.Focus();
			return false;
		}
		if (((UltraGridBase)combdeliverytrm).ActiveRow == null)
		{
			MessageBox.Show("Delivery Term Field is Required");
			combdeliverytrm.Focus();
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
		if (((UltraGridBase)combjob).ActiveRow == null)
		{
			MessageBox.Show("Job Lot Field is Required");
			combjob.Focus();
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
		if (((UltraGridBase)CmbCity).ActiveRow == null)
		{
			MessageBox.Show("City Name Field is Required");
			CmbCity.Focus();
			return false;
		}
		return true;
	}

	private void btnplus_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationDetail())
			{
				return;
			}
			double TaxPercent = 0.0;
			double TaxAmount = 0.0;
			double TotalAmount = 0.0;
			if (txtTaxPercent.Text != string.Empty)
			{
				TaxPercent = Conversion.ToDouble((object)txtTaxPercent.Text.Trim());
			}
			if (txtTaxAmount.Text != string.Empty)
			{
				TaxAmount = Conversion.ToDouble((object)txtTaxAmount.Text.Trim());
			}
			if (txtTotalAmount.Text != string.Empty)
			{
				TotalAmount = Conversion.ToDouble((object)txtTotalAmount.Text.Trim());
			}
			GridEXRow[] rows = grd.GetRows();
			for (int i = 0; i < rows.Length; i++)
			{
				int ItemId = Conversion.ToInt(rows[i].Cells["ItemId"].Value);
				if (Conversion.ToInt(combitem.Value) == ItemId)
				{
					MessageBox.Show("Duplicate Item Not Add in Grid");
					return;
				}
			}
			table.Rows.Add(0, combitem.Value, ((Control)(object)combitem).Text, combjob.Value, ((Control)(object)combjob).Text.Trim(), combitempck.Value, ((Control)(object)combitempck).Text, Conversion.ToDouble((object)txtqty.Text), Convert.ToDouble(txtweight.Text), Conversion.ToDouble((object)txtRate.Text), combrateuom.Value, ((Control)(object)combrateuom).Text, Conversion.ToDouble((object)txtItemAmount.Text), CmbTaxType.Value, ((Control)(object)CmbTaxType).Text.Trim(), TaxPercent, TaxAmount, TotalAmount, ((Control)(object)CmbCity).Text, txtremarksdetail.Text.Trim());
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			((Control)(object)combitem).Text = string.Empty;
			((Control)(object)combitempck).Text = string.Empty;
			txtqty.Text = string.Empty;
			txtweight.Text = string.Empty;
			txtRetailPrice.Text = string.Empty;
			((Control)(object)combrateuom).Text = string.Empty;
			txtRate.Text = string.Empty;
			txtItemAmount.Text = string.Empty;
			((Control)(object)CmbTaxType).Text = string.Empty;
			txtTaxPercent.Text = string.Empty;
			txtTaxAmount.Text = string.Empty;
			txtremarksdetail.Text = string.Empty;
			txtTotalAmount.Text = string.Empty;
			combitem.Focus();
			TotalCommissionAmount();
			IsTaxable = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Expected O, but got Unknown
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
				po.BranchesId = Conversion.ToInt(cmbbranch1.Value);
				po.ProjectsId = Conversion.ToInt(combproject1.Value);
				po.DocDate = DocDate.Value;
				po.DocNo = Conversion.ToInt((object)txtdocno.Text);
				po.OrderCatagoryId = Conversion.ToInt(combordercat.SelectedValue);
				po.CatagorySrNo = Conversion.ToInt((object)txtcatsr.Text);
				po.OrderSupCustId = Conversion.ToInt(combsuppname.Value);
				po.SupplierRefNo = Conversion.ToString((object)txtsupprefno.Text);
				po.BillCalculateTypeId = Conversion.ToInt(CmbBillType.Value);
				po.RemarksHeader = Conversion.ToString((object)txtremarks.Text.Trim());
				po.PaymentTermsId = Conversion.ToInt(combpttrm.Value);
				po.RefrenenceParty = Conversion.ToInt(CmbAgreementNo.Value);
				po.OrderDueDays = Conversion.ToInt((object)txtduedays.Text);
				po.OrderDueDate = duedate.Value;
				po.OrderExpiryDate = validate.Value;
				po.DeliveryTerm = Conversion.ToString((object)((Control)(object)combdeliverytrm).Text);
				po.DeliveryStartDate = deliverystartdate.Value;
				po.DeliveryDays = Conversion.ToInt((object)txtdeliverydays.Text);
				po.OrderType = Conversion.ToString((object)((Control)(object)CmbWholeSale).Text);
				if (combsalesman.Value != null)
				{
					po.CommissionType = Conversion.ToString((object)combcommtype.Text);
					po.CommRate = Conversion.ToDouble((object)txtCommrate.Text);
					po.UomScheduleIdCmRate = Conversion.ToInt((object)combruom.Text);
					po.CommAmount = Conversion.ToDouble((object)txtcommamount.Text.Trim());
					po.CommissionRemarks = Conversion.ToString((object)txtcommremarks.Text);
				}
				po.OrderStatus = ((Control)(object)CmbStatus).Text.Trim();
				po.BrokerAgentSupCustId = Conversion.ToInt(combsalesman.Value);
				po.OrganizationId = UserAccount.OrganizationId;
				po.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				po.CompanyId = UserAccount.CompanyId;
				po.EntryUser = UserAccount.ID;
				po.ModifyUser = UserAccount.ID;
				po.EntryDate = DateTime.Now;
				po.ModifyDate = DateTime.Now;
				po.purchaseOrderDetailList = new List<PurchaseOrderDetail>();
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					PurchaseOrderDetail vd = new PurchaseOrderDetail();
					vd.Id = ((RecId > 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
					vd.OrderItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					vd.OrderItemUOMId = Conversion.ToInt(r.Cells["PackUomId"].Value);
					vd.OrderItemQty = Conversion.ToSingle(r.Cells["ItemQty"].Value);
					vd.NetWeight = Conversion.ToSingle(r.Cells["Weight"].Value);
					vd.OrderItemRate = Conversion.ToSingle(r.Cells["ItemRate"].Value);
					vd.OrderItemRateUOMId = Conversion.ToInt(r.Cells["RateUomId"].Value);
					vd.Amount = Conversion.ToSingle(r.Cells["ItemAmount"].Value);
					vd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					vd.OrderRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
					vd.TaxNameId = Conversion.ToInt(r.Cells["TaxTypeId"].Value);
					vd.TaxPercent = Conversion.ToSingle(r.Cells["TaxPercent"].Value);
					vd.TaxAmount = Conversion.ToSingle(r.Cells["TaxAmount"].Value);
					vd.TotalAmount = Conversion.ToSingle(r.Cells["TotalAmount"].Value);
					vd.CityArea = Conversion.ToString((object)r.Cells["CityName"].Text);
					vd.TaxableStatus = ((vd.TaxNameId > 0) ? ((vd.TaxPercent > 0.0) ? ((vd.TaxAmount > 0.0) ? "true" : "false") : "false") : "false");
					po.purchaseOrderDetailList.Add(vd);
				}
				po.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				po.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<PurchaseOrder>(po, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
				int success = PurchaseOrder.Save(po);
				if (RecId > 0)
				{
					MessageBox.Show("Data Update Successfully....[" + po.DocNo + "]");
					FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, po.DocumentTypeId, base.Name);
				}
				else
				{
					MessageBox.Show("Data Save Successfully....[" + po.DocNo + "]");
				}
				Reset();
				if (ChkBoxPrint.Checked)
				{
					CommonServices.PurchaseOrderSlip_Engr1600(success);
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

	private void OrderCatagoryfill()
	{
		try
		{
			combordercat.DataSource = InvOrderCategory.GetAll();
			combordercat.DisplayMember = "OrderCategoryName";
			combordercat.ValueMember = "Id";
			combordercat.SelectedIndex = 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combordercat_Leave(object sender, EventArgs e)
	{
		try
		{
			int code = CommonServices.GenerateOrderCategoryCodebyId(Conversion.ToInt(combordercat.SelectedValue));
			if (code > 0)
			{
				txtcatsr.Text = code.ToString();
			}
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

	private void ComboAgreementNoFill()
	{
	}

	private void combsalesman_Leave(object sender, EventArgs e)
	{
		combcommtype.Enabled = true;
		txtCommrate.Enabled = true;
		combruom.Enabled = true;
		txtcommamount.Enabled = true;
		txtcommremarks.Enabled = true;
		combcommtype.Focus();
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
				((UltraGridBase)combpttrm).Rows[1].Activate();
			}
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

	private void WholeAndRetail()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("TaxType");
			dt.Rows.Add(1, "Whole");
			dt.Rows.Add(2, "Retail ");
			DDL.BindDDL(dt, CmbWholeSale, "Id", "TaxType", "TaxType", ZeroIndex: false);
			((UltraGridBase)CmbWholeSale).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)CmbWholeSale).Rows[0].Activate();
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
			((UltraGridBase)CmbStatus).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
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
			DataTable lst = CommonServices.ItemGetForComboServiceBindForEngr();
			if (lst.Rows.Count > 0)
			{
				DDL.BindDDL(lst, combitem, "Id", "ItemName", "Item Name", ZeroIndex: false);
				((UltraGridBase)combitem).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void bindRateUomAndItemPackUom(int ID)
	{
		try
		{
			string Packuom = ((Control)(object)combitempck).Text;
			string Rateuom = ((Control)(object)combrateuom).Text;
			((Control)(object)combitempck).Text = string.Empty;
			((Control)(object)combrateuom).Text = string.Empty;
			DataTable dtUom = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(combitem.Value));
			if (dtUom.Rows.Count > 0)
			{
				DDL.BindDDL(dtUom, combitempck, "Id", "UOMCode", "PackUOM", ZeroIndex: false);
				((UltraGridBase)combitempck).DisplayLayout.Bands[0].Columns[1].Width = 75;
				((UltraGridBase)combitempck).DisplayLayout.Bands[0].Columns[2].Width = 85;
				DDL.BindDDL(dtUom, combrateuom, "Id", "UOMCode", "RateUOM", ZeroIndex: false);
				((UltraGridBase)combrateuom).DisplayLayout.Bands[0].Columns[1].Width = 75;
				((UltraGridBase)combrateuom).DisplayLayout.Bands[0].Columns[2].Width = 85;
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Packuom))
				{
					((Control)(object)combitempck).Text = Packuom;
				}
				else
				{
					((Control)(object)combitempck).Text = string.Empty;
				}
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Rateuom))
				{
					((Control)(object)combrateuom).Text = Rateuom;
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void TaxItemsBind()
	{
		try
		{
			DataTable dtitem = CommonServices.GetItemsFromItemTaxSchedule();
			if (dtitem.Rows.Count > 0)
			{
				DDL.BindDDL(dtitem, combitem, "Id", "ItemName", "Item Name", ZeroIndex: false);
				((UltraGridBase)combitem).DisplayLayout.Bands[0].Columns[1].Width = 500;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combojoblotfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			List<JobLot> lst = jobLot.GetList(new JobLot
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (lst.Count > 0)
			{
				DataTable dtJob = new DataTable();
				dtJob.Columns.Add("Id");
				dtJob.Columns.Add("JobLotDescription");
				dtJob.Rows.Clear();
				for (int i = 0; i < lst.Count; i++)
				{
					dtJob.Rows.Add(lst[i].Id, lst[i].JobLotDescription);
				}
				if (dtJob.Rows.Count > 0)
				{
					DDL.BindDDL(dtJob, combjob, "Id", "JobLotDescription", "Job Lot", ZeroIndex: false);
				}
				((UltraGridBase)combjob).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CityFill()
	{
		try
		{
			DataTable dt = CommonServices.CityGetAllService();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbCity, "Id", "CityName", "City Name", false);
				((UltraGridBase)CmbCity).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
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
		combordercat_Leave(null, null);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void PurchsaeOrder_Load(object sender, EventArgs e)
	{
		try
		{
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formrights.DoHaveSaveRight;
			btnupdate.Enabled = formrights.DoHaveUpdateRights;
			btnprint.Enabled = formrights.DoHavePrintRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formrights.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
			DocumentTypeId = 1600;
			dtViewAttachments.Columns.Add("AttchmentPath");
			table.Columns.Add("Id", typeof(int));
			table.Columns.Add("ItemId", typeof(int));
			table.Columns.Add("ItemName");
			table.Columns.Add("JobLotId", typeof(int));
			table.Columns.Add("JobLot");
			table.Columns.Add("PackUomId", typeof(int));
			table.Columns.Add("PackUom");
			table.Columns.Add("ItemQty", typeof(double));
			table.Columns.Add("Weight", typeof(double));
			table.Columns.Add("ItemRate", typeof(double));
			table.Columns.Add("RateUomId", typeof(int));
			table.Columns.Add("RateUom");
			table.Columns.Add("ItemAmount", typeof(double));
			table.Columns.Add("TaxTypeId", typeof(int));
			table.Columns.Add("TaxType");
			table.Columns.Add("TaxPercent", typeof(double));
			table.Columns.Add("TaxAmount", typeof(double));
			table.Columns.Add("TotalAmount", typeof(double));
			table.Columns.Add("CityName");
			table.Columns.Add("Remarks");
			grd.DataSource = table;
			dtitem.Columns.Add("Id");
			dtitem.Columns.Add("ItemName");
			dtitem.Columns.Add("ItemCode");
			ChkBoxPrint.Checked = true;
			BranchFill();
			ProjectFill();
			CompanyFill();
			DocumentNoFill();
			BillTypeComboFill();
			DeliveryTerm();
			PaymentTerms();
			OrderStatus();
			WholeAndRetail();
			SupplierNameFill();
			ItemDetailFill();
			combojoblotfill();
			CityFill();
			DocDate.Focus();
			((UltraGridBase)CmbWholeSale).Rows[0].Activate();
			((UltraGridBase)combdeliverytrm).Rows[0].Activate();
			btnupdate.Visible = false;
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
			((Control)(object)CmbStatus).Enabled = false;
			FromDateHistory.Value = DateTime.Now;
			ToDateHistory.Value = DateTime.Now;
			HistoryComboFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryComboFill()
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		try
		{
			DataTable dtSupplier = new DataTable();
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("Supplier", typeof(string));
			DataTable dt = PurchaseOrder.GetDataForDropDownFromPurchaseOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
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

	private void Reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			BtnSaveAs.Visible = false;
			RecId = 0;
			ItemId = 0;
			DetailPoId = 0;
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
			((Control)(object)combitem).Text = string.Empty;
			((Control)(object)combitempck).Text = string.Empty;
			txtqty.Text = string.Empty;
			txtweight.Text = string.Empty;
			((Control)(object)combrateuom).Text = string.Empty;
			txtRate.Text = string.Empty;
			txtItemAmount.Text = string.Empty;
			txtTaxPercent.Text = string.Empty;
			((Control)(object)CmbTaxType).Text = string.Empty;
			txtTaxAmount.Text = string.Empty;
			((Control)(object)combjob).Text = string.Empty;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			combcommtype.Enabled = false;
			txtCommrate.Enabled = false;
			combruom.Enabled = false;
			txtcommamount.Enabled = false;
			txtcommremarks.Enabled = false;
			txtTotalAmount.Text = string.Empty;
			DocumentNoFill();
			table.Clear();
			grd.DataSource = table;
			DocDate.Focus();
			((Control)(object)CmbStatus).Enabled = false;
			((UltraGridBase)CmbStatus).Rows[0].Activate();
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
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["JobLotId"].Visible = false;
			grd.RootTable.Columns["PackUomId"].Visible = false;
			grd.RootTable.Columns["RateUomId"].Visible = false;
			grd.RootTable.Columns["TaxTypeId"].Visible = false;
			grd.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			grd.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Weight"].FormatString = "#,##0.###";
			grd.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["TaxPercent"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["TaxPercent"].FormatString = "#,##0.###";
			grd.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["TaxAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["TaxAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["TaxAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["TaxAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["TotalAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["TotalAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["TotalAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["TotalAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustment(AryColWidth, grd);
			if (btnsave.Visible && btnsave.Enabled)
			{
				grd.RootTable.Columns.Add("Delete");
				grd.RootTable.Columns["Delete"].Key = "Delete";
				grd.RootTable.Columns["Delete"].Caption = "X";
				grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
				grd.RootTable.Columns["Delete"].ButtonText = "X";
				grd.RootTable.Columns["Delete"].Width = 20;
			}
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetailSettings()
	{
		try
		{
			grdDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Weight"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["TaxPercent"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["TaxPercent"].FormatString = "#,##0.####";
			grdDetail.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["TaxAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["TaxAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["TaxAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["TaxAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["TotalAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["TotalAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["TotalAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["TotalAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdDetail);
			ctrlGrdBar3_Load(null, null);
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
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					combsuppname.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					FromDateHistory.Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.F10)
			{
				btnattachment_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F12 && BtnSaveAs.Enabled && BtnSaveAs.Visible)
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
					combsuppname.Focus();
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					FromDateHistory.Focus();
				}
			}
			if (btnupdate.Visible && e.Control && e.KeyCode == Keys.U)
			{
				btnupdate_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				btnprint_Click(null, null);
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
			ReadById(Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value));
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
				btnsave.Visible = false;
				BtnSaveAs.Visible = true;
				btnupdate.Visible = true;
				combcomp1.Value = Conversion.ToInt((object)po.CompanyId);
				cmbbranch1.Value = Conversion.ToInt((object)po.BranchesId);
				combproject1.Value = Conversion.ToInt((object)po.ProjectsId);
				tabControl1.SelectedIndex = 0;
				DocDate.Value = po.DocDate;
				txtdocno.Text = Conversion.ToString((object)po.DocNo);
				combordercat.SelectedValue = Conversion.ToInt((object)po.OrderCatagoryId);
				txtcatsr.Text = po.CatagorySrNo.ToString();
				combsuppname.Value = Conversion.ToInt((object)po.OrderSupCustId);
				txtsupprefno.Text = Conversion.ToString((object)po.SupplierRefNo);
				CmbBillType.Value = po.BillCalculateTypeId;
				txtremarks.Text = Conversion.ToString((object)po.RemarksHeader);
				combpttrm.Value = Conversion.ToInt((object)po.PaymentTermsId);
				txtduedays.Text = Conversion.ToString((object)po.OrderDueDays);
				duedate.Value = po.OrderDueDate;
				validate.Value = po.OrderExpiryDate;
				((Control)(object)combdeliverytrm).Text = Conversion.ToString((object)po.DeliveryTerm);
				deliverystartdate.Value = po.DeliveryStartDate;
				txtdeliverydays.Text = Conversion.ToString((object)po.DeliveryDays);
				combsalesman.Value = Conversion.ToInt((object)po.BrokerAgentSupCustId);
				combcommtype.Text = Conversion.ToString((object)po.CommissionType);
				txtCommrate.Text = Conversion.ToString((object)po.CommRate);
				combruom.Text = Conversion.ToString((object)po.UomScheduleIdCmRate);
				txtcommamount.Text = Conversion.ToString((object)po.CommAmount);
				txtcommremarks.Text = Conversion.ToString((object)po.CommissionRemarks);
				((Control)(object)CmbStatus).Enabled = true;
				((Control)(object)CmbStatus).Text = Conversion.ToString((object)po.OrderStatus);
				table.Clear();
				for (int i = 0; i < po.purchaseOrderDetailList.Count; i++)
				{
					table.Rows.Add(po.purchaseOrderDetailList[i].Id, po.purchaseOrderDetailList[i].OrderItemId, po.purchaseOrderDetailList[i].ItemName, po.purchaseOrderDetailList[i].JobLotId, po.purchaseOrderDetailList[i].JobLotDescription, po.purchaseOrderDetailList[i].OrderItemUOMId, po.purchaseOrderDetailList[i].UOMCode, po.purchaseOrderDetailList[i].OrderItemQty, po.purchaseOrderDetailList[i].NetWeight, po.purchaseOrderDetailList[i].OrderItemRate, po.purchaseOrderDetailList[i].OrderItemRateUOMId, po.purchaseOrderDetailList[i].RateUom, po.purchaseOrderDetailList[i].Amount, po.purchaseOrderDetailList[i].TaxNameId, po.purchaseOrderDetailList[i].TaxName, po.purchaseOrderDetailList[i].TaxPercent, po.purchaseOrderDetailList[i].TaxAmount, po.purchaseOrderDetailList[i].TotalAmount, po.purchaseOrderDetailList[i].CityArea, po.purchaseOrderDetailList[i].OrderRemarks);
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

	private void GetDetailByHeaderId(int ID)
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
				table.Columns.Add("ItemName");
				table.Columns.Add("JobLot");
				table.Columns.Add("PackUom");
				table.Columns.Add("ItemQty", typeof(double));
				table.Columns.Add("Weight", typeof(double));
				table.Columns.Add("ItemRate", typeof(double));
				table.Columns.Add("RateUom");
				table.Columns.Add("ItemAmount", typeof(double));
				table.Columns.Add("TaxName");
				table.Columns.Add("TaxPercent", typeof(double));
				table.Columns.Add("TaxAmount", typeof(double));
				table.Columns.Add("TotalAmount", typeof(double));
				table.Columns.Add("CityName");
				table.Columns.Add("Remarks");
				table.Clear();
				for (int i = 0; i < po.purchaseOrderDetailList.Count; i++)
				{
					table.Rows.Add(po.purchaseOrderDetailList[i].ItemName, po.purchaseOrderDetailList[i].JobLotDescription, po.purchaseOrderDetailList[i].UOMDescription, po.purchaseOrderDetailList[i].OrderItemQty, po.purchaseOrderDetailList[i].NetWeight, po.purchaseOrderDetailList[i].OrderItemRate, po.purchaseOrderDetailList[i].RateUom, po.purchaseOrderDetailList[i].Amount, po.purchaseOrderDetailList[i].TaxName, po.purchaseOrderDetailList[i].TaxPercent, po.purchaseOrderDetailList[i].TaxAmount, po.purchaseOrderDetailList[i].TotalAmount, po.purchaseOrderDetailList[i].CityArea, po.purchaseOrderDetailList[i].OrderRemarks);
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
			obj.DocumentTypeId = DocumentTypeId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.CanViewAllRecord = formrights.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = clsGlobalVariables.UserAccount.ID;
			}
			if (rdpqtydoc.Checked)
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
			else if (rdpqtyentry.Checked)
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
			else if (rdpqtymodify.Checked)
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
			obj.FromDocNo = Conversion.ToInt((object)txtFromDocNoHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text);
			obj.SupplierCustomerId = Conversion.ToInt(cmbSupplierNameHistory.Value);
			DataTable dtHistory = PurchaseOrder.Getall(obj);
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("SupplierName", typeof(string));
				dt.Columns.Add("BillType", typeof(string));
				dt.Columns.Add("PaymentTerm", typeof(string));
				dt.Columns.Add("DueDays", typeof(int));
				dt.Columns.Add("DueDate", typeof(DateTime));
				dt.Columns.Add("DeliveryTerm", typeof(string));
				dt.Columns.Add("DeliveryDays", typeof(int));
				dt.Columns.Add("DeliveryStartDate", typeof(DateTime));
				dt.Columns.Add("OrderExpiryDate", typeof(DateTime));
				dt.Columns.Add("OrderStatus", typeof(string));
				dt.Columns.Add("IsApproved", typeof(string));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("Remarks", typeof(string));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				foreach (DataRow row in dtHistory.Rows)
				{
					dt.Rows.Add(row["Id"], row["DocNo"], Conversion.ToDateTime(row["DocDate"]), row["SupplierName"], row["BillType"], row["TermsDescription"], row["DueDays"], Conversion.ToDateTime(row["DueDate"]), row["DeliveryTerm"], row["DeliveryDays"], Conversion.ToDateTime(row["DeliveryStartDate"]), Conversion.ToDateTime(row["OrderExpiryDate"]), row["OrderStatus"], Conversion.ToBool(row["IsAproved"]) ? "Approved" : "Not Approved", row["UserName"], row["EntryDate"], row["ModifyUserName"], row["ModifyDate"], row["RemarksHeader"], row["NoOfAttachments"]);
				}
				grdhistory.DataSource = dt;
				grdhistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				grdhistory.ClearStructure();
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
			grdhistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			CommonServices.GridAutoAdjustmentNew(grdhistory);
			grdhistory.RootTable.Columns.Add("Print");
			grdhistory.RootTable.Columns["Print"].Caption = "Print";
			grdhistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Print"].ButtonText = "Print";
			grdhistory.RootTable.Columns["Print"].Width = 40;
			grdhistory.RootTable.Columns["Print"].Position = 0;
			grdhistory.RootTable.Columns.Add("SaveAs");
			grdhistory.RootTable.Columns["SaveAs"].Caption = "SaveAs";
			grdhistory.RootTable.Columns["SaveAs"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["SaveAs"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["SaveAs"].ButtonText = "SaveAs";
			grdhistory.RootTable.Columns["SaveAs"].Width = 60;
			grdhistory.RootTable.Columns["SaveAs"].Position = 1;
			grdhistory.RootTable.Columns.Add("Edit");
			grdhistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdhistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdhistory.RootTable.Columns["Edit"].Width = 40;
			grdhistory.RootTable.Columns["Edit"].Position = 2;
			grdhistory.FrozenColumns = 3;
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			ctrlGrdBar2_Load(null, null);
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
			MessageBox.Show(ex.Message);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateWeight()
	{
		try
		{
			if (((UltraGridBase)combitempck).ActiveRow != null)
			{
				double Qty = Conversion.ToDouble((object)txtqty.Text);
				double packUom = Conversion.ToDouble(((UltraDropDownBase)combitempck).SelectedRow.Cells[2].Value);
				txtweight.Text = (packUom * Qty).ToString("#,##0.###");
			}
			else
			{
				txtweight.Text = 0.ToString();
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
			if (((UltraGridBase)CmbTaxType).ActiveRow != null)
			{
				txtTaxPercent.Text = Conversion.ToDouble(((UltraDropDownBase)CmbTaxType).SelectedRow.Cells[2].Value).ToString("#,##0.###");
				double ItemAmount = Conversion.ToDouble((object)txtItemAmount.Text);
				double TaxPercent = Conversion.ToDouble(((UltraDropDownBase)CmbTaxType).SelectedRow.Cells[2].Value);
				double TaxAmt = 0.0;
				if (TaxPercent > 0.0)
				{
					TaxAmt = ItemAmount * TaxPercent / 100.0;
				}
				txtTaxAmount.Text = TaxAmt.ToString(clsGlobalVariables.stringFormatsingle);
				double TotalAmount = ItemAmount + TaxAmt;
				txtTotalAmount.Text = TotalAmount.ToString(clsGlobalVariables.stringFormatsingle);
			}
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
			if (listUOM.Rows.Count > 0)
			{
				for (int i = 0; i < listUOM.Rows.Count; i++)
				{
					if (Conversion.ToInt(listUOM.Rows[i]["Id"]) == Conversion.ToInt(combitempck.Value))
					{
						txtEqivalent.Text = listUOM.Rows[i]["Equivalent"].ToString();
					}
				}
			}
			CalculateWeight();
			TotalAmount();
			TotalCommissionAmount();
			CalculateTaxAmount();
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
			TotalAmount();
			CalculateTaxAmount();
			TotalCommissionAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalAmount()
	{
		try
		{
			double weight = Conversion.ToDouble((object)txtweight.Text);
			double Rate = Conversion.ToDouble((object)txtRate.Text);
			double RateUOM = 0.0;
			double TotalAmount = 0.0;
			if (weight > 0.0 && ((UltraGridBase)combrateuom).ActiveRow != null && Rate > 0.0)
			{
				txtItemAmount.Text = string.Empty;
				RateUOM = Conversion.ToDouble(((UltraDropDownBase)combrateuom).SelectedRow.Cells[2].Value);
				TotalAmount = weight / RateUOM * Rate;
				txtItemAmount.Text = TotalAmount.ToString(clsGlobalVariables.stringFormatsingle);
				txtTotalAmount.Text = TotalAmount.ToString(clsGlobalVariables.stringFormatsingle);
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

	private void TotalCommissionAmount()
	{
		try
		{
			if (txtCommrate.Text != string.Empty && combcommtype.SelectedIndex == 0)
			{
				double CommRate3 = double.Parse(txtCommrate.Text.Trim());
				txtcommamount.Text = CommRate3.ToString();
			}
			if (txtCommrate.Text != string.Empty && txtItemAmount.Text != string.Empty && combcommtype.SelectedIndex == 1)
			{
				double CommRate2 = double.Parse(txtCommrate.Text.Trim());
				object total = grd.GetTotal(grd.RootTable.Columns["Amount"], (AggregateFunction)2);
				if (total != null)
				{
					double CommAmt2 = double.Parse(total.ToString()) * CommRate2 / 100.0;
					txtcommamount.Text = Math.Round(CommAmt2).ToString();
				}
			}
			if (txtweight.Text != string.Empty && txtCommrate.Text != string.Empty && combruom.Text != string.Empty && combcommtype.SelectedIndex == 2)
			{
				double CommRate = double.Parse(txtCommrate.Text.Trim());
				object Weight = grd.GetTotal(grd.RootTable.Columns["Weight"], (AggregateFunction)2);
				if (Weight != null)
				{
					double num = double.Parse(Weight.ToString());
					double UOM = double.Parse(combruom.Text.Trim());
					double CommAmt = num / UOM * CommRate;
					txtcommamount.Text = Math.Round(CommAmt).ToString();
				}
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
			TotalAmount();
			CalculateTaxAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRate_TextChanged(object sender, EventArgs e)
	{
		CalculateWeight();
		TotalAmount();
		TotalCommissionAmount();
		CalculateTaxAmount();
	}

	private void combrateuom_SelectedIndexChanged(object sender, EventArgs e)
	{
		CalculateWeight();
		TotalAmount();
		TotalCommissionAmount();
		CalculateTaxAmount();
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
				combitem.Value = Conversion.ToInt(item.Cells["ItemId"].Value);
				GetTaxTypeIdAndTaxPercent();
				bindRateUomAndItemPackUom(Conversion.ToInt(combitem.Value));
				ItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
				combitempck.Value = item.Cells["PackUomId"].Value.ToString();
				((Control)(object)combitempck).Text = item.Cells["PackUom"].Text.ToString();
				txtqty.Text = Conversion.ToDouble(item.Cells["ItemQty"].Value).ToString("#,##0.###");
				txtweight.Text = Conversion.ToDouble(item.Cells["Weight"].Value).ToString("#,##0.###");
				combrateuom.Value = item.Cells["RateUomId"].Value.ToString();
				((Control)(object)combrateuom).Text = item.Cells["RateUom"].Text.ToString();
				txtRate.Text = Conversion.ToDouble(item.Cells["ItemRate"].Value).ToString(clsGlobalVariables.DecimalRateFormate);
				txtItemAmount.Text = Conversion.ToDouble(item.Cells["ItemAmount"].Value).ToString(clsGlobalVariables.stringFormatsingle);
				combjob.Value = item.Cells["JobLotId"].Value.ToString();
				txtremarksdetail.Text = item.Cells["Remarks"].Value.ToString();
				CmbTaxType.Value = item.Cells["TaxTypeId"].Value.ToString();
				((Control)(object)CmbTaxType).Text = item.Cells["TaxType"].Value.ToString();
				txtTaxPercent.Text = item.Cells["TaxPercent"].Value.ToString();
				txtTaxAmount.Text = Conversion.ToDouble(item.Cells["TaxAmount"].Value).ToString(clsGlobalVariables.stringFormatsingle);
				txtTotalAmount.Text = Conversion.ToDouble(item.Cells["TotalAmount"].Value).ToString(clsGlobalVariables.stringFormatsingle);
				((Control)(object)CmbCity).Text = Conversion.ToString((object)item.Cells["CityName"].Text);
				btnplus.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
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
			if (!FormValidationDetail())
			{
				return;
			}
			double TaxPercent = 0.0;
			double TaxAmount = 0.0;
			double TotalAmount = 0.0;
			if (txtTaxPercent.Text != string.Empty)
			{
				TaxPercent = Conversion.ToDouble((object)txtTaxPercent.Text.Trim());
			}
			if (txtTaxAmount.Text != string.Empty)
			{
				TaxAmount = Conversion.ToDouble((object)txtTaxAmount.Text.Trim());
			}
			if (txtTotalAmount.Text != string.Empty)
			{
				TotalAmount = Conversion.ToDouble((object)txtTotalAmount.Text.Trim());
			}
			if (Conversion.ToInt((object)this.ItemId) != Conversion.ToInt(combitem.Value))
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					int ItemId = Conversion.ToInt(obj.Cells["ItemId"].Value);
					if (obj.RowIndex != updateDetailIndex && Conversion.ToInt(combitem.Value) == ItemId)
					{
						MessageBox.Show("Duplicate Item Not Add in Grid");
						return;
					}
				}
			}
			this.ItemId = 0;
			table.Rows[updateDetailIndex]["Id"] = DetailPoId;
			table.Rows[updateDetailIndex]["ItemId"] = combitem.Value;
			table.Rows[updateDetailIndex]["ItemName"] = ((Control)(object)combitem).Text;
			table.Rows[updateDetailIndex]["JobLotId"] = combjob.Value;
			table.Rows[updateDetailIndex]["JobLot"] = ((Control)(object)combjob).Text;
			table.Rows[updateDetailIndex]["PackUomId"] = combitempck.Value;
			table.Rows[updateDetailIndex]["PackUom"] = ((Control)(object)combitempck).Text;
			table.Rows[updateDetailIndex]["ItemQty"] = txtqty.Text;
			table.Rows[updateDetailIndex]["Weight"] = txtweight.Text;
			table.Rows[updateDetailIndex]["RateUomId"] = combrateuom.Value;
			table.Rows[updateDetailIndex]["RateUom"] = ((Control)(object)combrateuom).Text;
			table.Rows[updateDetailIndex]["ItemRate"] = txtRate.Text;
			table.Rows[updateDetailIndex]["ItemAmount"] = txtItemAmount.Text;
			table.Rows[updateDetailIndex]["Remarks"] = txtremarksdetail.Text;
			table.Rows[updateDetailIndex]["TaxTypeId"] = Conversion.ToInt(CmbTaxType.Value);
			table.Rows[updateDetailIndex]["TaxType"] = ((Control)(object)CmbTaxType).Text;
			table.Rows[updateDetailIndex]["TaxPercent"] = TaxPercent;
			table.Rows[updateDetailIndex]["TaxAmount"] = TaxAmount;
			table.Rows[updateDetailIndex]["TotalAmount"] = TotalAmount;
			table.Rows[updateDetailIndex]["CityName"] = ((Control)(object)CmbCity).Text;
			grd.DataSource = table;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			((Control)(object)CmbTaxType).Text = string.Empty;
			txtTaxPercent.Text = string.Empty;
			txtTaxAmount.Text = string.Empty;
			((Control)(object)combitem).Text = string.Empty;
			((Control)(object)combitempck).Text = string.Empty;
			txtqty.Text = string.Empty;
			txtweight.Text = string.Empty;
			txtRetailPrice.Text = string.Empty;
			((Control)(object)combrateuom).Text = string.Empty;
			txtRate.Text = string.Empty;
			txtItemAmount.Text = string.Empty;
			txtremarksdetail.Text = string.Empty;
			txtTotalAmount.Text = string.Empty;
			combitem.Focus();
			CommonServices.GridAutoAdjustment(AryColWidth, grd);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		((Control)(object)combitem).Text = string.Empty;
		((Control)(object)combitempck).Text = string.Empty;
		txtqty.Text = string.Empty;
		txtweight.Text = string.Empty;
		txtRetailPrice.Text = string.Empty;
		((Control)(object)combrateuom).Text = string.Empty;
		txtRate.Text = string.Empty;
		txtItemAmount.Text = string.Empty;
		((Control)(object)CmbTaxType).Text = string.Empty;
		txtTaxPercent.Text = string.Empty;
		txtTaxAmount.Text = string.Empty;
		txtremarksdetail.Text = string.Empty;
		txtTotalAmount.Text = string.Empty;
		((Control)(object)CmbCity).Text = string.Empty;
		combitem.Focus();
		IsTaxable = false;
		btnplus.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelUpdateDetial.Visible = false;
	}

	private void grdhistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Edit")
			{
				GridEXRow item3 = grdhistory.CurrentRow;
				if (item3 != null && (int)item3.RowType == 0)
				{
					int id2 = Conversion.ToInt(item3.Cells["Id"].Value);
					ReadById(id2);
				}
			}
			if (e.Column.Key == "Print")
			{
				GridEXRow item2 = grdhistory.CurrentRow;
				if (item2 != null && (int)item2.RowType == 0)
				{
					CommonServices.PurchaseOrderSlip_Engr1600(Conversion.ToInt(item2.Cells["Id"].Value));
				}
			}
			if (e.Column.Key == "SaveAs")
			{
				GridEXRow item = grdhistory.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					int id = Conversion.ToInt(item.Cells["Id"].Value);
					ReadById(id);
				}
				btnsave.Visible = false;
				btnupdate.Visible = false;
				BtnSaveAs.Visible = true;
			}
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

	private void txtCommrate_TextChanged(object sender, EventArgs e)
	{
		TotalCommissionAmount();
	}

	private void combcommtype_Leave(object sender, EventArgs e)
	{
		TotalCommissionAmount();
		combruom.SelectedIndex = 0;
	}

	private void combruom_Leave(object sender, EventArgs e)
	{
		TotalCommissionAmount();
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		TotalCommissionAmount();
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

	private void btnsearch_Click(object sender, EventArgs e)
	{
		gridhistoryfill();
	}

	private void btnprint_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				CommonServices.PurchaseOrderSlip_Engr1600(RecId);
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtdocno_TextChanged(object sender, EventArgs e)
	{
	}

	private void combitem_Leave_1(object sender, EventArgs e)
	{
		try
		{
			bindRateUomAndItemPackUom(Conversion.ToInt(combitem.Value));
			List<ConfigrationsAllocation> lstJob = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Job/Lot").ToList();
			if (lstJob.Count > 0)
			{
				int JobId = Conversion.ToInt((object)lstJob[0].ConfigKey.ToString());
				combjob.Value = JobId;
			}
			GetTaxTypeIdAndTaxPercent();
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
			((Control)(object)CmbTaxType).Text = string.Empty;
			((UltraGridBase)CmbTaxType).DataSource = null;
			txtTaxPercent.Text = "0";
			DataTable dt = CommonServices.GetTaxTypeIdAndPercentByItemId(Conversion.ToInt(combitem.Value), DocDate.Value);
			if (dt.Rows.Count > 0)
			{
				IsTaxable = true;
				DataTable dtTax = new DataTable();
				dtTax.Columns.Add("Id");
				dtTax.Columns.Add("TaxType");
				dtTax.Columns.Add("TaxPrcnt");
				dtTax.Rows.Add(dt.Rows[0]["TaxNameId"], dt.Rows[0]["TaxName"], dt.Rows[0]["TaxPercent"]);
				DDL.BindDDL(dtTax, CmbTaxType, "Id", "TaxType", "TaxType", ZeroIndex: false);
				((UltraGridBase)CmbTaxType).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbTaxType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			else
			{
				IsTaxable = false;
				txtTaxPercent.Text = "0";
				txtTaxAmount.Text = "0";
			}
			if (IsTaxable)
			{
				((Control)(object)CmbTaxType).Enabled = true;
				txtTaxPercent.Enabled = true;
				txtTaxAmount.Enabled = true;
				txtTotalAmount.Enabled = true;
			}
			else
			{
				((Control)(object)CmbTaxType).Enabled = false;
				txtTaxPercent.Enabled = false;
				txtTaxAmount.Enabled = false;
				txtTotalAmount.Enabled = false;
			}
			CalculateTaxAmount();
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

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		try
		{
			OrderCatagoryfill();
			CompanyFill();
			BranchFill();
			ProjectFill();
			SupplierNameFill();
			DeliveryTerm();
			if (IsTaxable)
			{
				TaxItemsBind();
			}
			else
			{
				ItemDetailFill();
			}
			combojoblotfill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbTaxType_TextChanged(object sender, EventArgs e)
	{
		CalculateTaxAmount();
	}

	private void txtTaxPercent_TextChanged(object sender, EventArgs e)
	{
		CalculateTaxAmount();
	}

	private void CmbTaxType_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbTaxType).ActiveRow == null)
			{
				txtTaxPercent.Text = "0";
				txtTaxAmount.Text = "0";
				txtTotalAmount.Text = Conversion.ToDouble((object)txtItemAmount.Text).ToString(clsGlobalVariables.stringFormatsingle);
			}
			if (((UltraGridBase)CmbTaxType).ActiveRow != null)
			{
				CalculateTaxAmount();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbWholeSale_SelectedIndexChanged(object sender, EventArgs e)
	{
		CalculateTaxAmount();
	}

	private void CmbAgreementNo_Leave(object sender, EventArgs e)
	{
	}

	private void combsuppname_Leave(object sender, EventArgs e)
	{
		ComboAgreementNoFill();
	}

	private void combrateuom_TextChanged(object sender, EventArgs e)
	{
		CalculateWeight();
		TotalAmount();
		TotalCommissionAmount();
		CalculateTaxAmount();
	}

	private void grdhistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdhistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 1600);
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

	private void DocDate_Leave(object sender, EventArgs e)
	{
		try
		{
			if (grd.RowCount > 0 && btnsave.Enabled && btnsave.Visible)
			{
				string ItemIds = "";
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					ItemIds = ItemIds + "," + r.Cells["ItemId"].Value;
				}
				DataTable dt = ChangedTaxPercentFromGridByDatechange(ItemIds);
				rows = grd.GetRows();
				foreach (GridEXRow item in rows)
				{
					item.BeginEdit();
					double TaxPercent = 0.0;
					double ItemAmount = 0.0;
					double TaxAmount = 0.0;
					double TotalAmount = 0.0;
					bool flag = false;
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						if (Conversion.ToInt(dt.Rows[i]["ItemId"]) == Conversion.ToInt(item.Cells["ItemId"].Value))
						{
							flag = true;
							item.Cells["TaxTypeId"].Value = Conversion.ToInt(dt.Rows[i]["TaxNameId"]);
							item.Cells["TaxType"].Value = Conversion.ToString(dt.Rows[i]["TaxName"]);
							TaxPercent = Conversion.ToDouble(dt.Rows[i]["TaxPercent"]);
							item.Cells["TaxPercent"].Value = TaxPercent;
							ItemAmount = Conversion.ToDouble(item.Cells["Amount"].Value);
							if (ItemAmount > 0.0 && TaxPercent > 0.0)
							{
								TaxAmount = ItemAmount * TaxPercent / 100.0;
								item.Cells["TaxAmount"].Value = TaxAmount;
							}
							TotalAmount = ItemAmount + TaxAmount;
							item.Cells["TotalAmount"].Value = TotalAmount;
						}
					}
					if (!flag)
					{
						item.Cells["TaxTypeId"].Value = 0;
						item.Cells["TaxType"].Value = 0;
						item.Cells["TaxPercent"].Value = 0;
						item.Cells["TaxAmount"].Value = 0;
						item.Cells["TotalAmount"].Value = item.Cells["ItemAmount"].Value;
					}
					item.EndEdit();
				}
			}
			if (deliverystartdate.Value.Date < DocDate.Value.Date)
			{
				deliverystartdate.Value = DocDate.Value;
				MessageBox.Show("Delivery Start Date Can't Be Less Than Doc Date");
			}
			else
			{
				deliverystartdate.Value = DocDate.Value;
			}
			txtduedays_TextChanged(null, null);
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
			GridEXRow item = grdhistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				int id = Conversion.ToInt(item.Cells["Id"].Value);
				GetDetailByHeaderId(id);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdDetail).Name);
			if (fs != null)
			{
				grdDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
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
			if (((Control)(object)combpttrm).Text == "Cash")
			{
				txtduedays.Enabled = false;
				duedate.Enabled = false;
				txtduedays.Text = string.Empty;
				duedate.Value = DateTime.Now;
			}
			else
			{
				txtduedays.Enabled = true;
				duedate.Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtduedays_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtdeliverydays_KeyPress(object sender, KeyPressEventArgs e)
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

	private void BtnSaveAs_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void deliverystartdate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (deliverystartdate.Value.Date < DocDate.Value.Date)
			{
				deliverystartdate.Value = DocDate.Value;
				MessageBox.Show("Delivery Start Date Can't Be Less Than Doc Date");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void duedate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (duedate.Value.Date < DocDate.Value.Date)
			{
				duedate.Value = DocDate.Value;
			}
			int days = (int)(duedate.Value.Date - DocDate.Value.Date).TotalDays;
			txtduedays.Text = days.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void PurchsaeOrderWithQty_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now;
			ToDateHistory.Value = DateTime.Now;
			txtFromDocNoHistory.Text = string.Empty;
			txtToDocNoHistory.Text = string.Empty;
			((Control)(object)cmbSupplierNameHistory).Text = string.Empty;
			grdhistory.ClearStructure();
			grdDetail.ClearStructure();
			rdpqtydoc.Checked = true;
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

	private void btnshow_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name != null && base.ActiveControl.Name == "grdhistory")
			{
				FromDateHistory.Focus();
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
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Expected O, but got Unknown
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Expected O, but got Unknown
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Expected O, but got Unknown
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Expected O, but got Unknown
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04da: Expected O, but got Unknown
		//IL_04da: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Expected O, but got Unknown
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Expected O, but got Unknown
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Expected O, but got Unknown
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Expected O, but got Unknown
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Expected O, but got Unknown
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Expected O, but got Unknown
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Expected O, but got Unknown
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0512: Expected O, but got Unknown
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Expected O, but got Unknown
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Expected O, but got Unknown
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_0527: Expected O, but got Unknown
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_052e: Expected O, but got Unknown
		//IL_052e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Expected O, but got Unknown
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Expected O, but got Unknown
		//IL_053c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Expected O, but got Unknown
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Expected O, but got Unknown
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Expected O, but got Unknown
		//IL_0551: Unknown result type (might be due to invalid IL or missing references)
		//IL_0558: Expected O, but got Unknown
		//IL_0558: Unknown result type (might be due to invalid IL or missing references)
		//IL_055f: Expected O, but got Unknown
		//IL_055f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Expected O, but got Unknown
		//IL_0566: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Expected O, but got Unknown
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Expected O, but got Unknown
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Expected O, but got Unknown
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Expected O, but got Unknown
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Expected O, but got Unknown
		//IL_0589: Unknown result type (might be due to invalid IL or missing references)
		//IL_0590: Expected O, but got Unknown
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Expected O, but got Unknown
		//IL_0597: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Expected O, but got Unknown
		//IL_059e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Expected O, but got Unknown
		//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ac: Expected O, but got Unknown
		//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b3: Expected O, but got Unknown
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Expected O, but got Unknown
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c1: Expected O, but got Unknown
		//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c8: Expected O, but got Unknown
		//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cf: Expected O, but got Unknown
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d6: Expected O, but got Unknown
		//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dd: Expected O, but got Unknown
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e4: Expected O, but got Unknown
		//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Expected O, but got Unknown
		//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Expected O, but got Unknown
		//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f9: Expected O, but got Unknown
		//IL_0626: Unknown result type (might be due to invalid IL or missing references)
		//IL_0630: Expected O, but got Unknown
		//IL_0668: Unknown result type (might be due to invalid IL or missing references)
		//IL_0672: Expected O, but got Unknown
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0688: Expected O, but got Unknown
		//IL_0694: Unknown result type (might be due to invalid IL or missing references)
		//IL_069e: Expected O, but got Unknown
		//IL_0702: Unknown result type (might be due to invalid IL or missing references)
		//IL_070c: Expected O, but got Unknown
		//IL_0718: Unknown result type (might be due to invalid IL or missing references)
		//IL_0722: Expected O, but got Unknown
		//IL_072e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0738: Expected O, but got Unknown
		//IL_07e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f3: Expected O, but got Unknown
		//IL_07ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0809: Expected O, but got Unknown
		//IL_0820: Unknown result type (might be due to invalid IL or missing references)
		//IL_082a: Expected O, but got Unknown
		//IL_082b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0835: Expected O, but got Unknown
		//IL_0836: Unknown result type (might be due to invalid IL or missing references)
		//IL_0840: Expected O, but got Unknown
		//IL_0841: Unknown result type (might be due to invalid IL or missing references)
		//IL_084b: Expected O, but got Unknown
		//IL_084c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0856: Expected O, but got Unknown
		//IL_0883: Unknown result type (might be due to invalid IL or missing references)
		//IL_088d: Expected O, but got Unknown
		//IL_0975: Unknown result type (might be due to invalid IL or missing references)
		//IL_097f: Expected O, but got Unknown
		//IL_098b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0995: Expected O, but got Unknown
		//IL_0ae0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aea: Expected O, but got Unknown
		//IL_0b0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b16: Expected O, but got Unknown
		//IL_0b90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9a: Expected O, but got Unknown
		//IL_0bfe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c08: Expected O, but got Unknown
		//IL_11ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_11f8: Expected O, but got Unknown
		//IL_1205: Unknown result type (might be due to invalid IL or missing references)
		//IL_120f: Expected O, but got Unknown
		//IL_93c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_93cd: Expected O, but got Unknown
		//IL_93f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_93fb: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
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
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
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
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
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
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
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
		this.grd = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label16 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.txtEqivalent = new System.Windows.Forms.TextBox();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.CmbCity = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.CmbTaxType = new UltraCombo();
		this.lblTotalAmount = new System.Windows.Forms.Label();
		this.combrateuom = new UltraCombo();
		this.txtTaxPercent = new System.Windows.Forms.TextBox();
		this.txtremarksdetail = new System.Windows.Forms.TextBox();
		this.label47 = new System.Windows.Forms.Label();
		this.txtTotalAmount = new System.Windows.Forms.TextBox();
		this.lblTaxPercent = new System.Windows.Forms.Label();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.txtTaxAmount = new System.Windows.Forms.TextBox();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.lblTaxTypes = new System.Windows.Forms.Label();
		this.combjob = new UltraCombo();
		this.lblTaxAmount = new System.Windows.Forms.Label();
		this.combitempck = new UltraCombo();
		this.label34 = new System.Windows.Forms.Label();
		this.combitem = new UltraCombo();
		this.btnplus = new System.Windows.Forms.Button();
		this.txtRate = new System.Windows.Forms.TextBox();
		this.txtItemAmount = new System.Windows.Forms.TextBox();
		this.txtweight = new System.Windows.Forms.TextBox();
		this.txtqty = new System.Windows.Forms.TextBox();
		this.label40 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.txtRetailPrice = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.txtBagPrice = new System.Windows.Forms.TextBox();
		this.CmbWholeSale = new UltraCombo();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.CmbBillType = new UltraCombo();
		this.label22 = new System.Windows.Forms.Label();
		this.txtremarks = new System.Windows.Forms.RichTextBox();
		this.CmbStatus = new UltraCombo();
		this.combdeliverytrm = new UltraCombo();
		this.combpttrm = new UltraCombo();
		this.cmbbranch1 = new UltraCombo();
		this.combproject1 = new UltraCombo();
		this.ChkBoxPrint = new System.Windows.Forms.CheckBox();
		this.label26 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.txtduedays = new System.Windows.Forms.TextBox();
		this.combsuppname = new UltraCombo();
		this.deliverystartdate = new System.Windows.Forms.DateTimePicker();
		this.label33 = new System.Windows.Forms.Label();
		this.duedate = new System.Windows.Forms.DateTimePicker();
		this.label30 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.txtsupprefno = new System.Windows.Forms.TextBox();
		this.label28 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.txtdeliverydays = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.label17 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.combruom = new System.Windows.Forms.ComboBox();
		this.label14 = new System.Windows.Forms.Label();
		this.combamounttype = new System.Windows.Forms.ComboBox();
		this.label25 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
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
		this.txtcatsr = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.combordercat = new System.Windows.Forms.ComboBox();
		this.validate = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.BtnSaveAs = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdhistory = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.rdpqtymodify = new System.Windows.Forms.RadioButton();
		this.rdpqtydoc = new System.Windows.Forms.RadioButton();
		this.rdpqtyentry = new System.Windows.Forms.RadioButton();
		this.btnshow = new System.Windows.Forms.Button();
		this.label59 = new System.Windows.Forms.Label();
		this.label60 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.label63 = new System.Windows.Forms.Label();
		this.cmbSupplierNameHistory = new UltraCombo();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.combsalesman = new UltraCombo();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel14.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combrateuom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combjob).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combitempck).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combitem).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWholeSale).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBillType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combdeliverytrm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combpttrm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbranch1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combproject1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combsuppname).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combcomp1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAgreementNo).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel9.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel12.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierNameHistory).BeginInit();
		this.panel7.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.combsalesman).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1140, 680);
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
		this.tabControl1.Size = new System.Drawing.Size(1140, 680);
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
		this.tabPage1.Size = new System.Drawing.Size(1132, 651);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel6.Controls.Add(this.panel14);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(2, 338);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1128, 311);
		this.panel6.TabIndex = 4;
		this.grd.AllowDelete = (InheritableBoolean)1;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1128, 285);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.grd).TabStop = false;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.label16);
		this.panel14.Controls.Add(this.ctrlGrdBar1);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1128, 26);
		this.panel14.TabIndex = 0;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.White;
		this.label16.Location = new System.Drawing.Point(3, 3);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(53, 18);
		this.label16.TabIndex = 25;
		this.label16.Text = "Detail";
		this.panel5.Controls.Add(this.txtEqivalent);
		this.panel5.Controls.Add(this.groupBox3);
		this.panel5.Controls.Add(this.txtRetailPrice);
		this.panel5.Controls.Add(this.label10);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(2, 215);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1128, 123);
		this.panel5.TabIndex = 3;
		this.txtEqivalent.Location = new System.Drawing.Point(663, 246);
		this.txtEqivalent.Name = "txtEqivalent";
		this.txtEqivalent.Size = new System.Drawing.Size(100, 23);
		this.txtEqivalent.TabIndex = 42;
		this.txtEqivalent.TabStop = false;
		this.txtEqivalent.Visible = false;
		this.groupBox3.BackColor = System.Drawing.Color.White;
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCity);
		this.groupBox3.Controls.Add(this.label5);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTaxType);
		this.groupBox3.Controls.Add(this.lblTotalAmount);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.combrateuom);
		this.groupBox3.Controls.Add(this.txtTaxPercent);
		this.groupBox3.Controls.Add(this.txtremarksdetail);
		this.groupBox3.Controls.Add(this.label47);
		this.groupBox3.Controls.Add(this.txtTotalAmount);
		this.groupBox3.Controls.Add(this.lblTaxPercent);
		this.groupBox3.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox3.Controls.Add(this.txtTaxAmount);
		this.groupBox3.Controls.Add(this.btnUpdateDetail);
		this.groupBox3.Controls.Add(this.lblTaxTypes);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.combjob);
		this.groupBox3.Controls.Add(this.lblTaxAmount);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.combitempck);
		this.groupBox3.Controls.Add(this.label34);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.combitem);
		this.groupBox3.Controls.Add(this.btnplus);
		this.groupBox3.Controls.Add(this.txtRate);
		this.groupBox3.Controls.Add(this.txtItemAmount);
		this.groupBox3.Controls.Add(this.txtweight);
		this.groupBox3.Controls.Add(this.txtqty);
		this.groupBox3.Controls.Add(this.label40);
		this.groupBox3.Controls.Add(this.label44);
		this.groupBox3.Controls.Add(this.label41);
		this.groupBox3.Controls.Add(this.label39);
		this.groupBox3.Controls.Add(this.label38);
		this.groupBox3.Controls.Add(this.label36);
		this.groupBox3.Controls.Add(this.label37);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(4, -15);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1120, 133);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Detail";
		this.CmbCity.AllowNull = (DefaultableBoolean)1;
		this.CmbCity.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance201).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.CmbCity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance202).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance202).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance202).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance202;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance203;
		((SpecialBoxBase)((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance204).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance204).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.CmbCity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance205).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance207).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance208;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance209).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance210).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance210).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance210).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance210).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance210;
		((AppearanceBase)appearance211).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance213;
		((UltraGridBase)this.CmbCity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCity).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCity).Location = new System.Drawing.Point(343, 94);
		((System.Windows.Forms.Control)(object)this.CmbCity).Name = "CmbCity";
		((System.Windows.Forms.Control)(object)this.CmbCity).Size = new System.Drawing.Size(172, 26);
		((System.Windows.Forms.Control)(object)this.CmbCity).TabIndex = 12;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(346, 75);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(62, 15);
		this.label5.TabIndex = 37;
		this.label5.Text = "City Name";
		this.CmbTaxType.AllowNull = (DefaultableBoolean)1;
		this.CmbTaxType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTaxType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTaxType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance175).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance176).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance176).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance177;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance178).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance178).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance178).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance179).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance182).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance183).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance183).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance183).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance183;
		((AppearanceBase)appearance185).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTaxType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Location = new System.Drawing.Point(6, 94);
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Name = "CmbTaxType";
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Size = new System.Drawing.Size(82, 26);
		((System.Windows.Forms.Control)(object)this.CmbTaxType).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.CmbTaxType).TextChanged += new System.EventHandler(CmbTaxType_TextChanged);
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Leave += new System.EventHandler(CmbTaxType_Leave);
		this.lblTotalAmount.AutoSize = true;
		this.lblTotalAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalAmount.Location = new System.Drawing.Point(234, 75);
		this.lblTotalAmount.Name = "lblTotalAmount";
		this.lblTotalAmount.Size = new System.Drawing.Size(80, 15);
		this.lblTotalAmount.TabIndex = 35;
		this.lblTotalAmount.Text = "Total Amount";
		this.combrateuom.AllowNull = (DefaultableBoolean)1;
		this.combrateuom.AutoCompleteMode = (AutoCompleteMode)4;
		this.combrateuom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combrateuom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combrateuom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.combrateuom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combrateuom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combrateuom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combrateuom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance137;
		((SpecialBoxBase)((UltraGridBase)this.combrateuom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combrateuom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.combrateuom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combrateuom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance143).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance144).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance144).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance144).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).TextHAlignAsString = "Left";
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.combrateuom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combrateuom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combrateuom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combrateuom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combrateuom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combrateuom).Location = new System.Drawing.Point(893, 45);
		((System.Windows.Forms.Control)(object)this.combrateuom).Name = "combrateuom";
		((System.Windows.Forms.Control)(object)this.combrateuom).Size = new System.Drawing.Size(93, 26);
		((System.Windows.Forms.Control)(object)this.combrateuom).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.combrateuom).TextChanged += new System.EventHandler(combrateuom_TextChanged);
		this.txtTaxPercent.BackColor = System.Drawing.Color.White;
		this.txtTaxPercent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTaxPercent.Location = new System.Drawing.Point(90, 96);
		this.txtTaxPercent.Name = "txtTaxPercent";
		this.txtTaxPercent.ReadOnly = true;
		this.txtTaxPercent.Size = new System.Drawing.Size(62, 23);
		this.txtTaxPercent.TabIndex = 9;
		this.txtTaxPercent.TabStop = false;
		this.txtTaxPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTaxPercent.TextChanged += new System.EventHandler(txtTaxPercent_TextChanged);
		this.txtremarksdetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarksdetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarksdetail.Location = new System.Drawing.Point(516, 96);
		this.txtremarksdetail.Name = "txtremarksdetail";
		this.txtremarksdetail.Size = new System.Drawing.Size(432, 23);
		this.txtremarksdetail.TabIndex = 13;
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(515, 75);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(52, 15);
		this.label47.TabIndex = 25;
		this.label47.Text = "Remarks";
		this.txtTotalAmount.BackColor = System.Drawing.Color.White;
		this.txtTotalAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalAmount.Location = new System.Drawing.Point(237, 96);
		this.txtTotalAmount.Name = "txtTotalAmount";
		this.txtTotalAmount.ReadOnly = true;
		this.txtTotalAmount.Size = new System.Drawing.Size(104, 23);
		this.txtTotalAmount.TabIndex = 11;
		this.txtTotalAmount.TabStop = false;
		this.txtTotalAmount.Text = "0";
		this.txtTotalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.lblTaxPercent.AutoSize = true;
		this.lblTaxPercent.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTaxPercent.Location = new System.Drawing.Point(92, 75);
		this.lblTaxPercent.Name = "lblTaxPercent";
		this.lblTaxPercent.Size = new System.Drawing.Size(38, 15);
		this.lblTaxPercent.TabIndex = 31;
		this.lblTaxPercent.Text = "Tax %";
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(1042, 95);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(65, 25);
		this.btnCancelUpdateDetial.TabIndex = 16;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.txtTaxAmount.BackColor = System.Drawing.Color.White;
		this.txtTaxAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTaxAmount.Location = new System.Drawing.Point(154, 96);
		this.txtTaxAmount.Name = "txtTaxAmount";
		this.txtTaxAmount.ReadOnly = true;
		this.txtTaxAmount.Size = new System.Drawing.Size(82, 23);
		this.txtTaxAmount.TabIndex = 10;
		this.txtTaxAmount.TabStop = false;
		this.txtTaxAmount.Text = "0";
		this.txtTaxAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(978, 95);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(63, 25);
		this.btnUpdateDetail.TabIndex = 15;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.lblTaxTypes.AutoSize = true;
		this.lblTaxTypes.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTaxTypes.Location = new System.Drawing.Point(8, 75);
		this.lblTaxTypes.Name = "lblTaxTypes";
		this.lblTaxTypes.Size = new System.Drawing.Size(53, 15);
		this.lblTaxTypes.TabIndex = 23;
		this.lblTaxTypes.Text = "Tax Type";
		this.combjob.AllowNull = (DefaultableBoolean)1;
		this.combjob.AutoCompleteMode = (AutoCompleteMode)4;
		this.combjob.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combjob.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combjob).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.combjob).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combjob).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combjob).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combjob).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.combjob).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combjob).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.combjob).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combjob).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combjob).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combjob).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.combjob).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combjob).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combjob).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combjob).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.combjob).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combjob).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combjob).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.combjob).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.combjob).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combjob).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combjob).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.combjob).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combjob).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.combjob).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combjob).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combjob).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combjob).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combjob.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combjob).Location = new System.Drawing.Point(343, 45);
		((System.Windows.Forms.Control)(object)this.combjob).Name = "combjob";
		((System.Windows.Forms.Control)(object)this.combjob).Size = new System.Drawing.Size(172, 26);
		((System.Windows.Forms.Control)(object)this.combjob).TabIndex = 1;
		this.lblTaxAmount.AutoSize = true;
		this.lblTaxAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTaxAmount.Location = new System.Drawing.Point(152, 75);
		this.lblTaxAmount.Name = "lblTaxAmount";
		this.lblTaxAmount.Size = new System.Drawing.Size(72, 15);
		this.lblTaxAmount.TabIndex = 33;
		this.lblTaxAmount.Text = "Tax Amount";
		this.combitempck.AllowNull = (DefaultableBoolean)1;
		this.combitempck.AutoCompleteMode = (AutoCompleteMode)4;
		this.combitempck.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combitempck.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combitempck).DisplayLayout.Appearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.combitempck).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combitempck).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance189).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance189).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance189).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combitempck).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitempck).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance190;
		((SpecialBoxBase)((UltraGridBase)this.combitempck).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance191).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance191).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitempck).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.combitempck).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combitempck).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance192).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance193).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance196).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance197).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance197).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance197).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance197;
		((AppearanceBase)appearance198).TextHAlignAsString = "Left";
		((UltraGridBase)this.combitempck).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.combitempck).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combitempck).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combitempck).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combitempck).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combitempck.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combitempck).Location = new System.Drawing.Point(516, 45);
		((System.Windows.Forms.Control)(object)this.combitempck).Name = "combitempck";
		((System.Windows.Forms.Control)(object)this.combitempck).Size = new System.Drawing.Size(85, 26);
		((System.Windows.Forms.Control)(object)this.combitempck).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.combitempck).Leave += new System.EventHandler(combitempck_Leave);
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(346, 25);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(46, 15);
		this.label34.TabIndex = 23;
		this.label34.Text = "Job Lot";
		this.combitem.AllowNull = (DefaultableBoolean)1;
		this.combitem.AutoCompleteMode = (AutoCompleteMode)4;
		this.combitem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combitem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combitem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.combitem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combitem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance163).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance163).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combitem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance164;
		((SpecialBoxBase)((UltraGridBase)this.combitem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance165).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance165).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance165).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.combitem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combitem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance166).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combitem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance166;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combitem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.combitem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combitem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance168;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance169).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance171).TextHAlignAsString = "Left";
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combitem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.combitem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combitem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.combitem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combitem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combitem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combitem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combitem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combitem).Location = new System.Drawing.Point(7, 45);
		((System.Windows.Forms.Control)(object)this.combitem).Name = "combitem";
		((System.Windows.Forms.Control)(object)this.combitem).Size = new System.Drawing.Size(335, 26);
		((System.Windows.Forms.Control)(object)this.combitem).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.combitem).Leave += new System.EventHandler(combitem_Leave_1);
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(950, 95);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(26, 25);
		this.btnplus.TabIndex = 14;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.txtRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRate.Location = new System.Drawing.Point(803, 47);
		this.txtRate.Name = "txtRate";
		this.txtRate.Size = new System.Drawing.Size(88, 23);
		this.txtRate.TabIndex = 5;
		this.txtRate.Text = "0";
		this.txtRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRate.TextChanged += new System.EventHandler(txtRate_TextChanged);
		this.txtRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRate_KeyPress);
		this.txtItemAmount.BackColor = System.Drawing.Color.White;
		this.txtItemAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemAmount.Location = new System.Drawing.Point(988, 47);
		this.txtItemAmount.Name = "txtItemAmount";
		this.txtItemAmount.ReadOnly = true;
		this.txtItemAmount.Size = new System.Drawing.Size(119, 23);
		this.txtItemAmount.TabIndex = 7;
		this.txtItemAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtweight.Enabled = false;
		this.txtweight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtweight.Location = new System.Drawing.Point(703, 47);
		this.txtweight.Name = "txtweight";
		this.txtweight.Size = new System.Drawing.Size(98, 23);
		this.txtweight.TabIndex = 4;
		this.txtweight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtqty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtqty.Location = new System.Drawing.Point(603, 47);
		this.txtqty.Name = "txtqty";
		this.txtqty.Size = new System.Drawing.Size(98, 23);
		this.txtqty.TabIndex = 3;
		this.txtqty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtqty.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.txtqty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(803, 25);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(58, 15);
		this.label40.TabIndex = 17;
		this.label40.Text = "Item Rate";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(989, 25);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(79, 15);
		this.label44.TabIndex = 23;
		this.label44.Text = "Item Amount";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(893, 25);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(61, 15);
		this.label41.TabIndex = 19;
		this.label41.Text = "Rate UOM";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(706, 25);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(46, 15);
		this.label39.TabIndex = 15;
		this.label39.Text = "Weight";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(604, 25);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(54, 15);
		this.label38.TabIndex = 13;
		this.label38.Text = "Item Qty";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(517, 25);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(61, 15);
		this.label36.TabIndex = 8;
		this.label36.Text = "Pack Uom";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(8, 25);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(67, 15);
		this.label37.TabIndex = 9;
		this.label37.Text = "Item Name";
		this.txtRetailPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRetailPrice.Location = new System.Drawing.Point(787, 246);
		this.txtRetailPrice.Name = "txtRetailPrice";
		this.txtRetailPrice.Size = new System.Drawing.Size(74, 21);
		this.txtRetailPrice.TabIndex = 6;
		this.txtRetailPrice.Text = "0";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(787, 221);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(62, 15);
		this.label10.TabIndex = 41;
		this.label10.Text = "Retail Rate";
		this.panel4.Controls.Add(this.txtBagPrice);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWholeSale);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add(this.combruom);
		this.panel4.Controls.Add(this.label14);
		this.panel4.Controls.Add(this.combamounttype);
		this.panel4.Controls.Add(this.label25);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.label42);
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
		this.panel4.Controls.Add(this.txtcatsr);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add(this.combordercat);
		this.panel4.Controls.Add(this.validate);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add(this.label27);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 54);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1128, 161);
		this.panel4.TabIndex = 2;
		this.txtBagPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBagPrice.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBagPrice.Location = new System.Drawing.Point(1649, 192);
		this.txtBagPrice.Name = "txtBagPrice";
		this.txtBagPrice.Size = new System.Drawing.Size(74, 25);
		this.txtBagPrice.TabIndex = 9;
		this.txtBagPrice.Visible = false;
		this.CmbWholeSale.AllowNull = (DefaultableBoolean)1;
		this.CmbWholeSale.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWholeSale.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWholeSale.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWholeSale).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbWholeSale).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWholeSale).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWholeSale).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWholeSale).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.CmbWholeSale).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWholeSale).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbWholeSale).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWholeSale).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWholeSale).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWholeSale).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbWholeSale).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWholeSale).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWholeSale).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance114).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWholeSale).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbWholeSale).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWholeSale).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWholeSale).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWholeSale).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbWholeSale).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWholeSale).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWholeSale).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbWholeSale).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWholeSale).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbWholeSale).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWholeSale).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWholeSale).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWholeSale).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWholeSale.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWholeSale).Location = new System.Drawing.Point(924, 213);
		((System.Windows.Forms.Control)(object)this.CmbWholeSale).Name = "CmbWholeSale";
		((System.Windows.Forms.Control)(object)this.CmbWholeSale).Size = new System.Drawing.Size(132, 24);
		((System.Windows.Forms.Control)(object)this.CmbWholeSale).TabIndex = 14;
		((System.Windows.Forms.Control)(object)this.CmbWholeSale).TextChanged += new System.EventHandler(CmbWholeSale_SelectedIndexChanged);
		this.groupBox1.BackColor = System.Drawing.Color.White;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBillType);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add(this.txtremarks);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbStatus);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combdeliverytrm);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combpttrm);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbbranch1);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combproject1);
		this.groupBox1.Controls.Add(this.ChkBoxPrint);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add(this.label31);
		this.groupBox1.Controls.Add(this.txtduedays);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combsuppname);
		this.groupBox1.Controls.Add(this.deliverystartdate);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.duedate);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.txtsupprefno);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add(this.txtdeliverydays);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add(this.label18);
		this.groupBox1.Controls.Add(this.DocDate);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.txtdocno);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 4);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1120, 154);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.CmbBillType.AllowNull = (DefaultableBoolean)1;
		this.CmbBillType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBillType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBillType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbBillType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBillType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBillType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBillType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.CmbBillType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBillType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbBillType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBillType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbBillType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBillType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBillType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBillType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBillType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBillType).Location = new System.Drawing.Point(438, 13);
		((System.Windows.Forms.Control)(object)this.CmbBillType).Name = "CmbBillType";
		((System.Windows.Forms.Control)(object)this.CmbBillType).Size = new System.Drawing.Size(210, 26);
		((System.Windows.Forms.Control)(object)this.CmbBillType).TabIndex = 6;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(338, 19);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(51, 15);
		this.label22.TabIndex = 16;
		this.label22.Text = "Bill Type";
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(438, 97);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(574, 52);
		this.txtremarks.TabIndex = 10;
		this.txtremarks.Text = "";
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
		((System.Windows.Forms.Control)(object)this.CmbStatus).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbStatus).Location = new System.Drawing.Point(803, 68);
		((System.Windows.Forms.Control)(object)this.CmbStatus).Name = "CmbStatus";
		((System.Windows.Forms.Control)(object)this.CmbStatus).Size = new System.Drawing.Size(209, 26);
		((System.Windows.Forms.Control)(object)this.CmbStatus).TabIndex = 14;
		this.combdeliverytrm.AllowNull = (DefaultableBoolean)1;
		this.combdeliverytrm.AutoCompleteMode = (AutoCompleteMode)4;
		this.combdeliverytrm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combdeliverytrm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combdeliverytrm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Location = new System.Drawing.Point(803, 13);
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Name = "combdeliverytrm";
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Size = new System.Drawing.Size(209, 26);
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).TabIndex = 11;
		this.combpttrm.AllowNull = (DefaultableBoolean)1;
		this.combpttrm.AutoCompleteMode = (AutoCompleteMode)4;
		this.combpttrm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combpttrm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combpttrm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.combpttrm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combpttrm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.combpttrm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combpttrm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.combpttrm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combpttrm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combpttrm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combpttrm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combpttrm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combpttrm).Location = new System.Drawing.Point(438, 41);
		((System.Windows.Forms.Control)(object)this.combpttrm).Name = "combpttrm";
		((System.Windows.Forms.Control)(object)this.combpttrm).Size = new System.Drawing.Size(210, 26);
		((System.Windows.Forms.Control)(object)this.combpttrm).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.combpttrm).TextChanged += new System.EventHandler(combpttrm_TextChanged);
		this.cmbbranch1.AllowNull = (DefaultableBoolean)1;
		this.cmbbranch1.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbbranch1.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbbranch1.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Appearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbbranch1).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance150;
		((SpecialBoxBase)((UltraGridBase)this.cmbbranch1).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance152).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance152).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance156).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance157).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance157).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance157).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance158).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbbranch1).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbbranch1.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbbranch1).Location = new System.Drawing.Point(99, 13);
		((System.Windows.Forms.Control)(object)this.cmbbranch1).Name = "cmbbranch1";
		((System.Windows.Forms.Control)(object)this.cmbbranch1).Size = new System.Drawing.Size(233, 26);
		((System.Windows.Forms.Control)(object)this.cmbbranch1).TabIndex = 0;
		this.combproject1.AllowNull = (DefaultableBoolean)1;
		this.combproject1.AutoCompleteMode = (AutoCompleteMode)4;
		this.combproject1.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combproject1.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combproject1).DisplayLayout.Appearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.combproject1).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combproject1).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combproject1).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combproject1).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance121;
		((SpecialBoxBase)((UltraGridBase)this.combproject1).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combproject1).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.combproject1).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combproject1).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance127).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).TextHAlignAsString = "Left";
		((UltraGridBase)this.combproject1).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.combproject1).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combproject1).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combproject1).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combproject1).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combproject1.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combproject1).Location = new System.Drawing.Point(99, 41);
		((System.Windows.Forms.Control)(object)this.combproject1).Name = "combproject1";
		((System.Windows.Forms.Control)(object)this.combproject1).Size = new System.Drawing.Size(233, 26);
		((System.Windows.Forms.Control)(object)this.combproject1).TabIndex = 1;
		this.ChkBoxPrint.AutoSize = true;
		this.ChkBoxPrint.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBoxPrint.Location = new System.Drawing.Point(1017, 125);
		this.ChkBoxPrint.Name = "ChkBoxPrint";
		this.ChkBoxPrint.Size = new System.Drawing.Size(95, 19);
		this.ChkBoxPrint.TabIndex = 15;
		this.ChkBoxPrint.Text = "Print Preview";
		this.ChkBoxPrint.UseVisualStyleBackColor = true;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(338, 47);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(89, 15);
		this.label26.TabIndex = 8;
		this.label26.Text = "Payment Terms";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(654, 74);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(40, 15);
		this.label31.TabIndex = 8;
		this.label31.Text = "Status";
		this.txtduedays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtduedays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtduedays.Location = new System.Drawing.Point(438, 70);
		this.txtduedays.Name = "txtduedays";
		this.txtduedays.Size = new System.Drawing.Size(78, 23);
		this.txtduedays.TabIndex = 8;
		this.txtduedays.TextChanged += new System.EventHandler(txtduedays_TextChanged);
		this.txtduedays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtduedays_KeyPress);
		this.combsuppname.AllowNull = (DefaultableBoolean)1;
		this.combsuppname.AutoCompleteMode = (AutoCompleteMode)4;
		this.combsuppname.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combsuppname.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combsuppname).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.combsuppname).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combsuppname).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combsuppname).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combsuppname).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.combsuppname).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combsuppname).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.combsuppname).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combsuppname).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.combsuppname).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combsuppname).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combsuppname).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combsuppname).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combsuppname.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combsuppname).Location = new System.Drawing.Point(98, 97);
		((System.Windows.Forms.Control)(object)this.combsuppname).Name = "combsuppname";
		((System.Windows.Forms.Control)(object)this.combsuppname).Size = new System.Drawing.Size(233, 26);
		((System.Windows.Forms.Control)(object)this.combsuppname).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.combsuppname).Leave += new System.EventHandler(combsuppname_Leave);
		this.deliverystartdate.CustomFormat = "dd-MMM-yyyy";
		this.deliverystartdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.deliverystartdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.deliverystartdate.Location = new System.Drawing.Point(883, 43);
		this.deliverystartdate.Name = "deliverystartdate";
		this.deliverystartdate.Size = new System.Drawing.Size(129, 23);
		this.deliverystartdate.TabIndex = 13;
		this.deliverystartdate.ValueChanged += new System.EventHandler(deliverystartdate_ValueChanged);
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(337, 103);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(52, 15);
		this.label33.TabIndex = 8;
		this.label33.Text = "Remarks";
		this.duedate.CustomFormat = "dd-MMM-yyyy";
		this.duedate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.duedate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.duedate.Location = new System.Drawing.Point(517, 70);
		this.duedate.Name = "duedate";
		this.duedate.Size = new System.Drawing.Size(129, 23);
		this.duedate.TabIndex = 9;
		this.duedate.ValueChanged += new System.EventHandler(duedate_ValueChanged);
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(655, 19);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(80, 15);
		this.label30.TabIndex = 8;
		this.label30.Text = "Delivery Term";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(4, 103);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(86, 15);
		this.label12.TabIndex = 0;
		this.label12.Text = "Supplier Name";
		this.txtsupprefno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsupprefno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtsupprefno.Location = new System.Drawing.Point(98, 126);
		this.txtsupprefno.Name = "txtsupprefno";
		this.txtsupprefno.Size = new System.Drawing.Size(232, 23);
		this.txtsupprefno.TabIndex = 5;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(655, 47);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(147, 15);
		this.label28.TabIndex = 0;
		this.label28.Text = "Delivery Days && Start Date";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(4, 130);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(72, 15);
		this.label8.TabIndex = 3;
		this.label8.Text = "Refrence No";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(337, 74);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(98, 15);
		this.label29.TabIndex = 3;
		this.label29.Text = "Due Days && Date";
		this.txtdeliverydays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdeliverydays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdeliverydays.Location = new System.Drawing.Point(803, 43);
		this.txtdeliverydays.Name = "txtdeliverydays";
		this.txtdeliverydays.Size = new System.Drawing.Size(78, 23);
		this.txtdeliverydays.TabIndex = 12;
		this.txtdeliverydays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtdeliverydays_KeyPress);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(4, 19);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(79, 15);
		this.label13.TabIndex = 0;
		this.label13.Text = "Branch Name";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(4, 47);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(79, 15);
		this.label18.TabIndex = 0;
		this.label18.Text = "Project Name";
		this.DocDate.CustomFormat = "dd-MMM-yyyy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(195, 70);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(135, 23);
		this.DocDate.TabIndex = 3;
		this.DocDate.Leave += new System.EventHandler(DocDate_Leave);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(4, 74);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(88, 15);
		this.label17.TabIndex = 3;
		this.label17.Text = "Doc No && Date";
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(99, 70);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(94, 23);
		this.txtdocno.TabIndex = 2;
		this.txtdocno.TabStop = false;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.combruom.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.combruom.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.combruom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.combruom.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combruom.FormattingEnabled = true;
		this.combruom.Items.AddRange(new object[4] { "40", "50", "60", "100" });
		this.combruom.Location = new System.Drawing.Point(1489, 82);
		this.combruom.Name = "combruom";
		this.combruom.Size = new System.Drawing.Size(220, 25);
		this.combruom.TabIndex = 4;
		this.combruom.Visible = false;
		this.combruom.Leave += new System.EventHandler(combruom_Leave);
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
		this.label25.Location = new System.Drawing.Point(1377, 145);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(100, 17);
		this.label25.TabIndex = 3;
		this.label25.Text = "Comm Remarks";
		this.label25.Visible = false;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(875, 217);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(43, 15);
		this.label6.TabIndex = 13;
		this.label6.Text = "WS/RP";
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(1663, 117);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(84, 17);
		this.label42.TabIndex = 27;
		this.label42.Text = "Amount Type";
		this.label42.Visible = false;
		this.combcomp1.AllowNull = (DefaultableBoolean)1;
		this.combcomp1.AutoCompleteMode = (AutoCompleteMode)4;
		this.combcomp1.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combcomp1.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combcomp1).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.combcomp1).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combcomp1).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combcomp1).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcomp1).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.combcomp1).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcomp1).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.combcomp1).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combcomp1).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance132).TextHAlignAsString = "Left";
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.combcomp1).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combcomp1).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combcomp1).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combcomp1).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combcomp1.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combcomp1).Location = new System.Drawing.Point(112, 318);
		((System.Windows.Forms.Control)(object)this.combcomp1).Name = "combcomp1";
		((System.Windows.Forms.Control)(object)this.combcomp1).Size = new System.Drawing.Size(220, 24);
		((System.Windows.Forms.Control)(object)this.combcomp1).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.combcomp1).TabStop = false;
		((System.Windows.Forms.Control)(object)this.combcomp1).Visible = false;
		this.combcommtype.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.combcommtype.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.combcommtype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.combcommtype.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combcommtype.FormattingEnabled = true;
		this.combcommtype.Items.AddRange(new object[3] { "Flat", "Percent", "Comm Weight" });
		this.combcommtype.Location = new System.Drawing.Point(1489, 24);
		this.combcommtype.Name = "combcommtype";
		this.combcommtype.Size = new System.Drawing.Size(220, 25);
		this.combcommtype.TabIndex = 2;
		this.combcommtype.Visible = false;
		this.combcommtype.Leave += new System.EventHandler(combcommtype_Leave);
		this.CmbAgreementNo.AllowNull = (DefaultableBoolean)1;
		this.CmbAgreementNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAgreementNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAgreementNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance214).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance215).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance215).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance215).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAgreementNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance215;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance216;
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
		((System.Windows.Forms.Control)(object)this.CmbAgreementNo).Location = new System.Drawing.Point(112, 261);
		((System.Windows.Forms.Control)(object)this.CmbAgreementNo).Name = "CmbAgreementNo";
		((System.Windows.Forms.Control)(object)this.CmbAgreementNo).Size = new System.Drawing.Size(220, 24);
		((System.Windows.Forms.Control)(object)this.CmbAgreementNo).TabIndex = 9;
		((System.Windows.Forms.Control)(object)this.CmbAgreementNo).TabStop = false;
		((System.Windows.Forms.Control)(object)this.CmbAgreementNo).Visible = false;
		((System.Windows.Forms.Control)(object)this.CmbAgreementNo).Leave += new System.EventHandler(CmbAgreementNo_Leave);
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(1377, 86);
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
		this.label20.Location = new System.Drawing.Point(1377, 28);
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
		this.label19.Location = new System.Drawing.Point(1377, 115);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(95, 17);
		this.label19.TabIndex = 0;
		this.label19.Text = "Comm Amount";
		this.label19.Visible = false;
		this.txtcommremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommremarks.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommremarks.Location = new System.Drawing.Point(1489, 140);
		this.txtcommremarks.Multiline = true;
		this.txtcommremarks.Name = "txtcommremarks";
		this.txtcommremarks.Size = new System.Drawing.Size(220, 57);
		this.txtcommremarks.TabIndex = 6;
		this.txtcommremarks.Visible = false;
		this.txtCommrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommrate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommrate.Location = new System.Drawing.Point(1489, 53);
		this.txtCommrate.Name = "txtCommrate";
		this.txtCommrate.Size = new System.Drawing.Size(220, 23);
		this.txtCommrate.TabIndex = 3;
		this.txtCommrate.Visible = false;
		this.txtCommrate.TextChanged += new System.EventHandler(txtCommrate_TextChanged);
		this.txtcommamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommamount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommamount.Location = new System.Drawing.Point(1489, 111);
		this.txtcommamount.Name = "txtcommamount";
		this.txtcommamount.ReadOnly = true;
		this.txtcommamount.Size = new System.Drawing.Size(220, 23);
		this.txtcommamount.TabIndex = 5;
		this.txtcommamount.Visible = false;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(1377, 57);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(76, 17);
		this.label11.TabIndex = 0;
		this.label11.Text = "Comm Rate";
		this.label11.Visible = false;
		this.txtcatsr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcatsr.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcatsr.Location = new System.Drawing.Point(112, 374);
		this.txtcatsr.Name = "txtcatsr";
		this.txtcatsr.ReadOnly = true;
		this.txtcatsr.Size = new System.Drawing.Size(220, 21);
		this.txtcatsr.TabIndex = 4;
		this.txtcatsr.TabStop = false;
		this.txtcatsr.Visible = false;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(6, 348);
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
		this.combordercat.Location = new System.Drawing.Point(112, 346);
		this.combordercat.Name = "combordercat";
		this.combordercat.Size = new System.Drawing.Size(220, 23);
		this.combordercat.TabIndex = 3;
		this.combordercat.TabStop = false;
		this.combordercat.Visible = false;
		this.combordercat.Leave += new System.EventHandler(combordercat_Leave);
		this.validate.CustomFormat = "dd-MMM-yyyy";
		this.validate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.validate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.validate.Location = new System.Drawing.Point(112, 291);
		this.validate.Name = "validate";
		this.validate.Size = new System.Drawing.Size(198, 21);
		this.validate.TabIndex = 15;
		this.validate.TabStop = false;
		this.validate.Visible = false;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(6, 377);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(68, 15);
		this.label2.TabIndex = 8;
		this.label2.Text = "Catagory Sr";
		this.label2.Visible = false;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(17, 294);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(72, 15);
		this.label27.TabIndex = 0;
		this.label27.Text = "Validite Date";
		this.label27.Visible = false;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(8, 322);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(59, 15);
		this.label1.TabIndex = 0;
		this.label1.Text = "Company";
		this.label1.Visible = false;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(10, 266);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(82, 15);
		this.label4.TabIndex = 0;
		this.label4.Text = "AgreementNo";
		this.label4.Visible = false;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1128, 27);
		this.panel3.TabIndex = 1;
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.Teal;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(4, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(162, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "PURCHASE ORDER";
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1128, 25);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.btnnew, this.toolStripButton1, this.btnupdate, this.BtnSaveAs, this.btnsave, this.btnprint, this.btnattachment });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1128, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(60, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(81, 24);
		this.toolStripButton1.Text = "Refresh";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.btnupdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(79, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.BtnSaveAs.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.BtnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnSaveAs.Name = "BtnSaveAs";
		this.BtnSaveAs.Size = new System.Drawing.Size(86, 24);
		this.BtnSaveAs.Text = "&Save As";
		this.BtnSaveAs.Visible = false;
		this.BtnSaveAs.Click += new System.EventHandler(BtnSaveAs_Click);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(65, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnprint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(101, 24);
		this.btnprint.Text = "&Print-1600";
		this.btnprint.Click += new System.EventHandler(btnprint_Click);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(110, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.tabPage2.Controls.Add(this.panel10);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Controls.Add(this.panel12);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.toolStrip2);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(1132, 651);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(2, 436);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1128, 213);
		this.panel10.TabIndex = 56;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1128, 213);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 46;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label3);
		this.panel9.Controls.Add(this.ctrlGrdBar3);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(2, 409);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1128, 27);
		this.panel9.TabIndex = 55;
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.Teal;
		this.label3.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(2, 5);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(236, 17);
		this.label3.TabIndex = 0;
		this.label3.Text = "Detail of the Above Selected Row";
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(2, 131);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1128, 278);
		this.panel8.TabIndex = 54;
		this.grdhistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdhistory).BackColor = System.Drawing.Color.White;
		grdhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistory.DesignTimeLayout = grdhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistory.DynamicFiltering = true;
		this.grdhistory.FilterMode = (FilterMode)2;
		this.grdhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1128, 278);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 45;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.grdhistory.SelectionChanged += new System.EventHandler(grdhistory_SelectionChanged);
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick_1);
		this.panel12.Controls.Add(this.groupBox2);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(2, 56);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1128, 75);
		this.panel12.TabIndex = 53;
		this.groupBox2.Controls.Add(this.rdpqtymodify);
		this.groupBox2.Controls.Add(this.rdpqtydoc);
		this.groupBox2.Controls.Add(this.rdpqtyentry);
		this.groupBox2.Controls.Add(this.btnshow);
		this.groupBox2.Controls.Add(this.label59);
		this.groupBox2.Controls.Add(this.label60);
		this.groupBox2.Controls.Add(this.label61);
		this.groupBox2.Controls.Add(this.label62);
		this.groupBox2.Controls.Add(this.label63);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory);
		this.groupBox2.Controls.Add(this.txtToDocNoHistory);
		this.groupBox2.Controls.Add(this.txtFromDocNoHistory);
		this.groupBox2.Controls.Add(this.ToDateHistory);
		this.groupBox2.Controls.Add(this.FromDateHistory);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(5, 3);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1045, 69);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.rdpqtymodify.AutoSize = true;
		this.rdpqtymodify.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdpqtymodify.Location = new System.Drawing.Point(947, 41);
		this.rdpqtymodify.Name = "rdpqtymodify";
		this.rdpqtymodify.Size = new System.Drawing.Size(91, 19);
		this.rdpqtymodify.TabIndex = 8;
		this.rdpqtymodify.Text = "Modify Date";
		this.rdpqtymodify.UseVisualStyleBackColor = true;
		this.rdpqtydoc.AutoSize = true;
		this.rdpqtydoc.Checked = true;
		this.rdpqtydoc.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdpqtydoc.Location = new System.Drawing.Point(785, 41);
		this.rdpqtydoc.Name = "rdpqtydoc";
		this.rdpqtydoc.Size = new System.Drawing.Size(75, 19);
		this.rdpqtydoc.TabIndex = 6;
		this.rdpqtydoc.TabStop = true;
		this.rdpqtydoc.Text = "Doc Date";
		this.rdpqtydoc.UseVisualStyleBackColor = true;
		this.rdpqtyentry.AutoSize = true;
		this.rdpqtyentry.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdpqtyentry.Location = new System.Drawing.Point(865, 41);
		this.rdpqtyentry.Name = "rdpqtyentry";
		this.rdpqtyentry.Size = new System.Drawing.Size(80, 19);
		this.rdpqtyentry.TabIndex = 7;
		this.rdpqtyentry.Text = "Entry Date";
		this.rdpqtyentry.UseVisualStyleBackColor = true;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(727, 36);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(53, 26);
		this.btnshow.TabIndex = 9;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.btnshow.Leave += new System.EventHandler(btnshow_Leave);
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label59.Location = new System.Drawing.Point(485, 19);
		this.label59.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(86, 15);
		this.label59.TabIndex = 142;
		this.label59.Text = "Supplier Name";
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label60.Location = new System.Drawing.Point(384, 19);
		this.label60.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(64, 15);
		this.label60.TabIndex = 141;
		this.label60.Text = "To Doc No";
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label61.Location = new System.Drawing.Point(282, 19);
		this.label61.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(79, 15);
		this.label61.TabIndex = 140;
		this.label61.Text = "From Doc No";
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label62.Location = new System.Drawing.Point(143, 19);
		this.label62.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(48, 15);
		this.label62.TabIndex = 139;
		this.label62.Text = "To Date";
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label63.Location = new System.Drawing.Point(5, 20);
		this.label63.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(63, 15);
		this.label63.TabIndex = 13;
		this.label63.Text = "From Date";
		this.cmbSupplierNameHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplierNameHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplierNameHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplierNameHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance140;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance151).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance151).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance162;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance173).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance184;
		((AppearanceBase)appearance195).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance195).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance206).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance206).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance206).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance206;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSupplierNameHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Location = new System.Drawing.Point(485, 36);
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Name = "cmbSupplierNameHistory";
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Size = new System.Drawing.Size(240, 26);
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).TabIndex = 4;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(384, 38);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.ShortcutsEnabled = false;
		this.txtToDocNoHistory.Size = new System.Drawing.Size(100, 23);
		this.txtToDocNoHistory.TabIndex = 3;
		this.txtToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoHistory_KeyPress);
		this.txtFromDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoHistory.Location = new System.Drawing.Point(282, 38);
		this.txtFromDocNoHistory.Name = "txtFromDocNoHistory";
		this.txtFromDocNoHistory.ShortcutsEnabled = false;
		this.txtFromDocNoHistory.Size = new System.Drawing.Size(100, 23);
		this.txtFromDocNoHistory.TabIndex = 2;
		this.txtFromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNoHistory_KeyPress);
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(143, 38);
		this.ToDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(137, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(5, 38);
		this.FromDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(137, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label15);
		this.panel7.Controls.Add(this.ctrlGrdBar2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(2, 29);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1128, 27);
		this.panel7.TabIndex = 48;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.Teal;
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
		this.toolStrip2.Location = new System.Drawing.Point(2, 2);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1128, 27);
		this.toolStrip2.TabIndex = 47;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(69, 24);
		this.btnNewHistory.Text = "&Reset";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(81, 24);
		this.btnRefreshHistory.Text = "&Refresh";
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
		((System.Windows.Forms.Control)(object)this.combsalesman).Leave += new System.EventHandler(combsalesman_Leave);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1088, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1088, 3);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdDetail;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 21;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1089, 3);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdhistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1140, 680);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "PurchsaeOrderWithQty";
		this.Text = "Purchsae Order";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(PurchsaeOrderWithQty_FormClosing);
		base.Load += new System.EventHandler(PurchsaeOrder_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(PurchsaeOrder_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combrateuom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combjob).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combitempck).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combitem).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWholeSale).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBillType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combdeliverytrm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combpttrm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbranch1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combproject1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combsuppname).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combcomp1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAgreementNo).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.tabPage2.PerformLayout();
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel12.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierNameHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.combsalesman).EndInit();
		base.ResumeLayout(false);
	}
}

}
