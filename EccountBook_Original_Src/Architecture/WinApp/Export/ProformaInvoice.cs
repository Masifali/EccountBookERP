using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Export;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Export;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Model.Inventory;
using Architecture.Reports;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Export;

public class ProformaInvoice : Form
{
	private Rightsobjects formrights = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtdetail = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private int RecId;

	private int DefaultDaysToLessFromHistoryFromDate;

	private int updateDetailIndex;

	private int updateDetailIndexOther;

	private bool FlagFin;

	private bool AllItemsBindonExportContract;

	private Attachment AT = new Attachment();

	public DataTable dtGPGRNS = new DataTable();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private DataTable dtitem = new DataTable();

	private DataTable listUOM = new DataTable();

	private DataTable dtpcktype = new DataTable();

	private List<ExImLcOrderPackingDetail> lstRemoveRecordDetail = new List<ExImLcOrderPackingDetail>();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private Panel panel4;

	private Panel panel3;

	private TabPage tabPage2;

	private ToolStrip toolstrip;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton Print;

	private Panel panel13;

	private Panel panel18;

	private Panel panel17;

	internal GridEX DataGridHistory;

	private ToolStrip toolStrip3;

	private ToolStripButton btnAttachment;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnDefinePaymentTerm;

	internal GridEX grdotheritems;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private Button btnplus;

	private Label label16;

	private TextBox txtQtyMTon;

	private Label label13;

	internal UltraCombo combrateuom;

	private Label label10;

	internal UltraCombo combpcktype;

	private Label label2;

	internal UltraCombo combitempck;

	private Label label1;

	internal UltraCombo combitem;

	private TextBox txtExportSoNo;

	internal UltraCombo CmbFCYCode;

	private Label label30;

	private Label label22;

	private Label label20;

	private Label label9;

	private Label label6;

	private Label label8;

	private Label label21;

	private Label label4;

	private Label label28;

	internal UltraCombo CmbExporterbank;

	internal UltraCombo CmbImporterBank;

	internal UltraCombo CmbDestinationPort;

	internal UltraCombo CmbLoadingPort;

	internal UltraCombo CmbPaymentTerms;

	internal UltraCombo CmbDeliveryTerm;

	private DateTimePicker datOrderDate;

	private DateTimePicker txtDocDate;

	internal UltraCombo CmbNotifyParty;

	internal UltraCombo CmbCustomer;

	internal UltraCombo CmbSalePerson;

	private TextBox txtSpecialInstructions;

	private TextBox txtMTon;

	private TextBox txtFCYAmount;

	private Label label29;

	private DateTimePicker txtShipmentDate;

	private TextBox txtBuyerOrderNo;

	private Label label27;

	private TextBox txtLotRefNo;

	private Label label26;

	private Label label32;

	private TextBox txtNoOfContainers;

	private RichTextBox txtProductSpecification;

	private Label label12;

	private TextBox txtNoOfBags;

	private Label label23;

	private TextBox txtCostMTon;

	private Label label25;

	private TextBox txtAmount;

	private Label label33;

	private TextBox txtRemarks;

	private Label label34;

	private ToolStripButton toolStripButton1;

	private ToolStripButton toolStripButton2;

	private Label label14;

	private Label label7;

	private Label label5;

	internal UltraCombo cmbStatus;

	private Label label15;

	private Label label18;

	private Label label17;

	private Label label19;

	private Panel panel5;

	private TabControl tabControl2;

	private TabPage tabOtherItems;

	private TabPage tabContractDetail;

	internal UltraCombo CmbCropYear;

	private Label label24;

	private Panel panel7;

	private Panel panel6;

	internal GridEX grdDetails;

	private Panel panel9;

	private Panel panel8;

	internal UltraCombo CmbOtherItem;

	private Label label31;

	private TextBox txtoQty;

	private Label label35;

	private TextBox txtoRate;

	private Label label36;

	private Label label37;

	private TextBox txtoAmount;

	private Label label38;

	private TextBox txtoRemarks;

	private Button btnoCancel;

	private Button btnoUpdate;

	private Button btnoadd;

	private ToolStripButton btnSeaPortDefine;

	private ToolStripButton btnPackingMaterialRequirement;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel10;

	private Label label53;

	private ToolStripButton btnSaveAs;

	private TextBox txtNoOfContainersDetail;

	private Label label11;

	private Label label3;

	private TextBox txtInsuraneRemarks;

	private RichTextBox txtCommodityDetail;

	private TextBox txtexpirydatedetail;

	private Label label41;

	private TextBox txtcontainersizedetail;

	private Label label40;

	private ToolStripButton btnPackingTypeLink;

	internal GridEX grddetailhistory;

	private Panel panel11;

	private Label label42;

	private Label label43;

	private Panel panel12;

	private Label label44;

	private Label label45;

	private CheckBox ChkOnCommission;

	private TextBox txtNoOfBAgsPerCntnr;

	private Label label39;

	private TextBox txtGrossWeightdetail;

	private Label label48;

	private TextBox txtPackingTotalWeight;

	private Label label47;

	private TextBox txtPackingWeight;

	private Label label46;

	private GroupBox groupBox1;

	private TextBox txtCommPercent;

	private Label label49;

	private TextBox txtCommAmount;

	internal UltraCombo cmbCommAgent;

	private Label label51;

	private Label label50;

	private CheckBox chkPrint;

	private TextBox txtPackingDetail;

	private Label label52;

	private Label label54;

	private ToolStripButton btnResetHistory;

	private ToolStripButton btnRefreshHistory;

	private GroupBox groupBox5;

	private RadioButton rdapproveddate;

	private Button btnShowHistory;

	private RadioButton rdmodifydate;

	private Label label67;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Label label73;

	private Label label89;

	private Label label90;

	private Label label91;

