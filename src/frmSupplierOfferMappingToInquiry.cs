using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.lgstcm;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.lgstcm;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.ModernHistories;
using Architecture.WinApp.Properties;
using Architecture.WinApp.Service;
using Architecture.WinApp.Service.lgstcm;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.CommissionAgent.Transactions
{

public class frmSupplierOfferMappingToInquiry : Form
{
	private WaitForm wf = new WaitForm();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtGridDbCall = new DataTable();

	private int currentPage = 1;

	private int pageSize = 20;

	private int totalRecords;

	public int DocumentTypeId;

	public int RecId;

	public int DefaultDaysToLessFromHistoryFromDate;

	public bool flagUnApprove;

	private bool FormInitialized;

	private bool HasMultiCurrencyFeature;

	private bool AscOrDesc = true;

	private List<int> ListForApproveOrUnapprove = new List<int>();

	private IContainer components;

	private Panel panel1;

	private Button btnshow;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private GroupBox GBFilters;

	private Label label10;

	private DateTimePicker GRNfromdate;

	private Label label27;

	private TextBox txtDocNoFrom;

	private TextBox txtDocNoTo;

	private DateTimePicker GRNToDate;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnShortCutKeys;

	internal UltraCombo CmbItemName;

	private Label label1;

	private FlowLayoutPanel FlpSideBar;

	internal UltraCombo CmbServiceProvider;

	private Label label7;

	internal UltraCombo CmbBrokerAgentName;

	private Label label14;

	private Panel TotalRecordPanel;

	private Label label5;

	private Label TotalHeaderRecords;

	private ToolStripButton BtnShowHideFilter;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator1;

	private Panel panel3;

	private Label labelFormHeading;

	private Panel PanelMainSelectedDocData;

	private Button BtnLoadNextRecord;

	private Button BtnLoadPreviousRecord;

	private Panel PanelHeaderInfo;

	private Label txtDocDate;

	private Label label3;

	private Label txtDocNo;

	private Label label6;

	private Label txtBuyerName;

	private Label label13;

	private Panel panel14;

	private Panel PanelMainSelectedNetPreviousDocData;

	private Button BtnShowPreviousRecord;

	private Button BtnShowNextRecord;

	private Label label19;

	private Button BtnApproveThis;

	private Label label11;

	private Label txtBrokerOrAgent;

	private Label txtCompanyName;

	private Label txtDueDays;

	private Label txtPaymentTerm;

	private Label label9;

	private Label label8;

	private Label label16;

	private Label txtRemarks;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator4;

	private Button BtnGoToFirst;

	private Button BtnGoToLast;

	private Label label17;

	internal GridEX grdSupplierOffers;

	private Label label2;

	private Label label4;

	private Label label18;

	private Label label20;

	private Label label25;

	private Label label26;

	private Label txtValidityUptoDate;

	private Label txtExpiryDate;

	private Label txtDeliveryStartDate;

	private Label txtDeliveryDays;

	private Label txtDeliveryTerm;

	private Label Line01;

	private Label label12;

	private TextBox textBox9;

	private Label label24;

	private TextBox textBox8;

	private Label label22;

	private Label label21;

	private TextBox txt;

	private Label label15;

	private Label label23;

	private Label label28;

	private Label label29;

	private Label label30;

	private Label label31;

	private Label label34;

	private Label label33;

	private Label label32;

	private Label label35;

	private Label label38;

	private Label label37;

	private Label label36;

	private Button BtnAddMoreOffers;

	private Label label39;

	private ToolStripButton BtnSorting;

	private Label txtValidityDays;

	public frmSupplierOfferMappingToInquiry(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InitializeComponentMethod();
	}

	private void frmSupplierOfferMappingToInquiry_Load(object sender, EventArgs e)
	{
		AutoScroll = true;
		base.AutoScrollMargin = new Size(20, 20);
		base.AutoScrollMinSize = new Size(1120, 740);
	}

	private void InitializeComponentMethod()
	{
		Task.Run(delegate
		{
			DataTable historyComboData = HistoryComboDbCall();
			if (DocumentTypeId > 0)
			{
				MainGridDbCall(Reset: false);
			}
			return new
			{
				HistoryComboData = historyComboData
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
				HasMultiCurrencyFeature = CommonServices.GetERPFeatureById(6);
				string text = ((DocumentTypeId == 155) ? "Logistics Services Bill Direct" : "Logistics Services Bill");
				if (flagUnApprove)
				{
					labelFormHeading.Text = "Pending '" + text + "' For UnApproval";
				}
				else
				{
					labelFormHeading.Text = "Pending '" + text + "' For Approval";
				}
				if (DocumentTypeId > 0)
				{
					GenerateCardInSideBarLayout(AscOrDesc);
				}
				GRNfromdate.Value = clsGlobalVariables.ActiveYr.Start_Period;
				HistoryComboBind(result.HistoryComboData);
				FormInitialized = true;
				GRNfromdate.Focus();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private DataTable HistoryComboDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		try
		{
			return ServicesBillHeader.GetDataForDropDownFromServicesBillHeader(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeIds = DocumentTypeId.ToString()
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
			DataTable dtCust = new DataTable();
			dtCust.Columns.Add("Id", typeof(int));
			dtCust.Columns.Add("Name", typeof(string));
			DataTable dtSP = dtCust.Clone();
			DataTable dtItem = dtCust.Clone();
			DataTable dtST = dtCust.Clone();
			DataTable dtInv = dtCust.Clone();
			foreach (DataRow dr in dt.Rows)
			{
				switch (Conversion.ToString(dr["Activity"]))
				{
				case "BillToPartyName":
					dtCust.Rows.Add(dr["Id"], dr["ReferenceName"]);
					break;
				case "BrokerOrServiceProvider":
					dtSP.Rows.Add(dr["Id"], dr["ReferenceName"]);
					break;
				case "ServiceItem":
					dtItem.Rows.Add(dr["Id"], dr["ReferenceName"]);
					break;
				case "ServiceType":
					dtST.Rows.Add(dr["Id"], dr["ReferenceName"]);
					break;
				case "ExportInvoiceNo":
					dtInv.Rows.Add(dr["Id"], dr["ReferenceName"]);
					break;
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbBrokerAgentName, dtCust, "Id", "Name", "Broker Agent Name", AllColumns: true, Conversion.ToInt(CmbBrokerAgentName.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbServiceProvider, dtSP, "Id", "Name", "Service Provider", AllColumns: true, Conversion.ToInt(CmbServiceProvider.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbItemName, dtItem, "Id", "Name", "Item Name", AllColumns: true, Conversion.ToInt(CmbItemName.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void Reset()
	{
		try
		{
			GRNfromdate.Focus();
			txtDocNoFrom.Text = string.Empty;
			txtDocNoTo.Text = string.Empty;
			((Control)(object)CmbBrokerAgentName).Text = string.Empty;
			((Control)(object)CmbServiceProvider).Text = string.Empty;
			((Control)(object)CmbItemName).Text = string.Empty;
			((Control)(object)CmbServiceProvider).Text = string.Empty;
			FromReset();
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
			HistoryComboBind(HistoryComboDbCall());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShowHideFilter_Click(object sender, EventArgs e)
	{
		try
		{
			GBFilters.Visible = !GBFilters.Visible;
			BtnShowHideFilter.Image = (GBFilters.Visible ? Resources.hide : Resources.show);
			BtnShowHideFilter.ToolTipText = (GBFilters.Visible ? "Hide Filters" : "Show Filters");
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
			wf.Show();
			MainGridDbCall();
			GenerateCardInSideBarLayout(AscOrDesc);
			wf.Close();
		}
		catch (Exception ex)
		{
			wf.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			wf.Close();
		}
	}

	public void MainGridDbCall(bool Reset = true)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		try
		{
			if (Reset)
			{
				FromReset();
			}
			dtGridDbCall.Clear();
			ReportsParameters grn = new ReportsParameters();
			grn.OrganizationId = UserAccount.OrganizationId;
			grn.CompanyId = UserAccount.CompanyId;
			grn.BranchesId = UserAccount.BranchesId;
			grn.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			grn.DocumentTypeId = DocumentTypeId;
			grn.FromDate = ((!Reset) ? clsGlobalVariables.ActiveYr.Start_Period : Conversion.ToDateTime((object)GRNfromdate.Value));
			grn.ToDate = Conversion.ToDateTime((object)GRNToDate.Value);
			grn.FromDocNo = Conversion.ToInt((object)txtDocNoFrom.Text.Trim());
			grn.ToDocNo = Conversion.ToInt((object)txtDocNoTo.Text.Trim());
			grn.BillToPartyId = Conversion.ToInt(CmbBrokerAgentName.Value);
			grn.BrokerOrServiceProviderId = Conversion.ToInt(CmbServiceProvider.Value);
			grn.ItemId = Conversion.ToInt(CmbItemName.Value);
			grn.IsApproved = flagUnApprove;
			dtGridDbCall = ServicesBillHeader.ServicesBill_DataForApproval(grn);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateCardInSideBarLayout(bool AscOrDesc)
	{
		try
		{
			FlpSideBar.SuspendLayout();
			FlpSideBar.Controls.Clear();
			FlpSideBar.AutoScroll = true;
			IEnumerable<DataRow> distinctRows = from r in dtGridDbCall.AsEnumerable()
				group r by r.Field<int>("ServicesBillHeaderId") into g
				select g.First();
			distinctRows = (AscOrDesc ? distinctRows.OrderBy((DataRow r) => r.Field<int>("documentNo")) : distinctRows.OrderByDescending((DataRow r) => r.Field<int>("documentNo")));
			List<DataRow> distinctList = distinctRows.ToList();
			if (totalRecords == 0)
			{
				totalRecords = distinctList.Count;
			}
			int skip = (currentPage - 1) * pageSize;
			foreach (DataRow row in distinctList.Skip(skip).Take(pageSize))
			{
				int docId = Conversion.ToInt(row["ServicesBillHeaderId"]);
				ApprovalSideBarMainCard card = new ApprovalSideBarMainCard
				{
					DocumentId = docId,
					Width = 350,
					Height = 140
				};
				card.chkSelectionWithMainText.Text = Conversion.ToString(row["BillToPartyName"]);
				card.FlpMain.SuspendLayout();
				card.FlpMain.Controls.Clear();
				DateTime docDate = Conversion.ToDateTime(row["documentDate"]);
				DateTime entryDate = Conversion.ToDateTime(row["EntryDate"]);
				DateTime modifyDate = Conversion.ToDateTime(row["ModifyDate"]);
				ApprovalFormHelper.DynamicaCardRowRender(card.FlpMain, new CardRowModel
				{
					Col1 = ApprovalFormHelper.DefaultLabelModel("Doc No:"),
					Col2 = ApprovalFormHelper.DefaultLinkModel(Conversion.ToString(row["documentNo"]), delegate
					{
						if (DocumentTypeId == 155)
						{
							CommonServices.ServicesBillSlip540(docId);
						}
						else
						{
							CrystalReportPrint_Helper.ServicesBillHeader_Slip_1304(docId);
						}
					}),
					Col3 = ApprovalFormHelper.DefaultLabelModel("Doc Date:"),
					Col4 = ApprovalFormHelper.DefaultValueModel(docDate.ToString("dd-MMM-yyyy")),
					RowHeight = 20
				});
				ApprovalFormHelper.DynamicaCardRowRender(card.FlpMain, new CardRowModel
				{
					Col1 = ApprovalFormHelper.DefaultLabelModel("Entry User:"),
					Col2 = ApprovalFormHelper.DefaultValueModel(Conversion.ToString(row["EntryUserName"])),
					Col3 = ApprovalFormHelper.DefaultLabelModel("Entry Date:"),
					Col4 = ApprovalFormHelper.DefaultValueModel(entryDate.ToString("dd-MMM-yyyy")),
					RowHeight = 20
				});
				ApprovalFormHelper.DynamicaCardRowRender(card.FlpMain, new CardRowModel
				{
					Col1 = ApprovalFormHelper.DefaultLabelModel("Modify User:"),
					Col2 = ApprovalFormHelper.DefaultValueModel(Conversion.ToString(row["ModifyUserName"])),
					Col3 = ApprovalFormHelper.DefaultLabelModel("Modify Date:"),
					Col4 = ApprovalFormHelper.DefaultValueModel(Conversion.CheckDateTimeNull(modifyDate) ? "" : modifyDate.ToString("dd-MMM-yyyy")),
					RowHeight = 20
				});
				ApprovalFormHelper.SetupCardTableLayout(card.FlpMain, 76, 100, 76, 80);
				card.FlpMain.ResumeLayout();
				card.UserControlClicked += HeaderCard_Click;
				FlpSideBar.Controls.Add(card);
			}
			TotalHeaderRecords.Text = $"{Math.Min(currentPage * pageSize, totalRecords)} / {totalRecords}";
			BtnLoadPreviousRecord.Enabled = currentPage > 1;
			BtnLoadNextRecord.Enabled = currentPage * pageSize < totalRecords;
			BtnGoToFirst.Enabled = currentPage > 1;
			BtnGoToLast.Enabled = currentPage * pageSize < totalRecords;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		finally
		{
			FlpSideBar.ResumeLayout();
		}
	}

	private void BtnLoadPreviousRecord_Click(object sender, EventArgs e)
	{
		if (currentPage > 1)
		{
			currentPage--;
			GenerateCardInSideBarLayout(AscOrDesc);
		}
	}

	private void BtnLoadNextRecord_Click(object sender, EventArgs e)
	{
		if (currentPage * pageSize < totalRecords)
		{
			currentPage++;
			GenerateCardInSideBarLayout(AscOrDesc);
		}
	}

	private void BtnGoToFirst_Click(object sender, EventArgs e)
	{
		currentPage = 1;
		GenerateCardInSideBarLayout(AscOrDesc);
	}

	private void BtnGoToLast_Click(object sender, EventArgs e)
	{
		currentPage = (int)Math.Ceiling((double)totalRecords / (double)pageSize);
		GenerateCardInSideBarLayout(AscOrDesc);
	}

	private void BtnSorting_Click(object sender, EventArgs e)
	{
		try
		{
			if (FlpSideBar.Controls.Count == 0)
			{
				MessageBox.Show("No cards available For Sorting.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}
			AscOrDesc = !AscOrDesc;
			GenerateCardInSideBarLayout(AscOrDesc);
			BtnSorting.Image = ((!AscOrDesc) ? Resources.SortAsc : Resources.SortDesc);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HeaderCard_Click(object sender, EventArgs e)
	{
		try
		{
			ApprovalSideBarMainCard card = null;
			if (sender is ApprovalSideBarMainCard mainCard)
			{
				card = mainCard;
			}
			else if (sender is Control ctrl)
			{
				card = ctrl.Parent as ApprovalSideBarMainCard;
			}
			if (card != null)
			{
				ReadById(card.DocumentId);
			}
			else
			{
				MessageBox.Show("Could not identify the clicked card.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ReadById(int Id)
	{
		try
		{
			FromReset();
			RecId = Id;
			ServicesBillHeader Or = ServicesBillHeader.ReadById(RecId);
			if (Or == null || Or.ServicesBillDetailList == null)
			{
				MessageBox.Show("No Record Found");
				return;
			}
			txtDocDate.Text = Or.DocumentDate.ToString("d");
			txtDocNo.Text = Conversion.ToString((object)Or.DocumentNo);
			txtCompanyName.Text = Conversion.ToString((object)Or.BilltoPartyName);
			txtBrokerOrAgent.Text = Conversion.ToString((object)Or.BrokerOrServiceProviderName);
			txtBuyerName.Text = Conversion.ToString((object)Or.ServiceType);
			txtPaymentTerm.Text = Conversion.ToString((object)Or.PaymentTerm);
			txtDueDays.Text = Or.DueDays.ToString("#,#0");
			txtRemarks.Text = Conversion.ToString((object)Or.RemarksHeader);
			grdSettingSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingSettings()
	{
	}

	private void FromReset()
	{
		try
		{
			RecId = 0;
			currentPage = 1;
			pageSize = 20;
			totalRecords = 0;
			txtDocNo.Text = "0";
			txtDocDate.Text = "None";
			txtCompanyName.Text = "None";
			txtBrokerOrAgent.Text = "None";
			txtBuyerName.Text = "None";
			txtPaymentTerm.Text = "None";
			txtDueDays.Text = "0";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDocNo_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				MessageBox.Show("No record selected Yet.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else if (DocumentTypeId == 155)
			{
				FormHelper.CanOpenForm("ExImClearingAgentBillDirect", delegate
				{
					ExImClearingAgentBillDirect exImClearingAgentBillDirect = new ExImClearingAgentBillDirect(UserAccount);
					exImClearingAgentBillDirect.Show();
					exImClearingAgentBillDirect.ReadById(RecId);
					exImClearingAgentBillDirect.BringToFront();
				}, UserAccount);
			}
			else
			{
				FormHelper.CanOpenForm("frmLogisticPurchaseServicesBill", delegate
				{
					frmLogisticPurchaseServicesBill frmLogisticPurchaseServicesBill = new frmLogisticPurchaseServicesBill(UserAccount);
					frmLogisticPurchaseServicesBill.Show();
					frmLogisticPurchaseServicesBill.ReadById(RecId);
					frmLogisticPurchaseServicesBill.BringToFront();
				}, UserAccount);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShowNextRecord_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				MessageBox.Show("No record selected Yet.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			int currDocNo = Conversion.ToInt((object)txtDocNo.Text);
			int nextDocNo = currDocNo + 1;
			DataRow nextRecord = (from row in dtGridDbCall.AsEnumerable()
				select new
				{
					Row = row,
					DocNo = Convert.ToInt32(row["documentNo"])
				} into x
				where x.DocNo > nextDocNo
				orderby x.DocNo
				select x.Row).FirstOrDefault();
			if (nextRecord != null)
			{
				int nextDocId = Conversion.ToInt(nextRecord["ServicesBillHeaderId"]);
				ReadById(nextDocId);
				return;
			}
			throw new Exception("No Next Record Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShowPreviousRecord_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				MessageBox.Show("No record selected Yet.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			int currDocNo = Conversion.ToInt((object)txtDocNo.Text);
			int PreviousDocNo = currDocNo - 1;
			if (currDocNo > 1)
			{
				DataRow previousRecord = (from row in dtGridDbCall.AsEnumerable()
					select new
					{
						Row = row,
						DocNo = Convert.ToInt32(row["documentNo"])
					} into x
					where x.DocNo < PreviousDocNo
					orderby x.DocNo
					select x.Row).FirstOrDefault();
				if (previousRecord != null)
				{
					int PreviousDocId = Conversion.ToInt(previousRecord["ServicesBillHeaderId"]);
					ReadById(PreviousDocId);
					return;
				}
				throw new Exception("No Previous Record Found");
			}
			throw new Exception("Already at the first record");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmPurchaseInvoiceHistory_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				GRNfromdate.Focus();
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Control)
			{
				_ = e.KeyCode;
				_ = 40;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortCutKeys_Click(object sender, EventArgs e)
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
			dt.Rows.Add("Ctrl+S", "For Show Record");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+F5", "For Focus on Combo Date");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid History");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Date combo in Filters Box");
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
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Expected O, but got Unknown
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Expected O, but got Unknown
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Expected O, but got Unknown
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
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
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
		GridEXLayout grdSupplierOffers_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.CommissionAgent.Transactions.frmSupplierOfferMappingToInquiry));
		this.panel1 = new System.Windows.Forms.Panel();
		this.FlpSideBar = new System.Windows.Forms.FlowLayoutPanel();
		this.GBFilters = new System.Windows.Forms.GroupBox();
		this.CmbServiceProvider = new UltraCombo();
		this.CmbBrokerAgentName = new UltraCombo();
		this.txtDocNoFrom = new System.Windows.Forms.TextBox();
		this.CmbItemName = new UltraCombo();
		this.GRNfromdate = new System.Windows.Forms.DateTimePicker();
		this.txtDocNoTo = new System.Windows.Forms.TextBox();
		this.GRNToDate = new System.Windows.Forms.DateTimePicker();
		this.label7 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.label10 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnShowHideFilter = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnSorting = new System.Windows.Forms.ToolStripButton();
		this.TotalRecordPanel = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.TotalHeaderRecords = new System.Windows.Forms.Label();
		this.BtnGoToFirst = new System.Windows.Forms.Button();
		this.BtnGoToLast = new System.Windows.Forms.Button();
		this.BtnLoadPreviousRecord = new System.Windows.Forms.Button();
		this.BtnLoadNextRecord = new System.Windows.Forms.Button();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label17 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.labelFormHeading = new System.Windows.Forms.Label();
		this.PanelMainSelectedDocData = new System.Windows.Forms.Panel();
		this.grdSupplierOffers = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label19 = new System.Windows.Forms.Label();
		this.BtnAddMoreOffers = new System.Windows.Forms.Button();
		this.PanelHeaderInfo = new System.Windows.Forms.Panel();
		this.label39 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.textBox9 = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.textBox8 = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.txt = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.txtBrokerOrAgent = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.txtExpiryDate = new System.Windows.Forms.Label();
		this.txtDeliveryStartDate = new System.Windows.Forms.Label();
		this.txtDeliveryDays = new System.Windows.Forms.Label();
		this.txtDeliveryTerm = new System.Windows.Forms.Label();
		this.txtDueDays = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.txtPaymentTerm = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.txtDocNo = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.txtValidityUptoDate = new System.Windows.Forms.Label();
		this.txtDocDate = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.Line01 = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.txtBuyerName = new System.Windows.Forms.Label();
		this.PanelMainSelectedNetPreviousDocData = new System.Windows.Forms.Panel();
		this.BtnShowPreviousRecord = new System.Windows.Forms.Button();
		this.BtnApproveThis = new System.Windows.Forms.Button();
		this.BtnShowNextRecord = new System.Windows.Forms.Button();
		this.txtCompanyName = new System.Windows.Forms.Label();
		this.txtValidityDays = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.GBFilters.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbServiceProvider).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokerAgentName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		this.toolStrip3.SuspendLayout();
		this.TotalRecordPanel.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel3.SuspendLayout();
		this.PanelMainSelectedDocData.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSupplierOffers).BeginInit();
		this.panel14.SuspendLayout();
		this.PanelHeaderInfo.SuspendLayout();
		this.PanelMainSelectedNetPreviousDocData.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.FlpSideBar);
		this.panel1.Controls.Add(this.GBFilters);
		this.panel1.Controls.Add(this.toolStrip3);
		this.panel1.Controls.Add(this.TotalRecordPanel);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel1.Location = new System.Drawing.Point(0, 24);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(380, 687);
		this.panel1.TabIndex = 0;
		this.FlpSideBar.Dock = System.Windows.Forms.DockStyle.Fill;
		this.FlpSideBar.Location = new System.Drawing.Point(0, 209);
		this.FlpSideBar.Name = "FlpSideBar";
		this.FlpSideBar.Size = new System.Drawing.Size(380, 442);
		this.FlpSideBar.TabIndex = 1;
		this.GBFilters.BackColor = System.Drawing.Color.White;
		this.GBFilters.Controls.Add((System.Windows.Forms.Control)(object)this.CmbServiceProvider);
		this.GBFilters.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrokerAgentName);
		this.GBFilters.Controls.Add(this.txtDocNoFrom);
		this.GBFilters.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.GBFilters.Controls.Add(this.GRNfromdate);
		this.GBFilters.Controls.Add(this.txtDocNoTo);
		this.GBFilters.Controls.Add(this.GRNToDate);
		this.GBFilters.Controls.Add(this.label7);
		this.GBFilters.Controls.Add(this.label14);
		this.GBFilters.Controls.Add(this.btnshow);
		this.GBFilters.Controls.Add(this.label10);
		this.GBFilters.Controls.Add(this.label1);
		this.GBFilters.Controls.Add(this.label27);
		this.GBFilters.Dock = System.Windows.Forms.DockStyle.Top;
		this.GBFilters.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.GBFilters.ForeColor = System.Drawing.Color.Maroon;
		this.GBFilters.Location = new System.Drawing.Point(0, 62);
		this.GBFilters.Name = "GBFilters";
		this.GBFilters.Size = new System.Drawing.Size(380, 147);
		this.GBFilters.TabIndex = 0;
		this.GBFilters.TabStop = false;
		this.GBFilters.Text = "Filters";
		this.CmbServiceProvider.AllowNull = (DefaultableBoolean)1;
		this.CmbServiceProvider.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbServiceProvider.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbServiceProvider.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance178).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Appearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbServiceProvider).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance180;
		((SpecialBoxBase)((UltraGridBase)this.CmbServiceProvider).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance181).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance181).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance181).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance182).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance182;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance183).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance184;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance185).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance186).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance186).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance186).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance187).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbServiceProvider).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbServiceProvider).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbServiceProvider.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbServiceProvider).Location = new System.Drawing.Point(86, 88);
		((System.Windows.Forms.Control)(object)this.CmbServiceProvider).Name = "CmbServiceProvider";
		((System.Windows.Forms.Control)(object)this.CmbServiceProvider).Size = new System.Drawing.Size(262, 26);
		((System.Windows.Forms.Control)(object)this.CmbServiceProvider).TabIndex = 5;
		this.CmbBrokerAgentName.AllowNull = (DefaultableBoolean)1;
		this.CmbBrokerAgentName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrokerAgentName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrokerAgentName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance168;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance169).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance169).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance169).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance172;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance173).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance174).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance174).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance174).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance174;
		((AppearanceBase)appearance175).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrokerAgentName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrokerAgentName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrokerAgentName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrokerAgentName).Location = new System.Drawing.Point(86, 60);
		((System.Windows.Forms.Control)(object)this.CmbBrokerAgentName).Name = "CmbBrokerAgentName";
		((System.Windows.Forms.Control)(object)this.CmbBrokerAgentName).Size = new System.Drawing.Size(262, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrokerAgentName).TabIndex = 4;
		this.txtDocNoFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoFrom.Location = new System.Drawing.Point(86, 35);
		this.txtDocNoFrom.Margin = new System.Windows.Forms.Padding(4);
		this.txtDocNoFrom.Name = "txtDocNoFrom";
		this.txtDocNoFrom.ShortcutsEnabled = false;
		this.txtDocNoFrom.Size = new System.Drawing.Size(130, 23);
		this.txtDocNoFrom.TabIndex = 2;
		this.txtDocNoFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance190).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance191).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance191).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance191;
		((AppearanceBase)appearance192).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance192;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance193).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance193).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance193).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance194).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance195).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance196;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance197).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance198).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance198).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance198).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance198;
		((AppearanceBase)appearance199).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance200).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(86, 116);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(206, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 6;
		this.GRNfromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNfromdate.CustomFormat = "dd-MMM-yy";
		this.GRNfromdate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNfromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.GRNfromdate.Location = new System.Drawing.Point(86, 12);
		this.GRNfromdate.Margin = new System.Windows.Forms.Padding(4);
		this.GRNfromdate.Name = "GRNfromdate";
		this.GRNfromdate.Size = new System.Drawing.Size(130, 22);
		this.GRNfromdate.TabIndex = 0;
		this.txtDocNoTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoTo.Location = new System.Drawing.Point(218, 35);
		this.txtDocNoTo.Margin = new System.Windows.Forms.Padding(4);
		this.txtDocNoTo.Name = "txtDocNoTo";
		this.txtDocNoTo.ShortcutsEnabled = false;
		this.txtDocNoTo.Size = new System.Drawing.Size(130, 23);
		this.txtDocNoTo.TabIndex = 3;
		this.txtDocNoTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.GRNToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNToDate.CustomFormat = "dd-MMM-yy";
		this.GRNToDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.GRNToDate.Location = new System.Drawing.Point(218, 12);
		this.GRNToDate.Margin = new System.Windows.Forms.Padding(4);
		this.GRNToDate.Name = "GRNToDate";
		this.GRNToDate.Size = new System.Drawing.Size(130, 22);
		this.GRNToDate.TabIndex = 1;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.Black;
		this.label7.Location = new System.Drawing.Point(3, 94);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(84, 15);
		this.label7.TabIndex = 635;
		this.label7.Text = "Agent / Broker";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.Black;
		this.label14.Location = new System.Drawing.Point(5, 66);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(72, 15);
		this.label14.TabIndex = 636;
		this.label14.Text = "Buyer Name";
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(293, 116);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(55, 26);
		this.btnshow.TabIndex = 9;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.Black;
		this.label10.Location = new System.Drawing.Point(5, 39);
		this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(76, 15);
		this.label10.TabIndex = 144;
		this.label10.Text = "From,To Doc";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Black;
		this.label1.Location = new System.Drawing.Point(5, 122);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(67, 15);
		this.label1.TabIndex = 121;
		this.label1.Text = "Item Name";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.ForeColor = System.Drawing.Color.Black;
		this.label27.Location = new System.Drawing.Point(5, 16);
		this.label27.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(79, 15);
		this.label27.TabIndex = 119;
		this.label27.Text = "From,To Date";
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.btnNew, this.toolStripSeparator5, this.btnRefresh, this.toolStripSeparator4, this.btnShortCutKeys, this.toolStripSeparator2, this.BtnShowHideFilter, this.toolStripSeparator1, this.BtnSorting });
		this.toolStrip3.Location = new System.Drawing.Point(0, 35);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(380, 27);
		this.toolStrip3.TabIndex = 10;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(24, 24);
		this.btnNew.Text = "&New";
		this.btnNew.ToolTipText = "Click To Reset Default";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(24, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.ToolTipText = "Click To Refresh Filters";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.btnShortCutKeys.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.btnShortCutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeys.Name = "btnShortCutKeys";
		this.btnShortCutKeys.Size = new System.Drawing.Size(24, 24);
		this.btnShortCutKeys.Text = "ShortCutKeys";
		this.btnShortCutKeys.ToolTipText = "Click to See ShortCutKeys";
		this.btnShortCutKeys.Click += new System.EventHandler(btnShortCutKeys_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.BtnShowHideFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.BtnShowHideFilter.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShowHideFilter.Image = Architecture.WinApp.Properties.Resources.hide;
		this.BtnShowHideFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShowHideFilter.Name = "BtnShowHideFilter";
		this.BtnShowHideFilter.Size = new System.Drawing.Size(24, 24);
		this.BtnShowHideFilter.Text = " ";
		this.BtnShowHideFilter.ToolTipText = "Hide Filters";
		this.BtnShowHideFilter.Click += new System.EventHandler(BtnShowHideFilter_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.BtnSorting.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.BtnSorting.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnSorting.Image = Architecture.WinApp.Properties.Resources.SortDesc;
		this.BtnSorting.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnSorting.Name = "BtnSorting";
		this.BtnSorting.Size = new System.Drawing.Size(24, 24);
		this.BtnSorting.Text = " ";
		this.BtnSorting.ToolTipText = "Hide Filters";
		this.BtnSorting.Click += new System.EventHandler(BtnSorting_Click);
		this.TotalRecordPanel.BackColor = System.Drawing.Color.White;
		this.TotalRecordPanel.Controls.Add(this.label5);
		this.TotalRecordPanel.Controls.Add(this.TotalHeaderRecords);
		this.TotalRecordPanel.Controls.Add(this.BtnGoToFirst);
		this.TotalRecordPanel.Controls.Add(this.BtnGoToLast);
		this.TotalRecordPanel.Controls.Add(this.BtnLoadPreviousRecord);
		this.TotalRecordPanel.Controls.Add(this.BtnLoadNextRecord);
		this.TotalRecordPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.TotalRecordPanel.Location = new System.Drawing.Point(0, 651);
		this.TotalRecordPanel.Name = "TotalRecordPanel";
		this.TotalRecordPanel.Size = new System.Drawing.Size(380, 36);
		this.TotalRecordPanel.TabIndex = 2;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(100, 6);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(112, 20);
		this.label5.TabIndex = 1;
		this.label5.Text = "Total Records: ";
		this.TotalHeaderRecords.AutoSize = true;
		this.TotalHeaderRecords.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.TotalHeaderRecords.Location = new System.Drawing.Point(208, 6);
		this.TotalHeaderRecords.Name = "TotalHeaderRecords";
		this.TotalHeaderRecords.Size = new System.Drawing.Size(18, 20);
		this.TotalHeaderRecords.TabIndex = 2;
		this.TotalHeaderRecords.Text = "0";
		this.BtnGoToFirst.BackColor = System.Drawing.Color.Transparent;
		this.BtnGoToFirst.FlatAppearance.BorderSize = 0;
		this.BtnGoToFirst.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnGoToFirst.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnGoToFirst.ForeColor = System.Drawing.Color.White;
		this.BtnGoToFirst.Image = Architecture.WinApp.Properties.Resources.GoToFirst;
		this.BtnGoToFirst.Location = new System.Drawing.Point(47, 1);
		this.BtnGoToFirst.Margin = new System.Windows.Forms.Padding(2);
		this.BtnGoToFirst.Name = "BtnGoToFirst";
		this.BtnGoToFirst.Size = new System.Drawing.Size(41, 31);
		this.BtnGoToFirst.TabIndex = 8;
		this.BtnGoToFirst.UseVisualStyleBackColor = false;
		this.BtnGoToFirst.Click += new System.EventHandler(BtnGoToFirst_Click);
		this.BtnGoToLast.BackColor = System.Drawing.Color.Transparent;
		this.BtnGoToLast.FlatAppearance.BorderSize = 0;
		this.BtnGoToLast.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnGoToLast.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnGoToLast.ForeColor = System.Drawing.Color.White;
		this.BtnGoToLast.Image = Architecture.WinApp.Properties.Resources.download_removebg_previewd;
		this.BtnGoToLast.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.BtnGoToLast.Location = new System.Drawing.Point(298, 1);
		this.BtnGoToLast.Margin = new System.Windows.Forms.Padding(2);
		this.BtnGoToLast.Name = "BtnGoToLast";
		this.BtnGoToLast.Size = new System.Drawing.Size(41, 31);
		this.BtnGoToLast.TabIndex = 8;
		this.BtnGoToLast.UseVisualStyleBackColor = false;
		this.BtnGoToLast.Click += new System.EventHandler(BtnGoToLast_Click);
		this.BtnLoadPreviousRecord.BackColor = System.Drawing.Color.Transparent;
		this.BtnLoadPreviousRecord.FlatAppearance.BorderSize = 0;
		this.BtnLoadPreviousRecord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnLoadPreviousRecord.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnLoadPreviousRecord.ForeColor = System.Drawing.Color.White;
		this.BtnLoadPreviousRecord.Image = Architecture.WinApp.Properties.Resources.arrow_left_line;
		this.BtnLoadPreviousRecord.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnLoadPreviousRecord.Location = new System.Drawing.Point(3, 1);
		this.BtnLoadPreviousRecord.Margin = new System.Windows.Forms.Padding(2);
		this.BtnLoadPreviousRecord.Name = "BtnLoadPreviousRecord";
		this.BtnLoadPreviousRecord.Size = new System.Drawing.Size(38, 31);
		this.BtnLoadPreviousRecord.TabIndex = 8;
		this.BtnLoadPreviousRecord.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnLoadPreviousRecord.UseVisualStyleBackColor = false;
		this.BtnLoadPreviousRecord.Click += new System.EventHandler(BtnLoadPreviousRecord_Click);
		this.BtnLoadNextRecord.BackColor = System.Drawing.Color.Transparent;
		this.BtnLoadNextRecord.FlatAppearance.BorderSize = 0;
		this.BtnLoadNextRecord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnLoadNextRecord.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnLoadNextRecord.ForeColor = System.Drawing.Color.White;
		this.BtnLoadNextRecord.Image = Architecture.WinApp.Properties.Resources.arrow_right_line1;
		this.BtnLoadNextRecord.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.BtnLoadNextRecord.Location = new System.Drawing.Point(337, 1);
		this.BtnLoadNextRecord.Margin = new System.Windows.Forms.Padding(2);
		this.BtnLoadNextRecord.Name = "BtnLoadNextRecord";
		this.BtnLoadNextRecord.Size = new System.Drawing.Size(38, 31);
		this.BtnLoadNextRecord.TabIndex = 8;
		this.BtnLoadNextRecord.UseVisualStyleBackColor = false;
		this.BtnLoadNextRecord.Click += new System.EventHandler(BtnLoadNextRecord_Click);
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.label17);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Font = new System.Drawing.Font("Verdana", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(380, 35);
		this.panel2.TabIndex = 1;
		this.panel2.Tag = "";
		this.label17.AutoSize = true;
		this.label17.BackColor = System.Drawing.Color.Transparent;
		this.label17.Font = new System.Drawing.Font("Tahoma", 16f, System.Drawing.FontStyle.Bold);
		this.label17.ForeColor = System.Drawing.Color.Teal;
		this.label17.Location = new System.Drawing.Point(3, 3);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(248, 27);
		this.label17.TabIndex = 4;
		this.label17.Text = "Outstanding Inquires";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.labelFormHeading);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1184, 24);
		this.panel3.TabIndex = 52;
		this.labelFormHeading.AutoSize = true;
		this.labelFormHeading.BackColor = System.Drawing.Color.Teal;
		this.labelFormHeading.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelFormHeading.ForeColor = System.Drawing.Color.White;
		this.labelFormHeading.Location = new System.Drawing.Point(3, 2);
		this.labelFormHeading.Name = "labelFormHeading";
		this.labelFormHeading.Size = new System.Drawing.Size(287, 19);
		this.labelFormHeading.TabIndex = 4;
		this.labelFormHeading.Text = "Supplier-Offer Mapping To Inquiry";
		this.PanelMainSelectedDocData.Controls.Add((System.Windows.Forms.Control)(object)this.grdSupplierOffers);
		this.PanelMainSelectedDocData.Controls.Add(this.panel14);
		this.PanelMainSelectedDocData.Controls.Add(this.PanelHeaderInfo);
		this.PanelMainSelectedDocData.Controls.Add(this.PanelMainSelectedNetPreviousDocData);
		this.PanelMainSelectedDocData.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PanelMainSelectedDocData.Location = new System.Drawing.Point(380, 24);
		this.PanelMainSelectedDocData.Name = "PanelMainSelectedDocData";
		this.PanelMainSelectedDocData.Size = new System.Drawing.Size(804, 687);
		this.PanelMainSelectedDocData.TabIndex = 53;
		this.grdSupplierOffers.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdSupplierOffers).BackColor = System.Drawing.Color.White;
		grdSupplierOffers_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdSupplierOffers.DesignTimeLayout = grdSupplierOffers_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSupplierOffers).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdSupplierOffers).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSupplierOffers.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdSupplierOffers).Location = new System.Drawing.Point(0, 383);
		((System.Windows.Forms.Control)(object)this.grdSupplierOffers).Name = "grdSupplierOffers";
		((ScrollableBase)this.grdSupplierOffers).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSupplierOffers).Size = new System.Drawing.Size(804, 304);
		((System.Windows.Forms.Control)(object)this.grdSupplierOffers).TabIndex = 135;
		this.grdSupplierOffers.TotalRow = (InheritableBoolean)1;
		this.grdSupplierOffers.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSupplierOffers).VisualStyle = (VisualStyle)4;
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.label19);
		this.panel14.Controls.Add(this.BtnAddMoreOffers);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 359);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(804, 24);
		this.panel14.TabIndex = 133;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.White;
		this.label19.Location = new System.Drawing.Point(5, 2);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(103, 16);
		this.label19.TabIndex = 126;
		this.label19.Text = "Supplier Offers";
		this.BtnAddMoreOffers.BackColor = System.Drawing.Color.SeaGreen;
		this.BtnAddMoreOffers.FlatAppearance.BorderColor = System.Drawing.Color.Black;
		this.BtnAddMoreOffers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAddMoreOffers.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnAddMoreOffers.ForeColor = System.Drawing.Color.White;
		this.BtnAddMoreOffers.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.BtnAddMoreOffers.Location = new System.Drawing.Point(682, 1);
		this.BtnAddMoreOffers.Margin = new System.Windows.Forms.Padding(2);
		this.BtnAddMoreOffers.Name = "BtnAddMoreOffers";
		this.BtnAddMoreOffers.Size = new System.Drawing.Size(120, 22);
		this.BtnAddMoreOffers.TabIndex = 0;
		this.BtnAddMoreOffers.Text = "Add More Offer";
		this.BtnAddMoreOffers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnAddMoreOffers.UseVisualStyleBackColor = false;
		this.PanelHeaderInfo.BackColor = System.Drawing.Color.White;
		this.PanelHeaderInfo.Controls.Add(this.label39);
		this.PanelHeaderInfo.Controls.Add(this.label12);
		this.PanelHeaderInfo.Controls.Add(this.textBox9);
		this.PanelHeaderInfo.Controls.Add(this.label24);
		this.PanelHeaderInfo.Controls.Add(this.textBox8);
		this.PanelHeaderInfo.Controls.Add(this.label22);
		this.PanelHeaderInfo.Controls.Add(this.label21);
		this.PanelHeaderInfo.Controls.Add(this.txt);
		this.PanelHeaderInfo.Controls.Add(this.label15);
		this.PanelHeaderInfo.Controls.Add(this.label23);
		this.PanelHeaderInfo.Controls.Add(this.label28);
		this.PanelHeaderInfo.Controls.Add(this.label29);
		this.PanelHeaderInfo.Controls.Add(this.label30);
		this.PanelHeaderInfo.Controls.Add(this.label31);
		this.PanelHeaderInfo.Controls.Add(this.txtBrokerOrAgent);
		this.PanelHeaderInfo.Controls.Add(this.label2);
		this.PanelHeaderInfo.Controls.Add(this.label4);
		this.PanelHeaderInfo.Controls.Add(this.label18);
		this.PanelHeaderInfo.Controls.Add(this.label20);
		this.PanelHeaderInfo.Controls.Add(this.label25);
		this.PanelHeaderInfo.Controls.Add(this.label26);
		this.PanelHeaderInfo.Controls.Add(this.label35);
		this.PanelHeaderInfo.Controls.Add(this.label34);
		this.PanelHeaderInfo.Controls.Add(this.txtExpiryDate);
		this.PanelHeaderInfo.Controls.Add(this.txtDeliveryStartDate);
		this.PanelHeaderInfo.Controls.Add(this.txtDeliveryDays);
		this.PanelHeaderInfo.Controls.Add(this.txtDeliveryTerm);
		this.PanelHeaderInfo.Controls.Add(this.txtDueDays);
		this.PanelHeaderInfo.Controls.Add(this.label38);
		this.PanelHeaderInfo.Controls.Add(this.label37);
		this.PanelHeaderInfo.Controls.Add(this.label36);
		this.PanelHeaderInfo.Controls.Add(this.txtPaymentTerm);
		this.PanelHeaderInfo.Controls.Add(this.label9);
		this.PanelHeaderInfo.Controls.Add(this.label6);
		this.PanelHeaderInfo.Controls.Add(this.label8);
		this.PanelHeaderInfo.Controls.Add(this.txtDocNo);
		this.PanelHeaderInfo.Controls.Add(this.label11);
		this.PanelHeaderInfo.Controls.Add(this.txtValidityDays);
		this.PanelHeaderInfo.Controls.Add(this.txtValidityUptoDate);
		this.PanelHeaderInfo.Controls.Add(this.txtDocDate);
		this.PanelHeaderInfo.Controls.Add(this.label3);
		this.PanelHeaderInfo.Controls.Add(this.label16);
		this.PanelHeaderInfo.Controls.Add(this.label13);
		this.PanelHeaderInfo.Controls.Add(this.Line01);
		this.PanelHeaderInfo.Controls.Add(this.txtRemarks);
		this.PanelHeaderInfo.Controls.Add(this.label33);
		this.PanelHeaderInfo.Controls.Add(this.label32);
		this.PanelHeaderInfo.Controls.Add(this.txtBuyerName);
		this.PanelHeaderInfo.Dock = System.Windows.Forms.DockStyle.Top;
		this.PanelHeaderInfo.Location = new System.Drawing.Point(0, 31);
		this.PanelHeaderInfo.Name = "PanelHeaderInfo";
		this.PanelHeaderInfo.Size = new System.Drawing.Size(804, 328);
		this.PanelHeaderInfo.TabIndex = 1;
		this.label39.BackColor = System.Drawing.Color.DarkGray;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label39.Location = new System.Drawing.Point(119, 147);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(667, 1);
		this.label39.TabIndex = 230;
		this.label39.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label12.ForeColor = System.Drawing.Color.Teal;
		this.label12.Location = new System.Drawing.Point(509, 300);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(123, 15);
		this.label12.TabIndex = 229;
		this.label12.Text = "Supplier Target Price";
		this.textBox9.BackColor = System.Drawing.Color.White;
		this.textBox9.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.textBox9.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox9.Location = new System.Drawing.Point(660, 296);
		this.textBox9.Name = "textBox9";
		this.textBox9.Size = new System.Drawing.Size(130, 16);
		this.textBox9.TabIndex = 219;
		this.textBox9.Text = "0";
		this.textBox9.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label24.ForeColor = System.Drawing.Color.Teal;
		this.label24.Location = new System.Drawing.Point(509, 276);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(110, 15);
		this.label24.TabIndex = 228;
		this.label24.Text = "Buyer Target Price";
		this.textBox8.BackColor = System.Drawing.Color.White;
		this.textBox8.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.textBox8.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox8.Location = new System.Drawing.Point(660, 272);
		this.textBox8.Name = "textBox8";
		this.textBox8.Size = new System.Drawing.Size(130, 16);
		this.textBox8.TabIndex = 218;
		this.textBox8.Text = "0";
		this.textBox8.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label22.ForeColor = System.Drawing.Color.Teal;
		this.label22.Location = new System.Drawing.Point(509, 193);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(121, 15);
		this.label22.TabIndex = 221;
		this.label22.Text = "Quality Specification";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label21.ForeColor = System.Drawing.Color.Teal;
		this.label21.Location = new System.Drawing.Point(194, 244);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(63, 15);
		this.label21.TabIndex = 227;
		this.label21.Text = "Pack Uom";
		this.txt.BackColor = System.Drawing.Color.White;
		this.txt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txt.Location = new System.Drawing.Point(509, 213);
		this.txt.Multiline = true;
		this.txt.Name = "txt";
		this.txt.Size = new System.Drawing.Size(281, 58);
		this.txt.TabIndex = 217;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label15.ForeColor = System.Drawing.Color.Teal;
		this.label15.Location = new System.Drawing.Point(194, 266);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(60, 15);
		this.label15.TabIndex = 226;
		this.label15.Text = "Crop Year";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label23.ForeColor = System.Drawing.Color.Teal;
		this.label23.Location = new System.Drawing.Point(11, 266);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(48, 15);
		this.label23.TabIndex = 224;
		this.label23.Text = "Weight";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label28.ForeColor = System.Drawing.Color.Teal;
		this.label28.Location = new System.Drawing.Point(11, 244);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(27, 15);
		this.label28.TabIndex = 222;
		this.label28.Text = "Qty";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label29.ForeColor = System.Drawing.Color.Teal;
		this.label29.Location = new System.Drawing.Point(11, 220);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(70, 15);
		this.label29.TabIndex = 223;
		this.label29.Text = "Item Name";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label30.ForeColor = System.Drawing.Color.Teal;
		this.label30.Location = new System.Drawing.Point(11, 194);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(74, 15);
		this.label30.TabIndex = 220;
		this.label30.Text = "Parent Item";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label31.ForeColor = System.Drawing.Color.Teal;
		this.label31.Location = new System.Drawing.Point(11, 288);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(79, 15);
		this.label31.TabIndex = 225;
		this.label31.Text = "Packing Type";
		this.txtBrokerOrAgent.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.txtBrokerOrAgent.Location = new System.Drawing.Point(115, 62);
		this.txtBrokerOrAgent.Name = "txtBrokerOrAgent";
		this.txtBrokerOrAgent.Size = new System.Drawing.Size(313, 30);
		this.txtBrokerOrAgent.TabIndex = 184;
		this.txtBrokerOrAgent.Text = "None";
		this.txtBrokerOrAgent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.Teal;
		this.label2.Location = new System.Drawing.Point(535, 113);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(71, 15);
		this.label2.TabIndex = 209;
		this.label2.Text = "Expiry Date";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label4.ForeColor = System.Drawing.Color.Teal;
		this.label4.Location = new System.Drawing.Point(535, 92);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(115, 15);
		this.label4.TabIndex = 208;
		this.label4.Text = "Delivery Start Date";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label18.ForeColor = System.Drawing.Color.Teal;
		this.label18.Location = new System.Drawing.Point(535, 71);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(83, 15);
		this.label18.TabIndex = 207;
		this.label18.Text = "Delivery Days";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label20.ForeColor = System.Drawing.Color.Teal;
		this.label20.Location = new System.Drawing.Point(535, 50);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(86, 15);
		this.label20.TabIndex = 206;
		this.label20.Text = "Delivery Term";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.label25.ForeColor = System.Drawing.Color.Teal;
		this.label25.Location = new System.Drawing.Point(199, 36);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(99, 19);
		this.label25.TabIndex = 197;
		this.label25.Text = "Validity UpTo";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.label26.ForeColor = System.Drawing.Color.Teal;
		this.label26.Location = new System.Drawing.Point(5, 36);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(96, 19);
		this.label26.TabIndex = 200;
		this.label26.Text = "Validity Days";
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label35.Location = new System.Drawing.Point(91, 263);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(100, 20);
		this.label35.TabIndex = 188;
		this.label35.Text = "0";
		this.label35.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label34.Location = new System.Drawing.Point(91, 241);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(100, 20);
		this.label34.TabIndex = 188;
		this.label34.Text = "0";
		this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtExpiryDate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.txtExpiryDate.Location = new System.Drawing.Point(650, 110);
		this.txtExpiryDate.Name = "txtExpiryDate";
		this.txtExpiryDate.Size = new System.Drawing.Size(140, 20);
		this.txtExpiryDate.TabIndex = 188;
		this.txtExpiryDate.Text = "0";
		this.txtExpiryDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtDeliveryStartDate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.txtDeliveryStartDate.Location = new System.Drawing.Point(650, 89);
		this.txtDeliveryStartDate.Name = "txtDeliveryStartDate";
		this.txtDeliveryStartDate.Size = new System.Drawing.Size(140, 20);
		this.txtDeliveryStartDate.TabIndex = 188;
		this.txtDeliveryStartDate.Text = "0";
		this.txtDeliveryStartDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtDeliveryDays.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.txtDeliveryDays.Location = new System.Drawing.Point(650, 68);
		this.txtDeliveryDays.Name = "txtDeliveryDays";
		this.txtDeliveryDays.Size = new System.Drawing.Size(140, 20);
		this.txtDeliveryDays.TabIndex = 188;
		this.txtDeliveryDays.Text = "0";
		this.txtDeliveryDays.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtDeliveryTerm.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.txtDeliveryTerm.Location = new System.Drawing.Point(650, 47);
		this.txtDeliveryTerm.Name = "txtDeliveryTerm";
		this.txtDeliveryTerm.Size = new System.Drawing.Size(140, 20);
		this.txtDeliveryTerm.TabIndex = 188;
		this.txtDeliveryTerm.Text = "0";
		this.txtDeliveryTerm.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.txtDueDays.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.txtDueDays.Location = new System.Drawing.Point(650, 26);
		this.txtDueDays.Name = "txtDueDays";
		this.txtDueDays.Size = new System.Drawing.Size(140, 20);
		this.txtDueDays.TabIndex = 188;
		this.txtDueDays.Text = "0";
		this.txtDueDays.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label38.Location = new System.Drawing.Point(91, 285);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(267, 20);
		this.label38.TabIndex = 188;
		this.label38.Text = "None";
		this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label37.Location = new System.Drawing.Point(258, 263);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(100, 20);
		this.label37.TabIndex = 188;
		this.label37.Text = "None";
		this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label36.Location = new System.Drawing.Point(258, 241);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(100, 20);
		this.label36.TabIndex = 188;
		this.label36.Text = "None";
		this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.txtPaymentTerm.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.txtPaymentTerm.Location = new System.Drawing.Point(650, 5);
		this.txtPaymentTerm.Name = "txtPaymentTerm";
		this.txtPaymentTerm.Size = new System.Drawing.Size(140, 20);
		this.txtPaymentTerm.TabIndex = 188;
		this.txtPaymentTerm.Text = "None";
		this.txtPaymentTerm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label9.ForeColor = System.Drawing.Color.Teal;
		this.label9.Location = new System.Drawing.Point(535, 29);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(59, 15);
		this.label9.TabIndex = 188;
		this.label9.Text = "Due Days";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Calibri", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.Teal;
		this.label6.Location = new System.Drawing.Point(5, 2);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(103, 26);
		this.label6.TabIndex = 6;
		this.label6.Text = "Inquiry No";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label8.ForeColor = System.Drawing.Color.Teal;
		this.label8.Location = new System.Drawing.Point(535, 8);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(88, 15);
		this.label8.TabIndex = 188;
		this.label8.Text = "Payment Term";
		this.txtDocNo.Cursor = System.Windows.Forms.Cursors.Hand;
		this.txtDocNo.Font = new System.Drawing.Font("Calibri", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNo.ForeColor = System.Drawing.Color.Blue;
		this.txtDocNo.Location = new System.Drawing.Point(115, 2);
		this.txtDocNo.Name = "txtDocNo";
		this.txtDocNo.Size = new System.Drawing.Size(80, 26);
		this.txtDocNo.TabIndex = 7;
		this.txtDocNo.Text = "0";
		this.txtDocNo.Click += new System.EventHandler(txtDocNo_Click);
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold);
		this.label11.ForeColor = System.Drawing.Color.Teal;
		this.label11.Location = new System.Drawing.Point(5, 68);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(111, 19);
		this.label11.TabIndex = 8;
		this.label11.Text = "Agent / Broker";
		this.txtValidityUptoDate.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtValidityUptoDate.ForeColor = System.Drawing.Color.Black;
		this.txtValidityUptoDate.Location = new System.Drawing.Point(298, 35);
		this.txtValidityUptoDate.Name = "txtValidityUptoDate";
		this.txtValidityUptoDate.Size = new System.Drawing.Size(130, 20);
		this.txtValidityUptoDate.TabIndex = 9;
		this.txtValidityUptoDate.Text = "None";
		this.txtDocDate.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocDate.ForeColor = System.Drawing.Color.Black;
		this.txtDocDate.Location = new System.Drawing.Point(298, 5);
		this.txtDocDate.Name = "txtDocDate";
		this.txtDocDate.Size = new System.Drawing.Size(130, 20);
		this.txtDocDate.TabIndex = 9;
		this.txtDocDate.Text = "None";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.Teal;
		this.label3.Location = new System.Drawing.Point(199, 6);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(102, 19);
		this.label3.TabIndex = 8;
		this.label3.Text = "Inquiry Date: ";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.Teal;
		this.label16.Location = new System.Drawing.Point(5, 135);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(67, 19);
		this.label16.TabIndex = 8;
		this.label16.Text = "Remarks";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.Teal;
		this.label13.Location = new System.Drawing.Point(5, 102);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(93, 19);
		this.label13.TabIndex = 8;
		this.label13.Text = "Buyer Name";
		this.Line01.BackColor = System.Drawing.Color.FromArgb(0, 192, 192);
		this.Line01.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.Line01.Location = new System.Drawing.Point(9, 183);
		this.Line01.Name = "Line01";
		this.Line01.Size = new System.Drawing.Size(780, 2);
		this.Line01.TabIndex = 184;
		this.Line01.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.txtRemarks.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.txtRemarks.Location = new System.Drawing.Point(115, 132);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(675, 46);
		this.txtRemarks.TabIndex = 184;
		this.txtRemarks.Text = "None";
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label33.Location = new System.Drawing.Point(91, 215);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(267, 24);
		this.label33.TabIndex = 184;
		this.label33.Text = "None";
		this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label32.Location = new System.Drawing.Point(91, 189);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(267, 24);
		this.label32.TabIndex = 184;
		this.label32.Text = "None";
		this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.txtBuyerName.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.txtBuyerName.Location = new System.Drawing.Point(115, 96);
		this.txtBuyerName.Name = "txtBuyerName";
		this.txtBuyerName.Size = new System.Drawing.Size(313, 30);
		this.txtBuyerName.TabIndex = 184;
		this.txtBuyerName.Text = "None";
		this.txtBuyerName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.PanelMainSelectedNetPreviousDocData.BackColor = System.Drawing.Color.White;
		this.PanelMainSelectedNetPreviousDocData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.PanelMainSelectedNetPreviousDocData.Controls.Add(this.BtnShowPreviousRecord);
		this.PanelMainSelectedNetPreviousDocData.Controls.Add(this.BtnApproveThis);
		this.PanelMainSelectedNetPreviousDocData.Controls.Add(this.BtnShowNextRecord);
		this.PanelMainSelectedNetPreviousDocData.Controls.Add(this.txtCompanyName);
		this.PanelMainSelectedNetPreviousDocData.Dock = System.Windows.Forms.DockStyle.Top;
		this.PanelMainSelectedNetPreviousDocData.Location = new System.Drawing.Point(0, 0);
		this.PanelMainSelectedNetPreviousDocData.Name = "PanelMainSelectedNetPreviousDocData";
		this.PanelMainSelectedNetPreviousDocData.Size = new System.Drawing.Size(804, 31);
		this.PanelMainSelectedNetPreviousDocData.TabIndex = 0;
		this.BtnShowPreviousRecord.BackColor = System.Drawing.Color.Transparent;
		this.BtnShowPreviousRecord.FlatAppearance.BorderSize = 0;
		this.BtnShowPreviousRecord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShowPreviousRecord.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnShowPreviousRecord.ForeColor = System.Drawing.Color.White;
		this.BtnShowPreviousRecord.Image = Architecture.WinApp.Properties.Resources.arrow_left_line;
		this.BtnShowPreviousRecord.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.BtnShowPreviousRecord.Location = new System.Drawing.Point(6, 6);
		this.BtnShowPreviousRecord.Margin = new System.Windows.Forms.Padding(2);
		this.BtnShowPreviousRecord.Name = "BtnShowPreviousRecord";
		this.BtnShowPreviousRecord.Size = new System.Drawing.Size(27, 19);
		this.BtnShowPreviousRecord.TabIndex = 8;
		this.BtnShowPreviousRecord.UseVisualStyleBackColor = false;
		this.BtnShowPreviousRecord.Click += new System.EventHandler(BtnShowPreviousRecord_Click);
		this.BtnApproveThis.BackColor = System.Drawing.Color.SeaGreen;
		this.BtnApproveThis.FlatAppearance.BorderColor = System.Drawing.Color.Black;
		this.BtnApproveThis.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnApproveThis.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnApproveThis.ForeColor = System.Drawing.Color.White;
		this.BtnApproveThis.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.BtnApproveThis.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.BtnApproveThis.Location = new System.Drawing.Point(77, 3);
		this.BtnApproveThis.Margin = new System.Windows.Forms.Padding(2);
		this.BtnApproveThis.Name = "BtnApproveThis";
		this.BtnApproveThis.Size = new System.Drawing.Size(101, 25);
		this.BtnApproveThis.TabIndex = 0;
		this.BtnApproveThis.Text = "Map This";
		this.BtnApproveThis.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnApproveThis.UseVisualStyleBackColor = false;
		this.BtnShowNextRecord.BackColor = System.Drawing.Color.Transparent;
		this.BtnShowNextRecord.FlatAppearance.BorderSize = 0;
		this.BtnShowNextRecord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShowNextRecord.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnShowNextRecord.ForeColor = System.Drawing.Color.White;
		this.BtnShowNextRecord.Image = Architecture.WinApp.Properties.Resources.arrow_right_line1;
		this.BtnShowNextRecord.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnShowNextRecord.Location = new System.Drawing.Point(39, 6);
		this.BtnShowNextRecord.Margin = new System.Windows.Forms.Padding(2);
		this.BtnShowNextRecord.Name = "BtnShowNextRecord";
		this.BtnShowNextRecord.Size = new System.Drawing.Size(27, 19);
		this.BtnShowNextRecord.TabIndex = 8;
		this.BtnShowNextRecord.UseVisualStyleBackColor = false;
		this.BtnShowNextRecord.Click += new System.EventHandler(BtnShowNextRecord_Click);
		this.txtCompanyName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCompanyName.Font = new System.Drawing.Font("Verdana", 10f, System.Drawing.FontStyle.Bold);
		this.txtCompanyName.Location = new System.Drawing.Point(481, 1);
		this.txtCompanyName.Name = "txtCompanyName";
		this.txtCompanyName.Size = new System.Drawing.Size(318, 25);
		this.txtCompanyName.TabIndex = 184;
		this.txtCompanyName.Text = "None";
		this.txtCompanyName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtValidityDays.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtValidityDays.ForeColor = System.Drawing.Color.Black;
		this.txtValidityDays.Location = new System.Drawing.Point(115, 35);
		this.txtValidityDays.Name = "txtValidityDays";
		this.txtValidityDays.Size = new System.Drawing.Size(80, 20);
		this.txtValidityDays.TabIndex = 9;
		this.txtValidityDays.Text = "0";
		this.txtValidityDays.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1184, 711);
		base.Controls.Add(this.PanelMainSelectedDocData);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.panel3);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmSupplierOfferMappingToInquiry";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmSupplierOfferMappingToInquiry_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmPurchaseInvoiceHistory_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.GBFilters.ResumeLayout(false);
		this.GBFilters.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbServiceProvider).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokerAgentName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.TotalRecordPanel.ResumeLayout(false);
		this.TotalRecordPanel.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.PanelMainSelectedDocData.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSupplierOffers).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.PanelHeaderInfo.ResumeLayout(false);
		this.PanelHeaderInfo.PerformLayout();
		this.PanelMainSelectedNetPreviousDocData.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}

}
