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
using Architecture.BLL.Steel.Inventory;
using Architecture.BLL.SystemUtilities;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.Model.Steel.Inventory;
using Architecture.Model.SystemUtilities;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Steel.Sale;

public class SaleOrder_St : Form
{
	private Attachment AT = new Attachment();

	private int[] AryColWidth;

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private List<SaleOrderDetail> lstRemoveRecord = new List<SaleOrderDetail>();

	private bool UpdateMode;

	private int updateDetailIndex;

	private int ItemId;

	private int DefaultDaysToLessFromHistoryFromDate;

	private int DetailPoId;

	private DropDownBind DDL = new DropDownBind();

	public static int Id;

	private DataTable table = new DataTable();

	private bool Approved;

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

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private ToolStripButton btnprint;

	private ToolStripButton btnattachment;

	private Label label23;

	private Panel panel4;

	private TextBox txtcatsr;

	private Label label33;

	private Label label12;

	private TextBox txtsupprefno;

	private Label label7;

	private Label label10;

	private Label label8;

	private DateTimePicker DocDate;

	private Label label4;

	private Label label17;

	private TextBox txtdocno;

	private Label label6;

	private Label label25;

	private Label label21;

	private Label label20;

	private Label label19;

	private TextBox txtCommRate;

	private Label label11;

	private TextBox txtCommAmount;

	private Label label26;

	private TextBox txtduedays;

	private Label label24;

	private DateTimePicker duedate;

	private Label label32;

	private DateTimePicker deliverystartdate;

	private Label label30;

	private Label label29;

	private DateTimePicker validate;

	private Label label27;

	private Label label28;

	private TextBox txtDeliveryDays;

	private Panel panel5;

	private GroupBox groupBox3;

	private Button btnplus;

	private TextBox txtRemarksDetail;

	private Label label47;

	private TextBox txtRate;

	private TextBox txtamount;

	private TextBox txtweight;

	private TextBox txtItemQty;

	private Label label40;

	private Label label35;

	private Label label45;

	private Label label44;

	private Label label41;

	private Label label39;

	private Label label38;

	private Label label36;

	private Label label37;

	private Panel panel6;

	private Panel panel7;

	private Panel panel8;

	internal UltraCombo combstockparty;

	internal UltraCombo combrefparty;

	internal UltraCombo CmbCustomerName;

	internal UltraCombo CmbCommAgent;

	internal UltraCombo CmbItemName;

	internal UltraCombo CmbJobLot;

	internal UltraCombo CmbPackUom;

	internal UltraCombo CmbCityName;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grd;

	private CtrlGrdBar ctrlGrdBar2;

	private ComboBox combamounttype;

	private Label label42;

	private Button btnUpdateDetail;

	private Button btnCancelUpdateDetial;

	internal GridEX grdhistory;

	private Label label31;

	private ToolStripButton btnRefresh;

	private CheckBox ChkBox;

	internal GridEX DataGridHistoryDetail;

	private RichTextBox txtRemarks;

	internal UltraCombo CmbOrderCategory;

	internal UltraCombo CmbDeliveryTerm;

	internal UltraCombo CmbPaymentTerm;

	internal UltraCombo CmbOrderStatus;

	internal UltraCombo CmbRateUom;

	internal UltraCombo CmbCommRateUom;

	internal UltraCombo CmbCommType;

	private ToolStripButton btnDelete;

	private ToolStripButton btnSaveAs;

	internal UltraCombo cmbPackType;

	private Label label1;

	private GroupBox groupBox1;

	private Panel panel10;

	private Label label2;

	private Panel panel11;

	private Label label43;

	private TextBox txtOrderWeight;

	private Label label34;

	private TextBox txtOrderAmount;

	private Label label22;

	private TextBox txtFcyAmount;

	private Label label16;

	private TextBox txtExchangeRate;

	private Label label15;

	internal UltraCombo cmbCurrency;

	private Label label5;

	private TextBox txtOrderQty;

	private Label label13;

	private DateTimePicker datExpiryDate;

	private RichTextBox txtCommRemarks;

	private ToolStripButton SendEmail;

	private ToolStrip toolStrip2;

	private ToolStripButton btnResetHistory;

	private ToolStripButton btnRefreshHistory;

	private Label label55;

	private Panel panel14;

	private Label label3;

	private Panel panel15;

	private GroupBox groupBox2;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Button btnShow;

	private Label label50;

	private Label label51;

	private Label label52;

	private Label label53;

	private Label label54;

	internal UltraCombo CmbCustomerHistory;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Panel panel13;

	private Label label49;

