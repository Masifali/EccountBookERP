using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Export;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Export;

public class frmExportInvoicePackingList : Form
{
	private Rightsobjects formrights = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtPackingListDetail = new DataTable();

	private DataTable dtdetail = new DataTable();

	private DataTable dtHistoryGrid = new DataTable();

	private DataTable dtInvoiceNo = new DataTable();

	private DataTable dtBrandForPMItem = new DataTable();

	private bool FormInitialized;

	private string ScreenName = "frmExportInvoicePackingList";

	private int RecId;

	private int DocumentTypeId = 204;

	private bool UpdateMode;

	private int updateDetailIndexPackList = -1;

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool ItemSearchWithNameOrCode;

	private bool AllowExportMultiCompanies;

	private List<ExImInvoicePackingListDetail> LstRemoveRecordDetail = new List<ExImInvoicePackingListDetail>();

	private IContainer components;

	private Panel panel4;

	private Panel panel5;

	private Panel panel1;

	private Panel panel7;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private GroupBox HeaderGropBox;

	private Label label28;

	private Label label21;

	private Label label3;

	private DateTimePicker txtDocDate;

	private TextBox txtdocno;

	private Label label5;

	private Label label1;

	private Label label43;

	private Panel panel10;

	private Label label23;

	private Panel panel12;

	private ToolStrip toolStrip4;

	private ToolStripButton btnNew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private TabPage tabPage2;

	private Panel panel16;

	private ToolStrip toolStrip3;

	private ToolStripSeparator toolStripSeparator1;

	internal UltraCombo cmbFCurrencr;

	internal UltraCombo cmbEFormNo;

	private ToolStrip miniToolStrip;

	private ToolStripButton toolStripButton1;

	private ToolStripButton toolStripButton2;

	private ToolStripButton toolStripButton3;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton toolStripButton4;

	private ToolStripButton toolStripButton5;

	private ToolStripButton helpToolStripButton;

	private ToolStrip toolStrip2;

	private CtrlGrdBar ctrlDataGridHistory;

	private Label label4;

	private Label label10;

	private Label label8;

	private Label label7;

	private TextBox txtLotRefHeader;

	private Label label9;

	private Label label12;

	private Label label24;

	private Label label26;

	private Label label56;

	private Label label16;

	private Label label41;

	private Label label47;

	private Label label22;

	private TextBox txtFcyAmount;

	private Label label25;

	private Label label27;

	private TextBox txtLocalAmt;

	private Label label42;

	private TextBox txtExRate;

	private Label label44;

	private Label label45;

	private DateTimePicker txtGdDate;

	private TextBox txtGdNo;

	private Label label49;

	private Label label46;

	private TextBox txtGrossWeight;

	private Label label54;

	private TextBox txtNetWeight;

	private Label label55;

	private TextBox txtNoOfContainer;

	private Label label57;

	private Label label53;

	private Label label50;

	private Label label31;

	private Label label60;

	private Label label62;

	private Label label61;

	private TextBox txtCertificateOfOrigion;

	private TextBox txtCertificate1;

	private TextBox txtTotalNetAmount;

	private Label label66;

	private TextBox txtaddlessamount;

	private Label label64;

	private Label label40;

	private TextBox txtaddlesscommnets;

	private Label label67;

	private Label label68;

	private CheckBox ChkPrintPreview;

	private ToolStripButton btnPrint;

	private Panel panel17;

	private Panel panel11;

	private Label label17;

	private Label label2;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnShortCutKey;

	private Label label86;

	private TextBox txtDeliveryRemarks;

	private Label label87;

	private Panel panel15;

	private Label label69;

	private Panel panel3;

	internal GridEX DataGridHistory;

	private Label label92;

	private GroupBox groupBox2;

	private Button btnShowHistory;

	private Label label20;

	private Label label73;

	private Label label89;

	private Label label90;

	private Label label91;

	internal UltraCombo CmbCustomerHistory;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private ToolStripButton btnResetHistory;

	private ToolStripButton btnRefreshHistory;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Label labelExportCompany;

	private Label label48;

	internal UltraCombo CmbInvoiceNoHistory;

	private GroupBox groupBox1;

	private RadioButton rdAll;

	private RadioButton rdReferred;

	private RadioButton rdNotReferred;

	private TextBox txtCustomerContractsNos;

	private TextBox txtDocumentaryCreditNo;

	private Label label107;

	private Label label95;

	private DateTimePicker txtDocumentaryCreditNoDate;

	private Label label114;

	private Label label108;

	private TextBox txtTcpRegNo;

	private Label label113;

	private Label label110;

	private Label label111;

	private Label label115;

	private Label label109;

	private TextBox txtCustomerContractsDates;

	private Label label120;

	private TextBox txtRemarks2;

	private TextBox txtRemarks1;

	private Label label118;

	private Label label117;

	private Label label116;

	private Label label129;

	internal GridEX grdPackListDetail;

	private Panel panel25;

	private CtrlGrdBar ctrlgrdPackListDetail;

	private Panel panelDetail;

	private Label label141;

	private TextBox txtRemarksPackListDetail;

	private Label label145;

	internal UltraCombo CmbContainerNoPackListDetail;

	private Label label148;

	private Button BtnCancelPackListDetail;

	private Button BtnUpdatePackListDetail;

	private Button BtnAddPackListDetail;

	internal UltraCombo CmbPmDetailByItemCusPackListDetail;

	internal UltraCombo CmbCommodityByItemCusPackListDetail;

	private TextBox txtPmDetailPackListDetail;

	private TextBox txtCommodityPackListDetail;

	private Label label135;

	private Label label137;

	private Label label138;

	private Label label139;

	private DateTimePicker txtPackageDatePackListDetail;

	private Label label142;

	internal UltraCombo CmbInspectionNoPackListDetail;

	private Label label144;

	private TextBox txtLotNoPackListDetail;

	private Label label146;

	private DateTimePicker txtExpiryDatePackListDetail;

	private Label label143;

	private TextBox txtNetWeightPackListDetail;

	private Label label150;

	private TextBox txtGrossWeightPackListDetail;

	private Label label149;

	private TextBox txtOuterQtyPackListDetail;

	private Label label147;

	private TextBox txtOtherRemarks2;

	private Label label155;

	private TextBox txtOtherRemarks1;

	private Label label154;

	internal UltraCombo CmbItemFromInvDetailPackListDetail;

	private Label label84;

	private Label label160;

	private Label label159;

	private Label label158;

	private Label label157;

	private Label label156;

	private Label label85;

	private Label label29;

	private Label label18;

	private Label label13;

	private Label label6;

	private RadioButton RadItemCodePackListDetail;

	private RadioButton RadItemNamePackListDetail;

	internal GridEX grdPackListHistory;

	private Panel panel32;

	private Label label34;

	private GroupBox groupBox3;

	private TextBox txtFcyAmountHeaderCustom;

	private Label label112;

	private Label label119;

	private TextBox txtAddLessHeaderCustom;

	private TextBox txtLocalAmountHeaderCustom;

	private TextBox txtTotalFcyAmountHeaderCustom;

	private Label label121;

	private Label label122;

	internal UltraCombo cmbInvoiceNo;

	private Panel panel38;

	internal UltraCombo CmbInnerUomInDetail;

	private Label label11;

	private TextBox txtQtyInnerInDetail;

	private Label label14;

	internal UltraCombo CmbOuterUomInDetail;

	private Label label30;

	internal UltraCombo CmbInspectionSubLotNoPackListDetail;

	private Label label15;

	private Label label37;

	private Label label33;

	private Label label32;

	internal TextBox txtCustomer;

	private TextBox txtExporterBank;

	private TextBox txtImporterBank;

	private TextBox txtNotifyParty3;

	private TextBox txtNotifyParty2;

	private TextBox txtNotifyParty1;

	internal TextBox txtConsignee;

	private TextBox CmbFcyCode;

	private TextBox txtPlaceOfDelivery;

	private TextBox txtImporterCountry;

	private TextBox txtOriginCountry;

	private TextBox txtContinent;

	private TextBox txtLoadingPort;

	private TextBox txtDeliveryTerm;

	private TextBox txtCareierType;

	private TextBox txtDestinationPort;

	internal TextBox CmbExportCompany;

	private ToolStripButton BtnDelete;

	private ToolStripButton btnPrint529B;

	private ToolStripButton btnPrint529C;

	private CheckBox chkSlipC;

	private CheckBox ChkSlipB;

	public frmExportInvoicePackingList(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InitializeComponentCustom();
		InitializeComponentMethod();
		FormHelper.FormEventHandler(this);
	}