	internal UltraCombo CmbCustomerHistory;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	public ProformaInvoice(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void FrmExportSalesContract_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1180, 700);
			ChkOnCommission.Visible = false;
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formrights.DoHaveSaveRight;
			Print.Enabled = formrights.DoHavePrintRights;
			btnUpdate.Enabled = formrights.DoHaveUpdateRights;
			AllItemsBindonExportContract = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("AllItemsBindonExportContract"));
			_ = FlagFin;
			dtViewAttachments.Columns.Add("AttchmentPath");
			dtitem.Columns.Add("Id");
			dtitem.Columns.Add("ItemName");
			dtitem.Columns.Add("ItemCode");
			dtpcktype.Columns.Add("id");
			dtpcktype.Columns.Add("Description");
			dtpcktype.Columns.Add("PackingWeight");
			GenerateCode();
			SupplierFill();
			salesPersonFill();
			DeliveryTermFill();
			PaymentTermsFill();
			LoadingPortFill();
			ImporterandExportBankFill();
			MultiCurrencyfill();
			ItemDetailFill();
			ItemPackingTypeFill();
			StatusFill();
			CropYearFill();
			BindOtherItems();
			dtdetail.Columns.Add("Id");
			dtdetail.Columns.Add("ItemId");
			dtdetail.Columns.Add("ItemName");
			dtdetail.Columns.Add("PackTypeId");
			dtdetail.Columns.Add("PackType");
			dtdetail.Columns.Add("PackingWeight", typeof(double));
			dtdetail.Columns.Add("CropYearId");
			dtdetail.Columns.Add("CropYear");
			dtdetail.Columns.Add("NoOfContainers", typeof(double));
			dtdetail.Columns.Add("NoOfBags/Cntnr", typeof(double));
			dtdetail.Columns.Add("PackSizeId");
			dtdetail.Columns.Add("PackSize");
			dtdetail.Columns.Add("Qty/M.Ton", typeof(double));
			dtdetail.Columns.Add("NoOfBags", typeof(double));
			dtdetail.Columns.Add("PackingTotalWeight", typeof(double));
			dtdetail.Columns.Add("GrossWeight", typeof(double));
			dtdetail.Columns.Add("Rate", typeof(double));
			dtdetail.Columns.Add("RateUOMId");
			dtdetail.Columns.Add("RateUOM");
			dtdetail.Columns.Add("Amount", typeof(double));
			dtdetail.Columns.Add("PackingExpiryDate");
			dtdetail.Columns.Add("ContainerSize");
			dtdetail.Columns.Add("Remarks");
			dtdetail.Columns.Add("PackingDetail");
			grdDetails.DataSource = dtdetail;
			grdDetails.RetrieveStructure();
			grdSettings();
			dtOtherItem.Columns.Add("ItemId");
			dtOtherItem.Columns.Add("ItemName");
			dtOtherItem.Columns.Add("Qty", typeof(double));
			dtOtherItem.Columns.Add("Rate", typeof(double));
			dtOtherItem.Columns.Add("Amount", typeof(double));
			dtOtherItem.Columns.Add("Remarks");
			grdotheritems.DataSource = dtOtherItem;
			grdotheritems.RetrieveStructure();
			grdSettingsOtherItems();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			txtDocDate.Focus();
			tabControl2.SelectedIndex = 0;
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			HistoryCombosFill();
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
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
			grdDetails.RootTable.Columns["Qty/M.Ton"].Caption = "M.Ton";
			grdDetails.RootTable.Columns["PackType"].Caption = "Packing Type";
			grdDetails.RootTable.Columns["PackSize"].Caption = "Pack Uom";
			grdDetails.RootTable.Columns["Qty/M.Ton"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["Qty/M.Ton"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["Qty/M.Ton"].FormatString = "#,##0.##";
			grdDetails.RootTable.Columns["Qty/M.Ton"].TotalFormatString = "#,##0.##";
			grdDetails.RootTable.Columns["NoOfBags"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["NoOfBags"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["NoOfBags"].FormatString = "#,##0.##";
			grdDetails.RootTable.Columns["NoOfBags"].TotalFormatString = "#,##0.##";
			grdDetails.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["Amount"].FormatString = "0,0.00";
			grdDetails.RootTable.Columns["Amount"].TotalFormatString = "#,##0.##";
			grdDetails.RootTable.Columns["PackingWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["PackingWeight"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["PackingWeight"].FormatString = "#,##0.##";
			grdDetails.RootTable.Columns["PackingWeight"].TotalFormatString = "#,##0.##";
			grdDetails.RootTable.Columns["PackingTotalWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetails.RootTable.Columns["PackingTotalWeight"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["PackingTotalWeight"].FormatString = "#,##0.##";
			grdDetails.RootTable.Columns["PackingTotalWeight"].TotalFormatString = "#,##0.##";
			grdDetails.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdDetails.RootTable.Columns["Rate"].FormatString = "#,##0.##";
			grdDetails.RootTable.Columns["Id"].Visible = false;
			grdDetails.RootTable.Columns["ItemId"].Visible = false;
			grdDetails.RootTable.Columns["CropYearId"].Visible = false;
			grdDetails.RootTable.Columns["PackTypeId"].Visible = false;
			grdDetails.RootTable.Columns["PackSizeId"].Visible = false;
			grdDetails.RootTable.Columns["RateUOMId"].Visible = false;
			grdDetails.RootTable.Columns.Add("Delete");
			grdDetails.RootTable.Columns["Delete"].Key = "Delete";
			grdDetails.RootTable.Columns["Delete"].Caption = "X";
			grdDetails.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetails.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdDetails.RootTable.Columns["Delete"].ButtonText = "X";
			grdDetails.RootTable.Columns["Delete"].Width = 20;
			grdDetails.RootTable.Columns["Delete"].Position = 0;
			grdDetails.FrozenColumns = 1;
			grdDetails.RootTable.Columns["PackingExpiryDate"].Position = ((JanusCollectionBase)grdDetails.RootTable.Columns).Count - 2;
			CommonServices.GridAutoAdjustmentNew(grdDetails);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsOtherItems()
	{
		try
		{
			grdotheritems.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdotheritems.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdotheritems.RootTable.Columns["Qty"].FormatString = "0,0";
			grdotheritems.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			grdotheritems.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdotheritems.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdotheritems.RootTable.Columns["Amount"].FormatString = "0,0";
			grdotheritems.RootTable.Columns["Amount"].TotalFormatString = "#,##0.##";
			grdotheritems.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdotheritems.RootTable.Columns["Rate"].FormatString = "0,0";
			grdotheritems.RootTable.Columns["ItemId"].Visible = false;
			grdotheritems.RootTable.Columns["ItemName"].Width = 350;
			grdotheritems.RootTable.Columns["Remarks"].Width = 350;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetails_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDetails.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					ExImLcOrderPackingDetail vd = new ExImLcOrderPackingDetail();
					vd.Id = ((RecId > 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
					vd.ExImItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					vd.InvPackingMaterialTypeId = Conversion.ToInt(r.Cells["PackTypeId"].Value);
					vd.PackingWeight = Conversion.ToDouble(r.Cells["PackingWeight"].Value);
					vd.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
					vd.NoofContainer = Conversion.ToDouble(r.Cells["NoOfContainers"].Value);
					vd.NoofBagsPerContainer = Conversion.ToDouble(r.Cells["NoOfBags/Cntnr"].Value);
					vd.UOMScheduleIdOuter = Conversion.ToInt(r.Cells["PackSizeId"].Value);
					vd.UOMScheduleIdInner = Conversion.ToInt(r.Cells["PackSizeId"].Value);
					vd.NetWeight = Conversion.ToDouble(r.Cells["Qty/M.Ton"].Value) * 1000.0;
					vd.NoOfUnit = Conversion.ToDouble(r.Cells["NoOfBags"].Value);
					vd.OuterQty = Conversion.ToDouble(r.Cells["Qty/M.Ton"].Value);
					vd.InnerQty = Conversion.ToDouble(r.Cells["Qty/M.Ton"].Value);
					vd.PackingTotalWeight = Conversion.ToDouble(r.Cells["PackingTotalWeight"].Value);
					vd.GrossWeight = Conversion.ToDouble(r.Cells["GrossWeight"].Value);
					vd.UOMScheduleIdRate = Conversion.ToInt(r.Cells["RateUOMId"].Value);
					vd.RatePrice = Conversion.ToDouble(r.Cells["Rate"].Value);
					vd.TotalAmount = Conversion.ToDouble(r.Cells["Amount"].Value);
					vd.ContainerSize = Conversion.ToString(r.Cells["ContainerSize"].Value);
					vd.PackingExpiryDate = Conversion.ToString(r.Cells["PackingExpiryDate"].Value);
					vd.Other_Description = Conversion.ToString(r.Cells["Remarks"].Value);
					vd.CommodityDetail = Conversion.ToString(r.Cells["PackingDetail"].Value);
					vd.ActionTypeId = 3;
					lstRemoveRecordDetail.Add(vd);
					dtdetail.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
			}
			grdDetails.UpdateData();
			InsterAmountandQtyMtonInHeader();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtExportSoNo.Text.Trim() == string.Empty || txtExportSoNo.Text.Trim() == "0")
		{
			MessageBox.Show("DocNo Is required");
			txtExportSoNo.Focus();
			return false;
		}
		if (((Control)(object)CmbSalePerson).Text.Trim() == string.Empty)
		{
			MessageBox.Show("Sale Person Is required");
			CmbSalePerson.Focus();
			return false;
		}
		if (((Control)(object)CmbCustomer).Text.Trim() == string.Empty)
		{
			MessageBox.Show("Customer/Importer Is required");
			CmbCustomer.Focus();
			return false;
		}
		if (txtBuyerOrderNo.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Proforma Ref # Is required");
			txtBuyerOrderNo.Focus();
			return false;
		}
		if (((Control)(object)CmbDeliveryTerm).Text.Trim() == string.Empty)
		{
			MessageBox.Show("DeliveryTerms Is  required");
			CmbDeliveryTerm.Focus();
			return false;
		}
		if (((Control)(object)CmbPaymentTerms).Text.Trim() == string.Empty)
		{
			MessageBox.Show("PaymentTerms Is required");
			CmbPaymentTerms.Focus();
			return false;
		}
		if (((Control)(object)CmbLoadingPort).Text.Trim() == string.Empty)
		{
			MessageBox.Show("LoadingPort Is required");
			CmbLoadingPort.Focus();
			return false;
		}
		if (((Control)(object)CmbDestinationPort).Text.Trim() == string.Empty)
		{
			MessageBox.Show("Destination Port  Is required");
			CmbDestinationPort.Focus();
			return false;
		}
		if (((Control)(object)CmbDestinationPort).Text.Trim() == ((Control)(object)CmbLoadingPort).Text.Trim())
		{
			MessageBox.Show("Destination Port and Loaing Port cannot be same...");
			CmbDestinationPort.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtCommPercent.Text) > 0.0 && ((UltraGridBase)cmbCommAgent).ActiveRow == null)
		{
			MessageBox.Show("As you have entered 'Commission %', the 'Commission Agent' field is required. ");
			cmbCommAgent.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtCommPercent.Text) == 0.0 && ((UltraGridBase)cmbCommAgent).ActiveRow != null)
		{
			MessageBox.Show("As you have selected 'Commission agent', please enter the commission percentage.");
			txtCommPercent.Focus();
			return false;
		}
		if (((Control)(object)CmbFCYCode).Text.Trim() == string.Empty)
		{
			MessageBox.Show("Fcy Code Is  required");
			CmbFCYCode.Focus();
			return false;
		}
		if (!(Conversion.ToDouble((object)txtFCYAmount.Text) > 0.0) || txtFCYAmount.Text.Trim() == string.Empty)
		{
			MessageBox.Show("FCY Amount Must Be Greater Than 0 ThankYou");
			txtFCYAmount.Focus();
			return false;
		}
		if (!(Conversion.ToDouble((object)txtMTon.Text) > 0.0))
		{
			MessageBox.Show("M.Ton Must Be Greater Than 0 ThankYou");
			txtMTon.Focus();
			return false;
		}
		return true;
	}

	private bool DetailFormValidation()
	{
		if (((UltraGridBase)combitem).ActiveRow == null)
		{
			MessageBox.Show("Item field required");
			combitem.Focus();
			return false;
		}
		if (((UltraGridBase)combitempck).ActiveRow == null)
		{
			MessageBox.Show("Pack Type field required");
			combitempck.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtPackingWeight.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Packing Weight field required");
			txtPackingWeight.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCropYear).ActiveRow == null)
		{
			MessageBox.Show("CropYear field required");
			CmbCropYear.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtNoOfContainersDetail.Text.Trim()) == 0.0)
		{
			MessageBox.Show("NoOfContainers field required");
			txtNoOfContainersDetail.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtNoOfBAgsPerCntnr.Text.Trim()) == 0.0)
		{
			MessageBox.Show("NoOfBagsPerCntnr field required");
			txtNoOfBAgsPerCntnr.Focus();
			return false;
		}
		if (((UltraGridBase)combitempck).ActiveRow == null)
		{
			MessageBox.Show("Pack Size field required");
			combitempck.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtQtyMTon.Text.Trim()) == 0.0)
		{
			MessageBox.Show("M.Ton field required");
			txtQtyMTon.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtNoOfBags.Text.Trim()) == 0.0)
		{
			MessageBox.Show("No of Bags field required");
			txtNoOfBags.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtGrossWeightdetail.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Gross Weight field required");
			txtGrossWeightdetail.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtCostMTon.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Rate field required");
			txtCostMTon.Focus();
			return false;
		}
		if (((UltraGridBase)combrateuom).ActiveRow == null)
		{
			MessageBox.Show("Rate UOM field required");
			combrateuom.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtAmount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Amount field required");
			txtAmount.Focus();
			return false;
		}
		return true;
	}

	private bool OtherItemFormValidation()
	{
		if (((UltraGridBase)CmbOtherItem).ActiveRow == null)
		{
			MessageBox.Show("Item field required");
			CmbOtherItem.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtoQty.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Qty field required");
			txtoQty.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtoRate.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Rate field required");
			txtoRate.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtoAmount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Amount field required");
			txtoAmount.Focus();
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
			int GenerateCode = ExImLcOrder.GenerateCode(new ExImLcOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 151,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (GenerateCode > 0)
			{
				txtExportSoNo.Text = GenerateCode.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtSupplier = SupplierCustomer.ReadByOrganizationCompanyIdForExport(new SupplierCustomer
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtSupplier.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtSupplier, CmbCustomer, "Id", "CompanyName", "Customer/Impoter", false);
				DDL.BindDDLNew(dtSupplier, CmbNotifyParty, "Id", "CompanyName", "Notify Party", false);
				DDL.BindDDLNew(dtSupplier, cmbCommAgent, "Id", "CompanyName", "Commission Agent", false);
				((UltraGridBase)CmbCustomer).DisplayLayout.Bands[0].Columns[1].Width = 300;
				((UltraGridBase)CmbNotifyParty).DisplayLayout.Bands[0].Columns[1].Width = 300;
				((UltraGridBase)cmbCommAgent).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void salesPersonFill()
	{
		try
		{
			DataTable dtSalePerson = CommonServices.GetSupplierustomerByCustomerGroupId("9");
			if (dtSalePerson.Rows.Count > 0)
			{
				DDL.BindDDL(dtSalePerson, CmbSalePerson, "Id", "CompanyName", "Sales Person", ZeroIndex: false);
				((UltraGridBase)CmbSalePerson).DisplayLayout.Bands[0].Columns[1].Width = 300;
				((UltraGridBase)CmbSalePerson).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeliveryTermFill()
	{
		try
		{
			DataTable dtDeliveryTerms = ExImDeliveryTerm.Getall();
			if (dtDeliveryTerms.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtDeliveryTerms, CmbDeliveryTerm, "Id", "Code", "DeliveryTerms", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PaymentTermsFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtDeliveryTerms = ExImLcPaymentTerm.Getall(new ExImLcPaymentTerm
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtDeliveryTerms.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtDeliveryTerms, CmbPaymentTerms, "Id", "lcOrderTerm", "PaymentTerms", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadingPortFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = SeaPorts.Getall(new SeaPorts
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbLoadingPort, "Id", "PortName", "Loading Port", false);
				DDL.BindDDLNew(dt, CmbDestinationPort, "Id", "PortName", "Destination Port", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MultiCurrencyfill()
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
				DDL.BindDDLNew(dt, CmbFCYCode, "Id", "CurrencyCode", "Fcy Code", false);
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
			DataTable dt = CommonServices.CropYearGetAllService();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbCropYear, "Id", "CropYear", "Crop Year", false);
				((UltraGridBase)CmbCropYear).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ImporterandExportBankFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			List<Bank> lst = Bank.GetAll(new Bank
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (lst.Count <= 0)
			{
				return;
			}
			DataTable dtForeign = new DataTable();
			dtForeign.Columns.Add("Id");
			dtForeign.Columns.Add("BranchName");
			DataTable dtHome = new DataTable();
			dtHome.Columns.Add("Id");
			dtHome.Columns.Add("BranchName");
			for (int i = 0; i < lst.Count; i++)
			{
				if (lst[i].IsHomeland == "Foreign Country")
				{
					dtForeign.Rows.Add(lst[i].Id, lst[i].BranchName);
				}
				else if (lst[i].IsHomeland == "Home Country")
				{
					dtHome.Rows.Add(lst[i].Id, lst[i].BranchName);
				}
			}
			if (dtForeign.Rows.Count > 0)
			{
				DDL.BindDDL(dtForeign, CmbImporterBank, "Id", "BranchName", "ImporterBank", ZeroIndex: false);
			}
			if (dtHome.Rows.Count > 0)
			{
				DDL.BindDDL(dtHome, CmbExporterbank, "Id", "BranchName", "ExporterBank", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDetailFill()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt = ((!AllItemsBindonExportContract) ? Item.ReadAllForExportCombo(new Item
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			}) : CommonServices.ItemGetForComboServiceBind());
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, combitem, "Id", "ItemName", "Item Name", false);
				((UltraGridBase)combitem).DisplayLayout.Bands[0].Columns[1].Width = 400;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindOtherItems()
	{
		try
		{
			DataTable lst = CommonServices.GetItemByItemTypeId("14");
			if (lst.Rows.Count > 0)
			{
				DDL.BindDDLNew(lst, CmbOtherItem, "Id", "ItemName", "Item Name", false);
				((UltraGridBase)CmbOtherItem).DisplayLayout.Bands[0].Columns[1].Width = 400;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StatusFill()
	{
		try
		{
			DataTable dtstatus = new DataTable();
			dtstatus.Columns.Add("Id");
			dtstatus.Columns.Add("Status");
			dtstatus.Rows.Add(1, "Open");
			dtstatus.Rows.Add(2, "Complete");
			dtstatus.Rows.Add(3, "Cancel");
			DDL.BindDDL(dtstatus, cmbStatus, "Id", "Status", "Order Status", ZeroIndex: false);
			((UltraGridBase)cmbStatus).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbStatus).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemPackingTypeFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable lst = ExImPackMaterilaType.Getall(new ExImPackMaterilaType
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (lst.Rows.Count > 0)
			{
				dtpcktype.Clear();
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dtpcktype.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["Description"], lst.Rows[i]["PackingWeight"]);
				}
				if (dtpcktype.Rows.Count > 0)
				{
					DDL.BindDDL(dtpcktype, combpcktype, "Id", "Description", "Packing Type", ZeroIndex: false);
				}
			}
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
			DataTable dtUom = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(combitem.Value));
			if (dtUom.Rows.Count > 0)
			{
				DDL.BindDDL(dtUom, combitempck, "Id", "UOMCode", "PackUom", ZeroIndex: false);
				DDL.BindDDL(dtUom, combrateuom, "Id", "UOMCode", "RateUom", ZeroIndex: false);
				return;
			}
			((Control)(object)combitempck).Text = string.Empty;
			((UltraGridBase)combitempck).DataSource = null;
			((Control)(object)combrateuom).Text = string.Empty;
			((UltraGridBase)combrateuom).DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		//IL_051e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0525: Expected O, but got Unknown
		//IL_0891: Unknown result type (might be due to invalid IL or missing references)
		//IL_0898: Expected O, but got Unknown
		try
		{
			if (RecId == 0)
			{
				if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				GenerateCode();
			}
			else if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			if (!FormValidation())
			{
				return;
			}
			if (!btnUpdate.Visible && !btnUpdate.Enabled && ((Control)(object)cmbStatus).Text.Trim() != "Open")
			{
				MessageBox.Show("Status Should Be Open In Save Mode");
				cmbStatus.Focus();
				return;
			}
			grdDetails.GetTotal(grdDetails.RootTable.Columns["Amount"], (AggregateFunction)2);
			if (grdotheritems.GetRows().Length != 0)
			{
				Conversion.ToDouble(grdotheritems.GetTotal(grdotheritems.RootTable.Columns["Amount"], (AggregateFunction)2));
			}
			ExImLcOrder obj = new ExImLcOrder();
			if (RecId > 0)
			{
				obj.Id = RecId;
			}
			obj.Status = ((Control)(object)cmbStatus).Text;
			obj.LcOrderDocNo = Conversion.ToInt((object)txtExportSoNo.Text.Trim());
			obj.LcOrderDate = Conversion.ToDateTime((object)txtDocDate.Value);
			obj.SalesPersonId = Conversion.ToInt(CmbSalePerson.Value);
			obj.SupCustId = Conversion.ToInt(CmbCustomer.Value);
			obj.NotifyPartyId = Conversion.ToInt(CmbNotifyParty.Value);
			obj.LotReference = Conversion.ToString((object)txtLotRefNo.Text.Trim());
			obj.LcOrderNo = Conversion.ToString((object)txtBuyerOrderNo.Text.Trim());
			obj.SalesContratDate = Conversion.ToDateTime((object)datOrderDate.Value);
			obj.LastShipmentDate = Conversion.ToDateTime((object)txtShipmentDate.Value);
			obj.DeliveryTermId = Conversion.ToInt(CmbDeliveryTerm.Value);
			obj.PaymentTermId = Conversion.ToInt(CmbPaymentTerms.Value);
			obj.LoadingPortId = Conversion.ToInt(CmbLoadingPort.Value);
			obj.DestinationPortId = Conversion.ToInt(CmbDestinationPort.Value);
			obj.ImporterBankId = Conversion.ToInt(CmbImporterBank.Value);
			obj.ExporterBankId = Conversion.ToInt(CmbExporterbank.Value);
			obj.NoOfContainers = Conversion.ToDouble((object)txtNoOfContainers.Text.Trim());
			obj.FcurrencyId = Conversion.ToInt(CmbFCYCode.Value);
			obj.FCurrencyAmount = Conversion.ToDouble((object)txtFCYAmount.Text.Trim());
			obj.CommissionAgentId = Conversion.ToInt(cmbCommAgent.Value);
			obj.CommRate = Conversion.ToDouble((object)txtCommPercent.Text.Trim());
			obj.CommAmount = Conversion.ToDouble((object)txtCommAmount.Text.Trim());
			obj.TotalAmountWithAdls = Conversion.ToDouble((object)txtFCYAmount.Text.Trim());
			obj.NetWeightKgs = Conversion.ToDouble((object)txtMTon.Text.Trim());
			obj.CommodityDetial = Conversion.ToString((object)txtCommodityDetail.Text.Trim());
			obj.RemarksHeader = Conversion.ToString((object)txtSpecialInstructions.Text.Trim());
			obj.InsuranceRemarks = Conversion.ToString((object)txtInsuraneRemarks.Text.Trim());
			obj.ProductSpecification = Conversion.ToString((object)txtProductSpecification.Text.Trim());
			if (ChkOnCommission.Checked && FlagFin)
			{
				obj.ContractType = 1;
			}
			obj.DocumentTypeId = 151;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.EntryUser = UserAccount.ID;
			obj.ModifyUser = UserAccount.ID;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.LcOrderDetail = new List<ExImLcOrderPackingDetail>();
			obj.ExImLcOrderOtherItemslist = new List<ExImLcOrderOtherItems>();
			obj.ExImLcContractPackingMaterialDetailslist = new List<ExImLcContractPackingMaterialDetail>();
			obj.ExImLcOrderPaymentTermsDetailslist = new List<ExImLcOrderPaymentTermsDetail>();
			if (RecId > 0 && lstRemoveRecordDetail.Count > 0)
			{
				for (int i = 0; i < lstRemoveRecordDetail.Count; i++)
				{
					obj.LcOrderDetail.Add(lstRemoveRecordDetail[i]);
				}
			}
			if (grdDetails.RowCount > 0)
			{
				GridEXRow[] rows = grdDetails.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					ExImLcOrderPackingDetail vd2 = new ExImLcOrderPackingDetail();
					vd2.Id = ((RecId > 0) ? Conversion.ToInt(r2.Cells["Id"].Value) : 0);
					vd2.ExImItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
					vd2.InvPackingMaterialTypeId = Conversion.ToInt(r2.Cells["PackTypeId"].Value);
					vd2.PackingWeight = Conversion.ToDouble(r2.Cells["PackingWeight"].Value);
					vd2.CropYearId = Conversion.ToInt(r2.Cells["CropYearId"].Value);
					vd2.NoofContainer = Conversion.ToDouble(r2.Cells["NoOfContainers"].Value);
					vd2.NoofBagsPerContainer = Conversion.ToDouble(r2.Cells["NoOfBags/Cntnr"].Value);
					vd2.UOMScheduleIdOuter = Conversion.ToInt(r2.Cells["PackSizeId"].Value);
					vd2.UOMScheduleIdInner = Conversion.ToInt(r2.Cells["PackSizeId"].Value);
					vd2.NetWeight = Conversion.ToDouble(r2.Cells["Qty/M.Ton"].Value) * 1000.0;
					vd2.NoOfUnit = Conversion.ToDouble(r2.Cells["NoOfBags"].Value);
					vd2.OuterQty = Conversion.ToDouble(r2.Cells["Qty/M.Ton"].Value);
					vd2.InnerQty = Conversion.ToDouble(r2.Cells["Qty/M.Ton"].Value);
					vd2.PackingTotalWeight = Conversion.ToDouble(r2.Cells["PackingTotalWeight"].Value);
					vd2.GrossWeight = Conversion.ToDouble(r2.Cells["GrossWeight"].Value);
					vd2.UOMScheduleIdRate = Conversion.ToInt(r2.Cells["RateUOMId"].Value);
					vd2.RatePrice = Conversion.ToDouble(r2.Cells["Rate"].Value);
					vd2.TotalAmount = Conversion.ToDouble(r2.Cells["Amount"].Value);
					vd2.ContainerSize = Conversion.ToString(r2.Cells["ContainerSize"].Value);
					vd2.PackingExpiryDate = Conversion.ToString(r2.Cells["PackingExpiryDate"].Value);
					vd2.Other_Description = Conversion.ToString(r2.Cells["Remarks"].Value);
					vd2.CommodityDetail = Conversion.ToString(r2.Cells["PackingDetail"].Value);
					vd2.ActionTypeId = ((vd2.Id <= 0) ? 1 : 2);
					obj.LcOrderDetail.Add(vd2);
				}
				if (grdotheritems.RowCount > 0)
				{
					rows = grdotheritems.GetRows();
					foreach (GridEXRow r in rows)
					{
						ExImLcOrderOtherItems vd = new ExImLcOrderOtherItems();
						vd.otherItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						vd.oItemQty = Conversion.ToDecimal(r.Cells["Qty"].Value);
						vd.oItemRate = Conversion.ToDouble(r.Cells["Rate"].Value);
						vd.oItemAmount = Conversion.ToDecimal(r.Cells["Amount"].Value);
						vd.OtherItemRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
						obj.ExImLcOrderOtherItemslist.Add(vd);
					}
				}
				obj.AttachmentsList = new List<DMSAttachments>();
				obj.DeleteAttachmentsList = new List<DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<ExImLcOrder>(obj, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
				int success = ExImLcOrder.Save(obj);
				if (RecId == 0)
				{
					MessageBox.Show("Record Save Successfully");
				}
				else
				{
					MessageBox.Show("Record Update Successfully");
					FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, obj.DocumentTypeId, base.Name);
				}
				Reset();
				if (chkPrint.Checked)
				{
					GenerateReport(success);
				}
				return;
			}
			throw new Exception("Detail Grid record not found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int ID)
	{
		try
		{
			btnsave.Visible = false;
			btnSaveAs.Visible = false;
			btnUpdate.Visible = true;
			RecId = ID;
			lstRemoveRecordDetail.Clear();
			ExImLcOrder Obj = ExImLcOrder.GetByID(ID);
			if (Obj != null)
			{
				tabControl1.SelectedIndex = 0;
				((Control)(object)cmbStatus).Text = Obj.Status;
				txtExportSoNo.Text = Conversion.ToString((object)Obj.LcOrderDocNo);
				txtDocDate.Value = Conversion.ToDateTime((object)Obj.LcOrderDate);
				CmbSalePerson.Value = Obj.SalesPersonId;
				CmbCustomer.Value = Obj.SupCustId;
				CmbNotifyParty.Value = Obj.NotifyPartyId;
				cmbCommAgent.Value = Obj.CommissionAgentId;
				txtLotRefNo.Text = Conversion.ToString((object)Obj.LotReference);
				txtCommPercent.Text = Conversion.ToString((object)Obj.CommRate);
				txtCommAmount.Text = Conversion.ToString((object)Obj.CommAmount);
				txtBuyerOrderNo.Text = Conversion.ToString((object)Obj.LcOrderNo);
				datOrderDate.Value = Conversion.ToDateTime((object)Obj.SalesContratDate);
				txtShipmentDate.Value = Conversion.ToDateTime((object)Obj.LastShipmentDate);
				CmbDeliveryTerm.Value = Obj.DeliveryTermId;
				CmbPaymentTerms.Value = Obj.PaymentTermId;
				CmbLoadingPort.Value = Obj.LoadingPortId;
				CmbDestinationPort.Value = Obj.DestinationPortId;
				CmbImporterBank.Value = Obj.ImporterBankId;
				CmbExporterbank.Value = Obj.ExporterBankId;
				txtNoOfContainers.Text = Conversion.ToString((object)Obj.NoOfContainers);
				txtInsuraneRemarks.Text = Conversion.ToString((object)Obj.InsuranceRemarks);
				txtcontainersizedetail.Text = Conversion.ToString((object)Obj.LcOrderDetail[0].ContainerSize);
				CmbFCYCode.Value = Obj.FcurrencyId;
				txtFCYAmount.Text = Obj.FCurrencyAmount.ToString();
				txtMTon.Text = Conversion.ToString((object)Obj.NetWeightKgs);
				txtCommodityDetail.Text = Conversion.ToString((object)Obj.CommodityDetial);
				txtSpecialInstructions.Text = Conversion.ToString((object)Obj.RemarksHeader);
				txtProductSpecification.Text = Conversion.ToString((object)Obj.ProductSpecification);
				if (Obj.ContractType == 1)
				{
					ChkOnCommission.Checked = true;
				}
				dtdetail.Rows.Clear();
				for (int j = 0; j < Obj.LcOrderDetail.Count; j++)
				{
					dtdetail.Rows.Add(Obj.LcOrderDetail[j].Id, Obj.LcOrderDetail[j].ExImItemId, Obj.LcOrderDetail[j].ItemName, Obj.LcOrderDetail[j].InvPackingMaterialTypeId, Obj.LcOrderDetail[j].ExImPackMaterialTypeName, Obj.LcOrderDetail[j].PackingWeight, Obj.LcOrderDetail[j].CropYearId, Obj.LcOrderDetail[j].CropYear, Obj.LcOrderDetail[j].NoofContainer, Obj.LcOrderDetail[j].NoofBagsPerContainer, Obj.LcOrderDetail[j].UOMScheduleIdOuter, Obj.LcOrderDetail[j].PackUom, Obj.LcOrderDetail[j].NetWeight / 1000.0, Obj.LcOrderDetail[j].NoOfUnit, Obj.LcOrderDetail[j].PackingTotalWeight, Obj.LcOrderDetail[j].GrossWeight, Obj.LcOrderDetail[j].RatePrice, Obj.LcOrderDetail[j].UOMScheduleIdRate, Obj.LcOrderDetail[j].RateUom, Obj.LcOrderDetail[j].TotalAmount, Obj.LcOrderDetail[j].PackingExpiryDate, Obj.LcOrderDetail[j].ContainerSize, Obj.LcOrderDetail[j].Other_Description, Obj.LcOrderDetail[j].CommodityDetail);
				}
				grdDetails.DataSource = dtdetail;
				grdDetails.RetrieveStructure();
				grdSettings();
				dtOtherItem.Rows.Clear();
				for (int i = 0; i < Obj.ExImLcOrderOtherItemslist.Count; i++)
				{
					dtOtherItem.Rows.Add(Obj.ExImLcOrderOtherItemslist[i].otherItemId, Obj.ExImLcOrderOtherItemslist[i].ItemName, Obj.ExImLcOrderOtherItemslist[i].oItemQty, Obj.ExImLcOrderOtherItemslist[i].oItemRate, Obj.ExImLcOrderOtherItemslist[i].oItemAmount, Obj.ExImLcOrderOtherItemslist[i].OtherItemRemarks);
				}
				grdotheritems.DataSource = dtOtherItem;
				grdotheritems.RetrieveStructure();
				grdSettingsOtherItems();
				FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			}
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
			RecId = 0;
			GenerateCode();
			ChkOnCommission.Checked = false;
			((Control)(object)cmbCommAgent).Text = string.Empty;
			txtCommAmount.Text = string.Empty;
			txtCommPercent.Text = string.Empty;
			((Control)(object)CmbCustomer).Text = string.Empty;
			((Control)(object)CmbNotifyParty).Text = string.Empty;
			((Control)(object)CmbFCYCode).Text = string.Empty;
			txtFCYAmount.Text = string.Empty;
			txtSpecialInstructions.Text = string.Empty;
			txtMTon.Text = string.Empty;
			txtCommodityDetail.Text = string.Empty;
			((Control)(object)CmbDeliveryTerm).Text = string.Empty;
			((Control)(object)CmbPaymentTerms).Text = string.Empty;
			((Control)(object)CmbLoadingPort).Text = string.Empty;
			((Control)(object)CmbDestinationPort).Text = string.Empty;
			txtInsuraneRemarks.Text = string.Empty;
			((Control)(object)CmbImporterBank).Text = string.Empty;
			((Control)(object)CmbExporterbank).Text = string.Empty;
			((Control)(object)combitem).Text = string.Empty;
			((Control)(object)combitempck).Text = string.Empty;
			((Control)(object)combrateuom).Text = string.Empty;
			((Control)(object)combpcktype).Text = string.Empty;
			txtQtyMTon.Text = string.Empty;
			txtBuyerOrderNo.Text = string.Empty;
			txtLotRefNo.Text = string.Empty;
			txtNoOfContainers.Text = string.Empty;
			txtProductSpecification.Text = string.Empty;
			txtcontainersizedetail.Text = string.Empty;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			btnSaveAs.Visible = false;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			dtdetail.Rows.Clear();
			grdotheritems.ClearStructure();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			txtDocDate.Focus();
			tabControl2.SelectedIndex = 0;
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
			((Control)(object)combitem).Text = string.Empty;
			((Control)(object)combpcktype).Text = string.Empty;
			txtQtyMTon.Text = string.Empty;
			((Control)(object)combitempck).Text = string.Empty;
			((Control)(object)CmbCropYear).Text = string.Empty;
			txtNoOfBags.Text = string.Empty;
			txtNoOfBAgsPerCntnr.Text = string.Empty;
			txtCostMTon.Text = string.Empty;
			((Control)(object)combrateuom).Text = string.Empty;
			txtGrossWeightdetail.Text = string.Empty;
			txtPackingWeight.Text = string.Empty;
			txtPackingTotalWeight.Text = string.Empty;
			txtexpirydatedetail.Text = string.Empty;
			txtNoOfContainers.Text = string.Empty;
			txtAmount.Text = string.Empty;
			txtRemarks.Text = string.Empty;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetOtherItems()
	{
		try
		{
			((Control)(object)CmbOtherItem).Text = string.Empty;
			txtoQty.Text = string.Empty;
			txtoRate.Text = string.Empty;
			txtoAmount.Text = string.Empty;
			txtoRemarks.Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeIds = "151";
			obj.CanViewAllRecord = formrights.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
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
			obj.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			obj.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			obj.ApprovedFilter = "All";
			DataTable dt = ExImLcOrder.FormHistory(obj);
			if (dt.Rows.Count > 0)
			{
				DataTable dtGrid = new DataTable();
				dtGrid.Columns.Add("Id");
				dtGrid.Columns.Add("DocNo", typeof(int));
				dtGrid.Columns.Add("DocDate", typeof(DateTime));
				dtGrid.Columns.Add("Pri_Ref#");
				dtGrid.Columns.Add("Pri_RefDate", typeof(DateTime));
				dtGrid.Columns.Add("Customer/Importer");
				dtGrid.Columns.Add("NotifyParty");
				dtGrid.Columns.Add("Lot Ref No");
				dtGrid.Columns.Add("PaymentTerm");
				dtGrid.Columns.Add("DeliveryTerm");
				dtGrid.Columns.Add("NetMTon", typeof(double));
				dtGrid.Columns.Add("FcyCode");
				dtGrid.Columns.Add("FcyAmount", typeof(double));
				dtGrid.Columns.Add("NoOfContainers", typeof(double));
				dtGrid.Columns.Add("LoadingPort");
				dtGrid.Columns.Add("DestinationPort");
				dtGrid.Columns.Add("InsuranceRemarks");
				dtGrid.Columns.Add("NoOfAttachments");
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtGrid.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["LcOrderDocNo"], Conversion.ToDateTime(dt.Rows[i]["LcOrderDate"]).ToString("dd-MMM-yyyy"), dt.Rows[i]["LcOrderNo"], Conversion.ToDateTime(dt.Rows[i]["SalesContratDate"]).ToString("dd-MMM-yyyy"), dt.Rows[i]["CustomerName"], dt.Rows[i]["NotifyParty"], dt.Rows[i]["LotReference"], dt.Rows[i]["PaymentTerm"], dt.Rows[i]["DeliveryTerm"], dt.Rows[i]["NetKgs"], dt.Rows[i]["CurrencyCode"], dt.Rows[i]["FCurrencyAmount"], dt.Rows[i]["NoOfContr"], dt.Rows[i]["LoadingPort"], dt.Rows[i]["PortName"], dt.Rows[i]["InsuranceRemarks"], dt.Rows[i]["NoOfAttachments"]);
				}
				DataGridHistory.DataSource = dtGrid;
				DataGridHistory.RetrieveStructure();
				GridSetting();
			}
			else
			{
				DataGridHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSetting()
	{
		try
		{
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns.Add("Print");
			DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridHistory.RootTable.Columns["Print"].Width = 50;
			DataGridHistory.RootTable.Columns.Add("Print523");
			DataGridHistory.RootTable.Columns["Print523"].Caption = "Print523";
			DataGridHistory.RootTable.Columns["Print523"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print523"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print523"].ButtonText = "Print523";
			DataGridHistory.RootTable.Columns["Print523"].Width = 50;
			DataGridHistory.RootTable.Columns.Add("SaveAs");
			DataGridHistory.RootTable.Columns["SaveAs"].Caption = "SaveAs";
			DataGridHistory.RootTable.Columns["SaveAs"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["SaveAs"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["SaveAs"].ButtonText = "SaveAs";
			DataGridHistory.RootTable.Columns["SaveAs"].Width = 50;
			DataGridHistory.RootTable.Columns.Add("Edit");
			DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Width = 50;
			DataGridHistory.RootTable.Columns["Print"].Position = 0;
			DataGridHistory.RootTable.Columns["Print523"].Position = 1;
			DataGridHistory.RootTable.Columns["Edit"].Position = 2;
			DataGridHistory.RootTable.Columns["SaveAs"].Position = 3;
			DataGridHistory.FrozenColumns = 4;
			DataGridHistory.RootTable.Columns["NetMTon"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["NetMTon"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["NetMTon"].FormatString = "#,#";
			DataGridHistory.RootTable.Columns["FcyAmount"].FormatString = "#,#";
			DataGridHistory.RootTable.Columns["NetMTon"].TotalFormatString = "#,#";
			DataGridHistory.RootTable.Columns["FcyAmount"].TotalFormatString = "#,#";
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			CommonServices.GridAutoAdjustmentNew(DataGridHistory);
			ctrlGrdBar1_Load(null, null);
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

	private void txtLcOrderNo_Leave(object sender, EventArgs e)
	{
	}

	private void GenerateReport(int PrintId)
	{
		try
		{
			CommonServices.ExportSalesContractExport501(PrintId);
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

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), 151);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnImporterBank_Click(object sender, EventArgs e)
	{
		new AcfrmDefineBank(UserAccount).Show();
	}

	private void btnExporterBank_Click(object sender, EventArgs e)
	{
		new AcfrmDefineBank(UserAccount).Show();
	}

	private void btnDeliveryTerm_Click(object sender, EventArgs e)
	{
		new DeliveryTerms(UserAccount).Show();
	}

	private void btnPaymentTerms_Click(object sender, EventArgs e)
	{
		new PaymentTerms(UserAccount).Show();
	}

	private void btnLoadingPort_Click(object sender, EventArgs e)
	{
		new SeaPortsDefine(UserAccount).Show();
	}

	private void btnDestination_Click(object sender, EventArgs e)
	{
		new SeaPortsDefine(UserAccount).Show();
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		btnsave_Click(null, null);
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
			DataGridHistory.DataSource = null;
			grddetailhistory.DataSource = null;
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
			DataTable dt = ExImLcOrder.GetDataForDropDownFromExportContract(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["ActivityType"]) == "Customer")
					{
						dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
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

	private void btnShowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryFill();
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
			txtDocDate.Focus();
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

	private void FrmExportSalesContract_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0 && btnsave.Visible)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					btnnew_Click(null, null);
				}
				else
				{
					btnnew_Click(null, null);
				}
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
			if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && RecId > 0)
			{
				btnUpdate_Click(null, null);
			}
			if (e.Control)
			{
				_ = e.KeyCode;
				_ = 80;
			}
		}
		catch (Exception)
		{
			throw;
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		RecId = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value);
		ReadById(RecId);
	}

	private void btnLoadAll_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		new DefineMultiCurrency(UserAccount).Show();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			SupplierFill();
			DeliveryTermFill();
			PaymentTermsFill();
			LoadingPortFill();
			ImporterandExportBankFill();
			MultiCurrencyfill();
			ItemDetailFill();
			ItemPackingTypeFill();
			BindOtherItems();
			CropYearFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Print_Click(object sender, EventArgs e)
	{
		GenerateReport(RecId);
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		btnplus.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelUpdateDetial.Visible = false;
		ResetDetail();
	}

	private void combitem_Leave_1(object sender, EventArgs e)
	{
		try
		{
			bindRateUomAndItemPackUom();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetails_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdDetails.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				combitem.Value = item.Cells["ItemId"].Value.ToString();
				bindRateUomAndItemPackUom();
				combpcktype.Value = item.Cells["PackTypeId"].Value;
				txtPackingWeight.Text = item.Cells["PackingWeight"].Value.ToString();
				CmbCropYear.Value = item.Cells["CropYearId"].Value;
				((Control)(object)CmbCropYear).Text = item.Cells["CropYear"].Text;
				txtNoOfContainersDetail.Text = item.Cells["NoOfContainers"].Value.ToString();
				txtNoOfBAgsPerCntnr.Text = item.Cells["NoOfBags/Cntnr"].Value.ToString();
				combitempck.Value = item.Cells["PackSizeId"].Value;
				txtQtyMTon.Text = item.Cells["Qty/M.Ton"].Value.ToString();
				txtNoOfBags.Text = item.Cells["NoOfBags"].Value.ToString();
				txtPackingTotalWeight.Text = item.Cells["PackingTotalWeight"].Value.ToString();
				txtGrossWeightdetail.Text = item.Cells["GrossWeight"].Value.ToString();
				combrateuom.Value = item.Cells["RateUOMId"].Value;
				txtCostMTon.Text = item.Cells["Rate"].Value.ToString();
				txtAmount.Text = item.Cells["Amount"].Value.ToString();
				txtexpirydatedetail.Text = item.Cells["PackingExpiryDate"].Value.ToString();
				txtcontainersizedetail.Text = item.Cells["ContainerSize"].Value.ToString();
				txtRemarks.Text = item.Cells["Remarks"].Value.ToString();
				txtPackingDetail.Text = item.Cells["PackingDetail"].Value.ToString();
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

	private void AddtoGrid()
	{
		try
		{
			if (DetailFormValidation())
			{
				dtdetail.Rows.Add(0, combitem.Value, ((Control)(object)combitem).Text, combpcktype.Value, Conversion.ToString(((UltraDropDownBase)combpcktype).SelectedRow.Cells[1].Value), Conversion.ToString((object)txtPackingWeight.Text), CmbCropYear.Value, ((Control)(object)CmbCropYear).Text, Conversion.ToDouble((object)txtNoOfContainersDetail.Text), Conversion.ToDouble((object)txtNoOfBAgsPerCntnr.Text), combitempck.Value, ((Control)(object)combitempck).Text, Conversion.ToDouble((object)txtQtyMTon.Text), Conversion.ToDouble((object)txtNoOfBags.Text), Conversion.ToDouble((object)txtPackingTotalWeight.Text), Conversion.ToDouble((object)txtGrossWeightdetail.Text), Conversion.ToDouble((object)txtCostMTon.Text), combrateuom.Value, Conversion.ToString(((UltraDropDownBase)combrateuom).SelectedRow.Cells[1].Value), Conversion.ToDouble((object)txtAmount.Text), txtexpirydatedetail.Text, txtcontainersizedetail.Text, txtRemarks.Text, txtPackingDetail.Text);
				grdDetails.DataSource = dtdetail;
				grdDetails.RetrieveStructure();
				grdSettings();
				ResetDetail();
				combitem.Focus();
				InsterAmountandQtyMtonInHeader();
			}
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
			AddtoGrid();
			CalculateCommission();
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
			if (Conversion.ToDouble((object)txtNoOfContainersDetail.Text) > 0.0 && Conversion.ToDouble((object)txtNoOfBAgsPerCntnr.Text) > 0.0)
			{
				double OuterUom = 0.0;
				double TotalNoOfBags = Conversion.ToDouble((object)txtNoOfContainersDetail.Text) * Conversion.ToDouble((object)txtNoOfBAgsPerCntnr.Text);
				txtNoOfBags.Text = TotalNoOfBags.ToString();
				double PackingWeight = Conversion.ToDouble((object)txtPackingWeight.Text);
				txtPackingTotalWeight.Text = (TotalNoOfBags * PackingWeight).ToString();
				if (((UltraGridBase)combitempck).ActiveRow != null)
				{
					OuterUom = Conversion.ToDouble(((UltraDropDownBase)combitempck).SelectedRow.Cells[2].Value);
					txtQtyMTon.Text = (TotalNoOfBags * OuterUom / 1000.0).ToString();
					txtGrossWeightdetail.Text = (TotalNoOfBags * OuterUom + TotalNoOfBags * PackingWeight).ToString();
				}
				else
				{
					txtNoOfBags.Text = 0.ToString();
					txtQtyMTon.Text = 0.ToString();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculatAmount()
	{
		try
		{
			double ItemRate = 0.0;
			double ItemRateUom = 0.0;
			double NetWeight = 0.0;
			txtAmount.Text = 0.ToString();
			if (((UltraGridBase)combrateuom).ActiveRow != null)
			{
				ItemRate = Conversion.ToDouble((object)txtCostMTon.Text);
				ItemRateUom = Conversion.ToDouble(((UltraDropDownBase)combrateuom).SelectedRow.Cells[2].Value);
				NetWeight = Conversion.ToDouble((object)txtQtyMTon.Text);
				if (ItemRate > 0.0 && ItemRateUom > 0.0 && NetWeight > 0.0)
				{
					double weightkgs = NetWeight * 1000.0;
					double amount = ItemRate / ItemRateUom * weightkgs;
					txtAmount.Text = Math.Round(amount).ToString();
				}
			}
			else
			{
				txtAmount.Text = 0.ToString();
			}
			CalculateCommission();
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
			if (DetailFormValidation())
			{
				dtdetail.Rows[updateDetailIndex]["ItemId"] = combitem.Value;
				dtdetail.Rows[updateDetailIndex]["ItemName"] = ((Control)(object)combitem).Text;
				dtdetail.Rows[updateDetailIndex]["PackTypeId"] = combpcktype.Value;
				dtdetail.Rows[updateDetailIndex]["PackType"] = ((Control)(object)combpcktype).Text;
				dtdetail.Rows[updateDetailIndex]["PackingWeight"] = txtPackingWeight.Text;
				dtdetail.Rows[updateDetailIndex]["CropYearId"] = CmbCropYear.Value;
				dtdetail.Rows[updateDetailIndex]["CropYear"] = ((Control)(object)CmbCropYear).Text;
				dtdetail.Rows[updateDetailIndex]["NoOfContainers"] = txtNoOfContainersDetail.Text;
				dtdetail.Rows[updateDetailIndex]["NoOfBags/Cntnr"] = txtNoOfBAgsPerCntnr.Text;
				dtdetail.Rows[updateDetailIndex]["PackSizeId"] = combitempck.Value;
				dtdetail.Rows[updateDetailIndex]["PackSize"] = ((Control)(object)combitempck).Text;
				dtdetail.Rows[updateDetailIndex]["Qty/M.Ton"] = txtQtyMTon.Text;
				dtdetail.Rows[updateDetailIndex]["NoOfBags"] = txtNoOfBags.Text;
				dtdetail.Rows[updateDetailIndex]["PackingTotalWeight"] = txtPackingTotalWeight.Text;
				dtdetail.Rows[updateDetailIndex]["GrossWeight"] = txtGrossWeightdetail.Text;
				dtdetail.Rows[updateDetailIndex]["RateUOM"] = ((Control)(object)combrateuom).Text;
				dtdetail.Rows[updateDetailIndex]["Rate"] = txtCostMTon.Text;
				dtdetail.Rows[updateDetailIndex]["RateUOMId"] = combrateuom.Value;
				dtdetail.Rows[updateDetailIndex]["RateUOM"] = ((Control)(object)combrateuom).Text;
				dtdetail.Rows[updateDetailIndex]["Amount"] = txtAmount.Text;
				dtdetail.Rows[updateDetailIndex]["PackingExpiryDate"] = txtexpirydatedetail.Text;
				dtdetail.Rows[updateDetailIndex]["ContainerSize"] = txtcontainersizedetail.Text;
				dtdetail.Rows[updateDetailIndex]["Remarks"] = txtRemarks.Text;
				dtdetail.Rows[updateDetailIndex]["PackingDetail"] = txtPackingDetail.Text;
				grdDetails.DataSource = dtdetail;
				btnplus.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				ResetDetail();
				combitem.Focus();
				InsterAmountandQtyMtonInHeader();
				CalculateCommission();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQtyMTon_TextChanged(object sender, EventArgs e)
	{
		CalculateWeight();
		CalculatAmount();
	}

	private void combitempck_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
			CalculatAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtCostMTon_TextChanged(object sender, EventArgs e)
	{
		CalculateWeight();
		CalculatAmount();
		CommonServices.CommasApplyWhileTyping(sender, e);
	}

	private void combrateuom_TextChanged(object sender, EventArgs e)
	{
		CalculateWeight();
		CalculatAmount();
	}

	private void InsterAmountandQtyMtonInHeader()
	{
		try
		{
			double OtherAmount = 0.0;
			object total = grdDetails.GetTotal(grdDetails.RootTable.Columns["Amount"], (AggregateFunction)2);
			if (grdotheritems.GetRows().Length != 0)
			{
				OtherAmount = Conversion.ToDouble(grdotheritems.GetTotal(grdotheritems.RootTable.Columns["Amount"], (AggregateFunction)2));
			}
			object NoOfContainers = grdDetails.GetTotal(grdDetails.RootTable.Columns["NoOfContainers"], (AggregateFunction)2);
			object Qty = grdDetails.GetTotal(grdDetails.RootTable.Columns["Qty/M.Ton"], (AggregateFunction)2);
			txtMTon.Text = Qty.ToString();
			txtNoOfContainers.Text = NoOfContainers.ToString();
			double TotalAmount = Conversion.ToDouble(total) + Conversion.ToDouble((object)OtherAmount);
			txtFCYAmount.Text = TotalAmount.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQtyMTon_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextdecimelFunction(sender, e);
	}

	private void txtNoOfBags_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextdecimelFunction(sender, e);
	}

	private void txtCostMTon_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextdecimelFunction(sender, e);
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Print")
			{
				DataTable dt2 = new DataTable();
				dt2 = ExportPdfReport.ExpRptSalesContractExport_501(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value),
					ApprovedFilter = "All"
				});
				if (dt2 == null || dt2.Rows.Count == 0)
				{
					MessageBox.Show("Not Record Found For Display");
				}
				else
				{
					Reporting val = new Reporting();
					val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
					val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
					val.ShowReportWithDataTable(dt2, "501-ExpRptSalesContractExport.rpt");
				}
			}
			if (e.Column.Key == "Print523")
			{
				DataTable dt = new DataTable();
				dt = ExportPdfReport.ExpRptSalesContractExportRegister_523(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					ExImLcOrderId = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value)
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("Not Record Found For Display");
				}
				else
				{
					Reporting val2 = new Reporting();
					val2.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
					val2.RptPerameter("CompanyName", UserAccount.CompName.ToString());
					val2.ShowReportWithDataTable(dt, "523-ExpRptSalesContractWiseInvoiceRegister.rpt");
				}
			}
			if (e.Column.Key == "Edit")
			{
				RecId = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value);
				ReadById(RecId);
			}
			if (e.Column.Key == "SaveAs")
			{
				RecId = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value);
				ReadById(RecId);
				btnSaveAs.Visible = true;
				btnsave.Visible = false;
				btnUpdate.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OtherItemsCalculations()
	{
		try
		{
			double Qty = Conversion.ToDouble((object)txtoQty.Text.Trim());
			double Rate = Conversion.ToDouble((object)txtoRate.Text.Trim());
			if (Qty > 0.0 && Rate > 0.0)
			{
				txtoAmount.Text = (Qty * Rate).ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtoQty_TextChanged(object sender, EventArgs e)
	{
		OtherItemsCalculations();
	}

	private void txtoRate_TextChanged(object sender, EventArgs e)
	{
		OtherItemsCalculations();
	}

	private void btnoadd_Click(object sender, EventArgs e)
	{
		try
		{
			if (OtherItemFormValidation())
			{
				dtOtherItem.Rows.Add(CmbOtherItem.Value, ((Control)(object)CmbOtherItem).Text, Conversion.ToDouble((object)txtoQty.Text.Trim()), Conversion.ToDouble((object)txtoRate.Text.Trim()), Conversion.ToDouble((object)txtoAmount.Text.Trim()), txtoRemarks.Text.Trim());
				grdotheritems.DataSource = dtOtherItem;
				grdotheritems.RetrieveStructure();
				grdSettingsOtherItems();
				ResetOtherItems();
				InsterAmountandQtyMtonInHeader();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnoUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (OtherItemFormValidation())
			{
				dtOtherItem.Rows[updateDetailIndexOther]["ItemId"] = CmbOtherItem.Value;
				dtOtherItem.Rows[updateDetailIndexOther]["ItemName"] = ((Control)(object)CmbOtherItem).Text;
				dtOtherItem.Rows[updateDetailIndexOther]["Qty"] = txtoQty.Text;
				dtOtherItem.Rows[updateDetailIndexOther]["Rate"] = txtoRate.Text;
				dtOtherItem.Rows[updateDetailIndexOther]["Amount"] = txtoAmount.Text;
				dtOtherItem.Rows[updateDetailIndexOther]["Remarks"] = txtoRemarks.Text;
				grdotheritems.DataSource = dtOtherItem;
				btnoadd.Visible = true;
				btnoUpdate.Visible = false;
				btnoCancel.Visible = false;
				ResetOtherItems();
				txtoQty.Focus();
				InsterAmountandQtyMtonInHeader();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdotheritems_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdotheritems.CurrentRow;
			if (item != null)
			{
				updateDetailIndexOther = item.RowIndex;
				CmbOtherItem.Value = item.Cells["ItemId"].Value.ToString();
				((Control)(object)CmbOtherItem).Text = item.Cells["ItemName"].Text.ToString();
				txtoQty.Text = item.Cells["Qty"].Value.ToString();
				txtoRate.Text = item.Cells["Rate"].Value.ToString();
				txtoAmount.Text = item.Cells["Amount"].Value.ToString();
				txtoRemarks.Text = item.Cells["Remarks"].Value.ToString();
				btnoadd.Visible = false;
				btnoUpdate.Visible = true;
				btnoCancel.Visible = true;
				txtoQty.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnoCancel_Click(object sender, EventArgs e)
	{
		btnoadd.Visible = true;
		btnoUpdate.Visible = false;
		btnoCancel.Visible = false;
		ResetOtherItems();
	}

	private void btnSeaPortDefine_Click(object sender, EventArgs e)
	{
		new SeaPortsDefine(UserAccount).Show();
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		try
		{
			SupplierFill();
			salesPersonFill();
			DeliveryTermFill();
			PaymentTermsFill();
			LoadingPortFill();
			ImporterandExportBankFill();
			MultiCurrencyfill();
			ItemDetailFill();
			ItemPackingTypeFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPackingMaterialRequirement_Click(object sender, EventArgs e)
	{
		try
		{
			new PackingMaterialRequirement(UserAccount).Show();
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DataGridHistory).Name);
			if (fs != null)
			{
				DataGridHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSaveAs_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0;
			btnsave_Click(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtNoOfBags_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
			CalculatAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtNoOfContainersDetail_KeyPress(object sender, KeyPressEventArgs e)
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

	private void btnPackingTypeLink_Click(object sender, EventArgs e)
	{
		try
		{
			new DefineExportPackingType(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && (int)item.RowType != 5 && item.Cells.Count > 0)
			{
				GetDetailGrdByHeadId();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailGrdByHeadId()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("ItemName");
			dt.Columns.Add("PackingType");
			dt.Columns.Add("PackingWeight");
			dt.Columns.Add("CropYear");
			dt.Columns.Add("NoOfContainers", typeof(double));
			dt.Columns.Add("NoOfBags/Container", typeof(double));
			dt.Columns.Add("PackUom");
			dt.Columns.Add("M.Ton", typeof(double));
			dt.Columns.Add("NoOfBags", typeof(double));
			dt.Columns.Add("TotalPackingWeight", typeof(double));
			dt.Columns.Add("GrossWeight", typeof(double));
			dt.Columns.Add("Rate", typeof(double));
			dt.Columns.Add("RateUOM");
			dt.Columns.Add("Amount", typeof(double));
			dt.Columns.Add("PackingExpiryDate");
			dt.Columns.Add("ContainerSize");
			dt.Columns.Add("Remarks");
			dt.Clear();
			ExImLcOrder Obj = ExImLcOrder.GetByID(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
			if (Obj.LcOrderDetail.Count > 0)
			{
				dt.Rows.Clear();
				for (int i = 0; i < Obj.LcOrderDetail.Count; i++)
				{
					dt.Rows.Add(Obj.LcOrderDetail[i].ItemName, Obj.LcOrderDetail[i].ExImPackMaterialTypeName, Obj.LcOrderDetail[i].PackingWeight, Obj.LcOrderDetail[i].CropYear, Obj.LcOrderDetail[i].NoofContainer, Obj.LcOrderDetail[i].NoofBagsPerContainer, Obj.LcOrderDetail[i].PackUom, Obj.LcOrderDetail[i].NetWeight / 1000.0, Obj.LcOrderDetail[i].NoOfUnit, Obj.LcOrderDetail[i].PackingTotalWeight, Obj.LcOrderDetail[i].GrossWeight, Obj.LcOrderDetail[i].RatePrice, Obj.LcOrderDetail[i].RateUom, Obj.LcOrderDetail[i].TotalAmount, Obj.LcOrderDetail[i].PackingExpiryDate, Obj.LcOrderDetail[i].ContainerSize, Obj.LcOrderDetail[i].Other_Description);
				}
				grddetailhistory.DataSource = dt;
				grddetailhistory.RetrieveStructure();
				CommonServices.GridAutoAdjustmentNew(grddetailhistory);
				grddetailhistory.RootTable.Columns["M.Ton"].AggregateFunction = (AggregateFunction)2;
				grddetailhistory.RootTable.Columns["NoOfBags"].AggregateFunction = (AggregateFunction)2;
				grddetailhistory.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
				grddetailhistory.RootTable.Columns["M.Ton"].FormatString = "#,#";
				grddetailhistory.RootTable.Columns["NoOfBags"].FormatString = "#,#";
				grddetailhistory.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
				grddetailhistory.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
				grddetailhistory.RootTable.Columns["Amount"].FormatString = "#,#";
				grddetailhistory.RootTable.Columns["Amount"].TotalFormatString = "#,#";
				grddetailhistory.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
				grddetailhistory.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
				grddetailhistory.RootTable.Columns["GrossWeight"].FormatString = "#,#";
				grddetailhistory.RootTable.Columns["GrossWeight"].TotalFormatString = "#,#";
				grddetailhistory.RootTable.Columns["PackingWeight"].AggregateFunction = (AggregateFunction)2;
				grddetailhistory.RootTable.Columns["PackingWeight"].TextAlignment = (TextAlignment)3;
				grddetailhistory.RootTable.Columns["PackingWeight"].FormatString = "#,#.##";
				grddetailhistory.RootTable.Columns["PackingWeight"].TotalFormatString = "#,#.##";
				grddetailhistory.RootTable.Columns["TotalPackingWeight"].AggregateFunction = (AggregateFunction)2;
				grddetailhistory.RootTable.Columns["TotalPackingWeight"].TextAlignment = (TextAlignment)3;
				grddetailhistory.RootTable.Columns["TotalPackingWeight"].FormatString = "#,#.##";
				grddetailhistory.RootTable.Columns["TotalPackingWeight"].TotalFormatString = "#,#.##";
				grddetailhistory.RootTable.Columns["NoOfContainers"].AggregateFunction = (AggregateFunction)2;
				grddetailhistory.RootTable.Columns["NoOfContainers"].TextAlignment = (TextAlignment)3;
				grddetailhistory.RootTable.Columns["NoOfContainers"].FormatString = "#,#.##";
				grddetailhistory.RootTable.Columns["NoOfContainers"].TotalFormatString = "#,#.##";
				grddetailhistory.RootTable.Columns["M.Ton"].TotalFormatString = "#,#";
				grddetailhistory.RootTable.Columns["NoOfBags"].TotalFormatString = "#,#";
				grddetailhistory.RootTable.Columns["M.Ton"].TextAlignment = (TextAlignment)3;
				grddetailhistory.RootTable.Columns["NoOfBags"].TextAlignment = (TextAlignment)3;
			}
			else
			{
				grddetailhistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtNoOfContainers_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CommonServices.CommasApplyWhileTyping(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFCYAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CommonServices.CommasApplyWhileTyping(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtCommPercent_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtCommAmount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
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

	private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtCommAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CommonServices.CommasApplyWhileTyping(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtNoOfContainersDetail_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
			CalculatAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtNoOfBAgsPerCntnr_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
			CalculatAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtCommPercent_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToDouble((object)txtCommPercent.Text.Trim()) > 100.0)
			{
				MessageBox.Show("Commission % Can not Greater than 100...");
				txtCommPercent.Text = "100";
			}
			else
			{
				CalculateCommission();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateCommission()
	{
		try
		{
			double commRate = Conversion.ToDouble((object)txtCommPercent.Text.Trim());
			if (grdDetails.CurrentRow != null)
			{
				double total = Conversion.ToDouble(grdDetails.GetTotal(grdDetails.RootTable.Columns["Amount"], (AggregateFunction)2));
				if (!double.IsNaN(total))
				{
					double commAmt = Conversion.ToDouble((object)total.ToString()) * commRate / 100.0;
					txtCommAmount.Text = Math.Round(commAmt).ToString();
				}
				else
				{
					txtCommAmount.Text = "0";
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combpcktype_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)combpcktype).ActiveRow != null)
			{
				double PackingWeight = Conversion.ToDouble(((UltraDropDownBase)combpcktype).SelectedRow.Cells[2].Value);
				if (((UltraGridBase)combpcktype).ActiveRow != null && PackingWeight > 0.0)
				{
					txtPackingWeight.Text = PackingWeight.ToString();
				}
				else
				{
					txtPackingWeight.Text = 0.ToString();
				}
			}
			else
			{
				txtPackingWeight.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPackingWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
			CalculatAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ProformaInvoice_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_0603: Unknown result type (might be due to invalid IL or missing references)
		//IL_060a: Expected O, but got Unknown
		//IL_060a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0611: Expected O, but got Unknown
		//IL_0611: Unknown result type (might be due to invalid IL or missing references)
		//IL_0618: Expected O, but got Unknown
		//IL_0618: Unknown result type (might be due to invalid IL or missing references)
		//IL_061f: Expected O, but got Unknown
		//IL_061f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0626: Expected O, but got Unknown
		//IL_0626: Unknown result type (might be due to invalid IL or missing references)
		//IL_062d: Expected O, but got Unknown
		//IL_062d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0634: Expected O, but got Unknown
		//IL_0634: Unknown result type (might be due to invalid IL or missing references)
		//IL_063b: Expected O, but got Unknown
		//IL_063b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0642: Expected O, but got Unknown
		//IL_0642: Unknown result type (might be due to invalid IL or missing references)
		//IL_0649: Expected O, but got Unknown
		//IL_0649: Unknown result type (might be due to invalid IL or missing references)
		//IL_0650: Expected O, but got Unknown
		//IL_0650: Unknown result type (might be due to invalid IL or missing references)
		//IL_0657: Expected O, but got Unknown
		//IL_0657: Unknown result type (might be due to invalid IL or missing references)
		//IL_065e: Expected O, but got Unknown
		//IL_065e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0665: Expected O, but got Unknown
		//IL_06be: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c8: Expected O, but got Unknown
		//IL_0763: Unknown result type (might be due to invalid IL or missing references)
		//IL_076d: Expected O, but got Unknown
		//IL_07f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fc: Expected O, but got Unknown
		//IL_07fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0807: Expected O, but got Unknown
		//IL_0829: Unknown result type (might be due to invalid IL or missing references)
		//IL_0833: Expected O, but got Unknown
		//IL_083f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0849: Expected O, but got Unknown
		//IL_0881: Unknown result type (might be due to invalid IL or missing references)
		//IL_088b: Expected O, but got Unknown
		//IL_0926: Unknown result type (might be due to invalid IL or missing references)
		//IL_0930: Expected O, but got Unknown
		//IL_097e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0988: Expected O, but got Unknown
		//IL_0a23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2d: Expected O, but got Unknown
		//IL_0ad3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0add: Expected O, but got Unknown
		//IL_0b41: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4b: Expected O, but got Unknown
		//IL_0b4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b56: Expected O, but got Unknown
		//IL_0b57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b61: Expected O, but got Unknown
		//IL_0b62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6c: Expected O, but got Unknown
		//IL_0b6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b77: Expected O, but got Unknown
		//IL_0b78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b82: Expected O, but got Unknown
		//IL_0b99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba3: Expected O, but got Unknown
		//IL_0ba4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bae: Expected O, but got Unknown
		//IL_0baf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb9: Expected O, but got Unknown
		//IL_0c96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca0: Expected O, but got Unknown
		//IL_0cb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc1: Expected O, but got Unknown
		//IL_0d88: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d92: Expected O, but got Unknown
		//IL_1506: Unknown result type (might be due to invalid IL or missing references)
		//IL_1510: Expected O, but got Unknown
		//IL_ab4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_ab56: Expected O, but got Unknown
		//IL_ab7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_ab84: Expected O, but got Unknown
		GridEXLayout grdDetails_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		GridEXLayout grdotheritems_DesignTimeLayout = new GridEXLayout();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
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
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Export.ProformaInvoice));
		GridEXLayout grddetailhistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabContractDetail = new System.Windows.Forms.TabPage();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grdDetails = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.btnplus = new System.Windows.Forms.Button();
		this.txtGrossWeightdetail = new System.Windows.Forms.TextBox();
		this.label48 = new System.Windows.Forms.Label();
		this.txtPackingTotalWeight = new System.Windows.Forms.TextBox();
		this.label47 = new System.Windows.Forms.Label();
		this.txtPackingWeight = new System.Windows.Forms.TextBox();
		this.label46 = new System.Windows.Forms.Label();
		this.txtNoOfBAgsPerCntnr = new System.Windows.Forms.TextBox();
		this.label39 = new System.Windows.Forms.Label();
		this.txtexpirydatedetail = new System.Windows.Forms.TextBox();
		this.label41 = new System.Windows.Forms.Label();
		this.txtNoOfContainersDetail = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.combitem = new UltraCombo();
		this.txtPackingDetail = new System.Windows.Forms.TextBox();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.label52 = new System.Windows.Forms.Label();
		this.txtQtyMTon = new System.Windows.Forms.TextBox();
		this.label34 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.txtCostMTon = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.label25 = new System.Windows.Forms.Label();
		this.combpcktype = new UltraCombo();
		this.combrateuom = new UltraCombo();
		this.txtNoOfBags = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.combitempck = new UltraCombo();
		this.label23 = new System.Windows.Forms.Label();
		this.CmbCropYear = new UltraCombo();
		this.label33 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.tabOtherItems = new System.Windows.Forms.TabPage();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grdotheritems = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label44 = new System.Windows.Forms.Label();
		this.btnoCancel = new System.Windows.Forms.Button();
		this.btnoUpdate = new System.Windows.Forms.Button();
		this.btnoadd = new System.Windows.Forms.Button();
		this.label38 = new System.Windows.Forms.Label();
		this.txtoRemarks = new System.Windows.Forms.TextBox();
		this.txtoQty = new System.Windows.Forms.TextBox();
		this.label35 = new System.Windows.Forms.Label();
		this.txtoRate = new System.Windows.Forms.TextBox();
		this.label36 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.txtoAmount = new System.Windows.Forms.TextBox();
		this.CmbOtherItem = new UltraCombo();
		this.label31 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.chkPrint = new System.Windows.Forms.CheckBox();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtCommPercent = new System.Windows.Forms.TextBox();
		this.label49 = new System.Windows.Forms.Label();
		this.txtCommAmount = new System.Windows.Forms.TextBox();
		this.cmbCommAgent = new UltraCombo();
		this.label51 = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtInsuraneRemarks = new System.Windows.Forms.TextBox();
		this.txtCommodityDetail = new System.Windows.Forms.RichTextBox();
		this.txtcontainersizedetail = new System.Windows.Forms.TextBox();
		this.label40 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label53 = new System.Windows.Forms.Label();
		this.ChkOnCommission = new System.Windows.Forms.CheckBox();
		this.label19 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.cmbStatus = new UltraCombo();
		this.label15 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.txtProductSpecification = new System.Windows.Forms.RichTextBox();
		this.label32 = new System.Windows.Forms.Label();
		this.txtNoOfContainers = new System.Windows.Forms.TextBox();
		this.label29 = new System.Windows.Forms.Label();
		this.txtShipmentDate = new System.Windows.Forms.DateTimePicker();
		this.txtBuyerOrderNo = new System.Windows.Forms.TextBox();
		this.label27 = new System.Windows.Forms.Label();
		this.txtLotRefNo = new System.Windows.Forms.TextBox();
		this.label26 = new System.Windows.Forms.Label();
		this.txtExportSoNo = new System.Windows.Forms.TextBox();
		this.CmbFCYCode = new UltraCombo();
		this.label30 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.CmbExporterbank = new UltraCombo();
		this.CmbImporterBank = new UltraCombo();
		this.CmbDestinationPort = new UltraCombo();
		this.CmbLoadingPort = new UltraCombo();
		this.CmbPaymentTerms = new UltraCombo();
		this.CmbDeliveryTerm = new UltraCombo();
		this.datOrderDate = new System.Windows.Forms.DateTimePicker();
		this.txtDocDate = new System.Windows.Forms.DateTimePicker();
		this.CmbNotifyParty = new UltraCombo();
		this.CmbCustomer = new UltraCombo();
		this.CmbSalePerson = new UltraCombo();
		this.txtSpecialInstructions = new System.Windows.Forms.TextBox();
		this.txtMTon = new System.Windows.Forms.TextBox();
		this.txtFCYAmount = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolstrip = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnSaveAs = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
		this.btnDefinePaymentTerm = new System.Windows.Forms.ToolStripButton();
		this.btnPackingTypeLink = new System.Windows.Forms.ToolStripButton();
		this.btnSeaPortDefine = new System.Windows.Forms.ToolStripButton();
		this.btnPackingMaterialRequirement = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel13 = new System.Windows.Forms.Panel();
		this.panel18 = new System.Windows.Forms.Panel();
		this.grddetailhistory = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label42 = new System.Windows.Forms.Label();
		this.DataGridHistory = new GridEX();
		this.panel17 = new System.Windows.Forms.Panel();
		this.label43 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.label54 = new System.Windows.Forms.Label();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.label67 = new System.Windows.Forms.Label();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.label73 = new System.Windows.Forms.Label();
		this.label89 = new System.Windows.Forms.Label();
		this.label90 = new System.Windows.Forms.Label();
		this.label91 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel5.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabContractDetail.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetails).BeginInit();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.combitem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combpcktype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combrateuom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combitempck).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).BeginInit();
		this.tabOtherItems.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdotheritems).BeginInit();
		this.panel8.SuspendLayout();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOtherItem).BeginInit();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCommAgent).BeginInit();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFCYCode).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbExporterbank).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbImporterBank).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDestinationPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLoadingPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTerms).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbNotifyParty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSalePerson).BeginInit();
		this.panel3.SuspendLayout();
		this.toolstrip.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel13.SuspendLayout();
		this.panel18.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grddetailhistory).BeginInit();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel17.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.groupBox5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1064, 713);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1064, 713);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1056, 687);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel5);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1050, 681);
		this.panel2.TabIndex = 0;
		this.panel5.Controls.Add(this.tabControl2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 351);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1050, 330);
		this.panel5.TabIndex = 2;
		this.tabControl2.Controls.Add(this.tabContractDetail);
		this.tabControl2.Controls.Add(this.tabOtherItems);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1050, 330);
		this.tabControl2.TabIndex = 0;
		this.tabContractDetail.Controls.Add(this.panel7);
		this.tabContractDetail.Controls.Add(this.panel6);
		this.tabContractDetail.Location = new System.Drawing.Point(4, 22);
		this.tabContractDetail.Name = "tabContractDetail";
		this.tabContractDetail.Padding = new System.Windows.Forms.Padding(3);
		this.tabContractDetail.Size = new System.Drawing.Size(1042, 304);
		this.tabContractDetail.TabIndex = 1;
		this.tabContractDetail.Text = "Contract Detail";
		this.tabContractDetail.UseVisualStyleBackColor = true;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetails);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(3, 151);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1036, 150);
		this.panel7.TabIndex = 491;
		this.grdDetails.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetails).BackColor = System.Drawing.Color.White;
		grdDetails_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetails.DesignTimeLayout = grdDetails_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetails).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdDetails).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetails.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetails).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetails).Name = "grdDetails";
		((ScrollableBase)this.grdDetails).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetails).Size = new System.Drawing.Size(1036, 150);
		((System.Windows.Forms.Control)(object)this.grdDetails).TabIndex = 0;
		this.grdDetails.TotalRow = (InheritableBoolean)1;
		this.grdDetails.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetails).VisualStyle = (VisualStyle)4;
		this.grdDetails.ColumnButtonClick += new ColumnActionEventHandler(grdDetails_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetails).DoubleClick += new System.EventHandler(grdDetails_DoubleClick);
		this.panel6.Controls.Add(this.btnplus);
		this.panel6.Controls.Add(this.txtGrossWeightdetail);
		this.panel6.Controls.Add(this.label48);
		this.panel6.Controls.Add(this.txtPackingTotalWeight);
		this.panel6.Controls.Add(this.label47);
		this.panel6.Controls.Add(this.txtPackingWeight);
		this.panel6.Controls.Add(this.label46);
		this.panel6.Controls.Add(this.txtNoOfBAgsPerCntnr);
		this.panel6.Controls.Add(this.label39);
		this.panel6.Controls.Add(this.txtexpirydatedetail);
		this.panel6.Controls.Add(this.label41);
		this.panel6.Controls.Add(this.txtNoOfContainersDetail);
		this.panel6.Controls.Add(this.label11);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.combitem);
		this.panel6.Controls.Add(this.txtPackingDetail);
		this.panel6.Controls.Add(this.txtRemarks);
		this.panel6.Controls.Add(this.btnCancelUpdateDetial);
		this.panel6.Controls.Add(this.label52);
		this.panel6.Controls.Add(this.txtQtyMTon);
		this.panel6.Controls.Add(this.label34);
		this.panel6.Controls.Add(this.label16);
		this.panel6.Controls.Add(this.txtCostMTon);
		this.panel6.Controls.Add(this.label10);
		this.panel6.Controls.Add(this.label1);
		this.panel6.Controls.Add(this.btnUpdateDetail);
		this.panel6.Controls.Add(this.label25);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.combpcktype);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.combrateuom);
		this.panel6.Controls.Add(this.txtNoOfBags);
		this.panel6.Controls.Add(this.label24);
		this.panel6.Controls.Add(this.label13);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.combitempck);
		this.panel6.Controls.Add(this.label23);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYear);
		this.panel6.Controls.Add(this.label33);
		this.panel6.Controls.Add(this.label2);
		this.panel6.Controls.Add(this.txtAmount);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1036, 148);
		this.panel6.TabIndex = 0;
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(910, 117);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(28, 26);
		this.btnplus.TabIndex = 17;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.txtGrossWeightdetail.Enabled = false;
		this.txtGrossWeightdetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrossWeightdetail.Location = new System.Drawing.Point(109, 75);
		this.txtGrossWeightdetail.Name = "txtGrossWeightdetail";
		this.txtGrossWeightdetail.Size = new System.Drawing.Size(91, 23);
		this.txtGrossWeightdetail.TabIndex = 10;
		this.txtGrossWeightdetail.Text = "0";
		this.txtGrossWeightdetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(109, 56);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(78, 15);
		this.label48.TabIndex = 570;
		this.label48.Text = "Gross Weight";
		this.txtPackingTotalWeight.Enabled = false;
		this.txtPackingTotalWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackingTotalWeight.Location = new System.Drawing.Point(8, 75);
		this.txtPackingTotalWeight.Name = "txtPackingTotalWeight";
		this.txtPackingTotalWeight.Size = new System.Drawing.Size(98, 23);
		this.txtPackingTotalWeight.TabIndex = 9;
		this.txtPackingTotalWeight.Text = "0";
		this.txtPackingTotalWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(8, 56);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(97, 15);
		this.label47.TabIndex = 568;
		this.label47.Text = "Packing Total Wt";
		this.txtPackingWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackingWeight.Location = new System.Drawing.Point(359, 27);
		this.txtPackingWeight.Name = "txtPackingWeight";
		this.txtPackingWeight.Size = new System.Drawing.Size(101, 23);
		this.txtPackingWeight.TabIndex = 2;
		this.txtPackingWeight.Text = "0";
		this.txtPackingWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPackingWeight.TextChanged += new System.EventHandler(txtPackingWeight_TextChanged);
		this.txtPackingWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(textBox3_KeyPress);
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(359, 5);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(68, 15);
		this.label46.TabIndex = 566;
		this.label46.Text = "Packing Wt";
		this.txtNoOfBAgsPerCntnr.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNoOfBAgsPerCntnr.Location = new System.Drawing.Point(683, 27);
		this.txtNoOfBAgsPerCntnr.Name = "txtNoOfBAgsPerCntnr";
		this.txtNoOfBAgsPerCntnr.Size = new System.Drawing.Size(103, 23);
		this.txtNoOfBAgsPerCntnr.TabIndex = 5;
		this.txtNoOfBAgsPerCntnr.Text = "0";
		this.txtNoOfBAgsPerCntnr.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNoOfBAgsPerCntnr.TextChanged += new System.EventHandler(txtNoOfBAgsPerCntnr_TextChanged);
		this.txtNoOfBAgsPerCntnr.KeyPress += new System.Windows.Forms.KeyPressEventHandler(textBox2_KeyPress);
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(685, 5);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(101, 15);
		this.label39.TabIndex = 497;
		this.label39.Text = "No Of Bags/Cntnr";
		this.txtexpirydatedetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtexpirydatedetail.Location = new System.Drawing.Point(462, 75);
		this.txtexpirydatedetail.Name = "txtexpirydatedetail";
		this.txtexpirydatedetail.Size = new System.Drawing.Size(108, 23);
		this.txtexpirydatedetail.TabIndex = 14;
		this.txtexpirydatedetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(462, 56);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(112, 15);
		this.label41.TabIndex = 495;
		this.label41.Text = "Packing Expiry Date";
		this.txtNoOfContainersDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNoOfContainersDetail.Location = new System.Drawing.Point(572, 27);
		this.txtNoOfContainersDetail.Name = "txtNoOfContainersDetail";
		this.txtNoOfContainersDetail.Size = new System.Drawing.Size(109, 23);
		this.txtNoOfContainersDetail.TabIndex = 4;
		this.txtNoOfContainersDetail.Text = "0";
		this.txtNoOfContainersDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNoOfContainersDetail.TextChanged += new System.EventHandler(txtNoOfContainersDetail_TextChanged);
		this.txtNoOfContainersDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNoOfContainersDetail_KeyPress);
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(568, 5);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(101, 15);
		this.label11.TabIndex = 491;
		this.label11.Text = "No. Of Containers";
		this.combitem.AllowNull = (DefaultableBoolean)1;
		this.combitem.AutoCompleteMode = (AutoCompleteMode)4;
		this.combitem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combitem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combitem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.combitem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combitem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combitem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.combitem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.combitem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combitem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combitem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combitem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.combitem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combitem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance114).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).TextHAlignAsString = "Left";
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combitem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.combitem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combitem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.combitem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combitem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combitem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combitem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combitem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combitem).Location = new System.Drawing.Point(8, 25);
		((System.Windows.Forms.Control)(object)this.combitem).Name = "combitem";
		((System.Windows.Forms.Control)(object)this.combitem).Size = new System.Drawing.Size(192, 26);
		((System.Windows.Forms.Control)(object)this.combitem).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.combitem).Leave += new System.EventHandler(combitem_Leave_1);
		this.txtPackingDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackingDetail.Location = new System.Drawing.Point(8, 119);
		this.txtPackingDetail.Name = "txtPackingDetail";
		this.txtPackingDetail.Size = new System.Drawing.Size(899, 23);
		this.txtPackingDetail.TabIndex = 16;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(572, 75);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(460, 23);
		this.txtRemarks.TabIndex = 15;
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(975, 117);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(58, 26);
		this.btnCancelUpdateDetial.TabIndex = 18;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(8, 100);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(83, 15);
		this.label52.TabIndex = 489;
		this.label52.Text = "Packing Detail";
		this.txtQtyMTon.Enabled = false;
		this.txtQtyMTon.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQtyMTon.Location = new System.Drawing.Point(867, 27);
		this.txtQtyMTon.Name = "txtQtyMTon";
		this.txtQtyMTon.Size = new System.Drawing.Size(82, 23);
		this.txtQtyMTon.TabIndex = 7;
		this.txtQtyMTon.Tag = "Mton";
		this.txtQtyMTon.Text = "0";
		this.txtQtyMTon.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQtyMTon.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQtyMTon_KeyPress);
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(576, 56);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(52, 15);
		this.label34.TabIndex = 489;
		this.label34.Text = "Remarks";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(867, 5);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(41, 15);
		this.label16.TabIndex = 483;
		this.label16.Text = "M.Ton";
		this.txtCostMTon.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCostMTon.Location = new System.Drawing.Point(204, 75);
		this.txtCostMTon.Name = "txtCostMTon";
		this.txtCostMTon.Size = new System.Drawing.Size(73, 23);
		this.txtCostMTon.TabIndex = 11;
		this.txtCostMTon.Text = "0";
		this.txtCostMTon.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCostMTon.TextChanged += new System.EventHandler(txtCostMTon_TextChanged);
		this.txtCostMTon.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCostMTon_KeyPress);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(204, 5);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(77, 15);
		this.label10.TabIndex = 476;
		this.label10.Text = "Packing Type";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(8, 5);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(67, 15);
		this.label1.TabIndex = 470;
		this.label1.Text = "Item Name";
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(910, 117);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(62, 26);
		this.btnUpdateDetail.TabIndex = 17;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(204, 56);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(30, 15);
		this.label25.TabIndex = 487;
		this.label25.Text = "Rate";
		this.combpcktype.AllowNull = (DefaultableBoolean)1;
		this.combpcktype.AutoCompleteMode = (AutoCompleteMode)4;
		this.combpcktype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combpcktype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combpcktype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.combpcktype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combpcktype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combpcktype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpcktype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.combpcktype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpcktype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.combpcktype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combpcktype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combpcktype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.combpcktype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combpcktype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combpcktype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combpcktype).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combpcktype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combpcktype).Location = new System.Drawing.Point(204, 25);
		((System.Windows.Forms.Control)(object)this.combpcktype).Name = "combpcktype";
		((System.Windows.Forms.Control)(object)this.combpcktype).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.combpcktype).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.combpcktype).Leave += new System.EventHandler(combpcktype_Leave);
		this.combrateuom.AllowNull = (DefaultableBoolean)1;
		this.combrateuom.AutoCompleteMode = (AutoCompleteMode)4;
		this.combrateuom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combrateuom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combrateuom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.combrateuom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combrateuom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combrateuom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combrateuom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance121;
		((SpecialBoxBase)((UltraGridBase)this.combrateuom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combrateuom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.combrateuom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combrateuom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance127).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).TextHAlignAsString = "Left";
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.combrateuom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combrateuom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combrateuom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combrateuom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combrateuom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combrateuom).Location = new System.Drawing.Point(279, 73);
		((System.Windows.Forms.Control)(object)this.combrateuom).Name = "combrateuom";
		((System.Windows.Forms.Control)(object)this.combrateuom).Size = new System.Drawing.Size(78, 26);
		((System.Windows.Forms.Control)(object)this.combrateuom).TabIndex = 12;
		((System.Windows.Forms.Control)(object)this.combrateuom).TextChanged += new System.EventHandler(combrateuom_TextChanged);
		this.txtNoOfBags.Enabled = false;
		this.txtNoOfBags.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNoOfBags.Location = new System.Drawing.Point(952, 27);
		this.txtNoOfBags.Name = "txtNoOfBags";
		this.txtNoOfBags.Size = new System.Drawing.Size(80, 23);
		this.txtNoOfBags.TabIndex = 8;
		this.txtNoOfBags.Tag = "NoOfBags";
		this.txtNoOfBags.Text = "0";
		this.txtNoOfBags.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNoOfBags.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNoOfBags_KeyPress);
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(464, 5);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(54, 15);
		this.label24.TabIndex = 476;
		this.label24.Text = "CropYear";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(279, 56);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(61, 15);
		this.label13.TabIndex = 479;
		this.label13.Text = "Rate UOM";
		this.combitempck.AllowNull = (DefaultableBoolean)1;
		this.combitempck.AutoCompleteMode = (AutoCompleteMode)4;
		this.combitempck.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combitempck.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combitempck).DisplayLayout.Appearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.combitempck).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combitempck).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance148).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance148).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combitempck).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance148;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitempck).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance149;
		((SpecialBoxBase)((UltraGridBase)this.combitempck).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance150).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance150).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitempck).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.combitempck).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combitempck).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance155).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance156).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance156).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance156).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance157).TextHAlignAsString = "Left";
		((UltraGridBase)this.combitempck).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.combitempck).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combitempck).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combitempck).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combitempck).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combitempck.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combitempck).Location = new System.Drawing.Point(789, 25);
		((System.Windows.Forms.Control)(object)this.combitempck).Name = "combitempck";
		((System.Windows.Forms.Control)(object)this.combitempck).Size = new System.Drawing.Size(75, 26);
		((System.Windows.Forms.Control)(object)this.combitempck).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.combitempck).TextChanged += new System.EventHandler(combitempck_TextChanged);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(951, 5);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(67, 15);
		this.label23.TabIndex = 485;
		this.label23.Text = "No Of Bags";
		this.CmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance174).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance174).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance174;
		((AppearanceBase)appearance185).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance185;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance196).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance196).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance196).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance207).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance207;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance218).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance218;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance132).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Location = new System.Drawing.Point(464, 25);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Name = "CmbCropYear";
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Size = new System.Drawing.Size(106, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).TabIndex = 3;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(359, 56);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(51, 15);
		this.label33.TabIndex = 485;
		this.label33.Text = "Amount";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(789, 5);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(63, 15);
		this.label2.TabIndex = 474;
		this.label2.Text = "Pack UOM";
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmount.Location = new System.Drawing.Point(359, 75);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.ReadOnly = true;
		this.txtAmount.Size = new System.Drawing.Size(101, 23);
		this.txtAmount.TabIndex = 13;
		this.txtAmount.Text = "0";
		this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAmount.TextChanged += new System.EventHandler(txtAmount_TextChanged);
		this.tabOtherItems.Controls.Add(this.panel9);
		this.tabOtherItems.Controls.Add(this.panel8);
		this.tabOtherItems.Location = new System.Drawing.Point(4, 22);
		this.tabOtherItems.Name = "tabOtherItems";
		this.tabOtherItems.Padding = new System.Windows.Forms.Padding(3);
		this.tabOtherItems.Size = new System.Drawing.Size(1042, 304);
		this.tabOtherItems.TabIndex = 0;
		this.tabOtherItems.Text = "Other Items";
		this.tabOtherItems.UseVisualStyleBackColor = true;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdotheritems);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(3, 77);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1036, 224);
		this.panel9.TabIndex = 2;
		this.grdotheritems.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdotheritems).BackColor = System.Drawing.Color.White;
		grdotheritems_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdotheritems.DesignTimeLayout = grdotheritems_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdotheritems.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdotheritems).Name = "grdotheritems";
		((ScrollableBase)this.grdotheritems).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Size = new System.Drawing.Size(1036, 224);
		((System.Windows.Forms.Control)(object)this.grdotheritems).TabIndex = 0;
		this.grdotheritems.TotalRow = (InheritableBoolean)1;
		this.grdotheritems.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdotheritems).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdotheritems).DoubleClick += new System.EventHandler(grdotheritems_DoubleClick);
		this.panel8.Controls.Add(this.panel12);
		this.panel8.Controls.Add(this.btnoCancel);
		this.panel8.Controls.Add(this.btnoUpdate);
		this.panel8.Controls.Add(this.btnoadd);
		this.panel8.Controls.Add(this.label38);
		this.panel8.Controls.Add(this.txtoRemarks);
		this.panel8.Controls.Add(this.txtoQty);
		this.panel8.Controls.Add(this.label35);
		this.panel8.Controls.Add(this.txtoRate);
		this.panel8.Controls.Add(this.label36);
		this.panel8.Controls.Add(this.label37);
		this.panel8.Controls.Add(this.txtoAmount);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOtherItem);
		this.panel8.Controls.Add(this.label31);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1036, 74);
		this.panel8.TabIndex = 1;
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label44);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.ForeColor = System.Drawing.Color.White;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1036, 23);
		this.panel12.TabIndex = 561;
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label44.Location = new System.Drawing.Point(11, 3);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(117, 17);
		this.label44.TabIndex = 561;
		this.label44.Text = "Other Expenses";
		this.btnoCancel.BackColor = System.Drawing.Color.Teal;
		this.btnoCancel.FlatAppearance.BorderSize = 0;
		this.btnoCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnoCancel.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnoCancel.ForeColor = System.Drawing.Color.White;
		this.btnoCancel.Location = new System.Drawing.Point(731, 46);
		this.btnoCancel.Name = "btnoCancel";
		this.btnoCancel.Size = new System.Drawing.Size(67, 26);
		this.btnoCancel.TabIndex = 7;
		this.btnoCancel.Text = "Cancel";
		this.btnoCancel.UseVisualStyleBackColor = false;
		this.btnoCancel.Visible = false;
		this.btnoCancel.Click += new System.EventHandler(btnoCancel_Click);
		this.btnoUpdate.BackColor = System.Drawing.Color.Teal;
		this.btnoUpdate.FlatAppearance.BorderSize = 0;
		this.btnoUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnoUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnoUpdate.ForeColor = System.Drawing.Color.White;
		this.btnoUpdate.Location = new System.Drawing.Point(662, 46);
		this.btnoUpdate.Name = "btnoUpdate";
		this.btnoUpdate.Size = new System.Drawing.Size(68, 26);
		this.btnoUpdate.TabIndex = 6;
		this.btnoUpdate.Text = "Update";
		this.btnoUpdate.UseVisualStyleBackColor = false;
		this.btnoUpdate.Visible = false;
		this.btnoUpdate.Click += new System.EventHandler(btnoUpdate_Click);
		this.btnoadd.BackColor = System.Drawing.Color.Teal;
		this.btnoadd.FlatAppearance.BorderSize = 0;
		this.btnoadd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnoadd.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnoadd.ForeColor = System.Drawing.Color.White;
		this.btnoadd.Location = new System.Drawing.Point(661, 46);
		this.btnoadd.Name = "btnoadd";
		this.btnoadd.Size = new System.Drawing.Size(27, 26);
		this.btnoadd.TabIndex = 5;
		this.btnoadd.Text = "+";
		this.btnoadd.UseVisualStyleBackColor = false;
		this.btnoadd.Click += new System.EventHandler(btnoadd_Click);
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(442, 29);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(52, 15);
		this.label38.TabIndex = 495;
		this.label38.Text = "Remarks";
		this.txtoRemarks.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtoRemarks.Location = new System.Drawing.Point(445, 48);
		this.txtoRemarks.Name = "txtoRemarks";
		this.txtoRemarks.Size = new System.Drawing.Size(211, 23);
		this.txtoRemarks.TabIndex = 4;
		this.txtoQty.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtoQty.Location = new System.Drawing.Point(180, 48);
		this.txtoQty.Name = "txtoQty";
		this.txtoQty.Size = new System.Drawing.Size(76, 23);
		this.txtoQty.TabIndex = 1;
		this.txtoQty.Text = "0";
		this.txtoQty.TextChanged += new System.EventHandler(txtoQty_TextChanged);
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(187, 29);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(26, 15);
		this.label35.TabIndex = 491;
		this.label35.Text = "Qty";
		this.txtoRate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtoRate.Location = new System.Drawing.Point(257, 48);
		this.txtoRate.Name = "txtoRate";
		this.txtoRate.Size = new System.Drawing.Size(80, 23);
		this.txtoRate.TabIndex = 2;
		this.txtoRate.Text = "0";
		this.txtoRate.TextChanged += new System.EventHandler(txtoRate_TextChanged);
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(254, 29);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(30, 15);
		this.label36.TabIndex = 493;
		this.label36.Text = "Rate";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(338, 29);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(51, 15);
		this.label37.TabIndex = 492;
		this.label37.Text = "Amount";
		this.txtoAmount.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtoAmount.Location = new System.Drawing.Point(341, 48);
		this.txtoAmount.Name = "txtoAmount";
		this.txtoAmount.ReadOnly = true;
		this.txtoAmount.Size = new System.Drawing.Size(101, 23);
		this.txtoAmount.TabIndex = 3;
		this.txtoAmount.Text = "0";
		this.CmbOtherItem.AllowNull = (DefaultableBoolean)1;
		this.CmbOtherItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOtherItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOtherItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance161).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance161).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOtherItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance162;
		((SpecialBoxBase)((UltraGridBase)this.CmbOtherItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance164).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance164).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance165).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance166).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance168).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance169).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance169).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance169).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance169;
		((AppearanceBase)appearance170).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance171).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbOtherItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).Location = new System.Drawing.Point(7, 46);
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).Name = "CmbOtherItem";
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).TabIndex = 0;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(4, 29);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(67, 15);
		this.label31.TabIndex = 472;
		this.label31.Text = "Item Name";
		this.panel4.Controls.Add(this.chkPrint);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add(this.label45);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.txtInsuraneRemarks);
		this.panel4.Controls.Add(this.txtCommodityDetail);
		this.panel4.Controls.Add(this.txtcontainersizedetail);
		this.panel4.Controls.Add(this.label40);
		this.panel4.Controls.Add(this.panel10);
		this.panel4.Controls.Add(this.ChkOnCommission);
		this.panel4.Controls.Add(this.label19);
		this.panel4.Controls.Add(this.label18);
		this.panel4.Controls.Add(this.label17);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbStatus);
		this.panel4.Controls.Add(this.label15);
		this.panel4.Controls.Add(this.label14);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Controls.Add(this.label12);
		this.panel4.Controls.Add(this.txtProductSpecification);
		this.panel4.Controls.Add(this.label32);
		this.panel4.Controls.Add(this.txtNoOfContainers);
		this.panel4.Controls.Add(this.label29);
		this.panel4.Controls.Add(this.txtShipmentDate);
		this.panel4.Controls.Add(this.txtBuyerOrderNo);
		this.panel4.Controls.Add(this.label27);
		this.panel4.Controls.Add(this.txtLotRefNo);
		this.panel4.Controls.Add(this.label26);
		this.panel4.Controls.Add(this.txtExportSoNo);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFCYCode);
		this.panel4.Controls.Add(this.label30);
		this.panel4.Controls.Add(this.label22);
		this.panel4.Controls.Add(this.label20);
		this.panel4.Controls.Add(this.label9);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add(this.label21);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add(this.label28);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbExporterbank);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbImporterBank);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDestinationPort);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLoadingPort);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentTerms);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm);
		this.panel4.Controls.Add(this.datOrderDate);
		this.panel4.Controls.Add(this.txtDocDate);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbNotifyParty);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomer);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSalePerson);
		this.panel4.Controls.Add(this.txtSpecialInstructions);
		this.panel4.Controls.Add(this.txtMTon);
		this.panel4.Controls.Add(this.txtFCYAmount);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 30);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1050, 321);
		this.panel4.TabIndex = 0;
		this.chkPrint.AutoSize = true;
		this.chkPrint.Checked = true;
		this.chkPrint.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkPrint.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPrint.Location = new System.Drawing.Point(644, 298);
		this.chkPrint.Name = "chkPrint";
		this.chkPrint.Size = new System.Drawing.Size(67, 19);
		this.chkPrint.TabIndex = 565;
		this.chkPrint.Text = "Preview";
		this.chkPrint.UseVisualStyleBackColor = true;
		this.groupBox1.Controls.Add(this.txtCommPercent);
		this.groupBox1.Controls.Add(this.label49);
		this.groupBox1.Controls.Add(this.txtCommAmount);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCommAgent);
		this.groupBox1.Controls.Add(this.label51);
		this.groupBox1.Controls.Add(this.label50);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(641, 35);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(406, 82);
		this.groupBox1.TabIndex = 15;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Commission";
		this.txtCommPercent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommPercent.Location = new System.Drawing.Point(83, 48);
		this.txtCommPercent.Name = "txtCommPercent";
		this.txtCommPercent.Size = new System.Drawing.Size(112, 23);
		this.txtCommPercent.TabIndex = 1;
		this.txtCommPercent.Text = "0";
		this.txtCommPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCommPercent.TextChanged += new System.EventHandler(txtCommPercent_TextChanged);
		this.txtCommPercent.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCommPercent_KeyPress);
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(0, 52);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(86, 15);
		this.label49.TabIndex = 569;
		this.label49.Text = "Commission %";
		this.txtCommAmount.Enabled = false;
		this.txtCommAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommAmount.Location = new System.Drawing.Point(298, 48);
		this.txtCommAmount.Name = "txtCommAmount";
		this.txtCommAmount.Size = new System.Drawing.Size(104, 23);
		this.txtCommAmount.TabIndex = 2;
		this.txtCommAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCommAmount.TextChanged += new System.EventHandler(txtCommAmount_TextChanged);
		this.txtCommAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCommAmount_KeyPress);
		this.cmbCommAgent.AllowNull = (DefaultableBoolean)1;
		this.cmbCommAgent.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCommAgent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCommAgent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance188).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance188).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCommAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance188;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance189;
		((SpecialBoxBase)((UltraGridBase)this.cmbCommAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance145).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance145).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance145).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance138).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance139).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance139).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance139).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance197).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCommAgent).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCommAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCommAgent).Location = new System.Drawing.Point(83, 16);
		((System.Windows.Forms.Control)(object)this.cmbCommAgent).Name = "cmbCommAgent";
		((System.Windows.Forms.Control)(object)this.cmbCommAgent).Size = new System.Drawing.Size(319, 26);
		((System.Windows.Forms.Control)(object)this.cmbCommAgent).TabIndex = 0;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(201, 52);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(90, 15);
		this.label51.TabIndex = 570;
		this.label51.Text = "Comm Amount";
		this.label50.AutoSize = true;
		this.label50.BackColor = System.Drawing.Color.White;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(0, 22);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(78, 15);
		this.label50.TabIndex = 571;
		this.label50.Text = "Comm Agent";
		this.label50.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(343, 158);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(85, 15);
		this.label45.TabIndex = 564;
		this.label45.Text = "Proforma Date";
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(640, 242);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(72, 34);
		this.label3.TabIndex = 563;
		this.label3.Text = "Insurance Remarks";
		this.txtInsuraneRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInsuraneRemarks.Location = new System.Drawing.Point(724, 240);
		this.txtInsuraneRemarks.Multiline = true;
		this.txtInsuraneRemarks.Name = "txtInsuraneRemarks";
		this.txtInsuraneRemarks.Size = new System.Drawing.Size(319, 36);
		this.txtInsuraneRemarks.TabIndex = 20;
		this.txtCommodityDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommodityDetail.Location = new System.Drawing.Point(117, 238);
		this.txtCommodityDetail.Name = "txtCommodityDetail";
		this.txtCommodityDetail.Size = new System.Drawing.Size(518, 38);
		this.txtCommodityDetail.TabIndex = 8;
		this.txtCommodityDetail.Text = "";
		this.txtcontainersizedetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcontainersizedetail.Location = new System.Drawing.Point(939, 154);
		this.txtcontainersizedetail.Name = "txtcontainersizedetail";
		this.txtcontainersizedetail.Size = new System.Drawing.Size(104, 23);
		this.txtcontainersizedetail.TabIndex = 22;
		this.txtcontainersizedetail.Text = "0";
		this.txtcontainersizedetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(841, 158);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(83, 15);
		this.label40.TabIndex = 493;
		this.label40.Text = "Container Size";
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.label53);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.ForeColor = System.Drawing.Color.White;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1050, 30);
		this.panel10.TabIndex = 560;
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(11, 5);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(149, 19);
		this.label53.TabIndex = 561;
		this.label53.Text = "Proforma Invoice";
		this.ChkOnCommission.AutoSize = true;
		this.ChkOnCommission.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkOnCommission.Location = new System.Drawing.Point(1030, 441);
		this.ChkOnCommission.Name = "ChkOnCommission";
		this.ChkOnCommission.Size = new System.Drawing.Size(111, 19);
		this.ChkOnCommission.TabIndex = 24;
		this.ChkOnCommission.Text = "On Commission";
		this.ChkOnCommission.UseVisualStyleBackColor = true;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(343, 42);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(80, 15);
		this.label19.TabIndex = 506;
		this.label19.Text = "Exporter Bank";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(6, 158);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(83, 15);
		this.label18.TabIndex = 505;
		this.label18.Text = "Importer Bank";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(343, 100);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(93, 15);
		this.label17.TabIndex = 504;
		this.label17.Text = "Destination Port";
		this.cmbStatus.AllowNull = (DefaultableBoolean)1;
		this.cmbStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance152).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance152).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance163;
		((SpecialBoxBase)((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance79).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance90).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance90).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance90).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance101).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbStatus).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbStatus).Location = new System.Drawing.Point(939, 209);
		((System.Windows.Forms.Control)(object)this.cmbStatus).Name = "cmbStatus";
		((System.Windows.Forms.Control)(object)this.cmbStatus).Size = new System.Drawing.Size(104, 26);
		((System.Windows.Forms.Control)(object)this.cmbStatus).TabIndex = 24;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.White;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(841, 215);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(40, 15);
		this.label15.TabIndex = 503;
		this.label15.Text = "Status";
		this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(343, 71);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(75, 15);
		this.label14.TabIndex = 501;
		this.label14.Text = "Loading Port";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(640, 158);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(84, 15);
		this.label7.TabIndex = 500;
		this.label7.Text = "Payment Term";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(640, 129);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(80, 15);
		this.label5.TabIndex = 499;
		this.label5.Text = "Delivery Term";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(6, 291);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(76, 15);
		this.label12.TabIndex = 497;
		this.label12.Text = "Specification";
		this.txtProductSpecification.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtProductSpecification.Location = new System.Drawing.Point(117, 279);
		this.txtProductSpecification.Name = "txtProductSpecification";
		this.txtProductSpecification.Size = new System.Drawing.Size(518, 38);
		this.txtProductSpecification.TabIndex = 9;
		this.txtProductSpecification.Text = "";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(841, 187);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(98, 15);
		this.label32.TabIndex = 495;
		this.label32.Text = "No Of Containers";
		this.txtNoOfContainers.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNoOfContainers.Location = new System.Drawing.Point(939, 183);
		this.txtNoOfContainers.Name = "txtNoOfContainers";
		this.txtNoOfContainers.Size = new System.Drawing.Size(104, 23);
		this.txtNoOfContainers.TabIndex = 23;
		this.txtNoOfContainers.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNoOfContainers.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNoOfContainers_KeyPress);
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(1027, 386);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(87, 15);
		this.label29.TabIndex = 493;
		this.label29.Text = "Shipment Date";
		this.label29.Visible = false;
		this.txtShipmentDate.CustomFormat = "dd-MMM-yyyy";
		this.txtShipmentDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtShipmentDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtShipmentDate.Location = new System.Drawing.Point(1030, 407);
		this.txtShipmentDate.Name = "txtShipmentDate";
		this.txtShipmentDate.Size = new System.Drawing.Size(126, 23);
		this.txtShipmentDate.TabIndex = 11;
		this.txtShipmentDate.Visible = false;
		this.txtBuyerOrderNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBuyerOrderNo.Location = new System.Drawing.Point(436, 125);
		this.txtBuyerOrderNo.Name = "txtBuyerOrderNo";
		this.txtBuyerOrderNo.Size = new System.Drawing.Size(199, 23);
		this.txtBuyerOrderNo.TabIndex = 13;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(343, 129);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(87, 15);
		this.label27.TabIndex = 491;
		this.label27.Text = "Proforma Ref #";
		this.txtLotRefNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLotRefNo.Location = new System.Drawing.Point(117, 183);
		this.txtLotRefNo.Name = "txtLotRefNo";
		this.txtLotRefNo.Size = new System.Drawing.Size(518, 23);
		this.txtLotRefNo.TabIndex = 6;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(6, 187);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(66, 15);
		this.label26.TabIndex = 489;
		this.label26.Text = "Lot Ref No.";
		this.txtExportSoNo.Enabled = false;
		this.txtExportSoNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExportSoNo.Location = new System.Drawing.Point(117, 38);
		this.txtExportSoNo.Name = "txtExportSoNo";
		this.txtExportSoNo.Size = new System.Drawing.Size(84, 23);
		this.txtExportSoNo.TabIndex = 0;
		this.CmbFCYCode.AllowNull = (DefaultableBoolean)1;
		this.CmbFCYCode.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFCYCode.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFCYCode.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Appearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance175).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFCYCode).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance175;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance176;
		((SpecialBoxBase)((UltraGridBase)this.CmbFCYCode).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance177).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance177).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance178).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance179).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance141).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance142).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance142).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance142).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance183).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFCYCode).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFCYCode).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbFCYCode.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbFCYCode).Location = new System.Drawing.Point(724, 181);
		((System.Windows.Forms.Control)(object)this.CmbFCYCode).Name = "CmbFCYCode";
		((System.Windows.Forms.Control)(object)this.CmbFCYCode).Size = new System.Drawing.Size(112, 26);
		((System.Windows.Forms.Control)(object)this.CmbFCYCode).TabIndex = 18;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(6, 215);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(70, 15);
		this.label30.TabIndex = 456;
		this.label30.Text = "Instructions";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(841, 129);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(41, 15);
		this.label22.TabIndex = 457;
		this.label22.Text = "M.Ton";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(640, 187);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(55, 15);
		this.label20.TabIndex = 458;
		this.label20.Text = "Fcy Code";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(6, 129);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(70, 15);
		this.label9.TabIndex = 466;
		this.label9.Text = "Notify Party";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(6, 100);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(110, 15);
		this.label6.TabIndex = 459;
		this.label6.Text = "Customer/Importer";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(6, 250);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(104, 15);
		this.label8.TabIndex = 460;
		this.label8.Text = "Commodity Detail";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(640, 215);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(72, 15);
		this.label21.TabIndex = 461;
		this.label21.Text = "Fcy Amount";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(6, 42);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(88, 15);
		this.label4.TabIndex = 464;
		this.label4.Text = "Doc No && Date";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(6, 71);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(86, 15);
		this.label28.TabIndex = 465;
		this.label28.Text = "Exporter Name";
		this.CmbExporterbank.AllowNull = (DefaultableBoolean)1;
		this.CmbExporterbank.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbExporterbank.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbExporterbank.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbExporterbank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbExporterbank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbExporterbank).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbExporterbank).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbExporterbank.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbExporterbank).Location = new System.Drawing.Point(436, 36);
		((System.Windows.Forms.Control)(object)this.CmbExporterbank).Name = "CmbExporterbank";
		((System.Windows.Forms.Control)(object)this.CmbExporterbank).Size = new System.Drawing.Size(199, 26);
		((System.Windows.Forms.Control)(object)this.CmbExporterbank).TabIndex = 10;
		this.CmbImporterBank.AllowNull = (DefaultableBoolean)1;
		this.CmbImporterBank.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbImporterBank.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbImporterBank.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance200).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance201).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbImporterBank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance201;
		((AppearanceBase)appearance202).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance202;
		((SpecialBoxBase)((UltraGridBase)this.CmbImporterBank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance203).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance203).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance204;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance205).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance205;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance206;
		((AppearanceBase)appearance208).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance208).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance209).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance209).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance209).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance209;
		((AppearanceBase)appearance210).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance211).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbImporterBank).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbImporterBank.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbImporterBank).Location = new System.Drawing.Point(117, 152);
		((System.Windows.Forms.Control)(object)this.CmbImporterBank).Name = "CmbImporterBank";
		((System.Windows.Forms.Control)(object)this.CmbImporterBank).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbImporterBank).TabIndex = 5;
		this.CmbDestinationPort.AllowNull = (DefaultableBoolean)1;
		this.CmbDestinationPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDestinationPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDestinationPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance213;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance214).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance214).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance214).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDestinationPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance214;
		((AppearanceBase)appearance215).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance215;
		((SpecialBoxBase)((UltraGridBase)this.CmbDestinationPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance216).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance216).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance217).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance217;
		((AppearanceBase)appearance219).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance219).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance220;
		((AppearanceBase)appearance221).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance221).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance222).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance222).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance222).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance222).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance222;
		((AppearanceBase)appearance223).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance223;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance224).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDestinationPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).Location = new System.Drawing.Point(436, 94);
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).Name = "CmbDestinationPort";
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).Size = new System.Drawing.Size(199, 26);
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).TabIndex = 12;
		this.CmbLoadingPort.AllowNull = (DefaultableBoolean)1;
		this.CmbLoadingPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLoadingPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLoadingPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance226).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance227).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance227).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLoadingPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance227;
		((AppearanceBase)appearance228).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance228;
		((SpecialBoxBase)((UltraGridBase)this.CmbLoadingPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLoadingPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbLoadingPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).Location = new System.Drawing.Point(436, 65);
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).Name = "CmbLoadingPort";
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).Size = new System.Drawing.Size(199, 26);
		((System.Windows.Forms.Control)(object)this.CmbLoadingPort).TabIndex = 11;
		this.CmbPaymentTerms.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentTerms.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentTerms.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentTerms.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentTerms).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerms).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPaymentTerms.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerms).Location = new System.Drawing.Point(724, 152);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerms).Name = "CmbPaymentTerms";
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerms).Size = new System.Drawing.Size(112, 26);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerms).TabIndex = 17;
		this.CmbDeliveryTerm.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Location = new System.Drawing.Point(724, 123);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Name = "CmbDeliveryTerm";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Size = new System.Drawing.Size(112, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).TabIndex = 16;
		this.datOrderDate.CustomFormat = "dd-MMM-yyyy";
		this.datOrderDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datOrderDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datOrderDate.Location = new System.Drawing.Point(436, 154);
		this.datOrderDate.Name = "datOrderDate";
		this.datOrderDate.Size = new System.Drawing.Size(199, 23);
		this.datOrderDate.TabIndex = 14;
		this.txtDocDate.CustomFormat = "dd-MMM-yyyy";
		this.txtDocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocDate.Location = new System.Drawing.Point(203, 38);
		this.txtDocDate.Name = "txtDocDate";
		this.txtDocDate.Size = new System.Drawing.Size(134, 23);
		this.txtDocDate.TabIndex = 1;
		this.CmbNotifyParty.AllowNull = (DefaultableBoolean)1;
		this.CmbNotifyParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbNotifyParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbNotifyParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbNotifyParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.CmbNotifyParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbNotifyParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbNotifyParty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty).Location = new System.Drawing.Point(117, 123);
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty).Name = "CmbNotifyParty";
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbNotifyParty).TabIndex = 4;
		this.CmbCustomer.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Location = new System.Drawing.Point(117, 94);
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Name = "CmbCustomer";
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomer).TabIndex = 3;
		this.CmbSalePerson.AllowNull = (DefaultableBoolean)1;
		this.CmbSalePerson.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSalePerson.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSalePerson.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSalePerson).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.CmbSalePerson).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSalePerson).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSalePerson.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSalePerson).Location = new System.Drawing.Point(117, 65);
		((System.Windows.Forms.Control)(object)this.CmbSalePerson).Name = "CmbSalePerson";
		((System.Windows.Forms.Control)(object)this.CmbSalePerson).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbSalePerson).TabIndex = 2;
		this.txtSpecialInstructions.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSpecialInstructions.Location = new System.Drawing.Point(117, 211);
		this.txtSpecialInstructions.Name = "txtSpecialInstructions";
		this.txtSpecialInstructions.Size = new System.Drawing.Size(518, 23);
		this.txtSpecialInstructions.TabIndex = 7;
		this.txtMTon.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMTon.Location = new System.Drawing.Point(939, 125);
		this.txtMTon.Name = "txtMTon";
		this.txtMTon.ReadOnly = true;
		this.txtMTon.Size = new System.Drawing.Size(104, 23);
		this.txtMTon.TabIndex = 21;
		this.txtMTon.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFCYAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFCYAmount.Location = new System.Drawing.Point(724, 211);
		this.txtFCYAmount.Name = "txtFCYAmount";
		this.txtFCYAmount.ReadOnly = true;
		this.txtFCYAmount.Size = new System.Drawing.Size(112, 23);
		this.txtFCYAmount.TabIndex = 19;
		this.txtFCYAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFCYAmount.TextChanged += new System.EventHandler(txtFCYAmount_TextChanged);
		this.panel3.Controls.Add(this.toolstrip);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1050, 30);
		this.panel3.TabIndex = 0;
		this.toolstrip.Font = new System.Drawing.Font("Verdana", 8.5f);
		this.toolstrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[12]
		{
			this.btnnew, this.btnsave, this.btnSaveAs, this.btnUpdate, this.Print, this.btnAttachment, this.toolStripButton1, this.toolStripButton2, this.btnDefinePaymentTerm, this.btnPackingTypeLink,
			this.btnSeaPortDefine, this.btnPackingMaterialRequirement
		});
		this.toolstrip.Location = new System.Drawing.Point(0, 0);
		this.toolstrip.Name = "toolstrip";
		this.toolstrip.Size = new System.Drawing.Size(1050, 27);
		this.toolstrip.TabIndex = 5;
		this.toolstrip.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 8.75f);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 8.75f);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(62, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnSaveAs.Font = new System.Drawing.Font("Verdana", 8.75f);
		this.btnSaveAs.Image = (System.Drawing.Image)resources.GetObject("btnSaveAs.Image");
		this.btnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveAs.Name = "btnSaveAs";
		this.btnSaveAs.Size = new System.Drawing.Size(81, 24);
		this.btnSaveAs.Text = "&Save As";
		this.btnSaveAs.Visible = false;
		this.btnSaveAs.Click += new System.EventHandler(btnSaveAs_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 8.75f);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(77, 24);
		this.btnUpdate.Text = "Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.Print.Font = new System.Drawing.Font("Verdana", 8.75f);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(89, 24);
		this.Print.Text = "501-Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 8.75f);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(103, 24);
		this.btnAttachment.Text = "Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripButton1.Font = new System.Drawing.Font("Verdana", 8.75f);
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(79, 24);
		this.toolStripButton1.Text = "Refresh";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.toolStripButton2.Font = new System.Drawing.Font("Verdana", 8.75f);
		this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton2.Name = "toolStripButton2";
		this.toolStripButton2.Size = new System.Drawing.Size(95, 24);
		this.toolStripButton2.Text = "Delivery Term";
		this.toolStripButton2.Click += new System.EventHandler(btnDeliveryTerm_Click);
		this.btnDefinePaymentTerm.Font = new System.Drawing.Font("Verdana", 8.75f);
		this.btnDefinePaymentTerm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDefinePaymentTerm.Name = "btnDefinePaymentTerm";
		this.btnDefinePaymentTerm.Size = new System.Drawing.Size(100, 24);
		this.btnDefinePaymentTerm.Text = "Payment Term";
		this.btnDefinePaymentTerm.Click += new System.EventHandler(btnPaymentTerms_Click);
		this.btnPackingTypeLink.Font = new System.Drawing.Font("Verdana", 8.75f);
		this.btnPackingTypeLink.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPackingTypeLink.Name = "btnPackingTypeLink";
		this.btnPackingTypeLink.Size = new System.Drawing.Size(92, 24);
		this.btnPackingTypeLink.Text = "Packing Type";
		this.btnPackingTypeLink.Click += new System.EventHandler(btnPackingTypeLink_Click);
		this.btnSeaPortDefine.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSeaPortDefine.Name = "btnSeaPortDefine";
		this.btnSeaPortDefine.Size = new System.Drawing.Size(65, 24);
		this.btnSeaPortDefine.Text = "Sea Port";
		this.btnSeaPortDefine.Click += new System.EventHandler(btnSeaPortDefine_Click);
		this.btnPackingMaterialRequirement.Font = new System.Drawing.Font("Verdana", 8.75f);
		this.btnPackingMaterialRequirement.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPackingMaterialRequirement.Name = "btnPackingMaterialRequirement";
		this.btnPackingMaterialRequirement.Size = new System.Drawing.Size(197, 24);
		this.btnPackingMaterialRequirement.Text = "Packing Material Requirement";
		this.btnPackingMaterialRequirement.Click += new System.EventHandler(btnPackingMaterialRequirement_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.panel18);
		this.tabPage2.Controls.Add(this.panel17);
		this.tabPage2.Controls.Add(this.panel11);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grddetailhistory);
		this.tabPage2.Controls.Add(this.panel13);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1056, 687);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.toolStrip3);
		this.panel13.Controls.Add(this.label43);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(3, 3);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1050, 52);
		this.panel13.TabIndex = 0;
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.ctrlGrdBar1);
		this.panel18.Controls.Add(this.label54);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(3, 129);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1050, 24);
		this.panel18.TabIndex = 1;
		this.grddetailhistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grddetailhistory).BackColor = System.Drawing.Color.White;
		this.grddetailhistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grddetailhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grddetailhistory.DesignTimeLayout = grddetailhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grddetailhistory).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grddetailhistory.FilterMode = (FilterMode)2;
		((System.Windows.Forms.Control)(object)this.grddetailhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grddetailhistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grddetailhistory).Location = new System.Drawing.Point(3, 464);
		((System.Windows.Forms.Control)(object)this.grddetailhistory).Name = "grddetailhistory";
		((ScrollableBase)this.grddetailhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grddetailhistory).Size = new System.Drawing.Size(1050, 220);
		((System.Windows.Forms.Control)(object)this.grddetailhistory).TabIndex = 2;
		this.grddetailhistory.TotalRow = (InheritableBoolean)1;
		this.grddetailhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grddetailhistory).VisualStyle = (VisualStyle)4;
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label42);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel11.Location = new System.Drawing.Point(3, 437);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1050, 27);
		this.panel11.TabIndex = 343;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label42.ForeColor = System.Drawing.Color.White;
		this.label42.Location = new System.Drawing.Point(5, 5);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(241, 17);
		this.label42.TabIndex = 562;
		this.label42.Text = "Detail Of The Selected Above Row";
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.DataGridHistory).BackColor = System.Drawing.Color.White;
		this.DataGridHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(3, 153);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1050, 284);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 1;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		this.panel17.BackColor = System.Drawing.Color.White;
		this.panel17.Controls.Add(this.groupBox5);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(3, 55);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1050, 74);
		this.panel17.TabIndex = 0;
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.ForeColor = System.Drawing.Color.White;
		this.label43.Location = new System.Drawing.Point(4, 30);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(212, 19);
		this.label43.TabIndex = 562;
		this.label43.Text = "Proforma Invoice History";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1002, 1);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 22);
		this.ctrlGrdBar1.TabIndex = 8;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnResetHistory, this.btnRefreshHistory });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1050, 27);
		this.toolStrip3.TabIndex = 7;
		this.toolStrip3.Text = "toolStrip3";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label54.ForeColor = System.Drawing.Color.White;
		this.label54.Location = new System.Drawing.Point(5, 2);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(132, 18);
		this.label54.TabIndex = 562;
		this.label54.Text = "Filtered Records";
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.btnResetHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnResetHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetHistory.Name = "btnResetHistory";
		this.btnResetHistory.Size = new System.Drawing.Size(59, 24);
		this.btnResetHistory.Text = "&New";
		this.btnResetHistory.Click += new System.EventHandler(btnResetHistory_Click);
		this.groupBox5.Controls.Add(this.rdapproveddate);
		this.groupBox5.Controls.Add(this.btnShowHistory);
		this.groupBox5.Controls.Add(this.rdmodifydate);
		this.groupBox5.Controls.Add(this.label67);
		this.groupBox5.Controls.Add(this.drdocdate);
		this.groupBox5.Controls.Add(this.rdentrydate);
		this.groupBox5.Controls.Add(this.label73);
		this.groupBox5.Controls.Add(this.label89);
		this.groupBox5.Controls.Add(this.label90);
		this.groupBox5.Controls.Add(this.label91);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox5.Controls.Add(this.ToDocNoHistory);
		this.groupBox5.Controls.Add(this.FromDocNoHistory);
		this.groupBox5.Controls.Add(this.ToDateHistory);
		this.groupBox5.Controls.Add(this.FromDateHistory);
		this.groupBox5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox5.Location = new System.Drawing.Point(6, 1);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(922, 71);
		this.groupBox5.TabIndex = 0;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(816, 44);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 9;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(668, 40);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 5;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(816, 18);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label67.Location = new System.Drawing.Point(373, 20);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(58, 15);
		this.label67.TabIndex = 87;
		this.label67.Text = "Customer";
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(735, 18);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 6;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(735, 44);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 7;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.Location = new System.Drawing.Point(213, 46);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(64, 15);
		this.label73.TabIndex = 86;
		this.label73.Text = "To Doc No";
		this.label89.AutoSize = true;
		this.label89.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label89.Location = new System.Drawing.Point(213, 20);
		this.label89.Name = "label89";
		this.label89.Size = new System.Drawing.Size(79, 15);
		this.label89.TabIndex = 85;
		this.label89.Text = "From Doc No";
		this.label90.AutoSize = true;
		this.label90.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label90.Location = new System.Drawing.Point(3, 46);
		this.label90.Name = "label90";
		this.label90.Size = new System.Drawing.Size(48, 15);
		this.label90.TabIndex = 84;
		this.label90.Text = "To Date";
		this.label91.AutoSize = true;
		this.label91.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label91.Location = new System.Drawing.Point(3, 20);
		this.label91.Name = "label91";
		this.label91.Size = new System.Drawing.Size(63, 15);
		this.label91.TabIndex = 83;
		this.label91.Text = "From Date";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance181).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance181).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance182;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance184).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance184).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance184).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance186).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance190;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance191).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance192).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance192).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance192).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance192).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance194;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(434, 14);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(296, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 4;
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(294, 42);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(294, 16);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(69, 42);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(142, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(69, 16);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(142, 23);
		this.FromDateHistory.TabIndex = 0;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1064, 713);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "ProformaInvoice";
		this.Text = "Export Contract";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ProformaInvoice_FormClosing);
		base.Load += new System.EventHandler(FrmExportSalesContract_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(FrmExportSalesContract_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabContractDetail.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetails).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.combitem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combpcktype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combrateuom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combitempck).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).EndInit();
		this.tabOtherItems.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdotheritems).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOtherItem).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCommAgent).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFCYCode).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbExporterbank).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbImporterBank).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDestinationPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLoadingPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTerms).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbNotifyParty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomer).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSalePerson).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolstrip.ResumeLayout(false);
		this.toolstrip.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grddetailhistory).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel17.ResumeLayout(false);
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		base.ResumeLayout(false);
	}
}