	public SaleOrder_St(UserAccount user)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private bool FormValidation()
	{
		if (txtdocno.Text.Trim() == string.Empty || txtdocno.Text.Trim() == "0" || txtdocno.Text.Trim() == null)
		{
			MessageBox.Show("Doc No Field is Required");
			txtdocno.Focus();
			return false;
		}
		if (((UltraGridBase)CmbOrderCategory).ActiveRow == null)
		{
			MessageBox.Show("Order Catagory Field is Required");
			CmbOrderCategory.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCustomerName).ActiveRow == null)
		{
			MessageBox.Show("Customer Name Field is Required");
			CmbCustomerName.Focus();
			return false;
		}
		if (((UltraGridBase)CmbDeliveryTerm).ActiveRow == null)
		{
			MessageBox.Show("Delivery Term Field is Required");
			CmbDeliveryTerm.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPaymentTerm).ActiveRow == null)
		{
			MessageBox.Show("Payment Term Field is Required");
			CmbPaymentTerm.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbPaymentTerm.Value) == 2 && (txtduedays.Text.Trim() == string.Empty || txtduedays.Text.Trim() == "0" || txtduedays.Text.Trim() == null))
		{
			MessageBox.Show("Due Days Field is Required");
			txtduedays.Focus();
			return false;
		}
		if (((UltraGridBase)CmbOrderStatus).ActiveRow == null)
		{
			MessageBox.Show("Status Field is Required");
			CmbOrderStatus.Focus();
			return false;
		}
		if (txtOrderAmount.Text.Trim() == string.Empty || txtOrderAmount.Text.Trim() == "0" || txtOrderAmount.Text.Trim() == null)
		{
			MessageBox.Show("Order Amount Should Greater Than 0");
			return false;
		}
		if (((UltraGridBase)cmbCurrency).ActiveRow == null)
		{
			MessageBox.Show("Currency Field is Required");
			cmbCurrency.Focus();
			return false;
		}
		if (txtExchangeRate.Text.Trim() == string.Empty || Conversion.ToDecimal((object)txtExchangeRate.Text) == 0m || txtExchangeRate.Text.Trim() == null)
		{
			MessageBox.Show("Exchange Rate Field is Required");
			txtExchangeRate.Focus();
			return false;
		}
		if (Conversion.ToDecimal((object)txtCommAmount.Text) > 0m && (((UltraGridBase)CmbCommAgent).ActiveRow == null || ((Control)(object)CmbCommAgent).Text == string.Empty))
		{
			MessageBox.Show("Sales Man Agent Is Requird");
			CmbCommAgent.Focus();
			return false;
		}
		if (((Control)(object)CmbCommAgent).Text != "0" && ((UltraGridBase)CmbCommAgent).ActiveRow != null && ((Control)(object)CmbCommAgent).Text != string.Empty && Conversion.ToDecimal((object)txtCommAmount.Text) == 0m)
		{
			MessageBox.Show("Commission Amount Is Requird");
			txtCommAmount.Focus();
			return false;
		}
		if (txtFcyAmount.Text.Trim() == string.Empty || txtFcyAmount.Text.Trim() == "0" || txtFcyAmount.Text.Trim() == null)
		{
			MessageBox.Show("Fcy Amount Should Greater Than 0");
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)CmbItemName).ActiveRow == null)
		{
			MessageBox.Show("Item Name Field is Required");
			CmbItemName.Focus();
			return false;
		}
		if (((UltraGridBase)CmbJobLot).ActiveRow == null)
		{
			MessageBox.Show("Job Lot Field is Required");
			CmbJobLot.Focus();
			return false;
		}
		if (((UltraGridBase)cmbPackType).ActiveRow == null)
		{
			MessageBox.Show("Packing Type Field is Required");
			cmbPackType.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackUom).ActiveRow == null)
		{
			MessageBox.Show("Pack Uom  Field is Required");
			CmbPackUom.Focus();
			return false;
		}
		if (txtItemQty.Text.Trim() == string.Empty || txtItemQty.Text.Trim() == "0")
		{
			MessageBox.Show("Qty Field is Required");
			txtItemQty.Focus();
			return false;
		}
		if (txtRate.Text.Trim() == string.Empty || txtRate.Text.Trim() == "0")
		{
			MessageBox.Show("Item Rate Field is Required");
			txtRate.Focus();
			return false;
		}
		if (((UltraGridBase)CmbRateUom).ActiveRow == null)
		{
			MessageBox.Show("Rate UOM Field is Required");
			CmbRateUom.Focus();
			return false;
		}
		if (txtamount.Text.Trim() == string.Empty || txtamount.Text.Trim() == "0")
		{
			MessageBox.Show("Item Amount Should Greater Than 0");
			return false;
		}
		if (((UltraGridBase)CmbCityName).ActiveRow == null)
		{
			MessageBox.Show("CityName Field is Required");
			CmbCityName.Focus();
			return false;
		}
		return true;
	}

	private void ChkCommission_Click(object sender, EventArgs e)
	{
	}

	private void btnplus_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				table.Rows.Add(0, CmbItemName.Value, ((Control)(object)CmbItemName).Text.Trim(), CmbJobLot.Value, ((Control)(object)CmbJobLot).Text.Trim(), cmbPackType.Value, ((Control)(object)cmbPackType).Text.Trim(), CmbPackUom.Value, ((Control)(object)CmbPackUom).Text.Trim(), Conversion.ToDouble((object)txtItemQty.Text.Trim()), Conversion.ToDouble((object)txtweight.Text.Trim()), Conversion.ToDouble((object)txtRate.Text.Trim()), CmbRateUom.Value, ((Control)(object)CmbRateUom).Text.Trim(), Convert.ToDouble(txtamount.Text), CmbCityName.Value, ((Control)(object)CmbCityName).Text.Trim(), (Conversion.ToDecimal((object)txtExchangeRate.Text) > 0m) ? Conversion.ToDecimal((object)txtExchangeRate.Text) : 0m, (Conversion.ToDecimal((object)txtExchangeRate.Text) > 0m) ? Math.Round(Conversion.ToDecimal((object)txtamount.Text) / Conversion.ToDecimal((object)txtExchangeRate.Text), clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount) : 0m, txtRemarksDetail.Text.Trim());
				grd.DataSource = table;
				grd.RetrieveStructure();
				grdSettings();
				ResetDetail();
				TotalCommissionAmount();
				CalculateTotalInformation();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ResetDetail()
	{
		try
		{
			((Control)(object)CmbItemName).Text = string.Empty;
			((Control)(object)cmbPackType).Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			txtItemQty.Text = string.Empty;
			txtweight.Text = string.Empty;
			txtRate.Text = string.Empty;
			((Control)(object)CmbRateUom).Text = string.Empty;
			txtamount.Text = string.Empty;
			txtRemarksDetail.Text = string.Empty;
			CmbItemName.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Expected O, but got Unknown
		//IL_06d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0703: Unknown result type (might be due to invalid IL or missing references)
		//IL_0719: Unknown result type (might be due to invalid IL or missing references)
		//IL_0725: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			SaleOrder po = new SaleOrder();
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				po.Id = Id;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			po.DocumentTypeId = 1505;
			po.BranchesId = UserAccount.BranchesId;
			po.DocDate = DocDate.Value;
			po.DocNo = Conversion.ToInt((object)txtdocno.Text);
			po.OrganizationId = UserAccount.OrganizationId;
			po.CompanyId = UserAccount.CompanyId;
			po.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			po.EntryDate = DateTime.Now;
			po.ModifyDate = DateTime.Now;
			po.ApprovedDate = DateTime.Now;
			po.EntryUserId = UserAccount.ID;
			po.ModifyUserId = UserAccount.ID;
			po.OrderCatagoryId = Conversion.ToInt(CmbOrderCategory.Value);
			po.CatagorySrNo = Conversion.ToInt((object)txtcatsr.Text);
			po.OrderSupCustId = Conversion.ToInt(CmbCustomerName.Value);
			po.PartyReference = Conversion.ToString((object)txtsupprefno.Text);
			po.RefrenencePartyId = Conversion.ToInt(combrefparty.Value);
			po.RemarksHeader = Conversion.ToString((object)txtRemarks.Text);
			po.PaymentTermsId = Conversion.ToInt(CmbPaymentTerm.Value);
			po.OrderDueDays = Conversion.ToInt((object)txtduedays.Text);
			po.OrderDueDate = duedate.Value;
			po.OrderExpiryDate = datExpiryDate.Value;
			po.DeliveryTerm = Conversion.ToString((object)((Control)(object)CmbDeliveryTerm).Text);
			po.DeliveryStartDate = deliverystartdate.Value;
			po.DeliveryDays = Conversion.ToInt((object)txtDeliveryDays.Text);
			po.OrderStatus = ((Control)(object)CmbOrderStatus).Text;
			po.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
			po.OrderQty = Conversion.ToDecimal((object)txtOrderQty.Text);
			po.OrderWeight = Conversion.ToDecimal((object)txtOrderWeight.Text);
			po.OrderAmount = Conversion.ToDecimal((object)txtOrderAmount.Text);
			po.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
			po.FcyAmount = Conversion.ToDecimal((object)txtFcyAmount.Text);
			if (((UltraGridBase)CmbCommAgent).ActiveRow != null)
			{
				po.CommissionAgentId = Conversion.ToInt(CmbCommAgent.Value);
				po.CommissionType = Conversion.ToString((object)((Control)(object)CmbCommType).Text);
				po.CommRate = Conversion.ToDecimal((object)txtCommRate.Text);
				po.CommRateUom = Conversion.ToInt((object)((Control)(object)CmbCommRateUom).Text);
				po.CommAmount = Conversion.ToDecimal((object)txtCommAmount.Text.Trim());
				po.CommRemarks = Conversion.ToString((object)txtCommRemarks.Text);
				CalculateTotalInformation();
			}
			po.SaleOrderDetailList = new List<SaleOrderDetail>();
			if (Id > 0 && lstRemoveRecord.Count > 0)
			{
				for (int i = 0; i < lstRemoveRecord.Count; i++)
				{
					po.SaleOrderDetailList.Add(lstRemoveRecord[i]);
				}
			}
			if (grd.RowCount > 0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					SaleOrderDetail vd = new SaleOrderDetail();
					if (Conversion.ToDouble(r.Cells["ItemQty"].Value) > 0.0)
					{
						if (Id > 0)
						{
							vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
						}
						else
						{
							vd.Id = 0;
						}
						if (vd.Id > 0)
						{
							vd.ActionTypeId = 2;
						}
						else
						{
							vd.ActionTypeId = 1;
						}
						vd.OrderItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						vd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
						vd.PackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
						vd.OrderItemUOMId = Conversion.ToInt(r.Cells["PackUomId"].Value);
						vd.OrderItemQty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
						vd.NetWeight = Conversion.ToDecimal(r.Cells["Weight"].Value);
						if (!(Conversion.ToDouble(r.Cells["ItemRate"].Value) > 0.0))
						{
							throw new Exception("Rate Field Required");
						}
						vd.OrderItemRate = Conversion.ToDecimal(r.Cells["ItemRate"].Value);
						vd.OrderItemRateUOMId = Conversion.ToInt(r.Cells["RateUomId"].Value);
						vd.ItemAmount = Conversion.ToDecimal(r.Cells["ItemAmount"].Value);
						vd.ItemNetAmount = vd.ItemAmount;
						vd.CityId = Conversion.ToInt(r.Cells["CityId"].Value);
						vd.RemarkDetail = Conversion.ToString(r.Cells["Remarks"].Value);
						vd.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
						vd.ExchangeRate = Conversion.ToDecimal(r.Cells["ExchangeRate"].Value);
						vd.FcyAmount = Conversion.ToDecimal(r.Cells["FcyAmount"].Value);
						vd.IsOrderTaxable = false;
						po.SaleOrderDetailList.Add(vd);
					}
				}
				if (po.SaleOrderDetailList.Count == 0)
				{
					throw new Exception("At least enter value/quantity in one of the rows");
				}
				po.AttachmentsList = new List<DMSAttachments>();
				po.DeleteAttachmentsList = new List<DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<SaleOrder>(po, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
				double AvailableLimit = SaleOrder.CHECKSUPPLIERCUSTOMERLIMITS(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					SupplierCustomerId = po.OrderSupCustId,
					Amount = Conversion.ToDouble((object)txtOrderAmount.Text),
					ActionId = 2
				});
				if (AvailableLimit > 0.0)
				{
					if (MessageBox.Show("Debit Limit of Customer Exceeds Define Debit Limit & remaining limit value is   " + Conversion.ToString((object)AvailableLimit), "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
					{
						return;
					}
				}
				else if (AvailableLimit < 0.0 && MessageBox.Show("Debit Limit of Customer already Exceeds Define Debit Limit", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
				{
					return;
				}
				int success = (po.Id = SaleOrder.Save(po));
				if (Id > 0)
				{
					MessageBox.Show("Data Update Successfully....  " + po.DocNo);
					if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
					{
						DMSAttachments.RemoveByIdAndNames(Id, base.Name, 1505, 0);
					}
				}
				else
				{
					MessageBox.Show("Data Save Successfully....  " + po.DocNo);
				}
				Reset();
				if (ChkBox.Checked)
				{
					CommonServices.SaleOrderSlip_1511(success);
				}
			}
			else
			{
				MessageBox.Show("Grid Record Not Found");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			Approved = false;
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
			if (Id == 0)
			{
				throw new Exception("Record Not Update because RecId Not Found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void PaymentTerms()
	{
		try
		{
			DataTable dtTerm = CommonServices.GetDueTermServiceBind();
			if (dtTerm.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtTerm, CmbPaymentTerm, "Id", "TermsDescription", "Payment Term", false);
				((UltraGridBase)CmbPaymentTerm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)CmbPaymentTerm).Rows[1].Activate();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DeliveryTerms()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("DeliveryTerm");
			dt.Rows.Add(1, "Load");
			dt.Rows.Add(2, "Ponch");
			DDL.BindDDL(dt, CmbDeliveryTerm, "Id", "DeliveryTerm", "Delivery Term", ZeroIndex: false);
			((UltraGridBase)CmbDeliveryTerm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)CmbDeliveryTerm).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
			DDL.BindDDL(dt, CmbOrderStatus, "Id", "Status", "Order Status", ZeroIndex: false);
			((UltraGridBase)CmbOrderStatus).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)CmbOrderStatus).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void OrderCatagoryfill()
	{
		try
		{
			DataTable dt = InvOrderCategory.GetAll();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbOrderCategory, "Id", "OrderCategoryName", "OrderCategoryName", false);
				((UltraGridBase)CmbOrderCategory).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GenerateOrderCategoryNo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		try
		{
			int code = InvOrderCategory.GenerateSaleOrderCategoryCodebyId(new InvOrderCategory
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(CmbOrderCategory.Value),
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (code > 0)
			{
				txtcatsr.Text = code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void combordercat_Leave(object sender, EventArgs e)
	{
		GenerateOrderCategoryNo();
	}

	private void SupplierNameFill()
	{
		try
		{
			DataTable dtsuppcus = CommonServices.SupplierCustomerGetforComboServiceBind();
			if (dtsuppcus.Rows.Count > 0)
			{
				DDL.BindDDL(dtsuppcus, CmbCustomerName, "Id", "CompanyName", "Customer Name", ZeroIndex: false);
				DDL.BindDDL(dtsuppcus, CmbCommAgent, "Id", "CompanyName", "SalesMan Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CommissionTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("CommissionType");
			dt.Rows.Add(1, "Flat");
			dt.Rows.Add(2, "Percent");
			dt.Rows.Add(3, "Comm Weight");
			DDL.BindDDL(dt, CmbCommType, "Id", "CommissionType", "CommissionType", ZeroIndex: false);
			((UltraGridBase)CmbCommType).DisplayLayout.Bands[0].Columns[1].Width = 120;
			((UltraGridBase)CmbCommType).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CommissionUOMFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("UOM");
			dt.Rows.Add(1, "40");
			dt.Rows.Add(2, "50");
			dt.Rows.Add(3, "60");
			dt.Rows.Add(4, "100");
			DDL.BindDDL(dt, CmbCommRateUom, "Id", "UOM", "UOM", ZeroIndex: false);
			((UltraGridBase)CmbCommRateUom).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)CmbCommRateUom).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void combsalesman_Leave(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)CmbCommType).Enabled = true;
			txtCommRate.Enabled = true;
			((Control)(object)CmbCommRateUom).Enabled = true;
			txtCommAmount.Enabled = true;
			txtCommRemarks.Enabled = true;
			CmbCommType.Focus();
			CalculateTotalInformation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DocumentNoFill()
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
			int code = SaleOrder.GenerateCodeSaleOrder(new SaleOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 1505,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (code > 0)
			{
				txtdocno.Text = code.ToString();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ItemDetailFill()
	{
		try
		{
			DataTable dtitem = CommonServices.AllItemsBindForSteel();
			if (dtitem.Rows.Count > 0)
			{
				DDL.BindDDL(dtitem, CmbItemName, "Id", "ItemName", "Item Name", ZeroIndex: false);
				((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void bindRateUomAndItemPackUom(int ID)
	{
		try
		{
			string Packuom = ((Control)(object)CmbPackUom).Text;
			string Rateuom = ((Control)(object)CmbRateUom).Text;
			((Control)(object)CmbPackUom).Text = string.Empty;
			((Control)(object)CmbRateUom).Text = string.Empty;
			DataTable dtUom = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(CmbItemName.Value));
			if (dtUom.Rows.Count > 0)
			{
				DDL.BindDDL(dtUom, CmbPackUom, "Id", "UOMCode", "PackUOM", ZeroIndex: false);
				((UltraGridBase)CmbPackUom).DisplayLayout.Bands[0].Columns[1].Width = 75;
				((UltraGridBase)CmbPackUom).DisplayLayout.Bands[0].Columns[2].Width = 85;
				DDL.BindDDL(dtUom, CmbRateUom, "Id", "UOMCode", "RateUOM", ZeroIndex: false);
				((UltraGridBase)CmbRateUom).DisplayLayout.Bands[0].Columns[1].Width = 75;
				((UltraGridBase)CmbRateUom).DisplayLayout.Bands[0].Columns[2].Width = 85;
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Packuom))
				{
					((Control)(object)CmbPackUom).Text = Packuom;
				}
				else
				{
					((Control)(object)CmbPackUom).Text = string.Empty;
				}
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Rateuom))
				{
					((Control)(object)CmbRateUom).Text = Rateuom;
				}
				else
				{
					((Control)(object)CmbRateUom).Text = string.Empty;
				}
			}
			else
			{
				((Control)(object)CmbPackUom).Text = string.Empty;
				((Control)(object)CmbRateUom).Text = string.Empty;
				((UltraGridBase)CmbPackUom).DataSource = null;
				((UltraGridBase)CmbRateUom).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void combitem_Leave(object sender, EventArgs e)
	{
	}

	private void combojoblotfill()
	{
		try
		{
			DataTable dt = CommonServices.JobLotGetAllService();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbJobLot, "Id", "JobLotDescription", "JobLot", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbPackingTypeFill()
	{
		try
		{
			DataTable dtpkt = CommonServices.InvPackingTypeGetAllService();
			if (dtpkt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtpkt, cmbPackType, "Id", "PackTypeDesc", "Packing Type", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void cmbCityFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtCity = City.GetAll(new City
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtCity.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCity, CmbCityName, "Id", "CityName", "Description", false);
				((UltraGridBase)CmbCityName).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CurrencyFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = MultiCurrency.GetAll(new MultiCurrency
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbCurrency, "Id", "CurrencyCode", "Currency", false);
				((UltraGridBase)cmbCurrency).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow r = grd.CurrentRow;
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					SaleOrderDetail vd = new SaleOrderDetail();
					vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					vd.OrderItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					vd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					vd.PackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
					vd.OrderItemUOMId = Conversion.ToInt(r.Cells["PackUomId"].Value);
					vd.OrderItemQty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
					vd.NetWeight = Conversion.ToDecimal(r.Cells["Weight"].Value);
					vd.OrderItemRate = Conversion.ToDecimal(r.Cells["ItemRate"].Value);
					vd.OrderItemRateUOMId = Conversion.ToInt(r.Cells["RateUomId"].Value);
					vd.ItemAmount = Conversion.ToDecimal(r.Cells["ItemAmount"].Value);
					vd.ItemNetAmount = vd.ItemAmount;
					vd.CityId = Conversion.ToInt(r.Cells["CityId"].Value);
					vd.ExchangeRate = Conversion.ToDecimal(r.Cells["ExchangeRate"].Value);
					vd.FcyAmount = Conversion.ToDecimal(r.Cells["FcyAmount"].Value);
					vd.RemarkDetail = Conversion.ToString(r.Cells["Remarks"].Value);
					vd.ActionTypeId = 3;
					lstRemoveRecord.Add(vd);
					table.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					grd.UpdateData();
					TotalCommissionAmount();
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
			}
			TotalCommissionAmount();
			CalculateTotalInformation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void PurchsaeOrder_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1100, 830);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnprint.Enabled = formright.DoHavePrintRights;
			btnupdate.Enabled = formright.DoHaveUpdateRights;
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
			dtViewAttachments.Columns.Add("AttchmentPath");
			table.Columns.Add("Id");
			table.Columns.Add("ItemId");
			table.Columns.Add("ItemName");
			table.Columns.Add("JobLotId");
			table.Columns.Add("JobLot");
			table.Columns.Add("PackingTypeId");
			table.Columns.Add("PackingType");
			table.Columns.Add("PackUomId");
			table.Columns.Add("PackUom");
			table.Columns.Add("ItemQty", typeof(double));
			table.Columns.Add("Weight", typeof(double));
			table.Columns.Add("ItemRate", typeof(double));
			table.Columns.Add("RateUomId");
			table.Columns.Add("RateUom");
			table.Columns.Add("ItemAmount", typeof(double));
			table.Columns.Add("CityId");
			table.Columns.Add("CityName");
			table.Columns.Add("ExchangeRate", typeof(double));
			table.Columns.Add("FcyAmount", typeof(double));
			table.Columns.Add("Remarks");
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			DocumentNoFill();
			OrderCatagoryfill();
			SupplierNameFill();
			DeliveryTerms();
			PaymentTerms();
			OrderStatus();
			CurrencyFill();
			CommissionTypeFill();
			CommissionUOMFill();
			ItemDetailFill();
			combojoblotfill();
			CmbPackingTypeFill();
			cmbCityFill();
			ConfigurationDefault();
			DocDate.Focus();
			btnupdate.Visible = false;
			btnsave.Visible = true;
			btnplus.Visible = true;
			btnSaveAs.Visible = false;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			((Control)(object)CmbCommType).Enabled = false;
			txtCommRate.Enabled = false;
			((Control)(object)CmbCommRateUom).Enabled = false;
			txtCommAmount.Enabled = false;
			txtCommRemarks.Enabled = false;
			((UltraGridBase)CmbOrderCategory).Rows[1].Activate();
			((UltraGridBase)CmbOrderStatus).Rows[0].Activate();
			((Control)(object)CmbOrderStatus).Enabled = false;
			GenerateOrderCategoryNo();
			ChkBox.Checked = true;
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			HistoryCombosFill();
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			lstRemoveRecord.Clear();
			Approved = false;
			Id = 0;
			ItemId = 0;
			DetailPoId = 0;
			btnSaveAs.Visible = false;
			((Control)(object)CmbCustomerName).Text = string.Empty;
			txtsupprefno.Text = string.Empty;
			txtduedays.Text = string.Empty;
			txtDeliveryDays.Text = string.Empty;
			((Control)(object)CmbCommAgent).Text = string.Empty;
			((Control)(object)CmbCommType).Text = string.Empty;
			txtCommRate.Text = string.Empty;
			txtCommAmount.Text = string.Empty;
			txtCommRemarks.Text = string.Empty;
			((Control)(object)combrefparty).Text = string.Empty;
			((Control)(object)combstockparty).Text = string.Empty;
			((Control)(object)combrefparty).Text = string.Empty;
			DocDate.Focus();
			((Control)(object)CmbItemName).Text = string.Empty;
			((Control)(object)CmbJobLot).Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			txtItemQty.Text = string.Empty;
			txtweight.Text = string.Empty;
			txtRate.Text = string.Empty;
			((Control)(object)CmbRateUom).Text = string.Empty;
			txtamount.Text = string.Empty;
			((Control)(object)CmbCityName).Text = string.Empty;
			txtRemarksDetail.Clear();
			txtRemarks.Text = string.Empty;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			((Control)(object)CmbCommType).Enabled = false;
			txtCommRate.Enabled = false;
			((Control)(object)CmbCommRateUom).Enabled = false;
			txtCommAmount.Enabled = false;
			txtCommRemarks.Enabled = false;
			((Control)(object)CmbOrderStatus).Enabled = false;
			((UltraGridBase)CmbOrderStatus).Rows[0].Activate();
			((UltraGridBase)CmbOrderCategory).Rows[1].Activate();
			GenerateOrderCategoryNo();
			DocumentNoFill();
			ConfigurationDefault();
			txtOrderQty.Clear();
			txtOrderWeight.Clear();
			txtOrderAmount.Clear();
			txtFcyAmount.Clear();
			table.Clear();
			grd.DataSource = table;
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
			int qty = 0;
			int rate = 0;
			int ratemultiply = 0;
			int totalamount = 0;
			Conversion.ToInt((object)txtweight.Text);
			Conversion.ToInt((object)((Control)(object)CmbRateUom).Text);
			rate = Conversion.ToInt((object)txtRate.Text);
			qty = Conversion.ToInt((object)txtItemQty.Text);
			ratemultiply = Convert.ToInt32(int.Parse(txtweight.ToString()) / int.Parse(((object)CmbRateUom).ToString()));
			if (combamounttype.SelectedIndex == 0)
			{
				totalamount = Conversion.ToInt((object)(qty * rate));
				txtamount.Text = totalamount.ToString();
			}
			else
			{
				totalamount = Conversion.ToInt((object)(ratemultiply * rate));
				txtamount.Text = totalamount.ToString();
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
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["JobLotId"].Visible = false;
			grd.RootTable.Columns["PackingTypeId"].Visible = false;
			grd.RootTable.Columns["PackUomId"].Visible = false;
			grd.RootTable.Columns["RateUomId"].Visible = false;
			grd.RootTable.Columns["CityId"].Visible = false;
			grd.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grd.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Weight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["Weight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ExchangeRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grd.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
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
			throw new Exception(ex.Message);
		}
	}

	private void PurchsaeOrder_KeyDown(object sender, KeyEventArgs e)
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
		if (btnupdate.Visible && btnupdate.Enabled && e.Control && e.KeyCode == Keys.U && (UpdateMode = true))
		{
			btnupdate_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.F5)
		{
			CmbItemName.Focus();
		}
		if (btnsave.Visible && btnsave.Enabled && e.Control)
		{
			_ = e.KeyCode;
			_ = 71;
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

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			gridhistoryfill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			grdhistory.DataSource = null;
			DataGridHistoryDetail.DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryCombosFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HistoryCombosFill()
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		try
		{
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id", typeof(int));
			dtCustomer.Columns.Add("Customer", typeof(string));
			DataTable dt = SaleOrder.GetDataForDropDownFromSaleOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Customer")
					{
						dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtCustomer, CmbCustomerHistory, "Id", "Customer", "Party", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void ReadById(int ID)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		try
		{
			Id = Conversion.ToInt((object)ID);
			lstRemoveRecord.Clear();
			SaleOrder po = new SaleOrder();
			po = SaleOrder.GetByID(Id);
			if (po != null)
			{
				tabControl1.SelectedIndex = 0;
				btnSaveAs.Visible = true;
				DocDate.Value = po.DocDate;
				txtdocno.Text = Conversion.ToString((object)po.DocNo);
				CmbOrderCategory.Value = Conversion.ToInt((object)po.OrderCatagoryId);
				txtcatsr.Text = po.CatagorySrNo.ToString();
				CmbCustomerName.Value = Conversion.ToInt((object)po.OrderSupCustId);
				txtsupprefno.Text = Conversion.ToString((object)po.PartyReference);
				txtRemarks.Text = Conversion.ToString((object)po.RemarksHeader);
				CmbPaymentTerm.Value = Conversion.ToInt((object)po.PaymentTermsId);
				txtduedays.Text = Conversion.ToString((object)po.OrderDueDays);
				duedate.Value = po.OrderDueDate;
				datExpiryDate.Value = po.OrderExpiryDate;
				((Control)(object)CmbDeliveryTerm).Text = Conversion.ToString((object)po.DeliveryTerm);
				deliverystartdate.Value = po.DeliveryStartDate;
				txtDeliveryDays.Text = Conversion.ToString((object)po.DeliveryDays);
				CmbCommAgent.Value = Conversion.ToInt((object)po.CommissionAgentId);
				((Control)(object)CmbCommType).Text = Conversion.ToString((object)po.CommissionType);
				txtCommRate.Text = Conversion.ToString((object)po.CommRate);
				((Control)(object)CmbCommRateUom).Text = Conversion.ToString((object)po.CommRateUom);
				txtCommAmount.Text = Conversion.ToString((object)po.CommAmount);
				txtCommRemarks.Text = Conversion.ToString((object)po.CommRemarks);
				((Control)(object)CmbOrderStatus).Enabled = true;
				((Control)(object)CmbOrderStatus).Text = Conversion.ToString((object)po.OrderStatus);
				Approved = Conversion.ToBool((object)po.IsApproved);
				cmbCurrency.Value = po.CurrencyId;
				txtExchangeRate.Text = po.ExchangeRate.ToString();
				table.Clear();
				for (int i = 0; i < po.SaleOrderDetailList.Count; i++)
				{
					table.Rows.Add(po.SaleOrderDetailList[i].Id, po.SaleOrderDetailList[i].OrderItemId, po.SaleOrderDetailList[i].ItemName, po.SaleOrderDetailList[i].JobLotId, po.SaleOrderDetailList[i].JobLotDescription, po.SaleOrderDetailList[i].PackingTypeId, po.SaleOrderDetailList[i].PackingType, po.SaleOrderDetailList[i].OrderItemUOMId, po.SaleOrderDetailList[i].UOMCode, po.SaleOrderDetailList[i].OrderItemQty, po.SaleOrderDetailList[i].NetWeight, po.SaleOrderDetailList[i].OrderItemRate, po.SaleOrderDetailList[i].OrderItemRateUOMId, po.SaleOrderDetailList[i].RateUom, po.SaleOrderDetailList[i].ItemAmount, po.SaleOrderDetailList[i].CityId, po.SaleOrderDetailList[i].CityName, po.SaleOrderDetailList[i].ExchangeRate, po.SaleOrderDetailList[i].FcyAmount, po.SaleOrderDetailList[i].RemarkDetail);
				}
				grd.DataSource = table;
				grd.RetrieveStructure();
				grdSettings();
				btnsave.Visible = false;
				btnupdate.Visible = true;
				FormHelper.LoadAttachmentsForObject(AT, Id, base.Name);
				TotalCommissionAmount();
				CalculateTotalInformation();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void gridhistoryfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters reports = new ReportsParameters();
			reports.OrganizationId = UserAccount.OrganizationId;
			reports.CompanyId = UserAccount.CompanyId;
			reports.BranchesId = UserAccount.BranchesId;
			reports.DocumentTypeId = 1505;
			reports.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			reports.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!reports.CanViewAllRecord)
			{
				reports.EntryUser = UserAccount.ID;
			}
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					reports.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					reports.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					reports.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					reports.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					reports.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					reports.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					reports.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					reports.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			reports.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			reports.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			reports.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			DataTable dthistoryfill = SaleOrder.FormHistory(reports);
			if (dthistoryfill.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("OrderCategory");
				dt.Columns.Add("PartyName");
				dt.Columns.Add("CommAgent");
				dt.Columns.Add("CommType");
				dt.Columns.Add("CommRate", typeof(double));
				dt.Columns.Add("CommAmount", typeof(double));
				dt.Columns.Add("CommRemarks");
				dt.Columns.Add("OrderStatus");
				dt.Columns.Add("DeliveryTerm");
				dt.Columns.Add("PaymentTerm");
				dt.Columns.Add("DueDays", typeof(int));
				dt.Columns.Add("DueDate", typeof(DateTime));
				dt.Columns.Add("ExpiryDate", typeof(DateTime));
				dt.Columns.Add("DeliveryDays");
				dt.Columns.Add("DeliveryStartDate", typeof(DateTime));
				dt.Columns.Add("OrderQty", typeof(double));
				dt.Columns.Add("OrderWeight", typeof(double));
				dt.Columns.Add("OrderAmount", typeof(double));
				dt.Columns.Add("CurrencyName");
				dt.Columns.Add("ExchangeRate", typeof(double));
				dt.Columns.Add("FcyAmount", typeof(double));
				dt.Columns.Add("EntryUser");
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser");
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("IsApproved");
				dt.Columns.Add("ApprovedUser");
				dt.Columns.Add("ApprovedDate", typeof(DateTime));
				dt.Columns.Add("Remarks");
				dt.Columns.Add("NoOfAttachments");
				for (int i = 0; i < dthistoryfill.Rows.Count; i++)
				{
					dt.Rows.Add(dthistoryfill.Rows[i]["Id"], dthistoryfill.Rows[i]["DocNo"], Conversion.ToDateTime(dthistoryfill.Rows[i]["DocDate"]).ToShortDateString(), dthistoryfill.Rows[i]["OrderCategoryDescription"], dthistoryfill.Rows[i]["SupplierName"], dthistoryfill.Rows[i]["CommissionAgent"], dthistoryfill.Rows[i]["CommissionType"], dthistoryfill.Rows[i]["CommRate"], dthistoryfill.Rows[i]["CommAmount"], dthistoryfill.Rows[i]["CommRemarks"], dthistoryfill.Rows[i]["OrderStatus"], dthistoryfill.Rows[i]["DeliveryTerm"], dthistoryfill.Rows[i]["TermsDescription"], dthistoryfill.Rows[i]["DueDays"], Conversion.ToDateTime(dthistoryfill.Rows[i]["DueDate"]).ToShortDateString(), Conversion.ToDateTime(dthistoryfill.Rows[i]["OrderExpiryDate"]).ToShortDateString(), dthistoryfill.Rows[i]["DeliveryDays"], Conversion.ToDateTime(dthistoryfill.Rows[i]["DeliveryStartDate"]).ToShortDateString(), dthistoryfill.Rows[i]["OrderQty"], dthistoryfill.Rows[i]["OrderWeight"], dthistoryfill.Rows[i]["OrderAmount"], dthistoryfill.Rows[i]["CurrencyName"], dthistoryfill.Rows[i]["ExchangeRate"], dthistoryfill.Rows[i]["FcyAmount"], dthistoryfill.Rows[i]["UserName"], Conversion.ToDateTime(dthistoryfill.Rows[i]["EntryDate"]).ToShortDateString(), dthistoryfill.Rows[i]["ModifyUserName"], Conversion.ToDateTime(dthistoryfill.Rows[i]["ModifyDate"]).ToShortDateString(), Conversion.ToBool(dthistoryfill.Rows[i]["IsApproved"]) ? "Approved" : "Not Approved", dthistoryfill.Rows[i]["ApprovedUserName"], Conversion.ToDateTime(dthistoryfill.Rows[i]["ApprovedDate"]).ToShortDateString(), dthistoryfill.Rows[i]["RemarksHeader"], dthistoryfill.Rows[i]["NoOfAttachments"]);
				}
				grdhistory.DataSource = dt;
				grdhistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				grdhistory.ClearStructure();
				DataGridHistoryDetail.ClearStructure();
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
			CommonServices.GridAutoAdjustment(AryColWidth, grdhistory);
			grdhistory.RootTable.Columns["Id"].Visible = false;
			grdhistory.RootTable.Columns.Add("Edit");
			grdhistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdhistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdhistory.RootTable.Columns["Edit"].Width = 50;
			grdhistory.RootTable.Columns["Edit"].Position = 0;
			grdhistory.RootTable.Columns.Add("Print");
			grdhistory.RootTable.Columns["Print"].Caption = "Print";
			grdhistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Print"].ButtonText = "Print";
			grdhistory.RootTable.Columns["Print"].Width = 50;
			grdhistory.RootTable.Columns["Print"].Position = 1;
			grdhistory.FrozenColumns = 2;
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdhistory.RootTable.Columns["CommRate"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["CommRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdhistory.RootTable.Columns["CommAmount"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["CommAmount"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["CommAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdhistory.RootTable.Columns["CommAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdhistory.RootTable.Columns["OrderQty"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["OrderQty"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["OrderQty"].FormatString = "#,##0.##";
			grdhistory.RootTable.Columns["OrderQty"].TotalFormatString = "#,##0.##";
			grdhistory.RootTable.Columns["OrderWeight"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["OrderWeight"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["OrderWeight"].FormatString = "#,##0.##";
			grdhistory.RootTable.Columns["OrderWeight"].TotalFormatString = "#,##0.##";
			grdhistory.RootTable.Columns["OrderAmount"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["OrderAmount"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["OrderAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdhistory.RootTable.Columns["OrderAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdhistory.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["ExchangeRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdhistory.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdhistory.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			ctrlGrdBar2_Load(null, null);
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
			MessageBox.Show(ex.ToString());
		}
	}

	public void CalculateWeight()
	{
		try
		{
			if (((Control)(object)CmbPackUom).Text != string.Empty && txtItemQty.Text != string.Empty)
			{
				decimal Packuom = Conversion.ToDecimal(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value);
				decimal Qty = Conversion.ToDecimal((object)txtItemQty.Text);
				txtweight.Text = Math.Round(Packuom * Qty, 2).ToString();
			}
			else
			{
				txtweight.Text = "0";
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void txtqty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
			TotalAmount();
			TotalCommissionAmount();
			CalculateTotalInformation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void TotalAmount()
	{
		try
		{
			decimal RateUOM = default(decimal);
			decimal Rate = default(decimal);
			decimal TotalAmount = default(decimal);
			if (txtweight.Text != string.Empty && ((Control)(object)CmbRateUom).Text != string.Empty && txtRate.Text != string.Empty)
			{
				txtamount.Text = string.Empty;
				decimal num = Conversion.ToDecimal((object)txtweight.Text);
				RateUOM = Conversion.ToDecimal(((UltraDropDownBase)CmbRateUom).SelectedRow.Cells[2].Value);
				Rate = Conversion.ToDecimal((object)txtRate.Text);
				TotalAmount = num / RateUOM * Rate;
				txtamount.Text = Math.Round(TotalAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount).ToString();
			}
			else
			{
				txtamount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void TotalCommissionAmount()
	{
		try
		{
			if (txtCommRate.Text != string.Empty)
			{
				if (((Control)(object)CmbCommType).Text == "Flat")
				{
					decimal CommRate2 = Conversion.ToDecimal((object)txtCommRate.Text.Trim());
					txtCommAmount.Text = Math.Round(CommRate2, clsGlobalVariables.DefaultNoofDecimalPointsForAmount).ToString(clsGlobalVariables.stringFormatsingle);
				}
				else if (((Control)(object)CmbCommType).Text == "Percent")
				{
					decimal CommRate3 = Conversion.ToDecimal((object)txtCommRate.Text.Trim());
					decimal totalItemAmount = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2));
					if (totalItemAmount > 0m)
					{
						decimal CommAmt = totalItemAmount * CommRate3 / 100m;
						txtCommAmount.Text = Math.Round(CommAmt, clsGlobalVariables.DefaultNoofDecimalPointsForAmount).ToString(clsGlobalVariables.stringFormatsingle);
					}
				}
				else if (((Control)(object)CmbCommType).Text == "Comm Weight")
				{
					decimal CommRate = Conversion.ToDecimal((object)txtCommRate.Text.Trim());
					decimal TotalWeight = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["Weight"], (AggregateFunction)2));
					if (TotalWeight > 0m)
					{
						decimal UOM = Conversion.ToDecimal((object)((Control)(object)CmbCommRateUom).Text.Trim());
						decimal CommAmt2 = TotalWeight / UOM * CommRate;
						txtCommAmount.Text = Math.Round(CommAmt2, clsGlobalVariables.DefaultNoofDecimalPointsForAmount).ToString(clsGlobalVariables.stringFormatsingle);
					}
				}
			}
			else
			{
				txtCommAmount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void combamounttype_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (combamounttype.Text == "QTY")
			{
				((Control)(object)CmbRateUom).Text = "1";
				((Control)(object)CmbRateUom).Enabled = false;
			}
			else
			{
				((Control)(object)CmbRateUom).Enabled = true;
			}
			CalculateWeight();
			TotalAmount();
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
			CalculateWeight();
			TotalAmount();
			TotalCommissionAmount();
			CalculateTotalInformation();
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
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				DetailPoId = Conversion.ToInt(item.Cells["Id"].Value);
				CmbItemName.Value = item.Cells["ItemId"].Value.ToString();
				ItemId = Conversion.ToInt((object)item.Cells["ItemId"].Value.ToString());
				CmbJobLot.Value = item.Cells["JobLotId"].Value.ToString();
				cmbPackType.Value = item.Cells["PackingTypeId"].Value.ToString();
				CmbPackUom.Value = item.Cells["PackUomId"].Value.ToString();
				txtItemQty.Text = item.Cells["ItemQty"].Value.ToString();
				txtweight.Text = item.Cells["Weight"].Value.ToString();
				txtRate.Text = item.Cells["ItemRate"].Value.ToString();
				CmbRateUom.Value = item.Cells["RateUomId"].Value.ToString();
				txtamount.Text = item.Cells["ItemAmount"].Value.ToString();
				CmbCityName.Value = item.Cells["CityId"].Value.ToString();
				txtRemarksDetail.Text = item.Cells["Remarks"].Value.ToString();
				btnplus.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		if (FormValidationDetail())
		{
			ItemId = 0;
			table.Rows[updateDetailIndex]["Id"] = DetailPoId;
			table.Rows[updateDetailIndex]["ItemId"] = CmbItemName.Value;
			table.Rows[updateDetailIndex]["ItemName"] = ((Control)(object)CmbItemName).Text;
			table.Rows[updateDetailIndex]["JobLotId"] = CmbJobLot.Value;
			table.Rows[updateDetailIndex]["JobLot"] = ((Control)(object)CmbJobLot).Text;
			table.Rows[updateDetailIndex]["PackingTypeId"] = cmbPackType.Value;
			table.Rows[updateDetailIndex]["PackingType"] = ((Control)(object)cmbPackType).Text;
			table.Rows[updateDetailIndex]["PackUomId"] = CmbPackUom.Value;
			table.Rows[updateDetailIndex]["PackUom"] = ((Control)(object)CmbPackUom).Text;
			table.Rows[updateDetailIndex]["ItemQty"] = txtItemQty.Text;
			table.Rows[updateDetailIndex]["Weight"] = txtweight.Text;
			table.Rows[updateDetailIndex]["ItemRate"] = txtRate.Text;
			table.Rows[updateDetailIndex]["RateUomId"] = CmbRateUom.Value;
			table.Rows[updateDetailIndex]["RateUom"] = ((Control)(object)CmbRateUom).Text;
			table.Rows[updateDetailIndex]["ItemAmount"] = txtamount.Text;
			table.Rows[updateDetailIndex]["CityId"] = CmbCityName.Value;
			table.Rows[updateDetailIndex]["CityName"] = ((Control)(object)CmbCityName).Text;
			table.Rows[updateDetailIndex]["Remarks"] = txtRemarksDetail.Text;
			txtExchangeRate_TextChanged(null, null);
			grd.DataSource = table;
			ResetDetail();
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
		}
	}

	private void combitem_ValueChanged(object sender, EventArgs e)
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

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		try
		{
			ResetDetail();
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
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
				Id = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(Id);
			}
			if (e.Column.Key == "Print")
			{
				CommonServices.SaleOrderSlip_1511(Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetailBind()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		try
		{
			if (grdhistory.CurrentRow == null || (int)grdhistory.CurrentRow.RowType != 0 || grdhistory.CurrentRow.Cells.Count <= 0)
			{
				return;
			}
			Id = Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value);
			SaleOrder po = new SaleOrder();
			po = SaleOrder.GetByID(Id);
			if (po != null)
			{
				DataTable dtdetailhistory = new DataTable();
				dtdetailhistory.Columns.Add("ItemName");
				dtdetailhistory.Columns.Add("JobLot");
				dtdetailhistory.Columns.Add("PackingType");
				dtdetailhistory.Columns.Add("PackUom");
				dtdetailhistory.Columns.Add("ItemQty", typeof(double));
				dtdetailhistory.Columns.Add("Weight", typeof(double));
				dtdetailhistory.Columns.Add("ItemRate", typeof(double));
				dtdetailhistory.Columns.Add("RateUOM");
				dtdetailhistory.Columns.Add("ItemAmount", typeof(double));
				dtdetailhistory.Columns.Add("City");
				dtdetailhistory.Columns.Add("ExchangeRate", typeof(double));
				dtdetailhistory.Columns.Add("FcyAmount", typeof(double));
				dtdetailhistory.Columns.Add("Remarks");
				for (int i = 0; i < po.SaleOrderDetailList.Count; i++)
				{
					dtdetailhistory.Rows.Add(po.SaleOrderDetailList[i].ItemName, po.SaleOrderDetailList[i].JobLotDescription, po.SaleOrderDetailList[i].PackingType, po.SaleOrderDetailList[i].UOMCode, po.SaleOrderDetailList[i].OrderItemQty, po.SaleOrderDetailList[i].NetWeight, po.SaleOrderDetailList[i].OrderItemRate, po.SaleOrderDetailList[i].RateUom, po.SaleOrderDetailList[i].ItemAmount, po.SaleOrderDetailList[i].CityName, po.SaleOrderDetailList[i].ExchangeRate, po.SaleOrderDetailList[i].FcyAmount, po.SaleOrderDetailList[i].RemarkDetail);
				}
				DataGridHistoryDetail.DataSource = dtdetailhistory;
				DataGridHistoryDetail.RetrieveStructure();
				DataGridHistoryDetailSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DataGridHistoryDetailSetting()
	{
		try
		{
			CommonServices.GridAutoAdjustment(AryColWidth, DataGridHistoryDetail);
			DataGridHistoryDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDetail.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			DataGridHistoryDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			DataGridHistoryDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDetail.RootTable.Columns["Weight"].FormatString = "#,##0.##";
			DataGridHistoryDetail.RootTable.Columns["Weight"].TotalFormatString = "#,##0.##";
			DataGridHistoryDetail.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDetail.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			DataGridHistoryDetail.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDetail.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDetail.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistoryDetail.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistoryDetail.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDetail.RootTable.Columns["ExchangeRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			DataGridHistoryDetail.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDetail.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDetail.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			DataGridHistoryDetail.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnattachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void txtCommrate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalCommissionAmount();
			CalculateTotalInformation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			FromDateHistory.Focus();
		}
		else
		{
			DocDate.Focus();
		}
	}

	private void txtdocno_TextChanged(object sender, EventArgs e)
	{
	}

	private void btnprint_Click(object sender, EventArgs e)
	{
		try
		{
			if (Id > 0)
			{
				CommonServices.SaleOrderSlip_1511(Id);
				return;
			}
			throw new Exception("No Record Selected");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			OrderCatagoryfill();
			SupplierNameFill();
			DeliveryTerms();
			PaymentTerms();
			CurrencyFill();
			ItemDetailFill();
			combojoblotfill();
			CmbPackingTypeFill();
			cmbCityFill();
			bindRateUomAndItemPackUom(Conversion.ToInt(CmbItemName.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ConfigurationDefault()
	{
		try
		{
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "City Area").ToList();
			if (list.Count > 0)
			{
				int CityId = Conversion.ToInt((object)list[0].ConfigKey.ToString());
				CmbCityName.Value = CityId;
			}
			List<ConfigrationsAllocation> lstJob = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Job/Lot").ToList();
			if (lstJob.Count > 0)
			{
				int JobId = Conversion.ToInt((object)lstJob[0].ConfigKey.ToString());
				CmbJobLot.Value = JobId;
			}
			List<ConfigrationsAllocation> BaseCurrencylist = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Base Currency").ToList();
			if (BaseCurrencylist.Count > 0)
			{
				int BaseCurrency = Conversion.ToInt((object)BaseCurrencylist[0].ConfigKey.ToString());
				cmbCurrency.Value = BaseCurrency;
			}
			List<ConfigrationsAllocation> BaseRatelist = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "BaseCurrencyRate").ToList();
			if (BaseRatelist.Count > 0)
			{
				decimal BaseCurrencyRate = Conversion.ToInt((object)BaseRatelist[0].ConfigKey.ToString());
				txtExchangeRate.Text = BaseCurrencyRate.ToString();
			}
		}
		catch (Exception)
		{
			MessageBox.Show("Confiquration list Error");
		}
	}

	private void btnLoadAll_Click(object sender, EventArgs e)
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

	private void grdhistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdhistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 1505);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					SaleOrder.DeleteById(Id, UserAccount.ID);
					MessageBox.Show("Record Deleted Seccessfully");
					Reset();
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

	private void combrateuom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalAmount();
			TotalCommissionAmount();
			CalculateTotalInformation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtduedays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (txtduedays.Text.Trim() != string.Empty && txtduedays.Text != null)
			{
				duedate.Value = DateTime.Now.AddDays(Conversion.ToDouble((object)txtduedays.Text.Trim()));
			}
			else
			{
				duedate.Value = DateTime.Now;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnSaveAs_Click(object sender, EventArgs e)
	{
		try
		{
			Approved = false;
			Id = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			Id = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString());
			ReadById(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_SelectionChanged(object sender, EventArgs e)
	{
		GridDetailBind();
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

	private void txtCommrate_KeyPress(object sender, KeyPressEventArgs e)
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
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtdeliverydays_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtdeliverydays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateExpiryDate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CalculateExpiryDate()
	{
		try
		{
			if (Conversion.ToInt((object)txtDeliveryDays.Text) == 0)
			{
				datExpiryDate.Value = deliverystartdate.Value;
			}
			else
			{
				datExpiryDate.Value = deliverystartdate.Value.AddDays(Conversion.ToDouble((object)txtDeliveryDays.Text.Trim()));
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void txtExchangeRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtExchangeRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetRows().Count() > 0 && Conversion.ToDecimal((object)txtExchangeRate.Text) > 0m)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					obj.Cells["ExchangeRate"].Value = txtExchangeRate.Text;
					decimal amount = Conversion.ToDecimal(obj.Cells["ItemAmount"].Value) / Conversion.ToDecimal((object)txtExchangeRate.Text);
					obj.Cells["FcyAmount"].Value = Math.Round(amount, clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount);
					obj.EndEdit();
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["ExchangeRate"].Value = 0;
					obj2.Cells["FcyAmount"].Value = 0;
					obj2.EndEdit();
				}
			}
			CalculateTotalInformation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbPackUom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
			TotalAmount();
			TotalCommissionAmount();
			CalculateTotalInformation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CalculateTotalInformation()
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				decimal TotalQty = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["ItemQty"], (AggregateFunction)2));
				decimal TotalWeight = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["Weight"], (AggregateFunction)2));
				decimal TotalAmount = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2));
				decimal TotalFcyAmount = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["FcyAmount"], (AggregateFunction)2));
				if (((UltraGridBase)CmbCommAgent).ActiveRow != null && Conversion.ToDecimal((object)txtCommAmount.Text) > 0m && Conversion.ToInt(CmbCustomerName.Value) == Conversion.ToInt(CmbCommAgent.Value))
				{
					TotalAmount -= Conversion.ToDecimal((object)txtCommAmount.Text);
				}
				txtOrderQty.Text = Math.Round(TotalQty, 2).ToString("#,##0.##");
				txtOrderWeight.Text = Math.Round(TotalWeight, 2).ToString("#,##0.##");
				txtOrderAmount.Text = TotalAmount.ToString(clsGlobalVariables.stringFormatsingle);
				txtFcyAmount.Text = TotalFcyAmount.ToString(clsGlobalVariables.stringFormatsingleForFcy);
			}
			else
			{
				txtOrderQty.Text = "0";
				txtOrderWeight.Text = "0";
				txtOrderAmount.Text = "0";
				txtFcyAmount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbCommType_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalCommissionAmount();
			CalculateTotalInformation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbCommRateUom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalCommissionAmount();
			CalculateTotalInformation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbPaymentTerm_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)CmbPaymentTerm).Text == "Cash")
			{
				txtduedays.Enabled = false;
				txtduedays.Text = string.Empty;
				duedate.Value = DocDate.Value;
			}
			else
			{
				txtduedays.Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void SendEmail_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
		try
		{
			EmailHistory val = new EmailHistory
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				EmailHistoryList = new List<EmailHistory>()
			};
			EmailHistory email = new EmailHistory();
			email.BranchesId = UserAccount.BranchesId;
			email.EntryDate = DateTime.Now;
			email.EntryUser = UserAccount.ID;
			email.EmailFrom = "Ali Rehman";
			email.EmailTo = "amirshah828925@gmail.com,ali025824@gmail.com";
			email.EmailSubject = "Mothly Attendence Report";
			email.EmailBody = "April Month Attendence Sheet";
			email.FileUrl1 = "Mothly Attendence April  2023";
			val.EmailHistoryList.Add(email);
			Email.SaveList(val);
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
				throw new Exception("Delivery Start Date Can't Be Less Than Doc Date");
			}
			CalculateExpiryDate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void SaleOrder_St_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05af: Expected O, but got Unknown
		//IL_05af: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b6: Expected O, but got Unknown
		//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bd: Expected O, but got Unknown
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Expected O, but got Unknown
		//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cb: Expected O, but got Unknown
		//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d2: Expected O, but got Unknown
		//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d9: Expected O, but got Unknown
		//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e0: Expected O, but got Unknown
		//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e7: Expected O, but got Unknown
		//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ee: Expected O, but got Unknown
		//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f5: Expected O, but got Unknown
		//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fc: Expected O, but got Unknown
		//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0603: Expected O, but got Unknown
		//IL_0603: Unknown result type (might be due to invalid IL or missing references)
		//IL_060a: Expected O, but got Unknown
		//IL_0642: Unknown result type (might be due to invalid IL or missing references)
		//IL_064c: Expected O, but got Unknown
		//IL_0684: Unknown result type (might be due to invalid IL or missing references)
		//IL_068e: Expected O, but got Unknown
		//IL_069a: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a4: Expected O, but got Unknown
		//IL_06bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c5: Expected O, but got Unknown
		//IL_06c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d0: Expected O, but got Unknown
		//IL_06d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06db: Expected O, but got Unknown
		//IL_06dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e6: Expected O, but got Unknown
		//IL_0852: Unknown result type (might be due to invalid IL or missing references)
		//IL_085c: Expected O, but got Unknown
		//IL_087e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0888: Expected O, but got Unknown
		//IL_0894: Unknown result type (might be due to invalid IL or missing references)
		//IL_089e: Expected O, but got Unknown
		//IL_08c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ca: Expected O, but got Unknown
		//IL_08d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e0: Expected O, but got Unknown
		//IL_08e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08eb: Expected O, but got Unknown
		//IL_08f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0901: Expected O, but got Unknown
		//IL_0918: Unknown result type (might be due to invalid IL or missing references)
		//IL_0922: Expected O, but got Unknown
		//IL_094f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0959: Expected O, but got Unknown
		//IL_0986: Unknown result type (might be due to invalid IL or missing references)
		//IL_0990: Expected O, but got Unknown
		//IL_099c: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a6: Expected O, but got Unknown
		//IL_0af1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afb: Expected O, but got Unknown
		//IL_0afc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b06: Expected O, but got Unknown
		//IL_0be3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bed: Expected O, but got Unknown
		//IL_121c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1226: Expected O, but got Unknown
		//IL_9355: Unknown result type (might be due to invalid IL or missing references)
		//IL_935f: Expected O, but got Unknown
		//IL_9383: Unknown result type (might be due to invalid IL or missing references)
		//IL_938d: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
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
		Appearance appearance35 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
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
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
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
		Appearance appearance146 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
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
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Steel.Sale.SaleOrder_St));
		GridEXLayout DataGridHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel11 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label2 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.cmbPackType = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbRateUom = new UltraCombo();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.CmbCityName = new UltraCombo();
		this.CmbPackUom = new UltraCombo();
		this.CmbJobLot = new UltraCombo();
		this.CmbItemName = new UltraCombo();
		this.btnplus = new System.Windows.Forms.Button();
		this.txtRemarksDetail = new System.Windows.Forms.TextBox();
		this.label47 = new System.Windows.Forms.Label();
		this.txtRate = new System.Windows.Forms.TextBox();
		this.txtamount = new System.Windows.Forms.TextBox();
		this.txtweight = new System.Windows.Forms.TextBox();
		this.txtItemQty = new System.Windows.Forms.TextBox();
		this.label40 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.combamounttype = new System.Windows.Forms.ComboBox();
		this.label42 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtCommRemarks = new System.Windows.Forms.RichTextBox();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.datExpiryDate = new System.Windows.Forms.DateTimePicker();
		this.txtFcyAmount = new System.Windows.Forms.TextBox();
		this.txtOrderQty = new System.Windows.Forms.TextBox();
		this.label43 = new System.Windows.Forms.Label();
		this.txtOrderWeight = new System.Windows.Forms.TextBox();
		this.label34 = new System.Windows.Forms.Label();
		this.txtOrderAmount = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.cmbCurrency = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.CmbOrderStatus = new UltraCombo();
		this.txtCommRate = new System.Windows.Forms.TextBox();
		this.CmbCommRateUom = new UltraCombo();
		this.label31 = new System.Windows.Forms.Label();
		this.txtCommAmount = new System.Windows.Forms.TextBox();
		this.txtRemarks = new System.Windows.Forms.RichTextBox();
		this.combstockparty = new UltraCombo();
		this.label29 = new System.Windows.Forms.Label();
		this.combrefparty = new UltraCombo();
		this.CmbCommType = new UltraCombo();
		this.txtDeliveryDays = new System.Windows.Forms.TextBox();
		this.CmbOrderCategory = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.CmbDeliveryTerm = new UltraCombo();
		this.validate = new System.Windows.Forms.DateTimePicker();
		this.label30 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.CmbPaymentTerm = new UltraCombo();
		this.deliverystartdate = new System.Windows.Forms.DateTimePicker();
		this.label32 = new System.Windows.Forms.Label();
		this.duedate = new System.Windows.Forms.DateTimePicker();
		this.txtduedays = new System.Windows.Forms.TextBox();
		this.CmbCustomerName = new UltraCombo();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.CmbCommAgent = new UltraCombo();
		this.label24 = new System.Windows.Forms.Label();
		this.txtcatsr = new System.Windows.Forms.TextBox();
		this.label17 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.txtsupprefno = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnSaveAs = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.SendEmail = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel7 = new System.Windows.Forms.Panel();
		this.DataGridHistoryDetail = new GridEX();
		this.grdhistory = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.label55 = new System.Windows.Forms.Label();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.panel15 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnShow = new System.Windows.Forms.Button();
		this.label50 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label49 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel10.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPackType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCityName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommRateUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combstockparty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combrefparty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommAgent).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistoryDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel8.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.panel14.SuspendLayout();
		this.panel15.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel13.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1076, 788);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1076, 788);
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
		this.tabPage1.Size = new System.Drawing.Size(1068, 762);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add(this.panel11);
		this.panel6.Controls.Add(this.panel10);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(2, 493);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1064, 267);
		this.panel6.TabIndex = 4;
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(0, 30);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1064, 237);
		this.panel11.TabIndex = 3;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1064, 237);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		this.panel10.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel10.Controls.Add(this.ctrlGrdBar1);
		this.panel10.Controls.Add(this.label2);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Margin = new System.Windows.Forms.Padding(2);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1064, 30);
		this.panel10.TabIndex = 2;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1015, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 26);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(8, 8);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(46, 17);
		this.label2.TabIndex = 0;
		this.label2.Text = "Detail";
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.groupBox3);
		this.panel5.Controls.Add(this.combamounttype);
		this.panel5.Controls.Add(this.label42);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(2, 362);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1064, 131);
		this.panel5.TabIndex = 3;
		this.groupBox3.BackColor = System.Drawing.Color.White;
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackType);
		this.groupBox3.Controls.Add(this.label1);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRateUom);
		this.groupBox3.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox3.Controls.Add(this.btnUpdateDetail);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCityName);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUom);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.groupBox3.Controls.Add(this.btnplus);
		this.groupBox3.Controls.Add(this.txtRemarksDetail);
		this.groupBox3.Controls.Add(this.label47);
		this.groupBox3.Controls.Add(this.txtRate);
		this.groupBox3.Controls.Add(this.txtamount);
		this.groupBox3.Controls.Add(this.txtweight);
		this.groupBox3.Controls.Add(this.txtItemQty);
		this.groupBox3.Controls.Add(this.label40);
		this.groupBox3.Controls.Add(this.label35);
		this.groupBox3.Controls.Add(this.label45);
		this.groupBox3.Controls.Add(this.label44);
		this.groupBox3.Controls.Add(this.label41);
		this.groupBox3.Controls.Add(this.label39);
		this.groupBox3.Controls.Add(this.label38);
		this.groupBox3.Controls.Add(this.label36);
		this.groupBox3.Controls.Add(this.label37);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(12, 3);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1044, 119);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Detail";
		this.cmbPackType.AllowNull = (DefaultableBoolean)1;
		this.cmbPackType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbPackType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbPackType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbPackType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPackType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPackType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackType).Location = new System.Drawing.Point(406, 37);
		((System.Windows.Forms.Control)(object)this.cmbPackType).Name = "cmbPackType";
		((System.Windows.Forms.Control)(object)this.cmbPackType).Size = new System.Drawing.Size(150, 26);
		((System.Windows.Forms.Control)(object)this.cmbPackType).TabIndex = 2;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(406, 18);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(60, 15);
		this.label1.TabIndex = 34;
		this.label1.Text = "Pack Type";
		this.CmbRateUom.AllowNull = (DefaultableBoolean)1;
		this.CmbRateUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance46).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance47).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRateUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Location = new System.Drawing.Point(881, 37);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Name = "CmbRateUom";
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Size = new System.Drawing.Size(87, 26);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).TextChanged += new System.EventHandler(combrateuom_TextChanged);
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(910, 86);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(65, 26);
		this.btnCancelUpdateDetial.TabIndex = 13;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(840, 86);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(69, 26);
		this.btnUpdateDetail.TabIndex = 12;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.CmbCityName.AllowNull = (DefaultableBoolean)1;
		this.CmbCityName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCityName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCityName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbCityName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCityName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbCityName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCityName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbCityName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCityName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCityName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCityName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCityName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCityName).Location = new System.Drawing.Point(117, 86);
		((System.Windows.Forms.Control)(object)this.CmbCityName).Name = "CmbCityName";
		((System.Windows.Forms.Control)(object)this.CmbCityName).Size = new System.Drawing.Size(126, 26);
		((System.Windows.Forms.Control)(object)this.CmbCityName).TabIndex = 9;
		this.CmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance87;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance98).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance98).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance98).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance134).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance145).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance145).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance145).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance201).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance202).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Location = new System.Drawing.Point(558, 37);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Name = "CmbPackUom";
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Size = new System.Drawing.Size(71, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TextChanged += new System.EventHandler(CmbPackUom_TextChanged);
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance53;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance58).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(245, 37);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(159, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 1;
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance65;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance70).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance71).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(9, 37);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 0;
		this.CmbItemName.ValueChanged += new System.EventHandler(combitem_ValueChanged);
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(805, 86);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(34, 26);
		this.btnplus.TabIndex = 11;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.txtRemarksDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDetail.Location = new System.Drawing.Point(245, 88);
		this.txtRemarksDetail.Name = "txtRemarksDetail";
		this.txtRemarksDetail.Size = new System.Drawing.Size(558, 23);
		this.txtRemarksDetail.TabIndex = 10;
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label47.Location = new System.Drawing.Point(248, 70);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(52, 15);
		this.label47.TabIndex = 25;
		this.label47.Text = "Remarks";
		this.txtRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRate.Location = new System.Drawing.Point(805, 39);
		this.txtRate.Name = "txtRate";
		this.txtRate.Size = new System.Drawing.Size(75, 23);
		this.txtRate.TabIndex = 6;
		this.txtRate.Text = "0";
		this.txtRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRate.TextChanged += new System.EventHandler(txtRate_TextChanged);
		this.txtRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRate_KeyPress);
		this.txtamount.BackColor = System.Drawing.Color.White;
		this.txtamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtamount.Enabled = false;
		this.txtamount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtamount.Location = new System.Drawing.Point(9, 88);
		this.txtamount.Name = "txtamount";
		this.txtamount.Size = new System.Drawing.Size(106, 23);
		this.txtamount.TabIndex = 8;
		this.txtamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtweight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtweight.Enabled = false;
		this.txtweight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtweight.Location = new System.Drawing.Point(713, 39);
		this.txtweight.Name = "txtweight";
		this.txtweight.Size = new System.Drawing.Size(90, 23);
		this.txtweight.TabIndex = 5;
		this.txtweight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtItemQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemQty.Location = new System.Drawing.Point(630, 39);
		this.txtItemQty.Name = "txtItemQty";
		this.txtItemQty.Size = new System.Drawing.Size(82, 23);
		this.txtItemQty.TabIndex = 4;
		this.txtItemQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtItemQty.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.txtItemQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label40.Location = new System.Drawing.Point(805, 18);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(58, 15);
		this.label40.TabIndex = 17;
		this.label40.Text = "Item Rate";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label35.Location = new System.Drawing.Point(117, 71);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(56, 15);
		this.label35.TabIndex = 25;
		this.label35.Text = "City/Area";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label45.Location = new System.Drawing.Point(248, 18);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(48, 15);
		this.label45.TabIndex = 23;
		this.label45.Text = "Job/Lot";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label44.Location = new System.Drawing.Point(9, 71);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(79, 15);
		this.label44.TabIndex = 23;
		this.label44.Text = "Item Amount";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label41.Location = new System.Drawing.Point(881, 18);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(61, 15);
		this.label41.TabIndex = 19;
		this.label41.Text = "Rate UOM";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label39.Location = new System.Drawing.Point(713, 18);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(46, 15);
		this.label39.TabIndex = 15;
		this.label39.Text = "Weight";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label38.Location = new System.Drawing.Point(630, 18);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(54, 15);
		this.label38.TabIndex = 13;
		this.label38.Text = "Item Qty";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label36.Location = new System.Drawing.Point(558, 18);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(61, 15);
		this.label36.TabIndex = 8;
		this.label36.Text = "Pack Uom";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label37.Location = new System.Drawing.Point(6, 18);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(67, 15);
		this.label37.TabIndex = 9;
		this.label37.Text = "Item Name";
		this.combamounttype.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.combamounttype.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.combamounttype.BackColor = System.Drawing.Color.White;
		this.combamounttype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.combamounttype.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combamounttype.FormattingEnabled = true;
		this.combamounttype.Items.AddRange(new object[2] { "Weight", "Qty" });
		this.combamounttype.Location = new System.Drawing.Point(852, 249);
		this.combamounttype.Name = "combamounttype";
		this.combamounttype.Size = new System.Drawing.Size(94, 25);
		this.combamounttype.TabIndex = 7;
		this.combamounttype.TabStop = false;
		this.combamounttype.Visible = false;
		this.combamounttype.SelectedIndexChanged += new System.EventHandler(combamounttype_SelectedIndexChanged);
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(852, 226);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(78, 15);
		this.label42.TabIndex = 27;
		this.label42.Text = "Amount Type";
		this.label42.Visible = false;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 60);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1064, 302);
		this.panel4.TabIndex = 0;
		this.groupBox1.Controls.Add(this.txtCommRemarks);
		this.groupBox1.Controls.Add(this.txtExchangeRate);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add(this.label16);
		this.groupBox1.Controls.Add(this.datExpiryDate);
		this.groupBox1.Controls.Add(this.txtFcyAmount);
		this.groupBox1.Controls.Add(this.txtOrderQty);
		this.groupBox1.Controls.Add(this.label43);
		this.groupBox1.Controls.Add(this.txtOrderWeight);
		this.groupBox1.Controls.Add(this.label34);
		this.groupBox1.Controls.Add(this.txtOrderAmount);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCurrency);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.DocDate);
		this.groupBox1.Controls.Add(this.ChkBox);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOrderStatus);
		this.groupBox1.Controls.Add(this.txtCommRate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommRateUom);
		this.groupBox1.Controls.Add(this.label31);
		this.groupBox1.Controls.Add(this.txtCommAmount);
		this.groupBox1.Controls.Add(this.txtRemarks);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combstockparty);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combrefparty);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommType);
		this.groupBox1.Controls.Add(this.txtDeliveryDays);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOrderCategory);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm);
		this.groupBox1.Controls.Add(this.validate);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentTerm);
		this.groupBox1.Controls.Add(this.deliverystartdate);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add(this.duedate);
		this.groupBox1.Controls.Add(this.txtduedays);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerName);
		this.groupBox1.Controls.Add(this.txtdocno);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommAgent);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.txtcatsr);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.label25);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.txtsupprefno);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(12, 5);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1044, 292);
		this.groupBox1.TabIndex = 22;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.txtCommRemarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCommRemarks.Location = new System.Drawing.Point(700, 128);
		this.txtCommRemarks.Name = "txtCommRemarks";
		this.txtCommRemarks.Size = new System.Drawing.Size(268, 103);
		this.txtCommRemarks.TabIndex = 25;
		this.txtCommRemarks.Text = "";
		this.txtExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtExchangeRate.Location = new System.Drawing.Point(700, 18);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.Size = new System.Drawing.Size(91, 23);
		this.txtExchangeRate.TabIndex = 18;
		this.txtExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.TextChanged += new System.EventHandler(txtExchangeRate_TextChanged);
		this.txtExchangeRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtExchangeRate_KeyPress);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label15.Location = new System.Drawing.Point(602, 22);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(78, 15);
		this.label15.TabIndex = 22;
		this.label15.Text = "Echange Rate";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(4, 213);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(67, 15);
		this.label13.TabIndex = 33;
		this.label13.Text = "Expiry Date";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(793, 22);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(72, 15);
		this.label16.TabIndex = 24;
		this.label16.Text = "Fcy Amount";
		this.datExpiryDate.CustomFormat = "dd-MMM-yy";
		this.datExpiryDate.Enabled = false;
		this.datExpiryDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.datExpiryDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datExpiryDate.Location = new System.Drawing.Point(118, 209);
		this.datExpiryDate.Name = "datExpiryDate";
		this.datExpiryDate.Size = new System.Drawing.Size(250, 23);
		this.datExpiryDate.TabIndex = 9;
		this.txtFcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmount.Enabled = false;
		this.txtFcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFcyAmount.Location = new System.Drawing.Point(866, 18);
		this.txtFcyAmount.Name = "txtFcyAmount";
		this.txtFcyAmount.Size = new System.Drawing.Size(102, 23);
		this.txtFcyAmount.TabIndex = 19;
		this.txtFcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOrderQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOrderQty.Enabled = false;
		this.txtOrderQty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtOrderQty.Location = new System.Drawing.Point(463, 129);
		this.txtOrderQty.Name = "txtOrderQty";
		this.txtOrderQty.Size = new System.Drawing.Size(131, 23);
		this.txtOrderQty.TabIndex = 14;
		this.txtOrderQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label43.Location = new System.Drawing.Point(374, 133);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(59, 15);
		this.label43.TabIndex = 30;
		this.label43.Text = "Order Qty";
		this.txtOrderWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOrderWeight.Enabled = false;
		this.txtOrderWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtOrderWeight.Location = new System.Drawing.Point(463, 156);
		this.txtOrderWeight.Name = "txtOrderWeight";
		this.txtOrderWeight.Size = new System.Drawing.Size(131, 23);
		this.txtOrderWeight.TabIndex = 15;
		this.txtOrderWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label34.Location = new System.Drawing.Point(374, 160);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(79, 15);
		this.label34.TabIndex = 28;
		this.label34.Text = "Order Weight";
		this.txtOrderAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOrderAmount.Enabled = false;
		this.txtOrderAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtOrderAmount.Location = new System.Drawing.Point(463, 182);
		this.txtOrderAmount.Name = "txtOrderAmount";
		this.txtOrderAmount.Size = new System.Drawing.Size(131, 23);
		this.txtOrderAmount.TabIndex = 16;
		this.txtOrderAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(374, 186);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(84, 15);
		this.label22.TabIndex = 26;
		this.label22.Text = "Order Amount";
		this.cmbCurrency.AllowNull = (DefaultableBoolean)1;
		this.cmbCurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance176).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance176).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance177;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance160).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance162;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance173).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance184).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance184).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance184).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance184).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance184;
		((AppearanceBase)appearance185).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Location = new System.Drawing.Point(463, 207);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Name = "cmbCurrency";
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Size = new System.Drawing.Size(131, 26);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).TabIndex = 17;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(374, 213);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(55, 15);
		this.label5.TabIndex = 20;
		this.label5.Text = "Fcy Code";
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(118, 18);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(151, 23);
		this.DocDate.TabIndex = 0;
		this.DocDate.TabStop = false;
		this.ChkBox.AutoSize = true;
		this.ChkBox.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBox.Location = new System.Drawing.Point(973, 266);
		this.ChkBox.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(67, 19);
		this.ChkBox.TabIndex = 27;
		this.ChkBox.Text = "Preview";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.CmbOrderStatus.AllowNull = (DefaultableBoolean)1;
		this.CmbOrderStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOrderStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOrderStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance135).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance135).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance136;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance137).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance137).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance141).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance142).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance142).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance142).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOrderStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOrderStatus).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbOrderStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOrderStatus).Location = new System.Drawing.Point(463, 99);
		((System.Windows.Forms.Control)(object)this.CmbOrderStatus).Name = "CmbOrderStatus";
		((System.Windows.Forms.Control)(object)this.CmbOrderStatus).Size = new System.Drawing.Size(131, 26);
		((System.Windows.Forms.Control)(object)this.CmbOrderStatus).TabIndex = 13;
		this.txtCommRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCommRate.Location = new System.Drawing.Point(865, 73);
		this.txtCommRate.Name = "txtCommRate";
		this.txtCommRate.Size = new System.Drawing.Size(103, 23);
		this.txtCommRate.TabIndex = 22;
		this.txtCommRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCommRate.TextChanged += new System.EventHandler(txtCommrate_TextChanged);
		this.txtCommRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCommrate_KeyPress);
		this.CmbCommRateUom.AllowNull = (DefaultableBoolean)1;
		this.CmbCommRateUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommRateUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommRateUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance204).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommRateUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.CmbCommRateUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommRateUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance205).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance205).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance205).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommRateUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance207).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommRateUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance207;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommRateUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance208).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance208).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance208).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommRateUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.CmbCommRateUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommRateUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance209).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommRateUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance209;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance210).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommRateUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.CmbCommRateUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommRateUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommRateUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance211;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance212).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommRateUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.CmbCommRateUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommRateUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance213).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommRateUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommRateUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.CmbCommRateUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommRateUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance215).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommRateUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.CmbCommRateUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommRateUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.CmbCommRateUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommRateUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommRateUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommRateUom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCommRateUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommRateUom).Location = new System.Drawing.Point(700, 99);
		((System.Windows.Forms.Control)(object)this.CmbCommRateUom).Name = "CmbCommRateUom";
		((System.Windows.Forms.Control)(object)this.CmbCommRateUom).Size = new System.Drawing.Size(91, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommRateUom).TabIndex = 23;
		((System.Windows.Forms.Control)(object)this.CmbCommRateUom).TextChanged += new System.EventHandler(CmbCommRateUom_TextChanged);
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(374, 105);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(73, 15);
		this.label31.TabIndex = 8;
		this.label31.Text = "Order Status";
		this.txtCommAmount.BackColor = System.Drawing.Color.White;
		this.txtCommAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommAmount.Enabled = false;
		this.txtCommAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCommAmount.Location = new System.Drawing.Point(865, 101);
		this.txtCommAmount.Name = "txtCommAmount";
		this.txtCommAmount.Size = new System.Drawing.Size(103, 23);
		this.txtCommAmount.TabIndex = 24;
		this.txtCommAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRemarks.Location = new System.Drawing.Point(118, 238);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(850, 50);
		this.txtRemarks.TabIndex = 26;
		this.txtRemarks.Text = "";
		this.combstockparty.AllowNull = (DefaultableBoolean)1;
		this.combstockparty.AutoCompleteMode = (AutoCompleteMode)4;
		this.combstockparty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combstockparty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combstockparty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.combstockparty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combstockparty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance189).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance189).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance189).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combstockparty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combstockparty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance190;
		((SpecialBoxBase)((UltraGridBase)this.combstockparty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance191).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance191).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combstockparty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.combstockparty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combstockparty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance192).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combstockparty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance193).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combstockparty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.combstockparty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combstockparty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combstockparty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance196).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combstockparty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.combstockparty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combstockparty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance197).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance197).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance197).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combstockparty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance197;
		((AppearanceBase)appearance198).TextHAlignAsString = "Left";
		((UltraGridBase)this.combstockparty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.combstockparty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combstockparty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combstockparty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.combstockparty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combstockparty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.combstockparty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combstockparty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combstockparty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combstockparty).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combstockparty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combstockparty).Location = new System.Drawing.Point(944, 381);
		((System.Windows.Forms.Control)(object)this.combstockparty).Name = "combstockparty";
		((System.Windows.Forms.Control)(object)this.combstockparty).Size = new System.Drawing.Size(144, 26);
		((System.Windows.Forms.Control)(object)this.combstockparty).TabIndex = 10;
		((System.Windows.Forms.Control)(object)this.combstockparty).TabStop = false;
		((System.Windows.Forms.Control)(object)this.combstockparty).Visible = false;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label29.Location = new System.Drawing.Point(374, 77);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(57, 15);
		this.label29.TabIndex = 3;
		this.label29.Text = "Due Date";
		this.combrefparty.AllowNull = (DefaultableBoolean)1;
		this.combrefparty.AutoCompleteMode = (AutoCompleteMode)4;
		this.combrefparty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combrefparty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combrefparty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.combrefparty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combrefparty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance103).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance103).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combrefparty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combrefparty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance104;
		((SpecialBoxBase)((UltraGridBase)this.combrefparty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combrefparty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.combrefparty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combrefparty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combrefparty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combrefparty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.combrefparty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combrefparty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combrefparty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance111).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combrefparty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.combrefparty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combrefparty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combrefparty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance113).TextHAlignAsString = "Left";
		((UltraGridBase)this.combrefparty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.combrefparty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combrefparty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combrefparty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.combrefparty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combrefparty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.combrefparty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combrefparty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combrefparty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combrefparty).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combrefparty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combrefparty).Location = new System.Drawing.Point(944, 354);
		((System.Windows.Forms.Control)(object)this.combrefparty).Name = "combrefparty";
		((System.Windows.Forms.Control)(object)this.combrefparty).Size = new System.Drawing.Size(144, 26);
		((System.Windows.Forms.Control)(object)this.combrefparty).TabIndex = 9;
		((System.Windows.Forms.Control)(object)this.combrefparty).TabStop = false;
		((System.Windows.Forms.Control)(object)this.combrefparty).Visible = false;
		this.CmbCommType.AllowNull = (DefaultableBoolean)1;
		this.CmbCommType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbCommType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance78;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbCommType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance83).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance84).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbCommType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCommType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommType).Location = new System.Drawing.Point(700, 71);
		((System.Windows.Forms.Control)(object)this.CmbCommType).Name = "CmbCommType";
		((System.Windows.Forms.Control)(object)this.CmbCommType).Size = new System.Drawing.Size(91, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommType).TabIndex = 21;
		((System.Windows.Forms.Control)(object)this.CmbCommType).TextChanged += new System.EventHandler(CmbCommType_TextChanged);
		this.txtDeliveryDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDeliveryDays.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDeliveryDays.Location = new System.Drawing.Point(118, 182);
		this.txtDeliveryDays.Name = "txtDeliveryDays";
		this.txtDeliveryDays.Size = new System.Drawing.Size(250, 23);
		this.txtDeliveryDays.TabIndex = 8;
		this.txtDeliveryDays.TextChanged += new System.EventHandler(txtdeliverydays_TextChanged);
		this.txtDeliveryDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtdeliverydays_KeyPress);
		this.CmbOrderCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbOrderCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOrderCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOrderCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance118;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance124).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance125).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOrderCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOrderCategory).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbOrderCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOrderCategory).Location = new System.Drawing.Point(118, 43);
		((System.Windows.Forms.Control)(object)this.CmbOrderCategory).Name = "CmbOrderCategory";
		((System.Windows.Forms.Control)(object)this.CmbOrderCategory).Size = new System.Drawing.Size(151, 26);
		((System.Windows.Forms.Control)(object)this.CmbOrderCategory).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbOrderCategory).Leave += new System.EventHandler(combordercat_Leave);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(837, 385);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(67, 15);
		this.label10.TabIndex = 8;
		this.label10.Text = "Stock Party";
		this.label10.Visible = false;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(837, 358);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(94, 15);
		this.label4.TabIndex = 0;
		this.label4.Text = "Reference Party";
		this.label4.Visible = false;
		this.CmbDeliveryTerm.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance90).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance90).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance91;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance96).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance97).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance97).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance97).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance99).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbDeliveryTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Location = new System.Drawing.Point(118, 127);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Name = "CmbDeliveryTerm";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Size = new System.Drawing.Size(250, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).TabIndex = 6;
		this.validate.CustomFormat = "dd-MMM-yy";
		this.validate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.validate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.validate.Location = new System.Drawing.Point(944, 412);
		this.validate.Name = "validate";
		this.validate.Size = new System.Drawing.Size(144, 23);
		this.validate.TabIndex = 3;
		this.validate.TabStop = false;
		this.validate.Visible = false;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label30.Location = new System.Drawing.Point(4, 133);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(80, 15);
		this.label30.TabIndex = 8;
		this.label30.Text = "Delivery Term";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(837, 414);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(76, 15);
		this.label27.TabIndex = 0;
		this.label27.Text = "Validite Date";
		this.label27.Visible = false;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label28.Location = new System.Drawing.Point(4, 160);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(106, 15);
		this.label28.TabIndex = 0;
		this.label28.Text = "Delivery Start Date";
		this.CmbPaymentTerm.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance163).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance163).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance164;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance165).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance165).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance165).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance166).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance166;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance195;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance206).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance2).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance2).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance2).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance2).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance171).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPaymentTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Location = new System.Drawing.Point(463, 16);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Name = "CmbPaymentTerm";
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Size = new System.Drawing.Size(131, 26);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).TabIndex = 10;
		this.CmbPaymentTerm.ValueChanged += new System.EventHandler(CmbPaymentTerm_ValueChanged);
		this.deliverystartdate.CustomFormat = "dd-MMM-yy";
		this.deliverystartdate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.deliverystartdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.deliverystartdate.Location = new System.Drawing.Point(118, 156);
		this.deliverystartdate.Name = "deliverystartdate";
		this.deliverystartdate.Size = new System.Drawing.Size(250, 23);
		this.deliverystartdate.TabIndex = 7;
		this.deliverystartdate.ValueChanged += new System.EventHandler(deliverystartdate_ValueChanged);
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label32.Location = new System.Drawing.Point(4, 186);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(79, 15);
		this.label32.TabIndex = 3;
		this.label32.Text = "Delivery Days";
		this.duedate.CustomFormat = "dd-MMM-yy";
		this.duedate.Enabled = false;
		this.duedate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.duedate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.duedate.Location = new System.Drawing.Point(463, 73);
		this.duedate.Name = "duedate";
		this.duedate.Size = new System.Drawing.Size(131, 23);
		this.duedate.TabIndex = 12;
		this.txtduedays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtduedays.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtduedays.Location = new System.Drawing.Point(463, 45);
		this.txtduedays.Name = "txtduedays";
		this.txtduedays.Size = new System.Drawing.Size(131, 23);
		this.txtduedays.TabIndex = 11;
		this.txtduedays.TextChanged += new System.EventHandler(txtduedays_TextChanged);
		this.txtduedays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtduedays_KeyPress);
		this.CmbCustomerName.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance148).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance148).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance148;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance149;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance150).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance150).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance155).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance156).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance156).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance156).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance157).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCustomerName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Location = new System.Drawing.Point(118, 71);
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Name = "CmbCustomerName";
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Size = new System.Drawing.Size(250, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).TabIndex = 4;
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtdocno.Location = new System.Drawing.Point(270, 18);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(98, 23);
		this.txtdocno.TabIndex = 1;
		this.txtdocno.TabStop = false;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.CmbCommAgent.AllowNull = (DefaultableBoolean)1;
		this.CmbCommAgent.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommAgent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommAgent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance5;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance10).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance11).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance129).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommAgent).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCommAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommAgent).Location = new System.Drawing.Point(700, 43);
		((System.Windows.Forms.Control)(object)this.CmbCommAgent).Name = "CmbCommAgent";
		((System.Windows.Forms.Control)(object)this.CmbCommAgent).Size = new System.Drawing.Size(268, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommAgent).TabIndex = 20;
		((System.Windows.Forms.Control)(object)this.CmbCommAgent).Leave += new System.EventHandler(combsalesman_Leave);
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(374, 49);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(58, 15);
		this.label24.TabIndex = 0;
		this.label24.Text = "Due Days";
		this.txtcatsr.BackColor = System.Drawing.Color.White;
		this.txtcatsr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcatsr.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtcatsr.Location = new System.Drawing.Point(270, 45);
		this.txtcatsr.Name = "txtcatsr";
		this.txtcatsr.ReadOnly = true;
		this.txtcatsr.Size = new System.Drawing.Size(98, 23);
		this.txtcatsr.TabIndex = 3;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label17.Location = new System.Drawing.Point(4, 22);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(88, 15);
		this.label17.TabIndex = 3;
		this.label17.Text = "Doc Date && No";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label20.Location = new System.Drawing.Point(602, 77);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(71, 15);
		this.label20.TabIndex = 3;
		this.label20.Text = "Comm Type";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(602, 105);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(59, 15);
		this.label21.TabIndex = 8;
		this.label21.Text = "Rate Uom";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(4, 243);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(52, 15);
		this.label33.TabIndex = 8;
		this.label33.Text = "Remarks";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(602, 133);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(91, 15);
		this.label25.TabIndex = 3;
		this.label25.Text = "Comm Remarks";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label26.Location = new System.Drawing.Point(374, 22);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(89, 15);
		this.label26.TabIndex = 8;
		this.label26.Text = "Payment Terms";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label19.Location = new System.Drawing.Point(794, 105);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(51, 15);
		this.label19.TabIndex = 0;
		this.label19.Text = "Amount";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(4, 77);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(69, 15);
		this.label12.TabIndex = 0;
		this.label12.Text = "Party Name";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(4, 105);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(73, 15);
		this.label8.TabIndex = 3;
		this.label8.Text = "Party Ref No";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(794, 77);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(69, 15);
		this.label11.TabIndex = 0;
		this.label11.Text = "Comm Rate";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(602, 49);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(98, 15);
		this.label6.TabIndex = 0;
		this.label6.Text = "Sales Man/Agent";
		this.txtsupprefno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsupprefno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtsupprefno.Location = new System.Drawing.Point(118, 101);
		this.txtsupprefno.Name = "txtsupprefno";
		this.txtsupprefno.Size = new System.Drawing.Size(250, 23);
		this.txtsupprefno.TabIndex = 5;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(4, 49);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(118, 15);
		this.label7.TabIndex = 0;
		this.label7.Text = "Order Catagory && No";
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1064, 33);
		this.panel3.TabIndex = 1;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(10, 7);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(113, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "SALE ORDER";
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1064, 25);
		this.panel2.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.btnnew, this.btnSaveAs, this.btnsave, this.btnupdate, this.btnDelete, this.btnRefresh, this.btnattachment, this.btnprint, this.SendEmail });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1064, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(55, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnSaveAs.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveAs.Name = "btnSaveAs";
		this.btnSaveAs.Size = new System.Drawing.Size(68, 24);
		this.btnSaveAs.Text = "SaveAs";
		this.btnSaveAs.Click += new System.EventHandler(btnSaveAs_Click);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(55, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnupdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(69, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.icons8_cut_16;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(64, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(70, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(94, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.btnprint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(85, 24);
		this.btnprint.Text = "1511-Print";
		this.btnprint.Click += new System.EventHandler(btnprint_Click);
		this.SendEmail.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.SendEmail.Image = (System.Drawing.Image)resources.GetObject("SendEmail.Image");
		this.SendEmail.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.SendEmail.Name = "SendEmail";
		this.SendEmail.Size = new System.Drawing.Size(69, 24);
		this.SendEmail.Text = "Send Email";
		this.SendEmail.Visible = false;
		this.SendEmail.Click += new System.EventHandler(SendEmail_Click);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(1068, 762);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.panel7.Controls.Add(this.panel13);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail);
		this.panel7.Controls.Add(this.panel14);
		this.panel7.Controls.Add(this.panel15);
		this.panel7.Controls.Add(this.panel8);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(2, 2);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1064, 758);
		this.panel7.TabIndex = 0;
		this.DataGridHistoryDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.DataGridHistoryDetail).BackColor = System.Drawing.Color.White;
		DataGridHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistoryDetail.DesignTimeLayout = DataGridHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.DataGridHistoryDetail.DynamicFiltering = true;
		this.DataGridHistoryDetail.FilterMode = (FilterMode)2;
		this.DataGridHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Location = new System.Drawing.Point(0, 482);
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Name = "DataGridHistoryDetail";
		((ScrollableBase)this.DataGridHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Size = new System.Drawing.Size(1064, 276);
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).TabIndex = 2;
		this.DataGridHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.DataGridHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistoryDetail).VisualStyle = (VisualStyle)4;
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
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(0, 147);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1064, 316);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 1;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.grdhistory.SelectionChanged += new System.EventHandler(grdhistory_SelectionChanged);
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label55);
		this.panel8.Controls.Add(this.toolStrip2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1064, 54);
		this.panel8.TabIndex = 0;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1022, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdhistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(39, 21);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.toolStrip2.BackColor = System.Drawing.Color.White;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnResetHistory, this.btnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1064, 27);
		this.toolStrip2.TabIndex = 2;
		this.toolStrip2.Text = "toolStrip2";
		this.btnResetHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetHistory.Name = "btnResetHistory";
		this.btnResetHistory.Size = new System.Drawing.Size(68, 24);
		this.btnResetHistory.Text = "Reset";
		this.btnResetHistory.Click += new System.EventHandler(btnResetHistory_Click);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label55.ForeColor = System.Drawing.Color.White;
		this.label55.Location = new System.Drawing.Point(5, 29);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(158, 19);
		this.label55.TabIndex = 3;
		this.label55.Text = "Sale Order History";
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.label3);
		this.panel14.Controls.Add(this.ctrlGrdBar2);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 125);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1064, 22);
		this.panel14.TabIndex = 49;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(1, 1);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(142, 19);
		this.label3.TabIndex = 0;
		this.label3.Text = "Filtered Records";
		this.panel15.Controls.Add(this.groupBox2);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 54);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1064, 71);
		this.panel15.TabIndex = 0;
		this.groupBox2.Controls.Add(this.rdapproveddate);
		this.groupBox2.Controls.Add(this.rdmodifydate);
		this.groupBox2.Controls.Add(this.drdocdate);
		this.groupBox2.Controls.Add(this.rdentrydate);
		this.groupBox2.Controls.Add(this.btnShow);
		this.groupBox2.Controls.Add(this.label50);
		this.groupBox2.Controls.Add(this.label51);
		this.groupBox2.Controls.Add(this.label52);
		this.groupBox2.Controls.Add(this.label53);
		this.groupBox2.Controls.Add(this.label54);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox2.Controls.Add(this.ToDocNoHistory);
		this.groupBox2.Controls.Add(this.FromDocNoHistory);
		this.groupBox2.Controls.Add(this.ToDateHistory);
		this.groupBox2.Controls.Add(this.FromDateHistory);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(2, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1045, 66);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(937, 39);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 9;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(937, 17);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(856, 17);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 6;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(856, 39);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 7;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(791, 36);
		this.btnShow.Margin = new System.Windows.Forms.Padding(2);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(58, 25);
		this.btnShow.TabIndex = 5;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(489, 19);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(34, 15);
		this.label50.TabIndex = 87;
		this.label50.Text = "Party";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(408, 19);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(64, 15);
		this.label51.TabIndex = 86;
		this.label51.Text = "To Doc No";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(328, 19);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(79, 15);
		this.label52.TabIndex = 85;
		this.label52.Text = "From Doc No";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(166, 19);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(48, 15);
		this.label53.TabIndex = 84;
		this.label53.Text = "To Date";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(5, 19);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(63, 15);
		this.label54.TabIndex = 83;
		this.label54.Text = "From Date";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance169).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance169).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance169;
		((AppearanceBase)appearance170).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance170;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance172).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance172).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance172).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance174;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance179).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance180).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance180).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance180).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance180;
		((AppearanceBase)appearance181).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(492, 35);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(296, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 4;
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(411, 37);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(331, 37);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(169, 37);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(159, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(8, 37);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(159, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label49);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel13.Location = new System.Drawing.Point(0, 463);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1064, 19);
		this.panel13.TabIndex = 50;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label49.ForeColor = System.Drawing.Color.White;
		this.label49.Location = new System.Drawing.Point(2, 3);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(269, 14);
		this.label49.TabIndex = 0;
		this.label49.Text = "Detail Information Of Above Selected Row";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1076, 788);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "SaleOrder_St";
		this.Text = "Sale Order";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(SaleOrder_St_FormClosing);
		base.Load += new System.EventHandler(PurchsaeOrder_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(PurchsaeOrder_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPackType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCityName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		this.panel4.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommRateUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combstockparty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combrefparty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommAgent).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistoryDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.panel15.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		base.ResumeLayout(false);
	}
}