	private void InitializeComponentCustom()
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 800);
			ScreenName = Conversion.ToString((object)ScreenName);
			dtInvoiceNo.Columns.Add("Id", typeof(int));
			dtInvoiceNo.Columns.Add("InvoiceNo");
			dtBrandForPMItem.Columns.Add("Id", typeof(int));
			dtBrandForPMItem.Columns.Add("ItemCode");
			dtBrandForPMItem.Columns.Add("ItemName");
			dtBrandForPMItem.Columns.Add("PackingTypeId", typeof(int));
			dtBrandForPMItem.Columns.Add("PackingType", typeof(string));
			dtBrandForPMItem.Columns.Add("PackUomId", typeof(int));
			dtBrandForPMItem.Columns.Add("PackUom", typeof(string));
			dtBrandForPMItem.Columns.Add("OuterQty", typeof(double));
			dtBrandForPMItem.Columns.Add("InnerQty", typeof(double));
			dtBrandForPMItem.Columns.Add("M_Ton", typeof(double));
			dtPackingListDetail.Columns.Add("Id", typeof(int));
			dtPackingListDetail.Columns.Add("DoDocumentTypeId", typeof(int));
			dtPackingListDetail.Columns.Add("DoId", typeof(int));
			dtPackingListDetail.Columns.Add("DoDetailId", typeof(int));
			dtPackingListDetail.Columns.Add("ContainerId", typeof(int));
			dtPackingListDetail.Columns.Add("ContainerNo", typeof(string));
			dtPackingListDetail.Columns.Add("ItemId", typeof(int));
			dtPackingListDetail.Columns.Add("ItemCode", typeof(string));
			dtPackingListDetail.Columns.Add("ItemName", typeof(string));
			dtPackingListDetail.Columns.Add("ItemCommodityDetail", typeof(string));
			dtPackingListDetail.Columns.Add("ItemPmDetail", typeof(string));
			dtPackingListDetail.Columns.Add("PackageDate", typeof(DateTime));
			dtPackingListDetail.Columns.Add("ExpiryDate", typeof(DateTime));
			dtPackingListDetail.Columns.Add("LotNo");
			dtPackingListDetail.Columns.Add("ThirdPartyAnalysisId", typeof(int));
			dtPackingListDetail.Columns.Add("ThirdPartyAnalysisNo", typeof(string));
			dtPackingListDetail.Columns.Add("ThirdPartyAnalysisSubId", typeof(int));
			dtPackingListDetail.Columns.Add("ThirdPartyAnalysisSubNo");
			dtPackingListDetail.Columns.Add("OuterQty", typeof(double));
			dtPackingListDetail.Columns.Add("OuterUomId", typeof(int));
			dtPackingListDetail.Columns.Add("OuterUom", typeof(string));
			dtPackingListDetail.Columns.Add("OuterUomEquivalent", typeof(double));
			dtPackingListDetail.Columns.Add("InnerQty", typeof(double));
			dtPackingListDetail.Columns.Add("InnerUomId", typeof(int));
			dtPackingListDetail.Columns.Add("InnerUom", typeof(string));
			dtPackingListDetail.Columns.Add("InnerUomEquivalent", typeof(double));
			dtPackingListDetail.Columns.Add("NetWeight", typeof(double));
			dtPackingListDetail.Columns.Add("GrossWeight", typeof(double));
			dtPackingListDetail.Columns.Add("Remarks", typeof(string));
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
			AllowExportMultiCompanies = CommonServices.GetERPFeatureById(9);
			formrights = CommonServices.SetRightsValueInRightsObject(ScreenName);
			DataTable historyComboData = HistoryComboDbCall();
			DataTable invoiceNoData = ExImInvoice.ExImInvoice_GetInvoicesRefferedInDo(UserAccount.OrganizationId, UserAccount.CompanyId, clsGlobalVariables.ActiveYr.Id);
			return new
			{
				HistoryComboData = historyComboData,
				InvoiceNoData = invoiceNoData
			};
		}).ContinueWith(t =>
		{
			if (t.IsFaulted)
			{
				MessageBox.Show("Error occurred during database call.");
			}
			else
			{
				var result = t.Result;
				btnsave.Enabled = formrights.DoHaveSaveRight;
				btnupdate.Enabled = formrights.DoHaveUpdateRights;
				BtnDelete.Enabled = formrights.DoHaveCanDelete;
				btnPrint.Enabled = formrights.DoHavePrintRights;
				ctrlDataGridHistory.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
				ctrlDataGridHistory.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
				ctrlDataGridHistory.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
				ctrlDataGridHistory.mGridExport.Enabled = formrights.DoHaveGridExportRights;
				ctrlDataGridHistory.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
				ctrlDataGridHistory.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
				CmbExportCompany.Visible = AllowExportMultiCompanies;
				labelExportCompany.Visible = AllowExportMultiCompanies;
				if (ItemSearchWithNameOrCode)
				{
					RadItemCodePackListDetail.Checked = true;
				}
				else
				{
					RadItemCodePackListDetail.Checked = false;
				}
				InvoiceNoBind(result.InvoiceNoData);
				BindGrids(grdPackListDetail, dtPackingListDetail);
				HistoryComboBind(result.HistoryComboData);
				FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
				ToDateHistory.Value = DateTime.Now;
				FormInitialized = true;
				ChkPrintPreview.Checked = true;
				txtDocDate.Focus();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private DataTable HistoryComboDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return ExImLcOrder.GetDataForDropDownFromExportInvoice(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void HistoryComboBind(DataTable dt)
	{
		try
		{
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id", typeof(int));
			dtCustomer.Columns.Add("name", typeof(string));
			DataTable dtInvoice = dtCustomer.Clone();
			if (dt.Rows.Count > 0)
			{
				foreach (DataRow dr in dt.Rows)
				{
					string Activity = Conversion.ToString(dr["ActivityType"]);
					if (Activity == "Customer")
					{
						dtCustomer.Rows.Add(dr["Id"], dr["name"]);
					}
					else if (Activity == "Invoice")
					{
						dtInvoice.Rows.Add(dr["Id"], dr["name"]);
					}
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbCustomerHistory, dtCustomer, "Id", "name", "Customer Name", AllColumns: true, Conversion.ToInt(CmbCustomerHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbInvoiceNoHistory, dtInvoice, "Id", "name", "Invoice No", AllColumns: true, Conversion.ToInt(CmbInvoiceNoHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetConfigurationsFromGlobal()
	{
		try
		{
			AllowExportMultiCompanies = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("AllowExportMultiCompanies"));
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"));
			ItemSearchWithNameOrCode = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ItemSearchByCode"));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvoiceNoBind(DataTable dt)
	{
		try
		{
			dtInvoiceNo.Clear();
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtInvoiceNo.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["InvoiceNo"]);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(cmbInvoiceNo, dtInvoiceNo, "Id", "InvoiceNo", "InvoiceNo", AllColumns: true, Conversion.ToInt(cmbInvoiceNo.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbInvoiceNo_Leave(object sender, EventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(cmbInvoiceNo.Value);
			if (Id == 0)
			{
				throw new Exception("Please Select an Invoice First");
			}
			ReadById(Id, History: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbItemFromInvDetailPackListDetail_Leave(object sender, EventArgs e)
	{
		try
		{
			Conversion.ToInt(CmbItemFromInvDetailPackListDetail.Value);
			new DataTable();
			new DataTable();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCommodityByItemCusPackListDetail_Leave(object sender, EventArgs e)
	{
		try
		{
			int num = Conversion.ToInt(CmbCommodityByItemCusPackListDetail.Value);
			string selectedText = Conversion.ToString((object)((Control)(object)CmbCommodityByItemCusPackListDetail).Text);
			if (num != 0 && !string.IsNullOrWhiteSpace(selectedText) && string.IsNullOrWhiteSpace(txtCommodityPackListDetail.Text))
			{
				txtCommodityPackListDetail.Text = selectedText;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPmDetailByItemCusPackListDetail_Leave(object sender, EventArgs e)
	{
		try
		{
			int num = Conversion.ToInt(CmbPmDetailByItemCusPackListDetail.Value);
			string selectedText = Conversion.ToString((object)((Control)(object)CmbPmDetailByItemCusPackListDetail).Text);
			if (num != 0 && !string.IsNullOrWhiteSpace(selectedText) && string.IsNullOrWhiteSpace(txtPmDetailPackListDetail.Text))
			{
				txtPmDetailPackListDetail.Text = selectedText;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable ItemCommodityDetailDbCall(int ItemId, int SupplierCustomerId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		try
		{
			return CommodityDetailItemCustomerWise.GetRemarks(new CommodityDetailItemCustomerWise
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ItemId = ItemId,
				SupplierCustomerId = SupplierCustomerId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private bool PackingListFormValidation()
	{
		return FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
		{
			((Control)(object)CmbContainerNoPackListDetail, "Container No", FormHelper.ValidType.INT),
			((Control)(object)CmbItemFromInvDetailPackListDetail, "Item", FormHelper.ValidType.INT),
			(txtCommodityPackListDetail, "Commodity Detail", FormHelper.ValidType.String),
			(txtPmDetailPackListDetail, "Pm Detail", FormHelper.ValidType.String),
			(txtPackageDatePackListDetail, "Package Date", FormHelper.ValidType.DateTime),
			(txtExpiryDatePackListDetail, "Expiry Date", FormHelper.ValidType.DateTime),
			(txtOuterQtyPackListDetail, "Qty", FormHelper.ValidType.Double),
			(txtNetWeightPackListDetail, "Net Weight", FormHelper.ValidType.Double),
			(txtGrossWeightPackListDetail, "Gross Weight", FormHelper.ValidType.Double)
		});
	}

	private void BindGrids(GridEX grid, DataTable dt)
	{
		try
		{
			if (dt != null && dt.Columns.Count > 0)
			{
				grid.DataSource = dt;
				grid.RetrieveStructure();
				grdSettingsPackListDetaill(grid);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ValidatePackListWithDetailGrid(DataTable dtdetail)
	{
		try
		{
			if (Conversion.ToDouble((object)txtGrossWeightPackListDetail.Text) < Conversion.ToDouble((object)txtNetWeightPackListDetail.Text))
			{
				throw new Exception("Gross Weight Cannot be less than Net Weight");
			}
			int ItemId = Conversion.ToInt(CmbItemFromInvDetailPackListDetail.Value);
			double CurrQty = Conversion.ToDouble((object)txtOuterQtyPackListDetail.Text.Trim());
			double CurrNetWeight = Conversion.ToDouble((object)txtNetWeightPackListDetail.Text.Trim());
			Conversion.ToDouble((object)txtGrossWeightPackListDetail.Text.Trim());
			double DetailGridQtyMton = 0.0;
			double DetailGridQty = 0.0;
			foreach (DataRow row2 in dtdetail.Rows)
			{
				if (Conversion.ToInt(row2["ItemId"]) == ItemId)
				{
					DetailGridQty += Conversion.ToDouble(row2["OuterQty"]);
					DetailGridQtyMton += Conversion.ToDouble(row2["Qty/M.Ton"]);
				}
			}
			double PackListGridQty = 0.0;
			double PackListGridNetWeight = 0.0;
			GridEXRow[] rows = grdPackListDetail.GetRows();
			foreach (GridEXRow row in rows)
			{
				if (Conversion.ToInt(row.Cells["ItemId"].Value) == ItemId && row.RowIndex != updateDetailIndexPackList)
				{
					PackListGridQty += Conversion.ToDouble(row.Cells["OuterQty"].Value);
					PackListGridNetWeight += Conversion.ToDouble(row.Cells["NetWeight"].Value);
				}
			}
			if (PackListGridQty + CurrQty > DetailGridQty)
			{
				throw new Exception($"Total Qty{PackListGridQty + CurrQty} can not Greater than Detail Grid Qty {DetailGridQty} for This Item");
			}
			if (CurrNetWeight + PackListGridNetWeight > DetailGridQtyMton * 1000.0)
			{
				throw new Exception($"Total NetWeight{CurrNetWeight + PackListGridNetWeight} can not Greater than Detail Grid Weight{DetailGridQtyMton * 1000.0} for This Item");
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BtnAddPackListDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (PackingListFormValidation())
			{
				ValidatePackListWithDetailGrid(dtdetail);
				dtPackingListDetail.Rows.Add(0, 0, 0, 0, Conversion.ToInt(CmbContainerNoPackListDetail.Value), Conversion.ToString((object)((Control)(object)CmbContainerNoPackListDetail).Text), Conversion.ToInt(CmbItemFromInvDetailPackListDetail.Value), Conversion.ToString(((UltraDropDownBase)CmbItemFromInvDetailPackListDetail).SelectedRow.Cells["ItemCode"].Value), Conversion.ToString(((UltraDropDownBase)CmbItemFromInvDetailPackListDetail).SelectedRow.Cells["ItemName"].Value), Conversion.ToString(CmbCommodityByItemCusPackListDetail.Value), Conversion.ToString(CmbPmDetailByItemCusPackListDetail.Value), Conversion.ToDateTime((object)txtPackageDatePackListDetail.Value), Conversion.ToDateTime((object)txtExpiryDatePackListDetail.Value), Conversion.ToString((object)txtLotNoPackListDetail.Text), Conversion.ToInt(CmbInspectionNoPackListDetail.Value), Conversion.ToString((object)((Control)(object)CmbInspectionNoPackListDetail).Text), Conversion.ToInt(CmbInspectionSubLotNoPackListDetail.Value), Conversion.ToString((object)((Control)(object)CmbInspectionSubLotNoPackListDetail).Text), Conversion.ToDouble((object)txtOuterQtyPackListDetail.Text.Trim()), Conversion.ToInt(CmbOuterUomInDetail.Value), Conversion.ToString((object)((Control)(object)CmbOuterUomInDetail).Text), Conversion.ToString(((UltraDropDownBase)CmbOuterUomInDetail).SelectedRow.Cells["Equivalent"].Value), Conversion.ToDouble((object)txtQtyInnerInDetail.Text.Trim()), Conversion.ToInt(CmbInnerUomInDetail.Value), Conversion.ToString((object)((Control)(object)CmbInnerUomInDetail).Text), Conversion.ToString(((UltraDropDownBase)CmbInnerUomInDetail).SelectedRow.Cells["Equivalent"].Value), Conversion.ToDouble((object)txtNetWeightPackListDetail.Text.Trim()), Conversion.ToDouble((object)txtGrossWeightPackListDetail.Text.Trim()), txtRemarksPackListDetail.Text.Trim());
				BindGrids(grdPackListDetail, dtPackingListDetail);
				ResetPackListDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUpdatePackListDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (PackingListFormValidation())
			{
				ValidatePackListWithDetailGrid(dtdetail);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["ContainerId"] = Conversion.ToInt(CmbContainerNoPackListDetail.Value);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["ContainerNo"] = Conversion.ToString((object)((Control)(object)CmbContainerNoPackListDetail).Text);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["ItemId"] = Conversion.ToInt(CmbItemFromInvDetailPackListDetail.Value);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["ItemCode"] = Conversion.ToString(((UltraDropDownBase)CmbItemFromInvDetailPackListDetail).SelectedRow.Cells["ItemCode"].Value);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["ItemName"] = Conversion.ToString(((UltraDropDownBase)CmbItemFromInvDetailPackListDetail).SelectedRow.Cells["ItemName"].Value);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["ItemCommodityDetail"] = Conversion.ToString((object)txtCommodityPackListDetail.Text);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["ItemPmDetail"] = Conversion.ToString((object)txtPmDetailPackListDetail.Text);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["PackageDate"] = Conversion.ToString((object)txtPackageDatePackListDetail.Value);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["ExpiryDate"] = Conversion.ToString((object)txtExpiryDatePackListDetail.Value);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["LotNo"] = Conversion.ToString((object)txtLotNoPackListDetail.Text);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["ThirdPartyAnalysisId"] = Conversion.ToInt(CmbInspectionNoPackListDetail.Value);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["ThirdPartyAnalysisNo"] = Conversion.ToString((object)((Control)(object)CmbInspectionNoPackListDetail).Text);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["ThirdPartyAnalysisSubId"] = Conversion.ToInt(CmbInspectionSubLotNoPackListDetail.Value);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["ThirdPartyAnalysisSubNo"] = Conversion.ToString((object)((Control)(object)CmbInspectionSubLotNoPackListDetail).Text);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["OuterQty"] = Convert.ToDouble(txtOuterQtyPackListDetail.Text);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["OuterUomId"] = Conversion.ToInt(CmbOuterUomInDetail.Value);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["OuterUom"] = Conversion.ToString((object)((Control)(object)CmbOuterUomInDetail).Text);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["OuterUomEquivalent"] = Conversion.ToString(((UltraDropDownBase)CmbOuterUomInDetail).SelectedRow.Cells["Equivalent"].Value);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["InnerQty"] = Convert.ToDouble(txtQtyInnerInDetail.Text);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["InnerUomId"] = Conversion.ToInt(CmbInnerUomInDetail.Value);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["InnerUom"] = Conversion.ToString((object)((Control)(object)CmbInnerUomInDetail).Text);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["InnerUomEquivalent"] = Conversion.ToString(((UltraDropDownBase)CmbInnerUomInDetail).SelectedRow.Cells["Equivalent"].Value);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["NetWeight"] = Convert.ToDouble(txtNetWeightPackListDetail.Text);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["GrossWeight"] = Convert.ToDouble(txtGrossWeightPackListDetail.Text);
				dtPackingListDetail.Rows[updateDetailIndexPackList]["Remarks"] = Conversion.ToString((object)txtRemarksPackListDetail.Text);
				grdPackListDetail.DataSource = dtPackingListDetail;
				ResetPackListDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnCancelPackListDetail_Click(object sender, EventArgs e)
	{
		ResetPackListDetail();
	}

	private void grdSettingsPackListDetaill(GridEX grid)
	{
		try
		{
			List<string> hiddenColumns = new List<string>
			{
				"Id", "DoDocumentTypeId", "DoId", "DoDetailId", "ContainerId", "ItemId", "ThirdPartyAnalysisId", "ThirdPartyAnalysisSubId", "OuterUomId", "OuterUomEquivalent",
				"InnerUomId", "InnerUomEquivalent"
			};
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"ItemCommodityDetail",
					(EditType)1
				},
				{
					"ItemPmDetail",
					(EditType)1
				},
				{
					"PackageDate",
					(EditType)2
				},
				{
					"ExpiryDate",
					(EditType)2
				},
				{
					"LotNo",
					(EditType)1
				},
				{
					"GrossWeight",
					(EditType)1
				},
				{
					"Remarks",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grid, 2, 3, hiddenColumns, editableColumns);
			grid.RootTable.Columns["ThirdPartyAnalysisNo"].Caption = "Inspection No";
			grid.RootTable.Columns["ThirdPartyAnalysisSubNo"].Caption = "Inspection Sub Lot No";
			grid.RootTable.Columns["Remarks"].Width = 350;
			if (((Control)(object)grid).Name == "grdPackListDetail")
			{
				GridEX_Helper.AddButton(grid, "Delete", "X", 20, 1);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackListDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdPackListDetail.CurrentRow;
			if (item != null)
			{
				updateDetailIndexPackList = item.RowIndex;
				CmbContainerNoPackListDetail.Value = item.Cells["ContainerId"].Value;
				CmbItemFromInvDetailPackListDetail.Value = item.Cells["ItemId"].Value;
				CmbItemFromInvDetailPackListDetail_Leave(null, null);
				txtCommodityPackListDetail.Text = item.Cells["ItemCommodityDetail"].Value.ToString();
				txtPmDetailPackListDetail.Text = item.Cells["ItemPmDetail"].Value.ToString();
				txtPackageDatePackListDetail.Value = Convert.ToDateTime(item.Cells["PackageDate"].Value);
				txtExpiryDatePackListDetail.Value = Convert.ToDateTime(item.Cells["ExpiryDate"].Value);
				txtLotNoPackListDetail.Text = item.Cells["LotNo"].Value.ToString();
				CmbInspectionNoPackListDetail.Value = item.Cells["ThirdPartyAnalysisId"].Value;
				CmbInspectionSubLotNoPackListDetail.Value = item.Cells["ThirdPartyAnalysisSubId"].Value.ToString();
				txtOuterQtyPackListDetail.Text = item.Cells["OuterQty"].Value.ToString();
				CmbOuterUomInDetail.Value = item.Cells["OuterUomId"].Value;
				txtQtyInnerInDetail.Text = item.Cells["InnerQty"].Value.ToString();
				CmbInnerUomInDetail.Value = item.Cells["InnerUomId"].Value;
				txtNetWeightPackListDetail.Text = item.Cells["NetWeight"].Value.ToString();
				txtGrossWeightPackListDetail.Text = item.Cells["GrossWeight"].Value.ToString();
				txtRemarksPackListDetail.Text = item.Cells["Remarks"].Value.ToString();
				BtnAddPackListDetail.Visible = false;
				BtnUpdatePackListDetail.Visible = true;
				BtnCancelPackListDetail.Visible = true;
				CmbContainerNoPackListDetail.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackListDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPackListDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "Delete")
			{
				DeletePackListDetailRow(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeletePackListDetailRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				ExImInvoicePackingListDetail vd = new ExImInvoicePackingListDetail();
				vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
				FillDetailListCommonForInsertAndDelete(vd, r);
				vd.ActionTypeId = 3;
				LstRemoveRecordDetail.Add(vd);
				r.Delete();
			}
			else
			{
				r.Delete();
			}
			grdPackListDetail.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void CheckItemIds(DataTable dtDetail, DataTable dtPackListDetail)
	{
		try
		{
			double dtDetailTotalNetWeight = DatatableHelper.GetColumnSum(dtDetail, "NetWeight");
			double dtDetailTotalGrossWeight = DatatableHelper.GetColumnSum(dtDetail, "GrossWeight");
			double dtPackListTotalNetWeight = DatatableHelper.GetColumnSum(dtPackListDetail, "NetWeight");
			double dtPackListTotalGrossWeight = DatatableHelper.GetColumnSum(dtPackListDetail, "GrossWeight");
			if (dtDetailTotalNetWeight - dtPackListTotalNetWeight > 1.0)
			{
				throw new Exception($"Total Net Weight in Detail ({dtDetailTotalNetWeight} kg) does not match PackList ({dtPackListTotalNetWeight} kg).");
			}
			if (dtDetailTotalGrossWeight - dtPackListTotalGrossWeight > 1.0)
			{
				throw new Exception($"Total Gross Weight in Detail ({dtDetailTotalGrossWeight} kg) does not match PackList ({dtPackListTotalGrossWeight} kg).");
			}
		}
		catch (Exception ex)
		{
			throw new Exception("Validation failed: " + ex.Message);
		}
	}

	public void CheckItemIdsOld(DataTable dtDetail, DataTable dtPackListDetail)
	{
		try
		{
			var detailSummary = (from row in dtDetail.AsEnumerable()
				group row by new
				{
					ItemId = row.Field<int>("ItemId"),
					OuterUomId = row.Field<int>("OuterQtyUomId"),
					InnerUomId = row.Field<int>("InnerQtyUomId")
				}).ToDictionary(g => g.Key, g => new
			{
				ItemName = g.First().Field<string>("ItemName"),
				OuterUom = g.First().Field<string>("OuterUOM"),
				InnerUom = g.First().Field<string>("InnerUOM"),
				TotalOuterQty = g.Sum((DataRow r) => r.Field<double>("OuterQty")),
				TotalInnerQty = g.Sum((DataRow r) => r.Field<double>("InnerQty")),
				TotalWeightKg = g.Sum((DataRow r) => r.Field<double>("NetWeight")),
				TotalGrossWeight = g.Sum((DataRow r) => r.Field<double>("GrossWeight"))
			});
			var dictionary = (from row in dtPackListDetail.AsEnumerable()
				group row by new
				{
					ItemId = row.Field<int>("ItemId"),
					OuterUomId = row.Field<int>("OuterUomId"),
					InnerUomId = row.Field<int>("InnerUomId")
				}).ToDictionary(g => g.Key, g => new
			{
				ItemName = g.First().Field<string>("ItemName"),
				OuterUom = g.First().Field<string>("OuterUom"),
				InnerUom = g.First().Field<string>("InnerUom"),
				TotalOuterQty = g.Sum((DataRow r) => r.Field<double>("OuterQty")),
				TotalInnerQty = g.Sum((DataRow r) => r.Field<double>("InnerQty")),
				TotalNetWeight = g.Sum((DataRow r) => r.Field<double>("NetWeight")),
				TotalGrossWeight = g.Sum((DataRow r) => r.Field<double>("GrossWeight"))
			});
			List<string> errors = new List<string>();
			foreach (var pack in dictionary)
			{
				var key = pack.Key;
				var packData = pack.Value;
				if (!detailSummary.ContainsKey(key))
				{
					errors.Add("Item '" + packData.ItemName + "' with OuterUom '" + packData.InnerUom + "' and InnerUom '" + packData.InnerUom + "' exists in PackList but not in Detail.\n");
					continue;
				}
				var detailData = detailSummary[key];
				if (packData.TotalOuterQty != detailData.TotalOuterQty)
				{
					errors.Add($"Total OuterQty for '{packData.ItemName}' with OuterUom '{packData.OuterUom}' in PackList ({packData.TotalOuterQty}) not equal to Detail ({detailData.TotalOuterQty}).\n");
				}
				if (packData.TotalInnerQty != detailData.TotalInnerQty)
				{
					errors.Add($"Total InnerQty for '{packData.ItemName}' with InnerUom '{packData.InnerUom}' in PackList ({packData.TotalInnerQty}) not equal to Detail ({detailData.TotalInnerQty}).\n");
				}
				if (packData.TotalNetWeight != detailData.TotalWeightKg)
				{
					errors.Add($"Total Net Weight for '{packData.ItemName}' in PackList ({packData.TotalNetWeight} kg) not equal to Detail ({detailData.TotalWeightKg} kg).\n");
				}
				if (packData.TotalGrossWeight != detailData.TotalGrossWeight)
				{
					errors.Add($"Total Gross Weight for '{packData.ItemName}' in PackList ({packData.TotalGrossWeight} kg) not equal to Detail ({detailData.TotalGrossWeight} kg).\n");
				}
			}
			if (errors.Any())
			{
				throw new Exception(string.Join("\n", errors));
			}
		}
		catch (Exception ex)
		{
			throw new Exception("Validation failed: " + ex.Message);
		}
	}

	private void Insert()
	{
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Expected O, but got Unknown
		try
		{
			if (grdPackListDetail.GetRows().Count() == 0)
			{
				throw new Exception("Detail Record Not Found");
			}
			double num = Conversion.ToDouble((object)txtGrossWeight.Text);
			double netWeight = Conversion.ToDouble((object)txtNetWeight.Text);
			if (num < netWeight)
			{
				MessageBox.Show("Gross Weight must be equal to or greater than Net Weight. Thank you.");
				txtGrossWeight.Focus();
				return;
			}
			CheckItemIds(dtdetail, dtPackingListDetail);
			if (!FormHelper.ConfirmAction((RecId == 0) ? "Are you sure to Save" : "Are you sure to Update"))
			{
				return;
			}
			ExImInvoice PIH = new ExImInvoice();
			PIH.OrganizationId = UserAccount.OrganizationId;
			PIH.CompanyId = UserAccount.CompanyId;
			PIH.BranchesId = UserAccount.BranchesId;
			PIH.ProjectsId = UserAccount.BranchesId;
			PIH.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			PIH.EntryDate = DateTime.Now;
			PIH.ModifyDate = DateTime.Now;
			PIH.EntryUser = UserAccount.ID;
			PIH.ModifyUser = UserAccount.ID;
			PIH.ApprovedDate = DateTime.Now;
			PIH.IsApproved = false;
			PIH.Id = Conversion.ToInt(cmbInvoiceNo.Value);
			PIH.ExImInvoiceDetail = new List<ExImInvoicePackingDetail>();
			PIH.ExImInvoiceDetailCustom = new List<ExImInvoicePackingDetailCustom>();
			PIH.ExImInvoicePaymentTermsDetail = new List<ExImInvoicePaymentTermsDetail>();
			PIH.ExImInvoicePaymentTermsDetailCustom = new List<ExImInvoicePaymentTermsDetailCustom>();
			PIH.ExImInvoiceOtherChargesDetailList = new List<ExImInvoiceOtherChargesDetail>();
			PIH.ExImInvoiceOtherChargesDetailCustomList = new List<ExImInvoiceOtherChargesDetailCustom>();
			PIH.ExImInvoicePackingListDetailList = new List<ExImInvoicePackingListDetail>();
			PIH.ExImInvoiceOtherItemslist = new List<ExImInvoiceOtherItems>();
			PIH.AttachmentsList = new List<DMSAttachments>();
			PIH.DeleteAttachmentsList = new List<DMSAttachments>();
			if (LstRemoveRecordDetail.Count > 0 && RecId > 0)
			{
				for (int i = 0; i < LstRemoveRecordDetail.Count; i++)
				{
					PIH.ExImInvoicePackingListDetailList.Add(LstRemoveRecordDetail[i]);
				}
			}
			GridEXRow[] rows = grdPackListDetail.GetRows();
			foreach (GridEXRow r in rows)
			{
				ExImInvoicePackingListDetail vd = new ExImInvoicePackingListDetail();
				vd.Id = ((RecId != 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
				vd.ActionTypeId = ((vd.Id <= 0) ? 1 : 2);
				FillDetailListCommonForInsertAndDelete(vd, r);
				FormHelper.ValidateField(vd.ContainerId, "Container", r.RowIndex, "Packing List");
				FormHelper.ValidateField(vd.ItemId, "Item", r.RowIndex, "Packing List");
				FormHelper.ValidateField(vd.CommodityDetail, "Commodity Detail", r.RowIndex, "Packing List");
				FormHelper.ValidateField(vd.PmDetail, "Pm Detail", r.RowIndex, "Packing List");
				FormHelper.ValidateField(vd.PackageDate, "Package Date", r.RowIndex, "Packing List");
				FormHelper.ValidateField(vd.ExpiryDate, "Expiry Date", r.RowIndex, "Packing List");
				FormHelper.ValidateField(vd.OuterQty, "Outer Qty", r.RowIndex, "Packing List");
				FormHelper.ValidateField(vd.NetWeight, "Net Weight", r.RowIndex, "Packing List");
				FormHelper.ValidateField(vd.GrossWeight, "Gross Weight", r.RowIndex, "Packing List");
				PIH.ExImInvoicePackingListDetailList.Add(vd);
			}
			ExImInvoice.SavePackingList(PIH);
			if (RecId == 0)
			{
				MessageBox.Show("Save SuccessFully");
			}
			if (RecId > 0)
			{
				MessageBox.Show("Update SuccessFully");
			}
			FormReset();
			if (ChkPrintPreview.Checked)
			{
				CommonServices.ExportPackingList529A(PIH.Id);
			}
			if (ChkSlipB.Checked)
			{
				CommonServices.ExportPackingList529B(PIH.Id);
			}
			if (chkSlipC.Checked)
			{
				CommonServices.ExportPackingList529C(PIH.Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("Rec Id not found...");
			}
			Insert();
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
			RecId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int ID, bool History)
	{
		try
		{
			FormReset();
			DataTable Obj = ExImInvoice.ExImInvoice_ReadByIdForPackingList(ID);
			if (Obj != null)
			{
				DataRow row = Obj.Rows[0];
				tabControl1.SelectedIndex = 0;
				if (History)
				{
					RecId = ID;
					btnsave.Visible = false;
					btnupdate.Visible = true;
					UpdateMode = true;
					BtnDelete.Visible = true;
					dtInvoiceNo.Clear();
					dtInvoiceNo.Rows.Add(row["Id"], row["InvoiceNo"]);
					InfragisticsHelper.BindAndRetainSelection(cmbInvoiceNo, dtInvoiceNo, "Id", "InvoiceNo", "InvoiceNo", AllColumns: true, Conversion.ToInt(cmbInvoiceNo.Value), insertDefaultRow: false, null, null, ActivateRow: false);
					((Control)(object)cmbInvoiceNo).Text = Conversion.ToString(row["InvoiceNo"]);
					((Control)(object)cmbInvoiceNo).Enabled = false;
				}
				else
				{
					RecId = 0;
					btnsave.Visible = true;
					btnupdate.Visible = false;
					UpdateMode = false;
					((Control)(object)cmbInvoiceNo).Text = Conversion.ToString(row["InvoiceNo"]);
				}
				CmbExportCompany.Text = Conversion.ToString(row["ExportCompany"]);
				txtdocno.Text = Conversion.ToString(row["DocCode"]);
				txtDocDate.Value = Conversion.ToDateTime(row["DocDate"]);
				txtDocumentaryCreditNo.Text = Conversion.ToString(row["DocumentaryCreditNo"]);
				txtDocumentaryCreditNoDate.Value = Conversion.ToDateTime(row["DocumentaryCreditNoIssueDate"]);
				txtCustomer.Text = Conversion.ToString(row["Customer"]);
				txtConsignee.Text = Conversion.ToString(row["Consignee"]);
				txtCustomerContractsNos.Text = Conversion.ToString(row["CustomerContractNos"]);
				txtCustomerContractsDates.Text = Conversion.ToString(row["ContractDates"]);
				txtNotifyParty1.Text = Conversion.ToString(row["NotifyParty1Name"]);
				txtNotifyParty2.Text = Conversion.ToString(row["NotifyParty2Name"]);
				txtNotifyParty3.Text = Conversion.ToString(row["NotifyParty3Name"]);
				txtImporterBank.Text = Conversion.ToString(row["ImporterBank"]);
				txtExporterBank.Text = Conversion.ToString(row["ExporterBank"]);
				txtLotRefHeader.Text = Conversion.ToString(row["LotNoRef"]);
				txtGdNo.Text = Conversion.ToString(row["EFormNo"]);
				txtGdDate.Value = Conversion.ToDateTime(row["EFormDate"]);
				txtLoadingPort.Text = Conversion.ToString(row["LoadingPort"]);
				txtDestinationPort.Text = Conversion.ToString(row["DestinationPort"]);
				txtCareierType.Text = Conversion.ToString(row["CarierType"]);
				txtDeliveryTerm.Text = Conversion.ToString(row["DeliveryTerm"]);
				txtDeliveryRemarks.Text = Conversion.ToString(row["DeliveryRemarks"]);
				txtTcpRegNo.Text = Conversion.ToString(row["TCPRegNo"]);
				txtContinent.Text = Conversion.ToString(row["ContinentName"]);
				txtOriginCountry.Text = Conversion.ToString(row["OriginCountry"]);
				txtImporterCountry.Text = Conversion.ToString(row["ImporterCountry"]);
				txtPlaceOfDelivery.Text = Conversion.ToString(row["PlaceOfDelivery"]);
				CmbFcyCode.Text = Conversion.ToString(row["CurrencyCode"]);
				txtFcyAmount.Text = Conversion.ToDouble(row["FCurrencyAmount"]).ToString("#,##0.####");
				txtExRate.Text = Conversion.ToDouble(row["ConversionRate"]).ToString("#,##0.####");
				txtLocalAmt.Text = Conversion.ToDouble(row["EquivalentAmount"]).ToString("#,##0.####");
				txtaddlesscommnets.Text = Conversion.ToString(row["AddLessComments"]);
				txtaddlessamount.Text = Conversion.ToDouble(row["AddLessAmount"]).ToString();
				txtTotalNetAmount.Text = Conversion.ToDouble(row["TotalAmount"]).ToString("#,##0.####");
				txtNoOfContainer.Text = Conversion.ToInt(row["NoOfContainers"]).ToString();
				double grossMton = Conversion.ToDouble(row["GrossMton"]);
				double grossWeight = Conversion.ToDouble(row["GrossWeight"]);
				txtGrossWeight.Text = ((grossMton > 0.0) ? grossMton.ToString("#,##0.####") : (grossWeight / 1000.0).ToString("#,##0.####"));
				double netMton = Conversion.ToDouble(row["NetMton"]);
				double netWeight = Conversion.ToDouble(row["NetWeight"]);
				txtNetWeight.Text = ((netMton > 0.0) ? netMton.ToString("#,##0.####") : (netWeight / 1000.0).ToString("#,##0.####"));
				txtCertificate1.Text = Conversion.ToString(row["Certificate1"]);
				txtCertificateOfOrigion.Text = Conversion.ToString(row["Certificate2"]);
				txtRemarks1.Text = Conversion.ToString(row["Remarks1"]);
				txtRemarks2.Text = Conversion.ToString(row["Remarks2"]);
				txtOtherRemarks1.Text = Conversion.ToString(row["OtherRemarks1"]);
				txtOtherRemarks2.Text = Conversion.ToString(row["OtherRemarks2"]);
				double FcyAmountCustom = Conversion.ToDouble(row["FcyAmountCustom"]);
				double AddLessAmountCustom = Conversion.ToDouble(row["AddLessAmountCustom"]);
				txtFcyAmountHeaderCustom.Text = FcyAmountCustom.ToString("#,##0.####");
				txtAddLessHeaderCustom.Text = AddLessAmountCustom.ToString("#,##0.####");
				double TotalFcyAmountCustom = FcyAmountCustom + AddLessAmountCustom;
				txtTotalFcyAmountHeaderCustom.Text = TotalFcyAmountCustom.ToString("#,##0.####");
				txtLocalAmountHeaderCustom.Text = (TotalFcyAmountCustom * Conversion.ToDouble(row["ConversionRate"])).ToString("#,##0.####");
				dtdetail = ExImInvoice.ExImInvoicePackingDetail_ReadByHeaderId(ID);
				dtPackingListDetail.Rows.Clear();
				if (History)
				{
					List<ExImInvoicePackingListDetail> DetailList = ExImInvoice.ExImInvoicePackingListDetail_ReadByInvoiceId(ID);
					FillPackingListDetailFromListCommonForReadById(DetailList, dtPackingListDetail);
				}
				else
				{
					FillDetaildtsFromDeliveryOrder(ID);
				}
				BindGrids(grdPackListDetail, dtPackingListDetail);
				((Control)(object)grdPackListDetail).Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailListCommonForInsertAndDelete(ExImInvoicePackingListDetail vd, GridEXRow r)
	{
		vd.RefDocumentTypeId = Conversion.ToInt(r.Cells["DoDocumentTypeId"].Value);
		vd.RefDocId = Conversion.ToInt(r.Cells["DoId"].Value);
		vd.RefDocSubId = Conversion.ToInt(r.Cells["DoDetailId"].Value);
		vd.ContainerId = Conversion.ToInt(r.Cells["ContainerId"].Value);
		vd.ContainerNo = Conversion.ToString(r.Cells["ContainerNo"].Value);
		vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
		vd.ItemName = Conversion.ToString(r.Cells["ItemName"].Value);
		vd.ItemCode = Conversion.ToString(r.Cells["ItemCode"].Value);
		vd.CommodityDetail = Conversion.ToString(r.Cells["ItemCommodityDetail"].Value);
		vd.PmDetail = Conversion.ToString(r.Cells["ItemPmDetail"].Value);
		vd.PackageDate = Conversion.ToDateTime(r.Cells["PackageDate"].Value);
		vd.ExpiryDate = Conversion.ToDateTime(r.Cells["ExpiryDate"].Value);
		vd.LotNo = Conversion.ToString(r.Cells["LotNo"].Value);
		vd.ThirdPartyAnalysisId = Conversion.ToInt(r.Cells["ThirdPartyAnalysisId"].Value);
		vd.ThirdPartyAnalysisNo = Conversion.ToString(r.Cells["ThirdPartyAnalysisNo"].Value);
		vd.ThirdPartyAnalysisSubId = Conversion.ToInt(r.Cells["ThirdPartyAnalysisSubId"].Value);
		vd.SubLotNo = Conversion.ToString(r.Cells["ThirdPartyAnalysisSubNo"].Value);
		vd.OuterQty = Conversion.ToDouble(r.Cells["OuterQty"].Value);
		vd.OuterUomId = Conversion.ToInt(r.Cells["OuterUomId"].Value);
		vd.OuterUom = Conversion.ToString(r.Cells["OuterUom"].Value);
		vd.OuterUomEquivalent = Conversion.ToDouble(r.Cells["OuterUomEquivalent"].Value);
		vd.InnerQty = Conversion.ToDouble(r.Cells["InnerQty"].Value);
		vd.InnerUomId = Conversion.ToInt(r.Cells["InnerUomId"].Value);
		vd.InnerUom = Conversion.ToString(r.Cells["InnerUom"].Value);
		vd.InnerUomEquivalent = Conversion.ToDouble(r.Cells["InnerUomEquivalent"].Value);
		vd.NetWeight = Conversion.ToDouble(r.Cells["NetWeight"].Value);
		vd.GrossWeight = Conversion.ToDouble(r.Cells["GrossWeight"].Value);
		vd.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
	}

	private void FillDetaildtsFromDeliveryOrder(int InvoiceId)
	{
		try
		{
			DataTable dt = ExImInvoice.InvDeliveryOrder_ReadByInvoiceForPackingList(InvoiceId);
			HashSet<int> existingDetailIds = new HashSet<int>();
			GridEXRow[] rows = grdPackListDetail.GetRows();
			for (int i = 0; i < rows.Length; i++)
			{
				int DeliveryOrderDetailId = Conversion.ToInt(rows[i].Cells["DoDetailId"].Value);
				existingDetailIds.Add(DeliveryOrderDetailId);
			}
			foreach (DataRow row in dt.Rows)
			{
				int Id = Conversion.ToInt(row["DetailId"]);
				if (existingDetailIds.Add(Id))
				{
					object[] values = new object[29]
					{
						0,
						row["DocumentTypeId"],
						row["InvDeliveryOrderId"],
						row["DetailId"],
						row["ContainerId"],
						row["ContainerNo"],
						row["ItemId"],
						row["ItemCode"],
						row["ItemName"],
						"",
						"",
						DateTime.Now,
						DateTime.Now,
						"",
						row["ThirdPartyAnalysisId"],
						row["ThirdPartyAnalysisNo"],
						row["ThirdPartyAnalysisSubId"],
						row["ThirdPartyAnalysisSubNo"],
						row["OuterQty"],
						row["OuterUomId"],
						row["OuterUom"],
						row["OuterUomEquivalent"],
						row["InnerQty"],
						row["InnerUomId"],
						row["InnerUom"],
						row["InnerUomEquivalent"],
						row["NetWeight"],
						row["GrossWeight"],
						""
					};
					dtPackingListDetail.Rows.Add(values);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void FillPackingListDetailFromListCommonForReadById(List<ExImInvoicePackingListDetail> ExImInvoicePackingListDetailList, DataTable dt)
	{
		if (ExImInvoicePackingListDetailList == null || ExImInvoicePackingListDetailList.Count == 0)
		{
			return;
		}
		foreach (ExImInvoicePackingListDetail detail in ExImInvoicePackingListDetailList)
		{
			List<object> rowValues = new List<object>
			{
				detail.Id, detail.RefDocumentTypeId, detail.RefDocId, detail.RefDocSubId, detail.ContainerId, detail.ContainerNo, detail.ItemId, detail.ItemCode, detail.ItemName, detail.CommodityDetail,
				detail.PmDetail, detail.PackageDate, detail.ExpiryDate, detail.LotNo, detail.ThirdPartyAnalysisId, detail.ThirdPartyAnalysisNo, detail.ThirdPartyAnalysisSubId, detail.SubLotNo, detail.OuterQty, detail.OuterUomId,
				detail.OuterUom, detail.OuterUomEquivalent, detail.InnerQty, detail.InnerUomId, detail.InnerUom, detail.InnerUomEquivalent, detail.NetWeight, detail.GrossWeight, detail.Remarks
			};
			dt.Rows.Add(rowValues.ToArray());
		}
	}

	private void BtnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("RecId not found");
			}
			if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ExImInvoice.PackingListDeleteByInvoiceId(Conversion.ToInt(cmbInvoiceNo.Value), UserAccount.ID);
				MessageBox.Show("Delete Record Successfully");
				FormReset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FormReset()
	{
		try
		{
			btnsave.Visible = true;
			btnupdate.Visible = false;
			BtnDelete.Visible = false;
			RecId = 0;
			LstRemoveRecordDetail.Clear();
			((Control)(object)cmbInvoiceNo).Text = string.Empty;
			txtdocno.Text = string.Empty;
			txtDocumentaryCreditNo.Text = string.Empty;
			txtCustomer.Text = string.Empty;
			txtConsignee.Text = string.Empty;
			txtCustomerContractsNos.Text = string.Empty;
			txtCustomerContractsDates.Text = string.Empty;
			txtNotifyParty1.Text = string.Empty;
			txtNotifyParty2.Text = string.Empty;
			txtNotifyParty3.Text = string.Empty;
			txtImporterBank.Text = string.Empty;
			txtExporterBank.Text = string.Empty;
			txtLotRefHeader.Text = string.Empty;
			txtGdNo.Text = string.Empty;
			txtLoadingPort.Text = string.Empty;
			txtDestinationPort.Text = string.Empty;
			txtCareierType.Text = "";
			txtDeliveryTerm.Text = string.Empty;
			txtDeliveryRemarks.Text = string.Empty;
			txtTcpRegNo.Text = string.Empty;
			txtContinent.Text = string.Empty;
			txtOriginCountry.Text = string.Empty;
			txtImporterCountry.Text = string.Empty;
			txtPlaceOfDelivery.Text = string.Empty;
			CmbFcyCode.Text = string.Empty;
			txtExRate.Text = string.Empty;
			txtFcyAmount.Text = string.Empty;
			txtLocalAmt.Text = string.Empty;
			txtaddlessamount.Text = string.Empty;
			txtaddlesscommnets.Text = string.Empty;
			txtTotalNetAmount.Text = string.Empty;
			txtGrossWeight.Text = string.Empty;
			txtNetWeight.Text = string.Empty;
			txtCertificate1.Text = string.Empty;
			txtCertificateOfOrigion.Text = string.Empty;
			txtRemarks1.Text = string.Empty;
			txtRemarks2.Text = string.Empty;
			txtOtherRemarks1.Text = string.Empty;
			txtOtherRemarks2.Text = string.Empty;
			dtPackingListDetail.Rows.Clear();
			dtBrandForPMItem.Clear();
			((UltraGridBase)CmbItemFromInvDetailPackListDetail).DataSource = null;
			ResetPackListDetail();
			BindGrids(grdPackListDetail, dtPackingListDetail);
			((Control)(object)cmbInvoiceNo).Enabled = true;
			InvoiceNoBind(ExImInvoice.ExImInvoice_GetInvoicesRefferedInDo(UserAccount.OrganizationId, UserAccount.CompanyId, clsGlobalVariables.ActiveYr.Id));
			txtDocDate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetPackListDetail()
	{
		try
		{
			updateDetailIndexPackList = -1;
			((Control)(object)CmbContainerNoPackListDetail).Text = string.Empty;
			((Control)(object)CmbItemFromInvDetailPackListDetail).Text = string.Empty;
			((Control)(object)CmbCommodityByItemCusPackListDetail).Text = string.Empty;
			((UltraGridBase)CmbCommodityByItemCusPackListDetail).DataSource = null;
			txtCommodityPackListDetail.Text = string.Empty;
			((Control)(object)CmbPmDetailByItemCusPackListDetail).Text = string.Empty;
			((UltraGridBase)CmbPmDetailByItemCusPackListDetail).DataSource = null;
			txtPmDetailPackListDetail.Text = string.Empty;
			txtLotNoPackListDetail.Text = string.Empty;
			((Control)(object)CmbInspectionNoPackListDetail).Text = string.Empty;
			((Control)(object)CmbInspectionSubLotNoPackListDetail).Text = string.Empty;
			txtOuterQtyPackListDetail.Text = string.Empty;
			txtNetWeightPackListDetail.Text = string.Empty;
			txtGrossWeightPackListDetail.Text = string.Empty;
			txtRemarksPackListDetail.Text = string.Empty;
			BtnAddPackListDetail.Visible = true;
			BtnUpdatePackListDetail.Visible = false;
			BtnCancelPackListDetail.Visible = false;
			CmbContainerNoPackListDetail.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmExportInvoicePackingList_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnNew_Click(object sender, EventArgs e)
	{
		try
		{
			FormReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		try
		{
			Close();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			GetConfigurationsFromGlobal();
			if (((Control)(object)cmbInvoiceNo).Enabled)
			{
				InvoiceNoBind(ExImInvoice.ExImInvoice_GetInvoicesRefferedInDo(UserAccount.OrganizationId, UserAccount.CompanyId, clsGlobalVariables.ActiveYr.Id));
			}
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
		else
		{
			txtDocDate.Focus();
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
			DataGridHistory.DataSource = null;
			grdPackListHistory.DataSource = null;
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
			HistoryComboBind(HistoryComboDbCall());
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
			HistoryGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HistoryGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters Obj = new ReportsParameters();
			Obj.OrganizationId = UserAccount.OrganizationId;
			Obj.CompanyId = UserAccount.CompanyId;
			Obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			Obj.Ids = DocumentTypeId.ToString();
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					Obj.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					Obj.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					Obj.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					Obj.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					Obj.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					Obj.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					Obj.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					Obj.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			Obj.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			Obj.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			Obj.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			Obj.Id = Conversion.ToInt(CmbInvoiceNoHistory.Value);
			if (rdNotReferred.Checked)
			{
				Obj.ActionId = 2;
			}
			else if (rdReferred.Checked)
			{
				Obj.ActionId = 1;
			}
			else if (rdAll.Checked)
			{
				Obj.ActionId = 0;
			}
			Obj.ApprovedFilter = "All";
			DataTable dtgrd = ExImInvoice.FormHistoryForPackingList(Obj);
			if (dtgrd.Rows.Count > 0)
			{
				if (dtHistoryGrid.Columns.Count == 0)
				{
					dtHistoryGrid.Columns.Add("Id");
					dtHistoryGrid.Columns.Add("InvoiceNo");
					dtHistoryGrid.Columns.Add("DocCode", typeof(int));
					dtHistoryGrid.Columns.Add("DocDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("CustomerName");
					dtHistoryGrid.Columns.Add("NoOfContainers", typeof(double));
					dtHistoryGrid.Columns.Add("GrossWeight", typeof(double));
					dtHistoryGrid.Columns.Add("NetWeight", typeof(double));
					dtHistoryGrid.Columns.Add("FcyAmount", typeof(double));
					dtHistoryGrid.Columns.Add("Add/Less", typeof(double));
					dtHistoryGrid.Columns.Add("TotalAmount", typeof(double));
					dtHistoryGrid.Columns.Add("LoadingPort");
					dtHistoryGrid.Columns.Add("DestinationPort");
					dtHistoryGrid.Columns.Add("EntryUser");
					dtHistoryGrid.Columns.Add("EntryDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("ModifyUser");
					dtHistoryGrid.Columns.Add("ModifyDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("NoOfAttachments");
				}
				dtHistoryGrid.Clear();
				for (int i = 0; i < dtgrd.Rows.Count; i++)
				{
					dtHistoryGrid.Rows.Add(dtgrd.Rows[i]["Id"], dtgrd.Rows[i]["InvoiceNo"], dtgrd.Rows[i]["DocCode"], Conversion.ToDateTime(dtgrd.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dtgrd.Rows[i]["Customer"], dtgrd.Rows[i]["NoOfContainers"], dtgrd.Rows[i]["GrossWeight"], dtgrd.Rows[i]["NetWeight"], dtgrd.Rows[i]["FCurrencyAmount"], dtgrd.Rows[i]["AddLessAmount"], dtgrd.Rows[i]["TotalAmount"], dtgrd.Rows[i]["LoadingPort"], dtgrd.Rows[i]["DestinationPort"], dtgrd.Rows[i]["EntryUserName"], dtgrd.Rows[i]["EntryDate"], dtgrd.Rows[i]["ModifyUserName"], dtgrd.Rows[i]["ModifyDate"], dtgrd.Rows[i]["NoOfAttachments"]);
				}
				DataGridHistory.DataSource = dtHistoryGrid;
				DataGridHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				DataGridHistory.ClearStructure();
				grdPackListHistory.ClearStructure();
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
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(DataGridHistory, 2, 3);
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["InvoiceNo"].Width = Constants.ExportConstants.InvoiceNoConstant;
			DataGridHistory.RootTable.Columns["DocCode"].Width = Constants.ExportConstants.DocNoConstant;
			DataGridHistory.RootTable.Columns["DocDate"].Width = Constants.ExportConstants.DateConstant;
			DataGridHistory.RootTable.Columns["CustomerName"].Width = Constants.ExportConstants.StringConstant;
			DataGridHistory.RootTable.Columns["NoOfContainers"].Width = Constants.ExportConstants.NoOfContainers;
			DataGridHistory.RootTable.Columns["GrossWeight"].Width = Constants.ExportConstants.WeightConstant;
			DataGridHistory.RootTable.Columns["NetWeight"].Width = Constants.ExportConstants.WeightConstant;
			DataGridHistory.RootTable.Columns["FcyAmount"].Width = Constants.ExportConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["Add/Less"].Width = Constants.ExportConstants.AddLessConstant;
			DataGridHistory.RootTable.Columns["TotalAmount"].Width = Constants.ExportConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["LoadingPort"].Width = Constants.ExportConstants.PortConstant;
			DataGridHistory.RootTable.Columns["DestinationPort"].Width = Constants.ExportConstants.PortConstant;
			DataGridHistory.RootTable.Columns["EntryUser"].Width = Constants.ExportConstants.EntryUserConstants;
			DataGridHistory.RootTable.Columns["EntryDate"].Width = Constants.ExportConstants.DateTimeConstantddMMM;
			DataGridHistory.RootTable.Columns["ModifyUser"].Width = Constants.ExportConstants.ModifyUserConstants;
			DataGridHistory.RootTable.Columns["ModifyDate"].Width = Constants.ExportConstants.DateTimeConstantddMMM;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.ExportConstants.NoOfAttachmentsConstants - 12;
			GridEX_Helper.AddButton(DataGridHistory, "AddAttachment", "AddAttachment", 105);
			ctrlDataGridHistory_Load(null, null);
			if (formrights.DoHavePrintRights)
			{
				GridEX_Helper.AddButton(DataGridHistory, "Slip", "Slip", 50, DataGridHistory.RootTable.Columns["InvoiceNo"].Position - 1);
				GridEX_Helper.AddButton(DataGridHistory, "SlipB", "SlipB", 50, DataGridHistory.RootTable.Columns["InvoiceNo"].Position - 1);
				GridEX_Helper.AddButton(DataGridHistory, "SlipC", "SlipC", 50, DataGridHistory.RootTable.Columns["InvoiceNo"].Position - 1);
			}
			if (formrights.DoHaveUpdateRights)
			{
				GridEX_Helper.AddButton(DataGridHistory, "Edit", "Edit", 50, DataGridHistory.RootTable.Columns["InvoiceNo"].Position - 1);
			}
			DataGridHistory.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				ReadById(Conversion.ToInt(r.Cells["Id"].Value), History: true);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), DocumentTypeId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int Id = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "Slip")
				{
					CommonServices.ExportPackingList529A(Id);
				}
				else if (e.Column.Key == "SlipB")
				{
					CommonServices.ExportPackingList529B(Id);
				}
				else if (e.Column.Key == "SlipC")
				{
					CommonServices.ExportPackingList529C(Id);
				}
				else if (e.Column.Key == "Edit")
				{
					ReadById(Id, History: true);
				}
				else if (e.Column.Key == "AddAttachment")
				{
					AttachmentAddingFromHistory attachmentAddingFromHistory = new AttachmentAddingFromHistory(UserAccount, Id, DocumentTypeId, ScreenName);
					attachmentAddingFromHistory.BringToFront();
					attachmentAddingFromHistory.ShowDialog();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				GetDetailGrdByHeadId(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailGrdByHeadId(int Id)
	{
		try
		{
			List<ExImInvoicePackingListDetail> DetailList = ExImInvoice.ExImInvoicePackingListDetail_ReadByInvoiceId(Id);
			if (DetailList == null || DetailList.Count == 0)
			{
				grdPackListHistory.ClearStructure();
				return;
			}
			DataTable dtPmH = dtPackingListDetail.Clone();
			FillPackingListDetailFromListCommonForReadById(DetailList, dtPmH);
			grdPackListHistory.DataSource = dtPmH;
			grdPackListHistory.RetrieveStructure();
			grdSettingsPackListDetaill(grdPackListHistory);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.ExportPackingList529A(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlgrdPackListDetail_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, grdPackListDetail, dtPackingListDetail);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlDataGridHistory_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, DataGridHistory, dtHistoryGrid);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ImProformaInvoice_KeyDown(object sender, KeyEventArgs e)
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
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					txtDocDate.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					FromDateHistory.Focus();
				}
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.S && !UpdateMode)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && UpdateMode)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.P && formrights.DoHavePrintRights)
				{
					btnPrint_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					txtDocDate.Focus();
				}
				return;
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnShowHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnResetHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefreshHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				FromDateHistory.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				if (((Control)(object)DataGridHistory).Focused)
				{
					((Control)(object)grdPackListHistory).Focus();
				}
				else
				{
					((Control)(object)DataGridHistory).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				FromDateHistory.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)DataGridHistory).Focused)
				{
					((Control)(object)grdPackListHistory).Focus();
				}
				else
				{
					((Control)(object)DataGridHistory).Focus();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortCutKey_Click(object sender, EventArgs e)
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
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+L", "For Load All Records");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowRight", "For Moving In Detail Tabs");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Selected Tab Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On First Entry of Detail Selected Tab");
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

	public static void ValidateRowAgainstReference(GridEXRow row, DataTable referenceTable, string matchColumn1, string matchColumn2, Dictionary<string, object> resetColumns)
	{
		if (string.IsNullOrWhiteSpace(matchColumn1))
		{
			return;
		}
		object obj;
		switch (matchColumn1)
		{
		default:
			obj = matchColumn1;
			break;
		case "ItemId":
		case "OuterUomId":
		case "InnerUomId":
			obj = "Id";
			break;
		}
		string refCol1 = (string)obj;
		int val1 = Conversion.ToInt(row.Cells[matchColumn1].Value);
		bool exists = false;
		if (string.IsNullOrWhiteSpace(matchColumn2))
		{
			exists = referenceTable.AsEnumerable().Any((DataRow r) => r[refCol1] != DBNull.Value && Conversion.ToInt(r[refCol1]) == val1);
		}
		else
		{
			string refCol2 = ((matchColumn2 == "ItemId") ? "Id" : matchColumn2);
			int val2 = Conversion.ToInt(row.Cells[matchColumn2].Value);
			exists = referenceTable.AsEnumerable().Any((DataRow r) => r[refCol1] != DBNull.Value && r[refCol2] != DBNull.Value && Conversion.ToInt(r[refCol1]) == val1 && Conversion.ToInt(r[refCol2]) == val2);
		}
		if (exists)
		{
			return;
		}
		foreach (KeyValuePair<string, object> kvp in resetColumns)
		{
			row.Cells[kvp.Key].Value = kvp.Value;
		}
	}

	private void ShowPopupAndSetValue(GridEXRow row, DataTable dataSource, string idField, string displayField, string columnKey, string columnName = null)
	{
		using GrdPopUp popup = new GrdPopUp(dataSource, idField, displayField);
		popup.StartPosition = FormStartPosition.CenterScreen;
		popup.ShowDialog();
		row.Cells[columnKey].Value = Conversion.ToInt((object)popup.ReturnId);
		if (!string.IsNullOrWhiteSpace(columnName))
		{
			row.Cells[columnName].Value = Conversion.ToString((object)popup.ReturnName);
		}
	}

	private void HandleUomSelection(GridEXRow row, string prefix)
	{
		int itemId = Conversion.ToInt(row.Cells["ItemId"].Value);
		if (itemId <= 0)
		{
			MessageBox.Show("Please Select an Item First");
			return;
		}
		DataTable dtUom = CommonServices.dtUomFromGloablUomScheduleByItemId(itemId);
		using GrdPopUp popup = new GrdPopUp(dtUom, "Id", "UomCode");
		popup.StartPosition = FormStartPosition.CenterScreen;
		popup.ShowDialog();
		int uomId = Conversion.ToInt((object)popup.ReturnId);
		string uomName = Conversion.ToString((object)popup.ReturnName);
		row.Cells[prefix + "UomId"].Value = uomId;
		row.Cells[prefix + "Uom"].Value = uomName;
		if (uomId > 0)
		{
			DataRow found = dtUom.AsEnumerable().FirstOrDefault((DataRow r) => r.Field<int>("Id") == uomId);
			double equivalent = ((found != null) ? Conversion.ToDouble(found["Equivalent"]) : 0.0);
			row.Cells[prefix + "Equivalent"].Value = equivalent;
			double qtyMton = Conversion.ToDouble(row.Cells["Qty/M.Ton"].Value);
			double qty = ((equivalent > 0.0) ? (qtyMton * 1000.0 / equivalent) : 0.0);
			row.Cells[prefix + "Qty"].Value = qty;
			if (prefix == "Outer")
			{
				double num = Conversion.ToDouble(row.Cells["NetRate"].Value);
				double rateEq = Conversion.ToDouble(row.Cells["RateEquivalent"].Value);
				double amount = num / rateEq * qtyMton * 1000.0;
				row.Cells["Amount"].Value = amount;
			}
		}
	}

	private void HandleItemSelection(GridEXRow row, DataTable dt, string prefix)
	{
		string ValuName = ((prefix == "PmItem") ? "Item" : prefix);
		using GrdPopUp popup = new GrdPopUp(dt, "Id", ValuName + "Name");
		popup.StartPosition = FormStartPosition.CenterScreen;
		popup.ShowDialog();
		int uomId = Conversion.ToInt((object)popup.ReturnId);
		string uomName = Conversion.ToString((object)popup.ReturnName);
		row.Cells[prefix + "Id"].Value = uomId;
		row.Cells[prefix + "Name"].Value = uomName;
		if (uomId > 0)
		{
			DataRow found = dt.AsEnumerable().FirstOrDefault((DataRow r) => r.Field<int>("Id") == uomId);
			string Code = ((found != null) ? Conversion.ToString(found[ValuName + "Code"]) : "");
			row.Cells[prefix + "Code"].Value = Code;
		}
	}

	private void HandleCommodityOrPackingDetail(GridEXRow row, int detailTypeId, string columnKey)
	{
		int itemId = Conversion.ToInt(row.Cells["ItemId"].Value);
		if (itemId <= 0)
		{
			MessageBox.Show("Please Select an Item First");
			return;
		}
		DataRow[] filteredRows = ItemCommodityDetailDbCall(itemId).Select($"DetailTypeId={detailTypeId}");
		if (filteredRows.Length != 0)
		{
			using (GrdPopUp popup = new GrdPopUp(filteredRows.CopyToDataTable(), "Id", "Remarks"))
			{
				popup.StartPosition = FormStartPosition.CenterScreen;
				popup.ShowDialog();
				row.Cells[columnKey].Value = Conversion.ToString((object)popup.ReturnName);
				return;
			}
		}
		MessageBox.Show("No Record Found");
	}

	private void grdPackListDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
				return;
			}
			GridEX grid = grdPackListDetail;
			GridEXRow r = grid.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			GridEXColumn currentColumn = grid.CurrentColumn;
			string key = ((currentColumn != null) ? currentColumn.Key : null);
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdPackListDetail_DoubleClick(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.Space && grid.CurrentColumn != null && key == "Delete")
			{
				DeletePackListDetailRow(r);
			}
			if (e.KeyCode != Keys.F1)
			{
				return;
			}
			if (!(key == "ItemCommodityDetail"))
			{
				if (key == "ItemPmDetail")
				{
					HandleCommodityOrPackingDetail(r, 2, "ItemPmDetail");
				}
			}
			else
			{
				HandleCommodityOrPackingDetail(r, 1, "ItemCommodityDetail");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || DataGridHistory.CurrentColumn == null)
			{
				return;
			}
			int Id = Conversion.ToInt(r.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (formrights.DoHaveUpdateRights)
				{
					ReadById(Id, History: true);
				}
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (DataGridHistory.CurrentColumn.Key == "Edit" && formrights.DoHaveUpdateRights)
				{
					ReadById(Id, History: true);
				}
				if (DataGridHistory.CurrentColumn.Key == "Slip" && formrights.DoHavePrintRights)
				{
					CommonServices.ExportPackingList529A(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
				}
				if (DataGridHistory.CurrentColumn.Key == "SlipB" && formrights.DoHavePrintRights)
				{
					CommonServices.ExportPackingList529B(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
				}
				if (DataGridHistory.CurrentColumn.Key == "SlipC" && formrights.DoHavePrintRights)
				{
					CommonServices.ExportPackingList529C(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
				}
				if (DataGridHistory.CurrentColumn.Key == "Attachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
				}
				if (DataGridHistory.CurrentColumn.Key == "AddAttachment")
				{
					AttachmentAddingFromHistory attachmentAddingFromHistory = new AttachmentAddingFromHistory(UserAccount, Id, DocumentTypeId, ScreenName);
					attachmentAddingFromHistory.BringToFront();
					attachmentAddingFromHistory.ShowDialog();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint529B_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.ExportPackingList529B(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint529C_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.ExportPackingList529C(RecId);
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
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a8: Expected O, but got Unknown
		//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d4: Expected O, but got Unknown
		//IL_04d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04df: Expected O, but got Unknown
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f5: Expected O, but got Unknown
		//IL_0517: Unknown result type (might be due to invalid IL or missing references)
		//IL_0521: Expected O, but got Unknown
		//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dc: Expected O, but got Unknown
		//IL_05e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Expected O, but got Unknown
		//IL_05f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fd: Expected O, but got Unknown
		//IL_06a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ad: Expected O, but got Unknown
		//IL_0727: Unknown result type (might be due to invalid IL or missing references)
		//IL_0731: Expected O, but got Unknown
		//IL_0cbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc7: Expected O, but got Unknown
		//IL_0cde: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce8: Expected O, but got Unknown
		//IL_0d6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d77: Expected O, but got Unknown
		//IL_0dc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dcf: Expected O, but got Unknown
		//IL_0e54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e5e: Expected O, but got Unknown
		//IL_0e5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e69: Expected O, but got Unknown
		//IL_14f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_14fb: Expected O, but got Unknown
		//IL_ab7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_ab86: Expected O, but got Unknown
		//IL_abaa: Unknown result type (might be due to invalid IL or missing references)
		//IL_abb4: Expected O, but got Unknown
		GridEXLayout grdPackListDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
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
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPackListHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Export.frmExportInvoicePackingList));
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel38 = new System.Windows.Forms.Panel();
		this.grdPackListDetail = new GridEX();
		this.panel25 = new System.Windows.Forms.Panel();
		this.ctrlgrdPackListDetail = new grid.User_Conrtols.CtrlGrdBar();
		this.panelDetail = new System.Windows.Forms.Panel();
		this.CmbItemFromInvDetailPackListDetail = new UltraCombo();
		this.CmbInspectionSubLotNoPackListDetail = new UltraCombo();
		this.label15 = new System.Windows.Forms.Label();
		this.CmbInnerUomInDetail = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.txtQtyInnerInDetail = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.CmbOuterUomInDetail = new UltraCombo();
		this.label30 = new System.Windows.Forms.Label();
		this.RadItemCodePackListDetail = new System.Windows.Forms.RadioButton();
		this.RadItemNamePackListDetail = new System.Windows.Forms.RadioButton();
		this.label37 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label160 = new System.Windows.Forms.Label();
		this.label159 = new System.Windows.Forms.Label();
		this.label158 = new System.Windows.Forms.Label();
		this.BtnAddPackListDetail = new System.Windows.Forms.Button();
		this.txtNetWeightPackListDetail = new System.Windows.Forms.TextBox();
		this.label150 = new System.Windows.Forms.Label();
		this.txtGrossWeightPackListDetail = new System.Windows.Forms.TextBox();
		this.label149 = new System.Windows.Forms.Label();
		this.txtOuterQtyPackListDetail = new System.Windows.Forms.TextBox();
		this.label147 = new System.Windows.Forms.Label();
		this.CmbInspectionNoPackListDetail = new UltraCombo();
		this.label144 = new System.Windows.Forms.Label();
		this.CmbPmDetailByItemCusPackListDetail = new UltraCombo();
		this.CmbCommodityByItemCusPackListDetail = new UltraCombo();
		this.txtPmDetailPackListDetail = new System.Windows.Forms.TextBox();
		this.txtCommodityPackListDetail = new System.Windows.Forms.TextBox();
		this.label135 = new System.Windows.Forms.Label();
		this.label137 = new System.Windows.Forms.Label();
		this.label84 = new System.Windows.Forms.Label();
		this.label138 = new System.Windows.Forms.Label();
		this.label139 = new System.Windows.Forms.Label();
		this.label141 = new System.Windows.Forms.Label();
		this.txtRemarksPackListDetail = new System.Windows.Forms.TextBox();
		this.label157 = new System.Windows.Forms.Label();
		this.label156 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label85 = new System.Windows.Forms.Label();
		this.label145 = new System.Windows.Forms.Label();
		this.CmbContainerNoPackListDetail = new UltraCombo();
		this.txtLotNoPackListDetail = new System.Windows.Forms.TextBox();
		this.label148 = new System.Windows.Forms.Label();
		this.BtnCancelPackListDetail = new System.Windows.Forms.Button();
		this.BtnUpdatePackListDetail = new System.Windows.Forms.Button();
		this.label146 = new System.Windows.Forms.Label();
		this.txtExpiryDatePackListDetail = new System.Windows.Forms.DateTimePicker();
		this.label143 = new System.Windows.Forms.Label();
		this.txtPackageDatePackListDetail = new System.Windows.Forms.DateTimePicker();
		this.label142 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.HeaderGropBox = new System.Windows.Forms.GroupBox();
		this.cmbInvoiceNo = new UltraCombo();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.txtFcyAmountHeaderCustom = new System.Windows.Forms.TextBox();
		this.txtAddLessHeaderCustom = new System.Windows.Forms.TextBox();
		this.txtLocalAmountHeaderCustom = new System.Windows.Forms.TextBox();
		this.txtTotalFcyAmountHeaderCustom = new System.Windows.Forms.TextBox();
		this.label112 = new System.Windows.Forms.Label();
		this.label119 = new System.Windows.Forms.Label();
		this.label121 = new System.Windows.Forms.Label();
		this.label122 = new System.Windows.Forms.Label();
		this.txtaddlesscommnets = new System.Windows.Forms.TextBox();
		this.txtLocalAmt = new System.Windows.Forms.TextBox();
		this.txtNoOfContainer = new System.Windows.Forms.TextBox();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.txtaddlessamount = new System.Windows.Forms.TextBox();
		this.txtNetWeight = new System.Windows.Forms.TextBox();
		this.txtTotalNetAmount = new System.Windows.Forms.TextBox();
		this.txtFcyAmount = new System.Windows.Forms.TextBox();
		this.CmbFcyCode = new System.Windows.Forms.TextBox();
		this.txtPlaceOfDelivery = new System.Windows.Forms.TextBox();
		this.txtExRate = new System.Windows.Forms.TextBox();
		this.label43 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label60 = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label66 = new System.Windows.Forms.Label();
		this.label64 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.txtLotRefHeader = new System.Windows.Forms.TextBox();
		this.txtGdDate = new System.Windows.Forms.DateTimePicker();
		this.label116 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.txtCustomerContractsNos = new System.Windows.Forms.TextBox();
		this.label108 = new System.Windows.Forms.Label();
		this.txtExporterBank = new System.Windows.Forms.TextBox();
		this.txtImporterBank = new System.Windows.Forms.TextBox();
		this.txtNotifyParty3 = new System.Windows.Forms.TextBox();
		this.txtNotifyParty2 = new System.Windows.Forms.TextBox();
		this.txtNotifyParty1 = new System.Windows.Forms.TextBox();
		this.txtCustomerContractsDates = new System.Windows.Forms.TextBox();
		this.txtLoadingPort = new System.Windows.Forms.TextBox();
		this.txtDeliveryRemarks = new System.Windows.Forms.TextBox();
		this.label87 = new System.Windows.Forms.Label();
		this.label120 = new System.Windows.Forms.Label();
		this.label114 = new System.Windows.Forms.Label();
		this.label86 = new System.Windows.Forms.Label();
		this.txtDeliveryTerm = new System.Windows.Forms.TextBox();
		this.txtImporterCountry = new System.Windows.Forms.TextBox();
		this.txtCareierType = new System.Windows.Forms.TextBox();
		this.txtOriginCountry = new System.Windows.Forms.TextBox();
		this.txtDestinationPort = new System.Windows.Forms.TextBox();
		this.txtContinent = new System.Windows.Forms.TextBox();
		this.txtTcpRegNo = new System.Windows.Forms.TextBox();
		this.txtConsignee = new System.Windows.Forms.TextBox();
		this.txtCustomer = new System.Windows.Forms.TextBox();
		this.txtDocumentaryCreditNo = new System.Windows.Forms.TextBox();
		this.label31 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label113 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label107 = new System.Windows.Forms.Label();
		this.txtRemarks2 = new System.Windows.Forms.TextBox();
		this.txtRemarks1 = new System.Windows.Forms.TextBox();
		this.label118 = new System.Windows.Forms.Label();
		this.txtCertificate1 = new System.Windows.Forms.TextBox();
		this.label117 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.label55 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.txtCertificateOfOrigion = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.label67 = new System.Windows.Forms.Label();
		this.label95 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label109 = new System.Windows.Forms.Label();
		this.txtGdNo = new System.Windows.Forms.TextBox();
		this.label129 = new System.Windows.Forms.Label();
		this.label115 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label110 = new System.Windows.Forms.Label();
		this.label68 = new System.Windows.Forms.Label();
		this.txtDocumentaryCreditNoDate = new System.Windows.Forms.DateTimePicker();
		this.label16 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label111 = new System.Windows.Forms.Label();
		this.txtDocDate = new System.Windows.Forms.DateTimePicker();
		this.label5 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.label56 = new System.Windows.Forms.Label();
		this.panel17 = new System.Windows.Forms.Panel();
		this.txtOtherRemarks2 = new System.Windows.Forms.TextBox();
		this.label155 = new System.Windows.Forms.Label();
		this.txtOtherRemarks1 = new System.Windows.Forms.TextBox();
		this.label154 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.labelExportCompany = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.ChkPrintPreview = new System.Windows.Forms.CheckBox();
		this.CmbExportCompany = new System.Windows.Forms.TextBox();
		this.panel12 = new System.Windows.Forms.Panel();
		this.toolStrip4 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.BtnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.DataGridHistory = new GridEX();
		this.panel32 = new System.Windows.Forms.Panel();
		this.label34 = new System.Windows.Forms.Label();
		this.grdPackListHistory = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label92 = new System.Windows.Forms.Label();
		this.ctrlDataGridHistory = new grid.User_Conrtols.CtrlGrdBar();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.rdAll = new System.Windows.Forms.RadioButton();
		this.rdReferred = new System.Windows.Forms.RadioButton();
		this.rdNotReferred = new System.Windows.Forms.RadioButton();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.label48 = new System.Windows.Forms.Label();
		this.CmbInvoiceNoHistory = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
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
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label69 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.panel1 = new System.Windows.Forms.Panel();
		this.cmbFCurrencr = new UltraCombo();
		this.cmbEFormNo = new UltraCombo();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
		this.helpToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.btnPrint529B = new System.Windows.Forms.ToolStripButton();
		this.btnPrint529C = new System.Windows.Forms.ToolStripButton();
		this.ChkSlipB = new System.Windows.Forms.CheckBox();
		this.chkSlipC = new System.Windows.Forms.CheckBox();
		this.panel4.SuspendLayout();
		this.panel7.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel38.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPackListDetail).BeginInit();
		this.panel25.SuspendLayout();
		this.panelDetail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemFromInvDetailPackListDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInspectionSubLotNoPackListDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInnerUomInDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOuterUomInDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInspectionNoPackListDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPmDetailByItemCusPackListDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommodityByItemCusPackListDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContainerNoPackListDetail).BeginInit();
		this.panel11.SuspendLayout();
		this.HeaderGropBox.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbInvoiceNo).BeginInit();
		this.groupBox3.SuspendLayout();
		this.panel17.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel12.SuspendLayout();
		this.toolStrip4.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel32.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPackListHistory).BeginInit();
		this.panel15.SuspendLayout();
		this.panel3.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNoHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel16.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbFCurrencr).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEFormNo).BeginInit();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel4.Controls.Add(this.panel7);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1189, 761);
		this.panel4.TabIndex = 3;
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.tabControl1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1189, 761);
		this.panel7.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1189, 761);
		this.tabControl1.TabIndex = 2;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel38);
		this.tabPage1.Controls.Add(this.panel11);
		this.tabPage1.Controls.Add(this.panel17);
		this.tabPage1.Controls.Add(this.panel10);
		this.tabPage1.Controls.Add(this.panel12);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1181, 732);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel38.Controls.Add((System.Windows.Forms.Control)(object)this.grdPackListDetail);
		this.panel38.Controls.Add(this.panel25);
		this.panel38.Controls.Add(this.panelDetail);
		this.panel38.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel38.Location = new System.Drawing.Point(3, 425);
		this.panel38.Name = "panel38";
		this.panel38.Size = new System.Drawing.Size(1175, 252);
		this.panel38.TabIndex = 5;
		this.grdPackListDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPackListDetail).BackColor = System.Drawing.Color.White;
		grdPackListDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPackListDetail.DesignTimeLayout = grdPackListDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPackListDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPackListDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPackListDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPackListDetail).Location = new System.Drawing.Point(0, 122);
		((System.Windows.Forms.Control)(object)this.grdPackListDetail).Name = "grdPackListDetail";
		((ScrollableBase)this.grdPackListDetail).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdPackListDetail).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdPackListDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPackListDetail).Size = new System.Drawing.Size(1175, 130);
		((System.Windows.Forms.Control)(object)this.grdPackListDetail).TabIndex = 1;
		this.grdPackListDetail.TotalRow = (InheritableBoolean)1;
		this.grdPackListDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPackListDetail).VisualStyle = (VisualStyle)4;
		this.grdPackListDetail.ColumnButtonClick += new ColumnActionEventHandler(grdPackListDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPackListDetail).DoubleClick += new System.EventHandler(grdPackListDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdPackListDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPackListDetail_KeyDown);
		this.panel25.BackColor = System.Drawing.Color.Teal;
		this.panel25.Controls.Add(this.ctrlgrdPackListDetail);
		this.panel25.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel25.Location = new System.Drawing.Point(0, 99);
		this.panel25.Name = "panel25";
		this.panel25.Size = new System.Drawing.Size(1175, 23);
		this.panel25.TabIndex = 2;
		this.ctrlgrdPackListDetail.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlgrdPackListDetail.FormName = this;
		this.ctrlgrdPackListDetail.Location = new System.Drawing.Point(1133, 1);
		this.ctrlgrdPackListDetail.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlgrdPackListDetail.MyGrid = this.grdPackListDetail;
		this.ctrlgrdPackListDetail.Name = "ctrlgrdPackListDetail";
		this.ctrlgrdPackListDetail.Size = new System.Drawing.Size(39, 22);
		this.ctrlgrdPackListDetail.TabIndex = 27;
		this.ctrlgrdPackListDetail.Load += new System.EventHandler(ctrlgrdPackListDetail_Load);
		this.panelDetail.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemFromInvDetailPackListDetail);
		this.panelDetail.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInspectionSubLotNoPackListDetail);
		this.panelDetail.Controls.Add(this.label15);
		this.panelDetail.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInnerUomInDetail);
		this.panelDetail.Controls.Add(this.label11);
		this.panelDetail.Controls.Add(this.txtQtyInnerInDetail);
		this.panelDetail.Controls.Add(this.label14);
		this.panelDetail.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOuterUomInDetail);
		this.panelDetail.Controls.Add(this.label30);
		this.panelDetail.Controls.Add(this.RadItemCodePackListDetail);
		this.panelDetail.Controls.Add(this.RadItemNamePackListDetail);
		this.panelDetail.Controls.Add(this.label37);
		this.panelDetail.Controls.Add(this.label33);
		this.panelDetail.Controls.Add(this.label32);
		this.panelDetail.Controls.Add(this.label160);
		this.panelDetail.Controls.Add(this.label159);
		this.panelDetail.Controls.Add(this.label158);
		this.panelDetail.Controls.Add(this.BtnAddPackListDetail);
		this.panelDetail.Controls.Add(this.txtNetWeightPackListDetail);
		this.panelDetail.Controls.Add(this.label150);
		this.panelDetail.Controls.Add(this.txtGrossWeightPackListDetail);
		this.panelDetail.Controls.Add(this.label149);
		this.panelDetail.Controls.Add(this.txtOuterQtyPackListDetail);
		this.panelDetail.Controls.Add(this.label147);
		this.panelDetail.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInspectionNoPackListDetail);
		this.panelDetail.Controls.Add(this.label144);
		this.panelDetail.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPmDetailByItemCusPackListDetail);
		this.panelDetail.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommodityByItemCusPackListDetail);
		this.panelDetail.Controls.Add(this.txtPmDetailPackListDetail);
		this.panelDetail.Controls.Add(this.txtCommodityPackListDetail);
		this.panelDetail.Controls.Add(this.label135);
		this.panelDetail.Controls.Add(this.label137);
		this.panelDetail.Controls.Add(this.label84);
		this.panelDetail.Controls.Add(this.label138);
		this.panelDetail.Controls.Add(this.label139);
		this.panelDetail.Controls.Add(this.label141);
		this.panelDetail.Controls.Add(this.txtRemarksPackListDetail);
		this.panelDetail.Controls.Add(this.label157);
		this.panelDetail.Controls.Add(this.label156);
		this.panelDetail.Controls.Add(this.label29);
		this.panelDetail.Controls.Add(this.label18);
		this.panelDetail.Controls.Add(this.label85);
		this.panelDetail.Controls.Add(this.label145);
		this.panelDetail.Controls.Add((System.Windows.Forms.Control)(object)this.CmbContainerNoPackListDetail);
		this.panelDetail.Controls.Add(this.txtLotNoPackListDetail);
		this.panelDetail.Controls.Add(this.label148);
		this.panelDetail.Controls.Add(this.BtnCancelPackListDetail);
		this.panelDetail.Controls.Add(this.BtnUpdatePackListDetail);
		this.panelDetail.Controls.Add(this.label146);
		this.panelDetail.Controls.Add(this.txtExpiryDatePackListDetail);
		this.panelDetail.Controls.Add(this.label143);
		this.panelDetail.Controls.Add(this.txtPackageDatePackListDetail);
		this.panelDetail.Controls.Add(this.label142);
		this.panelDetail.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelDetail.Location = new System.Drawing.Point(0, 0);
		this.panelDetail.Name = "panelDetail";
		this.panelDetail.Size = new System.Drawing.Size(1175, 99);
		this.panelDetail.TabIndex = 0;
		this.panelDetail.Visible = false;
		this.CmbItemFromInvDetailPackListDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbItemFromInvDetailPackListDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemFromInvDetailPackListDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemFromInvDetailPackListDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance154;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance12).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemFromInvDetailPackListDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemFromInvDetailPackListDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemFromInvDetailPackListDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemFromInvDetailPackListDetail).Location = new System.Drawing.Point(136, 21);
		((System.Windows.Forms.Control)(object)this.CmbItemFromInvDetailPackListDetail).Name = "CmbItemFromInvDetailPackListDetail";
		((System.Windows.Forms.Control)(object)this.CmbItemFromInvDetailPackListDetail).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemFromInvDetailPackListDetail).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbItemFromInvDetailPackListDetail).Leave += new System.EventHandler(CmbItemFromInvDetailPackListDetail_Leave);
		this.CmbInspectionSubLotNoPackListDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbInspectionSubLotNoPackListDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInspectionSubLotNoPackListDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInspectionSubLotNoPackListDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInspectionSubLotNoPackListDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbInspectionSubLotNoPackListDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInspectionSubLotNoPackListDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInspectionSubLotNoPackListDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInspectionSubLotNoPackListDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance19;
		((SpecialBoxBase)((UltraGridBase)this.CmbInspectionSubLotNoPackListDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInspectionSubLotNoPackListDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbInspectionSubLotNoPackListDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInspectionSubLotNoPackListDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInspectionSubLotNoPackListDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInspectionSubLotNoPackListDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbInspectionSubLotNoPackListDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInspectionSubLotNoPackListDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInspectionSubLotNoPackListDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance24).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInspectionSubLotNoPackListDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbInspectionSubLotNoPackListDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInspectionSubLotNoPackListDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance25).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInspectionSubLotNoPackListDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInspectionSubLotNoPackListDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbInspectionSubLotNoPackListDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInspectionSubLotNoPackListDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInspectionSubLotNoPackListDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbInspectionSubLotNoPackListDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInspectionSubLotNoPackListDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbInspectionSubLotNoPackListDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInspectionSubLotNoPackListDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInspectionSubLotNoPackListDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInspectionSubLotNoPackListDetail).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbInspectionSubLotNoPackListDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInspectionSubLotNoPackListDetail).Location = new System.Drawing.Point(317, 67);
		((System.Windows.Forms.Control)(object)this.CmbInspectionSubLotNoPackListDetail).Name = "CmbInspectionSubLotNoPackListDetail";
		((System.Windows.Forms.Control)(object)this.CmbInspectionSubLotNoPackListDetail).Size = new System.Drawing.Size(110, 26);
		((System.Windows.Forms.Control)(object)this.CmbInspectionSubLotNoPackListDetail).TabIndex = 632;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label15.Location = new System.Drawing.Point(317, 51);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(107, 15);
		this.label15.TabIndex = 633;
		this.label15.Text = "Inspection Sub Lot";
		this.CmbInnerUomInDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbInnerUomInDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInnerUomInDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInnerUomInDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance82;
		((SpecialBoxBase)((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance100).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance101).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance101).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance101).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInnerUomInDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInnerUomInDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInnerUomInDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInnerUomInDetail).Location = new System.Drawing.Point(666, 67);
		((System.Windows.Forms.Control)(object)this.CmbInnerUomInDetail).Name = "CmbInnerUomInDetail";
		((System.Windows.Forms.Control)(object)this.CmbInnerUomInDetail).Size = new System.Drawing.Size(85, 26);
		((System.Windows.Forms.Control)(object)this.CmbInnerUomInDetail).TabIndex = 626;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(666, 51);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(66, 15);
		this.label11.TabIndex = 631;
		this.label11.Text = "Inner UOM";
		this.txtQtyInnerInDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQtyInnerInDetail.Location = new System.Drawing.Point(590, 69);
		this.txtQtyInnerInDetail.Name = "txtQtyInnerInDetail";
		this.txtQtyInnerInDetail.Size = new System.Drawing.Size(75, 23);
		this.txtQtyInnerInDetail.TabIndex = 627;
		this.txtQtyInnerInDetail.Tag = "NoofBags";
		this.txtQtyInnerInDetail.Text = "0";
		this.txtQtyInnerInDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(590, 51);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(57, 15);
		this.label14.TabIndex = 629;
		this.label14.Text = "Qty Inner";
		this.CmbOuterUomInDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbOuterUomInDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOuterUomInDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOuterUomInDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance88;
		((SpecialBoxBase)((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance93).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance95).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOuterUomInDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOuterUomInDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbOuterUomInDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOuterUomInDetail).Location = new System.Drawing.Point(504, 67);
		((System.Windows.Forms.Control)(object)this.CmbOuterUomInDetail).Name = "CmbOuterUomInDetail";
		((System.Windows.Forms.Control)(object)this.CmbOuterUomInDetail).Size = new System.Drawing.Size(85, 26);
		((System.Windows.Forms.Control)(object)this.CmbOuterUomInDetail).TabIndex = 624;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(506, 51);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(68, 15);
		this.label30.TabIndex = 628;
		this.label30.Text = "Outer UOM";
		this.RadItemCodePackListDetail.AutoSize = true;
		this.RadItemCodePackListDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadItemCodePackListDetail.Location = new System.Drawing.Point(225, 3);
		this.RadItemCodePackListDetail.Name = "RadItemCodePackListDetail";
		this.RadItemCodePackListDetail.Size = new System.Drawing.Size(52, 19);
		this.RadItemCodePackListDetail.TabIndex = 623;
		this.RadItemCodePackListDetail.Text = "Code";
		this.RadItemCodePackListDetail.UseVisualStyleBackColor = true;
		this.RadItemNamePackListDetail.AutoSize = true;
		this.RadItemNamePackListDetail.Checked = true;
		this.RadItemNamePackListDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadItemNamePackListDetail.Location = new System.Drawing.Point(169, 3);
		this.RadItemNamePackListDetail.Name = "RadItemNamePackListDetail";
		this.RadItemNamePackListDetail.Size = new System.Drawing.Size(57, 19);
		this.RadItemNamePackListDetail.TabIndex = 622;
		this.RadItemNamePackListDetail.TabStop = true;
		this.RadItemNamePackListDetail.Text = "Name";
		this.RadItemNamePackListDetail.UseVisualStyleBackColor = true;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.ForeColor = System.Drawing.Color.Red;
		this.label37.Location = new System.Drawing.Point(574, 50);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(14, 17);
		this.label37.TabIndex = 621;
		this.label37.Text = "*";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.ForeColor = System.Drawing.Color.Red;
		this.label33.Location = new System.Drawing.Point(645, 50);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(14, 17);
		this.label33.TabIndex = 621;
		this.label33.Text = "*";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.Red;
		this.label32.Location = new System.Drawing.Point(732, 50);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(14, 17);
		this.label32.TabIndex = 621;
		this.label32.Text = "*";
		this.label160.AutoSize = true;
		this.label160.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label160.ForeColor = System.Drawing.Color.Red;
		this.label160.Location = new System.Drawing.Point(932, 50);
		this.label160.Name = "label160";
		this.label160.Size = new System.Drawing.Size(14, 17);
		this.label160.TabIndex = 621;
		this.label160.Text = "*";
		this.label159.AutoSize = true;
		this.label159.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label159.ForeColor = System.Drawing.Color.Red;
		this.label159.Location = new System.Drawing.Point(835, 50);
		this.label159.Name = "label159";
		this.label159.Size = new System.Drawing.Size(14, 17);
		this.label159.TabIndex = 620;
		this.label159.Text = "*";
		this.label158.AutoSize = true;
		this.label158.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label158.ForeColor = System.Drawing.Color.Red;
		this.label158.Location = new System.Drawing.Point(485, 50);
		this.label158.Name = "label158";
		this.label158.Size = new System.Drawing.Size(14, 17);
		this.label158.TabIndex = 589;
		this.label158.Text = "*";
		this.BtnAddPackListDetail.BackColor = System.Drawing.Color.Teal;
		this.BtnAddPackListDetail.FlatAppearance.BorderSize = 0;
		this.BtnAddPackListDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAddPackListDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnAddPackListDetail.ForeColor = System.Drawing.Color.White;
		this.BtnAddPackListDetail.Location = new System.Drawing.Point(1107, 70);
		this.BtnAddPackListDetail.Name = "BtnAddPackListDetail";
		this.BtnAddPackListDetail.Size = new System.Drawing.Size(31, 21);
		this.BtnAddPackListDetail.TabIndex = 15;
		this.BtnAddPackListDetail.Text = "+";
		this.BtnAddPackListDetail.UseVisualStyleBackColor = false;
		this.BtnAddPackListDetail.Click += new System.EventHandler(BtnAddPackListDetail_Click);
		this.txtNetWeightPackListDetail.BackColor = System.Drawing.Color.White;
		this.txtNetWeightPackListDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeightPackListDetail.Location = new System.Drawing.Point(752, 69);
		this.txtNetWeightPackListDetail.Name = "txtNetWeightPackListDetail";
		this.txtNetWeightPackListDetail.Size = new System.Drawing.Size(95, 22);
		this.txtNetWeightPackListDetail.TabIndex = 12;
		this.txtNetWeightPackListDetail.Text = "0";
		this.txtNetWeightPackListDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label150.AutoSize = true;
		this.label150.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label150.Location = new System.Drawing.Point(752, 51);
		this.label150.Name = "label150";
		this.label150.Size = new System.Drawing.Size(68, 15);
		this.label150.TabIndex = 617;
		this.label150.Text = "Net Weight";
		this.txtGrossWeightPackListDetail.BackColor = System.Drawing.Color.White;
		this.txtGrossWeightPackListDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrossWeightPackListDetail.Location = new System.Drawing.Point(848, 69);
		this.txtGrossWeightPackListDetail.Name = "txtGrossWeightPackListDetail";
		this.txtGrossWeightPackListDetail.Size = new System.Drawing.Size(100, 22);
		this.txtGrossWeightPackListDetail.TabIndex = 13;
		this.txtGrossWeightPackListDetail.Text = "0";
		this.txtGrossWeightPackListDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label149.AutoSize = true;
		this.label149.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label149.Location = new System.Drawing.Point(848, 51);
		this.label149.Name = "label149";
		this.label149.Size = new System.Drawing.Size(78, 15);
		this.label149.TabIndex = 617;
		this.label149.Text = "Gross Weight";
		this.txtOuterQtyPackListDetail.BackColor = System.Drawing.Color.White;
		this.txtOuterQtyPackListDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOuterQtyPackListDetail.Location = new System.Drawing.Point(428, 69);
		this.txtOuterQtyPackListDetail.Name = "txtOuterQtyPackListDetail";
		this.txtOuterQtyPackListDetail.Size = new System.Drawing.Size(75, 22);
		this.txtOuterQtyPackListDetail.TabIndex = 11;
		this.txtOuterQtyPackListDetail.Text = "0";
		this.txtOuterQtyPackListDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label147.AutoSize = true;
		this.label147.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label147.Location = new System.Drawing.Point(428, 51);
		this.label147.Name = "label147";
		this.label147.Size = new System.Drawing.Size(59, 15);
		this.label147.TabIndex = 617;
		this.label147.Text = "Outer Qty";
		this.CmbInspectionNoPackListDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbInspectionNoPackListDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInspectionNoPackListDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInspectionNoPackListDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance63).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance74).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInspectionNoPackListDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInspectionNoPackListDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInspectionNoPackListDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInspectionNoPackListDetail).Location = new System.Drawing.Point(216, 67);
		((System.Windows.Forms.Control)(object)this.CmbInspectionNoPackListDetail).Name = "CmbInspectionNoPackListDetail";
		((System.Windows.Forms.Control)(object)this.CmbInspectionNoPackListDetail).Size = new System.Drawing.Size(100, 26);
		((System.Windows.Forms.Control)(object)this.CmbInspectionNoPackListDetail).TabIndex = 9;
		this.label144.AutoSize = true;
		this.label144.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label144.Location = new System.Drawing.Point(216, 51);
		this.label144.Name = "label144";
		this.label144.Size = new System.Drawing.Size(82, 15);
		this.label144.TabIndex = 615;
		this.label144.Text = "Inspection No";
		this.CmbPmDetailByItemCusPackListDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbPmDetailByItemCusPackListDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPmDetailByItemCusPackListDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPmDetailByItemCusPackListDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance60;
		((SpecialBoxBase)((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance155).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance156).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance156).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance156).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPmDetailByItemCusPackListDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPmDetailByItemCusPackListDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPmDetailByItemCusPackListDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPmDetailByItemCusPackListDetail).Location = new System.Drawing.Point(689, 21);
		((System.Windows.Forms.Control)(object)this.CmbPmDetailByItemCusPackListDetail).Name = "CmbPmDetailByItemCusPackListDetail";
		((System.Windows.Forms.Control)(object)this.CmbPmDetailByItemCusPackListDetail).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.CmbPmDetailByItemCusPackListDetail).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbPmDetailByItemCusPackListDetail).Leave += new System.EventHandler(CmbPmDetailByItemCusPackListDetail_Leave);
		this.CmbCommodityByItemCusPackListDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbCommodityByItemCusPackListDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommodityByItemCusPackListDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommodityByItemCusPackListDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance31;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance36).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance37).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommodityByItemCusPackListDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommodityByItemCusPackListDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCommodityByItemCusPackListDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommodityByItemCusPackListDetail).Location = new System.Drawing.Point(317, 21);
		((System.Windows.Forms.Control)(object)this.CmbCommodityByItemCusPackListDetail).Name = "CmbCommodityByItemCusPackListDetail";
		((System.Windows.Forms.Control)(object)this.CmbCommodityByItemCusPackListDetail).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommodityByItemCusPackListDetail).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbCommodityByItemCusPackListDetail).Leave += new System.EventHandler(CmbCommodityByItemCusPackListDetail_Leave);
		this.txtPmDetailPackListDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPmDetailPackListDetail.Location = new System.Drawing.Point(870, 23);
		this.txtPmDetailPackListDetail.Name = "txtPmDetailPackListDetail";
		this.txtPmDetailPackListDetail.Size = new System.Drawing.Size(186, 23);
		this.txtPmDetailPackListDetail.TabIndex = 5;
		this.txtCommodityPackListDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommodityPackListDetail.Location = new System.Drawing.Point(498, 23);
		this.txtCommodityPackListDetail.Name = "txtCommodityPackListDetail";
		this.txtCommodityPackListDetail.Size = new System.Drawing.Size(190, 23);
		this.txtCommodityPackListDetail.TabIndex = 3;
		this.label135.AutoSize = true;
		this.label135.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label135.Location = new System.Drawing.Point(870, 5);
		this.label135.Name = "label135";
		this.label135.Size = new System.Drawing.Size(59, 15);
		this.label135.TabIndex = 600;
		this.label135.Text = "Pm Detail";
		this.label137.AutoSize = true;
		this.label137.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label137.Location = new System.Drawing.Point(498, 5);
		this.label137.Name = "label137";
		this.label137.Size = new System.Drawing.Size(104, 15);
		this.label137.TabIndex = 601;
		this.label137.Text = "Commodity Detail";
		this.label84.AutoSize = true;
		this.label84.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label84.Location = new System.Drawing.Point(136, 5);
		this.label84.Name = "label84";
		this.label84.Size = new System.Drawing.Size(32, 15);
		this.label84.TabIndex = 599;
		this.label84.Text = "Item";
		this.label138.AutoSize = true;
		this.label138.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label138.Location = new System.Drawing.Point(689, 5);
		this.label138.Name = "label138";
		this.label138.Size = new System.Drawing.Size(103, 15);
		this.label138.TabIndex = 598;
		this.label138.Text = "Pm Detail By Item";
		this.label139.AutoSize = true;
		this.label139.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label139.Location = new System.Drawing.Point(317, 5);
		this.label139.Name = "label139";
		this.label139.Size = new System.Drawing.Size(144, 15);
		this.label139.TabIndex = 599;
		this.label139.Text = "Comoodity Detail By Item";
		this.label141.AutoSize = true;
		this.label141.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label141.Location = new System.Drawing.Point(949, 51);
		this.label141.Name = "label141";
		this.label141.Size = new System.Drawing.Size(52, 15);
		this.label141.TabIndex = 593;
		this.label141.Text = "Remarks";
		this.txtRemarksPackListDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksPackListDetail.Location = new System.Drawing.Point(949, 69);
		this.txtRemarksPackListDetail.Name = "txtRemarksPackListDetail";
		this.txtRemarksPackListDetail.Size = new System.Drawing.Size(155, 22);
		this.txtRemarksPackListDetail.TabIndex = 14;
		this.label157.AutoSize = true;
		this.label157.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label157.ForeColor = System.Drawing.Color.Red;
		this.label157.Location = new System.Drawing.Point(75, 50);
		this.label157.Name = "label157";
		this.label157.Size = new System.Drawing.Size(14, 17);
		this.label157.TabIndex = 589;
		this.label157.Text = "*";
		this.label156.AutoSize = true;
		this.label156.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label156.ForeColor = System.Drawing.Color.Red;
		this.label156.Location = new System.Drawing.Point(1139, 4);
		this.label156.Name = "label156";
		this.label156.Size = new System.Drawing.Size(14, 17);
		this.label156.TabIndex = 589;
		this.label156.Text = "*";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.ForeColor = System.Drawing.Color.Red;
		this.label29.Location = new System.Drawing.Point(935, 4);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(14, 17);
		this.label29.TabIndex = 589;
		this.label29.Text = "*";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.Color.Red;
		this.label18.Location = new System.Drawing.Point(613, 4);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(14, 17);
		this.label18.TabIndex = 589;
		this.label18.Text = "*";
		this.label85.AutoSize = true;
		this.label85.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label85.ForeColor = System.Drawing.Color.Red;
		this.label85.Location = new System.Drawing.Point(275, 4);
		this.label85.Name = "label85";
		this.label85.Size = new System.Drawing.Size(14, 17);
		this.label85.TabIndex = 589;
		this.label85.Text = "*";
		this.label145.AutoSize = true;
		this.label145.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label145.ForeColor = System.Drawing.Color.Red;
		this.label145.Location = new System.Drawing.Point(82, 4);
		this.label145.Name = "label145";
		this.label145.Size = new System.Drawing.Size(14, 17);
		this.label145.TabIndex = 589;
		this.label145.Text = "*";
		this.CmbContainerNoPackListDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbContainerNoPackListDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbContainerNoPackListDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbContainerNoPackListDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance43;
		((SpecialBoxBase)((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance49).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance50).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbContainerNoPackListDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbContainerNoPackListDetail).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbContainerNoPackListDetail).Location = new System.Drawing.Point(5, 22);
		((System.Windows.Forms.Control)(object)this.CmbContainerNoPackListDetail).Name = "CmbContainerNoPackListDetail";
		((System.Windows.Forms.Control)(object)this.CmbContainerNoPackListDetail).Size = new System.Drawing.Size(130, 25);
		((System.Windows.Forms.Control)(object)this.CmbContainerNoPackListDetail).TabIndex = 0;
		this.txtLotNoPackListDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLotNoPackListDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLotNoPackListDetail.Location = new System.Drawing.Point(115, 69);
		this.txtLotNoPackListDetail.Name = "txtLotNoPackListDetail";
		this.txtLotNoPackListDetail.Size = new System.Drawing.Size(100, 22);
		this.txtLotNoPackListDetail.TabIndex = 8;
		this.label148.AutoSize = true;
		this.label148.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label148.Location = new System.Drawing.Point(5, 5);
		this.label148.Name = "label148";
		this.label148.Size = new System.Drawing.Size(77, 15);
		this.label148.TabIndex = 511;
		this.label148.Text = "Container No";
		this.BtnCancelPackListDetail.BackColor = System.Drawing.Color.Teal;
		this.BtnCancelPackListDetail.FlatAppearance.BorderSize = 0;
		this.BtnCancelPackListDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancelPackListDetail.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.BtnCancelPackListDetail.ForeColor = System.Drawing.Color.White;
		this.BtnCancelPackListDetail.Location = new System.Drawing.Point(1107, 48);
		this.BtnCancelPackListDetail.Name = "BtnCancelPackListDetail";
		this.BtnCancelPackListDetail.Size = new System.Drawing.Size(60, 20);
		this.BtnCancelPackListDetail.TabIndex = 17;
		this.BtnCancelPackListDetail.Text = "Cancel";
		this.BtnCancelPackListDetail.UseVisualStyleBackColor = false;
		this.BtnCancelPackListDetail.Visible = false;
		this.BtnCancelPackListDetail.Click += new System.EventHandler(BtnCancelPackListDetail_Click);
		this.BtnUpdatePackListDetail.BackColor = System.Drawing.Color.Teal;
		this.BtnUpdatePackListDetail.FlatAppearance.BorderSize = 0;
		this.BtnUpdatePackListDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdatePackListDetail.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.BtnUpdatePackListDetail.ForeColor = System.Drawing.Color.White;
		this.BtnUpdatePackListDetail.Location = new System.Drawing.Point(1107, 70);
		this.BtnUpdatePackListDetail.Name = "BtnUpdatePackListDetail";
		this.BtnUpdatePackListDetail.Size = new System.Drawing.Size(60, 21);
		this.BtnUpdatePackListDetail.TabIndex = 16;
		this.BtnUpdatePackListDetail.Text = "Update";
		this.BtnUpdatePackListDetail.UseVisualStyleBackColor = false;
		this.BtnUpdatePackListDetail.Visible = false;
		this.BtnUpdatePackListDetail.Click += new System.EventHandler(BtnUpdatePackListDetail_Click);
		this.label146.AutoSize = true;
		this.label146.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label146.Location = new System.Drawing.Point(115, 51);
		this.label146.Name = "label146";
		this.label146.Size = new System.Drawing.Size(43, 15);
		this.label146.TabIndex = 57;
		this.label146.Text = "Lot No";
		this.txtExpiryDatePackListDetail.CustomFormat = "dd-MMM-yy";
		this.txtExpiryDatePackListDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExpiryDatePackListDetail.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtExpiryDatePackListDetail.Location = new System.Drawing.Point(5, 69);
		this.txtExpiryDatePackListDetail.Name = "txtExpiryDatePackListDetail";
		this.txtExpiryDatePackListDetail.Size = new System.Drawing.Size(109, 22);
		this.txtExpiryDatePackListDetail.TabIndex = 7;
		this.label143.AutoSize = true;
		this.label143.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label143.Location = new System.Drawing.Point(5, 51);
		this.label143.Name = "label143";
		this.label143.Size = new System.Drawing.Size(67, 15);
		this.label143.TabIndex = 590;
		this.label143.Text = "Expiry Date";
		this.txtPackageDatePackListDetail.CustomFormat = "dd-MMM-yy";
		this.txtPackageDatePackListDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackageDatePackListDetail.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtPackageDatePackListDetail.Location = new System.Drawing.Point(1057, 23);
		this.txtPackageDatePackListDetail.Name = "txtPackageDatePackListDetail";
		this.txtPackageDatePackListDetail.Size = new System.Drawing.Size(110, 22);
		this.txtPackageDatePackListDetail.TabIndex = 6;
		this.label142.AutoSize = true;
		this.label142.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label142.Location = new System.Drawing.Point(1057, 5);
		this.label142.Name = "label142";
		this.label142.Size = new System.Drawing.Size(79, 15);
		this.label142.TabIndex = 590;
		this.label142.Text = "Package Date";
		this.panel11.Controls.Add(this.HeaderGropBox);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(3, 57);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1175, 368);
		this.panel11.TabIndex = 3;
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbInvoiceNo);
		this.HeaderGropBox.Controls.Add(this.groupBox3);
		this.HeaderGropBox.Controls.Add(this.txtaddlesscommnets);
		this.HeaderGropBox.Controls.Add(this.txtLocalAmt);
		this.HeaderGropBox.Controls.Add(this.txtNoOfContainer);
		this.HeaderGropBox.Controls.Add(this.txtGrossWeight);
		this.HeaderGropBox.Controls.Add(this.txtaddlessamount);
		this.HeaderGropBox.Controls.Add(this.txtNetWeight);
		this.HeaderGropBox.Controls.Add(this.txtTotalNetAmount);
		this.HeaderGropBox.Controls.Add(this.txtFcyAmount);
		this.HeaderGropBox.Controls.Add(this.CmbFcyCode);
		this.HeaderGropBox.Controls.Add(this.txtPlaceOfDelivery);
		this.HeaderGropBox.Controls.Add(this.txtExRate);
		this.HeaderGropBox.Controls.Add(this.label43);
		this.HeaderGropBox.Controls.Add(this.label1);
		this.HeaderGropBox.Controls.Add(this.label45);
		this.HeaderGropBox.Controls.Add(this.label47);
		this.HeaderGropBox.Controls.Add(this.label13);
		this.HeaderGropBox.Controls.Add(this.label60);
		this.HeaderGropBox.Controls.Add(this.label50);
		this.HeaderGropBox.Controls.Add(this.label49);
		this.HeaderGropBox.Controls.Add(this.label53);
		this.HeaderGropBox.Controls.Add(this.label9);
		this.HeaderGropBox.Controls.Add(this.label42);
		this.HeaderGropBox.Controls.Add(this.label27);
		this.HeaderGropBox.Controls.Add(this.label66);
		this.HeaderGropBox.Controls.Add(this.label64);
		this.HeaderGropBox.Controls.Add(this.label44);
		this.HeaderGropBox.Controls.Add(this.label25);
		this.HeaderGropBox.Controls.Add(this.label6);
		this.HeaderGropBox.Controls.Add(this.label40);
		this.HeaderGropBox.Controls.Add(this.txtLotRefHeader);
		this.HeaderGropBox.Controls.Add(this.txtGdDate);
		this.HeaderGropBox.Controls.Add(this.label116);
		this.HeaderGropBox.Controls.Add(this.txtdocno);
		this.HeaderGropBox.Controls.Add(this.txtCustomerContractsNos);
		this.HeaderGropBox.Controls.Add(this.label108);
		this.HeaderGropBox.Controls.Add(this.txtExporterBank);
		this.HeaderGropBox.Controls.Add(this.txtImporterBank);
		this.HeaderGropBox.Controls.Add(this.txtNotifyParty3);
		this.HeaderGropBox.Controls.Add(this.txtNotifyParty2);
		this.HeaderGropBox.Controls.Add(this.txtNotifyParty1);
		this.HeaderGropBox.Controls.Add(this.txtCustomerContractsDates);
		this.HeaderGropBox.Controls.Add(this.txtLoadingPort);
		this.HeaderGropBox.Controls.Add(this.txtDeliveryRemarks);
		this.HeaderGropBox.Controls.Add(this.label87);
		this.HeaderGropBox.Controls.Add(this.label120);
		this.HeaderGropBox.Controls.Add(this.label114);
		this.HeaderGropBox.Controls.Add(this.label86);
		this.HeaderGropBox.Controls.Add(this.txtDeliveryTerm);
		this.HeaderGropBox.Controls.Add(this.txtImporterCountry);
		this.HeaderGropBox.Controls.Add(this.txtCareierType);
		this.HeaderGropBox.Controls.Add(this.txtOriginCountry);
		this.HeaderGropBox.Controls.Add(this.txtDestinationPort);
		this.HeaderGropBox.Controls.Add(this.txtContinent);
		this.HeaderGropBox.Controls.Add(this.txtTcpRegNo);
		this.HeaderGropBox.Controls.Add(this.txtConsignee);
		this.HeaderGropBox.Controls.Add(this.txtCustomer);
		this.HeaderGropBox.Controls.Add(this.txtDocumentaryCreditNo);
		this.HeaderGropBox.Controls.Add(this.label31);
		this.HeaderGropBox.Controls.Add(this.label17);
		this.HeaderGropBox.Controls.Add(this.label113);
		this.HeaderGropBox.Controls.Add(this.label7);
		this.HeaderGropBox.Controls.Add(this.label107);
		this.HeaderGropBox.Controls.Add(this.txtRemarks2);
		this.HeaderGropBox.Controls.Add(this.txtRemarks1);
		this.HeaderGropBox.Controls.Add(this.label118);
		this.HeaderGropBox.Controls.Add(this.txtCertificate1);
		this.HeaderGropBox.Controls.Add(this.label117);
		this.HeaderGropBox.Controls.Add(this.label61);
		this.HeaderGropBox.Controls.Add(this.label57);
		this.HeaderGropBox.Controls.Add(this.label62);
		this.HeaderGropBox.Controls.Add(this.label55);
		this.HeaderGropBox.Controls.Add(this.label2);
		this.HeaderGropBox.Controls.Add(this.label54);
		this.HeaderGropBox.Controls.Add(this.txtCertificateOfOrigion);
		this.HeaderGropBox.Controls.Add(this.label10);
		this.HeaderGropBox.Controls.Add(this.label67);
		this.HeaderGropBox.Controls.Add(this.label95);
		this.HeaderGropBox.Controls.Add(this.label12);
		this.HeaderGropBox.Controls.Add(this.label109);
		this.HeaderGropBox.Controls.Add(this.txtGdNo);
		this.HeaderGropBox.Controls.Add(this.label129);
		this.HeaderGropBox.Controls.Add(this.label115);
		this.HeaderGropBox.Controls.Add(this.label3);
		this.HeaderGropBox.Controls.Add(this.label110);
		this.HeaderGropBox.Controls.Add(this.label68);
		this.HeaderGropBox.Controls.Add(this.txtDocumentaryCreditNoDate);
		this.HeaderGropBox.Controls.Add(this.label16);
		this.HeaderGropBox.Controls.Add(this.label24);
		this.HeaderGropBox.Controls.Add(this.label111);
		this.HeaderGropBox.Controls.Add(this.txtDocDate);
		this.HeaderGropBox.Controls.Add(this.label5);
		this.HeaderGropBox.Controls.Add(this.label41);
		this.HeaderGropBox.Controls.Add(this.label22);
		this.HeaderGropBox.Controls.Add(this.label21);
		this.HeaderGropBox.Controls.Add(this.label28);
		this.HeaderGropBox.Controls.Add(this.label26);
		this.HeaderGropBox.Controls.Add(this.label4);
		this.HeaderGropBox.Controls.Add(this.label8);
		this.HeaderGropBox.Controls.Add(this.label46);
		this.HeaderGropBox.Controls.Add(this.label56);
		this.HeaderGropBox.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.HeaderGropBox.Location = new System.Drawing.Point(4, 2);
		this.HeaderGropBox.Name = "HeaderGropBox";
		this.HeaderGropBox.Size = new System.Drawing.Size(1163, 365);
		this.HeaderGropBox.TabIndex = 0;
		this.HeaderGropBox.TabStop = false;
		this.HeaderGropBox.Text = "Main Header";
		this.cmbInvoiceNo.AllowNull = (DefaultableBoolean)1;
		this.cmbInvoiceNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbInvoiceNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbInvoiceNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance131).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance131).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance132;
		((SpecialBoxBase)((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance133).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance137).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance138).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance140).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbInvoiceNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Location = new System.Drawing.Point(115, 11);
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Name = "cmbInvoiceNo";
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Size = new System.Drawing.Size(235, 26);
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Leave += new System.EventHandler(cmbInvoiceNo_Leave);
		this.groupBox3.Controls.Add(this.txtFcyAmountHeaderCustom);
		this.groupBox3.Controls.Add(this.txtAddLessHeaderCustom);
		this.groupBox3.Controls.Add(this.txtLocalAmountHeaderCustom);
		this.groupBox3.Controls.Add(this.txtTotalFcyAmountHeaderCustom);
		this.groupBox3.Controls.Add(this.label112);
		this.groupBox3.Controls.Add(this.label119);
		this.groupBox3.Controls.Add(this.label121);
		this.groupBox3.Controls.Add(this.label122);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.groupBox3.Location = new System.Drawing.Point(881, 238);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(280, 121);
		this.groupBox3.TabIndex = 601;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Custom Values";
		this.txtFcyAmountHeaderCustom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmountHeaderCustom.Enabled = false;
		this.txtFcyAmountHeaderCustom.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFcyAmountHeaderCustom.Location = new System.Drawing.Point(106, 15);
		this.txtFcyAmountHeaderCustom.Name = "txtFcyAmountHeaderCustom";
		this.txtFcyAmountHeaderCustom.Size = new System.Drawing.Size(172, 22);
		this.txtFcyAmountHeaderCustom.TabIndex = 29;
		this.txtFcyAmountHeaderCustom.Text = "0";
		this.txtFcyAmountHeaderCustom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAddLessHeaderCustom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAddLessHeaderCustom.Enabled = false;
		this.txtAddLessHeaderCustom.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAddLessHeaderCustom.Location = new System.Drawing.Point(106, 41);
		this.txtAddLessHeaderCustom.Name = "txtAddLessHeaderCustom";
		this.txtAddLessHeaderCustom.Size = new System.Drawing.Size(172, 22);
		this.txtAddLessHeaderCustom.TabIndex = 30;
		this.txtAddLessHeaderCustom.Text = "0";
		this.txtAddLessHeaderCustom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtLocalAmountHeaderCustom.BackColor = System.Drawing.Color.White;
		this.txtLocalAmountHeaderCustom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLocalAmountHeaderCustom.Enabled = false;
		this.txtLocalAmountHeaderCustom.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLocalAmountHeaderCustom.Location = new System.Drawing.Point(106, 93);
		this.txtLocalAmountHeaderCustom.Name = "txtLocalAmountHeaderCustom";
		this.txtLocalAmountHeaderCustom.Size = new System.Drawing.Size(172, 22);
		this.txtLocalAmountHeaderCustom.TabIndex = 33;
		this.txtLocalAmountHeaderCustom.TabStop = false;
		this.txtLocalAmountHeaderCustom.Text = "0";
		this.txtLocalAmountHeaderCustom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTotalFcyAmountHeaderCustom.BackColor = System.Drawing.Color.White;
		this.txtTotalFcyAmountHeaderCustom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalFcyAmountHeaderCustom.Enabled = false;
		this.txtTotalFcyAmountHeaderCustom.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalFcyAmountHeaderCustom.Location = new System.Drawing.Point(106, 67);
		this.txtTotalFcyAmountHeaderCustom.Name = "txtTotalFcyAmountHeaderCustom";
		this.txtTotalFcyAmountHeaderCustom.Size = new System.Drawing.Size(172, 22);
		this.txtTotalFcyAmountHeaderCustom.TabIndex = 32;
		this.txtTotalFcyAmountHeaderCustom.TabStop = false;
		this.txtTotalFcyAmountHeaderCustom.Text = "0";
		this.txtTotalFcyAmountHeaderCustom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label112.AutoSize = true;
		this.label112.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label112.Location = new System.Drawing.Point(4, 19);
		this.label112.Name = "label112";
		this.label112.Size = new System.Drawing.Size(74, 15);
		this.label112.TabIndex = 538;
		this.label112.Text = "Fcy-Amount";
		this.label119.AutoSize = true;
		this.label119.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label119.Location = new System.Drawing.Point(4, 45);
		this.label119.Name = "label119";
		this.label119.Size = new System.Drawing.Size(80, 15);
		this.label119.TabIndex = 582;
		this.label119.Text = "Add Less Amt";
		this.label121.AutoSize = true;
		this.label121.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label121.Location = new System.Drawing.Point(4, 71);
		this.label121.Name = "label121";
		this.label121.Size = new System.Drawing.Size(101, 15);
		this.label121.TabIndex = 585;
		this.label121.Text = "Total Fcy Amount";
		this.label122.AutoSize = true;
		this.label122.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label122.Location = new System.Drawing.Point(4, 97);
		this.label122.Name = "label122";
		this.label122.Size = new System.Drawing.Size(82, 15);
		this.label122.TabIndex = 549;
		this.label122.Text = "Local Amount";
		this.txtaddlesscommnets.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtaddlesscommnets.Enabled = false;
		this.txtaddlesscommnets.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtaddlesscommnets.Location = new System.Drawing.Point(734, 138);
		this.txtaddlesscommnets.Multiline = true;
		this.txtaddlesscommnets.Name = "txtaddlesscommnets";
		this.txtaddlesscommnets.Size = new System.Drawing.Size(145, 47);
		this.txtaddlesscommnets.TabIndex = 31;
		this.txtLocalAmt.BackColor = System.Drawing.Color.White;
		this.txtLocalAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLocalAmt.Enabled = false;
		this.txtLocalAmt.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLocalAmt.Location = new System.Drawing.Point(734, 213);
		this.txtLocalAmt.Name = "txtLocalAmt";
		this.txtLocalAmt.ReadOnly = true;
		this.txtLocalAmt.Size = new System.Drawing.Size(145, 22);
		this.txtLocalAmt.TabIndex = 33;
		this.txtLocalAmt.TabStop = false;
		this.txtLocalAmt.Text = "0";
		this.txtLocalAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNoOfContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNoOfContainer.Enabled = false;
		this.txtNoOfContainer.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNoOfContainer.Location = new System.Drawing.Point(734, 238);
		this.txtNoOfContainer.Name = "txtNoOfContainer";
		this.txtNoOfContainer.Size = new System.Drawing.Size(145, 22);
		this.txtNoOfContainer.TabIndex = 34;
		this.txtNoOfContainer.Text = "0";
		this.txtNoOfContainer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Enabled = false;
		this.txtGrossWeight.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrossWeight.Location = new System.Drawing.Point(734, 263);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.Size = new System.Drawing.Size(145, 22);
		this.txtGrossWeight.TabIndex = 35;
		this.txtGrossWeight.Text = "0";
		this.txtGrossWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtaddlessamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtaddlessamount.Enabled = false;
		this.txtaddlessamount.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtaddlessamount.Location = new System.Drawing.Point(734, 113);
		this.txtaddlessamount.Name = "txtaddlessamount";
		this.txtaddlessamount.Size = new System.Drawing.Size(145, 22);
		this.txtaddlessamount.TabIndex = 30;
		this.txtaddlessamount.Text = "0";
		this.txtaddlessamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeight.Enabled = false;
		this.txtNetWeight.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeight.Location = new System.Drawing.Point(734, 288);
		this.txtNetWeight.Name = "txtNetWeight";
		this.txtNetWeight.Size = new System.Drawing.Size(145, 22);
		this.txtNetWeight.TabIndex = 36;
		this.txtNetWeight.Text = "0";
		this.txtNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTotalNetAmount.BackColor = System.Drawing.Color.White;
		this.txtTotalNetAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalNetAmount.Enabled = false;
		this.txtTotalNetAmount.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalNetAmount.Location = new System.Drawing.Point(734, 188);
		this.txtTotalNetAmount.Name = "txtTotalNetAmount";
		this.txtTotalNetAmount.ReadOnly = true;
		this.txtTotalNetAmount.Size = new System.Drawing.Size(145, 22);
		this.txtTotalNetAmount.TabIndex = 32;
		this.txtTotalNetAmount.TabStop = false;
		this.txtTotalNetAmount.Text = "0";
		this.txtTotalNetAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmount.Enabled = false;
		this.txtFcyAmount.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFcyAmount.Location = new System.Drawing.Point(734, 88);
		this.txtFcyAmount.Name = "txtFcyAmount";
		this.txtFcyAmount.Size = new System.Drawing.Size(145, 22);
		this.txtFcyAmount.TabIndex = 29;
		this.txtFcyAmount.Text = "0";
		this.txtFcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.CmbFcyCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.CmbFcyCode.Enabled = false;
		this.CmbFcyCode.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbFcyCode.Location = new System.Drawing.Point(735, 39);
		this.CmbFcyCode.Name = "CmbFcyCode";
		this.CmbFcyCode.Size = new System.Drawing.Size(145, 22);
		this.CmbFcyCode.TabIndex = 28;
		this.CmbFcyCode.Text = "0";
		this.CmbFcyCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPlaceOfDelivery.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPlaceOfDelivery.Enabled = false;
		this.txtPlaceOfDelivery.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPlaceOfDelivery.Location = new System.Drawing.Point(735, 13);
		this.txtPlaceOfDelivery.Name = "txtPlaceOfDelivery";
		this.txtPlaceOfDelivery.Size = new System.Drawing.Size(145, 22);
		this.txtPlaceOfDelivery.TabIndex = 28;
		this.txtPlaceOfDelivery.Text = "0";
		this.txtPlaceOfDelivery.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExRate.Enabled = false;
		this.txtExRate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExRate.Location = new System.Drawing.Point(734, 63);
		this.txtExRate.Name = "txtExRate";
		this.txtExRate.Size = new System.Drawing.Size(145, 22);
		this.txtExRate.TabIndex = 28;
		this.txtExRate.Text = "0";
		this.txtExRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.ForeColor = System.Drawing.Color.Red;
		this.label43.Location = new System.Drawing.Point(724, 216);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(14, 17);
		this.label43.TabIndex = 42;
		this.label43.Text = "*";
		this.label43.Visible = false;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Red;
		this.label1.Location = new System.Drawing.Point(724, 91);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(14, 17);
		this.label1.TabIndex = 43;
		this.label1.Text = "*";
		this.label1.Visible = false;
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.ForeColor = System.Drawing.Color.Red;
		this.label45.Location = new System.Drawing.Point(724, 191);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(14, 17);
		this.label45.TabIndex = 550;
		this.label45.Text = "*";
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.ForeColor = System.Drawing.Color.Red;
		this.label47.Location = new System.Drawing.Point(724, 66);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(14, 17);
		this.label47.TabIndex = 543;
		this.label47.Text = "*";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.Red;
		this.label13.Location = new System.Drawing.Point(724, 16);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(14, 17);
		this.label13.TabIndex = 598;
		this.label13.Text = "*";
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.ForeColor = System.Drawing.Color.Red;
		this.label60.Location = new System.Drawing.Point(724, 42);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(14, 17);
		this.label60.TabIndex = 573;
		this.label60.Text = "*";
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.ForeColor = System.Drawing.Color.Red;
		this.label50.Location = new System.Drawing.Point(724, 266);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(14, 17);
		this.label50.TabIndex = 569;
		this.label50.Text = "*";
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.ForeColor = System.Drawing.Color.Red;
		this.label49.Location = new System.Drawing.Point(724, 291);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(14, 17);
		this.label49.TabIndex = 560;
		this.label49.Text = "*";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.ForeColor = System.Drawing.Color.Red;
		this.label53.Location = new System.Drawing.Point(724, 241);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(14, 17);
		this.label53.TabIndex = 570;
		this.label53.Text = "*";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.Red;
		this.label9.Location = new System.Drawing.Point(427, 166);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(14, 17);
		this.label9.TabIndex = 528;
		this.label9.Text = "*";
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label42.Location = new System.Drawing.Point(627, 217);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(82, 15);
		this.label42.TabIndex = 549;
		this.label42.Text = "Local Amount";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label27.Location = new System.Drawing.Point(627, 43);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(55, 15);
		this.label27.TabIndex = 537;
		this.label27.Text = "Fcy Code";
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label66.Location = new System.Drawing.Point(627, 192);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(101, 15);
		this.label66.TabIndex = 585;
		this.label66.Text = "Total Fcy Amount";
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label64.Location = new System.Drawing.Point(627, 117);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(80, 15);
		this.label64.TabIndex = 582;
		this.label64.Text = "Add Less Amt";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label44.Location = new System.Drawing.Point(627, 67);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(84, 15);
		this.label44.TabIndex = 546;
		this.label44.Text = "Exchange Rate";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(627, 92);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(74, 15);
		this.label25.TabIndex = 538;
		this.label25.Text = "Fcy-Amount";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.Red;
		this.label6.Location = new System.Drawing.Point(427, 291);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(14, 17);
		this.label6.TabIndex = 545;
		this.label6.Text = "*";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label40.Location = new System.Drawing.Point(627, 142);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(99, 15);
		this.label40.TabIndex = 580;
		this.label40.Text = "Ad/Ls Comments";
		this.txtLotRefHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLotRefHeader.Enabled = false;
		this.txtLotRefHeader.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLotRefHeader.Location = new System.Drawing.Point(444, 13);
		this.txtLotRefHeader.Name = "txtLotRefHeader";
		this.txtLotRefHeader.Size = new System.Drawing.Size(179, 22);
		this.txtLotRefHeader.TabIndex = 14;
		this.txtGdDate.CustomFormat = "dd-MMM-yy";
		this.txtGdDate.Enabled = false;
		this.txtGdDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGdDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtGdDate.Location = new System.Drawing.Point(444, 63);
		this.txtGdDate.Name = "txtGdDate";
		this.txtGdDate.Size = new System.Drawing.Size(179, 22);
		this.txtGdDate.TabIndex = 16;
		this.label116.AutoSize = true;
		this.label116.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label116.ForeColor = System.Drawing.Color.Red;
		this.label116.Location = new System.Drawing.Point(427, 116);
		this.label116.Name = "label116";
		this.label116.Size = new System.Drawing.Size(14, 17);
		this.label116.TabIndex = 545;
		this.label116.Text = "*";
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Enabled = false;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(115, 39);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(125, 22);
		this.txtdocno.TabIndex = 1;
		this.txtdocno.TabStop = false;
		this.txtCustomerContractsNos.Enabled = false;
		this.txtCustomerContractsNos.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCustomerContractsNos.Location = new System.Drawing.Point(115, 138);
		this.txtCustomerContractsNos.Name = "txtCustomerContractsNos";
		this.txtCustomerContractsNos.Size = new System.Drawing.Size(235, 23);
		this.txtCustomerContractsNos.TabIndex = 7;
		this.label108.AutoSize = true;
		this.label108.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label108.Location = new System.Drawing.Point(5, 242);
		this.label108.Name = "label108";
		this.label108.Size = new System.Drawing.Size(80, 15);
		this.label108.TabIndex = 596;
		this.label108.Text = "Notify Party 3";
		this.txtExporterBank.Enabled = false;
		this.txtExporterBank.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExporterBank.Location = new System.Drawing.Point(115, 288);
		this.txtExporterBank.Name = "txtExporterBank";
		this.txtExporterBank.Size = new System.Drawing.Size(235, 23);
		this.txtExporterBank.TabIndex = 8;
		this.txtImporterBank.Enabled = false;
		this.txtImporterBank.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtImporterBank.Location = new System.Drawing.Point(115, 263);
		this.txtImporterBank.Name = "txtImporterBank";
		this.txtImporterBank.Size = new System.Drawing.Size(235, 23);
		this.txtImporterBank.TabIndex = 8;
		this.txtNotifyParty3.Enabled = false;
		this.txtNotifyParty3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNotifyParty3.Location = new System.Drawing.Point(115, 238);
		this.txtNotifyParty3.Name = "txtNotifyParty3";
		this.txtNotifyParty3.Size = new System.Drawing.Size(235, 23);
		this.txtNotifyParty3.TabIndex = 8;
		this.txtNotifyParty2.Enabled = false;
		this.txtNotifyParty2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNotifyParty2.Location = new System.Drawing.Point(115, 213);
		this.txtNotifyParty2.Name = "txtNotifyParty2";
		this.txtNotifyParty2.Size = new System.Drawing.Size(235, 23);
		this.txtNotifyParty2.TabIndex = 8;
		this.txtNotifyParty1.Enabled = false;
		this.txtNotifyParty1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNotifyParty1.Location = new System.Drawing.Point(115, 188);
		this.txtNotifyParty1.Name = "txtNotifyParty1";
		this.txtNotifyParty1.Size = new System.Drawing.Size(235, 23);
		this.txtNotifyParty1.TabIndex = 8;
		this.txtCustomerContractsDates.Enabled = false;
		this.txtCustomerContractsDates.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCustomerContractsDates.Location = new System.Drawing.Point(115, 163);
		this.txtCustomerContractsDates.Name = "txtCustomerContractsDates";
		this.txtCustomerContractsDates.Size = new System.Drawing.Size(235, 23);
		this.txtCustomerContractsDates.TabIndex = 8;
		this.txtLoadingPort.Enabled = false;
		this.txtLoadingPort.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLoadingPort.Location = new System.Drawing.Point(444, 88);
		this.txtLoadingPort.Name = "txtLoadingPort";
		this.txtLoadingPort.Size = new System.Drawing.Size(179, 23);
		this.txtLoadingPort.TabIndex = 21;
		this.txtDeliveryRemarks.Enabled = false;
		this.txtDeliveryRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDeliveryRemarks.Location = new System.Drawing.Point(444, 188);
		this.txtDeliveryRemarks.Name = "txtDeliveryRemarks";
		this.txtDeliveryRemarks.Size = new System.Drawing.Size(179, 23);
		this.txtDeliveryRemarks.TabIndex = 21;
		this.label87.AutoSize = true;
		this.label87.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label87.Location = new System.Drawing.Point(352, 192);
		this.label87.Name = "label87";
		this.label87.Size = new System.Drawing.Size(52, 15);
		this.label87.TabIndex = 573;
		this.label87.Text = "Remarks";
		this.label120.AutoSize = true;
		this.label120.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label120.Location = new System.Drawing.Point(5, 167);
		this.label120.Name = "label120";
		this.label120.Size = new System.Drawing.Size(115, 15);
		this.label120.TabIndex = 579;
		this.label120.Text = "Sales Contract Dates";
		this.label114.AutoSize = true;
		this.label114.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label114.Location = new System.Drawing.Point(5, 142);
		this.label114.Name = "label114";
		this.label114.Size = new System.Drawing.Size(106, 15);
		this.label114.TabIndex = 579;
		this.label114.Text = "Sales Contract Nos";
		this.label86.AutoSize = true;
		this.label86.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label86.Location = new System.Drawing.Point(5, 117);
		this.label86.Name = "label86";
		this.label86.Size = new System.Drawing.Size(97, 15);
		this.label86.TabIndex = 594;
		this.label86.Text = "Consignee Name";
		this.txtDeliveryTerm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDeliveryTerm.Enabled = false;
		this.txtDeliveryTerm.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDeliveryTerm.Location = new System.Drawing.Point(444, 163);
		this.txtDeliveryTerm.Name = "txtDeliveryTerm";
		this.txtDeliveryTerm.Size = new System.Drawing.Size(179, 22);
		this.txtDeliveryTerm.TabIndex = 22;
		this.txtImporterCountry.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtImporterCountry.Enabled = false;
		this.txtImporterCountry.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtImporterCountry.Location = new System.Drawing.Point(444, 288);
		this.txtImporterCountry.Name = "txtImporterCountry";
		this.txtImporterCountry.Size = new System.Drawing.Size(179, 22);
		this.txtImporterCountry.TabIndex = 22;
		this.txtCareierType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCareierType.Enabled = false;
		this.txtCareierType.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCareierType.Location = new System.Drawing.Point(444, 138);
		this.txtCareierType.Name = "txtCareierType";
		this.txtCareierType.Size = new System.Drawing.Size(179, 22);
		this.txtCareierType.TabIndex = 22;
		this.txtOriginCountry.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOriginCountry.Enabled = false;
		this.txtOriginCountry.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOriginCountry.Location = new System.Drawing.Point(444, 263);
		this.txtOriginCountry.Name = "txtOriginCountry";
		this.txtOriginCountry.Size = new System.Drawing.Size(179, 22);
		this.txtOriginCountry.TabIndex = 22;
		this.txtDestinationPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDestinationPort.Enabled = false;
		this.txtDestinationPort.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDestinationPort.Location = new System.Drawing.Point(444, 113);
		this.txtDestinationPort.Name = "txtDestinationPort";
		this.txtDestinationPort.Size = new System.Drawing.Size(179, 22);
		this.txtDestinationPort.TabIndex = 22;
		this.txtContinent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtContinent.Enabled = false;
		this.txtContinent.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtContinent.Location = new System.Drawing.Point(444, 238);
		this.txtContinent.Name = "txtContinent";
		this.txtContinent.Size = new System.Drawing.Size(179, 22);
		this.txtContinent.TabIndex = 22;
		this.txtTcpRegNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTcpRegNo.Enabled = false;
		this.txtTcpRegNo.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTcpRegNo.Location = new System.Drawing.Point(444, 213);
		this.txtTcpRegNo.Name = "txtTcpRegNo";
		this.txtTcpRegNo.Size = new System.Drawing.Size(179, 22);
		this.txtTcpRegNo.TabIndex = 22;
		this.txtConsignee.Enabled = false;
		this.txtConsignee.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtConsignee.Location = new System.Drawing.Point(115, 113);
		this.txtConsignee.Name = "txtConsignee";
		this.txtConsignee.Size = new System.Drawing.Size(235, 23);
		this.txtConsignee.TabIndex = 3;
		this.txtCustomer.Enabled = false;
		this.txtCustomer.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCustomer.Location = new System.Drawing.Point(115, 88);
		this.txtCustomer.Name = "txtCustomer";
		this.txtCustomer.Size = new System.Drawing.Size(235, 23);
		this.txtCustomer.TabIndex = 3;
		this.txtDocumentaryCreditNo.Enabled = false;
		this.txtDocumentaryCreditNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocumentaryCreditNo.Location = new System.Drawing.Point(115, 63);
		this.txtDocumentaryCreditNo.Name = "txtDocumentaryCreditNo";
		this.txtDocumentaryCreditNo.Size = new System.Drawing.Size(125, 23);
		this.txtDocumentaryCreditNo.TabIndex = 3;
		this.label31.AutoSize = true;
		this.label31.BackColor = System.Drawing.Color.White;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label31.Location = new System.Drawing.Point(5, 267);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(83, 15);
		this.label31.TabIndex = 524;
		this.label31.Text = "Importer Bank";
		this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.Red;
		this.label17.Location = new System.Drawing.Point(427, 66);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(14, 17);
		this.label17.TabIndex = 591;
		this.label17.Text = "*";
		this.label17.Visible = false;
		this.label113.AutoSize = true;
		this.label113.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label113.Location = new System.Drawing.Point(352, 217);
		this.label113.Name = "label113";
		this.label113.Size = new System.Drawing.Size(71, 15);
		this.label113.TabIndex = 57;
		this.label113.Text = "TCP REG No";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(352, 17);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(79, 15);
		this.label7.TabIndex = 57;
		this.label7.Text = "Lot Reference";
		this.label107.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label107.Location = new System.Drawing.Point(5, 59);
		this.label107.Name = "label107";
		this.label107.Size = new System.Drawing.Size(99, 31);
		this.label107.TabIndex = 57;
		this.label107.Text = "Documentory Credit No && Date";
		this.txtRemarks2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks2.Enabled = false;
		this.txtRemarks2.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks2.Location = new System.Drawing.Point(115, 337);
		this.txtRemarks2.Name = "txtRemarks2";
		this.txtRemarks2.Size = new System.Drawing.Size(764, 22);
		this.txtRemarks2.TabIndex = 40;
		this.txtRemarks1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks1.Enabled = false;
		this.txtRemarks1.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks1.Location = new System.Drawing.Point(115, 313);
		this.txtRemarks1.Name = "txtRemarks1";
		this.txtRemarks1.Size = new System.Drawing.Size(764, 22);
		this.txtRemarks1.TabIndex = 39;
		this.label118.BackColor = System.Drawing.Color.Gainsboro;
		this.label118.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.label118.Location = new System.Drawing.Point(5, 337);
		this.label118.Name = "label118";
		this.label118.Size = new System.Drawing.Size(107, 22);
		this.label118.TabIndex = 576;
		this.label118.Text = "Remarks 2";
		this.label118.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.txtCertificate1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCertificate1.Enabled = false;
		this.txtCertificate1.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCertificate1.Location = new System.Drawing.Point(881, 39);
		this.txtCertificate1.Multiline = true;
		this.txtCertificate1.Name = "txtCertificate1";
		this.txtCertificate1.Size = new System.Drawing.Size(278, 96);
		this.txtCertificate1.TabIndex = 37;
		this.label117.BackColor = System.Drawing.Color.Gainsboro;
		this.label117.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.label117.Location = new System.Drawing.Point(5, 313);
		this.label117.Name = "label117";
		this.label117.Size = new System.Drawing.Size(107, 22);
		this.label117.TabIndex = 576;
		this.label117.Text = "Remarks 1";
		this.label117.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label61.BackColor = System.Drawing.Color.Gainsboro;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.label61.Location = new System.Drawing.Point(881, 13);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(278, 22);
		this.label61.TabIndex = 576;
		this.label61.Text = "Certificate -1";
		this.label61.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label57.Location = new System.Drawing.Point(627, 242);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(93, 15);
		this.label57.TabIndex = 552;
		this.label57.Text = "No.Of Container";
		this.label62.BackColor = System.Drawing.Color.Gainsboro;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.label62.Location = new System.Drawing.Point(881, 138);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(278, 22);
		this.label62.TabIndex = 577;
		this.label62.Text = "Certificate Of Origin";
		this.label62.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label55.Location = new System.Drawing.Point(627, 292);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(67, 15);
		this.label55.TabIndex = 554;
		this.label55.Text = "Net MTon's";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(352, 67);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(55, 15);
		this.label2.TabIndex = 590;
		this.label2.Text = "G.D Date";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label54.Location = new System.Drawing.Point(627, 267);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(77, 15);
		this.label54.TabIndex = 553;
		this.label54.Text = "Gross MTon's";
		this.txtCertificateOfOrigion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCertificateOfOrigion.Enabled = false;
		this.txtCertificateOfOrigion.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCertificateOfOrigion.Location = new System.Drawing.Point(881, 163);
		this.txtCertificateOfOrigion.Multiline = true;
		this.txtCertificateOfOrigion.Name = "txtCertificateOfOrigion";
		this.txtCertificateOfOrigion.Size = new System.Drawing.Size(278, 73);
		this.txtCertificateOfOrigion.TabIndex = 38;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(5, 217);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(80, 15);
		this.label10.TabIndex = 120;
		this.label10.Text = "Notify Party 2";
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label67.Location = new System.Drawing.Point(5, 17);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(65, 15);
		this.label67.TabIndex = 57;
		this.label67.Text = "Invoice No";
		this.label95.AutoSize = true;
		this.label95.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label95.ForeColor = System.Drawing.Color.Red;
		this.label95.Location = new System.Drawing.Point(103, 66);
		this.label95.Name = "label95";
		this.label95.Size = new System.Drawing.Size(14, 17);
		this.label95.TabIndex = 50;
		this.label95.Text = "*";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(352, 167);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(80, 15);
		this.label12.TabIndex = 526;
		this.label12.Text = "Delivery Term";
		this.label109.AutoSize = true;
		this.label109.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label109.Location = new System.Drawing.Point(352, 267);
		this.label109.Name = "label109";
		this.label109.Size = new System.Drawing.Size(40, 15);
		this.label109.TabIndex = 542;
		this.label109.Text = "Origin";
		this.txtGdNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGdNo.Enabled = false;
		this.txtGdNo.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGdNo.Location = new System.Drawing.Point(444, 39);
		this.txtGdNo.Name = "txtGdNo";
		this.txtGdNo.Size = new System.Drawing.Size(179, 22);
		this.txtGdNo.TabIndex = 15;
		this.label129.AutoSize = true;
		this.label129.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label129.Location = new System.Drawing.Point(627, 17);
		this.label129.Name = "label129";
		this.label129.Size = new System.Drawing.Size(81, 15);
		this.label129.TabIndex = 542;
		this.label129.Text = "Delivery Place";
		this.label115.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label115.Location = new System.Drawing.Point(352, 284);
		this.label115.Name = "label115";
		this.label115.Size = new System.Drawing.Size(79, 31);
		this.label115.TabIndex = 542;
		this.label115.Text = "Importer  Country\r\n";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.Red;
		this.label3.Location = new System.Drawing.Point(103, 42);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(14, 17);
		this.label3.TabIndex = 0;
		this.label3.Text = "*";
		this.label110.AutoSize = true;
		this.label110.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label110.ForeColor = System.Drawing.Color.Red;
		this.label110.Location = new System.Drawing.Point(427, 266);
		this.label110.Name = "label110";
		this.label110.Size = new System.Drawing.Size(14, 17);
		this.label110.TabIndex = 545;
		this.label110.Text = "*";
		this.label68.AutoSize = true;
		this.label68.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label68.ForeColor = System.Drawing.Color.Red;
		this.label68.Location = new System.Drawing.Point(103, 16);
		this.label68.Name = "label68";
		this.label68.Size = new System.Drawing.Size(14, 17);
		this.label68.TabIndex = 50;
		this.label68.Text = "*";
		this.txtDocumentaryCreditNoDate.CustomFormat = "dd-MMM-yy";
		this.txtDocumentaryCreditNoDate.Enabled = false;
		this.txtDocumentaryCreditNoDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocumentaryCreditNoDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocumentaryCreditNoDate.Location = new System.Drawing.Point(241, 63);
		this.txtDocumentaryCreditNoDate.Name = "txtDocumentaryCreditNoDate";
		this.txtDocumentaryCreditNoDate.Size = new System.Drawing.Size(109, 22);
		this.txtDocumentaryCreditNoDate.TabIndex = 4;
		this.label16.BackColor = System.Drawing.Color.White;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(352, 109);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(79, 31);
		this.label16.TabIndex = 533;
		this.label16.Text = "Destination Port";
		this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label24.AutoSize = true;
		this.label24.BackColor = System.Drawing.Color.White;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(5, 292);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(80, 15);
		this.label24.TabIndex = 525;
		this.label24.Text = "Exporter Bank";
		this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label111.AutoSize = true;
		this.label111.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label111.Location = new System.Drawing.Point(352, 242);
		this.label111.Name = "label111";
		this.label111.Size = new System.Drawing.Size(59, 15);
		this.label111.TabIndex = 542;
		this.label111.Text = "Continent";
		this.txtDocDate.CustomFormat = "dd-MMM-yy";
		this.txtDocDate.Enabled = false;
		this.txtDocDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocDate.Location = new System.Drawing.Point(242, 39);
		this.txtDocDate.Name = "txtDocDate";
		this.txtDocDate.Size = new System.Drawing.Size(109, 22);
		this.txtDocDate.TabIndex = 2;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(5, 43);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(48, 15);
		this.label5.TabIndex = 49;
		this.label5.Text = "Doc No";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.ForeColor = System.Drawing.Color.Red;
		this.label41.Location = new System.Drawing.Point(427, 141);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(14, 17);
		this.label41.TabIndex = 545;
		this.label41.Text = "*";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(352, 142);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(69, 15);
		this.label22.TabIndex = 542;
		this.label22.Text = "Carrier Type";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(5, 92);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(93, 15);
		this.label21.TabIndex = 78;
		this.label21.Text = "Customer Name";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.ForeColor = System.Drawing.Color.Red;
		this.label28.Location = new System.Drawing.Point(102, 91);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(14, 17);
		this.label28.TabIndex = 111;
		this.label28.Text = "*";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ForeColor = System.Drawing.Color.Red;
		this.label26.Location = new System.Drawing.Point(427, 91);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(14, 17);
		this.label26.TabIndex = 535;
		this.label26.Text = "*";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.Red;
		this.label4.Location = new System.Drawing.Point(102, 91);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(14, 17);
		this.label4.TabIndex = 115;
		this.label4.Text = "*";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(5, 192);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(78, 15);
		this.label8.TabIndex = 117;
		this.label8.Text = "Notify Party 1";
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label46.Location = new System.Drawing.Point(352, 43);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(46, 15);
		this.label46.TabIndex = 561;
		this.label46.Text = "G.D No";
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label56.Location = new System.Drawing.Point(352, 92);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(75, 15);
		this.label56.TabIndex = 534;
		this.label56.Text = "Loading Port";
		this.panel17.AutoScroll = true;
		this.panel17.Controls.Add(this.txtOtherRemarks2);
		this.panel17.Controls.Add(this.label155);
		this.panel17.Controls.Add(this.txtOtherRemarks1);
		this.panel17.Controls.Add(this.label154);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel17.Location = new System.Drawing.Point(3, 677);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1175, 52);
		this.panel17.TabIndex = 1;
		this.txtOtherRemarks2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherRemarks2.Enabled = false;
		this.txtOtherRemarks2.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOtherRemarks2.Location = new System.Drawing.Point(119, 26);
		this.txtOtherRemarks2.Name = "txtOtherRemarks2";
		this.txtOtherRemarks2.Size = new System.Drawing.Size(1045, 22);
		this.txtOtherRemarks2.TabIndex = 1;
		this.label155.AutoSize = true;
		this.label155.BackColor = System.Drawing.Color.Gainsboro;
		this.label155.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label155.Location = new System.Drawing.Point(4, 28);
		this.label155.Name = "label155";
		this.label155.Size = new System.Drawing.Size(114, 19);
		this.label155.TabIndex = 576;
		this.label155.Text = "Other Remarks 2";
		this.label155.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtOtherRemarks1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherRemarks1.Enabled = false;
		this.txtOtherRemarks1.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOtherRemarks1.Location = new System.Drawing.Point(119, 3);
		this.txtOtherRemarks1.Name = "txtOtherRemarks1";
		this.txtOtherRemarks1.Size = new System.Drawing.Size(1045, 22);
		this.txtOtherRemarks1.TabIndex = 0;
		this.label154.AutoSize = true;
		this.label154.BackColor = System.Drawing.Color.Gainsboro;
		this.label154.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label154.Location = new System.Drawing.Point(4, 5);
		this.label154.Name = "label154";
		this.label154.Size = new System.Drawing.Size(112, 19);
		this.label154.TabIndex = 576;
		this.label154.Text = "Other Remarks 1";
		this.label154.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.labelExportCompany);
		this.panel10.Controls.Add(this.label23);
		this.panel10.Controls.Add(this.chkSlipC);
		this.panel10.Controls.Add(this.ChkSlipB);
		this.panel10.Controls.Add(this.ChkPrintPreview);
		this.panel10.Controls.Add(this.CmbExportCompany);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 30);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1175, 27);
		this.panel10.TabIndex = 1;
		this.labelExportCompany.AutoSize = true;
		this.labelExportCompany.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelExportCompany.ForeColor = System.Drawing.Color.White;
		this.labelExportCompany.Location = new System.Drawing.Point(320, 6);
		this.labelExportCompany.Name = "labelExportCompany";
		this.labelExportCompany.Size = new System.Drawing.Size(98, 15);
		this.labelExportCompany.TabIndex = 501;
		this.labelExportCompany.Text = "Export Company";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(3, 3);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(230, 19);
		this.label23.TabIndex = 25;
		this.label23.Text = "Export Invoice Packing List";
		this.ChkPrintPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrintPreview.AutoSize = true;
		this.ChkPrintPreview.Checked = true;
		this.ChkPrintPreview.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkPrintPreview.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrintPreview.ForeColor = System.Drawing.Color.White;
		this.ChkPrintPreview.Location = new System.Drawing.Point(852, 3);
		this.ChkPrintPreview.Name = "ChkPrintPreview";
		this.ChkPrintPreview.Size = new System.Drawing.Size(101, 20);
		this.ChkPrintPreview.TabIndex = 32;
		this.ChkPrintPreview.Text = "Preview-A";
		this.ChkPrintPreview.UseVisualStyleBackColor = true;
		this.CmbExportCompany.Enabled = false;
		this.CmbExportCompany.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbExportCompany.Location = new System.Drawing.Point(444, 2);
		this.CmbExportCompany.Name = "CmbExportCompany";
		this.CmbExportCompany.Size = new System.Drawing.Size(235, 23);
		this.CmbExportCompany.TabIndex = 3;
		this.panel12.Controls.Add(this.toolStrip4);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(3, 3);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1175, 27);
		this.panel12.TabIndex = 0;
		this.toolStrip4.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip4.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.btnNew, this.btnRefresh, this.btnsave, this.BtnDelete, this.btnupdate, this.btnShortCutKey, this.btnPrint, this.btnPrint529B, this.btnPrint529C });
		this.toolStrip4.Location = new System.Drawing.Point(0, 0);
		this.toolStrip4.Name = "toolStrip4";
		this.toolStrip4.Size = new System.Drawing.Size(1175, 27);
		this.toolStrip4.TabIndex = 4;
		this.toolStrip4.Text = "toolStrip4";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.BtnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.BtnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnDelete.Name = "BtnDelete";
		this.BtnDelete.Size = new System.Drawing.Size(73, 24);
		this.BtnDelete.Text = "Delete";
		this.BtnDelete.Visible = false;
		this.BtnDelete.Click += new System.EventHandler(BtnDelete_Click);
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(101, 24);
		this.btnPrint.Text = "Print-529A";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnShortCutKey.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.panel32);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdPackListHistory);
		this.tabPage2.Controls.Add(this.panel15);
		this.tabPage2.Controls.Add(this.panel3);
		this.tabPage2.Controls.Add(this.panel16);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1181, 732);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(3, 164);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.DataGridHistory).Office2007CustomColor = System.Drawing.Color.LimeGreen;
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1175, 361);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel32.BackColor = System.Drawing.Color.Teal;
		this.panel32.Controls.Add(this.label34);
		this.panel32.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel32.Location = new System.Drawing.Point(3, 525);
		this.panel32.Name = "panel32";
		this.panel32.Size = new System.Drawing.Size(1175, 25);
		this.panel32.TabIndex = 348;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label34.ForeColor = System.Drawing.Color.White;
		this.label34.Location = new System.Drawing.Point(5, 5);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(313, 17);
		this.label34.TabIndex = 562;
		this.label34.Text = "Packing List Detail of the selected above row";
		this.grdPackListHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPackListHistory).BackColor = System.Drawing.Color.White;
		this.grdPackListHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdPackListHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPackListHistory.DesignTimeLayout = grdPackListHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPackListHistory).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdPackListHistory.FilterMode = (FilterMode)2;
		this.grdPackListHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPackListHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPackListHistory.GroupByBoxVisible = false;
		this.grdPackListHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdPackListHistory).Location = new System.Drawing.Point(3, 550);
		((System.Windows.Forms.Control)(object)this.grdPackListHistory).Name = "grdPackListHistory";
		((ScrollableBase)this.grdPackListHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPackListHistory).Size = new System.Drawing.Size(1175, 179);
		((System.Windows.Forms.Control)(object)this.grdPackListHistory).TabIndex = 347;
		this.grdPackListHistory.TotalRow = (InheritableBoolean)1;
		this.grdPackListHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPackListHistory).VisualStyle = (VisualStyle)4;
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.label92);
		this.panel15.Controls.Add(this.ctrlDataGridHistory);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(3, 136);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1175, 28);
		this.panel15.TabIndex = 47;
		this.label92.AutoSize = true;
		this.label92.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label92.ForeColor = System.Drawing.Color.White;
		this.label92.Location = new System.Drawing.Point(2, 3);
		this.label92.Name = "label92";
		this.label92.Size = new System.Drawing.Size(142, 19);
		this.label92.TabIndex = 1;
		this.label92.Text = "Filtered Records";
		this.ctrlDataGridHistory.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlDataGridHistory.FormName = this;
		this.ctrlDataGridHistory.Location = new System.Drawing.Point(1127, 1);
		this.ctrlDataGridHistory.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlDataGridHistory.MyGrid = this.DataGridHistory;
		this.ctrlDataGridHistory.Name = "ctrlDataGridHistory";
		this.ctrlDataGridHistory.Size = new System.Drawing.Size(45, 25);
		this.ctrlDataGridHistory.TabIndex = 45;
		this.ctrlDataGridHistory.Load += new System.EventHandler(ctrlDataGridHistory_Load);
		this.panel3.Controls.Add(this.groupBox2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 55);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1175, 81);
		this.panel3.TabIndex = 46;
		this.groupBox2.Controls.Add(this.groupBox1);
		this.groupBox2.Controls.Add(this.rdapproveddate);
		this.groupBox2.Controls.Add(this.btnShowHistory);
		this.groupBox2.Controls.Add(this.label48);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory);
		this.groupBox2.Controls.Add(this.label20);
		this.groupBox2.Controls.Add(this.drdocdate);
		this.groupBox2.Controls.Add(this.rdentrydate);
		this.groupBox2.Controls.Add(this.label73);
		this.groupBox2.Controls.Add(this.label89);
		this.groupBox2.Controls.Add(this.label90);
		this.groupBox2.Controls.Add(this.label91);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox2.Controls.Add(this.ToDocNoHistory);
		this.groupBox2.Controls.Add(this.FromDocNoHistory);
		this.groupBox2.Controls.Add(this.ToDateHistory);
		this.groupBox2.Controls.Add(this.FromDateHistory);
		this.groupBox2.Controls.Add(this.rdmodifydate);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(6, 0);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(963, 75);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.groupBox1.Controls.Add(this.rdAll);
		this.groupBox1.Controls.Add(this.rdReferred);
		this.groupBox1.Controls.Add(this.rdNotReferred);
		this.groupBox1.Location = new System.Drawing.Point(860, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(96, 67);
		this.groupBox1.TabIndex = 11;
		this.groupBox1.TabStop = false;
		this.rdAll.AutoSize = true;
		this.rdAll.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdAll.Location = new System.Drawing.Point(6, 48);
		this.rdAll.Name = "rdAll";
		this.rdAll.Size = new System.Drawing.Size(39, 19);
		this.rdAll.TabIndex = 2;
		this.rdAll.Text = "All";
		this.rdAll.UseVisualStyleBackColor = true;
		this.rdReferred.AutoSize = true;
		this.rdReferred.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdReferred.Location = new System.Drawing.Point(5, 29);
		this.rdReferred.Name = "rdReferred";
		this.rdReferred.Size = new System.Drawing.Size(69, 19);
		this.rdReferred.TabIndex = 1;
		this.rdReferred.Text = "Referred";
		this.rdReferred.UseVisualStyleBackColor = true;
		this.rdNotReferred.AutoSize = true;
		this.rdNotReferred.Checked = true;
		this.rdNotReferred.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdNotReferred.Location = new System.Drawing.Point(5, 11);
		this.rdNotReferred.Name = "rdNotReferred";
		this.rdNotReferred.Size = new System.Drawing.Size(92, 19);
		this.rdNotReferred.TabIndex = 0;
		this.rdNotReferred.TabStop = true;
		this.rdNotReferred.Text = "Not Referred";
		this.rdNotReferred.UseVisualStyleBackColor = true;
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(754, 58);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 10;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdapproveddate.Visible = false;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(689, 44);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 6;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(373, 50);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(53, 15);
		this.label48.TabIndex = 89;
		this.label48.Text = "Invoice#";
		this.CmbInvoiceNoHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbInvoiceNoHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInvoiceNoHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInvoiceNoHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance126;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance127).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance127).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance146).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInvoiceNoHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Location = new System.Drawing.Point(434, 44);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Name = "CmbInvoiceNoHistory";
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Size = new System.Drawing.Size(254, 26);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).TabIndex = 5;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(373, 21);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(58, 15);
		this.label20.TabIndex = 87;
		this.label20.Text = "Customer";
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(754, 10);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 7;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(754, 26);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 8;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.Location = new System.Drawing.Point(213, 50);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(64, 15);
		this.label73.TabIndex = 86;
		this.label73.Text = "To Doc No";
		this.label89.AutoSize = true;
		this.label89.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label89.Location = new System.Drawing.Point(213, 21);
		this.label89.Name = "label89";
		this.label89.Size = new System.Drawing.Size(79, 15);
		this.label89.TabIndex = 85;
		this.label89.Text = "From Doc No";
		this.label90.AutoSize = true;
		this.label90.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label90.Location = new System.Drawing.Point(3, 50);
		this.label90.Name = "label90";
		this.label90.Size = new System.Drawing.Size(48, 15);
		this.label90.TabIndex = 84;
		this.label90.Text = "To Date";
		this.label91.AutoSize = true;
		this.label91.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label91.Location = new System.Drawing.Point(3, 21);
		this.label91.Name = "label91";
		this.label91.Size = new System.Drawing.Size(63, 15);
		this.label91.TabIndex = 83;
		this.label91.Text = "From Date";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance111).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance111).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance112;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance113).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance113).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance118).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance119).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(434, 15);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(254, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 4;
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(294, 46);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(294, 17);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(69, 46);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(142, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(69, 17);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(142, 23);
		this.FromDateHistory.TabIndex = 0;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(754, 42);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 9;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.label69);
		this.panel16.Controls.Add(this.toolStrip3);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(3, 3);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1175, 52);
		this.panel16.TabIndex = 0;
		this.label69.AutoSize = true;
		this.label69.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label69.ForeColor = System.Drawing.Color.White;
		this.label69.Location = new System.Drawing.Point(2, 29);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(293, 19);
		this.label69.TabIndex = 1;
		this.label69.Text = "Export Invoice Packing List History";
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnResetHistory, this.btnRefreshHistory, this.toolStripSeparator1 });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1175, 27);
		this.toolStrip3.TabIndex = 52;
		this.toolStrip3.Text = "toolStrip2";
		this.btnResetHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnResetHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetHistory.Name = "btnResetHistory";
		this.btnResetHistory.Size = new System.Drawing.Size(59, 24);
		this.btnResetHistory.Text = "&New";
		this.btnResetHistory.Click += new System.EventHandler(btnResetHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1189, 761);
		this.panel1.TabIndex = 1;
		this.cmbFCurrencr.AllowNull = (DefaultableBoolean)1;
		this.cmbFCurrencr.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbFCurrencr.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbFCurrencr.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Appearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance123;
		((SpecialBoxBase)((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance106).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance107).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance4).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbFCurrencr.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).Location = new System.Drawing.Point(385, 129);
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).Name = "cmbFCurrencr";
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).Size = new System.Drawing.Size(159, 27);
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).TabIndex = 12;
		this.cmbEFormNo.AllowNull = (DefaultableBoolean)1;
		this.cmbEFormNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEFormNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEFormNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance72).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance73).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbEFormNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEFormNo).Location = new System.Drawing.Point(662, 164);
		((System.Windows.Forms.Control)(object)this.cmbEFormNo).Name = "cmbEFormNo";
		((System.Windows.Forms.Control)(object)this.cmbEFormNo).Size = new System.Drawing.Size(164, 22);
		((System.Windows.Forms.Control)(object)this.cmbEFormNo).TabIndex = 0;
		this.miniToolStrip.AccessibleName = "New item selection";
		this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
		this.miniToolStrip.AutoSize = false;
		this.miniToolStrip.CanOverflow = false;
		this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.miniToolStrip.Location = new System.Drawing.Point(346, 4);
		this.miniToolStrip.Name = "miniToolStrip";
		this.miniToolStrip.Size = new System.Drawing.Size(280, 27);
		this.miniToolStrip.TabIndex = 0;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(35, 24);
		this.toolStripButton1.Text = "&New";
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.toolStripButton1, this.toolStripButton2, this.toolStripButton3, this.toolStripSeparator, this.toolStripButton4, this.toolStripButton5, this.helpToolStripButton });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(280, 27);
		this.toolStrip2.TabIndex = 0;
		this.toolStrip2.Text = "toolStrip1";
		this.toolStripButton2.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton2.Name = "toolStripButton2";
		this.toolStripButton2.Size = new System.Drawing.Size(55, 24);
		this.toolStripButton2.Text = "&Save";
		this.toolStripButton3.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton3.Name = "toolStripButton3";
		this.toolStripButton3.Size = new System.Drawing.Size(69, 24);
		this.toolStripButton3.Text = "&Update";
		this.toolStripButton3.Visible = false;
		this.toolStripButton4.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton4.Name = "toolStripButton4";
		this.toolStripButton4.Size = new System.Drawing.Size(94, 24);
		this.toolStripButton4.Text = "&Attachment";
		this.toolStripButton5.Image = Architecture.WinApp.Properties.Resources.icons8_messaging_16;
		this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton5.Name = "toolStripButton5";
		this.toolStripButton5.Size = new System.Drawing.Size(54, 24);
		this.toolStripButton5.Text = "&SMS";
		this.helpToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.helpToolStripButton.Image = Architecture.WinApp.Properties.Resources.icons8_help_16;
		this.helpToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.helpToolStripButton.Name = "helpToolStripButton";
		this.helpToolStripButton.Size = new System.Drawing.Size(24, 24);
		this.helpToolStripButton.Text = "He&lp";
		this.btnPrint529B.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint529B.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint529B.Name = "btnPrint529B";
		this.btnPrint529B.Size = new System.Drawing.Size(100, 24);
		this.btnPrint529B.Text = "Print-529B";
		this.btnPrint529B.Click += new System.EventHandler(btnPrint529B_Click);
		this.btnPrint529C.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint529C.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint529C.Name = "btnPrint529C";
		this.btnPrint529C.Size = new System.Drawing.Size(101, 24);
		this.btnPrint529C.Text = "Print-529C";
		this.btnPrint529C.Click += new System.EventHandler(btnPrint529C_Click);
		this.ChkSlipB.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkSlipB.AutoSize = true;
		this.ChkSlipB.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkSlipB.ForeColor = System.Drawing.Color.White;
		this.ChkSlipB.Location = new System.Drawing.Point(962, 3);
		this.ChkSlipB.Name = "ChkSlipB";
		this.ChkSlipB.Size = new System.Drawing.Size(101, 20);
		this.ChkSlipB.TabIndex = 32;
		this.ChkSlipB.Text = "Preview-B";
		this.ChkSlipB.UseVisualStyleBackColor = true;
		this.chkSlipC.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkSlipC.AutoSize = true;
		this.chkSlipC.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkSlipC.ForeColor = System.Drawing.Color.White;
		this.chkSlipC.Location = new System.Drawing.Point(1069, 3);
		this.chkSlipC.Name = "chkSlipC";
		this.chkSlipC.Size = new System.Drawing.Size(101, 20);
		this.chkSlipC.TabIndex = 32;
		this.chkSlipC.Text = "Preview-C";
		this.chkSlipC.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1189, 761);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmExportInvoicePackingList";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmExportInvoicePackingList_FormClosing);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ImProformaInvoice_KeyDown);
		this.panel4.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel38.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPackListDetail).EndInit();
		this.panel25.ResumeLayout(false);
		this.panelDetail.ResumeLayout(false);
		this.panelDetail.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemFromInvDetailPackListDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInspectionSubLotNoPackListDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInnerUomInDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOuterUomInDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInspectionNoPackListDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPmDetailByItemCusPackListDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommodityByItemCusPackListDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContainerNoPackListDetail).EndInit();
		this.panel11.ResumeLayout(false);
		this.HeaderGropBox.ResumeLayout(false);
		this.HeaderGropBox.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbInvoiceNo).EndInit();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.toolStrip4.ResumeLayout(false);
		this.toolStrip4.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel32.ResumeLayout(false);
		this.panel32.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPackListHistory).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNoHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.cmbFCurrencr).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEFormNo).EndInit();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
